package com.mst.repositories;

import com.mst.models.UserAccount;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * Inventory Payables and Receivables (screen 61) - Architecture.WinApp.Account_Reports.InventoryPayablesandReceivables.
 * Report: GeneralReprots.InventoryPayablesandReceivables -> SpAccounts_InventoryPayablesandReceivables_Rpt.
 * Every parameter is added exactly when the desktop BLL adds it; nothing is bound as NULL.
 */
@Repository
public class InventoryPayablesReceivablesRepository {
    private final JdbcTemplate jdbc;

    public InventoryPayablesReceivablesRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String, Object>> load(UserAccount u, int financialYearId, LocalDate from, LocalDate to, int tranTypes,
                                          int reportType, boolean approved, double clDebit, double clCredit,
                                          String customerGroupIds, String parentCategoryIds, int customGroupId, String branchIds) {
        LinkedHashMap<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("FinancialYearId", financialYearId);
        p.put("UserId", u.getId());
        p.put("TranTypes", tranTypes);                       // always added (obj.TranTypes, 0 when no radio is ticked)
        if (from != null) p.put("FromDate", java.sql.Date.valueOf(from));
        if (to != null) p.put("ToDate", java.sql.Date.valueOf(to));
        if (notBlank(customerGroupIds)) p.put("CustomerGroupIds", customerGroupIds);
        if (notBlank(parentCategoryIds)) p.put("InventoryParentCategories", parentCategoryIds);
        if (reportType != 0) p.put("ReportType", reportType);
        if (clDebit != 0.0) p.put("ClDebit", clDebit);
        if (clCredit != 0.0) p.put("ClCredit", clCredit);
        if (approved) p.put("IsApproved", true);             // unticked: ApprovedFilter = "All" -> not added
        if (customGroupId != 0) p.put("CustomeGroupId", customGroupId);
        if (notBlank(branchIds)) p.put("BranchesIds", branchIds);
        return ReportValueSupport.decimalStrings(execute("SpAccounts_InventoryPayablesandReceivables_Rpt", p));
    }

    public Map<String, Object> lookups(UserAccount u) {
        Map<String, Object> data = new LinkedHashMap<>();
        int org = u.getOrganizationId(), comp = u.getCompanyId();
        data.put("amountDecimals", ReportValueSupport.amountDecimals(jdbc, u));
        data.put("features", jdbc.queryForList("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", org, comp));
        /* BranchesFill(): GetBranchesFromVouchersByAccountId(OrganizationId, CompanyIdFromBreakUp, "", 0). Opened from
           the menu CompanyIdFromBreakUp is 0, so the BLL leaves @CompanyId out - reproduced as-is. */
        List<Map<String, Object>> branches = new ArrayList<>();
        try { branches = jdbc.queryForList("EXEC dbo.USP_GetBranchesFromVouchersByAccountId @OrganizationId=?", org); } catch (Exception ignored) {}
        data.put("branches", branches);
        data.put("branchId", u.getBranchesId());
        /* CustomeGroupsDefine(): CommonServices.CustomeGroupsDefine(1) -> AcLookUps.GetAll. */
        data.put("customGroups", jdbc.queryForList("EXEC dbo.Sp_AcLookUps_GetAllMethod @OrganizationId=?, @CompanyId=?, @AcLookUpTypesId=1, @Activity='ReadAll'", org, comp));
        /* CustomerGroupBind(): CustomerGroup.GetSupplierCustomerGroupFromInventoryStockEvaluation. */
        data.put("inventoryGroups", jdbc.queryForList("EXEC dbo.Sp_CustomerGroup_GetAllMethod @Activity='GetSupplierCustomerGroupFromInventoryStockEvaluation', @organizationId=?, @CompanyId=?", org, comp));
        /* TradeTypeFill(): Item.InventoryParentCategories{Activity} - the BLL sends only @Activity (no tenancy). */
        data.put("tradeTypes", jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @Activity='InventoryParentCategories'"));
        return data;
    }

    private static boolean notBlank(String s) { return s != null && !s.isBlank(); }

    private List<Map<String, Object>> execute(String procedure, LinkedHashMap<String, Object> p) {
        StringJoiner sql = new StringJoiner(", ", "EXEC dbo." + procedure + " ", "");
        p.keySet().forEach(k -> sql.add("@" + k + "=?"));
        return jdbc.queryForList(sql.toString(), p.values().toArray());
    }
}
