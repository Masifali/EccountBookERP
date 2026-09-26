/* ============================================================================================
 * Job Order Summary Report - frmProductionJobOrderSummaryRpt.cs, ScreenDefinition 975.
 *
 *   Show      [dbo].[usp_JobOrderSummary_Report]  (gridHisory:137)
 *             @IsApproved only when not "All"; @FromDate only when ticked; @ToDate always.
 *   Grid      the twelve dtGrid columns (Load:100), Id hidden; JobOrderNo is a LINK column.
 *   Link      DataGridHistory_LinkClicked:210 - actionId = 1 ->
 *             [dbo].[USP_ProductionSettlement_WithReferenceDocumentDetailReport] ->
 *             613_02_ProductionSettlement_WithReferenceDocumentDetailReport.rpt (+ 612 sub-report)
 *   672       BtnPrint_Click:311 - the raw result into 672_JobOrderSummaryReport.rpt
 *   Keys      KeyDown:347 - Enter=Tab, Ctrl+E/Esc close, Ctrl+Alt shortcuts, Ctrl+Down grid,
 *             Ctrl+S show, Ctrl+P print, Ctrl+Up From Date. (Ctrl+N/Ctrl+R are listed by the
 *             desktop's own shortcut form but are NOT handled by its KeyDown.)
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/reports/job-order-summary';
    var K = window.ReportKit;
    var rows = [];
    var lastArgs = null;          // the parameters of the last Show - what 672 prints
    var amountDecimals = 2;
    var current = -1;

    /* dtGrid, Id hidden. [column, kind] - kind: date | link | w (#,##0.## + sum) | both (stringFormatboth + sum) | text */
    var COLS = [
        ['DocDate', 'date'], ['JobOrderNo', 'link'],
        ['InputWeight', 'w'], ['InputAmount', 'w'], ['OutPutWeight', 'w'], ['OutPutAmount', 'w'],
        ['BalanceWeight', 'w'], ['BalanceAmount', 'both'], ['LedgerBalance', 'both'],
        ['SettlementStatus', 'text'], ['ApprovalStatus', 'text'], ['JobOrderStatus', 'text']
    ];

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function box(m) { window.alert(m); }
    var esc = K.esc;
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) {
            if (b.disabled || b.classList.contains('is-busy')) return;
            b.disabled = true;
            b.classList.add('is-busy');
        }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } };
        var p;
        try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }

    function getJson(url) {
        return fetch(url, { headers: { 'Accept': 'application/json' }, credentials: 'same-origin' })
            .then(function (r) {
                return r.text().then(function (t) {
                    var body = null;
                    try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                    if (!r.ok) throw new Error((body && body.message) || ('Request failed (' + r.status + ')'));
                    return body;
                });
            });
    }

    function approval() {
        var r = document.querySelector('input[name="approval"]:checked');
        return r ? r.value : 'Unapproved';
    }

    /** The picker's own checkbox: unticked = @FromDate not sent. The value is kept. */
    function toggleFrom() { $id('datFromDate').disabled = !$id('chkFromDate').checked; }

    function load() {
        return busy('btnShow', function () {
            if (!val('datToDate')) $id('datToDate').value = iso(new Date());   // @ToDate is always sent
            var q = ['approvalFilter=' + encodeURIComponent(approval())];
            var args = { toDate: val('datToDate') };
            if ($id('chkFromDate').checked && val('datFromDate')) {
                q.push('fromDate=' + encodeURIComponent(val('datFromDate')));
                args.fromDate = val('datFromDate');
            }
            q.push('toDate=' + encodeURIComponent(val('datToDate')));
            if (approval() !== 'All') args.isApproved = approval() === 'Approved';

            hideDetail();
            return getJson(api + '?' + q.join('&')).then(function (data) {
                rows = data || [];
                lastArgs = args;
                render();
            }).catch(function (e) {
                rows = []; lastArgs = null;
                render();
                box(e.message);
            });
        });
    }

    function cell(c, r, i) {
        var v = r[c[0]];
        switch (c[1]) {
            case 'date': return '<td>' + esc(K.dMMMyy(v)) + '</td>';
            case 'link': return '<td><a class="jo-link" tabindex="0" data-i="' + i + '">' + esc(v) + '</a></td>';
            case 'w':    return '<td class="num">' + esc(K.num(v, 2)) + '</td>';
            case 'both': return '<td class="num">' + esc(K.both(v, amountDecimals)) + '</td>';
            default:     return '<td>' + esc(v) + '</td>';
        }
    }

    function render() {
        current = -1;
        $id('summaryHead').innerHTML = COLS.map(function (c) {
            return '<th' + (c[1] === 'w' || c[1] === 'both' ? ' class="num"' : '') + '>' + esc(c[0]) + '</th>';
        }).join('');
        $id('summaryBody').innerHTML = rows.length
            ? rows.map(function (r, i) { return '<tr tabindex="0" data-i="' + i + '">' + COLS.map(function (c) { return cell(c, r, i); }).join('') + '</tr>'; }).join('')
            : '';
        /* Totals row, bottom fixed. */
        $id('summaryFoot').innerHTML = rows.length ? '<tr>' + COLS.map(function (c, k) {
            if (c[1] !== 'w' && c[1] !== 'both') return '<td>' + (k === 0 ? 'Total' : '') + '</td>';
            var t = rows.reduce(function (a, r) { var n = parseFloat(r[c[0]]); return a + (isNaN(n) ? 0 : n); }, 0);
            return '<td class="num">' + esc(c[1] === 'both' ? K.both(t, amountDecimals) : K.num(t, 2)) + '</td>';
        }).join('') + '</tr>' : '';
        $id('lblCount').textContent = rows.length + ' record(s)';
        K.filterRow($id('tblSummary'));
    }

    /** DataGridHistory_LinkClicked:210 - rows first ("Record Not Found For DisPlay"), then 613_02. */
    function detail(index) {
        var r = rows[index];
        if (!r) return;
        var jobOrderId = parseInt(r.Id, 10) || 0;
        var link = $id('summaryBody').querySelector('a.jo-link[data-i="' + index + '"]');
        var win = window.CrystalPrint.reserve();
        return busy(link, function () {
            return getJson(api + '/detail?jobOrderId=' + jobOrderId + '&actionId=1').then(function (data) {
                var d = data || [];
                if (!d.length) { window.CrystalPrint.release(win); hideDetail(); box('Record Not Found For DisPlay'); return; }
                /* The rows stay visible below the grid as well, so the settlement can be read even
                   where the Crystal renderer is switched off. */
                var dc = Object.keys(d[0]);
                $id('detailBox').style.display = '';
                $id('lblDetailTitle').textContent = '613_02 Production Settlement - ' + (r.JobOrderNo || jobOrderId);
                $id('detailHead').innerHTML = dc.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');
                $id('detailBody').innerHTML = d.map(function (x) {
                    return '<tr>' + dc.map(function (c) { return '<td>' + esc(x[c]) + '</td>'; }).join('') + '</tr>';
                }).join('');
                return window.CrystalPrint.open('jo-613-02', { id: jobOrderId }, null, win);
            }).catch(function (e) { window.CrystalPrint.release(win); box('Error: ' + e.message); });
        });
    }

    function hideDetail() {
        $id('detailBox').style.display = 'none';
        $id('detailHead').innerHTML = '';
        $id('detailBody').innerHTML = '';
    }

    /** BtnPrint_Click:311 - prints the last Show's rows. */
    function print() {
        if (!lastArgs || !rows.length) { box('Not Record Found For Display'); return; }
        return window.CrystalPrint.open('jos-672', lastArgs, 'btnPrint');
    }

    /** Reset:285 - clears the grid and focuses From Date. Nothing else. */
    function reset() {
        rows = []; lastArgs = null;
        $id('summaryHead').innerHTML = ''; $id('summaryBody').innerHTML = ''; $id('summaryFoot').innerHTML = '';
        $id('lblCount').textContent = '';
        hideDetail();
        $id('chkFromDate').focus();
    }

    function shortcuts() {
        K.shortcuts([['Ctrl+S', 'For Show Data'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'],
                     ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Date Type'],
                     ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Main Grid'],
                     ['Ctrl+ArrowUp', 'For Focus On on Date Type'],
                     ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    function exportCsv() {
        if (!rows.length) { box('Nothing to export yet.'); return; }
        var names = COLS.map(function (c) { return c[0]; });
        var lines = [names.map(csvCell).join(',')];
        rows.forEach(function (r) { lines.push(names.map(function (c) { return csvCell(r[c]); }).join(',')); });
        var blob = new Blob(['﻿' + lines.join('\r\n')], { type: 'text/csv;charset=utf-8;' });
        var a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = 'job-order-summary.csv';
        document.body.appendChild(a); a.click(); document.body.removeChild(a);
        setTimeout(function () { URL.revokeObjectURL(a.href); }, 1000);
    }
    function csvCell(v) {
        var s = (v === null || v === undefined) ? '' : String(v);
        return /[",\r\n]/.test(s) ? '"' + s.replace(/"/g, '""') + '"' : s;
    }

    function toggleFullscreen(boxId) { var el = $id(boxId); if (el) el.classList.toggle('is-fullscreen'); }

    function focusGrid() {
        var tr = $id('summaryBody').querySelector('tr[tabindex="0"]');
        if (tr) tr.focus();
    }

    function boot() {
        var today = new Date();
        $id('datToDate').value = iso(today);
        var from = new Date(); from.setDate(from.getDate() - 3);
        $id('datFromDate').value = iso(from);
        $id('chkFromDate').checked = false;
        toggleFrom();

        /* InitializeComponentMethod:122 - the configured look-back and the amount format. */
        getJson(api + '/defaults').then(function (d) {
            if (!d) return;
            var f = new Date(); f.setDate(f.getDate() - (parseInt(d.fromDaysBack, 10) || 3));
            $id('datFromDate').value = iso(f);
            if (d.amountDecimals !== undefined) amountDecimals = parseInt(d.amountDecimals, 10) || 0;
        }).catch(function () { /* the -3 default already stands */ });

        var body = $id('summaryBody');
        body.addEventListener('click', function (e) {
            var a = e.target.closest('a.jo-link');
            var tr = e.target.closest('tr[data-i]');
            if (tr) {
                current = +tr.getAttribute('data-i');
                body.querySelectorAll('tr').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            }
            if (a) detail(+a.getAttribute('data-i'));
        });
        body.addEventListener('keydown', function (e) {
            /* Ctrl+Space on the grid calls the link, as the desktop's shortcut list says. */
            if (e.ctrlKey && e.key === ' ') {
                var tr = e.target.closest('tr[data-i]');
                if (tr) { e.preventDefault(); detail(+tr.getAttribute('data-i')); }
            }
        });

        K.enterToTab();
        K.keys({
            'ctrl+e': K.close, 'esc': K.close, 'ctrl+alt': shortcuts,
            'ctrl+arrowdown': focusGrid, 'ctrl+s': load, 'ctrl+p': print,
            'ctrl+arrowup': function () { $id('chkFromDate').focus(); }
        });
        $id('chkFromDate').focus();
        render();
    }

    window.JobOrderSummary = {
        load: load, detail: detail, toggleFrom: toggleFrom, print: print, reset: reset,
        shortcuts: shortcuts, exportCsv: exportCsv, toggleFullscreen: toggleFullscreen
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
