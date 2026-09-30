/* ============================================================================================
 * countx_prod_define_type.js - DefineProductionType.cs and DefineProductionPlanType.cs
 * (Architecture.WinApp.Production). The two forms are the same code apart from the BLL class and
 * the description column; the page's <body> says which: data-api, data-desc-col, data-form.
 *
 * DefineCountry_Load :194      DefineGridFill, focus txtDescription.
 * DefineGridFill :76           GetAll(new X()) -> dt(Id, Description); Id hidden.
 * FormValidation :122          "Description Field Required".
 * save() :46                   Save { Code = Description = txtDescription.Text.Trim() }; success >= 1 ->
 *                              "Record Save Successfully...[n]"; DefineGridFill; clear. Buttons unchanged.
 * grdcountrydefine_DoubleClick :100  Save hidden, Update shown, RecId = Cells[0], GetAll(Id) -> textbox.
 * btnUpdate_Click :119         Save { Id = RecId, ... }; "Record Update Successfully...[n]"; DefineGridFill;
 *                              clear; Update hidden, New and Save shown.
 * btnnew_Click :185            clear; Save shown, Update hidden.
 * DefineCountry_KeyDown :164   KeyPreview = true: Ctrl+N New, Ctrl+S Save, Ctrl+U Update, Ctrl+E / Esc Close.
 *
 * Reproduced as written: after a plain Save the Update button stays as it was; New does not reset
 * RecId, so Ctrl+U after New updates the last row read (the update button is hidden, the key is not).
 * ============================================================================================ */
(function () {
    'use strict';

    var body = document.body;
    var api = body.getAttribute('data-api');
    var DESC = body.getAttribute('data-desc-col') || 'Description';

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
    /** Conversion.ToInt. */
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
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }

    // ------------------------------------------------------------------------------ state

    var RecId = 0;
    var GRID = [];      // dt(Id, Description)
    var CUR = -1;

    // ------------------------------------------------------------------------------ grid

    /** DefineGridFill :76. */
    function defineGridFill() {
        return getJson(api + '/list').then(function (rows) {
            GRID = (rows || []).map(function (r) { return { Id: col(r, 'Id'), Description: col(r, DESC) }; });
            CUR = -1;
            draw();
        }).catch(function (e) { box(e.message); });
    }
    function draw() {
        $id('gridBody').innerHTML = GRID.map(function (r, i) {
            return '<tr data-i="' + i + '"' + (i === CUR ? ' class="is-current"' : '') + '>'
                + '<td style="display:none;">' + esc(r.Id) + '</td><td>' + esc(r.Description) + '</td></tr>';
        }).join('');
    }

    // ------------------------------------------------------------------------ Save / Update

    /** FormValidation :122. */
    function formValidation() {
        if ($id('txtDescription').value.trim() === '') {
            box('Description Field Required');
            $id('txtDescription').focus();
            return false;
        }
        return true;
    }
    /** countryformReFresh :71. */
    function refresh() { $id('txtDescription').value = ''; }

    /** save() :46. */
    function btnsave(btn) {
        try {
            if (!formValidation()) return;
            return busy(btn || 'btnsave', function () {
                return postJson(api + '/save', { id: 0, description: $id('txtDescription').value.trim() })
                    .then(function (d) {
                        var n = netI(d && d.id);
                        if (n >= 1) box('Record Save Successfully...[' + n + ']');
                        return defineGridFill().then(refresh);
                    }).catch(function (e) { box(e.message); });
            });
        } catch (e) { box(e.message); }
    }

    /** btnUpdate_Click :119. */
    function btnUpdate(btn) {
        try {
            if (!formValidation()) return;
            return busy(btn || 'btnUpdate', function () {
                return postJson(api + '/save', { id: RecId, description: $id('txtDescription').value.trim() })
                    .then(function (d) {
                        var n = netI(d && d.id);
                        if (n >= 1) box('Record Update Successfully...[' + n + ']');
                        return defineGridFill().then(function () {
                            refresh();
                            show('btnUpdate', false); show('btnnew', true); show('btnsave', true);
                        });
                    }).catch(function (e) { box(e.message); });
            });
        } catch (e) { box(e.message); }
    }

    /** grdcountrydefine_DoubleClick :100. */
    function rowDoubleClick(i) {
        try {
            var r = GRID[i];
            if (!r) return;
            show('btnsave', false); show('btnUpdate', true);
            RecId = netI(r.Id);
            return getJson(api + '/by-id?id=' + encodeURIComponent(RecId)).then(function (rows) {
                var row = (rows || [])[0];
                if (!row) throw new Error('Record not found.');
                $id('txtDescription').value = String(col(row, DESC) === null ? '' : col(row, DESC));
            }).catch(function (e) { box(e.message); });
        } catch (e) { box(e.message); }
    }

    /** btnnew_Click :185. */
    function btnnew() {
        $id('txtDescription').value = '';
        show('btnsave', true); show('btnUpdate', false);
    }
    /** btnclose_Click / Ctrl+E / Esc - Close(). */
    function close() {
        window.close();
        setTimeout(function () { if (!window.closed) { if (history.length > 1) history.back(); else location.href = '/production'; } }, 150);
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
        /* DefineCountry_KeyDown :164 - KeyPreview = true, so the keys work wherever the focus is. */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnnew(); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); btnsave(); }
            if (e.ctrlKey && k === 'u') { e.preventDefault(); btnUpdate(); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); close(); }
        });
        /* DefineCountry_Load :194. */
        defineGridFill().then(function () { $id('txtDescription').focus(); });
    }

    window.ProdDefine = { btnnew: btnnew, btnsave: btnsave, btnUpdate: btnUpdate, close: close };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
