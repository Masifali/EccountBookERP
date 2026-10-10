package com.mst.services.sale.salt;

import com.mst.services.sale.steel.SaleStInvoiceCalc.Cfg;

import java.math.BigDecimal;
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
 * Screen 585 "frmSaleDirectInvoice" = Architecture.WinApp.SaleForSalt.frmSaleDirectInvoice (Sale Salt, document type 1809).
 *
 * Server-side copy of the text box / grid event handlers of frmSaleDirectInvoice.cs working on a state object (the text of the text boxes, the entry
 * panel, the three grids). Desktop methods (frmSaleDirectInvoice.cs): BillAmount :3707, ExpProportion :3767, BillProportion :3788, CommissionProportion :3824,
 * FreightProportion :3856, Total :3889, TotalWeight :3926, DetailAmountCaluculation :3953, TotalCommissionAmount :3991, txtQty_TextChanged :4048 ..
 * comPackUOM_Leave :4120, txtExchangeRate_TextChanged :1278, cmbCurrency_Leave :1235, combdeliverytrm_TextChanged :4310, DueDateGenerate :4290,
 * EnableDiableCommissionFields :4215, btnAdd_Click :1685, btnUpdateDetail_Click :1911, grd_DoubleClick :1873, grd_ColumnButtonClick :1970,
 * grd_CellUpdated :2014, AddRowInDetailGridInCaseOfLoader :2194, grdInvExp_* :1403/:1422, grdGLedger_* :1590/:1620, grd_KeyDown :4806,
 * grdGLedger_KeyDown :4918, grdInvExp_KeyDown :4984, Reset :2483, ResetDetail :2561, OptionResetFields :2587, LoadInGridDetail :4544.
 * Every desktop method has its own try/catch that shows ex.Message: a failure is added to Inv.messages and the rest of that method is skipped.
 * Differences to the other ports: Math.Round(.., MidpointRounding.AwayFromZero) in BillAmount / proportions / DetailAmount / Commission (rndA),
 * Conversion.ToDouble is lenient (no FormatException), one Freight + Transporter field instead of a freight grid, no Branch / Project / WtCut / RateCut fields.
 */
public class SaleSaltDirectCalc {

    /** dtGrid (36 columns in the designer order; the field names are the column names). */
    public static class Line {
        public int Id, OrderId, OrderDetailId, OrderNo, WarehouseId, ItemId, JobLotId, PackingTypeId, PackUOMId, RateUOMId, GpNo;
        public String Item = "", ItemCode = "", JobLot = "", PackingType = "", GpDate = "", VehicleNo = "";
        public double PackUOM, ItemQty, BalQty, GrossWeight, BalWeight, AddLss, NetBillWeight, StockWeight, Rate, RateUOM, RateCutAmount, ItemAmount,
                FcyAmount, BillAmount, Expense, Freights, Commission;
    }

    /** dtInvExp: ItemId, Qty, Rate, Amount, Remarks. */
    public static class Ex { public int ItemId; public double Qty, Rate, Amount; public String Remarks = ""; }

    /** dtGrdGL: AccountId, Remarks, Percentage, Qty, Rate, Debit, Credit. */
    public static class Jv { public int AccountId; public String Remarks = ""; public double Percentage, Qty, Rate, Debit, Credit; }

    /** The entry panel (groupBox1): combos as id + text, text boxes as text. */
    public static class E {
        public int warehouseId, itemId, jobLotId, packingTypeId, packUomId, rateUomId, cityId;
        public String warehouseText = "", itemText = "", itemCode = "", jobLotText = "", packingText = "", packUomText = "", rateUomText = "", cityText = "";
        public String qty = "", gross = "", addLss = "", netBill = "", stock = "", rate = "", rateCutTotal = "", amount = "", gpDate = "", gpNo = "", vehicleNo = "", remarks = "";
    }

    /** The form. */
    public static class Inv {
        public int id, supplierId, supplierGlId, paymentTermId, currencyId, commAgentId, transporterId, updateIndex = -1;
        public boolean orderExist, resetOnSave, approved, saveMode = true;
        public boolean supplierEnabled = true, paymentTermEnabled = true, deliveryTermEnabled = true, itemEnabled = true, dueDaysEnabled = true,
                commTypeEnabled, commRateEnabled, commUomEnabled, commAmountEnabled, fieldsEnabled = true;
        public String docNo = "", docDate = "", refNo = "", billNo = "", dueDays = "0", dueDate = "", deliveryTerm = "", exchangeRate = "", invoiceQty = "", invoiceWeight = "",
                fcyAmount = "", billAmount = "", commType = "", commRate = "", commUom = "", commAmount = "", commRemarks = "", remarks = "", freightText = "",
                totalGrossOrder = "", totalQtyOrder = "", focus = "";
        public E e = new E();
        public List<Line> lines = new ArrayList<>();
        public List<Ex> exp = new ArrayList<>();
        public List<Jv> gl = new ArrayList<>();
        public List<String> messages = new ArrayList<>();
    }

    /** CommonServices.GetUomScheduleByItemId rows of an item (Id, UOMCode, Equivalent) as the pack / rate UOM combos hold them. */
    public interface Lookups {
        Double equivalent(int itemId, int uomId);
        String code(int itemId, int uomId);
        Integer idByCode(int itemId, String code);
        /** VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(DocumentTypeIds 1608, Ids = currency): LastExchRate or null. */
        Double lastExchangeRate(int currencyId);
    }

