package com.mst.repositories;

import com.mst.models.UserAccount;
import java.sql.*;
import java.time.LocalDate;
import java.time.temporal.ChronoUnit;
import java.util.*;
import org.springframework.jdbc.core.*;
import org.springframework.stereotype.Repository;

@Repository
public class PayablesReportRepository {
    private final JdbcTemplate jdbc;

    public PayablesReportRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    public List<List<Map<String, Object>>> load(UserAccount u, LocalDate fromDate, LocalDate toDate, int controlAccountId, int accountId, int customGroupId, int inventoryGroupId, int cityId, double closingFrom, double closingTo, boolean onlyCredit, boolean onlyDebit, boolean tradeParties, boolean approvedTransactions, String classification, String typeNature, String sortField, String sortOrder) {
        List<Map<String,Object>> years=jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",u.getOrganizationId(),u.getCompanyId());
        if(years.size()!=1) throw new IllegalStateException("Select a single active financial year");
        List<Object> values = new ArrayList<>(Arrays.asList(years.get(0).get("Id"),u.getOrganizationId(),u.getCompanyId(),u.getId()));
        StringBuilder sql = new StringBuilder("EXEC dbo.Sp_Accounts_Payables_Rpt @FinancialYearId=?, @OrganizationId=?, @CompanyId=?, @UserId=?");
        if(fromDate!=null){sql.append(", @FromDate=?");values.add(java.sql.Date.valueOf(fromDate));}
        if(toDate!=null){sql.append(", @ToDate=?");values.add(java.sql.Date.valueOf(toDate));}
        if(controlAccountId!=0){sql.append(", @ControlAccountIds=?");values.add(String.valueOf(controlAccountId));}
        if(inventoryGroupId!=0){sql.append(", @CustomerGroupIds=?");values.add(String.valueOf(inventoryGroupId));}
        add(sql,values,"CityId",cityId);add(sql,values,"CustomGroupId",customGroupId);
        if(closingFrom!=0){sql.append(", @BalanceFrom=?");values.add(closingFrom);}
        if(closingTo!=0){sql.append(", @BalanceTo=?");values.add(closingTo);}
        if(tradeParties){sql.append(", @ShowOnlyTrade=?");values.add(1);}
        return executeProcedure(sql.toString(),values);
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
