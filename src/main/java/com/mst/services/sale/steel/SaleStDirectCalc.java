package com.mst.services.sale.steel;

import com.mst.services.sale.steel.SaleStInvoiceCalc.Cfg;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.List;
import java.util.function.ToDoubleFunction;

import static com.mst.services.sale.engr.SaleEngrSupport.toDouble;
import static com.mst.services.sale.engr.SaleEngrSupport.toInt;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.dec;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.decText;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.fmt;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.hash;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.num;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.rnd;
import static com.mst.services.sale.steel.SaleStInvoiceCalc.rndDec;

/**
 * Screen 544 "SaleInvoiceDirect_St" = Architecture.WinApp.Steel.Sale.SaleInvoiceDirect_St (Steel Sale Invoice Direct, document type 1510).
 *
 * The server-side copy of the text box / grid event handlers of SaleInvoiceDirect_St.cs working on a state object (the text of the text boxes, the entry
 * panel, the four grids): Total :2439, TotalWeight :2501, AmountCaluculation :2531, txtQty/txtwtcut/txtAddLss/txtRate/txtratecut handlers :2591-:2664,
 * BillAmount :1869, ExpProportion :2192, BillProportion :2214, CommissionProportion :2243, FreightProportion :2275, TotalCommissionAmount :2876,
 * CalculateTotalInformation :3850, txtExchangeRate_TextChanged :3803, grdFreight_* :3893/:3925, grdInvExp_* :2080/:2112, grdGLedger_* :2049/:2137,
 * grd_ColumnButtonClick :3975, grd_DoubleClick :3113, btnAdd_Click :2308, btnUpdateDetail_Click :3167, Reset :1768, ResetDetail :1840, LoadInGridDetail :4094 (tail).
 * Every desktop method has its own try/catch that shows ex.Message: a failure is added to Inv.messages and the rest of that method is skipped.
 * A text box assignment raises the TextChanged handler of the desktop only when the text really changes (set()).
 */
public class SaleStDirectCalc {

    /** dtGrid (40 columns, the field names are the column names). */
    public static class Line {
        public int Id, OrderId, OrderDetailId, OrderNo, ItemId, JobLotId, PackingTypeId, PackUomId, RateUomId, WarehouseId, CityId;
        public String ItemName = "", JobLot = "", PackingType = "", PackUom = "", RateUom = "", Warehouse = "", CityName = "", GpDate = "", GpNo = "0", VehicleNo = "", Remarks = "";
        public double ItemQty, GrossWeight, WeightCut, WeightCutTotal, AddLessWeight, NetBillWeight, StockWeight, ItemRate, RateCut, RateCutTotal, ItemAmount,
                ExchangeRate, FcyAmount, BillAmount, Expense, Journal, Commission, Freight;
    }

    /** dtFreight: TransporterId, Percentage, Qty, Rate, Freight (caption Credit), Remarks. */
    public static class Fr { public int TransporterId; public double Percentage, Qty, Rate, Freight; public String Remarks = ""; }

    /** dtInvExp: ItemId, Qty, Rate, Amount, Remarks. */
    public static class Ex { public int ItemId; public double Qty, Rate, Amount; public String Remarks = ""; }

    /** dtGrdGL: AccountId, Remarks, Percentage, Qty, Rate, Debit, Credit. */
    public static class Jv { public int AccountId; public String Remarks = ""; public double Percentage, Qty, Rate, Debit, Credit; }

    /** The entry panel (groupBox1): combos as id + text, text boxes as text. */
    public static class E {
        public int itemId, jobLotId, packingTypeId, packUomId, rateUomId, warehouseId, cityId;
        public String itemText = "", jobLotText = "", packingText = "", packUomText = "", rateUomText = "", warehouseText = "", cityText = "";
        public String itemQty = "", gross = "", wtCut = "", wtCutTotal = "", addLess = "", netBill = "", stock = "", rate = "", rateCut = "", rateCutTotal = "", amount = "",
                gpDate = "", gpNo = "", vehicleNo = "", remarks = "";
    }

    /** The form. */
    public static class Inv {
        public int id, branchId, projectId, supplierId, supplierGlId, paymentTermId, currencyId, commAgentId, transporterId, updateIndex = -1;
        public boolean saveMode = true, canSave = true;
        public String docNo = "", docDate = "", refNo = "", billNo = "", deliveryTerm = "", deliveryStart = "", expiryDate = "", deliveryDays = "", dueDays = "", dueDate = "",
                exchangeRate = "", totalQty = "", totalWeight = "", fcyAmount = "", billAmount = "", commType = "", commRate = "", commUom = "", commAmount = "", commRemarks = "",
                remarks = "", freightText = "", focus = "";
        public E e = new E();
        public List<Line> lines = new ArrayList<>();
        public List<Line> removed = new ArrayList<>();
        public List<Fr> freight = new ArrayList<>();
        public List<Ex> exp = new ArrayList<>();
        public List<Jv> gl = new ArrayList<>();
        public List<String> messages = new ArrayList<>();
    }

