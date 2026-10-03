/* ============================================================================================
 * countx_export_comparison_summary.js - ExportComparisonSummaryReport.cs, screen 248 "Export Comparison Summary".
 * Data: /api/export/comparison-summary (SpStaticColumnNames 'ExportComparisonsSummery', the six combo sources,
 * SpExImInvoice_ExportComparisonsSummery_Report). Print 536 through CrystalPrint key "exp-536" (@ReportType = the combo text).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, N = global.ExportRptN, $id = H.$id, box = H.box;
    var API = '/api/export/comparison-summary';
    var S = { rows: [], last: null, decA: 0, decR: 2 };
    var COMBOS = [['customers', 'CmbCustomerName'], ['contracts', 'CmbContractNo'], ['items', 'CmbItemName'], ['itemCategories', 'CmbItemCategory'],
        ['itemTypes', 'CmbItemType'], ['seaPorts', 'CmbSeaPort']];

    function cols() { /* GridBind dtcol + GridSetting + GridWrappingAndColumnSettings (decimal columns: Amount -> Σ stringFormatsingle,
                         Rate -> DecimalRateFormate, others -> Σ "#,##0.##") */
        var single = function (v) { return N.fmtSingle(v, S.decA); }, rate = function (v) { return N.fmtRate(v, S.decR); }, o2 = function (v) { return N.fmtUpTo(v, 2); };
        var cap = S.rows.length ? H.str(S.rows[0].GroupCaption) : 'GroupName';
        return [
            { key: 'GroupCaption', hidden: true }, { key: 'GroupName', caption: cap || 'GroupName' },
            { key: 'MTons', caption: 'MTons', num: true, sum: true, fmt: o2 }, { key: 'FcyCode', caption: 'FcyCode' },
            { key: 'AvgRate', caption: 'AvgRate', num: true, fmt: rate }, { key: 'Fcy_Amount', caption: 'Fcy_Amount', num: true, sum: true, fmt: single },
            { key: 'ExchangeRate', caption: 'ExchangeRate', num: true, fmt: rate }, { key: 'LcyAmount', caption: 'LcyAmount', num: true, sum: true, fmt: single },
            { key: 'TotalMTons', hidden: true }, { key: 'PrctExport', caption: 'PrctExport', num: true, sum: true, fmt: o2 }
        ];
    }
    function applyCombos(d) {
        COMBOS.forEach(function (c) {
            if (d[c[0] + 'Error']) box(d[c[0] + 'Error']);
            /* each Bind runs only when its source has rows (an empty source leaves the previous list) */
            if ((d[c[0]] || []).length) H.bind(c[1], d[c[0]], 'Id', 'name');
        });
    }
    function filters() {
        return { reportTypeId: N.intVal('CmbReportType'), reportTypeText: H.selText('CmbReportType').trim(),
            fromChecked: H.checked('datFromDateChk'), fromDate: H.val('datFromDate'), toChecked: H.checked('datToDateChk'), toDate: H.val('datToDate'),
            itemId: N.intVal('CmbItemName'), supplierCustomerId: N.intVal('CmbCustomerName'), lcContractId: N.intVal('CmbContractNo'),
            itemTypeId: N.intVal('CmbItemType'), itemCategoryId: N.intVal('CmbItemCategory'), destinationPortId: N.intVal('CmbSeaPort') };
    }
    /** GridBind: "Report Type filed is required" (server), rows -> grid, none -> ClearStructure. dtdetail keeps the last result. */
    function gridBind() {
        var f = filters();
        return H.postJson(API + '/show', f).then(function (d) {
            S.rows = (d && d.rows) || [];
            S.last = f;
            if (S.rows.length) H.drawGrid('grdfrm', cols(), S.rows, {});
            else { $id('grdfrm').querySelector('thead').innerHTML = ''; H.clearGrid('grdfrm'); }
            H.show('grdfrmEmpty', !S.rows.length);
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn || 'BtnShow', gridBind); }
    /** btnnew_Click -> Reset: the seven combos (Report Type included) cleared, focus To Date, GridBind (-> "Report Type filed is required"). */
    function btnNew(btn) {
        return H.busy(btn || 'btnnew', function () {
            ['CmbReportType', 'CmbItemCategory', 'CmbItemType', 'CmbCustomerName', 'CmbItemName', 'CmbContractNo', 'CmbSeaPort'].forEach(function (id) { H.setVal(id, '0'); });
            H.focus('datToDate');
            return gridBind();
        });
    }
    /** btnRefreshSummary_Click: the six combo binds again (Report Type is not re-read). */
    function refresh(btn) {
        return H.busy(btn || 'btnRefreshSummary', function () {
            return H.getJson(API + '/combos').then(function (d) { applyCombos(d || {}); }).catch(function (e) { box(e.message); });
        });
    }
    /** btnReport_Click: "Not Record Found For Display" while dtdetail is empty; 536 re-runs the last GridBind's query. */
    function print(btn) {
        var l = S.last;
        if (!S.rows.length || !l) { box('Not Record Found For Display'); return; }
        return H.print('exp-536', N.args({ reportType: l.reportTypeText, fromDate: l.fromChecked ? l.fromDate : '', toDate: l.toChecked ? l.toDate : '',
            supplierCustomerId: l.supplierCustomerId, destinationPortId: l.destinationPortId, itemId: l.itemId, itemTypeId: l.itemTypeId,
            itemCategoryId: l.itemCategoryId, lcContractId: l.lcContractId }), btn || $id('btnReport'));
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+P', 'For Print'], ['Ctrl+S', 'For Show'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    document.addEventListener('DOMContentLoaded', function () {
        N.embedFooter('cmpFooter');
        H.fullscreenButtons();
        H.gridEvents('grdfrm', {});
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); show(); }
            else if ((e.ctrlKey && k === 'p') || (e.altKey && !e.ctrlKey && (e.key === '1' || e.code === 'Digit1'))) { e.preventDefault(); print(); }
            else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); H.focus('datFromDate'); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdfrm'); }
        });
        H.setText('datFromDate', H.today());
        H.setText('datToDate', H.today());
        /* Load: datFromDate = ActiveYr.Start_Period, ReportTypeBind (Rows[0].Activate), the six binds, GridBind. */
        H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            S.decA = H.netI(d.decimalsAmount); S.decR = H.netI(d.decimalsRate) || 2;
            if (d.yearStart) H.setText('datFromDate', d.yearStart);
            if (d.reportTypesError) box(d.reportTypesError);
            if ((d.reportTypes || []).length) { H.bind('CmbReportType', d.reportTypes, 'Id', 'name'); H.setVal('CmbReportType', String(d.reportTypes[0].Id)); }
            applyCombos(d);
            H.focus('datFromDate');
        }).catch(function (e) { box(e.message); }).then(function () { return show($id('BtnShow')); });
    });
    global.ExportCmp = { btnNew: btnNew, refresh: refresh, show: show, print: print, shortcuts: shortcuts };
}(window));
