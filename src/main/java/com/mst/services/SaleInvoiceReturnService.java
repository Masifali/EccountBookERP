package com.mst.services;

import com.mst.repositories.PurchaseInvoiceWriteRepository;
import com.mst.repositories.SaleInvoiceReturnRepository;
import com.mst.repositories.support.ProcExec;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.*;

import static com.mst.repositories.SaleInvoiceReturnRepository.*;

/**
 * InvfrmSaleInvoiceReturn (98): Insert :2188 (validations, header/detail/children mapping), the BLL
 * InvPurchaseInvoice.Save (0581:18) with the 98 financial builder, the DAL SetData order (0434 D:20-578),
 * btnDelete :4030, and the LoadSaleInvoiceForReturn dialog. Amount proportions are the desktop's own
 * (:2961-3069, :3742, :4372, :2885) so the saved rows equal the desktop's for the same input.
 */
@Service
public class SaleInvoiceReturnService {

    private final SaleInvoiceReturnRepository repo;
    private final PurchaseInvoiceWriteRepository writes;
    private final PurchaseInvoiceAttachmentService attachments;
    private final CurrentUserContext context;
    private final JdbcTemplate jdbc;

    public SaleInvoiceReturnService(SaleInvoiceReturnRepository repo, PurchaseInvoiceWriteRepository writes,
                                    PurchaseInvoiceAttachmentService attachments, CurrentUserContext context, JdbcTemplate jdbc) {
        this.repo = repo; this.writes = writes; this.attachments = attachments; this.context = context; this.jdbc = jdbc;
    }

    // ------------------------------------------------------------------ helpers (Conversion.* semantics)
    private static double round(double v, int digits) { return BigDecimal.valueOf(v).setScale(digits, RoundingMode.HALF_UP).doubleValue(); }  // MidpointRounding.AwayFromZero
    private static int toInt(Object o) {   // Conversion.ToInt: Convert.ToInt32 of a double = banker's rounding; a non-integer STRING -> 0
        if (o instanceof Number) return (int) Math.rint(((Number) o).doubleValue());
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
        Map<String, Object> flags = repo.flags();
        Map<String, Object> out = new LinkedHashMap<>(flags);
        out.put("lookups", repo.lookups(flags));
        return out;
    }

