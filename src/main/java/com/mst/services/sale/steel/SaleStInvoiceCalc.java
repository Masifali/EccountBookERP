package com.mst.services.sale.steel;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.text.DecimalFormat;
import java.text.DecimalFormatSymbols;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.function.ToDoubleFunction;

import static com.mst.services.sale.engr.SaleEngrSupport.toDouble;
import static com.mst.services.sale.engr.SaleEngrSupport.toInt;

/**
 * Screen 543 "SaleInvoice_St" = Architecture.WinApp.Steel.Sale.SaleInvoice_St (Steel Sale Invoice, module 84, document type 1509).
 *
 * The desktop keeps its totals in text boxes and in cells of the Janus grids and recalculates them from the grid events. This class is the
 * server-side copy of those event handlers, working on a state object (the text of the text boxes, the rows of the four grids) and answering the
 * same state: BillAmount :1121, TotalCommissionAmount :1488, ExpProportion :1838, FreightProportion :1871, LedgerProportion :1904, BillProportion :1963,
 * CommissionProportion :1996, CalculateTotalInformation :3361, txtExchangeRate_TextChanged :3326, grd_CellUpdated :930, grdFreight_* :723/:755,
 * grdInvExp_* :770/:802, grdGLedger_* :835/:866. Every desktop method has its own try/catch that shows ex.Message: a failure is added to Inv.messages
 * and the rest of that method is skipped, like the MessageBox does.
 *
 * .NET number behaviour kept: Math.Round(double, n) = round-half-even of v*10^n (Math.rint), Math.Round(decimal, n) half-even, custom format strings
 * ("#,##0.##", stringFormatsingle ...) round half away from zero after 15 significant digits, decimal(double) keeps 15 significant digits.
 */
public class SaleStInvoiceCalc {

    /** clsGlobalVariables.GetDecimalConfiguration + the CreditAmountInItemSaleGL configuration. */
    public static class Cfg {
        public int amtRound, amtDec, fcyRound, fcyDec, rateDec;
        /** raw ConfigKey of CreditAmountInItemSaleGL, null when the configuration row is missing. */
        public String creditRaw;
    }

    /** dtGrid (41 columns, the names are the column names of the desktop DataTable). */
    public static class Line {
        public int Id, InvGdnId, InvGdnDetailId, SaleOrderId, SaleOrderDetailId, SaleOrder, ItemId, JobLotId, PackingTypeId, PackUomId, RateUomId, WarehouseId, CityId, GpNo;
        public String ItemName = "", JobLot = "", PackingType = "", PackUom = "", RateUom = "", Warehouse = "", CityName = "", VehicleNo = "";
        public double ItemQty, GrossWeight, WtCut, WtCutTotal, AddLessWeight, NetBillWeight, StockWeight, ItemRate, EquivalentSoRate, RateCut, RateCutAmount, ItemAmount,
                BillAmount, ExchangeRate, FcyAmount, Freights, Expense, Commission, Journal;
    }

    /** dtFreight: InvGdnId, Transporter, Freight (caption Credit), Debit. */
    public static class Fr { public int InvGdnId, Transporter; public double Freight, Debit; }

    /** dtInvExp: ItemId, Qty, Rate, Amount, Remarks. */
    public static class Ex { public int ItemId; public double Qty, Rate, Amount; public String Remarks = ""; }

    /** dtGrdGL: AccountId, Remarks, Percentage, Qty, Rate, Debit, Credit. */
    public static class Jv { public int AccountId; public String Remarks = ""; public double Percentage, Qty, Rate, Debit, Credit; }

    /** The form: text boxes as text, combos as ids / text, the four grids and the private fields of SaleInvoice_St. */
    public static class Inv {
        public int id, branchId, projectId, supplierId, supplierGlId, paymentTermId, currencyId, commAgentId, soid;
        public String docNo = "", docDate = "", refNo = "", billNo = "", dueDays = "", dueDate = "", deliveryTerm = "", exchangeRate = "", totalQty = "", totalWeight = "",
                fcyAmount = "", billAmount = "", commType = "", commRate = "", commUom = "", commAmount = "", commRemarks = "", remarks = "", orderType = "";
        /** SupplierGLDebitAmount / SupplierGLCreditAmount / SupplierCommissionAmount (private fields, only changed by BillAmount()). */
        public double glDebit, glCredit, supplierCommission;
        public List<Line> lines = new ArrayList<>();
        public List<Fr> freight = new ArrayList<>();
        public List<Ex> exp = new ArrayList<>();
        public List<Jv> gl = new ArrayList<>();
        public List<String> messages = new ArrayList<>();
    }

