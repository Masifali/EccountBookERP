package com.mst.repositories.cmagt;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Commission Trading reports - the five desktop report procedures and their filter-combo
 * procedures, executed with EXACTLY the parameters the desktop BLL adds.
 *
 * The desktop BLL builds a List<SqlParameter> and only adds an optional parameter when it has a
 * value (e.g. `if (obj.ItemId != 0)`), so an omitted parameter falls back to the procedure's own
 * default. That matters here: @Activity defaults to 'Detail' in the four [cmagt] procedures, and
 * SimpleJdbcCall (used before) binds every metadata parameter it is not given as NULL, which is
 * not the same thing. The call is therefore built as a named-parameter EXEC containing only the
 * entries the service put in the map, in the BLL's order.
 *
 * GenericProvider.GetDataTableProc fills a DataTable from the FIRST result set; queryForList does
 * the same.
 */
@Repository
public class CmagtReportRepository {

    @Autowired
    private JdbcTemplate jdbcTemplate;

    /** Runs {@code proc} with the given named parameters (names without '@'); nothing else is sent. */
    public List<Map<String, Object>> exec(String proc, LinkedHashMap<String, Object> params) {
        StringBuilder sql = new StringBuilder("EXEC ").append(proc);
        List<Object> args = new ArrayList<>();
        boolean first = true;
        for (Map.Entry<String, Object> e : params.entrySet()) {
            sql.append(first ? " " : ", ").append('@').append(e.getKey()).append("=?");
            args.add(e.getValue());
            first = false;
        }
        return jdbcTemplate.queryForList(sql.toString(), args.toArray());
    }

    /**
     * GlobalVariables_Helper.GetConfigValueFromGlobal(description) - the same procedure/activity the
     * other ported screens use (SaleOrderHistoryLookupsRepository.config). Empty string when unset.
     */
    public String config(int organizationId, int companyId, String description) {
        try {
            List<Map<String, Object>> rows = jdbcTemplate.queryForList(
                    "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, "
                            + "@ConfigDescription=?, @DefinitionIds=?, @Activity=?",
                    organizationId, companyId, description, null,
                    "GetConfigurationByOrgCompandConfigDescription");
            Object v = rows.isEmpty() ? null : rows.get(0).get("ConfigKey");
            return v == null ? "" : String.valueOf(v).trim();
        } catch (Exception e) {
            return "";
        }
    }
}
