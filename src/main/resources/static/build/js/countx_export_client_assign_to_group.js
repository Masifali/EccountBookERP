/* ============================================================================================
 * countx_export_client_assign_to_group.js - frmClientCustomGroup.cs (Architecture.WinApp.SDT), screen 622
 * "Client Assign To Group". Data: /api/export/client-assign-to-group.
 * The desktop form has no history grid: the "Allocated (Active)" grid is the saved state; the footer
 * History button scrolls to it.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var S = global.ExportSdt;
    var API = '/api/export/client-assign-to-group';
    var $id = S.$id, box = S.box, netI = S.netI;

    var PERM = { Save: true, Update: true };
    var UN = [], AL = [];

    /** frmItemsCustomizedGroups_Load. */
    function load() {
        return S.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
            $id('BtnAllocateItems').disabled = !PERM.Save;
            $id('btnDeAllocate').disabled = !PERM.Update;
            S.bind('CmbCutomGroup', d.customGroups || [], 'Id', 'name', []);
        }).catch(function (e) { box(e.message); });
    }
    /** btnRefresh_Click -> CustomGroupBind (selection retained). */
    function refresh(btn) {
        return S.busy(btn, function () { return S.getJson(API + '/custom-groups').then(function (rows) { S.bind('CmbCutomGroup', rows || [], 'Id', 'name', []); }); });
    }
    var COLS = [{ key: 'Suppliercustomer' }];
    function render() {
        S.drawGrid('unBody', null, UN, COLS, { select: true }); S.show('unEmpty', UN.length === 0);
        S.drawGrid('alBody', null, AL, COLS, { select: true }); S.show('alEmpty', AL.length === 0);
    }
    function fill() {
        return S.getJson(API + '/show?customGroupId=' + netI(S.val('CmbCutomGroup'))).then(function (d) {
            UN = d.unallocated || []; AL = d.allocated || []; render();
        });
    }
    /** BtnShow_Click. */
    function show(btn) {
        return S.busy(btn, function () {
            if (netI(S.val('CmbCutomGroup')) <= 0) { box('Select Custom Group First'); return; }
            return fill();
        });
    }
    /** BtnAllocateItems_Click / btnDeAllocate_Click -> Insert(grd, Active). */
    function post(btn, active) {
        return S.busy(btn, function () {
            if (netI(S.val('CmbCutomGroup')) === 0) { box('Select Custom Group First'); S.focus('CmbCutomGroup'); return; }
            var src = active ? UN : AL;
            var rows = src.filter(function (r) { return r._checked; });
            if (rows.length === 0) throw new Error(active ? "Check Row's first To Allocate" : "Check Row's first To UnAllocate");
            return S.postJson(API + (active ? '/allocate' : '/unallocate'), { customGroupId: netI(S.val('CmbCutomGroup')), rows: rows })
                .then(function (d) { box(d.message); return fill(); });
        });
    }
    function allocate(btn) { return post(btn, true); }
    function unallocate(btn) { return post(btn, false); }
    /** BtnNew_Click. */
    function reset() { UN = []; AL = []; render(); S.setVal('CmbCutomGroup', 0); S.focus('CmbCutomGroup'); }
    function toggleHistory() { $id('GridAllocated').scrollIntoView({ behavior: 'smooth' }); }

    document.addEventListener('DOMContentLoaded', function () {
        S.wireFullscreen();
        S.wireGrid('unBody', { sel: function (i, on) { if (UN[i]) UN[i]._checked = on; } });
        S.wireGrid('alBody', { sel: function (i, on) { if (AL[i]) AL[i]._checked = on; } });
        S.wireHeaderSelector('unSelTh', function () { return UN; }, render);
        S.wireHeaderSelector('alSelTh', function () { return AL; }, render);
        document.addEventListener('keydown', function (e) {
            if (S.enterMovesOn(e)) return;
            var k = (e.key || '').toLowerCase();
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); S.cancel(); return; }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); return; }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); S.focus('CmbCutomGroup'); return; }
            if (e.ctrlKey && k === 'a') { e.preventDefault(); allocate($id('BtnAllocateItems')); return; }
            if (e.ctrlKey && k === 'd') { e.preventDefault(); unallocate($id('btnDeAllocate')); return; }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); $id('GridUnAllocated').scrollIntoView(); }
        });
        load();
    });

    global.ExportClientCustomGroup = { reset: reset, refresh: refresh, show: show, allocate: allocate, unallocate: unallocate, toggleHistory: toggleHistory };
}(window));
