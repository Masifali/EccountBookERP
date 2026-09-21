package com.mst.repositories;

import java.math.BigDecimal;
import java.util.List;
import java.util.Map;

final class ReportValueSupport {
    private ReportValueSupport() { }
    static int amountDecimals(org.springframework.jdbc.core.JdbcTemplate jdbc, com.mst.models.UserAccount user) {
        List<Map<String,Object>> rows = jdbc.queryForList(
                "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                user.getOrganizationId(), user.getCompanyId(), "Default NoofDecimal Points For Amount", "GetConfigurationByOrgCompandConfigDescription");
        if (rows.isEmpty() || rows.get(0).get("ConfigKey") == null || rows.get(0).get("ConfigKey").toString().isBlank()) return 0;
        int places = Integer.parseInt(rows.get(0).get("ConfigKey").toString().trim());
        return places >= 1 && places <= 4 ? places : 0;
    }
    static List<Map<String, Object>> decimalStrings(List<Map<String, Object>> rows) {
        for (Map<String, Object> row : rows) {
            row.replaceAll((key, value) -> value instanceof BigDecimal ? ((BigDecimal) value).toPlainString() : value);
        }
        return rows;
    }
}
