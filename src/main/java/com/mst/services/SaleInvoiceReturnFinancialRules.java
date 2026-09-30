package com.mst.services;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.*;

import static com.mst.services.PurchaseInvoiceFinancialRules.*;

/**
 * PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice (0549 F:14) for DocumentTypeId 98 (Sale Invoice
 * Return): ItemAndSupplierByWeightFinancial (0614 G:476-1280, 98 branch), FreightFinancial (G:292),
 * SupplierAddLessFinancial (G:171), EmptyBagsFinancial (G:17). No database access here - the CGS rates
 * (usp_getAvgRateOrlastPurchaseRate) are supplied by the caller per detail row as ItemCgsRate /
 * EquivalentPoRate when the CGS reversal applies.
 */
public final class SaleInvoiceReturnFinancialRules {

    /** flag2 = DebitAmountChargetoExpenseAcOfCommission, cgs = !InventoryFinancialsEffectsInActive && (CGSEntryAllow || feature 5 || SaleCostingJobOrderWise). */
    public record Options(boolean commissionToExpense, boolean cgsReversal) {}

    private static String num(double n) { return BigDecimal.valueOf(n).stripTrailingZeros().toPlainString(); }
    private static String round0(double n) { return BigDecimal.valueOf(n).setScale(0, RoundingMode.HALF_EVEN).toPlainString(); }
    private static String round2(double n) { return BigDecimal.valueOf(n).setScale(2, RoundingMode.HALF_EVEN).stripTrailingZeros().toPlainString(); }
    private static Map<String, Object> entry(int ac, int against, double debit, double credit, String comments) {
        var m = copy(null); m.put("AccountId", ac); m.put("AgainstAccountId", against); m.put("DebitAmount", debit); m.put("CreditAmount", credit); m.put("Comments", comments); return m;
    }
    private static void subsidiary(Map<String, Object> row, int party) { row.put("SupplierCustomerId", party); row.put("SubsidiaryAccountId", party); row.put("SubsidiaryTypeId", 1); }
    private static void againstSubsidiary(Map<String, Object> row, int party) { row.put("SubsidiaryAgainstAccountId", party); row.put("SubsidiaryAgainstTypeId", 1); }
    private static Map<String, Object> item(Accounts a, int id) {
        var it = a.items().get(id); if (it == null) throw new IllegalArgumentException("ItemGlAccount Not found"); return it;
    }
    private static Map<String, Object> party(Accounts a, int id, String message) {
        var p = a.parties().get(id); if (p == null || i(p, "GlAccountId") <= 0) throw new IllegalArgumentException(message); return p;
    }
    /** SaleAc = jobLot.AccountId > 0 ? that : item.SaleGLAC */
    private static int saleAccount(Accounts a, Map<String, Object> line) {
        int lot = a.lots().getOrDefault(i(line, "JobLotId"), 0);
        return lot > 0 ? lot : i(item(a, i(line, "ItemId")), "SaleGLAC");
    }
    private static void mainFields(Map<String, Object> row, Map<String, Object> d) {
        for (String k : List.of("ItemId", "ItemRate", "RateCut", "RateCutAmount", "ItemAmount", "JobLotId", "GpNo", "VehicleNo")) row.put(k, d.get(k));
        row.put("QtyIn", d.get("ItemQty")); row.put("WeightIn", d.get("NetBillWeight")); row.put("Freight", d.get("FreightAmount"));
        row.put("Expenses", d.get("ExpenseAmount")); row.put("Journal", d.get("Brokery")); row.put("Commission", d.get("CommissionAmount"));
        row.put("OrderNo", d.get("PurchaseOrder")); row.put("DMultiCurrencyId", d.get("CurrencyId")); row.put("DExchangeCurrencyRate", d.get("ExchangeRate")); row.put("DCurrencyAmount", d.get("FcyAmount"));
        row.put("BranchesId", d.get("BranchId"));
    }

