/* ============================================================================================
 * Screen 70 "Receiveables By Due Report" - Architecture.WinApp.Account_Reports.ReceiveablesByDueDate (ReceiveablesByDueDate.cs, 1268 lines)
 * Page: templates/accounts/reports/acrpt1_receivables_by_due_date.html (route /accounts/reports/receivables-by-due-date).
 * Group R1, every desktop handler (line numbers are ReceiveablesByDueDate.cs, .NET-decompiled):
 *   VoucherValidation_Load  :238  lbllang + CmbLanguage hidden; CustomerGroupFill (:119 Sp_CustomerGroup_GetAllMethod ReadAll minus Ids 7,9,10,12,13,14,15,
 *                                 checked, caption "Party Group"), LanguageDropdownBind (:186), AccountFill3rdLevel (:150, ChartofAccount.ReadAllAccountgroup
 *                                 Level 3 / TypeId 3 / ClassId 2, Id = AccountCode, checked, caption "Account Title"), CustomeGroupsDefine (:205,
 *                                 CommonServices.CustomeGroupsDefine(1), caption "Custom Group"), CmbGroup.Focus()
 *   btnshow_Click           :330  GroupIds = id + "," per checked party group text; account = "," + AccountCode per checked account text;
 *                                 VoucherReports.DueByDateReceivables = USP_DueByDateReceivables (OrganizationId, CompanyId, UserId, DueDateTo always;
 *                                 BalanceFrom / BalanceTo = Conversion.ToInt(text), LanguageId, CustomGroupId, CustomerGroupIds, ParentAccountCode only when set);
 *                                 no rows -> grd.DataSource = null + "Record Not Found For Display"
 *   GridSettings            :476  HeaderLines 2, Groups ParentAccount (HideWhenGrouped), hidden AccountId / ReportCriteria / OtherLanguageTitle (shown when
 *                                 the language box is visible), sums + stringFormatboth on Debit / Credit / DueAmount, AccountCode link, widths, DueDate dd-MMM-yy
 *   grd_LinkClicked         :523  GoToGeneralLedgerFromLinkedEvent(AccountId, ActiveYr.Start_Period, datDueDateTo)
 *   print_Click_1           :390  no rows -> "Not Record Found For Display"; 380-DueByDateReceivables.rpt over dtrpt (@CompanyAddress, @CompanyName)
 *   btnNew_Click / reset    :263  ("&Refresh") language hidden again, CmbGroup.Focus() - nothing is reloaded
 *   btnCustomGroup_Click    :551  opens frmAccountCustomGroup -> /accounts/custom_group
 *   txtBalFrom / txtBalTo KeyPress -> OnlytextdecimelFunction (digits, one decimal point, control keys)
 *   VoucherValidation_KeyDown :158 Enter = {TAB}, Ctrl+P, Ctrl+L (show language), Ctrl+E / Esc, Ctrl+N
 *   Not ported: ctrlGrdBar1_Load (layout restore), ReceiveablesByDueDate_FormClosing (HistoryStack).
 *   This form has no Show shortcut, no ShortCut Keys button and no Refresh of the lists.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, el = A.el;
    var API = '/api/accounts/acrpt1/receivables-due';
    var lookup = { amountDecimals: 0, yearStart: null };
    var dtrpt = [], lastArgs = null, seq = 0, langVisible = false;

    function wrapOf(id) { var s = el(id); return s ? (s.closest('.dtcombo-wrap') || s) : null; }
    function setLang(on) {                                                 // lbllang.Visible / CmbLanguage.Visible
        langVisible = on;
        el('lblLang').hidden = !on;
        var x = wrapOf('cmbLanguage'); if (x) x.style.display = on ? '' : 'none';
    }

    function load() {
        setLang(false);
        return A.getJson(API + '/lookups').then(function (data) {
            lookup = data || lookup;
            A.fill(el('cmbGroup'), lookup.customerGroups, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'Description'); });
            A.fill(el('cmbLanguage'), lookup.languages, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'LanguageDescription'); });
            A.fill(el('cmbAccount'), lookup.accounts, function (r) { return A.ci(r, 'AccountCode'); }, function (r) { return A.ci(r, 'AccountTitle'); });
            A.fill(el('cmbCustomGroup'), lookup.customGroups, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'AcLookUpsDescription'); });
            setLang(false);                                                // the enhanced combo wrap exists only after fill
            var f = A.focusable('cmbGroup'); if (f) f.focus();
        }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------ grid
    function colDefs() {
        function amt(key) { return { key: key, caption: key, width: 120, type: 'amount', sum: true }; }
        return [
            { key: 'ReportCriteria', caption: 'ReportCriteria', hidden: true },
            { key: 'AccountId', caption: 'AccountId', hidden: true },
            { key: 'ParentAccount', caption: 'ParentAccount', hidden: true },                       // grouped, HideWhenGrouped
            { key: 'AccountCode', caption: 'AccountCode', width: 80, link: true },
            { key: 'AccountTitle', caption: 'AccountTitle', width: 200 },
            { key: 'AccountType', caption: 'AccountType', width: 65 },
            { key: 'OtherLanguageTitle', caption: 'OtherLanguageTitle', hidden: !langVisible },
            { key: 'DueDays', caption: 'DueDays', width: 60, type: 'int', align: 'c' },
            { key: 'DueDate', caption: 'DueDate', width: 75, type: 'date' },
            amt('DebitAmount'), amt('CreditAmount'), amt('DueAmount')
        ];
    }
    function mapRow(r) {                                                // dt.Rows.Add(Conversion.ToString/ToInt/ToDouble(...)); DueDate is passed through as is
        var c = A.ci, s = function (v) { return v == null ? '' : String(v); };
        var due = c(r, 'DueDate');
        return {
            'ReportCriteria': s(c(r, 'ReportCriteria')), 'AccountId': A.toInt(c(r, 'AccountId')), 'ParentAccount': s(c(r, 'ParentAccount')),
            'AccountCode': A.toInt(c(r, 'AccountCode')), 'AccountTitle': s(c(r, 'AccountTitle')), 'AccountType': s(c(r, 'AccountType')),
            'OtherLanguageTitle': s(c(r, 'OtherLanguageTitle')), 'DueDays': A.toInt(c(r, 'DueDays')), 'DueDate': due == null ? '' : due,
            'DebitAmount': A.toDouble(c(r, 'DebitAmount')), 'CreditAmount': A.toDouble(c(r, 'CreditAmount')), 'DueAmount': A.toDouble(c(r, 'DueAmount'))
        };
    }
    var grid = new A.Grid({
        tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', groupBy: 'ParentAccount', groupTotals: true,
        decimals: function () { return lookup.amountDecimals; }, onLink: function (row) { ledger(row); }
    });

    // ------------------------------------------------------------------ Show
    function show() {
        var b = el('show'); if (b.disabled) return;
        var groupIds = '', account = '';
        A.checkedValues(el('cmbGroup')).forEach(function (v) { groupIds += v + ','; });
        A.checkedValues(el('cmbAccount')).forEach(function (v) { account += ',' + v; });
        var args = { dueDateTo: el('datDueDateTo').value, fromDocNo: A.toIntText(el('txtBalFrom').value), toDocNo: A.toIntText(el('txtBalTo').value),
                     languageId: A.toInt(el('cmbLanguage').value), customGroupId: A.toInt(el('cmbCustomGroup').value), ids: groupIds, parentAccountCode: account };
        if (!args.dueDateTo) { alert('Please select the Due Date To'); return; }
        A.busy(b, true);
        var token = ++seq;
        A.getJson(API + '?' + new URLSearchParams(args).toString()).then(function (rows) {
            if (token !== seq) return;
            dtrpt = rows || [];
            if (!dtrpt.length) { lastArgs = null; grid.clear(); alert('Record Not Found For Display'); return; }
            lastArgs = args;
            grid.setData(colDefs(), dtrpt.map(mapRow));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }

    function ledger(r) {
        var q = new URLSearchParams();
        q.set('accountId', A.ci(r, 'AccountId'));
        if (lookup.yearStart) q.set('fromDate', lookup.yearStart);
        if (el('datDueDateTo').value) q.set('toDate', el('datDueDateTo').value);
        w.open('/accounts/reports/general-ledger?' + q.toString(), '_blank');
    }
    function print() {
        var b = el('print'); if (b.disabled) return;
        if (!dtrpt.length || !lastArgs) { alert('Not Record Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/accounts/reports/acrpt1/print/receivables-due?' + new URLSearchParams(lastArgs).toString())
            .catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() { setLang(false); var f = A.focusable('cmbGroup'); if (f) f.focus(); }        // reset(): hide language, CmbGroup.Focus()
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('print').addEventListener('click', print);
    el('customGroup').addEventListener('click', function () { w.open('/accounts/custom_group', '_blank'); });      // frmAccountCustomGroup.Show()
    ['txtBalFrom', 'txtBalTo'].forEach(function (id) {                    // OnlytextdecimelFunction
        el(id).addEventListener('keypress', function (e) {
            if (e.key.length !== 1 || e.ctrlKey) return;
            if (e.key === '.' ? this.value.indexOf('.') >= 0 : !/\d/.test(e.key)) e.preventDefault();
        });
        el(id).addEventListener('input', function () {
            var v = this.value.replace(/[^\d.]/g, ''), i = v.indexOf('.');
            if (i >= 0) v = v.slice(0, i + 1) + v.slice(i + 1).replace(/\./g, '');
            if (v !== this.value) this.value = v;
        });
        el(id).addEventListener('paste', function (e) { e.preventDefault(); });           // ShortcutsEnabled = false
    });

    var TAB_ORDER = ['cmbGroup', 'cmbAccount', 'cmbCustomGroup', 'datDueDateTo', 'txtBalFrom', 'txtBalTo', 'cmbLanguage', 'show'];
    function enterAsTab(e) {                                              // SendKeys.Send("{TAB}")
        if (A.comboOpen()) return;
        var t = e.target, order = TAB_ORDER.filter(function (id) { return id !== 'cmbLanguage' || langVisible; }), cur = -1;
        order.forEach(function (id, i) { var x = el(id), wr = wrapOf(id); if (x && (x === t || (wr && wr.contains(t)))) cur = i; });
        if (cur < 0 || order[cur] === 'show') return;
        e.preventDefault();
        var nx = A.focusable(order[cur + 1]); if (nx) nx.focus();
    }
    d.addEventListener('keydown', function (e) {                          // VoucherValidation_KeyDown (KeyPreview)
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey) { enterAsTab(e); return; }
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); print(); }
        else if (k === 'l') { e.preventDefault(); setLang(true); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // ------------------------------------------------------------------ start
    el('datDueDateTo').value = A.today();
    grid.render();
    load();
}(window, document));