    /** CommonServices.GetUomScheduleByItemId: the Equivalent (Cells[2]) of a UOM of the item, null when the combo has no such row. */
    public interface Uoms { Double equivalent(int itemId, int uomId); }

    private final Cfg c;
    private final Inv v;
    private final Uoms uoms;

    public SaleStDirectCalc(Cfg c, Inv v, Uoms uoms) {
        this.c = c; this.v = v; this.uoms = uoms;
        if (v.lines == null) v.lines = new ArrayList<>();
        if (v.removed == null) v.removed = new ArrayList<>();
        if (v.freight == null) v.freight = new ArrayList<>();
        if (v.exp == null) v.exp = new ArrayList<>();
        if (v.gl == null) v.gl = new ArrayList<>();
        if (v.e == null) v.e = new E();
        v.messages = new ArrayList<>();
        v.focus = "";
    }

    // ------------------------------------------------------------------ helpers

    private static double D(String s) { return toDouble(s); }
    private String amt(double x) { return fmt(x, c.amtDec); }
    private double tot(ToDoubleFunction<Line> f) { double s = 0; for (Line l : v.lines) s += f.applyAsDouble(l); return s; }
    private double totExp() { double s = 0; for (Ex e : v.exp) s += e.Amount; return s; }
    private static double fin(double x) { return Double.isNaN(x) || Double.isInfinite(x) ? 0.0 : x; }

    private void tryRun(Runnable r) {
        try { r.run(); } catch (RuntimeException ex) { v.messages.add(ex.getMessage() == null ? ex.toString() : ex.getMessage()); }
    }

    /** double.Parse(text): culture-invariant, thousands separators allowed, anything else is the FormatException of the desktop. */
    static double pd(String s) {
        String t = s == null ? "" : s.trim().replace(",", "");
        if (!t.matches("[+-]?(\\d+\\.?\\d*|\\.\\d+)([eE][+-]?\\d+)?")) throw new IllegalArgumentException("Input string was not in a correct format.");
        return Double.parseDouble(t);
    }

    private static BigDecimal bd(double x) { return new BigDecimal(num(x)); }

    // ------------------------------------------------------------------ entry panel text boxes (TextChanged only when the text changes)

    private String get(String f) {
        E e = v.e;
        switch (f) {
            case "itemQty": return e.itemQty; case "gross": return e.gross; case "wtCut": return e.wtCut; case "wtCutTotal": return e.wtCutTotal;
            case "addLess": return e.addLess; case "netBill": return e.netBill; case "stock": return e.stock; case "rate": return e.rate;
            case "rateCut": return e.rateCut; case "rateCutTotal": return e.rateCutTotal; case "amount": return e.amount; case "remarks": return e.remarks;
            default: throw new IllegalArgumentException(f);
        }
    }

    private void put(String f, String val) {
        E e = v.e;
        switch (f) {
            case "itemQty": e.itemQty = val; break; case "gross": e.gross = val; break; case "wtCut": e.wtCut = val; break; case "wtCutTotal": e.wtCutTotal = val; break;
            case "addLess": e.addLess = val; break; case "netBill": e.netBill = val; break; case "stock": e.stock = val; break; case "rate": e.rate = val; break;
            case "rateCut": e.rateCut = val; break; case "rateCutTotal": e.rateCutTotal = val; break; case "amount": e.amount = val; break; case "remarks": e.remarks = val; break;
            default: throw new IllegalArgumentException(f);
        }
    }

    /** TextBox.Text = value: the TextChanged handler of the desktop runs when the text changed. */
    private void set(String f, String val) {
        String nv = val == null ? "" : val;
        if (get(f).equals(nv)) return;
        put(f, nv);
        entryChanged(f);
    }

    private void entryChanged(String f) {
        switch (f) {
            case "itemQty": hQty(); break;                                 // txtQty_TextChanged :2591
            case "gross": total(); break;                                  // txtGrossWeight_TextChanged :2607
            case "wtCut": case "addLess": case "remarks": hAddLess(); break;   // txtwtcut_TextChanged :2620, txtAddLss_TextChanged :2628 (also txtRemarksDetail)
            case "rate": case "rateCut": hRate(); break;                   // txtRate_TextChanged :2650, txtratecut_TextChanged :2664
            default: break;
        }
    }

