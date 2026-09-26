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

    // Exact non-virtual fields of desktop model 1015. Preserve fields not exposed by this form.
    public Integer saveHeader(InwardGatePass obj) {
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
            if (properties.containsKey(field)) {
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

    public Integer generateGpCode(Integer orgId,Integer compId,Integer branchId,Integer yearId,Integer docTypeId) {
        return ((Number)jdbcTemplate.queryForMap("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@DocumentTypeId=?,@Activity='GenerategpCode'",orgId,compId,branchId,yearId,docTypeId).get("GpSrNo")).intValue();
    }
    public Integer generateGpTypeCode(Integer orgId,Integer compId,Integer branchId,Integer yearId,String type) {
        return ((Number)jdbcTemplate.queryForMap("EXEC dbo.Sp_GatePassInward_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@GatepassType=?,@Activity='GenerateGPTypeCode'",orgId,compId,branchId,yearId,type).get("GpTypeSrNo")).intValue();
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
                positive(fromDocNo),positive(toDocNo),positive(supplierId),canViewAll,userId);
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

    public List<Map<String,Object>> getDocumentTypes(Integer orgId, Integer compId) {
        List<Map<String,Object>> list = new ArrayList<>();
        try {
            List<Map<String,Object>> rows = jdbcTemplate.queryForList(
                    "EXEC dbo.USP_GetDataForDropDownFromPurchaseOrder @OrganizationId=?, @CompanyId=?", orgId, compId);
            for (Map<String,Object> row : rows) {
                Object activity = row.get("Activity");
                if (activity != null && "DocumentType".equalsIgnoreCase(activity.toString().trim())) {
                    list.add(Map.of("id", row.get("Id"), "name", row.get("ReferenceName")));
                }
            }
        } catch (Exception e) {
            try {
                List<Map<String,Object>> rows = jdbcTemplate.queryForList(
                        "EXEC dbo.Sp_DocumentType_GetAllMethod @Activity='GetAll'");
                for (Map<String,Object> row : rows) {
                    list.add(Map.of("id", row.get("Id"), "name", row.get("Description") != null ? row.get("Description") : row.get("DocumentTypeName")));
                }
            } catch (Exception ignored) {}
        }
        if (list.isEmpty()) {
            list.add(Map.of("id", 41, "name", "Purchase Order"));
            list.add(Map.of("id", 700, "name", "Market Purchase Order"));
            list.add(Map.of("id", 1500, "name", "Purchase Order (Steel)"));
        }
        return list;
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

    public List<Map<String,Object>> getOrderPartyItems(int org,int company,int branch,int year,int number,String date,int gatePassId) {
        return jdbcTemplate.queryForList("EXEC dbo.Sp_SupplierCustomer_GetAllMethod @OrganizationId=?,@CompanyId=?,@BranchesId=?,@FinancialYearId=?,@PurchaseOrderId=?,@DocumentTypeId=41,@GpDate=?,@GpId=?,@Activity='SupplierByPurchaseOrderNo'",
                org,company,branch,year,number,new org.springframework.jdbc.core.SqlParameterValue(java.sql.Types.DATE,date(date)),positive(gatePassId));
    }
}
