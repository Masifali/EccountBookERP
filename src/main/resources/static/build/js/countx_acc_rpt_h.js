/* ============================================================================================
 * Shared helpers for the Account Reports ported 2026-09-30 (group H; look unified with
 * countx_acc_rpt_f.css by R5 the same day - pages are body.accf, this file only does data/formats):
 *   62 Balance Sheet, 64 Profit & Loss 01, 84 BS and PL Breakup, 958 Monthly Profit Loss,
 *   886 Commission Agent Report, 911 Freight Voucher Report.
 * No jQuery. Number formats reproduce the desktop's .NET custom format strings.
 * ============================================================================================ */
(function (global) {
    'use strict';

    function $(id) { return document.getElementById(id); }
    function esc(s) {
        return String(s == null ? '' : s).replace(/[&<>"']/g, function (c) {
            return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
        });
    }
    function csrf() {
        var t = document.querySelector('meta[name="_csrf"]'), h = document.querySelector('meta[name="_csrf_header"]');
        var o = {};
        if (t && h && t.content && h.content) o[h.content] = t.content;
        return o;
    }
    /* {success,data} | {success:false,message} */
    function api(method, url, body) {
        var headers = Object.assign({ 'Accept': 'application/json' }, csrf());
        var init = { method: method, headers: headers, credentials: 'same-origin' };
        if (body !== undefined) { headers['Content-Type'] = 'application/json'; init.body = JSON.stringify(body); }
        return fetch(url, init).then(function (r) {
            return r.text().then(function (t) {
                var j = null;
                try { j = t ? JSON.parse(t) : null; } catch (e) { j = null; }
                if (!r.ok || !j || j.success === false) throw new Error((j && j.message) || ('Request failed (' + r.status + ')'));
                return j.data !== undefined ? j.data : j;
            });
        });
    }

    /* ------------------------------------------------------------------ .NET number formats */
    function toNum(v) { if (v == null || v === '') return 0; var n = Number(v); return isNaN(n) ? 0 : n; }
    function group(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    /* core: abs value, minInt integer digits, fixed decimals fd, then up to od optional decimals */
    function core(a, minInt, fd, od, grp) {
        var total = fd + od;
        var s = a.toFixed(total);
        var ip = s.split('.')[0], dp = s.split('.')[1] || '';
        if (od > 0) { var keep = fd; for (var i = dp.length - 1; i >= fd; i--) { if (dp[i] !== '0') { keep = i + 1; break; } } dp = dp.substring(0, keep); }
        while (ip.length < minInt) ip = '0' + ip;
        if (minInt === 0 && ip === '0') ip = '';
        if (grp) ip = group(ip);
        return dp.length ? ip + '.' + dp : ip;
    }
    /* stringFormatboth: "#,##0.<d zeros>;(0,0.<d zeros>); 0" */
    function both(v, dec) {
        var n = toNum(v); var r = Math.pow(10, dec || 0);
        if (Math.round(Math.abs(n) * r) === 0) return ' 0';
        return n < 0 ? '(' + core(-n, 2, dec || 0, 0, true) + ')' : core(n, 1, dec || 0, 0, true);
    }
    /* "#,##0.<d #>;(0,0.<d #>); 0" (CommonServices.GridColumnSettings, NegativeFormat) */
    function bothOpt(v, dec) {
        var n = toNum(v); var r = Math.pow(10, dec || 0);
        if (Math.round(Math.abs(n) * r) === 0) return ' 0';
        return n < 0 ? '(' + core(-n, 2, 0, dec, true) + ')' : core(n, 1, 0, dec, true);
    }
    /* "#,##0.<d #>" */
    function opt(v, dec) { var n = toNum(v); var s = core(Math.abs(n), 1, 0, dec, true); return (n < 0 && s.replace(/[0,.]/g, '') !== '' ? '-' : '') + s; }
    /* "#,##0.<d 0>" (stringFormatsingle) */
    function fixed(v, dec) { var n = toNum(v); var s = core(Math.abs(n), 1, dec, 0, true); return (n < 0 && s.replace(/[0,.]/g, '') !== '' ? '-' : '') + s; }
    /* "0,0" */
    function zz(v) { var n = toNum(v); var s = core(Math.abs(n), 2, 0, 0, true); return (n < 0 && s.replace(/[0,.]/g, '') !== '' ? '-' : '') + s; }
    /* DecimalRateFormate "#,#0.<d 0>" */
    function rate(v, dec) { return fixed(v, dec); }

    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function dparts(v) {
        if (typeof v === 'number') { var d = new Date(v); v = d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2) + 'T' + ('0' + d.getHours()).slice(-2) + ':' + ('0' + d.getMinutes()).slice(-2); }
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(String(v == null ? '' : v)); return m; }
    function shortDate(v) { var m = dparts(v); return m ? m[3] + '/' + m[2] + '/' + m[1] : (v == null ? '' : String(v)); }
    function dMMMyyyy(v) { var m = dparts(v); return m ? m[3] + '-' + MON[+m[2] - 1] + '-' + m[1] : (v == null ? '' : String(v)); }
    function dMMMyyyyhm(v) {
        var m = dparts(v); if (!m) return v == null ? '' : String(v);
        var hh = +(m[4] || 0), ap = hh >= 12 ? 'PM' : 'AM'; var h12 = hh % 12 || 12;
        return m[3] + '-' + MON[+m[2] - 1] + '-' + m[1] + ' ' + (h12 < 10 ? '0' : '') + h12 + ':' + (m[5] || '00') + ' ' + ap;
    }
    function today() { var d = new Date(); return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2); }
    function ci(r, k) {
        if (!r) return null;
        if (k in r) return r[k];
        var lk = k.toLowerCase();
        for (var p in r) if (p.toLowerCase() === lk) return r[p];
        return null;
    }

    /* ------------------------------------------------------------------ message box */
    function box(msg) {
        var m = document.createElement('div');
        m.className = 'h-modal';
        m.innerHTML = '<div class="h-dlg"><div class="cap">EccountBook</div><div class="msg"></div><div class="btns"><button type="button">OK</button></div></div>';
        m.querySelector('.msg').textContent = msg;
        document.body.appendChild(m);
        var b = m.querySelector('button'); b.focus();
        b.addEventListener('click', function () { m.remove(); });
    }
    function shortcuts(rows) {
        box(rows.map(function (r) { return r[0] + '    ' + r[1]; }).join('\n'));
    }

    /* ------------------------------------------------------------------ branch combo
     * BranchFill: UltraCombo bound Id/BranchName. With features 17+18 it is a checked list whose Text
     * (ticked names joined by ",") is what GetBranchesIdsByFeature reads; with 17 only it is a single
     * pick. Text is preset to UserAccount.BranchName. */
    function branchCombo(host, ctx) {
        var multi = !!(ctx.branchFeature && ctx.branchFeatureConsolidated);
        var list = ctx.branches || [];
        host.innerHTML = '';
        if (!multi) {
            var sel = document.createElement('select');
            /* searchable single pick (countx_prod_combo.js enhances select[data-dtcombo]) */
            sel.className = 'inp'; sel.style.width = '100%';
            sel.setAttribute('data-dtcombo', 'single'); sel.setAttribute('data-dtcombo-caption', 'Branch Name');
            sel.innerHTML = '<option value="0"></option>' + list.map(function (b) {
                return '<option value="' + esc(b.Id) + '">' + esc(b.BranchName) + '</option>';
            }).join('');
            host.appendChild(sel);
            list.forEach(function (b) { if (String(b.BranchName) === String(ctx.userBranchName)) sel.value = String(b.Id); });
            return { text: function () { return sel.options[sel.selectedIndex] ? sel.options[sel.selectedIndex].text : ''; },
                     id: function () { return +sel.value || 0; } };
        }
        var wrap = document.createElement('div'); wrap.className = 'h-bc';
        wrap.innerHTML = '<input type="text" class="inp bc-text" autocomplete="off"><button type="button" class="bc-drop" tabindex="-1">&#9662;</button><div class="bc-pop" style="display:none"></div>';
        host.appendChild(wrap);
        var txt = wrap.querySelector('input'), pop = wrap.querySelector('.bc-pop');
        txt.value = list.length ? (ctx.userBranchName || '') : '';
        function names() { return txt.value.split(',').filter(function (s) { return s !== ''; }); }
        function draw() {
            var cur = names();
            pop.innerHTML = '<table><tr><th></th><th>Branch Name</th></tr>' + list.map(function (b, i) {
                return '<tr data-i="' + i + '"><td><input type="checkbox"' + (cur.indexOf(String(b.BranchName)) >= 0 ? ' checked' : '') + '></td><td>' + esc(b.BranchName) + '</td></tr>';
            }).join('') + '</table>';
        }
        wrap.querySelector('.bc-drop').addEventListener('click', function () {
            if (pop.style.display === 'none') { draw(); pop.style.display = ''; } else pop.style.display = 'none';
        });
        pop.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var b = list[+tr.getAttribute('data-i')], cur = names(), k = cur.indexOf(String(b.BranchName));
            if (k >= 0) cur.splice(k, 1); else cur.push(String(b.BranchName));
            txt.value = cur.join(',');
            draw();
        });
        document.addEventListener('click', function (e) { if (!wrap.contains(e.target)) pop.style.display = 'none'; });
        return { text: function () { return txt.value; }, id: function () { return 0; } };
    }

    /* ------------------------------------------------------------------ browser print of rows */
    function printRows(title, note, cols, rows, fmtCell) {
        var w = global.open('', '_blank');
        if (!w) { box('The print window was blocked by the browser.'); return; }
        var h = '<!DOCTYPE html><html><head><meta charset="utf-8"><title>' + esc(title) + '</title><style>'
            + 'body{font-family:Segoe UI,Verdana,sans-serif;font-size:11px;margin:16px}h2{margin:0 0 4px;font-size:15px}'
            + '.muted{color:#666;font-size:10px}table{border-collapse:collapse;width:100%;margin-top:6px}'
            + 'th,td{border:1px solid #999;padding:2px 5px;text-align:left}th{background:#eee}td.n{text-align:right}'
            + '@media print{.noprint{display:none}}</style></head><body><div class="noprint"><button onclick="window.print()">Print</button></div>'
            + '<h2>' + esc(title) + '</h2><div class="muted">' + esc(note) + '</div><table><tr>'
            + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>'
            + rows.map(function (r) {
                return '<tr>' + cols.map(function (c) {
                    var v = ci(r, c);
                    var f = fmtCell ? fmtCell(c, v, r) : (v == null ? '' : v);
                    return '<td' + (typeof v === 'number' ? ' class="n"' : '') + '>' + esc(f) + '</td>';
                }).join('') + '</tr>';
            }).join('') + '</table></body></html>';
        w.document.open(); w.document.write(h); w.document.close();
    }

    function keys(map) {
        document.addEventListener('keydown', function (e) {
            var k = (e.ctrlKey ? 'Ctrl+' : '') + (e.altKey ? 'Alt+' : '') + (e.key.length === 1 ? e.key.toUpperCase() : e.key);
            if (map[k]) { e.preventDefault(); map[k](e); }
        });
    }

    global.AccH = {
        $: $, esc: esc, api: api, ci: ci, toNum: toNum,
        both: both, bothOpt: bothOpt, opt: opt, fixed: fixed, zz: zz, rate: rate,
        shortDate: shortDate, dMMMyyyy: dMMMyyyy, dMMMyyyyhm: dMMMyyyyhm, today: today,
        box: box, shortcuts: shortcuts, branchCombo: branchCombo, printRows: printRows, keys: keys
    };
})(window);