    private final Cfg c;
    private final Inv v;
    private final Lookups lk;
    private final int outwardAcId, baseCurrency;
    private final double baseRate;
    private final boolean itemsAgainstWarehouse;

    public SaleSaltDirectCalc(Cfg c, Inv v, Lookups lk, int outwardAcId, int baseCurrency, double baseRate) {
        this.c = c; this.v = v; this.lk = lk; this.outwardAcId = outwardAcId; this.baseCurrency = baseCurrency; this.baseRate = baseRate; this.itemsAgainstWarehouse = false;
        if (v.lines == null) v.lines = new ArrayList<>();
        if (v.exp == null) v.exp = new ArrayList<>();
        if (v.gl == null) v.gl = new ArrayList<>();
        if (v.e == null) v.e = new E();
        v.messages = new ArrayList<>();
        v.focus = "";
    }

    // ------------------------------------------------------------------ helpers

    private static double D(String s) { return toDouble(s); }
    private String amt(double x) { return fmt(x, c.amtDec); }
    private String rateText(double x) { return fmt(x, c.rateDec); }
    private double tot(ToDoubleFunction<Line> f) { double s = 0; for (Line l : v.lines) s += f.applyAsDouble(l); return s; }
    private double totExp() { double s = 0; for (Ex e : v.exp) s += e.Amount; return s; }
    private static double fin(double x) { return Double.isNaN(x) || Double.isInfinite(x) ? 0.0 : x; }

    private void tryRun(Runnable r) {
        try { r.run(); } catch (RuntimeException ex) { v.messages.add(ex.getMessage() == null ? ex.toString() : ex.getMessage()); }
    }

    /** Math.Round(x, digits, MidpointRounding.AwayFromZero) of .NET Core (value * 10^digits, ModF, |fraction| >= 0.5 moves away from zero). */
    public static double rndA(double x, int digits) {
        if (digits < 0 || digits > 15) throw new IllegalArgumentException("Rounding digits must be between 0 and 15, inclusive.");
        if (Double.isNaN(x) || Double.isInfinite(x) || Math.abs(x) >= 1e16) return x;
        double p = Math.pow(10, digits), t = x * p, ip = t < 0 ? Math.ceil(t) : Math.floor(t), fr = t - ip;
        if (Math.abs(fr) >= 0.5) ip += Math.signum(fr);
        return ip / p;
    }

    private double ra(double x) { return rndA(x, c.amtRound); }

    /** Conversion.ToDecimal(double): non finite values are 0. */
    private static double decd(double x) { return Double.isNaN(x) || Double.isInfinite(x) ? 0.0 : dec(x).doubleValue(); }

    // ------------------------------------------------------------------ entry panel text boxes (TextChanged only when the text changes)

    private String get(String f) {
        E e = v.e;
        switch (f) {
            case "qty": return e.qty; case "gross": return e.gross; case "addLss": return e.addLss; case "netBill": return e.netBill; case "stock": return e.stock;
            case "rate": return e.rate; case "rateCutTotal": return e.rateCutTotal; case "amount": return e.amount;
            default: throw new IllegalArgumentException(f);
        }
    }

    private void put(String f, String val) {
        E e = v.e;
        switch (f) {
            case "qty": e.qty = val; break; case "gross": e.gross = val; break; case "addLss": e.addLss = val; break; case "netBill": e.netBill = val; break;
            case "stock": e.stock = val; break; case "rate": e.rate = val; break; case "rateCutTotal": e.rateCutTotal = val; break; case "amount": e.amount = val; break;
            default: throw new IllegalArgumentException(f);
        }
    }

    /** TextBox.Text = value: the TextChanged handler of the desktop runs when the text changed. */
    private void set(String f, String val) {
        String nv = val == null ? "" : val;
        if (get(f).equals(nv)) return;
        put(f, nv);
        changed(f);
    }

    private void changed(String f) {
        switch (f) {
            case "qty": hQty(); break;                                      // txtQty_TextChanged :4048
            case "gross": hGross(); break;                                  // txtGrossWeight_TextChanged :4067
            case "addLss": hAddLss(); break;                                // txtAddLss_TextChanged :4087
            case "rate": case "rateCutTotal": hRate(); break;               // txtRate_TextChanged :4105 / txtratecuttotal_TextChanged :4228
            default: break;                                                 // txtStockWeight / txtNetBillWeight / txtAmount: no handler of consequence
        }
    }

    private void hQty() { tryRun(() -> { totalWeight(); total(); detailAmount(); totalCommissionAmount(); billAmount(); }); }
    private void hGross() { total(); detailAmount(); }
    private void hAddLss() { total(); detailAmount(); totalCommissionAmount(); billAmount(); }
    private void hRate() { detailAmount(); totalCommissionAmount(); billAmount(); }

    /** An event raised by the browser: the entry panel already holds the new text. */
    public void entryEvent(String name) {
        switch (name) {
            case "qty": hQty(); break;
            case "gross": hGross(); break;
            case "addless": hAddLss(); break;
            case "rate": case "ratecut": case "rateuom": hRate(); break;     // comRateUOM_Leave :4093
            case "packuom": tryRun(() -> { totalWeight(); total(); detailAmount(); totalCommissionAmount(); billAmount(); }); break;   // comPackUOM_Leave :4120
            default: break;
        }
    }

