/* ============================================================================================
 * countx_export_container_inspection.js - GatePassInspection.cs (Architecture.WinApp.Export), screen 205
 * "Container Inspection" ("Vehicle Cargo Inspection Report"). Form | History; every button, cmbgatepass_Leave,
 * the two container grids (grouped by Category, Status checkbox, Remarks editable), Save / Update with the
 * desktop's two-document flow and messages, history Show / New, GpSlip / Edit buttons, selection detail and
 * the KeyDown shortcuts have their counterpart here; data from /api/export/container-inspection.
 * Print-244 -> CrystalPrint key exp-244 (USP_ExImVCITransaction_SlipAndRegister @GpId), to be registered by
 * the coordinator; GpSlip -> CommonServices.OutwardGatePassWithWb (258-OutwardGatePassWithWbAndLabSlip.rpt) - not registered.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var F = global.ExportF, $id = F.$id, box = F.box, ask = F.ask, str = F.str, netI = F.netI, val = F.val, setText = F.setText,
        setVal = F.setVal, bind = F.bind, busy = F.busy, getJson = F.getJson, postJson = F.postJson, show = F.show, focus = F.focus, esc = F.esc;
    var API = '/api/export/container-inspection';

    var PERM = { Save: true, Update: true };
    var GPS = [], PARAMS = [];
    var S = { recId: 0, recId2: 0, gpId: 0 };
    var DOC1 = [], DOC2 = [];
    var HIST = { rows: [], cur: -1 };

    function bindGps() { bind('cmbgatepass', GPS, 'Id', 'GpSrNo', ['GpDate', 'VehicleNo', 'BiltyNo', 'Status'], true); }

    /** GatePassInspection_Load. */
    function load() {
        return getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
            $id('btnsave').disabled = !PERM.Save; $id('btnupdate').disabled = !PERM.Update;
            ['gatePasses', 'parameters'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
            GPS = d.gatePasses || []; bindGps(); setVal('cmbgatepass', '0');
            PARAMS = d.parameters || [];
            DOC1 = clone(PARAMS); DOC2 = clone(PARAMS); render(1); render(2);
            setText('FromDateHistory', F.daysAgo(3)); setText('ToDateHistory', F.today());
            $id('vciFooterInfo').textContent = 'GatePassInspection  -  Document Type 180';
            focus('cmbgatepass');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function clone(rows) { return rows.map(function (r) { var c = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) c[k] = r[k]; return c; }); }
    /** btnRefresh_Click -> Gatepassfill(0). */
    function refresh(btn) { return busy(btn, function () { return getJson(API + '/gate-passes?id=0').then(function (rows) { GPS = rows || []; bindGps(); }).catch(function (e) { box(e.message); }); }); }

    // ------------------------------------------------------------------------------ grids (grouped by Category)

    function render(which) {
        var rows = which === 1 ? DOC1 : DOC2, body = $id(which === 1 ? 'doc1Body' : 'doc2Body');
        var h = '', lastCat = null;
        rows.forEach(function (r, i) {
            if (r.Category !== lastCat) { h += '<tr class="exf-group"><td colspan="4">Category: ' + esc(r.Category) + '</td></tr>'; lastCat = r.Category; }
            h += '<tr data-i="' + i + '"><td>' + esc(r.Category) + '</td><td>' + esc(r.InspectionPoint) + '</td>' +
                '<td class="ctr"><input type="checkbox" class="exf-cell-chk" data-i="' + i + '" data-k="Status"' + (r.Status ? ' checked' : '') + '/></td>' +
                '<td><input class="exf-cell" data-i="' + i + '" data-k="Remarks" value="' + esc(r.Remarks) + '"/></td></tr>';
        });
        body.innerHTML = h;
    }
    function renderHist(which, rows) {
        $id(which === 1 ? 'h1Body' : 'h2Body').innerHTML = (rows || []).map(function (r) {
            return '<tr><td>' + esc(r.Category) + '</td><td>' + esc(r.InspectionPoint) + '</td><td>' + (r.Status ? 'True' : 'False') + '</td><td>' + esc(r.Remarks) + '</td></tr>';
        }).join('');
    }
    /** ResetgrdDocFirst / ResetgrdDocSecond: Status false, Remarks "" on every row. */
    function resetGrid(which) { (which === 1 ? DOC1 : DOC2).forEach(function (r) { r.Status = false; r.Remarks = ''; }); render(which); }

    // ------------------------------------------------------------------------------ read / reset

    /** cmbgatepass_Leave. */
    function gatePassLeave() {
        var id = netI(val('cmbgatepass'));
        if (id > 0) {
            var gp = F.findRow(GPS, id);
            return readById(id).then(function () { setText('txtvehicleno1', gp ? gp.VehicleNo : ''); });
        }
        resetForReadById();
        return Promise.resolve();
    }
    /** ReadById(GatePassId). */
    function readById(id) {
        return getJson(API + '/by-gate-pass?gatePassId=' + id).then(function (d) {
            show('btnsave', false); show('btnupdate', true);
            S.recId = 0; S.recId2 = 0; S.gpId = id;
            tab('tabPage1');
            if (d.found) {
                fill(1, d.doc1); fill(2, d.doc2 || { RecId: 0, rows: clone(PARAMS) });
            } else {
                resetForReadById();
                setText('txtcontainerno1', d.doc1 ? d.doc1.ContainerNo : ''); setText('txtcontainer2', d.doc2 ? d.doc2.ContainerNo : '');
            }
        }).catch(function (e) { box(e.message); });
    }
    function fill(which, d) {
        var p = which === 1 ? { c: 'txtcontainerno1', s: 'txtsealno1', i: 'txtinspect1', v: 'txtverify1', pt: 'txtpict1', r: 'txtremarks1', k: 'chkloading1' }
            : { c: 'txtcontainer2', s: 'txtseal2', i: 'txtinspect2', v: 'txtverify2', pt: 'txtpict2', r: 'txtremarks2', k: 'chkloading2' };
        if (which === 1) S.recId = netI(d.RecId); else S.recId2 = netI(d.RecId);
        if (netI(d.RecId) > 0 || d.ContainerNo !== undefined) {
            setText(p.c, d.ContainerNo); setText(p.s, d.SealNo); setText(p.i, d.InspectedBy); setText(p.v, d.VerifiedBy); setText(p.pt, d.PictureTakenBy);
            setText(p.r, d.InspectorRemarks); $id(p.k).checked = !!d.IsContainerAccepted;
        }
        var rows = d.rows && d.rows.length ? d.rows : clone(PARAMS);
        if (which === 1) DOC1 = rows; else DOC2 = rows;
        render(which);
    }
    /** ResetMain(). */
    function resetMain() {
        S.recId = 0; S.recId2 = 0; S.gpId = 0;
        ['txtvehicleno1', 'txtcontainerno1', 'txtcontainer2', 'txtsealno1', 'txtseal2'].forEach(function (id) { setText(id, ''); });
        setVal('cmbgatepass', '0');
        show('btnsave', true); show('btnupdate', false);
        focus('cmbgatepass');
        resetGrid(1); resetGrid(2);
        return getJson(API + '/gate-passes?id=0').then(function (rows) { GPS = rows || []; bindGps(); setVal('cmbgatepass', '0'); }).catch(function (e) { box(e.message); });
    }
    /** ResetForReadById(). */
    function resetForReadById() {
        S.recId = 0; S.recId2 = 0;
        ['txtcontainerno1', 'txtcontainer2', 'txtsealno1', 'txtseal2'].forEach(function (id) { setText(id, ''); });
        show('btnsave', true); show('btnupdate', false);
        DOC1 = clone(PARAMS); DOC2 = clone(PARAMS); render(1); render(2);
    }

    // ------------------------------------------------------------------------------ save

    function docBody(which) {
        var p = which === 1 ? { c: 'txtcontainerno1', s: 'txtsealno1', i: 'txtinspect1', v: 'txtverify1', pt: 'txtpict1', r: 'txtremarks1', k: 'chkloading1' }
            : { c: 'txtcontainer2', s: 'txtseal2', i: 'txtinspect2', v: 'txtverify2', pt: 'txtpict2', r: 'txtremarks2', k: 'chkloading2' };
        return { RecId: which === 1 ? S.recId : S.recId2, ContainerNo: val(p.c), SealNo: val(p.s), InspectedBy: val(p.i), VerifiedBy: val(p.v),
            PictureTakenBy: val(p.pt), InspectorRemarks: val(p.r), IsContainerAccepted: F.checked(p.k), rows: which === 1 ? DOC1 : DOC2 };
    }
    /** FormValidationDocFirst / FormValidationDocSecond. */
    function validateDoc(which) {
        var d = docBody(which), no = which === 1 ? '01' : '02';
        var ids = which === 1 ? ['txtinspect1', 'txtverify1', 'txtpict1'] : ['txtinspect2', 'txtverify2', 'txtpict2'];
        if (d.InspectedBy === '') { box('Inspected By Field is Required'); focus(ids[0]); return false; }
        if (d.VerifiedBy === '') { box('Verified By Field is Required'); focus(ids[1]); return false; }
        if (d.PictureTakenBy === '') { box('Picture Taken By Field is Required'); focus(ids[2]); return false; }
        if (d.rows.length > 0 && !d.rows.some(function (r) { return r.Status; })) { box('AtLeast one Row Should be Checked in Grid ' + no); return false; }
        return true;
    }
    /** btnsave_Click (update = false) / btnUpdate_Click (update = true). */
    function saveFlow(btn, update) {
        return busy(btn, function () {
            if (!F.hasSel('cmbgatepass')) { box('Gate Pass No Field is Required'); focus('cmbgatepass'); return Promise.resolve(); }
            var v1 = false, v2 = false;
            if (val('txtcontainerno1') !== '') { if (!validateDoc(1)) return Promise.resolve(); v1 = true; }
            if (val('txtcontainer2') !== '') { if (!validateDoc(2)) return Promise.resolve(); v2 = true; }
            if (!v1 && !v2) { box(update ? 'Container No fields is Required...' : 'Container No field is Required...'); focus('txtcontainerno1'); return Promise.resolve(); }
            if (v1 && !ask((update && S.recId > 0) ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();   /* InsertDocFirst confirms; InsertDocSecond does not */
            return postJson(API + '/save', { update: update, gatePassId: netI(val('cmbgatepass')), doc1: docBody(1), doc2: docBody(2) }).then(function (d) {
                box((d && d.message) || (update ? 'Record Updated Successfully...' : 'Record Save Successfully...'));
                return resetMain();
            }).catch(function (e) { box(e.message); });
        });
    }
    function save(btn) { return saveFlow(btn, false); }
    function update(btn) { return saveFlow(btn, true); }

    // ------------------------------------------------------------------------------ history

    var HCOLS = [{ key: 'GpDate', fmt: 'date' }, { key: 'GpSrNo', fmt: 'int' }, { key: 'VehicleNo' }, { key: 'FactoryWeight', fmt: 'num', sum: true },
        { key: 'Container1No' }, { key: 'Container2No' }, { key: 'EntryDate', fmt: 'datetime', mmm: false }, { key: 'EntryUser' }, { key: 'ModifyDate', fmt: 'datetime', mmm: false }, { key: 'ModifyUser' }];
    function histRender() {
        F.drawGrid('histBody', 'histFoot', HIST.rows, HCOLS, HIST.cur, function (r, i) {
            return '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="GpSlip" data-i="' + i + '">GpSlip</button></td>' +
                '<td class="win-cell-btn"><button type="button" class="win-edit" data-btn="Edit" data-i="' + i + '">Edit</button></td>';
        }, { leadCols: 2 });
        show('histEmpty', HIST.rows.length === 0);
    }
    function historyShow(btn) {
        return busy(btn, function () {
            return postJson(API + '/history', { fromChecked: F.checked('FromDateHistoryChk'), toChecked: F.checked('ToDateHistoryChk'),
                fromDate: val('FromDateHistory'), toDate: val('ToDateHistory'), fromDocNo: netI(val('FromDocNoHistory')), toDocNo: netI(val('ToDocNoHistory')) })
                .then(function (rows) { HIST.rows = rows || []; HIST.cur = -1; histRender(); }).catch(function (e) { box(e.message); });
        });
    }
    /** DataGridHistory_SelectionChanged -> FillHistoryDetailByGpId. */
    function historySelect(i) {
        HIST.cur = i; var r = HIST.rows[i]; if (!r) return;
        getJson(API + '/history-detail?gatePassId=' + r.GatePassOutwardId).then(function (d) {
            var a = d.doc1 || {}, b = d.doc2 || {};
            setText('txtContainer01History', a.ContainerNo); setText('txtSeal01History', a.SealNo); setText('txtInspectedBy01History', a.InspectedBy);
            setText('txtVerifiedBy01History', a.VerifiedBy); setText('txtPicBy01History', a.PictureTakenBy); setText('txtRemarks01History', a.InspectorRemarks);
            $id('chkContainerAccepted01History').checked = !!a.IsContainerAccepted;
            setText('txtContainer02History', b.ContainerNo); setText('txtSeal02History', b.SealNo); setText('txtInspectedBy02History', b.InspectedBy);
            setText('txtVerifiedBy02History', b.VerifiedBy); setText('txtPicBy02History', b.PictureTakenBy); setText('txtRemarks02History', b.InspectorRemarks);
            $id('chkContainerAccepted02History').checked = !!b.IsContainerAccepted;
            renderHist(1, a.rows); renderHist(2, b.rows);
        }).catch(function (e) { box(e.message); });
    }
    /** History "Edit": Gatepassfill(GpId), cmbgatepass.Value = GpId, cmbgatepass_Leave. */
    function historyEdit(i) {
        var r = HIST.rows[i]; if (!r) return;
        getJson(API + '/gate-passes?id=' + r.GatePassOutwardId).then(function (rows) {
            GPS = rows || []; bindGps();
            F.setValOrAdd('cmbgatepass', r.GatePassOutwardId, str(r.GpSrNo));
            return gatePassLeave();
        }).catch(function (e) { box(e.message); });
    }
    function historyReset() {
        setText('FromDateHistory', F.daysAgo(3)); setText('ToDateHistory', F.today()); setText('FromDocNoHistory', ''); setText('ToDocNoHistory', '');
        ['txtContainer01History', 'txtSeal01History', 'txtInspectedBy01History', 'txtPicBy01History', 'txtVerifiedBy01History', 'txtRemarks01History',
            'txtContainer02History', 'txtSeal02History', 'txtInspectedBy02History', 'txtPicBy02History', 'txtVerifiedBy02History', 'txtRemarks02History'].forEach(function (id) { setText(id, ''); });
        HIST.rows = []; HIST.cur = -1; histRender(); renderHist(1, []); renderHist(2, []);
    }
    /** btnPrint_Click: USP_ExImVCITransaction_SlipAndRegister @GpId -> 244-GatePassInspectionSlip.rpt ("Record not found for display" when empty). */
    function print(btn) {
        var gp = netI(val('cmbgatepass'));
        if (global.CrystalPrint) return global.CrystalPrint.open('exp-244', { gpId: gp }, btn);
        F.printSeeded('244-gatepassinspectionslip', { gatepass: gp });
    }
    /** GpSlip -> CommonServices.OutwardGatePassWithWb(GpId): 258-OutwardGatePassWithWbAndLabSlip.rpt (+ WbTransationByOutwardGPID sub-report). */
    function gpSlip(i) { var r = HIST.rows[i]; if (!r) return; if (global.CrystalPrint) global.CrystalPrint.open('exp-258-gp', { id: r.GatePassOutwardId }); }
    function attachment(n) { box('Attachments Container ' + n + ' (DMS popup) are not part of the web port.'); }
    function shortcuts() {
        F.shortcutPopup([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+F5', 'For Focus on GatePass'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Grd Left'], ['Ctrl+ArrowUp', 'For Focus On GatePass in GatePass Inspection'], ['Ctrl+ArrowRight', 'For Focus On Grid Right'],
            ['Ctrl+Enter', 'When Focus On Any Grid For Update Record']]);
    }
    function tab(id) { F.innerTab('main', id, 'btnVciFooterHistory', function (p, onHist) { focus(onHist ? 'FromDateHistory' : 'cmbgatepass'); }); }
    function toggleHistory() { tab(F.activeInner('main') === 'tabHistory' ? 'tabPage1' : 'tabHistory'); }

    document.addEventListener('DOMContentLoaded', function () {
        F.wireTabs(function (p, onHist) { focus(onHist ? 'FromDateHistory' : 'cmbgatepass'); }, 'btnVciFooterHistory');
        F.on('cmbgatepass', 'change', gatePassLeave);
        F.wireGrid('doc1Body', { cell: function (i, k, v) { var r = DOC1[i]; if (r) r[k] = k === 'Status' ? !!v : v; } });
        F.wireGrid('doc2Body', { cell: function (i, k, v) { var r = DOC2[i]; if (r) r[k] = k === 'Status' ? !!v : v; } });
        F.wireGrid('histBody', { select: historySelect, btn: function (name, i) { if (name === 'GpSlip') gpSlip(i); else if (name === 'Edit') historyEdit(i); } });
        /* GatePassInspection_KeyDown / DataGridHistory_KeyDown */
        document.addEventListener('keydown', function (e) {
            if (F.enterMoves(e)) return;
            var k = (e.key || '').toLowerCase(), onForm = F.activeInner('main') !== 'tabHistory';
            if (e.ctrlKey && k === 't') { e.preventDefault(); toggleHistory(); return; }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); F.cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (onForm) {
                if (e.ctrlKey && k === 's' && !$id('btnsave').classList.contains('is-hidden') && PERM.Save) { e.preventDefault(); save($id('btnsave')); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); resetMain(); }
                if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
                if (e.ctrlKey && k === 'u' && !$id('btnupdate').classList.contains('is-hidden') && PERM.Update) { e.preventDefault(); update($id('btnupdate')); }
                if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); focus('cmbgatepass'); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var c = $id('doc1Body').querySelector('input'); if (c) c.focus(); }
                if (e.ctrlKey && e.key === 'ArrowRight') { e.preventDefault(); var inDoc1 = e.target && e.target.closest && e.target.closest('#doc1Body'); var t = $id(inDoc1 ? 'doc2Body' : 'doc1Body').querySelector('input'); if (t) t.focus(); }
                if (e.ctrlKey && (e.key === 'F10' || e.key === 'F11')) { e.preventDefault(); attachment(e.key === 'F10' ? 1 : 2); }
                if (e.ctrlKey && k === 'p') { e.preventDefault(); print($id('btnPrint')); }
            } else {
                if (e.ctrlKey && k === 's') { e.preventDefault(); historyShow($id('btnShowHistory')); }
                if (e.ctrlKey && k === 'n') { e.preventDefault(); historyReset(); }
                if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); focus('FromDateHistory'); }
                if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); var tr = $id('histBody').querySelector('tr'); if (tr) tr.scrollIntoView(); }
                if (e.ctrlKey && e.key === 'End' && HIST.cur >= 0) { e.preventDefault(); historyEdit(HIST.cur); }
                if (e.ctrlKey && k === 'p' && HIST.cur >= 0) { e.preventDefault(); gpSlip(HIST.cur); }
                if (e.ctrlKey && e.key === ' ' && HIST.cur >= 0) { e.preventDefault(); historyEdit(HIST.cur); }
            }
        });
        load();
    });

    global.ExportVCI = { resetMain: resetMain, refresh: refresh, save: save, update: update, attachment: attachment, shortcuts: shortcuts, print: print,
        historyReset: historyReset, historyShow: historyShow, toggleHistory: toggleHistory };
}(window));
