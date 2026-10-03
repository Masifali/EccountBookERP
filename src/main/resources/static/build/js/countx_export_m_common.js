/* ============================================================================================
 * countx_export_m_common.js - shared helpers of the batch-M Export pages:
 *   /export/export-opening (189 ExportOpening), /export/return-grn (190 ExportReturn_Grn),
 *   /export/return-invoice (191 ExportReturnInvoice).
 * window.ExM: DOM / number / date helpers in the desktop's formats, the button contract (busy()),
 * JSON calls with the CSRF meta, searchable-combo binding (countx_prod_combo.js), a GridEX-like
 * table renderer (hidden columns, Σ totals of numeric columns, editable cells, button columns,
 * clickable codes, current row), tabs, fullscreen boxes and modal popups.
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
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function group(s) { var p = s.split('.'); p[0] = p[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return p.join('.'); }
    /* .NET "#,##0.<d zeros>" (d = 0: "#,##0."): fixed decimals, AwayFromZero. */
    function fixed(v, d) {
        var n = netD(v); d = d || 0;
        var f = Math.pow(10, d), r = Math.round(Math.abs(n) * f + 1e-9) / f * (n < 0 ? -1 : 1);
        return group(r.toFixed(d));
    }
    /* .NET "#,##0.###" (optional decimals); zeroEmpty for "#,#.###" / "#,#" (0 prints nothing). */
    function opt(v, d, zeroEmpty) {
        var n = netD(v);
        if (zeroEmpty && n === 0) return '';
        var s = fixed(n, d);
        if (s.indexOf('.') >= 0) s = s.replace(/\.?0+$/, '');
        return s;
    }
    /* double.ToString() / decimal.ToString() for a textbox the desktop fills with a raw number. */
    function raw(v) { var n = netD(v); return String(Math.round(n * 1e10) / 1e10); }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function isoDate(v) {
        var s = str(v).trim(); if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        return isNaN(d.getTime()) ? '' : d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
    }
    /* GridEX DateTime column: short date (M/d/yyyy). */
    function shortDate(v) { var s = isoDate(v); if (!s || s.substring(0, 4) === '0001') return ''; return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4); }
    /* "dd-MMM-yyyy". */
    function ddMMMyyyy(v) { var s = isoDate(v); if (!s) return ''; return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(0, 4); }
    /* "dd-MM-yyyy hh:mm tt" / "dd-MMM-yyyy hh:mm tt". */
    function dateTime(v, monName) {
        var s = str(v).trim(); if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(s);
        if (!m) return monName ? ddMMMyyyy(s) : shortDate(s);
        var h = parseInt(m[4], 10), tt = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + (monName ? MON[parseInt(m[2], 10) - 1] : m[2]) + '-' + m[1] + ' ' + pad(h) + ':' + m[5] + ' ' + tt;
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
    function csrf(h) {
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
        return h;
    }
    function postJson(url, data) {
        return fetch(url, { method: 'POST', headers: csrf({ 'Accept': 'application/json', 'Content-Type': 'application/json' }), credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
    }
    /* A report endpoint that answers with a PDF (POST JSON); opened in a tab reserved inside the click. */
    function printPdf(url, data, btn) {
        var w = null;
        try { w = global.open('', '_blank'); } catch (e) { w = null; }
        return busy(btn, function () {
            return fetch(url, { method: 'POST', headers: csrf({ 'Content-Type': 'application/json', 'Accept': 'application/pdf, text/plain' }), credentials: 'same-origin', body: JSON.stringify(data || {}) })
                .then(function (r) {
                    var type = r.headers.get('Content-Type') || '';
                    if (r.ok && type.indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else global.open(u, '_blank'); });
                    return r.text().then(function (t) { if (w) w.close(); box(t || ('Print failed (' + r.status + ')')); });
                }).catch(function (e) { if (w) w.close(); throw e; });
        });
    }

    function refreshCombos() { if (global.DesktopCombo) global.DesktopCombo.refresh(); }
    function col(row, name) {
        if (!row) return null;
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        var l = name.toLowerCase();
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === l) return row[k];
        return null;
    }
    /* Bind a <select class="win-combo dtcombo">: rows -> options (value / text, extra columns as data-extra),
       keeping the current value when it still exists (InfragisticsHelper.BindAndRetainSelection). */
    function bind(id, rows, valueCol, textCol, extraCols, keepValue) {
        var s = $id(id); if (!s) return;
        var keep = keepValue === undefined ? s.value : String(keepValue);
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) {
            var ex = (extraCols || []).map(function (c) { return str(col(r, c)).replace(/\|/g, '/'); }).join('|');
            h += '<option value="' + esc(col(r, valueCol)) + '"' + (extraCols && extraCols.length ? ' data-extra="' + esc(ex) + '"' : '') + '>' + esc(col(r, textCol)) + '</option>';
        });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.value = '0';
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; var t = str(v); s.value = t; if (s.value !== t) s.value = s.tagName === 'SELECT' ? '0' : ''; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function vint(id) { return netI(val(id)); }
    function text(id) { var s = $id(id); if (!s) return ''; if (s.tagName === 'SELECT') { var o = s.options[s.selectedIndex]; return o && s.value !== '0' ? o.text : ''; } return s.value; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function show(id, on) { var e = typeof id === 'string' ? $id(id) : id; if (e) e.classList.toggle('is-hidden', !on); }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    /* UltraCombo Leave: focus left the wrap (combo + its popup). */
    function onLeave(id, fn) {
        var s = $id(id); if (!s) return;
        var attach = function () {
            var w = s.closest('.dtcombo-wrap') || s;
            if (w.dataset.exLeave) return;
            w.dataset.exLeave = '1';
            w.addEventListener('focusout', function (e) { if (e.relatedTarget && w.contains(e.relatedTarget)) return; setTimeout(function () { if (!w.contains(document.activeElement)) fn(); }, 0); });
        };
        setTimeout(attach, 0);
        s.addEventListener('change', function () { attach(); });
    }
    function radio(name) { var r = document.querySelector('input[name="' + name + '"]:checked'); return r ? r.value : ''; }

    // ------------------------------------------------------------------------------ grid

    /**
     * grid(tableId, spec): spec.cols = [{ k, t, num (decimals or true = 3 optional), fixed (decimals), date ('short' | 'dt' | 'dmy'),
     * hide, edit ('num' | 'text'), w, code (row => click handler name) }]; spec.lead = [{ t, html(row, i) }] button columns
     * before the data; spec.rows; spec.current; spec.onRow(i); spec.onDbl(i); spec.onEdit(i, k, value, input); spec.total (default true).
     * A numeric column (num / fixed) gets a Σ total in the footer.
     */
    function grid(tableId, spec) {
        var t = $id(tableId); if (!t) return;
        var cols = spec.cols.filter(function (c) { return !c.hide; });
        var lead = spec.lead || [];
        var rows = spec.rows || [];
        var h = '<thead><tr>';
        lead.forEach(function (l) { h += '<th class="ctr">' + esc(l.t) + '</th>'; });
        cols.forEach(function (c) { h += '<th' + (c.num !== undefined || c.fixed !== undefined ? ' class="num"' : '') + (c.w ? ' style="min-width:' + c.w + 'px"' : '') + '>' + esc(c.t || c.k) + '</th>'; });
        h += '</tr></thead><tbody>';
        var sums = {};
        rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (spec.current === i ? ' class="is-current"' : '') + '>';
            lead.forEach(function (l) { h += '<td class="win-cell-btn">' + l.html(r, i) + '</td>'; });
            cols.forEach(function (c) {
                var v = r[c.k];
                var isNum = c.num !== undefined || c.fixed !== undefined;
                if (isNum) sums[c.k] = (sums[c.k] || 0) + netD(v);
                var shown = isNum ? (c.fixed !== undefined ? fixed(v, c.fixed) : opt(v, c.num === true ? 3 : c.num, !!c.zeroEmpty))
                    : c.date === 'short' ? shortDate(v) : c.date === 'dt' ? dateTime(v) : c.date === 'dmy' ? ddMMMyyyy(v) : c.date === 'dtm' ? dateTime(v, true)
                    : c.bool ? (v ? '&#10004;' : '') : esc(v);
                if (c.bool) shown = v ? '&#10004;' : '';
                if (c.edit) {
                    h += '<td class="win-editable' + (isNum ? ' num' : '') + '"><input type="text" class="' + (c.edit === 'num' ? 'num' : '') + '" data-k="' + esc(c.k) + '" value="' + esc(c.edit === 'num' ? raw(v) : str(v)) + '"' + (c.edit === 'num' ? ' data-guard="decimal"' : '') + '/></td>';
                } else if (c.code && str(v) !== '') {
                    h += '<td' + (isNum ? ' class="num"' : '') + '><a class="win-code" data-code="' + esc(c.k) + '">' + shown + '</a></td>';
                } else {
                    h += '<td' + (isNum ? ' class="num"' : (c.bool ? ' class="ctr"' : '')) + '>' + shown + '</td>';
                }
            });
            h += '</tr>';
        });
        h += '</tbody>';
        if (spec.total !== false && rows.length) {
            h += '<tfoot><tr>';
            lead.forEach(function (l, idx) { h += '<td class="lbl">' + (idx === 0 ? '&Sigma;' : '') + '</td>'; });
            cols.forEach(function (c, idx) {
                var isNum = c.num !== undefined || c.fixed !== undefined;
                h += '<td' + (isNum ? '' : ' class="lbl"') + '>' + (isNum && c.noTotal ? '' : isNum ? esc(c.fixed !== undefined ? fixed(sums[c.k] || 0, c.fixed) : opt(sums[c.k] || 0, c.num === true ? 3 : c.num)) : (idx === 0 && !lead.length ? '&Sigma;' : '')) + '</td>';
            });
            h += '</tr></tfoot>';
        }
        t.innerHTML = h;
        var tb = t.tBodies[0];
        tb.onclick = function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var i = parseInt(tr.getAttribute('data-i'), 10);
            Array.prototype.forEach.call(tb.rows, function (x) { x.classList.toggle('is-current', x === tr); });
            var a = e.target.closest('a.win-code');
            if (a && spec.onCode) { spec.onCode(i, a.getAttribute('data-code')); return; }
            var b = e.target.closest('button[data-act]');
            if (b && spec.onButton) { spec.onButton(i, b.getAttribute('data-act'), b); return; }
            if (e.target.tagName !== 'INPUT' && spec.onRow) spec.onRow(i);
        };
        tb.ondblclick = function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && spec.onDbl && e.target.tagName !== 'INPUT') spec.onDbl(parseInt(tr.getAttribute('data-i'), 10)); };
        tb.onchange = function (e) {
            if (e.target.tagName !== 'INPUT' || !spec.onEdit) return;
            if (e.target.type === 'checkbox') return;
            var tr = e.target.closest('tr[data-i]');
            spec.onEdit(parseInt(tr.getAttribute('data-i'), 10), e.target.getAttribute('data-k'), e.target.value, e.target);
        };
        tb.onkeydown = function (e) { if (spec.onKey) { var tr = e.target.closest('tr[data-i]'); if (tr) spec.onKey(parseInt(tr.getAttribute('data-i'), 10), e); } };
        if (spec.empty) show(spec.empty, rows.length === 0);
        if (global.CountxInputGuards && global.CountxInputGuards.scan) { try { global.CountxInputGuards.scan(t); } catch (x) { /* ignore */ } }
    }
    function btnHtml(act, label, cls, disabled) { return '<button type="button" class="' + (cls || 'win-edit') + '" data-act="' + act + '"' + (disabled ? ' disabled' : '') + '>' + esc(label) + '</button>'; }
    function checkHtml(act, checked) { return '<input type="checkbox" data-act="' + act + '"' + (checked ? ' checked' : '') + '/>'; }

    // ------------------------------------------------------------------------------ tabs / boxes / modal

    function tabs(group, onChange) {
        document.querySelectorAll('.win-tabs[data-tabs="' + group + '"] .win-tab').forEach(function (b) {
            b.addEventListener('click', function () { selectTab(group, b.getAttribute('data-tab')); if (onChange) onChange(b.getAttribute('data-tab')); });
        });
    }
    function selectTab(group, panelId) {
        var bar = document.querySelector('.win-tabs[data-tabs="' + group + '"]'); if (!bar) return;
        bar.querySelectorAll('.win-tab').forEach(function (b) {
            var on = b.getAttribute('data-tab') === panelId;
            b.classList.toggle('is-active', on);
            var p = $id(b.getAttribute('data-tab')); if (p) p.classList.toggle('is-active', on);
        });
    }
    function fullscreen() {
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            if (b.dataset.exFs) return; b.dataset.exFs = '1';
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
    }
    function modal(id, open) { var m = $id(id); if (!m) return; m.classList.toggle('is-hidden', !open); if (open) refreshCombos(); }
    /* GrdPopUp - a searchable Id / Name picker; resolves with the picked row (or null). */
    function pick(title, rows, idCol, nameCol) {
        return new Promise(function (resolve) {
            var m = $id('exmPicker');
            if (!m) {
                m = document.createElement('div');
                m.id = 'exmPicker';
                m.className = 'exm-modal is-hidden';
                m.innerHTML = '<div class="exm-modal-box" style="width:min(560px,96vw);"><div class="pd-caption pd-caption-teal exm-modal-cap"><span id="exmPickerTitle"></span>'
                    + '<button type="button" class="ex-fs-btn" id="exmPickerClose">&times;</button></div><div style="padding:6px;"><input type="text" id="exmPickerSearch" class="win-textbox" placeholder="Search..." autocomplete="off"/></div>'
                    + '<div class="ex-grid-scroll" style="max-height:52vh;"><table class="win-grid" id="exmPickerGrid"></table></div></div>';
                document.body.appendChild(m);
            }
            var done = function (r) { m.classList.add('is-hidden'); resolve(r); };
            $id('exmPickerTitle').textContent = title;
            var draw = function () {
                var q = $id('exmPickerSearch').value.toLowerCase();
                var list = rows.filter(function (r) { return !q || str(col(r, nameCol)).toLowerCase().indexOf(q) >= 0; });
                var h = '<thead><tr><th>' + esc(nameCol) + '</th></tr></thead><tbody>';
                list.forEach(function (r, i) { h += '<tr data-i="' + i + '"><td>' + esc(col(r, nameCol)) + '</td></tr>'; });
                $id('exmPickerGrid').innerHTML = h + '</tbody>';
                $id('exmPickerGrid').tBodies[0].onclick = function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) done(list[parseInt(tr.getAttribute('data-i'), 10)]); };
            };
            $id('exmPickerSearch').value = '';
            $id('exmPickerSearch').oninput = draw;
            $id('exmPickerClose').onclick = function () { done(null); };
            draw();
            m.classList.remove('is-hidden');
            $id('exmPickerSearch').focus();
        });
    }
    function shortcuts(rows) {
        box(rows.map(function (r) { return r[0] + '  -  ' + r[1]; }).join('\n'));
    }
    function cancel() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150);
    }

    global.ExM = {
        $id: $id, box: box, ask: ask, esc: esc, str: str, netI: netI, netD: netD, fixed: fixed, opt: opt, raw: raw,
        today: today, daysAgo: daysAgo, isoDate: isoDate, shortDate: shortDate, ddMMMyyyy: ddMMMyyyy, dateTime: dateTime,
        busy: busy, getJson: getJson, postJson: postJson, printPdf: printPdf, refreshCombos: refreshCombos, col: col,
        bind: bind, setVal: setVal, val: val, vint: vint, text: text, setText: setText, setEnabled: setEnabled, show: show,
        focus: focus, on: on, onLeave: onLeave, radio: radio, grid: grid, btnHtml: btnHtml, checkHtml: checkHtml,
        tabs: tabs, selectTab: selectTab, fullscreen: fullscreen, modal: modal, pick: pick, shortcuts: shortcuts, cancel: cancel
    };
}(window));
