package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.PurchaseInvoicePmRequest;
import com.mst.repositories.GrnPmRepository;
import com.mst.repositories.InventoryOpeningRepository;
import com.mst.repositories.PurchaseInvoicePmRepository;
import com.mst.repositories.PurchaseInvoiceWriteRepository;
import com.mst.repositories.PurchaseOrderPmRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Transactional;

import static com.mst.repositories.PurchaseInvoicePmRepository.*;
import static com.mst.repositories.PurchaseOrderPmRepository.col;
import static com.mst.repositories.PurchaseOrderPmRepository.dbl;
import static com.mst.repositories.PurchaseOrderPmRepository.intOf;

/**
 * Screen 501 "Purchase Invoice PM" - Packing Material module 54. Desktop
 * {@code Architecture.WinApp.PackingMaterial_Store.PurchaseInvoicePackingMaterial}, DocumentTypeId 702.
 *
 * Save = {@code Insert()} :1916 -> BLL {@code InvPurchaseInvoice.Save} (PurchaseInvoiceFinancial
 * .MakeVoucherForPurchaseInvoice, then the detail-id guard) -> DAL {@code InvPurchaseInvoice.SetData}.
 * For 702 the voucher is: ItemAndSupplierTradingFinancial + FreightFinancial + SupplierAddLessFinancial
 * + SalesTaxFinancial (ByWeight, Return, Brokery and EmptyBags do not include 702).
 *
 * The grid figures are recomputed here with the form's own formulas (grd_CellUpdated :1017,
 * CalculateTaxAmount :3013, FreightProportion :1486, WagesAmountProportion :1436, BillAmount :1058).
 */
@Service
public class PurchaseInvoicePmService {

    public static final int SCREEN_ID = 501;

    private final PurchaseInvoicePmRepository repo;
    private final PurchaseInvoiceWriteRepository writes;
    private final GrnPmRepository grn;
    private final PurchaseOrderPmRepository po;
    private final InventoryOpeningRepository shared;
    private final DesktopAttachmentStore store;
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public PurchaseInvoicePmService(PurchaseInvoicePmRepository repo, PurchaseInvoiceWriteRepository writes, GrnPmRepository grn,
                                    PurchaseOrderPmRepository po, InventoryOpeningRepository shared, DesktopAttachmentStore store,
                                    JdbcTemplate jdbc, CurrentUserContext context, DesktopReportRights rights) {
        this.repo = repo; this.writes = writes; this.grn = grn; this.po = po; this.shared = shared; this.store = store;
        this.jdbc = jdbc; this.context = context; this.rights = rights;
    }

