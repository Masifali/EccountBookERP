package com.mst.services;

import com.mst.repositories.PurchaseInvoiceNumberingRepository;
import com.mst.repositories.PurchaseInvoiceReturnRepository;
import com.mst.repositories.PurchaseInvoiceWriteRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.jdbc.core.SqlParameterValue;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.sql.Types;
import java.util.*;

import static com.mst.repositories.PurchaseInvoiceReturnRepository.*;

/**
 * Purchase Invoice Return (InvfrmInvPurchaseInvoiceReturn, screen 125, DocumentTypeId 59).
 * Insert :2399 (validations in the desktop's order and words, header/detail/children mapping), BLL 0581 Save
 * (voucher built first, detail-id check), the 59 voucher (PurchaseInvoiceReturnFinancialRules), and the DAL 0434 SetData
 * order with its type-59 branches (no stock-in-transit call, FIFO/CGS when ERP feature 5, InventoryTransactions,
 * USP_InventoryValidation per line). The desktop form has no header Delete (btnDelete Visible=false, no handler).
 */
@Service
public class PurchaseInvoiceReturnService {

    private final PurchaseInvoiceReturnRepository repo;
    private final PurchaseInvoiceWriteRepository writes;
    private final PurchaseInvoiceNumberingRepository numbering;
    private final PurchaseInvoiceAttachmentService attachments;
    private final CurrentUserContext context;
    private final JdbcTemplate jdbc;

    public PurchaseInvoiceReturnService(PurchaseInvoiceReturnRepository repo, PurchaseInvoiceWriteRepository writes, PurchaseInvoiceNumberingRepository numbering,
                                        PurchaseInvoiceAttachmentService attachments, CurrentUserContext context, JdbcTemplate jdbc) {
        this.repo = repo; this.writes = writes; this.numbering = numbering; this.attachments = attachments; this.context = context; this.jdbc = jdbc;
    }

