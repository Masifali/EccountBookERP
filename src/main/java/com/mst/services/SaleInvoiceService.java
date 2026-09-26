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
 * Sale Invoice (95): InvfrmSaleInvoice.Insert (:4054), BLL 0580 InvSaleInvoice.Save and DAL 0433
 * InvSaleInvoice.SetData, in that order, inside one transaction.
 *
 * The browser's grids are only the desktop's DataTables. Everything Insert() validates is validated
 * again here, every proportion Insert() recomputes before reading the grids is recomputed here, and the
 * voucher is built here from the same rules - so a crafted request cannot post an entry the desktop
 * form could not have posted.
 */
@Service
public class SaleInvoiceService {
    private final SaleInvoiceRepository repo;
    private final CurrentUserContext context;

    public SaleInvoiceService(SaleInvoiceRepository repo, CurrentUserContext context) {
        this.repo = repo;
        this.context = context;
    }

    /** The configuration and features the form reads in InitializeComponentMethod / GetConfigurationsFromGlobal. */
    public Map<String, Object> settings(UserAccount u) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("BranchFeature", repo.feature(u, 11));
        m.put("SubsidiaryAccountAllownOnVouchers", repo.feature(u, 4));
        m.put("HasMultiCurrencyFeature", repo.feature(u, 6));
        for (String key : List.of("RateCutAndRateCutAmountAddOnInvoice", "CommissionEditableOnInvoice", "EnableItemWiseCommOnSale",
                "CommissionPolicyIsActive", "IsStockReservedPerParty", "CommissionPolicyForCommissionAgentIsActive", "SaleInvoiceBranchWise",
                "CreditAmountInItemSaleGL"))
            m.put(key, truthy(repo.config(u, key)));
        /* grd_CellUpdated reads the raw row: absent -> ItemAmount + Expense - Commission; present -> bool.Parse. */
        m.put("CreditAmountInItemSaleGLRaw", repo.config(u, "CreditAmountInItemSaleGL").trim());
        m.put("DebitAmountChargetoExpenseAcFreightGrid", truthy(repo.config(u, "DebitAmountChargetoExpenseAcFreightGrid")));
        m.put("DebitAmountChargetoExpenseAcOfCommission", truthy(repo.config(u, "DebitAmountChargetoExpenseAcOfCommission")));
        m.put("DefaultDaysToLessFromHistoryFromDate", i(repo.config(u, "DefaultDaysToLessFromHistoryFromDate").trim()));
        m.put("BaseCurrencyId", i(repo.config(u, "Base Currency").trim()));
        m.put("BaseCurrencyRate", d(repo.config(u, "BaseCurrencyRate").trim()));
        m.put("DefaultNoofDecimalPointsForAmount", decimals(u, "DefaultNoofDecimalPointsForAmount", 0));
        return m;
    }

    /** clsGlobalVariables.DefaultNoofDecimalPointsForAmount - the configured count (0 when unset, as Conversion.ToInt gives). */
    private int decimals(UserAccount u, String key, int fallback) {
        String v = repo.config(u, key).trim();
        return v.isEmpty() ? fallback : i(v);
    }

    /** Everything the form binds when it opens (InitializeComponentMethod + btnRefresh_Click). */
    public Map<String, Object> lookups() {
        UserAccount u = context.requireAccountingUser();
        int year = context.currentFinancialYearId();
        Map<String, Object> s = settings(u);
        boolean subsidiary = (Boolean) s.get("SubsidiaryAccountAllownOnVouchers");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("settings", s);
        m.put("rights", repo.rights(u, context.currentRoleName()));
        m.put("docNo", repo.nextDocNo(u, year));
        m.put("branchSrNo", (Boolean) s.get("BranchFeature") ? repo.nextBranchSrNo(u, year) : 0);
        m.put("locationTypes", repo.locationTypes());
        m.put("currencies", repo.currencies(u));
        m.put("customers", repo.customers(u, subsidiary));
        m.put("accounts", repo.accounts(u, subsidiary, (Boolean) s.get("DebitAmountChargetoExpenseAcFreightGrid")));
        m.put("commissionDebitAccounts", (Boolean) s.get("DebitAmountChargetoExpenseAcOfCommission") && !subsidiary
                ? repo.commissionDebitAccounts(u) : List.of());
        m.put("otherItems", repo.otherItems(u));
        m.put("commissionUoms", repo.commissionUoms());
        m.put("paymentTerms", repo.paymentTerms(u));
        m.put("historyBranches", repo.historyBranches(u, (Boolean) s.get("SaleInvoiceBranchWise")));
        m.put("historyCustomers", repo.historyCustomers(u));
        m.put("userBranchName", repo.branchName(u));
        m.put("userBranchId", n(u.getBranchesId()));
        LocalDateTime start = repo.financialYearStart(u, year);
        m.put("yearStart", start == null ? null : start.toLocalDate().toString());
        return m;
    }

    public Map<String, Object> nextCodes() {
        UserAccount u = context.requireAccountingUser();
        int year = context.currentFinancialYearId();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", repo.nextDocNo(u, year));
        m.put("branchSrNo", repo.feature(u, 11) ? repo.nextBranchSrNo(u, year) : 0);
        return m;
    }

    // ============================================================================== Load GDN

    /**
     * LoadDataDetailGridAgainstGP, LoadFreightData, LoadExpensesFromGdn / LoadExpData and
     * SaleOrderPaymentTermDetailBySoIds for the GDNs chosen in frmLoadGDN. The row checks that BtnLoad
     * runs (same customer, same document type, same order, same cost centre, same reserved status) are
     * repeated here from the database rows, not trusted from the browser.
     */
    public Map<String, Object> loadGdns(List<Integer> gdnIds, Integer currentCustomerId, String loadedReserveStatus) {
        UserAccount u = context.requireAccountingUser();
        if (gdnIds == null || gdnIds.isEmpty()) throw new IllegalArgumentException("Check the row first");
        String ids = gdnIds.stream().map(x -> "," + x).collect(Collectors.joining());
        var rows = repo.gdnLoad(ids);
        Map<String, Object> out = new LinkedHashMap<>();
        if (rows.isEmpty()) { out.put("details", List.of()); return out; }
        for (var r : rows) {
            if (i(col(r, "OrganizationId")) != 0 && i(col(r, "OrganizationId")) != u.getOrganizationId())
                throw new IllegalArgumentException("GDN is outside the current company");
        }
        int customer = i(col(rows.get(0), "SupplierCustomerId"));
        if (currentCustomerId != null && currentCustomerId > 0 && customer != currentCustomerId)
            throw new IllegalArgumentException("You Can't Load GDN Of Different Customer At Same Time");
        String status = b(col(rows.get(0), "IsStockReserved")) ? "Reserved" : "Not Reserved";
        if (loadedReserveStatus != null && !loadedReserveStatus.isEmpty() && !loadedReserveStatus.equals(status))
            throw new IllegalArgumentException("You Can't Load GDN Of Different Reserved Status! Already Loaded rows of:" + loadedReserveStatus + " Status");
        out.put("reserveStatus", status);
        out.put("customerId", customer);
        out.put("remarks", s(col(rows.get(0), "RemarksHeader")));
        out.put("dueDays", s(col(rows.get(0), "OrderDueDays")));
        out.put("dueDate", col(rows.get(0), "OrderDueDate"));
        out.put("details", rows);
        out.put("freights", repo.freightFromGdn(ids));
        List<Map<String, Object>> exp = repo.expensesFromGdn(ids);
        out.put("expenses", exp);
        out.put("soExpenses", exp.isEmpty() ? repo.itemQtyAndRateFromSo(ids) : List.of());
        return out;
    }

    public List<Map<String, Object>> saleOrderPaymentTerms(List<Integer> orderIds) {
        context.requireAccountingUser();
        if (orderIds == null || orderIds.isEmpty()) return List.of();
        return repo.saleOrderPaymentTerms(orderIds.stream().map(String::valueOf).collect(Collectors.joining(",")));
    }

    public List<Map<String, Object>> commissionPolicy(int customerId, String docDate, String itemIds, int policyTypeId) {
        UserAccount u = context.requireAccountingUser();
        return repo.commissionPolicy(u, customerId, dt(docDate), itemIds == null ? "" : itemIds, policyTypeId);
    }

    public Map<String, Object> ledger(int customerId, String docDate) {
        UserAccount u = context.requireAccountingUser();
        boolean subsidiary = repo.feature(u, 4);
        Map<String, Object> party = repo.customers(u, subsidiary).stream().filter(r -> i(r.get("Id")) == customerId).findFirst().orElse(null);
        Map<String, Object> m = new LinkedHashMap<>();
        if (party == null) { m.put("balance", 0); m.put("glAccountId", 0); return m; }
        int gl = i(party.get("GlAccountId"));
        LocalDateTime start = repo.financialYearStart(u, context.currentFinancialYearId());
        LocalDateTime date = dt(docDate) == null ? LocalDateTime.now() : dt(docDate);
        m.put("glAccountId", gl);
        m.put("balance", repo.ledgerBalance(u, subsidiary, customerId, gl, start == null ? date : start, date));
        return m;
    }

    // ============================================================================== read / delete / history

    public Map<String, Object> read(int id) { return read(id, DOCUMENT_TYPE_ID); }

    /** InvSaleInvoice.GetByID, limited to the signed-in company and to the form's DocumentTypeId. */
    public Map<String, Object> read(int id, int documentTypeId) {
        UserAccount u = context.requireAccountingUser();
        Map<String, Object> r = repo.readById(id);
        if (r == null || i(col(r, "OrganizationId")) != u.getOrganizationId() || i(col(r, "CompanyId")) != u.getCompanyId()
                || i(col(r, "DocumentTypeId")) != documentTypeId)
            throw new NoSuchElementException("Sale Invoice not found");
        r.put("VoucherHeadId", repo.voucherHeadId(u.getOrganizationId(), u.getCompanyId(), documentTypeId, id));
        return r;
    }

    @Transactional
    public void delete(int id) {
        UserAccount u = context.requireAccountingUser();
        if (!Boolean.TRUE.equals(repo.rights(u, context.currentRoleName()).get("Delete"))) throw new SecurityException("You do not have the Delete right on Sale Invoice");
        Map<String, Object> r = read(id);
        if (b(col(r, "IsApproved"))) throw new IllegalStateException("Record cannot be  Delete because Record has approved");
        repo.delete(u, id);
    }

    public List<Map<String, Object>> history(String dateMode, String from, String to, Integer fromDocNo, Integer toDocNo, Integer customerId, String branchIds) {
        UserAccount u = context.requireAccountingUser();
        Map<String, Boolean> rights = repo.rights(u, context.currentRoleName());
        boolean branchWise = truthy(repo.config(u, "SaleInvoiceBranchWise"));
        Set<Integer> allowed = repo.historyBranches(u, branchWise).stream().map(r -> i(r.get("Id"))).collect(Collectors.toSet());
        StringBuilder ids = new StringBuilder();
        for (String p : (branchIds == null ? "" : branchIds).split(",")) {
            int b = i(p);
            if (b > 0 && allowed.contains(b)) ids.append(',').append(b);
        }
        if (ids.length() == 0) throw new IllegalArgumentException("Select branch first");
        return repo.history(u, context.currentFinancialYearId(), Boolean.TRUE.equals(rights.get("CanViewAllRecord")), dateMode,
                dt(from), dt(to), fromDocNo == null ? 0 : fromDocNo, toDocNo == null ? 0 : toDocNo, customerId == null ? 0 : customerId, ids.toString());
    }

    public Map<String, Object> pendingGdns(String from, String to, String branchIds) {
        UserAccount u = context.requireAccountingUser();
        boolean branchWise = truthy(repo.config(u, "SaleInvoiceBranchWise"));
        var branches = repo.gdnBranches(u, branchWise);
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
        m.put("rows", repo.pendingGdn(u, context.currentFinancialYearId(), dt(from), dt(to), ids.toString()));
        return m;
    }

    public List<Map<String, Object>> gdnDetail(int gdnId) { return repo.gdnDetail(context.requireAccountingUser(), gdnId); }

    public Double lastExchangeRate(int currencyId) { return repo.lastExchangeRate(context.requireAccountingUser(), currencyId); }

    // ============================================================================== save (Insert :4054)

    @Transactional
    public Map<String, Object> save(Map<String, Object> req) {
        UserAccount u = context.requireAccountingUser();
        int year = context.currentFinancialYearId();
        Map<String, Object> s = settings(u);
        boolean subsidiary = (Boolean) s.get("SubsidiaryAccountAllownOnVouchers");
        boolean multiCurrency = (Boolean) s.get("HasMultiCurrencyFeature");
        boolean freightDebitToExpenses = (Boolean) s.get("DebitAmountChargetoExpenseAcFreightGrid");
        boolean commissionDebitToExpenses = (Boolean) s.get("DebitAmountChargetoExpenseAcOfCommission");
        boolean policy = (Boolean) s.get("CommissionPolicyIsActive") || (Boolean) s.get("CommissionPolicyForCommissionAgentIsActive");
        boolean creditInItemSaleGl = (Boolean) s.get("CreditAmountInItemSaleGL");
        int amountDp = (Integer) s.get("DefaultNoofDecimalPointsForAmount");
        Map<String, Boolean> rights = repo.rights(u, context.currentRoleName());

        int id = i(req.get("Id"));
        if (!Boolean.TRUE.equals(rights.get(id > 0 ? "Update" : "Save")))
            throw new SecurityException("You do not have the " + (id > 0 ? "Update" : "Save") + " right on Sale Invoice");
        if (id > 0) {
            Map<String, Object> existing = read(id);
            if (b(col(existing, "IsApproved"))) throw new IllegalStateException("Record cannot be updated because Record has approved");
        }

        List<Map<String, Object>> grid = rows(req.get("details"));
        List<Map<String, Object>> freightGrid = rows(req.get("freights"));
        List<Map<String, Object>> glGrid = rows(req.get("journals"));
        List<Map<String, Object>> expGrid = rows(req.get("expenses"));
        List<Map<String, Object>> commGrid = rows(req.get("commissions"));
        List<Map<String, Object>> payGrid = rows(req.get("paymentTerms"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        /* FormValidation (:1402). */
        if (i(req.get("LocationTypeId")) == 0 && !repo.locationTypes().isEmpty()) throw new IllegalArgumentException("Location Type Field is Required");
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

        /* txtExchangeRate_TextChanged: FcyAmount = ItemAmount / rate when rate > 0. */
        double rate = exchangeRate.doubleValue();
        for (var r : grid) r.put("FcyAmount", rate > 0 ? d(r.get("ItemAmount")) / rate : 0d);
        /* FreightProportion (:5880). */
        if (!freightDebitToExpenses) {
            double netWeight = grid.stream().mapToDouble(r -> d(r.get("NetBillWeight"))).sum();
            double credit = freightGrid.stream().mapToDouble(r -> d(r.get("Freight"))).sum();
            for (var r : grid) r.put("Freights", credit > 0 ? credit / netWeight * d(r.get("NetBillWeight")) : 0d);
        }
        /* ExpProportion (:5835). */
        if (!creditInItemSaleGl) {
            double total = expGrid.stream().mapToDouble(r -> d(r.get("Amount"))).sum();
            double qty = grid.stream().mapToDouble(r -> d(r.get("ItemQty"))).sum();
            for (var r : grid) r.put("Expense", total > 0 ? total / qty * d(r.get("ItemQty")) : 0d);
        } else for (var r : grid) r.put("Expense", 0d);

        /* SupCustIdUpdateforFrieghtGrid / ...GLGrid / ...CommissionGrid: the hidden GlAccountId cell is looked up
           in dtAccountlst by the AccountTitle the chosen value displays - resolved here, never taken from the browser. */
        List<Map<String, Object>> accounts = repo.accounts(u, subsidiary, freightDebitToExpenses);
        String valueKey = subsidiary ? "SupplierCustomerId" : "Id";
        for (var r : freightGrid) r.put("GlAccountId", glByTitle(accounts, titleOf(accounts, valueKey, i(r.get("Transporter"))), i(r.get("GlAccountId"))));
        for (var r : glGrid) r.put("GlAccountId", glByTitle(accounts, titleOf(accounts, valueKey, i(r.get("AccountId"))), i(r.get("GlAccountId"))));
        if (commissionDebitToExpenses) {
            List<Map<String, Object>> drList = subsidiary ? accounts : repo.commissionDebitAccounts(u);
            for (var r : commGrid) r.put("GlAccountId", glByTitle(accounts, titleOf(drList, valueKey, i(r.get("DebitAccountId"))), i(r.get("GlAccountId"))));
        }

        /* Freight grid checks. */
        double freightDebit = 0, freightCredit = 0;
        for (var r : freightGrid) {
            freightCredit += d(r.get("Freight"));
            freightDebit += d(r.get("Debit"));
            if (d(r.get("Freight")) > 0 && i(r.get("Transporter")) == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
        }
        if (freightDebitToExpenses && freightCredit != freightDebit) throw new IllegalArgumentException("Freight Debit Amount not equal to Credit Amount Please Check");
        int rowNo = 0;
        for (var r : glGrid) {
            rowNo++;
            if ((d(r.get("Credit")) > 0 || i(r.get("Debit")) > 0) && i(r.get("AccountId")) == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
            if (customerGl == i(r.get("AccountId"))) throw new IllegalArgumentException("Customer Account Not select In Party Add Less Grid row#" + rowNo);
        }
        for (var r : expGrid) {
            if (d(r.get("Amount")) > 0 || i(r.get("ItemId")) > 0) {
                if (i(r.get("ItemId")) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");
                if (d(r.get("Amount")) == 0) throw new IllegalArgumentException("Amount Field is Required Against Expense");
            }
        }

        Head h = new Head();
        /* The current row of grdComm is the last one the user touched; the web sends its index. */
        int current = Math.max(0, Math.min(commGrid.size() - 1, i(req.get("CommissionCurrentRow"))));
        Map<String, Object> cur = commGrid.isEmpty() ? new HashMap<>() : commGrid.get(current);
        int comm = i(cur.get("CommissionAgentId"));
        String commType = s(cur.get("CommType"));
        boolean same = true;
        double commissionAmt = 0;
        for (var r : commGrid) {
            commissionAmt += d(r.get("CommissionAmount"));
            if (d(r.get("CommissionAmount")) > 0) {
                if (i(r.get("CommissionAgentId")) == 0) throw new IllegalArgumentException("Please Select Commission Agent In Commission Grid");
                if (i(r.get("CommissionAgentId")) != comm || !s(r.get("CommType")).equals(commType)) same = false;
                if (commissionDebitToExpenses && i(r.get("DebitAccountId")) == 0) throw new IllegalArgumentException("Debit Account Feild Required In Commission Grid ...");
            }
        }
        if (same && commissionAmt > 0) {
            h.CommissionAgentId = comm;
            h.CommissionDebitAcGLId = policy ? 0 : i(cur.get("DebitAccountId"));
            h.CommissionType = s(cur.get("CommType"));
            /* grdComm.GetTotal(CommRate, AggregateFunction 3 = Average). */
            h.CommRate = commGrid.stream().mapToDouble(r -> d(r.get("CommRate"))).average().orElse(0);
            h.CommAmount = commGrid.stream().mapToDouble(r -> d(r.get("CommissionAmount"))).sum();
            h.CommissionRemarks = s(cur.get("Remarks"));
        }
        LocalDateTime now = LocalDateTime.now();
        h.Id = id;
        h.BranchesId = n(u.getBranchesId());
        h.DocDate = dt(req.get("DocDate"));
        if (h.DocDate == null) throw new IllegalArgumentException("Doc Date is required");
        h.DocNo = docNo;
        h.BranchSrNo = i(req.get("BranchSrNo"));
        h.DocumentTypeId = DOCUMENT_TYPE_ID;
        h.ManualBillNo = s(req.get("ManualBillNo")).trim();
        h.RemarksHeader = s(req.get("Remarks")).trim();
        h.OtherRemarks = h.RemarksHeader;
        h.SupplierCustomerId = customerId;
        h.SupplierInvoiceDate = now;
        h.SupplierInvoiceNo = docNo;
        h.SupplierReferenceNo = s(req.get("SupplierReferenceNo")).trim();
        h.DueDays = i(s(req.get("DueDays")).trim());
        h.DueDate = dt(req.get("DueDate"));
        if (h.DueDate == null) h.DueDate = h.DueDays != 0 ? h.DocDate.plusDays(h.DueDays) : h.DocDate; /* DueDateGenerate */
        h.UomScheduleIdCmRate = s(req.get("UomScheduleIdCmRate")).trim();
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.FinancialYearId = year;
        h.EntryUser = u.getId();
        h.EntryDate = now;
        h.ModifyDate = now;
        h.ModifyUser = u.getId();
        h.ScreenName = SaleInvoiceRepository.SCREEN_NAME;
        h.CustomAccounts = b(req.get("CustomAccounts"));
        h.CurrencyId = currencyId;
        h.ExchangeRate = exchangeRate;
        h.LocationTypeId = i(req.get("LocationTypeId"));
        h.AttachmentsValues = id > 0 ? nullable(req.get("AttachmentsValues")) : "";
        h.CustomAttachmentsValues = id > 0 ? nullable(req.get("CustomAttachmentsValues")) : "";

        SaleInvoiceFinancial.Invoice inv = new SaleInvoiceFinancial.Invoice();
        inv.h = h;
        int line = 0;
        for (var r : grid) {
            Detail p = new Detail();
            p.LineId = ++line;
            p.CropYear = s(r.get("CropYear"));
            if (p.CropYear.isEmpty()) throw new IllegalArgumentException("CropYear Field is Required");
            p.GrossWeight = d(r.get("GrossWeight"));
            p.AdLsWeight = d(r.get("AddLsWt"));
            p.RefRefDocumentTypeId = i(r.get("RefRefDocumentTypeId"));
            p.RefRefDocIdNo = i(r.get("RefRefDocIdNo"));
            p.RefDocSubId = i(r.get("RefDocSubId"));
            p.SaleOrderId = i(r.get("SaleOrderId"));
            p.SaleOrder = i(r.get("SaleOrder"));
            p.UOMCodeItem = s(r.get("UOMCodeItem"));
            if (p.SaleOrderId == 0) throw new IllegalArgumentException("SaleOrderId Field Required");
            p.InvGdnDetailId = i(r.get("Id"));
            if (p.InvGdnDetailId == 0) throw new IllegalArgumentException("InvGdnDetailId not found");
            p.InvGdnId = i(r.get("InvGdnId"));
            if (p.InvGdnId == 0) throw new IllegalArgumentException("InvGdnId not found");
            p.ItemId = i(r.get("ItemId"));
            p.BrandItemId = i(r.get("BrandItemId"));
            if (p.ItemId == 0) throw new IllegalArgumentException("Item Required");
            p.ItemQty = d(r.get("ItemQty"));
            if (p.ItemQty == 0) throw new IllegalArgumentException("ItemQty must be greater than zero");
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
            p.ItemRateWOExp = dec(r.get("ItemRateWOExp"));
            if (p.ItemRateWOExp.signum() == 0) throw new IllegalArgumentException("ItemRate Field Required");
            p.PackingAddLess = dec(r.get("PackingAddLess"));
            p.ItemRate = d(r.get("ItemRate"));
            if (p.ItemRate == 0) throw new IllegalArgumentException("ItemNetRate Field Required");
            p.RateUOM = d(r.get("RateEquivalent"));
            p.UomScheduleIdRate = i(r.get("OrderItemRateUOMId"));
            if (p.UomScheduleIdRate == 0) {
                var uoms = repo.uomScheduleByItem(u.getOrganizationId(), u.getCompanyId(), p.ItemId);
                if (!uoms.isEmpty()) {
                    int eq = i(r.get("RateEquivalent"));
                    var match = uoms.stream().filter(x -> d(col(x, "Equivalent")) == eq).findFirst().orElse(null);
                    if (match == null) throw new IllegalArgumentException("this RateUom not define please check");
                    p.UomScheduleIdRate = i(col(match, "Id"));
                }
            }
            if (p.UomScheduleIdRate == 0) throw new IllegalArgumentException("Rate UOM Required");
            p.RateCut = d(r.get("RateCut"));
            p.RateCutAmount = d(r.get("RateCutAmount"));
            p.ItemAmount = SaleInvoiceFinancial.roundAway(d(r.get("ItemAmount")), amountDp);
            p.ItemDiscount = d(r.get("ItemDiscount"));
            p.ItemDiscountAmount = d(r.get("DiscAmount"));
            if (p.ItemAmount == 0) throw new IllegalArgumentException("ItemAmount Field Required");
            p.WarehouseId = i(r.get("WarehouseId"));
            p.WareHouseName = s(r.get("Warehouse"));
            if (p.WarehouseId == 0) throw new IllegalArgumentException("Warehouse Required");
            p.BillAmount = SaleInvoiceFinancial.roundAway(d(r.get("BillAmount")), amountDp);
            if (!freightDebitToExpenses) p.FreightAmount = d(r.get("Freights"));
            p.JournalAmount = d(r.get("Journal"));
            p.ExpenseAmount = d(r.get("Expense"));
            p.CommOnSale = b(r.get("CommOnSale"));
            if (!commissionDebitToExpenses) p.CommissionAmount = d(r.get("Commission"));
            p.GpDate = now;
            p.GpNo = i(r.get("GpNo"));
            p.BranchId = i(r.get("BranchId"));
            p.VehicleNo = s(r.get("VehicleNo"));
            p.CostCenterId = i(r.get("CostCenterId"));
            inv.details.add(p);
        }
        /* Every InvGdnDetailId must be a line of one of the GDNs it names, still pending for invoicing. */
        verifyLinesBelongToGdns(u, inv.details, id, customerId);

        for (var r : freightGrid) {
            if (i(r.get("Transporter")) == 0) continue;
            Freight f = new Freight();
            f.InvGdnId = i(r.get("InvGdnId"));
            f.TansporterId = i(r.get("GlAccountId"));
            f.TransporterSupCustId = subsidiary ? i(r.get("Transporter")) : 0;
            f.FreightAmount = i(r.get("Freight"));
            f.Debit = i(r.get("Debit"));
            f.Remarks = s(r.get("Remarks"));
            if (f.Remarks.isEmpty()) throw new IllegalArgumentException("Remarks Field Requried in Freight Grid");
            inv.freights.add(f);
        }
        for (var r : expGrid) {
            if (i(r.get("ItemId")) == 0) continue;
            Expense e = new Expense();
            e.InvRevExpItemId = i(r.get("ItemId"));
            e.Qty = i(r.get("Qty"));
            e.Rate = d(r.get("Rate"));
            e.Amount = d(r.get("Amount"));
            e.CustomRemarks = s(r.get("Remarks"));
            e.Remarks = s(r.get("Remarks"));
            if (e.Qty > 0 && e.Rate > 0)
                e.Remarks = "ItemName : " + s(r.get("ItemName")).trim() + "  Qty: " + SaleInvoiceFinancial.g(e.Qty) + " Rate: " + SaleInvoiceFinancial.g(e.Rate) + " " + e.Remarks;
            inv.expenses.add(e);
        }
        for (var r : glGrid) {
            if (i(r.get("AccountId")) == 0 || !(d(r.get("Debit")) > 0 || d(r.get("Credit")) > 0)) continue;
            Journal j = new Journal();
            j.ChartofAccountId = i(r.get("GlAccountId"));
            j.TransporterSupCustId = subsidiary ? i(r.get("AccountId")) : 0;
            j.JvRemarks = s(r.get("Remarks"));
            j.JvPrcnt = d(r.get("Percentage"));
            j.JvQty = d(r.get("Qty"));
            j.JvRate = d(r.get("Rate"));
            j.JvDebit = d(r.get("Debit"));
            j.JvCredit = d(r.get("Credit"));
            inv.journals.add(j);
        }
        for (var r : commGrid) {
            if (i(r.get("CommissionAgentId")) <= 0 || !(d(r.get("CommissionAmount")) > 0)) continue;
            Commission c = new Commission();
            c.Id = i(r.get("Id"));
            c.SaleOrderId = i(r.get("OrderId"));
            c.InvSaleInvoiceId = i(r.get("InvoiceId"));
            c.CommDebitSupCustId = subsidiary ? i(r.get("DebitAccountId")) : 0;
            c.DebitAccountId = i(r.get("GlAccountId"));
            c.CommissionAgentId = i(r.get("CommissionAgentId"));
            String t = commTypeText(r.get("CommType"));
            if ("Percent".equals(t)) c.CommType = 2.0;
            if ("Flat".equals(t)) c.CommType = 1.0;
            if ("Comm Weight".equals(t)) c.CommType = 3.0;
            if (d(r.get("CommType")) > 0) c.CommType = d(r.get("CommType"));
            c.Rate = d(r.get("CommRate"));
            c.RateUom = d(r.get("CommUom"));
            c.CommAmount = d(r.get("CommissionAmount"));
            c.Remarks = s(r.get("Remarks"));
            if (policy) { c.Rate = c.CommAmount; c.CommType = 1.0; c.RateUom = 0.0; c.SaleOrderId = 0; }
            inv.commissions.add(c);
        }
        /* txtBillAmount = Math.Round(BillAmount, dp, AwayFromZero); txtFcyAmount = (BillAmount / rate).ToString("#,##0.####")
           from the unrounded BillAmount - a .NET custom format rounds half away from zero. */
        double rawBill = billAmount(grid, freightGrid, glGrid, expGrid, commGrid, subsidiary, customerId, customerGl, amountDp);
        h.BillAmount = SaleInvoiceFinancial.roundAway(rawBill, amountDp);
        h.FcyAmount = rate == 0 ? BigDecimal.ZERO : new BigDecimal(rawBill / rate).setScale(4, RoundingMode.HALF_UP);
        if (multiCurrency && h.FcyAmount.signum() == 0) throw new IllegalArgumentException("Fcy Amount Field is Required");

        paymentTerms(inv, payGrid, h.BillAmount, h.DocDate, i(s(req.get("DueDays")).trim()), b(req.get("PaymentByPercent")));

        /* BLL 0580 Save. */
        if (h.Id == 0) {
            h.ModifyUser = 0;
        } else {
            h.EntryUser = 0;
        }
        SaleInvoiceFinancial fin = new SaleInvoiceFinancial(repo);
        SaleInvoiceFinancial.Voucher voucher = fin.makeVoucherForSaleInvoice(inv);
        int saved = persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("voucherHeadId", repo.voucherHeadId(h.OrganizationId, h.CompanyId, DOCUMENT_TYPE_ID, saved));
        out.put("message", (id == 0 ? "Record saved Successfully" : "Record Update Successfully") + h.DocNo);
        return out;
    }

    /** BillAmount (:5763) - recomputed from the submitted grids so the header and the voucher agree. */
    private double billAmount(List<Map<String, Object>> grid, List<Map<String, Object>> freightGrid, List<Map<String, Object>> glGrid,
                              List<Map<String, Object>> expGrid, List<Map<String, Object>> commGrid, boolean subsidiary,
                              int customerId, int customerGl, int dp) {
        double item = grid.stream().mapToDouble(r -> d(r.get("ItemAmount"))).sum();
        double exp = expGrid.stream().mapToDouble(r -> d(r.get("Amount"))).sum();
        double jd = 0, jc = 0, tc = 0, td = 0;
        for (var r : glGrid) if (i(r.get("AccountId")) > 0) { jd += d(r.get("Debit")); jc += d(r.get("Credit")); }
        for (var r : freightGrid) {
            int t = i(r.get("Transporter"));
            if (subsidiary ? t == customerId : t == customerGl) { td += d(r.get("Debit")); tc += d(r.get("Freight")); }
        }
        double bill = item + exp;
        double trans = SaleInvoiceFinancial.roundAway(tc - td, dp);
        bill = !(trans > 0) ? bill + Math.abs(trans) : bill - Math.abs(trans);
        double gl = SaleInvoiceFinancial.roundAway(jc - jd, dp);
        bill = !(gl < 0) ? bill + gl : bill - Math.abs(gl);
        double partyComm = 0;
        for (var r : commGrid) if (customerId == i(r.get("CommissionAgentId"))) partyComm += d(r.get("CommissionAmount"));
        bill -= partyComm;
        return bill;
    }

    /** Insert :4498-4628 - the payment-term rows exactly as the desktop builds them. */
    private void paymentTerms(SaleInvoiceFinancial.Invoice inv, List<Map<String, Object>> pay, double billAmount, LocalDateTime docDate,
                              int headerDueDays, boolean byPercent) {
        BigDecimal pamount = pay.stream().map(r -> dec(r.get("Amount"))).reduce(BigDecimal.ZERO, BigDecimal::add);
        BigDecimal paymentDetailAmount = BigDecimal.ZERO;
        BigDecimal ptDiff = BigDecimal.valueOf(billAmount).subtract(pamount);
        if (pamount.signum() > 0) {
            if (pay.stream().anyMatch(r -> dec(r.get("PrcntOfTotal")).signum() > 0 || dec(r.get("Amount")).signum() > 0)) {
                int no = 1;
                for (var r : pay) {
                    if (dec(r.get("PrcntOfTotal")).signum() <= 0 || dec(r.get("Amount")).signum() <= 0)
                        throw new IllegalArgumentException("Payment Detail:All rows must have both % and Amount when any row contains a value. Error at row# " + no);
                    no++;
                }
            }
            Map<Integer, List<Map<String, Object>>> byOrder = new LinkedHashMap<>();
            for (var r : pay) byOrder.computeIfAbsent(i(r.get("SaleOrderId")), k -> new ArrayList<>()).add(r);
            if (byOrder.values().stream().anyMatch(g -> g.size() == 1)) {
                /* PCalculateByAmount -> PCalculateByPercent, then PaymentAmountReCalculate (:3498):
                   amount = percent * NetBillAmount / 100, Math.Round(4, AwayFromZero). */
                for (var r : pay) {
                    BigDecimal pct = dec(r.get("PrcntOfTotal")).max(BigDecimal.ZERO);
                    r.put("Amount", pct.multiply(dec(r.get("NetBillAmount"))).divide(BigDecimal.valueOf(100), 4, RoundingMode.HALF_UP));
                }
            }
            int rowNo = 0;
            for (var r : pay) {
                rowNo++;
                int term = i(r.get("PaymentTerm"));
                if (term <= 0) throw new IllegalArgumentException("Payment Term Required in row# " + rowNo);
                if (term == 2 && i(r.get("DueDays")) <= 0) throw new IllegalArgumentException("Due Days Required in case of Credit in row# " + rowNo);
            }
            for (var e : byOrder.entrySet()) {
                BigDecimal pct = e.getValue().stream().map(r -> dec(r.get("PrcntOfTotal"))).reduce(BigDecimal.ZERO, BigDecimal::add).setScale(4, RoundingMode.HALF_EVEN);
                if (BigDecimal.valueOf(100).subtract(pct).abs().compareTo(new BigDecimal("0.05")) > 0)
                    throw new IllegalArgumentException("Percent Of SaleOrder:" + i(e.getValue().get(0).get("SaleOrderNo")) + " not near to 100. Please Check!");
            }
            for (var r : pay) {
                BigDecimal due = dec(r.get("Amount"));
                BigDecimal paymentDue = pay.size() > 1 && ptDiff.compareTo(BigDecimal.ONE) <= 0 ? due.add(ptDiff) : due;
                PaymentTerm p = new PaymentTerm();
                p.SaleOrderId = i(r.get("SaleOrderId"));
                p.SaleOrderNo = i(r.get("SaleOrderNo"));
                p.PaymentTermId = i(r.get("PaymentTerm"));
                p.DueDays = i(r.get("DueDays"));
                p.PrcntOfTotal = dec(r.get("PrcntOfTotal"));
                p.Amount = pay.size() == 1 ? BigDecimal.valueOf(billAmount) : paymentDue;
                p.DueDate = dt(r.get("DueDate"));
                p.PaymentRemarks = s(r.get("Remarks"));
                p.SystemGeneratedRow = false;
                ptDiff = BigDecimal.ZERO;
                paymentDetailAmount = paymentDetailAmount.add(p.Amount);
                inv.paymentTerms.add(p);
            }
        } else {
            int dueDays = Math.max(0, headerDueDays);
            Set<Integer> seen = new HashSet<>();
            for (var r : pay) {
                int so = i(r.get("SaleOrderId"));
                if (!seen.add(so)) continue;
                PaymentTerm p = new PaymentTerm();
                p.SaleOrderId = so;
                p.SaleOrderNo = i(r.get("SaleOrderNo"));
                p.PaymentTermId = i(r.get("PaymentTerm"));
                p.DueDays = dueDays;
                p.DueDate = docDate.plusDays(dueDays);
                p.PrcntOfTotal = BigDecimal.valueOf(100);
                p.Amount = dec(r.get("NetBillAmount"));
                p.SystemGeneratedRow = true;
                paymentDetailAmount = paymentDetailAmount.add(p.Amount);
                inv.paymentTerms.add(p);
            }
        }
        if (paymentDetailAmount.subtract(BigDecimal.valueOf(billAmount)).abs().compareTo(new BigDecimal("0.55")) > 0)
            throw new IllegalArgumentException("Payment Detail Amount:" + paymentDetailAmount.stripTrailingZeros().toPlainString()
                    + " Not Equal to Bill Amount:" + SaleInvoiceFinancial.g(billAmount));
    }

    /**
     * The desktop trusts its own loader for the GDN link. The web cannot trust a browser, so each line's
     * InvGdnDetailId is checked to belong to its InvGdnId in this company - using the same load
     * procedure the form used to put it on the grid.
     */
    private void verifyLinesBelongToGdns(UserAccount u, List<Detail> details, int invoiceId, int customerId) {
        String ids = details.stream().map(x -> x.InvGdnId).distinct().map(x -> "," + x).collect(Collectors.joining());
        if (invoiceId > 0) return; /* An edit re-sends the lines it read back from the invoice itself. */
        var rows = repo.gdnLoad(ids);
        Set<String> valid = new HashSet<>();
        for (var r : rows) valid.add(i(col(r, "invGdnId")) + ":" + i(col(r, "Id")));
        for (var r : rows)
            if (i(col(r, "SupplierCustomerId")) != customerId) throw new IllegalArgumentException("You Can't Load GDN Of Different Customer At Same Time");
        for (Detail d : details)
            if (!valid.contains(d.InvGdnId + ":" + d.InvGdnDetailId)) throw new IllegalArgumentException("GDN line " + d.InvGdnDetailId + " is not pending for this invoice");
    }

    // ============================================================================== DAL 0433 SetData

    int persist(SaleInvoiceFinancial.Invoice obj, SaleInvoiceFinancial.Voucher voucher, String procName) {
        Head h = obj.h;
        if (obj.details.isEmpty()) throw new IllegalStateException("Detail List not found");
        if (h.Id == 0 && obj.details.stream().anyMatch(x -> x.Id > 0)) throw new IllegalStateException("Record cannot be inserted because detailId greater than zero");
        int num3 = repo.setProc(procName, h);
        if (num3 > 0) h.Id = num3; else num3 = h.Id;
        int num4 = 1;
        boolean flag2 = false;
        for (Detail d : obj.details) {
            if (d.RefRefDocumentTypeId > 0 && d.RefRefDocIdNo > 0 && d.RefDocSubId > 0) flag2 = true;
            d.LineId = num4;
            d.InvSaleInvoiceId = h.Id;
            d.BillAmount = d.ItemAmount + d.ExpenseAmount - d.CommissionAmount - d.FreightAmount;
            d.Id = repo.setProc("Sp_InvSaleInvoiceDetail_Insert", d);
            num4++;
        }
        for (Freight f : obj.freights) { f.InvSaleInvoiceId = h.Id; repo.setProc("Sp_InvSaleInvoiceFreight_Insert", f); }
        for (Journal j : obj.journals) { j.InvSaleInvoiceId = h.Id; repo.setProc("Sp_InvSaleInvoiceJournal_Insert", j); }
        for (Expense e : obj.expenses) { e.InvSaleInvoiceId = h.Id; repo.setProc("Sp_InvSaleInvoiceExpense_Insert", e); }
        for (Commission c : obj.commissions) { c.InvSaleInvoiceId = h.Id; repo.setProc("Sp_InvSaleInvoiceCommission_Insert", c); }
        int sort = 1;
        for (PaymentTerm p : obj.paymentTerms) { p.InvSaleInvoiceId = h.Id; p.SortNo = sort++; repo.setProc("USP_SaleInvoicePaymentTermsDetail_Insert", p); }

        int org = h.OrganizationId, company = h.CompanyId;
        Map<Integer, Map<String, Object>> itemGl = repo.itemGl(org, company);
        Map<Integer, Integer> jobLots = repo.jobLotAccounts(org, company);
        boolean num8 = !Set.of(103, 126, 133, 145).contains(h.DocumentTypeId) && featureOf(org, company, 5);
        boolean flag3 = truthy(repo.config(org, company, "InventoryFinancialsEffectsInActive"));
        boolean num9 = num8 && !flag2 && h.DocumentTypeId != 1611 && h.DocumentTypeId != 1662;
        /* The GDN reference type: 86 for 95; for 171, 170 when any line has a sale order, else 221
           (the num9 switch and GetReferenceDocumentTypeId agree for both). With or without feature 5. */
        if (h.DocumentTypeId != 95 && h.DocumentTypeId != 171) throw new UnsupportedOperationException("DocumentTypeId " + h.DocumentTypeId + " is not ported");
        final int refType = h.DocumentTypeId == 171 ? (obj.details.stream().anyMatch(x -> x.SaleOrderId > 0) ? 170 : 221) : 86;
        List<StockDetail> stock = new ArrayList<>();
        for (Detail item : obj.details) {
            var refs = repo.stockByOtherIds(org, company, refType, item.InvGdnId, item.InvGdnDetailId);
            if (refs.isEmpty()) throw new IllegalStateException("InventoryStockEvalautionDetailslist not found other reference.");
            boolean hasJobLotAccount = jobLots.getOrDefault(item.JobLotId, 0) > 0;
            for (var r : refs) {
                StockDetail sd = stockDetail(r);
                if (num9) {
                    Map<String, Object> ig = itemGl.get(item.ItemId);
                    if (ig == null) throw new IllegalStateException("ItemId Not Found Against CGS Transaction");
                    if (!hasJobLotAccount && !flag3) {
                        String remarks = "ItemQty: " + SaleInvoiceFinancial.g(sd.QtyOut) + " " + s(col(ig, "ItemName")) + " Net Weight: "
                                + SaleInvoiceFinancial.g(sd.StockWeightOut) + " CGS Rate: " + SaleInvoiceFinancial.g(sd.CgsRate) + " " + obj.ids.CompanyName;
                        voucher.details.add(cgsDetail(item, sd, remarks, i(col(ig, "COGSGLAC")), i(col(ig, "PurchaseGLAC")), sd.CgsAmount, 0.0, h.SupplierCustomerId));
                        voucher.details.add(cgsDetail(item, sd, remarks, i(col(ig, "PurchaseGLAC")), i(col(ig, "COGSGLAC")), 0.0, sd.CgsAmount, h.SupplierCustomerId));
                    }
                    updateStockDetail(sd, item, obj, refType);
                } else {
                    prepareGdnReferenceStockDetail(sd, item, obj, refType);
                }
                stock.add(sd);
            }
        }
        for (StockDetail sd : stock) repo.setProc("USP_InventoryStockEvalautionDetailGdnReferences_Update", sd);
        repo.run("EXEC dbo.usp_StockInTransit_VoucherDelete_ByGdnId @Id=?", h.Id);
        /* DAL :316 - every type but 95/1611/1662/1660 (and 126 with GDN lines) runs USP_InventoryValidation per line
           while flag4 (= !(99 && feature 14)) holds. */
        if (h.DocumentTypeId != 95 && h.DocumentTypeId != 1611 && h.DocumentTypeId != 1662 && h.DocumentTypeId != 1660
                && !(h.DocumentTypeId == 126 && obj.details.stream().anyMatch(x -> x.InvGdnId > 0))
                && !(h.DocumentTypeId == 99 && featureOf(org, company, 14))) {
            for (Detail d : obj.details)
                repo.inventoryValidation(org, company, h.DocumentTypeId, h.DocDate, d.ItemId, d.WarehouseId, d.JobLotId, d.CropYear,
                        d.PackingTypeId, d.ItemUOMId, d.NetStockWeight, d.RefRefDocumentTypeId, d.RefRefDocIdNo, d.RefDocSubId, d.ItemConditionId);
        }

        VoucherHead vh = voucher.head;
        int existing = repo.voucherHeadId(org, company, h.DocumentTypeId, h.Id);
        if (existing > 0) vh.Id = existing;
        vh.DocumentTypeSrNo = h.Id;
        vh.RefDocNoId = h.Id;
        int num = repo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
        if (num > 0) vh.Id = num;
        for (VoucherDetail vd : voucher.details) {
            vd.VoucherHeadId = vh.Id;
            vd.BranchesId = h.BranchesId;
            for (Detail d : obj.details) if (d.LineId == vd.LineId && d.LineId > 0 && vd.LineId > 0) { vd.RefDocSubIdNo = d.Id; break; }
            repo.setProc("Sp_VoucherDetail_Insert", vd);
        }
        if (voucher.details.isEmpty()) throw new IllegalStateException("Voucher Detail list Not Found");
        repo.run("EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", org, company, vh.Id);
        int docTypeRef = repo.setProc("Sp_VoucherHead_H_Insert", vh);
        for (VoucherDetail vd : voucher.details) {
            vd.VoucherHeadId = vh.Id;
            vd.DocumentTypeIdRef = docTypeRef;
            repo.setProc("Sp_VoucherDetail_H_Insert", vd);
        }
        ApprovalDetail a = new ApprovalDetail();
        a.OrganizationId = org; a.CompanyId = company; a.DocumentTypeId = h.DocumentTypeId; a.Id = h.Id;
        a.LimitAmount = BigDecimal.valueOf(h.BillAmount);
        repo.setProc("[DAW].[USp_DocumentApprovalDetail_Insert]", a);
        return num3;
    }

    private boolean featureOf(int org, int company, int id) {
        return repo.q("EXEC dbo.USP_GetERPFeaturesByCompanyId @OrganizationId=?, @CompanyId=?", org, company).stream().anyMatch(r -> i(col(r, "Id")) == id);
    }

    /** Conversion.ConvertDataTableToList<InventoryStockEvalautionDetail> - columns matched to properties by name. */
    private static StockDetail stockDetail(Map<String, Object> r) {
        StockDetail x = new StockDetail();
        for (var f : StockDetail.class.getFields()) {
            Object v = col(r, f.getName());
            if (v == null) continue;
            try {
                Class<?> t = f.getType();
                if (t == int.class) f.setInt(x, i(v));
                else if (t == double.class) f.setDouble(x, d(v));
                else if (t == boolean.class) f.setBoolean(x, b(v));
                else if (t == String.class) f.set(x, String.valueOf(v));
                else if (t == LocalDateTime.class) f.set(x, dt(v));
                else if (t == BigDecimal.class) f.set(x, dec(v));
            } catch (IllegalAccessException ignored) { }
        }
        return x;
    }

    /** DAL 0433 UpdateStockDetail (:599). */
    private void updateStockDetail(StockDetail sd, Detail item, SaleInvoiceFinancial.Invoice obj, int refType) {
        Head h = obj.h;
        sd.OrganizationId = h.OrganizationId; sd.CompanyId = h.CompanyId; sd.DocDate = h.DocDate; sd.DocCodeNo = h.DocNo;
        sd.SupplierCustomerId = h.SupplierCustomerId; sd.RefDocumentTypeId = h.DocumentTypeId; sd.BranchesId = h.BranchesId;
        sd.EntryUser = h.EntryUser; sd.ModifyUser = h.ModifyUser; sd.CalcType = "Weight";
        Detail d = obj.details.stream().filter(x -> x.InvGdnId == sd.OtherDocNoId && x.InvGdnDetailId == sd.OtherSubDocNoId && sd.OtherDocumentTypeId == refType)
                .findFirst().orElse(null);
        if (d == null) return;
        double eq = repo.equivalent(h.OrganizationId, h.CompanyId, item.ItemId, d.UomScheduleIdRate);
        if (sd.BillWeightOut > 0.0 && d.ItemRate > 0.0 && eq > 0.0) {
            sd.AmountOut = sd.BillWeightOut / eq * d.ItemRate;
            double num = item.ExpenseAmount > 0 && item.ItemQty > 0 && sd.QtyOut > 0 ? item.ExpenseAmount / item.ItemQty * sd.QtyOut : 0.0;
            double num2 = item.FreightAmount > 0 && item.NetBillWeight > 0 && sd.BillWeightOut > 0 ? item.FreightAmount / item.NetBillWeight * sd.BillWeightOut : 0.0;
            double num3 = item.CommissionAmount > 0 && item.ItemAmount > 0 && sd.AmountOut > 0 ? item.CommissionAmount / item.ItemAmount * sd.AmountOut : 0.0;
            sd.AmountOut += num - num2 - num3;
        }
        sd.ItemRate = d.ItemRate;
        sd.RefDocIdNo = d.InvSaleInvoiceId;
        sd.RefDocSubIdNo = d.Id;
    }

    /** DAL 0433 PrepareGdnReferenceStockDetail (:735). */
    private void prepareGdnReferenceStockDetail(StockDetail sd, Detail item, SaleInvoiceFinancial.Invoice obj, int refType) {
        Head h = obj.h;
        sd.OrganizationId = h.OrganizationId; sd.CompanyId = h.CompanyId; sd.DocDate = h.DocDate; sd.DocCodeNo = h.DocNo;
        sd.SupplierCustomerId = h.SupplierCustomerId; sd.BranchesId = h.BranchesId; sd.RefDocumentTypeId = h.DocumentTypeId;
        sd.EntryUser = h.EntryUser; sd.ModifyUser = h.ModifyUser; sd.CalcType = "Weight";
        Detail d = obj.details.stream().filter(x -> x.InvGdnId == sd.OtherDocNoId && x.InvGdnDetailId == sd.OtherSubDocNoId && sd.OtherDocumentTypeId == refType)
                .findFirst().orElse(null);
        if (d == null) return;
        double eq = repo.equivalent(h.OrganizationId, h.CompanyId, item.ItemId, d.UomScheduleIdRate);
        if (sd.BillWeightOut > 0.0 && d.ItemRate > 0.0 && eq > 0.0) {
            sd.RateUom = d.UomScheduleIdRate;
            sd.AmountOut = d.BillAmount;
            sd.CgsAmount = d.ItemCgsRate > 0.0 ? sd.StockWeightOut / eq * d.ItemCgsRate : sd.AmountOut;
        }
        sd.ItemRate = d.ItemRate;
        sd.CgsRate = d.ItemCgsRate > 0.0 ? d.ItemCgsRate : d.ItemRate;
        sd.RefDocIdNo = d.InvSaleInvoiceId;
        sd.RefDocSubIdNo = d.Id;
    }

    /** DAL 0433 CreateCgsVoucherDetail (:565). */
    private static VoucherDetail cgsDetail(Detail item, StockDetail sd, String remarks, int accountId, int againstAccountId,
                                           double debit, double credit, int party) {
        VoucherDetail x = new VoucherDetail();
        x.LineId = item.LineId; x.IsCGS = 1; x.AccountId = accountId; x.AgainstAccountId = againstAccountId; x.Comments = remarks;
        x.DebitAmount = debit; x.CreditAmount = credit;
        x.RefDocumentTypeId = sd.RefRefDocumentTypeId; x.RefDocNoId = sd.RefRefDocIdNo; x.RefDocNoDetailId = sd.RefRefDocSubIdNo;
        x.ItemId = item.ItemId; x.QtyOut = sd.QtyOut; x.WeightOut = sd.StockWeightOut; x.ItemCgsRate = sd.CgsRate;
        x.RateCut = item.RateCut; x.RateCutAmount = item.RateCutAmount; x.ItemAmount = sd.AmountOut; x.Expenses = item.ExpenseAmount;
        x.Commission = item.CommissionAmount; x.Freight = item.FreightAmount; x.OrderNo = item.SaleOrder; x.GpNo = item.GpNo;
        x.VehicleNo = item.VehicleNo; x.JobLotId = item.JobLotId; x.SupplierCustomerId = party; x.BranchesId = item.BranchId;
        x.CostCenterId = item.CostCenterId;
        return x;
    }

    // ============================================================================== helpers

    /** The display text of a value-list entry, or null when the value is not in the list. */
    private static String titleOf(List<Map<String, Object>> list, String key, int value) {
        if (value == 0) return null;
        for (var r : list) if (i(r.get(key)) == value) return s(r.get("AccountTitle"));
        return null;
    }
    /** dtAccountlst.Select("AccountTitle='..'")[0].Id - unchanged when the title is not there. */
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
    /** The grid's CommType cell holds the type id (1/2/3) and shows its text; the desktop compares the text. */
    private static String commTypeText(Object v) {
        String t = s(v).trim();
        switch (t) {
            case "1": return "Flat";
            case "2": return "Percent";
            case "3": return "Comm Weight";
            default: return t;
        }
    }
    private static String nullable(Object o) { return o == null ? null : String.valueOf(o); }
}
