package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/** R3 account reports - the four new desktop-faithful pages (screens 60, 67, 85, 86). */
@Controller
public class AcRpt3PageController {
    /** Screen 60 FcyGeneralLedgerRpt - "Fcy General Ledger". Opened with ?accountId=&fromDate=&toDate= by the FCY payables / receivables report. */
    @GetMapping("/accounts/reports/fcy-general-ledger")
    public String fcyGeneralLedger() { return "accounts/reports/acrpt3_fcy_general_ledger"; }

    /** Screen 85 FcyBankCharges_Register - "Fcy Bank Charges Register". */
    @GetMapping("/accounts/reports/fcy-bank-charges-register")
    public String fcyBankChargesRegister() { return "accounts/reports/acrpt3_fcy_bank_charges_register"; }

    /** Screen 86 ProfitLoss - "Profit &amp; Loss 02" (frmProfitAndLoss). */
    @GetMapping("/accounts/reports/profit-and-loss-02")
    public String profitLoss02() { return "accounts/reports/acrpt3_profit_loss"; }

    /** Screen 67 CostomerWiseVoucherSlip - "Customer Voucher Report". */
    @GetMapping("/accounts/reports/customer-voucher-report")
    public String customerVoucherReport() { return "accounts/reports/acrpt3_customer_voucher_report"; }
}
