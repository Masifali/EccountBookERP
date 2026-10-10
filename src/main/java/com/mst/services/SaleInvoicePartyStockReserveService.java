package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels.*;
import com.mst.repositories.SaleInvoicePartyStockReserveRepository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.repositories.SaleInvoicePartyStockReserveRepository.DOC;
import static com.mst.repositories.SaleInvoicePartyStockReserveRepository.SCREEN;
import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Screen 143 "Sale Invoice Party Stock Reserve" - Architecture.WinApp.Sale.InvfrmInvSaleInvoiceDirect with FlagForm = 1
 * (ScreenName "SaleInvoicepartyStockReserve", DocumentTypeId 99, header IsReserve = true, detail ReserveWareHouse = the picked reserve warehouse).
 * The same desktop form with FlagForm = 0 is screen 154 (DocumentTypeId 99, IsReserve = false, Load GDN button); this service is written for FlagForm = 1.
 *
 *   Load :622  bindWareHouse :885  ConfigurationDefault :1826  AvailableStockGetByItem :1993  btnAdd :2474  grd_DoubleClick :2531  btnUpdateDetail :2589
 *   FormValidation :3222  FormValidationDetila :3307  Reset :3461  Insert :3664  ReadById :4128  btnDelete :4270  btnRecordsUpdate :4310  GetAll :4470
 *   BillAmount :5195  ExpProportion :5256  BillProportion :5277  CommissionProportion :5313  FreightProportion :5345  Total :5382
 *   AmountCaluculation :5496  TotalCommissionAmount :5541  comsupplier_ValueChanged :5714  KeyDown :6268
 *
 * Save = BLL 0580 InvSaleInvoice.Save -> SaleInvoiceFinancialDirect (BLL 0612/0613) -> SaleInvoiceDirectPersist (DAL 0433 SetData), one transaction.
 * The browser sends what the user typed; the server redoes the form's arithmetic (entry-bar weights and amount, expense / commission / freight
 * proportions, bill amount, foreign amount) for every row the user entered this session. A row loaded from the record and not touched keeps the
 * stored cell values exactly as the desktop grid would post them (ReadById rounds the empty-bag total and weight-cut total with Math.Round, banker's).
 *
 * DEVIATIONS (web-only, listed in reports/D143.md): DocNo and BranchSrNo are the generator values on insert; the Load GDN button is hidden on the
 * FlagForm = 1 form so it is not drawn; attachments are not ported; the desktop's Janus grid layout / print-preview dialogs are not ported.
 */
@Service
public class SaleInvoicePartyStockReserveService {
    private final SaleInvoicePartyStockReserveRepository own;
    private final SaleInvoiceRepository repo;
    private final SaleInvoiceService shared;
    private final SaleInvoiceDirectPersist persist;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public SaleInvoicePartyStockReserveService(SaleInvoicePartyStockReserveRepository own, SaleInvoiceRepository repo, SaleInvoiceService shared,
                                               SaleInvoiceDirectPersist persist, StoreScreenRights rights, CurrentUserContext ctx) {
        this.own = own; this.repo = repo; this.shared = shared; this.persist = persist; this.rights = rights; this.ctx = ctx;
    }

    private UserAccount user() { return ctx.requireAccountingUser(); }
    private Map<String, Boolean> rt() { return rights.of(SCREEN); }

    // ================================================================================= configuration (Load :622, ConfigurationDefault :1826)

    private int amountDecimals(UserAccount u) {
        int n = ci(repo.config(u, "Default NoofDecimal Points For Amount").trim());
        return n < 0 || n > 15 ? 0 : n;
    }
    /** clsGlobalVariables.DecimalRateFormate: 0 (or missing) -> 2, 1..4 -> n, else 0. */
    private int rateDecimals(UserAccount u) {
        int n = ci(repo.config(u, "Default NoofDecimal Points For Rate").trim());
        return n == 0 ? 2 : (n >= 1 && n <= 4 ? n : 0);
    }

    /** The values Load / ConfigurationDefault read into form fields. */
    private Map<String, Object> configuration(UserAccount u, boolean branchFeature, boolean branchImplemented, boolean subsidiary, boolean multiCurrency) {
        Map<String, Object> c = new LinkedHashMap<>();
        c.put("amountDecimals", amountDecimals(u));
        c.put("rateDecimals", rateDecimals(u));
        c.put("creditAmountInItemSaleGL", repo.config(u, "CreditAmountInItemSaleGL").trim());
        c.put("defaultCropYearId", ci(repo.config(u, "Default Crop Year").trim()));
        c.put("defaultPackingTypeId", ci(repo.config(u, "Paking Type").trim()));
        c.put("defaultWarehouseId", ci(repo.config(u, "Warehouse").trim()));
        c.put("defaultCityId", ci(repo.config(u, "City Area").trim()));
        c.put("defaultJobLotId", ci(repo.config(u, "Job/Lot").trim()));
        c.put("baseCurrencyId", ci(repo.config(u, "Base Currency").trim()));
        String rate = repo.config(u, "BaseCurrencyRate").trim();
        c.put("baseCurrencyRate", rate.isEmpty() ? null : d(rate));
        c.put("branchFeature", branchFeature);
        c.put("branchImplemented", branchImplemented);
        c.put("subsidiary", subsidiary);
        c.put("multiCurrency", multiCurrency);
        c.put("saleCostingJobOrderWise", truthy(repo.config(u, "SaleCostingJobOrderWise")));
        String itemStatus = repo.config(u, "GetItemsagainstWarehouse").trim();
        c.put("itemsAgainstWarehouse", !itemStatus.isEmpty() && truthy(itemStatus));
        c.put("wagesStatus", truthy(repo.config(u, "WagesCompulsoryOnSaleInvoiceDirect")));
        c.put("wagesActive", own.wagesActive());
        c.put("outwardFreightAccountId", ci(repo.config(u, "FreightOutwardAc").trim()));
        c.put("commissionDebitToExpenses", truthy(repo.config(u, "DebitAmountChargetoExpenseAcOfCommission")));
        c.put("freightDebitToExpenses", truthy(repo.config(u, "DebitAmountChargetoExpenseAcFreightGrid")));
        LocalDateTime start = repo.financialYearStart(u, ctx.currentFinancialYearId());
        c.put("yearStart", start == null ? null : start.toLocalDate().toString());
        c.put("userBranchName", repo.branchName(u));
        return c;
    }

    /** Load :622 - the lists the form binds, in its order. */
    public Map<String, Object> lookups() {
        UserAccount u = user();
        boolean branchFeature = repo.feature(u, 11);
        boolean branchImplemented = truthy(repo.config(u, "SaleInvoiceDirectBranchWise"));
        boolean subsidiary = repo.feature(u, 4);
        boolean multiCurrency = repo.feature(u, 6);
        Map<String, Object> cfg = configuration(u, branchFeature, branchImplemented, subsidiary, multiCurrency);
        boolean jobOrderWise = (Boolean) cfg.get("saleCostingJobOrderWise");
        boolean freightDr = (Boolean) cfg.get("freightDebitToExpenses");
        int branchParam = branchFeature && branchImplemented ? n(u.getBranchesId()) : 0;
        int outward = (Integer) cfg.get("outwardFreightAccountId");

        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", rt());
        m.put("configuration", cfg);
        m.put("customers", repo.customers(u, subsidiary));
        m.put("dueTerms", own.dueTerms(u));
        m.put("deliveryTerms", deliveryTerms());
        m.put("commissionTypes", commissionTypes());
        m.put("commissionUoms", repo.commissionUoms());
        m.put("warehouses", own.warehouses(u, branchParam));
        List<Map<String, Object>> lotItems = jobOrderWise ? own.jobLotsWithItems(u) : List.of();
        m.put("jobLots", jobOrderWise ? distinctLots(lotItems) : own.jobLots(u, branchParam));
        m.put("jobLotItems", lotItems);
        m.put("cities", own.cities(u));
        m.put("cropYears", own.cropYears(u));
        m.put("packingTypes", own.packingTypes());
        m.put("otherItems", repo.otherItems(u));
        m.put("otherCategories", own.otherCategories(u));
        m.put("items", own.items(u));
        m.put("brands", own.brands(u));
        m.put("currencies", repo.currencies(u));
        m.put("accounts", own.transporterAccounts(u, subsidiary, freightDr));
        m.put("commissionDebitAccounts", (Boolean) cfg.get("commissionDebitToExpenses") ? own.commissionDebitAccounts(u, subsidiary) : List.of());
        m.put("projects", own.projects(u));
        m.put("branches", own.branches(u));
        m.put("historyBranches", own.historyBranches(u, branchImplemented));
        m.putAll(numbers());
        return m;
    }

    private static List<Map<String, Object>> distinctLots(List<Map<String, Object>> rows) {
        Set<Integer> seen = new HashSet<>();
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : rows) {
            if (!seen.add(i(col(r, "Id")))) continue;
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", i(col(r, "Id"))); x.put("JobLotDescription", s(col(r, "JobLotDescription")));
            x.put("BranchId", i(col(r, "BranchId"))); x.put("BranchName", s(col(r, "BranchName")));
            out.add(x);
        }
        return out;
    }
    private static List<Map<String, Object>> deliveryTerms() {
        List<Map<String, Object>> l = new ArrayList<>();
        l.add(two("Id", 1, "DeliveryTerm", "Load")); l.add(two("Id", 2, "DeliveryTerm", "Ponch"));
        return l;
    }
    private static List<Map<String, Object>> commissionTypes() {
        List<Map<String, Object>> l = new ArrayList<>();
        l.add(two("Id", 1, "CommissionType", "Flat")); l.add(two("Id", 2, "CommissionType", "Percent")); l.add(two("Id", 3, "CommissionType", "Comm Weight"));
        return l;
    }
    private static Map<String, Object> two(String k1, Object v1, String k2, Object v2) {
        Map<String, Object> x = new LinkedHashMap<>();
        x.put(k1, v1); x.put(k2, v2);
        return x;
    }

    /** DocumentNo() :1519 + BranchSrNoFill() :1547. */
    public Map<String, Object> numbers() {
        UserAccount u = user();
        int year = ctx.currentFinancialYearId();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", repo.nextDocNo(u, year, DOC));
        m.put("branchSrNo", own.nextBranchSrNo(u, year));
        return m;
    }

    /** ItemdtFillFromAll :1148 with "GetItemsagainstWarehouse" on. */
    public List<Map<String, Object>> itemsByWarehouse(int warehouseId) { return own.itemsByWarehouse(user(), warehouseId); }

    /** PackUOM() :1442. */
    public List<Map<String, Object>> uoms(int itemId) { return own.uoms(user(), itemId); }

    /** AvailableStockGetByItem() :1993. */
    public Map<String, Object> stock(int itemId, String docDate, int warehouseId, int jobLotId, String cropYear, int packingTypeId, int stockUom) {
        double v = own.currentStock(user(), itemId, dtOrNow(docDate), warehouseId, jobLotId, cropYear, packingTypeId, stockUom);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("stock", v > 0.0 ? v : 0.0);
        return m;
    }

    /** cmbCurrency_Leave :1905 - VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher with DocumentTypeIds "1608". */
    public Map<String, Object> exchangeRate(int currencyId) {
        Double r = repo.lastExchangeRate(user(), currencyId, 1608);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rate", r);
        return m;
    }

    /** comsupplier_ValueChanged :5714. */
    public Map<String, Object> ledger(int customerId, String docDate) { return shared.ledger(customerId, docDate); }

    public List<Map<String, Object>> historyBranches() {
        UserAccount u = user();
        return own.historyBranches(u, truthy(repo.config(u, "SaleInvoiceDirectBranchWise")));
    }
    public List<Map<String, Object>> historyCustomers() { return own.historyCustomers(user()); }

    /** GetAll :4470. */
    public List<Map<String, Object>> history(String dateMode, String from, String to, int fromDocNo, int toDocNo, int customerId, String branchIds) {
        UserAccount u = user();
        Set<Integer> allowed = new HashSet<>();
        for (var r : historyBranches()) allowed.add(i(col(r, "Id")));
        StringBuilder ids = new StringBuilder();
        for (String p : (branchIds == null ? "" : branchIds).split(",")) {
            int b = i(p);
            if (b > 0 && allowed.contains(b)) ids.append(',').append(b);
        }
        if (ids.length() == 0) throw new IllegalArgumentException("Select branch first");
        return own.history(u, ctx.currentFinancialYearId(), dateMode, dtOrNull(from), dtOrNull(to), fromDocNo, toDocNo, customerId, ids.toString());
    }

    /** ReadById :4128 - InvSaleInvoice.GetByID, limited to the signed-in company and document type 99. */
    public Map<String, Object> read(int id) { return shared.read(id, DOC); }

    /** btnRecordsUpdate_Click :4310 - the ids the loop walks. */
    public Map<String, Object> autoUpdateIds() {
        UserAccount u = user();
        List<Integer> ids = own.autoUpdateIds(u, ctx.currentFinancialYearId());
        if (ids.isEmpty()) throw new IllegalArgumentException("Ids Not Found For Update");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("ids", ids);
        return m;
    }

    /** btnDelete_Click :4270. */
    @Transactional
    public Map<String, Object> delete(int id) {
        if (!rt().get("delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("RecordId Not Found.....");
        Map<String, Object> m = read(id);
        if (b(col(m, "IsApproved"))) throw new IllegalArgumentException("Record Not Delete because Record has approved");
        repo.delete(user(), id, DOC);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("message", "Delete Record Successfully");
        return out;
    }

    // ================================================================================= save (Insert :3664)

    /** One grid row while Insert builds it (the cells of the Janus row). */
    private static final class Row {
        int id, invGdnId, invGdnDetailId;
        int itemId, brandId, jobLotId, packingTypeId, packUomId, rateUomId, warehouseId, reserveWh, cityId, gpNo, branchId;
        String cropYear = "", vehicleNo = "", itemName = "", jobLotName = "";
        LocalDateTime gpDate;
        double qty, gross, ebUnit, ebTotal, wtCut, wtCutTotal, addLss, net, stock, rate, rateCut, rateCutTotal, itemAmount, rateEq, packEq;
        double expense, commission, freight, billAmount, journal;
        BigDecimal fcy = BigDecimal.ZERO;
        boolean stored;
    }

    @Transactional
    public Map<String, Object> save(Map<String, Object> req) {
        UserAccount u = user();
        int year = ctx.currentFinancialYearId();
        Map<String, Boolean> rts = rt();
        int id = i(req.get("Id"));
        boolean autoUpdate = b(req.get("AutoUpdate"));
        if (autoUpdate && id <= 0) throw new IllegalArgumentException("Ids Not Found For Update");
        if (!rts.get(id > 0 ? "update" : "save")) throw new IllegalStateException("You do not have the " + (id > 0 ? "Update" : "Save") + " right for this screen.");
        Map<String, Object> existing = null;
        if (id > 0) {
            existing = read(id);
            if (!autoUpdate && b(col(existing, "IsApproved"))) throw new IllegalArgumentException("Record Not Update because Record has approved");
        }
        int amtDec = amountDecimals(u);
        boolean branchFeature = repo.feature(u, 11);
        boolean branchImplemented = truthy(repo.config(u, "SaleInvoiceDirectBranchWise"));
        boolean subsidiary = repo.feature(u, 4);
        boolean multiCurrency = repo.feature(u, 6);
        boolean freightDr = truthy(repo.config(u, "DebitAmountChargetoExpenseAcFreightGrid"));
        boolean commDr = truthy(repo.config(u, "DebitAmountChargetoExpenseAcOfCommission"));
        boolean jobOrderWise = truthy(repo.config(u, "SaleCostingJobOrderWise"));
        String itemStatus = repo.config(u, "GetItemsagainstWarehouse").trim();
        boolean itemsAgainstWh = !itemStatus.isEmpty() && truthy(itemStatus);
        int outwardFreightAc = ci(repo.config(u, "FreightOutwardAc").trim());
        String cfgCredit = repo.config(u, "CreditAmountInItemSaleGL").trim();
        int branchParam = branchFeature && branchImplemented ? n(u.getBranchesId()) : 0;

        List<Map<String, Object>> grid = rows(req.get("details"));
        List<Map<String, Object>> expGrid = rows(req.get("expenses"));
        List<Map<String, Object>> frGrid = rows(req.get("freights"));
        List<Map<String, Object>> glGrid = rows(req.get("journals"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        /* ------------------------------------------------ FormValidation :3222 */
        List<Map<String, Object>> branches = own.branches(u), projects = own.projects(u);
        int branchValue = id > 0 ? i(col(existing, "BranchesId")) : (branches.isEmpty() ? 0 : i(col(branches.get(0), "Id")));
        if (branchValue == 0 || branches.stream().noneMatch(x -> i(col(x, "Id")) == branchValue)) throw new IllegalArgumentException("Branch Field is Required");
        int projectValue = id > 0 ? i(col(existing, "ProjectsId")) : (projects.isEmpty() ? 0 : i(col(projects.get(0), "Id")));
        if (projectValue == 0 || projects.stream().noneMatch(x -> i(col(x, "Id")) == projectValue)) throw new IllegalArgumentException("Project Field is Required");
        int customerId = i(req.get("SupplierCustomerId"));
        List<Map<String, Object>> customers = repo.customers(u, subsidiary);
        Map<String, Object> customer = customers.stream().filter(x -> i(col(x, "Id")) == customerId).findFirst().orElse(null);
        if (customerId == 0 || customer == null) throw new IllegalArgumentException("CustomerName Field is Required");
        int customerGl = i(col(customer, "GlAccountId"));
        int docNo = id > 0 ? i(col(existing, "DocNo")) : repo.nextDocNo(u, year, DOC);
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        List<Map<String, Object>> categories = own.otherCategories(u);
        int categoryId = i(req.get("OtherCategoryId"));
        final int categoryIdF = categoryId;
        if (!categories.isEmpty() && (categoryId == 0 || categories.stream().noneMatch(x -> i(col(x, "Id")) == categoryIdF)))
            throw new IllegalArgumentException("Other Category Field is Required");
        if (categories.isEmpty()) categoryId = 0;
        int currencyId = i(req.get("CurrencyId"));
        boolean currencyOk = currencyId != 0 && repo.currencies(u).stream().anyMatch(x -> i(col(x, "Id")) == i(req.get("CurrencyId")));
        BigDecimal exchangeRate = dec(req.get("ExchangeRate"));
        if (multiCurrency) {
            if (!currencyOk) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (exchangeRate.signum() == 0) throw new IllegalArgumentException("Exchange Rate Field is Required");
        } else {
            if (!currencyOk) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (exchangeRate.signum() == 0) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }
        String dueDaysText = s(req.get("DueDays")).trim();
        if (dueDaysText.isEmpty()) throw new IllegalArgumentException("Due Days Field is Required");

        /* header freight (CmbTransporterAc / txtFreightAmount are visible only with the FreightOutwardAc configuration) */
        List<Map<String, Object>> accounts = own.transporterAccounts(u, subsidiary, freightDr);
        String valueKey = subsidiary ? "SupplierCustomerId" : "Id";
        double freightText = outwardFreightAc > 0 ? awayZero(d(req.get("FreightAmount")), amtDec) : 0.0;
        int transporterValue = outwardFreightAc > 0 ? i(req.get("TransporterId")) : 0;
        Map<String, Object> transporterRow = transporterValue > 0
                ? accounts.stream().filter(x -> i(col(x, valueKey)) == transporterValue).findFirst().orElse(null) : null;
        if (d(req.get("FreightAmount")) > 0.0 && outwardFreightAc > 0 && transporterRow == null) throw new IllegalArgumentException("Transporter Account field Required");

        /* ------------------------------------------------ grids' own checks before the header is built */
        for (var r : glGrid) {          /* resolved later; here only the desktop's order of messages */
        }
        int jl = 0;
        for (var r : glGrid) {
            jl++;
            if ((d(r.get("Credit")) > 0.0 || i(r.get("Debit")) > 0) && i(r.get("AccountId")) == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
            if (customerGl == i(r.get("AccountId"))) throw new IllegalArgumentException("Customer Account Not select In Party Add Less Grid row#" + jl);
        }
        for (var r : expGrid)
            if (d(r.get("Amount")) > 0.0 && i(r.get("ItemId")) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");

        /* ------------------------------------------------ the grid rows (btnAdd / btnUpdateDetail arithmetic + Insert :3664 checks) */
        List<Map<String, Object>> savedLines = existing == null ? List.of() : rows(existing.get("details"));
        Map<Integer, Map<String, Object>> savedById = new HashMap<>();
        for (var x : savedLines) savedById.put(i(col(x, "Id")), x);
        List<Map<String, Object>> warehouses = own.warehouses(u, branchParam);
        Map<Integer, Map<String, Object>> whById = new HashMap<>();
        for (var x : warehouses) whById.put(i(col(x, "Id")), x);
        List<Map<String, Object>> lotItems = jobOrderWise ? own.jobLotsWithItems(u) : List.of();
        List<Map<String, Object>> jobLots = jobOrderWise ? distinctLots(lotItems) : own.jobLots(u, branchParam);
        Map<Integer, Map<String, Object>> lotById = new HashMap<>();
        for (var x : jobLots) lotById.put(i(col(x, "Id")), x);
        Set<Integer> packIds = ids(own.packingTypes()), cityIds = ids(own.cities(u));
        List<Map<String, Object>> cropYears = own.cropYears(u);
        Map<Integer, String> cropById = new HashMap<>();
        for (var x : cropYears) cropById.put(i(col(x, "Id")), s(col(x, "CropYear")));
        Map<Integer, Map<String, Object>> allItems = new HashMap<>();
        for (var x : own.items(u)) allItems.put(i(col(x, "Id")), x);
        Map<Integer, Set<Integer>> whItems = new HashMap<>();
        Set<Integer> brandIds = ids(own.brands(u));
        Map<Integer, List<Map<String, Object>>> uomCache = new HashMap<>();

        List<Row> rowsOut = new ArrayList<>();
        int rowNo = 0;
        for (var g : grid) {
            rowNo++;
            Row w = new Row();
            w.id = i(g.get("Id"));
            if (w.id > 0 && (id == 0 || !savedById.containsKey(w.id))) throw new IllegalArgumentException("Detail row " + rowNo + " is not part of this invoice");
            Map<String, Object> saved = w.id > 0 ? savedById.get(w.id) : null;
            boolean gdnRow = saved != null && i(col(saved, "InvGdnId")) > 0;
            boolean dirty = saved == null || b(g.get("Dirty"));
            if (gdnRow) { w.invGdnId = i(col(saved, "InvGdnId")); w.invGdnDetailId = i(col(saved, "InvGdnDetailId")); }
            String at = " (Detail row " + rowNo + ")";
            if (saved != null && (!dirty || gdnRow)) {
                /* untouched cells of a loaded row (ReadById :4204): Math.Round on the two totals, banker's */
                w.itemId = i(col(saved, "ItemId")); w.itemName = s(col(saved, "ItemName")); w.brandId = i(col(saved, "BrandItemId"));
                w.cropYear = s(col(saved, "CropYear")); w.jobLotId = i(col(saved, "JobLotId")); w.jobLotName = s(col(saved, "JobLotDescription"));
                w.packingTypeId = i(col(saved, "PackingTypeId")); w.packUomId = i(col(saved, "ItemUOMId")); w.packEq = d(col(saved, "PackEquivalent"));
                w.qty = d(col(saved, "ItemQty")); w.gross = d(col(saved, "GrossWeight")); w.ebUnit = d(col(saved, "EBWeight"));
                w.ebTotal = Math.rint(d(col(saved, "EBTotalWt"))); w.wtCut = d(col(saved, "WeightCut")); w.wtCutTotal = Math.rint(d(col(saved, "WeightCutTotal")));
                w.addLss = d(col(saved, "AdLsWeight")); w.net = d(col(saved, "NetBillWeight")); w.stock = d(col(saved, "NetStockWeight"));
                w.warehouseId = i(col(saved, "WarehouseId"));
                w.branchId = i(col(saved, "BranchId"));
            }
            if (saved != null && !dirty) {
                w.rate = d(col(saved, "ItemRate")); w.rateUomId = i(col(saved, "UomScheduleIdRate")); w.rateEq = d(col(saved, "RateUOM"));
                w.rateCut = d(col(saved, "RateCut")); w.rateCutTotal = d(col(saved, "RateCutAmount")); w.itemAmount = d(col(saved, "ItemAmount"));
                w.cityId = i(col(saved, "CityId")); w.reserveWh = i(col(saved, "ReserveWareHouse"));
                w.gpDate = dt(col(saved, "GpDate")); w.gpNo = i(col(saved, "GpNo")); w.vehicleNo = s(col(saved, "VehicleNo"));
                w.stored = true;
            } else {
                if (!gdnRow) {
                    w.itemId = i(g.get("ItemId"));
                    Map<String, Object> it = allItems.get(w.itemId);
                    if (itemsAgainstWh) {
                        int whPick = i(g.get("WarehouseId"));
                        Set<Integer> set = whItems.computeIfAbsent(whPick, k -> ids(own.itemsByWarehouse(u, k)));
                        if (!set.contains(w.itemId)) it = null;
                    }
                    if (it == null) throw new IllegalArgumentException("Item Name Field is Required" + at);
                    w.itemName = s(col(it, "ItemName"));
                    w.brandId = i(g.get("BrandItemId"));
                    if (w.brandId != 0 && !brandIds.contains(w.brandId)) w.brandId = 0;
                    int cropId = i(g.get("CropYearId"));
                    String cropText = s(g.get("CropYear"));
                    if (cropId != 0 && cropById.containsKey(cropId)) cropText = cropById.get(cropId);
                    else if (cropYears.stream().noneMatch(x -> s(col(x, "CropYear")).equals(s(g.get("CropYear"))))) cropText = "";
                    if (cropText.isEmpty()) throw new IllegalArgumentException("Crop Year Field is Required" + at);
                    w.cropYear = cropText;
                    w.jobLotId = i(g.get("JobLotId"));
                    if (w.jobLotId == 0 || !lotById.containsKey(w.jobLotId)) throw new IllegalArgumentException("Job/Lot Field is Required" + at);
                    w.jobLotName = s(col(lotById.get(w.jobLotId), "JobLotDescription"));
                    itemInJobLot(jobOrderWise, lotItems, w.itemId, w.itemName, w.jobLotId, w.jobLotName, 0);
                    w.packingTypeId = i(g.get("PackingTypeId"));
                    if (w.packingTypeId == 0 || !packIds.contains(w.packingTypeId)) throw new IllegalArgumentException("Packing Type Field is Required" + at);
                    w.warehouseId = i(g.get("WarehouseId"));
                    var packRows = uomCache.computeIfAbsent(w.itemId, k -> own.uoms(u, k));
                    w.packUomId = i(g.get("PackUOMId"));
                    var pu = packRows.stream().filter(x -> i(col(x, "Id")) == w.packUomId).findFirst().orElse(null);
                    if (w.qty == 0.0 && d(g.get("ItemQty")) == 0.0) throw new IllegalArgumentException("Qty Field is Required" + at);
                    if (pu == null) throw new IllegalArgumentException("Pack Unit Field is Required" + at);
                    w.packEq = d(col(pu, "Equivalent"));
                    w.qty = d(g.get("ItemQty"));
                    w.gross = d(g.get("GrossWeight"));
                    /* Total() :5382 - the empty-bag total follows the unit (typed unit) or the unit follows the total (typed total) */
                    w.ebTotal = d(g.get("EmptyBagsTotal"));
                    if ("unit".equals(s(g.get("EbMode")))) w.ebTotal = d(g.get("EmptyBags")) * w.qty;
                    w.ebUnit = w.qty != 0.0 ? w.ebTotal / w.qty : 0.0;
                    if (Double.isNaN(w.ebUnit) || Double.isInfinite(w.ebUnit)) w.ebUnit = 0.0;
                    w.wtCut = d(g.get("WeightCut"));
                    w.wtCutTotal = s(g.get("WeightCut")).trim().isEmpty() ? 0.0 : w.qty * w.wtCut;
                    w.addLss = d(g.get("AddLss"));
                    double weight = w.gross - w.ebTotal - w.wtCutTotal + w.addLss;
                    w.net = Math.rint(weight * 100.0) / 100.0;
                    w.stock = w.net;
                    if (w.gross <= 0.0 && false) w.gross = 0.0;
                }
                /* rate side: Rate, Rate UOM and Rate Cut stay editable on a GDN row */
                var packRows2 = uomCache.computeIfAbsent(w.itemId, k -> own.uoms(u, k));
                w.rateUomId = i(g.get("RateUOMId"));
                var ru = packRows2.stream().filter(x -> i(col(x, "Id")) == w.rateUomId).findFirst().orElse(null);
                if (ru == null) throw new IllegalArgumentException("Rate UOM Field is Required" + at);
                w.rateEq = d(col(ru, "Equivalent"));
                w.rate = d(g.get("Rate"));
                w.rateCut = d(g.get("RateCut"));
                /* AmountCaluculation() :5496 */
                if (w.net > 0.0 && w.rateEq > 0.0 && w.rate > 0.0) {
                    double amount = awayZero(w.net / w.rateEq * w.rate, amtDec);
                    w.rateCutTotal = awayZero(w.net / w.rateEq * w.rateCut, amtDec);
                    w.itemAmount = awayZero(amount - w.rateCutTotal, amtDec);
                } else { w.itemAmount = 0.0; w.rateCutTotal = 0.0; }
                if (w.itemAmount == 0.0 && !(w.rate <= 0.0)) throw new IllegalArgumentException("Please Check Item Amount" + at);
                w.cityId = i(g.get("CityId"));
                w.reserveWh = i(g.get("ReserveWarehouseId"));
                w.gpNo = i(g.get("GpNo")); w.vehicleNo = s(g.get("VehicleNo"));
                LocalDateTime gp = dt(g.get("GpDate"));
                w.gpDate = gp != null ? gp : LocalDate.now().atStartOfDay();
                if (!gdnRow) {
                    if (w.gross == 0.0) throw new IllegalArgumentException("Gross Weight Field is Required" + at);
                    if (w.net == 0.0) throw new IllegalArgumentException("Net Bill Weight Field is Required" + at);
                }
                if (w.reserveWh == 0 || !whById.containsKey(w.reserveWh)) throw new IllegalArgumentException("Reserve Warehouse Field is Required" + at);
                if (!gdnRow && (w.warehouseId == 0 || !whById.containsKey(w.warehouseId))) throw new IllegalArgumentException("Warehouse Field is Required" + at);
                if (w.cityId == 0 || !cityIds.contains(w.cityId)) throw new IllegalArgumentException("city Field is Required" + at);
                if (!gdnRow) w.branchId = i(col(whById.get(w.warehouseId), "BranchId"));
            }
            rowsOut.add(w);
        }

        /* ------------------------------------------------ Insert :3664 - messages in the desktop's order */
        double glDebitSum = 0.0, glCreditSum = 0.0;
        /* freight grid: GlAccountId looked up by the displayed title (SupCustIdUpdateforFrieghtGrid :3042) */
        double freightDebit = 0.0, freightCredit = 0.0;
        for (var r : frGrid) {
            freightCredit += d(r.get("Freight"));
            freightDebit += d(r.get("Debit"));
            if (d(r.get("Freight")) > 0.0 && i(r.get("Transporter")) == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
        }
        if (freightDr && freightCredit != freightDebit) throw new IllegalArgumentException("Freight Debit Amount not equal to Credit Amount Please Check");

        /* TotalCommissionAmount() :5541 */
        String commType = s(req.get("CommissionType")).trim();
        String commRateText = s(req.get("CommRate")).trim();
        double commRate = d(commRateText);
        if ("Flat".equals(commType) && !commRateText.isEmpty()) commRate = awayZero(commRate, 0);
        String uomText = s(req.get("UomScheduleIdCmRate")).trim();
        double totalItem = 0.0, totalNet = 0.0, totalQty = 0.0;
        for (Row w : rowsOut) { totalItem += w.itemAmount; totalNet += w.net; totalQty += w.qty; }
        double commAmount = 0.0;
        if (!commRateText.isEmpty()) {
            if ("Flat".equals(commType)) commAmount = awayZero(commRate, amtDec);
            else if ("Percent".equals(commType) || "Percentage".equals(commType)) commAmount = awayZero(totalItem * commRate / 100.0, amtDec);
            else if ("Comm Weight".equals(commType)) commAmount = awayZero(totalNet / d(uomText) * commRate, amtDec);
            if (Double.isNaN(commAmount) || Double.isInfinite(commAmount)) commAmount = 0.0;
        }
        int agentId = i(req.get("CommissionAgentId"));
        if (agentId != 0 && customers.stream().noneMatch(x -> i(col(x, "Id")) == i(req.get("CommissionAgentId")))) agentId = 0;
        List<Map<String, Object>> commDebitAccounts = commDr ? own.commissionDebitAccounts(u, subsidiary) : List.of();
        int commDebitValue = commDr ? i(req.get("CommissionDebitAccountId")) : 0;
        Map<String, Object> commDebitRow = commDebitValue > 0 ? commDebitAccounts.stream().filter(x -> i(col(x, "GlAccountId")) == i(req.get("CommissionDebitAccountId"))).findFirst().orElse(null) : null;
        if (commAmount > 0.0) {
            if (agentId == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");
            if (commDr && (commDebitRow == null || commDebitValue == 0)) throw new IllegalArgumentException("Please Select Commission Debit Account First");
        } else if (commAmount == 0.0 && commRate > 0.0) {
            throw new IllegalArgumentException("Commission Amount is Required when Comm Rate is greater then 0");
        }

        /* ExpProportion / CommissionProportion / FreightProportion / BillProportion / FcyAmount */
        double totalExp = 0.0;
        List<Map<String, Object>> expRows = new ArrayList<>();
        for (var e : expGrid) {
            double qty = d(e.get("Qty")), rate = d(e.get("Rate")), amount = d(e.get("Amount"));
            if (qty > 0.0 && rate > 0.0) amount = awayZero(qty * rate, amtDec);
            else amount = awayZero(amount, amtDec);
            e.put("_amount", amount);
            totalExp += amount;
        }
        double freightGridTotal = 0.0;
        for (var r : frGrid) freightGridTotal += d(r.get("Freight"));
        double rateD = exchangeRate.doubleValue();
        for (Row w : rowsOut) {
            w.expense = totalNet == 0.0 ? 0.0 : awayZero(totalExp / totalNet * w.net, amtDec);
            if ("Percent".equals(commType)) w.commission = w.itemAmount * commRate / 100.0;
            else w.commission = totalNet == 0.0 ? 0.0 : commAmount / totalNet * w.net;
            if (!freightDr) w.freight = freightGridTotal > 0.0 && totalNet != 0.0 ? awayZero(freightGridTotal / totalNet * w.net, amtDec) : 0.0;
            else if (w.id > 0 && savedById.containsKey(w.id)) w.freight = d(col(savedById.get(w.id), "FreightAmount"));
            double item = awayZero(w.itemAmount, amtDec), com = awayZero(w.commission, amtDec), exp = awayZero(w.expense, amtDec);
            if (!cfgCredit.isEmpty()) w.billAmount = truthy(cfgCredit) ? awayZero(item - com, amtDec) : item;
            else w.billAmount = awayZero(item + exp - com, amtDec);
            w.fcy = rateD > 0.0 ? BigDecimal.valueOf(w.itemAmount / rateD).setScale(4, RoundingMode.HALF_UP) : BigDecimal.ZERO;
        }

        /* resolve the hidden GlAccountId of the freight / party add-less cells from the displayed title */
        List<Freight> freights = new ArrayList<>();
        for (var r : frGrid) {
            int t = i(r.get("Transporter"));
            if (t == 0) continue;
            Freight f = new Freight();
            String saveGdn = s(r.get("InvGdnId"));
            f.InvGdnId = existing != null && rows(existing.get("freights")).stream().anyMatch(x -> s(col(x, "InvGdnId")).equals(saveGdn) && i(col(x, "InvGdnId")) > 0) ? i(saveGdn) : 0;
            f.TansporterId = glByTitle(accounts, titleOf(accounts, valueKey, t), 0);
            f.TransporterSupCustId = subsidiary ? t : 0;
            f.FreightAmount = i(r.get("Freight"));
            f.Debit = i(r.get("Debit"));
            f.Remarks = s(r.get("Remarks"));
            if (f.Remarks.isEmpty()) throw new IllegalArgumentException("Remarks Field Requried in Freight Grid");
            freights.add(f);
        }
        List<Journal> journals = new ArrayList<>();
        for (var r : glGrid) {
            int a = i(r.get("AccountId"));
            if (a != 0) { glDebitSum += d(r.get("Debit")); glCreditSum += d(r.get("Credit")); }
        }
        for (var r : glGrid) {
            int a = i(r.get("AccountId"));
            if (a == 0) continue;
            Journal j = new Journal();
            j.ChartofAccountId = glByTitle(accounts, titleOf(accounts, valueKey, a), 0);
            j.TransporterSupCustId = subsidiary ? a : 0;
            j.JvRemarks = s(r.get("Remarks"));
            j.JvPrcnt = d(r.get("Percentage")); j.JvQty = d(r.get("Qty")); j.JvRate = d(r.get("Rate"));
            j.JvDebit = d(r.get("Debit")); j.JvCredit = d(r.get("Credit"));
            if ((j.JvDebit > 0.0 || j.JvCredit > 0.0) && j.JvRemarks.isEmpty()) throw new IllegalArgumentException("Remarks Field is Required in Party AddLess");
            journals.add(j);
        }

        /* BillAmount() :5195 */
        double itemSum = awayZero(totalItem, amtDec), expSum = awayZero(totalExp, amtDec);
        double jd = awayZero(glDebitSum, amtDec), jc = awayZero(glCreditSum, amtDec);
        double bill = itemSum + expSum - awayZero(freightText, amtDec);
        bill = bill + jc - jd;
        if (customerId == agentId) bill -= commAmount;
        double billRounded = awayZero(bill, amtDec);
        BigDecimal fcyAmount = rateD == 0.0 ? BigDecimal.ZERO : BigDecimal.valueOf(bill / rateD).setScale(4, RoundingMode.HALF_UP);
        if (multiCurrency && fcyAmount.signum() == 0) throw new IllegalArgumentException("Fcy Amount Rate Field is Required");

        /* ------------------------------------------------ detail rows */
        List<Detail> details = new ArrayList<>();
        double grossTotal = 0.0;
        int line = 0;
        for (Row w : rowsOut) {
            line++;
            String where = " in row # " + line;
            if (w.itemId <= 0) throw new IllegalArgumentException("Item filed is required" + where);
            if (w.cropYear.isEmpty()) throw new IllegalArgumentException("Crop Year filed is required" + where);
            if (w.jobLotId <= 0) throw new IllegalArgumentException("JobLot filed is required" + where);
            itemInJobLot(jobOrderWise, lotItems, w.itemId, w.itemName, w.jobLotId, w.jobLotName, 0);
            if (w.gross <= 0.0) throw new IllegalArgumentException("Gross weight filed is required" + where);
            if (w.net <= 0.0) throw new IllegalArgumentException("Bill weight filed is required" + where);
            if (w.stock <= 0.0) throw new IllegalArgumentException("Stock weight filed is required" + where);
            if (w.rate <= 0.0) throw new IllegalArgumentException("ItemRate filed is required" + where);
            if (w.rateUomId <= 0) throw new IllegalArgumentException("RateUom filed is required" + where);
            double itemAmount = awayZero(w.itemAmount, amtDec);
            if (itemAmount <= 0.0) throw new IllegalArgumentException("ItemAmount filed is required" + where);
            if (w.warehouseId <= 0) throw new IllegalArgumentException("Warehouse filed is required" + where);
            grossTotal += w.gross;
            Detail v = new Detail();
            v.LineId = line;
            v.Id = w.id;
            v.InvGdnDetailId = w.invGdnDetailId; v.InvGdnId = w.invGdnId;
            v.ItemId = w.itemId; v.ItemName = w.itemName; v.BrandItemId = w.brandId; v.CropYear = w.cropYear;
            v.JobLotId = w.jobLotId; v.JobLotDescription = w.jobLotName; v.PackingTypeId = w.packingTypeId; v.ItemUOMId = w.packUomId;
            v.ItemQty = w.qty; v.GrossWeight = w.gross; v.EBWeight = w.ebUnit; v.EBTotalWt = w.ebTotal;
            v.WeightCut = (float) w.wtCut; v.WeightCutTotal = (float) w.wtCutTotal; v.AdLsWeight = w.addLss;
            v.NetBillWeight = w.net; v.NetStockWeight = w.stock;
            v.ItemRate = w.rate; v.ItemRateWOExp = BigDecimal.valueOf(w.rate); v.UomScheduleIdRate = w.rateUomId;
            v.RateCut = w.rateCut; v.RateCutAmount = w.rateCutTotal;
            v.ItemAmount = itemAmount;
            v.ExchangeRate = exchangeRate; v.CurrencyId = currencyId; v.FcyAmount = w.fcy;
            v.RateUOM = w.rateEq;
            v.WarehouseId = w.warehouseId;
            Map<String, Object> wh = whById.get(w.warehouseId);
            v.WareHouseName = wh == null ? "" : s(col(wh, "WareHouseName"));
            v.GpDate = w.gpDate; v.GpNo = w.gpNo; v.VehicleNo = w.vehicleNo;
            v.BillAmount = awayZero(w.billAmount, amtDec);
            v.ExpenseAmount = w.expense; v.JournalAmount = w.journal; v.CommissionAmount = w.commission; v.FreightAmount = w.freight;
            v.CityId = w.cityId; v.BranchId = w.branchId;
            v.ReserveWareHouse = w.reserveWh;
            details.add(v);
        }
        List<Expense> expenses = new ArrayList<>();
        List<Map<String, Object>> otherItems = repo.otherItems(u);
        for (var e : expGrid) {
            int itemId = i(e.get("ItemId"));
            if (itemId == 0) continue;
            Expense pe = new Expense();
            pe.InvRevExpItemId = itemId;
            pe.Qty = i(e.get("Qty"));
            pe.Rate = d(e.get("Rate"));
            pe.Amount = d(e.get("_amount"));
            pe.CustomRemarks = s(e.get("Remarks"));
            pe.Remarks = s(e.get("Remarks"));
            if (pe.Qty > 0.0 && pe.Rate > 0.0) {
                String nm = otherItems.stream().filter(x -> i(col(x, "Id")) == itemId).map(x -> s(col(x, "OtherItemName"))).findFirst().orElse("");
                pe.Remarks = "ItemName : " + nm.trim() + "  Qty: " + fmtNum(pe.Qty) + " Rate: " + fmtNum(pe.Rate) + " " + pe.Remarks;
            }
            expenses.add(pe);
        }

        /* ------------------------------------------------ header */
        LocalDateTime now = LocalDateTime.now();
        LocalDateTime docDate = dt(req.get("DocDate"));
        if (docDate == null) throw new IllegalArgumentException("Doc Date is required");
        int dueDays = ci(dueDaysText);
        LocalDateTime dueDate = dueDaysText.isEmpty() ? docDate : docDate.plusDays(dueDays);
        Head h = new Head();
        h.Id = id;
        h.AutoUpdate = autoUpdate ? 1 : 0;
        h.BranchesId = n(u.getBranchesId());
        h.BranchSrNo = id > 0 ? i(col(existing, "BranchSrNo")) : own.nextBranchSrNo(u, year);
        h.CommAmount = commAmount;
        h.CommissionAgentId = agentId;
        h.CommissionRemarks = s(req.get("CommissionRemarks")).trim();
        h.CommissionType = commType;
        h.CommRate = commRate;
        h.UomScheduleIdCmRate = uomText;
        h.PaymentTermId = i(req.get("PaymentTermId"));
        h.DocDate = docDate;
        h.DueDate = dueDate;
        h.DueDays = dueDays;
        h.DeliveryTerm = s(req.get("DeliveryTerm"));
        h.DocNo = docNo;
        h.DocumentTypeId = DOC;
        h.ManualBillNo = s(req.get("ManualBillNo")).trim();
        h.ProjectsId = projectValue;
        h.ReferencePartyId = 0;
        h.OtherCategoryId = categoryId;
        h.RemarksHeader = s(req.get("RemarksHeader")).trim();
        h.OtherRemarks = h.RemarksHeader;
        h.StockPartyId = 0;
        h.SupplierCustomerId = customerId;
        if (!subsidiary) { h.TransporterId = transporterValue; h.TransporterCreditPartyId = 0; }
        else if (transporterValue > 0 && transporterRow != null) { h.TransporterId = i(col(transporterRow, "Id")); h.TransporterCreditPartyId = i(col(transporterRow, "SupplierCustomerId")); }
        h.FreightAmount = freightText;
        LocalDateTime stockDate = dt(req.get("StockDate"));
        h.SupplierInvoiceDate = stockDate != null ? stockDate : docDate;
        h.SupplierInvoiceNo = dueDays;
        h.SupplierReferenceNo = s(req.get("SupplierReferenceNo")).trim();
        h.BillAmount = billRounded;
        h.EntryDate = now;
        h.ModifyDate = now;
        h.EntryUser = u.getId();
        h.ModifyUser = u.getId();
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.ScreenName = SCREEN;
        h.FinancialYearId = year;
        h.IsReserve = true;                                   /* FlagForm == 1 */
        h.ExchangeRate = exchangeRate;
        h.CurrencyId = currencyId;
        h.InvoiceQty = BigDecimal.valueOf(totalQty).setScale(2, RoundingMode.HALF_EVEN);
        h.InvoiceWeight = BigDecimal.valueOf(totalNet).setScale(2, RoundingMode.HALF_EVEN);
        h.FcyAmount = fcyAmount;
        h.CustomAccounts = b(req.get("CustomAccounts"));
        h.AttachmentsValues = id > 0 ? nullable(col(existing, "AttachmentsValues")) : "";
        h.CustomAttachmentsValues = id > 0 ? nullable(col(existing, "CustomAttachmentsValues")) : "";

        List<Commission> commissions = new ArrayList<>();
        if (agentId > 0 && commAmount > 0.0) {
            Commission c = new Commission();
            c.Id = 0; c.SaleOrderId = 0; c.CommDebitSupCustId = 0;
            c.CommissionAgentId = agentId;
            if ("Percent".equals(commType)) c.CommType = 2.0;
            if ("Flat".equals(commType)) c.CommType = 1.0;
            if ("Comm Weight".equals(commType)) c.CommType = 3.0;
            c.Rate = commRate;
            c.RateUom = d(uomText);
            c.CommAmount = commAmount;
            c.Remarks = h.CommissionRemarks;
            if (commDebitRow != null && commDebitValue > 0) {
                c.DebitAccountId = i(col(commDebitRow, "GlAccountId"));
                c.CommDebitSupCustId = i(col(commDebitRow, "SupplierCustomerId"));
            }
            commissions.add(c);
        }

        SaleInvoiceFinancialDirect.Invoice inv = new SaleInvoiceFinancialDirect.Invoice();
        inv.h = h;
        inv.details.addAll(details);
        inv.expenses.addAll(expenses);
        inv.freights.addAll(freights);
        inv.journals.addAll(journals);
        inv.commissions.addAll(commissions);

        /* BLL 0580 Save */
        if (h.Id == 0) h.ModifyUser = 0; else h.EntryUser = 0;
        SaleInvoiceFinancialDirect.Voucher voucher = new SaleInvoiceFinancialDirect(repo).makeVoucherForSaleInvoice(inv);
        int saved = persist.persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("voucherHeadId", repo.voucherHeadId(h.OrganizationId, h.CompanyId, DOC, saved));
        out.put("openWages", !autoUpdate && truthy(repo.config(u, "WagesCompulsoryOnSaleInvoiceDirect")) && own.wagesActive());
        out.put("grossWeightTotal", grossTotal);
        out.put("message", (id == 0 ? "Record Saved Successfully [" : "Record Update Successfully [") + h.DocNo + "] ");
        return out;
    }

    // ================================================================================= helpers

    /** ItemIdExistsInJobLot :2458. */
    private static void itemInJobLot(boolean jobOrderWise, List<Map<String, Object>> lotItems, int itemId, String itemName, int jobLotId, String jobLotName, int rowNo) {
        if (!jobOrderWise) return;
        for (var r : lotItems) if (i(col(r, "Id")) == jobLotId && i(col(r, "ItemId")) == itemId) return;
        throw new IllegalArgumentException(rowNo == 0
                ? "The selected item '" + itemName + "' does not exist against the job lot '" + jobLotName + "'."
                : "The selected item '" + itemName + "' does not exist against the job lot '" + jobLotName + "' in detail grid row no: " + rowNo + ".");
    }

    private static String titleOf(List<Map<String, Object>> list, String key, int value) {
        for (var r : list) if (i(col(r, key)) == value) return s(col(r, "AccountTitle"));
        return "";
    }
    /** dtAccountlst.Select("AccountTitle='x'")[0].ItemArray[0]. */
    private static int glByTitle(List<Map<String, Object>> list, String title, int previous) {
        if (title.isEmpty() || title.contains("'")) return previous;
        for (var r : list) if (title.equals(s(col(r, "AccountTitle")))) return i(col(r, "Id"));
        return previous;
    }

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
        return v == Math.rint(v) && Math.abs(v) < 1e15 ? String.valueOf((long) v) : BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }
}
