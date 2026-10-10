package com.mst.services.sale.pcc;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.List;

/**
 * The amount rules of the Sale Pcc invoice forms (SaleInvoiceReturnConcrete / AgainstGDN / Direct): CalculateDetailAmount, grd_CellUpdated,
 * CalculateRowNetAmount and BillAmount. All rounding is Math.Round(x, DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero)
 * (BigDecimal HALF_UP), and every value the desktop shows in a text box is re-read from its formatted text, so it is cut to the displayed places
 * (clsGlobalVariables.stringFormatsingle: "#,##0." + 1..4 zeros, otherwise no decimals).
 */
public final class SalePccInvoiceCalc {
    private SalePccInvoiceCalc() { }

    /** The decimal settings CommonServices.GetDecimalConfiguration builds. */
    public static final class Fmt {
        /** DefaultNoofDecimalPointsForAmount (used as-is by Math.Round). */
        public final int amtRound;
        /** places of stringFormatsingle: the configuration when 1..4, otherwise 0. */
        public final int amtPlaces;
        /** DefaultNoofDecimalPointsForRate (>0 ? n : 2). */
        public final int rateRound;
        /** places of DecimalRateFormate: 1..4 -> n, 0 -> 2, otherwise 0. */
        public final int ratePlaces;
        /** places of stringFormatsingleForFcy: the DefaultNoOfDecimalPointsForFcyAmount configuration when 1..4, otherwise 0. */
        public final int fcyPlaces;

        public Fmt(int amount, int rate, int fcyAmount) {
            this.amtRound = Math.max(0, Math.min(15, amount));
            this.amtPlaces = amount >= 1 && amount <= 4 ? amount : 0;
            this.rateRound = rate > 0 ? Math.min(15, rate) : 2;
            this.ratePlaces = rate >= 1 && rate <= 4 ? rate : (rate == 0 ? 2 : 0);
            this.fcyPlaces = fcyAmount >= 1 && fcyAmount <= 4 ? fcyAmount : 0;
        }
    }

    public static BigDecimal rnd(BigDecimal v, int dp) { return v.setScale(dp, RoundingMode.HALF_UP); }

    /** value.ToString(stringFormatsingle) read back with Conversion.ToDecimal. */
    public static BigDecimal txtAmt(BigDecimal v, Fmt f) { return rnd(v, f.amtPlaces); }

    /** Math.Round(v, DefaultNoofDecimalPointsForAmount, AwayFromZero) then the text round trip. */
    public static BigDecimal amt(BigDecimal v, Fmt f) { return txtAmt(rnd(v, f.amtRound), f); }

    /** One detail row. In: the entry-panel / grid values. Out: the values CalculateDetailAmount, grd_CellUpdated and CalculateRowNetAmount produce. */
    public static final class Line {
        public BigDecimal qty = BigDecimal.ZERO, unit = BigDecimal.ZERO, weight = BigDecimal.ZERO;
        public BigDecimal price = BigDecimal.ZERO, addLess = BigDecimal.ZERO, rate = BigDecimal.ZERO;
        public int discTypeId;
        public BigDecimal discRate = BigDecimal.ZERO, discAmount = BigDecimal.ZERO;
        public boolean foc;
        // out
        public BigDecimal netWeight = BigDecimal.ZERO, itemAmount = BigDecimal.ZERO, withDisc = BigDecimal.ZERO;
        public BigDecimal fcy = BigDecimal.ZERO, commission = BigDecimal.ZERO, netAmount = BigDecimal.ZERO;
    }

