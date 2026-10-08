package com.mst.services.sale.engr;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.repositories.support.DesktopProc;
import com.mst.services.SaleInvoiceEngrPersist;
import com.mst.services.SaleInvoiceFinancialDirect;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.web.server.ResponseStatusException;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;
import java.util.concurrent.ConcurrentHashMap;

import static com.mst.services.sale.engr.SaleEngrSupport.*;

/**
 * Screen 537 "frmSaleInvoiceReturn_Engr" = Architecture.WinApp.SaleTrading.frmSaleInvoiceReturn_Engr (Sale Engr, module 83, document type 1611).
 *
 * Desktop map (sirT.cs = frmSaleInvoiceReturn_Engr.cs, 3399 lines; method : line):
 *   ctor :216, Load (InvfrmPurchaseInvoice_Load) :241, BranchFill :347, ProjectFill :363, PreviousSalesTaxAc :379, TaxAccountFill :398, AccountsFill :412,
 *   DocumentNo :431, VoucherHeadIdGet :454, PaymentTerms :465, DeliveryTerm :482, AllComboBind :500, CityFill :545, BindPackUomAndRateUom :565,
 *   GetallTaxType :620, CmbItem_Leave :638, FormValidation :657, FormValidationDetila :703, grdFreight* :767-868, grdGLedger* :869-995,
 *   btnCancelUpdateDetial :996, grdSettings :1011, grd_CellUpdated :1144, grd_ColumnButtonClick :1231, btnAdd_Click :1251, grd_DoubleClick :1272,
 *   btnUpdateDetail_Click :1312, grd_UpdatingCell :1358, BillAmount :1366, Reset :1405, btnFrmRefresh_Click :1474, ResetDetail :1486,
 *   FreightProportion :1513, LedgerProportion :1570, BillProportion :1627, ReadById :1644, Insert :1731, GetAll (history) :1985,
 *   HistoryGridSettings :2080, grdHistory_* :2138-2224, btnshow_Click :2225, BindDetailOfHeaderId :2236, prints :2351-2445, btnDelete_Click :2449 (EMPTY),
 *   txtDueDays_TextChanged :2455, DocDate_Leave :2473, DueDate_ValueChanged :2529, CmbPaymentTerm_TextChanged :2545, CalculateWeight :2687,
 *   CalculateNetWeightAgainstAddLess :2710, CalculateAmount :2730, CalculateTaxAmount :2757, KeyDown :2951, MakeShortCutKeys :3086, grd*_KeyDown :3121-3322.
 * BLL 0580 InvSaleInvoice.Save (Id 0: ModifyUser 0, Sp_InvSaleInvoice_Insert; else EntryUser 0, Sp_InvSaleInvoice_Update) + DAL 0433 SetData
 * (SaleInvoiceEngrPersist, document type 1611) + SaleInvoiceFinancialDirect.makeVoucherForSaleInvoice, one transaction.
 *
 * The page posts its rows to {@link #calc}; Save runs the same code on rows whose source (the stored invoice line) is re-read from the
 * database, so the saved numbers never depend on what the browser computed. A row carries a sticky "calc" flag:
 * "entry" (added/updated from the entry panel), "amt" (ItemQty / AdLsWt / RateCut cell edited), "tax" (city / warehouse cell edited),
 * "taxc" (TaxTypeId / Tax% cell edited), "dl" (DocDate_Leave tax refresh), "" (untouched: the source values are kept).
 */
@Service
public class SaleInvoiceReturnEngrService {
    public static final String SCREEN = "frmSaleInvoiceReturn_Engr";
    public static final int DOC_TYPE = 1611;

    private final SaleEngrSupport sup;
    private final SaleEngrAttachments attachments;
    private final SaleInvoiceRepository repo;
    private final SaleInvoiceEngrPersist persist;

    public SaleInvoiceReturnEngrService(SaleEngrSupport sup, SaleEngrAttachments attachments, SaleInvoiceRepository repo, SaleInvoiceEngrPersist persist) {
        this.sup = sup; this.attachments = attachments; this.repo = repo; this.persist = persist;
    }

    // ------------------------------------------------------------------ configuration (cached for a few seconds per company)

    private final Map<String, Object[]> cache = new ConcurrentHashMap<>();

    @SuppressWarnings("unchecked")
    private <T> T cached(String key, java.util.function.Supplier<T> s) {
        String k = sup.org() + "|" + sup.company() + "|" + key;
        Object[] e = cache.get(k);
        long now = System.currentTimeMillis();
        if (e != null && (long) e[0] > now) return (T) e[1];
        T v = s.get();
        cache.put(k, new Object[]{now + 15000L, v});
        if (cache.size() > 500) cache.clear();
        return v;
    }

    private static final class Cfg {
        boolean taxEditable;
        int amtRound, amtPlaces, ratePlaces;
    }

    private Cfg cfg() {
        return cached("cfg", () -> {
            Cfg c = new Cfg();
            c.taxEditable = sup.configBool("TaxPercentEditable");                          // TaxPercntEditableOrNot
            int amt = toInt(sup.config("Default NoofDecimal Points For Amount"));
            int rate = toInt(sup.config("Default NoofDecimal Points For Rate"));
            c.amtRound = Math.max(0, Math.min(10, amt));
            c.amtPlaces = amt >= 1 && amt <= 4 ? amt : 0;                                  // CommonServices.GetDecimalConfiguration
            c.ratePlaces = rate >= 1 && rate <= 4 ? rate : (rate == 0 ? 2 : 0);
            return c;
        });
    }

    // ------------------------------------------------------------------ Load

    public Map<String, Object> initial() {
        Map<String, Object> m = new LinkedHashMap<>(lists(LocalDate.now().toString()));
        m.put("rights", sup.rights(SCREEN));
        m.put("nextNo", nextNo());                                         // DocumentNo
        m.put("lastTaxAcId", lastTaxAcId());                               // PreviousSalesTaxAc
        m.put("branchId", sup.branch());
        return m;
    }

    /** btnFrmRefresh_Click: BranchFill, ProjectFill, AllComboBind, AccountsFill, GetallTaxType. */
    public Map<String, Object> refresh(String taxDate) {
        cache.clear();
        Map<String, Object> m = new LinkedHashMap<>(lists(taxDate));
        m.remove("terms"); m.remove("deliveryTerms"); m.remove("cities");
        return m;
    }

