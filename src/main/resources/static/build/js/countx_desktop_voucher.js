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
                        /* 2026-10-02 R2: a question asked per account (confirmKey) is acknowledged for that
                           account only - VoucherExistWithSameAmountInSameDate asks once per debit account. */
                        if (res.body.confirm === 'duplicate' && res.body.confirmKey != null) {
                            payload.duplicateAcknowledgedAccounts = (payload.duplicateAcknowledgedAccounts || []).concat([res.body.confirmKey]);
                        } else if (res.body.confirm === 'duplicate') payload.duplicateAcknowledged = true;
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
     * 2026-10-02 R3: the combo IS the project's searchable dropdown (countx_prod_combo.js,
     * window.DesktopCombo): a hidden native <select> built from the rows (store format:
     * data-columns="A|B|C" + data-extra per option) and enhanced by DesktopCombo.enhance. The
     * public API the voucher pages use (rows, row, value, setData, setValue, setFree, clear, text,
     * cell, find, focus, setEnabled, setDisplayKey, opts.limitToList, onChange, onLeave) is kept.
     * limitToList:false (the cheque-number combos) still lets the user type a value that is not in
     * the list, as the UltraCombo does.
     */
    DV.Combo = function (host, opts) {
        var self = this;
        this.opts = opts || {};
        this._rows = [];
        this.row = null;
        this.value = 0;
        this.host = host;
        host.classList.add('dv-combo');
        host.innerHTML = '';
        var sel = document.createElement('select');
        sel.className = 'dv-csel';
        host.appendChild(sel);
        this.sel = sel;
        this.pc = (w.DesktopCombo && typeof w.DesktopCombo.enhance === 'function') ? w.DesktopCombo.enhance(sel) : null;
        this.input = this.pc ? this.pc.input : sel;
        if (opts.id) this.input.id = opts.id;
        this.wrap = this.pc ? this.pc.wrap : host;
        this._typed = false;
        this.build();

        sel.addEventListener('change', function () {
            if (self._quiet) return;
            var v = sel.value;
            var r = (v === '' || v === '__free__') ? null : self.find(v);
            self.row = r;
            self.value = r ? Number(DV.pick(r, self.opts.valueKey)) : 0;
            self._typed = false;
            if (typeof self.opts.onChange === 'function') self.opts.onChange(self.row);
        });
        /* limitToList:false - printable keys type into the field instead of opening the search box */
        this.wrap.addEventListener('keydown', function (e) {
            if (self.opts.limitToList !== false || e.target !== self.input || e.ctrlKey || e.metaKey || e.altKey) return;
            if ((e.key && e.key.length === 1 && e.key !== ' ') || e.key === 'Backspace' || e.key === 'Delete') {
                self.input.readOnly = false;
                self.input.style.caretColor = 'auto';
                e.stopPropagation();
            }
        }, true);
        /* ... a click in the field puts the caret there (the arrow still opens the list) ... */
        this.wrap.addEventListener('mousedown', function (e) {
            if (self.opts.limitToList !== false || e.target !== self.input || self.sel.disabled) return;
            self.input.readOnly = false;
            self.input.style.caretColor = 'auto';
            e.stopPropagation();
        }, true);
        /* ... and Enter / Tab in the search box with no matching row takes the typed text as the value */
        if (this.pc) this.wrap.addEventListener('keydown', function (e) {
            if (self.opts.limitToList !== false || e.target !== self.pc.search) return;
            if ((e.key !== 'Enter' && e.key !== 'Tab') || self.pc.active >= 0) return;
            var t = self.pc.search.value.trim();
            if (!t) return;
            if (e.key === 'Enter') e.preventDefault();
            e.stopPropagation();
            self.pc.closePop(true);
            self.input.value = t;
            self._typed = true;
            if (e.key === 'Enter') self.commitText();
        }, true);
        this.input.addEventListener('input', function () { if (self.opts.limitToList === false) self._typed = true; });
        this.wrap.addEventListener('focusout', function () {
            setTimeout(function () {
                if (self.wrap.contains(document.activeElement)) return;
                self.commitText();
                if (self.pc) { self.input.readOnly = true; self.input.style.caretColor = ''; }
                if (typeof self.opts.onLeave === 'function') self.opts.onLeave(self.row);
            }, 0);
        });
    };
    Object.defineProperty(DV.Combo.prototype, 'rows', {
        get: function () { return this._rows; },
        set: function (v) { this._rows = v || []; this.build(); }
    });
    DV.Combo.prototype.cols = function () {
        var dk = this.opts.displayKey, hd = this.opts.header;
        var cols = (this.opts.columns || [{ key: dk, caption: hd || dk }]).filter(function (c) { return !c.hidden; });
        var first = null, rest = [];
        cols.forEach(function (c) { if (!first && c.key === dk) first = c; else rest.push(c); });
        first = { key: dk, caption: hd || (first && first.caption) || dk };
        return [first].concat(rest);
    };
    /** Rebuild the <option> list from this.rows, keeping the current row selected. */
    DV.Combo.prototype.build = function () {
        var cols = this.cols(), vk = this.opts.valueKey;
        this.sel.setAttribute('data-columns', cols.map(function (c) { return String(c.caption || c.key).replace(/\|/g, '/'); }).join('|'));
        var h = '<option value=""></option>';
        for (var i = 0; i < this._rows.length; i++) {
            var r = this._rows[i];
            var extra = [];
            for (var c = 1; c < cols.length; c++) { var x = DV.pick(r, cols[c].key); extra.push(x == null ? '' : String(x).replace(/\|/g, '/')); }
            var t = DV.pick(r, cols[0].key);
            h += '<option value="' + DV.esc(DV.pick(r, vk)) + '"' + (extra.length ? ' data-extra="' + DV.esc(extra.join('|')) + '"' : '') + '>'
                + DV.esc(t == null ? '' : t) + '</option>';
        }
        this._quiet = true;
        this.sel.innerHTML = h;
        var keep = this.row && this.find(DV.pick(this.row, vk));
        if (keep) this.sel.value = String(DV.pick(keep, vk)); else this.sel.value = '';
        if (!keep && this.row) { this.row = null; this.value = 0; }
        this._quiet = false;
        if (this.pc) this.pc.syncFromSelect();
    };
    DV.Combo.prototype.setData = function (rows, keepValue) {
        var keep = keepValue ? this.value : 0;
        this.row = null; this.value = 0;
        this.rows = rows || [];
        if (keep) this.setValue(keep, true); else this.clear(true);
    };
    DV.Combo.prototype.setDisplayKey = function (key, header) {
        this.opts.displayKey = key;
        if (header) this.opts.header = header;
        this.build();
    };
    DV.Combo.prototype.find = function (v) {
        var vk = this.opts.valueKey;
        for (var i = 0; i < this._rows.length; i++) {
            if (String(DV.pick(this._rows[i], vk)) === String(v)) return this._rows[i];
        }
        return null;
    };
    DV.Combo.prototype._select = function (val, text) {
        this._quiet = true;
        var free = this.sel.querySelector('option[value="__free__"]');
        if (val === '__free__') {
            if (!free) { free = document.createElement('option'); free.value = '__free__'; free.hidden = true; this.sel.appendChild(free); }
            free.textContent = text == null ? '' : text;
        } else if (free) { free.parentNode.removeChild(free); }
        this.sel.value = val;
        this._quiet = false;
        if (this.pc) this.pc.syncFromSelect();
        this._typed = false;
    };
    DV.Combo.prototype.setValue = function (v, silent) {
        var r = (v === null || v === undefined || v === '' || Number(v) === 0) ? null : this.find(v);
        this.row = r;
        this.value = r ? Number(DV.pick(r, this.opts.valueKey)) : 0;
        this._select(r ? String(DV.pick(r, this.opts.valueKey)) : '');
        if (!silent && typeof this.opts.onChange === 'function') this.opts.onChange(this.row);
        return !!r;
    };
    /** A value that is not in the list, shown as text (the desktop does this for cheque numbers). */
    DV.Combo.prototype.setFree = function (value, text) {
        this.row = null;
        this.value = Number(value) || 0;
        this._select('__free__', text);
    };
    DV.Combo.prototype.clear = function (silent) {
        this.row = null; this.value = 0;
        this._select('');
        if (!silent && typeof this.opts.onChange === 'function') this.opts.onChange(null);
    };
    DV.Combo.prototype.text = function () {
        if (this.pc) return this.input.value;
        var o = this.sel.options[this.sel.selectedIndex];
        return o ? o.textContent : '';
    };
    DV.Combo.prototype.cell = function (key) { return this.row ? DV.pick(this.row, key) : undefined; };
    DV.Combo.prototype.focus = function () { this.input.focus(); };
    DV.Combo.prototype.setEnabled = function (on) { this.sel.disabled = !on; if (this.pc) this.pc.reflectDisabled(); };
    /** Text typed into a limitToList:false combo: a list entry when it matches one, otherwise free text. */
    DV.Combo.prototype.commitText = function () {
        if (!this._typed) return;
        this._typed = false;
        var t = this.input.value.trim(), dk = this.opts.displayKey, match = null;
        if (t === '') { var had = this.row || this.value; this.clear(true); if (had && typeof this.opts.onChange === 'function') this.opts.onChange(null); return; }
        for (var i = 0; i < this._rows.length; i++) {
            if (String(DV.pick(this._rows[i], dk) || '').toLowerCase() === t.toLowerCase()) { match = this._rows[i]; break; }
        }
        if (match) { this.setValue(DV.pick(match, this.opts.valueKey)); return; }
        var hadRow = this.row;
        this.setFree(0, t);
        if (hadRow && typeof this.opts.onChange === 'function') this.opts.onChange(null);
    };
    DV.Combo.prototype.open = function () { if (this.pc) { this.input.focus(); this.pc.openPop(''); } };
    DV.Combo.prototype.close = function () { if (this.pc) this.pc.closePop(false); };
    DV.Combo.prototype.toggle = function () { if (this.pc && this.pc.open) this.close(); else this.open(); };
    /** 2026-10-02 R3: a FlowLayoutPanel whose panels wrap onto a third line (855 with every feature on)
     *  grows its GroupBox and the designer area instead of hiding the "+" button. */
    DV.fitFlow = function (flow, box, area, base) {
        if (!flow) return;
        var extra = Math.max(0, flow.scrollHeight - base);
        [box, area].forEach(function (el) {
            if (!el) return;
            if (el.__dvH == null) el.__dvH = parseInt(el.style.height, 10) || el.offsetHeight;
            el.style.height = (el.__dvH + extra) + 'px';
        });
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

    // ------------------------------------------------- 2026-10-02 R2 additions (additive only)
    /** A server button: disabled at once with a spinner, a repeat click is refused while the request
     *  runs, and the button is restored (to the state it had before) on success AND on failure. */
    DV.busy = function (btn, fn) {
        if (!btn) { try { return Promise.resolve(fn()); } catch (e) { return Promise.reject(e); } }
        if (btn.getAttribute('data-dv-busy') === '1') return Promise.resolve(null);
        var wasDisabled = btn.disabled;
        btn.setAttribute('data-dv-busy', '1');
        btn.disabled = true;
        btn.classList.add('dv-spin');
        function done() { btn.removeAttribute('data-dv-busy'); btn.classList.remove('dv-spin'); btn.disabled = wasDisabled; }
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { done(); return Promise.reject(e); }
        return p.then(function (v) { done(); return v; }, function (e) { done(); throw e; });
    };
    /** A PDF print in a new window (opened at once, so a pop-up blocker lets it through); the
     *  server's text ("No Record Found For Display" ...) is shown as a message box. */
    DV.printPdf = function (url) {
        var win = null;
        try { win = window.open('about:blank', '_blank'); } catch (e) { win = null; }
        return fetch(url, { credentials: 'same-origin' }).then(function (r) {
            var type = r.headers.get('Content-Type') || '';
            if (r.ok && type.indexOf('application/pdf') === 0) {
                return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (win) win.location = u; else window.open(u, '_blank'); });
            }
            return r.text().then(function (t) { try { if (win) win.close(); } catch (x) { } return DV.msg(t || ('Print failed (' + r.status + ')')); });
        }).catch(function (e) { try { if (win) win.close(); } catch (x) { } return DV.msg(e.message); });
    };
    /** CommonServices.AcRptPaymentReceiptsVoucherSlip_102(VoucherHeadId, DocumentTypeId) (:6129) -
     *  VoucherReports.VoucherReport {Id, DocumentTypeId, ApprovedFilter "All"} ->
     *  Sp_Vouchers_PaymentReceiptVoucherSlip_Rpt, 102-AcRptPaymentReceiptsVoucherSlip.rpt
     *  (the registered print contract "acc-102": @Id, @DocumentTypeId; @IsApproved not sent). */
    DV.printAcRpt102 = function (id, documentTypeId) {
        if (!(Number(id) > 0)) return DV.msg('VoucherId Not Found');
        return DV.printPdf('/api/print/acc-102/pdf?id=' + encodeURIComponent(id)
            + '&documentTypeId=' + encodeURIComponent(documentTypeId || 0));
    };
    /** CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(VoucherHeadId) (:5841) -
     *  102-ANewAcRptPaymentReceiptsVoucherSlip.rpt through print-rpt.js (the traced contract). */
    DV.printANew102 = function (id) {
        if (!(Number(id) > 0)) return DV.msg('VoucherId Not Found');
        if (typeof window.printRpt === 'function') return window.printRpt('102-ANewAcRptPaymentReceiptsVoucherSlip.rpt', { id: Number(id) });
        return DV.printPdf('/api/print/by-template/102-ANewAcRptPaymentReceiptsVoucherSlip.rpt/pdf?id=' + encodeURIComponent(id));
    };
    /** The forms' KeyDown: e.KeyData == Keys.Return -> SendKeys.Send("{TAB}") - Enter moves to the
     *  next field. A combo / message box that uses Enter itself (preventDefault) keeps it. */
    DV.enterAsTab = function (root) {
        (root || document).addEventListener('keydown', function (e) {
            if (e.key !== 'Enter' || e.defaultPrevented || e.ctrlKey || e.altKey || e.shiftKey || e.metaKey) return;
            var t = e.target;
            if (!t || !t.tagName || (t.tagName !== 'INPUT' && t.tagName !== 'SELECT')) return;
            if (document.querySelector('.dv-modal-back')) return;
            if (t.closest && (t.closest('.dtcombo-pop') || t.closest('.dv-combo-panel'))) return;
            var list = Array.prototype.filter.call(document.querySelectorAll('input,select,textarea,button,a[href],[tabindex]'), function (el) {
                return !el.disabled && el.tabIndex >= 0 && el.type !== 'hidden' && el.offsetParent !== null;
            });
            var i = list.indexOf(t);
            if (i >= 0 && i + 1 < list.length) { e.preventDefault(); list[i + 1].focus(); }
        });
    };

    w.DV = DV;
})(window);
