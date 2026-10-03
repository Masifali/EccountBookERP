package com.mst.services;

import java.math.BigDecimal;
import java.util.*;

import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/**
 * Purchase Invoice Direct (InvfrmPurchasedirectInvoice, screen 117, DocumentTypeId 57): the desktop refusals of
 * FormValidation (:1444-1470), Insert (:3075-3465), btnUpdate_Click (:3530) and btnDelete_Click (:3699), checked in
 * the desktop's order and with its words, and the Insert() mapping of the four supplement grids (Charge to Product,
 * Other Expenses, Supplier AddLess, EmptyBags) to the InvPurchaseInvoiceFreight / Expense / Journal / EmptyBags rows
 * (:3327-3457). The page sends the grid rows in the desktop grid's own shape:
 * <ul>
 *   <li>freight: SupplierCustomerId = "Transporter" value, TansporterId = hidden GlAccountId, Percentage, FrQty, FrRate,
 *       FreightAmount = "Freight" (Credit), Debit, Remarks, PurchaseOrderId (OrderId), FreightId;</li>
 *   <li>expenses: Id, PurchaseOrderId, PurchaseOrderSupplierExpId, InvRevExpItemId ("ItemId"), Qty, Rate, Amount, CustomRemarks ("Remarks");</li>
 *   <li>journal: AccountId (value list: party id with subsidiary accounts, else chart account id), GlAccountId, JvRemarks,
 *       JvPrcnt, JvQty, JvRate, JvDebit, JvCredit;</li>
 *   <li>emptyBags: PurchaseOrderId, TypeId ("Type"), ItemId, ItemConditionId, ReceivedQty, PurchaseQty, Rate, Amount,
 *       CustomRemarks ("Remarks"), CreditAccountId.</li>
 * </ul>
 * Only the direct form uses this class.
 */
final class PurchaseDirectInvoiceDesktopRules {
    private PurchaseDirectInvoiceDesktopRules() {}

    /** btnUpdate_Click :3534 / btnDelete_Click :3710 - the Approved flag comes from ReadById (:3588 Approved = invPI.IsApproved). */
    static boolean approved(Map<String, Object> stored) {
        Object v = stored == null ? null : stored.get("IsApproved");
        return v instanceof Boolean b ? b : v instanceof Number x ? x.intValue() != 0 : "true".equalsIgnoreCase(Objects.toString(v, "")) || "1".equals(Objects.toString(v, ""));
    }

    /** The journal row's "AccountId" cell: with subsidiary accounts the party id (SupplierCustomerId), else the chart account id. */
    static int journalAccount(Map<String, Object> j) {
        int account = i(j, "AccountId");
        if (account != 0) return account;
        return i(j, "SupplierCustomerId") > 0 ? i(j, "SupplierCustomerId") : i(j, "ChartofAccountId");
    }

    /**
     * BillAmount :4082-4088 compares the "AccountId" cell with txtSupplierGLId (the supplier's GL). With subsidiary accounts
     * the cell holds a party id, so the desktop compares a party id with a GL id (kept as it is).
     */
    static void journalSupplier(List<Map<String, Object>> journal, int supplierGl) {
        for (var j : journal) { int account = journalAccount(j); if (account > 0 && account == supplierGl) throw new IllegalArgumentException("You cannot select supplier Account"); }
    }

    /** Conversion.ToInt of a double (Convert.ToInt32: banker's rounding). */
    static int toInt(double v) { return (int) Math.rint(v); }

