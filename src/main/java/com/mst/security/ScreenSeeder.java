package com.mst.security;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.CommandLineRunner;
import org.springframework.core.annotation.Order;
import org.springframework.stereotype.Component;

import com.mst.models.Screen;
import com.mst.repositories.IScreenRepository;
import lombok.extern.slf4j.Slf4j;

@Slf4j
// Not registered: legacy ERP reference data must not be seeded at startup.
@Order(1)
public class ScreenSeeder implements CommandLineRunner {

	@Autowired
	private IScreenRepository screenRepository;

	@Override
	public void run(String... args) {
		try {
			// ==========================================
			// 1. ACCOUNT DEFINITION (8)
			// ==========================================
			seed("Bank Reconciliation Upload Excel", "Accounts", "/accounts/bank-reconciliation-upload-excel", "BANK_RECON_EXCEL", "ACCOUNTS", null, 1);
			seed("Chart Of Account Definition", "Accounts", "/accounts/chart_of_accounts", "CHART_OF_ACCOUNT", "ACCOUNTS", 9, 1);
			seed("Define Supplier / Customer", "Accounts", "/accounts/supplier", "DEFINE_SUPPLIER", "ACCOUNTS", 11, 1);
			seed("Cheque Book Registration", "Accounts", "/accounts/cheque_book", "CHEQUE_BOOK_REGISTRATION", "ACCOUNTS", 42, 1);
			seed("User Chart Of Account Management", "Accounts", "/accounts/user-coa-management", "USER_COA_MGMT", "ACCOUNTS", null, 1);
			seed("Account Allocation", "Accounts", "/accounts/allocation", "ACCOUNT_ALLOCATION", "ACCOUNTS", null, 1);
			seed("Account Opening Balance", "Accounts", "/accounts/opening_balance", "ACCOUNT_OPENING_BALANCE", "ACCOUNTS", 10, 1);
			seed("Account Custom Group", "Accounts", "/accounts/custom_group", "ACCOUNT_CUSTOM_GROUP", "ACCOUNTS", 1, 1);

			// ==========================================
			// 2. ACCOUNTS TRANSACTION (18)
			// ==========================================
			seed("Cash Payment Voucher", "Accounts", "/accounts/vouchers/cash-payment", "VOUCHER_CASH_PAYMENT", "ACCOUNTS", null, 2);
			seed("Bank Payment Voucher", "Accounts", "/accounts/vouchers/bank-payment", "VOUCHER_BANK_PAYMENT", "ACCOUNTS", null, 2);
			seed("Cash Receipt Voucher", "Accounts", "/accounts/vouchers/cash-receipt", "VOUCHER_CASH_RECEIPT", "ACCOUNTS", null, 2);
			seed("Bank Receipt Voucher", "Accounts", "/accounts/vouchers/bank-receipt", "VOUCHER_BANK_RECEIPT", "ACCOUNTS", null, 2);
			seed("Journal Voucher", "Accounts", "/accounts/vouchers/journal", "VOUCHER_JOURNAL", "ACCOUNTS", null, 2);
			seed("Contra Voucher", "Accounts", "/accounts/vouchers/contra", "VOUCHER_CONTRA", "ACCOUNTS", null, 2);
			seed("Expense Voucher", "Accounts", "/accounts/vouchers/expense", "VOUCHER_EXPENSE", "ACCOUNTS", null, 2);
			seed("Party Receipt Voucher", "Accounts", "/accounts/vouchers/party-receipt", "VOUCHER_PARTY_RECEIPT", "ACCOUNTS", null, 2);
			seed("Party Payment Voucher", "Accounts", "/accounts/vouchers/party-payment", "VOUCHER_PARTY_PAYMENT", "ACCOUNTS", null, 2);
			seed("Post Dated Cheque Payment Vouchers", "Accounts", "/accounts/vouchers/pdc-payment", "VOUCHER_PDC_PAYMENT", "ACCOUNTS", null, 2);
			seed("Payment By Invoice Voucher", "Accounts", "/accounts/vouchers/payment-by-invoice", "VOUCHER_PAYMENT_INVOICE", "ACCOUNTS", null, 2);
			seed("Voucher Invoices Adjustment", "Accounts", "/accounts/vouchers/invoices-adjustment", "VOUCHER_INVOICES_ADJ", "ACCOUNTS", null, 2);
			seed("Freight Voucher", "Accounts", "/accounts/vouchers/freight", "VOUCHER_FREIGHT", "ACCOUNTS", null, 2);
			seed("Day Book Approval", "Accounts", "/accounts/vouchers/day-book-approval", "VOUCHER_DAYBOOK_APPROVAL", "ACCOUNTS", null, 2);
			seed("PDC Transaction Payment", "Accounts", "/accounts/vouchers/pdc-transaction-payment", "VOUCHER_PDC_TRANS_PAYMENT", "ACCOUNTS", null, 2);
			seed("Advance Adjustment", "Accounts", "/accounts/vouchers/advance-adjustment", "VOUCHER_ADVANCE_ADJ", "ACCOUNTS", null, 2);
			seed("Contractor Wages Account", "Accounts", "/accounts/vouchers/contractor-wages", "VOUCHER_CONTRACTOR_WAGES", "ACCOUNTS", null, 2);
			seed("Voucher Validation", "Accounts", "/accounts/vouchers/voucher-validation", "VOUCHER_VALIDATION", "ACCOUNTS", null, 2);

			// ==========================================
			// 3. ACCOUNT REPORTS (27)
			// ==========================================
			seed("General Ledger Statement", "Accounts", "/accounts/reports/general-ledger-statement", "RPT_GL_STATEMENT", "ACCOUNTS", null, 3);
			seed("General Ledger", "Accounts", "/accounts/reports/general-ledger", "RPT_GENERAL_LEDGER", "ACCOUNTS", null, 3);
			seed("Day Book", "Accounts", "/accounts/reports/day-book", "RPT_DAY_BOOK", "ACCOUNTS", null, 3);
			seed("Balance Sheet", "Accounts", "/accounts/reports/balance-sheet", "RPT_BALANCE_SHEET", "ACCOUNTS", null, 3);
			seed("Profit & Loss Statement", "Accounts", "/accounts/reports/profit-and-loss", "RPT_PROFIT_LOSS", "ACCOUNTS", null, 3);
			seed("Selected Trial Balance", "Accounts", "/accounts/reports/selected-trial-balance", "RPT_SELECTED_TRIAL_BAL", "ACCOUNTS", null, 3);
			seed("Trial Balances All Level", "Accounts", "/accounts/reports/trial-balances-all-level", "RPT_TRIAL_BAL_ALL_LEVEL", "ACCOUNTS", null, 3);
			seed("Trial Balance", "Accounts", "/accounts/reports/trial-balance", "RPT_TRIAL_BALANCE", "ACCOUNTS", null, 3);
			seed("Payables Aging", "Accounts", "/accounts/reports/payables-aging", "RPT_PAYABLES_AGING", "ACCOUNTS", null, 3);
			seed("Receivables Aging", "Accounts", "/accounts/reports/receivables-aging", "RPT_RECEIVABLES_AGING", "ACCOUNTS", null, 3);
			seed("Payables Report", "Accounts", "/accounts/reports/payables-report", "RPT_PAYABLES", "ACCOUNTS", null, 3);
			seed("Receivables Report", "Accounts", "/accounts/reports/receivables-report", "RPT_RECEIVABLES", "ACCOUNTS", null, 3);
			seed("Voucher Report", "Accounts", "/accounts/reports/voucher-report", "RPT_VOUCHER", "ACCOUNTS", null, 3);
			seed("Bank Balances", "Accounts", "/accounts/reports/bank-balances", "RPT_BANK_BALANCES", "ACCOUNTS", null, 3);
			seed("Post Dated Cheque Register", "Accounts", "/accounts/reports/pdc-register", "RPT_PDC_REGISTER", "ACCOUNTS", null, 3);
			seed("Post Dated Cheque Information", "Accounts", "/accounts/reports/pdc-information", "RPT_PDC_INFO", "ACCOUNTS", null, 3);
			seed("Payables Aging New", "Accounts", "/accounts/reports/payables-aging-new", "RPT_PAYABLES_AGING_NEW", "ACCOUNTS", null, 3);
			seed("Receivables Aging New", "Accounts", "/accounts/reports/receivables-aging-new", "RPT_RECEIVABLES_AGING_NEW", "ACCOUNTS", null, 3);
			seed("Payables Report Invoice Wise", "Accounts", "/accounts/reports/payables-report-invoice-wise", "RPT_PAYABLES_INVOICE_WISE", "ACCOUNTS", null, 3);
			seed("Receivables By Due Dates", "Accounts", "/accounts/reports/receivables-by-due-dates", "RPT_RECEIVABLES_DUE_DATES", "ACCOUNTS", null, 3);
			seed("Payables And Payment Schedule", "Accounts", "/accounts/reports/payables-payment-schedule", "RPT_PAYABLES_PAY_SCHED", "ACCOUNTS", null, 3);
			seed("Receivables And Receipt Schedule", "Accounts", "/accounts/reports/receivables-receipt-schedule", "RPT_RECEIVABLES_REC_SCHED", "ACCOUNTS", null, 3);
			seed("Bills Payables Report", "Accounts", "/accounts/reports/bills-payables-report", "RPT_BILLS_PAYABLES", "ACCOUNTS", null, 3);
			seed("Party Limits & Balances", "Accounts", "/accounts/reports/party-limits-balances", "RPT_PARTY_LIMITS", "ACCOUNTS", null, 3);
			seed("Subsidiary Payables Report", "Accounts", "/accounts/reports/subsidiary-payables-report", "RPT_SUBSIDIARY_PAYABLES", "ACCOUNTS", null, 3);
			seed("Chart of Accounts Report", "Accounts", "/accounts/reports/chart-of-accounts-report", "RPT_COA", "ACCOUNTS", null, 3);
			seed("Account Notes Report", "Accounts", "/accounts/reports/account-notes-report", "RPT_ACCOUNT_NOTES", "ACCOUNTS", null, 3);

			// ==========================================
			// 4. BANKING MANAGEMENT (7)
			// ==========================================
			seed("Bank Reconciliation With Vouchers", "Accounts", "/accounts/banking/bank-reconciliation", "BNK_RECON_VOUCHERS", "ACCOUNTS", null, 4);
			seed("Bank Detail Definition", "Accounts", "/accounts/banking/bank-detail-definition", "BNK_DETAIL_DEF", "ACCOUNTS", null, 4);
			seed("Define Bank", "Accounts", "/accounts/banking/define-bank", "BNK_DEFINE_BANK", "ACCOUNTS", null, 4);
			seed("PDC Management", "Accounts", "/accounts/banking/pdc-management", "BNK_PDC_MGMT", "ACCOUNTS", null, 4);
			seed("Cheque Printing", "Accounts", "/accounts/banking/cheque-printing", "BNK_CHEQUE_PRINTING", "ACCOUNTS", null, 4);
			seed("Bank Charges", "Accounts", "/accounts/banking/bank-charges", "BNK_BANK_CHARGES", "ACCOUNTS", null, 4);
			seed("Chart of Account Allocate Cost Center", "Accounts", "/accounts/banking/coa-allocate-cost-center", "BNK_COA_COST_CENTER", "ACCOUNTS", null, 4);

			// Inventory Screens
			seed("Brands", "Inventory", "/inventory/brands", "INVENTORY_BRANDS", "INVENTORY", null, null);
			seed("Item Groups", "Inventory", "/inventory/item_groups", "INVENTORY_ITEM_GROUPS", "INVENTORY", null, null);
			seed("Product Types", "Inventory", "/inventory/product_types", "INVENTORY_PRODUCT_TYPES", "INVENTORY", null, null);
			seed("Warehouses", "Inventory", "/inventory/warehouses", "INVENTORY_WAREHOUSES", "INVENTORY", null, null);
			seed("Racks", "Inventory", "/inventory/racks", "INVENTORY_RACKS", "INVENTORY", null, null);
			seed("Item Categories", "Inventory", "/inventory/item_categories", "INVENTORY_ITEM_CATEGORIES", "INVENTORY", null, null);
			seed("Item Types", "Inventory", "/inventory/item_types", "INVENTORY_ITEM_TYPES", "INVENTORY", null, null);
			seed("Items", "Inventory", "/inventory/items", "INVENTORY_ITEMS", "INVENTORY", null, null);

			// User Management Screens
			seed("User Registration", "User Management", "/user-management/users", "ADD_NEW_USER", "UTILITIES", null, null);
			seed("User Roles", "User Management", "/user-management/roles", "USER_ROLES", "UTILITIES", null, null);
			seed("User Rights", "User Management", "/user-management/rights", "USER_RIGHTS", "UTILITIES", null, null);

			// ==========================================
			// 5. PURCHASE MODULES (10)
			// ==========================================
			seed("Purchase Dashboard", "Purchase", "/purchase", "PURCHASE_DASHBOARD", "PURCHASE", null, 5);
			seed("Supplier Purchases", "Purchase", "/purchase/supplier", "PURCHASE_SUPPLIER", "PURCHASE", null, 5);
			seed("Purchase Reports", "Purchase", "/purchase/reports", "PURCHASE_REPORTS", "PURCHASE", null, 5);
			seed("Purchase Order", "Purchase", "/purchase/purchase-order", "PURCHASE_ORDER", "PURCHASE", 16, 5);
			seed("Inward Gate Pass", "Purchase", "/purchase/inward-gate-pass", "INWARD_GATE_PASS", "PURCHASE", 34, 5);
			seed("Goods Receipt Notes", "Purchase", "/purchase/goods-receipt-notes", "GOODS_RECEIPT_NOTES", "PURCHASE", 36, 5);
			seed("GRN (Sale Return)", "Purchase", "/purchase/grn-sale-return", "GRN_SALE_RETURN", "PURCHASE", 37, 5);
			seed("Purchase Invoice", "Purchase", "/purchase/purchase-invoice", "PURCHASE_INVOICE", "PURCHASE", 18, 5);
			seed("Purchase Invoice Again GRN Direct", "Purchase", "/purchase/purchase-invoice-again-grn-direct", "PURCHASE_INVOICE_GRN_DIRECT", "PURCHASE", 18, 5);
			seed("Purchase Invoice Direct", "Purchase", "/purchase/purchase-invoice-direct", "PURCHASE_INVOICE_DIRECT", "PURCHASE", 18, 5);
			seed("Purchase Invoice Return", "Purchase", "/purchase/purchase-invoice-return", "PURCHASE_INVOICE_RETURN", "PURCHASE", 19, 5);
		} catch (Throwable t) {
			log.warn("ScreenSeeder skipped: {}", t.getMessage());
		}
	}

	private void seed(String name, String module, String url, String authorityCode, String sectionAuthorityCode,
			Integer realScreenDefinitionId, Integer realModuleId) {
		try {
			Screen screen = screenRepository.findByTargetUrl(url);
			if (screen == null) {
				screen = new Screen();
				screen.setTargetUrl(url);
			}
			screen.setScreenName(name);
			screen.setModuleDescription(module);
			screen.setScreenAlias(name);
			screen.setAuthorityCode(authorityCode);
			screen.setSectionAuthorityCode(sectionAuthorityCode);
			if (realScreenDefinitionId != null) {
				screen.setRealScreenDefinitionId(realScreenDefinitionId);
			}
			if (realModuleId != null) {
				screen.setRealModuleId(realModuleId);
			}
			screenRepository.save(screen);
		} catch (Throwable t) {
			// Ignore if table does not exist
		}
	}
}
