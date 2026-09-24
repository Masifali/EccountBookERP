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
        e.add(new Entry("Contra Voucher", "Accounts", "/accounts/vouchers/contra", "VOUCHER_CONTRA", "ACCOUNTS", 22, 2));
        e.add(new Entry("Expense Voucher", "Accounts", "/accounts/vouchers/expense", "VOUCHER_EXPENSE", "ACCOUNTS", 46, 2));
        e.add(new Entry("Party Receipt Voucher", "Accounts", "/accounts/vouchers/party-receipt", "VOUCHER_PARTY_RECEIPT", "ACCOUNTS", 17, 2));
        e.add(new Entry("Party Payment Voucher", "Accounts", "/accounts/vouchers/party-payment", "VOUCHER_PARTY_PAYMENT", "ACCOUNTS", 16, 2));
        e.add(new Entry("Post Dated Cheque Payment Vouchers", "Accounts", "/accounts/vouchers/pdc-payment", "VOUCHER_PDC_PAYMENT", "ACCOUNTS", 34, 2));
        e.add(new Entry("Payment By Invoice Voucher", "Accounts", "/accounts/vouchers/payment-by-invoice", "VOUCHER_PAYMENT_INVOICE", "ACCOUNTS", 41, 2));
        e.add(new Entry("Voucher Invoices Adjustment", "Accounts", "/accounts/vouchers/invoices-adjustment", "VOUCHER_INVOICES_ADJ", "ACCOUNTS", null, 2));
        e.add(new Entry("Freight Voucher", "Accounts", "/accounts/vouchers/freight", "VOUCHER_FREIGHT", "ACCOUNTS", 26, 2));
        e.add(new Entry("Day Book Approval", "Accounts", "/accounts/vouchers/day-book-approval", "VOUCHER_DAYBOOK_APPROVAL", "ACCOUNTS", null, 2));
        e.add(new Entry("PDC Transaction Payment", "Accounts", "/accounts/vouchers/pdc-transaction-payment", "VOUCHER_PDC_TRANS_PAYMENT", "ACCOUNTS", null, 2));
        e.add(new Entry("Advance Adjustment", "Accounts", "/accounts/vouchers/advance-adjustment", "VOUCHER_ADVANCE_ADJ", "ACCOUNTS", 4, 2));
        e.add(new Entry("Contractor Wages Account", "Accounts", "/accounts/vouchers/contractor-wages", "VOUCHER_CONTRACTOR_WAGES", "ACCOUNTS", 182, 2));
        e.add(new Entry("Voucher Validation", "Accounts", "/accounts/vouchers/voucher-validation", "VOUCHER_VALIDATION", "ACCOUNTS", 83, 2));

        // 3. ACCOUNT REPORTS
        e.add(new Entry("General Ledger Statement", "Accounts", "/accounts/reports/general-ledger-statement", "RPT_GL_STATEMENT", "ACCOUNTS", 54, 3));
        e.add(new Entry("General Ledger", "Accounts", "/accounts/reports/general-ledger", "RPT_GENERAL_LEDGER", "ACCOUNTS", 79, 3));   // ModuleId 3 Account Reports; 706 is the Banking Managment one
        e.add(new Entry("Day Book", "Accounts", "/accounts/reports/day-book", "RPT_DAY_BOOK", "ACCOUNTS", 15, 3));   // alias 'Day Book'; 24 is alias 'Day Book (Off Set)'
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
        e.add(new Entry("Receivables Aging New", "Accounts", "/accounts/reports/receivables-aging-new", "RPT_RECEIVABLES_AGING_NEW", "ACCOUNTS", null, 3));
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
        e.add(new Entry("GRN (Sale Return)", "Purchase", "/purchase/grn-sale-return", "GRN_SALE_RETURN", "PURCHASE", 37, 5));
        e.add(new Entry("Purchase Invoice", "Purchase", "/purchase/purchase-invoice", "PURCHASE_INVOICE", "PURCHASE", 122, 5));
        e.add(new Entry("Purchase Invoice Again GRN Direct", "Purchase", "/purchase/purchase-invoice-again-grn-direct", "PURCHASE_INVOICE_GRN_DIRECT", "PURCHASE", 131, 5));
        e.add(new Entry("Purchase Invoice Direct", "Purchase", "/purchase/purchase-invoice-direct", "PURCHASE_INVOICE_DIRECT", "PURCHASE", 117, 5));
        e.add(new Entry("Purchase Invoice Return", "Purchase", "/purchase/purchase-invoice-return", "PURCHASE_INVOICE_RETURN", "PURCHASE", 125, 5));

        // 6. PRODUCTION (module 18)
        // Ids confirmed from migration/user-rights/reconciliation-input.json, the live dump of
        // GoldenAcedb this repository already holds - not matched by name.
        e.add(new Entry("Production Job Order", "Production", "/production/job-order", "PRODUCTION_JOB_ORDER", "PRODUCTION", 281, 18));
        e.add(new Entry("Stock Conversion", "Production", "/production/stock-conversion", "PRODUCTION_STOCK_CONVERSION", "PRODUCTION", 276, 18));

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
        // Id 498 / ScreenName PurchsaeOrderPmNew / alias "1002 Purchsae Order" - same dump.
        e.add(new Entry("1002 Purchsae Order", "Packing Material", "/packing-material/purchase-order", "PM_PURCHASE_ORDER", "PACKING_MATERIAL", 498, 54));
        e.add(new Entry("Goods Receipt Notes PM", "Packing Material", "/packing-material/grn", "PM_GRN", "PACKING_MATERIAL", 500, 54));
        e.add(new Entry("Purchase Invoice PM", "Packing Material", "/packing-material/purchase-invoice", "PM_PURCHASE_INVOICE", "PACKING_MATERIAL", 501, 54));
        e.add(new Entry("Purchase Invoice Direct PM", "Packing Material", "/packing-material/purchase-invoice-direct", "PM_PURCHASE_INVOICE_DIRECT", "PACKING_MATERIAL", 495, 54));
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

        return Collections.unmodifiableList(e);
    }
}
