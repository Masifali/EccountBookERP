/* ============================================================================================
 * countx_export_shipment_costing.js - ExImShipmentCosting.cs, screen 238 "Shipment Costing Register".
 * Data: /api/export/shipment-costing (SpExport_ShipmentCosting_Report). Print 542-ShipmentCostingRegister.rpt
 * through the seeded CrystalPrint key "542-shipmentcostingregister" (arg exImInvoiceId).
 * ?invoiceId=<id> on the URL plays the desktop's public InvoiceId field (Load -> select + Show).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, $id = H.$id, box = H.box;
    var API = '/api/export/shipment-costing';
    var S = { rows: [], decA: 0, decF: 0 };
    function fA(v) { return H.fmtFixed(v, S.decA); }      /* clsGlobalVariables.stringFormatsingle       "#,##0." + amount decimals */
    function fF(v) { return H.fmtFixed(v, S.decF); }      /* clsGlobalVariables.stringFormatsingleForFcy "#,##0." + fcy decimals    */
    var COLS = [
        { key: 'SortNo', hidden: true }, { key: 'ExImInvoiceId', hidden: true }, { key: 'SubSortNo', hidden: true },
        { key: 'MasterType', caption: 'Master Type' }, { key: 'VDate', caption: 'V Date', fmt: 'd' }, { key: 'VType', caption: 'V Type' },
        { key: 'VNo', caption: 'V No', fmt: 'i' }, { key: 'InvoiceNo', caption: 'Invoice No' }, { key: 'ItemName', caption: 'Item Name' },
        { key: 'JobLot', caption: 'Job Lot' }, { key: 'TranRemarks', caption: 'Tran Remarks' },
        { key: 'ItemRate', caption: 'Item Rate', num: true, fmt: 'h4' }, { key: 'Qty', caption: 'Qty', num: true, fmt: 'o2' },
        { key: 'NetWeight', caption: 'Net Weight', num: true, fmt: 'n3' }, { key: 'FcyCode', caption: 'Fcy Code' },
        { key: 'ExchangeRate', caption: 'Exchange Rate', num: true, fmt: 'n4' }, { key: 'FcyAmount', caption: 'Fcy Amount', num: true, fmt: fF },
        { key: 'TranAmount', caption: 'Tran Amount', num: true, fmt: 'n3' }, { key: 'TotalTranAmount', hidden: true },
        { key: 'ClearForwardValueTotal', hidden: true }, { key: 'PackingMaterialValueTotal', hidden: true }, { key: 'SalesValueTotal', hidden: true },
        { key: 'GainLossValueTotal', hidden: true }, { key: 'ShipmentWeight', hidden: true }
    ];
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            S.decA = H.netI(d.decimalsAmount); S.decF = H.netI(d.decimalsFcy);
            if (d.invoicesError) box(d.invoicesError);
            H.bind('cmbInvoiceNo', d.invoices, 'Id', 'name', false);
            var m = /[?&]invoiceId=(\d+)/.exec(location.search);
            if (m && H.netI(m[1]) > 0) { H.setVal('cmbInvoiceNo', m[1]); if (H.netI(H.val('cmbInvoiceNo')) > 0) gridBind(); }
            H.focus('cmbInvoiceNo');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function clearSummary() {
        ['txtSaleValue', 'txtStockValue', 'txtPackingMaterial', 'txtLogisticgsValue', 'txtGainLoss', 'txtCommissionValue'].forEach(function (id) { H.setText(id, ''); });
        $id('txtPlValue').textContent = ''; $id('txtPlValue').className = 'rpta-pl';
    }
    /** GridBind(): "Please select Invoice No first!" without an invoice, summary boxes, grid, ShimpmentPLCalculate. */
    function gridBind() {
        var id = H.netI(H.val('cmbInvoiceNo'));
        if (id === 0) { H.focus('cmbInvoiceNo'); box('Please select Invoice No first!'); return Promise.resolve(); }
        return H.getJson(API + '/show?invoiceId=' + id).then(function (d) {
            d = d || {};
            S.rows = d.rows || [];
            var s = d.summary || {};
            if (!S.rows.length) { clearSummary(); H.clearGrid('grdfrm'); H.show('grdfrmEmpty', true); return; }
            /* the boxes are only overwritten when a total > 0 arrives; otherwise the previous text stays (desktop loop) */
            if (s.StockValue !== undefined) H.setText('txtStockValue', fA(s.StockValue));
            if (s.SaleValue !== undefined) H.setText('txtSaleValue', fF(s.SaleValue));
            if (s.LogisticsValue !== undefined) H.setText('txtLogisticgsValue', fA(s.LogisticsValue));
            if (s.PackingMaterial !== undefined) H.setText('txtPackingMaterial', fA(s.PackingMaterial));
            if (s.GainLoss !== undefined) H.setText('txtGainLoss', fA(s.GainLoss));
            if (s.CommissionValue !== undefined) H.setText('txtCommissionValue', fA(s.CommissionValue));
            H.drawGrid('grdfrm', COLS, S.rows, {});
            H.show('grdfrmEmpty', false);
            plCalculate();
        }).catch(function (e) { box(e.message); });
    }
    /** ShimpmentPLCalculate(): PL = Sale - Stock - Packing - Logistics + GainLoss - Commission, "#,#", green / red / black. */
    function plCalculate() {
        var g = function (id) { var v = H.netD(H.val(id)); return v > 0 ? v : 0; };
        var pl = g('txtSaleValue') - g('txtStockValue') - g('txtPackingMaterial') - g('txtLogisticgsValue') + g('txtGainLoss') - g('txtCommissionValue');
        var e = $id('txtPlValue');
        e.textContent = H.fmtHash(pl, 0);
        e.className = 'rpta-pl ' + (pl > 0 ? 'pos' : pl < 0 ? 'neg' : '');
    }
    function show(btn) { return H.busy(btn, gridBind); }
    /** btnnew_Click -> Reset(): invoice blank, focus. */
    function btnNew() { H.setVal('cmbInvoiceNo', ''); $id('cmbInvoiceNo').selectedIndex = -1; H.refreshCombos(); H.focus('cmbInvoiceNo'); }
    /** btnRegister_Click: "Record Not Found For Display" without rows, else 542 with the invoice. */
    function btnRegister(btn) {
        if (!S.rows.length) { box('Record Not Found For Display'); return; }
        return H.print('542-shipmentcostingregister', { exImInvoiceId: H.netI(H.val('cmbInvoiceNo')) }, btn);
    }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, null)) return;
            if (e.ctrlKey && (e.key || '').toLowerCase() === 'n') { e.preventDefault(); btnNew(); }
        });
        load();
    });
    global.ExportCst = { btnNew: btnNew, show: show, btnRegister: btnRegister };
}(window));
