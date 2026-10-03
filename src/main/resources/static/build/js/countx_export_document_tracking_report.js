/* ============================================================================================
 * countx_export_document_tracking_report.js - frmExportDocumentTrackingReport.cs (Architecture.WinApp.SDT_Reports),
 * screen 625 "Export Document Tracking Report". Data: /api/export/document-tracking-report.
 * The three raw result sets stay in memory (dtGridI / dtGridII / deDetailGrid); the level check boxes
 * re-filter them client-side (FillGrids(Value:false)) exactly as the desktop does.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var S = global.ExportSdt;
    var API = '/api/export/document-tracking-report';
    var $id = S.$id, box = S.box, ask = S.ask, netI = S.netI, str = S.str;

    var PERM = { Save: true };
    var STATUS = [], RAW = { I: [], II: [], detail: [] }, G1 = [], G2 = [], DET = [];
    var YEAR_START = '';

    // ------------------------------------------------------------------ load

    /** frmExportDocumentTrackingReport_Load: rights, ParameterFill, ComboFill, StatusFill, btnshow_Click. */
    function load() {
        return S.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM.Save = d.permissions.Save !== false;
            $id('BtnUpdateTemplateI').disabled = !PERM.Save;
            $id('BtnUpdateTemplateII').disabled = !PERM.Save;
            S.show('GroupBoxValuesToUpdateInGrid', PERM.Save);
            YEAR_START = str(d.financialYearStart);
            S.bind('cmbperemeter', d.dateTypes || [], 'Id', 'Parameters', []);
            S.setVal('cmbperemeter', 1); parameterChanged();          /* Rows[0].Activate() */
            bindCombos(d);
            STATUS = d.statuses || [];
            S.bind('CmbDocmentStausToUpdate', STATUS, 'Id', 'type', []);
            S.setText('GRNToDate', S.today());
            S.setText('txtReadyDateUpdateInGrid', S.today());
            return show($id('btnshow'));
        }).catch(function (e) { box(e.message); });
    }
    function bindCombos(d) {
        S.bind('CmbCustomerName', d.customers || [], 'Id', 'name', []);
        S.bind('CmbInvoiceNo', d.invoices || [], 'Id', 'name', []);
        S.bind('CmbDocument', d.documents || [], 'Id', 'name', []);
        S.bind('CmbdocumentProvider', d.providers || [], 'Id', 'name', []);
        S.bind('ultraCombo1', d.salesPersons || [], 'Id', 'name', []);
    }
    /** btnRefresh_Click: ComboFill + StatusFill (selection retained). */
    function refresh(btn) {
        return S.busy(btn, function () {
            return S.getJson(API + '/refresh').then(function (d) {
                bindCombos(d); STATUS = d.statuses || [];
                S.bind('CmbDocmentStausToUpdate', STATUS, 'Id', 'type', []);
                renderAll();
            });
        });
    }
    /** cmbperemeter_ValueChanged. */
    function parameterChanged() {
        var v = netI(S.val('cmbperemeter')), d = new Date();
        var pad = function (n) { return (n < 10 ? '0' : '') + n; };
        var iso = function (x) { return x.getFullYear() + '-' + pad(x.getMonth() + 1) + '-' + pad(x.getDate()); };
        if (v === 1) S.setText('GRNfromdate', iso(d));
        else if (v === 2) { d.setDate(d.getDate() - 7); S.setText('GRNfromdate', iso(d)); }
        else if (v === 3) { S.setText('GRNfromdate', iso(new Date(d.getFullYear(), d.getMonth(), 1))); S.setText('GRNToDate', S.today()); }
        else if (v === 4) { S.setText('GRNfromdate', d.getFullYear() + '-01-01'); S.setText('GRNToDate', S.today()); }
        else if (v === 5) S.setText('GRNfromdate', S.isoDate(YEAR_START) || '1900-01-01');
    }

    // ------------------------------------------------------------------ grids

    function levelOk(r) {
        var f = S.checked('ChkFirstLevel'), s = S.checked('ChkSecondLevel'), t = S.checked('ChkThirdLevel'), l = netI(r.LevelId);
        return (!f && !s && !t) || (l === 31 && f) || (l === 32 && s) || (l === 33 && t);
    }
    /* the level check texts / colours and the counts, from the raw rows of the grid just filled */
    function levels(rows) {
        var txt = { 31: '', 32: '', 33: '' }, clr = { 31: '', 32: '', 33: '' }, cnt = { 31: 0, 32: 0, 33: 0 };
        rows.forEach(function (r) {
            var l = netI(r.LevelId);
            if (cnt[l] !== undefined) cnt[l]++;
            if (txt[l] === '' && r.LevelName) { txt[l] = str(r.LevelName); clr[l] = str(r.ColorCode); }
        });
        var set = function (id, cid, l, dflt) {
            var e = $id(id); e.textContent = txt[l] || dflt; e.style.color = clr[l] || '';
            $id(cid).textContent = S.fmt(cnt[l], 0);
        };
        set('ChkFirstLevelText', 'txtFirstLevelRowCount', 31, 'First');
        set('ChkSecondLevelText', 'txtSecondLevelRowCount', 32, 'Second');
        set('ChkThirdLevelText', 'txtThirdLevelRowCount', 33, 'Third');
    }
    var dueStyle = function (r) { return r.ColorCode ? 'color:' + r.ColorCode + ';font-weight:bold;' : ''; };
    function g1Cols() {
        return [{ key: 'DocumentName' }, { key: 'ProviderName' }, { key: 'DueDate', kind: 'date', style: dueStyle },
            { key: 'ReadyDate', edit: PERM.Save ? 'date' : null, kind: 'date' }, { key: 'DocumentStatus', edit: PERM.Save ? 'select' : null, options: STATUS, kind: 'status' }, { key: 'Remarks' }];
    }
    function g2Cols() {
        return [{ key: 'ScheduleCode' }, { key: 'InvoiceNo' }, { key: 'InvoiceDate', kind: 'date' }, { key: 'DueDate', kind: 'date', style: dueStyle },
            { key: 'ReadyDate', edit: PERM.Save ? 'date' : null, kind: 'date' }, { key: 'DocumentStatus', edit: PERM.Save ? 'select' : null, options: STATUS, kind: 'status' }, { key: 'Remarks' }];
    }
    var DET_COLS = [{ key: 'CustomerName' }, { key: 'ScheduleCode' }, { key: 'InvoiceNo' }, { key: 'InvoiceDate', kind: 'date' }, { key: 'DocumentName' }, { key: 'ProviderName' },
        { key: 'DueDate', kind: 'date', style: dueStyle }, { key: 'ReadyDate', kind: 'date' }, { key: 'DocumentStatus' }, { key: 'Remarks' }, { key: 'Original', kind: 'int' },
        { key: 'Duplicate', kind: 'int' }, { key: 'CriteriaDate', kind: 'date' }, { key: 'BeforeDays', kind: 'int' }, { key: 'AfterDays', kind: 'int' }, { key: 'Contract' }, { key: 'NoOfAttachments', kind: 'int' }];
    /* a read-only DocumentStatus cell shows the status text */
    function statusText(rows) {
        rows.forEach(function (r) { if (!PERM.Save) { var s = S.findRow(STATUS, r.DocumentStatus); r.DocumentStatusText = s ? s.type : str(r.DocumentStatus); } });
    }
    function renderGroup() {
        G1 = RAW.I.filter(levelOk); G2 = RAW.II.filter(levelOk);
        statusText(G1); statusText(G2);
        var c1 = g1Cols(), c2 = g2Cols();
        if (!PERM.Save) { c1[4] = { key: 'DocumentStatusText' }; c2[5] = { key: 'DocumentStatusText' }; }
        S.drawGrid('g1Body', null, G1, c1, { group: 'Description', groupTitle: 'Description' }); S.show('g1Empty', G1.length === 0);
        S.drawGrid('g2Body', null, G2, c2, { group: 'Description', groupTitle: 'Description' }); S.show('g2Empty', G2.length === 0);
        levels(RAW.II);
    }
    function renderDetail() {
        DET = RAW.detail.filter(levelOk);
        S.drawGrid('detBody', null, DET, DET_COLS, {}); S.show('detEmpty', DET.length === 0);
        levels(RAW.detail);
    }
    function renderAll() { if (S.activeTab('main') === 'tabPage2') renderGroup(); else renderDetail(); }

    /** btnshow_Click -> FillGrids(true). */
    function show(btn) {
        return S.busy(btn, function () {
            var group = S.activeTab('main') === 'tabPage2';
            return S.postJson(API + '/show', {
                which: group ? 'group' : 'detail', dateMode: S.radio('dateMode'),
                fromChecked: S.checked('GRNfromdateChk'), fromDate: S.val('GRNfromdate'), toChecked: S.checked('GRNToDateChk'), toDate: S.val('GRNToDate'),
                supplierCustomerId: netI(S.val('CmbCustomerName')), salesPersonId: netI(S.val('ultraCombo1')), exImInvoiceId: netI(S.val('CmbInvoiceNo')),
                chartOfDocumentId: netI(S.val('CmbDocument')), documentProviderId: netI(S.val('CmbdocumentProvider')),
                readyDocsOnly: netI(S.radio('readyDocs')), pendingInvoice: netI(S.radio('pendingInvoice'))
            }).then(function (d) {
                if (group) { RAW.I = d.gridI || []; RAW.II = d.gridII || []; renderGroup(); }
                else { RAW.detail = d.detail || []; renderDetail(); }
            });
        });
    }
    function cellChanged(rows) {
        return function (i, key, v) { var r = rows[i]; if (!r) return; r[key] = key === 'DocumentStatus' ? netI(v) : v; };
    }
    /** Insert(grd): the changed rows through UpdateDocStatusAndReadyDate. */
    function update(btn, rows) {
        return S.busy(btn, function () {
            if (rows.length === 0) throw new Error('Grid Record not found...');
            if (!ask('Are you sure to Update?')) return;
            var changed = rows.filter(function (r) { return netI(r.DocumentStatus) !== netI(r.PreviousDocumentStatus) || S.isoDate(r.ReadyDate) !== S.isoDate(r.PreviousReadyDate); });
            if (changed.length === 0) throw new Error('No record Found For Updation');
            return S.postJson(API + '/update', { rows: changed }).then(function (d) { box(d.message); return show($id('btnshow')); });
        });
    }
    function updateI(btn) { return update(btn, G2); }    /* BtnUpdateTemplateI -> Insert(grdII) */
    function updateII(btn) { return update(btn, G1); }   /* BtnUpdateTemplateII -> Insert(grd) */
    /** BtnUpdateInGrid_Click -> UpdateGrid(grid). */
    function updateInGrid() {
        if (!PERM.Save) { box("You don't have the right to update the record."); return; }
        var view = S.radio('gridView');
        var rows = view === 'I' ? G2 : view === 'II' ? G1 : null;
        if (!rows || rows.length <= 0) return;
        var readyDate = S.checked('txtReadyDateUpdateInGridChk') ? S.val('txtReadyDateUpdateInGrid') : '';
        var status = netI(S.val('CmbDocmentStausToUpdate'));
        var all = S.radio('updateScope') === 'all';
        rows.forEach(function (r) {
            if (all || !S.isoDate(r.ReadyDate)) r.ReadyDate = readyDate;
            if (status > 0 && (all || netI(r.DocumentStatus) === 0)) r.DocumentStatus = status;
        });
        var c1 = g1Cols(), c2 = g2Cols();
        S.drawGrid('g1Body', null, G1, c1, { group: 'Description', groupTitle: 'Description' });
        S.drawGrid('g2Body', null, G2, c2, { group: 'Description', groupTitle: 'Description' });
    }
    function reset() { S.focus('GRNfromdate'); }
    function tabChanged(id) {
        $id('btnFooterHistory').querySelector('span').textContent = id === 'tabPage2' ? 'Detail' : 'Group Wise';
        show($id('btnshow'));
    }
    function toggleHistory() { S.selectTab('main', S.activeTab('main') === 'tabPage2' ? 'tabPage1' : 'tabPage2', tabChanged); }
    function shortcuts() {
        S.shortcutKeys([['Ctrl+S', 'For Shoe Record'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Alt+1', 'For Print 841'], ['Alt+2', 'For Print 841A'],
            ['Ctrl+F5', 'For Focus on Combo Date'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid History'],
            ['Ctrl+ArrowUp', 'For Focus On Date combo in Filters Box'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        S.wireFullscreen();
        S.tabs('main', tabChanged);
        S.wireGrid('g1Body', { cell: function (i, k, v) { cellChanged(G1)(i, k, v); } });
        S.wireGrid('g2Body', { cell: function (i, k, v) { cellChanged(G2)(i, k, v); } });
        S.wireGrid('detBody', {});
        var on = function (id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); };
        on('cmbperemeter', 'change', parameterChanged);
        ['ChkFirstLevel', 'ChkSecondLevel', 'ChkThirdLevel'].forEach(function (id) { on(id, 'change', renderAll); });   /* FillGrids(Value:false) */
        document.addEventListener('keydown', function (e) {
            if (S.enterMovesOn(e)) return;
            var k = (e.key || '').toLowerCase();
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); S.cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnshow')); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); return; }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); return; }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); S.focus('cmbperemeter'); return; }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); $id('grd').scrollIntoView(); }
            /* Ctrl+P, Alt+1, Alt+2: BtnPrint_Click / btn335Register_Click are empty on the desktop */
        });
        load();
    });

    global.ExportDocTrackingReport = { reset: reset, refresh: refresh, show: show, updateI: updateI, updateII: updateII, updateInGrid: updateInGrid, toggleHistory: toggleHistory, shortcuts: shortcuts };
}(window));
