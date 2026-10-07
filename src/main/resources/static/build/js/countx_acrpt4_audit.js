/* ============================================================================================
 * Screen 56 "Un_Balanced Voucher Report" - Architecture.WinApp.Account_Reports.AuditDashboard
 * Page: templates/accounts/reports/acrpt4_un_balanced_voucher.html (route /accounts/reports/un-balanced-voucher-report).
 * Every desktop handler, in desktop order (AuditDashboard.cs):
 *   VoucherValidation_Load   -> fromdate.Focus(), fromdate = ActiveYr.Start_Period, todate = today, btnshow_Click(null, null).
 *   btnshow_Click            -> Dashboard.ReadUnBalancedVoucher: Sp_Accounts_AuditDashboard_Rpt (FinancialYearId, FromDate, ToDate, ActivityId 2);
 *                               rows > 0 -> grd.DataSource = dt; RetrieveStructure(); GridSettings() (only the saved grid layout);
 *                               no rows -> the grid is left exactly as it was (nothing is cleared).
 *   btnNew_Click / reset     -> btnshow_Click (the toolbar "Refresh").
 *   print_Click_1            -> ShowReport(), an empty method (Ctrl+P does nothing).
 *   VoucherValidation_KeyDown (KeyPreview) -> Ctrl+P, Ctrl+E / Esc close, Ctrl+N refresh. (No Enter = Tab on this form.)
 *   ctrlGrdBar1_Load         -> saved grid layout (countx_grid_bar.js).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt4, el = A.el, API = '/api/accounts/acrpt4/audit', seq = 0;
    var grid = A.createGrid({ table: el('grd'), scroller: el('gridWrap'), nav: el('nav'), navText: el('navText'),
        headerLines: 1, groupTotals: true, totalRow: true });          // GroupByBoxVisible = false: no group box bar
    grid.clear();

    function show() {                                                   // btnshow_Click
        var b = el('show'); if (b.disabled) return Promise.resolve();
        A.busy(b, true); A.busy(el('refresh'), true);
        var token = ++seq;
        var q = new URLSearchParams({ fromDate: el('fromDate').value || A.today(), toDate: el('toDate').value || A.today() });
        return A.getJson(API + '/data?' + q.toString()).then(function (data) {
            if (token !== seq) return;
            if (data && data.rows && data.rows.length) grid.setData(A.inferColumns(data), data.rows);   // else: left unchanged
        }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); A.busy(el('refresh'), false); });
    }
    function reset() { return show(); }                                // btnNew_Click -> reset() -> btnshow_Click
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    el('show').addEventListener('click', show);
    el('refresh').addEventListener('click', reset);
    d.addEventListener('keydown', function (e) {                       // VoucherValidation_KeyDown
        if (e.key === 'Escape') { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') e.preventDefault();                              // ShowReport() is empty
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // VoucherValidation_Load
    el('fromDate').value = A.today(); el('toDate').value = A.today();
    A.getJson(API + '/init').catch(function () { return {}; }).then(function (init) {
        el('fromDate').focus();
        if (init && init.yearStart) el('fromDate').value = init.yearStart;     // ActiveYr.Start_Period
        el('toDate').value = A.today();
        return show();
    });
}(window, document));
