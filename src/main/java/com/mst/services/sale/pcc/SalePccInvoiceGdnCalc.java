package com.mst.services.sale.pcc;

import com.mst.services.sale.pcc.SalePccInvoiceCalc.Fmt;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.util.List;

import static com.mst.services.sale.pcc.SalePccInvoiceCalc.amt;
import static com.mst.services.sale.pcc.SalePccInvoiceCalc.rnd;

/**
 * The totals chain of the Sale Pcc invoices that carry freight / expense / wages / settlement-discount proportions
 * (547 SaleInvoiceAgainstGDNConcrete and 546 SaleInvoiceDirectConcrete, document types 1861 / 1856).
 *
 * Desktop methods (SaleInvoiceAgainstGDNConcrete.cs): OtherItemsProportion :4478, WagesProportion :4498, SettlementDiscountProportion :4525,
 * FreightHeaderProportion :4559, GridItemNetAmountCalculate :4435, txtExchangeRate_TextChanged :1231, BillAmount :4246, TotalCommissionAmount :4605.
 * The proportions are Math.Round(Total / TotalQty * RowQty, DefaultNoofDecimalPointsForAmount, AwayFromZero) (the grid cell keeps the rounded decimal).
 */
public final class SalePccInvoiceGdnCalc {
    private SalePccInvoiceGdnCalc() { }

    private static final MathContext DEC = new MathContext(28, RoundingMode.HALF_EVEN);       // System.Decimal precision

    /** One detail row, with the grid columns the chain reads and writes. */
    public static final class Row {
        public BigDecimal qty = BigDecimal.ZERO, netWeight = BigDecimal.ZERO;
        public BigDecimal itemAmount = BigDecimal.ZERO, discAmount = BigDecimal.ZERO, withDisc = BigDecimal.ZERO;
        public boolean foc;
        // written by the chain
        public BigDecimal expense = BigDecimal.ZERO, wages = BigDecimal.ZERO, settle = BigDecimal.ZERO, freight = BigDecimal.ZERO;
        public BigDecimal fcy = BigDecimal.ZERO, netAmount = BigDecimal.ZERO;
    }

    /** Header inputs of the chain. */
    public static final class Head {
        public BigDecimal expenseTotal = BigDecimal.ZERO;      // grdChargeToProduct Amount total
        public BigDecimal wagesTotal = BigDecimal.ZERO;        // grdContractorWages NetAmount total
        public BigDecimal freight = BigDecimal.ZERO;           // txtFreightAmountHeader
        public BigDecimal otherWages = BigDecimal.ZERO;       // txtOtherWagesHeader
        public BigDecimal settle = BigDecimal.ZERO;            // txtSettlementDiscountHeader
        public BigDecimal commission = BigDecimal.ZERO;        // txtcommamount
        public BigDecimal exchange = BigDecimal.ZERO;          // txtExchangeRate
        public boolean settleAccountSet;                       // cmbSettlementDiscountAccount.Value > 0
        public Boolean creditAmountInItemSaleGl;               // CreditAmountInItemSaleGL configuration (null = the configuration row does not exist)
        public int supplierId, agentId;                        // comsupplier.Value, combcommAgent.Value
        /** SupplierCustomer id the transporter / labour / settlement account belongs to (GetSupplierCustomerInfoByGLAccountId), 0 = none; -1 = no account selected */
        public int transporterParty = -1, wagesParty = -1, settleParty = -1;
    }

    /** BillAmount() output. */
    public static final class Totals {
        public BigDecimal itemAmount = BigDecimal.ZERO, discount = BigDecimal.ZERO, itemNet = BigDecimal.ZERO;
        public BigDecimal billWithoutCommission = BigDecimal.ZERO, bill = BigDecimal.ZERO, commNet = BigDecimal.ZERO;
        public BigDecimal qty = BigDecimal.ZERO, weight = BigDecimal.ZERO, fcy = BigDecimal.ZERO;
    }

    private static BigDecimal portion(BigDecimal total, BigDecimal totalQty, BigDecimal qty, Fmt f) {
        if (totalQty.signum() == 0) throw new IllegalArgumentException("Attempted to divide by zero.");
        return rnd(total.divide(totalQty, DEC).multiply(qty), f.amtRound);
    }