    // ------------------------------------------------------------------ helpers (Conversion.* semantics)
    static double afz(double v, int digits) { return BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_UP).doubleValue(); }          // MidpointRounding.AwayFromZero
    static double even(double v, int digits) { return BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_EVEN).doubleValue(); }       // Math.Round default
    private static int toInt(Object o) {   // Conversion.ToInt: a double is banker's-rounded, a non-integer string is 0
        if (o instanceof Number n) return (int) Math.rint(n.doubleValue());
        String s = str(o); if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { return 0; }
    }
    private static Map<String, Object> ci(Map<String, Object> m) { var t = new TreeMap<String, Object>(String.CASE_INSENSITIVE_ORDER); if (m != null) t.putAll(m); return t; }
    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Map<String, Object> p, String key) {
        Object v = p.get(key); List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List<?> l) for (Object o : l) if (o instanceof Map<?, ?> m) out.add(ci((Map<String, Object>) m));
        return out;
    }
    private static double sum(List<Map<String, Object>> rows, String key) { double s = 0; for (var r : rows) s += dbl(r.get(key)); return s; }

    // ------------------------------------------------------------------ page data
    public Map<String, Object> init() {
        repo.requireRight("View");
        Map<String, Object> flags = repo.flags();
        Map<String, Object> out = new LinkedHashMap<>(flags);
        out.put("lookups", repo.lookups(flags));
        out.put("numbers", repo.numbers(numbering));
        return out;
    }
    public Map<String, Object> numbers() { repo.requireRight("View"); return repo.numbers(numbering); }

    // ------------------------------------------------------------------ calculations (:3777-3960)
    /** ExpProportion :3830, FreightProportion :3865, CommissionProportion :3917, BillProportion :3899 (row values). */
    static void proportions(List<Map<String, Object>> grid, double commAmount, String commType, double commRate, double expenseTotal, double freightCredit, int N) {
        double w = sum(grid, "NetWeight");
        for (var r : grid) {
            double nw = dbl(r.get("NetWeight"));
            r.put("Expense", expenseTotal > 0 ? expenseTotal / w * nw : 0d);
            r.put("Freights", freightCredit > 0 ? freightCredit / w * nw : 0d);
            if (commAmount > 0) r.put("Commission", "Percent".equals(commType) ? dbl(r.get("Amount")) * commRate / 100.0 : commAmount / w * nw);
            else r.put("Commission", 0d);
            r.put("BillAmount", afz(dbl(r.get("Amount")) + dbl(r.get("Expense")) + dbl(r.get("Freights")) + dbl(r.get("Commission")), N));
        }
    }
    /** BillAmount :3777 (the header total as displayed in txtBillAmount). */
    static double headerBill(List<Map<String, Object>> grid, List<Map<String, Object>> expenses, List<Map<String, Object>> journal, List<Map<String, Object>> freight,
                             int supplierGlId, int supplierId, int agentId, double commAmount, int N) {
        double item = even(sum(grid, "Amount"), 0);   // Math.Round(double) - banker's, to 0 decimals (Q-B1)
        double exp = sum(expenses, "Amount");
        double dr = 0, cr = 0;
        for (var j : journal) {
            String a = str(j.get("AccountId"));
            int acc; try { acc = Integer.parseInt(a); } catch (NumberFormatException e) { throw new IllegalArgumentException("Input string was not in a correct format."); }   // int.Parse
            if (acc > 0) { dr += dbl(j.get("Debit")); cr += dbl(j.get("Credit")); }
        }
        double transporterCredit = 0;
        for (var f : freight) if (toInt(f.get("Transporter")) > 0 && String.valueOf(supplierGlId).equals(str(f.get("Transporter")))) transporterCredit += dbl(f.get("Freight"));
        double bill = item + exp;
        double diff = cr - dr;
        bill = diff < 0 ? bill - Math.abs(diff) : bill + diff;
        bill += transporterCredit;
        if (supplierId == agentId) bill += commAmount;
        return afz(bill, N);
    }

    // ------------------------------------------------------------------ save (Insert :2399)
    @Transactional
    public Map<String, Object> save(Map<String, Object> payload) {
        var p = ci(payload);
        Map<String, Object> flags = repo.flags();
        @SuppressWarnings("unchecked") Map<String, Object> defaults = (Map<String, Object>) flags.get("defaults");
        boolean multiCurrency = Boolean.TRUE.equals(flags.get("hasMultiCurrencyFeature"));
        int N = num(flags.get("amountDecimals"));
        int recId = num(p.get("Id"));
        boolean update = recId > 0;
        repo.requireRight(update ? "Update" : "Save");
        if (update) {
            var old = repo.header(recId);
            if (old == null || (num(old.get("EntryUser")) != context.currentUserId() && !repo.hasRight("CanView AllRecord")))
                throw new IllegalArgumentException("Record not found");
        }
        var grid = list(p, "details"); var freightGrid = list(p, "freight"); var glGrid = list(p, "journal"); var expGrid = list(p, "expenses");
        // :2413
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        // FormValidation :676
        String docNoText = str(p.get("DocNo"));
        if (docNoText.isEmpty() || "0".equals(docNoText)) throw new IllegalArgumentException("DocNo Field is Required");
        int supplier = num(p.get("SupplierCustomerId"));
        if (supplier == 0) throw new IllegalArgumentException("Supplier Name Field is Required");
        int currencyId = num(p.get("CurrencyId"));
        String exchangeText = str(p.get("ExchangeRate"));
        if (multiCurrency) {
            if (currencyId == 0) throw new IllegalArgumentException("Fcy Code Field is Required");
            if (dbl(exchangeText) == 0) throw new IllegalArgumentException("Exchange Rate Field is Required");
            if (dbl(p.get("FcyAmount")) == 0) throw new IllegalArgumentException("Fcy Amount Field is Required");
        } else {
            if (currencyId == 0) throw new IllegalArgumentException("Please Configure Your Base Currency In configurations");
            if (exchangeText.isEmpty() || "0".equals(exchangeText)) throw new IllegalArgumentException("Please Configure Your Base Currency Rate In configurations");
        }
        // amounts as the form holds them before the freight loop (the desktop never recomputes after it, Q-S1)
        double commAmount = dbl(p.get("CommAmount"));
        String commType = str(p.get("CommissionType"));
        int agent = num(p.get("CommissionAgentId"));
        int supplierGl = 0; for (var s : repo.suppliers()) if (num(s.get("Id")) == supplier) supplierGl = num(s.get("GlAccountId"));
        proportions(grid, commAmount, commType, dbl(p.get("CommRate")), sum(expGrid, "Amount"), sum(freightGrid, "Freight"), N);
        double billAmount = headerBill(grid, expGrid, glGrid, freightGrid, supplierGl, supplier, agent, commAmount, N);
        // :2433 freight grid
        String delivery = str(p.get("DeliveryTerm"));
        boolean ponch = delivery.equals("Ponch") || delivery.equals("Ponch & PartyWeight") || delivery.equals("Ponch & FactoryWeight");
        for (var f : freightGrid) {
            if (ponch) { f.put("Freight", 0d); f.put("Transporter", ""); continue; }
            if (dbl(f.get("Freight")) > 0 && toInt(f.get("Transporter")) == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
            if (dbl(f.get("Freight")) > 0 && str(f.get("Remarks")).isEmpty()) throw new IllegalArgumentException("Grid Charge to Product Remarks required Please Check");
        }
        for (var j : glGrid) if ((dbl(j.get("Credit")) > 0 || toInt(j.get("Debit")) > 0) && toInt(j.get("AccountId")) == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
        for (var e : expGrid) if (dbl(e.get("Amount")) > 0 && toInt(e.get("ItemId")) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");
        if (commAmount > 0 && agent == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");

        // header (:2479-2502) - a new model object on every save, update included (Q-S2)
        var h = writes.defaults("Sp_InvPurchaseInvoice_Insert");
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Object docDate = str(p.get("DocDate")).isEmpty() ? now : p.get("DocDate");
        h.put("Id", recId);
        h.put("CommAmount", commAmount); h.put("CommissionAgentId", agent); h.put("CommissionRemarks", str(p.get("CommissionRemarks")));
        h.put("CommissionType", commType); h.put("CommRate", dbl(p.get("CommRate"))); h.put("UomScheduleIdCmRate", str(p.get("UomScheduleIdCmRate")));
        h.put("DocDate", docDate); h.put("DocNo", toInt(docNoText)); h.put("DocumentTypeId", TYPE); h.put("ManualBillNo", str(p.get("ManualBillNo")));
        h.put("ProjectsId", context.currentBranchId()); h.put("ReferencePartyId", 0); h.put("RemarksHeader", str(p.get("RemarksHeader"))); h.put("StockPartyId", 0);
        h.put("SupplierCustomerId", supplier);
        h.put("SupplierInvoiceNo", toInt(p.get("DueDaysText")));   // txtduedays is saved as SupplierInvoiceNo (Q-S3)
        h.put("SupplierReferenceNo", str(p.get("SupplierReferenceNo")));
        h.put("BillAmount", billAmount);
        h.put("EntryDate", now); h.put("ModifyDate", now); h.put("EntryUser", context.currentUserId()); h.put("ModifyUser", context.currentUserId());
        h.put("OrganizationId", context.currentOrganizationId()); h.put("CompanyId", context.currentCompanyId()); h.put("BranchesId", context.currentBranchId());
        h.put("FinancialYearId", context.currentFinancialYearId());
        h.put("DueDate", str(p.get("DueDate")).isEmpty() ? docDate : p.get("DueDate"));
        h.put("PostDate", now); h.put("SupplierInvoiceDate", now);
        h.put("CurrencyId", currencyId); h.put("ExchangeRate", dbl(exchangeText)); h.put("FcyAmount", dbl(p.get("FcyAmount")));
        h.put("CustomAccounts", truthy(p.get("CustomAccounts")));
        h.put("ScreenName", SAVE_SCREEN);
        h.put("ActionId", update ? 2 : 1);

        // details (:2508-2640)
        double grossTotal = 0;
        var details = new ArrayList<Map<String, Object>>();
        int rowNo = 0;
        for (var r : grid) {
            rowNo++;
            var d = writes.defaults("Sp_InvPurchaseInvoiceDetail_Insert");
            d.put("PurchaseOrderId", 0); d.put("Id", num(r.get("Id"))); d.put("GdnId", num(r.get("GdnId"))); d.put("GdnDetailId", num(r.get("GdnDetailId")));
            d.put("GdnDocumentTypeId", 703); d.put("GdnNo", num(r.get("GdnNo")));
            d.put("WarehouseId", num(r.get("WarehouseId"))); d.put("ItemId", num(r.get("ItemId"))); d.put("CropYear", str(r.get("CropYear")));
            d.put("JobLotId", num(r.get("JobLotId"))); d.put("PackingTypeId", num(r.get("PackingTypeId"))); d.put("ItemUOMId", num(r.get("PackUOMId")));
            d.put("ItemQty", dbl(r.get("ItemQty"))); d.put("GrossWeight", dbl(r.get("GrossWeight"))); grossTotal += dbl(r.get("GrossWeight"));
            d.put("EBWeight", dbl(r.get("EBUnit"))); d.put("EBTotalWt", dbl(r.get("EBTotal")));
            d.put("WeightCut", (double) (float) dbl(r.get("WeightCut"))); d.put("WeightCutTotal", (double) (float) dbl(r.get("WeightCutTotal")));   // Conversion.ToSingle
            d.put("AdLsWeight", dbl(r.get("AdLs"))); d.put("NetBillWeight", dbl(r.get("NetWeight"))); d.put("NetStockWeight", dbl(r.get("StockWeight")));
            d.put("ItemRate", dbl(r.get("ItemRate"))); d.put("UomScheduleIdRate", num(r.get("RateUOMId"))); d.put("EquivalentPoRate", dbl(r.get("RateEquivalent")));
            d.put("RateCut", dbl(r.get("RateCut"))); d.put("RateCutAmount", dbl(r.get("RateCutTotal"))); d.put("ItemAmount", dbl(r.get("Amount")));
            d.put("GpDate", str(r.get("GpDate")).isEmpty() ? null : r.get("GpDate")); d.put("GpNo", toInt(r.get("GpNo"))); d.put("VehicleNo", str(r.get("VehicleNo")));
            d.put("RemarksDetail", str(r.get("RemarksDetail")));
            d.put("CommissionAmount", dbl(r.get("Commission"))); d.put("ExpenseAmount", dbl(r.get("Expense"))); d.put("FreightAmount", dbl(r.get("Freights")));
            d.put("BillAmount", dbl(r.get("BillAmount"))); d.put("FcyAmount", dbl(r.get("FcyAmount"))); d.put("BranchId", num(r.get("BranchId")));
            d.put("CurrencyId", currencyId); d.put("ExchangeRate", dbl(exchangeText));
            d.put("RefDocumentTypeId", num(r.get("RefDocumentTypeId"))); d.put("RefDocId", num(r.get("RefDocId"))); d.put("RefDocSubId", num(r.get("RefDocSubId")));
            String at = " in Detail Grid And row No: " + rowNo;
            if (num(d.get("WarehouseId")) == 0) throw new IllegalArgumentException("Warehouse Field is Required" + at);
            if (num(d.get("ItemId")) == 0) throw new IllegalArgumentException("Item Field is Required" + at);
            if (str(d.get("CropYear")).isEmpty()) throw new IllegalArgumentException("Crop Year Field is Required" + at);
            if (num(d.get("JobLotId")) == 0) throw new IllegalArgumentException("Job/Lot Field is Required" + at);
            if (num(d.get("PackingTypeId")) == 0) throw new IllegalArgumentException("Packing Type Field is Required" + at);
            if (num(d.get("ItemUOMId")) == 0) throw new IllegalArgumentException("UOM Field is Required" + at);
            if (dbl(d.get("ItemQty")) == 0) throw new IllegalArgumentException("Item Qty Field is Required" + at);
            if (dbl(d.get("GrossWeight")) == 0) throw new IllegalArgumentException("Gross Weight Field is Required" + at);
            if (dbl(d.get("NetBillWeight")) == 0) throw new IllegalArgumentException("Net Weight Field is Required" + at);
            if (dbl(d.get("NetStockWeight")) == 0) throw new IllegalArgumentException("Stock Weight Field is Required" + at);
            if (dbl(d.get("ItemRate")) == 0) throw new IllegalArgumentException("Item Rate Field is Required" + at);
            if (num(d.get("UomScheduleIdRate")) == 0) throw new IllegalArgumentException("Rate UOM Field is Required" + at);
            int refType = num(d.get("RefDocumentTypeId")), refId = num(d.get("RefDocId"));
            if (refType == 0 && refId > 0) throw new IllegalArgumentException("$Ref DocumentType Id not Found Please Check");
            if (refType > 0 && refId == 0) throw new IllegalArgumentException("$RefDocId not Found Please Check");
            if (refType > 0 && dbl(d.get("NetBillWeight")) > dbl(r.get("RefInvoiceWeight"))) throw new IllegalArgumentException("NetBillWeight can not greater than Balance Weight");
            details.add(d);
        }
        var freight = new ArrayList<Map<String, Object>>();
        for (var f : freightGrid) if (toInt(f.get("Transporter")) != 0) {   // :2612
            var x = writes.defaults("Sp_InvPurchaseInvoiceFreight_Insert");
            x.put("TansporterId", toInt(f.get("Transporter"))); x.put("Percentage", dbl(f.get("Percentage"))); x.put("FrQty", dbl(f.get("Qty")));
            x.put("FrRate", dbl(f.get("Rate"))); x.put("FreightAmount", dbl(f.get("Freight"))); x.put("Remarks", str(f.get("Remarks")));
            freight.add(x);
        }
        var expenses = new ArrayList<Map<String, Object>>();
        for (var e : expGrid) if (toInt(e.get("ItemId")) != 0) {   // :2627
            var x = writes.defaults("Sp_InvPurchaseInvoiceExpense_Insert");
            x.put("GdnId", toInt(e.get("GdnId"))); x.put("GdnExpId", toInt(e.get("GdnExpId"))); x.put("InvRevExpItemId", toInt(e.get("ItemId")));
            x.put("Qty", dbl(e.get("Qty"))); x.put("Rate", dbl(e.get("Rate"))); x.put("Amount", dbl(e.get("Amount")));
            String remarks = str(e.get("Remarks"));
            if (remarks.equals("0") || remarks.isEmpty()) remarks = "Other Item:  " + str(e.get("ItemName"));
            x.put("Remarks", remarks); expenses.add(x);
        }
        var journal = new ArrayList<Map<String, Object>>();
        for (var j : glGrid) if (toInt(j.get("AccountId")) != 0) {   // :2645
            var x = writes.defaults("Sp_InvPurchaseInvoiceJournal_Insert");
            x.put("ChartofAccountId", toInt(j.get("AccountId"))); x.put("JvRemarks", str(j.get("Remarks"))); x.put("JvPrcnt", dbl(j.get("Percentage")));
            x.put("JvQty", dbl(j.get("Qty"))); x.put("JvRate", dbl(j.get("Rate"))); x.put("JvDebit", dbl(j.get("Debit"))); x.put("JvCredit", dbl(j.get("Credit")));
            journal.add(x);
        }

        // BLL 0581 Save: voucher first, then the insert-with-detail-id check
        var accounts = writes.accounts(h, details);
        var voucher = PurchaseInvoiceReturnFinancialRules.calculate(h, details, freight, journal, accounts);
        if (!update && details.stream().anyMatch(d -> num(d.get("Id")) > 0)) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");

        var attachmentChange = p.get("attachments") == null ? null : attachments.prepare(recId, TYPE, p.get("attachments"));
        if (attachmentChange != null) { h.put("AttachmentsValues", attachmentChange.names()); h.put("CustomAttachmentsValues", attachmentChange.storedNames()); }

        // DAL 0434 SetData
        boolean gdn = details.stream().anyMatch(d -> num(d.get("GdnId")) > 0);                                  // D:39
        h.put("InvoiceQty", sum(details, "ItemQty")); h.put("InvoiceWeight", sum(details, "NetBillWeight"));      // D:40-41
        int newId = writes.execute(update ? "Sp_InvPurchaseInvoice_Update" : "Sp_InvPurchaseInvoice_Insert", h);
        int id = newId > 0 ? newId : recId;
        if (id <= 0) throw new IllegalStateException("Invoice procedure did not return an ID");
        h.put("Id", id);
        boolean wagesToProduct = truthy(repo.config("ContractWagesChargetoProduct"));
        int line = 0; var ids = new ArrayList<String>();
        for (var d : details) {
            d.put("LineId", ++line);
            d.put("BillAmount", dbl(d.get("ItemAmount")) + dbl(d.get("FreightAmount")) + dbl(d.get("ExpenseAmount")) + dbl(d.get("CommissionAmount")) + dbl(d.get("Brokery"))
                    + (wagesToProduct ? dbl(d.get("WagesAmount")) : 0) - dbl(d.get("EbPurAgainstWeightAmount")) - dbl(d.get("FreightDeduction")));   // D:76-86
            d.put("InvPurchaseInvoiceId", id);
            d.put("Id", writes.execute("Sp_InvPurchaseInvoiceDetail_Insert", d));
            ids.add(String.valueOf(num(d.get("Id"))));
        }
        for (var x : freight) { x.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceFreight_Insert", x); }
        for (var x : journal) { x.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceJournal_Insert", x); }
        for (var x : expenses) { x.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceExpense_Insert", x); }
        int org = context.currentOrganizationId(), company = context.currentCompanyId();
        ProcExec.run(jdbc, "EXEC dbo.usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations @OrganizationId=?,@CompanyId=?,@Id=?,@DetailIds=?", org, company, id, String.join(",", ids) + ",");
        // (type 59 skips usp_StockInTransit_EvaluationAndVoucherDelete_ByPurchaseInvoiceId, D:150)
        if (attachmentChange != null) attachments.persist(id, TYPE, supplier, attachmentChange);
        var vdetails = new ArrayList<>(voucher.details());
        if (Boolean.TRUE.equals(flags.get("feature5"))) costOfSales(h, details, vdetails, gdn, accounts, String.valueOf(voucher.header().get("_CompanyName")));   // D:193-469
        else ProcExec.run(jdbc, "EXEC dbo.Sp_InventoryStockEvalautionDetail_Update @OrganizationId=?,@CompanyId=?,@RefDocumentTypeId=?,@RefDocIdNo=?", org, company, TYPE, id);   // D:470
        var vh = voucher.header(); vh.remove("_CompanyName");
        int previous = repo.voucherHeadId(id);
        vh.put("Id", previous); vh.put("DocumentTypeSrNo", id); vh.put("RefDocNoId", id);
        int voucherId = writes.execute(previous > 0 ? "Sp_VoucherHead_Update" : "Sp_VoucherHead_Insert", vh);
        if (voucherId <= 0) voucherId = previous;
        if (voucherId <= 0) throw new IllegalStateException("Voucher procedure did not return an ID");
        vh.put("Id", voucherId);
        if (vdetails.isEmpty()) throw new IllegalArgumentException("VoucherDetail list Not Found");   // D:575
        for (var row : vdetails) { row.put("VoucherHeadId", voucherId); row.put("BranchesId", h.get("BranchesId")); writes.execute("Sp_VoucherDetail_Insert", row); }
        ProcExec.run(jdbc, "EXEC dbo.USP_VoucherBalanceCheck @OrganizationId=?,@CompanyId=?,@Id=?", org, company, voucherId);
        int historyId = writes.execute("Sp_VoucherHead_H_Insert", vh);
        for (var row : vdetails) { row.put("VoucherHeadId", voucherId); row.put("DocumentTypeIdRef", historyId); writes.execute("Sp_VoucherDetail_H_Insert", row); }
        ProcExec.run(jdbc, "EXEC dbo.Sp_InventoryTransactions_GetALLMethod @OrganizationId=?,@CompanyId=?,@RefDocumentTypeId=?,@RefDocIdNo=?", org, company, TYPE, id);   // D:524
        if (!gdn) for (var d : details)   // D:533
            ProcExec.run(jdbc, "EXEC dbo.USP_InventoryValidation @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@DocDate=?,@ItemId=?,@WarehouseId=?,@JobLotId=?,@CropYear=?,@InvPackingTypeId=?,@PackUomId=?,@NetWeight=?",
                    org, company, TYPE, new SqlParameterValue(Types.TIMESTAMP, ts(h.get("DocDate"))), num(d.get("ItemId")), num(d.get("WarehouseId")), num(d.get("JobLotId")), str(d.get("CropYear")),
                    num(d.get("PackingTypeId")), num(d.get("ItemUOMId")), dbl(d.get("NetStockWeight")));
        ProcExec.run(jdbc, "EXEC DAW.USp_DocumentApprovalDetail_Insert @OrganizationId=?,@CompanyId=?,@DocumentTypeId=?,@Id=?,@LimitAmount=?", org, company, TYPE, id, billAmount);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true); out.put("id", id); out.put("docNo", docNoText);
        out.put("message", (update ? "Record Update Successfully [" : "Record Saved Successfully [") + docNoText + "] ");
        out.put("voucherHeadId", voucherId);
        out.put("openWagesBill", num(grid.get(0).get("GdnId")) == 0 && Boolean.TRUE.equals(flags.get("wagesStatus")) && Boolean.TRUE.equals(flags.get("wagesActiveOrInActive")));
        out.put("grossWeightTotal", grossTotal);
        out.put("supplierCustomerId", supplier); out.put("docDate", str(p.get("DocDate")));
        return out;
    }
    private static Timestamp ts(Object d) {
        if (d instanceof Timestamp t) return t;
        if (d instanceof java.util.Date x) return new Timestamp(x.getTime());
        String s = str(d); if (s.isEmpty()) return new Timestamp(System.currentTimeMillis());
        return Timestamp.valueOf(s.length() >= 10 ? s.substring(0, 10) + " 00:00:00" : s);
    }

    /** DAL 0434:193-469 - ERP feature 5 (FIFO costing) for a purchase return. */
    private void costOfSales(Map<String, Object> h, List<Map<String, Object>> details, List<Map<String, Object>> vdetails, boolean gdn,
                             PurchaseInvoiceFinancialRules.Accounts accounts, String companyName) {
        int org = context.currentOrganizationId(), company = context.currentCompanyId(), id = num(h.get("Id"));
        boolean inactive = truthy(repo.config("InventoryFinancialsEffectsInActive"));   // flag4
        Map<Integer, Map<String, Object>> items = new LinkedHashMap<>();
        for (var r : repo.itemGlIds()) items.putIfAbsent(num(r.get("Id")), r);
        List<Integer> purchaseGl = new ArrayList<>();
        double num9 = 0;
        if (!inactive) {
            Set<Integer> unique = new LinkedHashSet<>(); for (var d : details) unique.add(num(d.get("ItemId")));
            for (var e : items.entrySet()) if (unique.contains(e.getKey())) purchaseGl.add(num(e.getValue().get("PurchaseGLAC")));
            double dr = 0, cr = 0;
            for (var v : vdetails) if (purchaseGl.contains(num(v.get("AccountId")))) { dr += dbl(v.get("DebitAmount")); cr += dbl(v.get("CreditAmount")); }
            num9 = dr - cr;
            vdetails.removeIf(v -> purchaseGl.contains(num(v.get("AccountId"))));
        }
        List<Map<String, Object>> evaluation = new ArrayList<>();
        if (!gdn) {
            for (var d : details) {
                var item = items.get(num(d.get("ItemId")));
                if (item == null) continue;
                String itemName = str(item.get("ItemName"));
                for (var x : fifo(h, d, itemName, evaluation)) {
                    if (!inactive) {
                        var v = PurchaseInvoiceFinancialRules.copy(null);
                        v.put("LineId", d.get("LineId")); v.put("IsCGS", 1); v.put("AccountId", num(item.get("PurchaseGLAC"))); v.put("AgainstAccountId", num(item.get("COGSGLAC")));
                        v.put("Comments", "ItemQty: " + PurchaseInvoiceReturnFinancialRules.cs(dbl(x.get("QtyOut"))) + "   " + itemName + "  Net Weight: " + PurchaseInvoiceReturnFinancialRules.cs(dbl(x.get("StockWeightOut")))
                                + "  CGS Rate:" + PurchaseInvoiceReturnFinancialRules.cs(dbl(x.get("CgsRate"))) + "  " + companyName);
                        v.put("CreditAmount", dbl(x.get("CgsAmount"))); v.put("ItemId", num(d.get("ItemId"))); v.put("QtyOut", dbl(x.get("QtyOut"))); v.put("WeightOut", dbl(x.get("StockWeightOut")));
                        v.put("ItemCgsRate", dbl(x.get("CgsRate"))); v.put("RateCut", dbl(d.get("RateCut"))); v.put("RateCutAmount", dbl(d.get("RateCutAmount"))); v.put("ItemAmount", dbl(x.get("AmountOut")));
                        v.put("Expenses", dbl(d.get("ExpenseAmount"))); v.put("Commission", dbl(d.get("CommissionAmount"))); v.put("Freight", dbl(d.get("FreightAmount")));
                        v.put("GpNo", num(d.get("GpNo"))); v.put("VehicleNo", d.get("VehicleNo")); v.put("JobLotId", num(d.get("JobLotId"))); v.put("SupplierCustomerId", num(h.get("SupplierCustomerId")));
                        vdetails.add(v);
                    }
                    evaluation.add(x);
                }
            }
            if (!evaluation.isEmpty()) {
                // ModifyUser is always the current user on this form, so the reversal always runs (D:275)
                ProcExec.run(jdbc, "EXEC [dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId] @OrganizationId=?,@CompanyId=?,@RefDocumentTypeId=?,@RefDocIdNo=?", org, company, TYPE, id);
                for (var x : evaluation) {
                    stamp(x, h);
                    x.put("CalcType", "Weight");
                    Map<String, Object> d = null;
                    for (var dd : details) if (num(dd.get("LineId")) == num(x.get("LineId")) && num(dd.get("LineId")) > 0 && num(x.get("LineId")) > 0) { d = dd; break; }
                    if (d != null) {
                        double eq = repo.equivalent(num(x.get("ItemId")), num(d.get("UomScheduleIdRate")));
                        if (dbl(x.get("BillWeightOut")) > 0 && dbl(d.get("ItemRate")) > 0 && eq > 0) x.put("AmountOut", dbl(x.get("BillWeightOut")) / eq * dbl(d.get("ItemRate")));
                        x.put("ItemRate", dbl(d.get("ItemRate"))); x.put("RefDocIdNo", num(d.get("InvPurchaseInvoiceId"))); x.put("RefDocSubIdNo", num(d.get("Id")));
                    }
                    repo.setProc("USP_InventoryStockEvalautionDetail_Insert", x);
                }
            }
        } else {
            Map<Integer, Integer> lotAccounts = new HashMap<>();
            for (var r : repo.jobLotGlIds()) lotAccounts.putIfAbsent(num(r.get("Id")), num(r.get("AccountId")));
            for (var d : details) {
                var stock = repo.stockByOtherIds(703, num(d.get("GdnId")), num(d.get("GdnDetailId")));
                if (stock == null || stock.isEmpty()) throw new IllegalArgumentException("InventoryStockEvaluationDetails list not found against CGS other reference");
                boolean lotHasAccount = lotAccounts.getOrDefault(num(d.get("JobLotId")), 0) > 0;
                for (var sd : stock) {
                    var x = ci(sd);
                    var item = items.get(num(d.get("ItemId")));
                    if (item == null) throw new IllegalArgumentException("ItemId Not Found Against CGS Transaction");
                    if (!lotHasAccount && !inactive) {
                        var v = PurchaseInvoiceFinancialRules.copy(null);
                        v.put("LineId", d.get("LineId")); v.put("IsCGS", 1); v.put("AccountId", num(item.get("PurchaseGLAC"))); v.put("AgainstAccountId", num(item.get("COGSGLAC")));
                        v.put("Comments", "ItemQty: " + PurchaseInvoiceReturnFinancialRules.cs(dbl(x.get("QtyOut"))) + "   " + str(item.get("ItemName")) + "  Net Weight: " + PurchaseInvoiceReturnFinancialRules.cs(dbl(x.get("StockWeightOut")))
                                + "  CGS Rate:" + PurchaseInvoiceReturnFinancialRules.cs(dbl(x.get("CgsRate"))) + "  " + companyName);
                        v.put("CreditAmount", dbl(x.get("CgsAmount"))); v.put("ItemId", num(d.get("ItemId"))); v.put("QtyOut", dbl(x.get("QtyOut"))); v.put("WeightOut", dbl(x.get("StockWeightOut")));
                        v.put("ItemCgsRate", dbl(x.get("CgsRate"))); v.put("RateCut", dbl(d.get("RateCut"))); v.put("RateCutAmount", dbl(d.get("RateCutAmount"))); v.put("ItemAmount", dbl(x.get("AmountOut")));
                        v.put("Expenses", dbl(d.get("ExpenseAmount"))); v.put("Commission", dbl(d.get("CommissionAmount"))); v.put("Freight", dbl(d.get("FreightAmount")));
                        v.put("GpNo", num(d.get("GpNo"))); v.put("VehicleNo", d.get("VehicleNo")); v.put("JobLotId", num(d.get("JobLotId"))); v.put("SupplierCustomerId", num(h.get("SupplierCustomerId")));
                        v.put("BranchesId", num(d.get("BranchId")));
                        vdetails.add(v);
                    }
                    stamp(x, h); x.put("CalcType", "Weight");
                    if (num(d.get("GdnId")) == num(x.get("OtherDocNoId")) && num(d.get("GdnDetailId")) == num(x.get("OtherSubDocNoId")) && num(x.get("OtherDocumentTypeId")) == 703) {
                        double eq = repo.equivalent(num(d.get("ItemId")), num(d.get("UomScheduleIdRate")));
                        if (dbl(x.get("BillWeightOut")) > 0 && dbl(d.get("ItemRate")) > 0 && eq > 0) {
                            double amountOut = dbl(x.get("BillWeightOut")) / eq * dbl(d.get("ItemRate")), e = 0, f = 0, c = 0;
                            if (dbl(d.get("ExpenseAmount")) > 0 && dbl(d.get("ItemQty")) > 0 && dbl(x.get("QtyOut")) > 0) e = dbl(d.get("ExpenseAmount")) / dbl(d.get("ItemQty")) * dbl(x.get("QtyOut"));
                            if (dbl(d.get("FreightAmount")) > 0 && dbl(d.get("NetBillWeight")) > 0 && dbl(x.get("BillWeightOut")) > 0) f = dbl(d.get("FreightAmount")) / dbl(d.get("NetBillWeight")) * dbl(x.get("BillWeightOut"));
                            if (dbl(d.get("CommissionAmount")) > 0 && dbl(d.get("ItemAmount")) > 0 && amountOut > 0) c = dbl(d.get("CommissionAmount")) / dbl(d.get("ItemAmount")) * amountOut;
                            x.put("AmountOut", amountOut + (e - f - c));
                        }
                        x.put("ItemRate", dbl(d.get("ItemRate"))); x.put("RefDocIdNo", num(d.get("InvPurchaseInvoiceId"))); x.put("RefDocSubIdNo", num(d.get("Id")));
                    }
                    evaluation.add(x);
                }
            }
            for (var x : evaluation) repo.setProc("USP_InventoryStockEvalautionDetailGdnReferences_Update", x);
        }
        if (!inactive) {   // D:446 "Stock Difference to CGS"
            double num13 = 0; int num14 = 0; boolean first = true;
            for (var v : vdetails) {
                if (num(v.get("IsCGS")) == 1 && purchaseGl.contains(num(v.get("AccountId")))) num13 += dbl(v.get("CreditAmount"));
                if (num(v.get("IsCGS")) == 1 && first) { num14 = num(v.get("AgainstAccountId")); first = false; }
            }
            double num15 = num9 < 0 ? Math.abs(num9) : 0, num16 = num9 > 0 ? num9 : 0;
            double num17 = num13 - (num15 > 0 ? num15 : num16);
            var v = PurchaseInvoiceFinancialRules.copy(null);
            v.put("IsCGS", 1); v.put("AccountId", num14); v.put("AgainstAccountId", purchaseGl.size() == 1 ? purchaseGl.get(0) : num14);
            v.put("Comments", "Stock Difference to CGS");
            if (num17 > 0) v.put("DebitAmount", num17); else v.put("CreditAmount", num17);   // Q-S4: a negative credit, as the desktop
            v.put("BranchesId", h.get("BranchesId"));
            vdetails.add(v);
        }
    }
    private void stamp(Map<String, Object> x, Map<String, Object> h) {
        x.put("OrganizationId", h.get("OrganizationId")); x.put("CompanyId", h.get("CompanyId")); x.put("DocDate", h.get("DocDate")); x.put("DocCodeNo", h.get("DocNo"));
        x.put("SupplierCustomerId", h.get("SupplierCustomerId")); x.put("BranchesId", h.get("BranchesId")); x.put("RefDocumentTypeId", TYPE);
        x.put("EntryUser", h.get("EntryUser")); x.put("ModifyUser", h.get("ModifyUser"));
    }
    /** CommonServices.FIFOImplemention (DAL 0205:725) for one line; reserve = the layers already taken by earlier lines. */
    private List<Map<String, Object>> fifo(Map<String, Object> h, Map<String, Object> d, String itemName, List<Map<String, Object>> reserve) {
        Map<String, Object> q = new LinkedHashMap<>();
        q.put("OrganizationId", h.get("OrganizationId")); q.put("CompanyId", h.get("CompanyId")); q.put("ItemId", num(d.get("ItemId")));
        q.put("DocDate", ts(h.get("DocDate")));
        if (num(d.get("ItemUOMId")) != 0) q.put("PackUomId", num(d.get("ItemUOMId")));
        if (num(d.get("WarehouseId")) != 0) q.put("WarehouseId", num(d.get("WarehouseId")));
        if (num(d.get("JobLotId")) != 0) q.put("JobLotId", num(d.get("JobLotId")));
        if (num(d.get("PackingTypeId")) != 0) q.put("PackingTypeId", num(d.get("PackingTypeId")));
        if (!str(d.get("CropYear")).isEmpty()) q.put("CropYear", str(d.get("CropYear")));
        q.put("DocumentTypeId", TYPE);                       // ModifyUser > 0 on every save of this form (D:227-231)
        if (num(h.get("Id")) != 0) q.put("Id", num(h.get("Id")));
        if (!reserve.isEmpty()) {
            StringBuilder xml = new StringBuilder("<ArrayOfFIFOStockEvaluation>");
            for (var r : reserve) xml.append("<FIFOStockEvaluation><RefDocumentTypeId>").append(num(r.get("RefRefDocumentTypeId"))).append("</RefDocumentTypeId><RefDocIdNo>")
                    .append(num(r.get("RefRefDocIdNo"))).append("</RefDocIdNo><RefDocSubIdNo>").append(num(r.get("RefRefDocSubIdNo"))).append("</RefDocSubIdNo><ReserveQty>")
                    .append(dbl(r.get("QtyOut"))).append("</ReserveQty><ReserveWeight>").append(dbl(r.get("StockWeightOut"))).append("</ReserveWeight></FIFOStockEvaluation>");
            q.put("FIFOXML", xml.append("</ArrayOfFIFOStockEvaluation>").toString());
        }
        var layers = repo.stockByFifo(q);
        if (layers.isEmpty()) throw new IllegalArgumentException("Stock Not Found this Item " + itemName + " against FIFO Method .....");
        double available = 0; for (var l : layers) available += dbl(l.get("NetBalWeight"));
        double need = dbl(d.get("NetStockWeight")), needQty = dbl(d.get("ItemQty"));
        if (!(need <= even(available, 2))) throw new IllegalArgumentException("Weight available is " + PurchaseInvoiceReturnFinancialRules.cs(available) + " and row Weight is " + PurchaseInvoiceReturnFinancialRules.cs(need) + " this item " + itemName + " against FIFO....");
        List<Map<String, Object>> out = new ArrayList<>();
        double usedW = 0, usedQ = 0;
        for (var l : layers) {
            if (dbl(l.get("AvgRate")) <= 0) throw new IllegalArgumentException("Rate Not Found this Item " + itemName + " against FIFO Method");
            if (num(l.get("RateUomId")) == 0) throw new IllegalArgumentException("RateUomId not found  this " + itemName + " against FIFO Method");
            double w = dbl(l.get("NetBalWeight")), qn = dbl(l.get("NetBalQty"));
            double eq = repo.equivalent(num(d.get("ItemId")), num(l.get("RateUomId")));
            if (eq == 0) throw new IllegalArgumentException("RateUom Not Found");
            var x = new TreeMap<String, Object>(String.CASE_INSENSITIVE_ORDER);
            x.put("Id", num(l.get("Id"))); x.put("LineId", num(d.get("LineId"))); x.put("ItemId", num(d.get("ItemId"))); x.put("WarehouseId", num(d.get("WarehouseId")));
            x.put("RateUom", num(l.get("RateUomId"))); x.put("JobLotId", num(d.get("JobLotId"))); x.put("InvPackingTypeId", num(d.get("PackingTypeId"))); x.put("ItemUom", num(d.get("ItemUOMId")));
            x.put("CropBatch", str(d.get("CropYear")));
            x.put("RefRefDocumentTypeId", num(l.get("RefDocumentTypeId"))); x.put("RefRefDocIdNo", num(l.get("RefDocIdNo"))); x.put("RefRefDocSubIdNo", num(l.get("RefDocSubIdNo")));
            double takeQ, takeW;
            if (w <= need - usedW) { takeQ = qn; takeW = w; }
            else if (w >= need - usedW) { takeQ = needQty - usedQ; takeW = need - usedW; }
            else continue;
            x.put("QtyOut", takeQ); x.put("BillWeightOut", takeW); x.put("StockWeightOut", takeW);
            double cgsRate = dbl(l.get("AvgRate")) * eq;
            x.put("CgsRate", cgsRate); x.put("CgsAmount", takeW / eq * cgsRate);
            usedQ += takeQ; usedW += takeW;
            out.add(x);
            if (need == usedW) break;
        }
        return out;
    }

    // ------------------------------------------------------------------ read :2746
    public Map<String, Object> read(int id) {
        repo.requireRight("View");
        var old = repo.header(id);
        if (old == null || (num(old.get("EntryUser")) != context.currentUserId() && !repo.hasRight("CanView AllRecord"))) throw new IllegalArgumentException("Record not found");
        var h = repo.read(id);
        if (h == null) throw new IllegalArgumentException("Record not found");
        return h;
    }
    public List<Map<String, Object>> uoms(int itemId) { repo.requireRight("View"); return repo.uoms(itemId); }
    public double currentStock(int warehouseId, int itemId, int jobLotId, String cropYear, String docDate, int packingTypeId, int packUomId) {
        repo.requireRight("View");
        return repo.currentStock(warehouseId, itemId, jobLotId, cropYear, docDate, packingTypeId, packUomId);
    }

    // ------------------------------------------------------------------ history
    public List<Map<String, Object>> history(String dateMode, String fromDate, String toDate, int fromDocNo, int toDocNo, int supplierId) {
        repo.requireRight("View");
        return repo.history(dateMode, fromDate, toDate, fromDocNo, toDocNo, supplierId, repo.hasRight("CanView AllRecord"));
    }
    public List<Map<String, Object>> historySuppliers(String branchesIds) { repo.requireRight("View"); return repo.historySuppliers(branchesIds); }
    public List<Map<String, Object>> historyBranches() { repo.requireRight("View"); return repo.historyBranches(truthy(repo.config("PurchaseInvoiceReturnBranchWise"))); }

    // ------------------------------------------------------------------ loader (frmLoadPurchaseInvoiceForReturn)
    public Map<String, Object> loaderInit() {
        repo.requireRight("View");
        boolean branchImplemented = truthy(repo.config("PurchaseInvoiceReturnBranchWise"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branchImplemented", branchImplemented);
        m.put("branches", repo.loaderBranches(branchImplemented));
        m.put("fromDate", repo.financialYearStart());
        m.put("toDate", java.time.LocalDate.now().toString());
        m.put("userBranchName", repo.userBranchName());
        return m;
    }
    public List<Map<String, Object>> pending(String fromDate, String toDate, String branchesIds) {
        repo.requireRight("View");
        if (str(branchesIds).isEmpty()) throw new IllegalArgumentException("Select branch first");
        return repo.pendingInvoices(fromDate, toDate, branchesIds);
    }
    /**
     * L:BtnLoad - the checked rows are re-read from the same pending query (not trusted from the browser), checked in grid
     * order exactly as the dialog does, and every pending line of the checked invoices is returned (dtGrid rows whose Id
     * is among the checked ones).
     */
    public List<Map<String, Object>> load(List<Map<String, Object>> checked, String fromDate, String toDate, String branchesIds) {
        repo.requireRight("View");
        if (checked == null || checked.isEmpty()) throw new IllegalArgumentException("Chek the row first");
        boolean branchImplemented = truthy(repo.config("PurchaseInvoiceReturnBranchWise"));
        var all = pending(fromDate, toDate, branchesIds);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (var c : checked) {
            for (var r : all) if (num(r.get("Id")) == num(c.get("Id")) && num(r.get("DetailId")) == num(c.get("DetailId"))) { rows.add(r); break; }
        }
        if (rows.isEmpty()) throw new IllegalArgumentException("Chek the row first");
        int sId = 0, docId = 0, branchId = 0; Set<Integer> ids = new LinkedHashSet<>();
        for (var r : rows) {
            int s = num(r.get("SupplierCustomerId"));
            if (s != 0) { if (sId == 0) sId = s; if (sId != s) throw new IllegalArgumentException("Sorry!. The Selected Invoices are not of same Supplier"); }
            int t = num(r.get("DocumentTypeId"));
            if (t != 0) { if (docId == 0) docId = t; if (docId != t) throw new IllegalArgumentException("Sorry!. The Selected Invoices are not of same DocumentType"); }
            if (branchImplemented) { int b = num(r.get("BranchId")); if (b != 0) { if (branchId == 0) branchId = b; if (branchId != b) throw new IllegalArgumentException("Sorry!. The Selected Invoices are not of same Branch"); } }
            ids.add(num(r.get("Id")));
            sId = s; docId = t; branchId = num(r.get("BranchId"));
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : all) if (ids.contains(num(r.get("Id")))) out.add(r);
        return out;
    }
}
