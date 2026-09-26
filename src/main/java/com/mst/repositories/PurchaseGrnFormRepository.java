package com.mst.repositories;

import com.mst.security.CurrentUserContext;
import java.sql.Types;
import java.util.*;
import org.springframework.http.HttpStatus;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;
import org.springframework.web.server.ResponseStatusException;
import static com.mst.repositories.PurchaseGrnWriteRepository.number;

/**
 * InvFrmGRN.cs (Purchase GRN, DocumentTypeId 46) — the form-time lookups the desktop makes
 * while an operator fills the screen, each one the BLL's own procedure with the BLL's own
 * parameters (null/zero parameters OMITTED where the BLL guards them, so the proc default applies).
 *
 *   config()          GetConfigurationsFromGlobal :1004 + GetConfigurationsFromGlobalAndBindValuesInColumns :928
 *   pendingRow()      GatepassDataDbCall :2464 → USP_GatePassInward_PendingForGrn, shaped as GatepassGridFill :2490
 *   gatePassForGrn()  GatePassRecordFill :2852 → Sp_GatePassInward_GetAllMethod 'ReadByGpNoForGRN'
 *   items()           ItemdtFillFromLab :2994 → usp_getItemsFromLabOrPurchaseOrderOrAllByGpId (ActionId 0 omitted)
 *   lab()             LabNoFillWithGpIdAndItemId :1385 → Sp_InvLabAnalysisPurchaseHeader_GetAllMethod
 *   labParameters()   CmbLabNo_Leave :6995 → usp_getLabAnaylsisAgainstMasterParamsById
 *   preBills()        PreBillNoFill :884 → USP_SupplierDispatch_GetNoForGpandGrn
 *   purchaseOrder()   comborderno_Leave :1346 → Sp_PurchaseOrderDetail_GetAllMethod / Sp_PurchaseOrder_GetAllMethod
 *   previousData()    GetPrevoiusDataOfSelectedGP :2981 → Sp_InvGRN_GetAllMethod 'GrnPrevoiusDataOfSelectedGP'
 *   receivedWeight()  ReadById :4054 → Sp_InvGrn_GetAllMethod 'GetReceivedWeightAndBalanceWeight'
 *   deductionPolicy() DeductionPolicyForGrn :3305 → USP_DeductionPolicyForGrn_GetAllMethod 'GetPolicyForGrn'
 */
@Repository
public class PurchaseGrnFormRepository {
    public static final List<String> CONFIGS=List.of(
            "WeightCutForJuteBagsStock","WeightCutForPPBagsStock","WeightCutForOpenBulkStock","DefaultDaysToLessFromHistoryFromDate",
            "ItemSearchWithNameOrCode","DeductionPolicyForGrnIsOn","EBWtAccordingToGrossWeight","WeightCutForJuteBags","WeightCutForPPBags",
            "WeightCutForOpenBulk","EmptyBagsWeightCutEditableOnGRN","AddLessWeightCutEditableOnGRN","ContractorWagesCompulsoryBeforeInvoices",
            "EmptyBagsInofrmationCompulsoryOnGRN","LabCompulsoryForWeighBridgeAgainstGatePurchase","LabCompulsoryForWeighBridge",
            "BillWeightAndStockWeightDifferenceTolerance","DeliveryTermEditableOnGrnForGatePurchase","WeightCutEditable",
            "LabCompulsoryNotCheckingOnGRN","ExcludeWeightShortageBusinessOnGrn","ValidateGrnAndInvoiceDateWithGpDate",
            "AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt",
            "Job/Lot","Default Crop Year","Paking Type","Warehouse","FreightInwardAc");

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final PurchaseGrnRecordRepository records;
    public PurchaseGrnFormRepository(JdbcTemplate jdbc,CurrentUserContext context,PurchaseGrnRecordRepository records) {
        this.jdbc=jdbc;this.context=context;this.records=records;
    }

    private int org() { return context.currentOrganizationId(); }
    private int company() { return context.currentCompanyId(); }

