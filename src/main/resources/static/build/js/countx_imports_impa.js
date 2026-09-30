/* ============================================================================================
 * countx_imports_impa.js - helpers shared by the ImpA Import pages (525 Lc Order, 526 Lc Order Schedule,
 * 221 Import Invoice, 392-395 registers). Built on countx_hrm.js (window.HRM); exports window.ImpA.
 *   ImpA.fmt(v, '#,##0.###')   .NET custom numeric format (# / 0 placeholders, grouping, AwayFromZero)
 *   ImpA.roundAway(v, d)       Math.Round(v, d, MidpointRounding.AwayFromZero)
 *   ImpA.tabs(groupEl, onShow) desktop TabControl: [data-tab] buttons + [data-tab-page] pages
 *   ImpA.buttons(grid, id, fn) GridEX button cells (ColumnButtonClick)
 *   ImpA.footAvg(grid, key, f) AggregateFunction.Average in the totals row
 *   ImpA.gridPrint(btn, rpt, title, rows)  POST /reports/print/grid (a print of the screen's own rows)
 *   ImpA.shortcuts(title, rows) the ShortCutKeyPopUp dialog
 * ============================================================================================ */
(function (global) {
    'use strict';
    var doc = global.document;
    var ImpA = {};

    /** Math.Round(v, d, AwayFromZero). */
    ImpA.roundAway = function (v, d) {
        var p = Math.pow(10, d || 0), x = HRM.num(v) * p;
        var r = Math.round(Math.abs(x) + 1e-9) * (x < 0 ? -1 : 1);
        return r / p;
    };

    /**
     * .NET custom numeric format: "#,##0.###", "#,#.##", "##,#.##", "0,0", "#,#" ... Integer part: grouped when the
     * pattern has ',', at least as many digits as '0's before the point. Fraction: '0' forced, '#' optional.
     * Rounding is half away from zero (decimal.ToString). A value that formats to nothing shows ''.
     */
    ImpA.fmt = function (v, pattern) {
        if (v === null || v === undefined || v === '') return '';
        var n = HRM.num(v);
        var pat = String(pattern || '#,##0.##');
        var dot = pat.indexOf('.');
        var ip = dot < 0 ? pat : pat.substring(0, dot), fp = dot < 0 ? '' : pat.substring(dot + 1);
        var forced = (fp.match(/0/g) || []).length, maxd = fp.length;
        var minInt = (ip.replace(/,/g, '').match(/0/g) || []).length;
        var group = ip.indexOf(',') >= 0;
        var r = ImpA.roundAway(Math.abs(n), maxd);
        var s = r.toFixed(maxd);
        var parts = s.split('.');
        var ipart = parts[0], fpart = parts[1] || '';
        while (fpart.length > forced && fpart.charAt(fpart.length - 1) === '0') fpart = fpart.substring(0, fpart.length - 1);
        if (ipart === '0' && minInt === 0) ipart = '';
        while (ipart.length < minInt) ipart = '0' + ipart;
        if (group && ipart.length > 3) ipart = ipart.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        var out = ipart + (fpart ? '.' + fpart : '');
        if (out === '') return '';
        return (n < 0 && r !== 0 ? '-' : '') + out;
    };
    /** "#,##0.00"-style fixed format with d decimals (stringFormatsingle / ForFcy / DecimalRateFormate). */
    ImpA.fixed = function (v, d) {
        if (v === null || v === undefined || v === '') return '';
        return ImpA.fmt(v, '#,##0' + (d > 0 ? '.' + new Array(d + 1).join('0') : ''));
    };
    /** decimal.ToString(): plain invariant text, no grouping, trailing zeros of the value kept as JS prints them. */
    ImpA.plain = function (v) { var n = HRM.num(v); return String(Math.round(n * 1e10) / 1e10); };

    ImpA.numCol = function (key, caption, pattern, sum, extra) {
        return Object.assign({ key: key, caption: caption, type: 'num', sum: !!sum, align: 'right',
            render: function (v) { return HRM.esc(ImpA.fmt(v, pattern)); } }, extra || {});
    };
    ImpA.dateCol = function (key, caption, hidden) { return { key: key, caption: caption, type: 'date', hidden: !!hidden }; };
    ImpA.hid = function (key) { return { key: key, hidden: true }; };
    ImpA.btnCol = function (key, text, width) {
        return { key: key, caption: text, width: width || 50, render: function () {
            return '<button type="button" class="win-btn-action hrm-cell-btn" data-act="' + HRM.esc(key) + '">' + HRM.esc(text) + '</button>'; } };
    };

    /** Totals row: rewrite a sum column's footer with its formatted text / an Average. */
    ImpA.footer = function (grid, fns) {
        var t = grid.table; if (!t || !t.tFoot) return;
        var tds = t.tFoot.querySelectorAll('td');
        grid.columns.forEach(function (c, i) {
            if (!fns[c.key] || !tds[i]) return;
            tds[i].className = 'hrm-al-right';
            tds[i].textContent = fns[c.key](grid.visibleRows());
        });
    };
    ImpA.sumOf = function (rows, key) { var s = 0; rows.forEach(function (r) { s += HRM.num(HRM.col(r, key)); }); return s; };
    ImpA.avgOf = function (rows, key) { return rows.length ? ImpA.sumOf(rows, key) / rows.length : 0; };

    /** GridEX ColumnButtonClick: a click on a [data-act] button inside the grid's table. */
    ImpA.buttons = function (grid, tableId, fn) {
        HRM.$(tableId).addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            e.stopPropagation();
            var i = +tr.getAttribute('data-i');
            grid.select(i);
            fn(grid.rows()[i], b.getAttribute('data-act'), i, b);
        });
    };

    /** TabControl: buttons [data-tab=pageId] inside `bar`; pages carry data-tab-page=<group>. */
    ImpA.tabs = function (bar, onShow) {
        bar = HRM.$(bar);
        var api = {
            show: function (id) {
                bar.querySelectorAll('.hrm-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
                bar.querySelectorAll('.hrm-tab').forEach(function (t) {
                    var p = HRM.$(t.getAttribute('data-tab'));
                    if (p) p.classList.toggle('is-active', t.getAttribute('data-tab') === id);
                });
                api.current = id;
                if (onShow) onShow(id);
            },
            current: null
        };
        bar.addEventListener('click', function (e) {
            var t = e.target.closest('.hrm-tab'); if (!t || t.disabled) return;
            api.show(t.getAttribute('data-tab'));
        });
        var a = bar.querySelector('.hrm-tab.is-active');
        api.current = a ? a.getAttribute('data-tab') : null;
        return api;
    };

    /** ShortCutKeyPopUp: the form's MakeShortCutKeys() table. */
    ImpA.shortcuts = function (rows) {
        var html = '<table class="win-grid hrm-grid"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>' +
            rows.map(function (r) { return '<tr><td>' + HRM.esc(r[0]) + '</td><td>' + HRM.esc(r[1]) + '</td></tr>'; }).join('') + '</tbody></table>';
        HRM.modal({ title: 'ShortCut Keys', width: 'min(560px, 96vw)', html: '<div class="hrm-grid-wrap">' + html + '</div>' });
    };

    /** Rows the page holds, printed with the desktop .rpt (ShowReportWithDataTable of the form's DataTable). */
    ImpA.gridPrint = function (btn, rpt, title, rows) {
        var w = null;
        try { w = global.open('', '_blank'); } catch (e) { w = null; }
        return HRM.busy(btn, function () {
            var t = doc.querySelector('meta[name="_csrf"]'), n = doc.querySelector('meta[name="_csrf_header"]');
            var h = { 'Content-Type': 'application/json', 'Accept': 'application/pdf' };
            if (t && n && t.getAttribute('content') && n.getAttribute('content')) h[n.getAttribute('content')] = t.getAttribute('content');
            return fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: h,
                body: JSON.stringify({ rpt: rpt, title: title, rows: rows }) }).then(function (r) {
                var ct = r.headers.get('Content-Type') || '';
                if (r.ok && ct.indexOf('application/pdf') === 0) {
                    return r.blob().then(function (b) { var url = URL.createObjectURL(b); if (w) w.location.href = url; else global.open(url, '_blank'); });
                }
                return r.text().then(function (x) { if (w) w.close(); throw new Error(x || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { try { if (w && !w.closed) w.close(); } catch (x) { /* ignore */ } HRM.fail(e); });
        });
    };

    /** A print whose record / rights the page's API checks first; the viewer tab is reserved inside the click. */
    ImpA.checkedPrint = function (btn, checkUrl, params, key, args) {
        var w = global.CrystalPrint ? global.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return HRM.get(checkUrl, params).then(function () {
                if (!global.CrystalPrint) { HRM.box('The report viewer is not loaded.'); return; }
                return global.CrystalPrint.open(key, args, null, w);
            }).catch(function (e) { if (w && global.CrystalPrint) global.CrystalPrint.release(w); HRM.fail(e); });
        });
    };

    /** yyyy-MM-dd of a server date value ('' when none / the .NET minimum). */
    ImpA.day = function (v) { var d = HRM.day(v); return d && d.indexOf('0001-') !== 0 && d.indexOf('1900-01-01') !== 0 ? d : ''; };

    /** Text-only numeric guard (CommonServices.OnlytextdecimelFunction / OnlytextNumberFunction). */
    ImpA.guard = function (id, decimals) {
        var e = HRM.$(id); if (!e) return;
        e.addEventListener('keypress', function (ev) {
            var ch = ev.key;
            if (ch.length !== 1 || ev.ctrlKey) return;
            if (/\d/.test(ch)) return;
            if (decimals && ch === '.' && e.value.indexOf('.') < 0) return;
            ev.preventDefault();
        });
    };

    global.ImpA = ImpA;
})(window);