    private void hQty() { totalWeight(); total(); amountCalculation(); totalCommissionAmount(); billAmount(); }
    private void hAddLess() { total(); amountCalculation(); totalCommissionAmount(); billAmount(); }
    private void hRate() { amountCalculation(); totalCommissionAmount(); billAmount(); }

    /** An event raised by the browser: the entry panel already holds the new text. */
    public void entryEvent(String name) {
        switch (name) {
            case "qty": hQty(); break;
            case "gross": total(); break;
            case "wtcut": case "addless": case "remarks": hAddLess(); break;
            case "rate": case "ratecut": case "rateuom": hRate(); break;      // comRateUOM_Leave :2636
            case "packuom": tryRun(() -> { totalWeight(); total(); amountCalculation(); totalCommissionAmount(); billAmount(); }); break;   // comPackUOM_Leave :2574
            default: break;
        }
    }

    private double equivalent(int itemId, int uomId) {
        Double q = uomId == 0 ? null : uoms.equivalent(itemId, uomId);
        if (q == null) throw new IllegalStateException("Object reference not set to an instance of an object.");   // SelectedRow == null
        return q;
    }

    /** TotalWeight :2501 */
    private void totalWeight() {
        tryRun(() -> {
            if (v.saveMode && v.canSave) {
                E e = v.e;
                double pack = 0, qty = 0;
                if (!e.packUomText.isEmpty()) pack = equivalent(e.itemId, e.packUomId);
                if (!e.itemQty.isEmpty()) qty = pd(e.itemQty);
                if (!e.gross.isEmpty()) pd(e.gross);
                set("gross", num(pack * qty));
            }
        });
    }

    /** Total :2439 */
    private void total() {
        tryRun(() -> {
            E e = v.e;
            double qty = 0, gross = 0, wcTotal = 0;
            String addLess = "0";
            if (!e.packUomText.isEmpty()) equivalent(e.itemId, e.packUomId);
            if (!e.itemQty.isEmpty()) qty = pd(e.itemQty);
            if (!e.gross.isEmpty()) gross = pd(e.gross);
            if (!e.netBill.isEmpty()) pd(e.netBill);
            if (!e.stock.isEmpty()) pd(e.stock);
            if (!e.wtCut.isEmpty()) { wcTotal = qty * pd(e.wtCut); e.wtCutTotal = num(wcTotal); } else e.wtCutTotal = "0";
            if (!e.addLess.isEmpty()) addLess = e.addLess.trim();
            double w = "-".equals(addLess) ? gross - wcTotal : gross - wcTotal + pd(addLess);
            e.netBill = num(rnd(w, 2));
            e.stock = num(rnd(w, 2));
        });
    }

    /** AmountCaluculation :2531 */
    private void amountCalculation() {
        tryRun(() -> {
            E e = v.e;
            double rateUom = e.rateUomText.isEmpty() ? 0.0 : equivalent(e.itemId, e.rateUomId);
            double rate = 0, rateCut = 0, net = 0;
            if (!e.rate.trim().isEmpty()) rate = pd(e.rate);
            if (!e.rateCut.trim().isEmpty()) rateCut = pd(e.rateCut);
            if (!e.netBill.trim().isEmpty()) net = pd(e.netBill);
            if (net > 0.0 && rateUom > 0.0 && rate > 0.0) {
                double item = net / rateUom * rate;
                double cut = net / rateUom * rateCut;
                e.rateCutTotal = num(cut);
                item -= cut;
                e.amount = amt(item);
            } else {
                e.amount = "0";
                v.billAmount = "0";
            }
        });
    }

    // ------------------------------------------------------------------ the form methods

    /** BillAmount :1869 */
    public void billAmount() {
        tryRun(() -> {
            double item = rnd(fin(tot(l -> l.ItemAmount)), c.amtRound);
            double ex = fin(totExp());
            double jd = 0, jc = 0;
            for (Jv r : v.gl) if (r.AccountId > 0) { jd += r.Debit; jc += r.Credit; }
            double bill = item + ex - D(v.freightText.trim());
            bill = bill + jc - jd;
            if (v.supplierId == v.commAgentId) bill -= D(v.commAmount.trim());
            v.billAmount = amt(bill);
            billProportion();
        });
    }

