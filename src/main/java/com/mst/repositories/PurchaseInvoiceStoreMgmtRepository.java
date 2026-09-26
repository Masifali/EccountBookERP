package com.mst.repositories;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Repository;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;

/**
 * Screen 323 "Purchase Invoice Store Management" (PurchaseInvoiceStoreManagement.cs, DocumentTypeId
 * 64) and its Load-GRN dialog (frmPendingGrnStoreLoader.cs, GRN DocumentTypeId 48).
 *
 * Every procedure, @Activity and parameter below was read from the recovered desktop source and
 * checked against /root/ddl/procs.json:
 *
 *   BLL 0581 / DAL 0434  Architecture.*.Inventory.InvPurchaseInvoice   (codes, history, read, save, delete)
 *   BLL 0576             Architecture.BLL.Inventory.InvGrn             (loader branches / combos / pending)
 *   BLL 0313             Architecture.BLL.StoreManagement.DeliveryChallanHeader (expense rows)
 *   BLL 0573             Architecture.BLL.Inventory.InventoryItemsOther (other-item list)
 *   BLL 0267             CommonServies (financial-effect lists)
 *   BLL 0379             GlobalServicesMethods (suppliers, accounts, payment terms)
 *   BLL 0592             ItemTaxSchedule.GetTaxScheduleDetailbyItemIds (DocDate_Leave)
 *   BLL 0132 / 0141      the three print procedures
 *
 * A parameter the BLL only adds under a condition is only added here under the same condition; a
 * null is never bound (DesktopProc omits it, as ADO.NET omits a CLR null).
 */
@Repository
public class PurchaseInvoiceStoreMgmtRepository {

    public static final String P_GETALL = "[Sp_InvPurchaseInvoice_GetAllMethod]";

    private final JdbcTemplate jdbc;
    public PurchaseInvoiceStoreMgmtRepository(JdbcTemplate jdbc) { this.jdbc = jdbc; }

    // =========================================================================== numbering

