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

    /**
     * Architecture.BLL.tblUserRights.GetByUserId - dbo.Sp_tblUserRights_GetAllMethod
     * @Activity='GetByUserId' for the report form's own screen name (base.Name), which
     * CommonServices.SetRightsValueInRightsObject reads into Rightsobjects.
     */
    public List<Map<String, Object>> userRights(int userId, String screenName, String roleName, int companyId) {
        try {
            return jdbcTemplate.queryForList("EXEC dbo.Sp_tblUserRights_GetAllMethod @UserId=?, @ScreenName=?, "
                    + "@RightName=?, @CompanyId=?, @Activity=?",
                    userId, screenName, roleName == null ? "" : roleName, companyId, "GetByUserId");
        } catch (Exception e) {
            return new ArrayList<>();
        }
    }

    /**
     * BLL 0489 SaleOrderHeader_UpdateStatus / 0490 PurchaseOrderHeader_UpdateStatus: ONE call of
     * {@code proc} with @ApprovalData = dbo.TVP_StatusChangeListType built from the rows, inside one
     * transaction (the proc is a single statement batch here, so it is atomic as on the desktop).
     * Row columns, in the BLL DataTable's ordinal order: OrganizationId, CompanyId,
     * FinancialYearId, DocumentTypeId, Id, UserId, ReqType, DateForUpdate, Remarks.
     * The TVP is filled positionally, exactly like the BLL's DataTable (bound by ordinal).
     */
    public void updateOrderStatus(String proc, List<Object[]> rows) {
        StringBuilder sql = new StringBuilder("SET NOCOUNT ON; DECLARE @t dbo.[TVP_StatusChangeListType]; ");
        List<Object> args = new ArrayList<>();
        for (Object[] r : rows) {
            sql.append("INSERT INTO @t VALUES (?,?,?,?,?,?,?,?,?); ");
            for (Object o : r) args.add(o);
        }
        sql.append("EXEC ").append(proc).append(" @ApprovalData=@t;");
        jdbcTemplate.execute(sql.toString(), (org.springframework.jdbc.core.PreparedStatementCallback<Object>) ps -> {
            for (int i = 0; i < args.size(); i++) ps.setObject(i + 1, args.get(i));
            boolean rs = ps.execute();
            while (true) {                       // drain so a RAISERROR after the inserts surfaces
                if (!rs && ps.getUpdateCount() == -1) break;
                rs = ps.getMoreResults();
            }
            return null;
        });
    }

    /** clsGlobalVariables.ActiveYr.Start_Period - Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId. */
    public Object financialYearStart(int organizationId, int companyId, int financialYearId) {
        try {
            List<Map<String, Object>> years = jdbcTemplate.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    organizationId, companyId);
            Map<String, Object> row = null;
            for (Map<String, Object> r : years) {
                Object id = ci(r, "Id");
                if (id instanceof Number && ((Number) id).intValue() == financialYearId) { row = r; break; }
            }
            if (row == null && !years.isEmpty()) row = years.get(0);
            return row == null ? null : ci(row, "Start_Period");
        } catch (Exception e) {
            return null;
        }
    }

    private static Object ci(Map<String, Object> r, String key) {
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
}
