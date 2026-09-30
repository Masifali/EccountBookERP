package com.mst.repositories;

import com.mst.models.InwardGatePass;
import com.mst.models.InwardGatePassDetail;
import com.mst.models.InwardGatePassPurchaseBreakUp;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.*;

@Repository
public class InwardGatePassRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    // Exact non-virtual fields of desktop model 1015. Values in `exact` are sent as given (null = parameter
    // not supplied, as ADO.NET AddWithValue(null) does); fields the desktop form never sets keep the stored value.
    public Integer saveHeader(InwardGatePass obj, Map<String,Object> exact) {
        boolean updating=obj.getId()!=null && obj.getId()>0;
        Map<String,Object> values=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        values.put("IsApproved", false);
        values.put("PostState", false);
        values.put("AdvanceByParty", 0d);
        values.put("AdvanceByFactory", 0d);
        values.put("DifferenceWeight", 0d);
        values.put("FactoryWeight", 0d);
        values.put("Freight", 0d);
        values.put("NetPaid", 0d);
        values.put("SupplierWeight", 0d);
        values.put("PackUnit", 0d);
        values.put("WeightComparedToPoWt", 0d);
        values.put("AccessWeight", 0d);
        values.put("SupplierFirstWeight", 0d);
        values.put("SupplierSecondWeight", 0d);
        values.put("CityId", 0);
        values.put("ItemId", 0);
        values.put("CompanyId", 0);
        values.put("DocumentTypeId", 0);
        values.put("EntryUser", 0);
        values.put("GpSrNo", 0);
        values.put("GpTypeSrNo", 0);
        values.put("PackingTypeId", 0);
        values.put("Id", 0);
        values.put("ModifyUser", 0);
        values.put("NoOfPackages", 0);
        values.put("OrganizationId", 0);
        values.put("PostUser", 0);
        values.put("PurchaseOrderId", 0);
        values.put("RefDocumentEntryNo", 0);
        values.put("RefDocumentTypeId", 0);
        values.put("SupplierCustomerId", 0);
        values.put("SupplierDispatchId", 0);
        values.put("WarehouseId", 0);
        values.put("WeighBridgeId", 0);
        values.put("FinancialYearId", 0);
        values.put("BranchesId", 0);
        values.put("FreightOn", 0);
        values.put("ActionIdForSpecialApproval", 0);
        values.put("driverBioDataId", 0);
        if (updating) values.putAll(jdbcTemplate.queryForMap("SELECT * FROM dbo.GatePassInward WHERE Id=? AND OrganizationId=? AND CompanyId=?",obj.getId(),obj.getOrganizationId(),obj.getCompanyId()));
        var bean=new org.springframework.beans.BeanWrapperImpl(obj);
        Map<String,String> properties=new TreeMap<>(String.CASE_INSENSITIVE_ORDER);
        for (var descriptor:bean.getPropertyDescriptors()) properties.put(descriptor.getName(),descriptor.getName());
        String[] fields={"IsApproved", "PostState", "EntryDate", "GpDate", "InDateTimeStamp", "ModifyDate", "OutDateTimeStamp", "PostDate", "BiltyDate", "AdvanceByParty", "AdvanceByFactory", "DifferenceWeight", "FactoryWeight", "Freight", "NetPaid", "SupplierWeight", "PackUnit", "WeightComparedToPoWt", "AccessWeight", "SupplierFirstWeight", "SupplierSecondWeight", "CityId", "ItemId", "CompanyId", "DocumentTypeId", "EntryUser", "GpSrNo", "GpTypeSrNo", "PackingTypeId", "Id", "ModifyUser", "NoOfPackages", "OrganizationId", "PostUser", "PurchaseOrderId", "RefDocumentEntryNo", "RefDocumentTypeId", "SupplierCustomerId", "SupplierDispatchId", "WarehouseId", "WeighBridgeId", "FinancialYearId", "BranchesId", "FreightOn", "ActionIdForSpecialApproval", "BiltyNo", "DocAttachment", "GatepassType", "OtherRemarks", "OtherSupCust", "Status", "SupplierContractCode", "VarietyName", "VehicleNo", "VehicleType", "Container", "Container1", "GpQRCode", "DestrictId", "TehsilId", "GhallaMandiId", "LoadingContractorId", "CarriageContractorId", "WorkingReportNo", "SupplierCNICNO", "SupplierMobileNo", "TransporterName", "DriverName", "DriverCNICNO", "DriverMobileNo", "WeightDiffComments", "driverBioDataId"};
        int[] sqlTypes={java.sql.Types.BIT,java.sql.Types.BIT,java.sql.Types.TIMESTAMP,java.sql.Types.TIMESTAMP,java.sql.Types.TIMESTAMP,java.sql.Types.TIMESTAMP,java.sql.Types.TIMESTAMP,java.sql.Types.TIMESTAMP,java.sql.Types.TIMESTAMP,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.DOUBLE,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.VARBINARY,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.INTEGER,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.NVARCHAR,java.sql.Types.INTEGER};
        int parameterIndex=0;
        List<String> parameters=new ArrayList<>(); List<Object> arguments=new ArrayList<>();
        for (String field:fields) {
            if (exact.containsKey(field)) values.put(field,exact.get(field));
            else if (properties.containsKey(field)) {
                Object supplied=bean.getPropertyValue(properties.get(field));
                if (supplied!=null || !values.containsKey(field)) values.put(field,supplied);
            }
            Object value=values.get(field);
            if (value instanceof java.util.Date) value=new java.sql.Timestamp(((java.util.Date)value).getTime());
            parameters.add("@"+field+"=?"); arguments.add(new org.springframework.jdbc.core.SqlParameterValue(sqlTypes[parameterIndex++],value));
        }
        Integer id=com.mst.repositories.support.ProcExec.call(jdbcTemplate,"EXEC dbo."+(updating?"Sp_GatePassInward_Update":"Sp_GatePassInward_Insert")+" "+String.join(",",parameters),arguments.toArray());
        if (updating) return obj.getId();
        if (id==null || id<=0) throw new IllegalStateException("Inward Gate Pass procedure did not return a record ID");
        return id;
    }

    // DAL 0421 SetData: EntryDate/ModifyDate/ApprovedDate come from the header's EntryDate/ModifyDate/PostDate.
    public Integer saveDriverBio(InwardGatePass obj) {
        return com.mst.repositories.support.ProcExec.call(jdbcTemplate,
                "EXEC dbo.USP_DriverBiodata_InsertIfNotExists @driverName=?,@cnicNo=?,@cellNo=?,@whatsappNo=?,@alternateCellNo=?,@fatherName=?,@fatherCnicNo=?,@OrganizationId=?,@CompanyId=?,@EntryUserId=?,@EntryDate=?,@BranchId=?,@ModifyDate=?,@ApprovedDate=?",
                obj.getDriverName(),obj.getDriverCNICNO(),obj.getDriverMobileNo(),obj.getWhatsappNo(),obj.getAlternateCellNo(),obj.getFatherName(),obj.getFatherCnicNo(),obj.getOrganizationId(),obj.getCompanyId(),obj.getEntryUser(),obj.getEntryDate(),obj.getBranchesId(),obj.getModifyDate(),obj.getPostDate());
    }

    // Save Detail Row
    public void saveDetail(InwardGatePassDetail detail) {
        String sql = "EXEC Sp_GatePassInwardDetail_Insert " +
                "@GatePassInwardId=?, @ItemId=?, @PackUnit=?, @Weight=?, @ItemUOMId=?, @ItemQty=?, @CropYear=?, " +
                "@JobLotId=?, @SupplyScheduleId=?, @WareHouseId=?, @PackingTypeId=?, @RefDocumentTypeId=?, " +
                "@SupplierCustomerId=?, @PurchaseOrderId=?, @PurchaseOrderDetailId=?, @CityId=?, @RemarksDetail=?";
        com.mst.repositories.support.ProcExec.run(jdbcTemplate, sql,
                detail.getGatePassInwardId(), detail.getItemId(), detail.getPackUnit(), detail.getWeight(),
                detail.getItemUOMId(), detail.getItemQty(), detail.getCropYear(), detail.getJobLotId(),
                detail.getSupplyScheduleId(), detail.getWareHouseId(), detail.getPackingTypeId(),
                detail.getRefDocumentTypeId(), detail.getSupplierCustomerId(), detail.getPurchaseOrderId(),
                detail.getPurchaseOrderDetailId(), detail.getCityId(), detail.getRemarksDetail()
        );
    }

    // Save Purchase BreakUp Row
    public void savePurchaseBreakUp(InwardGatePassPurchaseBreakUp breakUp) {
        String sql = "EXEC [dbo].[USP_GatePassInwardPurchaseBreakUp_Insert] " +
                "@InwardGatePassId=?, @Qty=?, @UOM=?, @GrossWeight=?, @EbWeight=?, @EBTotal=?, @NetWeight=?";
        com.mst.repositories.support.ProcExec.run(jdbcTemplate, sql,
                breakUp.getInwardGatePassId(), breakUp.getQty(), breakUp.getUom(), breakUp.getGrossWeight(),
                breakUp.getEbWeight(), breakUp.getEbTotal(), breakUp.getNetWeight()
        );
    }

    // Get Header by ID
    public Map<String, Object> getHeaderById(Integer id) {
        String sql = "EXEC Sp_GatePassInward_GetAllMethod @Id=?, @Activity='ReadById'";
        List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, id);
        return (list != null && !list.isEmpty()) ? list.get(0) : null;
    }

    // Get Details by Header ID
    public List<Map<String, Object>> getDetailsByHeaderId(Integer id) {
        String sql = "EXEC Sp_GatePassInwardDetail_GetAllMethod @Id=?, @Activity='ReadById'";
        return jdbcTemplate.queryForList(sql, id);
    }

    // Get Purchase Breakups by Header ID
    public List<Map<String, Object>> getPurchaseBreakUpsByHeaderId(Integer id) {
        String sql = "EXEC Sp_GatePassInward_GetAllMethod @Id=?, @Activity='ReadByHeaderId_PurchaseBrekup'";
        return jdbcTemplate.queryForList(sql, id);
    }

    // BLL 0567 GenerategpCode / GenerateGPTypeCode: first row's GpSrNo / GpTypeSrNo, 0 when no row.
    public Integer generateGpCode(Integer orgId,Integer compId,Integer branchId,Integer yearId,Integer docTypeId) {
        return firstInt(jdbcTemplate.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=?,@Activity='GenerategpCode'",orgId,compId,branchId,yearId,docTypeId),"GpSrNo");
    }
    public Integer generateGpTypeCode(Integer orgId,Integer compId,Integer branchId,Integer yearId,String type) {
        return firstInt(jdbcTemplate.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@GatepassType=?,@Activity='GenerateGPTypeCode'",orgId,compId,branchId,yearId,type),"GpTypeSrNo");
    }
    private static int firstInt(List<Map<String,Object>> rows,String key) {
        return rows.isEmpty() || !(rows.get(0).get(key) instanceof Number)?0:((Number)rows.get(0).get(key)).intValue();
    }

    // BLL GatepassHistory: bind dates and apply the user's visibility, branch and year.
    public List<Map<String,Object>> getOpenGatePasses(int org,int company,int branch,int year) {
        return jdbcTemplate.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=51,@Activity='ReadByGPDate'",org,company,branch,year);
    }

    public List<Map<String,Object>> getHistory(Integer orgId,Integer compId,Integer branchId,Integer yearId,Integer docTypeId,String fromDate,String toDate,Double fromDocNo,Double toDocNo,Integer supplierId,boolean canViewAll,int userId,String dateField) {
        String from="@DateFrom",to="@DateTo";
        if ("entryDate".equals(dateField)) { from="@EntryFromDate"; to="@EntryToDate"; }
        else if ("modifyDate".equals(dateField)) { from="@ModifyFromDate"; to="@ModifyToDate"; }
        else if (!"docDate".equals(dateField)) throw new IllegalArgumentException("Unknown history date filter");
        return jdbcTemplate.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=?,"+from+"=?,"+to+"=?,@DocNoFrom=?,@DocNoTo=?,@SupplierCustomerId=?,@CanViewAllRecord=?,@EntryUser=?,@Activity='GatepassHistory' WITH RECOMPILE",
                orgId,compId,branchId,yearId,docTypeId,
                new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.DATE,date(fromDate)),
                new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.DATE,date(toDate)),
                positive(fromDocNo),positive(toDocNo),positive(supplierId),canViewAll,canViewAll?null:userId);
    }
    private static Object positive(Number v) { return new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.INTEGER,v!=null && v.doubleValue()>0?v.intValue():null); }
    private static java.sql.Date date(String v) { return v==null || v.isBlank()?null:java.sql.Date.valueOf(v); }
    private static List<Map<String,Object>> options(List<Map<String,Object>> rows,String id,String label) {
        List<Map<String,Object>> result=new ArrayList<>();
        for (var row:rows) { var option=new LinkedHashMap<String,Object>(row); option.put("id",row.get(id)); option.put("name",row.get(label)); result.add(option); }
        return result;
    }

    // Dropdown loaders
    public List<Map<String,Object>> getSuppliers(Integer orgId,Integer compId) {
        boolean subsidiary=jdbcTemplate.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",orgId,compId).stream().anyMatch(r->r.get("Id") instanceof Number && ((Number)r.get("Id")).intValue()==4);
        return options(subsidiary
                ?jdbcTemplate.queryForList("EXEC dbo.USP_GetVendorsAndCustomers @OrganizationId=?,@CompanyId=?,@PartyTypeId=1",orgId,compId)
                :jdbcTemplate.queryForList("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadByOrganizationIdCompanyIdForBinding'",orgId,compId),"Id","CompanyName");
    }

    public List<Map<String,Object>> getCities(Integer orgId,Integer compId) {
        return options(jdbcTemplate.queryForList("EXEC dbo.Sp_City_GetAllMethod @OrganizationId=?,@CompanyId=?,@MethodType='GetAll'",orgId,compId),"Id","CityName");
    }
    public List<Map<String,Object>> getVehicleTypes(Integer orgId,Integer compId) {
        return options(jdbcTemplate.queryForList("EXEC dbo.Sp_VehicleType_GetAllMethod"),"Id","VehicleDescription");
    }
    public List<Map<String,Object>> getGatePassTypes(Integer orgId,Integer compId) {
        return options(jdbcTemplate.queryForList("EXEC dbo.Sp_GatePassType_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='Inward'",orgId,compId),"Id","GpTypeDescription");
    }
    public List<Map<String,Object>> getOrderTypes(Integer orgId,Integer compId,Integer branchId) {
        return options(jdbcTemplate.queryForList("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@Activity='GetOrderTypeForGpInward'",orgId,compId,branchId),"Id","OrderType");
    }

    public List<Map<String,Object>> getItems(Integer orgId,Integer compId) {
        return options(jdbcTemplate.queryForList("EXEC dbo.Sp_Item_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAllItemsIncludedPM'",orgId,compId),"Id","ItemName");
    }

    // InwardGatePass.cmbWeighBridgeFill(): fixed workflow choices, not scale records.
    public List<Map<String,Object>> getWeighBridges() {
        return List.of(Map.of("id",1,"name","Auto"),Map.of("id",2,"name","Mannual"));
    }
    public List<Map<String,Object>> getPackingTypes() {
        return options(jdbcTemplate.queryForList("EXEC dbo.Sp_InvPackingType_GetAllMethod @Activity='ReadAll'"),"Id","PackTypeDesc");
    }

    // InwardGatePass.PreBillNoFill: supplier dispatches, not the vehicle catalogue.
    public List<Map<String,Object>> getTransitVehicles(int org,int company,int supplier,int order,int gatePass) {
        if (supplier<=0 && order<=0) return List.of();
        String sql="EXEC dbo.USP_SupplierDispatch_GetNoForGpandGrn @OrganizationId=?,@CompanyId=?,@SupplierId=?";
        List<Object> args=new ArrayList<>(List.of(org,company,supplier));
        if (gatePass>0) { sql+=",@GpRecId=?"; args.add(gatePass); }
        if (order>0) { sql+=",@OrderId=?"; args.add(order); }
        return options(jdbcTemplate.queryForList(sql,args.toArray()),"SupplierDispatchId","SupplierDispatchNo");
    }

    public List<Map<String,Object>> getWeighBridgeWeights(int org,int company,int gatePass) {
        return jdbcTemplate.queryForList("EXEC dbo.Sp_WbTransation_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@RefDocumentTypeId=51,@Activity='GetNetWeightFromWbTransactions'",org,company,gatePass);
    }

    public List<Map<String,Object>> getStatuses(int org,int company) {
        List<Map<String,Object>> rows=new ArrayList<>();
        for (String label:List.of("Open","Accepted","Rejected")) rows.add(Map.of("id",label,"name",label));
        boolean reserved=jdbcTemplate.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?,@CompanyId=?",org,company).stream()
                .anyMatch(r->r.get("Id") instanceof Number && ((Number)r.get("Id")).intValue()==21);
        if (reserved) rows.add(Map.of("id","Accept As Reserved","name","Accept As Reserved"));
        return rows;
    }

    public Map<String,Object> findDriverBio(int org,int company,String cnic,String cell) {
        if ((cnic==null || cnic.isBlank()) && (cell==null || cell.isBlank())) return null;
        // Desktop loads DriverBioInfo from the original driver catalogue and searches exact CNIC/cell.
        for (var row:jdbcTemplate.queryForList("EXEC dbo.USP_driverBiodata_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'",org,company)) {
            if (cnic!=null?cnic.trim().equals(Objects.toString(row.get("cnicNo"),"")):cell.trim().equals(Objects.toString(row.get("cellNo"),""))) {
                Map<String,Object> result=new LinkedHashMap<>();
                result.put("Id",row.get("driverBiodataId")); result.put("DriverName",row.get("driverName"));
                result.put("CnicNo",row.get("cnicNo")); result.put("DriverCellNo",row.get("cellNo"));
                result.put("WhatsappNo",row.get("whatsappNo")); result.put("AlternateCellNo",row.get("alternateCellNo"));
                result.put("FatherName",row.get("fatherName")); result.put("FatherCnicNo",row.get("fatherCnicNo"));
                return result;
            }
        }
        return null;
    }

    /**
     * InwardGatePass.OrderInformationComboFill(41): PurchaseOrder.GetDataForDropDownFromPurchaseOrder(Org, Company)
     * -> USP_GetDataForDropDownFromPurchaseOrder; rows with Activity "DocumentType" feed CmbDocumentTypePoInfo and
     * rows with Activity "Supplier" feed cmbSupplierNamePoInfo. For 1500 the desktop calls the Steel overload with
     * (OrganizationId, OrganizationId) - reproduced.
     */
    public Map<String,List<Map<String,Object>>> getPoInfoCombos(int org,int company,int poInfoDocumentTypeId) {
        Map<String,List<Map<String,Object>>> result=new HashMap<>();
        List<Map<String,Object>> types=new ArrayList<>(), suppliers=new ArrayList<>();
        result.put("documentTypes",types); result.put("suppliers",suppliers);
        if (poInfoDocumentTypeId!=41 && poInfoDocumentTypeId!=1500) return result;
        int companyArg=poInfoDocumentTypeId==1500?org:company;
        for (Map<String,Object> row:jdbcTemplate.queryForList("EXEC dbo.USP_GetDataForDropDownFromPurchaseOrder @OrganizationId=?,@CompanyId=?",org,companyArg)) {
            String activity=Objects.toString(row.get("Activity"),"");
            Map<String,Object> option=new LinkedHashMap<>(); option.put("id",row.get("Id")); option.put("name",row.get("ReferenceName"));
            if ("DocumentType".equals(activity)) types.add(option);
            if ("Supplier".equals(activity)) suppliers.add(option);
        }
        return result;
    }

    /** InwardGatePass.HistoryComboFill: GetDataForDropDownFromGPI(OrganizationId, CompanyId, "Supplier", BranchesId) - the
     *  BLL signature is (CompanyId, OrganizationId, ...), so the desktop binds them swapped; reproduced. */
    public List<Map<String,Object>> getHistorySuppliers(int org,int company,int branch) {
        return options(jdbcTemplate.queryForList("EXEC dbo.USP_GetDataForDropDownFromGPI @OrganizationId=?,@CompanyId=?,@Activity='Supplier',@BranchesIds=?",
                company,org,String.valueOf(branch)),"Id","ReferenceName");
    }

    /** BindAllSupplierCustomer: clsGlobalVariables.globalAllSupplierCustomer (USP_GetVendorsAndCustomersWithCityName) where !IsSubSupCust. */
    public List<Map<String,Object>> getAllSupplierCustomers(int org,int company) {
        List<Map<String,Object>> result=new ArrayList<>();
        for (Map<String,Object> row:jdbcTemplate.queryForList("EXEC dbo.USP_GetVendorsAndCustomersWithCityName @OrganizationId=?,@CompanyId=?",org,company)) {
            Object sub=row.get("IsSubSupCust");
            if (Boolean.TRUE.equals(sub) || "1".equals(Objects.toString(sub,""))) continue;
            result.add(Map.of("id",row.get("Id"),"name",Objects.toString(row.get("CompanyName"),"")));
        }
        return result;
    }

    /** BindSaleInvoiceSupplierCustomer: InvSaleInvoice.GetPartiesFromSaleInvoiceWithGlAccount(Org, Company, "95,99,186"). */
    public List<Map<String,Object>> getSaleInvoiceParties(int org,int company) {
        return options(jdbcTemplate.queryForList("EXEC dbo.USP_GetPartiesFromSaleInvoiceWithGlAccount @OrganizationId=?,@CompanyId=?,@DocumentTypeIds=?",org,company,"95,99,186"),"Id","CompanyName");
    }

    /** LabDataGetByGpId: GatePassInward.GetDataByGpId -> Sp_GatePassInward_GetAllMethod @Activity='GetDataByGpId'. */
    public Map<String,Object> getLabData(int org,int company,int branch,int gatePassId) {
        String sql="EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@Id=?,@Activity='GetDataByGpId'";
        List<Object> args=new ArrayList<>(List.of(org,company,gatePassId));
        if (branch!=0) { sql+=",@BranchesId=?"; args.add(branch); }
        List<Map<String,Object>> rows=jdbcTemplate.queryForList(sql,args.toArray());
        return rows.isEmpty()?null:rows.get(0);
    }

    /** GatePassInward.GetAccessWeightByWeightComparedToPoWtAndGpId -> USP_GatePassInward_GetPoAccessWeightByGpId (GpId only when > 0). */
    public Map<String,Object> getPoAccessWeight(int org,int company,String gatepassType,int purchaseOrderId,int refDocumentTypeId,double weightCompared,double compareWeight,int gatePassId) {
        String sql="EXEC dbo.USP_GatePassInward_GetPoAccessWeightByGpId @OrganizationId=?,@CompanyId=?,@GatepassType=?,@PurchaseOrderId=?,@RefDocumentTypeId=?,@WeightComparedToPoWt=?,@FactoryWeight=?";
        List<Object> args=new ArrayList<>(List.of(org,company));
        args.add(new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.NVARCHAR,gatepassType));
        args.addAll(List.of(purchaseOrderId,refDocumentTypeId,weightCompared,compareWeight));
        if (gatePassId>0) { sql+=",@GpId=?"; args.add(gatePassId); }
        List<Map<String,Object>> rows=jdbcTemplate.queryForList(sql,args.toArray());
        return rows.isEmpty()?null:rows.get(0);
    }

    /** CmbOrderno_Leave for 1500: PurchaseOrder.GetPurchaseOrderForGatePassInwardByOrderId -> [ST].[USP_GetPurchaseOrderForGPIByOrderId]. */
    public List<Map<String,Object>> getSteelOrderPartyItems(int org,int company,int year,int number,String date) {
        String sql="EXEC [ST].[USP_GetPurchaseOrderForGPIByOrderId] @OrganizationId=?,@CompanyId=?,@DocumentTypeId=1500,@OrderNo=?,@FinancialYearId=?";
        List<Object> args=new ArrayList<>(List.of(org,company,number,year));
        if (date!=null && !date.isBlank()) { sql+=",@GpDate=?"; args.add(new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.DATE,date(date))); }
        return jdbcTemplate.queryForList(sql,args.toArray());
    }

    public List<Map<String,Object>> getPoInfoGrid(Integer orgId, Integer compId, Integer branchId, Integer yearId,
            String fromDate, String toDate, Double fromDocNo, Double toDocNo,
            Integer supplierId, Integer documentTypeId, Integer expiryDays, String dateField) {

        List<Object> args = new ArrayList<>();
        StringBuilder sql = new StringBuilder("EXEC dbo.usp_GetPurchaseOrderInformationForGatepassInward ");
        sql.append("@OrganizationId=?, @CompanyId=?, @Ids='41,700', @BranchesId=?, @FinancialYearId=?");
        args.add(orgId);
        args.add(compId);
        args.add(branchId);
        args.add(yearId);

        String fromParam = "@DocDateFrom", toParam = "@DocDateTo";
        if ("entryDate".equals(dateField)) { fromParam = "@EntryFromDate"; toParam = "@EntryToDate"; }
        else if ("modifyDate".equals(dateField)) { fromParam = "@ModifyFromDate"; toParam = "@ModifyToDate"; }
        else if ("approvedDate".equals(dateField)) { fromParam = "@ApprovedFromDate"; toParam = "@ApprovedToDate"; }

        if (fromDate != null && !fromDate.isBlank()) {
            sql.append(", ").append(fromParam).append("=?");
            args.add(new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.DATE, date(fromDate)));
        }
        if (toDate != null && !toDate.isBlank()) {
            sql.append(", ").append(toParam).append("=?");
            args.add(new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.DATE, date(toDate)));
        }
        if (fromDocNo != null && fromDocNo > 0) {
            sql.append(", @FromDocNo=?");
            args.add(fromDocNo.intValue());
        }
        if (toDocNo != null && toDocNo > 0) {
            sql.append(", @ToDocNo=?");
            args.add(toDocNo.intValue());
        }
        if (supplierId != null && supplierId > 0) {
            sql.append(", @OrderSupCustId=?");
            args.add(supplierId);
        }
        if (documentTypeId != null && documentTypeId > 0) {
            sql.append(", @DocumentTypeId=?");
            args.add(documentTypeId);
        }
        if (expiryDays != null && expiryDays > 0) {
            sql.append(", @ExpiryDays=?");
            args.add(expiryDays);
        }

        return jdbcTemplate.queryForList(sql.toString(), args.toArray());
    }

    /** SupplierCustomer.GetSupplierByPurchaseOrderNo (BLL 0600): Sp_SupplierCustomer_GetAllMethod @Activity='SupplierByPurchaseOrderNo'. */
    public List<Map<String,Object>> getOrderPartyItems(int org,int company,int branch,int year,int documentTypeId,int number,String date,int gatePassId) {
        String sql="EXEC dbo.Sp_SupplierCustomer_GetAllMethod @PurchaseOrderId=?,@OrganizationId=?,@CompanyId=?,@DocumentTypeId=?";
        List<Object> args=new ArrayList<>(List.of(number,org,company,documentTypeId));
        if (year!=0) { sql+=",@FinancialYearId=?"; args.add(year); }
        if (branch!=0) { sql+=",@BranchesId=?"; args.add(branch); }
        if (gatePassId!=0) { sql+=",@GpId=?"; args.add(gatePassId); }
        if (date!=null && !date.isBlank()) { sql+=",@GpDate=?"; args.add(new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.DATE,date(date))); }
        return jdbcTemplate.queryForList(sql+",@Activity='SupplierByPurchaseOrderNo'",args.toArray());
    }

    /** clsGlobalVariables.configrationsAllocation lookup by ConfigDescription (ConfigKey, "" when not allocated). */
    public String config(int org,int company,String description) {
        List<Map<String,Object>> rows=jdbcTemplate.queryForList("EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?,@CompanyId=?,@ConfigDescription=?,@Activity='GetConfigurationByOrgCompandConfigDescription'",org,company,description);
        return rows.isEmpty() || rows.get(0).get("ConfigKey")==null?"":String.valueOf(rows.get(0).get("ConfigKey")).trim();
    }
}
