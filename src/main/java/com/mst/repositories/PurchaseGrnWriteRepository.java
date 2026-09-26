package com.mst.repositories;

import com.mst.repositories.support.ProcExec;
import java.sql.Types;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Repository;

/** Full named-parameter contracts from original InvGrn DAL; no raw accounting writes. */
@Repository
public class PurchaseGrnWriteRepository {
    private final JdbcTemplate jdbc;
    public PurchaseGrnWriteRepository(JdbcTemplate jdbc) { this.jdbc=jdbc; }
    private static final String[] HEADER_FIELDS={"Id","DocumentTypeId","DocDate","DocNo","SupplierCustomerId","GpNo","ReferencePartyId","StockPartyId","SupplierReference","ReferenceDocNo","PartyWeight","FactoryWeight","VehicleType","BiltyNo","VehicleNo","TransporterDocRef","TransporterId","CarriageAmount","OtherCharges","RemarksHeader","IsApproved","EntryDate","EntryUser","ModifyDate","ModifyUser","PostDate","PostUser","OrganizationId","CompanyId","BranchesId","ProjectsId","InwardGatePassId","ActionId","FinancialYearId","DeliveryTerm","Status","ScaleKart","GhallaMandiId","TransporterSupCustId","BillCalculateTypeId","BillWeightExceedsStockWeight","GrnTypeId","ReqestedById","ReturnableDate","TransType","ScreenName","AddWages","ScaleShortWeightApply","SupplierShortWeightApply","RateCut","RateCutRemarks","AccessWeight","AttachmentsValues","CustomAttachmentsValues","DeductionPolicyForGrnId","SupplierDispatchId","FreightDeduction","BiltyFreight","AdvanceByFactoryFreight","AdvanceByPartyFreight","BaseDocumentTypeId","GainLossWeight","UserLogId"};
    private static final int[] HEADER_TYPES={Types.INTEGER,Types.INTEGER,Types.DATE,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.NVARCHAR,Types.NVARCHAR,Types.DOUBLE,Types.DOUBLE,Types.NVARCHAR,Types.NVARCHAR,Types.NVARCHAR,Types.NVARCHAR,Types.INTEGER,Types.DOUBLE,Types.DOUBLE,Types.NVARCHAR,Types.BIT,Types.TIMESTAMP,Types.INTEGER,Types.TIMESTAMP,Types.INTEGER,Types.TIMESTAMP,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.NVARCHAR,Types.NVARCHAR,Types.DOUBLE,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.DATE,Types.NVARCHAR,Types.NVARCHAR,Types.BIT,Types.BIT,Types.BIT,Types.DOUBLE,Types.NVARCHAR,Types.DOUBLE,Types.NVARCHAR,Types.NVARCHAR,Types.INTEGER,Types.INTEGER,Types.DOUBLE,Types.DOUBLE,Types.DOUBLE,Types.DOUBLE,Types.INTEGER,Types.DECIMAL,Types.BIGINT};
    private static final String[] DETAIL_FIELDS={"Id","InvGrnId","PurchaseOrderId","ItemId","PackingTypeId","CropYear","JobLotId","ItemQty","ItemUomId","GrossWeight","EBWPerUnit","EBWTotal","WtCut","WtCutTotal","AdLsWeight","NetBillWeight","StockWeight","WarehouseId","LabReportRef","AreaCity","CommentsDetail","SupplySchedulId","GatePassInwarDetailId","InvPurchasedemondId","PurchaseOrderDetailId","PurchaserOrderNo","ContractorId","PurchaseDemondDetailId","ScaleKart","EBCutApprovedUserId","EBCutDate","EBCutApprovedUserRemarks","CityId","GdnId","GdnDetailId","AssetId","ConditionId","WareHouseFromId","EbPurAgainstWeight","WbTicketId","RefDocumentTypeId","RefDocNoId","RefDocSubIdNo","LineId","ScaleShortWeight","SupplierShortWeight","WeightCutOnId","FreightAmount","LabId","QtyForWtCut","PackingDate","ExpiryDate","CropYearId","GdnDocumentTypeId","SupplierQty","StockEbUnit","StockEbTotal","DeliveryChallanId","DeliveryChallanDetailId","ItemConditionId","RackId","GainLossWeightDetail"};
    private static final int[] DETAIL_TYPES={Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.NVARCHAR,Types.INTEGER,Types.DOUBLE,Types.INTEGER,Types.DOUBLE,Types.DOUBLE,Types.DOUBLE,Types.DOUBLE,Types.DOUBLE,Types.DOUBLE,Types.DOUBLE,Types.DOUBLE,Types.INTEGER,Types.NVARCHAR,Types.NVARCHAR,Types.NVARCHAR,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.DOUBLE,Types.INTEGER,Types.TIMESTAMP,Types.NVARCHAR,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.DOUBLE,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.DOUBLE,Types.DOUBLE,Types.INTEGER,Types.DOUBLE,Types.INTEGER,Types.DOUBLE,Types.TIMESTAMP,Types.TIMESTAMP,Types.INTEGER,Types.INTEGER,Types.DOUBLE,Types.DOUBLE,Types.DOUBLE,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.DECIMAL};
    private static final String[] EMPTY_BAG_FIELDS={"Id","ItemId","ItemUomId","InvGrnId","ReceivedQty","PurchaseQty","Remarks","WarehouseId","TypeId","PurchaseOrderId","BagsCondition","RackId"};
    private static final int[] EMPTY_BAG_TYPES={Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.DOUBLE,Types.DOUBLE,Types.NVARCHAR,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER};
    private static final String[] BREAKUP_FIELDS={"Id","InvGrnId","InwardGatePassId","InwardBreakupId","Qty","UOM","GrossWeight","EbWeight","EBTotal","SupplierShortWeight","NetWeight","NetPackSize","SupplierRcvdWeight","ScaleShortage","BillWeight"};
    private static final int[] BREAKUP_TYPES={Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.INTEGER,Types.DECIMAL,Types.DECIMAL,Types.DECIMAL,Types.DECIMAL,Types.DECIMAL,Types.DECIMAL,Types.DECIMAL,Types.DECIMAL,Types.DECIMAL,Types.DECIMAL,Types.DECIMAL};