    /** Raw ConfigKey strings, keyed by the desktop's exact ConfigDescription spelling. */
    public Map<String,String> configValues() {
        Map<String,String> values=new LinkedHashMap<>();
        for(String name:CONFIGS)values.put(name,"");
        for(var row:jdbc.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetMultipleConfigurationsByConfigDescriptions'",
                org(),company(),String.join(",",CONFIGS))) {
            String name=Objects.toString(row.get("ConfigDescription"),"");
            for(String known:CONFIGS)if(known.equalsIgnoreCase(name))values.put(known,Objects.toString(row.get("ConfigKey"),"").trim());
        }
        return values;
    }

    public Map<String,Object> config() {
        records.requireRight(46,"View");
        var raw=configValues();
        Map<String,Object> result=new LinkedHashMap<>();
        for(var e:raw.entrySet())result.put(e.getKey(),e.getValue());
        boolean subsidiary=jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",org(),company()).stream().anyMatch(r->number(r.get("Id"))==4);
        result.put("SubsidiaryAccountAllownOnVouchers",subsidiary);
        // GetWagesRefDocumentsStatusById(46): the start-up list is USP_GetRefDocumentsForWages with the id OMITTED.
        boolean wages=false;
        try {
            wages=jdbc.queryForList("EXEC [dbo].[USP_GetRefDocumentsForWages]").stream()
                    .anyMatch(r->number(r.get("RefDocumentTypeId"))==46 && flag(r.get("IsActive")));
        } catch(RuntimeException ignored) { /* the wages module is optional on some databases */ }
        result.put("WagesActiveOrInActive",wages);
        return result;
    }

    public static boolean flag(Object v) {
        if(v instanceof Boolean)return (Boolean)v;
        if(v instanceof Number)return ((Number)v).intValue()!=0;
        String s=Objects.toString(v,"").trim();
        return "1".equals(s)||"true".equalsIgnoreCase(s);
    }
    public static double dbl(Object v) {
        if(v instanceof Number)return ((Number)v).doubleValue();
        try { String s=Objects.toString(v,"").trim(); return s.isEmpty()?0:Double.parseDouble(s); } catch(NumberFormatException e) { return 0; }
    }

    /** One GatepassGridFill row: FVStatus computed, LabId→LastLabId, AccessWeight→PoAccessWeight. */
    public static Map<String,Object> pendingShape(Map<String,Object> row) {
        Map<String,Object> r=new LinkedHashMap<>(row);
        r.put("FVStatus",number(row.get("FreightId"))==0?"Pending":"Complete");
        r.put("LastLabId",row.get("LabId"));
        r.put("PoAccessWeight",row.get("AccessWeight"));
        r.put("ItemName",row.get("VarietyName"));
        return r;
    }

    public List<Map<String,Object>> pendingRows() {
        List<Map<String,Object>> result=new ArrayList<>();
        for(var row:jdbc.queryForList("EXEC dbo.USP_GatePassInward_PendingForGrn @OrganizationId=?,@CompanyId=?,@FinancialYearId=?,@DocumentTypeId=51,@BranchesId=?",
                org(),company(),context.currentFinancialYearId(),context.currentBranchId()))result.add(pendingShape(row));
        return result;
    }

    public Map<String,Object> pendingRow(int gpId) {
        for(var row:pendingRows())if(number(row.get("Id"))==gpId)return row;
        return null;
    }

    /** GatePassInward.GetByGpNoForGRN — BranchesId is not set on the desktop object, so it is omitted. */
    public Map<String,Object> gatePassForGrn(int gpId) {
        var rows=jdbc.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@DocumentTypeId=51,@FinancialYearId=?,@Activity='ReadByGpNoForGRN'",
                org(),company(),gpId,context.currentFinancialYearId());
        return rows.isEmpty()?null:rows.get(0);
    }

    public List<Map<String,Object>> items(int gpId) {
        List<Map<String,Object>> result=new ArrayList<>();
        for(var row:jdbc.queryForList("EXEC dbo.usp_getItemsFromLabOrPurchaseOrderOrAllByGpId @OrganizationId=?,@CompanyId=?,@Id=?",org(),company(),gpId)) {
            Map<String,Object> m=new LinkedHashMap<>();
            m.put("id",row.get("ItemId"));m.put("name",row.get("ItemName"));
            m.put("ItemId",row.get("ItemId"));m.put("ItemName",row.get("ItemName"));m.put("ItemCode",row.get("ItemCode"));
            m.put("PoDetailId",row.get("PoDetailId"));m.put("ItemWbWeight",row.get("ItemWbWeight"));m.put("Moisture",row.get("Moisture"));
            result.add(m);
        }
        return result;
    }

    /** Only the first row is offered — dtLab is built from dt.Rows[0] (:1406). */
    public List<Map<String,Object>> lab(int gpId,int itemId) {
        records.requireRight(46,"View");
        var rows=jdbc.queryForList("EXEC dbo.Sp_InvLabAnalysisPurchaseHeader_GetAllMethod @OrganizationId=?,@CompanyId=?,@GatePassInwardId=?,@ItemId=?,@Activity='GetPurchaseAnalysisDataForGrnByGpIdAndItemId'",
                org(),company(),gpId,itemId);
        if(rows.isEmpty())return List.of();
        var r=rows.get(0);
        Map<String,Object> m=new LinkedHashMap<>();
        m.put("Id",r.get("LabId"));m.put("LabNo",r.get("LabNo"));m.put("QtyForWtCut",r.get("QtyForWtCut"));
        m.put("WtCut",r.get("WeightCut"));m.put("WtCutOnId",r.get("WeightCutOnId"));m.put("WtCutOn",r.get("WeightCutOn"));m.put("WeightCutUom",r.get("WeightCutUom"));
        return List.of(m);
    }

    public List<Map<String,Object>> labParameters(int labId) {
        records.requireRight(46,"View");
        return jdbc.queryForList("EXEC dbo.usp_getLabAnaylsisAgainstMasterParamsById @OrganizationId=?,@CompanyId=?,@Id=?",org(),company(),labId);
    }

    /** SupplierDispatch_GetNoForGpandGrn(org, company, party, RecId, 0, GPID, OrderId) — zero ids omitted. */
    public List<Map<String,Object>> preBills(int supplierId,int grnId,int gpId,int orderId) {
        records.requireRight(46,"View");
        StringBuilder sql=new StringBuilder("EXEC [dbo].[USP_SupplierDispatch_GetNoForGpandGrn] @OrganizationId=?,@CompanyId=?,@SupplierId=?");
        List<Object> args=new ArrayList<>(List.of(org(),company(),supplierId));
        if(grnId!=0){sql.append(",@GrnRecId=?");args.add(grnId);}
        if(gpId!=0){sql.append(",@GpId=?");args.add(gpId);}
        if(orderId!=0){sql.append(",@OrderId=?");args.add(orderId);}
        return jdbc.queryForList(sql.toString(),args.toArray());
    }

    public Map<String,Object> purchaseOrder(int orderId) {
        Map<String,Object> m=new LinkedHashMap<>();
        if(orderId<=0){m.put("detail",List.of());m.put("emptyBags",List.of());return m;}
        m.put("detail",jdbc.queryForList("EXEC dbo.Sp_PurchaseOrderDetail_GetAllMethod @PurchaseOrderId=?,@Activity='ReadByPurchaseOrderIDNOrderItemId'",orderId));
        m.put("emptyBags",jdbc.queryForList("EXEC dbo.Sp_PurchaseOrder_GetAllMethod @Id=?,@Activity='GetPurchaseOrderEmptyBagsDetailByOrderId'",orderId));
        return m;
    }

    public List<Map<String,Object>> previousData(int gpId,int recId) {
        if(gpId<=0)return List.of();
        if(recId!=0)return jdbc.queryForList("EXEC dbo.Sp_InvGRN_GetAllMethod @OrganizationId=?,@CompanyId=?,@GatePassInwardId=?,@RecId=?,@Activity='GrnPrevoiusDataOfSelectedGP'",org(),company(),gpId,recId);
        return jdbc.queryForList("EXEC dbo.Sp_InvGRN_GetAllMethod @OrganizationId=?,@CompanyId=?,@GatePassInwardId=?,@Activity='GrnPrevoiusDataOfSelectedGP'",org(),company(),gpId);
    }

    public Map<String,Object> receivedWeight(int gpId) {
        var rows=jdbc.queryForList("EXEC dbo.Sp_InvGrn_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@Activity='GetReceivedWeightAndBalanceWeight'",org(),company(),gpId);
        return rows.isEmpty()?Map.of():rows.get(0);
    }

    public Map<String,Object> deductionPolicy(String date,double difference) {
        records.requireRight(46,"View");
        var rows=jdbc.queryForList("EXEC [dbo].[USP_DeductionPolicyForGrn_GetAllMethod] @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@EffectedDATE=?,@Difference=?,@Activity='GetPolicyForGrn'",
                org(),company(),context.currentBranchId(),context.currentFinancialYearId(),
                new SqlParameterValue(Types.TIMESTAMP,date==null||date.isBlank()?new java.sql.Timestamp(System.currentTimeMillis()):java.sql.Timestamp.valueOf(date.substring(0,10)+" 00:00:00")),difference);
        return rows.isEmpty()?Map.of("PolicyTypeId",0,"ConditionDescription",""):rows.get(0);
    }

    /**
     * Load Gate Pass (LoadGPByRow :2716 then combgatepass_Leave :2961 → GatePassRecordFill :2852).
     * The pending-grid row supplies the load rules and the freight block; ReadByGpNoForGRN supplies
     * the header, weights and PO link; the rest follows from RefDocumentTypeId.
     */
    public Map<String,Object> load(int gpId,Map<String,String> cfg) {
        records.requireRight(46,"View");
        if(jdbc.queryForObject("SELECT COUNT(*) FROM dbo.GatePassInward WHERE Id=? AND OrganizationId=? AND CompanyId=? AND BranchesId=? AND FinancialYearId=? AND DocumentTypeId=51",Integer.class,
                gpId,org(),company(),context.currentBranchId(),context.currentFinancialYearId())!=1)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND,"Gate pass not found in the current company, branch and financial year");
        var pending=pendingRow(gpId);
        if(pending==null)throw new IllegalArgumentException("This gate pass is not pending for GRN.");
        if(!"Accepted".equals(Objects.toString(pending.get("Status"),"")))throw new IllegalArgumentException("Status Not Accepted Please check status");
        if(!flag(cfg.get("LabCompulsoryNotCheckingOnGRN")) && number(pending.get("LastLabId"))<=0)
            throw new IllegalArgumentException("Lab is pending for this gate pass. Please do lab first then Load");
        if(number(pending.get("FreightSpecialApprovalStatusId"))==1)
            throw new IllegalArgumentException("The shortage allowed on the Freight Voucher exceeds the configured discount policy.\r\nTherefore, approval from an authorized person is required.\r\nPlease complete the Freight Voucher approval before proceeding further");
        Map<String,Object> result=new LinkedHashMap<>();
        result.put("pending",pending);
        var gp=gatePassForGrn(gpId);
        result.put("gatePass",gp);
        int ref=gp!=null?number(gp.get("RefDocumentTypeId")):number(pending.get("RefDocumentTypeId"));
        result.put("refDocumentTypeId",ref);
        result.put("items",items(gpId));
        int orderId=gp!=null&&ref==41?number(gp.get("PurchaseOrderId")):0;
        result.put("purchaseOrder",purchaseOrder(orderId));
        result.put("previousData",previousData(gpId,0));
        int supplier=gp!=null?number(gp.get("SupplierCustomerId")):number(pending.get("SupplierCustomerId"));
        result.put("preBills",supplier>0||orderId>0?preBills(supplier,0,gpId,orderId):List.of());
        var breakups=jdbc.queryForList("EXEC dbo.Sp_InvGrn_GetAllMethod @Id=?,@Activity='ReadByGPId_InvGrnPurchaseBreakup'",gpId);
        result.put("breakupLocked",!breakups.isEmpty());
        if(breakups.isEmpty()) {
            breakups=jdbc.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @Id=?,@Activity='ReadByHeaderId_PurchaseBrekup'",gpId);
            for(var row:breakups){row.put("InwardBreakupId",row.get("Id"));row.put("Id",0);}
        }
        result.put("purchaseBreakups",breakups);
        result.put("referenceType",ref);
        return result;
    }
}
