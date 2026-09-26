package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.PurchaseInvoiceStoreMgmtDto;
import com.mst.repositories.PurchaseInvoiceStoreMgmtRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Purchase (ModuleId 67) — screen 323 "Purchase Invoice Store Management".
 *
 * <pre>
 * Screen id        323
 * ScreenName       PurchaseInvoiceStoreManagement   (base.Name; rights key)
 * DocumentTypeId   64   (PurchaseInvoiceStoreManagement.cs:448 — "PurchaseInvoiceStoreManagement" ? 64 : 58)
 * Loader           frmPendingGrnStoreLoader.cs — GRN DocumentTypeId 48 (frmPendingGrnStoreLoader.cs:116)
 * Route            /store/purchase-invoice-store-management
 * API              /api/store/purchase-invoice-store-management
 * Desktop          PurchaseInvoiceStoreManagement.cs (5,195 lines) + frmPendingGrnStoreLoader.cs
 *                  + Architecture.WinApp.Inventory.FromWise_Helper_Methods.frmPurchaseInvoiceStoreManagement_Helper
 * BLL / DAL        BLL 0581 + DAL 0434 InvPurchaseInvoice (Save / GetByID / FormHistory / codes / RemoveByID)
 *                  BLL 0549 PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice
 *                  BLL 0614 PurchaseInvoiceGeneralFinancialMethods (Trading, Freight, SupplierAddLess, SalesTax …)
 *                  BLL 0576 InvGrn (loader), BLL 0313 DeliveryChallanHeader, BLL 0573 InventoryItemsOther,
 *                  BLL 0267 CommonServies, BLL 0379 GlobalServicesMethods, BLL 0592 ItemTaxSchedule,
 *                  BLL 0132 InvPurchaseInvoiceReports, BLL 0141 VoucherReports
 * Models           1036 InvPurchaseInvoice, 1037 …Detail, 1034 …Expense, 1035 …Freight, 1038 …Journal,
 *                  1195 VoucherHead / 1191 VoucherDetail (mirrored by ContraVoucherDto.Head / Detail)
 * </pre>
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED, NOT CORRECTED (each is the user's call to change)
 * ---------------------------------------------------------------------------------------------
 *  D1  Load-GRN dialog never filters by branch. The loader sets {@code Obj.BranchIds}
 *      (frmPendingGrnStoreLoader.cs:283) but BLL 0576 InvGrnStore_PendingDataLoader reads
 *      {@code obj.BranchesIds} (a different ReportsParameters property, model 0083:214 vs :946), so
 *      @BranchesIds is never sent and USP_InvGrnStore_PendingDataLoader's
 *      {@code @BranchesIds IS NULL} branch returns pending GRNs of EVERY branch. The "Select branch
 *      first" check (:278) still runs, on the ids that are then thrown away.
 *  D2  A second Load GRN while the grid already has rows always fails: LoadInGridDetailFromGrn
 *      (:1979) reads {@code firstGridRow.Cells["DemondId"]}, a column dtGrid does not have
 *      (helper InitializeDetailtable). The exception is caught and shown; GrnBaseDocumentTypeId has
 *      already been set (:1974). The page shows the same refusal.
 *  D3  First load of the History branch list ignores the PurchaseInvoiceBranchWise config:
 *      HistoryBranchComboDbCall runs inside Task.Run (:450) BEFORE GetConfigurationsFromGlobal
 *      (:479) sets BranchImplemented, so the proc list is always used on open; History → Refresh
 *      (:2253) then honours the config. The grid's BranchSrNo/BranchName visibility uses the
 *      config value read at load.
 *  D4  Voucher RefDocNoId is the invoice id AT THE TIME MakeVoucher runs (BLL 0549:24), i.e. 0 on
 *      a new invoice (DocumentTypeSrNo is fixed by the DAL afterwards, RefDocNoId is not).
 *  D5  Freight grid credit is saved through Conversion.ToInt (:1556) — the amount is rounded to a
 *      whole number (banker's rounding) before it is stored and posted. Expense Qty likewise (:1592).
 *  D6  FreightFinancial's against-account: the detail list's PurchaseGLAC is never filled by this
 *      form, so {@code Distinct().Count() == 1} is always true with value 0, and every transporter
 *      line is posted against ITSELF (BLL 0614:304-316).
 *  D7  Update does not check "approved" (only Delete does, :1692).
 *  D8  The 103-Voucher button prints the voucher of the last document OPENED — Reset (:1846) does
 *      not clear VoucherHeadId. Kept on the page.
 *  D9  Detail source links PurchaseDemandId / PreBillId / DeliveryChallanId, their numbers / document
 *      types and GrnNo are VIRTUAL on model 1037 — SetProc never sends them; only InvGrnId,
 *      InvGrnDetailId and PurchaseOrderId are stored. On reopen USP_InvPurchaseInvoiceDetail_ReadById
 *      rebuilds them by joining InvGrnDetail on (InvGrnId, InvGrnDetailId) (then the order / demand /
 *      challan / pre-bill tables), so they come back filled while that GRN link is intact.
 *  D10 Commission Agent / Type / Rate / UOM / Amount / Remarks and Term are hidden and never bound
 *      (designer Visible = false) — saved as 0 / "" as the desktop saves them; the
 *      "Please Select Commission Agent Account First" check can never fire.
 *  D11 SupplierInvoiceNo is saved from txtDueDays (:1507) and SupplierInvoiceDate from the Due
 *      Date picker (:1503); ReadById restores Due Days from SupplierInvoiceNo (:1745).
 *  D12 Rate is editable only for GRNs against a Purchase Demand (GrnBaseDocumentTypeId == 2, :676).
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  W1  Doc No, Branch Sr No and Invoice Tax No are ReadOnly on the desktop; the server takes them
 *      from the generators on a new save and from the stored header on update, never from the
 *      request. (The desktop saves the numbers shown at the last reset.)
 *  W2  Open / update / delete / print only when the invoice belongs to the user's organization and
 *      company AND is DocumentTypeId 64. Posted GRN lines must be ones the Load-GRN dialog could
 *      offer: GRN of this org/company, current financial year, DocumentTypeId 48, BaseDocumentTypeId
 *      != 4, on no other purchase invoice (the filters of USP_InvGrnStore_PendingDataLoader), the
 *      (InvGrnId, InvGrnDetailId) pair must exist with the row's PurchaseOrderId, and a freight row's
 *      InvGrnId must be one of the rows' GRNs (the /loader/extras call applies the same GRN filter); posted
 *      supplier, payment term, tax / discount / freight / JL accounts and other items must be ones
 *      the screen's own lists offer; on update a posted detail Id must be one of this invoice's.
 *  W3  The "Are you sure to Save/Update" prompt is a two-step request: the checks the desktop runs
 *      before its prompt are answered first, the rest after the Yes.
 *  W4  A header procedure that returns no id stops the save (the DAL would continue with Id 0).
 *
 * Not ported: attachments (Attachment button, attachment count links, Add Attachment history
 * button — the stored AttachmentsValues are kept as they are), saved grid layouts (CtrlGrdBar),
 * shortcut-key popup, the GRN slip 212 link in the loader, Crystal layouts (the print endpoints
 * return the report procedure's rows).
 */
@Service
public class PurchaseInvoiceStoreMgmtService {

    private static final Logger LOG = LoggerFactory.getLogger(PurchaseInvoiceStoreMgmtService.class);

    public static final String SCREEN_NAME = "PurchaseInvoiceStoreManagement";
    public static final int DOCUMENT_TYPE_ID = 64;
    public static final int GRN_DOCUMENT_TYPE_ID = 48;

    private static final int[] TAX_ACCOUNT_TYPES = { 3, 6, 8, 16, 17, 18, 19 };          // GetTaxAccountsFromGlobal
    private static final int[] DISCOUNT_ACCOUNT_TYPES = { 11, 13, 20, 21, 22 };           // :564
    private static final int[] GL_EXCLUDED_TYPES = { 2, 11, 12, 13, 14, 15, 20, 21, 22 }; // :781 / :889

    private final PurchaseInvoiceStoreMgmtRepository repo;
    private final StoreIssuanceRepository common;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final JdbcTemplate jdbc;

    public PurchaseInvoiceStoreMgmtService(PurchaseInvoiceStoreMgmtRepository repo, StoreIssuanceRepository common,
                                           StoreScreenRights rights, CurrentUserContext ctx, JdbcTemplate jdbc) {
        this.repo = repo;
        this.common = common;
        this.rights = rights;
        this.ctx = ctx;
        this.jdbc = jdbc;
    }

    // ======================================================================== form load

    /** InitializeComponentMethod (:444) — everything the form binds on open. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        int branchId = branch(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("documentTypeId", DOCUMENT_TYPE_ID);
        /* D3 — the proc list, whatever PurchaseInvoiceBranchWise says (BranchImplemented is still false). */
        out.put("historyBranches", branchRows(repo.branchesFromPurchaseInvoice(u, DOCUMENT_TYPE_ID)));
        out.put("historySuppliers", historySuppliers(u));
        out.put("docNo", repo.nextDocNo(u, fy, DOCUMENT_TYPE_ID));
        out.put("branchSrNo", repo.nextBranchSrNo(u, fy, DOCUMENT_TYPE_ID, branchId));
        out.put("taxNo", repo.nextSalesTaxNo(u));
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(common.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("branchImplemented", toBool(common.config(u, "PurchaseInvoiceBranchWise")));
        out.put("suppliers", suppliers(u));
        List<Map<String, Object>> accounts = repo.accountsWithCustomGroup(u);
        out.put("taxAccounts", accountsWith(accounts, TAX_ACCOUNT_TYPES));
        out.put("discountAccounts", accountsWith(accounts, DISCOUNT_ACCOUNT_TYPES));
        out.put("glAccounts", accountsWithout(accounts, GL_EXCLUDED_TYPES));
        out.put("paymentTerms", paymentTerms(u));
        out.put("otherItems", otherItems(u));
        out.put("erpFeature4", repo.erpFeature(u, 4));                       // BillAmount():1235
        out.put("userBranchId", branchId);
        out.put("userBranchName", repo.branchName(u, branchId));
        return out;
    }

    /** btnRefreshHistory_Click (:2249) — the branch list now honours PurchaseInvoiceBranchWise. */
    public Map<String, Object> historyRefresh() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("historyBranches", historyBranchesAfterLoad(u));
        out.put("historySuppliers", historySuppliers(u));
        return out;
    }

    /** btnFrmRefresh_Click (:1902) — the globals rebound. */
    public Map<String, Object> formRefresh() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", suppliers(u));
        List<Map<String, Object>> accounts = repo.accountsWithCustomGroup(u);
        out.put("taxAccounts", accountsWith(accounts, TAX_ACCOUNT_TYPES));
        out.put("discountAccounts", accountsWith(accounts, DISCOUNT_ACCOUNT_TYPES));
        out.put("glAccounts", accountsWithout(accounts, GL_EXCLUDED_TYPES));
        out.put("paymentTerms", paymentTerms(u));
        out.put("otherItems", otherItems(u));
        return out;
    }

    /** Reset (:1891-1893) — the three generators, asked again after New / save / delete. */
    public Map<String, Object> numbers() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", repo.nextDocNo(u, fy, DOCUMENT_TYPE_ID));
        out.put("taxNo", repo.nextSalesTaxNo(u));
        out.put("branchSrNo", repo.nextBranchSrNo(u, fy, DOCUMENT_TYPE_ID, branch(u)));
        return out;
    }

    /**
     * SupplierDtFillFromGlobal — clsGlobalVariables.globalSupplierCustomer, which
     * DatatableHelper.GlobalSupplierCustomerListsFillDbCall(org, comp, partyTypeId 0) builds from
     * USP_GetVendorsAndCustomersWithCityName: groups 7/9/10 always out; 8 and 13 also out when
     * ShowBothVendorAndCustomerOnSalesPurchase == 1; otherwise (party type 0) everything else in.
     */
    private List<Map<String, Object>> suppliers(UserAccount u) {
        int showBoth = toInt(common.config(u, "ShowBothVendorAndCustomerOnSalesPurchase"));
        Set<Integer> excluded = new HashSet<>(java.util.Arrays.asList(7, 8, 9, 10, 13));
        Set<Integer> mustExclude = new HashSet<>(java.util.Arrays.asList(7, 9, 10));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.vendorsAndCustomers(u)) {
            int group = toInt(r.get("CustomerGroupId"));
            if (mustExclude.contains(group)) continue;
            if (showBoth == 1 && excluded.contains(group)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("CompanyName", str(r.get("CompanyName")));
            o.put("PartyCode", str(r.get("PartyCode")));
            o.put("GlAccountId", toInt(r.get("GlAccountId")));
            o.put("CityId", toInt(r.get("CityId")));
            o.put("CityName", str(r.get("CityName")));
            o.put("MobileNo", str(r.get("MobilePersonal")));
            out.add(o);
        }
        return out;
    }

    /** PaymentTermBindFromGlobal — Id / TermsDescription as "Description". */
    private List<Map<String, Object>> paymentTerms(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.paymentTerms(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Description", str(r.get("TermsDescription")));
            out.add(o);
        }
        return out;
    }

    /** OtherItemdtDbCall — the expense grid's ItemId combo (Id / OtherItemName). */
    private List<Map<String, Object>> otherItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.otherItems(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("OtherItemName", str(r.get("OtherItemName")));
            out.add(o);
        }
        return out;
    }

    /** HistoryComboBind — Activity "Supplier" rows of Usp_AllComboAgainstPurchaseInvoice ("64"). */
    private List<Map<String, Object>> historySuppliers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.historyCombos(u, String.valueOf(DOCUMENT_TYPE_ID))) {
            if (!"Supplier".equals(str(r.get("Activity")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            out.add(o);
        }
        return out;
    }

    /** HistoryBranchComboDbCall once BranchImplemented has been read (:2150). */
    private List<Map<String, Object>> historyBranchesAfterLoad(UserAccount u) {
        if (toBool(common.config(u, "PurchaseInvoiceBranchWise"))) {
            List<Map<String, Object>> one = new ArrayList<>();
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", branch(u));
            o.put("BranchName", repo.branchName(u, branch(u)));
            one.add(o);
            return one;
        }
        return branchRows(repo.branchesFromPurchaseInvoice(u, DOCUMENT_TYPE_ID));
    }

    private static List<Map<String, Object>> branchRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("BranchId")));
            o.put("BranchName", str(r.get("BranchName")));
            out.add(o);
        }
        return out;
    }

    /** DatatableHelper.GetTaxAccountsFromGlobal / GetAccountsFromGlobalByTypeIds(with): first row per ChartOfAccountId. */
    private static List<Map<String, Object>> accountsWith(List<Map<String, Object>> all, int[] types) {
        Set<Integer> t = new HashSet<>();
        for (int x : types) t.add(x);
        return accounts(all, t, true);
    }

    /** GetAccountsFromGlobalByTypeIds(null, excluded). */
    private static List<Map<String, Object>> accountsWithout(List<Map<String, Object>> all, int[] types) {
        Set<Integer> t = new HashSet<>();
        for (int x : types) t.add(x);
        return accounts(all, t, false);
    }

    private static List<Map<String, Object>> accounts(List<Map<String, Object>> all, Set<Integer> types, boolean include) {
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new HashSet<>();
        for (Map<String, Object> a : all) {
            boolean in = types.contains(toInt(a.get("AccountTypeId")));
            if (in != include) continue;
            int id = toInt(a.get("ChartOfAccountId"));
            if (!seen.add(id)) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("AccountTitle", str(a.get("AccountTitle")));
            o.put("AccountCode", str(a.get("AccountCode")));
            o.put("ParentAccountTitle", str(a.get("ParentAccountTitle")));
            out.add(o);
        }
        return out;
    }

    // =================================================================== entry-bar events

    /**
     * DocDate_Leave (:2738) — CommonServices.GetTaxScheduleDetailbyItemIds(ItemIds, DocDate.Value);
     * ItemIds is built as "," + id per row, leading comma included.
     */
    public List<Map<String, Object>> taxSchedule(String itemIds, String docDate, int recId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.taxScheduleByItemIds(u, itemIds == null ? "" : itemIds,
                StoreIssuanceService.formDate(docDate, recId))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("TaxNameId", toInt(r.get("TaxNameId")));
            o.put("TaxName", str(r.get("TaxName")));
            o.put("TaxPercent", toDouble(r.get("TaxPercent")));
            out.add(o);
        }
        return out;
    }

    // ============================================================================= loader

    /** frmPendingGrnStoreLoader LoadInvoices_Load (:123) — branches, combos, From date. */
    public Map<String, Object> loaderLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        /* BranchImplemented is never assigned in the loader, so it is always the proc list (:157). */
        out.put("branches", branchRows(repo.branchesFromGrn(u, GRN_DOCUMENT_TYPE_ID)));
        out.put("userBranchId", branch(u));
        out.put("userBranchName", repo.branchName(u, branch(u)));
        out.put("financialYearStart", financialYearStart(u, ctx.currentFinancialYearId()));
        return out;
    }

    /** ComboDbCall + CombosFill (:195-250) — Supplier / Item from USP_GetDataForDropDownFromGrn. */
    public Map<String, Object> loaderCombos(String branchIds) {
        UserAccount u = ctx.requireAccountingUser();
        String ids = allowedGrnBranches(u, branchIds);
        List<Map<String, Object>> sup = new ArrayList<>(), items = new ArrayList<>();
        for (Map<String, Object> r : repo.grnDropDowns(u, String.valueOf(GRN_DOCUMENT_TYPE_ID), ids)) {
            String a = str(r.get("Activity"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            if ("Item".equals(a)) items.add(o);
            else if ("Supplier".equals(a)) sup.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", sup);
        out.put("items", items);
        return out;
    }

    /**
     * PendingDataDbCall (:265). "Select branch first" is checked on the chosen ids; they are then
     * NOT sent (D1). FromDate / ToDate are the pickers' values.
     */
    public List<Map<String, Object>> pendingGrns(String branchIds, String from, String to, int fromDocNo, int toDocNo,
                                                 int itemId, int billToPartyId) {
        UserAccount u = ctx.requireAccountingUser();
        String ids = allowedGrnBranches(u, branchIds);
        if (ids.isEmpty()) throw new IllegalArgumentException("Select branch first");
        Timestamp f = isBlank(from) ? null : StoreIssuanceService.dateOnly(from);   // Start_Period is a midnight value
        Timestamp t = isBlank(to) ? null : StoreIssuanceService.pickerDate(to);      // designer default Now
        return plainRows(repo.pendingGrnStore(u, ctx.currentFinancialYearId(), GRN_DOCUMENT_TYPE_ID, f, t,
                fromDocNo, toDocNo, itemId, billToPartyId));
    }

    /**
     * LoadFreightData (:2077) + LoadExpData (:2094) for the loaded GRN ids and delivery-challan ids,
     * both strings built on the page exactly as :1952-1957 build them.
     */
    public Map<String, Object> grnExtras(String grnIds, String challanIds) {
        UserAccount u = ctx.requireAccountingUser();
        List<Integer> posted = ids(grnIds);
        /* W2 — only GRNs the dialog itself offers (org/company, current FY, type 48, base != 4, on no invoice). */
        List<Integer> own = repo.loadableGrnIds(u, ctx.currentFinancialYearId(), GRN_DOCUMENT_TYPE_ID,
                new ArrayList<>(new LinkedHashSet<>(posted)), 0);
        if (own.size() != new LinkedHashSet<>(posted).size()) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> freight = new ArrayList<>();
        if (!posted.isEmpty()) {
            for (Map<String, Object> r : repo.transporterAndFreightFromGrn(grnIds)) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("InvGrnId", toInt(r.get("MainId")));
                o.put("Transporter", toInt(r.get("Transporter")));
                o.put("Freight", toDouble(r.get("CarriageAmount")));
                o.put("Debit", 0d);
                freight.add(o);
            }
        }
        out.put("freight", freight);
        List<Map<String, Object>> exp = new ArrayList<>();
        if (!isBlank(challanIds)) {
            List<Integer> dc = ids(challanIds);
            dc.removeIf(x -> x == 0);
            if (!dc.isEmpty() && !ownChallans(own, dc)) throw new IllegalArgumentException("Record Not Found");
            for (Map<String, Object> r : repo.deliveryChallanExpenses(challanIds)) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", 0);
                o.put("ItemId", toInt(r.get("OtherItemId")));
                o.put("Qty", toDouble(r.get("Qty")));
                o.put("Rate", toDouble(r.get("Rate")));
                o.put("Amount", toDouble(r.get("Amount")));
                o.put("Remarks", str(r.get("Remarks")));
                exp.add(o);
            }
        }
        out.put("expenses", exp);
        return out;
    }

    /** W2 — the challan ids must be ones this company's GRN details point at. */
    private boolean ownChallans(List<Integer> grnIds, List<Integer> dc) {
        if (grnIds.isEmpty()) return false;
        Set<Integer> found = new HashSet<>();
        for (Map<String, Object> r : repo.grnDetailKeys(grnIds)) found.add(toInt(r.get("DeliveryChallanId")));
        return found.containsAll(dc);
    }

    private String allowedGrnBranches(UserAccount u, String branchIds) {
        Set<Integer> allowed = new HashSet<>();
        for (Map<String, Object> r : repo.branchesFromGrn(u, GRN_DOCUMENT_TYPE_ID)) allowed.add(toInt(r.get("BranchId")));
        return csv(ids(branchIds), allowed);
    }

    // ============================================================================ history

    /** GetAll (:2309). dateType: doc / entry / modify / approved (the four radio buttons). */
    public List<Map<String, Object>> history(boolean branchChosen, String branchIds, String dateType,
                                             boolean fromChecked, String from, boolean toChecked, String to,
                                             String fromDocNo, String toDocNo, int supplierId) {
        UserAccount u = ctx.requireAccountingUser();
        if (!branchChosen) throw new IllegalArgumentException("Select branch first");
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        Timestamp f = fromChecked && !isBlank(from) ? StoreIssuanceService.pickerDate(from) : null;
        Timestamp t = toChecked && !isBlank(to) ? StoreIssuanceService.pickerDate(to) : null;
        Timestamp df = null, dt = null, ef = null, et = null, mf = null, mt = null, af = null, at = null;
        String type = dateType == null ? "doc" : dateType;
        switch (type) {
            case "entry":    ef = f; et = t; break;
            case "modify":   mf = f; mt = t; break;
            case "approved": af = f; at = t; break;
            default:         df = f; dt = t; break;
        }
        Set<Integer> allowed = new HashSet<>();
        for (Map<String, Object> r : repo.branchesFromPurchaseInvoice(u, DOCUMENT_TYPE_ID)) allowed.add(toInt(r.get("BranchId")));
        allowed.add(branch(u));
        String ids = csv(ids(branchIds), allowed);
        List<Map<String, Object>> rows = repo.formHistory(u, DOCUMENT_TYPE_ID, viewAll, ctx.currentFinancialYearId(),
                u.getId(), df, dt, ef, et, mf, mt, af, at, cvIntText(fromDocNo), cvIntText(toDocNo), supplierId, ids);
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> added = new HashSet<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("Id"));
            if (!added.add(id)) continue;                                                    // :2423
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[] { "Id", "DocumentTypeId", "DocNo", "OrderNo", "DocDate", "BranchSrNo",
                    "BranchName", "PurchaseAgainst", "DueDays", "DueDate", "ManualBillNo", "SupplierName",
                    "ReferencePartyName", "CommissionAgent", "CommissionType", "CommRate", "CommAmount",
                    "CommissionRemarks", "BillAmount", "CurrencyName", "ExchangeRate", "FcyAmount", "BillType",
                    "ApprovedStatus", "EntryUser", "EntryDate", "ModifyUser", "ModifyDate", "ApprovedUser" }) {
                o.put(k, plain(ci(r, k)));
            }
            o.put("ApprovedDate", plain(ci(r, "PostDate")));                                      // r["PostDate"]
            o.put("RemarksHeader", ci(r, "RemarksHeader"));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // =============================================================================== read

    /** ReadById (:1721) — header, detail, freight, JL and expense rows. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("BranchSrNo", toInt(ci(h, "BranchSrNo")));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("DocDate", plain(ci(h, "DocDate")));
        out.put("SupplierCustomerId", toInt(ci(h, "SupplierCustomerId")));
        out.put("ManualBillNo", str(ci(h, "ManualBillNo")));
        out.put("TaxAccountId", toInt(ci(h, "ReferencePartyId")));                        // CmbTaxAccount.Value = ReferencePartyId
        out.put("SalesTaxNo", toInt(ci(h, "SalesTaxNo")));
        out.put("PaymentTermsId", toInt(ci(h, "PaymentTermsId")));
        out.put("DueDays", str(ci(h, "SupplierInvoiceNo")));                              // D11
        out.put("DueDate", plain(ci(h, "SupplierInvoiceDate")));
        out.put("BillAmount", toDouble(ci(h, "BillAmount")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("DiscountAccountId", toInt(ci(h, "DiscountAccountId")));
        out.put("DiscountAmount", toDouble(ci(h, "DiscountAmount")));
        out.put("IsApproved", toBool(str(ci(h, "IsApproved"))));
        out.put("GrnBaseDocumentTypeId", toInt(ci(h, "DocumentTypeSrNo")));
        out.put("voucherHeadId", common.voucherHeadId(u, DOCUMENT_TYPE_ID, id));
        out.put("rows", detailRows(repo.details(id)));
        List<Map<String, Object>> fr = new ArrayList<>();
        for (Map<String, Object> r : repo.freights(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("InvGrnId", toInt(r.get("InvGrnId")));
            o.put("Transporter", toInt(r.get("TansporterId")));
            o.put("Freight", toDouble(r.get("FreightAmount")));
            o.put("Debit", toDouble(r.get("Debit")));
            fr.add(o);
        }
        out.put("freight", fr);
        List<Map<String, Object>> jl = new ArrayList<>();
        for (Map<String, Object> r : repo.journals(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("AccountId", toInt(r.get("ChartofAccountId")));
            o.put("Remarks", str(r.get("JvRemarks")));
            o.put("Percentage", toDouble(r.get("JvPrcnt")));
            o.put("Qty", toDouble(r.get("JvQty")));
            o.put("Rate", toDouble(r.get("JvRate")));
            o.put("Debit", toDouble(r.get("JvDebit")));
            o.put("Credit", toDouble(r.get("JvCredit")));
            jl.add(o);
        }
        out.put("journal", jl);
        List<Map<String, Object>> ex = new ArrayList<>();
        for (Map<String, Object> r : repo.expenses(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemId", toInt(r.get("InvRevExpItemId")));
            o.put("Qty", toDouble(r.get("Qty")));
            o.put("Rate", toDouble(r.get("Rate")));
            o.put("Amount", toDouble(r.get("Amount")));
            o.put("Remarks", str(r.get("Remarks")));
            ex.add(o);
        }
        out.put("expenses", ex);
        return out;
    }

    /** frmPurchaseInvoiceStoreManagement_Helper.FillDetailFromListCommonForReadById. */
    private static List<Map<String, Object>> detailRows(List<Map<String, Object>> details) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(d.get("Id")));
            o.put("InvGrnId", toInt(d.get("InvGrnId")));
            o.put("InvGrnDetailId", toInt(d.get("InvGrnDetailId")));
            o.put("InvGrnDocumentTypeId", toInt(d.get("GrnDocumentTypeId")));
            o.put("GrnNo", toInt(d.get("GrnNo")));
            o.put("PurchaseOrderDocumentTypeId", toInt(d.get("PurchaseOrderDocumentTypeId")));
            o.put("PurchaseOrderId", toInt(d.get("PurchaseOrderId")));
            o.put("PurchaseOrderNo", toInt(d.get("PurchaseOrder")));
            o.put("PurchaseDemandDocumentTypeId", toInt(d.get("PurchaseDemandDocumentTypeId")));
            o.put("PurchaseDemandId", toInt(d.get("PurchaseDemandId")));
            o.put("PurchaseDemandNo", toInt(d.get("PurchaseDemandNo")));
            o.put("PreBillDocumentTypeId", toInt(d.get("PreBillDocumentTypeId")));
            o.put("PreBillId", toInt(d.get("PreBillId")));
            o.put("PreBillNo", toInt(d.get("PreBillNo")));
            o.put("DeliveryChallanDocumentTypeId", toInt(d.get("DeliveryChallanDocumentTypeId")));
            o.put("DeliveryChallanId", toInt(d.get("DeliveryChallanId")));
            o.put("DeliveryChallanNo", toInt(d.get("DeliveryChallanNo")));
            o.put("ItemId", toInt(d.get("ItemId")));
            o.put("ItemName", str(d.get("ItemName")));
            o.put("ItemUomId", toInt(d.get("ItemUOMId")));
            o.put("UOMCodeItem", str(d.get("UOMCodeItem")));
            o.put("ItemQty", toDouble(d.get("ItemQty")));
            o.put("WarehouseId", toInt(d.get("WarehouseId")));
            o.put("WareHouseName", str(d.get("WareHouseName")));
            o.put("ItemConditionId", toInt(d.get("ItemConditionId")));
            o.put("ItemCondition", str(d.get("ItemCondition")));
            o.put("RackId", toInt(d.get("RackId")));
            o.put("RackName", str(d.get("rackName")));
            o.put("Rate", roundEven(toDouble(d.get("ItemRate")), 2));
            o.put("ItemAmountWithoutDiscount", roundEven(toDouble(d.get("ItemAmountWithoutDiscount")), 2));
            o.put("DiscountAmount", roundEven(toDouble(d.get("DiscountAmount")), 2));
            o.put("ItemAmount", roundEven(toDouble(d.get("ItemAmount")), 2));
            o.put("TaxNameId", toInt(d.get("TaxNameId")));
            o.put("TaxName", str(d.get("TaxDescriptions")));
            o.put("TaxPercent", toDouble(d.get("TaxPercent")));
            o.put("TaxAmount", toDouble(d.get("TaxAmount")));
            o.put("BillAmount", toDouble(d.get("BillAmount")));
            o.put("Freights", toDouble(d.get("FreightAmount")));
            o.put("ExpenseAmount", toDouble(d.get("ExpenseAmount")));
            o.put("PoAttachments", toInt(d.get("NoOfAttachmentsPO")));
            o.put("DemandAttachments", toInt(d.get("NoOfAttachmentsPD")));
            o.put("PreBillAttachments", toInt(d.get("NoOfAttachmentsPreBill")));
            o.put("DeliveryChallanAttachments", toInt(d.get("NoOfAttachmentsDC")));
            o.put("GrnAttachments", toInt(d.get("NoOfAttachmentsGrn")));
            rows.add(o);
        }
        return rows;
    }

    // ============================================================================== print

    /** OpenSlip (:2688): 64 → 230 StorePurchaseInvoiceSlip230, otherwise 238. */
    public List<Map<String, Object>> slip(int id, int documentTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        if (id == 0) throw new IllegalArgumentException(documentTypeId == DOCUMENT_TYPE_ID ? "No Record Found For Display" : "Record Id Not Found");
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> rows = documentTypeId == DOCUMENT_TYPE_ID ? repo.storeBillSlip(u, id) : repo.storeBillWithTax(u, id);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        return plainRows(rows);
    }

    /** The slips' sub-report (InvRptPurchaseBillSupplierOthers). */
    public List<Map<String, Object>> slipSubReport(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (ownedHeader(u, id) == null) throw new IllegalArgumentException("No Record Found For Display");
        return plainRows(repo.supplierAddLessSubReport(id));
    }

    /** AcRptPurchaseSalesVoucherSlip_103(VoucherHeadIdGet(id, 64), 64). */
    public List<Map<String, Object>> voucherSlip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id > 0 && ownedHeader(u, id) == null) throw new IllegalArgumentException("No Record Found For Display");
        int vh = id > 0 ? common.voucherHeadId(u, DOCUMENT_TYPE_ID, id) : 0;
        if (vh == 0) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> rows = repo.voucherSlip(u, vh, DOCUMENT_TYPE_ID);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        return plainRows(rows);
    }

    // ============================================================================= delete

    /** btnDelete_Click (:1681) → BLL 0581 RemoveByID → Sp_InvoicesVouchersandStocksDelete. */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "delete")) throw new org.springframework.security.access.AccessDeniedException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("RecordId Not Found.....");
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) throw new IllegalArgumentException("RecordId Not Found.....");
        if (toBool(str(ci(h, "IsApproved")))) throw new IllegalArgumentException("Record Not Update because Record has approved");
        common.removeInvoiceVoucherAndStock(u, DOCUMENT_TYPE_ID, id, u.getId());
        return ok("Delete Record Successfully", id, 0);
    }

    // =============================================================================== save

    /** btnSave_Click / btnUpdate_Click → Insert() (:1383). */
    @Transactional
    public Map<String, Object> save(PurchaseInvoiceStoreMgmtDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new org.springframework.security.access.AccessDeniedException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new org.springframework.security.access.AccessDeniedException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = ownedHeader(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Not Found");
        }
        int fy = ctx.currentFinancialYearId();
        List<PurchaseInvoiceStoreMgmtDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        List<PurchaseInvoiceStoreMgmtDto.Freight> freight = dto.freight == null ? new ArrayList<>() : dto.freight;
        List<PurchaseInvoiceStoreMgmtDto.Journal> journal = dto.journal == null ? new ArrayList<>() : dto.journal;
        List<PurchaseInvoiceStoreMgmtDto.Expense> expenses = dto.expenses == null ? new ArrayList<>() : dto.expenses;

        // ----------------------------------------------------- checks before the prompt (:1397-1474)
        if (rows.isEmpty()) throw new IllegalArgumentException("Detail Record Not Found");
        int supplierId = i(dto.SupplierCustomerId);
        Map<String, Object> supplier = null;
        for (Map<String, Object> s : suppliers(u)) if (toInt(s.get("Id")) == supplierId && supplierId != 0) supplier = s;
        if (supplier == null) throw new IllegalArgumentException("Supplier field is required");
        /* W1 — Doc No / Branch Sr No / Tax No from their sources. */
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : repo.nextDocNo(u, fy, DOCUMENT_TYPE_ID);
        int branchSrNo = existing != null ? toInt(ci(existing, "BranchSrNo")) : repo.nextBranchSrNo(u, fy, DOCUMENT_TYPE_ID, branch(u));
        if (docNo == 0) throw new IllegalArgumentException("Doc No must be a non-zero number");
        int paymentTermId = i(dto.PaymentTermsId);
        if (paymentTermId == 0 || !contains(paymentTerms(u), paymentTermId)) throw new IllegalArgumentException("Payment Term field is required");
        double billAmount = parseDouble(dto.BillAmount);
        if (Double.isNaN(billAmount) || billAmount == 0d) throw new IllegalArgumentException("Bill amount must be a non-zero number");
        int dueDays = cvIntText(dto.DueDays);
        if (paymentTermId == 2 && dueDays == 0) throw new IllegalArgumentException("Due Days Field is Required");
        List<Map<String, Object>> accounts = repo.accountsWithCustomGroup(u);
        int discountAccountId = i(dto.DiscountAccountId);
        if (discountAccountId != 0 && !contains(accountsWith(accounts, DISCOUNT_ACCOUNT_TYPES), discountAccountId)) {
            throw new IllegalArgumentException("Discount Account Field is Required");
        }
        double discountAmount = isBlank(dto.DiscountAmount) ? 0d : cvDouble(dto.DiscountAmount);
        if (discountAccountId > 0 || discountAmount > 0d) {
            if (discountAccountId == 0) throw new IllegalArgumentException("Discount Account Field is Required");
            if (discountAmount == 0d) throw new IllegalArgumentException("Discount Amount Field is Required");
        }
        boolean hasTax = false;
        for (PurchaseInvoiceStoreMgmtDto.Row r : rows) if (i(r.TaxNameId) > 0) hasTax = true;           // :1436
        int taxAccountId = i(dto.TaxAccountId);
        boolean taxAccountValid = taxAccountId != 0 && contains(accountsWith(accounts, TAX_ACCOUNT_TYPES), taxAccountId);
        if (taxAccountId != 0 && !taxAccountValid) throw new IllegalArgumentException("Tax Account field is required");
        int taxNo = existing != null ? toInt(ci(existing, "SalesTaxNo")) : repo.nextSalesTaxNo(u);
        if (hasTax) {
            if (!taxAccountValid) throw new IllegalArgumentException("Tax Account field is required");
            if (taxNo == 0) throw new IllegalArgumentException("Tax Invoice No must be a non-zero number");
        }
        List<Map<String, Object>> glAccounts = accountsWithout(accounts, GL_EXCLUDED_TYPES);
        for (PurchaseInvoiceStoreMgmtDto.Journal j : journal) {
            if ((d(j.Debit) > 0d || d(j.Credit) > 0d) && i(j.AccountId) == 0) {
                throw new IllegalArgumentException("Please Select an Account Against JL First");
            }
            if (i(j.AccountId) != 0 && !contains(glAccounts, i(j.AccountId))) throw new IllegalArgumentException("Please Select an Account Against JL First");
        }
        for (PurchaseInvoiceStoreMgmtDto.Freight f : freight) {
            if (d(f.Freight) > 0d && i(f.Transporter) == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
            if (i(f.Transporter) != 0 && !contains(glAccounts, i(f.Transporter))) throw new IllegalArgumentException("Please Select an Account Against Freight First");
        }
        /* W2 — every row must be a GRN line the Load-GRN dialog could have offered: its GRN passes the
           loader's own filters (org/company, current FY, type 48, base != 4, on no other invoice — this
           invoice's own GRNs allowed on update), the (InvGrnId, InvGrnDetailId) pair exists, and the
           row's PurchaseOrderId is that GRN line's. A freight row's InvGrnId must be one of the rows' GRNs.
           On update a detail Id must be this invoice's. */
        List<Integer> grnIds = new ArrayList<>();
        for (PurchaseInvoiceStoreMgmtDto.Row r : rows) if (!grnIds.contains(i(r.InvGrnId))) grnIds.add(i(r.InvGrnId));
        if (grnIds.contains(0)) throw new IllegalArgumentException("Record Not Found");
        for (PurchaseInvoiceStoreMgmtDto.Freight f : freight) {
            if (i(f.InvGrnId) != 0 && !grnIds.contains(i(f.InvGrnId))) throw new IllegalArgumentException("Record Not Found");
        }
        if (repo.loadableGrnIds(u, fy, GRN_DOCUMENT_TYPE_ID, grnIds, recId).size() != grnIds.size()) {
            throw new IllegalArgumentException("Record Not Found");
        }
        Map<String, Integer> grnLines = new HashMap<>();
        for (Map<String, Object> k : repo.grnDetailKeys(grnIds)) {
            grnLines.put(toInt(k.get("InvGrnId")) + ":" + toInt(k.get("Id")), toInt(k.get("PurchaseOrderId")));
        }
        for (PurchaseInvoiceStoreMgmtDto.Row r : rows) {
            Integer po = grnLines.get(i(r.InvGrnId) + ":" + i(r.InvGrnDetailId));
            if (po == null || po != i(r.PurchaseOrderId)) throw new IllegalArgumentException("Record Not Found");
        }
        if (existing != null) {
            Set<Integer> mine = new HashSet<>();
            for (Map<String, Object> d0 : repo.details(recId)) mine.add(toInt(d0.get("Id")));
            for (PurchaseInvoiceStoreMgmtDto.Row r : rows) if (i(r.Id) > 0 && !mine.contains(i(r.Id))) throw new IllegalArgumentException("Record Not Found");
        }
        List<Map<String, Object>> otherItems = otherItems(u);
        for (PurchaseInvoiceStoreMgmtDto.Expense e : expenses) {
            if (i(e.ItemId) != 0 && !contains(otherItems, i(e.ItemId))) throw new IllegalArgumentException("Other Item Purchase GL Account Not found");
        }
        if (!Boolean.TRUE.equals(dto.confirmed)) {                                                      // W3
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("confirm", recId == 0 ? "Are you sure to Save" : "Are you sure to Update");
            return m;
        }

        // ----------------------------------------------------------------- the object (:1480-1603)
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        Timestamp dueDate = dueDate(dto.DueDate, existing);
        String remarksHeader = trim(dto.RemarksHeader);
        Map<String, Object> head = headerModel();
        head.put("DocDate", docDate);
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("DueDate", dueDate);
        head.put("SupplierInvoiceDate", dueDate);
        head.put("BillAmount", billAmount);
        head.put("BranchesId", branch(u));
        head.put("BranchSrNo", branchSrNo);
        head.put("FinancialYearId", fy);
        head.put("CompanyId", u.getCompanyId());
        head.put("DocNo", docNo);
        head.put("SalesTaxNo", hasTax ? taxNo : 0);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("DocumentTypeSrNo", i(dto.GrnBaseDocumentTypeId));
        head.put("EntryUser", u.getId());
        head.put("Id", recId);
        head.put("ModifyUser", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("ProjectsId", 0);
        head.put("TaxAccountId", taxAccountId);
        head.put("ReferencePartyId", taxAccountId);
        head.put("SupplierCustomerId", supplierId);
        head.put("SupplierInvoiceNo", dueDays);                                                         // D11
        head.put("DueDays", dueDays);
        head.put("CommissionRemarks", "");
        head.put("CommissionType", "");
        head.put("IsTaxable", hasTax);
        head.put("ManualBillNo", trim(dto.ManualBillNo));
        head.put("RemarksHeader", remarksHeader);
        head.put("ScreenName", SCREEN_NAME);
        head.put("UomScheduleIdCmRate", "");
        head.put("PaymentTermsId", paymentTermId);
        head.put("DiscountAmount", cvDouble(trim(dto.DiscountAmount)));
        head.put("DiscountAccountId", discountAccountId);
        head.put("AttachmentsValues", existing == null ? "" : str(ci(existing, "AttachmentsValues")));
        head.put("CustomAttachmentsValues", existing == null ? "" : str(ci(existing, "CustomAttachmentsValues")));

        List<Map<String, Object>> details = new ArrayList<>();
        int lineId = 1;
        for (int idx = 0; idx < rows.size(); idx++) {
            PurchaseInvoiceStoreMgmtDto.Row r = rows.get(idx);
            Map<String, Object> vd = detailModel();
            vd.put("LineId", lineId);
            vd.put("Id", recId != 0 ? i(r.Id) : 0);
            fillDetail(vd, r);
            validate(vd.get("WarehouseId"), "WareHouse", idx);
            validate(vd.get("ItemId"), "Item", idx);
            validate(vd.get("ItemUOMId"), "Pack Uom", idx);
            validate(vd.get("ItemConditionId"), "Item Condition", idx);
            validate(vd.get("RackId"), "Rack Name", idx);
            validate(vd.get("ItemQty"), "Qty", idx);
            validate(vd.get("ItemRate"), "Rate", idx);
            validate(vd.get("ItemAmount"), "Amount", idx);
            if (toInt(vd.get("TaxNameId")) > 0 || toDouble(vd.get("TaxPercent")) > 0d || toDouble(vd.get("TaxAmount")) > 0d) {
                validate(vd.get("TaxNameId"), "Tax Name", idx);
                validate(vd.get("TaxPercent"), "Tax Percent", idx);
                validate(vd.get("TaxAmount"), "Tax Amount", idx);
            }
            validate(vd.get("BillAmount"), "Bill Amount", idx);
            details.add(vd);
            lineId++;
        }
        List<Map<String, Object>> freights = new ArrayList<>();
        for (PurchaseInvoiceStoreMgmtDto.Freight f : freight) {
            if (i(f.Transporter) == 0) continue;
            Map<String, Object> pf = freightModel();
            pf.put("InvGrnId", i(f.InvGrnId));
            pf.put("TansporterId", i(f.Transporter));
            pf.put("FreightAmount", (double) cvInt(d(f.Freight)));                                    // D5
            pf.put("Debit", (double) cvInt(d(f.Debit)));
            freights.add(pf);
        }
        List<Map<String, Object>> journals = new ArrayList<>();
        for (PurchaseInvoiceStoreMgmtDto.Journal j : journal) {
            if (i(j.AccountId) == 0) continue;
            Map<String, Object> pj = journalModel();
            pj.put("ChartofAccountId", i(j.AccountId));
            pj.put("JvRemarks", j.Remarks == null ? "" : j.Remarks);
            pj.put("JvDebit", d(j.Debit));
            pj.put("JvCredit", d(j.Credit));
            pj.put("JvPrcnt", d(j.Percentage));
            pj.put("JvQty", d(j.Qty));
            pj.put("JvRate", d(j.Rate));
            journals.add(pj);
        }
        List<Map<String, Object>> exps = new ArrayList<>();
        for (int idx = 0; idx < expenses.size(); idx++) {
            PurchaseInvoiceStoreMgmtDto.Expense e = expenses.get(idx);
            if (i(e.ItemId) == 0) continue;
            if (d(e.Amount) == 0d) throw new IllegalArgumentException("Amount Field Required in Expense Grid Row no : " + (idx + 1));
            Map<String, Object> pe = expenseModel();
            pe.put("Id", i(e.Id));
            pe.put("InvRevExpItemId", i(e.ItemId));
            pe.put("Qty", (double) cvInt(d(e.Qty)));                                                   // D5
            pe.put("Rate", d(e.Rate));
            pe.put("Amount", d(e.Amount));
            pe.put("Remarks", e.Remarks == null ? "" : e.Remarks);
            exps.add(pe);
        }

        // ------------------------------------------------------------------ BLL 0581 Save
        String companyName = str(supplier.get("CompanyName"));
        Voucher voucher = makeVoucher(u, head, details, freights, journals, exps);
        if (recId == 0) {
            head.put("ActionId", 1);
        } else {
            repo.recordDate(recId);                                                                     // obj.PreviousDate (virtual)
            head.put("ActionId", 2);
        }
        int id = setData(u, head, details, freights, journals, exps, voucher, recId == 0 ? "Sp_InvPurchaseInvoice_Insert" : "Sp_InvPurchaseInvoice_Update");
        LOG.debug("Store purchase invoice {} saved for supplier {}", id, companyName);
        return ok(recId == 0 ? "Record Saved Successfully [" + docNo + "] " : "Record Update Successfully [" + docNo + "] ", id, docNo);
    }

    /** FillDetailListCommonForInsertAndDelete (:1631) — only the non-virtual properties reach the procedure. */
    private static void fillDetail(Map<String, Object> pd, PurchaseInvoiceStoreMgmtDto.Row r) {
        pd.put("InvGrnDetailId", i(r.InvGrnDetailId));
        pd.put("InvGrnId", i(r.InvGrnId));
        pd.put("PurchaseOrderId", i(r.PurchaseOrderId));
        pd.put("WarehouseId", i(r.WarehouseId));
        pd.put("ItemId", i(r.ItemId));
        pd.put("ItemUOMId", i(r.ItemUomId));
        pd.put("ItemConditionId", i(r.ItemConditionId));
        pd.put("RackId", i(r.RackId));
        double qty = d(r.ItemQty);
        pd.put("ItemQty", qty);
        pd.put("GrossWeight", qty);
        pd.put("NetBillWeight", qty);
        pd.put("NetStockWeight", qty);
        pd.put("ItemRate", d(r.Rate));
        pd.put("ItemAmountWithoutDiscount", BigDecimal.valueOf(d(r.ItemAmountWithoutDiscount)));
        pd.put("DiscountAmount", d(r.DiscountAmount));
        pd.put("ItemAmount", d(r.ItemAmount));
        pd.put("TaxNameId", i(r.TaxNameId));
        pd.put("TaxDescriptions", r.TaxName == null ? "" : r.TaxName);
        pd.put("TaxPercent", d(r.TaxPercent));
        pd.put("TaxAmount", d(r.TaxAmount));
        pd.put("BillAmount", d(r.BillAmount));
        pd.put("FreightAmount", d(r.Freights));
        pd.put("ExpenseAmount", d(r.ExpenseAmount));
        /* virtual on model 1037 — kept for the voucher text only, never sent (D9) */
        pd.put("~ItemName", r.ItemName == null ? "" : r.ItemName);
        pd.put("~UOMCodeItem", r.UOMCodeItem == null ? "" : r.UOMCodeItem);
        pd.put("~PurchaseOrder", i(r.PurchaseOrderNo));
    }

    /**
     * DAL 0434 SetData (:20-578) for DocumentTypeId 64, in one transaction:
     *   header Insert|Update → details (BillAmount recomputed, LineId 1..n) → freight → journal →
     *   expense → usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations →
     *   usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId →
     *   Sp_InventoryStockEvalautionDetail_Update (64 is not 59) → voucher head Insert|Update →
     *   Sp_VoucherDetail_Insert each → USP_VoucherBalanceCheck → Sp_VoucherHead_H_Insert →
     *   Sp_VoucherDetail_H_Insert each → [DAW].[USp_DocumentApprovalDetail_Insert].
     * 64 is not in the InventoryTransactions set (:524), so no Sp_InventoryTransactions_GetALLMethod,
     * and not 59, so no USP_InventoryValidation.
     */
    private int setData(UserAccount u, Map<String, Object> head, List<Map<String, Object>> details,
                        List<Map<String, Object>> freights, List<Map<String, Object>> journals,
                        List<Map<String, Object>> exps, Voucher voucher, String proc) {
        if (details.isEmpty()) throw new IllegalArgumentException("Detail List not found");
        BigDecimal invoiceQty = BigDecimal.ZERO, invoiceWeight = BigDecimal.ZERO;
        double sq = 0d, sw = 0d;
        for (Map<String, Object> dd : details) { sq += toDouble(dd.get("ItemQty")); sw += toDouble(dd.get("NetBillWeight")); }
        invoiceQty = BigDecimal.valueOf(sq);
        invoiceWeight = BigDecimal.valueOf(sw);
        head.put("InvoiceQty", invoiceQty);
        head.put("InvoiceWeight", invoiceWeight);

        int recId = toInt(head.get("Id"));
        int num3 = repo.setProc(proc, head);
        if (num3 > 0) head.put("Id", num3); else num3 = recId;
        if (num3 <= 0) throw new IllegalArgumentException("Save returned no document id.");      // W4
        int id = num3;

        boolean wagesToProduct = toBool(common.config(u, "ContractWagesChargetoProduct"));
        StringBuilder detailIds = new StringBuilder();
        int n = 0;
        for (Map<String, Object> dd : details) {
            dd.put("LineId", ++n);
            /* :87-94 — DocumentTypeId 64 is not 98. */
            double bill = toDouble(dd.get("ItemAmount")) + toDouble(dd.get("FreightAmount")) + toDouble(dd.get("ExpenseAmount"))
                    + toDouble(dd.get("CommissionAmount")) + toDouble(dd.get("Brokery"))
                    + (wagesToProduct ? toDouble(dd.get("WagesAmount")) : 0d)
                    - toDouble(dd.get("EbPurAgainstWeightAmount")) - toDouble(dd.get("FreightDeduction"));
            dd.put("BillAmount", bill);
            dd.put("InvPurchaseInvoiceId", id);
            dd.put("Id", repo.setProc("Sp_InvPurchaseInvoiceDetail_Insert", sendable(dd)));
            detailIds.append(toInt(dd.get("Id"))).append(",");
        }
        for (Map<String, Object> f : freights) { f.put("InvPurchaseInvoiceId", id); repo.setProc("Sp_InvPurchaseInvoiceFreight_Insert", f); }
        for (Map<String, Object> j : journals) { j.put("InvPurchaseInvoiceId", id); repo.setProc("Sp_InvPurchaseInvoiceJournal_Insert", j); }
        for (Map<String, Object> e : exps)     { e.put("InvPurchaseInvoiceId", id); repo.setProc("Sp_InvPurchaseInvoiceExpense_Insert", e); }
        repo.removeDetailsNotIn(u, id, detailIds.toString());
        repo.stockInTransitDelete(id);
        repo.evaluationUpdate(u, DOCUMENT_TYPE_ID, id);

        ContraVoucherDto.Head vh = voucher.head;
        int existingVh = common.voucherHeadId(u, DOCUMENT_TYPE_ID, id);
        if (existingVh > 0) vh.Id = existingVh;
        vh.DocumentTypeSrNo = id;
        int num = repo.setProc(existingVh == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(vh));
        if (num > 0) vh.Id = num;
        if (voucher.lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail list Not Found");
        for (ContraVoucherDto.Detail l : voucher.lines) {
            l.VoucherHeadId = vh.Id;
            l.BranchesId = branch(u);
            repo.setProc("Sp_VoucherDetail_Insert", model(l));
        }
        repo.voucherBalanceCheck(u, vh.Id);
        int documentTypeIdRef = repo.setProc("Sp_VoucherHead_H_Insert", model(vh));
        for (ContraVoucherDto.Detail l : voucher.lines) {
            l.VoucherHeadId = vh.Id;
            l.DocumentTypeIdRef = documentTypeIdRef;
            repo.setProc("Sp_VoucherDetail_H_Insert", model(l));
        }
        repo.approvalInsert(u, DOCUMENT_TYPE_ID, id, BigDecimal.valueOf(toDouble(head.get("BillAmount"))));
        return id;
    }

    // ============================================================================ voucher

    private static final class Voucher {
        ContraVoucherDto.Head head;
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
    }

    /**
     * BLL 0549 MakeVoucherForPurchaseInvoice for DocumentTypeId 64. Of the eight builders it calls
     * only these produce lines for 64: ItemAndSupplierTradingFinancial (items, freight charged to
     * product, expenses, bill discount), FreightFinancial, SupplierAddLessFinancial and
     * SalesTaxFinancial. ItemAndSupplierByWeightFinancial only reads a config for 64;
     * PurchaseReturn (59), Brokery (56/57/172/138) and EmptyBags (56/57/98/138/166/168/172) do not apply.
     */
    private Voucher makeVoucher(UserAccount u, Map<String, Object> head, List<Map<String, Object>> details,
                                List<Map<String, Object>> freights, List<Map<String, Object>> journals,
                                List<Map<String, Object>> exps) {
        Voucher v = new Voucher();
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        v.head = vh;
        int headId = toInt(head.get("Id"));
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = headId;
        vh.RefDocNoId = headId;                                                                  // D4
        vh.VoucherCode = toInt(head.get("DocNo"));
        vh.VoucherDate = head.get("DocDate").toString();
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = toDouble(head.get("BillAmount"));
        vh.MultiCurrencyId = 0;
        vh.ExchangeCurrencyRate = 0d;
        vh.FcAmount = 0d;
        String stockAgainst = common.config(u, "StockAgainstAccount");
        if (stockAgainst.isEmpty()) throw new IllegalArgumentException("Stock AgainstAc Configuration Not Found");
        int offsetAccountId = cvIntText(stockAgainst);
        String companyName = null;
        List<Map<String, Object>> list2 = repo.supplierGlList(u);
        if (!list2.isEmpty()) {
            Map<String, Object> s = null;
            int sid = toInt(head.get("SupplierCustomerId"));
            for (Map<String, Object> r : list2) if (toInt(r.get("Id")) == sid) { s = r; break; }
            if (s == null) throw new IllegalArgumentException("Supplier GLAccountId not Found");
            vh.RefAccountId = toInt(s.get("GlAccountId"));
            companyName = str(s.get("CompanyName"));
        }
        List<Map<String, Object>> items = common.itemGlAccounts(u);
        int supplierGl = vh.RefAccountId;
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();
        vh.IncludeWHT = false;
        vh.BranchId = branch(u);
        vh.ProjectId = 0;
        vh.BillAmount = toDouble(head.get("BillAmount"));
        vh.ManualBillNo = (String) head.get("ManualBillNo");
        vh.DueDate = head.get("DueDate").toString();
        vh.DueDays = toInt(head.get("DueDays"));
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = toInt(head.get("FinancialYearId"));
        vh.EntryUser = u.getId();
        String now = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = u.getId();

        trading(u, v, head, details, exps, items, supplierGl, offsetAccountId, companyName);
        freightFinancial(v, freights, supplierGl, companyName);
        supplierAddLess(v, journals, supplierGl, toInt(head.get("SupplierCustomerId")));
        salesTax(v, head, details, supplierGl);

        String remarks = (String) head.get("RemarksHeader");
        if (!"".equals(remarks)) vh.Remarks = remarks;
        return v;
    }

    /** BLL 0614 ItemAndSupplierTradingFinancial (:1282), branch "case 64". */
    private void trading(UserAccount u, Voucher v, Map<String, Object> head, List<Map<String, Object>> details,
                         List<Map<String, Object>> exps, List<Map<String, Object>> items, int supplierGl,
                         int offsetAccountId, String companyName) {
        int supplierId = toInt(head.get("SupplierCustomerId"));
        double num = 0d, num2 = 0d;
        for (Map<String, Object> d3 : details) {
            Map<String, Object> g = itemGl(items, toInt(d3.get("ItemId")));
            if (g == null) throw new IllegalArgumentException("Item Purchase GL Account Not found");
            /* JobLotId is always 0 here, so jobLot = new JobLot() and AccountId is 0 → PurchaseGLAC. */
            int purchaseGl = toInt(g.get("PurchaseGLAC"));
            double freight = toDouble(d3.get("FreightAmount"));
            StringBuilder sb = new StringBuilder();
            sb.append(str(g.get("ItemName"))).append(" ").append("")                   // JobLotDescription (null)
              .append(" ").append(str(d3.get("~UOMCodeItem")))
              .append(" ").append(clr(toDouble(d3.get("NetBillWeight"))))
              .append(" @").append(clr(toDouble(d3.get("ItemRate")))).append("/-");
            if (freight > 0d) sb.append(" Freight Amount: ").append(clr(Math.rint(freight)));
            String text = sb.toString();
            double amount = toDouble(d3.get("ItemAmount"));

            ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
            dr.AccountId = purchaseGl;
            dr.AgainstAccountId = supplierGl;
            dr.Comments = text;
            dr.DebitAmount = amount;
            dr.CreditAmount = 0d;
            dr.ItemId = toInt(d3.get("ItemId"));
            dr.JobLotId = 0;
            dr.QtyIn = toDouble(d3.get("ItemQty"));
            dr.ItemRate = toDouble(d3.get("ItemRate"));
            dr.WeightIn = toDouble(d3.get("NetBillWeight"));
            dr.RateCut = 0d;
            dr.RateCutAmount = 0d;
            dr.ItemAmount = amount;
            dr.Freight = freight;
            dr.Commission = 0d;
            dr.OrderNo = toInt(d3.get("~PurchaseOrder"));
            dr.GpNo = 0;
            dr.VehicleNo = null;
            dr.SupplierCustomerId = supplierId;
            v.lines.add(dr);

            ContraVoucherDto.Detail cr = new ContraVoucherDto.Detail();
            cr.AccountId = supplierGl;
            cr.AgainstAccountId = purchaseGl;
            cr.Comments = text;
            cr.CreditAmount = amount;
            cr.DebitAmount = 0d;
            cr.ItemId = toInt(d3.get("ItemId"));
            cr.JobLotId = 0;
            cr.QtyIn = toDouble(d3.get("ItemQty"));
            cr.ItemRate = toDouble(d3.get("ItemRate"));
            cr.RateCut = 0d;
            cr.RateCutAmount = 0d;
            cr.WeightIn = toDouble(d3.get("NetBillWeight"));
            cr.ItemAmount = amount;
            cr.Freight = freight;
            cr.Commission = 0d;
            cr.OrderNo = toInt(d3.get("~PurchaseOrder"));
            cr.GpNo = 0;
            cr.VehicleNo = null;
            cr.SupplierCustomerId = supplierId;
            cr.SubsidiaryAccountId = supplierId;
            cr.SubsidiaryTypeId = 1;
            v.lines.add(cr);
            num += freight;
            num2 += toDouble(d3.get("NetBillWeight"));
        }
        if (num > 0d) {                                                                            // :1430
            for (Map<String, Object> d2 : details) {
                Map<String, Object> g = itemGl(items, toInt(d2.get("ItemId")));
                if (g == null) throw new IllegalArgumentException("Item Purchase GL Account Not found");
                double freight = toDouble(d2.get("FreightAmount"));
                ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
                l.AccountId = toInt(g.get("PurchaseGLAC"));
                l.AgainstAccountId = offsetAccountId;
                l.DebitAmount = freight;
                l.Comments = "ChargeToProduct/TotalWeightInDetail * Qty = Proportionated Charges : " + clr(num) + " / "
                        + clr(num2) + " * " + clr(toDouble(d2.get("ItemQty"))) + " = " + clr(Math.rint(freight));
                l.CreditAmount = 0d;
                l.ItemId = toInt(d2.get("ItemId"));
                l.JobLotId = 0;
                v.lines.add(l);
            }
        }
        if (!exps.isEmpty()) {                                                                     // :1466
            List<Map<String, Object>> other = repo.otherItems(u);
            for (Map<String, Object> e : exps) {
                Map<String, Object> m = null;
                for (Map<String, Object> o : other) if (toInt(o.get("Id")) == toInt(e.get("InvRevExpItemId"))) { m = o; break; }
                if (m == null) throw new IllegalArgumentException("Other Item Purchase GL Account Not found");
                String otherName = str(m.get("OtherItemName"));
                int stockGl = toInt(m.get("StockGLAcId"));
                if (stockGl <= 0) throw new IllegalArgumentException("GL Account mapping missing for '" + otherName + "'.");
                List<String> parts = new ArrayList<>();
                String rem = str(e.get("Remarks"));
                if (!rem.trim().isEmpty()) parts.add(rem.trim());
                if (!otherName.trim().isEmpty()) parts.add(otherName.trim());
                double qty = toDouble(e.get("Qty")), rate = toDouble(e.get("Rate"));
                if (qty > 0d) parts.add("Qty " + clr(qty));
                if (rate > 0d) parts.add("Rate " + clr(rate));
                String comments = String.join(" ", parts);
                ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
                a.AccountId = stockGl;
                a.AgainstAccountId = supplierGl;
                a.DebitAmount = toDouble(e.get("Amount"));
                a.Comments = comments;
                v.lines.add(a);
                ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
                b.AccountId = supplierGl;
                b.AgainstAccountId = stockGl;
                b.CreditAmount = toDouble(e.get("Amount"));
                b.Comments = comments;
                v.lines.add(b);
            }
        }
        int discountAccountId = toInt(head.get("DiscountAccountId"));
        double discount = toDouble(head.get("DiscountAmount"));
        if (discountAccountId > 0 && discount > 0d) {                                             // :1518
            String text3 = "Discount " + format0dd(discount);
            String docNo = String.valueOf(toInt(head.get("DocNo")));
            ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
            a.AccountId = supplierGl;
            a.AgainstAccountId = discountAccountId;
            a.Comments = text3 + " received from " + (companyName == null ? "" : companyName) + " (Inv: " + docNo + ")";
            a.DebitAmount = discount;
            v.lines.add(a);
            ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
            b.AccountId = discountAccountId;
            b.AgainstAccountId = supplierGl;
            b.Comments = text3 + " allowed (Inv: " + docNo + ")";
            b.CreditAmount = discount;
            b.SupplierCustomerId = toInt(head.get("SupplierCustomerId"));
            b.SubsidiaryAccountId = toInt(head.get("SupplierCustomerId"));
            b.SubsidiaryTypeId = 1;
            v.lines.add(b);
        }
    }

    /** BLL 0614 FreightFinancial (:292) — D6: against-account is the transporter itself. */
    private static void freightFinancial(Voucher v, List<Map<String, Object>> freights, int supplierGl, String companyName) {
        int against = 0;                                                                           // Distinct PurchaseGLAC == {0}
        for (Map<String, Object> f : freights) {
            int t = toInt(f.get("TansporterId"));
            double debit = toDouble(f.get("Debit"));
            double amount = toDouble(f.get("FreightAmount"));
            if (t > 0 && debit > 0d) {
                ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
                a.AccountId = t;
                a.AgainstAccountId = against > 0 ? against : a.AccountId;
                a.Comments = "PartyName :  " + str(companyName) + "  Freight : " + clr(amount);
                a.DebitAmount = debit;
                a.CreditAmount = 0d;
                v.lines.add(a);
            }
            if (t > 0 && amount > 0d) {
                ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
                b.AccountId = t;
                b.AgainstAccountId = against > 0 ? against : b.AccountId;
                b.Comments = "SupplierName :  " + str(companyName) + "  Freight : " + clr(amount);
                b.DebitAmount = 0d;
                b.CreditAmount = amount;
                v.lines.add(b);
            }
        }
    }

    /** BLL 0614 SupplierAddLessFinancial (:171). */
    private static void supplierAddLess(Voucher v, List<Map<String, Object>> journals, int supplierGl, int supplierId) {
        for (Map<String, Object> j : journals) {
            int acc = toInt(j.get("ChartofAccountId"));
            double debit = toDouble(j.get("JvDebit")), credit = toDouble(j.get("JvCredit"));
            if (acc <= 0 || !(credit > 0d || debit > 0d)) continue;
            String rem = str(j.get("JvRemarks"));
            ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
            a.AccountId = acc;
            a.AgainstAccountId = supplierGl;
            a.Comments = rem;
            a.DebitAmount = debit;
            a.CreditAmount = credit;
            v.lines.add(a);
            ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
            b.AccountId = supplierGl;
            b.AgainstAccountId = acc;
            b.Comments = rem;
            b.DebitAmount = credit;
            b.CreditAmount = debit;
            b.SupplierCustomerId = supplierId;
            b.SubsidiaryAccountId = supplierId;
            b.SubsidiaryTypeId = 1;
            v.lines.add(b);
        }
    }

    /** BLL 0614 SalesTaxFinancial (:232) — 64 posts against ReferencePartyId. */
    private static void salesTax(Voucher v, Map<String, Object> head, List<Map<String, Object>> details, int supplierGl) {
        int taxAc = toInt(head.get("ReferencePartyId"));
        for (Map<String, Object> d : details) {
            double tax = toDouble(d.get("TaxAmount"));
            if (!(tax > 0d)) continue;
            double pct = toDouble(d.get("TaxPercent"));
            String text = clr(pct) + "Tax %  " + clr(tax);
            ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
            a.AccountId = taxAc;
            a.Comments = text;
            a.AgainstAccountId = supplierGl;
            a.DebitAmount = tax;
            a.TaxesTotalAmount = tax;
            a.TaxPrcnt = pct;
            a.IsTaxable = "True";
            a.TaxTypeId = toInt(d.get("TaxNameId"));
            v.lines.add(a);
            ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
            b.AccountId = supplierGl;
            b.AgainstAccountId = taxAc;
            b.Comments = text;
            b.CreditAmount = tax;
            b.TaxesTotalAmount = tax;
            b.IsTaxable = "True";
            b.TaxPrcnt = pct;
            b.TaxTypeId = toInt(d.get("TaxNameId"));
            v.lines.add(b);
        }
    }

    private static Map<String, Object> itemGl(List<Map<String, Object>> items, int itemId) {
        for (Map<String, Object> x : items) if (toInt(x.get("Id")) == itemId) return x;
        return null;
    }

    // ============================================================================= models

    /** Model 1036 InvPurchaseInvoice — non-virtual properties in declaration order, constructor defaults. */
    private static Map<String, Object> headerModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("IsApproved", false);
        p.put("CustomAccounts", false);
        p.put("DocDate", null);
        p.put("EntryDate", null);
        p.put("ModifyDate", null);
        p.put("DueDate", null);
        p.put("PostDate", null);
        p.put("DeliveryStartDate", null);
        p.put("SupplierInvoiceDate", null);
        p.put("BillAmount", 0d);
        p.put("CashReceived", 0d);
        p.put("CommAmount", 0d);
        p.put("CommRate", 0d);
        p.put("FreightAmount", 0d);
        p.put("TransportAccountId", 0);
        p.put("BranchesId", 0);
        p.put("BranchSrNo", 0);
        p.put("TransporterCreditPartyId", 0);
        p.put("TransporterDebitGLId", 0);
        p.put("TransporterDebitPartyId", 0);
        p.put("FinancialYearId", 0);
        p.put("CommissionAgentId", 0);
        p.put("CommissionCreditAccountId", 0);
        p.put("CompanyId", 0);
        p.put("DocNo", 0);
        p.put("SalesTaxNo", 0);
        p.put("DocumentTypeId", 0);
        p.put("DocumentTypeSrNo", 0);
        p.put("EntryUser", 0);
        p.put("Id", 0);
        p.put("ModifyUser", 0);
        p.put("OrganizationId", 0);
        p.put("PostUser", 0);
        p.put("ProjectsId", 0);
        p.put("TaxAccountId", 0);
        p.put("ReferencePartyId", 0);
        p.put("StockPartyId", 0);
        p.put("SupplierCustomerId", 0);
        p.put("SupplierInvoiceNo", 0);
        p.put("SupplierReferenceNo", null);
        p.put("CurrencyId", 0);
        p.put("ReferencePartyName", null);
        p.put("DueDays", 0);
        p.put("ActionId", 0);
        p.put("CommissionRemarks", null);
        p.put("CommissionType", null);
        p.put("IsAttachments", null);
        p.put("IsTaxable", false);
        p.put("ManualBillNo", null);
        p.put("OtherRemarks", null);
        p.put("RemarksHeader", null);
        p.put("ScreenName", null);
        p.put("UomScheduleIdCmRate", null);
        p.put("DeliveryTerm", null);
        p.put("PaymentTermsId", 0);
        p.put("DiscountAmount", 0d);
        p.put("DiscountAccountId", 0);
        p.put("DeliveryDays", 0);
        p.put("BrokerAgentId", 0);
        p.put("InvoiceTypeId", 0);
        p.put("GhallaMandiId", 0);
        p.put("BillCalculateTypeId", 0);
        p.put("BrokeryType", null);
        p.put("BrokeryRate", 0d);
        p.put("BrokeryUom", 0d);
        p.put("BrokeryAmount", 0d);
        p.put("WagesAmount", 0d);
        p.put("InvoiceQty", BigDecimal.ZERO);
        p.put("InvoiceWeight", BigDecimal.ZERO);
        p.put("ExchangeRate", BigDecimal.ZERO);
        p.put("FcyAmount", BigDecimal.ZERO);
        p.put("AttachmentsValues", null);
        p.put("CustomAttachmentsValues", null);
        p.put("FreightRemark", null);
        p.put("IsUploaded", false);
        return p;
    }

    /** Model 1037 InvPurchaseInvoiceDetail — non-virtual properties in declaration order. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("GpDate", null);
        for (String k : new String[] { "AdLsWeight", "AvgCgsRate", "BillAmount", "CommissionAmount", "EBTotalWt",
                "EbPurAgainstWeight", "EBWeight", "ExpenseAmount", "FreightAmount", "GrossWeight", "WagesAmount",
                "FreightDeduction", "EbPurAgainstWeightAmount", "ScaleKart", "ItemAmount", "ItemCgsRate", "ItemQty",
                "ItemRate", "JournalAmount", "NetBillWeight", "NetStockWeight", "RateCut", "RateCutAmount",
                "ExcessWeight", "RateCutForExcessWeight", "RateCutAmountForExcessWeight", "TaxAmount", "TaxPercent",
                "WeightCut", "WeightCutTotal", "Brokery" }) p.put(k, 0d);
        p.put("ExchangeRate", BigDecimal.ZERO);
        p.put("FcyAmount", BigDecimal.ZERO);
        p.put("ItemAmountWithoutDiscount", BigDecimal.ZERO);
        for (String k : new String[] { "GpNo", "BranchId", "Id", "ActionTypeId", "InvGrnDetailId", "InvGrnId",
                "InvPurchaseInvoiceId", "ItemId", "ItemUOMId", "JobLotId", "PackingTypeId", "PurchaseOrderId",
                "PurchaseOrderDetailId", "TaxNameId", "UomScheduleIdRate", "WarehouseId", "LineId", "CurrencyId",
                "AttributeVariantId", "ItemConditionId", "RackId" }) p.put(k, 0);
        for (String k : new String[] { "CropYear", "IsTaxable", "LabAnalisysNo", "RemarksDetail", "TaxDescriptions",
                "VehicleNo", "PackingDate", "ExpiryDate" }) p.put(k, null);
        p.put("CropYearId", 0);
        p.put("DiscountTypeId", 0);
        p.put("DiscountPercent", 0d);
        p.put("DiscountAmount", 0d);
        for (String k : new String[] { "RefDocumentTypeId", "RefDocId", "RefDocSubId", "RefDocInvoiceId", "CityId",
                "ProductionStageId", "CastingTypeId", "GdnDocumentTypeId", "GdnId", "GdnDetailId", "LocationTypeId" }) p.put(k, 0);
        return p;
    }

    /** Model 1035 InvPurchaseInvoiceFreight. */
    private static Map<String, Object> freightModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("InvPurchaseInvoiceId", 0);
        p.put("Id", 0);
        p.put("FrRate", 0d);
        p.put("FrWeight", 0d);
        p.put("FrQty", 0d);
        p.put("FreightAmount", 0d);
        p.put("Debit", 0d);
        p.put("Percentage", 0d);
        p.put("InvGrnId", 0);
        p.put("FreightId", 0);
        p.put("TansporterId", 0);
        p.put("PurchaseOrderId", 0);
        p.put("SupplierCustomerId", 0);
        p.put("Remarks", null);
        return p;
    }

    /** Model 1038 InvPurchaseInvoiceJournal. */
    private static Map<String, Object> journalModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Id", 0);
        p.put("InvPurchaseInvoiceId", 0);
        p.put("InvGrnId", 0);
        p.put("FreightId", 0);
        p.put("ChartofAccountId", 0);
        p.put("TransporterSupCustId", 0);
        p.put("SupplierCustomerId", 0);
        p.put("JvRemarks", null);
        p.put("JvDebit", 0d);
        p.put("JvCredit", 0d);
        p.put("JvPrcnt", 0d);
        p.put("JvQty", 0d);
        p.put("JvRate", 0d);
        p.put("RowType", 0);
        return p;
    }

    /** Model 1034 InvPurchaseInvoiceExpense (PurchaseOrderId / PurchaseOrderSupplierExpId set to 0, :1588). */
    private static Map<String, Object> expenseModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Id", 0);
        p.put("InvPurchaseInvoiceId", 0);
        p.put("PurchaseOrderId", 0);
        p.put("PurchaseOrderSupplierExpId", 0);
        p.put("GdnId", 0);
        p.put("GdnExpId", 0);
        p.put("InvRevExpItemId", 0);
        p.put("Qty", 0d);
        p.put("Rate", 0d);
        p.put("Amount", 0d);
        p.put("Remarks", null);
        p.put("CustomRemarks", null);
        p.put("GrnId", 0);
        p.put("SupplierDispatchId", 0);
        p.put("SupplierDispatchExpenseId", 0);
        return p;
    }

    /** Drops the page-side "~" helper keys (the model's virtual properties). */
    private static Map<String, Object> sendable(Map<String, Object> m) {
        Map<String, Object> out = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : m.entrySet()) if (!e.getKey().startsWith("~")) out.put(e.getKey(), e.getValue());
        return out;
    }

    /** Public fields in declaration order — ContraVoucherDto mirrors VoucherHead / VoucherDetail. */
    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                if (v != null && ("VoucherDate".equals(f.getName()) || "ChequeDate".equals(f.getName())
                        || "DueDate".equals(f.getName()) || "EntryDate".equals(f.getName())
                        || "ModifyDate".equals(f.getName()) || "PostDate".equals(f.getName())
                        || "DCheqDate".equals(f.getName()) || "GpDate".equals(f.getName()))) {
                    v = Timestamp.valueOf(String.valueOf(v).length() == 10 ? v + " 00:00:00" : String.valueOf(v));
                }
                m.put(f.getName(), v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    // ============================================================================ helpers

    /** The header must exist, be DocumentTypeId 64, and belong to the user's organization and company. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    /**
     * DueDate.Value. The picker carries a time of day (txtDueDays_TextChanged sets
     * DateTime.Now.AddDays); an opened invoice keeps the stored SupplierInvoiceDate while its day
     * is not changed.
     */
    private static Timestamp dueDate(String day, Map<String, Object> existing) {
        if (existing != null && !isBlank(day)) {
            Object stored = ci(existing, "SupplierInvoiceDate");
            Timestamp ts = stored instanceof Timestamp ? (Timestamp) stored
                    : stored instanceof java.time.LocalDateTime ? Timestamp.valueOf((java.time.LocalDateTime) stored) : null;
            if (ts != null && ts.toLocalDateTime().toLocalDate().toString().equals(day.trim().substring(0, 10))) return ts;
        }
        LocalDate d = isBlank(day) ? LocalDate.now() : LocalDate.parse(day.trim().substring(0, 10));
        return Timestamp.valueOf(LocalDateTime.of(d, LocalTime.now().withNano(0)));
    }

    private String financialYearStart(UserAccount u, int yearId) {
        try {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId())) {
                if (toInt(ci(r, "Id")) == yearId) {
                    Object v = ci(r, "Start_Period");
                    return v == null ? null : String.valueOf(v).substring(0, 10);
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the financial year start", e);
        }
        return null;
    }

    /** FormHelper.ValidateField. */
    private static void validate(Object v, String field, int rowIndex) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof Double && (Double) v <= 0d)
                || (v instanceof BigDecimal && ((BigDecimal) v).signum() <= 0)
                || (v instanceof String && ((String) v).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** Conversion.ToInt on a double — Convert.ToInt32(double) rounds half to even. */
    static int cvInt(double v) { return (int) Math.rint(v); }

    /** Conversion.ToInt on text — Convert.ToInt32(string) only accepts an integer; anything else is 0. */
    static int cvIntText(String s) {
        if (s == null) return 0;
        try { return Integer.parseInt(s.trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble on text — thousands separators allowed; anything unparseable is 0. */
    static double cvDouble(String s) {
        double v = parseDouble(s);
        return Double.isNaN(v) ? 0d : v;
    }

    /** double.TryParse — NaN when it fails. */
    static double parseDouble(String s) {
        if (s == null || s.trim().isEmpty()) return Double.NaN;
        try { return Double.parseDouble(s.trim().replace(",", "")); } catch (NumberFormatException e) { return Double.NaN; }
    }

    /** $"{x:0.##}". */
    static String format0dd(double x) {
        BigDecimal b = new BigDecimal(clr(x)).setScale(2, RoundingMode.HALF_UP).stripTrailingZeros();
        return b.signum() == 0 ? "0" : b.toPlainString();
    }

    /** Math.Round(x, places) — MidpointRounding.ToEven. */
    static double roundEven(double x, int places) {
        return BigDecimal.valueOf(x).setScale(places, RoundingMode.HALF_EVEN).doubleValue();
    }

    private static List<Integer> ids(String csv) {
        List<Integer> out = new ArrayList<>();
        if (csv == null) return out;
        for (String p : csv.split(",")) {
            String t = p.trim();
            if (t.isEmpty()) continue;
            try { out.add(Integer.parseInt(t)); } catch (NumberFormatException ignored) { }
        }
        return out;
    }

    private static String csv(List<Integer> ids, Set<Integer> allowed) {
        StringBuilder sb = new StringBuilder();
        for (Integer id : ids) {
            if (!allowed.contains(id)) continue;
            if (sb.length() > 0) sb.append(",");
            sb.append(id);
        }
        return sb.toString();
    }

    private static boolean contains(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return true;
        return false;
    }

    /**
     * Date values leave as local "yyyy-MM-dd HH:mm:ss" text, never as a JSON timestamp — a midnight
     * DATE must not move to the previous day in a UTC rendering.
     */
    static Object plain(Object v) {
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + " 00:00:00";
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDate) return v.toString() + " 00:00:00";
        if (v instanceof java.time.OffsetDateTime) return ((java.time.OffsetDateTime) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        return v;
    }

    static List<Map<String, Object>> plainRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) o.put(e.getKey(), plain(e.getValue()));
            out.add(o);
        }
        return out;
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
    private static String trim(String s) { return s == null ? "" : s.trim(); }
    private static boolean toBool(String v) {
        if (v == null) return false;
        String t = v.trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }

    private static Map<String, Object> ok(String message, int id, int docNo) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        m.put("docNo", docNo);
        return m;
    }
}
