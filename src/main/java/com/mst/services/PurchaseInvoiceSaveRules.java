package com.mst.services;

import com.mst.repositories.PurchaseInvoiceLookupRepository;
import com.mst.repositories.PurchaseInvoiceRecordRepository;
import com.mst.security.CurrentUserContext;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.util.*;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/**
 * InvfrmPurchaseInvoice (DocumentTypeId 56) Insert():3451-3990 and btnUpdate_Click:4012 - the form-side checks, in the
 * desktop order and with the desktop texts, and the row filtering/remark building the form does before
 * InvPurchaseInvoice.Save. Runs before the shared {@link PurchaseInvoicePersistenceService}, which keeps its own checks.
 */
@Service
public class PurchaseInvoiceSaveRules {
    private final PurchaseInvoiceLookupRepository lookups;
    private final PurchaseInvoiceRecordRepository records;
    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;

    public PurchaseInvoiceSaveRules(PurchaseInvoiceLookupRepository lookups, PurchaseInvoiceRecordRepository records, JdbcTemplate jdbc, CurrentUserContext context) {
        this.lookups = lookups; this.records = records; this.jdbc = jdbc; this.context = context;
    }

    private static IllegalArgumentException fail(String message) { return new IllegalArgumentException(message); }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Map<String, Object> p, String key) {
        Object v = p.get(key);
        if (v == null) return new ArrayList<>();
        if (!(v instanceof List<?>)) throw fail(key + " must be a list");
        var out = new ArrayList<Map<String, Object>>();
        for (Object row : (List<?>) v) { if (!(row instanceof Map<?, ?>)) throw fail("Invalid " + key + " row"); out.add(copy((Map<String, Object>) row)); }
        return out;
    }

    private static boolean truthy(Object v) { return v instanceof Boolean ? (Boolean) v : v instanceof Number ? ((Number) v).intValue() != 0 : "true".equalsIgnoreCase(Objects.toString(v, "")) || "1".equals(Objects.toString(v, "")); }

    /** C# double.ToString() in the remark texts: 5 -> "5", 2.5 -> "2.5". */
    private static String cs(double v) { return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString(); }

    /** "#,##0.####" of ShowPaymentDetailError:3935. */
    private static String money(double v) { var f = new DecimalFormat("#,##0.####", DecimalFormatSymbols.getInstance(Locale.US)); f.setRoundingMode(RoundingMode.HALF_UP); return f.format(v); }

    /** FormHelper.ValidateField (text as in the sibling ports of FormHelper). */
    private static void field(boolean missing, String name, int index) { if (missing) throw fail(name + " is required in Detail Grid at row No: " + (index + 1)); }

    /** btnUpdate_Click:4016 / btnDelete_Click:4222 - the same text for both. */
    public void requireNotApproved(int id) {
        if (truthy(records.require(id, 56).get("IsApproved"))) throw fail("Record Not Update because Record has approved");
    }

    public Map<String, Object> prepare(Map<String, Object> supplied) {
        var p = copy(supplied);
        int id = i(p, "Id");
        boolean update = id > 0;
        if (update) requireNotApproved(id);
        var details = list(p, "details");
        var freight = list(p, "freight");
        var journal = list(p, "journal");
        var expenses = list(p, "expenses");
        var bags = list(p, "emptyBags");
        var terms = list(p, "paymentTerms");
        int org = context.currentOrganizationId(), company = context.currentCompanyId();
        boolean subsidiary = lookups.subsidiary();
        boolean freightToExpense = lookups.enabled("DebitAmountChargetoExpenseAcFreightGridPurchase");

        // :3488-3503
        if (details.isEmpty()) throw fail("Grid Record Not Found");
        if (i(p, "SupplierCustomerId") <= 0) throw fail("Supplier Name field is required");
        if (!jdbc.queryForList("EXEC dbo.usp_getLocationType").isEmpty() && i(p, "LocationTypeId") <= 0) throw fail("Location Type field is required");
        // :3504-3509 - and TransportAccountId/TransporterCreditPartyId are only sent with a deduction (:3625-3634).
        double deduction = n(p, "FreightAmount");
        int deductionAccount = subsidiary ? i(p, "TransporterCreditPartyId") : i(p, "TransportAccountId");
        if (deduction > 0 && deductionAccount == 0) throw fail("Freight Deduction Debit Account Field Is Required");
        if (!(deduction > 0)) { p.put("TransportAccountId", 0); p.put("TransporterCreditPartyId", 0); }
        if (!subsidiary) p.put("TransporterCreditPartyId", 0);
        // :3510-3513 - GrnDate is the loaded GRN's date (LoadDataDetailGridAgainstGP:6175-6179).
        if (!update && lookups.enabled("ValidateGrnAndInvoiceDateWithGpDate")) {
            var grn = jdbc.queryForList("SELECT CONVERT(date,DocDate) AS DocDate FROM dbo.InvGrn WHERE Id=? AND OrganizationId=? AND CompanyId=?", i(details.get(0), "InvGrnId"), org, company);
            String grnDate = grn.isEmpty() ? "" : Objects.toString(grn.get(0).get("DocDate"), "");
            if (!s(p, "DocDate").startsWith(grnDate) || grnDate.isEmpty()) throw fail("Invoice Date Should be Equal to GrnDate");
        }

        // The form recalculates (TotalCommissionAmount..BillAmount :3514-3522) before its checks.
        var h = copy(p);
        var supplierRow = jdbc.queryForList("SELECT GlAccountId FROM dbo.SupplierCustomer WHERE Id=? AND OrganizationId=? AND CompanyId=?", i(p, "SupplierCustomerId"), org, company);
        if (supplierRow.isEmpty()) throw fail("Supplier Name field is required");
        int supplierGl = i(copy(supplierRow.get(0)), "GlAccountId");
        var calcDetails = details.stream().map(PurchaseInvoiceFinancialRules::copy).toList();
        PurchaseInvoiceCalculations.bill(h, calcDetails, expenses, freight, journal, bags, supplierGl,
                new PurchaseInvoiceCalculations.Configuration(lookups.amountDigits(), freightToExpense, lookups.enabled("WagesAmountCalculateOnQty"), lookups.enabled("ContractWagesChargetoProduct"), subsidiary));

        // :3523-3551 freight grid
        double totalCredit = 0, totalDebit = 0;
        for (var f : freight) {
            totalCredit += n(f, "FreightAmount"); totalDebit += n(f, "Debit");
            if (!(n(f, "FreightAmount") > 0)) continue;
            if ((subsidiary ? i(f, "SupplierCustomerId") : i(f, "TansporterId")) == 0) throw fail("Please Select an Account Against Freight First");
            if (s(f, "Remarks").isEmpty()) throw fail("Grid Charge to Product Remarks required Please Check");
        }
        if (freightToExpense && (totalCredit > 0 || totalDebit > 0) && totalCredit != totalDebit) throw fail("Freight Debit Amount not equal to Credit Amount Please Check");
        if (!freightToExpense && totalDebit > 0) throw fail("Freight Debit Row Remove first");
        // :3552-3558 Supplier Add/Less
        for (var j : journal) {
            int account = subsidiary ? i(j, "SupplierCustomerId") : i(j, "ChartofAccountId");
            if ((n(j, "JvCredit") > 0 || (int) n(j, "JvDebit") > 0) && account == 0) throw fail("Please Select an Account Against JL First");
        }
        // :3559-3565 other expenses
        for (var e : expenses) if (n(e, "Amount") > 0 && i(e, "InvRevExpItemId") == 0) throw fail("Please Select an Item Against Expense First");
        // :3566-3572 empty bags
        for (var b : bags) if (i(b, "ItemId") > 0 && n(b, "PurchaseQty") > 0 && n(b, "Amount") == 0) throw fail("Packing Material Amount Required");
        // :3573-3576
        if (n(h, "CommAmount") > 0 && i(p, "CommissionAgentId") == 0) throw fail("Please Select Commission Agent Account First");
        // :3610-3614
        if (i(p, "BrokerAgentId") > 0 && n(h, "BrokeryAmount") > 0 && i(p, "BrokerAgentId") == i(p, "SupplierCustomerId")) throw fail("The broker and party account cannot be equal");
        // :3587-3593 CommissionRemarks default
        if (s(p, "CommissionRemarks").trim().isEmpty()) p.put("CommissionRemarks", " CommRate:   " + cs(n(p, "CommRate")) + "  CommType: " + s(p, "CommissionType").trim());

        // :3654-3676 detail rows (FillDetailListCommonForInsertAndDelete + ValidateField order)
        for (int r = 0; r < details.size(); r++) {
            var d = details.get(r);
            field(i(d, "InvGrnDetailId") == 0, "InvGrnDetailId", r);
            field(i(d, "InvGrnId") == 0, "InvGrnId", r);
            field(i(d, "WarehouseId") == 0, "WareHouse", r);
            field(i(d, "ItemId") == 0, "Item", r);
            field(s(d, "CropYear").trim().isEmpty(), "CropYear", r);
            field(i(d, "PackingTypeId") == 0, "PackingType", r);
            field(i(d, "JobLotId") == 0, "JobLot", r);
            field(i(d, "ItemUOMId") == 0, "ItemUom", r);
            field(n(d, "ItemQty") <= 0, "ItemQty", r);
            field(n(d, "GrossWeight") <= 0, "GrossWeight", r);
            field(n(d, "NetBillWeight") <= 0, "NetBillWeight", r);
            field(n(d, "NetStockWeight") <= 0, "NetStockWeight", r);
            field(n(d, "ItemRate") <= 0, "Item Rate", r);
            field(n(d, "ItemAmount") <= 0, "ItemAmount", r);
        }

        // :3677-3697 Charge to Product rows: only rows with an account are sent.
        var keptFreight = new ArrayList<Map<String, Object>>();
        for (var f : freight) if ((subsidiary ? i(f, "SupplierCustomerId") : i(f, "TansporterId")) != 0) keptFreight.add(f);
        // :3698-3726 expenses: CustomRemarks = typed text; Remarks = "ItemName : <item>  Qty: q Rate: r " + text.
        var items = new HashMap<Integer, String>();
        for (var raw : jdbc.queryForList("EXEC dbo.Sp_InventoryItemsOther_GetAllMethod @OrganizationId=?,@CompanyId=?,@Activity='ReadAll'", org, company)) {
            var r = copy(raw); items.put(i(r, "Id"), r.containsKey("OtherItemName") ? s(r, "OtherItemName") : s(r, "ItemName"));
        }
        var keptExpenses = new ArrayList<Map<String, Object>>();
        for (var e : expenses) {
            if (i(e, "InvRevExpItemId") == 0) continue;
            String text = (e.containsKey("CustomRemarks") ? s(e, "CustomRemarks") : s(e, "Remarks")).trim();
            e.put("CustomRemarks", text);
            e.put("Remarks", n(e, "Qty") > 0 && n(e, "Rate") > 0 ? "ItemName : " + items.getOrDefault(i(e, "InvRevExpItemId"), "").trim() + "  " + "Qty: " + cs(n(e, "Qty")) + " " + "Rate: " + cs(n(e, "Rate")) + " " + text : text);
            keptExpenses.add(e);
        }
        // :3727-3772 Supplier Add/Less: rate-cut rows (RowType 1) always, others with an account and an amount.
        Map<Integer, String> titles = new HashMap<>();
        if (!journal.isEmpty()) {
            @SuppressWarnings("unchecked") var accounts = (List<Map<String, Object>>) lookups.all().get("accounts");
            for (var raw : accounts) { var a = copy(raw); titles.put(subsidiary ? i(a, "Id") : i(a, "ChartOfAccountId"), subsidiary ? s(a, "CompanyName") : s(a, "AccountTitle")); }
        }
        var keptJournal = new ArrayList<Map<String, Object>>();
        for (var j : journal) {
            int account = subsidiary ? i(j, "SupplierCustomerId") : i(j, "ChartofAccountId");
            int rowType = i(j, "RowType");
            if (rowType == 1 && account == 0 && n(j, "JvCredit") > 0) throw fail("Excess Weight Row in Supplier/Add Less Grid must have Account!");
            boolean keep = rowType == 1 || (account > 0 && (n(j, "JvDebit") > 0 || n(j, "JvCredit") > 0));
            if (!keep) continue;
            if (s(j, "JvRemarks").isEmpty()) j.put("JvRemarks", titles.getOrDefault(account, account == 0 ? "" : String.valueOf(account)));
            keptJournal.add(j);
        }
        // :3773-3836 empty bags. EmptyBagPurAgWtPropotionate = CalculateEbPurAgainstWeightAmountBasedOnBalQty():2375-2493.
        boolean anyCredit = false; double againstQty = 0, againstAmount = 0;
        for (var b : bags) if (Set.of(2, 3).contains(i(b, "TypeId"))) { anyCredit |= n(b, "CreditAccountId") > 0; againstQty += n(b, "PurchaseQty"); againstAmount += n(b, "Amount"); }
        boolean proportionate = !anyCredit && againstQty > 0 && againstAmount > 0;
        var keptBags = new ArrayList<Map<String, Object>>();
        for (int r = 0; r < bags.size(); r++) {
            var b = bags.get(r);
            if (i(b, "ItemId") == 0) continue;
            int type = i(b, "TypeId");
            if (type == 1) b.put("CreditAccountId", 0);
            else if ((type == 2 || type == 3) && i(b, "CreditAccountId") <= 0 && !proportionate) throw fail("Credit Account Required in Empty Bag Grid in Row#" + (r + 1));
            if (i(b, "ItemConditionId") == 0) throw fail("Item Condition Required in Empty Bag Grid in Row#" + (r + 1));
            if (type == 1 && n(b, "Amount") == 0) throw fail("Packing Material Amount Required");
            String text = (b.containsKey("CustomRemarks") ? s(b, "CustomRemarks") : s(b, "Remarks"));
            b.put("CustomRemarks", text);
            // Desktop quirk: r9.Cells["ItemId"].Text of the hidden id column, i.e. the item's number, not its name.
            b.put("Remarks", n(b, "PurchaseQty") > 0 && n(b, "Rate") > 0 ? "ItemName : " + i(b, "ItemId") + "  " + "Qty: " + cs(n(b, "PurchaseQty")) + " " + "Rate: " + cs(n(b, "Rate")) + " " + text : text);
            keptBags.add(b);
        }

        // :3837-3940 payment detail
        boolean byPercent = !Boolean.FALSE.equals(supplied.get("paymentByPercent"));
        var payment = PurchaseInvoicePaymentRules.calculate(h, calcDetails, terms, byPercent, false);
        boolean system = payment.stream().anyMatch(r -> truthy(r.get("SystemGeneratedRow")));
        double paymentTotal = 0;
        if (!system) {
            if (payment.stream().anyMatch(r -> n(r, "PrcntOfTotal") > 0 || n(r, "Amount") > 0))
                for (int r = 0; r < payment.size(); r++) if (n(payment.get(r), "PrcntOfTotal") <= 0 || n(payment.get(r), "Amount") <= 0) throw fail("Payment detail: All rows must have both % and Amount. Error at row# " + (r + 1));
            for (int r = 0; r < payment.size(); r++) {
                int term = i(payment.get(r), "PaymentTermId");
                if (term <= 0) throw fail("Payment Term Required in row# " + (r + 1));
                if (term == 2 && i(payment.get(r), "DueDays") <= 0) throw fail("Due Days Required in case of Credit in row# " + (r + 1));
            }
            var byOrder = new LinkedHashMap<Integer, double[]>(); var orderNo = new HashMap<Integer, Integer>();
            for (var r : payment) { byOrder.computeIfAbsent(i(r, "PurchaseOrderId"), k -> new double[1])[0] += n(r, "PrcntOfTotal"); orderNo.putIfAbsent(i(r, "PurchaseOrderId"), i(r, "PurchaseOrderNo")); }
            for (var e : byOrder.entrySet()) if (BigDecimal.valueOf(100).subtract(BigDecimal.valueOf(e.getValue()[0]).setScale(4, RoundingMode.HALF_EVEN)).abs().compareTo(new BigDecimal("0.05")) > 0)
                throw fail("Percent Of SaleOrder:" + orderNo.get(e.getKey()) + " not near to 100. Please Check!");
        }
        for (var r : payment) paymentTotal += n(r, "Amount");
        if (Math.abs(paymentTotal - n(h, "BillAmount")) > 0.99) throw fail("Payment Detail Amount:" + money(paymentTotal) + " Not Equal to Bill Amount:" + money(n(h, "BillAmount")));

        p.put("details", details);
        p.put("freight", keptFreight);
        p.put("journal", keptJournal);
        p.put("expenses", keptExpenses);
        p.put("emptyBags", keptBags);
        p.put("paymentTerms", terms);
        return p;
    }
}