    /** BLL 0581 GenerateInvPurchaseInvoiceCode (via CommonServices.PurchaseInvoiceGenerateCode). */
    public int nextDocNo(UserAccount u, int financialYearId, int documentTypeId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId,
                "Activity", "GenerateCode"));
        return r.isEmpty() ? 0 : StoreIssuanceRepository.toInt(r.get(0).get("DocNo"));
    }

    /** BLL 0581 GenerateInvPurchaseInvoiceCodeBranch. */
    public int nextBranchSrNo(UserAccount u, int financialYearId, int documentTypeId, int branchesId) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "FinancialYearId", financialYearId,
                "BranchesId", branchesId, "Activity", "GenerateBranchSrNo"));
        return r.isEmpty() ? 0 : StoreIssuanceRepository.toInt(r.get(0).get("BranchSrNo"));
    }

    /** BLL 0581 GenerateSalesTaxNo — the form always asks with IsTaxable = true. */
    public int nextSalesTaxNo(UserAccount u) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, P_GETALL, params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "IsTaxable", true, "Activity", "GenerateSalesTaxNo"));
        return r.isEmpty() ? 0 : StoreIssuanceRepository.toInt(r.get(0).get("SalesTaxNo"));
    }

    // ============================================================================= globals

    /** clsGlobalVariables.globalAllSupplierCustomer — BLL 0379 getGlobalSupplierCustomer(org, comp, 0, 0, 0, ""). */
    public List<Map<String, Object>> vendorsAndCustomers(UserAccount u) {
        return DesktopProc.rows(jdbc, "USP_GetVendorsAndCustomersWithCityName", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.AllAccountsWithCustomGroupId — BLL 0379 GetGlobalAllAccountsWithCustomGroup(org, comp, 0, 0, ""). */
    public List<Map<String, Object>> accountsWithCustomGroup(UserAccount u) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_GETAllAccountsFromCustomGroups]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** clsGlobalVariables.globalPaymentTerm — BLL 0379 getPaymentTermlist. */
    public List<Map<String, Object>> paymentTerms(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InvDueTerms_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Activity", "GetAll"));
    }

    /**
     * BLL 0573 InventoryItemsOther.GetAll (OtherItemdtDbCall) and, with the same three parameters,
     * BLL 0267 GetOtherItemForFinancialEffects. Parameter spelling "@organizationId" is the BLL's.
     */
    public List<Map<String, Object>> otherItems(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_InventoryItemsOther_GetAllMethod", params(
                "Activity", "ReadAll", "organizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
    }

    /** CommonServices.GetERPFeatureById — clsGlobalVariables.ErpFeaturesList (USP_GetERPFeaturesByCompanyId). */
    public boolean erpFeature(UserAccount u, int featureId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_GetERPFeaturesByCompanyId", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()))) {
            if (StoreIssuanceRepository.toInt(r.get("Id")) == featureId) return true;
        }
        return false;
    }

    /**
     * UserAccount.BranchName is a login-time value this port does not carry; it is resolved from
     * [dbo].[USP_GetBranchsAllocatedToUser] (BranchId, BranchName), as other ports already do.
     */
    public String branchName(UserAccount u, int branchId) {
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUser]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId()))) {
            if (StoreIssuanceRepository.toInt(r.get("BranchId")) == branchId) return StoreIssuanceRepository.str(r.get("BranchName"));
        }
        return "";
    }

    // ============================================================================= history

    /** BLL 0581 GetBranchsAllocatedToUserFromPurchaseInvoice — @DocumentTypeId only when != 0. */
    public List<Map<String, Object>> branchesFromPurchaseInvoice(UserAccount u, int documentTypeId) {
        java.util.Map<String, Object> p = params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "UserId", u.getId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromPurchaseInvoice]", p);
    }

    /** BLL 0581 AllComboBindAgainstPurchaseInvoice — only OrganizationId, CompanyId and DocumentTypeIds are set. */
    public List<Map<String, Object>> historyCombos(UserAccount u, String documentTypeIds) {
        java.util.Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        return DesktopProc.rows(jdbc, "[dbo].[Usp_AllComboAgainstPurchaseInvoice]", p);
    }

    /** BLL 0581 FormHistory — every parameter under the BLL's own condition, in the BLL's order. */
    public List<Map<String, Object>> formHistory(UserAccount u, int documentTypeId, boolean canViewAll,
                                                 int financialYearId, int entryUser,
                                                 Timestamp from, Timestamp to,
                                                 Timestamp entryFrom, Timestamp entryTo,
                                                 Timestamp modifyFrom, Timestamp modifyTo,
                                                 Timestamp approvedFrom, Timestamp approvedTo,
                                                 int fromDocNo, int toDocNo, int supplierCustomerId,
                                                 String branchesIds) {
        java.util.Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "CanViewAllRecord", canViewAll);
        if (financialYearId != 0) p.put("FinancialYearId", financialYearId);
        if (!canViewAll)          p.put("EntryUser", entryUser);
        if (from != null)         p.put("FromDate", from);
        if (to != null)           p.put("ToDate", to);
        if (entryFrom != null)    p.put("EntryFromDate", entryFrom);
        if (entryTo != null)      p.put("EntryToDate", entryTo);
        if (modifyFrom != null)   p.put("ModifyFromDate", modifyFrom);
        if (modifyTo != null)     p.put("ModifyToDate", modifyTo);
        if (approvedFrom != null) p.put("ApprovedFromDate", approvedFrom);
        if (approvedTo != null)   p.put("ApprovedToDate", approvedTo);
        if (fromDocNo != 0)       p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0)         p.put("ToDocNo", toDocNo);
        if (supplierCustomerId != 0) p.put("SupplierCustomerId", supplierCustomerId);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(jdbc, P_GETALL, p);
    }

    // ================================================================================ read

    /** BLL 0581 GetByID → DAL 0434 GetDate: the header row (index 0) or null. */
    public Map<String, Object> header(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_GetAllMethod",
                params("Id", id, "Activity", "ReadById"));
        return r.isEmpty() ? null : r.get(0);
    }

    /** DAL 0434 GetDate: DocumentTypeId 61 / 64 / 245 read their details from this procedure. */
    public List<Map<String, Object>> details(int id) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_InvPurchaseInvoiceDetail_ReadById]", params("Id", id));
    }

    public List<Map<String, Object>> expenses(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_GetAllMethod",
                params("Id", id, "Activity", "InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID"));
    }

    public List<Map<String, Object>> freights(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_GetAllMethod",
                params("Id", id, "Activity", "InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID"));
    }

    public List<Map<String, Object>> journals(int id) {
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_GetAllMethod",
                params("Id", id, "Activity", "InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID"));
    }

    /** BLL 0581 GetRecordsById — Rows[0][0]; throws the DataTable message when there is no row. */
    public Object recordDate(int id) {
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_GetAllMethod",
                params("Id", id, "Activity", "GetRecordId"));
        if (r.isEmpty()) throw new IllegalArgumentException("There is no row at position 0.");
        return r.get(0).values().iterator().next();
    }

    // ============================================================================= loader

    /** BLL 0576 GetBranchesAllocatedToUserFromGrn (frmPendingGrnStoreLoader.BranchesFill, DocumentTypeId 48). */
    public List<Map<String, Object>> branchesFromGrn(UserAccount u, int documentTypeId) {
        java.util.Map<String, Object> p = params("OrganizationId", u.getOrganizationId(),
                "CompanyId", u.getCompanyId(), "UserId", u.getId());
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetBranchsAllocatedToUserFromGrn]", p);
    }

    /** BLL 0576 GetDataForDropDownFromGrn(org, comp, "48", null, BranchIds, 0). */
    public List<Map<String, Object>> grnDropDowns(UserAccount u, String documentTypeIds, String branchesIds) {
        java.util.Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (documentTypeIds != null && !documentTypeIds.isEmpty()) p.put("DocumentTypeIds", documentTypeIds);
        if (branchesIds != null && !branchesIds.isEmpty()) p.put("BranchesIds", branchesIds);
        return DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromGrn]", p);
    }

    /**
     * BLL 0576 InvGrnStore_PendingDataLoader. The BLL reads {@code obj.BranchesIds}; the loader
     * fills {@code Obj.BranchIds} (frmPendingGrnStoreLoader.cs:283), a different property of
     * ReportsParameters (model 0083 lines 214 and 946). So @BranchesIds is ALWAYS omitted and
     * the procedure's {@code @BranchesIds IS NULL} branch returns every branch. Reproduced:
     * there is deliberately no branch parameter on this method.
     */
    public List<Map<String, Object>> pendingGrnStore(UserAccount u, int financialYearId, int documentTypeId,
                                                     Timestamp from, Timestamp to, int fromDocNo, int toDocNo,
                                                     int itemId, int billToPartyId) {
        java.util.Map<String, Object> p = params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", financialYearId, "DocumentTypeId", documentTypeId);
        if (from != null)       p.put("FromDate", from);
        if (to != null)         p.put("ToDate", to);
        if (fromDocNo != 0)     p.put("FromDocNo", fromDocNo);
        if (toDocNo != 0)       p.put("ToDocNo", toDocNo);
        if (itemId != 0)        p.put("ItemId", itemId);
        if (billToPartyId != 0) p.put("BillToPartyId", billToPartyId);
        return DesktopProc.rows(jdbc, "[dbo].[USP_InvGrnStore_PendingDataLoader]", p);
    }

    /** BLL 0581 GetTransporterAndFreightFromGrn — the GRN ids go in @GdnIds. */
    public List<Map<String, Object>> transporterAndFreightFromGrn(String ids) {
        return DesktopProc.rows(jdbc, P_GETALL, params("GdnIds", ids, "Activity", "GetTransporterAndFreightFromGrn"));
    }

    /** BLL 0313 DeliveryChallanHeader_ExpenseDetailByIds. */
    public List<Map<String, Object>> deliveryChallanExpenses(String ids) {
        return DesktopProc.rows(jdbc, "[dbo].[USP_DeliveryChallan_ExpenseSubReport]", params("DeliveryChallanHeaderIds", ids));
    }

    /** BLL 0592 GetTaxScheduleDetailbyItemIds (CommonServices.GetTaxScheduleDetailbyItemIds). */
    public List<Map<String, Object>> taxScheduleByItemIds(UserAccount u, String itemIds, Timestamp effectedDate) {
        return DesktopProc.rows(jdbc, "Sp_ItemTaxSchedule_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemIds", itemIds, "EffectedDate", effectedDate, "Activity", "GetTaxScheduleDetailbyItemIds"));
    }

    /**
     * Web-only guard (W2): the GRN ids a page posts must be GRNs the Load-GRN dialog could have
     * offered — the same filters USP_InvGrnStore_PendingDataLoader applies to its header rows:
     * this organization / company, {@code FinancialYearId = @FinancialYearId}, DocumentTypeId 48,
     * {@code BaseDocumentTypeId != 4}, and NOT on any purchase invoice of this organization /
     * company (#ExcludedGRN) — except the invoice being updated ({@code exceptInvoiceId}, 0 on a
     * new save). Returns the ids that pass.
     */
    public List<Integer> loadableGrnIds(UserAccount u, int financialYearId, int documentTypeId,
                                        List<Integer> ids, int exceptInvoiceId) {
        List<Integer> out = new ArrayList<>();
        if (ids.isEmpty()) return out;
        StringBuilder sb = new StringBuilder(
                "SELECT g.Id FROM dbo.InvGrn g WHERE g.OrganizationId = ? AND g.CompanyId = ? AND g.FinancialYearId = ? "
              + "AND g.DocumentTypeId = ? AND g.BaseDocumentTypeId != 4 "
              + "AND NOT EXISTS (SELECT 1 FROM dbo.InvPurchaseInvoice hh JOIN dbo.InvPurchaseInvoiceDetail dd ON hh.Id = dd.InvPurchaseInvoiceId "
              + "WHERE hh.OrganizationId = ? AND hh.CompanyId = ? AND dd.InvGrnId = g.Id AND hh.Id <> ?) AND g.Id IN (");
        List<Object> args = new ArrayList<>();
        args.add(u.getOrganizationId());
        args.add(u.getCompanyId());
        args.add(financialYearId);
        args.add(documentTypeId);
        args.add(u.getOrganizationId());
        args.add(u.getCompanyId());
        args.add(exceptInvoiceId);
        for (int i = 0; i < ids.size(); i++) { sb.append(i == 0 ? "?" : ",?"); args.add(ids.get(i)); }
        sb.append(")");
        for (Map<String, Object> r : jdbc.queryForList(sb.toString(), args.toArray())) out.add(StoreIssuanceRepository.toInt(r.get("Id")));
        return out;
    }

    /** The GRN detail rows of the given GRNs: InvGrnId, Id, PurchaseOrderId (ISNULL 0), DeliveryChallanId (ISNULL 0). */
    public List<Map<String, Object>> grnDetailKeys(List<Integer> grnIds) {
        if (grnIds.isEmpty()) return new ArrayList<>();
        StringBuilder sb = new StringBuilder("SELECT d.InvGrnId, d.Id, ISNULL(d.PurchaseOrderId, 0) AS PurchaseOrderId, "
                + "ISNULL(d.DeliveryChallanId, 0) AS DeliveryChallanId FROM dbo.InvGrnDetail d WHERE d.InvGrnId IN (");
        List<Object> args = new ArrayList<>();
        for (int i = 0; i < grnIds.size(); i++) { sb.append(i == 0 ? "?" : ",?"); args.add(grnIds.get(i)); }
        sb.append(")");
        return jdbc.queryForList(sb.toString(), args.toArray());
    }

    // ============================================================================ financial

    /** BLL 0267 GetSupplierCustomerListForFinancialEffects. */
    public List<Map<String, Object>> supplierGlList(UserAccount u) {
        return DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId"));
    }

    // ================================================================================ write

    public int setProc(String proc, Map<String, Object> model) { return DesktopProc.setProc(jdbc, proc, model); }

    /** DAL 0434 :140-149. */
    public void removeDetailsNotIn(UserAccount u, int id, String detailIds) {
        DesktopProc.scalar(jdbc, "usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "Id", id, "DetailIds", detailIds));
    }

    /** DAL 0434 :150-159 (DocumentTypeId != 59). */
    public void stockInTransitDelete(int id) {
        DesktopProc.scalar(jdbc, "usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId", params("Id", id));
    }

    /** DAL 0434 :467-475 — a fresh InventoryStockEvalautionDetail with four properties set. */
    public void evaluationUpdate(UserAccount u, int documentTypeId, int id) {
        Map<String, Object> e = InventoryOpeningDefaults.evaluation();
        e.put("OrganizationId", u.getOrganizationId());
        e.put("CompanyId", u.getCompanyId());
        e.put("RefDocumentTypeId", documentTypeId);
        e.put("RefDocIdNo", id);
        setProc("Sp_InventoryStockEvalautionDetail_Update", e);
    }

    /** DAL 0434 :508-516. */
    public void voucherBalanceCheck(UserAccount u, int voucherHeadId) {
        DesktopProc.scalar(jdbc, "USP_VoucherBalanceCheck", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", voucherHeadId));
    }

    /** DAL 0434 :556-562 — DocumentApprovalDetail, LimitAmount = Conversion.ToDecimal(BillAmount). */
    public void approvalInsert(UserAccount u, int documentTypeId, int id, BigDecimal limitAmount) {
        setProc("[DAW].[USp_DocumentApprovalDetail_Insert]", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "DocumentTypeId", documentTypeId, "Id", id, "LimitAmount", limitAmount));
    }

    // ================================================================================ print

    /** BLL 0132 InvPurchaseInvoice_StoreBillSlip — 230-InvRptPurchaseBillStoreSlip.rpt. */
    public List<Map<String, Object>> storeBillSlip(UserAccount u, int id) {
        java.util.Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (id != 0) p.put("Id", id);
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_StoreBill_Rpt", p);
    }

    /** BLL 0132 PurchaseInvoiceStoreBillWithTax_238 — 238-InvPurchaseInvoice_StoreBillWithTax.rpt. */
    public List<Map<String, Object>> storeBillWithTax(UserAccount u, int id) {
        return DesktopProc.rows(jdbc, "Sp_InvPurchaseInvoice_StoreBillWithtax_Rpt", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "Id", id));
    }

    /** BLL 0132 InvPurchaseInvoiceSlipReport220SupReprt — the slips' sub-report. */
    public List<Map<String, Object>> supplierAddLessSubReport(int id) {
        return DesktopProc.rows(jdbc, "SP_InvPurchaseInvoice_SupplierBillOthersAddLess_SubRpt", params("PihId", id));
    }

    /** BLL 0141 VoucherSlipForInventoryReport with ApprovedFilter "All" (so @IsApproved is omitted). */
    public List<Map<String, Object>> voucherSlip(UserAccount u, int voucherHeadId, int documentTypeId) {
        java.util.Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (voucherHeadId != 0)  p.put("Id", voucherHeadId);
        if (documentTypeId != 0) p.put("DocumentTypeId", documentTypeId);
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt", p);
    }
}
