/* ============================================================================================
 * countx_export_shipment_costing_summary.js - frmExImShipmentCostingSummary.cs, screen 919 "5005 Shipment Costing Summary".
 * Data: /api/export/shipment-costing-summary (USP_ExportShipmentCosting_SummaryReport + USP_GetDataForDropDownFromExportInvoiceVoucher).
 * Print: exp-542-01. InvoiceNo link -> the Shipment Costing Detail page (ExImShipmentCosting, screen 238) with ?invoiceId=.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptB, API = '/api/export/shipment-costing-summary';
    var $id = H.$id, box = H.box;
    var LINKS = { shipmentCosting: '/export/shipment-costing' };
    var S = { rows: [], cur: -1, yearStart: '' };

    var CHARGES = ['CustomClearing', 'ForwarderCharges', 'Transportation', 'OtherCharges', 'PortAndClearing', 'OceanFreight', 'InspectionAndTesting', 'Insurance'];
    var CAPS = { CustomClearing: 'Custom Clearing', ForwarderCharges: 'Forwarder Charges', Transportation: 'Transportation', OtherCharges: 'Other Charges',
        PortAndClearing: 'Port & Clearing', OceanFreight: 'Ocean Freight', InspectionAndTesting: 'Inspection & Testing', Insurance: 'Insurance' };
    var pl = function (v) { var n = H.netD(v); return n > 0 ? 'rptb-green' : (n < 0 ? 'rptb-red' : ''); };

    /* GridBind's dt + GridSetting: hidden SupplierCustomerId, InvoiceId, ContractIds, ShipmentWeight, InvoiceWeight, PaymentTerms, LogisticsValue and
       every charge column whose total is 0; GridWrappingAndColumnSettings sums the numeric columns. */
    function cols(totals) {
        var c = [
            { key: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'ContractIds', hidden: true }, { key: "ContractNo's", caption: "ContractNo's" },
            { key: 'InvoiceId', hidden: true }, { key: 'InvoiceNo', caption: 'Invoice No', link: true }, { key: 'PaymentTerms', hidden: true }, { key: 'DeliveryTerm', caption: 'Delivery Term' },
            { key: 'InvoiceDate', caption: 'Invoice Date', fmt: 'd' }, { key: 'InvoiceWeight', hidden: true }, { key: 'InvoiceMTon', caption: 'Invoice M Ton', num: true, sum: true, fmt: 'o3' },
            { key: 'FcyCode', caption: 'Fcy Code' }, { key: 'ExchangeRate', caption: 'Exchange Rate', num: true, fmt: 'n4' }, { key: 'InvoiceAmount', caption: 'Invoice Amount', num: true, sum: true, fmt: 'n2' },
            { key: 'RealizedAmount', caption: 'Realized Amount', num: true, sum: true, fmt: 'n2' }, { key: 'InvoiceBalance', caption: 'Invoice Balance', num: true, sum: true, fmt: 'n2' },
            { key: 'SaleValue', caption: 'Sale Value', num: true, sum: true, fmt: 'o0' }, { key: 'CommissionValue', caption: 'Commission Value', num: true, sum: true, fmt: 'o0' },
            { key: 'StockValue', caption: 'Stock Value', num: true, sum: true, fmt: 'o0' }, { key: 'PackingValue', caption: 'Packing Value', num: true, sum: true, fmt: 'o0' }, { key: 'LogisticsValue', hidden: true }];
        CHARGES.forEach(function (k) { c.push({ key: k, caption: CAPS[k], num: true, sum: true, fmt: 'o0', hidden: !(totals[k] > 0) }); });
        return c.concat([
            { key: 'GainLossValue', caption: 'Gain Loss Value', num: true, sum: true, fmt: function (v) { var n = H.netD(v); return n < 0 ? '(' + H.fmtFixed(-n, 0) + ')' : H.fmtFixed(n, 0); } },
            { key: 'RealizedLcyAmount', caption: 'Realized Lcy Amount', num: true, sum: true, fmt: 'o0' }, { key: 'PLValue', caption: 'Profit Loss Amount', num: true, sum: true, fmt: 'p0', cls: pl },
            { key: 'PlPercent', caption: 'Profit Loss(%)', num: true, fmt: 'p3' }, { key: 'ShipmentWeight', hidden: true }]);
    }

    function bindCombos(d) {
        var c = (d && d.combos) || {};
        if (d && d.combosError) box(d.combosError);
        H.bind('CmbSupplierCustomer', c.Customer || []); H.bind('CmbDestinationPort', c.DestinationPort || []); H.bind('cmbInvoiceNo', c.Invoice || []);
    }
    /** ExImShipmentCosting_Load: ComboFill, DateFrom = FY start, focus Customer. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {}; S.yearStart = H.isoDate(d.yearStart);
            H.setText('DateFrom', S.yearStart || H.today()); H.setText('DateTo', H.today());
            bindCombos(d);
            render();
            H.focus('CmbSupplierCustomer');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function filters() { return { fromDate: H.val('DateFrom'), toDate: H.val('DateTo'), supplierCustomerId: H.netI(H.val('CmbSupplierCustomer')), destinationPortId: H.netI(H.val('CmbDestinationPort')), invoiceId: H.netI(H.val('cmbInvoiceNo')) }; }
    function render() {
        var totals = {};
        CHARGES.forEach(function (k) { totals[k] = 0; S.rows.forEach(function (r) { totals[k] += H.netD(r[k]); }); });
        H.drawGrid('grdfrm', cols(totals), S.rows, { cur: S.cur });
        H.show('grdEmpty', S.rows.length === 0);
    }
    function show(btn) {
        return H.busy(btn, function () { return H.postJson(API + '/show', filters()).then(function (rows) { S.rows = rows || []; S.cur = -1; render(); }).catch(function (e) { box(e.message); }); });
    }
    /** Reset(): Invoice No emptied and focused (nothing else). */
    function reset() { H.setVal('cmbInvoiceNo', '0'); H.focus('cmbInvoiceNo'); }
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/setup').then(bindCombos).catch(function (e) { box(e.message); }); }); }
    /** btnRegister_Click: "Record Not Found For Display" - 542_01-ShipmentCostingSummary.rpt on dtGrid. */
    function print542(btn) {
        if (!S.rows.length) { box('Record Not Found For Display'); return; }
        var f = filters();
        return H.print('exp-542-01', { fromDate: f.fromDate, toDate: f.toDate, exImInvoiceId: f.invoiceId, supplierCustomerId: f.supplierCustomerId, destinationPortId: f.destinationPortId }, btn);
    }
    /** grdfrm_LinkClicked (any column on the desktop; InvoiceNo is the link column): ExImShipmentCosting(InvoiceId). */
    function link(key, i) { var r = S.rows[i]; if (!r) return; global.open(LINKS.shipmentCosting + '?invoiceId=' + H.netI(r.InvoiceId), '_blank'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        H.gridEvents('grdfrm', { select: function (i) { S.cur = i; }, link: link, ctrlSpace: function (i) { link('InvoiceNo', i); } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, null)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnShow')); }
        });
        load();
    });

    global.ExportCss = { show: show, reset: reset, refresh: refresh, print542: print542 };
}(window));
