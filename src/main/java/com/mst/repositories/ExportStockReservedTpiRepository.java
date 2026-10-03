package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * frmStockReservedAgainstThirdPartyInspection "Stock Reserved Against third Party Analysis"
 * (Architecture.WinApp.Export; opened from 857 / 858). Each call is the desktop's own procedure:
 *
 *   USP_Inventory_StockEvalautionDetail_DropDownAndLists    @OrganizationId @CompanyId @BranchesIds    StocksReport.Inventory_StockEvalautionDetail_DropDownAndLists (BLL 0125)
 *   USP_GetOnlyAcceptedAndAwaitingAnalysisNoFromLabPreProductionExportLotInspection  @OrganizationId @CompanyId   tracking no combo (BLL 0472)
 *   USP_StockEvalualtion_GetAvailableTransactionsForStockReserved  @OrganizationId @CompanyId [@BranchesId] [@DateFrom]
 *        [@DateTo] [@SupplierCustomerId] [@WarehouseId] [@ItemId] [@ReferenceDocumentTypeId] [@JobLotId]
 *        [@InventoryParentCategories] [@CropYear]   (each only when set, as BLL 0574 builds the list)
 *   USP_InventoryStockReserved_InsertAndUpdate  the 38 non-virtual model properties per checked row (SetProc),
 *        one transaction (DAL 0377 SetDataList)
 *   Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId  clsGlobalVariables.ActiveYr (Start_Period)
 *
 * No table, column or procedure is created or changed.
 */
@Repository
public class ExportStockReservedTpiRepository {

    private final JdbcTemplate jdbc;

    public ExportStockReservedTpiRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    /** BranchesIds = Conversion.ToString(UserAccount.BranchesId) - sent when not empty. */
    public List<Map<String, Object>> dropDownsAndLists(UserAccount u) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        Integer b = u.getBranchesId();
        if (b != null) p.put("BranchesIds", String.valueOf(b));
        return DesktopProc.rows(jdbc, "USP_Inventory_StockEvalautionDetail_DropDownAndLists", p);
    }

    public List<Map<String, Object>> trackingNos(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetOnlyAcceptedAndAwaitingAnalysisNoFromLabPreProductionExportLotInspection",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** The caller passes only the parameters the BLL would send (nulls are omitted by DesktopProc). */
    public List<Map<String, Object>> availableTransactions(Map<String, Object> p) {
        return DesktopProc.rows(jdbc, "USP_StockEvalualtion_GetAvailableTransactionsForStockReserved", p);
    }

    @Transactional(rollbackFor = Exception.class)
    public int saveList(List<Map<String, Object>> items) {
        int result = 0;
        for (Map<String, Object> item : items) {
            Integer v = DesktopProc.scalar(jdbc, "USP_InventoryStockReserved_InsertAndUpdate", item);
            result = v == null ? 0 : v;
        }
        return result;
    }

    public Map<String, Object> financialYear(UserAccount u, int financialYearId) {
        List<Map<String, Object>> years = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        for (Map<String, Object> r : years) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == financialYearId) return r;
        }
        return years.isEmpty() ? null : years.get(0);
    }
}
