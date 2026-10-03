package com.mst.services;

import com.mst.repositories.GrnLoaderRepository;
import com.mst.repositories.PurchaseInvoiceAgainstGrnOrderRepository;
import com.mst.repositories.PurchaseInvoiceRecordRepository;
import com.mst.repositories.PurchaseInvoiceWriteRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.util.*;
import java.util.stream.Collectors;

import static com.mst.repositories.PurchaseInvoiceAgainstGrnOrderRepository.GRN_TYPE;
import static com.mst.repositories.PurchaseInvoiceAgainstGrnOrderRepository.SCREEN_NAME;
import static com.mst.repositories.PurchaseInvoiceAgainstGrnOrderRepository.TYPE;
import static com.mst.services.PurchaseInvoiceFinancialRules.copy;
import static com.mst.services.PurchaseInvoiceFinancialRules.i;
import static com.mst.services.PurchaseInvoiceFinancialRules.n;
import static com.mst.services.PurchaseInvoiceFinancialRules.s;

/**
 * Purchase Invoice Against GRN Order - desktop Architecture.WinApp.Purchase.PurchaseInvoiceAgainstGrnOrder
 * (ScreenDefinition 132, DocumentTypeId 172, GRN loader frmLoadGRN with DocumentTypeId 169). Line numbers below are that file.
 *
 * Save chain (btnSave_Click :2979 / btnUpdate_Click :2563 -> Insert :2583 -> BLL 0581 Save -> PurchaseInvoiceFinancial
 * .MakeVoucherForPurchaseInvoice (0549) with the 172 branches of 0614 -> DAL 0434 SetData):
 * Sp_InvPurchaseInvoice_Insert/_Update, Sp_InvPurchaseInvoiceDetail_Insert per row, Sp_InvPurchaseInvoiceFreight_Insert,
 * Sp_InvPurchaseInvoiceJournal_Insert, Sp_InvPurchaseInvoiceExpense_Insert, Sp_InvPurchaseInvoiceEmptyBags_Insert,
 * usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations, DMS attachments, then (PurchaseInvoiceWriteRepository.post)
 * usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId, Sp_InventoryStockEvalautionDetail_Update,
 * Sp_VoucherHead_Insert/_Update + Sp_VoucherDetail_Insert, USP_VoucherBalanceCheck, voucher history,
 * DAW.USp_DocumentApprovalDetail_Insert. One transaction.
 */
@Service
public class PurchaseInvoiceAgainstGrnOrderService {

    private final PurchaseInvoiceAgainstGrnOrderRepository repository;
    private final PurchaseInvoiceRecordRepository records;
    private final PurchaseInvoiceWriteRepository writes;
    private final PurchaseInvoiceAttachmentService attachments;
    private final GrnLoaderRepository grnLoader;
    private final CurrentUserContext context;
    private final JdbcTemplate jdbc;

    public PurchaseInvoiceAgainstGrnOrderService(PurchaseInvoiceAgainstGrnOrderRepository repository, PurchaseInvoiceRecordRepository records,
                                                 PurchaseInvoiceWriteRepository writes, PurchaseInvoiceAttachmentService attachments,
                                                 GrnLoaderRepository grnLoader, CurrentUserContext context, JdbcTemplate jdbc) {
        this.repository = repository; this.records = records; this.writes = writes; this.attachments = attachments;
        this.grnLoader = grnLoader; this.context = context; this.jdbc = jdbc;
    }

    // ------------------------------------------------------------------------------------------------ helpers
    private String config(String name) { return writes.configuration(context.currentOrganizationId(), context.currentCompanyId(), name); }
    /** Load :339-348 - Conversion.ToBool of the configuration text when it is not empty. */
    private boolean flag(String name) { return Boolean.parseBoolean(config(name).trim()); }
    private static boolean blank(Object v) { return v == null || v.toString().trim().isEmpty(); }
    private static double num(Object v) {
        if (blank(v)) return 0;
        if (v instanceof Number x) return x.doubleValue();
        try { return Double.parseDouble(v.toString().trim().replace(",", "")); } catch (NumberFormatException e) { return 0; }
    }
    /** Conversion.ToInt: Convert.ToInt32 of the value (midpoint to even). */
    private static int toInt(Object v) { return (int) Math.rint(num(v)); }
    private static String text(Object v) { return v == null ? "" : v.toString(); }
    /** Control.Text = value.ToString(): a number the way .NET prints it (no trailing ".0"). */
    private static String numText(Object v) { return v instanceof Number x ? cs(x.doubleValue()) : text(v); }
    private static double bankers(double v) { return BigDecimal.valueOf(v).setScale(0, RoundingMode.HALF_EVEN).doubleValue(); }
    private static double awayFromZero(double v) { return BigDecimal.valueOf(v).setScale(0, RoundingMode.HALF_UP).doubleValue(); }
    /** C# double.ToString() (15 significant digits, no trailing zeros). */
    static String cs(double v) {
        if (v == 0) return "0";
        return new BigDecimal(v).round(new MathContext(15)).stripTrailingZeros().toPlainString();
    }
    private static String dateText(Object v) { return v == null ? "" : v.toString().length() >= 10 ? v.toString().substring(0, 10) : v.toString(); }
    private static Object jsonValue(Object v) {
        if (v instanceof java.sql.Timestamp t) return t.toLocalDateTime().toString().replace('T', ' ');
        if (v instanceof java.util.Date d) return new java.sql.Timestamp(d.getTime()).toLocalDateTime().toString().replace('T', ' ');
        return v;
    }
    private static String idList(Collection<Integer> ids) { return ids.stream().map(x -> "," + x).collect(Collectors.joining()); }
    private static boolean approved(Map<String, Object> row) { return Boolean.TRUE.equals(row.get("IsApproved")) || "1".equals(Objects.toString(row.get("IsApproved"))); }

    private Map<String, Boolean> rights() {
        var r = new LinkedHashMap<String, Boolean>();
        for (String name : List.of("View", "Save", "Update", "Delete", "Print", "CanView AllRecord")) r.put(name.replace(" ", ""), records.hasRight(TYPE, name));
        return r;
    }

