/* ============================================================================================
 * countx_export_bankgd_common.js - helpers shared by the five "Bank Export & GD Management"
 * pages (951 Custom / Bank Invoice, 952 GD / Bank Invoice & GD Mapping, 953 Fcy Receipts
 * Utilization, 197 Gd Bank Request, 198 GD Break Up Manual). Same contract as the 881 / 882
 * pages: busy() on every button (disabled + spinner, duplicates ignored, re-enabled on success
 * and failure), fetch with the CSRF meta pair, searchable combos through countx_prod_combo.js
 * (bind()/setVal() rebuild the <select> and call DesktopCombo.refresh()), grids drawn from rows
 * with a Sigma footer for the summed columns, Form | History tabs with the footer History button.
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
    /* Conversion.ToInt / ToDouble - text with thousands separators parses, anything else is 0. */
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    /* ToString("#,##0.###") family. */
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
    function fmt4(v) { return fmt(v, 4); }
    /* "#,#.###" - zero prints as "" on the desktop; kept as "0" is NOT what .NET does, so return "". */
    function fmtHash(v, dec) { var n = netD(v); return n === 0 ? '' : fmt(n, dec === undefined ? 3 : dec); }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
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
    /* "dd-MMM-yyyy". */
    function ddMMMyyyy(v) { var s = isoDate(v); if (!s) return ''; return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(0, 4); }
    /* "dd-MM-yyyy hh:mm tt" from an ISO date-time. */
    function ddMMyyyyHm(v) {
        var s = str(v).trim(); if (!s) return '';
        var d = new Date(s.length === 10 ? s + 'T00:00:00' : s);
        if (isNaN(d.getTime())) return s;
        var h = d.getHours(), tt = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return pad(d.getDate()) + '-' + pad(d.getMonth() + 1) + '-' + d.getFullYear() + ' ' + pad(h) + ':' + pad(d.getMinutes()) + ' ' + tt;
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
    function postJson(url, data) {
        var h = { 'Accept': 'application/json', 'Content-Type': 'application/json' };
        var t = document.querySelector('meta[name="_csrf"]'), n = document.querySelector('meta[name="_csrf_header"]');
        if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
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
    /**
     * DropDownBind.BindDDL / InfragisticsHelper.BindAndRetainSelection: options from the rows, extra
     * columns in the popup, the previous value kept when it is still in the list. The first empty
     * option is the combo without an active row (value "0").
     */
    function bind(id, rows, valueCol, textCol, extraCols, keepValue) {
        var s = $id(id); if (!s) return;
        var keep = keepValue === undefined ? s.value : str(keepValue);
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) {
            var extra = (extraCols || []).map(function (k) { return str(col(r, k)).replace(/\|/g, '/'); }).join('|');
            h += '<option value="' + esc(col(r, valueCol)) + '" data-extra="' + esc(extra) + '">' + esc(col(r, textCol)) + '</option>';
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
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function hasSel(id) { var s = $id(id); return !!s && s.value !== '0' && s.value !== ''; }
    function selText(id) { var s = $id(id); return s && s.selectedIndex > 0 ? s.options[s.selectedIndex].textContent.trim() : ''; }
    function findRow(rows, id, idCol) { idCol = idCol || 'Id'; for (var i = 0; i < (rows || []).length; i++) if (netI(col(rows[i], idCol)) === netI(id)) return rows[i]; return null; }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function cancel() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150);
    }
    function copy(r) { var c = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) c[k] = r[k]; return c; }
    function sum(rows, name, pred) { var t = 0; (rows || []).forEach(function (r, i) { if (!pred || pred(r, i)) t += netD(col(r, name)); }); return t; }

    /**
     * drawGrid(bodyId, footId, rows, cols, opts)
     *   cols: [{ key, fmt(v,row) -> string, num:true, sum:true, link:true, cls }]  (or a plain key string)
     *   opts: { cur, leading(r,i) -> td html, empty: elementId }
     */
    function drawGrid(bodyId, footId, rows, cols, opts) {
        opts = opts || {};
        var body = $id(bodyId), foot = footId ? $id(footId) : null;
        if (!body) return;
        var specs = cols.map(function (c) { return typeof c === 'string' ? { key: c } : c; });
        var sums = {};
        var lead = opts.leading;
        var hasLead = !!lead && rows.length > 0 && lead(rows[0], 0) !== '';
        body.innerHTML = rows.map(function (r, i) {
            var h = '<tr data-i="' + i + '"' + (i === opts.cur ? ' class="is-current"' : '') + '>' + (lead ? lead(r, i) : '');
            specs.forEach(function (c) {
                var raw = col(r, c.key);
                var v = c.fmt ? c.fmt(raw, r) : str(raw);
                if (c.sum) sums[c.key] = (sums[c.key] || 0) + netD(raw);
                var inner = esc(v);
                if (c.link && v !== '') inner = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + esc(v) + '</a>';
                if (c.html) inner = c.html(raw, r, i);
                h += '<td' + (c.num ? ' class="num' + (c.cls ? ' ' + c.cls : '') + '"' : (c.cls ? ' class="' + c.cls + '"' : '')) + '>' + inner + '</td>';
            });
            return h + '</tr>';
        }).join('');
        if (foot) {
            if (!rows.length) { foot.innerHTML = ''; }
            else {
                var t = '<tr>' + (hasLead ? '<td class="lbl">&Sigma;</td>' : '');
                specs.forEach(function (c, idx) {
                    var v = c.sum ? (c.fmt ? c.fmt(sums[c.key] || 0, null) : fmt(sums[c.key] || 0)) : '';
                    var cell = esc(v);
                    if (idx === 0 && !hasLead && !c.sum) cell = '&Sigma;';
                    t += '<td' + (c.num ? '' : ' class="lbl"') + '>' + cell + '</td>';
                });
                foot.innerHTML = t + '</tr>';
            }
        }
        if (opts.empty) show(opts.empty, rows.length === 0);
    }
    /* click = select row / X button / Edit button / code link / any data-act button; dblclick = open. */
    function wireGrid(bodyId, handlers) {
        var gb = $id(bodyId); if (!gb) return;
        gb.addEventListener('click', function (e) {
            var act = e.target.closest('button[data-act]');
            if (act) { if (handlers.act) handlers.act(act.getAttribute('data-act'), +act.getAttribute('data-i'), act); return; }
            var del = e.target.closest('button[data-del]');
            if (del) { if (handlers.del) handlers.del(+del.getAttribute('data-del')); return; }
            var ed = e.target.closest('button[data-edit]');
            if (ed) { if (handlers.open) handlers.open(+ed.getAttribute('data-edit')); return; }
            var lk = e.target.closest('a.win-code');
            if (lk) { if (handlers.open) handlers.open(+lk.getAttribute('data-i')); return; }
            if (e.target.closest('input,select,textarea')) return;
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            if (handlers.select) handlers.select(+tr.getAttribute('data-i'));
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) {
            if (e.target.closest('input,select,textarea,button,a')) return;
            var tr = e.target.closest('tr[data-i]'); if (tr && handlers.open) handlers.open(+tr.getAttribute('data-i'));
        });
        if (handlers.input) {
            gb.addEventListener('change', function (e) {
                var inp = e.target.closest('[data-cell]'); if (!inp) return;
                var tr = e.target.closest('tr[data-i]'); if (!tr) return;
                handlers.input(+tr.getAttribute('data-i'), inp.getAttribute('data-cell'), inp.value, inp);
            });
        }
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    /* Form | History tabs (.win-tabs[data-tabs=group] + .win-tab-panel siblings) with the footer button. */
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
    function wireTabs(handler) {
        document.querySelectorAll('.win-tabs .win-tab').forEach(function (b) {
            b.addEventListener('click', function () { handler(b.closest('.win-tabs').getAttribute('data-tabs'), b.getAttribute('data-tab')); });
        });
        document.querySelectorAll('.ex-sub-tab').forEach(function (b) {
            b.addEventListener('click', function () { subTab(b.closest('.ex-sub-tabs').getAttribute('data-subtabs'), b.getAttribute('data-sub')); });
        });
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
    }
    /* Inner detail tab strips (tabControl2 of 951, tabControl2 of 953): .ex-sub-tabs[data-subtabs] + .ex-sub-panel[data-sub]. */
    function subTab(group, name) {
        var strip = document.querySelector('.ex-sub-tabs[data-subtabs="' + group + '"]'); if (!strip) return;
        strip.querySelectorAll('.ex-sub-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-sub') === name); });
        document.querySelectorAll('.ex-sub-panel[data-subgroup="' + group + '"]').forEach(function (p) { p.classList.toggle('is-active', p.getAttribute('data-sub') === name); });
    }
    function activeSub(group) { var b = document.querySelector('.ex-sub-tabs[data-subtabs="' + group + '"] .ex-sub-tab.is-active'); return b ? b.getAttribute('data-sub') : ''; }
    /* KeyData == Keys.Return -> SendKeys "{TAB}". */
    function enterMovesOn(e) {
        if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox' && !e.target.closest('.win-grid')) {
            var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(e.target);
            if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
            return true;
        }
        return false;
    }
    /* The ShortCutKeyPopUp form: a fixed list shown in a small dialog. */
    function shortcutPopup(rows) {
        var old = $id('exShortcutPopup'); if (old) old.parentNode.removeChild(old);
        var d = document.createElement('div');
        d.id = 'exShortcutPopup'; d.className = 'ex-shortcut-popup';
        d.innerHTML = '<div class="ex-shortcut-box"><div class="pd-caption pd-caption-teal">ShortCut Keys <button type="button" class="ex-fs-btn" style="float:right" onclick="document.getElementById(\'exShortcutPopup\').remove()">X</button></div>' +
            '<table class="win-grid"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>' +
            rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('') + '</tbody></table></div>';
        document.body.appendChild(d);
    }
    /* CrystalPrint.open through ReportRegistry; message when the print helper is not on the page. */
    function print(key, args, btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return Promise.resolve(); }
        return global.CrystalPrint.open(key, args, btn);
    }

    global.ExBG = {
        $id: $id, box: box, ask: ask, esc: esc, str: str, netI: netI, netD: netD, fmt: fmt, fmt2: fmt2, fmt4: fmt4, fmtHash: fmtHash,
        pad: pad, today: today, daysAgo: daysAgo, isoDate: isoDate, shortDate: shortDate, ddMMMyyyy: ddMMMyyyy, ddMMyyyyHm: ddMMyyyyHm,
        busy: busy, getJson: getJson, postJson: postJson, refreshCombos: refreshCombos, show: show, col: col, bind: bind, setVal: setVal,
        val: val, setText: setText, hasSel: hasSel, selText: selText, findRow: findRow, setEnabled: setEnabled, focus: focus, cancel: cancel,
        copy: copy, sum: sum, drawGrid: drawGrid, wireGrid: wireGrid, on: on, innerTab: innerTab, activeInner: activeInner, wireTabs: wireTabs,
        subTab: subTab, activeSub: activeSub, enterMovesOn: enterMovesOn, shortcutPopup: shortcutPopup, print: print
    };
}(window));
