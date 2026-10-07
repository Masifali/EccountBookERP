/* ============================================================================================
 * Group R1 (Account Reports 48 / 63 / 70 / 68) - shared page engine.
 *   window.AcRpt1 = { el, busy, getJson, today, ci, netFormat, fmtDate, toInt, toDouble, checkedValues, openPdf,
 *                     shortcuts, Grid }
 * AcRpt1.Grid is the Janus GridEX look the four desktop forms configure: HeaderLines 2 (centre), FilterMode Automatic
 * (filter row, Contains), AlternatingColors, optional Groups.Add(<column>) with HideWhenGrouped, GroupTotals Always,
 * TotalRow bottom-fixed with AggregateFunction Sum, RecordNavigator, link column (ColumnType Link).
 * Number display = clsGlobalVariables.stringFormatboth "#,##0.<n zeros>;(0,0.<n zeros>); 0" with n = DefaultNoofDecimalPointsForAmount.
 * Sums are plain double sums (the desktop columns are double).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1 = {};
    var MON = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    A.el = function (id) { return d.getElementById(id); };
    A.busy = function (btn, on) {
        if (!btn) return;
        btn.disabled = !!on;
        btn.classList.toggle('busy', !!on);
        if (on) btn.setAttribute('aria-busy', 'true'); else btn.removeAttribute('aria-busy');
    };
    A.getJson = function (url, opts) {
        return fetch(url, Object.assign({ credentials: 'same-origin', headers: { 'Accept': 'application/json' } }, opts || {}))
            .then(function (r) {
                if (!r.ok) return r.text().then(function (t) {
                    var m = t; try { var j = JSON.parse(t); m = j.message || j.error || t; } catch (e) { }
                    throw new Error(m || ('Request failed (' + r.status + ')'));
                });
                return r.json();
            });
    };
    A.today = function () {
        var t = new Date();
        return t.getFullYear() + '-' + ('0' + (t.getMonth() + 1)).slice(-2) + '-' + ('0' + t.getDate()).slice(-2);
    };
    A.addDays = function (iso, n) {
        var p = iso.split('-'), t = new Date(+p[0], +p[1] - 1, +p[2] + n);
        return t.getFullYear() + '-' + ('0' + (t.getMonth() + 1)).slice(-2) + '-' + ('0' + t.getDate()).slice(-2);
    };
    A.ci = function (row, key) {
        if (!row) return undefined;
        if (key in row) return row[key];
        var low = key.toLowerCase();
        for (var k in row) if (k.toLowerCase() === low) return row[k];
        return undefined;
    };
    /* Conversion.ToInt: Convert.ToInt32 (banker's rounding for a number), any failure / null -> 0 */
    A.toInt = function (v) {
        if (v === null || v === undefined || v === '') return 0;
        var n = typeof v === 'number' ? v : Number(String(v).trim());
        if (!isFinite(n)) return 0;
        var f = Math.floor(n), diff = n - f, r;
        if (diff > 0.5) r = f + 1; else if (diff < 0.5) r = f; else r = (f % 2 === 0) ? f : f + 1;
        return (r > 2147483647 || r < -2147483648) ? 0 : r;
    };
    /* Convert.ToInt32(string): whole numbers only (a text with a decimal point throws -> 0) */
    A.toIntText = function (s) {
        s = String(s == null ? '' : s).trim();
        if (!/^[+-]?\d+$/.test(s)) return 0;
        var n = Number(s);
        return (n > 2147483647 || n < -2147483648) ? 0 : n;
    };
    A.toDouble = function (v) {
        if (v === null || v === undefined || v === '') return 0;
        var n = typeof v === 'number' ? v : Number(String(v).trim());
        return isFinite(n) ? n : 0;
    };

    function group3(intPart, minDigits) {
        while (intPart.length < minDigits) intPart = '0' + intPart;
        return intPart.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    /* "#,##0.<zeros>;(0,0.<zeros>); 0" - optional = false (stringFormatboth) */
    A.netFormat = function (value, places) {
        var n = Number(value);
        if (!isFinite(n)) return value == null ? '' : String(value);
        var abs = Math.abs(n), s = abs.toFixed(places), parts = s.split('.');
        var frac = parts[1] || '';
        if (Number(s) === 0) return '0';
        if (n < 0) return '(' + group3(parts[0], 2) + (frac ? '.' + frac : '') + ')';
        return group3(parts[0], 1) + (frac ? '.' + frac : '');
    };
    /* dd-MMM-yy (FormatString) from yyyy-MM-dd[Thh:mm:ss] */
    A.fmtDate = function (v, longYear) {
        if (v === null || v === undefined || v === '') return '';
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v));
        if (!m) return String(v);
        return m[3] + '-' + MON[+m[2] - 1] + '-' + (longYear ? m[1] : m[1].slice(2));
    };
    /* the checked values of a <select multiple data-dtcombo-checked>, in list order */
    A.checkedValues = function (sel) {
        var out = [];
        if (!sel) return out;
        for (var i = 0; i < sel.options.length; i++) if (sel.options[i].selected && sel.options[i].value !== '') out.push(sel.options[i].value);
        return out;
    };
    A.checkedTexts = function (sel) {
        var out = [];
        if (!sel) return out;
        for (var i = 0; i < sel.options.length; i++) if (sel.options[i].selected && sel.options[i].value !== '') out.push(sel.options[i].textContent.trim());
        return out;
    };
    /* fill a <select>: first option = empty slot (AllowNull, ZeroIndex false), then rows */
    A.fill = function (sel, rows, valueFn, textFn, keepSelected) {
        var old = keepSelected ? A.checkedValues(sel).concat(sel.multiple ? [] : [sel.value]) : [];
        while (sel.options.length) sel.remove(0);
        if (!sel.multiple) sel.add(new Option('', ''));
        (rows || []).forEach(function (r) {
            var o = new Option(String(textFn(r) == null ? '' : textFn(r)), String(valueFn(r)));
            if (old.indexOf(o.value) >= 0 && o.value !== '') o.selected = true;
            sel.add(o);
        });
        if (!sel.multiple) { var keep = old.filter(function (x) { return x !== ''; })[0]; sel.value = keep || ''; if (sel.value !== (keep || '')) sel.value = ''; }
        sel.dispatchEvent(new Event('change', { bubbles: true }));
    };

    /* Reporting.ShowReportWithDataTable -> the server renders the template over the same procedure rows (new tab) */
    A.openPdf = function (url) {
        var win = w.open('', '_blank');
        return fetch(url, { credentials: 'same-origin' }).then(function (r) {
            if (!r.ok) return r.text().then(function (t) { throw new Error(t || ('Print failed (' + r.status + ')')); });
            return r.blob();
        }).then(function (b) {
            var u = URL.createObjectURL(new Blob([b], { type: 'application/pdf' }));
            if (win) win.location.href = u; else w.open(u, '_blank');
        }).catch(function (e) { if (win) win.close(); throw e; });
    };

    /* ShortCutKeyPopUp(dt) */
    A.shortcuts = function (rows) {
        var dlg = A.el('shortcutDialog'); if (!dlg || dlg.open) return;
        var tb = A.el('shortcutRows'); tb.innerHTML = '';
        rows.forEach(function (s) { var tr = tb.insertRow(); tr.insertCell().textContent = s[0]; tr.insertCell().textContent = s[1]; });
        if (dlg.showModal) dlg.showModal(); else dlg.setAttribute('open', '');
    };
    A.comboOpen = function () {
        return Array.prototype.some.call(d.querySelectorAll('.dtcombo-pop'), function (x) { return x.style.display === 'block'; });
    };
    /* the focusable inner element of a (possibly combo-enhanced) control */
    A.focusable = function (id) {
        var x = A.el(id); if (!x) return null;
        if (x.tagName === 'SELECT') { var wrap = x.closest('.dtcombo-wrap'); return wrap ? wrap.querySelector('.dtcombo-input') : x; }
        return x;
    };

    // ====================================================================== Janus-style grid
    /* cfg: tableId, gridId, navId, navTextId, groupBy (column key | null), groupTotals, decimals(): int, onLink(row, col),
            cellStyle(col, row) -> {color, bold}, sort: {key, dir} initial
       column: {key, caption, width, type: 'amount'|'int'|'str'|'date'|'text', align: 'l'|'c'|'r', hidden, link, sum, fmt(v,row) } */
    A.Grid = function (cfg) {
        var g = this;
        g.cfg = cfg; g.columns = null; g.rows = []; g.filters = {}; g.sortKey = (cfg.sort && cfg.sort.key) || ''; g.sortDir = (cfg.sort && cfg.sort.dir) || 1;
        g.collapsed = {}; g.selected = -1; g.flat = [];
        var nav = A.el(cfg.navId);
        if (nav) nav.addEventListener('click', function (e) {
            var b = e.target.closest('[data-nav]'); if (!b || !g.flat.length) return;
            var n = b.getAttribute('data-nav');
            g.select(n === 'first' ? 0 : n === 'last' ? g.flat.length - 1 : n === 'prev' ? Math.max(0, g.selected - 1) : Math.min(g.flat.length - 1, g.selected + 1));
        });
        A.el(cfg.gridId).addEventListener('keydown', function (e) {
            if (e.target.tagName === 'INPUT') return;
            if (e.ctrlKey && (e.code === 'Space' || e.key === ' ')) {     // Ctrl+Space: the link of the current row
                e.preventDefault();
                if (g.selected >= 0 && g.flat[g.selected] && cfg.onLink) cfg.onLink(g.flat[g.selected].row);
                return;
            }
            if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
                if (e.ctrlKey) return;
                e.preventDefault(); if (!g.flat.length) return;
                g.select(g.selected < 0 ? 0 : Math.max(0, Math.min(g.flat.length - 1, g.selected + (e.key === 'ArrowDown' ? 1 : -1))));
            }
        });
    };
    var G = A.Grid.prototype;
    G.places = function () { return (this.cfg.decimals ? this.cfg.decimals() : 0) || 0; };
    G.fmt = function (c, row) {
        var v = A.ci(row, c.key);
        if (c.fmt) return c.fmt(v, row);
        if (v === null || v === undefined || v === '') return '';
        if (c.type === 'amount') return A.netFormat(v, this.places());
        if (c.type === 'date') return A.fmtDate(v);
        return String(v);
    };
    G.isNum = function (c) { return c.type === 'amount' || c.type === 'int'; };
    G.setData = function (columns, rows) {
        this.columns = columns; this.rows = rows || [];
        this.filters = {}; this.collapsed = {}; this.selected = -1;
        this.render();
    };
    G.clear = function () { this.columns = null; this.rows = []; this.filters = {}; this.selected = -1; this.render(); };   // ClearStructure
    G.hasData = function () { return !!this.columns; };
    G.visibleCols = function () { return (this.columns || []).filter(function (c) { return !c.hidden; }); };
    G.visibleRows = function () {
        var g = this, cols = this.columns || [];
        var keys = Object.keys(this.filters).filter(function (k) { return g.filters[k]; });
        var out = this.rows.filter(function (r) {
            return keys.every(function (k) {
                var c = cols.filter(function (x) { return x.key === k; })[0];
                return c && g.fmt(c, r).toLowerCase().indexOf(g.filters[k].toLowerCase()) >= 0;      // DefaultFilterRowComparison Contains
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
    G.totalsRow = function (cls, rows, tag) {
        var g = this, tr = d.createElement('tr'); tr.className = cls;
        this.visibleCols().forEach(function (c, i) {
            var td = d.createElement('td');
            if (c.sum) {
                var s = 0; rows.forEach(function (r) { s += A.toDouble(A.ci(r, c.key)); });
                td.className = 'num'; td.textContent = c.type === 'amount' ? A.netFormat(s, g.places()) : String(s);
            } else if (i === 0 && tag) td.textContent = tag;
            tr.appendChild(td);
        });
        return tr;
    };
    G.render = function () {
        var g = this, cfg = this.cfg, t = A.el(cfg.tableId), cg = t.querySelector('colgroup'), th = t.tHead, tb = t.tBodies[0], tf = t.tFoot;
        cg.innerHTML = ''; th.innerHTML = ''; tb.innerHTML = ''; tf.innerHTML = ''; this.flat = [];
        var nav = A.el(cfg.navId); if (nav) nav.hidden = !this.columns;
        if (!this.columns) { this.navText(); return; }
        var cols = this.visibleCols();
        cols.forEach(function (c) { var col = d.createElement('col'); col.style.width = (c.width || 100) + 'px'; cg.appendChild(col); });
        var hr = th.insertRow();                                                              // HeaderLines 2, HeaderAlignment Center
        cols.forEach(function (c) {
            var h = d.createElement('th'); h.textContent = c.caption; h.title = c.caption; h.setAttribute('data-col', c.key);
            if (g.sortKey === c.key) { var s = d.createElement('span'); s.className = 'sort'; s.textContent = g.sortDir > 0 ? '▲' : '▼'; h.appendChild(s); }
            h.onclick = function () { g.sortDir = g.sortKey === c.key ? -g.sortDir : 1; g.sortKey = c.key; g.render(); };
            hr.appendChild(h);
        });
        var fr = th.insertRow(); fr.className = 'flt';                                       // FilterMode Automatic
        cols.forEach(function (c) {
            var td = fr.insertCell(), inp = d.createElement('input');
            inp.value = g.filters[c.key] || ''; inp.setAttribute('aria-label', 'Filter ' + c.caption); inp.dataset.key = c.key;
            inp.oninput = function () {
                var pos = inp.selectionStart; g.filters[c.key] = inp.value; g.selected = -1; g.render();
                var again = th.querySelector('input[data-key="' + c.key + '"]'); if (again) { again.focus(); again.setSelectionRange(pos, pos); }
            };
            td.appendChild(inp);
        });
        var rows = this.visibleRows(), n = 0;
        var addRow = function (r) {
            var tr = tb.insertRow(), idx = g.flat.length; tr.className = 'r' + ((n++ % 2) ? ' alt' : ''); tr.tabIndex = -1; g.flat.push({ row: r, tr: tr });
            if (idx === g.selected) tr.classList.add('sel');
            cols.forEach(function (c) {
                var td = tr.insertCell(), text = g.fmt(c, r);
                if (c.link) {
                    var a = d.createElement('a'); a.className = 'lnk'; a.textContent = text; a.href = '#';
                    a.onclick = function (e) { e.preventDefault(); g.select(idx); if (cfg.onLink) cfg.onLink(r, c); }; td.appendChild(a);
                } else td.textContent = text;
                var al = c.align || (g.isNum(c) ? 'r' : 'l');
                if (al === 'r') td.className = 'num'; else if (al === 'c') td.className = 'ctr';
                var st = cfg.cellStyle ? cfg.cellStyle(c, r) : null;
                if (st) { if (st.color) td.style.color = st.color; if (st.bold) td.style.fontWeight = 'bold'; }
                td.title = text;
            });
            tr.onmousedown = function () { g.select(idx); };
        };
        if (cfg.groupBy) {
            var groups = {}, order = [], gc = this.columns.filter(function (x) { return x.key === cfg.groupBy; })[0];
            rows.forEach(function (r) { var k = String(A.ci(r, cfg.groupBy) == null ? '' : A.ci(r, cfg.groupBy)); if (!groups[k]) { groups[k] = []; order.push(k); } groups[k].push(r); });
            order.sort(function (a, b) { return a.localeCompare(b); });                                   // Groups.Add - ascending
            order.forEach(function (k) {
                var gr = tb.insertRow(); gr.className = 'grp';
                var cell = gr.insertCell(); cell.colSpan = cols.length;
                var tg = d.createElement('span'); tg.className = 'tg'; tg.textContent = g.collapsed[k] ? '+' : '−';
                cell.appendChild(tg); cell.appendChild(d.createTextNode((gc ? gc.caption : cfg.groupBy) + ': ' + k));
                gr.onclick = function () { g.collapsed[k] = !g.collapsed[k]; g.render(); };
                if (!g.collapsed[k]) groups[k].forEach(addRow);
                if (cfg.groupTotals) tb.appendChild(g.totalsRow('gtot', groups[k], ''));                // GroupTotals Always
            });
        } else rows.forEach(addRow);
        tf.appendChild(this.totalsRow('tot', rows, 'Total'));                                          // TotalRow
        if (this.selected >= this.flat.length) this.selected = this.flat.length ? this.flat.length - 1 : -1;
        this.navText();
    };
    G.select = function (i) {
        if (i < 0 || i >= this.flat.length) return;
        if (this.selected >= 0 && this.flat[this.selected]) this.flat[this.selected].tr.classList.remove('sel');
        this.selected = i; this.flat[i].tr.classList.add('sel');
        var gd = A.el(this.cfg.gridId), tr = this.flat[i].tr;
        var top = tr.offsetTop - 54, bottom = tr.offsetTop + tr.offsetHeight - gd.clientHeight + 24;
        if (gd.scrollTop > top) gd.scrollTop = top; else if (gd.scrollTop < bottom) gd.scrollTop = bottom;
        this.navText();
    };
    G.navText = function () {
        var nt = A.el(this.cfg.navTextId);
        if (nt) nt.textContent = this.columns ? ('Record: ' + (this.selected >= 0 ? this.selected + 1 : 0) + ' of ' + this.flat.length) : '';
    };
    G.focus = function () { A.el(this.cfg.gridId).focus(); if (this.selected < 0 && this.flat.length) this.select(0); };
    G.current = function () { return this.selected >= 0 && this.flat[this.selected] ? this.flat[this.selected].row : null; };
}(window, document));
