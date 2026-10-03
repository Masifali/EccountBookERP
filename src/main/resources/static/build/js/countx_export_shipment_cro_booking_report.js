/* ============================================================================================
 * countx_export_shipment_cro_booking_report.js - frmExportShipingLineBookingRpt.cs, screen 266 "5010 Shipment CRO Booking Report".
 * Data: /api/export/shipment-cro-booking-report (Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt distinct by Id,
 * USP_GetDataForDropDownFromShippingBookingInfo). Prints: exp-509 (register on the shown filters), exp-560 (CRO slip by booking Id).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptB, API = '/api/export/shipment-cro-booking-report';
    var $id = H.$id, box = H.box;
    var S = { rows: [], cur: -1 };

    /* GridBind's dt + GridSetting (dates dd-MMM-yy, centred CarierMedium/BookingRate/NoofContainers/FreeDaysAtDestination/TransitDays; no totals). */
    var COLS = [
        { key: 'Id', hidden: true }, { key: 'InvoiceNo', caption: 'Invoice No' }, { key: 'BookingDate', caption: 'Booking Date', fmt: 'dmy' },
        { key: 'BookingCroNo', caption: 'Booking Cro No', link: true }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'ClearingAgentName', caption: 'Clearing Agent Name' },
        { key: 'TransporterName', caption: 'Transporter Name' }, { key: 'CarierMedium', caption: 'Carier Medium', ctr: true }, { key: 'VesselName', caption: 'Vessel Name' },
        { key: 'VoyageNo', caption: 'Voyage No' }, { key: 'NoofContainers', caption: 'Noof Containers', ctr: true, fmt: 'i' }, { key: 'ETADate', caption: 'ETA Date', fmt: 'dmy' },
        { key: 'ETDDate', caption: 'ETD Date', fmt: 'dmy' }, { key: 'CuttOFFDate', caption: 'Cutt OFF Date', fmt: 'dmy' }, { key: 'TransitDays', caption: 'Transit Days', ctr: true, fmt: 'i' },
        { key: 'DestinationPortName', caption: 'Destination Port Name' }, { key: 'FreeDaysAtDestination', caption: 'Free Days At Destination', ctr: true },
        { key: 'BookingRate', caption: 'Booking Rate', ctr: true, fmt: 'o2' }
    ];

    function bindCombos(d) {
        var c = (d && d.combos) || {};
        if (d && d.combosError) box(d.combosError);
        H.bind('cmbSupplierName', c.customers || []); H.bind('cmbExportInvoice', c.invoices || []); H.bind('cmbDestinationPort', c.ports || []);
    }
    function filters() { return { invoiceId: H.netI(H.val('cmbExportInvoice')), supplierCustomerId: H.netI(H.val('cmbSupplierName')), destinationPortId: H.netI(H.val('cmbDestinationPort')) }; }
    function render() { H.drawGrid('grdfrm', COLS, S.rows, { cur: S.cur }); H.show('grdEmpty', S.rows.length === 0); }
    function gridBind() { return H.postJson(API + '/show', filters()).then(function (rows) { S.rows = rows || []; S.cur = -1; render(); }); }
    /** frmExportShipingLineBookingRpt_Load: GridBind (no filters) then AllComboBind. */
    function load() {
        return gridBind().catch(function (e) { box(e.message); }).then(function () { return H.getJson(API + '/setup').then(bindCombos); }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function show(btn) { return H.busy(btn, function () { return gridBind().catch(function (e) { box(e.message); }); }); }
    /** Reset(): the three combos emptied, focus Customer Name, GridBind. */
    function reset(btn) { H.setVal('cmbDestinationPort', '0'); H.setVal('cmbExportInvoice', '0'); H.setVal('cmbSupplierName', '0'); H.focus('cmbSupplierName'); return show(btn); }
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/setup').then(bindCombos).catch(function (e) { box(e.message); }); }); }
    /** ShowRegister(): 509-ExImExportShipingLineBooking_Register.rpt on the shown rows. */
    function print509(btn) {
        if (!S.rows.length) { box('No Record Found For Display'); return; }
        var f = filters();
        return H.print('exp-509', { exImInvoiceId: f.invoiceId, supplierCustomerId: f.supplierCustomerId, destinationPortId: f.destinationPortId }, btn);
    }
    /** grdfrm_LinkClicked "BookingCroNo": PrintSlipandRegister(Id) -> 560-ExBooking Info(CRO).rpt. */
    function link(key, i) {
        var r = S.rows[i]; if (!r || key !== 'BookingCroNo') return;
        var id = H.netI(r.Id);
        H.getJson(API + '/cro-exists?id=' + id).then(function (d) {
            if (!d || !d.found) box('No Record Found For Display'); else H.print('exp-560', { id: id });
        }).catch(function (e) { box(e.message); });
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Customer'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Enter', 'For Showing Data'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        H.gridEvents('grdfrm', { select: function (i) { S.cur = i; }, link: link, ctrlSpace: function (i) { link('BookingCroNo', i); } });
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.key === 'Enter') { e.preventDefault(); show($id('btnShow')); return; }
            if (H.baseKeys(e, shortcuts)) return;
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset($id('btnnew')); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); print509($id('btnPrintRegister')); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); H.focus('cmbSupplierName'); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdfrm'); }
        });
        load();
    });

    global.ExportCro = { show: show, reset: reset, refresh: refresh, print509: print509, shortcuts: shortcuts };
}(window));
