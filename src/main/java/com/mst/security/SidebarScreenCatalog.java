package com.mst.security;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

/**
 * The 97 screens this port has actually built, as code.
 *
 * ---------------------------------------------------------------------------------------------
 * WHY THIS REPLACES A TABLE
 * ---------------------------------------------------------------------------------------------
 * This list used to be rows in MstScreen, written at startup by ScreenSeeder. MstScreen is not a
 * desktop table - the desktop's screen master is the real dbo.ScreenDefinition, which already has
 * roughly 900 rows describing every WinForms screen. The only thing MstScreen added was the
 * string authority code that fixed_sidebar.html gates each menu item on
 * ({@code hasAuthority('CHART_OF_ACCOUNT')}), because the real schema has no such column.
 *
 * An authority code is a fact about THIS application's template, not about the customer's data.
 * It belongs in the application, so here it is - no table, no seeder, no startup write, and
 * nothing to drift out of step with the routes it names.
 *
 * Generated from ScreenSeeder's own seed(...) calls, in its order and grouping, so nothing was
 * retyped by hand.
 *
 * ---------------------------------------------------------------------------------------------
 * WHERE THE realScreenDefinitionId VALUES CAME FROM
 * ---------------------------------------------------------------------------------------------
 * 41 were already confirmed when the seeder was written. A further 21 were filled from the live
 * dump this repository already holds - migration/user-rights/reconciliation-input.json, taken
 * from GoldenAcedb by the project's own Query.ps1 - by matching the screen's name against the
 * real ScreenName and ScreenAlias, normalised.
 *
 * A name that matched MORE than one real screen was deliberately left null: "Bank Payment
 * Voucher" is both 29 and 703, "Day Book" is both 15 and 24, and guessing between them would
 * gate a menu item on another screen's rights. 35 entries are still null - 31 absent from that
 * dump (it covers 247 of roughly 900 screens, being one user's grants) and 4 ambiguous. They are
 * matched by name at runtime instead, and hidden when that does not resolve either.
 *
 * ---------------------------------------------------------------------------------------------
 * HOW A ROW BECOMES AN AUTHORITY
 * ---------------------------------------------------------------------------------------------
 * {@code realScreenDefinitionId} is the real dbo.ScreenDefinition.Id where it was confirmed.
 * Where it is null, DesktopScreenRightsService resolves the screen by {@code screenName} against
 * the real ScreenDefinition at runtime. Either way the grant comes from the real chain -
 * CompanyRights (is this screen enabled for the company) plus tblUserRights/ScreenRights (has
 * this user been granted "View") - exactly as the desktop decides it.
 */
public final class SidebarScreenCatalog {

    private SidebarScreenCatalog() { }

    /** One built screen: its desktop name, its web route, and the authorities it unlocks. */
    public static final class Entry {
        public final String screenName;
        public final String module;
        public final String targetUrl;
        public final String authorityCode;
        public final String sectionAuthorityCode;
        /** The confirmed real dbo.ScreenDefinition.Id, or null to resolve by screenName. */
        public final Integer realScreenDefinitionId;
        /** The confirmed real AppModules.Id, or null. Not used for gating - kept for reference. */
        public final Integer realModuleId;

        Entry(String screenName, String module, String targetUrl, String authorityCode,
              String sectionAuthorityCode, Integer realScreenDefinitionId, Integer realModuleId) {
            this.screenName = screenName;
            this.module = module;
            this.targetUrl = targetUrl;
            this.authorityCode = authorityCode;
            this.sectionAuthorityCode = sectionAuthorityCode;
            this.realScreenDefinitionId = realScreenDefinitionId;
            this.realModuleId = realModuleId;
        }
    }

    private static final List<Entry> ENTRIES = build();

    public static List<Entry> all() { return ENTRIES; }