    private final Cfg c;
    private final Inv v;

    public SaleStInvoiceCalc(Cfg c, Inv v) {
        this.c = c; this.v = v;
        if (v.lines == null) v.lines = new ArrayList<>();
        if (v.freight == null) v.freight = new ArrayList<>();
        if (v.exp == null) v.exp = new ArrayList<>();
        if (v.gl == null) v.gl = new ArrayList<>();
        v.messages = new ArrayList<>();
    }

    // ------------------------------------------------------------------ .NET number helpers

    /** Math.Round(double, digits). */
    public static double rnd(double x, int digits) {
        if (digits < 0 || digits > 15) throw new IllegalArgumentException("Rounding digits must be between 0 and 15, inclusive.");
        if (Double.isNaN(x) || Double.isInfinite(x)) return x;
        if (Math.abs(x) >= 1e16) return x;
        double p = Math.pow(10, digits);
        return Math.rint(x * p) / p;
    }

    /** (decimal)double: 15 significant digits. */
    public static BigDecimal dec(double x) {
        if (Double.isNaN(x) || Double.isInfinite(x)) throw new IllegalArgumentException("Value was either too large or too small for a Decimal.");
        return new BigDecimal(x).round(new MathContext(15, RoundingMode.HALF_EVEN));
    }

    /** Conversion.ToDecimal(text). */
    public static BigDecimal decText(String s) {
        try { return new BigDecimal(s == null ? "0" : s.trim().replace(",", "")); } catch (RuntimeException e) { return BigDecimal.ZERO; }
    }

    /** Math.Round(decimal, digits). */
    public static BigDecimal rndDec(BigDecimal x, int digits) {
        if (digits < 0 || digits > 28) throw new IllegalArgumentException("Rounding digits must be between 0 and 28, inclusive.");
        return x.setScale(digits, RoundingMode.HALF_EVEN);
    }

    /** double.ToString(): shortest round-trip text without exponent for the usual magnitudes. */
    public static String num(double x) {
        if (Double.isNaN(x)) return "NaN";
        if (Double.isInfinite(x)) return x > 0 ? "Infinity" : "-Infinity";
        if (x == 0) return "0";
        return new BigDecimal(Double.toString(x)).stripTrailingZeros().toPlainString();
    }

    /** decimal.ToString(): scale kept. */
    public static String num(BigDecimal x) { return x.toPlainString(); }

    private static final DecimalFormatSymbols SYM = DecimalFormatSymbols.getInstance(Locale.US);

    /** Custom numeric format "#,##0" + "." + n zeros (n == 0: no decimals) applied to a double: 15 significant digits first, then half away from zero. */
    public static String fmt(double x, int dec) {
        if (Double.isNaN(x)) return "NaN";
        if (Double.isInfinite(x)) return x > 0 ? "Infinity" : "-Infinity";
        return fmt(new BigDecimal(x).round(new MathContext(15, RoundingMode.HALF_EVEN)), dec);
    }

    public static String fmt(BigDecimal x, int dec) {
        StringBuilder p = new StringBuilder("#,##0");
        if (dec > 0) { p.append('.'); for (int i = 0; i < dec; i++) p.append('0'); }
        DecimalFormat f = new DecimalFormat(p.toString(), SYM);
        f.setRoundingMode(RoundingMode.HALF_UP);
        f.setParseBigDecimal(true);
        String s = f.format(x);
        return s;
    }

    /** "#,##0.##" / "#,##0.###" on a decimal. */
    public static String hash(BigDecimal x, int maxDec) {
        StringBuilder p = new StringBuilder("#,##0.");
        for (int i = 0; i < maxDec; i++) p.append('#');
        DecimalFormat f = new DecimalFormat(p.toString(), SYM);
        f.setRoundingMode(RoundingMode.HALF_UP);
        String s = f.format(x);
        return s.endsWith(".") ? s.substring(0, s.length() - 1) : s;
    }

    private static double D(String s) { return toDouble(s); }

    private String amt(double x) { return fmt(x, c.amtDec); }
    private String fcy(BigDecimal x) { return fmt(x, c.fcyDec); }

