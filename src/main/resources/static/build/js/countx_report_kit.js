/* ============================================================================================
 * ReportKit - the pieces every desktop report form shares, written once.
 *
 *   ReportKit.num(v, dec)            "#,##0.##"-style: dec = max decimals (trailing zeros dropped)
 *   ReportKit.fixed(v, dec)          "#,##0.00"-style: exactly dec decimals
 *   ReportKit.both(v, dec)           clsGlobalVariables.stringFormatboth: "#,##0.xx;(0,0.xx); 0"
 *   ReportKit.pad2(v)                .NET "0,0": thousands, whole number, at least two digits
 *   ReportKit.dMMMyy(v) / dMMMyyyy(v) / dmyhm(v)   dd-MMM-yy, dd-MMM-yyyy, dd-MM-yyyy hh:mm tt
 *   ReportKit.enterToTab()           KeyDown "Enter -> SendKeys {TAB}" (installed once)
 *   ReportKit.keys(map)              Ctrl/Alt shortcut table: { 'ctrl+s': fn, 'esc': fn, ... }
 *   ReportKit.shortcuts(rows)        the desktop "ShortCut Keys" form: [[keys, description], ...]
 *   ReportKit.filterRow(table)       GridEX FilterMode.Automatic - a Contains row under the header
 *   ReportKit.close()                Close() - back to the Production hub
 * ============================================================================================ */