    /** ((UltraDropDownBase)combo).SelectedRow.Cells[2].Value : NullReferenceException when the combo has no selected row. */
    private double equivalent(int itemId, int uomId) {
        Double q = uomId == 0 ? null : lk.equivalent(itemId, uomId);
        if (q == null) throw new IllegalStateException("Object reference not set to an instance of an object.");
        return q;
    }

    /** TotalWeight :3926 */
    private void totalWeight() {
        tryRun(() -> {
            E e = v.e;
            double pack = 0, qty = 0;
            if (e.packUomId != 0 && !e.packUomText.isEmpty()) pack = equivalent(e.itemId, e.packUomId);          // ActiveRow != null && Value != 0
            if (D(e.qty) != 0.0) qty = D(e.qty);
            set("gross", hash(dec(pack * qty), 3));
        });
    }

    /** Total :3889 */
    private void total() {
        tryRun(() -> {
            E e = v.e;
            double gross = 0, addLess = 0;
            if (!e.packUomText.isEmpty() && e.packUomId != 0) equivalent(e.itemId, e.packUomId);
            if (!e.gross.isEmpty()) gross = D(e.gross.trim());
            if (!e.addLss.isEmpty()) addLess = Math.abs(D(e.addLss.trim()));
            double net = gross - addLess;
            String t = hash(dec(rnd(net, 2)), 2);
            e.netBill = t;
            e.stock = t;
        });
    }

    /** DetailAmountCaluculation :3953 */
    private void detailAmount() {
        tryRun(() -> {
            E e = v.e;
            double rateUom = (e.rateUomText.isEmpty() || e.rateUomId == 0) ? 0.0 : equivalent(e.itemId, e.rateUomId);
            double rate = 0, net = 0;
            if (D(e.rate) != 0.0) rate = D(e.rate);
            if (D(e.netBill) != 0.0) net = D(e.netBill);
            if (net > 0.0 && rateUom > 0.0 && rate > 0.0) {
                double item = ra(net / rateUom * rate);
                double cut = D(e.rateCutTotal);
                item = ra(item - cut);
                e.amount = amt(item);
            } else e.amount = "0";
        });
    }

    // ------------------------------------------------------------------ the form methods

    /** BillAmount :3707 */
    public void billAmount() {
        tryRun(() -> {
            if (!v.lines.isEmpty()) {
                BigDecimal tq = dec(fin(tot(l -> l.ItemQty))), tw = dec(fin(tot(l -> l.NetBillWeight)));
                double item = fin(tot(l -> l.ItemAmount)), ex = fin(totExp());
                double jd = 0, jc = 0;
                for (Jv r : v.gl) if (r.AccountId > 0) { jd += r.Debit; jc += r.Credit; }
                item = ra(item); ex = ra(ex); jd = ra(jd); jc = ra(jc);
                double fr = ra(D(v.freightText.trim()));
                double bill = item + ex - fr;
                bill = bill + jc - jd;
                if (v.supplierId == v.commAgentId) bill -= D(v.commAmount.trim());
                v.billAmount = amt(ra(bill));
                v.invoiceQty = hash(rndDec(tq, 2), 2);
                v.invoiceWeight = hash(rndDec(tw, 2), 2);
                v.fcyAmount = fmt(bill / D(v.exchangeRate), c.fcyDec);
            } else { v.invoiceQty = "0"; v.invoiceWeight = "0"; v.fcyAmount = "0"; }
            billProportion();
        });
    }

    /** ExpProportion :3767 */
    public void expProportion() {
        tryRun(() -> {
            double total = totExp(), nw = tot(l -> l.NetBillWeight);
            for (Line l : v.lines) l.Expense = fin(ra(total / nw * l.NetBillWeight));
            billProportion();
        });
    }

    /** BillProportion :3788 (bool.Parse of the configuration: a bad text is the FormatException of the desktop). */
    public void billProportion() {
        tryRun(() -> {
            for (Line l : v.lines) {
                double item = ra(l.ItemAmount), comm = ra(l.Commission), exp = ra(l.Expense);
                if (c.creditRaw != null) {
                    String t = c.creditRaw.trim();
                    boolean b;
                    if (t.equalsIgnoreCase("true")) b = true;
                    else if (t.equalsIgnoreCase("false")) b = false;
                    else throw new IllegalArgumentException("String was not recognized as a valid Boolean.");
                    l.BillAmount = b ? ra(item - comm) : item;
                } else l.BillAmount = ra(item + exp - comm);
            }
        });
    }

    /** CommissionProportion :3824 */
    public void commissionProportion() {
        tryRun(() -> {
            double total = D(v.commAmount), pct = D(v.commRate), nw = tot(l -> l.NetBillWeight);
            for (Line l : v.lines) {
                if ("Percent".equals(v.commType)) l.Commission = fin(ra(l.ItemAmount * pct / 100.0));
                else l.Commission = fin(ra(total / nw * l.NetBillWeight));
            }
            billProportion();
        });
    }

    /** FreightProportion :3856 */
    public void freightProportion() {
        tryRun(() -> {
            double total = D(v.freightText.trim());
            if (total > 0.0) {
                double nw = tot(l -> l.NetBillWeight);
                for (Line l : v.lines) l.Freights = fin(ra(total / nw * l.NetBillWeight));
                billProportion();
            } else for (Line l : v.lines) l.Freights = 0;
        });
    }