    // ------------------------------------------------------------------ calculations (desktop, unrounded)
    /** TotalCommissionAmount :3742 - only when the rate is non-empty; result is the raw double. */
    public static Double commissionAmount(String type, Object rate, Object commUom, double sumItemAmount, double sumNetWeight) {
        if (str(rate).isEmpty()) return null;
        double r = dbl(rate);
        return switch (str(type)) {
            case "Flat" -> r;
            case "Percent", "Percentage" -> sumItemAmount * r / 100;
            case "Comm Weight" -> sumNetWeight / dbl(commUom) * r;
            default -> null;
        };
    }
    /** CommissionProportion, ExpProportion, FreightProportion, WagesAmountProportion, BillProportion (:2961-3069, :4372). */
    public static void proportions(List<Map<String, Object>> grid, double commAmount, double expenseTotal, double freightCredit, double wages, boolean wagesOnQty, boolean wagesToProduct) {
        double w = sum(grid, "NetBillWeight"), q = sum(grid, "ItemQty");
        for (var r : grid) {
            double nw = dbl(r.get("NetBillWeight"));
            r.put("CommissionAmount", commAmount / w * nw);
            r.put("ExpenseAmount", expenseTotal / w * nw);
            r.put("FreightAmount", freightCredit > 0 ? freightCredit / w * nw : 0d);
            r.put("WagesAmount", wages > 0 ? (wagesOnQty ? wages / q * dbl(r.get("ItemQty")) : wages / w * nw) : 0d);
            double bill = dbl(r.get("ItemAmount")) + dbl(r.get("ExpenseAmount")) + dbl(r.get("FreightAmount")) - dbl(r.get("CommissionAmount")) - dbl(r.get("EbPurAgainstWeightAmount"));
            if (wagesToProduct) bill += dbl(r.get("WagesAmount"));
            r.put("BillAmount", bill);
        }
    }
    /** BillAmount() :2885 header total (before rounding). */
    public static double headerBill(List<Map<String, Object>> grid, List<Map<String, Object>> expenses, List<Map<String, Object>> journal, List<Map<String, Object>> freight, List<Map<String, Object>> bags,
                                    int customerId, String customerPartyCode, int agentId, double commAmount, boolean subsidiary) {
        double item = sum(grid, "ItemAmount"), exp = sum(expenses, "Amount");
        double jDr = 0, jCr = 0; for (var j : journal) if (toInt(j.get("AccountId")) > 0) { jDr += dbl(j.get("Debit")); jCr += dbl(j.get("Credit")); }
        double tCr = 0, tDr = 0;
        for (var f : freight) if (toInt(f.get("Transporter")) > 0 || toInt(f.get("GlAccountId")) > 0) {
            boolean same = subsidiary ? customerId == toInt(f.get("Transporter"))
                    : customerId > 0 && toInt(customerPartyCode) == toInt(f.get("GlAccountId"));   // Q-B1: PartyCode compared with GL id
            if (same) { tCr += dbl(f.get("Freight")); tDr += dbl(f.get("Debit")); }
        }
        double eb = 0; for (var b : bags) if (dbl(b.get("Amount")) > 0 && toInt(b.get("TypeId")) == 1) eb += dbl(b.get("Amount"));
        double bill = item + exp + eb + jDr - jCr + tCr - tDr;
        if (customerId == agentId) bill -= commAmount;   // Q-B2
        return bill;
    }
    /** CalculateEbPurAgainstWeightAmountBasedOnBalQty :4694 */
    public static boolean emptyBagProportion(List<Map<String, Object>> grid, List<Map<String, Object>> bags) {
        boolean anyCredit = bags.stream().anyMatch(b -> (toInt(b.get("TypeId")) == 2 || toInt(b.get("TypeId")) == 3) && toInt(b.get("CreditAccountId")) > 0);
        double a = 0, q = 0;
        for (var b : bags) if (toInt(b.get("TypeId")) == 2 || toInt(b.get("TypeId")) == 3) { a += dbl(b.get("Amount")); q += dbl(b.get("Qty")); }
        if (anyCredit || q <= 0 || a <= 0) { for (var r : grid) r.put("EbPurAgainstWeightAmount", 0d); return false; }
        double per = a / q, balQ = q, balA = a;
        for (var r : grid) {
            double used = Math.min(dbl(r.get("ItemQty")), balQ); double eb = per * used;
            r.put("EbPurAgainstWeightAmount", eb); balQ -= used; balA -= eb;
            if (balA <= 0) break;
        }
        return true;
    }

