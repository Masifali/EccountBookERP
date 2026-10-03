/* ============================================================================================
 * countx_export_sale_report.js - ExportSalesReport.cs (Architecture.WinApp.ExportReports), screen 270 "5004 Sale Report".
 * Data: /api/export/sale-report (USP_GetExportSales + the four ...FromVouchers combo procedures). Print: exp-359.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptB, API = '/api/export/sale-report';
    var $id = H.$id, box = H.box, esc = H.esc;
    var S = { rows: [], cur: -1, yearStart: '' };

    /* GridBind's dtData + GridSetting: dates dd-MMM-yy, MTon "#,##0.###" summed, Fcy amounts summed, ExchangeRate 4 decimals. */
    var COLS = [
        { key: 'VoucherCode', caption: 'Voucher Code', fmt: 'i' }, { key: 'VoucherDate', caption: 'Voucher Date', fmt: 'dmy' }, { key: 'DueDate', caption: 'Due Date', fmt: 'dmy' },
        { key: 'BLDate', caption: 'BL Date', fmt: 'dmy' }, { key: 'InvoiceNo', caption: 'Invoice No' }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'ItemName', caption: 'Item Name' },
        { key: 'MTon', caption: 'M Ton', num: true, sum: true, fmt: 'o3' }, { key: 'JobLot', caption: 'Job Lot' }, { key: 'FcAmount', caption: 'Fc Amount', num: true, sum: true, fmt: 'n2' },
        { key: 'FcyCode', caption: 'Fcy Code' }, { key: 'ExchangeRate', caption: 'Exchange Rate', num: true, fmt: 'n4' }, { key: 'Amount', caption: 'Amount', num: true, sum: true, fmt: 'n2' },
        { key: 'RealizedAmount', caption: 'Realized Amount', num: true, sum: true, fmt: 'n2' }, { key: 'InvoiceStatus', caption: 'Invoice Status' }
    ];

    /** frmExportShipingLineBookingRpt_Load: From = FY start, combos, GridBind. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {}; S.yearStart = H.isoDate(d.yearStart);
            ['invoices', 'customers', 'items', 'jobLots'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
            H.setText('datFromDate', S.yearStart || H.today()); H.setText('datToDate', H.today());
            H.bind('CmbInvoiceNo', d.invoices || []); H.bind('CmbCustomerName', d.customers || []); H.bind('CmbItemName', d.items || []); H.bind('CmbJobLot', d.jobLots || []);
            H.focus('datFromDate');
            return gridBind();
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function filters() {
        return { fromDate: H.checked('chkFromDate') ? H.val('datFromDate') : '', toDate: H.checked('chkToDate') ? H.val('datToDate') : '',
            supplierCustomerId: H.netI(H.val('CmbCustomerName')), invoiceId: H.netI(H.val('CmbInvoiceNo')), itemId: H.netI(H.val('CmbItemName')),
            jobLotId: H.netI(H.val('CmbJobLot')), actionId: H.netI(H.radio('realized')) };
    }
    /** GridBind + the LableWithValue currency cards (Sum(FcAmount) per CurrencyCode, stringFormatsingleForFcy). */
    function render() {
        H.drawGrid('grdfrm', COLS, S.rows, { cur: S.cur });
        H.show('grdEmpty', S.rows.length === 0);
        var sums = {}, order = [];
        S.rows.forEach(function (r) { var c = H.str(r.FcyCode); if (!(c in sums)) { sums[c] = 0; order.push(c); } sums[c] += H.netD(r.FcAmount); });
        $id('CurrencyFlowLayout').innerHTML = order.map(function (c) { return '<div class="rptb-card"><span>' + esc(c) + '</span><b>' + esc(H.fmtFixed(sums[c], 2)) + '</b></div>'; }).join('');
    }
    function gridBind() { return H.postJson(API + '/show', filters()).then(function (rows) { S.rows = rows || []; S.cur = -1; render(); }); }
    function show(btn) { return H.busy(btn, function () { return gridBind().catch(function (e) { box(e.message); }); }); }
    /** Reset(): Customer / Invoice emptied, From = FY start, focus, GridBind. */
    function reset(btn) { H.setVal('CmbCustomerName', '0'); H.setVal('CmbInvoiceNo', '0'); H.setText('datFromDate', S.yearStart || H.today()); H.focus('datFromDate'); return show(btn); }
    /** btnReport_Click: 359-GetExportSales.rpt on dtdetail. */
    function print359(btn) {
        if (!S.rows.length) { box('Not Record Found For Display'); return; }
        var f = filters();
        return H.print('exp-359', { supplierCustomerId: f.supplierCustomerId, exImInvoiceNoId: f.invoiceId, fromDate: f.fromDate, toDate: f.toDate, itemId: f.itemId, jobLotId: f.jobLotId, actionId: f.actionId }, btn);
    }

    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        H.gridEvents('grdfrm', { select: function (i) { S.cur = i; } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, null)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset($id('btnnew')); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); print359($id('btnReport')); }
        });
        load();
    });

    global.ExportSal = { show: show, reset: reset, print359: print359 };
}(window));
