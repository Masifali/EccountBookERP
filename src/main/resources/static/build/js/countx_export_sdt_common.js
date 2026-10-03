/* ============================================================================================
 * countx_export_sdt_common.js - helpers shared by the seven "Export document Tracking" (module 130,
 * Architecture.WinApp.SDT) pages: 619 DocDue Color Schedule, 620 Define Chart Of Document, 621 Define
 * Custom Group, 622 Client Assign To Group, 623 Document Assign To Group, 624 Shipment Doc Schedule,
 * 625 Export Document Tracking Report. Same contracts as countx_export_gd_break_up.js (881): busy()
 * button contract, getJson / postJson with the CSRF meta, bind() = DropDownBind.BindDDL /
 * BindAndRetainSelection, the Conversion.* parsers, the desktop date formats, and a grid painter with
 * Σ totals, selector check boxes, Edit / X buttons, clickable codes and editable cells.
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
    /* Conversion.ToInt / ToDouble / ToBool */
    function netI(v) { if (v === true) return 1; if (v === false) return 0; var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    function netB(v) { if (v === true || v === 1) return true; var s = str(v).trim().toLowerCase(); return s === 'true' || s === '1' || s === 'on'; }
    /* "#,##0.##" and "#,##0.###" */
    function fmt(v, dec) {
        var n = netD(v);
        if (dec === undefined) dec = 2;
        var s = n.toFixed(dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        var parts = s.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return parts.join('.');
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function today() { var d = new Date(); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function isoDate(v) {
        var s = str(v).trim();
        if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var m2 = /^(\d{1,2})\/(\d{1,2})\/(\d{4})/.exec(s);
        if (m2) return m2[3] + '-' + pad(+m2[1]) + '-' + pad(+m2[2]);
        var d = new Date(s);
        return isNaN(d.getTime()) ? '' : d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate());
    }
    /* DateTime.ToShortDateString() under the desktop's culture: M/d/yyyy */
    function shortDate(v) {
        var s = isoDate(v); if (!s) return '';
        return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4);
    }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    /* "dd-MMM-yyyy" */
    function dmy(v) {
        var s = isoDate(v); if (!s) return '';
        return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(0, 4);
    }
    /* "dd-MMM-yyyy hh:mm tt" (mode 'mmm'), "dd-MM-yyyy hh:mm tt" (mode 'mm'), "dd-MMM-yyyy hh:mm:ss" (mode 'sec') */
    function dateTime(v, mode) {
        var s = str(v).trim(); if (!s) return '';
        var d = isoDate(s); if (!d) return s;
        var t = /T(\d{2}):(\d{2}):?(\d{2})?/.exec(s) || / (\d{2}):(\d{2}):?(\d{2})?/.exec(s);
        var hh = t ? +t[1] : 0, mm = t ? t[2] : '00', ss = t && t[3] ? t[3] : '00';
        var day = d.substring(8, 10), mon = mode === 'mm' ? d.substring(5, 7) : MON[parseInt(d.substring(5, 7), 10) - 1], yr = d.substring(0, 4);
        if (mode === 'sec') return day + '-' + mon + '-' + yr + ' ' + pad(hh) + ':' + mm + ':' + ss;
        var tt = hh >= 12 ? 'PM' : 'AM', h12 = hh % 12; if (h12 === 0) h12 = 12;
        return day + '-' + mon + '-' + yr + ' ' + pad(h12) + ':' + mm + ' ' + tt;
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
    /* DropDownBind.BindDDL / BindAndRetainSelection: options from the rows, extra popup columns, the
       previous value kept when still in the list; the empty first option is "no active row". */
    function bind(id, rows, valueCol, textCol, extraCols) {
        var s = $id(id); if (!s) return;
        var keep = s.value;
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
    function selText(id) { var s = $id(id); return s && s.selectedIndex >= 0 ? s.options[s.selectedIndex].textContent.trim() : ''; }
    function findRow(rows, id, key) { key = key || 'Id'; for (var i = 0; i < rows.length; i++) if (netI(col(rows[i], key)) === netI(id)) return rows[i]; return null; }
    function setEnabled(id, on) { var e = $id(id); if (e) { e.disabled = !on; refreshCombos(); } }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function cancel() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150);
    }
    function checked(id) { var e = $id(id); return !!e && e.checked; }
    function radio(name) { var e = document.querySelector('input[name="' + name + '"]:checked'); return e ? e.value : ''; }

    /**
     * Grid painter. cols: [{ key, title?, kind: 'text'|'int'|'num'|'date'|'dmy'|'dt'|'bool'|'color', sum?, edit?:
     * 'int'|'text'|'date'|'select'|'check', options?: [{Id,name}], link?: true, hidden?: true }].
     * opts: { select: true (header selector column), del: true (X column), edit: true (Edit button column),
     *         cur: index, group: key (Janus group rows), footer: true (Σ row) }.
     * Rows carry _checked for the selector. Returns nothing; the caller wires events on the tbody.
     */
    function drawGrid(bodyId, footId, rows, cols, opts) {
        opts = opts || {};
        var body = $id(bodyId), foot = footId ? $id(footId) : null;
        var vis = cols.filter(function (c) { return !c.hidden; });
        var sums = {};
        var h = '', lastGroup = null;
        var lead = (opts.select ? 1 : 0) + (opts.del ? 1 : 0) + (opts.edit ? 1 : 0);
        rows.forEach(function (r, i) {
            if (opts.group) {
                var g = str(col(r, opts.group));
                if (g !== lastGroup) {
                    h += '<tr class="win-group-row"><td colspan="' + (vis.length + lead) + '">' + esc((opts.groupTitle || opts.group) + ': ' + g) + '</td></tr>';
                    lastGroup = g;
                }
            }
            h += '<tr data-i="' + i + '"' + (i === opts.cur ? ' class="is-current"' : '') + '>';
            if (opts.select) h += '<td class="ctr win-cell-btn"><input type="checkbox" class="win-sel" data-sel="' + i + '"' + (r._checked ? ' checked' : '') + '/></td>';
            if (opts.del) h += '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>';
            if (opts.edit) h += '<td class="win-cell-btn"><button type="button" class="win-edit" data-edit="' + i + '">Edit</button></td>';
            vis.forEach(function (c) {
                var v = col(r, c.key), cell, cls = '';
                if (c.sum) sums[c.key] = (sums[c.key] || 0) + netD(v);
                if (c.edit && opts.editable !== false) {
                    if (c.edit === 'int') cell = '<input type="text" class="num" data-guard="integer" data-i="' + i + '" data-c="' + c.key + '" value="' + esc(str(v)) + '"/>';
                    else if (c.edit === 'date') cell = '<input type="date" data-i="' + i + '" data-c="' + c.key + '" value="' + esc(isoDate(v)) + '"/>';
                    else if (c.edit === 'check') cell = '<input type="checkbox" data-i="' + i + '" data-c="' + c.key + '"' + (netB(v) ? ' checked' : '') + '/>';
                    else if (c.edit === 'select') {
                        cell = '<select data-i="' + i + '" data-c="' + c.key + '"><option value="0"></option>';
                        (c.options || []).forEach(function (o) { cell += '<option value="' + esc(o.Id) + '"' + (netI(o.Id) === netI(v) ? ' selected' : '') + '>' + esc(o.name !== undefined ? o.name : o.type) + '</option>'; });
                        cell += '</select>';
                    } else cell = '<input type="text" data-i="' + i + '" data-c="' + c.key + '" value="' + esc(str(v)) + '"' + (c.width ? ' style="min-width:' + c.width + 'px;"' : '') + '/>';
                    h += '<td class="win-editable' + (c.kind === 'int' || c.kind === 'num' ? ' num' : '') + '">' + cell + '</td>';
                    return;
                }
                if (c.kind === 'int') { cell = str(netI(v)); cls = 'num'; }
                else if (c.kind === 'num') { cell = fmt(v, c.dec === undefined ? 2 : c.dec); cls = 'num'; }
                else if (c.kind === 'date') cell = shortDate(v);
                else if (c.kind === 'dmy') cell = dmy(v);
                else if (c.kind === 'dt') cell = dateTime(v, c.mode || 'mmm');
                else if (c.kind === 'bool') { cell = '<input type="checkbox" disabled' + (netB(v) ? ' checked' : '') + '/>'; cls = 'ctr'; }
                else if (c.kind === 'color') { var hx = str(col(r, c.hex || 'ColorHex')) || str(v); cell = '<span class="win-swatch" style="background:' + esc(hx) + '"></span>' + esc(str(v)); }
                else cell = esc(str(v));
                if (c.kind !== 'bool' && c.kind !== 'color') cell = esc(cell);
                if (c.link && cell !== '') cell = '<a class="win-code" data-i="' + i + '" href="javascript:void(0)">' + cell + '</a>';
                if (c.style) { var st = c.style(r); if (st) cell = '<span style="' + esc(st) + '">' + cell + '</span>'; }
                h += '<td' + (cls ? ' class="' + cls + '"' : '') + '>' + cell + '</td>';
            });
            h += '</tr>';
        });
        body.innerHTML = h;
        if (foot) {
            var any = vis.some(function (c) { return c.sum; });
            if (!rows.length || !any) { foot.innerHTML = ''; return; }
            var t = '<tr>' + (lead ? '<td class="lbl" colspan="' + lead + '">&Sigma;</td>' : '');
            vis.forEach(function (c, idx) {
                var cell = c.sum ? esc(c.kind === 'int' ? fmt(sums[c.key] || 0, 0) : fmt(sums[c.key] || 0, c.dec === undefined ? 2 : c.dec)) : '';
                if (idx === 0 && !lead) cell = '&Sigma;';
                t += '<td' + (c.sum ? '' : ' class="lbl"') + '>' + cell + '</td>';
            });
            foot.innerHTML = t + '</tr>';
        }
    }

    /** click / dblclick / change wiring for one grid body. handlers: sel(i, on), del(i), edit(i), open(i), select(i), cell(i, key, value, input) */
    function wireGrid(bodyId, handlers) {
        var gb = $id(bodyId); if (!gb) return;
        gb.addEventListener('click', function (e) {
            var del = e.target.closest('button[data-del]');
            if (del) { if (handlers.del) handlers.del(+del.getAttribute('data-del')); return; }
            var ed = e.target.closest('button[data-edit]');
            if (ed) { if (handlers.edit) handlers.edit(+ed.getAttribute('data-edit')); return; }
            var lk = e.target.closest('a.win-code');
            if (lk) { if (handlers.open) handlers.open(+lk.getAttribute('data-i')); return; }
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            if (handlers.select) handlers.select(+tr.getAttribute('data-i'));
            gb.querySelectorAll('tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
        });
        gb.addEventListener('dblclick', function (e) {
            if (e.target.closest('input,select,button,a')) return;
            var tr = e.target.closest('tr[data-i]'); if (tr && handlers.open) handlers.open(+tr.getAttribute('data-i'));
        });
        gb.addEventListener('change', function (e) {
            var s = e.target.closest('input.win-sel');
            if (s) { if (handlers.sel) handlers.sel(+s.getAttribute('data-sel'), s.checked); return; }
            var c = e.target.closest('[data-c]');
            if (c && handlers.cell) handlers.cell(+c.getAttribute('data-i'), c.getAttribute('data-c'), c.type === 'checkbox' ? c.checked : c.value, c);
        });
    }
    /** UseHeaderSelector: a header check box that checks every row. */
    function wireHeaderSelector(thId, rowsFn, onChange) {
        var th = $id(thId); if (!th) return;
        var cb = th.querySelector('input[type=checkbox]'); if (!cb) return;
        cb.addEventListener('change', function () { rowsFn().forEach(function (r) { r._checked = cb.checked; }); onChange(); });
    }
    function tabs(group, onChange) {
        document.querySelectorAll('.win-tabs[data-tabs="' + group + '"] .win-tab').forEach(function (b) {
            b.addEventListener('click', function () { selectTab(group, b.getAttribute('data-tab'), onChange); });
        });
    }
    function selectTab(group, panelId, onChange) {
        var t = document.querySelector('.win-tabs[data-tabs="' + group + '"]'); if (!t) return;
        t.querySelectorAll('.win-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === panelId); });
        Array.prototype.forEach.call(t.parentNode.children, function (p) { if (p.classList.contains('win-tab-panel')) p.classList.toggle('is-active', p.id === panelId); });
        if (onChange) onChange(panelId);
    }
    function activeTab(group) {
        var b = document.querySelector('.win-tabs[data-tabs="' + group + '"] .win-tab.is-active');
        return b ? b.getAttribute('data-tab') : '';
    }
    function wireFullscreen() {
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
    }
    /* KeyData == Return -> SendKeys.Send("{TAB}") */
    function enterMovesOn(e) {
        if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') {
            var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled), textarea'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(e.target);
            if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
            return true;
        }
        return false;
    }
    /* ShortCutKeyPopUp - the desktop's fixed table, shown as a message. */
    function shortcutKeys(rows) {
        box(rows.map(function (r) { return r[0] + '  -  ' + r[1]; }).join('\n'));
    }
    function copyRow(r) { var c = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) c[k] = r[k]; return c; }

    global.ExportSdt = {
        $id: $id, box: box, ask: ask, esc: esc, str: str, netI: netI, netD: netD, netB: netB, fmt: fmt, today: today, daysAgo: daysAgo,
        isoDate: isoDate, shortDate: shortDate, dmy: dmy, dateTime: dateTime, busy: busy, getJson: getJson, postJson: postJson,
        refreshCombos: refreshCombos, show: show, col: col, bind: bind, setVal: setVal, val: val, setText: setText, hasSel: hasSel,
        selText: selText, findRow: findRow, setEnabled: setEnabled, focus: focus, cancel: cancel, checked: checked, radio: radio,
        drawGrid: drawGrid, wireGrid: wireGrid, wireHeaderSelector: wireHeaderSelector, tabs: tabs, selectTab: selectTab,
        activeTab: activeTab, wireFullscreen: wireFullscreen, enterMovesOn: enterMovesOn, shortcutKeys: shortcutKeys, copyRow: copyRow
    };
}(window));
