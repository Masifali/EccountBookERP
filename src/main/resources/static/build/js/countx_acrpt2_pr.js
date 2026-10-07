/* ============================================================================================
 * Screen 870 "Payables And Receivables With Payment And Receipts" - Architecture.WinApp.Account_Reports.frmPayablesAndReceivablesWithPaymentAndReceipts
 * Page: templates/accounts/reports/acrpt2_payables_receivables_payment_receipts.html (route /accounts/reports/payables-receivables-with-payment-receipts).
 * Every desktop handler, in desktop order (frmPayablesAndReceivablesWithPaymentAndReceipts.cs):
 *   InitializeComponentMethod     -> features 17 / 18, DefaultDaysToLessFromHistoryFromDate, branches (GetBranchesFromVouchersByAccountId), custom groups
 *                                    (CustomeGroupsDefine(1)), 3rd level accounts of class 2,3, cities; From = today - DefaultDays (else today - 3), To = today;
 *                                    no branch feature -> "Branch" label and combo hidden; txtFromDate.Focus().
 *   BranchesFill                  -> features 17 + 18: checked list, all rows checked; otherwise a plain combo showing the user's branch.
 *   CustomGroupsBind / AccountFill3rdLevelBind / CityDtFillFromGlobalAndBind -> BindAndRetainSelection (a leading empty row for group / parent, none for city).
 *   btnshow_Click / GetData       -> BranchesIds by feature, From, To, AccouuntClassId (Purchase 3 / Sale 2 / Both 0), CustomGroupId, ParentId, AreaCity = the city text;
 *                                    usp_getPayablesAndReceivablesWithPaymentAndReceipts; rows -> dtGrid (19 columns) and GridSettings, no rows -> grd.ClearStructure().
 *   GridSettings                  -> AccountId, Debit, Credit, ParentAccount hidden; HeaderLines 2; AccountCode is a link; grouped by ParentAccount, GroupTotals Always;
 *                                    widths AccountCode 100, AccountTitle 200, CityName 120, Opening 80, Closing 90, TotalSale / TotalPurchase 80,
 *                                    Bank / Cash / Party Receipts and Payments 75, JvCredit / JvDebit 70.
 *   grd_LinkClicked / grd_KeyDown (Ctrl+Space on AccountCode) -> GoToGeneralLedgerFromLinkedEvent(AccountId, From, To, 0, UserAccount.BranchesId).
 *   btnNew_Click / reset          -> group, parent, city cleared, Purchase checked (not Sale), both tables cleared, empty structure re-drawn.
 *   btnRefresh_Click              -> all lookups re-read; groups / parent / city keep their selection; branches rebound.
 *   btnPrintCurrent_Click         -> dtRecordsFromDB with rows -> 134_PayablesAndReceivablesWithPaymentAndReceipts.rpt (nothing happens when empty).
 *   VoucherValidation_KeyDown     -> Enter = Tab, Ctrl+E / Esc close, Ctrl+S show, Ctrl+F5 / Ctrl+Up From date, Ctrl+N new, Ctrl+P / Alt+1 print,
 *                                    Ctrl+R refresh, Ctrl+Down grid, Ctrl+Alt shortcut keys.
 *   btnShortCutKeys_Click / MakeShortCutKeys -> ShortCutKeyPopUp.
 *   ctrlGrdBar1_Load              -> saved grid layout (countx_grid_bar.js).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt2, el = A.el;
    var API = '/api/accounts/acrpt2/pr';
    var RPT = '134_PayablesAndReceivablesWithPaymentAndReceipts.rpt';
    var GL = '/accounts/reports/general-ledger';

    var COLS = ['ParentAccount', 'AccountId', 'AccountCode', 'AccountTitle', 'CityName', 'Opening', 'TotalSale', 'TotalPurchase', 'BankReceipts',
        'CashReceipts', 'BankPayment', 'CashPayment', 'PartyReceipts', 'PartyPayment', 'JvCredit', 'JvDebit', 'Closing', 'Debit', 'Credit'];
    var HIDDEN = { AccountId: 1, Debit: 1, Credit: 1, ParentAccount: 1 };
    var WIDTH = { AccountCode: 100, AccountTitle: 200, CityName: 120, Opening: 80, Debit: 75, Credit: 75, Closing: 90, TotalSale: 80, TotalPurchase: 80,
        BankReceipts: 75, CashReceipts: 75, PartyReceipts: 75, JvCredit: 70, BankPayment: 75, CashPayment: 75, PartyPayment: 75, JvDebit: 70 };
    var TEXT = { ParentAccount: 1, AccountCode: 1, AccountTitle: 1, CityName: 1 };
    var SHORTCUTS = [['Ctrl+S', 'For Show Record'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print '],
        ['Alt+1', 'For Print '], ['Ctrl+F5', 'For Focus on From Date'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On From Date'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];

    var look = { branchFeature: false, branchFeatureConsolidated: false, decimals: 2 };
    var recs = [];                  // dtRecordsFromDB
    var grid, seq = 0, multi = false;

    function gridFormats() {        // GridColumnSettings: key-based formats
        var n = look.amountDecimals > 0 ? look.amountDecimals : 2;
        return { whole: '#,#;(#,#.##);0', dec: '#,##0.' + new Array(n + 1).join('#') };
    }
    function columns() {
        var f = gridFormats();
        return COLS.map(function (c) {
            var o = { key: c, caption: c, width: WIDTH[c] || 100, hidden: !!HIDDEN[c] };
            if (c === 'AccountCode') o.link = true;
            if (c === 'AccountId') o.type = 'int';
            if (!TEXT[c] && c !== 'AccountId') {
                var whole = /Cash|Bank|Credit|Debit|Opening|Closing|Receipt|Payment|Profit/.test(c);
                o.type = 'num'; o.fmt = whole ? f.whole : f.dec; o.sum = true; o.totalFmt = o.fmt;
            }
            return o;
        });
    }
    function emptyStructure() { grid.setData(columns(), []); }      // grd.DataSource = dtGrid (no rows); RetrieveStructure(); GridSettings()

    // ------------------------------------------------------------------ lookups
    function bindLookups(first) {
        return A.getJson(API + '/lookups').then(function (data) {
            look = Object.assign(look, data || {});
            multi = !!(look.branchFeature && look.branchFeatureConsolidated);
            // BranchesFill
            var branches = look.branches || [];
            el('wrapBranchMulti').hidden = !(look.branchFeature && multi);
            el('wrapBranchOne').hidden = !(look.branchFeature && !multi);
            el('lblBranch').hidden = !look.branchFeature;
            if (multi) {
                A.fillSelect(el('branchMulti'), branches, 'Id', 'BranchName', { keep: false });
                for (var i = 0; i < el('branchMulti').options.length; i++) el('branchMulti').options[i].selected = true;   // BranchesIds "" -> all checked
            } else {
                A.fillSelect(el('branchOne'), branches, 'Id', 'BranchName', { keep: false, empty: false });
                if (look.userBranchId) el('branchOne').value = String(look.userBranchId);
            }
            // BindAndRetainSelection
            A.fillSelect(el('grp'), look.customGroups || [], 'Id', 'AcLookUpsDescription');
            A.fillSelect(el('parent'), look.accounts || [], 'Id', 'AccountTitle', { extra: { code: 'AccountCode' } });
            var cityKeep = el('city').value;
            A.fillSelect(el('city'), look.cities || [], 'Id', 'CityName', { empty: false });
            if (!cityKeep) el('city').selectedIndex = -1;          // no city selected (Id 0) stays empty
            if (w.DesktopCombo && w.DesktopCombo.refresh) w.DesktopCombo.refresh();
            ['grp', 'parent', 'city', 'branchMulti', 'branchOne'].forEach(function (id) { el(id).dispatchEvent(new Event('change', { bubbles: true })); });
        });
    }

    // ------------------------------------------------------------------ GetData
    function branchIds() {
        if (!look.branchFeature) return '';
        return multi ? A.checkedValues(el('branchMulti')).join(',') : (el('branchOne').value || '');
    }
    function cityText() {
        var s = el('city'), o = s.selectedIndex >= 0 ? s.options[s.selectedIndex] : null;
        return o ? o.textContent.trim() : '';
    }
    function classId() { return el('rdPurchase').checked ? 3 : (el('rdSale').checked ? 2 : 0); }

    function getData() {
        var q = new URLSearchParams({ fromDate: el('fromDate').value || A.today(), toDate: el('toDate').value || A.today(),
            branchIds: branchIds(), classId: classId(), customGroupId: A.intArg(el('grp').value), parentId: A.intArg(el('parent').value), city: cityText() });
        var token = ++seq;
        return A.getJson(API + '/data?' + q.toString()).then(function (rows) {
            if (token !== seq) return;
            recs = rows || [];
            if (!recs.length) { grid.clear(); return; }                   // grd.ClearStructure()
            grid.setData(columns(), recs.map(function (r) {
                var o = {}; COLS.forEach(function (c) { var v = A.ci(r, c); o[c] = v == null ? (TEXT[c] ? '' : 0) : v; }); return o;
            }));
        });
    }
    function show() {
        var b = el('show'); if (b.disabled) return Promise.resolve();
        A.busy(b, true);
        return getData().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ New / Refresh / Print
    function reset() {
        A.clearSelect(el('grp')); A.clearSelect(el('parent'));
        el('city').selectedIndex = -1; el('city').dispatchEvent(new Event('change', { bubbles: true }));
        el('rdPurchase').checked = true;
        recs = [];
        emptyStructure();
    }
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        bindLookups(false).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function print134() {
        if (!recs.length) return;
        A.postGrid(RPT, recs, null, el('print'));
    }

    // ------------------------------------------------------------------ links
    function ledger(row) {
        var q = new URLSearchParams({ accountId: A.ci(row, 'AccountId'), fromDate: el('fromDate').value || A.today(), toDate: el('toDate').value || A.today() });
        w.open(GL + '?' + q.toString(), '_blank');
    }
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('new').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print134);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    el('closeShortcuts').addEventListener('click', function () { el('shortcutDialog').close(); });

    function focusables() {
        var ids = ['fromDate', 'toDate', 'grp', 'parent', 'city'];
        if (look.branchFeature) ids.push(multi ? 'branchMulti' : 'branchOne');
        ids.push('rdPurchase', 'rdSale', 'rdBoth', 'show');
        return ids.map(A.focusOf);
    }
    d.addEventListener('keydown', function (e) {                         // VoucherValidation_KeyDown (KeyPreview)
        var dlg = el('shortcutDialog'); if (dlg.open) return;
        if (e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt')) { e.preventDefault(); A.shortcuts(SHORTCUTS); return; }
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !A.comboOpen() && e.target.tagName !== 'BUTTON' && !(grid && el('grid').contains(e.target))) {
            var list = focusables(), i = list.indexOf(d.activeElement);
            if (i >= 0 && i < list.length - 1) { e.preventDefault(); list[i + 1].focus(); return; }
        }
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (e.altKey && !e.ctrlKey && (e.key === '1' || e.code === 'Digit1' || e.code === 'Numpad1')) { e.preventDefault(); print134(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 's') { e.preventDefault(); show(); }
        else if (e.key === 'F5') { e.preventDefault(); el('fromDate').focus(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
        else if (k === 'p') { e.preventDefault(); print134(); }
        else if (k === 'r') { e.preventDefault(); refresh(); }
        else if (e.key === 'ArrowDown') { e.preventDefault(); grid.focus(); }
        else if (e.key === 'ArrowUp') { e.preventDefault(); el('fromDate').focus(); }
    });

    // ------------------------------------------------------------------ start (InitializeComponentMethod)
    grid = A.createGrid({ table: el('grd'), scroller: el('grid'), nav: el('nav'), navText: el('navText'), headerLines: 2, groupBy: 'ParentAccount',
        groupTotals: true, totalRow: true, filterRow: true, onLink: ledger });
    grid.clear();
    el('toDate').value = A.today();
    el('fromDate').value = A.addDays(A.today(), -3);
    bindLookups(true).catch(function (e) { alert(e.message); }).then(function () {
        var back = A.intArg(look.defaultDaysBack);
        el('fromDate').value = A.addDays(A.today(), back > 0 ? -back : -3);
        el('toDate').value = A.today();
        el('fromDate').focus();
    });
}(window, document));
