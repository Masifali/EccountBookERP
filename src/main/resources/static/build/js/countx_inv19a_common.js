/* Batch inv19a shared grid plumbing (Janus GridEX look-alike) for the four Inventory "Stock Reports" report screens:
   294 RequiredRawMaterialAgainstSaleOrders, 290 frmFiFoAuditReport, 296 StockReportyWithFIFO, 842 StockReportByFifo.
   Nothing here is business logic - each screen's file carries the desktop line references. Requires countx_store_common.js. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc;

    /* .NET custom numeric formats used by the four GridSettings():
         "#,##0.##"               -> { dec: 2 }
         "#,##0.##;(#,##0.##)"    -> { dec: 2, paren: true }
         "#,#;(#,#.##);0"         -> { dec: 0, paren: true, negDec: 2 }
         "#,##0.###"              -> { dec: 3 }
         "#,##0"                  -> { dec: 0 }
         "#,#0.####"              -> { dec: 4 }                                                   */
    function roundAway(x, d) { var f = Math.pow(10, d), s = x < 0 ? -1 : 1; return s * Math.round(Math.abs(x) * f + 1e-9) / f; }
    function fmt(v, f) {
        v = Number(String(v === null || v === undefined ? '' : v).replace(/,/g, '')) || 0;
        f = f || { dec: 2 };
        var neg = v < 0, dec = neg && f.negDec !== undefined ? f.negDec : f.dec;
        var a = Math.abs(roundAway(v, dec)).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dec });
        if (!neg) return a;
        return f.paren ? '(' + a + ')' : '-' + a;
    }

    /* A GridEX: cols = [{key, caption, width, num:{...}, sum:true|'avg', link:true, button:text, hidden}],
       opts = {groups:[key], onLink(key,row), onButton(key,row), navigator:'#id'}. Filter row (FilterMode 2), TotalRow, GroupTotals Always. */
    function Grid(tableId, cols, opts) {
        this.t = $id(tableId); this.cols = cols; this.opts = opts || {}; this.rows = []; this.filters = {}; this.sel = -1;
        var self = this;
        this.t.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (!k) return; self.filters[k] = e.target.value.trim().toLowerCase(); self.renderBody(); });
        this.t.addEventListener('click', function (e) {
            var a = e.target.closest('a[data-link]'); if (a) { e.preventDefault(); if (self.opts.onLink) self.opts.onLink(a.getAttribute('data-link'), self.rows[+a.getAttribute('data-i')]); return; }
            var b = e.target.closest('button[data-btn]'); if (b) { e.preventDefault(); if (self.opts.onButton) self.opts.onButton(b.getAttribute('data-btn'), self.rows[+b.getAttribute('data-i')]); return; }
            var tr = e.target.closest('tbody tr[data-i]'); if (tr) { self.sel = +tr.getAttribute('data-i'); self.t.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); }
        });
    }
    Grid.prototype.visible = function () { return this.cols.filter(function (c) { return !c.hidden; }); };
    Grid.prototype.cellText = function (r, c) { var v = r[c.key]; return c.num ? fmt(v, c.num) : String(v === null || v === undefined ? '' : v); };
    Grid.prototype.setRows = function (rows) { this.rows = rows || []; this.filters = {}; this.sel = -1; this.render(); };
    Grid.prototype.clear = function () { this.setRows([]); };          // grdfrm.ClearStructure()
    Grid.prototype.render = function () {
        var vs = this.visible(), nav = this.opts.navigator ? document.querySelector(this.opts.navigator) : null;
        if (!this.rows.length) { this.t.querySelector('thead').innerHTML = ''; this.t.querySelector('tbody').innerHTML = ''; this.t.querySelector('tfoot').innerHTML = ''; if (nav) nav.textContent = ''; return; }
        var self = this;
        var h = '<tr>' + vs.map(function (c) { return '<th data-col="' + esc(c.key) + '"' + (c.width ? ' style="min-width:' + c.width + 'px;width:' + c.width + 'px"' : '') + '>' + esc(c.caption || c.key) + '</th>'; }).join('') + '</tr>';
        var f = '<tr class="flt">' + vs.map(function (c) { return c.button ? '<td></td>' : '<td><input type="text" data-flt="' + esc(c.key) + '" value="' + esc(self.filters[c.key] || '') + '"></td>'; }).join('') + '</tr>';
        this.t.querySelector('thead').innerHTML = h + f;
        this.renderBody();
    };
    Grid.prototype.renderBody = function () {
        var self = this, vs = this.visible(), groups = this.opts.groups || [], html = '', total = {}, cnt = {};
        var keys = Object.keys(this.filters).filter(function (k) { return self.filters[k]; }), idx = [];
        var byKey = {}; this.cols.forEach(function (c) { byKey[c.key] = c; });
        this.rows.forEach(function (r, i) { if (keys.every(function (k) { return self.cellText(r, byKey[k]).toLowerCase().indexOf(self.filters[k]) >= 0; })) idx.push(i); });
        var gt = groups.map(function () { return {}; }), gc = groups.map(function () { return {}; }), prev = groups.map(function () { return null; });
        function sums(obj, c2) { return vs.map(function (c) { if (!c.sum) return '<td></td>'; var v = c.sum === 'avg' ? (c2[c.key] ? obj[c.key] / c2[c.key] : 0) : obj[c.key]; return '<td class="n">' + esc(fmt(v || 0, c.num)) + '</td>'; }).join(''); }
        function closeFrom(level) { for (var L = groups.length - 1; L >= level; L--) { if (prev[L] !== null) html += '<tr class="gtot">' + sums(gt[L], gc[L]) + '</tr>'; gt[L] = {}; gc[L] = {}; prev[L] = null; } }
        idx.forEach(function (i) {
            var r = self.rows[i];
            for (var L = 0; L < groups.length; L++) {
                var key = String(r[groups[L]] == null ? '' : r[groups[L]]);
                if (prev[L] !== key) {
                    closeFrom(L);
                    for (var M = L; M < groups.length; M++) {
                        var k2 = String(r[groups[M]] == null ? '' : r[groups[M]]);
                        html += '<tr class="grp"><td colspan="' + vs.length + '" style="padding-left:' + (4 + M * 16) + 'px">' + esc((byKey[groups[M]] ? byKey[groups[M]].caption : groups[M]) + ': ' + k2) + '</td></tr>';
                        prev[M] = k2;
                    }
                    break;
                }
            }
            html += '<tr data-i="' + i + '"' + (i === self.sel ? ' class="sel"' : '') + '>' + vs.map(function (c) {
                var txt = self.cellText(r, c);
                if (c.button) return '<td><button type="button" class="cell-btn" data-btn="' + esc(c.key) + '" data-i="' + i + '">' + esc(c.button) + '</button></td>';
                if (c.link) return '<td' + (c.title ? ' title="' + esc(c.title) + '"' : '') + '><a data-link="' + esc(c.key) + '" data-i="' + i + '">' + esc(txt) + '</a></td>';
                return c.num ? '<td class="n">' + esc(txt) + '</td>' : '<td>' + esc(txt) + '</td>';
            }).join('') + '</tr>';
            vs.forEach(function (c) { if (c.sum) { var v = Number(String(r[c.key] == null ? '' : r[c.key]).replace(/,/g, '')) || 0; total[c.key] = (total[c.key] || 0) + v; cnt[c.key] = (cnt[c.key] || 0) + 1; gt.forEach(function (o, L) { o[c.key] = (o[c.key] || 0) + v; gc[L][c.key] = (gc[L][c.key] || 0) + 1; }); } });
        });
        closeFrom(0);
        this.t.querySelector('tbody').innerHTML = html;
        this.t.querySelector('tfoot').innerHTML = vs.some(function (c) { return c.sum; }) ? '<tr>' + sums(total, cnt) + '</tr>' : '';
        var nav = this.opts.navigator ? document.querySelector(this.opts.navigator) : null;
        if (nav) nav.textContent = 'Records: ' + idx.length + (idx.length !== this.rows.length ? ' of ' + this.rows.length : '');
    };

    /* Reporting.ShowReportWithDataTable(dt, rpt) + @CompanyName / @CompanyAddress (added by /reports/print/grid), exactly as the pattern screens. */
    function printRows(rpt, rows, emptyMessage) {
        if (!rows || !rows.length) { alert(emptyMessage || 'Record Not Found For Display'); return; }
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify({ rpt: rpt, title: null, rows: rows }) })
            .then(function (r) {
                if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); });
                return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); });
            })
            .catch(function (e) { if (w) w.close(); alert(e.message); });
    }

    /* small in-page modal (desktop child form); body = html string */
    function modal(title, bodyHtml) {
        var m = $id('inv19aModal');
        if (!m) {
            m = document.createElement('div'); m.id = 'inv19aModal'; m.className = 'fx-modal';
            m.innerHTML = '<div class="fx-modal-box"><div class="fx-modal-head"><span></span><button type="button" title="Close">&times;</button></div><div class="fx-modal-body"></div></div>';
            document.body.appendChild(m);
            m.querySelector('.fx-modal-head button').addEventListener('click', function () { m.classList.remove('is-open'); });
            m.addEventListener('mousedown', function (e) { if (e.target === m) m.classList.remove('is-open'); });
        }
        m.querySelector('.fx-modal-head span').textContent = title;
        m.querySelector('.fx-modal-body').innerHTML = bodyHtml;
        m.classList.add('is-open');
    }
    function rowsTable(rows) {
        if (!rows || !rows.length) return '<div style="font:12px Verdana,sans-serif;padding:8px">No Record Found</div>';
        var cols = Object.keys(rows[0]);
        return '<div class="fx-grid-host" style="max-height:70vh"><table class="win-grid"><thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr></thead><tbody>'
            + rows.map(function (r) { return '<tr>' + cols.map(function (c) { var v = r[c]; return '<td' + (typeof v === 'number' ? ' class="n"' : '') + '>' + esc(v === null || v === undefined ? '' : v) + '</td>'; }).join('') + '</tr>'; }).join('') + '</tbody></table></div>';
    }
    function closeForm() { if (window.opener || history.length <= 1) window.close(); else history.back(); }

    window.Inv19a = { fmt: fmt, Grid: Grid, printRows: printRows, modal: modal, rowsTable: rowsTable, closeForm: closeForm };
})();
