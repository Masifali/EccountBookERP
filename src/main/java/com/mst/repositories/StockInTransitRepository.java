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
 * Screen 868 "Stock In Transit" — frmSupplierDispatchPreBill.cs (Architecture.WinApp.Purchase),
 * DocumentTypeId 251 (frmSupplierDispatchPreBill.cs:870 / :898 / :2074 / :3340).
 *
 * Every procedure, @Activity and parameter below was read from the decompiled desktop source and
 * checked against procs/procdure.utf8.sql:
 *
 *   BLL 0517 / DAL 0383  Architecture.*.Inventory.SupplierDispatch
 *        GenerateCode / GenerateBranchCode / ReadById (+ ReadByHeaderId, ReadByHeaderId_SupplierDispatchExpense,
 *        ReadByHeaderId_SupplierDispatchCommDetail) / LastSavedRecord / FormHistory / DeleteById /
 *        SupplierDispatchApprove                    -> [dbo].[USP_SupplierDispatch_GetAllMethod]
 *        GetBranchsAllocatedToUserFromSupplierDispatch -> [dbo].[USP_GetBranchsAllocatedToUserFromSupplierDispatch]
 *        Save (SetData)  -> [dbo].[USP_SupplierDispatch_InsertAndUpdate], [dbo].[USP_SupplierDispatchDetail_Insert],
 *                           USP_SupplierDispatchCommDetail_Insert, USP_SupplierDispatchExpense_Insert
 *   BLL 0595  PurchaseOrder.GetPurchaseOrderForPurchaseInvoice (the LoadPurchaseOrder dialog, DocumentTypeId 41)
 *   BLL 0581  InvPurchaseInvoice.PurchaseOrderSupplierExpenseByOrderIds (LoadExpData :4654)
 *   Globals   USP_GetVendorsAndCustomersWithCityName, USP_Item_AllItemsWithModal, Sp_InvCropYear_GetAllMethod,
 *             Sp_InvPackingType_GetAllMethod, USP_City_GetAllWithCountryAndTehsil, usp_getAllUomsByCompanyId,
 *             Sp_MultiCurrency_GetAllMethod, Sp_ReferenceParties_GetAllMethod, USP_DeliveryTerm_GetAllMethod,
 *             Sp_InventoryItemsOther_GetAllMethod, USP_GetBranchsAllocatedToUser, SpStaticColumnNames,
 *             USP_GetERPFeaturesByCompanyId, Sp_ConfigrationsAllocation_GetAllMethod
 *
 * A parameter the BLL only adds under a condition is only added here under the same condition; a
 * null is never bound (DesktopProc omits it, as ADO.NET omits a CLR null). Nothing here catches:
 * a failing procedure is a failing request.
 */
@Repository
public class StockInTransitRepository {

    public static final String P_GETALL = "[dbo].[USP_SupplierDispatch_GetAllMethod]";

    private final JdbcTemplate jdbc;
    public StockInTransitRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =========================================================================== numbering

    /** BLL 0517 GenerateCode (:38) — org, comp, DocumentTypeId, FinancialYearId; first row's DocNo. */
    public int generateCode(UserAccount u, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId, "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : StoreIssuanceRepository.toInt(r.get(0).get("DocNo"));
    }

