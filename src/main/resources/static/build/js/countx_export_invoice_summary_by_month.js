/* ============================================================================================
 * countx_export_invoice_summary_by_month.js - frmExportInvoiceSummaryByMonth.cs "Summary By Month" (no ScreenDefinition row).
 * Data: /api/export/invoice-summary-by-month (SpExImInvoiceSummaryByMonthly_Report). Print: exp-o-531
 * (531-ExImInvoiceSummeryByMonth.rpt) with the dates of the last GridBind (the desktop prints dtHistory).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptO, $id = H.$id, box = H.box;
    var API = '/api/export/invoice-summary-by-month';
    var DATE_TYPES = [{ Id: 1, Parameters: 'This Day' }, { Id: 2, Parameters: 'This Week' }, { Id: 3, Parameters: 'This Month' },
        { Id: 4, Parameters: 'This Year' }, { Id: 5, Parameters: 'Financial Year' }];       /* CommonServices.DateType() - fixed */
    var S = { rows: [], last: null, yearStart: '' };
    var COLS = [{ key: 'Customer', caption: 'Customer' }, { key: 'FcyCode', caption: 'FcyCode' }];
    ['July', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'June', 'Total'].forEach(function (m) {
        COLS.push({ key: 'Fcy' + m, caption: 'Fcy' + m, num: true, sum: true, fmt: 'o3' });
        COLS.push({ key: 'Pkr' + m, caption: 'Pkr' + m, num: true, sum: true, fmt: 'o3' });
    });
    /** GridBind(): dtHistory -> the form's DataTable; none -> ClearStructure. */
    function gridBind() {
        var f = { fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto') };
        return H.postJson(API + '/show', f).then(function (rows) {
            S.rows = rows || []; S.last = f;
            if (S.rows.length) { H.drawGrid('grdContractRegister', COLS, S.rows, {}); H.show('grdEmpty', false); }
            else { H.clearGrid('grdContractRegister'); $id('grdContractRegister').querySelector('thead').innerHTML = ''; H.show('grdEmpty', true); }
        }).catch(function (e) { box(e.message); });
    }
    function dateTypeChanged() { H.dateTypeChanged('cmbperemeter', 'txtdatefrom', 'txtdateto', S.yearStart); }
    function load() {
        H.setText('txtdatefrom', H.today()); H.setText('txtdateto', H.today());
        H.bind('cmbperemeter', DATE_TYPES, 'Id', 'Parameters', false);
        return H.getJson(API + '/setup').then(function (d) {
            S.yearStart = (d && d.yearStart) || '';
            /* Load: Value 0 -> Rows[1].Activate() ("This Week") -> ValueChanged */
            H.setVal('cmbperemeter', '2'); dateTypeChanged();
            return gridBind();
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn, gridBind); }
    /** Reset(): focus Date From, GridBind. */
    function btnNew(btn) { $id('txtdatefrom').focus(); return H.busy(btn, gridBind); }
    /** btnPrint_Click: "Not Record Found For Display" without rows, else 531. */
    function btnPrint(btn) {
        if (!S.rows.length || !S.last) { box('Not Record Found For Display'); return; }
        return H.print('exp-o-531', { fromDate: S.last.fromDate, toDate: S.last.toDate }, btn);
    }
    function shortcuts() {
        H.showShortcuts([['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+F5', 'For Focus on Date Type'],
            ['Ctrl+S', 'For Showing Data'], ['Alt+1', 'For Print 518'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        H.gridEvents('grdContractRegister', {});
        $id('cmbperemeter').addEventListener('change', dateTypeChanged);
        /* btnShow_PreviewKeyDown / btnShow_Leave: Tab (not Shift+Tab) out of Show returns to Date Type (keyboard only here) */
        $id('btnShow').addEventListener('keydown', function (e) {
            if (e.key === 'Tab' && !e.shiftKey) { e.preventDefault(); H.focus('cmbperemeter'); }
        });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew($id('btnnew')); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); H.focus('cmbperemeter'); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnShow')); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdContractRegister'); }
        });
        load();
    });
    global.ExportIsm = { show: show, btnNew: btnNew, btnPrint: btnPrint, shortcuts: shortcuts };
}(window));
