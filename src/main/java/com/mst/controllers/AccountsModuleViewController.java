package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

/**
 * Controller handling view routing for Accounts Dashboard and Submodule navigation:
 * - 8 Account Definition Modules
 * - 18 Accounts Transaction Modules (dedicated voucher templates)
 * - 27 Account Reports Modules (dedicated report templates)
 * - 7 Banking Management Modules (dedicated banking templates)
 */
@Controller
@RequestMapping("/accounts")
public class AccountsModuleViewController {

	@org.springframework.beans.factory.annotation.Autowired
	private com.mst.services.VoucherValidationService voucherValidationService;

	@org.springframework.beans.factory.annotation.Autowired
	private com.mst.services.PartyPaymentVoucherService partyPaymentVoucherService;

	@org.springframework.beans.factory.annotation.Autowired
	private com.mst.services.AccountsReportService accountsReportService;

	/* Account Reports recheck (group F): desktop-exact procedures for screens 79/49/51/81/82/53/83/52/69/73/74. */
	@org.springframework.beans.factory.annotation.Autowired
	private com.mst.services.AccountReportsDesktopService accountReportsDesktopService;

	@org.springframework.beans.factory.annotation.Autowired
	private org.springframework.jdbc.core.JdbcTemplate jdbcTemplate;

	@GetMapping({"", "/", "/dashboard"})
	public String accountsDashboard(Model model) {
		model.addAttribute("activeMenu", "accounts");
		return "accounts/accounts_dashboard";
	}

	@GetMapping("/bank-reconciliation-upload-excel")
	public String bankReconciliationUploadExcel(Model model) {
		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("moduleTitle", "Bank Reconciliation Upload Excel");
		return "accounts/bank_reconciliation_upload_excel";
	}

	@GetMapping({"/bank-balance-manual-entry", "/reports/bank-balance-manual-entry", "/banking/bank-balance-manual-entry"})
	public String bankBalanceManualEntry(Model model) {
		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("moduleTitle", "Bank Balance Manual Entry");
		model.addAttribute("bankAccountsList", accountsReportService.getBankAccounts());
		return "accounts/banking/bank_balance_manual_entry";
	}

	// ==========================================
	// 2. ACCOUNTS TRANSACTION MODULES (18)
	// ==========================================

