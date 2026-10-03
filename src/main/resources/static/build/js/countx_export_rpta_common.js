/* ============================================================================================
 * countx_export_rpta_common.js - helpers shared by the Export Reports group A pages
 * (235, 236, 238, 239, 249, 250, 251). Exposes window.ExportRptA: the button contract (busy),
 * JSON calls with CSRF, combo binding, number/date formats of the desktop GridEX FormatStrings,
 * a grid drawer with per-column formats, grouping (GridEX Groups.Add) and Σ totals, the
 * fullscreen toggle, the ShortCutKeyPopUp, CommonServices.DateType() -> cmbperemeter_ValueChanged,
 * and the frmContractDetailByContractId popup (usp_ExportContractDetailById).
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
    function netI(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isFinite(n) ? n : 0; }
    function netD(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isFinite(n) ? n : 0; }
    /* "#,##0.00" style: fixed decimals. */
    function fmtFixed(v, dec) {
        var n = netD(v);
        var s = Math.abs(n).toFixed(dec);
        var parts = s.split('.');
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        return (n < 0 ? '-' : '') + parts.join('.');
    }
    /* "#,##0.##" style: up to dec decimals, trailing zeros dropped. */
    function fmtOpt(v, dec) {
        var s = fmtFixed(v, dec);
        if (dec > 0) s = s.replace(/\.?0+$/, '');
        return s;
    }
    /* "#,#" / "#,#.###" style: zero prints as "" (the desktop shows blank for 0). */
    function fmtHash(v, dec) {
        var n = netD(v);
        if (n === 0) return '';
        return fmtOpt(n, dec);
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
    /* "dd-MMM-yy" */
    function ddMMMyy(v) { var s = isoDate(v); if (!s) return ''; return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(2, 4); }
    /* "dd-MMM-yyyy" */
    function ddMMMyyyy(v) { var s = isoDate(v); if (!s) return ''; return s.substring(8, 10) + '-' + MON[parseInt(s.substring(5, 7), 10) - 1] + '-' + s.substring(0, 4); }
    /* GridEX DateTime default: short date M/d/yyyy */
    function shortDate(v) { var s = isoDate(v); if (!s) return ''; return parseInt(s.substring(5, 7), 10) + '/' + parseInt(s.substring(8, 10), 10) + '/' + s.substring(0, 4); }
    /* "dd-MMM-yy hh:mm tt" */
    function ddMMMyyHm(v) {
        var s = str(v).trim(); if (!s) return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(s);
        if (!m) return ddMMMyy(s);
        var h = parseInt(m[4], 10), tt = h >= 12 ? 'PM' : 'AM'; h = h % 12; if (h === 0) h = 12;
        return m[3] + '-' + MON[parseInt(m[2], 10) - 1] + '-' + m[1].substring(2) + ' ' + pad(h) + ':' + m[5] + ' ' + tt;
    }
    /* Button contract: disabled + spinner while running, duplicates ignored, re-enabled on success and failure. */
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
    /* DDL.BindDDL / BindDDLNew: ZeroIndex adds a blank first row; the selection is kept when it still exists. */
    function bind(id, rows, valueCol, textCol, zeroIndex) {
        var s = $id(id); if (!s) return;
        var keep = s.value;
        var h = zeroIndex === false ? '' : '<option value="0"></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + esc(col(r, valueCol)) + '">' + esc(col(r, textCol)) + '</option>'; });
        s.innerHTML = h;
        s.value = keep;
        if (s.value !== keep) s.selectedIndex = 0;
        refreshCombos();
    }
    function setVal(id, v) { var s = $id(id); if (!s) return; s.value = str(v); if (s.value !== str(v)) s.selectedIndex = 0; refreshCombos(); }
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
    function shortcuts(rows) { box(rows.map(function (r) { return r[0] + '  ' + r[1]; }).join('\n')); }

    /* ------------------------------------------------------------------ grid drawer
     * cols: [{key, caption, fmt: fn(v,row) | 'n2'|'n3'|'n4'|'o2'|'o3'|'h0'|'h2'|'h3'|'d'|'dmy'|'dmyy'|'dt', num, sum, avg,
     *        link: fn(row,i) -> href|true, cls: fn(v,row) -> css class, html: fn(v,row,i) -> cell html, hidden}]
     * opts: {group: key, groupTotals: bool, cur: index, rowClass: fn(row,i), total: bool} */
    var FMTS = {
        n2: function (v) { return fmtFixed(v, 2); }, n3: function (v) { return fmtFixed(v, 3); }, n4: function (v) { return fmtFixed(v, 4); },
        o2: function (v) { return fmtOpt(v, 2); }, o3: function (v) { return fmtOpt(v, 3); }, o4: function (v) { return fmtOpt(v, 4); },
        h0: function (v) { return fmtHash(v, 0); }, h2: function (v) { return fmtHash(v, 2); }, h3: function (v) { return fmtHash(v, 3); }, h4: function (v) { return fmtHash(v, 4); },
        d: shortDate, dmy: ddMMMyy, dmyy: ddMMMyyyy, dt: ddMMMyyHm, i: function (v) { return str(netI(v)); }
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
        if (thead) thead.innerHTML = '<tr>' + vis.map(function (c) { return '<th' + (c.num ? ' class="num"' : '') + ' data-col="' + esc(c.key) + '"' + (c.width ? ' style="min-width:' + c.width + 'px"' : '') + '>' + esc(c.caption || c.key) + '</th>'; }).join('') + '</tr>';
        var sums = {}, cnt = {};
        var html = '';
        var groupKey = opts.group, lastGroup = null, gsum = {}, gcnt = 0;
        function totalRow(cls, label, s, n) {
            var t = '<tr class="' + cls + '">';
            vis.forEach(function (c, idx) {
                var v = '';
                if (c.sum) v = cellFmt(c, s[c.key] || 0, null);
                else if (c.avg) v = cellFmt(c, n ? (s[c.key] || 0) / n : 0, null);
                else if (idx === 0) v = label;
                t += '<td' + (c.num ? ' class="num"' : ' class="lbl"') + '>' + esc(v) + '</td>';
            });
            return t + '</tr>';
        }
        rows.forEach(function (r, i) {
            if (groupKey) {
                var g = str(col(r, groupKey));
                if (g !== lastGroup) {
                    if (lastGroup !== null && opts.groupTotals) html += totalRow('rpta-group-total', 'Σ', gsum, gcnt);
                    html += '<tr class="rpta-group"><td colspan="' + vis.length + '">' + esc(groupKey) + ': ' + esc(g) + '</td></tr>';
                    lastGroup = g; gsum = {}; gcnt = 0;
                }
                gcnt++;
            }
            var rc = opts.rowClass ? (opts.rowClass(r, i) || '') : '';
            html += '<tr data-i="' + i + '" class="' + (i === opts.cur ? 'is-current ' : '') + rc + '">';
            vis.forEach(function (c) {
                var v = col(r, c.key);
                if (c.sum || c.avg) { sums[c.key] = (sums[c.key] || 0) + netD(v); cnt[c.key] = (cnt[c.key] || 0) + 1; gsum[c.key] = (gsum[c.key] || 0) + netD(v); }
                var cls = (c.num ? 'num ' : '') + (c.cls ? (c.cls(v, r) || '') : '');
                var inner;
                if (c.html) inner = c.html(v, r, i);
                else if (c.link) inner = '<a class="win-code" href="javascript:void(0)" data-link="' + esc(c.key) + '" data-i="' + i + '">' + esc(cellFmt(c, v, r)) + '</a>';
                else inner = esc(cellFmt(c, v, r));
                html += '<td' + (cls.trim() ? ' class="' + cls.trim() + '"' : '') + '>' + inner + '</td>';
            });
            html += '</tr>';
        });
        if (groupKey && lastGroup !== null && opts.groupTotals) html += totalRow('rpta-group-total', 'Σ', gsum, gcnt);
        tbody.innerHTML = html;
        if (tfoot) {
            var hasTotal = vis.some(function (c) { return c.sum || c.avg; });
            tfoot.innerHTML = rows.length && hasTotal && opts.total !== false ? totalRow('', 'Σ', sums, rows.length) : '';
        }
    }
    function clearGrid(tableId) { var t = $id(tableId); if (!t) return; var b = t.querySelector('tbody'), f = t.querySelector('tfoot'); if (b) b.innerHTML = ''; if (f) f.innerHTML = ''; }
    /* row selection + delegated link / button clicks on a grid body */
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
        /* Ctrl+Space on a focused grid row = the desktop "call the button / link of the current column" */
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
    /* Enter -> next control (SendKeys {TAB}), Ctrl+E / Esc -> Close, Ctrl+Alt -> shortcuts. */
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
    /* cmbperemeter_ValueChanged / cmbdatetype_ValueChanged - CommonServices.DateType() 1..5 */
    function dateTypeChanged(id, fromId, toId, yearStart) {
        var v = netI(val(id)), now = new Date();
        if (v === 1) setText(fromId, today());
        else if (v === 2) setText(fromId, addDays(today(), -7));
        else if (v === 3) { setText(fromId, ymd(new Date(now.getFullYear(), now.getMonth(), 1))); setText(toId, today()); }
        else if (v === 4) { setText(fromId, now.getFullYear() + '-01-01'); setText(toId, today()); }
        else if (v === 5) setText(fromId, yearStart || '');
    }
    function print(key, args, btn) {
        if (!global.CrystalPrint) { box('Printing is not available on this page.'); return Promise.resolve(); }
        return global.CrystalPrint.open(key, args || {}, btn || null);
    }

    /* ------------------------------------------------------------------ frmContractDetailByContractId popup */
    function contractDetail(contractId, screenId) {
        if (!contractId) return;
        var m = $id('rptaContractDetail');
        if (!m) {
            m = document.createElement('div');
            m.id = 'rptaContractDetail';
            m.className = 'rpta-modal is-hidden';
            m.innerHTML = '<div class="rpta-modal-box">'
                + '<div class="pd-caption pd-caption-teal" style="height:28px;justify-content:space-between;"><span>Sale Contract Detail</span><button type="button" class="ex-fs-btn" id="rptaCdClose">&times;</button></div>'
                + '<div class="rpta-kv" id="rptaCdHeader"></div>'
                + '<div class="pd-caption pd-caption-teal" style="height:24px;">Item Information</div><div class="ex-grid-scroll ex-h240"><table class="win-grid" id="rptaCdItems"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>'
                + '<div class="pd-caption pd-caption-teal" style="height:24px;">Invoice Information</div><div class="ex-grid-scroll ex-h240"><table class="win-grid" id="rptaCdInvoices"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>'
                + '<div class="pd-caption pd-caption-teal" style="height:24px;">Schedule Information</div><div class="ex-grid-scroll ex-h240"><table class="win-grid" id="rptaCdSchedules"><thead></thead><tbody></tbody><tfoot></tfoot></table></div>'
                + '</div>';
            document.body.appendChild(m);
            $id('rptaCdClose').addEventListener('click', function () { m.classList.add('is-hidden'); });
            m.addEventListener('click', function (e) { if (e.target === m) m.classList.add('is-hidden'); });
        }
        m.classList.remove('is-hidden');
        $id('rptaCdHeader').innerHTML = '<span>Loading...</span>';
        return getJson('/api/export/reports-a/contract-detail?contractId=' + contractId + '&screenId=' + (screenId || 251)).then(function (d) {
            var h = (d && d.header) || {};
            var f2 = function (v) { return fmtFixed(v, 2); };
            $id('rptaCdHeader').innerHTML =
                '<div><b>Contract No &amp; Date:</b> ' + esc(h.LcOrderNo || 'No.') + ' &nbsp; ' + esc(shortDate(h.LcOrderDate) || 'Date') + '</div>'
                + '<div><b>Customer Name:</b> ' + esc(h.CustomerName || 'Customer') + '</div>'
                + '<div><b>Shipment Last Date:</b> ' + esc(shortDate(h.LastShipmentDate) || 'Date') + '</div>'
                + '<div><b>Contract M.Tons:</b> ' + f2(h.lcMtons) + '</div>'
                + '<div><b>Shipped M.Tons:</b> ' + f2(h.InvoiceMtons) + '</div>'
                + '<div><b>Balance M.Tons:</b> ' + f2(netD(h.lcMtons) - netD(h.InvoiceMtons)) + '</div>'
                + '<div><b>Fcy Code &amp; Amount:</b> ' + esc(h.CurrencyCode || 'FcyCode') + ' &nbsp; ' + f2(h.LcFcyAmount) + '</div>'
                + '<div><b>Shipped Amount:</b> ' + f2(h.InvoiceValue) + '</div>'
                + '<div><b>Balance Amount:</b> ' + f2(netD(h.LcFcyAmount) - netD(h.InvoiceValue)) + '</div>';
            drawGrid('rptaCdItems', [
                { key: 'ItemName', caption: 'Item Name' }, { key: 'TotalMTons', caption: 'Total MTons', num: true, sum: true, fmt: 'n3' },
                { key: 'ShippedMTons', caption: 'Shipped MTons', num: true, sum: true, fmt: 'n3' }, { key: 'BalanceMTons', caption: 'Balance MTons', num: true, sum: true, fmt: 'n3' }
            ], d.items || [], {});
            drawGrid('rptaCdInvoices', [
                { key: 'DocType', caption: 'Doc Type' }, { key: 'InvoiceNo', caption: 'Invoice No' }, { key: 'InvoiceDate', caption: 'Invoice Date', fmt: 'd' },
                { key: 'BolDate', caption: 'Bol Date', fmt: 'd' }, { key: 'CustomerContractNo', caption: 'Customer Contract No' }, { key: 'MTons', caption: 'MTons', num: true, sum: true, fmt: 'n3' },
                { key: 'PaymentTerm', caption: 'Payment Term' }, { key: 'DueDate', caption: 'Due Date', fmt: 'd' }, { key: 'ReceivedDate', caption: 'Received Date', fmt: 'd' },
                { key: 'Amount', caption: 'Amount', num: true, sum: true, fmt: 'n2' }, { key: 'Received', caption: 'Received', num: true, sum: true, fmt: 'n2' }, { key: 'Balance', caption: 'Balance', num: true, sum: true, fmt: 'n2' }
            ], d.invoices || [], {});
            drawGrid('rptaCdSchedules', [
                { key: 'ScheduleCode', caption: 'Schedule Code' }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'CustomerContractNo', caption: 'Customer Contract No' },
                { key: 'LoadingDate', caption: 'Loading Date', fmt: 'd' }, { key: 'ItemName', caption: 'Item Name' }, { key: 'PackSize', caption: 'Pack Size' },
                { key: 'NoOfBags', caption: 'No Of Bags', num: true, sum: true, fmt: 'o3' }, { key: 'MTon', caption: 'MTon', num: true, sum: true, fmt: 'n3' },
                { key: 'ShippedMTon', caption: 'Shipped MTon', num: true, sum: true, fmt: 'n3' }, { key: 'BalanceMTon', caption: 'Balance MTon', num: true, sum: true, fmt: 'n3' },
                { key: 'NoOfAttachments', caption: 'No Of Attachments', num: true }, { key: 'DestinationPort', caption: 'Destination Port' }
            ], d.schedules || [], {});
        }).catch(function (e) { $id('rptaCdHeader').innerHTML = '<span class="red">' + esc(e.message) + '</span>'; });
    }

    global.ExportRptA = {
        $id: $id, box: box, ask: ask, esc: esc, str: str, netI: netI, netD: netD, fmtFixed: fmtFixed, fmtOpt: fmtOpt, fmtHash: fmtHash,
        today: today, addDays: addDays, isoDate: isoDate, ddMMMyy: ddMMMyy, ddMMMyyyy: ddMMMyyyy, shortDate: shortDate, ddMMMyyHm: ddMMMyyHm,
        busy: busy, getJson: getJson, postJson: postJson, refreshCombos: refreshCombos, show: show, col: col, bind: bind, setVal: setVal, val: val,
        selText: selText, setText: setText, checked: checked, radio: radio, focus: focus, cancel: cancel, shortcuts: shortcuts,
        drawGrid: drawGrid, clearGrid: clearGrid, gridEvents: gridEvents, focusGrid: focusGrid, fullscreenButtons: fullscreenButtons,
        baseKeys: baseKeys, dateTypeChanged: dateTypeChanged, print: print, contractDetail: contractDetail, FMTS: FMTS
    };
}(window));
