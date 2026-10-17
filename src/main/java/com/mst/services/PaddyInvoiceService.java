package com.mst.services;

import com.mst.repositories.InwardGatePassRecordRepository;
import com.mst.repositories.PaddyGrnRepository;
import com.mst.repositories.PaddyInvoiceRepository;
import com.mst.repositories.PaddyPurchaseRepository;
import com.mst.repositories.PurchaseInvoiceWriteRepository;
import com.mst.security.CurrentUserContext;
import java.util.*;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

/**
 * Paddy Purchase invoice screens: 466 frmPurchaseInvoiceForGate (InvPurchaseInvoice DocumentTypeId 166) and, through the same service,
 * 467 frmPurchaseInvoiceForGhallaMandi (168). Line references are to frmPurchaseInvoiceForGate.cs; the save chain is BLL 0581 InvPurchaseInvoice.Save
 * (PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice, then DAL 0434 SetData with Sp_InvPurchaseInvoice_Insert/_Update).
 */
@Service
public class PaddyInvoiceService {
    /** screen = form Name (rights, attachments); docType = InvPurchaseInvoice DocumentTypeId; grnDocType = DocumentTypeId the Load GRN popup is opened with. */
    public record Profile(String screen, int docType, int grnDocType) {}
    public static final Profile GATE = new Profile("frmPurchaseInvoiceForGate", 166, 165);
    /** 467 frmPurchaseInvoiceForGhallaMandi: DocumentTypeId 168, Load GRN popup frmLoadGrnForGhallaMandi opened with DocumentTypeId 167. */
    public static final Profile GHALLA = new Profile("frmPurchaseInvoiceForGhallaMandi", 168, 167);
    private static boolean ghalla(Profile p) { return p.docType() == 168; }
    /** SelectedRow.Cells[2] of the Loading Contractor combo (frmPurchaseInvoiceForGhallaMandi.cs :2511) = the GL account column of the group-6 list. */
    private static int glOf(Map<String, Object> row) {
        Object v = pick(row, "GlAccountId");
        if (v == null) { int i = 0; for (Object o : row.values()) if (i++ == 2) { v = o; break; } }
        return num(v);
    }

    @Autowired private PaddyInvoiceRepository repo;
    @Autowired private PaddyPurchaseRepository paddy;
    @Autowired private PaddyGrnRepository grn;
    @Autowired private PurchaseInvoiceWriteRepository writes;
    @Autowired private InwardGatePassRecordRepository rights;
    @Autowired private CurrentUserContext context;
    @Autowired private JdbcTemplate jdbc;

