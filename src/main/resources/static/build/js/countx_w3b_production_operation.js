/* ============================================================================================
 * Screen 770 "Production Operation" - Architecture.WinApp.Mfg.ProductionOperation (base.Name "ProductionOperation").
 * Line numbers are the .cs: _Load:64, Reset:92, FormValidation:134, Insert():145, btnsave_Click:189, btnUpdate_Click:202,
 * BtnSaveII_Click:218, FormHistory:237, grdDetail_DoubleClick:391, KeyDown:311, MakeShortCutKeys:354.
 * API /production/mfg/operation/api.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/mfg/operation/api';
    var $ = W3B.$;
    var recId = 0, rows = [];

    function setMode(update) {
        recId = update ? recId : 0;
        W3B.show('btnsave', !update); W3B.show('btnUpdate', update);
        $('BtnSaveII').textContent = update ? 'Update' : 'Save';
    }

    function history() {
        return W3B.getJson(api + '/history').then(function (d) {
            rows = d || [];
            $('grdDetail').innerHTML = W3B.tableHtml([
                { h: 'SerialNo', k: 'SerialNo', cls: 'num', w: 60 },
                { h: 'OperationName', k: 'OperationName', w: 200 }
            ], rows);
        }).catch(W3B.fail);
    }

    function reset() {                                         // Reset:92
        recId = 0; setMode(false);
        $('txtOperationName').value = '';
        W3B.focus('txtOperationName');
        return history();
    }

    function validate() {                                      // FormValidation:134
        if ($('txtOperationName').value === '') { W3B.box('Operation Name Field is Required'); W3B.focus('txtOperationName'); return false; }
        return true;
    }

    function insert() {                                        // Insert():145
        if (!validate()) return;
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        W3B.postJson(api + '/save', { id: recId, operationName: $('txtOperationName').value }).then(function (r) {
            W3B.box(r.message);
            return reset();
        }).catch(W3B.fail);
    }

    function save() { recId = 0; insert(); }                   // btnsave_Click:189
    function update() {                                        // btnUpdate_Click:202
        if (recId === 0) { W3B.box('RecId not found'); return; }
        insert();
    }
    function saveII() { if ($('BtnSaveII').textContent === 'Save') save(); else update(); }   // BtnSaveII_Click:218

    function edit(i) {                                         // grdDetail_DoubleClick:391
        var r = rows[i]; if (!r) return;
        W3B.getJson(api + '/' + r.Id).then(function (d) {
            recId = d.Id; setMode(true);
            $('txtOperationName').value = d.OperationName;
            W3B.focus('txtOperationName');
        }).catch(W3B.fail);
    }

    function shortcutKeys() {                                  // MakeShortCutKeys:354
        W3B.box(['Ctrl+S   For Save', 'Ctrl+U   For Update', 'Ctrl+E   For Close', 'Ctrl+N   For New',
            'Ctrl+F5   For Focus On Operation Name', 'Ctrl+alt   To Show ShortCut Keys Form',
            'Ctrl+ArrowDown   For Focus On Grid', 'Ctrl+ArrowUp   For Focus On Operation Name'].join('\n'));
    }

    window.Op = { reset: reset, save: save, update: update, saveII: saveII, shortcutKeys: shortcutKeys };

    document.addEventListener('DOMContentLoaded', function () {
        $('grdDetail').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) edit(+tr.getAttribute('data-i')); });
        W3B.keys({
            n: reset,
            s: function () { if (!$('btnsave').classList.contains('is-hidden')) save(); },
            u: function () { if (!$('btnUpdate').classList.contains('is-hidden')) update(); },
            e: function () { window.location.href = '/production'; },
            escape: function () { window.location.href = '/production'; },
            f5: function () { W3B.focus('txtOperationName'); },
            arrowup: function () { W3B.focus('txtOperationName'); },
            arrowdown: function () { $('grdDetailHost').scrollIntoView(); }
        });
        setMode(false);                                        // _Load:64 btnsave visible, btnUpdate hidden
        W3B.focus('txtOperationName');
        history();
    });
}());
