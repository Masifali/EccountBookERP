/* ============================================================================================
 * Screen 490 OrdersWithLedgerBalance - Architecture.WinApp.Inventory_Reports.OrdersWithLedgerBalance (.cs, TabControl with 3 tabs)
 * Page: templates/sale/reports/sarpt1_orders_with_ledger_balance.html (route /sale/reports/orders-with-ledger-balance).
 *   OrdersWithLedgerBalance_Load :171   suppliercustomer() then DateTypeFill(); focus on cmbperemeter
 *   DateTypeFill :185                   1 This Day .. 5 Financial Year, 6 All (Id, Parameters), ZeroIndex false, Rows[5] (All) on all three tabs
 *   suppliercustomer :211               SupplierCustomer.SupplierCustomerAgainstSaleOrder = Usp_SupplierCustomerAgainstSaleOrder (Id, CompanyName, GlAccountId) -> CmbSupplierCustomer and
 *                                       cmbCustomerCurrLedger (ZeroIndex false, the Customer Name column 350 wide)
 *   Tab 1 "Date Wise Legder Balance"    cmbperemeter_ValueChanged :264 (1 From = today | 2 today - 7 | 3 first of UTC month, To = today | 4 1 Jan, To = today | 5 Start_Period, each then
 *                                       GetLedgerReport), btnshow_Click :306, GetLedgerReport :318 (needs a customer: the active row of CmbSupplierCustomer; ActionId 1; the dates only when
 *                                       the date type is not 6), GridSetting :388 (CustomerGlId / Id / SuppCustId hidden, widths, PartyName link, sums "#,##0.##",
 *                                       LedgerBalance / RunningBalance "#,#;(#,#);0"), grd_LinkClicked :457 (general ledger of CustomerGlId from Start_Period to now),
 *                                       btnPrint_Click :477 (146-OutstandingOrderWithLedgerBalance.rpt over the grid rows), btnNew_Click :250 (Rows[0] + its handler), btnRefresh_Click :238
 *   Tab 2 "Current Legder Balance"      cmbperemeterCurrLedger_ValueChanged :543, GetCurrentLedgerReport :597 (ActionId 2; the guard looks at the customer of TAB 1 as the desktop does,
 *                                       the customer sent is that of tab 2), GridCurrLedgerSetting :667 (+ GridAutoAdjustmentNew), grdCurrLedger_LinkClicked :725, print :745, new :529, refresh :517
 *   Tab 3 "Party Wise Summary"          CmbDateTypePartyWiseTab_ValueChanged :943 (dates only), btnShowPartyWiseTab_Click :832 / PartyWiseSummary :844 (ActionId 3, no customer),
 *                                       GridSettingPartyWiseTab :892 (Qty "#,##0.###", Weight / Amount "#,##0.####", ledger columns "#,#;(#,#);0", autosize),
 *                                       grdPartyWise_LinkClicked :981 (general ledger of GlAccountId for the tab dates), btnPrintPartyWiseTab_Click :810 (199-GetOutstandingOrdersWithLedgerBalance.rpt),
 *                                       btnNewPartyWiseTab_Click :797 (Rows[0], ClearStructure)
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/sarpt1/orders-with-ledger-balance';
    var lookup = { yearStart: null }, customers = [];
    var NUM = '#,##0.##', BAL = '#,#;(#,#);0';

    // ------------------------------------------------------------------ tabs
    var TABS = {
        1: { name: 'Date Wise Legder Balance', type: 'cmbperemeter', from: 'txtFromDate', to: 'txtToDate', cust: 'CmbSupplierCustomer', show: 'show1', neu: 'new1', refresh: 'refresh1', print: 'print1',
             page: 'page1', rows: [], args: null, auto: true, grid: null },
        2: { name: 'Current Legder Balance', type: 'cmbperemeterCurrLedger', from: 'txtFromDateCurrLedger', to: 'txtToDateCurrLedger', cust: 'cmbCustomerCurrLedger', show: 'show2', neu: 'new2',
             refresh: 'refresh2', print: 'print2', page: 'page2', rows: [], args: null, auto: true, grid: null },
        3: { name: 'Party Wise Summary', type: 'CmbDateTypePartyWiseTab', from: 'FromDatePartyWiseTab', to: 'ToDatePartyWiseTab', cust: null, show: 'show3', neu: 'new3', refresh: null,
             print: 'print3', page: 'page3', rows: [], args: null, auto: false, grid: null }
    };
    function mkGrid(n) {
        return new S.Grid({ tableId: 'results' + n, gridId: 'grid' + n, navId: 'nav' + n, navTextId: 'navText' + n, headerLines: 2, autosize: n !== 1,
            onLink: function (row) { openLedger(n, row); } });
    }
    [1, 2, 3].forEach(function (n) { TABS[n].grid = mkGrid(n); });

    function openLedger(n, row) {                                              // CommonServices.GoToGeneralLedgerFromLinkedEvent
        var acc, from, to;
        if (n === 3) { acc = A.toInt(A.ci(row, 'GlAccountId')); from = el(TABS[3].from).value; to = el(TABS[3].to).value; }
        else { acc = A.toInt(A.ci(row, 'CustomerGlId')); from = lookup.yearStart ? S.isoDay(lookup.yearStart) : ''; to = A.today(); }
        if (acc <= 0) { alert('No ledger account is linked to this customer.'); return; }
        w.open('/accounts/reports/general-ledger?' + S.qs({ accountId: acc, fromDate: from, toDate: to }), '_blank');
    }

    // ------------------------------------------------------------------ columns
    function colsDetail(n) {
        var t1 = n === 1;
        function num(k, wd) { return { key: k, caption: k, width: t1 ? wd : undefined, fmt: NUM, totalFmt: NUM, sum: true, num: true }; }
        return [
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'DocDate', caption: 'DocDate', width: t1 ? 80 : undefined, date: 'dd-MMM-yy' },
            { key: 'DocNo', caption: 'DocNo', width: t1 ? 60 : undefined }, { key: 'SuppCustId', caption: 'SuppCustId', hidden: true },
            { key: 'CustomerName', caption: 'PartyName', width: t1 ? 200 : undefined, link: true },
            num('OrderQty', 80), num('DispatchQty', 80), num('BalQty', 80), num('OrderWeight', 90), num('DispatchWeight', 90), num('BalWeight', 80),
            num('OrderAmount', 90), num('DispatchAmount', 90), num('BalAmount', 80),
            { key: 'LedgerBalance', caption: 'LedgerBalance', fmt: BAL, num: true }, { key: 'RunningBalance', caption: 'RunningBalance', fmt: BAL, num: true },
            { key: 'Remarks', caption: 'Remarks' }, { key: 'CustomerGlId', caption: 'CustomerGlId', hidden: true }
        ];
    }
    function mapDetail(r) {
        var c = A.ci;
        return { Id: c(r, 'Id'), DocDate: c(r, 'DocDate'), DocNo: c(r, 'DocNo'), SuppCustId: c(r, 'OrderSupCustId'), CustomerName: c(r, 'CustomerName'), OrderQty: c(r, 'OrderQty'),
            DispatchQty: c(r, 'DispatchQty'), BalQty: c(r, 'BalQty'), OrderWeight: c(r, 'OrderWeight'), DispatchWeight: c(r, 'DispatchWeight'), BalWeight: c(r, 'BalWeight'),
            OrderAmount: c(r, 'OrderAmount'), DispatchAmount: c(r, 'DispatchAmount'), BalAmount: c(r, 'OrderBalAmount'), LedgerBalance: c(r, 'LedgerBalance'),
            RunningBalance: c(r, 'RunningBalance'), Remarks: c(r, 'RemarksHeader'), CustomerGlId: c(r, 'CustomerGlId') };
    }
    function colsParty() {
        return [
            { key: 'GlAccountId', caption: 'GlAccountId', hidden: true }, { key: 'SuppCustId', caption: 'SuppCustId', hidden: true },
            { key: 'CustomerName', caption: 'PartyName', link: true },
            { key: 'Qty', caption: 'Qty', fmt: '#,##0.###', totalFmt: '#,##0.###', sum: true, num: true },
            { key: 'Weight', caption: 'Weight', fmt: '#,##0.####', totalFmt: '#,##0.####', sum: true, num: true },
            { key: 'Amount', caption: 'Amount', fmt: '#,##0.####', totalFmt: '#,##0.####', sum: true, num: true },
            { key: 'LedgerBalance', caption: 'LedgerBalance', fmt: BAL, num: true }, { key: 'Balance', caption: 'Balance', fmt: BAL, num: true }
        ];
    }
    function mapParty(r) {
        var c = A.ci;
        return { GlAccountId: c(r, 'GlAccountId'), SuppCustId: c(r, 'OrderSupCustId'), CustomerName: c(r, 'CustomerName'), Qty: c(r, 'BalQty'), Weight: c(r, 'BalWeight'),
            Amount: c(r, 'OrderBalAmount'), LedgerBalance: c(r, 'LedgerBalance'), Balance: c(r, 'RunningBalance') };
    }

    // ------------------------------------------------------------------ show
    var seq = { 1: 0, 2: 0, 3: 0 };
    function hasCustomer(id) { var s = el(id); return !!s && s.value !== '' && s.selectedIndex >= 0; }
    function getReport(n) {                                                    // GetLedgerReport / GetCurrentLedgerReport / PartyWiseSummary
        var t = TABS[n];
        if (n !== 3 && !hasCustomer(TABS[1].cust)) { t.rows = []; t.args = null; t.grid.clear(); return Promise.resolve(); }   // the desktop checks CmbSupplierCustomer on tab 2 as well
        var a = { tab: n, dateType: S.selInt(t.type) };
        if (a.dateType !== 6) { a.fromDate = el(t.from).value; a.toDate = el(t.to).value; }
        if (n !== 3) a.supplierCustomerId = S.selInt(t.cust);
        var token = ++seq[n], b = el(t.show);
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq[n]) return;
            if (!rows || !rows.length) { t.rows = []; t.args = null; t.grid.clear(); return; }
            t.args = a;
            t.rows = rows;
            if (n === 3) t.grid.setData(colsParty(), rows.map(mapParty)); else t.grid.setData(colsDetail(n), rows.map(mapDetail));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq[n]) A.busy(b, false); });
    }
    function print(n) {
        var t = TABS[n], b = el(t.print); if (b.disabled) return;
        if (!t.rows.length || !t.args) { alert('Record Not Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/sale/reports/sarpt1/print/orders-with-ledger-balance?' + S.qs(t.args)).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ date types
    function fillTypes() {
        [1, 2, 3].forEach(function (n) {
            S.fill(el(TABS[n].type), S.dateTypeRows(true), false);
            S.activate(TABS[n].type, 5, false);                                // Rows[5] = All (the change handlers see type 6: no Show)
        });
    }
    function onType(n) {
        var t = TABS[n], id = S.selInt(t.type);
        S.dateRule(id, el(t.from), el(t.to), lookup.yearStart);
        if (n === 3) { if (id === 1) { el(t.from).value = A.today(); el(t.to).value = A.today(); } return; }
        if (id >= 1 && id <= 5) getReport(n);                                  // each type 1..5 runs the report
    }
    function fillCustomers() {
        var rows = customers;
        if (!rows.length) return;                                              // dt.Rows.Count > 0
        S.fill(el(TABS[1].cust), rows, false, true);
        S.fill(el(TABS[2].cust), rows, false, true);
    }
    function loadCustomers() { return A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; customers = (data && data.customers) || []; fillCustomers(); }); }

    function neu(n) {                                                          // btnNew: Rows[0] and its handler (tab 3: also ClearStructure)
        var t = TABS[n];
        S.focus(t.type);
        S.activate(t.type, 0, false);
        if (n === 3) { t.rows = []; t.args = null; t.grid.clear(); }
    }

    [1, 2, 3].forEach(function (n) {
        var t = TABS[n];
        el(t.type).addEventListener('change', function () { onType(n); });
        el(t.show).addEventListener('click', function () { getReport(n); });
        el(t.neu).addEventListener('click', function () { neu(n); });
        el(t.print).addEventListener('click', function () { print(n); });
        if (t.refresh) el(t.refresh).addEventListener('click', function () { var b = el(t.refresh); A.busy(b, true); loadCustomers().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); }); });
        el(t.from).value = A.today(); el(t.to).value = A.today();
        t.grid.render();
    });

    // ------------------------------------------------------------------ tab strip
    function selectTab(n) {
        [1, 2, 3].forEach(function (i) {
            el(TABS[i].page).hidden = i !== n;
            el('tab' + i).classList.toggle('on', i === n);
        });
    }
    [1, 2, 3].forEach(function (n) { el('tab' + n).addEventListener('click', function () { selectTab(n); }); });
    selectTab(1);
    S.closeShortcuts();

    // ------------------------------------------------------------------ start (Load)
    fillTypes();
    loadCustomers().then(function () { S.focus(TABS[1].type); }).catch(function (e) { alert(e.message); });
}(window, document));
