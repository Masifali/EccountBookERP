/* ============================================================================================
 * countx_logistics_lgsb_common.js - shared client helpers of the LgsB logistics pages
 *   949 /logistics/purchase-order                (countx_logistics_purchase_order.js)
 *   955 /logistics/services-bill                 (countx_logistics_services_bill.js)
 *   965 /logistics/freight-voucher-export        (countx_logistics_freight_voucher_export.js)
 *   967 /logistics/freight-voucher-export-multi  (countx_logistics_freight_voucher_export_multi.js)
 * Built on countx_hrm.js (window.HRM); exposes window.LgsB.
 *
 *   LgsB.net(v, fmt)        .NET custom numeric format ("#,##.##", "#,#0.0000", "#,#;(#,#);0" ...), rounding away from zero
 *   LgsB.fillCols(...)      a multi-column UltraCombo (data-columns + data-extra read by countx_prod_combo.js)
 *   LgsB.onLeave(id, fn)    UltraCombo.Leave / ValueChanged
 *   LgsB.tabs()             the TabControl (Form / History), Ctrl+T switch
 *   LgsB.dateType(...)      InfragisticsHelper.BindComboDateType + cmbDateTypeHistory_ValueChanged
 *   LgsB.pick(...)          GrdPopUp (F1 lookup on a grid column)
 *   LgsB.loader(...)        the loader forms (frmLoad...) as a dialog: filters, Show, a Select column, Load
 *   LgsB.shortcuts(rows)    ShortCutKeyPopUp
 *   LgsB.lastCol(grid)      the grid column last clicked (GridEX CurrentColumn, for F1 / Ctrl+Space)
 * ============================================================================================ */