    private double tot(ToDoubleFunction<Line> f) { double s = 0; for (Line l : v.lines) s += f.applyAsDouble(l); return s; }
    private double totExp() { double s = 0; for (Ex e : v.exp) s += e.Amount; return s; }
    private double fin(double x) { return Double.isNaN(x) || Double.isInfinite(x) ? 0.0 : x; }   // Conversion.ToDouble(text of NaN / Infinity)

    private void tryRun(Runnable r) {
        try { r.run(); } catch (RuntimeException e) { v.messages.add(e.getMessage() == null ? e.toString() : e.getMessage()); }
    }

    // ------------------------------------------------------------------ the desktop methods

    /** BillAmount :1121 */
    public void billAmount() {
        tryRun(() -> {
            double item = fin(tot(l -> l.ItemAmount));
            double ex = fin(totExp());
            double jd = 0, jc = 0, tc = 0;
            for (Jv r : v.gl) if (r.AccountId > 0) { jd += r.Debit; jc += r.Credit; v.glDebit = jd; v.glCredit = jc; }
            for (Fr r : v.freight) if (r.Transporter > 0 && v.supplierGlId > 0 && v.supplierGlId == r.Transporter) tc += r.Freight;
            double bill = item + ex;
            if (tc > 0.0) bill -= Math.abs(tc);
            double gd = v.glCredit - v.glDebit;
            double dg = Math.abs(gd);
            bill = gd < 0.0 ? bill - dg : bill + gd;
            if (v.supplierId == v.commAgentId) {
                if (!v.commRate.isEmpty()) { bill -= D(v.commAmount.trim()); v.supplierCommission = D(v.commAmount.trim()); }
                else v.commAmount = "0";
            } else v.supplierCommission = 0.0;
            v.billAmount = amt(bill);
            billProportion();
        });
    }

    /** TotalCommissionAmount :1488 */
    public void totalCommissionAmount() {
        tryRun(() -> {
            if (!v.commRate.isEmpty()) {
                String t = v.commType;
                if ("Flat".equals(t)) {
                    v.commAmount = amt(D(v.commRate.trim()));
                } else if ("Percent".equals(t) || "Percentage".equals(t)) {
                    double rate = D(v.commRate.trim());
                    double a = rnd(fin(tot(l -> l.ItemAmount)) * rate / 100.0, c.amtRound);
                    v.commAmount = num(a);
                } else if ("Weight".equals(t)) {
                    double rate = D(v.commRate.trim());
                    double w = fin(tot(l -> l.NetBillWeight));
                    double uom = D(v.commUom.trim());
                    v.commAmount = num(rnd(w / uom * rate, c.amtRound));
                }
            } else v.commAmount = "0";
        });
    }

    /** ExpProportion :1838 */
    public void expProportion() {
        tryRun(() -> {
            double total = fin(totExp());
            double nw = fin(tot(l -> l.NetBillWeight));
            if (total > 0.0) for (Line l : v.lines) l.Expense = fin(total / nw * l.NetBillWeight);
            else for (Line l : v.lines) l.Expense = 0;
            billProportion();
        });
    }

    /** FreightProportion :1871 */
    public void freightProportion() {
        tryRun(() -> {
            double nw = fin(tot(l -> l.NetBillWeight));
            double credit = 0; for (Fr f : v.freight) credit += f.Freight;
            if (credit > 0.0) for (Line l : v.lines) l.Freights = fin(Math.rint(credit) / nw * l.NetBillWeight);
            else for (Line l : v.lines) l.Freights = 0;
            billProportion();
        });
    }

    /** LedgerProportion :1904 */
    public void ledgerProportion() {
        tryRun(() -> {
            double total = D(v.commAmount);
            double nw = fin(tot(l -> l.NetBillWeight));
            double cr = 0, db = 0;
            for (Jv j : v.gl) { cr += j.Credit; db += j.Debit; }
            cr += total;
            if (db > cr) {
                double diff = db - cr;
                for (Line l : v.lines) l.Journal = fin(diff / nw * l.NetBillWeight);
            } else {
                if (db == cr) for (Line l : v.lines) l.Journal = cr - db;
                if (db < cr) for (Line l : v.lines) l.Journal = 0;
            }
            billProportion();
        });
    }

