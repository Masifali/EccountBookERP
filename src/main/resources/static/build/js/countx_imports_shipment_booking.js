/* ============================================================================================
 * countx_imports_shipment_booking.js - 785 Shipment Booking, ported from
 * Architecture.WinApp.Import.Transactions.frmShipmentBooking. Built on countx_hrm.js (window.HRM) and
 * countx_imports_impb_core.js (window.ImpB); exports window.ImpBBooking. Each handler follows the form
 * method named in its comment: validation order and wording, what New / Save / Update / Edit do.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/import/shipment-booking';
    var P = {}; window.ImpBBooking = P;
    var RecId = 0, rights = {}, historyDays = 0, dtInvoice = [], AT = ImpB.attachmentState();

    function btnCell(act, text) { return '<button type="button" class="win-btn-action impb-cell-btn" data-act="' + act + '">' + text + '</button>'; }
    function fmtRate(v) { return ImpB.fmt(v, 3, 0); }                       // exchangeRate.ToString("#,##0.###")

    // ------------------------------------------------------------------ History grid (GridBind + GridSetting)
    var grd = new HRM.Grid('grdfrm', {
        columns: [
            { key: 'Print', caption: 'Print', width: 50, render: function () { return btnCell('print', 'Print'); } },
            { key: 'Edit', caption: 'Edit', width: 40, render: function () { return btnCell('edit', 'Edit'); } },
            { key: 'Id', caption: 'Id', hidden: true },
            { key: 'BookingDate', caption: 'BookingDate', type: 'date' },
            { key: 'BookingNo', caption: 'BookingNo' },
            { key: 'InvoiceNo', caption: 'InvoiceNo' },
            { key: 'ContainerType', caption: 'ContainerType' },
            { key: 'NoOfContainers', caption: 'NoOfContainers', type: 'int' },
            { key: 'LoadingPort', caption: 'LoadingPort' },
            { key: 'DestinationPort', caption: 'DestinationPort' },
            { key: 'Currency', caption: 'Currency' },
            { key: 'ExchangeRate', caption: 'ExchangeRate', type: 'num', render: function (v) { return HRM.esc(v === null || v === undefined ? '' : String(HRM.num(v))); } },
            { key: 'Vessel', caption: 'Vessel' },
            { key: 'Voyage', caption: 'Voyage' },
            { key: 'ETADate', caption: 'ETADate', type: 'date' },
            { key: 'ETDDate', caption: 'ETDDate', type: 'date' },
            { key: 'TransitDays', caption: 'TransitDays', type: 'int' },
            { key: 'ETAFinalDate', caption: 'ETAFinalDate', type: 'date' },
            { key: 'ShippingLine', caption: 'ShippingLine' },
            { key: 'ContainerLocation', caption: 'ContainerLocation' },
            { key: 'Address', caption: 'Address' },
            { key: 'FreeDays', caption: 'FreeDays', type: 'int' },
            { key: 'EntryUser', caption: 'EntryUser' },
            { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
            { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'code' }
        ],
        filterRow: true,
        onDouble: function (r) { if (rights.update) ReadById(HRM.int(HRM.col(r, 'Id'))); },          // grdfrm_DoubleClick
        onCode: function (r) { showAttachments(HRM.int(HRM.col(r, 'Id'))); }                         // grdfrm_LinkClicked
    });
    HRM.$('grdfrm').addEventListener('click', function (e) {                                            // grdfrm_ColumnButtonClick
        var b = e.target.closest('button[data-act]'); if (!b) return;
        var tr = b.closest('tr[data-i]'); if (!tr) return;
        var r = grd.rows()[+tr.getAttribute('data-i')], id = HRM.int(HRM.col(r, 'Id'));
        grd.select(+tr.getAttribute('data-i'));
        if (b.getAttribute('data-act') === 'edit') ReadById(id);
        if (b.getAttribute('data-act') === 'print') GenerateReport(id, b);
    });
    HRM.$('grdfrm').addEventListener('keydown', function (e) {                                          // grdfrm_KeyDown
        var r = grd.current(); if (!r) return;
        if (e.ctrlKey && e.key === 'Enter' && rights.update) { e.preventDefault(); ReadById(HRM.int(HRM.col(r, 'Id'))); }
    });

    /** CommonServices.GetNoofAttachmentsByScreenName(Id, base.Name): the attachments of that booking. */
    function showAttachments(id) {
        HRM.get(API + '/attachments', { id: id }).then(function (rows) {
            var st = ImpB.attachmentState(); st.existing = rows || [];
            ImpB.attachments({ state: st, fileUrl: function (a) { return API + '/attachment-file?id=' + id + '&attachmentId=' + HRM.int(HRM.col(a, 'Id')); } });
        }).catch(HRM.fail);
    }

    function GridBind() {
        var f = ImpB.historyFilter('h');
        f.bookingNoId = HRM.comboVal('CmbBookingNoHistory');
        f.shippingLineId = HRM.comboVal('CmbShippingLinHistory');
        f.invoiceId = HRM.comboVal('CmbInvoiceNoHistory');          // sent to ReportsParameters.ImInvoiceId - the BLL never reads it
        f.destinationPortId = HRM.comboVal('CmbDestinationPortHistory');
        return HRM.post(API + '/history', f).then(function (rows) { grd.set(rows || []); }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------ combos
    function InvoiceNoBind(rows) {                                     // keeps the chosen invoice when still listed
        dtInvoice = rows || [];
        HRM.fill('CmbInvoiceNo', dtInvoice, 'Id', 'InvoiceNo', { zero: '', keep: true });
    }
    function fillCombos(d) {
        if (d.invoices) InvoiceNoBind(d.invoices);
        if (d.containerTypes) HRM.fill('CmbContainerType', d.containerTypes, 'Id', 'Type', { zero: false });
        if (d.currencies) HRM.fill('CmbCurrency', d.currencies, 'Id', 'CurrencyCode', { zero: '', keep: true });
        if (d.lines) HRM.fill('cmbshippingline', d.lines, 'Id', 'CompanyName', { zero: '', keep: true });
        if (d.ports) { HRM.fill('cmbDestinationPort', d.ports, 'Id', 'Port', { zero: '', keep: true }); HRM.fill('CmbLoadingPort', d.ports, 'Id', 'Port', { zero: '', keep: true }); }
        if (d.history) {                                                // HistoryCombosFill
            HRM.fill('CmbInvoiceNoHistory', d.history.invoices, 'Id', 'name', { zero: '', keep: true });
            HRM.fill('CmbDestinationPortHistory', d.history.ports, 'Id', 'name', { zero: '', keep: true });
            HRM.fill('CmbShippingLinHistory', d.history.lines, 'Id', 'name', { zero: '', keep: true });
            HRM.fill('CmbBookingNoHistory', d.history.bookingNos, 'Id', 'name', { zero: '', keep: true });
        }
    }

    /** CmbInvoiceNo_Leave: No of Containers = fclTotal, destination / loading port from the invoice row. */
    function CmbInvoiceNo_Leave() {
        var id = HRM.comboVal('CmbInvoiceNo'), row = null;
        dtInvoice.forEach(function (r) { if (HRM.int(HRM.col(r, 'Id')) === id) row = r; });
        if (id > 0 && row) {
            HRM.setVal('txtContrsQty', HRM.str(HRM.num(HRM.col(row, 'fclTotal'))));
            HRM.setCombo('cmbDestinationPort', HRM.int(HRM.col(row, 'destinationPortId')));
            HRM.setCombo('CmbLoadingPort', HRM.int(HRM.col(row, 'loadingPortId')));
        } else {
            HRM.setVal('txtContrsQty', '0');
            HRM.setCombo('cmbDestinationPort', 0); HRM.setCombo('CmbLoadingPort', 0);
        }
    }

    /** FinalETADate(): Final ETA = ETA + Transit Days (txtETA_ValueChanged / txtTransitDays_TextChanged). */
    function FinalETADate() {
        var eta = HRM.val('txtETA') || HRM.today(), t = HRM.val('txtTransitDays').trim();
        HRM.setVal('datFinalEtaDate', t !== '' ? HRM.addDays(eta, Math.trunc(HRM.num(t))) : eta);
    }

    // ------------------------------------------------------------------ New / Reset
    function buttons(update) { HRM.show('btnsave', !update); HRM.show('btnupdate', update); }
    function Reset() {
        AT = ImpB.attachmentState();
        RecId = 0;
        HRM.setVal('txtBookingDate', HRM.today());
        HRM.setVal('txtBookingCRO', '');
        HRM.setCombo('CmbInvoiceNo', 0);
        HRM.setVal('txtContrsQty', '');
        HRM.setCombo('cmbDestinationPort', 0); HRM.setCombo('CmbLoadingPort', 0); HRM.setCombo('CmbCurrency', 0);
        HRM.setVal('txtBookingRate', '');
        HRM.setVal('txtVesselName', ''); HRM.setVal('txtVoyageNo', '');
        HRM.setVal('txtETA', HRM.today()); HRM.setVal('txtETD', HRM.today());
        HRM.setVal('txtTransitDays', '0'); FinalETADate();
        HRM.setCombo('cmbshippingline', 0);
        HRM.setVal('txtContainerLocation', ''); HRM.setVal('txtAddress', ''); HRM.setVal('txtFreeDays', '');
        HRM.enable('CmbInvoiceNo', true);
        buttons(false);
        HRM.focus('txtBookingDate');
        return Promise.all([GridBind(), HRM.get(API + '/invoices', { recId: 0 }).then(InvoiceNoBind).catch(HRM.fail)]);
    }

    // ------------------------------------------------------------------ ReadById
    function ReadById(id) {
        RecId = id;
        return HRM.loading(HRM.get(API + '/by-id', { id: id })).then(function (d) {
            var b = d.record || {};
            InvoiceNoBind(d.invoices);
            AT = ImpB.attachmentState(); AT.existing = d.attachments || [];
            HRM.setVal('txtBookingDate', HRM.day(HRM.col(b, 'bookingDate')));
            HRM.setVal('txtBookingCRO', HRM.str(HRM.col(b, 'bookingNo')));
            HRM.setCombo('CmbInvoiceNo', HRM.int(HRM.col(b, 'invoiceMasterId')));
            CmbInvoiceNo_Leave();
            HRM.setCombo('CmbContainerType', HRM.int(HRM.col(b, 'containerTypeId')));
            HRM.setVal('txtContrsQty', HRM.str(HRM.col(b, 'totalContainer')));
            HRM.setCombo('cmbDestinationPort', HRM.int(HRM.col(b, 'dischargePortId')));
            HRM.setCombo('CmbLoadingPort', HRM.int(HRM.col(b, 'loadingPortId')));   // the form shows dischargePortId here - fixed, see the report
            if (HRM.int(HRM.col(b, 'currencyId')) > 0) HRM.setCombo('CmbCurrency', HRM.int(HRM.col(b, 'currencyId')));
            HRM.setVal('txtBookingRate', fmtRate(HRM.col(b, 'exchangeRate')));
            HRM.setVal('txtVesselName', HRM.str(HRM.col(b, 'vessel')));
            HRM.setVal('txtVoyageNo', HRM.str(HRM.col(b, 'voyage')));
            HRM.setVal('txtETA', HRM.day(HRM.col(b, 'etaLoadingport')));
            HRM.setVal('txtETD', HRM.day(HRM.col(b, 'etdLoadingPort')));
            HRM.setVal('txtTransitDays', HRM.str(HRM.col(b, 'transitDays')));
            HRM.setVal('datFinalEtaDate', HRM.day(HRM.col(b, 'etaDestinationPort')));
            HRM.setCombo('cmbshippingline', HRM.int(HRM.col(b, 'shippingLIneId')));
            HRM.setVal('txtContainerLocation', HRM.str(HRM.col(b, 'containerCollectionLocation')));
            HRM.setVal('txtAddress', HRM.str(HRM.col(b, 'address')));
            HRM.setVal('txtFreeDays', HRM.str(HRM.col(b, 'freeDays')));
            buttons(true);
            HRM.focus('txtBookingDate');
        }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------ Insert
    function formValidation() {
        var no = HRM.val('txtBookingCRO').trim();
        if (no === '' || no === '0') { HRM.box('Booking No Is Required'); HRM.focus('txtBookingCRO'); return false; }
        if (!HRM.comboVal('CmbInvoiceNo')) { HRM.box('Invoice No Is Required'); HRM.focus('CmbInvoiceNo'); return false; }
        if (HRM.val('txtContrsQty').trim() === '' || HRM.int(HRM.val('txtContrsQty')) <= 0) { HRM.box('No of Containers Must Be Greater Than 0 : Thank You'); HRM.focus('txtContrsQty'); return false; }
        if (HRM.comboVal('CmbLoadingPort') <= 0) { HRM.box('Loading Port Is Required'); HRM.focus('CmbLoadingPort'); return false; }
        if (HRM.comboVal('cmbDestinationPort') <= 0) { HRM.box('Destination Port Is Required'); HRM.focus('cmbDestinationPort'); return false; }
        if (HRM.comboVal('cmbshippingline') <= 0) { HRM.box('Shipping Line Is Required'); HRM.focus('cmbshippingline'); return false; }
        if (HRM.val('txtFreeDays').trim() === '' || HRM.int(HRM.val('txtFreeDays')) <= 0) { HRM.box('Free Days Required'); HRM.focus('txtFreeDays'); return false; }
        return true;
    }
    function Insert(btn) {
        if (!formValidation()) return;
        if (!HRM.ask(RecId === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var body = {
            id: RecId, bookingDate: HRM.val('txtBookingDate'), bookingNo: HRM.val('txtBookingCRO'), invoiceMasterId: HRM.comboVal('CmbInvoiceNo'),
            containerTypeId: HRM.comboVal('CmbContainerType'), totalContainer: HRM.val('txtContrsQty'), loadingPortId: HRM.comboVal('CmbLoadingPort'),
            dischargePortId: HRM.comboVal('cmbDestinationPort'), currencyId: HRM.comboVal('CmbCurrency'), exchangeRate: HRM.val('txtBookingRate'),
            vessel: HRM.val('txtVesselName'), voyage: HRM.val('txtVoyageNo'), etaLoadingport: HRM.val('txtETA'), etdLoadingPort: HRM.val('txtETD'),
            transitDays: HRM.val('txtTransitDays'), etaDestinationPort: HRM.val('datFinalEtaDate'), shippingLIneId: HRM.comboVal('cmbshippingline'),
            containerCollectionLocation: HRM.val('txtContainerLocation'), address: HRM.val('txtAddress'), freeDays: HRM.val('txtFreeDays'),
            attachments: ImpB.attachmentPayload(AT)
        };
        return HRM.busy(btn, function () {
            return HRM.post(API + '/save', body).then(function (d) { HRM.box(d && d.message); return Reset(); }).catch(HRM.fail);
        }, 'booking-save');
    }
    function GenerateReport(id, btn) { return ImpB.print(API, id, btn); }         // CommonServices.ImBookingInfoSlip(id)

    // ------------------------------------------------------------------ toolbar
    P.btnnew = function () { return Reset(); };
    P.btnsave = function (btn) { RecId = 0; return Insert(btn || 'btnsave'); };
    P.btnupdate = function (btn) { return Insert(btn || 'btnupdate'); };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () {
            return HRM.get(API + '/refresh', { recId: RecId }).then(fillCombos).catch(HRM.fail);
        });
    };
    P.btnAttachment = function () { ImpB.attachments({ state: AT, fileUrl: function (a) { return API + '/attachment-file?id=' + RecId + '&attachmentId=' + HRM.int(HRM.col(a, 'Id')); } }); };
    P.btnPrint = function (btn) {
        if (RecId > 0) return GenerateReport(RecId, btn || 'btnPrint');
        HRM.box('No Record found');
    };
    P.btnShowHistory = function (btn) { return HRM.busy(btn || 'btnShowHistory', GridBind); };
    P.btnShortCutKey = function () {
        ImpB.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+F5', 'For Focus on Booking Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+D', 'When Focus On Container List Add New Row'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+ArrowRight', 'For Toggle in Container Grid And History Grid'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    };

    ImpB.onLeave('CmbInvoiceNo', CmbInvoiceNo_Leave);
    HRM.$('txtETA').addEventListener('change', FinalETADate);
    HRM.$('txtTransitDays').addEventListener('input', FinalETADate);
    ImpB.wireDateChecks('h');
    HRM.keys({                                                          // frmShippingBookingInfo_KeyDown
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+alt+alt': P.btnShortCutKey, 'ctrl+alt+control': P.btnShortCutKey,
        'ctrl+n': function () { P.btnnew(); },
        'ctrl+s': function () { var b = HRM.$('btnsave'); if (HRM.visible('btnsave') && !b.disabled) P.btnsave(); },
        'ctrl+u': function () { var b = HRM.$('btnupdate'); if (HRM.visible('btnupdate') && !b.disabled) P.btnupdate(); },
        'ctrl+r': function () { P.btnRefresh(); },
        'ctrl+f10': function () { P.btnAttachment(); },
        'ctrl+f5': function () { HRM.focus('txtBookingDate'); }
    });
    HRM.footer(function (b) { return HRM.busy(b, function () { return GridBind().then(function () { HRM.$('boxHistory').scrollIntoView({ block: 'nearest' }); }); }); });

    // ------------------------------------------------------------------ frmShippingBookingInfo_Load
    HRM.setVal('txtBookingDate', HRM.today()); HRM.setVal('txtETA', HRM.today()); HRM.setVal('txtETD', HRM.today());
    FinalETADate();
    HRM.loading(HRM.get(API + '/setup')).then(function (d) {
        rights = d.rights || {};
        HRM.applyRights(rights, { save: 'btnsave', update: 'btnupdate', print: 'btnPrint' });
        historyDays = d.historyDays;
        ImpB.historyDefaults('h', historyDays);
        fillCombos(d);
        buttons(false);
        HRM.focus('txtBookingDate');
        var id = HRM.int(HRM.param('id'));
        if (id > 0) ReadById(id);
    }).catch(HRM.fail);
})();
