package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels.*;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDateTime;
import java.util.*;
import java.util.stream.Collectors;

import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Sale Invoice Against GDN Without WB (frmSaleInvoiceAgainstGdnWithoutWb, DocumentTypeId 171).
 *
 * The form is a sibling of InvfrmSaleInvoice (95): the same BLL (0580 InvSaleInvoice.Save), the same
 * financial builder (0612/0613 SaleInvoiceFinancial) and the same DAL (0433 SetData), so those are shared
 * with {@link SaleInvoiceService}. What differs is the form: a header commission instead of a commission
 * grid, no payment-term grid, no location type, GDNs of types 170/221 loaded through
 * usp_getGdnDataForSaleInvoiceWithoutWeightBridgeByIds, and its own proportions and bill amount
 * (frmSaleInvoiceAgainstGdnWithoutWb.cs :3082-3380). Each method names the desktop lines it follows.
 */
@Service
public class SaleInvoiceGdnNoWbService {
    public static final int DOCUMENT_TYPE_ID = 171;
    public static final String SCREEN_NAME = "frmSaleInvoiceAgainstGdnWithoutWb";
    /** BtnLoadGdn_Click: LoadGDN.DocumentTypeIds = "170,221". */
    public static final String GDN_TYPES = "170,221";
    /** AccountsFill runs before ConfigurationDefault on Load, so FreightDebitToExpenses is still false there. */
    private static final String ACCOUNT_TYPES_NOT = "2,11,12,,13,14,15,20,21,22";
    private static final String ACCOUNT_TYPES_NOT_FREIGHT_TO_EXPENSES = "2,15,22";

    private final SaleInvoiceRepository repo;
    private final CurrentUserContext context;
    private final SaleInvoiceService shared;

    public SaleInvoiceGdnNoWbService(SaleInvoiceRepository repo, CurrentUserContext context, SaleInvoiceService shared) {
        this.repo = repo;
        this.context = context;
        this.shared = shared;
    }

