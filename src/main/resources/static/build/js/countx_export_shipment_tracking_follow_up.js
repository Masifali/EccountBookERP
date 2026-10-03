/* ============================================================================================
 * countx_export_shipment_tracking_follow_up.js - ExportPendingShipmentsFollowupReport.cs, screen 254
 * "5014 Shipment Tracking Follow up". Data: /api/export/shipment-tracking-follow-up
 * (ExportReportsBController -> ExportReportsBService -> ExportPendingShipmentsFollowup and the combo procedures).
 * Prints go through CrystalPrint (ReportRegistry keys exp-552 / exp-521 / exp-560).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptB, API = '/api/export/shipment-tracking-follow-up';
    var $id = H.$id, box = H.box, esc = H.esc;
    /* Routes of the forms the desktop opens from here (BtnShipmentForm / btnDHLTracking / LoadingStart / LoadingEnd links);
       the coordinator maps them to the real screens. */
    var LINKS = { shipmentFollowUp: '/export/shipment-follow-up', dhlTracking: '/export/dhl-tracking',
        forwardingHistory: '/export/forwarding-report', deliveryOrderHistory: '/export/delivery-order-report' };

    var S = { rows: [], eta: [], cur: -1, yearStart: '', docStatuses: [], curStatuses: [] };

    /* Gridfill's dt columns (InvoiceDocumentTypeId..NoOfAttachments) with grdsetting's captions, visibility, formats:
       hidden InvoiceDocumentTypeId, InvoiceId, ContractId, FcyId, InvoiceStatus, RealizedAmount, InvoiceValue, BalAmount,
       ShippingLine, BLNumber, FcyCode; links InvoiceNo, CroBooking, LoadingStart, LoadingEnd, NoOfAttachments;
       dates dd-MMM-yy; ETDFinalDate / ETADestination bold dark-orange when present (GridEXFormatCondition NotEqual ""). */
    var orange = function (v) { return H.isoDate(v) ? 'rptb-orange' : ''; };
    var COLS = [
        { key: 'InvoiceDocumentTypeId', hidden: true }, { key: 'InvoiceId', hidden: true }, { key: 'ContractId', hidden: true },
        { key: 'Customer', caption: 'Customer' }, { key: 'LcOrderNo', caption: 'Contract' }, { key: 'InvoiceNo', caption: 'Invoice', link: true },
        { key: 'InvoiceDate', caption: 'Invoice Date', fmt: 'dmy' }, { key: 'FclQty', caption: 'FCL', num: true, sum: true, fmt: 'o2' },
        { key: 'Mtons', caption: 'M.Tons', num: true, sum: true, fmt: 'n2' }, { key: 'PortName', caption: 'Destination Port' },
        { key: 'InspectionDate', caption: 'Inspection Date', fmt: 'dmy' }, { key: 'CroBooking', caption: 'CRO Booked', fmt: 'dmy', link: true },
        { key: 'ETADate', caption: 'Loading Port E.T.A', fmt: 'dmy' }, { key: 'ETDDate', caption: 'Loading Port E.T.D', fmt: 'dmy' },
        { key: 'CutOffDate', caption: 'Cut Off Date', fmt: 'dmy' }, { key: 'LoadingStart', caption: 'Dispatch Started', fmt: 'dmy', link: true },
        { key: 'LoadingEnd', caption: 'Dispatch Completed', fmt: 'dmy', link: true }, { key: 'LoadOnTrainDate', caption: 'Load On Train', fmt: 'dmy' },
        { key: 'ReachedAtLoadingPortDate', caption: 'Reached At Port', fmt: 'dmy' }, { key: 'LoadedOnVesselDate', caption: 'Loaded On Vessel', fmt: 'dmy' },
        { key: 'BLDate', caption: 'BL Date', fmt: 'dmy' }, { key: 'DocumentStatus', caption: 'Document Status' },
        { key: 'DocToBank', caption: 'Doc To Bank Date', fmt: 'dmy' }, { key: 'DocToParty', caption: 'Doc To Party Date', fmt: 'dmy' },
        { key: 'CurrentStatus', caption: 'Current Status' }, { key: 'ETADestination', caption: 'ETA Destination', fmt: 'dmy', cls: orange },
        { key: 'ExpectedInTransitDays', caption: 'Expected In Transit Days', num: true, fmt: 'i' }, { key: 'InTransitDays', caption: 'In Transit Days', num: true, fmt: 'i' },
        { key: 'ETDFinalDate', caption: 'Reached At Destination Port', fmt: 'dmy', cls: orange },
        { key: 'FcyId', hidden: true }, { key: 'FcyCode', hidden: true }, { key: 'InvoiceValue', hidden: true }, { key: 'InvoiceStatus', hidden: true },
        { key: 'RealizedAmount', hidden: true }, { key: 'BalAmount', hidden: true }, { key: 'ShippingLine', hidden: true }, { key: 'BLNumber', hidden: true },
        { key: 'NoOfAttachments', caption: 'No Of Attachments', num: true, fmt: 'i', link: true }
    ];
    var ETA_COLS = [
        { key: 'SortNo', hidden: true }, { key: 'ETA-DestinationPort', caption: 'ETA-DestinationPort', width: 200 },
        { key: 'M.Tons', caption: 'M.Tons', num: true, sum: true, fmt: 'o3' }, { key: 'FCL', caption: 'FCL', num: true, sum: true, fmt: 'o3' }
    ];

    function bindCombos(d) {
        var c = d.combos || {};
        H.bind('cmbsupplierCustomer', c.customers || []);
        H.bind('cmbdestinationport', c.ports || []);
        S.docStatuses = d.documentStatuses || []; S.curStatuses = d.currentStatuses || [];
        H.checkList('cmbStatus', S.docStatuses, 'Document Status');
        H.checkList('CmbCurrentStatus', S.curStatuses, 'Current Status');
        ['documentStatuses', 'currentStatuses', 'combos'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
    }
    /** ExportPendingShipmentsFollowupReport_Load: AllcomboBind, Statusfill, CurrentStatusfill (no grid until Show). */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {}; S.yearStart = H.isoDate(d.yearStart);
            bindCombos(d);
            render();
            H.focus('cmbsupplierCustomer');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function filters(sortNo) {
        return { supplierCustomerId: H.netI(H.val('cmbsupplierCustomer')), destinationPortId: H.netI(H.val('cmbdestinationport')),
            documentStatusIds: H.checkListIds('cmbStatus'), currentStatusIds: H.checkListIds('CmbCurrentStatus'),
            activityId: H.netI(H.radio('realized')), statusId: H.netI(H.radio('shipments')), sortNo: sortNo || 0 };
    }
    function render() {
        H.drawGrid('grd', COLS, S.rows, { cur: S.cur });
        H.show('grdEmpty', S.rows.length === 0);
        H.drawGrid('grdEtaDestination', ETA_COLS, S.eta, {});
    }
    /** Gridfill(SortNo) - btnshow_Click, grdEtaDestination_Click. */
    function gridFill(sortNo) {
        return H.postJson(API + '/show', filters(sortNo)).then(function (d) {
            S.rows = (d && d.rows) || []; S.eta = (d && d.eta) || []; S.cur = -1;
            render();
        });
    }
    function show(btn) { return H.busy(btn, function () { return gridFill(0).catch(function (e) { box(e.message); }); }); }
    /** Reset(): the three combos emptied, focus on Document Status. */
    function reset() {
        H.checkListClear('cmbStatus'); H.setVal('cmbdestinationport', '0'); H.setVal('cmbsupplierCustomer', '0');
        var t = $id('cmbStatus') && $id('cmbStatus').querySelector('.rptb-check-text'); if (t) t.focus();
    }
    /** btnRefresh_Click: Statusfill, CurrentStatusfill, AllcomboBind (selections kept when still present). */
    function refresh(btn) {
        return H.busy(btn, function () { return H.getJson(API + '/refresh').then(bindCombos).catch(function (e) { box(e.message); }); });
    }
    /** btnPrintCurrent_Click: 552-ExportPendingShippmentFollowUp.rpt on the loaded dt1 - "No Record Found For Display" when empty. */
    function print552(btn) {
        if (!S.rows.length) { box('No Record Found For Display'); return; }
        var f = filters(0);
        return H.print('exp-552', { supplierCustomerId: f.supplierCustomerId, destinationPortId: f.destinationPortId,
            documentStatusIds: f.documentStatusIds ? ',' + f.documentStatusIds.split(',').join(',') : '', currentStatusIds: f.currentStatusIds,
            activityId: f.activityId, statusId: f.statusId, sortNo: 0 }, btn);
    }
    /** grd_LinkClicked. */
    function link(key, i) {
        var r = S.rows[i]; if (!r) return;
        var invoiceId = H.netI(r.InvoiceId), docTypeId = H.netI(r.InvoiceDocumentTypeId);
        if (key === 'InvoiceNo') { if (!invoiceId) { box('No Record Found For Display'); return; } H.print('exp-521', { eximInvoiceId: invoiceId }); }
        else if (key === 'CroBooking') {
            H.getJson(API + '/cro-exists?invoiceId=' + invoiceId).then(function (d) {
                if (!d || !d.found) box('No Record Found For Display'); else H.print('exp-560', { exImInvoiceId: invoiceId });
            }).catch(function (e) { box(e.message); });
        }
        else if (key === 'LoadingEnd') {
            if (!H.isoDate(r.LoadingEnd)) return;   /* only when the cell has a value */
            global.open(LINKS.forwardingHistory + '?invoiceId=' + invoiceId + '&fromDate=' + (S.yearStart || H.today()) + '&toDate=' + H.today() + '&auto=1', '_blank');
        }
        else if (key === 'LoadingStart') {
            global.open(LINKS.deliveryOrderHistory + '?doType=Export&invoiceId=' + invoiceId + '&fromDate=' + (S.yearStart || H.today()) + '&toDate=' + H.today(), '_blank');
        }
        else if (key === 'NoOfAttachments') H.attachments(invoiceId, docTypeId);
    }
    /** BtnShipmentForm_Click - frmshippedConsignment_followup (its own View right is checked by that page). */
    function shipmentForm() { global.open(LINKS.shipmentFollowUp, '_blank'); }
    /** btnDHLTracking_Click - frmDhlTracking. */
    function dhlTracking() { global.open(LINKS.dhlTracking, '_blank'); }
    function shortcuts() {
        H.shortcuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+F5', 'For Focus on Supplier Customer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+ArrowUp', 'For Focus On Supplier Customer in Filter']]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        H.gridEvents('grd', { select: function (i) { S.cur = i; }, link: link });
        H.gridEvents('grdEtaDestination', { select: function (i) { var r = S.eta[i]; if (r) H.busy($id('btnshow'), function () { return gridFill(H.netI(r.SortNo)).catch(function (e) { box(e.message); }); }); } });
        /* ExportPendingShipmentsFollowupReport_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnshow')); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); print552($id('btnPrintCurrent')); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grd'); }
            else if (e.ctrlKey && (e.key === 'ArrowUp' || e.key === 'F5')) { e.preventDefault(); H.focus('cmbsupplierCustomer'); }
        });
        load();
    });

    global.ExportTrk = { show: show, reset: reset, refresh: refresh, print552: print552, shortcuts: shortcuts, shipmentForm: shipmentForm, dhlTracking: dhlTracking };
    void esc;
}(window));
