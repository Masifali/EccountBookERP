package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Page of screen 862 ExpenseVoucherNew (Expense Voucher New). The literal route wins over the generic
 * /accounts/vouchers/{voucherType} mapping.
 */
@Controller
public class ExpenseTaxVoucherPageController {

    @GetMapping("/accounts/vouchers/expense-tax")
    public String expenseTax() {
        return "accounts/vouchers/expense_tax_voucher";
    }
}
