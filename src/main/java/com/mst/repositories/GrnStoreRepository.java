package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 324 "Grn Store" (GrnStore.cs, DocumentTypeId 48) — every procedure the form and its three
 * loaders call, with each parameter added under the same condition the BLL adds it.
 *
 *   BLL 0576 / DAL 0429  Architecture.(BLL|DAL).Inventory.InvGrn
 *   BLL 0611 / DAL 0464  VehicleType.GetAll
 *   BLL 0557             InvPurchaseDemandHeader (GetPendingPurchaseOrderForGrnStore, GetDataForDropDownFromPurchaseDemand)
 *   BLL 0595             PurchaseOrder.GetDataForDropDownFromPurchaseOrder
 *   BLL 0314             PurchasePreBillHeader.PurchaseDemand_PendingDataLoader
 *   BLL 0313             DeliveryChallanHeader (GetDataForDropDownFromDeliveryChallanHeader, _PendingDataLoader)
 *   BLL 0131             InvGrnandGdnReports.GrnSlipStore (212 slip)
 *   BLL 0134             PurchaseOrderReports.PurchaseOrderSlipReport203 (register OrderNo link)
 *
 * A null value is never bound (DesktopProc omits it), which is what a SqlParameter whose Value is
 * a CLR null does on the desktop.
 */
@Repository
public class GrnStoreRepository {

    public static final String P_GRN_GETALL = "Sp_InvGRN_GetAllMethod";
    public static final String P_GRN_DETAIL_GETALL = "Sp_InvGrnDetail_GetAllMethod";

    private final JdbcTemplate jdbc;
    public GrnStoreRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =============================================================================== numbering

