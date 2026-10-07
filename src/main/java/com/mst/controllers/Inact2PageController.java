package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;

/** Inactive Account_Reports group K: the four pages. Each is a new template; no existing route or template is touched. */
@Controller
public class Inact2PageController {
    /** 54 GeneralLedgerStatment ("General Ledger Statement"). */
    @GetMapping("/accounts/reports/general-ledger-statement-desktop")
    public String generalLedgerStatement() { return "accounts/reports/inact2_general_ledger_statement"; }

    /** 55 AccountsBalanceSheetStandardRpt ("Account Balance Sheet"). */
    @GetMapping("/accounts/reports/balance-sheet-standard")
    public String balanceSheetStandard() { return "accounts/reports/inact2_balance_sheet_standard"; }

    /** 57 GeneralJournalSummeryRegister. */
    @GetMapping("/accounts/reports/general-journal-summary-register")
    public String generalJournalSummary() { return "accounts/reports/inact2_general_journal_summary"; }

    /** 58 PayablesAging (ScreenName tag "PayablesAging": "Accounts Payables Aging"). */
    @GetMapping("/accounts/reports/payables-aging-ledger")
    public String payablesAging() { return "accounts/reports/inact2_payables_aging"; }

    /** 59 ReceivableAging ("Accounts Receivable Aging"). */
    @GetMapping("/accounts/reports/receivable-aging-company")
    public String receivableAging() { return "accounts/reports/inact2_receivable_aging"; }
}
