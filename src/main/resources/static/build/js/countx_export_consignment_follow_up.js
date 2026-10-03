/* countx_export_consignment_follow_up.js - frmshippedConsignment_followup.cs, screen 203 "5009 Consignment Follow Up Report".
 * Data: /api/export/consignment-follow-up. Header entry (Save / Update), the filtered History grid with in-grid date /
 * status editing (Update per row, "Update" for the checked rows), the CRO / invoice slip links. */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/consignment-follow-up';
    var PERM = { Save: true, Update: true };
    var CUR_STATUS = [], DOC_STATUS = [], ROWS = [], CUR = -1, REC_ID = 0, UPDATE_MODE = false;
    var DATE_PICKERS = ['txtDocToBank', 'txtDocToParty', 'txtLoadOnTrainDate1', 'txtReachedAtLoadingPortDate2', 'txtLoadedOnVesselDate3', 'datCuttOfDate',
        'txtETDLoadingPort', 'txtETDFinalDate4', 'txtReachedDestinationPortDate', 'txtGoodsDeclarationDate', 'txtPortPayment'];
    var GRID_DATES = ['DispatchedFactory', 'LoadOnTrainDate', 'ReachedAtLoadingPortDate', 'LoadedOnVesselDate', 'CuttOfDate', 'ETDLoadingPort', 'ETDDestinationPort',
        'DocToBank', 'DocToParty', 'ReachedAtDestinationPort', 'GoodsDeclarationDate', 'PortPaymentDate'];

    function dateCell(key) {
        return function (r, i, shown) {
            if (!PERM.Update) return H.esc(shown);
            return '<input type="date" class="cf-edit" data-i="' + i + '" data-key="' + key + '" value="' + H.esc(H.isoDate(H.col(r, key))) + '"/>';
        };
    }
    function listCell(key, list) {
        return function (r, i) {
            var v = H.netI(H.col(r, key));
            if (!PERM.Update) { var f = list.filter(function (x) { return H.netI(x.Id) === v; })[0]; return H.esc(f ? f.Name : ''); }
            var h = '<select class="cf-edit" data-i="' + i + '" data-key="' + key + '"><option value="0"></option>';
            list.forEach(function (x) { h += '<option value="' + H.esc(x.Id) + '"' + (H.netI(x.Id) === v ? ' selected' : '') + '>' + H.esc(x.Name) + '</option>'; });
            return h + '</select>';
        };
    }
    /* grdHistory_FormattingRow: CuttOfDate within txtCutOffDateDays -> red bold; ETDDestinationPort / ReachedAtDestinationPort within txtETADestinationPortDays -> dark green bold. */
    function within(v, days) {
        var s = H.isoDate(v); if (!s || s.substring(0, 4) === '0001' || s === '1900-01-01') return false;
        var d = new Date(s + 'T00:00:00'), t = new Date(); t.setHours(0, 0, 0, 0);
        var rem = Math.round((d - t) / 86400000);
        return rem >= 0 && rem <= days;
    }
    var COLS = [
        { key: 'ShipmentId', hidden: true }, { key: 'InvoiceId', hidden: true }, { key: 'InvoiceNo', cap: 'Invoice No', link: true }, { key: 'InvoiceDate', cap: 'Invoice Date', kind: 'date' },
        { key: 'Customer', cap: 'Customer' }, { key: 'DestinationPort', cap: 'Destination Port' }, { key: 'CRO_Number', cap: 'CRO Number', link: true }, { key: 'ShippingLine', cap: 'Shipping Line' },
        { key: 'ShippingAgent', hidden: true }, { key: 'Forwarder', cap: 'Forwarder' }, { key: 'FclQty', cap: 'Fcl Qty', kind: 'q2', sum: true }, { key: 'MTons', cap: 'M Tons', kind: 'q2', sum: true },
        { key: 'FcurrencyId', hidden: true }, { key: 'PaymentStatus', cap: 'Payment Status' },
        { key: 'DispatchedFactory', cap: 'Dispatched Factory', kind: 'date', render: dateCell('DispatchedFactory'), cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'LoadOnTrainDate', cap: 'Load On Train Date', kind: 'date', render: dateCell('LoadOnTrainDate'), cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'ReachedAtLoadingPortDate', cap: 'Reached At Loading Port Date', kind: 'date', render: dateCell('ReachedAtLoadingPortDate'), cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'LoadedOnVesselDate', cap: 'Loaded On Vessel Date', kind: 'date', render: dateCell('LoadedOnVesselDate'), cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'CuttOfDate', cap: 'Cutt Of Date', kind: 'date', render: dateCell('CuttOfDate'), cls: function (r) { return (PERM.Update ? 'win-editable ' : '') + (within(H.col(r, 'CuttOfDate'), H.netI(H.val('txtCutOffDateDays'))) ? 'warn-red' : ''); } },
        { key: 'ETDLoadingPort', cap: 'ETD Loading Port', kind: 'date', render: dateCell('ETDLoadingPort'), cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'ETDDestinationPort', cap: 'ETD Destination Port', kind: 'date', render: dateCell('ETDDestinationPort'), cls: function (r) { return (PERM.Update ? 'win-editable ' : '') + (within(H.col(r, 'ETDDestinationPort'), H.netI(H.val('txtETADestinationPortDays'))) ? 'warn-green' : ''); } },
        { key: 'DocToBank', cap: 'Doc To Bank', kind: 'date', render: dateCell('DocToBank'), cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'DocToParty', cap: 'Doc To Party', kind: 'date', render: dateCell('DocToParty'), cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'ReachedAtDestinationPort', cap: 'Reached At Destination Port', kind: 'date', render: dateCell('ReachedAtDestinationPort'), cls: function (r) { return (PERM.Update ? 'win-editable ' : '') + (within(H.col(r, 'ReachedAtDestinationPort'), H.netI(H.val('txtETADestinationPortDays'))) ? 'warn-green' : ''); } },
        { key: 'CourierTracingNo', cap: 'Courier Tracing No' },
        { key: 'DocumentStatus', cap: 'Document Status', render: function (r, i) { return listCell('DocumentStatus', DOC_STATUS)(r, i); }, cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'CurrentStatus', cap: 'Current Status', render: function (r, i) { return listCell('CurrentStatus', CUR_STATUS)(r, i); }, cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'GoodsDeclarationNo', cap: 'Goods Declaration No' },
        { key: 'GoodsDeclarationDate', cap: 'Goods Declaration Date', kind: 'date', render: dateCell('GoodsDeclarationDate'), cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'PortPaymentDate', cap: 'Port Payment Date', kind: 'date', render: dateCell('PortPaymentDate'), cls: function () { return PERM.Update ? 'win-editable' : ''; } },
        { key: 'ContainerNos', cap: 'Container Nos' }, { key: 'DocumentTypeId', hidden: true }, { key: 'NoOfAttachments', cap: 'No Of Attachments', kind: 'int' },
        { key: 'Update', cap: 'Update', render: function (r, i) { return PERM.Update ? '<button type="button" class="win-edit" data-act="update" data-i="' + i + '">Update</button>' : ''; } }
    ];

    function render() {
        H.drawGrid('grdHistory', ROWS, COLS, {
            cur: CUR,
            leadHead: '<th class="lead ctr"><input type="checkbox" id="chkSelectAll" title="Select all"/></th><th class="lead">Edit</th>',
            lead: function (r, i) {
                return '<td class="lead ctr"><input type="checkbox" class="cf-sel" data-i="' + i + '"' + (r.__sel ? ' checked' : '') + '/></td>' +
                    '<td class="lead win-cell-btn"><button type="button" class="win-edit" data-act="edit" data-i="' + i + '">Edit</button></td>';
            }
        });
        H.show('grdHistoryEmpty', !ROWS.length);
        var all = H.$id('chkSelectAll');
        if (all) all.addEventListener('change', function () { ROWS.forEach(function (r) { r.__sel = all.checked; }); H.$id('grdHistory').querySelectorAll('.cf-sel').forEach(function (c) { c.checked = all.checked; }); });
    }
    function bindStatusList() {
        var box = H.$id('cmbStatus');
        var keep = checkedStatusIds();
        box.innerHTML = DOC_STATUS.map(function (s) {
            return '<label><input type="checkbox" class="cf-status" value="' + H.esc(s.Id) + '"' + (keep.indexOf(String(s.Id)) >= 0 ? ' checked' : '') + '/> ' + H.esc(s.Name) + '</label>';
        }).join('');
    }
    function checkedStatusIds() { return Array.prototype.map.call(document.querySelectorAll('#cmbStatus .cf-status:checked'), function (c) { return c.value; }); }
    function bindCombos(d) {
        H.bind('InvoiceNo', d.invoices || [], 'Id', 'Name');
        CUR_STATUS = d.currentStatuses || []; DOC_STATUS = d.documentStatuses || [];
        H.bind('cmbcurrsatatus', CUR_STATUS, 'Id', 'Name');
        H.bind('cmbdocstatus', DOC_STATUS, 'Id', 'Name');
        bindStatusList();
    }
    /** GridFill with the checked Document Status ids. */
    function showGrid(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/grid', { statusIds: checkedStatusIds() }).then(function (rows) { ROWS = rows || []; CUR = -1; render(); });
        });
    }
    function setPicker(id, iso) {
        var has = !!iso && iso.substring(0, 4) !== '0001' && iso !== '1900-01-01';
        H.setText(id, has ? iso : H.today());
        H.setChecked(id + 'Chk', has);
    }
    function clearPickers() { DATE_PICKERS.forEach(function (id) { setPicker(id, ''); }); }
    function setMode(update) {
        UPDATE_MODE = update;
        H.show('btnSave', !update); H.show('btnUpdate', update);
    }
    /** InvoiceNo_Leave: GetByID(invoice) fills the header (found -> Update mode), else the defaults. */
    function invoiceLeave() {
        var id = H.netI(H.val('InvoiceNo'));
        return H.getJson(API + '/by-invoice?invoiceId=' + id).then(function (d) {
            d = d || {};
            REC_ID = H.netI(d.recId);
            if (d.found && d.record) {
                var r = d.record;
                H.setVal('cmbdocstatus', r.DocumentStatusId); H.setVal('cmbcurrsatatus', r.CurrentStatusId);
                H.setText('txtCourierTrackingNo', r.CourierTracingNo); H.setText('txtGdNo', r.GdNo);
                setPicker('datCuttOfDate', r.CuttOfDate); setPicker('txtDocToBank', r.DocToBankDate); setPicker('txtDocToParty', r.DocCourierToPartyDate);
                setPicker('txtLoadOnTrainDate1', r.LoadOnTrainDate); setPicker('txtReachedAtLoadingPortDate2', r.ReachedAtLoadingPortDate);
                setPicker('txtLoadedOnVesselDate3', r.LoadedOnVesselDate); setPicker('txtETDFinalDate4', r.ETDFinalDate);
                setPicker('txtReachedDestinationPortDate', r.ReachedAtDestinationPort); setPicker('txtGoodsDeclarationDate', r.GdDate);
                setPicker('txtPortPayment', r.PortPayment); setPicker('txtETDLoadingPort', r.ETDLoadingPort);
                setMode(true);
            } else {
                /* the desktop resets only these six pickers, the two statuses and the courier no */
                ['txtDocToBank', 'txtDocToParty', 'txtETDFinalDate4', 'txtLoadedOnVesselDate3', 'txtLoadOnTrainDate1', 'txtReachedAtLoadingPortDate2'].forEach(function (p) { setPicker(p, ''); });
                H.setVal('cmbcurrsatatus', 0); H.setVal('cmbdocstatus', 0);
                H.setText('txtCourierTrackingNo', '');
            }
        }).catch(function (e) { H.box(e.message); });
    }
    /** ReadById(InvoiceId) - the grid's Edit button / double-click / Ctrl+Enter. */
    function readById(invoiceId) {
        H.setVal('InvoiceNo', invoiceId);
        return invoiceLeave().then(function () { setMode(true); H.$id('panel3').scrollIntoView({ behavior: 'smooth' }); });
    }
    /** Reset(): status filter cleared, GridFill, header back to defaults, Save mode. */
    function reset() {
        document.querySelectorAll('#cmbStatus .cf-status').forEach(function (c) { c.checked = false; });
        showGrid(null);
        H.setVal('InvoiceNo', 0);
        clearPickers();
        H.setVal('cmbcurrsatatus', 0); H.setVal('cmbdocstatus', 0);
        setMode(false);
        H.setText('txtCourierTrackingNo', '');
        REC_ID = 0;
        H.focus('InvoiceNo');
    }
    /** Insert(): "Please Select Invoice!", the Yes/No confirm, then Save; RecId > 0 -> Update. */
    function insert(btn, recId) {
        if (H.netI(H.val('InvoiceNo')) === 0) { H.focus('InvoiceNo'); H.box('Please Select Invoice!'); return Promise.resolve(); }
        if (recId > 0) { if (!H.ask('Are you sure to Update?')) return Promise.resolve(); }
        else if (!H.ask('Are you sure to Save?')) return Promise.resolve();
        var body = { recId: recId, invoiceId: H.netI(H.val('InvoiceNo')), currentStatusId: H.netI(H.val('cmbcurrsatatus')), documentStatusId: H.netI(H.val('cmbdocstatus')),
            courierTrackingNo: H.val('txtCourierTrackingNo'), gdNo: H.val('txtGdNo') };
        DATE_PICKERS.forEach(function (p) { body[p] = H.val(p); body[p + 'Checked'] = H.checked(p + 'Chk'); });
        return H.busy(btn, function () {
            return H.postJson(API + '/save', body).then(function (d) { H.box((d && d.message) || 'Record Save Successfully'); reset(); });
        });
    }
    function save(btn) { REC_ID = 0; return insert(btn, 0); }
    function update(btn) { return insert(btn, REC_ID); }
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindCombos); }); }

    function rowPayload(r) {
        var p = {};
        for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k) && k.indexOf('__') !== 0) p[k] = r[k];
        return p;
    }
    /** UpdateByGrid(row): the row's edited cells against the stored record; confirm, save, GridFill. */
    function updateRow(i, btn) {
        var r = ROWS[i]; if (!r) return;
        var shipmentId = H.netI(H.col(r, 'ShipmentId'));
        return H.busy(btn, function () {
            /* the change check runs on the server (needs the stored record); the confirmation text follows the desktop */
            if (shipmentId > 0) { if (!H.ask('Are you sure to Update the record?')) return; }
            else if (!H.ask('Are you sure to Save new record?')) return;
            return H.postJson(API + '/update-row', rowPayload(r)).then(function (d) { H.box((d && d.message) || 'Record Update Successfully'); return showGrid(null); });
        });
    }
    /** btnUpdateStatus_Click: the checked rows through MultiSave. */
    function updateStatuses(btn) {
        var sel = ROWS.filter(function (r) { return r.__sel; });
        if (!sel.length) { H.box('Please Select Rows first to update document status of multi rows'); return; }
        if (!H.ask('Are you sure to Update the Statuses?')) return;
        return H.busy(btn, function () {
            return H.postJson(API + '/update-statuses', { rows: sel.map(rowPayload) }).then(function () { return showGrid(null); });
        });
    }
    /* in-grid edits: kept on the row until Update / the multi Update sends them */
    function cellChanged(el) {
        var i = +el.getAttribute('data-i'), key = el.getAttribute('data-key'), r = ROWS[i]; if (!r) return;
        r[key] = el.value;
        if (key === 'CuttOfDate' || key === 'ETDDestinationPort' || key === 'ReachedAtDestinationPort') {
            var td = el.closest('td');
            var days = key === 'CuttOfDate' ? H.netI(H.val('txtCutOffDateDays')) : H.netI(H.val('txtETADestinationPortDays'));
            td.classList.toggle(key === 'CuttOfDate' ? 'warn-red' : 'warn-green', within(el.value, days));
        }
    }
    /** grdHistory_LinkClicked: CRO_Number -> 560 CRO slip (after the row-count check), InvoiceNo -> 521 invoice slip. */
    function link(i, key) {
        var r = ROWS[i]; if (!r) return;
        var invoiceId = H.netI(H.col(r, 'InvoiceId'));
        if (key === 'CRO_Number') {
            H.getJson(API + '/cro-check?invoiceId=' + invoiceId).then(function (d) {
                if (!d || !d.count) { H.box('No Record Found For Display'); return; }
                H.print('560-ExBooking Info(CRO).rpt', { exImInvoiceId: invoiceId }, null);
            }).catch(function (e) { H.box(e.message); });
        } else if (key === 'InvoiceNo') {
            if (!invoiceId) { H.box('No Record Found For Display'); return; }
            H.print('521-ExportInvoiceSlip.rpt', { eximInvoiceId: invoiceId, id: invoiceId }, null);
        }
    }
    /* btnPrintSlip / btnPrintRegister are Visible=false on the desktop; kept callable. */
    function printSlip(btn) { var id = H.netI(H.val('InvoiceNo')); if (!id) { H.box('No Record Found For Display'); return; } return H.print('511-ExImShippedConsignmentFollowUps_Slip.rpt', { eximInvoiceId: id }, btn); }
    function printRegister(btn) { var id = H.netI(H.val('InvoiceNo')); if (!id) { H.box('No Record Found For Display'); return; } return H.print('512-ExImShippedConsignmentFollowUps_Register.rpt', { eximInvoiceId: id }, btn); }
    /* btnLookup_Click opens the ExImLookups master (a separate desktop form, not part of this port). */
    function lookup() { H.box('The Export Look-ups form (ExImLookups) is not available on this page.'); }
    function shortcuts() { H.shortcuts([]); }
    function toggleHistory() { var b = H.$id('grdHistoryBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('grdHistory'); }

    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
            H.$id('btnSave').disabled = !PERM.Save;
            H.$id('btnUpdate').disabled = !PERM.Update;
            H.$id('btnUpdateStatus').disabled = !PERM.Update;
            bindCombos(d);
            clearPickers();
            H.$id('footerInfo').textContent = 'frmshippedConsignment_followup';
            return showGrid(null).then(function () { H.focus('cmbStatus'); });
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.$id('InvoiceNo').addEventListener('change', invoiceLeave);
        var g = H.$id('grdHistory');
        g.addEventListener('change', function (e) {
            var s = e.target.closest('.cf-sel'); if (s) { var r = ROWS[+s.getAttribute('data-i')]; if (r) r.__sel = s.checked; return; }
            var el = e.target.closest('.cf-edit'); if (el) cellChanged(el);
        });
        H.wireGrid('grdHistory', {
            select: function (i) { CUR = i; },
            open: function (i) { var r = ROWS[i]; if (r) readById(H.col(r, 'InvoiceId')); },
            link: link,
            button: function (i, act, b) { var r = ROWS[i]; if (!r) return; if (act === 'edit') readById(H.col(r, 'InvoiceId')); else if (act === 'update') updateRow(i, b); }
        });
        ['txtCutOffDateDays', 'txtETADestinationPortDays'].forEach(function (id) { H.$id(id).addEventListener('input', render); });
        H.wireKeys({ shortcuts: shortcuts, handle: function (e, k) {
            if (!e.ctrlKey) return;
            if (e.target.closest('.cf-edit')) return;
            if (k === '1' || k === '2') { e.preventDefault(); printSlip(H.$id('btnPrintSlip')); }
            else if (k === 'u' && UPDATE_MODE) { e.preventDefault(); update(H.$id('btnUpdate')); }
            else if (k === 's' && !UPDATE_MODE) { e.preventDefault(); save(H.$id('btnSave')); }
            else if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'r') { e.preventDefault(); refresh(H.$id('btnRefresh')); }
            else if (e.key === 'F5') { e.preventDefault(); H.focus('InvoiceNo'); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdHistory'); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); H.focus('cmbStatus'); }
            else if (e.key === 'Enter' && CUR >= 0) { e.preventDefault(); var r = ROWS[CUR]; if (r) readById(H.col(r, 'InvoiceId')); }
        } });
        load();
    });

    global.ExportConsignmentFollowUp = { showGrid: showGrid, reset: reset, save: save, update: update, refresh: refresh, updateStatuses: updateStatuses,
        printSlip: printSlip, printRegister: printRegister, lookup: lookup, shortcuts: shortcuts, toggleHistory: toggleHistory };
}(window));
