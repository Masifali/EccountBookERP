/* ============================================================================================
 * countx_export_f_common.js - helpers shared by the Export batch-F pages (206, 213, 214, 205, 188, 907, 915):
 * the same busy / getJson / postJson / bind / setVal / fmt / shortDate / drawGrid helpers as the 881/882
 * pages (countx_export_gd_break_up.js), exposed once as window.ExportF so each page script stays focused
 * on its desktop form's own events. Loaded before the page script.
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
    /* ToString("#,##0.###") */
    function fmt(v, dec) {
        var n = netD(v);
        if (dec === undefined) dec = 3;
        var s = n.toFixed(dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        var parts = s.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    function fmt2(v) { return fmt(v, 2); }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function addDays(iso, n) {
        var s = isoDate(iso); if (!s) return '';
        var d = new Date(s + 'T00:00:00'); d.setDate(d.getDate() + n);
        return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
    }
    function dayDiff(a, b) {
        var x = isoDate(a), y = isoDate(b); if (!x || !y) return 0;
        return Math.round((new Date(x + 'T00:00:00') - new Date(y + 'T00:00:00')) / 86400000);
    }
    function isoDate(v) {
        var s = str(v).trim();
        if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        return isNaN(d.getTime()) ? '' : d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
    }
    /* DateTime.ToShortDateString() under the desktop's culture: M/d/yyyy. */
    function shortDate(v) {
        var s = isoDate(v); if (!s) return '';
        return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4);
    }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    /* "dd-MMM-yyyy hh:mm tt" / "dd-MM-yyyy hh:mm tt" grid formats. */
    function dateTime(v, mmm) {
        var s = str(v).trim(); if (!s) return '';
        var d = new Date(s.length === 10 ? s + 'T00:00:00' : s);
        if (isNaN(d.getTime())) return s;
        var h = d.getHours(), tt = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        var mon = mmm === false ? pad(d.getMonth() + 1) : MON[d.getMonth()];
        return pad(d.getDate()) + '-' + mon + '-' + d.getFullYear() + ' ' + pad(h) + ':' + pad(d.getMinutes()) + ' ' + tt;
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
        var h = csrf({ 'Accept': 'application/json', 'Content-Type': 'application/json' });
        return fetch(url, { method: 'POST', headers: h, credentials: 'same-origin', body: JSON.stringify(data) }).then(parse);
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
    /* DropDownBind.BindDDL / InfragisticsHelper.BindAndRetainSelection: options + popup columns, previous value kept. */
    function bind(id, rows, valueCol, textCol, extraCols, noBlank) {
        var s = $id(id); if (!s) return;
        var keep = s.value;
        var h = noBlank ? '' : '<option value="0"></option>';
        (rows || []).forEach(function (r) {
            var extra = (extraCols || []).map(function (k) { return str(col(r, k)).replace(/\|/g, '/'); }).join('|');
            h += '<option value="' + esc(col(r, valueCol)) + '" data-extra="' + esc(extra) + '">' + esc(col(r, textCol)) + '</option>';
        });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.value = noBlank && s.options.length ? s.options[0].value : '0';
        refreshCombos();
    }
    function setVal(id, v) {
        var s = $id(id); if (!s) return;
        s.value = str(v);
        if (s.value !== str(v)) s.value = '0';
        refreshCombos();
    }
    /* UltraCombo.Text = "" / Value = id when the id is not in the list - option added so the text still shows. */
    function setValOrAdd(id, v, text) {
        var s = $id(id); if (!s) return;
        if (netI(v) > 0 && !Array.prototype.some.call(s.options, function (o) { return o.value === str(netI(v)); })) {
            var o = document.createElement('option'); o.value = str(netI(v)); o.textContent = str(text); s.appendChild(o);
        }
        setVal(id, netI(v) > 0 ? netI(v) : '0');
    }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function hasSel(id) { var s = $id(id); return !!s && s.value !== '0' && s.value !== ''; }
    function selText(id) { var s = $id(id); return s && s.selectedIndex >= 0 ? s.options[s.selectedIndex].textContent.trim() : ''; }
    function findRow(rows, id) { for (var i = 0; i < rows.length; i++) if (netI(col(rows[i], 'Id')) === netI(id)) return rows[i]; return null; }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function checked(id) { var e = $id(id); return !!e && !!e.checked; }
    function cancel() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150);
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    /* Inner Form | History tabs (.win-tabs[data-tabs] + sibling .win-tab-panel), footer History button text. */
    function innerTab(group, panelId, footBtnId, onChange) {
        var tabs = document.querySelector('.win-tabs[data-tabs="' + group + '"]');
        if (!tabs) return;
        tabs.querySelectorAll('.win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        var host = tabs.parentNode;
        Array.prototype.forEach.call(host.children, function (p) {
            if (p.classList.contains('win-tab-panel')) p.classList.toggle('is-active', p.id === panelId);
        });
        var footBtn = footBtnId ? $id(footBtnId) : null;
        var onHist = /History$/.test(panelId);
        if (footBtn) {
            footBtn.querySelector('span').textContent = onHist ? 'Form' : 'History';
            footBtn.querySelector('i').className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        }
        if (onChange) onChange(panelId, onHist);
    }
    function activeInner(group) {
        var t = document.querySelector('.win-tabs[data-tabs="' + group + '"] .win-tab.is-active');
        return t ? t.getAttribute('data-tab') : '';
    }
    function wireTabs(onChange, footBtnId) {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) {
            b.addEventListener('click', function () { innerTab(b.closest('.win-tabs').getAttribute('data-tabs'), b.getAttribute('data-tab'), footBtnId, onChange); });
        });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
    }

    /* Grid rows: cols = [{key, fmt:'int'|'num'|'rate'|'date'|'datetime'|'text'|'bool', sum:bool, link:bool, cls:fn}] */
    function cellText(c, v) {
        switch (c.fmt) {
            case 'int': return str(netI(v));
            case 'num': return fmt2(v);
            case 'num3': return fmt(v);
            case 'rate': return fmt(v, 4);
            case 'date': return shortDate(v);
            case 'datetime': return dateTime(v, c.mmm !== false);
            case 'bool': return (v === true || str(v).toLowerCase() === 'true' || str(v) === '1') ? 'True' : 'False';
            default: return str(v);
        }
    }
    function isNum(c) { return /^(int|num|num3|rate)$/.test(c.fmt || ''); }
    function drawGrid(bodyId, footId, rows, cols, cur, leading, opts) {
        var body = $id(bodyId), foot = footId ? $id(footId) : null;
        opts = opts || {};
        var sums = {};
        body.innerHTML = rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === cur ? ' class="is-current"' : '') + '>' + (leading ? leading(r, i) : '');
            cols.forEach(function (c) {
                var v = col(r, c.key);
                if (c.sum) sums[c.key] = (sums[c.key] || 0) + netD(v);
                var inner;
                if (c.edit === 'text') inner = '<input class="exf-cell" data-i="' + i + '" data-k="' + c.key + '" value="' + esc(v) + '"/>';
                else if (c.edit === 'select') inner = '<select class="exf-cell" data-i="' + i + '" data-k="' + c.key + '">' + (c.options ? c.options(v) : '') + '</select>';
                else if (c.edit === 'check') inner = '<input type="checkbox" class="exf-cell-chk" data-i="' + i + '" data-k="' + c.key + '"' + ((v === true || str(v).toLowerCase() === 'true') ? ' checked' : '') + '/>';
                else {
                    var t = cellText(c, v);
                    inner = esc(t);
                    if (c.link && t !== '') inner = '<a class="win-code" data-i="' + i + '" data-k="' + c.key + '" href="javascript:void(0)">' + esc(t) + '</a>';
                }
                var cls = (isNum(c) ? 'num' : '') + (c.cls ? ' ' + c.cls(r) : '') + (c.link ? ' exf-link' : '');
                h += '<td' + (cls.trim() ? ' class="' + cls.trim() + '"' : '') + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        if (foot) {
            if (!rows.length || !cols.some(function (c) { return c.sum; })) { foot.innerHTML = ''; return; }
            var t = '<tr>' + (leading ? '<td class="lbl" colspan="' + (opts.leadCols || 1) + '">&Sigma;</td>' : '');
            cols.forEach(function (c, idx) {
                var v = c.sum ? cellText(c, sums[c.key] || 0) : '';
                var cell = esc(v);
                if (idx === 0 && !leading) cell = '&Sigma;';
                t += '<td' + (isNum(c) ? '' : ' class="lbl"') + '>' + cell + '</td>';
            });
            foot.innerHTML = t + '</tr>';
        }
    }
    /* Row click / dblclick / button / link dispatcher. handlers: {select(i), open(i), del(i), btn(name, i), link(key, i)} */
    function wireGrid(bodyId, handlers) {
        var gb = $id(bodyId); if (!gb) return;
        gb.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]');
            if (tr) {
                var i = +tr.getAttribute('data-i');
                if (handlers.select) handlers.select(i);
                gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            }
            var del = e.target.closest('button[data-del]');
            if (del && handlers.del) { handlers.del(+del.getAttribute('data-del')); return; }
            var bt = e.target.closest('button[data-btn]');
            if (bt && handlers.btn) { handlers.btn(bt.getAttribute('data-btn'), +bt.getAttribute('data-i')); return; }
            var lk = e.target.closest('a.win-code');
            if (lk) { if (handlers.link) handlers.link(lk.getAttribute('data-k'), +lk.getAttribute('data-i')); else if (handlers.open) handlers.open(+lk.getAttribute('data-i')); }
        });
        gb.addEventListener('dblclick', function (e) {
            if (e.target.closest('input,select,button,a')) return;
            var tr = e.target.closest('tr[data-i]'); if (tr && handlers.open) handlers.open(+tr.getAttribute('data-i'));
        });
        gb.addEventListener('change', function (e) {
            var c = e.target.closest('.exf-cell, .exf-cell-chk'); if (!c || !handlers.cell) return;
            handlers.cell(+c.getAttribute('data-i'), c.getAttribute('data-k'), c.type === 'checkbox' ? c.checked : c.value);
        });
    }
    /* ShortCutKeyPopUp(dt) - KeyCombination / Description rows in a small popup. */
    function shortcutPopup(rows) {
        var old = document.querySelector('.exf-shortcut-pop'); if (old) old.parentNode.removeChild(old);
        var d = document.createElement('div'); d.className = 'exf-shortcut-pop';
        d.innerHTML = '<b>ShortCut Keys</b><table>' + rows.map(function (r) { return '<tr><td><b>' + esc(r[0]) + '</b></td><td>' + esc(r[1]) + '</td></tr>'; }).join('') +
            '</table><button type="button" class="win-btn-small">Close</button>';
        d.querySelector('button').addEventListener('click', function () { d.parentNode.removeChild(d); });
        document.body.appendChild(d);
    }
    /* Enter moves focus to the next control (SendKeys.Send("{TAB}")). */
    function enterMoves(e) {
        if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
            var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(e.target);
            if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
            return true;
        }
        return false;
    }
    /* Prints: a seeded contract (/api/print/{key}/pdf?arg=..) or a hand-traced one (/api/reports/{key}/print.pdf). */
    function printSeeded(key, args) {
        var q = Object.keys(args || {}).filter(function (k) { return args[k] !== null && args[k] !== undefined && args[k] !== ''; })
            .map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(args[k]); }).join('&');
        global.open('/api/print/' + encodeURIComponent(key) + '/pdf' + (q ? '?' + q : ''), '_blank');
    }

    global.ExportF = {
        $id: $id, box: box, ask: ask, esc: esc, str: str, netI: netI, netD: netD, fmt: fmt, fmt2: fmt2, pad: pad, today: today, daysAgo: daysAgo,
        addDays: addDays, dayDiff: dayDiff, isoDate: isoDate, shortDate: shortDate, dateTime: dateTime, busy: busy, getJson: getJson, postJson: postJson,
        refreshCombos: refreshCombos, show: show, col: col, bind: bind, setVal: setVal, setValOrAdd: setValOrAdd, val: val, setText: setText, hasSel: hasSel,
        selText: selText, findRow: findRow, setEnabled: setEnabled, focus: focus, checked: checked, cancel: cancel, on: on, innerTab: innerTab,
        activeInner: activeInner, wireTabs: wireTabs, drawGrid: drawGrid, wireGrid: wireGrid, shortcutPopup: shortcutPopup, enterMoves: enterMoves,
        printSeeded: printSeeded, cellText: cellText
    };
}(window));