    /** BLL 0576 GenerateInvGrnCode:192 — FinancialYearId and BranchesId only when non-zero. */
    public int nextDocNo(UserAccount u, int documentTypeId, int financialYearId, int branchesId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (branchesId != 0) p.put("BranchesId", branchesId);
        p.put("Activity", "GenerateInvGrnCode");
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GRN_GETALL, p);
        return r.isEmpty() ? 0 : toInt(r.get(0).get("DocNo"));
    }

    // ================================================================================= combos

    /** BLL 0611 VehicleType.GetAll — the @Activity list is built and thrown away; the proc takes no parameters. */
    public List<Map<String, Object>> vehicleTypes() {
        return DesktopProc.rows(jdbc, "Sp_VehicleType_GetAllMethod", null);
    }

    /** BLL 0576 GetDataForDropDownFromGrn:1978 (history and register combos). */
    public List<Map<String, Object>> grnDropDowns(UserAccount u, String documentTypeIds, String branchesIds, int financialYearId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromGrn]", p);
    }

    /** clsGlobalVariables.globalSupplierCustomer — GlobalServicesMethods.getGlobalSupplierCustomer. */
    public List<Map<String, Object>> suppliers(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.getGlobalVendorsAndCustomersForTransporters. */
    public List<Map<String, Object>> vendorsAndCustomersForTransporter(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersForTransporter",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.AllAccountsWithCustomGroupId. */
    public List<Map<String, Object>> accountsWithCustomGroup(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** CommonServices.GetERPFeatureById(id) — membership in the login-time USP_GetERPFeaturesByCompanyId list. */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (toInt(r.get("Id")) == featureId) return true;
        }
        return false;
    }

    // ================================================================================ history

    /** BLL 0576 GetHisoty:334 ('GRNFormHistory') — the caller passes nulls for what the BLL leaves out. */
    public List<Map<String, Object>> history(Map<String, Object> p) {
        p.put("Activity", "GRNFormHistory");
        return DesktopProc.rows(jdbc, P_GRN_GETALL, p);
    }

    /** BLL 0576 GrnRegisterStore:1690 → USp_InvGrnStore_Register. */
    public List<Map<String, Object>> register(Map<String, Object> p) {
        return DesktopProc.rows(jdbc, "USp_InvGrnStore_Register", p);
    }

    // =================================================================================== read

    /** BLL 0576 GetByID:168 → DAL GetDate: the header ('ReadByID'), or null. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GRN_GETALL, params("Id", id, "Activity", "ReadByID"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0429 GetDate — DocumentTypeId 48 / 701 read 'ReadByInvGrnIDStore'. */
    public List<Map<String, Object>> storeDetails(int id) {
        return DesktopProc.rows(jdbc, P_GRN_DETAIL_GETALL, params("Id", id, "Activity", "ReadByInvGrnIDStore"));
    }

    /**
     * BLL 0576 GetRecordsById:1608 ('GetRecordId') — the update path reads it into PreviousDate
     * (a virtual property, so never sent). Rows[0][0]: no row is "There is no row at position 0."
     */
    public Object recordDate(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GRN_GETALL, params("Id", id, "Activity", "GetRecordId"));
        if (r.isEmpty()) throw new IllegalArgumentException("There is no row at position 0.");
        return r.get(0).get("DocDate");
    }

    // ================================================================================== write

    /** GenericProvider.SetProc — Convert.ToInt32(ExecuteScalar()). */
    public int setProc(String proc, Map<String, Object> model) {
        return DesktopProc.setProc(jdbc, proc, model);
    }

    /** DAL 0429 SetData:210-214 — InventoryTransactions model, RefDocIdNo = num. */
    public void inventoryTransactions(UserAccount u, int documentTypeId, int refDocIdNo) {
        Map<String, Object> t = InventoryOpeningDefaults.transactions();
        t.put("OrganizationId", u.getOrganizationId());
        t.put("CompanyId", u.getCompanyId());
        t.put("RefDocumentTypeId", documentTypeId);
        t.put("RefDocIdNo", refDocIdNo);
        setProc("Sp_InventoryTransactions_GetALLMethod", t);
    }

    /** DAL 0429 SetData:215-223 — only for DocumentTypeId 48 with a gate pass. */
    public void stockEvaluation(UserAccount u, int documentTypeId, int refDocIdNo) {
        Map<String, Object> e = InventoryOpeningDefaults.evaluation();
        e.put("OrganizationId", u.getOrganizationId());
        e.put("CompanyId", u.getCompanyId());
        e.put("RefDocumentTypeId", documentTypeId);
        e.put("RefDocIdNo", refDocIdNo);
        setProc("Sp_InventoryStockEvalautionDetail_Update", e);
    }

    /** DAL 0429 SetData:377-389 — ExecuteNonQuery (every result drained). */
    public void stockInTransitAndVoucher(UserAccount u, int documentTypeId, int grnId) {
        DesktopProc.scalar(jdbc, "usp_StockInTransitUpdate_StockEvaluationAndVoucherInsertFromGrn", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "GrnId", grnId));
    }

    // ================================================================================= prints

    /** CommonServices.GrnSlipReport212 → BLL 0131 GrnSlipStore (all four parameters always sent). */
    public List<Map<String, Object>> slip(UserAccount u, int documentTypeId, int id) {
        return DesktopProc.rows(jdbc, "Sp_InvGrn_StoreSlip_Rpt", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId,
                "Id", id));
    }

    /**
     * GrdiRegister_LinkClicked:2549 → BLL 0134 PurchaseOrderSlipReport203 with DocumentTypeId 42,
     * OrderId and ApprovedFilter "All" (so @IsApproved is not sent). Status is null, and the BLL's
     * {@code Status != "" || Status != null} is true, so @Status is "added" with a null value —
     * which ADO.NET leaves out.
     */
    public List<Map<String, Object>> purchaseOrderSlip(UserAccount u, int orderId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (orderId != 0) p.put("OrderId", orderId);
        p.put("DocumentTypeId", 42);
        return DesktopProc.rows(jdbc, "Sp_PurchaseOrderSlip_Rpt", p);
    }

    /** CommonServices.PurchaseOrderSlipReport201 → BLL 0134 PurchaseOrderSlipReport201 (Id → @OrderId; ApprovedFilter "All"). */
    public List<Map<String, Object>> purchaseOrderGeneralSlip(UserAccount u, int orderId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (orderId != 0) p.put("OrderId", orderId);
        return DesktopProc.rows(jdbc, "Sp_PurchaseOrder_GeneralOrderSlip_Rpt", p);
    }

    /** CommonServices.PurchaseDemandSlip454 → BLL 0557 InvPurchaseDemondSlip. */
    public List<Map<String, Object>> purchaseDemandSlip(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseDemand_Rpt", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }

    /** CrystalReportPrint_Helper.StoreDeliveryChallanSlip148 → BLL 0313 DeliveryChallanHeader_Slip (Tables[0]). */
    public List<Map<String, Object>> deliveryChallanSlip(UserAccount u, int branchesId, int financialYearId, int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_DeliveryChallanHeader_Slip]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId, "FinancialYearId", financialYearId, "Id", id));
    }

    /**
     * Web tenancy check (not a desktop call): the stored GRN lines already referenced by a live
     * Goods Dispatch Note For Purchase Return (DocumentTypeId 703) — the join Sp_InvGrnDetail_Insert
     * uses for its "already exist in [Goods Dispatched Notes For Purchase Return]" refusal, scoped
     * to this organization and company.
     */
    public List<Integer> grnLinesInPurchaseReturn(UserAccount u, int grnId) {
        return jdbc.queryForList(
                "SELECT DISTINCT gd.GrnDetailId FROM dbo.InvGdnDetail gd "
              + "INNER JOIN dbo.InvGdn gh ON gh.Id = gd.InvGdnId "
              + "WHERE gh.OrganizationId = ? AND gh.CompanyId = ? AND gh.DocumentTypeId = 703 "
              + "AND ISNULL(gh.ActionId,0) <> 3 AND ISNULL(gd.ActionTypeId,0) <> 3 AND gd.GrnId = ?",
                Integer.class, u.getOrganizationId(), u.getCompanyId(), grnId);
    }

    // ================================================================================ loaders

    /** frmPendingPurchaseOrderStoreLoader.ComboDbCall → BLL 0595 GetDataForDropDownFromPurchaseOrder. */
    public List<Map<String, Object>> purchaseOrderDropDowns(UserAccount u, String documentTypeIds, String branchesIds) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        return DesktopProc.rows(jdbc, "USP_GetDataForDropDownFromPurchaseOrder", p);
    }

    /**
     * BLL 0557 GetPendingPurchaseOrderForGrnStore. @GdnIds / @SaleInvoiceDocumentTypeIds are
     * "added" with null values (null != "") and so never sent. @FromDocNo / @ToDocNo are sent
     * under those names although the procedure declares @DocNoFrom / @DocNoTo — kept as the BLL
     * has it (see the service's desktop note). ItemId is set by the form but never sent.
     */
    public List<Map<String, Object>> pendingPurchaseOrders(UserAccount u, int documentTypeId, int financialYearId, int branchesId,
                                                           int supplierId, Timestamp from, Timestamp to, int fromDocNo, int toDocNo) {
        Map<String, Object> p = params(
                "Activity", "GetPendingPurchaseOrderForGrnStore",
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId());
        if (supplierId != 0)      p.put("SupplierCustomerId", supplierId);
        if (branchesId != 0)      p.put("BranchesId", branchesId);
        if (documentTypeId != 0)  p.put("DocumentTypeId", documentTypeId);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (from != null)         p.put("FromDate", from);
        if (to != null)           p.put("ToDate", to);
        if (fromDocNo != 0)       p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0)         p.put("ToDocNo", toDocNo);
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseDemand_GetAllMethod", p);
    }

    /** frmPendingPurchaseDemand.ComboDbCall → BLL 0557 GetDataForDropDownFromPurchaseDemand(org, comp, "141", "Item"). */
    public List<Map<String, Object>> purchaseDemandDropDowns(UserAccount u, String documentTypeIds, String activity) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (activity != null && !activity.isEmpty()) p.put("Activity", activity);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromPurchaseDemand]", p);
    }

    /** BLL 0314 PurchaseDemand_PendingDataLoader — the first five always sent. */
    public List<Map<String, Object>> pendingPurchaseDemands(UserAccount u, int branchesId, int financialYearId, int documentTypeId,
                                                            Timestamp from, Timestamp to, int fromDocNo, int toDocNo, int itemId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId,
                "FinancialYearId", financialYearId,
                "DocumentTypeId", documentTypeId);
        if (from != null)   p.put("FromDate", from);
        if (to != null)     p.put("ToDate", to);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0)   p.put("ToDocNo", toDocNo);
        if (itemId != 0)    p.put("ItemId", itemId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_InvPurchaseDemand_PendingDataLoader]", p);
    }

    /** BLL 0313 GetDataForDropDownFromDeliveryChallanHeader — DocumentTypeId is set by the form but not sent; Activity is null. */
    public List<Map<String, Object>> deliveryChallanDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromDeliveryChallanHeader]",
                params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** BLL 0313 DeliveryChallanHeader_PendingDataLoader — the first five always sent. */
    public List<Map<String, Object>> pendingDeliveryChallans(UserAccount u, int branchesId, int financialYearId, int documentTypeId,
                                                             Timestamp from, Timestamp to, int fromDocNo, int toDocNo,
                                                             int itemId, int billToPartyId) {
        Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "BranchesId", branchesId,
                "FinancialYearId", financialYearId,
                "DocumentTypeId", documentTypeId);
        if (from != null)       p.put("FromDate", from);
        if (to != null)         p.put("ToDate", to);
        if (fromDocNo != 0)     p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0)       p.put("ToDocNo", toDocNo);
        if (itemId != 0)        p.put("ItemId", itemId);
        if (billToPartyId != 0) p.put("BillToPartyId", billToPartyId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_DeliveryChallanHeader_PendingDataLoader]", p);
    }
}
