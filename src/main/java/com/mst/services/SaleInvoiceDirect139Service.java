package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.saleinvoice.SaleInvoiceModels.*;
import com.mst.repositories.SaleInvoiceDirect139Repository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.*;

import static com.mst.repositories.SaleInvoiceDirect139Repository.DOC;
import static com.mst.repositories.SaleInvoiceDirect139Repository.SCREEN;
import static com.mst.repositories.SaleInvoiceRepository.*;

/**
 * Screen 145 "Sale Invoice Direct 139" - Architecture.WinApp.Sale.frmSaleInvoiceDirect139, DocumentTypeId 139.
 *
 *   Load :364  Insert :743  ReadById :1163  BillAmount :1026  CommissionProportion :1118  BillProportion :1100
 *   Total :1217  AmountCaluculation :1279  TotalCommissionAmount :1492  GetAll :1390
 *   Save = BLL 0580 InvSaleInvoice.Save -> SaleInvoiceFinancialDirect (0612/0613) -> SaleInvoiceDirectPersist (DAL 0433 SetData),
 *   one transaction. 139 is not in the num8 exclusion set, so with ERP feature 5 the stock goes through FIFO
 *   (USP_GetStockByFifoMethod), else UpdateInventoryReference.
 * The browser sends only what the user typed; amounts, net weights, commission and bill amount are recomputed here the
 * way the form computes them.
 */
@Service
public class SaleInvoiceDirect139Service {
    private final SaleInvoiceDirect139Repository own;
    private final SaleInvoiceRepository repo;
    private final SaleInvoiceService shared;
    private final SaleInvoiceDirectPersist persist;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public SaleInvoiceDirect139Service(SaleInvoiceDirect139Repository own, SaleInvoiceRepository repo, SaleInvoiceService shared,
                                       SaleInvoiceDirectPersist persist, StoreScreenRights rights, CurrentUserContext ctx) {
        this.own = own; this.repo = repo; this.shared = shared; this.persist = persist; this.rights = rights; this.ctx = ctx;
    }

    private UserAccount user() { return ctx.requireAccountingUser(); }
    private Map<String, Boolean> r() { return rights.of(SCREEN); }

    // ================================================================================= load

    /** InvfrmPurchasedirectInvoice_Load :364 - the lists the form binds, in its order. */
    public Map<String, Object> lookups() {
        UserAccount u = user();
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("rights", r());
        m.put("customers", own.customers(u));
        m.put("dueTerms", own.dueTerms(u));
        List<Map<String, Object>> delivery = new ArrayList<>();
        delivery.add(two(1, "Load")); delivery.add(two(2, "Ponch"));
        m.put("deliveryTerms", delivery);
        m.put("warehouses", own.warehouses(u));
        m.put("jobLots", own.jobLots(u));
        m.put("items", own.items(u));
        m.put("packingTypes", own.packingTypes());
        m.put("otherItems", own.otherItems(u));
        m.putAll(numbers());
        return m;
    }

    private static Map<String, Object> two(int id, String text) {
        Map<String, Object> x = new LinkedHashMap<>();
        x.put("Id", id); x.put("DeliveryTerm", text);
        return x;
    }