    /** ExpProportion :2192 */
    public void expProportion() {
        tryRun(() -> {
            BigDecimal total = bd(fin(totExp()));
            BigDecimal nw = bd(fin(tot(l -> l.NetBillWeight)));
            for (Line l : v.lines) {
                BigDecimal w = bd(l.NetBillWeight);
                if (nw.signum() == 0) throw new IllegalStateException("Attempted to divide by zero.");
                BigDecimal x = total.divide(nw, new MathContext(28, RoundingMode.HALF_EVEN)).multiply(w);
                l.Expense = rndDec(x, c.amtRound).doubleValue();
            }
            billProportion();
        });
    }

    /** BillProportion :2214 (bool.Parse of the configuration: a bad text is the FormatException of the desktop). */
    public void billProportion() {
        tryRun(() -> {
            for (Line l : v.lines) {
                if (c.creditRaw != null) {
                    String t = c.creditRaw.trim();
                    boolean b;
                    if (t.equalsIgnoreCase("true")) b = true;
                    else if (t.equalsIgnoreCase("false")) b = false;
                    else throw new IllegalArgumentException("String was not recognized as a valid Boolean.");
                    if (!b) l.BillAmount = l.ItemAmount - l.Commission;
                } else l.BillAmount = l.ItemAmount + l.Expense - l.Commission;
            }
        });
    }

    /** CommissionProportion :2243 */
    public void commissionProportion() {
        tryRun(() -> {
            double total = D(v.commAmount);
            double pct = D(v.commRate);
            double nw = fin(tot(l -> l.NetBillWeight));
            for (Line l : v.lines) {
                if ("Percent".equals(v.commType)) l.Commission = fin(l.ItemAmount * pct / 100.0);
                else l.Commission = fin(total / nw * l.NetBillWeight);
            }
            billProportion();
        });
    }

    /** FreightProportion :2275 */
    public void freightProportion() {
        tryRun(() -> {
            if (!v.lines.isEmpty()) {
                double nw = fin(tot(l -> l.NetBillWeight));
                double credit = 0; for (Fr f : v.freight) credit += f.Freight;
                for (Line l : v.lines) l.Freight = fin(credit / nw * l.NetBillWeight);
                billProportion();
            } else for (Line l : v.lines) l.Freight = 0;
        });
    }

    /** TotalCommissionAmount :2876 */
    public void totalCommissionAmount() {
        tryRun(() -> {
            String t = v.commType;
            if (!v.commRate.isEmpty() && "Flat".equals(t)) v.commAmount = amt(D(v.commRate.trim()));
            if (!v.commRate.isEmpty() && ("Percent".equals(t) || "Percentage".equals(t))) {
                double rate = D(v.commRate.trim());
                v.commAmount = amt(fin(tot(l -> l.ItemAmount)) * rate / 100.0);
            }
            if (!v.commRate.isEmpty() && "By Weight".equals(t)) {
                double rate = D(v.commRate.trim());
                double w = fin(tot(l -> l.NetBillWeight));
                double uom = D(v.commUom.trim());
                v.commAmount = fmt(w / uom * rate, c.amtDec);
            }
            commissionProportion();
        });
    }

    /** CalculateTotalInformation :3850 (its exception is re-thrown to the caller's catch). */
    public void calculateTotalInformation() {
        tryRun(() -> {
            if (!v.lines.isEmpty()) {
                BigDecimal tq = dec(tot(l -> l.ItemQty));
                BigDecimal tw = dec(tot(l -> l.NetBillWeight));
                BigDecimal tf = dec(tot(l -> l.FcyAmount));
                v.totalQty = hash(tq, 2);
                v.totalWeight = hash(tw, 2 + 1);
                v.fcyAmount = fmt(rndDec(tf, c.fcyRound), c.fcyDec);
            } else { v.totalQty = "0"; v.totalWeight = "0"; v.fcyAmount = "0"; }
        });
    }

    /** txtExchangeRate_TextChanged :3803 */
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

    /** txtFreightAmount_TextChanged :3564 */
    private void freightTextChanged() {
        tryRun(() -> { if (!v.lines.isEmpty()) { freightProportion(); billAmount(); } });
    }

    /** combdeliverytrm_TextChanged :3546 (the transporter combo is hidden, its value stays 0). */
    public void deliveryTermChanged() {
        tryRun(() -> {
            v.transporterId = 0;                                           // CmbTransporterAc.Value = OutwardFreightAccountId (never set = 0) / Text = ""
            if ("Ponch".equals(v.deliveryTerm)) return;
            String old = v.freightText;
            v.freightText = "0";
            if (!"0".equals(old)) freightTextChanged();
        });
    }