    private static List<Entry> build() {
        List<Entry> e = new ArrayList<>();

        // 1. ACCOUNT DEFINITION
        e.add(new Entry("Bank Reconciliation Upload Excel", "Accounts", "/accounts/bank-reconciliation-upload-excel", "BANK_RECON_EXCEL", "ACCOUNTS", 884, 1));
        e.add(new Entry("Chart Of Account Definition", "Accounts", "/accounts/chart_of_accounts", "CHART_OF_ACCOUNT", "ACCOUNTS", 9, 1));
        e.add(new Entry("Define Supplier / Customer", "Accounts", "/accounts/supplier", "DEFINE_SUPPLIER", "ACCOUNTS", 11, 1));
        e.add(new Entry("Cheque Book Registration", "Accounts", "/accounts/cheque_book", "CHEQUE_BOOK_REGISTRATION", "ACCOUNTS", 42, 1));
        e.add(new Entry("User Chart Of Account Management", "Accounts", "/accounts/user-coa-management", "USER_COA_MGMT", "ACCOUNTS", 14, 1));
        e.add(new Entry("Account Allocation", "Accounts", "/accounts/allocation", "ACCOUNT_ALLOCATION", "ACCOUNTS", 2, 1));
        e.add(new Entry("Account Opening Balance", "Accounts", "/accounts/opening_balance", "ACCOUNT_OPENING_BALANCE", "ACCOUNTS", 10, 1));
        e.add(new Entry("Account Custom Group", "Accounts", "/accounts/custom_group", "ACCOUNT_CUSTOM_GROUP", "ACCOUNTS", 1, 1));

        // 2. ACCOUNTS TRANSACTION
        e.add(new Entry("Cash Payment Voucher", "Accounts", "/accounts/vouchers/cash-payment", "VOUCHER_CASH_PAYMENT", "ACCOUNTS", 28, 2));
        e.add(new Entry("Bank Payment Voucher", "Accounts", "/accounts/vouchers/bank-payment", "VOUCHER_BANK_PAYMENT", "ACCOUNTS", 29, 2));   // ModuleId 2 Accounts Transaction; 703 is the Banking Managment one
        e.add(new Entry("Cash Receipt Voucher", "Accounts", "/accounts/vouchers/cash-receipt", "VOUCHER_CASH_RECEIPT", "ACCOUNTS", 30, 2));
        e.add(new Entry("Bank Receipt Voucher", "Accounts", "/accounts/vouchers/bank-receipt", "VOUCHER_BANK_RECEIPT", "ACCOUNTS", 31, 2));   // ModuleId 2 Accounts Transaction; 704 is the Banking Managment one
        e.add(new Entry("Journal Voucher", "Accounts", "/accounts/vouchers/journal", "VOUCHER_JOURNAL", "ACCOUNTS", 860, 2));
        /* Screen 19 "Jounal Voucher" (ScreenAlias as in dbo.ScreenDefinition) - Account_Definition.VoucherEntry. */
        e.add(new Entry("Jounal Voucher", "Accounts", "/accounts/vouchers/voucher-entry", "VOUCHER_JOURNAL_ENTRY", "ACCOUNTS", 19, 2));
        e.add(new Entry("Contra Voucher", "Accounts", "/accounts/vouchers/contra", "VOUCHER_CONTRA", "ACCOUNTS", 22, 2));
        e.add(new Entry("Contra Voucher New", "Accounts", "/accounts/vouchers/contra-tax", "VOUCHER_CONTRA_TAX", "ACCOUNTS", 855, 2));
        e.add(new Entry("Expense Voucher", "Accounts", "/accounts/vouchers/expense", "VOUCHER_EXPENSE", "ACCOUNTS", 46, 2));
        e.add(new Entry("Party Receipt Voucher", "Accounts", "/accounts/vouchers/party-receipt", "VOUCHER_PARTY_RECEIPT", "ACCOUNTS", 17, 2));
        e.add(new Entry("Party Payment Voucher", "Accounts", "/accounts/vouchers/party-payment", "VOUCHER_PARTY_PAYMENT", "ACCOUNTS", 16, 2));
        e.add(new Entry("Premature Receipts", "Accounts", "/accounts/vouchers/premature-receipts", "VOUCHER_PREMATURE_RECEIPTS", "ACCOUNTS", 25, 2));
        e.add(new Entry("Bills Payable", "Accounts", "/accounts/vouchers/bills-payables", "VOUCHER_BILLS_PAYABLE", "ACCOUNTS", 38, 2));
        e.add(new Entry("Bills Receivables", "Accounts", "/accounts/vouchers/bills-receivables", "VOUCHER_BILLS_RECEIVABLE", "ACCOUNTS", 39, 2));
        e.add(new Entry("Post Dated Cheque Payment Vouchers", "Accounts", "/accounts/vouchers/pdc-payment", "VOUCHER_PDC_PAYMENT", "ACCOUNTS", 34, 2));
        e.add(new Entry("Payment By Invoice Voucher", "Accounts", "/accounts/vouchers/payment-by-invoice", "VOUCHER_PAYMENT_INVOICE", "ACCOUNTS", 41, 2));
        e.add(new Entry("Voucher Invoices Adjustment", "Accounts", "/accounts/vouchers/invoices-adjustment", "VOUCHER_INVOICES_ADJ", "ACCOUNTS", 861, 2));   // 861 frmInvoicesAdjustmentVoucher
        e.add(new Entry("Receipt Invoices Adjustment Voucher", "Accounts", "/accounts/vouchers/receipt-invoices-adjustment", "VOUCHER_RECEIPT_INVOICES_ADJ", "ACCOUNTS", 863, 2));   // 863 frmReceiptInvoicesAdjustmentVoucher
        e.add(new Entry("Freight Voucher", "Accounts", "/accounts/vouchers/freight", "VOUCHER_FREIGHT", "ACCOUNTS", 26, 2));
        e.add(new Entry("Day Book Approval", "Accounts", "/accounts/vouchers/day-book-approval", "VOUCHER_DAYBOOK_APPROVAL", "ACCOUNTS", null, 2));
        e.add(new Entry("PDC Transaction Payment", "Accounts", "/accounts/vouchers/pdc-transaction-payment", "VOUCHER_PDC_TRANS_PAYMENT", "ACCOUNTS", null, 2));
        e.add(new Entry("Advance Adjustment", "Accounts", "/accounts/vouchers/advance-adjustment", "VOUCHER_ADVANCE_ADJ", "ACCOUNTS", 4, 2));
        e.add(new Entry("Contractor Wages Account", "Accounts", "/accounts/vouchers/contractor-wages", "VOUCHER_CONTRACTOR_WAGES", "ACCOUNTS", 182, 2));
        e.add(new Entry("Voucher Validation", "Accounts", "/accounts/vouchers/voucher-validation", "VOUCHER_VALIDATION", "ACCOUNTS", 83, 2));

        // 3. ACCOUNT REPORTS
        e.add(new Entry("General Ledger Statement", "Accounts", "/accounts/reports/general-ledger-statement", "RPT_GL_STATEMENT", "ACCOUNTS", 54, 3));
        e.add(new Entry("General Ledger", "Accounts", "/accounts/reports/general-ledger", "RPT_GENERAL_LEDGER", "ACCOUNTS", 79, 3));   // ModuleId 3 Account Reports; 706 is the Banking Managment one
        e.add(new Entry("Day Book", "Accounts", "/accounts/vouchers/day-book", "RPT_DAY_BOOK", "ACCOUNTS", 15, 2));   // alias 'Day Book' - ScreenDefinition 15 DayBook, ModuleId 2 (Accounts Transaction), a voucher-entry form
        e.add(new Entry("Day Book (Off Set)", "Accounts", "/accounts/vouchers/day-book-offset", "VOUCHER_DAYBOOK_OFFSET", "ACCOUNTS", 24, 2));   // alias 'Day Book (Off Set)' - ScreenDefinition 24 frmDayBook, ModuleId 2
        e.add(new Entry("Balance Sheet", "Accounts", "/accounts/reports/balance-sheet", "RPT_BALANCE_SHEET", "ACCOUNTS", 62, 3));
        e.add(new Entry("Profit & Loss Statement", "Accounts", "/accounts/reports/profit-and-loss", "RPT_PROFIT_LOSS", "ACCOUNTS", null, 3));
        e.add(new Entry("Selected Trial Balance", "Accounts", "/accounts/reports/selected-trial-balance", "RPT_SELECTED_TRIAL_BAL", "ACCOUNTS", 81, 3));
        e.add(new Entry("Trial Balances All Level", "Accounts", "/accounts/reports/trial-balances-all-level", "RPT_TRIAL_BAL_ALL_LEVEL", "ACCOUNTS", 82, 3));
        e.add(new Entry("Trial Balance", "Accounts", "/accounts/reports/trial-balance", "RPT_TRIAL_BALANCE", "ACCOUNTS", 51, 3));
        e.add(new Entry("Payables & Receivables Reports", "Accounts", "/accounts/reports/all-payables", "RPT_ALL_PAYABLES", "ACCOUNTS", 787, 3));
        e.add(new Entry("1001 Trade Creditors", "Accounts", "/accounts/reports/trade-payables", "RPT_TRADE_CREDITORS", "ACCOUNTS", 66, 3));
        e.add(new Entry("1002 Trade Debtors", "Accounts", "/accounts/reports/trade-receivables", "RPT_TRADE_DEBTORS", "ACCOUNTS", 65, 3));
        e.add(new Entry("1006 Receivables Aging", "Accounts", "/accounts/reports/receivables-new", "RPT_RECEIVABLES_AGING", "ACCOUNTS", 769, 3));
        e.add(new Entry("1008 Payables & Receivable Forecast", "Accounts", "/accounts/reports/due-date-analysis", "RPT_PAYABLES_FORECAST", "ACCOUNTS", 75, 3));
        e.add(new Entry("Payables Aging", "Accounts", "/accounts/reports/payables-aging", "RPT_PAYABLES_AGING", "ACCOUNTS", 47, 3));
        e.add(new Entry("Receivables Aging", "Accounts", "/accounts/reports/receivables-aging", "RPT_RECEIVABLES_AGING", "ACCOUNTS", 769, 3));
        e.add(new Entry("Payables Report", "Accounts", "/accounts/reports/payables-report", "RPT_PAYABLES", "ACCOUNTS", 48, 3));
        e.add(new Entry("Receivables Report", "Accounts", "/accounts/reports/receivables-report", "RPT_RECEIVABLES", "ACCOUNTS", 80, 3));
        e.add(new Entry("Voucher Report", "Accounts", "/accounts/reports/voucher-report", "RPT_VOUCHER", "ACCOUNTS", 53, 3));
        e.add(new Entry("Bank Balances", "Accounts", "/accounts/reports/bank-balances", "RPT_BANK_BALANCES", "ACCOUNTS", 73, 3));
        e.add(new Entry("Post Dated Cheque Register", "Accounts", "/accounts/reports/pdc-register", "RPT_PDC_REGISTER", "ACCOUNTS", 88, 3));
        e.add(new Entry("Post Dated Cheque Information", "Accounts", "/accounts/reports/pdc-information", "RPT_PDC_INFO", "ACCOUNTS", null, 3));
        e.add(new Entry("Payables Aging New", "Accounts", "/accounts/reports/payables-aging-new", "RPT_PAYABLES_AGING_NEW", "ACCOUNTS", 47, 3));
        e.add(new Entry("Receivables Aging New", "Accounts", "/accounts/reports/receivables-aging-new", "RPT_RECEIVABLES_AGING_NEW", "ACCOUNTS", 872, 3));   // page = frmReceivableAgingDocumentWise (872 Customer Aging Report)
        e.add(new Entry("Supplier Aging Report", "Accounts", "/accounts/reports/supplier-aging", "RPT_SUPPLIER_AGING_DOC", "ACCOUNTS", 873, 3));   // frmPayablesAgingDocumentWise
        e.add(new Entry("Inventory Payables and Receivables", "Accounts", "/accounts/reports/inventory-payables-receivables", "RPT_INV_PAY_REC", "ACCOUNTS", 61, 3));   // InventoryPayablesandReceivables
        e.add(new Entry("Payables Report Invoice Wise", "Accounts", "/accounts/reports/payables-report-invoice-wise", "RPT_PAYABLES_INVOICE_WISE", "ACCOUNTS", 909, 3));
        e.add(new Entry("Receivables By Due Dates", "Accounts", "/accounts/reports/receivables-by-due-dates", "RPT_RECEIVABLES_DUE_DATES", "ACCOUNTS", 68, 3));
        e.add(new Entry("Payables And Payment Schedule", "Accounts", "/accounts/reports/payables-payment-schedule", "RPT_PAYABLES_PAY_SCHED", "ACCOUNTS", 78, 3));
        e.add(new Entry("Receivables And Receipt Schedule", "Accounts", "/accounts/reports/receivables-receipt-schedule", "RPT_RECEIVABLES_REC_SCHED", "ACCOUNTS", 71, 3));
        e.add(new Entry("Bills Payables Report", "Accounts", "/accounts/reports/bills-payables-report", "RPT_BILLS_PAYABLES", "ACCOUNTS", null, 3));
        e.add(new Entry("Party Limits & Balances", "Accounts", "/accounts/reports/party-limits-balances", "RPT_PARTY_LIMITS", "ACCOUNTS", null, 3));
        e.add(new Entry("Subsidiary Payables Report", "Accounts", "/accounts/reports/subsidiary-payables-report", "RPT_SUBSIDIARY_PAYABLES", "ACCOUNTS", null, 3));
        e.add(new Entry("Chart of Accounts Report", "Accounts", "/accounts/reports/chart-of-accounts-report", "RPT_COA", "ACCOUNTS", null, 3));
        e.add(new Entry("Account Notes Report", "Accounts", "/accounts/reports/account-notes-report", "RPT_ACCOUNT_NOTES", "ACCOUNTS", null, 3));

        // 4. BANKING MANAGEMENT
        e.add(new Entry("Bank Reconciliation With Vouchers", "Accounts", "/accounts/banking/bank-reconciliation", "BNK_RECON_VOUCHERS", "ACCOUNTS", 885, 4));
        e.add(new Entry("Bank Detail Definition", "Accounts", "/accounts/banking/bank-detail-definition", "BNK_DETAIL_DEF", "ACCOUNTS", null, 4));
        e.add(new Entry("Define Bank", "Accounts", "/accounts/banking/define-bank", "BNK_DEFINE_BANK", "ACCOUNTS", 709, 4));
        e.add(new Entry("PDC Management", "Accounts", "/accounts/banking/pdc-management", "BNK_PDC_MGMT", "ACCOUNTS", null, 4));
        e.add(new Entry("Cheque Printing", "Accounts", "/accounts/banking/cheque-printing", "BNK_CHEQUE_PRINTING", "ACCOUNTS", 700, 4));
        e.add(new Entry("Bank Charges", "Accounts", "/accounts/banking/bank-charges", "BNK_BANK_CHARGES", "ACCOUNTS", null, 4));
        e.add(new Entry("Chart of Account Allocate Cost Center", "Accounts", "/accounts/banking/coa-allocate-cost-center", "BNK_COA_COST_CENTER", "ACCOUNTS", null, 4));
        e.add(new Entry("Define Brand", "Inventory", "/inventory/brands", "INVENTORY_BRANDS", "INVENTORY", 876, 4));
        e.add(new Entry("Pos Define Item", "Inventory", "/inventory/pos-define-item", "INVENTORY_POS_ITEMS", "INVENTORY", 106, 4));
        e.add(new Entry("Inventory Stock Transactions Report", "Inventory", "/inventory/stock-transactions", "INVENTORY_STOCK_TRANSACTIONS", "INVENTORY", 580, 93));
        e.add(new Entry("Item Ledger (Inventory)", "Inventory", "/stocks/item_ledger", "INVENTORY_ITEM_LEDGER", "INVENTORY", 287, 19));
        e.add(new Entry("Stock Report Store", "Inventory", "/stocks/store-stock-report", "INVENTORY_STORE_STOCK", "INVENTORY", 288, 19));
        e.add(new Entry("Transaction Report Vehicle Wise", "Inventory", "/stocks/transaction-vehicle-wise", "INVENTORY_VEHICLE_TRANSACTIONS", "INVENTORY", 291, 19));
        e.add(new Entry("Item Groups", "Inventory", "/inventory/item_groups", "INVENTORY_ITEM_GROUPS", "INVENTORY", null, null));
        e.add(new Entry("Product Types", "Inventory", "/inventory/product_types", "INVENTORY_PRODUCT_TYPES", "INVENTORY", null, null));
        e.add(new Entry("Define WareHouse", "Inventory", "/inventory/warehouses", "INVENTORY_WAREHOUSES", "INVENTORY", 116, 4));
        e.add(new Entry("Racks", "Inventory", "/inventory/racks", "INVENTORY_RACKS", "INVENTORY", null, null));
        e.add(new Entry("Define Item Category", "Inventory", "/inventory/item_categories", "INVENTORY_ITEM_CATEGORIES", "INVENTORY", 112, 4));
        e.add(new Entry("Define Item Type", "Inventory", "/inventory/item_types", "INVENTORY_ITEM_TYPES", "INVENTORY", 113, 4));
        e.add(new Entry("Define Item", "Inventory", "/inventory/items", "INVENTORY_ITEMS", "INVENTORY", 111, 4));
        e.add(new Entry("Opening Stock Balancing", "Inventory", "/stocks/stock_opening_form", "INVENTORY_OPENING_STOCK", "INVENTORY", 92, 4));
        e.add(new Entry("Define Item Uom Schedule", "Inventory", "/inventory/item_uom_schedule", "INVENTORY_UOM_SCHEDULE", "INVENTORY", 115, 4));
        e.add(new Entry("Item Min Max Rate Schedule", "Inventory", "/inventory/item-min-max-rate", "INVENTORY_MIN_MAX", "INVENTORY", 104, 4));
        e.add(new Entry("Consumption Items", "Inventory", "/inventory/consumption-items", "INVENTORY_CONSUMPTION", "INVENTORY", 105, 4));
        e.add(new Entry("Define Lots", "Inventory", "/inventory/define-lots", "INVENTORY_LOTS", "INVENTORY", 110, 4));
        e.add(new Entry("User Registration", "User Management", "/user-management/users", "ADD_NEW_USER", "UTILITIES", null, null));
        e.add(new Entry("User Roles", "User Management", "/user-management/roles", "USER_ROLES", "UTILITIES", null, null));
        e.add(new Entry("User Rights", "User Management", "/user-management/rights", "USER_RIGHTS", "UTILITIES", 409, 43));


        /* CORRECTED 2026-09-20. Six of these ids were filled by matching the catalog's own screen
           name against ScreenName/ScreenAlias, and the match landed on the wrong rows. Read from
           dbo.ScreenDefinition in the GoldenAcedb dump, the ids above were pointing at entirely
           different screens - 18 is "Receipt By Contract", 19 is "Jounal Voucher", 34 is
           "Post Dated Cheque Payment Voucher", 36 is "Payment By Invoice", 37 is "With Holding Tax
           Deposit Challan". Because an entry's id is what its menu item is gated on, each of these
           was reading another screen's grants. The ids now come from ScreenDefinition.TargetUrl,
           which is what the desktop itself reflects on to open a form. */
        // 5. PURCHASE MODULES
        e.add(new Entry("Purchase Dashboard", "Purchase", "/purchase", "PURCHASE_DASHBOARD", "PURCHASE", null, 5));
        e.add(new Entry("Supplier Purchases", "Purchase", "/purchase/supplier", "PURCHASE_SUPPLIER", "PURCHASE", null, 5));
        e.add(new Entry("Purchase Reports", "Purchase", "/purchase/reports", "PURCHASE_REPORTS", "PURCHASE", null, 5));
        e.add(new Entry("Purchase Order", "Purchase", "/purchase/purchase-order", "PURCHASE_ORDER", "PURCHASE", 120, 5));
        e.add(new Entry("Inward Gate Pass", "Purchase", "/purchase/inward-gate-pass", "INWARD_GATE_PASS", "PURCHASE", 134, 5));
        e.add(new Entry("Goods Receipt Notes", "Purchase", "/purchase/goods-receipt-notes", "GOODS_RECEIPT_NOTES", "PURCHASE", 121, 5));
        e.add(new Entry("Goods Receipt Notes Direct", "Purchase", "/purchase/grn-direct", "GOODS_RECEIPT_NOTES_DIRECT", "PURCHASE", 129, 5));
        e.add(new Entry("GRN Direct Against Order", "Purchase", "/purchase/grn-direct-against-order", "GRN_DIRECT_AGAINST_ORDER", "PURCHASE", 133, 5));
        e.add(new Entry("Purchase Invoice Against Grn Order", "Purchase", "/purchase/purchase-invoice-against-grn-order", "PURCHASE_INVOICE_GRN_ORDER", "PURCHASE", 132, 5));
        e.add(new Entry("Purchase Invoice For Upload", "Purchase", "/purchase/purchase-invoice-for-upload", "PURCHASE_INVOICE_FOR_UPLOAD", "PURCHASE", 910, 5));
        /* Was 37 - that is GRN's old web DocumentTypeId, not a screen; the real ScreenDefinition row is 866. */
        e.add(new Entry("Grn (Sale Return)", "Purchase", "/purchase/grn-sale-return", "GRN_SALE_RETURN", "PURCHASE", 866, 5));
        e.add(new Entry("Stock In Transit", "Purchase", "/purchase/stock-in-transit", "STOCK_IN_TRANSIT", "PURCHASE", 868, 5));
        e.add(new Entry("Purchase Invoice", "Purchase", "/purchase/purchase-invoice", "PURCHASE_INVOICE", "PURCHASE", 122, 5));
        e.add(new Entry("Purchase Invoice Again GRN Direct", "Purchase", "/purchase/purchase-invoice-again-grn-direct", "PURCHASE_INVOICE_GRN_DIRECT", "PURCHASE", 131, 5));
        e.add(new Entry("Purchase Invoice Direct", "Purchase", "/purchase/purchase-invoice-direct", "PURCHASE_INVOICE_DIRECT", "PURCHASE", 117, 5));
        e.add(new Entry("Purchase Invoice Return", "Purchase", "/purchase/purchase-invoice-return", "PURCHASE_INVOICE_RETURN", "PURCHASE", 125, 5));
        // Purchase Reports, module 52 (were not in the catalog)
        e.add(new Entry("Purchase Order Report", "Purchase", "/purchase/reports/purchase-order-register", "PURCHASE_ORDER_REPORT", "PURCHASE", 479, 52));
        e.add(new Entry("Gate Pass Report", "Purchase", "/purchase/reports/inward-gate-pass-register", "PURCHASE_GATE_PASS_REPORT", "PURCHASE", 478, 52));
        e.add(new Entry("Grn Report", "Purchase", "/purchase/reports/grn-register", "PURCHASE_GRN_REPORT", "PURCHASE", 477, 52));
        e.add(new Entry("Purchase Report (With Activites)", "Purchase", "/purchase/reports/purchase-invoice-register", "PURCHASE_REPORT_WITH_ACTIVITIES", "PURCHASE", 480, 52));
        e.add(new Entry("Stock In Transit Report", "Purchase", "/purchase/reports/stock-in-transit", "STOCK_IN_TRANSIT_REPORT", "PURCHASE", 869, 52));

        // 6. PRODUCTION (module 18)
        // Ids confirmed from migration/user-rights/reconciliation-input.json, the live dump of
        // GoldenAcedb this repository already holds - not matched by name.
        e.add(new Entry("Production Job Order", "Production", "/production/job-order", "PRODUCTION_JOB_ORDER", "PRODUCTION", 281, 18));
        e.add(new Entry("Stock Conversion", "Production", "/production/stock-conversion", "PRODUCTION_STOCK_CONVERSION", "PRODUCTION", 276, 18));
        // The three master-data definitions (DefineProductionType.cs, DefineProductionPlanType.cs,
        // DefineProductionPlant.cs). In dbo.ScreenDefinition (GoldenAceDb(0509)t.sql) they are NOT under
        // Production (18) but under AppModules 45 "Accounts Definition" (ModuleTypeId 4, the Utility menu):
        //   420 frmDefinePlant / "Define Plant Utility"   TargetUrl Architecture.WinApp.Production.DefineProductionPlant
        //   425 DefineProductionPlanType / "Production Plan Type"  TargetUrl "WinApp.Production.DefineProductionType"
        //       (no "Architecture." prefix - the desktop cannot open this row; matched here by its ScreenName)
        // DefineProductionType has no row of its own, so it is null: hidden from the menu, reachable by URL.
        e.add(new Entry("Define Plant Utility", "Production", "/production/define-production-plant", "PRODUCTION_DEFINE_PLANT", "PRODUCTION", 420, 45));
        e.add(new Entry("Production Plan Type", "Production", "/production/define-production-plan-type", "PRODUCTION_DEFINE_PLAN_TYPE", "PRODUCTION", 425, 45));
        e.add(new Entry("DefineProductionType", "Production", "/production/define-production-type", "PRODUCTION_DEFINE_TYPE", "PRODUCTION", null, 45));

        // MASTER DATA DEFINITION (App 19, AppModules 2039 "System_Level") - ids from the ScreenDefinition dump.
        e.add(new Entry("Country", "Master Data Definition", "/master-data/country", "MDD_COUNTRY", "MASTER_DATA_DEFINITION", 749, 2039));
        e.add(new Entry("City", "Master Data Definition", "/master-data/city", "MDD_CITY", "MASTER_DATA_DEFINITION", 750, 2039));
        e.add(new Entry("Currency", "Master Data Definition", "/master-data/currency", "MDD_CURRENCY", "MASTER_DATA_DEFINITION", 751, 2039));
        e.add(new Entry("Sea Ports", "Master Data Definition", "/master-data/sea-ports", "MDD_SEA_PORTS", "MASTER_DATA_DEFINITION", 752, 2039));
        e.add(new Entry("Province", "Master Data Definition", "/master-data/province", "MDD_PROVINCE", "MASTER_DATA_DEFINITION", 753, 2039));
        e.add(new Entry("Date Lock", "Master Data Definition", "/master-data/date-lock", "MDD_DATE_LOCK", "MASTER_DATA_DEFINITION", 754, 2039));
        e.add(new Entry("Tehsil", "Master Data Definition", "/master-data/tehsil", "MDD_TEHSIL", "MASTER_DATA_DEFINITION", 755, 2039));
        e.add(new Entry("District", "Master Data Definition", "/master-data/district", "MDD_DISTRICT", "MASTER_DATA_DEFINITION", 756, 2039));
        e.add(new Entry("Other Items", "Master Data Definition", "/master-data/other-items", "MDD_OTHER_ITEMS", "MASTER_DATA_DEFINITION", 742, 2039));
        // AppModules 45 "Accounts Definition", the other module of App 19 (412 Account Custom Group opens screen 1's page).
        e.add(new Entry("Bank", "Master Data Definition", "/master-data/bank", "MDD_BANK", "MASTER_DATA_DEFINITION", 413, 45));
        e.add(new Entry("BsPl Settings Form", "Master Data Definition", "/master-data/bs-pl-setting", "MDD_BSPL_SETTING", "MASTER_DATA_DEFINITION", 414, 45));
        e.add(new Entry("PDC Bank", "Master Data Definition", "/master-data/pdc-bank", "MDD_PDC_BANK", "MASTER_DATA_DEFINITION", 427, 45));
        e.add(new Entry("Tax Lookup", "Master Data Definition", "/master-data/tax-lookup", "MDD_TAX_LOOKUP", "MASTER_DATA_DEFINITION", 428, 45));
        e.add(new Entry("Document Group", "Master Data Definition", "/master-data/document-group", "MDD_DOCUMENT_GROUP", "MASTER_DATA_DEFINITION", 429, 45));
        e.add(new Entry("Shipment Documents", "Master Data Definition", "/master-data/shipment-documents", "MDD_SHIPMENT_DOCUMENTS", "MASTER_DATA_DEFINITION", 430, 45));

        // EXPORT (App 8, AppModules 11 "Export") - the two screens CompanyRights enables for company 78;
        // ids from the ScreenDefinition dump (GoldenAceDb(0509)t.sql).
        e.add(new Entry("Gd Break Up By Invoice", "Export", "/export/gd-break-up-by-invoice", "EXP_GD_BREAK_UP_BY_INVOICE", "EXPORT", 881, 11));
        e.add(new Entry("Export Invoice Packing List", "Export", "/export/invoice-packing-list", "EXP_INVOICE_PACKING_LIST", "EXPORT", 882, 11));
        e.add(new Entry("5011 Receivable By Due Report", "Export", "/export/receivable-by-due-date", "EXP_RECEIVABLE_BY_DUE_DATE", "EXPORT", 235, 17));
        e.add(new Entry("5003 Contract Schedule Periodic", "Export", "/export/contract-schedule-periodic", "EXP_CONTRACT_SCHEDULE_PERIODIC", "EXPORT", 236, 17));
        e.add(new Entry("5006 Shipment Costing Detail", "Export", "/export/shipment-costing", "EXP_SHIPMENT_COSTING", "EXPORT", 238, 17));
        e.add(new Entry("5008 Shipments Document Status Report", "Export", "/export/shipments-document-status", "EXP_SHIPMENTS_DOCUMENT_STATUS", "EXPORT", 239, 17));
        e.add(new Entry("Export Detail History (Not Use)", "Export", "/export/detail-history", "EXP_DETAIL_HISTORY", "EXPORT", 249, 17));
        e.add(new Entry("5015 Delivery Order Report", "Export", "/export/delivery-order-report", "EXP_DELIVERY_ORDER_REPORT", "EXPORT", 250, 17));
        e.add(new Entry("5001 Contract Report", "Export", "/export/contract-register", "EXP_CONTRACT_REGISTER", "EXPORT", 251, 17));
        e.add(new Entry("5014 Shipment Tracking Follow up", "Export", "/export/shipment-tracking-follow-up", "EXP_SHIPMENT_TRACKING_FOLLOW_UP", "EXPORT", 254, 17));
        e.add(new Entry("5016 Export Shipment Weight Audit", "Export", "/export/shipment-weight-audit", "EXP_SHIPMENT_WEIGHT_AUDIT", "EXPORT", 261, 17));
        e.add(new Entry("5013 Loading Sheet", "Export", "/export/loading-sheet", "EXP_LOADING_SHEET", "EXPORT", 262, 17));
        e.add(new Entry("5010 Shipment CRO Booking Report", "Export", "/export/shipment-cro-booking-report", "EXP_SHIPMENT_CRO_BOOKING_REPORT", "EXPORT", 266, 17));
        e.add(new Entry("5012 Forwarding Report", "Export", "/export/forwarding-report", "EXP_FORWARDING_REPORT", "EXPORT", 269, 17));
        e.add(new Entry("5004 Sale Report", "Export", "/export/sale-report", "EXP_SALE_REPORT", "EXPORT", 270, 17));
        e.add(new Entry("5007 Sale Comparisons Report", "Export", "/export/sale-comparisons-report", "EXP_SALE_COMPARISONS_REPORT", "EXPORT", 759, 17));
        e.add(new Entry("Service Bill Register", "Export", "/export/service-bill-register", "EXP_SERVICE_BILL_REGISTER", "EXPORT", 912, 17));
        e.add(new Entry("Export Forwarding (Costing Report)", "Export", "/export/forwarding-costing-report", "EXP_FORWARDING_COSTING_REPORT", "EXPORT", 913, 17));
        e.add(new Entry("5005 Shipment Costing Summary", "Export", "/export/shipment-costing-summary", "EXP_SHIPMENT_COSTING_SUMMARY", "EXPORT", 919, 17));
        e.add(new Entry("5017 FI Utilization Report", "Export", "/export/fi-utilization-report", "EXP_FI_UTILIZATION_REPORT", "EXPORT", 935, 17));
        e.add(new Entry("5017 Fcy Receipts Report", "Export", "/export/fcy-receipts-summary-register", "EXP_FCY_RECEIPTS_SUMMARY_REGISTER", "EXPORT", 241, 133));
        e.add(new Entry("Shipment data For Brokery Tax", "Export", "/export/shipment-data-for-brokery-tax", "EXP_SHIPMENT_DATA_FOR_BROKERY_TAX", "EXPORT", 242, 133));
        e.add(new Entry("Export Packing List Register", "Export", "/export/packing-list-register", "EXP_PACKING_LIST_REGISTER", "EXPORT", 243, 133));
        e.add(new Entry("Pending Forwarding For Commercial Invoice", "Export", "/export/pending-forwarding-for-commercial-invoice", "EXP_PENDING_FORWARDING_FOR_COMMERCIAL_INVOICE", "EXPORT", 244, 133));
        e.add(new Entry("GD BreakUp and Realized_Register", "Export", "/export/gd-break-up-and-realized-register", "EXP_GD_BREAK_UP_AND_REALIZED_REGISTER", "EXPORT", 245, 133));
        e.add(new Entry("FI Balance Summary", "Export", "/export/fi-balance-summary", "EXP_FI_BALANCE_SUMMARY", "EXPORT", 246, 133));
        e.add(new Entry("Comm Agent Fcy Ledger", "Export", "/export/commission-agent-fcy-ledger", "EXP_COMMISSION_AGENT_FCY_LEDGER", "EXPORT", 256, 133));
        e.add(new Entry("EEReport", "Export", "/export/ee-report-export-gd", "EXP_EE_REPORT_EXPORT_GD", "EXPORT", 257, 133));
        e.add(new Entry("Commercial Invoice Shipments", "Export", "/export/commercial-invoice-shipments", "EXP_COMMERCIAL_INVOICE_SHIPMENTS", "EXPORT", 258, 133));
        e.add(new Entry("5009 Consignment Follow Up Report", "Export", "/export/consignment-follow-up", "EXP_CONSIGNMENT_FOLLOW_UP", "EXPORT", 203, 11));
        e.add(new Entry("Third Party Inspection Lot Tracking Report", "Export", "/export/third-party-inspection-lot-tracking-report", "EXP_THIRD_PARTY_INSPECTION_LOT_TRACKING_REPORT", "EXPORT", 859, 136));
        e.add(new Entry("DocDue Color Schedule", "Export", "/export/doc-due-color-schedule", "EXP_DOC_DUE_COLOR_SCHEDULE", "EXPORT", 619, 130));
        e.add(new Entry("Define Chart Of Document", "Export", "/export/define-chart-of-document", "EXP_DEFINE_CHART_OF_DOCUMENT", "EXPORT", 620, 130));
        e.add(new Entry("Define Custom Group", "Export", "/export/define-custom-group", "EXP_DEFINE_CUSTOM_GROUP", "EXPORT", 621, 130));
        e.add(new Entry("Client Assign To Group", "Export", "/export/client-assign-to-group", "EXP_CLIENT_ASSIGN_TO_GROUP", "EXPORT", 622, 130));
        e.add(new Entry("Document Assign To Group", "Export", "/export/document-assign-to-group", "EXP_DOCUMENT_ASSIGN_TO_GROUP", "EXPORT", 623, 130));
        e.add(new Entry("Shipment Doc Schedule", "Export", "/export/shipment-doc-schedule", "EXP_SHIPMENT_DOC_SCHEDULE", "EXPORT", 624, 130));
        e.add(new Entry("Export Document Tracking Report", "Export", "/export/document-tracking-report", "EXP_DOCUMENT_TRACKING_REPORT", "EXPORT", 625, 130));
        e.add(new Entry("Custom / Bank Invoice", "Export", "/export/custom-invoice", "EXP_CUSTOM_INVOICE", "EXPORT", 951, 2047));
        e.add(new Entry("Goods Declaration (GD) / Bank Invoice & GD Mapping", "Export", "/export/gd-bank-invoice-mapping", "EXP_GD_BANK_INVOICE_MAPPING", "EXPORT", 952, 2047));
        e.add(new Entry("Fcy Receipts Utilization Against Bank Invoice / GD", "Export", "/export/fcy-receipt-gd-utilization", "EXP_FCY_RECEIPT_GD_UTILIZATION", "EXPORT", 953, 2047));
        e.add(new Entry("Gd Bank Request", "Export", "/export/gd-bank-request", "EXP_GD_BANK_REQUEST", "EXPORT", 197, 100));
        e.add(new Entry("GD Break Up Manual", "Export", "/export/gd-break-up-manual", "EXP_GD_BREAK_UP_MANUAL", "EXPORT", 198, 100));
        e.add(new Entry("Export Parties Define", "Export", "/export/parties-define", "EXP_PARTIES_DEFINE", "EXPORT", 206, 11));
        e.add(new Entry("Export Bill Of Lading", "Export", "/export/bill-of-lading", "EXP_BILL_OF_LADING", "EXPORT", 213, 11));
        e.add(new Entry("Export Shipping Booking Info", "Export", "/export/shipping-booking-info", "EXP_SHIPPING_BOOKING_INFO", "EXPORT", 214, 11));
        e.add(new Entry("Container Inspection", "Export", "/export/container-inspection", "EXP_CONTAINER_INSPECTION", "EXPORT", 205, 11));
        e.add(new Entry("Pre-Shipment Inspection", "Export", "/export/pre-shipment-inspection", "EXP_PRE_SHIPMENT_INSPECTION", "EXPORT", 188, 11));
        e.add(new Entry("Multi Invoices Allocate To Gdn", "Export", "/export/multi-invoices-allocate-to-gdn", "EXP_MULTI_INVOICES_ALLOCATE_TO_GDN", "EXPORT", 907, 11));
        e.add(new Entry("Advance Utilize Against Invoice", "Export", "/export/advance-utilize-against-invoice", "EXP_ADVANCE_UTILIZE_AGAINST_INVOICE", "EXPORT", 915, 11));
        e.add(new Entry("Export Contract", "Export", "/export/sales-contract", "EXP_SALES_CONTRACT", "EXPORT", 209, 11));
        e.add(new Entry("Export Contract (III)", "Export", "/export/contract-iii", "EXP_CONTRACT_III", "EXPORT", 879, 11));
        e.add(new Entry("Export Contract Schedule", "Export", "/export/contract-schedule", "EXP_CONTRACT_SCHEDULE", "EXPORT", 216, 11));
        e.add(new Entry("Packing Detail By Export Contract", "Export", "/export/packing-detail-by-contract", "EXP_PACKING_DETAIL_BY_CONTRACT", "EXPORT", 240, 11));
        e.add(new Entry("Export Commercial Invoice", "Export", "/export/commercial-invoice", "EXP_COMMERCIAL_INVOICE", "EXPORT", 211, 11));
        e.add(new Entry("Commercial Invoice III", "Export", "/export/commercial-invoice-iii", "EXP_COMMERCIAL_INVOICE_III", "EXPORT", 880, 11));
        e.add(new Entry("Pre Invoice", "Export", "/export/pre-invoice", "EXP_PRE_INVOICE", "EXPORT", 192, 100));
        e.add(new Entry("Packing Detail", "Export", "/export/packing-detail", "EXP_PACKING_DETAIL", "EXPORT", 193, 100));
        e.add(new Entry("Export Opening Balance", "Export", "/export/opening-balance", "EXP_OPENING_BALANCE", "EXPORT", 194, 100));
        e.add(new Entry("Proforma Invoice", "Export", "/export/proforma-invoice", "EXP_PROFORMA_INVOICE", "EXPORT", 215, 100));
        e.add(new Entry("Export Forwarding", "Export", "/export/forwarding", "EXP_FORWARDING", "EXPORT", 212, 11));
        e.add(new Entry("Export Forwarding (New)", "Export", "/export/forwarding-new", "EXP_FORWARDING_NEW", "EXPORT", 793, 100));
        e.add(new Entry("Export Delivery Order", "Export", "/export/delivery-order", "EXP_DELIVERY_ORDER", "EXPORT", 208, 11));
        e.add(new Entry("Forwarding Direct (Not Use)", "Export", "/export/forwarding-direct", "EXP_FORWARDING_DIRECT", "EXPORT", 195, 11));
        e.add(new Entry("Forwarding Without WeighBridge (Not Use)", "Export", "/export/forwarding-without-weighbridge", "EXP_FORWARDING_WITHOUT_WEIGHBRIDGE", "EXPORT", 196, 11));
        e.add(new Entry("Export Lc Order (Not Use)", "Export", "/export/lc-order", "EXP_LC_ORDER", "EXPORT", 210, 11));
        e.add(new Entry("EFrom Registration (Not Use)", "Export", "/export/eform-registration", "EXP_EFORM_REGISTRATION", "EXPORT", 217, 11));
        e.add(new Entry("Financial Insturment (Not Use)", "Export", "/export/financial-instrument", "EXP_FINANCIAL_INSTRUMENT", "EXPORT", 218, 11));
        e.add(new Entry("Packing Material Requirement (Not Use)", "Export", "/export/packing-material-requirement", "EXP_PACKING_MATERIAL_REQUIREMENT", "EXPORT", 219, 11));
        e.add(new Entry("Third Party Inspection", "Export", "/export/third-party-inspection", "EXP_THIRD_PARTY_INSPECTION", "EXPORT", 857, 136));
        e.add(new Entry("Lab Against Third Party Inspection", "Export", "/export/lab-against-third-party-inspection", "EXP_LAB_AGAINST_THIRD_PARTY_INSPECTION", "EXPORT", 858, 136));
        e.add(new Entry("Fcy Receipts", "Export", "/export/fcy-receipts", "EXP_FCY_RECEIPTS", "EXPORT", 794, 100));

        // 7. PRODUCTION REPORTS (module 21)
        // Ids, ScreenName and ScreenAlias all read from reconciliation-input.json, the live dump
        // of GoldenAcedb this repository already holds. The catalog keys on the alias because
        // DesktopScreenRightsService resolves alias first, then ScreenName.
        e.add(new Entry("Job Order Summary Report", "Production", "/production/reports/job-order-summary", "PRODUCTION_RPT_JOB_ORDER_SUMMARY", "PRODUCTION", 975, 21));
        e.add(new Entry("Production Summary Report", "Production", "/production/reports/production-summary", "PRODUCTION_RPT_PRODUCTION_SUMMARY", "PRODUCTION", 309, 21));
        e.add(new Entry("Production Register", "Production", "/production/reports/production-register", "PRODUCTION_RPT_PRODUCTION_REGISTER", "PRODUCTION", 310, 21));
        e.add(new Entry("Production Comparison Report", "Production", "/production/reports/production-comparison", "PRODUCTION_RPT_PRODUCTION_COMPARISON", "PRODUCTION", 308, 21));
        e.add(new Entry("Production PackingMaterial Consumption Register", "Production", "/production/reports/packing-material-consumption", "PRODUCTION_RPT_PM_CONSUMPTION", "PRODUCTION", 306, 21));

        // 8. PACKING MATERIAL (module 54)
        // Id 496 / ScreenName AddItemPM / alias "Item PM" read from reconciliation-input.json.
        e.add(new Entry("Item PM", "Packing Material", "/packing-material/item-pm", "PM_ITEM", "PACKING_MATERIAL", 496, 54));
        e.add(new Entry("Item And PM Item Map", "Packing Material", "/packing-material/item-pm-map", "PM_ITEM_MAP", "PACKING_MATERIAL", 497, 54));
        // Id 498 / ScreenName PurchsaeOrderPmNew / alias "1002 Purchsae Order" - same dump.
        e.add(new Entry("1002 Purchsae Order", "Packing Material", "/packing-material/purchase-order", "PM_PURCHASE_ORDER", "PACKING_MATERIAL", 498, 54));
        e.add(new Entry("Goods Receipt Notes PM", "Packing Material", "/packing-material/grn", "PM_GRN", "PACKING_MATERIAL", 500, 54));
        e.add(new Entry("Purchase Invoice PM", "Packing Material", "/packing-material/purchase-invoice", "PM_PURCHASE_INVOICE", "PACKING_MATERIAL", 501, 54));
        e.add(new Entry("Purchase Invoice Direct PM", "Packing Material", "/packing-material/purchase-invoice-direct", "PM_PURCHASE_INVOICE_DIRECT", "PACKING_MATERIAL", 495, 54));
        e.add(new Entry("Store Send Receipt", "Packing Material", "/packing-material/store-send-receipt", "PM_STORE_SEND_RECEIPT", "PACKING_MATERIAL", 504, 54));
        e.add(new Entry("Stock Transfer Store", "Packing Material", "/packing-material/stock-transfer-store", "PM_STOCK_TRANSFER_STORE", "PACKING_MATERIAL", 505, 54));
        e.add(new Entry("Stock Adjustment For PM", "Packing Material", "/packing-material/stock-adjustment", "PM_STOCK_ADJUSTMENT", "PACKING_MATERIAL", 492, 54));
        e.add(new Entry("Store Stock Conversion", "Packing Material", "/packing-material/store-stock-conversion", "PM_STORE_STOCK_CONVERSION", "PACKING_MATERIAL", 502, 54));
        e.add(new Entry("Delivery Order PM", "Packing Material", "/packing-material/delivery-order", "PM_DELIVERY_ORDER", "PACKING_MATERIAL", 506, 54));
        e.add(new Entry("Sale Invoice Store Direct", "Packing Material", "/packing-material/sale-invoice-direct", "PM_SALE_INVOICE_DIRECT", "PACKING_MATERIAL", 507, 54));

        // 8b. PACKING MATERIAL REPORTS (module 2031)
        e.add(new Entry("Goods Receipt Notes PM Register", "Packing Material Reports", "/packing-material/reports/grn-register", "PM_RPT_GRN_REGISTER", "PACKING_MATERIAL_REPORTS", 697, 2031));
        e.add(new Entry("Packing Material Requirement Planning Detail", "Packing Material Reports", "/packing-material/reports/requirement-planning-detail", "PM_RPT_REQ_PLANNING_DETAIL", "PACKING_MATERIAL_REPORTS", 699, 2031));
        e.add(new Entry("Purchase Order Register_PM", "Packing Material Reports", "/packing-material/reports/purchase-order-register", "PM_RPT_PO_REGISTER", "PACKING_MATERIAL_REPORTS", 778, 2031));
        e.add(new Entry("Inventory Transaction Report", "Packing Material Reports", "/packing-material/reports/inventory-transaction-report", "PM_RPT_INV_TXN", "PACKING_MATERIAL_REPORTS", 695, 2031));
        e.add(new Entry("Stock With Supplier Report", "Packing Material Reports", "/packing-material/reports/stock-with-supplier", "PM_RPT_STOCK_SUPPLIER", "PACKING_MATERIAL_REPORTS", 696, 2031));
        // 9. WEIGH BRIDGE (module 44)
        // Id 411, ScreenName "frmWeightbridge", ScreenAlias "Weigh Bridge", ModuleId 44 - read from
        // migration/user-rights/reconciliation-input.json, the live GoldenAcedb dump, not matched by
        // name.
        e.add(new Entry("Weigh Bridge", "Weigh Bridge", "/weighbridge/weight-bridge", "WEIGH_BRIDGE", "WEIGHBRIDGE", 411, 44));
        // Id 410, ScreenName "WeightbridgeMannual", ScreenAlias "Weigh Bridge Manual", ModuleId 44 - same dump.
        e.add(new Entry("Weigh Bridge Manual", "Weigh Bridge", "/weighbridge/weightbridge-mannual", "WEIGH_BRIDGE_MANUAL", "WEIGHBRIDGE", 410, 44));
        // Weigh bridge reports (module 27 - these two are its only screens). Same dump:
        //   360 frmWeightBridgeHistory "Weigh Bridge Report",
        //   359 frmWeighBridgeRejectedTicketNos "WeighBridge History For Rejected Status".
        e.add(new Entry("Weigh Bridge Report", "Weigh Bridge", "/weighbridge/reports/weight-bridge-history", "WEIGH_BRIDGE_RPT_HISTORY", "WEIGHBRIDGE", 360, 27));
        e.add(new Entry("WeighBridge History For Rejected Status", "Weigh Bridge", "/weighbridge/reports/weigh-bridge-rejected-ticket-nos", "WEIGH_BRIDGE_RPT_REJECTED", "WEIGHBRIDGE", 359, 27));
        // The two weigh bridge lookups have their own ScreenDefinition rows (GoldenAceDb(0509)t.sql):
        //   432 VehicleWeightLookUp "Define Vehicle" module 45, 728 WeighBridgeGeneralLookups "Weigh Bridge Lookups" module 2037.
        e.add(new Entry("Define Vehicle", "Weigh Bridge", "/weighbridge/vehicle-weight-lookup", "WEIGH_BRIDGE_DEFINE_VEHICLE", "WEIGHBRIDGE", 432, 45));
        e.add(new Entry("Weigh Bridge Lookups", "Weigh Bridge", "/weighbridge/weigh-bridge-general-lookups", "WEIGH_BRIDGE_LOOKUPS", "WEIGHBRIDGE", 728, 2037));

        // 9. STORE MANAGEMENT (module 24)
        // Ids / ScreenName / alias read from reconciliation-input.json:
        //   322 frmGSIssuance "Store Issuance", 321 StoreIssuanceDirect "Store Issuance Direct",
        //   330 StoreReturn "Store Return".
        e.add(new Entry("Store Issuance", "Store Management", "/store/store-issuance", "STORE_ISSUANCE", "STORE_MANAGEMENT", 322, 24));
        e.add(new Entry("Store Issuance Direct", "Store Management", "/store/store-issuance-direct", "STORE_ISSUANCE_DIRECT", "STORE_MANAGEMENT", 321, 24));
        e.add(new Entry("Store Return", "Store Management", "/store/store-return", "STORE_RETURN", "STORE_MANAGEMENT", 330, 24));
        //   338 frmDepartmentRequest "Department Request" (DocType 450), 320 frmStockAdjustment "Stock Adjustment" (70),
        //   339 frmStockTransfer "Stock Transfer" (68); Store Purchase module 67: 340 frmPurchaseDemand "Purchase Demand" (141).
        //   ScreenName = the form's base.Name, which is its rights key (CommonServices.SetRightsValueInRightsObject(base.Name)).
        e.add(new Entry("Department Request", "Store Management", "/store/department-request", "STORE_DEPARTMENT_REQUEST", "STORE_MANAGEMENT", 338, 24));
        e.add(new Entry("Stock Adjustment", "Store Management", "/store/stock-adjustment", "STORE_STOCK_ADJUSTMENT", "STORE_MANAGEMENT", 320, 24));
        e.add(new Entry("Stock Transfer", "Store Management", "/store/stock-transfer", "STORE_STOCK_TRANSFER", "STORE_MANAGEMENT", 339, 24));
        e.add(new Entry("Purchase Demand", "Store Purchase", "/store/purchase-demand", "STORE_PURCHASE_DEMAND", "STORE_MANAGEMENT", 340, 67));
        //   Store Purchase module 67, batch 2: 324 GrnStore "Grn Store" (DocType 48), 334 frmPurchaseInvoiceDirectStore
        //   "Purchase Invoice Direct Store" (61), 346 PurchaseInvoiceReturn_Store "Purchase Invoice Return Store" (145),
        //   323 PurchaseInvoiceStoreManagement "Purchase Invoice Store Management" (64) - the real port; the older
        //   /purchase/purchase-invoice-store-management page (DocType 61, save disabled) is not a port of this form.
        e.add(new Entry("Grn Store", "Store Purchase", "/store/grn-store", "STORE_GRN_STORE", "STORE_MANAGEMENT", 324, 67));
        e.add(new Entry("Purchase Invoice Direct Store", "Store Purchase", "/store/purchase-invoice-direct-store", "STORE_PURCHASE_INVOICE_DIRECT", "STORE_MANAGEMENT", 334, 67));
        e.add(new Entry("Purchase Invoice Return Store", "Store Purchase", "/store/purchase-invoice-return-store", "STORE_PURCHASE_INVOICE_RETURN_STORE", "STORE_MANAGEMENT", 346, 67));
        e.add(new Entry("Purchase Invoice Store Management", "Store Purchase", "/store/purchase-invoice-store-management", "STORE_PURCHASE_INVOICE_STORE_MANAGEMENT", "STORE_MANAGEMENT", 323, 67));
        //   Store Management module 24, batch 3 (ScreenNames read from the dump's rights rows):
        //   961 frmPurchasePreBill (147), 960 frmDeliveryChallanAgainstPurchasePreBill (148),
        //   332 frmStockTransferManual (806), 331 DeliveryOrderTransfer (84, Architecture.WinApp.Sale.DeliveryOrderTransfer).
        e.add(new Entry("Store Purchase Pre Bill", "Store Management", "/store/purchase-pre-bill", "STORE_PURCHASE_PRE_BILL", "STORE_MANAGEMENT", 961, 24));
        e.add(new Entry("Delivery Challan Against PreBill", "Store Management", "/store/delivery-challan-prebill", "STORE_DELIVERY_CHALLAN_PREBILL", "STORE_MANAGEMENT", 960, 24));
        e.add(new Entry("Stock Transfer Manual", "Store Management", "/store/stock-transfer-manual", "STORE_STOCK_TRANSFER_MANUAL", "STORE_MANAGEMENT", 332, 24));
        e.add(new Entry("Delivery Order (Stock Transfer)", "Store Management", "/store/delivery-order-transfer", "STORE_DELIVERY_ORDER_TRANSFER", "STORE_MANAGEMENT", 331, 24));
        //   batch 4: 329 AddItemStore; 336 ItemCategoryStore and 337 ItemTypeStore (TargetUrl = the Inventory forms
        //   InvDeffrmItemCatagory / InvDeffrmItemType, Tag ignored); 341 frmStoreOpeningStockBalancing (DocType 39).
        e.add(new Entry("Item Store", "Store Management", "/store/item-store", "STORE_ITEM", "STORE_MANAGEMENT", 329, 24));
        e.add(new Entry("Item Category Store", "Store Management", "/store/item-category-store", "STORE_ITEM_CATEGORY_STORE", "STORE_MANAGEMENT", 336, 24));
        e.add(new Entry("Item Type Store", "Store Management", "/store/item-type-store", "STORE_ITEM_TYPE_STORE", "STORE_MANAGEMENT", 337, 24));
        e.add(new Entry("Opening Stock Store", "Store Management", "/store/opening-stock-store", "STORE_OPENING_STOCK_STORE", "STORE_MANAGEMENT", 341, 24));
        //   batch 5 (the three "(Not Use)" forms, ScreenDefinition aliases verbatim): 335 PartyToPartyPackingMaterialTransfer
        //   (DocType 125; rights key = ScreenName, the form's base.Name has no ScreenDefinition row), 347 (1616), 349 (1615).
        e.add(new Entry("Packing Material Transfer (Party to Party) (Not Use)", "Store Management", "/store/party-to-party-pm-transfer", "STORE_PARTY_TO_PARTY_PM_TRANSFER", "STORE_MANAGEMENT", 335, 24));
        e.add(new Entry("Store Issuance To Cosumable Store (Not Use)", "Store Management", "/store/issuance-to-consumable-store", "STORE_ISSUANCE_TO_CONSUMABLE_STORE", "STORE_MANAGEMENT", 347, 24));
        e.add(new Entry("Department Request To Consumable Store (Not Use)", "Store Management", "/store/department-request-to-consumable", "STORE_DEPARTMENT_REQUEST_TO_CONSUMABLE", "STORE_MANAGEMENT", 349, 24));
        //   Store Management Reports, AppModules 46 (ScreenDefinition rows read from the dump; aliases verbatim).
        e.add(new Entry("Store Purchase Register", "Store Management Reports", "/store/reports/store-purchase-register", "STORE_RPT_STORE_PURCHASE_REGISTER", "STORE_MANAGEMENT", 333, 46));
        e.add(new Entry("Store Purchase Demand Report", "Store Management Reports", "/store/reports/store-purchase-demand-register", "STORE_RPT_STORE_PURCHASE_DEMAND_REGISTER", "STORE_MANAGEMENT", 452, 46));
        e.add(new Entry("Store Issuance Return Report", "Store Management Reports", "/store/reports/store-issuance-return-register", "STORE_RPT_STORE_ISSUANCE_RETURN_REGISTER", "STORE_MANAGEMENT", 453, 46));
        e.add(new Entry("Stock Adjustment Report", "Store Management Reports", "/store/reports/stock-adjustment-register", "STORE_RPT_STOCK_ADJUSTMENT_REGISTER", "STORE_MANAGEMENT", 454, 46));
        e.add(new Entry("Department Request History", "Store Management Reports", "/store/reports/department-request-history", "STORE_RPT_DEPARTMENT_REQUEST_HISTORY", "STORE_MANAGEMENT", 455, 46));
        e.add(new Entry("Stock Transfer Report", "Store Management Reports", "/store/reports/stock-transfer-register", "STORE_RPT_STOCK_TRANSFER_REGISTER", "STORE_MANAGEMENT", 456, 46));
        e.add(new Entry("Store Issuance Report", "Store Management Reports", "/store/reports/store-issuance-history", "STORE_RPT_STORE_ISSUANCE_HISTORY", "STORE_MANAGEMENT", 458, 46));
        e.add(new Entry("General Gate Pass  Report", "Store Management Reports", "/store/reports/general-gate-pass", "STORE_RPT_GENERAL_GATE_PASS", "STORE_MANAGEMENT", 460, 46));

        // 10. LAB (module 7)
        // Ids / ScreenName / alias read from reconciliation-input.json, not name-matched:
        //   155 InvLabSampleLogRegister "Sample Log Register",
        //   156 InvLabAnalysisItems "Item Analysis Parameter",
        //   157 InvLabAnalysisGroup "Analysis Group",
        //   158 InvLabGroupAnalysisStandards "Group Analysis Standards",
        //   159 InvLabSampleAnalysis "Sample Analysis",
        //   160 InvLabPurchaseAnalysis "Purchase Analysis",
        //   162 InvLabAnalysisInProcess "In-Process Analysis",
        //   163 LabInProcessAnalysisStepAndParameterSchedule "InProcess Analysis Steps Schedule".
        // 161 is absent from that dump (it covers one user's grants), so it is left out rather
        // than guessed. Lab reports are module 1011 and are not listed here.
        // Only 156 is built; the rest are deliberately NOT registered, because a registered route
        // with no page behind it is what put unrelated screens behind the same URL elsewhere.
        e.add(new Entry("Item Analysis Parameter", "Lab", "/lab/item-analysis-parameter", "LAB_ITEM_ANALYSIS_PARAMETER", "LAB", 156, 7));
        // Quality Control (App 4): the rest of module 7 "Lab" and module 1011 "Lab Report", all ported 30-Sep / 01-Oct-2026.
        e.add(new Entry("Sample Log Register", "Lab", "/quality/sample-log-register", "LAB_SAMPLE_LOG_REGISTER", "LAB", 155, 7));
        e.add(new Entry("Analysis Group", "Lab", "/quality/analysis-group", "LAB_ANALYSIS_GROUP", "LAB", 157, 7));
        e.add(new Entry("Group Analysis Standards", "Lab", "/quality/group-analysis-standards", "LAB_GROUP_ANALYSIS_STANDARDS", "LAB", 158, 7));
        e.add(new Entry("Sample Analysis", "Lab", "/quality/sample-analysis", "LAB_SAMPLE_ANALYSIS", "LAB", 159, 7));
        e.add(new Entry("Purchase Analysis", "Lab", "/quality/purchase-analysis", "LAB_PURCHASE_ANALYSIS", "LAB", 160, 7));
        e.add(new Entry("Sale Analysis", "Lab", "/quality/sale-analysis", "LAB_SALE_ANALYSIS", "LAB", 161, 7));
        e.add(new Entry("In-Process Analysis", "Lab", "/quality/inprocess-analysis", "LAB_INPROCESS_ANALYSIS", "LAB", 162, 7));
        e.add(new Entry("InProcess Analysis Steps Schedule", "Lab", "/quality/inprocess-analysis-steps", "LAB_INPROCESS_ANALYSIS_STEPS", "LAB", 163, 7));
        e.add(new Entry("Lab Deduction Policy For Purchase", "Lab", "/quality/lab-deduction-policy-for-purchase", "LAB_QC_DEDUCTION_POLICY", "LAB", 164, 7));
        e.add(new Entry("Lab Standard Policy For Deduction (Not Use)", "Lab", "/quality/lab-standard-policy-for-deduction", "LAB_STANDARD_POLICY_FOR_DEDUCTION", "LAB", 166, 7));
        e.add(new Entry("Export Pre Shipment Analysis", "Lab", "/quality/export-pre-shipment-analysis", "LAB_EXPORT_PRE_SHIPMENT_ANALYSIS", "LAB", 168, 7));
        e.add(new Entry("VCI Parameter", "Lab", "/quality/vci-parameter", "LAB_VCI_PARAMETER", "LAB", 809, 7));
        e.add(new Entry("Purchase Analysis By Vehicle", "Lab", "/quality/reports/purchase-analysis-by-vehicle", "LAB_RPT_PURCHASE_ANALYSIS_BY_VEHICLE", "LAB", 626, 1011));
        e.add(new Entry("Lab Purchase Analysis Report (Not Use)", "Lab", "/quality/reports/lab-purchase-analysis-report-old", "LAB_RPT_LAB_PURCHASE_REPORT", "LAB", 627, 1011));
        e.add(new Entry("Lab Sale Analysis Report", "Lab", "/quality/reports/lab-sale-analysis-report", "LAB_RPT_LAB_SALE_REGISTER", "LAB", 628, 1011));
        e.add(new Entry("Lab Sample Analysis Report", "Lab", "/quality/reports/lab-sample-analysis-report", "LAB_RPT_LAB_SAMPLE_ANALYSIS", "LAB", 629, 1011));
        e.add(new Entry("Lab Purchase Analysis Periodic Report", "Lab", "/quality/reports/lab-purchase-analysis-periodic-report", "LAB_RPT_PURCHASE_ANALYSIS_PERIODIC", "LAB", 630, 1011));
        e.add(new Entry("In-Process Analysis Report", "Lab", "/quality/reports/inprocess-analysis-report", "LAB_RPT_INPROCESS_ANALYSIS", "LAB", 631, 1011));
        e.add(new Entry("Sample Analysis Register", "Lab", "/quality/reports/sample-analysis-register", "LAB_RPT_SAMPLE_ANALYSIS_REGISTER", "LAB", 632, 1011));
        e.add(new Entry("Purchase Analylsis Report", "Lab", "/quality/reports/purchase-analysis-report", "LAB_RPT_PURCHASE_ANALYSIS_REGISTER", "LAB", 633, 1011));

        return Collections.unmodifiableList(e);
    }
}
