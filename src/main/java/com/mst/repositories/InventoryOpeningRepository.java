package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryOpeningRequest.History;
import java.sql.Date;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/** Read contracts from Architecture.BLL.PurchaseTrading.InvStockOpeningBalanceHeader. */
@Repository
public class InventoryOpeningRepository {
    private static final org.slf4j.Logger LOG = org.slf4j.LoggerFactory.getLogger(InventoryOpeningRepository.class);
    private final JdbcTemplate jdbc;
    public InventoryOpeningRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    public Map<String,Object> lookups(UserAccount u) {
        Map<String,Object> r=new LinkedHashMap<>();
        r.put("projects",jdbc.queryForList("EXEC dbo.Sp_Projects_GetAllMethod @OrganizationId=?, @CompanyId=?, @MethodType=?",u.getOrganizationId(),u.getCompanyId(),"GetAll"));
        r.put("warehouses",scoped(u,"Sp_InvWareHouse_GetAllMethod","GetActiveWareHouse"));
        r.put("items",items(u));
        r.put("packingTypes",jdbc.queryForList("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity=?","ReadAll"));
        r.put("crops",scoped(u,"Sp_InvCropYear_GetAllMethod","ReadAll"));
        r.put("lots",scoped(u,"SP_JobLot_ReadMethod","GetAll"));
        r.put("accounts",jdbc.queryForList("EXEC dbo.USP_Accounts_GetAccountTitleByAccountTypeIds @OrganizationId=?, @CompanyId=?, @AppId=?, @UserId=?, @AccountTypeIds=?",u.getOrganizationId(),u.getCompanyId(),u.getAppId(),u.getId(),"3,4,6,8,9"));
        r.put("historyChoices",historyChoices(u));
        r.put("financialYears",years(u));
        return r;
    }
    public List<Map<String,Object>> years(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
    }
    public List<Map<String,Object>> historyChoices(UserAccount u) {
        return jdbc.queryForList("EXEC dbo.USP_GetDataForDropDownFromInvStockOpeningBalanceHeader @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?",u.getOrganizationId(),u.getCompanyId(),40);
    }
    private List<Map<String,Object>> scoped(UserAccount u,String proc,String activity) {
        return jdbc.queryForList("EXEC dbo."+proc+" @OrganizationId=?, @CompanyId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),activity);
    }
    public List<Map<String,Object>> items(UserAccount u) {
        // RPC matches SqlCommand.CommandType.StoredProcedure; the first three procedure
        // parameters are OrganizationId, CompanyId, Activity (captured DB signature).
        return jdbc.execute("{call dbo.Sp_Item_GetAllMethod(?,?,?)}",(org.springframework.jdbc.core.CallableStatementCallback<List<Map<String,Object>>>) statement->{
            statement.setInt(1,u.getOrganizationId());statement.setInt(2,u.getCompanyId());statement.setNString(3,"ReadAllItems");
            try(var rs=statement.executeQuery()){return new org.springframework.jdbc.core.RowMapperResultSetExtractor<Map<String,Object>>(new org.springframework.jdbc.core.ColumnMapRowMapper()).extractData(rs);}
        });
    }
    public List<Map<String,Object>> uoms(UserAccount u,int item) {
        if (item <= 0) {
            try {
                return jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity=?", u.getOrganizationId(), u.getCompanyId(), "ReadByOrganizationCompanyId");
            } catch (Exception e) { LOG.warn("Sp_UOMSchedule_GetAllMethod failed", e); }
        }
        return jdbc.queryForList("EXEC dbo.Sp_UOMSchedule_GetAllMethod @OrganizationId=?, @CompanyId=?, @ItemId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),item,"ReadByItemID");
    }
    public int nextCode(UserAccount u,int year) {
        List<Map<String,Object>> rows=jdbc.queryForList("EXEC dbo.Sp_InvStockOpeningBalanceHeader_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),40,year,"GenerateCode");
        if(rows.isEmpty())throw new IllegalStateException("Document number could not be generated");
        return ((Number)rows.get(0).get("DocNo")).intValue();
    }
    public List<Map<String,Object>> history(UserAccount u,int year,History h) {
        StringBuilder q=new StringBuilder("EXEC dbo.Sp_InvStockOpeningBalanceHeader_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @FinancialYearId=?, @Activity=?");
        List<Object> p=new ArrayList<>(List.of(u.getOrganizationId(),u.getCompanyId(),40,year,"ReadAll"));
        add(q,p,"FromDate",h.fromDate==null?null:Date.valueOf(h.fromDate));add(q,p,"ToDate",h.toDate==null?null:Date.valueOf(h.toDate));
        add(q,p,"FromDocNo",h.fromDocNo);add(q,p,"ToDocNo",h.toDocNo);add(q,p,"AccountId",h.accountId);add(q,p,"ItemId",h.itemId);add(q,p,"ItemStockAccountId",h.itemStockAccountId);
        // The desktop BLL ignores BranchesId, EntryUser and CanViewAllRecord here.
        return jdbc.queryForList(q.toString(),p.toArray());
    }
    public Map<String,Object> record(UserAccount u,int id) {
        List<Map<String,Object>> rows=jdbc.queryForList("EXEC dbo.Sp_InvStockOpeningBalanceHeader_GetAllMethod @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?, @Activity=?",u.getOrganizationId(),u.getCompanyId(),40,id,"ReadAll");
        if(rows.size()!=1)throw new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND,"Opening stock record not found in this company");
        return rows.get(0);
    }
    public List<Map<String,Object>> attachments(UserAccount u,int id) {
        record(u,id);
        return jdbc.queryForList("EXEC dbo.Sp_DMSAttachments_GetAllMethod @ScreenName=?, @Id=?, @Activity=?","frmOpeningStockBlancing",id,"ReadById").stream()
            .filter(row->Objects.equals(row.get("OrganizationId"),u.getOrganizationId())&&Objects.equals(row.get("CompanyId"),u.getCompanyId()))
            .map(row->{Map<String,Object> visible=new LinkedHashMap<>();for(String key:List.of("Id","UploadedFileCustomName","Attachment","EntryDate","EntryUserName","UploadedFileSizeMb"))visible.put(key,row.get(key));return visible;}).toList();
    }
    private static void add(StringBuilder q,List<Object> p,String name,Object value) {
        if(value==null || value instanceof Number && ((Number)value).intValue()==0)return;
        q.append(", @").append(name).append("=?");p.add(value);
    }
}
