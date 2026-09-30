package com.mst.repositories;

import com.mst.models.UserAccount;
import java.sql.*;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;

@Repository
public class PayablesAgingRepository {
    private final JdbcTemplate jdbc;

    public PayablesAgingRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    /** PayablesAging_New.GridFill -> VoucherReports.PayablesAging_New (UPS_PayablesAging_New). The desktop
     *  form has no cost centre: no @AppId and no @CostCenterId are sent. @ControlAccountIds is the
     *  comma list of the checked control accounts' Ids (GetSelectedIdsFromMultiSelectionCombo, column 0). */
    public List<List<Map<String, Object>>> load(UserAccount u, LocalDate date, int days, int report, String controlAccounts, int account, int custom, String branches) {
        List<Object> values = new ArrayList<>(Arrays.asList(u.getOrganizationId(), u.getCompanyId(), java.sql.Date.valueOf(date), days, u.getId(), 3, "Detail"));
        StringBuilder sql = new StringBuilder("EXEC dbo.UPS_PayablesAging_New @OrganizationId=?, @CompanyId=?, @AsOnDate=?, @AgingDays=?, @UserId=?, @AccouuntClassId=?, @Activity=?");
        add(sql, values, "ReportId", report);
        if (controlAccounts != null && !controlAccounts.isBlank()) { sql.append(", @ControlAccountIds=?"); values.add(controlAccounts); }
        add(sql, values, "AccountId", account);
        add(sql, values, "CustomGroupId", custom);
        if (branches != null && !branches.isBlank()) { sql.append(", @BranchesIds=?"); values.add(branches); }
        return executeProcedure(sql.toString(), values);
    }
    private List<List<Map<String, Object>>> executeProcedure(String sql, List<Object> values) {
        return jdbc.execute(sql, (PreparedStatementCallback<List<List<Map<String, Object>>>>) statement -> {
            for (int i = 0; i < values.size(); i++) statement.setObject(i + 1, values.get(i));
            List<List<Map<String, Object>>> sets = new ArrayList<>();
            boolean result = statement.execute();
            while (true) {
                if (result) {
                    try (ResultSet rs = statement.getResultSet()) {
                        List<Map<String, Object>> rows = new ArrayList<>();
                        ColumnMapRowMapper mapper = new ColumnMapRowMapper();
                        while (rs.next()) rows.add(mapper.mapRow(rs, rows.size()));
                        sets.add(rows);
                    }
                } else if (statement.getUpdateCount() == -1) break;
                result = statement.getMoreResults();
            }
            return sets;
        });
    }

    public Map<String, Object> lookups(UserAccount u, int financialYearId) {
        Map<String,Object> data=new LinkedHashMap<>();
        int org=u.getOrganizationId(),comp=u.getCompanyId();
        data.put("amountDecimals",ReportValueSupport.amountDecimals(jdbc,u));
        /* AccountFill3rdLevel(): ChartofAccount.ReadAllAccountgroup{FinancialYearId=ActiveYr, Account_Level=3,
           AccountTypeId=3, AccouuntClassId=3 -> @AccountClassId}; CoaType ReadAllAccountGroup. */
        data.put("controls",jdbc.queryForList("EXEC dbo.Sp_ChartofAccount_GetAllMethodFromCOA @OrganizationId=?, @CompanyId=?, @FinancialYearId=?, @Account_Level=3, @AccountTypeId=3, @AccountClassId=3, @CoaType='ReadAllAccountGroup'",org,comp,financialYearId));
        data.put("accounts",jdbc.queryForList("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?, @CompanyId=?",org,comp));
        data.put("customGroups",jdbc.queryForList("EXEC dbo.Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, @AcLookUpTypesId=1, @Activity='ReadAll'",org,comp));
        List<Map<String, Object>> branches = new ArrayList<>();
        try { branches = jdbc.queryForList("EXEC dbo.USP_GetBranchesFromVouchersByAccountId @OrganizationId=?, @CompanyId=?", org, comp); } catch (Exception ignored) {}
        /* BranchesFill(): VoucherHead.GetBranchesFromVouchersByAccountId(org, company, "", 0) only.
           The desktop has no second or third source; an empty result leaves the combo empty. */
        for (Map<String, Object> map : branches) {
            Object id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : (map.get("BranchId") != null ? map.get("BranchId") : map.get("branchId")));
            Object name = map.get("BranchName") != null ? map.get("BranchName") : (map.get("branchName") != null ? map.get("branchName") : (map.get("Name") != null ? map.get("Name") : map.get("name")));
            map.put("Id", id); map.put("id", id); map.put("ID", id); map.put("BranchId", id); map.put("branchId", id);
            map.put("BranchName", name); map.put("branchName", name); map.put("Name", name); map.put("name", name);
            /* R3 casing 2026-09-30: queryForList rows are LinkedCaseInsensitiveMaps - each put above REPLACES the stored key
               spelling, leaving "ID"/"branchName"/"name" in the JSON, so pages reading r.Id / r.BranchName got empty options.
               Re-putting restores the spellings the pages read. */
            map.put("Id", id); map.put("BranchName", name);
        }
        data.put("branches", branches);
        data.put("features",jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",org,comp));
        data.put("branchId",u.getBranchesId());data.put("appId",u.getAppId());
        List<Map<String, Object>> years = jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?", org, comp);
        data.put("year", years.stream().filter(y -> y.get("Id") instanceof Number && ((Number) y.get("Id")).intValue() == financialYearId).findFirst().orElse(years.isEmpty() ? null : years.get(0)));
        return data;
    }
    private static void add(StringBuilder sql, List<Object> values, String name, int v) {
        if (v != 0) {
            sql.append(", @").append(name).append("=?");
            values.add(v);
        }
    }
}
