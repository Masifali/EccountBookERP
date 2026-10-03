package com.mst.services;

import com.mst.models.saleinvoice.SaleInvoiceModels;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;

/**
 * Shared, logic-free helpers of the two DocumentTypeId 204 commercial-invoice screens (211 ExImCommercialInvoice,
 * 880 frmCommercialInvoiceIII): the desktop's Conversion.* semantics, the projections of the GetByID child lists to
 * each form's DataTable columns, and BLL ExImInvoice.MakeVoucherForExImInvoice (BLL 0467:60) for DocumentTypeId 204.
 */
public final class ExportCommercialInvoiceSupport {

    private ExportCommercialInvoiceSupport() { }

    // ================================================================== conversions

    public static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    public static String text(Object v) { return v == null ? "" : String.valueOf(v); }
    public static String trim(Object v) { return text(v).trim(); }

    public static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        if (v instanceof Boolean) return ((Boolean) v) ? 1 : 0;
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); }
        catch (NumberFormatException e) { try { return (int) Double.parseDouble(s); } catch (NumberFormatException e2) { return 0; } }
    }

    public static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return 0;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0; }
    }

    public static BigDecimal dec(Object v) {
        if (v instanceof BigDecimal) return (BigDecimal) v;
        return BigDecimal.valueOf(asDouble(v));
    }

    /** Conversion.ToBool: "true"/"false", else a number != 0, else false. */
    public static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = String.valueOf(v).trim();
        if (s.equalsIgnoreCase("true")) return true;
        if (s.equalsIgnoreCase("false") || s.isEmpty()) return false;
        try { return Integer.parseInt(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    /** .NET Math.Round(x, 0): banker's rounding (MidpointRounding.ToEven). */
    public static double netRound(double v) { return Math.rint(v); }

    public static LocalDate asDate(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).toLocalDate();
        if (v instanceof LocalDate) return (LocalDate) v;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); } catch (DateTimeParseException e) { return null; }
    }

    public static java.sql.Date sqlDate(Object v) { LocalDate d = asDate(v); return d == null ? null : java.sql.Date.valueOf(d); }

    /** DateTimePicker.Value from the page (yyyy-MM-dd); blank is "now" - a picker is never empty. */
    public static Timestamp ts(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return new Timestamp(System.currentTimeMillis());
        try {
            if (s.length() == 10) return Timestamp.valueOf(LocalDate.parse(s).atTime(LocalDateTime.now().toLocalTime().withNano(0)));
            return Timestamp.valueOf(LocalDateTime.parse(s.length() == 16 ? s + ":00" : s));
        } catch (DateTimeParseException e) {
            try { return Timestamp.valueOf(s.replace('T', ' ')); } catch (IllegalArgumentException e2) { return new Timestamp(System.currentTimeMillis()); }
        }
    }

    public static String iso(Object v) {
        LocalDate d = asDate(v);
        return d == null ? "" : d.toString();
    }

    public static String isoDateTime(Object v) {
        if (v == null) return "";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).withNano(0).toString();
        return String.valueOf(v);
    }

    /** DateTime.ToShortDateString() (M/d/yyyy). */
    public static String shortDate(Object v) {
        LocalDate d = asDate(v);
        if (d == null) d = LocalDate.of(1, 1, 1);           // Conversion.ToDateTime(null) is DateTime.MinValue
        return d.getMonthValue() + "/" + d.getDayOfMonth() + "/" + d.getYear();
    }

    /** double.ToString() for comments ("G": no trailing zeros, no exponent for ordinary values). */
    public static String csNum(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        String s = BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
        return s;
    }

    @SuppressWarnings("unchecked")
    public static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    @SuppressWarnings("unchecked")
    public static Map<String, Object> map(Object v) { return v instanceof Map ? (Map<String, Object>) v : new LinkedHashMap<>(); }

    public static String msg(Throwable e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    /**
     * The form's ContractIds / ContractNos / ... reduction: ",a,b,a".Split(',').Distinct() - one element -> that
     * element, otherwise string.Join(",", ...).TrimStart(','). Kept literal (the leading "" survives Distinct).
     */
    public static String distinctJoin(String csv) {
        if (csv == null || csv.isEmpty()) return null;
        LinkedHashSet<String> set = new LinkedHashSet<>();
        for (String s : csv.split(",", -1)) set.add(s);
        if (set.size() == 1) return set.iterator().next();
        String j = String.join(",", set);
        int i = 0;
        while (i < j.length() && j.charAt(i) == ',') i++;
        return j.substring(i);
    }

    /** FormHelper.ValidateField. */
    public static void validateField(Object value, String fieldName, int rowIndex, String gridName) {
        boolean bad = value == null
                || (value instanceof Integer && (Integer) value == 0)
                || (value instanceof Double && (Double) value <= 0)
                || (value instanceof BigDecimal && ((BigDecimal) value).signum() <= 0)
                || (value instanceof String && ((String) value).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(fieldName + " is required in " + gridName + " at row No: " + (rowIndex + 1));
    }

    public static Map<String, Object> saved(int id, String message) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", message);
        return out;
    }

    public static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idCol)));
            m.put("Name", text(ci(r, nameCol)));
            out.add(m);
        }
        return out;
    }

    /** Copies the listed columns (case-insensitive) into an ordered map; numbers stay numbers. */
    public static Map<String, Object> pick(Map<String, Object> r, String... cols) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (String c : cols) {
            Object v = ci(r, c);
            if (v instanceof Timestamp || v instanceof java.sql.Date) v = iso(v);
            else if (v instanceof BigDecimal) v = ((BigDecimal) v).doubleValue();
            m.put(c, v);
        }
        return m;
    }

    // ================================================================== GetByID child projections (common)

    public static Map<String, Object> paymentTermRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "Id")));
        m.put("PaymentTermId", asInt(ci(d, "PaymentTermId")));
        m.put("PaymentTerm", text(ci(d, "PaymentTerm")));
        m.put("DocumentTypeId", asInt(ci(d, "DocumentTypeId")));
        m.put("ExImEFormRegistrationId", asInt(ci(d, "ExImEFormRegistrationId")));
        m.put("FinancialInstrumentNo", text(ci(d, "FinancialInstrumentNo")));
        m.put("PrcntOfTotal", asDouble(ci(d, "PrcntOfTotal")));
        m.put("FcyAmount", asDouble(ci(d, "FcyAmount")));
        m.put("DueDays", asInt(ci(d, "DueDays")));
        m.put("Remarks", text(ci(d, "PaymentRemarks")));
        return m;
    }

    public static Map<String, Object> otherChargeRow(Map<String, Object> d, boolean custom) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "Id")));
        if (!custom) {
            m.put("ContractId", asInt(ci(d, "ContractId")));
            m.put("ContractOtherChargesDetailId", asInt(ci(d, "ContractOtherChargesDetailId")));
        }
        m.put("ChargesItemId", asInt(ci(d, "ChargesItemId")));
        m.put("ChargesItem", text(ci(d, "ChargesItemName")));
        m.put("AddAmount", asDouble(ci(d, "AddAmount")));
        m.put("LessAmount", asDouble(ci(d, "LessAmount")));
        m.put("Remarks", text(ci(d, "Remarks")));
        return m;
    }

    public static Map<String, Object> otherItemRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("ItemId", asInt(ci(d, "otherItemId")));
        m.put("ItemName", text(ci(d, "ItemName")));
        m.put("Qty", asDouble(ci(d, "oItemQty")));
        m.put("Rate", asDouble(ci(d, "oItemRate")));
        m.put("Amount", asDouble(ci(d, "oItemAmount")));
        m.put("Remarks", text(ci(d, "OtherItemRemarks")));
        return m;
    }

    public static Map<String, Object> commissionRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "exImInvoiceSaleManCommissionId")));
        m.put("SalesPersonId", asInt(ci(d, "SalesPersonId")));
        m.put("SalesPerson", text(ci(d, "SalesPerson")));
        m.put("CommissionTypeId", asInt(ci(d, "commissionTypeId")));
        m.put("CommissionType", text(ci(d, "CommissionType")));
        m.put("Rate", asDouble(ci(d, "commissionRate")));
        m.put("RateUom", text(ci(d, "rateUom")));
        m.put("FcyAmount", asDouble(ci(d, "fcyAmount")));
        m.put("ExchangeRate", asDouble(ci(d, "exchangeRate")));
        m.put("LcyAmount", asDouble(ci(d, "lcyAmount")));
        m.put("Remarks", text(ci(d, "Remarks")));
        return m;
    }

    // ================================================================== child models (non-virtual properties, SetProc order)

    /** ExImInvoicePaymentTermsDetail(Custom) as Insert()/btnsave_Click fill it (Id, EntryUserId, ModifyUserId, RefDocumentTypeId stay 0). */
    public static Map<String, Object> paymentTermModel(Map<String, Object> r, int sortNo) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", 0);
        d.put("ExImInvoiceId", 0);
        d.put("DocumentTypeId", asInt(r.get("DocumentTypeId")));
        d.put("ExImEFormRegistrationId", asInt(r.get("ExImEFormRegistrationId")));
        d.put("FinancialInstrumentNo", text(r.get("FinancialInstrumentNo")));
        d.put("PaymentTermId", asInt(r.get("PaymentTermId")));
        d.put("PrcntOfTotal", asDouble(r.get("PrcntOfTotal")));
        d.put("FcyAmount", asDouble(r.get("FcyAmount")));
        d.put("DueDays", asInt(r.get("DueDays")));
        d.put("SortNo", sortNo);
        d.put("PaymentRemarks", text(r.get("Remarks")));
        d.put("EntryUserId", 0);
        d.put("ModifyUserId", 0);
        d.put("RefDocumentTypeId", 0);
        return d;
    }

    public static Map<String, Object> otherChargeModel(Map<String, Object> r, boolean custom) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", custom ? 0 : asInt(r.get("Id")));
        d.put("ExImInvoiceId", 0);
        d.put("ChargesItemId", asInt(r.get("ChargesItemId")));
        d.put("AddAmount", asDouble(r.get("AddAmount")));
        d.put("LessAmount", asDouble(r.get("LessAmount")));
        d.put("Remarks", text(r.get("Remarks")));
        if (!custom) {
            d.put("ContractId", asInt(r.get("ContractId")));
            d.put("ContractOtherChargesDetailId", asInt(r.get("ContractOtherChargesDetailId")));
        }
        return d;
    }

    public static Map<String, Object> otherItemModel(Map<String, Object> r) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", 0);
        d.put("ExImInvoiceId", 0);
        d.put("otherItemId", asInt(r.get("ItemId")));
        d.put("oItemQty", dec(r.get("Qty")));
        d.put("oItemWeightKgs", BigDecimal.ZERO);
        d.put("oItemRate", asDouble(r.get("Rate")));
        d.put("oItemAmount", dec(r.get("Amount")));
        d.put("OtherItemRemarks", text(r.get("Remarks")));
        return d;
    }

    /** ExImInvoicePackingDetail with every int/double property at its default (strings left null = not sent). */
    public static Map<String, Object> blankPacking(boolean custom) {
        Map<String, Object> d = new LinkedHashMap<>();
        String[] ints = {"Id", "ExImInvoiceId", "ItemId", "ItemUOMId", "BrandId", "PackingMaterialTypeId", "InnerQtyUomId", "OuterQtyUomId",
                "RateUomId", "ItemSpicificationId", "CropYearId", "JobLotId", "ContractDetailId", "ContractId", "PreInvoiceId",
                "PreInvoiceDetailId", "LineId", "ContractScheduleId", "ActionTypeId", "AmountCalulationId", "ExImTradeTypeId",
                "ExImFarmingTypeId", "ExImFarmingNTradeId", "ContractScheduleDetailId", "LabInspectionId", "PmItemId"};
        String[] dbls = {"InnerQty", "OuterQty", "NetWeight", "RatePrice", "FcAmount", "PackingWeight", "TotalPackingWeight", "GrossWeight",
                "MTon", "NoofContainers", "NoofBagsPerContainer", "OtherRate", "OtherAmount", "RateAddLess", "RateWithoutAddLess", "PalletQty"};
        for (String k : ints) d.put(k, 0);
        for (String k : dbls) d.put(k, 0d);
        if (!custom) d.put("AddLessAmount", 0d);
        return d;
    }

    // ================================================================== BLL ExImInvoice.MakeVoucherForExImInvoice (DocumentTypeId 204)

    public static class Voucher {
        public SaleInvoiceModels.VoucherHead head;
        public List<SaleInvoiceModels.VoucherDetail> details = new ArrayList<>();
        /** obj.CreditAccountId as the BLL leaves it (it overwrites a 0 with the item's SaleGLAC before the header is saved). */
        public int creditAccountId;
    }

    /**
     * BLL 0467:60 for DocumentTypeId 204: one debit/credit pair per packing row that is not removed (ActionTypeId 3),
     * one pair per other item. Quirks kept: RefDocNoId / DocumentTypeSrNo are the header Id at build time (0 on insert,
     * the DAL then sets DocumentTypeSrNo); a CreditAccountId of 0 is replaced by the first item's SaleGLAC and that value
     * is saved on the header too; the other-item fallback looks the item up by oItemAmount (sic); the credit line of an
     * other item carries no DocumentTypeIdRef / RefInvoiceNo (the BLL sets them on the debit line twice).
     */
    public static Voucher makeVoucher(Map<String, Object> h, List<Map<String, Object>> packing, List<Map<String, Object>> otherItems,
                                      List<Map<String, Object>> supplierGl, List<Map<String, Object>> itemGl) {
        Voucher out = new Voucher();
        SaleInvoiceModels.VoucherHead vh = new SaleInvoiceModels.VoucherHead();
        int documentTypeId = asInt(h.get("DocumentTypeId"));
        int headerId = asInt(h.get("Id"));
        double rate = asDouble(h.get("ConversionRate"));
        int fcyId = asInt(h.get("FcurrencyId"));
        int customerId = asInt(h.get("SupplierCustomerId"));
        int creditAccountId = asInt(h.get("CreditAccountId"));
        vh.DocumentTypeId = documentTypeId;
        vh.DocumentTypeSrNo = headerId;
        vh.RefDocNoId = headerId;
        vh.VoucherCode = asInt(h.get("DocCode"));
        vh.VoucherDate = ((Timestamp) h.get("DocDate")).toLocalDateTime();
        vh.Remarks = text(h.get("RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.VoucherAmount = asDouble(h.get("EquivalentAmount"));
        vh.FcAmount = asDouble(h.get("TotalAmount"));
        vh.ExchangeCurrencyRate = rate;
        vh.MultiCurrencyId = fcyId;
        if (!supplierGl.isEmpty()) {
            Map<String, Object> c = null;
            for (Map<String, Object> r : supplierGl) if (asInt(ci(r, "Id")) == customerId) { c = r; break; }
            if (c == null) throw new IllegalStateException("Customer GLAccountId not Found");
            vh.RefAccountId = asInt(ci(c, "GlAccountId"));
        }
        vh.ChequeDate = LocalDate.now().atStartOfDay();
        vh.IncludeWHT = false;
        vh.BranchId = asInt(h.get("BranchesId"));
        vh.ProjectId = asInt(h.get("ProjectsId"));
        vh.BillAmount = asDouble(h.get("TotalAmount"));
        vh.ManualBillNo = text(h.get("InvoiceNo"));
        vh.DueDate = ((Timestamp) h.get("EFormDate")).toLocalDateTime();
        vh.DueDays = 0;
        vh.OrganizationId = asInt(h.get("OrganizationId"));
        vh.CompanyId = asInt(h.get("CompanyId"));
        vh.FinancialYearId = asInt(h.get("FinancialYearId"));
        vh.EntryUser = asInt(h.get("EntryUser"));
        vh.EntryDate = LocalDateTime.now();
        vh.ModifyDate = LocalDateTime.now();
        vh.ModifyUser = asInt(h.get("ModifyUser"));
        vh.ActionId = 1;
        out.head = vh;

        if (documentTypeId == 204 || documentTypeId == 211 || documentTypeId == 212) {
            if (!packing.isEmpty() && documentTypeId != 212) {
                for (Map<String, Object> it : packing) {
                    if (asInt(it.get("ActionTypeId")) == 3) continue;
                    double outerQty = asDouble(it.get("OuterQty")), netWeight = asDouble(it.get("NetWeight"));
                    double ratePrice = asDouble(it.get("RatePrice")), fcAmount = asDouble(it.get("FcAmount"));
                    int itemId = asInt(it.get("ItemId"));
                    String comments = "   NoOfBags: " + csNum(outerQty) + "   M.Ton: " + csNum(netWeight / 1000.0) + "  @Rate: " + csNum(ratePrice) + "  FcAmount:" + csNum(fcAmount);
                    if (creditAccountId == 0) {
                        if (itemGl.isEmpty()) throw new IllegalStateException("Item Record Not found");
                        Map<String, Object> g = null;
                        for (Map<String, Object> r : itemGl) if (asInt(ci(r, "Id")) == itemId) { g = r; break; }
                        if (g == null) throw new IllegalStateException("Item Record Not found");
                        creditAccountId = asInt(ci(g, "SaleGLAC"));
                    }
                    if (creditAccountId == 0) throw new IllegalStateException("CreditAccount Not Found please Check");
                    double amount = fcAmount * rate;
                    out.details.add(itemLine(vh.RefAccountId, creditAccountId, comments, amount, 0, itemId, outerQty, ratePrice, netWeight, fcAmount, rate, fcyId, customerId));
                    out.details.add(itemLine(creditAccountId, vh.RefAccountId, comments, 0, amount, itemId, outerQty, ratePrice, netWeight, fcAmount, rate, fcyId, customerId));
                }
            }
            for (Map<String, Object> oi : otherItems) {
                double oAmount = asDouble(oi.get("oItemAmount"));
                if (creditAccountId == 0) {
                    if (itemGl.isEmpty()) throw new IllegalStateException("OtherItem Record Not found");
                    Map<String, Object> g = null;
                    int byAmount = (int) oAmount;                       // BLL: x.Id == Conversion.ToInt(item.oItemAmount)
                    for (Map<String, Object> r : itemGl) if (asInt(ci(r, "Id")) == byAmount) { g = r; break; }
                    if (g == null) throw new IllegalStateException("OtherItem Record Not found");
                    creditAccountId = asInt(ci(g, "SaleGLAC"));
                }
                if (creditAccountId == 0) throw new IllegalStateException("CreditAccount Not Found against Other Item please Check");
                double amount = oAmount * rate;
                double qty = asDouble(oi.get("oItemQty"));
                SaleInvoiceModels.VoucherDetail dr = otherLine(vh.RefAccountId, creditAccountId, text(oi.get("OtherItemRemarks")), amount, 0,
                        asInt(oi.get("otherItemId")), qty, asDouble(oi.get("oItemRate")), oAmount, rate, fcyId);
                dr.DocumentTypeIdRef = 1;
                dr.RefInvoiceNo = "SALE";
                out.details.add(dr);
                out.details.add(otherLine(creditAccountId, vh.RefAccountId, text(oi.get("OtherItemRemarks")), 0, amount,
                        asInt(oi.get("otherItemId")), qty, asDouble(oi.get("oItemRate")), oAmount, rate, fcyId));
            }
        }
        out.creditAccountId = creditAccountId;
        return out;
    }

    private static SaleInvoiceModels.VoucherDetail itemLine(int account, int against, String comments, double debit, double credit, int itemId,
                                                           double qtyOut, double rate, double weight, double fcAmount, double exRate, int fcyId, int customerId) {
        SaleInvoiceModels.VoucherDetail d = new SaleInvoiceModels.VoucherDetail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        d.ItemId = itemId;
        d.QtyOut = qtyOut;
        d.ItemRate = rate;
        d.WeightOut = weight;
        d.RateCut = 0;
        d.RateCutAmount = 0;
        d.ItemAmount = debit != 0 ? debit : credit;
        d.DCurrencyAmount = fcAmount;
        d.DExchangeCurrencyRate = exRate;
        d.DMultiCurrencyId = fcyId;
        d.Freight = 0; d.Expenses = 0; d.Journal = 0; d.JobLotId = 0; d.Commission = 0; d.OrderNo = 0; d.GpNo = 0;
        d.VehicleNo = "";
        d.SupplierCustomerId = customerId;
        d.DocumentTypeIdRef = 1;
        d.RefInvoiceNo = "SALE";
        return d;
    }

    private static SaleInvoiceModels.VoucherDetail otherLine(int account, int against, String comments, double debit, double credit, int itemId,
                                                            double qty, double rate, double fcAmount, double exRate, int fcyId) {
        SaleInvoiceModels.VoucherDetail d = new SaleInvoiceModels.VoucherDetail();
        d.AccountId = account;
        d.AgainstAccountId = against;
        d.Comments = comments;
        d.DebitAmount = debit;
        d.CreditAmount = credit;
        d.ItemId = itemId;
        d.QtyOut = qty;
        d.ItemRate = rate;
        d.WeightOut = qty;
        d.ItemAmount = debit != 0 ? debit : credit;
        d.DCurrencyAmount = fcAmount;
        d.DExchangeCurrencyRate = exRate;
        d.DMultiCurrencyId = fcyId;
        return d;
    }
}
