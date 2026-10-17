/* ============================================================================================
 * Screen 929 "Production Manual" - Architecture.WinApp.FeedMill.Production.frmProductionManualFeed (DocumentTypeId 1122).
 * Desktop line numbers (.cs): frmBillOfMaterial_Manufacturing_Load:463, ProductionTypeFill:653, ShiftComboFill:685, PlantFeader:730, ItemFillFromBom:807, BOMNobyItemFill:923,
 * FormValidation:1321, FillInputDetailForMultiBom:1185, Calculation, UpdateOverHeadByOutput, UpdateOutputRates, IsDuplicateRowExists:2739, Insert:2809, ReadById:3334,
 * btnDelete_Click:3452, VoucherReprot:3558, HistoryGridFill:3781, DatagridHistoryDetailbind:4084, grdInputMaterial_KeyDown:4600, UpdateRowStockAndRate:5799.
 * API /production/manual-feed/api.
 * Not ported: Attachment dialog (DMS), shortcut-keys dialog, frmGenShift pop-up, grid layout files, Auto Update / Re-Processing (admin), Formulas button (empty in the desktop).
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/manual-feed/api';
    var $ = W3B.$, n = W3B.num, I = W3B.int;

    var look = {}, rights = {};
    var recId = 0, saveAsMode = false, editIdx = -1;
    var cfg = { restrictSingle: false, packing: false, decimals: 2 };
    var L = { wh: [], uoms: [], items: [], consItems: [], accounts: [], bomItems: [], itemBoms: [], types: [] };
    var G = { inp: [], out: [], oh: [], con: [] };
    var removed = { inp: [], out: [], con: [] };
    var hist = [];

    // ----------------------------------------------------------------------------- helpers
    function alertMsg(m) { W3B.box(m); }
    function v(id) { return $(id).value; }
    function sel(id) { return I(v(id)); }
    function fmt(x, p) { return n(x).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: p === undefined ? 3 : p }); }
    function roundAway(x, d) { var s = x < 0 ? -1 : 1, f = Math.pow(10, d); return s * Math.round(Math.abs(x) * f + 1e-9) / f; }
    function uomsOf(itemId) { return L.uoms.filter(function (u) { return I(u.ItemId) === I(itemId); }); }
    function uomName(u) { return u.UOMCode || u.UOMDescription || ''; }
    function sum(a, f) { return a.reduce(function (s, x) { return s + n(x[f]); }, 0); }
    function typeId() { return sel('CmbProductionType'); }
    function freeType() { var t = typeId(); return t === 4 || t === 5; }                  // types 4 and 5: any item, free UOM and quantity, no BOM

    var BLANK = {
        inp: function () { return { Id: 0, BomHeaderId: 0, BomNo: '', BomInputId: 0, WarehouseId: 0, WarehouseName: '', ItemId: 0, ItemName: '', ItemUomId: 0, Uom: 0, UomCode: '', BalanceStock: 0, BomQty: 0, ItemQty: 0, PrctOfTotalQty: 0, NetWeight: 0, BomRate: 0, Rate: 0, RateUomId: 0, RateUom: 0, RateUomCode: '', Amount: 0, BomMinQty: 0, BomMaxQty: 0 }; },
        con: function () { return { Id: 0, BomHeaderId: 0, BomNo: '', BomConsumptionId: 0, WarehouseId: 0, WarehouseName: '', ItemId: 0, ItemName: '', ParentCategory: '', ItemUomId: 0, Uom: 0, UomCode: '', BalanceStock: 0, BomQty: 0, ItemQty: 0, NetWeight: 0, BomRate: 0, Rate: 0, RateUomId: 0, RateUom: 0, RateUomCode: '', Amount: 0, OutPutItemId: 0 }; },
        oh: function () { return { Id: 0, BomHeaderId: 0, BomNo: '', BomOverHeadId: 0, OverHeadTypeId: 0, OverHeadAccountId: 0, OverHeadAccount: '', PercentOfTotalInput: 0, BomQty: 0, Qty: 0, Rate: 0, BomAmount: 0, Amount: 0, OhRemarks: '' }; }
    };
    function ensureBlanks() { ['inp', 'con', 'oh'].forEach(function (g) { if (!G[g].length) G[g].push(BLANK[g]()); }); }

    // ----------------------------------------------------------------------------- picker (GrdPopUp)
    var pickRows = [], pickCb = null, pickCols = null;
    function pick(title, rows, cols, cb) {
        pickRows = rows || []; pickCb = cb; pickCols = cols;
        $('pickTitle').textContent = title; $('pickSearch').value = '';
        pickRender(); $('pickModal').classList.add('is-open'); W3B.focus('pickSearch');
    }
    function pickRender() {
        var q = $('pickSearch').value.toLowerCase(), c = 0,
            h = '<thead><tr>' + pickCols.map(function (x) { return '<th>' + W3B.esc(x.h) + '</th>'; }).join('') + '</tr></thead><tbody>';
        pickRows.forEach(function (r, i) {
            var hay = pickCols.map(function (x) { return String(r[x.k] === undefined || r[x.k] === null ? '' : r[x.k]); }).join(' ').toLowerCase();
            if (c < 400 && (!q || hay.indexOf(q) >= 0)) {
                c++;
                h += '<tr data-pi="' + i + '" class="pk-row">' + pickCols.map(function (x) { return '<td>' + W3B.esc(r[x.k]) + '</td>'; }).join('') + '</tr>';
            }
        });
        if (!c) h += '<tr class="rc-empty"><td colspan="' + pickCols.length + '">No records</td></tr>';
        $('pickGrid').innerHTML = h + '</tbody>';
    }
    function pickClose() { $('pickModal').classList.remove('is-open'); pickCb = null; }
    function pickChoose(i) { var r = pickRows[i], cb = pickCb; pickClose(); if (cb && r) cb(r); }
    $('pickSearch').addEventListener('input', pickRender);
    $('pickSearch').addEventListener('keydown', function (e) {
        if (e.key === 'Escape') pickClose();
        if (e.key === 'Enter') { var f = document.querySelector('#pickGrid tr.pk-row'); if (f) pickChoose(+f.dataset.pi); }
    });
    $('pickGrid').addEventListener('click', function (e) { var tr = e.target.closest('tr.pk-row'); if (tr) pickChoose(+tr.dataset.pi); });

    // ----------------------------------------------------------------------------- lists
    function applyLookups(d) {
        cfg.restrictSingle = !!d.restrictSingleBom; cfg.packing = !!d.packingMaterialRequired;
        cfg.decimals = d.amountDecimals > 0 ? I(d.amountDecimals) : 2;
        L.wh = d.warehouses || []; L.uoms = d.uoms || []; L.items = d.items || []; L.consItems = d.consumptionItems || []; L.accounts = d.accounts || [];
        L.bomItems = d.bomItems || []; L.types = d.productionTypes || [];
        L.allBoms = {}; (d.bomData || []).forEach(function (b) { L.allBoms[b.Id] = b; });
        W3B.fill('CmbProductionType', L.types, true, sel('CmbProductionType'));
        W3B.fill('CmbShift', d.shifts || [], false, sel('CmbShift'));                         // ShiftComboFill: no zero row, first shift selected
        W3B.fill('CmbPlant', d.plants || [], true, sel('CmbPlant'));
        W3B.fill('CmbWarehouse', L.wh, true, sel('CmbWarehouse'));
        W3B.show('btnUpdateQtySingle', cfg.restrictSingle);
        fillItems();
    }
    function fillItems() {                                                // types 4 / 5: every item; otherwise the BOM items (ItemFillFromBom:807)
        var rows = freeType() ? L.items.map(function (i) { return { Id: i.Id, Name: i.ItemName }; }) : L.bomItems;
        W3B.fill('CmbItem', rows, true, sel('CmbItem'));
    }
    function typeChanged() {
        fillItems(); W3B.clearCombo('CmbItem'); W3B.clearCombo('CmbBom'); fillUom([], 0, false); $('txtQty').value = ''; $('txtQty').disabled = !freeType();
        L.itemBoms = [];
    }
    function fillUom(rows, cur, enabled) {
        var h = rows.map(function (u) { return '<option value="' + W3B.esc(u.Id) + '"' + (String(u.Id) === String(cur) ? ' selected' : '') + '>' + W3B.esc(u.Name) + '</option>'; }).join('');
        $('CmbUom').innerHTML = h; $('CmbUom').disabled = !enabled;
    }
    function itemChanged() {
        var id = sel('CmbItem');
        W3B.clearCombo('CmbBom'); $('txtQty').value = '';
        if (id <= 0) { fillUom([], 0, false); L.itemBoms = []; return; }
        if (freeType()) {
            fillUom(uomsOf(id).map(function (u) { return { Id: u.Id, Name: uomName(u) }; }), 0, true); $('txtQty').disabled = false; return;
        }
        fillUom([], 0, false); $('txtQty').disabled = true;
        W3B.getJson(api + '/boms-by-item?itemId=' + id).then(function (rows) {            // BOMNobyItemFill:923
            L.itemBoms = rows || [];
            W3B.fill('CmbBom', L.itemBoms.map(function (b) { return { Id: b.Id, Name: b.Name }; }), true);
        }).catch(W3B.fail);
    }
    function bomChanged() {
        var b = L.itemBoms.filter(function (x) { return I(x.Id) === sel('CmbBom'); })[0];
        if (!b) { fillUom([], 0, false); $('txtQty').value = ''; return; }
        fillUom([{ Id: b.ItemUomId, Name: b.PackUom }], b.ItemUomId, false);
        $('txtQty').value = b.BomBatchQty ? String(b.BomBatchQty) : ''; $('txtQty').disabled = true;
    }

    // ----------------------------------------------------------------------------- output panel (the "+" detail panel)
    function panelRow() {
        var qty = n(v('txtQty')), uomId = sel('CmbUom');
        var wh = sel('CmbWarehouse'), item = sel('CmbItem'), bomId = sel('CmbBom');
        if (wh === 0) { alertMsg('Warehouse Field Required'); W3B.focus('CmbWarehouse'); return null; }
        if (item === 0) { alertMsg('ItemBOM Field Required'); W3B.focus('CmbItem'); return null; }
        if (!freeType() && bomId === 0) { alertMsg('BOM No Field Required'); W3B.focus('CmbBom'); return null; }
        if (uomId === 0) { alertMsg('BOM Item Uom No Field Required'); W3B.focus('CmbUom'); return null; }
        if (qty === 0) { alertMsg('Qty Field Required'); W3B.focus('txtQty'); return null; }
        var eq, code;
        if (freeType()) {
            var u = uomsOf(item).filter(function (x) { return I(x.Id) === uomId; })[0]; eq = u ? n(u.Equivalent) : 0; code = u ? uomName(u) : '';
        } else {
            var b = L.itemBoms.filter(function (x) { return I(x.Id) === bomId; })[0]; eq = b ? n(b.PackUomEquivalent) : 0; code = b ? b.PackUom : '';
        }
        return { Id: 0, BomHeaderId: freeType() ? 0 : bomId, BomNo: freeType() ? '' : W3B.selText('CmbBom'), WarehouseId: wh, WarehouseName: W3B.selText('CmbWarehouse'),
            ItemId: item, ItemName: W3B.selText('CmbItem'), ItemUomId: uomId, Uom: eq, UomCode: code, ConsumptionQty: qty, ItemQty: qty, NetWeight: qty * eq, Rate: 0,
            RateUomId: uomId, RateUom: eq, RateUomCode: code, Amount: 0, BomQty: qty };
    }
    function addRow() {
        var r = panelRow(); if (!r) return;
        if (editIdx < 0) {
            if (cfg.restrictSingle && G.out.length && G.out.some(function (x) { return I(x.BomHeaderId) !== r.BomHeaderId; })) { alertMsg('You can Only Added Single BOM in Output'); return; }
            if (G.out.some(function (x) { return I(x.BomHeaderId) === r.BomHeaderId && I(x.ItemId) === r.ItemId && I(x.WarehouseId) === r.WarehouseId; })) {
                alertMsg('Duplicate Record not added to the grid. An item with the same BOM No and WareHouse already exists'); return;
            }
            G.out.push(r);
        } else {
            var o = G.out[editIdx];
            ['BomHeaderId', 'BomNo', 'WarehouseId', 'WarehouseName', 'ItemId', 'ItemName', 'ItemUomId', 'Uom', 'UomCode', 'ConsumptionQty', 'ItemQty', 'RateUomId', 'RateUom', 'RateUomCode', 'BomQty']
                .forEach(function (k) { o[k] = r[k]; });
            o.NetWeight = o.ItemQty * o.Uom; outAmount(o);
        }
        cancelRow(); render('out'); typeLock();
    }
    function editRow(i) {
        var r = G.out[i]; if (!r) return;
        editIdx = i;
        W3B.setCombo('CmbWarehouse', r.WarehouseId); W3B.setCombo('CmbItem', r.ItemId);
        var fin = function () {
            W3B.setCombo('CmbBom', r.BomHeaderId);
            if (freeType()) fillUom(uomsOf(r.ItemId).map(function (u) { return { Id: u.Id, Name: uomName(u) }; }), r.ItemUomId, true);
            else fillUom([{ Id: r.ItemUomId, Name: r.UomCode }], r.ItemUomId, false);
            $('txtQty').value = String(r.ItemQty); $('txtQty').disabled = !freeType();
            W3B.show('btnCancelRow', true); $('btnAddRow').textContent = 'Update'; render('out');
        };
        if (freeType()) { fin(); return; }
        W3B.getJson(api + '/boms-by-item?itemId=' + r.ItemId).then(function (rows) {
            L.itemBoms = rows || []; W3B.fill('CmbBom', L.itemBoms.map(function (b) { return { Id: b.Id, Name: b.Name }; }), true); fin();
        }).catch(W3B.fail);
    }
    function cancelRow() {
        editIdx = -1; W3B.clearCombo('CmbItem'); W3B.clearCombo('CmbBom'); fillUom([], 0, false); $('txtQty').value = ''; $('txtQty').disabled = !freeType();
        W3B.show('btnCancelRow', false); $('btnAddRow').textContent = 'Add'; render('out');
    }
    function typeLock() { $('CmbProductionType').disabled = recId > 0 || G.out.length > 0; }

    // ----------------------------------------------------------------------------- grids
    function whOpts(cur) { return '<option value="0"></option>' + L.wh.map(function (w) { return '<option value="' + W3B.esc(w.Id) + '"' + (String(w.Id) === String(cur) ? ' selected' : '') + '>' + W3B.esc(w.Name) + '</option>'; }).join(''); }
    function uomOpts(itemId, cur, name) {
        var h = '<option value="0"></option>', found = false;
        uomsOf(itemId).forEach(function (u) { var s = String(u.Id) === String(cur); if (s) found = true; h += '<option value="' + W3B.esc(u.Id) + '"' + (s ? ' selected' : '') + '>' + W3B.esc(uomName(u)) + '</option>'; });
        if (!found && I(cur) > 0) h += '<option value="' + W3B.esc(cur) + '" selected>' + W3B.esc(name) + '</option>';
        return h;
    }
    function cellInput(g, i, f, val, cls) { return '<input class="cell ' + (cls || 'num') + '" data-g="' + g + '" data-i="' + i + '" data-f="' + f + '" value="' + W3B.esc(val) + '">'; }
    function cellText(g, i, f, val) { return '<span data-g="' + g + '" data-i="' + i + '" data-f="' + f + '">' + W3B.esc(val) + '</span>'; }
    function btns(g, i, add) { return '<button type="button" class="rc-x" data-act="del" data-g="' + g + '" data-i="' + i + '">X</button>' + (add ? ' <button type="button" class="rc-add" data-act="add" data-g="' + g + '" data-i="' + i + '">+</button>' : ''); }
    function whCell(g) { return { h: 'WareHouse', html: true, w: 150, f: function (r, i) { return '<select class="cell" data-g="' + g + '" data-i="' + i + '" data-f="WarehouseId">' + whOpts(r.WarehouseId) + '</select>'; } }; }
    function itemCell(g) { return { h: 'Item Name', html: true, cls: 'nowrap', w: 230, f: function (r, i) { return cellText(g, i, 'ItemName', r.ItemName) + '<button type="button" class="pk" data-pk="' + g + '" data-g="' + g + '" data-i="' + i + '">F1</button>'; } }; }
    function uomCell(g) { return { h: 'Uom', html: true, w: 90, f: function (r, i) { return '<select class="cell" data-g="' + g + '" data-i="' + i + '" data-f="ItemUomId">' + uomOpts(r.ItemId, r.ItemUomId, r.UomCode) + '</select>'; } }; }

    function render(which) {
        var all = !which;
        if (all || which === 'out') {
            var cols = [
                { h: '', html: true, w: 70, f: function (r, i) { return '<button type="button" class="rc-x" data-act="del" data-g="out" data-i="' + i + '">X</button> <button type="button" class="rc-add" data-act="clone" data-g="out" data-i="' + i + '">+</button>'; } },
                { h: 'BOM No', f: function (r) { return r.BomNo; } }, { h: 'WareHouse', f: function (r) { return r.WarehouseName; } }, { h: 'Item Name', f: function (r) { return r.ItemName; } },
                { h: 'Uom', f: function (r) { return r.UomCode; } }
            ];
            if (cfg.restrictSingle) cols.push({ h: 'Consumption Qty', html: true, w: 100, f: function (r, i) { return cellInput('out', i, 'ConsumptionQty', r.ConsumptionQty ? fmt(r.ConsumptionQty) : ''); } });
            cols.push({ h: 'Qty', html: true, w: 100, f: function (r, i) { return cellInput('out', i, 'ItemQty', r.ItemQty ? fmt(r.ItemQty) : ''); } },
                { h: 'Net Weight', html: true, cls: 'num', f: function (r, i) { return cellText('out', i, 'NetWeight', fmt(r.NetWeight)); } },
                { h: 'Rate', html: true, cls: 'num', f: function (r, i) { return cellText('out', i, 'Rate', fmt(r.Rate)); } },
                { h: 'Amount', html: true, cls: 'num', f: function (r, i) { return cellText('out', i, 'Amount', fmt(r.Amount)); } });
            $('grdOutPut').innerHTML = W3B.tableHtml(cols, G.out, { empty: 'No rows' });
            var trs = $('grdOutPut').querySelectorAll('tbody tr'); if (editIdx >= 0 && trs[editIdx]) trs[editIdx].classList.add('pm-edit');
        }
        if (all || which === 'inp') $('grdInput').innerHTML = W3B.tableHtml([
            { h: '', html: true, w: 70, f: function (r, i) { return btns('inp', i, true); } }, whCell('inp'), itemCell('inp'), uomCell('inp'),
            { h: 'Stock', html: true, cls: 'num', f: function (r, i) { return cellText('inp', i, 'BalanceStock', fmt(r.BalanceStock)); } },
            { h: 'Qty', html: true, w: 100, f: function (r, i) { return cellInput('inp', i, 'ItemQty', r.ItemQty ? fmt(r.ItemQty) : ''); } },
            { h: 'Net Weight', html: true, cls: 'num', f: function (r, i) { return cellText('inp', i, 'NetWeight', fmt(r.NetWeight)); } },
            { h: 'Avg Rate', html: true, cls: 'num', f: function (r, i) { return cellText('inp', i, 'Rate', fmt(r.Rate)); } },
            { h: 'Amount', html: true, cls: 'num', f: function (r, i) { return cellText('inp', i, 'Amount', fmt(r.Amount)); } }
        ], G.inp, { empty: 'No rows' });
        if (all || which === 'oh') $('grdOverHead').innerHTML = W3B.tableHtml([
            { h: '', html: true, w: 70, f: function (r, i) { return btns('oh', i, true); } },
            { h: 'Account', html: true, w: 280, f: function (r, i) { return '<select class="cell" data-g="oh" data-i="' + i + '" data-f="OverHeadAccountId">' + accOpts(r.OverHeadAccountId) + '</select>'; } },
            { h: 'Percentage', html: true, w: 90, f: function (r, i) { return cellInput('oh', i, 'PercentOfTotalInput', r.PercentOfTotalInput ? fmt(r.PercentOfTotalInput) : ''); } },
            { h: 'Qty', html: true, w: 90, f: function (r, i) { return cellInput('oh', i, 'Qty', r.Qty ? fmt(r.Qty) : ''); } },
            { h: 'Rate', html: true, w: 90, f: function (r, i) { return cellInput('oh', i, 'Rate', r.Rate ? fmt(r.Rate) : ''); } },
            { h: 'Amount', html: true, w: 110, f: function (r, i) { return cellInput('oh', i, 'Amount', r.Amount ? fmt(r.Amount) : ''); } },
            { h: 'ohRemarks', html: true, f: function (r, i) { return cellInput('oh', i, 'OhRemarks', r.OhRemarks, 'txt'); } }
        ], G.oh, { empty: 'No rows' });
        if (all || which === 'con') $('grdConsumption').innerHTML = W3B.tableHtml([
            { h: '', html: true, w: 70, f: function (r, i) { return btns('con', i, true); } }, whCell('con'), itemCell('con'),
            { h: 'Parent Category', html: true, f: function (r, i) { return cellText('con', i, 'ParentCategory', r.ParentCategory); } }, uomCell('con'),
            { h: 'Stock', html: true, cls: 'num', f: function (r, i) { return cellText('con', i, 'BalanceStock', fmt(r.BalanceStock)); } },
            { h: 'Qty', html: true, w: 100, f: function (r, i) { return cellInput('con', i, 'ItemQty', r.ItemQty ? fmt(r.ItemQty) : ''); } },
            { h: 'Avg Rate', html: true, cls: 'num', f: function (r, i) { return cellText('con', i, 'Rate', fmt(r.Rate)); } },
            { h: 'Amount', html: true, cls: 'num', f: function (r, i) { return cellText('con', i, 'Amount', fmt(r.Amount)); } }
        ], G.con, { empty: 'No rows' });
        calculation();
    }
    function accOpts(cur) {
        return '<option value="0"></option>' + L.accounts.map(function (a) { return '<option value="' + W3B.esc(a.Id) + '"' + (String(a.Id) === String(cur) ? ' selected' : '') + '>' + W3B.esc(a.AccountTitle) + '</option>'; }).join('');
    }

    function refreshRow(g, i) {
        var r = G[g][i]; if (!r) return;
        var els = document.querySelectorAll('[data-g="' + g + '"][data-i="' + i + '"]');
        for (var k = 0; k < els.length; k++) {
            var el = els[k], f = el.dataset.f; if (!f || !(f in r) || el === document.activeElement) continue;
            var val = typeof r[f] === 'string' ? r[f] : fmt(r[f]);
            if (el.tagName === 'SPAN') el.textContent = val;
            else if (el.tagName === 'INPUT') el.value = typeof r[f] === 'string' ? r[f] : (r[f] ? fmt(r[f]) : '');
        }
    }
    function outAmount(r) { r.Amount = n(r.RateUom) > 0 ? n(r.Rate) * (n(r.NetWeight) / n(r.RateUom)) : 0; }       // output Amount
    function rowAmount(r) { r.Amount = n(r.NetWeight) > 0 ? n(r.NetWeight) * n(r.Rate) : 0; }                       // input / consumption Amount
    function prct() {
        var tot = sum(G.inp, 'ItemQty');
        G.inp.forEach(function (r, i) { r.PrctOfTotalQty = tot > 0 ? n(r.BomQty) / tot * 100 : 0; });
    }

    /* UpdateRowStockAndRate:5799 / grdInputMaterial_KeyDown:4600 - the stock is not per warehouse (the desktop never sends it), the rate is the average rate on the document date. */
    function fetchRates(items) {
        var ids = [], seen = {};
        items.forEach(function (r) { if (I(r.ItemId) > 0 && !seen[r.ItemId]) { seen[r.ItemId] = 1; ids.push(I(r.ItemId)); } });
        if (!ids.length) return Promise.resolve({});
        return W3B.postJson(api + '/item-rates', { docDate: v('DocDate'), recId: saveAsMode ? 0 : recId, itemIds: ids }).then(function (rows) {
            var m = {}; (rows || []).forEach(function (x) { m[x.itemId] = x; }); return m;
        });
    }
    function applyRate(r, m) {
        var x = m[r.ItemId]; if (!x) return;
        r.BalanceStock = n(x.stock) > 0 ? n(x.stock) : 0;
        r.Rate = n(x.rate) > 0 ? n(x.rate) : 0;
        rowAmount(r);
    }
    function rowChanged(g, i) {                                           // grdInputMaterial_CellUpdated / grdConsumption_CellUpdated
        var r = G[g][i]; if (!r) return;
        if (n(r.ItemQty) === 0) { r.NetWeight = 0; r.Rate = 0; r.Amount = 0; r.PrctOfTotalQty = 0; refreshRow(g, i); calculation(); return; }
        r.NetWeight = n(r.ItemQty) * n(r.Uom);
        fetchRates([r]).then(function (m) {
            applyRate(r, m); if (g === 'inp') prct(); render(g);
        }).catch(W3B.fail);
    }
    function selectedUom(r, uomId) {
        var u = uomsOf(r.ItemId).filter(function (x) { return I(x.Id) === uomId; })[0];
        r.ItemUomId = uomId; r.Uom = u ? n(u.Equivalent) : 0; r.UomCode = u ? uomName(u) : '';
        r.RateUomId = uomId; r.RateUom = r.Uom; r.RateUomCode = r.UomCode;
    }
    function cellChange(e) {
        var t = e.target; if (!t.dataset || !t.dataset.g || (t.tagName !== 'INPUT' && t.tagName !== 'SELECT')) return;
        var g = t.dataset.g, i = +t.dataset.i, f = t.dataset.f, r = G[g][i]; if (!r) return;
        if (t.tagName === 'SELECT') {
            var id = I(t.value);
            if (f === 'WarehouseId') {
                r.WarehouseId = id; r.WarehouseName = id ? t.options[t.selectedIndex].text : '';
                if (g !== 'out' && I(r.ItemId) > 0) fetchRates([r]).then(function (m) { applyRate(r, m); render(g); }).catch(W3B.fail);
                return;
            }
            if (f === 'ItemUomId') {
                selectedUom(r, id);
                if (n(r.ItemQty) > 0) r.NetWeight = n(r.ItemQty) * n(r.Uom);
                fetchRates([r]).then(function (m) { applyRate(r, m); if (g === 'inp') prct(); render(g); }).catch(W3B.fail);
                return;
            }
            if (f === 'OverHeadAccountId') {
                r.OverHeadAccountId = id; r.OverHeadTypeId = id; r.OverHeadAccount = id ? t.options[t.selectedIndex].text : '';
                return;
            }
            return;
        }
        if (g === 'oh') { ohChanged(i, f, t.value); return; }
        if (f === 'OhRemarks') { r[f] = t.value; return; }
        r[f] = t.value === '' ? 0 : n(t.value);
        t.value = r[f] ? fmt(r[f]) : '';
        if (g === 'out') {
            if (f === 'ItemQty') { r.NetWeight = n(r.ItemQty) * n(r.Uom); if (!cfg.restrictSingle) r.ConsumptionQty = r.ItemQty; }
            outAmount(r); refreshRow('out', i); calculation(); return;
        }
        rowChanged(g, i);
    }
    function ohChanged(i, f, raw) {                                       // grdOverHead cell updates
        var r = G.oh[i];
        if (f === 'OhRemarks') { r.OhRemarks = raw; return; }
        r[f] = raw === '' ? 0 : n(raw);
        if (f === 'Qty' || f === 'Rate') r.Amount = n(r.Qty) * n(r.Rate);
        else if (f === 'PercentOfTotalInput') {
            var tot = sum(G.oh, 'PercentOfTotalInput');
            if (tot > 100) { r.PercentOfTotalInput = n(r.PercentOfTotalInput) - (tot - 100); alertMsg('TotalPercentage Can not be Greater than 100...'); }
            var inputAmt = sum(G.inp, 'Amount');
            r.Amount = roundAway(inputAmt / 100 * n(r.PercentOfTotalInput), cfg.decimals); r.Qty = 0; r.Rate = 0;
        } else if (f === 'Amount') { r.Qty = 0; r.Rate = 0; }
        render('oh');
    }
    ['grdInput', 'grdOverHead', 'grdConsumption', 'grdOutPut'].forEach(function (id) {
        $(id).addEventListener('change', cellChange);
        $(id).addEventListener('click', gridClick);
    });
    $('grdOutPut').addEventListener('dblclick', function (e) { var tr = e.target.closest('tbody tr'); if (!tr || e.target.closest('button,input,select')) return; editRow(tr.rowIndex - 1); });
    function gridClick(e) {
        var b = e.target.closest('button'); if (!b || !b.dataset.g) return;
        var g = b.dataset.g, i = +b.dataset.i;
        if (b.dataset.act === 'del') deleteRow(g, i);
        else if (b.dataset.act === 'add') { G[g].splice(i + 1, 0, BLANK[g]()); render(g); }
        else if (b.dataset.act === 'clone') { var c = JSON.parse(JSON.stringify(G.out[i])); c.Id = 0; G.out.splice(i + 1, 0, c); render('out'); typeLock(); }
        else if (b.dataset.pk) openPick(g, i);
    }
    function deleteRow(g, i) {                                            // the X buttons of each grid
        if (g === 'out') {
            if (editIdx >= 0) { alertMsg('Please Reset the Detail First...'); return; }
            var o = G.out.splice(i, 1)[0]; if (o && I(o.Id) > 0) removed.out.push(o);
            render('out'); typeLock(); return;
        }
        var r = G[g].splice(i, 1)[0];
        if (r && I(r.Id) > 0 && g !== 'oh') removed[g].push(r);
        ensureBlanks(); render(g);
    }
    function openPick(g, i) {
        var r = G[g][i]; if (!r) return;
        if (g === 'con') {
            pick('Item', L.consItems, [{ h: 'Item', k: 'ItemName' }, { h: 'Category', k: 'ParentCategory' }], function (it) {
                var changed = I(r.ItemId) !== I(it.Id);
                r.ItemId = I(it.Id); r.ItemName = it.ItemName; r.ParentCategory = it.ParentCategory || '';
                itemPicked(g, r, changed);
            });
        } else {
            pick('Item', L.items, [{ h: 'Code', k: 'ItemCode' }, { h: 'Item', k: 'ItemName' }], function (it) {
                var changed = I(r.ItemId) !== I(it.Id);
                r.ItemId = I(it.Id); r.ItemName = it.ItemName;
                itemPicked(g, r, changed);
            });
        }
    }
    function itemPicked(g, r, changed) {
        if (changed) { r.ItemUomId = 0; r.Uom = 0; r.UomCode = ''; r.RateUomId = 0; r.RateUom = 0; r.RateUomCode = ''; r.NetWeight = 0; }
        if (I(r.ItemUomId) === 0) { var u = uomsOf(r.ItemId)[0]; if (u) selectedUom(r, I(u.Id)); }
        if (n(r.ItemQty) > 0) r.NetWeight = n(r.ItemQty) * n(r.Uom);
        fetchRates([r]).then(function (m) { applyRate(r, m); if (g === 'inp') prct(); render(g); }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- Calculation
    function calculation() {
        var ti = G.inp.length ? sum(G.inp, 'Amount') : 0, to = G.oh.length ? sum(G.oh, 'Amount') : 0, tc = G.con.length ? sum(G.con, 'Amount') : 0, net = ti + to + tc;
        $('txtTotalInputs').value = fmt(roundAway(ti, 2), 2); $('txtTotalOverHead').value = fmt(roundAway(to, 2), 2); $('txtTotalConsumption').value = fmt(roundAway(tc, 2), 2);
        $('txtNetValue').value = String(net);
        $('txtTotalOutPutAutoRatedAmount').value = fmt(sum(G.out, 'Amount'), cfg.decimals);
        var q = sum(G.out, 'ItemQty');
        if (q > 0) $('txtValuePerQty').value = fmt(roundAway(net / q, 2), 2);
    }

    // ----------------------------------------------------------------------------- toolbar helpers of the grids
    function clearInput() {
        if (!W3B.ask('are you sure to reset Input Detail?')) return;
        G.inp.forEach(function (r) { if (I(r.Id) > 0) removed.inp.push(r); });
        G.inp = [BLANK.inp()]; render('inp');
    }
    function sameWarehouse(g) {
        var cap = g === 'inp' ? 'Input' : g === 'con' ? 'Consumption' : 'OutPut';
        if (!G[g].length) { alertMsg(cap + ' Grid Record not Found'); return; }
        var w = I(G[g][0].WarehouseId);
        if (w === 0) { alertMsg('Warehouse Field is Empty in ' + cap + ' Grid Row No: 01'); return; }
        var nm = G[g][0].WarehouseName;
        G[g].forEach(function (r) { r.WarehouseId = w; r.WarehouseName = nm; });
        render(g);
    }
    function loadFromBom() {                                              // FillInputDetailForMultiBom:1185
        if (!G.out.length) { alertMsg('OutPut Grid Record Not Found...'); return; }
        if (G.inp.some(function (r) { return I(r.ItemId) > 0; }) && !W3B.ask('Input Grid already have Records.Do you want To Refresh Data?')) return;
        W3B.postJson(api + '/bom-inputs', { bomIds: G.out.map(function (r) { return I(r.BomHeaderId); }) }).then(function (d) {
            G.inp.forEach(function (r) { if (I(r.Id) > 0) removed.inp.push(r); });
            var rows = d.rows || [], seen = {};
            G.inp = rows.filter(function (r) {
                var k = d.single ? 'b' + r.BomInputId : r.ItemId + '-' + r.ItemUomId + '-' + r.RateUomId;
                if (seen[k]) return false; seen[k] = 1; return true;
            });
            if (!G.inp.length) G.inp = [BLANK.inp()];
            prct(); render('inp');
        }).catch(W3B.fail);
    }
    function updateQtySingle() {                                          // Update Qty if Single BOM
        if (!G.inp.length) { alertMsg('InPut have No Row'); return; }
        var outs = G.out.filter(function (r) { return I(r.BomHeaderId) > 0; });
        if (!outs.length) { alertMsg('OutPut have No Row With BOM'); return; }
        var outItemQty = sum(outs, 'ConsumptionQty'), outBomQty = sum(outs, 'BomQty');
        fetchRates(G.inp).then(function (m) {
            var tot = sum(G.inp, 'ItemQty');
            G.inp.forEach(function (r) {
                if (n(r.BomQty) > 0) {
                    var q = (outBomQty > 0 && outItemQty > 0) ? outItemQty / outBomQty * n(r.BomQty) : 0;
                    r.ItemQty = roundAway(q, 2); r.NetWeight = roundAway(q * n(r.Uom), 2);
                    applyRate(r, m); r.PrctOfTotalQty = tot > 0 ? n(r.BomQty) / tot * 100 : 0; rowAmount(r);
                }
            });
            render('inp');
        }).catch(W3B.fail);
    }
    function generate() {                                                 // btnGenerateRatesOutput: inputs, consumption, over heads, then the output rates
        var rows = G.inp.concat(G.con);
        fetchRates(rows).then(function (m) {
            rows.forEach(function (r) { if (I(r.ItemId) > 0) { applyRate(r, m); } });
            // UpdateOverHeadByOutput
            G.oh.forEach(function (r) {
                if (I(r.BomOverHeadId) <= 0) return;
                var bom = L.allBoms ? L.allBoms[r.BomHeaderId] : null, finish = bom ? n(bom.BomBatchQty) : n(r.BomFinishQty);
                var prod = sum(G.out.filter(function (o) { return I(o.BomHeaderId) === I(r.BomHeaderId); }), 'ItemQty');
                if (n(r.BomQty) > 0) { r.Qty = finish > 0 ? n(r.BomQty) / finish * prod : 0; r.Amount = n(r.Qty) * n(r.Rate); }
                else if (n(r.BomAmount) > 0) { r.Rate = 0; r.Qty = 0; r.Amount = finish > 0 ? n(r.BomAmount) / finish * prod : 0; }
                else { r.Qty = 0; r.Rate = 0; r.Amount = 0; }
            });
            // UpdateOutputRates
            if (!G.out.length) throw new Error('Output Grid Record not Found');
            if (G.out.length === 1 && I(G.out[0].BomHeaderId) > 0 && I(G.out[0].ItemId) === 0) throw new Error('Output Grid Record not Filled Completely');
            var net = sum(G.inp, 'Amount') + sum(G.oh, 'Amount') + sum(G.con, 'Amount'), qty = sum(G.out, 'ItemQty');
            G.out.forEach(function (o) {
                o.Amount = qty > 0 ? net / qty * n(o.ItemQty) : 0;
                if (n(o.ItemQty) > 0) o.Rate = o.Amount / n(o.ItemQty);
            });
            render();
        }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- rights / mode
    function applyRights() {
        $('btnSave').disabled = !rights.save; $('btnUpdate').disabled = !rights.update; $('btnDelete').disabled = !rights['delete'];
        $('btnPrint').disabled = !rights.print; $('ChkPreviewSlip').disabled = !rights.print; $('chkPrintVoucher').disabled = !rights.print;
        $('ChkPreviewSlip').checked = !!rights.print; $('chkPrintVoucher').checked = !!rights.print;
        var m = [];
        if (!rights.save) m.push('Save'); if (!rights.update) m.push('Update'); if (!rights['delete']) m.push('Delete'); if (!rights.print) m.push('Print');
        var nt = $('rightsNote');
        nt.textContent = m.length ? 'Your account does not have the ' + m.join(', ') + ' right on this screen, so those buttons are off.' : '';
        nt.classList.toggle('is-hidden', !m.length);
    }
    function mode() {
        var edit = recId > 0;
        W3B.show('btnSave', !edit && !saveAsMode); W3B.show('btnSaveAs', saveAsMode); W3B.show('btnUpdate', edit); W3B.show('btnDelete', edit);
        $('CmbProductionType').disabled = edit || G.out.length > 0;
    }
    function reset() {                                                    // FromReset
        recId = 0; saveAsMode = false; editIdx = -1; removed = { inp: [], out: [], con: [] };
        G.inp = [BLANK.inp()]; G.con = [BLANK.con()]; G.oh = [BLANK.oh()]; G.out = [];
        $('txtremarks').value = ''; $('txtProductionNo').value = '';
        W3B.clearCombo('CmbProductionType'); W3B.clearCombo('CmbPlant'); W3B.clearCombo('CmbItem'); W3B.clearCombo('CmbBom'); W3B.clearCombo('CmbWarehouse');
        fillItems(); fillUom([], 0, false); $('txtQty').value = ''; $('txtQty').disabled = true;
        W3B.show('btnCancelRow', false); $('btnAddRow').textContent = 'Add';
        mode(); render();
        return W3B.getJson(api + '/doc-no').then(function (d) { $('txtdocno').value = d.docNo; }).catch(W3B.fail);
    }
    function newClick() { reset(); }
    function refresh() { W3B.getJson(api + '/refresh').then(function (d) { applyLookups(d); render(); }).catch(W3B.fail); }
    function dateChanged() { /* the desktop re-reads rates on the next row change */ }

    // ----------------------------------------------------------------------------- save (Insert:2809)
    function formValidation() {                                           // FormValidation:1321
        var d = v('txtdocno').trim();
        if (d === '' || d === '0') { alertMsg('Doc No Field Required'); return false; }
        if (sel('CmbPlant') === 0 || typeId() === 0) { alertMsg('Production Type Field Required'); W3B.focus('CmbProductionType'); return false; }
        if (sel('CmbShift') === 0) { alertMsg('Shift Field Required'); W3B.focus('CmbShift'); return false; }
        if (v('txtProductionNo').trim() === '') { alertMsg('Production No Field Required'); W3B.focus('txtProductionNo'); return false; }
        if (sel('CmbPlant') === 0) { alertMsg('Plant Name Field Required'); W3B.focus('CmbPlant'); return false; }
        return true;
    }
    var saving = false;
    function save() {
        if (saving) return;
        if (!formValidation()) return;
        if (!G.inp.length || !G.out.length) { alertMsg('Grid Record Not Found...\nGrid (Input and OutPut) Should have atleast one record'); return; }
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = {
            id: recId, docDate: v('DocDate'), docNo: v('txtdocno'), productionTypeId: typeId(), shiftId: sel('CmbShift'), plantId: sel('CmbPlant'), productionNo: v('txtProductionNo'),
            remarks: v('txtremarks'), totalOutputAutoRated: v('txtNetValue'),
            output: G.out, input: G.inp, overhead: G.oh, consumption: G.con, removedInput: removed.inp, removedOutput: removed.out, removedConsumption: removed.con
        };
        saving = true;
        W3B.postJson(api + '/save', body).then(function (d) {
            saving = false; alertMsg(d.message);
            var id = d.id, slip = $('ChkPreviewSlip').checked && rights.print, vch = $('chkPrintVoucher').checked && rights.print;
            return reset().then(function () { if (slip) printSlip(id); if (vch) printVoucherFor(id); });
        }).catch(function (e) { saving = false; W3B.fail(e); });
    }
    function del() {                                                      // btnDelete_Click:3452
        if (recId <= 0) { alertMsg('No record found to Delete'); return; }
        if (!W3B.ask('Are you sure to Delete?')) return;
        W3B.postForm(api + '/delete?id=' + recId).then(function (d) { alertMsg(d.message); return reset(); }).catch(W3B.fail);
    }
    function printSlip(id) {                                              // 1122 production slip (existing ConcretePrintController endpoint)
        if (!id) { alertMsg('No Data Found'); return; }
        window.open('/reports/print/1122-production-slip?branchesId=' + encodeURIComponent(look.branchId || '') + '&documentTypeId=1122&id=' + encodeURIComponent(id), '_blank');
    }
    function print() { printSlip(recId); }
    function printVoucherFor(id) {                                        // VoucherReprot:3558 -> AcRptPurchaseSalesVoucherSlip_103
        if (!id) return;
        var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        W3B.getJson(api + '/voucher-head?id=' + id).then(function (d) {
            var vh = I(d.voucherHeadId);
            if (!vh) { if (win) window.CrystalPrint.release(win); alertMsg('VoucherId Not Found'); return; }
            return window.CrystalPrint.open('acc-103', { id: vh, documentTypeId: 1122 }, null, win);
        }).catch(function (e) { if (win) window.CrystalPrint.release(win); W3B.fail(e); });
    }
    function printVoucher() { printVoucherFor(recId); }

    // ----------------------------------------------------------------------------- ReadById:3334
    function load(id, asNew) {
        return reset().then(function () { return W3B.getJson(api + '/read/' + id); }).then(function (d) {
            recId = asNew ? 0 : d.id; saveAsMode = !!asNew;
            tab('form');
            $('txtdocno').value = asNew ? $('txtdocno').value : d.docNo; $('DocDate').value = d.docDate; $('txtProductionNo').value = d.productionNo; $('txtremarks').value = d.remarks;
            W3B.setCombo('CmbProductionType', d.productionTypeId); W3B.setCombo('CmbShift', d.shiftId); W3B.setCombo('CmbPlant', d.plantId);
            fillItems();
            var zero = function (rows) { if (asNew) rows.forEach(function (r) { r.Id = 0; }); return rows; };
            G.out = zero(d.output || []); G.inp = zero(d.input || []); G.oh = zero(d.overhead || []); G.con = zero(d.consumption || []);
            ensureBlanks(); mode(); render();
        }).catch(function (e) { recId = 0; saveAsMode = false; mode(); W3B.fail(e); });
    }

    // ----------------------------------------------------------------------------- history
    function tab(t) {
        $('panelForm').classList.toggle('is-active', t === 'form'); $('panelHist').classList.toggle('is-active', t === 'hist');
        $('tabForm').classList.toggle('is-active', t === 'form'); $('tabHist').classList.toggle('is-active', t === 'hist');
        if (t === 'hist' && !$('CmbHistNo').options.length) refreshHistLookups();
    }
    function refreshHistLookups() {                                       // AllCombobind:3570
        W3B.getJson(api + '/history-lookups').then(function (d) {
            W3B.fill('CmbHistNo', d.productionNo || [], true, sel('CmbHistNo'));
            W3B.fill('CmbHistType', d.productionType || [], true, sel('CmbHistType'));
            W3B.fill('CmbHistShift', d.shift || [], true, sel('CmbHistShift'));
        }).catch(W3B.fail);
    }
    function dateType() {
        var t = I(v('cmbperemeter')), now = new Date(), today = W3B.iso(now);
        if (t === 1) $('fromdate').value = today;
        else if (t === 2) { var d = new Date(now); d.setDate(now.getDate() - 7); $('fromdate').value = W3B.iso(d); }
        else if (t === 3) { $('fromdate').value = W3B.iso(new Date(now.getFullYear(), now.getMonth(), 1)); $('ToDate').value = today; }
        else if (t === 4) { $('fromdate').value = now.getFullYear() + '-01-01'; $('ToDate').value = today; }
        else if (t === 5) { if (look.financialYearStart) $('fromdate').value = look.financialYearStart; $('ToDate').value = today; }
    }
    function btnCell(act, i, cap) { return '<button type="button" class="rc-cellbtn" data-hact="' + act + '" data-hi="' + i + '">' + cap + '</button>'; }
    function history() {                                                  // HistoryGridFill:3781
        var q = { fromDocNo: I(v('txtDocNoFrom')), toDocNo: I(v('txtDocNoTo')), productionHeaderId: sel('CmbHistNo'), productTypeId: sel('CmbHistType') };
        if ($('chkFrom').checked) q.fromDate = v('fromdate');
        if ($('chkTo').checked) q.toDate = v('ToDate');
        W3B.getJson(api + '/history' + W3B.qs(q)).then(function (rows) {
            hist = rows; clearHistDetail();
            if (!rows.length) { $('grdhistory').innerHTML = ''; return; }
            $('grdhistory').innerHTML = W3B.tableHtml([
                { h: 'Print', html: true, w: 40, f: function (r, i) { return btnCell('print', i, 'Print'); } },
                { h: 'Voucher', html: true, w: 50, f: function (r, i) { return btnCell('voucher', i, 'Voucher'); } },
                { h: 'Edit', html: true, w: 40, f: function (r, i) { return btnCell('edit', i, 'Edit'); } },
                { h: 'SaveAs', html: true, w: 50, f: function (r, i) { return btnCell('saveas', i, 'SaveAs'); } },
                { h: 'DocNo', k: 'DocNo' }, { h: 'DocDate', f: function (r) { return W3B.fmtDMY(r.DocDate); } }, { h: 'Production Type', k: 'ProductionType' }, { h: 'Shift', k: 'ShiftName' },
                { h: 'Production No', k: 'ProductionNo' }, { h: 'Plant', k: 'PlantName' }, { h: 'RemarksHeader', k: 'RemarksHeader' },
                { h: 'EntryDate', f: function (r) { return W3B.fmtStamp(r.EntryDate); } }, { h: 'EntryUser', k: 'EntryUserName' },
                { h: 'ModifyDate', f: function (r) { return W3B.fmtStamp(r.ModifyDate); } }, { h: 'ModifyUser', k: 'ModifyUserName' },
                { h: 'IsApproved', f: function (r) { return r.IsApproved ? 'Yes' : ''; } }, { h: 'ApprovedDate', f: function (r) { return W3B.fmtStamp(r.ApprovedDate); } },
                { h: 'NoOfAttachments', k: 'NoOfAttachments', cls: 'num' }
            ], rows, { empty: 'No records' });
        }).catch(W3B.fail);
    }
    $('grdhistory').addEventListener('click', function (e) {
        var b = e.target.closest('button[data-hact]'), tr = e.target.closest('tbody tr');
        if (b) {
            var row = hist[+b.dataset.hi], act = b.dataset.hact; if (!row) return;
            if (act === 'print') printSlip(row.Id);
            else if (act === 'voucher') printVoucherFor(row.Id);
            else if (act === 'edit') { if (!rights.update) { alertMsg("ypu don't have update rights..."); return; } load(row.Id, false); }
            else if (act === 'saveas') { if (!rights.save) { alertMsg("you don't have save rights..."); return; } load(row.Id, true); }
            return;
        }
        if (tr && hist[tr.rowIndex - 1]) historyDetail(hist[tr.rowIndex - 1].Id);
    });
    $('grdhistory').addEventListener('dblclick', function (e) {
        var tr = e.target.closest('tbody tr'); if (!tr || e.target.closest('button') || !hist[tr.rowIndex - 1]) return;
        if (!rights.update) { alertMsg("you don't have updae rights..."); return; }
        load(hist[tr.rowIndex - 1].Id, false);
    });
    function clearHistDetail() { ['hInput', 'hOutput', 'hOverHead', 'hConsumption'].forEach(function (id) { $(id).innerHTML = ''; }); }
    function historyDetail(id) {                                          // DatagridHistoryDetailbind:4084
        W3B.getJson(api + '/read/' + id).then(function (d) {
            var num = function (h, k) { return { h: h, k: k, cls: 'num' }; };
            $('hInput').innerHTML = W3B.tableHtml([{ h: 'Warehouse', k: 'WarehouseName' }, { h: 'Item', k: 'ItemName' }, { h: 'Uom', k: 'UomCode' }, num('ItemQty', 'ItemQty'),
                num('%', 'PrctOfTotalQty'), num('NetWeight', 'NetWeight'), num('Rate', 'Rate'), num('Amount', 'Amount')], d.input || [], { empty: 'No rows' });
            var oc = [{ h: 'BomNo', k: 'BomNo' }, { h: 'Warehouse', k: 'WarehouseName' }, { h: 'Item', k: 'ItemName' }];
            if (cfg.restrictSingle) oc.push(num('ConsumptionQty', 'ConsumptionQty'));
            oc.push(num('ItemQty', 'ItemQty'), num('NetWeight', 'NetWeight'), num('Rate', 'Rate'), num('Amount', 'Amount'));
            $('hOutput').innerHTML = W3B.tableHtml(oc, d.output || [], { empty: 'No rows' });
            $('hOverHead').innerHTML = W3B.tableHtml([{ h: 'AccountTitle', k: 'OverHeadAccount' }, num('Percentage', 'PercentOfTotalInput'), num('Qty', 'Qty'), num('Rate', 'Rate'),
                num('Amount', 'Amount'), { h: 'ohRemarks', k: 'OhRemarks' }], d.overhead || [], { empty: 'No rows' });
            $('hConsumption').innerHTML = W3B.tableHtml([{ h: 'Warehouse', k: 'WarehouseName' }, { h: 'Item', k: 'ItemName' }, { h: 'Uom', k: 'UomCode' }, num('ItemQty', 'ItemQty'),
                num('Rate', 'Rate'), num('Amount', 'Amount')], d.consumption || [], { empty: 'No rows' });
        }).catch(W3B.fail);
    }
    function newHistory() {
        var now = new Date(), f = new Date(now); f.setDate(now.getDate() - 3);
        $('fromdate').value = W3B.iso(f); $('ToDate').value = W3B.iso(now); $('txtDocNoFrom').value = ''; $('txtDocNoTo').value = '';
        W3B.clearCombo('CmbHistNo'); W3B.clearCombo('CmbHistType'); W3B.clearCombo('CmbHistShift');
        $('grdhistory').innerHTML = ''; clearHistDetail(); hist = [];
    }

    window.Pm = { newClick: newClick, refresh: refresh, save: save, del: del, print: print, voucher: printVoucher, tab: tab, history: history, newHistory: newHistory,
        refreshHistLookups: refreshHistLookups, dateType: dateType, dateChanged: dateChanged, typeChanged: typeChanged, itemChanged: itemChanged, bomChanged: bomChanged,
        addRow: addRow, cancelRow: cancelRow, loadFromBom: loadFromBom, clearInput: clearInput, sameWarehouse: sameWarehouse, updateQtySingle: updateQtySingle, generate: generate,
        pickClose: pickClose };

    W3B.keys({ n: newClick, s: save, u: function () { if (recId > 0) save(); }, r: refresh, p: print });

    // ----------------------------------------------------------------------------- boot (frmBillOfMaterial_Manufacturing_Load:463)
    W3B.getJson(api + '/lookups').then(function (d) {
        look = d; rights = d.rights || {};
        applyRights(); applyLookups(d);
        W3B.fill('cmbperemeter', d.dateTypes || [], false);
        $('DocDate').value = W3B.today();
        var now = new Date(), f = new Date(now); f.setDate(now.getDate() - 3);
        $('fromdate').value = W3B.iso(f); $('ToDate').value = W3B.iso(now);
        $('txtdocno').value = d.docNo;
        G.inp = [BLANK.inp()]; G.con = [BLANK.con()]; G.oh = [BLANK.oh()]; G.out = [];
        $('txtQty').disabled = true; fillUom([], 0, false);
        mode(); render();
    }).catch(W3B.fail);
}());