    /** TotalCommissionAmount :3991 */
    public void totalCommissionAmount() {
        tryRun(() -> {
            String t = v.commType;
            if (!v.commRate.isEmpty()) {
                if ("Flat".equals(t)) v.commAmount = amt(ra(D(v.commRate.trim())));
                if ("Percent".equals(t) || "Percentage".equals(t)) v.commAmount = amt(ra(fin(tot(l -> l.ItemAmount)) * D(v.commRate.trim()) / 100.0));
                if ("Comm Weight".equals(t)) {
                    double w = fin(tot(l -> l.NetBillWeight)), uom = D(v.commUom.trim());
                    v.commAmount = amt(ra(w / uom * D(v.commRate.trim())));
                }
            }
            commissionProportion();
        });
    }

    /** txtExchangeRate_TextChanged :1278 */
    public void exchangeChanged() {
        tryRun(() -> {
            if (!v.lines.isEmpty() && decText(v.exchangeRate).signum() > 0) {
                for (Line l : v.lines) l.FcyAmount = decd(l.ItemAmount / D(v.exchangeRate));
            } else for (Line l : v.lines) l.FcyAmount = 0;
            billAmount();
        });
    }

    /** txtExchangeRate_Leave :1311 */
    public void exchangeLeave() {
        tryRun(() -> setExchange(fmt(decText(v.exchangeRate), c.rateDec)));
    }

    /** cmbCurrency_Leave :1235 */
    public void currencyLeave() {
        tryRun(() -> {
            if (v.currencyId == 0 || D(v.exchangeRate) != 0.0) return;
            if (v.currencyId != baseCurrency) {
                Double r = lk.lastExchangeRate(v.currencyId);
                setExchange(r != null ? rateText(r) : "0");
            } else setExchange(rateText(baseRate));
        });
    }

    private void setExchange(String text) {
        if (!v.exchangeRate.equals(text)) { v.exchangeRate = text; exchangeChanged(); }
    }

    /** txtFreightAmount_TextChanged :4180 */
    public void freightTextChanged() {
        tryRun(() -> { if (!v.lines.isEmpty()) { freightProportion(); billAmount(); } });
    }

    /** combdeliverytrm_TextChanged :4310 */
    public void deliveryTermChanged() {
        tryRun(() -> {
            if ("Ponch".equals(v.deliveryTerm)) { v.transporterId = outwardAcId; return; }
            v.transporterId = 0;
            if (!"0".equals(v.freightText)) { v.freightText = "0"; freightTextChanged(); } else v.freightText = "0";
        });
    }

    /** DueDateGenerate :4290 (txtduedays_TextChanged / DocDate_ValueChanged) */
    public void dueDateGenerate() {
        tryRun(() -> {
            LocalDate d = LocalDate.parse(v.docDate.trim().substring(0, 10));
            v.dueDate = (v.dueDays.trim().isEmpty() ? d : d.plusDays((long) Math.floor(D(v.dueDays.trim())))).toString();
        });
    }

    /** combpttrm_Leave :5109 */
    public void paymentTermLeave() {
        tryRun(() -> {
            v.dueDaysEnabled = true;
            if (v.paymentTermId == 1) { setDueDays(""); v.dueDaysEnabled = false; }
        });
    }

    private void setDueDays(String t) {
        if (!v.dueDays.equals(t)) { v.dueDays = t; dueDateGenerate(); }
    }

    /** cmbsuppliername_ValueChanged / combCommType_Leave / combcommUOM_Leave / txtcommrate_TextChanged. */
    public void commissionChanged() { totalCommissionAmount(); billAmount(); }

    /** combcommAgent_Leave :4195 */
    public void agentLeave() {
        totalCommissionAmount();
        billAmount();
        enableDisableCommissionFields();
    }

    /** EnableDiableCommissionFields :4215 */
    public void enableDisableCommissionFields() {
        tryRun(() -> {
            if (v.commAgentId > 0) {
                v.commTypeEnabled = true; v.commRateEnabled = true;
                v.commUomEnabled = "Comm Weight".equals(v.commType);       // combCommType.Value == 3
                v.commAmountEnabled = true;
                v.focus = "CmbCommType";
            } else { v.commTypeEnabled = false; v.commRateEnabled = false; v.commUomEnabled = false; v.commAmountEnabled = false; }
        });
    }

    public void supplierChanged() { billAmount(); }

    // ------------------------------------------------------------------ sub grids

    public void addExpRow() { v.exp.add(new Ex()); }
    public void addGlRow() { v.gl.add(new Jv()); }

    /** grdInvExp_ColumnButtonClick :1403 (the confirmation is asked by the browser) */
    public void expButton(String key, int row) {
        tryRun(() -> {
            if ("Delete".equals(key)) {
                if (row >= 0 && row < v.exp.size()) v.exp.remove(row);
                if (v.exp.isEmpty()) addExpRow();
            }
            if ("Add".equals(key)) addExpRow();
            billAmount();
        });
    }

    /** grdInvExp_CellUpdated :1422 */
    public void expCell(int row, String col) {
        tryRun(() -> {
            if (row >= 0 && row < v.exp.size()) {
                Ex e = v.exp.get(row);
                if ("Qty".equals(col) || "Rate".equals(col)) e.Amount = ra(e.Qty * e.Rate);
                else if ("Amount".equals(col)) e.Amount = ra(e.Amount);
            }
            billAmount();
            expProportion();
            freightProportion();
        });
    }

