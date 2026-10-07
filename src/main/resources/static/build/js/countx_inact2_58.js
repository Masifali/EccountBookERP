/* ============================================================================================
 * Screen 58 "Payables Aging Report" - Architecture.WinApp.Account_Reports.PayablesAging (Account_Reports/PayablesAging.cs, 698 lines)
 * Page: templates/accounts/reports/inact2_payables_aging.html (route /accounts/reports/payables-aging-ledger). Group K (Inact2).
 * Every desktop handler in desktop order (line numbers are PayablesAging.cs):
 *   VoucherValidation_Load  :94   Tag "PayablesAging" -> ActionId 1, AccouuntClassId 3, label23 "Accounts Payables Aging"; txtAgingDays.Focus (NO Show on load)
 *   reset / btnNew_Click    :115 / :122  txtAgingDays.Clear(); btnshow_Click; txtAgingDays.Focus()
 *   btnshow_Click           :132  ReportsParameters: Organization, Company, UserId, FinancialYear, EndDate, AgingDays = ToInt(text), IntervalDays = ToInt(txtNoofIntervals "4"),
 *                                 ActionId, AccouuntClassId, ApprovedFilter "All", ZeroBalanceType 1 when chkSkipZero -> VoucherReports.LedgerAging = SpAccounts_LedgerAging
 *                                 (AsOnDate, AgingDays, NoOfInternal, ActionId, AccouuntClassId, SkipZero each only when non-zero). Rows -> the table "dt":
 *                                 AccountId, AccountCode, AccountTitle, SubsidiaryAccountId, then one double column per i <= IntervalDays named by the VALUE of
 *                                 row 0's column i+4 (Col_01..Col_05 = the slab captions), filled from column 9+j (Value_01..). No rows -> grd.DataSource = null.
 *   GridSettings            :202  SubsidiaryAccountId / AccountId hidden, AccountCode = link, slab columns Sum + right + stringFormatboth (row and total), GridAutoAdjustmentNew
 *   grd_LinkClicked         :225  AccountCode -> GoToGeneralLedgerFromLinkedEvent(AccountId, ActiveYr.Start_Period, EndDate, SubsidiaryAccountId)
 *   print_Click_1           :243  dtrpt rows > 0 -> 120-AcRptPayablesAging.rpt over the LAST Show's procedure rows (@CompanyName, @CompanyAddress), else "Not Record Found For Display"
 *   VoucherValidation_KeyDown :266 Enter -> Tab; Ctrl+S show; Ctrl+P print; Ctrl+E / Esc close; Ctrl+N reset; Ctrl+F5 / Ctrl+Up EndDate; Ctrl+Down grid; Ctrl+Alt shortcut keys
 *   btnshortcutkey_Click / MakeShortCutKeys :343 / :355  ShortCutKeyPopUp with the 10 rows below
 *   grd_KeyDown             :386  Ctrl+Space on AccountCode -> GoToGeneralLedgerFromLinkedEvent(AccountId, ActiveYr.Start_Period, EndDate) (grid engine: any current row)
 *   ctrlGrdBar1_Load :313 (layout restore, in countx_grid_bar) / PayablesAging_FormClosing (HistoryStack) have no further web equivalent; btnclose / btnRefresh have no body / no control.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, I = w.Inact2, el = A.el;
    var API = '/api/accounts/inact2/payables-aging';
    var lookup = { amountDecimals: 0, yearStart: null };
    var dtrpt = [];          // the last Show's procedure rows (what Print hands to the template)
    var lastArgs = null;
    var seq = 0;

    function load() {
        return A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; }).catch(function (e) { alert(e.message); });
    }

    var grid = new A.Grid({
        tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', groupTotals: false,
        decimals: function () { return lookup.amountDecimals; },
        onLink: function (row) { ledger(row); }
    });
    I.totals(grid);

    /* the "dt" of btnshow_Click: the columns come from the first row's own column names, the slab captions from its values */
    function build(rows) {
        var names = Object.keys(rows[0]);
        var lenthofcolumns = names.length - 4, intervalDays = A.toInt(el('txtNoofIntervals').value);
        var cols = [
            { key: 'AccountId', caption: 'AccountId', hidden: true },
            { key: 'AccountCode', caption: 'AccountCode', width: 110, link: true },
            { key: 'AccountTitle', caption: 'AccountTitle', width: 280 },
            { key: 'SubsidiaryAccountId', caption: 'SubsidiaryAccountId', hidden: true }
        ];
        var slabs = [], seen = { 'AccountId': 1, 'AccountCode': 1, 'AccountTitle': 1, 'SubsidiaryAccountId': 1 };
        for (var i = 0; i < lenthofcolumns - 5; i++) {
            if (i <= intervalDays) {
                var v = rows[0][names[i + 4]], n = v == null ? '' : String(v);
                if (n === '') n = 'Column' + (slabs.length + 1);
                if (seen[n]) throw new Error("A column named '" + n + "' already belongs to this DataTable.");     // DataColumnCollection duplicate name
                seen[n] = 1; slabs.push(n);
            }
        }
        slabs.forEach(function (n) {
            cols.push({ key: n, caption: n, width: 115, type: 'amount', sum: true,
                        fmt: function (v) { return v === null || v === undefined || v === '' ? '' : A.netFormat(v, lookup.amountDecimals); },
                        totalFmt: function (s) { return A.netFormat(s, lookup.amountDecimals); } });
        });
        var out = rows.map(function (r) {
            var o = { 'AccountId': A.ci(r, 'AccountId'), 'AccountTitle': A.ci(r, 'AccountTitle'), 'AccountCode': A.ci(r, 'AccountCode'),
                      'SubsidiaryAccountId': A.ci(r, 'SubsidiaryAccountId') };
            slabs.forEach(function (n, j) { o[n] = r[names[lenthofcolumns - (lenthofcolumns - 5) + 4 + j]]; });
            return o;
        });
        return { cols: cols, rows: out };
    }

    // ------------------------------------------------------------------ Show (btnshow_Click)
    function show() {
        var b = el('show'); if (b.disabled) return;
        var args = {
            endDate: el('EndDate').value, agingDays: A.toInt(el('txtAgingDays').value),
            intervalDays: A.toInt(el('txtNoofIntervals').value), skipZero: el('chkSkipZero').checked
        };
        if (!args.endDate) { alert('Please select the End Date'); return; }
        A.busy(b, true);
        var token = ++seq;
        return A.getJson(API + '?' + new URLSearchParams(args).toString()).then(function (rows) {
            if (token !== seq) return;
            dtrpt = rows || [];
            lastArgs = args;
            if (!dtrpt.length) { grid.clear(); return; }                           // grd.DataSource = null
            var b2 = build(dtrpt);
            grid.setData(b2.cols, b2.rows);
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }

    // ------------------------------------------------------------------ link / print / reset
    function ledger(r) {
        w.open(I.ledgerUrl(A.ci(r, 'AccountId'), lookup.yearStart, el('EndDate').value, A.ci(r, 'SubsidiaryAccountId')), '_blank');
    }
    function print() {
        var b = el('print'); if (b.disabled) return;
        if (!dtrpt.length || !lastArgs) { alert('Not Record Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/accounts/reports/inact2/print/payables-aging-120?' + new URLSearchParams(lastArgs).toString())
            .catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() {                                          // btnNew_Click -> reset()
        el('txtAgingDays').value = '';
        var p = show();
        el('txtAgingDays').focus();
        return p;
    }
    function shortcuts() {                                      // MakeShortCutKeys
        A.shortcuts([
            ['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl + P', 'For print'],
            ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]
        ]);
    }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('print').addEventListener('click', print);
    el('reset').addEventListener('click', reset);
    el('shortcutkey').addEventListener('click', shortcuts);
    el('closeShortcuts').addEventListener('click', function () { el('shortcutDialog').close(); });
    I.enterTab(['EndDate', 'txtAgingDays', 'show', 'chkSkipZero']);
    d.addEventListener('keydown', function (e) {                // VoucherValidation_KeyDown (KeyPreview)
        if (e.key === 'Escape' && !A.comboOpen() && !el('shortcutDialog').open) { e.preventDefault(); I.closeForm(); return; }
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase();
        if (e.altKey) { e.preventDefault(); shortcuts(); return; }                                // Ctrl + Alt
        if (k === 's') { e.preventDefault(); show(); }
        else if (k === 'p') { e.preventDefault(); print(); }
        else if (k === 'e') { e.preventDefault(); I.closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
        else if (e.key === 'F5' || e.key === 'ArrowUp') { e.preventDefault(); el('EndDate').focus(); }
        else if (e.key === 'ArrowDown') { e.preventDefault(); grid.focus(); }
    });

    // ------------------------------------------------------------------ start (VoucherValidation_Load)
    el('EndDate').value = A.today();                            // DateTimePicker default = Now
    grid.render();
    load().then(function () { el('txtAgingDays').focus(); });
}(window, document));
