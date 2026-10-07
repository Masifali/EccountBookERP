/* ============================================================================================
 * Screen 55 "Balance Sheet Report" - Architecture.WinApp.Account_Reports.AccountsBalanceSheetStandardRpt
 * (Account_Reports/AccountsBalanceSheetStandardRpt.cs, 717 lines). Page: templates/accounts/reports/inact2_balance_sheet_standard.html
 * (route /accounts/reports/balance-sheet-standard). Group K (Inact2). Every desktop handler in desktop order (line numbers are the .cs):
 *   AccountsBalanceSheetStandardRpt_Load :82   txtfromdate = ActiveYr.Start_Period; focus; GridBind(); GridBind1()  (txtToDate = today, the DateTimePicker default)
 *   btnshow_Click_1         :97   GridBind(); GridBind1()  (each has its own try/catch -> MessageBox)
 *   GridBind                :110  VoucherReports.AccountsBalanceSheetStandardRpt = Sp_Accounts_BalanceSheetStandard_Rpt (FinancialYearId, OrganizationId, CompanyId, ToDate);
 *                                 rows > 0 -> the 11-column table Class_Name, Ac1LevelCode .. Ac4LevelTitle (strings), ClosingBalance (double), Column1 -> DataGridHistory
 *                                 (RetrieveStructure, GridSetting); NO rows -> the grid is left as it was
 *   GridBind1               :159  VoucherReports.AccountsProfitLoassStandard = Sp_Accounts_ProfitLoassStandard_Rpt (+ FromDate) -> the same table -> DataGridHistory1, Grid1Setting
 *   GridSetting             :210  ClosingBalance: AggregateFunction Sum, right, FormatString + TotalFormatString = stringFormatsingle, GridAutoAdjustment
 *   Grid1Setting            :226  the SAME statements, but written against DataGridHistory (not DataGridHistory1): the right grid keeps the unformatted ClosingBalance, no Sum
 *   Reset / btnnew_Click    :242 / :255  txtfromdate = txtToDate = now (no re-Show)
 *   KeyDown                 :267  Ctrl+E / Esc close, Ctrl+N reset (no Enter-to-Tab on this form)
 *   btnProfitLoosePrint     :286  152-Profit Loss-I: AccountsProfitLoassStandard rows -> 152-AcRptAccountsProfitLoss.rpt  (@CompanyAddress, @CompanyName)
 *   btnBalanceSheetPrint    :325  151-Balance Sheet Print: AccountsBalanceSheetStandardRpt rows -> 151-AcRptAccountsBalanceSheetStandard.rpt
 *   btnProfitlossFormat2    :362  154-Profit Loss-II: VoucherReports.ProfitandLoss (ToDate only) = SpAccounts_ProfitLoassFormatA_Report -> 154-Profit&Loss.rpt
 *   btnBalanceSheetFormat2  :399  153-BalanceSheet-II: AccountsBalanceSheetStandardFormatII (UserId, From, To) = SpAccounts_BalanceSheetFormatA_Report -> 153-BalanceSheetStatementRpt.rpt
 *   Every print: no rows -> "Not Record Found For Display"; the server re-runs the same BLL call and renders the template.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, I = w.Inact2, el = A.el;
    var API = '/api/accounts/inact2';
    var P = '/accounts/reports/inact2/print/';
    var lookup = { amountDecimals: 0, yearStart: null };

    function str(v) { return v === null || v === undefined ? '' : String(v); }        // Conversion.ToString
    var TEXT = ['Class_Name', 'Ac1LevelCode', 'Ac1LevelTitle', 'Ac2LevelCode', 'Ac2LevelTitle', 'Ac3LevelCode', 'Ac3LevelTitle', 'Ac4LevelCode', 'Ac4LevelTitle'];
    var WIDTH = { Class_Name: 110, Ac1LevelCode: 90, Ac1LevelTitle: 150, Ac2LevelCode: 90, Ac2LevelTitle: 150, Ac3LevelCode: 90, Ac3LevelTitle: 150, Ac4LevelCode: 90, Ac4LevelTitle: 190, Column1: 90 };

    function cols(formatted) {
        var out = TEXT.map(function (k) { return { key: k, caption: k, width: WIDTH[k] }; });
        var c = { key: 'ClosingBalance', caption: 'ClosingBalance', width: 120, type: 'amount', align: 'r' };
        if (formatted) {                                                           // GridSetting: Sum + stringFormatsingle (row and total)
            c.sum = true;
            c.fmt = function (v) { return I.fmtSingle(v, lookup.amountDecimals); };
            c.totalFmt = function (s) { return I.fmtSingle(s, lookup.amountDecimals); };
        } else c.fmt = function (v) { return v === null || v === undefined || v === '' ? '' : String(v); };
        out.push(c);
        out.push({ key: 'Column1', caption: 'Column1', width: WIDTH.Column1 });
        return out;
    }
    /* dtGrid.Rows.Add(Conversion.ToString(...), ..., Conversion.ToDouble(ClosingBalance), Conversion.ToString(Column1)) */
    function table(rows) {
        return rows.map(function (r) {
            var o = {};
            TEXT.forEach(function (k) { o[k] = str(A.ci(r, k)); });
            o.ClosingBalance = A.toDouble(A.ci(r, 'ClosingBalance'));
            o.Column1 = str(A.ci(r, 'Column1'));
            return o;
        });
    }

    var grid = new A.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', groupTotals: false, decimals: function () { return lookup.amountDecimals; } });
    var grid1 = new A.Grid({ tableId: 'results1', gridId: 'grid1', navId: 'nav1', navTextId: 'navText1', groupTotals: false, decimals: function () { return lookup.amountDecimals; } });
    I.totals(grid);
    I.totals(grid1, true);

    // ------------------------------------------------------------------ GridBind / GridBind1
    function gridBind() {
        var q = new URLSearchParams({ toDate: el('txtToDate').value });
        return A.getJson(API + '/balance-sheet?' + q.toString()).then(function (rows) {
            if (rows && rows.length) grid.setData(cols(true), table(rows));          // rows == 0: the grid is not touched
        }).catch(function (e) { alert(e.message); });
    }
    function gridBind1() {
        var q = new URLSearchParams({ fromDate: el('txtfromdate').value, toDate: el('txtToDate').value });
        return A.getJson(API + '/profit-loss?' + q.toString()).then(function (rows) {
            if (rows && rows.length) grid1.setData(cols(false), table(rows));
        }).catch(function (e) { alert(e.message); });
    }
    function show() {                                                                // btnshow_Click_1
        var b = el('btnshow'); if (b.disabled) return Promise.resolve();
        if (!el('txtfromdate').value || !el('txtToDate').value) { alert('Please select the From Date and To Date'); return Promise.resolve(); }
        A.busy(b, true);
        return gridBind().then(gridBind1).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ prints
    function dates() { return { fromDate: el('txtfromdate').value, toDate: el('txtToDate').value }; }
    function q(o) { return new URLSearchParams(o).toString(); }
    function printProfitLoss1() { I.openPrint(el('btnProfitLoosePrint'), P + 'profit-loss-152?' + q(dates())); }
    function printBalanceSheet1() { I.openPrint(el('btnBalanceSheetPrint'), P + 'balance-sheet-151?' + q({ toDate: el('txtToDate').value })); }
    function printProfitLoss2() { I.openPrint(el('btnProfitlossFormat2'), P + 'profit-loss-154?' + q({ toDate: el('txtToDate').value })); }
    function printBalanceSheet2() { I.openPrint(el('btnBalanceSheetFormat2'), P + 'balance-sheet-153?' + q(dates())); }

    function reset() {                                                               // Reset(): both dates = now
        el('txtfromdate').value = A.today();
        el('txtToDate').value = A.today();
    }

    // ------------------------------------------------------------------ events
    el('btnshow').addEventListener('click', show);
    el('btnReset').addEventListener('click', reset);
    el('btnProfitLoosePrint').addEventListener('click', printProfitLoss1);
    el('btnBalanceSheetPrint').addEventListener('click', printBalanceSheet1);
    el('btnProfitlossFormat2').addEventListener('click', printProfitLoss2);
    el('btnBalanceSheetFormat2').addEventListener('click', printBalanceSheet2);
    d.addEventListener('keydown', function (e) {                // AccountsBalanceSheetStandardRpt_KeyDown (KeyPreview)
        if (e.key === 'Escape') { e.preventDefault(); I.closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'e') { e.preventDefault(); I.closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // ------------------------------------------------------------------ start (AccountsBalanceSheetStandardRpt_Load)
    el('txtfromdate').value = A.today();
    el('txtToDate').value = A.today();                          // DateTimePicker default = Now
    grid.render(); grid1.render();
    A.getJson(API + '/balance-sheet/lookups').then(function (data) {
        lookup = data || lookup;
        el('txtfromdate').value = lookup.yearStart || A.today();                    // ActiveYr.Start_Period
        el('txtfromdate').focus();
        return gridBind().then(gridBind1);
    }).catch(function (e) { alert(e.message); });
}(window, document));
