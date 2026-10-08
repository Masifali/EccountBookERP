/*
 * Shared client helpers for the Sale Engr screens (Architecture.WinApp.SaleTrading.*).
 * window.SE: api, busy, MessageBox (alert / ask), formatting, Janus-style grid, tab control, shortcut keys.
 * No dependency except what the page already loads (countx_prod_combo.js / acc_c_xcombo.js for the combos).
 */
(function (w, d) {
    'use strict';
    var SE = {};
    function $(id) { return d.getElementById(id); }
    function esc(s) { return String(s == null ? '' : s).replace(/[&<>"']/g, function (c) { return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]; }); }
    SE.$ = $; SE.esc = esc;

    /* ------------------------------------------------------------------ busy + api */
    var pending = 0, loader = null;
    SE.busy = function (on) {
        pending = Math.max(0, pending + (on ? 1 : -1));
        if (!loader) { loader = d.createElement('div'); loader.className = 'se-loader'; loader.innerHTML = '<span>Loading, please wait...</span>'; d.body.appendChild(loader); }
        loader.classList.toggle('on', pending > 0);
    };
    SE.api = function (url, opt) {
        opt = opt || {};
        var quiet = opt.quiet; delete opt.quiet;
        if (!quiet) SE.busy(true);
        var init = { credentials: 'same-origin', headers: { 'Accept': 'application/json' } };
        for (var k in opt) init[k] = opt[k];
        if (init.body && typeof init.body !== 'string') init.body = JSON.stringify(init.body);
        if (init.body) init.headers['Content-Type'] = 'application/json';
        return fetch(url, init).then(function (r) {
            return r.text().then(function (t) {
                if (r.redirected && /\/login(?:[?#]|$)/.test(r.url)) throw new Error('Your session has expired. Please sign in again.');
                var b = {};
                try { b = t ? JSON.parse(t) : {}; } catch (e) { if (r.ok) throw new Error('The server did not return valid data. Please refresh or sign in again.'); b = { message: t }; }
                if (!r.ok) {
                    var m = (b && (b.message || b.detail || b.error)) || ('Request failed (' + r.status + ')');
                    var err = new Error(m); err.status = r.status; throw err;
                }
                return b;
            });
        }).then(function (b) { if (!quiet) SE.busy(false); return b; },
                function (e) { if (!quiet) SE.busy(false); throw e; });
    };
    SE.get = function (url) { return SE.api(url); };
    SE.post = function (url, body) { return SE.api(url, { method: 'POST', body: body || {} }); };

    /* ------------------------------------------------------------------ MessageBox */
    var box = null, boxQueue = [];
    function showBox(item) {
        if (!box) {
            box = d.createElement('div'); box.className = 'se-msg';
            box.innerHTML = '<div class="cap"></div><div class="body"></div><div class="btns"></div>';
            d.body.appendChild(box);
        }
        box.querySelector('.cap').textContent = item.caption;
        box.querySelector('.body').textContent = item.text;
        var b = box.querySelector('.btns'); b.innerHTML = '';
        item.buttons.forEach(function (label, i) {
            var x = d.createElement('button'); x.type = 'button'; x.textContent = label;
            x.onclick = function () { box.classList.remove('on'); item.resolve(i === 0); next(); };
            b.appendChild(x);
        });
        box.classList.add('on');
        var f = b.querySelector('button'); if (f) f.focus();
    }
    function next() { if (boxQueue.length) showBox(boxQueue.shift()); else box = box; }
    function enqueue(item) { return new Promise(function (res) { item.resolve = res; if (box && box.classList.contains('on')) boxQueue.push(item); else showBox(item); }); }
    /* MessageBox.Show(text) / MessageBox.Show(text, caption) */
    SE.alert = function (text, caption) { return enqueue({ text: String(text == null ? '' : text), caption: caption || 'Message', buttons: ['OK'] }); };
    /* MessageBox.Show(text, caption, YesNo) -> true for Yes */
    SE.ask = function (text, caption) { return enqueue({ text: String(text), caption: caption || 'Confirm', buttons: ['Yes', 'No'] }); };
    SE.dbError = function (e) { return SE.alert(e && e.message ? e.message : String(e), 'Database Error'); };

    /* ------------------------------------------------------------------ formatting */
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    function p2(n) { return (n < 10 ? '0' : '') + n; }
    SE.pad2 = p2;
    /* any server date value -> {y,m,d,h,i} (no time-zone shifting: the text is what the database holds) */
    SE.parts = function (v) {
        if (v == null || v === '') return null;
        if (typeof v === 'number') { var dt = new Date(v); return { y: dt.getFullYear(), m: dt.getMonth() + 1, d: dt.getDate(), h: dt.getHours(), i: dt.getMinutes(), s: dt.getSeconds() }; }
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(String(v));
        if (!m) return null;
        if (+m[1] <= 1901) return null;                       /* .NET MinValue / 1900-01-01 = "no date" */
        return { y: +m[1], m: +m[2], d: +m[3], h: +(m[4] || 0), i: +(m[5] || 0), s: +(m[6] || 0) };
    };
    SE.dateInput = function (v) { var p = SE.parts(v); return p ? p.y + '-' + p2(p.m) + '-' + p2(p.d) : ''; };
    SE.dtInput = function (v) { var p = SE.parts(v); return p ? p.y + '-' + p2(p.m) + '-' + p2(p.d) + 'T' + p2(p.h) + ':' + p2(p.i) : ''; };
    SE.today = function () { var n = new Date(); return n.getFullYear() + '-' + p2(n.getMonth() + 1) + '-' + p2(n.getDate()); };
    SE.nowInput = function () { var n = new Date(); return n.getFullYear() + '-' + p2(n.getMonth() + 1) + '-' + p2(n.getDate()) + 'T' + p2(n.getHours()) + ':' + p2(n.getMinutes()); };
    SE.addDays = function (days) { var n = new Date(); n.setDate(n.getDate() + days); return n.getFullYear() + '-' + p2(n.getMonth() + 1) + '-' + p2(n.getDate()); };
    SE.dMY = function (v) { var p = SE.parts(v); return p ? p2(p.d) + '/' + p2(p.m) + '/' + p.y : ''; };
    SE.dMonY = function (v) { var p = SE.parts(v); return p ? p2(p.d) + '-' + MON[p.m - 1] + '-' + p.y : ''; };
    SE.dMonYTime = function (v) { var p = SE.parts(v); return p ? p2(p.d) + '-' + MON[p.m - 1] + '-' + p.y + ' ' + p2(p.h) + ':' + p2(p.i) + ':' + p2(p.s) : ''; };
    SE.dMYTime12 = function (v) { var p = SE.parts(v); if (!p) return ''; var h = p.h % 12 || 12; return p2(p.d) + '-' + p2(p.m) + '-' + p.y + ' ' + p2(h) + ':' + p2(p.i) + ' ' + (p.h >= 12 ? 'PM' : 'AM'); };
    SE.num = function (v, maxDec, minDec) {
        if (v == null || v === '') return '';
        var n = Number(v); if (!isFinite(n)) return String(v);
        return n.toLocaleString('en-US', { minimumFractionDigits: minDec || 0, maximumFractionDigits: maxDec == null ? 3 : maxDec });
    };
    SE.toNum = function (v) { if (v == null) return 0; var n = Number(String(v).replace(/,/g, '')); return isFinite(n) ? n : 0; };
    SE.toInt = function (v) { var n = parseInt(String(v == null ? '' : v).trim(), 10); return isNaN(n) ? 0 : n; };
    /* Conversion.ToInt on a text box that holds only digits */
    SE.digitsOnly = function (el) { el.addEventListener('keypress', function (e) { if (e.key.length === 1 && !/\d/.test(e.key) && !e.ctrlKey) e.preventDefault(); }); };
    SE.upper = function (el) { el.addEventListener('input', function () { var p = el.selectionStart; el.value = el.value.toUpperCase(); try { el.setSelectionRange(p, p); } catch (x) { } }); };

    /* ------------------------------------------------------------------ tab control */
    SE.tabs = function (root, onChange) {
        var tc = typeof root === 'string' ? $(root) : root;
        var tabs = tc.querySelectorAll(':scope > .dsubtabs > .tab');
        var api = {
            select: function (id) {
                tabs.forEach(function (t) { t.classList.toggle('on', t.getAttribute('data-tab') === id); });
                tc.querySelectorAll(':scope > .tpane').forEach(function (p) { p.style.display = p.id === id ? '' : 'none'; });
                if (onChange) onChange(id);
            },
            index: function () { var r = 0; tabs.forEach(function (t, i) { if (t.classList.contains('on')) r = i; }); return r; },
            visibleTabs: function () { return Array.prototype.filter.call(tabs, function (t) { return t.style.display !== 'none'; }).map(function (t) { return t.getAttribute('data-tab'); }); },
            show: function (id, on) { tabs.forEach(function (t) { if (t.getAttribute('data-tab') === id) t.style.display = on ? '' : 'none'; }); },
            selectIndex: function (i) { var v = api.visibleTabs(); if (v[i]) api.select(v[i]); }
        };
        tabs.forEach(function (t) { t.addEventListener('click', function () { api.select(t.getAttribute('data-tab')); }); });
        return api;
    };

    /* ------------------------------------------------------------------ Janus-style grid */
    /* spec: { cols:[{k,t,w,f,sum,btn,link,hide,edit,cls}], frozen, footer, onDbl(row), onBtn(key,row,i), onLink(key,row,i), onEdit(row,key,val,i), dec }
       f: 'n0' '#,##0'  'n3' '#,##0.###'  'amt' (spec.dec decimals)  'date' dd-MMM-yyyy  'dt' dd-MMM-yyyy HH:mm:ss  'dt12' dd-MM-yyyy hh:mm tt  'chk'  */
    SE.grid = function (el, spec) {
        if (typeof el === 'string') el = $(el);
        var rows = [], curRow = -1, curCol = null, visCols = function () { return spec.cols.filter(function (c) { return !c.hide; }); };
        function fmt(c, v, row) {
            if (c.render) return c.render(v, row);
            if (c.list) { var L = c.list() || [], lk = c.lk || 'Id', lt = c.lt || 'Name'; for (var q = 0; q < L.length; q++) if (String(L[q][lk]) === String(v)) return L[q][lt] == null ? '' : String(L[q][lt]); return ''; }
            switch (c.f) {
                case 'n0': return v == null || v === '' ? '' : SE.num(v, 0);
                case 'n2': return v == null || v === '' ? '' : SE.num(v, 2);
                case 'n3': return v == null || v === '' ? '' : SE.num(v, 3);
                case 'amt': return v == null || v === '' ? '' : SE.num(v, spec.dec == null ? 0 : spec.dec, spec.dec == null ? 0 : spec.dec);
                case 'date': return SE.dMonY(v);
                case 'sdate': return SE.dMY(v);
                case 'dt': return SE.dMonYTime(v);
                case 'dt12': return SE.dMYTime12(v);
                case 'chk': return v ? '&#9745;' : '&#9744;';
                default: return v == null ? '' : String(v);
            }
        }
        function isNum(c) { return /^(n0|n2|n3|amt)$/.test(c.f || ''); }
        function render() {
            var vc = visCols();
            var h = '<table class="jg"><colgroup>' + vc.map(function (c) { return '<col style="width:' + (c.w || 90) + 'px">'; }).join('') + '</colgroup><thead><tr>';
            h += vc.map(function (c) { return '<th class="' + (isNum(c) ? 'num ' : '') + (c.btn ? 'btn' : '') + '" data-k="' + esc(c.k) + '">' + (c.sel ? '<input type="checkbox" data-hsel' + (rows.length && rows.every(function (r) { return r._chk; }) ? ' checked' : '') + '>' : esc(c.t == null ? c.k : c.t)) + '</th>'; }).join('') + '</tr></thead><tbody>';
            if (!rows.length) h += '';
            rows.forEach(function (r, i) {
                h += '<tr data-i="' + i + '"' + (i === curRow ? ' class="sel"' : '') + '>';
                vc.forEach(function (c) {
                    var cls = (isNum(c) ? 'num ' : '') + (c.btn ? 'btn ' : '') + (c.edit ? 'ed ' : '') + (c.cls || '');
                    var v = (c.btn || c.sel) ? null : r[c.k];
                    var inner;
                    if (c.sel) inner = '<input type="checkbox" data-sel' + (r._chk ? ' checked' : '') + '>';
                    else if (c.btn) inner = '<button type="button" class="tbtn" data-b="' + esc(c.k) + '">' + esc(c.btn) + '</button>';
                    else if (c.link && v != null && v !== '') inner = '<a class="lnk" data-l="' + esc(c.k) + '">' + esc(fmt(c, v, r)) + '</a>';
                    else if (c.f === 'chk') inner = fmt(c, v, r);
                    else inner = esc(fmt(c, v, r));
                    h += '<td class="' + cls + '" data-k="' + esc(c.k) + '">' + inner + '</td>';
                });
                h += '</tr>';
            });
            h += '</tbody>';
            if (spec.footer !== false && vc.some(function (c) { return c.sum; })) {
                h += '<tfoot><tr>' + vc.map(function (c, ix) {
                    if (!c.sum) return '<td>' + (ix === 0 && !c.sum ? '' : '') + '</td>';
                    var t = rows.reduce(function (a, r) { return a + SE.toNum(r[c.k]); }, 0);
                    return '<td class="num">' + esc(c.f === 'amt' ? SE.num(t, spec.dec || 0, spec.dec || 0) : SE.num(t, c.f === 'n0' ? 0 : 3)) + '</td>';
                }).join('') + '</tr></tfoot>';
            }
            h += '</table>';
            var sy = el.scrollTop, sx = el.scrollLeft;
            el.innerHTML = h;
            el.scrollTop = sy; el.scrollLeft = sx;
        }
        el.addEventListener('click', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i'); var td = e.target.closest('td'); curCol = td ? td.getAttribute('data-k') : null;
            if (i !== curRow) { curRow = i; el.querySelectorAll('tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); if (spec.onSel) spec.onSel(rows[i], i); }
            var b = e.target.closest('button[data-b]'); if (b && spec.onBtn) { spec.onBtn(b.getAttribute('data-b'), rows[i], i); return; }
            var l = e.target.closest('a[data-l]'); if (l && spec.onLink) { e.preventDefault(); spec.onLink(l.getAttribute('data-l'), rows[i], i); return; }
            if (td && spec.cols.some(function (c) { return c.k === curCol && c.edit; }) && spec.onEdit) startEdit(td, i, curCol);
        });
        el.addEventListener('change', function (e) {
            var t = e.target;
            if (t.hasAttribute && t.hasAttribute('data-hsel')) { rows.forEach(function (r) { r._chk = t.checked; }); render(); if (spec.onCheck) spec.onCheck(null, -1); return; }
            if (t.hasAttribute && t.hasAttribute('data-sel')) {
                var tr = t.closest('tr[data-i]'); if (!tr) return;
                var i = +tr.getAttribute('data-i'); rows[i]._chk = t.checked; render(); if (spec.onCheck) spec.onCheck(rows[i], i);
            }
        });
        el.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr || e.target.closest('button,input')) return;
            var i = +tr.getAttribute('data-i'); curRow = i;
            if (spec.onDbl) spec.onDbl(rows[i], i);
        });
        function startEdit(td, i, key) {
            var c = spec.cols.filter(function (x) { return x.k === key; })[0];
            if (td.querySelector('input')) return;
            var cur = rows[i][key];
            if (c.list) {
                var sel = d.createElement('select'); sel.className = 'cell';
                var L = c.list() || [], lk = c.lk || 'Id', lt = c.lt || 'Name';
                sel.innerHTML = '<option value=""></option>' + L.map(function (o) { return '<option value="' + esc(o[lk]) + '"' + (String(o[lk]) === String(cur) ? ' selected' : '') + '>' + esc(o[lt]) + '</option>'; }).join('');
                td.innerHTML = ''; td.appendChild(sel); sel.focus();
                var fin = false;
                function cm(ok) { if (fin) return; fin = true; if (ok) spec.onEdit(rows[i], key, sel.value, i); render(); }
                sel.addEventListener('change', function () { cm(true); });
                sel.addEventListener('blur', function () { cm(false); });
                sel.addEventListener('keydown', function (ev) { if (ev.key === 'Escape') cm(false); });
                return;
            }
            var inp = d.createElement('input'); inp.className = 'cell' + (isNum(c) ? ' num' : ''); inp.value = cur == null ? '' : cur;
            td.innerHTML = ''; td.appendChild(inp); inp.focus(); inp.select();
            var done = false;
            function commit(ok) {
                if (done) return; done = true;
                if (ok) spec.onEdit(rows[i], key, inp.value, i);
                render();
            }
            inp.addEventListener('blur', function () { commit(true); });
            inp.addEventListener('keydown', function (ev) { if (ev.key === 'Enter') { ev.preventDefault(); commit(true); } else if (ev.key === 'Escape') { commit(false); } });
        }
        el.addEventListener('keydown', function (e) {
            if (e.target.tagName === 'INPUT') return;
            if (e.key === 'ArrowDown' && curRow < rows.length - 1) { e.preventDefault(); api.select(curRow + 1); }
            else if (e.key === 'ArrowUp' && curRow > 0) { e.preventDefault(); api.select(curRow - 1); }
            else if (e.ctrlKey && e.key === 'Enter' && spec.onDbl && rows[curRow]) { e.preventDefault(); spec.onDbl(rows[curRow], curRow); }
            else if (e.ctrlKey && e.key === ' ' && rows[curRow] && curCol) {
                e.preventDefault();
                var c = spec.cols.filter(function (x) { return x.k === curCol; })[0];
                if (c && c.btn && spec.onBtn) spec.onBtn(c.k, rows[curRow], curRow);
                else if (c && c.link && spec.onLink) spec.onLink(c.k, rows[curRow], curRow);
            }
        });
        var api = {
            el: el, spec: spec,
            setRows: function (r) { rows = r || []; curRow = rows.length ? Math.min(Math.max(curRow, 0), rows.length - 1) : -1; if (curRow < 0 && rows.length) curRow = 0; render(); if (spec.onSel) spec.onSel(rows[curRow] || null, curRow); return api; },
            rows: function () { return rows; },
            clear: function () { rows = []; curRow = -1; render(); return api; },
            cur: function () { return rows[curRow] || null; },
            curIndex: function () { return curRow; },
            curKey: function () { return curCol; },
            select: function (i) { curRow = i; render(); var tr = el.querySelector('tr.sel'); if (tr && tr.scrollIntoView) tr.scrollIntoView({ block: 'nearest' }); if (spec.onSel) spec.onSel(rows[i] || null, i); },
            refresh: render,
            addRow: function (r) { rows.push(r); curRow = rows.length - 1; render(); },
            removeAt: function (i) { rows.splice(i, 1); curRow = Math.min(curRow, rows.length - 1); render(); },
            count: function () { return rows.length; },
            checked: function () { return rows.filter(function (r) { return r._chk; }); },
            check: function (fn) { rows.forEach(function (r, i) { if (fn(r, i)) r._chk = true; }); render(); }
        };
        el.setAttribute('tabindex', '0');
        render();
        return api;
    };

    /* ------------------------------------------------------------------ pop-up (modal grid / form host) */
    SE.pop = function (title, bodyHtml, buttons, opts) {
        var pop = d.createElement('div'); pop.className = 'se-pop' + (opts && opts.wide ? ' wide' : '');
        pop.innerHTML = '<div><div class="ph"><span></span><span style="cursor:pointer" data-x>&#10005;</span></div><div class="pb"></div><div class="pf"></div></div>';
        d.body.appendChild(pop);
        pop.querySelector('.ph span').textContent = title;
        pop.querySelector('.pb').innerHTML = bodyHtml || '';
        var pf = pop.querySelector('.pf');
        (buttons || [{ t: 'Close' }]).forEach(function (b) {
            var x = d.createElement('button'); x.type = 'button'; x.textContent = b.t;
            x.onclick = function () { if (b.fn) b.fn(api); else api.close(); };
            pf.appendChild(x);
        });
        var api = { el: pop, body: pop.querySelector('.pb'), open: function () { pop.classList.add('on'); return api; }, close: function () { pop.classList.remove('on'); if (pop.parentNode) pop.parentNode.removeChild(pop); } };
        pop.querySelector('[data-x]').onclick = api.close;
        return api;
    };

    SE.shortcuts = function (rows) {
        var h = '<div class="dgrid" style="max-height:60vh"><table class="jg"><thead><tr><th style="width:160px">KeyCombination</th><th>Description</th></tr></thead><tbody>' +
            rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('') + '</tbody></table></div>';
        SE.pop('Shortcut Keys', h).open();
    };

    /* ------------------------------------------------------------------ misc */
    SE.fill = function (cb, rows) { cb.setData(rows || []); };
    SE.printRpt = function (rpt, args) {
        if (typeof w.printRpt !== 'function') { SE.alert('Printing is not available on this page.'); return; }
        return w.printRpt(rpt, args || {});
    };
    SE.param = function (name) { return new URLSearchParams(w.location.search).get(name); };
    SE.q = function (o) { var q = new URLSearchParams(); Object.keys(o).forEach(function (k) { if (o[k] !== undefined && o[k] !== null && o[k] !== '') q.set(k, o[k]); }); var s = q.toString(); return s ? '?' + s : ''; };
    w.SE = SE;
})(window, document);
