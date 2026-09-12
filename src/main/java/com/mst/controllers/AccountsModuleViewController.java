package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;

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
				model.addAttribute("moduleTitle", "Contra Voucher");
				return "accounts/vouchers/contra_voucher";
			case "expense":
				model.addAttribute("moduleTitle", "Expense Voucher");
				return "accounts/vouchers/expense_voucher";
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
				model.addAttribute("moduleTitle", "Payment Adjustment Voucher");
				return "accounts/vouchers/invoices_adjustment_voucher";
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
			case "contractor-wages":
				model.addAttribute("moduleTitle", "Contractor Wages Account");
				return "accounts/vouchers/contractor_wages";
			case "voucher-validation":
				model.addAttribute("moduleTitle", "Voucher Validation Report");
				model.addAttribute("accountsList", voucherValidationService.getAllDetailAccounts());
				model.addAttribute("docTypesList", voucherValidationService.getDocumentTypes());
				model.addAttribute("customGroupsList", voucherValidationService.getCustomGroups());
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
	public String getReportModule(@PathVariable("reportType") String reportType, Model model) {
		model.addAttribute("activeMenu", "accounts");
		model.addAttribute("accountsList", accountsReportService.getAllDetailAccounts());
		model.addAttribute("customGroupsList", accountsReportService.getCustomGroups());
		model.addAttribute("citiesList", accountsReportService.getCities());
		model.addAttribute("costCentersList", accountsReportService.getCostCenters());
		model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
		model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
		model.addAttribute("languagesList", accountsReportService.getLanguages());
		model.addAttribute("dateTypesList", accountsReportService.getDateTypes());

		String normalized = reportType.toLowerCase().trim();

		switch (normalized) {
			case "activity-summary":
			case "accounts-activity-summary":
				model.addAttribute("moduleTitle", "Accounts Activity Summary Report");
				return "accounts/reports/activity_summary";
			case "general-ledger-statement":
				model.addAttribute("moduleTitle", "General Ledger Statement");
				return "accounts/reports/general_ledger_statement";
			case "general-ledger":
			case "general_ledger":
				model.addAttribute("moduleTitle", "General Ledger");
				model.addAttribute("costCentersList", accountsReportService.getCostCenters());
				model.addAttribute("subsidiaryAccountsList", accountsReportService.getSubsidiaryAccounts(null));
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				model.addAttribute("languagesList", accountsReportService.getLanguages());
				model.addAttribute("dateTypesList", accountsReportService.getDateTypes());
				return "accounts/reports/general_ledger";
			case "customer-ledger":
			case "customer_ledger":
				model.addAttribute("moduleTitle", "Customer Ledger Report");
				model.addAttribute("supplierCustomersList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("dateTypesList", accountsReportService.getDateTypes());
				return "accounts/reports/customer_ledger";
			case "day-book":
				model.addAttribute("moduleTitle", "Day Book");
				model.addAttribute("accountsList", accountsReportService.getBankAccounts());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				model.addAttribute("costCentersList", accountsReportService.getCostCenters());
				return "accounts/reports/day_book";
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
				return "accounts/reports/selected_trial_balance";
			case "trial-balances-all-level":
				model.addAttribute("moduleTitle", "Trial Balances All Level");
				return "accounts/reports/trial_balances_all_level";
			case "trial-balance":
				model.addAttribute("moduleTitle", "Trial Balance");
				model.addAttribute("documentTypesList", accountsReportService.getDocumentTypesForReports());
				model.addAttribute("accountGroupsList", accountsReportService.getAccountGroups());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				return "accounts/reports/trial_balance";
			case "payables-aging":
				model.addAttribute("moduleTitle", "Payables Aging");
				return "accounts/reports/payables_aging";
			case "receivables-aging":
				model.addAttribute("moduleTitle", "Receivables Aging");
				return "accounts/reports/receivables_aging";
			case "payables-report":
				model.addAttribute("moduleTitle", "Payables Report");
				return "accounts/reports/payables_report";
			case "receivables-report":
				model.addAttribute("moduleTitle", "Receivables Report");
				return "accounts/reports/receivables_report";
			case "voucher-report":
				model.addAttribute("moduleTitle", "Voucher Report");
				model.addAttribute("docTypesList", voucherValidationService.getDocumentTypes());
				return "accounts/reports/voucher_report";
			case "bank-balances":
				model.addAttribute("moduleTitle", "Bank Balances");
				return "accounts/reports/bank_balances";
			case "pdc-register":
				model.addAttribute("moduleTitle", "Post Dated Cheque Register");
				return "accounts/reports/pdc_register";
			case "pdc-information":
				model.addAttribute("moduleTitle", "Post Dated Cheque Information");
				return "accounts/reports/pdc_information";
			case "payables-aging-new":
				model.addAttribute("moduleTitle", "Payables Aging New");
				return "accounts/reports/payables_aging_new";
			case "receivables-aging-new":
				model.addAttribute("moduleTitle", "Receivables Aging New");
				return "accounts/reports/receivables_aging_new";
			case "payables-report-invoice-wise":
				model.addAttribute("moduleTitle", "Payables Report Invoice Wise");
				return "accounts/reports/payables_report_invoice_wise";
			case "receivables-by-due-dates":
				model.addAttribute("moduleTitle", "Receivables By Due Dates");
				return "accounts/reports/receivables_by_due_dates";
			case "payables-payment-schedule":
				model.addAttribute("moduleTitle", "Payables And Payment Schedule");
				return "accounts/reports/payables_payment_schedule";
			case "receivables-receipt-schedule":
				model.addAttribute("moduleTitle", "Receivables And Receipt Schedule");
				return "accounts/reports/receivables_receipt_schedule";
			case "bills-payables-report":
				model.addAttribute("moduleTitle", "Bills Payables Report");
				return "accounts/reports/bills_payables_report";
			case "party-limits-balances":
				model.addAttribute("moduleTitle", "Party Limits & Balances");
				return "accounts/reports/party_limits_balances";
			case "subsidiary-payables-report":
				model.addAttribute("moduleTitle", "Subsidiary Payables Report");
				return "accounts/reports/subsidiary_payables_report";
			case "chart-of-accounts-report":
				model.addAttribute("moduleTitle", "Chart of Accounts Report");
				return "accounts/reports/chart_of_accounts_report";
			case "account-notes-report":
				model.addAttribute("moduleTitle", "Account Notes Report");
				return "accounts/reports/account_notes_report";
			case "wages-report":
			case "wages_report":
				model.addAttribute("moduleTitle", "Wages Report");
				model.addAttribute("contractorsList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("costCentersList", accountsReportService.getCostCenters());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				model.addAttribute("dateTypesList", accountsReportService.getDateTypes());
				return "accounts/reports/wages_report";
			case "wages-report-activities":
			case "wages_report_activities":
				model.addAttribute("moduleTitle", "Wages Report (With Activities)");
				model.addAttribute("contractorsList", accountsReportService.getSupplierCustomersForCombo());
				model.addAttribute("costCentersList", accountsReportService.getCostCenters());
				model.addAttribute("branchesList", accountsReportService.getBranchesForReports());
				model.addAttribute("dateTypesList", accountsReportService.getDateTypes());
				return "accounts/reports/wages_report_activities";
			default:
				model.addAttribute("moduleTitle", formatTitle(reportType));
				return "accounts/reports/general_ledger";
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
				model.addAttribute("moduleTitle", "Bank Reconciliation With Vouchers");
				return "accounts/banking/bank_reconciliation";
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
