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

    // Save or Update InwardGatePass Header
    public Integer saveHeader(InwardGatePass obj) {
        String procName = (obj.getId() == null || obj.getId() == 0) ? "Sp_GatePassInward_Insert" : "Sp_GatePassInward_Update";
        
        List<Object> params = new ArrayList<>();
        if (obj.getId() != null && obj.getId() > 0) {
            params.add(obj.getId());
        }

        String sql;
        if ("Sp_GatePassInward_Insert".equals(procName)) {
            sql = "EXEC Sp_GatePassInward_Insert " +
                    "@OrganizationId=?, @CompanyId=?, @BranchesId=?, @FinancialYearId=?, @DocumentTypeId=?, " +
                    "@GpDate=?, @GpSrNo=?, @GatepassType=?, @GpTypeSrNo=?, @SupplierCustomerId=?, @CityId=?, " +
                    "@ItemId=?, @PurchaseOrderId=?, @VehicleType=?, @VehicleNo=?, @BiltyNo=?, @BiltyDate=?, " +
                    "@Freight=?, @AdvanceByParty=?, @AdvanceByFactory=?, @FreightOn=?, @NetPaid=?, " +
                    "@SupplierFirstWeight=?, @SupplierSecondWeight=?, @SupplierWeight=?, @FactoryWeight=?, " +
                    "@DifferenceWeight=?, @InDateTimeStamp=?, @OutDateTimeStamp=?, @WeighBridgeId=?, @Status=?, " +
                    "@PackingTypeId=?, @AccessWeight=?, @PackUnit=?, @WeightComparedToPoWt=?, @NoOfPackages=?, " +
                    "@Container=?, @Container1=?, @OtherRemarks=?, @OtherSupCust=?, @SupplierContractCode=?, " +
                    "@VarietyName=?, @SupplierDispatchId=?, @WarehouseId=?, @RefDocumentEntryNo=?, " +
                    "@RefDocumentTypeId=?, @ActionIdForSpecialApproval=?, @DriverName=?, @DriverCNICNO=?, " +
                    "@DriverMobileNo=?, @WeightDiffComments=?, @driverBioDataId=?, @IsApproved=?, @PostState=?, " +
                    "@EntryDate=?, @EntryUser=?";
        } else {
            sql = "EXEC Sp_GatePassInward_Update " +
                    "@Id=?, @OrganizationId=?, @CompanyId=?, @BranchesId=?, @FinancialYearId=?, @DocumentTypeId=?, " +
                    "@GpDate=?, @GpSrNo=?, @GatepassType=?, @GpTypeSrNo=?, @SupplierCustomerId=?, @CityId=?, " +
                    "@ItemId=?, @PurchaseOrderId=?, @VehicleType=?, @VehicleNo=?, @BiltyNo=?, @BiltyDate=?, " +
                    "@Freight=?, @AdvanceByParty=?, @AdvanceByFactory=?, @FreightOn=?, @NetPaid=?, " +
                    "@SupplierFirstWeight=?, @SupplierSecondWeight=?, @SupplierWeight=?, @FactoryWeight=?, " +
                    "@DifferenceWeight=?, @InDateTimeStamp=?, @OutDateTimeStamp=?, @WeighBridgeId=?, @Status=?, " +
                    "@PackingTypeId=?, @AccessWeight=?, @PackUnit=?, @WeightComparedToPoWt=?, @NoOfPackages=?, " +
                    "@Container=?, @Container1=?, @OtherRemarks=?, @OtherSupCust=?, @SupplierContractCode=?, " +
                    "@VarietyName=?, @SupplierDispatchId=?, @WarehouseId=?, @RefDocumentEntryNo=?, " +
                    "@RefDocumentTypeId=?, @ActionIdForSpecialApproval=?, @DriverName=?, @DriverCNICNO=?, " +
                    "@DriverMobileNo=?, @WeightDiffComments=?, @driverBioDataId=?, @IsApproved=?, @PostState=?, " +
                    "@ModifyDate=?, @ModifyUser=?";
        }

        List<Map<String, Object>> result = jdbcTemplate.queryForList(sql,
                obj.getOrganizationId(), obj.getCompanyId(), obj.getBranchesId(), obj.getFinancialYearId(), obj.getDocumentTypeId(),
                obj.getGpDate(), obj.getGpSrNo(), obj.getGatepassType(), obj.getGpTypeSrNo(), obj.getSupplierCustomerId(), obj.getCityId(),
                obj.getItemId(), obj.getPurchaseOrderId(), obj.getVehicleType(), obj.getVehicleNo(), obj.getBiltyNo(), obj.getBiltyDate(),
                obj.getFreight(), obj.getAdvanceByParty(), obj.getAdvanceByFactory(), obj.getFreightOn(), obj.getNetPaid(),
                obj.getSupplierFirstWeight(), obj.getSupplierSecondWeight(), obj.getSupplierWeight(), obj.getFactoryWeight(),
                obj.getDifferenceWeight(), obj.getInDateTimeStamp(), obj.getOutDateTimeStamp(), obj.getWeighBridgeId(), obj.getStatus(),
                obj.getPackingTypeId(), obj.getAccessWeight(), obj.getPackUnit(), obj.getWeightComparedToPoWt(), obj.getNoOfPackages(),
                obj.getContainer(), obj.getContainer1(), obj.getOtherRemarks(), obj.getOtherSupCust(), obj.getSupplierContractCode(),
                obj.getVarietyName(), obj.getSupplierDispatchId(), obj.getWarehouseId(), obj.getRefDocumentEntryNo(),
                obj.getRefDocumentTypeId(), obj.getActionIdForSpecialApproval(), obj.getDriverName(), obj.getDriverCNICNO(),
                obj.getDriverMobileNo(), obj.getWeightDiffComments(), obj.getDriverBioDataId(), obj.getIsApproved(), obj.getPostState(),
                (obj.getId() == null || obj.getId() == 0) ? obj.getEntryDate() : obj.getModifyDate(),
                (obj.getId() == null || obj.getId() == 0) ? obj.getEntryUser() : obj.getModifyUser()
        );

        if (result != null && !result.isEmpty()) {
            Object idObj = result.get(0).get("Id");
            if (idObj == null) {
                idObj = result.get(0).get("ID");
            }
            if (idObj instanceof Number) {
                return ((Number) idObj).intValue();
            }
        }
        return obj.getId();
    }

    // Save Driver Biodata
    public Integer saveDriverBio(InwardGatePass obj) {
        try {
            String sql = "EXEC [dbo].[USP_DriverBiodata_InsertIfNotExists] " +
                    "@driverName=?, @cnicNo=?, @cellNo=?, @whatsappNo=?, @alternateCellNo=?, @fatherName=?, @fatherCnicNo=?, " +
                    "@OrganizationId=?, @CompanyId=?, @EntryUserId=?, @EntryDate=?, @BranchId=?, @ModifyDate=?, @ApprovedDate=?";
            List<Map<String, Object>> result = jdbcTemplate.queryForList(sql,
                    obj.getDriverName(), obj.getDriverCNICNO(), obj.getDriverMobileNo(), obj.getWhatsappNo(),
                    obj.getAlternateCellNo(), obj.getFatherName(), obj.getFatherCnicNo(),
                    obj.getOrganizationId(), obj.getCompanyId(), obj.getEntryUser(), obj.getEntryDate(),
                    obj.getBranchesId(), obj.getModifyDate(), obj.getPostDate()
            );
            if (result != null && !result.isEmpty()) {
                Object idObj = result.get(0).get("Id");
                if (idObj instanceof Number) {
                    return ((Number) idObj).intValue();
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 0;
    }

    // Save Detail Row
    public void saveDetail(InwardGatePassDetail detail) {
        String sql = "EXEC Sp_GatePassInwardDetail_Insert " +
                "@GatePassInwardId=?, @ItemId=?, @PackUnit=?, @Weight=?, @ItemUOMId=?, @ItemQty=?, @CropYear=?, " +
                "@JobLotId=?, @SupplyScheduleId=?, @WareHouseId=?, @PackingTypeId=?, @RefDocumentTypeId=?, " +
                "@SupplierCustomerId=?, @PurchaseOrderId=?, @PurchaseOrderDetailId=?, @CityId=?, @RemarksDetail=?";
        jdbcTemplate.update(sql,
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
        jdbcTemplate.update(sql,
                breakUp.getInwardGatePassId(), breakUp.getQty(), breakUp.getUom(), breakUp.getGrossWeight(),
                breakUp.getEbWeight(), breakUp.getEbTotal(), breakUp.getNetWeight()
        );
    }

    // Clear details before update
    public void deleteDetailsByHeaderId(Integer headerId) {
        String sql = "DELETE FROM GatePassInwardDetail WHERE GatePassInwardId = ?";
        jdbcTemplate.update(sql, headerId);
    }

    // Clear breakups before update
    public void deletePurchaseBreakUpsByHeaderId(Integer headerId) {
        String sql = "DELETE FROM GatePassInwardPurchaseBreakUp WHERE InwardGatePassId = ?";
        jdbcTemplate.update(sql, headerId);
    }

    // Delete full record
    public void deleteRecord(Integer id, Integer orgId, Integer compId) {
        String sql = "EXEC Sp_GatePassInward_Delete @Id=?, @OrganizationId=?, @CompanyId=?";
        jdbcTemplate.update(sql, id, orgId, compId);
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

    // Generate Document Code (gpSrNo)
    public Integer generateGpCode(Integer orgId, Integer compId, Integer branchId, Integer yearId, Integer docTypeId) {
        try {
            String sql = "EXEC Sp_GatePassInward_GetAllMethod @OrganizationId=?, @CompanyId=?, @BranchesId=?, @FinancialYearId=?, @DocumentTypeId=?, @Activity='GenerategpCode'";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, orgId, compId, branchId, yearId, docTypeId);
            if (list != null && !list.isEmpty()) {
                Object val = list.get(0).get("GpSrNo");
                if (val instanceof Number) {
                    return ((Number) val).intValue();
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 1;
    }

    // Generate GP Type Code (gpTypeSrNo)
    public Integer generateGpTypeCode(Integer orgId, Integer compId, Integer branchId, Integer yearId, String gatepassType) {
        try {
            String sql = "EXEC Sp_GatePassInward_GetAllMethod @OrganizationId=?, @CompanyId=?, @BranchesId=?, @FinancialYearId=?, @GatepassType=?, @Activity='GenerateGPTypeCode'";
            List<Map<String, Object>> list = jdbcTemplate.queryForList(sql, orgId, compId, branchId, yearId, gatepassType);
            if (list != null && !list.isEmpty()) {
                Object val = list.get(0).get("GpTypeSrNo");
                if (val instanceof Number) {
                    return ((Number) val).intValue();
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 1;
    }

    // History query
    public List<Map<String, Object>> getHistory(Integer orgId, Integer compId, Integer branchId, Integer yearId,
                                                Integer docTypeId, String fromDate, String toDate,
                                                Double fromDocNo, Double toDocNo, Integer supplierId) {
        StringBuilder sb = new StringBuilder("EXEC Sp_GatePassInward_GetAllMethod ");
        sb.append("@OrganizationId=").append(orgId);
        sb.append(", @CompanyId=").append(compId);
        if (branchId != null && branchId > 0) sb.append(", @BranchesId=").append(branchId);
        if (yearId != null && yearId > 0) sb.append(", @FinancialYearId=").append(yearId);
        if (docTypeId != null && docTypeId > 0) sb.append(", @DocumentTypeId=").append(docTypeId);
        if (fromDate != null && !fromDate.isEmpty()) sb.append(", @DateFrom='").append(fromDate).append("'");
        if (toDate != null && !toDate.isEmpty()) sb.append(", @DateTo='").append(toDate).append("'");
        if (fromDocNo != null && fromDocNo > 0) sb.append(", @DocNoFrom=").append(fromDocNo);
        if (toDocNo != null && toDocNo > 0) sb.append(", @DocNoTo=").append(toDocNo);
        if (supplierId != null && supplierId > 0) sb.append(", @SupplierCustomerId=").append(supplierId);
        sb.append(", @Activity='GatepassHistory'");

        return jdbcTemplate.queryForList(sb.toString());
    }

    // Dropdown loaders
    public List<Map<String, Object>> getSuppliers(Integer orgId, Integer compId) {
        try {
            String sql = "SELECT Id as id, CompanyName as name, SupCustCode as code FROM SupplierCustomer " +
                    "WHERE (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL) " +
                    "ORDER BY CompanyName";
            return jdbcTemplate.queryForList(sql, orgId, compId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getCities(Integer orgId, Integer compId) {
        try {
            String sql = "SELECT Id as id, Description as name FROM City " +
                    "WHERE (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL) " +
                    "ORDER BY Description";
            return jdbcTemplate.queryForList(sql, orgId, compId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getVehicleTypes(Integer orgId, Integer compId) {
        try {
            return jdbcTemplate.queryForList("EXEC Sp_VehicleType_GetAllMethod @Activity='ReadAll'");
        } catch (Exception e1) {
            try {
                return jdbcTemplate.queryForList("SELECT Id as id, VehicleDescription as name FROM VehicleType ORDER BY VehicleDescription");
            } catch (Exception e2) {
                try {
                    return jdbcTemplate.queryForList("SELECT Id as id, TypeDescription as name FROM VehicleType ORDER BY TypeDescription");
                } catch (Exception e3) {
                    return Collections.emptyList();
                }
            }
        }
    }

    public List<Map<String, Object>> getGatePassTypes() {
        try {
            return jdbcTemplate.queryForList("EXEC Sp_GatepassType_GetAllMethod @Activity='ReadAll'");
        } catch (Exception e1) {
            try {
                return jdbcTemplate.queryForList("SELECT Id as id, GpTypeDescription as name FROM GatepassType ORDER BY GpTypeDescription");
            } catch (Exception e2) {
                return Collections.emptyList();
            }
        }
    }

    public List<Map<String, Object>> getItems(Integer orgId, Integer compId) {
        try {
            String sql = "SELECT Id as id, ItemName as name, ItemCode as code FROM Item " +
                    "WHERE (OrganizationId = ? OR OrganizationId IS NULL) AND (CompanyId = ? OR CompanyId IS NULL) " +
                    "ORDER BY ItemName";
            return jdbcTemplate.queryForList(sql, orgId, compId);
        } catch (Exception e) {
            return Collections.emptyList();
        }
    }

    public List<Map<String, Object>> getWeighBridges() {
        try {
            String sql = "SELECT Id as id, ScaleName as name FROM WeighbridgeScales WHERE IsActive = 1 ORDER BY ScaleName";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            try {
                String sql = "SELECT Id as id, ScaleName as name FROM WeighbridgeScales ORDER BY ScaleName";
                return jdbcTemplate.queryForList(sql);
            } catch (Exception ex) {
                return Collections.emptyList();
            }
        }
    }

    public List<Map<String, Object>> getPackingTypes() {
        try {
            String sql = "SELECT Id as id, PackTypeDesc as name FROM InvPackingType WHERE IsActive = 1 ORDER BY PackTypeDesc";
            return jdbcTemplate.queryForList(sql);
        } catch (Exception e) {
            try {
                String sql = "SELECT Id as id, PackTypeDesc as name FROM InvPackingType ORDER BY PackTypeDesc";
                return jdbcTemplate.queryForList(sql);
            } catch (Exception ex) {
                return Collections.emptyList();
            }
        }
    }
}
