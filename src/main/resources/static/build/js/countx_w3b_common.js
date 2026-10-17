/* ============================================================================================
 * W3B - shared helpers of the Mfg Production hub pages (module 103). window.W3B.
 * Same conventions as the Recipe (Branding) page: window.alert / window.confirm stand in for the desktop's MessageBox,
 * <select class="dtcombo" data-dtcombo="single"> are the searchable UltraCombo replacements (countx_prod_combo.js).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var W3B = {};
    W3B.$ = function (id) { return document.getElementById(id); };
    W3B.val = function (id) { var e = W3B.$(id); return e ? e.value : ''; };
    W3B.setVal = function (id, v) { var e = W3B.$(id); if (e) e.value = (v === null || v === undefined) ? '' : v; };
    W3B.box = function (m) { window.alert(m); };
    W3B.ask = function (m) { return window.confirm(m); };
    W3B.show = function (id, on) { var e = W3B.$(id); if (e) e.classList.toggle('is-hidden', !on); };
    W3B.focus = function (id) { var e = W3B.$(id); if (!e) return; try { e.focus(); } catch (x) { /* ignore */ } };
    W3B.esc = function (s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;'); };
    W3B.num = function (v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; };
    W3B.int = function (v) { return Math.trunc(W3B.num(v)); };
    W3B.fmt = function (v, places) { return W3B.num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: places === undefined ? 3 : places }); };
    W3B.iso = function (d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); };
    W3B.today = function () { return W3B.iso(new Date()); };
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    W3B.fmtDMY = function (v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || '')); return m ? m[3] + '-' + MONTHS[+m[2] - 1] + '-' + m[1].slice(2) : String(v || ''); };
    W3B.fmtStamp = function (v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2}))?/.exec(String(v || '')); if (!m) return String(v || '');
        var h = +(m[4] || 0), tt = h < 12 ? 'AM' : 'PM', h12 = h % 12 === 0 ? 12 : h % 12;
        return m[3] + '-' + m[2] + '-' + m[1] + ' ' + String(h12).padStart(2, '0') + ':' + (m[5] || '00') + ' ' + tt;
    };
    W3B.qs = function (o) { var a = []; Object.keys(o).forEach(function (k) { if (o[k] !== undefined && o[k] !== null && o[k] !== '') a.push(k + '=' + encodeURIComponent(o[k])); }); return a.length ? '?' + a.join('&') : ''; };
    function csrf(opt) {
        var token = document.querySelector('meta[name="_csrf"]'), header = document.querySelector('meta[name="_csrf_header"]');
        if (token && header && opt.method && opt.method.toUpperCase() !== 'GET') opt.headers[header.getAttribute('content')] = token.getAttribute('content');
    }
    W3B.request = function (url, options) {
        var opt = options || {};
        opt.credentials = 'same-origin'; opt.headers = opt.headers || {}; opt.headers.Accept = 'application/json';
        csrf(opt);
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var body = null;
                try { body = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((body && body.message) || t || ('Request failed (' + r.status + ')'));
                return body;
            });
        });
    };
    W3B.getJson = function (url) { return W3B.request(url, {}); };
    W3B.postJson = function (url, body) { return W3B.request(url, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) }); };
    W3B.postForm = function (url) { return W3B.request(url, { method: 'POST', headers: {} }); };
    W3B.fail = function (e) { W3B.box(e && e.message ? e.message : String(e)); };
    /** DropDownBind.BindDDL / BindDDLNew: zeroIndex adds the "...Select Any Value..." row; the selection is kept when still present. */
    W3B.fill = function (selectId, rows, zeroIndex, keepValue) {
        var el = W3B.$(selectId); if (!el) return;
        var keep = keepValue === undefined ? el.value : String(keepValue);
        var h = zeroIndex ? '<option value="0">...Select Any Value...</option>' : '<option value=""></option>';
        (rows || []).forEach(function (r) { h += '<option value="' + W3B.esc(r.Id) + '">' + W3B.esc(r.Name) + '</option>'; });
        el.innerHTML = h;
        el.value = keep;
        if (el.value !== keep) el.selectedIndex = 0;
    };
    W3B.setCombo = function (id, v) { var el = W3B.$(id); if (!el) return; el.value = String(v === null || v === undefined ? '' : v); if (el.value !== String(v) && el.options.length) el.selectedIndex = 0; };
    W3B.clearCombo = function (id) { var el = W3B.$(id); if (el && el.options.length) el.selectedIndex = 0; };
    W3B.selText = function (id) { var el = W3B.$(id); if (!el || el.selectedIndex < 0) return ''; var o = el.options[el.selectedIndex]; return (o && o.value !== '' && o.value !== '0') ? o.text : ''; };
    /** Click handler for ctrl-key shortcut tables: W3B.keys({ 's': fn, 'u': fn, ... }) (Ctrl+key; 'Escape' / 'Esc' without Ctrl). */
    W3B.keys = function (map) {
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && !e.altKey && map[(e.shiftKey ? 'shift+' : '') + k]) { e.preventDefault(); map[(e.shiftKey ? 'shift+' : '') + k](e); return; }
            if (!e.ctrlKey && !e.altKey && k === 'escape' && map.escape) { map.escape(e); }
        });
    };
    /** Sorted-by-nothing grid table builder: cols [{h, k|f, cls, w}], rows array; optional rowClick(i). Returns HTML. */
    W3B.tableHtml = function (cols, rows, opt) {
        opt = opt || {};
        var h = '<thead><tr>' + cols.map(function (c) { return '<th' + (c.w ? ' style="width:' + c.w + 'px"' : '') + '>' + W3B.esc(c.h) + '</th>'; }).join('') + '</tr></thead><tbody>';
        if (!rows.length) h += '<tr class="rc-empty"><td colspan="' + cols.length + '">' + W3B.esc(opt.empty || 'No records') + '</td></tr>';
        rows.forEach(function (r, i) {
            h += '<tr data-i="' + i + '"' + (opt.rowClass ? ' class="' + opt.rowClass(r, i) + '"' : '') + '>' + cols.map(function (c) {
                var v = c.f ? c.f(r, i) : r[c.k];
                if (c.html) return '<td class="' + (c.cls || '') + '">' + v + '</td>';
                if (c.bool) v = v ? '&#10003;' : '';
                else v = W3B.esc(v);
                return '<td class="' + (c.cls || '') + '">' + v + '</td>';
            }).join('') + '</tr>';
        });
        return h + '</tbody>';
    };
    global.W3B = W3B;
}(window));
