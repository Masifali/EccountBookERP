package com.mst.services;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.*;

import static com.mst.services.PurchaseInvoiceFinancialRules.copy;
import static com.mst.services.PurchaseInvoiceFinancialRules.i;
import static com.mst.services.PurchaseInvoiceFinancialRules.n;
import static com.mst.services.PurchaseInvoiceFinancialRules.s;

/**
 * PurchaseInvoiceFinancial.MakeVoucherForPurchaseInvoice (BLL 0549) for DocumentTypeId 138 only
 * (Purchase Invoice Against GRN Direct), with the 138 branches of PurchaseInvoiceGeneralFinancialMethods
 * (BLL 0614). Kept separate from {@link PurchaseInvoiceFinancialRules} (types 56/57) because the 138
 * branches differ in five places:
 * <ul>
 *   <li>ItemAndSupplierByWeightFinancial :789 - the supplier CREDIT row gets SupplierCustomerId but NO
 *       SubsidiaryAccountId/SubsidiaryTypeId (only 56/57/1603/1604/1653/1654 get them).</li>
 *   <li>:1110 - the per-group "freight charged to product" row is skipped for 138;</li>
 *   <li>:1191-1265 - 138 posts its own freight: DeliveryTerm "Load" = Dr product / Cr transporter per
 *       UNGROUPED detail row, "Ponch" = Dr supplier / Cr transporter once for the header freight.</li>
 *   <li>BrokeryBeHalfOnPartyFinancial :435-471 - 138 credits the broker against
 *       CommonIdsForFinancials.ItemStockAccountId (never assigned anywhere, so 0) when brokery &gt; 0,
 *       and Dr supplier / Cr broker when brokery &lt; 0.</li>
 *   <li>FreightFinancial (freight grid) and SupplierAddLessFinancial (journal grid) do not include 138;
 *       ItemAndSupplierTradingFinancial, PurchaseReturnFinancial and SalesTaxFinancial do not either.</li>
 * </ul>
 * No database access here; accounts come from the same scoped queries the 56/57 port uses
 * (PurchaseInvoiceWriteRepository.accounts).
 */
public final class PurchaseInvoiceGrnDirectFinancialRules {

    private PurchaseInvoiceGrnDirectFinancialRules() {}

