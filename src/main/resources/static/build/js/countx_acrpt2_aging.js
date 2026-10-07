/* ============================================================================================
 * Screen 77 "Payables and Receivables Aging Report" - Architecture.WinApp.Account_Reports.PayablesandReceivablesAging
 * Page: templates/accounts/reports/acrpt2_payables_receivables_aging.html (route /accounts/reports/payables-receivables-aging-report).
 * Desktop handlers, in desktop order (PayablesandReceivablesAging.cs):
 *   Receivables_Load                  -> dt columns (18, Opening..Closing double), txtFromDate = ActiveYr.Start_Period, DataGridBindForShow(),
 *                                        CustomeGroupsDefine(), CustomerGroupsData(). (ReceivablePayblesStatus is 0 when opened from the menu.)
 *   DataGridBindForShow / btnshow / btnRefresh
 *                                     -> NotEqualTo < 0 -> message; ItemClassId 1 (Account) / 2 (Balance) and ItemTypeId 1 (Payables) /
 *                                        2 (Receivables) only when a radio is checked; SpAccounts_PayablesReceivablesAging_Rpt; dtreceive = result,
 *                                        dt = copy of every row; ReportParameters = dtreceive[0].ReportCriteria (no rows -> "There is no row at position 0.").
 *   GridSettings                      -> Opening/Interval1St/2nd/3rd/IntervalAbove/Closing summed, "#,#;(#,#);0", right aligned; ArApTypeDesc,
 *                                        Ac1-3 code/title, AcId, RCounter, ReportCriteria hidden; Ac4LevelCode caption "AccountCode"; AccountTitle 300.
 *   RBAccountsPayables_Click / RBAccountsReceievables_Click
 *                                     -> AccountFill3rdLevel() then (classification radio + type radio) AccountsClassificationPayables()
 *                                        (accountClass 3 for Payables / 2 for Receivables) or BalanceClassificationPayablesandReceivables()
 *                                        (ArApTypeDesc "Payables" / "Receivables"); both work on dtreceive; they do nothing when dtreceive is empty.
 *   RBAccountClassification_Click / RBBalanceClassification_Click -> the same two filters (only when a type radio is checked).
 *   AccountFill3rdLevel               -> ReadAll3rdLevelAccountsForPayablesandReceeivablesAging; rebinds only when rows > 0; AcPayables = 1 if
 *                                        Payables checked else 0, AcReceievables = 2 if Receivables checked else 0; per row TypeNo == AcPayables
 *                                        adds the row, then TypeNo == AcReceievables adds it again (the desktop quirk is kept).
 *   btnFilter_Click / DataGridBind    -> NotEqualTo check; dt cleared; checked account texts matched to Ac3LevelTitle; checked groups ->
 *                                        dtCustomeGroupAll (AcLookUpsDescription) ChartOfAccountId == AcId; both lists add rows (a row can repeat).
 *   cmbAccount_ValueChanged / CmbCustomeGroups_ValueChanged -> the other combo's text is cleared.
 *   btnNew / reset                    -> dates, interval, not-equal, both combos cleared, radios unchecked, grid ClearStructure(); dt is NOT cleared.
 *   print_Click (154-Print)           -> dt.Rows.Count > 0 -> 154-Payables&Receiveableaging-Rpt.rpt over all 18 columns, else "Record not found for Display".
 *   There is no KeyDown handler on the form, so no key handling and no shortcut dialog.
 *   ctrlGrdBar1_Load                  -> saved grid layout (countx_grid_bar.js).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt2, el = A.el;
    var API = '/api/accounts/acrpt2/aging';
    var RPT = '154-Payables&Receiveableaging-Rpt.rpt';
    var NUM = '#,#;(#,#);0';

    var COLS = ['ArApTypeDesc', 'Ac1LevelCode', 'Ac1LevelTitle', 'Ac2LevelCode', 'Ac2LevelTitle', 'Ac3LevelCode', 'Ac3LevelTitle', 'AcId',
        'Ac4LevelCode', 'AccountTitle', 'Opening', 'Interval1St', 'Interval2nd', 'Interval3rd', 'IntervalAbove', 'Closing', 'RCounter', 'ReportCriteria'];
    var DBL = { Opening: 1, Interval1St: 1, Interval2nd: 1, Interval3rd: 1, IntervalAbove: 1, Closing: 1 };

    var look = { accounts: [], customGroups: [], customGroupAll: [], yearStart: null };
    var dtreceive = [];             // desktop dtreceive
    var dt = [];                    // desktop dt (what the grid shows and what prints)
    var grid, guard = false, seq = 0;

    function copyRow(r) {           // dt.Rows.Add(<the 18 columns>)
        var o = {};
        COLS.forEach(function (c) {
            var v = A.ci(r, c);
            o[c] = DBL[c] ? (v == null || v === '' ? 0 : Number(v)) : (v == null ? '' : v);
        });
        return o;
    }

    function columns() {            // GridSettings
        return COLS.map(function (c) {
            var o = { key: c, caption: c, width: 100 };
            if (DBL[c]) { o.type = 'num'; o.fmt = NUM; o.sum = true; o.totalFmt = NUM; }
            if (['ArApTypeDesc', 'Ac1LevelCode', 'Ac2LevelCode', 'Ac3LevelCode', 'AcId', 'ReportCriteria', 'RCounter', 'Ac1LevelTitle', 'Ac2LevelTitle', 'Ac3LevelTitle'].indexOf(c) >= 0) o.hidden = true;
            if (c === 'Ac4LevelCode') o.caption = 'AccountCode';
            if (c === 'AccountTitle') o.width = 300;
            return o;
        });
    }

    function bindGrid() {           // grd.DataSource = dt; RetrieveStructure(); ReportParameters.Text = ...; GridSettings()
        if (!dtreceive.length) throw new Error('There is no row at position 0.');
        grid.setData(columns(), dt);
        el('lblCrit').textContent = String(A.ci(dtreceive[0], 'ReportCriteria') == null ? '' : A.ci(dtreceive[0], 'ReportCriteria'));
    }

    function notEqualOk() {
        if (A.intArg(el('notEq').value) < 0) { alert("Please Check Not Equal To Field ! It should be Greater then '0'"); return false; }
        return true;
    }

    // ------------------------------------------------------------------ DataGridBindForShow
    function showData(quietEmpty) {
        if (!notEqualOk()) return Promise.resolve();
        var q = new URLSearchParams({ toDate: el('toDate').value || A.today(), intervalDays: A.intArg(el('interval').value), notEqualTo: A.intArg(el('notEq').value) });
        var classId = el('rbClassAcc').checked ? 1 : (el('rbClassBal').checked ? 2 : 0);
        var typeId = el('rbPay').checked ? 1 : (el('rbRcv').checked ? 2 : 0);
        if (classId) q.set('classId', classId);
        if (typeId) q.set('typeId', typeId);
        var token = ++seq;
        return A.getJson(API + '/data?' + q.toString()).then(function (rows) {
            if (token !== seq) return;
            dtreceive = rows || [];
            dt = dtreceive.map(copyRow);
            if (!dtreceive.length) {
                grid.clear();
                el('lblCrit').textContent = '';
                if (!quietEmpty) alert('There is no row at position 0.');
                return;
            }
            bindGrid();
        });
    }

    function run(btn, quietEmpty) {
        if (btn && btn.disabled) return Promise.resolve();
        if (btn) A.busy(btn, true);
        return showData(quietEmpty).catch(function (e) { alert(e.message); }).then(function () { if (btn) A.busy(btn, false); });
    }

    // ------------------------------------------------------------------ AccountFill3rdLevel
    function accountFill3rdLevel() {
        return A.getJson(API + '/lookups').then(function (data) {
            var rows = (data && data.accounts) || [];
            look.accounts = rows;
            if (!rows.length) return;                                     // only rebinds when rows > 0
            var acPay = el('rbPay').checked ? 1 : 0, acRcv = el('rbRcv').checked ? 2 : 0;
            var list = [];
            rows.forEach(function (r) {
                var t = A.intArg(A.ci(r, 'TypeNo'));
                if (t === acPay) list.push({ Id: A.ci(r, 'AccountCode'), AccountTitle: A.ci(r, 'AccountTitle') });
                if (t === acRcv) list.push({ Id: A.ci(r, 'AccountCode'), AccountTitle: A.ci(r, 'AccountTitle') });
            });
            guard = true;
            try { A.fillSelect(el('acc'), list, 'Id', 'AccountTitle', { keep: false }); } finally { guard = false; }
        });
    }

    // ------------------------------------------------------------------ classification filters (on dtreceive)
    function applyFilter(kind) {
        if (!dtreceive.length) return;
        var pay = el('rbPay').checked, rcv = el('rbRcv').checked;
        dt = [];
        dtreceive.forEach(function (r) {
            if (kind === 'account') {
                var ac = A.intArg(A.ci(r, 'accountClass'));
                if (pay && ac === 3) dt.push(copyRow(r));
                if (rcv && ac === 2) dt.push(copyRow(r));
            } else {
                var t = String(A.ci(r, 'ArApTypeDesc') == null ? '' : A.ci(r, 'ArApTypeDesc'));
                if (pay && t === 'Payables') dt.push(copyRow(r));
                if (rcv && t === 'Receivables') dt.push(copyRow(r));
            }
        });
        bindGrid();
    }
    function classify() {           // the four if-blocks of the radio click handlers collapse to one call
        if (!(el('rbPay').checked || el('rbRcv').checked)) return;
        if (el('rbClassAcc').checked) applyFilter('account');
        else if (el('rbClassBal').checked) applyFilter('balance');
    }

    function typeClick() {          // RBAccountsPayables_Click / RBAccountsReceievables_Click
        accountFill3rdLevel().catch(function (e) { alert(e.message); }).then(function () {
            try { classify(); } catch (e) { alert(e.message); }
        });
    }
    function classClick() {         // RBAccountClassification_Click / RBBalanceClassification_Click
        try { classify(); } catch (e) { alert(e.message); }
    }

    // ------------------------------------------------------------------ btnFilter_Click -> DataGridBind
    function filter3rd() {
        if (!notEqualOk()) return;
        try {
            dt = [];
            var accs = A.checkedTexts(el('acc'));
            if (accs.length && dtreceive.length) {
                accs.forEach(function (t) {
                    dtreceive.forEach(function (r) { if (String(A.ci(r, 'Ac3LevelTitle')) === t) dt.push(copyRow(r)); });
                });
            }
            var grps = A.checkedTexts(el('grp'));
            if (grps.length && look.customGroupAll.length && dtreceive.length) {
                grps.forEach(function (g) {
                    look.customGroupAll.forEach(function (c) {
                        if (String(A.ci(c, 'AcLookUpsDescription')) !== g) return;
                        var id = String(A.ci(c, 'ChartOfAccountId'));
                        dtreceive.forEach(function (r) { if (String(A.ci(r, 'AcId')) === id) dt.push(copyRow(r)); });
                    });
                });
            }
            bindGrid();
        } catch (e) { alert(e.message); }
    }

    // ------------------------------------------------------------------ cross-clear (ValueChanged)
    el('acc').addEventListener('change', function () { if (guard) return; if (A.checkedValues(el('acc')).length) { guard = true; try { A.clearSelect(el('grp')); } finally { guard = false; } } });
    el('grp').addEventListener('change', function () { if (guard) return; if (A.checkedValues(el('grp')).length) { guard = true; try { A.clearSelect(el('acc')); } finally { guard = false; } } });

    // ------------------------------------------------------------------ New
    function reset() {
        el('toDate').value = A.today();
        el('fromDate').value = look.yearStart || A.today();
        el('interval').value = '';
        el('notEq').value = '';
        guard = true;
        try { A.clearSelect(el('acc')); A.clearSelect(el('grp')); A.fillSelect(el('acc'), [], 'Id', 'AccountTitle', { keep: false }); } finally { guard = false; }
        el('rbClassAcc').checked = false; el('rbClassBal').checked = false; el('rbPay').checked = false; el('rbRcv').checked = false;
        grid.clear();                                   // grd.ClearStructure(); dt is kept
        el('lblCrit').textContent = '';
    }

    // ------------------------------------------------------------------ print
    function print154() {
        var b = el('print');
        if (!dt.length) { alert('Record not found for Display'); return; }
        A.postGrid(RPT, dt.map(function (r) { return Object.assign({}, r); }), null, b);
    }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', function () { run(el('show'), false); });
    el('refresh').addEventListener('click', function () { run(el('refresh'), false); });
    el('filter').addEventListener('click', filter3rd);
    el('new').addEventListener('click', reset);
    el('print').addEventListener('click', print154);
    el('rbPay').addEventListener('click', typeClick);
    el('rbRcv').addEventListener('click', typeClick);
    el('rbClassAcc').addEventListener('click', classClick);
    el('rbClassBal').addEventListener('click', classClick);

    // ------------------------------------------------------------------ start (Receivables_Load)
    grid = A.createGrid({ table: el('grd'), scroller: el('grid'), nav: el('nav'), navText: el('navText'), headerLines: 2, totalRow: true, groupTotals: false, filterRow: true });
    grid.clear();
    el('toDate').value = A.today();
    el('fromDate').value = A.today();
    A.getJson(API + '/lookups').then(function (data) {
        look = Object.assign(look, data || {});
        look.accounts = (data && data.accounts) || [];
        if (look.yearStart) el('fromDate').value = look.yearStart;           // txtFromDate = ActiveYr.Start_Period
        // CustomeGroupsDefine: DDL.BindDDLNew(dtCustome, CmbCustomeGroups, "Id", "AcLookUpsDescription", "Account Title")
        guard = true;
        try { A.fillSelect(el('grp'), look.customGroups || [], 'AcLookUpsDescription', 'AcLookUpsDescription', { keep: false }); } finally { guard = false; }
    }).catch(function (e) { alert(e.message); }).then(function () {
        return run(null, true);                                              // DataGridBindForShow() on load
    });
}(window, document));
