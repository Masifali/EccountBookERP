package com.mst.services;

import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Collections;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * DashBoard module.
 *
 * The desktop's menu is NOT hard-coded - DashboardNew.cs builds every menu item from the database
 * and resolves the form by the row's own TargetUrl:
 *
 *   clsGlobalVariables.ScreenViewReights = GetViewRightsByUserId();            (DashboardNew.cs :1198)
 *      -> tblUserRights.GetUserRightsForViewbyUserId(...)                      (BLL :276-313)
 *      -> USP_GetUserRightsForViewbyUserId  @UserId, @CompanyId [, @AppId, @AppModuleId]
 *
 *   MenuItem_Click  looks the clicked item up by ScreenID and opens Type.GetType(TargetUrl)  (:1553)
 *   ScreenBindInNavigation  keeps only rows where Value is true and TargetUrl is not empty   (:1595)
 *
 * So the DashBoard hub's cards are whatever that procedure returns for this user and company, in
 * the module the row names - the same source, the same per-user rights. Nothing is invented here.
 *
 * Related procedures, kept for reference and used by getModules():
 *   usp_getModulesByCompanyId          @CompanyId, @ModuleTypeId, @UserId
 *   usp_getScreensByModuleId           @ModuleId, @ModuleTypeId, @UserId
 *   usp_getScreenRightsByModuleAndUserId  @ScreenId, @UserId, @AppId
 */
@Service
public class DashboardModuleService {

    private static final Logger LOG = LoggerFactory.getLogger(DashboardModuleService.class);

    /** Not thread-shared state that matters: the hub reads it immediately after the call. */
    private volatile String lastError;

    @Autowired private JdbcTemplate jdbcTemplate;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private ScreenRouteIndex screenRouteIndex;

    /**
     * The desktop reaches this through USP_GetUserRightsForViewbyUserId, but that procedure appears
     * nowhere else in this project and could not be confirmed against the database from here. The
     * same rows are available through the join DesktopUserRightsRepository.filterScreens already
     * uses successfully (tblUserRights -> ScreenDefinition -> AppModules -> CompanyRights), so that
     * proven path is used and extended with TargetUrl and the View right:
     *
     *   ScreenRights holds one row per right per screen; the View right is the one
     *   DashboardNew.cs's ScreenViewReights list is built from, and tblUserRights.Value is the
     *   per-user flag for it. ScreenBindInNavigation (:1595) also drops rows with no TargetUrl.
     */
    private static final String SQL_VIEW_RIGHTS =
            "SELECT DISTINCT S.Id AS ScreenID, S.ScreenName, S.ScreenAlias, S.TargetUrl, "
          + "S.SortNo, AP.ModuleDescription "
          + "FROM tblUserRights UR "
          + "JOIN ScreenDefinition S ON UR.ScreenId = S.Id AND S.IsActive = 1 "
          + "JOIN AppModules AP ON S.ModuleId = AP.Id "
          /* The column is RightId, not RightsID: the C# model property is tblUserRights.RightsID but
             the table column is RightId - resources/sql/user-rights-report.sql, which runs against
             this database, joins ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID. */
          + "JOIN ScreenRights SR ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID "
          + "AND SR.RightName = 'View' "
          + "JOIN CompanyRights CR ON CR.ScreenId = S.Id AND CR.CompanyId = ? "
          + "WHERE UR.CompanyId = ? AND UR.UserId = ? "
          + "AND ISNULL(CR.IsActive, 0) = 1 "
          + "AND ISNULL(UR.Value, 0) = 1 "
          + "AND ISNULL(S.TargetUrl, '') <> '' "
          + "AND AP.ModuleDescription = ? "
          + "ORDER BY S.SortNo, S.ScreenAlias";

    /* The BLL's own module call is usp_getModulesByCompanyId, but DesktopUserRightsRepository
       already uses USP_GetAppModulesByCompany against this database successfully, so the proven
       one is used here rather than a second, unverified procedure name. */
    private static final String SQL_MODULES =
            "EXEC dbo.USP_GetAppModulesByCompany @CompanyId=?, @UserId=?";

    /**
     * Where a ported screen lives on the web, keyed by the desktop TargetUrl's class name (the part
     * after the last dot). A screen that is not in this map is reported as not built, naming its
     * desktop form - the card still appears, because the desktop shows it, but it opens an honest
     * placeholder instead of an unrelated page.
     *
     * The previous hub had six invented cards ("Executive DashBoards", "Followups & Pending Works
     * DashBoards" ...) whose links went to receivables, purchase and quality report pages - screens
     * from other modules entirely.
     */
    private static final Map<String, String> WEB_ROUTES = new LinkedHashMap<>();
    static {
        /* Deliberately empty.
         *
         * Four entries used to live here and all four were wrong:
         *
         *   AcFrmDashboard          -> /accounts/dashboard
         *        No controller maps that path. No @GetMapping("/dashboard") exists under the
         *        @RequestMapping("/accounts") controllers, and requesting it is refused. Several
         *        templates link to it too (index.html :323, modules.html :207,
         *        account/custom_group.html :270) - those links are broken for the same reason.
         *        "Accounts Current Postition (Dashboard)" was showing as built because of it.
         *
         *   SalesAnalytics          -> /business-dashboard
         *   SalesAnalyticsDashBoard -> /business-dashboard
         *   frmSalesAnalytics       -> /business-dashboard
         *        /business-dashboard renders index.html, a generic KPI page written for this web
         *        app. It is not a port of SalesAnalytics.cs (523 lines,
         *        Architecture.WinApp.Dashboard), so sending that screen there breaks the rule that
         *        a screen must never open an unrelated page.
         *
         * A screen is linked only when ScreenRouteIndex finds a REGISTERED route whose name
         * matches it exactly. Add an entry here only for a page that genuinely ports that desktop
         * form and whose name does not match by itself. */
    }

