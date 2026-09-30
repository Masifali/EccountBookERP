package com.mst.repositories;

import com.mst.models.UserAccount;
import java.sql.*;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;

@Repository
public class ReceivablesAgingRepository {
    private final JdbcTemplate jdbc;

    public ReceivablesAgingRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    public List<List<Map<String, Object>>> load(UserAccount u, LocalDate date, int days, int report, int parent, int account, int custom, int cost, String branches) {
        List<Object> values = new ArrayList<>(Arrays.asList(u.getOrganizationId(), u.getCompanyId(), u.getAppId(), java.sql.Date.valueOf(date), days, u.getId(), 2, "Detail"));
        StringBuilder sql = new StringBuilder("EXEC dbo.UPS_ReceivablesAging_New @OrganizationId=?, @CompanyId=?, @AppId=?, @AsOnDate=?, @AgingDays=?, @UserId=?, @AccouuntClassId=?, @Activity=?");
        add(sql, values, "ReportId", report);
        add(sql, values, "ParentAccountId", parent);
        add(sql, values, "AccountId", account);
        add(sql, values, "CustomGroupId", custom);
        add(sql, values, "CostCenterId", cost);
        if (branches != null && !branches.isBlank()) {
            sql.append(", @BranchesIds=?");
            values.add(branches);
        }

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
                        sets.add(ReportValueSupport.decimalStrings(rows));
                    }
                } else if (statement.getUpdateCount() == -1) break;
                result = statement.getMoreResults();
            }
            return sets;
        });
    }

    public Map<String, Object> lookups(UserAccount u) {
        Map<String, Object> data = new LinkedHashMap<>();
        data.put("amountDecimals", ReportValueSupport.amountDecimals(jdbc,u));
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        data.put("controls", jdbc.queryForList("EXEC dbo.Sp_AccountsOpeningBalances_GetMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAll3rdLevelAccountsForPayablesandReceeivablesAging'", org, comp));
        data.put("accounts", jdbc.queryForList("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?, @CompanyId=?", org, comp));
        data.put("customGroups", jdbc.queryForList("EXEC dbo.Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, @AcLookUpTypesId=1, @Activity='ReadAll'", org, comp));
        data.put("costCenters", jdbc.queryForList("EXEC dbo.usp_getCostCenters @OrganizationId=?, @CompanyId=?, @UserId=?, @AppId=?", org, comp, u.getId(), u.getAppId()));
        List<Map<String, Object>> branches = new ArrayList<>();
        try { branches = jdbc.queryForList("EXEC dbo.USP_GetBranchesFromVouchersByAccountId @OrganizationId=?, @CompanyId=?", org, comp); } catch (Exception ignored) {}
        /* BranchesFill(): VoucherHead.GetBranchesFromVouchersByAccountId(org, company, "", 0) only.
           The desktop has no second or third source; an empty result leaves the combo empty. */
        for (Map<String, Object> map : branches) {
            Object id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : (map.get("BranchId") != null ? map.get("BranchId") : map.get("branchId")));
            Object name = map.get("BranchName") != null ? map.get("BranchName") : (map.get("branchName") != null ? map.get("branchName") : (map.get("Name") != null ? map.get("Name") : map.get("name")));
            map.put("Id", id); map.put("id", id); map.put("ID", id); map.put("BranchId", id); map.put("branchId", id);
            map.put("BranchName", name); map.put("branchName", name); map.put("Name", name); map.put("name", name);
            /* R4 2026-09-30: map is a LinkedCaseInsensitiveMap - every alias put above REPLACED the stored key spelling
               (the JSON carried "ID"/"branchId"/"branchName"/"name" only, so the page's r.Id / r.BranchName were undefined and the Branches
               combo showed blank rows). Re-put the proc's own spelling (USP_GetBranchesFromVouchersByAccountId: Id, BranchName). */
            map.put("Id", id); map.put("BranchName", name);
        }
        data.put("branches", branches);
        data.put("features", jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", org, comp));
        data.put("branchId", u.getBranchesId());
        data.put("appId", u.getAppId());
        /* grd_LinkClicked opens the ledger from clsGlobalVariables.ActiveYr.Start_Period. */
        List<Map<String, Object>> years = jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?", org, comp);
        data.put("year", years.isEmpty() ? null : years.get(0));
        return data;
    }

    private static void add(StringBuilder sql, List<Object> values, String name, int v) {
        if (v != 0) {
            sql.append(", @").append(name).append("=?");
            values.add(v);
        }
    }
}
