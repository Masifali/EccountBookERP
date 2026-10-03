/* ============================================================================================
 * countx_export_dhl_tracking.js - frmDhlTracking.cs (Architecture.WinApp.Export) "DHL Tracking",
 * DocumentTypeId 248. Events: Load, Save, Update, New, Refresh, Attachment, ShortCut Keys, Register-248,
 * CmbCurrentStatus Leave (loads the history), Show, grid DoubleClick / X / Add-Attachment / NoOfAttachments
 * link, form KeyDown - desktop messages and order. No external DHL service exists on the desktop.
 * Data: /api/export/dhl-tracking.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var G = global.ExportG, S = global.ExportSF;
    var API = '/api/export/dhl-tracking';

    var RIGHTS = { Save: false, Update: false };
    var RECID = 0;
    var HIST = [];            /* the grid's rows */
    var LAST = { rows: 0, filter: {} };    /* dtHistory of the last BindGrid (register print) */

    var grdfrm = G.grid('grdfrm', [
        { key: 'DateOfDispatched', fmt: G.ddMMMyyyy }, { key: 'AWBBillNo', link: true }, { key: 'FinalETA', fmt: G.ddMMMyyyy }, { key: 'CurrentStatus' },
        { key: 'Detail' }, { key: 'EntryUser' }, { key: 'EntryDate', datetime: true }, { key: 'ModifyUser' }, { key: 'ModifyDate', datetime: true },
        { key: 'NoOfAttachments', num: true, dec: 0, link: true }
    ], { buttons: [{ key: 'Add-Attachment', text: 'Add-Attachment' }, { key: 'Delete', text: 'X' }], emptyId: 'grdfrmEmpty',
        onButton: gridButton, onDblClick: gridDoubleClick,
        onLink: function (i, key) { if (key === 'NoOfAttachments') G.box('Attachments are not available in the web version.'); else gridDoubleClick(i); } });

    // ------------------------------------------------------------------------------ load

    /** DateLock_Load. */
    function load() {
        ['datDispatched', 'datFinalETA', 'datDispatchedFromHistory', 'datDispatchedToHistory', 'datFinalEtaFromHistory', 'datFinalEtaToHistory']
            .forEach(function (id) { G.setText(id, G.today()); });
        return G.getJson(API + '/setup').then(function (d) {
            d = d || {};
            RIGHTS = d.rights || RIGHTS;
            G.$id('btnSave').disabled = !RIGHTS.Save;
            G.$id('btnUpdate').disabled = !RIGHTS.Update;
            G.show('btnUpdate', false);
            applyCombos(d);
            G.focus('datDispatched');
        }).catch(function (e) { G.box(e.message); });
    }
    function applyCombos(d) {
        if (d.statusesError) G.box(d.statusesError);
        if (d.historyCombosError) G.box(d.historyCombosError);
        if ((d.statuses || []).length) S.bindX('CmbCurrentStatus', d.statuses, 'Id', 'LookUpName', []);
        var h = d.historyCombos;
        if (h && ((h.awb || []).length || (h.status || []).length)) {
            G.bind('txtAwbBillNoHistory', h.awb || [], 'Id', 'name');
            G.bind('CmbCurrentStatusHistory', h.status || [], 'Id', 'name');
        }
    }
    /** btnRefresh_Click. */
    function refresh(btn) {
        return G.busy(btn, function () { return G.getJson(API + '/refresh').then(function (d) { applyCombos(d || {}); }).catch(function (e) { G.box(e.message); }); });
    }

    // ------------------------------------------------------------------------------ save

    /** FormValidation. */
    function validation() {
        if (G.val('txtAwbBillNo').trim() === '') { G.box('AWB Bill No Field is Required'); G.focus('txtAwbBillNo'); return false; }
        if (G.val('txtDetail').trim() === '') { G.box('Detail/Description Field is Required'); G.focus('txtDetail'); return false; }
        if (G.valI('CmbCurrentStatus') === 0) { G.box('Current Status Field is Required'); G.focus('CmbCurrentStatus'); return false; }
        if (G.val('datFinalETA') < G.val('datDispatched')) { G.box("ETA date Can't be less than Dispatched date"); G.focus('datFinalETA'); return false; }
        return true;
    }
    /** Insert(). */
    function insert(btn) {
        return G.busy(btn, function () {
            if (!validation()) return Promise.resolve();
            if (!G.ask(RECID > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            return G.postJson(API + '/save', { recId: RECID, dateOfDispatched: G.val('datDispatched'), finalEtaDate: G.val('datFinalETA'),
                awbBillNo: G.val('txtAwbBillNo'), detail: G.val('txtDetail'), currentStatusId: G.valI('CmbCurrentStatus') }).then(function (d) {
                G.box(d && d.message);
                reset();
            }).catch(function (e) { G.box(e.message); });
        });
    }
    function save(btn) { RECID = 0; return insert(btn); }
    function update(btn) { return insert(btn); }
    /** Reset (btnNew_Click). */
    function reset() {
        RECID = 0;
        G.setText('txtDetail', ''); G.setText('txtAwbBillNo', '');
        G.show('btnUpdate', false); G.show('btnSave', true);
        G.focus('CmbCurrentStatus');
    }

    // ------------------------------------------------------------------------------ history

    function selText(id) { var s = G.$id(id); return s && s.selectedIndex > 0 ? s.options[s.selectedIndex].textContent : ''; }
    /** BindGrid: checked pickers, the AWB combo's TEXT, the status value; the grid is only re-bound when rows came back. */
    function bindGrid(btn) {
        return G.busy(btn, function () {
            var f = {
                dispatchedFrom: G.checked('datDispatchedFromHistoryChk') ? G.val('datDispatchedFromHistory') : '',
                dispatchedTo: G.checked('datDispatchedToHistoryChk') ? G.val('datDispatchedToHistory') : '',
                etaFrom: G.checked('datFinalEtaFromHistoryChk') ? G.val('datFinalEtaFromHistory') : '',
                etaTo: G.checked('datFinalEtaToHistoryChk') ? G.val('datFinalEtaToHistory') : '',
                awbBillNo: selText('txtAwbBillNoHistory').trim(),
                statusId: G.valI('CmbCurrentStatusHistory')
            };
            return G.postJson(API + '/history', f).then(function (rows) {
                rows = rows || [];
                LAST = { rows: rows.length, filter: f };
                if (rows.length > 0) { HIST = rows; grdfrm.current = -1; grdfrm.draw(HIST); }
                else if (!HIST.length) grdfrm.draw([]);
            }).catch(function (e) { G.box(e.message); });
        });
    }
    /** grdfrm_DoubleClick: GetByID into the entry boxes, Update mode. */
    function gridDoubleClick(i) {
        var r = HIST[i]; if (!r) return;
        RECID = G.netI(r.DHLTrackingId);
        G.getJson(API + '/by-id?id=' + RECID).then(function (h) {
            G.setText('datDispatched', h.DateOfDispatched);
            G.setText('txtAwbBillNo', h.AWBBillNumber);
            G.setText('datFinalETA', h.FinalETADate);
            G.setText('txtDetail', h.Detail);
            G.setVal('CmbCurrentStatus', h.CurrentStatusId);
            G.show('btnSave', false); G.show('btnUpdate', true);
        }).catch(function (e) { G.box(e.message); });
    }
    /** grdfrm_ColumnButtonClick: Add-Attachment / Delete. */
    function gridButton(i, key, btn) {
        var r = HIST[i]; if (!r) return;
        var id = G.netI(r.DHLTrackingId);
        if (key === 'Add-Attachment') { G.box('Attachments are not available in the web version.'); return; }
        if (key === 'Delete') {
            if (RECID === id || RECID > 0) { G.box('Please reset form first to delete'); return; }
            if (!G.ask('Are you sure to delete?')) return;
            G.busy(btn, function () {
                return G.postJson(API + '/delete', { id: id, openRecId: RECID }).then(function (d) {
                    HIST.splice(i, 1); grdfrm.draw(HIST);
                    G.box((d && d.message) || 'Record deleted successfully!');
                }).catch(function (e) { G.box(e.message); });
            });
        }
    }
    /** btnRegister_Click: 248 register of the last history set. */
    function register(btn) {
        if (!LAST.rows) { G.box('No Record Found For Display'); return; }
        S.print('exp-248', LAST.filter, btn);
    }
    function shortcuts() {
        G.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on LookUp'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On Lookup'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = G.$id(id); if (e) e.addEventListener(ev, fn); }

    document.addEventListener('DOMContentLoaded', function () {
        G.initFullscreen();
        on('btnNew', 'click', reset);
        on('btnRefresh', 'click', function () { refresh(this); });
        on('btnSave', 'click', function () { save(this); });
        on('btnUpdate', 'click', function () { update(this); });
        on('btnAttachment', 'click', function () { G.box('Attachments are not available in the web version.'); });
        on('btnShortcukeys', 'click', shortcuts);
        on('btnRegister', 'click', function () { register(this); });
        on('btnShow', 'click', function () { bindGrid(this); });
        on('CmbCurrentStatus', 'change', function () { bindGrid(null); });        /* CmbLookupsType_Leave */
        on('btnFooterHistory', 'click', function () { var b = G.$id('grdfrmBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); });
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('.ex-fullscreen')) { e.preventDefault(); G.cancelWindow(); } return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            if (e.ctrlKey && k === 's' && !G.$id('btnSave').classList.contains('is-hidden') && !G.$id('btnSave').disabled) { e.preventDefault(); save(G.$id('btnSave')); }
            if (e.ctrlKey && k === 'u' && !G.$id('btnUpdate').classList.contains('is-hidden') && !G.$id('btnUpdate').disabled) { e.preventDefault(); update(G.$id('btnUpdate')); }
            if (e.ctrlKey && k === 'p') { e.preventDefault(); register(G.$id('btnRegister')); }
            if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); G.box('Attachments are not available in the web version.'); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); G.focus('grdfrm'); }
            if (e.ctrlKey && (e.key === 'ArrowUp' || e.key === 'F5')) { e.preventDefault(); G.focus('datDispatched'); }
        });
        load();
    });

    global.ExportDhlTracking = { save: save, update: update, reset: reset, bindGrid: bindGrid };
})(window);
