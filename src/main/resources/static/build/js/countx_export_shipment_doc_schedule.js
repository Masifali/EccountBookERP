/* ============================================================================================
 * countx_export_shipment_doc_schedule.js - frmShipmentDocumentSchedule.cs (Architecture.WinApp.SDT), screen
 * 624 "Shipment Doc Schedule". Data: /api/export/shipment-doc-schedule.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var S = global.ExportSdt;
    var API = '/api/export/shipment-doc-schedule';
    var $id = S.$id, box = S.box, ask = S.ask, netI = S.netI, str = S.str, netB = S.netB;

    var PERM = { Save: true, Update: true };
    var SCHEDULES = [], STATUS = [], UN = [], AS = [];
    var CUR = { as: -1 };

    // ------------------------------------------------------------------ load

    /** frmShipmentDocumentSchedule_Load. */
    function load() {
        return S.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
            $id('BtnDelete').disabled = !PERM.Save;
            $id('BtnSave').disabled = !PERM.Save;
            S.bind('CmbSalesPersonContract', d.salesPersons || [], 'Id', 'Name', []);
            S.bind('CmbCustomerContract', d.customers || [], 'Id', 'Name', []);
            SCHEDULES = d.schedules || []; bindSchedules();
            STATUS = d.statuses || [];
            unRender(); asRender();
            $id('RadReferedRefDoc').checked = true;
        }).catch(function (e) { box(e.message); });
    }
    function bindSchedules() {
        S.bind('CmbScheduleNo', SCHEDULES, 'Id', 'ScheduleCode', ['CustomerName', 'LoadingDate', 'NoOfContainer', 'MTon', 'ExImLcOrderId', 'ScheduleNo', 'InvoiceDate', 'InvoiceMTon', 'IsForInvoiced']);
    }
    function selectedSchedule() { return S.findRow(SCHEDULES, S.val('CmbScheduleNo')); }
    function refDocumentTypeId() { var r = selectedSchedule(); return r ? netI(r.DocumentTypeId) : 0; }
    /** ScheduleNoComboDbCall + ScheduleNoComboBind (selection retained). */
    function reloadSchedules() {
        return S.getJson(API + '/schedules?supplierCustomerId=' + netI(S.val('CmbCustomerContract')) + '&salesPersonId=' + netI(S.val('CmbSalesPersonContract'))
            + '&referedInSDS=' + ($id('RadReferedRefDoc').checked ? 'true' : 'false')).then(function (rows) { SCHEDULES = rows || []; bindSchedules(); });
    }
    /** BtnRefreshUnAssigned_Click: schedules + StatusFill. */
    function refresh(btn) {
        return S.busy(btn, function () {
            return reloadSchedules().then(function () { return S.getJson(API + '/statuses'); }).then(function (rows) { STATUS = rows || []; asRender(); });
        });
    }
    /** CmbScheduleNo_Leave: the custom groups of the schedule's customer, the lock flag and the attachment button text. */
    function scheduleChanged() {
        var r = selectedSchedule();
        var isForInvoiced = false, lock = false;
        if (r && netI(r.Id) > 0) {
            lock = netB(r.LockScheduleForSDS);
            isForInvoiced = netB(r.IsForInvoiced);
            var cg = netI(r.CustomGroupId);
            S.getJson(API + '/custom-groups?supplierCustomerId=' + netI(r.SupplierCustomerId)).then(function (rows) {
                rows = rows || [];
                S.bind('CmbCustomGroup', rows, 'Id', 'name', []);
                if (cg > 0) S.setVal('CmbCustomGroup', S.findRow(rows, cg) ? cg : 0);
            }).catch(function (e) { box(e.message); });
        } else {
            S.bind('CmbCustomGroup', [], 'Id', 'name', []);
        }
        $id('btnAttachment').querySelector('span').textContent = isForInvoiced ? 'Invoice Attachments' : 'Schedule Attachments';
        $id('ChkIsLockSchedule').checked = lock;
    }

    // ------------------------------------------------------------------ grids

    var UN_COLS = [{ key: 'Document' }, { key: 'RequiredLevel' }];
    function unRender() { S.drawGrid('unBody', null, UN, UN_COLS, { select: true }); S.show('unEmpty', UN.length === 0); }
    function asCols() {
        return [
            { key: 'CustomGroup' }, { key: 'Document' }, { key: 'Original', kind: 'int', sum: true }, { key: 'Duplicate', kind: 'int', sum: true },
            { key: 'BaseCriteriaDate', kind: 'dmy' }, { key: 'BeforeDays', kind: 'int', sum: true }, { key: 'AfterDays', kind: 'int' }, { key: 'DueDate', kind: 'dmy' },
            { key: 'ReadyDate', edit: 'date' }, { key: 'DocStatus', edit: 'select', options: STATUS }, { key: 'PrioritySequence', kind: 'int', sum: true },
            { key: 'Remarks', edit: 'text', width: 160 }, { key: 'DocumentProviderName', edit: 'text', width: 120 }, { key: 'AddAttachments', kind: 'int' }
        ];
    }
    function asRender() { S.drawGrid('asBody', 'asFoot', AS, asCols(), { select: true, del: true, cur: CUR.as }); S.show('asEmpty', AS.length === 0); }

    /** ShowData(Confirm). */
    function showData(confirm) {
        var refDocId = netI(S.val('CmbScheduleNo')), cg = netI(S.val('CmbCustomGroup'));
        if (refDocId === 0) { UN = []; AS = []; unRender(); asRender(); S.focus('CmbScheduleNo'); throw new Error('Please Select Schedule First'); }
        if (cg === 0) { UN = []; AS = []; unRender(); asRender(); S.focus('CmbCustomGroup'); throw new Error('Please Select CustomGroup First'); }
        if (confirm) {
            if (AS.length > 0) { if (!ask('grd Allocated have Records,Do you want to refresh?')) return Promise.resolve(); }
            else if (UN.length > 0 && !ask('grd UnAllocated have Records,Do you want to refresh?')) return Promise.resolve();
        }
        return S.getJson(API + '/show?refDocId=' + refDocId + '&refDocumentTypeId=' + refDocumentTypeId() + '&customGroupId=' + cg).then(function (d) {
            UN = (d.unassigned || []).map(function (r) { r._checked = netI(r.RequiredLevelId) === 1; return r; });
            AS = d.assigned || []; CUR.as = -1;
            unRender(); asRender();
        });
    }
    function show(btn) { return S.busy(btn, function () { return showData(true); }); }

    /** InsertAuto - BtnSave (toolStrip3, "Click to Allocate selected rows"). */
    function allocate(btn) {
        return S.busy(btn, function () {
            if (netI(S.val('CmbScheduleNo')) === 0) { S.focus('CmbScheduleNo'); throw new Error('ScheduleNo Field Required...'); }
            if (UN.length === 0) throw new Error('Grid has no Record');
            if (!ask('Are you sure to Save?')) return;
            var rows = UN.filter(function (r) { return r._checked; });
            if (rows.length === 0) throw new Error('Please select at least one record.');
            return S.postJson(API + '/allocate', { refDocId: netI(S.val('CmbScheduleNo')), refDocumentTypeId: refDocumentTypeId(), customGroupId: netI(S.val('CmbCustomGroup')), hasRows: true, rows: rows })
                .then(function (d) { box(d.message); return showData(false); });
        });
    }
    /** BtnUpdate_Click -> Insert(): the service repeats the row rules. */
    function save(btn) {
        return S.busy(btn, function () {
            if (netI(S.val('CmbScheduleNo')) === 0) { S.focus('CmbScheduleNo'); throw new Error('ScheduleNo Field Required...'); }
            if (netI(S.val('CmbCustomGroup')) === 0) { S.focus('CmbCustomGroup'); throw new Error('CustomGroup Field Required...'); }
            if (AS.length === 0) throw new Error('Grid Record not found...');
            var update = AS.some(function (r) { return netI(r.Id) > 0; });
            if (!ask(update ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            return S.postJson(API + '/save', { refDocId: netI(S.val('CmbScheduleNo')), refDocumentTypeId: refDocumentTypeId(), customGroupId: netI(S.val('CmbCustomGroup')),
                lockSchedule: $id('ChkIsLockSchedule').checked, rows: AS }).then(function (d) { box(d.message); return showData(false); });
        });
    }
    /** BtnDelete_Click (UnAllocate the checked allocated rows). */
    function unallocate(btn) {
        return S.busy(btn, function () {
            if (netI(S.val('CmbScheduleNo')) === 0) { S.focus('CmbScheduleNo'); throw new Error('ScheduleNo Field Required...'); }
            if (netI(S.val('CmbCustomGroup')) === 0) { S.focus('CmbCustomGroup'); throw new Error('CustomGroup Field Required...'); }
            if (AS.length === 0) throw new Error('Grid has no records.');
            var rows = AS.filter(function (r) { return r._checked; });
            if (rows.length === 0) throw new Error('Please select at least one record.');
            if (rows.some(function (r) { return netI(r.AddAttachments) > 0; }) && !ask('Attachments will be deleted along with the selected rows. Are you sure you want to continue?')) return;
            return S.postJson(API + '/unallocate', { refDocId: netI(S.val('CmbScheduleNo')), refDocumentTypeId: refDocumentTypeId(), customGroupId: netI(S.val('CmbCustomGroup')), hasRows: true, rows: rows })
                .then(function (d) { box(d.message); return showData(false); });
        });
    }
    /** DeletedtGridRow: every allocated row is saved on this form -> the desktop's message. */
    function deleteRow(i) {
        var r = AS[i]; if (!r) return;
        if (netI(r.Id) > 0) { box("You Can't Delete Saved Record..."); return; }
        AS.splice(i, 1); CUR.as = -1; asRender();
    }
    function cellChanged(i, key, v, input) {
        var r = AS[i]; if (!r) return;
        if (key === 'Original' || key === 'Duplicate' || key === 'BeforeDays' || key === 'AfterDays' || key === 'PrioritySequence') {
            if (str(v).trim() !== '' && !/^\s*-?\d+(\.\d+)?\s*$/.test(str(v))) { box('Please Type Only Numeric Value'); input.value = str(r[key]); return; }
            r[key] = netI(v); return;
        }
        if (key === 'DocStatus') r[key] = netI(v); else r[key] = v;
    }
    /** FormReset. */
    function reset() {
        UN = []; AS = []; CUR.as = -1;
        S.setVal('CmbScheduleNo', 0);
        unRender(); asRender();
        S.focus('CmbScheduleNo');
        $id('RadReferedRefDoc').checked = true;
        reloadSchedules().catch(function (e) { box(e.message); });   /* RadReferedRefDoc_CheckedChanged */
    }
    function attachments() {
        if (!selectedSchedule()) { S.focus('CmbScheduleNo'); box('Please select a Schedule / Invoice No first.'); return; }
        box('The attachment dialog (GetSaveAndDeleteAttachmentsForRecord) is a desktop file dialog and is not available on the web.');
    }
    function toggleHistory() { box('The desktop removes the History tab of this form on load (tabControl1.TabPages.Remove(tabPage3)); there is no history here.'); }
    function shortcuts() {
        S.shortcutKeys([['Ctrl+S', 'For Save of Focused Grid'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New of Focused Grid'], ['Ctrl+L', 'For Load All'], ['Ctrl+P', 'For Print'],
            ['Alt+1', 'For Print 551 in History'], ['Alt+2', 'For Print 551_01 in History'], ['Ctrl+F5', "For Focus on 'Sale Contract for Schedule' Grid"], ['Ctrl+F10', 'For Open Attachments'],
            ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Right', 'For Move focus From One Grid to Next'], ['Ctrl+ArrowDown', 'For Focus On Schedule Detail Grid'],
            ['Ctrl+ArrowUp', 'For Focus On Date Combo in Detail Box'], ['Ctrl+Enter', 'For Update Record When Focus On Any Grid '], ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        S.wireFullscreen();
        S.wireGrid('unBody', { sel: function (i, on) { if (UN[i]) UN[i]._checked = on; } });
        S.wireHeaderSelector('unSelTh', function () { return UN; }, unRender);
        S.wireGrid('asBody', { sel: function (i, on) { if (AS[i]) AS[i]._checked = on; }, del: deleteRow, select: function (i) { CUR.as = i; }, cell: cellChanged });
        S.wireHeaderSelector('asSelTh', function () { return AS; }, asRender);
        var on = function (id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); };
        on('CmbCustomerContract', 'change', function () { reloadSchedules().catch(function (e) { box(e.message); }); });
        on('CmbSalesPersonContract', 'change', function () { reloadSchedules().catch(function (e) { box(e.message); }); });
        on('RadReferedRefDoc', 'change', function () { reloadSchedules().catch(function (e) { box(e.message); }); });
        on('RadReferedNotRefDoc', 'change', function () { reloadSchedules().catch(function (e) { box(e.message); }); });
        on('CmbScheduleNo', 'change', scheduleChanged);
        document.addEventListener('keydown', function (e) {
            if (S.enterMovesOn(e)) return;
            var k = (e.key || '').toLowerCase();
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); S.cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); return; }
            if (e.ctrlKey && (k === 's' || k === 'u')) { e.preventDefault(); if (PERM.Update) save($id('BtnUpdate')); return; }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $id('grdUnAssignDoc').scrollIntoView(); return; }
            if (e.ctrlKey && (e.key === 'ArrowDown' || e.key === 'ArrowRight')) { e.preventDefault(); $id('grdAssignDoc').scrollIntoView(); return; }
            if (e.ctrlKey && e.key === 'Delete' && CUR.as >= 0) { e.preventDefault(); deleteRow(CUR.as); }
        });
        load();
    });

    global.ExportShipmentDocSchedule = { reset: reset, save: save, refresh: refresh, show: show, allocate: allocate, unallocate: unallocate, attachments: attachments, toggleHistory: toggleHistory, shortcuts: shortcuts };
}(window));