    /**
     * Where a ported screen lives on the web, keyed by ScreenDefinition.Id.
     *
     * ---------------------------------------------------------------------------------------
     * WHY BY ID, AND WHY THESE ARE NOT GUESSES
     * ---------------------------------------------------------------------------------------
     * WEB_ROUTES above is keyed by the desktop class name and is deliberately empty, because the
     * four entries it once held were all guessed from a tile's wording and all four pointed a
     * screen at an unrelated page. The rule that replaced them - link only on an exact route-name
     * match - is right, but it is too strict for a page whose URL reads naturally in English
     * while the desktop class does not:
     *
     *     ScreenName  frmProductionPackingMaterialConsumptionRegister
     *     route       /production/reports/packing-material-consumption
     *
     * Both name the same report; neither normalises to the other. Five Production pages were
     * built, deployed and working while the hub reported them as not built.
     *
     * The id is the strongest key available and the only one that cannot drift: it is the
     * primary key of the row the hub is already rendering. Every entry below is corroborated
     * twice over -
     *
     *   1. the id and its real ScreenName come from the live GoldenAcedb dump the repository
     *      holds (migration/user-rights/reconciliation-input.json), recorded in
     *      claude/PRODUCTION-SCREENS-WIRED-TO-RIGHTS-AND-ALL-8-REAL-IDS-FOUND.md;
     *   2. each controller method already carries that same id in the section comment written
     *      when the page was ported - ProductionReportsController "975 Job Order Summary
     *      Report", "309 Production Summary Report", and so on.
     *
     * So each line below joins two independent records that were written months apart and agree.
     * That is the standard for adding one here: an id confirmed against the database dump AND a
     * route whose controller was demonstrably built from that form. A tile's wording is not
     * evidence, and neither is a plausible-looking URL.
     */
    private static final Map<Integer, String> WEB_ROUTES_BY_SCREEN_ID = new LinkedHashMap<>();
    static {
        /* Purchase, App 3. Module 5 Supplier Purchases and module 52 Purchase Reports; ids and TargetUrls from
           dbo.ScreenDefinition. Rechecked / ported 2026-09-30 (129, 868 and 869 are new pages). */
        WEB_ROUTES_BY_SCREEN_ID.put(120, "/purchase/purchase-order");                  // PurchsaeOrder (41)
        WEB_ROUTES_BY_SCREEN_ID.put(134, "/purchase/inward-gate-pass");                // InwardGatePass (51)
        WEB_ROUTES_BY_SCREEN_ID.put(121, "/purchase/goods-receipt-notes");             // InvFrmGRN (46)
        WEB_ROUTES_BY_SCREEN_ID.put(129, "/purchase/grn-direct");                      // InvFrmGRNDirect (137)
        WEB_ROUTES_BY_SCREEN_ID.put(133, "/purchase/grn-direct-against-order");        // GRNDirectAgainstOrder (169)
        WEB_ROUTES_BY_SCREEN_ID.put(132, "/purchase/purchase-invoice-against-grn-order"); // PurchaseInvoiceAgainstGrnOrder (172)
        WEB_ROUTES_BY_SCREEN_ID.put(910, "/purchase/purchase-invoice-for-upload");     // DataSyncing.frmPendingPurchaseInvoiceForUpload
        WEB_ROUTES_BY_SCREEN_ID.put(866, "/purchase/grn-sale-return");                 // SaleReturnGrn (143)
        WEB_ROUTES_BY_SCREEN_ID.put(122, "/purchase/purchase-invoice");                // InvfrmPurchaseInvoice (56)
        WEB_ROUTES_BY_SCREEN_ID.put(131, "/purchase/purchase-invoice-again-grn-direct"); // frmPurchaseInvoiceAgaintGrnDirect (138)
        WEB_ROUTES_BY_SCREEN_ID.put(117, "/purchase/purchase-invoice-direct");         // InvfrmPurchasedirectInvoice (57)
        WEB_ROUTES_BY_SCREEN_ID.put(125, "/purchase/purchase-invoice-return");         // InvfrmInvPurchaseInvoiceReturn (59)
        WEB_ROUTES_BY_SCREEN_ID.put(868, "/purchase/stock-in-transit");                // frmSupplierDispatchPreBill (251)
        WEB_ROUTES_BY_SCREEN_ID.put(479, "/purchase/reports/purchase-order-register"); // InventoryReports.PurchaseOrderHistory
        WEB_ROUTES_BY_SCREEN_ID.put(478, "/purchase/reports/inward-gate-pass-register"); // Inventory_Reports.frmGatePassReport
        WEB_ROUTES_BY_SCREEN_ID.put(477, "/purchase/reports/grn-register");            // Inventory_Reports.frmGRNHistory
        WEB_ROUTES_BY_SCREEN_ID.put(480, "/purchase/reports/purchase-invoice-register"); // Inventory_Reports.PurchaseRegisterNew
        WEB_ROUTES_BY_SCREEN_ID.put(869, "/purchase/reports/stock-in-transit");        // SupplierPortal.Reports.frmSupplierDispatchPreBillReport

        /* Production, ModuleId 18 */
        WEB_ROUTES_BY_SCREEN_ID.put(281, "/production/job-order");            // frmProductionJobOrderMain
        WEB_ROUTES_BY_SCREEN_ID.put(276, "/production/stock-conversion");     // invfrmStockConversionProduction
        WEB_ROUTES_BY_SCREEN_ID.put(280, "/production/production-against-job-order"); // FoodProductionWithValues
        /* 280 is the SHELL only. Six of its eight tabs host a separate desktop form that is not
           ported; the page names each one rather than pretending the tab works. The route is
           registered because the shell itself is real - the switches, rights and shared pickers
           all come from the database, as frmFoodProduction_Load reads them. */

        /* Production Reports, ModuleId 21 */
        WEB_ROUTES_BY_SCREEN_ID.put(309, "/production/reports/production-summary");            // ProductionSummaryReport
        WEB_ROUTES_BY_SCREEN_ID.put(310, "/production/reports/production-register");           // ProductionRegister
        WEB_ROUTES_BY_SCREEN_ID.put(975, "/production/reports/job-order-summary");             // frmProductionJobOrderSummaryRpt
        WEB_ROUTES_BY_SCREEN_ID.put(308, "/production/reports/production-comparison");         // FoodProductionComparisonRpt
        WEB_ROUTES_BY_SCREEN_ID.put(306, "/production/reports/packing-material-consumption");  // frmProductionPackingMaterialConsumptionRegister

        /* Taxation, ModuleId 9 (App 13). 176 was built under Inventory; the other four were built for
           the Taxation hub on 29-Sep-2026. 172-175 and 177-180 confirmed against the ScreenDefinition
           dump (ModuleId 9); all nine now have a page (174/175/179/180 added 30-Sep). */
        WEB_ROUTES_BY_SCREEN_ID.put(176, "/inventory/item-tax-schedules");               // InvDeffrmItemTaxSchdule
        WEB_ROUTES_BY_SCREEN_ID.put(178, "/taxation/add-tax-type");                      // InvfrmAddTaxType
        WEB_ROUTES_BY_SCREEN_ID.put(177, "/taxation/add-tax-schedule");                  // InvfrmAddTaxSchedule
        WEB_ROUTES_BY_SCREEN_ID.put(172, "/taxation/tax-notes-and-gl-maping");           // frmTaxNotesAndGLMaping
        WEB_ROUTES_BY_SCREEN_ID.put(173, "/taxation/supplier-customer-tax-exemption");   // frmSupplierCustomerExemptionSchedule
        /* 30-Sep-2026: the remaining four. */
        WEB_ROUTES_BY_SCREEN_ID.put(179, "/taxation/items-allocate-to-tax-item");        // RegularItemsAllocateToTaxItem
        WEB_ROUTES_BY_SCREEN_ID.put(175, "/taxation/sale-tax-summary");                  // frmSaleTaxSummaryRpt
        WEB_ROUTES_BY_SCREEN_ID.put(174, "/taxation/wht-challan-deposit");               // frmWhtTaxChallanDeposit (screen 37 under Accounts is the same form)
        WEB_ROUTES_BY_SCREEN_ID.put(180, "/taxation/define-tax-item");                   // DefineTaxItem = InvDefrmAddItem page in taxable mode

        /* Export, App 8, ModuleId 11 - the two rows CompanyRights activates for company 78, built
           2026-09-29 (ExportModuleController); ids confirmed against the ScreenDefinition dump. */
        WEB_ROUTES_BY_SCREEN_ID.put(881, "/export/gd-break-up-by-invoice");   // frmGdBreakUpByInvoice
        WEB_ROUTES_BY_SCREEN_ID.put(882, "/export/invoice-packing-list");     // frmExportInvoicePackingList
        /* Export reports / document tracking / bank-GD groups built 2026-09-30 by the Export port agents A-D. */
        WEB_ROUTES_BY_SCREEN_ID.put(235, "/export/receivable-by-due-date");
        WEB_ROUTES_BY_SCREEN_ID.put(236, "/export/contract-schedule-periodic");
        WEB_ROUTES_BY_SCREEN_ID.put(238, "/export/shipment-costing");
        WEB_ROUTES_BY_SCREEN_ID.put(239, "/export/shipments-document-status");
        WEB_ROUTES_BY_SCREEN_ID.put(249, "/export/detail-history");
        WEB_ROUTES_BY_SCREEN_ID.put(250, "/export/delivery-order-report");
        WEB_ROUTES_BY_SCREEN_ID.put(251, "/export/contract-register");
        WEB_ROUTES_BY_SCREEN_ID.put(254, "/export/shipment-tracking-follow-up");
        WEB_ROUTES_BY_SCREEN_ID.put(261, "/export/shipment-weight-audit");
        WEB_ROUTES_BY_SCREEN_ID.put(262, "/export/loading-sheet");
        WEB_ROUTES_BY_SCREEN_ID.put(266, "/export/shipment-cro-booking-report");
        WEB_ROUTES_BY_SCREEN_ID.put(269, "/export/forwarding-report");
        WEB_ROUTES_BY_SCREEN_ID.put(270, "/export/sale-report");
        WEB_ROUTES_BY_SCREEN_ID.put(759, "/export/sale-comparisons-report");
        WEB_ROUTES_BY_SCREEN_ID.put(912, "/export/service-bill-register");
        WEB_ROUTES_BY_SCREEN_ID.put(913, "/export/forwarding-costing-report");
        WEB_ROUTES_BY_SCREEN_ID.put(919, "/export/shipment-costing-summary");
        WEB_ROUTES_BY_SCREEN_ID.put(935, "/export/fi-utilization-report");
        WEB_ROUTES_BY_SCREEN_ID.put(241, "/export/fcy-receipts-summary-register");
        WEB_ROUTES_BY_SCREEN_ID.put(242, "/export/shipment-data-for-brokery-tax");
        WEB_ROUTES_BY_SCREEN_ID.put(243, "/export/packing-list-register");
        WEB_ROUTES_BY_SCREEN_ID.put(244, "/export/pending-forwarding-for-commercial-invoice");
        WEB_ROUTES_BY_SCREEN_ID.put(245, "/export/gd-break-up-and-realized-register");
        WEB_ROUTES_BY_SCREEN_ID.put(246, "/export/fi-balance-summary");
        WEB_ROUTES_BY_SCREEN_ID.put(256, "/export/commission-agent-fcy-ledger");
        WEB_ROUTES_BY_SCREEN_ID.put(257, "/export/ee-report-export-gd");
        WEB_ROUTES_BY_SCREEN_ID.put(258, "/export/commercial-invoice-shipments");
        WEB_ROUTES_BY_SCREEN_ID.put(203, "/export/consignment-follow-up");
        WEB_ROUTES_BY_SCREEN_ID.put(859, "/export/third-party-inspection-lot-tracking-report");
        WEB_ROUTES_BY_SCREEN_ID.put(619, "/export/doc-due-color-schedule");
        WEB_ROUTES_BY_SCREEN_ID.put(620, "/export/define-chart-of-document");
        WEB_ROUTES_BY_SCREEN_ID.put(621, "/export/define-custom-group");
        WEB_ROUTES_BY_SCREEN_ID.put(622, "/export/client-assign-to-group");
        WEB_ROUTES_BY_SCREEN_ID.put(623, "/export/document-assign-to-group");
        WEB_ROUTES_BY_SCREEN_ID.put(624, "/export/shipment-doc-schedule");
        WEB_ROUTES_BY_SCREEN_ID.put(625, "/export/document-tracking-report");

        /* Master Data Definition, App 19, ModuleId 2039 "System_Level" - all nine confirmed against the
           ScreenDefinition dump (GoldenAceDb(0509)t.sql) on 2026-09-30. */
        WEB_ROUTES_BY_SCREEN_ID.put(749, "/master-data/country");       // frmCountry -> DefineCountry
        WEB_ROUTES_BY_SCREEN_ID.put(750, "/master-data/city");          // frmCity -> DefineCity
        WEB_ROUTES_BY_SCREEN_ID.put(751, "/master-data/currency");      // frmExImCurrency -> DefineMultiCurrency
        WEB_ROUTES_BY_SCREEN_ID.put(752, "/master-data/sea-ports");     // frmExImSeaPorts -> SeaPortsDefine
        WEB_ROUTES_BY_SCREEN_ID.put(753, "/master-data/province");      // frmDefineProvince -> DefineProvince
        WEB_ROUTES_BY_SCREEN_ID.put(754, "/master-data/date-lock");     // DateLock
        WEB_ROUTES_BY_SCREEN_ID.put(755, "/master-data/tehsil");        // DefineTehsil
        WEB_ROUTES_BY_SCREEN_ID.put(756, "/master-data/district");      // DefineDistrict
        WEB_ROUTES_BY_SCREEN_ID.put(742, "/master-data/other-items");   // frmInvDefineOtherItems -> InvOtherItems

        /* Master Data Definition, App 19, ModuleId 45 "Accounts Definition" (the user's second card). 412 is the same
           desktop class as screen 1 (Account_Definition.frmAccountCustomGroup) and opens that page; the other six were
           built on 2026-09-30. Ids confirmed against the ScreenDefinition dump. */
        WEB_ROUTES_BY_SCREEN_ID.put(412, "/accounts/custom_group");         // frmAccountCustomGroup (= screen 1)
        WEB_ROUTES_BY_SCREEN_ID.put(413, "/master-data/bank");              // frmBank -> AcfrmDefineBank
        WEB_ROUTES_BY_SCREEN_ID.put(414, "/master-data/bs-pl-setting");     // BsPlSettingForm
        WEB_ROUTES_BY_SCREEN_ID.put(427, "/master-data/pdc-bank");          // PdcBank
        WEB_ROUTES_BY_SCREEN_ID.put(428, "/master-data/tax-lookup");        // Lookups.TaxLookup
        WEB_ROUTES_BY_SCREEN_ID.put(429, "/master-data/document-group");    // DocumentGroup
        WEB_ROUTES_BY_SCREEN_ID.put(430, "/master-data/shipment-documents"); // ExImShipmentDocuments

        /* Accounts, dbo.App 2 - AppModules 1 Account Definition, 2 Accounts Transaction, 3 Account Reports,
           2032 Banking Managment (30-Sep-2026). /accounts/dashboard used to be a hand-written page with invented
           counts (8/18/27/7) and lists; it now renders App 2 through this renderer like every other hub.
           Ids and TargetUrls from the ScreenDefinition rows of GoldenAceDb(0509)t.sql; each route is the page the
           Accounts recheck (claude/ACCOUNTS-MODULE-FULL-RECHECK-56-SCREENS-...) built from that desktop form.
           Rows sharing one desktop class share its page (28/29 + 703/704 PaymentVoucherNew/ReceiptsVoucherNew,
           24/44 frmDayBook, 79/706 GeneralLedger, 73/702 BankBalances, 709 = 413 AcfrmDefineBank).
           84 BSandPLBreakup has no standalone page (only the Balance Sheet breakup dialog), so it stays unbuilt. */
        WEB_ROUTES_BY_SCREEN_ID.put(1,   "/accounts/custom_group");                    // frmAccountCustomGroup
        WEB_ROUTES_BY_SCREEN_ID.put(2,   "/accounts/allocation");                      // AcfrmAcAllocation
        WEB_ROUTES_BY_SCREEN_ID.put(9,   "/accounts/chart_of_accounts");               // AcfrmDefCoa
        WEB_ROUTES_BY_SCREEN_ID.put(10,  "/accounts/opening_balance");                 // AcfrmOpeningBalance
        WEB_ROUTES_BY_SCREEN_ID.put(11,  "/accounts/supplier");                        // supfrmDefineSupplier
        WEB_ROUTES_BY_SCREEN_ID.put(14,  "/accounts/user-coa-management");             // UserChartOfAccountManagement
        WEB_ROUTES_BY_SCREEN_ID.put(42,  "/accounts/cheque_book");                     // AcfrmChequebookRegistration
        WEB_ROUTES_BY_SCREEN_ID.put(884, "/accounts/bank-reconciliation-upload-excel"); // frmBankReconciliationUploadExcelSheet

        WEB_ROUTES_BY_SCREEN_ID.put(15,  "/accounts/vouchers/day-book");               // DayBook
        WEB_ROUTES_BY_SCREEN_ID.put(16,  "/accounts/vouchers/party-payment");          // frmPartyPaymentVoucher
        WEB_ROUTES_BY_SCREEN_ID.put(17,  "/accounts/vouchers/party-receipt");          // frmPartyReceiptVoucher
        WEB_ROUTES_BY_SCREEN_ID.put(19,  "/accounts/vouchers/voucher-entry");          // VoucherEntry
        WEB_ROUTES_BY_SCREEN_ID.put(22,  "/accounts/vouchers/contra");                 // ContraVoucher
        WEB_ROUTES_BY_SCREEN_ID.put(24,  "/accounts/vouchers/day-book-offset");        // frmDayBook
        WEB_ROUTES_BY_SCREEN_ID.put(44,  "/accounts/vouchers/day-book-offset");        // DayBookVoucher -> frmDayBook
        WEB_ROUTES_BY_SCREEN_ID.put(25,  "/accounts/vouchers/premature-receipts");     // AccountsPrematurePaymentsReceipts
        WEB_ROUTES_BY_SCREEN_ID.put(26,  "/accounts/vouchers/freight");                // FreightVoucher
        WEB_ROUTES_BY_SCREEN_ID.put(28,  "/accounts/vouchers/cash-payment");           // PaymentVoucherNew
        WEB_ROUTES_BY_SCREEN_ID.put(29,  "/accounts/vouchers/bank-payment");           // PaymentVoucherNew
        WEB_ROUTES_BY_SCREEN_ID.put(30,  "/accounts/vouchers/cash-receipt");           // ReceiptsVoucherNew
        WEB_ROUTES_BY_SCREEN_ID.put(31,  "/accounts/vouchers/bank-receipt");           // ReceiptsVoucherNew
        WEB_ROUTES_BY_SCREEN_ID.put(34,  "/accounts/vouchers/pdc-payment");            // PostDatedCheqPaymentVouchers
        WEB_ROUTES_BY_SCREEN_ID.put(38,  "/accounts/vouchers/bills-payables");         // frmBillsPayables
        WEB_ROUTES_BY_SCREEN_ID.put(39,  "/accounts/vouchers/bills-receivables");      // frmBillsReceivables
        WEB_ROUTES_BY_SCREEN_ID.put(41,  "/accounts/vouchers/payment-by-invoice");     // PaymentByInvoiceVoucherNew
        WEB_ROUTES_BY_SCREEN_ID.put(46,  "/accounts/vouchers/expense");                // ExpenseVoucher
        WEB_ROUTES_BY_SCREEN_ID.put(855, "/accounts/vouchers/contra-tax");             // VouchersWithTax.ContraVoucher
        WEB_ROUTES_BY_SCREEN_ID.put(860, "/accounts/vouchers/journal");                // VouchersWithTax.JournalVoucher
        WEB_ROUTES_BY_SCREEN_ID.put(861, "/accounts/vouchers/invoices-adjustment");    // frmInvoicesAdjustmentVoucher
        WEB_ROUTES_BY_SCREEN_ID.put(863, "/accounts/vouchers/receipt-invoices-adjustment"); // frmReceiptInvoicesAdjustmentVoucher
        WEB_ROUTES_BY_SCREEN_ID.put(885, "/accounts/banking/bank-reconciliation");     // frmBankReconciliationWithVouchers

        WEB_ROUTES_BY_SCREEN_ID.put(47,  "/accounts/reports/payables-aging-new");      // PayablesAging_New
        WEB_ROUTES_BY_SCREEN_ID.put(49,  "/accounts/reports/customer-ledger");         // CustomerLedger
        WEB_ROUTES_BY_SCREEN_ID.put(51,  "/accounts/reports/trial-balance");           // TrialBalance
        WEB_ROUTES_BY_SCREEN_ID.put(52,  "/accounts/reports/chart-of-accounts-report"); // CoaTitleChange
        WEB_ROUTES_BY_SCREEN_ID.put(53,  "/accounts/reports/voucher-report");          // VoucherReport
        WEB_ROUTES_BY_SCREEN_ID.put(61,  "/accounts/reports/inventory-payables-receivables"); // InventoryPayablesandReceivables
        WEB_ROUTES_BY_SCREEN_ID.put(62,  "/accounts/reports/balance-sheet");           // BalanceSheet
        WEB_ROUTES_BY_SCREEN_ID.put(64,  "/accounts/reports/profit-and-loss");         // frmProfitLossHararical
        WEB_ROUTES_BY_SCREEN_ID.put(65,  "/accounts/reports/trade-receivables");       // TradeDebitorsReport
        WEB_ROUTES_BY_SCREEN_ID.put(66,  "/accounts/reports/trade-payables");          // TradeDebitorsReport (creditors)
        WEB_ROUTES_BY_SCREEN_ID.put(69,  "/accounts/reports/activity-summary");        // ActicitySummery
        WEB_ROUTES_BY_SCREEN_ID.put(71,  "/accounts/reports/receivables-receipt-schedule"); // ReceivablesAndReceiptSchedule
        WEB_ROUTES_BY_SCREEN_ID.put(73,  "/accounts/reports/bank-balances");           // BankBalances
        WEB_ROUTES_BY_SCREEN_ID.put(74,  "/accounts/reports/cash-balances");           // frmCashBalance
        WEB_ROUTES_BY_SCREEN_ID.put(78,  "/accounts/reports/payables-payment-schedule"); // PayablesAndPaymentSchedule
        WEB_ROUTES_BY_SCREEN_ID.put(79,  "/accounts/reports/general-ledger");          // GeneralLedger
        WEB_ROUTES_BY_SCREEN_ID.put(80,  "/accounts/reports/receivables-report");      // Receivables
        WEB_ROUTES_BY_SCREEN_ID.put(81,  "/accounts/reports/selected-trial-balance");  // SelectedTrialBalance
        WEB_ROUTES_BY_SCREEN_ID.put(82,  "/accounts/reports/trial-balances-all-level"); // frmTrialBalancesAllLevel
        WEB_ROUTES_BY_SCREEN_ID.put(83,  "/accounts/vouchers/voucher-validation");     // VoucherValidation
        WEB_ROUTES_BY_SCREEN_ID.put(88,  "/accounts/reports/pdc-register");            // PostDatedCheqRegister
        WEB_ROUTES_BY_SCREEN_ID.put(89,  "/accounts/reports/subsidiary-payables-report"); // SubsidiaryPayableReceiveables
        WEB_ROUTES_BY_SCREEN_ID.put(769, "/accounts/reports/receivables-new");         // Receivables_New
        WEB_ROUTES_BY_SCREEN_ID.put(787, "/accounts/reports/all-payables");            // frmPayablesReports
        WEB_ROUTES_BY_SCREEN_ID.put(872, "/accounts/reports/customer-aging");          // frmReceivableAgingDocumentWise
        WEB_ROUTES_BY_SCREEN_ID.put(873, "/accounts/reports/supplier-aging");          // frmPayablesAgingDocumentWise
        WEB_ROUTES_BY_SCREEN_ID.put(886, "/accounts/reports/commission-agent-report"); // CommissionAgentLedger
        WEB_ROUTES_BY_SCREEN_ID.put(909, "/accounts/reports/payables-report-invoice-wise"); // frmPayablesReportInvoiceWise
        WEB_ROUTES_BY_SCREEN_ID.put(911, "/accounts/reports/freight-voucher-report");  // FreightVoucherRegister
        WEB_ROUTES_BY_SCREEN_ID.put(958, "/accounts/reports/monthly-profit-loss");     // frmProfitLossMonthWiseComparison

        WEB_ROUTES_BY_SCREEN_ID.put(700, "/accounts/banking/cheque-printing");         // ChequePrinting.ChequePrinting
        WEB_ROUTES_BY_SCREEN_ID.put(702, "/accounts/reports/bank-balances");           // BankBalances
        WEB_ROUTES_BY_SCREEN_ID.put(703, "/accounts/vouchers/bank-payment");           // PaymentVoucherNew
        WEB_ROUTES_BY_SCREEN_ID.put(704, "/accounts/vouchers/bank-receipt");           // ReceiptsVoucherNew
        WEB_ROUTES_BY_SCREEN_ID.put(705, "/accounts/bank-balance-manual-entry");       // BankBalanceManualEntry
        WEB_ROUTES_BY_SCREEN_ID.put(706, "/accounts/reports/general-ledger");          // GeneralLedger
        WEB_ROUTES_BY_SCREEN_ID.put(709, "/master-data/bank");                         // AcfrmDefineBank (= 413)

        /* HRM, App 12 - every ScreenDefinition row of modules 2018-2027, 47 and 29 (GoldenAceDb(0509)t.sql),
           ported 2026-09-30 (project doc HRM-01-...). 650/651 and 662/463 are two rows for one desktop form each. */
        WEB_ROUTES_BY_SCREEN_ID.put(634, "/hrm/employee-group");                          // genEmployeeGroup
        WEB_ROUTES_BY_SCREEN_ID.put(635, "/hrm/employee-category");                       // frmEmployeeCategory
        WEB_ROUTES_BY_SCREEN_ID.put(636, "/hrm/benefit");                                 // BenifitDefine
        WEB_ROUTES_BY_SCREEN_ID.put(637, "/hrm/designation");                             // genDesignation
        WEB_ROUTES_BY_SCREEN_ID.put(638, "/hrm/department");                              // genDepartment
        WEB_ROUTES_BY_SCREEN_ID.put(639, "/hrm/section");                                 // DefineSection
        WEB_ROUTES_BY_SCREEN_ID.put(640, "/hrm/location");                                // frmgenLocation
        WEB_ROUTES_BY_SCREEN_ID.put(641, "/hrm/profile-type");                            // ProfileTypes
        WEB_ROUTES_BY_SCREEN_ID.put(642, "/hrm/define-profile");                          // ProfileDefine
        WEB_ROUTES_BY_SCREEN_ID.put(643, "/hrm/social-security");                         // SocialSecurity
        WEB_ROUTES_BY_SCREEN_ID.put(644, "/hrm/general-policy");                          // GeneralPolicy
        WEB_ROUTES_BY_SCREEN_ID.put(645, "/hrm/salary-breakup-policy");                   // SalaryBreakupPolicy
        WEB_ROUTES_BY_SCREEN_ID.put(646, "/hrm/leave-quota-policy");                      // LeaveQuotaPolicy
        WEB_ROUTES_BY_SCREEN_ID.put(647, "/hrm/eobi-policy");                             // EOBIPolicy
        WEB_ROUTES_BY_SCREEN_ID.put(648, "/hrm/employee-registration");                   // frmEmployeeRegistration
        WEB_ROUTES_BY_SCREEN_ID.put(649, "/hrm/device-configuration");                    // frmDeviceManagement
        WEB_ROUTES_BY_SCREEN_ID.put(650, "/hrm/pull-attendance-by-machine");              // PullAttendanceByMachine (650 and 651 both target this form)
        WEB_ROUTES_BY_SCREEN_ID.put(651, "/hrm/pull-attendance-by-machine");              // PullAttendanceByMachine
        WEB_ROUTES_BY_SCREEN_ID.put(652, "/hrm/define-shift");                            // frmGenShift
        WEB_ROUTES_BY_SCREEN_ID.put(653, "/hrm/shift-location");                          // frmShiftLocation
        WEB_ROUTES_BY_SCREEN_ID.put(654, "/hrm/shift-timing");                            // frmGenShiftTiming
        WEB_ROUTES_BY_SCREEN_ID.put(655, "/hrm/holiday");                                 // frmHoliday
        WEB_ROUTES_BY_SCREEN_ID.put(656, "/hrm/duty-roaster");                            // frmDutyRoasterNew
        WEB_ROUTES_BY_SCREEN_ID.put(657, "/hrm/employee-roaster");                        // frmEmployeeRoaster
        WEB_ROUTES_BY_SCREEN_ID.put(658, "/hrm/daily-attendance");                        // frmDailyAttendance
        WEB_ROUTES_BY_SCREEN_ID.put(659, "/hrm/manual-attendance");                       // hrmManualAttendance
        WEB_ROUTES_BY_SCREEN_ID.put(660, "/hrm/leave-opening");                           // frmEmployeeLeaveOpening
        WEB_ROUTES_BY_SCREEN_ID.put(661, "/hrm/employee-leave");                          // frmEmployeeLeaveRequest
        WEB_ROUTES_BY_SCREEN_ID.put(662, "/hrm/cpl-leave-opening");                       // frmEmployeeCPLLeaveOpening
        WEB_ROUTES_BY_SCREEN_ID.put(463, "/hrm/cpl-leave-opening");                       // frmEmployeeCPLLeaveOpening (same form as 662)
        WEB_ROUTES_BY_SCREEN_ID.put(461, "/hrm/cpl-attendance");                          // frmEmployeeCPLAttendance
        WEB_ROUTES_BY_SCREEN_ID.put(462, "/hrm/cpl-request");                             // frmEmployeeCPLRequest
        WEB_ROUTES_BY_SCREEN_ID.put(663, "/hrm/employee-loan");                           // frmEmployeeLoan
        WEB_ROUTES_BY_SCREEN_ID.put(664, "/hrm/employee-advance");                        // frmEmployeeAdvance
        WEB_ROUTES_BY_SCREEN_ID.put(665, "/hrm/over-time-request");                       // OverTimeRequest
        WEB_ROUTES_BY_SCREEN_ID.put(666, "/hrm/employee-over-time");                      // frmEmployeeOverTime
        WEB_ROUTES_BY_SCREEN_ID.put(667, "/hrm/loan-approval");                           // LoanApproval
        WEB_ROUTES_BY_SCREEN_ID.put(668, "/hrm/leave-approval");                          // LeaveApproval
        WEB_ROUTES_BY_SCREEN_ID.put(669, "/hrm/advance-approval");                        // AdvanceApproval
        WEB_ROUTES_BY_SCREEN_ID.put(670, "/hrm/payroll-posting");                         // frmPayrollPosting
        WEB_ROUTES_BY_SCREEN_ID.put(671, "/hrm/employee-allowance");                      // frmEmployeeAllowance
        WEB_ROUTES_BY_SCREEN_ID.put(672, "/hrm/employee-late-adjustment");                // frmEmployeeLateAdjustment
        WEB_ROUTES_BY_SCREEN_ID.put(673, "/hrm/employee-loan-deduction");                 // EmployeeLoanDeduction
        WEB_ROUTES_BY_SCREEN_ID.put(674, "/hrm/employee-short-adjustment");               // EmployeeShortAdjustment
        WEB_ROUTES_BY_SCREEN_ID.put(371, "/hrm/reports/employee-register");               // EmployeeHistoryRpt
        WEB_ROUTES_BY_SCREEN_ID.put(372, "/hrm/reports/employee-monthly-register");       // HRM_Reports
        WEB_ROUTES_BY_SCREEN_ID.put(373, "/hrm/reports/daily-attendance");                // DailyAttendanceRpt
        WEB_ROUTES_BY_SCREEN_ID.put(374, "/hrm/reports/daily-late-and-early-departure");  // DailyLateandEarlyDeparture
        WEB_ROUTES_BY_SCREEN_ID.put(375, "/hrm/reports/duty-roster-employee-wise");       // genDutyRosterEmployeeWise
        WEB_ROUTES_BY_SCREEN_ID.put(376, "/hrm/reports/employee-attendence");             // genEmployeeAttendence
        WEB_ROUTES_BY_SCREEN_ID.put(377, "/hrm/reports/monthly-attendance-summary");      // MonthlyAttendanceSummary
        WEB_ROUTES_BY_SCREEN_ID.put(378, "/hrm/reports/salary-sheet");                    // PayRollSalarySheetRpt

        /* Packing Material, ModuleId 54 */
        WEB_ROUTES_BY_SCREEN_ID.put(496, "/packing-material/item-pm");        // AddItemPM (Architecture.WinApp.StoreManagement)

        /* Store Management, ModuleId 24 — StoreIssuanceController / StoreReturnController carry the
           same ids in their class notes. */
        WEB_ROUTES_BY_SCREEN_ID.put(322, "/store/store-issuance");            // frmGSIssuance
        WEB_ROUTES_BY_SCREEN_ID.put(321, "/store/store-issuance-direct");     // StoreIssuanceDirect
        WEB_ROUTES_BY_SCREEN_ID.put(330, "/store/store-return");              // StoreReturn
        WEB_ROUTES_BY_SCREEN_ID.put(338, "/store/department-request");        // frmDepartmentRequest
        WEB_ROUTES_BY_SCREEN_ID.put(320, "/store/stock-adjustment");          // frmStockAdjustment
        WEB_ROUTES_BY_SCREEN_ID.put(339, "/store/stock-transfer");            // frmStockTransfer
        /* Store Purchase, ModuleId 67 */
        WEB_ROUTES_BY_SCREEN_ID.put(340, "/store/purchase-demand");           // frmPurchaseDemand
        WEB_ROUTES_BY_SCREEN_ID.put(324, "/store/grn-store");                 // GrnStore
        WEB_ROUTES_BY_SCREEN_ID.put(334, "/store/purchase-invoice-direct-store");      // frmPurchaseInvoiceDirectStore
        WEB_ROUTES_BY_SCREEN_ID.put(346, "/store/purchase-invoice-return-store");      // PurchaseInvoiceReturn_Store
        WEB_ROUTES_BY_SCREEN_ID.put(323, "/store/purchase-invoice-store-management");  // PurchaseInvoiceStoreManagement (the real port)
        WEB_ROUTES_BY_SCREEN_ID.put(961, "/store/purchase-pre-bill");         // frmPurchasePreBill
        WEB_ROUTES_BY_SCREEN_ID.put(960, "/store/delivery-challan-prebill");  // frmDeliveryChallanAgainstPurchasePreBill
        WEB_ROUTES_BY_SCREEN_ID.put(332, "/store/stock-transfer-manual");     // frmStockTransferManual
        WEB_ROUTES_BY_SCREEN_ID.put(331, "/store/delivery-order-transfer");   // DeliveryOrderTransfer
        WEB_ROUTES_BY_SCREEN_ID.put(329, "/store/item-store");                // AddItemStore
        WEB_ROUTES_BY_SCREEN_ID.put(336, "/store/item-category-store");       // ItemCategoryStore → InvDeffrmItemCatagory
        WEB_ROUTES_BY_SCREEN_ID.put(337, "/store/item-type-store");           // ItemTypeStore → InvDeffrmItemType
        WEB_ROUTES_BY_SCREEN_ID.put(341, "/store/opening-stock-store");       // frmStoreOpeningStockBalancing
        WEB_ROUTES_BY_SCREEN_ID.put(335, "/store/party-to-party-pm-transfer"); // frmPackingMaterialTransferPartyToParty
        WEB_ROUTES_BY_SCREEN_ID.put(347, "/store/issuance-to-consumable-store"); // frmStoreIssuanceToCosumableStore
        WEB_ROUTES_BY_SCREEN_ID.put(349, "/store/department-request-to-consumable"); // DepartmentRequestToConsumableStore
        WEB_ROUTES_BY_SCREEN_ID.put(333, "/store/reports/store-purchase-register"); // StorePurchaseRegister
        WEB_ROUTES_BY_SCREEN_ID.put(452, "/store/reports/store-purchase-demand-register"); // StorePurchaseDemandRegister
        WEB_ROUTES_BY_SCREEN_ID.put(453, "/store/reports/store-issuance-return-register"); // StoreIssuanceReturnRegister
        WEB_ROUTES_BY_SCREEN_ID.put(454, "/store/reports/stock-adjustment-register"); // StockAdjustmentRegister
        WEB_ROUTES_BY_SCREEN_ID.put(455, "/store/reports/department-request-history"); // DepartmentRequestHistory
        WEB_ROUTES_BY_SCREEN_ID.put(456, "/store/reports/stock-transfer-register"); // StockTransferRegister
        WEB_ROUTES_BY_SCREEN_ID.put(458, "/store/reports/store-issuance-history"); // StoreIssuenceHistory
        WEB_ROUTES_BY_SCREEN_ID.put(460, "/store/reports/general-gate-pass");      // frmGatePassGeneral

        /* Quality Control (dbo.App 4): AppModules 7 "Lab" and 1011 "Lab Report". Every ScreenDefinition
           row of both modules is mapped to its own page (ported from Architecture.WinApp.Lab / QCL /
           Export / Lookups). These lines were lost once when another change overwrote this file
           (2026-09-30 17:59) - keep them when merging. */
        WEB_ROUTES_BY_SCREEN_ID.put(155, "/quality/sample-log-register");        // InvLabSampleLogRegister
        WEB_ROUTES_BY_SCREEN_ID.put(156, "/lab/item-analysis-parameter");        // InvLabAnalysisItems
        WEB_ROUTES_BY_SCREEN_ID.put(157, "/quality/analysis-group");             // InvLabAnalysisGroup
        WEB_ROUTES_BY_SCREEN_ID.put(158, "/quality/group-analysis-standards");   // InvLabGroupAnalysisStandards
        WEB_ROUTES_BY_SCREEN_ID.put(159, "/quality/sample-analysis");            // InvLabSampleAnalysis (302)
        WEB_ROUTES_BY_SCREEN_ID.put(160, "/quality/purchase-analysis");          // InvLabPurchaseAnalysis
        WEB_ROUTES_BY_SCREEN_ID.put(162, "/quality/inprocess-analysis");         // InvLabAnalysisInProcess
        WEB_ROUTES_BY_SCREEN_ID.put(163, "/quality/inprocess-analysis-steps");   // LabInProcessAnalysisStepAndParameterSchedule
        WEB_ROUTES_BY_SCREEN_ID.put(626, "/quality/reports/purchase-analysis-by-vehicle");          // LabDataVehicleWiseByParent
        WEB_ROUTES_BY_SCREEN_ID.put(629, "/quality/reports/lab-sample-analysis-report");            // InvLabSampleRegister
        WEB_ROUTES_BY_SCREEN_ID.put(630, "/quality/reports/lab-purchase-analysis-periodic-report"); // LabPurchaseAnalyticPeriodic
        WEB_ROUTES_BY_SCREEN_ID.put(631, "/quality/reports/inprocess-analysis-report");             // InProcessLabAnalysisRegister
        WEB_ROUTES_BY_SCREEN_ID.put(632, "/quality/reports/sample-analysis-register");              // SampleAnalysisRegister (InvLabSampleRegister)
        WEB_ROUTES_BY_SCREEN_ID.put(633, "/quality/reports/purchase-analysis-report");              // InvLabPurchaseRegister
        /*QC-NEW-ROUTES*/
        WEB_ROUTES_BY_SCREEN_ID.put(498, "/packing-material/purchase-order"); // PurchsaeOrderPmNew (Architecture.WinApp.PackingMaterial_Store)
        WEB_ROUTES_BY_SCREEN_ID.put(500, "/packing-material/grn"); // GrnPackingMaterial (Architecture.WinApp.PackingMaterial_Store)
        WEB_ROUTES_BY_SCREEN_ID.put(501, "/packing-material/purchase-invoice"); // PurchaseInvoicePackingMaterial
        WEB_ROUTES_BY_SCREEN_ID.put(495, "/packing-material/purchase-invoice-direct"); // frmPurchaseInvoiceDirectPM
        WEB_ROUTES_BY_SCREEN_ID.put(504, "/packing-material/store-send-receipt"); // StoreSendReceipt
        WEB_ROUTES_BY_SCREEN_ID.put(505, "/packing-material/stock-transfer-store"); // frmStockTransferStore
        WEB_ROUTES_BY_SCREEN_ID.put(492, "/packing-material/stock-adjustment"); // StockAdjustmentForPM
        WEB_ROUTES_BY_SCREEN_ID.put(502, "/packing-material/store-stock-conversion"); // StoreStockConversion
        WEB_ROUTES_BY_SCREEN_ID.put(506, "/packing-material/delivery-order"); // DeliveryOrderPackingMaterial
        WEB_ROUTES_BY_SCREEN_ID.put(507, "/packing-material/sale-invoice-direct"); // FrmSaleInvoiceDirectPackingMaterial
        WEB_ROUTES_BY_SCREEN_ID.put(497, "/packing-material/item-pm-map"); // frmItemAndPMItemMap (Architecture.WinApp.PackingMaterial_Store)

        /* Packing Material Reports, ModuleId 2031 */
        /* 30-Sep-2026: the five applications that had no Java port. Ids and TargetUrls from the
           ScreenDefinition rows of GoldenAceDb(0509)t.sql; each page names its desktop form. */
        /* Fixed Assets - dbo.App 9 */
        WEB_ROUTES_BY_SCREEN_ID.put(353, "/fixed-assets/assets-register");        // AssetSchema.frmAssetsRegister
        WEB_ROUTES_BY_SCREEN_ID.put(356, "/fixed-assets/define-assets");          // FixedAsset.frmDefineAssets
        WEB_ROUTES_BY_SCREEN_ID.put(918, "/fixed-assets/fixed-asset-purchase");   // Account_Definition.frmFixedAssetPurchase
        /* Upload documents - dbo.App 23 */
        WEB_ROUTES_BY_SCREEN_ID.put(473, "/upload-documents/folder-structure");   // DMS_FolderHierarchy.CreateFolderStructure
        /* Logistics Management - dbo.App 14 */
        WEB_ROUTES_BY_SCREEN_ID.put(233, "/logistics/service-bill-direct");        // Service.ExImClearingAgentBillDirect
        WEB_ROUTES_BY_SCREEN_ID.put(234, "/logistics/define-services-item");       // Service.EximServicesDefine
        WEB_ROUTES_BY_SCREEN_ID.put(937, "/logistics/rate-negotiation");           // lgstcm.frmLogisticRateNegotiation
        WEB_ROUTES_BY_SCREEN_ID.put(939, "/logistics/agreement");                  // lgstcm.frmLogisticAgreement
        WEB_ROUTES_BY_SCREEN_ID.put(941, "/logistics/rate-negotiation-transporter"); // lgstcm.frmLogisticRateNegotiationTransporter
        WEB_ROUTES_BY_SCREEN_ID.put(949, "/logistics/purchase-order");             // lgstcm.frmLogisticPurchaseOrder
        WEB_ROUTES_BY_SCREEN_ID.put(955, "/logistics/services-bill");              // lgstcm.frmLogisticPurchaseServicesBill
        WEB_ROUTES_BY_SCREEN_ID.put(965, "/logistics/freight-voucher-export");     // lgstcm.frmFreightVoucherExport
        WEB_ROUTES_BY_SCREEN_ID.put(967, "/logistics/freight-voucher-export-multi"); // lgstcm.frmFreightVoucherExportMultiVehicles
        /* Import - dbo.App 15 */
        WEB_ROUTES_BY_SCREEN_ID.put(221, "/import/commercial-invoice");            // Import.ImCommercialInvoice
        WEB_ROUTES_BY_SCREEN_ID.put(525, "/import/lc-order");                      // Import.ImLcOrder
        WEB_ROUTES_BY_SCREEN_ID.put(526, "/import/lc-order-schedule");             // Import.ImLcOrderSchedule
        WEB_ROUTES_BY_SCREEN_ID.put(392, "/import/reports/invoice-register");      // ImportReports.ImportInvoiceRegister
        WEB_ROUTES_BY_SCREEN_ID.put(393, "/import/reports/contract-register");     // ImportReports.ImportContractRegister
        WEB_ROUTES_BY_SCREEN_ID.put(394, "/import/reports/grn-register");          // ImportReports.ImportGrnRegister
        WEB_ROUTES_BY_SCREEN_ID.put(395, "/import/reports/purchase-register");     // ImportReports.ImportPurchaseRegister
        WEB_ROUTES_BY_SCREEN_ID.put(782, "/import/proforma-invoice");              // Import.frmProformaInvoice
        WEB_ROUTES_BY_SCREEN_ID.put(783, "/import/import-invoice");                // Import.frmImportInvoice
        WEB_ROUTES_BY_SCREEN_ID.put(784, "/import/invoice-packing-detail");        // Import.Transactions.frmInvoicePackingDetail
        WEB_ROUTES_BY_SCREEN_ID.put(785, "/import/shipment-booking");              // Import.Transactions.frmShipmentBooking
        /* Party Processing - dbo.App 16. 679/680, 681/682, 687/688, 692/693 are two rows for one
           desktop form each (GRN / GDN mode); each row opens its own mode through ?mode=. */
        WEB_ROUTES_BY_SCREEN_ID.put(683, "/party-processing/define-item");         // DefineItemPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(684, "/party-processing/stock-party-define");  // frmPartyProcessingDefineSupplier
        WEB_ROUTES_BY_SCREEN_ID.put(685, "/party-processing/item-category");       // DefItemCatagoryPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(686, "/party-processing/item-type");           // DefPartyProcessingItemType
        WEB_ROUTES_BY_SCREEN_ID.put(689, "/party-processing/reference-parties");   // DefineReferenceParties
        WEB_ROUTES_BY_SCREEN_ID.put(678, "/party-processing/stock-opening-balance"); // StockOpeningBalancePartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(690, "/party-processing/reports/grn-info");    // frmPartyProcessingGrnInfo
        WEB_ROUTES_BY_SCREEN_ID.put(691, "/party-processing/reports/gate-pass");   // frmGatePassPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(692, "/party-processing/reports/grn-register?mode=grn"); // frmGRNGDNPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(693, "/party-processing/reports/grn-register?mode=gdn"); // frmGRNGDNPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(694, "/party-processing/reports/stock-report"); // frmStockReportPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(613, "/party-processing/grn-purchase");        // GRNForPurchaseFromPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(615, "/party-processing/gdn-sale");            // GDNForSaleToPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(679, "/party-processing/grn-gdn-store?mode=grn"); // frmGrnGdnStorePartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(680, "/party-processing/grn-gdn-store?mode=gdn"); // frmGrnGdnStorePartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(681, "/party-processing/gate-pass?mode=inward");  // GatePassInwardPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(682, "/party-processing/gate-pass?mode=outward"); // GatePassInwardPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(687, "/party-processing/grn-gdn?mode=grn");    // PartyProGrnAndGdn
        WEB_ROUTES_BY_SCREEN_ID.put(688, "/party-processing/grn-gdn?mode=gdn");    // PartyProGrnAndGdn
        WEB_ROUTES_BY_SCREEN_ID.put(875, "/party-processing/advance-delivery-order"); // AdvanceDeliveryOrderPP
        WEB_ROUTES_BY_SCREEN_ID.put(602, "/party-processing/stock-conversion");    // invfrmStockConversionPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(603, "/party-processing/stock-transfer");      // PartyProcessing.StockTransfer
        WEB_ROUTES_BY_SCREEN_ID.put(604, "/party-processing/stock-adjustment");    // PartyProcessing.StockAdjustment
        WEB_ROUTES_BY_SCREEN_ID.put(614, "/party-processing/wages-bill");          // frmWagesBillPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(675, "/party-processing/processing-bill");     // ProductionPartyProcessingBill
        WEB_ROUTES_BY_SCREEN_ID.put(676, "/party-processing/production-job-order"); // ProductionJobOrderPartyProcessing
        WEB_ROUTES_BY_SCREEN_ID.put(677, "/party-processing/production");          // frmProductionPartyProcessing

        WEB_ROUTES_BY_SCREEN_ID.put(697, "/packing-material/reports/grn-register");
        WEB_ROUTES_BY_SCREEN_ID.put(699, "/packing-material/reports/requirement-planning-detail");
        WEB_ROUTES_BY_SCREEN_ID.put(778, "/packing-material/reports/purchase-order-register");
        WEB_ROUTES_BY_SCREEN_ID.put(695, "/packing-material/reports/inventory-transaction-report");
        WEB_ROUTES_BY_SCREEN_ID.put(696, "/packing-material/reports/stock-with-supplier");
    }

