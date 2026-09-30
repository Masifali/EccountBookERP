package com.mst.services;

import com.mst.repositories.GrnLoaderRepository;
import com.mst.repositories.PurchaseInvoiceAgainstGrnDirectRepository;
import com.mst.repositories.PurchaseInvoiceNumberingRepository;
import com.mst.repositories.PurchaseInvoiceRecordRepository;
import com.mst.repositories.PurchaseInvoiceWriteRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.*;
import java.util.stream.Collectors;

import static com.mst.repositories.PurchaseInvoiceAgainstGrnDirectRepository.GRN_TYPE;
import static com.mst.repositories.PurchaseInvoiceAgainstGrnDirectRepository.TYPE;
import static com.mst.services.PurchaseInvoiceFinancialRules.copy;
import static com.mst.services.PurchaseInvoiceFinancialRules.i;
import static com.mst.services.PurchaseInvoiceFinancialRules.n;
import static com.mst.services.PurchaseInvoiceFinancialRules.s;

/**
 * Purchase Invoice Against GRN Direct - desktop Architecture.WinApp.Purchase.frmPurchaseInvoiceAgaintGrnDirect
 * (ScreenDefinition 131 "PurchaseInvoiceAgainstGrnDirect", DocumentTypeId 138). Line numbers below are that file.
 *
 * Save chain (BLL 0581 Save -> PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice -> DAL 0434 SetData):
 * Sp_InvPurchaseInvoice_Insert/_Update, Sp_InvPurchaseInvoiceDetail_Insert per row, Sp_InvPurchaseInvoiceExpense_Insert,
 * Sp_InvPurchaseInvoiceEmptyBags_Insert, usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations, DMS attachments,
 * then (PurchaseInvoiceWriteRepository.post) usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId,
 * Sp_InventoryStockEvalautionDetail_Update, Sp_VoucherHead_Insert/_Update + Sp_VoucherDetail_Insert, USP_VoucherBalanceCheck,
 * voucher history, DAW.USp_DocumentApprovalDetail_Insert. One transaction.
 */
@Service
public class PurchaseInvoiceAgainstGrnDirectService {

    private static final String SCREEN_NAME = "frmPurchaseInvoiceAgaintGrnDirect";

    private final PurchaseInvoiceAgainstGrnDirectRepository repository;
    private final PurchaseInvoiceRecordRepository records;
    private final PurchaseInvoiceWriteRepository writes;
    private final PurchaseInvoiceNumberingRepository numbering;
    private final PurchaseInvoiceAttachmentService attachments;
    private final GrnLoaderRepository grnLoader;
    private final CurrentUserContext context;
    private final JdbcTemplate jdbc;

    public PurchaseInvoiceAgainstGrnDirectService(PurchaseInvoiceAgainstGrnDirectRepository repository, PurchaseInvoiceRecordRepository records,
                                                  PurchaseInvoiceWriteRepository writes, PurchaseInvoiceNumberingRepository numbering,
                                                  PurchaseInvoiceAttachmentService attachments, GrnLoaderRepository grnLoader,
                                                  CurrentUserContext context, JdbcTemplate jdbc) {
        this.repository = repository; this.records = records; this.writes = writes; this.numbering = numbering;
        this.attachments = attachments; this.grnLoader = grnLoader; this.context = context; this.jdbc = jdbc;
    }

