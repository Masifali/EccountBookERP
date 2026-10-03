/* ============================================================================================
 * PR - shared page code of the Purchase Reports (module 52): 477 Grn Report, 478 Gate Pass Report,
 * 479 Purchase Order Report, 480 Purchase Report (With Activites), 869 Stock In Transit Report.
 *
 *   PR.request / PR.run      JSON calls, wait overlay, MessageBox-style errors
 *   PR.Picker                DateTimePicker: a date plus the time of day it carries (Value keeps
 *                            DateTime.Now's time until a preset sets DateTime.Today), optional
 *                            ShowCheckBox
 *   PR.CheckList             Infragistics checked-list combo (branches, parent categories, custom groups)
 *   PR.fill                  DDL.BindDDL / BindDDLNew into a <select data-dtcombo="single">
 *   PR.Grid                  Janus GridEX: filter row (Contains), sort, group rows + group totals,
 *                            total row, link / button cells, selector column
 *   PR.printGrid             ShowReportWithDataTable(dt, rpt) -> POST /reports/print/grid (the desktop's dt)
 *   PR.printTemplate         a slip by .rpt name -> POST /api/print/by-template/{rpt}/pdf
 * ============================================================================================ */
(function (global) {
    'use strict';
    var PR = {};
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    PR.cfg = { amountDecimals: 2, rateDecimals: 2 };
    /** Column widths standing in for Constants.InventoryConstants / ExportConstants (Constants.cs is not in the
        decompiled sources; values as used by the other ported registers). */
    PR.K = { DocNo: 60, Date: 73, DateTime: 120, SupplierName: 170, String: 130, WareHouseName: 150, Amount: 90, Status: 75,
        BranchName: 120, ItemName: 150, ItemCode: 75, Qty: 70, Weight: 80, Uom: 60, CropYear: 73, JobLot: 80, VehicleNo: 80,
        BiltyNo: 60, Freight: 90, EntryUser: 80, NoOfAttachments: 60, GpNo: 50, Remarks: 100, CityName: 75, Rate: 70,
        Term: 90, ApprovedBy: 90, DueDays: 60, AddLess: 70, Percent: 60, PackingType: 115, Currency: 60 };

    PR.$ = function (id) { return document.getElementById(id); };
    PR.esc = function (v) { return String(v == null ? '' : v).replace(/[&<>"']/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]; }); };
    PR.num = function (v) { var n = Number(v); return isFinite(n) ? n : 0; };
    PR.int = function (v) { var n = parseInt(v, 10); return isNaN(n) ? 0 : n; };
    function pad(n) { return String(n).padStart(2, '0'); }
    PR.ymd = function (d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); };
    PR.hms = function (d) { return pad(d.getHours()) + ':' + pad(d.getMinutes()) + ':' + pad(d.getSeconds()); };

    // ------------------------------------------------------------------ messages / requests
    PR.box = function (text) { global.alert(text); };
    PR.message = function (text, ok) { var m = PR.$('prMessage'); if (!m) return; m.textContent = text || ''; m.className = ok ? 'ok' : ''; };
    PR.request = function (url, body) {
        var opt = { credentials: 'same-origin', headers: { Accept: 'application/json' } };
        if (body !== undefined) { opt.method = 'POST'; opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        return fetch(url, opt).then(function (r) {
            if (r.redirected && /\/login(?:[?#]|$)/.test(r.url)) throw new Error('Your session has expired. Please sign in again.');
            return r.text().then(function (raw) {
                var data = null;
                try { data = raw ? JSON.parse(raw) : null; } catch (e) { if (!r.ok) throw new Error(raw || ('Request failed (' + r.status + ')')); throw new Error('The server could not load the requested data. Please refresh or sign in again.'); }
                if (!r.ok) throw new Error((data && (data.message || data.error)) || ('Request failed (' + r.status + ')'));
                return data;
            });
        });
    };
    var busy = 0;
    /* The button whose click started the current handler (capture phase, cleared after the event), so PR.run can
       disable it at once, keep it disabled while the work runs, refuse a second click and re-enable it afterwards. */
    var clicked = null, locks = new WeakMap();
    document.addEventListener('click', function (e) {
        clicked = e.target && e.target.closest ? e.target.closest('button,summary') : null;
        setTimeout(function () { clicked = null; }, 0);
    }, true);
    /** waitForm.Show / Close around an action; an exception becomes MessageBox.Show(ex.Message). */
    PR.run = function (fn, button) {
        var b = button || clicked, l = PR.$('prLoading');
        clicked = null;                                                     // one click locks one run (nested runs do not re-lock it)
        if (b && locks.has(b)) return Promise.resolve();                  // already running: duplicate click ignored
        if (b) { locks.set(b, b.disabled); b.disabled = true; b.setAttribute('aria-busy', 'true'); }
        busy++; if (l) l.hidden = false;
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.catch(function (e) { PR.box(e && e.message ? e.message : String(e)); })
            .then(function (v) {
                busy--; if (l && busy <= 0) { busy = 0; l.hidden = true; }
                if (b) { b.disabled = !!locks.get(b); b.removeAttribute('aria-busy'); locks.delete(b); }
                return v;
            });
    };

    /* Document numbers open their own record (EditMethodFromLinked, CommonServices.cs:2533): the purchase pages read ?id=.
       Other document types go through DocLink (countx_doc_link.js) when the page loads it. */
    PR.DOC_ROUTES = { 41: '/purchase/purchase-order', 46: '/purchase/goods-receipt-notes', 51: '/purchase/inward-gate-pass',
        56: '/purchase/purchase-invoice', 57: '/purchase/purchase-invoice-direct', 59: '/purchase/purchase-invoice-return',
        138: '/purchase/purchase-invoice-again-grn-direct', 143: '/purchase/grn-sale-return', 172: '/purchase/purchase-invoice-against-grn-order' };
    PR.docUrl = function (documentTypeId, id) { var r = PR.DOC_ROUTES[PR.int(documentTypeId)]; return r && PR.int(id) > 0 ? r + '?id=' + PR.int(id) : ''; };
    PR.openDoc = function (documentTypeId, id) {
        var url = PR.docUrl(documentTypeId, id);
        if (url) { if (!global.open(url, '_blank')) PR.box('The browser blocked the new window.'); return; }
        if (global.DocLink && global.DocLink.open(PR.int(documentTypeId), PR.int(id), { message: PR.box })) return;
        PR.box('Document type ' + documentTypeId + ' does not have a web page to open yet.');
    };

    // ------------------------------------------------------------------ formats
    /** "#,##0.##" (max 2), "#,##0.###" (3), "#,##0.####" (4); 'amt' stringFormatsingle; 'rate' DecimalRateFormate. */
    PR.fmt = function (v, kind) {
        if (v === null || v === undefined || v === '') return '';
        var n = Number(v); if (!isFinite(n)) return String(v);
        var o;
        switch (kind) {
            case 'n2': o = { maximumFractionDigits: 2 }; break;
            case 'n3': o = { maximumFractionDigits: 3 }; break;
            case 'n4': o = { maximumFractionDigits: 4 }; break;
            case 'amt': o = { minimumFractionDigits: PR.cfg.amountDecimals, maximumFractionDigits: PR.cfg.amountDecimals }; break;
            case 'rate': o = { minimumFractionDigits: PR.cfg.rateDecimals, maximumFractionDigits: PR.cfg.rateDecimals }; break;
            default: return String(v);
        }
        return n.toLocaleString('en-US', o);
    };
    function parts(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(String(v || ''));
        return m ? { y: +m[1], mo: +m[2], d: +m[3], h: +(m[4] || 0), mi: +(m[5] || 0) } : null;
    }
    /** "dd-MMM-yy" */
    PR.dmy = function (v) { var p = parts(v); return p ? pad(p.d) + '-' + MONTHS[p.mo - 1] + '-' + String(p.y).slice(-2) : (v == null ? '' : String(v)); };
    /** "dd-MMM-yy hh:mm tt" */
    PR.dmyt = function (v) {
        var p = parts(v); if (!p) return v == null ? '' : String(v);
        var h = p.h % 12 || 12;
        return PR.dmy(v) + ' ' + pad(h) + ':' + pad(p.mi) + ' ' + (p.h < 12 ? 'AM' : 'PM');
    };
    /** DateTime.ToShortDateString() (en-GB style dd/MM/yyyy as shown on the Pakistani desktop). */
    PR.shortDate = function (v) { var p = parts(v); return p ? pad(p.d) + '/' + pad(p.mo) + '/' + p.y : (v == null ? '' : String(v)); };

    // ------------------------------------------------------------------ DateTimePicker
    /**
     * A WinForms DateTimePicker: Value = date + time of day. The time is DateTime.Now's at page load and
     * stays when the user picks another date; presets that assign DateTime.Today reset it to midnight.
     * With ShowCheckBox, value() is '' while unchecked (the BLL then omits the parameter).
     */
    PR.Picker = function (inputId, checkId) {
        this.input = PR.$(inputId); this.check = checkId ? PR.$(checkId) : null;
        this.time = PR.hms(new Date());
        var self = this;
        if (this.check) {
            var sync = function () { self.input.disabled = !self.check.checked; };
            this.check.addEventListener('change', sync); sync();
        }
    };
    PR.Picker.prototype.set = function (value, midnight) {
        if (value instanceof Date) { this.input.value = PR.ymd(value); this.time = midnight ? '00:00:00' : PR.hms(value); return; }
        var s = String(value || '');
        this.input.value = s.slice(0, 10);
        if (midnight) this.time = '00:00:00';
        else if (s.length > 10) this.time = s.slice(11, 19).padEnd(8, ':00').slice(0, 8);
    };
    PR.Picker.prototype.setChecked = function (on) { if (this.check) { this.check.checked = !!on; this.check.dispatchEvent(new Event('change')); } };
    PR.Picker.prototype.checked = function () { return !this.check || this.check.checked; };
    PR.Picker.prototype.value = function () { return this.checked() && this.input.value ? this.input.value + 'T' + this.time : ''; };
    PR.Picker.prototype.date = function () { return this.input.value; };
    PR.Picker.prototype.focus = function () { this.input.focus(); };

    /** cmbperemeter / cmbDateType ValueChanged - CommonServices.DateType presets (e.g. frmGRNHistory.cs:304-339). */
    PR.applyDateType = function (id, from, to, yearStart) {
        var today = new Date(), d;
        switch (PR.int(id)) {
            case 1: from.set(today, true); break;
            case 2: d = new Date(); d.setDate(d.getDate() - 7); from.set(d, true); break;
            case 3: from.set(new Date(today.getUTCFullYear(), today.getUTCMonth(), 1), true); to.set(today, true); break;   // UtcNow's month
            case 4: from.set(new Date(today.getFullYear(), 0, 1), true); to.set(today, true); break;
            case 5: if (yearStart) from.set(yearStart, true); break;
        }
    };

    // ------------------------------------------------------------------ combos
    /**
     * DDL.BindDDL(dt, combo, value, text, caption, ZeroIndex) - zeroIndex adds the "...Select Any Value..."
     * row; without it the combo still starts empty (no row active) unless the form activates one.
     */
    PR.fill = function (select, rows, opt) {
        opt = opt || {};
        var el = typeof select === 'string' ? PR.$(select) : select, keep = el.value;
        var vk = opt.value || 'Id', tk = opt.text || 'ReferenceName';
        var html = '<option value="">' + (opt.zeroIndex ? '...Select Any Value...' : '') + '</option>';
        (rows || []).forEach(function (r) { html += '<option value="' + PR.esc(r[vk]) + '">' + PR.esc(r[tk]) + '</option>'; });
        el.innerHTML = html;
        el.value = keep; if (el.value !== keep) el.value = '';
    };
    PR.byActivity = function (rows, activity, key) {
        key = key || 'Activity';
        return (rows || []).filter(function (r) { return r[key] === activity; });
    };
    PR.text = function (id) { var el = PR.$(id); return el && el.selectedIndex > 0 && el.value !== '' ? el.options[el.selectedIndex].textContent : (el && el.tagName === 'SELECT' ? '' : (el ? el.value : '')); };
    PR.val = function (id) { return PR.int(PR.$(id) ? PR.$(id).value : 0); };

    // ------------------------------------------------------------------ checked-list combo
    /** An UltraCombo with a "Selected" check column (CheckedListSettings, ListSeparator ","). */
    PR.CheckList = function (detailsId, onChange) {
        this.el = PR.$(detailsId); this.items = []; this.onChange = onChange;
        this.el.innerHTML = '<summary></summary><div class="pr-check-pop"><input type="search" placeholder="Search" aria-label="Search">'
            + '<label><input type="checkbox" data-all="1"> (Select all)</label><div class="pr-check-items"></div></div>';
        var self = this;
        this.el.querySelector('input[type=search]').addEventListener('input', function (e) {
            var t = e.target.value.toLowerCase();
            self.el.querySelectorAll('.pr-check-items label').forEach(function (l) { l.hidden = t && l.textContent.toLowerCase().indexOf(t) < 0; });
        });
        this.el.querySelector('[data-all]').addEventListener('change', function (e) {
            self.el.querySelectorAll('.pr-check-items input').forEach(function (i) { i.checked = e.target.checked; });
            self.sync(); if (self.onChange) self.onChange();
        });
        this.el.querySelector('.pr-check-items').addEventListener('change', function () { self.sync(); if (self.onChange) self.onChange(); });
        this.el.addEventListener('toggle', function () { if (!self.el.open && self.onLeave) self.onLeave(); });
    };
    PR.CheckList.prototype.set = function (items, checkedIds) {
        this.items = items || [];
        var on = {}; (checkedIds || []).forEach(function (i) { on[String(i)] = 1; });
        this.el.querySelector('.pr-check-items').innerHTML = this.items.map(function (it) {
            return '<label><input type="checkbox" value="' + PR.esc(it.id) + '"' + (on[String(it.id)] ? ' checked' : '') + '> ' + PR.esc(it.name) + '</label>';
        }).join('');
        this.sync();
    };
    PR.CheckList.prototype.ids = function () {
        return Array.prototype.map.call(this.el.querySelectorAll('.pr-check-items input:checked'), function (i) { return PR.int(i.value); });
    };
    PR.CheckList.prototype.clear = function () { this.el.querySelectorAll('.pr-check-items input').forEach(function (i) { i.checked = false; }); this.sync(); };
    PR.CheckList.prototype.sync = function () {
        var ids = this.ids().map(String);
        var names = this.items.filter(function (it) { return ids.indexOf(String(it.id)) >= 0; }).map(function (it) { return it.name; });
        this.el.querySelector('summary').textContent = names.join(',');
        this.el.querySelector('[data-all]').checked = ids.length > 0 && ids.length === this.items.length;
    };

    // ------------------------------------------------------------------ grid
    /**
     * columns: { key, caption, width, type: 'text'|'num'|'int'|'date'|'datetime'|'bool'|'short', fmt, agg: 'sum'|'avg',
     *            hidden, link, button, edit: 'text'|'date', red(row) }
     * opts:    { groupBy, selector, onLink(col,row), onButton(col,row), rowClass(row) }
     */
    PR.Grid = function (tableId, countId) {
        this.table = PR.$(tableId); this.count = countId ? PR.$(countId) : null;
        this.columns = []; this.rows = []; this.view = []; this.filters = {}; this.sort = null; this.opts = {};
        var self = this;
        this.table.addEventListener('input', function (e) {
            if (e.target.dataset.filter !== undefined) { self.filters[e.target.dataset.filter] = e.target.value.toLowerCase(); self.draw(false); }
            if (e.target.dataset.edit !== undefined) { var r = self.rows[PR.int(e.target.dataset.row)]; if (r) r[e.target.dataset.edit] = e.target.value; }
        });
        this.table.addEventListener('change', function (e) {
            if (e.target.dataset.edit !== undefined) { var r = self.rows[PR.int(e.target.dataset.row)]; if (r) r[e.target.dataset.edit] = e.target.value; }
            if (e.target.dataset.sel !== undefined) {
                var row = self.rows[PR.int(e.target.dataset.sel)]; if (row) row.__checked = e.target.checked;
                e.target.closest('tr').classList.toggle('pr-checked', e.target.checked && !!self.opts.checkedStyle);
            }
            if (e.target.dataset.selall !== undefined) { self.view.forEach(function (r) { r.__checked = e.target.checked; }); self.draw(false); }
        });
        this.table.addEventListener('click', function (e) {
            var th = e.target.closest('th[data-sort]');
            if (th) { var k = th.dataset.sort; self.sort = [k, self.sort && self.sort[0] === k ? -self.sort[1] : 1]; self.draw(false); return; }
            var o = e.target.closest('[data-open]');
            if (o) {
                e.preventDefault();
                var orow = self.rows[PR.int(o.dataset.row)], ocol = self.columns.find(function (x) { return x.key === o.dataset.open; });
                var doc = orow && ocol && ocol.open ? ocol.open(orow) : null;
                if (doc) PR.openDoc(doc[0], doc[1]);
                return;
            }
            var a = e.target.closest('[data-link],[data-btn]');
            if (!a) return;
            e.preventDefault();
            var row = self.rows[PR.int(a.dataset.row)], col = a.dataset.link || a.dataset.btn;
            if (a.dataset.link && self.opts.onLink) self.opts.onLink(col, row);
            if (a.dataset.btn && self.opts.onButton) self.opts.onButton(col, row);
        });
    };
    PR.Grid.prototype.show = function (columns, rows, opts) {
        this.columns = columns; this.rows = rows || []; this.opts = opts || {}; this.filters = {}; this.sort = null;
        this.rows.forEach(function (r, i) { r.__i = i; });
        this.draw(true);
    };
    PR.Grid.prototype.clear = function () { this.show([], [], {}); };
    PR.Grid.prototype.visible = function () { return this.columns.filter(function (c) { return !c.hidden; }); };
    PR.Grid.prototype.checked = function () { return this.rows.filter(function (r) { return r.__checked; }); };
    PR.Grid.prototype.cell = function (c, r) {
        var v = r[c.key];
        switch (c.type) {
            case 'num': return PR.fmt(v, c.fmt || 'n2');
            case 'date': return PR.dmy(v);
            case 'datetime': return PR.dmyt(v);
            case 'short': return PR.shortDate(v);
            case 'bool': return v ? '☑' : '☐';
            default: return v == null ? '' : typeof v === 'boolean' ? (v ? 'True' : 'False') : String(v);
        }
    };
    PR.Grid.prototype.draw = function (head) {
        var self = this, cols = this.visible(), t = this.table, sel = !!this.opts.selector;
        if (!cols.length) { t.innerHTML = ''; if (this.count) this.count.textContent = ''; return; }
        if (head) {
            var w = (sel ? 30 : 0) + cols.reduce(function (s, c) { return s + (c.width || 90); }, 0);
            t.style.width = w + 'px';
            var h = '<colgroup>' + (sel ? '<col style="width:30px">' : '') + cols.map(function (c) { return '<col style="width:' + (c.width || 90) + 'px">'; }).join('') + '</colgroup>';
            h += '<thead><tr>' + (sel ? '<th><input type="checkbox" data-selall="1" aria-label="Select all"></th>' : '')
                + cols.map(function (c) { return '<th data-sort="' + PR.esc(c.key) + '" title="' + PR.esc(c.caption || c.key) + '">' + PR.esc(c.caption || c.key) + '</th>'; }).join('') + '</tr>'
                + '<tr class="pr-filter-row">' + (sel ? '<th></th>' : '') + cols.map(function (c) { return '<th>' + (c.button ? '' : '<input data-filter="' + PR.esc(c.key) + '" aria-label="Filter ' + PR.esc(c.caption || c.key) + '">') + '</th>'; }).join('') + '</tr></thead><tbody></tbody><tfoot></tfoot>';
            t.innerHTML = h;
        }
        var fk = Object.keys(this.filters).filter(function (k) { return self.filters[k]; });
        var view = this.rows.filter(function (r) {
            return fk.every(function (k) { var c = self.columns.find(function (x) { return x.key === k; }); return self.cell(c, r).toLowerCase().indexOf(self.filters[k]) >= 0; });
        });
        if (this.sort) {
            var k = this.sort[0], d = this.sort[1];
            view.sort(function (a, b) { var x = a[k], y = b[k]; return typeof x === 'number' && typeof y === 'number' ? d * (x - y) : d * String(x == null ? '' : x).localeCompare(String(y == null ? '' : y)); });
        }
        var g = this.opts.groupBy;
        if (g) view.sort(function (a, b) { return String(a[g] == null ? '' : a[g]).localeCompare(String(b[g] == null ? '' : b[g])); });
        this.view = view;
        var body = '', prev = null, bucket = [];
        function totals(list, cls, label) {
            return '<tr class="' + cls + '">' + (sel ? '<td></td>' : '') + cols.map(function (c, i) {
                if (!c.agg) return '<td>' + (i === 0 ? PR.esc(label) : '') + '</td>';
                var s = list.reduce(function (a, r) { return a + PR.num(r[c.key]); }, 0);
                if (c.agg === 'avg') s = list.length ? s / list.length : 0;
                return '<td class="n">' + PR.esc(PR.fmt(s, c.fmt || 'n2')) + '</td>';
            }).join('') + '</tr>';
        }
        view.forEach(function (r) {
            if (g && String(r[g]) !== prev) {
                if (prev !== null) body += totals(bucket, 'pr-gtotal', 'Total');
                bucket = []; prev = String(r[g]);
                body += '<tr class="pr-group"><td colspan="' + (cols.length + (sel ? 1 : 0)) + '">' + PR.esc(r[g] == null ? '' : r[g]) + '</td></tr>';
            }
            bucket.push(r);
            var cls = (self.opts.rowClass ? self.opts.rowClass(r) : '') + (r.__checked && self.opts.checkedStyle ? ' pr-checked' : '');
            body += '<tr class="' + cls + '">' + (sel ? '<td><input type="checkbox" data-sel="' + r.__i + '"' + (r.__checked ? ' checked' : '') + '></td>' : '')
                + cols.map(function (c) {
                    var txt = self.cell(c, r), cl = (c.type === 'num' || c.type === 'int' ? 'n' : '') + (c.red && c.red(r) ? ' pr-red' : '');
                    var inner;
                    if (c.button) inner = (!c.buttonIf || c.buttonIf(r)) ? '<button type="button" class="pr-cell-btn" data-btn="' + PR.esc(c.key) + '" data-row="' + r.__i + '">' + PR.esc(c.button) + '</button>' : '';
                    else if (c.edit) inner = '<input class="pr-edit" type="' + (c.edit === 'date' ? 'date' : 'text') + '" data-edit="' + PR.esc(c.key) + '" data-row="' + r.__i + '" value="' + PR.esc(c.edit === 'date' ? String(r[c.key] || '').slice(0, 10) : (r[c.key] == null ? '' : r[c.key])) + '">';
                    else if (c.open && c.open(r)) {
                        // the code opens its record; the desktop's own link action (a slip) stays on the small print button
                        var doc = c.open(r), href = PR.docUrl(doc[0], doc[1]) || '#';
                        inner = '<a class="pr-open" href="' + PR.esc(href) + '" data-open="' + PR.esc(c.key) + '" data-row="' + r.__i + '" title="Open">' + PR.esc(txt) + '</a>'
                            + (c.link && (!c.linkIf || c.linkIf(r)) ? ' <button type="button" class="pr-link pr-slip" data-link="' + PR.esc(c.key) + '" data-row="' + r.__i + '" title="Print slip" aria-label="Print slip">&#9113;</button>' : '');
                    }
                    else if (c.link && (!c.linkIf || c.linkIf(r))) inner = '<button type="button" class="pr-link" data-link="' + PR.esc(c.key) + '" data-row="' + r.__i + '">' + PR.esc(txt) + '</button>';
                    else inner = PR.esc(txt);
                    return '<td class="' + cl + '" title="' + PR.esc(txt) + '">' + inner + '</td>';
                }).join('') + '</tr>';
        });
        if (g && prev !== null) body += totals(bucket, 'pr-gtotal', 'Total');
        t.tBodies[0].innerHTML = body || '';
        t.tFoot.innerHTML = cols.some(function (c) { return c.agg; }) ? totals(view, '', 'Total').replace('<tr class="">', '<tr>') : '';
        if (this.count) this.count.textContent = view.length + ' Records';
    };

    /** The grid as the user sees it (visible columns, filtered/sorted rows) - caption -> cell text. */
    PR.Grid.prototype.shownRows = function () {
        var self = this, cols = this.visible().filter(function (c) { return !c.button; });
        return this.view.map(function (r) { var o = {}; cols.forEach(function (c) { o[c.caption || c.key] = self.cell(c, r); }); return o; });
    };
    /** CtrlGrdBar: grid print and export of what the grid shows. */
    PR.gridTools = function (grid, printId, exportId, title) {
        var p = PR.$(printId), x = PR.$(exportId);
        if (p) p.addEventListener('click', function () { PR.run(function () { return PR.printGrid('', title(), grid.shownRows()); }); });
        if (x) x.addEventListener('click', function () {
            var rows = grid.shownRows();
            if (!rows.length) { PR.box('Not Record Found For Display'); return; }
            var keys = Object.keys(rows[0]);
            var q = function (s) { s = String(s == null ? '' : s); return /[",\r\n]/.test(s) ? '"' + s.replace(/"/g, '""') + '"' : s; };
            var csv = [keys.map(q).join(',')].concat(rows.map(function (r) { return keys.map(function (k) { return q(r[k]); }).join(','); })).join('\r\n');
            var a = document.createElement('a'); a.href = URL.createObjectURL(new Blob(['﻿' + csv], { type: 'text/csv' }));
            a.download = title().replace(/[^\w\- ]+/g, '') + '.csv'; a.click(); setTimeout(function () { URL.revokeObjectURL(a.href); }, 60000);
        });
    };

    // ------------------------------------------------------------------ prints
    function openPdf(promise) {
        var w = global.open('', '_blank');
        return promise.then(function (r) {
            if (r.redirected && /\/login(?:[?#]|$)/.test(r.url)) throw new Error('Your session has expired. Please sign in again.');
            if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0)
                return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else global.open(u); setTimeout(function () { URL.revokeObjectURL(u); }, 120000); });
            return r.text().then(function (x) { throw new Error(x || ('Print failed (' + r.status + ')')); });
        }).catch(function (e) { if (w) w.close(); throw e; });
    }
    /** Reporting.ShowReportWithDataTable(dt, rpt): the rows the desktop hands to the report. */
    PR.printGrid = function (rpt, title, rows) {
        if (!rows || !rows.length) return Promise.reject(new Error('Not Record Found For Display'));
        return openPdf(fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' },
            body: JSON.stringify({ rpt: rpt, title: title, rows: rows }) }));
    };
    /** A seeded print by the .rpt name the desktop uses (CommonServices slips). */
    PR.printTemplate = function (rpt, args) {
        return openPdf(fetch('/api/print/by-template/' + encodeURIComponent(rpt) + '/pdf', { method: 'POST', credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify(args || {}) }));
    };

    // ------------------------------------------------------------------ dialogs and links
    function dialog(title, bodyHtml, buttons) {
        var d = document.createElement('dialog'); d.className = 'pr-dialog';
        d.innerHTML = '<h2>' + PR.esc(title) + '</h2><div class="pr-dialog-body">' + bodyHtml + '</div><div class="pr-dialog-buttons"></div>';
        var bar = d.querySelector('.pr-dialog-buttons');
        return new Promise(function (resolve) {
            (buttons || [{ text: 'Close', value: null }]).forEach(function (b) {
                var el = document.createElement('button'); el.type = 'button'; el.textContent = b.text;
                el.addEventListener('click', function () { var v = typeof b.value === 'function' ? b.value(d) : b.value; d.close(); d.remove(); resolve(v); });
                bar.appendChild(el);
            });
            d.addEventListener('cancel', function () { d.remove(); resolve(null); });
            document.body.appendChild(d); d.showModal();
        });
    }
    PR.dialog = dialog;
    /** ShortCutKeyPopUp(dt) - KeyCombination / Description. */
    PR.shortcuts = function (rows) {
        return dialog('ShortCut Keys', '<table class="pr-grid"><thead><tr><th style="width:130px">KeyCombination</th><th style="width:330px">Description</th></tr></thead><tbody>'
            + rows.map(function (r) { return '<tr><td>' + PR.esc(r[0]) + '</td><td>' + PR.esc(r[1]) + '</td></tr>'; }).join('') + '</tbody></table>');
    };
    PR.confirm = function (text) { return Promise.resolve(global.confirm(text)); };
    /** RemarksPopUp - txtRemarks.Text ('' when cancelled). */
    PR.remarks = function () {
        return dialog('Remarks', '<textarea id="prRemarksText" aria-label="Remarks"></textarea>',
            [{ text: 'OK', value: function (d) { return d.querySelector('#prRemarksText').value; } }, { text: 'Cancel', value: '' }]);
    };
    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId) - the AttachmentView list. */
    PR.attachments = function (screen, id, documentTypeId) {
        var q = '?id=' + encodeURIComponent(id) + '&documentTypeId=' + encodeURIComponent(documentTypeId);
        var base = '/purchase/api/reports/' + screen + '/attachments';
        return PR.run(function () {
            return PR.request(base + q).then(function (rows) {
                if (!rows || !rows.length) return;                  // nothing is shown when there is none
                return dialog('Attachments', '<table class="pr-grid"><thead><tr><th style="width:220px">AttachmentName</th><th style="width:220px">CustomName</th><th style="width:130px">EntryDate</th></tr></thead><tbody>'
                    + rows.map(function (a) { return '<tr><td><a href="' + base + '/' + PR.esc(a.Id) + q + '">' + PR.esc(a.AttachmentName) + '</a></td><td>' + PR.esc(a.CustomName) + '</td><td>' + PR.esc(PR.dmyt(a.EntryDate)) + '</td></tr>'; }).join('') + '</tbody></table>');
            });
        });
    };
    /** CommonServices.GoToGeneralLedgerFromLinkedEvent(GetGlAccountIdBySupplierCustomerId(id), from, to). */
    PR.openLedger = function (screen, supplierCustomerId, from, to) {
        return PR.run(function () {
            return PR.request('/purchase/api/reports/' + screen + '/gl-account?supplierCustomerId=' + encodeURIComponent(supplierCustomerId)).then(function (r) {
                var q = new URLSearchParams(); q.set('accountId', r.accountId);
                if (from) q.set('fromDate', String(from).slice(0, 10)); if (to) q.set('toDate', String(to).slice(0, 10));
                global.open('/accounts/reports/general-ledger?' + q.toString(), '_blank');
            });
        });
    };
    /** CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(ItemId, from, to). */
    PR.openItemLedger = function (itemId, from, to) {
        var q = new URLSearchParams(); q.set('itemId', itemId);
        if (from) q.set('fromDate', String(from).slice(0, 10)); if (to) q.set('toDate', String(to).slice(0, 10));
        global.open('/stocks/item-ledger?' + q.toString(), '_blank');
    };
    /** Approval Detail - frmApprovalCommentory, through the shared approval-history dialog. */
    PR.approval = function (url, body) {
        if (!global.SaleApprovalHistory) { PR.box('The approval history dialog is not available on this page.'); return; }
        return global.SaleApprovalHistory.open(function () { return PR.request(url, body); }, function (fn) { return PR.run(fn); });
    };
    /** A desktop form that is not ported: say what the desktop would open. */
    PR.notPorted = function (form, what) { PR.box(what + ' opens ' + form + ' on the desktop. That screen is not ported yet, so nothing was opened.'); };

    /** Form KeyDown: Enter -> SendKeys("{TAB}") (next control). */
    PR.enterAsTab = function (root) {
        (root || document).addEventListener('keydown', function (e) {
            if (e.key !== 'Enter' || e.ctrlKey || e.altKey || !e.target.matches('input:not([type=checkbox]):not([type=radio]),select')) return;
            if (e.target.closest('table.pr-grid')) return;
            e.preventDefault();
            var list = Array.prototype.filter.call(document.querySelectorAll('input,select,button,summary'), function (el) { return !el.disabled && el.getClientRects().length && !el.closest('table.pr-grid'); });
            var i = list.indexOf(e.target); if (i >= 0 && list[i + 1]) list[i + 1].focus();
        });
    };
    PR.digitsOnly = function (ids) {
        ids.forEach(function (id) {
            var el = PR.$(id); if (!el) return;
            el.addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault(); });
            el.addEventListener('input', function () { var v = el.value.replace(/[^0-9]/g, ''); if (v !== el.value) el.value = v; });
        });
    };
    PR.fullscreen = function (btnId, sectionId) {
        var b = PR.$(btnId); if (!b) return;
        b.addEventListener('click', function () { var s = PR.$(sectionId); try { if (document.fullscreenElement) document.exitFullscreen(); else s.requestFullscreen(); } catch (e) { PR.box(e.message); } });
    };
    PR.tabs = function (onChange) {
        document.querySelectorAll('.pr-tabs [data-tab]').forEach(function (b) {
            b.addEventListener('click', function () { PR.selectTab(b.dataset.tab); if (onChange) onChange(b.dataset.tab); });
        });
    };
    PR.selectTab = function (name) {
        document.querySelectorAll('.pr-tabs [data-tab]').forEach(function (b) { b.setAttribute('aria-selected', b.dataset.tab === name ? 'true' : 'false'); });
        document.querySelectorAll('[data-tab-panel]').forEach(function (p) { p.hidden = p.dataset.tabPanel !== name; });
    };
    PR.currentTab = function () { var b = document.querySelector('.pr-tabs [aria-selected="true"]'); return b ? b.dataset.tab : ''; };

    global.PR = PR;
})(window);
