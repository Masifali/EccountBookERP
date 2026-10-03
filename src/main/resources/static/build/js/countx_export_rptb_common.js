/* ============================================================================================
 * countx_export_rptb_common.js - shared helpers of the Export Reports group B pages
 * (254, 261, 262, 266, 269, 270, 759, 912, 913, 919, 935 - Architecture.WinApp.ExportReports /
 * Audit_Dashboard / Service). Same helper names and contracts as the 881/882 reference files:
 * busy() button contract, getJson/postJson with the CSRF meta, bind/setVal/refreshCombos for the
 * searchable combos (countx_prod_combo.js), drawGrid with the desktop's column formats and Σ totals,
 * Enter -> next control, Ctrl+E / Esc -> close, Ctrl+Alt -> shortcut keys popup, CommonServices.DateType()
 * handling, a checked-list combo (UltraCombo CheckedListSettings) and the CrystalPrint bridge.
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
    function group(s) { var parts = s.split('.'); parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ','); return parts.join('.'); }
    /* "#,##0.00" - fixed decimals */
    function fmtFixed(v, dec) { var n = netD(v); var neg = n < 0; var s = group(Math.abs(n).toFixed(dec)); return (neg ? '-' : '') + s; }
    /* "#,##0.###" - optional decimals */
    function fmtOpt(v, dec) {
        var n = netD(v); var neg = n < 0; var s = Math.abs(n).toFixed(dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        return (neg ? '-' : '') + group(s);
    }
    /* "#,##0;(0,0);0" / "#,##0.###;(0,0.###);0" - negatives in parentheses, zero as 0 */
    function fmtParen(v, dec) {
        var n = netD(v);
        if (n === 0) return '0';
        var s = dec > 0 ? fmtOpt(Math.abs(n), dec) : group(Math.abs(n).toFixed(0));
        return n < 0 ? '(' + s + ')' : s;
    }
    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function ymd(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function today() { return ymd(new Date()); }
    function addDays(iso, n) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(str(iso)); if (!m) return ''; var d = new Date(+m[1], +m[2] - 1, +m[3]); d.setDate(d.getDate() + n); return ymd(d); }
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function isoDate(v) {
        var s = str(v).trim();
        if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(s);
        if (m) return m[1] + '-' + m[2] + '-' + m[3];
        var d = new Date(s);
        return isNaN(d.getTime()) ? '' : ymd(d);
    }
    /* FormatString "dd-MMM-yy" */
    function ddMMMyy(v) { var s = isoDate(v); if (!s) return ''; if (s.substring(0, 4) === '1900') return ''; return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(2, 4); }
    /* FormatString "MMM" (913 month column) */
    function mmm(v) { var s = isoDate(v); if (!s) return ''; return MON[parseInt(s.substring(5, 7), 10) - 1]; }
    /* DateTime.ToShortDateString() under the desktop's culture: M/d/yyyy. */
    function shortDate(v) { var s = isoDate(v); if (!s) return ''; return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4); }

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
     * DropDownBind.BindDDL / BindDDLNew: options from {Id, Name} rows (or the named columns); the previous
     * value is kept when it is still in the list (the desktop re-selects it after a refresh). The empty
     * first option is the combo with no active row (Text = string.Empty).
     */
    function bind(id, rows, valueCol, textCol) {
        var s = $id(id); if (!s) return;
        var keep = s.value;
        var h = '<option value="0"></option>';
        (rows || []).forEach(function (r) {
            h += '<option value="' + esc(col(r, valueCol || 'Id')) + '">' + esc(col(r, textCol || 'Name')) + '</option>';
        });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.value = '0';
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.value = '0'; refreshCombos(); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function selText(id) { var s = $id(id); if (!s || s.selectedIndex < 0) return ''; var o = s.options[s.selectedIndex]; return o ? o.text : ''; }
    function setText(id, v) { var e = $id(id); if (e) e.value = str(v); }
    function checked(id) { var e = $id(id); return !!(e && e.checked); }
    function radio(name) { var r = document.querySelector('input[name="' + name + '"]:checked'); return r ? r.value : ''; }
    function focus(id) { var e = $id(id); if (!e) return; var w = e.closest && e.closest('.dtcombo-wrap'); var t = w ? w.querySelector('.dtcombo-input') : e; try { (t || e).focus(); } catch (x) { /* ignore */ } }
    function cancel() {
        global.close();
        setTimeout(function () { if (!global.closed) { if (history.length > 1) history.back(); else location.href = '/export'; } }, 150);
    }
    function q(name) { var m = new RegExp('[?&]' + name + '=([^&]*)').exec(location.search); return m ? decodeURIComponent(m[1].replace(/\+/g, ' ')) : ''; }

    // ------------------------------------------------------------------------------ modal / shortcuts

    function modal(id, title, bodyHtml, wide) {
        var m = $id(id);
        if (!m) {
            m = document.createElement('div');
            m.id = id;
            m.className = 'rptb-modal';
            m.innerHTML = '<div class="rptb-modal-box' + (wide ? ' is-wide' : '') + '"><div class="pd-caption pd-caption-teal rptb-modal-head"><span class="rptb-modal-title"></span><button type="button" class="ex-fs-btn rptb-modal-close" title="Close">&times;</button></div><div class="rptb-modal-body"></div></div>';
            document.body.appendChild(m);
            m.querySelector('.rptb-modal-close').addEventListener('click', function () { m.classList.add('is-hidden'); });
            m.addEventListener('click', function (e) { if (e.target === m) m.classList.add('is-hidden'); });
        }
        m.querySelector('.rptb-modal-title').textContent = title;
        m.querySelector('.rptb-modal-body').innerHTML = bodyHtml;
        m.classList.remove('is-hidden');
        return m;
    }
    function closeModal(id) { var m = $id(id); if (m) m.classList.add('is-hidden'); }
    /* ShortCutKeyPopUp(dt) - KeyCombination / Description */
    function shortcuts(rows) {
        var h = '<table class="win-grid rptb-keys"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>';
        rows.forEach(function (r) { h += '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; });
        modal('rptbShortcuts', 'ShortCut Keys', h + '</tbody></table>');
    }

    // ------------------------------------------------------------------------------ grid drawer
    /* cols: [{key, caption, fmt: fn(v,row) | 'n2'|'n3'|'n4'|'o2'|'o3'|'p0'|'p3'|'d'|'dmy'|'mmm'|'i', num, sum, link, cls: fn(v,row), html: fn(v,row,i), hidden, ctr}]
       opts: {group: key, groupCaption, cur, rowClass: fn(row,i), total: bool} */
    var FMTS = {
        n2: function (v) { return fmtFixed(v, 2); }, n3: function (v) { return fmtFixed(v, 3); }, n4: function (v) { return fmtFixed(v, 4); },
        o2: function (v) { return fmtOpt(v, 2); }, o3: function (v) { return fmtOpt(v, 3); }, o0: function (v) { return fmtOpt(v, 0); },
        p0: function (v) { return fmtParen(v, 0); }, p3: function (v) { return fmtParen(v, 3); },
        d: shortDate, dmy: ddMMMyy, mmm: mmm, i: function (v) { return str(netI(v)); }
    };
    function cellFmt(c, v, row) {
        var f = c.fmt;
        if (typeof f === 'function') return f(v, row);
        if (f && FMTS[f]) return FMTS[f](v);
        return str(v);
    }
    function drawGrid(tableId, cols, rows, opts) {
        opts = opts || {};
        var table = $id(tableId); if (!table) return;
        var thead = table.querySelector('thead'), tbody = table.querySelector('tbody'), tfoot = table.querySelector('tfoot');
        var vis = cols.filter(function (c) { return !c.hidden; });
        if (thead) thead.innerHTML = '<tr>' + vis.map(function (c) { return '<th' + (c.num ? ' class="num"' : (c.ctr ? ' class="ctr"' : '')) + ' data-col="' + esc(c.key) + '"' + (c.width ? ' style="min-width:' + c.width + 'px"' : '') + '>' + esc(c.caption || c.key) + '</th>'; }).join('') + '</tr>';
        var sums = {};
        var html = '';
        var groupKey = opts.group, lastGroup = null, gsum = {};
        function totalRow(cls, label, s) {
            var t = '<tr class="' + cls + '">';
            vis.forEach(function (c, idx) {
                var v = '';
                if (c.sum) v = cellFmt(c, s[c.key] || 0, null);
                else if (idx === 0) v = label;
                t += '<td' + (c.num ? ' class="num"' : ' class="lbl"') + '>' + esc(v) + '</td>';
            });
            return t + '</tr>';
        }
        rows.forEach(function (r, i) {
            if (groupKey) {
                var g = str(col(r, groupKey));
                if (g !== lastGroup) {
                    if (lastGroup !== null && opts.groupTotals) html += totalRow('rptb-group-total', 'Σ', gsum);
                    html += '<tr class="rptb-group"><td colspan="' + vis.length + '">' + esc(opts.groupCaption || groupKey) + ': ' + esc(g) + '</td></tr>';
                    lastGroup = g; gsum = {};
                }
            }
            var rc = opts.rowClass ? (opts.rowClass(r, i) || '') : '';
            html += '<tr data-i="' + i + '" class="' + (i === opts.cur ? 'is-current ' : '') + rc + '">';
            vis.forEach(function (c) {
                var v = col(r, c.key);
                if (c.sum) { sums[c.key] = (sums[c.key] || 0) + netD(v); gsum[c.key] = (gsum[c.key] || 0) + netD(v); }
                var cls = (c.num ? 'num ' : (c.ctr ? 'ctr ' : '')) + (c.cls ? (c.cls(v, r) || '') : '');
                var inner;
                if (c.html) inner = c.html(v, r, i);
                else if (c.link) { var txt = cellFmt(c, v, r); inner = txt === '' ? '' : '<a class="win-code" href="javascript:void(0)" data-link="' + esc(c.key) + '" data-i="' + i + '">' + esc(txt) + '</a>'; }
                else inner = esc(cellFmt(c, v, r));
                html += '<td' + (cls.trim() ? ' class="' + cls.trim() + '"' : '') + '>' + inner + '</td>';
            });
            html += '</tr>';
        });
        if (groupKey && lastGroup !== null && opts.groupTotals) html += totalRow('rptb-group-total', 'Σ', gsum);
        tbody.innerHTML = html;
        if (tfoot) {
            var hasTotal = vis.some(function (c) { return c.sum; });
            tfoot.innerHTML = rows.length && hasTotal && opts.total !== false ? totalRow('', 'Σ', sums) : '';
        }
    }
    function clearGrid(tableId) { var t = $id(tableId); if (!t) return; var b = t.querySelector('tbody'), f = t.querySelector('tfoot'); if (b) b.innerHTML = ''; if (f) f.innerHTML = ''; }
    /* row selection + delegated link / button clicks on a grid */
    function gridEvents(tableId, handlers) {
        var t = $id(tableId); if (!t) return;
        t.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            t.querySelectorAll('tbody tr[data-i]').forEach(function (x) { x.classList.toggle('is-current', x === tr); });
            var i = +tr.getAttribute('data-i');
            if (handlers.select) handlers.select(i);
            var lk = e.target.closest('a[data-link]');
            if (lk && handlers.link) { e.preventDefault(); handlers.link(lk.getAttribute('data-link'), i, lk); return; }
            var bt = e.target.closest('button[data-btn]');
            if (bt && handlers.button) { handlers.button(bt.getAttribute('data-btn'), i, bt); }
        });
        if (handlers.dblclick) t.addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr) handlers.dblclick(+tr.getAttribute('data-i')); });
        /* Ctrl+Space on a focused grid = the desktop "call the button / link of the current row" */
        if (handlers.ctrlSpace) t.addEventListener('keydown', function (e) {
            if (!(e.ctrlKey && e.code === 'Space')) return;
            var tr = t.querySelector('tbody tr.is-current'); if (!tr) return;
            e.preventDefault(); handlers.ctrlSpace(+tr.getAttribute('data-i'));
        });
    }
    function focusGrid(tableId) { var t = $id(tableId); if (!t) return; t.setAttribute('tabindex', '0'); var tr = t.querySelector('tbody tr.is-current') || t.querySelector('tbody tr[data-i]'); if (tr) tr.classList.add('is-current'); t.focus(); }
    function fullscreenButtons() {
        document.querySelectorAll('button[data-fullscreen]').forEach(function (b) {
            b.addEventListener('click', function () { var bx = $id(b.getAttribute('data-fullscreen')); if (bx) bx.classList.toggle('ex-fullscreen'); });
        });
    }
    /* Enter -> next control (SendKeys {TAB}), Ctrl+E / Esc -> Close, Ctrl+Alt -> shortcut keys. Returns true when handled. */
    function baseKeys(e, extra) {
        if (e.key === 'Enter' && !e.ctrlKey && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox' && e.target.type !== 'radio') {
            var f = Array.prototype.filter.call(document.querySelectorAll('input:not([type=hidden]):not(:disabled), select:not(:disabled), button:not(:disabled)'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(e.target);
            if (i >= 0 && i + 1 < f.length) { e.preventDefault(); f[i + 1].focus(); }
            return true;
        }
        var k = (e.key || '').toLowerCase();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { e.preventDefault(); cancel(); return true; }
        if (e.ctrlKey && e.altKey && extra) { e.preventDefault(); extra(); return true; }
        return false;
    }
    /* cmbperemeter_ValueChanged / cmbdatetype_ValueChanged - CommonServices.DateType() 1..5. `both` = the
       forwarding forms, which also set the To date on 1, 2 and 5. */
    function dateTypeChanged(id, fromId, toId, yearStart, both) {
        var v = netI(val(id)), now = new Date();
        if (v === 1) { setText(fromId, today()); if (both) setText(toId, today()); }
        else if (v === 2) { setText(fromId, addDays(today(), -7)); if (both) setText(toId, today()); }
        else if (v === 3) { setText(fromId, ymd(new Date(now.getFullYear(), now.getMonth(), 1))); setText(toId, today()); }
        else if (v === 4) { setText(fromId, now.getFullYear() + '-01-01'); setText(toId, today()); }
        else if (v === 5) { setText(fromId, isoDate(yearStart) || ''); if (both) setText(toId, today()); }
    }
    /** Reporting.ShowReportWithDataTable -> /api/reports/{key}/print.pdf (ReportRegistry key, args). */
    function print(key, args, btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return Promise.resolve(); }
        return global.CrystalPrint.open(key, args || {}, btn || null);
    }

    // ------------------------------------------------------------------------------ checked-list combo
    /**
     * UltraCombo with CheckedListSettings (254 Document Status / Current Status): a text box showing the
     * checked names joined by "," and a popup with a filter row, a check-all header and one checkbox per row.
     * getIds(hostId) returns the checked ids joined by ",", getText the caption text, clear() empties it.
     */
    function checkList(hostId, rows, caption) {
        var host = $id(hostId); if (!host) return;
        host.classList.add('rptb-check');
        var prev = {};
        host.querySelectorAll('input[type=checkbox][data-id]').forEach(function (c) { if (c.checked) prev[c.getAttribute('data-id')] = true; });
        var h = '<input type="text" class="win-textbox rptb-check-text" readonly placeholder="' + esc(caption || '') + '"/>'
            + '<div class="rptb-check-pop is-hidden"><div class="rptb-check-filter"><input type="text" class="win-textbox" placeholder="Filter..."/></div>'
            + '<div class="rptb-check-list"><label class="rptb-check-all"><input type="checkbox" data-all="1"/> <b>' + esc(caption || '') + '</b></label>';
        (rows || []).forEach(function (r) {
            var id = str(col(r, 'Id'));
            h += '<label><input type="checkbox" data-id="' + esc(id) + '"' + (prev[id] ? ' checked' : '') + '/> <span>' + esc(col(r, 'Name')) + '</span></label>';
        });
        host.innerHTML = h + '</div></div>';
        var text = host.querySelector('.rptb-check-text'), pop = host.querySelector('.rptb-check-pop'), filter = host.querySelector('.rptb-check-filter input');
        function sync() {
            var names = [];
            host.querySelectorAll('input[data-id]').forEach(function (c) { if (c.checked) names.push(c.nextElementSibling.textContent); });
            text.value = names.join(',');
        }
        text.addEventListener('click', function () { pop.classList.toggle('is-hidden'); if (!pop.classList.contains('is-hidden')) filter.focus(); });
        text.addEventListener('keydown', function (e) { if (e.key === 'ArrowDown' || e.key === 'F4') { pop.classList.remove('is-hidden'); filter.focus(); e.preventDefault(); } });
        filter.addEventListener('input', function () {
            var f = filter.value.toLowerCase();
            host.querySelectorAll('label:not(.rptb-check-all)').forEach(function (l) { l.classList.toggle('is-hidden', f !== '' && l.textContent.toLowerCase().indexOf(f) < 0); });
        });
        host.querySelector('input[data-all]').addEventListener('change', function (e) {
            host.querySelectorAll('label:not(.is-hidden) input[data-id]').forEach(function (c) { c.checked = e.target.checked; });
            sync();
        });
        host.addEventListener('change', function (e) { if (e.target.hasAttribute('data-id')) sync(); });
        document.addEventListener('click', function (e) { if (!host.contains(e.target)) pop.classList.add('is-hidden'); });
        sync();
    }
    function checkListIds(hostId) { var ids = []; var host = $id(hostId); if (host) host.querySelectorAll('input[data-id]').forEach(function (c) { if (c.checked) ids.push(c.getAttribute('data-id')); }); return ids.join(','); }
    function checkListText(hostId) { var host = $id(hostId); var t = host && host.querySelector('.rptb-check-text'); return t ? t.value : ''; }
    function checkListClear(hostId) { var host = $id(hostId); if (!host) return; host.querySelectorAll('input[type=checkbox]').forEach(function (c) { c.checked = false; }); var t = host.querySelector('.rptb-check-text'); if (t) t.value = ''; }

    // ------------------------------------------------------------------------------ popups shared with group A
    /** frmContractDetailByContractId (ContractNo links) - the same popup data the group-A pages read. */
    function contractDetail(contractId, screenId) {
        if (!contractId) return Promise.resolve();
        var m = modal('rptbContractDetail', 'Sale Contract Detail', '<div class="rptb-kv" id="rptbCdHeader">Loading...</div>'
            + '<div class="pd-caption pd-caption-teal" style="height:24px;">Item Information</div><div class="ex-grid-scroll ex-h240"><table class="win-grid" id="rptbCdItems"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>'
            + '<div class="pd-caption pd-caption-teal" style="height:24px;">Invoice Information</div><div class="ex-grid-scroll ex-h240"><table class="win-grid" id="rptbCdInvoices"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>'
            + '<div class="pd-caption pd-caption-teal" style="height:24px;">Schedule Information</div><div class="ex-grid-scroll ex-h240"><table class="win-grid" id="rptbCdSchedules"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>', true);
        return getJson('/api/export/reports-a/contract-detail?contractId=' + contractId + '&screenId=' + (screenId || 759)).then(function (d) {
            var h = (d && d.header) || {};
            var f2 = function (v) { return fmtFixed(v, 2); };
            $id('rptbCdHeader').innerHTML =
                '<div><b>Contract No &amp; Date:</b> ' + esc(h.LcOrderNo || 'No.') + ' &nbsp; ' + esc(shortDate(h.LcOrderDate) || 'Date') + '</div>'
                + '<div><b>Customer Name:</b> ' + esc(h.CustomerName || 'Customer') + '</div>'
                + '<div><b>Shipment Last Date:</b> ' + esc(shortDate(h.LastShipmentDate) || 'Date') + '</div>'
                + '<div><b>Contract M.Tons:</b> ' + f2(h.lcMtons) + '</div><div><b>Shipped M.Tons:</b> ' + f2(h.InvoiceMtons) + '</div>'
                + '<div><b>Balance M.Tons:</b> ' + f2(netD(h.lcMtons) - netD(h.InvoiceMtons)) + '</div>'
                + '<div><b>Fcy Code &amp; Amount:</b> ' + esc(h.CurrencyCode || 'FcyCode') + ' &nbsp; ' + f2(h.LcFcyAmount) + '</div>'
                + '<div><b>Shipped Amount:</b> ' + f2(h.InvoiceValue) + '</div><div><b>Balance Amount:</b> ' + f2(netD(h.LcFcyAmount) - netD(h.InvoiceValue)) + '</div>';
            drawGrid('rptbCdItems', [{ key: 'ItemName', caption: 'Item Name' }, { key: 'TotalMTons', caption: 'Total MTons', num: true, sum: true, fmt: 'n3' },
                { key: 'ShippedMTons', caption: 'Shipped MTons', num: true, sum: true, fmt: 'n3' }, { key: 'BalanceMTons', caption: 'Balance MTons', num: true, sum: true, fmt: 'n3' }], d.items || [], {});
            drawGrid('rptbCdInvoices', [{ key: 'DocType', caption: 'Doc Type' }, { key: 'InvoiceNo', caption: 'Invoice No' }, { key: 'InvoiceDate', caption: 'Invoice Date', fmt: 'd' },
                { key: 'BolDate', caption: 'Bol Date', fmt: 'd' }, { key: 'CustomerContractNo', caption: 'Customer Contract No' }, { key: 'MTons', caption: 'MTons', num: true, sum: true, fmt: 'n3' },
                { key: 'PaymentTerm', caption: 'Payment Term' }, { key: 'DueDate', caption: 'Due Date', fmt: 'd' }, { key: 'ReceivedDate', caption: 'Received Date', fmt: 'd' },
                { key: 'Amount', caption: 'Amount', num: true, sum: true, fmt: 'n2' }, { key: 'Received', caption: 'Received', num: true, sum: true, fmt: 'n2' }, { key: 'Balance', caption: 'Balance', num: true, sum: true, fmt: 'n2' }], d.invoices || [], {});
            drawGrid('rptbCdSchedules', [{ key: 'ScheduleCode', caption: 'Schedule Code' }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'CustomerContractNo', caption: 'Customer Contract No' },
                { key: 'LoadingDate', caption: 'Loading Date', fmt: 'd' }, { key: 'ItemName', caption: 'Item Name' }, { key: 'PackSize', caption: 'Pack Size' },
                { key: 'NoOfBags', caption: 'No Of Bags', num: true, sum: true, fmt: 'o3' }, { key: 'MTon', caption: 'MTon', num: true, sum: true, fmt: 'n3' },
                { key: 'ShippedMTon', caption: 'Shipped MTon', num: true, sum: true, fmt: 'n3' }, { key: 'BalanceMTon', caption: 'Balance MTon', num: true, sum: true, fmt: 'n3' },
                { key: 'NoOfAttachments', caption: 'No Of Attachments', num: true }, { key: 'DestinationPort', caption: 'Destination Port' }], d.schedules || [], {});
        }).catch(function (e) { var hd = $id('rptbCdHeader'); if (hd) hd.innerHTML = '<span class="rptb-red">' + esc(e.message) + '</span>'; else box(e.message); void m; });
    }
    /** CommonServices.GetNoofAttachmentsByRefDocumentTypeID - the DMS AttachmentView has no web counterpart yet. */
    function attachments(id, documentTypeId) {
        box('Attachments of record ' + id + ' (document type ' + documentTypeId + ') - the attachment viewer is not available on the web port.');
    }

    global.ExportRptB = {
        $id: $id, box: box, ask: ask, esc: esc, str: str, netI: netI, netD: netD, fmtFixed: fmtFixed, fmtOpt: fmtOpt, fmtParen: fmtParen,
        today: today, addDays: addDays, isoDate: isoDate, ddMMMyy: ddMMMyy, mmm: mmm, shortDate: shortDate, ymd: ymd,
        busy: busy, getJson: getJson, postJson: postJson, refreshCombos: refreshCombos, show: show, col: col, bind: bind, setVal: setVal, val: val,
        selText: selText, setText: setText, checked: checked, radio: radio, focus: focus, cancel: cancel, q: q, modal: modal, closeModal: closeModal, shortcuts: shortcuts,
        drawGrid: drawGrid, clearGrid: clearGrid, gridEvents: gridEvents, focusGrid: focusGrid, fullscreenButtons: fullscreenButtons,
        baseKeys: baseKeys, dateTypeChanged: dateTypeChanged, print: print, FMTS: FMTS,
        checkList: checkList, checkListIds: checkListIds, checkListText: checkListText, checkListClear: checkListClear,
        contractDetail: contractDetail, attachments: attachments
    };
}(window));