    /** CalculateDetailAmount + the total of grd_CellUpdated / CalculateDiscountAndTotalAmount + txtExchangeRate_TextChanged (FcyAmount) + CalculateRowNetAmount. */
    public static void row(Line r, Fmt f, BigDecimal exchange) {
        if (r.unit.signum() > 0 && r.qty.signum() > 0) {
            r.netWeight = rnd(r.unit.multiply(r.qty).multiply(r.weight), 2);                 // txtNetWeight.Text = NetWeight.ToString("##,#.##")
            r.itemAmount = amt(r.unit.multiply(r.qty).multiply(r.rate), f);                  // Math.Round twice, then ToString(stringFormatsingle)
        } else {
            r.netWeight = rnd(r.netWeight, 2);
            r.itemAmount = BigDecimal.ZERO;
        }
        BigDecimal disc = r.discTypeId == 0 ? BigDecimal.ZERO : r.discAmount;
        if (r.itemAmount.signum() > 0 && disc.signum() > 0) r.withDisc = r.itemAmount.subtract(disc);
        else if (r.itemAmount.signum() > 0) r.withDisc = r.itemAmount;
        else r.withDisc = BigDecimal.ZERO;
        r.fcy = exchange != null && exchange.signum() > 0 ? r.withDisc.divide(exchange, 10, RoundingMode.HALF_EVEN) : BigDecimal.ZERO;
        r.commission = BigDecimal.ZERO;                                                      // CommissionProportion() has an empty body on the desktop
        r.netAmount = amt(amt(r.withDisc, f).add(amt(r.commission, f)), f);                  // CalculateRowNetAmount
    }

    /** One Party Add/Less row (grdPartyAddLs). */
    public static final class Jv {
        public int accountId, glAccountId;
        public BigDecimal debit = BigDecimal.ZERO, credit = BigDecimal.ZERO;
    }

    /** The header texts BillAmount() fills. */
    public static final class Totals {
        public BigDecimal itemAmount = BigDecimal.ZERO, discount = BigDecimal.ZERO, itemNet = BigDecimal.ZERO;
        public BigDecimal billWithoutCommission = BigDecimal.ZERO, bill = BigDecimal.ZERO, commNet = BigDecimal.ZERO;
        public BigDecimal qty = BigDecimal.ZERO, weight = BigDecimal.ZERO, fcy = BigDecimal.ZERO;
        public BigDecimal journalDebit = BigDecimal.ZERO, journalCredit = BigDecimal.ZERO;
    }

    /** BillAmount(): FOC rows are left out of the money totals but not of the quantity / weight. */
    public static Totals bill(List<Line> rows, List<Jv> journals, BigDecimal commAmount, int supplierId, int agentId, BigDecimal exchange, Fmt f) {
        Totals t = new Totals();
        if (rows.isEmpty()) return t;
        BigDecimal qty = BigDecimal.ZERO, wt = BigDecimal.ZERO, ia = BigDecimal.ZERO, da = BigDecimal.ZERO, wd = BigDecimal.ZERO;
        for (Line r : rows) {
            qty = qty.add(r.qty); wt = wt.add(r.netWeight);
            if (!r.foc) { ia = ia.add(r.itemAmount); da = da.add(r.discTypeId == 0 ? BigDecimal.ZERO : r.discAmount); wd = wd.add(r.withDisc); }
        }
        BigDecimal jd = BigDecimal.ZERO, jc = BigDecimal.ZERO;
        for (Jv j : journals) if (j.accountId > 0 || j.glAccountId > 0) { jd = jd.add(j.debit); jc = jc.add(j.credit); }
        t.journalDebit = jd; t.journalCredit = jc;
        BigDecimal bill = wd.add(jc).subtract(jd);
        t.itemAmount = amt(ia, f);
        t.discount = amt(da, f);
        t.itemNet = amt(wd, f);
        t.billWithoutCommission = amt(bill, f);
        if (agentId > 0 && commAmount.signum() > 0 && supplierId == agentId) { bill = bill.subtract(commAmount); t.commNet = commAmount; }
        t.bill = amt(bill, f);
        t.qty = rnd(qty, 2);
        t.weight = rnd(wt, 2);
        t.fcy = exchange != null && exchange.signum() > 0 && wd.signum() > 0 ? rnd(wd.divide(exchange, 10, RoundingMode.HALF_EVEN), f.fcyPlaces) : rnd(BigDecimal.ZERO, f.fcyPlaces);
        return t;
    }

    /** TotalCommissionAmount(): Flat -> the rate, Percent -> BillAmountWithoutCommission * rate / 100; any other type leaves the amount unchanged (null). */
    public static BigDecimal commission(String type, BigDecimal rate, BigDecimal billWithoutCommission, Fmt f) {
        if ("Flat".equals(type)) return amt(rate, f);
        if ("Percent".equals(type) || "Percentage".equals(type)) return amt(billWithoutCommission.multiply(rate).divide(BigDecimal.valueOf(100), 10, RoundingMode.HALF_EVEN), f);
        return null;
    }
}
