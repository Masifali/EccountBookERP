/* ============================================================================================
 * countx_export_shipping_booking_info.js - EximShippingBookingInfo.cs (Architecture.WinApp.Export), screen 214
 * "Export Shipping Booking Info". Form | History; every button, cmbExportInvoice_Leave, txtContrsQty_TextChanged,
 * ETA / Transit Days / Final ETA cross-calculation, the containers grid (Add button, duplicate Container# check,
 * container type combo), history filters / Show / New / Refresh, grid Edit / Print / double-click / attachments
 * link and the KeyDown shortcuts of the desktop form have their counterpart here; data from
 * /api/export/shipping-booking-info. Print -> CommonServices.ExBookingInfoSlip560 -> seeded 560-exbooking-info-cro (id).
 * ============================================================================================ */
(function (global) {
    'use strict';

    var F = global.ExportF, $id = F.$id, box = F.box, ask = F.ask, str = F.str, netI = F.netI, netD = F.netD, val = F.val, setText = F.setText,
        setVal = F.setVal, bind = F.bind, busy = F.busy, getJson = F.getJson, postJson = F.postJson, show = F.show, focus = F.focus, esc = F.esc;
    var API = '/api/export/shipping-booking-info';

    var PERM = { Save: true, Update: true, Print: true };
    var INVOICES = [], SUPPLIERS = [], MEDIUMS = [], PORTS = [], CTYPES = [], COMPANY = '';
    var S = { recId: 0, days: 0, attachmentsValues: '', customAttachmentsValues: '', updating: false };
    var DET = { rows: [] };
    var HIST = { rows: [], cur: -1 };

    function bindAll() {
        bind('cmbExportInvoice', INVOICES, 'Id', 'Name', [], true);
        ['cmbshippingline', 'cmbForwarder', 'cmbClearingAgent'].forEach(function (id) { bind(id, SUPPLIERS, 'Id', 'CompanyName', ['PartyCode', 'CityName', 'MobileNo']); });
        bind('cmbCarierMedium', MEDIUMS, 'Id', 'Name', [], true);
        ['cmbDestinationPort', 'CmbContainerDispatchedAtPort', 'CmbContainerReceivedFromPort'].forEach(function (id) { bind(id, PORTS, 'Id', 'Name', []); });
        bind('CmbDELIVEREDAT', [{ Id: 1, Name: COMPANY }, { Id: 2, Name: 'Other' }], 'Id', 'Name', []);
    }
    function applyData(d) {
        ['invoices', 'suppliers', 'carrierMediums', 'ports', 'containerTypes', 'companyName'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
        INVOICES = d.invoices || []; SUPPLIERS = d.suppliers || []; MEDIUMS = d.carrierMediums || []; PORTS = d.ports || []; CTYPES = d.containerTypes || [];
        if (typeof d.companyName === 'string') COMPANY = d.companyName;
        S.days = netI(d.defaultDaysToLessFromHistoryFromDate);
        bindAll();
    }
    function bindHistoryCombos(h) {
        h = h || {};
        bind('CmbCustomerHistory', h.Customer || [], 'Id', 'Name', [], true);
        bind('CmbInvoiceNoHistory', h.InvoiceNo || [], 'Id', 'Name', [], true);
        bind('CmbDestinationPortHistory', h.DestinationPort || [], 'Id', 'Name', [], true);
        bind('CmbShippingLinHistory', h.ShippingLine || [], 'Id', 'Name', [], true);
        bind('CmbClearingAgentHistory', h.ShippingAgent || [], 'Id', 'Name', [], true);
        bind('CmbForwarderHistory', h.Forwarder || [], 'Id', 'Name', [], true);
        bind('CmbBookingNoHistory', h.BookingCroNo || [], 'Id', 'Name', [], true);
    }

    /** InitializeComponentMethod. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false, Print: d.permissions.Print !== false };
            $id('btnsave').disabled = !PERM.Save; $id('btnupdate').disabled = !PERM.Update; $id('btnPrint').disabled = !PERM.Print;
            applyData(d);
            bindHistoryCombos(d.historyCombos);
            setText('FromDateHistory', F.daysAgo(S.days > 0 ? S.days : 3)); setText('ToDateHistory', F.today());
            ['txtBookingDate', 'txtETA', 'txtETD', 'txtcutoffdate', 'datFinalEtaDate'].forEach(function (id) { setText(id, F.today()); });
            DET.rows = []; bindGrids();
            show('btnsave', true); show('btnupdate', false);
            $id('bkFooterInfo').textContent = 'EximShippingBookingInfo';
            focus('txtBookingDate');
        }).catch(function () { box('Error occurred during database call.'); });
    }
    /** btnRefresh_Click. */
    function refresh(btn) { return busy(btn, function () { return getJson(API + '/refresh').then(applyData).catch(function (e) { box(e.message); }); }); }

    // ------------------------------------------------------------------------------ containers grid

    function ctypeOptions(v) {
        var h = '<option value="0"></option>';
        CTYPES.forEach(function (r) { h += '<option value="' + esc(r.Id) + '"' + (netI(r.Id) === netI(v) ? ' selected' : '') + '>' + esc(r.Name) + '</option>'; });
        return h;
    }
    var DCOLS = [{ key: 'ContainerNo', edit: 'text' }, { key: 'SealNo', edit: 'text' }, { key: 'ContainerTypeId', edit: 'select', options: ctypeOptions }];
    /** BindGrids: one empty row when the table is empty. */
    function bindGrids() {
        if (DET.rows.length === 0) addRowInGrid(false);
        F.drawGrid('contBody', null, DET.rows, DCOLS, -1, function (r, i) {
            return '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Add" data-i="' + i + '">Add</button></td>';
        });
    }
    function addRowInGrid(redraw) {
        DET.rows.push({ Id: 0, ContainerNo: '', SealNo: '', ContainerTypeId: 0 });
        if (redraw !== false) bindGrids();
    }
    /** grdContainers "Add" button / Ctrl+Space / Ctrl+Enter on the grid. */
    function addClick() {
        if (netI(val('cmbExportInvoice')) <= 0) { box('Please Select Invoice First'); focus('cmbExportInvoice'); return; }
        if (DET.rows.length === netD(val('txtContrsQty'))) { box("You Can't Add More Rows"); return; }
        addRowInGrid();
    }
    /** grdContainers_CellUpdated: a Container# already in another row is cleared with a warning. */
    function cellUpdated(i, k, v) {
        var r = DET.rows[i]; if (!r) return;
        if (k === 'ContainerNo') {
            var cur = str(v).trim();
            if (cur) {
                for (var j = 0; j < DET.rows.length; j++) {
                    if (j !== i && str(DET.rows[j].ContainerNo).trim().toLowerCase() === cur.toLowerCase()) {
                        r.ContainerNo = '';
                        bindGrids();
                        box('Container# ' + str(DET.rows[j].ContainerNo).trim() + ' already exists.');
                        return;
                    }
                }
            }
            r.ContainerNo = v;
        } else if (k === 'ContainerTypeId') { r.ContainerTypeId = netI(v); }
        else r[k] = v;
    }
    /** txtContrsQty_TextChanged: rows beyond the quantity are dropped. */
    function contrsQtyChanged() {
        var n = netI(val('txtContrsQty'));
        if (DET.rows.length > 0 && n < DET.rows.length) { DET.rows = DET.rows.slice(0, n); if (DET.rows.length) F.drawGrid('contBody', null, DET.rows, DCOLS, -1, function (r, i) { return '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Add" data-i="' + i + '">Add</button></td>'; }); else bindGrids(); }
    }

    // ------------------------------------------------------------------------------ form

    /** cmbExportInvoice_Leave. */
    function invoiceLeave() {
        return getJson(API + '/invoice-leave?invoiceId=' + netI(val('cmbExportInvoice'))).then(function (d) {
            if (d && d.found) { setText('txtContrsQty', d.NoOfContainers); contrsQtyChanged(); setVal('cmbDestinationPort', d.DestinationPortId); }
            else { setText('txtContrsQty', '0'); contrsQtyChanged(); setVal('cmbDestinationPort', '0'); }
        }).catch(function (e) { box(e.message); });
    }
    /** FinalETADate / datFinalEtaDate_ValueChanged. */
    function finalEta() {
        if (S.updating) return; S.updating = true;
        var t = val('txtTransitDays').trim();
        setText('datFinalEtaDate', /^-?\d+(\.\d+)?$/.test(t) ? F.addDays(val('txtETA'), Math.trunc(parseFloat(t))) : val('txtETA'));
        S.updating = false;
    }
    function finalEtaChanged() {
        if (S.updating) return; S.updating = true;
        setText('txtTransitDays', String(F.dayDiff(val('datFinalEtaDate'), val('txtETA'))));
        S.updating = false;
    }
    /** Reset(). */
    function reset() {
        S.attachmentsValues = ''; S.customAttachmentsValues = ''; S.recId = 0;
        show('btnsave', true); show('btnupdate', false);
        F.setEnabled('cmbExportInvoice', true);
        setText('txtBookingDate', F.today()); setText('txtBookingCRO', '');
        ['cmbExportInvoice', 'cmbshippingline', 'cmbClearingAgent', 'cmbCarierMedium', 'cmbForwarder', 'cmbDestinationPort', 'CmbContainerDispatchedAtPort', 'CmbContainerReceivedFromPort', 'CmbDELIVEREDAT'].forEach(function (id) { setVal(id, '0'); });
        setText('txtVesselName', ''); setText('txtVoyageNo', '');
        setText('txtETA', F.today()); setText('txtETD', F.today()); setText('txtcutoffdate', F.today()); setText('datFinalEtaDate', F.today());
        setText('txtTransitDays', '0'); setText('txtFreeDays', ''); setText('txtContrsQty', ''); setText('txtBookingRate', ''); setText('txtRemarks', '');
        DET.rows = []; bindGrids();
        focus('txtBookingDate');
        return getJson(API + '/refresh').then(function (d) { INVOICES = d.invoices || []; bind('cmbExportInvoice', INVOICES, 'Id', 'Name', [], true); setVal('cmbExportInvoice', '0'); }).catch(function (e) { box(e.message); });
    }
    /** Insert(): FormHelper.ValidateControls order, ConfirmAction, the detail rows, Save. */
    function insert(btn) {
        return busy(btn, function () {
            if (!val('txtBookingCRO').trim()) { box('BookingCRO field is required'); focus('txtBookingCRO'); return Promise.resolve(); }
            if (!F.hasSel('cmbExportInvoice')) { box('Invoice No field is required'); focus('cmbExportInvoice'); return Promise.resolve(); }
            if (!F.hasSel('cmbCarierMedium')) { box('Carrier Medium field is required'); focus('cmbCarierMedium'); return Promise.resolve(); }
            if (!F.hasSel('cmbshippingline')) { box('Shipping Line field is required'); focus('cmbshippingline'); return Promise.resolve(); }
            if (netI(val('txtContrsQty')) === 0) { box('No of Containers must be a non-zero number'); focus('txtContrsQty'); return Promise.resolve(); }
            if (netI(val('txtFreeDays')) === 0) { box('Free Days must be a non-zero number'); focus('txtFreeDays'); return Promise.resolve(); }
            if (!ask(S.recId === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return Promise.resolve();
            var details = DET.rows.map(function (r) {
                var t = CTYPES.filter(function (c) { return netI(c.Id) === netI(r.ContainerTypeId); })[0];
                return { Id: r.Id, ContainerNo: r.ContainerNo, SealNo: r.SealNo, ContainerTypeId: netI(r.ContainerTypeId), ContainerType: t ? t.Name : '' };
            });
            if (netI(val('txtContrsQty')) < DET.rows.length) { box('Container Rows can not greater than No. of Containers '); return Promise.resolve(); }
            return postJson(API + '/save', {
                recId: S.recId, bookingDate: val('txtBookingDate'), bookingCro: val('txtBookingCRO'), invoiceId: netI(val('cmbExportInvoice')),
                shippingLineId: netI(val('cmbshippingline')), forwarderId: netI(val('cmbForwarder')), clearingAgentId: netI(val('cmbClearingAgent')),
                carierMediumId: netI(val('cmbCarierMedium')), vesselName: val('txtVesselName').trim(), voyageNo: val('txtVoyageNo').trim(),
                eta: val('txtETA'), etd: val('txtETD'), cutOffDate: val('txtcutoffdate'), transitDays: netI(val('txtTransitDays')), freeDays: netI(val('txtFreeDays')),
                destinationPortId: netI(val('cmbDestinationPort')), contrsQty: netI(val('txtContrsQty')), bookingRate: netD(val('txtBookingRate')),
                containerDispatchedAtPortId: netI(val('CmbContainerDispatchedAtPort')), containerReceivedFromPortId: netI(val('CmbContainerReceivedFromPort')),
                deliveredAtId: netI(val('CmbDELIVEREDAT')), remarks: val('txtRemarks'), details: details,
                attachmentsValues: S.attachmentsValues, customAttachmentsValues: S.customAttachmentsValues
            }).then(function (d) {
                box((d && d.message) || (S.recId > 0 ? 'Record Update Successfully' : 'Record Saved Successfully'));
                return reset();
            }).catch(function (e) { box(e.message); });
        });
    }
    function save(btn) { S.recId = 0; return insert(btn); }
    function update(btn) { if (S.recId === 0) { box('RecId not found'); return Promise.resolve(); } return insert(btn); }
    /** ReadById(Id). */
    function readById(id) {
        return reset().then(function () { return getJson(API + '/by-id?id=' + encodeURIComponent(id)); }).then(function (r) {
            S.recId = netI(r.Id);
            tab('tabPage1');
            show('btnsave', false); show('btnupdate', true);
            setText('txtBookingDate', r.BookingDate || F.today()); setText('txtBookingCRO', r.BookingCroNo);
            if (netI(r.ExImInvoiceId) > 0) { bind('cmbExportInvoice', [{ Id: r.ExImInvoiceId, Name: r.InvoiceNo }], 'Id', 'Name', [], true); setVal('cmbExportInvoice', r.ExImInvoiceId); }
            F.setEnabled('cmbExportInvoice', false);
            F.setValOrAdd('cmbshippingline', r.ShippingLineId, ''); F.setValOrAdd('cmbForwarder', r.TransporterId, ''); F.setValOrAdd('cmbClearingAgent', r.ShippingAgentId, '');
            setText('txtContrsQty', str(r.ContrsQty));
            F.setValOrAdd('cmbCarierMedium', r.CarierMediumId, '');
            setText('txtVesselName', r.VesselName); setText('txtVoyageNo', r.VoyageNo);
            setText('txtETA', r.ETADate || F.today()); setText('txtETD', r.ETDDate || F.today());
            setText('txtcutoffdate', r.CuttOFFDate || F.today());
            setText('txtTransitDays', str(r.TransitDays)); finalEta();
            F.setValOrAdd('cmbDestinationPort', r.PortId, ''); setText('txtFreeDays', str(r.FreeDaysAtDestinations));
            setText('txtBookingRate', F.fmt(r.BookingRate));
            F.setValOrAdd('CmbContainerDispatchedAtPort', r.ContainerDispatchedAtPortId, ''); F.setValOrAdd('CmbContainerReceivedFromPort', r.ContainerReceivedFromPortId, '');
            setVal('CmbDELIVEREDAT', r.DeliveredAtId);
            setText('txtRemarks', r.RemarksHeader);
            S.attachmentsValues = str(r.AttachmentsValues); S.customAttachmentsValues = str(r.CustomAttachmentsValues);
            DET.rows = (r.details || []).map(function (d) { return { Id: d.Id, ContainerNo: d.ContainerNo, SealNo: d.SealNo, ContainerTypeId: d.ContainerTypeId }; });
            bindGrids();
            focus('txtBookingDate');
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------------------ history

    var HCOLS = [
        { key: 'BookingDate', fmt: 'date' }, { key: 'BookingCroNo' }, { key: 'InvoiceNo' }, { key: 'ShippingLine' }, { key: 'DestinationPort' },
        { key: 'ForwarderName' }, { key: 'VesselName' }, { key: 'VoyageNo' }, { key: 'FreeDays', fmt: 'int' }, { key: 'NoOfContainers', fmt: 'int' },
        { key: 'ClearingAgent' }, { key: 'ETADate', fmt: 'date' }, { key: 'ETDDate', fmt: 'date' },
        { key: 'NotReferredContainerNos', cls: function () { return 'exf-red'; } }, { key: 'ReferredContainerNos', cls: function () { return 'exf-green'; } },
        { key: 'Remarks' }, { key: 'EntryUser' }, { key: 'EntryDate', fmt: 'datetime', mmm: false }, { key: 'ModifyUser' }, { key: 'ModifyDate', fmt: 'datetime', mmm: false },
        { key: 'NoOfAttachments', fmt: 'int', link: true }];
    function histRender() {
        F.drawGrid('histBody', null, HIST.rows, HCOLS, HIST.cur, function (r, i) {
            return '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Edit" data-i="' + i + '">Edit</button></td>' +
                '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Print" data-i="' + i + '">Print</button></td>';
        });
        show('histEmpty', HIST.rows.length === 0);
    }
    /** btnShowHistory_Click -> GridBind. */
    function historyShow(btn) {
        return busy(btn, function () {
            return postJson(API + '/history', {
                dateBy: (document.querySelector('input[name="dateBy"]:checked') || {}).value || 'doc',
                fromChecked: F.checked('FromDateHistoryChk'), toChecked: F.checked('ToDateHistoryChk'), fromDate: val('FromDateHistory'), toDate: val('ToDateHistory'),
                bookingId: netI(val('CmbBookingNoHistory')), customerId: netI(val('CmbCustomerHistory')), forwarderId: netI(val('CmbForwarderHistory')),
                shippingLineId: netI(val('CmbShippingLinHistory')), shippingAgentId: netI(val('CmbClearingAgentHistory')),
                invoiceId: netI(val('CmbInvoiceNoHistory')), destinationPortId: netI(val('CmbDestinationPortHistory'))
            }).then(function (rows) { HIST.rows = rows || []; HIST.cur = -1; histRender(); }).catch(function (e) { box(e.message); });
        });
    }
    /** btnResetHistory_Click. */
    function historyReset() {
        setText('FromDateHistory', F.daysAgo(3)); setText('ToDateHistory', F.today());
        ['CmbCustomerHistory', 'CmbClearingAgentHistory', 'CmbBookingNoHistory', 'CmbInvoiceNoHistory', 'CmbDestinationPortHistory'].forEach(function (id) { var s = $id(id); if (s) { s.selectedIndex = -1; } });
        F.refreshCombos();
        HIST.rows = []; HIST.cur = -1; histRender();
    }
    function historyRefresh(btn) { return busy(btn, function () { return getJson(API + '/history-combos').then(bindHistoryCombos).catch(function (e) { box(e.message); }); }); }
    /** btnPrint_Click / grid Print -> CommonServices.ExBookingInfoSlip560(id): Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt, "560-ExBooking Info(CRO).rpt". */
    function print() {
        if (!PERM.Print) return;
        if (S.recId > 0) F.printSeeded('560-exbooking-info-cro', { id: S.recId }); else box('No Record found');
    }
    function printRow(i) { var r = HIST.rows[i]; if (r) F.printSeeded('560-exbooking-info-cro', { id: r.Id }); }
    function attachment() { box('Attachments (DMS popup) are not part of the web port.'); }
    function lookup() { box('ExImLookups (LookupId 4, Carrier Medium) is a separate definition screen; refresh after defining.'); }
    function shortcuts() {
        F.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+F5', 'For Focus on Booking Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+D', 'When Focus On Container List Add New Row'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowRight', 'For Toggle in Container Grid And History Grid'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function tab(id) { F.innerTab('main', id, 'btnBkFooterHistory', function (p, onHist) { focus(onHist ? 'FromDateHistory' : 'txtBookingDate'); }); }
    function toggleHistory() { tab(F.activeInner('main') === 'tabHistory' ? 'tabPage1' : 'tabHistory'); }

    document.addEventListener('DOMContentLoaded', function () {
        F.wireTabs(function (p, onHist) { focus(onHist ? 'FromDateHistory' : 'txtBookingDate'); }, 'btnBkFooterHistory');
        F.on('cmbExportInvoice', 'change', invoiceLeave);
        F.on('txtContrsQty', 'input', contrsQtyChanged);
        F.on('txtETA', 'change', finalEta);
        F.on('txtTransitDays', 'input', finalEta);
        F.on('datFinalEtaDate', 'change', finalEtaChanged);
        F.on('txtRemarks', 'blur', function (e) { if (!(e.relatedTarget && e.shiftKey)) { /* txtRemarks_Leave -> Booking Date unless Shift+Tab */ } });
        F.wireGrid('contBody', { btn: function (name) { if (name === 'Add') addClick(); }, cell: cellUpdated });
        F.wireGrid('histBody', {
            select: function (i) { HIST.cur = i; },
            open: function (i) { if (PERM.Update) readById(HIST.rows[i].Id); },
            btn: function (name, i) { if (name === 'Edit') readById(HIST.rows[i].Id); else if (name === 'Print') printRow(i); },
            link: function (k, i) { box('Attachments of record ' + HIST.rows[i].Id + ': ' + netI(HIST.rows[i].NoOfAttachments)); }
        });
        /* frmShippingBookingInfo_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (F.enterMoves(e)) return;
            var k = (e.key || '').toLowerCase(), onForm = F.activeInner('main') !== 'tabHistory';
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); F.cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (onForm) {
                if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
                if (e.ctrlKey && k === 's' && !$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled) { e.preventDefault(); save($id('btnsave')); }
                if (e.ctrlKey && k === 'u' && !$id('btnupdate').classList.contains('is-hidden') && !$id('btnupdate').disabled) { e.preventDefault(); update($id('btnupdate')); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
                if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); attachment(); }
                if (e.ctrlKey && e.key === 'F1') { e.preventDefault(); lookup(); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('txtBookingDate'); }
                if (e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowRight')) { e.preventDefault(); var c = $id('contBody').querySelector('input'); if (c) c.focus(); }
                if (e.ctrlKey && k === 'd') { e.preventDefault(); addClick(); }
                if (e.ctrlKey && (e.key === 'Enter' || e.key === ' ') && e.target && e.target.closest && e.target.closest('#contBody')) { e.preventDefault(); addClick(); }
            } else {
                if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
                if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); historyRefresh($id('btnRefreshHistory')); }
                if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); focus('FromDateHistory'); }
                if (e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowRight')) { e.preventDefault(); var tr = $id('histBody').querySelector('tr'); if (tr) tr.scrollIntoView(); }
                if (e.ctrlKey && e.key === 'Enter' && PERM.Update && HIST.cur >= 0) { e.preventDefault(); readById(HIST.rows[HIST.cur].Id); }
            }
        });
        load();
    });

    global.ExportBooking = { reset: reset, save: save, update: update, refresh: refresh, attachment: attachment, lookup: lookup, print: print, shortcuts: shortcuts,
        historyReset: historyReset, historyRefresh: historyRefresh, historyShow: historyShow, toggleHistory: toggleHistory };
}(window));
