package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screens 97 "Stock Close Run" (StockCloseRun.cs) and 103 "Stock Closing" (MonthlyStockClosing.cs),
 * Architecture.WinApp.Inventory_Definition.Stock_Closing — DocumentTypeId 300. The reads/writes of
 * BLL 0525 / DAL 0379 StockMonthlyClosingHeader, BLL 0546 InvStockAdjustment and the CommonServices
 * helpers the two forms call, each exactly as its BLL builds the call:
 *
 *   CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10,12")  Sp_COAAllocation_GetAllMethod 'GetAccountTitleByAccountTypeIds'
 *   StockMonthlyClosingHeader.ReadByClosingDateFromStockClosing        USP_StockMonthlyClosing_GetAllMethod 'ReadByClosingDate' (@ClosingDate only)
 *   StockMonthlyClosingHeader.GetAvailableStockForMonthlyClosing       usp_getAvailableStockForMonthlyClosing            (103)
 *   InvStockAdjustment.GetStockEvaludationDataForAdjustment            USP_GetStockEvaludationDataForAdjustment          (97, @ToDate only)
 *   StockMonthlyClosingHeader.MonthlyClosingStock_Slip                 usp_MonthlyClosingStock_Slip                      (300-Slip)
 *   VoucherReports.VoucherSlipForInventoryReport                       Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt (103-Voucher)
 *   DAL 0379 SetData                                                   USP_StockMonthlyClosingHeader_Insert|Update,
 *                                                                      USP_StockMonthlyClosing_InsertAndUpdate (per row),
 *                                                                      usp_MonthlyStockClosing_InventoryStockEvalautionDetail,
 *                                                                      usp_MonthlyStockClosing_InventoryTransactions,
 *                                                                      Sp_VoucherHead_Insert|Update, Sp_VoucherDetail_Insert,
 *                                                                      USP_VoucherBalanceCheck, Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert
 *
 * The VoucherHead / VoucherDetail parameter maps come from {@link InventoryOpeningDefaults} (the CLR defaults
 * of every non-virtual property, as GenericProvider.SetProc sends them). A null is never bound (DesktopProc).
 */
@Repository
public class Inv4cStockClosingRepository {

    private final JdbcTemplate jdbc;
    private final StoreIssuanceRepository shared;

    public Inv4cStockClosingRepository(JdbcTemplate jdbc, StoreIssuanceRepository shared) {
        this.jdbc = jdbc;
        this.shared = shared;
    }

    /** StockCreditAccountBind — BLL COAAllocation.GetAccountTitleByAccountTypeIds with AccountTypeIds "10,12" (Org, Company, AppId, UserId). */
    public List<Map<String, Object>> differenceAccounts(UserAccount u, int appId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "AppId", appId,
                "AccountTypeIds", "10,12");
        if (u.getId() != 0) p.put("UserId", u.getId());
        p.put("Activity", "GetAccountTitleByAccountTypeIds");
        return DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p);
    }

    /** BLL 0525 ReadByClosingDateFromStockClosing — @ClosingDate + 'ReadByClosingDate' (no company filter on the desktop). */
    public List<Map<String, Object>> readByClosingDate(Timestamp closingDate) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_StockMonthlyClosing_GetAllMethod]", params(
                "ClosingDate", closingDate,
                "Activity", "ReadByClosingDate"));
    }

    /** BLL 0525 GetAvailableStockForMonthlyClosing (MonthlyStockClosing.cs:205). */
    public List<Map<String, Object>> availableStockForMonthlyClosing(UserAccount u, Timestamp asOnDate) {
        return DesktopProc.rows(jdbc, "usp_getAvailableStockForMonthlyClosing", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocDate", asOnDate));
    }

    /** BLL 0546 GetStockEvaludationDataForAdjustment (StockCloseRun.cs:210) — only Org, Company, ToDate are set. */
    public List<Map<String, Object>> stockEvaluationDataForAdjustment(UserAccount u, Timestamp toDate) {
        return DesktopProc.rows(jdbc, "USP_GetStockEvaludationDataForAdjustment", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ToDate", toDate));
    }

    /** CommonServices.StockMonthlyClosing_Slip → BLL 0525 MonthlyClosingStock_Slip. */
    public List<Map<String, Object>> slip(UserAccount u, Timestamp closingDate) {
        return DesktopProc.rows(jdbc, "usp_MonthlyClosingStock_Slip", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "ClosingDate", closingDate));
    }

    /** AcRptPurchaseSalesVoucherSlip_103 → VoucherReports.VoucherSlipForInventoryReport (Id, DocumentTypeId, ApprovedFilter "All"). */
    public List<Map<String, Object>> voucherSlip(UserAccount u, int voucherHeadId, int documentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "Id", voucherHeadId,
                "DocumentTypeId", documentTypeId));
    }

    /** CommonServices.VoucherHeadIdGet / DAL 0205 GetVoucherHeadId. */
    public int voucherHeadId(UserAccount u, int documentTypeId, int documentTypeSrNo) {
        return shared.voucherHeadId(u, documentTypeId, documentTypeSrNo);
    }

    /** DAL 0205 CommonServices.GetItemGlIdsandItemName — Sp_Item_GetAllMethod 'GetItemGlIdsandItemName' (Id, ItemName, PurchaseGLAC). */
    public List<Map<String, Object>> itemGlIdsAndNames(UserAccount u) {
        return shared.itemGlAccounts(u);
    }

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Map<String, Object> model) {
        return shared.setProc(proc, model);
    }

    /** DAL 0379: usp_MonthlyStockClosing_InventoryStockEvalautionDetail / usp_MonthlyStockClosing_InventoryTransactions / USP_VoucherBalanceCheck. */
    public void exec(String proc, Map<String, Object> p) {
        DesktopProc.rows(jdbc, proc, p);
    }

    public static Map<String, Object> voucherHeadModel() { return InventoryOpeningDefaults.voucher(); }
    public static Map<String, Object> voucherDetailModel() { return InventoryOpeningDefaults.detail(); }
}
