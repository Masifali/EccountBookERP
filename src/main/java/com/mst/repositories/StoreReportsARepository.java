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
 * Store Management Reports (module 46), group A — the procedure calls behind three report screens:
 * <ul>
 *   <li>333 Store Purchase Register        (StorePurchaseRegister.cs)</li>
 *   <li>452 Store Purchase Demand Report   (StorePurchaseDemandRegister.cs)</li>
 *   <li>453 Store Issuance Return Report   (StoreIssuanceReturnRegister.cs)</li>
 * </ul>
 * Every method mirrors one BLL method; the parameter map is built in the BLL's own order and a
 * parameter the BLL guards ({@code if (obj.X != 0)}) is left out (null = omitted, as
 * ADO.NET AddWithValue(null) does) under the same condition.
 *
 * BLL files: 0017 BranchesAllocationToUser, 0581 InvPurchaseInvoice, 0252 InvGsStoreIssuanceHeader,
 * 0557 InvPurchaseDemandHeader, 0132 InvPurchaseInvoiceReports, 0131 InvGrnandGdnReports,
 * 0134 PurchaseOrderReports, 0067 Department, 0223 FixedAssetsRegister, 0582 InvWareHouse,
 * 0141 VoucherReports.
 */
@Repository
public class StoreReportsARepository {

    private final JdbcTemplate jdbc;

    public StoreReportsARepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // ===================================================================== shared

