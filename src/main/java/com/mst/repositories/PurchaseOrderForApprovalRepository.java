package com.mst.repositories;

import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

/**
 * Procedure calls of Architecture.WinApp.ApprovalDashboard\PurchaseOrderForApproval.cs (TypeID 20) and of
 * Architecture.WinApp.FeedMill.Approval\PurchaseOrderForApproval.cs (TypeID 1100), parameter for parameter
 * as the desktop BLL builds them (DesktopProc omits a null, exactly as AddWithValue(null) does).
 *
 * TypeID 20 - Architecture.BLL.Inventory.PurchaseOrder (BLL 0595)
 *   GetDataForDropDownFromPurchaseOrder (:1948)        dbo.USP_GetDataForDropDownFromPurchaseOrder
 *   PurchaseOrderHistory (:494)                        dbo.Sp_PurchaseOrder_History_Rpt
 *   UpdateStatusandIsApprovedbyOrderId (:678)          dbo.Sp_PurchaseOrder_GetAllMethod @Activity='UpdateStatusandIsapprovedByOrderId'
 *                                                      (one call per order, no transaction; the procedure writes the user audit row)
 *
 * TypeID 1100 - Architecture.BLL.FeedMill.Purchase.PurchaseOrder (BLL 0427)
 *   GetDataForDropDownFromPurchaseOrder (:268)         fed.USP_DropDownFillFromPurchaseOrder
 *   PurchaseOrderHistory (:299)                        fed.usp_PurchaseOrder_History_Rpt
 *   PurchaseOrderandVoucherApprove (:443)              fed.USP_PurchaseOrder_GetAllMethod @Activity='PurchaseOrderandVoucherApprove'
 *
 * Plus the rights lookup CommonServices.SetRightsValueInRightsObject makes:
 *   tblUserRights.GetByUserId (BLL 0084:39)            dbo.Sp_tblUserRights_GetAllMethod @Activity='GetByUserId'
 */
@Repository
public class PurchaseOrderForApprovalRepository {

    private final JdbcTemplate jdbc;

    public PurchaseOrderForApprovalRepository(JdbcTemplate jdbc) {
        this.jdbc = jdbc;
    }

    // ------------------------------------------------------------------ TypeID 20

