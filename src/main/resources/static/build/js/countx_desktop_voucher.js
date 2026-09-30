/* Shared helpers of the plain Account_Definition voucher pages (screens 19 / 22 / 46).
 * DV.api(screen)  - the /accounts/api/desktop-voucher/{screen} endpoints
 * DV.Combo        - a searchable multi-column combo (the desktop's UltraCombo)
 * DV.msg/confirm  - MessageBox.Show equivalents (promise based)
 * DV.fmt*         - the desktop's number / date formats
 * Nothing here chooses a tenancy value: the server takes them from the signed-in user. */
(function (w) {
    'use strict';
    var DV = {};

    // ---------------------------------------------------------------------------------- http
    function qs(params) {
        var p = new URLSearchParams();
        Object.keys(params || {}).forEach(function (k) {
            var v = params[k];
            if (v === undefined || v === null || v === '') return;
            p.append(k, v);
        });
        var s = p.toString();
        return s ? ('?' + s) : '';
    }
    DV.get = function (url, params) {
        return fetch(url + qs(params), { credentials: 'same-origin', headers: { 'Accept': 'application/json' } })
            .then(function (r) {
                return r.json().catch(function () { return null; }).then(function (body) {
                    if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
                    return body;
                });
            });
    };
    DV.post = function (url, data) {
        return fetch(url, {
            method: 'POST', credentials: 'same-origin',
            headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
            body: JSON.stringify(data)
        }).then(function (r) {
            return r.json().catch(function () { return null; }).then(function (body) {
                return { status: r.status, ok: r.ok, body: body };
            });
        });
    };
    DV.api = function (screen) {
        var base = '/accounts/api/desktop-voucher/' + screen;
        return {
            base: base,
            get: function (path, params) { return DV.get(base + path, params); }
        };
    };

    /** Save, answering the desktop's Yes/No questions (409 {confirm}) and re-posting on Yes. */
    DV.saveWithConfirm = function (url, payload) {
        function attempt() {
            return DV.post(url, payload).then(function (res) {
                if (res.status === 409 && res.body && res.body.confirm) {
                    return DV.confirm(res.body.message, 'Confirm').then(function (yes) {
                        if (!yes) return null;
                        if (res.body.confirm === 'duplicate') payload.duplicateAcknowledged = true;
                        if (res.body.confirm === 'negativeBalance') payload.negativeBalanceAcknowledged = true;
                        return attempt();
                    });
                }
                if (!res.ok) {
                    var m = (res.body && res.body.message) || ('Save failed (' + res.status + ')');
                    var e = new Error(m);
                    e.dvTitle = res.status >= 500 ? 'Database Error' : 'Message';
                    throw e;
                }
                return res.body;
            });
        }
        return attempt();
    };

    // ---------------------------------------------------------------------------- message box
    function modal(text, title, buttons) {
        return new Promise(function (resolve) {
            var back = document.createElement('div');
            back.className = 'dv-modal-back';
            var box = document.createElement('div');
            box.className = 'dv-modal';
            var t = document.createElement('div'); t.className = 'dv-modal-title'; t.textContent = title || 'Message';
            var b = document.createElement('div'); b.className = 'dv-modal-body'; b.textContent = text == null ? '' : String(text);
            var f = document.createElement('div'); f.className = 'dv-modal-foot';
            buttons.forEach(function (bt, i) {
                var x = document.createElement('button');
                x.type = 'button'; x.className = 'dv-btn'; x.textContent = bt.text;
                x.addEventListener('click', function () { document.body.removeChild(back); resolve(bt.value); });
                f.appendChild(x);
                if (i === 0) setTimeout(function () { x.focus(); }, 0);
            });
            box.appendChild(t); box.appendChild(b); box.appendChild(f); back.appendChild(box);
            document.body.appendChild(back);
        });
    }
    DV.msg = function (text, title) { return modal(text, title || 'Message', [{ text: 'OK', value: true }]); };
    DV.confirm = function (text, title) {
        return modal(text, title || 'Confirm', [{ text: 'Yes', value: true }, { text: 'No', value: false }]);
    };

    // -------------------------------------------------------------------------------- formats
    /** "#,##0.<n zeros>" - clsGlobalVariables.stringFormatsingle (n = Default NoofDecimal Points For Amount). */
    DV.fmtAmount = function (v, decimals) {
        var n = Number(v) || 0;
        var d = decimals > 0 ? decimals : 0;
        return n.toLocaleString('en-US', { minimumFractionDigits: d, maximumFractionDigits: d });
    };
    /** "#,##0.###" */
    DV.fmt3 = function (v) {
        var n = Number(v) || 0;
        return n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 });
    };
    /** "#,#;(#,#);0" - balances shown next to the account combos. */
    DV.fmtBal = function (v) {
        var n = Math.round(Number(v) || 0);
        if (n === 0) return '0';
        var s = Math.abs(n).toLocaleString('en-US', { maximumFractionDigits: 0 });
        return n < 0 ? '(' + s + ')' : s;
    };
    DV.num = function (v) {
        if (v === null || v === undefined) return 0;
        var n = parseFloat(String(v).replace(/,/g, ''));
        return isNaN(n) ? 0 : n;
    };
    DV.today = function () {
        var d = new Date();
        return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2);
    };
    DV.isoDay = function (v) { return v ? String(v).substring(0, 10) : ''; };
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    /** dd-MMM-yyyy (the history grid's VoucherDate text). */
    DV.fmtDate = function (v) {
        if (!v) return '';
        var s = String(v);
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (!m) return s;
        return m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1];
    };
    /** yyyy-MM-dd HH:mm:ss (EntryDate / ModifyDate / ApprovedDate columns). */
    DV.fmtStamp = function (v) {
        if (!v) return '';
        return String(v).replace('T', ' ').substring(0, 19);
    };
    DV.esc = function (v) {
        return String(v == null ? '' : v).replace(/[&<>"']/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
        });
    };
    DV.pick = function (row, key) {
        if (!row) return undefined;
        if (key in row) return row[key];
        var lk = key.toLowerCase();
        for (var k in row) if (k.toLowerCase() === lk) return row[k];
        return undefined;
    };

    // ---------------------------------------------------------------------------------- combo
    /**
     * new DV.Combo(hostElement, {
     *   valueKey: 'Id', displayKey: 'AccountTitle', columns: [{key, caption, hidden}],
     *   header: 'Debit Account', limitToList: true, onChange: fn(row), onLeave: fn()
     * })
     */
    DV.Combo = function (host, opts) {
        var self = this;
        this.opts = opts || {};
        this.rows = [];
        this.row = null;
        this.value = 0;
        this.host = host;
        host.classList.add('dv-combo');
        host.innerHTML = '';
        this.input = document.createElement('input');
        this.input.className = 'dv-in';
        this.input.autocomplete = 'off';
        if (opts.id) this.input.id = opts.id;
        var arrow = document.createElement('button');
        arrow.type = 'button'; arrow.className = 'dv-combo-arrow'; arrow.tabIndex = -1; arrow.textContent = '▼';
        host.appendChild(this.input); host.appendChild(arrow);
        this.panel = null;
        this.hot = -1;
        this.filtered = [];

        arrow.addEventListener('mousedown', function (e) { e.preventDefault(); self.input.focus(); self.toggle(); });
        this.input.addEventListener('input', function () { self.open(self.input.value); });
        this.input.addEventListener('keydown', function (e) {
            if (e.key === 'ArrowDown') { e.preventDefault(); if (!self.panel) self.open(''); else self.move(1); }
            else if (e.key === 'ArrowUp') { e.preventDefault(); self.move(-1); }
            else if (e.key === 'Enter') {
                if (self.panel && self.hot >= 0) { e.preventDefault(); self.choose(self.filtered[self.hot]); }
            } else if (e.key === 'Escape') { self.close(); }
            else if (e.key === 'Tab') { if (self.panel && self.hot >= 0) self.choose(self.filtered[self.hot], true); }
        });
        this.input.addEventListener('blur', function () {
            setTimeout(function () {
                if (self._picking) return;
                self.close();
                self.commitText();
                if (typeof self.opts.onLeave === 'function') self.opts.onLeave(self.row);
            }, 150);
        });
    };
    DV.Combo.prototype.cols = function () {
        return (this.opts.columns || [{ key: this.opts.displayKey, caption: this.opts.header || this.opts.displayKey }])
            .filter(function (c) { return !c.hidden; });
    };
    DV.Combo.prototype.setData = function (rows, keepValue) {
        var keep = keepValue ? this.value : 0;
        this.rows = rows || [];
        if (keep) this.setValue(keep, true); else this.clear(true);
    };
    DV.Combo.prototype.setDisplayKey = function (key, header) {
        this.opts.displayKey = key;
        if (header) this.opts.header = header;
        if (this.row) this.input.value = DV.pick(this.row, key) == null ? '' : DV.pick(this.row, key);
    };
    DV.Combo.prototype.find = function (v) {
        var vk = this.opts.valueKey;
        for (var i = 0; i < this.rows.length; i++) {
            if (String(DV.pick(this.rows[i], vk)) === String(v)) return this.rows[i];
        }
        return null;
    };
    DV.Combo.prototype.setValue = function (v, silent) {
        var r = (v === null || v === undefined || v === '' || Number(v) === 0) ? null : this.find(v);
        this.row = r;
        this.value = r ? Number(DV.pick(r, this.opts.valueKey)) : 0;
        this.input.value = r ? (DV.pick(r, this.opts.displayKey) == null ? '' : DV.pick(r, this.opts.displayKey)) : '';
        if (!silent && typeof this.opts.onChange === 'function') this.opts.onChange(this.row);
        return !!r;
    };
    /** A value that is not in the list, shown as text (the desktop does this for cheque numbers). */
    DV.Combo.prototype.setFree = function (value, text) {
        this.row = null;
        this.value = Number(value) || 0;
        this.input.value = text == null ? '' : text;
    };
    DV.Combo.prototype.clear = function (silent) {
        this.row = null; this.value = 0; this.input.value = '';
        if (!silent && typeof this.opts.onChange === 'function') this.opts.onChange(null);
    };
    DV.Combo.prototype.text = function () { return this.input.value; };
    DV.Combo.prototype.cell = function (key) { return this.row ? DV.pick(this.row, key) : undefined; };
    DV.Combo.prototype.focus = function () { this.input.focus(); };
    DV.Combo.prototype.setEnabled = function (on) { this.input.disabled = !on; };
    DV.Combo.prototype.commitText = function () {
        var t = this.input.value.trim();
        if (t === '') { if (this.row || this.value) this.clear(); return; }
        if (this.row && String(DV.pick(this.row, this.opts.displayKey)) === this.input.value) return;
        var dk = this.opts.displayKey, match = null;
        for (var i = 0; i < this.rows.length; i++) {
            if (String(DV.pick(this.rows[i], dk) || '').toLowerCase() === t.toLowerCase()) { match = this.rows[i]; break; }
        }
        if (match) { this.setValue(DV.pick(match, this.opts.valueKey)); return; }
        if (this.opts.limitToList !== false) { this.clear(); }
        else { var had = this.row; this.row = null; this.value = 0; if (had && typeof this.opts.onChange === 'function') this.opts.onChange(null); }
    };
    DV.Combo.prototype.toggle = function () { if (this.panel) this.close(); else this.open(''); };
    DV.Combo.prototype.open = function (filter) {
        var self = this, dk = this.opts.displayKey, f = (filter || '').toLowerCase();
        this.filtered = this.rows.filter(function (r) {
            return !f || String(DV.pick(r, dk) || '').toLowerCase().indexOf(f) >= 0;
        });
        if (!this.panel) {
            this.panel = document.createElement('div');
            this.panel.className = 'dv-combo-panel';
            this.panel.addEventListener('mousedown', function (e) { e.preventDefault(); self._picking = true; });
            this.panel.addEventListener('mouseup', function () { setTimeout(function () { self._picking = false; }, 0); });
            document.body.appendChild(this.panel);
        }
        var cols = this.cols();
        var h = '<table><thead><tr>' + cols.map(function (c) {
            var cap = (c.key === dk && self.opts.header) ? self.opts.header : (c.caption || c.key);
            return '<th>' + DV.esc(cap) + '</th>';
        }).join('') + '</tr></thead><tbody>';
        var max = Math.min(this.filtered.length, 500);
        for (var i = 0; i < max; i++) {
            var r = this.filtered[i];
            h += '<tr data-i="' + i + '">' + cols.map(function (c) {
                var v = DV.pick(r, c.key);
                return '<td>' + DV.esc(v == null ? '' : v) + '</td>';
            }).join('') + '</tr>';
        }
        h += '</tbody></table>';
        this.panel.innerHTML = h;
        Array.prototype.forEach.call(this.panel.querySelectorAll('tbody tr'), function (tr) {
            tr.addEventListener('click', function () {
                self._picking = false;
                self.choose(self.filtered[Number(tr.getAttribute('data-i'))]);
                self.input.focus();
            });
        });
        var rc = this.input.getBoundingClientRect();
        this.panel.style.left = rc.left + 'px';
        this.panel.style.top = (rc.bottom + 1) + 'px';
        this.panel.style.minWidth = Math.max(rc.width, 260) + 'px';
        this.hot = this.filtered.length ? 0 : -1;
        this.paintHot();
    };
    DV.Combo.prototype.move = function (d) {
        if (!this.filtered.length) return;
        this.hot = Math.max(0, Math.min(this.filtered.length - 1, this.hot + d));
        this.paintHot();
    };
    DV.Combo.prototype.paintHot = function () {
        if (!this.panel) return;
        var trs = this.panel.querySelectorAll('tbody tr');
        for (var i = 0; i < trs.length; i++) trs[i].classList.toggle('dv-hot', i === this.hot);
        if (trs[this.hot]) trs[this.hot].scrollIntoView({ block: 'nearest' });
    };
    DV.Combo.prototype.choose = function (row, keepFocus) {
        if (!row) return;
        this.setValue(DV.pick(row, this.opts.valueKey));
        this.close();
    };
    DV.Combo.prototype.close = function () {
        if (this.panel) { this.panel.parentNode.removeChild(this.panel); this.panel = null; }
        this.hot = -1;
    };

    /** Fill a plain <select> (used for in-grid editors). */
    DV.fillSelect = function (sel, rows, valueKey, textKey, value, blank) {
        var h = blank ? '<option value="0"></option>' : '';
        (rows || []).forEach(function (r) {
            var v = DV.pick(r, valueKey);
            h += '<option value="' + DV.esc(v) + '"' + (String(v) === String(value) ? ' selected' : '') + '>'
                + DV.esc(DV.pick(r, textKey)) + '</option>';
        });
        sel.innerHTML = h;
    };

    /** Tabs: DV.tabs({form: el, history: el}, onChange). */
    DV.tabs = function (bar, pages, onChange) {
        var btns = bar.querySelectorAll('button[data-tab]');
        function show(name) {
            Array.prototype.forEach.call(btns, function (b) { b.classList.toggle('active', b.getAttribute('data-tab') === name); });
            Object.keys(pages).forEach(function (k) { pages[k].classList.toggle('dv-hidden', k !== name); });
            if (onChange) onChange(name);
        }
        Array.prototype.forEach.call(btns, function (b) { b.addEventListener('click', function () { show(b.getAttribute('data-tab')); }); });
        return { show: show, current: function () {
            for (var i = 0; i < btns.length; i++) if (btns[i].classList.contains('active')) return btns[i].getAttribute('data-tab');
            return null;
        } };
    };

    /** The desktop's shortcut list (MakeShortCutKeys), as a message box. */
    DV.showShortcuts = function (rows) {
        return DV.msg(rows.map(function (r) { return r[0] + '   ' + r[1]; }).join('\n'), 'ShortCut Keys');
    };

    /** OnlytextdecimelFunction: digits, one '.', and control keys. */
    DV.decimalOnly = function (input, allowDot) {
        input.addEventListener('keypress', function (e) {
            var ch = e.key;
            if (ch.length !== 1) return;
            if (/[0-9]/.test(ch)) return;
            if (ch === '.' && allowDot !== false && input.value.indexOf('.') < 0) return;
            e.preventDefault();
        });
    };
    /** OnlytextNumberFunction: digits only. */
    DV.digitsOnly = function (input) {
        input.addEventListener('keypress', function (e) {
            if (e.key.length === 1 && !/[0-9]/.test(e.key)) e.preventDefault();
        });
    };

    w.DV = DV;
})(window);
