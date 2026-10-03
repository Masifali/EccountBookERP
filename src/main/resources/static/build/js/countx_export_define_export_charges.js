/* ============================================================================================
 * countx_export_define_export_charges.js - DefineExportCharges.cs (Architecture.WinApp.Export) "Define Export
 * Charges" (pop-up of 951 / 209 / 879 / 211 / 880). Every button, double-click and KeyDown of the desktop form,
 * with its messages and order; data from /api/export/define-export-charges.
 *
 * Desktop behaviour kept: the toolbar Save always inserts (btnSave_Click sets RecId = 0) even with a record
 * open; Update and the Save/Update button use the open record; validation runs before the confirmation;
 * duplicates are checked against the history grid (exact text); after a save the form resets and the grid
 * re-reads; an empty history empties the grid. ?id= opens that charge.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var Q = global.ExportQ;
    var API = '/api/export/define-export-charges';
    var box = Q.box, netI = Q.netI, str = Q.str;

    var S = { history: [], accounts: [], recId: 0, cur: -1 };
    var COLS = [{ key: 'ChargesCode', link: true }, { key: 'ChargesName' }, { key: 'AccountTitle' }, { key: 'EntryUser' },
        { key: 'EntryDate', fmt: 'datetime', mmm: false }, { key: 'ModifyUser' }, { key: 'ModifyDate', fmt: 'datetime', mmm: false }];

    function render() {
        Q.drawGrid('histBody', null, S.history, COLS, S.cur);
        Q.show('histEmpty', S.history.length === 0);
    }
    /* ExpensesAcBind: BindAndRetainSelection, all columns. */
    function bindAccounts() { Q.bind('CmbExpenseAccount', S.accounts, 'Id', 'AccountTitle', ['AccountCode', 'ParentAccountTitle', 'AccountClass']); }
    function modeSave() { Q.show('btnSave', true); Q.show('btnupdate', false); Q.$id('btnAdd').textContent = 'Save'; }
    function modeUpdate() { Q.show('btnSave', false); Q.show('btnupdate', true); Q.$id('btnAdd').textContent = 'Update'; }

    /* InitializeComponentMethod. */
    function load() {
        return Q.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.historyError || d.accountsError) { box('Error occurred during database call.'); }
            S.history = d.history || []; S.cur = -1; render();
            S.accounts = d.accounts || []; bindAccounts();
            modeSave();
            var id = netI(Q.qs('id'));
            if (id > 0) { for (var i = 0; i < S.history.length; i++) if (netI(S.history[i].Id) === id) { S.cur = i; render(); readById(i); break; } }
            else Q.focus('txtbrandCode');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }

    /* GridEX_Helper.IsAnyCellValueHaveSameValueExcept(grdfrm, col, value.Trim(), "Id", RecId). */
    function dup(col, value) {
        for (var i = 0; i < S.history.length; i++) {
            var r = S.history[i];
            if (netI(r.Id) === S.recId) continue;
            if (r[col] !== null && r[col] !== undefined && str(r[col]) === value) return true;
        }
        return false;
    }
    /* ValidateInputs. */
    function validate() {
        if (!Q.val('txtbrandCode').trim()) { box('Charges Code is required'); Q.focus('txtbrandCode'); return false; }
        if (!Q.val('txtBrandName').trim()) { box('Charges Name is required'); Q.focus('txtBrandName'); return false; }
        if (!Q.hasSel('CmbExpenseAccount')) { box('Account Title is required'); Q.focus('CmbExpenseAccount'); return false; }
        if (dup('ChargesCode', Q.val('txtbrandCode').trim())) { box('This Charges Code already exists.'); Q.focus('txtbrandCode'); return false; }
        if (dup('ChargesName', Q.val('txtBrandName').trim())) { box('This Charges Name already exists.'); Q.focus('txtBrandName'); return false; }
        return true;
    }
    /* Insert(). */
    function insert(btn) {
        return Q.busy(btn, function () {
            if (!validate()) return Promise.resolve();
            if (!Q.ask(S.recId > 0 ? 'Are you sure you want to update?' : 'Are you sure you want to save?')) return Promise.resolve();
            return Q.postJson(API + '/save', { recId: S.recId, code: Q.val('txtbrandCode'), name: Q.val('txtBrandName'), accountId: netI(Q.val('CmbExpenseAccount')) })
                .then(function (r) {
                    box((r && r.message) || '');
                    return resetForm().then(reloadHistory);
                });
        });
    }
    function reloadHistory() {
        return Q.getJson(API + '/history').then(function (d) {
            if (d && d.historyError) { box(d.historyError); return; }
            S.history = (d && d.history) || []; S.cur = -1; render();
        }).catch(function (e) { box(e.message); });
    }
    /* btnSave_Click: RecId = 0, Insert(). */
    function toolSave(btn) { S.recId = 0; return insert(btn); }
    /* btnupdate_Click / btnAdd_Click: Insert(). */
    function toolUpdate(btn) { return insert(btn); }
    function add(btn) { return insert(btn); }

    /* ReadById: from the grid's current row. */
    function readById(i) {
        var r = S.history[i]; if (!r) return;
        S.recId = netI(r.Id);
        Q.setText('txtbrandCode', str(r.ChargesCode));
        Q.setText('txtBrandName', str(r.ChargesName));
        if (netI(r.AccountId) > 0) Q.setValOrAdd('CmbExpenseAccount', r.AccountId, r.AccountTitle);
        modeUpdate();
        Q.focus('txtbrandCode');
    }
    /* ResetForm: fields cleared, buttons back, accounts re-read (GlobalServicesDbCall + ExpensesAcBind). */
    function resetForm(btn) {
        var run = function () {
            S.recId = 0;
            Q.setText('txtbrandCode', ''); Q.setText('txtBrandName', ''); Q.setVal('CmbExpenseAccount', '0');
            modeSave();
            Q.focus('txtbrandCode');
            return Q.getJson(API + '/accounts').then(function (d) {
                if (d && d.accountsError) { box(d.accountsError); return; }
                S.accounts = (d && d.accounts) || []; bindAccounts();
            }).catch(function (e) { box(e.message); });
        };
        return btn ? Q.busy(btn, run) : run();
    }
    /* ShowShortcutKeys. */
    function shortcuts() {
        Q.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+F5', 'For Focus on ChargesCode'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+ArrowUp', 'For Focus On ChargesCode'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record']]);
    }
    /* Footer History: re-read the history grid and bring it into view. */
    function history(btn) {
        return Q.busy(btn, function () {
            return reloadHistory().then(function () { var b = Q.$id('historyBar'); if (b) b.scrollIntoView({ behavior: 'smooth' }); });
        });
    }

    document.addEventListener('DOMContentLoaded', function () {
        Q.wireTabs(null, null);
        Q.wireGrid('histBody', { select: function (i) { S.cur = i; }, open: readById });
        Q.$id('grdfrm').addEventListener('keydown', function (e) {
            if (e.ctrlKey && e.key === 'Enter' && S.cur >= 0) { e.preventDefault(); e.stopPropagation(); readById(S.cur); }
        });
        /* DefineExportCharges_KeyDown (else-if chain). */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey) { if (k === 'control' || k === 'alt') { shortcuts(); } return; }
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (!Q.$id('btnSave').classList.contains('is-hidden')) toolSave(Q.$id('btnSave')); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (!Q.$id('btnupdate').classList.contains('is-hidden')) toolUpdate(Q.$id('btnupdate')); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); resetForm(); }
            else if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); Q.cancel(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); Q.focus('txtbrandCode'); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); Q.$id('grdfrm').focus(); }
        });
        load();
    });

    global.ExportDefineCharges = { resetForm: resetForm, toolSave: toolSave, toolUpdate: toolUpdate, add: add, shortcuts: shortcuts, history: history };
}(window));