    /** Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId — ActiveYr.Start_Period comes from here. */
    public List<Map<String, Object>> activeYears(UserAccount u) {
        return DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId()));
    }

    // ===================================================================== 333 Store Purchase Register

    /** BranchesFill:170 → BLL 0017 GetBranchsAllocatedToUser → [dbo].[USP_GetBranchsAllocatedToUser]. */
    public List<Map<String, Object>> branchesAllocatedToUser(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUser]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "UserId", u.getId()));
    }

    /**
     * ALlDropDown:235 → BLL 0581 AllComboBindAgainstPurchaseInvoice → [dbo].[Usp_AllComboAgainstPurchaseInvoice].
     * The form sets OrganizationId, CompanyId and DocumentTypeIds "58,61,64,131" only. It computes a
     * BranchIds string (:222-234) but never assigns it to obj.BranchesIds, and Activity is null, so
     * both guarded parameters are omitted.
     */
    public List<Map<String, Object>> purchaseInvoiceCombos(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[Usp_AllComboAgainstPurchaseInvoice]", params(
                "OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", "58,61,64,131"));
    }

    /**
     * GridFill:360 → BLL 0252 InvGsStoreIssuanceHeader.StorePurchaseRegister (:961) → USP_StorePurchaseRegister.
     * Guards (BLL order): BranchesId, Id, ProjectsId, ItemCategoryId, InventoryParentCategories,
     * ItemId (!= 0); FromDate/ToDate (!CheckDateTimeNull); ItemTypeId, SupplierCustomerId (!= 0);
     * FromDocNo/ToDocNo (!= 0.0) as @GrnNoFrom/@GrnNoTo; GpSrNoF/T; OrderNoFrom/To as @PoNoFrom/To;
     * WarehouseId. The form never sets BranchesId (it fills BranchesIds, which this BLL does not
     * read), Id or ProjectsId — so those three are never sent.
     */
    public List<Map<String, Object>> storePurchaseRegister(UserAccount u, Timestamp from, Timestamp to,
                                                           int parentCategoryId, int itemCategoryId, int itemTypeId,
                                                           int itemId, int grnNoFrom, int grnNoTo, int gpNoFrom,
                                                           int gpNoTo, int poNoFrom, int poNoTo, int supplierId,
                                                           int warehouseId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (itemCategoryId != 0) p.put("ItemCategoryId", itemCategoryId);
        if (parentCategoryId != 0) p.put("InventoryParentCategoriesId", parentCategoryId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (from != null) p.put("DateFrom", from);
        if (to != null) p.put("DateTo", to);
        if (itemTypeId != 0) p.put("ItemTypeId", itemTypeId);
        if (supplierId != 0) p.put("SupplierCustomerId", supplierId);
        if (grnNoFrom != 0) p.put("GrnNoFrom", grnNoFrom);
        if (grnNoTo != 0) p.put("GrnNoTo", grnNoTo);
        if (gpNoFrom != 0) p.put("GpSrNoFrom", gpNoFrom);
        if (gpNoTo != 0) p.put("GpSrNoTo", gpNoTo);
        if (poNoFrom != 0) p.put("PoNoFrom", poNoFrom);
        if (poNoTo != 0) p.put("PoNoTo", poNoTo);
        if (warehouseId != 0) p.put("WarehouseId", warehouseId);
        return DesktopProc.rows(jdbc, "USP_StorePurchaseRegister", p);
    }

    /** CommonServices.PurchaseInvoicePMSlip_231 (:7330) → BLL 0132 PurchaseInvoice_PM231 (all three always sent). */
    public List<Map<String, Object>> purchaseInvoicePm231(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "USp_InvPurchaseInvoice_PackingMaterialBill_Rpt", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }

    /** CommonServices.PurchaseInvoiceStoreBillWithtax_238 (:7410) → BLL 0132 PurchaseInvoiceStoreBillWithTax_238. */
    public List<Map<String, Object>> purchaseInvoiceStoreBillWithTax238(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_StoreBillWithtax_Rpt", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }

    /** The 231 / 238 sub-report "InvRptPurchaseBillSupplierOthers" → BLL 0132 InvPurchaseInvoiceSlipReport220SupReprt. */
    public List<Map<String, Object>> supplierOthersSubReport(int invoiceId) {
        return DesktopProc.rows(jdbc, "SP_InvPurchaseInvoice_SupplierBillOthersAddLess_SubRpt", params("PihId", invoiceId));
    }

    /** CommonServices.GrnSlipReport212 (:14438) → BLL 0131 GrnSlipStore with DocumentTypeId 48. */
    public List<Map<String, Object>> grnStoreSlip212(UserAccount u, int grnId) {
        return DesktopProc.rows(jdbc, "Sp_InvGrn_StoreSlip_Rpt", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", 48, "Id", grnId));
    }

    /** CommonServices.PurchaseOrderSlipReport201 (:8290) → BLL 0134 (Id → @OrderId; ApprovedFilter "All" omits @IsApproved). */
    public List<Map<String, Object>> purchaseOrderGeneralSlip201(UserAccount u, int orderId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (orderId != 0) p.put("OrderId", orderId);
        return DesktopProc.rows(jdbc, "Sp_PurchaseOrder_GeneralOrderSlip_Rpt", p);
    }

    /**
     * CommonServices.PurchaseInvoiceStoreDirectSlip_233 / _237 (:7469 / :7500). The desktop calls
     * Reporting.ShowReport(rpt) with report parameters @OrganizationId, @CompanyId, @Id, @PihId —
     * the TEMPLATE runs its own procedure. The procedure is the one named after the template
     * (the same choice PurchaseInvoiceDirectStoreRepository.slip made for 233).
     */
    public List<Map<String, Object>> purchaseInvoiceStoreDirectSlip(UserAccount u, int id, boolean withTax) {
        return DesktopProc.rows(jdbc, withTax ? "SpInvPurchaseInvoice_StoreBillDirectWithTax_Rpt"
                                              : "SpInvPurchaseInvoice_StoreBillDirect_Rpt", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }

    // ===================================================================== 452 Store Purchase Demand Report

    /** BranchesFill:246 → BLL 0557 GetBranchesAllocatedToUserFromStorePurchaseDemadRegister(…, 141). */
    public List<Map<String, Object>> demandBranches(UserAccount u, int documentTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "UserId", u.getId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromStorePurchaseDemandRegister]", p);
    }

    /**
     * AllDropDownBind:178 → BLL 0557 GetDataForDropDownFromPurchaseDemand(org, comp, "141", null, null, 0).
     * Activity null, BranchesIds null and ParentCategoryId 0 are all guarded, so only
     * @DocumentTypeIds is added. (The BLL would send @BranchesIds, which the procedure does not
     * declare — the form never passes one.)
     */
    public List<Map<String, Object>> demandDropDowns(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromPurchaseDemand]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeIds", "141"));
    }

    /**
     * gridHisory:483 → BLL 0557 InvStorePurchaseDemandRegister (:772) → USP_StorePurchaseDemandRegister.
     * @BranchesId and @FinancialYearId are added UNCONDITIONALLY with the object's values — the
     * form sets neither, so both go as 0 (desktop behaviour, see the service notes).
     * Guards: FromDate/ToDate; FromDocNo/ToDocNo → @DocNoFrom/@DocNoTo; ParentCategoryId (never
     * set); InventoryParentCategories; ItemCategoryId; ItemTypeId; JobLotId (never set by the
     * form); ItemId; DepartmentId; AssetId → @FixedAssetsRegisterId; Status (non-empty);
     * IsApproved whenever ApprovedFilter != "All".
     *
     * @param isApproved null = omitted (ApprovedFilter "All"); otherwise sent as a bit.
     */
    public List<Map<String, Object>> storePurchaseDemandRegister(UserAccount u, Timestamp from, Timestamp to,
                                                                 int docNoFrom, int docNoTo, int parentCategoryId,
                                                                 int itemCategoryId, int itemTypeId, int itemId,
                                                                 int departmentId, int assetId, String status,
                                                                 Boolean isApproved) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchesId", 0, "FinancialYearId", 0);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (docNoFrom != 0) p.put("DocNoFrom", docNoFrom);
        if (docNoTo != 0) p.put("DocNoTo", docNoTo);
        if (parentCategoryId != 0) p.put("InventoryParentCategoriesId", parentCategoryId);
        if (itemCategoryId != 0) p.put("ItemCategoryId", itemCategoryId);
        if (itemTypeId != 0) p.put("ItemTypeId", itemTypeId);
        if (itemId != 0) p.put("ItemId", itemId);
        if (departmentId != 0) p.put("DepartmentId", departmentId);
        if (assetId != 0) p.put("FixedAssetsRegisterId", assetId);
        if (status != null && !status.isEmpty()) p.put("Status", status);
        if (isApproved != null) p.put("IsApproved", isApproved);
        return DesktopProc.rows(jdbc, "USP_StorePurchaseDemandRegister", p);
    }

    /** CommonServices.PurchaseDemandSlip454 (:8141) → BLL 0557 InvPurchaseDemondSlip → Sp_InvPurchaseDemand_Rpt. */
    public List<Map<String, Object>> purchaseDemandSlip454(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseDemand_Rpt", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }

    /**
     * CompleteStatus:764 / CancelStatus:817 → BLL 0557 UpdateStatusandIsApprovedbyId →
     * [dbo].[USP_PurchaseDemandDocAndApprovedStatusUpdatebyId] through GetDataTableProc (no
     * transaction of its own; one row in the list, so one call). @StatusRemarks is guarded
     * (non-empty) — the form never reaches here with it empty.
     */
    public void purchaseDemandStatusUpdate(UserAccount u, int id, String reqType, String statusRemarks) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ReqType", reqType, "Id", id, "ApprovedUserId", u.getId());
        if (statusRemarks != null && !statusRemarks.isEmpty()) p.put("StatusRemarks", statusRemarks);
        DesktopProc.rows(jdbc, "[dbo].[USP_PurchaseDemandDocAndApprovedStatusUpdatebyId]", p);
    }

    /**
     * Tenancy guard for Complete / Cancel (web only): the demand header must be this organization's
     * and company's Purchase Demand (DocumentTypeId 141). Plain parameterized read — the desktop
     * has no such check because its grid can only hold rows the register returned for the company.
     */
    public List<Map<String, Object>> ownedDemandHeader(UserAccount u, int id) {
        return jdbc.queryForList(
                "SELECT Id, DocNo, Status FROM dbo.InvPurchaseDemandHeader "
              + "WHERE Id = ? AND OrganizationId = ? AND CompanyId = ? AND DocumentTypeId = ?",
                id, u.getOrganizationId(), u.getCompanyId(), 141);
    }

    /**
     * The row's ReceivedQty re-derived server-side exactly as USP_StorePurchaseDemandRegister
     * builds it (#TmpRecivings: SUM(InvGrnDetail.ItemQty) joined on InvPurchasedemondId /
     * PurchaseDemondDetailId), so a posted grid value cannot lower it.
     */
    public double demandDetailReceivedQty(int headerId, int detailId) {
        Double v = jdbc.queryForObject(
                "SELECT ISNULL(SUM(d.ItemQty), 0) FROM dbo.InvGrnDetail d "
              + "WHERE d.InvPurchasedemondId = ? AND d.PurchaseDemondDetailId = ?",
                Double.class, headerId, detailId);
        return v == null ? 0d : v;
    }

    // ===================================================================== attachments (all three)

    /**
     * CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId) → BLL 0069
     * DMSAttachments.ReadAttachmentsbyRefDocumentTypeId → Sp_DMSAttachments_GetAllMethod
     * 'ReadAttachmentsbyRefDocumentTypeId' (@RefDocumentTypeId, @Id, @Activity — BLL order).
     * The procedure does not filter by company; the service drops foreign rows.
     */
    public List<Map<String, Object>> attachmentsByRefDocument(int id, int refDocumentTypeId) {
        return DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", params(
                "RefDocumentTypeId", refDocumentTypeId, "Id", id,
                "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
    }

    // ===================================================================== 453 Store Issuance Return Report

    /** DepartmentNameFill:119 → BLL 0067 Department.GetAll → Sp_Department_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> departments(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_Department_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** ItemNameFill:143 → BLL 0252 GetItemsForReturntoStore → Sp_InvGsStoreIssuanceHeader_GetAllMethod. */
    public List<Map<String, Object>> itemsForReturnToStore(UserAccount u) {
        return DesktopProc.rows(jdbc, StoreIssuanceRepository.P_HEADER_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetItemsForReturntoStore"));
    }

    /** FixedAssest:168 → BLL 0223 FixedAssetsRegister.GetAll → Sp_FixedAssetsRegister_GetAllMethod 'ReadAll'. */
    public List<Map<String, Object>> fixedAssets(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_FixedAssetsRegister_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "ReadAll"));
    }

    /** CmbWareHouseFill:188 → CommonServices.WareHouseGetAllService → BLL 0582 InvWareHouse.Getall. */
    public List<Map<String, Object>> warehouses(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvWareHouse_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "ReadByOrganizationCompanyId"));
    }

    /** CreditAcDetailFill:209 → BLL 0252 GetDebitAccountForStoreReturn (ItemId guarded != 0). */
    public List<Map<String, Object>> debitAccountsForStoreReturn(UserAccount u, int itemId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (itemId != 0) p.put("ItemId", itemId);
        p.put("Activity", "GetDebitAccountForStoreReturn");
        return DesktopProc.rows(jdbc, StoreIssuanceRepository.P_HEADER_GETALL, p);
    }

    /**
     * GridFill:309 and the row Print (:461) → BLL 0252 StoreIssuanceReturnRegister (:1120) →
     * Sp_InvStoreRetrun_SlipandRegister. Guards in BLL order: Id; FromDate; ToDate; FromDocNo;
     * ToDocNo; ItemId; AssetId; WarehouseId → @WareHouseId; RefDocumentTypeId → @DepartmentId
     * (the form puts the Department combo's value in RefDocumentTypeId); AccountId;
     * ItemConditionId (never set by the form).
     */
    public List<Map<String, Object>> storeIssuanceReturnRegister(UserAccount u, int id, Timestamp from, Timestamp to,
                                                                 int fromDocNo, int toDocNo, int itemId, int assetId,
                                                                 int warehouseId, int departmentId, int accountId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (id != 0) p.put("Id", id);
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (fromDocNo != 0) p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0) p.put("ToDocNo", toDocNo);
        if (itemId != 0) p.put("ItemId", itemId);
        if (assetId != 0) p.put("AssetId", assetId);
        if (warehouseId != 0) p.put("WareHouseId", warehouseId);
        if (departmentId != 0) p.put("DepartmentId", departmentId);
        if (accountId != 0) p.put("AccountId", accountId);
        return DesktopProc.rows(jdbc, "Sp_InvStoreRetrun_SlipandRegister", p);
    }

    /**
     * CommonServices.VoucherReport_118 (:5647) → BLL 0141 VoucherValidationReport with UserId, Id,
     * DocumentTypeId and ApprovedFilter "All" (so @IsApproved is omitted); every other guard is 0.
     */
    public List<Map<String, Object>> voucher118(UserAccount u, int voucherHeadId, int documentTypeId) {
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (u.getId() != null && u.getId() != 0) p.put("UserId", u.getId());
        if (voucherHeadId != 0) p.put("Id", voucherHeadId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "Sp_Accounts_VouchersValidation_Rpt", p);
    }
}