(function (global) {
    'use strict';

    function toNum(v) {
        if (v === null || v === undefined || v === '') return NaN;
        var n = typeof v === 'number' ? v : parseFloat(String(v).replace(/,/g, ''));
        return n;
    }
    function group(intPart) { return intPart.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }

    function num(v, dec) {
        var n = toNum(v);
        if (isNaN(n)) return v === null || v === undefined ? '' : String(v);
        var s = Math.abs(n).toFixed(dec === undefined ? 2 : dec);
        if (s.indexOf('.') >= 0) s = s.replace(/0+$/, '').replace(/\.$/, '');
        var p = s.split('.');
        return (n < 0 ? '-' : '') + group(p[0]) + (p[1] ? '.' + p[1] : '');
    }
    function fixed(v, dec) {
        var n = toNum(v);
        if (isNaN(n)) return v === null || v === undefined ? '' : String(v);
        var p = Math.abs(n).toFixed(dec).split('.');
        return (n < 0 ? '-' : '') + group(p[0]) + (p[1] ? '.' + p[1] : '');
    }
    function both(v, dec) {
        var n = toNum(v);
        if (isNaN(n)) return v === null || v === undefined ? '' : String(v);
        if (Math.abs(n) < Math.pow(10, -(dec || 0)) / 2) return ' 0';
        var s = fixed(Math.abs(n), dec || 0);
        return n < 0 ? '(' + s + ')' : s;
    }
    function pad2(v) {
        var n = toNum(v);
        if (isNaN(n)) return v === null || v === undefined ? '' : String(v);
        var r = Math.round(Math.abs(n));
        var s = r < 10 ? '0' + r : group(String(r));
        return (n < 0 ? '-' : '') + s;
    }

    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function parts(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(String(v || ''));
        return m ? { y: m[1], M: m[2], d: m[3], h: m[4], mi: m[5] } : null;
    }
    function dMMMyy(v)   { var p = parts(v); return p ? p.d + '-' + MON[+p.M - 1] + '-' + p.y.slice(2) : (v == null ? '' : String(v)); }
    function dMMMyyyy(v) { var p = parts(v); return p ? p.d + '-' + MON[+p.M - 1] + '-' + p.y : (v == null ? '' : String(v)); }
    function dmyhm(v) {
        var p = parts(v);
        if (!p) return v == null ? '' : String(v);
        var h = +(p.h || 0), h12 = h % 12 === 0 ? 12 : h % 12;
        return p.d + '-' + p.M + '-' + p.y + ' ' + String(h12).padStart(2, '0') + ':' + (p.mi || '00') + ' ' + (h < 12 ? 'AM' : 'PM');
    }

    function visibleFocusable() {
        return Array.prototype.filter.call(
            document.querySelectorAll('input, select, textarea, button, a[href], [tabindex="0"]'),
            function (el) { return !el.disabled && el.tabIndex >= 0 && el.offsetParent !== null; });
    }
    var enterInstalled = false;
    function enterToTab() {
        if (enterInstalled) return;
        enterInstalled = true;
        document.addEventListener('keydown', function (e) {
            if (e.key !== 'Enter' || e.ctrlKey || e.altKey || e.defaultPrevented) return;
            var t = e.target;
            if (!t || t.tagName === 'BUTTON' || t.tagName === 'TEXTAREA' || t.tagName === 'A') return;
            if (t.closest && t.closest('.rk-modal')) return;
            e.preventDefault();
            var all = visibleFocusable(), i = all.indexOf(t);
            if (i >= 0 && i + 1 < all.length) all[i + 1].focus();
        });
    }

    function keys(map) {
        document.addEventListener('keydown', function (e) {
            if (e.defaultPrevented) return;
            var k = e.key === 'Escape' ? 'esc' : (e.key.length === 1 ? e.key.toLowerCase() : e.key.toLowerCase());
            var combo = (e.ctrlKey || e.metaKey ? 'ctrl+' : '') + (e.altKey ? 'alt+' : '') + (e.shiftKey && k.length > 1 ? 'shift+' : '') + k;
            /* "Ctrl+Alt" on its own - the desktop opens its shortcut list on that chord. */
            if ((e.ctrlKey && e.key === 'Alt') || (e.altKey && e.key === 'Control')) combo = 'ctrl+alt';
            var fn = map[combo];
            if (!fn) return;
            if (combo === 'esc' && document.querySelector('.rk-modal.is-open, .is-fullscreen, .jo-full')) return;
            e.preventDefault();
            fn(e);
        });
    }

    var modal = null;
    function shortcuts(rows) {
        if (!modal) {
            modal = document.createElement('div');
            modal.className = 'rk-modal';
            modal.style.cssText = 'position:fixed;inset:0;background:rgba(0,0,0,.35);z-index:9500;display:none;';
            modal.innerHTML = '<div style="position:absolute;top:10%;left:50%;transform:translateX(-50%);width:min(520px,94vw);background:#fff;border:1px solid #555;box-shadow:0 6px 24px rgba(0,0,0,.4);font:12px Segoe UI,sans-serif;">'
                + '<div style="background:#008080;color:#fff;height:28px;display:flex;align-items:center;justify-content:space-between;padding:0 10px;font-weight:bold;">ShortCut Keys'
                + '<button type="button" data-rk-close style="background:none;border:none;color:#fff;font-size:16px;cursor:pointer;">&times;</button></div>'
                + '<div style="max-height:60vh;overflow:auto;padding:6px;"><table style="width:100%;border-collapse:collapse;"><thead><tr>'
                + '<th style="border:1px solid #bbb;background:#eee;padding:3px 6px;text-align:left;">KeyCombination</th>'
                + '<th style="border:1px solid #bbb;background:#eee;padding:3px 6px;text-align:left;">Description</th></tr></thead><tbody></tbody></table></div></div>';
            document.body.appendChild(modal);
            modal.addEventListener('click', function (e) {
                if (e.target === modal || e.target.hasAttribute('data-rk-close')) close_();
            });
            document.addEventListener('keydown', function (e) { if (e.key === 'Escape' && modal.classList.contains('is-open')) close_(); });
        }
        modal.querySelector('tbody').innerHTML = rows.map(function (r) {
            return '<tr><td style="border:1px solid #ddd;padding:3px 6px;">' + esc(r[0]) + '</td><td style="border:1px solid #ddd;padding:3px 6px;">' + esc(r[1]) + '</td></tr>';
        }).join('');
        modal.classList.add('is-open');
        modal.style.display = 'block';
        function close_() { modal.classList.remove('is-open'); modal.style.display = 'none'; }
    }

    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }

    /** A Contains filter row under the header of a rendered table. Re-call after each render. */
    function filterRow(table) {
        if (!table || !table.tHead || !table.tHead.rows.length) return;
        var head = table.tHead, cols = head.rows[0].cells.length;
        var old = head.querySelector('tr.rk-filter');
        if (old) old.remove();
        var tr = document.createElement('tr');
        tr.className = 'rk-filter';
        for (var i = 0; i < cols; i++) {
            var th = document.createElement('th');
            th.style.cssText = 'padding:1px;background:#fff;position:sticky;top:24px;z-index:4;';
            var inp = document.createElement('input');
            inp.type = 'text';
            inp.setAttribute('data-col', i);
            inp.style.cssText = 'width:100%;min-width:40px;height:19px;font-size:8pt;border:1px solid #c8c8c8;padding:0 3px;';
            th.appendChild(inp);
            tr.appendChild(th);
        }
        head.appendChild(tr);
        tr.addEventListener('input', function () {
            var f = Array.prototype.map.call(tr.querySelectorAll('input'), function (x) { return x.value.trim().toLowerCase(); });
            Array.prototype.forEach.call(table.tBodies[0] ? table.tBodies[0].rows : [], function (row) {
                if (row.classList.contains('rk-keep')) return;
                var ok = true;
                for (var c = 0; c < f.length && ok; c++) {
                    if (!f[c]) continue;
                    var cell = row.cells[c];
                    ok = !!cell && cell.textContent.toLowerCase().indexOf(f[c]) >= 0;
                }
                row.style.display = ok ? '' : 'none';
            });
        });
    }

    function close() { global.location.href = '/production'; }

    global.ReportKit = { num: num, fixed: fixed, both: both, pad2: pad2, dMMMyy: dMMMyy, dMMMyyyy: dMMMyyyy,
                         dmyhm: dmyhm, enterToTab: enterToTab, keys: keys, shortcuts: shortcuts,
                         filterRow: filterRow, close: close, esc: esc };
}(window));
