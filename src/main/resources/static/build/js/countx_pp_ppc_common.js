/* ============================================================================================
 * countx_pp_ppc_common.js - helpers shared by the Party Processing pages of group PpC
 * (production-job-order, production, processing-bill, stock-conversion, stock-transfer, stock-adjustment, wages-bill).
 * Built on countx_hrm.js (window.HRM); exports window.PPC.
 *
 *   PPC.tabs(onChange) / PPC.subTabs(group, onChange)   tab strips (data-tabs=group, pages data-group=group)
 *   PPC.btn(act, text) / PPC.onButton(grid, tableId, fn)   GridEX button columns
 *   PPC.fmt(v, pattern) / PPC.net(v) / PPC.round(v, d)     .NET number formats / double.ToString() / Math.Round
 *   PPC.hist(prefix)               the History filter group -> {dateMode, from, fromChecked, to, toChecked, fromDocNo, toDocNo}
 *   PPC.print(gateUrl, key, args, btn)   the screen's print gate (rights + own record), then CrystalPrint.open
 *   PPC.voucher(url, id, btn)      history "Voucher": voucher head id -> acc-118
 *   PPC.loader(opts)               LoadavailableTransactionsForIssuancePartyProcessing (issuance loader dialog)
 *   PPC.shortcuts(rows)            ShortCutKeyPopUp
 * ============================================================================================ */
