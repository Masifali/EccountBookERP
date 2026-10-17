/* ============================================================================================
 * Screen 771 "Production Work Station" - Architecture.WinApp.Mfg.ProductionWorkStation (base.Name "ProductionWorkStation").
 * Line numbers are the .cs: _Load:102, DeprtmentFill:131, Reset:156, btnRefresh_Click:203, FormValidation:215, Insert():238,
 * ReadById:337, FormHistory:365, GrdSetting:416, grdDetail_DoubleClick:441, MakeShortCutKeys:520, KeyPress (decimal only):569-605.
 * API /production/mfg/work-station/api.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/mfg/work-station/api';
    var $ = W3B.$;
    var recId = 0, rows = [];
    var FIELDS = ['txtStationName', 'txtelectricityCost', 'txtproductionCapacity', 'txtrentCost', 'txtwages', 'txtWorkStationcode'];

    function setMode(update) {
        W3B.show('btnsave', !update); W3B.show('btnUpdate', update);
        $('BtnSaveII').textContent = update ? 'Update' : 'Save';
    }

    /** FormHistory:365 + GrdSetting:416 - grouped by Department (hidden column), SerialNo restarts per group. */
    function renderGrid() {
        var cols = [
            { h: 'SerialNo', k: 'SerialNo', cls: 'num', w: 55 }, { h: 'WorkStationName', k: 'WorkStationName', w: 125 },
            { h: 'WorkStationCode', k: 'WorkStationCode', w: 90 }, { h: 'ElectricityCost', f: function (r) { return W3B.fmt(r.ElectricityCost); }, cls: 'num', w: 70 },
            { h: 'ProductionCapacity', f: function (r) { return W3B.fmt(r.ProductionCapacity); }, cls: 'num', w: 75 },
            { h: 'RentCost', f: function (r) { return W3B.fmt(r.RentCost); }, cls: 'num', w: 65 },
            { h: 'Wages', f: function (r) { return W3B.fmt(r.Wages); }, cls: 'num', w: 65 },
            { h: 'Active', k: 'Active', bool: true, cls: 'ctr', w: 50 }];
        var h = '<thead><tr>' + cols.map(function (c) { return '<th style="width:' + c.w + 'px">' + c.h + '</th>'; }).join('') + '</tr></thead><tbody>';
        if (!rows.length) h += '<tr class="rc-empty"><td colspan="' + cols.length + '">No records</td></tr>';
        var last = null;
        rows.forEach(function (r, i) {
            if (r.DepartmentId !== last) { last = r.DepartmentId; h += '<tr class="w3-group"><td colspan="' + cols.length + '">DepartmentName: ' + W3B.esc(r.DepartmentName) + '</td></tr>'; }
            h += '<tr data-i="' + i + '">' + cols.map(function (c) {
                var v = c.f ? c.f(r) : r[c.k]; if (c.bool) v = v ? '&#10003;' : ''; else v = W3B.esc(v);
                return '<td class="' + (c.cls || '') + '">' + v + '</td>';
            }).join('') + '</tr>';
        });
        $('grdDetail').innerHTML = h + '</tbody>';
    }

    function history() {
        return W3B.getJson(api + '/history').then(function (d) { rows = d || []; renderGrid(); }).catch(W3B.fail);
    }

    function reset() {                                         // Reset:156 (the Department and Active are not cleared)
        recId = 0; setMode(false);
        FIELDS.forEach(function (id) { $(id).value = ''; });
        W3B.focus('txtStationName');
        return history();
    }

    function refresh() {                                       // btnRefresh_Click:203 - DeprtmentFill
        W3B.getJson(api + '/departments').then(function (d) { W3B.fill('cmbDepartmentTo', d, true); }).catch(W3B.fail);
    }

    function validate() {                                      // FormValidation:215
        if (W3B.int($('cmbDepartmentTo').value) === 0) { W3B.box('Department Field Is Required'); W3B.focus('cmbDepartmentTo'); return false; }
        if ($('txtStationName').value === '') { W3B.box('Work Station Name Field Is Required'); W3B.focus('txtStationName'); return false; }
        if ($('txtWorkStationcode').value === '') { W3B.box('Work Station Code Field Is Required'); W3B.focus('txtWorkStationcode'); return false; }
        return true;
    }

    function insert() {                                        // Insert():238
        if (!validate()) return;
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        W3B.postJson(api + '/save', {
            id: recId, departmentId: W3B.int($('cmbDepartmentTo').value), workStationName: $('txtStationName').value,
            workStationCode: $('txtWorkStationcode').value, electricityCost: W3B.num($('txtelectricityCost').value),
            productionCapacity: W3B.num($('txtproductionCapacity').value), wages: W3B.num($('txtwages').value),
            rentCost: W3B.num($('txtrentCost').value), isActive: $('chkIsActive').checked
        }).then(function (r) { W3B.box(r.message); return reset(); }).catch(W3B.fail);
    }

    function save() { recId = 0; insert(); }
    function update() { if (recId === 0) { W3B.box('RecId not found'); return; } insert(); }
    function saveII() { if ($('BtnSaveII').textContent === 'Save') save(); else update(); }

    function edit(i) {                                         // grdDetail_DoubleClick:441 -> ReadById:337
        var r = rows[i]; if (!r) return;
        W3B.getJson(api + '/' + r.Id).then(function (d) {
            recId = d.Id; setMode(true);
            W3B.setCombo('cmbDepartmentTo', d.departmentId);
            $('txtStationName').value = d.workStationName; $('txtWorkStationcode').value = d.workStationCode;
            $('txtelectricityCost').value = d.electricityCost; $('txtproductionCapacity').value = d.productionCapacity;
            $('txtwages').value = d.wages; $('txtrentCost').value = d.rentCost; $('chkIsActive').checked = !!d.isActive;
            W3B.focus('cmbDepartmentTo');
        }).catch(W3B.fail);
    }

    function defineDepartment() {                              // BtnDefineDepartment_Click:557 (Define_Department)
        W3B.box('Define Department is a separate screen (Define_Department); open it from the Admin / HR menu, then press Refresh here.');
    }

    function shortcutKeys() {                                  // MakeShortCutKeys:520
        W3B.box(['Ctrl+S   For Save', 'Ctrl+U   For Update', 'Ctrl+E   For Close', 'Ctrl+N   For New',
            'Ctrl+F5   For Focus On Work Station Name', 'Ctrl+alt   To Show ShortCut Keys Form',
            'Ctrl+ArrowDown   For Focus On Grid', 'Ctrl+ArrowUp   For Focus On Work Station Name'].join('\n'));
    }

    window.Ws = { reset: reset, save: save, update: update, saveII: saveII, refresh: refresh, defineDepartment: defineDepartment, shortcutKeys: shortcutKeys };

    document.addEventListener('DOMContentLoaded', function () {
        $('grdDetail').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) edit(+tr.getAttribute('data-i')); });
        // OnlytextdecimelFunction: digits and one decimal point only
        document.querySelectorAll('input[data-dec]').forEach(function (el) {
            el.addEventListener('input', function () { var v = el.value.replace(/[^0-9.]/g, ''); var p = v.indexOf('.'); if (p >= 0) v = v.slice(0, p + 1) + v.slice(p + 1).replace(/\./g, ''); if (v !== el.value) el.value = v; });
        });
        W3B.keys({
            n: reset,
            s: function () { if (!$('btnsave').classList.contains('is-hidden')) save(); },
            u: function () { if (!$('btnUpdate').classList.contains('is-hidden')) update(); },
            e: function () { window.location.href = '/production'; },
            escape: function () { window.location.href = '/production'; },
            f5: function () { W3B.focus('txtStationName'); },
            arrowup: function () { W3B.focus('txtStationName'); },
            arrowdown: function () { $('grdDetailHost').scrollIntoView(); }
        });
        setMode(false);
        W3B.getJson(api + '/lookups').then(function (d) {
            W3B.fill('cmbDepartmentTo', d.departments, true);   // DeprtmentFill:131 (BindDDLNew ZeroIndex: true)
            rows = d.history || []; renderGrid();
        }).catch(W3B.fail);
        W3B.focus('txtStationName');
    });
}());