    /** OtherItemsProportion / WagesProportion / SettlementDiscountProportion / FreightHeaderProportion / GridItemNetAmountCalculate / txtExchangeRate_TextChanged. */
    public static void chain(List<Row> rows, Head h, Fmt f) {
        BigDecimal q = BigDecimal.ZERO;
        for (Row r : rows) q = q.add(r.qty);
        for (Row r : rows) {
            r.wages = portion(h.wagesTotal, q, r.qty, f);
            r.settle = h.settle.signum() > 0 ? portion(h.settle, q, r.qty, f) : BigDecimal.ZERO;
            r.expense = portion(h.expenseTotal, q, r.qty, f);
            r.freight = h.freight.signum() > 0 ? portion(h.freight, q, r.qty, f) : BigDecimal.ZERO;
        }
        gridNet(rows, h, f);
        for (Row r : rows)
            r.fcy = h.exchange.signum() > 0 ? r.withDisc.divide(h.exchange, DEC) : BigDecimal.ZERO;
    }

    /** GridItemNetAmountCalculate. */
    public static void gridNet(List<Row> rows, Head h, Fmt f) {
        for (Row r : rows) {
            BigDecimal wd = rnd(r.withDisc, f.amtRound), sd = rnd(r.settle, f.amtRound), ex = rnd(r.expense, f.amtRound);
            if (h.creditAmountInItemSaleGl != null) {
                if (!h.creditAmountInItemSaleGl) r.netAmount = wd;
                else if (h.settleAccountSet) r.netAmount = rnd(wd, f.amtRound);
                else r.netAmount = rnd(wd.subtract(sd), f.amtRound);
            } else if (h.settleAccountSet) r.netAmount = rnd(wd.add(ex), f.amtRound);
            else r.netAmount = rnd(wd.add(ex).subtract(sd), f.amtRound);
        }
    }

    /** BillAmount(): the transporter / labour amount is subtracted when its account is the customer's own account, otherwise added; the settlement discount is always subtracted. */
    public static Totals bill(List<Row> rows, BigDecimal journalDebit, BigDecimal journalCredit, Head h, Fmt f) {
        Totals t = new Totals();
        if (rows.isEmpty()) return t;
        BigDecimal qty = BigDecimal.ZERO, wt = BigDecimal.ZERO, ia = BigDecimal.ZERO, da = BigDecimal.ZERO, wd = BigDecimal.ZERO;
        for (Row r : rows) {
            qty = qty.add(r.qty); wt = wt.add(r.netWeight);
            if (!r.foc) { ia = ia.add(r.itemAmount); da = da.add(r.discAmount); wd = wd.add(r.withDisc); }
        }
        BigDecimal exp = rnd(h.expenseTotal, f.amtRound), jd = rnd(journalDebit, f.amtRound), jc = rnd(journalCredit, f.amtRound);
        BigDecimal bill = wd.add(exp).add(jc).subtract(jd);
        t.itemAmount = amt(rnd(ia, f.amtRound), f);
        t.discount = amt(da, f);
        t.itemNet = amt(wd, f);
        bill = h.transporterParty > 0 && h.supplierId == h.transporterParty ? bill.subtract(h.freight) : bill.add(h.freight);
        bill = h.wagesParty > 0 && h.supplierId == h.wagesParty ? bill.subtract(h.otherWages) : bill.add(h.otherWages);
        t.billWithoutCommission = amt(bill, f);
        if (h.agentId > 0 && h.commission.signum() > 0 && h.supplierId == h.agentId) { bill = bill.subtract(h.commission); t.commNet = h.commission; }
        if (h.settleAccountSet && h.settleParty > 0 && h.supplierId == h.settleParty)
            throw new IllegalArgumentException("SettlementDiscountAccount can not be Equal to customer Account");
        bill = bill.subtract(h.settle);
        t.bill = amt(bill, f);
        t.qty = rnd(qty, 2);
        t.weight = rnd(wt, 2);
        t.fcy = h.exchange.signum() > 0 && wd.signum() > 0 ? rnd(wd.divide(h.exchange, DEC), f.fcyPlaces) : rnd(BigDecimal.ZERO, f.fcyPlaces);
        return t;
    }
}
