package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.SaRpt1Repository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import java.time.LocalDate;
import java.util.*;
import org.springframework.stereotype.Service;

/**
 * Group R1 - Sales Reports 832 / 840 / 908 / 490 / 485 / 487 / 491 / 486 / 488 / 489. Tenancy (Organization / Company / User /
 * Branch / Financial Year) always comes from CurrentUserContext, rights are the desktop's ScreenId "View" right
 * (DesktopReportRights; 908's update asks for "Update"). Nothing here edits a shared service.
 */
@Service
public class SaRpt1Service {
    /** dbo.ScreenDefinition ids of the ten screens, keyed by the route segment. */
    public static final Map<String, Integer> SCREENS;
    static {
        Map<String, Integer> m = new LinkedHashMap<>();
        m.put("gdn-register", 832);
        m.put("sale-invoice-register", 840);
        m.put("sale-invoice-stock-rate-update", 908);
        m.put("orders-with-ledger-balance", 490);
        m.put("sale-invoice-history", 485);
        m.put("sale-invoice-direct-register", 487);
        m.put("sale-price-list-with-discount", 491);
        m.put("sale-invoice-return-register", 486);
        m.put("purchase-and-sale-detail-by-joblot", 488);
        m.put("delivery-order-history", 489);
        SCREENS = Collections.unmodifiableMap(m);
    }

    private final SaRpt1Repository repository;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;

    public SaRpt1Service(SaRpt1Repository repository, CurrentUserContext context, DesktopReportRights rights) {
        this.repository = repository;
        this.context = context;
        this.rights = rights;
    }

    private UserAccount user(String key) { return user(key, "View"); }

    private UserAccount user(String key, String action) {
        Integer screen = SCREENS.get(key);
        if (screen == null) throw new IllegalArgumentException("Unknown report " + key);
        UserAccount u = context.requireAccountingUser();
        rights.require(u, screen, action);
        return u;
    }

    private int appId(UserAccount u) {
        Integer a = u.getAppId();
        return a != null && a > 0 ? a : context.currentAppId();
    }

    // ------------------------------------------------------------------ lookups (the form's Load / Refresh)
    public Map<String, Object> lookups(String key) {
        UserAccount u = user(key);
        int fy = context.currentFinancialYearId();
        Map<String, Object> d;
        switch (key) {
            case "gdn-register": return repository.gdnLookups(u, fy);
            case "sale-invoice-register": return repository.saleInvoiceRegisterLookups(u, fy);
            case "orders-with-ledger-balance": return repository.ledgerBalanceLookups(u, fy);
            case "sale-price-list-with-discount": return repository.priceListLookups(u, fy);
            case "delivery-order-history": return repository.deliveryOrderCombos(u, fy);
            case "sale-invoice-history":
                d = repository.basics(u, fy);
                d.put("branches", repository.branchesOfSaleInvoice(u, 95));
                return d;
            case "sale-invoice-direct-register":
                d = repository.basics(u, fy);
                d.put("branches", repository.branchesOfSaleInvoice(u, 99));
                return d;
            case "sale-invoice-return-register":
                d = repository.basics(u, fy);
                d.put("branches", repository.branchesAll(u));
                return d;
            case "purchase-and-sale-detail-by-joblot":
                d = repository.basics(u, fy);
                d.put("branches", repository.branchesOfUser(u));
                return d;
            default: return repository.basics(u, fy);
        }
    }

    /** The branch dependent lists (ComboFill) of 485 / 487 / 486 / 488. */
    public Map<String, Object> combos(String key, String branchesIds) {
        UserAccount u = user(key);
        switch (key) {
            case "sale-invoice-history": return repository.saleInvoiceCombos(u, appId(u), null, branchesIds);
            case "sale-invoice-direct-register": return repository.saleInvoiceCombos(u, appId(u), "99,139,184", branchesIds);
            case "sale-invoice-return-register": return repository.returnRegisterCombos(u, branchesIds);
            case "purchase-and-sale-detail-by-joblot": return repository.jobLotCombos(u, branchesIds);
            default: throw new IllegalArgumentException(key + " has no branch dependent lists");
        }
    }

    /** 487 btnRefresh_Click: warehouses, job lots, items and suppliers re-bound from the all-records lists. */
    public Map<String, Object> directRegisterRefresh() {
        UserAccount u = user("sale-invoice-direct-register");
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("warehouses", repository.allWarehouses(u));
        d.put("jobLots", repository.allJobLots(u));
        d.put("items", repository.allItems(u));
        d.put("suppliers", repository.allSuppliers(u));
        return d;
    }

    // ------------------------------------------------------------------ report rows (the Show buttons)
    public List<Map<String, Object>> rows(String key, Map<String, String> q) {
        UserAccount u = user(key);
        int fy = context.currentFinancialYearId();
        switch (key) {
            case "gdn-register": return repository.gdnRegister(u, fy, "main".equals(q.get("mode")), q);
            case "sale-invoice-register": return repository.saleInvoiceRegister(u, fy, q);
            case "orders-with-ledger-balance": {
                int tab = SaRpt1Repository.i(q.get("tab"));
                int actionId = tab == 2 ? 2 : tab == 3 ? 3 : 1;
                boolean all = SaRpt1Repository.i(q.get("dateType")) == 6;
                return repository.ledgerBalance(u, fy, actionId, actionId == 3 ? 0 : SaRpt1Repository.i(q.get("supplierCustomerId")),
                        all ? null : SaRpt1Repository.day(q.get("fromDate")), all ? null : SaRpt1Repository.day(q.get("toDate")));
            }
            case "sale-invoice-history": return repository.saleInvoiceHistory(u, q);
            case "sale-invoice-direct-register": return repository.saleInvoiceDirect(u, q);
            case "sale-price-list-with-discount": return priceList(u, q);
            case "sale-invoice-return-register": return repository.saleReturnRegister(u, q);
            case "purchase-and-sale-detail-by-joblot": return repository.purchaseAndSaleByJobLot(u, q);
            case "delivery-order-history": return deliveryOrderRows(q);
            default: throw new IllegalArgumentException(key + " has no row source");
        }
    }