    /** The desktop forms behind the DashBoard menu, for the placeholder to name (DashboardNew.cs :5161-5196). */
    private static final Map<String, String> DESKTOP_FORMS = new LinkedHashMap<>();
    static {
        DESKTOP_FORMS.put("PendingVouchersForApproval",   "PendingVouchersForApproval.cs");
        DESKTOP_FORMS.put("frmPendingWorksRpt",           "frmPendingWorksRpt.cs");
        DESKTOP_FORMS.put("frmStockDashboard",            "frmStockDashboard.cs");
        DESKTOP_FORMS.put("btnStocksDashBoard",           "frmStockDashboard.cs");
        DESKTOP_FORMS.put("UnApprovedVouchersDashBoard",  "UnApprovedInvoicesAndVouchers.cs");
        DESKTOP_FORMS.put("UnApprovedInvoicesAndVouchers","UnApprovedInvoicesAndVouchers.cs");
        DESKTOP_FORMS.put("OrderManagementDashboard",     "OrderManagementDashboard.cs");
        DESKTOP_FORMS.put("frmPaddyPurchaseAnalytics",    "PurchaseAnalytiicsDashBoard.cs");
        DESKTOP_FORMS.put("frmRicePurchaseAnalytics",     "PurchaseAnalytiicsDashBoard.cs");
    }