(function (global) {
    'use strict';
    var PPC = {};
    global.PPC = PPC;
    function $(id) { return typeof id === 'string' ? document.getElementById(id) : id; }
    PPC.$ = $;

    // ------------------------------------------------------------------ tabs
    /** The form's main tabControl: <div class="hrm-tabs" data-tabs="main"> + pages data-group="main". */
    PPC.tabs = function (onChange) { return PPC.subTabs('main', onChange); };
    /** A secondary tab strip (data-tabs="name") whose pages carry data-group="name". */
    PPC.subTabs = function (group, onChange) {
        var strip = document.querySelector('.hrm-tabs[data-tabs="' + group + '"]');
        var btns = strip ? strip.querySelectorAll('.hrm-tab[data-tab]') : [];
        function show(name) {
            btns.forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === name); });
            document.querySelectorAll('.hrm-tab-page[data-group="' + group + '"]').forEach(function (p) {
                p.classList.toggle('is-active', p.getAttribute('data-page') === name);
            });
            if (onChange) onChange(name);
        }
        btns.forEach(function (b) { b.addEventListener('click', function () { if (!b.disabled && !b.classList.contains('is-hidden')) show(b.getAttribute('data-tab')); }); });
        return {
            show: show,
            current: function () { for (var i = 0; i < btns.length; i++) if (btns[i].classList.contains('is-active')) return btns[i].getAttribute('data-tab'); return ''; },
            showTab: function (name, on) { btns.forEach(function (b) { if (b.getAttribute('data-tab') === name) b.classList.toggle('is-hidden', !on); }); }
        };
    };

    // ------------------------------------------------------------------ grid buttons
    PPC.btn = function (act, text, disabled) {
        return '<button type="button" class="win-btn-action ppc-cell-btn" data-act="' + act + '"' + (disabled ? ' disabled' : '') + '>' + HRM.esc(text) + '</button>';
    };
    PPC.onButton = function (grid, tableId, fn) {
        $(tableId).addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            grid.select(i);
            fn(grid.rows()[i], b.getAttribute('data-act'), i, b);
        });
    };
    /** Shows / hides grid columns after the grid was built (GridEX Column.Visible at run time). */
    PPC.showCols = function (grid, keys, on) {
        keys.forEach(function (k) {
            var c = grid.columnOf(k); if (!c) return;
            c.hidden = !on;
            var th = grid.table.querySelector('thead th[data-key="' + k + '"]');
            if (th) th.style.display = on ? '' : 'none';
        });
        grid.draw();
    };
    PPC.btnCol = function (key, text, width) {
        return { key: key, caption: text, width: width || 50, render: function () { return PPC.btn(key, text); } };
    };

    // ------------------------------------------------------------------ numbers
    /** .NET custom numeric format: "#,##0.###", "#,##.###", "#,##", "0,0", "#,##0.##" ... */
    PPC.fmt = function (v, pattern) {
        if (v === null || v === undefined || v === '') return '';
        var n = HRM.num(v);
        var p = pattern || '#,##0.###';
        var dot = p.indexOf('.');
        var intPat = dot < 0 ? p : p.substring(0, dot);
        var decPat = dot < 0 ? '' : p.substring(dot + 1);
        var fixed = (decPat.match(/0/g) || []).length;
        var s = Math.abs(n).toFixed(decPat.length);
        var parts = s.split('.');
        var fr = parts[1] || '';
        while (fr.length > fixed && fr.charAt(fr.length - 1) === '0') fr = fr.substring(0, fr.length - 1);
        var ip = parts[0];
        if (intPat.charAt(intPat.length - 1) !== '0' && ip === '0') ip = '';
        if (intPat.indexOf(',') >= 0) ip = ip.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        var out = ip + (fr ? '.' + fr : '');
        if (out === '') return '';
        return (n < 0 && /[1-9]/.test(out) ? '-' : '') + out;
    };
    PPC.numCol = function (key, caption, pattern, extra) {
        var c = { key: key, caption: caption || key, type: 'num', render: function (v) { return HRM.esc(PPC.fmt(v, pattern || '#,##0.###')); } };
        Object.keys(extra || {}).forEach(function (k) { c[k] = extra[k]; });
        return c;
    };
    /** double.ToString() */
    PPC.net = function (v) {
        var n = HRM.num(v);
        if (!isFinite(n)) return '0';
        var s = String(+n.toPrecision(15));
        return s.indexOf('e') >= 0 ? n.toFixed(6).replace(/\.?0+$/, '') : s;
    };
    /** Math.Round(v, d) - MidpointRounding.ToEven */
    PPC.round = function (v, d) {
        var p = Math.pow(10, d || 0), x = +(HRM.num(v) * p).toPrecision(15);
        var r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r / p;
    };
    /** Conversion.ToDouble of a text box ("1,234.5" -> 1234.5) */
    PPC.dbl = function (id) { return HRM.num(String(HRM.val(id)).replace(/,/g, '')); };
    /** Keeps only digits / one dot (OnlytextdecimelFunction) or digits (OnlytextNumberFunction). */
    PPC.guard = function (id, kind) {
        var e = $(id); if (!e) return;
        e.addEventListener('input', function () {
            var v = e.value;
            var n = kind === 'int' ? v.replace(/[^\d]/g, '') : v.replace(/[^\d.]/g, '').replace(/(\..*)\./g, '$1');
            if (n !== v) e.value = n;
        });
    };

    // ------------------------------------------------------------------ history filters
    /** ids: {from, fromChk, to, toChk, docFrom, docTo, radio} (defaults FromDateHistory ...). */
    PPC.hist = function (ids, extra) {
        ids = ids || {};
        var r = document.querySelector('input[name="' + (ids.radio || 'histDateMode') + '"]:checked');
        var o = {
            dateMode: r ? r.value : 'doc',
            from: HRM.val(ids.from || 'FromDateHistory'), fromChecked: ids.fromChk === false ? true : HRM.checked(ids.fromChk || 'chkFromDateHistory'),
            to: HRM.val(ids.to || 'ToDateHistory'), toChecked: ids.toChk === false ? true : HRM.checked(ids.toChk || 'chkToDateHistory'),
            fromDocNo: HRM.val(ids.docFrom || 'txtFromDocNoHistory'), toDocNo: HRM.val(ids.docTo || 'txtToDocNoHistory')
        };
        Object.keys(extra || {}).forEach(function (k) { o[k] = extra[k]; });
        return o;
    };

    // ------------------------------------------------------------------ prints
    PPC.print = function (gateUrl, key, args, btn) {
        return HRM.busy(btn, function () {
            return HRM.get(gateUrl, args).then(function (g) {
                var a = {}; Object.keys(args || {}).forEach(function (k) { if (k !== 'history') a[k] = args[k]; });
                Object.keys(g || {}).forEach(function (k) { if (a[k] === undefined) a[k] = g[k]; });
                if (global.CrystalPrint) return global.CrystalPrint.open(key, a);
                HRM.box('The print module (countx_crystal_print.js) is not loaded on this page.');
            }).catch(HRM.fail);
        });
    };
    /** CommonServices.VoucherReport_118(CommonServices.VoucherHeadIdGet(Id, DocType)) */
    PPC.voucher = function (url, id, btn) {
        return HRM.busy(btn, function () {
            return HRM.get(url, { id: id }).then(function (r) {
                var vid = HRM.int(r && r.voucherHeadId);
                if (!vid) { HRM.box('Voucher Not Found'); return; }
                if (global.CrystalPrint) return global.CrystalPrint.open('acc-118', { id: vid });
            }).catch(HRM.fail);
        });
    };

    PPC.shortcuts = function (rows) {
        var html = '<table class="win-grid hrm-grid"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>' +
            rows.map(function (r) { return '<tr><td>' + HRM.esc(r[0]) + '</td><td>' + HRM.esc(r[1]) + '</td></tr>'; }).join('') + '</tbody></table>';
        HRM.modal({ title: 'ShortCut Keys', width: 'min(520px, 96vw)', html: '<div class="hrm-grid-wrap">' + html + '</div>' });
    };

    // ------------------------------------------------------------------ issuance loader
    /**
     * LoadavailableTransactionsForIssuancePartyProcessing: opts.page (API page key), opts.stockPartyId (the caller's
     * StockPartyId property), opts.onLoad(rows) with the checked rows (dtIssuance), nothing when closed.
     */
    PPC.loader = function (opts) {
        var html =
            '<div class="win-tool-strip">' +
            '<button type="button" class="win-btn-tool" data-l="new"><i class="fa fa-plus"></i> <span><u>N</u>ew</span></button>' +
            '<button type="button" class="win-btn-tool" data-l="refresh"><i class="fa fa-refresh"></i> <span><u>R</u>efresh</span></button></div>' +
            '<div class="hrm-caption"><span>Available Transactions For Issuance</span></div>' +
            '<div class="hrm-entry ppc-loader-grid">' +
            '<div class="hrm-field"><label>From &amp; To Date</label><div class="ppc-pair"><input type="date" class="win-textbox" data-f="fromDate"><input type="date" class="win-textbox" data-f="toDate"></div></div>' +
            '<div class="hrm-field"><label>Ref Warehouse</label><select class="win-combo dtcombo" data-dtcombo="single" data-f="refWarehouseId" data-src="refWarehouses"></select></div>' +
            '<div class="hrm-field"><label>Item Name</label><select class="win-combo dtcombo" data-dtcombo="single" data-f="itemId" data-src="items"></select></div>' +
            '<div class="hrm-field"><label>Selected Qty</label><input type="text" class="win-textbox hrm-al-right" data-f="selQty" readonly tabindex="-1"></div>' +
            '<div class="hrm-field"><label>Reference Party</label><select class="win-combo dtcombo" data-dtcombo="single" data-f="referencePartyId" data-src="referenceParties"></select></div>' +
            '<div class="hrm-field"><label>WareHouse</label><select class="win-combo dtcombo" data-dtcombo="single" data-f="warehouseId" data-src="warehouses"></select></div>' +
            '<div class="hrm-field"><label>Packing Type</label><select class="win-combo dtcombo" data-dtcombo="single" data-f="packingTypeId" data-src="packingTypes"></select></div>' +
            '<div class="hrm-field"><label>Selected Wt</label><input type="text" class="win-textbox hrm-al-right" data-f="selWt" readonly tabindex="-1"></div>' +
            '<div class="hrm-field"><label>Ref DocumentT ype</label><select class="win-combo dtcombo" data-dtcombo="single" data-f="refDocumentTypeId" data-src="refDocTypes"></select></div>' +
            '<div class="hrm-field"><label>JobLot</label><select class="win-combo dtcombo" data-dtcombo="single" data-f="jobLotId" data-src="jobLots"></select></div>' +
            '<div class="hrm-field"><label>CropYear</label><div class="hrm-inline" style="flex-wrap:nowrap"><div class="hrm-grow"><select class="win-combo dtcombo" data-dtcombo="single" data-f="cropYearId" data-src="cropYears"></select></div>' +
            '<button type="button" class="win-btn-action" data-l="search">Search</button><button type="button" class="win-btn-action" data-l="load">Load</button></div></div>' +
            '<div class="hrm-field"></div>' +
            '</div>' +
            '<div class="hrm-grid-box"><div class="hrm-subcaption"><span>Filtere Records</span><button type="button" class="hrm-fs-btn" data-hrm-fullscreen title="Full screen"><i class="fa fa-expand"></i></button></div>' +
            '<div class="hrm-grid-wrap"><table class="ppc-loader-table"></table></div></div>';
        var loaded = false;
        var m = HRM.modal({ title: 'Available Transactions For Issuance', width: 'min(1180px, 97vw)', html: html,
            onClose: function () { if (!loaded && opts.onClose) opts.onClose(); } });
        var body = m.body;
        function f(name) { return body.querySelector('[data-f="' + name + '"]'); }
        var n3 = function (k, c) { return PPC.numCol(k, c, '#,##.###'); };
        var tbl = body.querySelector('.ppc-loader-table');
        var grid = new HRM.Grid(tbl, {
            filterRow: true, checkAll: 'Select', emptyText: '',
            columns: [
                { key: 'Select', caption: '', type: 'edit-check', width: 30 },
                { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocIdNo', hidden: true }, { key: 'RefDocSubIdNo', hidden: true },
                { key: 'RefDocType', caption: 'RefDocType' }, { key: 'DocDate', caption: 'DocDate', type: 'date' }, { key: 'DocCodeNo', caption: 'DocCodeNo', type: 'int' },
                { key: 'WarehouseId', hidden: true }, { key: 'WareHouse', caption: 'WareHouse' }, { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName' },
                { key: 'ItemCode', hidden: true }, { key: 'CropYearId', hidden: true }, { key: 'CropYear', caption: 'CropYear' }, { key: 'JobLotId', hidden: true },
                { key: 'JobLotCode', caption: 'JobLotCode' }, { key: 'InvPackingTypeId', hidden: true }, { key: 'PackingType', caption: 'PackingType' },
                { key: 'ItemUomId', hidden: true }, { key: 'PackUom', caption: 'PackUom' }, { key: 'Equivalent', hidden: true },
                n3('QtyIn', 'QtyIn'), n3('QtyOut', 'QtyOut'), n3('QtyBalance', 'QtyBalance'), n3('WeightIn', 'WeightIn'), n3('WeightOut', 'WeightOut'),
                n3('WeightBalance', 'WeightBalance'), { key: 'StockPartyId', hidden: true }, { key: 'StockParty', caption: 'StockParty' },
                { key: 'SupplierCustomerId', hidden: true }, { key: 'ReferenceParty', caption: 'ReferenceParty' }, { key: 'GpNo', caption: 'GpNo', type: 'int' },
                { key: 'RefWarehouse', caption: 'RefWarehouse' }, { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'JobOrderId', hidden: true },
                { key: 'JobOrderNo', caption: 'JobOrderNo' }
            ],
            onChange: function () { selected(); }
        });
        tbl.addEventListener('change', function (e) { if (e.target.getAttribute('data-checkall')) setTimeout(selected, 0); });
        function selected() {                                           // SelectedWeightCalculation
            var q = 0, w = 0;
            grid.checked('Select').forEach(function (r) { q += HRM.num(HRM.col(r, 'QtyBalance')); w += HRM.num(HRM.col(r, 'WeightBalance')); });
            f('selWt').value = PPC.fmt(PPC.round(w, 0), '0,0');
            f('selQty').value = PPC.fmt(PPC.round(q, 0), '0,0');
        }
        var combos = {};
        function fillCombos() {                                         // ComboFill
            return HRM.get('/api/party-processing/loader/combos', { page: opts.page }).then(function (c) {
                combos = c || {};
                body.querySelectorAll('select[data-src]').forEach(function (s) {
                    var keep = s.value;
                    HRM.fill(s, combos[s.getAttribute('data-src')] || [], 'Id', 'name', { zero: '' });
                    if (keep && HRM.hasOption(s, keep)) s.value = keep;
                });
                HRM.refreshCombos();
                return c;
            });
        }
        function search(btn) {                                          // PendingInventoryTransactionsForIssuanceLoad
            return HRM.busy(btn, function () {
                var b = { fromDate: f('fromDate').value, toDate: f('toDate').value, stockPartyId: opts.stockPartyId || 0 };
                ['referencePartyId', 'refDocumentTypeId', 'refWarehouseId', 'warehouseId', 'jobLotId', 'itemId', 'packingTypeId', 'cropYearId']
                    .forEach(function (k) { b[k] = HRM.comboVal(f(k)); });
                return HRM.post('/api/party-processing/loader/list?page=' + encodeURIComponent(opts.page), b).then(function (rows) {
                    grid.set((rows || []).map(function (r) { r.Select = false; return r; }));
                    selected();
                }).catch(HRM.fail);
            }, 'ppcLoaderSearch');
        }
        function load() {                                               // btnLoadOnInvoice_Click
            var rows = grid.checked('Select');
            if (!rows.length) { HRM.box('Check the row first'); return; }
            var sID = HRM.int(HRM.col(rows[0], 'StockPartyId'));
            for (var i = 0; i < rows.length; i++) {
                if (HRM.int(opts.stockPartyId) > 0 && HRM.int(HRM.col(rows[i], 'StockPartyId')) !== sID) { HRM.box('Sorry... the Rows should be of the same Stock Party'); return; }
            }
            loaded = true;
            m.close();
            opts.onLoad(rows.map(function (r) { var o = Object.assign({}, r); delete o.Select; return o; }));
        }
        body.querySelector('[data-l="search"]').addEventListener('click', function (e) { search(e.currentTarget); });
        body.querySelector('[data-l="load"]').addEventListener('click', load);
        body.querySelector('[data-l="refresh"]').addEventListener('click', function (e) { HRM.busy(e.currentTarget, function () { return fillCombos().catch(HRM.fail); }); });
        body.querySelector('[data-l="new"]').addEventListener('click', function () {   // btnReset_Click
            HRM.setCombo(f('referencePartyId'), 0); HRM.setCombo(f('itemId'), 0); grid.clear(); selected();
        });
        body.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var k = (e.key || '').toLowerCase();
            if (k === 's') { e.preventDefault(); search(null); }
            else if (k === 'l') { e.preventDefault(); load(); }
            else if (k === 'n') { e.preventDefault(); body.querySelector('[data-l="new"]').click(); }
            else if (k === 'r') { e.preventDefault(); body.querySelector('[data-l="refresh"]').click(); }
        });
        f('toDate').value = HRM.today();
        f('fromDate').value = HRM.today();
        HRM.loading(fillCombos().then(function (c) {                   // LoadInvoices_Load
            if (c && c.fromDate) f('fromDate').value = HRM.day(c.fromDate);
            return search(null);
        }).catch(HRM.fail));
        return m;
    };
})(window);
