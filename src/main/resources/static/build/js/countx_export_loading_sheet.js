/* ============================================================================================
 * countx_export_loading_sheet.js - ExportLoadingSheet.cs (Architecture.WinApp.ExportReports), screen 262 "5013 Loading Sheet".
 * Data: /api/export/loading-sheet (USP_ExportLoadSheet + USP_GetDataForDropDownFromGoodsForwarding). Print: exp-556.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptB, API = '/api/export/loading-sheet';
    var $id = H.$id, box = H.box;
    var S = { rows: [], cur: -1, yearStart: '', suspend: true };

    /* GridFill's dt + GridSetting: Id hidden, weights "#,##0.###" summed, LoadingDate dd-MMM-yy. */
    var COLS = [
        { key: 'Id', hidden: true }, { key: 'LoadingDate', caption: 'Loading Date', fmt: 'dmy' }, { key: 'InvoiceNo', caption: 'Invoice No' },
        { key: 'ContainerNo', caption: 'Container No' }, { key: 'SealNo', caption: 'Seal No' }, { key: 'NoofBagsMaster', caption: 'Noof Bags Master', num: true, sum: true, fmt: 'o3' },
        { key: 'NetWeightBag', caption: 'Net Weight Bag', num: true, sum: true, fmt: 'o3' }, { key: 'GrossWeightBag', caption: 'Gross Weight Bag', num: true, sum: true, fmt: 'o3' },
        { key: 'NetWeight', caption: 'Net Weight', num: true, sum: true, fmt: 'o3' }, { key: 'GrossWeight', caption: 'Gross Weight', num: true, sum: true, fmt: 'o3' },
        { key: 'BrandName', caption: 'Brand Name' }
    ];

    function bindCombos(c) { H.bind('cmbinvoiceno', c.invoices || []); H.bind('cmbsupcut', c.parties || []); }
    /** Datetypefill: Rows[2].Activate() -> with the zero row that is Id 2 "This Week". */
    function dateTypeFill() { H.setVal('cmbDateType', '2'); dateType(); }
    /** ExportLoadingSheet_Load: AllComboBind, Datetypefill, focus Date Type. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {}; S.yearStart = H.isoDate(d.yearStart);
            H.bind('cmbDateType', d.dateTypes || [], 'Id', 'Parameters');
            H.setText('txtdatefrom', H.today()); H.setText('txtdateto', H.today());
            if (d.combosError) box(d.combosError);
            bindCombos(d.combos || {});
            S.suspend = false;
            dateTypeFill();
            H.focus('cmbDateType');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function dateType() { if (!S.suspend) H.dateTypeChanged('cmbDateType', 'txtdatefrom', 'txtdateto', S.yearStart, false); }
    function filters() { return { fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto'), invoiceId: H.netI(H.val('cmbinvoiceno')), supplierCustomerId: H.netI(H.val('cmbsupcut')) }; }
    function render() { H.drawGrid('grd', COLS, S.rows, { cur: S.cur }); H.show('grdEmpty', S.rows.length === 0); }
    function show(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/show', filters()).then(function (rows) { S.rows = rows || []; S.cur = -1; render(); }).catch(function (e) { box(e.message); });
        });
    }
    /** Reset(): combos emptied, Datetypefill, focus Date Type (the grid is kept). */
    function reset() { H.setVal('cmbinvoiceno', '0'); H.setVal('cmbsupcut', '0'); dateTypeFill(); H.focus('cmbDateType'); }
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/setup').then(function (d) { bindCombos((d && d.combos) || {}); }).catch(function (e) { box(e.message); }); }); }
    function print556(btn) {
        if (!S.rows.length) { box('Not Record Found For Display'); return; }
        var f = filters();
        return H.print('exp-556', { eximInvoiceId: f.invoiceId, supplierCustomerId: f.supplierCustomerId, fromDate: f.fromDate, toDate: f.toDate }, btn);
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Print + P', 'For print'],
            ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On DateType in Filter']]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        $id('cmbDateType').addEventListener('change', dateType);
        H.gridEvents('grd', { select: function (i) { S.cur = i; } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'p') { e.preventDefault(); print556($id('btnprint')); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnShow')); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
            else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); H.focus('cmbDateType'); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grd'); }
        });
        load();
    });

    global.ExportLds = { show: show, reset: reset, refresh: refresh, print556: print556, shortcuts: shortcuts };
}(window));
