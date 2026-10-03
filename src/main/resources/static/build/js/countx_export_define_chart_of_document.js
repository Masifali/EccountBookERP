/* ============================================================================================
 * countx_export_define_chart_of_document.js - DefineChartOfDocument.cs (Architecture.WinApp.SDT), screen 620
 * "Define Chart Of Document". Data: /api/export/define-chart-of-document.
 * Desktop quirk kept: DateLock_Load disables btnSave / btnAdd / btnupdate unconditionally, so the entry
 * fields load a record (grid double-click) but nothing can be saved; the save call stays wired for parity.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var S = global.ExportSdt;
    var API = '/api/export/define-chart-of-document';
    var $id = S.$id, box = S.box, ask = S.ask, netI = S.netI, str = S.str;

    var RecId = 0, HIST = [], CUR = -1;

    function load() {
        return S.getJson(API + '/setup').then(function (d) {
            d = d || {};
            S.bind('CmbRequiredLevel', d.requiredLevels || [], 'Id', 'name', []);
            S.bind('CmbProvider', d.documentProviders || [], 'Id', 'name', []);
            HIST = d.history || []; render();
            /* DateLock_Load */
            $id('btnSave').disabled = true; $id('btnAdd').disabled = true; $id('btnupdate').disabled = true;
            S.show('btnSave', true); S.show('btnupdate', false);
            $id('btnAdd').textContent = 'Save';
        }).catch(function (e) { box(e.message); });
    }
    /** refreshToolStripMenuItem_Click: RequiredLevelFill + DocumentProviderFill (selection retained). */
    function refresh(btn) {
        return S.busy(btn, function () {
            return S.getJson(API + '/combos').then(function (d) {
                S.bind('CmbRequiredLevel', d.requiredLevels || [], 'Id', 'name', []);
                S.bind('CmbProvider', d.documentProviders || [], 'Id', 'name', []);
            });
        });
    }
    var COLS = [
        { key: 'DocumentCode' }, { key: 'DocumentName', link: true }, { key: 'SequenceNo', kind: 'int' }, { key: 'RequiredLevel' }, { key: 'LeadTime', kind: 'int' },
        { key: 'ProviderName' }, { key: 'EntryUser' }, { key: 'EntryDate', kind: 'dt' }, { key: 'ModifyUser' }, { key: 'ModifyDate', kind: 'dt' }
    ];
    function render() { S.drawGrid('histBody', null, HIST, COLS, { cur: CUR }); S.show('histEmpty', HIST.length === 0); }
    function bindGrid() { return S.getJson(API + '/history').then(function (rows) { HIST = rows || []; CUR = -1; render(); }); }

    /** grdfrm_DoubleClick -> ReadById. */
    function readById(i) {
        var r = HIST[i]; if (!r) return;
        S.getJson(API + '/by-id?id=' + netI(r.Id)).then(function (d) {
            RecId = netI(d.Id);
            S.setText('txtChartOfDocumentCode', d.documentCode);
            S.setText('txtChartOfDocumentName', d.documentName);
            S.setVal('CmbRequiredLevel', d.requiredLevelId);
            S.setVal('CmbProvider', d.DocumentProviderId);
            S.setText('txtSeqNo', str(d.seqNo));
            S.setText('txtLeadTime', str(d.leadTime));
            S.show('btnSave', false); S.show('btnupdate', true);
            $id('btnAdd').textContent = 'Update';
        }).catch(function (e) { box(e.message); });
    }
    /** Insert(): validations in the desktop's order, confirm, save. */
    function insert(btn) {
        return S.busy(btn, function () {
            if (!S.val('txtChartOfDocumentCode')) { box('ChartOfDocument Code Field is Required'); S.focus('txtChartOfDocumentCode'); return; }
            if (!S.val('txtChartOfDocumentName')) { box('ChartOfDocument Name Field is Required'); S.focus('txtChartOfDocumentName'); return; }
            if (!S.hasSel('CmbRequiredLevel')) { box('Type Field is Required'); S.focus('CmbRequiredLevel'); return; }
            if (netI(S.val('txtSeqNo')) === 0) { box('Sequence No Field is Required'); S.focus('txtSeqNo'); return; }
            if (netI(S.val('txtLeadTime')) === 0) { box('LeadTime No Field is Required'); S.focus('txtSeqNo'); return; }
            if (!ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            return S.postJson(API + '/save', {
                recId: RecId, documentCode: S.val('txtChartOfDocumentCode').trim(), documentName: S.val('txtChartOfDocumentName').trim(),
                requiredLevelId: netI(S.val('CmbRequiredLevel')), DocumentProviderId: netI(S.val('CmbProvider')),
                seqNo: netI(S.val('txtSeqNo')), leadTime: netI(S.val('txtLeadTime'))
            }).then(function (d) { box(d.message); reset(); return bindGrid(); });
        });
    }
    function save(btn, isUpdate) {
        if (isUpdate) { if (RecId === 0) { box('RecId not found...'); return; } }
        else RecId = 0;
        return insert(btn);
    }
    function add(btn) { return insert(btn); }
    /** Reset(): the desktop clears only the code and the mode. */
    function reset() {
        RecId = 0;
        S.setText('txtChartOfDocumentCode', '');
        S.show('btnSave', true); S.show('btnupdate', false);
        $id('btnAdd').textContent = 'Save';
    }
    function toggleHistory() { $id('grdfrm').scrollIntoView({ behavior: 'smooth' }); }
    function shortcuts() {
        S.shortcutKeys([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+F5', 'For Focus on Description'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+ArrowUp', 'For Focus On Description'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record']]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        S.wireFullscreen();
        S.wireGrid('histBody', { select: function (i) { CUR = i; }, open: readById });
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); S.cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); return; }
            if (e.ctrlKey && k === 's' && !$id('btnSave').classList.contains('is-hidden') && !$id('btnSave').disabled) { e.preventDefault(); save($id('btnSave'), false); return; }
            if (e.ctrlKey && k === 'u' && !$id('btnupdate').classList.contains('is-hidden') && !$id('btnupdate').disabled) { e.preventDefault(); save($id('btnupdate'), true); return; }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); $id('grdfrm').scrollIntoView(); return; }
            if (e.ctrlKey && (e.key === 'ArrowUp' || e.key === 'F5')) { e.preventDefault(); S.focus('txtChartOfDocumentCode'); return; }
            if (e.ctrlKey && e.key === 'Enter' && CUR >= 0) { e.preventDefault(); readById(CUR); }
        });
        load();
    });

    global.ExportChartOfDocument = { reset: reset, refresh: refresh, save: save, add: add, toggleHistory: toggleHistory, shortcuts: shortcuts };
}(window));
