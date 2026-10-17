/* ============================================================================================
 * Screen 773 "Production Routing" - Architecture.WinApp.Mfg.frmProductionRouting (base.Name "frmProductionRouting").
 * Line numbers are the .cs: _Load:169, HistoryRoutingNameBind:214, RoutingNameBind:263, AllWorkStationOperationFill:311,
 * FormValidationDetail:360, BtnAddOp_Click:377, BtnUpdateOp_Click:418, grdOperationsDetail_DoubleClick:476,
 * grdOperationsDetailSetting:502, CellUpdated:554, DeleteDetailRow:593, FromReset:654, FromResetDetail:677, btnRefresh_Click:698,
 * GetMissingSequence:754, Insert():778, ReadById:925, btnDelete_Click:959, CmbRountingName_Leave:988, HistoryGridFill:1068,
 * HistoryGridSetting:1123, DatagridHistory_DoubleClick:1159, KeyDown:1226, MakeShortCutKeys:1329, txtMinutes_TextChanged:1525.
 * API /production/mfg/routing/api.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/mfg/routing/api';
    var $ = W3B.$;
    var look = {}, rights = {};
    var recId = 0, updateIdx = -1;
    var ops = [], removed = [], histRows = [], wsoMap = {};

    function applyRights() {                                   // _Load:169-187
        $('btnsave').disabled = !rights.save; $('btnUpdate').disabled = !rights.update; $('btnDelete').disabled = !rights.delete;
        var missing = [];
        if (!rights.save) missing.push('Save'); if (!rights.update) missing.push('Update'); if (!rights.delete) missing.push('Delete');
        var n = $('rightsNote');
        n.textContent = missing.length ? 'Your account does not have the ' + missing.join(', ') + ' right on this screen, so those buttons are off.' : '';
        n.classList.toggle('is-hidden', !missing.length);
    }

    function applyCombos(d) {
        W3B.fill('CmbRountingName', d.routingNames, false);                      // RoutingNameBind:263 (ZeroIndex false)
        W3B.fill('cmbWorkStationOperation', d.workStationOperations, true);      // AllWorkStationOperationFill:311
        W3B.fill('CmbroutingNameHistory', d.historyNames, false);                // HistoryRoutingNameBind:214
        wsoMap = {};
        (d.workStationOperations || []).forEach(function (o) { wsoMap[o.Id] = o; });
    }

    // ------------------------------------------------------------------ operations grid (grdOperationsDetail)
    function renderOps() {
        var cols = [
            { h: '', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return '<button type="button" class="rc-x" data-act="del" data-i="' + i + '" title="Ctrl+Delete">X</button>'; } },
            { h: 'Edit', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-act="edit" data-i="' + i + '">Edit</button>'; } },
            { h: 'OperationName', k: 'OperationName', w: 130 }, { h: 'WorkStation', k: 'WorkStation', w: 150 },
            { h: 'CapacityQty', html: true, cls: 'num', w: 70, f: function (r, i) { return cell('CapacityQty', i, r.CapacityQty); } },
            { h: 'Hours', html: true, cls: 'num', w: 45, f: function (r, i) { return cell('Hours', i, r.Hours); } },
            { h: 'Minutes', html: true, cls: 'num', w: 60, f: function (r, i) { return cell('Minutes', i, r.Minutes); } },
            { h: 'SeqNo', html: true, cls: 'num', w: 45, f: function (r, i) { return cell('SeqNo', i, r.SeqNo); } },
            { h: 'Active', html: true, cls: 'ctr', w: 50, f: function (r, i) { return '<input type="checkbox" data-f="Active" data-i="' + i + '"' + (r.Active ? ' checked' : '') + '>'; } }
        ];
        $('grdOperationsDetail').innerHTML = W3B.tableHtml(cols, ops, { empty: 'No operations' });
    }
    function cell(f, i, v) { return '<input class="cell" style="width:60px" inputmode="decimal" data-f="' + f + '" data-i="' + i + '" value="' + W3B.esc(v) + '">'; }

    function validateDetail() {                                // FormValidationDetail:360
        if (W3B.int($('cmbWorkStationOperation').value) === 0) { W3B.box('Operation Name Field is Required'); W3B.focus('cmbWorkStationOperation'); return false; }
        if (W3B.num($('txtCapacityQty').value) === 0) { W3B.box('CapacityQty Field is Required'); W3B.focus('txtCapacityQty'); return false; }
        return true;
    }

    function detailRow() {
        var id = W3B.int($('cmbWorkStationOperation').value), w = wsoMap[id] || {};
        return { Id: 0, OperationId: id, OperationName: w.OperationName || '', WorkStation: w.WorkStationName || '',
            CapacityQty: W3B.num($('txtCapacityQty').value), Hours: W3B.int($('txtHours').value), Minutes: W3B.int($('txtMinutes').value),
            SeqNo: W3B.int($('txtSeqNo').value), Active: $('chkIsActive').checked };
    }

    function addOp() {                                         // BtnAddOp_Click:377
        if (!validateDetail()) return;
        var id = W3B.int($('cmbWorkStationOperation').value);
        for (var i = 0; i < ops.length; i++) if (ops[i].OperationId === id) { W3B.box('Operation Already Exist in Grid!'); return; }
        ops.push(detailRow());
        renderOps(); resetDetail();
    }

    function updateOp() {                                      // BtnUpdateOp_Click:418
        if (!validateDetail()) return;
        var id = W3B.int($('cmbWorkStationOperation').value);
        for (var i = 0; i < ops.length; i++) if (ops[i].OperationId === id && i !== updateIdx) { W3B.box('WorkStation Operation Already Exist in Grid!'); return; }
        var keep = ops[updateIdx], n = detailRow();
        n.Id = keep.Id;
        ops[updateIdx] = n;
        renderOps(); resetDetail();
    }

    function cancelOp() { resetDetail(); }                     // BtnCancelOp_Click:464

    function resetDetail() {                                   // FromResetDetail:677
        updateIdx = -1;
        W3B.clearCombo('cmbWorkStationOperation');
        ['txtCapacityQty', 'txtHours', 'txtMinutes', 'txtSeqNo'].forEach(function (id) { $(id).value = ''; });
        W3B.show('BtnAddOp', true); W3B.show('BtnCancelOp', false); W3B.show('BtnUpdateOp', false);
        W3B.focus('cmbWorkStationOperation');
    }

    function editOp(i) {                                       // grdOperationsDetail_DoubleClick:476
        var r = ops[i]; if (!r) return;
        updateIdx = i;
        W3B.setCombo('cmbWorkStationOperation', r.OperationId);
        $('txtCapacityQty').value = W3B.fmt(r.CapacityQty); $('txtHours').value = r.Hours; $('txtMinutes').value = r.Minutes; $('txtSeqNo').value = r.SeqNo;
        $('chkIsActive').checked = !!r.Active;
        W3B.show('BtnAddOp', false); W3B.show('BtnUpdateOp', true); W3B.show('BtnCancelOp', true);
        W3B.focus('cmbWorkStationOperation');
    }

    function delOp(i) {                                        // DeleteDetailRow:593
        var r = ops[i]; if (!r) return;
        if (updateIdx !== -1) { W3B.box('Please Reset the Detail First...'); return; }
        if (r.Id !== 0) {
            if (!W3B.ask('Are you sure to Delete?')) return;
            removed.push({ Id: r.Id, OperationId: r.OperationId, CapacityQty: r.CapacityQty, SeqNo: r.SeqNo, Hours: r.Hours, Minutes: r.Minutes, Active: r.Active });
        }
        ops.splice(i, 1);
        renderOps();
    }

    // ------------------------------------------------------------------ form
    function setMode(update, canDelete) {
        W3B.show('btnsave', !update); W3B.show('btnUpdate', update); W3B.show('btnDelete', !!canDelete);
    }

    function fromReset() {                                     // FromReset:654
        recId = 0; removed = []; ops = [];
        resetDetail(); renderOps();
        W3B.clearCombo('CmbRountingName');
        setMode(false, false);
        W3B.focus('CmbRountingName');
    }

    function refresh() {                                       // btnRefresh_Click:698
        W3B.getJson(api + '/refresh').then(applyCombos).catch(W3B.fail);
    }

    function readById(d) {                                     // ReadById:925
        ops = d.rows.map(function (r) { return r; }); removed = [];
        recId = d.recId; updateIdx = -1;
        W3B.setCombo('CmbRountingName', d.routingNameId);
        renderOps();
        setMode(true, true);
    }

    function routingNameLeave() {                              // CmbRountingName_Leave:988 (only with the Update right)
        var rid = W3B.int($('CmbRountingName').value);
        if (rid <= 0 || !rights.update) return;
        if (ops.length > 0 && !W3B.ask('Are you sure to Refresh Operations Grid?')) return;
        W3B.getJson(api + '/read/' + rid).then(readById).catch(function () { /* "Record not found..." for a new routing name: nothing to load */ });
    }

    function insert() {                                        // Insert():778
        var rid = W3B.int($('CmbRountingName').value);
        if (rid === 0) { W3B.box('Routing Name is Required...'); W3B.focus('CmbRountingName'); return; }
        if (ops.length === 0) { W3B.box('Grid Record Not Found...\nGrid Should have atleast one record'); return; }
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        W3B.postJson(api + '/save', { id: recId, routingNameId: rid, rows: ops, removed: removed }).then(function (r) {
            W3B.box(r.message);
            fromReset(); history();
        }).catch(W3B.fail);
    }

    function save() { recId = 0; insert(); }
    function update() { if (recId === 0) { W3B.box('RecId Not found'); return; } insert(); }

    function del() {                                           // btnDelete_Click:959
        if (recId <= 0) { W3B.box('No record found to Delete'); return; }
        var rid = W3B.int($('CmbRountingName').value);
        if (rid === 0) { W3B.box('Please Select RoutingName...'); W3B.focus('CmbRountingName'); return; }
        if (!W3B.ask('Are you sure to Delete?')) return;
        W3B.request(api + '/delete?routingNameId=' + rid, { method: 'POST', headers: {} }).then(function (r) {
            W3B.box(r.message); fromReset(); history();
        }).catch(W3B.fail);
    }

    // ------------------------------------------------------------------ history
    function history() {                                       // HistoryGridFill:1068
        return W3B.getJson(api + '/history' + W3B.qs({ routingNameId: W3B.int($('CmbroutingNameHistory').value) || '' })).then(function (d) {
            histRows = d || [];
            var cols = [{ h: 'Edit', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-i="' + i + '">Edit</button>'; } },
                { h: 'Seq No', k: 'SequenceNo', cls: 'num', w: 60 }, { h: 'OperationName', k: 'OperationName', w: 120 }, { h: 'WorkStation', k: 'WorkStation', w: 120 },
                { h: 'CapacityQty', f: function (r) { return W3B.fmt(r.CapacityQty); }, cls: 'num', w: 70 }, { h: 'Hours', k: 'Hours', cls: 'num', w: 50 },
                { h: 'Minutes', k: 'Minutes', cls: 'num', w: 60 }, { h: 'Active', k: 'Active', bool: true, cls: 'ctr', w: 45 }];
            var h = '<thead><tr>' + cols.map(function (c) { return '<th style="width:' + c.w + 'px">' + c.h + '</th>'; }).join('') + '</tr></thead><tbody>';
            if (!histRows.length) h += '<tr class="rc-empty"><td colspan="' + cols.length + '">No records</td></tr>';
            var last = null;
            histRows.forEach(function (r, i) {
                if (r.RoutingNameId !== last) { last = r.RoutingNameId; h += '<tr class="w3-group"><td colspan="' + cols.length + '">RoutingName: ' + W3B.esc(r.RoutingName) + '</td></tr>'; }
                h += '<tr data-i="' + i + '">' + cols.map(function (c) {
                    var v = c.f ? c.f(r, i) : r[c.k];
                    if (c.html) return '<td class="' + c.cls + '">' + v + '</td>';
                    if (c.bool) v = v ? '&#10003;' : ''; else v = W3B.esc(v);
                    return '<td class="' + (c.cls || '') + '">' + v + '</td>';
                }).join('') + '</tr>';
            });
            $('DatagridHistory').innerHTML = h + '</tbody>';
        }).catch(W3B.fail);
    }

    function openHistory(i) {                                  // DatagridHistory_DoubleClick:1159 / Edit button
        var r = histRows[i]; if (!r) return;
        if (!rights.update) { W3B.box("you don't have update rights..."); return; }
        W3B.getJson(api + '/read/' + r.RoutingNameId).then(readById).catch(W3B.fail);
    }

    function lookup() {                                        // BtnLookup_Click:1476 (ProductionRoutingName define form)
        W3B.box('Define Routing Name is a separate screen (ProductionRoutingName). Add the name there, then press Refresh here.');
    }

    function shortcutKeys() {                                  // MakeShortCutKeys:1329
        W3B.box(['Ctrl+S   For Save', 'Alt+S   For Show Data on History', 'Ctrl+U   For Update', 'Ctrl+Shift+Delete   For Delete',
            'Ctrl+E   For Close', 'Ctrl+R   For Refresh', 'Ctrl+N   For New', 'Ctrl+F5   For Focus on RoutingName',
            'Ctrl+alt   To Show ShortCut Keys Form', 'Ctrl+Delete   For Deleting an row of Focused Grid',
            'Ctrl+ArrowDown   For Focus On Detail Grid', 'Ctrl+ArrowUp   For Focus On on RoutingName'].join('\n'));
    }

    window.Rt = { newClick: fromReset, save: save, update: update, refresh: refresh, del: del, history: history, addOp: addOp, updateOp: updateOp,
        cancelOp: cancelOp, routingNameLeave: routingNameLeave, lookup: lookup, shortcutKeys: shortcutKeys };

    document.addEventListener('DOMContentLoaded', function () {
        var g = $('grdOperationsDetail');
        g.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var i = +b.getAttribute('data-i');
            if (b.getAttribute('data-act') === 'del') delOp(i); else editOp(i);
        });
        g.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && e.target.tagName !== 'INPUT') editOp(+tr.getAttribute('data-i')); });
        g.addEventListener('change', function (e) {            // grdOperationsDetail_CellUpdated:554 / UpdatingCell:1540 (numeric only)
            var t = e.target, f = t.getAttribute('data-f'), i = +t.getAttribute('data-i'); if (!f || !ops[i]) return;
            if (f === 'Active') { ops[i].Active = t.checked; return; }
            if (isNaN(Number(String(t.value).replace(/,/g, '')))) { W3B.box('Please Type Only Numeric Value'); t.value = ops[i][f]; return; }
            ops[i][f] = f === 'CapacityQty' ? W3B.num(t.value) : W3B.int(t.value);
            if (f === 'Minutes' && ops[i].Minutes > 60) { ops[i].Minutes = 0; t.value = 0; }
        });
        $('DatagridHistory').addEventListener('click', function (e) { var b = e.target.closest('button[data-i]'); if (b) openHistory(+b.getAttribute('data-i')); });
        $('DatagridHistory').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) openHistory(+tr.getAttribute('data-i')); });
        $('txtMinutes').addEventListener('input', function () { if (W3B.int($('txtMinutes').value) > 60) $('txtMinutes').value = '0'; });   // :1525
        ['txtHours', 'txtMinutes', 'txtSeqNo'].forEach(function (id) { $(id).addEventListener('input', function () { var e = $(id); var v = e.value.replace(/[^0-9]/g, ''); if (v !== e.value) e.value = v; }); });
        $('txtCapacityQty').addEventListener('input', function () { var e = $('txtCapacityQty'); var v = e.value.replace(/[^0-9.]/g, ''); if (v !== e.value) e.value = v; });
        W3B.keys({
            n: fromReset, r: refresh,
            s: function () { if (!$('btnsave').classList.contains('is-hidden') && !$('btnsave').disabled) save(); },
            u: function () { if (!$('btnUpdate').classList.contains('is-hidden') && !$('btnUpdate').disabled) update(); },
            'shift+delete': function () { if (!$('btnDelete').classList.contains('is-hidden') && !$('btnDelete').disabled) del(); },
            e: function () { window.location.href = '/production'; }, escape: function () { window.location.href = '/production'; },
            f5: function () { W3B.focus('CmbRountingName'); }, arrowup: function () { W3B.focus('CmbRountingName'); },
            arrowdown: function () { $('grdOperationsDetailHost').scrollIntoView(); }
        });
        document.addEventListener('keydown', function (e) { if (e.altKey && !e.ctrlKey && (e.key || '').toLowerCase() === 's') { e.preventDefault(); history(); } });
        setMode(false, false);
        renderOps();
        W3B.getJson(api + '/lookups').then(function (d) { look = d; rights = d.rights || {}; applyRights(); applyCombos(d); }).catch(W3B.fail);
        W3B.focus('CmbRountingName');
    });
}());