	@GetMapping("/vouchers/{voucherType}")
	public String getVoucherModule(@PathVariable("voucherType") String voucherType, Model model) {
		model.addAttribute("activeMenu", "accounts");
		String normalized = voucherType.toLowerCase().trim();

		switch (normalized) {
			case "cash-payment":
				model.addAttribute("moduleTitle", "Cash Payment Voucher");
				return "accounts/vouchers/cash_payment_voucher";
			case "bank-payment":
				model.addAttribute("moduleTitle", "Bank Payment Voucher");
				return "accounts/vouchers/bank_payment_voucher";
			case "cash-receipt":
				model.addAttribute("moduleTitle", "Cash Receipt Voucher");
				return "accounts/vouchers/cash_receipt_voucher";
			case "bank-receipt":
				model.addAttribute("moduleTitle", "Bank Receipt Voucher");
				return "accounts/vouchers/bank_receipt_voucher";
			case "journal":
				model.addAttribute("moduleTitle", "Journal Voucher");
				return "accounts/vouchers/journal_voucher";
			case "contra":
				/* Screen 22 - Account_Definition/ContraVoucher.cs (2026-09-30 recheck: the page is rebuilt on
				   the desktop form and saves through /accounts/api/contra/save; contra_voucher.html is kept
				   unreferenced for rollback). */
				model.addAttribute("moduleTitle", "Contra Voucher");
				return "accounts/vouchers/contra_voucher_desktop";
			case "voucher-entry":
				/* Screen 19 "Jounal Voucher" - Account_Definition/VoucherEntry.cs (ScreenDefinition 19's
				   TargetUrl). "journal" above stays the VouchersWithTax.JournalVoucher page (screen 860). */
				model.addAttribute("moduleTitle", "Journal Voucher");
				return "accounts/vouchers/journal_voucher_entry";
			case "contra-tax":
				/* Screen 855 "Contra Voucher New" - Account_Definition/VouchersWithTax/ContraVoucher.cs
				   (ScreenDefinition 855 ContraVoucherTax; CommonServices.OpenDocument doc 10 base 3).
				   Saves through /accounts/api/desktop-voucher/contra-tax/save. */
				model.addAttribute("moduleTitle", "Contra Voucher New");
				return "accounts/vouchers/contra_voucher_tax";
			case "expense":
				/* Screen 46 - Account_Definition/ExpenseVoucher.cs, the plain form ScreenDefinition 46 opens.
				   expense_voucher.html was built from VouchersWithTax/ExpenseVoucherNew.cs (screen 862) and is
				   kept unreferenced. */
				model.addAttribute("moduleTitle", "Expense Voucher");
				return "accounts/vouchers/expense_voucher_desktop";
			case "party-receipt":
				model.addAttribute("moduleTitle", "Party Receipt Voucher");
				return "accounts/vouchers/party_receipt_voucher";
			case "party-payment":
				model.addAttribute("moduleTitle", "Party Payment Voucher");
				model.addAttribute("drAccountsList", partyPaymentVoucherService.getDrAccounts());
				model.addAttribute("jobLotsList", partyPaymentVoucherService.getJobLots());
				int nextCode = partyPaymentVoucherService.generateNextVoucherCode();
				model.addAttribute("nextVoucherCode", nextCode);
				model.addAttribute("nextVoucherDisplay", String.valueOf(nextCode));
				return "accounts/vouchers/party_payment_voucher";
			case "pdc-payment":
				model.addAttribute("moduleTitle", "Post Dated Cheque Payment Vouchers");
				return "accounts/vouchers/pdc_payment_voucher";
			case "payment-by-invoice":
				model.addAttribute("moduleTitle", "Payment By Invoice Voucher");
				return "accounts/vouchers/payment_by_invoice_voucher";
			case "invoices-adjustment":
				/* Screen 861 frmInvoicesAdjustmentVoucher (TransactionTypeId 1) - one template with
				   screen 863 below; InvoicesAdjustmentVoucherController serves both. */
				model.addAttribute("moduleTitle", "Payment Adjustment Voucher");
				model.addAttribute("adjKind", "payment");
				model.addAttribute("adjTitle", "Payment Adjustment Voucher");
				model.addAttribute("adjSlipCaption", "901-Print");
				model.addAttribute("adjSlipName", "901_VoucherInvoicesAdjustment_Slip");
				model.addAttribute("adjAutoCaption", "Auto Utilize Voucher Amount");
				return "accounts/vouchers/adjustment_voucher";
			case "receipt-invoices-adjustment":
				/* Screen 863 frmReceiptInvoicesAdjustmentVoucher (TransactionTypeId 2). */
				model.addAttribute("moduleTitle", "Receipt Adjustment Voucher");
				model.addAttribute("adjKind", "receipt");
				model.addAttribute("adjTitle", "Receipt Adjustment Voucher");
				model.addAttribute("adjSlipCaption", "901_01-Print");
				model.addAttribute("adjSlipName", "901_01_VoucherInvoicesAdjustment_Slip");
				model.addAttribute("adjAutoCaption", "Auto Utilize");
				return "accounts/vouchers/adjustment_voucher";
			case "day-book":
				/* Screen 15 DayBook.cs (DocumentTypeId 9) - DayBookVoucherController. */
				model.addAttribute("moduleTitle", "Day Book");
				return "accounts/vouchers/day_book_voucher";
			case "day-book-offset":
				/* Screen 24 frmDayBook.cs "Day Book (Off Set)" (DocumentTypeId 8). */
				model.addAttribute("moduleTitle", "Day Book (Off Set)");
				return "accounts/vouchers/day_book_offset";
			case "freight":
				model.addAttribute("moduleTitle", "Freight Voucher");
				return "accounts/vouchers/freight_voucher";
			case "day-book-approval":
				model.addAttribute("moduleTitle", "Day Book Approval");
				try {
					String sql = "SELECT Id as id, AccountTitle as accountTitle, AccountCode as accountCode FROM ChartofAccount WHERE (AccountGroup = 'Detail' OR Account_Level >= 4) ORDER BY AccountTitle ASC";
					model.addAttribute("cashAccountsList", jdbcTemplate.queryForList(sql));
				} catch (Exception e) {}
				return "accounts/vouchers/day_book_approval";
			case "pdc-transaction-payment":
				model.addAttribute("moduleTitle", "PDC Transaction Payment");
				return "accounts/vouchers/pdc_transaction_payment";
			case "advance-adjustment":
				model.addAttribute("moduleTitle", "Advance Adjustment");
				return "accounts/vouchers/advance_adjustment";
			case "contractor-wages-dashboard":
				model.addAttribute("moduleTitle", "Contractor Wages Dashboard");
				return "accounts/vouchers/contractor_wages_dashboard";
			/* Contractor Wages.
			 *
			 * These five submodules are five DIFFERENT desktop forms in
			 * Architecture.WinApp.Contractor_Wages. They all used to return
			 * "accounts/vouchers/contractor_wages", which is a 96%-identical copy of
			 * contra_voucher.html: it posts documentTypeId 10 to the voucher save endpoint, so
			 * pressing Save on any of them wrote a CONTRA VOUCHER, not a wages record. The
			 * submoduleKey attribute they each set was never read by that template, so all five
			 * URLs rendered the same page under five different headings.
			 *
			 * Wages Account is now built from its own desktop form. The other four return a page
			 * that names the form each still needs, so no URL silently shows an unrelated screen
			 * and no one can save the wrong document from them. */
			case "contractor-wages":
			case "wages-account":
				model.addAttribute("moduleTitle", "Wages Account");
				return "accounts/vouchers/contractor_wages_account";
			/* Built from frmContractWagesSchedule.cs - see ContractorWagesScheduleService for the
			 * procedure contract (Sp_InvContractorWagesSchedule_Insert / _Update / _GetAllMethod). */
			case "wages-rate-schedule":
				model.addAttribute("moduleTitle", "Wages Rate Schedule");
				return "accounts/vouchers/contractor_wages_schedule";
			/* Built from frmContractWiseWagesSchedule.cs - same model and procedures as the plain
			 * schedule, plus ContractorId and its own Company Rate box. */
			case "wages-rate-schedule-contractor":
				model.addAttribute("moduleTitle", "Wages Rate Schedule Contractor Wise");
				return "accounts/vouchers/contractor_wise_wages_schedule";
			/* Built from frmwagesBillHeader.cs (5,407 lines) - Contractor Wages Bill,
			 * DocumentTypeId 101. Procedure contract in LabourWagesService; the voucher
			 * posting, audit copies and stock-evaluation call in ContractorWagesBillWriter. */
			case "labour-wages":
				model.addAttribute("moduleTitle", "Labour Wages");
				return "accounts/vouchers/labour_wages";
			case "labour-wages-manual":
				/* frmWagesBillManual, screen 187, DocumentTypeId 810 - LabourWagesManualService. */
				model.addAttribute("moduleTitle", "Labour Wages Manual");
				return "accounts/vouchers/labour_wages_manual";
			case "voucher-validation":
				model.addAttribute("moduleTitle", "Voucher Validation Report");
				model.addAttribute("accountsList", voucherValidationService.getAllDetailAccounts());
				model.addAttribute("docTypesList", voucherValidationService.getDocumentTypes());
				model.addAttribute("customGroupsList", voucherValidationService.getCustomGroups());
				model.addAttribute("languagesList", accountReportsDesktopService.languages());
				return "accounts/vouchers/voucher_validation";
			default:
				model.addAttribute("moduleTitle", formatTitle(voucherType) + " Voucher");
				return "accounts/vouchers/cash_payment_voucher";
		}
	}