    /** BLL 0517 GenerateBranchCode (:83) — adds @BranchesId (the user's branch). */
    public int generateBranchCode(UserAccount u, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId,
                "BranchesId", branch(u), "Activity", "GenerateBranchCode"));
        return r.isEmpty() ? 0 : StoreIssuanceRepository.toInt(r.get(0).get("BranchSrNo"));
    }

    // ============================================================================= globals

    /** clsGlobalVariables.globalSupplierCustomer source — USP_GetVendorsAndCustomersWithCityName (org, comp). */
    public List<Map<String, Object>> vendorsAndCustomers(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** CommonServices.ReferencePartyServiceBind -> ReferenceParties.GetAll -> 'ReadAll'. */
    public List<Map<String, Object>> referenceParties(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_ReferenceParties_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** DeliveryTerm.FormHistory() (BLL 0512) — no organization / company parameter. */
    public List<Map<String, Object>> deliveryTerms() {
        return DesktopProc.rows(jdbc, "[dbo].[USP_DeliveryTerm_GetAllMethod]", params("Activity", "FormHistory"));
    }

    /** CurrencyDbCall (:1053) — MultiCurrency.GetAll: org, comp, 'ReadAll'. */
    public List<Map<String, Object>> currencies(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_MultiCurrency_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** clsGlobalVariables.getGlobalAllItems — USP_Item_AllItemsWithModal (org, comp). */
    public List<Map<String, Object>> allItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_Item_AllItemsWithModal]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.globalCropYear — Sp_InvCropYear_GetAllMethod org, comp, 'ReadAll'. */
    public List<Map<String, Object>> cropYears(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[Sp_InvCropYear_GetAllMethod]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** clsGlobalVariables.globalInvPackingType — Sp_InvPackingType_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> packingTypes() {
        return DesktopProc.rows(jdbc, "[dbo].[Sp_InvPackingType_GetAllMethod]", params("Activity", "ReadAll"));
    }

    /** clsGlobalVariables.globalAllCities — USP_City_GetAllWithCountryAndTehsil (org, comp). */
    public List<Map<String, Object>> cities(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_City_GetAllWithCountryAndTehsil]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.globalUomSchedule — usp_getAllUomsByCompanyId (org, comp, Active 1). */
    public List<Map<String, Object>> uomSchedule(UserAccount u) {
        return DesktopProc.rows(jdbc, "usp_getAllUomsByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Active", 1));
    }

    /** OtherItemdtDbCall (:1195) — InventoryItemsOther.GetAll; "@organizationId" is the BLL's spelling. */
    public List<Map<String, Object>> otherItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod", params(
                "Activity", "ReadAll", "organizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** OtherItemdtDbCall (:1205) — CommonServices.StaticColumnsService("SupplierDispatchStatus"). */
    public List<Map<String, Object>> statuses() {
        return DesktopProc.rows(jdbc, "SpStaticColumnNames", params("Activity", "SupplierDispatchStatus"));
    }

    /** BranchDetailDbCall (:1169) — BranchesAllocationToUser.GetBranchsAllocatedToUser(org, comp, user). */
    public List<Map<String, Object>> branchesAllocatedToUser(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUser]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId()));
    }

    /** HistoryBranchComboDbCall (:772) — DocumentTypeId passed as 0, so the BLL leaves it out. */
    public List<Map<String, Object>> historyBranches(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromSupplierDispatch]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId()));
    }

    /** CommonServices.GetERPFeatureById(id): the id is among USP_GetERPFeaturesByCompanyId's rows. */
    public boolean feature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (StoreIssuanceRepository.toInt(r.get("Id")) == featureId) return true;
        }
        return false;
    }

    /** GetConfigurationByOrgCompandConfigDescription / GetConfigValueFromGlobal — ConfigKey or "". */
    public String config(UserAccount u, String description) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        if (r.isEmpty()) return "";
        Object v = r.get(0).get("ConfigKey");
        return v == null ? "" : String.valueOf(v);
    }

    // ============================================================================= form

    /** CmbSupplierDetail_Leave (:1273) — LastSavedRecord(org, comp, FY, supplier, 0): @ItemId omitted. */
    public List<Map<String, Object>> lastSavedRecord(UserAccount u, int financialYearId, int supplierId) {
        return DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId, "SupplierCustomerId", supplierId, "Activity", "LastSavedRecord"));
    }

    /** BLL 0517 GetByID — the header row (ActionId &lt;&gt; 3; no organization filter in the procedure). */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0383 GetData — ReadByHeaderId (detail rows with ActionTypeId &lt;&gt; 3). */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, P_GETALL, params("Id", id, "Activity", "ReadByHeaderId"));
    }

    /** DAL 0383 GetData — ReadByHeaderId_SupplierDispatchExpense. */
    public List<Map<String, Object>> expenses(int id) {
        return DesktopProc.rows(jdbc, "USP_SupplierDispatch_GetAllMethod", params("Id", id, "Activity", "ReadByHeaderId_SupplierDispatchExpense"));
    }

    /** DAL 0383 GetData — ReadByHeaderId_SupplierDispatchCommDetail. */
    public List<Map<String, Object>> commDetails(int id) {
        return DesktopProc.rows(jdbc, "USP_SupplierDispatch_GetAllMethod", params("Id", id, "Activity", "ReadByHeaderId_SupplierDispatchCommDetail"));
    }

    /**
     * BLL 0517 FormHistory (:190). @BranchesId is never sent (the form never sets it, and the BLL
     * guards it with != 0); @BranchesIds is always sent (the BLL's guard is always true).
     */
    public List<Map<String, Object>> formHistory(UserAccount u, int financialYearId, int documentTypeId, boolean canViewAll,
                                                 Timestamp fromDate, Timestamp toDate, int fromDocNo, int toDocNo,
                                                 Timestamp entryFrom, Timestamp entryTo, Timestamp modifyFrom, Timestamp modifyTo,
                                                 Timestamp approvedFrom, Timestamp approvedTo, String branchesIds) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId,
                "CanViewAllRecord", canViewAll);
        if (!canViewAll) p.put("EntryUser", u.getId());       // desktop quirk: the procedure filters on @EntryUserId
        p.put("FromDate", fromDate);
        p.put("ToDate", toDate);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        p.put("EntryFromDate", entryFrom);
        p.put("EntryToDate", entryTo);
        p.put("ModifyFromDate", modifyFrom);
        p.put("ModifyToDate", modifyTo);
        p.put("ApprovedFromDate", approvedFrom);
        p.put("ApprovedToDate", approvedTo);
        p.put("BranchesIds", branchesIds == null ? "" : branchesIds);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    // ================================================================================ save

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Map<String, Object> model) { return DesktopProc.setProc(jdbc, proc, model); }

    /** BLL 0517 DeleteByID — @EntryUserId, @Id, @Activity 'DeleteById' (ExecuteNonQuery). */
    public void deleteById(int entryUserId, int id) {
        DesktopProc.scalar(jdbc, P_GETALL, params("EntryUserId", entryUserId, "Id", id, "Activity", "DeleteById"));
    }

    /** BLL 0517 SupplierDispatchApprove — one call per row: @ReqType, @Status, @EntryUserId, @Id, @Activity. */
    public void supplierDispatchApprove(String reqType, String status, int entryUserId, int id) {
        DesktopProc.scalar(jdbc, P_GETALL, params("ReqType", reqType, "Status", status == null ? "" : status,
                "EntryUserId", entryUserId, "Id", id, "Activity", "SupplierDispatchApprove"));
    }

    // ============================================================================== loaders

    /**
     * LoadPurchaseOrder (DocumentTypeId 41, ReturnFulldt) -> BLL 0595 GetPurchaseOrderForPurchaseInvoice:
     * org, comp always; FromDocNo / ToDocNo / Id / DocumentTypeId / OrderSupCustId only when non-zero;
     * DocDateFrom / DocDateTo only when set; @Ids only when non-empty.
     */
    public List<Map<String, Object>> purchaseOrdersForLoader(UserAccount u, int documentTypeId, int supplierId, int orderId,
                                                             Timestamp fromDate, Timestamp toDate, int fromDocNo, int toDocNo) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (orderId != 0) p.put("Id", orderId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (supplierId != 0) p.put("OrderSupCustId", supplierId);
        p.put("DocDateFrom", fromDate);
        p.put("DocDateTo", toDate);
        p.put("Activity", "GetPurchaseOrderForPurchaseInvoice");
        return DesktopProc.rows(jdbc, "Sp_PurchaseOrder_GetAllMethod", p);
    }

    /** LoadExpData (:4654) -> BLL 0581 PurchaseOrderSupplierExpenseByOrderIds(@OrderIds). */
    public List<Map<String, Object>> purchaseOrderSupplierExpenses(String orderIds) {
        return DesktopProc.rows(jdbc, "[Sp_InvPurchaseInvoice_GetAllMethod]", params(
                "OrderIds", orderIds, "Activity", "PurchaseOrderSupplierExpenseByOrderIds"));
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
}
