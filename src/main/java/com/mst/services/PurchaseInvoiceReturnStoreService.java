package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.PurchaseInvoiceReturnStoreDto;
import com.mst.repositories.PurchaseInvoiceReturnStoreRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.LoginContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.context.request.RequestAttributes;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Store Purchase (ModuleId 67) — screen 346 "Purchase Invoice Return Store".
 *
 * <ul>
 *   <li>Desktop form: Architecture.WinApp.StoreManagement/PurchaseInvoiceReturn_Store.cs (8,161 lines; logic
 *       :456-5141, designer :5152-). The same class also runs as ScreenName "PurchaseInvoiceReturnPM"
 *       (BaseDocumentTypeId 2, party list "245,702", Load Purchase Invoice hidden — Form_Load:470); only the
 *       {@code PurchaseInvoiceReturn_Store} path is ported here.</li>
 *   <li>ScreenName (base.Tag, and base.Name): {@code PurchaseInvoiceReturn_Store} — rights key and the ScreenName
 *       saved on the header (Insert:2880) and sent to FormHistory (:3396).</li>
 *   <li>DocumentTypeId: <b>145</b> (DocumentNo:680, Insert:2858, history :3346, delete :3154, voucher :756).
 *       144 is the GDN type its (hidden) GDN loader would read (btnLoadGdn_Click:4249); 2 is the
 *       BaseDocumentTypeId of the PM variant; this screen's BaseDocumentTypeId is 1 (Form_Load:468).</li>
 *   <li>Route /store/purchase-invoice-return-store, API /api/store/purchase-invoice-return-store.</li>
 *   <li>Loaders: frmPendingPurchaseInvoiceForReturn.cs (ported), frmDefineReasons.cs (Architecture.WinApp.Lookups,
 *       ported), frmLoadGDN_Store.cs (NOT reachable — btnLoadGdn is hidden in the designer and again at
 *       Form_Load:482; not ported).</li>
 *   <li>BLL/DAL/Models: BLL 0580 / DAL 0433 InvSaleInvoice (Save, GetByID, GenerateCode, FormHistory,
 *       AllComboBindAgainstSaleInvoice), BLL 0612 SaleInvoiceFinancial + 0613 SaleInvoiceGeneralFinancialMethods
 *       (the voucher), BLL 0581 / DAL 0434 InvPurchaseInvoice (parties/items, pending loader, branches,
 *       RemoveByID), BLL 0267 CommonServies, 0594 jobLot, 0006 / DAL 0013 / Model 0012 Reasons, 0070 DocumentType,
 *       0076 MultiCurrency, 0592 ItemTaxSchedule, 0573 InventoryItemsOther, 0574 InventoryStockEvalautionDetail,
 *       0610 UOMSchedule, 0600 SupplierCustomer, 0648 COAAllocation, 0654 VoucherHead, 0379 GlobalServicesMethods,
 *       0128 InvSaleInvoiceReports, 0141 VoucherReports; Models 1031 InvSaleInvoice, 1039 InvSaleInvoiceDetail,
 *       1040/1041/1042 Expense/Freight/Journal, 1195/1191 VoucherHead/VoucherDetail (as ContraVoucherDto).</li>
 * </ul>
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * ---------------------------------------------------------------------------------------------
 *  1. The document is a PURCHASE return saved through the SALE invoice BLL/DAL (InvSaleInvoice) but deleted
 *     through InvPurchaseInvoice.RemoveByID (btnDelete_Click:3150) — both end in Sp_InvoicesVouchersandStocksDelete.
 *  2. Sp_InvSaleInvoice_Insert recomputes DocNo itself; the success message and the voucher's VoucherCode use the
 *     number the form generated (Insert:2998, BLL 0612) — they differ if another user saved in between.
 *  3. DueDays is saved twice: DueDays and SupplierInvoiceNo both take txtduedays (Insert:2869-2870);
 *     SupplierInvoiceDate is DateTime.Now (:2884). ReadById shows due days from SupplierInvoiceNo and the due
 *     DATE from SupplierInvoiceDate (the save timestamp), not DueDate (ReadById:3054-3055).
 *  4. DAL 0433:78 overwrites every detail's BillAmount with ItemAmount + ExpenseAmount - CommissionAmount -
 *     FreightAmount before inserting — the grid's "Item Net Amount" (which includes tax) is not what is stored.
 *  5. Freight rows save FreightAmount as Conversion.ToInt(Freight) (banker's rounding to a whole number) and
 *     never save the grid's Debit column (Insert:2985); expense Qty is likewise Conversion.ToInt (:2936).
 *  6. On a return loaded from a purchase invoice, rows with Return Qty <= 0 are skipped (Insert:2899) but the
 *     voucher lines keep the form's LineId (RowIndex + 1) while DAL 0433 renumbers the details 1..n, so a
 *     voucher line after a skipped row points its RefDocSubIdNo at the wrong detail (or none).
 *  7. Voucher (BLL 0613, DocumentTypeId 145): Dr party / Cr item Purchase GL (or the job lot's account) per row;
 *     when CGSEntryAllow / SaleCostingJobOrderWise / (feature 5 and a loaded row) apply, a "Stock Difference"
 *     pair of (ItemAmount - AvgRate x Qty) is added, and when it is negative the NEGATIVE amount is written as
 *     the Credit of the first line and the Debit of the second (:1072-1096). Freight adds a DEBIT-only line per
 *     row (Purchase GL, :1150) plus the transporter's credit line (FreightFinancial); tax Dr party / Cr Tax
 *     Account; Party Add/Less rows post both sides.
 *  8. FormHistory sends @EntryUser only when the user lacks "CanView AllRecord", and the form never sets it —
 *     so it is 0 and such a user sees no history (:3347; BLL 0580 FormHistory).
 *  9. Reset (New) does not hide Delete; after opening a document and pressing New, Delete answers
 *     "RecordId Not Found....." (btnDelete_Click:3163). Reset keeps Doc Date, Payment/Delivery Term, currency,
 *     tax type and the GP/Vehicle/City boxes unless "Reset On Add and Save" is ticked.
 * 10. "Update" of an entry-bar row writes Id 0 into the row (FillDetailRow:1412); harmless because
 *     Sp_InvSaleInvoice_Update deletes and re-inserts every detail.
 * 11. ReadById does not restore the detail Remarks (FillDetailFromListCommonForReadById:3175), so an update
 *     saves them empty; the detail read returns UOMDescription as the UOM text.
 * 12. The freight grid's "+" adds (0, Transporter, 0, Remarks) into (Id, GdnId, Transporter, GlAccountId)
 *     (grdFreight_ColumnButtonClick:2294) — the transporter lands in GdnId and a non-numeric Remarks fails the
 *     row with the DataTable's own error; Ctrl+D adds a proper blank row.
 * 13. Tax type change in the entry bar keeps an already typed Tax % (CalculateTaxAmountandTotalAmount:3913).
 * 14. The history detail grid is laid out with the CURRENT form's flags (captions "Return Qty"/"Item Qty",
 *     BalQty/Reason visibility) — grddetailhistorySettings:3618 calls grdSettings.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web-only, with reason)
 * ---------------------------------------------------------------------------------------------
 *  D1. Doc No is read-only and never trusted: generated on insert, the stored header's on update.
 *  D2. Open / update / delete / print only when the document is DocumentTypeId 145 of the signed-in company;
 *      org/company/branch/FY/user always come from the session. Rights are checked server-side.
 *  D3. A saved row that references a GDN (InvGdnId > 0) is refused: that path (DAL 0433 GetReferenceDocumentTypeId
 *      144 + USP_InventoryStockEvalautionDetailGdnReferences) is reachable on the desktop only through the hidden
 *      Load Gdn button, and is not ported.
 *  D4. frmDefineReasons has no rights check on the desktop; here Save/Update of a reason needs this screen's
 *      Save right (it writes master data shared by every document type).
 *  D5. A header save whose procedure returns no id is refused instead of writing orphan details.
 *  D6. Crystal layouts (145 / 145A / 103) are out of scope: the print endpoints return the procedures' rows.
 *  D7. The purchase-invoice reference of every row is re-read on the server (the desktop trusts its own grid,
 *      filled by the loader): all three ids set or none; referenced and direct rows are not mixed; the invoice
 *      must belong to the session organization/company, be DocumentTypeId 61/64/131 (the loader's filter) and
 *      equal the row's RefDocumentTypeId; the detail must belong to that invoice and carry the row's item; the
 *      invoice's party must be the header party; Return Qty must not exceed the line's quantity less what every
 *      OTHER return (InvSaleInvoiceDetail referencing it, this document excluded) has already taken.
 *  D8. Ids are checked against the lists the page is served: Tax Account (GetTaxAccountsFromGlobal), Party
 *      Add/Less and Transporter accounts (AccountsFill), Other Items, Job Lot. 0 is accepted where the desktop
 *      accepts an empty cell (Tax Account without tax, rows the save skips). On update, an id already stored on
 *      this document is also accepted, so a document whose account later left the list can still be updated.
 *
 * NOT PORTED: attachments (btnAttachment, AddAttachment, NoOfAttachments link), saved grid layouts
 * (ctrlGrdBar1-3), ShortCut Keys popup and the form-level keyboard shortcuts, frmLoadGDN_Store (hidden).
 */
@Service
public class PurchaseInvoiceReturnStoreService {

    private static final Logger LOG = LoggerFactory.getLogger(PurchaseInvoiceReturnStoreService.class);

    public static final int DOC_TYPE = 145;
    public static final String SCREEN = "PurchaseInvoiceReturn_Store";
    private static final int BASE_DOC_TYPE = 1;                                   // Form_Load:468
    private static final String PARTY_DOC_TYPES = "58,61,63,64,131,1604,1603";    // GetPartiesAndItem:768

    private final PurchaseInvoiceReturnStoreRepository repo;
    private final StoreIssuanceRepository common;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final JdbcTemplate jdbc;

    public PurchaseInvoiceReturnStoreService(PurchaseInvoiceReturnStoreRepository repo, StoreIssuanceRepository common,
                                             StoreScreenRights rights, CurrentUserContext ctx, JdbcTemplate jdbc) {
        this.repo = repo;
        this.common = common;
        this.rights = rights;
        this.ctx = ctx;
        this.jdbc = jdbc;
    }

    // ================================================================================= load

    /** Form_Load:456 — everything the form binds, plus the configuration the page's calculations need. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        Set<Integer> f = repo.features(u);
        boolean subsidiary = f.contains(4);                                     // :464
        boolean multiCurrency = f.contains(6);                                  // MultiCurrencyFeature:1240
        boolean freightDebitToExpenses = bool(repo.configRaw(u, "DebitAmountChargetoExpenseAcFreightGridPurchase"));

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("subsidiary", subsidiary);
        out.put("multiCurrency", multiCurrency);
        out.put("formats", formats(u));
        /* GetConfigurationsFromGlobal:601 */
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(repo.configRaw(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("itemSearchByCode", bool(repo.configRaw(u, "ItemSearchByCode")));
        out.put("taxPercentEditable", bool(repo.configRaw(u, "TaxPercentEditable")));
        out.put("freightDebitToExpenses", freightDebitToExpenses);
        /* GetConfigurationsFromGlobalandBind:619 */
        out.put("defaultWarehouseId", toInt(repo.configRaw(u, "Warehouse")));
        out.put("defaultCityId", toInt(repo.configRaw(u, "City Area")));
        out.put("defaultJobLotId", toInt(repo.configRaw(u, "Job/Lot")));
        out.put("baseCurrency", toInt(repo.configRaw(u, "Base Currency")));
        out.put("baseCurrencyRate", toDouble(repo.configRaw(u, "BaseCurrencyRate")));
        /* GridItemNetAmountCalculation:3848 — null when the company has no row for it. */
        out.put("creditAmountInItemSaleGL", repo.configRaw(u, "CreditAmountInItemSaleGL"));
        /* FormHelper.ResolveWarehouseRack defaults (the PI loader). */
        out.put("defaultWarehouseForStoreFlow", toInt(repo.configRaw(u, "DefaultWarehouseForStoreFlow")));
        out.put("packingMaterialDefaultWarehouse", toInt(repo.configRaw(u, "PackingMaterialDefaultWarehouse")));

        out.put("docNo", repo.generateCode(u, fy, DOC_TYPE));                  // DocumentNo:663
        out.putAll(combos(u, subsidiary, freightDebitToExpenses));
        return out;
    }

    /** Every combo / value list the form fills; also btnRefresh_Click:2585. */
    private Map<String, Object> combos(UserAccount u, boolean subsidiary, boolean freightDebitToExpenses) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("currencies", PurchaseInvoiceReturnStoreRepository.project(repo.currencies(u), "Id", "CurrencyCode"));   // CurrencyFill:1198
        out.put("partiesAndItems", PurchaseInvoiceReturnStoreRepository.project(repo.partiesAndItems(u, PARTY_DOC_TYPES),
                "SupplierCustomerId", "CompanyName", "GlAccountId", "ItemId", "ItemName", "ItemCodeNew"));
        List<Map<String, Object>> terms = new ArrayList<>();                                                            // PaymentTermBindFromGlobal:721
        for (Map<String, Object> r : repo.paymentTerms(u)) terms.add(row("Id", toInt(ci(r, "Id")), "Description", str(ci(r, "TermsDescription"))));
        out.put("paymentTerms", terms);
        out.put("taxAccounts", taxAccounts(u));                                                                        // BindTaxAccountFromGlobal:871
        List<Map<String, Object>> taxes = new ArrayList<>();                                                            // GridCombdtFill:1043 (TaxTypeId 2, DocDate.Value)
        for (Map<String, Object> r : repo.taxSchedule(u, Timestamp.valueOf(LocalDateTime.now().withNano(0)), 2)) {
            taxes.add(row("TaxNameId", toInt(ci(r, "TaxNameId")), "TaxName", str(ci(r, "TaxName")), "TaxPercent", toDouble(ci(r, "TaxPercent"))));
        }
        out.put("taxTypes", taxes);
        List<Map<String, Object>> conds = new ArrayList<>();                                                            // ItemConditionBindFromGlobal:1001
        for (Map<String, Object> r : repo.itemConditions()) conds.add(row("Id", toInt(ci(r, "Id")), "Description", str(ci(r, "ConditionStatus"))));
        out.put("itemConditions", conds);
        List<Map<String, Object>> lots = new ArrayList<>();                                                             // JoblotBindFromGlobal:1016
        for (Map<String, Object> r : repo.jobLots(u)) lots.add(row("Id", toInt(ci(r, "Id")), "Description", str(ci(r, "JobLotDescription"))));
        out.put("jobLots", lots);
        List<Map<String, Object>> cities = new ArrayList<>();                                                           // CityFillFromGlobal:1085
        for (Map<String, Object> r : repo.cities(u)) cities.add(row("Id", toInt(ci(r, "Id")), "Description", str(ci(r, "CityName"))));
        out.put("cities", cities);
        out.put("racks", common.racksWithWarehouseAndItem(u, branch(u)));                                              // racksWithWarehouseAndItems
        List<Map<String, Object>> uoms = new ArrayList<>();                                                             // globalUomSchedule
        for (Map<String, Object> r : repo.uoms(u)) {
            uoms.add(row("Id", toInt(ci(r, "Id")), "ItemId", toInt(ci(r, "ItemId")), "UOMCode", str(ci(r, "UOMCode")),
                    "Equivalent", toDouble(ci(r, "Equivalent")), "BaseRateUom", bool(ci(r, "BaseRateUom")), "BasePackUom", bool(ci(r, "BasePackUom"))));
        }
        out.put("uoms", uoms);
        out.put("otherItems", PurchaseInvoiceReturnStoreRepository.project(repo.otherItems(u), "Id", "OtherItemName")); // GridCombdtFill:1040
        out.putAll(accounts(u, subsidiary, freightDebitToExpenses));                                                  // AccountsFill:777
        out.put("reasons", reasons());                                                                                 // DetailGridComboBind:1624
        out.put("historyCustomers", historyCustomers(u));                                                              // HistoryCombosFill:3273
        return out;
    }

    /** DocumentNo:663 — Reset() generates the next number again. */
    public Map<String, Object> nextDocNo() {
        UserAccount u = ctx.requireAccountingUser();
        return row("docNo", repo.generateCode(u, ctx.currentFinancialYearId(), DOC_TYPE));
    }

    /** btnRefresh_Click:2585 — the combos again (the page keeps each selection, BindAndRetainSelection). */
    public Map<String, Object> refresh() {
        UserAccount u = ctx.requireAccountingUser();
        Set<Integer> f = repo.features(u);
        return combos(u, f.contains(4), bool(repo.configRaw(u, "DebitAmountChargetoExpenseAcFreightGridPurchase")));
    }

    /** DatatableHelper.GetTaxAccountsFromGlobal — types {3,6,8,16,17,18,19}, distinct by ChartOfAccountId, first wins. */
    private List<Map<String, Object>> taxAccounts(UserAccount u) {
        Set<Integer> types = new HashSet<>(java.util.Arrays.asList(3, 6, 8, 16, 17, 18, 19));
        Set<Integer> seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> a : repo.accountsWithCustomGroup(u)) {
            if (!types.contains(toInt(ci(a, "AccountTypeId")))) continue;
            int id = toInt(ci(a, "ChartOfAccountId"));
            if (!seen.add(id)) continue;
            out.add(row("Id", id, "AccountTitle", str(ci(a, "AccountTitle")), "AccountCode", str(ci(a, "AccountCode")),
                    "ParentAccountTitle", str(ci(a, "ParentAccountTitle"))));
        }
        return out;
    }

    /**
     * AccountsFill:777 — the Party Add/Less account list and the freight Transporter list.
     * Rows are {Id = GL account, SupplierCustomerId, AccountTitle}, exactly as dtAccountlst holds them.
     */
    private Map<String, Object> accounts(UserAccount u, boolean subsidiary, boolean freightDebitToExpenses) {
        List<Map<String, Object>> list = new ArrayList<>();
        List<Map<String, Object>> freight = new ArrayList<>();
        int appId = u.getAppId() == null ? 0 : u.getAppId();
        if (subsidiary) {
            for (Map<String, Object> r : repo.vendorsAndCustomers(u, 1)) {
                list.add(row("Id", toInt(ci(r, "GlAccountId")), "SupplierCustomerId", toInt(ci(r, "Id")), "AccountTitle", str(ci(r, "CompanyName"))));
            }
            freight = list;                                                    // :792
        } else {
            for (Map<String, Object> r : repo.accountsByTypeIds(u, appId, null, "2,11,12,,13,14,15,20,21,22")) {
                list.add(row("Id", toInt(ci(r, "Id")), "SupplierCustomerId", 0, "AccountTitle", str(ci(r, "AccountTitle"))));
            }
            if (!freightDebitToExpenses) {
                freight = list;                                                // :810
            } else {
                for (Map<String, Object> r : repo.accountsByTypeIds(u, appId, null, "2,15,22")) {
                    freight.add(row("Id", toInt(ci(r, "Id")), "SupplierCustomerId", 0, "AccountTitle", str(ci(r, "AccountTitle"))));
                }
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("accounts", list);
        out.put("freightAccounts", freight);
        return out;
    }

    /** Reasons.GetByRefDocumentType(145) — re-read by the desktop on every BindGrid (DetailGridComboBind:1624). */
    public List<Map<String, Object>> reasons() {
        ctx.requireAccountingUser();
        return PurchaseInvoiceReturnStoreRepository.project(repo.reasonsByRefDocumentType(DOC_TYPE), "ReasonId", "Reason");
    }

    /** HistoryCombosFill:3273 — the rows whose Activity is "Supplier". Also btnRefreshHistory_Click. */
    public List<Map<String, Object>> historyCustomers() { return historyCustomers(ctx.requireAccountingUser()); }

    private List<Map<String, Object>> historyCustomers(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        int appId = u.getAppId() == null ? 0 : u.getAppId();
        for (Map<String, Object> r : repo.historyCombos(u, appId, u.getId(), String.valueOf(DOC_TYPE))) {
            if ("Supplier".equals(str(ci(r, "Activity")))) out.add(row("Id", ci(r, "Id"), "Customer", ci(r, "ReferenceName")));
        }
        return out;
    }

    /** clsGlobalVariables number formats (CommonServices.GetDecimalConfiguration:5376). */
    private Map<String, Object> formats(UserAccount u) {
        int amountRaw = toInt(repo.configRaw(u, "Default NoofDecimal Points For Amount"));
        int rateRaw = toInt(repo.configRaw(u, "Default NoofDecimal Points For Rate"));
        int fcyRaw = toInt(repo.configRaw(u, "DefaultNoOfDecimalPointsForFcyAmount"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("amountRound", amountRaw);                                        // DefaultNoofDecimalPointsForAmount (Math.Round digits)
        m.put("amountDecimals", amountRaw >= 1 && amountRaw <= 4 ? amountRaw : 0);   // stringFormatsingle "#,##0." + N
        m.put("rateDecimals", rateRaw >= 1 && rateRaw <= 4 ? rateRaw : (rateRaw == 0 ? 2 : 0)); // DecimalRateFormate
        m.put("fcyDecimals", fcyRaw >= 1 && fcyRaw <= 4 ? fcyRaw : 0);         // stringFormatsingleForFcy
        return m;
    }

    // ================================================================================= entry bar

    /** AvailableStockGetByItem:1158 — lblBalance: CurrStock.ToString() when positive, else "0". */
    public Map<String, Object> stock(int recId, String docDate, int itemId, int warehouseId, int jobLotId, int uomId) {
        UserAccount u = ctx.requireAccountingUser();
        double s = repo.currentStock(u, itemId, StoreIssuanceService.formDate(docDate, recId), warehouseId, jobLotId, uomId);
        return row("balance", s > 0d ? clr(s) : "0");
    }

    /** cmbCurrency_Leave:1266 — the last exchange rate used on a 145 voucher for that currency, or null. */
    public Map<String, Object> lastExchangeRate(int currencyId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> r = repo.lastExchangeRate(u, String.valueOf(DOC_TYPE), String.valueOf(currencyId));
        return row("found", !r.isEmpty(), "rate", r.isEmpty() ? 0d : toDouble(ci(r.get(0), "LastExchRate")));
    }

    // ================================================================================= loader

    /** frmPendingPurchaseInvoiceForReturn Load:102 — FromDate = ActiveYr.Start_Period; the branch combo. */
    public Map<String, Object> loaderInit() {
        UserAccount u = ctx.requireAccountingUser();
        boolean branchWise = bool(repo.configRaw(u, "PurchaseInvoiceReturnBranchWise"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("branchImplemented", branchWise);
        out.put("branches", loaderBranches(u, branchWise));
        out.put("userBranchName", branchName(u));
        out.put("fromDate", financialYearStart(u, ctx.currentFinancialYearId()));
        return out;
    }

    private List<Map<String, Object>> loaderBranches(UserAccount u, boolean branchWise) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (branchWise) {                                                       // BranchesFill:120
            out.add(row("BranchId", branch(u), "BranchName", branchName(u)));
        } else {
            for (Map<String, Object> r : repo.loaderBranches(u)) out.add(row("BranchId", toInt(ci(r, "BranchId")), "BranchName", str(ci(r, "BranchName"))));
        }
        return out;
    }

    /**
     * PendingGdnLoad:182 — BranchesIds is built as ",id,id" from the ticked branch names; an empty branch
     * text is "Select branch first". Only branches of this user's own list are accepted (D2). Each row gets the
     * item's InventoryParentCategoriesId so the page can run FormHelper.ResolveWarehouseRack as the desktop does.
     */
    public List<Map<String, Object>> pendingInvoices(String from, String to, List<Integer> branchIds) {
        UserAccount u = ctx.requireAccountingUser();
        if (branchIds == null || branchIds.isEmpty()) throw new IllegalArgumentException("Select branch first");
        boolean branchWise = bool(repo.configRaw(u, "PurchaseInvoiceReturnBranchWise"));
        Set<Integer> allowed = new HashSet<>();
        for (Map<String, Object> b : loaderBranches(u, branchWise)) allowed.add(toInt(b.get("BranchId")));
        StringBuilder ids = new StringBuilder();
        for (Integer id : branchIds) if (id != null && allowed.contains(id)) ids.append(',').append(id);
        List<Map<String, Object>> rows = repo.pendingInvoices(u, ctx.currentFinancialYearId(),
                isBlank(from) ? null : StoreIssuanceService.dateOnly(from),
                isBlank(to) ? null : StoreIssuanceService.dateOnly(to), ids.toString());
        Map<Integer, Integer> parents = rows.isEmpty() ? new java.util.HashMap<>() : repo.itemParentCategories(u);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = tsAll(r);
            Integer p = parents.get(toInt(ci(r, "ItemId")));
            o.put("ParentCategoryId", p == null ? 0 : p);
            out.add(o);
        }
        return out;
    }

    // ================================================================================= history

    /**
     * FillHistoryGrid:3337. dateMode is the ticked radio: "doc" (FromDate/ToDate), "entry", "modify",
     * "approved"; a date whose checkbox is cleared is not sent.
     */
    public List<Map<String, Object>> history(String dateMode, String from, String to, int fromDocNo, int toDocNo,
                                             int supplierCustomerId) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        String[] keys;
        switch (dateMode == null ? "doc" : dateMode) {
            case "entry":    keys = new String[] { "EntryFromDate", "EntryToDate" }; break;
            case "modify":   keys = new String[] { "ModifyFromDate", "ModifyToDate" }; break;
            case "approved": keys = new String[] { "ApprovedFromDate", "ApprovedToDate" }; break;
            default:         keys = new String[] { "FromDate", "ToDate" }; break;
        }
        Map<String, Timestamp> dates = new LinkedHashMap<>();
        if (!isBlank(from)) dates.put(keys[0], StoreIssuanceService.pickerDate(from));
        if (!isBlank(to)) dates.put(keys[1], StoreIssuanceService.pickerDate(to));
        /* Note 8 — PI.EntryUser is never assigned on the desktop: 0. */
        List<Map<String, Object>> rows = repo.formHistory(u, DOC_TYPE, viewAll, ctx.currentFinancialYearId(), dates,
                fromDocNo, toDocNo, supplierCustomerId, 0, SCREEN);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("VoucherHeadId", ci(r, "VoucherHeadId"));
            o.put("DocNo", ci(r, "DocNo"));
            o.put("DocDate", dayOr1900(ci(r, "DocDate")));                     // ToShortDateString
            o.put("ManualBillNo", ci(r, "ManualBillNo"));
            o.put("SupplierCustomerId", ci(r, "SupplierCustomerId"));
            o.put("CustomerName", ci(r, "CustomerName"));
            o.put("PaymentTerm", ci(r, "PaymentTerm"));
            o.put("DueDate", dayOr1900(ci(r, "DueDate")));
            o.put("BillAmount", toDouble(ci(r, "BillAmount")));
            o.put("EntryUser", ci(r, "UserName"));
            o.put("EntryDate", ts(ci(r, "EntryDate")));
            o.put("ModifyUser", ci(r, "ModifyUserName"));
            o.put("ModifyDate", ts(ci(r, "ModifyDate")));
            o.put("Remarks", ci(r, "RemarksHeader"));
            o.put("NoOfAttachments", ci(r, "NoOfAttachments"));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= read

    /** ReadById:3038 (and GetDetailGrdByHeadId:3591) — header, grids, voucher id. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        boolean subsidiary = repo.features(u).contains(4);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("DocDate", ts(ci(h, "DocDate")));
        out.put("SupplierCustomerId", toInt(ci(h, "SupplierCustomerId")));
        out.put("SupplierReferenceNo", str(ci(h, "SupplierReferenceNo")));
        out.put("ManualBillNo", str(ci(h, "ManualBillNo")));
        out.put("PaymentTermId", toInt(ci(h, "PaymentTermId")));
        out.put("SupplierInvoiceNo", toInt(ci(h, "SupplierInvoiceNo")));        // → txtduedays
        out.put("SupplierInvoiceDate", ts(ci(h, "SupplierInvoiceDate")));      // → duedate
        out.put("DeliveryTerm", str(ci(h, "DeliveryTerm")));
        out.put("OtherRemarks", str(ci(h, "OtherRemarks")));
        out.put("InvoiceQty", toDouble(ci(h, "InvoiceQty")));
        out.put("FcyAmount", toDouble(ci(h, "FcyAmount")));
        out.put("TaxAccountId", toInt(ci(h, "TaxAccountId")));
        out.put("BillAmount", toDouble(ci(h, "BillAmount")));
        out.put("IsApproved", bool(ci(h, "IsApproved")));
        out.put("CurrencyId", toInt(ci(h, "CurrencyId")));
        Object ex = ci(h, "ExchangeRate");
        out.put("ExchangeRateText", ex instanceof BigDecimal ? ((BigDecimal) ex).toPlainString() : str(ex));   // Conversion.ToString(decimal)
        out.put("voucherHeadId", common.voucherHeadId(u, DOC_TYPE, id));       // VoucherHeadIdGet:752

        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id, toInt(ci(h, "DocumentTypeId")))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("InvGdnId", toInt(ci(d, "InvGdnId")));
            o.put("InvGdnDetailId", toInt(ci(d, "InvGdnDetailId")));
            o.put("GdnNo", toInt(ci(d, "GdnNo")));
            o.put("RefDocumentTypeId", toInt(ci(d, "RefRefDocumentTypeId")));
            o.put("RefDocId", toInt(ci(d, "RefRefDocIdNo")));
            o.put("RefDocSubId", toInt(ci(d, "RefDocSubId")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("Item", str(ci(d, "ItemName")));
            o.put("ItemCode", str(ci(d, "ItemCode")));
            o.put("WarehouseId", toInt(ci(d, "WarehouseId")));
            o.put("Warehouse", str(ci(d, "WareHouseName")));
            o.put("RackId", toInt(ci(d, "RackId")));
            o.put("RackName", str(ci(d, "rackName")));
            o.put("ItemConditionId", toInt(ci(d, "ItemConditionId")));
            o.put("ItemCondition", str(ci(d, "ItemCondition")));
            o.put("JobLotId", toInt(ci(d, "JobLotId")));
            o.put("JobLot", str(ci(d, "JobLotDescription")));
            o.put("PackUOMId", toInt(ci(d, "ItemUOMId")));
            o.put("PackUOM", str(ci(d, "UOMCodeItem")));
            o.put("PackEquivalent", toDouble(ci(d, "PackEquivalent")));
            o.put("ItemQty", toDouble(ci(d, "ItemQty")));
            o.put("BalQty", toDouble(ci(d, "PurchaseBalanceQty")));
            o.put("ReasonId", toInt(ci(d, "ReasonId")));
            o.put("Rate", toDouble(ci(d, "ItemRate")));
            o.put("ItemAmount", toDouble(ci(d, "ItemAmount")));
            o.put("FcyAmount", toDouble(ci(d, "FcyAmount")));
            o.put("TaxTypeId", toInt(ci(d, "TaxNameId")));
            o.put("TaxPercent", toDouble(ci(d, "TaxPercent")));
            o.put("TaxAmount", toDouble(ci(d, "TaxAmount")));
            o.put("BillAmount", toDouble(ci(d, "BillAmount")));
            o.put("Expense", toDouble(ci(d, "ExpenseAmount")));
            o.put("Journal", toDouble(ci(d, "JournalAmount")));
            o.put("Freight", toDouble(ci(d, "FreightAmount")));
            Object gp = ts(ci(d, "GpDate"));
            o.put("GpDate", gp == null ? null : String.valueOf(gp).substring(0, Math.min(10, String.valueOf(gp).length())));
            o.put("GpNo", toInt(ci(d, "GpNo")));
            o.put("VehicleNo", str(ci(d, "VehicleNo")));
            o.put("CityId", toInt(ci(d, "CityId")));
            o.put("CityName", str(ci(d, "CityName")));
            o.put("Remarks", "");                                              // not restored (note 11)
            o.put("TaxType", str(ci(d, "TaxDescriptions")));                   // history detail ExtraColumns
            rows.add(o);
        }
        out.put("rows", rows);

        List<Map<String, Object>> exps = new ArrayList<>();                    // :3078
        for (Map<String, Object> e : repo.expenses(id)) {
            exps.add(row("Id", toInt(ci(e, "Id")), "ItemId", toInt(ci(e, "InvRevExpItemId")), "Qty", toDouble(ci(e, "Qty")),
                    "Rate", toDouble(ci(e, "Rate")), "Amount", toDouble(ci(e, "Amount")), "Remarks", str(ci(e, "Remarks"))));
        }
        out.put("expenses", exps);
        List<Map<String, Object>> jvs = new ArrayList<>();                     // :3090
        for (Map<String, Object> j : repo.journals(id)) {
            jvs.add(row("AccountId", subsidiary ? toInt(ci(j, "TransporterSupCustId")) : toInt(ci(j, "ChartofAccountId")),
                    "GlAccountId", toInt(ci(j, "ChartofAccountId")), "Remarks", str(ci(j, "JvRemarks")),
                    "Percentage", toDouble(ci(j, "JvPrcnt")), "Qty", toDouble(ci(j, "JvQty")), "Rate", toDouble(ci(j, "JvRate")),
                    "Debit", toDouble(ci(j, "JvDebit")), "Credit", toDouble(ci(j, "JvCredit"))));
        }
        out.put("journals", jvs);
        List<Map<String, Object>> frs = new ArrayList<>();                     // :3102
        for (Map<String, Object> r : repo.freights(id)) {
            frs.add(row("Id", toInt(ci(r, "Id")), "GdnId", toInt(ci(r, "InvGdnId")),
                    "Transporter", subsidiary ? toInt(ci(r, "TransporterSupCustId")) : toInt(ci(r, "TansporterId")),
                    "GlAccountId", toInt(ci(r, "TansporterId")), "Freight", toDouble(ci(r, "FreightAmount")),
                    "Debit", toDouble(ci(r, "Debit")), "Remarks", ci(r, "Remarks") == null ? null : str(ci(r, "Remarks"))));
        }
        out.put("freights", frs);
        return out;
    }

    // ================================================================================= print

    /** 145 / 145A slips — both read USP_PurchaseInvoiceReturn_StoreSip (the Crystal layout is out of scope). */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id <= 0 || ownedHeader(u, id) == null) return null;
        return tsRows(repo.slip(u, branch(u), ctx.currentFinancialYearId(), id));
    }

    /** The slips' sub-report rows (InvRptPurchaseBillSupplierOthers). */
    public List<Map<String, Object>> slipSubReport(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id <= 0 || ownedHeader(u, id) == null) return null;
        return tsRows(repo.slipSubReport(id));
    }

    /** 103-Voucher — AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 145); the procedure is company-scoped. */
    public List<Map<String, Object>> voucherSlip(int voucherHeadId) {
        UserAccount u = ctx.requireAccountingUser();
        if (voucherHeadId == 0) return null;                                  // "No Record Found For Display"
        return tsRows(repo.voucherSlip(u, voucherHeadId, DOC_TYPE));
    }

    // ================================================================================= reasons (frmDefineReasons)

    public Map<String, Object> reasonsDialog() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> types = new ArrayList<>();                   // DocumentTypeFill:82
        for (Map<String, Object> r : repo.documentTypes()) {
            types.add(row("Id", toInt(ci(r, "Id")), "Description", str(ci(r, "DocumentTypeDescription")), "Code", str(ci(r, "DocumentTypeCode"))));
        }
        out.put("documentTypes", types);
        out.put("history", reasonsHistory());
        return out;
    }

    /** frmDefineReasons.FormHistory:144. */
    public List<Map<String, Object>> reasonsHistory() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.reasonsHistory()) {
            out.add(row("ReasonId", ci(r, "ReasonId"), "RefDocumentTypeId", ci(r, "RefDocumentTypeId"),
                    "RefDocumentType", ci(r, "RefDocumentType"), "ReasonName", ci(r, "Reason"),
                    "EntryDate", ts(ci(r, "EntryDate")), "EntryUser", ci(r, "EntryUserName"),
                    "ModifyDate", ts(ci(r, "ModifyDate")), "ModifyUser", ci(r, "ModifyUserName")));
        }
        return out;
    }

    public Map<String, Object> reason(int id) {
        ctx.requireAccountingUser();
        Map<String, Object> r = repo.reasonById(id);
        if (r == null) return null;
        return row("ReasonId", toInt(ci(r, "ReasonId")), "RefDocumentTypeId", toInt(ci(r, "RefDocumentTypeId")), "Reason", str(ci(r, "Reason")));
    }

    /** btnsave_Click:208 / btnUpdate_Click:174 — FormValidation:127, Reasons.Save (BLL 0006). */
    @Transactional
    public Map<String, Object> saveReason(PurchaseInvoiceReturnStoreDto.Reason dto) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen."); // D4
        int refType = i(dto.RefDocumentTypeId);
        if (refType == 0) throw new IllegalArgumentException("Reference Document Type Field Required");
        if (dto.Reason == null || dto.Reason.trim().isEmpty()) throw new IllegalArgumentException("Reason Name Field Required");
        int reasonId = i(dto.ReasonId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> m = new LinkedHashMap<>();                          // Model 0012, declaration order
        m.put("ReasonId", reasonId);
        m.put("Reason", dto.Reason);
        m.put("RefDocumentTypeId", refType);
        m.put("EntryDate", now);
        m.put("EntryUserId", u.getId());
        m.put("ModifyDate", now);
        m.put("ModifyUserId", u.getId());
        int n = repo.saveReason(reasonId == 0, m);
        return ok(reasonId == 0 ? "Record Save Successfully...[" + n + "]" : "Record Update Successfully...[" + n + "]", n);
    }

    // ================================================================================= delete

    /** btnDelete_Click:3131. */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("RecordId Not Found.....");
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) throw new IllegalArgumentException("RecordId Not Found.....");
        if (bool(ci(h, "IsApproved"))) throw new IllegalArgumentException("Record Not Delete beacause Record has approved");
        common.removeInvoiceVoucherAndStock(u, DOC_TYPE, id, u.getId());       // InvPurchaseInvoice.RemoveByID → DAL 0434
        return ok("Delete Record Successfully", id);
    }

    // ================================================================================= save

    /**
     * Insert():2722 → BLL 0580 Save → SaleInvoiceFinancial.MakeVoucherForSaleInvoice → DAL 0433 SetData,
     * in one transaction.
     */
    @Transactional
    public Map<String, Object> save(PurchaseInvoiceReturnStoreDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = i(dto.Id);
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = ownedHeader(u, recId);
            if (existing == null) throw new IllegalArgumentException("RecordId Not Found.....");
            if (bool(ci(existing, "IsApproved"))) throw new IllegalArgumentException("Record Not Update beacause Record has approved"); // btnUpdate_Click:2650
        }
        Set<Integer> features = repo.features(u);
        boolean subsidiary = features.contains(4);
        boolean multiCurrency = features.contains(6);
        List<PurchaseInvoiceReturnStoreDto.Row> grid = dto.rows == null ? new ArrayList<>() : dto.rows;
        boolean refDocumentExist = false, manualRow = false;
        for (PurchaseInvoiceReturnStoreDto.Row r : grid) {
            int set = (i(r.RefDocumentTypeId) > 0 ? 1 : 0) + (i(r.RefDocId) > 0 ? 1 : 0) + (i(r.RefDocSubId) > 0 ? 1 : 0);
            if (set == 3) refDocumentExist = true;
            else if (set == 0) manualRow = true;
            else throw new IllegalArgumentException("Invalid purchase invoice reference in the detail grid.");          // D7
            if (i(r.InvGdnId) > 0) throw new IllegalArgumentException("A GDN-linked return cannot be saved from the web (Load Gdn is hidden on the desktop).");  // D3
        }
        if (refDocumentExist && manualRow) throw new IllegalArgumentException("Purchase invoice rows and direct rows cannot be mixed in one return.");  // D7

        /* ---- Insert():2736-2846, in the desktop's order ---- */
        if (grid.isEmpty()) throw new IllegalArgumentException("Detail Record Not Found");
        if (i(dto.SupplierCustomerId) == 0) throw new IllegalArgumentException("Customer field is required");
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : repo.generateCode(u, ctx.currentFinancialYearId(), DOC_TYPE);  // D1
        if (docNo == 0) throw new IllegalArgumentException("Doc No must be a non-zero number");
        if (i(dto.PaymentTermId) == 0) throw new IllegalArgumentException("Payment Term field is required");
        if (i(dto.DeliveryTermId) == 0) throw new IllegalArgumentException("Delivery Term field is required");
        int dueDays = convInt(dto.DueDaysText);
        if (i(dto.PaymentTermId) == 2 && dueDays == 0) throw new IllegalArgumentException("Due Days Field is Required");
        String rateText = dto.ExchangeRateText == null ? "" : dto.ExchangeRateText.trim();
        String fcyText = dto.FcyAmountText == null ? "" : dto.FcyAmountText.trim();
        if (multiCurrency) {
            if (i(dto.CurrencyId) == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (rateText.isEmpty() || rateText.equals("0")) throw new IllegalArgumentException("Exchange Rate Field is Required");
            if (fcyText.isEmpty() || fcyText.equals("0")) throw new IllegalArgumentException("Fcy Amount Rate Field is Required");
        } else {
            if (i(dto.CurrencyId) == 0) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (rateText.isEmpty() || rateText.equals("0")) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }
        boolean anyTax = false;
        for (PurchaseInvoiceReturnStoreDto.Row r : grid) if (i(r.TaxTypeId) > 0) anyTax = true;
        if (anyTax && i(dto.TaxAccountId) == 0) throw new IllegalArgumentException("Tax Account field is required");
        int supplierGl = convInt(dto.SupplierGLIdText);
        List<PurchaseInvoiceReturnStoreDto.Journal> jl = dto.journals == null ? new ArrayList<>() : dto.journals;
        for (int k = 0; k < jl.size(); k++) {                                   // :2796
            PurchaseInvoiceReturnStoreDto.Journal r = jl.get(k);
            if (!(d(r.Credit) > 0d) && convInt(d(r.Debit)) <= 0) continue;
            if (i(r.AccountId) == 0 && i(r.GlAccountId) == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
            if (!subsidiary) {
                if (i(r.GlAccountId) > 0 && supplierGl > 0 && supplierGl == i(r.GlAccountId))
                    throw new IllegalArgumentException("you cannot Select Account Same As Supplier Account in GL Grid Row no : " + (k + 1));
            } else if (i(r.AccountId) > 0 && i(dto.SupplierCustomerId) > 0 && i(dto.SupplierCustomerId) == i(r.AccountId)) {
                throw new IllegalArgumentException("you cannot Select Account Same As Supplier Account in GL Grid Row no : " + (k + 1));
            }
        }
        List<PurchaseInvoiceReturnStoreDto.Freight> fl = dto.freights == null ? new ArrayList<>() : dto.freights;
        for (PurchaseInvoiceReturnStoreDto.Freight r : fl) {                    // :2824
            if (d(r.Freight) > 0d || convInt(d(r.Debit)) > 0) {
                if (i(r.GlAccountId) == 0 && i(r.Transporter) == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
            }
        }
        List<PurchaseInvoiceReturnStoreDto.Expense> el = dto.expenses == null ? new ArrayList<>() : dto.expenses;
        for (PurchaseInvoiceReturnStoreDto.Expense r : el) {                    // :2838
            if (d(r.Amount) > 0d && i(r.ItemId) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");
        }
        checkAgainstLookups(u, dto, grid, jl, fl, el, subsidiary, existing, recId);                                  // D8
        if (refDocumentExist) checkReferences(u, dto, grid, recId);                                                  // D7

        /* ---- the header object, Insert():2851-2889 ---- */
        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        Timestamp dueDate = dueDate(dto.DueDate, docDate);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        String remarks = dto.RemarksHeader == null ? "" : dto.RemarksHeader.trim();
        String manualBillNo = dto.ManualBillNo == null ? "" : dto.ManualBillNo.trim();
        BigDecimal exchangeRate = convDecimal(rateText);
        int currencyId = i(dto.CurrencyId);
        Map<String, Object> head = new LinkedHashMap<>();                       // Model 1031, declaration order
        head.put("IsApproved", false);
        head.put("CustomAccounts", false);
        head.put("DocDate", docDate);
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("PostDate", now);
        head.put("DueDate", dueDate);
        head.put("SupplierInvoiceDate", now);                                   // :2884
        head.put("BillAmount", convDouble(dto.BillAmountText));
        head.put("CashReceived", 0d);
        head.put("CommAmount", 0d);
        head.put("CommRate", 0d);
        head.put("FreightAmount", 0d);
        head.put("TransporterDebitGLId", 0);
        head.put("TransporterDebitPartyId", 0);
        head.put("TransporterCreditPartyId", 0);
        head.put("DueDays", dueDays);
        head.put("BranchesId", branch(u));
        head.put("CommissionAgentId", 0);
        head.put("CompanyId", u.getCompanyId());
        head.put("DocNo", docNo);
        head.put("BranchSrNo", 0);
        head.put("SalesTaxNo", 0);
        head.put("DocumentTypeId", DOC_TYPE);
        head.put("BaseDocumentTypeId", BASE_DOC_TYPE);
        head.put("DocumentTypeSrNo", 0);
        head.put("AutoUpdate", 0);
        head.put("EntryUser", recId == 0 ? u.getId() : 0);                     // BLL 0580: EntryUser = 0 on update
        head.put("Id", recId);
        head.put("ModifyUser", recId == 0 ? 0 : u.getId());                    // BLL 0580: ModifyUser = 0 on insert
        head.put("OrganizationId", u.getOrganizationId());
        head.put("PostUser", 0);
        head.put("ProjectsId", branch(u));
        head.put("ReferencePartyId", 0);
        head.put("StockPartyId", 0);
        head.put("SupplierCustomerId", i(dto.SupplierCustomerId));
        head.put("SupplierInvoiceNo", dueDays);                                 // :2870
        head.put("SupplierReferenceNo", dto.SupplierReferenceNo == null ? "" : dto.SupplierReferenceNo.trim());
        head.put("FinancialYearId", ctx.currentFinancialYearId());
        head.put("TransporterId", 0);
        head.put("CommissionDebitAcGLId", 0);
        head.put("PaymentTermId", i(dto.PaymentTermId));
        head.put("TaxAccountId", i(dto.TaxAccountId));
        head.put("DiscountAccountId", 0);
        head.put("InvoiceQty", convDecimal(dto.InvoiceQtyText));
        head.put("InvoiceWeight", BigDecimal.ZERO);
        head.put("LocationTypeId", 0);
        head.put("CurrencyId", currencyId);
        head.put("TransTypeId", 0);
        head.put("CustomgroupId", 0);
        head.put("OtherCategoryId", 0);
        head.put("ExchangeRate", exchangeRate);
        head.put("DiscountAmount", BigDecimal.ZERO);
        head.put("FcyAmount", convDecimal(fcyText));
        head.put("CommissionRemarks", null);
        head.put("CommissionType", null);
        head.put("IsAttachments", null);
        head.put("IsTaxable", false);
        head.put("IsReserve", false);
        head.put("ManualBillNo", manualBillNo);
        head.put("OtherRemarks", remarks);
        head.put("RemarksHeader", remarks);
        head.put("UomScheduleIdCmRate", null);
        head.put("DeliveryTerm", dto.DeliveryTerm == null ? "" : dto.DeliveryTerm);
        head.put("ScreenName", SCREEN);
        head.put("FreightRemark", null);
        head.put("AttachmentsValues", existing == null ? "" : str(ci(existing, "AttachmentsValues")));
        head.put("CustomAttachmentsValues", existing == null ? "" : str(ci(existing, "CustomAttachmentsValues")));
        head.put("IsUploaded", false);

        /* ---- the detail list, Insert():2892-2928 ---- */
        List<Map<String, Object>> details = new ArrayList<>();
        int amountRound = toInt(repo.configRaw(u, "Default NoofDecimal Points For Amount"));   // DefaultNoofDecimalPointsForAmount
        for (int idx = 0; idx < grid.size(); idx++) {
            PurchaseInvoiceReturnStoreDto.Row r = grid.get(idx);
            double qty = d(r.ItemQty);
            if (refDocumentExist && qty <= 0d) continue;
            Map<String, Object> vd = detailModel();
            vd.put("LineId", idx + 1);
            vd.put("Id", recId != 0 ? i(r.Id) : 0);
            fillDetail(u, vd, r, idx, exchangeRate, currencyId, amountRound);
            validate(vd.get("ItemId"), "Item", idx);
            validate(vd.get("WarehouseId"), "WareHouse", idx);
            validate(vd.get("RackId"), "Rack Name", idx);
            validate(vd.get("ItemConditionId"), "Item Condition", idx);
            validate(vd.get("JobLotId"), "JobLot", idx);
            validate(vd.get("ItemUOMId"), "Pack Uom", idx);
            validate(vd.get("ItemQty"), "Qty", idx);
            validate(vd.get("ItemRate"), "Rate", idx);
            validate(vd.get("UomScheduleIdRate"), "Rate Uom", idx);
            validate(vd.get("ItemAmount"), "Amount", idx);
            if (refDocumentExist && qty > 0d) validate(vd.get("ReasonId"), "Reason", idx);
            if (toInt(vd.get("TaxNameId")) > 0 || toDouble(vd.get("TaxPercent")) > 0d || toDouble(vd.get("TaxAmount")) > 0d) {
                validate(vd.get("TaxNameId"), "Tax Name", idx);
                validate(vd.get("TaxPercent"), "Tax Percent", idx);
                validate(vd.get("TaxAmount"), "Tax Amount", idx);
            }
            if (toDouble(vd.get("BillAmount")) <= 0d) throw new IllegalArgumentException("Item Net Amount can't be less than equal to zero");
            details.add(vd);
        }
        List<Map<String, Object>> expenses = new ArrayList<>();                 // :2929
        for (PurchaseInvoiceReturnStoreDto.Expense r : el) {
            if (i(r.ItemId) == 0) continue;
            Map<String, Object> pe = new LinkedHashMap<>();                     // Model 1040
            pe.put("Amount", d(r.Amount));
            pe.put("Qty", (double) convInt(d(r.Qty)));
            pe.put("Rate", d(r.Rate));
            pe.put("Id", 0);
            pe.put("InvRevExpItemId", i(r.ItemId));
            pe.put("InvSaleInvoiceId", 0);
            pe.put("Remarks", r.Remarks == null ? "" : r.Remarks);
            pe.put("CustomRemarks", null);
            expenses.add(pe);
        }
        List<Map<String, Object>> journals = new ArrayList<>();                 // :2943
        for (PurchaseInvoiceReturnStoreDto.Journal r : jl) {
            if (i(r.AccountId) == 0) continue;
            Map<String, Object> pj = new LinkedHashMap<>();                     // Model 1042
            pj.put("JvCredit", d(r.Credit));
            pj.put("JvDebit", d(r.Debit));
            pj.put("JvPrcnt", d(r.Percentage));
            pj.put("JvQty", d(r.Qty));
            pj.put("JvRate", d(r.Rate));
            pj.put("ChartofAccountId", i(r.GlAccountId));
            pj.put("TransporterSupCustId", subsidiary ? i(r.AccountId) : 0);
            pj.put("Id", 0);
            pj.put("InvSaleInvoiceId", 0);
            pj.put("JvRemarks", r.Remarks == null ? "" : r.Remarks);
            journals.add(pj);
        }
        List<Map<String, Object>> freights = new ArrayList<>();                 // :2968
        for (PurchaseInvoiceReturnStoreDto.Freight r : fl) {
            if (i(r.Transporter) == 0) continue;
            Map<String, Object> pf = new LinkedHashMap<>();                     // Model 1041
            pf.put("Debit", 0d);                                               // never set (note 5)
            pf.put("FreightAmount", (double) convInt(d(r.Freight)));
            pf.put("FrQty", 0d);
            pf.put("FrRate", 0d);
            pf.put("FrWeight", 0d);
            pf.put("Id", 0);
            pf.put("InvGdnId", i(r.GdnId));
            pf.put("InvSaleInvoiceId", 0);
            pf.put("TansporterId", i(r.GlAccountId));
            pf.put("TransporterSupCustId", subsidiary ? i(r.Transporter) : 0);
            pf.put("Remarks", r.Remarks == null ? "" : r.Remarks);
            freights.add(pf);
        }

        /* ---- BLL 0580 Save ---- */
        if (recId == 0) {
            for (Map<String, Object> dd : details) {
                if (toInt(dd.get("Id")) > 0) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");
            }
        }
        Voucher voucher = makeVoucher(u, head, details, freights, journals, features);
        int code = persist(u, head, details, freights, journals, expenses, voucher);
        String msg = recId == 0 ? "Record Saved Successfully [" + docNo + "] " : "Record Update Successfully [" + docNo + "] ";
        Map<String, Object> res = ok(msg, code);
        res.put("voucherHeadId", common.voucherHeadId(u, DOC_TYPE, code));    // ChkBok → VoucherHeadIdGet
        return res;
    }

    /** FillDetailListCommonForInsertAndDelete:2662. */
    private void fillDetail(UserAccount u, Map<String, Object> pd, PurchaseInvoiceReturnStoreDto.Row r, int idx,
                            BigDecimal exchangeRate, int currencyId, int amountRound) {
        double qty = d(r.ItemQty);
        pd.put("InvGdnId", i(r.InvGdnId));
        pd.put("InvGdnDetailId", i(r.InvGdnDetailId));
        pd.put("RefRefDocumentTypeId", i(r.RefDocumentTypeId));
        pd.put("RefRefDocIdNo", i(r.RefDocId));
        pd.put("RefDocSubId", i(r.RefDocSubId));
        pd.put("ItemId", i(r.ItemId));
        pd.put("WarehouseId", i(r.WarehouseId));
        pd.put("RackId", i(r.RackId));
        pd.put("ItemConditionId", i(r.ItemConditionId));
        pd.put("JobLotId", i(r.JobLotId));
        pd.put("ItemUOMId", i(r.PackUOMId));
        pd.put("ItemQty", qty);
        pd.put("GrossWeight", qty);
        pd.put("NetBillWeight", qty);
        pd.put("NetStockWeight", qty);
        pd.put("ItemRate", d(r.Rate));
        pd.put("ItemRateWOExp", BigDecimal.valueOf(d(r.Rate)));
        /* virtual properties, kept for the voucher text (never sent) */
        pd.put("~ItemName", r.Item == null ? "" : r.Item);
        List<Map<String, Object>> uoms = repo.uomScheduleByItem(u, i(r.ItemId));
        if (!uoms.isEmpty()) {
            Map<String, Object> one = null;
            for (Map<String, Object> x : uoms) if (toDouble(ci(x, "Equivalent")) == 1d) { one = x; break; }
            if (one == null) {
                /* :2696 shows this and RETURNS — the rest of the row stays at the model's defaults, and the
                   validation that follows then fails on "Rate Uom". Both messages are shown, in that order. */
                throw new IllegalArgumentException("RateUom (1) of the Item " + i(r.ItemId) + " in row No " + (idx + 1)
                        + " is not define please check \n" + "Rate Uom is required in Detail Grid at row No: " + (idx + 1));
            }
            pd.put("UomScheduleIdRate", toInt(ci(one, "Id")));
        }
        pd.put("ItemAmount", roundAway(d(r.ItemAmount), amountRound));
        pd.put("ExchangeRate", exchangeRate);
        pd.put("CurrencyId", currencyId);
        pd.put("FcyAmount", BigDecimal.valueOf(d(r.FcyAmount)));
        pd.put("TaxNameId", i(r.TaxTypeId));
        pd.put("TaxDescriptions", r.TaxTypeText == null ? "" : r.TaxTypeText);
        pd.put("TaxPercent", d(r.TaxPercent));
        pd.put("TaxAmount", d(r.TaxAmount));
        pd.put("BillAmount", d(r.BillAmount));
        pd.put("ExpenseAmount", d(r.Expense));
        pd.put("JournalAmount", d(r.Journal));
        pd.put("FreightAmount", d(r.Freight));
        pd.put("GpDate", gpDate(r.GpDate));
        pd.put("GpNo", i(r.GpNo));
        pd.put("VehicleNo", r.VehicleNo == null ? "" : r.VehicleNo);
        pd.put("CityId", i(r.CityId));
        pd.put("RemarksDetail", r.Remarks == null ? "" : r.Remarks);
        pd.put("ReasonId", i(r.ReasonId));
    }


    /** Architecture.Model.Inventory.InvSaleInvoiceDetail — non-virtual properties, declaration order, defaults. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("GpDate", null);
        p.put("DueDate", null);
        for (String k : new String[] { "AdLsWeight", "AvgCgsRate", "BillAmount", "CommissionAmount", "EBTotalWt", "EBWeight",
                "ExpenseAmount", "FreightAmount", "GrossWeight", "ItemAmount", "ItemCgsRate", "ItemQty", "ItemRate",
                "JournalAmount", "NetBillWeight", "NetStockWeight", "RateCut", "RateCutAmount", "TaxAmount", "TaxPercent",
                "WeightCut", "WeightCutTotal", "ItemDiscount", "ItemDiscountAmount" }) p.put(k, 0d);
        for (String k : new String[] { "ItemRateWOExp", "PackingAddLess", "ExchangeRate", "FcyAmount" }) p.put(k, BigDecimal.ZERO);
        for (String k : new String[] { "CurrencyId", "GpNo", "DiscountTypeId", "Id", "InvGdnDetailId", "InvGdnId", "InvSaleInvoiceId",
                "ItemId", "ItemUOMId", "ItemVariantId", "CastingTypeId", "JobLotId", "PackingTypeId", "SaleOrderId",
                "SaleOrderDetailId", "TaxNameId", "UomScheduleIdRate", "WarehouseId", "RackId", "InvForwardingId",
                "InvForwardingDetailId", "RefRefDocumentTypeId", "RefRefDocIdNo", "RefDocSubId", "LineId", "CityId",
                "BranchId", "ReserveWareHouse", "PaymentTermId", "BillCalculateTypeId", "DueDays", "CgsRateUomId",
                "GdnPartyProcessingId", "ProductionStageId", "GdnDetailPartyProcessingId", "CostCenterId", "BrandItemId",
                "ItemConditionId" }) p.put(k, 0);
        for (String k : new String[] { "CropYear", "IsTaxable", "LabAnalisysNo", "RemarksDetail", "TaxDescriptions",
                "VehicleNo", "ReferenceNo" }) p.put(k, null);
        p.put("CommOnSale", false);
        p.put("SecondaryUomId", 0);
        p.put("ReasonId", 0);
        p.put("SecondaryUomQty", BigDecimal.ZERO);
        p.put("SecondaryUomItemRate", BigDecimal.ZERO);
        return p;
    }

    /** The model map without the "~" helper keys (virtual properties SetProc never sends). */
    private static Map<String, Object> sendable(Map<String, Object> m) {
        Map<String, Object> o = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : m.entrySet()) if (!e.getKey().startsWith("~")) o.put(e.getKey(), e.getValue());
        return o;
    }

    // ================================================================================= voucher (BLL 0612 / 0613)

    private static final class Voucher {
        ContraVoucherDto.Head head;
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
    }

    /** SaleInvoiceFinancial.MakeVoucherForSaleInvoice for DocumentTypeId 145. */
    private Voucher makeVoucher(UserAccount u, Map<String, Object> obj, List<Map<String, Object>> details,
                                List<Map<String, Object>> freights, List<Map<String, Object>> journals, Set<Integer> features) {
        Voucher v = new Voucher();
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.DocumentTypeId = DOC_TYPE;
        vh.DocumentTypeSrNo = toInt(obj.get("Id"));
        vh.RefDocNoId = toInt(obj.get("Id"));
        vh.VoucherCode = toInt(obj.get("DocNo"));
        vh.VoucherDate = String.valueOf(obj.get("DocDate"));
        vh.Remarks = str(obj.get("RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = toDouble(obj.get("BillAmount"));
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();   // DateTime.Today
        vh.IncludeWHT = false;
        vh.BranchId = toInt(obj.get("BranchesId"));
        vh.ProjectId = toInt(obj.get("ProjectsId"));
        vh.BillAmount = toDouble(obj.get("BillAmount"));
        vh.ManualBillNo = str(obj.get("ManualBillNo"));
        vh.DueDate = obj.get("DueDate") == null ? null : String.valueOf(obj.get("DueDate"));
        vh.DueDays = toInt(obj.get("DueDays"));
        vh.MultiCurrencyId = toInt(obj.get("CurrencyId"));
        vh.ExchangeCurrencyRate = toDouble(obj.get("ExchangeRate"));
        vh.FcAmount = toDouble(obj.get("FcyAmount"));
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = toInt(obj.get("FinancialYearId"));
        vh.EntryUser = toInt(obj.get("EntryUser"));
        String now = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = toInt(obj.get("ModifyUser"));
        v.head = vh;

        String stockAgainst = repo.configRaw(u, "StockAgainstAccount");
        if (stockAgainst == null || stockAgainst.isEmpty()) throw new IllegalArgumentException("Stock AgainstAc Configuration Not Found");
        List<Map<String, Object>> items = common.itemGlAccounts(u);
        List<Map<String, Object>> suppliers = repo.supplierGlAccounts(u);
        int supplierId = toInt(obj.get("SupplierCustomerId"));
        Map<String, Object> party = null;
        for (Map<String, Object> s : suppliers) if (toInt(ci(s, "Id")) == supplierId) { party = s; break; }
        if (party == null) throw new IllegalArgumentException("Party GLAccountId not Found");
        int refAccountId = toInt(ci(party, "GlAccountId"));
        String companyName = str(ci(party, "CompanyName"));
        vh.RefAccountId = refAccountId;
        vh.AgainstAccountId = 0;
        int branchesId = toInt(obj.get("BranchesId"));

        itemLines(u, v, obj, details, freights, items, refAccountId, companyName, features);

        /* FreightFinancial (145 is in its set) */
        for (Map<String, Object> f : freights) {
            int tansporterId = toInt(f.get("TansporterId"));
            if (tansporterId <= 0) continue;
            ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
            l.AccountId = tansporterId;
            l.AgainstAccountId = tansporterId;                                  // CommonIdsForFinancials.SaleGLAccountId is never set
            String rem = str(f.get("Remarks"));
            l.Comments = !rem.trim().isEmpty() ? rem : "Freight";
            l.DebitAmount = toDouble(f.get("Debit"));
            l.CreditAmount = toDouble(f.get("FreightAmount"));
            l.SubsidiaryTypeId = 1;
            int sub = tansporterId == refAccountId ? supplierId : toInt(f.get("TransporterSupCustId"));
            l.SubsidiaryAccountId = sub;
            l.SupplierCustomerId = sub;
            l.CostCenterId = 0;
            l.BranchesId = branchesId;
            v.lines.add(l);
        }
        /* SupplierAddLessFinancial */
        for (Map<String, Object> j : journals) {
            int acc = toInt(j.get("ChartofAccountId"));
            if (acc == 0) continue;
            int sc = toInt(j.get("TransporterSupCustId"));
            ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
            a.AccountId = acc;
            a.AgainstAccountId = refAccountId;
            a.Comments = str(j.get("JvRemarks"));
            a.DebitAmount = toDouble(j.get("JvDebit"));
            a.CreditAmount = toDouble(j.get("JvCredit"));
            a.SubsidiaryTypeId = 1;
            a.SubsidiaryAccountId = sc;
            a.SupplierCustomerId = sc;
            a.SubsidiaryAgainstTypeId = 1;
            a.SubsidiaryAgainstAccountId = supplierId;
            a.BranchesId = branchesId;
            v.lines.add(a);
            ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
            b.AccountId = refAccountId;
            b.AgainstAccountId = acc;
            b.Comments = str(j.get("JvRemarks"));
            b.CreditAmount = toDouble(j.get("JvDebit"));
            b.DebitAmount = toDouble(j.get("JvCredit"));
            b.SubsidiaryTypeId = 1;
            b.SubsidiaryAccountId = supplierId;
            b.SupplierCustomerId = supplierId;
            b.SubsidiaryAgainstTypeId = 1;
            b.SubsidiaryAgainstAccountId = sc;
            b.BranchesId = branchesId;
            b.CostCenterId = 0;
            v.lines.add(b);
        }
        /* OtherExpenseFinancial — 145 is not in its set: nothing. SalesTaxFinancial: */
        double tax = 0d;
        for (Map<String, Object> dd : details) tax += toDouble(dd.get("TaxAmount"));
        if (tax > 0d) {
            int taxAccount = toInt(obj.get("TaxAccountId"));
            ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
            a.AccountId = refAccountId;
            a.Comments = "Tax " + clr(tax);
            a.AgainstAccountId = taxAccount;
            a.DebitAmount = tax;
            a.TaxesTotalAmount = tax;
            a.IsTaxable = "True";
            a.TaxTypeId = 2;
            a.SubsidiaryTypeId = 1;
            a.SubsidiaryAccountId = supplierId;
            a.SupplierCustomerId = supplierId;
            a.BranchesId = branchesId;
            a.CostCenterId = 0;
            v.lines.add(a);
            ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
            b.AccountId = taxAccount;
            b.Comments = "Tax " + clr(tax);
            b.AgainstAccountId = refAccountId;
            b.CreditAmount = tax;
            b.TaxesTotalAmount = tax;
            b.IsTaxable = "True";
            b.TaxTypeId = 2;
            b.SubsidiaryAgainstTypeId = 1;
            b.SubsidiaryAgainstAccountId = supplierId;
            b.BranchesId = branchesId;
            v.lines.add(b);
        }
        vh.Remarks = str(obj.get("RemarksHeader"));                              // BLL 0612 tail
        return v;
    }

    /** SaleInvoiceGeneralFinancialMethods.ItemAndCustomerByWeightFinancial — the DocumentTypeId 145 branches. */
    private void itemLines(UserAccount u, Voucher v, Map<String, Object> obj, List<Map<String, Object>> details,
                           List<Map<String, Object>> freights, List<Map<String, Object>> items, int refAccountId,
                           String companyName, Set<Integer> features) {
        int supplierId = toInt(obj.get("SupplierCustomerId"));
        String remarksHeader = str(obj.get("RemarksHeader"));
        String manualBillNo = str(obj.get("ManualBillNo"));
        /* num14 — one transporter only */
        int num14 = 0;
        double freightSum = 0d;
        Set<Integer> transporters = new java.util.LinkedHashSet<>();
        int maxT = Integer.MIN_VALUE;
        for (Map<String, Object> f : freights) {
            freightSum += toDouble(f.get("FreightAmount"));
            int t = toInt(f.get("TansporterId"));
            transporters.add(t);
            maxT = Math.max(maxT, t);
        }
        if (freightSum > 0d && transporters.size() == 1) num14 = maxT;
        boolean inventoryFinancialsInactive = bool(repo.configRaw(u, "InventoryFinancialsEffectsInActive"));
        boolean cgsEntryAllow = bool(repo.configRaw(u, "CGSEntryAllow"));
        boolean saleCostingJobOrderWise = bool(repo.configRaw(u, "SaleCostingJobOrderWise"));
        boolean feature5 = features.contains(5), feature2 = features.contains(2), feature3 = features.contains(3);

        boolean flag6 = false;
        for (Map<String, Object> item : details) {
            if (toInt(item.get("RefRefDocumentTypeId")) > 0 && toInt(item.get("RefRefDocIdNo")) > 0 && toInt(item.get("RefDocSubId")) > 0) flag6 = true;
            if (items.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
            int itemId = toInt(item.get("ItemId"));
            Map<String, Object> gl = null;
            for (Map<String, Object> x : items) if (toInt(ci(x, "Id")) == itemId) { gl = x; break; }
            if (gl == null) throw new IllegalArgumentException("Item Record Not found");
            int jobLotAccount = 0;
            int jobLotId = toInt(item.get("JobLotId"));
            if (jobLotId > 0) jobLotAccount = toInt(ci(repo.jobLotById(jobLotId), "AccountId"));
            String itemName = str(ci(gl, "ItemName"));
            int purchaseGl = toInt(ci(gl, "PurchaseGLAC"));
            int cogsGl = toInt(ci(gl, "COGSGLAC"));
            int stockAcc = jobLotAccount > 0 ? jobLotAccount : purchaseGl;
            double qty = toDouble(item.get("ItemQty"));
            double rate = toDouble(item.get("ItemRate"));
            double amount = toDouble(item.get("ItemAmount"));
            double expense = toDouble(item.get("ExpenseAmount"));
            double freight = toDouble(item.get("FreightAmount"));
            String vehicle = str(item.get("VehicleNo"));
            int gpNo = toInt(item.get("GpNo"));
            int lineId = toInt(item.get("LineId"));

            List<String> parts = new ArrayList<>();                            // :418 (60, 128, 184, 186, 145)
            if (!remarksHeader.trim().isEmpty()) parts.add(remarksHeader);
            parts.add("Item: " + itemName);
            parts.add("Qty: " + clr(qty));
            parts.add("Rate: " + clr(rate));
            if (expense > 0d) parts.add("Expense: " + clr(expense));
            String comments = String.join(", ", parts);

            ContraVoucherDto.Detail dr = itemLine(item, lineId);
            dr.AccountId = refAccountId;
            dr.AgainstAccountId = stockAcc;
            dr.Comments = comments;
            dr.DebitAmount = amount;
            dr.SubsidiaryTypeId = 1;
            dr.SubsidiaryAccountId = supplierId;
            dr.SupplierCustomerId = supplierId;
            v.lines.add(dr);
            ContraVoucherDto.Detail cr = itemLine(item, lineId);
            cr.AccountId = stockAcc;
            cr.AgainstAccountId = refAccountId;
            cr.Comments = comments + "  " + companyName;
            cr.CreditAmount = amount;
            cr.SubsidiaryAgainstTypeId = 1;
            cr.SubsidiaryAgainstAccountId = supplierId;
            v.lines.add(cr);
            double num15 = cr.CreditAmount;

            /* :807 — CGS / stock difference */
            if (jobLotAccount == 0 && !inventoryFinancialsInactive && (cgsEntryAllow || saleCostingJobOrderWise || (feature5 && flag6))) {
                boolean flag11 = feature3;                                      // 145: never forced by feature 5
                double cgsRate = 0d;
                if (flag11) {
                    int rdt = toInt(item.get("RefRefDocumentTypeId")), rid = toInt(item.get("RefRefDocIdNo"));
                    if (rdt <= 0 || rid <= 0) throw new IllegalArgumentException("RefIds Not Found");
                    List<Map<String, Object>> r = repo.avgRateFromLoaderStock(u, rdt, rid, toInt(item.get("RefDocSubId")));
                    if (!r.isEmpty()) {
                        cgsRate = toDouble(ci(r.get(0), "AvgRate"));
                        item.put("CgsRateUomId", toInt(ci(r.get(0), "RateUomId")));
                    }
                    item.put("ItemCgsRate", cgsRate);
                    if (cgsRate <= 0d) throw new IllegalArgumentException("CGS Rate not found");
                } else {                                                        // flag10: 145 is in {103,126,133,145}
                    int recId = toInt(obj.get("Id"));
                    Map<String, Object> r = common.avgRateAndStock(u, itemId, (Timestamp) obj.get("DocDate"),
                            toInt(item.get("ItemConditionId")), recId, recId > 0 ? DOC_TYPE : 0, 0, 0);
                    if (r != null) cgsRate = roundAway(toDouble(ci(r, "AvgRate")), 3);
                    item.put("ItemCgsRate", cgsRate);
                    if (cgsRate <= 0d) throw new IllegalArgumentException("CGS Rate not found.for Item " + str(item.get("~ItemName")));
                }
                double num19 = cgsRate * qty;
                double num22 = num15 - num19;                                   // flag9: 145 writes the difference only
                if (num22 != 0d) {
                    ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
                    a.IsCGS = 1;
                    a.AccountId = purchaseGl;
                    a.AgainstAccountId = cogsGl;
                    a.Comments = "Stock Difference";
                    if (num22 > 0d) a.DebitAmount = num22; else a.CreditAmount = num22;
                    a.BranchesId = toInt(obj.get("BranchesId"));
                    v.lines.add(a);
                    ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
                    b.IsCGS = 1;
                    b.AccountId = cogsGl;
                    b.AgainstAccountId = purchaseGl;
                    b.Comments = "Stock Difference";
                    if (num22 > 0d) b.CreditAmount = num22; else b.DebitAmount = num22;
                    b.BranchesId = toInt(obj.get("BranchesId"));
                    v.lines.add(b);
                }
            }

            /* :1105 — freight debit line (TransporterDebitGLId is 0 on this form) */
            if (freight > 0d) {
                List<String> fp = new ArrayList<>();
                fp.add("Purchase Return " + clr(qty) + " Qty of " + itemName);
                fp.add("@ NetRate " + clr(rate));
                fp.add("= Rs. " + clr(amount));
                fp.add("Freight " + clr(freight));
                if (!vehicle.trim().isEmpty()) fp.add("Vehicle # " + vehicle);
                if (gpNo != 0) fp.add("Gp # " + gpNo);
                if (!manualBillNo.trim().isEmpty()) fp.add("Manual # " + manualBillNo);
                fp.add("From " + companyName);
                ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
                l.AccountId = stockAcc;
                l.AgainstAccountId = num14 > 0 ? num14 : l.AccountId;
                l.Comments = String.join(", ", fp);
                l.DebitAmount = freight;
                l.CreditAmount = 0d;
                l.JobLotId = jobLotId;
                l.BranchesId = toInt(item.get("BranchId"));
                l.CostCenterId = 0;
                v.lines.add(l);
            }

            /* :1239 — other expense of a purchase return */
            if (expense > 0d) {
                String t = "Purchase Return " + itemName + " Qty " + clr(qty) + " @ NetRate. " + clr(rate) + "   = Rs. " + clr(amount);
                t = t + " Other Exp " + clr(expense);
                if (!vehicle.isEmpty()) t = t + " Vehicle # " + vehicle;
                if (gpNo != 0) t = t + " Gp # " + gpNo;
                if (!manualBillNo.isEmpty()) t = t + " Manual # " + manualBillNo;
                t = t + " from " + companyName;
                ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
                a.AccountId = refAccountId;
                a.AgainstAccountId = stockAcc;
                a.DebitAmount = expense;
                a.Comments = t;
                a.JobLotId = jobLotId;
                a.SubsidiaryTypeId = 1;
                a.SubsidiaryAccountId = supplierId;
                a.SupplierCustomerId = supplierId;
                a.BranchesId = toInt(item.get("BranchId"));
                v.lines.add(a);
                ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
                b.AccountId = stockAcc;
                b.AgainstAccountId = refAccountId;
                b.CreditAmount = expense;
                b.Comments = t;
                b.JobLotId = jobLotId;
                b.SubsidiaryAgainstTypeId = 1;
                b.SubsidiaryAgainstAccountId = supplierId;
                b.BranchesId = toInt(item.get("BranchId"));
                v.lines.add(b);
            }
        }
    }

    /** The fields both item lines share (:566-640). */
    private static ContraVoucherDto.Detail itemLine(Map<String, Object> item, int lineId) {
        ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
        l.LineId = lineId;
        l.ItemId = toInt(item.get("ItemId"));
        l.QtyOut = toDouble(item.get("ItemQty"));
        l.ItemRate = toDouble(item.get("ItemRate"));
        l.WeightOut = toDouble(item.get("NetBillWeight"));
        l.RateCut = toDouble(item.get("RateCut"));
        l.RateCutAmount = toDouble(item.get("RateCutAmount"));
        l.ItemAmount = toDouble(item.get("ItemAmount"));
        l.Expenses = toDouble(item.get("ExpenseAmount"));
        l.Freight = toDouble(item.get("FreightAmount"));
        l.Commission = toDouble(item.get("CommissionAmount"));
        l.OrderNo = 0;
        l.GpNo = toInt(item.get("GpNo"));
        l.JobLotId = toInt(item.get("JobLotId"));
        l.VehicleNo = str(item.get("VehicleNo"));
        l.DMultiCurrencyId = toInt(item.get("CurrencyId"));
        l.DExchangeCurrencyRate = toDouble(item.get("ExchangeRate"));
        l.DCurrencyAmount = toDouble(item.get("FcyAmount"));
        l.BranchesId = toInt(item.get("BranchId"));
        l.CostCenterId = toInt(item.get("CostCenterId"));
        return l;
    }

    // ================================================================================= DAL 0433 SetData

    private int persist(UserAccount u, Map<String, Object> head, List<Map<String, Object>> details,
                        List<Map<String, Object>> freights, List<Map<String, Object>> journals,
                        List<Map<String, Object>> expenses, Voucher voucher) {
        if (details.isEmpty()) throw new IllegalArgumentException("Detail List not found");
        int recId = toInt(head.get("Id"));
        int num3 = repo.setProc(recId == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update", head);
        if (num3 > 0) head.put("Id", num3); else num3 = recId;
        if (num3 <= 0) throw new IllegalStateException("Save returned no document id.");        // D5
        int id = num3;

        int line = 1;
        for (Map<String, Object> dd : details) {
            dd.put("LineId", line++);
            dd.put("InvSaleInvoiceId", id);
            dd.put("BillAmount", toDouble(dd.get("ItemAmount")) + toDouble(dd.get("ExpenseAmount"))
                    - toDouble(dd.get("CommissionAmount")) - toDouble(dd.get("FreightAmount")));      // note 4
            dd.put("Id", repo.setProc("Sp_InvSaleInvoiceDetail_Insert", sendable(dd)));
        }
        for (Map<String, Object> f : freights) { f.put("InvSaleInvoiceId", id); repo.setProc("Sp_InvSaleInvoiceFreight_Insert", f); }
        for (Map<String, Object> j : journals) { j.put("InvSaleInvoiceId", id); repo.setProc("Sp_InvSaleInvoiceJournal_Insert", j); }
        for (Map<String, Object> e : expenses) { e.put("InvSaleInvoiceId", id); repo.setProc("Sp_InvSaleInvoiceExpense_Insert", e); }

        /* 145 is excluded from the FIFO branch; no GDN rows (D3) → UpdateInventoryReference. */
        repo.updateInventoryReference(u, DOC_TYPE, id);
        repo.stockInTransitVoucherDelete(id);
        /* 145 is not in the InventoryTransactions set. USP_InventoryValidation per detail: */
        for (Map<String, Object> dd : details) {
            Map<String, Object> p = params(
                    "OrganizationId", u.getOrganizationId(),
                    "CompanyId", u.getCompanyId(),
                    "DocumentTypeId", DOC_TYPE,
                    "DocDate", head.get("DocDate"),
                    "ItemId", dd.get("ItemId"),
                    "WarehouseId", dd.get("WarehouseId"),
                    "JobLotId", dd.get("JobLotId"),
                    "CropYear", dd.get("CropYear"),
                    "InvPackingTypeId", dd.get("PackingTypeId"),
                    "PackUomId", dd.get("ItemUOMId"),
                    "NetWeight", dd.get("NetStockWeight"),
                    "RefDocumentTypeId", dd.get("RefRefDocumentTypeId"),
                    "RefDocNoId", dd.get("RefRefDocIdNo"),
                    "RefDocSubIdNo", dd.get("RefDocSubId"),
                    "ItemConditionId", dd.get("ItemConditionId"));
            repo.inventoryValidation(p);
        }

        /* voucher */
        ContraVoucherDto.Head vh = voucher.head;
        int num2 = common.voucherHeadId(u, DOC_TYPE, id);
        if (num2 > 0) vh.Id = num2;
        vh.DocumentTypeSrNo = id;
        vh.RefDocNoId = id;
        int num = repo.setProc(num2 == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(vh));
        if (num > 0) vh.Id = num;
        for (ContraVoucherDto.Detail l : voucher.lines) {
            l.VoucherHeadId = vh.Id;
            l.BranchesId = toInt(head.get("BranchesId"));
            int ln = l.LineId == null ? 0 : l.LineId;
            for (Map<String, Object> dd : details) {
                if (ln > 0 && toInt(dd.get("LineId")) == ln) { l.RefDocSubIdNo = toInt(dd.get("Id")); break; }
            }
            repo.setProc("Sp_VoucherDetail_Insert", model(l));
        }
        if (voucher.lines.isEmpty()) throw new IllegalArgumentException("Voucher Detail list Not Found");
        repo.voucherBalanceCheck(u, vh.Id);
        int documentTypeIdRef = repo.setProc("Sp_VoucherHead_H_Insert", model(vh));
        for (ContraVoucherDto.Detail l : voucher.lines) {
            l.VoucherHeadId = vh.Id;
            l.DocumentTypeIdRef = documentTypeIdRef;
            repo.setProc("Sp_VoucherDetail_H_Insert", model(l));
        }
        Map<String, Object> appr = new LinkedHashMap<>();                       // DocumentApprovalDetail (model 0046)
        appr.put("OrganizationId", u.getOrganizationId());
        appr.put("CompanyId", u.getCompanyId());
        appr.put("DocumentTypeId", DOC_TYPE);
        appr.put("Id", id);
        appr.put("LimitAmount", BigDecimal.valueOf(toDouble(head.get("BillAmount"))));
        repo.setProc("[DAW].[USp_DocumentApprovalDetail_Insert]", appr);
        return id;
    }

    /** Public fields in declaration order; date strings become timestamps (null stays omitted). */
    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                if (v != null && f.getType() == String.class && (f.getName().endsWith("Date") || "DCheqDate".equals(f.getName()))) {
                    String s = String.valueOf(v);
                    v = Timestamp.valueOf(s.length() == 10 ? s + " 00:00:00" : s.replace('T', ' '));
                }
                m.put(f.getName(), v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    // ================================================================================= helpers

    /** D8 — ids must come from the lists the lookups serve (or be stored on this document already). */
    private void checkAgainstLookups(UserAccount u, PurchaseInvoiceReturnStoreDto dto, List<PurchaseInvoiceReturnStoreDto.Row> grid,
                                     List<PurchaseInvoiceReturnStoreDto.Journal> jl, List<PurchaseInvoiceReturnStoreDto.Freight> fl,
                                     List<PurchaseInvoiceReturnStoreDto.Expense> el, boolean subsidiary,
                                     Map<String, Object> existing, int recId) {
        Set<Integer> taxAcc = new HashSet<>();
        for (Map<String, Object> a : taxAccounts(u)) taxAcc.add(toInt(a.get("Id")));
        if (existing != null) taxAcc.add(toInt(ci(existing, "TaxAccountId")));
        if (i(dto.TaxAccountId) != 0 && !taxAcc.contains(i(dto.TaxAccountId))) throw new IllegalArgumentException("Invalid Tax Account.");

        Map<String, Object> acc = accounts(u, subsidiary, bool(repo.configRaw(u, "DebitAmountChargetoExpenseAcFreightGridPurchase")));
        Set<Integer> glIds = new HashSet<>(), glSub = new HashSet<>(), frIds = new HashSet<>(), frSub = new HashSet<>();
        for (Object o : (List<?>) acc.get("accounts")) { Map<?, ?> m = (Map<?, ?>) o; glIds.add(toInt(m.get("Id"))); glSub.add(toInt(m.get("SupplierCustomerId"))); }
        for (Object o : (List<?>) acc.get("freightAccounts")) { Map<?, ?> m = (Map<?, ?>) o; frIds.add(toInt(m.get("Id"))); frSub.add(toInt(m.get("SupplierCustomerId"))); }
        if (recId > 0) {
            for (Map<String, Object> j : repo.journals(recId)) { glIds.add(toInt(ci(j, "ChartofAccountId"))); glSub.add(toInt(ci(j, "TransporterSupCustId"))); }
            for (Map<String, Object> f : repo.freights(recId)) { frIds.add(toInt(ci(f, "TansporterId"))); frSub.add(toInt(ci(f, "TransporterSupCustId"))); }
        }
        for (PurchaseInvoiceReturnStoreDto.Journal r : jl) {
            if (i(r.AccountId) == 0) continue;                                   // skipped by the save (:2946)
            if (!glIds.contains(i(r.GlAccountId)) || (subsidiary ? !glSub.contains(i(r.AccountId)) : !glIds.contains(i(r.AccountId))))
                throw new IllegalArgumentException("Invalid account in the Party Add/Less grid.");
        }
        for (PurchaseInvoiceReturnStoreDto.Freight r : fl) {
            if (i(r.Transporter) == 0) continue;                                 // skipped by the save (:2971)
            if ((i(r.GlAccountId) != 0 && !frIds.contains(i(r.GlAccountId))) || (subsidiary ? !frSub.contains(i(r.Transporter)) : !frIds.contains(i(r.Transporter))))
                throw new IllegalArgumentException("Invalid transporter in the Charge To Product grid.");
        }
        Set<Integer> other = new HashSet<>();
        for (Map<String, Object> o : repo.otherItems(u)) other.add(toInt(ci(o, "Id")));
        for (PurchaseInvoiceReturnStoreDto.Expense r : el) {
            if (i(r.ItemId) != 0 && !other.contains(i(r.ItemId))) throw new IllegalArgumentException("Invalid item in the Other Items grid.");
        }
        Set<Integer> lots = new HashSet<>();
        for (Map<String, Object> j : repo.jobLots(u)) lots.add(toInt(ci(j, "Id")));
        for (int k = 0; k < grid.size(); k++) {
            int lot = i(grid.get(k).JobLotId);
            if (lot != 0 && !lots.contains(lot)) throw new IllegalArgumentException("Invalid Job Lot in Detail Grid at row No: " + (k + 1));
        }
    }

    /** D7 — every referenced row re-read against this company's purchase invoices. */
    private void checkReferences(UserAccount u, PurchaseInvoiceReturnStoreDto dto, List<PurchaseInvoiceReturnStoreDto.Row> grid, int recId) {
        for (int k = 0; k < grid.size(); k++) {
            PurchaseInvoiceReturnStoreDto.Row r = grid.get(k);
            double qty = d(r.ItemQty);
            if (qty <= 0d) continue;                                             // skipped by the save (:2899)
            Map<String, Object> line = repo.purchaseInvoiceLine(u, i(r.RefDocId), i(r.RefDocSubId));
            int type = line == null ? 0 : toInt(ci(line, "DocumentTypeId"));
            if (line == null || (type != 61 && type != 64 && type != 131) || type != i(r.RefDocumentTypeId)
                    || toInt(ci(line, "ItemId")) != i(r.ItemId)) {
                throw new IllegalArgumentException("Purchase invoice line not found for Detail Grid row No: " + (k + 1));
            }
            if (toInt(ci(line, "SupplierCustomerId")) != i(dto.SupplierCustomerId))
                throw new IllegalArgumentException("The purchase invoice of Detail Grid row No: " + (k + 1) + " belongs to another party.");
            double used = repo.returnedQtyElsewhere(type, i(r.RefDocId), i(r.RefDocSubId), recId);
            double bal = toDouble(ci(line, "ItemQty")) - used;
            if (qty > bal + 1e-9) throw new IllegalArgumentException("Item Quantity can't be greater than Balance Quantity... (row No: " + (k + 1) + ", balance " + clr(bal) + ")");
        }
    }

    /** Header of this screen's document type and the user's company, or null. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOC_TYPE) return null;
        if (ci(h, "CompanyId") != null && toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (ci(h, "OrganizationId") != null && toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    /** duedate.Value — a date typed on the page carries the doc date's time of day (DocDate.Value.AddDays). */
    private static Timestamp dueDate(String s, Timestamp docDate) {
        if (isBlank(s)) return docDate;
        String t = s.trim().replace('T', ' ');
        if (t.length() > 10) {
            try { return Timestamp.valueOf(t.length() == 16 ? t + ":00" : t.substring(0, Math.min(t.length(), 19))); }
            catch (IllegalArgumentException e) { /* fall through to the date part */ }
        }
        LocalDate d = LocalDate.parse(t.substring(0, 10));
        return Timestamp.valueOf(LocalDateTime.of(d, docDate.toLocalDateTime().toLocalTime()));
    }

    /** Conversion.ToDateTime(GpDate cell) — DBNull is 1900-01-01; ToShortDateString round-trip is midnight. */
    private static Timestamp gpDate(String s) {
        if (isBlank(s)) return Timestamp.valueOf(LocalDate.of(1900, 1, 1).atStartOfDay());
        try { return Timestamp.valueOf(LocalDate.parse(s.trim().substring(0, 10)).atStartOfDay()); }
        catch (Exception e) { return Timestamp.valueOf(LocalDate.of(1900, 1, 1).atStartOfDay()); }
    }

    /** Dates leave as local "yyyy-MM-dd HH:mm:ss" text, so no JSON time-zone conversion can move the day. */
    private static Object ts(Object v) {
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + " 00:00:00";
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString().replace('T', ' ');
        if (v instanceof LocalDate) return v.toString() + " 00:00:00";
        return v;
    }

    private static Map<String, Object> tsAll(Map<String, Object> m) {
        Map<String, Object> o = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : m.entrySet()) o.put(e.getKey(), ts(e.getValue()));
        return o;
    }

    private static List<Map<String, Object>> tsRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(tsAll(r));
        return out;
    }

    private static String dayOr1900(Object v) {
        if (v == null) return "1900-01-01";
        String s = String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : s;
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

    /** Math.Round(v, digits, MidpointRounding.AwayFromZero). */
    static double roundAway(double v, int digits) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return v;
        int dg = Math.max(0, Math.min(15, digits));
        return BigDecimal.valueOf(v).setScale(dg, RoundingMode.HALF_UP).doubleValue();
    }

    /** Conversion.ToInt(text) — Convert.ToInt32(string): an integer literal only (commas/decimals → 0). */
    private static int convInt(String s) {
        if (s == null) return 0;
        String t = s.trim();
        if (t.isEmpty()) return 0;
        try { return Integer.parseInt(t.startsWith("+") ? t.substring(1) : t); }
        catch (NumberFormatException e) {
            /* Convert.ToInt32(string) accepts thousands separators in en-US ("1,234"), nothing else. */
            try { if (t.matches("[-+]?\\d{1,3}(,\\d{3})+")) return Integer.parseInt(t.replace(",", "").replace("+", "")); }
            catch (NumberFormatException ignored) { }
            return 0;
        }
    }

    /** Conversion.ToInt(double) — Convert.ToInt32(double): banker's rounding. */
    private static int convInt(double v) {
        if (Double.isNaN(v) || Math.abs(v) > Integer.MAX_VALUE) return 0;
        return (int) Math.rint(v);
    }

    private static double convDouble(String s) {
        if (s == null) return 0d;
        try { double v = Double.parseDouble(s.trim().replace(",", "")); return Double.isInfinite(v) ? 0d : v; }
        catch (NumberFormatException e) { return 0d; }
    }

    private static BigDecimal convDecimal(String s) {
        if (s == null) return BigDecimal.ZERO;
        try { return new BigDecimal(s.trim().replace(",", "")); }
        catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    static boolean bool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String t = String.valueOf(v).trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }

    /** UserAccount.BranchName — the login context's branch name, else the Branches row. */
    private String branchName(UserAccount u) {
        try {
            Authentication auth = SecurityContextHolder.getContext().getAuthentication();
            RequestAttributes attrs = RequestContextHolder.getRequestAttributes();
            if (auth != null && attrs instanceof ServletRequestAttributes) {
                LoginContext c = LoginContext.of(((ServletRequestAttributes) attrs).getRequest(), auth.getName());
                if (c != null && c.getBranchId() == branch(u) && c.getBranchName() != null) return c.getBranchName();
            }
        } catch (Exception e) {
            LOG.debug("No login context branch name", e);
        }
        try {
            List<Map<String, Object>> r = jdbc.queryForList("SELECT BranchName FROM dbo.Branches WHERE Id = ?", branch(u));
            return r.isEmpty() ? "" : str(ci(r.get(0), "BranchName"));
        } catch (Exception e) {
            LOG.warn("Could not read the branch name", e);
            return "";
        }
    }

    private String financialYearStart(UserAccount u, int yearId) {
        try {
            List<Map<String, Object>> years = jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId());
            for (Map<String, Object> r : years) {
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

    private static Map<String, Object> row(Object... kv) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (int k = 0; k + 1 < kv.length; k += 2) m.put(String.valueOf(kv[k]), kv[k + 1]);
        return m;
    }

    private static Map<String, Object> ok(String message, int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        return m;
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
}