	// ==========================================
	// 3. ACCOUNT REPORTS MODULES (27)
	// ==========================================

	@GetMapping({"/reports/{reportType}", "/accounts/reports/{reportType}"})
	public String getReportModule(
			@PathVariable("reportType") String reportType,
			@RequestParam(name = "accountId", required = false) Integer accountId,
			@RequestParam(name = "fromDate", required = false) String fromDate,
			@RequestParam(name = "toDate", required = false) String toDate,
			Model model) {
		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("selectedAccountId", accountId);
		model.addAttribute("selectedFromDate", fromDate);
		model.addAttribute("selectedToDate", toDate);
        // The ledger template binds only these two lookups. Avoid loading unrelated
        // report filters (and loading the same filters again in the switch below).
        String normalized = reportType.toLowerCase().trim();
        if (normalized.equals("general-ledger") || normalized.equals("general_ledger")) {
            model.addAttribute("moduleTitle", "General Ledger");
            model.addAttribute("accountsList", accountsReportService.getAllDetailAccounts());
            model.addAttribute("dateTypesList", accountsReportService.getDateTypes());
            return "accounts/reports/general_ledger_desktop";
        }
        if (normalized.equals("general-ledger-multi")) {
            model.addAttribute("moduleTitle", "General Ledger");
            model.addAttribute("accountsList", accountsReportService.getAllDetailAccounts());
            model.addAttribute("dateTypesList", accountsReportService.getDateTypes());
            return "accounts/reports/general_ledger";
        }
        /* group F: pages that load every lookup themselves from /accounts/api/reports-desktop */
        switch (normalized) {
            case "selected-trial-balance":
                model.addAttribute("moduleTitle", "Trial Balance Selected");
                return "accounts/reports/selected_trial_balance_desktop";
            case "activity-summary":
            case "accounts-activity-summary":
                model.addAttribute("moduleTitle", "Accounts Activity Summary Report");
                return "accounts/reports/activity_summary_desktop";
            case "bank-balances":
                model.addAttribute("moduleTitle", "Bank Balances Report");
                return "accounts/reports/bank_balances_desktop";
            case "cash-balances":
            case "cash_balances":
                model.addAttribute("moduleTitle", "Cash Balances Report");
                return "accounts/reports/cash_balances_desktop";
            case "chart-of-accounts-report":
                model.addAttribute("moduleTitle", "Chart of Account");
                return "accounts/reports/chart_of_accounts_desktop";
            case "trial-balances-all-level":
                model.addAttribute("moduleTitle", "Trial Balance All Levels");
                return "accounts/reports/trial_balances_all_level";
            case "customer-ledger":
            case "customer_ledger":
                model.addAttribute("moduleTitle", "Customer Ledger Report");
                model.addAttribute("supplierCustomersList", accountReportsDesktopService.supplierCustomers());
                model.addAttribute("dateTypesList", accountsReportService.getDateTypes());
                return "accounts/reports/customer_ledger";
            case "trial-balance":
                model.addAttribute("moduleTitle", "Trial Balance");
                model.addAttribute("documentTypesList", accountReportsDesktopService.documentTypes());
                model.addAttribute("accountGroupsList", accountReportsDesktopService.accountGroups());
                model.addAttribute("customGroupsList", accountReportsDesktopService.customGroups());
                return "accounts/reports/trial_balance";
            case "voucher-report":
                model.addAttribute("moduleTitle", "Voucher Report");
                model.addAttribute("docTypesList", accountReportsDesktopService.documentTypes());
                model.addAttribute("accountsList", accountReportsDesktopService.detailAccounts(null));
                model.addAttribute("customGroupsList", accountReportsDesktopService.customGroups());
                return "accounts/reports/voucher_report";
            default:
                break;
        }
		model.addAttribute("accountsList", accountsReportService.getAllDetailAccounts());
		model.addAttribute("customGroupsList", accountsReportService.getCustomGroups());
		model.addAttribute("citiesList", accountsReportService.getCities());
		model.addAttribute("costCentersList", accountsReportService.getCostCenters());
		model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
		model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
		model.addAttribute("languagesList", accountsReportService.getLanguages());
		model.addAttribute("dateTypesList", accountsReportService.getDateTypes());


		switch (normalized) {
			case "all-payables":
			case "all_payables":
				model.addAttribute("moduleTitle", "Payables & Receivables Reports");
				return "accounts/reports/all_payables";
			case "activity-summary":
			case "accounts-activity-summary":
				model.addAttribute("moduleTitle", "Accounts Activity Summary Report");
				return "accounts/reports/activity_summary";
			case "general-ledger-statement":
				model.addAttribute("moduleTitle", "General Ledger Statement");
				return "accounts/reports/general_ledger_statement";
			case "customer-ledger":
			case "customer_ledger":
				model.addAttribute("moduleTitle", "Customer Ledger Report");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("dateTypesList", accountsReportService.getDateTypes());
				return "accounts/reports/customer_ledger";
			case "day-book":
				/* Screen 15 "Day Book" is the DayBook.cs voucher-entry form, not a report; the page
				   this route used to render (reports/day_book.html over raw SQL) has no desktop
				   counterpart. Old links land on the real screen. */
				return "redirect:/accounts/vouchers/day-book";
			case "balance-sheet":
				model.addAttribute("moduleTitle", "Balance Sheet");
				return "accounts/reports/balance_sheet";
			case "profit-and-loss":
				model.addAttribute("moduleTitle", "Profit & Loss Statement");
				return "accounts/reports/profit_and_loss";
			case "selected-trial-balance":
				model.addAttribute("moduleTitle", "Selected Trial Balance");
				model.addAttribute("accountGroupsSelectedList", accountsReportService.getAccountGroupsForSelectedTrial());
				model.addAttribute("documentTypesList", accountsReportService.getDocumentTypesForReports());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				model.addAttribute("languagesList", accountsReportService.getLanguages());
				model.addAttribute("customGroupsList", accountsReportService.getCustomGroups());
				model.addAttribute("citiesList", accountsReportService.getCities());
				return "accounts/reports/selected_trial_balance";
			case "trial-balances-all-level":
				model.addAttribute("moduleTitle", "Trial Balances All Level");
				return "accounts/reports/trial_balances_all_level";
			case "trial-balance":
				model.addAttribute("moduleTitle", "Trial Balance");
				model.addAttribute("documentTypesList", accountsReportService.getDocumentTypesForReports());
				model.addAttribute("accountGroupsList", accountsReportService.getAccountGroups());
				model.addAttribute("customGroupsList", accountsReportService.getCustomGroups());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/trial_balance";
			case "payables-aging":
				model.addAttribute("moduleTitle", "Payables Aging");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/payables_new"; /* PayablesAging_New (screen 47), group G 2026-09-30 */
			case "receivables-aging":
				model.addAttribute("moduleTitle", "Receivables Aging");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/receivables_aging";
			case "payables-report":
			case "payables-report-new":
				model.addAttribute("moduleTitle", "Payables Report Accounts Classification Wise");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/payables-report";
			case "receivables-report":
				model.addAttribute("moduleTitle", "Receivables Report");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/receivables_report";
			case "voucher-report":
				model.addAttribute("moduleTitle", "Voucher Report");
				model.addAttribute("docTypesList", voucherValidationService.getDocumentTypes());
				model.addAttribute("accountsList", accountsReportService.getAllDetailAccounts());
				model.addAttribute("customGroupsList", accountsReportService.getCustomGroups());
				return "accounts/reports/voucher_report";
			case "bank-balances":
				model.addAttribute("moduleTitle", "Bank Balances");
				model.addAttribute("accountsList", accountsReportService.getBankAccounts());
				return "accounts/reports/bank_balances";
			case "cash-balances":
			case "cash_balances":
				model.addAttribute("moduleTitle", "Cash Balances");
				model.addAttribute("accountsList", accountsReportService.getCashAccounts());
				return "accounts/reports/cash_balances";
			case "pdc-register":
				model.addAttribute("moduleTitle", "Post Dated Cheque Register");
				return "accounts/reports/pdc_register";
			case "pdc-information":
				model.addAttribute("moduleTitle", "Post Dated Cheque Information");
				model.addAttribute("accountsList", accountsReportService.getAllDetailAccounts());
				return "accounts/reports/pdc_information";
			case "payables-aging-new":
				model.addAttribute("moduleTitle", "Payables Aging New");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/payables_new"; /* PayablesAging_New (screen 47), group G 2026-09-30 */
			case "receivables-aging-new":
				model.addAttribute("moduleTitle", "Receivables Aging New");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/receivables_aging_new";
			case "payables-report-invoice-wise":
				model.addAttribute("moduleTitle", "Payables Report Invoice Wise");
				return "accounts/reports/payables_invoice_wise"; /* frmPayablesReportInvoiceWise (screen 909), group G 2026-09-30 */
			case "receivables-by-due-dates":
				model.addAttribute("moduleTitle", "Receivables By Due Dates");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/receivables_by_due_dates";
			case "payables-payment-schedule":
			case "payables-payment-schedule-new":
				model.addAttribute("moduleTitle", "Payables And Payment Schedule");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				return "dashboard/payables_payment_schedule"; /* PayablesAndPaymentSchedule.cs port (screen 78), group G 2026-09-30 */
			case "receivables-receipt-schedule":
				model.addAttribute("moduleTitle", "Receivables And Receipt Schedule");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				return "accounts/reports/receivables_receipt_schedule";
			case "bills-payables-report":
				model.addAttribute("moduleTitle", "Bills Payables Report");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				return "accounts/reports/bills_payables_report";
			case "party-limits-balances":
				model.addAttribute("moduleTitle", "Party Limits & Balances");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/party_limits_balances";
			case "subsidiary-payables-report":
				model.addAttribute("moduleTitle", "Subsidiary Payables Report");
				model.addAttribute("accountsList", accountsReportService.getAllDetailAccounts());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/subsidiary_payables_report";
			case "chart-of-accounts-report":
				model.addAttribute("moduleTitle", "Chart of Accounts Report");
				model.addAttribute("accountsList", accountsReportService.getAllDetailAccounts());
				return "accounts/reports/chart_of_accounts_report";
			case "account-notes-report":
				model.addAttribute("moduleTitle", "Account Notes Report");
				model.addAttribute("accountsList", accountsReportService.getAllDetailAccounts());
				return "accounts/reports/account_notes_report";
			case "wages-report":
			case "wages_report":
				model.addAttribute("moduleTitle", "Wages Report");
				/* frmStockContractorWagesHistory.cs - every list is loaded by the page from its own API. */
				return "accounts/reports/contractor_wages_history";
			case "wages-report-activities":
			case "wages_report_activities":
				model.addAttribute("moduleTitle", "Wages Report (With Activities)");
				/* The desktop's "Wages Report (With Activities)" menu opens frmEvaulationDetailWagesReports
				   ("Wages Register"), already ported for screen 280 - one page for both entries. */
				return "production/reports/evaluation_detail_wages";
			/* Group H 2026-09-30 - desktop screens 958, 886, 911; each page loads its lists from its own API. */
			case "monthly-profit-loss":
				model.addAttribute("moduleTitle", "Monthly Profit Loss");
				return "accounts/reports/monthly_profit_loss";
			case "commission-agent-report":
				model.addAttribute("moduleTitle", "Commission Agent Report");
				return "accounts/reports/commission_agent_report";
			case "freight-voucher-report":
				model.addAttribute("moduleTitle", "Freight Voucher Report");
				return "accounts/reports/freight_voucher_report";
			default:
				throw new org.springframework.web.server.ResponseStatusException(
						org.springframework.http.HttpStatus.NOT_FOUND, "Report route is not implemented: " + reportType);
		}
	}

