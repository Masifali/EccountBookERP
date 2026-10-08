/* ============================================================================================
 * Group R1 (Sales Reports 832 / 840 / 908 / 490 / 485 / 487 / 491 / 486 / 488 / 489) - shared page engine.
 *   window.SaRpt1 = { el, fmtNum, fmtDate, fmtSingle, fmtRate, fill, idOf, textOf, selText, selInt, dateRule, DATE_TYPES, branchIds,
 *                     fillBranches, Grid, keys, post }
 * Built on window.AcRpt1 (countx_acrpt1_core.js: getJson, openPdf, shortcuts, today ...). Nothing of the shared engines is edited.
 *
 * SaRpt1.Grid is the Janus GridEX look the ten desktop forms configure, with what AcRpt1.Grid does not have:
 *   FrozenColumns (sticky left), ColumnType Button (Print / Voucher), Select column (ActAsSelector + UseHeaderSelector),
 *   per column FormatString / TotalFormatString in the .NET custom numeric + date syntax, row format conditions (rowStyle),
 *   GridAutoAdjustment (best fit on every column but the last), HeaderLines 2 / 3, the filter row (Contains) and sort.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1;
    var S = w.SaRpt1 = {};
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    S.el = A.el;

    // ------------------------------------------------------------------ .NET custom numeric format
    function group3(s) { return s.replace(/\B(?=(\d{3})+(?!\d))/g, ','); }
    function roundAway(n, places) {                      // decimal rounding, midpoint away from zero (String.Format on a decimal / Janus)
        var f = Math.pow(10, places);
        var v = Math.abs(n) * f;
        var r = Math.round(v + 1e-9 * Math.max(1, v));
        return r / f;
    }
    /* "#,##0.##", "#,#", "#,#;(#,#);0", "#,##0.00;(0,0.00); 0", "##,#.##" ... */
    S.fmtNum = function (value, fmt) {
        var n = typeof value === 'number' ? value : Number(String(value).trim());
        if (value === null || value === undefined || value === '' || !isFinite(n)) return value == null ? '' : String(value);
        if (!fmt) return String(n);
        var secs = fmt.split(';'), sec, neg = false;
        if (n > 0 || (n === 0 && secs.length < 3)) { sec = secs[0]; if (n < 0) neg = true; }
        else if (n < 0) { if (secs.length > 1) sec = secs[1]; else { sec = secs[0]; neg = true; } }
        else sec = secs[2];
        var m = /[#0][#0,]*(?:\.[#0]*)?|\.[#0]+/.exec(sec);
        if (!m) return sec;                                  // a literal section (e.g. " 0" has a digit, so this is rare)
        var pat = m[0], pre = sec.slice(0, m.index), post = sec.slice(m.index + pat.length);
        var dot = pat.indexOf('.'), ip = dot < 0 ? pat : pat.slice(0, dot), fp = dot < 0 ? '' : pat.slice(dot + 1);
        var grouping = ip.indexOf(',') >= 0;
        var minInt = (ip.match(/0/g) || []).length;
        var minFrac = (fp.match(/0/g) || []).length, maxFrac = fp.length;
        var abs = secs.length > 1 && n < 0 && sec === secs[1] ? Math.abs(n) : Math.abs(n);
        var r = roundAway(abs, maxFrac), s = r.toFixed(maxFrac), parts = s.split('.');
        var intPart = parts[0], frac = parts[1] || '';
        while (frac.length > minFrac && frac.charAt(frac.length - 1) === '0') frac = frac.slice(0, -1);
        if (intPart === '0' && minInt === 0) intPart = '';
        while (intPart.length < minInt) intPart = '0' + intPart;
        if (grouping) intPart = group3(intPart);
        var body = intPart + (frac ? '.' + frac : '');
        if (body === '' && r === 0 && minInt === 0 && !frac) body = '';
        var out = pre + body + post;
        if (neg && r !== 0) out = '-' + out;
        return out;
    };
    S.fmtSingle = function (n) { n = A.toInt(n); return '#,##0.' + new Array((n >= 1 && n <= 4 ? n : 0) + 1).join('0'); };     // stringFormatsingle
    S.fmtRate = function (n) { n = A.toInt(n); return '#,#0.' + new Array((n >= 1 && n <= 4 ? n : (n === 0 ? 2 : 0)) + 1).join('0'); };   // DecimalRateFormate

    // ------------------------------------------------------------------ date / time pattern
    S.fmtDate = function (v, pat) {
        if (v === null || v === undefined || v === '') return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(String(v));
        if (!m) return String(v);
        pat = pat || 'dd-MMM-yy';
        var H = m[4] ? +m[4] : 0, h12 = H % 12 === 0 ? 12 : H % 12;
        function p2(x) { return ('0' + x).slice(-2); }
        return pat.replace(/yyyy|yy|MMM|MM|dd|hh|HH|mm|ss|tt/g, function (t) {
            switch (t) {
                case 'yyyy': return m[1]; case 'yy': return m[1].slice(2); case 'MMM': return MON[+m[2] - 1]; case 'MM': return m[2];
                case 'dd': return m[3]; case 'hh': return p2(h12); case 'HH': return p2(H); case 'mm': return m[5] || '00';
                case 'ss': return m[6] || '00'; case 'tt': return H >= 12 ? 'PM' : 'AM';
            }
            return t;
        });
    };
    S.isoDay = function (v) { var m = /^(\d{4}-\d{2}-\d{2})/.exec(String(v == null ? '' : v)); return m ? m[1] : ''; };

    // ------------------------------------------------------------------ combos
    var TEXT_KEYS = ['ReferenceName', 'name', 'Name', 'Description', 'CompanyName', 'ItemName', 'WareHouseName', 'JobLotDescription', 'BranchName', 'Status', 'Parameters'];
    S.idOf = function (r) { var v = A.ci(r, 'Id'); if (v === undefined) { var k = Object.keys(r)[0]; v = r[k]; } return v; };
    S.textOf = function (r) {
        for (var i = 0; i < TEXT_KEYS.length; i++) { var v = A.ci(r, TEXT_KEYS[i]); if (v !== undefined && v !== null) return String(v); }
        return '';
    };
    /* DropDownBind.BindDDL: zero = ZeroIndex true (row 0 "...Select Any Value..." value 0, no empty row); otherwise the empty slot AcRpt1.fill adds. */
    S.fill = function (sel, rows, zero, keep) {
        if (!sel) return;
        if (sel.multiple) { A.fill(sel, rows, S.idOf, S.textOf, keep); return; }
        if (zero) {
            var old = keep ? sel.value : '';
            while (sel.options.length) sel.remove(0);
            sel.add(new Option('...Select Any Value...', '0'));
            (rows || []).forEach(function (r) { sel.add(new Option(S.textOf(r), String(S.idOf(r)))); });
            sel.value = old && Array.prototype.some.call(sel.options, function (o) { return o.value === old; }) ? old : '0';
            sel.dispatchEvent(new Event('change', { bubbles: true }));
        } else A.fill(sel, rows, S.idOf, S.textOf, keep);
    };
    S.selInt = function (id) { var s = A.el(id); return s ? A.toInt(s.value) : 0; };
    S.selText = function (id) {
        var s = A.el(id); if (!s || s.selectedIndex < 0 || s.value === '') return '';
        return s.options[s.selectedIndex].textContent.trim();
    };
    S.setIndex = function (id, i) {                         // Rows[i].Activate()
        var s = A.el(id); if (!s || i >= s.options.length) return;
        s.selectedIndex = i; s.dispatchEvent(new Event('change', { bubbles: true }));
    };
    /* Rows[row].Activate(): AcRpt1.fill puts an empty slot first on single selects (ZeroIndex false), so the desktop row n is option n+1 there */
    S.activate = function (id, row, zero) {
        var s = A.el(id); if (!s) return;
        var i = zero ? row : row + 1;
        if (i >= s.options.length) return;
        s.selectedIndex = i; s.dispatchEvent(new Event('change', { bubbles: true }));
    };
    /* ValueChanged -> Leave: run fn when focus leaves the (combo enhanced) control */
    S.onLeave = function (id, fn) {
        var w = S.wrapOf(id); if (!w) return;
        w.addEventListener('focusout', function (e) { if (!e.relatedTarget || !w.contains(e.relatedTarget)) setTimeout(fn, 0); });
    };
    S.wrapOf = function (id) { var s = A.el(id); return s ? (s.closest('.dtcombo-wrap') || s) : null; };

    /* CommonServices.DateType(): Id 1..5 */
    S.DATE_TYPES = [[1, 'This Day'], [2, 'This Week'], [3, 'This Month'], [4, 'This Year'], [5, 'Financial Year']];
    S.dateTypeRows = function (withAll) {
        var rows = S.DATE_TYPES.map(function (r) { return { Id: r[0], name: r[1] }; });
        if (withAll) rows.push({ Id: 6, name: 'All' });
        return rows;
    };
    /* cmbDateType_ValueChanged. o.swap4 (832: To = 1 Jan, From = today), o.to5 (840: Financial Year also sets To = today). */
    S.dateRule = function (typeId, from, to, yearStart, o) {
        o = o || {};
        var t = A.today(), u = new Date();
        if (typeId === 1) from.value = t;
        else if (typeId === 2) from.value = A.addDays(t, -7);
        else if (typeId === 3) { from.value = u.getUTCFullYear() + '-' + ('0' + (u.getUTCMonth() + 1)).slice(-2) + '-01'; to.value = t; }
        else if (typeId === 4) {
            var j = new Date().getFullYear() + '-01-01';
            if (o.swap4) { to.value = j; from.value = t; } else { from.value = j; to.value = t; }
        } else if (typeId === 5) { if (yearStart) from.value = S.isoDay(yearStart); if (o.to5) to.value = t; }
    };

    /* the comma led id string the desktop builds from the checked branch names */
    S.branchIds = function (sel) { var s = ''; A.checkedValues(sel).forEach(function (v) { s += ',' + v; }); return s; };
    S.fillBranches = function (sel, rows, userBranchId) {
        var list = rows || [];
        A.fill(sel, list.map(function (r) {
            var id = A.ci(r, 'BranchId'); if (id === undefined) id = A.ci(r, 'Id'); if (id === undefined) id = r[Object.keys(r)[0]];
            return { Id: id, name: A.ci(r, 'BranchName') };
        }), function (r) { return r.Id; }, function (r) { return r.name; });
        for (var i = 0; i < sel.options.length; i++) sel.options[i].selected = String(sel.options[i].value) === String(userBranchId);
        sel.dispatchEvent(new Event('change', { bubbles: true }));
    };

    // ------------------------------------------------------------------ misc
    S.digitsOnly = function (id, decimal) {                 // OnlytextNumberFunction
        var x = A.el(id); if (!x) return;
        var re = decimal ? /[^\d.]/g : /\D/g;
        x.addEventListener('keypress', function (e) { if (e.key.length === 1 && !e.ctrlKey && !(decimal ? /[\d.]/ : /\d/).test(e.key)) e.preventDefault(); });
        x.addEventListener('input', function () { var v = this.value.replace(re, ''); if (v !== this.value) this.value = v; });
    };
    S.post = function (url, body) {
        return A.getJson(url, { method: 'POST', headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' }, body: JSON.stringify(body) });
    };
    S.qs = function (o) {
        var q = new URLSearchParams();
        Object.keys(o).forEach(function (k) { if (o[k] !== undefined && o[k] !== null && o[k] !== '') q.set(k, o[k]); });
        return q.toString();
    };
    /* Ctrl+key shortcuts of the desktop KeyPreview handler. map: { 's': fn, 'ArrowDown': fn ... }, esc: fn */
    S.keys = function (map, esc, dialogId) {
        d.addEventListener('keydown', function (e) {
            var dlg = A.el(dialogId || 'shortcutDialog');
            if (dlg && dlg.open) return;
            if (e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt')) { if (map.alt) { e.preventDefault(); map.alt(); } return; }
            if (e.key === 'Escape' && !A.comboOpen()) { if (esc) { e.preventDefault(); esc(); } return; }
            if (!e.ctrlKey || e.altKey) return;
            var k = e.key.length === 1 ? e.key.toLowerCase() : e.key;
            if (k === 'F5' || k === 'f5') k = 'F5';
            if (map[k]) { e.preventDefault(); map[k](); }
        });
    };
    S.focus = function (id) { var f = A.focusable(id); if (f) f.focus(); };
    S.closeShortcuts = function () {
        var b = A.el('closeShortcuts'); if (b) b.addEventListener('click', function () { A.el('shortcutDialog').close(); });
    };
    /* ToolStripDropDownButton: the button toggles the menu; items(): names; onPick(name) */
    S.dropdown = function (btnId, menuId, items, onPick) {
        var b = A.el(btnId), m = A.el(menuId);
        function fill() {
            m.innerHTML = '';
            var list = items() || [];
            if (!list.length) { var n = d.createElement('div'); n.className = 'none'; n.textContent = '(no reports)'; m.appendChild(n); return; }
            list.forEach(function (name) {
                var x = d.createElement('button'); x.type = 'button'; x.textContent = name;
                x.onclick = function () { m.hidden = true; onPick(name); };
                m.appendChild(x);
            });
        }
        b.addEventListener('click', function (e) { e.stopPropagation(); if (m.hidden) { fill(); m.hidden = false; } else m.hidden = true; });
        d.addEventListener('click', function () { m.hidden = true; });
        return { close: function () { m.hidden = true; }, toggle: function () { b.click(); } };
    };
    S.measure = (function () {
        var c = d.createElement('canvas').getContext('2d');
        return function (text, bold) { c.font = (bold ? 'bold ' : '') + '11px Verdana'; return c.measureText(text).width; };
    }());

    // ====================================================================== Grid
    /* cfg: tableId, gridId, navId, navTextId, headerLines (2), places(): amount decimals (unused when fmt given), frozen (n),
            onLink(row, col), onButton(row, col), onSelect(row), rowStyle(row) -> {color,bold}, autosize (bool),
            checkSelect (true: the Select column drives checkedRows)
       column: { key, caption, width, hidden, link, button: 'Print', check: true, fmt, totalFmt, date: 'dd-MMM-yy',
                 sum, num (right aligned number), align: 'l'|'c'|'r', auto }                                                       */
    S.Grid = function (cfg) {
        var g = this;
        g.cfg = cfg; g.columns = null; g.rows = []; g.filters = {}; g.sortKey = ''; g.sortDir = 1; g.selected = -1; g.flat = [];
        var nav = A.el(cfg.navId);
        if (nav) nav.addEventListener('click', function (e) {
            var b = e.target.closest('[data-nav]'); if (!b || !g.flat.length) return;
            var n = b.getAttribute('data-nav');
            g.select(n === 'first' ? 0 : n === 'last' ? g.flat.length - 1 : n === 'prev' ? Math.max(0, g.selected - 1) : Math.min(g.flat.length - 1, g.selected + 1));
        });
        A.el(cfg.gridId).addEventListener('keydown', function (e) {
            if (e.target.tagName === 'INPUT' && e.target.type !== 'checkbox') return;
            if (e.ctrlKey && (e.code === 'Space' || e.key === ' ')) {            // Ctrl+Space: the link of the current row
                e.preventDefault();
                if (g.selected >= 0 && g.flat[g.selected] && cfg.onLink) cfg.onLink(g.flat[g.selected].row, g.firstLinkColumn());
                return;
            }
            if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
                if (e.ctrlKey) return;
                e.preventDefault(); if (!g.flat.length) return;
                g.select(g.selected < 0 ? 0 : Math.max(0, Math.min(g.flat.length - 1, g.selected + (e.key === 'ArrowDown' ? 1 : -1))));
            }
        });
    };
    var G = S.Grid.prototype;
    G.firstLinkColumn = function () { return (this.columns || []).filter(function (c) { return c.link && !c.hidden; })[0]; };
    G.text = function (c, row) {
        var v = A.ci(row, c.key);
        if (c.fmt && !c.date) return v === null || v === undefined || v === '' ? '' : S.fmtNum(v, c.fmt);
        if (c.date) return S.fmtDate(v, c.date);
        if (v === null || v === undefined) return '';
        if (c.num) return S.fmtNum(v, c.fmt || '#,##0.##');
        return String(v);
    };
    G.isNum = function (c) { return !!(c.num || (c.fmt && !c.date)); };
    G.setData = function (columns, rows) {
        this.columns = columns; this.rows = rows || [];
        this.rows.forEach(function (r) { if (r._chk === undefined) r._chk = false; });
        this.filters = {}; this.selected = -1; this.sortKey = '';
        this.autoWidths();
        this.render();
    };
    G.clear = function () { this.columns = null; this.rows = []; this.filters = {}; this.selected = -1; this.render(); };    // ClearStructure
    G.hasData = function () { return !!this.columns; };
    G.visibleCols = function () { return (this.columns || []).filter(function (c) { return !c.hidden; }); };
    /* GridAutoAdjustment: best fit on every column but the last */
    G.autoWidths = function () {
        if (!this.cfg.autosize) return;
        var g = this, cols = this.visibleCols();
        cols.forEach(function (c, i) {
            if (i === cols.length - 1 || c.button || c.check) return;
            if (c.width && !c.auto) return;
            var best = S.measure(c.caption || '', true) * 0.6;
            g.rows.forEach(function (r) { var t = g.text(c, r); var m = S.measure(t); if (m > best) best = m; });
            c.width = Math.max(40, Math.min(420, Math.ceil(best) + 18));
        });
    };
    G.visibleRows = function () {
        var g = this, cols = this.columns || [];
        var keys = Object.keys(this.filters).filter(function (k) { return g.filters[k]; });
        var out = this.rows.filter(function (r) {
            return keys.every(function (k) {
                var c = cols.filter(function (x) { return x.key === k; })[0];
                return c && g.text(c, r).toLowerCase().indexOf(g.filters[k].toLowerCase()) >= 0;
            });
        });
        if (this.sortKey) {
            var sc = cols.filter(function (x) { return x.key === g.sortKey; })[0];
            if (sc) out = out.slice().sort(function (a, b) {
                var x = A.ci(a, g.sortKey), y = A.ci(b, g.sortKey);
                if (g.isNum(sc)) return g.sortDir * (A.toDouble(x) - A.toDouble(y));
                return g.sortDir * String(x == null ? '' : x).localeCompare(String(y == null ? '' : y));
            });
        }
        return out;
    };
    G.checkedRows = function () { return this.rows.filter(function (r) { return r._chk; }); };
    G.widthOf = function (c) { return c.width || 100; };
    G.render = function () {
        var g = this, cfg = this.cfg, t = A.el(cfg.tableId), cg = t.querySelector('colgroup'), th = t.tHead, tb = t.tBodies[0], tf = t.tFoot;
        cg.innerHTML = ''; th.innerHTML = ''; tb.innerHTML = ''; tf.innerHTML = ''; this.flat = [];
        t.style.setProperty('--hh', ((cfg.headerLines || 2) * 16) + 'px');
        var nav = A.el(cfg.navId); if (nav) nav.hidden = !this.columns;
        if (!this.columns) { this.navText(); return; }
        var cols = this.visibleCols(), frozen = cfg.frozen || 0, left = [], acc = 0;
        cols.forEach(function (c, i) { left[i] = acc; acc += g.widthOf(c); var col = d.createElement('col'); col.style.width = g.widthOf(c) + 'px'; cg.appendChild(col); });
        function freeze(el, i) { if (i < frozen) { el.classList.add('frz'); el.style.left = left[i] + 'px'; } }
        var rows = this.visibleRows();
        var hr = th.insertRow();
        cols.forEach(function (c, i) {
            var h = d.createElement('th'); h.setAttribute('data-col', c.key); freeze(h, i);
            if (c.check) {
                var all = d.createElement('input'); all.type = 'checkbox'; all.title = 'Select all';
                all.checked = rows.length > 0 && rows.every(function (r) { return r._chk; });
                all.onchange = function () { rows.forEach(function (r) { r._chk = all.checked; }); g.render(); };
                h.appendChild(all);
            } else {
                h.textContent = c.caption; h.title = c.caption;
                if (g.sortKey === c.key) { var s = d.createElement('span'); s.className = 'sort'; s.textContent = g.sortDir > 0 ? '▲' : '▼'; h.appendChild(s); }
                if (!c.button) h.onclick = function () { g.sortDir = g.sortKey === c.key ? -g.sortDir : 1; g.sortKey = c.key; g.render(); };
            }
            hr.appendChild(h);
        });
        var fr = cfg.noFilter ? null : th.insertRow(); if (fr) fr.className = 'flt';
        if (fr) cols.forEach(function (c, i) {
            var td = fr.insertCell(); freeze(td, i);
            if (c.button || c.check) return;
            var inp = d.createElement('input');
            inp.value = g.filters[c.key] || ''; inp.setAttribute('aria-label', 'Filter ' + c.caption); inp.dataset.key = c.key;
            inp.oninput = function () {
                var pos = inp.selectionStart; g.filters[c.key] = inp.value; g.selected = -1; g.render();
                var again = th.querySelector('input[data-key="' + c.key + '"]'); if (again) { again.focus(); again.setSelectionRange(pos, pos); }
            };
            td.appendChild(inp);
        });
        rows.forEach(function (r, n) {
            var tr = tb.insertRow(), idx = g.flat.length; tr.className = 'r' + ((n % 2) ? ' alt' : ''); tr.tabIndex = -1; g.flat.push({ row: r, tr: tr });
            if (idx === g.selected) tr.classList.add('sel');
            var st = cfg.rowStyle ? cfg.rowStyle(r) : null;
            cols.forEach(function (c, i) {
                var td = tr.insertCell(), text = '';
                freeze(td, i);
                if (c.check) {
                    var cb = d.createElement('input'); cb.type = 'checkbox'; cb.checked = !!r._chk; td.className = 'ctr';
                    cb.onchange = function () { r._chk = cb.checked; g.render(); };
                    td.appendChild(cb); return;
                }
                if (c.button) {
                    var b = d.createElement('button'); b.type = 'button'; b.className = 'cellbtn'; b.textContent = c.button;
                    b.onclick = function (e) { e.stopPropagation(); g.select(idx); if (cfg.onButton) cfg.onButton(r, c); };
                    td.className = 'ctr'; td.appendChild(b); return;
                }
                text = g.text(c, r);
                if (c.link && text !== '') {
                    var a = d.createElement('a'); a.className = 'lnk'; a.textContent = text; a.href = '#';
                    a.onclick = function (e) { e.preventDefault(); g.select(idx); if (cfg.onLink) cfg.onLink(r, c); };
                    td.appendChild(a);
                } else td.textContent = text;
                var al = c.align || (g.isNum(c) ? 'r' : 'l');
                if (al === 'r') td.classList.add('num'); else if (al === 'c') td.classList.add('ctr');
                if (st) { if (st.color) td.style.color = st.color; if (st.bold) td.style.fontWeight = 'bold'; }
                td.title = text;
            });
            tr.onmousedown = function () { g.select(idx); };
        });
        var tot = d.createElement('tr'); tot.className = 'tot';
        cols.forEach(function (c, i) {
            var td = d.createElement('td'); freeze(td, i);
            if (c.sum) {
                var s = 0; rows.forEach(function (r) { s += A.toDouble(A.ci(r, c.key)); });
                td.className = 'num'; td.classList.add('tt'); td.textContent = S.fmtNum(s, c.totalFmt || c.fmt || '#,##0.##');
            } else if (i === 0 && !c.button && !c.check) td.textContent = 'Total';
            tot.appendChild(td);
        });
        if (!cfg.noTotal) tf.appendChild(tot);
        if (this.selected >= this.flat.length) this.selected = this.flat.length ? this.flat.length - 1 : -1;
        this.navText();
    };
    G.select = function (i) {
        if (i < 0 || i >= this.flat.length) return;
        var changed = i !== this.selected;
        if (this.selected >= 0 && this.flat[this.selected]) this.flat[this.selected].tr.classList.remove('sel');
        this.selected = i; this.flat[i].tr.classList.add('sel');
        var gd = A.el(this.cfg.gridId), tr = this.flat[i].tr;
        var top = tr.offsetTop - 54, bottom = tr.offsetTop + tr.offsetHeight - gd.clientHeight + 24;
        if (gd.scrollTop > top) gd.scrollTop = top; else if (gd.scrollTop < bottom) gd.scrollTop = bottom;
        this.navText();
        if (changed && this.cfg.onSelect) this.cfg.onSelect(this.flat[i].row);
    };
    G.navText = function () {
        var nt = A.el(this.cfg.navTextId);
        if (nt) nt.textContent = this.columns ? ('Record: ' + (this.selected >= 0 ? this.selected + 1 : 0) + ' of ' + this.flat.length) : '';
    };
    G.focus = function () { A.el(this.cfg.gridId).focus(); if (this.selected < 0 && this.flat.length) this.select(0); };
    G.current = function () { return this.selected >= 0 && this.flat[this.selected] ? this.flat[this.selected].row : null; };
}(window, document));
