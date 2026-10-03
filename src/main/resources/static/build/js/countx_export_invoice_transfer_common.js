/* ============================================================================================
 * countx_export_invoice_transfer_common.js - helpers shared by the Export screens of this pass:
 *   200 /export/performa-invoice, 201 /export/delivery-order-new, 202 /export/commercial-invoice-transfer.
 * busy() is the button contract: disabled + spinner (.is-busy) while a request runs, no duplicate requests,
 * re-enabled on success and on failure. Messages are the desktop's MessageBox texts (alert / confirm).
 * ============================================================================================ */
(function (global) {
    'use strict';

    function $id(id) { return document.getElementById(id); }
    function box(m) { global.alert(m); }
    function ask(m) { return global.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function str(v) { return v === null || v === undefined ? '' : String(v); }
    function netI(v) {
        if (typeof v === 'number') return isFinite(v) ? Math.trunc(v) : 0;
        var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim();
        if (!s) return 0;
        if (/^[-+]?\d+$/.test(s)) return parseInt(s, 10);
        var d = parseFloat(s); return isFinite(d) && /^[-+]?(\d+\.?\d*|\.\d+)$/.test(s) ? Math.round(d) : 0;
    }
    function netD(v) {
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim();
        if (!s || !/^[-+]?(\d+\.?\d*|\.\d+)([eE][-+]?\d+)?$/.test(s)) return 0;
        var n = parseFloat(s); return isFinite(n) ? n : 0;
    }
    /** Double.ToString(): no trailing zeros, 15 significant digits; Infinity / NaN as .NET Framework prints them. */
    function clr(v) {
        var n = (typeof v === 'number') ? v : netD(v);
        if (n === Infinity) return 'Infinity';
        if (n === -Infinity) return '-Infinity';
        if (isNaN(n)) return 'NaN';
        if (n === Math.round(n) && Math.abs(n) < 1e15) return String(n);
        return Number(n.toPrecision(15)).toString();
    }
    function roundAway(n, d) { var p = Math.pow(10, d); var r = Math.round(Math.abs(n) * p + 1e-9) / p; return n < 0 ? -r : r; }
    function roundEven(n, d) {
        var p = Math.pow(10, d || 0), x = n * p, f = Math.floor(x), diff = x - f;
        var r = Math.abs(diff - 0.5) < 1e-9 ? (f % 2 === 0 ? f : f + 1) : Math.round(x);
        return r / p;
    }
    function group(ip) { return ip.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /** .NET custom numeric format for the patterns the forms use: "0,0", "0,0.00", "#,##0.##", "N2", "N0". */
    function fmtNet(v, pattern) {
        var n = netD(v);
        if (pattern === 'N2' || pattern === 'N0' || pattern === 'N3') {
            var dd = +pattern.charAt(1), rr = roundAway(n, dd);
            var ps = Math.abs(rr).toFixed(dd).split('.');
            return (rr < 0 ? '-' : '') + group(ps[0]) + (ps[1] ? '.' + ps[1] : '');
        }
        var dot = pattern.indexOf('.');
        var intPat = dot >= 0 ? pattern.substring(0, dot) : pattern, decPat = dot >= 0 ? pattern.substring(dot + 1) : '';
        var fixedCount = (decPat.match(/0/g) || []).length, dec = decPat.length;
        var r = roundAway(n, dec), neg = r < 0; r = Math.abs(r);
        var parts = r.toFixed(dec).split('.'), ip = parts[0], fp = parts[1] || '';
        while (fp.length > fixedCount && fp.charAt(fp.length - 1) === '0') fp = fp.substring(0, fp.length - 1);
        var minInt = (intPat.match(/0/g) || []).length;
        if (minInt === 0 && ip === '0') ip = '';
        while (ip.length < minInt) ip = '0' + ip;
        var out = (intPat.indexOf(',') >= 0 ? group(ip) : ip) + (fp ? '.' + fp : '');
        if (out === '') return '';
        return (neg && /[1-9]/.test(out) ? '-' : '') + out;
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function addDays(iso, n) { var p = iso.split('-'); var d = new Date(+p[0], +p[1] - 1, +p[2]); d.setDate(d.getDate() + n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function isoDate(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(v).trim()); return m ? m[1] + '-' + m[2] + '-' + m[3] : ''; }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function ddMMMyyyy(v) { var s = isoDate(v); if (!s) return str(v); return s.substring(8, 10) + '-' + MON[+s.substring(5, 7) - 1] + '-' + s.substring(0, 4); }
    function shortDate(v) { var s = isoDate(v); if (!s) return str(v); return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4); }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve();
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(done, function (e) { done(); if (e && e.message) box(e.message); });
        done();
        return Promise.resolve(p);
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
    function refreshCombos() { if (global.DesktopCombo && global.DesktopCombo.refresh) global.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function visible(id) { var e = $id(id); return !!e && !e.classList.contains('is-hidden'); }
    /** Bind a <select> from rows; extra columns go to data-* attributes (multi-column combos use data-extra). */
    function bind(id, rows, valueCol, textCol, opts) {
        var s = $id(id); if (!s) return;
        opts = opts || {};
        var old = s.value, h = opts.noBlank ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) {
            var extra = '';
            if (opts.extra) extra = ' data-extra="' + esc(opts.extra.map(function (c) { return str(r[c]); }).join('|')) + '"';
            h += '<option value="' + esc(r[valueCol]) + '"' + extra + '>' + esc(r[textCol]) + '</option>';
        });
        s.innerHTML = h;
        if (opts.keep) { s.value = old; if (s.value !== old) s.value = ''; } else s.value = '';
        refreshCombos();
    }
    function setVal(id, v) {
        var s = $id(id); if (!s) return;
        var t = (v === null || v === undefined) ? '' : String(v);
        s.value = t; if (s.value !== t) s.value = '';
        refreshCombos();
    }
    /** UltraCombo.Text = x: selects the first option whose display text equals x. */
    function setByText(id, t) {
        var s = $id(id); if (!s) return false;
        t = str(t);
        for (var i = 0; i < s.options.length; i++) if (s.options[i].value !== '' && s.options[i].text === t) { s.selectedIndex = i; refreshCombos(); return true; }
        s.value = ''; refreshCombos(); return false;
    }
    function val(id) { var e = $id(id); if (!e) return ''; if (e.type === 'checkbox') return e.checked; return e.value; }
    function txt(id) { var s = $id(id); if (!s) return ''; if (s.tagName === 'SELECT') { var o = s.options[s.selectedIndex]; return o && s.value !== '' ? o.text : ''; } return s.value; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function checked(id) { var e = $id(id); return !!e && e.checked; }
    function setChecked(id, on) { var e = $id(id); if (e) e.checked = !!on; }
    function enable(id, on) { var e = $id(id); if (e) { e.disabled = !on; } refreshCombos(); }
    function focus(id) {
        var e = $id(id); if (!e) return;
        var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e;
        try { (t || e).focus(); } catch (x) { /* ignore */ }
    }
    function find(list, key, v) { for (var i = 0; i < (list || []).length; i++) if (str(list[i][key]) === str(v)) return list[i]; return null; }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function closeForm() { global.close(); setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150); }

    /**
     * Draw a desktop grid into <tbody id=bodyId> / <tfoot id=footId>.
     * cols: [{ key, fmt (pattern), num (bool), sum (pattern, Σ in footer), cls, render(row, i) }]
     * opts.lead(row, i) -> leading cells html; opts.rowAttr(row, i); opts.current index.
     */
    function drawGrid(bodyId, footId, rows, cols, opts) {
        opts = opts || {};
        var b = $id(bodyId), f = footId ? $id(footId) : null;
        if (!b) return;
        var h = '';
        (rows || []).forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (opts.current === i ? ' class="is-current"' : '') + (opts.rowAttr ? ' ' + opts.rowAttr(r, i) : '') + '>';
            if (opts.lead) h += opts.lead(r, i);
            cols.forEach(function (c) {
                var v = c.render ? c.render(r, i) : (c.fmt ? esc(fmtNet(r[c.key], c.fmt)) : esc(r[c.key]));
                h += '<td' + (c.num || c.fmt ? ' class="num"' : (c.cls ? ' class="' + c.cls + '"' : '')) + '>' + v + '</td>';
            });
            h += '</tr>';
        });
        b.innerHTML = h;
        if (f) {
            var any = cols.some(function (c) { return c.sum; });
            if (!any || !rows || !rows.length) { f.innerHTML = ''; return; }
            var ft = '<tr>';
            if (opts.leadCount) for (var k = 0; k < opts.leadCount; k++) ft += '<td' + (k === 0 ? ' class="lbl"' : '') + '>' + (k === 0 ? '&Sigma;' : '') + '</td>';
            cols.forEach(function (c, ci) {
                if (c.sum) {
                    var s = 0; rows.forEach(function (r) { s += netD(r[c.key]); });
                    ft += '<td class="num">' + esc(fmtNet(s, c.sum)) + '</td>';
                } else ft += '<td' + (!opts.leadCount && ci === 0 ? ' class="lbl"' : '') + '>' + (!opts.leadCount && ci === 0 ? '&Sigma;' : '') + '</td>';
            });
            f.innerHTML = ft + '</tr>';
        }
    }
    function wireFullscreen() {
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
    }
    /** Enter moves to the next control (SendKeys "{TAB}"). */
    function enterAsTab(e) {
        if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox' && e.target.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled):not([readonly]), select:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(e.target);
            if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
            return true;
        }
        return false;
    }
    /** Client-side filter row: filters a projected list by "contains" on every column (Janus FilterRow). */
    function filterRows(rows, filters) {
        var keys = Object.keys(filters || {}).filter(function (k) { return str(filters[k]).trim() !== ''; });
        if (!keys.length) return rows;
        return rows.filter(function (r) {
            return keys.every(function (k) { return str(r[k]).toLowerCase().indexOf(str(filters[k]).trim().toLowerCase()) >= 0; });
        });
    }

    global.ExR2 = {
        $id: $id, box: box, ask: ask, esc: esc, str: str, netI: netI, netD: netD, clr: clr, roundAway: roundAway, roundEven: roundEven,
        fmtNet: fmtNet, today: today, addDays: addDays, isoDate: isoDate, ddMMMyyyy: ddMMMyyyy, shortDate: shortDate, busy: busy,
        getJson: getJson, postJson: postJson, refreshCombos: refreshCombos, show: show, visible: visible, bind: bind, setVal: setVal,
        setByText: setByText, val: val, txt: txt, setText: setText, checked: checked, setChecked: setChecked, enable: enable, focus: focus,
        find: find, on: on, closeForm: closeForm, drawGrid: drawGrid, wireFullscreen: wireFullscreen, enterAsTab: enterAsTab, filterRows: filterRows
    };
}(window));
