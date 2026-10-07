/* ============================================================================================
 * AcRpt4 - shared pieces of the five R4 report pages (screens 56, 76, 87, 932, 933). New file; nothing existing is touched.
 * Same helpers as AcRpt2 (busy, getJson, netFmt, sumDec, shortcuts, postGrid, fillSelect ...) plus a fuller Janus GridEX look-alike:
 *   AcRpt4.createGrid(opts)  filter row, header lines + alignment, GroupByBox (drag a header into the bar; chips remove it), multi-level
 *                            groups with GroupTotals, bottom total row (sum / avg), record navigator, link columns, SelectionChanged
 *                            (onSelect), FormatConditions (cellStyle), alternating colours.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt4 = w.AcRpt4 || {};
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

    A.ci = function (row, key) {
        if (!row) return undefined;
        if (key in row) return row[key];
        var low = String(key).toLowerCase();
        for (var k in row) if (k.toLowerCase() === low) return row[k];
        return undefined;
    };

    A.pad2 = function (n) { return ('0' + n).slice(-2); };
    A.ymd = function (dt) { return dt.getFullYear() + '-' + A.pad2(dt.getMonth() + 1) + '-' + A.pad2(dt.getDate()); };
    A.today = function () { return A.ymd(new Date()); };
    A.addDays = function (iso, n) {
        var p = /^(\d{4})-(\d{2})-(\d{2})/.exec(iso); if (!p) return iso;
        var dt = new Date(+p[1], +p[2] - 1, +p[3] + n); return A.ymd(dt);
    };
    A.intArg = function (v) { var n = parseInt(String(v == null ? '' : v).trim(), 10); return isNaN(n) ? 0 : n; };   // Conversion.ToInt

    /* dd-MMM-yy / dd-MMM-yyyy */
    A.fmtDate = function (v, full) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v == null ? '' : v));
        if (!m) return v == null ? '' : String(v);
        return m[3] + '-' + MON[+m[2] - 1] + '-' + (full === false ? m[1].slice(2) : m[1]);
    };

    // ------------------------------------------------------------------ .NET custom numeric formats
    function groupDigits(s, minDigits, group) {
        while (s.length < minDigits) s = '0' + s;
        return group ? s.replace(/\B(?=(\d{3})+(?!\d))/g, ',') : s;
    }
    function sectionOf(pattern) {
        var out = [], cur = '';
        for (var i = 0; i < pattern.length; i++) { if (pattern.charAt(i) === ';') { out.push(cur); cur = ''; } else cur += pattern.charAt(i); }
        out.push(cur);
        return out;
    }
    function roundTo(abs, places) {
        // decimal-exact rounding (away from zero) on the plain text of the number
        var s = typeof abs === 'string' ? abs : String(abs);
        if (/e/i.test(s)) s = Number(abs).toFixed(Math.max(places, 0) + 2);
        var p = s.split('.'), ip = p[0] || '0', fp = p[1] || '';
        while (fp.length < places + 1) fp += '0';
        var keep = fp.slice(0, places), next = fp.charAt(places);
        var digits = ip + keep, n = BigInt(digits || '0');
        if (next >= '5') n += BigInt(1);
        var t = n.toString();
        while (t.length < places + 1) t = '0' + t;
        return places > 0 ? t.slice(0, t.length - places) + '.' + t.slice(t.length - places) : t;
    }
    function applyPattern(sec, abs) {
        // locate the first run made of # 0 , .
        var idx = -1, i;
        for (i = 0; i < sec.length; i++) { if ('#0,.'.indexOf(sec.charAt(i)) >= 0) { idx = i; break; } }
        if (idx < 0) return { text: sec, zero: false };
        var j = idx; while (j < sec.length && '#0,.'.indexOf(sec.charAt(j)) >= 0) j++;
        var core = sec.slice(idx, j), pre = sec.slice(0, idx), post = sec.slice(j);
        var dot = core.indexOf('.');
        var ipat = dot >= 0 ? core.slice(0, dot) : core, fpat = dot >= 0 ? core.slice(dot + 1) : '';
        var places = fpat.length, minFrac = (fpat.match(/0/g) || []).length, minInt = (ipat.match(/0/g) || []).length;
        var grp = ipat.indexOf(',') >= 0 && ipat.replace(/,/g, '').length > 0;
        var r = roundTo(abs, places), rp = r.split('.'), ip = rp[0], fp = rp[1] || '';
        var isZero = /^0*$/.test(ip) && /^0*$/.test(fp);
        while (fp.length > minFrac && fp.charAt(fp.length - 1) === '0') fp = fp.slice(0, -1);
        var ints = groupDigits(ip.replace(/^0+(?=\d)/, ''), minInt, grp);
        if (ints === '0' && minInt === 0) ints = '';
        var num = ints + (fp ? '.' + fp : '');
        return { text: pre + num + post, zero: isZero };
    }
    /** value (number or decimal text) + .NET pattern -> text. */
    A.netFmt = function (value, pattern) {
        if (value === null || value === undefined || value === '') return '';
        var n = Number(value);
        if (!isFinite(n)) return String(value);
        var text = typeof value === 'number' ? (Math.abs(n) < 1e21 && !/e/i.test(String(n)) ? String(Math.abs(n)) : Math.abs(n).toFixed(6)) : String(value).trim().replace(/^[+-]/, '');
        var neg = n < 0;
        var secs = sectionOf(pattern), idx = n === 0 ? (secs.length > 2 ? 2 : 0) : (neg && secs.length > 1 ? 1 : 0);
        var res = applyPattern(secs[idx], text);
        if (res.zero && n !== 0 && secs.length > 2) res = applyPattern(secs[2], '0');    // rounds to zero -> third section
        else if (res.zero && n !== 0) return res.text;
        var out = res.text;
        if (neg && idx === 0 && !res.zero) out = '-' + out;
        return out.trim() === '' ? '' : out;
    };

    /* exact sums of DECIMAL strings (scaled integers) */
    A.sumDec = function (values) {
        var SCALE = 6, total = BigInt(0);
        values.forEach(function (v) {
            if (v === null || v === undefined || v === '') return;
            var s = typeof v === 'number' ? v.toFixed(SCALE) : String(v).trim(), neg = s.charAt(0) === '-';
            if (neg || s.charAt(0) === '+') s = s.slice(1);
            var p = s.split('.'), frac = ((p[1] || '') + '000000').slice(0, SCALE);
            if (!/^\d*$/.test(p[0]) || !/^\d*$/.test(frac)) return;
            var b = BigInt((p[0] || '0') + frac); total += neg ? -b : b;
        });
        var negT = total < 0, a = (negT ? -total : total).toString().padStart(SCALE + 1, '0');
        return (negT ? '-' : '') + a.slice(0, -SCALE) + '.' + a.slice(-SCALE);
    };

    // ------------------------------------------------------------------ ShortCutKeyPopUp
    A.shortcuts = function (rows) {
        var dlg = A.el('shortcutDialog'); if (!dlg || dlg.open) return;
        var tb = A.el('shortcutRows'); tb.innerHTML = '';
        rows.forEach(function (s) { var tr = tb.insertRow(); tr.insertCell().textContent = s[0]; tr.insertCell().textContent = s[1]; });
        if (dlg.showModal) dlg.showModal(); else dlg.setAttribute('open', '');
    };

    // ------------------------------------------------------------------ print: ShowReportWithDataTable
    /** Reporting.ShowReportWithDataTable(dt, rpt): the table's rows go to the converted .rpt (company name/address from the session). */
    A.postGrid = function (rpt, rows, title, btn) {
        var win = w.open('about:blank', '_blank');
        A.busy(btn, true);
        return fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ rpt: rpt, title: title || null, rows: rows }) })
            .then(function (r) {
                var type = r.headers.get('Content-Type') || '';
                if (r.ok && type.indexOf('application/pdf') === 0) return r.blob().then(function (b) { if (win) win.location = URL.createObjectURL(b); });
                return r.text().then(function (t) { if (win) win.close(); var m = t; try { var j = JSON.parse(t); m = j.message || j.error || t; } catch (e) { } alert(m || ('Print failed (' + r.status + ')')); });
            })
            .catch(function (e) { try { if (win) win.close(); } catch (x) { } alert(e.message); })
            .then(function () { A.busy(btn, false); });
    };

    /* The desktop's ComboBox text (comma separated checked captions). */
    A.checkedTexts = function (sel) {
        var out = [];
        for (var i = 0; i < sel.options.length; i++) if (sel.options[i].selected && sel.options[i].value !== '') out.push(sel.options[i].textContent.trim());
        return out;
    };
    A.checkedValues = function (sel) {
        var out = [];
        for (var i = 0; i < sel.options.length; i++) if (sel.options[i].selected && sel.options[i].value !== '') out.push(sel.options[i].value);
        return out;
    };
    /** Fill a <select> (optionally with the "empty" slot); extra = { dataAttr: rowKey }. */
    A.fillSelect = function (sel, rows, valueKey, textKey, opts) {
        opts = opts || {};
        var old = opts.keep === false ? '' : sel.value;
        var oldMulti = sel.multiple ? A.checkedValues(sel) : null;
        while (sel.options.length) sel.remove(0);
        if (!sel.multiple && opts.empty !== false) sel.add(new Option(opts.emptyText || '', opts.emptyValue == null ? '' : String(opts.emptyValue)));
        (rows || []).forEach(function (r) {
            var t = A.ci(r, textKey), o = new Option(t == null ? '' : String(t), String(A.ci(r, valueKey)));
            if (opts.extra) Object.keys(opts.extra).forEach(function (a) { var v = A.ci(r, opts.extra[a]); if (v != null) o.setAttribute('data-' + a, v); });
            sel.add(o);
        });
        if (sel.multiple) { for (var i = 0; i < sel.options.length; i++) sel.options[i].selected = oldMulti && oldMulti.indexOf(sel.options[i].value) >= 0; }
        else { sel.value = old; if (sel.value !== old) sel.value = sel.options.length ? sel.options[0].value : ''; }
    };
    A.clearSelect = function (sel) {
        if (sel.multiple) { for (var i = 0; i < sel.options.length; i++) sel.options[i].selected = false; }
        else sel.value = sel.options.length ? sel.options[0].value : '';
        sel.dispatchEvent(new Event('change', { bubbles: true }));
    };
    A.comboOpen = function () {
        return Array.prototype.some.call(d.querySelectorAll('.dtcombo-pop'), function (x) { return x.style.display === 'block'; });
    };
    A.focusOf = function (id) {
        var x = A.el(id); if (!x) return null;
        if (x.tagName === 'SELECT') { var wrap = x.closest('.dtcombo-wrap'); return wrap ? wrap.querySelector('.dtcombo-input') : x; }
        return x;
    };

    /**
     * GridEX.RetrieveStructure(): one column per table column, in the table's order, typed from the data source
     * (data = {columns, types, rows} as the API returns it). extra = {key: {caption, width, ...}} per-column overrides.
     */
    A.inferColumns = function (data, extra) {
        extra = extra || {};
        return (data.columns || []).map(function (k, i) {
            var t = (data.types || [])[i] || 'text';
            var c = { key: k, caption: k, width: t === 'date' ? 80 : 100, type: t === 'bool' ? 'text' : t };
            if (t === 'num') { c.fmt = '0.########'; c.align = 'right'; }
            if (t === 'int') c.align = 'right';
            return Object.assign(c, extra[k] || {});
        });
    };

    // ------------------------------------------------------------------ Janus-style grid
    /**
     * opts: table, scroller, nav, navText, groupBoxEl (the GroupByBox bar; omit when GroupByBoxVisible = false),
     *       headerLines (2), groupBy (key | [keys] | null, the initial grouping), groupTotals (bool), totalRow (bool),
     *       filterRow (bool), alternate (bool), autoSelect (bool: first record becomes current on bind),
     *       onLink(row, col), onSelect(row) (SelectionChanged), onRowDblClick(row), cellStyle(col, row, raw) -> {color,bold},
     *       rowHeight (px)
     * columns: {key, caption, width, type:'text'|'int'|'date'|'num', fmt, dateFmt:'yy', hidden, link, sum|agg:'sum'|'avg',
     *           totalFmt, align:'left'|'right'|'center', hAlign, wrap}
     */
    A.createGrid = function (opts) {
        var G = { columns: null, rows: [], filters: {}, sortKey: '', sortDir: 1, collapsed: {}, selected: -1, flat: [],
                  groups: opts.groupBy ? [].concat(opts.groupBy) : [] };
        var table = opts.table, scroller = opts.scroller, headerLines = opts.headerLines || 2, gbEl = opts.groupBoxEl || null;
        var HEAD = 16 * headerLines + 2, FLT = 22;

        function colOf(key) { return (G.columns || []).filter(function (x) { return x.key === key; })[0]; }
        function shown() { return (G.columns || []).filter(function (c) { return !c.hidden && G.groups.indexOf(c.key) < 0; }); }
        function text(c, v) {
            if (v === null || v === undefined || v === '') return '';
            if (c.type === 'num' || (c.type === 'int' && c.fmt)) return A.netFmt(v, c.fmt || '#,##0.##');
            if (c.type === 'date') return A.fmtDate(v, c.dateFmt === 'yy' ? false : true);
            return String(v);
        }
        G.text = text;
        function numeric(c) { return c && (c.type === 'num' || c.type === 'int'); }
        function filtered() {
            var keys = Object.keys(G.filters).filter(function (k) { return G.filters[k]; });
            var out = G.rows.filter(function (r) {
                return keys.every(function (k) {
                    var c = colOf(k); if (!c) return true;
                    var t = text(c, A.ci(r, k)).toLowerCase(), f = G.filters[k].toLowerCase().trim();
                    if (c.type === 'date' && /^\d{1,2}-[a-z]{3}-\d{2,4}$/.test(f)) return t === f || t.replace(/-(\d{2})(\d{2})$/, '-$2') === f;
                    return t.indexOf(f) >= 0;
                });
            });
            if (G.sortKey) {
                var sc = colOf(G.sortKey);
                out = out.slice().sort(function (a, b) {
                    var x = A.ci(a, G.sortKey), y = A.ci(b, G.sortKey);
                    if (numeric(sc)) return G.sortDir * (Number(x || 0) - Number(y || 0));
                    return G.sortDir * String(x == null ? '' : x).localeCompare(String(y == null ? '' : y));
                });
            }
            return out;
        }
        function aggregate(c, rows) {
            var vals = rows.map(function (r) { return A.ci(r, c.key); });
            var s = A.sumDec(vals);
            if (c.agg === 'avg') {
                var n = vals.filter(function (v) { return v !== null && v !== undefined && v !== ''; }).length;
                return n ? String(Number(s) / n) : '0';
            }
            return s;
        }
        function totals(cls, rows, cols) {
            var tr = d.createElement('tr'); tr.className = cls;
            cols.forEach(function (c) {
                var td = d.createElement('td');
                if (c.sum || c.agg) { td.className = 'num'; td.textContent = A.netFmt(aggregate(c, rows), c.totalFmt || c.fmt || '#,##0.##'); }
                tr.appendChild(td);
            });
            return tr;
        }
        function drawGroupBox() {
            if (!gbEl) return;
            gbEl.innerHTML = '';
            if (!G.groups.length) { var h = d.createElement('span'); h.className = 'hint'; h.textContent = 'Drag a column header here to group by that column.'; gbEl.appendChild(h); return; }
            G.groups.forEach(function (k, i) {
                var c = colOf(k), chip = d.createElement('span'); chip.className = 'gchip';
                chip.appendChild(d.createTextNode(c ? c.caption : k));
                var x = d.createElement('a'); x.href = '#'; x.textContent = '×'; x.title = 'Remove grouping';
                x.onclick = function (e) { e.preventDefault(); G.groups.splice(i, 1); G.collapsed = {}; G.selected = -1; render(); };
                chip.appendChild(x); gbEl.appendChild(chip);
            });
        }
        function render() {
            var cg = table.querySelector('colgroup'), th = table.tHead, tb = table.tBodies[0], tf = table.tFoot;
            if (!cg) { cg = d.createElement('colgroup'); table.insertBefore(cg, table.firstChild); }
            cg.innerHTML = ''; th.innerHTML = ''; tb.innerHTML = ''; tf.innerHTML = ''; G.flat = [];
            if (opts.nav) opts.nav.hidden = !G.columns;
            drawGroupBox();
            if (!G.columns) { navText(); table.style.width = ''; return; }
            var cols = shown(), total = 0;
            cols.forEach(function (c) { var col = d.createElement('col'); col.style.width = c.width + 'px'; cg.appendChild(col); total += c.width; });
            table.style.width = total + 'px';
            var hr = th.insertRow();
            cols.forEach(function (c) {
                var h = d.createElement('th'); h.textContent = c.caption; h.title = c.caption; h.setAttribute('data-col', c.key);
                h.style.height = HEAD + 'px';
                if (c.hAlign) h.style.textAlign = c.hAlign;
                if (G.sortKey === c.key) { var s = d.createElement('span'); s.className = 'sort'; s.textContent = G.sortDir > 0 ? '▲' : '▼'; h.appendChild(s); }
                h.onclick = function () { G.sortDir = G.sortKey === c.key ? -G.sortDir : 1; G.sortKey = c.key; render(); };
                if (gbEl) {
                    h.draggable = true;
                    h.ondragstart = function (e) { e.dataTransfer.setData('text/plain', c.key); e.dataTransfer.effectAllowed = 'move'; };
                }
                hr.appendChild(h);
            });
            if (opts.filterRow !== false) {
                var fr = th.insertRow(); fr.className = 'flt';
                cols.forEach(function (c) {
                    var td = fr.insertCell(), inp = d.createElement('input');
                    td.style.top = HEAD + 'px';
                    inp.value = G.filters[c.key] || ''; inp.setAttribute('aria-label', 'Filter ' + c.caption); inp.dataset.key = c.key;
                    inp.oninput = function () {
                        var pos = inp.selectionStart; G.filters[c.key] = inp.value; G.selected = -1; render();
                        var again = th.querySelector('input[data-key="' + c.key + '"]'); if (again) { again.focus(); again.setSelectionRange(pos, pos); }
                    };
                    td.appendChild(inp);
                });
            }
            var rows = filtered(), alt = 0;
            function addRows(list) {
                list.forEach(function (r) {
                    var tr = tb.insertRow(), idx = G.flat.length; tr.className = 'r' + ((opts.alternate !== false && (alt++ % 2)) ? ' alt' : ''); tr.tabIndex = -1;
                    if (opts.rowHeight) tr.style.height = opts.rowHeight + 'px';
                    G.flat.push({ row: r, tr: tr });
                    if (idx === G.selected) tr.classList.add('sel');
                    cols.forEach(function (c) {
                        var raw = A.ci(r, c.key), td = tr.insertCell(), t = text(c, raw);
                        if (c.link) { var a = d.createElement('a'); a.className = 'lnk'; a.textContent = t; a.href = '#';
                                      a.onclick = function (e) { e.preventDefault(); select(idx); if (opts.onLink) opts.onLink(r, c); }; td.appendChild(a); }
                        else td.textContent = t;
                        var cls = '';
                        if (c.align === 'right' || (!c.align && c.type === 'num')) cls = 'num';
                        else if (c.align === 'center') cls = 'ctr';
                        if (c.wrap) cls += ' wrap';
                        td.className = cls.trim();
                        if (!c.wrap) td.title = t;
                        if (opts.cellStyle) { var st = opts.cellStyle(c, r, raw); if (st) { if (st.color) td.style.color = st.color; if (st.bold) td.style.fontWeight = 'bold'; } }
                    });
                    tr.onmousedown = function () { select(idx); };
                    tr.ondblclick = function () { if (opts.onRowDblClick) opts.onRowDblClick(r); };
                });
            }
            function build(list, level, path) {
                if (level >= G.groups.length) { addRows(list); return; }
                var key = G.groups[level], gc = colOf(key), buckets = {}, order = [];
                list.forEach(function (r) {
                    var v = A.ci(r, key), g = String(v == null ? '' : v);
                    if (!buckets[g]) { buckets[g] = []; order.push(g); }
                    buckets[g].push(r);
                });
                order.sort(function (a, b) { return numeric(gc) ? Number(a || 0) - Number(b || 0) : a.localeCompare(b); });
                order.forEach(function (g) {
                    var p = path + '/' + key + '=' + g;
                    var gr = tb.insertRow(); gr.className = 'grp';
                    var cell = gr.insertCell(); cell.colSpan = cols.length; cell.style.paddingLeft = (4 + level * 16) + 'px';
                    var tg = d.createElement('span'); tg.className = 'tg'; tg.textContent = G.collapsed[p] ? '+' : '−';
                    cell.appendChild(tg);
                    cell.appendChild(d.createTextNode((gc ? gc.caption : key) + ': ' + (gc ? text(gc, buckets[g][0] && A.ci(buckets[g][0], key)) : g)));
                    gr.onclick = function () { G.collapsed[p] = !G.collapsed[p]; render(); };
                    if (!G.collapsed[p]) build(buckets[g], level + 1, p);
                    if (opts.groupTotals !== false) tb.appendChild(totals('gtot', buckets[g], cols));
                });
            }
            build(rows, 0, '');
            if (opts.totalRow !== false) tf.appendChild(totals('tot', rows, cols));
            if (G.selected >= G.flat.length) G.selected = G.flat.length ? G.flat.length - 1 : -1;
            navText();
        }
        function select(i) {
            if (i < 0 || i >= G.flat.length) return;
            var changed = i !== G.selected;
            if (G.selected >= 0 && G.flat[G.selected]) G.flat[G.selected].tr.classList.remove('sel');
            G.selected = i; G.flat[i].tr.classList.add('sel');
            var tr = G.flat[i].tr, top = tr.offsetTop - (HEAD + FLT + 2), bottom = tr.offsetTop + tr.offsetHeight - scroller.clientHeight + 26;
            if (scroller.scrollTop > top) scroller.scrollTop = top; else if (scroller.scrollTop < bottom) scroller.scrollTop = bottom;
            navText();
            if (changed && opts.onSelect) opts.onSelect(G.flat[i].row);
        }
        function navText() {
            if (opts.navText) opts.navText.textContent = G.columns ? ('Record: ' + (G.selected >= 0 ? G.selected + 1 : 0) + ' of ' + G.flat.length) : '';
        }
        G.render = render;
        G.select = select;
        /* DataSource + RetrieveStructure + settings (a new bind keeps the user's grouping, like the Janus layout does) */
        G.setData = function (columns, rows) {
            G.columns = columns; G.rows = rows || []; G.filters = {}; G.sortKey = ''; G.collapsed = {}; G.selected = -1; render();
            if (opts.autoSelect && G.flat.length) select(0);
        };
        /* GridEX.ClearStructure() / DataSource = null */
        G.clear = function () { G.columns = null; G.rows = []; G.filters = {}; G.sortKey = ''; G.selected = -1; render(); };
        G.hasStructure = function () { return !!G.columns; };
        G.current = function () { return G.selected >= 0 && G.flat[G.selected] ? G.flat[G.selected].row : null; };
        G.count = function () { return G.flat.length; };
        G.focus = function () { scroller.focus(); if (G.selected < 0 && G.flat.length) select(0); };
        if (gbEl) {
            gbEl.addEventListener('dragover', function (e) { e.preventDefault(); });
            gbEl.addEventListener('drop', function (e) {
                e.preventDefault();
                var k = e.dataTransfer.getData('text/plain');
                if (k && colOf(k) && G.groups.indexOf(k) < 0) { G.groups.push(k); G.collapsed = {}; G.selected = -1; render(); }
            });
        }
        scroller.addEventListener('keydown', function (e) {
            if (e.target.tagName === 'INPUT') return;
            if (e.ctrlKey && (e.code === 'Space' || e.key === ' ')) {
                e.preventDefault();
                var r = G.current(), lc = (G.columns || []).filter(function (c) { return c.link && !c.hidden; })[0];
                if (r && lc && opts.onLink) opts.onLink(r, lc);
                return;
            }
            if (e.key === 'ArrowDown' || e.key === 'ArrowUp') {
                if (e.ctrlKey) return;
                e.preventDefault(); if (!G.flat.length) return;
                select(G.selected < 0 ? 0 : Math.max(0, Math.min(G.flat.length - 1, G.selected + (e.key === 'ArrowDown' ? 1 : -1))));
            }
        });
        if (opts.nav) opts.nav.addEventListener('click', function (e) {
            var b = e.target.closest('[data-nav]'); if (!b || !G.flat.length) return;
            var n = b.getAttribute('data-nav');
            select(n === 'first' ? 0 : n === 'last' ? G.flat.length - 1 : n === 'prev' ? Math.max(0, G.selected - 1) : Math.min(G.flat.length - 1, G.selected + 1));
        });
        return G;
    };
}(window, document));