    /** DocumentNo() :623. */
    public Map<String, Object> numbers() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("docNo", repo.nextDocNo(user(), ctx.currentFinancialYearId(), DOC));
        return m;
    }

    /** PackUOM() :586. */
    public List<Map<String, Object>> uoms(int itemId) { return own.uomsOfItem(user(), itemId); }

    /** AvailableStockGetByItem() :663. */
    public Map<String, Object> stock(int itemId, String docDate, int packingTypeId, int stockUom) {
        LocalDateTime d = dt(docDate);
        double v = own.currentStock(user(), itemId, d == null ? LocalDateTime.now() : d, packingTypeId, stockUom);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("stock", v > 0.0 ? v : 0.0);
        return m;
    }

    public List<Map<String, Object>> history(int noOfRecords) {
        UserAccount u = user();
        return own.history(u, ctx.currentFinancialYearId(), r().get("viewAll"), noOfRecords);
    }

    /** ReadById :1163 - InvSaleInvoice.GetByID. */
    public Map<String, Object> read(int id) { return shared.read(id, DOC); }

    // ================================================================================= save (Insert :743)

    @Transactional
    public Map<String, Object> save(Map<String, Object> req) {
        UserAccount u = user();
        int year = ctx.currentFinancialYearId();
        Map<String, Boolean> rt = r();
        int id = i(req.get("Id"));
        if (!rt.get(id > 0 ? "update" : "save")) throw new IllegalStateException("You do not have the " + (id > 0 ? "Update" : "Save") + " right for this screen.");
        Map<String, Object> existing = null;
        if (id > 0) {
            existing = read(id);
            if (b(col(existing, "IsApproved"))) throw new IllegalArgumentException("Record Not Update beacause Record has approved");
        }
        List<Map<String, Object>> grid = rows(req.get("details"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        /* FormValidation :270 */
        int customerId = i(req.get("SupplierCustomerId"));
        List<Map<String, Object>> customers = own.customers(u);
        Map<String, Object> customer = customers.stream().filter(x -> i(col(x, "Id")) == customerId).findFirst().orElse(null);
        if (customer == null) throw new IllegalArgumentException("CustomerName Field is Required");
        String docNoText = s(req.get("DocNo")).trim();
        if (docNoText.isEmpty() || docNoText.equals("0")) throw new IllegalArgumentException("DocNo Field is Required");
        String dueDaysText = s(req.get("DueDays")).trim();
        if (dueDaysText.isEmpty()) throw new IllegalArgumentException("Due Days Field is Required");

        List<Map<String, Object>> otherItems = own.otherItems(u);
        List<Map<String, Object>> expGrid = rows(req.get("expenses"));
        for (var e : expGrid)
            if (d(e.get("Amount")) > 0.0 && i(e.get("ItemId")) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");

        double commRate = d(req.get("CommRate"));
        int commAgent = i(req.get("CommissionAgentId"));

        /* lookups the detail rows are checked against (FormValidationDetila :293) */
        Set<Integer> warehouseIds = ids(own.warehouses(u)), jobLotIds = ids(own.jobLots(u)), packingIds = ids(own.packingTypes());
        Map<Integer, Map<String, Object>> itemById = new HashMap<>();
        for (var x : own.items(u)) itemById.putIfAbsent(i(col(x, "Id")), x);

        List<Detail> details = new ArrayList<>();
        double totalItemAmount = 0.0;
        int line = 1;
        for (var L : grid) {
            Detail v = new Detail();
            v.LineId = line;
            v.WarehouseId = i(L.get("WarehouseId"));
            v.JobLotId = i(L.get("JobLotId"));
            v.ItemId = i(L.get("ItemId"));
            v.PackingTypeId = i(L.get("PackingTypeId"));
            v.ItemUOMId = i(L.get("PackUOMId"));
            v.ItemQty = d(L.get("ItemQty"));
            v.GrossWeight = d(L.get("GrossWeight"));
            v.AdLsWeight = d(L.get("AddLss"));
            v.ItemRate = d(L.get("Rate"));
            v.UomScheduleIdRate = i(L.get("RateUOMId"));
            v.VehicleNo = s(L.get("VehicleNo"));
            if (!warehouseIds.contains(v.WarehouseId)) throw new IllegalArgumentException("WareHouse Field is Required");
            if (!jobLotIds.contains(v.JobLotId)) throw new IllegalArgumentException("JobLot Field is Required");
            if (!itemById.containsKey(v.ItemId)) throw new IllegalArgumentException("Item Name Field is Required");
            if (!packingIds.contains(v.PackingTypeId)) throw new IllegalArgumentException("Packing Type Field is Required");
            if (v.ItemQty <= 0.0) throw new IllegalArgumentException("Qty Field is Required");
            List<Map<String, Object>> schedule = own.uomsOfItem(u, v.ItemId);
            Map<String, Object> packUom = schedule.stream().filter(x -> i(col(x, "Id")) == v.ItemUOMId).findFirst().orElse(null);
            if (packUom == null) throw new IllegalArgumentException("Pack Unit Field is Required");
            if (v.GrossWeight <= 0.0) throw new IllegalArgumentException("Gross Weight Field is Required");
            /* Total() :1217 - Math.Round(Gross + Add/Less, 2) */
            v.NetBillWeight = even(v.GrossWeight + v.AdLsWeight, 2);
            if (v.NetBillWeight <= 0.0) throw new IllegalArgumentException("Net Bill Weight Field is Required");
            if (v.ItemRate <= 0.0) throw new IllegalArgumentException("Rate Field is Required");
            Map<String, Object> rateUom = schedule.stream().filter(x -> i(col(x, "Id")) == v.UomScheduleIdRate).findFirst().orElse(null);
            if (rateUom == null) throw new IllegalArgumentException("Rate UOM Field is Required");
            v.RateUOM = d(col(rateUom, "Equivalent"));
            /* AmountCaluculation :1279 - Math.Round(Net / RateUOM * Rate) */
            v.ItemAmount = v.NetBillWeight > 0.0 && v.RateUOM > 0.0 && v.ItemRate > 0.0 ? even(v.NetBillWeight / v.RateUOM * v.ItemRate, 0) : 0.0;
            if (v.ItemAmount <= 0.0) throw new IllegalArgumentException("Net Bill Weight Field is Required");
            v.NetStockWeight = v.NetBillWeight;
            v.GpDate = LocalDateTime.now();
            totalItemAmount += v.ItemAmount;
            details.add(v);
            line++;
        }

        /* TotalCommissionAmount :1492 + CommissionProportion :1118 + BillProportion :1100 */
        double commAmount = even(totalItemAmount * commRate / 100.0, 0);
        for (Detail v : details) {
            v.CommissionAmount = v.ItemAmount * commRate / 100.0;
            v.BillAmount = v.ItemAmount - v.CommissionAmount;
        }
        if (commAmount > 0.0 && commAgent == 0) throw new IllegalArgumentException("Please Select Broker Account First");

        List<Expense> expenses = new ArrayList<>();
        double expTotal = 0.0;
        for (var e : expGrid) {
            int itemId = i(e.get("ItemId"));
            if (itemId == 0) continue;
            if (otherItems.stream().noneMatch(x -> i(col(x, "Id")) == itemId)) throw new IllegalArgumentException("Please Select an Item Against Expense First");
            Expense pe = new Expense();
            pe.InvRevExpItemId = itemId;
            pe.Qty = d(e.get("Qty"));
            pe.Rate = d(e.get("Rate"));
            /* grdInvExp_CellUpdated :1072 - Amount = Qty * Rate once either is typed */
            pe.Amount = pe.Qty * pe.Rate;
            pe.Remarks = s(e.get("Remarks"));
            expTotal += pe.Amount;
            expenses.add(pe);
        }

        /* BillAmount() :1026 */
        double bill = totalItemAmount + expTotal;
        if (customerId == commAgent) bill -= commAmount;
        bill = even(bill, 0);

        LocalDateTime now = LocalDateTime.now();
        LocalDateTime docDate = dt(req.get("DocDate"));
        if (docDate == null) throw new IllegalArgumentException("Doc Date is required");
        int dueDays = ci(dueDaysText);
        LocalDateTime dueDate = dt(req.get("DueDate"));
        if (dueDate == null) dueDate = now.plusDays(dueDays);

        Head h = new Head();
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.FinancialYearId = year;
        h.DocumentTypeId = DOC;
        h.ScreenName = SCREEN;
        h.IsReserve = false;
        h.EntryDate = now;
        h.ModifyDate = now;
        h.EntryUser = u.getId();
        h.ModifyUser = u.getId();
        h.Id = id;
        h.DocNo = id > 0 ? i(col(existing, "DocNo")) : repo.nextDocNo(u, year, DOC);
        if (h.DocNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        h.DocDate = docDate;
        h.SupplierCustomerId = customerId;
        h.ManualBillNo = s(req.get("ManualBillNo")).trim();
        h.RemarksHeader = s(req.get("RemarksHeader")).trim();
        h.SupplierInvoiceDate = dueDate;
        h.SupplierInvoiceNo = ci(dueDaysText);
        h.CommissionAgentId = commAgent;
        h.CommRate = commRate;
        h.CommAmount = commAmount;
        h.CommissionRemarks = "Brokery% : " + SaleInvoiceFinancialDirect.g(commRate) + "  PartyName:" + s(col(customer, "CompanyName")).trim();
        h.BillAmount = bill;
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
        out.put("voucherHeadId", repo.voucherHeadId(h.OrganizationId, h.CompanyId, DOC, saved));
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

    /** Conversion.ToInt of a text: whole numbers only, else 0. */
    private static int ci(String t) {
        t = t == null ? "" : t.trim();
        return t.matches("[-+]?\\d+") ? Integer.parseInt(t) : 0;
    }

    /** .NET Math.Round(double, digits): midpoint to even. */
    private static double even(double x, int digits) {
        double f = Math.pow(10, digits);
        return Math.rint(x * f) / f;
    }
}
