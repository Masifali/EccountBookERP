/* ============================================================================================
 * countx_prod_define_plant.js - DefineProductionPlant.cs (Architecture.WinApp.Production), "Define Plant".
 *
 * DefineCountry_Load :215      dt(Id, Code, Description, BranchName); GrdSetting; DefineGridFill; BranchFill;
 *                              focus txtcountrycode; Save/SaveF shown, Update/UpdateF hidden.
 * DefineGridFill :96           InvProductionPlant.GetAll -> ONLY when rows came back: dt.Clear() and refill.
 *                              An empty answer leaves the grid as it was.
 * BranchFill :232              CommonServices.BrancheServiceBind -> BindDDLNew(Id, BranchName); Rows[0].Activate().
 * FormValidation :122          "Code Field Required", "Description Field Required", "Branch Field Required".
 * save() :55                   Save { Code, Description (as typed), ActionId 0, Org, Company, BranchId = cmb.Value };
 *                              success >= 1 -> "Record Save Successfully...[n]"; DefineGridFill; clear the two boxes.
 * grdcountrydefine_DoubleClick :139  Save/SaveF hidden, Update/UpdateF shown; RecId = Cells[0]; GetById ->
 *                              Code, Description, cmbbranch.Text = BranchName.
 * btnUpdate_Click :162         Save { Id = RecId, ... }; "Record Update Successfully...[n]"; DefineGridFill; clear;
 *                              New, Save, SaveF shown; Update, UpdateF hidden.
 * btnnew_Click :264            clear the two boxes; Save/SaveF shown; Update/UpdateF hidden.
 * btnWarehouseAllocation_Click :262  Admin or View right of frmWarehousesAllocationToPlant -> Show(); else
 *                              "You Dont Have rights View Of This Form..".
 * DefineCountry_KeyDown :247   KeyPreview = true: Ctrl+N, Ctrl+S, Ctrl+U, Ctrl+E / Esc Close.
 *
 * Database note: as dumped on 23-Sep-2026, Sp_InvProductionPlant_Insert / _Update start with
 * RAISERROR('Record cannot be inserted' / 'Record cannot be updated') and RETURN. The desktop shows that
 * text in its MessageBox; this page shows the same text from the server's error message.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/master-data/plant';

    function $id(id) { return document.getElementById(id); }
    function box(m) { window.alert(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    function netI(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? Math.round(v) : 0;
        var s = String(v).trim();
        return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0;
    }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            return b;
        });
    }
    function getJson(url) { return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse); }
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    function refreshCombos() { if (window.DesktopCombo) window.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function hasSel(id) { var s = $id(id); return !!s && s.value !== '0' && s.value !== ''; }
    /** UltraCombo.Text = x: the first row whose BranchName is x becomes active; no match -> no row. */
    function setText(id, text) {
        var s = $id(id), t = String(text === null || text === undefined ? '' : text);
        if (!s) return;
        for (var i = 0; i < s.options.length; i++) if (t !== '' && s.options[i].value !== '0' && s.options[i].textContent === t) { s.selectedIndex = i; return; }
        s.value = '0';
    }

    // ------------------------------------------------------------------------------ state

    var RecId = 0;
    var GRID = [];      // dt(Id, Code, Description, BranchName)
    var CUR = -1;

    // ------------------------------------------------------------------------------ Load

    /** DefineCountry_Load :215. */
    function load() {
        return getJson(api + '/setup').then(function (d) {
            d = d || {};
            if (d.plantsError) box(d.plantsError); else fillGrid(d.plants || []);
            /* BranchFill :232 - bound only when rows came back; the first row is activated. */
            if (d.branchesError) box(d.branchesError);
            else if ((d.branches || []).length) {
                var sel = $id('cmbbranch'), html = '<option value="0"></option>';
                d.branches.forEach(function (b) { html += '<option value="' + esc(col(b, 'Id')) + '">' + esc(col(b, 'BranchName')) + '</option>'; });
                sel.innerHTML = html;
                sel.selectedIndex = 1;
                refreshCombos();
            }
        }).catch(function (e) { box(e.message); }).then(function () {
            $id('txtcountrycode').focus();
            saveMode(true);
        });
    }
    function saveMode(save) {
        show('btnsave', save); show('btnUpdate', !save);
        show('BtnSavef', save); show('btnUpdateF', !save);
    }

    // ------------------------------------------------------------------------------ grid

    /** DefineGridFill :96 - the grid changes only when the procedure returned rows. */
    function defineGridFill() {
        return getJson(api + '/list').then(function (rows) { fillGrid(rows || []); })
            .catch(function (e) { box(e.message); });
    }
    function fillGrid(rows) {
        if (!rows.length) return;                                   // lst.Rows.Count > 0 guard
        GRID = rows.map(function (r) {
            return { Id: col(r, 'Id'), Code: String(col(r, 'Code') === null ? '' : col(r, 'Code')),
                     Description: col(r, 'Description'), BranchName: col(r, 'BranchName') };
        });
        CUR = -1;
        draw();
    }
    function draw() {
        $id('gridBody').innerHTML = GRID.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === CUR ? ' class="is-current"' : '') + '>'
                + '<td style="display:none;">' + esc(r.Id) + '</td><td>' + esc(r.Code) + '</td><td>' + esc(r.Description) + '</td><td>' + esc(r.BranchName) + '</td></tr>';
        }).join('');
    }

    // ------------------------------------------------------------------------ Save / Update

    /** FormValidation :122. */
    function formValidation() {
        if ($id('txtcountrycode').value.trim() === '') { box('Code Field Required'); $id('txtcountrycode').focus(); return false; }
        if ($id('txtcountryName').value.trim() === '') { box('Description Field Required'); $id('txtcountryName').focus(); return false; }
        if (!hasSel('cmbbranch')) { box('Branch Field Required'); $id('cmbbranch').focus(); return false; }
        return true;
    }
    /** countryformReFresh :88. */
    function refresh() { $id('txtcountrycode').value = ''; $id('txtcountryName').value = ''; }
    function payload(id) {
        return { id: id, code: $id('txtcountrycode').value, description: $id('txtcountryName').value, branchId: netI($id('cmbbranch').value) };
    }

    /** save() :55. */
    function btnsave(btn) {
        try {
            if (!formValidation()) return;
            return busy(btn || 'btnsave', function () {
                return postJson(api + '/save', payload(0)).then(function (d) {
                    var n = netI(d && d.id);
                    if (n >= 1) box('Record Save Successfully...[' + n + ']');
                    return defineGridFill().then(refresh);
                }).catch(function (e) { box(e.message); });
            });
        } catch (e) { box(e.message); }
    }

    /** btnUpdate_Click :162. */
    function btnUpdate(btn) {
        try {
            if (!formValidation()) return;
            return busy(btn || 'btnUpdate', function () {
                return postJson(api + '/save', payload(RecId)).then(function (d) {
                    var n = netI(d && d.id);
                    if (n >= 1) box('Record Update Successfully...[' + n + ']');
                    return defineGridFill().then(function () {
                        refresh();
                        show('btnnew', true);
                        saveMode(true);
                    });
                }).catch(function (e) { box(e.message); });
            });
        } catch (e) { box(e.message); }
    }

    /** grdcountrydefine_DoubleClick :139. */
    function rowDoubleClick(i) {
        try {
            var r = GRID[i];
            if (!r) return;
            saveMode(false);
            RecId = netI(r.Id);
            return getJson(api + '/by-id?id=' + encodeURIComponent(RecId)).then(function (rows) {
                var row = (rows || [])[0];
                if (!row) throw new Error('Record not found.');
                $id('txtcountrycode').value = String(col(row, 'Code') === null ? '' : col(row, 'Code'));
                $id('txtcountryName').value = String(col(row, 'Description') === null ? '' : col(row, 'Description'));
                setText('cmbbranch', col(row, 'BranchName'));
                refreshCombos();
            }).catch(function (e) { box(e.message); });
        } catch (e) { box(e.message); }
    }

    /** btnnew_Click :264. */
    function btnnew() {
        refresh();
        saveMode(true);
    }
    /** btnclose_Click / BtnCancel / Ctrl+E / Esc - Close(). */
    function cancel() {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/production'; } }, 150);
    }
    /** btnWarehouseAllocation_Click :262. */
    function btnWarehouseAllocation(btn) {
        return busy(btn || 'btnWarehouseAllocation', function () {
            return getJson(api + '/warehouse-allocation-right').then(function (d) {
                if (!d || !d.allowed) { box('You Dont Have rights View Of This Form..'); return; }
                if (d.ported === false) { box((d.screen || 'frmWarehousesAllocationToPlant') + ' is not available in the web application yet.'); return; }
                window.open('/production/warehouses-allocation-to-plant', '_blank');
            }).catch(function (e) { box(e.message); });
        });
    }

    // ------------------------------------------------------------------------------ boot

    function boot() {
        var gb = $id('gridBody');
        gb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            CUR = +tr.getAttribute('data-i');
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (tr) rowDoubleClick(+tr.getAttribute('data-i'));
        });
        /* DefineCountry_KeyDown :247 - KeyPreview = true. */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnnew(); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); btnsave(); }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); btnUpdate(); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); }
        });
        load();
    }

    window.ProdPlant = { btnnew: btnnew, btnsave: btnsave, btnUpdate: btnUpdate, cancel: cancel, btnWarehouseAllocation: btnWarehouseAllocation };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
