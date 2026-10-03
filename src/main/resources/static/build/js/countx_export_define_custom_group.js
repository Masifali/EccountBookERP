/* ============================================================================================
 * countx_export_define_custom_group.js - DefineCustomGroup.cs (Architecture.WinApp.SDT), screen 621
 * "Define Custom Group". Data: /api/export/define-custom-group.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var S = global.ExportSdt;
    var API = '/api/export/define-custom-group';
    var $id = S.$id, box = S.box, ask = S.ask, netI = S.netI;

    var PERM = { Save: true, Update: true };
    var RecId = 0, HIST = [], CUR = -1;

    /** DefineCustomGroup_Load. */
    function load() {
        return S.getJson(API + '/setup').then(function (d) {
            d = d || {};
            if (d.permissions) PERM = { Save: d.permissions.Save !== false, Update: d.permissions.Update !== false };
            $id('btnsave').disabled = !PERM.Save;
            $id('btnUpdate').disabled = !PERM.Update;
            S.show('btnsave', true); S.show('btnUpdate', false);
            HIST = d.history || []; render();
            $id('chk').checked = true;
            S.focus('txtCustomGroup');
        }).catch(function (e) { box(e.message); });
    }
    var COLS = [
        { key: 'CustomGroup', link: true }, { key: 'Active', kind: 'bool' }, { key: 'EntryUser' }, { key: 'EntryDate', kind: 'dt', mode: 'mm' },
        { key: 'ModifyUser' }, { key: 'ModifyDate', kind: 'dt', mode: 'mm' }
    ];
    function render() { S.drawGrid('histBody', null, HIST, COLS, { cur: CUR, edit: true }); S.show('histEmpty', HIST.length === 0); }
    function bindGrid() { return S.getJson(API + '/history').then(function (rows) { HIST = rows || []; CUR = -1; render(); }); }

    /** grdcropyear_DoubleClick / Edit button -> ReadbyId. */
    function readById(i) {
        var r = HIST[i]; if (!r) return;
        S.getJson(API + '/by-id?id=' + netI(r.Id)).then(function (d) {
            RecId = netI(d.Id);
            S.show('btnsave', false); S.show('btnUpdate', true);
            S.setText('txtCustomGroup', d.customGroupName);
            $id('chk').checked = !!d.isActive;
        }).catch(function (e) { box(e.message); });
    }
    /** Insert(). */
    function insert(btn) {
        return S.busy(btn, function () {
            if (!S.val('txtCustomGroup').trim()) { box('Crop Year Field Required'); S.focus('txtCustomGroup'); return; }
            if (!ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            return S.postJson(API + '/save', { recId: RecId, customGroupName: S.val('txtCustomGroup'), isActive: $id('chk').checked })
                .then(function (d) { box(d.message); return formRefresh(); });
        });
    }
    function save(btn, isUpdate) { if (!isUpdate) RecId = 0; return insert(btn); }
    /** formReFresh - the desktop keeps RecId (only btnsave_Click zeroes it). */
    function formRefresh() {
        S.setText('txtCustomGroup', '');
        S.show('btnsave', true); S.show('btnUpdate', false);
        return bindGrid().then(function () { S.focus('txtCustomGroup'); });
    }
    function reset() { formRefresh(); }
    function toggleHistory() { $id('grdcropyear').scrollIntoView({ behavior: 'smooth' }); }
    function shortcuts() {
        S.shortcutKeys([['Ctrl+S', 'For Save When on Entry form and For Show Data when on History Form'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'],
            ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Vehicle Type'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On on Vehicle Type'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        S.wireFullscreen();
        S.wireGrid('histBody', { select: function (i) { CUR = i; }, open: readById, edit: readById });
        document.addEventListener('keydown', function (e) {
            if (S.enterMovesOn(e)) return;
            var k = (e.key || '').toLowerCase();
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); S.cancel(); return; }
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); return; }
            if (e.ctrlKey && k === 's' && !$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled) { e.preventDefault(); save($id('btnsave'), false); return; }
            if (e.ctrlKey && k === 'u' && !$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) { e.preventDefault(); save($id('btnUpdate'), true); return; }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); $id('grdcropyear').scrollIntoView(); return; }
            if (e.ctrlKey && (e.key === 'ArrowUp' || e.key === 'F5')) { e.preventDefault(); S.focus('txtCustomGroup'); return; }
            if (e.ctrlKey && (e.key === 'Enter' || e.key === ' ') && CUR >= 0) { e.preventDefault(); readById(CUR); }
        });
        load();
    });

    global.ExportCustomGroup = { reset: reset, save: save, toggleHistory: toggleHistory, shortcuts: shortcuts };
}(window));
