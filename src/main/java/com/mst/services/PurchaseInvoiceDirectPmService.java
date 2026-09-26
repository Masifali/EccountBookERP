package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.PurchaseInvoiceDirectPmRequest;
import com.mst.repositories.GrnPmRepository;
import com.mst.repositories.InventoryOpeningRepository;
import com.mst.repositories.PurchaseInvoiceDirectPmRepository;
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

import static com.mst.repositories.PurchaseInvoiceDirectPmRepository.*;
import static com.mst.repositories.PurchaseOrderPmRepository.col;
import static com.mst.repositories.PurchaseOrderPmRepository.dbl;
import static com.mst.repositories.PurchaseOrderPmRepository.intOf;

/**
 * Screen 495 "Purchase Invoice Direct PM" - Packing Material module 54. Desktop
 * {@code Architecture.WinApp.PackingMaterial_Store.frmPurchaseInvoiceDirectPM}, DocumentTypeId 245.
 *
 * Save = {@code Insert()} :2356 -> BLL {@code InvPurchaseInvoice.Save} -> DAL {@code InvPurchaseInvoice.SetData}.
 * For 245 the voucher (PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice) is:
 * ItemAndSupplierTradingFinancial (case 245 remarks, job-lot account) + FreightFinancial + SupplierAddLessFinancial.
 * SalesTaxFinancial does NOT list 245, so tax is billed but never posted (ported as-is).
 * After the voucher, 245 is in the DAL's InventoryTransactions set (:524).
 */
@Service
public class PurchaseInvoiceDirectPmService {

    public static final int SCREEN_ID = 495;

