/* ============================================================================================
 * StoreRptB - the grid / format pieces the three Store report pages of group B share
 * (454 Stock Adjustment Report, 455 Department Request History, 456 Stock Transfer Report).
 *
 *   StoreRptB.fmt(v, pattern)         .NET custom numeric format ("#,##0.###", "#,#", "#,#0.00" ...),
 *                                     rounding away from zero as .NET custom formats do
 *   StoreRptB.caption(key)            the caption GridEX.RetrieveStructure gives a column
 *                                     ("NoOfAttachments" -> "No Of Attachments", "UOMCode" -> "UOM Code")
 *   StoreRptB.dMMMyyyy / dMMMyy / dMMMyyyyhm / general   date cells
 *   StoreRptB.render(table, cols, rows, opts)   header, body and the GridEX TotalRow
 *   StoreRptB.printRows(rows, title)  the rows the desktop pushes into its .rpt, as a table
 *
 * Nothing here decides business rules; each page's own file carries those with its desktop refs.
 * Uses StoreCommon (countx_store_common.js) for escaping.
 * ============================================================================================ */
(function (global) {
    'use strict';

    var C = global.StoreCommon;
    function esc(s) { return C.esc(s); }

    function toNum(v) {
        if (v === null || v === undefined || v === '') return NaN;
        return typeof v === 'number' ? v : parseFloat(String(v).replace(/,/g, ''));
    }
    function group(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /** Math.round away from zero at `dec` places. */
    function roundAway(n, dec) {
        var f = Math.pow(10, dec);
        var r = Math.round(Math.abs(n) * f + 1e-9) / f;
        return n < 0 ? -r : r;
    }

    /**
     * .NET custom numeric format for the patterns these forms use: optional grouping, a minimum
     * number of integer digits ('0' count before the point), minimum ('0') and maximum ('0' + '#')
     * decimals after it. "#,#" of 0 prints "" as on the desktop.
     */
    function fmt(v, pattern) {
        var n = toNum(v);
        if (isNaN(n)) return v === null || v === undefined ? '' : String(v);
        var p = String(pattern || '');
        var dot = p.indexOf('.');
        var ip = dot >= 0 ? p.slice(0, dot) : p;
        var dp = dot >= 0 ? p.slice(dot + 1) : '';
        var minInt = (ip.match(/0/g) || []).length;
        var grouping = ip.indexOf(',') >= 0;
        var minDec = (dp.match(/0/g) || []).length;
        var maxDec = minDec + (dp.match(/#/g) || []).length;
        var r = roundAway(n, maxDec);
        var s = Math.abs(r).toFixed(maxDec);
        var parts = s.split('.');
        var intPart = parts[0], dec = parts[1] || '';
        while (dec.length > minDec && dec.charAt(dec.length - 1) === '0') dec = dec.slice(0, -1);
        intPart = intPart.replace(/^0+/, '');
        while (intPart.length < minInt) intPart = '0' + intPart;
        if (grouping) intPart = group(intPart);
        var out = intPart + (dec ? '.' + dec : '');
        if (out === '') return '';
        return (r < 0 ? '-' : '') + out;
    }

    /** RetrieveStructure caption: a space before each word start inside a CamelCase name. */
    function caption(key) {
        return String(key)
            .replace(/([a-z0-9])([A-Z])/g, '$1 $2')
            .replace(/([A-Z])([A-Z][a-z])/g, '$1 $2');
    }

    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function parts(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(String(v || ''));
        return m ? { y: m[1], M: +m[2], d: m[3], h: m[4] === undefined ? 0 : +m[4], mi: m[5] || '00' } : null;
    }
    function hm(p) {
        var h = p.h % 12; if (h === 0) h = 12;
        return String(h).padStart(2, '0') + ':' + p.mi + ' ' + (p.h >= 12 ? 'PM' : 'AM');
    }
    function dMMMyyyy(v) { var p = parts(v); return p ? p.d + '-' + MON[p.M - 1] + '-' + p.y : (v == null ? '' : String(v)); }
    function dMMMyy(v) { var p = parts(v); return p ? p.d + '-' + MON[p.M - 1] + '-' + p.y.slice(2) : (v == null ? '' : String(v)); }
    function dMMMyyyyhm(v) { var p = parts(v); return p ? dMMMyyyy(v) + ' ' + hm(p) : (v == null ? '' : String(v)); }
    /** An unformatted DateTime cell: the date, plus the time when it is not midnight. */
    function general(v) {
        var p = parts(v);
        if (!p) return v == null ? '' : String(v);
        return (p.h === 0 && p.mi === '00') ? dMMMyyyy(v) : dMMMyyyyhm(v);
    }

    function cell(r, key) { return C.ci(r, key); }

    /**
     * cols: [{ key, caption, format(v,row) -> text, align: 'right'|'center', agg: 'sum'|'avg',
     *          total(v) -> text, button: 'Print', onButton(row), link: fn(row) }]
     * opts: { frozen: n (sticky leading columns), empty: text }
     */
    function render(table, cols, rows, opts) {
        if (typeof table === 'string') table = C.$id(table);
        opts = opts || {};
        var frozen = opts.frozen || 0;
        var head = '<tr>' + cols.map(function (c, i) {
            return '<th' + (i < frozen ? ' class="rb-frozen"' : '') + '>' + esc(c.caption) + '</th>';
        }).join('') + '</tr>';
        var body = rows.map(function (r, ri) {
            return '<tr data-i="' + ri + '">' + cols.map(function (c, i) {
                var cls = [];
                if (i < frozen) cls.push('rb-frozen');
                if (c.align === 'right') cls.push('num');
                if (c.align === 'center') cls.push('rb-center');
                var inner;
                if (c.button) inner = '<button type="button" class="win-btn-small" data-b="' + i + '">' + esc(c.button) + '</button>';
                else {
                    var raw = cell(r, c.key);
                    var t = c.format ? c.format(raw, r) : (raw === null || raw === undefined ? '' : String(raw));
                    inner = c.link ? '<a href="#" class="cx-link" data-l="' + i + '">' + esc(t) + '</a>' : esc(t);
                }
                return '<td' + (cls.length ? ' class="' + cls.join(' ') + '"' : '') + '>' + inner + '</td>';
            }).join('') + '</tr>';
        }).join('');
        var foot = '';
        if (rows.length && cols.some(function (c) { return c.agg; })) {
            foot = '<tr class="rb-total">' + cols.map(function (c, i) {
                var cls = (i < frozen ? 'rb-frozen ' : '') + (c.agg ? 'num' : '');
                if (!c.agg) return '<td class="' + cls + '"></td>';
                var s = 0;
                rows.forEach(function (r) { var n = toNum(cell(r, c.key)); if (!isNaN(n)) s += n; });
                if (c.agg === 'avg') s = s / rows.length;
                return '<td class="' + cls + '">' + esc(c.total ? c.total(s) : String(s)) + '</td>';
            }).join('') + '</tr>';
        }
        table.innerHTML = '<thead>' + head + '</thead><tbody>' + body + '</tbody><tfoot>' + foot + '</tfoot>';
        table.querySelectorAll('button[data-b]').forEach(function (b) {
            b.onclick = function () {
                var c = cols[+b.getAttribute('data-b')];
                var r = rows[+b.closest('tr').getAttribute('data-i')];
                if (c.onButton) c.onButton(r, b);
            };
        });
        table.querySelectorAll('a[data-l]').forEach(function (a) {
            a.onclick = function (e) {
                e.preventDefault();
                var c = cols[+a.getAttribute('data-l')];
                c.link(rows[+a.closest('tr').getAttribute('data-i')]);
            };
        });
    }

    function clear(table) {
        if (typeof table === 'string') table = C.$id(table);
        table.innerHTML = '<thead></thead><tbody></tbody><tfoot></tfoot>';
    }

    /**
     * The rows the desktop hands to ShowReportWithDataTable, printed as a plain table. The Crystal
     * layout itself is not reproduced (see the service Javadoc).
     */
    function printRows(rows, title) {
        if (!rows || !rows.length) return false;
        var cols = Object.keys(rows[0]);
        var w = global.open('', '_blank');
        if (!w) { global.alert('Allow pop-ups to print.'); return true; }
        var h = '<html><head><title>' + esc(title) + '</title><style>body{font-family:Verdana;font-size:10px}' +
            'table{border-collapse:collapse}td,th{border:1px solid #444;padding:2px 4px;white-space:nowrap}</style></head><body>' +
            '<h3>' + esc(title) + '</h3><table><thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') +
            '</tr></thead><tbody>' + rows.map(function (r) {
                return '<tr>' + cols.map(function (c) { return '<td>' + esc(r[c]) + '</td>'; }).join('') + '</tr>';
            }).join('') + '</tbody></table><script>window.print()<\/script></body></html>';
        w.document.write(h);
        w.document.close();
        return true;
    }

    /**
     * A desktop DateTimePicker (no ShowCheckBox) always holds a date, so its filter is always sent.
     * A browser date input can be cleared; this puts the last value back when that happens.
     */
    function guardDate(id) {
        var el = C.$id(id);
        if (!el || el.getAttribute('data-guard')) return;
        el.setAttribute('data-guard', '1');
        var last = el.value;
        var keep = function () { if (el.value) last = el.value; else if (last) el.value = last; };
        el.addEventListener('change', keep);
        el.addEventListener('blur', keep);
        el.addEventListener('focus', function () { if (el.value) last = el.value; });
    }

    /**
     * BindDDL(..., ZeroIndex: true) (DropDownBind.cs:48-93): a "...Select Any Value..." row with
     * value 0 is inserted first (unless the list already has one) and DDL.Value = 0 - the previous
     * selection is not kept.
     */
    function bindZeroIndex(id, rows, valueKey, textKey) {
        var el = C.$id(id);
        if (!el) return;
        var list = rows || [];
        var h = list.some(function (r) { return String(C.ci(r, textKey)) === '...Select Any Value...'; }) ? ''
            : '<option value="0">...Select Any Value...</option>';
        list.forEach(function (r) { h += '<option value="' + esc(C.ci(r, valueKey)) + '">' + esc(C.ci(r, textKey)) + '</option>'; });
        el.innerHTML = h;
        el.value = '0';
    }

    /** combo.Text = string.Empty + ActiveRow = null: nothing shown, Value null (ToInt -> 0). */
    function clearCombo(id) {
        var el = C.$id(id);
        if (!el) return;
        var blank = el.querySelector('option[data-blank]');
        if (!blank) {
            el.insertAdjacentHTML('afterbegin', '<option value="" data-blank="1" hidden></option>');
        }
        el.value = '';
    }

    global.StoreRptB = {
        fmt: fmt, caption: caption, dMMMyyyy: dMMMyyyy, dMMMyy: dMMMyy, dMMMyyyyhm: dMMMyyyyhm,
        general: general, render: render, clear: clear, printRows: printRows, toNum: toNum,
        guardDate: guardDate, bindZeroIndex: bindZeroIndex, clearCombo: clearCombo
    };
}(window));