    /** CalculateExpiryDate :3784 */
    private void calculateExpiryDate() {
        if (v.deliveryStart == null || v.deliveryStart.isEmpty()) return;
        LocalDate s = LocalDate.parse(v.deliveryStart.substring(0, 10));
        v.expiryDate = (toInt(v.deliveryDays.trim()) == 0 ? s : s.plusDays((long) D(v.deliveryDays.trim()))).toString();
    }

    /** txtDeliveryDays_TextChanged :3772 */
    public void deliveryDaysChanged() { tryRun(this::calculateExpiryDate); }

    /** DeliveryStartDate_ValueChanged :3876 */
    public void deliveryStartChanged() {
        tryRun(() -> {
            if (v.deliveryStart != null && v.docDate != null && !v.docDate.isEmpty() && LocalDate.parse(v.deliveryStart.substring(0, 10)).isBefore(LocalDate.parse(v.docDate.substring(0, 10)))) {
                v.deliveryStart = v.docDate.substring(0, 10);
                deliveryStartChanged();                                    // the assignment raises ValueChanged again
                throw new IllegalStateException("Delivery Start Date Can't Be Less Than Doc Date");
            }
            calculateExpiryDate();
        });
    }

    /** txtduedays_TextChanged :3330 */
    public void dueDaysChanged() { tryRun(() -> v.dueDate = SaleStInvoiceCalc.dueDate(v.dueDays)); }

    /** cmbsuppliername_ValueChanged / combcommAgent_Leave / combCommType_Leave / combcommUOM_Leave / txtcommrate_TextChanged. */
    public void commissionChanged() { tryRun(() -> { totalCommissionAmount(); billAmount(); }); }

    public void supplierChanged() { tryRun(this::billAmount); }

    // ------------------------------------------------------------------ sub grids

    public void addFreightRow() { v.freight.add(new Fr()); }
    public void addExpRow() { v.exp.add(new Ex()); }
    public void addGlRow() { v.gl.add(new Jv()); }

    /** grdFreight_ColumnButtonClick :3893 */
    public void freightButton(String key, int row) {
        tryRun(() -> {
            if ("Delete".equals(key) && row >= 0 && row < v.freight.size()) { v.freight.remove(row); }
            if ("Delete".equals(key) && v.freight.isEmpty()) addFreightRow();
            if ("Add".equals(key)) addFreightRow();
            billAmount();
            freightProportion();
        });
    }

    /** grdFreight_CellUpdated :3925 */
    public void freightCell(int row, String col) {
        tryRun(() -> {
            if (row >= 0 && row < v.freight.size()) {
                Fr item = v.freight.get(row);
                if ("Qty".equals(col) || "Rate".equals(col)) { item.Freight = item.Qty * item.Rate; item.Percentage = 0; }
                if ("Percentage".equals(col)) {
                    if (Math.abs(toInt(num(item.Percentage))) > 100) { item.Percentage = 0; throw new IllegalArgumentException("Percentage mustbe less than 100"); }
                    double t = fin(tot(l -> l.ItemAmount)) / 100.0 * item.Percentage;
                    if (t > 0.0) item.Freight = rnd(t, c.amtRound);
                    item.Qty = 0; item.Rate = 0;
                }
                if ("Freight".equals(col)) { item.Qty = 0; item.Rate = 0; item.Percentage = 0; }
            }
            billAmount();
            freightProportion();
        });
    }

    /** grdInvExp_ColumnButtonClick :2080 */
    public void expButton(String key, int row) {
        tryRun(() -> {
            if ("Delete".equals(key) && row >= 0 && row < v.exp.size()) v.exp.remove(row);
            if ("Delete".equals(key) && v.exp.isEmpty()) addExpRow();
            if ("Add".equals(key)) addExpRow();
            expProportion();
            billAmount();
        });
    }

    /** grdInvExp_CellUpdated :2112 */
    public void expCell(int row, String col) {
        tryRun(() -> {
            if (("Qty".equals(col) || "Rate".equals(col)) && row >= 0 && row < v.exp.size()) { Ex e = v.exp.get(row); e.Amount = e.Qty * e.Rate; }
            expProportion();
            freightProportion();
            billAmount();
        });
    }

    /** grdGLedger_ColumnButtonClick :2049 (BillAmount runs after the try block in every case). */
    public void glButton(String key, int row) {
        tryRun(() -> {
            if ("Delete".equals(key) && row >= 0 && row < v.gl.size()) v.gl.remove(row);
            if ("Delete".equals(key) && v.gl.isEmpty()) addGlRow();
            if ("Add".equals(key)) addGlRow();
        });
        billAmount();
    }

