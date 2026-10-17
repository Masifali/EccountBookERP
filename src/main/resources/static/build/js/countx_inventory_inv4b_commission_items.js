/* CommissionItems (WholeSale.Commission/CommissionItems.cs) - ScreenDefinition 94 "Commission Items".
   Line refs ":NNN" are that file. API: /inventory/commission-items/api.
     InvDeffrmWarehouse_Load :299  rights, BindHistory, btnupdate hidden, txtcode focused
     Insert :212                   FormValiadation → confirm → Save/Update → clear() + BindHistory()
     ReadById :273                 Edit button / double-click → fields, Update mode
     clear :370                    txtcode / txtDescription cleared (ChkIsActive is NOT touched), Save mode */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv4b, $id = SC.$id, API = '/inventory/commission-items/api';
    var recId = 0, updateMood = false, rights = {};

    /* grdSettings :138 — Edit at position 0, Id hidden, "Commission Name" caption, EntryDate ToShortDateString (:125) */
    var grid = new X.Grid('grdfrm', [
        { key: 'Edit', cap: 'Edit', btn: 'Edit', width: 40 },
        { key: 'Id', hidden: true },
        { key: 'CommissionItemName', cap: 'Commission Name', width: 100 },
        { key: 'Percentage', cap: 'Percentage', width: 80, n: true, fmt: function (v) { return X.fmt(v, 3); } },
        { key: 'EntryUser', cap: 'EntryUser', width: 100 },
        { key: 'EntryDate', cap: 'EntryDate', width: 80, fmt: function (v) { return X.dmy(v); } },
        { key: 'IsActive', cap: 'IsActive' }
    ], { nav: 'lblNavigator', onEdit: function (r) { readById(r.Id); }, onDblClick: function (r) { readById(r.Id); } });

    /* MakeShortCutKeys :444 */
    var SHORTCUTS = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'],
        ['Ctrl+F5', 'For Focus on Discount Item Name'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On Discount Item Name']];

    function setMode(update) {
        updateMood = update;
        $id('btnsave').classList.toggle('is-hidden', update);
        $id('BtnSavef').classList.toggle('is-hidden', update);
        $id('btnupdate').classList.toggle('is-hidden', !update);
        $id('btnUpdateF').classList.toggle('is-hidden', !update);
    }

    /* BindHistory :100 — when the procedure returns no rows the grid is left as it was (:113) */
    function bindHistory() {
        return SC.getJson(API + '/history').then(function (rows) { if (rows && rows.length) grid.set(rows); }).catch(function (e) { alert(e.message); });
    }

    /* clear :370 */
    function clear() {
        $id('txtcode').value = '';
        $id('txtDescription').value = '';
        setMode(false);
        $id('txtcode').focus();
    }

    function body() {
        return { Id: recId, CommissionItemName: $id('txtcode').value, CommissionPerncentage: $id('txtDescription').value, IsActive: $id('ChkIsActive').checked };
    }
    /* Insert :212 */
    function insert() {
        X.insert(API, body(), function () { clear(); bindHistory(); });
    }
    function btnsave_Click() { recId = 0; insert(); }            // :187
    function btnupdate_Click() { insert(); }                       // :200

    /* ReadById :273 */
    function readById(id) {
        SC.getJson(API + '/' + id).then(function (c) {
            recId = c.Id;
            $id('txtcode').value = c.CommissionItemName || '';
            $id('txtDescription').value = c.CommissionPerncentage == null ? '' : String(c.CommissionPerncentage);
            $id('ChkIsActive').checked = !!c.IsActive;
            bindHistory();                                          // :285
            setMode(true);
        }).catch(function (e) { alert(e.message); });
    }

    /* btnnew_Click :344 */
    function btnnew_Click() { clear(); bindHistory(); }

    /* InvDeffrmWarehouse_Load :299 */
    function load() {
        SC.getJson(API + '/init').then(function (d) {
            rights = d.rights || {};
            $id('btnsave').disabled = !rights.save;
            $id('BtnSavef').disabled = !rights.save;
            $id('btnupdate').disabled = !rights.update;
            $id('btnUpdateF').disabled = !rights.update;
            if (d.history && d.history.length) grid.set(d.history);
            setMode(false);
            $id('txtcode').focus();
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnnew').addEventListener('click', btnnew_Click);
        $id('btnsave').addEventListener('click', btnsave_Click);
        $id('BtnSavef').addEventListener('click', btnsave_Click);
        $id('btnupdate').addEventListener('click', btnupdate_Click);
        $id('btnUpdateF').addEventListener('click', btnupdate_Click);
        $id('BtnCancel').addEventListener('click', X.closeForm);                       // BtnCancel_Click :339 → Close()
        $id('btnshortcut').addEventListener('click', function () { X.showShortcuts(SHORTCUTS); });
        X.decimalOnly($id('txtDescription'));                                            // txtDescription_KeyPress :481
        document.addEventListener('keydown', function (e) {                              // InvDeffrmWarehouse_KeyDown :401
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey) { e.preventDefault(); X.showShortcuts(SHORTCUTS); return; }
            if (e.ctrlKey && k === 's' && !updateMood) { e.preventDefault(); if (!$id('btnsave').disabled) btnsave_Click(); }
            else if (e.ctrlKey && k === 'u' && updateMood) { e.preventDefault(); if (!$id('btnupdate').disabled) btnupdate_Click(); }
            else if (e.ctrlKey && (k === 'f5' || k === 'arrowup')) { e.preventDefault(); $id('txtcode').focus(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); grid.focus(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnnew_Click(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); if (!X.closeShortcuts()) X.closeForm(); }
        });
        load();
    });
})();