    // ------------------------------------------------------------------------------------------------ helpers
    private String config(String name) { return writes.configuration(context.currentOrganizationId(), context.currentCompanyId(), name); }
    private boolean flag(String name) { return Boolean.parseBoolean(config(name).trim()); }
    private int amountDigits() {
        String v = config("Default NoofDecimal Points For Amount").trim();
        int d = v.isEmpty() ? 0 : Integer.parseInt(v);
        if (d < 0 || d > 15) throw new IllegalStateException("Invalid amount decimal configuration");
        return d;
    }
    private static double awayFromZero(double v, int digits) { return BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_UP).doubleValue(); }
    private static double bankers(double v, int digits) { return BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_EVEN).doubleValue(); }
    /** ToString("#,##0.###") then Conversion.ToDouble: three decimals, midpoint away from zero. */
    private static double three(double v) { return awayFromZero(v, 3); }
    private static boolean blank(Object v) { return v == null || v.toString().trim().isEmpty(); }
    private static double num(Object v) {
        if (blank(v)) return 0;
        if (v instanceof Number x) return x.doubleValue();
        try { return Double.parseDouble(v.toString().trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }
    private static String dateText(Object v) { return v == null ? "" : v.toString().length() >= 10 ? v.toString().substring(0, 10) : v.toString(); }

    private Map<String, Boolean> rights() {
        var r = new LinkedHashMap<String, Boolean>();
        for (String name : List.of("View", "Save", "Update", "Delete", "Print", "CanView AllRecord")) r.put(name.replace(" ", ""), records.hasRight(TYPE, name));
        return r;
    }

    // ------------------------------------------------------------------------------------------------ form load
    /** InvfrmPurchaseInvoice_Load :361-487 (the form's Load handler keeps the 56 name). */
    public Map<String, Object> init() {
        records.requireRight(TYPE, "View");
        var out = new LinkedHashMap<String, Object>();
        out.put("rights", rights());
        // DocumentNo :1351 shows "Max Number Not Found" and the form still opens.
        int code = numbering.next(context.currentOrganizationId(), context.currentCompanyId(), context.currentFinancialYearId(), TYPE);
        out.put("docNo", code > 0 ? code : "");
        if (code <= 0) out.put("docNoMessage", "Max Number Not Found");
        out.put("locationTypes", repository.locationTypes());
        out.put("currencies", repository.currencies());
        out.put("freightAccounts", repository.freightAccounts());
        out.put("bagCreditAccounts", repository.bagCreditAccounts());
        out.put("suppliers", suppliers());
        out.put("paymentTerms", repository.paymentTerms());
        out.put("otherItems", repository.otherItems());
        out.put("historySuppliers", repository.historySuppliers());
        out.put("lastTransport", repository.lastTransportAndBroker());
        var cfg = new LinkedHashMap<String, Object>();
        cfg.put("baseCurrency", config("Base Currency"));
        cfg.put("baseCurrencyRate", config("BaseCurrencyRate"));
        cfg.put("defaultDaysToLessFromHistoryFromDate", config("DefaultDaysToLessFromHistoryFromDate"));
        cfg.put("wagesAmountCalculateOnQty", flag("WagesAmountCalculateOnQty"));
        cfg.put("contractWagesChargetoProduct", flag("ContractWagesChargetoProduct"));
        cfg.put("acceptAccessWtHold", flag("AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt"));
        cfg.put("amountDigits", amountDigits());
        out.put("configuration", cfg);
        return out;
    }

    /** btnFrmRefresh_Click :2860 - Location, Currency, Freight accounts and Suppliers again. */
    public Map<String, Object> refresh() {
        records.requireRight(TYPE, "View");
        var out = new LinkedHashMap<String, Object>();
        out.put("locationTypes", repository.locationTypes());
        out.put("currencies", repository.currencies());
        out.put("freightAccounts", repository.freightAccounts());
        out.put("suppliers", suppliers());
        return out;
    }

    /** BtnRefreshHistory_Click :3324 exists on the desktop but is not wired to the button; kept for the combo only. */
    public List<Map<String, Object>> historySuppliers() { records.requireRight(TYPE, "View"); return repository.historySuppliers(); }

    private List<Map<String, Object>> suppliers() {
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : repository.suppliers()) {
            var r = copy(raw);
            var m = new LinkedHashMap<String, Object>();
            m.put("Id", i(r, "Id")); m.put("CompanyName", r.get("CompanyName")); m.put("GlAccountId", i(r, "GlAccountId"));
            out.add(m);
        }
        return out;
    }

    /** DocumentNo :1351 - CommonServices.PurchaseInvoiceGenerateCode(138); "Max Number Not Found" when 0. */
    public int nextDocNo() {
        int code = numbering.next(context.currentOrganizationId(), context.currentCompanyId(), context.currentFinancialYearId(), TYPE);
        if (code <= 0) throw new IllegalArgumentException("Max Number Not Found");
        return code;
    }

    /** cmbCurrency_Leave :544-585 - only called by the page when the currency is not the base currency. */
    public Map<String, Object> lastExchangeRate(int currencyId) {
        records.requireRight(TYPE, "View");
        var rows = repository.lastExchangeRate(currencyId);
        return Map.of("found", !rows.isEmpty(), "LastExchRate", rows.isEmpty() ? 0d : n(copy(rows.get(0)), "LastExchRate"));
    }

    // ------------------------------------------------------------------------------------------------ GRN load
    private static String idList(Collection<Integer> ids) { return ids.stream().map(x -> "," + x).collect(Collectors.joining()); }

    /** LoadDataDetailGridAgainstGP :1576-1581 - one grid row from a DirectGrnLoadForPurchaseInvoiceDirect row. */
    private static Map<String, Object> gridRowFromGrn(Map<String, Object> raw, double exchangeRate) {
        var row = copy(raw);
        double rateEquivalent = i(row, "PurchaseOrderId") == 0 ? 40.0 : n(row, "OrderRateEquivalent");
        double net = n(row, "NetBillWeight"), itemRate = n(row, "OrderItemRate");
        double amount = net / rateEquivalent * itemRate;
        var g = new LinkedHashMap<String, Object>();
        g.put("Id", 0); g.put("OrderDetailId", i(row, "PurchaseOrderDetailId")); g.put("OrderId", i(row, "PurchaseOrderId"));
        g.put("OrderNo", row.get("OrderNo")); g.put("GrnDetailId", i(row, "InvGrnDetailId")); g.put("InvGrnId", i(row, "InvGrnId"));
        g.put("GrnDate", dateText(row.get("DocDate"))); g.put("GrnNo", row.get("DocNo"));
        g.put("ItemId", i(row, "ItemId")); g.put("ItemName", row.get("ItemName")); g.put("CropYear", row.get("CropYear"));
        g.put("ItemUomId", i(row, "ItemUOMId")); g.put("PackUom", row.get("UOMCodeItem")); g.put("ItemQty", n(row, "ItemQty"));
        g.put("WarehouseId", i(row, "WarehouseId")); g.put("Warehouse", row.get("WareHouseName")); g.put("PackingTypeId", i(row, "PackingTypeId"));
        g.put("GrossWeight", n(row, "GrossWeight")); g.put("EbUnit", n(row, "EBWPerUnit")); g.put("EbTotal", n(row, "EBTotalWt"));
        g.put("AdLsWeight", n(row, "AdLsWeight")); g.put("NetBillWeight", net); g.put("StockWeight", n(row, "NetStockWeight"));
        g.put("ItemRate", itemRate); g.put("RateUomId", i(row, "OrderItemRateUOMId")); g.put("RateUom", rateEquivalent);
        g.put("ItemAmount", amount); g.put("FcyAmount", exchangeRate > 0 ? amount / exchangeRate : 0);
        g.put("JobLotId", i(row, "JobLotId"));
        for (String k : List.of("RateCut", "RateCutAmount", "Freights", "Expense", "Commission", "Brokery", "ItemNetAmount")) g.put(k, 0d);
        g.put("GpNo", row.get("GpNo")); g.put("VehicleNo", row.get("VehicleNo")); g.put("WagesAmount", 0d);
        return g;
    }

    /** ReadById :2170 - one grid row from a stored DirectPurchaseDetailReadByInvPurchaseInvoiceId row. */
    private static Map<String, Object> gridRowFromStored(Map<String, Object> raw) {
        var d = copy(raw);
        var g = new LinkedHashMap<String, Object>();
        g.put("Id", i(d, "Id")); g.put("OrderDetailId", i(d, "PurchaseOrderDetailId")); g.put("OrderId", i(d, "PurchaseOrderId"));
        g.put("OrderNo", d.get("PurchaseOrder")); g.put("GrnDetailId", i(d, "InvGrnDetailId")); g.put("InvGrnId", i(d, "InvGrnId"));
        g.put("GrnDate", dateText(d.get("GrnDate"))); g.put("GrnNo", d.get("GrnNo"));
        g.put("ItemId", i(d, "ItemId")); g.put("ItemName", d.get("ItemName")); g.put("CropYear", d.get("CropYear"));
        g.put("ItemUomId", i(d, "ItemUOMId")); g.put("PackUom", d.get("UOMCodeItem")); g.put("ItemQty", n(d, "ItemQty"));
        g.put("WarehouseId", i(d, "WarehouseId")); g.put("Warehouse", d.get("WareHouseName")); g.put("PackingTypeId", i(d, "PackingTypeId"));
        g.put("GrossWeight", n(d, "GrossWeight")); g.put("EbUnit", n(d, "EBWeight")); g.put("EbTotal", n(d, "EBTotalWt"));
        g.put("AdLsWeight", n(d, "AdLsWeight")); g.put("NetBillWeight", n(d, "NetBillWeight")); g.put("StockWeight", n(d, "NetStockWeight"));
        g.put("ItemRate", n(d, "ItemRate")); g.put("RateUomId", i(d, "UomScheduleIdRate")); g.put("RateUom", n(d, "EquivalentPoRate"));
        g.put("ItemAmount", n(d, "ItemAmount")); g.put("FcyAmount", n(d, "FcyAmount")); g.put("JobLotId", i(d, "JobLotId"));
        g.put("RateCut", n(d, "RateCut")); g.put("RateCutAmount", n(d, "RateCutAmount")); g.put("Freights", n(d, "FreightAmount"));
        g.put("Expense", n(d, "ExpenseAmount")); g.put("Commission", n(d, "CommissionAmount")); g.put("Brokery", n(d, "Brokery"));
        g.put("ItemNetAmount", n(d, "BillAmount")); g.put("GpNo", d.get("GpNo")); g.put("VehicleNo", d.get("VehicleNo"));
        g.put("WagesAmount", n(d, "WagesAmount"));
        g.put("LocationTypeId", i(d, "LocationTypeId"));
        return g;
    }

    private static Map<String, Object> bagRow(Object orderId, Object typeId, Object type, Object grnId, Object itemId, Object itemName,
                                              double qty, double rate, double amount, Object remarks, Object creditAccountId) {
        var m = new LinkedHashMap<String, Object>();
        m.put("OrderId", orderId); m.put("TypeId", typeId); m.put("Type", type); m.put("InvGrnId", grnId); m.put("ItemId", itemId);
        m.put("ItemName", itemName); m.put("PurchaseQty", qty); m.put("Rate", rate); m.put("Amount", amount);
        m.put("Remarks", remarks); m.put("CreditAccountId", creditAccountId);
        return m;
    }

    /** LoadEmptyBagsData :1631-1652 - rows with (int)PurchaseQty &gt; 0; Amount = PurchaseQty * Rate; Credit account 0. */
    private List<Map<String, Object>> emptyBagsFromGrn(String ids) {
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : repository.grnEmptyBags(ids)) {
            var r = copy(raw);
            if ((int) n(r, "PurchaseQty") <= 0) continue;
            out.add(bagRow(i(r, "PurchaseOrderId"), i(r, "TypeId"), r.get("EmptyBagsType"), i(r, "InvGrnId"), i(r, "ItemId"), r.get("ItemName"),
                    n(r, "PurchaseQty"), n(r, "Rate"), n(r, "PurchaseQty") * n(r, "Rate"), r.get("Remarks"), 0));
        }
        return out;
    }

    private void requirePending(Collection<Integer> grnIds) {
        var rows = new ArrayList<Map<String, Object>>();
        for (int id : grnIds) rows.add(Map.of("Id", id));
        grnLoader.selectionRows(rows, GRN_TYPE); // pending type-137 GRN in an allocated branch, or refused
    }

    /**
     * toolStripButton3_Click_1 :2770 -> frmLoadGRN (DocumentTypeId 137) -> LoadInGridDetail :1593-1629.
     * The body carries the GRN ids chosen in the loader plus the state of the form's grid that the desktop
     * compares against (current supplier, delivery term, first row's GRN, already-loaded GRN detail ids).
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> loadGrns(Map<String, Object> body) {
        records.requireRight(TYPE, "View");
        var grnIds = new LinkedHashSet<Integer>();
        if (body.get("grnIds") instanceof List<?> list) for (Object o : list) { int id = (int) num(o); if (id > 0) grnIds.add(id); }
        if (grnIds.isEmpty()) throw new IllegalArgumentException("Check the row first");
        requirePending(grnIds);
        String ids = idList(grnIds);
        var out = new LinkedHashMap<String, Object>();
        // :1602-1606 wages are read first, so they change even when the load below is refused.
        var wages = repository.wages(ids);
        if (!wages.isEmpty()) out.put("wagesAmount", copy(wages.get(0)).get("WagesAmount"));
        var grn = repository.grnRows(ids);
        if (grn.isEmpty()) { out.put("rows", List.of()); return out; }
        var first = copy(grn.get(0));
        int existingSupplier = (int) num(body.get("supplierId"));
        int newSupplier = i(first, "SupplierCustomerId");
        String deliveryTerm = Objects.toString(body.get("deliveryTerm"), "");
        boolean hasRows = num(body.get("rowCount")) > 0;
        if (hasRows) {
            String refused = null;
            if (existingSupplier > 0 && existingSupplier != newSupplier) refused = "Sorry, you can't load a different supplier at this moment!";
            else if (!deliveryTerm.isEmpty() && !deliveryTerm.equals(s(first, "DeliveryTerm")))
                refused = "Sorry, different Delivery Term rows already loaded. You can't load different Delivery Term rows.";
            else if (num(body.get("firstInvGrnId")) != i(first, "InvGrnId"))
                refused = "Sorry, different GRN rows already loaded. You can't load different GRN rows.";
            if (refused != null) { out.put("message", refused); out.put("rows", List.of()); return out; }
        }
        var existing = new HashSet<Integer>();
        if (body.get("existingGrnDetailIds") instanceof List<?> list) for (Object o : list) existing.add((int) num(o));
        double exchangeRate = num(body.get("exchangeRate"));
        var rows = new ArrayList<Map<String, Object>>();
        for (var raw : grn) if (!existing.contains(i(copy(raw), "InvGrnDetailId"))) rows.add(gridRowFromGrn(raw, exchangeRate));
        // :1565-1567 carriage = sum over the first row of each GRN
        var seen = new HashSet<Integer>();
        double carriage = 0;
        for (var raw : grn) { var r = copy(raw); if (seen.add(i(r, "InvGrnId"))) carriage += n(r, "CarriageAmount"); }
        out.put("supplierId", newSupplier);
        out.put("deliveryTerm", s(first, "DeliveryTerm"));
        out.put("rows", rows);
        out.put("freightAmount", carriage);
        out.put("transporterId", i(first, "TransporterId"));
        out.put("emptyBags", emptyBagsFromGrn(ids));
        return out;
    }

    // ------------------------------------------------------------------------------------------------ read
    /** ReadById :2127-2213. */
    public Map<String, Object> getById(int id) {
        var stored = records.require(id, TYPE);
        var h = copy(repository.header(id));
        var out = new LinkedHashMap<String, Object>();
        out.put("Id", id);
        out.put("DocNo", h.get("DocNo"));
        out.put("DocDate", dateText(h.get("DocDate")));
        out.put("SupplierCustomerId", i(h, "SupplierCustomerId"));
        out.put("StockPartyId", i(h, "StockPartyId"));
        out.put("ManualBillNo", h.get("ManualBillNo"));
        out.put("PaymentTermsId", i(h, "PaymentTermsId"));
        out.put("DeliveryTerm", h.get("DeliveryTerm"));
        out.put("CommissionAgentId", i(h, "CommissionAgentId"));
        out.put("CommRate", n(h, "CommRate"));
        out.put("CommAmount", n(h, "CommAmount"));
        out.put("BrokerAgentId", i(h, "BrokerAgentId"));
        out.put("BrokeryRate", n(h, "BrokeryRate"));
        out.put("BrokeryAmount", n(h, "BrokeryAmount"));
        out.put("TransportAccountId", i(h, "TransportAccountId"));
        out.put("FreightAmount", n(h, "FreightAmount"));
        out.put("RemarksHeader", h.get("RemarksHeader"));
        out.put("WagesAmount", n(h, "WagesAmount"));
        out.put("BillAmount", n(h, "BillAmount"));
        out.put("DueDate", dateText(h.get("DueDate")));
        out.put("DueDays", i(h, "DueDays"));
        out.put("IsApproved", Boolean.TRUE.equals(stored.get("IsApproved")) || "1".equals(Objects.toString(stored.get("IsApproved"))));
        out.put("VoucherHeadId", repository.voucherHeadId(id));
        var details = new ArrayList<Map<String, Object>>();
        var grnIds = new LinkedHashSet<Integer>();
        for (var raw : repository.read(id, "DirectPurchaseDetailReadByInvPurchaseInvoiceId")) {
            var g = gridRowFromStored(raw);
            details.add(g);
            grnIds.add((Integer) g.get("InvGrnId"));
        }
        out.put("details", details);
        out.put("LocationTypeId", details.isEmpty() ? 0 : details.get(0).get("LocationTypeId"));
        // :2166-2181 - GrnIds is built as ",id" per detail row (duplicates included).
        var wagesIds = new StringBuilder();
        for (var g : details) wagesIds.append(',').append(g.get("InvGrnId"));
        if (!details.isEmpty()) {
            var wages = repository.wages(wagesIds.toString());
            if (!wages.isEmpty()) out.put("WagesAmount", copy(wages.get(0)).get("WagesAmount"));
        }
        var bags = new ArrayList<Map<String, Object>>();
        for (var raw : repository.read(id, "InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID")) {
            var b = copy(raw);
            bags.add(bagRow(i(b, "PurchaseOrderId"), i(b, "TypeId"), b.get("EmptyBagsType"), i(b, "InvGrnId"), i(b, "ItemId"), b.get("ItemName"),
                    n(b, "PurchaseQty"), n(b, "Rate"), n(b, "Amount"), b.get("Remarks"), i(b, "CreditAccountId")));
        }
        out.put("emptyBags", bags);
        var expenses = new ArrayList<Map<String, Object>>();
        for (var raw : repository.read(id, "InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID")) {
            var e = copy(raw);
            var m = new LinkedHashMap<String, Object>();
            m.put("ItemId", i(e, "InvRevExpItemId")); m.put("ItemName", e.get("OtherItemName")); m.put("Remarks", e.get("Remarks")); m.put("Amount", n(e, "Amount"));
            expenses.add(m);
        }
        out.put("expenses", expenses);
        out.put("rights", rights());
        return out;
    }

    /** GetAll :1870-1978 + HistoryGridSettings :1980. */
    public List<Map<String, Object>> history(Map<String, Object> body) {
        records.requireRight(TYPE, "View");
        String mode = Objects.toString(body.get("dateMode"), "doc");
        java.sql.Date from = parseDate(body.get("fromDate")), to = parseDate(body.get("toDate"));
        var rows = repository.history(records.hasRight(TYPE, "CanView AllRecord"), mode, from, to,
                (int) num(body.get("fromDocNo")), (int) num(body.get("toDocNo")), (int) num(body.get("supplierId")));
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : rows) {
            var r = copy(raw);
            var m = new LinkedHashMap<String, Object>();
            m.put("Id", i(r, "Id")); m.put("VoucherHeadId", i(r, "VoucherHeadId")); m.put("DocumentTypeId", i(r, "DocumentTypeId"));
            m.put("DocNo", r.get("DocNo")); m.put("DocDate", dateText(r.get("DocDate"))); m.put("SupplierName", r.get("SupplierName"));
            m.put("ManualBillNo", r.get("ManualBillNo")); m.put("DueDays", r.get("DueDays")); m.put("DueDate", dateText(r.get("DueDate")));
            m.put("CommAgent", r.get("CommissionAgent")); m.put("CommType", r.get("CommissionType")); m.put("CommRate", r.get("CommRate"));
            m.put("CommAmount", r.get("CommAmount")); m.put("CommRemarks", r.get("CommissionRemarks")); m.put("BillAmount", r.get("BillAmount"));
            m.put("ApprovedStatus", r.get("ApprovedStatus")); m.put("EntryUser", r.get("EntryUser")); m.put("EntryDate", r.get("EntryDate"));
            m.put("ModifyUser", r.get("ModifyUser")); m.put("ModifyDate", r.get("ModifyDate")); m.put("ApprovedUser", r.get("ApprovedUser"));
            m.put("ApprovedDate", r.get("PostDate")); m.put("NoOfAttachments", r.get("NoOfAttachments")); m.put("Remarks", r.get("RemarksHeader"));
            out.add(m);
        }
        return out;
    }

    private static java.sql.Date parseDate(Object v) {
        if (blank(v)) return null;
        try { return java.sql.Date.valueOf(v.toString().trim().substring(0, 10)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Use a valid history filter date"); }
    }

    /** GetDetailGrdByHeadId :1014-1074 - the lower grid of the History tab. */
    public List<Map<String, Object>> historyDetail(int id) {
        records.require(id, TYPE);
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : repository.read(id, "DirectPurchaseDetailReadByInvPurchaseInvoiceId")) {
            var d = copy(raw);
            var m = new LinkedHashMap<String, Object>();
            m.put("Id", i(d, "Id")); m.put("OrderNo", d.get("PurchaseOrder")); m.put("GrnDate", dateText(d.get("GrnDate"))); m.put("GrnNo", d.get("GrnNo"));
            m.put("Warehouse", d.get("WareHouseName")); m.put("ItemName", d.get("ItemName")); m.put("CropYear", d.get("CropYear"));
            m.put("PackType", d.get("PackTypeDesc")); m.put("PackUom", d.get("UOMCodeItem")); m.put("ItemQty", n(d, "ItemQty"));
            m.put("GrossWeight", n(d, "GrossWeight")); m.put("EbUnit", n(d, "EBWeight")); m.put("EbTotal", n(d, "EBTotalWt"));
            m.put("NetBillWeight", n(d, "NetBillWeight")); m.put("StockWeight", n(d, "NetStockWeight")); m.put("ItemRate", n(d, "ItemRate"));
            m.put("RateUom", d.get("RateUom")); m.put("ItemAmount", n(d, "ItemAmount")); m.put("RateCut", n(d, "RateCut"));
            m.put("RateCutAmount", n(d, "RateCutAmount")); m.put("CommissionAmount", n(d, "CommissionAmount")); m.put("Brokery", n(d, "Brokery"));
            m.put("ExpenseAmount", n(d, "ExpenseAmount")); m.put("FreightAmount", n(d, "FreightAmount")); m.put("WagesAmount", n(d, "WagesAmount"));
            m.put("ItemNetAmount", n(d, "BillAmount")); m.put("VehicleNo", d.get("VehicleNo"));
            out.add(m);
        }
        return out;
    }

    // ------------------------------------------------------------------------------------------------ delete
    /** btnDelete_Click :3466-3506 -> InvPurchaseInvoice.RemoveByID -> Sp_InvoicesVouchersandStocksDelete. */
    public Map<String, Object> delete(int id) {
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        var stored = records.require(id, TYPE);
        if (Boolean.TRUE.equals(stored.get("IsApproved")) || "1".equals(Objects.toString(stored.get("IsApproved"))))
            throw new IllegalArgumentException("Record Not Delete because Record has approved");
        records.delete(id, TYPE);
        return Map.of("success", true, "message", "Delete record Successfully");
    }

    // ------------------------------------------------------------------------------------------------ save
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        var out = new ArrayList<Map<String, Object>>();
        if (v instanceof List<?> l) for (Object o : l) if (o instanceof Map<?, ?> m) out.add(copy((Map<String, Object>) m));
        return out;
    }

    /**
     * btnSave_Click :2561 / btnUpdate_Click :2215 -> Insert() :2235-2559. The browser supplies what the operator
     * types or edits (header fields, the four editable grid cells, expense amounts, empty-bag rate/remarks/credit
     * account, the manual FCY amount); every GRN-sourced value is re-read here, and every derived amount the form
     * computes (rate-cut amount, freight/expense/commission/brokery/wages proportions, brokery amount, bill amount,
     * FCY/LCY) is recomputed with the form's own formulas.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        int id = (int) num(body.get("id"));
        boolean update = id > 0;
        records.requireRight(TYPE, update ? "Update" : "Save");
        Map<String, Object> old = copy(null);
        if (update) {
            old = copy(records.require(id, TYPE));
            if (Boolean.TRUE.equals(old.get("IsApproved")) || "1".equals(Objects.toString(old.get("IsApproved"))))
                throw new IllegalArgumentException("Record Not Update because Record has approved");
        } else if (body.containsKey("id") && num(body.get("id")) < 0) {
            throw new IllegalArgumentException("Record Not Update because Record Id not found");
        }
        int org = context.currentOrganizationId(), company = context.currentCompanyId();
        int digits = amountDigits();

        // ---- FormValidation :1163-1220 (same order and wording)
        var locations = repository.locationTypes();
        int locationTypeId = (int) num(body.get("locationTypeId"));
        if (!locations.isEmpty() && (locationTypeId == 0 || locations.stream().noneMatch(r -> i(copy(r), "Id") == locationTypeId)))
            throw new IllegalArgumentException("Location Type Field is Required");
        int docNo = update ? i(old, "DocNo") : nextDocNo();
        if (docNo <= 0) throw new IllegalArgumentException("Doc No Name is Required");
        var supplierRows = suppliers();
        int supplierId = (int) num(body.get("supplierId"));
        var supplier = supplierRows.stream().filter(r -> i(r, "Id") == supplierId).findFirst().orElse(null);
        if (supplierId <= 0 || supplier == null) throw new IllegalArgumentException("Supplier Name is Required");
        int supplierGl = i(supplier, "GlAccountId");
        int paymentTermId = (int) num(body.get("paymentTermId"));
        var term = repository.paymentTerms().stream().map(PurchaseInvoiceFinancialRules::copy).filter(r -> i(r, "Id") == paymentTermId).findFirst().orElse(null);
        if (paymentTermId <= 0 || term == null) throw new IllegalArgumentException("Payment Term is Required");
        boolean cash = "Cash".equals(s(term, "TermsDescription"));
        int dueDays = cash ? 0 : (int) num(body.get("dueDays")); // CmbPaymentTerm_TextChanged :3255
        if (paymentTermId == 2 && dueDays <= 0) throw new IllegalArgumentException("DueDays Field is Required");

        // ---- the grid rows: GRN values from the database, editable cells from the browser
        var suppliedRows = list(body.get("details"));
        var storedDetails = new HashMap<Integer, Map<String, Object>>();
        if (update) for (var raw : repository.read(id, "DirectPurchaseDetailReadByInvPurchaseInvoiceId")) { var g = gridRowFromStored(raw); storedDetails.put((Integer) g.get("Id"), g); }
        var newGrnIds = new LinkedHashSet<Integer>();
        for (var r : suppliedRows) if ((int) num(r.get("Id")) <= 0 && (int) num(r.get("InvGrnId")) > 0) newGrnIds.add((int) num(r.get("InvGrnId")));
        var grnSource = new HashMap<Integer, Map<String, Object>>();
        if (!newGrnIds.isEmpty()) {
            requirePending(newGrnIds);
            for (var raw : repository.grnRows(idList(newGrnIds))) { var g = gridRowFromGrn(raw, 0); grnSource.put((Integer) g.get("GrnDetailId"), g); }
        }
        var rows = new ArrayList<Map<String, Object>>();
        var seenIds = new HashSet<Integer>(); var seenGrnDetails = new HashSet<Integer>();
        for (var r : suppliedRows) {
            int rowId = (int) num(r.get("Id"));
            Map<String, Object> source;
            if (rowId > 0) {
                source = storedDetails.get(rowId);
                if (source == null || !seenIds.add(rowId)) throw new IllegalArgumentException("A detail row belongs to another invoice or is repeated");
            } else {
                source = grnSource.get((int) num(r.get("GrnDetailId")));
                if (source == null) throw new IllegalArgumentException("InvGrnDetailId Required");
            }
            if (!seenGrnDetails.add(i(source, "GrnDetailId"))) throw new IllegalArgumentException("A GRN detail row is repeated");
            var g = new LinkedHashMap<String, Object>(source);
            for (String k : List.of("ItemRate", "RateUom", "RateCut", "ItemAmount", "FcyAmount")) g.put(k, num(r.get(k)));
            rows.add(g);
        }

        String deliveryTerm = update ? s(old, "DeliveryTerm") : "";
        if (!rows.isEmpty() && !newGrnIds.isEmpty()) {
            var first = repository.grnRows(idList(newGrnIds));
            if (!first.isEmpty()) {
                var f = copy(first.get(0));
                if (rows.stream().allMatch(x -> i(x, "Id") == 0)) deliveryTerm = s(f, "DeliveryTerm");
                if (i(f, "SupplierCustomerId") != supplierId) throw new IllegalArgumentException("Sorry, you can't load a different supplier at this moment!");
            }
        }
        if (deliveryTerm.isEmpty()) throw new IllegalArgumentException("Delivery Term is Required");
        var currencies = repository.currencies();
        int currencyId = (int) num(body.get("currencyId"));
        if (currencyId == 0 || currencies.stream().noneMatch(c -> i(copy(c), "Id") == currencyId)) throw new IllegalArgumentException("Fcy Code Field is Required");
        double exchangeRate = num(body.get("exchangeRate"));
        if (exchangeRate == 0) throw new IllegalArgumentException("Exchange Rate Field is Required");

        // ---- recompute what the form computes
        boolean fcyManual = Boolean.TRUE.equals(body.get("fcyManual"));
        boolean anyOrder = rows.stream().anyMatch(x -> i(x, "OrderId") > 0);
        if (anyOrder) fcyManual = false; // grdSettings :875-876 unticks and disables the FCY box when rows carry an order
        for (var g : rows) {
            if (n(g, "RateUom") == 0) g.put("RateUom", 1.0); // grd_CellUpdated :927-931
            g.put("RateCutAmount", n(g, "NetBillWeight") / n(g, "RateUom") * n(g, "RateCut"));
            if (!fcyManual) g.put("FcyAmount", n(g, "ItemAmount") / exchangeRate);
        }
        double itemTotal = rows.stream().mapToDouble(g -> n(g, "ItemAmount")).sum();
        double netWeight = rows.stream().mapToDouble(g -> n(g, "NetBillWeight")).sum();
        double qtyTotal = rows.stream().mapToDouble(g -> n(g, "ItemQty")).sum();
        boolean commRateBlank = blank(body.get("commRate"));
        double commRate = num(body.get("commRate"));
        // txtcommamount is typed over or recalculated from the rate (TotalCommissionAmount :1381); the typed value is what Insert() reads.
        double commAmount = body.containsKey("commAmount") ? num(body.get("commAmount")) : commRateBlank ? 0 : three(itemTotal * commRate / 100.0);
        boolean brokeryRateBlank = blank(body.get("brokeryRate"));
        double brokeryRate = num(body.get("brokeryRate"));
        double brokeryAmount = brokeryRateBlank ? 0 : three(itemTotal * brokeryRate / 100.0); // TotalBrokeryAmount :1407 (read-only box)
        int commissionAgentId = (int) num(body.get("commissionAgentId"));
        int brokerAgentId = (int) num(body.get("brokerAgentId"));
        int transportAccountId = (int) num(body.get("transportAccountId"));
        double freightAmount = num(body.get("freightAmount"));
        if (commissionAgentId > 0 && supplierRows.stream().noneMatch(r -> i(r, "Id") == commissionAgentId)) throw new IllegalArgumentException("Please Select Commission Agent Account First");
        if (brokerAgentId > 0 && supplierRows.stream().noneMatch(r -> i(r, "Id") == brokerAgentId)) throw new IllegalArgumentException("Please Select Brokery Account First");
        if (transportAccountId > 0 && repository.freightAccounts().stream().noneMatch(r -> i(copy(r), "Id") == transportAccountId)) throw new IllegalArgumentException("Please Select Transporter Account First");
        // wages: header box is disabled and comes from the GRNs (:1602-1606 / :2177-2181), otherwise the stored header value.
        double wagesHeader = update ? n(old, "WagesAmount") : 0;
        if (!rows.isEmpty()) {
            var ids = new StringBuilder();
            for (var g : rows) ids.append(',').append(i(g, "InvGrnId"));
            var w = repository.wages(ids.toString());
            if (!w.isEmpty()) wagesHeader = n(copy(w.get(0)), "WagesAmount");
        }
        boolean wagesOnQty = flag("WagesAmountCalculateOnQty");
        // expense grid (Other Expense): the master list rows (or the stored rows of a loaded invoice) with typed amount/remarks
        var expenseSource = new LinkedHashMap<Integer, Map<String, Object>>();
        for (var raw : repository.otherItems()) { var e = copy(raw); expenseSource.put(i(e, "Id"), Map.of("ItemName", s(e, "OtherItemName"))); }
        if (update) for (var raw : repository.read(id, "InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID")) { var e = copy(raw); expenseSource.putIfAbsent(i(e, "InvRevExpItemId"), Map.of("ItemName", s(e, "OtherItemName"))); }
        var expenses = new ArrayList<Map<String, Object>>();
        for (var r : list(body.get("expenses"))) {
            int itemId = (int) num(r.get("ItemId"));
            if (num(r.get("Amount")) > 0 && itemId == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");
            if (itemId == 0) continue;
            var src = expenseSource.get(itemId);
            if (src == null) throw new IllegalArgumentException("Please Select an Item Against Expense First");
            var e = new LinkedHashMap<String, Object>();
            e.put("ItemId", itemId); e.put("ItemName", src.get("ItemName")); e.put("Remarks", Objects.toString(r.get("Remarks"), "")); e.put("Amount", num(r.get("Amount")));
            expenses.add(e);
        }
        double expenseTotal = expenses.stream().mapToDouble(e -> n(e, "Amount")).sum();
        // proportions - FreightProportion :1687, ExpProportion :1654, CommissionProportion :1798, BrokeryProportion :1834, WagesAmountProportion :1723
        for (var g : rows) {
            double w = n(g, "NetBillWeight");
            g.put("Freights", "Load".equals(deliveryTerm) ? freightAmount / netWeight * w : 0d);
            g.put("Expense", expenseTotal > 0 ? expenseTotal / netWeight * w : 0d);
            g.put("Commission", commAmount > 0 ? n(g, "ItemAmount") * commRate / 100.0 : 0d);
            g.put("Brokery", brokeryAmount > 0 && brokeryRate > 0 ? n(g, "ItemAmount") * brokeryRate / 100.0 : 0d);
            g.put("WagesAmount", wagesHeader > 0 ? (wagesOnQty ? wagesHeader / qtyTotal * n(g, "ItemQty") : wagesHeader / netWeight * w) : 0d);
            // BillProportion :1773 - the wages term reads the GridEX cell object, not its value, so it always adds 0.
            g.put("ItemNetAmount", awayFromZero(n(g, "ItemAmount") + n(g, "Expense") + n(g, "Freights") + n(g, "Commission") + n(g, "Brokery"), digits));
        }
        // empty bags: GRN / stored source rows, typed rate / remarks / credit account
        var bagSource = new ArrayList<Map<String, Object>>();
        if (update) for (var raw : repository.read(id, "InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID")) {
            var b = copy(raw);
            bagSource.add(bagRow(i(b, "PurchaseOrderId"), i(b, "TypeId"), b.get("EmptyBagsType"), i(b, "InvGrnId"), i(b, "ItemId"), b.get("ItemName"),
                    n(b, "PurchaseQty"), n(b, "Rate"), n(b, "Amount"), b.get("Remarks"), i(b, "CreditAccountId")));
        }
        if (!newGrnIds.isEmpty()) bagSource.addAll(emptyBagsFromGrn(idList(newGrnIds)));
        var creditAccounts = repository.bagCreditAccounts().stream().map(r -> i(copy(r), "Id")).collect(Collectors.toSet());
        var bags = new ArrayList<Map<String, Object>>();
        var usedBags = Collections.newSetFromMap(new IdentityHashMap<Map<String, Object>, Boolean>());
        for (var r : list(body.get("emptyBags"))) {
            if ((int) num(r.get("ItemId")) == 0) continue;
            Map<String, Object> src = null;
            for (var b : bagSource)
                if (!usedBags.contains(b) && i(b, "InvGrnId") == (int) num(r.get("InvGrnId")) && i(b, "ItemId") == (int) num(r.get("ItemId")) && i(b, "TypeId") == (int) num(r.get("TypeId"))) { src = b; break; }
            if (src == null) throw new IllegalArgumentException("An empty-bag row does not belong to the loaded GRNs");
            usedBags.add(src);
            var b = new LinkedHashMap<String, Object>(src);
            double rate = num(r.get("Rate"));
            // grdEmptyBags_CellUpdated :792-797 recomputes Amount (rounded) only when Rate is edited.
            b.put("Rate", rate);
            b.put("Amount", rate == n(src, "Rate") ? n(src, "Amount") : awayFromZero(n(src, "PurchaseQty") * rate, digits));
            b.put("Remarks", Objects.toString(r.get("Remarks"), ""));
            int credit = (int) num(r.get("CreditAccountId"));
            if (credit != 0 && !creditAccounts.contains(credit)) throw new IllegalArgumentException("Credit account is not in the allocated list");
            b.put("CreditAccountId", credit);
            bags.add(b);
        }
        // BillAmount :1090-1161
        double bill = itemTotal + expenseTotal;
        for (var b : bags) if (n(b, "Amount") > 0 && i(b, "TypeId") == 1) bill += n(b, "Amount");
        if (commAmount > 0 && supplierId == commissionAgentId) bill += commAmount;
        if (brokerAgentId > 0 && brokeryAmount > 0 && supplierId == brokerAgentId) bill += brokeryAmount;
        if (brokeryAmount < 0) {
            if (supplierId == brokerAgentId) throw new IllegalArgumentException("Supplier And Brokery Account Can't be same!");
            if (brokerAgentId > 0) bill += brokeryAmount;
        }
        if (transportAccountId > 0 && freightAmount > 0) {
            if ("Ponch".equals(deliveryTerm)) {
                if (supplierGl == transportAccountId) throw new IllegalArgumentException("Supplier And Freight Account Can't be same in case of ponch!");
                bill -= freightAmount;
            } else if (supplierGl == transportAccountId) bill += freightAmount;
        }
        double billAmount = awayFromZero(bill, digits);
        double fcyAmount, lcyAmount;
        if (!fcyManual) { fcyAmount = itemTotal / exchangeRate; lcyAmount = itemTotal; }
        else { fcyAmount = num(body.get("fcyAmount")); lcyAmount = fcyAmount * exchangeRate; }
        if (fcyAmount == 0) throw new IllegalArgumentException("Fcy Amount Rate Field is Required");

        // ---- Insert() :2264-2294 checks
        if (commAmount > 0 && commissionAgentId == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");
        if (brokeryAmount != 0 && brokerAgentId == 0) throw new IllegalArgumentException("Please Select Brokery Account First");
        if (freightAmount > 0 && transportAccountId == 0) throw new IllegalArgumentException("Please Select Transporter Account First");
        for (var b : bags) if (i(b, "ItemId") > 0 && n(b, "PurchaseQty") > 0 && n(b, "Amount") == 0) throw new IllegalArgumentException("Packing Material Amount Required");
        // :2344-2347
        long lcyRounded = Math.round(bankers(lcyAmount, 0)), gridRounded = Math.round(bankers(itemTotal, 0));
        if (lcyRounded != gridRounded)
            throw new IllegalArgumentException("LcyAmount " + lcyRounded + " Is Not equal To Item Amount in Grid " + gridRounded + ".\n FcyAmount Should Equally Proportionate In Grid");
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not found");

        // ---- header :2295-2338 (a fresh model: everything the form does not set keeps its default)
        java.sql.Date docDate = parseDate(body.get("docDate"));
        if (docDate == null) docDate = java.sql.Date.valueOf(java.time.LocalDate.now());
        var now = new java.sql.Timestamp(System.currentTimeMillis());
        var h = writes.defaults("Sp_InvPurchaseInvoice_Insert");
        h.put("Id", update ? id : 0);
        if (commissionAgentId > 0 && commAmount > 0) {
            h.put("CommissionAgentId", commissionAgentId); h.put("CommRate", commRate); h.put("CommAmount", commAmount);
            h.put("CommissionRemarks", " Commission %:   " + PurchaseInvoiceGrnDirectFinancialRules.num(commRate));
        }
        if (brokerAgentId > 0 && brokeryAmount != 0) { h.put("BrokerAgentId", brokerAgentId); h.put("BrokeryRate", brokeryRate); h.put("BrokeryAmount", brokeryAmount); }
        if (transportAccountId > 0 && freightAmount > 0) { h.put("TransportAccountId", transportAccountId); h.put("FreightAmount", freightAmount); }
        h.put("DocDate", docDate);
        h.put("DocNo", docNo);
        h.put("DocumentTypeId", TYPE);
        h.put("ManualBillNo", Objects.toString(body.get("manualBillNo"), "").trim());
        h.put("RemarksHeader", Objects.toString(body.get("remarks"), "").trim());
        h.put("StockPartyId", update ? i(old, "StockPartyId") : 0); // hidden cmbstockprty (Designer) keeps the loaded value
        h.put("SupplierCustomerId", supplierId);
        h.put("DeliveryTerm", deliveryTerm);
        h.put("BillAmount", billAmount);
        h.put("WagesAmount", wagesHeader);
        h.put("SupplierInvoiceDate", now);
        h.put("PaymentTermsId", paymentTermId);
        h.put("DueDate", java.sql.Date.valueOf(docDate.toLocalDate().plusDays(dueDays)));
        h.put("DueDays", dueDays);
        h.put("OrganizationId", org); h.put("CompanyId", company); h.put("BranchesId", context.currentBranchId());
        h.put("FinancialYearId", context.currentFinancialYearId());
        h.put("EntryUser", context.currentUserId()); h.put("EntryDate", now); h.put("ModifyDate", now); h.put("ModifyUser", context.currentUserId());
        h.put("ScreenName", SCREEN_NAME);
        h.put("ExchangeRate", exchangeRate); h.put("CurrencyId", currencyId); h.put("FcyAmount", fcyAmount);
        h.put("AttachmentsValues", update ? old.get("AttachmentsValues") : ""); h.put("CustomAttachmentsValues", update ? old.get("CustomAttachmentsValues") : "");
        h.put("ActionId", update ? 2 : 1);

        // ---- detail rows :2348-2445 (checks in the form's nesting order)
        var details = new ArrayList<Map<String, Object>>();
        var schedules = new HashMap<Integer, List<Map<String, Object>>>();
        for (var g : rows) {
            if (i(g, "GrnDetailId") == 0) throw new IllegalArgumentException("InvGrnDetailId Required");
            if (i(g, "InvGrnId") == 0) throw new IllegalArgumentException("InvGrnId Required");
            if ((float) n(g, "GrossWeight") == 0f) throw new IllegalArgumentException("GrossWeight Required");
            if ((float) n(g, "ItemAmount") == 0f) throw new IllegalArgumentException("ItemAmount Required");
            if (s(g, "CropYear").isEmpty()) throw new IllegalArgumentException("Crop Year not Found in Detail Grid Row");
            if (i(g, "ItemId") == 0) throw new IllegalArgumentException("Item  Required");
            if ((float) n(g, "ItemQty") == 0f) throw new IllegalArgumentException("ItemQty  Required");
            if ((float) n(g, "ItemRate") == 0f) throw new IllegalArgumentException("Item Rate Required");
            if (i(g, "ItemUomId") == 0) throw new IllegalArgumentException("ItemUom Required");
            if ((float) n(g, "NetBillWeight") == 0f) throw new IllegalArgumentException("NetBillWeight Required");
            if (n(g, "StockWeight") == 0) throw new IllegalArgumentException("NetStockWeight Required");
            if (i(g, "PackingTypeId") == 0) throw new IllegalArgumentException("PackingType Required");
            int uomScheduleIdRate = 0;
            var uoms = schedules.computeIfAbsent(i(g, "ItemId"), repository::uomSchedules);
            if (!uoms.isEmpty()) {
                int equivalent = (int) Math.round(bankers(n(g, "RateUom"), 0)); // Conversion.ToInt
                var match = uoms.stream().map(PurchaseInvoiceFinancialRules::copy).filter(u -> n(u, "Equivalent") == equivalent).findFirst().orElse(null);
                if (match == null) throw new IllegalArgumentException("RateUom not found against this Item " + s(g, "ItemName"));
                uomScheduleIdRate = i(match, "Id");
            }
            if (uomScheduleIdRate == 0) throw new IllegalArgumentException("RateUOMId not found against this RateUom please check");
            var d = writes.defaults("Sp_InvPurchaseInvoiceDetail_Insert");
            d.put("Id", i(g, "Id")); d.put("PurchaseOrderDetailId", i(g, "OrderDetailId")); d.put("PurchaseOrderId", i(g, "OrderId"));
            d.put("PurchaseOrder", (int) num(g.get("OrderNo"))); d.put("InvGrnDetailId", i(g, "GrnDetailId")); d.put("InvGrnId", i(g, "InvGrnId"));
            d.put("GrnNo", (int) num(g.get("GrnNo")));
            d.put("GrossWeight", n(g, "GrossWeight")); d.put("EBWeight", n(g, "EbUnit")); d.put("EBTotalWt", n(g, "EbTotal"));
            d.put("ItemAmount", n(g, "ItemAmount")); d.put("ItemCgsRate", 0d); d.put("CropYear", s(g, "CropYear"));
            d.put("ItemId", i(g, "ItemId")); d.put("ItemName", g.get("ItemName")); d.put("ItemQty", n(g, "ItemQty")); d.put("ItemRate", n(g, "ItemRate"));
            d.put("ItemUOMId", i(g, "ItemUomId")); d.put("NetBillWeight", n(g, "NetBillWeight")); d.put("NetStockWeight", n(g, "StockWeight"));
            d.put("AdLsWeight", n(g, "AdLsWeight")); d.put("PackingTypeId", i(g, "PackingTypeId")); d.put("UomScheduleIdRate", uomScheduleIdRate);
            d.put("ExchangeRate", exchangeRate); d.put("CurrencyId", currencyId); d.put("FcyAmount", n(g, "FcyAmount"));
            d.put("WarehouseId", i(g, "WarehouseId")); d.put("WareHouseName", g.get("Warehouse")); d.put("JobLotId", i(g, "JobLotId"));
            d.put("RateCut", n(g, "RateCut")); d.put("RateCutAmount", n(g, "RateCutAmount")); d.put("FreightAmount", n(g, "Freights"));
            d.put("ExpenseAmount", n(g, "Expense")); d.put("CommissionAmount", n(g, "Commission")); d.put("WagesAmount", n(g, "WagesAmount"));
            d.put("Brokery", n(g, "Brokery")); d.put("GpNo", (int) num(g.get("GpNo"))); d.put("VehicleNo", Objects.toString(g.get("VehicleNo"), ""));
            d.put("LocationTypeId", locationTypeId);
            details.add(d);
        }
        // :2468-2483 every expense row with an item is saved, amount 0 included; empty remarks become the item name
        var expenseRows = new ArrayList<Map<String, Object>>();
        for (var e : expenses) {
            var pe = writes.defaults("Sp_InvPurchaseInvoiceExpense_Insert");
            pe.put("InvRevExpItemId", i(e, "ItemId")); pe.put("Amount", n(e, "Amount"));
            String remarks = s(e, "Remarks").trim();
            pe.put("Remarks", remarks.equals("0") || remarks.isEmpty() ? s(e, "ItemName").trim() : remarks);
            expenseRows.add(pe);
        }
        // :2484-2523
        var bagRows = new ArrayList<Map<String, Object>>();
        int rowNo = 0;
        for (var b : bags) {
            rowNo++;
            var peb = writes.defaults("Sp_InvPurchaseInvoiceEmptyBags_Insert");
            peb.put("PurchaseOrderId", (int) num(b.get("OrderId"))); peb.put("TypeId", i(b, "TypeId"));
            if (i(b, "TypeId") > 0) {
                if (i(b, "TypeId") == 1) peb.put("CreditAccountId", 0);
                else {
                    peb.put("CreditAccountId", i(b, "CreditAccountId"));
                    if (i(b, "CreditAccountId") <= 0) throw new IllegalArgumentException("Credit Account Required in Empty Bag Grid in Row#" + rowNo);
                }
            }
            peb.put("InvGrnId", i(b, "InvGrnId")); peb.put("ItemId", i(b, "ItemId")); peb.put("PurchaseQty", n(b, "PurchaseQty"));
            peb.put("Rate", n(b, "Rate")); peb.put("Amount", n(b, "Amount"));
            if (i(b, "TypeId") == 1 && n(b, "Amount") == 0) throw new IllegalArgumentException("Packing Material Amount Required");
            peb.put("Remarks", s(b, "Remarks"));
            bagRows.add(peb);
        }
        if (!update && details.stream().anyMatch(d -> i(d, "Id") > 0)) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");

        // ---- BLL Save :22 voucher first, then DAL SetData
        h.put("InvoiceQty", details.stream().mapToDouble(d -> n(d, "ItemQty")).sum());
        h.put("InvoiceWeight", details.stream().mapToDouble(d -> n(d, "NetBillWeight")).sum());
        h.put("ProjectsId", 0);
        var accounts = writes.accounts(h, details);
        var voucher = PurchaseInvoiceGrnDirectFinancialRules.calculate(h, details, bagRows, accounts);
        var attachmentChange = body.get("attachments") == null ? null : attachments.prepare(id, TYPE, body.get("attachments"));
        if (attachmentChange != null) { h.put("AttachmentsValues", attachmentChange.names()); h.put("CustomAttachmentsValues", attachmentChange.storedNames()); }
        int newId = writes.execute(update ? "Sp_InvPurchaseInvoice_Update" : "Sp_InvPurchaseInvoice_Insert", h);
        if (!update) id = newId;
        if (id <= 0) throw new IllegalStateException("Invoice procedure did not return an ID");
        h.put("Id", id);
        voucher.header().put("DocumentTypeSrNo", id); voucher.header().put("RefDocNoId", id);
        boolean wagesToProduct = flag("ContractWagesChargetoProduct");
        int line = 0;
        var detailIds = new StringBuilder();
        for (var d : details) {
            d.put("LineId", ++line);
            d.put("BillAmount", n(d, "ItemAmount") + n(d, "FreightAmount") + n(d, "ExpenseAmount") + n(d, "CommissionAmount") + n(d, "Brokery")
                    + (wagesToProduct ? n(d, "WagesAmount") : 0) - n(d, "EbPurAgainstWeightAmount") - n(d, "FreightDeduction"));
            d.put("InvPurchaseInvoiceId", id);
            int detailId = writes.execute("Sp_InvPurchaseInvoiceDetail_Insert", d);
            if (detailId <= 0) throw new IllegalStateException("Invoice detail procedure did not return an ID");
            d.put("Id", detailId);
            detailIds.append(detailId).append(',');
        }
        for (var e : expenseRows) { e.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceExpense_Insert", e); }
        for (var b : bagRows) { b.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceEmptyBags_Insert", b); }
        ProcExec.run(jdbc, "EXEC dbo.usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations @OrganizationId=?,@CompanyId=?,@Id=?,@DetailIds=?", org, company, id, detailIds.toString());
        if (attachmentChange != null) attachments.persist(id, TYPE, supplierId, attachmentChange);
        writes.post(h, voucher);
        int voucherHeadId = repository.voucherHeadId(id);
        var out = new LinkedHashMap<String, Object>();
        out.put("success", true); out.put("id", id); out.put("docNo", docNo); out.put("voucherHeadId", voucherHeadId);
        out.put("message", update ? "Record Update Successfully [" + docNo + "] " : "Record Saved Successfully [" + docNo + "] ");
        return out;
    }
}
