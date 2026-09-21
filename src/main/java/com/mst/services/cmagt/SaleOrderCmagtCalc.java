package com.mst.services.cmagt;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;

/**
 * The Sale Order / Deal With Buyer line arithmetic, as pure functions.
 *
 * Extracted from the service so the rules can be tested directly, without a database, and so
 * the same verified arithmetic can be reused by the other Commission Trading screens later
 * instead of being retyped.
 *
 * Every rule below comes from Architecture.WinApp.Cmagt/frmSaleOrderCmagt.cs:
 *
 *   CalculateWeight()      Weight = Qty * packEquivalent
 *   CalculateAmount()      Amount = (Weight > 0 && rateEquivalent > 0)
 *                                   ? Weight / rateEquivalent * Rate
 *                                   : 0
 *   CalculateTaxAmount()   TaxAmount   = Math.Round(Amount * TaxPercent / 100, 3, AwayFromZero)
 *                          TotalAmount = Math.Round(Amount + TaxAmount,        3, AwayFromZero)
 *
 * The conversion factor is the UOM schedule's **Equivalent** column - traced to
 * CommonServices.dtUomFromGloablUomScheduleByItemId, which builds the DataTable the desktop
 * combos bind to as (Id, UOMCode, Equivalent, BaseRateUom, BasePackUom, BaseSecondaryUom), so
 * the desktop's positional SelectedRow.Cells[2] is that column. It is NOT QtyEquivalent, and
 * it is NOT the UOM's own Id - the foreign key and the numeric factor are different things and
 * are kept separate throughout.
 *
 * Precision: the desktop writes each result through ToString("#,##0.###") and
 * FillCustomerDetailRow re-parses that formatted text, so every persisted figure carries at
 * most 3 decimals. MidpointRounding.AwayFromZero is HALF_UP for these non-negative values.
 */
public final class SaleOrderCmagtCalc {

    /** The desktop's "#,##0.###" - three decimals on every stored figure. */
    public static final int SCALE = 3;
    public static final RoundingMode ROUNDING = RoundingMode.HALF_UP;

    private SaleOrderCmagtCalc() {
    }

    /** Weight = Qty * packEquivalent. MULTIPLY - the pack factor scales quantity up to weight. */
    public static BigDecimal weight(BigDecimal qty, BigDecimal packEquivalent) {
        return nz(qty).multiply(nz(packEquivalent)).setScale(SCALE, ROUNDING);
    }

    /**
     * Amount = Weight / rateEquivalent * Rate. DIVIDE first - the rate is quoted per
     * rateEquivalent units of weight, so weight is converted into rate-units before pricing.
     *
     * The zero guard is the desktop's own: a zero weight or a zero factor yields a zero amount
     * rather than an error. Callers must have already established that the factor is available;
     * an unavailable factor is rejected upstream and never arrives here as a zero.
     */
    public static BigDecimal amount(BigDecimal weight, BigDecimal rateEquivalent, BigDecimal rate) {
        BigDecimal w = nz(weight);
        BigDecimal eq = nz(rateEquivalent);
        if (w.signum() <= 0 || eq.signum() <= 0) return BigDecimal.ZERO.setScale(SCALE);
        return w.divide(eq, MathContext.DECIMAL128).multiply(nz(rate)).setScale(SCALE, ROUNDING);
    }

    /** TaxAmount = Amount * TaxPercent / 100. */
    public static BigDecimal taxAmount(BigDecimal amount, BigDecimal taxPercent) {
        return nz(amount).multiply(nz(taxPercent))
                .divide(new BigDecimal("100"), MathContext.DECIMAL128)
                .setScale(SCALE, ROUNDING);
    }

    /**
     * The grid column captioned "Tax + Amount" (DetailGridCommonSetting sets that caption on
     * TotalAmount). The formula really is Amount + TaxAmount - confirmed at three independent
     * sites: CalculateTaxAmount, FillCustomerDetailRow and
     * FillDetailFromListCommonForReadById. The caption alone was not treated as evidence.
     */
    public static BigDecimal totalAmount(BigDecimal amount, BigDecimal taxAmount) {
        return nz(amount).add(nz(taxAmount)).setScale(SCALE, ROUNDING);
    }

    private static BigDecimal nz(BigDecimal v) {
        return v == null ? BigDecimal.ZERO : v;
    }
}
