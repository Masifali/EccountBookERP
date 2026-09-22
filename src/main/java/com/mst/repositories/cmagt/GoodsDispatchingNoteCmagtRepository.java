package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.GoodsDispatchingNoteCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.*;

@Repository
public class GoodsDispatchingNoteCmagtRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    private static final SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd");

    public Map<String, Object> saveOrUpdate(GoodsDispatchingNoteCmagtDto dto) {
        Map<String, Object> result = new HashMap<>();
        try {
            SimpleJdbcCall masterCall = new SimpleJdbcCall(jdbcTemplate)
                    .withSchemaName("cmagt")
                    .withProcedureName("USP_gdnBuyerDispatchMaster_InsertAndUpdate");

            MapSqlParameterSource masterParams = new MapSqlParameterSource();
            masterParams.addValue("gdnBuyerDispatchMasterId", dto.getGdnBuyerDispatchMasterId() != null ? dto.getGdnBuyerDispatchMasterId() : 0);
            masterParams.addValue("docNo", dto.getDocNo() != null ? dto.getDocNo() : 0);
            masterParams.addValue("docDate", parseDate(dto.getDocDate()));
            masterParams.addValue("saleOrderMasterId", dto.getSaleOrderMasterId() != null ? dto.getSaleOrderMasterId() : 0);
            masterParams.addValue("buyerId", dto.getBuyerId() != null ? dto.getBuyerId() : 0);
            masterParams.addValue("commissionAgentId", dto.getCommissionAgentId() != null ? dto.getCommissionAgentId() : 0);
            masterParams.addValue("vehicleNo", dto.getVehicleNo() != null ? dto.getVehicleNo() : "");
            masterParams.addValue("biltyNo", dto.getBiltyNo() != null ? dto.getBiltyNo() : "");
            masterParams.addValue("driverName", dto.getDriverName() != null ? dto.getDriverName() : "");
            masterParams.addValue("driverMobileNo", dto.getDriverMobileNo() != null ? dto.getDriverMobileNo() : "");
            masterParams.addValue("remarksHeader", dto.getRemarksHeader() != null ? dto.getRemarksHeader() : "");

            /* Tenancy and the user are set by the service from the session before this runs.
               They used to fall back to 1 here - the same fabricated default that made the
               Purchase Order screen read company 1 - so a payload that omitted them wrote a
               document into another company. No fallback now: the service guarantees them. */
            masterParams.addValue("organizationId", dto.getOrganizationId());
            masterParams.addValue("companyId", dto.getCompanyId());
            masterParams.addValue("branchId", dto.getBranchId());
            masterParams.addValue("financialYearId", dto.getFinancialYearId());
            masterParams.addValue("entryUserId", dto.getEntryUserId());
            masterParams.addValue("modifyUserId", dto.getModifyUserId());
            masterParams.addValue("documentTypeId", 1055);

            /* btnSave_Click :1877-1917 - the rest of the header. Omitting a parameter on a
               SimpleJdbcCall lets the procedure apply its own default, which is why freight,
               weights, transporter, both cities, the delivery term and the vehicle type were
               silently stored as 0/NULL on every web-saved GDN. */
            masterParams.addValue("BuyerRefDocNo",      nz(dto.getBuyerRefDocNo()));
            masterParams.addValue("DeliverToPartyId",   nzi(dto.getDeliverToPartyId()));
            masterParams.addValue("DeliverToPartyName", nz(dto.getDeliverToPartyName()));
            masterParams.addValue("DeliverToAddressId", nzi(dto.getDeliverToAddressId()));
            masterParams.addValue("DeliverToAddress",   nz(dto.getDeliverToAddress()));
            masterParams.addValue("loadingCityId",      nzi(dto.getLoadingCityId()));
            masterParams.addValue("unloadingCityId",    nzi(dto.getUnloadingCityId()));
            masterParams.addValue("transporterId",      nzi(dto.getTransporterId()));
            masterParams.addValue("transporterName",    nz(dto.getTransporterName()));
            masterParams.addValue("biltyFreight",       nzd(dto.getBiltyFreight()));
            masterParams.addValue("otherAdLesCharges",  nzd(dto.getOtherAdLesCharges()));
            masterParams.addValue("totalFreight",       nzd(dto.getTotalFreight()));
            masterParams.addValue("FreightRemarks",     nz(dto.getFreightRemarks()));
            masterParams.addValue("vehicleTypeId",      nzi(dto.getVehicleTypeId()));
            masterParams.addValue("biltyDate",          parseDate(dto.getBiltyDate()));
            masterParams.addValue("deliveryTermId",     nzi(dto.getDeliveryTermId()));
            masterParams.addValue("biltyQty",           nzd(dto.getBiltyQty()));
            masterParams.addValue("loadWeight",         nzd(dto.getLoadWeight()));
            masterParams.addValue("tareWeight",         nzd(dto.getTareWeight()));
            masterParams.addValue("scaleNetWeight",     nzd(dto.getScaleNetWeight()));
            masterParams.addValue("BillWeight",         nzd(dto.getBillWeight()));
            masterParams.addValue("warningRemarks",     nz(dto.getWarningRemarks()));

            Map<String, Object> masterOut = masterCall.execute(masterParams);
            Integer masterId = extractReturnedId(masterOut);
            if (masterId == null || masterId <= 0) {
                masterId = dto.getGdnBuyerDispatchMasterId();
            }

            if (dto.getGdnBuyerDispatchDetailList() != null) {
                for (GoodsDispatchingNoteCmagtDto.DetailDto det : dto.getGdnBuyerDispatchDetailList()) {
                    SimpleJdbcCall detCall = new SimpleJdbcCall(jdbcTemplate)
                            .withSchemaName("cmagt")
                            .withProcedureName("USP_gdnBuyerDispatchDetail_Insert");

                    MapSqlParameterSource detParams = new MapSqlParameterSource();
                    detParams.addValue("gdnBuyerDispatchDetailId", det.getGdnBuyerDispatchDetailId() != null ? det.getGdnBuyerDispatchDetailId() : 0);
                    detParams.addValue("gdnBuyerDispatchMasterId", masterId);
                    detParams.addValue("saleOrderDetailId", det.getSaleOrderDetailId() != null ? det.getSaleOrderDetailId() : 0);
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
            result.put("message", "Goods Dispatching Note saved successfully!");
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
                .withProcedureName("usp_gdnBuyerDispatchMaster_GetAllMethod");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("Activity", "ReadBySearch_gdnBuyerDispatchMaster");
        /* The controller passes the session's own values. The ': 1' fallbacks that used to sit
           here would have quietly widened a read to company 1 if one ever arrived null. */
        params.addValue("CompanyId", companyId);
        params.addValue("OrganizationId", organizationId);
        params.addValue("DocumentTypeId", 1055);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public Map<String, Object> getById(Integer id) {
        Map<String, Object> result = new HashMap<>();

        SimpleJdbcCall headerCall = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("usp_gdnBuyerDispatchMaster_GetAllMethod");

        MapSqlParameterSource headerParams = new MapSqlParameterSource();
        headerParams.addValue("Activity", "ReadById_gdnBuyerDispatchMaster");
        headerParams.addValue("Id", id);

        Map<String, Object> headerOut = headerCall.execute(headerParams);
        List<Map<String, Object>> headers = extractList(headerOut);

        if (headers != null && !headers.isEmpty()) {
            Map<String, Object> header = new HashMap<>(headers.get(0));

            SimpleJdbcCall detCall = new SimpleJdbcCall(jdbcTemplate)
                    .withSchemaName("cmagt")
                    .withProcedureName("usp_gdnBuyerDispatchMaster_GetAllMethod");
            MapSqlParameterSource detParams = new MapSqlParameterSource();
            detParams.addValue("Activity", "ReadDetailByHeaderId");
            detParams.addValue("Id", id);
            header.put("gdnBuyerDispatchDetailList", extractList(detCall.execute(detParams)));

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

    private static String nz(String v)      { return v == null ? "" : v; }
    private static int    nzi(Integer v)    { return v == null ? 0 : v; }
    private static BigDecimal nzd(BigDecimal v) { return v == null ? BigDecimal.ZERO : v; }
}
