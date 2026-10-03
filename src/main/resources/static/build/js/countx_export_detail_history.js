/* ============================================================================================
 * countx_export_detail_history.js - ExportDetailHistoryReport.cs, screen 249 "Export Detail History Report".
 * Data: /api/export/detail-history (SpExImInvoice_ExportHistoryDetail_Report). Print 537 through the seeded
 * key "537-exportdetailhistoryreport". The grid shows the procedure's own columns (grdfrm.DataSource = dtdetail);
 * the report type only changes the grouping (GridSetting) - the BLL never forwards ReportType or JobLotId.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, $id = H.$id, box = H.box;
    var API = '/api/export/detail-history';
    var S = { rows: [] };
    var GROUP_BY = { 'Export Detail By Item': 'ItemName', 'Export Detail By Customer': 'CustomerName', 'Export Detail By Contract': 'LcOrderNo',
        'Export Detail By Item ItemType': 'ItemType', 'Export Detail By Item Category': 'ItemCategory' };
    var FMT = { NoOfPack: { num: true, sum: true, fmt: 'h2' }, NetWeight: { num: true, sum: true, fmt: 'h0' }, M_Tons: { num: true, sum: true, fmt: 'h0' },
        FcAmount: { num: true, sum: true, fmt: 'h0' }, RatePrice: { num: true, avg: true, fmt: 'h2' } };
    function isDateish(v) { return typeof v === 'string' && /^\d{4}-\d{2}-\d{2}$/.test(v); }
    function cols(rows, group) {
        if (!rows.length) return [];
        var first = rows[0], c = [];
        Object.keys(first).forEach(function (k) {
            var d = { key: k, caption: k.replace(/_/g, ' ') };
            if (k === 'CustomGroupName' || k === group) d.hidden = true;
            if (FMT[k]) { d.num = true; d.sum = FMT[k].sum; d.avg = FMT[k].avg; d.fmt = FMT[k].fmt; }
            else if (isDateish(first[k])) d.fmt = 'd';
            else if (typeof first[k] === 'number') d.num = true;
            c.push(d);
        });
        return c;
    }
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            H.setText('datFromDate', H.today()); H.setText('datToDate', H.today());
            [['reportTypes', 'CmbReportType'], ['customers', 'CmbCustomerName'], ['contracts', 'CmbContractNo'], ['items', 'CmbItemName'],
             ['categories', 'CmbItemCategory'], ['types', 'CmbItemType'], ['ports', 'CmbSeaPort'], ['jobLots', 'CmbJobLot']].forEach(function (p) {
                if (d[p[0] + 'Error']) box(d[p[0] + 'Error']);
                H.bind(p[1], d[p[0]], 'Id', 'name', false);
                $id(p[1]).selectedIndex = -1;
            });
            H.refreshCombos();
            H.focus('datToDate');
            return gridBind();
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function filters() {
        return { fromDate: H.val('datFromDate'), toDate: H.val('datToDate'), itemId: H.netI(H.val('CmbItemName')), supplierCustomerId: H.netI(H.val('CmbCustomerName')),
            lcOrderId: H.netI(H.val('CmbContractNo')), itemTypeId: H.netI(H.val('CmbItemType')), itemCategoryId: H.netI(H.val('CmbItemCategory')),
            destinationPortId: H.netI(H.val('CmbSeaPort')), jobLotId: H.netI(H.val('CmbJobLot')), reportType: H.selText('CmbReportType') };
    }
    function gridBind() {
        return H.postJson(API + '/show', filters()).then(function (rows) {
            S.rows = rows || [];
            var group = GROUP_BY[H.selText('CmbReportType').trim()] || null;
            H.drawGrid('grdfrm', cols(S.rows, group), S.rows, { group: group, groupTotals: !!group });
            H.show('grdfrmEmpty', !S.rows.length);
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn, gridBind); }
    /** btnnew_Click -> Reset(): combos blank (dates untouched), focus To, GridBind. */
    function btnNew(btn) {
        ['CmbItemCategory', 'CmbItemType', 'CmbCustomerName', 'CmbItemName', 'CmbContractNo', 'CmbSeaPort', 'CmbReportType'].forEach(function (id) { $id(id).selectedIndex = -1; });
        H.refreshCombos();
        H.focus('datToDate');
        return H.busy(btn, gridBind);
    }
    /** btnReport_Click: "Not Record Found For Display" without rows, else 537 with the same filters. */
    function btnReport(btn) {
        if (!S.rows.length) { box('Not Record Found For Display'); return; }
        var f = filters();
        return H.print('537-exportdetailhistoryreport', { activity: '', itemTypeId: f.itemTypeId, itemCategoryId: f.itemCategoryId, itemId: f.itemId,
            fromDate: f.fromDate, toDate: f.toDate, lcOrderId: f.lcOrderId, supplierCustomerId: f.supplierCustomerId, destinationPortId: f.destinationPortId }, btn);
    }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, null)) return;
            if (e.ctrlKey && (e.key || '').toLowerCase() === 'n') { e.preventDefault(); btnNew($id('btnnew')); }
        });
        load();
    });
    global.ExportDth = { btnNew: btnNew, show: show, btnReport: btnReport };
}(window));
