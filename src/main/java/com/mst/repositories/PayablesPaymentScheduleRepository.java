package com.mst.repositories;

import com.mst.models.UserAccount;
import java.sql.*;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;

@Repository
public class PayablesPaymentScheduleRepository {
    private final JdbcTemplate jdbc;

    public PayablesPaymentScheduleRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    public List<List<Map<String, Object>>> load(UserAccount u, LocalDate fromDate, LocalDate toDate, LocalDate dueFrom, LocalDate dueUpTo, LocalDate purchaseFrom, LocalDate purchaseTo, int intervalDays, int agingDays, int report, int parent, int account, int custom, int cost, int customerGroup, String controlAccount, String branches) {
        List<Object> values = new ArrayList<>(Arrays.asList(u.getOrganizationId(), u.getCompanyId(), u.getId(), 0));
        StringBuilder sql = new StringBuilder("EXEC dbo.usp_PayablesAndPaymentSchedule @OrganizationId=?, @CompanyId=?, @UserId=?, @ParentCategoryId=?");
        LocalDate[] dates = {fromDate, toDate, dueFrom, dueUpTo, purchaseFrom, purchaseTo};
        String[] names = {"FromDate", "ToDate", "DueDateFrom", "DueDateTo", "PurchaseFromDate", "PurchaseToDate"};
        for (int i=0; i<dates.length; i++) if (dates[i]!=null) { sql.append(", @").append(names[i]).append("=?"); values.add(java.sql.Date.valueOf(dates[i])); }
        add(sql, values, "AgingDays", agingDays);
        add(sql, values, "CustomGroupId", custom);
        add(sql, values, "PartyGroupId", customerGroup);
        if (controlAccount != null && !controlAccount.isBlank() && !controlAccount.equals("0")) { sql.append(", @ControlAccountIds=?"); values.add(controlAccount); }
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

    public Map<String, Object> lookups(UserAccount u) {
        Map<String,Object> data=new LinkedHashMap<>();
        int org=u.getOrganizationId(),comp=u.getCompanyId();
        data.put("amountDecimals",ReportValueSupport.amountDecimals(jdbc,u));
        data.put("controls",jdbc.queryForList("EXEC dbo.Sp_AccountsOpeningBalances_GetMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAll3rdLevelAccountsForPayablesandReceeivablesAging'",org,comp));
        data.put("accounts",jdbc.queryForList("EXEC dbo.USP_GETAllAccountsFromCustomGroups @OrganizationId=?, @CompanyId=?",org,comp));
        data.put("customGroups",jdbc.queryForList("EXEC dbo.Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, @AcLookUpTypesId=1, @Activity='ReadAll'",org,comp));
        data.put("customerGroups",jdbc.queryForList("EXEC dbo.Sp_CustomerGroup_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='ReadAll'",org,comp));
        data.put("inventoryGroups",jdbc.queryForList("EXEC dbo.Sp_CustomerGroup_GetAllMethod @OrganizationId=?, @CompanyId=?, @Activity='GetSupplierCustomerGroupFromInventoryStockEvaluation'",org,comp));
        data.put("cities",jdbc.queryForList("EXEC dbo.SP_City_GetAllMethod @OrganizationId=?, @CompanyId=?, @MethodType='GetAll'",org,comp));
        data.put("costCenters",jdbc.queryForList("EXEC dbo.usp_getCostCenters @OrganizationId=?, @CompanyId=?, @UserId=?, @AppId=?",org,comp,u.getId(),u.getAppId()));
        List<Map<String, Object>> branches = new ArrayList<>();
        try { branches = jdbc.queryForList("EXEC dbo.USP_GetBranchesFromVouchersByAccountId @OrganizationId=?, @CompanyId=?", org, comp); } catch (Exception ignored) {}
        if (branches.isEmpty()) {
            try { branches = jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUser @OrganizationId=?, @CompanyId=?, @UserId=?", org, comp, u.getId()); } catch (Exception ignored) {}
        }
        if (branches.isEmpty()) {
            try { branches = jdbc.queryForList("SELECT ID as Id, BranchName FROM Branches"); } catch (Exception ignored) {}
        }
        for (Map<String, Object> map : branches) {
            Object id = map.get("Id") != null ? map.get("Id") : (map.get("id") != null ? map.get("id") : (map.get("BranchId") != null ? map.get("BranchId") : map.get("branchId")));
            Object name = map.get("BranchName") != null ? map.get("BranchName") : (map.get("branchName") != null ? map.get("branchName") : (map.get("Name") != null ? map.get("Name") : map.get("name")));
            map.put("Id", id); map.put("id", id); map.put("ID", id); map.put("BranchId", id); map.put("branchId", id);
            map.put("BranchName", name); map.put("branchName", name); map.put("Name", name); map.put("name", name);
        }
        data.put("branches", branches);
        data.put("features",jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?",org,comp));
        data.put("branchId",u.getBranchesId());data.put("appId",u.getAppId());
        return data;
    }
    private static void add(StringBuilder sql, List<Object> values, String name, int v) {
        if (v != 0) {
            sql.append(", @").append(name).append("=?");
            values.add(v);
        }
    }
}
