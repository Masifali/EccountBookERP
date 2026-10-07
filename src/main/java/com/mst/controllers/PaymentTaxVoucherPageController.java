package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Pages of screens 850 PaymentVoucherWithTax, 851 frmBankPaymentVoucherTax and 852 frmCashPaymentVoucherTax
 * (desktop class VouchersWithTax.PaymentVoucherNew, opened by its Tag). The literal routes win over the
 * generic /accounts/vouchers/{voucherType} mapping.
 */
@Controller
public class PaymentTaxVoucherPageController {

    @GetMapping("/accounts/vouchers/payment-tax")
    public String paymentVoucherWithTax() {
        return "accounts/vouchers/payment_tax_voucher";
    }

    @GetMapping("/accounts/vouchers/payment-tax-bank")
    public String bankPaymentVoucherTax() {
        return "accounts/vouchers/payment_tax_voucher_bank";
    }

    @GetMapping("/accounts/vouchers/payment-tax-cash")
    public String cashPaymentVoucherTax() {
        return "accounts/vouchers/payment_tax_voucher_cash";
    }
}
