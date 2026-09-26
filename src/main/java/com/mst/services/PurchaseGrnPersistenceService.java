package com.mst.services;

import com.mst.models.dto.PurchaseGrnDto;
import com.mst.repositories.PurchaseGrnRecordRepository;
import com.mst.repositories.PurchaseGrnWriteRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import static com.mst.repositories.PurchaseGrnWriteRepository.number;

/** InvGrn DAL SetData sequence for Purchase GRN (46) and Sale Return GRN (143). */
@Service
public class PurchaseGrnPersistenceService {
    private final PurchaseGrnRecordRepository records;
    private final PurchaseGrnWriteRepository writes;
    private final CurrentUserContext context;
    private final JdbcTemplate jdbc;
    private final PurchaseGrnSupplementService supplements;
    private final PurchaseGrnSaveRules saveRules;
    public PurchaseGrnPersistenceService(PurchaseGrnRecordRepository records,PurchaseGrnWriteRepository writes,CurrentUserContext context,JdbcTemplate jdbc,PurchaseGrnSupplementService supplements,PurchaseGrnSaveRules saveRules) {
        this.records=records; this.writes=writes; this.context=context; this.jdbc=jdbc; this.supplements=supplements; this.saveRules=saveRules;
    }

    @Transactional
    public Map<String,Object> save(Map<String,Object> payload,int type) {
        if(type!=46 && type!=143)throw new IllegalArgumentException("Unsupported GRN type");
        var dto=new PurchaseGrnDto(payload); int id=number(dto.header.get("Id")); boolean updating=id>0;
        records.requireRight(type,updating?"Update":"Save");
        Map<String,Object> existing=updating?records.require(id,type):null;
        var header=writes.headerValues(existing,dto.header);
        int org=context.currentOrganizationId(),company=context.currentCompanyId(),branch=context.currentBranchId(),year=context.currentFinancialYearId(),user=context.currentUserId();
        header.put("DocumentTypeId",type); header.put("OrganizationId",org); header.put("CompanyId",company);
        header.put("BranchesId",branch); header.put("FinancialYearId",year);
        header.put("EntryUser",updating?existing.get("EntryUser"):user);
        header.put("EntryDate",updating?existing.get("EntryDate"):new java.sql.Timestamp(System.currentTimeMillis()));
        header.put("ModifyUser",updating?user:0); header.put("ModifyDate",new java.sql.Timestamp(System.currentTimeMillis()));
        // Approval and attachment changes have separate desktop workflows; a generic save cannot overwrite them.
        for(String field:List.of("IsApproved","PostUser","PostDate","AttachmentsValues","CustomAttachmentsValues"))
            header.put(field,updating?existing.get(field):("IsApproved".equals(field)?false:"PostUser".equals(field)?0:null));
        header.put("ScreenName",type==46?"InvFrmGRN":"SaleReturnGrn");
        if(!updating && !dto.header.containsKey("DocDate"))header.put("DocDate",java.sql.Date.valueOf(java.time.LocalDate.now()));
        int gp=number(header.get("InwardGatePassId"));
        if(gp>0 && jdbc.queryForObject("SELECT COUNT(*) FROM dbo.GatePassInward WHERE Id=? AND OrganizationId=? AND CompanyId=? AND BranchesId=? AND FinancialYearId=? AND DocumentTypeId=51",Integer.class,gp,org,company,branch,year)!=1)
            throw new IllegalArgumentException("The gate pass is outside the current company, branch or financial year");

        var storedDetails=updating?writes.details(id):List.<Map<String,Object>>of();
        var details=PurchaseGrnGridRules.copy(dto.details==null?storedDetails:dto.details);
        if(details.isEmpty())throw new IllegalArgumentException("Detail list not found");
        var emptyBags=dto.emptyBags!=null?dto.emptyBags:updating?writes.emptyBags(id):List.<Map<String,Object>>of();
        var breakups=dto.purchaseBreakups!=null?dto.purchaseBreakups:updating?writes.breakups(id):List.<Map<String,Object>>of();
        Map<Integer,Map<String,Object>> originals=new HashMap<>();
        for(var row:storedDetails)originals.put(number(row.get("Id")),row);
        for(var row:details) {
            int rowId=number(row.get("Id"));
            if(rowId>0 && !originals.containsKey(rowId))throw new IllegalArgumentException("A detail row belongs to another GRN");
            var merged=new TreeMap<String,Object>(String.CASE_INSENSITIVE_ORDER);
            if(rowId>0)merged.putAll(originals.get(rowId)); merged.putAll(row);
            if(number(merged.get("ItemId"))<=0 || number(merged.get("WarehouseId"))<=0)
                throw new IllegalArgumentException("Select an item and warehouse for every GRN row");
            row.clear();row.putAll(merged);
        }

        if(type==46) {
            if(dto.emptyBags!=null) {
                supplements.validateEmptyBagLookups(emptyBags);
                PurchaseGrnGridRules.validateEmptyBags(emptyBags,details);
                var state=records.breakupContext(gp,id);
                boolean purchased=emptyBags.stream().anyMatch(row->number(row.get("TypeId"))==2);
                boolean previouslyPurchased=updating&&writes.emptyBags(id).stream().anyMatch(row->number(row.get("TypeId"))==2);
                if(purchased||previouslyPurchased)PurchaseGrnGridRules.applyPurchasedBagWeight(details,emptyBags,number(state.get("referenceType")),number(state.get("referenceType"))==41&&number(state.get("orderCategoryId"))==8);
            }
            if(dto.purchaseBreakups!=null)breakups=supplements.prepareBreakups(header,breakups,id);
            // InvFrmGRN.Insert refusals (weights, freight, detail rows, 105 qty vs breakup).
            saveRules.apply(header,details,emptyBags,breakups,dto.header,id);
        }

        // Sp_InvGrn_Update rebuilds children; capture all collections before invoking it.
        id=writes.saveHeader(header,updating); int line=1;
        for(var row:details)writes.saveDetail(row,originals.get(number(row.get("Id"))),id,line++);
        for(var row:emptyBags)writes.saveEmptyBag(row,id);
        for(var row:breakups)writes.saveBreakup(row,id);
        writes.validateAndPost(org,company,type,id,gp,user);
        return Map.of("success",true,"id",id,"docNo",writes.finalNumber(id),"message",(type==46?"Market GRN":"Sale Return GRN")+" saved successfully");
    }
}