    /** grdGLedger_CellUpdated :2137 */
    public void glCell(int row, String col) {
        tryRun(() -> {
            if (row >= 0 && row < v.gl.size()) {
                Jv j = v.gl.get(row);
                if ("Qty".equals(col) || "Rate".equals(col)) { j.Credit = j.Qty * j.Rate; j.Debit = 0; j.Percentage = 0; }
                if ("Percentage".equals(col)) {
                    double t = fin(tot(l -> l.ItemAmount)) / 100.0 * j.Percentage;
                    if (t > 0.0) { j.Credit = Math.rint(t); j.Debit = 0; } else { j.Debit = Math.abs(Math.rint(t)); j.Credit = 0; }
                    j.Qty = 0; j.Rate = 0;
                }
                if ("Credit".equals(col) && j.Debit > 0.0) { j.Credit = 0; v.messages.add("Debit Side is aleady added"); }
                if ("Debit".equals(col) && j.Credit > 0.0) { j.Debit = 0; v.messages.add("Credit Side is aleady added"); }
            }
            billAmount();
        });
    }

    // ------------------------------------------------------------------ detail grid and the entry buttons

    private void afterGridChange() {
        totalCommissionAmount();
        commissionProportion();
        expProportion();
        freightProportion();
        billAmount();
        calculateTotalInformation();
    }

    /** grd_ColumnButtonClick :3975 (the confirmation is asked by the browser) */
    public void gridButton(String key, int row) {
        if ("Delete".equals(key)) {
            if (row >= 0 && row < v.lines.size()) {
                Line item = v.lines.get(row);
                if (v.saveMode && v.canSave) v.lines.remove(row);
                else if (item.Id > 0) { v.removed.add(item); v.lines.remove(row); }
                else v.lines.remove(row);
            }
        } else if ("Add".equals(key)) {
            if (row >= 0 && row < v.lines.size()) {
                Line src = v.lines.get(row), n = copy(src);
                v.lines.add(n);
                if (!v.saveMode) n.Id = 0;                                 // btnUpdate.Visible && btnUpdate.Enabled
            }
        }
        afterGridChange();
    }

    private static Line copy(Line a) {
        Line b = new Line();
        b.Id = a.Id; b.OrderId = a.OrderId; b.OrderDetailId = a.OrderDetailId; b.OrderNo = a.OrderNo; b.ItemId = a.ItemId; b.ItemName = a.ItemName; b.JobLotId = a.JobLotId; b.JobLot = a.JobLot;
        b.PackingTypeId = a.PackingTypeId; b.PackingType = a.PackingType; b.PackUomId = a.PackUomId; b.PackUom = a.PackUom; b.RateUomId = a.RateUomId; b.RateUom = a.RateUom;
        b.WarehouseId = a.WarehouseId; b.Warehouse = a.Warehouse; b.CityId = a.CityId; b.CityName = a.CityName; b.GpDate = a.GpDate; b.GpNo = a.GpNo; b.VehicleNo = a.VehicleNo; b.Remarks = a.Remarks;
        b.ItemQty = a.ItemQty; b.GrossWeight = a.GrossWeight; b.WeightCut = a.WeightCut; b.WeightCutTotal = a.WeightCutTotal; b.AddLessWeight = a.AddLessWeight; b.NetBillWeight = a.NetBillWeight;
        b.StockWeight = a.StockWeight; b.ItemRate = a.ItemRate; b.RateCut = a.RateCut; b.RateCutTotal = a.RateCutTotal; b.ItemAmount = a.ItemAmount; b.ExchangeRate = a.ExchangeRate;
        b.FcyAmount = a.FcyAmount; b.BillAmount = a.BillAmount; b.Expense = a.Expense; b.Journal = a.Journal; b.Commission = a.Commission; b.Freight = a.Freight;
        return b;
    }

