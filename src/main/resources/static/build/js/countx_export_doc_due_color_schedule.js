/* ============================================================================================
 * countx_export_doc_due_color_schedule.js - frmDocDueAlertColorSchedule.cs (Architecture.WinApp.SDT),
 * screen 619 "DocDue Color Schedule". Data: /api/export/doc-due-color-schedule
 * (ExportDocumentTrackingController -> ExportDocDueColorScheduleService).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var S = global.ExportSdt;
    var API = '/api/export/doc-due-color-schedule';
    var $id = S.$id, box = S.box, ask = S.ask, netI = S.netI, str = S.str, col = S.col;

    var PERM = { Update: true };
    var COLORS = [], COD = [], UN = [], HIST = [], HIST_DOCS = [];
    var CUR = { cod: -1, hist: -1 };
    var DEFAULT_DAYS = 0;

    // ------------------------------------------------------------------ load

    /** frmDocDueAlertColorSchedule_Load. */
    function load() {
        return S.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM.Update = d.permissions.Update !== false;
            $id('BtnUpdateTemplateII').disabled = !PERM.Update;
            COLORS = d.alertLevelColors || [];
            COD = d.chartOfDocuments || [];
            codRender();
            DEFAULT_DAYS = netI(d.defaultDaysToLessFromHistoryFromDate);
            HIST_DOCS = d.historyDocuments || [];
            S.bind('cmbSupplierNameHistory', HIST_DOCS, 'Id', 'name', []);
            S.setText('FromDateHistory', S.daysAgo(DEFAULT_DAYS > 0 ? DEFAULT_DAYS : 3));
            S.setText('ToDateHistory', S.today());
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    /** btnRefresh_Click (Reset): AlertLevelColor.FormHistory + CODGridFill. */
    function refresh(btn) {
        return S.busy(btn, function () {
            return S.getJson(API + '/refresh').then(function (d) {
                COLORS = d.alertLevelColors || []; COD = d.chartOfDocuments || [];
                CUR.cod = -1; UN = []; codRender(); unRender();
                $id('labelUnAllocateGridTemplateII').textContent = 'Selected COD : ';
            });
        });
    }

    // ------------------------------------------------------------------ Chart Of Document grid

    var COD_COLS = [{ key: 'DocumentCode' }, { key: 'DocumentName' }, { key: 'LeadTime', kind: 'int' }];
    function codRender() {
        S.drawGrid('codBody', null, COD, COD_COLS, { cur: CUR.cod });
        S.show('codEmpty', COD.length === 0);
    }
    /** grdChartOfDocument_SelectionChanged -> UnAllocatedAttributesGridFill(ItemId, Item, LeadTime). */
    function codSelect(i) {
        CUR.cod = i;
        var r = COD[i];
        if (!r) { UN = []; unRender(); HIST = []; histRender(); return; }
        var leadTime = netI(r.LeadTime), interval = leadTime > 0 ? Math.floor(leadTime / 3) : 0;
        UN = [];
        var levelIndex = 0;
        COLORS.forEach(function (c) {
            UN.push({
                chartOfDocumentId: netI(r.Id), DocumentName: str(r.DocumentName),
                ColorGroupId: netI(c.ColorGroupId), ColorGroup: str(c.ColorGroup),
                LevelNameId: netI(c.LevelNameId), LevelName: str(c.LevelName),
                ColorCode: str(c.ColorCode), ColorHex: str(c.ColorHex),
                BeforeRangeFrom: levelIndex * interval + 1, BeforeRangeTo: (levelIndex + 1) * interval, _checked: false
            });
            levelIndex++;
            if (levelIndex === 3) levelIndex = 0;
        });
        unRender();
        $id('labelUnAllocateGridTemplateII').textContent = "Selected ChartOfDocument: '" + str(r.DocumentName) + "'";
    }

    // ------------------------------------------------------------------ Selected COD grid

    var UN_COLS = [
        { key: 'LevelName' }, { key: 'ColorCode', kind: 'color' },
        { key: 'BeforeRangeFrom', kind: 'int', edit: 'int' }, { key: 'BeforeRangeTo', kind: 'int', edit: 'int' }
    ];
    function unRender() {
        S.drawGrid('unBody', null, UN, UN_COLS, { select: true, group: 'ColorGroup', groupTitle: 'ColorGroup' });
        S.show('unEmpty', UN.length === 0);
    }

    /**
     * BtnUpdateTemplateII_Click -> Insert(GridUnAllocatedTemplateII): the first checked row's group gets every
     * row checked (UpdateGridBasedOnGroupId), then the checked rows are validated and saved by the service.
     */
    function update(btn) {
        return S.busy(btn, function () {
            var checkedRows = UN.filter(function (r) { return r._checked; });
            if (checkedRows.length === 0) throw new Error("Check Row's of ColorGroup You want To Update");
            if (!ask('Are you sure to Update ?')) return Promise.resolve();
            var groupId = netI(checkedRows[0].ColorGroupId);
            if (groupId > 0) UN.forEach(function (r) { if (netI(r.ColorGroupId) === groupId) r._checked = true; });
            unRender();
            var rows = [];
            UN.forEach(function (r, i) { if (r._checked) { var c = S.copyRow(r); c.RowIndex = i; rows.push(c); } });
            return S.postJson(API + '/save', { rows: rows }).then(function (d) {
                box(d && d.message ? d.message : "Row's Update Successfully");
                codSelect(CUR.cod);   /* grdChartOfDocument_SelectionChanged(null, null) */
            });
        });
    }

    // ------------------------------------------------------------------ History

    var HIST_COLS = [
        { key: 'ColorGroup' }, { key: 'LevelName' }, { key: 'ColorCode', kind: 'color' },
        { key: 'BeforeRangeFrom', kind: 'int' }, { key: 'BeforeRangeTo', kind: 'int' }, { key: 'SeqNo', kind: 'int' },
        { key: 'EntryDate', kind: 'dt', mode: 'mmm' }, { key: 'EntryUser' }, { key: 'ModifyDate', kind: 'dt', mode: 'mmm' }, { key: 'ModifyUser' }
    ];
    function histRender() {
        S.drawGrid('histBody', null, HIST, HIST_COLS, { cur: CUR.hist, group: 'DocumentName', groupTitle: 'DocumentName' });
        S.show('histEmpty', HIST.length === 0);
    }
    /** btnshow_Click -> HistoryFill. */
    function historyShow(btn) {
        return S.busy(btn, function () {
            return S.postJson(API + '/history', {
                dateType: S.radio('histDateType'),
                fromChecked: S.checked('FromDateHistoryChk'), fromDate: S.val('FromDateHistory'),
                toChecked: S.checked('ToDateHistoryChk'), toDate: S.val('ToDateHistory'),
                chartOfDocumentId: netI(S.val('cmbSupplierNameHistory'))
            }).then(function (rows) { HIST = rows || []; CUR.hist = -1; histRender(); });
        });
    }
    /** btnNewHistory_Click. */
    function historyReset() {
        S.setText('FromDateHistory', S.daysAgo(DEFAULT_DAYS > 0 ? DEFAULT_DAYS : 3));
        S.setText('ToDateHistory', S.today());
        S.setVal('cmbSupplierNameHistory', 0);
        HIST = []; histRender();
    }
    /** btnRefreshHistory_Click -> HistoryComboFill. */
    function historyRefresh(btn) {
        return S.busy(btn, function () {
            return S.getJson(API + '/history-documents').then(function (rows) { HIST_DOCS = rows || []; S.bind('cmbSupplierNameHistory', HIST_DOCS, 'Id', 'name', []); });
        });
    }
    function toggleHistory() { var p = $id('panel1'); p.scrollIntoView({ behavior: 'smooth' }); S.focus('FromDateHistory'); }

    function shortcuts() {
        S.shortcutKeys([['Ctrl+A/Ctrl+S', 'For Press Allocate Button'], ['Ctrl+U', 'For Press Update Button'], ['Ctrl+D/Ctrl+Delete', 'For Press UnAllocate Button'],
            ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+F5', 'For Focus on Item Grid'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowRight', 'to change focus from one grid to another']]);
    }

    // ------------------------------------------------------------------ wiring

    document.addEventListener('DOMContentLoaded', function () {
        S.wireFullscreen();
        S.wireGrid('codBody', { select: codSelect, open: codSelect });
        S.wireGrid('unBody', {
            sel: function (i, on) { if (UN[i]) UN[i]._checked = on; },
            cell: function (i, key, v) { if (UN[i]) UN[i][key] = netI(v); }
        });
        S.wireHeaderSelector('unSelTh', function () { return UN; }, unRender);
        S.wireGrid('histBody', { select: function (i) { CUR.hist = i; } });
        document.addEventListener('keydown', function (e) {
            if (S.enterMovesOn(e)) return;
            var k = (e.key || '').toLowerCase();
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); S.cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnReset')); return; }
            if (e.ctrlKey && (k === 's' || k === 'u') && !$id('BtnUpdateTemplateII').disabled) { e.preventDefault(); update($id('BtnUpdateTemplateII')); return; }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); $id('grdChartOfDocument').scrollIntoView(); return; }
            if (e.ctrlKey && e.key === 'ArrowLeft') { e.preventDefault(); $id('GridUnAllocatedTemplateII').scrollIntoView(); return; }
            if (e.ctrlKey && (e.key === 'ArrowRight' || e.key === 'ArrowDown')) { e.preventDefault(); $id('grdhistory').scrollIntoView(); }
        });
        load();
    });

    global.ExportDocDueColor = { refresh: refresh, update: update, historyShow: historyShow, historyReset: historyReset, historyRefresh: historyRefresh, toggleHistory: toggleHistory, shortcuts: shortcuts };
}(window));