    /** A double as C# string interpolation writes it ({PE.Qty}, {PE.Rate}). */
    static String net(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return Long.toString((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    /**
     * btn Update/Save row (grd_ColumnButtonClick "+" copies a row with its Id): Sp_InvPurchaseInvoiceDetail_Insert updates
     * by Id, so two rows with the same stored Id end as one row - the last one written. Kept here by keeping the last copy.
     */
    static List<Map<String, Object>> collapseRepeatedIds(List<Map<String, Object>> details) {
        var last = new HashMap<Integer, Integer>();
        for (int k = 0; k < details.size(); k++) { int id = i(details.get(k), "Id"); if (id > 0) last.put(id, k); }
        var out = new ArrayList<Map<String, Object>>();
        for (int k = 0; k < details.size(); k++) { int id = i(details.get(k), "Id"); if (id <= 0 || last.get(id) == k) out.add(details.get(k)); }
        return out;
    }

    /**
     * Insert :3090-3463 in order. {@code freightToExpenses} is DebitAmountChargetoExpenseAcFreightGridPurchase
     * (FreightDebitToExpenses); {@code commAmount} is txtcommamount as TotalCommissionAmount leaves it.
     */
    static void validate(Map<String, Object> h, List<Map<String, Object>> details, List<Map<String, Object>> freight, List<Map<String, Object>> journal,
                         List<Map<String, Object>> expenses, List<Map<String, Object>> bags, boolean freightToExpenses, double commAmount) {
        // FormValidation :1444
        if (i(h, "SupplierCustomerId") == 0) throw new IllegalArgumentException("Supplier Field is Required");
        if (s(h, "DeliveryTerm").isBlank()) throw new IllegalArgumentException("DeliveryTerm Field is Required");
        if ((i(h, "CommissionAgentId") > 0 || commAmount > 0) && s(h, "CommissionRemarks").isEmpty()) throw new IllegalArgumentException("Commission Remarks Field is Required");
        String docNo = s(h, "DocNo").trim();
        if (docNo.isEmpty() || docNo.equals("0")) throw new IllegalArgumentException("DocNo Field is Required");
        // Charge to Product (freight) grid :3110-3141
        double credit = 0, debit = 0;
        for (var f : freight) {
            credit += n(f, "FreightAmount"); debit += n(f, "Debit");
            if (!(n(f, "FreightAmount") > 0)) continue;
            if (i(f, "SupplierCustomerId") == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
            if (s(f, "Remarks").isEmpty()) throw new IllegalArgumentException("Grid Charge to Product Remarks required Please Check");
        }
        if (freightToExpenses && (credit > 0 || debit > 0) && credit != debit) throw new IllegalArgumentException("Freight Debit Amount not equal to Credit Amount Please Check");
        if (!freightToExpenses && debit > 0) throw new IllegalArgumentException("Freight Debit Row Remove first");
        // Supplier AddLess :3143 - Debit is read with ToInt (quirk kept: a debit below 0.5 does not trigger the check)
        for (var j : journal) if ((n(j, "JvCredit") > 0 || toInt(n(j, "JvDebit")) > 0) && journalAccount(j) == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
        for (var e : expenses) if (n(e, "Amount") > 0 && i(e, "InvRevExpItemId") == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");
        for (var b : bags) if (n(b, "Amount") > 0) {
            if (i(b, "ItemId") == 0) throw new IllegalArgumentException("EmptyBags Item field required");
            if (i(b, "TypeId") == 0) throw new IllegalArgumentException("EmptyBagsType field required");
        }
        if (commAmount > 0 && i(h, "CommissionAgentId") == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");
        // Detail grid :3219-3326 (grd.RowCount > 0 else "Grid Record Not Found" :3509)
        if (details.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        double invoiceQty = 0;
        for (var d : details) {
            if (s(d, "CropYear").isEmpty()) throw new IllegalArgumentException("CropYear field required");
            if (i(d, "JobLotId") == 0) throw new IllegalArgumentException("JobLot field required");
            if (i(d, "PackingTypeId") == 0) throw new IllegalArgumentException("PackingType field required");
            if (i(d, "ItemUOMId") == 0) throw new IllegalArgumentException("PackUOM field required");
            invoiceQty += n(d, "ItemQty");
            if (n(d, "ItemQty") == 0) throw new IllegalArgumentException("ItemQty field required");
            if (n(d, "GrossWeight") == 0) throw new IllegalArgumentException("GrossWeight field required");
            if (n(d, "NetBillWeight") == 0) throw new IllegalArgumentException("NetBillWeight field required");
            if (n(d, "NetStockWeight") == 0) throw new IllegalArgumentException("NetStockWeight field required");
            if (n(d, "ItemRate") == 0) throw new IllegalArgumentException("ItemRate field required");
            if (i(d, "UomScheduleIdRate") == 0) throw new IllegalArgumentException("RateUom field required");
            if (n(d, "ItemAmount") == 0) throw new IllegalArgumentException("ItemAmount field required");
            if (i(d, "WarehouseId") == 0) throw new IllegalArgumentException("Warehouse field required");
        }
        // EmptyBags grid :3404-3463 (rows without an item are skipped; the row number is the grid row)
        double received = 0, purchased = 0; int row = 0;
        for (var b : bags) {
            row++;
            if (i(b, "ItemId") <= 0) continue;
            int type = i(b, "TypeId");
            if (type > 0 && type != 1 && type != 4 && type != 5 && i(b, "CreditAccountId") <= 0) throw new IllegalArgumentException("Credit Account Required in Empty Bag Grid in Row#" + row);
            if (i(b, "ItemConditionId") == 0) throw new IllegalArgumentException("Item Condition Required in Empty Bag Grid in Row#" + row);
            received += n(b, "ReceivedQty"); purchased += n(b, "PurchaseQty");
            if ((type == 1 || type == 2 || type == 3) && n(b, "Amount") == 0) throw new IllegalArgumentException("Packing Material Amount Required");
        }
        double bagsQty = received + purchased;
        if (bagsQty > 0 && bagsQty > invoiceQty) throw new IllegalArgumentException("Empty Bags Quantity Not greater than ItemQty");
    }

    /** Names the desktop reads from the grid cells' display text (ItemId / AccountId / ItemConditionId value lists). */
    record Names(Map<Integer, String> otherItems, Map<Integer, String> pmItems, Map<Integer, String> accounts, Map<Integer, String> conditions) {}

    /** The grid rows as BillAmount (:4068) reads them - every row, account cells mapped to the stored columns. */
    static Map<String, List<Map<String, Object>>> forCalculation(Map<String, List<Map<String, Object>>> ui, boolean subsidiary) {
        var out = new LinkedHashMap<String, List<Map<String, Object>>>();
        out.put("freight", ui.get("freight").stream().map(f -> freightRow(f)).toList());
        out.put("expenses", ui.get("expenses").stream().map(e -> expenseRow(e, null)).toList());
        out.put("journal", ui.get("journal").stream().filter(j -> journalAccount(j) > 0).map(j -> journalRow(j, subsidiary, null)).toList());
        out.put("emptyBags", ui.get("emptyBags").stream().map(b -> bagRow(b, null)).toList());
        return out;
    }

    /** Insert :3327-3457 - the rows the desktop adds to the four lists (and nothing else). */
    static Map<String, List<Map<String, Object>>> forSave(Map<String, List<Map<String, Object>>> ui, boolean subsidiary, Names names) {
        var out = new LinkedHashMap<String, List<Map<String, Object>>>();
        out.put("freight", ui.get("freight").stream().filter(f -> i(f, "SupplierCustomerId") != 0).map(f -> freightRow(f)).toList());
        out.put("expenses", ui.get("expenses").stream().filter(e -> i(e, "InvRevExpItemId") != 0).map(e -> expenseRow(e, names)).toList());
        out.put("journal", ui.get("journal").stream().filter(j -> journalAccount(j) != 0).map(j -> journalRow(j, subsidiary, names)).toList());
        out.put("emptyBags", ui.get("emptyBags").stream().filter(b -> i(b, "ItemId") > 0).map(b -> bagRow(b, names)).toList());
        return out;
    }

    /** :3333-3345 */
    private static Map<String, Object> freightRow(Map<String, Object> f) {
        var r = copy(null);
        r.put("Id", i(f, "Id")); r.put("PurchaseOrderId", i(f, "PurchaseOrderId")); r.put("TansporterId", i(f, "TansporterId")); r.put("InvGrnId", 0);
        r.put("FreightId", i(f, "FreightId")); r.put("SupplierCustomerId", i(f, "SupplierCustomerId")); r.put("Percentage", n(f, "Percentage"));
        r.put("FrQty", n(f, "FrQty")); r.put("FrRate", n(f, "FrRate")); r.put("FreightAmount", n(f, "FreightAmount")); r.put("Debit", n(f, "Debit"));
        r.put("Remarks", s(f, "Remarks"));
        return r;
    }
    /** :3354-3367 - Qty is read with Conversion.ToInt; the remarks get "ItemName : ... Qty: ... Rate: ..." when Qty and Rate are positive. */
    private static Map<String, Object> expenseRow(Map<String, Object> e, Names names) {
        var r = copy(null);
        r.put("Id", i(e, "Id")); r.put("PurchaseOrderId", i(e, "PurchaseOrderId")); r.put("PurchaseOrderSupplierExpId", i(e, "PurchaseOrderSupplierExpId"));
        r.put("InvRevExpItemId", i(e, "InvRevExpItemId"));
        double qty = names == null ? n(e, "Qty") : toInt(n(e, "Qty")), rate = n(e, "Rate");
        r.put("Qty", qty); r.put("Rate", rate); r.put("Amount", n(e, "Amount"));
        String remarks = s(e, "CustomRemarks");
        r.put("CustomRemarks", remarks);
        if (names != null && qty > 0 && rate > 0)
            remarks = "ItemName : " + names.otherItems().getOrDefault(i(e, "InvRevExpItemId"), "").trim() + "  Qty: " + net(qty) + " Rate: " + net(rate) + " " + remarks;
        r.put("Remarks", remarks);
        return r;
    }
    /** :3377-3400 - ChartofAccountId = the hidden GlAccountId; SupplierCustomerId = the AccountId value only with subsidiary accounts. */
    private static Map<String, Object> journalRow(Map<String, Object> j, boolean subsidiary, Names names) {
        var r = copy(null);
        int account = journalAccount(j);
        r.put("Id", i(j, "Id"));
        r.put("ChartofAccountId", j.containsKey("GlAccountId") ? i(j, "GlAccountId") : i(j, "ChartofAccountId"));
        r.put("SupplierCustomerId", subsidiary ? account : 0);
        String remarks = s(j, "JvRemarks");
        if (remarks.isEmpty() && names != null) remarks = names.accounts().getOrDefault(account, "");
        r.put("JvRemarks", remarks);
        r.put("JvPrcnt", n(j, "JvPrcnt")); r.put("JvQty", n(j, "JvQty")); r.put("JvRate", n(j, "JvRate"));
        r.put("JvDebit", n(j, "JvDebit")); r.put("JvCredit", n(j, "JvCredit")); r.put("InvGrnId", 0); r.put("FreightId", 0);
        return r;
    }
    /** :3411-3455 - CreditAccountId 0 for types 1, 4 and 5; ItemCondition is the combo text. */
    private static Map<String, Object> bagRow(Map<String, Object> b, Names names) {
        var r = copy(null);
        int type = i(b, "TypeId");
        r.put("TypeId", type);
        r.put("CreditAccountId", type == 1 || type == 4 || type == 5 ? 0 : i(b, "CreditAccountId"));
        r.put("PurchaseOrderId", i(b, "PurchaseOrderId")); r.put("ItemId", i(b, "ItemId")); r.put("ItemConditionId", i(b, "ItemConditionId"));
        if (names != null) r.put("ItemCondition", names.conditions().getOrDefault(i(b, "ItemConditionId"), ""));
        r.put("ReceivedQty", n(b, "ReceivedQty")); r.put("PurchaseQty", n(b, "PurchaseQty")); r.put("Rate", n(b, "Rate")); r.put("Amount", n(b, "Amount"));
        String remarks = s(b, "CustomRemarks");
        r.put("CustomRemarks", remarks);
        if (names != null && n(b, "PurchaseQty") > 0 && n(b, "Rate") > 0)
            remarks = "ItemName : " + names.pmItems().getOrDefault(i(b, "ItemId"), "").trim() + "  Qty: " + net(n(b, "PurchaseQty")) + " Rate: " + net(n(b, "Rate")) + " " + remarks;
        r.put("Remarks", remarks);
        return r;
    }
}
