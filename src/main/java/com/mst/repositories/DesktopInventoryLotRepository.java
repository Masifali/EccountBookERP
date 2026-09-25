package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryLotRequest;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;

@Repository
public class DesktopInventoryLotRepository {
    private final JdbcTemplate jdbc;
    public DesktopInventoryLotRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String,Object>> history(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @OrganizationId=?, @CompanyId=?, @Activity=?", u.getOrganizationId(), u.getCompanyId(), "GetAll");
    }

    public Map<String,Object> record(UserAccount u, int id) {
        var rows = jdbc.queryForList("EXEC dbo.SP_JobLot_ReadMethod @Id=?, @Activity=?", id, "GetById");
        if (rows.size()!=1 || !Objects.equals(rows.get(0).get("OrganizationId"),u.getOrganizationId()) || !Objects.equals(rows.get(0).get("CompanyId"),u.getCompanyId()))
            throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Job lot not found in this company");
        return rows.get(0);
    }

    public Map<String,Object> lookups(UserAccount u) {
        var result = new LinkedHashMap<String,Object>();
        var config = jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"AllowCoaAccountForJobLots","GetConfigurationByOrgCompandConfigDescription");
        boolean accounts = !config.isEmpty() && DesktopInventoryCategoryRepository.toBool(config.get(0).get("ConfigKey"));
        result.put("accountsVisible",accounts);
        result.put("partyProcessing",new DesktopInventoryCategoryRepository(jdbc).feature(u,8));
        result.put("accounts",accounts ? /* AcfrmDefineLots.cs:240 -> CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4") -> Sp_COAAllocation_GetAllMethod GetAccountTitleByAccountTypeIds */ jdbc.queryForList("EXEC dbo.Sp_COAAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @AccountTypeIds=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),u.getAppId(),u.getId(),"4","GetAccountTitleByAccountTypeIds") : List.of());
        result.put("jobTypes",jdbc.queryForList("EXEC dbo.Sp_InvLookup_GetAllMethod @OrganizationId=?, @CompanyId=?, @InvLookupTypeId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),4,"ReadByInvlookTypeId"));
        result.put("documentTypes",jdbc.queryForList("EXEC dbo.usp_getDocumentsForJobLot"));
        result.put("branches",branches(u));
        result.put("now",LocalDateTime.now().withNano(0).toString());
        return result;
    }

    public List<Map<String,Object>> branches(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Sp_Branches_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),"GetAll");
    }

    public List<Map<String,Object>> references(UserAccount u,int documentType,int recordId) {
        if (!Set.of(401,403).contains(documentType)) throw new IllegalArgumentException("Select a production document type");
        if(recordId>0)record(u,recordId);
        // The legacy dropdown procedure is global. Intersect its results with the signed-in company.
        var owned = new HashSet<>(jdbc.queryForList("SELECT Id FROM dbo.InvProductionJobOrder WHERE OrganizationId=? AND CompanyId=? AND DocumentTypeId=?",Integer.class,u.getOrganizationId(),u.getCompanyId(),documentType));
        return jdbc.queryForList("EXEC dbo.usp_getReferenceNoByDocumentTypeIdFromJobLot @DocumentTypeId=?, @RecId=?",documentType,recordId).stream().filter(r->owned.contains(number(r.get("Id")))).toList();
    }

    public Map<String,Object> save(UserAccount u,InventoryLotRequest r) {
        if(r.id>0)record(u,r.id);
        var now = Timestamp.valueOf(LocalDateTime.now());
        Object[] values={r.id,r.code.trim(),r.description.trim(),Timestamp.valueOf(r.startDate),Timestamp.valueOf(r.endDate),false,r.status,now,u.getId(),now,0,now,u.getId(),false,u.getOrganizationId(),u.getCompanyId(),r.jobTypeId,r.company,r.thirdParty,r.accountId,r.refDocumentTypeId,r.refDocNoId};
        int id = execute("EXEC dbo.Proc_JobLot_"+(r.id==0?"Insert":"Update")+" @Id=?, @JobLotCode=?, @JobLotDescription=?, @StartDate=?, @EndDate=?, @IsApproved=?, @JobStatus=?, @EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @PostDate=?, @PostUser=?, @PostState=?, @OrganizationId=?, @CompanyId=?, @JobTypeId=?, @IsCompany=?, @IsThirdParty=?, @AccountId=?, @RefDocumentTypeId=?, @RefDocNoId=?",values,r.id);
        if(id<=0)throw new IllegalStateException("Job lot save returned no ID");
        if(r.id==0) {
            // Legacy Insert auto-allocates across ALL companies when feature11 is off.
            // Restrict only allocations created for this new lot, in the same transaction.
            // Existing allocations and the database procedure are never changed.
            jdbc.update("DELETE a FROM dbo.JobLotsAllocationToBranch a WHERE a.JobLotId=? AND a.OrganizationId=? AND a.CompanyId=? AND NOT EXISTS (SELECT 1 FROM dbo.Branches b WHERE b.Id=a.BranchId AND b.OrganizationId=a.OrganizationId AND b.CompanyId=a.CompanyId)",id,u.getOrganizationId(),u.getCompanyId());
        }
        return record(u,id);
    }

    public Map<String,Object> allocations(UserAccount u,int branch) {
        requireBranch(u,branch);
        return Map.of("unallocated",allocationRows(u,branch,1),"allocated",allocationRows(u,branch,2));
    }

    private List<Map<String,Object>> allocationRows(UserAccount u,int branch,int action) {
        return jdbc.queryForList("EXEC dbo.USP_JobLotsAllocatedOrUnAllocatedToBranch @OrganizationId=?, @CompanyId=?, @BranchId=?, @ActionId=?",u.getOrganizationId(),u.getCompanyId(),branch,action);
    }

    public void allocate(UserAccount u,InventoryLotRequest.Allocation r) {
        requireBranch(u,r.branchId);
        var available = new HashSet<Integer>();
        allocationRows(u,r.branchId,r.allocate?1:2).forEach(row->available.add(number(row.get("JobLotId"))));
        for(int id:r.lotIds) {
            record(u,id);
            if(!available.contains(id))throw new IllegalArgumentException("The selected allocation changed. Refresh the branch and select the rows again.");
        }
        if(r.allocate) {
            var now=Timestamp.valueOf(LocalDateTime.now());
            for(int id:new LinkedHashSet<>(r.lotIds))execute("EXEC dbo.USP_JobLotsAllocationToBranch_Insert @Id=?, @JobLotId=?, @BranchId=?, @EntryDate=?, @EntryUserId=?, @ModifyDate=?, @ModifyUserId=?, @OrganizationId=?, @CompanyId=?, @FinancialYearId=?",new Object[]{0,id,r.branchId,now,u.getId(),now,u.getId(),u.getOrganizationId(),u.getCompanyId(),0},0);
        } else {
            // Guard the legacy delete procedure, which accepts only branch and lot IDs.
            for(int id:r.lotIds)if(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.JobLotsAllocationToBranch WHERE BranchId=? AND JobLotId=? AND (OrganizationId<>? OR CompanyId<>?)",Integer.class,r.branchId,id,u.getOrganizationId(),u.getCompanyId())>0)
                throw new IllegalArgumentException("This allocation contains inconsistent company ownership and cannot be removed here");
            execute("EXEC dbo.USP_JobLotsAllocationToBranchDeleteById @BranchId=?, @JobLotIds=?",new Object[]{r.branchId,String.join(",",r.lotIds.stream().distinct().map(String::valueOf).toList())},0);
        }
    }

    private void requireBranch(UserAccount u,int id) {
        if(branches(u).stream().noneMatch(r->number(r.get("Id"))==id))throw new IllegalArgumentException("Select a branch from this company");
    }

    private int execute(String sql,Object[] values,int fallback) {
        return jdbc.execute(sql,(PreparedStatementCallback<Integer>) statement->{
            for(int i=0;i<values.length;i++)statement.setObject(i+1,values[i]);
            int saved=fallback;boolean result=statement.execute();
            while(true){if(result){try(var rs=statement.getResultSet()){while(rs.next())if(rs.getObject(1) instanceof Number n && n.intValue()>0)saved=n.intValue();}}else if(statement.getUpdateCount()==-1)break;result=statement.getMoreResults();}
            return saved;
        });
    }
    private static int number(Object n) {return n==null?0:Integer.parseInt(n.toString());}
}