    private final PurchaseInvoiceDirectPmRepository repo;
    private final PurchaseInvoicePmRepository pi;
    private final PurchaseInvoiceWriteRepository writes;
    private final GrnPmRepository grn;
    private final PurchaseOrderPmRepository po;
    private final InventoryOpeningRepository shared;
    private final DesktopAttachmentStore store;
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public PurchaseInvoiceDirectPmService(PurchaseInvoiceDirectPmRepository repo, PurchaseInvoicePmRepository pi,
                                          PurchaseInvoiceWriteRepository writes, GrnPmRepository grn, PurchaseOrderPmRepository po,
                                          InventoryOpeningRepository shared, DesktopAttachmentStore store, JdbcTemplate jdbc,
                                          CurrentUserContext context, DesktopReportRights rights) {
        this.repo = repo; this.pi = pi; this.writes = writes; this.grn = grn; this.po = po; this.shared = shared;
        this.store = store; this.jdbc = jdbc; this.context = context; this.rights = rights;
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

    /** clsGlobalVariables format digits: "#,##0." + n zeros for n 1..4, otherwise none. */
    private int digits(UserAccount u, String config) {
        int n = intOf(po.config(u, config));
        return n >= 1 && n <= 4 ? n : 0;
    }

    /** DecimalRateFormate: 1..4 -> n, 0 -> 2, anything else -> none. */
    private int rateDigits(UserAccount u) {
        int n = intOf(po.config(u, "Default NoofDecimal Points For Rate"));
        return n >= 1 && n <= 4 ? n : n == 0 ? 2 : 0;
    }

    // ================================================================== load

    /** InvfrmPurchasedirectInvoice_Load :392. */
    public Map<String, Object> lookups() {
        UserAccount u = user("View");
        Set<Integer> features = po.features(u);
        boolean subsidiary = features.contains(4);
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("suppliers", repo.suppliers(u));
        r.put("accounts", repo.accounts(u, subsidiary, grn));
        r.put("taxAccounts", pi.taxAccounts(u));
        r.put("items", po.items(u));
        r.put("uoms", po.uoms(u));
        r.put("racks", repo.racks(u));
        r.put("conditions", grn.itemConditions());
        r.put("cropYears", grn.cropYears(u));
        r.put("jobLots", repo.jobLots(u));
        r.put("currencies", repo.currencies(u));
        r.put("refDocumentTypes", po.refDocumentTypes());
        List<Map<String, Object>> branches = new ArrayList<>();
        for (var b : repo.historyBranches(u)) {                                                   // BranchImplemented is never true
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(col(b, "BranchId")));
            m.put("Description", col(b, "BranchName"));
            branches.add(m);
        }
        r.put("branches", branches);
        r.put("historySuppliers", repo.historySuppliers(u, String.valueOf(u.getBranchesId())));  // HistoryComboFill(ValidateBranch:false)

        Map<String, Object> cfg = new LinkedHashMap<>();
        cfg.put("subsidiaryAccounts", subsidiary);
        cfg.put("branchFeature", features.contains(11));
        cfg.put("multiCurrency", features.contains(6));
        cfg.put("taxPercentEditable", bool(po.config(u, "TaxPercentEditable")));
        cfg.put("defaultJobLotId", intOf(po.config(u, "Job/Lot")));
        cfg.put("baseCurrencyId", intOf(po.config(u, "Base Currency")));
        cfg.put("baseCurrencyRate", dbl(po.config(u, "BaseCurrencyRate")));
        cfg.put("defaultWarehouseId", intOf(po.config(u, "PackingMaterialDefaultWarehouse")));
        cfg.put("amountDecimals", digits(u, "Default NoofDecimal Points For Amount"));
        cfg.put("rateDecimals", rateDigits(u));
        cfg.put("fcyDecimals", digits(u, "DefaultNoOfDecimalPointsForFcyAmount"));
        cfg.put("financialYearStart", yearStart(u));
        r.put("configuration", cfg);

        Map<String, Boolean> p = new LinkedHashMap<>();
        for (String a : List.of("Save", "Update", "Delete", "Print", "CanView AllRecord")) p.put(a, allowed(u, a));
        r.put("permissions", p);
        r.put("userBranchId", u.getBranchesId());
        r.put("userBranchName", branchName(u));
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

    /** GetTaxTypeIdAndTaxPercent :1246 - the schedule row for this item on this date. */
    public List<Map<String, Object>> taxOptions(int itemId, String docDate) {
        UserAccount u = user("View");
        return pi.taxSchedule(u, itemId, date(docDate, "Doc Date"));
    }

    /** DocDate_Leave :3770. */
    public List<Map<String, Object>> taxByItems(String itemIds, String docDate) {
        UserAccount u = user("View");
        String ids = itemIds == null ? "" : itemIds.replaceAll("[^0-9,]", "");
        return repo.taxByItems(u, ids.startsWith(",") ? ids : "," + ids, date(docDate, "Doc Date"));
    }

    /** BalanceStock :1202. */
    public Map<String, Object> stock(int itemId, String docDate, int conditionId, int recId, int warehouseId, int rackId) {
        UserAccount u = user("View");
        if (recId > 0) repo.header(u, recId);
        double q = repo.stockInHand(u, itemId, date(docDate, "Doc Date"), conditionId, recId, warehouseId, rackId);
        return Map.of("qtyInHand", BigDecimal.valueOf(q).setScale(2, RoundingMode.HALF_EVEN).doubleValue());
    }

    /** CmbRefDocumentType_Leave :1280. */
    public List<Map<String, Object>> refDocs(int refDocumentTypeId, int recId) {
        UserAccount u = user("View");
        if (recId > 0) repo.header(u, recId);
        return repo.refDocs(u, refDocumentTypeId, recId);
    }

    /** cmbCurrency_Leave :1505. */
    public Map<String, Object> exchangeRate(int currencyId) {
        UserAccount u = user("View");
        int base = intOf(po.config(u, "Base Currency"));
        double rate = currencyId == base ? dbl(po.config(u, "BaseCurrencyRate")) : Optional.ofNullable(repo.lastExchangeRate(u, currencyId)).orElse(0d);
        return Map.of("rate", away(rate, rateDigits(u)));
    }

    /** DeleteDetailrow :2325 - a saved row asks the server before it leaves the grid. */
    public Map<String, Object> checkDetailDelete(int id, int detailId) {
        UserAccount u = user("View");
        repo.header(u, id);
        try {
            repo.stockInReferenceValidation(u, id, detailId);
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }
        return Map.of("ok", true);
    }

    // ================================================================== history / record

    public List<Map<String, Object>> history(String dateType, String from, String to, int fromDocNo, int toDocNo, int supplierId, String branchIds) {
        UserAccount u = user("View");
        if (blank(branchIds)) throw new IllegalArgumentException("Select branch first");                 // GetAll :3350
        HistoryFilter f = new HistoryFilter();
        f.canViewAll = allowed(u, "CanView AllRecord");
        f.financialYearId = financialYear(u);
        f.dateType = dateType == null ? "doc" : dateType;
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

    /** ReadById :2642 (also GetDetailGrdByHeadId :3535). */
    public Map<String, Object> record(int id) {
        UserAccount u = user("View");
        Map<String, Object> h = repo.header(u, id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        List<Map<String, Object>> lines = new ArrayList<>();
        for (var d : repo.details(id)) {
            Map<String, Object> l = new LinkedHashMap<>();
            l.put("Id", intOf(col(d, "Id")));
            l.put("ItemId", intOf(col(d, "ItemId")));
            l.put("ItemCode", col(d, "ItemCode"));
            l.put("ItemName", col(d, "ItemName"));
            l.put("WarehouseId", intOf(col(d, "WarehouseId")));
            l.put("WareHouseName", col(d, "WareHouseName"));
            l.put("RackId", intOf(col(d, "RackId")));
            l.put("RackName", col(d, "rackName"));
            l.put("ItemConditionId", intOf(col(d, "ItemConditionId")));
            l.put("ItemCondition", col(d, "ItemCondition"));
            l.put("CropYearId", intOf(col(d, "CropYearId")));
            l.put("CropYear", col(d, "CropYear"));
            l.put("JobLotId", intOf(col(d, "JobLotId")));
            l.put("JobLot", col(d, "JobLotDescription"));
            l.put("PackingDate", col(d, "PackingDate"));
            l.put("ExpiryDate", col(d, "ExpiryDate"));
            l.put("UOMId", intOf(col(d, "ItemUOMId")));
            l.put("UOM", col(d, "UOMCodeItem"));
            l.put("ItemQty", dbl(col(d, "ItemQty")));
            l.put("Rate", dbl(col(d, "ItemRate")));
            l.put("RateUOMId", intOf(col(d, "UomScheduleIdRate")));
            l.put("RateUOM", col(d, "RateUom"));
            l.put("ItemAmount", dbl(col(d, "ItemAmount")));
            l.put("FcyAmount", dbl(col(d, "FcyAmount")));
            l.put("TaxNameId", intOf(col(d, "TaxNameId")));
            l.put("TaxName", col(d, "TaxDescriptions"));
            l.put("TaxPercent", dbl(col(d, "TaxPercent")));
            l.put("TaxAmount", dbl(col(d, "TaxAmount")));
            l.put("BillAmount", dbl(col(d, "BillAmount")));
            l.put("Freights", dbl(col(d, "FreightAmount")));
            l.put("RemarksDetail", Objects.toString(col(d, "RemarksDetail"), ""));
            l.put("GpNo", intOf(col(d, "GpNo")));
            l.put("VehicleNo", Objects.toString(col(d, "VehicleNo"), ""));
            l.put("BranchId", intOf(col(d, "BranchId")));
            l.put("BranchName", col(d, "BranchName"));
            l.put("RefDocumentTypeId", intOf(col(d, "RefDocumentTypeId")));
            l.put("RefDocumentType", col(d, "RefDocumentType"));
            l.put("RefDocId", intOf(col(d, "RefDocId")));
            l.put("RefDocNo", col(d, "RefDocNo"));
            l.put("RefDocInvoiceId", intOf(col(d, "RefDocInvoiceId")));
            l.put("RefDocInvoiceNo", col(d, "RefDocInvoiceNo"));
            lines.add(l);
        }
        out.put("lines", lines);
        out.put("freight", repo.freight(id));
        out.put("journal", repo.journal(id));
        out.put("attachments", repo.attachments(u, id));
        out.put("voucherHeadId", repo.voucherHeadId(u, id));
        out.put("approved", approved(h));
        return out;
    }

    private static boolean approved(Map<String, Object> h) {
        Object a = col(h, "IsApproved");
        if (a instanceof Boolean b) return b;
        if (a != null && Set.of("1", "true").contains(a.toString().trim().toLowerCase(Locale.ROOT))) return true;
        return "APPROVED".equalsIgnoreCase(Objects.toString(col(h, "ApprovedStatus"), ""));
    }

    // ================================================================== save

    private static final class Row {
        PurchaseInvoiceDirectPmRequest.Line req;
        Map<String, Object> saved;
        String itemName, cropYear, taxName;
        int branchId;
        double amount, tax, fcy, freight;
    }

    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> save(PurchaseInvoiceDirectPmRequest r) {
        if (r == null || r.Id < 0) throw new IllegalArgumentException("Record Not Found");
        boolean insert = r.Id == 0;
        UserAccount u = user(insert ? "Save" : "Update");
        int fy = financialYear(u);
        Set<Integer> features = po.features(u);
        boolean subsidiary = features.contains(4), branchFeature = features.contains(11), multi = features.contains(6);
        boolean taxEditable = bool(po.config(u, "TaxPercentEditable"));
        Map<String, Object> old = insert ? null : repo.header(u, r.Id);
        if (!insert && approved(old)) throw new IllegalArgumentException("Record Not Update because Record has approved");   // btnUpdate_Click :2626
        List<PurchaseInvoiceDirectPmRequest.Line> reqLines = r.lines == null ? List.of() : r.lines;
        if (reqLines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        // -------------------------------------------------------- FormValidation :525
        int docNo = insert ? repo.nextDocNo(u, fy) : intOf(col(old, "DocNo"));
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        Map<String, Object> supplier = null;
        for (var s : repo.suppliers(u)) if (intOf(s.get("Id")) == r.SupplierId) supplier = s;
        if (r.SupplierId == 0 || supplier == null) throw new IllegalArgumentException("Supplier Field is Required");
        int supplierGl = intOf(supplier.get("GlAccountId"));
        int baseCurrency = intOf(po.config(u, "Base Currency"));
        int rateDp = rateDigits(u), fcyDp = digits(u, "DefaultNoOfDecimalPointsForFcyAmount");
        int currencyId;
        double exchangeRate;
        if (multi) {
            currencyId = r.CurrencyId;
            if (currencyId == 0 || repo.currencies(u).stream().noneMatch(c -> intOf(c.get("Id")) == currencyId)) {
                throw new IllegalArgumentException("Fcy Code Field is Required");
            }
            exchangeRate = r.ExchangeRate;
            if (!(exchangeRate > 0) || !Double.isFinite(exchangeRate)) throw new IllegalArgumentException("Exchange Rate Field is Required");
        } else {
            currencyId = baseCurrency;                                                              // the hidden combo keeps the configured base
            if (currencyId == 0) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            exchangeRate = away(dbl(po.config(u, "BaseCurrencyRate")), rateDp);
            if (exchangeRate == 0) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }

        // -------------------------------------------------------- lookups the rows are checked against
        java.sql.Date docDate = date(r.DocDate, "Doc Date");
        Map<Integer, Map<String, Object>> items = new HashMap<>();
        for (var i : po.items(u)) items.put(intOf(i.get("Id")), i);
        List<Map<String, Object>> racks = repo.racks(u), uoms = po.uoms(u);
        Map<Integer, Map<String, Object>> lots = new HashMap<>();
        for (var j : repo.jobLots(u)) lots.putIfAbsent(intOf(j.get("Id")), j);
        Map<Integer, Object> conditions = new HashMap<>(), crops = new HashMap<>();
        for (var c : grn.itemConditions()) conditions.put(intOf(c.get("Id")), c.get("Description"));
        for (var c : grn.cropYears(u)) crops.put(intOf(c.get("Id")), c.get("Description"));
        Set<Integer> refTypes = new HashSet<>();
        for (var t : po.refDocumentTypes()) refTypes.add(intOf(col(t, "Id")));
        Map<Integer, Map<String, Object>> saved = new HashMap<>();
        if (!insert) for (var d : repo.details(r.Id)) saved.put(intOf(col(d, "Id")), d);

        // -------------------------------------------------------- the rows (btnAdd_Click / Insert :2493-2552)
        List<Row> rows = new ArrayList<>();
        Set<Integer> kept = new HashSet<>();
        Map<Integer, List<Map<String, Object>>> refCache = new HashMap<>();
        Map<Integer, Map<String, Object>> taxCache = new HashMap<>();
        int n = 0;
        for (var l : reqLines) {
            n++;
            if (l == null) throw new IllegalArgumentException("Grid Record Not Found");
            Row x = new Row();
            x.req = l;
            if (l.Id > 0) {
                if (insert) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");   // BLL :24
                x.saved = saved.get(l.Id);
                if (x.saved == null || !kept.add(l.Id)) throw new IllegalArgumentException("Detail row does not belong to this invoice");
            }
            require(l.WarehouseId == 0, "Warehouse", n);
            require(l.ItemId == 0, "Item", n);
            require(l.ItemConditionId == 0, "Item Condition", n);
            require(l.RackId == 0, "Rack", n);
            require(l.JobLotId == 0, "Job Lot", n);
            require(l.UOMId == 0, "UOM", n);
            require(!(l.ItemQty != 0), "Quantity", n);
            require(!(l.Rate != 0), "Rate", n);
            require(l.RateUOMId == 0, "Rate UOM", n);
            var item = items.get(l.ItemId);
            if (item == null) throw new IllegalArgumentException("Item Name Field is Required");
            x.itemName = Objects.toString(item.get("ItemName"), "");
            Map<String, Object> rack = null;
            for (var k : racks) if (intOf(k.get("Id")) == l.RackId && intOf(k.get("ItemId")) == l.ItemId && intOf(k.get("WarehouseId")) == l.WarehouseId) rack = k;
            if (rack == null) throw new IllegalArgumentException("Rack Name Field is Required");
            if (!conditions.containsKey(l.ItemConditionId)) throw new IllegalArgumentException("Item Condition Field is Required");
            var lot = lots.get(l.JobLotId);
            if (lot == null) throw new IllegalArgumentException("Job/Lot Field is Required");
            if (l.CropYearId != 0 && !crops.containsKey(l.CropYearId)) throw new IllegalArgumentException("Select a crop year from the list");
            x.cropYear = l.CropYearId == 0 ? "" : Objects.toString(crops.get(l.CropYearId), "").trim();
            Map<String, Object> packUom = null, rateUom = null;
            for (var m : uoms) {
                if (intOf(m.get("ItemId")) != l.ItemId) continue;
                if (intOf(m.get("Id")) == l.UOMId) packUom = m;
                if (intOf(m.get("Id")) == l.RateUOMId) rateUom = m;
            }
            if (packUom == null) throw new IllegalArgumentException("UOM Field is Required");
            if (rateUom == null) throw new IllegalArgumentException("Rate UOM Field is Required");
            // btnAdd_Click :2040 - warehouse branch vs job lot branch (BranchImplemented is never true)
            x.branchId = intOf(rack.get("BranchId"));
            if (branchFeature && x.branchId != intOf(lot.get("BranchId"))) {
                throw new IllegalArgumentException("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + Objects.toString(rack.get("BranchName"), "")
                        + "' and JobLot is of Branch '" + Objects.toString(lot.get("BranchName"), "") + "'");
            }
            // AmountCaluculation :2943
            double eq = dbl(rateUom.get("Equivalent"));
            x.amount = (l.ItemQty > 0 && eq > 0 && l.Rate > 0) ? BigDecimal.valueOf(l.ItemQty / eq * l.Rate).setScale(2, RoundingMode.HALF_EVEN).doubleValue() : 0d;
            require(x.amount == 0, "Amount", n);
            // GetTaxTypeIdAndTaxPercent / CalculateTaxAmount :3072 - the only tax the combo can hold is the schedule row
            int taxNameId = l.TaxNameId;
            double pct = 0;
            x.taxName = "";
            if (taxNameId > 0) {
                Map<String, Object> s0 = taxCache.computeIfAbsent(l.ItemId, id -> {
                    var s = pi.taxSchedule(u, id, docDate);
                    return s.isEmpty() ? Map.of() : s.get(0);
                });
                boolean scheduleMatch = !s0.isEmpty() && intOf(col(s0, "TaxNameId")) == taxNameId;
                boolean savedMatch = x.saved != null && intOf(col(x.saved, "TaxNameId")) == taxNameId;
                if (!scheduleMatch && !savedMatch) throw new IllegalArgumentException("Tax Name is Required");
                double schedulePct = scheduleMatch ? dbl(col(s0, "TaxPercent")) : dbl(col(x.saved, "TaxPercent"));
                pct = l.TaxPercent > 0 ? l.TaxPercent : schedulePct;
                if (!taxEditable && pct != schedulePct && !(savedMatch && pct == dbl(col(x.saved, "TaxPercent")))) {
                    throw new IllegalArgumentException("Tax Percent is not editable");
                }
                x.taxName = scheduleMatch ? Objects.toString(col(s0, "TaxName"), "") : Objects.toString(col(x.saved, "TaxDescriptions"), "");
            }
            x.tax = taxNameId > 0 ? x.amount * pct / 100.0 : 0d;
            require(x.tax > 0 && taxNameId == 0, "Tax Name", n);
            require(taxNameId > 0 && pct == 0, "Tax Percent", n);
            l.TaxPercent = taxNameId > 0 ? pct : 0;
            // reference document :1280 / Insert :2512
            if (l.RefDocumentTypeId > 0) {
                if (!refTypes.contains(l.RefDocumentTypeId)) throw new IllegalArgumentException("Select a document type from the list");
                require(l.RefDocId == 0 && l.RefDocInvoiceId == 0, "Reference Document", n);
                var refs = refCache.computeIfAbsent(l.RefDocumentTypeId, t -> repo.refDocs(u, t, insert ? 0 : r.Id));
                if (l.RefDocId > 0 && refs.stream().noneMatch(f -> intOf(col(f, "RefDocId")) == l.RefDocId)) throw new IllegalArgumentException("Select a reference document from the list");
                if (l.RefDocInvoiceId > 0 && refs.stream().noneMatch(f -> intOf(col(f, "RefDocInvoiceId")) == l.RefDocInvoiceId)) throw new IllegalArgumentException("Select a reference invoice from the list");
            } else {
                l.RefDocId = 0;
                l.RefDocInvoiceId = 0;
            }
            // txtExchangeRate_TextChanged :1560
            x.fcy = exchangeRate > 0 ? x.amount / exchangeRate : 0d;
            rows.add(x);
        }
        // DeleteDetailrow :2325 - a saved row that left the grid is checked the way the X button checks it
        if (!insert) for (Integer id : saved.keySet()) if (!kept.contains(id)) repo.stockInReferenceValidation(u, r.Id, id);

        // -------------------------------------------------------- freight / journal grids :2381-2396
        var accounts = repo.accounts(u, subsidiary, grn);
        List<PurchaseInvoiceDirectPmRequest.Freight> freight = r.freight == null ? List.of() : r.freight;
        List<PurchaseInvoiceDirectPmRequest.Journal> journal = r.journal == null ? List.of() : r.journal;
        double freightCredit = 0;
        for (var f : freight) {
            if (f == null) throw new IllegalArgumentException("Invalid freight row");
            if (f.Freight < 0) throw new IllegalArgumentException("Freight amounts cannot be negative");
            if (f.Freight > 0 && f.Transporter == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
            if (f.Transporter != 0 && account(accounts, f.Transporter) == null) throw new IllegalArgumentException("Select a transporter from the list");
            freightCredit += f.Freight;
        }
        double jDebit = 0, jCredit = 0;
        for (var j : journal) {
            if (j == null) throw new IllegalArgumentException("Invalid journal row");
            if (j.Credit < 0 || j.Debit < 0) throw new IllegalArgumentException("Journal amounts cannot be negative");
            if (j.Credit > 0 && j.Debit > 0) throw new IllegalArgumentException("Debit Side is aleady added");
            if ((j.Credit > 0 || toInt32(j.Debit) > 0) && j.AccountId == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
            if (j.AccountId != 0) {
                if (account(accounts, j.AccountId) == null) throw new IllegalArgumentException("Select an account from the list");
                if (supplierGl == j.AccountId) throw new IllegalArgumentException("Supplier Account Not select");       // grdGLedger_CellUpdated :1972
            }
            if (j.AccountId > 0) { jDebit += j.Debit; jCredit += j.Credit; }
        }

        // -------------------------------------------------------- BillAmount :2899 / FreightProportion :2991
        double totalAmount = 0, totalTax = 0, totalQty = 0, totalFcy = 0;
        for (Row x : rows) { totalAmount += x.amount; totalTax += x.tax; totalQty += x.req.ItemQty; totalFcy += x.fcy; }
        for (Row x : rows) x.freight = freightCredit > 0 ? Math.rint(freightCredit) / totalQty * x.req.ItemQty : 0d;
        double billAmount = Math.rint(totalAmount + totalTax + jDebit - jCredit);                  // TransporterCredit is never assigned
        double fcyAmount = away(totalFcy, fcyDp);                                                     // CalculateTotalInformation :1593
        if (multi && fcyAmount == 0) throw new IllegalArgumentException("Fcy Amount Rate Field is Required");

        int taxAccountId = r.TaxAccountId;
        if (totalTax > 0 && (taxAccountId == 0 || pi.taxAccounts(u).stream().noneMatch(a -> intOf(a.get("Id")) == taxAccountId))) {
            throw new IllegalArgumentException("TaxAccount Field is Required");
        }

        List<Map<String, Object>> details = new ArrayList<>();
        Timestamp now = new Timestamp(System.currentTimeMillis());
        int line = 0;
        for (Row x : rows) {
            var l = x.req;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", l.Id);
            d.put("WarehouseId", l.WarehouseId);
            d.put("ItemId", l.ItemId);
            d.put("ItemConditionId", l.ItemConditionId);
            d.put("RackId", l.RackId);
            d.put("CropYearId", l.CropYearId);
            d.put("CropYear", x.cropYear);
            d.put("JobLotId", l.JobLotId);
            d.put("ExpiryDate", pickerDate(l.ExpiryDate, now));
            d.put("PackingDate", pickerDate(l.PackingDate, now));
            d.put("ItemUOMId", l.UOMId);
            d.put("ItemQty", l.ItemQty);
            d.put("GrossWeight", l.ItemQty);
            d.put("NetBillWeight", l.ItemQty);
            d.put("NetStockWeight", l.ItemQty);
            d.put("ItemRate", l.Rate);
            d.put("UomScheduleIdRate", l.RateUOMId);
            d.put("ItemAmount", x.amount);
            d.put("TaxNameId", l.TaxNameId);
            d.put("TaxDescriptions", x.taxName);
            d.put("TaxPercent", l.TaxPercent);
            d.put("TaxAmount", x.tax);
            d.put("CurrencyId", currencyId);
            d.put("ExchangeRate", exchangeRate);
            d.put("FcyAmount", x.fcy);
            d.put("RemarksDetail", l.RemarksDetail == null ? "" : l.RemarksDetail.trim());
            d.put("GpDate", now);
            d.put("GpNo", toInt(l.GpNo));
            d.put("VehicleNo", l.VehicleNo == null ? "" : l.VehicleNo.trim());
            d.put("FreightAmount", x.freight);
            d.put("BranchId", x.branchId);
            d.put("RefDocumentTypeId", l.RefDocumentTypeId);
            d.put("RefDocId", l.RefDocId);
            d.put("RefDocInvoiceId", l.RefDocInvoiceId);
            d.put("LineId", ++line);                                                                    // DAL :64
            // DAL :80-83 - 245 is the "else" branch: the form's BillAmount (= ItemAmount) is replaced by
            // ItemAmount + FreightAmount (+ WagesAmount when ContractWagesChargetoProduct; this form never sets it)
            d.put("BillAmount", x.amount + x.freight);
            details.add(d);
        }
        List<Map<String, Object>> freightRows = new ArrayList<>();
        for (var f : freight) {
            if (f.Transporter == 0) continue;
            var a = account(accounts, f.Transporter);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("TansporterId", intOf(a.get("Id")));                                                 // GlAccountId via SupCustIdUpdateforFrieghtGrid
            m.put("SupplierCustomerId", f.Transporter);
            m.put("FreightAmount", toInt32(f.Freight));
            m.put("Remarks", f.Remarks == null ? "" : f.Remarks);
            freightRows.add(m);
        }
        List<Map<String, Object>> journalRows = new ArrayList<>();
        for (var j : journal) {
            if (j.AccountId == 0) continue;
            var a = account(accounts, j.AccountId);
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ChartofAccountId", intOf(a.get("Id")));
            m.put("SupplierCustomerId", j.AccountId);
            m.put("JvRemarks", j.Remarks == null ? "" : j.Remarks);
            m.put("JvPrcnt", j.Percentage);
            m.put("JvQty", j.Qty);
            m.put("JvRate", j.Rate);
            m.put("JvDebit", j.Debit);
            m.put("JvCredit", j.Credit);
            journalRows.add(m);
        }

        // -------------------------------------------------------- header :2433-2478
        Map<String, Object> h = new LinkedHashMap<>();
        if (!insert) h.put("Id", r.Id);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("FinancialYearId", fy);
        h.put("BranchesId", u.getBranchesId());
        h.put("ProjectsId", u.getBranchesId());                                                         // :2455
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        h.put("EntryUser", u.getId());
        h.put("ModifyUser", u.getId());
        h.put("PostUser", u.getId());
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("PostDate", now);
        h.put("DocDate", new Timestamp(docDate.getTime()));
        h.put("DocNo", docNo);
        h.put("BranchSrNo", insert ? repo.nextBranchSrNo(u, fy) : intOf(col(old, "BranchSrNo")));
        h.put("SupplierCustomerId", r.SupplierId);
        h.put("ManualBillNo", r.ManualBillNo == null ? "" : r.ManualBillNo.trim());
        h.put("DueDate", now);
        h.put("SupplierInvoiceDate", now);
        h.put("RemarksHeader", r.RemarksHeader == null ? "" : r.RemarksHeader.trim());
        h.put("BillAmount", billAmount);
        h.put("TaxAccountId", taxAccountId);
        h.put("IsTaxable", totalTax > 0);
        h.put("ScreenName", SCREEN);
        h.put("InvoiceQty", totalQty);                                                                  // DAL :39-40
        h.put("InvoiceWeight", totalQty);
        h.put("CurrencyId", currencyId);
        h.put("ExchangeRate", exchangeRate);
        h.put("FcyAmount", fcyAmount);
        h.put("ActionId", insert ? 1 : 2);

        // -------------------------------------------------------- PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice
        var fin = writes.accounts(Map.of("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()), details);
        if (fin.stockAgainst() <= 0) throw new IllegalArgumentException("Stock AgainstAc Configuration Not Found");
        if (!fin.parties().isEmpty() && fin.parties().get(r.SupplierId) == null) throw new IllegalArgumentException("Supplier GLAccountId not Found");
        var party = fin.parties().isEmpty() ? Map.<String, Object>of() : PurchaseInvoiceFinancialRules.copy(fin.parties().get(r.SupplierId));
        int refAccount = intOf(party.get("GlAccountId"));
        String company = Objects.toString(party.get("CompanyName"), "");
        String remarks = Objects.toString(h.get("RemarksHeader"), "");
        Map<String, Object> vh = new LinkedHashMap<>();
        vh.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        vh.put("DocumentTypeSrNo", insert ? 0 : r.Id);
        vh.put("RefDocNoId", insert ? 0 : r.Id);                                                        // obj.Id before the insert
        vh.put("VoucherCode", docNo);
        vh.put("VoucherDate", new Timestamp(docDate.getTime()));
        vh.put("RemarksOtherLingo", "");
        vh.put("VoucherAmount", billAmount);
        vh.put("MultiCurrencyId", currencyId);
        vh.put("ExchangeCurrencyRate", exchangeRate);
        vh.put("FcAmount", fcyAmount);
        vh.put("RefAccountId", refAccount);
        vh.put("AgainstAccountId", 0);
        vh.put("ChequeDate", Timestamp.valueOf(LocalDate.now().atStartOfDay()));
        vh.put("IncludeWHT", false);
        vh.put("BranchId", u.getBranchesId());
        vh.put("ProjectId", u.getBranchesId());
        vh.put("BillAmount", billAmount);
        vh.put("ManualBillNo", h.get("ManualBillNo"));
        vh.put("DueDate", now);
        vh.put("DueDays", 0);
        vh.put("OrganizationId", u.getOrganizationId());
        vh.put("CompanyId", u.getCompanyId());
        vh.put("FinancialYearId", fy);
        vh.put("EntryUser", u.getId());
        vh.put("EntryDate", now);
        vh.put("ModifyDate", now);
        vh.put("ModifyUser", u.getId());
        if (!remarks.isEmpty()) vh.put("Remarks", remarks);
        List<Map<String, Object>> vd = voucher(details, rows, freightRows, journalRows, fin, refAccount, r.SupplierId, company, u.getBranchesId());

        // attachments are written to disk before the header (DAL :46-51)
        List<Map<String, Object>> newFiles = storeFiles(u, r);
        List<Map<String, Object>> keptFiles = new ArrayList<>();
        if (!insert) {
            Set<Integer> drop = new HashSet<>(r.removeAttachmentIds == null ? List.of() : r.removeAttachmentIds);
            for (var a : repo.attachments(u, r.Id)) if (!drop.contains(intOf(col(a, "Id")))) keptFiles.add(a);
        }
        List<String> names = new ArrayList<>(), custom = new ArrayList<>();
        for (var a : keptFiles) { names.add(Objects.toString(col(a, "Attachment"), "")); custom.add(Objects.toString(col(a, "UploadedFileCustomName"), "")); }
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
            attachments(u, id, r.SupplierId, r, newFiles);
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
            for (var v : vd) { v.put("VoucherHeadId", voucherId); writes.execute("Sp_VoucherDetail_Insert", v); }
            ProcExec.call(jdbc, "EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?, @CompanyId=?, @Id=?", u.getOrganizationId(), u.getCompanyId(), voucherId);
            int historyRef = writes.execute("Sp_VoucherHead_H_Insert", vh);
            for (var v : vd) { v.put("VoucherHeadId", voucherId); v.put("DocumentTypeIdRef", historyRef); writes.execute("Sp_VoucherDetail_H_Insert", v); }
            grn.inventoryTransactions(u, DOCUMENT_TYPE_ID, id);                                         // DAL :524 - 245 is in the set
            ProcExec.run(jdbc, "EXEC [DAW].[USp_DocumentApprovalDetail_Insert] @OrganizationId=?, @CompanyId=?, @DocumentTypeId=?, @Id=?, @LimitAmount=?",
                    u.getOrganizationId(), u.getCompanyId(), DOCUMENT_TYPE_ID, id, BigDecimal.valueOf(billAmount));
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", id);
        out.put("docNo", docNo);
        out.put("voucherHeadId", repo.voucherHeadId(u, id));
        out.put("message", insert ? "Record Saved Successfully" : "Record Update Successfully");
        return out;
    }

    /** MakeVoucherForPurchaseInvoice for 245: Trading, Freight, SupplierAddLess (SalesTax does not list 245). */
    private List<Map<String, Object>> voucher(List<Map<String, Object>> details, List<Row> rows, List<Map<String, Object>> freightRows,
                                              List<Map<String, Object>> journalRows, PurchaseInvoiceFinancialRules.Accounts fin,
                                              int ref, int supplierId, String company, int branchesId) {
        List<Map<String, Object>> out = new ArrayList<>();
        double freightSum = 0, weightSum = 0;
        // ItemAndSupplierTradingFinancial :1282, case 245 - JobLotDescription and UOMCodeItem are never set on the model
        for (int i = 0; i < details.size(); i++) {
            var d = details.get(i);
            var item = fin.items().get(intOf(d.get("ItemId")));
            if (item == null) throw new IllegalArgumentException("Item Purchase GL Account Not found");
            var it = PurchaseInvoiceFinancialRules.copy(item);
            int lotAccount = fin.lots().getOrDefault(intOf(d.get("JobLotId")), 0);
            int gl = lotAccount > 0 ? lotAccount : intOf(it.get("PurchaseGLAC"));
            double amount = dbl(d.get("ItemAmount")), fr = dbl(d.get("FreightAmount"));
            int gpNo = intOf(d.get("GpNo"));
            String vehicle = Objects.toString(d.get("VehicleNo"), ""), rem = Objects.toString(d.get("RemarksDetail"), "");
            StringBuilder sb = new StringBuilder().append(Objects.toString(it.get("ItemName"), "")).append(" ").append("").append(" ").append("")
                    .append(" ").append(cs(dbl(d.get("NetBillWeight")))).append(" @").append(cs(dbl(d.get("ItemRate")))).append("/-");
            if (gpNo > 0) sb.append(" GP# ").append(gpNo);
            if (!vehicle.isBlank()) sb.append(" V# ").append(vehicle);
            if (!rem.isBlank()) sb.append(" ").append(rem);
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
                v.put("JobLotId", d.get("JobLotId"));
                v.put("QtyIn", d.get("ItemQty"));
                v.put("ItemRate", d.get("ItemRate"));
                v.put("WeightIn", d.get("NetBillWeight"));
                v.put("RateCut", 0d);
                v.put("RateCutAmount", 0d);
                v.put("ItemAmount", amount);
                v.put("Freight", fr);
                v.put("Commission", 0d);
                v.put("OrderNo", 0);
                v.put("GpNo", gpNo);
                v.put("VehicleNo", vehicle);
                v.put("SupplierCustomerId", supplierId);
                if (leg == 1) { v.put("SubsidiaryAccountId", supplierId); v.put("SubsidiaryTypeId", 1); }
                v.put("BranchesId", d.get("BranchId"));
                out.add(v);
            }
            freightSum += fr;
            weightSum += dbl(d.get("NetBillWeight"));
        }
        if (freightSum > 0) {
            for (var d : details) {
                var it = PurchaseInvoiceFinancialRules.copy(fin.items().get(intOf(d.get("ItemId"))));
                int lotAccount = fin.lots().getOrDefault(intOf(d.get("JobLotId")), 0);
                double fr = dbl(d.get("FreightAmount"));
                Map<String, Object> v = new LinkedHashMap<>();
                v.put("AccountId", lotAccount > 0 ? lotAccount : intOf(it.get("PurchaseGLAC")));
                v.put("AgainstAccountId", fin.stockAgainst());
                v.put("DebitAmount", fr);
                v.put("Comments", "ChargeToProduct/TotalWeightInDetail * Qty = Proportionated Charges : " + cs(freightSum) + " / " + cs(weightSum)
                        + " * " + cs(dbl(d.get("ItemQty"))) + " = " + cs(Math.rint(fr)));
                v.put("CreditAmount", 0d);
                v.put("ItemId", d.get("ItemId"));
                v.put("JobLotId", d.get("JobLotId"));
                v.put("BranchesId", d.get("BranchId"));
                out.add(v);
            }
        }
        // FreightFinancial :292 - PurchaseGLAC is never set on the detail, so AgainstAccountId is the transporter itself
        for (var f : freightRows) {
            int t = intOf(f.get("TansporterId"));
            double cr = dbl(f.get("FreightAmount"));
            if (t <= 0 || cr <= 0) continue;
            String rem = Objects.toString(f.get("Remarks"), "");
            int sc = intOf(f.get("SupplierCustomerId"));
            Map<String, Object> v = new LinkedHashMap<>();
            v.put("AccountId", t);
            v.put("AgainstAccountId", t);
            v.put("Comments", rem.isEmpty() ? "SupplierName :  " + company + "  Freight : " + cs(cr) : rem);
            v.put("DebitAmount", 0d);
            v.put("CreditAmount", cr);
            if (sc > 0) { v.put("SupplierCustomerId", sc); v.put("SubsidiaryAccountId", sc); v.put("SubsidiaryTypeId", 1); }
            v.put("BranchesId", branchesId);
            out.add(v);
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
            v.put("BranchesId", branchesId);
            out.add(v);
            Map<String, Object> w = new LinkedHashMap<>();
            w.put("AccountId", ref); w.put("AgainstAccountId", acc); w.put("Comments", rem);
            w.put("DebitAmount", jc); w.put("CreditAmount", jd);
            w.put("SupplierCustomerId", supplierId); w.put("SubsidiaryAccountId", supplierId); w.put("SubsidiaryTypeId", 1);
            w.put("BranchesId", branchesId);
            out.add(w);
        }
        return out;
    }

    /** btnDelete_Click :2714. */
    @Transactional(isolation = Isolation.SERIALIZABLE)
    public Map<String, Object> delete(int id) {
        UserAccount u = user("Delete");
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        var h = repo.header(u, id);
        if (approved(h)) throw new IllegalArgumentException("Record Not Delete because Record has approved");
        try {
            repo.delete(u, id);
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable c = e.getMostSpecificCause();
            throw new IllegalArgumentException(c == null ? e.getMessage() : c.getMessage());
        }
        return Map.of("message", "Delete Voucher Successfully");
    }

    public Map<String, Object> voucherHead(int id) {
        UserAccount u = user("View");
        repo.header(u, id);
        return Map.of("voucherHeadId", repo.voucherHeadId(u, id));
    }

    // ================================================================== files

    private List<Map<String, Object>> storeFiles(UserAccount u, PurchaseInvoiceDirectPmRequest r) {
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

    private void attachments(UserAccount u, int id, int supplierId, PurchaseInvoiceDirectPmRequest r, List<Map<String, Object>> newFiles) {
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

    /** dtAccountlst row whose combo value (SuppliercustomerId) is this. */
    private static Map<String, Object> account(List<Map<String, Object>> accounts, int value) {
        for (var a : accounts) if (intOf(a.get("SupplierCustomerId")) == value) return a;
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

    /** FormHelper.Require :1089. */
    private static void require(boolean condition, String field, int row) {
        if (condition) throw new IllegalArgumentException(field + " is required (Row " + row + ")");
    }

    private static Timestamp pickerDate(String v, Timestamp fallback) {
        if (blank(v)) return fallback;
        try { return Timestamp.valueOf(LocalDate.parse(v.trim().substring(0, 10)).atStartOfDay()); }
        catch (Exception e) { throw new IllegalArgumentException("Invalid date " + v); }
    }

    /** Math.Round(x, n, MidpointRounding.AwayFromZero) / "#,##0.00" formatting. */
    private static double away(double v, int digits) {
        return BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_UP).doubleValue();
    }

    /** Convert.ToInt32(double) - banker's rounding. */
    private static int toInt32(double v) { return (int) Math.rint(v); }

    /** Conversion.ToInt(string). */
    private static int toInt(String v) {
        if (blank(v)) return 0;
        try { return Integer.parseInt(v.trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** double.ToString() as .NET writes it into a string (15 significant digits, no trailing zeros). */
    static String cs(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return Long.toString((long) v);
        return new BigDecimal(v).round(new MathContext(15)).stripTrailingZeros().toPlainString();
    }

    private static boolean blank(String v) { return v == null || v.isBlank(); }

    private static Date date(String v, String name) {
        if (blank(v)) throw new IllegalArgumentException(name + " is not a valid date");
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
