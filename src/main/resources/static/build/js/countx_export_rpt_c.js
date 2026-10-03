/* ============================================================================================
 * countx_export_rpt_c.js - shared helpers of the Export report pages built by agent C
 * (241 242 243 244 245 246 256 257 258 203 859). The same contract as countx_export_gd_break_up.js:
 * busy() button contract (disabled + spinner, duplicates ignored, re-enabled on success and failure),
 * getJson/postJson with the CSRF meta, bind()/setVal() for the searchable combos (countx_prod_combo.js),
 * the desktop's number / date formats, CommonServices.DateType() + the cmbperemeter_ValueChanged date
 * rules, a grid renderer with the Janus Sum totals, the fullscreen toggle and print-rpt.js printing.
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
    /* Conversion.ToInt / ToDouble: thousands separators parse, anything else is 0. */
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function group(s) { var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.'); }
    /* "#,##0.00" (stringFormatsingle / ...ForFcy) */
    function fmtAmt(v) { return group(netD(v).toFixed(2)); }
    /* "#,##0.00;(#,##0.00)" (stringFormatboth / ...ForFcy) */
    function fmtBoth(v) { var n = netD(v); return n < 0 ? '(' + group(Math.abs(n).toFixed(2)) + ')' : group(n.toFixed(2)); }
    /* "#,##0.0000" (DecimalRateFormate / DecimalFCYRateFormate) */
    function fmtRate(v) { return group(netD(v).toFixed(4)); }
    /* "#,##0.###" / "#,##0.##" / "#,##0" */
    function fmtOpt(v, dec) {
        var s = netD(v).toFixed(dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        return group(s);
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function isoOf(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return isoOf(new Date()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return isoOf(d); }
    function isoDate(v) {
        var s = str(v).trim();
        if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        return isNaN(d.getTime()) ? '' : isoOf(d);
    }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    /* FormatString "dd-MMM-yy" (the grids) - a null / 01-Jan-0001 / 01-Jan-1900 date is blank. */
    function dmy(v) {
        var s = isoDate(v); if (!s) return '';
        if (s.substring(0, 4) === '0001' || s === '1900-01-01') return '';
        return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(2, 4);
    }
    /* DateTime.ToShortDateString() under the desktop culture: M/d/yyyy. */
    function shortDate(v) {
        var s = isoDate(v); if (!s) return '';
        return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4);
    }
    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return Promise.resolve();
            b.disabled = true; b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); box(e.message); return Promise.resolve(); }
        if (p && typeof p.then === 'function') return p.then(function (r) { done(); return r; }, function (e) { done(); if (e && e.message) box(e.message); });
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
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data || {}) }).then(parse);
    }
    function refreshCombos() { if (global.DesktopCombo) global.DesktopCombo.refresh(); }
    function show(id, on) { var e = $id(id); if (e) e.classList.toggle('is-hidden', !on); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    /**
     * DropDownBind.BindDDL / BindDDLNew: options from the rows, the previous value kept when still listed.
     * The empty first option is the combo with no active row (Text = string.Empty).
     */
    function bind(id, rows, valueCol, textCol, extraCols) {
        var s = $id(id); if (!s) return;
        var keep = s.value;
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) {
            var extra = (extraCols || []).map(function (k) { return str(col(r, k)).replace(/\|/g, '/'); }).join('|');
            h += '<option value="' + esc(col(r, valueCol)) + '"' + (extraCols && extraCols.length ? ' data-extra="' + esc(extra) + '"' : '') + '>' + esc(col(r, textCol)) + '</option>';
        });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.value = '0';
        refreshCombos();
    }
    function setVal(id, v) {
        var s = $id(id); if (!s) return;
        s.value = str(v);
        if (s.value !== str(v)) s.value = '0';
        refreshCombos();
    }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) { if ('value' in e && e.tagName !== 'SPAN' && e.tagName !== 'LABEL' && e.tagName !== 'DIV') e.value = str(v); else e.textContent = str(v); } }
    function checked(id) { var e = $id(id); return !!e && !!e.checked; }
    function setChecked(id, on) { var e = $id(id); if (e) e.checked = !!on; }
    function selText(id) { var s = $id(id); return s && s.selectedIndex >= 0 && s.value !== '0' ? s.options[s.selectedIndex].textContent.trim() : ''; }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function cancel() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150);
    }

    /* CommonServices.DateType(): the fixed Id / Parameters table. */
    function bindDateTypes(id, rows) { bind(id, rows || [{ Id: 1, Parameters: 'This Day' }, { Id: 2, Parameters: 'This Week' }, { Id: 3, Parameters: 'This Month' }, { Id: 4, Parameters: 'This Year' }, { Id: 5, Parameters: 'Financial Year' }], 'Id', 'Parameters', []); }
    /**
     * cmbperemeter_ValueChanged: 1 today, 2 today-7, 3 first of month (+ To = today), 4 1-Jan (+ To = today),
     * 5 ActiveYr.Start_Period. {@code toAlso} adds "To = today" to 5 too (243 does that).
     */
    function applyDateType(v, fromId, toId, yearStart, toAlso5) {
        var n = netI(v), d = new Date();
        if (n === 1) setText(fromId, today());
        else if (n === 2) setText(fromId, daysAgo(7));
        else if (n === 3) { setText(fromId, isoOf(new Date(d.getFullYear(), d.getMonth(), 1))); setText(toId, today()); }
        else if (n === 4) { setText(fromId, d.getFullYear() + '-01-01'); setText(toId, today()); }
        else if (n === 5) { if (yearStart) setText(fromId, isoDate(yearStart)); if (toAlso5) setText(toId, today()); }
    }

    /* Cell format by kind. */
    function cell(kind, v) {
        switch (kind) {
            case 'amt': return { v: fmtAmt(v), num: true };
            case 'both': return { v: fmtBoth(v), num: true };
            case 'rate': return { v: fmtRate(v), num: true };
            case 'q3': return { v: fmtOpt(v, 3), num: true };
            case 'q2': return { v: fmtOpt(v, 2), num: true };
            case 'int': return { v: fmtOpt(v, 0), num: true };
            case 'date': return { v: dmy(v) };
            case 'short': return { v: shortDate(v) };
            default: return { v: str(v) };
        }
    }
    /**
     * Janus GridEX projection: cols = [{key, cap, kind, sum, link, hidden, cls, render}], an optional
     * leading cell builder (buttons / checkbox), the Σ row over the sum columns. Header comes from cols.
     */
    function drawGrid(tableId, rows, cols, opt) {
        opt = opt || {};
        var t = $id(tableId); if (!t) return;
        var head = t.tHead || t.createTHead(), body = t.tBodies[0] || t.createTBody(), foot = t.tFoot || t.createTFoot();
        var vis = cols.filter(function (c) { return !c.hidden; });
        var hh = '<tr>' + (opt.leadHead || '');
        vis.forEach(function (c) { var f = cell(c.kind, 0); hh += '<th' + (f.num ? ' class="num"' : '') + ' data-col="' + esc(c.key) + '">' + esc(c.cap || c.key) + '</th>'; });
        head.innerHTML = hh + '</tr>';
        var sums = {};
        body.innerHTML = rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === opt.cur ? ' class="is-current"' : '') + '>' + (opt.lead ? opt.lead(r, i) : '');
            vis.forEach(function (c) {
                var raw = col(r, c.key);
                if (c.sum) sums[c.key] = (sums[c.key] || 0) + netD(raw);
                var inner, f = cell(c.kind, raw), cls = f.num ? 'num' : '';
                if (c.render) inner = c.render(r, i, f.v);
                else if (c.link && f.v !== '') inner = '<a class="win-code" data-i="' + i + '" data-link="' + esc(c.key) + '" href="javascript:void(0)">' + esc(f.v) + '</a>';
                else inner = esc(f.v);
                if (c.cls) { var x = c.cls(r, i); if (x) cls += ' ' + x; }
                h += '<td' + (cls ? ' class="' + cls + '"' : '') + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        if (!rows.length) { foot.innerHTML = ''; return; }
        var any = vis.some(function (c) { return c.sum; });
        if (!any && !opt.alwaysFoot) { foot.innerHTML = ''; return; }
        var tf = '<tr>' + (opt.lead ? '<td class="lbl">&Sigma;</td>' : '');
        vis.forEach(function (c, idx) {
            var f = cell(c.kind, 0);
            var v = c.sum ? cell(c.kind, sums[c.key] || 0).v : '';
            var s = esc(v);
            if (idx === 0 && !opt.lead && !c.sum) s = '&Sigma;';
            tf += '<td' + (f.num ? '' : ' class="lbl"') + '>' + s + '</td>';
        });
        foot.innerHTML = tf + '</tr>';
    }
    function clearGrid(tableId) { var t = $id(tableId); if (!t) return; if (t.tBodies[0]) t.tBodies[0].innerHTML = ''; if (t.tFoot) t.tFoot.innerHTML = ''; }
    /* row selection + double-click + code links + buttons */
    function wireGrid(tableId, h) {
        var t = $id(tableId); if (!t) return;
        h = h || {};
        t.addEventListener('click', function (e) {
            var a = e.target.closest('a.win-code[data-i]');
            if (a && h.link) { h.link(+a.getAttribute('data-i'), a.getAttribute('data-link')); return; }
            var b = e.target.closest('button[data-act]');
            if (b && h.button) { h.button(+b.getAttribute('data-i'), b.getAttribute('data-act'), b); return; }
            var tr = e.target.closest('tr[data-i]');
            if (tr && h.select) {
                t.querySelectorAll('tbody tr.is-current').forEach(function (x) { x.classList.remove('is-current'); });
                tr.classList.add('is-current');
                h.select(+tr.getAttribute('data-i'));
            }
        });
        if (h.open) t.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('input,select,button,a')) h.open(+tr.getAttribute('data-i')); });
    }
    function wireFullscreen() {
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
    }
    /* Enter moves to the next field (SendKeys {TAB}); Ctrl+E / Esc close; the page adds its own keys. */
    function wireKeys(extra) {
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox' && e.target.type !== 'radio' && !e.target.closest('.win-grid')) {
                var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea'), function (x) { return x.offsetParent !== null; });
                var i = f.indexOf(e.target);
                if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
                return;
            }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); return; }
            if (e.ctrlKey && e.altKey && !e.key.match(/^(Control|Alt)$/)) { if (extra && extra.shortcuts) { e.preventDefault(); extra.shortcuts(); } return; }
            if (extra && extra.handle) extra.handle(e, k);
        });
    }
    /* ShortCutKeyPopUp - the desktop lists KeyCombination / Description in a small form. */
    function shortcuts(rows) {
        var t = 'ShortCut Keys\n\n';
        (rows || []).forEach(function (r) { t += r[0] + '\t' + r[1] + '\n'; });
        box(t);
    }
    /* Reporting.ShowReportWithDataTable(dt, rpt) -> the same .rpt name through print-rpt.js (Jasper PDF). */
    function print(rpt, args, btn) {
        if (typeof global.printRpt !== 'function') { box('Printing is not available on this page.'); return Promise.resolve(); }
        return busy(btn, function () { return global.printRpt(rpt, args || {}, null); });
    }
    function focusGrid(tableId) { var tr = $id(tableId) && $id(tableId).querySelector('tbody tr'); if (tr) { tr.classList.add('is-current'); tr.scrollIntoView({ block: 'nearest' }); } }

    global.ExRptC = {
        $id: $id, box: box, ask: ask, esc: esc, str: str, netI: netI, netD: netD,
        fmtAmt: fmtAmt, fmtBoth: fmtBoth, fmtRate: fmtRate, fmtOpt: fmtOpt, today: today, daysAgo: daysAgo, isoDate: isoDate, dmy: dmy, shortDate: shortDate,
        busy: busy, getJson: getJson, postJson: postJson, refreshCombos: refreshCombos, show: show, col: col, bind: bind, setVal: setVal, val: val,
        setText: setText, checked: checked, setChecked: setChecked, selText: selText, focus: focus, cancel: cancel,
        bindDateTypes: bindDateTypes, applyDateType: applyDateType, cell: cell, drawGrid: drawGrid, clearGrid: clearGrid, wireGrid: wireGrid,
        wireFullscreen: wireFullscreen, wireKeys: wireKeys, shortcuts: shortcuts, print: print, focusGrid: focusGrid
    };
}(window));