    private static Map<String,Object> headerDefaults() {
        Map<String,Object> values=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        values.put("IsApproved",false);
        values.put("AddWages",false);
        values.put("ScaleShortWeightApply",false);
        values.put("SupplierShortWeightApply",false);
        values.put("DocDate",java.sql.Timestamp.valueOf("1753-01-01 00:00:00"));
        values.put("EntryDate",java.sql.Timestamp.valueOf("1753-01-01 00:00:00"));
        values.put("ModifyDate",java.sql.Timestamp.valueOf("1753-01-01 00:00:00"));
        values.put("PostDate",null);
        values.put("CarriageAmount",0d);
        values.put("FreightDeduction",0d);
        values.put("BiltyFreight",0d);
        values.put("AdvanceByFactoryFreight",0d);
        values.put("AdvanceByPartyFreight",0d);
        values.put("AccessWeight",0d);
        values.put("FactoryWeight",0d);
        values.put("OtherCharges",0d);
        values.put("PartyWeight",0d);
        values.put("ScaleKart",0d);
        values.put("BranchesId",0);
        values.put("CompanyId",0);
        values.put("FinancialYearId",0);
        values.put("DocNo",0);
        values.put("SupplierDispatchId",0);
        values.put("DocumentTypeId",0);
        values.put("BaseDocumentTypeId",0);
        values.put("EntryUser",0);
        values.put("GpNo",0);
        values.put("Id",0);
        values.put("InwardGatePassId",0);
        values.put("ModifyUser",0);
        values.put("OrganizationId",0);
        values.put("PostUser",0);
        values.put("ProjectsId",0);
        values.put("ReferencePartyId",0);
        values.put("StockPartyId",0);
        values.put("SupplierCustomerId",0);
        values.put("TransporterId",0);
        values.put("TransporterSupCustId",0);
        values.put("GhallaMandiId",0);
        values.put("BillCalculateTypeId",0);
        values.put("GrnTypeId",0);
        values.put("ReqestedById",0);
        values.put("ReturnableDate",null);
        values.put("ActionId",0);
        return values;
    }
    private static Map<String,Object> detailDefaults() {
        Map<String,Object> values=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        values.put("AdLsWeight",0d);
        values.put("EBWPerUnit",0d);
        values.put("EBWTotal",0d);
        values.put("EbPurAgainstWeight",0d);
        values.put("GrossWeight",0d);
        values.put("SupplierQty",0d);
        values.put("ItemQty",0d);
        values.put("NetBillWeight",0d);
        values.put("StockWeight",0d);
        values.put("WtCut",0d);
        values.put("WtCutTotal",0d);
        values.put("ScaleKart",0d);
        values.put("ScaleShortWeight",0d);
        values.put("SupplierShortWeight",0d);
        values.put("FreightAmount",0d);
        values.put("StockEbUnit",0d);
        values.put("StockEbTotal",0d);
        values.put("Id",0);
        values.put("InvGrnId",0);
        values.put("ItemId",0);
        values.put("ItemUomId",0);
        values.put("JobLotId",0);
        values.put("PackingTypeId",0);
        values.put("RefDocumentTypeId",0);
        values.put("RefDocNoId",0);
        values.put("RefDocSubIdNo",0);
        values.put("PurchaseOrderId",0);
        values.put("PurchaseOrderDetailId",0);
        values.put("PurchaserOrderNo",0);
        values.put("SupplySchedulId",0);
        values.put("GatePassInwarDetailId",0);
        values.put("WarehouseId",0);
        values.put("WareHouseFromId",0);
        values.put("InvPurchasedemondId",0);
        values.put("PurchaseDemondDetailId",0);
        values.put("PackingDate",null);
        values.put("ExpiryDate",null);
        values.put("CropYearId",0);
        values.put("ContractorId",0);
        values.put("CityId",0);
        values.put("WbTicketId",0);
        values.put("LineId",0);
        values.put("WeightCutOnId",0);
        values.put("LabId",0);
        values.put("QtyForWtCut",0d);
        values.put("GdnId",0);
        values.put("GdnDetailId",0);
        values.put("GdnDocumentTypeId",0);
        values.put("AssetId",0);
        values.put("ConditionId",0);
        values.put("DeliveryChallanId",0);
        values.put("DeliveryChallanDetailId",0);
        values.put("ItemConditionId",0);
        values.put("RackId",0);
        return values;
    }
    private static Map<String,Object> empty_bagDefaults() {
        Map<String,Object> values=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        values.put("PurchaseQty",0d);
        values.put("ReceivedQty",0d);
        values.put("Id",0);
        values.put("InvGrnId",0);
        values.put("ItemId",0);
        values.put("TypeId",0);
        values.put("PurchaseOrderId",0);
        values.put("BagsCondition",0);
        return values;
    }
    private static Map<String,Object> breakupDefaults() {
        Map<String,Object> values=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        values.put("EBTotal",java.math.BigDecimal.ZERO);
        values.put("NetPackSize",java.math.BigDecimal.ZERO);
        values.put("SupplierShortWeight",java.math.BigDecimal.ZERO);
        values.put("SupplierRcvdWeight",java.math.BigDecimal.ZERO);
        values.put("ScaleShortage",java.math.BigDecimal.ZERO);
        values.put("BillWeight",java.math.BigDecimal.ZERO);
        values.put("EbWeight",java.math.BigDecimal.ZERO);
        values.put("GrossWeight",java.math.BigDecimal.ZERO);
        values.put("NetWeight",java.math.BigDecimal.ZERO);
        values.put("Qty",java.math.BigDecimal.ZERO);
        values.put("UOM",java.math.BigDecimal.ZERO);
        values.put("Id",0);
        values.put("InvGrnId",0);
        values.put("InwardBreakupId",0);
        values.put("InwardGatePassId",0);
        return values;
    }

