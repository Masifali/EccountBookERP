/* ============================================================================================
 * Screens 807 / 846 "Production Move (Stage Shift)" - Architecture.WinApp.Mfg.frmStockTransferManual_Engr (DocumentTypeId 1663).
 * Desktop line numbers (.cs): BindTransferType:562, BindItemCategory:607, BindBatch:659, ItemdtFill:695, ItemDetailBind:721, ItemModalBind:802, ItemUomBind:901,
 * WareHouseFill:988, BindCastingType:1100, CmbItem_Leave:1306, CmbSampleModal_Leave:1326, CmbCategory_Leave:1343, GetTransferTypeData:1356,
 * CmbTransferType_Leave:1398, BindCombos:1495, FormValidationDetail:1595, Reset:1687, FromResetDetail:1763, grdSettings:1969, btnplus_Click:2054,
 * btnUpdateDetail_Click:2073, grd_DoubleClick:2113, grd_CellUpdated:2165, Insert:2199, ReadById:2400, CalculateAmount:2486, GetAvailableQty*:2533-2630,
 * cmbperemeter_ValueChanged:3026, gridhistoryfill:3105, grdhistory_SelectionChanged:3282, BtnLoadStock_Click:3456, GetDetailByTransferTypeData:3476;
 * frmLoadAvailableStockEngr (loader dialog). API /production/engr-stage-move/api. Not ported: Attachment dialog (DMS), SaveAs, shortcut-keys dialog, grid layout files.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/engr-stage-move/api';
    var $ = W3B.$, n = W3B.num, I = W3B.int;

    var look = {}, rights = {};
    var recId = 0, updateIdx = -1, gridTypeId = 0, tag = 0, isApproved = false;
    var table = [], removed = [], stock = [], models = [], dtItem = [], hist = [];
    var L = { wh: [], stage: [], ws: [], wo: [], types: [] };
    var packEq = {}, rateEq = {};
    var seen = {};                                   // UltraCombo.Leave emulation: the last value a Leave handler ran for
    var ldRows = [], ldChecked = {};
    var attrCache = {};

    // ----------------------------------------------------------------------------- small helpers
    function r2(v) {                                   // Math.Round(x, 2, MidpointRounding.AwayFromZero)
        var s = v < 0 ? -1 : 1, x = Math.abs(v) * 100;
        return s * Math.round(x + 1e-9) / 100;
    }
    function fmt(v, places) { return n(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: places }); }   // "#,##0.##" / "#,##0.###"
    function alertMsg(m) { W3B.box(m); }
    function nameOf(list, id) { for (var i = 0; i < list.length; i++) if (String(list[i].Id) === String(id)) return list[i].Name; return ''; }
    function v(id) { return $(id).value; }
    function saveVisible() { return recId === 0; }                       // btnsave.Visible (the record is new)
    function canSave() { return !!rights.save && saveVisible(); }       // btnsave.Enabled && btnsave.Visible
    function sel(id) { return I(v(id)); }
    function clear(id) { W3B.clearCombo(id); }

    // stock rows arrive with the column labels of the procedure; read them case-insensitively once
    var KEYS = ['ItemCategoryId', 'ItemCategory', 'itemModalId', 'ItemId', 'ItemName', 'ItemCode', 'DyeWeight', 'SandWeight', 'ItemUomId', 'Uom', 'PackEquivalent', 'JobLotId', 'BatchNo',
        'WarehouseId', 'WareHouseName', 'ProductionStageId', 'FromProductionStage', 'CastingTypeId', 'CastingType', 'AvailableQty', 'Amount', 'AvgRate', 'RateUomId', 'RateUom', 'RateEquivalent'];
    function normalizeStock(rows) {
        return (rows || []).map(function (r) {
            var lc = {}; Object.keys(r).forEach(function (k) { lc[k.toLowerCase()] = r[k]; });
            var o = {}; KEYS.forEach(function (k) { var x = lc[k.toLowerCase()]; o[k] = x === undefined ? null : x; });
            return o;
        });
    }

    // ----------------------------------------------------------------------------- picker (GrdPopUp) - used for the Variant cell (F1)
    var pickRows = [], pickCb = null;
    function pick(title, rows, cb) {
        pickRows = rows || []; pickCb = cb;
        $('pickTitle').textContent = title; $('pickSearch').value = '';
        pickRender(); $('pickModal').classList.add('is-open'); W3B.focus('pickSearch');
    }
    function pickRender() {
        var q = $('pickSearch').value.toLowerCase(), h = '<thead><tr><th style="width:80px">Id</th><th>Name</th></tr></thead><tbody>', c = 0;
        pickRows.forEach(function (r, i) {
            if (c < 400 && (!q || String(r.Name).toLowerCase().indexOf(q) >= 0)) { c++; h += '<tr data-pi="' + i + '" class="pk-row"><td class="num">' + W3B.esc(r.Id) + '</td><td>' + W3B.esc(r.Name) + '</td></tr>'; }
        });
        if (!c) h += '<tr class="rc-empty"><td colspan="2">No records</td></tr>';
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
        L.types = d.transferTypes || []; L.wh = d.warehouses || []; L.stage = d.productionStages || []; L.ws = d.workStations || []; L.wo = d.workOrders || [];
        W3B.fill('CmbTransferType', L.types, true);                                 // BindTransferType:581
        ['CmbWarehouseFrom', 'CmbWarehouseTo', 'CmbWareHouseFromDetail', 'CmbWarehouseToDetail'].forEach(function (id) { W3B.fill(id, L.wh, true); });    // WareHouseFill:1005
        ['CmbProductionStageFrom', 'CmbProductionStageTo', 'CmbProductionStageFromDetail', 'CmbProductionStageToDetail'].forEach(function (id) { W3B.fill(id, L.stage, true); });
        ['CmbWorkStationFromDetail', 'CmbWorkStationToDetail'].forEach(function (id) { W3B.fill(id, L.ws, true); });
        W3B.fill('CmbWorkOrderNo', L.wo, true);
    }

    // ----------------------------------------------------------------------------- Bind* (entry panel combos)
    function distinct(rows, idKey, nameKey) {
        var seenIds = {}, out = [];
        rows.forEach(function (r) { var id = r[idKey]; if (!seenIds[id]) { seenIds[id] = 1; out.push({ Id: id, Name: r[nameKey] === null ? '' : String(r[nameKey]) }); } });
        return out;
    }
    function keepOrFirst(selId, rows, prev, zeroFirst) {                  // SetComboValue / Rows[n].Activate
        if (prev > 0 && rows.some(function (r) { return String(r.Id) === String(prev); })) { W3B.setCombo(selId, prev); return; }
        if (zeroFirst && rows.length) W3B.setCombo(selId, rows[0].Id);
    }
    function bindItemCategory() {                                         // :607 (BindDDLNew, no zero row)
        var prev = sel('CmbCategory'), rows = distinct(stock, 'ItemCategoryId', 'ItemCategory');
        if (rows.length) { W3B.fill('CmbCategory', rows, false, ''); if (prev > 0) keepOrFirst('CmbCategory', rows, prev, false); } else { W3B.fill('CmbCategory', [], false, ''); }
    }
    function bindBatch() {                                                // :659
        var prev = sel('txtBatchNo'), rows = distinct(stock, 'JobLotId', 'BatchNo');
        W3B.fill('txtBatchNo', rows, false, '');
        if (rows.length && prev > 0) keepOrFirst('txtBatchNo', rows, prev, false);
    }
    function itemdtFill() {                                               // :695
        dtItem = []; var s = {};
        stock.forEach(function (r) {
            if (!s[r.ItemId]) { s[r.ItemId] = 1; dtItem.push({ Id: r.ItemId, ItemName: r.ItemName || '', ItemCode: r.ItemCode || '', ItemCategoryId: r.ItemCategoryId, itemModalId: r.itemModalId, SandWeight: r.SandWeight, DyeWeight: r.DyeWeight }); }
        });
    }
    function itemDetailBind(categoryId, modalId) {                        // :721
        var prev = sel('CmbItem'), f;
        if (modalId > 0) f = dtItem.filter(function (x) { return +x.itemModalId === modalId; });
        else if (categoryId > 0) f = dtItem.filter(function (x) { return +x.ItemCategoryId === categoryId; });
        else f = dtItem;
        if (f.length) {
            var byName = $('rdbtnItemName').checked;
            W3B.fill('CmbItem', f.map(function (x) { return { Id: x.Id, Name: byName ? x.ItemName : x.ItemCode }; }), true, '');
            if (prev > 0 && f.some(function (x) { return +x.Id === prev; })) W3B.setCombo('CmbItem', prev); else W3B.setCombo('CmbItem', f[0].Id);     // Rows[1].Activate()
        } else { W3B.fill('CmbItem', [], true, ''); }
    }
    function itemModalBind(categoryId, itemId) {                          // :802
        var prev = sel('CmbSampleModal'), f;
        if (itemId > 0) f = models.filter(function (x) { return +x.ItemId === itemId; });
        else if (categoryId > 0) f = models.filter(function (x) { return +x.ItemCategoryId === categoryId; });
        else f = models;
        if (f.length) {
            W3B.fill('CmbSampleModal', f.map(function (x) { return { Id: x.Id, Name: x.SampleNo }; }), true, '');
            W3B.setCombo('CmbSampleModal', f[0].Id);                                                  // Rows[1].Activate()
            if (prev > 0 && f.some(function (x) { return +x.Id === prev; })) W3B.setCombo('CmbSampleModal', prev);
        } else { W3B.fill('CmbSampleModal', [], true, ''); }
    }
    function bindCasting() {                                              // CastingTypedtFill:1074 + BindCastingType:1100
        var prev = sel('CmbCastingType'), rows = distinct(stock, 'CastingTypeId', 'CastingType');
        if (rows.length) { W3B.fill('CmbCastingType', rows, true, ''); if (prev > 0 && rows.some(function (x) { return +x.Id === prev; })) W3B.setCombo('CmbCastingType', prev); }
        else W3B.fill('CmbCastingType', [], true, '');
    }
    function attributesOf(itemId) {
        if (attrCache[itemId]) return Promise.resolve(attrCache[itemId]);
        return W3B.getJson(api + '/attributes?itemId=' + itemId).then(function (rows) { attrCache[itemId] = rows; return rows; });
    }
    function itemUomBind(itemId) {                                        // :901
        var prevPack = sel('CmbPackUom'), prevRate = sel('CmbRateUom'), prevVar = sel('CmbVariantDescription');
        var f = stock.filter(function (r) { return +r.ItemId === itemId; });
        var pack = distinct(f, 'ItemUomId', 'Uom'), rate = distinct(f, 'RateUomId', 'RateUom');
        packEq = {}; rateEq = {};
        f.forEach(function (r) { packEq[r.ItemUomId] = n(r.PackEquivalent); rateEq[r.RateUomId] = n(r.RateEquivalent); });
        W3B.fill('CmbPackUom', pack, false, ''); if (pack.length) { W3B.setCombo('CmbPackUom', pack[0].Id); if (prevPack > 0) keepOrFirst('CmbPackUom', pack, prevPack, false); }
        W3B.fill('CmbRateUom', rate, false, ''); if (rate.length) { W3B.setCombo('CmbRateUom', rate[0].Id); if (prevRate > 0) keepOrFirst('CmbRateUom', rate, prevRate, false); }
        return attributesOf(itemId).then(function (rows) {
            W3B.fill('CmbVariantDescription', rows, false, '');
            if (rows.length) { W3B.setCombo('CmbVariantDescription', rows[0].Id); if (prevVar > 0) keepOrFirst('CmbVariantDescription', rows, prevVar, false); }
        });
    }
    function bindCombos() {                                               // :1495
        seen = {};
        if (stock.length) {
            bindItemCategory(); bindBatch();
            itemModalBind(sel('CmbCategory'), 0);
            itemdtFill();
            itemDetailBind(sel('CmbCategory'), sel('CmbSampleModal'));
            bindCasting();
            return;
        }
        ['CmbCategory', 'CmbItem', 'CmbPackUom', 'CmbVariantDescription', 'CmbRateUom', 'CmbCastingType', 'CmbSampleModal', 'txtBatchNo'].forEach(function (id) { W3B.fill(id, [], true, ''); });
        ['CmbWorkOrderNo', 'CmbWareHouseFromDetail', 'CmbWarehouseToDetail', 'CmbProductionStageFromDetail', 'CmbProductionStageToDetail', 'CmbWorkStationFromDetail', 'CmbWorkStationToDetail'].forEach(clear);
        ['txtTransferQty', 'txtAvailableQty', 'txtAvgRate', 'txtAmount', 'txtRemarksDetail'].forEach(function (id) { $(id).value = ''; });
    }

    // ----------------------------------------------------------------------------- available qty / avg rate / amount
    function calcAvailable(vals) {                                        // CalculateAvailableQty:2573
        var keys = Object.keys(vals).filter(function (k) { var x = vals[k]; return x !== null && x !== undefined && x !== '' && n(x) > 0; });
        if (!keys.length) return 0;
        var map = { ItemId: 'ItemId', ItemUomId: 'ItemUomId', JobLotId: 'JobLotId', WarehouseId: 'WarehouseId', ProductionStageId: 'ProductionStageId', CastingTypeId: 'CastingTypeId' };
        var total = 0;
        stock.forEach(function (r) {
            if (keys.every(function (k) { return r[map[k]] !== null && String(r[map[k]]) === String(vals[k]); })) total += n(r.AvailableQty);
        });
        return total;
    }
    function availableForTextBox() {                                      // GetAvailableQtyForTextBox:2533
        var q = calcAvailable({ ItemId: v('CmbItem'), ItemUomId: v('CmbPackUom'), JobLotId: v('txtBatchNo'), WarehouseId: v('CmbWarehouseFrom'),
            ProductionStageId: v('CmbProductionStageFrom'), CastingTypeId: v('CmbCastingType') });
        $('txtAvailableQty').value = fmt(q, 2);
    }
    function availableForRow(r) {                                         // GetAvailableQtyForGridRow:2592
        r.AvailableQty = calcAvailable({ ItemId: r.ItemId, ItemUomId: r.UomId, JobLotId: r.JobLotId, WarehouseId: v('CmbWarehouseFrom'),
            ProductionStageId: v('CmbProductionStageFrom'), CastingTypeId: r.CastingTypeId });
    }
    function cgsAvgRate() {                                               // GetCgsAvgRate:2514 -> CommonServices.AvgRateOnlyForCGS
        var itemId = sel('CmbItem');
        if (itemId <= 0) return Promise.resolve(0);
        return W3B.getJson(api + '/avg-rate' + W3B.qs({ itemId: itemId, warehouseId: sel('CmbWareHouseFromDetail'), docDate: v('DocDate'), recId: recId })).then(function (d) { return n(d.avgRate); });
    }
    function refreshAvgRate() { return cgsAvgRate().then(function (r) { $('txtAvgRate').value = fmt(r, 3); calcAmount(); }); }
    function calcAmount() {                                               // CalculateAmount:2486
        var qty = n(v('txtTransferQty')), rate = n(v('txtAvgRate')), eq = 0;
        var ru = sel('CmbRateUom');
        if (ru > 0) eq = n(rateEq[ru]);
        if (qty > 0 && eq > 0 && rate > 0) $('txtAmount').value = fmt(r2(qty / eq * rate), 2); else $('txtAmount').value = '0';
    }
    function availableLeave() { availableForTextBox(); }

    // ----------------------------------------------------------------------------- Leave handlers
    function itemLeave() {                                                // CmbItem_Leave:1306
        var id = sel('CmbItem');
        if (id > 0) { var it = dtItem.filter(function (x) { return +x.Id === id; })[0]; if (it) W3B.setCombo('CmbCategory', it.ItemCategoryId); }
        return itemUomBind(id).then(function () { itemModalBind(0, id); availableForTextBox(); return refreshAvgRate(); }).catch(W3B.fail);
    }
    function sampleModalLeave() {                                         // :1326
        var id = sel('CmbSampleModal');
        if (id > 0) { var m = models.filter(function (x) { return +x.Id === id; })[0]; if (m) W3B.setCombo('CmbCategory', m.ItemCategoryId); }
        itemDetailBind(0, id);
    }
    function categoryLeave() {                                            // :1343
        itemDetailBind(sel('CmbCategory'), 0); itemModalBind(sel('CmbCategory'), 0);
    }
    function itemMode() {                                                 // rdbtnItemName_CheckedChanged:1546
        if (!dtItem.length) return;
        var prev = sel('CmbItem'), byName = $('rdbtnItemName').checked;
        W3B.fill('CmbItem', dtItem.map(function (x) { return { Id: x.Id, Name: byName ? x.ItemName : x.ItemCode }; }), true, '');
        if (prev > 0) W3B.setCombo('CmbItem', prev);
    }
    function warehouseFromDetailLeave() { availableForTextBox(); return refreshAvgRate(); }       // :3503 (the combo is disabled on the page; kept for completeness)
    var LEAVE = { CmbItem: itemLeave, CmbSampleModal: sampleModalLeave, CmbCategory: categoryLeave, CmbPackUom: availableLeave, CmbVariantDescription: availableLeave,
        txtBatchNo: availableLeave, CmbCastingType: availableLeave, CmbWorkStationFromDetail: availableLeave };
    function maybeLeave(id) {
        var s = $(id); if (!s || !LEAVE[id]) return;
        if (seen[id] === s.value) return;
        seen[id] = s.value;
        LEAVE[id]();
    }
    document.addEventListener('focusout', function (e) {
        var wrap = e.target.closest ? e.target.closest('.dtcombo-wrap') : null;
        if (!wrap || (e.relatedTarget && wrap.contains(e.relatedTarget))) return;
        var s = wrap.querySelector('select'); if (s && s.id) setTimeout(function () { maybeLeave(s.id); }, 0);
    });
    document.addEventListener('change', function (e) { var t = e.target; if (t && t.id && LEAVE[t.id]) maybeLeave(t.id); });

    // ----------------------------------------------------------------------------- transfer type
    function typeData(t) {                                                // GetTransferTypeData:1356
        return W3B.getJson(api + '/transfer-type?id=' + t).then(function (d) {
            W3B.show('DetailManualEntryPanel', !!d.showPanel);
            if (d.found) {
                W3B.setCombo('CmbWarehouseFrom', d.fromWarehouseId); W3B.setCombo('CmbWarehouseTo', d.toWarehouseId);
                W3B.setCombo('CmbProductionStageFrom', d.fromStageId); W3B.setCombo('CmbProductionStageTo', d.toStageId);
                W3B.setCombo('CmbWareHouseFromDetail', d.fromWarehouseId); W3B.setCombo('CmbWarehouseToDetail', d.toWarehouseId);
                W3B.setCombo('CmbProductionStageFromDetail', d.fromStageId); W3B.setCombo('CmbProductionStageToDetail', d.toStageId);
            } else {
                ['CmbWarehouseFrom', 'CmbWarehouseTo', 'CmbProductionStageFrom', 'CmbProductionStageTo', 'CmbWareHouseFromDetail', 'CmbWarehouseToDetail',
                    'CmbProductionStageFromDetail', 'CmbProductionStageToDetail'].forEach(clear);
            }
        });
    }
    function loadAvailableStock() {
        return W3B.getJson(api + '/available-stock' + W3B.qs({ warehouseId: sel('CmbWarehouseFrom'), stageId: sel('CmbProductionStageFrom'), docDate: v('DocDate') })).then(function (d) {
            stock = normalizeStock(d.stock); models = d.models || [];
        });
    }
    function clearEntryFields() {
        ['CmbCategory', 'CmbItem', 'CmbPackUom', 'CmbVariantDescription', 'CmbRateUom', 'CmbCastingType', 'CmbSampleModal', 'txtBatchNo'].forEach(function (id) { W3B.fill(id, [], true, ''); });
        ['CmbWorkOrderNo', 'CmbWareHouseFromDetail', 'CmbWarehouseToDetail', 'CmbProductionStageFromDetail', 'CmbProductionStageToDetail', 'CmbWorkStationFromDetail', 'CmbWorkStationToDetail'].forEach(clear);
        ['txtTransferQty', 'txtAvailableQty', 'txtAvgRate', 'txtAmount', 'txtRemarksDetail'].forEach(function (id) { $(id).value = ''; });
    }
    function transferTypeLeave() {                                        // CmbTransferType_Leave:1398
        var t = sel('CmbTransferType');
        if (t === 0) W3B.show('DetailManualEntryPanel', false);
        if (t > 0) {
            if (t === tag && sel('CmbWarehouseFrom') !== 0 && stock.length !== 0) return Promise.resolve();
            tag = t;
            return typeData(t).then(function () {
                if (t === 8 || t === 10) {
                    stock = [];
                    return loadAvailableStock().then(function () { bindCombos(); });
                }
                if (canSave()) {
                    if (table.length && I(table[0].ItemId) > 0 &&
                        !W3B.ask('you are changing Transfer Type and in that Case Data in Grid will be Reset,are you sure to do that?')) {
                        W3B.setCombo('CmbTransferType', gridTypeId); tag = gridTypeId; return null;
                    }
                    stock = []; table = []; removedKeep(); gridTypeId = sel('CmbTransferType');
                    return loadAvailableStock().then(function () { bindCombos(); addFromStock(stock); });
                }
                return null;
            }).catch(W3B.fail);
        }
        tag = 0; stock = []; models = [];
        clearEntryFields();
        return Promise.resolve();
    }
    function removedKeep() { /* lstRemoveRecord is not touched by a type change (it only matters on update) */ }

    // ----------------------------------------------------------------------------- grid
    function blankFromStock(d) {                                          // GetDetailByTransferTypeData:3486
        return { Id: 0, ItemId: d.ItemId, ItemName: d.ItemName, UomId: d.ItemUomId, Uom: d.Uom, VariantId: 0, VariantDescription: '', JobLotId: d.JobLotId, BatchNo: d.BatchNo,
            CastingTypeId: d.CastingTypeId || 0, CastingType: d.CastingType || '', WareHouseFromId: d.WarehouseId, WareHouseToId: sel('CmbWarehouseTo'),
            ProductionStageFromId: d.ProductionStageId, ProductionStageToId: sel('CmbProductionStageTo'), WorkStationFromId: 0, WorkStationToId: 0,
            AvailableQty: n(d.AvailableQty), TransferQty: n(d.AvailableQty), AvgRate: n(d.AvgRate), RateUomId: d.RateUomId, RateUom: d.RateUom, RateEquivalent: n(d.RateEquivalent),
            Amount: n(d.Amount), WorkOrderId: 0, Remarks: '' };
    }
    function addFromStock(rows) {
        if (!rows || !rows.length) { render(); return; }
        rows.forEach(function (d) { table.push(blankFromStock(d)); });
        render();
    }
    function opts(list, cur) {
        var h = '<option value="0"></option>';
        list.forEach(function (x) { h += '<option value="' + W3B.esc(x.Id) + '"' + (String(x.Id) === String(cur) ? ' selected' : '') + '>' + W3B.esc(x.Name) + '</option>'; });
        return h;
    }
    function inp(f, i, val, cls) { return '<input class="cell ' + (cls || '') + '" data-g="1" data-f="' + f + '" data-i="' + i + '" value="' + W3B.esc(val) + '" style="width:' + (cls === 'txt' ? 150 : 76) + 'px">'; }
    function selc(f, i, list, cur) { return '<select class="cell" data-g="1" data-f="' + f + '" data-i="' + i + '" style="width:130px">' + opts(list, cur) + '</select>'; }
    function render() {
        var showAvail = (sel('CmbTransferType') !== 6 && canSave());               // grdSettings:1993 (SaveAs is never visible)
        var cols = [
            { h: 'X', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return '<button type="button" class="rc-x" data-act="del" data-i="' + i + '">X</button>'; } },
            { h: 'ItemName', k: 'ItemName', w: 220 }, { h: 'Uom', k: 'Uom', w: 60 },
            { h: 'Variant Description', html: true, w: 130, f: function (r, i) { return '<button type="button" class="rc-link pk" data-pk="variant" data-i="' + i + '">' + (r.VariantDescription ? W3B.esc(r.VariantDescription) : '&hellip;') + '</button>'; } },
            { h: 'BatchNo', k: 'BatchNo', w: 100 }, { h: 'CastingType', k: 'CastingType', w: 110 },
            { h: 'WareHouse From', f: function (r) { return W3B.esc(nameOf(L.wh, r.WareHouseFromId) || r.WarehouseFrom || ''); }, html: true, w: 130 },
            { h: 'WareHouse To', f: function (r) { return W3B.esc(nameOf(L.wh, r.WareHouseToId) || r.WarehouseTo || ''); }, html: true, w: 130 },
            { h: 'ProductionStage From', f: function (r) { return W3B.esc(nameOf(L.stage, r.ProductionStageFromId) || r.ProductionStageFrom || ''); }, html: true, w: 120 },
            { h: 'ProductionStage To', f: function (r) { return W3B.esc(nameOf(L.stage, r.ProductionStageToId) || r.ProductionStageTo || ''); }, html: true, w: 120 },
            { h: 'WorkStation From', html: true, w: 140, f: function (r, i) { return selc('WorkStationFromId', i, L.ws, r.WorkStationFromId); } },
            { h: 'WorkStation To', html: true, w: 140, f: function (r, i) { return selc('WorkStationToId', i, L.ws, r.WorkStationToId); } }
        ];
        if (showAvail) cols.push({ h: 'AvailableQty', cls: 'num', f: function (r, i) { return '<span data-c="AvailableQty" data-i="' + i + '">' + fmt(r.AvailableQty, 3) + '</span>'; }, html: true });
        cols.push(
            { h: 'TransferQty', html: true, cls: 'num', f: function (r, i) { return inp('TransferQty', i, r.TransferQty === 0 ? '' : r.TransferQty); } },
            { h: 'AvgRate', html: true, cls: 'num', f: function (r, i) { return inp('AvgRate', i, r.AvgRate === 0 ? '' : r.AvgRate); } },
            { h: 'RateUom', k: 'RateUom', w: 60 },
            { h: 'Amount', html: true, cls: 'num', f: function (r, i) { return '<span data-c="Amount" data-i="' + i + '">' + fmt(r.Amount, 3) + '</span>'; } },
            { h: 'WorkOrder No', html: true, w: 130, f: function (r, i) { return selc('WorkOrderId', i, L.wo, r.WorkOrderId); } },
            { h: 'Remarks', html: true, f: function (r, i) { return inp('Remarks', i, r.Remarks, 'txt'); } });
        $('grd').innerHTML = W3B.tableHtml(cols, table, { empty: 'No rows', rowClass: function (r, i) { return i === updateIdx ? 'is-current' : ''; } });
    }
    function updateRowCells(i) {
        var r = table[i]; if (!r) return;
        var a = document.querySelector('#grd [data-c="Amount"][data-i="' + i + '"]'); if (a) a.textContent = fmt(r.Amount, 3);
        var q = document.querySelector('#grd [data-c="AvailableQty"][data-i="' + i + '"]'); if (q) q.textContent = fmt(r.AvailableQty, 3);
    }
    $('grd').addEventListener('input', function (e) {
        var t = e.target; if (!t.dataset || t.dataset.g !== '1') return;
        var r = table[+t.dataset.i]; if (!r) return;
        var f = t.dataset.f;
        if (f === 'Remarks') r.Remarks = t.value;
        else if (f === 'TransferQty' || f === 'AvgRate') r[f] = t.value === '' ? 0 : n(t.value);
    });
    $('grd').addEventListener('change', function (e) {                    // grd_CellUpdated:2165
        var t = e.target; if (!t.dataset || t.dataset.g !== '1') return;
        var i = +t.dataset.i, r = table[i]; if (!r) return;
        var f = t.dataset.f;
        if (f === 'WorkStationFromId' || f === 'WorkStationToId' || f === 'WorkOrderId') r[f] = I(t.value);
        if (f === 'TransferQty' || f === 'AvgRate') {
            var qty = n(r.TransferQty), rate = n(r.AvgRate), eq = n(r.RateEquivalent);
            r.Amount = (qty > 0 && rate > 0 && eq > 0) ? qty / eq * rate : 0;
        }
        availableForRow(r);
        updateRowCells(i);
    });
    document.addEventListener('click', function (e) {
        var b = e.target.closest ? e.target.closest('button[data-act="del"]') : null;
        if (b) { deleteRow(+b.dataset.i); return; }
        var p = e.target.closest ? e.target.closest('button.pk[data-pk="variant"]') : null;
        if (p) { pickVariant(+p.dataset.i); return; }
        var h = e.target.closest ? e.target.closest('button[data-hact]') : null;
        if (h) {
            var hi = +h.dataset.hi;
            if (h.dataset.hact === 'edit') { reset().then(function () { return load(hist[hi].Id); }); }
            else printSlip(hist[hi].Id);
            return;
        }
        var tr = e.target.closest ? e.target.closest('#grdhistory tbody tr[data-i]') : null;
        if (tr) historyDetail(hist[+tr.dataset.i].Id);
    });
    document.addEventListener('dblclick', function (e) {
        var tr = e.target.closest ? e.target.closest('#grd tbody tr[data-i]') : null;
        if (tr && !e.target.closest('input,select,button')) { editRow(+tr.dataset.i); return; }
        var th = e.target.closest ? e.target.closest('#grdhistory tbody tr[data-i]') : null;
        if (th && !e.target.closest('button')) load(hist[+th.dataset.i].Id);                    // grdhistory_DoubleClick_1:3075
    });
    document.addEventListener('keydown', function (e) {
        if (e.key === 'F1') { var p = e.target.closest ? e.target.closest('button.pk') : null; if (p) { e.preventDefault(); pickVariant(+p.dataset.i); } }
        if (e.ctrlKey && e.key === 'Delete') {                                                   // grd_KeyDown:2904
            var tr = e.target.closest ? e.target.closest('#grd tbody tr[data-i]') : null;
            if (tr) { e.preventDefault(); deleteRow(+tr.dataset.i); }
        }
    });
    function pickVariant(i) {                                             // grd_KeyDown F1:2924
        var r = table[i]; if (!r) return;
        if (I(r.ItemId) <= 0) { alertMsg('Please Select an Item First'); return; }
        attributesOf(r.ItemId).then(function (rows) {
            pick('Variant', rows, function (a) { r.VariantId = a.Id; r.VariantDescription = a.Name; availableForRow(r); render(); });
        }).catch(W3B.fail);
    }
    function deleteRow(i) {                                               // DeleteDetailRow:1910
        var r = table[i]; if (!r) return;
        if (I(r.Id) > 0) {
            if (!W3B.ask('Are you sure to Delete?')) return;
            removed.push({ Id: r.Id, ItemId: r.ItemId, UomId: r.UomId, VariantId: r.VariantId, JobLotId: r.JobLotId, BatchNo: r.BatchNo, WorkOrderId: r.WorkOrderId,
                WareHouseFromId: r.WareHouseFromId, WareHouseToId: r.WareHouseToId, ProductionStageFromId: r.ProductionStageFromId, ProductionStageToId: r.ProductionStageToId,
                WorkStationFromId: r.WorkStationFromId, WorkStationToId: r.WorkStationToId, TransferQty: r.TransferQty, AvgRate: r.AvgRate, RateUomId: r.RateUomId,
                Amount: r.Amount, CastingTypeId: r.CastingTypeId, Remarks: r.Remarks });
        }
        table.splice(i, 1);                                               // AddRowInGrid() is an empty method on the desktop: the grid may stay empty
        if (updateIdx === i) cancelDetail(); else if (updateIdx > i) updateIdx--;
        render();
    }

    // ----------------------------------------------------------------------------- entry panel buttons
    function validateDetail() {                                           // FormValidationDetail:1595
        function bad(msg, focus) { alertMsg(msg); W3B.focus(focus); return false; }
        if (sel('CmbItem') === 0) return bad('Item / Modal Field is Required', 'CmbItem');
        if (sel('CmbPackUom') === 0) return bad('Uom Field is Required', 'CmbPackUom');
        if (sel('txtBatchNo') === 0) return bad('Batch No Field is Required', 'txtBatchNo');
        if (sel('CmbWareHouseFromDetail') === 0) return bad('Warehouse From Field is Required', 'CmbWareHouseFromDetail');
        if (sel('CmbWarehouseToDetail') === 0) return bad('Warehouse To Field is Required', 'CmbWarehouseToDetail');
        if (sel('CmbWareHouseFromDetail') === sel('CmbWarehouseToDetail')) return bad("Warehouse From And Warehouse To can't be same", 'CmbWarehouseToDetail');
        if (sel('CmbProductionStageFromDetail') === 0) return bad('Production From Field is Required', 'CmbProductionStageFromDetail');
        if (sel('CmbProductionStageToDetail') === 0) return bad('Production To Field is Required', 'CmbProductionStageToDetail');
        if (sel('CmbTransferType') !== 8 && sel('CmbProductionStageFromDetail') === sel('CmbProductionStageToDetail')) return bad("Production Stage From And Production Stage To can't be same", 'CmbProductionStageToDetail');
        if (n(v('txtTransferQty')) <= 0) return bad('Transfer Qty Field is Required', 'txtTransferQty');
        return true;
    }
    function itemNameOf(id) { var it = dtItem.filter(function (x) { return +x.Id === +id; })[0]; return it ? it.ItemName : ''; }
    function rateEqOf() { var id = sel('CmbRateUom'); return id > 0 ? n(rateEq[id]) : 0; }
    function addDetail() {                                                // btnplus_Click:2054
        if (!validateDetail()) return;
        if (!sel('CmbRateUom')) { alertMsg('Object reference not set to an instance of an object.'); return; }      // SelectedRow is null on the desktop
        table.push({ Id: 0, ItemId: sel('CmbItem'), ItemName: itemNameOf(sel('CmbItem')), UomId: sel('CmbPackUom'), Uom: W3B.selText('CmbPackUom'),
            VariantId: sel('CmbVariantDescription'), VariantDescription: W3B.selText('CmbVariantDescription'), JobLotId: sel('txtBatchNo'), BatchNo: W3B.selText('txtBatchNo'),
            CastingTypeId: sel('CmbCastingType'), CastingType: W3B.selText('CmbCastingType'), WareHouseFromId: sel('CmbWareHouseFromDetail'),
            WareHouseToId: sel('CmbWarehouseTo'), ProductionStageFromId: sel('CmbProductionStageFrom'), ProductionStageToId: sel('CmbProductionStageTo'),     // header combos, as btnplus_Click:2060
            WorkStationFromId: sel('CmbWorkStationFromDetail'), WorkStationToId: sel('CmbWorkStationToDetail'), AvailableQty: n(v('txtAvailableQty')),
            TransferQty: n(v('txtTransferQty')), AvgRate: n(v('txtAvgRate')), RateUomId: sel('CmbRateUom'), RateUom: W3B.selText('CmbRateUom'), RateEquivalent: rateEqOf(),
            Amount: n(v('txtAmount')), WorkOrderId: sel('CmbWorkOrderNo'), Remarks: v('txtRemarksDetail') });
        fromResetDetail(); render();
    }
    function updateDetail() {                                             // btnUpdateDetail_Click:2073
        if (!validateDetail()) return;
        if (!sel('CmbRateUom')) { alertMsg('Object reference not set to an instance of an object.'); return; }
        var r = table[updateIdx]; if (!r) return;
        r.ItemId = sel('CmbItem'); r.ItemName = itemNameOf(sel('CmbItem')); r.UomId = sel('CmbPackUom'); r.Uom = W3B.selText('CmbPackUom');
        r.VariantId = sel('CmbVariantDescription'); r.VariantDescription = W3B.selText('CmbVariantDescription'); r.BatchNo = W3B.selText('txtBatchNo');      // JobLotId is not updated (desktop :2085)
        r.WorkOrderId = sel('CmbWorkOrderNo'); r.WareHouseFromId = sel('CmbWareHouseFromDetail'); r.WareHouseToId = sel('CmbWarehouseToDetail');
        r.ProductionStageFromId = sel('CmbProductionStageFromDetail'); r.ProductionStageToId = sel('CmbProductionStageToDetail');
        r.WorkStationFromId = sel('CmbWorkStationFromDetail'); r.WorkStationToId = sel('CmbWorkStationToDetail');
        r.AvailableQty = n(v('txtAvailableQty')); r.TransferQty = n(v('txtTransferQty')); r.AvgRate = n(v('txtAvgRate')); r.RateUomId = sel('CmbRateUom');
        r.RateUom = W3B.selText('CmbRateUom'); r.RateEquivalent = rateEqOf(); r.Amount = n(v('txtAmount')); r.CastingTypeId = sel('CmbCastingType');
        r.CastingType = W3B.selText('CmbCastingType'); r.Remarks = v('txtRemarksDetail');
        fromResetDetail(); render();
    }
    function cancelDetail() { fromResetDetail(); render(); }
    function fromResetDetail() {                                          // FromResetDetail:1763
        updateIdx = -1;
        ['CmbCategory', 'CmbItem', 'CmbPackUom', 'CmbVariantDescription', 'CmbWorkOrderNo', 'CmbWorkStationFromDetail', 'CmbWorkStationToDetail', 'CmbRateUom', 'CmbCastingType'].forEach(clear);
        ['txtTransferQty', 'txtAvailableQty', 'txtAvgRate', 'txtAmount', 'txtRemarksDetail'].forEach(function (id) { $(id).value = ''; });
        W3B.show('btnplus', true); W3B.show('btnUpdateDetail', false); W3B.show('btnCancelUpdateDetial', false);
        seen = {};
    }
    function editRow(i) {                                                 // grd_DoubleClick:2113
        var r = table[i], t = sel('CmbTransferType');
        if (!r || !(t === 6 || t === 8)) return;
        updateIdx = i;
        clear('CmbCategory'); categoryLeave();
        W3B.setCombo('CmbItem', r.ItemId);
        itemLeave().then(function () {
            W3B.setCombo('CmbPackUom', r.UomId); W3B.setCombo('CmbVariantDescription', r.VariantId);
            var bo = Array.prototype.filter.call($('txtBatchNo').options, function (o) { return o.text === r.BatchNo; })[0]; if (bo) $('txtBatchNo').value = bo.value;
            W3B.setCombo('CmbWorkOrderNo', r.WorkOrderId);
            W3B.setCombo('CmbWareHouseFromDetail', r.WareHouseFromId); W3B.setCombo('CmbWarehouseToDetail', r.WareHouseToId);
            W3B.setCombo('CmbProductionStageFromDetail', r.ProductionStageFromId); W3B.setCombo('CmbProductionStageToDetail', r.ProductionStageToId);
            W3B.setCombo('CmbWorkStationFromDetail', r.WorkStationFromId); W3B.setCombo('CmbWorkStationToDetail', r.WorkStationToId);
            $('txtTransferQty').value = fmt(r.TransferQty, 3); $('txtAvgRate').value = fmt(r.AvgRate, 3);
            W3B.setCombo('CmbRateUom', r.RateUomId); $('txtAmount').value = fmt(r.Amount, 3);
            W3B.setCombo('CmbCastingType', r.CastingTypeId); $('txtRemarksDetail').value = r.Remarks;
            W3B.show('btnplus', false); W3B.show('btnUpdateDetail', true); W3B.show('btnCancelUpdateDetial', true);
            render();
        });
    }

    // ----------------------------------------------------------------------------- loader dialog (frmLoadAvailableStockEngr)
    function loadStock() {                                                // BtnLoadStock_Click:3456
        if (sel('CmbTransferType') === 0) { alertMsg('Please Select Transfer Type First...'); return; }
        ldChecked = {};
        var cats = distinct(stock, 'ItemCategoryId', 'ItemCategory');
        W3B.fill('LdCategory', cats, false, '');
        loadCategory();
        $('loadModal').classList.add('is-open');
        loadFill();
    }
    function loadCategory() {                                             // ItemDetailBind (loader):142 - the filtered rows, see the class notes
        var cat = I(v('LdCategory')), f = cat > 0 ? stock.filter(function (r) { return +r.ItemCategoryId === cat; }) : stock;
        var items = distinct(f, 'ItemId', 'ItemName');
        W3B.fill('LdItem', items, true, '');
    }
    function loadFill() {                                                 // FilterAvailableStock:250
        var cat = I(v('LdCategory')), item = I(v('LdItem'));
        ldRows = stock.filter(function (r) { return (cat <= 0 || +r.ItemCategoryId === cat) && (item <= 0 || +r.ItemId === item); });
        ldChecked = {};
        renderLoader();
    }
    function ldKey(r) { return [r.ItemId, r.ItemUomId, r.JobLotId, r.WarehouseId, r.ProductionStageId, r.CastingTypeId].join('|'); }
    function renderLoader() {
        $('ldGrid').innerHTML = W3B.tableHtml([
            { h: '', html: true, w: 26, f: function (r, i) { return '<input type="checkbox" data-ld="' + i + '"' + (ldChecked[i] ? ' checked' : '') + '>'; } },
            { h: 'ItemCategory', k: 'ItemCategory' }, { h: 'ItemName', k: 'ItemName' }, { h: 'Uom', k: 'Uom' }, { h: 'BatchNo', k: 'BatchNo' }, { h: 'WareHouse', k: 'WareHouseName' },
            { h: 'ProductionStage', k: 'FromProductionStage' }, { h: 'CastingType', k: 'CastingType' },
            { h: 'AvailableQty', cls: 'num', f: function (r) { return fmt(r.AvailableQty, 3); } }, { h: 'AvgRate', cls: 'num', f: function (r) { return fmt(r.AvgRate, 3); } },
            { h: 'RateUom', k: 'RateUom' }, { h: 'Amount', cls: 'num', f: function (r) { return fmt(r.Amount, 3); } }
        ], ldRows, { empty: 'No records' });
    }
    $('ldGrid').addEventListener('change', function (e) { var t = e.target; if (t.dataset && t.dataset.ld !== undefined) ldChecked[+t.dataset.ld] = t.checked; });
    function loadPick() {                                                 // btnLoadOnInvoice_Click_1:358
        var idx = Object.keys(ldChecked).filter(function (k) { return ldChecked[k]; });
        if (!idx.length) { alertMsg('Check the row first'); return; }
        var keys = {}, picked = [];
        idx.forEach(function (k) {
            var r = ldRows[+k], key = ldKey(r);
            if (!keys[key]) { keys[key] = 1; var m = stock.filter(function (s) { return ldKey(s) === key; })[0]; if (m) picked.push(m); }
        });
        loadClose();
        addFromStock(picked);
    }
    function loadClose() { $('loadModal').classList.remove('is-open'); }

    // ----------------------------------------------------------------------------- toolbar
    function applyRights() {
        $('btnSave').disabled = !rights.save; $('btnUpdate').disabled = !rights.update; $('btnDelete').disabled = !rights.update;
        $('btnPrint').disabled = !rights.print; $('ChkBoxPrintPreview').disabled = !rights.print; $('ChkBoxPrintPreview').checked = !!rights.print;
        var m = [];
        if (!rights.save) m.push('Save'); if (!rights.update) m.push('Update'); if (!rights.print) m.push('Print');
        var nt = $('rightsNote');
        nt.textContent = m.length ? 'Your account does not have the ' + m.join(', ') + ' right on this screen, so those buttons are off.' : '';
        nt.classList.toggle('is-hidden', !m.length);
    }
    function mode() {
        var edit = recId > 0;
        W3B.show('btnSave', !edit); W3B.show('btnUpdate', edit); W3B.show('btnDelete', edit);
    }
    function reset() {                                                    // Reset:1687
        removed = []; recId = 0; isApproved = false; gridTypeId = 0; tag = 0; updateIdx = -1; seen = {}; stock = []; models = []; table = [];
        ['CmbWarehouseFrom', 'CmbWarehouseTo', 'CmbProductionStageFrom', 'CmbProductionStageTo', 'CmbTransferType'].forEach(clear);
        $('CmbTransferType').disabled = false;
        $('txtRemarksHead').value = '';
        clearEntryFields();
        W3B.show('btnplus', true); W3B.show('btnUpdateDetail', false); W3B.show('btnCancelUpdateDetial', false);
        mode(); render(); W3B.show('DetailManualEntryPanel', false);
        return W3B.getJson(api + '/doc-no').then(function (d) { $('txtdocno').value = d.docNo; }).catch(W3B.fail);
    }
    function newClick() { reset(); }
    function refresh() {                                                  // btnRefresh_Click:1660
        W3B.getJson(api + '/refresh').then(function (d) { applyLookups(d); render(); }).catch(W3B.fail);
    }
    function docDateChanged() {                                           // DocDate_ValueChanged:2668
        if (sel('CmbItem') > 0) refreshAvgRate().catch(W3B.fail);
    }
    function formValidation() {                                           // :1578
        var d = v('txtdocno').trim();
        if (d === '' || d === '0') { alertMsg('DocNo Field is Required'); return false; }
        if (sel('CmbTransferType') === 0) { alertMsg('TransferType Field is Required'); W3B.focus('CmbTransferType'); return false; }
        return true;
    }
    var saving = false;
    function save() {                                                     // Insert:2199
        if (saving) return;
        if (!formValidation()) return;
        if (!table.length) { alertMsg('Grid Record Not Found'); return; }
        if (recId > 0 && isApproved) { alertMsg('Approved Record Not Update'); return; }                   // btnupdate_Click:2385
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var t = sel('CmbTransferType');
        for (var i = 0; i < table.length; i++) {                                                           // the available-qty check is done first, row by row (:2265)
            if (t !== 6 && canSave() && n(table[i].TransferQty) > n(table[i].AvailableQty)) { alertMsg('Item Quantity cannot greater than Available Qty in Detail in Row No: ' + (i + 1)); return; }
        }
        saving = true;
        W3B.postJson(api + '/save', { id: recId, docDate: v('DocDate'), docNo: v('txtdocno'), transferTypeId: t, remarksHeader: v('txtRemarksHead'), rows: table, removed: removed })
            .then(function (d) {
                saving = false;
                alertMsg(d.message);
                var id = d.id, doPrint = $('ChkBoxPrintPreview').checked;
                return reset().then(function () { if (doPrint) printSlip(id); });
            }).catch(function (e) { saving = false; W3B.fail(e); });
    }
    function del() {                                                      // btnDelete_Click:2445
        if (recId <= 0) { alertMsg('RecId not Found'); return; }
        if (!W3B.ask('Are you sure to Delete?')) return;
        W3B.postForm(api + '/delete?id=' + recId).then(function (d) { alertMsg(d.message); return reset(); }).catch(W3B.fail);
    }
    function printSlip(id) {                                              // CommonServices.StockTransfer_Slip_1663(id) -> 1663_StockTransfer_Slip.rpt
        if (!id) { alertMsg('Record Not Found For Display'); return; }
        window.open('/reports/print/1663-stock-transfer-slip?id=' + encodeURIComponent(id), '_blank');
    }
    function print() { printSlip(recId); }

    // ----------------------------------------------------------------------------- ReadById:2400
    function load(id) {
        return reset().then(function () {
            recId = id;
            return W3B.getJson(api + '/read/' + id);
        }).then(function (d) {
            mode();
            tab('form');
            $('CmbTransferType').disabled = !canSave();                                                    // (btnsave.Enabled && btnsave.Visible) || SaveAs: off for an existing record
            $('DocDate').value = d.docDate; $('txtdocno').value = d.docNo;
            W3B.setCombo('CmbTransferType', d.transferTypeId);
            return transferTypeLeave().then(function () {
                $('txtRemarksHead').value = d.remarksHeader; isApproved = d.isApproved;
                table = d.rows; render();
            });
        }).catch(function (e) { recId = 0; mode(); W3B.fail(e); });
    }

    // ----------------------------------------------------------------------------- history
    function tab(t) {
        $('panelForm').classList.toggle('is-active', t === 'form'); $('panelHist').classList.toggle('is-active', t === 'hist');
        $('tabForm').classList.toggle('is-active', t === 'form'); $('tabHist').classList.toggle('is-active', t === 'hist');
    }
    function dateType() {                                                 // cmbperemeter_ValueChanged:3026
        var t = I(v('cmbperemeter')), now = new Date(), today = W3B.iso(now);
        if (t === 1) $('fromdate').value = today;
        else if (t === 2) { var d = new Date(now); d.setDate(now.getDate() - 7); $('fromdate').value = W3B.iso(d); }
        else if (t === 3) { $('fromdate').value = W3B.iso(new Date(Date.UTC(now.getUTCFullYear(), now.getUTCMonth(), 1))); $('ToDate').value = today; }
        else if (t === 4) { $('ToDate').value = now.getFullYear() + '-01-01'; $('fromdate').value = today; }
        else if (t === 5) { if (look.financialYearStart) $('fromdate').value = look.financialYearStart; }
    }
    function history() {                                                  // gridhistoryfill:3105
        var q = { fromDocNo: I(v('txtDocNoFrom')), toDocNo: I(v('txtDocNoTo')) };
        if ($('chkFrom').checked) q.fromDate = v('fromdate');
        if ($('chkTo').checked) q.toDate = v('ToDate');
        W3B.getJson(api + '/history' + W3B.qs(q)).then(function (rows) {
            hist = rows;
            $('GridDetailHistory').innerHTML = '';
            if (!rows.length) { $('grdhistory').innerHTML = ''; return; }
            $('grdhistory').innerHTML = W3B.tableHtml([
                { h: 'Edit', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-hact="edit" data-hi="' + i + '">Edit</button>'; } },
                { h: 'Print', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-hact="print" data-hi="' + i + '">Print</button>'; } },
                { h: 'DocNo', k: 'DocNo' }, { h: 'DocDate', f: function (r) { return W3B.fmtDMY(r.DocDate); } }, { h: 'TransferType', k: 'TransferType' },
                { h: 'FromWarehouse', k: 'FromWarehouse' }, { h: 'ToWarehouse', k: 'ToWarehouse' }, { h: 'ProductionStageFrom', k: 'ProductionStageFrom' }, { h: 'ProductionStageTo', k: 'ProductionStageTo' },
                { h: 'EntryDate', f: function (r) { return W3B.fmtStamp(r.EntryDate); } }, { h: 'EntryUser', k: 'EntryUserName' },
                { h: 'ModifyDate', f: function (r) { return W3B.fmtStamp(r.ModifyDate); } }, { h: 'ModifyUser', k: 'ModifyUserName' },
                { h: 'RemarksHeader', k: 'RemarksHeader' }, { h: 'NoOfAttachments', k: 'NoOfAttachments', cls: 'num' }
            ], rows, { empty: 'No records' });
        }).catch(W3B.fail);
    }
    function historyDetail(id) {                                          // grdhistory_SelectionChanged:3282
        W3B.getJson(api + '/read/' + id).then(function (d) {
            if (!d.rows.length) { $('GridDetailHistory').innerHTML = ''; return; }
            $('GridDetailHistory').innerHTML = W3B.tableHtml([
                { h: 'ItemName', k: 'ItemName' }, { h: 'Uom', k: 'Uom' }, { h: 'VariantDescription', k: 'VariantDescription' }, { h: 'BatchNo', k: 'BatchNo' },
                { h: 'CastingType', k: 'CastingType' }, { h: 'WorkStationFrom', k: 'WorkStationFrom' }, { h: 'WorkStationTo', k: 'WorkStationTo' },
                { h: 'TransferQty', k: 'TransferQty', cls: 'num' }, { h: 'AvgRate', k: 'AvgRate', cls: 'num' }, { h: 'RateUom', k: 'RateUom' }, { h: 'Amount', k: 'Amount', cls: 'num' },
                { h: 'WorkOrderNo', k: 'WorkOrderNo' }, { h: 'Remarks', k: 'Remarks' }
            ], d.rows, { empty: 'No rows' });
        }).catch(W3B.fail);
    }
    function newHistory() {                                               // btnNewHistory_Click:3265
        var now = new Date(), f = new Date(now); f.setDate(now.getDate() - 3);
        $('fromdate').value = W3B.iso(f); $('ToDate').value = W3B.iso(now); $('txtDocNoFrom').value = ''; $('txtDocNoTo').value = '';
        $('grdhistory').innerHTML = ''; $('GridDetailHistory').innerHTML = ''; hist = [];
    }

    window.Sm = { newClick: newClick, refresh: refresh, save: save, del: del, print: print, tab: tab, history: history, newHistory: newHistory, dateType: dateType,
        transferTypeLeave: function () { maybeLeaveType(); }, categoryLeave: categoryLeave, sampleModalLeave: sampleModalLeave, itemLeave: itemLeave, itemMode: itemMode,
        availableLeave: availableLeave, calcAmount: calcAmount, addDetail: addDetail, updateDetail: updateDetail, cancelDetail: cancelDetail, docDateChanged: docDateChanged,
        loadStock: loadStock, loadCategory: loadCategory, loadFill: loadFill, loadPick: loadPick, loadClose: loadClose, pickClose: pickClose };
    function maybeLeaveType() { transferTypeLeave(); }
    $('CmbTransferType').addEventListener('focusout', function () { /* the change handler already ran the Leave; the tag guard makes a repeat harmless */ });

    W3B.keys({ n: newClick, s: save, u: function () { if (recId > 0) save(); }, r: refresh, p: print });

    // ----------------------------------------------------------------------------- boot (InitializeComponentMethod:465)
    W3B.getJson(api + '/lookups').then(function (d) {
        look = d; rights = d.rights || {};
        applyRights(); applyLookups(d);
        W3B.fill('cmbperemeter', d.dateTypes || [], false);
        $('cmbperemeter').value = '2';                                                                // Rows[1].Activate() -> From = today - 7 ...
        $('DocDate').value = W3B.today();
        var now = new Date(), days = d.defaultDaysToLessFromHistoryFromDate > 0 ? d.defaultDaysToLessFromHistoryFromDate : 3, f = new Date(now);
        f.setDate(now.getDate() - days);                                                              // ... then InitializeHistoryDefaults:522 overwrites it
        $('fromdate').value = W3B.iso(f); $('ToDate').value = W3B.iso(now);
        $('txtdocno').value = d.docNo;
        mode(); bindCombos(); render();
    }).catch(W3B.fail);
}());