    private static int num(Object v) { return v instanceof Number ? ((Number) v).intValue() : (v == null || v.toString().isBlank() ? 0 : (int) dbl(v)); }
    private static double dbl(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return v == null || v.toString().isBlank() ? 0 : Double.parseDouble(v.toString().replace(",", "").trim()); } catch (NumberFormatException e) { return 0; }
    }
    private static String text(Object v) { return v == null ? "" : v.toString().trim(); }
    private static Object pick(Map<String, ?> row, String key) {
        if (row == null) return null;
        for (Map.Entry<String, ?> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }
    /** .NET double.ToString() for the remark texts. */
    private static String dn(double d) { return d == Math.rint(d) && Math.abs(d) < 1e15 ? Long.toString((long) d) : java.math.BigDecimal.valueOf(d).stripTrailingZeros().toPlainString(); }
    private static double banker(double d) { return Math.rint(d); }   // Math.Round(double)
    private static Map<String, Object> opt(Object id, Object name) { Map<String, Object> m = new LinkedHashMap<>(); m.put("id", id); m.put("name", name); return m; }

    private int org() { return context.currentOrganizationId(); }
    private int company() { return context.currentCompanyId(); }

    /** frmPurchaseInvoiceForGate_Load (:260): rights, AccountsFill, OtherItemsBind, SupplierNameFilll, DocumentNo; commission type/UOM lists are static (:1149, :1174). */
    public Map<String, Object> init(Profile p) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", rights.hasScreenRight(p.screen(), "Save"));
        out.put("canUpdate", rights.hasScreenRight(p.screen(), "Update"));
        out.put("canPrint", rights.hasScreenRight(p.screen(), "Print"));
        out.put("docNo", docNo(p));
        out.putAll(lookups(p));
        return out;
    }
    public Map<String, Object> lookups() { return lookups(GATE); }
    /** Gate: SupplierNameFilll = SupplierCustomerGetAllServiceBind (:1090). Ghalla Mandi: GetSupplierustomerByCustomerGroupId("6") for Loading Contractor / Commission Agent, plus GhallaMandiBind (:1234). */
    public Map<String, Object> lookups(Profile p) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", ghalla(p) ? paddy.suppliersByGroupIds(org(), company(), "6", 0) : paddy.supplierCustomerAll(org(), company()));
        if (ghalla(p)) out.put("ghallaMandi", repo.ghallaMandi(org(), company()));
        List<Map<String, Object>> accounts = new ArrayList<>();
        for (Map<String, Object> r : repo.coaAllocation(org(), company(), context.currentUserId())) {   // AccountsFill (:1109)
            int t = num(pick(r, "AccountTypeId"));
            if (t != 2 && t != 11 && t != 12 && t != 15) accounts.add(opt(pick(r, "Id"), pick(r, "AccountTitle")));
        }
        out.put("accounts", accounts);
        out.put("otherItems", repo.otherItems(org(), company()));
        return out;
    }
    public int docNo(Profile p) {                                    // DocumentNo (:1192)
        int code = repo.generateCode(org(), company(), p.docType(), context.currentFinancialYearId());
        if (code > 0) return code;
        throw new IllegalStateException("Max Number Not Found");
    }

    /* ---------- Load GRN popup (frmLoadGRNForPaddyPurchase) ---------- */
    public Map<String, Object> popupLookups() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", paddy.suppliersByGroupIds(org(), company(), "2", 0));            // GetSupplierustomerByCustomerGroupId("2")
        return out;
    }
    public List<Map<String, Object>> popupOrders(int supplierId) {                               // GetOrderBySupplierId
        return repo.openApprovedOrders(org(), company(), supplierId, 0);
    }
    public List<Map<String, Object>> pendingGrns(Profile p, int orderId, String from, String to) {   // PendingGrnLoad
        java.sql.Date f = from == null || from.isBlank() ? null : java.sql.Date.valueOf(from.substring(0, 10));
        java.sql.Date t = to == null || to.isBlank() ? null : java.sql.Date.valueOf(to.substring(0, 10));
        return repo.pendingGrns(org(), company(), p.grnDocType(), 0, orderId, context.currentFinancialYearId(), f, t);
    }
    public Map<String, Object> startYear() {                                                   // clsGlobalVariables.ActiveYr.Start_Period
        int year = context.currentFinancialYearId();
        for (Map<String, Object> r : jdbc.queryForList("EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?,@CompanyId=?", org(), company()))
            if (num(pick(r, "Id")) == year) return Map.of("startDate", text(pick(r, "Start_Period")));
        return Map.of("startDate", "");
    }
    public List<Map<String, Object>> loadGrn(int grnId, int orderId) {                         // LoadDataDetailGridAgainstGP (:1332)
        return orderId > 0 ? repo.grnLoadByOrder(grnId, orderId) : repo.grnLoad(String.valueOf(grnId));
    }
    public Map<String, Object> loadExtras(String ids, int orderId) {                           // LoadFreightData / LoadEmptyBagsData / LoadExpData
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("freight", repo.transporterAndFreight(ids));
        out.put("emptyBags", repo.emptyBagsFromGrn(ids, orderId));
        out.put("expenses", repo.itemQtyAndRateFromPo(ids));
        return out;
    }
    public List<Map<String, Object>> uoms(int itemId) { return grn.uomScheduleByItem(org(), company(), itemId); }

    /* ---------- Ghalla Mandi: popup frmLoadGrnForGhallaMandi and the combined-ids loaders ---------- */
    /** frmLoadGRN_Load: LoadingContractorBind (group "6"), GhallaMandiBind. */
    public Map<String, Object> ghallaPopupLookups() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("suppliers", paddy.suppliersByGroupIds(org(), company(), "6", 0));
        out.put("ghallaMandi", repo.ghallaMandi(org(), company()));
        return out;
    }
    /** PendingGrnLoad: InvPurchaseInvoice.GetPendingGhallaMandiGrnForInvoice (DocumentTypeId of the popup = 167). */
    public List<Map<String, Object>> ghallaPending(Profile p, int supplierId, int ghallaMandiId, String from, String to) {
        java.sql.Date f = from == null || from.isBlank() ? null : java.sql.Date.valueOf(from.substring(0, 10));
        java.sql.Date t = to == null || to.isBlank() ? null : java.sql.Date.valueOf(to.substring(0, 10));
        return repo.ghallaPending(org(), company(), p.grnDocType(), context.currentFinancialYearId(), supplierId, ghallaMandiId, f, t);
    }
    /** LoadInGridDetail (:1450) builds GrnIds = "," + id + "," + id ...; the ids come from the checked popup rows. */
    private static String idList(String ids) {
        StringBuilder sb = new StringBuilder();
        for (String x : text(ids).split(",")) {
            if (x.isBlank()) continue;
            if (!x.trim().matches("[0-9]{1,9}")) throw new IllegalArgumentException("Invalid GRN id");
            sb.append(',').append(x.trim());
        }
        if (sb.length() == 0) throw new IllegalArgumentException("Invalid GRN id");
        return sb.toString();
    }
    /** LoadDataDetailGridAgainstGP (:1372): InvPurchaseInvoice.GrnGhallaMandiLoadForInvoice(GrnIds). */
    public List<Map<String, Object>> loadGhalla(String ids) { return repo.grnGhallaLoad(idList(ids)); }
    /** LoadFreightData(GrnIds) / LoadEmptyBagsData(GrnIds): GetEmptyBagsFromGrn(ids, 0). The Ghalla form has no LoadExpData. */
    public Map<String, Object> loadExtrasGhalla(String ids) {
        String list = idList(ids);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("freight", repo.transporterAndFreight(list));
        out.put("emptyBags", repo.emptyBagsFromGrn(list, 0));
        return out;
    }

    /* ---------- history / read ---------- */
    public List<Map<String, Object>> history(Profile p, int noOfRecords) {                     // GetAll (:1699)
        boolean all = rights.hasScreenRight(p.screen(), "CanView AllRecord");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(org(), company(), p.docType(), all, context.currentFinancialYearId(), noOfRecords, context.currentUserId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", pick(r, "Id")); m.put("VoucherHeadId", pick(r, "VoucherHeadId")); m.put("DocumentTypeId", pick(r, "DocumentTypeId"));
            m.put("InvoiceDate", pick(r, "DocDate")); m.put("InvoiceNo", pick(r, "DocNo")); m.put("DueDate", pick(r, "DueDate")); m.put("DueDays", pick(r, "DueDays"));
            m.put("ManualNo", pick(r, "ManualBillNo")); m.put("SupplierName", pick(r, "SupplierName")); m.put("RemarksHeader", pick(r, "RemarksHeader"));
            m.put("BillAmount", dbl(pick(r, "BillAmount"))); m.put("EntryDate", pick(r, "EntryDate")); m.put("EntryUser", pick(r, "EntryUser"));
            m.put("ModifyDate", pick(r, "ModifyDate")); m.put("ModifyUser", pick(r, "ModifyUser")); m.put("ApprovedStatus", pick(r, "ApprovedStatus"));
            m.put("NoOfAttachments", pick(r, "NoOfAttachments"));
            out.add(m);
        }
        return out;
    }

    private Map<String, Object> requireRecord(Profile p, int id) {
        Map<String, Object> h = repo.header(id);
        if (h == null || num(pick(h, "DocumentTypeId")) != p.docType() || num(pick(h, "OrganizationId")) != org() || num(pick(h, "CompanyId")) != company())
            throw new IllegalArgumentException("Record Not Found");
        return h;
    }

    /** ReadById (:1853): InvPurchaseInvoice.GetByID -> header + detail/exp/journal/freight/empty-bags collections, rows shaped as the form's data tables. */
    public Map<String, Object> getById(Profile p, int id) {
        Map<String, Object> h = requireRecord(p, id);
        boolean all = rights.hasScreenRight(p.screen(), "CanView AllRecord");
        if (!all && num(pick(h, "EntryUser")) != context.currentUserId()) throw new IllegalArgumentException("Record Not Found");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("header", h);
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> d : repo.collection(id, "PurchaseDetailReadByInvPurchaseInvoiceId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", pick(d, "Id")); m.put("PurchaseOrderId", pick(d, "PurchaseOrderId")); m.put("InvGrnDetailId", pick(d, "InvGrnDetailId")); m.put("InvGrnId", pick(d, "InvGrnId"));
            m.put("ItemId", pick(d, "ItemId")); m.put("ItemName", pick(d, "ItemName")); m.put("CropYear", pick(d, "CropYear")); m.put("ItemUomId", pick(d, "ItemUOMId"));
            m.put("PackUom", pick(d, "UOMCodeItem")); m.put("ItemQty", dbl(pick(d, "ItemQty"))); m.put("GrossWeight", dbl(pick(d, "GrossWeight"))); m.put("ScaleKart", dbl(pick(d, "ScaleKart")));
            m.put("EbTotal", dbl(pick(d, "EBTotalWt"))); m.put("NetBillWeight", dbl(pick(d, "NetBillWeight"))); m.put("StockWeight", dbl(pick(d, "NetStockWeight")));
            m.put("ItemRate", pick(d, "ItemRate")); m.put("ItemRateUOMId", pick(d, "UomScheduleIdRate")); m.put("RateUom", dbl(pick(d, "EquivalentPoRate")));
            m.put("ItemAmount", dbl(pick(d, "ItemAmount"))); m.put("ItemNetAmount", dbl(pick(d, "BillAmount"))); m.put("Freights", dbl(pick(d, "FreightAmount")));
            m.put("Expense", dbl(pick(d, "ExpenseAmount"))); m.put("Commission", dbl(pick(d, "CommissionAmount"))); m.put("JobLotId", pick(d, "JobLotId"));
            m.put("WarehouseId", pick(d, "WarehouseId")); m.put("WareHouseName", pick(d, "WareHouseName")); m.put("PackingTypeId", pick(d, "PackingTypeId"));
            m.put("LabNo", pick(d, "LabAnalisysNo")); m.put("GpNo", pick(d, "GpNo")); m.put("VehicleNo", pick(d, "VehicleNo"));
            m.put("PackTypeDesc", pick(d, "PackTypeDesc")); m.put("JobLotDescription", pick(d, "JobLotDescription"));
            grid.add(m);
        }
        out.put("details", grid);
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> d : repo.collection(id, "InvPurchaseInvoiceExpense_ReadByPurchaseInvoiceID")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", pick(d, "InvRevExpItemId")); m.put("Qty", pick(d, "Qty")); m.put("Rate", pick(d, "Rate")); m.put("Amount", dbl(pick(d, "Amount"))); m.put("Remarks", pick(d, "Remarks"));
            exp.add(m);
        }
        out.put("expenses", exp);
        List<Map<String, Object>> jl = new ArrayList<>();
        for (Map<String, Object> d : repo.collection(id, "InvPurchaseInvoiceJournal_ReadByPurchaseInvoiceID")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("AccountId", pick(d, "ChartofAccountId")); m.put("Remarks", pick(d, "JvRemarks")); m.put("Percentage", pick(d, "JvPrcnt")); m.put("Qty", pick(d, "JvQty"));
            m.put("Rate", pick(d, "JvRate")); m.put("Debit", dbl(pick(d, "JvDebit"))); m.put("Credit", dbl(pick(d, "JvCredit")));
            jl.add(m);
        }
        out.put("journal", jl);
        List<Map<String, Object>> fr = new ArrayList<>();
        for (Map<String, Object> d : repo.collection(id, "InvPurchaseInvoiceFreight_ReadByPurchaseInvoiceID")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("OrderId", pick(d, "PurchaseOrderId")); m.put("InvGrnId", pick(d, "InvGrnId")); m.put("Transporter", pick(d, "TansporterId")); m.put("Percentage", pick(d, "Percentage"));
            m.put("Qty", dbl(pick(d, "FrQty"))); m.put("Rate", dbl(pick(d, "FrRate"))); m.put("Freight", banker(dbl(pick(d, "FreightAmount")))); m.put("Debit", dbl(pick(d, "Debit"))); m.put("Remarks", pick(d, "Remarks"));
            fr.add(m);
        }
        out.put("freight", fr);
        List<Map<String, Object>> eb = new ArrayList<>();
        for (Map<String, Object> d : repo.collection(id, "InvPurchaseInvoiceEmptyBags_ReadByPurchaseInvoiceID")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("OrderId", pick(d, "PurchaseOrderId")); m.put("TypeId", pick(d, "TypeId")); m.put("Type", pick(d, "EmptyBagsType")); m.put("InvGrnId", pick(d, "InvGrnId"));
            m.put("ItemId", pick(d, "ItemId")); m.put("ItemName", pick(d, "ItemName")); m.put("PurchaseQty", dbl(pick(d, "PurchaseQty"))); m.put("Rate", pick(d, "Rate"));
            m.put("Amount", dbl(pick(d, "Amount"))); m.put("Remarks", pick(d, "Remarks"));
            eb.add(m);
        }
        out.put("emptyBags", eb);
        out.put("voucherHeadId", repo.voucherHeadId(org(), company(), p.docType(), id));      // VoucherHeadIdGet
        out.put("approved", truthy(pick(h, "IsApproved")));
        return out;
    }
    private static boolean truthy(Object v) { return v instanceof Boolean ? (Boolean) v : v instanceof Number ? ((Number) v).intValue() != 0 : "true".equalsIgnoreCase(text(v)) || "1".equals(text(v)); }

    /** GetDetailGrdByHeadId (:960): the history "Detail" button; same GetByID, shaped as dtdetail. */
    public List<Map<String, Object>> historyDetail(Profile p, int id) {
        requireRecord(p, id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.collection(id, "PurchaseDetailReadByInvPurchaseInvoiceId")) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("GpNo", pick(d, "GpNo")); m.put("VehicleNo", pick(d, "VehicleNo")); m.put("Unloading", pick(d, "WareHouseName")); m.put("ItemName", pick(d, "ItemName"));
            m.put("PackType", pick(d, "PackTypeDesc")); m.put("ItemUOM", pick(d, "UOMCodeItem")); m.put("CropYear", pick(d, "CropYear")); m.put("JobLot", pick(d, "JobLotDescription"));
            m.put("ItemQty", dbl(pick(d, "ItemQty"))); m.put("GrossWeight", dbl(pick(d, "GrossWeight"))); m.put("EBTotalWt", dbl(pick(d, "EBTotalWt"))); m.put("NetBillWeight", dbl(pick(d, "NetBillWeight")));
            m.put("ItemRate", dbl(pick(d, "ItemRate"))); m.put("RateUOM", pick(d, "EquivalentPoRate")); m.put("ItemAmount", dbl(pick(d, "ItemAmount")));
            m.put("CommissionAmount", dbl(pick(d, "CommissionAmount"))); m.put("ExpenseAmount", dbl(pick(d, "ExpenseAmount"))); m.put("FreightAmount", dbl(pick(d, "FreightAmount")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> attachments(Profile p, int id) { requireRecord(p, id); return repo.attachmentsByScreen(id, p.screen()); }

    /* ---------- the form's calculations, re-run on the server before saving (Insert :1973 calls them in this order) ---------- */
    private static double sum(List<Map<String, Object>> rows, String key) { double s = 0; for (Map<String, Object> r : rows) s += dbl(pick(r, key)); return s; }
    private static void set(Map<String, Object> row, String key, Object value) { row.put(key, value); }

    /** TotalCommissionAmount (:1222) then CommissionProportion (:1654). Returns the commission amount. */
    private double totalCommission(Map<String, Object> pl, List<Map<String, Object>> details) {
        String rate = text(pick(pl, "commRate")), type = text(pick(pl, "commType"));
        double amount = dbl(pick(pl, "commAmount"));
        if (rate.isEmpty()) amount = 0;
        else if (type.equals("Flat")) amount = Math.floor(dbl(rate) + 0.5);                    // CommRate.ToString("0,0")
        else if (type.equals("Percent")) amount = banker(sum(details, "ItemAmount") * dbl(rate) / 100.0);
        else if (type.equals("Comm Weight")) { double uom = dbl(text(pick(pl, "commUom"))); amount = banker(sum(details, "NetBillWeight") / uom * dbl(rate)); }
        return amount;
    }
    private void commissionProportion(Map<String, Object> pl, List<Map<String, Object>> details, double total) {
        double pct = dbl(pick(pl, "commRate")), net = sum(details, "NetBillWeight");
        for (Map<String, Object> r : details) {
            if (total > 0) {
                if (text(pick(pl, "commType")).equals("Percent")) set(r, "Commission", dbl(pick(r, "ItemAmount")) * pct / 100.0);
                else set(r, "Commission", net == 0 ? 0 : total / net * dbl(pick(r, "NetBillWeight")));
            } else set(r, "Commission", 0);
        }
    }
    private void expProportion(List<Map<String, Object>> details, List<Map<String, Object>> exp) {
        double total = sum(exp, "Amount"), qty = sum(details, "ItemQty");
        for (Map<String, Object> r : details) set(r, "Expense", total > 0 && qty != 0 ? total / qty * dbl(pick(r, "ItemQty")) : 0);
    }
    private void freightProportion(List<Map<String, Object>> details, List<Map<String, Object>> freight) {
        double net = sum(details, "NetBillWeight"), credit = sum(freight, "Freight"), debit = sum(freight, "Debit");
        for (Map<String, Object> r : details) set(r, "Freights", debit < credit && net != 0 ? banker(credit - debit) / net * dbl(pick(r, "NetBillWeight")) : 0);
    }
    /** BillAmount (:1003): returns Math.Round(BillAmount). */
    private double billAmount(int supplierId, int supplierGl, int commAgent, String commRate, double commAmount, List<Map<String, Object>> details, List<Map<String, Object>> exp,
                              List<Map<String, Object>> journal, List<Map<String, Object>> freight, List<Map<String, Object>> bags) {
        double bagsAmount = 0, jd = 0, jc = 0, td = 0, tc = 0;
        for (Map<String, Object> r : journal) if (supplierId > 0 && num(pick(r, "AccountId")) > 0) { jd += dbl(pick(r, "Debit")); jc += dbl(pick(r, "Credit")); }
        for (Map<String, Object> r : freight) if (num(pick(r, "Transporter")) > 0 && supplierGl == num(pick(r, "Transporter"))) { td += dbl(pick(r, "Debit")); tc += dbl(pick(r, "Freight")); }
        for (Map<String, Object> r : bags) if (dbl(pick(r, "Amount")) > 0 && num(pick(r, "TypeId")) == 1) bagsAmount += dbl(pick(r, "Amount"));
        double bill = sum(details, "ItemAmount") + sum(exp, "Amount") + bagsAmount;
        bill = bill + jd - jc;
        bill = bill + tc - td;
        if (supplierId == commAgent && !commRate.isEmpty()) bill += commAmount;
        return banker(bill);
    }

    /* ---------- Insert (:1973) ---------- */
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> rows(Map<String, Object> body, String key) {
        Object v = body.get(key);
        List<Map<String, Object>> out = new ArrayList<>();
        if (v == null) return out;
        if (!(v instanceof List<?>)) throw new IllegalArgumentException(key + " must be a list");
        for (Object o : (List<?>) v) {
            if (!(o instanceof Map<?, ?>)) throw new IllegalArgumentException("Invalid " + key + " row");
            out.add(new LinkedHashMap<>((Map<String, Object>) o));
        }
        return out;
    }

    @Transactional
    public Map<String, Object> save(Profile p, Map<String, Object> body) {
        int id = num(pick(body, "id")), docType = p.docType();
        boolean update = id > 0;
        if (!rights.hasScreenRight(p.screen(), update ? "Update" : "Save")) throw new SecurityException("You do not have the " + (update ? "Update" : "Save") + " right on this screen");
        Map<String, Object> old = null;
        if (update) {                                                                      // btnUpdate_Click (:1957)
            old = requireRecord(p, id);
            if (truthy(pick(old, "IsApproved"))) throw new IllegalArgumentException("Record Not Update beacause Record has approved");
        }
        // FormValidation (:1067)
        int supplierId = num(pick(body, "supplierId"));
        if (supplierId <= 0) throw new IllegalArgumentException("Supplier Name is Required");
        int ghallaMandiId = num(pick(body, "ghallaMandiId"));
        if (ghalla(p)) {                                                                    // frmPurchaseInvoiceForGhallaMandi FormValidation (:1091)
            boolean known = false;
            if (ghallaMandiId > 0) for (Map<String, Object> m : repo.ghallaMandi(org(), company())) if (num(pick(m, "Id")) == ghallaMandiId) { known = true; break; }
            if (!known) throw new IllegalArgumentException("Ghalla_Mandi  Field is Required");
        }
        String docNoText = text(pick(body, "docNo"));
        if (docNoText.isEmpty() || docNoText.equals("0")) throw new IllegalArgumentException("DocNo is Required");
        String dueDaysText = text(pick(body, "dueDays"));
        if (dueDaysText.isEmpty() || dueDaysText.equals("0")) throw new IllegalArgumentException("DueDays Field is Required");

        List<Map<String, Object>> details = rows(body, "details"), freight = rows(body, "freight"), exp = rows(body, "expenses"), journal = rows(body, "journal"), bags = rows(body, "emptyBags");
        var suppliers = ghalla(p) ? paddy.suppliersByGroupIds(org(), company(), "6", 0) : paddy.supplierCustomerAll(org(), company());
        Map<String, Object> supplier = null;
        for (Map<String, Object> s : suppliers) if (num(pick(s, "Id")) == supplierId) { supplier = s; break; }
        if (supplier == null) throw new IllegalArgumentException("Supplier Name is Required");
        int supplierGl = ghalla(p) ? glOf(supplier) : num(pick(supplier, "GlAccountId"));                                  // cmbsuppliername_ValueChanged -> txtSupplierGLId
        int commAgent = num(pick(body, "commAgentId"));
        String commRate = text(pick(body, "commRate"));

        // TotalCommissionAmount, BillAmount, ExpProportion, FreightProportion, CommissionProportion
        double commAmount = totalCommission(body, details);
        commissionProportion(body, details, commAmount);
        double billAmount = billAmount(supplierId, supplierGl, commAgent, commRate, commAmount, details, exp, journal, freight, bags);
        expProportion(details, exp);
        freightProportion(details, freight);
        commissionProportion(body, details, commAmount);

        for (Map<String, Object> r : freight) {                                               // Insert :1992
            if (!(dbl(pick(r, "Freight")) > 0)) continue;
            if (num(pick(r, "Transporter")) == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
            if (text(pick(r, "Remarks")).isEmpty()) throw new IllegalArgumentException("Grid Charge to Product Remarks required Please Check");
        }
        for (Map<String, Object> r : journal)
            if ((dbl(pick(r, "Credit")) > 0 || num(pick(r, "Debit")) > 0) && num(pick(r, "AccountId")) == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
        for (Map<String, Object> r : exp)
            if (dbl(pick(r, "Amount")) > 0 && num(pick(r, "ItemId")) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");
        for (Map<String, Object> r : bags)
            if (num(pick(r, "ItemId")) > 0 && num(pick(r, "TypeId")) == 1 && dbl(pick(r, "Amount")) == 0) throw new IllegalArgumentException("Packing Material Amount Required");
        if (commAmount > 0 && commAgent == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");
        String commType = text(pick(body, "commType"));
        double commRateNum = dbl(commRate);
        String commRemarks = text(pick(body, "commRemarks"));
        if (commRemarks.isEmpty()) commRemarks = " CommRate:   " + dn(commRateNum) + "  CommType: " + commType;

        Map<String, Object> h = writes.defaults("Sp_InvPurchaseInvoice_Insert");
        if (update) for (String k : List.of("IsApproved", "PostDate", "PostUser", "AttachmentsValues", "CustomAttachmentsValues", "RowVersion", "IsUploaded", "UploadedDate", "UploadedById")) h.put(k, old.get(k));
        h.put("CommAmount", commAmount); h.put("CommissionAgentId", commAgent); h.put("CommissionType", commType); h.put("CommRate", commRateNum); h.put("CommissionRemarks", commRemarks);
        h.put("DocDate", java.sql.Date.valueOf(text(pick(body, "docDate")).substring(0, 10)));
        h.put("DocNo", update ? num(pick(old, "DocNo")) : num(docNoText));
        if (ghalla(p)) h.put("GhallaMandiId", ghallaMandiId);                              // purchaseInvoice.GhallaMandiId = cmbGhallaMandi.Value (:2058)
        h.put("DocumentTypeId", docType); h.put("ManualBillNo", text(pick(body, "manualBillNo"))); h.put("RemarksHeader", text(pick(body, "remarksHeader")));
        h.put("SupplierCustomerId", supplierId); h.put("UomScheduleIdCmRate", text(pick(body, "commUom"))); h.put("BillAmount", billAmount);
        java.sql.Timestamp now = new java.sql.Timestamp(System.currentTimeMillis());
        h.put("SupplierInvoiceDate", now);
        int dueDays = num(dueDaysText);
        h.put("DueDays", dueDays);
        h.put("DueDate", java.sql.Date.valueOf(java.time.LocalDate.parse(text(pick(body, "docDate")).substring(0, 10)).plusDays(dueDays)));   // DuedaysCalculates (:2420)
        h.put("OrganizationId", org()); h.put("CompanyId", company()); h.put("FinancialYearId", context.currentFinancialYearId());
        h.put("EntryUser", context.currentUserId()); h.put("EntryDate", now); h.put("ModifyDate", now); h.put("ModifyUser", context.currentUserId());
        h.put("ActionId", update ? 2 : 1);
        if (update) h.put("Id", id);

        // detail rows (:2042..)
        if (details.isEmpty()) throw new IllegalArgumentException("Grid Record Not found");
        Set<Integer> storedDetailIds = new HashSet<>();
        if (update) for (Map<String, Object> d : repo.collection(id, "PurchaseDetailReadByInvPurchaseInvoiceId")) storedDetailIds.add(num(pick(d, "Id")));
        List<Map<String, Object>> list = new ArrayList<>();
        Map<Integer, List<Map<String, Object>>> uomCache = new HashMap<>();
        for (Map<String, Object> r : details) {
            Map<String, Object> d = new LinkedHashMap<>();
            int detailId = num(pick(r, "Id"));
            if (detailId > 0 && !storedDetailIds.contains(detailId)) throw new IllegalArgumentException("A detail row belongs to another invoice");
            if (!update && detailId > 0) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");
            d.put("Id", detailId); d.put("PurchaseOrderId", ghalla(p) ? 0 : num(pick(r, "PurchaseOrderId")));   // the Ghalla grid has no PurchaseOrderId column
            if (num(pick(r, "ItemId")) == 0) throw new IllegalArgumentException("Item  Required");
            d.put("ItemId", num(pick(r, "ItemId")));
            if ((float) dbl(pick(r, "ItemQty")) == 0f) throw new IllegalArgumentException("ItemQty  Required");
            d.put("ItemQty", dbl(pick(r, "ItemQty")));
            d.put("CropYear", text(pick(r, "CropYear")));
            if (text(pick(r, "CropYear")).isEmpty()) throw new IllegalArgumentException("CropYear Required");
            if (num(pick(r, "InvGrnDetailId")) == 0) throw new IllegalArgumentException("InvGrnDetailId Required");
            d.put("InvGrnDetailId", num(pick(r, "InvGrnDetailId")));
            if (num(pick(r, "InvGrnId")) == 0) throw new IllegalArgumentException("InvGrnId Required");
            d.put("InvGrnId", num(pick(r, "InvGrnId")));
            if ((float) dbl(pick(r, "GrossWeight")) == 0f) throw new IllegalArgumentException("GrossWeight Required");
            d.put("GrossWeight", dbl(pick(r, "GrossWeight")));
            if (ghalla(p) ? dbl(pick(r, "ItemRate")) == 0.0 : (float) dbl(pick(r, "ItemRate")) == 0f) throw new IllegalArgumentException("Item Rate Required");   // Ghalla compares as double (:2074)
            d.put("ItemRate", dbl(pick(r, "ItemRate")));
            if (ghalla(p) && dbl(pick(r, "RateUom")) == 0.0) throw new IllegalArgumentException("RateUom Field Required");
            if (num(pick(r, "ItemUomId")) == 0) throw new IllegalArgumentException("ItemUom Required");
            d.put("ItemUOMId", num(pick(r, "ItemUomId")));
            if ((float) dbl(pick(r, "ItemAmount")) == 0f) throw new IllegalArgumentException("ItemAmount Required");
            d.put("ItemAmount", dbl(pick(r, "ItemAmount")));
            d.put("ItemCgsRate", 0.0);
            if (num(pick(r, "JobLotId")) == 0) throw new IllegalArgumentException("JobLot Required");
            d.put("JobLotId", num(pick(r, "JobLotId")));
            d.put("LabAnalisysNo", text(pick(r, "LabNo")));
            if ((float) dbl(pick(r, "NetBillWeight")) == 0f) throw new IllegalArgumentException("NetBillWeight Required");
            d.put("NetBillWeight", dbl(pick(r, "NetBillWeight")));
            d.put("NetStockWeight", dbl(pick(r, "StockWeight")));
            if (dbl(pick(r, "StockWeight")) == 0.0) throw new IllegalArgumentException("NetStockWeight Required");
            if (num(pick(r, "PackingTypeId")) == 0) throw new IllegalArgumentException("PackingType Required");
            d.put("PackingTypeId", num(pick(r, "PackingTypeId")));
            int rateUomId = num(pick(r, "ItemRateUOMId"));
            if (rateUomId == 0) {                                                            // CommonServices.GetUomScheduleByItemId + Select("Equivalent='..'")
                int itemId = num(pick(r, "ItemId"));
                List<Map<String, Object>> sch = uomCache.computeIfAbsent(itemId, k -> grn.uomScheduleByItem(org(), company(), k));
                if (!sch.isEmpty()) {
                    int want = num(pick(r, "RateUom")); Integer found = null;
                    for (Map<String, Object> s : sch) if ((int) Math.rint(dbl(pick(s, "Equivalent"))) == want || Math.rint(dbl(pick(s, "Equivalent"))) == want) { found = num(pick(s, "Id")); break; }
                    if (found == null) throw new IllegalArgumentException("this RateUom not define please check");
                    rateUomId = found;
                }
            }
            d.put("UomScheduleIdRate", rateUomId);
            if (num(pick(r, "WarehouseId")) == 0) throw new IllegalArgumentException("WareHouse Required");
            d.put("WarehouseId", num(pick(r, "WarehouseId"))); d.put("WareHouseName", text(pick(r, "WareHouseName")));
            d.put("EBTotalWt", dbl(pick(r, "EbTotal"))); d.put("ScaleKart", ghalla(p) ? 0.0 : dbl(pick(r, "ScaleKart")));
            d.put("FreightAmount", dbl(pick(r, "Freights"))); d.put("ExpenseAmount", dbl(pick(r, "Expense"))); d.put("CommissionAmount", dbl(pick(r, "Commission")));
            d.put("GpNo", num(pick(r, "GpNo"))); d.put("VehicleNo", text(pick(r, "VehicleNo")));
            // the GRN reference must belong to this company, be a GRN of the popup's document type and match the item
            Integer ok = jdbc.queryForObject("SELECT COUNT(*) FROM dbo.InvGrn g JOIN dbo.InvGrnDetail gd ON gd.InvGrnId=g.Id WHERE g.Id=? AND gd.Id=? AND g.OrganizationId=? AND g.CompanyId=? AND g.DocumentTypeId=? AND gd.ItemId=?",
                    Integer.class, d.get("InvGrnId"), d.get("InvGrnDetailId"), org(), company(), p.grnDocType(), d.get("ItemId"));
            if (ok == null || ok != 1) throw new IllegalArgumentException("GRN reference is outside this company or does not match the item");
            list.add(d);
        }

        var freightRows = new ArrayList<Map<String, Object>>();
        for (Map<String, Object> r : freight) if (num(pick(r, "Transporter")) != 0) {
            Map<String, Object> f = new LinkedHashMap<>();
            f.put("PurchaseOrderId", num(pick(r, "OrderId"))); f.put("InvGrnId", num(pick(r, "InvGrnId"))); f.put("TansporterId", num(pick(r, "Transporter")));
            f.put("Percentage", dbl(pick(r, "Percentage"))); f.put("FrQty", dbl(pick(r, "Qty"))); f.put("FrRate", dbl(pick(r, "Rate"))); f.put("FreightAmount", dbl(pick(r, "Freight")));
            f.put("Debit", dbl(pick(r, "Debit"))); f.put("Remarks", text(pick(r, "Remarks")));
            freightRows.add(f);
        }
        Map<Integer, String> otherNames = new HashMap<>();
        for (Map<String, Object> o : repo.otherItems(org(), company())) otherNames.put(num(pick(o, "Id")), text(pick(o, "OtherItemName")));
        var expRows = new ArrayList<Map<String, Object>>();
        for (Map<String, Object> r : exp) if (num(pick(r, "ItemId")) != 0) {
            Map<String, Object> e = new LinkedHashMap<>();
            double q = dbl(pick(r, "Qty")), rt = dbl(pick(r, "Rate"));
            e.put("InvRevExpItemId", num(pick(r, "ItemId"))); e.put("Qty", q); e.put("Rate", rt); e.put("Amount", dbl(pick(r, "Amount")));
            String rem = text(pick(r, "Remarks"));
            if (rem.equals("0") || rem.isEmpty()) rem = "ItemName : " + otherNames.getOrDefault(num(pick(r, "ItemId")), "") + "  " + dn(q) + "  @" + dn(rt);
            e.put("Remarks", rem);
            expRows.add(e);
        }
        var journalRows = new ArrayList<Map<String, Object>>();
        for (Map<String, Object> r : journal) if (num(pick(r, "AccountId")) != 0) {
            Map<String, Object> j = new LinkedHashMap<>();
            j.put("ChartofAccountId", num(pick(r, "AccountId"))); j.put("JvRemarks", text(pick(r, "Remarks"))); j.put("JvPrcnt", dbl(pick(r, "Percentage")));
            j.put("JvQty", dbl(pick(r, "Qty"))); j.put("JvRate", dbl(pick(r, "Rate"))); j.put("JvDebit", dbl(pick(r, "Debit"))); j.put("JvCredit", dbl(pick(r, "Credit")));
            journalRows.add(j);
        }
        var bagRows = new ArrayList<Map<String, Object>>();
        for (Map<String, Object> r : bags) if (num(pick(r, "ItemId")) != 0) {
            Map<String, Object> b = new LinkedHashMap<>();
            b.put("PurchaseOrderId", num(pick(r, "OrderId"))); b.put("TypeId", num(pick(r, "TypeId"))); b.put("InvGrnId", num(pick(r, "InvGrnId"))); b.put("ItemId", num(pick(r, "ItemId")));
            b.put("PurchaseQty", dbl(pick(r, "PurchaseQty"))); b.put("Rate", dbl(pick(r, "Rate"))); b.put("Amount", dbl(pick(r, "Amount")));
            if (num(pick(r, "TypeId")) == 1 && dbl(pick(r, "Amount")) == 0.0) throw new IllegalArgumentException("Packing Material Amount Required");
            b.put("Remarks", text(pick(r, "Remarks")));
            bagRows.add(b);
        }

        // BLL 0581 Save: MakeVoucherForPurchaseInvoice first, then DAL 0434 SetData
        h.put("InvoiceQty", list.stream().mapToDouble(d -> PaddyInvoiceFinancialRules.n(d, "ItemQty")).sum());
        h.put("InvoiceWeight", list.stream().mapToDouble(d -> PaddyInvoiceFinancialRules.n(d, "NetBillWeight")).sum());
        var accounts = writes.accounts(h, list);
        var financial = PaddyInvoiceFinancialRules.calculate(h, list, freightRows, journalRows, bagRows, accounts);
        double difference = financial.details().stream().mapToDouble(d -> PaddyInvoiceFinancialRules.n(d, "DebitAmount") - PaddyInvoiceFinancialRules.n(d, "CreditAmount")).sum();
        if (Math.abs(difference) > 0.01) throw new IllegalArgumentException("Invoice voucher does not balance; check allocated freight, commission and empty-bag charges (difference " + difference + ")");

        int newId = writes.execute(update ? "Sp_InvPurchaseInvoice_Update" : "Sp_InvPurchaseInvoice_Insert", h);
        if (!update) id = newId;
        if (id <= 0) throw new IllegalStateException("Invoice procedure did not return an ID");
        h.put("Id", id);
        h.put("DocNo", jdbc.queryForObject("SELECT DocNo FROM dbo.InvPurchaseInvoice WHERE Id=?", Integer.class, id));
        boolean wages = Boolean.parseBoolean(writes.configuration(org(), company(), "ContractWagesChargetoProduct"));
        int line = 0; var ids = new ArrayList<String>();
        for (Map<String, Object> d : list) {
            d.put("InvPurchaseInvoiceId", id); d.put("LineId", ++line);
            d.put("BillAmount", PaddyInvoiceFinancialRules.n(d, "ItemAmount") + PaddyInvoiceFinancialRules.n(d, "FreightAmount") + PaddyInvoiceFinancialRules.n(d, "ExpenseAmount")
                    + PaddyInvoiceFinancialRules.n(d, "CommissionAmount") + PaddyInvoiceFinancialRules.n(d, "Brokery") + (wages ? PaddyInvoiceFinancialRules.n(d, "WagesAmount") : 0)
                    - PaddyInvoiceFinancialRules.n(d, "EbPurAgainstWeightAmount") - PaddyInvoiceFinancialRules.n(d, "FreightDeduction"));
            int detailId = writes.execute("Sp_InvPurchaseInvoiceDetail_Insert", d);
            if (detailId <= 0) throw new IllegalStateException("Invoice detail procedure did not return an ID");
            ids.add(Integer.toString(detailId));
        }
        for (Map<String, Object> f : freightRows) { f.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceFreight_Insert", f); }
        for (Map<String, Object> j : journalRows) { j.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceJournal_Insert", j); }
        for (Map<String, Object> e : expRows) { e.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceExpense_Insert", e); }
        for (Map<String, Object> b : bagRows) { b.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceEmptyBags_Insert", b); }
        com.mst.repositories.support.ProcExec.run(jdbc, "EXEC dbo.usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations @OrganizationId=?,@CompanyId=?,@Id=?,@DetailIds=?",
                h.get("OrganizationId"), h.get("CompanyId"), id, String.join(",", ids) + ",");
        writes.post(h, financial);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true); out.put("id", id); out.put("docNo", h.get("DocNo")); out.put("supplierId", supplierId);
        out.put("voucherHeadId", repo.voucherHeadId(org(), company(), docType, id));
        out.put("message", (update ? "Record Update Successfully [" : "Record Saved Successfully [") + h.get("DocNo") + "] ");
        return out;
    }
}