    /** PurchaseOrder.GetDataForDropDownFromPurchaseOrder: DocumentTypeIds "41", BranchesIds only when set. */
    public List<Map<String, Object>> dropDownPo(int org, int comp, String documentTypeIds, String branchesIds) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", org, "CompanyId", comp);
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromPurchaseOrder", p);
    }

    /**
     * PurchaseOrder.PurchaseOrderHistory (BLL 0595:494).
     *
     * @param isApproved null = @IsApproved not sent (ApprovedFilter "All")
     * @param status     blank = @Status not sent
     * @param postState  ReportsParameters.ReqType is "False" / "True" (never "All") so @PostState = ActionTypeId is always sent
     */
    public List<Map<String, Object>> historyPo(int org, int comp, int documentTypeId, Timestamp from, Timestamp to,
                                                int docNoFrom, int docNoTo, int supplierId, int itemId,
                                                Boolean isApproved, String status, boolean postState, String branchesIds) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", org, "CompanyId", comp, "DocumentTypeId", documentTypeId);
        if (from != null) p.put("StartOrderDate", from);
        if (to != null) p.put("EndOrderDate", to);
        if (docNoFrom != 0) p.put("PoSrFrom", docNoFrom);
        if (docNoTo != 0) p.put("PoSrTo", docNoTo);
        if (supplierId != 0) p.put("SupplierCustomerId", supplierId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (isApproved != null) p.put("IsApproved", isApproved);
        if (status != null && !status.isEmpty()) p.put("Status", status);
        p.put("PostState", postState);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        return DesktopProc.rows(jdbc, "Sp_PurchaseOrder_History_Rpt", p);
    }

    /** PurchaseOrder.UpdateStatusandIsApprovedbyOrderId (BLL 0595:678), one order. */
    public void updatePo(int org, int comp, int userId, int id, String reqType) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", org, "CompanyId", comp, "PostUser", userId);
        if (reqType != null && !reqType.isEmpty()) p.put("ReqType", reqType);
        if (id != 0) p.put("Id", id);
        p.put("Activity", "UpdateStatusandIsapprovedByOrderId");
        DesktopProc.rows(jdbc, "Sp_PurchaseOrder_GetAllMethod", p);
    }

    // ------------------------------------------------------------------ TypeID 1100

    /** FeedMill PurchaseOrder.GetDataForDropDownFromPurchaseOrder (BLL 0427:268): @Activity 'Supplier' / 'Item'. */
    public List<Map<String, Object>> dropDownFm(int org, int comp, String activity) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", org, "CompanyId", comp);
        if (activity != null) p.put("Activity", activity);
        return DesktopProc.rows(jdbc, "[fed].[USP_DropDownFillFromPurchaseOrder]", p);
    }

    /** FeedMill PurchaseOrder.PurchaseOrderHistory (BLL 0427:299); @Status is always sent by the BLL. */
    public List<Map<String, Object>> historyFm(int org, int comp, int documentTypeId, Timestamp from, Timestamp to,
                                                int docNoFrom, int docNoTo, int supplierId, int itemId,
                                                Boolean isApproved, String status, int pendingForView) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", org, "CompanyId", comp);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        if (from != null) p.put("StartOrderDate", from);
        if (to != null) p.put("EndOrderDate", to);
        if (docNoFrom != 0) p.put("PoSrFrom", docNoFrom);
        if (docNoTo != 0) p.put("PoSrTo", docNoTo);
        if (supplierId != 0) p.put("SupplierCustomerId", supplierId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (isApproved != null) p.put("IsApproved", isApproved);
        p.put("Status", status);
        if (pendingForView != 0) p.put("PendingForView", pendingForView);
        return DesktopProc.rows(jdbc, "[fed].[usp_PurchaseOrder_History_Rpt]", p);
    }

    /** FeedMill PurchaseOrder.PurchaseOrderandVoucherApprove (BLL 0427:443), one order. */
    public void updateFm(int userId, int id, String reqType) {
        Map<String, Object> p = DesktopProc.params("ReqType", reqType, "ApprovedUserId", userId, "Id", id,
                "Activity", "PurchaseOrderandVoucherApprove");
        DesktopProc.rows(jdbc, "[fed].[USP_PurchaseOrder_GetAllMethod]", p);
    }

    /** Web-only guard: the FeedMill procedure updates by Id alone, so the order must belong to this company first. */
    public boolean fmOrderBelongsTo(int org, int comp, int id) {
        Integer n = jdbc.queryForObject(
                "SELECT COUNT(1) FROM fed.PurchaseOrder WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?",
                Integer.class, id, org, comp);
        return n != null && n > 0;
    }

    // ------------------------------------------------------------------ rights and lookups

    /** tblUserRights.GetByUserId (BLL 0084:39): @UserId @ScreenName @RightName(=role name) @CompanyId @Activity. */
    public List<Map<String, Object>> rightsByUser(int userId, String screenName, String roleName, int comp) {
        Map<String, Object> p = DesktopProc.params("UserId", userId, "ScreenName", screenName, "RightName", roleName,
                "CompanyId", comp, "Activity", "GetByUserId");
        return DesktopProc.rows(jdbc, "Sp_tblUserRights_GetAllMethod", p);
    }

    /** CommonServices.GetGlAccountIdBySupplierCustomerId (GoToCustomerLedgerFromLinkedEvent). */
    public List<Map<String, Object>> glAccountOfParty(int org, int comp, int supplierCustomerId) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", org, "CompanyId", comp,
                "SupplierCustomerId", supplierCustomerId, "Activity", "GetGlAccountIdBySupplierCustomerId");
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", p);
    }

    /** clsGlobalVariables.ActiveYr.Start_Period source. */
    public List<Map<String, Object>> activeYears(int org, int comp) {
        return DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId",
                DesktopProc.params("OrganizationId", org, "CompanyId", comp));
    }
}
