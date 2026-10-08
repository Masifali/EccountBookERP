package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels.*;
import com.mst.repositories.SaleInvoiceDirect139Repository;
import com.mst.repositories.SaleInvoiceFlourRepository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.repositories.SaleInvoiceFlourRepository.*;
import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Screens 137 "Sale Invoice Direct (Flour)" (Architecture.WinApp.Sale.frmSaleInvoiceDirect, DocumentTypeId 186) and
 * 138 "Sale Invoice Direct (Auto Rated)" (frmSaleInvoiceDirectAutoRated, DocumentTypeId 184).
 *
 *   137  Load :429  Insert :2083  ReadById :2350  BillAmount :2482  ExpProportion :2517  BillProportion :2538  Total :2574
 *        DetailAmountCaluculation :2604  GetAll :2893  btnAdd :1815  btnLoadSaleOrder :3910 (Load From Issuance)
 *   138  Load :465  Insert :2680  ReadById :2965  BillAmount :3099  ExpProportion :3134  BillProportion :3155  Total :3191
 *        DetailAmountCaluculation :3221  DetailDiscAmtCalculation :3255  UpdateRateFromItemPriceScheduleByInGrid :1270
 *        UpdateDiscountTypeAndRateInGrid :1395  BtnPropotionateDiscApply_Click :4735
 *
 * Save = BLL 0580 InvSaleInvoice.Save -> SaleInvoiceFinancialDirect (BLL 0612/0613) -> SaleInvoiceDirectPersist (DAL 0433 SetData),
 * one transaction. The browser sends only what the user typed; the server redoes the form's arithmetic (net weight, item amount,
 * discount, expense and bill proportions, bill amount) and re-reads a loader row's source (LoadavailableTransactionsForIssuance).
 *
 * DESKTOP BEHAVIOUR REPRODUCED
 *  1. Project and Branch are hidden combos that activate the first row; an update uses the stored ids.
 *  2. The detail JobLot of 137 is the "Job/Lot" configuration value; 138 takes the grid row's job lot.
 *  3. 138 re-prices EVERY row from the item price schedule at the start of Insert; a row with no scheduled rate gets Rate 0 and is refused.
 *  4. Detail messages are "X is required in Detail Grid in Row No: n" + "Please Check..." (no space between).
 *  5. A loader row cannot be edited except its Qty (and, in 138, the Discount Rate); a loader row's Qty cannot exceed its balance.
 *
 * DEVIATIONS (web-only): DocNo is the generator value on insert; the party-add/less (journal) grid is hidden on the desktop form
 * (an off-screen panel) so it is not drawn and no journal lines are saved; attachments are not ported; the 302 history print is not ported.
 */
@Service
public class SaleInvoiceFlourService {
    private final SaleInvoiceFlourRepository own;
    private final SaleInvoiceDirect139Repository base;
    private final SaleInvoiceRepository repo;
    private final SaleInvoiceService shared;
    private final SaleInvoiceDirectPersist persist;
    private final StockConversionService loader;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public SaleInvoiceFlourService(SaleInvoiceFlourRepository own, SaleInvoiceDirect139Repository base, SaleInvoiceRepository repo, SaleInvoiceService shared,
                                   SaleInvoiceDirectPersist persist, StockConversionService loader, StoreScreenRights rights, CurrentUserContext ctx) {
        this.own = own; this.base = base; this.repo = repo; this.shared = shared; this.persist = persist; this.loader = loader; this.rights = rights; this.ctx = ctx;
    }

    private UserAccount user() { return ctx.requireAccountingUser(); }
    public static String screen(int doc) { return doc == DOC_AUTO_RATED ? SCREEN_AUTO_RATED : SCREEN_FLOUR; }
    private Map<String, Boolean> r(int doc) { return rights.of(screen(doc)); }

    // ================================================================================= load

    private int amountDecimals(UserAccount u) {
        String t = repo.config(u, "Default NoofDecimal Points For Amount").trim();
        int n = ci(t);
        return n < 0 || n > 15 ? 0 : n;
    }
    /** clsGlobalVariables.DecimalRateFormate: 0 (or missing) -> 2, 1..4 -> n, else 0. */
    private int rateDecimals(UserAccount u) {
        int n = ci(repo.config(u, "Default NoofDecimal Points For Rate").trim());
        return n == 0 ? 2 : (n >= 1 && n <= 4 ? n : 0);
    }

