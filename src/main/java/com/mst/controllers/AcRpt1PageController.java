package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/** Group R1: the four pages. Each is a new template; no existing route or template is touched. */
@Controller
public class AcRpt1PageController {
    /** 48 Payables = Architecture.WinApp.Account_Reports.Payables ("Accounts Payables Balance Classification Wise"). */
    @GetMapping("/accounts/reports/payables-balance-classification")
    public String payables() { return "accounts/reports/acrpt1_payables"; }

    /** 63 PayablesByDueDate ("Accounts Payables By Due Date"). */
    @GetMapping("/accounts/reports/payables-by-due-date")
    public String payablesByDueDate() { return "accounts/reports/acrpt1_payables_by_due_date"; }

    /** 70 ReceiveablesByDueDate ("Accounts Receivables By Due Date"). */
    @GetMapping("/accounts/reports/receivables-by-due-date")
    public String receivablesByDueDate() { return "accounts/reports/acrpt1_receivables_by_due_date"; }

    /** 68 ReceivablesByDueDatesNew ("Receivables By Due Date"). */
    @GetMapping("/accounts/reports/receivables-by-due-dates-new")
    public String receivablesByDueDatesNew() { return "accounts/reports/acrpt1_receivables_by_due_dates_new"; }
}
