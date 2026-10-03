/* ============================================================================================
 * countx_export_bill_of_lading_report.js - frmBillofLadingSlipandRegister.cs, screen 264 "Bill of Lading Report".
 * Data: /api/export/bill-of-lading-report (USP_GetDataForDropDownFromBillOfLading, Sp_ExImBillOfLading_SlipAndRegister_Rpt).
 * Print 506 through CrystalPrint key "506-eximbilloflading-register".
 * Quirk kept: the "Customer Name" combo lists Bill Of Lading Ids (the procedure's d.Id) and that value is sent as
 * @SupplierCustomerId, exactly as the desktop does.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, N = global.ExportRptN, $id = H.$id, box = H.box;
    var API = '/api/export/bill-of-lading-report';
    var S = { rows: [], dec: 0, last: null };

    function both(v) { return N.fmtBoth(v, S.dec); }
    function cols() { /* GridBind dt + GridSetting */
        return [
            { key: 'Id', hidden: true }, { key: 'InvoiceNo', caption: 'InvoiceNo' }, { key: 'DocDate', caption: 'DocDate', fmt: 'dmy' },
            { key: 'EFormNo', caption: 'EFormNo' }, { key: 'EFormDate', caption: 'EFormDate', fmt: 'dmy' },
            { key: 'EFormValueTotal', caption: 'EFormValueTotal', num: true, sum: true, fmt: both },
            { key: 'CustomerBuyerName', caption: 'CustomerBuyerName' }, { key: 'Address1', caption: 'Address1', width: 200 },
            { key: 'notifyPartyName', caption: 'notifyPartyName' }, { key: 'ToTheOrderOfName', caption: 'ToTheOrderOfName' },
            { key: 'BLNumber', caption: 'BLNumber' }, { key: 'BLDate', caption: 'BLDate', fmt: 'dmy' }, { key: 'VesselNo', caption: 'VesselNo' },
            { key: 'VoyageNo', caption: 'VoyageNo' }, { key: 'CarierType', caption: 'CarierType' }, { key: 'BookingCroNo', caption: 'BookingCroNo' },
            { key: 'BookingDate', caption: 'BookingDate', fmt: 'dmy' }, { key: 'FreightType', caption: 'FreightType' },
            { key: 'ShipingCertNo', caption: 'ShipingCertNo', num: true }, { key: 'ShipingCertDate', caption: 'ShipingCertDate', fmt: 'dmy' },
            { key: 'ItemsGoodsDesc', caption: 'ItemsGoodsDesc' }, { key: 'ShippingLineName', caption: 'ShippingLineName' },
            { key: 'ClearingAgentName', caption: 'ClearingAgentName' }, { key: 'TransporterName', caption: 'TransporterName' },
            { key: 'Remarks', caption: 'Remarks' }
        ];
    }
    /** AllComboBind: nothing changes when the procedure returns no row; otherwise each list is bound or emptied (DataSource = null). */
    function allComboBind() {
        return H.getJson(API + '/combos').then(applyCombos).catch(function (e) { box(e.message); });
    }
    function applyCombos(d) {
        d = d || {};
        if (d.combosError) { box(d.combosError); return; }
        var c = d.customers || [], i = d.invoices || [], l = d.shippingLines || [];
        if (!c.length && !i.length && !l.length) return;
        H.bind('BuyerName', c, 'Id', 'name');
        H.bind('InvoiceNo', i, 'Id', 'name');
        H.bind('cmbShippingLine', l, 'Id', 'name');
    }
    function filters() { return { invoiceId: N.intVal('InvoiceNo'), supplierCustomerId: N.intVal('BuyerName'), shippingLineId: N.intVal('cmbShippingLine') }; }
    /** GridBind: dtHistyory cleared, re-read; rows -> grid, none -> DataSource = null. */
    function gridBind() {
        var f = filters();
        S.rows = [];
        return H.postJson(API + '/show', f).then(function (d) {
            S.rows = (d && d.rows) || [];
            S.last = f;
            if (S.rows.length) H.drawGrid('grdHistory', cols(), S.rows, {});
            else H.clearGrid('grdHistory');
            H.show('grdHistoryEmpty', !S.rows.length);
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn || 'btnShow', gridBind); }
    /** btnnew_Click -> Reset: the three combos cleared, focus Invoice No, GridBind. */
    function btnNew(btn) {
        return H.busy(btn || 'btnnew', function () {
            H.setVal('BuyerName', '0'); H.setVal('InvoiceNo', '0'); H.setVal('cmbShippingLine', '0'); H.focus('InvoiceNo');
            return gridBind();
        });
    }
    function refresh(btn) { return H.busy(btn || 'btnRefresh', allComboBind); }
    /** ShowRegister: "No Record Found For Display" while dtHistyory is empty; 506 re-runs the last GridBind's filters. */
    function printRegister(btn) {
        if (!S.rows.length || !S.last) { box('No Record Found For Display'); return; }
        return H.print('506-eximbilloflading-register', N.args({ eximInvoiceId: S.last.invoiceId, supplierCustomerId: S.last.supplierCustomerId,
            shippingLineId: S.last.shippingLineId }), btn || $id('btnPrintRegister'));
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Invoice No'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Enter', 'For Showing Data'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    document.addEventListener('DOMContentLoaded', function () {
        N.embedFooter('bolFooter');
        H.fullscreenButtons();
        H.gridEvents('grdHistory', {});
        document.addEventListener('keydown', function (e) {
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); show(); return; }
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'p') { e.preventDefault(); printRegister(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); H.focus('InvoiceNo'); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdHistory'); }
        });
        /* Load: AllComboBind, GridBind. */
        H.getJson(API + '/setup').then(function (d) { S.dec = H.netI(d && d.decimalsAmount); applyCombos(d); })
            .catch(function (e) { box(e.message); })
            .then(function () { return show($id('btnShow')); });
    });
    global.ExportBol = { btnNew: btnNew, refresh: refresh, show: show, printRegister: printRegister, shortcuts: shortcuts };
}(window));
