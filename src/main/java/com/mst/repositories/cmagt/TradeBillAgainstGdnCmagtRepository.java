package com.mst.repositories.cmagt;

import com.mst.models.cmagt.dto.TradeBillAgainstGdnCmagtDto;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.text.SimpleDateFormat;
import java.util.*;

@Repository
public class TradeBillAgainstGdnCmagtRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    private static final SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd");

    public Map<String, Object> saveOrUpdate(TradeBillAgainstGdnCmagtDto dto) {
        Map<String, Object> result = new HashMap<>();
        try {
            SimpleJdbcCall masterCall = new SimpleJdbcCall(jdbcTemplate)
                    .withProcedureName("USP_InvCommAgentTradeBill_InsertAndUpdate");

            MapSqlParameterSource masterParams = new MapSqlParameterSource();
            masterParams.addValue("Id", dto.getId() != null ? dto.getId() : 0);
            masterParams.addValue("DocNo", dto.getDocNo() != null ? dto.getDocNo() : 0);
            masterParams.addValue("DocDate", parseDate(dto.getDocDate()));
            masterParams.addValue("SupplierId", dto.getSupplierId() != null ? dto.getSupplierId() : 0);
            masterParams.addValue("BuyerId", dto.getBuyerId() != null ? dto.getBuyerId() : 0);
            masterParams.addValue("CommissionAgentId", dto.getCommissionAgentId() != null ? dto.getCommissionAgentId() : 0);
            masterParams.addValue("NetAmount", dto.getNetAmount() != null ? dto.getNetAmount() : BigDecimal.ZERO);
            masterParams.addValue("RemarksHeader", dto.getRemarksHeader() != null ? dto.getRemarksHeader() : "");

            masterParams.addValue("OrganizationId", dto.getOrganizationId() != null ? dto.getOrganizationId() : 1);
            masterParams.addValue("CompanyId", dto.getCompanyId() != null ? dto.getCompanyId() : 1);
            masterParams.addValue("BranchesId", dto.getBranchId() != null ? dto.getBranchId() : 1);
            masterParams.addValue("FinancialYearId", dto.getFinancialYearId() != null ? dto.getFinancialYearId() : 1);
            masterParams.addValue("EnteryUserId", dto.getEntryUserId() != null ? dto.getEntryUserId() : 1);
            masterParams.addValue("ModifyUserId", dto.getModifyUserId() != null ? dto.getModifyUserId() : 1);
            masterParams.addValue("DocumentTypeId", 1056);

            Map<String, Object> masterOut = masterCall.execute(masterParams);
            Integer masterId = extractReturnedId(masterOut);
            if (masterId == null || masterId <= 0) {
                masterId = dto.getId();
            }

            if (dto.getInvCommAgentTradeBillDetailslist() != null) {
                for (TradeBillAgainstGdnCmagtDto.DetailDto det : dto.getInvCommAgentTradeBillDetailslist()) {
                    SimpleJdbcCall detCall = new SimpleJdbcCall(jdbcTemplate)
                            .withProcedureName("Sp_InvCommAgentTradeBillDetail_Insert");

                    MapSqlParameterSource detParams = new MapSqlParameterSource();
                    detParams.addValue("Id", det.getId() != null ? det.getId() : 0);
                    detParams.addValue("InvCommAgentTradeBillId", masterId);
                    detParams.addValue("gdnBuyerDispatchMasterId", det.getGdnBuyerDispatchMasterId() != null ? det.getGdnBuyerDispatchMasterId() : 0);
                    detParams.addValue("gdnBuyerDispatchDetailId", det.getGdnBuyerDispatchDetailId() != null ? det.getGdnBuyerDispatchDetailId() : 0);
                    detParams.addValue("itemId", det.getItemId() != null ? det.getItemId() : 0);
                    detParams.addValue("weight", det.getWeight() != null ? det.getWeight() : BigDecimal.ZERO);
                    detParams.addValue("qty", det.getQty() != null ? det.getQty() : BigDecimal.ZERO);
                    detParams.addValue("rate", det.getRate() != null ? det.getRate() : BigDecimal.ZERO);
                    detParams.addValue("amount", det.getAmount() != null ? det.getAmount() : BigDecimal.ZERO);
                    detParams.addValue("remarksDetail", det.getRemarksDetail() != null ? det.getRemarksDetail() : "");

                    detCall.execute(detParams);
                }
            }

            result.put("status", "SUCCESS");
            result.put("message", "Trade Bill Against GDN saved successfully!");
            result.put("id", masterId);
        } catch (Exception e) {
            result.put("status", "ERROR");
            result.put("message", e.getMessage());
        }
        return result;
    }

    public List<Map<String, Object>> getHistory(Integer companyId, Integer organizationId, String fromDate, String toDate) {
        SimpleJdbcCall call = new SimpleJdbcCall(jdbcTemplate)
                .withProcedureName("USP_InvCommAgentTradeBill_GetAllMethods");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("Activity", "ReadBySearch_InvCommAgentTradeBill");
        params.addValue("CompanyId", companyId != null ? companyId : 1);
        params.addValue("OrganizationId", organizationId != null ? organizationId : 1);
        params.addValue("DocumentTypeId", 1056);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public Map<String, Object> getById(Integer id) {
        Map<String, Object> result = new HashMap<>();

        SimpleJdbcCall headerCall = new SimpleJdbcCall(jdbcTemplate)
                .withProcedureName("USP_InvCommAgentTradeBill_GetAllMethods");

        MapSqlParameterSource headerParams = new MapSqlParameterSource();
        headerParams.addValue("Activity", "ReadById_InvCommAgentTradeBill");
        headerParams.addValue("Id", id);

        Map<String, Object> headerOut = headerCall.execute(headerParams);
        List<Map<String, Object>> headers = extractList(headerOut);

        if (headers != null && !headers.isEmpty()) {
            Map<String, Object> header = new HashMap<>(headers.get(0));

            SimpleJdbcCall detCall = new SimpleJdbcCall(jdbcTemplate)
                    .withProcedureName("USP_InvCommAgentTradeBill_GetAllMethods");
            MapSqlParameterSource detParams = new MapSqlParameterSource();
            detParams.addValue("Activity", "ReadByHeaderId_InvCommAgentTradeBillDetail");
            detParams.addValue("Id", id);
            header.put("invCommAgentTradeBillDetailslist", extractList(detCall.execute(detParams)));

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
