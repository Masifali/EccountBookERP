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
 * Screen 960 "Delivery Challan Against PreBill" — every procedure the form, its loader and its BLL
 * call, one method per BLL method, parameters in the BLL's order and with the BLL's omissions
 * (a null value is not sent, exactly as AddWithValue(null) / the BLL's "if (x != 0)" guards).
 *
 *   BLL 0313 Architecture.BLL.StoreManagement.DeliveryChallanHeader
 *   DAL 0253 Architecture.DAL.StoreManagement.DeliveryChallanHeader (SetData / GetData)
 *   BLL 0314 Architecture.BLL.StoreManagement.PurchasePreBillHeader  (loader, dropdowns, expenses, 147 slip)
 *   BLL 0512 Architecture.BLL.Inventory.DeliveryTerm.FormHistory
 *   BLL 0573 Architecture.BLL.Inventory.InventoryItemsOther.GetAll
 *   BLL 0379 getGlobalAllCity (clsGlobalVariables.globalAllCities)
 */
@Repository
public class DeliveryChallanPreBillRepository {

    public static final String P_GETALL = "[dbo].[USP_DeliveryChallanHeader_GetAllMethod]";
    public static final String P_SAVE = "[dbo].[USP_DeliveryChallanHeader_InsertAndUpdate]";
    public static final String P_DETAIL = "[dbo].[USP_DeliveryChallanDetail_Insert]";
    public static final String P_EXPENSE = "[dbo].[USP_DeliveryChallanExpenseDetail_Insert]";

    private final JdbcTemplate jdbc;

    public DeliveryChallanPreBillRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ================================================================= BLL 0313 DeliveryChallanHeader

    /** GenerateCode — reads DocNo; 0 when no row. */
    public int generateCode(UserAccount u, int branchesId, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId,
                "FinancialYearId", financialYearId,
                "DocumentTypeId", documentTypeId,
                "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : StoreIssuanceRepository.toInt(r.get(0).get("DocNo"));
    }

    /** ReadById — the header row (null when none; the BLL's GetData(...)[0] throws there). */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL GetData :126 — the detail list of one header. */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadByHeaderId_DeliveryChallanDetail"));
    }

    /** DAL GetData :139 — the expense list of one header. */
    public List<Map<String, Object>> expenses(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadByHeaderId_DeliveryChallanExpenseDetail"));
    }

    /** DeleteByID — @EntryUserId, @Id, @Activity, in its own transaction (the caller's). */
    public void deleteById(int entryUserId, int id) {
        DesktopProc.scalar(jdbc, P_GETALL, params("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    /**
     * FormHistory — parameters in the BLL's order: Activity, OrganizationId, CompanyId, BranchesId,
     * FinancialYearId, CanViewAllRecord, EntryUserId (only when !CanViewAllRecord), the checked
     * date pair of the chosen date type, then FromDocNo/ToDocNo/Id/CityId/ItemId/OnlyPending only
     * when non-zero. DocumentTypeId is set on the ReportsParameters but the BLL never sends it.
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int branchesId, int financialYearId, boolean canViewAll,
                                                 String dateType, Timestamp from, Timestamp to,
                                                 int fromDocNo, int toDocNo, int cityId, int itemId) {
        Map<String, Object> p = params(
                "Activity", "FormHistory",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId,
                "FinancialYearId", financialYearId,
                "CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUserId", u.getId());
        String f, t;
        switch (dateType == null ? "doc" : dateType) {
            case "entry":    f = "EntryFromDate";    t = "EntryToDate";    break;
            case "modify":   f = "ModifyFromDate";   t = "ModifyToDate";   break;
            case "approved": f = "ApprovedFromDate"; t = "ApprovedToDate"; break;
            default:         f = "FromDate";         t = "ToDate";         break;
        }
        if (from != null) p.put(f, from);
        if (to != null) p.put(t, to);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (cityId != 0) p.put("CityId", cityId);
        if (itemId != 0) p.put("ItemId", itemId);
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    /** GetDataForDropDownFromDeliveryChallanHeader — @Activity not sent (HistoryComboDbCall sets none). */
    public List<Map<String, Object>> historyDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromDeliveryChallanHeader]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** DeliveryChallanHeader_Slip — first table of the DataSet (the second is the company logo). */
    public List<Map<String, Object>> slip(UserAccount u, int branchesId, int financialYearId, int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_DeliveryChallanHeader_Slip]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId, "FinancialYearId", financialYearId, "Id", id));
    }

    /** DeliveryChallanHeader_ExpenseSubReport(Id). */
    public List<Map<String, Object>> slipExpenses(int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_DeliveryChallan_ExpenseSubReport]", params("DeliveryChallanHeaderId", id));
    }

    // ================================================================= BLL 0314 PurchasePreBillHeader

    /** GetDataForDropDownFromPurchasePreBillHeader — the loader's ComboDbCall; @Activity not sent. */
    public List<Map<String, Object>> preBillDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromPurchasePreBillHeader]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** PurchasePreBillHeader_PendingDataLoader — the optional filters only when set / non-zero. */
    public List<Map<String, Object>> pendingPreBills(UserAccount u, int branchesId, int financialYearId, int documentTypeId,
                                                     Timestamp from, Timestamp to, int fromDocNo, int toDocNo,
                                                     int itemId, int billToPartyId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId,
                "FinancialYearId", financialYearId,
                "DocumentTypeId", documentTypeId);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (itemId != 0) p.put("ItemId", itemId);
        if (billToPartyId != 0) p.put("BillToPartyId", billToPartyId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_PurchasePreBillHeader_PendingDataLoader]", p);
    }

    /** PurchasePreBillExpenseDetail_GetByHeaderIds — comma list, not company-scoped by the procedure. */
    public List<Map<String, Object>> preBillExpensesByHeaderIds(String ids) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_PurchasePreBillExpenseDetail_GetByHeaderIds]", params("HeaderIds", ids));
    }

    /** PurchasePreBillHeader_Slip (147) — first table. */
    public List<Map<String, Object>> preBillSlip(UserAccount u, int branchesId, int financialYearId, int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_PurchasePreBillHeader_Slip]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId, "FinancialYearId", financialYearId, "Id", id));
    }

    /** PurchasePreBillHeader_ExpenseSubReport(Id). */
    public List<Map<String, Object>> preBillSlipExpenses(int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_PurchasePreBillHeader_ExpenseSubReport]", params("PurchasePreBillHeaderId", id));
    }

    // ================================================================= globals / lookups

    /** BLL 0512 DeliveryTerm.FormHistory. */
    public List<Map<String, Object>> deliveryTerms() {
        return DesktopProc.rows(jdbc, "[dbo].[USP_DeliveryTerm_GetAllMethod]", params("Activity", "FormHistory"));
    }

    /** BLL 0573 InventoryItemsOther.GetAll — Activity ReadAll. */
    public List<Map<String, Object>> otherItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod", params(
                "Activity", "ReadAll", "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.globalAllCities — BLL 0379 getGlobalAllCity. */
    public List<Map<String, Object>> cities(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_City_GetAllWithCountryAndTehsil]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    // ================================================================= SetProc

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }
}