    /** 489: the procedure's Table0 (Table1, the company logo row, only feeds the Crystal template). */
    @SuppressWarnings("unchecked")
    public List<Map<String, Object>> deliveryOrderRows(Map<String, String> q) {
        UserAccount u = user("delivery-order-history");
        return (List<Map<String, Object>>) repository.deliveryOrders(u, q).get("rows");
    }

    // ------------------------------------------------------------------ 491 GeneratePriceForPercent
    private List<Map<String, Object>> priceList(UserAccount u, Map<String, String> q) {
        double factor = SaRpt1Repository.dbl(repository.config(u, "RoundingForItemPricing"));
        List<Map<String, Object>> src = repository.priceList(u, SaRpt1Repository.day(q.get("effectedDate")), SaRpt1Repository.i(q.get("itemCategoryId")));
        List<Map<String, Object>> out = new ArrayList<>(src.size());
        for (Map<String, Object> r : src) {
            double price = SaRpt1Repository.dbl(ci(r, "ItemPrice"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("CategoryDescription", ci(r, "CategoryDescription"));
            m.put("EffectedDate", ci(r, "EffectedDate"));
            m.put("ItemName", ci(r, "ItemName"));
            m.put("RateUom", ci(r, "RateUom"));
            m.put("ItemPrice", price);
            for (int p = 1; p <= 6; p++) m.put(p + "%Dicount", generatePriceForPercent(price, factor, p / 100.0));
            out.add(m);
        }
        return out;
    }

    private static Object ci(Map<String, Object> row, String key) {
        if (row.containsKey(key)) return row.get(key);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    /**
     * SalePriceListWithDiscount.GeneratePriceForPercent, Math.Round(x, 0) = banker's rounding (Math.rint). A RoundingForItemPricing of 0
     * (not configured) would divide by zero on the desktop (NaN); the discounted price is returned unrounded in that case.
     */
    static double generatePriceForPercent(double price, double roundingBy, double percent) {
        double discounted = price * (1.0 - percent);
        if (roundingBy == 0.0) return discounted;
        double roundingFactor = Math.rint(discounted / roundingBy) * roundingBy - discounted;
        double num = Math.rint((discounted + roundingFactor) / roundingBy) * roundingBy;
        double up = num + roundingBy, down = num;
        return Math.abs(discounted - up) <= Math.abs(discounted - down) ? up : down;
    }

    // ------------------------------------------------------------------ 908
    public Map<String, Object> cgsData(boolean sale, boolean settled, LocalDate from, LocalDate to) {
        UserAccount u = user("sale-invoice-stock-rate-update");
        return repository.cgsData(u, context.currentFinancialYearId(), sale, settled, from, to);
    }

    /**
     * btnInsertVoucherAndStock_Click: every checked invoice is validated first (a not-settled update needs every job order of the invoice
     * settled: "Some job order settlement is pending please check..."), then InvSaleInvoice.StockRateUpdateFromJobOrder runs
     * usp_StockRateUpdateFromJobOrder once per invoice with ActionId 1 for Settled, nothing for Un Settled.
     * The invoice list is re-read with the same filters so only invoices the screen would list can be updated.
     */
    @SuppressWarnings("unchecked")
    public String stockRateUpdate(boolean sale, boolean settled, LocalDate from, LocalDate to, List<Integer> ids) {
        UserAccount u = user("sale-invoice-stock-rate-update", "Update");
        if (ids == null || ids.isEmpty()) throw new IllegalArgumentException("Check the row first");
        Map<String, Object> data = repository.cgsData(u, context.currentFinancialYearId(), sale, settled, from, to);
        List<Map<String, Object>> main = (List<Map<String, Object>>) data.get("main");
        List<Map<String, Object>> detail = (List<Map<String, Object>>) data.get("detail");
        Map<Integer, Integer> docTypeOf = new LinkedHashMap<>();
        for (Map<String, Object> r : main) docTypeOf.put(SaRpt1Repository.i(ci(r, "Id")), SaRpt1Repository.i(ci(r, "DocumentTypeId")));
        String link = sale ? "InvSaleInvoiceId" : "ExportVoucherId";
        for (Integer id : ids) {
            if (!docTypeOf.containsKey(id)) throw new IllegalArgumentException("Invoice " + id + " is not in the current list, press Show again");
            if (!settled) {
                for (Map<String, Object> d : detail) {
                    if (SaRpt1Repository.i(ci(d, link)) == id && "UnSettled".equals(SaRpt1Repository.str(ci(d, "JobStatus")))) {
                        throw new IllegalStateException("Some job order settlement is pending please check...");
                    }
                }
            }
        }
        for (Integer id : ids) repository.stockRateUpdate(u, docTypeOf.get(id), id, settled ? 1 : 0);
        return "Stocks rate have been successfully updated.";
    }

    // ------------------------------------------------------------------ links
    public int voucherHeadId(String key, int id, int documentTypeId) {
        return repository.voucherHeadId(user(key), id, documentTypeId);
    }

    public int glAccountOfParty(String key, int supplierCustomerId) {
        return repository.glAccountIdOfParty(user(key), supplierCustomerId);
    }

    /** Company header for the Crystal @CompanyName / @CompanyAddress parameters is read by the print controller. */
    public UserAccount currentUser(String key) { return user(key); }
}
