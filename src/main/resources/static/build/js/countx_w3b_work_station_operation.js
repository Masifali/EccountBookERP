/* ============================================================================================
 * Screen 595 "Work Station Operation" - Architecture.WinApp.Mfg.WorkStationOperation (base.Name "WorkStationOperation").
 * Line numbers are the .cs: _Load:87, OeraptionNamefill:120, StationNamefill:159, Reset:198, btnRefresh_Click:229, FormValidation:254,
 * Insert():271, FormHistory:365, CreateGroupedDataTable:398, GridSetting:418, grdDetail_DoubleClick:438, grdII_DoubleClick:457,
 * KeyDown:476, MakeShortCutKeys:537, BtnOperation_Click:595. API /production/mfg/work-station-operation/api.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/mfg/work-station-operation/api';
    var $ = W3B.$;
    var recId = 0, hist = { byWorkStation: [], byOperation: [] };

    function setMode(update) {
        W3B.show('btnsave', !update); W3B.show('btnUpdate', update);
        $('BtnSaveII').textContent = update ? 'Update' : 'Save';
    }

    /** GridSetting:418 - grouped by the named column (hidden), columns SerialNo / WorkStationName / OperationName / OpHoursPerDay. */
    function renderGrid(tableId, rows, groupCol) {
        var cols = [{ h: 'SerialNo', k: 'SerialNo', cls: 'num', w: 60 }, { h: 'WorkStationName', k: 'WorkStationName', w: 180 },
            { h: 'OperationName', k: 'OperationName', w: 180 }, { h: 'OpHoursPerDay', f: function (r) { return W3B.fmt(r.OpHoursPerDay, 4); }, cls: 'num', w: 70 }]
            .filter(function (c) { return c.k !== groupCol; });
        var h = '<thead><tr>' + cols.map(function (c) { return '<th style="width:' + c.w + 'px">' + c.h + '</th>'; }).join('') + '</tr></thead><tbody>';
        if (!rows.length) h += '<tr class="rc-empty"><td colspan="' + cols.length + '">No records</td></tr>';
        var last = null;
        rows.forEach(function (r, i) {
            if (r[groupCol] !== last) { last = r[groupCol]; h += '<tr class="w3-group"><td colspan="' + cols.length + '">' + groupCol + ': ' + W3B.esc(last) + '</td></tr>'; }
            h += '<tr data-i="' + i + '">' + cols.map(function (c) {
                return '<td class="' + (c.cls || '') + '">' + W3B.esc(c.f ? c.f(r) : r[c.k]) + '</td>';
            }).join('') + '</tr>';
        });
        $(tableId).innerHTML = h + '</tbody>';
    }

    function renderHistory(d) {
        hist = d || { byWorkStation: [], byOperation: [] };
        renderGrid('grdDetail', hist.byWorkStation, 'WorkStationName');
        renderGrid('grdII', hist.byOperation, 'OperationName');
    }

    function history() { return W3B.getJson(api + '/history').then(renderHistory).catch(W3B.fail); }

    function applyCombos(d) {                                  // OeraptionNamefill:120 / StationNamefill:159 (BindDDLNew ZeroIndex: true)
        W3B.fill('cmboperationname', d.operations, true);
        W3B.fill('cmbworkstationname', d.workStations, true);
    }

    function reset() {                                         // Reset:198
        recId = 0; setMode(false);
        W3B.clearCombo('cmboperationname'); W3B.clearCombo('cmbworkstationname');
        W3B.focus('cmbworkstationname');
        return history();
    }

    function refresh() { W3B.getJson(api + '/refresh').then(applyCombos).catch(W3B.fail); }   // btnRefresh_Click:229

    function validate() {                                      // FormValidation:254
        if (W3B.int($('cmboperationname').value) === 0) { W3B.box('Operation Name Field Is Required'); W3B.focus('cmboperationname'); return false; }
        if (W3B.int($('cmbworkstationname').value) === 0) { W3B.box('Work Station Name Field Is Required'); W3B.focus('cmbworkstationname'); return false; }
        return true;
    }

    function insert() {                                        // Insert():271
        if (!validate()) return;
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        W3B.postJson(api + '/save', {
            id: recId, operationId: W3B.int($('cmboperationname').value), workStationId: W3B.int($('cmbworkstationname').value),
            opHoursPerDay: W3B.num($('txtOpHoursPerDay').value)
        }).then(function (r) { W3B.box(r.message); return reset(); }).catch(W3B.fail);
    }

    function save() { recId = 0; insert(); }
    function update() { if (recId === 0) { W3B.box('RecId not found'); return; } insert(); }
    function saveII() { if ($('BtnSaveII').textContent === 'Save') save(); else update(); }

    function edit(r) {                                         // grdDetail_DoubleClick:438 / grdII_DoubleClick:457
        if (!r) return;
        recId = r.Id; setMode(true);
        W3B.setCombo('cmboperationname', r.ProductionOperationId);
        W3B.setCombo('cmbworkstationname', r.ProductionWorkStationId);
        $('txtOpHoursPerDay').value = r.OpHoursPerDay;
        W3B.focus('cmboperationname');
    }

    function defineOperations() { window.location.href = '/production/mfg/operation'; }      // BtnOperation_Click:595 (screen 770)

    function shortcutKeys() {                                  // MakeShortCutKeys:537
        W3B.box(['Ctrl+S   For Save', 'Ctrl+U   For Update', 'Ctrl+R   For Refresh', 'Ctrl+E   For Close', 'Ctrl+N   For New',
            'Ctrl+F1   For Open On Work Station Define Form', 'Ctrl+F2   For Open On Operation Define form',
            'Ctrl+F5   For Focus On Work Station Name', 'Ctrl+alt   To Show ShortCut Keys Form',
            'Ctrl+ArrowDown   For Focus On Grid', 'Ctrl+ArrowUp   For Focus On Work Station Name'].join('\n'));
    }

    window.Wso = { reset: reset, refresh: refresh, save: save, update: update, saveII: saveII, defineOperations: defineOperations, shortcutKeys: shortcutKeys };

    document.addEventListener('DOMContentLoaded', function () {
        $('grdDetail').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) edit(hist.byWorkStation[+tr.getAttribute('data-i')]); });
        $('grdII').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) edit(hist.byOperation[+tr.getAttribute('data-i')]); });
        W3B.keys({
            n: reset, r: refresh,
            s: function () { if (!$('btnsave').classList.contains('is-hidden')) save(); },
            u: function () { if (!$('btnUpdate').classList.contains('is-hidden')) update(); },
            e: function () { window.location.href = '/production'; },
            escape: function () { window.location.href = '/production'; },
            f1: defineOperations, f5: function () { W3B.focus('cmbworkstationname'); },
            arrowup: function () { W3B.focus('cmbworkstationname'); },
            arrowdown: function () { $('grdDetailHost').scrollIntoView(); }
        });
        setMode(false);
        W3B.getJson(api + '/lookups').then(function (d) { applyCombos(d); renderHistory(d.history); }).catch(W3B.fail);   // _Load:87
        W3B.focus('cmbworkstationname');
    });
}());