    /** Load :429 / :465 - the lists the form binds, in its order. */
    public Map<String, Object> lookups(int doc) {
        UserAccount u = user();
        boolean auto = doc == DOC_AUTO_RATED;
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", r(doc));
        Map<String, Object> cfg = new LinkedHashMap<>();
        cfg.put("amountDecimals", amountDecimals(u));
        cfg.put("rateDecimals", rateDecimals(u));
        cfg.put("creditAmountInItemSaleGL", repo.config(u, "CreditAmountInItemSaleGL").trim());
        cfg.put("defaultCropYearId", ci(repo.config(u, "Default Crop Year").trim()));
        cfg.put("defaultPackingTypeId", ci(repo.config(u, "Paking Type").trim()));
        cfg.put("defaultWarehouseId", ci(repo.config(u, "Warehouse").trim()));
        String lot = repo.config(u, "Job/Lot").trim();
        cfg.put("defaultJobLotId", ci(lot));
        cfg.put("jobLotConfigured", !lot.isEmpty());
        LocalDateTime start = repo.financialYearStart(u, ctx.currentFinancialYearId());
        cfg.put("yearStart", start == null ? null : start.toLocalDate().toString());
        m.put("configuration", cfg);
        m.put("customers", auto ? repo.customers(u, false) : base.customers(u));
        m.put("dueTerms", base.dueTerms(u));
        m.put("deliveryTerms", deliveryTerms());
        m.put("warehouses", base.warehouses(u));
        m.put("cities", own.cities(u));
        m.put("cropYears", own.cropYears(u));
        m.put("items", auto ? own.itemsAutoRated(u) : own.itemsFlour(u));
        m.put("packingTypes", base.packingTypes());
        m.put("otherItems", base.otherItems(u));
        if (auto) {
            m.put("jobLots", base.jobLots(u));
            m.put("discountTypes", discountTypes());
        }
        m.putAll(numbers(doc));
        return m;
    }

    private static List<Map<String, Object>> deliveryTerms() {
        List<Map<String, Object>> l = new ArrayList<>();
        l.add(two("Id", 1, "DeliveryTerm", "Load")); l.add(two("Id", 2, "DeliveryTerm", "Ponch"));
        return l;
    }
    private static List<Map<String, Object>> discountTypes() {
        List<Map<String, Object>> l = new ArrayList<>();
        l.add(two("Id", 1, "DiscountType", "Flat")); l.add(two("Id", 2, "DiscountType", "Percent"));
        return l;
    }
    private static Map<String, Object> two(String k1, Object v1, String k2, Object v2) {
        Map<String, Object> x = new LinkedHashMap<>();
        x.put(k1, v1); x.put(k2, v2);
        return x;
    }

