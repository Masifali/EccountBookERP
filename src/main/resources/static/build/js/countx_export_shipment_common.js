/* ============================================================================================
 * countx_export_shipment_common.js - helpers the five Export shipment pages share on top of
 * countx_export_g_common.js (ExportG): an editable GridEX-like table (cells with EditType text /
 * number / value list, CellUpdated after the edit, X / + column buttons, Σ totals), a modal loader
 * dialog shell, the history footer toggle and CrystalPrint wrappers.
 *   /export/goods-receipts-at-port, /export/pm-issuance-for-shipment, /export/fi-utilized-against-shipment,
 *   /export/invoice-against-forwarding, /export/dhl-tracking
 * ============================================================================================ */
(function (global) {
    'use strict';
    var G = global.ExportG;

    /**
     * editGrid(tableId, columns, opts)
     *   columns: [{key, num, dec, edit: 'text'|'num'|'list', list: [{Id, name}], hidden, link, date, fmt}]
     *   opts: {withX, plus, totals: [keys], onCellUpdated(i, key, row), onDelete(i), onPlus(i), onDblClick(i), onSelect(i), onLink(i, key)}
     * Rows are plain objects; edits write back into the row and then call onCellUpdated (Janus CellUpdated).
     */
    function editGrid(tableId, columns, opts) {
        opts = opts || {};
        var t = G.$id(tableId);
        var g = { rows: [], current: -1 };
        function display(c, v) {
            if (c.fmt) return c.fmt(v);
            if (c.date) return G.shortDate(v);
            if (c.edit === 'list') { var o = (c.list || []).filter(function (x) { return G.netI(x.Id) === G.netI(v); })[0]; return o ? o.name : ''; }
            if (c.num) return G.fmt(v, c.dec === undefined ? 3 : c.dec);
            return G.str(v);
        }
        g.draw = function (rows) {
            if (rows) g.rows = rows;
            if (!t) return;
            var tb = t.tBodies[0] || t.createTBody();
            var h = '';
            g.rows.forEach(function (r, i) {
                h += '<tr data-i="' + i + '"' + (i === g.current ? ' data-current="1"' : '') + '>';
                if (opts.withX) h += '<td class="win-cell-btn"><button type="button" class="win-x" data-del="' + i + '" title="Delete">X</button></td>';
                if (opts.plus) h += '<td class="win-cell-btn"><button type="button" class="win-x" data-plus="' + i + '" title="Add" style="color:#064">+</button></td>';
                columns.forEach(function (c) {
                    if (c.hidden) return;
                    var v = G.col(r, c.key);
                    if (c.edit === 'list') {
                        var s = '<select class="win-cell-edit" data-i="' + i + '" data-key="' + G.esc(c.key) + '"><option value="0"></option>';
                        (c.list || []).forEach(function (o) { s += '<option value="' + G.esc(o.Id) + '"' + (G.netI(o.Id) === G.netI(v) ? ' selected' : '') + '>' + G.esc(o.name) + '</option>'; });
                        h += '<td class="win-editable">' + s + '</select></td>';
                    } else if (c.edit) {
                        h += '<td class="win-editable"><input type="text" class="win-cell-edit' + (c.edit === 'num' ? ' num' : '') + '" data-i="' + i + '" data-key="' + G.esc(c.key) + '" value="' + G.esc(c.edit === 'num' ? G.str(v === null || v === undefined ? '' : v) : G.str(v)) + '" autocomplete="off"/></td>';
                    } else {
                        var txt = G.esc(display(c, v));
                        if (c.link && txt !== '') txt = '<a class="win-code" data-i="' + i + '" data-key="' + G.esc(c.key) + '" href="javascript:void(0)">' + txt + '</a>';
                        h += '<td' + (c.num ? ' class="num"' : '') + '>' + txt + '</td>';
                    }
                });
                h += '</tr>';
            });
            tb.innerHTML = h;
            g.drawTotals();
        };
        g.drawTotals = function () {
            var tf = t ? t.tFoot : null;
            if (!tf) return;
            if (!opts.totals || !opts.totals.length || !g.rows.length) { tf.innerHTML = ''; return; }
            var f = '<tr>';
            if (opts.withX) f += '<td></td>';
            if (opts.plus) f += '<td></td>';
            columns.forEach(function (c) {
                if (c.hidden) return;
                if (opts.totals.indexOf(c.key) >= 0) f += '<td class="num">&Sigma; ' + G.esc(G.fmt(g.total(c.key), c.dec === undefined ? 3 : c.dec)) + '</td>';
                else f += '<td></td>';
            });
            tf.innerHTML = f + '</tr>';
        };
        g.total = function (key) { var s = 0; g.rows.forEach(function (r) { s += G.netD(G.col(r, key)); }); return s; };
        g.setCurrent = function (i) {
            g.current = i;
            if (!t) return;
            t.querySelectorAll('tbody tr').forEach(function (tr) { if (G.netI(tr.getAttribute('data-i')) === i) tr.setAttribute('data-current', '1'); else tr.removeAttribute('data-current'); });
        };
        if (t) {
            t.addEventListener('change', function (e) {
                var el = e.target.closest('.win-cell-edit'); if (!el) return;
                var i = G.netI(el.getAttribute('data-i')), key = el.getAttribute('data-key');
                var c = columns.filter(function (x) { return x.key === key; })[0];
                var r = g.rows[i]; if (!r) return;
                r[key] = c && c.edit === 'num' ? G.netD(el.value) : (c && c.edit === 'list' ? G.netI(el.value) : el.value);
                g.setCurrent(i);
                if (opts.onCellUpdated) opts.onCellUpdated(i, key, r);
                g.drawTotals();
            });
            t.addEventListener('click', function (e) {
                var x = e.target.closest('button[data-del]');
                if (x) { e.preventDefault(); g.setCurrent(G.netI(x.getAttribute('data-del'))); if (opts.onDelete) opts.onDelete(G.netI(x.getAttribute('data-del'))); return; }
                var p = e.target.closest('button[data-plus]');
                if (p) { e.preventDefault(); if (opts.onPlus) opts.onPlus(G.netI(p.getAttribute('data-plus'))); return; }
                var a = e.target.closest('a.win-code');
                if (a) { e.preventDefault(); g.setCurrent(G.netI(a.getAttribute('data-i'))); if (opts.onLink) opts.onLink(G.netI(a.getAttribute('data-i')), a.getAttribute('data-key')); return; }
                var tr = e.target.closest('tbody tr');
                if (tr && !e.target.closest('.win-cell-edit')) { var i = G.netI(tr.getAttribute('data-i')); g.setCurrent(i); if (opts.onSelect) opts.onSelect(i); }
                else if (tr) g.setCurrent(G.netI(tr.getAttribute('data-i')));
            });
            t.addEventListener('dblclick', function (e) {
                if (e.target.closest('button') || e.target.closest('.win-cell-edit')) return;
                var tr = e.target.closest('tbody tr');
                if (tr && opts.onDblClick) opts.onDblClick(G.netI(tr.getAttribute('data-i')));
            });
        }
        return g;
    }

    /** A modal dialog (the desktop's ShowDialog loaders): open(), close(); the box is an existing element id. */
    function dialog(id) {
        var el = G.$id(id);
        return {
            open: function () { if (el) el.classList.remove('is-hidden'); },
            close: function () { if (el) el.classList.add('is-hidden'); },
            isOpen: function () { return !!el && !el.classList.contains('is-hidden'); }
        };
    }

    /** Footer History button: flips the Form / History tab and its own caption. */
    function footerToggle(btnId, tabs, formPanel, historyPanel) {
        var b = G.$id(btnId); if (!b) return;
        function label() {
            var onHist = tabs.current() === historyPanel;
            var s = b.querySelector('span'), i = b.querySelector('i');
            if (s) s.textContent = onHist ? 'Form' : 'History';
            if (i) i.className = onHist ? 'fa fa-file-text-o' : 'fa fa-history';
        }
        b.addEventListener('click', function () { tabs.select(tabs.current() === historyPanel ? formPanel : historyPanel); label(); });
        return label;
    }

    function print(key, args, btn) {
        if (!global.CrystalPrint) { G.box('Print is not available.'); return Promise.resolve(); }
        return global.CrystalPrint.open(key, args || {}, btn);
    }

    /** the Enter-moves-on of SendKeys.Send("{TAB}") for inputs and combos. */
    function enterAsTab(e) {
        if (e.key !== 'Enter' || e.ctrlKey || !e.target || !/^(INPUT|SELECT)$/.test(e.target.tagName) || e.target.type === 'checkbox') return false;
        if (e.target.closest && e.target.closest('.win-grid')) return false;
        var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled):not([readonly]), select:not(:disabled), textarea:not(:disabled)'), function (x) { return x.offsetParent !== null; });
        var i = f.indexOf(e.target);
        if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); return true; }
        return false;
    }

    /**
     * InfragisticsHelper.BindAndRetainSelection / DDL.BindDDLNew with all columns: options from the rows, the extra columns in
     * data-extra for the multi-column popup (countx_prod_combo.js), the previous value kept when it is still in the list.
     */
    function bindX(id, rows, valueCol, textCol, extraCols, keepValue) {
        var s = G.$id(id); if (!s) return;
        var keep = keepValue === undefined ? s.value : G.str(keepValue);
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) {
            var extra = (extraCols || []).map(function (k) { return G.str(G.col(r, k)).replace(/\|/g, '/'); }).join('|');
            h += '<option value="' + G.esc(G.col(r, valueCol)) + '"' + (extraCols && extraCols.length ? ' data-extra="' + G.esc(extra) + '"' : '') + '>' + G.esc(G.col(r, textCol)) + '</option>';
        });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.value = '0';
        G.refreshCombos();
    }

    /** .NET Double.ToString("#,##.###") / ("#,#.##"): zero prints as an empty string. */
    function fmtHash(v, dec) { var n = G.netD(v); return n === 0 ? '' : G.fmt(n, dec); }

    global.ExportSF = { editGrid: editGrid, bindX: bindX, dialog: dialog, footerToggle: footerToggle, print: print, enterAsTab: enterAsTab, fmtHash: fmtHash };
})(window);
