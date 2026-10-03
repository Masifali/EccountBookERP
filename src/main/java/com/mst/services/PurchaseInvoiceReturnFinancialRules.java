package com.mst.services;

import java.math.BigDecimal;
import java.util.*;

import static com.mst.services.PurchaseInvoiceFinancialRules.copy;
import static com.mst.services.PurchaseInvoiceFinancialRules.i;
import static com.mst.services.PurchaseInvoiceFinancialRules.n;
import static com.mst.services.PurchaseInvoiceFinancialRules.s;

/**
 * Voucher of a Purchase Invoice Return (DocumentTypeId 59): BLL 0549 PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice
 * with the 0614 methods that act on type 59, in the BLL's order:
 * PurchaseReturnFinancial (0614:1550), FreightFinancial 59 branch (0614:371), SupplierAddLessFinancial (0614:171).
 * ItemAndSupplierByWeight / Trading / Brokery / EmptyBags / SalesTax do nothing for 59 (their type sets exclude it).
 * No database access; the account data comes from PurchaseInvoiceWriteRepository.accounts.
 */
public final class PurchaseInvoiceReturnFinancialRules {

    private PurchaseInvoiceReturnFinancialRules() { }

    /** C# double.ToString() for the narration texts (shortest round-trip, no exponent for ordinary values). */
    static String cs(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }
    private static Map<String, Object> entry(int ac, int against, double debit, double credit, String comments) {
        var m = copy(null); m.put("AccountId", ac); m.put("AgainstAccountId", against); m.put("DebitAmount", debit); m.put("CreditAmount", credit); m.put("Comments", comments); return m;
    }

    /**
     * @param h         header as saved (Id may still be 0 - the BLL builds the voucher before the insert)
     * @param details   detail rows as saved (ItemId, ItemRate, WarehouseId, JobLotId, amounts, BranchId...)
     * @param freight   InvPurchaseInvoiceFreight rows (TansporterId, FreightAmount, Remarks)
     * @param journal   InvPurchaseInvoiceJournal rows (ChartofAccountId, JvDebit, JvCredit, JvRemarks, SupplierCustomerId)
     * @param accounts  item GL ids / party GL ids / job-lot accounts / StockAgainstAccount
     */
    public static PurchaseInvoiceFinancialRules.Voucher calculate(Map<String, Object> h, List<Map<String, Object>> details, List<Map<String, Object>> freight,
                                                                  List<Map<String, Object>> journal, PurchaseInvoiceFinancialRules.Accounts accounts) {
        // 0549:33-38
        if (accounts.stockAgainst() <= 0) throw new IllegalArgumentException("Stock AgainstAc Configuration Not Found");
        int offset = accounts.stockAgainst();
        int supplierGl = 0, commissionGl = 0; String companyName = "";
        if (!accounts.parties().isEmpty()) {   // 0549:40
            if (n(h, "CommAmount") > 0) {
                if (i(h, "CommissionAgentId") <= 0) throw new IllegalArgumentException("CommissionAgent Account Not Found");
                var agent = accounts.parties().get(i(h, "CommissionAgentId"));
                if (agent != null) commissionGl = i(agent, "GlAccountId");
            }
            if (n(h, "BrokeryAmount") != 0) {
                if (i(h, "BrokerAgentId") <= 0) throw new IllegalArgumentException("BrokerAgent Account Not Found");
            }
            var sup = accounts.parties().get(i(h, "SupplierCustomerId"));
            if (sup == null) throw new IllegalArgumentException("Supplier GLAccountId not Found");
            supplierGl = i(sup, "GlAccountId"); companyName = s(sup, "CompanyName");
        }
        var out = new ArrayList<Map<String, Object>>();
        purchaseReturn(h, details, freight, accounts, supplierGl, commissionGl, offset, out);
        freight59(h, freight, offset, out);
        supplierAddLess(h, journal, supplierGl, out);

        var vh = copy(null);
        vh.put("DocumentTypeId", h.get("DocumentTypeId")); vh.put("DocumentTypeSrNo", h.get("Id")); vh.put("RefDocNoId", h.get("Id"));
        vh.put("VoucherCode", h.get("DocNo")); vh.put("VoucherDate", h.get("DocDate")); vh.put("RemarksOtherLingo", ""); vh.put("VoucherAmount", h.get("BillAmount"));
        vh.put("MultiCurrencyId", i(h, "CurrencyId")); vh.put("ExchangeCurrencyRate", n(h, "ExchangeRate")); vh.put("FcAmount", n(h, "FcyAmount"));
        vh.put("RefAccountId", supplierGl); vh.put("AgainstAccountId", commissionGl);
        vh.put("ChequeDate", java.sql.Date.valueOf(java.time.LocalDate.now())); vh.put("IncludeWHT", false);
        vh.put("BranchId", h.get("BranchesId")); vh.put("ProjectId", h.get("ProjectsId")); vh.put("BillAmount", h.get("BillAmount"));
        vh.put("ManualBillNo", h.get("ManualBillNo")); vh.put("DueDate", h.get("DueDate")); vh.put("DueDays", i(h, "DueDays"));
        for (String k : List.of("OrganizationId", "CompanyId", "FinancialYearId", "EntryUser", "ModifyUser")) vh.put(k, h.get(k));
        var now = new java.sql.Timestamp(System.currentTimeMillis()); vh.put("EntryDate", now); vh.put("ModifyDate", now);
        if (!s(h, "RemarksHeader").isEmpty()) vh.put("Remarks", s(h, "RemarksHeader"));   // 0549:134
        vh.put("_CompanyName", companyName);
        return new PurchaseInvoiceFinancialRules.Voucher(vh, out);
    }