(function (global) {
    'use strict';

    var L = {};
    global.LgsB = L;

    /* Page styles of the four LgsB pages (on top of countx_hrm.css), injected once. */
    (function css() {
        var s = document.createElement('style');
        s.setAttribute('data-lgsb', '1');
        s.textContent = [
            '.lgsb-row{display:grid;grid-template-columns:repeat(auto-fill,minmax(150px,1fr));gap:4px 10px;padding:6px 10px;align-items:end}',
            '.lgsb-row .hrm-field.hrm-stack>label{font-size:8.5pt}',
            '.lgsb-row .lgsb-w2{grid-column:span 2}',
            '.lgsb-radios{display:flex;gap:10px;flex-wrap:wrap;font:8.5pt "Segoe UI",sans-serif;align-items:center}',
            '.lgsb-radios label{display:inline-flex;gap:3px;align-items:center;margin:0;font-weight:normal}',
            '.lgsb-sub{display:flex;align-items:center;gap:6px;flex-wrap:wrap}',
            '.lgsb-sub .win-textbox{flex:1 1 90px;min-width:0}',
            '.lgsb-tools-right{margin-left:auto;display:inline-flex;gap:10px;align-items:center;font:9pt "Segoe UI",sans-serif}',
            '.lgsb-tools-right label{margin:0;font-weight:normal;display:inline-flex;gap:4px;align-items:center}',
            '.lgsb-desc{font:8.5pt "Segoe UI",sans-serif;color:#555;padding:2px 10px}',
            '.lgsb-actions{display:flex;gap:6px;align-items:end;flex-wrap:wrap}',
            '.lgsb-actions .win-btn-action{min-width:64px}',
            'table.hrm-grid .lgsb-cell-btn{height:20px;min-width:0;width:auto;padding:0 8px;font-size:8pt;line-height:18px}',
            '.lgsb-loader-filters{display:grid;grid-template-columns:repeat(auto-fill,minmax(160px,1fr));gap:4px 10px;padding:6px 10px;align-items:end}',
            '.lgsb-loader-grid{max-height:40vh;overflow:auto}',
            '.lgsb-loader-grid.lgsb-short{max-height:26vh}',
            '.lgsb-card{position:absolute;z-index:3000;background:#fff;border:1px solid #7a9a9e;box-shadow:0 4px 14px rgba(0,0,0,.2);width:min(920px,96vw);font:8.5pt "Segoe UI",sans-serif}',
            '.lgsb-card-h{background:#008080;color:#fff;padding:4px 8px;font-weight:bold}',
            '.lgsb-card-b{display:grid;grid-template-columns:repeat(auto-fill,minmax(170px,1fr));gap:4px 10px;padding:6px 8px}',
            '.lgsb-card-f{display:flex;flex-direction:column}.lgsb-card-f span{color:#555}.lgsb-card-f.is-hi b{color:#b00}',
            '.lgsb-totals .win-textbox{text-align:right}',
            '.lgsb-balance{font:bold 9pt "Segoe UI",sans-serif;color:#004080}',
            '@media (max-width:600px){.lgsb-row{grid-template-columns:repeat(2,minmax(0,1fr));padding:6px}.lgsb-row .lgsb-w2{grid-column:span 2}.lgsb-tools-right{margin-left:0}}'
        ].join('\n');
        document.head.appendChild(s);
    })();

    // ------------------------------------------------------------------------ .NET number formats
    function roundAway(v, d) {
        var p = Math.pow(10, d), x = Math.abs(v) * p;
        var r = Math.round(x + 1e-9 * Math.max(1, x));
        return (v < 0 ? -1 : 1) * r / p;
    }
    function section(v, fmt) {
        var neg = v < 0, a = Math.abs(v);
        var dot = fmt.indexOf('.');
        var ip = dot >= 0 ? fmt.slice(0, dot) : fmt, dp = dot >= 0 ? fmt.slice(dot + 1) : '';
        var group = ip.indexOf(',') >= 0;
        var minInt = (ip.match(/0/g) || []).length;
        var minDec = (dp.match(/0/g) || []).length, maxDec = (dp.match(/[0#]/g) || []).length;
        var r = roundAway(a, maxDec);
        var s = r.toFixed(maxDec), parts = s.split('.');
        var ints = parts[0], decs = parts[1] || '';
        while (decs.length > minDec && decs.charAt(decs.length - 1) === '0') decs = decs.slice(0, -1);
        if (ints === '0' && minInt === 0) ints = '';
        while (ints.length < minInt) ints = '0' + ints;
        if (group && ints) ints = ints.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        var out = ints + (decs ? '.' + decs : '');
        return { text: out, neg: neg && r !== 0 };
    }
    /** value.ToString(fmt) for the custom formats the logistics forms use. */
    L.net = function (v, fmt) {
        var n = HRM.num(v);
        var secs = String(fmt).split(';');
        if (secs.length >= 2) {
            if (n < 0) {
                var neg = section(-n, secs[1].replace(/[()]/g, ''));
                return secs[1].replace(/[#0,.]+/, neg.text);
            }
            if (n === 0 && secs.length >= 3) return secs[2];
            return section(n, secs[0]).text;
        }
        var s = section(n, fmt);
        return (s.neg ? '-' : '') + s.text;
    };
    /** "#,##0." + N zeros (stringFormatsingle) / "#,#0." + N zeros (DecimalFCYRateFormate). */
    L.fixed = function (v, d) { return L.net(v, d > 0 ? '#,##0.' + new Array(d + 1).join('0') : '#,##0'); };
    /** Math.Round(v, d, MidpointRounding.AwayFromZero). */
    L.roundAway = function (v, d) { return roundAway(HRM.num(v), d || 0); };
    /** Math.Round(v, d) (to even). */
    L.roundEven = function (v, d) {
        var p = Math.pow(10, d || 0), x = HRM.num(v) * p, r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r / p;
    };
    /** double.ToString() - up to 15 significant digits, no grouping. */
    L.dbl = function (v) { var n = HRM.num(v); return String(parseFloat(n.toPrecision(15))); };

    // ------------------------------------------------------------------------ combos
    /**
     * BindAndRetainSelection over a DataTable with AllColumns: the first column is the text; `extra` keys become the
     * other columns (captions `caps`). opts.zero: '' for the default row (insertDefaultRow), false for none.
     */
    L.fillCols = function (id, rows, valueKey, textKey, extra, caps, opts) {
        var sel = HRM.$(id); if (!sel) return;
        opts = opts || {};
        var keep = opts.keep === false ? null : sel.value;
        if (caps && caps.length) sel.setAttribute('data-columns', caps.join('|')); else sel.removeAttribute('data-columns');
        var zero = opts.zero === undefined ? '' : opts.zero;
        var html = zero === false ? '' : '<option value="0">' + HRM.esc(zero) + '</option>';
        (rows || []).forEach(function (r) {
            var t = typeof textKey === 'function' ? textKey(r) : HRM.col(r, textKey);
            var ex = (extra || []).map(function (k) { return HRM.str(HRM.col(r, k)).replace(/\|/g, '/'); }).join('|');
            html += '<option value="' + HRM.esc(HRM.col(r, valueKey)) + '"' + (ex ? ' data-extra="' + HRM.esc(ex) + '"' : '') + '>' + HRM.esc(t) + '</option>';
        });
        sel.innerHTML = html;
        sel._rows = rows || [];
        if (keep !== null && HRM.hasOption(id, keep)) sel.value = keep;
        else if (zero !== false) sel.value = '0';
        else sel.selectedIndex = -1;
        HRM.refreshCombos();
        if (sel.__dtcombo && sel.__dtcombo.syncFromSelect) sel.__dtcombo.syncFromSelect();
    };
    /** The source row of the chosen option (UltraCombo.SelectedRow). */
    L.row = function (id, valueKey) { return HRM.comboRow(id, valueKey || 'Id'); };
    /** Combo.Text. */
    L.text = function (id) { return HRM.comboText(id); };
    /** UltraCombo.Value = v, firing the ValueChanged handler the page registered. */
    L.set = function (id, v, silent) {
        HRM.setCombo(id, v);
        if (!silent) { var s = HRM.$(id); if (s) s.dispatchEvent(new Event('lgsb-set')); }
    };
    /** UltraCombo.Leave (and ValueChanged when valueChanged is true): a pick fires `change`; focus leaving the wrap is the Leave. */
    L.onLeave = function (id, fn, valueChanged) {
        var sel = HRM.$(id); if (!sel) return;
        var last = null;
        function run() { last = sel.value; fn(); }
        sel.addEventListener('change', function () { if (valueChanged !== false) run(); });
        if (valueChanged !== false) sel.addEventListener('lgsb-set', run);
        document.addEventListener('focusout', function (e) {
            var w = e.target && e.target.closest ? e.target.closest('.dtcombo-wrap') : null;
            if (!w || !w.contains(sel)) return;
            if (e.relatedTarget && w.contains(e.relatedTarget)) return;
            setTimeout(run, 0);
        });
        return function () { return last; };
    };

    // ------------------------------------------------------------------------ tabs
    L.tabs = function (onChange) {
        var bar = document.querySelector('[data-tabs="main"]'); if (!bar) return function () { };
        function show(id) {
            document.querySelectorAll('[data-tabs-page="main"]').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
            bar.querySelectorAll('.hrm-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
            if (onChange) onChange(id);
        }
        bar.addEventListener('click', function (e) { var b = e.target.closest('.hrm-tab'); if (b) show(b.getAttribute('data-tab')); });
        return show;
    };
    L.activeTab = function () { var p = document.querySelector('[data-tabs-page="main"].is-active'); return p ? p.id : ''; };

    // ------------------------------------------------------------------------ history date type
    /** InfragisticsHelper.BindComboDateType: This Day, This Week, This Month, This Year, Financial Year. */
    L.dateType = function (selId, fromId, toId, yearStart) {
        HRM.fill(selId, [[1, 'This Day'], [2, 'This Week'], [3, 'This Month'], [4, 'This Year'], [5, 'Financial Year']].map(function (p) { return { v: p[0], t: p[1] }; }), 'v', 't', { zero: '' });
        var sel = HRM.$(selId);
        if (sel._lgsbWired) return;
        sel._lgsbWired = true;
        sel.addEventListener('change', function () {                          // cmbDateTypeHistory_ValueChanged
            var v = HRM.comboVal(selId), t = new Date();
            if (v === 1) HRM.setVal(fromId, HRM.today());
            else if (v === 2) HRM.setVal(fromId, HRM.addDays(HRM.today(), -7));
            else if (v === 3) { HRM.setVal(fromId, HRM.firstOfMonth()); HRM.setVal(toId, HRM.today()); }
            else if (v === 4) { HRM.setVal(fromId, t.getFullYear() + '-01-01'); HRM.setVal(toId, HRM.today()); }
            else if (v === 5 && L.yearStart) HRM.setVal(fromId, L.yearStart);
            [fromId, toId].forEach(function (id) { var c = HRM.$(id + 'On'); if (c && HRM.val(id)) c.checked = true; });
        });
        L.yearStart = yearStart || null;
    };
    /** The history filter of HistoryGridFill: the picked date kind and the checked pickers only. */
    L.historyFilter = function (extra) {
        var kind = (document.querySelector('input[name="histDateKind"]:checked') || {}).value || 'doc';
        var f = {
            dateKind: kind,
            fromDate: HRM.checked('FromDateHistoryOn') ? HRM.val('FromDateHistory') : '',
            toDate: HRM.checked('ToDateHistoryOn') ? HRM.val('ToDateHistory') : '',
            fromDocNo: HRM.val('FromDocNoHistory'), toDocNo: HRM.val('ToDocNoHistory')
        };
        Object.keys(extra || {}).forEach(function (k) { f[k] = extra[k]; });
        return f;
    };

    // ------------------------------------------------------------------------ grid helpers
    /** GridEX CurrentColumn: the column of the cell last clicked in the table. */
    L.trackColumn = function (grid) {
        grid._lastCol = null;
        grid.table.addEventListener('mousedown', function (e) {
            var td = e.target.closest('td'); if (!td || !td.parentElement || !td.parentElement.hasAttribute('data-i')) return;
            var idx = td.cellIndex, c = grid.columns[idx];
            grid._lastCol = c ? c.key : null;
            grid.select(+td.parentElement.getAttribute('data-i'));
        });
        grid.table.addEventListener('focusin', function (e) {
            var inp = e.target.getAttribute && e.target.getAttribute('data-k');
            if (inp) grid._lastCol = inp;
        });
    };
    L.lastCol = function (grid) { return grid._lastCol; };
    /** GridEX_Helper.AddButton: a button column rendered in every row. */
    L.btnCol = function (key, caption, text, width) {
        return { key: key, caption: caption, width: width || 40, render: function () { return '<button type="button" class="win-btn-action lgsb-cell-btn" data-act="' + key + '">' + HRM.esc(text) + '</button>'; } };
    };
    /** ColumnButtonClick on a grid. */
    L.onButton = function (grid, fn) {
        grid.table.addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            grid.select(i);
            fn(grid.rows()[i], b.getAttribute('data-act'), i, b);
        });
    };
    /** Keyboard on a grid (Ctrl+Space, Ctrl+Delete, Ctrl+Enter, F1) - only while focus is inside the table. */
    L.gridKeys = function (grid, map) {
        grid.table.addEventListener('keydown', function (e) {
            var k = (e.ctrlKey ? 'ctrl+' : '') + (e.key === ' ' ? 'space' : (e.key || '').toLowerCase());
            if (map[k]) { e.preventDefault(); e.stopPropagation(); var r = grid.current(); if (r) map[k](r, grid.currentIndex()); }
        });
    };
    /**
     * A new HRM.Grid on a fresh copy of the table (the listeners of an earlier grid on it are dropped) - used when the
     * desktop re-runs its grid setting with other columns (RetrieveStructure + DetailGridCommonSetting).
     */
    L.freshGrid = function (id, opts) {
        var old = HRM.$(id); if (!old) return null;
        var t = old.cloneNode(false);
        old.parentNode.replaceChild(t, old);
        return new HRM.Grid(t, opts);
    };
    L.numCol = function (key, caption, d, opts) {
        var c = { key: key, caption: caption, type: 'num', decimals: d === undefined ? 2 : d };
        Object.keys(opts || {}).forEach(function (k) { c[k] = opts[k]; });
        return c;
    };

    // ------------------------------------------------------------------------ popups
    /** GrdPopUp(dt, "Id", nameCol [, codeCol]): pick one row; onPick(row). */
    L.pick = function (title, rows, cols, onPick) {
        var h = HRM.history({
            title: title, width: 'min(560px, 96vw)', columns: cols,
            load: function () { return Promise.resolve(rows || []); },
            onPick: onPick
        });
        return h;
    };
    /** ShortCutKeyPopUp(dt) - KeyCombination / Description. */
    L.shortcuts = function (pairs) {
        L.pick('ShortCut Keys', pairs.map(function (p) { return { KeyCombination: p[0], Description: p[1] }; }),
            [{ key: 'KeyCombination', caption: 'KeyCombination', width: 140 }, { key: 'Description', caption: 'Description', width: 360 }], null);
    };
    /** "The attachments of a document are not part of the web port." */
    L.noAttachments = function () { HRM.box('Attachments are not available on the web version of this form.'); };

    /**
     * A loader form (frmLoadLogisticAgreementForPO, frmLoadPurchaseOrderForServicesBill, ...) as a dialog.
     *  o.title, o.caption, o.filters (html), o.onReady(body), o.search(body) -> promise rows, o.columns (grid),
     *  o.detailColumns + o.detail(row, allRows) -> rows (the lower grid), o.load(checkedRows, allRows) -> rows to
     *  hand back (or a thrown message), o.done(rows). Buttons: New (o.reset), Refresh (o.refresh), Load, Show, Short Cut Keys.
     *  Esc closes and returns nothing (dtLoader = null).
     */
    L.loader = function (o) {
        var m = HRM.modal({ title: o.title, width: 'min(1200px, 97vw)', backdropClose: false,
            html: '<div class="win-tool-strip lgsb-loader-tools">' +
                '<button type="button" class="win-btn-tool" data-l="new"><i class="fa fa-plus"></i> <span><u>N</u>ew</span></button>' +
                '<button type="button" class="win-btn-tool" data-l="refresh"><i class="fa fa-refresh"></i> <span>Refresh</span></button>' +
                '<button type="button" class="win-btn-tool" data-l="load"><i class="fa fa-download"></i> <span>Load</span></button>' +
                '<button type="button" class="win-btn-tool" data-l="keys"><i class="fa fa-keyboard-o"></i> <span>ShortCut Keys</span></button></div>' +
                '<div class="hrm-caption lgsb-loader-caption">' + HRM.esc(o.caption || o.title) + '</div>' +
                '<div class="lgsb-loader-filters">' + (o.filters || '') +
                '<div class="hrm-field hrm-stack"><label>&nbsp;</label><button type="button" class="win-btn-action" data-l="show">Show</button></div></div>' +
                '<div class="hrm-grid-box"><div class="hrm-subcaption"><span>Records</span><span class="lgsb-count"></span></div>' +
                '<div class="hrm-grid-wrap lgsb-loader-grid"><table class="lgsb-lg-main"></table></div></div>' +
                (o.detailColumns ? '<div class="hrm-grid-box"><div class="hrm-subcaption"><span>Detail</span></div>' +
                    '<div class="hrm-grid-wrap lgsb-loader-grid lgsb-short"><table class="lgsb-lg-detail"></table></div></div>' : '')
        });
        var body = m.body, all = [];
        var cols = [{ key: '__sel', caption: 'Select', width: 40, type: 'edit-check' }].concat(o.columns);
        var g = new HRM.Grid(body.querySelector('.lgsb-lg-main'), {
            columns: cols, filterRow: true, emptyText: 'No record found.',
            onSelect: function (r) { if (dg) dg.set(o.detail(r, all) || []); }
        });
        var dg = o.detailColumns ? new HRM.Grid(body.querySelector('.lgsb-lg-detail'), { columns: o.detailColumns, filterRow: false }) : null;
        function btn(k) { return body.querySelector('[data-l="' + k + '"]'); }
        function show() {
            return HRM.busy(btn('show'), function () {
                return Promise.resolve(o.search(body)).then(function (rows) {
                    all = rows || [];
                    var shown = o.rows ? o.rows(all) : all;
                    g.set(shown);
                    if (o.checked) shown.forEach(function (r) { if (o.checked(r)) r.__sel = true; });
                    g.draw();
                    if (dg) dg.clear();
                    body.querySelector('.lgsb-count').textContent = shown.length + ' record(s)';
                }).catch(HRM.fail);
            });
        }
        function load() {
            var checked = g.rows().filter(function (r) { return HRM.bool(r.__sel); });
            if (!checked.length) { HRM.box('Check the row first'); return; }
            var out;
            try { out = o.load(checked, all); } catch (e) { HRM.fail(e); return; }
            if (out === false || out === undefined || out === null) return;
            m.close();
            o.done(out);
        }
        btn('show').addEventListener('click', show);
        btn('load').addEventListener('click', load);
        btn('new').addEventListener('click', function () { if (o.reset) o.reset(body); show(); });
        btn('refresh').addEventListener('click', function () { if (o.refresh) HRM.busy(btn('refresh'), function () { return o.refresh(body); }); });
        btn('keys').addEventListener('click', function () {
            L.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'],
                ['Ctrl+S', 'For Search'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
        });
        m.el.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var k = (e.key || '').toLowerCase();
            if (k === 's') { e.preventDefault(); show(); }
            else if (k === 'l') { e.preventDefault(); load(); }
            else if (k === 'n') { e.preventDefault(); btn('new').click(); }
            else if (k === 'r') { e.preventDefault(); btn('refresh').click(); }
            else if (k === 'e') { e.preventDefault(); m.close(); }
        });
        if (o.onReady) Promise.resolve(o.onReady(body)).then(function () { if (o.autoShow !== false) show(); }).catch(HRM.fail);
        else if (o.autoShow !== false) show();
        return { modal: m, grid: g, body: body, show: show };
    };

    /** A filter field for the loader dialogs. */
    L.f = function (label, inner) { return '<div class="hrm-field hrm-stack"><label>' + HRM.esc(label) + '</label>' + inner + '</div>'; };

    /** Opens a Crystal print by its registered key after the page's own check call; message when the print is not wired. */
    L.print = function (key, args, check, btn) {
        var win = global.CrystalPrint ? global.CrystalPrint.reserve() : null;
        return HRM.busy(btn, function () {
            return Promise.resolve(check ? check() : null).then(function () {
                if (!global.CrystalPrint) { HRM.box('The print is not available.'); return; }
                return global.CrystalPrint.open(key, args, null, win);
            }).catch(function (e) { if (win) global.CrystalPrint.release(win); HRM.fail(e); });
        });
    };

    /** The hover card of the commercial invoice (ShowInvoiceInfo) - shown under the combo. */
    L.invoiceCard = function (anchor, title, row) {
        L.hideCard();
        if (!row) return;
        var f = function (c, v, hi) { return '<div class="lgsb-card-f' + (hi ? ' is-hi' : '') + '"><span>' + HRM.esc(c) + '</span><b>' + HRM.esc(v) + '</b></div>'; };
        var card = document.createElement('div');
        card.className = 'lgsb-card';
        card.innerHTML = '<div class="lgsb-card-h">' + HRM.esc(title) + '</div><div class="lgsb-card-b">' +
            f('Customer Name', HRM.str(HRM.col(row, 'CustomerName'))) + f('Forwarder Name', HRM.str(HRM.col(row, 'ForwarderName'))) +
            f('LC / Contract', HRM.str(HRM.col(row, 'LcOrderNo'))) + f('BL No', HRM.str(HRM.col(row, 'BLNumber'))) +
            f('GD No', HRM.str(HRM.col(row, 'GdNo'))) + f('Loading Place', HRM.str(HRM.col(row, 'LoadingPort'))) +
            f('Destination Place', HRM.str(HRM.col(row, 'DestinationPort'))) + f('Container Dispatched At', HRM.str(HRM.col(row, 'ContainerDispatchedAtPort'))) +
            f('Container Received From', HRM.str(HRM.col(row, 'ContainerReceivedFromPort'))) + f('Delivered At', HRM.str(HRM.col(row, 'DeliveredAt'))) +
            f('Vessel Name', HRM.str(HRM.col(row, 'VesselName'))) + f('Income Term', HRM.str(HRM.col(row, 'DeliveryTerm'))) +
            f('No Of Containers', HRM.str(HRM.col(row, 'NoOfContainers'))) +
            f('M.Ton', L.net(HRM.num(HRM.col(row, 'NetWeight')) / 1000.0, '#,##0.00'), true) +
            f('Booking Rate', L.net(HRM.col(row, 'BookingRate'), '#,##0.00'), true) + '</div>';
        document.body.appendChild(card);
        var r = anchor.getBoundingClientRect();
        card.style.top = (global.scrollY + r.bottom + 2) + 'px';
        card.style.left = Math.max(4, Math.min(global.scrollX + r.left, global.scrollX + document.documentElement.clientWidth - card.offsetWidth - 4)) + 'px';
        card.addEventListener('mouseleave', L.hideCardSoon);
        card.addEventListener('mouseenter', function () { clearTimeout(L._cardT); });
    };
    L.hideCard = function () { clearTimeout(L._cardT); document.querySelectorAll('.lgsb-card').forEach(function (c) { c.remove(); }); };
    L.hideCardSoon = function () { clearTimeout(L._cardT); L._cardT = setTimeout(L.hideCard, 300); };


    // ------------------------------------------------------------------------ service detail entry (949 / 955)
    /**
     * The "Detail Information" group shared by frmLogisticPurchaseOrder and frmLogisticPurchaseServicesBill (the two
     * forms' item / currency / rate / tax / location code is line-for-line the same). o: { api, cfg(), docDate(),
     * currentTaxNameId() (grid row being edited), onGlcy() }.
     */
    L.serviceDetail = function (o) {
        var S = { items: [], itemsAll: [], currencies: [], locTypes: [], rateBases: [], cities: [], updateIndex: -1 };
        function cfg() { return o.cfg() || {}; }
        function fcyFmt() { var n = HRM.int(cfg().fcyRateDecimals) || 4; return '#,#0.' + new Array(n + 1).join('0'); }
        function amtFmt() { var n = HRM.int(cfg().amountDecimals); return n > 0 ? '#,##0.' + new Array(n + 1).join('0') : '#,##0'; }
        S.fcyFmt = fcyFmt; S.amtFmt = amtFmt;

        /** AllLookUpComboBind: ServiceType / PaymentTerm / LocationType / RateBaseUom (with OtherReference = Equivalent). */
        S.lookups = function (rows) {
            var out = { ServiceType: [], PaymentTerm: [], LocationType: [], RateBaseUom: [], BillWeightBase: [], ChargesType: [] };
            (rows || []).forEach(function (r) {
                var a = HRM.str(HRM.col(r, 'Activity')), id = HRM.int(HRM.col(r, 'Id')), n = HRM.str(HRM.col(r, 'ReferenceName'));
                if (!out[a]) return;
                out[a].push(a === 'RateBaseUom' ? { Id: id, Name: n, Equivalent: HRM.num(HRM.col(r, 'OtherReference')) } : { Id: id, Name: n });
            });
            return out;
        };
        S.bindLookups = function (lk) {
            S.locTypes = lk.LocationType; S.rateBases = lk.RateBaseUom;
            L.fillCols('CmbRateBaseType', lk.RateBaseUom, 'Id', 'Name', ['Equivalent'], ['Name', 'Equivalent'], { zero: '' });
            HRM.fill('CmbLocationType', lk.LocationType, 'Id', 'Name', { zero: '', keep: true });
        };
        /** CurrencyBindFromGlobal: Glcy / Fcy / Transaction currency combos. */
        S.bindCurrencies = function (rows) {
            S.currencies = rows || [];
            ['CmbGlcyCurrency', 'CmbFcyCurrency', 'CmbTransactionCurrencyDetail'].forEach(function (id) {
                L.fillCols(id, S.currencies, 'Id', 'CurrencyCode', ['CurrencyRate'], ['CurrencyCode', 'CurrencyRate'], { zero: '' });
            });
        };
        /** ItemCategoryOrMasterItemBind: the distinct categories or master items of the service items. */
        S.masterBind = function () {
            var byCat = HRM.checked('RadCategory'), seen = {}, out = [];
            S.itemsAll.forEach(function (r) {
                var d = HRM.str(byCat ? r.ItemCategory : r.MasterItem), id = HRM.int(byCat ? r.ItemCategoryId : r.MasterItemId);
                if (!d || seen[d]) return; seen[d] = 1; out.push({ Id: id, Description: d });
            });
            HRM.fill('CmbMasterItem', out, 'Id', 'Description', { zero: '', keep: true });
        };
        /** ItemDtsFillFromGlobal(CategoryOrMasterItemId) + ItemNameBind (Name / Code radio picks the shown column). */
        S.itemsFill = function (id) {
            id = id || 0;
            var byCat = HRM.checked('RadCategory');
            S.items = S.itemsAll.filter(function (r) { return id === 0 || (byCat ? HRM.int(r.ItemCategoryId) === id : HRM.int(r.MasterItemId) === id); });
            var byName = HRM.checked('RadItemNamePackListDetail');
            L.fillCols('CmbItemName', S.items, 'Id', byName ? 'ItemName' : 'ItemCode', [byName ? 'ItemCode' : 'ItemName', 'MasterItem', 'ItemCategory'],
                [byName ? 'Service Item Name' : 'Service Item Code', byName ? 'ItemCode' : 'ItemName', 'MasterItem', 'ItemCategory'], { zero: '' });
        };
        S.item = function (id) { var hit = null; S.itemsAll.forEach(function (r) { if (HRM.int(r.Id) === HRM.int(id)) hit = r; }); return hit; };
        S.bindItems = function (rows) { S.itemsAll = rows || []; S.masterBind(); S.itemsFill(0); };

        function rateEq() { var r = L.row('CmbRateBaseType'); return HRM.comboVal('CmbRateBaseType') > 0 && r ? HRM.num(r.Equivalent) : 0; }

        /** CalculateAmountsInDetail. */
        S.calc = function () {
            var tcy = HRM.num(HRM.val('txtServiceRate')) * rateEq() * HRM.num(HRM.val('txtQty'));
            HRM.setVal('txtTcyAmount', L.net(tcy, fcyFmt()));
            var ex = HRM.num(HRM.val('txtTransactionExchangeRateDetail'));
            HRM.setVal('txtLcyAmountDetail', L.net(ex > 0 ? tcy * ex : 0, fcyFmt()));
            S.tax();
        };
        /** CalculateTaxAmount. */
        S.tax = function (keepPercentText) {
            var tcy = HRM.num(HRM.val('txtTcyAmount')), pct = 0;
            if (HRM.comboVal('CmbTaxName') > 0) {
                var typed = HRM.num(HRM.val('txtTaxPercnt')), r = L.row('CmbTaxName');
                pct = typed > 0 ? typed : (r ? HRM.num(r.TaxPrcnt) : 0);
            }
            var tax = tcy * pct / 100.0, tot = tcy + tax, d = HRM.int(cfg().amountRound);
            if (!keepPercentText) HRM.setVal('txtTaxPercnt', L.net(pct, '#,##.###'));
            HRM.setVal('txtTaxAmount', L.net(L.roundAway(tax, d), amtFmt()));
            HRM.setVal('txtTotalAmount', L.net(L.roundAway(tot, d), amtFmt()));
        };
        /** txtServiceRate_TextChanged (also Qty / NetWeight). */
        S.rateChanged = function () {
            HRM.enable('txtNetWeight', true);
            var id = HRM.comboVal('CmbRateBaseType');
            if (id === 3 || id === 4) {
                HRM.enable('txtNetWeight', false);
                var w = id === 3 ? HRM.num(HRM.val('txtQty')) * 1000.0 : HRM.num(HRM.val('txtQty'));
                HRM.setVal('txtNetWeight', L.net(w, '#,##.##'));
            }
            S.calc();
        };
        /** CmbRateBaseType_Leave. */
        S.rateBaseLeave = function () {
            HRM.enable('txtNetWeight', true); HRM.enable('txtQty', true);
            var id = HRM.comboVal('CmbRateBaseType');
            if (id === 3 || id === 4) {
                HRM.enable('txtNetWeight', false);
                var w = id === 3 ? HRM.num(HRM.val('txtQty')) * 1000.0 : HRM.num(HRM.val('txtQty'));
                HRM.setVal('txtNetWeight', L.net(w, '#,##.##'));
            }
            if (id === 5) { HRM.setVal('txtQty', L.net(1, '#,##.##')); HRM.enable('txtQty', false); }
            S.calc();
        };
        /** CmbTransactionCurrencyDetail_ValueChanged. */
        S.currencyChanged = function () {
            var r = L.row('CmbTransactionCurrencyDetail');
            if (HRM.comboVal('CmbTransactionCurrencyDetail') > 0 && r) {
                var rate = HRM.num(r.CurrencyRate);
                if (rate > 0 && !(HRM.num(HRM.val('txtTransactionExchangeRateDetail')) > 0)) HRM.setVal('txtTransactionExchangeRateDetail', L.net(rate, fcyFmt()));
            }
            S.calc();
        };
        /** CmbItemName_Leave: TaxTypeBind(TaxTypeDbCall(ItemId, DocDate)) or clear the tax. */
        S.itemLeave = function () {
            var id = HRM.comboVal('CmbItemName');
            if (id > 0) {
                return HRM.get(o.api + '/tax', { itemId: id, docDate: o.docDate() }).then(function (rows) {
                    L.fillCols('CmbTaxName', rows || [], 'Id', 'TaxType', ['TaxPrcnt'], ['Tax Name', 'TaxPrcnt'], { zero: '' });
                    var cur = o.currentTaxNameId ? o.currentTaxNameId() : 0;
                    if (S.updateIndex > -1 && cur > 0) HRM.setCombo('CmbTaxName', cur);
                    S.tax();
                }).catch(HRM.fail);
            }
            HRM.fill('CmbTaxName', [], 'Id', 'TaxType', { zero: '' });
            HRM.setVal('txtTaxPercnt', ''); HRM.setVal('txtTaxAmount', '');
            return Promise.resolve();
        };
        /** CmbLocationType_Leave: LocationdtFill (16 ports, 17 cities) + LocationsBind (captions follow the type). */
        S.locations = function (typeId) {
            if (typeId === 16) return HRM.get(o.api + '/ports').then(function (rows) { return rows || []; });
            if (typeId === 17) return Promise.resolve(S.cities.map(function (c) { return { Id: c.Id, LocationName: c.Description }; }));
            return Promise.resolve([]);
        };
        S.locationLeave = function () {
            var t = HRM.comboVal('CmbLocationType');
            return S.locations(t).then(function (rows) {
                HRM.text('lblLocationFrom', t === 16 ? 'Loading Port' : 'City From');
                HRM.text('lblLocationTo', t === 16 ? 'Destination Port' : 'City To');
                HRM.fill('CmbLocationFrom', rows, 'Id', 'LocationName', { zero: '', keep: true });
                HRM.fill('CmbLocationTo', rows, 'Id', 'LocationName', { zero: '', keep: true });
            }).catch(HRM.fail);
        };

        /** FormHelper.ValidateControls (FormValidationCustomerDetail) - texts of the helper. */
        S.validate = function (serviceProviderId) {
            function c(id, name) { if (!HRM.comboVal(id)) { HRM.box(name + ' field is required'); HRM.focus(id); return false; } return true; }
            function t(id, name) { if (!HRM.val(id).trim()) { HRM.box(name + ' field is required'); HRM.focus(id); return false; } return true; }
            function d(id, name) { if (HRM.num(HRM.val(id)) === 0) { HRM.box(name + ' must be a non-zero number'); HRM.focus(id); return false; } return true; }
            if (!c(serviceProviderId, 'Service Provider') || !t('txtPartyRefDocNo', 'Ref Doc No') || !c('CmbItemName', 'Item Name') ||
                !c('CmbTransactionCurrencyDetail', 'Transaction Currency') || !d('txtTransactionExchangeRateDetail', 'Transaction Exchange Rate') ||
                !d('txtServiceRate', 'Service Rate') || !c('CmbRateBaseType', 'Rate Base') || !d('txtQty', 'Qty') || !d('txtNetWeight', 'NetWeight') ||
                !d('txtTcyAmount', 'Tcy Amount') || !d('txtLcyAmountDetail', 'Lcy Amount') || !d('txtTotalAmount', 'Total Tcy Amount') ||
                !c('CmbLocationType', 'Location Type') || !c('CmbLocationFrom', 'Location From') || !c('CmbLocationTo', 'Location To')) return false;
            if (HRM.comboVal('CmbLocationFrom') === HRM.comboVal('CmbLocationTo')) { HRM.focus('CmbLocationTo'); HRM.box("From Location Can't be Same as Location To"); return false; }
            return true;
        };
        /** FillCustomerDetailRow(dr). */
        S.fillRow = function (dr) {
            dr.PartyRefDocNo = HRM.val('txtPartyRefDocNo');
            dr.PartyRefDocDate = HRM.val('txtPartyRefDocDate');
            var itemId = HRM.comboVal('CmbItemName'), it = itemId > 0 ? S.item(itemId) : null;
            dr.ItemId = itemId;
            dr.ServiceItemName = it ? HRM.str(it.ItemName) : '';
            dr.ServiceItemCode = it ? HRM.str(it.ItemCode) : '';
            dr.TransactionCurrencyId = HRM.comboVal('CmbTransactionCurrencyDetail');
            dr.TransactionCurrency = L.text('CmbTransactionCurrencyDetail');
            dr.TransactionExchangeRate = HRM.num(HRM.val('txtTransactionExchangeRateDetail'));
            dr.ServiceRate = HRM.num(HRM.val('txtServiceRate'));
            dr.RateBaseId = HRM.comboVal('CmbRateBaseType');
            dr.RateBase = L.text('CmbRateBaseType');
            dr.RateBaseEquivalent = rateEq();
            dr.Qty = HRM.num(HRM.val('txtQty'));
            dr.NetWeight = HRM.num(HRM.val('txtNetWeight'));
            var tcy = HRM.num(HRM.val('txtTcyAmount'));
            dr.TcyAmount = tcy;
            dr.LcyAmount = HRM.num(HRM.val('txtLcyAmountDetail'));
            var taxId = HRM.comboVal('CmbTaxName'), pct = HRM.num(HRM.val('txtTaxPercnt')), tax = tcy * pct / 100;
            dr.TaxNameId = taxId;
            dr.TaxName = taxId > 0 ? L.text('CmbTaxName') : '';
            dr['Tax%'] = pct;
            dr.TaxAmount = tax;
            dr.TotalTcyAmount = tcy + tax;
            dr.LocationTypeId = HRM.comboVal('CmbLocationType');
            dr.LocationType = L.text('CmbLocationType');
            dr.LocationFromId = HRM.comboVal('CmbLocationFrom');
            dr.LocationFrom = L.text('CmbLocationFrom');
            dr.LocationToId = HRM.comboVal('CmbLocationTo');
            dr.LocationTo = L.text('CmbLocationTo');
            dr.Remarks = HRM.val('txtRemarksDetail');
            return dr;
        };
        /** grdCustomerDetail_DoubleClick: the row back into the entry fields. */
        S.loadRow = function (row, index) {
            S.updateIndex = index;
            HRM.setCombo('CmbMasterItem', 0);
            S.itemsFill(0);
            HRM.setVal('txtPartyRefDocNo', HRM.str(row.PartyRefDocNo));
            HRM.setVal('txtPartyRefDocDate', HRM.day(row.PartyRefDocDate));
            HRM.setCombo('CmbItemName', row.ItemId);
            HRM.setCombo('CmbTransactionCurrencyDetail', row.TransactionCurrencyId);
            HRM.setVal('txtTransactionExchangeRateDetail', HRM.str(row.TransactionExchangeRate));
            HRM.setVal('txtServiceRate', HRM.str(row.ServiceRate));
            HRM.setCombo('CmbRateBaseType', row.RateBaseId);
            HRM.setVal('txtQty', HRM.str(row.Qty));
            HRM.setVal('txtNetWeight', HRM.str(row.NetWeight));
            HRM.setVal('txtTcyAmount', HRM.str(row.TcyAmount));
            HRM.setVal('txtLcyAmountDetail', HRM.str(row.LcyAmount));
            return S.itemLeave().then(function () {
                HRM.setCombo('CmbTaxName', row.TaxNameId);
                HRM.setVal('txtTaxPercnt', HRM.str(row['Tax%']));
                HRM.setCombo('CmbLocationType', row.LocationTypeId);
                return S.locationLeave();
            }).then(function () {
                HRM.setCombo('CmbLocationFrom', row.LocationFromId);
                HRM.setCombo('CmbLocationTo', row.LocationToId);
                HRM.setVal('txtRemarksDetail', HRM.str(row.Remarks));
                HRM.show('BtnAddCustomer', false); HRM.show('BtnUpdateCustmer', true); HRM.show('BtnCancelCustomer', true);
                HRM.focus('txtPartyRefDocNo');
            });
        };
        /** ResetCustomerDetail. */
        S.reset = function () {
            ['txtPartyRefDocNo', 'txtServiceRate', 'txtTcyAmount', 'txtLcyAmountDetail', 'txtRemarksDetail'].forEach(function (i) { HRM.setVal(i, ''); });
            ['CmbItemName', 'CmbRateBaseType', 'CmbLocationType', 'CmbLocationFrom', 'CmbLocationTo'].forEach(function (i) { HRM.setCombo(i, 0); });
            S.updateIndex = -1;
            HRM.show('BtnAddCustomer', true); HRM.show('BtnCancelCustomer', false); HRM.show('BtnUpdateCustmer', false);
            HRM.focus('txtPartyRefDocNo');
        };
        /** The control events of the group. */
        S.wire = function () {
            HRM.$('txtTransactionExchangeRateDetail').addEventListener('input', S.calc);
            ['txtServiceRate', 'txtQty'].forEach(function (i) { HRM.$(i).addEventListener('input', S.rateChanged); });
            HRM.$('txtNetWeight').addEventListener('change', S.rateChanged);
            HRM.$('txtTaxPercnt').addEventListener('input', function () { S.tax(true); });
            HRM.$('txtTaxPercnt').addEventListener('change', function () { S.tax(false); });
            L.onLeave('CmbTransactionCurrencyDetail', S.currencyChanged);
            L.onLeave('CmbRateBaseType', S.rateBaseLeave);
            L.onLeave('CmbTaxName', function () { S.tax(); });
            L.onLeave('CmbItemName', S.itemLeave);
            L.onLeave('CmbLocationType', S.locationLeave);
            L.onLeave('CmbMasterItem', function () { S.itemsFill(HRM.comboVal('CmbMasterItem')); });
            HRM.$('RadMasterItem').addEventListener('change', function () { S.masterBind(); S.itemsFill(HRM.comboVal('CmbMasterItem')); });
            HRM.$('RadCategory').addEventListener('change', function () { S.masterBind(); S.itemsFill(HRM.comboVal('CmbMasterItem')); });
            HRM.$('RadItemNamePackListDetail').addEventListener('change', function () { S.itemsFill(HRM.comboVal('CmbMasterItem')); });
            HRM.$('RadItemCodePackListDetail').addEventListener('change', function () { S.itemsFill(HRM.comboVal('CmbMasterItem')); });
        };
        return S;
    };

    /**
     * CustomerDetail_AmountUpdateInRow / CustomerDetail_TaxUpdateInRow of the detail grid (the grid path uses Qty * 100 for
     * rate base 3 where the entry fields use * 1000 - both as in the desktop).
     */
    L.rowTax = function (r, amountRound) {
        var tcy = HRM.num(r.TcyAmount), pct = HRM.num(r['Tax%']) > 0 ? HRM.num(r['Tax%']) : (HRM.int(r.TaxNameId) > 0 ? HRM.num(r['Tax%']) : 0);
        var tax = tcy * pct / 100.0;
        r['Tax%'] = pct;
        r.TaxAmount = L.roundEven(tax, amountRound);
        r.TotalTcyAmount = L.roundEven(tcy + tax, amountRound);
    };
    L.rowAmount = function (r, amountRound) {
        var ex = HRM.num(r.TransactionExchangeRate), rate = HRM.num(r.ServiceRate), eq = HRM.num(r.RateBaseEquivalent), qty = HRM.num(r.Qty), id = HRM.int(r.RateBaseId);
        if (id === 3 || id === 4) r.NetWeight = id === 3 ? qty * 100 : qty;
        if (id === 5) r.Qty = 1;
        var tcy = rate * eq * qty;
        r.TcyAmount = tcy;
        r.LcyAmount = tcy * ex;
        L.rowTax(r, amountRound);
    };

    /** Rights: a button without the right is disabled (formright.DoHave...). */
    L.rights = function (r, map) { HRM.applyRights(r, map); };
})(window);
