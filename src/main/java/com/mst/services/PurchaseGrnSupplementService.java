package com.mst.services;

import com.mst.models.dto.PurchaseGrnDto;
import com.mst.repositories.PurchaseGrnRecordRepository;
import com.mst.repositories.PurchaseGrnWriteRepository;
import java.util.*;
import org.springframework.stereotype.Service;
import org.springframework.jdbc.core.JdbcTemplate;
import com.mst.security.CurrentUserContext;
import static com.mst.repositories.PurchaseGrnWriteRepository.number;
import static com.mst.services.PurchaseGrnGridRules.*;

/** The GRN supplementary grids share this calculation path with Save/Update. */
@Service
public class PurchaseGrnSupplementService {
    private final PurchaseGrnRecordRepository records;
    private final PurchaseGrnWriteRepository writes;
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    public PurchaseGrnSupplementService(PurchaseGrnRecordRepository records,PurchaseGrnWriteRepository writes,JdbcTemplate jdbc,CurrentUserContext context) {
        this.records=records;this.writes=writes;this.jdbc=jdbc;this.context=context;
    }
    public Map<String,Object> calculate(Map<String,Object> payload) {
        records.requireRight(46,"View");
        var dto=new PurchaseGrnDto(payload);int id=number(dto.header.get("Id"));
        if(id>0)records.require(id,46);
        var state=records.breakupContext(number(dto.header.get("InwardGatePassId")),id);
        if(Boolean.TRUE.equals(state.get("breakupLocked")))throw new IllegalArgumentException("Purchase breakup is locked because this gate pass has another GRN");
        if(number(state.get("referenceType"))!=105)throw new IllegalArgumentException("Purchase breakup editing is available for Market Purchase gate passes");
        var rows=calculateBreakups(dto.purchaseBreakups,value(dto.header,"PartyWeight"),value(dto.header,"FactoryWeight"),
                flag(dto.header,"SupplierShortWeightApply"),flag(dto.header,"ScaleShortWeightApply"));
        return Map.of("purchaseBreakups",rows);
    }
    public List<Map<String,Object>> prepareBreakups(Map<String,Object> header,List<Map<String,Object>> supplied,int id) {
        int gp=number(header.get("InwardGatePassId"));
        var state=records.breakupContext(gp,id);
        if(number(state.get("referenceType"))!=105) {
            if(id>0)return writes.breakups(id);
            return List.of();
        }
        if(Boolean.TRUE.equals(state.get("breakupLocked"))) {
            // The desktop locks these inputs; never trust replacements from a crafted request.
            return id>0?writes.breakups(id):jdbc.queryForList("EXEC dbo.Sp_InvGrn_GetAllMethod @Id=?,@Activity='ReadByGPId_InvGrnPurchaseBreakup'",gp);
        }
        var rows=calculateBreakups(supplied,value(header,"PartyWeight"),value(header,"FactoryWeight"),flag(header,"SupplierShortWeightApply"),flag(header,"ScaleShortWeightApply"));
        if(rows.isEmpty())throw new IllegalArgumentException("Purchase breakup is required for Market Purchase");
        for(var row:rows) {
            if(value(row,"Qty")<=0||value(row,"UOM")<=0)throw new IllegalArgumentException("Quantity and gross pack size are required in purchase breakup");
            row.put("InwardGatePassId",gp);
            int inwardRow=number(row.get("InwardBreakupId"));
            if(inwardRow>0&&jdbc.queryForObject("SELECT COUNT(*) FROM dbo.GatePassInwardPurchaseBreakUp WHERE Id=? AND InwardGatePassId=?",Integer.class,inwardRow,gp)!=1)
                throw new IllegalArgumentException("The purchase breakup row belongs to another gate pass");
        }
        return rows;
    }
    public void validateEmptyBagLookups(List<Map<String,Object>> bags) {
        if(bags.isEmpty())return;
        Set<Integer> items=new HashSet<>();
        for(var row:jdbc.queryForList("EXEC dbo.USP_PackingMaterialItemsAllocateToTransactionFlow_GetForCombo @OrganizationId=?,@CompanyId=?,@TransactionFlowId=1",context.currentOrganizationId(),context.currentCompanyId()))items.add(number(row.get("ItemId")));
        Set<Integer> types=new HashSet<>();for(var row:jdbc.queryForList("EXEC dbo.SpStaticColumnNames @Activity='PurchaseOrderEmptyBagsType'"))types.add(number(row.get("Id")));
        Set<Integer> conditions=new HashSet<>(jdbc.queryForList("SELECT Id FROM dbo.V_ItemCondition WHERE Id<>4",Integer.class));
        for(var row:bags) {
            if(!items.contains(number(row.get("ItemId")))||!types.contains(number(row.get("TypeId")))||!conditions.contains(number(row.get("BagsCondition"))))
                throw new IllegalArgumentException("Select an allocated packing item, empty-bag type and condition");
        }
    }
    private static boolean flag(Map<String,Object> row,String key) { return Boolean.TRUE.equals(row.get(key))||"1".equals(Objects.toString(row.get(key))); }
}
