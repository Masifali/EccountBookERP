/* ============================================================================================
 * Inactive Account_Reports group K (55 / 57 / 58 / 59) - helpers on top of the shared AcRpt1 page engine (countx_acrpt1_core.js).
 *   window.Inact2 = { fmtSingle, fmt00, fmtTotal2, totals, enterTab, digitsOnly, ledgerUrl, grdKeys, closeForm, shortcutRows }
 * Number formats (Janus FormatString / TotalFormatString):
 *   stringFormatsingle = "#,##0." + N zeros (N = DefaultNoofDecimalPointsForAmount)   fmtSingle
 *   "0,0"      -> integer with thousands separators                                    fmt00
 *   "#,##0.##" -> up to two decimals, trailing zeros dropped                           fmtTotal2
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, I = w.Inact2 = {};

    function group(intPart) { return intPart.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /* .NET custom formats round half away from zero */
    function roundTo(abs, places) { var f = Math.pow(10, places); return Math.round(abs * f + 1e-9) / f; }

    I.fmtSingle = function (value, places) {
        var n = Number(value);
        if (value === null || value === undefined || value === '' || !isFinite(n)) return value == null ? '' : String(value);
        var s = roundTo(Math.abs(n), places).toFixed(places), parts = s.split('.');
        var out = group(parts[0]) + (parts[1] ? '.' + parts[1] : '');
        return (n < 0 && Number(s) !== 0 ? '-' : '') + out;
    };
    I.fmt00 = function (value) {
        var n = Number(value);
        if (value === null || value === undefined || value === '' || !isFinite(n)) return value == null ? '' : String(value);
        var r = roundTo(Math.abs(n), 0);
        return (n < 0 && r !== 0 ? '-' : '') + group(String(r));
    };
    I.fmtTotal2 = function (value) {
        var n = Number(value);
        if (!isFinite(n)) return '';
        var s = roundTo(Math.abs(n), 2).toFixed(2).replace(/\.?0+$/, ''), parts = s.split('.');
        var out = group(parts[0]) + (parts[1] ? '.' + parts[1] : '');
        return (n < 0 && Number(s) !== 0 ? '-' : '') + out;
    };

    /* TotalRow bottom-fixed: grid.totalsRow override so each column can carry its own TotalFormatString (c.totalFmt(sum)); c.sum = AggregateFunction.Sum */
    I.totals = function (grid, noLabel) {
        grid.totalsRow = function (cls, rows, tag) {
            var tr = d.createElement('tr'); tr.className = cls;
            this.visibleCols().forEach(function (c, i) {
                var td = d.createElement('td');
                if (c.sum) {
                    var s = 0; rows.forEach(function (r) { s += A.toDouble(A.ci(r, c.key)); });
                    td.className = 'num'; td.textContent = c.totalFmt ? c.totalFmt(s) : String(s);
                } else if (i === 0 && tag && !noLabel) td.textContent = tag;
                tr.appendChild(td);
            });
            return tr;
        };
    };

    /* KeyPreview: Return -> SendKeys "{TAB}" (next control in TabIndex order; buttons keep their own Enter) */
    I.enterTab = function (order) {
        d.addEventListener('keydown', function (e) {
            if (e.key !== 'Enter' || e.ctrlKey || e.altKey || e.shiftKey) return;
            var t = e.target;
            if (!t || t.tagName === 'BUTTON' || t.tagName === 'TEXTAREA' || (t.closest && t.closest('#grid, table.g'))) return;
            var i = -1;
            order.forEach(function (id, k) {
                var x = A.focusable(id);
                if (x === t || (x && x.contains && x.contains(t)) || (A.el(id) === t)) i = k;
            });
            if (i < 0) return;
            if (A.comboOpen()) return;
            e.preventDefault();
            var next = A.focusable(order[(i + 1) % order.length]);
            if (next) next.focus();
        });
    };

    /* OnlytextNumberFunction: digits and control keys only */
    I.digitsOnly = function (input) {
        input.addEventListener('keypress', function (e) { if (e.key.length === 1 && !/\d/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
    };

    /* CommonServices.GoToGeneralLedgerFromLinkedEvent(accountId, from, to[, subsidiaryAccountId]) -> the General Ledger page in a new tab */
    I.ledgerUrl = function (accountId, from, to, subsidiaryId) {
        var q = new URLSearchParams();
        q.set('accountId', accountId);
        if (from) q.set('fromDate', from);
        if (to) q.set('toDate', to);
        if (A.toInt(subsidiaryId) > 0) q.set('subsidiaryAccountId', subsidiaryId);
        return '/accounts/reports/general-ledger?' + q.toString();
    };

    /* Reporting.ShowReportWithDataTable over the server's re-run of the BLL call; an empty result reads as the desktop's text */
    I.openPrint = function (btn, url) {
        if (btn && btn.disabled) return Promise.resolve();
        A.busy(btn, true);
        return A.openPdf(url).catch(function (e) {
            alert(/No Record Found For Display/i.test(e.message) ? 'Not Record Found For Display' : e.message);
        }).then(function () { A.busy(btn, false); });
    };

    I.closeForm = function () { w.location.href = '/accounts/dashboard'; };
}(window, document));