    /** FormValidationDetila :560 - false (and the message) when a field is missing. */
    private boolean validateDetail() {
        E e = v.e;
        if (e.itemId == 0) return fail("Item Name Field is Required", "CmbItem");
        if (e.jobLotId == 0) return fail("Job/Lot Field is Required", "CmbJobLot");
        if (e.packingTypeId == 0) return fail("Packing Type Field is Required", "CmbPackingType");
        if (e.packUomId == 0) return fail("Pack Uom Field is Required", "CmbPackUom");
        if (blank0(e.itemQty)) return fail("Qty Field is Required", "txtItemQty");
        if (blank0(e.gross)) return fail("Gross Weight Field is Required", "txtGrossWeight");
        if (blank0(e.netBill)) return fail("Net Bill Weight Field is Required", "txtNetBillWeight");
        if (blank0(e.stock)) return fail("Stock Weight Field is Required", "txtStockWeight");
        if (blank0(e.rate)) return fail("ItemRate Field is Required", "txtRate");
        if (e.rateUomId == 0) return fail("Rate UOM Field is Required", "CmbRateUom");
        if (e.warehouseId == 0) return fail("Warehouse Field is Required", "CmbWarehouse");
        if (e.cityId == 0) return fail("CityName Field is Required", "CmbCity");
        return true;
    }

    private static boolean blank0(String s) { String t = s == null ? "" : s.trim(); return t.isEmpty() || t.equals("0"); }

    private boolean fail(String m, String focus) { v.messages.add(m); v.focus = focus; return false; }

    /** The row of dtGrid that the entry panel makes (btnAdd_Click :2308 / btnUpdateDetail_Click :3167). */
    private void fillLine(Line r, boolean forUpdate) {
        E e = v.e;
        r.ItemId = e.itemId; r.ItemName = e.itemText; r.JobLotId = e.jobLotId; r.JobLot = e.jobLotText; r.PackingTypeId = e.packingTypeId; r.PackingType = e.packingText;
        r.PackUomId = e.packUomId; r.PackUom = e.packUomText;
        r.ItemQty = D(e.itemQty); r.GrossWeight = D(e.gross); r.WeightCut = D(e.wtCut); r.WeightCutTotal = D(e.wtCutTotal); r.AddLessWeight = D(e.addLess);
        r.NetBillWeight = D(e.netBill); r.StockWeight = D(e.stock); r.ItemRate = D(e.rate);
        r.RateUomId = e.rateUomId; r.RateUom = e.rateUomText; r.RateCut = D(e.rateCut); r.RateCutTotal = D(e.rateCutTotal); r.ItemAmount = D(e.amount);
        r.WarehouseId = e.warehouseId; r.Warehouse = e.warehouseText;
        r.GpDate = e.gpDate; r.GpNo = e.gpNo; r.VehicleNo = e.vehicleNo; r.Remarks = forUpdate ? e.remarks : e.remarks.trim();
        if (!forUpdate) {
            r.CityId = e.cityId; r.CityName = e.cityText;
            BigDecimal ex = decText(v.exchangeRate);
            if (ex.signum() > 0) {
                r.ExchangeRate = ex.doubleValue();
                r.FcyAmount = rndDec(decText(e.amount).divide(ex, new MathContext(28, RoundingMode.HALF_EVEN)), c.fcyRound).doubleValue();
            }
        }
    }

    /** btnAdd_Click :2308 */
    public void addDetail() {
        tryRun(() -> {
            if (!v.lines.isEmpty() && v.lines.get(0).OrderId > 0) {
                v.messages.add("You Can't add new detail record. Beacuse entry against Order Exist");
            } else if (validateDetail()) {
                Line r = new Line();
                fillLine(r, false);
                v.lines.add(r);
                totalCommissionAmount();
                commissionProportion();
                expProportion();
                freightProportion();
                billAmount();
                calculateTotalInformation();
                resetDetail();
            }
        });
    }

