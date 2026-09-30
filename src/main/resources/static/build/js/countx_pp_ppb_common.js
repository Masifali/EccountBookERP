/* ============================================================================================
 * countx_pp_ppb_common.js - helpers shared by the Party Processing transaction pages of this port
 * (gate-pass, grn-gdn-store, grn-gdn, grn-purchase, gdn-sale, advance-delivery-order).
 * Built on countx_hrm.js (window.HRM); exports window.PPB.
 *
 *   PPB.tabs()                  the desktop tabControl1 (Form / History) - Ctrl+T toggles
 *   PPB.btnCell(act, text)      a GridEX ButtonDisplayMode.Always cell; PPB.onButton(grid, table, fn)
 *   PPB.fmt(v, pattern)         .NET custom numeric formats used by the grids ("#,##0.###", "#,##.##" ...)
 *   PPB.historyFilters(prefix)  the History tab filter group (dates with their check boxes, doc nos, radios)
 *   PPB.printRows(title, rows)  slip rows -> the grid-to-PDF printer (print-rpt.js printRowsJasper)
 *   PPB.leave(id, fn)           UltraCombo.Leave on a searchable combo
 * ============================================================================================ */
(function (global) {
    'use strict';
    var PPB = {};
    global.PPB = PPB;

    PPB.mode = function () { return HRM.param('mode') || ''; };

    // ------------------------------------------------------------------ tabs
    PPB.tabs = function (onChange) {
        var btns = document.querySelectorAll('.hrm-tab[data-tab]');
        function show(name) {
            btns.forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-tab') === name); });
            document.querySelectorAll('.hrm-tab-page[data-page]').forEach(function (p) { p.classList.toggle('is-active', p.getAttribute('data-page') === name); });
            if (onChange) onChange(name);
        }
        btns.forEach(function (b) { b.addEventListener('click', function () { show(b.getAttribute('data-tab')); }); });
        PPB.showTab = show;
        PPB.currentTab = function () { var a = document.querySelector('.hrm-tab.is-active'); return a ? a.getAttribute('data-tab') : 'form'; };
        PPB.toggleTab = function () { show(PPB.currentTab() === 'form' ? 'history' : 'form'); };
        return show;
    };

    // ------------------------------------------------------------------ grid buttons
    PPB.btnCell = function (act, text, disabled) {
        return '<button type="button" class="win-btn-action ppb-cell-btn" data-act="' + act + '"' + (disabled ? ' disabled' : '') + '>' + HRM.esc(text) + '</button>';
    };
    PPB.onButton = function (grid, tableId, fn) {
        HRM.$(tableId).addEventListener('click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var tr = b.closest('tr[data-i]'); if (!tr) return;
            var i = +tr.getAttribute('data-i');
            grid.select(i);
            fn(grid.rows()[i], b.getAttribute('data-act'), i, b);
        });
    };

    // ------------------------------------------------------------------ numbers
    /**
     * .NET custom numeric format of the grids / text boxes: "#,##0.###" (grouped, 0-3 decimals, "0" for zero),
     * "#,##.##" (grouped, 0-2 decimals, "" for zero), "#,##0.####", "0,0".
     */
    PPB.fmt = function (v, pattern) {
        if (v === null || v === undefined || v === '') return '';
        var n = HRM.num(v);
        var p = pattern || '#,##0.###';
        var dot = p.indexOf('.');
        var intPat = dot < 0 ? p : p.substring(0, dot);
        var decPat = dot < 0 ? '' : p.substring(dot + 1);
        var fixed = (decPat.match(/0/g) || []).length;       // "0" places are always shown, "#" places only when not zero
        var s = Math.abs(n).toFixed(decPat.length);
        var parts = s.split('.');
        var fr = parts[1] || '';
        while (fr.length > fixed && fr.charAt(fr.length - 1) === '0') fr = fr.substring(0, fr.length - 1);
        var ip = parts[0];
        if (intPat.charAt(intPat.length - 1) !== '0' && ip === '0') ip = '';
        if (intPat.indexOf(',') >= 0) ip = ip.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        var out = ip + (fr ? '.' + fr : '');
        if (out === '') return '';
        var neg = n < 0 && /[1-9]/.test(out);
        return (neg ? '-' : '') + out;
    };
    PPB.numCol = function (key, caption, pattern, sum) {
        return { key: key, caption: caption || key, type: 'num', sum: sum !== false, decimals: 3,
                 render: function (v) { return HRM.esc(PPB.fmt(v, pattern || '#,##0.###')); } };
    };
    /** .NET double.ToString(): plain shortest text. */
    PPB.net = function (v) {
        var n = HRM.num(v);
        if (!isFinite(n)) return '0';
        var s = String(+n.toPrecision(15));
        return s.indexOf('e') >= 0 ? n.toFixed(6).replace(/\.?0+$/, '') : s;
    };
    /** Math.Round(v, d) with MidpointRounding.ToEven. */
    PPB.round = function (v, d) {
        var p = Math.pow(10, d || 0), x = HRM.num(v) * p;
        var r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r / p;
    };

    // ------------------------------------------------------------------ history filter group
    PPB.historyFilters = function (extra) {
        var o = {
            fromDate: HRM.val('FromDateHistory'), toDate: HRM.val('ToDateHistory'),
            fromChecked: HRM.checked('chkFromDateHistory'), toChecked: HRM.checked('chkToDateHistory'),
            fromDocNo: HRM.val('txtFromDocNoHistory'), toDocNo: HRM.val('txtToDocNoHistory'),
            dateType: (document.querySelector('input[name="histDateType"]:checked') || { value: 'doc' }).value
        };
        Object.keys(extra || {}).forEach(function (k) { o[k] = extra[k]; });
        return o;
    };
    /** btnNewHistory_Click: dates back to today - n days / today, doc nos and party cleared. */
    PPB.resetHistoryFilters = function (days, comboIds) {
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -(days > 0 ? days : 3)));
        HRM.setVal('ToDateHistory', HRM.today());
        HRM.check('chkFromDateHistory', true); HRM.check('chkToDateHistory', true);
        HRM.setVal('txtFromDocNoHistory', ''); HRM.setVal('txtToDocNoHistory', '');
        (comboIds || []).forEach(function (id) { HRM.setCombo(id, 0); });
        var doc = document.querySelector('input[name="histDateType"][value="doc"]'); if (doc) doc.checked = true;
    };

    // ------------------------------------------------------------------ print
    /** The procedure's slip rows as a PDF (print-rpt.js grid printer); the desktop shows "No Record Found For Display" when empty. */
    PPB.printRows = function (title, rows) {
        if (!rows || !rows.length) { HRM.box('No Record Found For Display'); return; }
        var clean = rows.map(function (r) {
            var o = {};
            Object.keys(r).forEach(function (k) {
                var v = r[k];
                if (typeof v === 'number' && /date|time/i.test(k) && v > 100000000000) v = HRM.fmtDate(v);
                o[k] = v === null || v === undefined ? '' : v;
            });
            return o;
        });
        if (global.printRowsJasper) global.printRowsJasper(title, clean);
        else HRM.box('The PDF printer (print-rpt.js) is not loaded on this page.');
    };

    // ------------------------------------------------------------------ combos
    /** UltraCombo.Leave: a pick fires change on the hidden select; focus leaving the combo's wrap is the Leave itself. */
    PPB.leave = function (id, fn) {
        var el = HRM.$(id); if (!el) return;
        var busy = false;
        function run() { if (busy) return; busy = true; setTimeout(function () { busy = false; }, 50); fn(); }
        el.addEventListener('change', run);
        document.addEventListener('focusout', function (e) {
            var w = e.target && e.target.closest ? e.target.closest('.dtcombo-wrap') : null;
            if (!w || !w.querySelector('#' + id)) return;
            if (e.relatedTarget && w.contains(e.relatedTarget)) return;
            setTimeout(run, 0);
        });
    };
    /** Row of a combo by value from a rows array. */
    PPB.rowOf = function (rows, key, v) {
        for (var i = 0; i < (rows || []).length; i++) if (String(HRM.col(rows[i], key)) === String(v)) return rows[i];
        return null;
    };
    /** Upper-case as typed (txtvehicleno_TextChanged). */
    PPB.upper = function (id) {
        var e = HRM.$(id); if (!e) return;
        e.addEventListener('input', function () { var p = e.selectionStart; e.value = e.value.toUpperCase(); try { e.setSelectionRange(p, p); } catch (x) { } });
    };
    /** Keeps only digits / one dot (CommonServices.OnlytextdecimelFunction) or digits (OnlytextNumberFunction). */
    PPB.guard = function (id, kind) {
        var e = HRM.$(id); if (!e) return;
        e.addEventListener('input', function () {
            var v = e.value;
            var n = kind === 'int' ? v.replace(/[^\d]/g, '') : kind === 'signed' ? v.replace(/[^\d.\-]/g, '').replace(/(?!^)-/g, '').replace(/(\..*)\./g, '$1')
                : v.replace(/[^\d.]/g, '').replace(/(\..*)\./g, '$1');
            if (n !== v) e.value = n;
        });
    };

    // ------------------------------------------------------------------ Load Data popup
    /**
     * LoadavailableTransactionsForIssuancePartyProcessing: PPB.loader({ get, post, stockPartyId, canLoad, onLoad(rows) }) - get / post are the
     * page's API helpers ('/loader-setup', '/loader'); the search uses the form's stock party; Load hands back the picked rows.
     */
    function sel(id, label) {
        return '<div class="hrm-field hrm-stack"><label for="' + id + '">' + label + '</label><select id="' + id + '" class="win-combo dtcombo" data-dtcombo="single"></select></div>';
    }
    function round0(v) { return PPB.round(v, 0); }
    function fmt00(v) {                                                                         // Math.Round(x).ToString("0,0")
        var r = Math.abs(round0(v)), s = String(r); if (s.length < 2) s = ('0' + s).slice(-2);
        return (round0(v) < 0 ? '-' : '') + s.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
    PPB.loader = function (o) {
        var get = o.get, post = o.post, stockPartyId = o.stockPartyId, canLoad = o.canLoad, n = HRM.num;
        var m = HRM.modal({ title: 'Load Available Transactions For Issuance Party Processing', width: '1150px',
            html: '<div class="win-tool-strip">' +
                '<button type="button" id="ldNew" class="win-btn-tool"><i class="fa fa-plus"></i> <span><u>N</u>ew</span></button>' +
                '<button type="button" id="ldRefresh" class="win-btn-tool"><i class="fa fa-refresh"></i> <span><u>R</u>efresh</span></button>' +
                '</div>' +
                '<fieldset class="hrm-group"><legend>Filters</legend><div class="hrm-entry" style="grid-template-columns:repeat(auto-fill,minmax(170px,1fr))">' +
                '<div class="hrm-field hrm-stack"><label for="ldFromDate">From Date</label><input type="date" id="ldFromDate" class="win-textbox"/></div>' +
                '<div class="hrm-field hrm-stack"><label for="ldToDate">To Date</label><input type="date" id="ldToDate" class="win-textbox"/></div>' +
                sel('ldStockParty', 'Stock Party') + sel('ldRefParty', 'Reference Party') + sel('ldRefDocType', 'Ref Document Type') +
                sel('ldRefWarehouse', 'Ref Warehouse') + sel('ldWarehouse', 'Warehouse') + sel('ldJobLot', 'Job Lot') + sel('ldItem', 'Item Name') +
                sel('ldPackingType', 'Packing Type') + sel('ldCropYear', 'Crop Year') +
                '<div class="hrm-field hrm-stack"><label>&nbsp;</label><button type="button" id="ldShow" class="win-btn-action">Show</button></div>' +
                '</div></fieldset>' +
                '<div class="hrm-grid-box"><div class="hrm-subcaption"><span>Available Transactions</span></div>' +
                '<div class="hrm-grid-wrap" style="max-height:340px"><table id="ldGrid"></table></div></div>' +
                '<div class="ppb-sel" style="margin-top:6px"><label>Selected Stock</label><input type="text" id="ldSelWeight" class="win-textbox hrm-al-right" readonly>' +
                '<label>Qty</label><input type="text" id="ldSelQty" class="win-textbox hrm-al-right" readonly>' +
                '<button type="button" id="ldLoad" class="win-btn-action"' + (canLoad ? '' : ' style="display:none"') + '>Load</button></div>'
        });
        var all = [];
        var g = new HRM.Grid('ldGrid', {
            columns: [
                { key: 'Select', caption: 'Select', type: 'edit-check', width: 40 },
                { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocIdNo', hidden: true }, { key: 'RefDocSubIdNo', hidden: true },
                { key: 'RefDocType', caption: 'RefDocType', width: 70 }, { key: 'DocDate', caption: 'DocDate', type: 'date', width: 70 },
                { key: 'DocCodeNo', caption: 'DocCodeNo', type: 'int', width: 50 },
                { key: 'WarehouseId', hidden: true }, { key: 'WareHouse', caption: 'WareHouse', width: 80 },
                { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName', width: 130 }, { key: 'ItemCode', hidden: true },
                { key: 'CropYearId', hidden: true }, { key: 'CropYear', caption: 'CropYear', width: 70 },
                { key: 'JobLotId', hidden: true }, { key: 'JobLotCode', caption: 'JobLotCode', width: 85 },
                { key: 'InvPackingTypeId', hidden: true }, { key: 'PackingType', caption: 'PackingType', width: 90 },
                { key: 'ItemUomId', hidden: true }, { key: 'PackUom', caption: 'PackUom', width: 60 }, { key: 'Equivalent', hidden: true },
                PPB.numCol('QtyIn', 'QtyIn', '#,##.###'), PPB.numCol('QtyOut', 'QtyOut', '#,##.###'), PPB.numCol('QtyBalance', 'QtyBalance', '#,##.###'),
                PPB.numCol('WeightIn', 'WeightIn', '#,##.###'), PPB.numCol('WeightOut', 'WeightOut', '#,##.###'),
                PPB.numCol('WeightBalance', 'WeightBalance', '#,##.###'),
                { key: 'StockPartyId', hidden: true }, { key: 'StockParty', caption: 'StockParty', width: 140 },
                { key: 'SupplierCustomerId', hidden: true }, { key: 'ReferenceParty', caption: 'ReferenceParty', width: 140 },
                { key: 'GpNo', caption: 'GpNo', type: 'int', width: 50 }, { key: 'RefWarehouse', caption: 'RefWarehouse', width: 120 },
                { key: 'VehicleNo', caption: 'VehicleNo', width: 90 }, { key: 'JobOrderId', hidden: true },
                { key: 'JobOrderNo', caption: 'JobOrderNo', width: 130 }
            ],
            checkAll: 'Select', totals: true, filterRow: true, emptyText: '',
            onChange: function () { selected(); }
        });
        function selected() {                                                                   // SelectedWeightCalculation()
            var q = 0, w = 0;
            g.checked('Select').forEach(function (r) { q += n(r.QtyBalance); w += n(r.WeightBalance); });
            HRM.setVal('ldSelWeight', fmt00(w)); HRM.setVal('ldSelQty', fmt00(q));
        }
        function combos(o) {
            [['ldStockParty', 'StockParty'], ['ldRefParty', 'ReferenceParty'], ['ldRefDocType', 'RefDocumentType'], ['ldRefWarehouse', 'RefWarehouse'],
                ['ldWarehouse', 'Warehouse'], ['ldJobLot', 'JobLot'], ['ldItem', 'Items'], ['ldPackingType', 'PackingType'], ['ldCropYear', 'CropYear']]
                .forEach(function (p) { HRM.fill(p[0], o[p[1]] || [], 'Id', 'name', { zero: '', keep: true }); });
            HRM.setCombo('ldStockParty', stockPartyId);
            HRM.enable('ldStockParty', false);                                                  // the search uses the form's stock party
        }
        function show(btn) {                                                                    // PendingInventoryTransactionsForIssuanceLoad()
            return HRM.busy(btn, function () {
                return post('/loader', {
                    fromDate: HRM.val('ldFromDate'), toDate: HRM.val('ldToDate'), stockPartyId: stockPartyId,
                    refPartyId: HRM.comboVal('ldRefParty'), refDocumentTypeId: HRM.comboVal('ldRefDocType'), refWarehouseId: HRM.comboVal('ldRefWarehouse'),
                    warehouseId: HRM.comboVal('ldWarehouse'), jobLotId: HRM.comboVal('ldJobLot'), itemId: HRM.comboVal('ldItem'),
                    packingTypeId: HRM.comboVal('ldPackingType'), cropYearId: HRM.comboVal('ldCropYear')
                }).then(function (rows) { all = rows || []; g.set(all.map(function (r) { var c = Object.assign({}, r); c.Select = false; return c; })); selected(); })
                    .catch(HRM.fail);
            }, 'ld-show');
        }
        HRM.setVal('ldToDate', HRM.today());
        HRM.loading(get('/loader-setup')).then(function (o) {
            combos(o);
            HRM.setVal('ldFromDate', HRM.day(o.fromDate) || HRM.today());
            return show(null);
        }).catch(HRM.fail);
        HRM.$('ldShow').addEventListener('click', function () { show('ldShow'); });
        HRM.$('ldNew').addEventListener('click', function () {                                   // btnReset_Click
            HRM.setCombo('ldRefParty', 0); HRM.setCombo('ldItem', 0); g.clear(); selected();
        });
        HRM.$('ldRefresh').addEventListener('click', function () {
            HRM.busy('ldRefresh', function () { return get('/loader-setup').then(combos).catch(HRM.fail); });
        });
        m.el.addEventListener('keydown', function (e) {                                         // LoadavailableTransactionsForIssuance_KeyDown
            if (!e.ctrlKey) return;
            var k = (e.key || '').toLowerCase(), t = { s: 'ldShow', l: 'ldLoad', n: 'ldNew', r: 'ldRefresh' }[k];
            if (t && HRM.visible(t) !== false && HRM.$(t).style.display !== 'none') { e.preventDefault(); HRM.$(t).click(); }
        });
        HRM.$('ldLoad').addEventListener('click', function () {                                  // btnLoadOnInvoice_Click
            var checked = g.checked('Select');
            if (!checked.length) { HRM.box('Check the row first'); return; }
            var sID = HRM.int(checked[0].StockPartyId);
            for (var i = 0; i < checked.length; i++) {
                if (stockPartyId > 0 && HRM.int(checked[i].StockPartyId) !== sID) { HRM.box('Sorry... the Rows should be of the same Stock Party'); return; }
            }
            var ids = checked.map(function (r) { return String(HRM.int(r.RefDocSubIdNo)); });
            var docIds = checked.map(function (r) { return String(HRM.int(r.RefDocumentTypeId)); });
            var picked = all.filter(function (r) { return ids.indexOf(String(HRM.int(r.RefDocSubIdNo))) >= 0 && docIds.indexOf(String(HRM.int(r.RefDocumentTypeId))) >= 0; });
            m.close();
            o.onLoad(picked);
        });
    };
})(window);