    // ------------------------------------------------------------------ save (Insert :2188)
    @Transactional
    public Map<String, Object> save(Map<String, Object> payload) {
        var p = ci(payload);
        Map<String, Object> flags = repo.flags();
        boolean subsidiary = Boolean.TRUE.equals(flags.get("subsidiaryAccountAllownOnVouchers"));
        boolean commissionToExpense = Boolean.TRUE.equals(flags.get("commissionDebitToExpenses"));
        boolean wagesToProduct = Boolean.TRUE.equals(flags.get("contractWagesChargetoProduct"));
        boolean wagesOnQty = Boolean.TRUE.equals(flags.get("wagesAmountCalculateOnQty"));
        int N = num(flags.get("amountDecimals"));
        int recId = num(p.get("Id"));
        boolean update = recId > 0;
        if (!repo.hasRight(update ? "Update" : "Save")) throw new IllegalArgumentException("You do not have " + (update ? "Update" : "Save") + " rights on InvfrmSaleInvoiceReturn");
        Map<String, Object> old = null;
        if (update) {
            old = repo.header(recId);
            if (old == null) throw new IllegalArgumentException("Record not found");
            if (truthy(old.get("IsApproved"))) throw new IllegalArgumentException("Record Not Update because Record has approved");
        }

        // 7.1 (1) FormValidation :917
        if (str(p.get("DocNo")).isEmpty() || "0".equals(str(p.get("DocNo")))) throw new IllegalArgumentException("DocNo Field is Required");
        int customer = num(p.get("SupplierCustomerId"));
        if (customer <= 0) throw new IllegalArgumentException("Supplier Field is Required");

        var grid = list(p, "details"); var freightGrid = list(p, "freight"); var glGrid = list(p, "journal"); var expGrid = list(p, "expenses"); var bagGrid = list(p, "emptyBags");
        // (3)-(6)
        for (var f : freightGrid) if (dbl(f.get("Freight")) > 0 && toInt(f.get("Transporter")) == 0) throw new IllegalArgumentException("Please Select an Account Against Freight First");
        for (var j : glGrid) if ((dbl(j.get("Credit")) > 0 || toInt(j.get("Debit")) > 0) && toInt(j.get("AccountId")) == 0) throw new IllegalArgumentException("Please Select an Account Against JL First");
        for (var e : expGrid) if (dbl(e.get("Amount")) > 0 && toInt(e.get("ItemId")) == 0) throw new IllegalArgumentException("Please Select an Item Against Expense First");
        double commAmount = dbl(p.get("CommAmount"));
        int agent = num(p.get("CommissionAgentId"));
        if (commAmount > 0) {
            if (agent == 0) throw new IllegalArgumentException("Please Select Commission Agent Account First");
            if (commissionToExpense && num(p.get("CommissionCreditAccountId")) <= 0) throw new IllegalArgumentException("Please Select Commission Debit Account First");
        } else if (dbl(p.get("CommRate")) > 0) throw new IllegalArgumentException("Commission Amount is Required when Comm Rate is greater then 0");
        // (7)
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        // the desktop's grid values (proportions) are recomputed from the same inputs so both save the same rows
        double wagesHeader = dbl(p.get("WagesAmount"));
        proportions(grid, commAmount, sum(expGrid, "Amount"), sum(freightGrid, "Freight"), wagesHeader, wagesOnQty, wagesToProduct);

        // (8) per detail row, in order
        for (var r : grid) {
            int grnId = num(r.get("GrnId")), grnDetailId = num(r.get("GrnDetailId"));
            if (grnId > 0 && grnDetailId <= 0) throw new IllegalArgumentException("$GrnDetail Id not Found Please Check");
            if (grnId <= 0 && grnDetailId > 0) throw new IllegalArgumentException("$GrnId not Found Please Check");
            if (num(r.get("WarehouseId")) <= 0) throw new IllegalArgumentException("$Warehouse not found. Please Check!");
            if (num(r.get("ItemId")) <= 0) throw new IllegalArgumentException("$ItemId Not Found.Please Check");
            if (num(r.get("JobLotId")) <= 0) throw new IllegalArgumentException("$JobLotId Not Found.Please Check");
            if (num(r.get("PackingTypeId")) <= 0) throw new IllegalArgumentException("$PackingTypeId Not Found.Please Check");
            if (num(r.get("PackUOMId")) <= 0) throw new IllegalArgumentException("$ItemUomId  Not Found.Please Check");
            if (!(dbl(r.get("ItemQty")) > 0)) throw new IllegalArgumentException("$ItemQty  Not Found.Please Check");
            if (!(dbl(r.get("Rate")) > 0)) throw new IllegalArgumentException("$ItemRate  Not Found.Please Check");
            if (num(r.get("RateUOMId")) == 0) {   // GRN rows: resolve the rate UOM from its equivalent
                double eq = dbl(r.get("RateEquivalent"));
                var match = repo.uoms(num(r.get("ItemId"))).stream().filter(u -> dbl(u.get("Equivalent")) == eq).findFirst();
                if (match.isEmpty()) throw new IllegalArgumentException("RateUom in Row No : " + (grid.indexOf(r) + 1) + " is not define against Item : " + str(r.get("Item")));
                r.put("RateUOMId", num(match.get().get("Id")));
            }
            if (!(dbl(r.get("ItemAmount")) > 0)) throw new IllegalArgumentException("$Item Amount Can't be 0.Please Check");
            if (!(dbl(r.get("BillAmount")) > 0)) throw new IllegalArgumentException("$Item Net Amount Can't be 0.Please Check");
            int refType = num(r.get("RefDocumentTypeId")), refId = num(r.get("RefDocId"));
            if (refType == 0 && refId > 0) throw new IllegalArgumentException("$Ref DocumentType Id not Found Please Check");
            if (refType > 0 && refId == 0) throw new IllegalArgumentException("$RefDocId not Found Please Check");
            if (refType > 0 && dbl(r.get("NetBillWeight")) > dbl(r.get("RefInvoiceWeight"))) throw new IllegalArgumentException("NetBillWeight can not greater than Balance Weight");
        }
        // (9) empty bags - the saved EbPur values are the ones BEFORE this call (list already built) -> keep a copy
        List<Double> ebBefore = new ArrayList<>(); for (var r : grid) ebBefore.add(dbl(r.get("EbPurAgainstWeightAmount")));
        boolean ebProportionate = emptyBagProportion(grid, bagGrid);
        for (int k = 0; k < grid.size(); k++) grid.get(k).put("EbPurAgainstWeightAmount", ebBefore.get(k));
        for (int k = 0; k < bagGrid.size(); k++) {
            var b = bagGrid.get(k); if (toInt(b.get("ItemId")) == 0) continue;
            int t = toInt(b.get("TypeId"));
            if ((t == 2 || t == 3) && toInt(b.get("CreditAccountId")) <= 0 && !ebProportionate) throw new IllegalArgumentException("Credit Account Required in Empty Bag Grid in Row#" + (k + 1));
            if (dbl(b.get("Amount")) == 0) throw new IllegalArgumentException("Packing Material Amount Required in EmptyBag Grid in Row#" + (k + 1));
        }

        // 7.2 header
        var h = writes.defaults("Sp_InvPurchaseInvoice_Insert");
        if (update) h.putAll(ci(old));
        java.sql.Timestamp now = new java.sql.Timestamp(System.currentTimeMillis());
        h.put("Id", update ? recId : 0);
        h.put("BranchesId", context.currentBranchId());
        h.put("BranchSrNo", num(p.get("BranchSrNo")));
        h.put("DocumentTypeId", TYPE);
        h.put("OrganizationId", context.currentOrganizationId()); h.put("CompanyId", context.currentCompanyId());
        h.put("FinancialYearId", context.currentFinancialYearId());
        h.put("EntryUser", context.currentUserId()); h.put("ModifyUser", context.currentUserId());
        h.put("EntryDate", now); h.put("ModifyDate", now);   // Q-H2: re-sent on update
        h.put("DocDate", str(p.get("DocDate")).isEmpty() ? now : p.get("DocDate"));
        h.put("DocNo", num(p.get("DocNo")));
        h.put("SupplierCustomerId", customer);
        h.put("ManualBillNo", str(p.get("ManualBillNo")));
        h.put("SupplierReferenceNo", str(p.get("SupplierReferenceNo")));
        h.put("SupplierInvoiceDate", now);
        h.put("PaymentTermsId", num(p.get("PaymentTermsId")));
        h.put("DueDays", num(p.get("DueDays")));
        h.put("DueDate", str(p.get("DueDate")).isEmpty() ? h.get("DocDate") : p.get("DueDate"));
        h.put("CommAmount", commAmount);
        h.put("CommissionCreditAccountId", num(p.get("CommissionCreditAccountId")));   // Q-H1: sent even when hidden
        h.put("CommissionAgentId", agent);
        h.put("CommissionRemarks", str(p.get("CommissionRemarks")));
        h.put("CommissionType", str(p.get("CommissionType")));
        h.put("CommRate", dbl(p.get("CommRate")));
        h.put("UomScheduleIdCmRate", str(p.get("UomScheduleIdCmRate")));
        h.put("WagesAmount", wagesHeader);
        h.put("RemarksHeader", str(p.get("RemarksHeader")));
        h.put("ScreenName", SCREEN);
        h.put("CustomAccounts", truthy(p.get("CustomAccounts")));
        h.put("ActionId", update ? 2 : 1);
        for (String k : List.of("DeliveryTerm", "StockPartyId", "ReferencePartyId", "ProjectsId", "CurrencyId", "ExchangeRate", "FcyAmount", "BrokerAgentId", "BrokeryRate", "BrokeryAmount", "TransportAccountId", "FreightAmount", "TaxAccountId", "InvoiceTypeId", "DiscountAmount"))
            if (!update) h.put(k, k.equals("DeliveryTerm") ? null : 0);
        if (update) { for (String k : List.of("IsApproved", "PostDate", "PostUser", "AttachmentsValues", "CustomAttachmentsValues")) h.put(k, old.get(k)); }
        // BillAmount: the displayed, rounded value (Q-H4)
        String partyCode = ""; for (var pr : repo.parties()) if (num(pr.get("Id")) == customer) partyCode = str(pr.get("PartyCode"));
        double bill = headerBill(grid, expGrid, glGrid, freightGrid, bagGrid, customer, partyCode, agent, commAmount, subsidiary);
        h.put("BillAmount", round(bill, N));
        h.put("InvoiceQty", sum(grid, "ItemQty")); h.put("InvoiceWeight", sum(grid, "NetBillWeight"));

        // 7.3 details
        var details = new ArrayList<Map<String, Object>>();
        for (var r : grid) {
            var d = writes.defaults("Sp_InvPurchaseInvoiceDetail_Insert");
            d.put("Id", num(r.get("Id"))); d.put("InvGrnId", num(r.get("GrnId"))); d.put("InvGrnDetailId", num(r.get("GrnDetailId")));
            d.put("WarehouseId", num(r.get("WarehouseId"))); d.put("ItemId", num(r.get("ItemId"))); d.put("CropYear", str(r.get("CropYear")));
            d.put("JobLotId", num(r.get("JobLotId"))); d.put("PackingTypeId", num(r.get("PackingTypeId"))); d.put("ItemUOMId", num(r.get("PackUOMId")));
            d.put("ItemQty", dbl(r.get("ItemQty"))); d.put("GrossWeight", dbl(r.get("GrossWeight")));
            d.put("EBWeight", dbl(r.get("EmptyBags"))); d.put("EBTotalWt", dbl(r.get("EmptyBagsTotal")));
            d.put("WeightCut", (double) (float) dbl(r.get("WeightCut"))); d.put("WeightCutTotal", (double) (float) dbl(r.get("WeightCutTotal")));   // ToSingle
            d.put("AdLsWeight", dbl(r.get("AddLss"))); d.put("NetBillWeight", dbl(r.get("NetBillWeight"))); d.put("NetStockWeight", dbl(r.get("StockWeight")));
            d.put("ItemRate", dbl(r.get("Rate"))); d.put("UomScheduleIdRate", num(r.get("RateUOMId")));
            d.put("RateCut", dbl(r.get("RateCut"))); d.put("RateCutAmount", dbl(r.get("RateCutTotal"))); d.put("ItemAmount", dbl(r.get("ItemAmount")));
            d.put("LabAnalisysNo", str(r.get("LabSampleNo")));
            d.put("GpDate", str(r.get("GpDate")).isEmpty() ? java.sql.Date.valueOf("1900-01-01") : r.get("GpDate"));
            d.put("GpNo", toInt(r.get("GpNo"))); d.put("VehicleNo", str(r.get("VehicleNo")));
            d.put("JournalAmount", 0d); d.put("ExpenseAmount", dbl(r.get("ExpenseAmount"))); d.put("FreightAmount", dbl(r.get("FreightAmount")));
            d.put("CommissionAmount", dbl(r.get("CommissionAmount"))); d.put("WagesAmount", dbl(r.get("WagesAmount")));
            d.put("BranchId", num(r.get("BranchId")));
            d.put("RefDocumentTypeId", num(r.get("RefDocumentTypeId"))); d.put("RefDocId", num(r.get("RefDocId"))); d.put("RefDocSubId", num(r.get("RefDocSubId")));
            d.put("EbPurAgainstWeightAmount", dbl(r.get("EbPurAgainstWeightAmount")));
            d.put("Brokery", 0d); d.put("ActionTypeId", 0);
            // carried for the narration / stock rows
            d.put("WareHouseName", str(r.get("Warehouse"))); d.put("EquivalentPoRate", dbl(r.get("RateEquivalent")));
            d.put("BillAmount", dbl(r.get("BillAmount")));
            details.add(d);
        }
        if (!update) for (var d : details) if (num(d.get("Id")) > 0) throw new IllegalArgumentException("Record cannot be inserted because detailId greater than zero");

        // 7.4 children
        var freight = new ArrayList<Map<String, Object>>();
        for (var f : freightGrid) if (toInt(f.get("Transporter")) != 0) {
            var x = writes.defaults("Sp_InvPurchaseInvoiceFreight_Insert");
            x.put("Id", num(f.get("Id"))); x.put("InvGrnId", toInt(f.get("GrnId"))); x.put("TansporterId", toInt(f.get("GlAccountId")));
            x.put("SupplierCustomerId", subsidiary ? toInt(f.get("Transporter")) : 0);
            x.put("FreightAmount", (double) toInt(f.get("Freight"))); x.put("Debit", (double) toInt(f.get("Debit")));   // Q-F3: ToInt
            x.put("Remarks", str(f.get("Remarks"))); freight.add(x);
        }
        var expenses = new ArrayList<Map<String, Object>>();
        for (var e : expGrid) if (toInt(e.get("ItemId")) != 0) {
            var x = writes.defaults("Sp_InvPurchaseInvoiceExpense_Insert");
            x.put("Id", num(e.get("Id"))); x.put("InvRevExpItemId", toInt(e.get("ItemId"))); x.put("Qty", (double) toInt(e.get("Qty")));
            x.put("Rate", dbl(e.get("Rate"))); x.put("Amount", dbl(e.get("Amount"))); x.put("Remarks", str(e.get("Remarks"))); expenses.add(x);
        }
        var journal = new ArrayList<Map<String, Object>>();
        for (var j : glGrid) if (toInt(j.get("AccountId")) != 0) {
            var x = writes.defaults("Sp_InvPurchaseInvoiceJournal_Insert");
            x.put("Id", num(j.get("Id"))); x.put("ChartofAccountId", toInt(j.get("GlAccountId"))); x.put("SupplierCustomerId", subsidiary ? toInt(j.get("AccountId")) : 0);
            x.put("JvRemarks", str(j.get("Remarks"))); x.put("JvPrcnt", dbl(j.get("Percentage"))); x.put("JvQty", dbl(j.get("Qty"))); x.put("JvRate", dbl(j.get("Rate")));
            x.put("JvDebit", dbl(j.get("Debit"))); x.put("JvCredit", dbl(j.get("Credit"))); journal.add(x);
        }
        var bags = new ArrayList<Map<String, Object>>();
        for (var b : bagGrid) if (toInt(b.get("ItemId")) != 0) {
            var x = writes.defaults("Sp_InvPurchaseInvoiceEmptyBags_Insert");
            int t = toInt(b.get("TypeId"));
            x.put("Id", num(b.get("Id"))); x.put("PurchaseOrderId", toInt(b.get("OrderId"))); x.put("TypeId", t); x.put("InvGrnId", toInt(b.get("InvGrnId")));
            x.put("ItemId", toInt(b.get("ItemId"))); x.put("PurchaseQty", dbl(b.get("Qty"))); x.put("Rate", dbl(b.get("Rate"))); x.put("Amount", dbl(b.get("Amount")));
            x.put("CreditAccountId", t == 1 ? 0 : toInt(b.get("CreditAccountId")));
            String remarks = str(b.get("Remarks"));
            if (dbl(b.get("Qty")) > 0 && dbl(b.get("Rate")) > 0 && !remarks.contains("ItemName"))
                remarks = "ItemName : " + str(b.get("ItemName")) + "  Qty: " + fmt(dbl(b.get("Qty"))) + " Rate: " + fmt(dbl(b.get("Rate"))) + " " + remarks;
            x.put("Remarks", remarks); bags.add(x);
        }

        // 7.5 financial (built before the header id exists, as the BLL does)
        var accounts = writes.accounts(h, details);
        boolean cgs = !truthy(repo.config("InventoryFinancialsEffectsInActive"))
                && (truthy(repo.config("CGSEntryAllow")) || repo.features().contains(5) || truthy(repo.config("SaleCostingJobOrderWise")));
        if (cgs) for (var d : details) {
            var c = repo.cgsRate(num(d.get("ItemId")), num(d.get("UomScheduleIdRate")), h.get("DocDate"), update ? recId : 0,
                    num(d.get("RefDocumentTypeId")), num(d.get("RefDocId")), num(d.get("RefDocSubId")));
            d.put("ItemCgsRate", dbl(c.get("CgsRate"))); d.put("EquivalentPoRate", dbl(c.get("RateUom")));
        }
        var voucher = SaleInvoiceReturnFinancialRules.calculate(h, details, freight, journal, bags, accounts,
                new SaleInvoiceReturnFinancialRules.Options(commissionToExpense, cgs));

        // attachments (dialog payload, same stack as the purchase invoices)
        var attachmentChange = p.get("attachments") == null ? null : attachments.prepare(recId, TYPE, p.get("attachments"));
        if (attachmentChange != null) { h.put("AttachmentsValues", attachmentChange.names()); h.put("CustomAttachmentsValues", attachmentChange.storedNames()); }

        // 7.6 DAL SetData
        int newId = writes.execute(update ? "Sp_InvPurchaseInvoice_Update" : "Sp_InvPurchaseInvoice_Insert", h);
        int id = update ? recId : newId;
        if (id <= 0) throw new IllegalStateException("Invoice procedure did not return an ID");
        h.put("Id", id);
        h.put("DocNo", jdbc.queryForObject("SELECT DocNo FROM dbo.InvPurchaseInvoice WHERE Id=?", Integer.class, id));
        int line = 0; var ids = new ArrayList<String>();
        for (var d : details) {
            d.put("InvPurchaseInvoiceId", id); d.put("LineId", ++line);
            // D:76-86: BillAmount recomputed for 98
            d.put("BillAmount", dbl(d.get("ItemAmount")) + dbl(d.get("FreightAmount")) + dbl(d.get("ExpenseAmount")) + (wagesToProduct ? dbl(d.get("WagesAmount")) : 0) - dbl(d.get("CommissionAmount")) - dbl(d.get("EbPurAgainstWeightAmount")));
            int did = writes.execute("Sp_InvPurchaseInvoiceDetail_Insert", d);
            if (did <= 0) did = num(d.get("Id"));
            d.put("Id", did); ids.add(String.valueOf(did));
        }
        for (var x : freight) { x.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceFreight_Insert", x); }
        for (var x : journal) { x.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceJournal_Insert", x); }
        for (var x : expenses) { x.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceExpense_Insert", x); }
        for (var x : bags) { x.put("InvPurchaseInvoiceId", id); writes.execute("Sp_InvPurchaseInvoiceEmptyBags_Insert", x); }
        ProcExec.run(jdbc, "EXEC dbo.usp_PurchaseInvoiceDetailRemoveByDetailIdsAndValidations @OrganizationId=?,@CompanyId=?,@Id=?,@DetailIds=?", h.get("OrganizationId"), h.get("CompanyId"), id, String.join(",", ids) + ",");
        if (attachmentChange != null) attachments.persist(id, TYPE, customer, attachmentChange);
        writes.post(h, voucher);   // stock evaluation, voucher (+balance check, history), inventory transactions, approval

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true); out.put("id", id); out.put("docNo", h.get("DocNo"));
        out.put("message", update ? "Record Update Successfully" : "Record Saved Successfully");
        out.put("voucherHeadId", repo.voucherHeadId(id));
        boolean firstIsManual = num(details.get(0).get("InvGrnId")) == 0;
        out.put("openWagesBill", firstIsManual && Boolean.TRUE.equals(flags.get("wagesStatus")) && Boolean.TRUE.equals(flags.get("wagesActiveOrInActive")));
        out.put("grossWeightTotal", sum(grid, "GrossWeight"));
        return out;
    }
    private static String fmt(double v) { return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString(); }

    // ------------------------------------------------------------------ delete :4030
    @Transactional
    public void delete(int id) {
        if (id == 0) throw new IllegalArgumentException("RecordId not found for delete");
        var old = repo.header(id);
        if (old == null) throw new IllegalArgumentException("Record not found");
        if (truthy(old.get("IsApproved"))) throw new IllegalArgumentException("Record Not Delete because Record has approved");
        if (!repo.hasRight("Delete")) throw new IllegalArgumentException("You do not have Delete rights on InvfrmSaleInvoiceReturn");
        repo.delete(id);
    }

    // ------------------------------------------------------------------ read :2609
    public Map<String, Object> read(int id) {
        var h = repo.read(id);
        if (h == null) throw new IllegalArgumentException("Record not found");
        return h;
    }

    // ------------------------------------------------------------------ loader (LoadSaleInvoiceForReturn)
    public Map<String, Object> loaderInit() {
        boolean branchImplemented = truthy(repo.config("SaleInvoiceReturnBranchWise"));
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("branches", repo.loaderBranches(branchImplemented));
        m.put("fromDate", repo.financialYearStart());
        m.put("toDate", java.time.LocalDate.now().toString());
        m.put("userBranchId", context.currentBranchId());
        return m;
    }
    public List<Map<String, Object>> pending(String fromDate, String toDate, String branchesIds) {
        if (str(branchesIds).isEmpty()) throw new IllegalArgumentException("Select branch first");
        return repo.pendingSaleInvoices(fromDate, toDate, branchesIds);
    }
    /** BtnLoad L:458 checks + AddInGridDetailFromSaleInvoice :5525 data (rows, wages, freight). */
    public Map<String, Object> loadSaleInvoices(List<Integer> invoiceIds, String fromDate, String toDate, String branchesIds, boolean branchImplemented) {
        if (invoiceIds == null || invoiceIds.isEmpty()) throw new IllegalArgumentException("Chek the row first");
        var all = repo.pendingSaleInvoices(fromDate, toDate, branchesIds);
        var rows = new ArrayList<Map<String, Object>>();
        int sId = 0, docType = 0, branchId = 0;
        for (var r : all) if (invoiceIds.contains(num(r.get("Id")))) {
            if (sId == 0) sId = num(r.get("SupplierCustomerId")); else if (sId != num(r.get("SupplierCustomerId"))) throw new IllegalArgumentException("Sorry!. The Selected Invoices are not of same Customer");
            if (docType == 0) docType = num(r.get("DocumentTypeId")); else if (docType != num(r.get("DocumentTypeId"))) throw new IllegalArgumentException("Sorry!. The Selected Invoices are not of same DocumentType");
            if (branchImplemented) { if (branchId == 0) branchId = num(r.get("BranchId")); else if (branchId != num(r.get("BranchId"))) throw new IllegalArgumentException("Sorry!. The Selected Invoices are not of same Branch"); }
            rows.add(r);   // Q-L1: every pending line of each checked invoice
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        if (rows.isEmpty()) return out;
        StringBuilder ids = new StringBuilder(); for (var r : rows) ids.append(",").append(num(r.get("Id")));
        out.put("wagesAmount", repo.wages(docType, ids.toString()));
        out.put("freight", repo.saleInvoiceFreight(ids.toString()));
        return out;
    }
}
