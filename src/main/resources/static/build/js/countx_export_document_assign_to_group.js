/* ============================================================================================
 * countx_export_document_assign_to_group.js - frmDocumentOfGroup.cs (Architecture.WinApp.SDT), screen 623
 * "Document Assign To Group" (DocumentTypeId 243). Data: /api/export/document-assign-to-group.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var S = global.ExportSdt;
    var API = '/api/export/document-assign-to-group';
    var $id = S.$id, box = S.box, ask = S.ask, netI = S.netI, str = S.str, netB = S.netB;

    var PERM = { Save: true };
    var CTYPE = [], PTYPE = [], UN = [], AS = [], HIST = [];
    var RecId = 0, LastCriteriaDateTypeId = 0, DEFAULT_DAYS = 0;
    var CUR = { as: -1, hist: -1 };

    // ------------------------------------------------------------------ load

    /** frmDocumentOfGroup_Load. */
    function load() {
        return S.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM.Save = d.permissions.Save !== false;
            $id('btnsave').disabled = !PERM.Save;
            CTYPE = d.criteriaDateTypes || []; PTYPE = d.documentProviders || [];
            S.bind('CmbCustomGroup', d.customGroups || [], 'Id', 'name', []);
            DEFAULT_DAYS = netI(d.defaultDaysToLessFromHistoryFromDate);
            bindHistoryCombos(d);
            S.setText('txtFromDate', S.daysAgo(DEFAULT_DAYS > 0 ? DEFAULT_DAYS : 3));
            S.setText('txtToDate', S.today());
            unRender(); asRender();
        }).catch(function (e) { box(e.message); });
    }
    function bindHistoryCombos(d) {
        S.bind('CmbCustomGroupHis', d.historyCustomGroups || [], 'Id', 'name', []);
        S.bind('CmbDocumentHis', d.historyDocuments || [], 'Id', 'name', []);
        S.bind('CmbRequiredLevelHis', d.historyRequiredLevels || [], 'Id', 'name', []);
        S.bind('CmbDocumentProviderHis', d.historyProviders || [], 'Id', 'name', []);
    }
    /** BtnRefreshUnAssigned_Click. */
    function refresh(btn) {
        return S.busy(btn, function () {
            return S.getJson(API + '/refresh').then(function (d) {
                S.bind('CmbCustomGroup', d.customGroups || [], 'Id', 'name', []);
                CTYPE = d.criteriaDateTypes || []; PTYPE = d.documentProviders || [];
                asRender();
            });
        });
    }

    // ------------------------------------------------------------------ grids

    var UN_COLS = [{ key: 'Document' }, { key: 'DocProviderName' }];
    function unRender() {
        S.drawGrid('unBody', null, UN, UN_COLS, { select: true, group: 'RequiredLevel', groupTitle: 'RequiredLevel' });
        S.show('unEmpty', UN.length === 0);
    }
    function asCols() {
        return [
            { key: 'Document' }, { key: 'RequiredLevel' },
            { key: 'DocumentProviderId', edit: 'select', options: PTYPE },
            { key: 'Original', kind: 'int', edit: 'int', sum: true }, { key: 'Duplicate', kind: 'int', edit: 'int', sum: true },
            { key: 'CriteriaDateTypeId', edit: 'select', options: CTYPE },
            { key: 'BeforeDays', kind: 'int', edit: 'int', sum: true }, { key: 'AfterDays', kind: 'int', edit: 'int' },
            { key: 'PrioritySequence', kind: 'int', edit: 'int', sum: true }, { key: 'Remarks', edit: 'text', width: 160 }, { key: 'Active', edit: 'check' }
        ];
    }
    function asRender() {
        S.drawGrid('asBody', 'asFoot', AS, asCols(), { del: true, cur: CUR.as });
        S.show('asEmpty', AS.length === 0);
    }

    /** btnShowByCustomGroup_Click -> ShowData(); then LastRecordBycustomGroupId. */
    function show(btn) { return S.busy(btn, function () { return showData(true); }); }
    function showData(confirm) {
        S.selectTab('main', 'tabPage1');
        var cg = netI(S.val('CmbCustomGroup'));
        if (cg <= 0) {
            UN = []; AS = []; unRender(); asRender(); S.focus('CmbCustomGroup');
            throw new Error('Please Select custom Group First');
        }
        if ((UN.length > 0 || AS.length > 0) && confirm && !ask('grd have Records,Do you want to refresh?')) return Promise.resolve();
        return S.getJson(API + '/show?customGroupId=' + cg + '&withLast=' + (confirm ? 'true' : 'false')).then(function (d) {
            RecId = cg;
            UN = (d.unassigned || []).map(function (r) { r._checked = netI(r.RequiredLevelId) === 1; return r; });   /* CheckedAllRowsWithMadatoryStatusInUnAssign */
            AS = d.assigned || [];
            if (confirm && d.lastCriteriaDateTypeId !== undefined) LastCriteriaDateTypeId = netI(d.lastCriteriaDateTypeId);
            CUR.as = -1; unRender(); asRender();
        });
    }

    /** BtnTransferToAssign_Click -> TransferDataToAssign. */
    function transfer() {
        try {
            if (UN.length === 0) throw new Error('No Record Found for transfer');
            var checkedRows = UN.filter(function (r) { return r._checked; });
            if (checkedRows.length === 0) throw new Error('Please Select any Record');
            var remove = {};
            checkedRows.forEach(function (r) {
                AS.push({ Id: 0, DocumentId: netI(r.DocumentId), Document: str(r.Document), RequiredLevelId: netI(r.RequiredLevelId), RequiredLevel: str(r.RequiredLevel),
                    DocumentProviderId: netI(r.DocumentProviderId), Original: 1, Duplicate: 0, CriteriaDateTypeId: LastCriteriaDateTypeId, BeforeDays: 0, AfterDays: 1,
                    PrioritySequence: 1, Remarks: '', Active: true });
                remove[netI(r.DocumentId)] = true;
            });
            UN = UN.filter(function (r) { return !remove[netI(r.DocumentId)]; });
            unRender(); asRender();
        } catch (e) { box(e.message); }
    }
    /** DeletedtGridRow: saved rows cannot be deleted; a new row goes back to the un-assigned grid. */
    function deleteRow(i) {
        var r = AS[i]; if (!r) return;
        if (netI(r.Id) > 0) { box("You Can't Delete Saved Record..."); return; }
        var docId = netI(r.DocumentId);
        if (!UN.some(function (u) { return netI(u.DocumentId) === docId; })) {
            var p = S.findRow(PTYPE, r.DocumentProviderId);
            UN.push({ DocumentId: docId, Document: str(r.Document), RequiredLevelId: netI(r.RequiredLevelId), RequiredLevel: str(r.RequiredLevel),
                DocumentProviderId: netI(r.DocumentProviderId), DocProviderName: p ? str(p.name) : '', _checked: false });
            unRender();
        }
        AS.splice(i, 1); CUR.as = -1; asRender();
    }
    /** grdAssignDoc_UpdatingCell / CellUpdated. */
    function cellChanged(i, key, v, input) {
        var r = AS[i]; if (!r) return;
        if (key === 'Original' || key === 'Duplicate' || key === 'BeforeDays' || key === 'AfterDays' || key === 'PrioritySequence') {
            if (str(v).trim() !== '' && !/^\s*-?\d+(\.\d+)?\s*$/.test(str(v))) { box('Please Type Only Numeric Value'); input.value = str(r[key]); return; }
            r[key] = netI(v);
            asRender();
            return;
        }
        if (key === 'Active') r[key] = !!v;
        else if (key === 'CriteriaDateTypeId' || key === 'DocumentProviderId') r[key] = netI(v);
        else r[key] = v;
    }

    /** btnsave_Click -> Insert(): the service repeats the row rules; confirm text by the update state. */
    function save(btn) {
        return S.busy(btn, function () {
            if (netI(S.val('CmbCustomGroup')) === 0) { S.focus('CmbCustomGroup'); throw new Error('Custom Group Field Required...'); }
            if (AS.length === 0) throw new Error('Grid Record not found...');
            var update = AS.some(function (r) { return netI(r.Id) > 0; });
            if (!ask(update ? 'Are you sure to Update Main Schedule?' : 'Are you sure to Save Main Schedule?')) return;
            return S.postJson(API + '/save', { customGroupId: netI(S.val('CmbCustomGroup')), rows: AS }).then(function (d) {
                box(d.message);
                reset();
            });
        });
    }
    /** FormReset. */
    function reset() {
        RecId = 0; UN = []; AS = []; CUR.as = -1;
        S.setVal('CmbCustomGroup', 0);
        unRender(); asRender();
        S.focus('CmbCustomGroup');
    }

    // ------------------------------------------------------------------ history

    var HIST_COLS = [
        { key: 'CustomGroup' }, { key: 'Document', link: true }, { key: 'RequiredLevel' }, { key: 'DocumentProvider' },
        { key: 'Original', kind: 'int', sum: true }, { key: 'Duplicate', kind: 'int', sum: true }, { key: 'CriteriaDateType' },
        { key: 'BeforeDays', kind: 'int', sum: true }, { key: 'AfterDays', kind: 'int' }, { key: 'PrioritySequence', kind: 'int', sum: true },
        { key: 'Remarks' }, { key: 'Active', kind: 'bool' }, { key: 'EntryDate', kind: 'dt', mode: 'sec' }, { key: 'EntryUser' },
        { key: 'ModifyDate', kind: 'dt', mode: 'sec' }, { key: 'ModifyUser' }, { key: 'ApprovedDate', kind: 'dt', mode: 'sec' }, { key: 'ApprovedUser' }
    ];
    function histRender() { S.drawGrid('histBody', 'histFoot', HIST, HIST_COLS, { edit: true, cur: CUR.hist }); S.show('histEmpty', HIST.length === 0); }
    /** btnShow_Click -> DataGridHistoryFill. */
    function historyShow(btn) {
        return S.busy(btn, function () {
            return S.postJson(API + '/history', {
                dateType: S.radio('histDateType'),
                fromChecked: S.checked('txtFromDateChk'), fromDate: S.val('txtFromDate'),
                toChecked: S.checked('txtToDateChk'), toDate: S.val('txtToDate'),
                customGroupId: netI(S.val('CmbCustomGroupHis')), chartOfDocumentId: netI(S.val('CmbDocumentHis')),
                requiredLevelId: netI(S.val('CmbRequiredLevelHis')), documentProviderId: netI(S.val('CmbDocumentProviderHis'))
            }).then(function (rows) { HIST = rows || []; CUR.hist = -1; histRender(); });
        });
    }
    /** btnLoadAllHistoryNew_Click. */
    function historyReset() {
        ['CmbCustomGroupHis', 'CmbDocumentHis', 'CmbRequiredLevelHis', 'CmbDocumentProviderHis'].forEach(function (id) { S.setVal(id, 0); });
        HIST = []; histRender(); S.focus('txtFromDate');
    }
    function historyRefresh(btn) { return S.busy(btn, function () { return S.getJson(API + '/history-combos').then(bindHistoryCombos); }); }
    /** DataGridHistory Edit / Ctrl+Enter: CmbCustomGroup.Value = row's CustomGroupId; ShowData(false). */
    function historyEdit(i) {
        var r = HIST[i]; if (!r) return;
        S.setVal('CmbCustomGroup', r.CustomGroupId);
        try { var p = showData(false); if (p && p.catch) p.catch(function (e) { box(e.message); }); } catch (e) { box(e.message); }
    }
    function tabChanged(id) {
        var b = $id('btnFooterHistory');
        b.querySelector('span').textContent = id === 'tabPage3' ? 'Form' : 'History';
        if (id === 'tabPage3') S.focus('txtFromDate'); else $id('grdUnAssignDoc').scrollIntoView();
    }
    function toggleHistory() { S.selectTab('main', S.activeTab('main') === 'tabPage3' ? 'tabPage1' : 'tabPage3', tabChanged); }
    function shortcuts() {
        S.shortcutKeys([['Ctrl+S', 'For Save of Focused Grid'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New of Focused Grid'], ['Ctrl+L', 'For Load All'], ['Ctrl+P', 'For Print'],
            ['Alt+1', 'For Print 551 in History'], ['Alt+2', 'For Print 551_01 in History'], ['Ctrl+F5', "For Focus on 'Sale Contract for Schedule' Grid"], ['Ctrl+F10', 'For Open Attachments'],
            ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Right', 'For Move focus From One Grid to Next'], ['Ctrl+ArrowDown', 'For Focus On Schedule Detail Grid'],
            ['Ctrl+ArrowUp', 'For Focus On Date Combo in Detail Box'], ['Ctrl+Enter', 'For Update Record When Focus On Any Grid '], ['Ctrl+Space', "To Call Function's On Button Or Link When Focus On Any Grid "]]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        S.wireFullscreen();
        S.tabs('main', tabChanged);
        S.wireGrid('unBody', { sel: function (i, on) { if (UN[i]) UN[i]._checked = on; } });
        S.wireHeaderSelector('unSelTh', function () { return UN; }, unRender);
        S.wireGrid('asBody', { del: deleteRow, select: function (i) { CUR.as = i; }, cell: cellChanged });
        S.wireGrid('histBody', { edit: historyEdit, open: historyEdit, select: function (i) { CUR.hist = i; } });
        document.addEventListener('keydown', function (e) {
            if (S.enterMovesOn(e)) return;
            var k = (e.key || '').toLowerCase();
            var onHist = S.activeTab('main') === 'tabPage3';
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); S.cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 'n' && !onHist) { e.preventDefault(); reset(); return; }
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (!onHist) { if (PERM.Save) save($id('btnsave')); } else historyShow($id('btnShowHis')); return; }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowDown' || e.key === 'ArrowRight')) {
                e.preventDefault();
                if (onHist) { if (e.key === 'F5') S.focus('txtFromDate'); else $id('DataGridHistory').scrollIntoView(); }
                else $id(e.key === 'F5' ? 'grdUnAssignDoc' : 'grdAssignDoc').scrollIntoView();
                return;
            }
            if (onHist && e.ctrlKey && (e.key === 'Enter' || e.key === ' ') && CUR.hist >= 0) { e.preventDefault(); historyEdit(CUR.hist); return; }
            if (!onHist && e.ctrlKey && e.key === 'Delete' && CUR.as >= 0) { e.preventDefault(); deleteRow(CUR.as); }
        });
        load();
    });

    global.ExportDocumentOfGroup = { reset: reset, save: save, refresh: refresh, show: show, transfer: transfer, historyShow: historyShow,
        historyReset: historyReset, historyRefresh: historyRefresh, toggleHistory: toggleHistory, shortcuts: shortcuts };
}(window));