    /**
     * Cards for one module, straight from the database.
     *
     * @param moduleName matched case-insensitively against the row's ModuleDescription
     */
    public List<Map<String, Object>> getModuleScreens(String moduleName) {
        lastError = null;
        List<Map<String, Object>> cards = new ArrayList<>();
        try {
            int compId = currentUserContext.currentCompanyId();
            int userId = currentUserContext.currentUserId();

            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    SQL_VIEW_RIGHTS, compId, compId, userId, moduleName)) {

                String targetUrl = str(col(r, "TargetUrl"));
                if (targetUrl.isEmpty()) continue;          // belt and braces; the SQL filters it too

                String cls = targetUrl.contains(".")
                        ? targetUrl.substring(targetUrl.lastIndexOf('.') + 1)
                        : targetUrl;

                Map<String, Object> card = new LinkedHashMap<>();
                card.put("screenId", col(r, "ScreenID"));
                card.put("screenName", str(col(r, "ScreenName")));
                // ScreenAlias is the caption the desktop puts on the menu item and the form
                card.put("title", str(col(r, "ScreenAlias")).isEmpty()
                        ? str(col(r, "ScreenName")) : str(col(r, "ScreenAlias")));
                card.put("module", str(col(r, "ModuleDescription")));
                card.put("targetUrl", targetUrl);
                card.put("iconUrl", "");
                card.put("isFavorite", false);

                /* One resolver for all three card builders. This site used to consult
                   WEB_ROUTES alone, so it could not see a route the registered-route index or
                   the screen-id map had found - the same screen reported "built" on one page of
                   the hub and "not built" on another. */
                String route = routeFor(r);
                card.put("built", route != null);
                card.put("route", route != null
                        ? route
                        : "/dashboard/screen?name=" + urlEncode(cls));
                card.put("desktopForm", DESKTOP_FORMS.getOrDefault(cls, cls + ".cs"));
                cards.add(card);
            }
        } catch (Exception e) {
            /* An empty list and a failed query are NOT the same thing: the first means the screens
               are not allocated, the second means the page is broken. The hub must be able to tell
               the user which, so the message is kept rather than swallowed. */
            LOG.error("Dashboard module screens failed for module '{}'", moduleName, e);
            lastError = e.getMessage();
            return Collections.emptyList();
        }
        return cards;
    }

    /**
     * Why is the list empty?
     *
     * The main query joins five tables and applies five predicates, so an empty result has a dozen
     * possible causes and the page cannot guess. This walks the same joins one step at a time and
     * reports where the rows disappear, plus the module names that ARE available - the usual cause
     * being that AppModules.ModuleDescription does not read exactly "DashBoard".
     *
     * Runs only when the main query returned nothing, and never throws: a diagnostic that fails
     * must not replace the answer it is diagnosing.
     */
    public Map<String, Object> diagnose(String moduleName) {
        Map<String, Object> d = new LinkedHashMap<>();
        int compId, userId;
        try {
            compId = currentUserContext.currentCompanyId();
            userId = currentUserContext.currentUserId();
        } catch (Exception e) {
            d.put("error", "No signed-in user/company context: " + e.getMessage());
            return d;
        }
        d.put("companyId", compId);
        d.put("userId", userId);
        d.put("moduleAskedFor", moduleName == null ? "(all modules)" : moduleName);

        String base = "FROM tblUserRights UR "
                    + "JOIN ScreenDefinition S ON UR.ScreenId = S.Id AND S.IsActive = 1 "
                    + "JOIN AppModules AP ON S.ModuleId = AP.Id "
                    + "JOIN ScreenRights SR ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID "
                    + "AND SR.RightName = 'View' "
                    + "JOIN CompanyRights CR ON CR.ScreenId = S.Id AND CR.CompanyId = ? ";

        count(d, "1. rights rows for this user+company",
              "SELECT COUNT(*) FROM tblUserRights WHERE CompanyId = ? AND UserId = ?", compId, userId);
        count(d, "2. + active screen",
              "SELECT COUNT(*) FROM tblUserRights UR JOIN ScreenDefinition S ON UR.ScreenId = S.Id "
            + "AND S.IsActive = 1 WHERE UR.CompanyId = ? AND UR.UserId = ?", compId, userId);
        count(d, "3. + module join",
              "SELECT COUNT(*) FROM tblUserRights UR JOIN ScreenDefinition S ON UR.ScreenId = S.Id "
            + "AND S.IsActive = 1 JOIN AppModules AP ON S.ModuleId = AP.Id "
            + "WHERE UR.CompanyId = ? AND UR.UserId = ?", compId, userId);
        count(d, "4. + View right row",
              "SELECT COUNT(*) FROM tblUserRights UR JOIN ScreenDefinition S ON UR.ScreenId = S.Id "
            + "AND S.IsActive = 1 JOIN AppModules AP ON S.ModuleId = AP.Id "
            + "JOIN ScreenRights SR ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID "
            + "AND SR.RightName = 'View' WHERE UR.CompanyId = ? AND UR.UserId = ?", compId, userId);
        count(d, "5. + company allocation", "SELECT COUNT(*) " + base
            + "WHERE UR.CompanyId = ? AND UR.UserId = ?", compId, compId, userId);
        count(d, "6. + CompanyRights.IsActive = 1", "SELECT COUNT(*) " + base
            + "WHERE UR.CompanyId = ? AND UR.UserId = ? AND ISNULL(CR.IsActive, 0) = 1",
              compId, compId, userId);
        count(d, "7. + View right granted (UR.Value = 1)", "SELECT COUNT(*) " + base
            + "WHERE UR.CompanyId = ? AND UR.UserId = ? AND ISNULL(CR.IsActive, 0) = 1 "
            + "AND ISNULL(UR.Value, 0) = 1", compId, compId, userId);
        count(d, "8. + TargetUrl present", "SELECT COUNT(*) " + base
            + "WHERE UR.CompanyId = ? AND UR.UserId = ? AND ISNULL(CR.IsActive, 0) = 1 "
            + "AND ISNULL(UR.Value, 0) = 1 AND ISNULL(S.TargetUrl, '') <> ''", compId, compId, userId);
        /* Step 9 only applies when a single module was asked for. The hub no longer filters by
           module - there is no "DashBoard" module on the desktop either - so it passes null. */
        if (moduleName != null && !moduleName.isEmpty()) {
            count(d, "9. + module = '" + moduleName + "'", "SELECT COUNT(*) " + base
                + "WHERE UR.CompanyId = ? AND UR.UserId = ? AND ISNULL(CR.IsActive, 0) = 1 "
                + "AND ISNULL(UR.Value, 0) = 1 AND ISNULL(S.TargetUrl, '') <> '' "
                + "AND AP.ModuleDescription = ?", compId, compId, userId, moduleName);
        }

        /* The decisive one: what module names does this user actually have screens under? */
        List<String> modules = new ArrayList<>();
        try {
            for (Map<String, Object> r : jdbcTemplate.queryForList(
                    "SELECT AP.ModuleDescription, COUNT(*) AS Screens " + base
                  + "WHERE UR.CompanyId = ? AND UR.UserId = ? AND ISNULL(CR.IsActive, 0) = 1 "
                  + "AND ISNULL(UR.Value, 0) = 1 AND ISNULL(S.TargetUrl, '') <> '' "
                  + "GROUP BY AP.ModuleDescription ORDER BY AP.ModuleDescription",
                    compId, compId, userId)) {
                modules.add(str(col(r, "ModuleDescription")) + " (" + str(col(r, "Screens")) + ")");
            }
        } catch (Exception e) {
            modules.add("could not be listed: " + e.getMessage());
        }
        d.put("modulesAvailableToYou", modules);
        return d;
    }

    private void count(Map<String, Object> into, String label, String sql, Object... args) {
        try {
            Integer n = jdbcTemplate.queryForObject(sql, Integer.class, args);
            into.put(label, n == null ? 0 : n);
        } catch (Exception e) {
            into.put(label, "failed: " + e.getMessage());
        }
    }


    /**
     * The whole menu this user can see, exactly as DashboardNew.InitializeMenu() builds it (:1445).
     *
     * The desktop does NOT have a module called "DashBoard" - the btnDashBoard tree in the
     * designer (:5159) is dead code, because InitializeMenu() starts by clearing both context
     * menus and rebuilds every item from the rights rows:
     *
     *     ModuleTypeId == 1  ->  ContextMenuStrip1   (the Screens menu)
     *     ModuleTypeId == 2  ->  contextMenuStrip2   (the Reports menu)
     *
     * grouped by { ModuleID, ModuleDescription }, each group's children being the rows with that
     * ModuleID and Value = true, captioned by ScreenAlias and opened by ScreenID.
     *
     * So the web hub shows the same two groups of modules, and a screen's link is resolved by
     * ScreenRouteIndex against the routes this application really registered - never invented.
     *
     * @return one entry per module: moduleTypeId, moduleId, moduleDescription, sortNo, screens
     */
    public List<Map<String, Object>> getMenu() {
        lastError = null;
        List<Map<String, Object>> modules = new ArrayList<>();
        try {
            int compId = currentUserContext.currentCompanyId();
            int userId = currentUserContext.currentUserId();

            Map<String, Map<String, Object>> byModule = new LinkedHashMap<>();

            for (Map<String, Object> r : jdbcTemplate.queryForList(SQL_MENU, compId, compId, userId)) {
                String targetUrl = str(col(r, "TargetUrl"));
                if (targetUrl.isEmpty()) continue;

                String moduleId = str(col(r, "ModuleId"));
                Map<String, Object> mod = byModule.get(moduleId);
                if (mod == null) {
                    mod = new LinkedHashMap<>();
                    mod.put("moduleId", col(r, "ModuleId"));
                    mod.put("moduleTypeId", asInt(col(r, "ModuleTypeId")));
                    mod.put("moduleDescription", str(col(r, "ModuleDescription")));
                    mod.put("sortNo", asInt(col(r, "ModuleSortNo")));
                    mod.put("screens", new ArrayList<Map<String, Object>>());
                    byModule.put(moduleId, mod);
                    modules.add(mod);
                }

                String screenName = str(col(r, "ScreenName"));
                String alias = str(col(r, "ScreenAlias"));
                String route;

                String cls = targetUrl.contains(".")
                        ? targetUrl.substring(targetUrl.lastIndexOf('.') + 1) : targetUrl;
                /* Same single resolver: screen id, then the explicit class map, then the
                   registered-route index. */
                route = routeFor(r);

                Map<String, Object> sc = new LinkedHashMap<>();
                sc.put("screenId", col(r, "ScreenID"));
                sc.put("screenName", screenName);
                sc.put("title", alias.isEmpty() ? screenName : alias);
                sc.put("targetUrl", targetUrl);
                sc.put("built", route != null);
                sc.put("route", route != null
                        ? route
                        : "/dashboard/screen?name=" + urlEncode(cls));
                sc.put("desktopForm", DESKTOP_FORMS.getOrDefault(cls, cls + ".cs"));

                @SuppressWarnings("unchecked")
                List<Map<String, Object>> screens = (List<Map<String, Object>>) mod.get("screens");
                screens.add(sc);
            }
        } catch (Exception e) {
            LOG.error("Dashboard menu read failed", e);
            lastError = e.getMessage();
            return Collections.emptyList();
        }
        return modules;
    }

    /**
     * Same joins as SQL_VIEW_RIGHTS, minus the single-module filter and carrying the module's own
     * type and sort order, because InitializeMenu() groups by ModuleTypeId then ModuleID.
     * AppModules.ModuleTypeId / SortNo are columns of that table - Architecture.Model.AppModules
     * declares AppId, Id, ModuleTypeId, SortNo, IconUrl, ModelMenuControllName, ModuleDescription,
     * and GenericProvider maps that model one property per column.
     */
    private static final String SQL_MENU =
            "SELECT DISTINCT S.Id AS ScreenID, S.ScreenName, S.ScreenAlias, S.TargetUrl, "
          + "S.SortNo, AP.Id AS ModuleId, AP.ModuleDescription, ISNULL(AP.AppId, 0) AS AppId, "
          + "ISNULL(AP.ModuleTypeId, 0) AS ModuleTypeId, ISNULL(AP.SortNo, 0) AS ModuleSortNo "
          + "FROM tblUserRights UR "
          + "JOIN ScreenDefinition S ON UR.ScreenId = S.Id AND S.IsActive = 1 "
          + "JOIN AppModules AP ON S.ModuleId = AP.Id "
          + "JOIN ScreenRights SR ON UR.RightId = SR.Id AND UR.ScreenId = SR.ScreenID "
          + "AND SR.RightName = 'View' "
          + "JOIN CompanyRights CR ON CR.ScreenId = S.Id AND CR.CompanyId = ? "
          + "WHERE UR.CompanyId = ? AND UR.UserId = ? "
          + "AND ISNULL(CR.IsActive, 0) = 1 "
          + "AND ISNULL(UR.Value, 0) = 1 "
          + "AND ISNULL(S.TargetUrl, '') <> '' "
          + "ORDER BY ISNULL(AP.ModuleTypeId, 0), ISNULL(AP.SortNo, 0), AP.ModuleDescription, "
          + "S.SortNo, S.ScreenAlias";

    private static int asInt(Object o) {
        if (o == null) return 0;
        if (o instanceof Number) return ((Number) o).intValue();
        try { return Integer.parseInt(String.valueOf(o).trim()); } catch (Exception e) { return 0; }
    }

    /** How many page routes this application registered - shown when the hub has nothing to show. */
    public int routeCount() { return screenRouteIndex.size(); }


    // ================================================================ the DashBoard screen itself

    /**
     * The rights rows the desktop menu is built from - its own procedure, not a rebuilt join.
     *
     *   DashboardNew.GetViewRightsByUserId()            (:1791-1812)
     *     -> tblUserRights.GetUserRightsForViewbyUserId (BLL :276-313)
     *        USP_GetUserRightsForViewbyUserId @UserId, @CompanyId
     *            [, @AppId]        only when non-zero
     *            [, @AppModuleId]  only when non-zero
     *
     * The rows come back as Architecture.Model.tblUserRights, which GenericProvider maps one
     * property per column, so every column the menu needs is on them: AppId, App, ModuleID,
     * ModuleDescription, ModuleTypeId, IconUrl, ScreenID, ScreenName, ScreenAlias, TargetUrl,
     * Value, IsFavorite.
     *
     * THIS is why the desktop works where the web did not. Earlier versions of this class rebuilt
     * the join by hand (tblUserRights -> ScreenDefinition -> AppModules -> ScreenRights ->
     * CompanyRights) and took AppId from AppModules, which is not the same AppId the rights rows
     * carry. Same user, same company, same database - different column. The procedure is the
     * desktop's own and is demonstrably working, so it is the source here too; the hand-built
     * join stays only as a fallback if the procedure fails, and the page says which was used.
     */
    public List<Map<String, Object>> viewRights() {
        List<String> names = new ArrayList<>();
        List<Object> args = new ArrayList<>();
        names.add("@UserId");    args.add(currentUserContext.currentUserId());
        names.add("@CompanyId"); args.add(currentUserContext.currentCompanyId());
        /* BLL :291-297 appends @AppId only when it is non-zero; DashboardNew passes
           UserAccount.AppId, or 0 when it is 3. Omitting a parameter is not the same as
           passing NULL, so it is left off entirely when it would be zero. */
        int appId = 0;
        try { appId = currentUserContext.currentAppId(); } catch (Exception ignored) { }
        if (appId != 0 && appId != 3) { names.add("@AppId"); args.add(appId); }

        StringBuilder sql = new StringBuilder("EXEC USP_GetUserRightsForViewbyUserId ");
        for (int i = 0; i < names.size(); i++) {
            if (i > 0) sql.append(", ");
            sql.append(names.get(i)).append("=?");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : jdbcTemplate.queryForList(sql.toString(), args.toArray())) {
            /* ScreenBindInNavigation (:1595) keeps only granted rows that have a TargetUrl. */
            if (!truthy(col(r, "Value"))) continue;
            if (str(col(r, "TargetUrl")).isEmpty()) continue;
            out.add(r);
        }
        return out;
    }

    /** The rights rows, from the procedure when it works and from the proven join when it does not. */
    private List<Map<String, Object>> rights() {
        lastError = null;
        rightsSource = "USP_GetUserRightsForViewbyUserId";
        try {
            return viewRights();
        } catch (Exception e) {
            LOG.error("USP_GetUserRightsForViewbyUserId failed; falling back to the rebuilt join", e);
            rightsSource = "rebuilt join (procedure failed: " + e.getMessage() + ")";
        }
        try {
            return jdbcTemplate.queryForList(SQL_MENU,
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentCompanyId(),
                    currentUserContext.currentUserId());
        } catch (Exception e) {
            LOG.error("Rebuilt join failed too", e);
            lastError = e.getMessage();
            return Collections.emptyList();
        }
    }

    private volatile String rightsSource = "";

    /** Which source the last read used - shown on the page so the two can never be confused. */
    public String getRightsSource() { return rightsSource; }

    /**
     * frmModules groups the rights rows by { AppId, App } (:69-98). "DashBoard" is one of those
     * App values, carried on the rows themselves.
     */
    public Map<String, Object> getAppByName(String appName) {
        for (Map<String, Object> r : rights()) {
            if (squash(str(col(r, "App"))).equals(squash(appName))) {
                Map<String, Object> out = new LinkedHashMap<>();
                out.put("appId", asInt(col(r, "AppId")));
                out.put("app", str(col(r, "App")));
                return out;
            }
        }
        return null;
    }

    /**
     * When no App matches, print what the rows actually carry - the distinct { AppId, App } pairs
     * and the modules inside each - so the right name is visible instead of an empty page.
     */
    public Map<String, Object> parentDiagnostics(String appName) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("lookedFor", appName);
        d.put("source", rightsSource);
        try {
            List<Map<String, Object>> rows = rights();
            d.put("rowsReturned", rows.size());
            Map<String, List<String>> byApp = new LinkedHashMap<>();
            for (Map<String, Object> r : rows) {
                String key = str(col(r, "App")) + " [AppId " + asInt(col(r, "AppId")) + "]";
                String m = str(col(r, "ModuleDescription"));
                List<String> l = byApp.computeIfAbsent(key, k -> new ArrayList<>());
                if (!l.contains(m)) l.add(m);
            }
            List<String> groups = new ArrayList<>();
            for (Map.Entry<String, List<String>> e : byApp.entrySet()) {
                groups.add(e.getKey() + " (" + e.getValue().size() + " modules) -> "
                         + String.join(", ", e.getValue()));
            }
            d.put("appGroups", groups);
            if (!rows.isEmpty()) d.put("columnsReturned", new ArrayList<>(rows.get(0).keySet()));
        } catch (Exception e) {
            d.put("appGroups", Collections.singletonList("could not be read: " + e.getMessage()));
        }
        return d;
    }

    /**
     * frmMenue.DynamicallyGenerateCardsForModule() (:201-244) - one card per module under the
     * application clicked, captioned by ModuleDescription, numbered by its screen count.
     */
    /**
     * frmModules.DynamicallyGenerateCardsForApp() (:69-98) — the desktop's LEVEL 1: one card per
     * application this user may view, captioned by the App name and numbered by how many screens
     * it contains.
     *
     * This level was missing from the port entirely. Without it there is no way to reach an
     * application that has no hand-written landing page of its own — Admin Panel and System
     * Utilities among them — because every module page was written one at a time by hand.
     */
    public List<Map<String, Object>> getAppCards() {
        List<Map<String, Object>> cards = new ArrayList<>();
        Map<String, Map<String, Object>> byApp = new LinkedHashMap<>();
        for (Map<String, Object> r : rights()) {
            int appId = asInt(col(r, "AppId"));
            String key = String.valueOf(appId);
            Map<String, Object> a = byApp.get(key);
            if (a == null) {
                a = new LinkedHashMap<>();
                a.put("appId", appId);
                a.put("title", str(col(r, "App")));
                a.put("count", 0);
                a.put("built", 0);
                a.put("modules", new java.util.LinkedHashSet<Integer>());
                byApp.put(key, a);
                cards.add(a);
            }
            a.put("count", asInt(a.get("count")) + 1);
            if (routeFor(r) != null) a.put("built", asInt(a.get("built")) + 1);
            @SuppressWarnings("unchecked")
            java.util.Set<Integer> mods = (java.util.Set<Integer>) a.get("modules");
            mods.add(asInt(moduleIdOf(r)));
        }
        for (Map<String, Object> a : cards) {
            @SuppressWarnings("unchecked")
            java.util.Set<Integer> mods = (java.util.Set<Integer>) a.remove("modules");
            a.put("moduleCount", mods.size());
        }
        return cards;
    }

    public List<Map<String, Object>> getModuleCards(int appId) {
        List<Map<String, Object>> cards = new ArrayList<>();
        Map<String, Map<String, Object>> byModule = new LinkedHashMap<>();
        for (Map<String, Object> r : rights()) {
            if (asInt(col(r, "AppId")) != appId) continue;
            String moduleId = String.valueOf(asInt(moduleIdOf(r)));
            Map<String, Object> m = byModule.get(moduleId);
            if (m == null) {
                m = new LinkedHashMap<>();
                m.put("moduleId", asInt(moduleIdOf(r)));
                m.put("moduleTypeId", asInt(col(r, "ModuleTypeId")));
                m.put("title", str(col(r, "ModuleDescription")));
                m.put("count", 0);
                m.put("built", 0);
                byModule.put(moduleId, m);
                cards.add(m);
            }
            m.put("count", asInt(m.get("count")) + 1);
            if (routeFor(r) != null) m.put("built", asInt(m.get("built")) + 1);
        }
        return cards;
    }

    /**
     * frmMenue.DynamicallyGenerateCards(ModId) (:84-113) - one card per screen of one module,
     * captioned by ScreenAlias. The module is checked against the application too, so a
     * hand-edited id cannot reach screens the user was not allocated.
     */
    public Map<String, Object> getScreenCards(int appId, int moduleId) {
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> screens = new ArrayList<>();
        String moduleTitle = "";
        for (Map<String, Object> r : rights()) {
            if (asInt(col(r, "AppId")) != appId) continue;
            if (asInt(moduleIdOf(r)) != moduleId) continue;
            moduleTitle = str(col(r, "ModuleDescription"));
            screens.add(screenCard(r));
        }
        out.put("moduleTitle", moduleTitle);
        out.put("screens", screens);
        return out;
    }

    /** The procedure spells it ModuleID; the rebuilt join aliases it ModuleId. Accept either. */
    private static Object moduleIdOf(Map<String, Object> r) {
        Object v = col(r, "ModuleID");
        return v != null ? v : col(r, "ModuleId");
    }

    /** One screen card, with its web route resolved or the placeholder that names its desktop form. */
    private Map<String, Object> screenCard(Map<String, Object> r) {
        String targetUrl = str(col(r, "TargetUrl"));
        String cls = targetUrl.contains(".")
                ? targetUrl.substring(targetUrl.lastIndexOf('.') + 1) : targetUrl;
        String alias = str(col(r, "ScreenAlias"));
        String route = routeFor(r);

        Map<String, Object> sc = new LinkedHashMap<>();
        sc.put("screenId", col(r, "ScreenID"));
        sc.put("screenName", str(col(r, "ScreenName")));
        sc.put("title", alias.isEmpty() ? str(col(r, "ScreenName")) : alias);
        sc.put("targetUrl", targetUrl);
        sc.put("built", route != null);
        sc.put("route", route != null ? route : "/dashboard/screen?name=" + urlEncode(cls));
        sc.put("desktopForm", DESKTOP_FORMS.getOrDefault(cls, cls + ".cs"));
        return sc;
    }

    /** The explicit map wins; otherwise the registered-route index, which never guesses a URL. */
    private String routeFor(Map<String, Object> r) {
        /* The id wins over everything: it is the row's own primary key, and every entry in the
           map is corroborated against both the live database dump and the controller that was
           built for it. Checked before TargetUrl because a screen can be built even when its
           TargetUrl is blank. */
        Integer screenId = asIntOrNull(col(r, "ScreenID"));
        if (screenId == null) screenId = asIntOrNull(col(r, "ScreenId"));
        if (screenId != null) {
            String byId = WEB_ROUTES_BY_SCREEN_ID.get(screenId);
            if (byId != null) return byId;
        }

        String targetUrl = str(col(r, "TargetUrl"));
        if (targetUrl.isEmpty()) return null;
        String cls = targetUrl.contains(".")
                ? targetUrl.substring(targetUrl.lastIndexOf('.') + 1) : targetUrl;
        String explicit = WEB_ROUTES.get(cls);
        if (explicit != null) return explicit;
        return screenRouteIndex.routeFor(str(col(r, "ScreenName")), targetUrl);
    }

    private static Integer asIntOrNull(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return null;
        try {
            return Integer.valueOf(String.valueOf(v).trim());
        } catch (NumberFormatException e) {
            return null;
        }
    }

    /** Lower-case, letters and digits only - "DashBoard", "Dash Board" and "dashboard" all match. */
    private static String squash(String s) {
        if (s == null) return "";
        StringBuilder b = new StringBuilder();
        for (int i = 0; i < s.length(); i++) {
            char c = s.charAt(i);
            if (Character.isLetterOrDigit(c)) b.append(Character.toLowerCase(c));
        }
        return b.toString();
    }

    /** Message from the last getModuleScreens call that threw, or null if it did not. */
    public String getLastError() { return lastError; }

    /** usp_getModulesByCompanyId - the module list the desktop's own hub is built from. */
    public List<Map<String, Object>> getModules() {
        try {
            return jdbcTemplate.queryForList(SQL_MODULES,
                    currentUserContext.currentCompanyId(), currentUserContext.currentUserId());
        } catch (Exception e) {
            LOG.error("Module list failed", e);
            return Collections.emptyList();
        }
    }

    /** The desktop form a not-yet-ported screen belongs to, for the placeholder page. */
    /**
     * Whether this class is one of the DashBoard menu's own forms.
     *
     * /dashboard/screen is the placeholder for EVERY module's unbuilt screen, not just the
     * DashBoard module's - screenCard() sends them all there. It used to state, for all of them,
     * that the form lives in `Architecture.WinApp.Dashboard`, which is false for most: screen 280
     * `FoodProductionWithValues` is in Architecture.WinApp.Production, and the page said
     * otherwise. DESKTOP_FORMS is the list of forms the DashBoard menu actually owns
     * (DashboardNew.cs:5161-5196), so it is the only thing that can honestly answer this.
     *
     * When the answer is no, the page names the form and says nothing about the assembly, rather
     * than naming the wrong one.
     */
    public boolean isDashboardOwnForm(String className) {
        return className != null && DESKTOP_FORMS.containsKey(className);
    }

    public String desktopFormFor(String className) {
        if (className == null || className.isEmpty()) return null;
        return DESKTOP_FORMS.getOrDefault(className, className + ".cs");
    }

    // ---------------------------------------------------------------- helpers

    private static Object col(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object o) { return o == null ? "" : String.valueOf(o).trim(); }

    private static boolean truthy(Object o) {
        if (o == null) return false;
        if (o instanceof Boolean) return (Boolean) o;
        if (o instanceof Number) return ((Number) o).intValue() != 0;
        String s = String.valueOf(o).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }

    private static String urlEncode(String s) {
        try { return java.net.URLEncoder.encode(s, "UTF-8"); }
        catch (Exception e) { return s; }
    }
}
