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
 * Screen 961 "Store Purchase Pre Bill" (frmPurchasePreBill.cs, DocumentTypeId 147) and its
 * Load-Demand dialog (frmPendingPurchaseDemand.cs, Purchase Demand DocumentTypeId 141).
 *
 * Every procedure, @Activity and parameter below was read from the recovered desktop source and
 * checked against /root/ddl/procs.json:
 *
 *   BLL 0314 / DAL 0254  Architecture.*.StoreManagement.PurchasePreBillHeader
 *                        (GenerateCode, ReadById + both child reads, FormHistory, DeleteByID,
 *                         GetDataForDropDownFromPurchasePreBillHeader, PurchaseDemand_PendingDataLoader,
 *                         PurchasePreBillHeader_Slip, PurchasePreBillHeader_ExpenseSubReport, SetData)
 *   BLL 0557             InvPurchaseDemandHeader.GetDataForDropDownFromPurchaseDemand (loader Item combo)
 *   BLL 0074             ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId
 *   BLL 0512             Inventory.DeliveryTerm.FormHistory
 *   BLL 0573             InventoryItemsOther.GetAll
 *   BLL 0379             GlobalServicesMethods (suppliers, cities)
 *
 * A parameter the BLL only adds under a condition is only added here under the same condition; a
 * null is never bound (DesktopProc omits it, as ADO.NET omits a CLR null).
 */
@Repository
public class PurchasePreBillRepository {

    public static final String P_GETALL = "[dbo].[USP_PurchasePreBillHeader_GetAllMethod]";

    private final JdbcTemplate jdbc;
    public PurchasePreBillRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =========================================================================== numbering

    /** BLL 0314 GenerateCode — first row's DocNo, 0 when none. */
    public int nextDocNo(UserAccount u, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branch(u), "FinancialYearId", financialYearId,
                "DocumentTypeId", documentTypeId, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : StoreIssuanceRepository.toInt(r.get(0).get("DocNo"));
    }

    // ============================================================================= globals

    /** clsGlobalVariables.globalAllSupplierCustomer — BLL 0379 getGlobalSupplierCustomer(org, comp, 0, 0, 0, ""). */
    public List<Map<String, Object>> vendorsAndCustomers(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** ReferancePartyDbCall (:528) — BLL 0074 with ReferencePartyTypeId 4 (SupplierCustomerId 0: not sent). */
    public List<Map<String, Object>> referenceParties(UserAccount u, int referencePartyTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (referencePartyTypeId != 0) p.put("ReferencePartyTypeId", referencePartyTypeId);
        p.put("Activity", "ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId");
        return DesktopProc.rows(jdbc, "[Sp_ReferenceParties_GetAllMethod]", p);
    }

    /** DeliveryTerm.FormHistory() (BLL 0512) — no organization / company parameter at all. */
    public List<Map<String, Object>> deliveryTerms() {
        return DesktopProc.rows(jdbc, "[dbo].[USP_DeliveryTerm_GetAllMethod]", params("Activity", "FormHistory"));
    }

    /** clsGlobalVariables.globalAllCities — BLL 0379 getGlobalAllCity(org, comp). */
    public List<Map<String, Object>> cities(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_City_GetAllWithCountryAndTehsil]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** OtherItemdtDbCall (:605) — BLL 0573 InventoryItemsOther.GetAll; "@organizationId" is the BLL's spelling. */
    public List<Map<String, Object>> otherItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod", params(
                "Activity", "ReadAll", "organizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    // ============================================================================= history

    /** HistoryComboDbCall (:1433) — ReportsParameters.Activity is null, so @Activity is not sent. */
    public List<Map<String, Object>> historyCombos(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromPurchasePreBillHeader]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** BLL 0314 FormHistory — every optional parameter under the BLL's own condition. */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, int documentTypeId, boolean canViewAll,
                                                 Timestamp fromDate, Timestamp toDate, Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo, Timestamp approvedFrom, Timestamp approvedTo,
                                                 int fromDocNo, int toDocNo, int billToPartyId, int itemId, int onlyPending) {
        Map<String, Object> p = params("Activity", "FormHistory",
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branch(u), "FinancialYearId", financialYearId,
                "CanViewAllRecord", canViewAll ? 1 : 0);
        if (!canViewAll) p.put("EntryUserId", u.getId());
        p.put("FromDate", fromDate);
        p.put("ToDate", toDate);
        p.put("EntryFromDate", entryFrom);
        p.put("EntryToDate", entryTo);
        p.put("ModifyFromDate", modifyFrom);
        p.put("ModifyToDate", modifyTo);
        p.put("ApprovedFromDate", approvedFrom);
        p.put("ApprovedToDate", approvedTo);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (billToPartyId != 0) p.put("BillToPartyId", billToPartyId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (onlyPending != 0) p.put("OnlyPending", onlyPending);
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    // ================================================================================ read

    /** BLL 0314 ReadById — the header row (no organization / company filter in the procedure). */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0254 GetData — ReadByHeaderId_PurchasePreBillDetail (rows with ActionTypeId 3 excluded). */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadByHeaderId_PurchasePreBillDetail"));
    }

    /** DAL 0254 GetData — ReadByHeaderId_PurchasePreBillExpenseDetail. */
    public List<Map<String, Object>> expenses(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadByHeaderId_PurchasePreBillExpenseDetail"));
    }

    // ================================================================================ save

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Map<String, Object> model) { return DesktopProc.setProc(jdbc, proc, model); }

    /** BLL 0314 DeleteByID — @EntryUserId, @Id, @Activity = 'DeleteById' (ExecuteNonQuery). */
    public void deleteById(int entryUserId, int id) {
        DesktopProc.scalar(jdbc, P_GETALL, params("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    // ============================================================================== loader

    /** frmPendingPurchaseDemand ComboDbCall (:140) — BLL 0557 (org, comp, "141", "Item", null, 0). */
    public List<Map<String, Object>> demandDropDowns(UserAccount u, String documentTypeIds, String activity) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (activity != null && !activity.isEmpty()) p.put("Activity", activity);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromPurchaseDemand]", p);
    }

    /**
     * frmPendingPurchaseDemand PendingDataDbCall (:195) → BLL 0314 PurchaseDemand_PendingDataLoader.
     * Pass null / 0 to leave a filter out, exactly as the BLL's guards do.
     */
    public List<Map<String, Object>> pendingDemands(UserAccount u, int financialYearId, int documentTypeId,
                                                    Timestamp fromDate, Timestamp toDate, int fromDocNo, int toDocNo, int itemId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branch(u), "FinancialYearId", financialYearId, "DocumentTypeId", documentTypeId);
        p.put("FromDate", fromDate);
        p.put("ToDate", toDate);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (itemId != 0) p.put("ItemId", itemId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_InvPurchaseDemand_PendingDataLoader]", p);
    }

    // =============================================================================== print

    /** CrystalReportPrint_Helper.StorePurchasePreBillSlip147 → BLL 0314 PurchasePreBillHeader_Slip (Tables[0]). */
    public List<Map<String, Object>> slip(UserAccount u, int financialYearId, int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_PurchasePreBillHeader_Slip]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branch(u), "FinancialYearId", financialYearId, "Id", id));
    }

    /** The slip's sub-report PurchasePreBillHeader_ExpenseSubReport.rpt. */
    public List<Map<String, Object>> expenseSubReport(int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_PurchasePreBillHeader_ExpenseSubReport]", params("PurchasePreBillHeaderId", id));
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
}
