package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Pages of screens 853 frmCashReceiptVoucherTax and 854 frmBankReceiptVoucherTax (desktop class
 * VouchersWithTax.ReceiptsVoucherNew, opened by its Tag). The literal routes win over the generic
 * /accounts/vouchers/{voucherType} mapping.
 */
@Controller
public class ReceiptTaxVoucherPageController {

    @GetMapping("/accounts/vouchers/receipt-tax-cash")
    public String cashReceiptVoucherTax() {
        return "accounts/vouchers/receipt_tax_voucher_cash";
    }

    @GetMapping("/accounts/vouchers/receipt-tax-bank")
    public String bankReceiptVoucherTax() {
        return "accounts/vouchers/receipt_tax_voucher_bank";
    }
}