    /** btnUpdateDetail_Click :3167 (the detail grid row keeps its CityId / Exchange / Fcy: the desktop does not write them) */
    public void updateDetail() {
        tryRun(() -> {
            if (validateDetail()) {
                if (v.updateIndex < 0 || v.updateIndex >= v.lines.size()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
                fillLine(v.lines.get(v.updateIndex), true);
                v.updateIndex = -1;
                totalCommissionAmount();
                commissionProportion();
                expProportion();
                freightProportion();
                billAmount();
                calculateTotalInformation();
                resetDetail();
            }
        });
    }

    /** ResetDetail :1840 */
    public void resetDetail() {
        tryRun(() -> {
            E e = v.e;
            e.itemId = 0; e.itemText = ""; e.packUomId = 0; e.packUomText = "";
            set("itemQty", ""); set("gross", ""); set("wtCut", ""); set("wtCutTotal", ""); set("addLess", ""); set("netBill", ""); set("stock", ""); set("rate", "");
            e.rateUomId = 0; e.rateUomText = "";
            set("rateCut", ""); set("rateCutTotal", ""); set("amount", ""); set("remarks", "");
            v.focus = "CmbItem";
        });
    }

    /** grd_DoubleClick :3113 (the browser has already put the item and its UOM lists in place; the text boxes are assigned in the desktop's order). */
    public void editRow(int row) {
        tryRun(() -> {
            if (row < 0 || row >= v.lines.size()) return;
            Line r = v.lines.get(row);
            E e = v.e;
            v.updateIndex = row;
            e.itemId = r.ItemId; e.itemText = r.ItemName;
            e.jobLotId = r.JobLotId; e.jobLotText = r.JobLot; e.packingTypeId = r.PackingTypeId; e.packingText = r.PackingType;
            e.packUomId = r.PackUomId; e.packUomText = r.PackUom;
            set("itemQty", num(r.ItemQty)); set("gross", num(r.GrossWeight)); set("wtCut", num(r.WeightCut)); set("wtCutTotal", num(r.WeightCutTotal));
            set("addLess", num(r.AddLessWeight)); set("netBill", num(r.NetBillWeight)); set("stock", num(r.StockWeight));
            set("rate", fmt(r.ItemRate, c.rateDec));
            e.rateUomId = r.RateUomId; e.rateUomText = r.RateUom;
            set("rateCut", num(r.RateCut)); set("rateCutTotal", num(r.RateCutTotal)); set("amount", amt(r.ItemAmount));
            e.warehouseId = r.WarehouseId; e.warehouseText = r.Warehouse;
            e.gpDate = r.GpDate; e.gpNo = r.GpNo; e.vehicleNo = r.VehicleNo;
            set("remarks", r.Remarks);
            v.focus = r.OrderId > 0 ? "CmbJobLot" : "CmbItem";
        });
    }

    // ------------------------------------------------------------------ sequences

    /** LoadInGridDetail :4094 - the tail after the header text boxes and the grid were filled. */
    public void afterLoad() {
        tryRun(() -> {
            exchangeChanged();
            totalCommissionAmount();
            commissionProportion();
            expProportion();
            freightProportion();
            billAmount();
        });
    }

    /** ReadById :2337 - the tail after the grids were filled. */
    public void afterRead() {
        tryRun(() -> {
            deliveryStartChanged();
            deliveryDaysChanged();
            exchangeChanged();
            commissionProportion();
            expProportion();
            freightProportion();
            billProportion();
            billAmount();
            calculateTotalInformation();
        });
    }

    /**
     * Reset :1768 - the clearing of the text boxes in the desktop's order (the handlers raised on the way are run on the state as it is at that moment),
     * then the grids and defaultConfiquration (the base currency is set by the caller).
     */
    public void resetForm() {
        tryRun(() -> {
            v.supplierId = 0; billAmount();                                // CmbSuppCustomer.Text = "" -> ValueChanged
            v.refNo = ""; v.billNo = "";
            v.commAgentId = 0; v.commType = "";
            v.commRemarks = "";
            if (!v.commRate.isEmpty()) { v.commRate = ""; commissionChanged(); }
            v.commAmount = ""; v.commUom = ""; v.remarks = "";
            if (!v.deliveryDays.isEmpty()) { v.deliveryDays = ""; deliveryDaysChanged(); }
            v.transporterId = 0;
            if (!v.freightText.isEmpty()) { v.freightText = ""; freightTextChanged(); }
            if (!"0".equals(v.dueDays)) { v.dueDays = "0"; dueDaysChanged(); }
            E e = v.e;
            e.itemId = 0; e.itemText = ""; e.packUomId = 0; e.packUomText = "";
            set("itemQty", ""); set("gross", ""); set("wtCut", ""); set("wtCutTotal", ""); set("addLess", ""); set("netBill", ""); set("stock", ""); set("rate", "");
            e.rateUomId = 0; e.rateUomText = "";
            set("rateCut", ""); set("rateCutTotal", ""); set("amount", "");
            v.billAmount = "";
            v.totalQty = ""; v.totalWeight = "";
            if (!v.exchangeRate.isEmpty()) { v.exchangeRate = ""; exchangeChanged(); }
            v.fcyAmount = "";
            v.freight.clear(); v.gl.clear(); v.exp.clear(); v.lines.clear(); v.removed.clear();
            addFreightRow(); addGlRow(); addExpRow();
            v.id = 0; v.updateIndex = -1; v.saveMode = true;
        });
    }

    /** defaultConfiquration :825 - BaseCurrencyRate into txtExchangeRate (TextChanged when the text changes). */
    public void setExchangeRate(String text) {
        if (!v.exchangeRate.equals(text)) { v.exchangeRate = text; exchangeChanged(); }
    }
}
