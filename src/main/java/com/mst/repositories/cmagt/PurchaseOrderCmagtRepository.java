package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.PurchaseOrderCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.util.*;

@Repository
public class PurchaseOrderCmagtRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    private static final SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd");

    public Map<String, Object> saveOrUpdate(PurchaseOrderCmagtDto dto) {
        Map<String, Object> result = new HashMap<>();
        try {
            SimpleJdbcCall masterCall = new SimpleJdbcCall(jdbcTemplate)
                    .withSchemaName("cmagt")
                    .withProcedureName("USP_purchaseOrderMaster_InsertAndUpdate");

            MapSqlParameterSource masterParams = new MapSqlParameterSource();
            masterParams.addValue("purchaseOrderMasterId", dto.getPurchaseOrderMasterId() != null ? dto.getPurchaseOrderMasterId() : 0);
            masterParams.addValue("docNo", dto.getDocNo() != null ? dto.getDocNo() : 0);
            masterParams.addValue("docDate", parseDate(dto.getDocDate()));
            masterParams.addValue("deliveryStartDate", parseDate(dto.getDeliveryStartDate()));
            masterParams.addValue("ValidityDate", parseDate(dto.getValidityDate()));
            masterParams.addValue("deliveryDays", dto.getDeliveryDays() != null ? dto.getDeliveryDays() : 0);
            masterParams.addValue("supplierId", dto.getSupplierId() != null ? dto.getSupplierId() : 0);
            masterParams.addValue("commissionAgentId", dto.getCommissionAgentId() != null ? dto.getCommissionAgentId() : 0);
            masterParams.addValue("deliveryTermId", dto.getDeliveryTermId() != null ? dto.getDeliveryTermId() : 0);
            masterParams.addValue("isSupplierOtherChargesAllowed", dto.getIsSupplierOtherChargesAllowed() != null ? dto.getIsSupplierOtherChargesAllowed() : false);
            masterParams.addValue("isWhtApplied", dto.getIsWhtApplied() != null ? dto.getIsWhtApplied() : false);
            masterParams.addValue("EBWeightDeductionTermId", dto.getEbWeightDeductionTermId() != null ? dto.getEbWeightDeductionTermId() : 0);
            masterParams.addValue("remarksHeader", dto.getRemarksHeader() != null ? dto.getRemarksHeader() : "");
            masterParams.addValue("shipToAddressId", dto.getShipToAddressId() != null ? dto.getShipToAddressId() : 0);
            masterParams.addValue("DeliveryToPartyId", dto.getDeliveryToPartyId() != null ? dto.getDeliveryToPartyId() : 0);
            masterParams.addValue("ShipToAddress", dto.getShipToAddress() != null ? dto.getShipToAddress() : "");
            masterParams.addValue("paymentScheduleDescription", dto.getPaymentScheduleDescription() != null ? dto.getPaymentScheduleDescription() : "");

            masterParams.addValue("organizationId", dto.getOrganizationId() != null ? dto.getOrganizationId() : 1);
            masterParams.addValue("companyId", dto.getCompanyId() != null ? dto.getCompanyId() : 1);
            masterParams.addValue("branchId", dto.getBranchId() != null ? dto.getBranchId() : 1);
            masterParams.addValue("financialYearId", dto.getFinancialYearId() != null ? dto.getFinancialYearId() : 1);
            masterParams.addValue("entryUserId", dto.getEntryUserId() != null ? dto.getEntryUserId() : 1);
            masterParams.addValue("modifyUserId", dto.getModifyUserId() != null ? dto.getModifyUserId() : 1);
            masterParams.addValue("documentTypeId", 1052);

            Map<String, Object> masterOut = masterCall.execute(masterParams);
            Integer masterId = extractReturnedId(masterOut);
            if (masterId == null || masterId <= 0) {
                masterId = dto.getPurchaseOrderMasterId();
            }

            // Details
            if (dto.getPurchaseOrderDetailList() != null) {
                for (PurchaseOrderCmagtDto.ItemDetailDto det : dto.getPurchaseOrderDetailList()) {
                    SimpleJdbcCall detCall = new SimpleJdbcCall(jdbcTemplate)
                            .withSchemaName("cmagt")
                            .withProcedureName("USP_purchaseOrderDetail_Insert");

                    MapSqlParameterSource detParams = new MapSqlParameterSource();
                    detParams.addValue("purchaseOrderDetailId", det.getPurchaseOrderDetailId() != null ? det.getPurchaseOrderDetailId() : 0);
                    detParams.addValue("purchaseOrderMasterId", masterId);
                    detParams.addValue("itemId", det.getItemId() != null ? det.getItemId() : 0);
                    detParams.addValue("cropYearId", det.getCropYearId() != null ? det.getCropYearId() : 0);
                    detParams.addValue("packTypeId", det.getPackTypeId() != null ? det.getPackTypeId() : 0);
                    detParams.addValue("weight", det.getWeight() != null ? det.getWeight() : BigDecimal.ZERO);
                    detParams.addValue("qty", det.getQty() != null ? det.getQty() : BigDecimal.ZERO);
                    detParams.addValue("packUomId", det.getPackUomId() != null ? det.getPackUomId() : 0);
                    detParams.addValue("rate", det.getRate() != null ? det.getRate() : BigDecimal.ZERO);
                    detParams.addValue("rateUomId", det.getRateUomId() != null ? det.getRateUomId() : 0);
                    detParams.addValue("amount", det.getAmount() != null ? det.getAmount() : BigDecimal.ZERO);
                    detParams.addValue("taxId", det.getTaxId() != null ? det.getTaxId() : 0);
                    detParams.addValue("taxPercent", det.getTaxPercent() != null ? det.getTaxPercent() : BigDecimal.ZERO);
                    detParams.addValue("taxAmount", det.getTaxAmount() != null ? det.getTaxAmount() : BigDecimal.ZERO);
                    detParams.addValue("totalAmount", det.getTotalAmount() != null ? det.getTotalAmount() : BigDecimal.ZERO);
                    detParams.addValue("remarksDetail", det.getRemarksDetail() != null ? det.getRemarksDetail() : "");

                    detParams.addValue("commissionTypeId", det.getCommissionTypeId() != null ? det.getCommissionTypeId() : 0);
                    detParams.addValue("commissionRate", det.getCommissionRate() != null ? det.getCommissionRate() : BigDecimal.ZERO);
                    detParams.addValue("commissionRateUomId", det.getCommissionRateUomId() != null ? det.getCommissionRateUomId() : 0);
                    detParams.addValue("commissionAmount", det.getCommissionAmount() != null ? det.getCommissionAmount() : BigDecimal.ZERO);

                    detParams.addValue("brokeryTypeId", det.getBrokeryTypeId() != null ? det.getBrokeryTypeId() : 0);
                    detParams.addValue("brokeryRate", det.getBrokeryRate() != null ? det.getBrokeryRate() : BigDecimal.ZERO);
                    detParams.addValue("brokeryRateUomId", det.getBrokeryRateUomId() != null ? det.getBrokeryRateUomId() : 0);
                    detParams.addValue("brokeryAmount", det.getBrokeryAmount() != null ? det.getBrokeryAmount() : BigDecimal.ZERO);

                    detCall.execute(detParams);
                }
            }

            result.put("status", "SUCCESS");
            result.put("message", "Purchase Order saved successfully!");
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
                .withProcedureName("USP_purchaseOrderMaster_GetAllMethod");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("Activity", "ReadBySearch_purchaseOrderMaster");
        params.addValue("CompanyId", companyId != null ? companyId : 1);
        params.addValue("OrganizationId", organizationId != null ? organizationId : 1);
        params.addValue("DocumentTypeId", 1052);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public Map<String, Object> getById(Integer id) {
        Map<String, Object> result = new HashMap<>();

        SimpleJdbcCall headerCall = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("USP_purchaseOrderMaster_GetAllMethod");

        MapSqlParameterSource headerParams = new MapSqlParameterSource();
        headerParams.addValue("Activity", "ReadById_purchaseOrderMaster");
        headerParams.addValue("Id", id);

        Map<String, Object> headerOut = headerCall.execute(headerParams);
        List<Map<String, Object>> headers = extractList(headerOut);

        if (headers != null && !headers.isEmpty()) {
            Map<String, Object> header = new HashMap<>(headers.get(0));

            // Details
            SimpleJdbcCall detCall = new SimpleJdbcCall(jdbcTemplate)
                    .withSchemaName("cmagt")
                    .withProcedureName("USP_purchaseOrderMaster_GetAllMethod");
            MapSqlParameterSource detParams = new MapSqlParameterSource();
            detParams.addValue("Activity", "ReadByHeaderId_purchaseOrderDetail");
            detParams.addValue("Id", id);
            header.put("purchaseOrderDetailList", extractList(detCall.execute(detParams)));

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