    /** C# double.ToString() for the narrative text. */
    static String num(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return BigDecimal.valueOf((long) v).toPlainString();
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }
    /** C# Math.Round(v) / Math.Round(v, d) - banker's rounding - then ToString(). */
    static String bankers(double v, int digits) {
        return num(BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_EVEN).doubleValue());
    }

    private static Map<String, Object> entry(int account, int against, double debit, double credit, String comments) {
        var m = copy(null);
        m.put("AccountId", account); m.put("AgainstAccountId", against);
        m.put("DebitAmount", debit); m.put("CreditAmount", credit); m.put("Comments", comments);
        return m;
    }

    private static Map<String, Object> item(PurchaseInvoiceFinancialRules.Accounts a, int id) {
        var it = a.items().get(id);
        if (it == null) throw new IllegalArgumentException("ItemGlAccount Not found");
        return it;
    }

    /** jobLot.GetByID(JobLotId).AccountId when &gt; 0, else the item's PurchaseGLAC. */
    private static int productAccount(PurchaseInvoiceFinancialRules.Accounts a, Map<String, Object> line, Map<String, Object> it) {
        int lot = a.lots().getOrDefault(i(line, "JobLotId"), 0);
        return lot > 0 ? lot : i(it, "PurchaseGLAC");
    }

    private static void weightFields(Map<String, Object> row, Map<String, Object> d) {
        row.put("ItemId", i(d, "ItemId"));
        row.put("QtyIn", n(d, "ItemQty"));
        row.put("ItemRate", n(d, "ItemRate"));
        row.put("WeightIn", n(d, "NetBillWeight"));
        row.put("RateCut", n(d, "RateCut"));
        row.put("RateCutAmount", n(d, "RateCutAmount"));
        row.put("ItemAmount", n(d, "ItemAmount"));
        row.put("Freight", n(d, "FreightAmount"));
        row.put("Expenses", n(d, "ExpenseAmount"));
        row.put("Journal", n(d, "Brokery"));
        row.put("JobLotId", i(d, "JobLotId"));
        row.put("Commission", n(d, "CommissionAmount"));
        row.put("OrderNo", i(d, "PurchaseOrder"));
        row.put("GpNo", i(d, "GpNo"));
        row.put("VehicleNo", d.get("VehicleNo"));
        row.put("DMultiCurrencyId", i(d, "CurrencyId"));
        row.put("DExchangeCurrencyRate", n(d, "ExchangeRate"));
        row.put("DCurrencyAmount", n(d, "FcyAmount"));
        row.put("BranchesId", i(d, "BranchId"));
    }

    /**
     * @param h       invoice header as the form builds it (Insert() :2295-2338); RemarksHeader is
     *                REPLACED by the generated narrative, exactly as :989 does.
     * @param details the invoice detail rows (Insert() :2353-2444)
     * @param bags    the empty-bag rows (Insert() :2484-2523)
     */
    public static PurchaseInvoiceFinancialRules.Voucher calculate(Map<String, Object> h, List<Map<String, Object>> details,
                                                                  List<Map<String, Object>> bags,
                                                                  PurchaseInvoiceFinancialRules.Accounts accounts) {
        if (i(h, "DocumentTypeId") != 138) throw new IllegalArgumentException("This voucher conversion supports type 138 only");
        // MakeVoucherForPurchaseInvoice :32-81
        if (accounts.stockAgainst() <= 0) throw new IllegalArgumentException("Stock AgainstAc Configuration Not Found");
        int supplier = i(h, "SupplierCustomerId");
        int commissionGl = 0, brokerGl = 0;
        String brokerName = "", supplierName;
        if (n(h, "CommAmount") > 0) {
            if (i(h, "CommissionAgentId") <= 0) throw new IllegalArgumentException("CommissionAgent Account Not Found");
            var agent = accounts.parties().get(i(h, "CommissionAgentId"));
            if (agent != null) commissionGl = i(agent, "GlAccountId");
        }
        if (n(h, "BrokeryAmount") != 0) {
            if (i(h, "BrokerAgentId") <= 0) throw new IllegalArgumentException("BrokerAgent Account Not Found");
            var broker = accounts.parties().get(i(h, "BrokerAgentId"));
            if (broker != null) { brokerGl = i(broker, "GlAccountId"); brokerName = s(broker, "CompanyName"); }
        }
        var sp = accounts.parties().get(supplier);
        if (sp == null) throw new IllegalArgumentException("Supplier GLAccountId not Found");
        int supplierGl = i(sp, "GlAccountId");
        supplierName = s(sp, "CompanyName");
        boolean auto = accounts.autoRemarks();
        var out = new ArrayList<Map<String, Object>>();

        // ---------------------------------------------------------------- ItemAndSupplierByWeightFinancial
        // :493-559 group by ItemId + ItemRate + WarehouseId; the listed fields are summed, every other
        // field (RateCutAmount, JobLotId, PurchaseOrder, GpNo, VehicleNo, FcyAmount, ...) is the FIRST row's.
        var groups = new ArrayList<Map<String, Object>>();
        String[] summed = {"ItemAmount", "ExpenseAmount", "FreightAmount", "CommissionAmount", "Brokery", "ItemQty",
                "NetBillWeight", "WeightCut", "RateCut", "EBTotalWt", "GrossWeight"};
        for (var d : details) {
            Map<String, Object> g = null;
            for (var x : groups)
                if (i(x, "ItemId") == i(d, "ItemId") && n(x, "ItemRate") == n(d, "ItemRate") && i(x, "WarehouseId") == i(d, "WarehouseId")) { g = x; break; }
            if (g == null) { g = copy(d); for (String k : summed) g.put(k, 0d); groups.add(g); }
            for (String k : summed) g.put(k, n(g, k) + n(d, k));
        }
        String text = "", vehicle = "", headerText = "";
        int firstOrder = 0, previousItem = 0, index = 1;
        double commissionSum = 0, freightSum = 0;
        String remarks = s(h, "RemarksHeader");
        for (var d : groups) {
            text = "";
            var it = item(accounts, i(d, "ItemId"));
            if (index == 1) { firstOrder = i(d, "PurchaseOrder"); previousItem = i(d, "ItemId"); vehicle = s(d, "VehicleNo"); }
            double mond = n(d, "NetBillWeight") / 40.0;
            String itemName = s(it, "ItemName");
            if (n(d, "CommissionAmount") > 0) commissionSum += n(d, "CommissionAmount");
            if (n(d, "FreightAmount") > 0) freightSum += n(d, "FreightAmount");
            String prefix = "Purchase ";
            if (auto) {
                text = prefix + num(n(d, "ItemQty")) + " Bags  ( " + num(n(d, "GrossWeight")) + " , " + num(n(d, "NetBillWeight")) + ")  of " + itemName + "  "
                        + bankers(mond, 2) + " Mond @ Rs. " + num(n(d, "ItemRate") - n(d, "RateCut")) + "   = Rs. " + num(n(d, "ItemAmount"));
                if (n(d, "CommissionAmount") > 0) text += " Commission paid @ " + num(n(h, "CommRate")) + "% = Rs. " + bankers(n(d, "CommissionAmount"), 0);
                if (n(h, "BrokeryAmount") > 0) text += " Commission deduct @ " + num(n(h, "BrokeryRate")) + "% = Rs. " + bankers(n(h, "BrokeryAmount"), 0);
                if (n(d, "FreightAmount") > 0) text += " Freight Inward = Rs. " + bankers(n(d, "FreightAmount"), 0) + " Vehicle # " + s(d, "VehicleNo");
                if (i(d, "PurchaseOrder") > 0) text += " Under contract # " + i(d, "PurchaseOrder");
                if (!s(h, "ManualBillNo").isEmpty()) text += " Manual # " + s(h, "ManualBillNo");
                if (!s(d, "WareHouseName").isEmpty()) text += " Warehouse " + s(d, "WareHouseName");
                text += " from " + supplierName;
            } else {
                if (!remarks.isEmpty()) text = remarks;
                text += "   NoOfBags: " + num(n(d, "ItemQty")) + "   " + itemName + "  GrossWeight: " + num(n(d, "GrossWeight"))
                        + "  Net Weight: " + num(n(d, "NetBillWeight")) + "  Rate:" + num(n(d, "ItemRate"));
                if (n(d, "RateCut") != 0) {
                    double net = n(d, "RateCut") > 0 ? n(d, "ItemRate") - n(d, "RateCut") : n(d, "ItemRate") + n(d, "RateCut");
                    text += "   RateCut:" + num(n(d, "RateCut")) + " NetRate " + num(net);
                }
                if (n(d, "EBTotalWt") > 0) text += "   BagsDeduction: " + num(n(d, "EBTotalWt"));
                if (n(d, "WeightCutTotal") > 0) text += "   WeightCut: " + num(n(d, "WeightCutTotal"));
                if (n(d, "AdLsWeight") != 0) text += "   AdLsWeight: " + num(n(d, "AdLsWeight"));
                if (n(d, "ExpenseAmount") > 0) text += "    Expense Amount: " + bankers(n(d, "ExpenseAmount"), 0);
                if (n(d, "CommissionAmount") > 0) text += "    Commission Amount: " + bankers(n(d, "CommissionAmount"), 0);
                if (n(d, "FreightAmount") > 0) text += "    Freight Amount: " + bankers(n(d, "FreightAmount"), 0);
                if (n(d, "Brokery") > 0) text += "    Brokery Amount: " + bankers(n(d, "Brokery"), 0);
                if (!s(d, "WareHouseName").isEmpty()) text += " Warehouse " + s(d, "WareHouseName");
            }
            String summary = num(n(d, "ItemQty")) + " Bags  ( " + num(n(d, "GrossWeight")) + " , " + num(n(d, "NetBillWeight")) + ")  ";
            String rate = bankers(mond, 2) + " Mond @ Rs. " + num(n(d, "ItemRate") - n(d, "RateCut")) + "   = Rs. " + num(n(d, "ItemAmount"));
            if (previousItem == i(d, "ItemId") && index == 1) {
                if (!remarks.isEmpty()) headerText = remarks + " ";
                headerText += prefix + summary + "of " + itemName + "  " + rate;
            } else if (previousItem == i(d, "ItemId") && index > 1) {
                headerText += " , " + summary + rate;
            } else if (previousItem != i(d, "ItemId")) {
                headerText += prefix + summary + "of " + itemName + "  " + rate;
            }
            int product = productAccount(accounts, d, it);
            // :721-759 Dr product
            var dr = entry(product, supplierGl, n(d, "ItemAmount"), 0, text);
            weightFields(dr, d);
            dr.put("SubsidiaryAgainstAccountId", supplier); dr.put("SubsidiaryAgainstTypeId", 1);
            out.add(dr);
            // :760-798 Cr supplier - no SubsidiaryAccountId/TypeId for 138 (:789)
            var cr = entry(supplierGl, product, 0, n(d, "ItemAmount"), text);
            weightFields(cr, d);
            cr.put("SupplierCustomerId", supplier);
            out.add(cr);
            if (n(d, "ExpenseAmount") > 0) { // :799-852
                String c = auto ? text : "Other Expenses";
                var e1 = entry(product, supplierGl, n(d, "ExpenseAmount"), 0, c);
                e1.put("ItemId", i(d, "ItemId")); e1.put("JobLotId", i(d, "JobLotId")); e1.put("BranchesId", i(d, "BranchId"));
                out.add(e1);
                var e2 = entry(supplierGl, product, 0, n(d, "ExpenseAmount"), c);
                e2.put("JobLotId", i(d, "JobLotId")); e2.put("ItemId", i(d, "ItemId"));
                e2.put("SupplierCustomerId", supplier); e2.put("SubsidiaryAccountId", supplier); e2.put("SubsidiaryTypeId", 1);
                e2.put("BranchesId", i(d, "BranchId"));
                out.add(e2);
            }
            if (n(d, "CommissionAmount") > 0) { // :897-961 (not type 98)
                String c;
                if (auto) c = text;
                else if (s(h, "CommissionRemarks").isEmpty()) c = "ItemName " + itemName + " ItemQty " + num(n(d, "ItemQty")) + " Amount " + num(n(d, "CommissionAmount"));
                else c = s(h, "CommissionRemarks");
                var c1 = entry(product, commissionGl, n(d, "CommissionAmount"), 0, c);
                c1.put("ItemId", i(d, "ItemId")); c1.put("JobLotId", i(d, "JobLotId"));
                c1.put("SubsidiaryAgainstTypeId", 1); c1.put("SubsidiaryAgainstAccountId", i(h, "CommissionAgentId"));
                c1.put("BranchesId", i(d, "BranchId"));
                out.add(c1);
                var c2 = entry(commissionGl, product, 0, n(d, "CommissionAmount"), c);
                c2.put("SupplierCustomerId", i(h, "CommissionAgentId")); c2.put("SubsidiaryAccountId", i(h, "CommissionAgentId")); c2.put("SubsidiaryTypeId", 1);
                c2.put("ItemId", i(d, "ItemId")); c2.put("JobLotId", i(d, "JobLotId")); c2.put("BranchesId", i(d, "BranchId"));
                out.add(c2);
            }
            previousItem = i(d, "ItemId");
            index++;
        }
        // :969-989 header narrative
        if (commissionSum > 0) headerText += " Commission paid @ " + num(n(h, "CommRate")) + "% = Rs. " + bankers(commissionSum, 0);
        if (n(h, "BrokeryAmount") > 0) headerText += " Brokery deduct @ " + num(n(h, "BrokeryRate")) + "% = Rs. " + bankers(n(h, "BrokeryAmount"), 0);
        if (freightSum > 0) headerText += " Freight Inward = Rs. " + bankers(freightSum, 0) + " Vehicle # " + vehicle;
        if (firstOrder > 0) headerText += " Under contract # " + firstOrder;
        if (!s(h, "ManualBillNo").isEmpty()) headerText += " Manual # " + s(h, "ManualBillNo");
        h.put("RemarksHeader", headerText);

        // :1079-1190 per group: brokery charged to product (Dr product against itself). Freight is skipped for 138 (:1110).
        double totalWeight = 0;
        for (var g : groups) totalWeight += n(g, "NetBillWeight");
        for (var d : groups) {
            var it = item(accounts, i(d, "ItemId"));
            if (n(d, "Brokery") > 0 && n(h, "BrokeryAmount") > 0) {
                int product = productAccount(accounts, d, it);
                String c = auto ? text : "ChargeToProduct/TotalWeightInDetail * Weight = Proportionated Charges : " + num(n(h, "BrokeryAmount"))
                        + " / " + num(totalWeight) + " * " + num(n(d, "NetBillWeight")) + " = " + bankers(n(d, "Brokery"), 0);
                var b = entry(product, product, n(d, "Brokery"), 0, c);
                b.put("JobLotId", i(d, "JobLotId")); b.put("ItemId", i(d, "ItemId")); b.put("BranchesId", i(d, "BranchId"));
                out.add(b);
            }
        }
        // :1191-1266 type-138 freight
        String deliveryTerm = s(h, "DeliveryTerm");
        int transport = i(h, "TransportAccountId");
        if ("Load".equals(deliveryTerm)) {
            for (var d : details) { // UNGROUPED rows
                var it = accounts.items().get(i(d, "ItemId"));
                if (it == null || n(d, "FreightAmount") <= 0) continue;
                int product = productAccount(accounts, d, it);
                var f1 = entry(product, transport > 0 ? transport : product, n(d, "FreightAmount"), 0,
                        "SupplierName : " + supplierName + " Weight " + num(n(d, "NetBillWeight")) + " Freight " + bankers(n(d, "FreightAmount"), 0)
                                + " VehicleNo " + s(d, "VehicleNo") + " GrnNo " + i(d, "GrnNo"));
                f1.put("JobLotId", i(d, "JobLotId")); f1.put("ItemId", i(d, "ItemId")); f1.put("BranchesId", i(d, "BranchId"));
                out.add(f1);
                var f2 = entry(transport, product, 0, n(d, "FreightAmount"),
                        "SupplierName :  " + supplierName + "  Freight : " + num(n(d, "FreightAmount")) + " VehicleNo " + s(d, "VehicleNo") + " GrnNo " + i(d, "GrnNo"));
                f2.put("JobLotId", i(d, "JobLotId")); f2.put("ItemId", i(d, "ItemId")); f2.put("BranchesId", i(d, "BranchId"));
                out.add(f2);
            }
        }
        if ("Ponch".equals(deliveryTerm)) {
            // Posted even when there is no freight (both rows then carry 0) - desktop quirk kept.
            String c = "SupplierName : " + supplierName + " Freight Inward = Rs. " + bankers(n(h, "FreightAmount"), 0) + " Vehicle # " + vehicle;
            var p1 = entry(supplierGl, transport, n(h, "FreightAmount"), 0, c); p1.put("BranchesId", i(h, "BranchesId")); out.add(p1);
            var p2 = entry(transport, supplierGl, 0, n(h, "FreightAmount"), c); p2.put("BranchesId", i(h, "BranchesId")); out.add(p2);
        }

        // ---------------------------------------------------------------- BrokeryBeHalfOnPartyFinancial :435-471
        if (i(h, "BrokerAgentId") > 0 && n(h, "BrokeryAmount") > 0) {
            // CommonIdsForFinancials.ItemStockAccountId is never assigned -> AgainstAccountId 0 (desktop quirk kept).
            var b = entry(brokerGl, 0, 0, n(h, "BrokeryAmount"),
                    "SupplierName: " + supplierName + "  Brokery%" + num(n(h, "BrokeryRate")) + "  BrokeryAmount" + num(n(h, "BrokeryAmount")));
            b.put("BranchesId", i(h, "BranchesId"));
            out.add(b);
        }
        if (i(h, "BrokerAgentId") > 0 && n(h, "BrokeryAmount") < 0) {
            double abs = Math.abs(n(h, "BrokeryAmount"));
            var b1 = entry(supplierGl, brokerGl, abs, 0,
                    "BrokerAgent: " + brokerName + "  Brokery Rate" + num(n(h, "BrokeryRate")) + "  BrokeryAmount" + num(n(h, "BrokeryAmount")));
            b1.put("SupplierCustomerId", supplier); b1.put("SubsidiaryAccountId", supplier); b1.put("SubsidiaryTypeId", 1);
            b1.put("BranchesId", i(h, "BranchesId"));
            out.add(b1);
            var b2 = entry(brokerGl, supplierGl, 0, abs,
                    "SupplierName: " + supplierName + "  Brokery Rate" + num(n(h, "BrokeryRate")) + "  BrokeryAmount" + num(n(h, "BrokeryAmount")));
            b2.put("SubsidiaryAgainstTypeId", 1); b2.put("SubsidiaryAgainstAccountId", i(h, "BrokerAgentId"));
            b2.put("BranchesId", i(h, "BranchesId"));
            out.add(b2);
        }

        // ---------------------------------------------------------------- EmptyBagsFinancial :17-169
        boolean explicit = bags.stream().anyMatch(b -> (i(b, "TypeId") == 2 || i(b, "TypeId") == 3) && i(b, "CreditAccountId") > 0);
        double bagTotal = 0;
        for (var b : bags) {
            int type = i(b, "TypeId");
            if ((int) n(b, "ItemId") <= 0 || (int) n(b, "PurchaseQty") <= 0) continue;
            if (type == 1) {
                if (accounts.items().isEmpty()) throw new IllegalArgumentException("ItemId Not found against Packing Material");
                var it = accounts.items().get(i(b, "ItemId"));
                if (it == null) continue;
                double amount = n(b, "PurchaseQty") * n(b, "Rate");
                String c = s(b, "Remarks").isEmpty() ? "ItemName: " + s(it, "ItemName") + " Rate: " + num(n(b, "Rate")) + " Amount: " + num(amount) : s(b, "Remarks");
                if (amount == 0) throw new IllegalArgumentException("Packing Material Amount Field Required");
                var d1 = entry(i(it, "PurchaseGLAC"), supplierGl, amount, 0, c);
                d1.put("ItemId", i(b, "ItemId")); d1.put("BranchesId", i(h, "BranchesId"));
                out.add(d1);
                var d2 = entry(supplierGl, i(it, "PurchaseGLAC"), 0, amount, c);
                d2.put("ItemId", i(b, "ItemId")); d2.put("SupplierCustomerId", supplier); d2.put("SubsidiaryAccountId", supplier); d2.put("SubsidiaryTypeId", 1);
                d2.put("BranchesId", i(h, "BranchesId"));
                out.add(d2);
            } else if (type == 2 || type == 3) {
                if (i(b, "CreditAccountId") == 0 && explicit) throw new IllegalArgumentException("Credit Account Not Found against Packing Material");
                if (accounts.items().isEmpty()) throw new IllegalArgumentException("ItemId Not found against Packing Material");
                var it = accounts.items().get(i(b, "ItemId"));
                if (it == null) continue;
                double amount = n(b, "PurchaseQty") * n(b, "Rate");
                int ac = i(it, "PurchaseGLAC");
                String c = s(b, "Remarks").isEmpty() ? "ItemName: " + s(it, "ItemName") + " Rate: " + num(n(b, "Rate")) + " Amount: " + num(amount) : s(b, "Remarks");
                if (amount == 0) throw new IllegalArgumentException("Packing Material Amount Field Required");
                var d1 = entry(ac, explicit ? i(b, "CreditAccountId") : ac, amount, 0, c);
                d1.put("ItemId", i(b, "ItemId")); d1.put("BranchesId", i(h, "BranchesId"));
                out.add(d1);
                bagTotal += amount;
                if (explicit) {
                    var d2 = entry(i(b, "CreditAccountId"), ac, 0, amount, c);
                    d2.put("ItemId", i(b, "ItemId")); d2.put("BranchesId", i(h, "BranchesId"));
                    out.add(d2);
                }
            }
        }
        if (bagTotal > 0 && !explicit) {
            var allocated = new LinkedHashMap<Integer, Double>();
            for (var d : details) allocated.merge(i(d, "ItemId"), n(d, "EbPurAgainstWeightAmount"), Double::sum);
            for (var x : allocated.entrySet()) {
                if (x.getValue() <= 0) continue;
                var it = accounts.items().get(x.getKey());
                if (it == null) continue;
                var c = entry(i(it, "PurchaseGLAC"), i(it, "PurchaseGLAC"), 0, x.getValue(),
                        "Empty Bags Purchase against " + s(it, "ItemName") + " or Free of Cost Empty Bags");
                c.put("ItemId", x.getKey());
                out.add(c);
            }
        }

        // ---------------------------------------------------------------- voucher head :22-97
        var vh = copy(null);
        vh.put("DocumentTypeId", 138);
        vh.put("DocumentTypeSrNo", h.get("Id")); vh.put("RefDocNoId", h.get("Id"));
        vh.put("VoucherCode", i(h, "DocNo")); vh.put("VoucherDate", h.get("DocDate"));
        vh.put("RemarksOtherLingo", "");
        vh.put("VoucherAmount", n(h, "BillAmount"));
        vh.put("MultiCurrencyId", i(h, "CurrencyId")); vh.put("ExchangeCurrencyRate", n(h, "ExchangeRate")); vh.put("FcAmount", n(h, "FcyAmount"));
        vh.put("AgainstAccountId", commissionGl); vh.put("RefAccountId", supplierGl);
        vh.put("ChequeDate", java.sql.Date.valueOf(java.time.LocalDate.now()));
        vh.put("IncludeWHT", false);
        vh.put("BranchId", i(h, "BranchesId")); vh.put("ProjectId", i(h, "ProjectsId"));
        vh.put("BillAmount", n(h, "BillAmount")); vh.put("ManualBillNo", h.get("ManualBillNo"));
        vh.put("DueDate", h.get("DueDate")); vh.put("DueDays", i(h, "DueDays"));
        vh.put("OrganizationId", i(h, "OrganizationId")); vh.put("CompanyId", i(h, "CompanyId")); vh.put("FinancialYearId", i(h, "FinancialYearId"));
        vh.put("EntryUser", i(h, "EntryUser")); vh.put("ModifyUser", i(h, "ModifyUser"));
        var now = new java.sql.Timestamp(System.currentTimeMillis());
        vh.put("EntryDate", now); vh.put("ModifyDate", now);
        if (!headerText.isEmpty()) vh.put("Remarks", headerText);
        return new PurchaseInvoiceFinancialRules.Voucher(vh, out);
    }
}
