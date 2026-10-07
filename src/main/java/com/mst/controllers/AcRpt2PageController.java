package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/** R2 account reports - the four new desktop-faithful pages (screens 72, 75, 77, 870). */
@Controller
public class AcRpt2PageController {
    /** Screen 72 FCYPayablesAndReceivablesRpt - "Export Payables & Receivables (Accounts)". */
    @GetMapping("/accounts/reports/fcy-payables-receivables")
    public String fcy() { return "accounts/reports/acrpt2_fcy_payables_receivables"; }

    /** Screen 75 DueDateAnalysisPayablesAndReceivablesForcast - "1008 Payables & Receivable Forecast". */
    @GetMapping("/accounts/reports/due-date-analysis-forecast")
    public String forecast() { return "accounts/reports/acrpt2_due_date_forecast"; }

    /** Screen 77 PayablesandReceivablesAging - "Payables and Receivables Aging Report". */
    @GetMapping("/accounts/reports/payables-receivables-aging-report")
    public String aging() { return "accounts/reports/acrpt2_payables_receivables_aging"; }

    /** Screen 870 frmPayablesAndReceivablesWithPaymentAndReceipts. */
    @GetMapping("/accounts/reports/payables-receivables-with-payment-receipts")
    public String withPaymentReceipts() { return "accounts/reports/acrpt2_payables_receivables_payment_receipts"; }
}
