/* ============================================================================================
 * Screen 63 "Payables By Due Report" - Architecture.WinApp.Account_Reports.PayablesByDueDate (PayablesByDueDate.cs, 1658 lines)
 * Page: templates/accounts/reports/acrpt1_payables_by_due_date.html (route /accounts/reports/payables-by-due-date).
 * Group R1, every desktop handler (line numbers are PayablesByDueDate.cs, .NET-decompiled):
 *   VoucherValidation_Load  :290  lbllang + CmbLanguage hidden; Datetypefill (:262: CommonServices.DateType = This Day / This Week / This Month /
 *                                 This Year / Financial Year with a "...Select Any Value..." row 0, Rows[2] activated = This Week), CustomerGroupFill
 *                                 (:210: Sp_CustomerGroup_GetAllMethod ReadAll minus Ids 7,9,10,12,13,14,15, checked, caption CustomerGroup),
 *                                 LanguageDropdownBind (:249: Sp_MultiLanguages_GetAll ReadAll), cmbDateType.Focus()
 *   cmbDateType_ValueChanged :769 1 From = today | 2 From = today - 7 | 3 From = first of (UTC) month, To = today | 4 From = 1 Jan, To = today |
 *                                 5 From = ActiveYr.Start_Period
 *   btnshow_Click           :341  GroupIds = id + "," for each checked text; rdPayablesByDueDate (Visible=false, Checked=true) ->
 *                                 VoucherReports.DueByDatePayablesAndReceivables = Sp_DueByDatePayablesAndReceivables (OrganizationId, CompanyId, UserId,
 *                                 DueDateTo always; CustomerGroupIds / BalanceFrom (ToInt text) / BalanceTo / LanguageId only when set);
 *                                 no rows -> grd.DataSource = null + "Record Not Found For Display"
 *   GridSettings            :492  HeaderLines 2, Groups ParentAccount (HideWhenGrouped), hidden ReportCriteria / AccountId / OtherLanguageTitle / Opening /
 *                                 CurrDebit / CurrCredit / AccountClass, widths, dd-MMM-yy dates, stringFormatboth amounts, sums, AccountCode link,
 *                                 format conditions on DueDays: Green (between Blue and Green), Blue (between Red and Blue), Red (>= Red), bold
 *   grd_LinkClicked         :678  GoToGeneralLedgerFromLinkedEvent(AccountId, datDueDateFrom, datDueDateTo)
 *   print_Click_1           :330  no rows -> "Not Record Found For Display"; 142-DueByDatePayablesAndReceivables.rpt over dtrpt (@CompanyAddress, @CompanyName)
 *   btnNew_Click / reset    :309  language hidden, Datetypefill, cmbDateType.Focus()
 *   btnRefresh_Click        :798  CustomerGroupFill + LanguageDropdownBind
 *   btnshortcutkey_Click / MakeShortCutKeys :183 ShortCutKeyPopUp, btnshow_Leave :818 (focus on the grid -> cmbDateType)
 *   txtBalFrom / txtBalTo / txtGreen / txtBlue / txtRed KeyPress -> OnlytextNumberFunction (digits + control keys)
 *   VoucherValidation_KeyDown :141 Ctrl+P, Ctrl+E / Esc, Ctrl+S, Ctrl+N, Ctrl+R, Ctrl+F5, Ctrl+Up, Ctrl+Down, Ctrl+Alt
 *   Not ported: ctrlGrdBar1_Load (layout restore), PayablesByDueDate_FormClosing (HistoryStack), GridSettingsDueDateBetween /
 *   rdPayablesByDueDateBetween (Visible = false, and nothing in the form can select it; its Show branch does nothing).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, el = A.el;
    var API = '/api/accounts/acrpt1/payables-due';
    var lookup = { amountDecimals: 0, yearStart: null };
    var dtrpt = [], lastArgs = null, conds = null, seq = 0;
    var DATE_TYPES = [[0, '...Select Any Value...'], [1, 'This Day'], [2, 'This Week'], [3, 'This Month'], [4, 'This Year'], [5, 'Financial Year']];

    function wrapOf(id) { var s = el(id); return s ? (s.closest('.dtcombo-wrap') || s) : null; }

    /* Datetypefill: BindDDL(ZeroIndex true) + Rows[2].Activate() */
    function dateTypeFill() {
        A.fill(el('cmbDateType'), DATE_TYPES, function (r) { return r[0]; }, function (r) { return r[1]; });
        el('cmbDateType').options[0].remove();                         // fill() added an empty slot; this list has its own row 0
        el('cmbDateType').value = '2';
        el('cmbDateType').dispatchEvent(new Event('change', { bubbles: true }));
    }
    el('cmbDateType').addEventListener('change', function () {          // cmbDateType_ValueChanged
        var id = A.toInt(this.value), t = A.today(), from = el('datDueDateFrom'), to = el('datDueDateTo');
        var u = new Date();                                             // DateTime.UtcNow for This Month
        if (id === 1) from.value = t;
        else if (id === 2) from.value = A.addDays(t, -7);
        else if (id === 3) { from.value = u.getUTCFullYear() + '-' + ('0' + (u.getUTCMonth() + 1)).slice(-2) + '-01'; to.value = t; }
        else if (id === 4) { from.value = new Date().getFullYear() + '-01-01'; to.value = t; }
        else if (id === 5) { if (lookup.yearStart) from.value = lookup.yearStart; }
    });

    function bindGroups(rows) { A.fill(el('cmbGroup'), rows, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'Description'); }); }
    function bindLanguages(rows) { A.fill(el('cmbLanguage'), rows, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'LanguageDescription'); }); }

    function load() {
        var x = wrapOf('cmbLanguage'); if (x) x.style.display = 'none';        // lbllang / CmbLanguage Visible = false
        dateTypeFill();
        return A.getJson(API + '/lookups').then(function (data) {
            lookup = data || lookup;
            bindGroups(lookup.customerGroups); bindLanguages(lookup.languages);
            el('cmbDateType').dispatchEvent(new Event('change', { bubbles: true }));   // financial year start is known now
            var f = A.focusable('cmbDateType'); if (f) f.focus();
        }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------ grid
    function dt(v) { return v === null || v === undefined || v === '' ? '1900-01-01' : v; }     // Conversion.ToDateTime(null) = 1900-01-01
    function colDefs() {
        function amt(key, width) { return { key: key, caption: key, width: width, type: 'amount', sum: true }; }
        return [
            { key: 'ReportCriteria', caption: 'ReportCriteria', hidden: true },
            { key: 'AccountId', caption: 'AccountId', hidden: true },
            { key: 'AccountCode', caption: 'AccountCode', width: 100, link: true },
            { key: 'AccountTitle', caption: 'AccountTitle', width: 200 },
            { key: 'OtherLanguageTitle', caption: 'OtherLanguageTitle', hidden: true },
            { key: 'ParentAccount', caption: 'ParentAccount', width: 150, hidden: true },           // grouped, HideWhenGrouped
            { key: 'AccountType', caption: 'AccountType', width: 65 },
            { key: 'AccountClass', caption: 'AccountClass', width: 80, hidden: true },
            { key: 'Opening', caption: 'Opening', width: 90, hidden: true, type: 'amount' },
            { key: 'CurrDebit', caption: 'CurrDebit', width: 90, hidden: true, type: 'amount' },
            { key: 'CurrCredit', caption: 'CurrCredit', width: 90, hidden: true, type: 'amount' },
            { key: 'DueDays', caption: 'DueDays', width: 50, type: 'int', align: 'r' },
            { key: 'DueDate', caption: 'DueDate', width: 80, type: 'date' },
            amt('DebitAmount', 110), amt('CreditAmount', 110), amt('DueAmount', 110),
            { key: 'LastPaymentDate', caption: 'LastPaymentDate', width: 90, type: 'date' },
            amt('LastPaymentAmount', 110),
            { key: 'PaymentDays', caption: 'PaymentDays', width: 70, type: 'int', align: 'r' },
            { key: 'LastBillDate', caption: 'LastBillDate', width: 80, type: 'date' },
            amt('LastBillsAmount', 110),
            { key: 'BillDays', caption: 'BillDays', width: 50, type: 'int', align: 'r' }
        ];
    }
    function mapRow(r) {                                                // dt.Rows.Add(Conversion.ToInt/ToString/ToDouble/ToDateTime(...))
        var c = A.ci, s = function (v) { return v == null ? '' : String(v); };
        return {
            'ReportCriteria': A.toInt(c(r, 'ReportCriteria')), 'AccountId': A.toInt(c(r, 'AccountId')), 'AccountCode': A.toInt(c(r, 'AccountCode')),
            'AccountTitle': s(c(r, 'AccountTitle')), 'OtherLanguageTitle': s(c(r, 'OtherLanguageTitle')), 'ParentAccount': s(c(r, 'ParentAccount')),
            'AccountType': s(c(r, 'AccountType')), 'AccountClass': s(c(r, 'ClassName')), 'Opening': 0, 'CurrDebit': 0, 'CurrCredit': 0,
            'DueDays': A.toInt(c(r, 'DueDays')), 'DueDate': dt(c(r, 'DueDate')),
            'DebitAmount': A.toDouble(c(r, 'DebitAmount')), 'CreditAmount': A.toDouble(c(r, 'CreditAmount')), 'DueAmount': A.toDouble(c(r, 'DueAmount')),
            'LastPaymentDate': dt(c(r, 'LastPaymentDate')), 'LastPaymentAmount': A.toDouble(c(r, 'LastPaymentAmount')), 'PaymentDays': A.toDouble(c(r, 'PaymentDays')),
            'LastBillDate': dt(c(r, 'LastBillDate')), 'LastBillsAmount': A.toDouble(c(r, 'LastBillsAmount')), 'BillDays': A.toDouble(c(r, 'BillDays'))
        };
    }
    /* GridEXFormatCondition on DueDays (FormatStyle bold + colour) - the first matching condition in the order Green, Blue, Red */
    function cellStyle(col, row) {
        if (!conds || col.key !== 'DueDays') return null;
        var v = A.toInt(A.ci(row, 'DueDays'));
        for (var i = 0; i < conds.length; i++) {
            var k = conds[i];
            if (k.between ? (v >= k.lo && v <= k.hi) : v >= k.lo) return { color: k.color, bold: true };
        }
        return null;
    }
    var grid = new A.Grid({
        tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', groupBy: 'ParentAccount', groupTotals: true,
        decimals: function () { return lookup.amountDecimals; }, cellStyle: cellStyle, onLink: function (row) { ledger(row); }
    });

    // ------------------------------------------------------------------ Show
    function show() {
        var b = el('show'); if (b.disabled) return;
        var groupIds = '';                                              // id + "," per checked text
        A.checkedValues(el('cmbGroup')).forEach(function (v) { groupIds += v + ','; });
        var args = { dueDateTo: el('datDueDateTo').value, ids: groupIds, fromDocNo: A.toIntText(el('txtBalFrom').value),
                     toDocNo: A.toIntText(el('txtBalTo').value), languageId: A.toInt(el('cmbLanguage').value) };
        if (!args.dueDateTo) { alert('Please select the Due Date To'); return; }
        var green = A.toIntText(el('txtGreen').value), blue = A.toIntText(el('txtBlue').value), red = A.toIntText(el('txtRed').value);
        var cond = [];
        if (el('chkGreen').checked) cond.push({ between: true, lo: blue, hi: green, color: 'Green' });
        if (el('chkBlue').checked) cond.push({ between: true, lo: red, hi: blue, color: 'Blue' });
        if (el('chkRed').checked) cond.push({ between: false, lo: red, color: 'Red' });
        A.busy(b, true);
        var token = ++seq;
        A.getJson(API + '?' + new URLSearchParams(args).toString()).then(function (rows) {
            if (token !== seq) return;
            dtrpt = rows || [];
            if (!dtrpt.length) { lastArgs = null; conds = null; grid.clear(); alert('Record Not Found For Display'); return; }
            lastArgs = args; conds = cond;
            grid.setData(colDefs(), dtrpt.map(mapRow));
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
        if (!dtrpt.length || !lastArgs) { alert('Not Record Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/accounts/reports/acrpt1/print/payables-due?' + new URLSearchParams(lastArgs).toString())
            .catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() { dateTypeFill(); var f = A.focusable('cmbDateType'); if (f) f.focus(); }   // language stays hidden
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        A.getJson(API + '/refresh').then(function (data) { bindGroups(data.customerGroups); bindLanguages(data.languages); })
            .catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
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
    ['txtBalFrom', 'txtBalTo', 'txtGreen', 'txtBlue', 'txtRed'].forEach(function (id) {        // OnlytextNumberFunction
        el(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/\d/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        el(id).addEventListener('input', function () { var v = this.value.replace(/\D/g, ''); if (v !== this.value) this.value = v; });
    });
    el('show').addEventListener('blur', function (e) {                    // btnshow_Leave
        if (e.relatedTarget && el('grid').contains(e.relatedTarget)) { var f = A.focusable('cmbDateType'); if (f) f.focus(); }
    });
    d.addEventListener('keydown', function (e) {                          // VoucherValidation_KeyDown (KeyPreview)
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
        else if (k === 'f5' || k === 'arrowup') { e.preventDefault(); var f = A.focusable('cmbDateType'); if (f) f.focus(); }
        else if (k === 'arrowdown') { e.preventDefault(); grid.focus(); }
    });

    // ------------------------------------------------------------------ start
    el('datDueDateFrom').value = A.today();
    el('datDueDateTo').value = A.today();
    grid.render();
    load();
}(window, document));