    /** grdInvExp_KeyDown :4984: Ctrl+Space on Delete / Add, Ctrl+Delete, Ctrl+D; each ends with txtExchangeRate_TextChanged. */
    public void expKey(String key, int row) {
        tryRun(() -> {
            if ("Delete".equals(key)) {
                if (row >= 0 && row < v.exp.size()) v.exp.remove(row);
                if (v.exp.isEmpty()) addExpRow();
            }
            if ("Add".equals(key)) addExpRow();
            exchangeChanged();
        });
    }

    /** grdGLedger_ColumnButtonClick :1590 (BillAmount runs after the try block in every case). */
    public void glButton(String key, int row) {
        tryRun(() -> {
            if ("Delete".equals(key)) {
                if (row >= 0 && row < v.gl.size()) v.gl.remove(row);
                if (v.gl.isEmpty()) addGlRow();
            }
            if ("Add".equals(key)) addGlRow();
        });
        billAmount();
    }

    /** grdGLedger_CellUpdated :1620 */
    public void glCell(int row, String col) {
        tryRun(() -> {
            if (row >= 0 && row < v.gl.size()) {
                Jv j = v.gl.get(row);
                if ("Qty".equals(col) || "Rate".equals(col)) { j.Credit = ra(j.Qty * j.Rate); j.Debit = 0; j.Percentage = 0; }
                if ("Percentage".equals(col)) {
                    double t = fin(tot(l -> l.ItemAmount)) / 100.0 * j.Percentage;
                    if (t > 0.0) { j.Credit = ra(t); j.Debit = 0; } else { j.Debit = Math.abs(ra(t)); j.Credit = 0; }
                    j.Qty = 0; j.Rate = 0;
                }
                if ("Credit".equals(col)) {
                    j.Credit = ra(j.Credit);
                    if (j.Debit > 0.0) { j.Credit = 0; v.messages.add("Debit Side is aleady added"); }
                }
                if ("Debit".equals(col)) {
                    j.Debit = ra(j.Debit);
                    if (j.Credit > 0.0) { j.Debit = 0; v.messages.add("Credit Side is aleady added"); }
                }
            }
            billAmount();
        });
    }

    /**
     * grdGLedger_KeyDown :4918 - the data row is deleted BEFORE the confirmation (desktop defect: answering No keeps the grid row but the table row is gone);
     * yes = the answer of the confirmation. Ctrl+Space on Delete / Add, Ctrl+Delete, Ctrl+D.
     */
    public void glKey(String key, int row, boolean yes) {
        tryRun(() -> {
            if ("Delete".equals(key)) {
                if (row >= 0 && row < v.gl.size()) v.gl.remove(row);
                if (!yes) return;
                if (v.gl.isEmpty()) addGlRow();
                billAmount();
            } else if ("Add".equals(key)) { addGlRow(); billAmount(); }
        });
    }

    // ------------------------------------------------------------------ detail grid and the entry buttons

    private void recalcAll() { expProportion(); freightProportion(); commissionProportion(); billProportion(); billAmount(); }

    /** grd_ColumnButtonClick :1970 (Delete: the mouse button does NOT recalculate; the confirmation is asked by the browser). */
    public void gridButton(String key, int row) {
        tryRun(() -> {
            if (row < 0 || row >= v.lines.size()) return;
            if ("Add".equals(key)) addRowLoader(row);
            if ("Edit".equals(key)) editRow(row);
            if ("Delete".equals(key)) v.lines.remove(row);
        });
    }

    /** grd_KeyDown :4806: key = Delete / Add / Edit (Ctrl+Space on that column), CtrlDelete, CtrlD. yes = answer of the confirmation. */
    public void gridKey(String key, int row, boolean yes) {
        tryRun(() -> {
            if (row < 0 || row >= v.lines.size()) return;
            if ("Delete".equals(key) || "Add".equals(key) || "Edit".equals(key)) {
                if ("Delete".equals(key)) { if (!yes) return; v.lines.remove(row); }
                if ("Add".equals(key)) addRowLoader(row);
                if ("Edit".equals(key)) editRow(row);
                recalcAll();
            }
            if ("CtrlD".equals(key)) addRowLoader(row);
            if ("CtrlDelete".equals(key)) {
                if (!yes) return;
                v.lines.remove(row);
                recalcAll();
            }
            if (v.lines.isEmpty()) v.orderExist = false;
        });
    }