    /** BillProportion :1963 (bool.Parse of the configuration: a bad text is the FormatException of the desktop). */
    public void billProportion() {
        tryRun(() -> {
            for (Line l : v.lines) {
                boolean full;
                if (c.creditRaw == null) full = true;
                else {
                    String t = c.creditRaw.trim();
                    if (t.equalsIgnoreCase("true")) full = true;
                    else if (t.equalsIgnoreCase("false")) full = false;
                    else throw new IllegalArgumentException("String was not recognized as a valid Boolean.");
                }
                l.BillAmount = full ? fin(l.ItemAmount) + fin(l.Expense) - fin(l.Commission) : fin(l.ItemAmount);
            }
        });
    }

    /** CommissionProportion :1996 */
    public void commissionProportion() {
        tryRun(() -> {
            double total = D(v.commAmount);
            double pct = D(v.commRate);
            double nw = fin(tot(l -> l.NetBillWeight));
            if (total > 0.0) {
                for (Line l : v.lines) {
                    if ("Percent".equals(v.commType)) l.Commission = fin(l.ItemAmount * pct / 100.0);
                    else l.Commission = fin(total / nw * l.NetBillWeight);
                }
            } else for (Line l : v.lines) l.Commission = 0;
            billProportion();
        });
    }

    /** CalculateTotalInformation :3361 (its exception is re-thrown to the caller's catch). */
    public void calculateTotalInformation() {
        tryRun(() -> {
            if (!v.lines.isEmpty()) {
                BigDecimal tq = dec(tot(l -> l.ItemQty));
                BigDecimal tw = dec(tot(l -> l.NetBillWeight));
                BigDecimal tf = dec(tot(l -> l.FcyAmount));
                v.totalQty = hash(tq, 2);
                v.totalWeight = hash(tw, 3);
                v.fcyAmount = fcy(rndDec(tf, c.fcyRound));
            } else { v.totalQty = "0"; v.totalWeight = "0"; v.fcyAmount = "0"; }
        });
    }

    /** txtExchangeRate_TextChanged :3326 */
    public void exchangeChanged() {
        tryRun(() -> {
            BigDecimal ex = decText(v.exchangeRate);
            if (!v.lines.isEmpty() && ex.signum() > 0) {
                for (Line l : v.lines) {
                    l.ExchangeRate = D(v.exchangeRate);
                    BigDecimal a = dec(l.ItemAmount).divide(ex, new MathContext(28, RoundingMode.HALF_EVEN));
                    l.FcyAmount = rndDec(a, c.fcyRound).doubleValue();
                }
            } else for (Line l : v.lines) { l.ExchangeRate = 0; l.FcyAmount = 0; }
            calculateTotalInformation();
        });
    }

    /** txtDueDays_TextChanged :3147 (DateTime.Now.AddDays); returns the date text yyyy-MM-dd. */
    public static String dueDate(String days) {
        java.time.LocalDate d = java.time.LocalDate.now();
        if (days != null && !days.trim().isEmpty()) {
            double n = toDouble(days.trim());
            return d.plusDays((long) n).toString();
        }
        return d.toString();
    }

    /** grd_CellUpdated :930 */
    public void grdCellUpdated(int row, String col) {
        tryRun(() -> {
            if (row >= 0 && row < v.lines.size() && ("RateCut".equals(col) || "ItemRate".equals(col) || "EquivalentSoRate".equals(col))) {
                Line l = v.lines.get(row);
                double eq = l.EquivalentSoRate;
                double rcAmt = l.NetBillWeight / eq * l.RateCut;
                l.RateCutAmount = rcAmt;
                double ia = l.NetBillWeight / eq * l.ItemRate;
                l.ItemAmount = rnd(ia - rcAmt, c.amtRound);
                BigDecimal ex = decText(v.exchangeRate);
                if (ex.signum() > 0) l.FcyAmount = rndDec(dec(l.ItemAmount).divide(ex, new MathContext(28, RoundingMode.HALF_EVEN)), c.fcyRound).doubleValue();
            }
            billAmount();
            calculateTotalInformation();
        });
    }

    // ------------------------------------------------------------------ sub grids

    public void addFreightRow() { v.freight.add(new Fr()); }
    public void addExpRow() { v.exp.add(new Ex()); }
    public void addGlRow() { v.gl.add(new Jv()); }

    /** grdFreight_ColumnButtonClick :723 */
    public void freightButton(String key, int row) {
        tryRun(() -> {
            if ("Delete".equals(key) && row >= 0 && row < v.freight.size()) { v.freight.remove(row); if (v.freight.isEmpty()) addFreightRow(); }
            if ("Add".equals(key)) addFreightRow();
            freightProportion();
            billAmount();
        });
    }