	// ==========================================
	// 4. BANKING MANAGEMENT MODULES (7)
	// ==========================================

	@GetMapping("/banking/{bankingType}")
	public String getBankingModule(@PathVariable("bankingType") String bankingType, Model model) {
		model.addAttribute("activeMenu", "accounts");
		try {
			String bankSql = "SELECT Id as id, AccountTitle as accountTitle, AccountCode as accountCode FROM ChartofAccount WHERE (AccountTypeId = 15 OR AccountGroup = 'Detail' OR Account_Level >= 4) ORDER BY AccountTitle ASC";
			model.addAttribute("bankAccountsList", jdbcTemplate.queryForList(bankSql));
			String countrySql = "SELECT Id as id, CountryName as countryName FROM Country ORDER BY CountryName ASC";
			model.addAttribute("countriesList", jdbcTemplate.queryForList(countrySql));
			String citySql = "SELECT Id as id, CityName as cityName FROM City ORDER BY CityName ASC";
			model.addAttribute("citiesList", jdbcTemplate.queryForList(citySql));
			String ccSql = "SELECT Id as id, CostCenterName as costCenterName FROM CostCenter ORDER BY CostCenterName ASC";
			model.addAttribute("costCentersList", jdbcTemplate.queryForList(ccSql));
		} catch (Exception e) {}

		String normalized = bankingType.toLowerCase().trim();

		switch (normalized) {
			case "bank-reconciliation":
				/* Screen 885 frmBankReconciliationWithVouchers - BankReconciliationVouchersController. */
				model.addAttribute("moduleTitle", "Bank Reconciliation With Vouchers");
				return "accounts/banking/bank_reconciliation_vouchers";
			case "bank-detail-definition":
				model.addAttribute("moduleTitle", "Bank Detail Definition");
				return "accounts/banking/bank_detail_definition";
			case "define-bank":
				model.addAttribute("moduleTitle", "Define Bank");
				return "accounts/banking/define_bank";
			case "pdc-management":
				model.addAttribute("moduleTitle", "PDC Management");
				return "accounts/banking/pdc_management";
			case "cheque-printing":
				model.addAttribute("moduleTitle", "Cheque Printing");
				return "accounts/banking/cheque_printing";
			case "bank-charges":
				model.addAttribute("moduleTitle", "Bank Charges");
				return "accounts/banking/bank_charges";
			case "coa-allocate-cost-center":
				model.addAttribute("moduleTitle", "Chart of Account Allocate Cost Center");
				return "accounts/banking/coa_allocate_cost_center";
			default:
				model.addAttribute("moduleTitle", formatTitle(bankingType));
				return "accounts/banking/define_bank";
		}
	}

	private String formatTitle(String slug) {
		if (slug == null || slug.isEmpty()) return "";
		String[] words = slug.split("-");
		StringBuilder sb = new StringBuilder();
		for (String word : words) {
			if (word.length() > 0) {
				sb.append(Character.toUpperCase(word.charAt(0))).append(word.substring(1)).append(" ");
			}
		}
		return sb.toString().trim();
	}
}
