/* ============================================================================================
 * countx_hrm.js - shared client core of every HRM page (dbo.App 12 "HRM").
 * Loaded before the page's own script (countx_hrm_<group>.js) and exposes window.HRM.
 *
 * Buttons   HRM.busy(btn, () => promise)  disables the button AT ONCE, shows the spinner, refuses a
 *           second click (and a second call with the same key) while the request runs, and re-enables
 *           on success AND on failure. Every Save / Update / Delete / Load / Show / Print goes through it.
 * Requests  HRM.get(url, params) / HRM.post(url, body)  JSON, CSRF header, {message} of a 4xx/5xx thrown.
 * Combos    HRM.fill(id, rows, valueKey, textKey, {zero:'...Select Any Value...'})  -> BindDDL / BindDDLNew;
 *           every <select class="dtcombo"> is made searchable by countx_prod_combo.js (DesktopCombo).
 * Grid      new HRM.Grid('tableId', {columns:[...], onDouble, onCode, filterRow, totals})  the GridEX:
 *           check columns, editable cells, a clickable document / code column, filter row, totals,
 *           row colours, current row, keyboard, fullscreen.
 * History   HRM.history({title, columns, load, onPick})  the desktop History popup / tab as a dialog;
 *           HRM.footer(fn) wires the footer's right-hand "History" button.
 * Keys      HRM.keys({ 'ctrl+s': fn, 'ctrl+n': fn, 'ctrl+u': fn, 'esc': HRM.close }) - Form.KeyPreview;
 *           Enter moves to the next field (SendKeys "{TAB}") unless {enterTab:false}.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var doc = global.document;
    var HRM = {};

    // ------------------------------------------------------------------------ dom
    function $id(id) { return typeof id === 'string' ? doc.getElementById(id) : id; }
    HRM.$ = $id;
    HRM.esc = function (s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    };
    HRM.val = function (id) { var e = $id(id); return e ? (e.value === undefined ? '' : e.value) : ''; };
    HRM.setVal = function (id, v) {
        var e = $id(id); if (!e) return;
        e.value = (v === null || v === undefined) ? '' : v;
        if (e.tagName === 'SELECT') HRM.refreshCombos();
    };
    HRM.checked = function (id) { var e = $id(id); return !!(e && e.checked); };
    HRM.check = function (id, on) { var e = $id(id); if (e) e.checked = !!on; };
    HRM.show = function (id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); };
    HRM.visible = function (id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden'); };
    HRM.enable = function (id, on) {
        var e = $id(id); if (!e) return;
        e.disabled = !on;
        var w = e.closest && e.closest('.dtcombo-wrap');
        if (w) w.classList.toggle('is-disabled', !on);
        if (e.tagName === 'SELECT') HRM.refreshCombos();
    };
    HRM.readOnly = function (id, on) { var e = $id(id); if (e) e.readOnly = !!on; };
    HRM.focus = function (id) {
        var e = $id(id); if (!e) return;
        var w = e.tagName === 'SELECT' && e.closest && e.closest('.dtcombo-wrap');
        var t = w ? w.querySelector('.dtcombo-input') : e;
        try { (t || e).focus(); } catch (x) { /* hidden */ }
    };
    HRM.text = function (id, t) { var e = $id(id); if (e) e.textContent = t === null || t === undefined ? '' : t; };

    // ------------------------------------------------------------------------ messages
    HRM.box = function (m) { global.alert(m === null || m === undefined ? '' : String(m)); };
    HRM.ask = function (m) { return global.confirm(m); };
    HRM.fail = function (e) { HRM.box(e && e.message ? e.message : String(e)); };

    // ------------------------------------------------------------------------ requests
    function csrf(h) {
        var t = doc.querySelector('meta[name="_csrf"]'), n = doc.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function parse(r) {
        return r.text().then(function (t) {
            var b = null;
            try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
            if (r.status === 401 || (r.redirected && /login/i.test(r.url))) throw new Error('Your session has expired. Sign in again.');
            if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
            if (b && b.success === false && b.message) throw new Error(b.message);
            return b;
        });
    }
    function qs(params) {
        if (!params) return '';
        var parts = [];
        Object.keys(params).forEach(function (k) {
            var v = params[k];
            if (v === null || v === undefined) return;
            if (Array.isArray(v)) v.forEach(function (x) { parts.push(encodeURIComponent(k) + '=' + encodeURIComponent(x)); });
            else parts.push(encodeURIComponent(k) + '=' + encodeURIComponent(v));
        });
        return parts.length ? '?' + parts.join('&') : '';
    }
    HRM.get = function (url, params) {
        return fetch(url + qs(params), { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' }).then(parse);
    };
    HRM.post = function (url, body) {
        return fetch(url, {
            method: 'POST', credentials: 'same-origin',
            headers: csrf({ 'Accept': 'application/json', 'Content-Type': 'application/json' }),
            body: JSON.stringify(body === undefined ? {} : body)
        }).then(parse);
    };

    // ------------------------------------------------------------------------ busy buttons
    var inflight = {};
    /**
     * Runs fn() with btn disabled and a spinner; ignores the call while the same button (or key) is busy.
     * The button is re-enabled when the promise settles - success or failure - unless the page
     * disabled / hid it meanwhile on purpose (opts.keepDisabled).
     */
    HRM.busy = function (btn, fn, key) {
        var b = $id(btn);
        var k = key || (b && b.id) || null;
        if (b && (b.classList.contains('is-busy'))) return Promise.resolve();
        if (k && inflight[k]) return Promise.resolve();
        var wasDisabled = b ? b.disabled : false;
        if (b) { b.disabled = true; b.classList.add('is-busy'); b.setAttribute('aria-busy', 'true'); }
        if (k) inflight[k] = true;
        doc.body.classList.add('hrm-loading');
        var done = function () {
            if (b) { b.classList.remove('is-busy'); b.removeAttribute('aria-busy'); if (!wasDisabled) b.disabled = false; }
            if (k) delete inflight[k];
            if (!Object.keys(inflight).length) doc.body.classList.remove('hrm-loading');
        };
        var p;
        try { p = fn(); } catch (e) { done(); HRM.fail(e); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
        done();
        return Promise.resolve(p);
    };
    /** Busy flag for work that has no button (page load): shows the top loading bar. */
    HRM.loading = function (promise) {
        doc.body.classList.add('hrm-loading');
        var off = function () { if (!Object.keys(inflight).length) doc.body.classList.remove('hrm-loading'); };
        return Promise.resolve(promise).then(function (v) { off(); return v; }, function (e) { off(); throw e; });
    };

    // ------------------------------------------------------------------------ values
    function col(row, name) {
        if (!row || name === null || name === undefined) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = String(name).toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    HRM.col = col;
    HRM.str = function (v) { return v === null || v === undefined ? '' : String(v); };
    /** Conversion.ToInt */
    HRM.int = function (v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'boolean') return v ? 1 : 0;
        if (typeof v === 'number') return isFinite(v) ? Math.trunc(v) : 0;
        var s = String(v).trim().replace(/,/g, '');
        if (/^[+-]?\d+$/.test(s)) return parseInt(s, 10);
        var f = parseFloat(s); return isFinite(f) && /^[+-]?\d*\.?\d+$/.test(s) ? Math.trunc(f) : 0;
    };
    /** Conversion.ToDecimal / ToDouble */
    HRM.num = function (v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        if (typeof v === 'boolean') return v ? 1 : 0;
        var s = String(v).trim().replace(/,/g, '');
        return /^[+-]?(\d+\.?\d*|\.\d+)([eE][+-]?\d+)?$/.test(s) ? parseFloat(s) : 0;
    };
    HRM.bool = function (v) {
        if (v === true || v === 1) return true;
        var s = String(v === null || v === undefined ? '' : v).trim().toLowerCase();
        return s === 'true' || s === '1' || s === 'yes' || s === 'y';
    };
    HRM.round = function (v, d) { var p = Math.pow(10, d || 0); return Math.round((HRM.num(v) + Number.EPSILON) * p) / p; };
    /** "#,##0.00" style with d decimals; blank for null. */
    HRM.fmtNum = function (v, d, noGroup) {
        if (v === null || v === undefined || v === '') return '';
        var n = HRM.num(v);
        var s = n.toFixed(d === undefined ? 2 : d);
        if (noGroup) return s;
        var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return p.join('.');
    };

    // ------------------------------------------------------------------------ dates
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    HRM.MONTHS = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    function pad(n) { return String(n).padStart(2, '0'); }
    HRM.iso = function (d) { d = d || new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); };
    HRM.today = function () { return HRM.iso(new Date()); };
    HRM.nowTime = function () { var d = new Date(); return pad(d.getHours()) + ':' + pad(d.getMinutes()); };
    /** "yyyy-MM-dd" of any server value (ISO string, epoch millis, Date). */
    HRM.day = function (v) {
        if (v === null || v === undefined || v === '') return '';
        if (v instanceof Date) return HRM.iso(v);
        if (typeof v === 'number') return HRM.iso(new Date(v));
        var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(v); return isNaN(d) ? '' : HRM.iso(d);
    };
    HRM.time = function (v) {
        if (v === null || v === undefined || v === '') return '';
        if (typeof v === 'number') { var d = new Date(v); return pad(d.getHours()) + ':' + pad(d.getMinutes()); }
        var m = String(v).match(/(\d{2}):(\d{2})(?::\d{2})?/);
        return m ? m[1] + ':' + m[2] : '';
    };
    /** dd-MMM-yyyy (the desktop's short date on the grids). */
    HRM.fmtDate = function (v) {
        var d = HRM.day(v); if (!d) return '';
        var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0];
    };
    HRM.fmtDateTime = function (v) {
        var d = HRM.fmtDate(v); if (!d) return '';
        var t = HRM.time(v); return t ? d + ' ' + t : d;
    };
    HRM.addDays = function (iso, n) { var d = new Date(iso + 'T00:00:00'); d.setDate(d.getDate() + n); return HRM.iso(d); };
    HRM.daysBetween = function (a, b) { return Math.round((new Date(b + 'T00:00:00') - new Date(a + 'T00:00:00')) / 86400000); };
    HRM.firstOfMonth = function (iso) { var d = iso ? new Date(iso + 'T00:00:00') : new Date(); return HRM.iso(new Date(d.getFullYear(), d.getMonth(), 1)); };
    HRM.lastOfMonth = function (iso) { var d = iso ? new Date(iso + 'T00:00:00') : new Date(); return HRM.iso(new Date(d.getFullYear(), d.getMonth() + 1, 0)); };

    // ------------------------------------------------------------------------ combos
    HRM.refreshCombos = function () { if (global.DesktopCombo && global.DesktopCombo.refresh) global.DesktopCombo.refresh(); };
    /**
     * BindDDL / BindDDLNew: rows -> <option value=valueKey>textKey</option>.
     * opts.zero: text of a first "0" row (ZeroIndex true -> '...Select Any Value...'); '' for an empty slot;
     *            false for no slot at all (the first row becomes active, as a ComboBox bound without ZeroIndex).
     * opts.keep: keep the current value when it is still in the list.
     * opts.attrs: extra row fields copied onto data-* attributes (for multi-column combos / dependent logic).
     */
    HRM.fill = function (id, rows, valueKey, textKey, opts) {
        var sel = $id(id); if (!sel) return;
        opts = opts || {};
        var keepV = opts.keep ? sel.value : null;
        var zero = opts.zero === undefined ? '' : opts.zero;
        var html = zero === false ? '' : '<option value="0">' + HRM.esc(zero) + '</option>';
        (rows || []).forEach(function (r) {
            var extra = '';
            (opts.attrs || []).forEach(function (a) { extra += ' data-' + a.toLowerCase() + '="' + HRM.esc(col(r, a)) + '"'; });
            var t = typeof textKey === 'function' ? textKey(r) : col(r, textKey);
            html += '<option value="' + HRM.esc(col(r, valueKey)) + '"' + extra + '>' + HRM.esc(t) + '</option>';
        });
        sel.innerHTML = html;
        if (keepV !== null && HRM.hasOption(id, keepV)) sel.value = keepV;
        else if (zero !== false) sel.value = '0';
        else sel.selectedIndex = sel.options.length ? 0 : -1;
        sel._rows = rows || [];
        HRM.refreshCombos();
    };
    /** Fixed items (a combo filled in the designer / Items.Add): [[value, text], ...]. */
    HRM.fillFixed = function (id, pairs, opts) {
        HRM.fill(id, pairs.map(function (p) { return { v: p[0], t: p[1] }; }), 'v', 't', opts || { zero: false });
    };
    HRM.hasOption = function (id, v) {
        var s = $id(id); if (!s) return false;
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value === String(v)) return true;
        return false;
    };
    /** UltraCombo.Value = v : that row, or the "0" slot when v is not in the list. */
    HRM.setCombo = function (id, v) {
        var s = $id(id); if (!s) return;
        var x = v === null || v === undefined ? '0' : String(v);
        if (HRM.hasOption(id, x)) s.value = x;
        else if (HRM.hasOption(id, '0')) s.value = '0';
        else s.selectedIndex = -1;
        HRM.refreshCombos();
    };
    /** Selects by the visible text (Combo.Text = x). */
    HRM.setComboText = function (id, t) {
        var s = $id(id); if (!s) return;
        for (var i = 0; i < s.options.length; i++) if (s.options[i].textContent === String(t)) { s.selectedIndex = i; HRM.refreshCombos(); return; }
    };
    HRM.comboVal = function (id) { var s = $id(id); return s && s.selectedIndex >= 0 ? HRM.int(s.value) : 0; };
    HRM.comboText = function (id) {
        var s = $id(id);
        return (!s || s.selectedIndex < 0 || s.value === '0') ? '' : s.options[s.selectedIndex].textContent;
    };
    /** The source row of the selected option (ActiveRow.Cells[...]). */
    HRM.comboRow = function (id, valueKey) {
        var s = $id(id); if (!s || !s._rows) return null;
        var v = s.value;
        for (var i = 0; i < s._rows.length; i++) if (String(col(s._rows[i], valueKey)) === v) return s._rows[i];
        return null;
    };

    // ------------------------------------------------------------------------ grid
    /**
     * new HRM.Grid(tableId, opts)
     *  columns: [{ key, caption, width, hidden, align, type, decimals, sum, editable, options, onChange, render, cls }]
     *     type: 'text' (default) | 'num' | 'int' | 'date' | 'datetime' | 'time' | 'check' (read-only tick)
     *           | 'code' (clickable document / voucher / sample code -> opts.onCode) | 'edit' | 'edit-num'
     *           | 'edit-check' | 'edit-date' | 'edit-time' | 'select' (options: [[v,t],...] or rows + valueKey/textKey)
     *  onDouble(row, index)          grd_DoubleClick
     *  onCode(row, index, column)    the code link (defaults to onDouble)
     *  onSelect(row, index)          CurrentRow changed
     *  onChange(row, key, index)     any editable cell changed (after the row value is written)
     *  rowClass(row) -> 'hrm-row-red' | 'hrm-row-green' | ... (FormatConditions / RowFormatStyle)
     *  filterRow: true               the GridEX filter row
     *  totals: true                  a footer with the sum of every column marked sum:true
     *  checkAll: 'key'               header tick that sets that edit-check column on every visible row
     *  emptyText                     shown when there are no rows
     */
    function Grid(tableId, opts) {
        this.table = $id(tableId);
        this.opts = opts || {};
        this.columns = (this.opts.columns || []).map(function (c) { return Object.assign({ type: 'text' }, c); });
        this.data = [];
        this.view = [];
        this.cur = -1;
        this.filters = {};
        this._build();
    }
    Grid.prototype._build = function () {
        var self = this, t = this.table;
        if (!t) return;
        t.classList.add('win-grid', 'hrm-grid');
        var head = '<tr>' + this.columns.map(function (c) {
            var style = (c.hidden ? 'display:none;' : '') + (c.width ? 'min-width:' + c.width + 'px;width:' + c.width + 'px;' : '');
            var inner = HRM.esc(c.caption === undefined ? c.key : c.caption);
            if (self.opts.checkAll === c.key) inner = '<label class="hrm-checkall"><input type="checkbox" data-checkall="1"> ' + inner + '</label>';
            return '<th data-key="' + HRM.esc(c.key) + '" style="' + style + '"' + (c.align ? ' class="hrm-al-' + c.align + '"' : '') + '>' + inner + '</th>';
        }).join('') + '</tr>';
        if (this.opts.filterRow) {
            head += '<tr class="hrm-filter-row">' + this.columns.map(function (c) {
                return '<th style="' + (c.hidden ? 'display:none;' : '') + '"><input type="text" data-filter="' + HRM.esc(c.key) + '" autocomplete="off" placeholder="&#x1F50D;"></th>';
            }).join('') + '</tr>';
        }
        t.innerHTML = '<thead>' + head + '</thead><tbody></tbody>' + (this.opts.totals ? '<tfoot></tfoot>' : '');
        this.body = t.tBodies[0];
        t.addEventListener('input', function (e) {
            var f = e.target.getAttribute && e.target.getAttribute('data-filter');
            if (f !== null && f !== undefined) { self.filters[f] = e.target.value; self.draw(); }
        });
        t.addEventListener('change', function (e) {
            if (e.target.getAttribute('data-checkall')) {
                var on = e.target.checked, k = self.opts.checkAll;
                self.view.forEach(function (i) {
                    if (self.opts.canCheck && !self.opts.canCheck(self.data[i], k)) return;
                    self.data[i][k] = on;
                    if (self.opts.onChange) self.opts.onChange(self.data[i], k, i);
                });
                self.draw();
                return;
            }
            self._cellChanged(e.target, true);
        });
        this.body.addEventListener('input', function (e) { self._cellChanged(e.target, false); });
        this.body.addEventListener('click', function (e) {
            var a = e.target.closest('a.hrm-code');
            var tr = e.target.closest('tr[data-i]');
            if (!tr) return;
            var i = +tr.getAttribute('data-i');
            self.select(i);
            if (a) {
                e.preventDefault();
                var c = self.columns[+a.getAttribute('data-c')];
                (self.opts.onCode || self.opts.onDouble || function () { })(self.data[i], i, c);
            }
        });
        this.body.addEventListener('dblclick', function (e) {
            if (e.target.closest('input,select,a')) return;
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            self.select(i);
            if (self.opts.onDouble) self.opts.onDouble(self.data[i], i);
        });
        this.body.addEventListener('keydown', function (e) {
            if (e.target.closest('input,select')) return;
            if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
                e.preventDefault();
                var p = self.view.indexOf(self.cur) + (e.key === 'ArrowDown' ? 1 : -1);
                if (p >= 0 && p < self.view.length) self.select(self.view[p], true);
            } else if (e.key === 'Enter' && self.cur >= 0 && self.opts.onDouble) {
                e.preventDefault(); self.opts.onDouble(self.data[self.cur], self.cur);
            }
        });
    };
    Grid.prototype._cellChanged = function (el, commit) {
        var k = el.getAttribute && el.getAttribute('data-k');
        if (k === null || k === undefined) return;
        var tr = el.closest('tr[data-i]'); if (!tr) return;
        var i = +tr.getAttribute('data-i'), c = this.columnOf(k), r = this.data[i];
        if (!c || !r) return;
        if (c.type === 'edit-check') { if (!commit) return; r[k] = el.checked; }
        else if (c.type === 'edit-num') { r[k] = el.value === '' ? null : HRM.num(el.value); if (!commit) { this._totals(); return; } }
        else r[k] = el.value;
        if (!commit && c.type !== 'edit' && c.type !== 'edit-num') return;
        if (commit || c.live) {
            if (c.onChange) c.onChange(r, k, i, el);
            if (this.opts.onChange) this.opts.onChange(r, k, i, el);
            this._totals();
            if (commit && c.redraw) this.draw();
        }
    };
    Grid.prototype.columnOf = function (k) {
        for (var i = 0; i < this.columns.length; i++) if (this.columns[i].key === k) return this.columns[i];
        return null;
    };
    Grid.prototype._cell = function (r, c, i, ci) {
        var v = col(r, c.key), t = c.type, s;
        var style = c.hidden ? ' style="display:none;"' : '';
        var cls = [];
        if (c.align) cls.push('hrm-al-' + c.align);
        else if (t === 'num' || t === 'int' || t === 'edit-num') cls.push('hrm-al-right');
        else if (t === 'check' || t === 'edit-check') cls.push('hrm-al-center');
        if (c.cls) cls.push(typeof c.cls === 'function' ? c.cls(r) : c.cls);
        var ro = c.readOnly && c.readOnly(r, i);
        if (c.render) s = c.render(v, r, i);
        else if (t === 'num') s = HRM.esc(v === null || v === undefined || v === '' ? '' : HRM.fmtNum(v, c.decimals === undefined ? 2 : c.decimals, c.noGroup));
        else if (t === 'int') s = HRM.esc(v === null || v === undefined || v === '' ? '' : HRM.int(v));
        else if (t === 'date') s = HRM.esc(HRM.fmtDate(v));
        else if (t === 'datetime') s = HRM.esc(HRM.fmtDateTime(v));
        else if (t === 'time') s = HRM.esc(HRM.time(v));
        else if (t === 'check') s = '<input type="checkbox" disabled' + (HRM.bool(v) ? ' checked' : '') + '>';
        else if (t === 'code') s = (v === null || v === undefined || v === '') ? '' : '<a href="#" class="hrm-code" data-c="' + ci + '">' + HRM.esc(v) + '</a>';
        else if (t === 'edit-check') s = '<input type="checkbox" data-k="' + HRM.esc(c.key) + '"' + (HRM.bool(v) ? ' checked' : '') + (ro ? ' disabled' : '') + '>';
        else if (t === 'edit' || t === 'edit-num' || t === 'edit-date' || t === 'edit-time') {
            var it = t === 'edit-date' ? 'date' : t === 'edit-time' ? 'time' : 'text';
            var shown = t === 'edit-date' ? HRM.day(v) : t === 'edit-time' ? HRM.time(v) : (v === null || v === undefined ? '' : v);
            s = '<input type="' + it + '" class="hrm-cell-input' + (t === 'edit-num' ? ' hrm-al-right' : '') + '" data-k="' + HRM.esc(c.key) + '" value="' + HRM.esc(shown) + '"' +
                (t === 'edit-num' ? ' inputmode="decimal"' : '') + (ro ? ' readonly' : '') + ' autocomplete="off">';
        } else if (t === 'select') {
            var opts = typeof c.options === 'function' ? c.options(r, i) : (c.options || []);
            s = '<select class="hrm-cell-select" data-k="' + HRM.esc(c.key) + '"' + (ro ? ' disabled' : '') + '>' + opts.map(function (o) {
                var ov = Array.isArray(o) ? o[0] : col(o, c.valueKey), ot = Array.isArray(o) ? o[1] : col(o, c.textKey);
                return '<option value="' + HRM.esc(ov) + '"' + (String(ov) === String(v) ? ' selected' : '') + '>' + HRM.esc(ot) + '</option>';
            }).join('') + '</select>';
        } else s = HRM.esc(v);
        return '<td' + style + (cls.length ? ' class="' + cls.join(' ') + '"' : '') + '>' + s + '</td>';
    };
    Grid.prototype._match = function (r) {
        var f = this.filters;
        for (var k in f) {
            if (!f[k]) continue;
            var c = this.columnOf(k), v = col(r, k);
            var shown = c && c.type === 'date' ? HRM.fmtDate(v) : c && c.type === 'check' ? (HRM.bool(v) ? 'true' : 'false') : HRM.str(v);
            if (shown.toLowerCase().indexOf(String(f[k]).toLowerCase()) < 0) return false;
        }
        return true;
    };
    Grid.prototype.draw = function () {
        var self = this;
        if (!this.body) return;
        this.view = [];
        for (var i = 0; i < this.data.length; i++) if (this._match(this.data[i])) this.view.push(i);
        if (!this.view.length) {
            var vis = this.columns.filter(function (c) { return !c.hidden; }).length || 1;
            this.body.innerHTML = '<tr class="hrm-empty"><td colspan="' + vis + '">' + HRM.esc(this.opts.emptyText || '') + '</td></tr>';
        } else {
            this.body.innerHTML = this.view.map(function (i) {
                var r = self.data[i];
                var rc = self.opts.rowClass ? self.opts.rowClass(r, i) : '';
                var cls = (i === self.cur ? 'is-current ' : '') + (rc || '');
                return '<tr data-i="' + i + '" tabindex="-1"' + (cls.trim() ? ' class="' + cls.trim() + '"' : '') + '>' +
                    self.columns.map(function (c, ci) { return self._cell(r, c, i, ci); }).join('') + '</tr>';
            }).join('');
        }
        this._totals();
        var ca = this.table.querySelector('input[data-checkall]');
        if (ca) {
            var k = this.opts.checkAll;
            ca.checked = this.view.length > 0 && this.view.every(function (i) { return HRM.bool(self.data[i][k]); });
        }
        if (this.opts.onDraw) this.opts.onDraw(this);
    };
    Grid.prototype._totals = function () {
        if (!this.opts.totals || !this.table.tFoot) return;
        var self = this;
        var tf = '<tr>' + this.columns.map(function (c, ci) {
            var style = c.hidden ? ' style="display:none;"' : '';
            if (!c.sum) return '<td' + style + '>' + (ci === 0 || (ci > 0 && self.columns.slice(0, ci).every(function (x) { return x.hidden; })) ? 'Total' : '') + '</td>';
            var s = 0; self.view.forEach(function (i) { s += HRM.num(col(self.data[i], c.key)); });
            return '<td class="hrm-al-right"' + style + '>' + HRM.esc(HRM.fmtNum(s, c.decimals === undefined ? 2 : c.decimals)) + '</td>';
        }).join('') + '</tr>';
        this.table.tFoot.innerHTML = tf;
    };
    Grid.prototype.sum = function (k, onlyVisible) {
        var self = this, s = 0;
        (onlyVisible ? this.view : this.data.map(function (_, i) { return i; })).forEach(function (i) { s += HRM.num(col(self.data[i], k)); });
        return s;
    };
    Grid.prototype.set = function (rows) { this.data = (rows || []).slice(); this.cur = -1; this.draw(); return this; };
    Grid.prototype.rows = function () { return this.data; };
    Grid.prototype.visibleRows = function () { var s = this; return this.view.map(function (i) { return s.data[i]; }); };
    Grid.prototype.add = function (row) { this.data.push(row); this.draw(); return this.data.length - 1; };
    Grid.prototype.update = function (i, row) { if (i >= 0 && i < this.data.length) { this.data[i] = row; this.draw(); } };
    Grid.prototype.remove = function (i) {
        if (i < 0 || i >= this.data.length) return null;
        var r = this.data.splice(i, 1)[0];
        if (this.cur === i) this.cur = -1; else if (this.cur > i) this.cur--;
        this.draw(); return r;
    };
    Grid.prototype.clear = function () { this.set([]); };
    Grid.prototype.current = function () { return this.cur >= 0 ? this.data[this.cur] : null; };
    Grid.prototype.currentIndex = function () { return this.cur; };
    Grid.prototype.checked = function (k) { return this.data.filter(function (r) { return HRM.bool(r[k]); }); };
    Grid.prototype.select = function (i, focus) {
        this.cur = i;
        var trs = this.body.querySelectorAll('tr[data-i]');
        for (var x = 0; x < trs.length; x++) trs[x].classList.toggle('is-current', +trs[x].getAttribute('data-i') === i);
        var tr = this.body.querySelector('tr[data-i="' + i + '"]');
        if (tr && focus) { tr.focus(); tr.scrollIntoView({ block: 'nearest' }); }
        if (this.opts.onSelect && i >= 0) this.opts.onSelect(this.data[i], i);
    };
    Grid.prototype.clearFilters = function () {
        this.filters = {};
        this.table.querySelectorAll('input[data-filter]').forEach(function (e) { e.value = ''; });
        this.draw();
    };
    HRM.Grid = Grid;

    // ------------------------------------------------------------------------ fullscreen table
    function wireFullscreen(root) {
        (root || doc).querySelectorAll('[data-hrm-fullscreen]').forEach(function (btn) {
            if (btn._fsWired) return; btn._fsWired = true;
            btn.addEventListener('click', function () {
                var box = btn.closest('.hrm-grid-box') || $id(btn.getAttribute('data-hrm-fullscreen'));
                if (!box) return;
                var on = !box.classList.contains('is-fullscreen');
                box.classList.toggle('is-fullscreen', on);
                doc.body.classList.toggle('hrm-has-fullscreen', on);
                btn.title = on ? 'Exit full screen (Esc)' : 'Full screen';
                btn.innerHTML = on ? '<i class="fa fa-compress"></i>' : '<i class="fa fa-expand"></i>';
            });
        });
    }
    HRM.wireFullscreen = wireFullscreen;
    function exitFullscreen() {
        var box = doc.querySelector('.hrm-grid-box.is-fullscreen');
        if (!box) return false;
        var b = box.querySelector('[data-hrm-fullscreen]');
        if (b) b.click(); else { box.classList.remove('is-fullscreen'); doc.body.classList.remove('hrm-has-fullscreen'); }
        return true;
    }

    // ------------------------------------------------------------------------ modal / history
    /** A dialog over the page. Returns { el, body, close }. */
    HRM.modal = function (o) {
        o = o || {};
        var wrap = doc.createElement('div');
        wrap.className = 'hrm-modal';
        wrap.innerHTML = '<div class="hrm-modal-box" style="' + (o.width ? 'width:' + o.width + ';' : '') + '">' +
            '<div class="hrm-modal-head"><span>' + HRM.esc(o.title || '') + '</span><span class="hrm-modal-tools">' +
            '<button type="button" class="hrm-modal-max" title="Full screen"><i class="fa fa-expand"></i></button>' +
            '<button type="button" class="hrm-modal-x" title="Close (Esc)">&times;</button></span></div>' +
            '<div class="hrm-modal-body"></div></div>';
        doc.body.appendChild(wrap);
        var box = wrap.querySelector('.hrm-modal-box');
        var body = wrap.querySelector('.hrm-modal-body');
        if (o.html) body.innerHTML = o.html;
        var closed = false;
        function close() { if (closed) return; closed = true; wrap.remove(); doc.removeEventListener('keydown', onKey, true); if (o.onClose) o.onClose(); }
        function onKey(e) { if (e.key === 'Escape') { e.stopPropagation(); e.preventDefault(); close(); } }
        doc.addEventListener('keydown', onKey, true);
        wrap.querySelector('.hrm-modal-x').addEventListener('click', close);
        wrap.querySelector('.hrm-modal-max').addEventListener('click', function () { box.classList.toggle('is-max'); });
        wrap.addEventListener('mousedown', function (e) { if (e.target === wrap && o.backdropClose !== false) close(); });
        wireFullscreen(wrap);
        return { el: wrap, body: body, close: close };
    };
    /**
     * The desktop History screen as a dialog:
     *   HRM.history({ title, columns, load: (filters) => promise rows, onPick: (row) => ..., filters: '<html>', width })
     * columns are Grid columns (a 'code' column opens the record through onPick). A Refresh button reloads;
     * the grid has a filter row, totals where sum:true, and fullscreen.
     */
    HRM.history = function (o) {
        o = o || {};
        var m = HRM.modal({
            title: o.title || 'History', width: o.width || 'min(1200px, 96vw)',
            html: '<div class="hrm-history-bar">' + (o.filters || '') +
                '<button type="button" class="win-btn-action hrm-history-refresh">Refresh</button>' +
                '<span class="hrm-history-count"></span></div>' +
                '<div class="hrm-grid-box hrm-grid-box-modal"><button type="button" class="hrm-fs-btn" data-hrm-fullscreen title="Full screen"><i class="fa fa-expand"></i></button>' +
                '<div class="hrm-grid-wrap"><table class="hrm-history-grid"></table></div></div>'
        });
        var g = new Grid(m.body.querySelector('.hrm-history-grid'), {
            columns: o.columns, filterRow: o.filterRow !== false, totals: !!o.totals, rowClass: o.rowClass,
            emptyText: 'No record found.',
            onDouble: function (r, i) { if (o.onPick) { m.close(); o.onPick(r, i); } },
            onCode: function (r, i) { if (o.onPick) { m.close(); o.onPick(r, i); } }
        });
        var btn = m.body.querySelector('.hrm-history-refresh');
        function load() {
            return HRM.busy(btn, function () {
                return Promise.resolve(o.load(m.body)).then(function (rows) {
                    g.set(rows || []);
                    m.body.querySelector('.hrm-history-count').textContent = (rows || []).length + ' record(s)';
                }).catch(HRM.fail);
            });
        }
        btn.addEventListener('click', load);
        if (o.onReady) o.onReady(m.body, g);
        load();
        return { modal: m, grid: g, reload: load };
    };
    /** Wires the footer's right-hand History button (id btnHistory) to fn. */
    HRM.footer = function (fn) {
        var b = $id('btnHistory');
        if (!b) return;
        if (!fn) { b.classList.add('is-hidden'); return; }
        b.addEventListener('click', function () { fn(b); });
    };

    // ------------------------------------------------------------------------ keys
    /**
     * Form.KeyPreview handlers: HRM.keys({ 'ctrl+s': fn, 'ctrl+n': fn, 'ctrl+u': fn, 'ctrl+e': fn, 'esc': fn, 'f5': fn })
     * Enter moves focus to the next field (the desktop's SendKeys.Send("{TAB}")) unless map.enterTab === false.
     */
    HRM.keys = function (map) {
        map = map || {};
        doc.addEventListener('keydown', function (e) {
            if (e.key === 'Escape' && exitFullscreen()) { e.preventDefault(); return; }
            if (doc.querySelector('.hrm-modal')) return;
            var k = (e.ctrlKey ? 'ctrl+' : '') + (e.altKey ? 'alt+' : '') + (e.shiftKey ? 'shift+' : '') +
                (e.key === 'Escape' ? 'esc' : (e.key || '').toLowerCase());
            var fn = map[k];
            if (fn) { e.preventDefault(); fn(e); return; }
            if (e.key === 'Enter' && map.enterTab !== false && !e.ctrlKey && !e.altKey) {
                var t = e.target;
                if (!t || t.tagName === 'TEXTAREA' || t.tagName === 'BUTTON' || t.closest('.hrm-grid,.dtcombo-pop')) return;
                if (!/^(INPUT|SELECT)$/.test(t.tagName)) return;
                e.preventDefault();
                var f = Array.prototype.filter.call(doc.querySelectorAll('input:not([type=hidden]):not([disabled]):not([readonly]),select:not([disabled]),textarea:not([disabled]),.dtcombo-input'),
                    function (x) { return x.offsetParent !== null && !x.closest('.hrm-grid') && !(x.tagName === 'SELECT' && x.closest('.dtcombo-wrap')); });
                var i = f.indexOf(t);
                if (i >= 0 && i + 1 < f.length) f[i + 1].focus();
            }
        });
    };

    // ------------------------------------------------------------------------ navigation
    HRM.close = function () {
        global.close();
        setTimeout(function () { if (!global.closed) { if (global.history.length > 1) global.history.back(); else global.location.href = '/hrm'; } }, 150);
    };
    /** Form.Show() of another form - a new window, as the desktop opens a second form. */
    HRM.open = function (path) { return global.open(path, '_blank'); };
    /** Query string value of the page URL (a form opened with a record id: ?id=12). */
    HRM.param = function (k) { try { return new URLSearchParams(global.location.search).get(k); } catch (e) { return null; } };
    /** Page data attribute on <body>. */
    HRM.page = function () { return doc.body.getAttribute('data-hrm'); };
    HRM.screen = function () { return HRM.int(doc.body.getAttribute('data-screen')); };

    /** Applies {save, update, delete, print} rights: a button without the right is disabled with a tooltip. */
    HRM.applyRights = function (r, map) {
        if (!r) return;
        Object.keys(map || {}).forEach(function (right) {
            (Array.isArray(map[right]) ? map[right] : [map[right]]).forEach(function (id) {
                var b = $id(id); if (!b) return;
                if (r[right] === false) { b.disabled = true; b.title = 'You do not have ' + right + ' rights on this screen'; b.setAttribute('data-no-right', '1'); }
            });
        });
    };

    doc.addEventListener('DOMContentLoaded', function () { wireFullscreen(doc); });
    global.HRM = HRM;
})(window);
