/* countx_export_commercial_invoice_shipments.js - Commercial_Invoice_Shipments.cs, screen 258.
 * Data: /api/export/commercial-invoice-shipments. */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/commercial-invoice-shipments';
    var YEAR_START = '', ROWS = [], CUR = -1;
    var COLS = [
        { key: 'Id', hidden: true }, { key: 'Ship/BL Date', cap: 'Ship/BL Date', kind: 'short' }, { key: 'InvoiceNo', cap: 'Invoice No' }, { key: 'Fcl', cap: 'Fcl', kind: 'q3', sum: true },
        { key: 'Consignee', cap: 'Consignee' }, { key: 'Brand', cap: 'Brand' }, { key: 'PackUom', cap: 'Pack Uom' }, { key: 'FcyAmount', cap: 'Fcy Amount', kind: 'amt', sum: true },
        { key: 'ExchangeRate', cap: 'Exchange Rate', kind: 'rate' }, { key: 'LcyAmount', cap: 'Lcy Amount', kind: 'amt', sum: true }
    ];

    function render() { H.drawGrid('grdShipment', ROWS, COLS, { cur: CUR }); H.show('grdShipmentEmpty', !ROWS.length); }
    function dateType() { H.applyDateType(H.val('CmbDateType'), 'txtdatefrom', 'txtdateto', YEAR_START); }
    function bindCustomers(rows) { H.bind('cmbSupplierName', rows || [], 'Id', 'Name'); }
    function filters() { return { fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto'), customerId: H.netI(H.val('cmbSupplierName')) }; }
    function show(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/show', filters()).then(function (rows) { ROWS = rows || []; CUR = -1; render(); });
        });
    }
    /** Commercial_Invoice_Shipments_Load: ParameterFill, CustomerFill, Rows[2] "This Month", GridBind. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            YEAR_START = d.yearStart || '';
            H.bindDateTypes('CmbDateType', d.dateTypes);
            H.setText('txtdatefrom', H.today()); H.setText('txtdateto', H.today());
            bindCustomers(d.customers);
            H.setVal('CmbDateType', 3); dateType();
            H.$id('footerInfo').textContent = 'Commercial_Invoice_Shipments  -  Print 569';
            return show(null);
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }
    /** btnnew_Click: date type 3, customer cleared, grid cleared. */
    function reset() {
        H.setVal('CmbDateType', 3); dateType();
        H.setVal('cmbSupplierName', 0);
        ROWS = []; CUR = -1; render();
    }
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindCustomers); }); }
    function print(btn) {
        if (!ROWS.length) { H.box('No Record Found For Display'); return; }
        var f = filters();
        return H.print('569-CommercialInvoice_Shipments.rpt', { fromDate: f.fromDate, toDate: f.toDate, supplierCustomerId: f.customerId || undefined }, btn);
    }
    function shortcuts() { H.shortcuts([]); }
    function toggleHistory() { var b = H.$id('grdShipmentBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('grdShipment'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.$id('CmbDateType').addEventListener('change', dateType);
        H.wireGrid('grdShipment', { select: function (i) { CUR = i; } });
        H.wireKeys({ shortcuts: shortcuts, handle: function (e, k) {
            if (!e.ctrlKey) return;
            if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'r') { e.preventDefault(); refresh(H.$id('btnRefresh')); }
            else if (k === 's') { e.preventDefault(); show(H.$id('btnShow')); }
            else if (k === 'p') { e.preventDefault(); print(H.$id('btnRegisterReport')); }
            else if (e.key === 'F5') { e.preventDefault(); H.focus('CmbDateType'); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdShipment'); }
        } });
        load();
    });

    global.ExportCiShipments = { show: show, reset: reset, refresh: refresh, print: print, shortcuts: shortcuts, toggleHistory: toggleHistory };
}(window));
