package com.mst.repositories.cmagt;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.namedparam.MapSqlParameterSource;
import org.springframework.jdbc.core.simple.SimpleJdbcCall;
import org.springframework.stereotype.Repository;

import java.text.SimpleDateFormat;
import java.util.*;

@Repository
public class CmagtReportRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    private static final SimpleDateFormat DATE_FORMAT = new SimpleDateFormat("yyyy-MM-dd");

    public List<Map<String, Object>> getSaleOrderReport(Integer companyId, Integer organizationId, String fromDate, String toDate, Integer buyerId, Integer agentId) {
        SimpleJdbcCall call = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("USP_saleOrderMaster_Report");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("CompanyId", companyId != null ? companyId : 1);
        params.addValue("OrganizationId", organizationId != null ? organizationId : 1);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));
        if (buyerId != null && buyerId > 0) params.addValue("BuyerId", buyerId);
        if (agentId != null && agentId > 0) params.addValue("CommissionAgentId", agentId);

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public List<Map<String, Object>> getPurchaseOrderReport(Integer companyId, Integer organizationId, String fromDate, String toDate, Integer supplierId, Integer agentId) {
        SimpleJdbcCall call = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("USP_purchaseOrderMaster_Report");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("CompanyId", companyId != null ? companyId : 1);
        params.addValue("OrganizationId", organizationId != null ? organizationId : 1);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));
        if (supplierId != null && supplierId > 0) params.addValue("SupplierId", supplierId);
        if (agentId != null && agentId > 0) params.addValue("CommissionAgentId", agentId);

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public List<Map<String, Object>> getGrnSupplierLoadingReport(Integer companyId, Integer organizationId, String fromDate, String toDate, Integer supplierId) {
        SimpleJdbcCall call = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("USP_grnSupplierLoadingMaster_Report");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("CompanyId", companyId != null ? companyId : 1);
        params.addValue("OrganizationId", organizationId != null ? organizationId : 1);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));
        if (supplierId != null && supplierId > 0) params.addValue("SupplierId", supplierId);

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public List<Map<String, Object>> getGdnBuyerDispatchReport(Integer companyId, Integer organizationId, String fromDate, String toDate, Integer buyerId) {
        SimpleJdbcCall call = new SimpleJdbcCall(jdbcTemplate)
                .withSchemaName("cmagt")
                .withProcedureName("USP_gdnBuyerDispatchMaster_Report");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("CompanyId", companyId != null ? companyId : 1);
        params.addValue("OrganizationId", organizationId != null ? organizationId : 1);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));
        if (buyerId != null && buyerId > 0) params.addValue("BuyerId", buyerId);

        Map<String, Object> out = call.execute(params);
        return extractList(out);
    }

    public List<Map<String, Object>> getAgentTradeBillRegister(Integer companyId, Integer organizationId, String fromDate, String toDate, Integer agentId) {
        SimpleJdbcCall call = new SimpleJdbcCall(jdbcTemplate)
                .withProcedureName("USP_CommissionAgentTrade_Register");

        MapSqlParameterSource params = new MapSqlParameterSource();
        params.addValue("CompanyId", companyId != null ? companyId : 1);
        params.addValue("OrganizationId", organizationId != null ? organizationId : 1);
        params.addValue("FromDate", parseDate(fromDate));
        params.addValue("ToDate", parseDate(toDate));
        if (agentId != null && agentId > 0) params.addValue("CommissionAgentId", agentId);

        Map<String, Object> out = call.execute(params);
        return extractList(out);
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

    private Date parseDate(String dateStr) {
        if (dateStr == null || dateStr.trim().isEmpty()) return new Date();
        try {
            return DATE_FORMAT.parse(dateStr);
        } catch (Exception e) {
            return new Date();
        }
    }
}