    /** 0614:1550 PurchaseReturnFinancial. */
    private static void purchaseReturn(Map<String, Object> h, List<Map<String, Object>> details, List<Map<String, Object>> freightRows, PurchaseInvoiceFinancialRules.Accounts a,
                                       int supplierGl, int commissionGl, int offset, List<Map<String, Object>> out) {
        if (details.isEmpty()) return;
        // group by ItemId + ItemRate + WarehouseId (first occurrence keeps its other fields, the sums replace 10 fields)
        List<Map<String, Object>> groups = new ArrayList<>();
        for (var d : details) {
            Map<String, Object> g = null;
            for (var x : groups) if (i(x, "ItemId") == i(d, "ItemId") && n(x, "ItemRate") == n(d, "ItemRate") && i(x, "WarehouseId") == i(d, "WarehouseId")) { g = x; break; }
            if (g == null) groups.add(copy(d));
        }
        String[] sums = {"ItemAmount", "ExpenseAmount", "FreightAmount", "CommissionAmount", "ItemQty", "NetBillWeight", "WeightCut", "RateCut", "EBTotalWt", "GrossWeight"};
        for (var g : groups) {
            double[] t = new double[sums.length];
            for (var d : details) if (i(g, "ItemId") == i(d, "ItemId") && n(g, "ItemRate") == n(d, "ItemRate") && i(g, "WarehouseId") == i(d, "WarehouseId"))
                for (int k = 0; k < sums.length; k++) t[k] += n(d, sums[k]);
            for (int k = 0; k < sums.length; k++) g.put(sums[k], t[k]);
        }
        String text = "";   // Q-V1: the narration accumulates across items; only a non-empty RemarksHeader restarts it
        for (var g : groups) {
            if (a.items().isEmpty()) throw new IllegalArgumentException("ItemGlAccount Not found");   // 0614:1776
            var item = a.items().get(i(g, "ItemId"));
            if (item == null) continue;   // 0614:1625 'continue' when the item is not in the GL list
            if (!s(h, "RemarksHeader").isEmpty()) text = s(h, "RemarksHeader");
            text = text + "   NoOfBags: " + cs(n(g, "ItemQty")) + "   " + s(item, "ItemName") + "  GrossWeight: " + cs(n(g, "GrossWeight")) + "  Net Weight: " + cs(n(g, "NetBillWeight")) + "  Rate:" + cs(n(g, "ItemRate"));
            if (n(g, "EBTotalWt") > 0) text = text + "   EmptyBagsDeduction: " + cs(n(g, "EBTotalWt"));
            if (n(g, "RateCut") > 0) text = text + "   RateCut:" + cs(n(g, "RateCut"));
            if (n(g, "ExpenseAmount") > 0) text = text + "    Expense Amount: " + cs(n(g, "ExpenseAmount"));
            if (n(g, "CommissionAmount") > 0) text = text + "    Commission Amount: " + cs(n(g, "CommissionAmount"));
            if (n(g, "FreightAmount") > 0) text = text + "    Freight Amount: " + cs(n(g, "FreightAmount"));
            int lotAc = a.lots().getOrDefault(i(g, "JobLotId"), 0);
            int productAc = lotAc > 0 ? lotAc : i(item, "PurchaseGLAC");
            // debit supplier, credit product
            var dr = entry(supplierGl, productAc, n(g, "ItemAmount"), 0, text); returnFields(dr, g, h); out.add(dr);
            var cr = entry(productAc, supplierGl, 0, n(g, "ItemAmount"), text); returnFields(cr, g, h); out.add(cr);
            if (n(g, "ExpenseAmount") > 0) {
                var e1 = entry(supplierGl, productAc, n(g, "ExpenseAmount"), 0, "Other Expenses"); lineFields(e1, g); out.add(e1);
                var e2 = entry(productAc, supplierGl, 0, n(g, "ExpenseAmount"), "Other Expenses"); lineFields(e2, g); out.add(e2);
            }
            if (n(g, "CommissionAmount") > 0) {
                var c1 = entry(commissionGl, productAc, n(g, "CommissionAmount"), 0, s(h, "CommissionRemarks")); lineFields(c1, g); out.add(c1);
                var c2 = entry(productAc, commissionGl, 0, n(g, "CommissionAmount"), s(h, "CommissionRemarks")); lineFields(c2, g); out.add(c2);
            }
        }
        // 0614:1790-1846 freight "less to product" proportion rows
        double totalWeight = 0; for (var g : groups) totalWeight += n(g, "NetBillWeight");
        double freightTotal = 0; for (var f : freightRows) freightTotal += n(f, "FreightAmount");
        for (var g : groups) {
            if (a.items().isEmpty()) continue;
            var item = a.items().get(i(g, "ItemId"));
            if (item == null || !(n(g, "FreightAmount") > 0)) continue;
            int lotAc = a.lots().getOrDefault(i(g, "JobLotId"), 0);
            var f = entry(lotAc > 0 ? lotAc : i(item, "PurchaseGLAC"), offset, 0, n(g, "FreightAmount"),
                    "LessToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + cs(freightTotal) + " / " + cs(totalWeight) + " * " + cs(n(g, "NetBillWeight")) + " = " + cs(n(g, "FreightAmount")));
            lineFields(f, g); out.add(f);
        }
    }
    private static void returnFields(Map<String, Object> row, Map<String, Object> g, Map<String, Object> h) {
        row.put("ItemId", i(g, "ItemId")); row.put("QtyOut", n(g, "ItemQty")); row.put("ItemRate", n(g, "ItemRate")); row.put("WeightOut", n(g, "NetBillWeight"));
        row.put("RateCut", n(g, "RateCut")); row.put("RateCutAmount", n(g, "RateCutAmount")); row.put("ItemAmount", n(g, "ItemAmount"));
        row.put("Freight", n(g, "FreightAmount")); row.put("Expenses", n(g, "ExpenseAmount")); row.put("Journal", 0d); row.put("Commission", n(g, "CommissionAmount"));
        row.put("OrderNo", i(g, "PurchaseOrder")); row.put("GpNo", i(g, "GpNo")); row.put("VehicleNo", g.get("VehicleNo"));
        row.put("SupplierCustomerId", i(h, "SupplierCustomerId")); row.put("BranchesId", i(g, "BranchId"));
    }
    private static void lineFields(Map<String, Object> row, Map<String, Object> g) {
        row.put("ItemId", i(g, "ItemId")); row.put("JobLotId", i(g, "JobLotId")); row.put("BranchesId", i(g, "BranchId"));
    }

    /** 0614:371 FreightFinancial, the DocumentTypeId 59 branch: debit the "less to product" account against StockAgainstAccount. */
    private static void freight59(Map<String, Object> h, List<Map<String, Object>> freight, int offset, List<Map<String, Object>> out) {
        for (var f : freight) {
            if (i(f, "TansporterId") > 0 && n(f, "FreightAmount") > 0) {
                var d = entry(i(f, "TansporterId"), offset, n(f, "FreightAmount"), 0, s(f, "Remarks"));
                d.put("BranchesId", h.get("BranchesId")); out.add(d);
            }
        }
    }

    /** 0614:171 SupplierAddLessFinancial (AutoRemarksStatus is false for 59 - it is only set by ItemAndSupplierByWeight). */
    private static void supplierAddLess(Map<String, Object> h, List<Map<String, Object>> journal, int supplierGl, List<Map<String, Object>> out) {
        for (var j : journal) {
            if (i(j, "ChartofAccountId") > 0 && (n(j, "JvCredit") > 0 || n(j, "JvDebit") > 0)) {
                var d = entry(i(j, "ChartofAccountId"), supplierGl, n(j, "JvDebit"), n(j, "JvCredit"), s(j, "JvRemarks"));
                if (i(j, "SupplierCustomerId") > 0) { d.put("SupplierCustomerId", i(j, "SupplierCustomerId")); d.put("SubsidiaryAccountId", i(j, "SupplierCustomerId")); d.put("SubsidiaryTypeId", 1); }
                d.put("BranchesId", h.get("BranchesId")); out.add(d);
                var c = entry(supplierGl, i(j, "ChartofAccountId"), n(j, "JvCredit"), n(j, "JvDebit"), s(j, "JvRemarks"));
                c.put("SupplierCustomerId", i(h, "SupplierCustomerId")); c.put("SubsidiaryAccountId", i(h, "SupplierCustomerId")); c.put("SubsidiaryTypeId", 1);
                c.put("BranchesId", h.get("BranchesId")); out.add(c);
            }
        }
    }
}
