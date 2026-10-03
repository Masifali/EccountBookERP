/* ============================================================================================
 * countx_export_generate_contract_nos.js - frmGenerateExportContractNos.cs (Architecture.WinApp.Export)
 * "Generate Export Contract Nos" (DocumentTypeId 223, pop-up of 879 Contract III) and its DefineExportPrefixType
 * dialog. Data from /api/export/generate-contract-nos.
 *
 * Desktop behaviour kept: Save always generates a new batch (MainId from GetMainIdForGenratingExportInvoiceNos;
 * the DAL first runs USP_ExportInvoiceNosValidateAndDelete with MainId 0); Update deletes the opened batch and
 * re-creates it (USP_ExportInvoiceNosValidateAndDelete refuses when a number is used on an Export Invoice);
 * "SerialFrom/Serial To Field Is Required" only for an empty box; the prefix type is not validated; New
 * keeps the Prefix Type combo; the history keeps the first row per MainId and a row click fills the detail
 * grid. ?mainId= selects that batch.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var Q = global.ExportQ;
    var API = '/api/export/generate-contract-nos';
    var box = Q.box, netI = Q.netI, str = Q.str;

    var S = { prefixTypes: [], history: [], rows: [], cur: -1, mainId: 0, recId: 0 };
    var PX = { rows: [], recId: 0 };
    var HIST_COLS = [{ key: 'PrefixType' }, { key: 'Prefix' }, { key: 'ContractNo', link: true }, { key: 'EntryDate', fmt: 'date' }, { key: 'EntryUser' }];
    var DET_COLS = [{ key: 'Prefix' }, { key: 'SerialFrom', fmt: 'int' }, { key: 'SerialTo', fmt: 'int' }, { key: 'ContractNo' }];

    /* BindPrefixType: DDL.BindDDL(Id, PrefixDescription); no rows -> Text "" and no source. */
    function bindPrefixTypes() { Q.bind('CmbPrefixType', S.prefixTypes, 'Id', 'PrefixDescription', []); }

    function renderHistory() {
        Q.drawGrid('histBody', null, S.history, HIST_COLS, S.cur, function (r, i) {
            return '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="edit" data-i="' + i + '">Edit</button></td>';
        });
        Q.show('histEmpty', S.history.length === 0);
    }
    /* BindHistory: no rows -> both grids cleared. */
    function applyHistory(d) {
        if (d.historyError) { box(d.historyError); return; }
        S.history = d.history || []; S.rows = d.rows || []; S.cur = -1;
        renderHistory();
        Q.drawGrid('detBody', null, [], DET_COLS, -1);
    }
    function bindHistory() {
        return Q.getJson(API + '/history').then(function (d) { applyHistory(d || {}); }).catch(function (e) { box(e.message); });
    }

    /* frmGenerateExportContractNos_Load. */
    function load() {
        return Q.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.prefixTypesError) box(d.prefixTypesError);
            S.prefixTypes = d.prefixTypes || []; bindPrefixTypes();
            applyHistory(d);
            modeSave();
            var mainId = netI(Q.qs('mainId'));
            if (mainId > 0) { for (var i = 0; i < S.history.length; i++) if (netI(S.history[i].MainId) === mainId) { selectRow(i); break; } }
            Q.focus('CmbPrefixType');
        }).catch(function (e) { box(e.message); });
    }

    function modeSave() { Q.show('btnUpdate', false); Q.show('btnsave', true); Q.show('btnUpdateF', false); Q.show('BtnSavef', true); }
    function modeUpdate() { Q.show('btnUpdate', true); Q.show('btnsave', false); Q.show('btnUpdateF', true); Q.show('BtnSavef', false); }

    /* Reset (btnnew_Click): buttons back, Prefix / Serial boxes cleared, history re-read (the combo is kept). */
    function reset(btn) {
        var run = function () {
            modeSave();
            Q.setText('txtPrefix', ''); Q.setText('txtSerialFrom', ''); Q.setText('txtSerialTo', '');
            return bindHistory();
        };
        return btn ? Q.busy(btn, run) : run();
    }

    /* FormValiadation. */
    function validate() {
        var p = Q.val('txtPrefix'), f = Q.val('txtSerialFrom'), t = Q.val('txtSerialTo');
        if (p === '') { box('PrefixField Is Required'); Q.focus('txtPrefix'); return false; }
        if (f === '' && netI(f) === 0) { box('SerialFrom Field Is Required'); Q.focus('txtSerialFrom'); return false; }
        if (t === '' && netI(t) === 0) { box('Serial To Field Is Required'); Q.focus('txtSerialTo'); return false; }
        if (netI(t) < netI(f)) { box('Serial To Can not Be Less than Serial From'); Q.focus('txtSerialTo'); return false; }
        return true;
    }
    function body(update) {
        return { update: update, mainId: S.mainId, prefixTypeId: netI(Q.val('CmbPrefixType')), prefix: Q.val('txtPrefix'),
            serialFrom: Q.val('txtSerialFrom'), serialTo: Q.val('txtSerialTo') };
    }
    /* btnsave_Click -> RecId = 0, Insert(). */
    function save(btn) {
        S.recId = 0;
        return Q.busy(btn, function () {
            if (!validate()) return Promise.resolve();
            if (!Q.ask('Are you sure to Save?')) return Promise.resolve();
            return Q.postJson(API + '/save', body(false)).then(function (r) {
                if (r && r.message) box(r.message);
                return reset();
            });
        });
    }
    /* btnUpdate_Click -> DeleteAndInsert(). */
    function update(btn) {
        return Q.busy(btn, function () {
            if (!validate()) return Promise.resolve();
            if (!Q.ask('Are you sure to Update?')) return Promise.resolve();
            return Q.postJson(API + '/save', body(true)).then(function (r) {
                box((r && r.message) || 'Update Successfully');
                return reset();
            });
        });
    }
    /* ReadById: from the history's current row. */
    function readById(i) {
        var r = S.history[i]; if (!r) return;
        S.recId = netI(r.Id);
        Q.setVal('CmbPrefixType', str(netI(r.PrefixTypeId)));
        S.mainId = netI(r.MainId);
        Q.setText('txtPrefix', str(r.Prefix));
        Q.setText('txtSerialFrom', str(r.SerialFrom));
        Q.setText('txtSerialTo', str(r.SerialTo));
        modeUpdate();
    }
    /* Grd_SelectionChanged: the dtHistory rows of the selected MainId. */
    function selectRow(i) {
        S.cur = i;
        var r = S.history[i]; if (!r) return;
        var det = S.rows.filter(function (x) { return netI(x.MainId) === netI(r.MainId); });
        Q.drawGrid('detBody', null, det, DET_COLS, -1);
        Array.prototype.forEach.call(Q.$id('histBody').querySelectorAll('tr[data-i]'), function (tr) { tr.classList.toggle('is-current', +tr.getAttribute('data-i') === i); });
    }
    function close() { Q.cancel(); }
    function history(btn) {
        return Q.busy(btn, function () {
            return bindHistory().then(function () { var b = Q.$id('historyBar'); if (b) b.scrollIntoView({ behavior: 'smooth' }); });
        });
    }

    // ------------------------------------------------------------------ DefineExportPrefixType

    function pxRender() { Q.drawGrid('pxBody', null, PX.rows, [{ key: 'Description' }], -1); }
    /* GridFill. */
    function pxFill() {
        return Q.getJson(API + '/prefix-types').then(function (rows) { PX.rows = rows || []; pxRender(); }).catch(function (e) { box(e.message); });
    }
    /* btnDefinePrefixType_Click: ShowDialog, then BindPrefixType when it closes. */
    function definePrefixType(btn) {
        return Q.busy(btn, function () {
            PX.recId = 0; Q.setText('txtPrefixDescription', '');
            Q.show('pxBtnSave', true); Q.show('pxBtnUpdate', false);
            Q.modal('prefixModal', true);
            Q.focus('txtPrefixDescription');
            return pxFill();
        });
    }
    function closePrefixType() {
        Q.modal('prefixModal', false);
        return Q.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.prefixTypesError) { box(d.prefixTypesError); return; }
            S.prefixTypes = d.prefixTypes || []; bindPrefixTypes();
        }).catch(function (e) { box(e.message); });
    }
    function pxInsert(btn) {
        return Q.busy(btn, function () {
            if (!Q.val('txtPrefixDescription').trim()) { box('Description Field Required'); Q.focus('txtPrefixDescription'); return Promise.resolve(); }
            if (!Q.ask(PX.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            return Q.postJson(API + '/save-prefix-type', { update: PX.recId > 0, recId: PX.recId, description: Q.val('txtPrefixDescription') }).then(function (r) {
                box((r && r.message) || '');
                Q.setText('txtPrefixDescription', '');   /* Reset(): only the text - RecId and the buttons stay */
                return pxFill();
            });
        });
    }
    /* btnsave_Click: RecId = 0, Insert(). */
    function pxSave(btn) { PX.recId = 0; return pxInsert(btn); }
    /* btnUpdate_Click: RecId == 0 -> "RecId not Found". */
    function pxUpdate(btn) { if (PX.recId === 0) { box('RecId not Found'); return Promise.resolve(); } return pxInsert(btn); }
    /* btnnew_Click. */
    function pxNew() { PX.recId = 0; Q.setText('txtPrefixDescription', ''); Q.show('pxBtnSave', true); Q.show('pxBtnUpdate', false); }
    /* grdcountrydefine_DoubleClick. */
    function pxEdit(i) {
        var r = PX.rows[i]; if (!r) return;
        PX.recId = netI(r.Id);
        Q.setText('txtPrefixDescription', str(r.Description));
        Q.show('pxBtnSave', false); Q.show('pxBtnUpdate', true);
    }

    document.addEventListener('DOMContentLoaded', function () {
        Q.wireTabs(null, null);
        Q.wireGrid('histBody', {
            select: selectRow, open: readById,
            btn: function (name, i) { if (name === 'edit') readById(i); },
            link: function (k, i) { readById(i); }
        });
        Q.wireGrid('pxBody', { open: pxEdit, link: function (k, i) { pxEdit(i); } });
        /* DefineCountry_KeyDown while the dialog is open. */
        document.addEventListener('keydown', function (e) {
            if (!Q.$id('prefixModal').classList.contains('is-open')) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); pxNew(); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); pxSave(Q.$id('pxBtnSave')); }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); pxUpdate(Q.$id('pxBtnUpdate')); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); closePrefixType(); }
        });
        load();
    });

    global.ExportGenerateContractNos = {
        reset: reset, save: save, update: update, close: close, history: history, definePrefixType: definePrefixType,
        closePrefixType: closePrefixType, pxSave: pxSave, pxUpdate: pxUpdate, pxNew: pxNew
    };
}(window));