    /** grd_CellUpdated :2014 (the browser has already put the typed value into the cell of the row). */
    public void gridCell(int row, String col) {
        tryRun(() -> {
            if (!v.orderExist || row < 0 || row >= v.lines.size()) return;
            Line it = v.lines.get(row);
            double exch = D(v.exchangeRate);
            if ("ItemQty".equals(col)) {
                double qty = it.ItemQty, balQty = it.BalQty, gross, balWt = it.BalWeight, addLss = it.AddLss, net;
                if (qty > balQty) {
                    v.messages.add("Item Cannot be greater than Balance Qty " + num(balQty) + " Of this row");
                    qty = balQty; it.ItemQty = qty;
                    gross = balWt; it.GrossWeight = gross;
                    net = gross - Math.abs(addLss); it.NetBillWeight = net; it.StockWeight = net;
                } else {
                    gross = it.PackUOM * qty; it.GrossWeight = gross;
                    net = gross - Math.abs(addLss); it.NetBillWeight = net; it.StockWeight = net;
                    if (gross > balWt) { gross = balWt; it.GrossWeight = gross; net = gross - Math.abs(addLss); it.NetBillWeight = net; it.StockWeight = net; }
                }
                amountOfRow(it, net, exch);
            } else if ("GrossWeight".equals(col)) {
                double gross = it.GrossWeight, balWt = it.BalWeight, addLss = it.AddLss, net;
                if (gross > balWt) {
                    v.messages.add("GrossWeight Cannot be greater than Balance weight " + num(balWt) + " Of this row");
                    gross = balWt; it.GrossWeight = gross;
                    net = gross - Math.abs(addLss); it.NetBillWeight = net; it.StockWeight = net;
                } else {
                    net = gross - Math.abs(addLss); it.NetBillWeight = net; it.StockWeight = net;
                }
                amountOfRow(it, net, exch);
            } else if ("AddLss".equals(col)) {
                double gross = it.GrossWeight, net = gross > 0.0 ? gross - Math.abs(it.AddLss) : 0.0;
                it.NetBillWeight = net; it.StockWeight = net;
                amountOfRow(it, net, exch);
            } else if ("RateCutAmount".equals(col)) {
                double net = it.NetBillWeight, after = 0.0;
                if (net > 0.0) after = net / it.RateUOM * it.Rate - it.RateCutAmount;
                after = fin(after);
                it.ItemAmount = after;
                it.FcyAmount = decd(after / exch);
            }
            recalcAll();
        });
    }

    /** the common tail of the ItemQty / GrossWeight / AddLss branches of grd_CellUpdated */
    private void amountOfRow(Line it, double net, double exch) {
        double after = 0.0;
        if (net > 0.0) after = net / it.RateUOM * it.Rate - it.RateCutAmount;
        after = fin(after);
        it.ItemAmount = after;
        it.FcyAmount = decd(after / exch);
    }

    /** AddRowInDetailGridInCaseOfLoader :2194 */
    public void addRowLoader(int rowIdx) {
        tryRun(() -> {
            if (rowIdx < 0 || rowIdx >= v.lines.size() || !v.orderExist) return;
            Line r = v.lines.get(rowIdx);
            double totalWeight = D(v.totalGrossOrder.trim()), totalQty = D(v.totalQtyOrder.trim());
            double weight = tot(l -> l.GrossWeight), qty = tot(l -> l.ItemQty);
            double num = totalWeight - weight, grossQty = totalQty - qty;
            if (num > 0.0) {
                if (grossQty > 0.0) {
                    double itemWeight = grossQty * r.PackUOM, addless = r.AddLss, netWt = itemWeight - addless, amount = netWt / r.RateUOM * r.Rate - r.RateCutAmount;
                    Line n = new Line();
                    n.Id = 0; n.OrderId = r.OrderId; n.OrderDetailId = r.OrderDetailId; n.OrderNo = r.OrderNo; n.WarehouseId = r.WarehouseId; n.ItemId = r.ItemId; n.Item = r.Item;
                    n.ItemCode = r.ItemCode; n.JobLotId = r.JobLotId; n.JobLot = r.JobLot; n.PackingTypeId = r.PackingTypeId; n.PackingType = r.PackingType; n.PackUOMId = r.PackUOMId;
                    n.PackUOM = r.PackUOM; n.ItemQty = rnd(grossQty, 2); n.BalQty = rnd(grossQty, 2); n.GrossWeight = rnd(itemWeight, 2); n.BalWeight = rnd(itemWeight, 2);
                    n.AddLss = rnd(addless, 2); n.NetBillWeight = rnd(netWt, 2); n.StockWeight = rnd(netWt, 2); n.Rate = r.Rate; n.RateUOMId = r.RateUOMId; n.RateUOM = r.RateUOM;
                    n.RateCutAmount = r.RateCutAmount; n.ItemAmount = fin(rnd(amount, 2)); n.GpDate = r.GpDate; n.GpNo = r.GpNo; n.VehicleNo = r.VehicleNo;
                    n.FcyAmount = decd(amount / D(v.exchangeRate)); n.BillAmount = r.BillAmount; n.Expense = r.Expense; n.Freights = r.Freights; n.Commission = r.Commission;
                    v.lines.add(n);
                } else v.messages.add("Please Check Grid Qty is Already Completed");
            } else v.messages.add("Please Check Grid GrossWeight is Already Completed");
            expProportion(); freightProportion(); commissionProportion(); billProportion(); billAmount();
        });
    }

    /** FormValidationDetila :2366 - false (and the message) when a field is missing. */
    private boolean validateDetail() {
        E e = v.e;
        if (e.warehouseId == 0) return fail("Warehouse Field is Required", "CmbWarehouse");
        if (e.itemId == 0) return fail("Item Name Field is Required", "CmbItem");
        if (e.jobLotId == 0) return fail("Job/Lot Field is Required", "CmbJobLot");
        if (e.packingTypeId == 0) return fail("Packing Type Field is Required", "CmbPackingType");
        if (e.packUomId == 0) return fail("Pack Unit Field is Required", "CmbPackUom");
        if (blank0(e.qty)) return fail("Qty Field is Required", "txtQty");
        if (blank0(e.gross)) return fail("Gross Weight Field is Required", "txtGrossWeight");
        if (blank0(e.netBill)) return fail("Net Bill Weight Field is Required", "txtNetBillWeight");
        if (blank0(e.stock)) return fail("Stock Weight Field is Required", "txtStockWeight");
        if (blank0(e.rate)) return fail("Rate Field is Required", "txtRate");
        if (e.rateUomId == 0) return fail("Rate UOM Field is Required", "CmbRateUom");
        if (e.amount.isEmpty() || "0".equals(e.amount)) throw new IllegalStateException("Please Check Item Amount");
        return true;
    }

