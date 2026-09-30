/* ============================================================================================
 * Shared helpers for the Accounts Transaction screens 15 Day Book, 24 Day Book (Off Set),
 * 861 / 863 Invoices Adjustment Vouchers and 885 Bank Reconciliation With Vouchers.
 *
 * Only formatting and plumbing live here; every screen keeps its own desktop logic in its own
 * file. Number formats follow CommonServices.GetDecimalConfiguration:
 *   stringFormatsingle  "#,##0.<n>"                  (n = Default NoofDecimal Points For Amount)
 *   stringFormatboth    "#,##0.<n>;(0,0.<n>); 0"     negative in brackets, zero as " 0"
 *   DecimalRateFormate  "#,#0.<r>"                   (r = Default NoofDecimal Points For Rate, 0 -> 2)
 * ============================================================================================ */
(function (global) {
    'use strict';

    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    function num(v) {
        if (v === null || v === undefined || v === '') return 0;
        if (typeof v === 'number') return isFinite(v) ? v : 0;
        var n = parseFloat(String(v).replace(/,/g, ''));
        return isFinite(n) ? n : 0;
    }

    function group(n, dec) {
        return Math.abs(n).toLocaleString('en-US', { minimumFractionDigits: dec, maximumFractionDigits: dec });
    }

    function amountDigits(cfg) {
        var d = parseInt(cfg, 10);
        return (d >= 1 && d <= 4) ? d : 0;
    }

    var GD = {
        amountDecimals: 0,
        rateDecimals: 2,

        num: num,

        setDecimals: function (amount, rate) {
            GD.amountDecimals = amountDigits(amount);
            var r = parseInt(rate, 10);
            GD.rateDecimals = (r >= 1 && r <= 4) ? r : 2;
        },

        /** stringFormatsingle */
        fmtSingle: function (v) {
            var n = num(v);
            return (n < 0 ? '-' : '') + group(n, GD.amountDecimals);
        },

        /** stringFormatboth */
        fmtBoth: function (v) {
            var n = num(v);
            if (n === 0) return ' 0';
            return n < 0 ? '(' + group(n, GD.amountDecimals) + ')' : group(n, GD.amountDecimals);
        },

        /** DecimalRateFormate */
        fmtRate: function (v) {
            var n = num(v);
            return (n < 0 ? '-' : '') + group(n, GD.rateDecimals);
        },

        /** "#,##0.###" */
        fmt3: function (v) {
            var n = num(v);
            return (n < 0 ? '-' : '') + Math.abs(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 });
        },

        /** the Dr / Cr / Nill caption beside a balance */
        drCr: function (v) {
            var n = num(v);
            return n > 0 ? 'Dr' : (n < 0 ? 'Cr' : 'Nill');
        },

        toDate: function (v) {
            if (!v) return null;
            if (v instanceof Date) return v;
            var s = String(v).trim();
            var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(s);
            if (m) return new Date(+m[1], +m[2] - 1, +m[3], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0));
            var d = new Date(s);
            return isNaN(d.getTime()) ? null : d;
        },

        /** yyyy-MM-dd */
        iso: function (v) {
            var d = GD.toDate(v);
            if (!d) return '';
            return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
        },

        /** yyyy-MM-dd HH:mm:ss */
        isoTime: function (v) {
            var d = GD.toDate(v);
            if (!d) return '';
            return GD.iso(d) + ' ' + String(d.getHours()).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ':' + String(d.getSeconds()).padStart(2, '0');
        },

        /** .NET custom date formats used by these grids: dd-MMM-yy, dd-MMM-yyyy, dd-MM-yyyy hh:mm tt ... */
        fmtDate: function (v, pattern) {
            var d = GD.toDate(v);
            if (!d) return '';
            var h12 = d.getHours() % 12 === 0 ? 12 : d.getHours() % 12;
            var map = {
                'yyyy': String(d.getFullYear()),
                'yy': String(d.getFullYear()).slice(-2),
                'MMM': MONTHS[d.getMonth()],
                'MM': String(d.getMonth() + 1).padStart(2, '0'),
                'dd': String(d.getDate()).padStart(2, '0'),
                'hh': String(h12).padStart(2, '0'),
                'mm': String(d.getMinutes()).padStart(2, '0'),
                'tt': d.getHours() < 12 ? 'AM' : 'PM'
            };
            return (pattern || 'dd-MMM-yyyy').replace(/yyyy|yy|MMM|MM|dd|hh|mm|tt/g, function (t) { return map[t]; });
        },

        esc: function (v) {
            return String(v === undefined || v === null ? '' : v)
                .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
        },

        /** case-insensitive column read, as a DataRow lookup is */
        col: function (row, name) {
            if (!row) return undefined;
            if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
            var low = name.toLowerCase();
            for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === low) return row[k];
            return undefined;
        },

        /**
         * fetch() wrapper. Resolves with the JSON body; rejects with {status, message, confirm}.
         */
        api: function (method, url, body) {
            var opt = { method: method, headers: { 'Accept': 'application/json' }, credentials: 'same-origin' };
            if (body !== undefined) {
                opt.headers['Content-Type'] = 'application/json';
                opt.body = JSON.stringify(body);
            }
            return fetch(url, opt).then(function (res) {
                return res.text().then(function (t) {
                    var j = null;
                    try { j = t ? JSON.parse(t) : null; } catch (e) { j = null; }
                    if (res.ok) return j;
                    throw { status: res.status, message: (j && j.message) || t || ('HTTP ' + res.status), confirm: j && j.confirm };
                });
            });
        },

        get: function (url, params) {
            var q = [];
            if (params) for (var k in params) {
                if (params[k] === undefined || params[k] === null || params[k] === '') continue;
                q.push(encodeURIComponent(k) + '=' + encodeURIComponent(params[k]));
            }
            return GD.api('GET', url + (q.length ? (url.indexOf('?') >= 0 ? '&' : '?') + q.join('&') : ''));
        },

        /**
         * Fills a <select>. rows: the procedure rows; value / text: column names; extra: {data-attr: column}.
         * blank: text of a leading empty option, or null for none.
         */
        fill: function (sel, rows, value, text, extra, blank) {
            if (typeof sel === 'string') sel = document.getElementById(sel);
            if (!sel) return;
            var h = blank === null || blank === undefined ? '' : '<option value="">' + GD.esc(blank) + '</option>';
            (rows || []).forEach(function (r) {
                var attrs = '';
                if (extra) for (var a in extra) attrs += ' data-' + a + '="' + GD.esc(GD.col(r, extra[a])) + '"';
                h += '<option value="' + GD.esc(GD.col(r, value)) + '"' + attrs + '>' + GD.esc(GD.col(r, text)) + '</option>';
            });
            sel.innerHTML = h;
        },

        selData: function (sel, attr) {
            if (typeof sel === 'string') sel = document.getElementById(sel);
            if (!sel || sel.selectedIndex < 0) return undefined;
            var o = sel.options[sel.selectedIndex];
            return o ? o.getAttribute('data-' + attr) : undefined;
        },

        /**
         * The browser rendering of a desktop print: the procedure's own rows in a table. The
         * Crystal layout itself is not reproduced.
         */
        printRows: function (title, rows) {
            if (!rows || rows.length === 0) { alert('No Record Found For Display'); return; }
            var cols = Object.keys(rows[0]).filter(function (c) {
                var v = rows[0][c];
                return !(typeof v === 'string' && v.length > 2000);
            });
            var h = '<!doctype html><html><head><meta charset="utf-8"><title>' + GD.esc(title) + '</title>'
                + '<style>body{font-family:Segoe UI,Arial,sans-serif;font-size:11px;margin:12px}table{border-collapse:collapse;width:100%}'
                + 'th,td{border:1px solid #999;padding:3px 5px;text-align:left}th{background:#eee}</style></head><body>'
                + '<h3>' + GD.esc(title) + '</h3><table><thead><tr>';
            cols.forEach(function (c) { h += '<th>' + GD.esc(c) + '</th>'; });
            h += '</tr></thead><tbody>';
            rows.forEach(function (r) {
                h += '<tr>';
                cols.forEach(function (c) { h += '<td>' + GD.esc(r[c]) + '</td>'; });
                h += '</tr>';
            });
            h += '</tbody></table><script>window.print();<\/script></body></html>';
            var w = window.open('', '_blank');
            if (!w) { alert('Allow pop-ups to print.'); return; }
            w.document.open(); w.document.write(h); w.document.close();
        }
    };

    global.GD = GD;

    if (global.DesktopCombo && global.DesktopCombo.define) {
        /* DatatableHelper.GetAccountsFromGlobalByTypeIds -> Id | AccountTitle | AccountCode |
           ParentAccountTitle | AccountClass, bound AllColumns with only the value column hidden. */
        global.DesktopCombo.define('gdAccount', [
            { caption: 'AccountTitle', flex: 4 },
            { caption: 'AccountCode', flex: 2, key: 'code' },
            { caption: 'ParentAccountTitle', flex: 3, key: 'parent' },
            { caption: 'AccountClass', flex: 2, key: 'class' }
        ]);
        /* USP_GetSubsidiaryAccountsByParentAccount with columns 2,3,4 hidden (DayBook.cs):
           SubsidiaryAccount | Code | AccountTitle. */
        global.DesktopCombo.define('gdSubsidiary', [
            { caption: 'Subsidiary Account', flex: 4 },
            { caption: 'Code', flex: 2, key: 'code' },
            { caption: 'AccountTitle', flex: 3, key: 'account' }
        ]);
        /* frmDayBook.BindAllSubsidiaryaccounts hides only columns 2 and 4:
           SubsidiaryAccount | SubsidiaryType | Code | AccountTitle. */
        global.DesktopCombo.define('gdSubsidiary4', [
            { caption: 'Account Title', flex: 4 },
            { caption: 'SubsidiaryType', flex: 2, key: 'type' },
            { caption: 'Code', flex: 2, key: 'code' },
            { caption: 'AccountTitle', flex: 3, key: 'account' }
        ]);
    }
}(window));