    /** InvfrmPurchaseInvoice_Load (:354) + ConfigurationDefault (:843). */
    public Map<String, Object> settings(UserAccount u) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("SubsidiaryAccountAllownOnVouchers", repo.feature(u, 4));
        m.put("HasMultiCurrencyFeature", repo.feature(u, 6));
        m.put("DebitAmountChargetoExpenseAcFreightGrid", truthy(repo.config(u, "DebitAmountChargetoExpenseAcFreightGrid")));
        m.put("DebitAmountChargetoExpenseAcOfCommission", truthy(repo.config(u, "DebitAmountChargetoExpenseAcOfCommission")));
        /* BillProportion (:3246): no row -> ItemAmount + Expense - Commission; a row -> bool.Parse(ConfigKey). */
        m.put("CreditAmountInItemSaleGLRaw", repo.config(u, "CreditAmountInItemSaleGL").trim());
        m.put("DefaultDaysToLessFromHistoryFromDate", i(repo.config(u, "DefaultDaysToLessFromHistoryFromDate").trim()));
        m.put("BaseCurrencyId", i(repo.config(u, "Base Currency").trim()));
        m.put("BaseCurrencyRate", d(repo.config(u, "BaseCurrencyRate").trim()));
        String dp = repo.config(u, "DefaultNoofDecimalPointsForAmount").trim();
        m.put("DefaultNoofDecimalPointsForAmount", dp.isEmpty() ? 0 : i(dp));
        m.put("SaleInvoiceBranchWise", truthy(repo.config(u, "SaleInvoiceBranchWise")));
        m.put("IsStockReservedPerParty", truthy(repo.config(u, "IsStockReservedPerParty")));
        return m;
    }

    /**
     * Load (:354) or btnRefresh_Click (:2493). The only difference is AccountsFill: on Load it runs before
     * ConfigurationDefault has read DebitAmountChargetoExpenseAcFreightGrid, on Refresh after it.
     */
    public Map<String, Object> lookups(boolean refresh) {
        UserAccount u = context.requireAccountingUser();
        int year = context.currentFinancialYearId();
        Map<String, Object> s = settings(u);
        boolean subsidiary = (Boolean) s.get("SubsidiaryAccountAllownOnVouchers");
        boolean freightToExpenses = refresh && (Boolean) s.get("DebitAmountChargetoExpenseAcFreightGrid");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("settings", s);
        m.put("rights", repo.rights(u, context.currentRoleName(), SCREEN_NAME));
        m.put("docNo", repo.nextDocNo(u, year, DOCUMENT_TYPE_ID));
        m.put("currencies", repo.currencies(u));
        m.put("customers", repo.customers(u, subsidiary));
        m.put("accounts", accounts(u, subsidiary, freightToExpenses));
        m.put("commissionDebitAccounts", (Boolean) s.get("DebitAmountChargetoExpenseAcOfCommission") ? commissionDebitAccounts(u, subsidiary) : List.of());
        m.put("otherItems", repo.otherItems(u));
        m.put("historyCustomers", repo.customersFromGdn(u));
        LocalDateTime start = repo.financialYearStart(u, year);
        m.put("yearStart", start == null ? null : start.toLocalDate().toString());
        return m;
    }

    public Map<String, Object> nextCodes() {
        UserAccount u = context.requireAccountingUser();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", repo.nextDocNo(u, context.currentFinancialYearId(), DOCUMENT_TYPE_ID));
        return m;
    }

    /** AccountsFill (:683) - dtAccountlst {Id, SupplierCustomerId, AccountTitle}. */
    List<Map<String, Object>> accounts(UserAccount u, boolean subsidiary, boolean freightToExpenses) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (subsidiary) {
            for (var r : repo.q("EXEC dbo.USP_GetVendorsAndCustomersForTransporter @OrganizationId=?, @CompanyId=?", u.getOrganizationId(), u.getCompanyId())) {
                Map<String, Object> x = new LinkedHashMap<>();
                x.put("Id", i(col(r, "GlAccountId"))); x.put("SupplierCustomerId", i(col(r, "Id"))); x.put("AccountTitle", s(col(r, "CompanyName")));
                out.add(x);
            }
            return out;
        }
        for (var r : repo.accountTitlesByTypes(u, null, freightToExpenses ? ACCOUNT_TYPES_NOT_FREIGHT_TO_EXPENSES : ACCOUNT_TYPES_NOT)) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "Id"))); x.put("SupplierCustomerId", 0); x.put("AccountTitle", s(col(r, "AccountTitle")));
            out.add(x);
        }
        return out;
    }

    /** CommissionDebitAccountFill (:639): feature 4 -> GetVendorsAndCustomers(2) {Id, CompanyName}; else COA types 11,12,13,14,20,21. */
    List<Map<String, Object>> commissionDebitAccounts(UserAccount u, boolean subsidiary) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (subsidiary) {
            for (var r : repo.vendorsAndCustomers(u, 2)) {
                Map<String, Object> x = new LinkedHashMap<>();
                x.put("Id", i(col(r, "Id"))); x.put("AccountTitle", s(col(r, "CompanyName")));
                out.add(x);
            }
            return out;
        }
        for (var r : repo.accountTitlesByTypes(u, "11,12,13,14,20,21", null)) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "Id"))); x.put("AccountTitle", s(col(r, "AccountTitle")));
            out.add(x);
        }
        return out;
    }

    // ============================================================================== frmLoadGDN (DocumentTypeIds "170,221")

    public Map<String, Object> pendingGdns(String from, String to, String branchIds) {
        UserAccount u = context.requireAccountingUser();
        boolean branchWise = truthy(repo.config(u, "SaleInvoiceBranchWise"));
        var branches = repo.gdnBranches(u, branchWise, GDN_TYPES);
        Set<Integer> allowed = branches.stream().map(r -> i(col(r, "BranchId"))).collect(Collectors.toSet());
        StringBuilder ids = new StringBuilder();
        for (String p : (branchIds == null ? "" : branchIds).split(",")) {
            int b = i(p);
            if (b > 0 && allowed.contains(b)) ids.append(',').append(b);
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branches", branches);
        m.put("branchImplemented", branchWise);
        m.put("isStockReservedPerParty", truthy(repo.config(u, "IsStockReservedPerParty")));
        if (branchIds == null) { m.put("rows", List.of()); return m; }
        if (ids.length() == 0) throw new IllegalArgumentException("Select branch first");
        m.put("rows", repo.pendingGdn(u, context.currentFinancialYearId(), dt(from), dt(to), ids.toString(), GDN_TYPES));
        return m;
    }

    public List<Map<String, Object>> gdnDetail(int gdnId) { return repo.gdnDetail(context.requireAccountingUser(), gdnId); }

    /** Every GDN id must be a 170/221 GDN of the signed-in company (the browser only picks from the pending list). */
    private void requireOwnGdns(UserAccount u, Collection<Integer> gdnIds) {
        for (int id : new LinkedHashSet<>(gdnIds)) {
            var h = repo.q("EXEC dbo.Sp_InvGdn_GetAllMethod @Id=?, @Activity=?", id, "GetById");
            if (h.isEmpty()) throw new IllegalArgumentException("GDN " + id + " not found");
            var head = h.get(0);
            if (i(col(head, "OrganizationId")) != u.getOrganizationId() || i(col(head, "CompanyId")) != u.getCompanyId())
                throw new IllegalArgumentException("GDN is outside the current company");
            int type = i(col(head, "DocumentTypeId"));
            if (type != 170 && type != 221) throw new IllegalArgumentException("GDN " + id + " is not a GDN Without WB");
        }
    }

    /**
     * LoadInGridDetail (:3575): LoadDataDetailGridAgainstGP (GdnLoadForSaleInvoiceWithoutWeighBridge),
     * LoadFreightData (GetTransporterAndFreightFromGdn) and LoadExpData (GetItemQtyAndItemRateFromSO).
     */
    public Map<String, Object> loadGdns(List<Integer> gdnIds) {
        UserAccount u = context.requireAccountingUser();
        if (gdnIds == null || gdnIds.isEmpty()) throw new IllegalArgumentException("Check the row first");
        requireOwnGdns(u, gdnIds);
        String ids = gdnIds.stream().map(x -> "," + x).collect(Collectors.joining());
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("details", repo.gdnLoadWithoutWb(ids));
        out.put("freights", repo.freightFromGdn(ids));
        out.put("soExpenses", repo.itemQtyAndRateFromSo(ids));
        return out;
    }

    // ============================================================================== read / delete / history

    public Map<String, Object> read(int id) { return shared.read(id, DOCUMENT_TYPE_ID); }

    /** btnDelete_Click (:2448) - InvPurchaseInvoice.RemoveByID with DocumentTypeId 171. */
    @Transactional
    public void delete(int id) {
        UserAccount u = context.requireAccountingUser();
        if (!Boolean.TRUE.equals(repo.rights(u, context.currentRoleName(), SCREEN_NAME).get("Delete")))
            throw new SecurityException("You do not have the Delete right on Sale Invoice");
        Map<String, Object> r = read(id);
        if (b(col(r, "IsApproved"))) throw new IllegalStateException("Record cannot be  Delete beacause Record has approved");
        repo.delete(u, id, DOCUMENT_TYPE_ID);
    }

    /** GetAll (:2701). */
    public List<Map<String, Object>> history(String dateMode, String from, String to, Integer fromDocNo, Integer toDocNo, Integer customerId) {
        UserAccount u = context.requireAccountingUser();
        boolean canViewAll = Boolean.TRUE.equals(repo.rights(u, context.currentRoleName(), SCREEN_NAME).get("CanViewAllRecord"));
        return repo.historyByUser(u, DOCUMENT_TYPE_ID, context.currentFinancialYearId(), canViewAll, dateMode, dt(from), dt(to),
                fromDocNo == null ? 0 : fromDocNo, toDocNo == null ? 0 : toDocNo, customerId == null ? 0 : customerId);
    }

    public Double lastExchangeRate(int currencyId) { return repo.lastExchangeRate(context.requireAccountingUser(), currencyId, DOCUMENT_TYPE_ID); }

    // ============================================================================== Insert (:1887)

    @Transactional
    public Map<String, Object> save(Map<String, Object> req) {
        UserAccount u = context.requireAccountingUser();
        int year = context.currentFinancialYearId();
        Map<String, Object> s = settings(u);
        boolean subsidiary = (Boolean) s.get("SubsidiaryAccountAllownOnVouchers");
        boolean multiCurrency = (Boolean) s.get("HasMultiCurrencyFeature");
        boolean freightToExpenses = (Boolean) s.get("DebitAmountChargetoExpenseAcFreightGrid");
        boolean commissionToExpenses = (Boolean) s.get("DebitAmountChargetoExpenseAcOfCommission");
        String creditInItemSaleGlRaw = (String) s.get("CreditAmountInItemSaleGLRaw");
        Map<String, Boolean> rights = repo.rights(u, context.currentRoleName(), SCREEN_NAME);

        int id = i(req.get("Id"));
        if (!Boolean.TRUE.equals(rights.get(id > 0 ? "Update" : "Save")))
            throw new SecurityException("You do not have the " + (id > 0 ? "Update" : "Save") + " right on Sale Invoice");
        Map<String, Object> existing = null;
        if (id > 0) {
            existing = read(id);
            if (b(col(existing, "IsApproved"))) throw new IllegalStateException("Record cannot be updated because Record has approved");
        }

        List<Map<String, Object>> grid = rows(req.get("details"));
        List<Map<String, Object>> freightGrid = rows(req.get("freights"));
        List<Map<String, Object>> glGrid = rows(req.get("journals"));
        List<Map<String, Object>> expGrid = rows(req.get("expenses"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        /* The hidden GlAccountId cells (SupCustIdUpdateforFrieghtGrid / ...GLGrid): resolved here from the displayed title. */
        List<Map<String, Object>> accounts = accounts(u, subsidiary, b(req.get("AccountsAfterRefresh")) && freightToExpenses);
        String valueKey = subsidiary ? "SupplierCustomerId" : "Id";
        for (var r : freightGrid) r.put("GlAccountId", glByTitle(accounts, titleOf(accounts, valueKey, i(r.get("Transporter"))), i(r.get("GlAccountId"))));
        for (var r : glGrid) r.put("GlAccountId", glByTitle(accounts, titleOf(accounts, valueKey, i(r.get("AccountId"))), i(r.get("GlAccountId"))));

        /* Transporter grid (:1905). */
        Set<Integer> forFreight = new HashSet<>(), forDebit = new HashSet<>();
        int rowNo = 0;
        for (var r : freightGrid) {
            rowNo++;
            int transporter = i(r.get("Transporter"));
            double freight = d(r.get("Freight")), debit = d(r.get("Debit"));
            if (freight > 0 && freightToExpenses) forFreight.add(transporter);
            if (debit > 0 && freightToExpenses) forDebit.add(transporter);
            if (freight > 0 && transporter == 0) throw new IllegalArgumentException("Please Select an Account Against Freight in Transporter Grid (row No: " + rowNo + ")");
            if (transporter > 0 && freight == 0 && debit == 0) throw new IllegalArgumentException("Freight Required when Account Exists in Transporter Grid (row No: " + rowNo + ")");
        }
        if (freightToExpenses) for (int t : forFreight) if (forDebit.contains(t)) throw new IllegalArgumentException("Transporter for Freight Cannot be Same as Transporter for Debit.");
        rowNo = 0;
        for (var r : glGrid) {
            rowNo++;
            if ((d(r.get("Credit")) > 0 || ci(r.get("Debit")) > 0) && i(r.get("AccountId")) == 0)
                throw new IllegalArgumentException("Please Select an Account  in JL Grid (row No: " + rowNo + " )");
            if (d(r.get("Credit")) == 0 && ci(r.get("Debit")) == 0 && i(r.get("AccountId")) > 0)
                throw new IllegalArgumentException("Please add Value in Debit or Credit in JL Grid (row No: " + rowNo + " )");
        }
        rowNo = 0;
        for (var r : expGrid) {
            rowNo++;
            if (d(r.get("Amount")) > 0 && i(r.get("ItemId")) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense Grid (row No: " + rowNo + " )");
        }

        /* FormValidation (:979). */
        int docNo = i(req.get("DocNo"));
        if (docNo == 0) throw new IllegalArgumentException("DocNo  field is Required");
        int currencyId = i(req.get("CurrencyId"));
        BigDecimal exchangeRate = dec(req.get("ExchangeRate"));
        if (multiCurrency) {
            if (currencyId == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (exchangeRate.signum() == 0) throw new IllegalArgumentException("Exchange Rate Field is Required");
        } else {
            if (currencyId == 0) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (exchangeRate.signum() == 0) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }
        int customerId = i(req.get("SupplierCustomerId"));
        Map<String, Object> customer = repo.customers(u, subsidiary).stream().filter(r -> i(r.get("Id")) == customerId).findFirst().orElse(null);
        if (customer == null) throw new IllegalArgumentException("Customer Name field is Required");
        int customerGl = i(customer.get("GlAccountId"));
        /* grdGLedger_CellUpdated (:1350) - the customer's own account is refused on the Party Add/Less grid. */
        if (!subsidiary) for (var r : glGrid) if (i(r.get("AccountId")) != 0 && i(r.get("AccountId")) == customerGl) throw new IllegalArgumentException("Customer Account Not select");

        /* Header commission controls. */
        int commAgent = i(req.get("CommissionAgentId"));
        String commTypeText = s(req.get("CommissionType")).trim();
        String commRateText = s(req.get("CommRate")).trim();
        String commUomText = s(req.get("CommUom")).trim();
        String commAmountText = s(req.get("CommAmount")).trim();
        int commDebitAccount = commissionToExpenses ? i(req.get("CommissionDebitAccountId")) : 0;
        if (commAgent != 0 && repo.customers(u, subsidiary).stream().noneMatch(r -> i(r.get("Id")) == commAgent))
            throw new IllegalArgumentException("Commission Agent not found");
        if (commDebitAccount != 0 && commissionDebitAccounts(u, subsidiary).stream().noneMatch(r -> i(r.get("Id")) == commDebitAccount))
            throw new IllegalArgumentException("Commission Debit Ac not found");
        if (d(commAmountText) > 0 && commissionToExpenses && commDebitAccount == 0) throw new IllegalArgumentException("Commission Debit Ac field is Required");
        if (d(commAmountText) > 0 && commAgent == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");
        if (d(commAmountText) == 0 && commAgent > 0) throw new IllegalArgumentException("Commission Amount Required when Commission Agent is Selected");

        /* The grid rows the desktop reads, checked against their source so the browser cannot change a GDN line. */
        verifyLines(u, grid, id, existing, customerId);

        /* Insert :1985 - TotalCommissionAmount, FreightProportion, ExpProportion, CommissionProportion,
           txtExchangeRate_TextChanged (-> BillAmount -> BillProportion). LedgerProportion keeps Journal current. */
        commAmountText = totalCommissionAmount(grid, commTypeText, commRateText, commUomText, commAmountText);
        freightProportion(grid, freightGrid, freightToExpenses);
        expProportion(grid, expGrid);
        commissionProportion(grid, commTypeText, commRateText, commAmountText, commissionToExpenses);
        ledgerProportion(grid, glGrid, commAmountText);
        double rate = exchangeRate.doubleValue();
        for (var r : grid) r.put("FcyAmount", rate > 0 ? d(r.get("ItemAmount")) / rate : 0d);
        BillResult bill = billAmount(grid, freightGrid, glGrid, expGrid, customerId, customerGl, commAgent, commRateText, commAmountText);
        commAmountText = bill.commAmountText;
        billProportion(grid, creditInItemSaleGlRaw);

        LocalDateTime now = LocalDateTime.now();
        Head h = new Head();
        h.Id = id;
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.FinancialYearId = year;
        h.BranchesId = n(u.getBranchesId());
        h.ProjectsId = n(u.getBranchesId());
        h.DocumentTypeId = DOCUMENT_TYPE_ID;
        h.ScreenName = SCREEN_NAME;
        h.EntryUser = u.getId();
        h.EntryDate = now;
        h.ModifyDate = now;
        h.ModifyUser = u.getId();
        h.DocNo = docNo;
        h.DocDate = dt(req.get("DocDate"));
        if (h.DocDate == null) throw new IllegalArgumentException("Doc Date is required");
        h.SupplierCustomerId = customerId;
        h.SupplierInvoiceDate = now;
        h.SupplierInvoiceNo = i(s(req.get("ManualBillNo")));
        h.SupplierReferenceNo = String.valueOf(docNo).trim();
        h.ManualBillNo = s(req.get("ManualBillNo")).trim();
        h.DueDays = i(s(req.get("DueDays")).trim());
        h.DueDate = dt(req.get("DueDate"));
        if (h.DueDate == null) h.DueDate = now;
        h.BillAmount = bill.header;
        h.RemarksHeader = s(req.get("Remarks")).trim();
        h.ReferencePartyId = 0;
        h.StockPartyId = 0;
        h.CommissionAgentId = commAgent;
        h.CommissionType = commTypeText;
        h.CommRate = d(commRateText);
        h.UomScheduleIdCmRate = commUomText;
        h.CommAmount = d(commAmountText);
        h.CommissionRemarks = s(req.get("CommissionRemarks")).trim();
        h.CommissionDebitAcGLId = commDebitAccount;
        h.CurrencyId = currencyId;
        h.ExchangeRate = exchangeRate;
        /* txtFcyAmount = (BillAmount / rate).ToString("#,##0.####") while the grid has rows. */
        h.FcyAmount = rate == 0 ? BigDecimal.ZERO : new BigDecimal(bill.raw / rate).setScale(4, RoundingMode.HALF_UP);
        if (multiCurrency && h.FcyAmount.signum() == 0) throw new IllegalArgumentException("Fcy Amount Field is Required");
        h.AttachmentsValues = id > 0 ? nullable(req.get("AttachmentsValues")) : "";
        h.CustomAttachmentsValues = id > 0 ? nullable(req.get("CustomAttachmentsValues")) : "";

        SaleInvoiceFinancial.Invoice inv = new SaleInvoiceFinancial.Invoice();
        inv.h = h;
        int line = 0;
        for (var r : grid) {
            Detail p = new Detail();
            p.LineId = ++line;
            p.CropYear = s(r.get("CropYear"));
            if (p.CropYear.isEmpty()) throw new IllegalArgumentException("CropYear Required");
            p.GrossWeight = d(r.get("GrossWeight"));
            p.RefRefDocumentTypeId = i(r.get("RefRefDocumentTypeId"));
            p.RefRefDocIdNo = i(r.get("RefRefDocIdNo"));
            p.RefDocSubId = i(r.get("RefDocSubId"));
            p.SaleOrderId = i(r.get("SaleOrderId"));
            p.InvGdnDetailId = i(r.get("Id"));
            if (p.InvGdnDetailId == 0) throw new IllegalArgumentException("InvGdnDetailId not found");
            p.InvGdnId = i(r.get("InvGdnId"));
            if (p.InvGdnId == 0) throw new IllegalArgumentException("InvGdnId not found");
            p.ItemId = i(r.get("ItemId"));
            if (p.ItemId == 0) throw new IllegalArgumentException("Item Required");
            p.ItemQty = d(r.get("ItemQty"));
            if (p.ItemQty == 0) throw new IllegalArgumentException("ItemQty mustbe greater than zero");
            p.ItemUOMId = i(r.get("ItemUomId"));
            if (p.ItemUOMId == 0) throw new IllegalArgumentException("ItemUOMId Field Required");
            p.JobLotId = i(r.get("JobLotId"));
            if (p.JobLotId == 0) throw new IllegalArgumentException("JobLot Required");
            p.NetBillWeight = d(r.get("NetBillWeight"));
            if (p.NetBillWeight == 0) throw new IllegalArgumentException("NetBillWeight Field Required");
            p.NetStockWeight = d(r.get("StockWeight"));
            if (p.NetStockWeight == 0) throw new IllegalArgumentException("NetStockWeight Field Required");
            p.EBTotalWt = d(r.get("EmptyBagsDeduction"));
            p.EBWeight = d(r.get("EBWPerUnit"));
            p.PackingTypeId = i(r.get("PackingTypeId"));
            if (p.PackingTypeId == 0) throw new IllegalArgumentException("PackingType Required");
            p.ItemRate = d(r.get("ItemRate"));
            if (p.ItemRate == 0) throw new IllegalArgumentException("ItemRate Field Required");
            if (i(r.get("OrderItemRateUOMId")) > 0) p.UomScheduleIdRate = i(r.get("OrderItemRateUOMId"));
            else {
                var uoms = repo.uomScheduleByItem(u.getOrganizationId(), u.getCompanyId(), p.ItemId);
                if (!uoms.isEmpty()) {
                    int eq = ci(r.get("RateUOM"));
                    var match = uoms.stream().filter(x -> d(col(x, "Equivalent")) == eq).findFirst().orElse(null);
                    if (match == null) throw new IllegalArgumentException("this RateUom not define please check");
                    p.UomScheduleIdRate = i(col(match, "Id"));
                }
            }
            if (p.UomScheduleIdRate == 0) throw new IllegalArgumentException("Rate UOM Required");
            p.RateUOM = ci(r.get("RateUOM"));
            p.RateCut = d(r.get("RateCut"));
            p.RateCutAmount = d(r.get("RateCutAmount"));
            p.ItemAmount = d(r.get("ItemAmount"));
            if (p.ItemAmount == 0) throw new IllegalArgumentException("ItemAmount Field Required");
            p.WarehouseId = i(r.get("WarehouseId"));
            if (p.WarehouseId == 0) throw new IllegalArgumentException("Warehouse Required");
            p.BillAmount = d(r.get("BillAmount"));
            p.FreightAmount = d(r.get("Freights"));
            p.JournalAmount = d(r.get("Journal"));
            p.ExpenseAmount = d(r.get("Expense"));
            p.CommissionAmount = d(r.get("Commission"));
            p.GpDate = now;
            inv.details.add(p);
        }
        for (var r : freightGrid) {
            if (i(r.get("Transporter")) == 0) continue;
            Freight f = new Freight();
            f.InvGdnId = i(r.get("InvGdnId"));
            f.TansporterId = i(r.get("GlAccountId"));
            f.TransporterSupCustId = subsidiary ? i(r.get("Transporter")) : 0;
            f.FreightAmount = ci(r.get("Freight"));
            f.Debit = ci(r.get("Debit"));
            f.Remarks = s(r.get("Remarks"));
            if (f.Remarks.isEmpty()) throw new IllegalArgumentException("Remarks Field Requried in Freight Grid");
            inv.freights.add(f);
        }
        List<Map<String, Object>> otherItems = repo.otherItems(u);
        for (var r : expGrid) {
            if (i(r.get("ItemId")) == 0) continue;
            if (otherItems.stream().noneMatch(o -> i(col(o, "Id")) == i(r.get("ItemId")))) throw new IllegalArgumentException("Expense item not found");
            Expense e = new Expense();
            e.InvRevExpItemId = i(r.get("ItemId"));
            e.Qty = ci(r.get("Qty"));
            e.Rate = d(r.get("Rate"));
            e.Amount = d(r.get("Amount"));
            e.Remarks = s(r.get("Remarks"));
            inv.expenses.add(e);
        }
        for (var r : glGrid) {
            if (i(r.get("AccountId")) == 0) continue;
            Journal j = new Journal();
            j.ChartofAccountId = i(r.get("GlAccountId"));
            j.TransporterSupCustId = subsidiary ? i(r.get("AccountId")) : 0;
            j.JvRemarks = s(r.get("Remarks"));
            j.JvPrcnt = d(r.get("Percentage"));
            j.JvQty = d(r.get("Qty"));
            j.JvRate = d(r.get("Rate"));
            j.JvDebit = d(r.get("Debit"));
            j.JvCredit = d(r.get("Credit"));
            if (j.JvRemarks.isEmpty()) throw new IllegalArgumentException("Remarks Field Requried in GL Grid");
            inv.journals.add(j);
        }
        /* Insert :2235 - one commission row built from the header controls. */
        if (commAgent != 0 && d(commAmountText) > 0) {
            Commission c = new Commission();
            c.Id = 0;
            c.SaleOrderId = inv.details.get(0).SaleOrderId;
            c.InvSaleInvoiceId = h.Id;
            if (subsidiary) {
                c.DebitAccountId = commDebitAccount;
                c.CommDebitSupCustId = repo.supplierCustomerIdByGl(u, c.DebitAccountId);
            } else {
                c.CommDebitSupCustId = 0;
                c.DebitAccountId = commDebitAccount;
            }
            c.CommissionAgentId = commAgent;
            /* cmbcommtype.Value is the list Id when the text is in the list, otherwise the text itself. */
            Object typeValue = commTypeValue(commTypeText);
            String tv = String.valueOf(typeValue);
            if ("Percent".equals(tv)) c.CommType = 2.0;
            if ("Flat".equals(tv)) c.CommType = 1.0;
            if ("Comm Weight".equals(tv)) c.CommType = 3.0;
            if (d(typeValue) > 0) c.CommType = d(typeValue);
            c.Rate = d(commRateText);
            c.RateUom = d(commUomText);
            c.CommAmount = d(commAmountText);
            c.Remarks = s(req.get("CommissionRemarks"));
            inv.commissions.add(c);
        }

        /* BLL 0580 Save. */
        if (h.Id == 0) h.ModifyUser = 0; else h.EntryUser = 0;
        SaleInvoiceFinancial.Voucher voucher = new SaleInvoiceFinancial(repo).makeVoucherForSaleInvoice(inv);
        int saved = shared.persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("voucherHeadId", repo.voucherHeadId(h.OrganizationId, h.CompanyId, DOCUMENT_TYPE_ID, saved));
        out.put("message", (id == 0 ? "Record saved Successfully" : "Record Update Successfully") + h.DocNo);
        return out;
    }

    /**
     * The desktop trusts the grid it filled from usp_getGdnDataForSaleInvoiceWithoutWeightBridgeByIds (new)
     * or from ReadById (edit). The web re-reads that source: identity, quantities and weights must be the
     * source's, a sale-order line keeps the order's rate and rate UOM (the columns are read-only there), and
     * ItemAmount must be what grd_CellUpdated gives for the sent rate/UOM/cut - or the loaded amount when
     * nothing was edited.
     */
    private void verifyLines(UserAccount u, List<Map<String, Object>> grid, int invoiceId, Map<String, Object> existing, int customerId) {
        List<Integer> ids = grid.stream().map(r -> i(r.get("InvGdnId"))).distinct().collect(Collectors.toList());
        requireOwnGdns(u, ids);
        Map<String, Map<String, Object>> source = new HashMap<>();
        for (var r : repo.gdnLoadWithoutWb(ids.stream().map(x -> "," + x).collect(Collectors.joining()))) {
            if (i(col(r, "SupplierCustomerId")) != customerId) throw new IllegalArgumentException("Another Customer already Exists ");
            source.put(i(col(r, "InvGdnId")) + ":" + i(col(r, "Id")), sourceFromLoad(r));
        }
        /* An edit re-sends the lines ReadById put on the grid: their saved rate, UOM and amount are also "as loaded". */
        Map<String, Map<String, Object>> saved = new HashMap<>();
        if (invoiceId > 0) for (var r : rowsOf(existing.get("details"))) saved.put(i(col(r, "InvGdnId")) + ":" + i(col(r, "InvGdnDetailId")), sourceFromSaved(r));
        boolean soLocked = i(grid.get(0).get("SaleOrderId")) > 0;
        Set<String> seen = new HashSet<>();
        for (var r : grid) {
            String key = i(r.get("InvGdnId")) + ":" + i(r.get("Id"));
            Map<String, Object> src = source.get(key), old = saved.get(key);
            if (src == null) throw new IllegalArgumentException("GDN line " + i(r.get("Id")) + " is not a line of the loaded GDN");
            if (!seen.add(key)) throw new IllegalArgumentException("GDN line " + i(r.get("Id")) + " is loaded twice");
            for (String k : List.of("ItemId", "ItemUomId", "WarehouseId", "PackingTypeId", "SaleOrderId", "JobLotId", "RefRefDocumentTypeId", "RefRefDocIdNo", "RefDocSubId"))
                if (i(r.get(k)) != i(src.get(k))) throw new IllegalArgumentException("GDN line " + i(r.get("Id")) + ": " + k + " does not match the GDN");
            for (String k : List.of("ItemQty", "GrossWeight", "EmptyBagsDeduction", "EBWPerUnit", "NetBillWeight", "StockWeight"))
                if (Math.abs(d(r.get(k)) - d((old != null ? old : src).get(k))) > 1e-6) throw new IllegalArgumentException("GDN line " + i(r.get("Id")) + ": " + k + " does not match the GDN");
            r.put("CropYear", (old != null ? old : src).get("CropYear"));
            if (soLocked) {
                Map<String, Object> rateSrc = old != null ? old : src;
                if (Math.abs(d(r.get("ItemRate")) - d(rateSrc.get("ItemRate"))) > 1e-9 || Math.abs(d(r.get("RateUOM")) - d(rateSrc.get("RateUOM"))) > 1e-9)
                    throw new IllegalArgumentException("GDN line " + i(r.get("Id")) + ": the sale order rate cannot be changed");
                r.put("OrderItemRateUOMId", rateSrc.get("OrderItemRateUOMId"));
            }
            /* grd_CellUpdated reads RateUOM from the cell Text, formatted "0;(0);0". */
            double w = d(r.get("NetBillWeight")), eq = d(r.get("RateUOM")) < 0 ? 0 : SaleInvoiceFinancial.roundAway(d(r.get("RateUOM")), 0);
            double cutAmount = w / eq * d(r.get("RateCut"));
            double expected = w / eq * d(r.get("ItemRate")) - cutAmount;
            double amount = d(r.get("ItemAmount"));
            boolean edited = Math.abs(amount - expected) <= 0.005;
            boolean asLoaded = d(r.get("RateCut")) == 0 && Math.abs(amount - d(src.get("ItemAmount"))) <= 0.005;
            boolean asSaved = old != null && Math.abs(amount - d(old.get("ItemAmount"))) <= 0.005 && Math.abs(d(r.get("RateCut")) - d(old.get("RateCut"))) <= 1e-9;
            if (!edited && !asLoaded && !asSaved) throw new IllegalArgumentException("GDN line " + i(r.get("Id")) + ": ItemAmount does not match its rate");
            if (edited) r.put("RateCutAmount", cutAmount);
            else if (asSaved) r.put("RateCutAmount", old.get("RateCutAmount"));
            else r.put("RateCutAmount", 0d);
        }
    }

    private static Map<String, Object> sourceFromLoad(Map<String, Object> r) {
        Map<String, Object> x = new HashMap<>();
        x.put("ItemId", col(r, "ItemId")); x.put("ItemUomId", col(r, "ItemUomId")); x.put("WarehouseId", col(r, "WarehouseId"));
        x.put("PackingTypeId", col(r, "PackingTypeId")); x.put("SaleOrderId", col(r, "SaleOrderId")); x.put("JobLotId", col(r, "JobLotId"));
        x.put("RefRefDocumentTypeId", col(r, "RefDocumentTypeId")); x.put("RefRefDocIdNo", col(r, "RefDocIdNo")); x.put("RefDocSubId", col(r, "RefDocSubIdNo"));
        x.put("ItemQty", col(r, "ItemQty")); x.put("GrossWeight", col(r, "GrossWeight")); x.put("EmptyBagsDeduction", col(r, "EBWTotal"));
        x.put("EBWPerUnit", col(r, "EBWPerUnit")); x.put("NetBillWeight", col(r, "NetBillWeight")); x.put("StockWeight", col(r, "StockWeight"));
        x.put("CropYear", s(col(r, "CropYear")));
        /* dtGrid.ItemRate is typeof(int): the order rate lands in the grid rounded half-to-even. */
        x.put("ItemRate", (double) Math.round(Math.rint(d(col(r, "OrderItemRate")))));
        x.put("RateUOM", d(col(r, "EquivalentPoRate")));
        x.put("OrderItemRateUOMId", col(r, "OrderItemRateUOMId"));
        x.put("ItemAmount", col(r, "ItemAmount"));
        return x;
    }

    private static Map<String, Object> sourceFromSaved(Map<String, Object> r) {
        Map<String, Object> x = new HashMap<>();
        x.put("ItemId", col(r, "ItemId")); x.put("ItemUomId", col(r, "ItemUOMId")); x.put("WarehouseId", col(r, "WarehouseId"));
        x.put("PackingTypeId", col(r, "PackingTypeId")); x.put("SaleOrderId", col(r, "SaleOrderId")); x.put("JobLotId", col(r, "JobLotId"));
        x.put("RefRefDocumentTypeId", col(r, "RefRefDocumentTypeId")); x.put("RefRefDocIdNo", col(r, "RefRefDocIdNo")); x.put("RefDocSubId", col(r, "RefDocSubId"));
        x.put("ItemQty", col(r, "ItemQty")); x.put("GrossWeight", col(r, "GrossWeight")); x.put("EmptyBagsDeduction", col(r, "EBTotalWt"));
        x.put("EBWPerUnit", col(r, "EBWeight")); x.put("NetBillWeight", col(r, "NetBillWeight")); x.put("StockWeight", col(r, "NetStockWeight"));
        x.put("CropYear", s(col(r, "CropYear")));
        x.put("ItemRate", (double) Math.round(Math.rint(d(col(r, "ItemRate")))));
        x.put("RateUOM", d(col(r, "RateUOM")));
        x.put("OrderItemRateUOMId", col(r, "UomScheduleIdRate"));
        x.put("ItemAmount", col(r, "ItemAmount"));
        x.put("RateCut", col(r, "RateCut"));
        x.put("RateCutAmount", col(r, "RateCutAmount"));
        return x;
    }

    // ============================================================================== the form's proportions

    /** TotalCommissionAmount (:3300). Returns the new txtcommamount text. */
    static String totalCommissionAmount(List<Map<String, Object>> grid, String type, String rateText, String uomText, String amountText) {
        if (rateText.isEmpty()) return "0";
        double rate = d(rateText);
        String out = amountText;
        if ("Flat".equals(type)) out = SaleInvoiceFinancial.g(rate);
        if ("Percent".equals(type) || "Percentage".equals(type))
            out = SaleInvoiceFinancial.g(Math.rint(grid.stream().mapToDouble(r -> d(r.get("ItemAmount"))).sum() * rate / 100.0));
        if ("Comm Weight".equals(type))
            out = SaleInvoiceFinancial.g(Math.rint(grid.stream().mapToDouble(r -> d(r.get("NetBillWeight"))).sum() / d(uomText) * rate));
        return out;
    }

    /** FreightProportion (:3183) - Math.Round(Credit) / NetWeight * Weight. */
    static void freightProportion(List<Map<String, Object>> grid, List<Map<String, Object>> freightGrid, boolean freightToExpenses) {
        double w = grid.stream().mapToDouble(r -> d(r.get("NetBillWeight"))).sum();
        double credit = freightGrid.stream().mapToDouble(r -> d(r.get("Freight"))).sum();
        for (var r : grid) r.put("Freights", credit > 0 && !freightToExpenses ? Math.rint(credit) / w * d(r.get("NetBillWeight")) : 0d);
    }

    /** ExpProportion (:3159) - by NetBillWeight, through double.ToString(). */
    static void expProportion(List<Map<String, Object>> grid, List<Map<String, Object>> expGrid) {
        double total = expGrid.stream().mapToDouble(r -> d(r.get("Amount"))).sum();
        double w = grid.stream().mapToDouble(r -> d(r.get("NetBillWeight"))).sum();
        for (var r : grid) r.put("Expense", total > 0 ? viaString(total / w * d(r.get("NetBillWeight"))) : 0d);
    }

    /** CommissionProportion (:3264). */
    static void commissionProportion(List<Map<String, Object>> grid, String type, String rateText, String amountText, boolean commissionToExpenses) {
        double total = d(amountText), pct = d(rateText);
        double w = grid.stream().mapToDouble(r -> d(r.get("NetBillWeight"))).sum();
        for (var r : grid) {
            if (total > 0 && !commissionToExpenses) {
                if ("Percent".equals(type)) r.put("Commission", d(r.get("ItemAmount")) * pct / 100.0);
                else r.put("Commission", viaString(total / w * d(r.get("NetBillWeight"))));
            } else r.put("Commission", 0d);
        }
    }

    /** LedgerProportion (:3207): only a debit excess over (credit + commission) is spread by weight. */
    static void ledgerProportion(List<Map<String, Object>> grid, List<Map<String, Object>> glGrid, String amountText) {
        double credit = glGrid.stream().mapToDouble(r -> d(r.get("Credit"))).sum() + d(amountText);
        double debit = glGrid.stream().mapToDouble(r -> d(r.get("Debit"))).sum();
        double w = grid.stream().mapToDouble(r -> d(r.get("NetBillWeight"))).sum();
        for (var r : grid) r.put("Journal", debit > credit ? (debit - credit) / w * d(r.get("NetBillWeight")) : 0d);
    }

    /** BillProportion (:3246). An unparseable config value made bool.Parse throw inside the try: the rows kept their value. */
    static void billProportion(List<Map<String, Object>> grid, String raw) {
        for (var r : grid) {
            double full = d(r.get("ItemAmount")) + d(r.get("Expense")) - d(r.get("Commission"));
            if (raw == null || raw.isEmpty()) { r.put("BillAmount", full); continue; }
            String t = raw.trim().toLowerCase(Locale.ROOT);
            if (t.equals("true")) r.put("BillAmount", full);
            else if (t.equals("false")) r.put("BillAmount", d(r.get("ItemAmount")));
            else return;
        }
    }

    static final class BillResult { double raw; double header; String commAmountText; }

    /** BillAmount (:3082). txtBillAmount = Math.Round(BillAmount).ToString("0,0"), which Insert reads back. */
    static BillResult billAmount(List<Map<String, Object>> grid, List<Map<String, Object>> freightGrid, List<Map<String, Object>> glGrid,
                                 List<Map<String, Object>> expGrid, int customerId, int customerGl, int commAgent, String rateText, String amountText) {
        double item = grid.stream().mapToDouble(r -> d(r.get("ItemAmount"))).sum();
        double exp = expGrid.stream().mapToDouble(r -> d(r.get("Amount"))).sum();
        double jd = 0, jc = 0, tc = 0;
        for (var r : glGrid) if (i(r.get("AccountId")) > 0) { jd += d(r.get("Debit")); jc += d(r.get("Credit")); }
        /* txtSupplierGLId.Text == Transporter.ToString(): the customer's GL id against the cell value, with or without feature 4. */
        for (var r : freightGrid) if (i(r.get("Transporter")) > 0 && i(r.get("Transporter")) == customerGl) tc += d(r.get("Freight"));
        double bill = item + exp;
        if (tc > 0) bill -= Math.abs(tc);
        double gl = jc - jd;
        bill = !(gl < 0) ? bill + gl : bill - Math.abs(gl);
        BillResult out = new BillResult();
        out.commAmountText = amountText;
        if (customerId == commAgent) {
            if (!rateText.isEmpty()) bill -= d(amountText);
            else out.commAmountText = "0";
        }
        out.raw = bill;
        out.header = Math.rint(bill);
        return out;
    }

    /** Convert.ToInt32 of a double cell: half-to-even. */
    static int ci(Object o) { return (int) Math.rint(d(o)); }

    /** A double written into a DataTable cell through ToString() (.NET Framework "G" = 15 significant digits). */
    static double viaString(double v) { return Double.parseDouble(SaleInvoiceFinancial.g(v)); }

    private static final String[] COMM_TYPES = {"Flat", "Percent", "Comm Weight"};

    /** CommissionTypeFill (:570): Ids 1..3; an unmatched Text leaves the editor value as the text. */
    static Object commTypeValue(String text) {
        for (int k = 0; k < COMM_TYPES.length; k++) if (COMM_TYPES[k].equals(text)) return k + 1;
        return text;
    }

    private static String titleOf(List<Map<String, Object>> list, String key, int value) {
        if (value == 0) return null;
        for (var r : list) if (i(r.get(key)) == value) return s(r.get("AccountTitle"));
        return null;
    }
    private static int glByTitle(List<Map<String, Object>> accounts, String title, int previous) {
        if (title == null || title.isEmpty() || "0".equals(title)) return previous;
        for (var r : accounts) if (title.equals(s(r.get("AccountTitle")))) return i(r.get("Id"));
        return previous;
    }
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rows(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) for (Object x : (List<Object>) o) if (x instanceof Map) out.add(new LinkedHashMap<>((Map<String, Object>) x));
        return out;
    }
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rowsOf(Object o) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (o instanceof List) for (Object x : (List<Object>) o) if (x instanceof Map) out.add((Map<String, Object>) x);
        return out;
    }
    private static String nullable(Object o) { return o == null ? null : String.valueOf(o); }
}