    private static boolean blank0(String s) { String t = s == null ? "" : s.trim(); return t.isEmpty() || t.equals("0"); }

    private boolean fail(String m, String focus) { v.messages.add(m); v.focus = focus; return false; }

    /** btnAdd_Click :1685 */
    public void addDetail() {
        tryRun(() -> {
            if (!v.lines.isEmpty() && v.orderExist) throw new IllegalStateException("You can not add manual Record beacause record against order exist in Grid");
            if (validateDetail()) {
                E e = v.e;
                Line r = new Line();
                r.WarehouseId = e.warehouseId; r.ItemId = e.itemId; r.Item = e.itemText; r.ItemCode = e.itemCode; r.JobLotId = e.jobLotId; r.JobLot = e.jobLotText;
                r.PackingTypeId = e.packingTypeId; r.PackingType = e.packingText; r.PackUOMId = e.packUomId; r.PackUOM = equivalent(e.itemId, e.packUomId);
                r.ItemQty = D(e.qty); r.BalQty = D(e.qty); r.GrossWeight = D(e.gross); r.BalWeight = D(e.gross); r.AddLss = D(e.addLss); r.NetBillWeight = D(e.netBill);
                r.StockWeight = D(e.stock); r.Rate = D(e.rate); r.RateUOMId = e.rateUomId; r.RateUOM = equivalent(e.itemId, e.rateUomId); r.RateCutAmount = D(e.rateCutTotal);
                r.ItemAmount = D(e.amount); r.GpDate = e.gpDate; r.GpNo = toInt(e.gpNo); r.VehicleNo = e.vehicleNo;
                v.lines.add(r);
                exchangeLeave();
                totalCommissionAmount();
                freightProportion();
                commissionProportion();
                expProportion();
                billAmount();
                resetDetail();
            }
        });
    }

    /** btnUpdateDetail_Click :1911 (the row keeps its BalQty / BalWeight / Item / ItemCode / Fcy: the desktop does not write them) */
    public void updateDetail() {
        tryRun(() -> {
            if (validateDetail()) {
                if (v.updateIndex < 0 || v.updateIndex >= v.lines.size()) throw new IllegalStateException("There is no row at position " + v.updateIndex + ".");
                E e = v.e;
                Line r = v.lines.get(v.updateIndex);
                r.ItemId = e.itemId; r.Item = e.itemText; r.ItemCode = e.itemCode; r.JobLotId = e.jobLotId; r.JobLot = e.jobLotText; r.PackingTypeId = e.packingTypeId;
                r.PackingType = e.packingText; r.ItemQty = D(e.qty); r.PackUOMId = e.packUomId; r.PackUOM = equivalent(e.itemId, e.packUomId); r.GrossWeight = D(e.gross);
                r.AddLss = D(e.addLss); r.NetBillWeight = D(e.netBill); r.StockWeight = D(e.stock); r.Rate = D(e.rate); r.RateUOMId = e.rateUomId;
                r.RateUOM = equivalent(e.itemId, e.rateUomId); r.RateCutAmount = D(e.rateCutTotal); r.ItemAmount = D(e.amount); r.WarehouseId = e.warehouseId;
                r.GpDate = e.gpDate; r.GpNo = toInt(e.gpNo); r.VehicleNo = e.vehicleNo;
                v.updateIndex = -1;                                         // btnAdd.Visible = true, btnUpdateDetail / btnCancelUpdateDetial hidden
                exchangeLeave();
                totalCommissionAmount();
                freightProportion();
                commissionProportion();
                expProportion();
                billAmount();
                resetDetail();
            }
        });
    }

    /** btnCancelUpdateDetial_Click: ResetDetail only (the buttons stay as they are in the desktop; updateIndex is kept). */
    public void cancelUpdate() { resetDetail(); }

    /** ResetDetail :2561 */
    public void resetDetail() {
        tryRun(() -> {
            E e = v.e;
            e.packUomId = 0; e.packUomText = "";
            set("qty", ""); set("gross", ""); set("addLss", ""); set("netBill", ""); set("stock", ""); set("rate", "");
            e.rateUomId = 0; e.rateUomText = "";
            set("rateCutTotal", ""); set("amount", "");
            if (v.resetOnSave) optionResetFields();
            v.focus = "CmbItem";
        });
    }

    /** OptionResetFields :2587 */
    private void optionResetFields() {
        E e = v.e;
        e.itemId = 0; e.itemText = ""; e.itemCode = ""; e.jobLotId = 0; e.jobLotText = ""; e.packingTypeId = 0; e.packingText = ""; e.warehouseId = 0; e.warehouseText = "";
        e.gpNo = ""; e.vehicleNo = ""; e.cityId = 0; e.cityText = "";
        v.focus = "CmbItem";
    }