    private List<Map<String, Object>> suppliers() {
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : repository.suppliers(repository.subsidiaryFeature())) {
            var r = copy(raw);
            var m = new LinkedHashMap<String, Object>();
            m.put("Id", i(r, "Id")); m.put("CompanyName", r.get("CompanyName")); m.put("GlAccountId", i(r, "GlAccountId"));
            out.add(m);
        }
        return out;
    }
    private List<Map<String, Object>> pick(List<Map<String, Object>> rows, String name) {
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : rows) { var r = copy(raw); var m = new LinkedHashMap<String, Object>(); m.put("Id", i(r, "Id")); m.put(name, r.get(name)); out.add(m); }
        return out;
    }

    // ------------------------------------------------------------------------------------------------ form load
    /** InvfrmPurchaseInvoice_Load :335-475 (the Load handler keeps the type-56 name). */
    public Map<String, Object> init() {
        records.requireRight(TYPE, "View");
        var out = new LinkedHashMap<String, Object>();
        out.put("rights", rights());
        // DocumentNo :1641 shows "Max Number Not Found" and the form still opens.
        int code = repository.nextDocNo();
        out.put("docNo", code > 0 ? code : "");
        if (code <= 0) out.put("docNoMessage", "Max Number Not Found");
        out.putAll(lookups());
        var cfg = new LinkedHashMap<String, Object>();
        cfg.put("weightAddLessOnPurchaseInvoice", flag("WeightAddLessOnPurchaseInvoice"));
        cfg.put("rateEditableInGatePurchase", flag("RateEditableOnPurchaseInvoice_InGatePurchase"));
        cfg.put("acceptAccessWtHold", flag("AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt"));
        cfg.put("currentBranchId", context.currentBranchId());
        out.put("configuration", cfg);
        return out;
    }

    /** btnFrmRefresh_Click :3206 - Other items, accounts, branches, projects and suppliers again. */
    public Map<String, Object> refresh() { records.requireRight(TYPE, "View"); return lookups(); }

    private Map<String, Object> lookups() {
        var out = new LinkedHashMap<String, Object>();
        out.put("branches", pick(repository.branches(), "BranchName"));
        out.put("projects", pick(repository.projects(), "ProjectName"));
        out.put("suppliers", suppliers());
        out.put("accounts", repository.accounts());
        out.put("otherItems", pick(repository.otherItems(), "OtherItemName"));
        return out;
    }

    /** DocumentNo :1641 - "Max Number Not Found" when 0. */
    public int nextDocNo() {
        records.requireRight(TYPE, "View");
        int code = repository.nextDocNo();
        if (code <= 0) throw new IllegalArgumentException("Max Number Not Found");
        return code;
    }

    // ------------------------------------------------------------------------------------------------ grid rows
    /** LoadDataDetailGridAgainstGP :1924-1930 - one dtGrid row from a usp_GrnLoadForPurchaseInvoice row, then
     *  RateCutAmountCalculations :1082 (only rows whose RateCut is not 0 are re-rated). */
    private static Map<String, Object> gridRowFromGrn(Map<String, Object> raw, int invoiceTypeId) {
        var r = copy(raw);
        double rateUom = invoiceTypeId != 2 ? n(r, "EquivalentPoRate") : 40.0;
        var g = new LinkedHashMap<String, Object>();
        g.put("Id", 0); g.put("InvGrnDetailId", i(r, "InvGrnDetailId")); g.put("InvGrnId", i(r, "InvGrnId"));
        g.put("ItemId", i(r, "ItemId")); g.put("ItemName", r.get("ItemName")); g.put("CropYear", r.get("CropYear"));
        g.put("ItemUomId", i(r, "ItemUOMId")); g.put("UOMCodeItem", r.get("UOMCodeItem")); g.put("ItemQty", n(r, "ItemQty"));
        g.put("WarehouseId", i(r, "WarehouseId")); g.put("WareHouseName", r.get("WareHouseName")); g.put("PackingTypeId", i(r, "PackingTypeId"));
        g.put("LabReportRef", text(r.get("LabAnalisysNo"))); g.put("PurchaseOrderId", i(r, "PurchaseOrderId")); g.put("PurchaseOrder", r.get("PurchaseOrder"));
        g.put("GrossWeight", n(r, "GrossWeight")); g.put("EbTotal", n(r, "EBTotalWt")); g.put("WtCutTotal", n(r, "WeightCutTotal"));
        g.put("AdLsWeight", n(r, "AdLsWeight")); g.put("NetBillWeight", n(r, "NetBillWeight")); g.put("StockWeight", n(r, "NetStockWeight"));
        g.put("OrderItemRate", n(r, "ItemRate")); g.put("OrderItemRateUOMId", i(r, "UomScheduleIdRate")); g.put("EquivalentPoRate", rateUom);
        g.put("ItemAmount", n(r, "ItemAmount")); g.put("JobLotId", i(r, "JobLotId")); g.put("RateCut", n(r, "DeductionRate"));
        for (String k : List.of("RateCutAmount", "BillAmount", "Freights", "Expense", "Journal", "Commission")) g.put(k, 0d);
        g.put("GpNo", r.get("GpNo")); g.put("VehicleNo", r.get("VehicleNo"));
        if (num(g.get("RateCut")) != 0) {
            double net = num(g.get("NetBillWeight")), eq = num(g.get("EquivalentPoRate"));
            double cut = net / eq * num(g.get("RateCut"));
            g.put("RateCutAmount", cut);
            g.put("ItemAmount", net / eq * num(g.get("OrderItemRate")) - cut);
        }
        return g;
    }

    /** ReadById :2495 - one dtGrid row from a stored PurchaseDetailReadByInvPurchaseInvoiceId row. */
    private static Map<String, Object> gridRowFromStored(Map<String, Object> raw) {
        var d = copy(raw);
        var g = new LinkedHashMap<String, Object>();
        g.put("Id", i(d, "Id")); g.put("InvGrnDetailId", i(d, "InvGrnDetailId")); g.put("InvGrnId", i(d, "InvGrnId"));
        g.put("ItemId", i(d, "ItemId")); g.put("ItemName", d.get("ItemName")); g.put("CropYear", d.get("CropYear"));
        g.put("ItemUomId", i(d, "ItemUOMId")); g.put("UOMCodeItem", d.get("UOMCodeItem")); g.put("ItemQty", n(d, "ItemQty"));
        g.put("WarehouseId", i(d, "WarehouseId")); g.put("WareHouseName", d.get("WareHouseName")); g.put("PackingTypeId", i(d, "PackingTypeId"));
        g.put("LabReportRef", text(d.get("LabAnalisysNo"))); g.put("PurchaseOrderId", i(d, "PurchaseOrderId")); g.put("PurchaseOrder", d.get("PurchaseOrder"));
        g.put("GrossWeight", n(d, "GrossWeight")); g.put("EbTotal", n(d, "EBTotalWt")); g.put("WtCutTotal", n(d, "WeightCutTotal"));
        g.put("AdLsWeight", n(d, "AdLsWeight")); g.put("NetBillWeight", n(d, "NetBillWeight")); g.put("StockWeight", n(d, "NetStockWeight"));
        g.put("OrderItemRate", n(d, "ItemRate")); g.put("OrderItemRateUOMId", i(d, "UomScheduleIdRate")); g.put("EquivalentPoRate", n(d, "EquivalentPoRate"));
        g.put("ItemAmount", n(d, "ItemAmount")); g.put("JobLotId", i(d, "JobLotId")); g.put("RateCut", n(d, "RateCut"));
        g.put("RateCutAmount", n(d, "RateCutAmount")); g.put("BillAmount", n(d, "BillAmount")); g.put("Freights", n(d, "FreightAmount"));
        g.put("Expense", n(d, "ExpenseAmount")); g.put("Journal", n(d, "JournalAmount")); g.put("Commission", n(d, "CommissionAmount"));
        g.put("GpNo", d.get("GpNo")); g.put("VehicleNo", d.get("VehicleNo"));
        return g;
    }

    private static Map<String, Object> freightRow(Object orderId, Object grnId, Object freightAcId, Object transporter, Object percentage,
                                                  Object qty, Object rate, Object freight, Object debit, Object freightAmount, Object remarks) {
        var m = new LinkedHashMap<String, Object>();
        m.put("OrderId", orderId); m.put("InvGrnId", grnId); m.put("FreightAcId", freightAcId); m.put("Transporter", transporter);
        m.put("Percentage", percentage); m.put("Qty", qty); m.put("Rate", rate); m.put("Freight", freight); m.put("Debit", debit);
        m.put("FreightAmount", freightAmount); m.put("Remarks", remarks);
        return m;
    }
    private static Map<String, Object> ledgerRow(Object grnId, Object freightAcId, Object accountId, Object remarks, Object percentage,
                                                 Object qty, Object rate, Object debit, Object credit, Object freightAmount) {
        var m = new LinkedHashMap<String, Object>();
        m.put("InvGrnId", grnId); m.put("FreightAcId", freightAcId); m.put("AccountId", accountId); m.put("Remarks", remarks);
        m.put("Percentage", percentage); m.put("Qty", qty); m.put("Rate", rate); m.put("Debit", debit); m.put("Credit", credit);
        m.put("FreightAmount", freightAmount);
        return m;
    }
    private static Map<String, Object> bagRow(Object orderId, Object typeId, Object type, Object grnId, Object itemId, Object itemName,
                                              double qty, double rate, double amount, Object remarks) {
        var m = new LinkedHashMap<String, Object>();
        m.put("OrderId", orderId); m.put("TypeId", typeId); m.put("Type", type); m.put("InvGrnId", grnId); m.put("ItemId", itemId);
        m.put("ItemName", itemName); m.put("PurchaseQty", qty); m.put("Rate", rate); m.put("Amount", amount); m.put("Remarks", remarks);
        return m;
    }

    /** LoadEmptyBagsData :2020 - rows with (int)PurchaseQty &gt; 0; Amount = PurchaseQty * Rate. */
    private List<Map<String, Object>> emptyBagsFromGrn(String ids) {
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : repository.grnEmptyBags(ids)) {
            var r = copy(raw);
            if (toInt(r.get("PurchaseQty")) <= 0) continue;
            out.add(bagRow(i(r, "PurchaseOrderId"), i(r, "TypeId"), r.get("EmptyBagsType"), i(r, "InvGrnId"), i(r, "ItemId"), r.get("ItemName"),
                    n(r, "PurchaseQty"), n(r, "Rate"), n(r, "PurchaseQty") * n(r, "Rate"), r.get("Remarks")));
        }
        return out;
    }

    /** InvGrn transporters read by LoadFreightData for the given GRNs (accounts the freight / JL grids may carry besides the list). */
    private Set<Integer> grnTransporters(String ids) {
        var out = new HashSet<Integer>();
        if (!ids.isEmpty()) for (var raw : repository.grnFreight(ids)) out.add(i(copy(raw), "Transporter"));
        return out;
    }

    /**
     * LoadFreightData :1939 and LoadPurchaseOrderExpensesChargToProductData :1986. The second one adds 9 values to the
     * 11-column dtFreight, so every value lands one or more columns to the LEFT of its name (desktop defect, reproduced):
     * OrderId=PurchaseOrderId, InvGrnId=GRNID, FreightAcId=AccountId, Transporter=Percentage, Percentage=Qty, Qty=Rate,
     * Rate=round(amount), Freight(Credit)=0, Debit=Remarks. A Remarks text that is not a number cannot be stored in the
     * double Debit column: the row is refused and the loop stops (earlier rows stay), as the DataTable does.
     */
    private Map<String, Object> freightAndLedger(String ids, String deliveryTerm, int poId, int grnId, double itemTotal, double qtyTotal) {
        var freight = new ArrayList<Map<String, Object>>();
        var ledger = new ArrayList<Map<String, Object>>();
        String message = null;
        for (var raw : repository.grnFreight(ids)) {
            var r = copy(raw);
            if (toInt(r.get("CarriageAmount")) <= 0) continue;
            String remarks = "";
            if (toInt(r.get("GpNo")) > 0) remarks += "GpNo: " + text(r.get("GpNo"));
            if (!text(r.get("VehicleNo")).isEmpty()) remarks += "   VehicleNo:  " + text(r.get("VehicleNo"));
            if (!text(r.get("BiltyNo")).isEmpty()) remarks += "   BiltyNo: " + text(r.get("BiltyNo"));
            double carriage = n(r, "CarriageAmount");
            switch (deliveryTerm) {
                case "Load", "Load & PartyWeight", "Load & FactoryWeight" ->
                        freight.add(freightRow(poId, i(r, "MainId"), i(r, "Transporter"), i(r, "Transporter"), 0d, 0d, 0d, carriage, 0d, carriage, remarks));
                default -> ledger.add(ledgerRow(i(r, "MainId"), i(r, "Transporter"), i(r, "Transporter"), remarks, 0d, 0d, 0d, 0d, carriage, carriage));
            }
        }
        for (var raw : repository.orderChargesToProduct(poId)) {
            var r = copy(raw);
            if (i(r, "AccountId") <= 0) continue;
            double amount = n(r, "Percentage") > 0 && itemTotal > 0 ? itemTotal * n(r, "Percentage") / 100.0 : qtyTotal * n(r, "Rate");
            Object qty = n(r, "Percentage") > 0 && itemTotal > 0 ? n(r, "Qty") : qtyTotal;
            Object remarks = r.get("Remarks");
            double debit;
            if (blank(remarks)) debit = 0;
            else {
                try { debit = Double.parseDouble(remarks.toString().trim()); }
                catch (NumberFormatException e) {
                    message = "Couldn't store <" + remarks + "> in Debit Column.  Expected type is Double.";
                    break;
                }
            }
            freight.add(freightRow(i(r, "PurchaseOrderId"), grnId, i(r, "AccountId"), toInt(r.get("Percentage")), qty, n(r, "Rate"), bankers(amount), 0d, debit, 0d, ""));
        }
        var out = new LinkedHashMap<String, Object>();
        out.put("freight", freight); out.put("ledger", ledger);
        if (message != null) out.put("chargeMessage", message);
        return out;
    }

    // ------------------------------------------------------------------------------------------------ GRN load
    private static final String APPROVAL_MESSAGE = "Approval is required because Bill weight exceeds the stock weight OR Empty Bags Deduction equal to zero";
    private static final String APPROVAL_MESSAGE_HOLD = "Approval is required because \nBill weight exceeds Balance Order Weight \n                           OR\nBill weight exceeds stock Weight \n                           OR\nEmpty Bags Deduction equal to zero";

    /** frmLoadGRN.btnLoadOnInvoice_Click_1 :496-624 for DocumentTypeId 169: the checked rows (grid order) re-read from the
     *  pending list, the same refusals in the same order and wording, and the GrnType of each accepted row. Returns the
     *  accepted GRN ids (rows with supplier 0 are skipped) and the last GrnType (LoadInGridDetail :2083). */
    private Map<String, Object> loaderSelection(List<Integer> grnIds) {
        if (grnIds.isEmpty()) throw new IllegalArgumentException("Check the row first");
        var supplied = new ArrayList<Map<String, Object>>();
        for (int id : grnIds) supplied.add(Map.of("Id", id));
        var rows = grnLoader.selectionRows(supplied, GRN_TYPE);
        boolean hold = flag("AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt");
        int sID = 0, sOrderId = 1, refId = 0, grnType = 1;
        String deliveryTerm = "";
        var accepted = new ArrayList<Integer>();
        for (var raw : rows) {
            var r = copy(raw);
            int supplier = i(r, "SupplierCustomerId"), orderId = i(r, "PurchaseOrderId"), refDocumentTypeId = i(r, "RefDocumentTypeId");
            String dTerm = refDocumentTypeId != 46 ? text(r.get("DeliveryTerm")) : "";
            if (i(r, "GrnStatusId") == 1) throw new IllegalArgumentException(hold ? APPROVAL_MESSAGE_HOLD : APPROVAL_MESSAGE);
            if (supplier == 0) continue;
            if (sID == 0) sID = supplier;
            if (refId == 0) refId = refDocumentTypeId;
            if (sOrderId == 1) sOrderId = orderId;
            if (deliveryTerm.isEmpty()) deliveryTerm = dTerm;
            if (sID != supplier) throw new IllegalArgumentException("Sorry the Grn should be of same Supplier");
            if (refId != refDocumentTypeId) throw new IllegalArgumentException("Sorry the Grn should be of same PurchaseAgainst type");
            if (refDocumentTypeId != 46 && !deliveryTerm.equals(dTerm)) throw new IllegalArgumentException("Sorry the Grn should be of same Delivery Term");
            if (sOrderId != orderId) throw new IllegalArgumentException("Sorry the Grn Not should be of same PurchaseOrder");
            switch (text(r.get("PurchaseAgainst"))) {
                case "PurchaseOrder" -> grnType = 1;
                case "Market Purchase" -> grnType = 2;
                case "Gate Purchase" -> grnType = 3;
                case "Purchase From Party Processing" -> grnType = 4;
                default -> { }
            }
            accepted.add(i(r, "Id"));
            sID = supplier; sOrderId = orderId; refId = refDocumentTypeId;
        }
        return Map.of("ids", accepted, "invoiceTypeId", grnType);
    }

    /** txtGrnNo_Leave :3384-3422 - GetGRNIdByDocNo (type 169) and the RefDocumentTypeId -&gt; invoice type mapping
     *  (41 -&gt; 1, 105 -&gt; 2, 106 -&gt; 3, anything else kept as it is). */
    private Map<String, Object> grnNoSelection(int grnNo) {
        var rows = repository.grnIdByDocNo(grnNo);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Loader");
        var r = copy(rows.get(0));
        int type = i(r, "RefDocumentTypeId");
        // :3397 also compares the form's own InvoiceTypeId field with 172, which it never holds.
        if (type == 41) type = 1;
        else if (type == 105) type = 2;
        else if (type == 106) type = 3;
        return Map.of("ids", List.of(i(r, "Id")), "invoiceTypeId", type);
    }

    /**
     * Load GRN (toolStripButton3_Click_1 :3107 -&gt; frmLoadGRN 169 -&gt; LoadInGridDetail :2066) or Grn No leave
     * (txtGrnNo_Leave :3384 -&gt; LoadInGridDetail(GrnId, type)). The grid is REPLACED (dtGrid.Rows.Clear), as are the
     * Charge-to-Product, Supplier Add/Less, Other Expense and Empty Bag grids.
     */
    @SuppressWarnings("unchecked")
    public Map<String, Object> loadGrns(Map<String, Object> body) {
        records.requireRight(TYPE, "View");
        Map<String, Object> selection;
        if (!blank(body.get("grnNo"))) {
            int grnNo = toInt(body.get("grnNo"));
            if (grnNo <= 0) throw new IllegalArgumentException("Record Not Found For Loader");
            selection = grnNoSelection(grnNo);
        } else {
            var ids = new ArrayList<Integer>();
            if (body.get("grnIds") instanceof List<?> list) for (Object o : list) { int id = toInt(o); if (id > 0 && !ids.contains(id)) ids.add(id); }
            selection = loaderSelection(ids);
        }
        var grnIds = (List<Integer>) selection.get("ids");
        int invoiceTypeId = (Integer) selection.get("invoiceTypeId");
        return loadInGridDetail(grnIds, invoiceTypeId, Objects.toString(body.get("deliveryTerm"), ""));
    }

    private Map<String, Object> loadInGridDetail(List<Integer> grnIds, int invoiceTypeId, String currentDeliveryTerm) {
        String ids = idList(grnIds);
        var out = new LinkedHashMap<String, Object>();
        out.put("invoiceTypeId", invoiceTypeId);
        var grn = ids.isEmpty() ? List.<Map<String, Object>>of() : repository.grnRows(ids);
        var rows = new ArrayList<Map<String, Object>>();
        String deliveryTerm = currentDeliveryTerm;
        int poId = 0, grnId = 0;
        if (!grn.isEmpty()) {
            var f = copy(grn.get(0));
            var h = new LinkedHashMap<String, Object>();
            h.put("SupplierCustomerId", i(f, "SupplierCustomerId"));
            h.put("CommissionAgentId", i(f, "BrokerAgentSupCustId"));
            h.put("CommissionType", text(f.get("CommissionType")));
            h.put("CommRate", numText(f.get("CommRate")));
            h.put("UomScheduleIdCmRate", text(f.get("UomScheduleIdCmRate")));
            h.put("CommAmount", numText(f.get("CommAmount")));
            h.put("CommissionRemarks", text(f.get("CommissionRemarks")));
            h.put("RemarksHeader", text(f.get("RemarksHeader")));
            h.put("BrokerAgentId", i(f, "BrokerAgentId"));
            h.put("BrokeryType", text(f.get("BrokeryType")));
            h.put("BrokeryUom", numText(f.get("BrokeryUom")));
            h.put("BrokeryRate", numText(f.get("BrokeryRate")));
            h.put("BrokeryAmount", numText(f.get("BrokeryAmount")));
            h.put("DeliveryTerm", text(f.get("DeliveryTerm")));
            h.put("DueDays", numText(f.get("OrderDueDays")));
            out.put("header", h);
            deliveryTerm = text(f.get("DeliveryTerm"));
            grnId = i(f, "InvGrnId");
            for (var raw : grn) rows.add(gridRowFromGrn(raw, invoiceTypeId));
            poId = i(rows.get(0), "PurchaseOrderId"); // POID = grd.CurrentRow (first row) :1928
        }
        out.put("rows", rows);
        double itemTotal = rows.stream().mapToDouble(r -> num(r.get("ItemAmount"))).sum();
        double qtyTotal = rows.stream().mapToDouble(r -> num(r.get("ItemQty"))).sum();
        out.putAll(freightAndLedger(ids.isEmpty() ? "" : ids, deliveryTerm, poId, grnId, itemTotal, qtyTotal));
        var expenses = new ArrayList<Map<String, Object>>();
        if (!ids.isEmpty()) for (var raw : repository.grnBagPrices(ids)) {
            var r = copy(raw);
            if (toInt(r.get("BagPrice")) <= 0) continue;
            var e = new LinkedHashMap<String, Object>();
            e.put("ItemId", 0); e.put("Qty", n(r, "ItemQty")); e.put("Rate", n(r, "BagPrice")); e.put("Amount", n(r, "ItemQty") * n(r, "BagPrice")); e.put("Remarks", "0");
            expenses.add(e);
        }
        out.put("expenses", expenses);
        out.put("emptyBags", ids.isEmpty() ? List.of() : emptyBagsFromGrn(ids));
        return out;
    }

    // ------------------------------------------------------------------------------------------------ read
    /** ReadById :2455-2561. */
    public Map<String, Object> getById(int id) {
        var stored = records.require(id, TYPE);
        var h = copy(repository.header(id));
        var out = new LinkedHashMap<String, Object>();
        out.put("Id", id);
        for (String k : List.of("BranchesId", "ProjectsId", "SupplierCustomerId", "StockPartyId", "CommissionAgentId", "BrokerAgentId", "DueDays", "DocumentTypeSrNo", "InvoiceTypeId"))
            out.put(k, i(h, k));
        out.put("DocNo", h.get("DocNo"));
        out.put("DocDate", dateText(h.get("DocDate")));
        out.put("DueDate", dateText(h.get("DueDate")));
        for (String k : List.of("ManualBillNo", "CommissionType", "UomScheduleIdCmRate", "CommissionRemarks", "BrokeryType", "RemarksHeader", "DeliveryTerm"))
            out.put(k, text(h.get(k)));
        out.put("CommRate", cs(n(h, "CommRate"))); out.put("CommAmount", cs(n(h, "CommAmount")));
        out.put("BrokeryRate", cs(n(h, "BrokeryRate"))); out.put("BrokeryAmount", cs(n(h, "BrokeryAmount"))); out.put("BrokeryUom", cs(n(h, "BrokeryUom")));
        out.put("BillAmount", n(h, "BillAmount"));
        out.put("IsApproved", approved(stored));
        out.put("VoucherHeadId", repository.voucherHeadId(id));
        var details = new ArrayList<Map<String, Object>>();
        for (var raw : repository.read(id, "PurchaseDetailReadByInvPurchaseInvoiceId")) details.add(gridRowFromStored(raw));
        out.put("details", details);
        var expenses = new ArrayList<Map<String, Object>>();
        for (var raw : repository.read(id, "InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID")) {
            var e = copy(raw);
            var m = new LinkedHashMap<String, Object>();
            m.put("ItemId", i(e, "InvRevExpItemId")); m.put("Qty", n(e, "Qty")); m.put("Rate", n(e, "Rate")); m.put("Amount", n(e, "Amount")); m.put("Remarks", text(e.get("Remarks")));
            expenses.add(m);
        }
        out.put("expenses", expenses);
        var ledger = new ArrayList<Map<String, Object>>();
        for (var raw : repository.read(id, "InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID")) {
            var j = copy(raw);
            ledger.add(ledgerRow(i(j, "InvGrnId"), i(j, "ChartofAccountId"), i(j, "ChartofAccountId"), text(j.get("JvRemarks")), n(j, "JvPrcnt"),
                    n(j, "JvQty"), n(j, "JvRate"), n(j, "JvDebit"), n(j, "JvCredit"), n(j, "JvCredit")));
        }
        out.put("ledger", ledger);
        var freight = new ArrayList<Map<String, Object>>();
        for (var raw : repository.read(id, "InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID")) {
            var f = copy(raw);
            freight.add(freightRow(i(f, "PurchaseOrderId"), i(f, "InvGrnId"), i(f, "TansporterId"), i(f, "TansporterId"), n(f, "Percentage"),
                    n(f, "FrQty"), n(f, "FrRate"), bankers(n(f, "FreightAmount")), n(f, "Debit"), n(f, "FreightAmount"), text(f.get("Remarks"))));
        }
        out.put("freight", freight);
        var bags = new ArrayList<Map<String, Object>>();
        for (var raw : repository.read(id, "InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID")) {
            var b = copy(raw);
            bags.add(bagRow(i(b, "PurchaseOrderId"), i(b, "TypeId"), b.get("EmptyBagsType"), i(b, "InvGrnId"), i(b, "ItemId"), b.get("ItemName"),
                    n(b, "PurchaseQty"), n(b, "Rate"), n(b, "Amount"), text(b.get("Remarks"))));
        }
        out.put("emptyBags", bags);
        out.put("rights", rights());
        return out;
    }

    /** GetAll :2292 + HistoryGridSettings :2314 - every FormHistory column (Id, VoucherHeadId, RecordNo, DocumentTypeId hidden
     *  by the page). tabControl1_SelectedIndexChanged :3135 asks for 50 records, LoadAll :3194 for all (0). */
    public List<Map<String, Object>> history(int noOfRecords) {
        records.requireRight(TYPE, "View");
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : repository.history(records.hasRight(TYPE, "CanView AllRecord"), Math.max(noOfRecords, 0))) {
            var m = new LinkedHashMap<String, Object>();
            for (var e : raw.entrySet()) m.put(e.getKey(), jsonValue(e.getValue()));
            out.add(m);
        }
        return out;
    }

    /** grdHistory "Detail" button -&gt; GetDetailGrdByHeadId :1226 (23 columns from GetByID's detail list). */
    public List<Map<String, Object>> historyDetail(int id) {
        records.require(id, TYPE);
        var out = new ArrayList<Map<String, Object>>();
        for (var raw : repository.read(id, "PurchaseDetailReadByInvPurchaseInvoiceId")) {
            var d = copy(raw);
            var m = new LinkedHashMap<String, Object>();
            m.put("PurchaseOrder", d.get("PurchaseOrder")); m.put("WareHouseName", d.get("WareHouseName")); m.put("ItemName", d.get("ItemName"));
            m.put("PackType", d.get("PackTypeDesc")); m.put("ItemUOM", d.get("UOMCodeItem")); m.put("CropYear", d.get("CropYear"));
            m.put("JobLot", d.get("JobLotDescription")); m.put("GpNo", d.get("GpNo")); m.put("VehicleNo", d.get("VehicleNo"));
            m.put("ItemQty", n(d, "ItemQty")); m.put("GrossWeight", n(d, "GrossWeight")); m.put("EBTotalWt", n(d, "EBTotalWt"));
            m.put("WeightCutTotal", n(d, "WeightCutTotal")); m.put("NetBillWeight", n(d, "NetBillWeight")); m.put("NetStockWeight", n(d, "NetStockWeight"));
            m.put("ItemRate", n(d, "ItemRate")); m.put("RateUOM", d.get("EquivalentPoRate")); m.put("ItemAmount", n(d, "ItemAmount"));
            m.put("RateCut", n(d, "RateCut")); m.put("RateCutAmount", n(d, "RateCutAmount")); m.put("CommissionAmount", n(d, "CommissionAmount"));
            m.put("ExpenseAmount", n(d, "ExpenseAmount")); m.put("FreightAmount", n(d, "FreightAmount"));
            out.add(m);
        }
        return out;
    }

    // ------------------------------------------------------------------------------------------------ calculations
    /** TotalCommissionAmount :1671 / TotalBrokeryAmount :1731. rate text empty -&gt; 0; Flat -&gt; rate.ToString("0,0");
     *  Percent -&gt; Math.Round(ItemAmount total * rate / 100); Comm Weight -&gt; Math.Round(NetBillWeight total / UOM * rate);
     *  any other type text leaves the box as it was (typed). */
    static double chargeAmount(String rateText, String type, String uomText, double itemTotal, double netTotal, double previous) {
        if (rateText == null || rateText.isEmpty()) return 0;
        double rate = num(rateText);
        switch (type == null ? "" : type) {
            case "Flat": return awayFromZero(rate);
            case "Percent": return bankers(itemTotal * rate / 100.0);
            case "Comm Weight": { double uom = num(uomText); return uom == 0 ? 0 : bankers(netTotal / uom * rate); }
            default: return previous;
        }
    }

    // ------------------------------------------------------------------------------------------------ save
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        var out = new ArrayList<Map<String, Object>>();
        if (v instanceof List<?> l) for (Object o : l) if (o instanceof Map<?, ?> m) out.add(copy((Map<String, Object>) m));
        return out;
    }

    /**
     * btnSave_Click :2979 / btnUpdate_Click :2563 -&gt; Insert() :2583-2977. The browser supplies what the operator types or
     * edits: header boxes and combos, the editable detail cells (ItemRate / RateUOM / RateCut / AdLsWeight), the Charge to
     * Product, Supplier Add/Less and Other Expense grids, and the empty-bag rate / remarks. Every GRN value is re-read here and
     * every derived amount (net weight, rate-cut amount, item amount, commission, brokery, proportions, bill amount) is
     * recomputed with the form's own formulas.
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        int id = toInt(body.get("id"));
        boolean update = id > 0;
        if (id < 0) throw new IllegalArgumentException("Record Not Update beacause Record Id not found");
        records.requireRight(TYPE, update ? "Update" : "Save");
        Map<String, Object> old = copy(null);
        if (update) {
            old = copy(records.require(id, TYPE));
            if (approved(old)) throw new IllegalArgumentException("Record Not Update beacause Record has approved");
        }
        int org = context.currentOrganizationId(), company = context.currentCompanyId();
        boolean weightAddLess = flag("WeightAddLessOnPurchaseInvoice");
        boolean rateEditGatePurchase = flag("RateEditableOnPurchaseInvoice_InGatePurchase");

        // ---- FormValidation :1453-1480 (same order and wording)
        int branchId = toInt(body.get("branchId"));
        if (branchId <= 0 || repository.branches().stream().noneMatch(r -> i(copy(r), "Id") == branchId)) throw new IllegalArgumentException("Branch Name is Required");
        // Tenancy: an invoice is written to the signed-in branch only (the desktop combo can pick any branch of the company).
        if (branchId != context.currentBranchId()) throw new IllegalArgumentException("Purchase invoices can only be saved in your current branch. Select your current branch.");
        int projectId = toInt(body.get("projectId"));
        if (projectId <= 0 || repository.projects().stream().noneMatch(r -> i(copy(r), "Id") == projectId)) throw new IllegalArgumentException("Project Name is Required");
        var supplierRows = suppliers();
        int supplierId = toInt(body.get("supplierId"));
        var supplier = supplierRows.stream().filter(r -> i(r, "Id") == supplierId).findFirst().orElse(null);
        if (supplierId <= 0 || supplier == null) throw new IllegalArgumentException("Supplier Name is Required");
        int docNo = update ? i(old, "DocNo") : repository.nextDocNo();
        if (docNo <= 0) throw new IllegalArgumentException("DocNo is Required");
        int supplierGl = i(supplier, "GlAccountId");

        // ---- detail rows: GRN / stored values from the database, editable cells from the browser
        var suppliedRows = list(body.get("details"));
        var storedDetails = new LinkedHashMap<Integer, Map<String, Object>>();
        if (update) for (var raw : repository.read(id, "PurchaseDetailReadByInvPurchaseInvoiceId")) { var g = gridRowFromStored(raw); storedDetails.put((Integer) g.get("Id"), g); }
        var newGrnIds = new LinkedHashSet<Integer>();
        for (var r : suppliedRows) if (toInt(r.get("Id")) <= 0 && toInt(r.get("InvGrnId")) > 0) newGrnIds.add(toInt(r.get("InvGrnId")));
        boolean anyStored = suppliedRows.stream().anyMatch(r -> toInt(r.get("Id")) > 0);
        if (!newGrnIds.isEmpty() && anyStored) throw new IllegalArgumentException("A detail row belongs to another invoice or is repeated");
        int invoiceTypeId = update ? i(old, "InvoiceTypeId") : 1;
        var grnSource = new HashMap<Integer, Map<String, Object>>();
        List<Map<String, Object>> grnRaw = List.of();
        if (!newGrnIds.isEmpty()) {
            invoiceTypeId = verifyNewGrns(Objects.toString(body.get("loadMode"), ""), newGrnIds);
            grnRaw = repository.grnRows(idList(newGrnIds));
            for (var raw : grnRaw) { var g = gridRowFromGrn(raw, invoiceTypeId); grnSource.put((Integer) g.get("InvGrnDetailId"), g); }
        }
        boolean rateLocked = invoiceTypeId == 1 || (invoiceTypeId == 3 && !rateEditGatePurchase); // grdSettings :716-731
        var rows = new ArrayList<Map<String, Object>>();
        var seenIds = new HashSet<Integer>(); var seenGrnDetails = new HashSet<Integer>();
        for (var r : suppliedRows) {
            int rowId = toInt(r.get("Id"));
            Map<String, Object> source;
            if (rowId > 0) {
                source = storedDetails.get(rowId);
                if (source == null || !seenIds.add(rowId)) throw new IllegalArgumentException("A detail row belongs to another invoice or is repeated");
            } else {
                source = grnSource.get(toInt(r.get("InvGrnDetailId")));
                if (source == null) throw new IllegalArgumentException("InvGrnDetailId Required");
            }
            if (!seenGrnDetails.add(i(source, "InvGrnDetailId"))) throw new IllegalArgumentException("A GRN detail row is repeated");
            rows.add(applyEdits(new LinkedHashMap<>(source), r, rateLocked, weightAddLess, invoiceTypeId));
        }
        String deliveryTerm = update ? s(old, "DeliveryTerm") : "";
        if (!grnRaw.isEmpty()) deliveryTerm = s(copy(grnRaw.get(0)), "DeliveryTerm");
        var grnIdsInRows = rows.stream().map(g -> i(g, "InvGrnId")).collect(Collectors.toCollection(LinkedHashSet::new));
        var orderIdsInRows = rows.stream().map(g -> i(g, "PurchaseOrderId")).collect(Collectors.toSet());

        // ---- the operator grids (validated against the form's lists and the loaded GRNs)
        var accountIds = repository.accounts().stream().map(r -> i(r, "Id")).collect(Collectors.toSet());
        var allowedFreight = new HashSet<>(accountIds);
        var allowedLedger = new HashSet<>(accountIds);
        var gridGrnIds = new HashSet<>(grnIdsInRows);
        if (!grnIdsInRows.isEmpty()) {
            var grnTransporters = grnTransporters(idList(grnIdsInRows));
            allowedFreight.addAll(grnTransporters); allowedLedger.addAll(grnTransporters);
        }
        if (!newGrnIds.isEmpty() && !rows.isEmpty()) {
            // the Charge-to-Product rows LoadPurchaseOrderExpensesChargToProductData adds (shifted columns, see freightAndLedger)
            for (var raw : repository.orderChargesToProduct(i(rows.get(0), "PurchaseOrderId"))) allowedFreight.add(toInt(copy(raw).get("Percentage")));
        }
        if (update) {
            for (var raw : repository.read(id, "InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID")) { var f = copy(raw); allowedFreight.add(i(f, "TansporterId")); gridGrnIds.add(i(f, "InvGrnId")); }
            for (var raw : repository.read(id, "InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID")) { var j = copy(raw); allowedLedger.add(i(j, "ChartofAccountId")); gridGrnIds.add(i(j, "InvGrnId")); }
        }
        gridGrnIds.add(0);
        var freight = list(body.get("freight"));
        var ledger = list(body.get("ledger"));
        var expenses = list(body.get("expenses"));
        for (var f : freight) {
            if (toInt(f.get("Transporter")) != 0 && !allowedFreight.contains(toInt(f.get("Transporter")))) throw new IllegalArgumentException("Please Select an Account Against Freight First");
            if (!gridGrnIds.contains(toInt(f.get("InvGrnId")))) throw new IllegalArgumentException("A Charge to Product row does not belong to the loaded GRNs");
        }
        for (var l : ledger) {
            if (toInt(l.get("AccountId")) != 0 && !allowedLedger.contains(toInt(l.get("AccountId")))) throw new IllegalArgumentException("Please Select an Account Against JL First");
            if (!gridGrnIds.contains(toInt(l.get("InvGrnId")))) throw new IllegalArgumentException("A Supplier Add/Less row does not belong to the loaded GRNs");
        }
        var otherItems = new HashMap<Integer, String>();
        for (var raw : repository.otherItems()) { var o = copy(raw); otherItems.put(i(o, "Id"), s(o, "OtherItemName")); }
        for (var e : expenses) if (toInt(e.get("ItemId")) != 0 && !otherItems.containsKey(toInt(e.get("ItemId")))) throw new IllegalArgumentException("Please Select an Item Against Expense First");
        var bags = bagsFromBody(body, update, id, newGrnIds, invoiceTypeId);

        // ---- Insert :2618-2622 recalculation, then the grid checks :2623-2671
        double itemTotal = rows.stream().mapToDouble(g -> num(g.get("ItemAmount"))).sum();
        double netTotal = rows.stream().mapToDouble(g -> num(g.get("NetBillWeight"))).sum();
        double qtyTotal = rows.stream().mapToDouble(g -> num(g.get("ItemQty"))).sum();
        String commType = Objects.toString(body.get("commType"), "").trim();
        String commRateText = Objects.toString(body.get("commRate"), "");
        String commUom = Objects.toString(body.get("commUom"), "").trim();
        int commissionAgentId = toInt(body.get("commissionAgentId"));
        double commAmount = chargeAmount(commRateText, commType, commUom, itemTotal, netTotal, num(body.get("commAmount")));
        // BillAmount :1413-1424 - when supplier and agent are the same (both empty included) an empty rate forces the amount to 0.
        if (supplierId == commissionAgentId && commRateText.isEmpty()) commAmount = 0;
        String brokeryType = Objects.toString(body.get("brokeryType"), "").trim();
        String brokeryRateText = Objects.toString(body.get("brokeryRate"), "");
        String brokeryUom = Objects.toString(body.get("brokeryUom"), "").trim();
        int brokerAgentId = toInt(body.get("brokerAgentId"));
        double brokeryAmount = chargeAmount(brokeryRateText, brokeryType, brokeryUom, itemTotal, netTotal, num(body.get("brokeryAmount")));
        if (brokerAgentId <= 0 || brokeryAmount <= 0) brokeryAmount = 0; // BillAmount :1429-1443 clears the box
        if (commissionAgentId != 0 && supplierRows.stream().noneMatch(r -> i(r, "Id") == commissionAgentId)) throw new IllegalArgumentException("Please Select Commission Agent Account First");
        if (brokerAgentId != 0 && supplierRows.stream().noneMatch(r -> i(r, "Id") == brokerAgentId)) throw new IllegalArgumentException("BrokerAgent Account Not Found");
        double expenseTotal = expenses.stream().mapToDouble(e -> num(e.get("Amount"))).sum();
        double freightCredit = freight.stream().mapToDouble(f -> num(f.get("Freight"))).sum();
        double freightDebit = freight.stream().mapToDouble(f -> num(f.get("Debit"))).sum();
        for (var g : rows) {
            // ExpProportion :2134 (by ItemQty)
            g.put("Expense", expenseTotal > 0 ? expenseTotal / qtyTotal * num(g.get("ItemQty")) : 0d);
            // FreightProportion :2167
            g.put("Freights", freightDebit < freightCredit ? bankers(freightCredit - freightDebit) / netTotal * num(g.get("NetBillWeight")) : 0d);
            // CommissionProportion :2247
            g.put("Commission", commAmount > 0 ? ("Percent".equals(commType) ? num(g.get("ItemAmount")) * num(commRateText) / 100.0 : commAmount / netTotal * num(g.get("NetBillWeight"))) : 0d);
            // BillProportion :2229 (Journal is never proportioned - LedgerProportion :2225 is empty)
            g.put("BillAmount", num(g.get("ItemAmount")) + num(g.get("Expense")) + num(g.get("Freights")) + num(g.get("Journal")) + num(g.get("Commission")));
        }
        for (var f : freight) {
            if (!(num(f.get("Freight")) > 0)) continue;
            if (toInt(f.get("Transporter")) == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
            if (text(f.get("Remarks")).isEmpty()) throw new IllegalArgumentException("Grid Charge to Product Remarks required Please Check");
        }
        for (var l : ledger)
            if ((num(l.get("Credit")) > 0 || toInt(l.get("Debit")) > 0) && toInt(l.get("AccountId")) == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
        for (var e : expenses)
            if (num(e.get("Amount")) > 0 && toInt(e.get("ItemId")) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");
        for (var b : bags)
            if (i(b, "ItemId") > 0 && i(b, "TypeId") == 1 && n(b, "Amount") == 0) throw new IllegalArgumentException("Packing Material Amount Required");
        if (commAmount > 0 && commissionAgentId == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");

        // BillAmount :1366-1451
        double journalDebit = 0, journalCredit = 0, transporterDebit = 0, transporterCredit = 0, bagAmount = 0;
        for (var l : ledger) if (supplierId > 0 && toInt(l.get("AccountId")) > 0) { journalDebit += num(l.get("Debit")); journalCredit += num(l.get("Credit")); }
        for (var f : freight) if (toInt(f.get("Transporter")) > 0 && supplierGl == toInt(f.get("Transporter"))) { transporterDebit += num(f.get("Debit")); transporterCredit += num(f.get("Freight")); }
        for (var b : bags) if (n(b, "Amount") > 0 && i(b, "TypeId") == 1) bagAmount += n(b, "Amount");
        double bill = itemTotal + expenseTotal + bagAmount + journalDebit - journalCredit + transporterCredit - transporterDebit;
        if (supplierId == commissionAgentId && !commRateText.isEmpty()) bill += commAmount;
        if (brokerAgentId > 0 && brokeryAmount > 0) bill -= brokeryAmount;
        double billAmount = bankers(bill); // Math.Round(BillAmount).ToString("0,0") read back by Insert :2699

        // ---- header :2672-2713 (a fresh model: what the form does not set keeps its default)
        java.sql.Date docDate = parseDate(body.get("docDate"));
        if (docDate == null) docDate = java.sql.Date.valueOf(java.time.LocalDate.now());
        String dueDaysText = Objects.toString(body.get("dueDays"), "").trim();
        int dueDays = toInt(dueDaysText);
        var now = new java.sql.Timestamp(System.currentTimeMillis());
        var h = writes.defaults("Sp_InvPurchaseInvoice_Insert");
        h.put("Id", update ? id : 0);
        h.put("CommAmount", commAmount); h.put("CommissionAgentId", commissionAgentId); h.put("CommissionType", commType);
        double commRate = num(commRateText);
        h.put("CommRate", commRate);
        String commRemarks = Objects.toString(body.get("commRemarks"), "").trim();
        h.put("CommissionRemarks", commRemarks.isEmpty() ? " CommRate:   " + cs(commRate) + "  CommType: " + commType : commRemarks);
        h.put("BrokerAgentId", brokerAgentId); h.put("BrokeryType", brokeryType); h.put("BrokeryRate", num(brokeryRateText));
        h.put("BrokeryUom", num(brokeryUom)); h.put("BrokeryAmount", brokeryAmount);
        h.put("DocDate", docDate); h.put("DocNo", docNo); h.put("DocumentTypeId", TYPE);
        h.put("ManualBillNo", Objects.toString(body.get("manualBillNo"), "").trim());
        h.put("BranchesId", branchId); h.put("ProjectsId", projectId);
        h.put("RemarksHeader", Objects.toString(body.get("remarks"), "").trim());
        h.put("StockPartyId", update ? i(old, "StockPartyId") : 0); // hidden, never-bound cmbstockprty keeps the loaded value
        h.put("SupplierCustomerId", supplierId);
        h.put("UomScheduleIdCmRate", commUom);
        h.put("BillAmount", billAmount);
        h.put("SupplierInvoiceDate", now);
        h.put("DueDate", dueDaysText.isEmpty() ? java.sql.Date.valueOf(java.time.LocalDate.now()) : java.sql.Date.valueOf(docDate.toLocalDate().plusDays(dueDays)));
        h.put("DueDays", dueDays);
        h.put("DocumentTypeSrNo", toInt(body.get("grnNoText")));
        h.put("DeliveryTerm", deliveryTerm);
        h.put("InvoiceTypeId", 1);
        h.put("OrganizationId", org); h.put("CompanyId", company); h.put("FinancialYearId", context.currentFinancialYearId());
        h.put("ScreenName", SCREEN_NAME);
        h.put("EntryUser", context.currentUserId()); h.put("EntryDate", now); h.put("ModifyDate", now); h.put("ModifyUser", context.currentUserId());
        h.put("AttachmentsValues", update ? old.get("AttachmentsValues") : ""); h.put("CustomAttachmentsValues", update ? old.get("CustomAttachmentsValues") : "");
        h.put("ActionId", update ? 2 : 1);
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not found");

        // ---- detail rows :2715-2835 (checks in the form's nesting order)
        var details = new ArrayList<Map<String, Object>>();
        for (var g : rows) {
            if (i(g, "ItemId") == 0) throw new IllegalArgumentException("Item  Required");
            if ((float) num(g.get("ItemQty")) == 0f) throw new IllegalArgumentException("ItemQty  Required");
            if (text(g.get("CropYear")).isEmpty()) throw new IllegalArgumentException("CropYear Required");
            if (i(g, "InvGrnDetailId") == 0) throw new IllegalArgumentException("InvGrnDetailId Required");
            if (i(g, "InvGrnId") == 0) throw new IllegalArgumentException("InvGrnId Required");
            if ((float) num(g.get("GrossWeight")) == 0f) throw new IllegalArgumentException("GrossWeight Required");
            if ((float) num(g.get("OrderItemRate")) == 0f) throw new IllegalArgumentException("Item Rate Required");
            if (i(g, "ItemUomId") == 0) throw new IllegalArgumentException("ItemUom Required");
            if ((float) num(g.get("ItemAmount")) == 0f) throw new IllegalArgumentException("ItemAmount Required");
            if (i(g, "JobLotId") == 0) throw new IllegalArgumentException("JobLot Required");
            if ((float) num(g.get("NetBillWeight")) == 0f) throw new IllegalArgumentException("NetBillWeight Required");
            if (num(g.get("StockWeight")) == 0) throw new IllegalArgumentException("NetStockWeight Required");
            if (i(g, "PackingTypeId") == 0) throw new IllegalArgumentException("PackingType Required");
            if (i(g, "PurchaseOrderId") == 0) throw new IllegalArgumentException("PurchaseOrder No Required");
            if (i(g, "OrderItemRateUOMId") == 0) throw new IllegalArgumentException("Rate UOM Required");
            if (i(g, "WarehouseId") == 0) throw new IllegalArgumentException("WareHouse Required");
            var d = writes.defaults("Sp_InvPurchaseInvoiceDetail_Insert");
            d.put("Id", i(g, "Id")); d.put("ItemId", i(g, "ItemId")); d.put("ItemQty", num(g.get("ItemQty"))); d.put("CropYear", text(g.get("CropYear")));
            d.put("InvGrnDetailId", i(g, "InvGrnDetailId")); d.put("InvGrnId", i(g, "InvGrnId")); d.put("GrossWeight", num(g.get("GrossWeight")));
            d.put("ItemRate", num(g.get("OrderItemRate"))); d.put("ItemUOMId", i(g, "ItemUomId")); d.put("ItemAmount", num(g.get("ItemAmount")));
            d.put("ItemCgsRate", 0d); d.put("JobLotId", i(g, "JobLotId")); d.put("LabAnalisysNo", text(g.get("LabReportRef")));
            d.put("NetBillWeight", num(g.get("NetBillWeight"))); d.put("NetStockWeight", num(g.get("StockWeight"))); d.put("AdLsWeight", num(g.get("AdLsWeight")));
            d.put("PackingTypeId", i(g, "PackingTypeId")); d.put("PurchaseOrderId", i(g, "PurchaseOrderId")); d.put("PurchaseOrder", toInt(g.get("PurchaseOrder")));
            d.put("UomScheduleIdRate", i(g, "OrderItemRateUOMId")); d.put("WarehouseId", i(g, "WarehouseId")); d.put("WareHouseName", text(g.get("WareHouseName")));
            d.put("RateCut", num(g.get("RateCut"))); d.put("WeightCutTotal", num(g.get("WtCutTotal"))); d.put("EBTotalWt", num(g.get("EbTotal")));
            d.put("RateCutAmount", num(g.get("RateCutAmount"))); d.put("FreightAmount", num(g.get("Freights"))); d.put("JournalAmount", num(g.get("Journal")));
            d.put("ExpenseAmount", num(g.get("Expense"))); d.put("CommissionAmount", num(g.get("Commission")));
            d.put("GpNo", toInt(g.get("GpNo"))); d.put("VehicleNo", text(g.get("VehicleNo")));
            details.add(d);
        }
        // :2836-2854 - rows with an account
        var freightRows = new ArrayList<Map<String, Object>>();
        for (var f : freight) {
            if (toInt(f.get("Transporter")) == 0) continue;
            var pf = writes.defaults("Sp_InvPurchaseInvoiceFreight_Insert");
            pf.put("PurchaseOrderId", toInt(f.get("OrderId"))); pf.put("InvGrnId", toInt(f.get("InvGrnId"))); pf.put("TansporterId", toInt(f.get("Transporter")));
            pf.put("Percentage", num(f.get("Percentage"))); pf.put("FrQty", num(f.get("Qty"))); pf.put("FrRate", num(f.get("Rate")));
            pf.put("FreightAmount", num(f.get("Freight"))); pf.put("Debit", num(f.get("Debit"))); pf.put("Remarks", text(f.get("Remarks")));
            freightRows.add(pf);
        }
        // :2855-2873 - rows with an item; "0" / empty remarks become "ItemName : <item>  <qty>  @<rate>"
        var expenseRows = new ArrayList<Map<String, Object>>();
        for (var e : expenses) {
            int itemId = toInt(e.get("ItemId"));
            if (itemId == 0) continue;
            var pe = writes.defaults("Sp_InvPurchaseInvoiceExpense_Insert");
            pe.put("InvRevExpItemId", itemId); pe.put("Qty", num(e.get("Qty"))); pe.put("Rate", num(e.get("Rate"))); pe.put("Amount", num(e.get("Amount")));
            String remarks = text(e.get("Remarks")).trim();
            if (remarks.equals("0") || remarks.isEmpty()) remarks = "ItemName : " + otherItems.get(itemId).trim() + "  " + cs(num(e.get("Qty"))) + "  @" + cs(num(e.get("Rate")));
            pe.put("Remarks", remarks);
            expenseRows.add(pe);
        }
        // :2874-2891 - rows with an account
        var journalRows = new ArrayList<Map<String, Object>>();
        for (var l : ledger) {
            if (toInt(l.get("AccountId")) == 0) continue;
            var pj = writes.defaults("Sp_InvPurchaseInvoiceJournal_Insert");
            pj.put("ChartofAccountId", toInt(l.get("AccountId"))); pj.put("JvRemarks", text(l.get("Remarks"))); pj.put("JvPrcnt", num(l.get("Percentage")));
            pj.put("JvQty", num(l.get("Qty"))); pj.put("JvRate", num(l.get("Rate"))); pj.put("JvDebit", num(l.get("Debit"))); pj.put("JvCredit", num(l.get("Credit")));
            pj.put("InvGrnId", toInt(l.get("InvGrnId")));
            journalRows.add(pj);
        }
        // :2892-2913 - rows with an item
        var bagRows = new ArrayList<Map<String, Object>>();
        for (var b : bags) {
            if (i(b, "ItemId") == 0) continue;
            var peb = writes.defaults("Sp_InvPurchaseInvoiceEmptyBags_Insert");
            peb.put("PurchaseOrderId", i(b, "OrderId")); peb.put("TypeId", i(b, "TypeId")); peb.put("InvGrnId", i(b, "InvGrnId")); peb.put("ItemId", i(b, "ItemId"));
            peb.put("PurchaseQty", n(b, "PurchaseQty")); peb.put("Rate", n(b, "Rate")); peb.put("Amount", n(b, "Amount"));
            if (i(b, "TypeId") == 1 && n(b, "Amount") == 0) throw new IllegalArgumentException("Packing Material Amount Required");
            peb.put("Remarks", text(b.get("Remarks")));
            bagRows.add(peb);
        }
        if (!update && details.stream().anyMatch(d -> i(d, "Id") > 0)) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");

        // ---- BLL Save :22 - MakeVoucherForPurchaseInvoice first (0549 :14-139), then DAL SetData
        h.put("InvoiceQty", details.stream().mapToDouble(d -> n(d, "ItemQty")).sum());
        h.put("InvoiceWeight", details.stream().mapToDouble(d -> n(d, "NetBillWeight")).sum());
        var accounts = writes.accounts(h, details);
        if (accounts.stockAgainst() <= 0) throw new IllegalArgumentException("Stock AgainstAc Configuration Not Found");
        if (commAmount > 0 && commissionAgentId <= 0) throw new IllegalArgumentException("CommissionAgent Account Not Found");
        if (brokeryAmount != 0 && brokerAgentId <= 0) throw new IllegalArgumentException("BrokerAgent Account Not Found");
        if (!accounts.parties().containsKey(supplierId)) throw new IllegalArgumentException("Supplier GLAccountId not Found");
        var voucher = PurchaseInvoiceFinancialRules.calculate(h, details, freightRows, journalRows, bagRows, accounts);
        var attachmentChange = body.get("attachments") == null ? null : attachments.prepare(id, TYPE, body.get("attachments"));
        if (attachmentChange != null) { h.put("AttachmentsValues", attachmentChange.names()); h.put("CustomAttachmentsValues", attachmentChange.storedNames()); }
        int newId = writes.execute(update ? "Sp_InvPurchaseInvoice_Update" : "Sp_InvPurchaseInvoice_Insert", h);
        if (!update) id = newId;
        if (id <= 0) throw new IllegalStateException("Invoice procedure did not return an ID");
        h.put("Id", id);
        boolean wagesToProduct = flag("ContractWagesChargetoProduct");
        int line = 0;
        var detailIds = new StringBuilder();
        for (var d : details) {
            d.put("LineId", ++line);
            // DAL 0434 :78-95
            d.put("BillAmount", n(d, "ItemAmount") + n(d, "FreightAmount") + n(d, "ExpenseAmount") + n(d, "CommissionAmount") + n(d, "Brokery")
                    + (wagesToProduct ? n(d, "WagesAmount") : 0) - n(d, "EbPurAgainstWeightAmount") - n(d, "FreightDeduction"));
            d.put("InvPurchaseInvoiceId", id);
            int detailId = writes.execute("Sp_InvPurchaseInvoiceDetail_Insert", d);
            if (detailId <= 0) throw new IllegalStateException("Invoice detail procedure did not return an ID");
            d.put("Id", detailId);
            detailIds.append(detailId).append(',');
        }
        for (var f : freightRows) { f.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceFreight_Insert", f); }
        for (var j : journalRows) { j.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceJournal_Insert", j); }
        for (var e : expenseRows) { e.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceExpense_Insert", e); }
        for (var b : bagRows) { b.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceEmptyBags_Insert", b); }
        ProcExec.run(jdbc, "EXEC dbo.usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations @OrganizationId=?,@CompanyId=?,@Id=?,@DetailIds=?", org, company, id, detailIds.toString());
        if (attachmentChange != null) attachments.persist(id, TYPE, supplierId, attachmentChange);
        writes.post(h, voucher);
        var out = new LinkedHashMap<String, Object>();
        out.put("success", true); out.put("id", id); out.put("docNo", docNo); out.put("voucherHeadId", repository.voucherHeadId(id));
        out.put("message", update ? "Record Update Successfully [" + docNo + "] " : "Record Saved Successfully [" + docNo + "] ");
        return out;
    }

    /**
     * New GRN rows on Save: the loader path re-runs the frmLoadGRN selection (pending type-169 GRNs of the allocated
     * branches, same refusals); the Grn No path re-runs GetGRNIdByDocNo for the GRN's number (same organization / company /
     * financial year, type 169, not yet on an invoice). Returns the invoice type the grid was loaded with.
     */
    private int verifyNewGrns(String loadMode, Set<Integer> grnIds) {
        if ("grnNo".equals(loadMode)) {
            if (grnIds.size() != 1) throw new IllegalArgumentException("Record Not Found For Loader");
            int grnId = grnIds.iterator().next();
            int docNo = repository.grnDocNo(grnId);
            if (docNo <= 0) throw new IllegalArgumentException("Record Not Found For Loader");
            var sel = grnNoSelection(docNo);
            if (!((List<?>) sel.get("ids")).contains(grnId)) throw new IllegalArgumentException("Record Not Found For Loader");
            return (Integer) sel.get("invoiceTypeId");
        }
        var sel = loaderSelection(new ArrayList<>(grnIds));
        if (!new HashSet<>((List<?>) sel.get("ids")).equals(new HashSet<>(grnIds))) throw new IllegalArgumentException("A selected GRN is no longer pending or is outside your allocated branches");
        return (Integer) sel.get("invoiceTypeId");
    }

    /**
     * One edited grid row. grd_CellUpdated :1036 runs only for the edited columns (OrderItemRate, RateCut, EquivalentPoRate,
     * AdLsWeight); an unedited row keeps the loaded values. Editability follows grdSettings :716-736.
     */
    private static Map<String, Object> applyEdits(Map<String, Object> g, Map<String, Object> r, boolean rateLocked, boolean weightAddLess, int invoiceTypeId) {
        double rate = rateLocked || !r.containsKey("OrderItemRate") ? num(g.get("OrderItemRate")) : num(r.get("OrderItemRate"));
        double eq = rateLocked || !r.containsKey("EquivalentPoRate") ? num(g.get("EquivalentPoRate")) : num(r.get("EquivalentPoRate"));
        double cut = r.containsKey("RateCut") ? num(r.get("RateCut")) : num(g.get("RateCut"));
        double adLs = weightAddLess && r.containsKey("AdLsWeight") ? num(r.get("AdLsWeight")) : num(g.get("AdLsWeight"));
        boolean edited = rate != num(g.get("OrderItemRate")) || eq != num(g.get("EquivalentPoRate")) || cut != num(g.get("RateCut")) || adLs != num(g.get("AdLsWeight"));
        if (!edited) return g;
        double net;
        if (invoiceTypeId == 2) net = num(g.get("GrossWeight")) - Math.abs(adLs);
        else {
            if (adLs > 0 && adLs > num(g.get("EbTotal"))) throw new IllegalArgumentException("AddWeight cannot be greater than EmptyBags weight");
            net = num(g.get("GrossWeight")) - num(g.get("EbTotal")) - num(g.get("WtCutTotal")) + adLs;
        }
        if (eq == 0) throw new IllegalArgumentException("Rate UOM Required");
        g.put("OrderItemRate", rate); g.put("EquivalentPoRate", eq); g.put("RateCut", cut); g.put("AdLsWeight", adLs);
        g.put("NetBillWeight", net);
        double cutAmount = net / eq * cut;
        g.put("RateCutAmount", cutAmount);
        g.put("ItemAmount", net / eq * rate - cutAmount);
        return g;
    }

    /** Empty bags: the GRN (new rows) or stored (update) rows, with the typed Rate (grdEmptyBagsSettings :788 - read-only for
     *  invoice type 1) and Remarks; Amount = PurchaseQty * Rate when the rate was edited (grdEmptyBags_CellUpdated :1343). */
    private List<Map<String, Object>> bagsFromBody(Map<String, Object> body, boolean update, int id, Set<Integer> newGrnIds, int invoiceTypeId) {
        var source = new ArrayList<Map<String, Object>>();
        if (!newGrnIds.isEmpty()) source.addAll(emptyBagsFromGrn(idList(newGrnIds)));
        else if (update) for (var raw : repository.read(id, "InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID")) {
            var b = copy(raw);
            source.add(bagRow(i(b, "PurchaseOrderId"), i(b, "TypeId"), b.get("EmptyBagsType"), i(b, "InvGrnId"), i(b, "ItemId"), b.get("ItemName"),
                    n(b, "PurchaseQty"), n(b, "Rate"), n(b, "Amount"), text(b.get("Remarks"))));
        }
        var used = Collections.newSetFromMap(new IdentityHashMap<Map<String, Object>, Boolean>());
        var out = new ArrayList<Map<String, Object>>();
        for (var r : list(body.get("emptyBags"))) {
            if (toInt(r.get("ItemId")) == 0) continue;
            Map<String, Object> src = null;
            for (var b : source)
                if (!used.contains(b) && i(b, "InvGrnId") == toInt(r.get("InvGrnId")) && i(b, "ItemId") == toInt(r.get("ItemId")) && i(b, "TypeId") == toInt(r.get("TypeId"))) { src = b; break; }
            if (src == null) throw new IllegalArgumentException("An empty-bag row does not belong to the loaded GRNs");
            used.add(src);
            var b = new LinkedHashMap<String, Object>(src);
            double rate = invoiceTypeId == 1 ? n(src, "Rate") : num(r.get("Rate"));
            b.put("Rate", rate);
            b.put("Amount", rate == n(src, "Rate") ? n(src, "Amount") : n(src, "PurchaseQty") * rate);
            b.put("Remarks", text(r.get("Remarks")));
            out.add(b);
        }
        return out;
    }

    private static java.sql.Date parseDate(Object v) {
        if (blank(v)) return null;
        try { return java.sql.Date.valueOf(v.toString().trim().substring(0, 10)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Use a valid document date"); }
    }
}