    /** grdFreight_CellUpdated :755 */
    public void freightCell() {
        tryRun(() -> { freightProportion(); billAmount(); });
    }

    /** grdInvExp_ColumnButtonClick :770 */
    public void expButton(String key, int row) {
        tryRun(() -> {
            if ("Delete".equals(key) && row >= 0 && row < v.exp.size()) { v.exp.remove(row); if (v.exp.isEmpty()) addExpRow(); }
            if ("Add".equals(key)) addExpRow();
            expProportion();
            billAmount();
        });
    }

    /** grdInvExp_CellUpdated :802 (Qty / Rate: Amount = Qty * Rate when both cells have text; Amount: Qty and Rate are zeroed). */
    public void expCell(int row, String col) {
        tryRun(() -> {
            if (row >= 0 && row < v.exp.size()) {
                Ex e = v.exp.get(row);
                if ("Qty".equals(col) || "Rate".equals(col)) e.Amount = e.Qty * e.Rate;
                else if ("Amount".equals(col)) { e.Qty = 0; e.Rate = 0; }
            }
            billAmount();
            expProportion();
        });
    }

    /** grdGLedger_ColumnButtonClick :835 (BillAmount runs after the try block in every case). */
    public void glButton(String key, int row) {
        tryRun(() -> {
            if ("Delete".equals(key) && row >= 0 && row < v.gl.size()) { v.gl.remove(row); if (v.gl.isEmpty()) addGlRow(); }
            if ("Add".equals(key)) addGlRow();
        });
        billAmount();
    }

    /**
     * grdGLedger_CellUpdated :866. cellSupplied is false when the edit was refused by the page. The Credit / Debit refusals and the customer
     * account refusal are MessageBoxes of the desktop: they are returned in messages and the cell is reset exactly as the desktop does.
     */
    public void glCell(int row, String col) {
        tryRun(() -> {
            if (row < 0 || row >= v.gl.size()) return;
            Jv j = v.gl.get(row);
            if (("Qty".equals(col) || "Rate".equals(col))) {
                j.Credit = j.Qty * j.Rate; j.Debit = 0; j.Percentage = 0;
            }
            if ("Percentage".equals(col)) {
                double t = fin(tot(l -> l.ItemAmount)) / 100.0 * j.Percentage;
                if (t > 0.0) { j.Credit = Math.rint(t); j.Debit = 0; }
                else { j.Debit = Math.abs(Math.rint(t)); j.Credit = 0; }
                j.Qty = 0; j.Rate = 0;
            }
            if ("Credit".equals(col) && j.Debit > 0.0) { j.Credit = 0; v.messages.add("Debit Side is aleady added"); }
            if ("Debit".equals(col) && j.Credit > 0.0) { j.Debit = 0; v.messages.add("Credit Side is aleady added"); }
            if ("AccountId".equals(col) && v.supplierGlId == j.AccountId) {
                v.messages.add("Customer Account Not select");
                j.AccountId = 0;
            } else {
                billAmount();
                ledgerProportion();
            }
        });
    }

    // ------------------------------------------------------------------ event sequences

    /** Commission fields changed (cmbcommtype_Leave :1533, txtcommrate_TextChanged :1547, cmbcommuom_Leave :1554, cmbcommagent_Leave :3222, cmbcommtype_TextChanged :3236). */
    public void commissionChanged() { tryRun(() -> { totalCommissionAmount(); billAmount(); commissionProportion(); }); }

    /** LoadInGridDetail :1794 - the tail after the grids were filled. */
    public void afterLoad() {
        tryRun(() -> {
            totalCommissionAmount();
            expProportion();
            freightProportion();
            commissionProportion();
            ledgerProportion();
            billProportion();
            billAmount();
        });
    }

    /** ReadById :2399 - the tail after the grids were filled. */
    public void afterRead() {
        tryRun(() -> {
            totalCommissionAmount();
            billAmount();
            expProportion();
            ledgerProportion();
            freightProportion();
            commissionProportion();
            billProportion();
        });
    }

    /** Insert :2514 - the recalculation before the checks. */
    public void beforeSave() {
        totalCommissionAmount();
        billAmount();
        freightProportion();
        expProportion();
        commissionProportion();
    }

    public static int ti(String s) { return toInt(s); }
}