    /** DocumentNo(). */
    public Map<String, Object> numbers(int doc) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", repo.nextDocNo(user(), ctx.currentFinancialYearId(), doc));
        return m;
    }

    /** PackUOM(). */
    public List<Map<String, Object>> uoms(int itemId) { return base.uomsOfItem(user(), itemId); }

    /** RateUOMFromSchedule() (138). */
    public List<Map<String, Object>> rateUoms(int itemId, String docDate) { return own.rateUomsFromSchedule(user(), itemId, dtOrNow(docDate)); }

    /** GetRateFromItemPriceScheduleByItemId() (138) - the rate of the scheduled Rate UOM. */
    public Map<String, Object> rate(int itemId, String docDate, int rateUomId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rate", own.rateFromSchedule(user(), itemId, dtOrNow(docDate), rateUomId));
        return m;
    }

    /** AvailableStockGetByItem(). */
    public Map<String, Object> stock(int itemId, String docDate, int warehouseId, int jobLotId, String cropYear, int packingTypeId, int stockUom) {
        double v = own.currentStock(user(), itemId, dtOrNow(docDate), warehouseId, jobLotId, cropYear, packingTypeId, stockUom);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("stock", v > 0.0 ? v : 0.0);
        return m;
    }

    public List<Map<String, Object>> historyCustomers(int doc) { return own.historyCustomers(user(), doc); }

    public List<Map<String, Object>> history(int doc, String dateMode, String from, String to, int fromDocNo, int toDocNo, int customerId) {
        UserAccount u = user();
        return own.history(u, doc, ctx.currentFinancialYearId(), r(doc).get("viewAll"), dateMode, dtOrNull(from), dtOrNull(to), fromDocNo, toDocNo, customerId);
    }

    /** ReadById :2350 - InvSaleInvoice.GetByID. */
    public Map<String, Object> read(int doc, int id) { return shared.read(id, doc); }

    /** LoadavailableTransactionsForIssuance - Load. */
    public Map<String, Object> loaderSetup() { return loader.loaderSetup(); }

    /** LoadavailableTransactionsForIssuance - btngrnlod. */
    public List<Map<String, Object>> loaderSearch(String fromDate, String toDate, int parentCategoryId, int itemCategoryId, int itemTypeId, int jobLotId,
                                                  String cropYear, int warehouseId, int refDocumentTypeId, int supplierCustomerId, int itemId) {
        return loader.loaderSearch(fromDate, toDate, parentCategoryId, itemCategoryId, itemTypeId, jobLotId, cropYear, warehouseId, refDocumentTypeId, supplierCustomerId, itemId);
    }

    /** btnDelete_Click. */
    @Transactional
    public Map<String, Object> delete(int doc, int id) {
        if (!r(doc).get("delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        Map<String, Object> m = read(doc, id);
        if (b(col(m, "IsApproved"))) throw new IllegalArgumentException(doc == DOC_AUTO_RATED ? "Record Not Delete beacause Record has approved" : "Record Not Delete because Record has approved");
        repo.delete(user(), id, doc);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("message", "Delete Voucher Successfully");
        return out;
    }

    // ================================================================================= save (Insert)

    /** One grid row while Insert builds it (the cells of the Janus row). */
    private static final class Row {
        int id, refType, refId, refSub, saleOrderId, saleOrderDetailId, gdnId, gdnDetailId;
        int warehouseId, itemId, jobLotId, packingTypeId, packUomId, rateUomId, discountTypeId, cityId, gpNo;
        String cropYear = "", vehicleNo = "";
        LocalDateTime gpDate;
        double qty, netWt, rate, cgsRate, rateEq, packEq, amountWithoutDisc, discountRate, discountAmount, itemAmount, expense, billAmount;
        double balQty, balWt, sourceAmount;
        boolean loader;
    }

    @Transactional
    public Map<String, Object> save(int doc, Map<String, Object> req) {
        boolean auto = doc == DOC_AUTO_RATED;
        UserAccount u = user();
        int year = ctx.currentFinancialYearId();
        Map<String, Boolean> rt = r(doc);
        int id = i(req.get("Id"));
        if (!rt.get(id > 0 ? "update" : "save")) throw new IllegalStateException("You do not have the " + (id > 0 ? "Update" : "Save") + " right for this screen.");
        Map<String, Object> existing = null;
        if (id > 0) {
            existing = read(doc, id);
            if (b(col(existing, "IsApproved"))) throw new IllegalArgumentException(auto ? "Record Not Update beacause Record has approved" : "Record Not Update because Record has approved");
        }
        int amtDec = amountDecimals(u), rateDec = rateDecimals(u);
        List<Map<String, Object>> grid = rows(req.get("details"));
        List<Map<String, Object>> expGrid = rows(req.get("expenses"));
        LocalDateTime docDate = dt(req.get("DocDate"));
        if (docDate == null) throw new IllegalArgumentException("Doc Date is required");

        /* UpdateRateFromItemPriceScheduleByInGrid() runs first in 138 (returns at once on an empty grid). */
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        /* FormValidation */
        List<Map<String, Object>> branches = own.branches(u), projects = own.projects(u);
        int branchValue = id > 0 ? i(col(existing, "BranchesId")) : (branches.isEmpty() ? 0 : i(col(branches.get(0), "Id")));
        if (branchValue == 0 || branches.stream().noneMatch(x -> i(col(x, "Id")) == branchValue)) throw new IllegalArgumentException("Branch Field is Required");
        int projectValue = id > 0 ? i(col(existing, "ProjectsId")) : (projects.isEmpty() ? 0 : i(col(projects.get(0), "Id")));
        if (projectValue == 0 || projects.stream().noneMatch(x -> i(col(x, "Id")) == projectValue)) throw new IllegalArgumentException("Project Field is Required");
        int customerId = i(req.get("SupplierCustomerId"));
        List<Map<String, Object>> customers = auto ? repo.customers(u, false) : base.customers(u);
        if (customerId == 0 || customers.stream().noneMatch(x -> i(col(x, "Id")) == customerId)) throw new IllegalArgumentException("CustomerName Field is Required");
        int docNo = id > 0 ? i(col(existing, "DocNo")) : repo.nextDocNo(u, year, doc);
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        int paymentTermId = i(req.get("PaymentTermId"));
        Map<String, Object> term = base.dueTerms(u).stream().filter(t -> i(col(t, "Id")) == paymentTermId).findFirst().orElse(null);
        String dueDaysText = s(req.get("DueDays")).trim();
        if ("Credit".equals(term == null ? "" : s(col(term, "TermsDescription"))) && dueDaysText.isEmpty()) throw new IllegalArgumentException("Due Days Field is Required");

        List<Map<String, Object>> otherItems = base.otherItems(u);
        for (var e : expGrid)
            if (d(e.get("Amount")) > 0.0 && i(e.get("ItemId")) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");

        /* ------------------------------------------------ the grid */
        List<Map<String, Object>> savedLines = existing == null ? List.of() : rows(existing.get("details"));
        Map<Integer, Map<String, Object>> savedById = new HashMap<>();
        for (var x : savedLines) savedById.put(i(col(x, "Id")), x);
        List<Map<String, Object>> warehouses = base.warehouses(u), jobLots = auto ? base.jobLots(u) : List.of(), packingTypes = base.packingTypes();
        List<Map<String, Object>> items = auto ? own.itemsAutoRated(u) : own.itemsFlour(u);
        Set<Integer> whIds = ids(warehouses), lotIds = ids(jobLots), packIds = ids(packingTypes);
        Set<Integer> itemIds = ids(items);
        Set<Integer> cityIds = ids(own.cities(u));
        int configLot = ci(repo.config(u, "Job/Lot").trim());
        List<Map<String, Object>> sources = null;           /* loader rows, read once */
        Map<Integer, List<Map<String, Object>>> uomCache = new HashMap<>();
        Map<Integer, List<Map<String, Object>>> rateUomCache = new HashMap<>();
        List<String> warnings = new ArrayList<>();
        boolean anyLoader = false, anyManual = false;
        List<Row> rowsOut = new ArrayList<>();
        int rowNo = 0;
        for (var g : grid) {
            rowNo++;
            Row w = new Row();
            w.id = i(g.get("Id"));
            if (w.id > 0 && (id == 0 || !savedById.containsKey(w.id))) throw new IllegalArgumentException("Detail row " + rowNo + " is not part of this invoice");
            w.refType = i(g.get("RefDocumentTypeId")); w.refId = i(g.get("RefDocIdNo")); w.refSub = i(g.get("RefDocSubIdNo"));
            Map<String, Object> saved = w.id > 0 ? savedById.get(w.id) : null;
            if (saved != null) {            /* an existing line keeps its own references */
                w.refType = i(col(saved, "RefRefDocumentTypeId")); w.refId = i(col(saved, "RefRefDocIdNo")); w.refSub = i(col(saved, "RefDocSubId"));
            }
            w.loader = w.refType > 0 || w.refSub > 0 || w.refId > 0;
            if (w.loader) anyLoader = true; else anyManual = true;
            w.saleOrderId = saved != null ? i(col(saved, "SaleOrderId")) : 0;
            w.saleOrderDetailId = saved != null ? i(col(saved, "SaleOrderDetailId")) : 0;
            w.gdnId = saved != null ? i(col(saved, "InvGdnId")) : 0;
            w.gdnDetailId = saved != null ? i(col(saved, "InvGdnDetailId")) : 0;
            w.discountTypeId = i(g.get("DiscountTypeId")); w.discountRate = d(g.get("DiscountRate"));
            if (w.loader) {
                if (saved != null) {
                    w.warehouseId = i(col(saved, "WarehouseId")); w.itemId = i(col(saved, "ItemId")); w.cropYear = s(col(saved, "CropYear"));
                    w.jobLotId = i(col(saved, "JobLotId")); w.packingTypeId = i(col(saved, "PackingTypeId")); w.packUomId = i(col(saved, "ItemUOMId"));
                    w.packEq = d(col(saved, "PackUom")); w.balQty = d(col(saved, "ItemQty")); w.balWt = d(col(saved, "NetBillWeight"));
                    w.rate = d(col(saved, "ItemRate")); w.cgsRate = d(col(saved, "ItemCgsRate")); w.rateUomId = i(col(saved, "UomScheduleIdRate"));
                    w.rateEq = d(col(saved, "RateUOM")); w.sourceAmount = d(col(saved, "ItemAmount")); w.cityId = i(col(saved, "CityId"));
                    w.gpDate = dt(col(saved, "GpDate")); w.gpNo = i(col(saved, "GpNo")); w.vehicleNo = s(col(saved, "VehicleNo"));
                } else {
                    if (sources == null) sources = loader.loaderSearch(null, null, 0, 0, 0, 0, null, 0, 0, 0, 0);
                    Map<String, Object> src = null;
                    for (var x : sources)
                        if (i(x.get("RefDocumentTypeId")) == w.refType && i(x.get("RefDocIdNo")) == w.refId && i(x.get("RefDocSubIdNo")) == w.refSub) { src = x; break; }
                    if (src == null) throw new IllegalArgumentException("Loaded record of row " + rowNo + " is no longer available");
                    w.warehouseId = i(src.get("WarehouseId")); w.itemId = i(src.get("ItemId")); w.cropYear = s(src.get("CropBatch"));
                    w.jobLotId = i(src.get("JobLotId")); w.packingTypeId = i(src.get("InvPackingTypeId")); w.packUomId = i(src.get("ItemUom"));
                    w.packEq = d(src.get("PackSize")); w.balQty = d(src.get("QtyBalance")); w.balWt = d(src.get("WeightBalance"));
                    w.rate = auto ? 0 : d(src.get("AVgRate")); w.cgsRate = d(src.get("AVgRate")); w.rateUomId = i(src.get("RateUomId"));
                    w.rateEq = d(src.get("Equivalent")); w.sourceAmount = d(src.get("ItemAmount"));
                    w.gpDate = LocalDateTime.now();
                }
                /* grd_CellUpdated: Qty may not exceed the balance; Net weight follows Qty and may not exceed the balance weight */
                w.qty = d(g.get("ItemQty"));
                if (w.qty > w.balQty) throw new IllegalArgumentException("Item Cannot be greater than Balance Qty " + fmtNum(w.balQty) + " Of this row");
                if (w.qty == w.balQty) w.netWt = w.balWt;
                else { w.netWt = w.packEq * w.qty; if (w.netWt > w.balWt) w.netWt = w.balWt; }
                if (!auto) w.itemAmount = w.qty == w.balQty ? w.sourceAmount : (w.netWt > 0 ? (w.rateEq > 0 ? w.netWt / w.rateEq * w.rate : 0) : 0);
            } else {
                /* a row typed on the entry bar (btnAdd) */
                w.warehouseId = i(g.get("WarehouseId")); w.itemId = i(g.get("ItemId")); w.cropYear = s(g.get("CropYear"));
                w.jobLotId = auto ? i(g.get("JobLotId")) : configLot; w.packingTypeId = i(g.get("PackingTypeId")); w.packUomId = i(g.get("PackUOMId"));
                w.rateUomId = i(g.get("RateUOMId")); w.qty = d(g.get("ItemQty")); w.rate = d(g.get("Rate"));
                w.cityId = i(g.get("CityId")); w.gpNo = i(g.get("GpNo")); w.vehicleNo = s(g.get("VehicleNo"));
                LocalDateTime gp = dt(g.get("GpDate"));
                w.gpDate = gp != null ? gp : LocalDate.now().atStartOfDay();
                if (w.cityId != 0 && !cityIds.contains(w.cityId)) w.cityId = 0;
                if (w.itemId != 0 && itemIds.contains(w.itemId)) {
                    var packRows = uomCache.computeIfAbsent(w.itemId, k -> base.uomsOfItem(u, k));
                    var pu = packRows.stream().filter(x -> i(col(x, "Id")) == w.packUomId).findFirst().orElse(null);
                    w.packEq = pu == null ? 0 : d(col(pu, "Equivalent"));
                    if (auto) {
                        var rateRows = rateUomCache.computeIfAbsent(w.itemId, k -> own.rateUomsFromSchedule(u, k, docDate));
                        var ru = rateRows.stream().filter(x -> i(col(x, "RateUomId")) == w.rateUomId).findFirst().orElse(null);
                        w.rateEq = ru == null ? 0 : d(col(ru, "Equivalent"));
                    } else {
                        var ru = packRows.stream().filter(x -> i(col(x, "Id")) == w.rateUomId).findFirst().orElse(null);
                        w.rateEq = ru == null ? 0 : d(col(ru, "Equivalent"));
                    }
                }
                /* Total() */
                w.netWt = w.packEq > 0 && w.qty > 0 ? awayZero(w.packEq * w.qty, 2) : 0;
                /* DetailAmountCaluculation() */
                double amt = w.netWt > 0 && w.rateEq > 0 && w.rate > 0 ? awayZero(w.netWt / w.rateEq * w.rate, amtDec) : 0;
                if (!auto) w.itemAmount = amt;
                else w.amountWithoutDisc = amt;
            }
            rowsOut.add(w);
        }
        if (anyLoader && anyManual) throw new IllegalArgumentException("You can not Add Manual Record Becuase Record from Loader Exist in grd");

        /* 138: UpdateRateFromItemPriceScheduleByInGrid() - every row is priced from the schedule */
        if (auto) {
            for (Row w : rowsOut) {
                double sched = own.rateFromSchedule(u, w.itemId, docDate, w.rateUomId);
                if (sched > 0.0) {
                    w.rate = sched;
                    double amt = w.netWt > 0.0 && w.rateEq > 0 ? w.netWt / w.rateEq * w.rate : 0;
                    w.amountWithoutDisc = amt;
                    double dis = 0.0;
                    if (w.discountTypeId != 0) {
                        double rate = w.discountRate;
                        if (w.discountTypeId == 1) dis = rate;
                        else if (w.discountTypeId == 2) {
                            if (rate >= 100.0) { warnings.add("DiscountRate Cannot be greater than 99 when Discount Type is Percent"); rate = 99.0; w.discountRate = rate; }
                            dis = amt * rate / 100.0;
                        }
                        w.discountAmount = dis;
                    } else { w.discountRate = 0; w.discountAmount = 0; }
                    w.itemAmount = amt > 0.0 && dis > 0.0 ? amt - dis : (amt > 0.0 ? amt : 0);
                } else {
                    w.rate = 0;
                }
                if (!w.loader) {
                    /* DetailDiscAmtCalculation() already shaped a manual row; the schedule pass above replaced it the same way */
                }
            }
        }

        /* ExpProportion() / BillProportion() */
        double totalExp = 0.0;
        List<Expense> expenses = new ArrayList<>();
        int er = 0;
        for (var e : expGrid) {
            er++;
            int itemId = i(e.get("ItemId"));
            double qty = d(e.get("Qty")), rate = d(e.get("Rate")), amount = d(e.get("Amount"));
            if (!s(e.get("Qty")).trim().isEmpty() && !s(e.get("Rate")).trim().isEmpty()) amount = awayZero(qty * rate, amtDec);
            else amount = awayZero(amount, amtDec);
            e.put("_amount", amount);
            totalExp += amount;
        }
        double totalWt = rowsOut.stream().mapToDouble(x -> x.netWt).sum();
        String cfgCredit = repo.config(u, "CreditAmountInItemSaleGL").trim();
        double totalItem = 0.0;
        for (Row w : rowsOut) {
            w.expense = awayZero(totalExp / totalWt * w.netWt, amtDec);
            double itemAmount = awayZero(w.itemAmount, amtDec);
            double exp = awayZero(w.expense, amtDec);
            if (!cfgCredit.isEmpty()) w.billAmount = truthy(cfgCredit) ? awayZero(itemAmount, amtDec) : itemAmount;
            else w.billAmount = awayZero(itemAmount + exp, amtDec);
            totalItem += w.itemAmount;
        }
        /* BillAmount() */
        double billAmount = awayZero(awayZero(totalItem, amtDec) + awayZero(totalExp, amtDec), amtDec);

        /* ------------------------------------------------ detail rows (Insert) */
        List<Detail> details = new ArrayList<>();
        int line = 0;
        for (Row w : rowsOut) {
            line++;
            String where = " in Detail Grid in Row No: " + line + "Please Check...";
            if (w.warehouseId == 0 || !whIds.contains(w.warehouseId)) throw new IllegalArgumentException("Warehouse is required" + where);
            if (w.itemId == 0 || (!w.loader && !itemIds.contains(w.itemId))) throw new IllegalArgumentException("ItemId is required" + where);
            if (auto && (w.jobLotId == 0 || (!w.loader && !lotIds.contains(w.jobLotId)))) throw new IllegalArgumentException("JobLotId is required" + where);
            if (w.packingTypeId == 0 || (!w.loader && !packIds.contains(w.packingTypeId))) throw new IllegalArgumentException("PackingType is required" + where);
            if (w.packUomId == 0) throw new IllegalArgumentException("PackUOM is required" + where);
            if (w.qty == 0.0) throw new IllegalArgumentException("ItemQty is required" + where);
            if (w.netWt == 0.0) throw new IllegalArgumentException("NetBillWeight is required" + where);
            if (w.rate == 0.0) throw new IllegalArgumentException("Rate is required" + where);
            if (w.cgsRate == 0.0 && w.loader) throw new IllegalArgumentException("CgsRate is required" + where);
            if (auto && !(w.discountAmount > 0.0)) { w.discountTypeId = 0; w.discountRate = 0; w.discountAmount = 0; }
            if (w.itemAmount == 0.0) throw new IllegalArgumentException("ItemAmount is required" + where);
            Detail v = new Detail();
            v.LineId = line;
            v.Id = id != 0 ? w.id : 0;
            v.RefRefDocumentTypeId = w.refType; v.RefRefDocIdNo = w.refId; v.RefDocSubId = w.refSub;
            v.SaleOrderId = w.saleOrderId; v.SaleOrderDetailId = w.saleOrderDetailId; v.InvGdnId = w.gdnId; v.InvGdnDetailId = w.gdnDetailId;
            v.WarehouseId = w.warehouseId; v.ItemId = w.itemId; v.CropYear = w.cropYear;
            v.PackingTypeId = w.packingTypeId; v.ItemUOMId = w.packUomId; v.ItemQty = w.qty;
            v.GrossWeight = w.netWt; v.NetBillWeight = w.netWt; v.NetStockWeight = w.netWt;
            v.ItemRate = w.rate; v.ItemRateWOExp = java.math.BigDecimal.valueOf(w.rate); v.ItemCgsRate = w.cgsRate; v.UomScheduleIdRate = w.rateUomId;
            if (auto) { v.DiscountTypeId = w.discountTypeId; v.ItemDiscount = w.discountRate; v.ItemDiscountAmount = w.discountAmount; }
            v.ItemAmount = w.itemAmount;
            v.GpDate = w.gpDate; v.GpNo = w.gpNo; v.VehicleNo = w.vehicleNo;
            v.ExpenseAmount = w.expense; v.JournalAmount = 0.0;
            v.BillAmount = awayZero(w.billAmount, amtDec);
            v.CityId = w.cityId;
            v.JobLotId = auto ? w.jobLotId : configLot;
            v.ReserveWareHouse = 0;
            details.add(v);
        }
        int expRow = 0;
        for (var e : expGrid) {
            expRow++;
            int itemId = i(e.get("ItemId"));
            double amount = d(e.get("_amount"));
            if (itemId != 0 || amount != 0.0) {
                if (itemId == 0) throw new IllegalArgumentException("Item is required in Expense Grid in Row No: " + expRow + "Please Check...");
                if (otherItems.stream().noneMatch(x -> i(col(x, "Id")) == itemId)) throw new IllegalArgumentException("Item is required in Expense Grid in Row No: " + expRow + "Please Check...");
                if (amount == 0.0) throw new IllegalArgumentException("Amount is required in Expense Grid in Row No: " + expRow + "Please Check...");
                Expense pe = new Expense();
                pe.InvRevExpItemId = itemId;
                pe.Qty = i(e.get("Qty"));
                pe.Rate = d(e.get("Rate"));
                pe.Amount = amount;
                pe.Remarks = s(e.get("Remarks"));
                expenses.add(pe);
            }
        }

        LocalDateTime now = LocalDateTime.now();
        int dueDays = ci(dueDaysText);
        LocalDateTime dueDate = dueDaysText.isEmpty() ? docDate : docDate.plusDays(dueDays);
        Head h = new Head();
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.FinancialYearId = year;
        h.BranchesId = n(u.getBranchesId());
        h.ProjectsId = projectValue;
        h.DocumentTypeId = doc;
        h.DocNo = docNo;
        h.DocDate = docDate;
        h.SupplierCustomerId = customerId;
        h.ManualBillNo = s(req.get("ManualBillNo")).trim();
        h.DeliveryTerm = s(req.get("DeliveryTerm"));
        h.PaymentTermId = paymentTermId;
        h.DueDate = dueDate;
        h.DueDays = dueDays;
        h.BillAmount = billAmount;
        h.RemarksHeader = s(req.get("RemarksHeader")).trim();
        h.CommAmount = 0.0;
        h.CommissionAgentId = 0;
        h.CommissionRemarks = "";
        h.CommissionType = "";
        h.CommRate = 0.0;
        h.UomScheduleIdCmRate = "";
        h.ScreenName = screen(doc);
        h.ReferencePartyId = 0;
        h.StockPartyId = 0;
        h.SupplierInvoiceDate = dueDate;
        h.SupplierInvoiceNo = dueDays;
        h.EntryDate = now;
        h.ModifyDate = now;
        h.EntryUser = u.getId();
        h.ModifyUser = u.getId();
        h.IsReserve = false;
        h.Id = id;
        h.AttachmentsValues = id > 0 ? nullable(col(existing, "AttachmentsValues")) : "";
        h.CustomAttachmentsValues = id > 0 ? nullable(col(existing, "CustomAttachmentsValues")) : "";

        SaleInvoiceFinancialDirect.Invoice inv = new SaleInvoiceFinancialDirect.Invoice();
        inv.h = h;
        inv.details.addAll(details);
        inv.expenses.addAll(expenses);

        /* BLL 0580 Save */
        if (h.Id == 0) h.ModifyUser = 0; else h.EntryUser = 0;
        SaleInvoiceFinancialDirect.Voucher voucher = new SaleInvoiceFinancialDirect(repo).makeVoucherForSaleInvoice(inv);
        int saved = persist.persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("voucherHeadId", repo.voucherHeadId(h.OrganizationId, h.CompanyId, doc, saved));
        out.put("warnings", warnings);
        out.put("message", (id == 0 ? "Record Saved Successfully [" : "Record Update Successfully [") + h.DocNo + "] ");
        return out;
    }

    // ================================================================================= helpers

    private static Set<Integer> ids(List<Map<String, Object>> rows) {
        Set<Integer> s = new HashSet<>();
        for (var x : rows) s.add(i(col(x, "Id")));
        return s;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rows(Object o) {
        return o instanceof List ? (List<Map<String, Object>>) o : new ArrayList<>();
    }

    private static String nullable(Object o) { return o == null ? null : String.valueOf(o); }

    private static LocalDateTime dtOrNull(String v) { return v == null || v.trim().isEmpty() ? null : dt(v); }
    private static LocalDateTime dtOrNow(String v) { LocalDateTime d = dtOrNull(v); return d == null ? LocalDateTime.now() : d; }

    /** Conversion.ToInt of a text: whole numbers only, else 0. */
    private static int ci(String t) {
        t = t == null ? "" : t.trim();
        return t.matches("[-+]?\\d+") ? Integer.parseInt(t) : 0;
    }

    /** .NET Math.Round(x, digits, MidpointRounding.AwayFromZero). */
    private static double awayZero(double x, int digits) {
        if (Double.isNaN(x) || Double.isInfinite(x)) return x;
        double f = Math.pow(10, digits);
        double y = Math.abs(x) * f;
        double r = Math.floor(y + 0.5);
        return Math.signum(x) * r / f;
    }

    private static String fmtNum(double v) {
        return v == Math.rint(v) && Math.abs(v) < 1e15 ? String.valueOf((long) v) : java.math.BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }
}
