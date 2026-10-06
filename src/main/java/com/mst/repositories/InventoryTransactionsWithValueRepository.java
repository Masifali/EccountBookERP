package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.models.dto.InventoryTransactionsWithValueRequest;
import java.sql.Date;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

/**
 * Desktop Architecture.WinApp.Inventory_Stocks_Report.InventoryEvaluationItemLedger (screen 293).
 *  - BranchesFill            -> BLL BranchesAllocationToUser.GetBranchsAllocatedToUser -> [dbo].[USP_GetBranchsAllocatedToUser]
 *  - ParentCategoryComboFill -> BLL StocksReport.Evalaution_DropDown_ByParentCategories   -> USP_Evalaution_DropDown_ByParentCategories
 *                               (@BranchesIds, @InventoryParentCategory = "1,2,3,4,6"; every activity in one call)
 *  - Load / reset            -> BLL StocksReport.GetInventoryStockAsOnDate               -> usp_getInventoryStockAsOnDate (+1 day)
 *  - btnshow_Click           -> BLL InventoryStockEvalautionDetail.InventoryTransactionReportNew -> USP_InventoryEvaluationItemLedger_Rpt
 */
@Repository
public class InventoryTransactionsWithValueRepository {
    private static final org.slf4j.Logger LOG = org.slf4j.LoggerFactory.getLogger(InventoryTransactionsWithValueRepository.class);
    private final JdbcTemplate jdbc;
    public InventoryTransactionsWithValueRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    public List<Map<String,Object>> branches(UserAccount u) {
        List<Map<String,Object>> list = new ArrayList<>();
        try {
            list = jdbc.queryForList("EXEC dbo.USP_GetBranchsAllocatedToUser @OrganizationId=?, @CompanyId=?, @UserId=?",
                    u.getOrganizationId(), u.getCompanyId(), u.getId());
        } catch (Exception e) { LOG.warn("USP_GetBranchsAllocatedToUser failed for user {}", u.getId(), e); }
        return list;
    }

    public List<Map<String,Object>> choices(UserAccount u, String branchesIds) {
        return jdbc.queryForList("EXEC dbo.USP_Evalaution_DropDown_ByParentCategories @OrganizationId=?, @CompanyId=?, @BranchesIds=?, @InventoryParentCategory=?",
                u.getOrganizationId(), u.getCompanyId(), branchesIds, "1,2,3,4,6");
    }

    /** StocksReport.GetInventoryStockAsOnDate: AsOnDate of the first row plus one day; null when none. */
    public String stockAsOnDate(UserAccount u) {
        List<Map<String,Object>> dates = new ArrayList<>();
        try {
            dates = jdbc.queryForList("EXEC dbo.usp_getInventoryStockAsOnDate @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId());
        } catch (Exception e) { LOG.warn("usp_getInventoryStockAsOnDate failed", e); }
        if (dates.isEmpty() || dates.get(0).get("AsOnDate") == null) return null;
        Object date = dates.get(0).get("AsOnDate");
        LocalDate day = date instanceof java.sql.Timestamp ? ((java.sql.Timestamp) date).toLocalDateTime().toLocalDate() : LocalDate.parse(date.toString().substring(0, 10));
        return day.plusDays(1).toString();
    }

    /** cmbDateType "Financial Year" -> clsGlobalVariables.ActiveYr.Start_Period. */
    public List<Map<String,Object>> financialYears(UserAccount u) {
        try {
            return jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId());
        } catch (Exception e) { LOG.warn("Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId failed", e); return new ArrayList<>(); }
    }

    /** clsGlobalVariables.stringFormatsingle = "#,##0." + 1..4 zeros ("Default NoofDecimal Points For Amount"). */
    public int amountDecimals(UserAccount u) {
        try { return ReportValueSupport.amountDecimals(jdbc, u); }
        catch (Exception e) { LOG.warn("amount decimal configuration failed", e); return 0; }
    }

    /** clsGlobalVariables.DecimalRateFormate = "#,#0." + zeros: 0 (or missing) -> 2, 1..4 -> n, anything else -> 0. */
    public int rateDecimals(UserAccount u) {
        try {
            List<Map<String,Object>> rows = jdbc.queryForList(
                    "EXEC dbo.Sp_ConfigrationsAllocation_GetAllMethod @OrganizationId=?, @CompanyId=?, @ConfigDescription=?, @Activity=?",
                    u.getOrganizationId(), u.getCompanyId(), "Default NoofDecimal Points For Rate", "GetConfigurationByOrgCompandConfigDescription");
            int places = 0;
            if (!rows.isEmpty() && rows.get(0).get("ConfigKey") != null && !rows.get(0).get("ConfigKey").toString().isBlank()) {
                try { places = Integer.parseInt(rows.get(0).get("ConfigKey").toString().trim()); } catch (NumberFormatException ignored) { places = 0; }
            }
            if (places == 0) return 2;
            return places >= 1 && places <= 4 ? places : 0;
        } catch (Exception e) { LOG.warn("rate decimal configuration failed", e); return 2; }
    }

    /** InventoryTransactionReportNew: the optional parameters are passed only when set, in the BLL's order. */
    public List<Map<String,Object>> load(UserAccount user, InventoryTransactionsWithValueRequest r, String branchesIds, String ids) {
        Map<String,Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", user.getOrganizationId());
        p.put("CompanyId", user.getCompanyId());
        p.put("DateFrom", Date.valueOf(r.getFromDate()));
        p.put("DateTo", Date.valueOf(r.getToDate()));
        add(p, "ItemTypeId", r.getItemTypeId());
        add(p, "ItemCategoryId", r.getItemCategoryId());
        if (r.getCropYear() != null && !r.getCropYear().isEmpty()) p.put("CropYear", r.getCropYear());
        add(p, "JobLotId", r.getJobLotId());
        add(p, "WarehouseId", r.getWarehouseId());
        add(p, "ItemId", r.getItemId());
        add(p, "ItemStockAc", r.getItemStockAc());
        add(p, "ItemClassGroupId", r.getItemClassGroupId());
        if (r.isSaleValue()) p.put("SaleValue", 1);
        add(p, "DocumentTypeId", r.getDocumentTypeId());
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        if (ids != null && !ids.isEmpty()) p.put("Ids", ids);
        StringJoiner sql = new StringJoiner(", ", "EXEC dbo.USP_InventoryEvaluationItemLedger_Rpt ", "");
        for (String key : p.keySet()) sql.add("@" + key + "=?");
        return ReportValueSupport.decimalStrings(jdbc.queryForList(sql.toString(), p.values().toArray()));
    }

    private static void add(Map<String,Object> p, String key, Integer value) {
        if (value == null || value == 0) return;
        p.put(key, value);
    }
}