    public static Voucher calculate(Map<String, Object> h, List<Map<String, Object>> details, List<Map<String, Object>> freight,
                                    List<Map<String, Object>> journal, List<Map<String, Object>> bags, Accounts accounts, Options opt) {
        if (details.isEmpty()) throw new IllegalArgumentException("Detail List not found");
        if (accounts.stockAgainst() <= 0) throw new IllegalArgumentException("Stock AgainstAc Configuration Not Found");
        int customer = i(h, "SupplierCustomerId");
        int agentGl = 0;
        if (n(h, "CommAmount") > 0) {
            if (i(h, "CommissionAgentId") <= 0) throw new IllegalArgumentException("CommissionAgent Account Not Found");
            agentGl = i(party(accounts, i(h, "CommissionAgentId"), "CommissionAgent Account Not Found"), "GlAccountId");
        }
        var cust = party(accounts, customer, "Supplier GLAccountId not Found");
        int customerGl = i(cust, "GlAccountId");
        String customerName = s(cust, "CompanyName");
        boolean auto = accounts.autoRemarks();
        var out = new ArrayList<Map<String, Object>>();

        // groups by (ItemId, ItemRate, WarehouseId); first row keeps its other fields, sums accumulate
        var groups = new LinkedHashMap<List<Object>, Map<String, Object>>();
        String[] totals = {"ItemAmount", "ExpenseAmount", "FreightAmount", "CommissionAmount", "Brokery", "ItemQty", "NetBillWeight", "WeightCut", "RateCut", "EBTotalWt", "GrossWeight"};
        for (var d : details) {
            List<Object> key = List.of(i(d, "ItemId"), n(d, "ItemRate"), i(d, "WarehouseId"));
            if (!groups.containsKey(key)) groups.put(key, copy(d));
            else { var g = groups.get(key); for (String k : totals) g.put(k, n(g, k) + n(d, k)); }
            d.put("PurchaseGLAC", i(item(accounts, i(d, "ItemId")), "PurchaseGLAC"));
        }

        String lastText = "", headerText = "", vehicle = "";
        int previousItem = 0, index = 0;
        double commissionSum = 0, freightSum = 0;
        for (var d : groups.values()) {
            var it = item(accounts, i(d, "ItemId"));
            int saleAc = saleAccount(accounts, d);
            if (index == 0) { vehicle = s(d, "VehicleNo"); previousItem = i(d, "ItemId"); }
            String summary = s(d, "ItemQty") + " Bags  ( " + s(d, "GrossWeight") + " , " + s(d, "NetBillWeight") + ")  ";
            String weightRate = round2(n(d, "NetBillWeight") / 40) + " Mond @ Rs. " + num(n(d, "ItemRate") - n(d, "RateCut")) + "   = Rs. " + s(d, "ItemAmount");
            String named = summary + "of " + s(it, "ItemName") + "  " + weightRate;
            if (index == 0) headerText = (s(h, "RemarksHeader").isEmpty() ? "" : s(h, "RemarksHeader") + " ") + "Sale Return " + named;
            else headerText += previousItem == i(d, "ItemId") ? " , " + summary + weightRate : "Sale Return " + named;
            String text;
            if (auto) {
                text = "Sale Return " + named;
                if (n(d, "CommissionAmount") > 0) text += " Commission paid @ " + s(h, "CommRate") + "% = Rs. " + round0(n(d, "CommissionAmount"));
                if (n(d, "Brokery") > 0) text += " Brokery = Rs. " + round0(n(d, "Brokery"));
                if (n(d, "FreightAmount") > 0) text += " Freight Inward = Rs. " + round0(n(d, "FreightAmount")) + " Vehicle # " + s(d, "VehicleNo");
                if (!s(h, "ManualBillNo").isEmpty()) text += " Manual # " + s(h, "ManualBillNo");
                if (!s(d, "WareHouseName").isEmpty()) text += " Warehouse " + s(d, "WareHouseName");
                text += " from " + customerName;
            } else {
                text = s(h, "RemarksHeader") + "   NoOfBags: " + s(d, "ItemQty") + "   " + s(it, "ItemName") + "  GrossWeight: " + s(d, "GrossWeight") + "  Net Weight: " + s(d, "NetBillWeight") + "  Rate:" + s(d, "ItemRate");
                if (n(d, "RateCut") != 0) text += "   RateCut:" + s(d, "RateCut") + " NetRate " + num(n(d, "RateCut") > 0 ? n(d, "ItemRate") - n(d, "RateCut") : n(d, "ItemRate") + n(d, "RateCut"));
                if (n(d, "EBTotalWt") > 0) text += "   BagsDeduction: " + s(d, "EBTotalWt");
                if (n(d, "WeightCutTotal") > 0) text += "   WeightCut: " + s(d, "WeightCutTotal");
                if (n(d, "AdLsWeight") != 0) text += "   AdLsWeight: " + s(d, "AdLsWeight");
                if (n(d, "ExpenseAmount") > 0) text += "    Expense Amount: " + round0(n(d, "ExpenseAmount"));
                if (n(d, "CommissionAmount") > 0) text += "    Commission Amount: " + round0(n(d, "CommissionAmount"));
                if (n(d, "FreightAmount") > 0) text += "    Freight Amount: " + round0(n(d, "FreightAmount"));
                if (n(d, "Brokery") > 0) text += "    Brokery Amount: " + round0(n(d, "Brokery"));
                if (!s(d, "WareHouseName").isEmpty()) text += " Warehouse " + s(d, "WareHouseName");
            }
            lastText = text;
            if (n(d, "CommissionAmount") > 0) commissionSum += n(d, "CommissionAmount");
            if (n(d, "FreightAmount") > 0) freightSum += n(d, "FreightAmount");
            // Line 1: Dr SaleAc against customer; Line 2: Cr customer against SaleAc (SupplierCustomerId only, no Subsidiary* for 98)
            var dr = entry(saleAc, customerGl, n(d, "ItemAmount"), 0, text); mainFields(dr, d); againstSubsidiary(dr, customer); out.add(dr);
            var cr = entry(customerGl, saleAc, 0, n(d, "ItemAmount"), text); mainFields(cr, d); cr.put("SupplierCustomerId", customer); out.add(cr);
            if (n(d, "ExpenseAmount") > 0) {
                String c = auto ? text : "Other Expenses";
                dr = entry(saleAc, customerGl, n(d, "ExpenseAmount"), 0, c); dr.put("ItemId", d.get("ItemId")); dr.put("JobLotId", d.get("JobLotId")); dr.put("BranchesId", d.get("BranchId")); out.add(dr);
                cr = entry(customerGl, saleAc, 0, n(d, "ExpenseAmount"), c); cr.put("ItemId", d.get("ItemId")); cr.put("JobLotId", d.get("JobLotId")); cr.put("BranchesId", d.get("BranchId")); subsidiary(cr, customer); out.add(cr);
            }
            if (n(d, "CommissionAmount") > 0) {
                int against = opt.commissionToExpense() && i(h, "CommissionCreditAccountId") > 0 ? i(h, "CommissionCreditAccountId") : saleAc;
                dr = entry(agentGl, against, n(d, "CommissionAmount"), 0, s(h, "CommissionRemarks")); dr.put("ItemId", d.get("ItemId")); dr.put("JobLotId", d.get("JobLotId")); dr.put("BranchesId", d.get("BranchId")); subsidiary(dr, i(h, "CommissionAgentId")); out.add(dr);
                cr = entry(against, agentGl, 0, n(d, "CommissionAmount"), s(h, "CommissionRemarks")); cr.put("ItemId", d.get("ItemId")); cr.put("JobLotId", d.get("JobLotId")); cr.put("BranchesId", d.get("BranchId")); againstSubsidiary(cr, i(h, "CommissionAgentId")); out.add(cr);
            }
            previousItem = i(d, "ItemId"); index++;
        }
        if (commissionSum > 0) headerText += " Commission paid @ " + s(h, "CommRate") + "% = Rs. " + round0(commissionSum);
        if (freightSum > 0) headerText += " Freight Inward = Rs. " + round0(freightSum) + " Vehicle # " + vehicle;
        if (!s(h, "ManualBillNo").isEmpty()) headerText += " Manual # " + s(h, "ManualBillNo");
        h.put("RemarksHeader", headerText);   // G:989 - Q-F1

        // CGS reversal (98 only), per UNGROUPED detail row: Dr item PurchaseGLAC / Cr item COGSGLAC
        if (opt.cgsReversal()) {
            for (var d : details) {
                var it = item(accounts, i(d, "ItemId"));
                if (n(d, "ItemCgsRate") <= 0) throw new IllegalArgumentException("CGS Rate not found against " + s(it, "ItemName") + " and row qty is " + s(d, "ItemQty"));
                int purchase = i(it, "PurchaseGLAC"), cogs = i(it, "COGSGLAC");
                if (purchase <= 0 || cogs <= 0) throw new IllegalArgumentException("Item GlAccount Not found");
                double amt = n(d, "NetBillWeight") / n(d, "EquivalentPoRate") * n(d, "ItemCgsRate");
                String c = "Bags: " + s(d, "ItemQty") + "   " + s(it, "ItemName") + " Net Weight: " + s(d, "NetBillWeight") + " Rate: " + s(d, "ItemCgsRate")
                        + (n(d, "CommissionAmount") > 0 ? "    Commission Amount: " + s(d, "CommissionAmount") : "") + "  " + customerName;
                var dr = entry(purchase, cogs, amt, 0, c); cgsFields(dr, d, customer); out.add(dr);
                var cr = entry(cogs, purchase, 0, amt, c); cgsFields(cr, d, customer); out.add(cr);
            }
        }

        // Freight to product per group: Dr SaleAc, against the single distinct transporter (else itself)
        double totalWeight = groups.values().stream().mapToDouble(d -> n(d, "NetBillWeight")).sum();
        double freightTotal = freight.stream().mapToDouble(f -> n(f, "FreightAmount")).sum();
        var transporters = new HashSet<Integer>(); for (var f : freight) if (i(f, "TansporterId") > 0) transporters.add(i(f, "TansporterId"));
        int transporter = transporters.size() == 1 ? transporters.iterator().next() : 0;
        for (var d : groups.values()) if (n(d, "FreightAmount") > 0) {
            int saleAc = saleAccount(accounts, d);
            String c = auto ? lastText : "ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + num(freightTotal) + " / " + num(totalWeight) + " * " + s(d, "NetBillWeight") + " = " + round0(n(d, "FreightAmount"));
            var dr = entry(saleAc, i(h, "TransportAccountId") > 0 ? i(h, "TransportAccountId") : transporter > 0 ? transporter : saleAc, n(d, "FreightAmount"), 0, c);
            dr.put("ItemId", d.get("ItemId")); dr.put("JobLotId", d.get("JobLotId")); dr.put("BranchesId", d.get("BranchId")); out.add(dr);
        }

        // FreightFinancial G:292
        var purchaseAccounts = new HashSet<Integer>(); for (var d : details) purchaseAccounts.add(i(d, "PurchaseGLAC"));
        int single = purchaseAccounts.size() == 1 ? purchaseAccounts.iterator().next() : 0;
        for (var f : freight) if (i(f, "TansporterId") > 0) {
            if (n(f, "Debit") > 0) {
                String c = auto ? lastText : !s(f, "Remarks").isEmpty() ? s(f, "Remarks") : "PartyName :  " + customerName + "  Freight : " + s(f, "FreightAmount");
                var e = entry(i(f, "TansporterId"), single > 0 ? single : i(f, "TansporterId"), n(f, "Debit"), 0, c);
                if (i(f, "SupplierCustomerId") > 0) subsidiary(e, i(f, "SupplierCustomerId")); e.put("BranchesId", h.get("BranchesId")); out.add(e);
            }
            if (n(f, "FreightAmount") > 0) {
                String c = auto ? lastText : !s(f, "Remarks").isEmpty() ? s(f, "Remarks") : "SupplierName :  " + customerName + "  Freight : " + s(f, "FreightAmount");
                var e = entry(i(f, "TansporterId"), single > 0 ? single : i(f, "TansporterId"), 0, n(f, "FreightAmount"), c);
                if (i(f, "SupplierCustomerId") > 0) subsidiary(e, i(f, "SupplierCustomerId")); e.put("BranchesId", h.get("BranchesId")); out.add(e);
            }
        }

        // SupplierAddLessFinancial G:171
        for (var j : journal) if (i(j, "ChartofAccountId") > 0 && (n(j, "JvDebit") > 0 || n(j, "JvCredit") > 0)) {
            String c = auto ? lastText : s(j, "JvRemarks");
            var a = entry(i(j, "ChartofAccountId"), customerGl, n(j, "JvDebit"), n(j, "JvCredit"), c); if (i(j, "SupplierCustomerId") > 0) subsidiary(a, i(j, "SupplierCustomerId")); a.put("BranchesId", h.get("BranchesId")); out.add(a);
            var b = entry(customerGl, i(j, "ChartofAccountId"), n(j, "JvCredit"), n(j, "JvDebit"), c); subsidiary(b, customer); b.put("BranchesId", h.get("BranchesId")); out.add(b);
        }

        // EmptyBagsFinancial G:17
        boolean explicit = bags.stream().anyMatch(b -> (i(b, "TypeId") == 2 || i(b, "TypeId") == 3) && i(b, "CreditAccountId") > 0);
        double bagTotal = 0;
        for (var b : bags) {
            int bt = i(b, "TypeId"); if (bt < 1 || bt > 3 || i(b, "ItemId") <= 0 || (int) n(b, "PurchaseQty") <= 0) continue;
            var it = item(accounts, i(b, "ItemId")); int ac = i(it, "PurchaseGLAC");
            double amount = n(b, "PurchaseQty") * n(b, "Rate");
            if (bt == 1 && amount == 0) throw new IllegalArgumentException("Packing Material Amount Field Required");
            if (bt != 1 && explicit && i(b, "CreditAccountId") == 0) throw new IllegalArgumentException("Credit Account Not Found against Packing Material");
            String remark = s(b, "Remarks").isEmpty() ? "ItemName: " + s(it, "ItemName") + " Rate: " + s(b, "Rate") + " Amount: " + num(amount) : s(b, "Remarks");
            int against = bt == 1 ? customerGl : explicit ? i(b, "CreditAccountId") : ac;
            var dr = entry(ac, against, amount, 0, remark); dr.put("ItemId", b.get("ItemId")); dr.put("BranchesId", h.get("BranchesId")); out.add(dr);
            if (bt == 1 || explicit) { var cr = entry(against, ac, 0, amount, remark); cr.put("ItemId", b.get("ItemId")); cr.put("BranchesId", h.get("BranchesId")); if (bt == 1) subsidiary(cr, customer); out.add(cr); }
            if (bt != 1) bagTotal += amount;
        }
        if (bagTotal > 0 && !explicit) {
            var allocated = new LinkedHashMap<Integer, Double>(); for (var d : details) allocated.merge(i(d, "ItemId"), n(d, "EbPurAgainstWeightAmount"), Double::sum);
            for (var x : allocated.entrySet()) if (x.getValue() > 0) {
                var it = item(accounts, x.getKey()); int ac = i(it, "PurchaseGLAC");
                var cr = entry(ac, ac, 0, x.getValue(), "Empty Bags Purchase against " + s(it, "ItemName") + " or Free of Cost Empty Bags"); cr.put("ItemId", x.getKey()); out.add(cr);
            }
        }

        // VoucherHead F:14-141
        var vh = copy(null);
        for (String k : List.of("DocumentTypeId", "BillAmount", "ManualBillNo", "DueDate", "DueDays", "OrganizationId", "CompanyId", "FinancialYearId", "EntryUser", "ModifyUser")) vh.put(k, h.get(k));
        vh.put("DocumentTypeSrNo", h.get("Id")); vh.put("RefDocNoId", h.get("Id")); vh.put("VoucherCode", h.get("DocNo")); vh.put("VoucherDate", h.get("DocDate"));
        vh.put("VoucherAmount", h.get("BillAmount")); vh.put("MultiCurrencyId", h.get("CurrencyId")); vh.put("ExchangeCurrencyRate", h.get("ExchangeRate")); vh.put("FcAmount", h.get("FcyAmount"));
        vh.put("RemarksOtherLingo", ""); vh.put("ChequeDate", new java.sql.Date(System.currentTimeMillis())); vh.put("IncludeWHT", false);
        vh.put("BranchId", h.get("BranchesId")); vh.put("ProjectId", h.get("ProjectsId")); vh.put("Remarks", h.get("RemarksHeader"));
        vh.put("OffsetAccountId", accounts.stockAgainst()); vh.put("RefAccountId", customerGl); vh.put("AgainstAccountId", agentGl);
        vh.put("EntryDate", new java.sql.Timestamp(System.currentTimeMillis())); vh.put("ModifyDate", new java.sql.Timestamp(System.currentTimeMillis()));
        return new Voucher(vh, out);
    }

    private static void cgsFields(Map<String, Object> row, Map<String, Object> d, int customer) {
        row.put("ItemId", d.get("ItemId")); row.put("ItemCgsRate", d.get("ItemCgsRate")); row.put("QtyIn", d.get("ItemQty")); row.put("WeightIn", d.get("NetBillWeight"));
        row.put("GpNo", d.get("GpNo")); row.put("VehicleNo", d.get("VehicleNo")); row.put("JobLotId", d.get("JobLotId")); row.put("SupplierCustomerId", customer);
        row.put("BranchesId", d.get("BranchId"));
    }

    private SaleInvoiceReturnFinancialRules() {}
}
