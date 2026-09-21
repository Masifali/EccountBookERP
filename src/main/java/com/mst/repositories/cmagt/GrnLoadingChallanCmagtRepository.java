package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.GrnLoadingChallanCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.*;

@Repository
public class GrnLoadingChallanCmagtRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    private static final SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd");

    public Map<String, Object> saveOrUpdate(GrnLoadingChallanCmagtDto dto) {
        Map<String, Object> result = new HashMap<>();
        try {
            SimpleJdbcCall masterCall = new SimpleJdbcCall(jdbcTemplate)
                    .withSchemaName("cmagt")
                    .withProcedureName("USP_grnSupplierLoadingMaster_InsertAndUpdate");

            MapSqlParameterSource masterParams = new MapSqlParameterSource();
            masterParams.addValue("grnSupplierLoadingMasterId", dto.getGrnSupplierLoadingMasterId() != null ? dto.getGrnSupplierLoadingMasterId() : 0);
            masterParams.addValue("docNo", dto.getDocNo() != null ? dto.getDocNo() : 0);
            masterParams.addValue("docDate", parseDate(dto.getDocDate()));
            masterParams.addValue("purchaseOrderMasterId", dto.getPurchaseOrderMasterId() != null ? dto.getPurchaseOrderMasterId() : 0);
            masterParams.addValue("supplierId", dto.getSupplierId() != null ? dto.getSupplierId() : 0);
            masterParams.addValue("commissionAgentId", dto.getCommissionAgentId() != null ? dto.getCommissionAgentId() : 0);
            masterParams.addValue("vehicleNo", dto.getVehicleNo() != null ? dto.getVehicleNo() : "");
            masterParams.addValue("biltyNo", dto.getBiltyNo() != null ? dto.getBiltyNo() : "");
            masterParams.addValue("driverName", dto.getDriverName() != null ? dto.getDriverName() : "");
            masterParams.addValue("driverMobileNo", dto.getDriverMobileNo() != null ? dto.getDriverMobileNo() : "");
            masterParams.addValue("remarksHeader", dto.getRemarksHeader() != null ? dto.getRemarksHeader() : "");

            masterParams.addValue("organizationId", dto.getOrganizationId() != null ? dto.getOrganizationId() : 1);
            masterParams.addValue("companyId", dto.getCompanyId() != null ? dto.getCompanyId() : 1);
            masterParams.addValue("branchId", dto.getBranchId() != null ? dto.getBranchId() : 1);
            masterParams.addValue("financialYearId", dto.getFinancialYearId() != null ? dto.getFinancialYearId() : 1);
            masterParams.addValue("entryUserId", dto.getEntryUserId() != null ? dto.getEntryUserId() : 1);
            masterParams.addValue("modifyUserId", dto.getModifyUserId() != null ? dto.getModifyUserId() : 1);
            masterParams.addValue("documentTypeId", 1054);

            Map<String, Object> masterOut = masterCall.execute(masterParams);
            Integer masterId = extractReturnedId(masterOut);
            if (masterId == null || masterId <= 0) {
                masterId = dto.getGrnSupplierLoadingMasterId();
            }

            if (dto.getGrnSupplierLoadingDetailList() != null) {
                for (GrnLoadingChallanCmagtDto.DetailDto det : dto.getGrnSupplierLoadingDetailList()) {
                    SimpleJdbcCall detCall = new SimpleJdbcCall(jdbcTemplate)
                            .withSchemaName("cmagt")
                            .withProcedureName("USP_grnSupplierLoadingDetail_Insert");

                    MapSqlParameterSource detParams = new MapSqlParameterSource();
                    detParams.addValue("grnSupplierLoadingDetailId", det.getGrnSupplierLoadingDetailId() != null ? det.getGrnSupplierLoadingDetailId() : 0);
                    detParams.addValue("grnSupplierLoadingMasterId", masterId);
                    detParams.addValue("purchaseOrderDetailId", det.getPurchaseOrderDetailId() != null ? det.getPurchaseOrderDetailId() : 0);
                    detParams.addValue("itemId", det.getItemId() != null ? det.getItemId() : 0);
                    detParams.addValue("cropYearId", det.getCropYearId() != null ? det.getCropYearId() : 0);
                    detParams.addValue("packTypeId", det.getPackTypeId() != null ? det.getPackTypeId() : 0);
                    detParams.addValue("weight", det.getWeight() != null ? det.getWeight() : BigDecimal.ZERO);
                    detParams.addValue("qty", det.getQty() != null ? det.getQty() : BigDecimal.ZERO);
                    detParams.addValue("rate", det.getRate() != null ? det.getRate() : BigDecimal.ZERO);
                    detParams.addValue("amount", det.getAmount() != null ? det.getAmount() : BigDecimal.ZERO);
                    detParams.addValue("remarksDetail", det.getRemarksDetail() != null ? det.getRemarksDetail() : "");

                    detCall.execute(detParams);
                }
            }

            result.put("status", "SUCCESS");
            result.put("message", "GRN Loading Challan saved successfully!");
            result.put("id", masterId);
        } catch (Exception e) {
            result.put("status", "ERROR");
            result.put("message", e.getMessage());
        }
        return result;
    }

    public List<Map<String, Object>> getHistory(Integer companyId, Integer organizationId, String fromDate, String toDate) {
        SimpleJdbcCall call = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("usp_grnSupplierLoadingMaster_GetAllMethod");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("Activity", "ReadBySearch_grnSupplierLoadingMaster");
        params.addValue("CompanyId", companyId != null ? companyId : 1);
        params.addValue("OrganizationId", organizationId != null ? organizationId : 1);
        params.addValue("DocumentTypeId", 1054);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public Map<String, Object> getById(Integer id) {
        Map<String, Object> result = new HashMap<>();

        SimpleJdbcCall headerCall = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("usp_grnSupplierLoadingMaster_GetAllMethod");

        MapSqlParameterSource headerParams = new MapSqlParameterSource();
        headerParams.addValue("Activity", "ReadById_grnSupplierLoadingMaster");
        headerParams.addValue("Id", id);

        Map<String, Object> headerOut = headerCall.execute(headerParams);
        List<Map<String, Object>> headers = extractList(headerOut);

        if (headers != null && !headers.isEmpty()) {
            Map<String, Object> header = new HashMap<>(headers.get(0));

            SimpleJdbcCall detCall = new SimpleJdbcCall(jdbcTemplate)
                    .withSchemaName("cmagt")
                    .withProcedureName("usp_grnSupplierLoadingMaster_GetAllMethod");
            MapSqlParameterSource detParams = new MapSqlParameterSource();
            detParams.addValue("Activity", "ReadDetailByHeaderId");
            detParams.addValue("Id", id);
            header.put("grnSupplierLoadingDetailList", extractList(detCall.execute(detParams)));

            result.put("status", "SUCCESS");
            result.put("data", header);
        } else {
            result.put("status", "ERROR");
            result.put("message", "Record not found");
        }
        return result;
    }

    @SuppressWarnings("unchecked")
    private List<Map<String, Object>> extractList(Map<String, Object> out) {
        for (Object val : out.values()) {
            if (val instanceof List) {
                return (List<Map<String, Object>>) val;
            }
        }
        return Collections.emptyList();
    }

    private Integer extractReturnedId(Map<String, Object> out) {
        for (Object val : out.values()) {
            if (val instanceof Integer) return (Integer) val;
            if (val instanceof Number) return ((Number) val).intValue();
        }
        return null;
    }

    private Date parseDate(String dateStr) {
        if (dateStr == null || dateStr.trim().isEmpty()) return new Date();
        try {
            return DATE_FORMAT.parse(dateStr);
        } catch (Exception e) {
            return new Date();
        }
    }
}
