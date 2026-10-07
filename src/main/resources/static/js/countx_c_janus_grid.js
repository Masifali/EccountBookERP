/* Janus-style grid used by the group C pages (Party Custom Group, Incentive Policy, InLand Freight Agreement).
   Behaviour modelled on GridEX: filter row (contains), click-to-sort headers, optional group-by box, record navigator,
   selector column with a header selector (UseHeaderSelector), frozen first column, empty total row.
   window.CJG.create(opts) -> grid object. */
(function (w, $) {
    'use strict';
    function esc(v) { return String(v == null ? '' : v).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function pick(r, k) {
        if (r == null) return null;
        if (Object.prototype.hasOwnProperty.call(r, k)) return r[k];
        var lk = String(k).toLowerCase();
        for (var p in r) if (Object.prototype.hasOwnProperty.call(r, p) && p.toLowerCase() === lk) return r[p];
        return null;
    }

    function create(opts) {
        var $t = $(opts.table), $nav = opts.nav ? $(opts.nav) : null, $gbx = opts.groupBox ? $(opts.groupBox) : null;
        var cols = opts.columns || [];
        var S = { rows: [], view: [], filters: {}, cur: -1, sortKey: null, sortDir: 1, groupKey: null, collapsed: {}, focusKey: null, shown: false };
        var G = {};

        function vis() { return cols.filter(function (c) { return !c.hidden && !(S.groupKey && c.key === S.groupKey); }); }
        function cellText(r, c) {
            var v = pick(r, c.key);
            if (c.format) return c.format(v, r);
            return v == null ? '' : String(v);
        }
        function navUpdate() {
            if (!$nav) return;
            var n = S.view.length;
            $nav.find('.t').text('Record ' + (S.cur >= 0 ? S.cur + 1 : 0) + ' of ' + n);
            $nav.find('[data-nav=first],[data-nav=prev]').prop('disabled', S.cur <= 0);
            $nav.find('[data-nav=next],[data-nav=last]').prop('disabled', S.cur < 0 || S.cur >= n - 1);
        }
        function gbxUpdate() {
            if (!$gbx) return;
            if (S.shown && S.groupKey) $gbx.html('<span class="chip" draggable="true" data-k="' + esc(S.groupKey) + '">' + esc(S.groupKey) + ' <b title="Remove grouping">&times;</b></span>');
            else $gbx.text('Drag a column header here to group by that column.');
        }
        function applyFilterSort() {
            var cs = vis();
            var v = S.rows.filter(function (r) {
                return cs.every(function (c) {
                    var f = S.filters[c.key]; if (!f || c.selector || c.button) return true;
                    return cellText(r, c).toLowerCase().indexOf(String(f).toLowerCase()) >= 0;
                });
            });
            var sk = S.sortKey;
            if (sk) {
                var sc = cols.filter(function (c) { return c.key === sk; })[0];
                v = v.map(function (r, i) { return { r: r, i: i }; }).sort(function (a, b) {
                    var x = pick(a.r, sk), y = pick(b.r, sk), c;
                    if (x == null && y == null) c = 0; else if (x == null) c = -1; else if (y == null) c = 1;
                    else if (sc && sc.num) c = (parseFloat(x) || 0) - (parseFloat(y) || 0);
                    else c = String(x).localeCompare(String(y));
                    return c * S.sortDir || a.i - b.i;
                }).map(function (o) { return o.r; });
            }
            if (S.groupKey) {
                var order = [], by = {};
                v.forEach(function (r) { var g = String(pick(r, S.groupKey) == null ? '' : pick(r, S.groupKey)); if (!by[g]) { by[g] = []; order.push(g); } by[g].push(r); });
                order.sort(function (a, b) { return a.localeCompare(b); });
                v = []; order.forEach(function (g) { v = v.concat(by[g]); });
            }
            S.view = v;
        }
        function render(keepFocus) {
            if (!S.shown) { $t.empty(); navUpdate(); gbxUpdate(); return; }
            var cs = vis();
            applyFilterSort();
            var tw = cs.reduce(function (a, c) { return a + (c.width || 100); }, 0);
            var h = '<colgroup>' + cs.map(function (c) { return '<col style="width:' + (c.width || 100) + 'px">'; }).join('') + '</colgroup>';
            h += '<thead><tr>' + cs.map(function (c, i) {
                var cls = (c.selector ? 'sel ' : '') + (c.num ? 'num ' : '') + (c.frozen ? 'frz' : '');
                if (c.button) return '<th class="btnh" data-k="' + esc(c.key) + '">' + esc(c.caption || c.key) + '</th>';
                if (c.selector) {
                    var all = S.view.length > 0 && S.view.every(function (r) { return r.__chk; });
                    return '<th class="' + cls + '" data-k="' + esc(c.key) + '">' + (opts.headerSelector === false ? '' : '<input type="checkbox" class="hsel"' + (all ? ' checked' : '') + '>') + '</th>';
                }
                return '<th class="' + cls + '" data-k="' + esc(c.key) + '" draggable="' + (opts.groupBox ? 'true' : 'false') + '">' + esc(c.caption || c.key)
                    + (S.sortKey === c.key ? '<span class="srt">' + (S.sortDir > 0 ? '&#9650;' : '&#9660;') + '</span>' : '') + '</th>';
            }).join('') + '</tr>';
            if (opts.filterRow !== false) {
                h += '<tr class="flt">' + cs.map(function (c) {
                    var cls = (c.selector ? 'sel ' : '') + (c.frozen ? 'frz' : '');
                    return (c.selector || c.button) ? '<td class="' + cls + '"></td>' : '<td class="' + cls + '"><input type="text" data-k="' + esc(c.key) + '" value="' + esc(S.filters[c.key] || '') + '"></td>';
                }).join('') + '</tr>';
            }
            h += '</thead><tbody>';
            function rowHtml(r, i) {
                return '<tr class="row' + (i === S.cur ? ' cur' : '') + '" data-i="' + i + '">' + cs.map(function (c) {
                    var cls = (c.selector ? 'sel ' : '') + (c.num ? 'num ' : '') + (c.frozen ? 'frz ' : '') + (c.readOnly ? 'ro' : '');
                    if (c.button) return '<td class="btnc"><button type="button" tabindex="-1" data-col="' + esc(c.key) + '"' + (c.buttonTitle ? ' title="' + esc(c.buttonTitle) + '"' : '') + '>' + (c.buttonHtml ? c.buttonHtml : esc(c.button)) + '</button></td>';
                    if (c.selector) return '<td class="' + cls + '"><input type="checkbox" class="rsel" tabindex="-1"' + (r.__chk ? ' checked' : '') + '></td>';
                    var t = cellText(r, c);
                    if (c.link) return '<td class="' + cls + '" data-k="' + esc(c.key) + '"><a href="#" class="lk" data-col="' + esc(c.key) + '">' + esc(t) + '</a></td>';
                    return '<td class="' + cls + '" data-k="' + esc(c.key) + '" title="' + esc(t) + '">' + esc(t) + '</td>';
                }).join('') + '</tr>';
            }
            if (S.groupKey) {
                var i = 0;
                while (i < S.view.length) {
                    var g = String(pick(S.view[i], S.groupKey) == null ? '' : pick(S.view[i], S.groupKey)), st = i, body = '';
                    while (i < S.view.length && String(pick(S.view[i], S.groupKey) == null ? '' : pick(S.view[i], S.groupKey)) === g) { body += rowHtml(S.view[i], i); i++; }
                    var col = !!S.collapsed[g];
                    h += '<tr class="grp" data-g="' + esc(g) + '"><td colspan="' + cs.length + '"><span class="tg">' + (col ? '+' : '&minus;') + '</span>' + esc(S.groupKey + ': ' + g) + ' (' + (i - st) + ')</td></tr>';
                    if (!col) h += body;
                }
            } else S.view.forEach(function (r, idx) { h += rowHtml(r, idx); });
            h += '</tbody>' + (opts.totalRow === false ? '' : '<tfoot><tr>' + cs.map(function (c) {
                if (!c.sum) return '<td class="' + (c.frozen ? 'frz' : '') + '"></td>';
                var tot = 0; S.view.forEach(function (r) { var n = parseFloat(pick(r, c.key)); if (isFinite(n)) tot += n; });
                var tt = c.totalFormat ? c.totalFormat(tot) : (c.format ? c.format(tot, null) : String(tot));
                return '<td class="num ' + (c.frozen ? 'frz' : '') + '">' + esc(tt) + '</td>';
            }).join('') + '</tr></tfoot>');
            $t.html(h);
            applyWidths(tw);
            if (keepFocus && S.focusKey) { var f = $t.find('tr.flt input').filter(function () { return $(this).data('k') === S.focusKey; }); if (f.length) { var v = f.val(); f.focus().val('').val(v); } }
            if (S.cur >= S.view.length) S.cur = S.view.length - 1;
            if (S.cur < 0 && S.view.length) S.cur = 0;
            var hh = $t.find('thead tr:first th').outerHeight() || 20;
            $t.find('tr.flt td').css('top', hh + 'px');
            $t.find('tbody tr.row').removeClass('cur'); $t.find('tbody tr.row[data-i="' + S.cur + '"]').addClass('cur');
            navUpdate(); gbxUpdate();
        }
        /* ColumnAutoResize: non-fixed columns share the width of the grid; button / selector columns keep their size */
        function applyWidths(tw) {
            var cs = vis();
            if (!opts.autoResize) { $t.css('width', tw + 'px'); return; }
            var avail = ($t.parent().innerWidth() || tw) - 2;
            var fixedSum = 0, flexSum = 0;
            cs.forEach(function (c) { if (c.button || c.selector || c.fixed) fixedSum += (c.width || 30); else flexSum += (c.width || 100); });
            var room = Math.max(avail - fixedSum, flexSum * 0.6);
            var total = 0, widths = cs.map(function (c) {
                var w = (c.button || c.selector || c.fixed) ? (c.width || 30) : Math.floor(room * (c.width || 100) / flexSum);
                total += w; return w;
            });
            $t.find('colgroup col').each(function (i) { this.style.width = widths[i] + 'px'; });
            $t.css('width', total + 'px');
        }
        $(w).on('resize', function () { if (opts.autoResize && S.shown) applyWidths(0); });
        function setCur(i, noScroll) {
            if (!S.view.length) { S.cur = -1; navUpdate(); return; }
            S.cur = Math.max(0, Math.min(S.view.length - 1, i));
            $t.find('tr.row').removeClass('cur');
            var $tr = $t.find('tr.row[data-i="' + S.cur + '"]').addClass('cur');
            if (!noScroll && $tr.length && $tr[0].scrollIntoView) $tr[0].scrollIntoView({ block: 'nearest' });
            navUpdate();
            if (opts.onCurrent) opts.onCurrent(S.view[S.cur], S.cur);
        }

        if ($nav) {
            $nav.html('<button type="button" data-nav="first" title="First">|&#9664;</button><button type="button" data-nav="prev" title="Previous">&#9664;</button>'
                + '<span class="t">Record 0 of 0</span><button type="button" data-nav="next" title="Next">&#9654;</button><button type="button" data-nav="last" title="Last">&#9654;|</button>');
            $nav.on('click', 'button', function () {
                var a = $(this).data('nav'), n = S.view.length;
                setCur(a === 'first' ? 0 : a === 'last' ? n - 1 : a === 'prev' ? S.cur - 1 : S.cur + 1);
            });
        }
        $t.attr('tabindex', 0);
        $t.on('click', 'tr.row td', function (e) { if ($(e.target).is('.rsel')) return; setCur(+$(this).closest('tr').data('i'), true); })
          .on('dblclick', 'tr.row td', function () {
              var i = +$(this).closest('tr').data('i'); setCur(i, true);
              if (opts.onDblClick) opts.onDblClick(S.view[i], i);
          })
          .on('click', 'td.btnc button', function (e) {
              e.stopPropagation();
              var i = +$(this).closest('tr').data('i'); setCur(i, true);
              if (opts.onButton) opts.onButton($(this).data('col'), S.view[i], i, this);
          })
          .on('click', 'td a.lk', function (e) {
              e.preventDefault(); e.stopPropagation();
              var i = +$(this).closest('tr').data('i'); setCur(i, true);
              if (opts.onLink) opts.onLink($(this).data('col'), S.view[i], i, this);
          })
          .on('click', 'tr.grp', function () { var g = $(this).data('g'); S.collapsed[g] = !S.collapsed[g]; render(true); })
          .on('input', 'tr.flt input', function () { S.focusKey = $(this).data('k'); S.filters[S.focusKey] = $(this).val(); render(true); })
          .on('change', 'input.rsel', function () {
              var i = +$(this).closest('tr').data('i'); if (S.view[i]) S.view[i].__chk = this.checked;
              var all = S.view.length > 0 && S.view.every(function (r) { return r.__chk; });
              $t.find('th input.hsel').prop('checked', all);
              if (opts.onCheck) opts.onCheck(S.view[i], this.checked);
          })
          .on('change', 'th input.hsel', function () {
              var on = this.checked; S.view.forEach(function (r) { r.__chk = on; }); $t.find('input.rsel').prop('checked', on);
          })
          .on('click', 'th[data-k]', function (e) {
              if ($(e.target).is('input')) return;
              var c = cols.filter(function (x) { return x.key === $(this).data('k'); }, this)[0];
              if (!c || c.selector || c.button || opts.sortable === false) return;
              if (S.sortKey === c.key) S.sortDir = -S.sortDir; else { S.sortKey = c.key; S.sortDir = 1; }
              render(true);
          })
          .on('keydown', function (e) {
              if ($(e.target).is('input, select, textarea')) return;
              if (e.key === 'ArrowDown' && !e.ctrlKey) { e.preventDefault(); setCur(S.cur + 1); }
              else if (e.key === 'ArrowUp' && !e.ctrlKey) { e.preventDefault(); setCur(S.cur - 1); }
              else if (e.key === ' ' && e.ctrlKey === false && S.cur >= 0 && cols.some(function (c) { return c.selector; })) {
                  e.preventDefault(); var r = S.view[S.cur]; r.__chk = !r.__chk; $t.find('tr.row[data-i="' + S.cur + '"] input.rsel').prop('checked', !!r.__chk);
              }
          })
          .on('dragstart', 'th[draggable="true"]', function (e) { e.originalEvent.dataTransfer.setData('text/plain', 'col:' + $(this).data('k')); });
        if ($gbx) {
            $gbx.on('dragover', function (e) { if (S.shown) { e.preventDefault(); $gbx.addClass('over'); } })
                .on('dragleave drop', function () { $gbx.removeClass('over'); })
                .on('drop', function (e) {
                    e.preventDefault();
                    var d = e.originalEvent.dataTransfer.getData('text/plain') || '';
                    if (d.indexOf('col:') === 0 && S.shown) { S.groupKey = d.substring(4); S.collapsed = {}; render(true); }
                })
                .on('click', '.chip b', function () { S.groupKey = null; render(true); });
        }

        G.setRows = function (rows) { S.rows = (rows || []).map(function (r) { var o = {}; for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k)) o[k] = r[k]; o.__chk = false; return o; });
            S.filters = {}; S.focusKey = null; S.collapsed = {}; S.groupKey = null; S.sortKey = null; S.cur = S.rows.length ? 0 : -1; S.shown = true; render(false); };
        /* DataSource = null */
        G.clear = function () { S.rows = []; S.view = []; S.cur = -1; S.shown = false; S.groupKey = null; S.filters = {}; render(false); };
        G.rerender = function () { render(true); };
        G.checked = function () { return S.rows.filter(function (r) { return r.__chk; }); };
        G.current = function () { return S.cur >= 0 ? S.view[S.cur] : null; };
        G.rows = function () { return S.rows; };
        G.view = function () { return S.view; };
        G.count = function () { return S.rows.length; };
        G.focus = function () { $t.focus(); };
        G.setCur = setCur;
        G.isShown = function () { return S.shown; };
        G.pick = pick;
        G.table = $t;
        return G;
    }
    w.CJG = { create: create, pick: pick, esc: esc };
})(window, window.jQuery);