    /**
     * grd_DoubleClick :1873 (not while an order is loaded). The combos are set to the ids of the row in the desktop's order; the text boxes assigned on the way
     * raise their TextChanged handlers (the pack UOM combo still holds the text kept by PackUOM() while the Qty box is assigned).
     */
    public void editRow(int row) {
        tryRun(() -> {
            if (row < 0 || row >= v.lines.size() || v.orderExist) return;
            Line r = v.lines.get(row);
            E e = v.e;
            v.updateIndex = row;
            e.warehouseId = r.WarehouseId;
            e.itemId = r.ItemId; e.itemText = r.Item; e.itemCode = r.ItemCode;
            String keepP = e.packUomText, keepR = e.rateUomText;                       // PackUOM() :991
            Integer p = keepP.isEmpty() ? null : lk.idByCode(e.itemId, keepP), q = keepR.isEmpty() ? null : lk.idByCode(e.itemId, keepR);
            e.packUomId = p == null ? 0 : p; e.packUomText = p == null ? "" : keepP;
            e.rateUomId = q == null ? 0 : q; e.rateUomText = q == null ? "" : keepR;
            e.jobLotId = r.JobLotId; e.packingTypeId = r.PackingTypeId;
            set("qty", hash(dec(r.ItemQty), 3));
            e.packUomId = r.PackUOMId; e.packUomText = nz(lk.code(r.ItemId, r.PackUOMId));
            set("gross", hash(dec(r.GrossWeight), 3));
            set("addLss", hash(dec(r.AddLss), 3));
            set("netBill", hash(dec(r.NetBillWeight), 3));
            set("stock", hash(dec(r.StockWeight), 3));
            set("rate", rateText(r.Rate));
            e.rateUomId = r.RateUOMId; e.rateUomText = nz(lk.code(r.ItemId, r.RateUOMId));
            set("rateCutTotal", amt(r.RateCutAmount));
            set("amount", amt(r.ItemAmount));
            e.gpDate = r.GpDate; e.gpNo = String.valueOf(r.GpNo); e.vehicleNo = r.VehicleNo;
            v.focus = "CmbWarehouse";
        });
    }

    private static String nz(String s) { return s == null ? "" : s; }

    // ------------------------------------------------------------------ sequences

    /** TotalGrosstInCaseOfOrderEntry :4620 */
    public void totalGrossInCaseOfOrderEntry() {
        tryRun(() -> {
            if (!v.lines.isEmpty()) {
                v.totalGrossOrder = num(fin(tot(l -> l.GrossWeight)));
                v.totalQtyOrder = num(fin(tot(l -> l.ItemQty)));
            }
        });
    }

    /** LoadInGridDetail :4544 - the tail after the header text boxes and the grid were filled. */
    public void afterLoad() {
        tryRun(() -> {
            totalGrossInCaseOfOrderEntry();
            expProportion();
            freightProportion();
            commissionProportion();
            billProportion();
            exchangeChanged();
        });
    }

    /** ReadById_Update :2964 - the tail after the header and the grids were filled. */
    public void afterRead() {
        tryRun(() -> {
            expProportion();
            freightProportion();
            commissionProportion();
            billAmount();
            exchangeChanged();
        });
    }

    /**
     * Reset :2483 - the clearing in the desktop's order; ConfigurationDefault is applied by the caller afterwards (combo ids from the configuration) and then
     * {@link #afterConfigurationDefault()}.
     */
    public void resetForm() {
        tryRun(() -> {
            v.fieldsEnabled = true;                                        // EnablesFields()
            v.id = 0; v.approved = false;
            v.supplierId = 0; v.refNo = ""; v.billNo = ""; v.commAgentId = 0; v.commType = "";
            v.invoiceQty = ""; v.invoiceWeight = ""; v.exchangeRate = "";
            v.commRemarks = ""; v.commRate = ""; v.commAmount = ""; v.commUom = ""; v.remarks = "";
            v.transporterId = 0; v.freightText = ""; v.dueDays = "0";
            E e = v.e;
            e.itemId = 0; e.itemText = ""; e.itemCode = ""; e.packUomId = 0; e.packUomText = "";
            e.qty = ""; e.gross = ""; e.addLss = ""; e.netBill = ""; e.stock = ""; e.rate = ""; e.rateUomId = 0; e.rateUomText = ""; e.rateCutTotal = ""; e.amount = "";
            v.supplierEnabled = true; v.paymentTermEnabled = true;
            v.exp.clear(); v.gl.clear(); v.lines.clear();
            addGlRow(); addExpRow();
            v.updateIndex = -1; v.saveMode = true;
            if (v.resetOnSave) optionResetFields();
            v.billAmount = "";
            dueDateGenerate();
        });
    }

    /** the end of Reset: ConfigurationDefault is done, then txtduedays / commission fields. */
    public void afterConfigurationDefault() {
        tryRun(() -> {
            v.dueDaysEnabled = true;
            if (v.paymentTermId == 1) { setDueDays(""); v.dueDaysEnabled = false; }
            enableDisableCommissionFields();
        });
    }

    /** ConfigurationDefault :1175 - Base Currency / BaseCurrencyRate (the combos of the entry panel are set by the caller). */
    public void configurationExchange(boolean hasBaseCurrency, boolean hasBaseRate) {
        tryRun(() -> {
            if (hasBaseCurrency && v.currencyId == 0) v.currencyId = baseCurrency;
            if (hasBaseRate && D(v.exchangeRate) == 0.0) setExchange(rateText(baseRate));
        });
    }
}