    private Map<String, Object> lists(String taxDate) {
        Cfg c = cfg();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branches", branches());
        m.put("projects", projects());
        Map<String, List<Map<String, Object>>> ev = evaluationLists();
        m.put("customers", ev.get("GetSupplierCustomer"));
        m.put("items", ev.get("GetItems"));
        m.put("jobLots", ev.get("GetJobLot"));
        m.put("warehouses", ev.get("GetWarehouse"));
        m.put("terms", terms());
        m.put("deliveryTerms", List.of(row("Id", 1, "DeliveryTerm", "Load"), row("Id", 2, "DeliveryTerm", "Ponch")));
        m.put("accounts", accountTitles("3,6,8,16,17,18,19"));
        m.put("taxTypes", taxTypes(taxDate));
        m.put("cities", cities());
        m.put("taxEditable", c.taxEditable);
        m.put("fmt", row("amountRound", c.amtRound, "amount", c.amtPlaces, "rate", c.ratePlaces));
        return m;
    }

    /** CommonServices.BrancheServiceBind -> Branches.GetAll (Sp_Branches_GetAllMethod 'GetAll'). */
    public List<Map<String, Object>> branches() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_Branches_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetAll"))
            out.add(row("Id", toInt(ci(r, "Id")), "BranchName", str(ci(r, "BranchName"))));
        return out;
    }

    /** CommonServices.ProjectServiceBind -> Projects.GetAlldt (Sp_Projects_GetAllMethod MethodType 'GetAll'). */
    public List<Map<String, Object>> projects() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_Projects_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"))
            out.add(row("Id", toInt(ci(r, "Id")), "ProjectName", str(ci(r, "ProjectName"))));
        return out;
    }

    /**
     * AllComboBind: InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales (USP_GetDataFromInventoryStocksEvaluationsForSales,
     * DocType 'Sale', no Activity), split by its Activity column into customers, items, job lots and warehouses.
     */
    private Map<String, List<Map<String, Object>>> evaluationLists() {
        UserAccount u = sup.user();
        Map<String, List<Map<String, Object>>> out = new HashMap<>();
        for (String a : List.of("GetSupplierCustomer", "GetItems", "GetJobLot", "GetWarehouse")) out.put(a, new ArrayList<>());
        for (Map<String, Object> r : sup.rows("USP_GetDataFromInventoryStocksEvaluationsForSales", "OrganizationId", sup.org(), "CompanyId", sup.company(),
                "UserId", u.getId(), "AppId", sup.ctx().currentAppId(), "DocType", "Sale")) {
            List<Map<String, Object>> l = out.get(str(ci(r, "Activity")));
            if (l != null) l.add(row("Id", toInt(ci(r, "Id")), "RefName", str(ci(r, "RefName"))));
        }
        return out;
    }

    /** CommonServices.GetDueTermServiceBind. */
    public List<Map<String, Object>> terms() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.paymentTerms(sup.user())) out.add(row("Id", toInt(ci(r, "Id")), "TermsDescription", str(ci(r, "TermsDescription"))));
        return out;
    }

    /** AccountsFill: CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8,16,17,18,19"). */
    private List<Map<String, Object>> accountTitles(String ids) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.accountTitlesByTypes(sup.user(), ids, null)) out.add(row("Id", toInt(ci(r, "Id")), "AccountTitle", str(ci(r, "AccountTitle"))));
        return out;
    }

    /** CityFill: City.GetAll -> SP_City_GetAllMethod @MethodType 'GetAll'. */
    public List<Map<String, Object>> cities() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("SP_City_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "MethodType", "GetAll"))
            out.add(row("Id", toInt(ci(r, "Id")), "CityName", str(ci(r, "CityName"))));
        return out;
    }

    /** CommonServices.GetUomScheduleByItemId -> Sp_UOMSchedule_GetAllMethod 'ReadByItemID'. */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sup.rows("Sp_UOMSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "ItemId", itemId, "Activity", "ReadByItemID"))
            out.add(row("Id", toInt(ci(r, "Id")), "UOMCode", str(ci(r, "UOMCode")), "Equivalent", toDouble(ci(r, "Equivalent"))));
        return out;
    }

    /** GetallTaxType: ItemTaxSchedule.GetAllTaxSchedule(org, company, DocDate, 2). */
    public List<Map<String, Object>> taxTypes(String date) {
        LocalDate d = parseDate(date);
        if (d == null) d = LocalDate.now();
        final LocalDate fd = d;
        return cached("tax|" + fd, () -> {
            List<Map<String, Object>> out = new ArrayList<>();
            for (Map<String, Object> r : sup.rows("Sp_ItemTaxSchedule_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "EffectedDate", fd,
                    "TaxTypeId", 2, "Activity", "GetAllTaxSchedule"))
                out.add(row("TaxNameId", toInt(ci(r, "TaxNameId")), "TaxName", str(ci(r, "TaxName")), "TaxPercent", toDouble(ci(r, "TaxPercent"))));
            return out;
        });
    }

    private Map<Integer, Double> taxMap(String date) {
        Map<Integer, Double> m = new HashMap<>();
        for (Map<String, Object> t : taxTypes(date)) m.put(toInt(t.get("TaxNameId")), toDouble(t.get("TaxPercent")));
        return m;
    }

    /** CommonServices.GetTaxScheduleDetailbyItemIds (DocDate_Leave): Sp_ItemTaxSchedule_GetAllMethod 'GetTaxScheduleDetailbyItemIds'. */
    public List<Map<String, Object>> taxByItems(String itemIds, String date) {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", sup.org()); p.put("CompanyId", sup.company());
        p.put("ItemIds", itemIds == null ? "" : itemIds);
        p.put("EffectedDate", parseDate(date));
        p.put("Activity", "GetTaxScheduleDetailbyItemIds");
        return DesktopProc.rows(sup.jdbc(), "Sp_ItemTaxSchedule_GetAllMethod", p);
    }

    /** DocumentNo: InvSaleInvoice.GenerateInvSaleInvoiceCode (document type 1611). */
    public int nextNo() { return repo.nextDocNo(sup.user(), sup.fy(), DOC_TYPE); }

    /** PreviousSalesTaxAc: InvSaleInvoice.GetLastSaleTaxAcId. */
    public int lastTaxAcId() {
        List<Map<String, Object>> r = sup.rows("Sp_InvSaleInvoice_GetAllMethod", "OrganizationId", sup.org(), "CompanyId", sup.company(), "Activity", "GetLastSaleTaxAcId");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "SaleTaxAcId"));
    }

    // ------------------------------------------------------------------ History (GetAll, BindDetailOfHeaderId)

    /** GetAll: InvSaleInvoice.FormHistory (Sp_InvSaleInvoice_GetAllMethod 'FormHistory'). The desktop sends the dates only; doc no / customer boxes are not read. */
    public List<Map<String, Object>> history(String dateType, String fromDate, String toDate) {
        boolean all = Boolean.TRUE.equals(sup.rights(SCREEN).get("viewAll"));
        UserAccount u = sup.user();
        LocalDate f = parseDate(fromDate), t = parseDate(toDate);
        String fromKey, toKey;
        switch (dateType == null ? "" : dateType) {
            case "entry": fromKey = "EntryFromDate"; toKey = "EntryToDate"; break;
            case "modify": fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; break;
            case "approved": fromKey = "ApprovedFromDate"; toKey = "ApprovedToDate"; break;
            default: fromKey = "FromDate"; toKey = "ToDate"; break;
        }
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId()); p.put("CompanyId", u.getCompanyId());
        p.put("DocumentTypeId", DOC_TYPE); p.put("CanViewAllRecord", all);
        if (sup.fy() != 0) p.put("FinancialYearId", sup.fy());
        if (f != null) p.put(fromKey, f.atStartOfDay());
        if (t != null) p.put(toKey, t.atStartOfDay());
        if (!all) p.put("EntryUser", u.getId());
        p.put("Activity", "FormHistory");
        return DesktopProc.rows(sup.jdbc(), "Sp_InvSaleInvoice_GetAllMethod", p);
    }

    // ------------------------------------------------------------------ ReadById (InvSaleInvoice.GetByID)

    private Map<String, Object> invoice(int id) {
        Map<String, Object> inv = repo.readById(id);
        UserAccount u = sup.user();
        if (inv == null || (inv.containsKey("OrganizationId") && toInt(ci(inv, "OrganizationId")) != u.getOrganizationId())
                || (inv.containsKey("CompanyId") && toInt(ci(inv, "CompanyId")) != u.getCompanyId())
                || toInt(ci(inv, "DocumentTypeId")) != DOC_TYPE)
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "Sale invoice return not found in this company");
        return inv;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> children(Map<String, Object> inv, String key) {
        Object o = inv.get(key);
        return o instanceof List ? (List<Map<String, Object>>) o : List.of();
    }

    /** ReadById's dtGrid.Rows.Add(...) from an InvSaleInvoiceDetail row. */
    private Map<String, Object> storedLine(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", toInt(ci(d, "Id"))); m.put("OrderId", toInt(ci(d, "SaleOrderId"))); m.put("OrderDetailId", toInt(ci(d, "SaleOrderDetailId")));
        m.put("OrderNo", toInt(ci(d, "SaleOrder"))); m.put("WarehouseId", toInt(ci(d, "WarehouseId")));
        m.put("ItemId", toInt(ci(d, "ItemId"))); m.put("ItemName", str(ci(d, "ItemName")));
        m.put("JobLotId", toInt(ci(d, "JobLotId"))); m.put("JobLot", str(ci(d, "JobLotDescription")));
        m.put("PackUomId", toInt(ci(d, "ItemUOMId"))); m.put("PackUom", str(ci(d, "UOMCodeItem"))); m.put("PackEquivalent", toDouble(ci(d, "PackEquivalent")));
        m.put("ItemQty", toDouble(ci(d, "ItemQty"))); m.put("GrossWeight", toDouble(ci(d, "GrossWeight"))); m.put("AdLsWt", toDouble(ci(d, "AdLsWeight")));
        m.put("NetBillWeight", toDouble(ci(d, "NetBillWeight")));
        m.put("ItemRate", toDouble(ci(d, "ItemRate"))); m.put("RateUomId", toInt(ci(d, "UomScheduleIdRate"))); m.put("RateUom", str(ci(d, "UOMCodeRate")));
        m.put("RateEquivalent", toDouble(ci(d, "RateUOM")));
        m.put("RateCut", toDouble(ci(d, "RateCut"))); m.put("RateCutTotal", toDouble(ci(d, "RateCutAmount")));
        m.put("ItemAmount", toDouble(ci(d, "ItemAmount")));
        m.put("CityId", toInt(ci(d, "CityId"))); m.put("CityName", str(ci(d, "CityName")));
        m.put("GpDate", ci(d, "GpDate")); m.put("GpNo", toInt(ci(d, "GpNo"))); m.put("VehicleNo", str(ci(d, "VehicleNo")));
        m.put("TaxTypeId", toInt(ci(d, "TaxNameId"))); m.put("TaxPct", toDouble(ci(d, "TaxPercent"))); m.put("TaxAmount", toDouble(ci(d, "TaxAmount")));
        m.put("Freights", toDouble(ci(d, "FreightAmount"))); m.put("Journal", toDouble(ci(d, "JournalAmount"))); m.put("BillAmount", toDouble(ci(d, "BillAmount")));
        return m;
    }

    public Map<String, Object> record(int id) {
        Map<String, Object> inv = invoice(id);
        Map<String, Object> head = new LinkedHashMap<>();
        for (String k : List.of("DocNo", "DocDate", "BranchesId", "ProjectsId", "SupplierCustomerId", "SupplierReferenceNo", "ManualBillNo", "TaxAccountId", "RemarksHeader",
                "DeliveryTerm", "PaymentTermId", "DueDays", "DueDate", "BillAmount", "IsApproved"))
            head.put(k, ci(inv, k));
        head.put("Id", id);
        List<Map<String, Object>> lines = new ArrayList<>();
        for (Map<String, Object> d : children(inv, "details")) lines.add(storedLine(d));
        List<Map<String, Object>> jl = new ArrayList<>();
        for (Map<String, Object> j : children(inv, "journals"))
            jl.add(row("AccountId", toInt(ci(j, "ChartofAccountId")), "Remarks", str(ci(j, "JvRemarks")), "Prcnt", toDouble(ci(j, "JvPrcnt")), "Qty", toDouble(ci(j, "JvQty")),
                    "Rate", toDouble(ci(j, "JvRate")), "Debit", toDouble(ci(j, "JvDebit")), "Credit", toDouble(ci(j, "JvCredit"))));
        List<Map<String, Object>> fr = new ArrayList<>();
        for (Map<String, Object> f : children(inv, "freights"))
            fr.add(row("InvGrnId", toInt(ci(f, "InvGdnId")), "Transporter", toInt(ci(f, "TansporterId")), "Freight", toDouble(ci(f, "FreightAmount")), "Debit", toDouble(ci(f, "Debit"))));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("head", head);
        out.put("lines", lines);
        out.put("journals", jl);
        out.put("freights", fr);
        out.put("voucherHeadId", repo.voucherHeadId(sup.org(), sup.company(), DOC_TYPE, id));
        out.put("attachments", attachments.list(SCREEN, id));
        return out;
    }

    /** BindDetailOfHeaderId: the rows of the highlighted history invoice. */
    public List<Map<String, Object>> historyDetail(int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : children(invoice(id), "details"))
            out.add(row("OrderNo", ci(d, "SaleOrder"), "Warehouse", ci(d, "WareHouseName"), "ItemName", ci(d, "ItemName"), "JobLot", ci(d, "JobLotDescription"),
                    "PackUom", ci(d, "UOMCodeItem"), "ItemQty", toDouble(ci(d, "ItemQty")), "GrossWeight", toDouble(ci(d, "GrossWeight")), "AdLsWt", toDouble(ci(d, "AdLsWeight")),
                    "NetBillWeight", toDouble(ci(d, "NetBillWeight")), "ItemRate", toDouble(ci(d, "ItemRate")), "RateUOM", ci(d, "UOMCodeRate"),
                    "RateCut", toDouble(ci(d, "RateCut")), "RateCutAmount", toDouble(ci(d, "RateCutAmount")), "ItemAmount", toDouble(ci(d, "ItemAmount")),
                    "CityName", ci(d, "CityName"), "GpDate", ci(d, "GpDate"), "GpNo", ci(d, "GpNo"), "VehicleNo", ci(d, "VehicleNo"),
                    "TaxName", ci(d, "TaxName"), "TaxPct", toDouble(ci(d, "TaxPercent")), "TaxAmount", toDouble(ci(d, "TaxAmount"))));
        return out;
    }

    // ------------------------------------------------------------------ the request (common to calc and save)

    public static class Req {
        public int id;
        public String docNo;
        public String docDate;
        public int branchId;
        public int projectId;
        public int customerId;
        public String deliveryTerm;
        public int paymentTermId;
        public String dueDays;
        public String dueDate;
        public int taxAccountId;
        public String manualBillNo;
        public String refNo;
        public String remarks;
        public List<Map<String, Object>> lines = new ArrayList<>();
        public List<Map<String, Object>> freights = new ArrayList<>();
        public List<Map<String, Object>> journals = new ArrayList<>();
        public SaleEngrAttachments.Change attachments;
    }

    private static final class Totals {
        double bill, item, tax, net;
    }

    private static double d(Map<String, Object> m, String k) { return toDouble(m.get(k)); }

    /** Math.Round(v, dec, MidpointRounding.AwayFromZero) and ToString("#,##0.00") rounding. */
    private static double round(double v, int dec) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0.0;
        return BigDecimal.valueOf(v).setScale(Math.max(0, dec), RoundingMode.HALF_UP).doubleValue();
    }

    /** Math.Round(v, 2) (banker's, exact binary value). */
    private static double bankers2(double v) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0.0;
        return new BigDecimal(v).setScale(2, RoundingMode.HALF_EVEN).doubleValue();
    }

    /** Convert.ToInt32(double): banker's rounding. */
    private static int convInt(double v) { return Double.isNaN(v) || Double.isInfinite(v) ? 0 : (int) Math.rint(v); }

    /** A double cell that came out as Infinity / NaN reads back as 0 (Conversion.ToDouble). */
    private static double fin(double v) { return Double.isNaN(v) || Double.isInfinite(v) ? 0.0 : v; }

    private static LocalDate parseDate(Object o) {
        if (o == null) return null;
        String s = String.valueOf(o).trim();
        if (s.length() < 10) return null;
        try { return LocalDate.parse(s.substring(0, 10)); } catch (RuntimeException e) { return null; }
    }

    private static List<Map<String, Object>> copy(List<Map<String, Object>> in) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (in != null) for (Map<String, Object> m : in) out.add(m == null ? new LinkedHashMap<>() : new LinkedHashMap<>(m));
        return out;
    }

    private static boolean has(Collection<Map<String, Object>> rows, String key, int id) {
        for (Map<String, Object> m : rows) if (toInt(ci(m, key)) == id) return true;
        return false;
    }

    // ------------------------------------------------------------------ the grid arithmetic

    /** grd_CellUpdated, ItemQty / RateCut / AdLsWt: every figure stays unrounded. */
    private void cellAmount(Map<String, Object> m) {
        double weight = d(m, "ItemQty") * d(m, "PackEquivalent");
        double net = weight + d(m, "AdLsWt");
        double eq = d(m, "RateEquivalent");
        double rateCutAmount = fin(net / eq * d(m, "RateCut"));
        double amount = fin(net / eq * d(m, "ItemRate"));
        m.put("GrossWeight", weight);
        m.put("NetBillWeight", net);
        m.put("RateCutTotal", rateCutAmount);
        m.put("ItemAmount", amount - rateCutAmount);
    }

    /** The always-run tax block of grd_CellUpdated (and the TaxTypeId / Tax% branches when the tax cell was edited). */
    private void cellTax(Cfg c, Map<String, Object> m, Map<Integer, Double> tax, boolean taxCell) {
        int type = toInt(m.get("TaxTypeId"));
        double pct = d(m, "TaxPct");
        if (taxCell) {
            if (type == 0) pct = 0.0;
            else if (!(pct > 0.0) && tax.containsKey(type)) pct = tax.get(type);
        }
        double ia = d(m, "ItemAmount");
        if (pct > 100.0) pct = 100.0;                                      // "Tax % Can't be greater than 100..."
        if (ia > 0.0 && pct > 0.0 && type > 0) { m.put("TaxPct", pct); m.put("TaxAmount", round(ia * pct / 100.0, c.amtRound)); }
        else { m.put("TaxPct", 0.0); m.put("TaxAmount", 0.0); }
    }

    /** DocDate_Leave: the percent of the type scheduled for the new date; TaxAmount = ItemAmount * percent / 100 (unrounded) when both are positive. */
    private void docDateTax(Map<String, Object> m) {
        int type = toInt(m.get("TaxTypeId"));
        double pct = d(m, "TaxPct"), ia = d(m, "ItemAmount");
        if (type == 0) { m.put("TaxPct", 0.0); m.put("TaxAmount", 0.0); return; }
        if (ia > 0.0 && pct > 0.0) m.put("TaxAmount", ia * pct / 100.0);
    }

    /** CalculateWeight / CalculateNetWeightAgainstAddLess / CalculateAmount / CalculateTaxAmount of the entry panel, as the row is added with btnAdd. */
    private void entryCalc(Cfg c, Map<String, Object> m, Map<Integer, Double> tax) {
        double gross = d(m, "GrossWeight"), addLess = d(m, "AdLsWt");
        double net = round(addLess != 0.0 ? gross + addLess : gross, 3);               // txtNetBillWeight.Text = ToString("#,##0.###")
        double rate = round(d(m, "ItemRate"), c.ratePlaces);                          // txtRate_Leave
        double cut = d(m, "RateCut"), eq = d(m, "RateEquivalent");
        double rateCutTotal = 0.0, amount = 0.0;
        if (net > 0.0 && rate > 0.0 && eq > 0.0) {
            double a = net / eq * rate;
            double rc = cut > 0.0 ? net / eq * cut : 0.0;
            a -= rc;
            rateCutTotal = round(rc, c.amtPlaces);
            amount = round(a, c.amtPlaces);
        }
        m.put("NetBillWeight", net); m.put("ItemRate", rate); m.put("RateCutTotal", rateCutTotal); m.put("ItemAmount", amount);
        int type = toInt(m.get("TaxTypeId"));
        if (type > 0 && tax.containsKey(type)) {
            double pct = c.taxEditable && d(m, "TaxPct") > 0.0 ? d(m, "TaxPct") : tax.get(type);
            if (pct > 100.0) pct = 100.0;
            m.put("TaxPct", pct);
            m.put("TaxAmount", round(amount * pct / 100.0, c.amtPlaces));
        } else { m.put("TaxTypeId", 0); m.put("TaxPct", 0.0); m.put("TaxAmount", 0.0); }
    }

    /** Runs, in the order the grid leaves them: every row's cell formulas, FreightProportion, LedgerProportion, BillAmount (+ BillProportion). */
    private Totals compute(Cfg c, Req r, List<Map<String, Object>> lines, List<Map<String, Object>> freights, List<Map<String, Object>> journals, Map<Integer, Double> tax) {
        Totals t = new Totals();
        for (Map<String, Object> m : lines) {
            switch (str(m.get("calc"))) {
                case "entry" -> entryCalc(c, m, tax);
                case "amt" -> { cellAmount(m); cellTax(c, m, tax, toBool(m.get("tc"))); }
                case "tax" -> cellTax(c, m, tax, false);
                case "taxc" -> cellTax(c, m, tax, true);
                case "dl" -> docDateTax(m);
                default -> { }
            }
            t.net += d(m, "NetBillWeight"); t.item += d(m, "ItemAmount"); t.tax += d(m, "TaxAmount");
        }
        // FreightProportion: only "Freight" (credit) beyond "Debit" is spread, by net weight
        double fCr = 0.0, fDr = 0.0;
        for (Map<String, Object> f : freights) { fCr += d(f, "Freight"); fDr += d(f, "Debit"); }
        for (Map<String, Object> m : lines)
            m.put("Freights", fDr < fCr ? fin(Math.rint(fCr - fDr) / t.net * d(m, "NetBillWeight")) : 0.0);
        // LedgerProportion: every GL row counts here, whether it has an account or not
        double jCr = 0.0, jDr = 0.0;
        for (Map<String, Object> j : journals) { jCr += d(j, "Credit"); jDr += d(j, "Debit"); }
        for (Map<String, Object> m : lines)
            m.put("Journal", jDr < jCr ? fin(Math.rint(jCr - jDr) / t.net * d(m, "NetBillWeight")) : 0.0);
        // BillAmount: items + tax + (credit - debit) of the GL rows that have an account; txtSupplierGLId stays empty, so no transporter credit
        double glCr = 0.0, glDr = 0.0;
        for (Map<String, Object> j : journals) if (toInt(j.get("AccountId")) > 0) { glDr += d(j, "Debit"); glCr += d(j, "Credit"); }
        double bill = t.item + t.tax;
        double diff = glCr - glDr;
        bill = diff < 0.0 ? bill - Math.abs(diff) : bill + diff;
        t.bill = round(bill, c.amtPlaces);                                           // txtBillAmount.Text = ToString(stringFormatsingle)
        // BillProportion
        for (Map<String, Object> m : lines) m.put("BillAmount", d(m, "ItemAmount") + d(m, "TaxAmount") + d(m, "Freights"));
        return t;
    }

    /** The page's preview: the same numbers Insert() would carry, for the rows as they stand. */
    public Map<String, Object> calc(Req r) {
        Cfg c = cfg();
        List<Map<String, Object>> lines = new ArrayList<>(), freights = copy(r.freights), journals = copy(r.journals);
        Map<Integer, Double> tax = taxMap(r.docDate);
        Map<String, Object> ctx = lookups(c, r, tax);
        lines = resolve(c, r, tax, ctx, false);
        Totals t = compute(c, r, lines, freights, journals, tax);
        return row("lines", lines, "billAmount", t.bill, "itemAmount", t.item, "taxAmount", t.tax, "netWeight", t.net);
    }

    // ------------------------------------------------------------------ row sources

    @SuppressWarnings("unchecked")
    private Map<String, Object> lookups(Cfg c, Req r, Map<Integer, Double> tax) {
        Map<String, Object> ctx = new HashMap<>();
        Set<Integer> wh = new HashSet<>(), city = new HashSet<>(), job = new HashSet<>(), item = new HashSet<>();
        Map<String, List<Map<String, Object>>> ev = evaluationLists();
        for (Map<String, Object> x : ev.get("GetWarehouse")) wh.add(toInt(x.get("Id")));
        for (Map<String, Object> x : cities()) city.add(toInt(x.get("Id")));
        for (Map<String, Object> x : ev.get("GetJobLot")) job.add(toInt(x.get("Id")));
        for (Map<String, Object> x : ev.get("GetItems")) item.add(toInt(x.get("Id")));
        ctx.put("wh", wh); ctx.put("city", city); ctx.put("job", job); ctx.put("item", item);
        ctx.put("stored", new HashMap<Integer, Map<String, Object>>());
        if (r.id > 0) {
            Map<Integer, Map<String, Object>> st = (Map<Integer, Map<String, Object>>) ctx.get("stored");
            for (Map<String, Object> d : children(invoice(r.id), "details")) st.put(toInt(ci(d, "Id")), storedLine(d));
        }
        return ctx;
    }

    /** Every row's source is the database (the stored invoice line) or the entry panel's choices checked against the lists. */
    @SuppressWarnings("unchecked")
    private List<Map<String, Object>> resolve(Cfg c, Req r, Map<Integer, Double> tax, Map<String, Object> ctx, boolean strict) {
        Set<Integer> whIds = (Set<Integer>) ctx.get("wh"), cityIds = (Set<Integer>) ctx.get("city"), jobIds = (Set<Integer>) ctx.get("job"), itemIds = (Set<Integer>) ctx.get("item");
        Map<Integer, Map<String, Object>> stored = (Map<Integer, Map<String, Object>>) ctx.get("stored");
        Map<Integer, List<Map<String, Object>>> uomCache = new HashMap<>();
        List<Map<String, Object>> lines = copy(r.lines);
        for (Map<String, Object> m : lines) {
            int id = toInt(m.get("Id"));
            Map<String, Object> mine = new LinkedHashMap<>(m);
            Map<String, Object> src = null;
            if (id > 0) {
                src = stored.get(id);
                if (src == null) throw new Warning("Sale invoice detail record not found");
            }
            boolean entryUpdate = id > 0 && str(mine.get("calc")).equals("entry");          // a stored row re-written from the entry panel
            if (src != null && !entryUpdate) mergeSource(c, m, new LinkedHashMap<>(src), mine, whIds, cityIds, tax);
            else mergeEntry(c, m, id, whIds, cityIds, jobIds, itemIds, tax, uomCache);
        }
        return lines;
    }

    /**
     * Row whose source is a stored invoice line: identity columns, rate, UOMs and (when the tax cells are not editable) the tax come from the source;
     * the cells the grid lets the user edit (ItemQty, AdLsWt, RateCut, CityName, WarehouseId, Tax cells) come from the page.
     */
    private void mergeSource(Cfg c, Map<String, Object> m, Map<String, Object> src, Map<String, Object> mine, Set<Integer> whIds, Set<Integer> cityIds, Map<Integer, Double> tax) {
        String calc = str(mine.get("calc"));
        m.clear();
        m.putAll(src);
        m.put("calc", calc);
        m.put("tc", toBool(mine.get("tc")));
        int wh = toInt(mine.get("WarehouseId"));
        m.put("WarehouseId", whIds.contains(wh) ? wh : toInt(src.get("WarehouseId")));
        int city = toInt(mine.get("CityId"));
        m.put("CityId", cityIds.contains(city) ? city : toInt(src.get("CityId")));
        if (calc.isEmpty()) return;
        if (!calc.equals("dl")) {                                          // ItemQty, AdLsWt and RateCut are the cells the grid lets the user edit
            m.put("ItemQty", d(mine, "ItemQty")); m.put("AdLsWt", d(mine, "AdLsWt")); m.put("RateCut", d(mine, "RateCut"));
        }
        int tt = toInt(mine.get("TaxTypeId"));
        double tp = d(mine, "TaxPct");
        if (c.taxEditable) {
            m.put("TaxTypeId", tax.containsKey(tt) ? tt : 0);
            m.put("TaxPct", tp);
        } else if (tt != toInt(src.get("TaxTypeId")) && tax.containsKey(tt) && Math.abs(tp - tax.get(tt)) < 1e-9) {
            m.put("TaxTypeId", tt);                                       // the DocDate_Leave refresh: a type with its own scheduled percent
            m.put("TaxPct", tp);
        } else if (tt == 0 && toInt(src.get("TaxTypeId")) != 0 && calc.equals("dl")) {
            m.put("TaxTypeId", 0); m.put("TaxPct", 0.0);
        }
    }

    /** Row added from the entry panel: nothing but the user's choices; every id is checked against the lists the combos hold. */
    private void mergeEntry(Cfg c, Map<String, Object> m, int keepId, Set<Integer> whIds, Set<Integer> cityIds, Set<Integer> jobIds, Set<Integer> itemIds,
                            Map<Integer, Double> tax, Map<Integer, List<Map<String, Object>>> uomCache) {
        m.put("Id", keepId); m.put("OrderId", 0); m.put("OrderDetailId", 0); m.put("OrderNo", 0);
        if (str(m.get("calc")).isEmpty()) m.put("calc", "entry");
        int wh = toInt(m.get("WarehouseId"));
        m.put("WarehouseId", whIds.contains(wh) ? wh : 0);
        int item = toInt(m.get("ItemId"));
        if (!itemIds.contains(item)) item = 0;
        m.put("ItemId", item);
        int job = toInt(m.get("JobLotId"));
        m.put("JobLotId", jobIds.contains(job) ? job : 0);
        int city = toInt(m.get("CityId"));
        m.put("CityId", cityIds.contains(city) ? city : 0);
        int tt = toInt(m.get("TaxTypeId"));
        m.put("TaxTypeId", tax.containsKey(tt) ? tt : 0);
        List<Map<String, Object>> uoms = item == 0 ? List.of() : uomCache.computeIfAbsent(item, this::uoms);
        int pu = toInt(m.get("PackUomId")), ru = toInt(m.get("RateUomId"));
        Map<String, Object> pm = null, rm = null;
        for (Map<String, Object> x : uoms) { if (toInt(x.get("Id")) == pu) pm = x; if (toInt(x.get("Id")) == ru) rm = x; }
        m.put("PackUomId", pm == null ? 0 : pu); m.put("PackEquivalent", pm == null ? 0.0 : toDouble(pm.get("Equivalent"))); if (pm != null) m.put("PackUom", pm.get("UOMCode"));
        m.put("RateUomId", rm == null ? 0 : ru); m.put("RateEquivalent", rm == null ? 0.0 : toDouble(rm.get("Equivalent"))); if (rm != null) m.put("RateUom", rm.get("UOMCode"));
        m.put("GpNo", toInt(m.get("GpNo")));
        m.put("VehicleNo", str(m.get("VehicleNo")));
        if (parseDate(m.get("GpDate")) == null) m.put("GpDate", LocalDate.now().toString());
    }

    // ------------------------------------------------------------------ Save (Insert / Update)

    /** FormValidation(), word for word and in the desktop's order. */
    private void formValidation(Req r, List<Map<String, Object>> branches, List<Map<String, Object>> projects, List<Map<String, Object>> customers, List<Map<String, Object>> terms) {
        if (!has(branches, "Id", r.branchId)) throw new Warning("Branch Name is Required");
        if (!has(projects, "Id", r.projectId)) throw new Warning("Project Name is Required");
        if (toInt(text(r.docNo)) == 0) throw new Warning("Doc No Name is Required");
        if (!has(customers, "Id", r.customerId)) throw new Warning("Customer Name is Required");
        if (!has(terms, "Id", r.paymentTermId)) throw new Warning("Payment Term is Required");
        if (r.paymentTermId == 2 && toInt(r.dueDays) == 0) throw new Warning("Due Days Field is Required");
        String dt = text(r.deliveryTerm);
        if (!dt.equals("Load") && !dt.equals("Ponch")) throw new Warning("Delivery Term is Required");
    }

    @Transactional
    public Map<String, Object> save(Req r) {
        UserAccount u = sup.user();
        Cfg c = cfg();
        Map<String, Boolean> rights = sup.rights(SCREEN);
        if (!Boolean.TRUE.equals(rights.get(r.id > 0 ? "update" : "save")))
            throw new Warning("You do not have the " + (r.id > 0 ? "Update" : "Save") + " right for this screen.");

        List<Map<String, Object>> branches = branches(), projects = projects(), terms = terms(), accounts = accountTitles("3,6,8,16,17,18,19");
        Map<String, List<Map<String, Object>>> ev = evaluationLists();
        formValidation(r, branches, projects, ev.get("GetSupplierCustomer"), terms);

        List<Map<String, Object>> freights = copy(r.freights), journals = copy(r.journals);
        for (Map<String, Object> f : freights)
            if (d(f, "Freight") > 0.0 && toInt(f.get("Transporter")) == 0) throw new Warning("Please Select an Account Against Freight First");
        for (Map<String, Object> j : journals)
            if ((d(j, "Credit") > 0.0 || toInt(j.get("Debit")) > 0) && toInt(j.get("AccountId")) == 0) throw new Warning("Please Select an Account Against JL First");

        Map<Integer, Double> tax = taxMap(r.docDate);
        Map<String, Object> stored = r.id > 0 ? invoice(r.id) : null;
        Map<String, Object> ctx = lookups(c, r, tax);
        List<Map<String, Object>> lines = resolve(c, r, tax, ctx, true);
        Totals t = compute(c, r, lines, freights, journals, tax);

        boolean taxAcOk = r.taxAccountId > 0 && has(accounts, "Id", r.taxAccountId);
        for (Map<String, Object> m : lines)
            if (d(m, "TaxAmount") > 0.0 && !taxAcOk) throw new Warning("Tax Account is Required");

        // ---- header
        SaleInvoiceModels.Head h = new SaleInvoiceModels.Head();
        LocalDateTime now = LocalDateTime.now();
        LocalDate docDate = parseDate(r.docDate);
        if (docDate == null) throw new Warning("Doc No Name is Required");
        h.Id = r.id;
        h.DocNo = r.id > 0 ? toInt(ci(stored, "DocNo")) : repo.nextDocNo(u, sup.fy(), DOC_TYPE);
        h.TaxAccountId = taxAcOk ? r.taxAccountId : 0;
        h.BranchesId = r.branchId;
        h.ProjectsId = r.projectId;
        h.DocumentTypeId = DOC_TYPE;
        h.SupplierInvoiceDate = now;
        h.SupplierInvoiceNo = h.DocNo;
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.EntryDate = now;
        h.ModifyDate = now;
        h.EntryUser = u.getId();
        h.ModifyUser = u.getId();
        h.FinancialYearId = sup.fy();
        h.DocDate = docDate.atStartOfDay();
        h.SupplierCustomerId = r.customerId;
        h.SupplierReferenceNo = text(r.refNo);
        h.ManualBillNo = text(r.manualBillNo);
        h.PaymentTermId = r.paymentTermId;
        int dueDays = toInt(r.dueDays);
        h.DueDays = dueDays;
        LocalDate dd = parseDate(r.dueDate);
        h.DueDate = (dd != null ? dd : docDate.plusDays(dueDays)).atStartOfDay();
        h.DeliveryTerm = text(r.deliveryTerm);
        h.BillAmount = t.bill;
        h.RemarksHeader = text(r.remarks);
        h.ScreenName = SCREEN;
        h.AttachmentsValues = r.id > 0 && stored != null && ci(stored, "AttachmentsValues") != null ? str(ci(stored, "AttachmentsValues")) : "";
        h.CustomAttachmentsValues = r.id > 0 && stored != null && ci(stored, "CustomAttachmentsValues") != null ? str(ci(stored, "CustomAttachmentsValues")) : "";

        if (lines.isEmpty()) throw new Warning("Grid Record Not Found");

        // ---- detail rows (per row checks, in the desktop's order)
        SaleInvoiceFinancialDirect.Invoice inv = new SaleInvoiceFinancialDirect.Invoice();
        inv.h = h;
        int rowNo = 0;
        for (Map<String, Object> m : lines) {
            rowNo++;
            String where = " In Row#" + rowNo + " In Detail Grid";
            SaleInvoiceModels.Detail pd = new SaleInvoiceModels.Detail();
            pd.Id = toInt(m.get("Id"));
            pd.SaleOrderId = toInt(m.get("OrderId"));
            pd.SaleOrderDetailId = toInt(m.get("OrderDetailId"));
            pd.WarehouseId = toInt(m.get("WarehouseId"));
            if (pd.WarehouseId == 0) throw new Warning("Warehouse Required" + where);
            pd.ItemId = toInt(m.get("ItemId"));
            if (pd.ItemId == 0) throw new Warning("Item Required" + where);
            pd.JobLotId = toInt(m.get("JobLotId"));
            if (pd.JobLotId == 0) throw new Warning("JobLot Required" + where);
            pd.ItemUOMId = toInt(m.get("PackUomId"));
            if (pd.ItemUOMId == 0) throw new Warning("Pack Uom Required" + where);
            pd.ItemQty = d(m, "ItemQty");
            if (pd.ItemQty == 0.0) throw new Warning("Item Qty Required" + where);
            pd.GrossWeight = d(m, "GrossWeight");
            pd.AdLsWeight = d(m, "AdLsWt");
            pd.NetBillWeight = convInt(d(m, "NetBillWeight"));            // Conversion.ToInt(NetBillWeight): the desktop saves the whole number
            if (pd.NetBillWeight == 0.0) throw new Warning("Net Weight Required" + where);
            pd.NetStockWeight = pd.NetBillWeight;
            pd.ItemRate = d(m, "ItemRate");
            if (pd.ItemRate == 0.0) throw new Warning("Item Rate Required" + where);
            pd.UomScheduleIdRate = toInt(m.get("RateUomId"));
            if (pd.UomScheduleIdRate == 0) throw new Warning("Rate Uom Required" + where);
            pd.RateCut = convInt(d(m, "RateCut"));                        // Conversion.ToInt(RateCut)
            pd.RateCutAmount = convInt(d(m, "RateCutTotal"));             // Conversion.ToInt(RateCutTotal)
            pd.ItemAmount = d(m, "ItemAmount");
            if (pd.ItemAmount == 0.0) throw new Warning("Item Amount Required" + where);
            pd.CityId = toInt(m.get("CityId"));
            LocalDate gp = parseDate(m.get("GpDate"));
            pd.GpDate = (gp == null ? LocalDate.of(1900, 1, 1) : gp).atStartOfDay();
            pd.GpNo = toInt(m.get("GpNo"));
            pd.VehicleNo = str(m.get("VehicleNo"));
            pd.TaxNameId = toInt(m.get("TaxTypeId"));
            pd.TaxPercent = d(m, "TaxPct");
            pd.TaxAmount = bankers2(d(m, "TaxAmount"));
            pd.FreightAmount = d(m, "Freights");
            pd.JournalAmount = d(m, "Journal");
            pd.BillAmount = d(m, "BillAmount");
            if (pd.BillAmount == 0.0) throw new Warning("Bill Amount Required" + where);
            pd.IsTaxable = pd.TaxNameId > 0 && pd.TaxPercent > 0.0 && pd.TaxAmount > 0.0 ? "true" : "false";
            pd.LineId = rowNo;
            inv.details.add(pd);
        }

        // ---- freight rows (Transporter chosen), ToInt on the amounts as the desktop does
        for (Map<String, Object> f : freights) {
            int tr = toInt(f.get("Transporter"));
            if (tr == 0) continue;
            if (!has(accounts, "Id", tr)) throw new Warning("Please Select an Account Against Freight First");
            SaleInvoiceModels.Freight pf = new SaleInvoiceModels.Freight();
            pf.InvGdnId = toInt(f.get("InvGrnId"));
            pf.TansporterId = tr;
            pf.FreightAmount = convInt(d(f, "Freight"));
            pf.Debit = convInt(d(f, "Debit"));
            inv.freights.add(pf);
        }
        // ---- journals (rows with an account)
        for (Map<String, Object> j : journals) {
            int acc = toInt(j.get("AccountId"));
            if (acc == 0) continue;
            if (!has(accounts, "Id", acc)) throw new Warning("Please Select an Account Against JL First");
            SaleInvoiceModels.Journal pj = new SaleInvoiceModels.Journal();
            pj.ChartofAccountId = acc;
            pj.JvRemarks = str(j.get("Remarks"));
            pj.JvPrcnt = d(j, "Prcnt"); pj.JvQty = d(j, "Qty"); pj.JvRate = d(j, "Rate"); pj.JvDebit = d(j, "Debit"); pj.JvCredit = d(j, "Credit");
            inv.journals.add(pj);
        }
        // the expense, commission and payment term lists stay empty on this screen

        // ---- BLL 0580 Save + DAL SetData (one transaction)
        if (h.Id == 0) h.ModifyUser = 0; else h.EntryUser = 0;
        SaleInvoiceFinancialDirect.Voucher voucher = new SaleInvoiceFinancialDirect(repo).makeVoucherForSaleInvoice(inv);
        int saved = persist.persist(inv, voucher, h.Id == 0 ? "Sp_InvSaleInvoice_Insert" : "Sp_InvSaleInvoice_Update");
        attachments.apply(SCREEN, DOC_TYPE, saved, r.customerId, r.attachments);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", saved);
        out.put("docNo", h.DocNo);
        out.put("voucherHeadId", repo.voucherHeadId(u.getOrganizationId(), u.getCompanyId(), DOC_TYPE, saved));
        out.put("message", (r.id == 0 ? "Record Save Successfully [" : "Record Update Successfully [") + h.DocNo + "]");
        return out;
    }

    // ------------------------------------------------------------------ attachments

    public List<Map<String, Object>> attachmentList(int id) { invoice(id); return attachments.list(SCREEN, id); }

    public SaleEngrAttachments.Download download(int id, int attachmentId) { invoice(id); return attachments.download(SCREEN, id, attachmentId); }
}
