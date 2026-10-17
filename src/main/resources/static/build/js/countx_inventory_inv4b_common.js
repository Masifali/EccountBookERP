/* Shared page helpers for the Inventory hub "Inventory Defination" definition screens of batch inv4b
   (94 CommissionItems, 95 CustomerCommissionPolicy, 96 CustomerDiscountPolicy, 98 DiscountPolicyForParty,
   114 InvDeffrmItemItemUom). Needs countx_store_common.js (window.StoreCommon). */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc;
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    function fmt(v, places) {
        var n = Number(v) || 0;
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: places == null ? 3 : places });
    }
    /* "dd-MMM-yyyy" of a "yyyy-MM-dd[ HH:mm:ss]" string */
    function dmy(s) {
        if (!s) return '';
        var p = String(s).substring(0, 10).split('-');
        if (p.length < 3) return String(s);
        return p[2] + '-' + MON[(+p[1]) - 1] + '-' + p[0];
    }
    /* "dd-MMM-yyyy hh:mm tt" of a "yyyy-MM-dd HH:mm:ss" string */
    function dmyHm(s) {
        if (!s) return '';
        s = String(s);
        var d = dmy(s), t = s.length >= 16 ? s.substring(11, 16) : '';
        if (!t) return d;
        var h = +t.substring(0, 2), m = t.substring(3, 5), tt = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return d + ' ' + (h < 10 ? '0' + h : h) + ':' + m + ' ' + tt;
    }

    /** DropDownBind.BindDDLNew / BindDDL: optional default row (value 0), then the rows; keeps the selection when possible. */
    function fillSelect(id, rows, defaultText) {
        var el = $id(id), keep = el.value;
        el.innerHTML = '';
        if (defaultText !== null && defaultText !== undefined) el.appendChild(new Option(defaultText, '0'));
        (rows || []).forEach(function (r) { el.appendChild(new Option(String(r.Name == null ? '' : r.Name), String(r.Id))); });
        el.value = keep;
        if (el.value !== keep) el.selectedIndex = el.options.length ? 0 : -1;
    }
    function selText(id) { var el = $id(id); return el.selectedIndex < 0 ? '' : el.options[el.selectedIndex].text; }

    /**
     * A Janus GridEX substitute: caption row + automatic filter row (FilterMode 2), optional Edit button column at
     * position 0, hidden columns, right-aligned numbers, TotalRow sums, record navigator text, row selection.
     * cols: [{ key, cap, hidden, n (right), fmt (fn), sum, btn ('Edit'), check (bool cell) }]
     * opts: { onEdit(row), onDblClick(row), nav (element id), sel (index) }
     */
    function Grid(tableId, cols, opts) {
        this.t = $id(tableId); this.cols = cols; this.opts = opts || {}; this.rows = []; this.filters = {}; this.sel = -1;
        var self = this;
        this.t.addEventListener('input', function (e) {
            var k = e.target.getAttribute('data-flt'); if (!k) return;
            self.filters[k] = e.target.value.trim().toLowerCase(); self.body();
        });
        this.t.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-btn]');
            var tr = e.target.closest('tbody tr[data-i]');
            if (!tr) return;
            var i = +tr.getAttribute('data-i');
            self.select(i);
            if (b && self.opts.onEdit) { e.stopPropagation(); self.opts.onEdit(self.rows[i], i); }
        });
        this.t.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tbody tr[data-i]');
            if (tr && self.opts.onDblClick) self.opts.onDblClick(self.rows[+tr.getAttribute('data-i')]);
        });
    }
    Grid.prototype.visible = function () { return this.cols.filter(function (c) { return !c.hidden; }); };
    Grid.prototype.cell = function (r, c) {
        var v = r[c.key];
        if (c.fmt) return c.fmt(v, r);
        if (c.check) return (v === true || v === 'True' || v === 1) ? 'True' : 'False';
        return v == null ? '' : String(v);
    };
    Grid.prototype.set = function (rows) { this.rows = rows || []; this.sel = -1; this.render(); };
    Grid.prototype.clear = function () { this.rows = []; this.sel = -1; this.render(); };
    Grid.prototype.render = function () {
        var vis = this.visible(), self = this;
        if (!this.rows.length) {   // grd.ClearStructure()
            this.t.tHead.innerHTML = ''; this.t.tBodies[0].innerHTML = ''; this.t.tFoot.innerHTML = '';
            if (this.opts.nav) $id(this.opts.nav).textContent = '';
            return;
        }
        this.t.tHead.innerHTML = '<tr>' + vis.map(function (c) { return '<th' + (c.width ? ' style="width:' + c.width + 'px"' : '') + '>' + esc(c.cap || c.key) + '</th>'; }).join('') + '</tr>'
            + '<tr class="flt">' + vis.map(function (c) { return c.btn ? '<td></td>' : '<td><input type="text" data-flt="' + esc(c.key) + '" value="' + esc(self.filters[c.key] || '') + '"></td>'; }).join('') + '</tr>';
        this.body();
    };
    Grid.prototype.body = function () {
        var vis = this.visible(), self = this, html = '', shown = 0, tot = {};
        var keys = Object.keys(this.filters).filter(function (k) { return self.filters[k]; });
        this.rows.forEach(function (r, i) {
            var ok = keys.every(function (k) {
                var c = self.cols.filter(function (x) { return x.key === k; })[0];
                return c && self.cell(r, c).toLowerCase().indexOf(self.filters[k]) >= 0;
            });
            if (!ok) return;
            shown++;
            html += '<tr data-i="' + i + '"' + (i === self.sel ? ' class="sel"' : '') + '>' + vis.map(function (c) {
                if (c.btn) return '<td class="ctr"><button type="button" class="cx-link" data-btn="' + esc(c.btn) + '">' + esc(c.btn) + '</button></td>';
                if (c.check) return '<td class="ctr"><input type="checkbox" disabled' + (self.cell(r, c) === 'True' ? ' checked' : '') + '></td>';
                return '<td' + (c.n ? ' class="n"' : '') + '>' + esc(self.cell(r, c)) + '</td>';
            }).join('') + '</tr>';
            vis.forEach(function (c) { if (c.sum) tot[c.key] = (tot[c.key] || 0) + (Number(r[c.key]) || 0); });
        });
        this.t.tBodies[0].innerHTML = html;
        var anySum = vis.some(function (c) { return c.sum; });
        this.t.tFoot.innerHTML = anySum ? '<tr>' + vis.map(function (c) { return c.sum ? '<td class="n">' + esc(fmt(tot[c.key] || 0, 3)) + '</td>' : '<td></td>'; }).join('') + '</tr>' : '';
        if (this.opts.nav) $id(this.opts.nav).textContent = 'Record ' + (this.sel >= 0 ? (this.sel + 1) : 0) + ' of ' + shown + (shown !== this.rows.length ? ' (' + this.rows.length + ' total)' : '');
    };
    Grid.prototype.select = function (i) {
        this.sel = i;
        this.t.tBodies[0].querySelectorAll('tr.sel').forEach(function (x) { x.classList.remove('sel'); });
        var tr = this.t.tBodies[0].querySelector('tr[data-i="' + i + '"]');
        if (tr) tr.classList.add('sel');
        if (this.opts.nav) this.body();
    };
    Grid.prototype.focus = function () {
        var f = this.t.querySelector('tbody tr') || this.t;
        f.setAttribute('tabindex', '-1');
        f.focus();
    };

    /** ShortCutKeyPopUp (MakeShortCutKeys) — a modal table of KeyCombination / Description. */
    function showShortcuts(list) {
        var m = $id('shortcutModal');
        if (!m) {
            m = document.createElement('div');
            m.id = 'shortcutModal'; m.className = 'fx-modal';
            m.innerHTML = '<div class="box"><h3>ShortCut Keys</h3><table><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody id="shortcutRows"></tbody></table>'
                + '<div class="foot"><button type="button" class="fx-btn" id="shortcutClose" style="width:70px;height:24px;">Close</button></div></div>';
            document.body.appendChild(m);
            m.querySelector('#shortcutClose').addEventListener('click', function () { m.classList.remove('is-open'); });
        }
        m.querySelector('#shortcutRows').innerHTML = list.map(function (s) { return '<tr><td>' + esc(s[0]) + '</td><td>' + esc(s[1]) + '</td></tr>'; }).join('');
        m.classList.add('is-open');
    }
    function closeShortcuts() { var m = $id('shortcutModal'); if (m && m.classList.contains('is-open')) { m.classList.remove('is-open'); return true; } return false; }

    /**
     * Insert(): POST /api/validate (FormValidation messages, in the desktop's order) → "Are you sure to Save/Update?"
     * → POST /api/save → alert(message) → after(result).
     */
    function insert(api, body, after) {
        return SC.postJson(api + '/validate', body).then(function () {
            if (!confirm(body.Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return null;
            return SC.postJson(api + '/save', body).then(function (res) {
                if (res && res.message) alert(res.message);
                if (after) after(res);
                return res;
            });
        }).catch(function (e) { alert(e.message); });
    }

    /** Close (Ctrl+E / Esc / Cancel): back to where the screen was opened from. */
    function closeForm() { if (window.opener || history.length <= 1) window.close(); else history.back(); }

    /** CommonServices.OnlytextdecimelFunction — digits and one decimal point only. */
    function decimalOnly(el) {
        el.addEventListener('input', function () {
            var v = el.value.replace(/[^0-9.]/g, ''), i = v.indexOf('.');
            if (i >= 0) v = v.substring(0, i + 1) + v.substring(i + 1).replace(/\./g, '');
            if (v !== el.value) el.value = v;
        });
    }

    window.Inv4b = { fmt: fmt, dmy: dmy, dmyHm: dmyHm, fillSelect: fillSelect, selText: selText, Grid: Grid,
        showShortcuts: showShortcuts, closeShortcuts: closeShortcuts, insert: insert, closeForm: closeForm, decimalOnly: decimalOnly };
})();