    private UserAccount user(String action) {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; } catch (AccessDeniedException e) { return false; }
    }

    private int financialYear(UserAccount u) {
        var years = shared.years(u);
        if (years.isEmpty()) throw new IllegalArgumentException("No active financial year allocated to this company");
        return intOf(years.get(0).get("Id"));
    }

    private int amountDigits(UserAccount u) {
        return Math.max(0, Math.min(15, intOf(po.config(u, "Default NoofDecimal Points For Amount"))));
    }

    // ================================================================== load

    /** InvfrmPurchaseInvoice_Load :294. */
    public Map<String, Object> lookups() {
        UserAccount u = user("View");
        int fy = financialYear(u);
        boolean subsidiary = po.features(u).contains(4);
        boolean branchWise = bool(po.config(u, "PurchaseInvoiceBranchWise"));
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("suppliers", repo.suppliers(u, fy));
        r.put("accounts", repo.accounts(u, subsidiary, grn));
        r.put("taxAccounts", repo.taxAccounts(u));
        r.put("paymentTerms", po.paymentTerms(u));
        List<Map<String, Object>> branches = new ArrayList<>();
        if (branchWise) {
            Map<String, Object> b = new LinkedHashMap<>();
            b.put("Id", u.getBranchesId());
            b.put("Description", branchName(u));
            branches.add(b);
        } else {
            for (var b : repo.historyBranches(u)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", intOf(col(b, "BranchId")));
                m.put("Description", col(b, "BranchName"));
                branches.add(m);
            }
        }
        r.put("branches", branches);
        r.put("historySuppliers", repo.historySuppliers(u, String.valueOf(u.getBranchesId())));   // HistoryComboFill(ValidateBranch:false)

        Map<String, Object> cfg = new LinkedHashMap<>();
        cfg.put("subsidiaryAccounts", subsidiary);
        cfg.put("taxPercentEditable", bool(po.config(u, "TaxPercentEditable")));
        cfg.put("wagesOnQty", bool(po.config(u, "WagesAmountCalculateOnQty")));
        cfg.put("wagesChargeToProduct", bool(po.config(u, "ContractWagesChargetoProduct")));
        cfg.put("amountDecimals", amountDigits(u));
        cfg.put("branchWise", branchWise);
        cfg.put("financialYearStart", yearStart(u));
        r.put("configuration", cfg);

        Map<String, Boolean> p = new LinkedHashMap<>();
        for (String a : List.of("Save", "Update", "Delete", "Print", "CanView AllRecord")) p.put(a, allowed(u, a));
        r.put("permissions", p);
        r.put("userBranchId", u.getBranchesId());
        r.putAll(numbers());
        return r;
    }

    public Map<String, Object> numbers() {
        UserAccount u = user("View");
        int fy = financialYear(u);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", repo.nextDocNo(u, fy));
        m.put("branchSrNo", repo.nextBranchSrNo(u, fy));
        return m;
    }

    /** LoadGrnForStore, Filterstatus 1. */
    public List<Map<String, Object>> pendingGrns(String from, String to) {
        UserAccount u = user("View");
        return repo.pendingGrns(u, blank(from) ? null : date(from, "From Date"), blank(to) ? null : date(to, "To Date"));
    }

    /** LoadInGridDetail :1386 - the rows, the wages header and the freight / journal rows for these GRNs. */
    public Map<String, Object> loadGrns(List<Integer> grnIds) {
        UserAccount u = user("View");
        Set<Integer> own = repo.ownGrns(u, grnIds == null ? List.of() : grnIds);
        if (own.isEmpty()) throw new IllegalArgumentException("Record Not Found");
        String ids = csv(own);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("lines", repo.grnLines(ids));
        Double w = repo.wages(u, ids);
        m.put("wagesAmount", w == null ? 0d : w);
        m.put("freight", repo.grnFreight(ids));
        return m;
    }

    /** grd_KeyDown F1 :2956 - the tax schedule row for this item on this date. */
    public List<Map<String, Object>> taxOptions(int itemId, String docDate) {
        UserAccount u = user("View");
        return repo.taxSchedule(u, itemId, date(docDate, "Doc Date"));
    }

    // ================================================================== history / record

    public List<Map<String, Object>> history(String from, String to, int fromDocNo, int toDocNo, int supplierId, String branchIds) {
        UserAccount u = user("View");
        if (blank(branchIds)) throw new IllegalArgumentException("Select branch first");                 // :1656
        HistoryFilter f = new HistoryFilter();
        f.canViewAll = allowed(u, "CanView AllRecord");
        f.financialYearId = financialYear(u);
        f.from = blank(from) ? null : date(from, "From Date");
        f.to = blank(to) ? null : date(to, "To Date");
        f.fromDocNo = fromDocNo;
        f.toDocNo = toDocNo;
        f.supplierId = supplierId;
        f.branchIds = "," + branchIds.replaceAll("[^0-9,]", "").replaceAll("^,+", "");
        return repo.history(u, f);
    }

    public List<Map<String, Object>> historySuppliers(String branchIds) {
        UserAccount u = user("View");
        if (blank(branchIds)) throw new IllegalArgumentException("Select branch first");
        return repo.historySuppliers(u, "," + branchIds.replaceAll("[^0-9,]", "").replaceAll("^,+", ""));
    }

    /** ReadById :1817. */
    public Map<String, Object> record(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = repo.header(u, id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        int dp = amountDigits(u);
        List<Map<String, Object>> lines = new ArrayList<>();
        Set<Integer> grnIds = new LinkedHashSet<>();
        for (var d : repo.details(id)) {
            Map<String, Object> l = new LinkedHashMap<>();
            double qty = dbl(col(d, "ItemQty")), nbw = dbl(col(d, "NetBillWeight"));
            l.put("Id", intOf(col(d, "Id")));
            l.put("PurchaseOrderId", intOf(col(d, "PurchaseOrderId")));
            l.put("OrderNo", intOf(col(d, "PurchaseOrder")));
            l.put("InvGrnDetailId", intOf(col(d, "InvGrnDetailId")));
            l.put("InvGrnId", intOf(col(d, "InvGrnId")));
            l.put("ItemId", intOf(col(d, "ItemId")));
            l.put("ItemCode", col(d, "ItemCode"));
            l.put("ItemName", col(d, "ItemName"));
            l.put("WarehouseId", intOf(col(d, "WarehouseId")));
            l.put("WareHouseName", col(d, "WareHouseName"));
            l.put("RackId", intOf(col(d, "RackId")));
            l.put("RackName", col(d, "rackName"));
            l.put("ItemConditionId", intOf(col(d, "ItemConditionId")));
            l.put("ItemCondition", col(d, "ItemCondition"));
            l.put("ItemUomId", intOf(col(d, "ItemUOMId")));
            l.put("UOMCodeItem", col(d, "UOMCodeItem"));
            l.put("ItemQty", qty);
            l.put("OrderBagWt", dbl(col(d, "EBWeight")));
            l.put("BagWt", qty == 0 ? 0d : nbw / qty);                                       // :1849
            l.put("NetBillWeight", nbw);
            l.put("Rate", dbl(col(d, "ItemRate")));
            l.put("OrderItemRateUOMId", intOf(col(d, "UomScheduleIdRate")));
            l.put("RateUom", dbl(col(d, "EquivalentPoRate")));
            l.put("RateUomCode", col(d, "RateUom"));
            l.put("AddLsAmount", dbl(col(d, "RateCutAmount")));
            l.put("ItemAmount", away(dbl(col(d, "ItemAmount")), dp));
            l.put("TaxNameId", intOf(col(d, "TaxNameId")));
            l.put("TaxName", col(d, "TaxDescriptions"));
            l.put("TaxPercent", dbl(col(d, "TaxPercent")));
            l.put("TaxAmount", dbl(col(d, "TaxAmount")));
            l.put("BillAmount", away(dbl(col(d, "BillAmount")), dp));
            l.put("Freights", dbl(col(d, "FreightAmount")));
            l.put("Wages", dbl(col(d, "WagesAmount")));
            lines.add(l);
            grnIds.add(intOf(col(d, "InvGrnId")));
        }
        out.put("lines", lines);
        out.put("freight", repo.freight(id));
        out.put("journal", repo.journal(id));
        Double w = repo.wages(u, csvNoLead(grnIds));
        out.put("wagesAmount", w);                                                             // null = keep what the form shows (:1879)
        out.put("attachments", repo.attachments(u, id));
        out.put("voucherHeadId", repo.voucherHeadId(u, id));
        return out;
    }

    // ================================================================== save

    private static final class Row {
        Map<String, Object> base;
        PurchaseInvoicePmRequest.Line req;
        int grnId, grnDetailId, poId, itemId, whId, rackId, condId, uomId, rateUomId, refType, supplierId, billType, jobLotId;
        double qty, nbw, orderBagWt, rate, rateUom, addLs, amount, taxPct, tax, freight, wages;
        int taxNameId;
    }

    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> save(PurchaseInvoicePmRequest r) {
        if (r == null || r.Id < 0) throw new IllegalArgumentException("Record Not Found");
        boolean insert = r.Id == 0;
        UserAccount u = user(insert ? "Save" : "Update");
        int fy = financialYear(u), dp = amountDigits(u);
        boolean subsidiary = po.features(u).contains(4);
        boolean taxEditable = bool(po.config(u, "TaxPercentEditable"));
        List<PurchaseInvoicePmRequest.Line> reqLines = r.lines == null ? List.of() : r.lines;
        Map<String, Object> old = insert ? null : repo.header(u, r.Id);
        if (!insert && "APPROVED".equalsIgnoreCase(Objects.toString(col(old, "ApprovedStatus"), ""))) {
            throw new IllegalArgumentException("Can't Update Approved Record");
        }

        // -------------------------------------------------------- base rows (GRN loader or saved invoice)
        Map<Integer, Map<String, Object>> saved = new HashMap<>();
        if (!insert) for (var d : repo.details(r.Id)) saved.put(intOf(col(d, "Id")), d);
        Set<Integer> newGrnIds = new LinkedHashSet<>();
        for (var l : reqLines) if (l != null && l.Id == 0) newGrnIds.add(l.InvGrnId);
        Set<Integer> own = repo.ownGrns(u, newGrnIds);
        Map<Integer, Map<String, Object>> loaded = new HashMap<>();
        if (!own.isEmpty()) for (var g : repo.grnLines(csv(own))) loaded.put(intOf(col(g, "Id")), g);

        List<Row> rows = new ArrayList<>();
        Set<Integer> usedGrnDetail = new HashSet<>(), keptIds = new HashSet<>();
        for (var l : reqLines) {
            if (l == null) throw new IllegalArgumentException("Grid Record Not found");
            Row x = new Row();
            x.req = l;
            if (l.Id > 0) {
                if (insert) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");   // BLL :24
                var d = saved.get(l.Id);
                if (d == null || !keptIds.add(l.Id)) throw new IllegalArgumentException("Detail row does not belong to this invoice");
                x.base = d;
                x.grnId = intOf(col(d, "InvGrnId")); x.grnDetailId = intOf(col(d, "InvGrnDetailId")); x.poId = intOf(col(d, "PurchaseOrderId"));
                x.itemId = intOf(col(d, "ItemId")); x.whId = intOf(col(d, "WarehouseId")); x.rackId = intOf(col(d, "RackId"));
                x.condId = intOf(col(d, "ItemConditionId")); x.uomId = intOf(col(d, "ItemUOMId")); x.rateUomId = intOf(col(d, "UomScheduleIdRate"));
                x.qty = dbl(col(d, "ItemQty")); x.nbw = dbl(col(d, "NetBillWeight")); x.orderBagWt = dbl(col(d, "EBWeight"));
                x.rate = dbl(col(d, "ItemRate")); x.rateUom = dbl(col(d, "EquivalentPoRate")); x.jobLotId = intOf(col(d, "JobLotId"));
                x.addLs = dbl(col(d, "RateCutAmount")); x.amount = away(dbl(col(d, "ItemAmount")), dp);
                x.taxNameId = intOf(col(d, "TaxNameId")); x.taxPct = dbl(col(d, "TaxPercent")); x.tax = dbl(col(d, "TaxAmount"));
                x.supplierId = intOf(col(old, "SupplierCustomerId")); x.billType = intOf(col(old, "BillCalculateTypeId"));
                x.refType = -1;
            } else {
                var g = own.contains(l.InvGrnId) ? loaded.get(l.InvGrnDetailId) : null;
                if (g == null || intOf(col(g, "InvGrnId")) != l.InvGrnId) throw new IllegalArgumentException("GRN row not found");
                x.base = g;
                x.grnId = intOf(col(g, "InvGrnId")); x.grnDetailId = intOf(col(g, "Id")); x.poId = intOf(col(g, "PurchaseOrderId"));
                x.itemId = intOf(col(g, "ItemId")); x.whId = intOf(col(g, "WarehouseId")); x.rackId = intOf(col(g, "RackId"));
                x.condId = intOf(col(g, "ItemConditionId")); x.uomId = intOf(col(g, "ItemUomId")); x.rateUomId = intOf(col(g, "OrderItemRateUOMId"));
                x.qty = dbl(col(g, "ItemQty")); x.nbw = dbl(col(g, "NetBillWeight")); x.orderBagWt = dbl(col(g, "OrderBagWt"));
                x.rate = dbl(col(g, "ItemRate")); x.rateUom = dbl(col(g, "RateUom")); x.jobLotId = 0;           // grid carries no JobLotId
                x.addLs = 0; x.amount = dbl(col(g, "ItemAmount"));
                x.taxNameId = intOf(col(g, "TaxNameId")); x.taxPct = dbl(col(g, "TaxPercent")); x.tax = dbl(col(g, "TaxAmount"));
                x.supplierId = intOf(col(g, "SupplierCustomerId")); x.billType = intOf(col(g, "BillCalculateTypeId"));
                x.refType = intOf(col(g, "RefDocumentTypeId"));
            }
            if (x.grnDetailId > 0 && !usedGrnDetail.add(x.grnDetailId)) throw new IllegalArgumentException("GRN row loaded twice");
            rows.add(x);
        }
        if (!insert) {
            for (Integer id : saved.keySet()) if (!keptIds.contains(id)) {
                throw new IllegalArgumentException("Saved detail rows cannot be removed from this screen");
            }
        }

        // -------------------------------------------------------- header fields the GRN / saved record decides
        int supplierId, billType, poFirst;
        String deliveryTerm;
        Integer forcedTerm = null;
        Map<String, Object> first = rows.isEmpty() ? null : rows.get(0).base;
        if (insert) {
            supplierId = first == null ? 0 : intOf(col(first, "SupplierCustomerId"));
            billType = first == null ? 0 : intOf(col(first, "BillCalculateTypeId"));
            deliveryTerm = first == null ? "" : Objects.toString(col(first, "DeliveryTerm"), "");
            poFirst = first == null ? 0 : intOf(col(first, "PurchaseOrderId"));
            if (first != null && intOf(col(first, "RefDocumentTypeId")) != 52) forcedTerm = intOf(col(first, "PaymentTermsId"));
        } else {
            supplierId = intOf(col(old, "SupplierCustomerId"));
            billType = intOf(col(old, "BillCalculateTypeId"));
            deliveryTerm = Objects.toString(col(old, "DeliveryTerm"), "");
            poFirst = rows.isEmpty() ? 0 : rows.get(0).poId;
            if (!saved.isEmpty()) {
                var d0 = saved.values().stream().min(Comparator.comparingInt(d -> intOf(col(d, "Id")))).get();
                if (intOf(col(d0, "PurchaseOrderId")) > 0) forcedTerm = intOf(col(old, "PaymentTermsId"));        // :1885
            }
        }
        // LoadDataDetailGridAgainstGrnIds :1249 / LoadGrnForStore :"Same Order Type"
        int refFirst = rows.stream().filter(x -> x.refType != -1).map(x -> x.refType).findFirst().orElse(-1);
        for (Row x : rows) {
            if (x.refType == -1) continue;
            if (x.refType != refFirst) throw new IllegalArgumentException("You Can Only Select Rows Of Same Order Type");
            if (x.refType == 52) {
                if (x.supplierId != supplierId) throw new IllegalArgumentException("You Can't load Grn of other customer");
                if (x.billType != billType) throw new IllegalArgumentException("You Can't load Grn of other Bill Type");
            } else if (x.poId != poFirst) {
                throw new IllegalArgumentException("You Can't load Grn of other Order");
            }
        }
        int paymentTermsId = forcedTerm != null ? forcedTerm : r.PaymentTermsId;
        int dueDays = paymentTermsId == 1 ? 0 : toInt(r.DueDays);                                  // CmbPaymentTerm_TextChanged :2694
        String billTypeText = billType == 1 ? "On Weight" : billType == 2 ? "On Qty" : "";

        // -------------------------------------------------------- FormValidation :1109
        int docNo = insert ? repo.nextDocNo(u, fy) : intOf(col(old, "DocNo"));
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        var suppliers = repo.suppliers(u, fy);
        Map<String, Object> supplier = null;
        for (var s : suppliers) if (intOf(col(s, "Id")) == supplierId) supplier = s;
        if (supplierId == 0 || supplier == null) throw new IllegalArgumentException("Supplier Name Field is Required");
        int supplierGl = intOf(col(supplier, "GlAccountId"));
        if (paymentTermsId <= 0 || po.paymentTerms(u).stream().noneMatch(t -> intOf(t.get("Id")) == paymentTermsId)) {
            throw new IllegalArgumentException("Payment Term Field is Required");
        }
        if (paymentTermsId == 2 && dueDays <= 0) throw new IllegalArgumentException("Due Days Field is Required");

        // -------------------------------------------------------- freight / journal grids :1945-1962
        var accounts = repo.accounts(u, subsidiary, grn);
        String key = subsidiary ? "SupplierCustomerId" : "Id";
        List<PurchaseInvoicePmRequest.Freight> freight = r.freight == null ? List.of() : r.freight;
        List<PurchaseInvoicePmRequest.Journal> journal = r.journal == null ? List.of() : r.journal;
        Set<Integer> invoiceGrns = new HashSet<>();
        for (Row x : rows) invoiceGrns.add(x.grnId);
        for (var f : freight) {
            if (f == null) throw new IllegalArgumentException("Invalid freight row");
            if (f.Freight < 0 || f.Debit < 0) throw new IllegalArgumentException("Freight amounts cannot be negative");
            if (f.Freight > 0 && f.Debit > 0) throw new IllegalArgumentException("Debit Side is aleady added");
            if (f.Freight > 0 && f.Transporter == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
            if (f.Transporter != 0 && account(accounts, key, f.Transporter) == null) throw new IllegalArgumentException("Select a transporter from the list");
            if (f.InvGrnId != 0 && !invoiceGrns.contains(f.InvGrnId)) throw new IllegalArgumentException("Freight row belongs to another GRN");
        }
        for (var j : journal) {
            if (j == null) throw new IllegalArgumentException("Invalid journal row");
            if (j.Credit < 0 || j.Debit < 0) throw new IllegalArgumentException("Journal amounts cannot be negative");
            if (j.Credit > 0 && j.Debit > 0) throw new IllegalArgumentException("Credit Side is aleady added");
            if ((j.Credit > 0 || toInt32(j.Debit) > 0) && j.AccountId == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
            if (j.AccountId != 0) {
                var a = account(accounts, key, j.AccountId);
                if (a == null) throw new IllegalArgumentException("Select an account from the list");
                if (supplierGl == j.AccountId) throw new IllegalArgumentException("You cannot select supplier Account");        // BillAmount :1074
                if (!subsidiary ? supplierGl == intOf(a.get("Id")) : supplierId == j.AccountId) {
                    throw new IllegalArgumentException("Selected Account Can not be Same As Supplier Account");                  // :958-978
                }
            }
        }

        // -------------------------------------------------------- the rows: grd_CellUpdated / CalculateTaxAmount
        java.sql.Date docDate = date(r.DocDate, "Doc Date");
        for (Row x : rows) {
            var l = x.req;
            if (l.Edited) {
                double amount = billType == 1 ? (x.rateUom == 0 ? 0d : x.nbw / x.rateUom * x.rate) : billType == 2 ? x.qty * x.rate : 0d;
                if (l.AddLsAmount != 0 && amount > 0) amount += l.AddLsAmount;
                x.addLs = l.AddLsAmount;
                x.amount = amount;
                int taxNameId = l.TaxNameId;
                double pct = l.TaxPercent;
                if (taxNameId != x.taxNameId || pct != x.taxPct) {
                    var sched = repo.taxSchedule(u, x.itemId, docDate);
                    Map<String, Object> s0 = sched.isEmpty() ? null : sched.get(0);
                    boolean nameOk = taxNameId == 0 || taxNameId == x.taxNameId || (s0 != null && intOf(col(s0, "TaxNameId")) == taxNameId);
                    boolean pctOk = taxEditable || pct == x.taxPct || pct == 0 || (s0 != null && dbl(col(s0, "TaxPercent")) == pct);
                    if (!nameOk || !pctOk) throw new IllegalArgumentException("Invalid tax in detail grid row#" + (rows.indexOf(x) + 1));
                }
                if (pct > 100) pct = 0;                                                              // :3019
                x.taxNameId = taxNameId;
                x.taxPct = pct;
                x.tax = (amount > 0 && pct > 0) ? away(amount * pct / 100, dp) : 0d;
            }
        }
        double totalAmount = 0, totalTax = 0, netQty = 0, netWeight = 0;
        for (Row x : rows) { totalAmount += x.amount; totalTax += x.tax; netQty += x.qty; netWeight += x.nbw; }

        // FreightProportion :1486
        double credit = 0, debit = 0;
        for (var f : freight) { credit += f.Freight; debit += f.Debit; }
        for (Row x : rows) {
            if (debit < credit) {
                double diff = Math.rint(credit - debit);
                x.freight = billType == 2 ? diff / netQty * x.qty : billType == 1 ? diff / netWeight * x.nbw : 0d;
            } else {
                x.freight = 0d;
            }
        }
        // WagesAmountProportion :1436 - the header comes from the GRN wages (LoadInGridDetail / ReadById)
        Set<Integer> allGrns = new LinkedHashSet<>();
        for (Row x : rows) allGrns.add(x.grnId);
        Double wagesRead = repo.wages(u, csvNoLead(allGrns));
        double wagesHeader = wagesRead == null ? (insert ? 0d : dbl(col(old, "WagesAmount"))) : wagesRead;
        boolean wagesOnQty = bool(po.config(u, "WagesAmountCalculateOnQty"));
        for (Row x : rows) x.wages = wagesHeader > 0 ? (wagesOnQty ? wagesHeader / netQty * x.qty : wagesHeader / netWeight * x.nbw) : 0d;

        // BillAmount :1058 - TransporterCredit is never assigned in the desktop, so it adds 0
        double jDebit = 0, jCredit = 0;
        for (var j : journal) if (j.AccountId > 0) { jDebit += j.Debit; jCredit += j.Credit; }
        double billAmount = away(Math.rint(totalAmount) + totalTax + jDebit - jCredit, dp);

        // -------------------------------------------------------- Insert() :1963-1977, :2006-2051
        int taxAccountId = r.TaxAccountId;
        if (totalTax > 0 || rows.stream().anyMatch(x -> x.tax > 0)) {
            if (taxAccountId == 0 || repo.taxAccounts(u).stream().noneMatch(a -> intOf(a.get("Id")) == taxAccountId)) {
                throw new IllegalArgumentException("Tax Account is Required");
            }
        }
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not found");
        boolean wagesToProduct = bool(po.config(u, "ContractWagesChargetoProduct"));
        List<Map<String, Object>> details = new ArrayList<>();
        int rowNo = 0;
        for (Row x : rows) {
            required(x.itemId, "Item Name", rowNo);
            required(x.whId, "Warehouse", rowNo);
            required(x.rackId, "Rack Name", rowNo);
            required(x.condId, "Item Condition", rowNo);
            if (x.qty <= 0) fieldError("Item Qty", rowNo);
            required(x.uomId, "Pack Uom", rowNo);
            if (x.nbw <= 0) fieldError("Net Weight", rowNo);
            if (x.rate <= 0) fieldError("Item Rate", rowNo);
            required(x.rateUomId, "Rate Uom", rowNo);
            if (x.amount <= 0) fieldError("Amount", rowNo);
            int taxNameId = x.taxPct > 0 ? x.taxNameId : 0;
            if (x.taxPct > 0 && x.tax > 0 && taxNameId == 0) throw new IllegalArgumentException("Tax name required in detail grid row#" + (rowNo + 1));

            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", x.req.Id);
            d.put("InvGrnDetailId", x.grnDetailId);
            d.put("InvGrnId", x.grnId);
            d.put("PurchaseOrderId", x.poId);
            d.put("ItemId", x.itemId);
            d.put("WarehouseId", x.whId);
            d.put("RackId", x.rackId);
            d.put("ItemConditionId", x.condId);
            d.put("ItemUOMId", x.uomId);
            d.put("ItemQty", x.qty);
            d.put("EBWeight", x.orderBagWt);
            d.put("GrossWeight", x.nbw);
            d.put("NetBillWeight", x.nbw);
            d.put("NetStockWeight", x.nbw);
            d.put("ItemRate", x.rate);
            d.put("UomScheduleIdRate", x.rateUomId);
            d.put("ItemAmount", x.amount);
            d.put("RateCutAmount", x.addLs);
            d.put("FreightAmount", x.freight);
            d.put("WagesAmount", x.wages);
            d.put("TaxNameId", taxNameId);
            d.put("TaxPercent", x.taxPct);
            d.put("TaxAmount", x.tax);
            d.put("LineId", rowNo + 1);                                                             // DAL :64
            // DAL :80-83 - 702 is the "else" branch; the form's BillAmount (with tax) is replaced
            d.put("BillAmount", x.amount + x.freight + (wagesToProduct ? x.wages : 0d));
            details.add(d);
            rowNo++;
        }
        List<Map<String, Object>> freightRows = new ArrayList<>();
        for (var f : freight) {
            if (f.Transporter == 0) continue;
            if (subsidiary) {
                // :2064 reads r5.Cells["AccountId"], a column grdFreight does not have - the desktop throws here.
                throw new IllegalArgumentException("Column 'AccountId' does not belong to the freight grid (desktop behaviour with subsidiary accounts)");
            }
            if (blank(f.Remarks)) throw new IllegalArgumentException("Freight Grid Remarks Field is Required");
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("InvGrnId", f.InvGrnId);
            m.put("TansporterId", f.Transporter);                                                    // :2071 overrides the GlAccountId assignment
            m.put("SupplierCustomerId", 0);
            m.put("FreightAmount", toInt32(f.Freight));
            m.put("Debit", toInt32(f.Debit));
            m.put("Remarks", f.Remarks);
            freightRows.add(m);
        }
        List<Map<String, Object>> journalRows = new ArrayList<>();
        for (var j : journal) {
            if (j.AccountId == 0) continue;
            var a = account(accounts, key, j.AccountId);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ChartofAccountId", intOf(a.get("Id")));
            m.put("SupplierCustomerId", subsidiary ? j.AccountId : 0);
            m.put("JvRemarks", j.Remarks == null ? "" : j.Remarks);
            m.put("JvPrcnt", j.Percentage);
            m.put("JvQty", j.Qty);
            m.put("JvRate", j.Rate);
            m.put("JvDebit", j.Debit);
            m.put("JvCredit", j.Credit);
            journalRows.add(m);
        }

        // -------------------------------------------------------- header :1978-2004
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> h = new LinkedHashMap<>();
        if (!insert) h.put("Id", r.Id);
        h.put("BranchesId", u.getBranchesId());
        h.put("DocDate", new Timestamp(docDate.getTime()));
        h.put("DocNo", docNo);
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        h.put("BranchSrNo", insert ? repo.nextBranchSrNo(u, fy) : intOf(col(old, "BranchSrNo")));
        h.put("ManualBillNo", r.ManualBillNo == null ? "" : r.ManualBillNo.trim());
        h.put("RemarksHeader", r.RemarksHeader == null ? "" : r.RemarksHeader.trim());
        h.put("DeliveryTerm", deliveryTerm.trim());
        h.put("SupplierCustomerId", supplierId);
        h.put("SupplierInvoiceDate", now);
        h.put("SupplierInvoiceNo", docNo);
        h.put("PaymentTermsId", paymentTermsId);
        h.put("DueDate", Timestamp.valueOf(docDate.toLocalDate().plusDays(dueDays).atStartOfDay()));
        h.put("DueDays", dueDays);
        h.put("BillAmount", billAmount);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("FinancialYearId", fy);
        h.put("EntryUser", u.getId());
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("ModifyUser", u.getId());
        h.put("TaxAccountId", taxAccountId);
        h.put("WagesAmount", wagesHeader);
        h.put("ScreenName", SCREEN);
        h.put("IsTaxable", taxAccountId > 0 && totalTax > 0);
        h.put("BillCalculateTypeId", billTypeText.equals("On Weight") ? 1 : billTypeText.equals("On Qty") ? 2 : 0);
        h.put("ActionId", insert ? 1 : 2);
        h.put("InvoiceQty", netQty);                                                              // DAL :39-40
        h.put("InvoiceWeight", netWeight);

        // -------------------------------------------------------- PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice
        var fin = writes.accounts(Map.of("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()), List.of());
        if (fin.stockAgainst() <= 0) throw new IllegalArgumentException("Stock AgainstAc Configuration Not Found");
        if (!fin.parties().isEmpty()) {
            var p = fin.parties().get(supplierId);
            if (p == null) throw new IllegalArgumentException("Supplier GLAccountId not Found");
        }
        int refAccount = fin.parties().isEmpty() ? 0 : intOf(PurchaseInvoiceFinancialRules.copy(fin.parties().get(supplierId)).get("GlAccountId"));
        Map<String, Object> vh = new LinkedHashMap<>();
        vh.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        vh.put("DocumentTypeSrNo", insert ? 0 : r.Id);
        vh.put("RefDocNoId", insert ? 0 : r.Id);                                                   // obj.Id before the insert
        vh.put("VoucherCode", docNo);
        vh.put("VoucherDate", new Timestamp(docDate.getTime()));
        vh.put("RemarksOtherLingo", "");
        vh.put("VoucherAmount", billAmount);
        vh.put("MultiCurrencyId", 0);
        vh.put("ExchangeCurrencyRate", 0d);
        vh.put("FcAmount", 0d);
        vh.put("RefAccountId", refAccount);
        vh.put("AgainstAccountId", 0);
        vh.put("ChequeDate", Timestamp.valueOf(LocalDate.now().atStartOfDay()));
        vh.put("IncludeWHT", false);
        vh.put("BranchId", u.getBranchesId());
        vh.put("ProjectId", 0);
        vh.put("BillAmount", billAmount);
        vh.put("ManualBillNo", h.get("ManualBillNo"));
        vh.put("DueDate", h.get("DueDate"));
        vh.put("DueDays", dueDays);
        vh.put("OrganizationId", u.getOrganizationId());
        vh.put("CompanyId", u.getCompanyId());
        vh.put("FinancialYearId", fy);
        vh.put("EntryUser", u.getId());
        vh.put("EntryDate", now);
        vh.put("ModifyDate", now);
        vh.put("ModifyUser", u.getId());
        String remarks = Objects.toString(h.get("RemarksHeader"), "");
        if (!remarks.isEmpty()) vh.put("Remarks", remarks);
        List<Map<String, Object>> vd = voucher(details, rows, freightRows, journalRows, fin, refAccount, supplierId, taxAccountId, remarks);

        // attachments are written to disk before the header (DAL :46-51)
        List<Map<String, Object>> newFiles = storeFiles(u, r);
        List<Map<String, Object>> kept = new ArrayList<>();
        if (!insert) {
            Set<Integer> drop = new HashSet<>(r.removeAttachmentIds == null ? List.of() : r.removeAttachmentIds);
            for (var a : repo.attachments(u, r.Id)) if (!drop.contains(intOf(col(a, "Id")))) kept.add(a);
        }
        List<String> names = new ArrayList<>(), custom = new ArrayList<>();
        for (var a : kept) { names.add(Objects.toString(col(a, "Attachment"), "")); custom.add(Objects.toString(col(a, "UploadedFileCustomName"), "")); }
        for (var a : newFiles) { names.add((String) a.get("name")); custom.add((String) a.get("stored")); }
        if (!names.isEmpty()) {
            h.put("AttachmentsValues", String.join(",", names));
            h.put("CustomAttachmentsValues", String.join(",", custom));
        } else if (!insert) {
            h.put("AttachmentsValues", col(old, "AttachmentsValues"));
            h.put("CustomAttachmentsValues", col(old, "CustomAttachmentsValues"));
        }

        // -------------------------------------------------------- DAL InvPurchaseInvoice.SetData
        int id;
        try {
            int num3 = writes.execute(insert ? "Sp_InvPurchaseInvoice_Insert" : "Sp_InvPurchaseInvoice_Update", h);
            id = num3 > 0 ? num3 : r.Id;
            if (id <= 0) throw new IllegalArgumentException("Record could not be saved");
            StringBuilder detailIds = new StringBuilder();
            for (var d : details) {
                d.put("InvPurchaseInvoiceId", id);
                int did = writes.execute("Sp_InvPurchaseInvoiceDetail_Insert", d);
                detailIds.append(did).append(',');
            }
            for (var f : freightRows) { f.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceFreight_Insert", f); }
            for (var j : journalRows) { j.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceJournal_Insert", j); }
            ProcExec.run(jdbc, "EXEC dbo.usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations @OrganizationId=?, @CompanyId=?, @Id=?, @DetailIds=?",
                    u.getOrganizationId(), u.getCompanyId(), id, detailIds.toString());
            ProcExec.run(jdbc, "EXEC dbo.usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId @Id=?", id);
            attachments(u, id, supplierId, r, newFiles);
            ProcExec.run(jdbc, "EXEC dbo.Sp_InventoryStockEvalautionDetail_Update @OrganizationId=?, @CompanyId=?, @RefDocumentTypeId=?, @RefDocIdNo=?",
                    u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id);
            int previous = repo.voucherHeadId(u, id);
            vh.put("Id", previous);
            vh.put("DocumentTypeSrNo", id);
            int voucherId = writes.execute(previous == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
            if (voucherId <= 0) voucherId = previous;
            if (voucherId <= 0) throw new IllegalArgumentException("Voucher could not be saved");
            vh.put("Id", voucherId);
            if (vd.isEmpty()) throw new IllegalArgumentException("VoucherDetail list Not Found");
            for (var v : vd) { v.put("VoucherHeadId", voucherId); v.put("BranchesId", u.getBranchesId()); writes.execute("Sp_VoucherDetail_Insert", v); }
            ProcExec.call(jdbc, "EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", u.getOrganizationId(), u.getCompanyId(), voucherId);
            int historyRef = writes.execute("Sp_VoucherHead_H_Insert", vh);
            for (var v : vd) { v.put("VoucherHeadId", voucherId); v.put("DocumentTypeIdRef", historyRef); writes.execute("Sp_VoucherDetail_H_Insert", v); }
            ProcExec.run(jdbc, "EXEC [DAW].[USp_DocumentApprovalDetail_Insert] @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?, @LimitAmount=?",
                    u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id, BigDecimal.valueOf(billAmount));
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }

        int savedNo = intOf(col(repo.header(u, id), "DocNo"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", savedNo);
        out.put("voucherHeadId", repo.voucherHeadId(u, id));
        out.put("message", (insert ? "Record saved Successfully [" : "Record Update Successfully [") + docNo + "] ");
        return out;
    }

    /** MakeVoucherForPurchaseInvoice for 702: Trading, Freight, SupplierAddLess, SalesTax - in that order. */
    private List<Map<String, Object>> voucher(List<Map<String, Object>> details, List<Row> rows, List<Map<String, Object>> freightRows,
                                              List<Map<String, Object>> journalRows, PurchaseInvoiceFinancialRules.Accounts fin,
                                              int ref, int supplierId, int taxAccountId, String remarks) {
        List<Map<String, Object>> out = new ArrayList<>();
        double freightSum = 0, weightSum = 0;
        // ItemAndSupplierTradingFinancial :1282
        for (int i = 0; i < details.size(); i++) {
            var d = details.get(i);
            var item = fin.items().get(intOf(d.get("ItemId")));
            if (item == null) throw new IllegalArgumentException("Item Purchase GL Account Not found");
            var it = PurchaseInvoiceFinancialRules.copy(item);
            int gl = intOf(it.get("PurchaseGLAC"));
            double amount = dbl(d.get("ItemAmount")), fr = dbl(d.get("FreightAmount"));
            StringBuilder sb = new StringBuilder().append(remarks).append(" ItemName: ").append(Objects.toString(it.get("ItemName"), ""))
                    .append(" Rate: ").append(cs(dbl(d.get("ItemRate")))).append(" Qty: ").append(cs(dbl(d.get("ItemQty"))))
                    .append(" Weight: ").append(cs(dbl(d.get("NetBillWeight")))).append(" Item Amount: ").append(cs(amount));
            if (fr > 0) sb.append(" Freight Amount: ").append(cs(Math.rint(fr)));
            String text = sb.toString();
            for (int leg = 0; leg < 2; leg++) {
                Map<String, Object> v = new LinkedHashMap<>();
                v.put("AccountId", leg == 0 ? gl : ref);
                v.put("AgainstAccountId", leg == 0 ? ref : gl);
                v.put("Comments", text);
                v.put("DebitAmount", leg == 0 ? amount : 0d);
                v.put("CreditAmount", leg == 0 ? 0d : amount);
                v.put("ItemId", d.get("ItemId"));
                v.put("JobLotId", 0);
                v.put("QtyIn", d.get("ItemQty"));
                v.put("ItemRate", d.get("ItemRate"));
                v.put("WeightIn", d.get("NetBillWeight"));
                v.put("RateCut", 0d);
                v.put("RateCutAmount", d.get("RateCutAmount"));
                v.put("ItemAmount", amount);
                v.put("Freight", fr);
                v.put("Commission", 0d);
                v.put("OrderNo", 0);
                v.put("GpNo", 0);
                v.put("SupplierCustomerId", supplierId);
                if (leg == 1) { v.put("SubsidiaryAccountId", supplierId); v.put("SubsidiaryTypeId", 1); }
                out.add(v);
            }
            freightSum += fr;
            weightSum += dbl(d.get("NetBillWeight"));
        }
        if (freightSum > 0) {
            for (var d : details) {
                var it = PurchaseInvoiceFinancialRules.copy(fin.items().get(intOf(d.get("ItemId"))));
                double fr = dbl(d.get("FreightAmount"));
                Map<String, Object> v = new LinkedHashMap<>();
                v.put("AccountId", intOf(it.get("PurchaseGLAC")));
                v.put("AgainstAccountId", fin.stockAgainst());
                v.put("DebitAmount", fr);
                v.put("Comments", "ChargeToProduct/TotalWeightInDetail * Qty = Proportionated Charges : " + cs(freightSum) + " / " + cs(weightSum)
                        + " * " + cs(dbl(d.get("ItemQty"))) + " = " + cs(Math.rint(fr)));
                v.put("CreditAmount", 0d);
                v.put("ItemId", d.get("ItemId"));
                v.put("JobLotId", 0);
                out.add(v);
            }
        }
        // FreightFinancial :292 - InvPurchaseInvoiceDetail.PurchaseGLAC is never set for 702, so the
        // "one distinct GL" test sees only 0 and AgainstAccountId falls back to the transporter itself.
        String company = Objects.toString(PurchaseInvoiceFinancialRules.copy(fin.parties().getOrDefault(supplierId, Map.of())).get("CompanyName"), "");
        for (var f : freightRows) {
            int t = intOf(f.get("TansporterId"));
            double dr = dbl(f.get("Debit")), cr = dbl(f.get("FreightAmount"));
            String rem = Objects.toString(f.get("Remarks"), "");
            if (t > 0 && dr > 0) {
                Map<String, Object> v = new LinkedHashMap<>();
                v.put("AccountId", t);
                v.put("AgainstAccountId", t);
                v.put("Comments", rem.isEmpty() ? "PartyName :  " + company + "  Freight : " + cs(cr) : rem);
                v.put("DebitAmount", dr);
                v.put("CreditAmount", 0d);
                out.add(v);
            }
            if (t > 0 && cr > 0) {
                Map<String, Object> v = new LinkedHashMap<>();
                v.put("AccountId", t);
                v.put("AgainstAccountId", t);
                v.put("Comments", rem.isEmpty() ? "SupplierName :  " + company + "  Freight : " + cs(cr) : rem);
                v.put("DebitAmount", 0d);
                v.put("CreditAmount", cr);
                out.add(v);
            }
        }
        // SupplierAddLessFinancial :171
        for (var j : journalRows) {
            int acc = intOf(j.get("ChartofAccountId"));
            double jd = dbl(j.get("JvDebit")), jc = dbl(j.get("JvCredit"));
            if (acc <= 0 || !(jc > 0 || jd > 0)) continue;
            String rem = Objects.toString(j.get("JvRemarks"), "");
            int sc = intOf(j.get("SupplierCustomerId"));
            Map<String, Object> v = new LinkedHashMap<>();
            v.put("AccountId", acc); v.put("AgainstAccountId", ref); v.put("Comments", rem);
            v.put("DebitAmount", jd); v.put("CreditAmount", jc);
            if (sc > 0) { v.put("SupplierCustomerId", sc); v.put("SubsidiaryAccountId", sc); v.put("SubsidiaryTypeId", 1); }
            out.add(v);
            Map<String, Object> w = new LinkedHashMap<>();
            w.put("AccountId", ref); w.put("AgainstAccountId", acc); w.put("Comments", rem);
            w.put("DebitAmount", jc); w.put("CreditAmount", jd);
            w.put("SupplierCustomerId", supplierId); w.put("SubsidiaryAccountId", supplierId); w.put("SubsidiaryTypeId", 1);
            out.add(w);
        }
        // SalesTaxFinancial :232
        for (var d : details) {
            double tax = dbl(d.get("TaxAmount")), pct = dbl(d.get("TaxPercent"));
            if (tax <= 0) continue;
            String text = cs(pct) + "Tax %  " + cs(tax);
            Map<String, Object> v = new LinkedHashMap<>();
            v.put("AccountId", taxAccountId); v.put("Comments", text); v.put("AgainstAccountId", ref);
            v.put("DebitAmount", tax); v.put("TaxesTotalAmount", tax); v.put("TaxPrcnt", pct); v.put("IsTaxable", "True");
            v.put("TaxTypeId", d.get("TaxNameId"));
            out.add(v);
            Map<String, Object> w = new LinkedHashMap<>();
            w.put("AccountId", ref); w.put("AgainstAccountId", taxAccountId); w.put("Comments", text);
            w.put("CreditAmount", tax); w.put("TaxesTotalAmount", tax); w.put("IsTaxable", "True"); w.put("TaxPrcnt", pct);
            w.put("TaxTypeId", d.get("TaxNameId"));
            out.add(w);
        }
        return out;
    }

    /** btnDelete_Click :2579. */
    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> delete(int id) {
        UserAccount u = user("Delete");
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        repo.header(u, id);
        try {
            repo.delete(u, id);
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }
        return Map.of("message", "Delete Record Successfully");
    }

    public Map<String, Object> voucherHead(int id) {
        UserAccount u = user("View");
        repo.header(u, id);
        return Map.of("voucherHeadId", repo.voucherHeadId(u, id));
    }

    // ================================================================== files

    private List<Map<String, Object>> storeFiles(UserAccount u, PurchaseInvoicePmRequest r) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (r.files == null) return out;
        if (r.files.size() > 10) throw new IllegalArgumentException("At most ten attachments may be uploaded at once");
        for (var file : r.files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(file);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("name", file.name);
            m.put("stored", store.store(u, file.name, bytes));
            m.put("size", bytes.length / 1048576d);
            out.add(m);
        }
        return out;
    }

    private void attachments(UserAccount u, int id, int supplierId, PurchaseInvoicePmRequest r, List<Map<String, Object>> newFiles) {
        var existing = repo.attachments(u, id);
        for (Integer removed : new LinkedHashSet<>(r.removeAttachmentIds == null ? List.<Integer>of() : r.removeAttachmentIds)) {
            if (removed == null || existing.stream().noneMatch(a -> intOf(col(a, "Id")) == removed)) {
                throw new IllegalArgumentException("Attachment does not belong to this invoice");
            }
            ProcExec.call(jdbc, "EXEC dbo.Sp_DMSAttachments_GetAllMethod @Id=?, @Activity=?", removed, "AttachmentDeleteById");
        }
        for (var f : newFiles) {
            Timestamp now = new Timestamp(System.currentTimeMillis());
            Object[] v = {0, supplierId, 0, DOCUMENT_TYPE_ID, id, f.get("name"), now, u.getId(), now, u.getId(),
                    u.getOrganizationId(), u.getCompanyId(), 0, SCREEN, false, f.get("stored"), f.get("size"), 0};
            ProcExec.run(jdbc, "EXEC dbo.Proc_DMSAttachments_Insert @Id=?, @RefAccountId=?, @DMSFoldersLabelsId=?, @RefDocumentTypeId=?, @RefDocumentNo=?, @Attachment=?, @EntryDate=?, @EntryUser=?, @ModifyDate=?, @ModifyUser=?, @OrganizationId=?, @CompanyId=?, @BranchId=?, @ScreenName=?, @DetailWiseAttachment=?, @UploadedFileCustomName=?, @UploadedFileSizeMb=?, @LineId=?", v);
        }
    }

    public DesktopInventoryItemFileService.Download attachment(int id, int attachmentId) {
        UserAccount u = user("View");
        repo.header(u, id);
        var row = repo.attachments(u, id).stream().filter(a -> intOf(col(a, "Id")) == attachmentId).findFirst()
                .orElseThrow(() -> new org.springframework.web.server.ResponseStatusException(org.springframework.http.HttpStatus.NOT_FOUND, "Attachment not found"));
        String stored = Objects.toString(col(row, "UploadedFileCustomName"), "");
        if (stored.isBlank()) stored = Objects.toString(col(row, "Attachment"), "");
        return new DesktopInventoryItemFileService.Download(basename(Objects.toString(col(row, "Attachment"), stored)),
                store.read(u, basename(stored)), "application/octet-stream");
    }

    // ================================================================== helpers

    private static Map<String, Object> account(List<Map<String, Object>> accounts, String key, int value) {
        for (var a : accounts) if (intOf(a.get(key)) == value) return a;
        return null;
    }

    private String branchName(UserAccount u) {
        var rows = jdbc.queryForList("SELECT BranchName FROM dbo.Branches WHERE Id=?", u.getBranchesId());
        return rows.isEmpty() ? "" : Objects.toString(col(rows.get(0), "BranchName"), "");
    }

    private String yearStart(UserAccount u) {
        var years = shared.years(u);
        if (years.isEmpty()) return "";
        Object s = col(years.get(0), "Start_Period");
        return s == null ? "" : String.valueOf(s).substring(0, Math.min(10, String.valueOf(s).length()));
    }

    private static void required(int v, String field, int rowIndex) { if (v == 0) fieldError(field, rowIndex); }

    private static void fieldError(String field, int rowIndex) {
        throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** Math.Round(x, n, MidpointRounding.AwayFromZero). */
    private static double away(double v, int digits) {
        return BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_UP).doubleValue();
    }

    /** Convert.ToInt32(double) - banker's rounding. */
    private static int toInt32(double v) { return (int) Math.rint(v); }

    /** Conversion.ToInt(string) - Convert.ToInt32 of text; a non-integer string is 0. */
    private static int toInt(String v) {
        if (blank(v)) return 0;
        try { return Integer.parseInt(v.trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** double.ToString() as .NET writes it into a StringBuilder (15 significant digits, no trailing zeros). */
    static String cs(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return Long.toString((long) v);
        return new BigDecimal(v).round(new MathContext(15)).stripTrailingZeros().toPlainString();
    }

    private static String csv(Collection<Integer> ids) {
        StringBuilder sb = new StringBuilder();
        for (Integer i : ids) sb.append(',').append(i);
        return sb.toString();
    }

    private static String csvNoLead(Collection<Integer> ids) {
        StringJoiner sj = new StringJoiner(",");
        for (Integer i : ids) sj.add(String.valueOf(i));
        return sj.toString();
    }

    private static boolean blank(String v) { return v == null || v.isBlank(); }

    private static Date date(String v, String name) {
        try { return Date.valueOf(LocalDate.parse(v.trim().substring(0, 10))); }
        catch (Exception e) { throw new IllegalArgumentException(name + " is not a valid date"); }
    }

    private static boolean bool(String v) {
        return v != null && Set.of("true", "1", "yes").contains(v.trim().toLowerCase(Locale.ROOT));
    }

    private static String basename(String name) {
        String n = name.replace('\\', '/');
        String r = n.substring(n.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(r);
        return r;
    }
}