    public Map<String,Object> headerValues(Map<String,Object> existing,Map<String,Object> supplied) {
        var values=headerDefaults(); if(existing!=null)values.putAll(existing); values.putAll(supplied); return values;
    }
    public int saveHeader(Map<String,Object> header,boolean updating) {
        Integer result=execute(updating?"Sp_InvGrn_Update":"Sp_InvGrn_Insert",HEADER_FIELDS,HEADER_TYPES,header);
        int id=updating?number(header.get("Id")):number(result);
        if(id<=0)throw new IllegalStateException("GRN procedure did not return a record ID");
        return id;
    }
    public void saveDetail(Map<String,Object> supplied,Map<String,Object> existing,int id,int line) {
        var values=detailDefaults(); if(existing!=null)values.putAll(existing); values.putAll(supplied);
        if(supplied.containsKey("qty")&&!supplied.containsKey("ItemQty")&&!supplied.containsKey("itemQty"))values.put("ItemQty",supplied.get("qty"));
        if(supplied.containsKey("ebUnit")&&!supplied.containsKey("EBWPerUnit")&&!supplied.containsKey("ebwPerUnit"))values.put("EBWPerUnit",supplied.get("ebUnit"));
        if(supplied.containsKey("ebTotal")&&!supplied.containsKey("EBWTotal")&&!supplied.containsKey("ebwTotal"))values.put("EBWTotal",supplied.get("ebTotal"));
        values.put("InvGrnId",id); values.put("LineId",line);
        execute("Sp_InvGrnDetail_Insert",DETAIL_FIELDS,DETAIL_TYPES,values);
    }
    public void saveEmptyBag(Map<String,Object> row,int id) {
        var values=empty_bagDefaults(); values.putAll(row); values.put("InvGrnId",id);
        execute("Sp_InvGrnDetailEmptyBags_Insert",EMPTY_BAG_FIELDS,EMPTY_BAG_TYPES,values);
    }
    public void saveBreakup(Map<String,Object> row,int id) {
        var values=breakupDefaults(); values.putAll(row); values.put("InvGrnId",id);
        execute("USP_InvGrnPurchaseBreakUp_Insert",BREAKUP_FIELDS,BREAKUP_TYPES,values);
    }
    public List<Map<String,Object>> details(int id) { return jdbc.queryForList("SELECT * FROM dbo.InvGrnDetail WHERE InvGrnId=? ORDER BY LineId,Id",id); }
    public List<Map<String,Object>> emptyBags(int id) { return jdbc.queryForList("SELECT * FROM dbo.InvGrnDetailEmptyBags WHERE InvGrnId=? ORDER BY Id",id); }
    public List<Map<String,Object>> breakups(int id) { return jdbc.queryForList("SELECT * FROM dbo.InvGrnPurchaseBreakUp WHERE InvGrnId=? ORDER BY Id",id); }
    public int finalNumber(int id) { return jdbc.queryForObject("SELECT DocNo FROM dbo.InvGrn WHERE Id=?",Integer.class,id); }
    public void validateAndPost(int org,int company,int type,int id,int gatePass,int user) {
        if(type==46) {
            ProcExec.run(jdbc,"EXEC dbo.USP_GrnBillWeightAndStockWeightValidations @OrganizationId=?,@CompanyId=?,@Id=?",org,company,id);
            ProcExec.run(jdbc,"EXEC dbo.USP_Validation_MarketGRNGrossWeightCompareToScaleWeight @GrnId=?,@GpId=?",id,gatePass);
        }
        ProcExec.run(jdbc,"EXEC dbo.Sp_InventoryTransactions_GetALLMethod @OrganizationId=?,@CompanyId=?,@RefDocumentTypeId=?,@RefDocIdNo=?",org,company,type,id);
        ProcExec.run(jdbc,"EXEC dbo.usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@GrnId=?",org,company,type,id);
        if(type==46)ProcExec.run(jdbc,"EXEC dbo.USP_InvGrn_GenerateSpecialApprovalLog @OrganizationId=?,@CompanyId=?,@GrnDocumentTypeId=?,@GrnDocId=?,@UserId=?",org,company,type,id,user);
    }
    private Integer execute(String procedure,String[] fields,int[] types,Map<String,Object> values) {
        List<String> parameters=new ArrayList<>(); List<Object> arguments=new ArrayList<>();
        for(int i=0;i<fields.length;i++) {
            Object value=values.get(fields[i]);
            if(value instanceof String && (types[i]==Types.DATE||types[i]==Types.TIMESTAMP)) {
                String text=((String)value).trim().replace('T',' ');
                value=text.isEmpty()?null:(types[i]==Types.DATE?java.sql.Date.valueOf(text.substring(0,10)):java.sql.Timestamp.valueOf(text.length()==10?text+" 00:00:00":text));
            }
            if(value instanceof java.util.Date)value=new java.sql.Timestamp(((java.util.Date)value).getTime());
            parameters.add("@"+fields[i]+"=?"); arguments.add(new SqlParameterValue(types[i],value));
        }
        return ProcExec.call(jdbc,"EXEC dbo."+procedure+" "+String.join(",",parameters),arguments.toArray());
    }
    public static int number(Object value) { return value instanceof Number?((Number)value).intValue():value==null?0:Integer.parseInt(value.toString()); }
}
