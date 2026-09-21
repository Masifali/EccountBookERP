/* ============================================================================================
 * Job Order Summary Report — frmProductionJobOrderSummaryRpt.cs, ScreenDefinition 975.
 *
 *   grid      [dbo].[usp_JobOrderSummary_Report]
 *   drilldown [dbo].[USP_ProductionSettlement_WithReferenceDocumentDetailReport]
 *
 * The desktop renders through Crystal (672_JobOrderSummaryReport.rpt). This shows the same rows
 * as a grid; printing is browser output and CSV export is additional. That substitution is this
 * port's standing one for .rpt, and it is stated on the page rather than left implicit.
 *
 * Columns are built from what the procedure returns rather than from a fixed list: the procedure
 * body has not been read, so a guessed header set would silently drop any column it projects.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/production/reports/job-order-summary';
    var rows = [];      // the last result, kept for CSV so export cannot disagree with the screen
    var cols = [];

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function box(m) { window.alert(m); }

    function esc(s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function int(v) {
        var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10);
        return isNaN(n) ? 0 : n;
    }
    function ci(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) {
            if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) {
                return row[k];
            }
        }
        return '';
    }

    /* Disable → spinner → ignore repeat clicks → re-enable on success AND failure. */
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
        return r ? r.value : 'All';
    }

    /* The From picker mirrors the desktop's ShowCheckBox: unticked means the parameter is not
       sent at all, which is a different query from sending an empty date. */
    function toggleFrom() {
        var on = $id('chkFromDate').checked;
        $id('datFromDate').disabled = !on;
        if (!on) $id('datFromDate').value = '';
    }

    function load() {
        return busy('btnShow', function () {
            var q = ['approvalFilter=' + encodeURIComponent(approval())];
            if ($id('chkFromDate').checked && val('datFromDate')) {
                q.push('fromDate=' + encodeURIComponent(val('datFromDate')));
            }
            if (val('datToDate')) q.push('toDate=' + encodeURIComponent(val('datToDate')));

            hideDetail();
            return getJson(api + '?' + q.join('&')).then(function (data) {
                rows = data || [];
                render();
            }).catch(function (e) {
                rows = []; cols = [];
                $id('summaryHead').innerHTML = '';
                $id('summaryBody').innerHTML = '';
                $id('lblCount').textContent = '';
                box(e.message);
            });
        });
    }

    function render() {
        var head = $id('summaryHead'), body = $id('summaryBody');
        if (!rows.length) {
            cols = [];
            head.innerHTML = '';
            body.innerHTML = '<tr><td>No records</td></tr>';
            $id('lblCount').textContent = '0 record(s)';
            return;
        }
        cols = Object.keys(rows[0]);
        var shown = cols.filter(function (c) { return c.toLowerCase() !== 'id'; });
        head.innerHTML = shown.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');

        body.innerHTML = rows.map(function (r, i) {
            return '<tr onclick="JobOrderSummary.detail(' + i + ')">'
                 + shown.map(function (c) { return '<td>' + esc(r[c]) + '</td>'; }).join('')
                 + '</tr>';
        }).join('');
        $id('lblCount').textContent = rows.length + ' record(s)';
    }

    /** The desktop's row double-click; a single click here, since a grid row is not a form. */
    function detail(index) {
        var r = rows[index];
        if (!r) return;
        var jobOrderId = int(ci(r, 'Id'));
        if (jobOrderId <= 0) { box('This row carries no job order id, so it has no detail.'); return; }

        return getJson(api + '/detail?jobOrderId=' + jobOrderId).then(function (data) {
            var d = data || [];
            var head = $id('detailHead'), body = $id('detailBody');
            $id('detailBox').style.display = '';
            $id('lblDetailTitle').textContent = 'Settlement Detail - Job Order ' + jobOrderId;
            if (!d.length) {
                head.innerHTML = '';
                body.innerHTML = '<tr><td>No detail rows</td></tr>';
                return;
            }
            var dc = Object.keys(d[0]);
            head.innerHTML = dc.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('');
            body.innerHTML = d.map(function (x) {
                return '<tr>' + dc.map(function (c) { return '<td>' + esc(x[c]) + '</td>'; }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { box(e.message); });
    }

    function hideDetail() {
        $id('detailBox').style.display = 'none';
        $id('detailHead').innerHTML = '';
        $id('detailBody').innerHTML = '';
    }

    /** Exports what is on screen, from the same array the grid was drawn from. */
    function exportCsv() {
        if (!rows.length) { box('Nothing to export yet.'); return; }
        var shown = cols.filter(function (c) { return c.toLowerCase() !== 'id'; });
        var lines = [shown.map(csvCell).join(',')];
        rows.forEach(function (r) { lines.push(shown.map(function (c) { return csvCell(r[c]); }).join(',')); });
        var blob = new Blob(['﻿' + lines.join('\r\n')], { type: 'text/csv;charset=utf-8;' });
        var a = document.createElement('a');
        a.href = URL.createObjectURL(blob);
        a.download = 'job-order-summary.csv';
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        setTimeout(function () { URL.revokeObjectURL(a.href); }, 1000);
    }

    function csvCell(v) {
        var s = (v === null || v === undefined) ? '' : String(v);
        return /[",\r\n]/.test(s) ? '"' + s.replace(/"/g, '""') + '"' : s;
    }

    function toggleFullscreen(boxId) {
        var el = $id(boxId);
        if (el) el.classList.toggle('is-fullscreen');
    }

    function boot() {
        var d = new Date();
        $id('datToDate').value = d.getFullYear() + '-'
            + String(d.getMonth() + 1).padStart(2, '0') + '-'
            + String(d.getDate()).padStart(2, '0');
        toggleFrom();
    }

    window.JobOrderSummary = {
        load: load,
        detail: detail,
        toggleFrom: toggleFrom,
        exportCsv: exportCsv,
        toggleFullscreen: toggleFullscreen
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
}());
