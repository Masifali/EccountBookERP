/* Batch inv4a (Inventory hub — pricing / discount schedules) — helpers the five pages share.
   Relies on window.StoreCommon (countx_store_common.js). Nothing here knows a screen. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc;

    /* ---------------------------------------------------------------- .NET format strings */
    function roundAway(x, d) { var f = Math.pow(10, d), s = x < 0 ? -1 : 1; return s * Math.round(Math.abs(x) * f + 1e-9) / f; }
    /** "#,#" → integer with separators (0 → ""); "#,#.##" / "#,#.#" → up to n decimals (0 → ""); "#,##0.##" → 0 shown. */
    function fmt(v, pattern) {
        var n = Number(v);
        if (v === null || v === undefined || v === '' || isNaN(n)) return '';
        var m = /^#,#(0?)(?:\.(#+))?$/.exec(pattern || '#,#');
        var zero = m && m[1] === '0', dec = m && m[2] ? m[2].length : 0;
        var r = roundAway(n, dec);
        if (r === 0 && !zero) return '';
        return r.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dec });
    }
    function num(v) { var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim(); if (!s) return 0; var n = Number(s); return isNaN(n) ? 0 : n; }
    /** Conversion.ToInt — truncation of the parsed double. */
    function toInt(v) { return Math.trunc(num(v)); }

    /* ---------------------------------------------------------------- dates */
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function parts(iso) { var s = String(iso || ''); var m = /^(\d{4})-(\d{2})-(\d{2})(?:[ T](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(s); return m; }
    function ddMMMyyyy(iso) { var m = parts(iso); return m ? m[3] + '-' + MON[+m[2] - 1] + '-' + m[1] : String(iso || ''); }
    function ddMMMyy(iso) { var m = parts(iso); return m ? m[3] + '-' + MON[+m[2] - 1] + '-' + m[1].slice(2) : String(iso || ''); }
    /** "dd-MMM-yy hh:mm tt" */
    function ddMMMyyTime(iso) {
        var m = parts(iso); if (!m) return String(iso || '');
        var h = m[4] === undefined ? 0 : +m[4], mi = m[5] === undefined ? 0 : +m[5], tt = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return ddMMMyy(iso) + ' ' + String(h).padStart(2, '0') + ':' + String(mi).padStart(2, '0') + ' ' + tt;
    }
    function nowTime() { var d = new Date(); return String(d.getHours()).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ':' + String(d.getSeconds()).padStart(2, '0'); }

    /* ---------------------------------------------------------------- combos */
    /** BindDDL(…, ZeroIndex:false): no default row; the combo text is empty until something is chosen. */
    function bindDdl(id, list, idKey, nameKey, keepValue) {
        var el = $id(id), prev = el.value;
        el.innerHTML = '<option value=""></option>';
        (list || []).forEach(function (r) { el.appendChild(new Option(String(r[nameKey || 'Name']), String(r[idKey || 'Id']))); });
        el.value = keepValue && (list || []).some(function (r) { return String(r[idKey || 'Id']) === prev; }) ? prev : '';
    }
    function selText(id) { var o = $id(id).selectedOptions[0]; return o && o.value !== '' ? o.textContent : ''; }
    function selVal(id) { return toInt($id(id).value); }
    function hasActiveRow(id) { return $id(id).value !== ''; }

    /* fx-multi checked-list combo (Infragistics CheckedListSettings) */
    function multi(id, list, textId) {
        var host = $id(id), listEl = $id(id + 'List'), txt = $id(textId || (id + 'Text'));
        listEl.innerHTML = (list || []).map(function (r) { return '<label><input type="checkbox" data-id="' + esc(r.Id) + '"> ' + esc(r.Name) + '</label>'; }).join('');
        txt.textContent = '';
        if (!host.getAttribute('data-wired')) {
            host.setAttribute('data-wired', '1');
            host.querySelector('.fx-multi-btn').addEventListener('click', function () { listEl.style.display = listEl.style.display === 'block' ? 'none' : 'block'; });
            listEl.addEventListener('change', function () { txt.textContent = multiChecked(id).map(function (c) { return c.name; }).join(','); });
            document.addEventListener('mousedown', function (e) { if (!e.target.closest('#' + id)) listEl.style.display = 'none'; });
        }
    }
    function multiChecked(id) {
        var out = [];
        $id(id + 'List').querySelectorAll('input[data-id]:checked').forEach(function (i) { out.push({ id: i.getAttribute('data-id'), name: i.parentNode.textContent.trim() }); });
        return out;
    }
    function multiClear(id) { $id(id + 'List').querySelectorAll('input').forEach(function (i) { i.checked = false; }); $id(id + 'Text').textContent = ''; }

    /* ---------------------------------------------------------------- grids
       cols: [{key, cap, fmt, cls, hidden, edit:'num'|'text'|{options}, btn, chk, width}]
       opts: {rows, group:'key', groupTotals:true, totals:[keys], filter:true, sel, onCell, redIf(r)} */
    function renderGrid(tableId, cols, opts) {
        var t = $id(tableId), rows = opts.rows || [], vis = cols.filter(function (c) { return !c.hidden && c.key !== opts.group; });
        var state = t.__fx || (t.__fx = { filters: {} });
        if (!rows.length && !opts.keepHeader) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
        var h = '<tr>' + vis.map(function (c) { return '<th data-col="' + c.key + '"' + (c.width ? ' style="min-width:' + c.width + 'px"' : '') + '>' + (c.chk === 'all' ? '<input type="checkbox" data-chkall="1">' : esc(c.cap === undefined ? c.key : c.cap)) + '</th>'; }).join('') + '</tr>';
        if (opts.filter) h += '<tr class="flt">' + vis.map(function (c) { return '<td>' + (c.btn || c.chk ? '' : '<input type="text" data-flt="' + c.key + '" value="' + esc(state.filters[c.key] || '') + '">') + '</td>'; }).join('') + '</tr>';
        t.tHead.innerHTML = h;
        function cellText(r, c) { if (c.date) return c.date(r[c.key]); return c.fmt ? fmt(r[c.key], c.fmt) : String(r[c.key] === null || r[c.key] === undefined ? '' : r[c.key]); }
        var keys = Object.keys(state.filters).filter(function (k) { return state.filters[k]; });
        var idx = []; rows.forEach(function (r, i) { if (keys.every(function (k) { var c = cols.filter(function (x) { return x.key === k; })[0]; return c && cellText(r, c).toLowerCase().indexOf(state.filters[k]) >= 0; })) idx.push(i); });
        var html = '', tot = {}, g = {}, prev = null, sums = opts.totals || [];
        function totRow(obj, cls) { return '<tr class="' + cls + '">' + vis.map(function (c) { return sums.indexOf(c.key) >= 0 ? '<td class="n">' + esc(fmt(obj[c.key] || 0, c.tfmt || c.fmt || '#,#')) + '</td>' : '<td></td>'; }).join('') + '</tr>'; }
        idx.forEach(function (i) {
            var r = rows[i];
            if (opts.group) {
                var k = String(r[opts.group] === null || r[opts.group] === undefined ? '' : r[opts.group]);
                if (prev !== k) { if (prev !== null && opts.groupTotals) html += totRow(g, 'gtot'); g = {}; prev = k; html += '<tr class="grp"><td colspan="' + vis.length + '">' + esc((opts.groupCaption || opts.group) + ': ' + k) + '</td></tr>'; }
            }
            html += '<tr data-i="' + i + '"' + (i === opts.sel ? ' class="sel"' : '') + '>' + vis.map(function (c) {
                if (c.btn) return '<td class="c"><button type="button" class="x" data-btn="' + c.key + '" data-i="' + i + '">' + esc(c.btn) + '</button></td>';
                if (c.chk) return '<td class="c"><input type="checkbox" data-chk="' + c.key + '" data-i="' + i + '"' + (r[c.key] ? ' checked' : '') + '></td>';
                if (c.edit === 'num') return '<td class="ed"><input type="text" class="n" data-ed="' + c.key + '" data-i="' + i + '" value="' + esc(r[c.key] === null || r[c.key] === undefined ? '' : r[c.key]) + '"></td>';
                if (c.edit === 'text') return '<td class="ed"><input type="text" data-ed="' + c.key + '" data-i="' + i + '" value="' + esc(r[c.key] === null || r[c.key] === undefined ? '' : r[c.key]) + '"></td>';
                if (c.edit && c.edit.options) return '<td class="ed"><select data-ed="' + c.key + '" data-i="' + i + '"><option value="0"></option>' + c.edit.options.map(function (o) { return '<option value="' + esc(o.Id) + '"' + (String(o.Id) === String(r[c.key]) ? ' selected' : '') + '>' + esc(o.Name) + '</option>'; }).join('') + '</select></td>';
                var cls = (c.fmt ? 'n' : (c.cls || '')) + (opts.redIf && opts.redIf(r) ? ' red' : '');
                return '<td' + (cls ? ' class="' + cls.trim() + '"' : '') + '>' + esc(cellText(r, c)) + '</td>';
            }).join('') + '</tr>';
            sums.forEach(function (k) { var v = Number(r[k]) || 0; tot[k] = (tot[k] || 0) + v; g[k] = (g[k] || 0) + v; });
        });
        if (opts.group && prev !== null && opts.groupTotals) html += totRow(g, 'gtot');
        t.tBodies[0].innerHTML = html;
        t.tFoot.innerHTML = sums.length ? totRow(tot, 'tot') : '';
        if (!t.__wired) {
            t.__wired = true;
            t.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (k) { state.filters[k] = e.target.value.trim().toLowerCase(); if (t.__rerender) t.__rerender(); } });
            t.addEventListener('change', function (e) {
                var ed = e.target.getAttribute('data-ed'), chk = e.target.getAttribute('data-chk');
                if (ed && t.__onEdit) t.__onEdit(+e.target.getAttribute('data-i'), ed, e.target.value);
                if (chk && t.__onCheck) t.__onCheck(+e.target.getAttribute('data-i'), chk, e.target.checked);
                if (e.target.hasAttribute('data-chkall')) t.tBodies[0].querySelectorAll('input[data-chk]').forEach(function (c) { c.checked = e.target.checked; if (t.__onCheck) t.__onCheck(+c.getAttribute('data-i'), c.getAttribute('data-chk'), c.checked); });
            });
            t.addEventListener('click', function (e) {
                var b = e.target.closest('button[data-btn]'); if (b && t.__onButton) { t.__onButton(+b.getAttribute('data-i'), b.getAttribute('data-btn')); return; }
                var tr = e.target.closest('tbody tr[data-i]'); if (tr && t.__onSelect) t.__onSelect(+tr.getAttribute('data-i'));
            });
            t.addEventListener('dblclick', function (e) { var tr = e.target.closest('tbody tr[data-i]'); if (tr && t.__onDouble) t.__onDouble(+tr.getAttribute('data-i')); });
        }
        t.__rerender = function () { renderGrid(tableId, cols, opts); };
        t.__onEdit = opts.onEdit || null; t.__onCheck = opts.onCheck || null; t.__onButton = opts.onButton || null; t.__onSelect = opts.onSelect || null; t.__onDouble = opts.onDouble || null;
        if (opts.navId) $id(opts.navId).textContent = 'Records: ' + idx.length + (idx.length !== rows.length ? ' of ' + rows.length : '');
    }
    function clearGrid(tableId) { var t = $id(tableId); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; if (t.__fx) t.__fx.filters = {}; }
    function checkedIdx(tableId) { var out = []; $id(tableId).tBodies[0].querySelectorAll('input[data-chk]:checked').forEach(function (c) { out.push(+c.getAttribute('data-i')); }); return out; }

    /* ---------------------------------------------------------------- print (Reporting.ShowReportWithDataTable) */
    function printTable(rpt, rows, title) {
        if (!rows || !rows.length) { alert('Record Not Found For Display'); return; }
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify({ rpt: rpt, title: title || null, rows: rows }) })
            .then(function (r) {
                if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); });
                return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (w) w.close(); alert(e.message); });
    }

    /* ---------------------------------------------------------------- tabs (TabAlignment.Bottom) */
    function tabs(rootId, onChange) {
        var root = $id(rootId);
        root.querySelectorAll('.win-tab').forEach(function (tb) {
            tb.addEventListener('click', function () { select(tb.getAttribute('data-tab')); });
        });
        function select(name, silent) {
            var changed = !$id(name).classList.contains('is-active');
            root.querySelectorAll('.win-tab').forEach(function (tb) { tb.classList.toggle('is-active', tb.getAttribute('data-tab') === name); });
            root.querySelectorAll('.win-tab').forEach(function (tb) { var p = $id(tb.getAttribute('data-tab')); if (p) p.classList.toggle('is-active', tb.getAttribute('data-tab') === name); });
            if (changed && !silent && onChange) onChange(name);
        }
        function current() { var a = root.querySelector('.win-tab.is-active'); return a ? a.getAttribute('data-tab') : null; }
        return { select: select, current: current };
    }
    function closeForm() { if (window.opener || history.length <= 1) window.close(); else history.back(); }
    function fail(e) { alert(e && e.message ? e.message : String(e)); }

    window.Inv4a = { fmt: fmt, num: num, toInt: toInt, ddMMMyyyy: ddMMMyyyy, ddMMMyy: ddMMMyy, ddMMMyyTime: ddMMMyyTime, nowTime: nowTime,
        bindDdl: bindDdl, selText: selText, selVal: selVal, hasActiveRow: hasActiveRow, multi: multi, multiChecked: multiChecked, multiClear: multiClear,
        renderGrid: renderGrid, clearGrid: clearGrid, checkedIdx: checkedIdx, printTable: printTable, tabs: tabs, closeForm: closeForm, fail: fail };
})();
