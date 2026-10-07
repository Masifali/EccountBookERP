/* ============================================================================================
 * Screen 68 "1007 Receivables By Due Dates New" - Architecture.WinApp.Account_Reports.ReceivablesByDueDatesNew (ReceivablesByDueDatesNew.cs, 1183 lines)
 * Page: templates/accounts/reports/acrpt1_receivables_by_due_dates_new.html (route /accounts/reports/receivables-by-due-dates-new).
 * Group R1, every desktop handler (line numbers are ReceivablesByDueDatesNew.cs, .NET-decompiled):
 *   ReceivablesByDueDatesNew_Load :87  AccountFill3rdLevel (:106 ChartofAccount.ReadAll3rdLevelAccountsForPayablesandReceeivablesAging, TypeNo = 2 rows only,
 *                                 Id = id, caption "Account Title", single), CustomeGroupsDefine (:151 CommonServices.CustomeGroupsDefine(1), caption "Custom Group");
 *                                 the three interval panels Visible = false
 *   btnRefresh_Click        :222  both lists are bound again; the chosen Id is kept when it is still in the list, otherwise the box is cleared
 *   btnNew_Click / reset    :195  datDueDateFrom.Focus() only
 *   btnshow_Click / FillGridData :270 VoucherReports.ReceivablesByDueDates = Usp_ReceivablesByDueDates (OrganizationId, CompanyId, UserId, FromDate only when the
 *                                 Due From check box is ticked, ToDate, ActionId 1, AccouuntClassId 2, CustomGroupId / ParentId (account id) only when set);
 *                                 rows -> interval panels shown, SumClosing / SumNotYetDue / SumOverDueReceivables formatted with stringFormatboth;
 *                                 no rows -> grd.ClearStructure, the three texts cleared, "Record Not Found For Display"
 *   GridSettings            :342  HeaderLines 2, hidden Opening / AccountId, AccountCode link, Groups ParentAccount (HideWhenGrouped), widths, sums + stringFormatboth
 *   grd_LinkClicked         :392  data rows only -> GoToGeneralLedgerFromLinkedEvent(AccountId, datDueDateFrom.Value, datDueDateTo.Value)
 *   Print_Click             :405  121_01-ReceivablesByDueDates.rpt over dtGrid (@CompanyAddress, @CompanyName)
 *   btnshortcutkey_Click / MakeShortCutKeys :476, btnshow_Leave :234 (focus on the grid -> datDueDateFrom)
 *   ReceivablesByDueDatesNew_KeyDown :432 Ctrl+P, Ctrl+E / Esc, Ctrl+S, Ctrl+N, Ctrl+R, Ctrl+F5, Ctrl+Up, Ctrl+Down, Ctrl+Alt
 *   Not ported: ctrlGrdBar1_Load (layout restore), ReceivablesByDueDatesNew_FormClosing (HistoryStack).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, el = A.el;
    var API = '/api/accounts/acrpt1/receivables-new';
    var lookup = { amountDecimals: 0 };
    var dtGrid = [], lastArgs = null, seq = 0;

    function intervals(on) { ['panelInterval01', 'panelInterval02', 'panelInterval03'].forEach(function (id) { el(id).hidden = !on; }); }
    function intervalText(a, b, c) { el('txtInterval01').value = a; el('txtInterval02').value = b; el('txtInterval03').value = c; }

    function bind(keep) {
        A.fill(el('cmbAccount'), lookup.accounts, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'AccountTitle'); }, keep);
        A.fill(el('cmbCustomGroup'), lookup.customGroups, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'AcLookUpsDescription'); }, keep);
    }
    function load(keep) {
        return A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; bind(keep); }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------ Due From (DateTimePicker.ShowCheckBox, unchecked)
    el('chkDueFrom').addEventListener('change', function () { el('datDueDateFrom').disabled = !this.checked; });
    function focusFrom() { (el('chkDueFrom').checked ? el('datDueDateFrom') : el('chkDueFrom')).focus(); }

    // ------------------------------------------------------------------ grid
    function colDefs() {
        function amt(key, width) { return { key: key, caption: key, width: width, type: 'amount', sum: true }; }
        return [
            { key: 'AccountId', caption: 'AccountId', hidden: true },
            { key: 'ParentAccount', caption: 'ParentAccount', width: 120, hidden: true },           // grouped, HideWhenGrouped
            { key: 'AccountCode', caption: 'AccountCode', width: 95, link: true },
            { key: 'AccountTitle', caption: 'AccountTitle', width: 250 },
            { key: 'AccountType', caption: 'AccountType', width: 65 },
            { key: 'Opening', caption: 'Opening', width: 110, hidden: true, type: 'amount', sum: true },
            amt('Debit', 110), amt('Credit', 110), amt('NetReceivables', 120), amt('NotYetDue', 110), amt('OverDue', 110)
        ];
    }
    function mapRow(r) {                                                // dt.Rows.Add(Conversion.ToInt/ToString/ToDouble(...)); Closing -> NetReceivables, OverDueReceivables -> OverDue
        var c = A.ci, s = function (v) { return v == null ? '' : String(v); };
        return {
            'AccountId': A.toInt(c(r, 'AccountId')), 'ParentAccount': s(c(r, 'ParentAccount')), 'AccountCode': A.toInt(c(r, 'AccountCode')),
            'AccountTitle': s(c(r, 'AccountTitle')), 'AccountType': s(c(r, 'AccountType')), 'Opening': A.toDouble(c(r, 'Opening')),
            'Debit': A.toDouble(c(r, 'Debit')), 'Credit': A.toDouble(c(r, 'Credit')), 'NetReceivables': A.toDouble(c(r, 'Closing')),
            'NotYetDue': A.toDouble(c(r, 'NotYetDue')), 'OverDue': A.toDouble(c(r, 'OverDueReceivables'))
        };
    }
    var grid = new A.Grid({
        tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', groupBy: 'ParentAccount', groupTotals: true,
        decimals: function () { return lookup.amountDecimals; }, onLink: function (row) { ledger(row); }
    });

    // ------------------------------------------------------------------ Show
    function show() {
        var b = el('show'); if (b.disabled) return;
        var args = { toDate: el('datDueDateTo').value, customGroupId: A.toInt(el('cmbCustomGroup').value), parentId: A.toInt(el('cmbAccount').value) };
        if (el('chkDueFrom').checked && el('datDueDateFrom').value) args.fromDate = el('datDueDateFrom').value;
        if (!args.toDate) { alert('Please select the Due To date'); return; }
        A.busy(b, true);
        var token = ++seq;
        A.getJson(API + '?' + new URLSearchParams(args).toString()).then(function (rows) {
            if (token !== seq) return;
            dtGrid = rows || [];
            if (!dtGrid.length) { lastArgs = null; grid.clear(); intervalText('', '', ''); alert('Record Not Found For Display'); return; }
            lastArgs = args;
            intervals(true);
            var sc = 0, sn = 0, so = 0;
            dtGrid.forEach(function (r) { sc += A.toDouble(A.ci(r, 'Closing')); sn += A.toDouble(A.ci(r, 'NotYetDue')); so += A.toDouble(A.ci(r, 'OverDueReceivables')); });
            grid.setData(colDefs(), dtGrid.map(mapRow));
            var p = lookup.amountDecimals;
            intervalText(A.netFormat(sc, p), A.netFormat(sn, p), A.netFormat(so, p));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }

    function ledger(r) {
        var q = new URLSearchParams();
        q.set('accountId', A.ci(r, 'AccountId'));
        if (el('datDueDateFrom').value) q.set('fromDate', el('datDueDateFrom').value);
        if (el('datDueDateTo').value) q.set('toDate', el('datDueDateTo').value);
        w.open('/accounts/reports/general-ledger?' + q.toString(), '_blank');
    }
    function print() {
        var b = el('print'); if (b.disabled) return;
        if (!dtGrid.length || !lastArgs) { alert('Not Record Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/accounts/reports/acrpt1/print/receivables-new?' + new URLSearchParams(lastArgs).toString())
            .catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() { focusFrom(); }                                    // reset(): datDueDateFrom.Focus() only
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        load(true).then(function () { A.busy(b, false); });
    }
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Print + P', 'For print'],
        ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On DateType in Filter']];
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    el('closeShortcuts').addEventListener('click', function () { el('shortcutDialog').close(); });
    el('show').addEventListener('blur', function (e) {                    // btnshow_Leave
        if (e.relatedTarget && el('grid').contains(e.relatedTarget)) focusFrom();
    });
    d.addEventListener('keydown', function (e) {                          // ReceivablesByDueDatesNew_KeyDown (KeyPreview)
        if (el('shortcutDialog').open) return;
        if (e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt')) { e.preventDefault(); A.shortcuts(SHORTCUTS); return; }
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); print(); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 's') { e.preventDefault(); show(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
        else if (k === 'r') { e.preventDefault(); refresh(); }
        else if (k === 'f5' || k === 'arrowup') { e.preventDefault(); focusFrom(); }
        else if (k === 'arrowdown') { e.preventDefault(); grid.focus(); }
    });

    // ------------------------------------------------------------------ start
    el('datDueDateFrom').value = A.today();                               // unchecked picker still holds today's date (used by the ledger link)
    el('datDueDateTo').value = A.today();
    intervals(false);
    grid.render();
    load(false);
}(window, document));
