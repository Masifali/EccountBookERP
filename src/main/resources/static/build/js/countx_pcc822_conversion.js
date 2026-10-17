/* ============================================================================================
 * Screen 822 "Stock Conversion" - Architecture.WinApp.Pcc.frmConversion (base.Name "frmConversion", DocumentTypeId 1867).
 * Desktop line numbers (.cs): frmBillOfMaterial_Manufacturing_Load:~480, FormValidation:1060, FormValidationOutPutGrid:1125, CmbPlantName_Leave:960,
 * CmbAttribute_Leave:942, grdInputMaterial_CellUpdated:1369, grdInputMaterial_KeyDown (F1 pickers):4283, grdOutPutByProduct_*:1556-2108 / 4677,
 * grdOverHead_CellUpdated:2184, grdContractorWages_*:2404 / 4966, Calculation:2685, Insert():2776, ReadById:3155, btnDelete_Click:3267,
 * btnGenerateRatesOutput_Click:5232 (UpdateInputCurrentStockAndAvgRate / UpdateWagesRates / UpdateRatesOutput), FromReset:2563.
 * API /production/pcc-conversion/api. Not ported: Attachment dialog (DMS).
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/pcc-conversion/api';
    var $ = W3B.$;
    var look = {}, rights = {}, fifo = false, amountDec = 2;
    var recId = 0, updateIdx = -1, checkWarehouse = true;
    var input = [], output = [], overhead = [], wages = [], rmInput = [], rmOutput = [];
    var itemsAll = [], itemsType = [], allUoms = [], plantWh = [], activeWh = [], contractors = [], accounts = [], hist = [];
    var attrCache = {};

    // ----------------------------------------------------------------------------- numbers (Conversion.ToInt = banker's rounding)
    function n(v) { return W3B.num(v); }
    function toIntBank(v) {
        var x = n(v), f = Math.floor(x), d = x - f;
        if (d < 0.5) return f;
        if (d > 0.5) return f + 1;
        return f % 2 === 0 ? f : f + 1;
    }
    function r2(v) { return Math.round((n(v) + Number.EPSILON) * 100) / 100; }
    function fmt2(v) { return n(v).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }); }
    function fmtQ(v) { return W3B.fmt(v, 4); }
    function alertMsg(m) { W3B.box(m); }

    // ----------------------------------------------------------------------------- blank rows (AddRowIn*Grid)
    function blankInput() {
        return { Id: 0, WarehouseId: 0, WarehouseName: '', ItemId: 0, ItemName: '', ItemUomId: 0, Uom: 0, UomCode: '', BalanceStock: 0, ItemQty: 0,
            ItemAttributeVarientId: 0, AttributeVarient: '', VarientEquivalent: 0, VarientQty: 0, NetWeight: 0, Rate: 0, RateUomId: 0, RateUom: 0, RateUomCode: '', Amount: 0 };
    }
    function blankOutput() {
        return { Id: 0, WarehouseId: 0, WarehouseName: '', ItemId: 0, ItemName: '', ItemAttributeVarientId: 0, AttributeVarient: '', VarientEquivalent: 0,
            ItemUomId: 0, Uom: 0, UomCode: '', ItemQty: 0, VarientQty: 0, NetWeight: 0, Rate: 0, RateUomId: 0, RateUom: 0, RateUomCode: '', Amount: 0 };
    }
    function blankOverhead() { return { Id: 0, OverHeadTypeId: 0, OverHeadAccountId: 0, OverHeadAccountTitle: '', PercentOfTotalInput: 0, Qty: 0, Rate: 0, Amount: 0, OhRemarks: '' }; }
    function blankWages() {
        return { Id: 0, OutPutItemId: 0, OutPutItemName: '', ContractorId: 0, ContractorName: '', ContractorWagesRateScheduleId: 0, ServiceActivity: '', Qty: 0, Rate: 0, Amount: 0, Remarks: '' };
    }

    // ----------------------------------------------------------------------------- picker (GrdPopUp)
    var pickRows = [], pickCb = null;
    function pick(title, rows, cb) {
        pickRows = rows || []; pickCb = cb;
        $('pickTitle').textContent = title;
        $('pickSearch').value = '';
        pickRender();
        $('pickModal').classList.add('is-open');
        W3B.focus('pickSearch');
    }
    function pickRender() {
        var q = $('pickSearch').value.toLowerCase(), shown = [];
        for (var i = 0; i < pickRows.length && shown.length < 400; i++) {
            var r = pickRows[i];
            if (!q || String(r.Name).toLowerCase().indexOf(q) >= 0 || (r.Code && String(r.Code).toLowerCase().indexOf(q) >= 0)) shown.push({ i: i, r: r });
        }
        var h = '<thead><tr><th style="width:80px">Id</th><th>Name</th></tr></thead><tbody>';
        if (!shown.length) h += '<tr class="rc-empty"><td colspan="2">No records</td></tr>';
        shown.forEach(function (s) { h += '<tr data-pi="' + s.i + '" class="pk-row"><td class="num">' + W3B.esc(s.r.Id) + '</td><td>' + W3B.esc(s.r.Name) + '</td></tr>'; });
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

    // Yes / No / Cancel
    function yesNoCancel(text) {
        return new Promise(function (resolve) {
            $('ynText').textContent = text;
            $('ynModal').classList.add('is-open');
            function done(v) { $('ynModal').classList.remove('is-open'); $('ynYes').onclick = $('ynNo').onclick = $('ynCancel').onclick = null; resolve(v); }
            $('ynYes').onclick = function () { done('yes'); };
            $('ynNo').onclick = function () { done('no'); };
            $('ynCancel').onclick = function () { done('cancel'); };
        });
    }

    // ----------------------------------------------------------------------------- lookups
    function itemLabelRows(rows) {
        var code = $('rdSearchByCode').checked;
        return (rows || []).map(function (r) { return { Id: r.Id, Name: code ? (r.Code || r.Name) : r.Name }; });
    }
    function fillItemCombo() { W3B.fill('cmbItemBOM', itemLabelRows(itemsType), true); }
    function itemSearchMode() { fillItemCombo(); }
    function applyLookups(d) {
        W3B.fill('CmbProductionType', d.productionTypes, true);
        W3B.fill('CmbPlantName', d.plants, true);
        allUoms = d.allUoms || []; activeWh = d.activeWarehouses || []; contractors = d.contractors || []; accounts = d.overheadAccounts || [];
        W3B.fill('CmbContractorForUpdateInWagesGrid', contractors, true);
    }
    function uomsOf(itemId) { return allUoms.filter(function (u) { return String(u.ItemId) === String(itemId); }); }
    function attributesOf(itemId) {
        if (attrCache[itemId]) return Promise.resolve(attrCache[itemId]);
        return W3B.getJson(api + '/attributes?itemId=' + itemId).then(function (rows) { attrCache[itemId] = rows; return rows; });
    }

    function loadItemsForType() {                                  // ItemDetailFill(TypeId)
        var t = W3B.int($('CmbProductionType').value);
        if (!t) { itemsType = []; fillItemCombo(); return Promise.resolve(); }
        return W3B.getJson(api + '/items?typeIds=' + t).then(function (rows) { itemsType = rows; fillItemCombo(); });
    }
    function productionTypeLeave() { loadItemsForType().then(syncEnable).catch(W3B.fail); }

    function plantLeave() {                                        // CmbPlantName_Leave:960
        var p = W3B.int($('CmbPlantName').value);
        return W3B.getJson(api + '/plant-warehouses?plantId=' + p).then(function (rows) {
            plantWh = rows;
            if (rows.length) {
                var keep = $('CmbWarehouseName').value;
                W3B.fill('CmbWarehouseName', rows, true);
                if (!W3B.int(keep)) {
                    var first = rows.filter(function (r) { return r.Id > 0; })[0];
                    if (first) $('CmbWarehouseName').value = String(first.Id);
                }
            }
            syncEnable();
        }).catch(W3B.fail);
    }

    function syncEnable() {                                        // EnableDisableProductionType / EnableDisablePlant
        var lockType = false;
        if (output.length && output[0].ItemId > 0) lockType = true; else if (input.length && input[0].ItemId > 0) lockType = true;
        $('CmbProductionType').disabled = lockType;
        $('CmbPlantName').disabled = !!(input.length && input[0].WarehouseId > 0);
    }

    // ----------------------------------------------------------------------------- rendering
    function ed(g, f, i, v, cls) {
        return '<input class="cell ' + (cls || '') + '" style="width:' + (cls === 'txt' ? 150 : 70) + 'px" inputmode="decimal" data-g="' + g + '" data-f="' + f + '" data-i="' + i + '" value="' + W3B.esc(cls === 'txt' ? v : (n(v) === 0 && v !== 0 ? v : (v === 0 ? '' : v))) + '">';
    }
    function pk(g, f, i, text) {
        return '<button type="button" class="rc-link pk" data-pk="' + g + '" data-f="' + f + '" data-i="' + i + '">' + (text ? W3B.esc(text) : '&hellip;') + '</button>';
    }
    function xb(g, i) { return '<button type="button" class="rc-x" data-act="del" data-g="' + g + '" data-i="' + i + '">X</button>'; }
    function ab(g, i) { return '<button type="button" class="rc-add" data-act="add" data-g="' + g + '" data-i="' + i + '">+</button>'; }
    function tot(rows, f) { return rows.reduce(function (a, r) { return a + n(r[f]); }, 0); }
    function totRow(label, cells) { return cells; }

    function render() {
        $('grdInputMaterial').innerHTML = W3B.tableHtml([
            { h: 'X', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xb('input', i); } },
            { h: '+', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return ab('input', i); } },
            { h: 'Warehouse', html: true, w: 150, f: function (r, i) { return pk('input', 'WarehouseName', i, r.WarehouseName); } },
            { h: 'ItemName', html: true, w: 230, f: function (r, i) { return pk('input', 'ItemName', i, r.ItemName); } },
            { h: 'Uom', html: true, w: 70, f: function (r, i) { return pk('input', 'UomCode', i, r.UomCode); } },
            { h: 'BalanceStock', cls: 'num', f: function (r) { return fmtQ(r.BalanceStock); } },
            { h: 'ItemQty', html: true, cls: 'num', f: function (r, i) { return ed('input', 'ItemQty', i, r.ItemQty); } },
            { h: 'Variant', html: true, w: 110, f: function (r, i) { return pk('input', 'AttributeVarient', i, r.AttributeVarient); } },
            { h: 'Variant Equivalent', cls: 'num', f: function (r) { return fmtQ(r.VarientEquivalent); } },
            { h: 'Variant Qty', cls: 'num', f: function (r) { return fmtQ(r.VarientQty); } },
            { h: 'Avg Rate', cls: 'num', f: function (r) { return W3B.fmt(r.Rate, look.rateDecimals || 2); } },
            { h: 'Rate Uom', html: true, w: 70, f: function (r, i) { return pk('input', 'RateUomCode', i, r.RateUomCode); } },
            { h: 'Amount', cls: 'num', f: function (r) { return fmt2(r.Amount); } }
        ], input, { empty: 'No input rows' });
        $('grdOutPutByProduct').innerHTML = W3B.tableHtml([
            { h: 'X', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xb('output', i); } },
            { h: '+', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return ab('output', i); } },
            { h: 'Warehouse', html: true, w: 150, f: function (r, i) { return pk('output', 'WarehouseName', i, r.WarehouseName); } },
            { h: 'ItemName', html: true, w: 230, f: function (r, i) { return pk('output', 'ItemName', i, r.ItemName); } },
            { h: 'Variant', html: true, w: 110, f: function (r, i) { return pk('output', 'AttributeVarient', i, r.AttributeVarient); } },
            { h: 'Variant Unit', cls: 'num', f: function (r) { return fmtQ(r.VarientEquivalent); } },
            { h: 'Qty', html: true, cls: 'num', f: function (r, i) { return ed('output', 'ItemQty', i, r.ItemQty); } },
            { h: 'T.Qty', cls: 'num', f: function (r) { return fmtQ(r.VarientQty); } },
            { h: 'Rate', cls: 'num', f: function (r) { return W3B.fmt(r.Rate, look.rateDecimals || 2); } },
            { h: 'Amount', cls: 'num', f: function (r) { return fmt2(r.Amount); } }
        ], output, { empty: 'No output rows', rowClass: function (r, i) { return i === updateIdx ? 'is-current' : ''; } });
        $('grdOverHead').innerHTML = W3B.tableHtml([
            { h: 'X', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xb('overhead', i); } },
            { h: '+', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return ab('overhead', i); } },
            { h: 'AccountTitle', html: true, w: 180, f: function (r, i) { return pk('overhead', 'OverHeadAccountId', i, r.OverHeadAccountTitle); } },
            { h: 'Percentage', html: true, cls: 'num', f: function (r, i) { return ed('overhead', 'PercentOfTotalInput', i, r.PercentOfTotalInput); } },
            { h: 'Qty', html: true, cls: 'num', f: function (r, i) { return ed('overhead', 'Qty', i, r.Qty); } },
            { h: 'Rate', html: true, cls: 'num', f: function (r, i) { return ed('overhead', 'Rate', i, r.Rate); } },
            { h: 'Amount', html: true, cls: 'num', f: function (r, i) { return ed('overhead', 'Amount', i, r.Amount); } },
            { h: 'OhRemarks', html: true, f: function (r, i) { return ed('overhead', 'OhRemarks', i, r.OhRemarks, 'txt'); } }
        ], overhead, { empty: 'No over head rows' });
        $('grdContractorWages').innerHTML = W3B.tableHtml([
            { h: 'X', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xb('wages', i); } },
            { h: '+', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return ab('wages', i); } },
            { h: 'OutPutItemName', html: true, w: 180, f: function (r, i) { return pk('wages', 'OutPutItemName', i, r.OutPutItemName); } },
            { h: 'ContractorName', html: true, w: 160, f: function (r, i) { return pk('wages', 'ContractorName', i, r.ContractorName); } },
            { h: 'ServiceActivity', html: true, w: 140, f: function (r, i) { return pk('wages', 'ServiceActivity', i, r.ServiceActivity); } },
            { h: 'Qty', html: true, cls: 'num', f: function (r, i) { return ed('wages', 'Qty', i, r.Qty); } },
            { h: 'Rate', cls: 'num', f: function (r) { return W3B.fmt(r.Rate, look.rateDecimals || 2); } },
            { h: 'Amount', cls: 'num', f: function (r) { return fmt2(r.Amount); } }
        ], wages, { empty: 'No wages rows' });
        syncEnable();
    }
    function arr(g) { return g === 'input' ? input : g === 'output' ? output : g === 'overhead' ? overhead : wages; }

    // ----------------------------------------------------------------------------- Calculation():2685
    function calculation() {
        var ti = tot(input, 'Amount'), to = tot(overhead, 'Amount'), tw = tot(wages, 'Amount');
        $('txtTotalInputs').value = input.length ? fmt2(r2(ti)) : '';
        $('txtOverHead').value = overhead.length ? fmt2(r2(to)) : '';
        $('txtWagesAmount').value = wages.length ? fmt2(r2(tw)) : '';
        var net = ti + to + tw;
        $('txtTotalOutPutAutoRatedAmount').value = fmt2(net);
        var vq = tot(output, 'VarientQty');
        $('txtOutPutAutoRatePerQtyAmount').value = vq ? fmt2(r2(net / vq)) : fmt2(0);
        return net;
    }

    // ----------------------------------------------------------------------------- input grid
    function weightAmount(r) {                                       // tail of grdInputMaterial_KeyDown after a pick
        r.NetWeight = (n(r.ItemQty) !== 0 && r.ItemUomId) ? n(r.ItemQty) * n(r.Uom) : 0;
        r.Amount = (r.NetWeight !== 0 && n(r.Rate) !== 0 && r.RateUomId && n(r.RateUom) !== 0) ? n(r.ItemQty) * n(r.Uom) / n(r.RateUom) * n(r.Rate) : 0;
    }
    function stockRefresh(r) {                                       // GetStockInHandAndAvgRateFromEvaluationConcrete (+ GetAvgRateFromFIFOMethodConcrete)
        return W3B.getJson(api + '/stock' + W3B.qs({ itemId: r.ItemId, warehouseId: r.WarehouseId, varientId: r.ItemAttributeVarientId, itemQty: n(r.ItemQty),
            docDate: $('txtdocdate').value, recId: recId })).then(function (s) {
            if (s.found) {
                r.BalanceStock = n(s.qtyInHand);
                r.Rate = s.fifo ? n(s.fifoRate) : n(s.avgRate);
                r.Amount = (n(r.RateUom) > 0 && n(r.Rate) > 0) ? n(r.ItemQty) * n(r.Uom) / n(r.RateUom) * n(r.Rate) : 0;
            } else { r.BalanceStock = 0; r.Rate = 0; r.Amount = 0; }
        });
    }
    function afterInputPick(r) {
        return stockRefresh(r).then(function () { weightAmount(r); render(); calculation(); }).catch(function (e) { render(); calculation(); W3B.fail(e); });
    }
    function inputQtyChanged(r) {                                    // grdInputMaterial_CellUpdated:1369 (ItemQty / Uom / ItemUomId / AttributeVarient)
        function amount() {
            r.Amount = (n(r.Rate) !== 0 && r.RateUomId && n(r.RateUom) !== 0) ? n(r.ItemQty) * n(r.Uom) / n(r.RateUom) * n(r.Rate) : 0;
        }
        if (n(r.ItemQty) !== 0 && r.ItemUomId) {
            r.NetWeight = n(r.ItemQty) * n(r.Uom);
            if (fifo && r.ItemId) {
                return W3B.getJson(api + '/stock' + W3B.qs({ itemId: r.ItemId, warehouseId: r.WarehouseId, varientId: r.ItemAttributeVarientId, itemQty: n(r.ItemQty),
                    docDate: $('txtdocdate').value, recId: recId })).then(function (s) { r.Rate = n(s.fifoRate); amount(); render(); calculation(); });
            }
            /* FIFO off: the stock average rate is read and discarded by the desktop (CellUpdated:1389) */
        } else { r.NetWeight = 0; }
        amount(); render(); calculation();
        return Promise.resolve();
    }

    function pickInput(i, f) {
        var r = input[i]; if (!r) return;
        if (f === 'ItemName') {
            pick('Item', itemsAll, function (it) {
                var prev = r.ItemId;
                r.ItemId = it.Id; r.ItemName = it.Name;
                if (it.Id !== prev) {          // :4330 - the desktop writes the item id into ItemUomId; the web leaves the UOM empty so the user picks it (validated on save)
                    r.ItemUomId = 0; r.Uom = 0; r.UomCode = ''; r.RateUom = 0; r.RateUomId = 0; r.RateUomCode = ''; r.NetWeight = 0; r.Amount = 0;
                }
                afterInputPick(r);
            });
        } else if (f === 'UomCode') {
            var u = uomsOf(r.ItemId).map(function (x) { return { Id: x.Id, Name: x.UOMCode, Eq: x.Equivalent }; });
            pick('Uom', u, function (x) {
                r.ItemUomId = x.Id; r.Uom = n(x.Eq); r.UomCode = x.Name; r.RateUom = n(x.Eq); r.RateUomId = x.Id; r.RateUomCode = x.Name;
                weightAmount(r); render(); calculation();
            });
        } else if (f === 'RateUomCode') {
            var ru = uomsOf(r.ItemId).map(function (x) { return { Id: x.Id, Name: x.UOMCode, Eq: x.Equivalent }; });
            pick('Rate Uom', ru, function (x) {
                r.RateUom = n(x.Eq); r.RateUomId = x.Id; r.RateUomCode = x.Name;
                afterInputPick(r);
            });
        } else if (f === 'WarehouseName') {
            if (!W3B.int($('CmbPlantName').value)) { alertMsg('Please Select Plant First.'); return; }
            W3B.getJson(api + '/plant-warehouses?plantId=' + W3B.int($('CmbPlantName').value)).then(function (rows) {
                plantWh = rows;
                pick('Warehouse', rows, function (w) { r.WarehouseId = w.Id; r.WarehouseName = w.Name; afterInputPick(r); });
            }).catch(W3B.fail);
        } else if (f === 'AttributeVarient') {
            attributesOf(r.ItemId).then(function (rows) {
                pick('Variant', rows, function (a) {
                    r.ItemAttributeVarientId = a.Id; r.AttributeVarient = a.Name; r.VarientEquivalent = n(a.Equivalent);
                    r.VarientQty = n(r.ItemQty) * n(r.VarientEquivalent);
                    afterInputPick(r);
                });
            }).catch(W3B.fail);
        }
    }

    function deleteInput(i) {                                        // DeleteInputDetailRow:1525
        var r = input[i]; if (!r) return;
        if (r.Id > 0) {
            if (!W3B.ask('Are you sure to Delete?')) return;
            rmInput.push({ Id: r.Id, WarehouseId: r.WarehouseId, ItemId: r.ItemId, ItemUomId: r.ItemUomId, ItemQty: r.ItemQty, ItemAttributeVarientId: r.ItemAttributeVarientId,
                VarientEquivalent: r.VarientEquivalent, VarientQty: r.VarientQty, NetWeight: r.NetWeight, Rate: r.Rate, RateUomId: r.RateUomId, Amount: r.Amount });
        }
        input.splice(i, 1);
        if (!input.length) input.push(blankInput());
        render(); calculation();
    }

    // ----------------------------------------------------------------------------- output grid + entry panel
    function validateOutputEntry() {                                 // FormValidationOutPutGrid:1125
        if (!W3B.int($('cmbItemBOM').value)) { alertMsg('ItemBOM Feild Required'); W3B.focus('cmbItemBOM'); return false; }
        if (!n($('txtQty').value)) { alertMsg('Qty Field Required'); W3B.focus('txtQty'); return false; }
        if (!W3B.int($('CmbAttribute').value)) { alertMsg('Item Attribute Field Required'); W3B.focus('CmbAttribute'); return false; }
        if (!n($('txtVarientEquivalent').value)) { alertMsg('Varient Equivalent Field Required'); return false; }
        if (!W3B.int($('CmbWarehouseName').value)) { alertMsg('Warehouse Field Required'); W3B.focus('CmbWarehouseName'); return false; }
        return true;
    }
    function selectedUom() {
        var id = W3B.int($('cmbUOM').value); if (!id) return null;
        var itemId = W3B.int($('cmbItemBOM').value);
        var u = uomsOf(itemId).filter(function (x) { return x.Id === id; })[0];
        return u ? { Id: id, Eq: n(u.Equivalent), Code: u.UOMCode } : null;
    }
    function itemLeave() {                                           // cmbItem_Leave -> AttributeVarientFill + bindRateUomAndItemPackUom
        var id = W3B.int($('cmbItemBOM').value);
        var prevUom = W3B.selText('cmbUOM');
        var rows = uomsOf(id).map(function (u) { return { Id: u.Id, Name: u.UOMCode }; });
        W3B.fill('cmbUOM', rows, true, '');
        rows.forEach(function (r) { if (r.Name === prevUom) $('cmbUOM').value = String(r.Id); });
        if (!id) { W3B.fill('CmbAttribute', [], true, ''); $('txtVarientEquivalent').value = ''; return Promise.resolve(); }
        return attributesOf(id).then(function (a) { W3B.fill('CmbAttribute', a, true, ''); attributeLeave(); }).catch(W3B.fail);
    }
    function attributeLeave() {                                      // CmbAttribute_Leave:942
        var id = W3B.int($('CmbAttribute').value), eq = 0;
        if (id > 0) {
            var a = (attrCache[W3B.int($('cmbItemBOM').value)] || []).filter(function (x) { return x.Id === id; })[0];
            eq = a ? n(a.Equivalent) : 0;
        }
        $('txtVarientEquivalent').value = eq;
    }
    function resetDetailOutput() {                                   // ResetDetailOutPut
        updateIdx = -1;
        W3B.clearCombo('cmbItemBOM'); W3B.fill('cmbUOM', [], true, ''); $('txtQty').value = ''; W3B.fill('CmbAttribute', [], true, '');
        $('txtVarientEquivalent').value = ''; W3B.clearCombo('CmbWarehouseName');
        W3B.show('BtnAddDetail', true); W3B.show('BtnUpdateDetail', false); W3B.show('BtnCancelDetail', false);
        render();
    }
    function addDetail() {                                           // BtnAddDetail_Click:1581
        if (!validateOutputEntry()) return;
        var item = W3B.int($('cmbItemBOM').value), wh = W3B.int($('CmbWarehouseName').value);
        for (var i = 0; i < output.length; i++) {
            if (output[i].ItemId === item && output[i].WarehouseId === wh) { alertMsg('Duplicate Record not added to the grid. An item with the same BOM No and WareHouse already exists'); return; }
        }
        var u = selectedUom();
        if (!u) { alertMsg('Object reference not set to an instance of an object.'); return; }    // SelectedRow is null on the desktop
        if (output.length === 1 && output[0].ItemId === 0) output = [];
        var qty = n($('txtQty').value), eq = n($('txtVarientEquivalent').value);
        output.push({ Id: 0, WarehouseId: wh, WarehouseName: W3B.selText('CmbWarehouseName'), ItemId: item, ItemName: W3B.selText('cmbItemBOM'),
            ItemAttributeVarientId: W3B.int($('CmbAttribute').value), AttributeVarient: W3B.selText('CmbAttribute'), VarientEquivalent: eq,
            ItemUomId: u.Id, Uom: u.Eq, UomCode: u.Code, ItemQty: qty, VarientQty: qty * eq, NetWeight: qty * u.Eq, Rate: 0, RateUomId: u.Id, RateUom: u.Eq, RateUomCode: u.Code, Amount: 0 });
        resetDetailOutput(); calculation();
    }
    function updateDetail() {                                        // BtnUpdateDetail_Click:1659
        if (!validateOutputEntry()) return;
        var item = W3B.int($('cmbItemBOM').value), wh = W3B.int($('CmbWarehouseName').value);
        for (var i = 0; i < output.length; i++) {
            if (output[i].ItemId === item && output[i].WarehouseId === wh && i !== updateIdx) { alertMsg('Duplicate Record not added to the grid. An item with the same BOM No and WareHouse already exists'); return; }
        }
        var u = selectedUom();
        if (!u) { alertMsg('Object reference not set to an instance of an object.'); return; }
        var r = output[updateIdx]; if (!r) return;
        var qty = n($('txtQty').value), eq = n($('txtVarientEquivalent').value);
        r.WarehouseId = wh; r.WarehouseName = W3B.selText('CmbWarehouseName'); r.ItemId = item; r.ItemName = W3B.selText('cmbItemBOM');
        r.ItemAttributeVarientId = W3B.int($('CmbAttribute').value); r.AttributeVarient = W3B.selText('CmbAttribute'); r.VarientEquivalent = eq;
        r.ItemUomId = u.Id; r.Uom = u.Eq; r.UomCode = u.Code; r.ItemQty = qty; r.VarientQty = qty * eq; r.NetWeight = qty * u.Eq;
        resetDetailOutput(); calculation();
    }
    function cancelDetail() { resetDetailOutput(); }
    function editOutputRow(i) {                                      // grdOutPutByProduct_DoubleClick:1556
        var r = output[i]; if (!r || !r.ItemId) return;
        updateIdx = i;
        W3B.setCombo('cmbItemBOM', r.ItemId);
        itemLeave().then(function () {
            W3B.setCombo('cmbUOM', r.ItemUomId); W3B.setCombo('CmbAttribute', r.ItemAttributeVarientId); attributeLeave();
            W3B.setCombo('CmbWarehouseName', r.WarehouseId);
            $('txtQty').value = r.ItemQty;
            W3B.show('BtnAddDetail', false); W3B.show('BtnUpdateDetail', true); W3B.show('BtnCancelDetail', true);
            render();
        });
    }
    function addOutputRow() {                                        // AddRowInOutPutByProductGrid:1779
        if (!output.length) { output.push(blankOutput()); return; }
        var cur = blankOutput();
        var src = output.filter(function (x) { return x.ItemId !== 0; })[0];
        if (src) {
            cur.WarehouseId = src.WarehouseId; cur.WarehouseName = src.WarehouseName; cur.ItemId = src.ItemId; cur.ItemName = src.ItemName;
            cur.ItemAttributeVarientId = src.ItemAttributeVarientId; cur.AttributeVarient = src.AttributeVarient; cur.VarientEquivalent = src.VarientEquivalent;
            cur.ItemUomId = src.ItemUomId; cur.Uom = src.Uom; cur.UomCode = src.UomCode; cur.ItemQty = src.ItemQty; cur.VarientQty = src.VarientQty;
            cur.NetWeight = src.NetWeight; cur.Rate = src.Rate; cur.RateUomId = src.ItemUomId; cur.RateUom = src.Uom; cur.RateUomCode = src.UomCode;
        }
        output.push(cur);
    }
    function deleteOutput(i) {                                       // DeleteOutputDetailRow:2033
        var r = output[i]; if (!r) return;
        if (updateIdx !== -1) { alertMsg('Please Reset the Detail First...'); return; }
        if (r.Id > 0) {
            if (!W3B.ask('Are you sure to Delete?')) return;
            rmOutput.push({ Id: r.Id, WarehouseId: r.WarehouseId, ItemId: r.ItemId, ItemAttributeVarientId: r.ItemAttributeVarientId, ItemQty: r.ItemQty, VarientQty: r.VarientQty,
                VarientEquivalent: r.VarientEquivalent, Rate: r.Rate, Amount: 0 });
        }
        output.splice(i, 1);
        if (!output.length) output.push(blankOutput());
        render(); calculation();
    }
    function outputQtyChanged(r) {                                   // grdOutPutByProduct_CellUpdated:1898
        var p = Promise.resolve();
        if (r.ItemId === 0) {
            var it = W3B.int($('cmbItemBOM').value);
            if (it) { r.ItemId = it; r.ItemName = W3B.selText('cmbItemBOM'); }
            var u = selectedUom();
            if (u) { r.ItemUomId = u.Id; r.Uom = u.Eq; r.UomCode = u.Code; r.RateUomId = u.Id; r.RateUom = u.Eq; r.RateUomCode = u.Code; }
        }
        if (r.ItemId > 0 && !r.ItemAttributeVarientId) {
            p = attributesOf(r.ItemId).then(function (a) {
                if (a.length) { r.ItemAttributeVarientId = a[0].Id; r.AttributeVarient = a[0].Name; r.VarientEquivalent = n(a[0].Equivalent); }
            });
        }
        return p.then(function () {
            if (r.ItemUomId !== undefined) { r.VarientQty = n(r.ItemQty) * n(r.VarientEquivalent); r.Amount = n(r.Rate) * r.VarientQty; }
            render(); calculation(); syncEnable();
        }).catch(W3B.fail);
    }
    function pickOutput(i, f) {
        var r = output[i]; if (!r) return;
        if (f === 'ItemName') {
            pick('Item', itemsType, function (it) {
                var prev = r.ItemId;
                r.ItemId = it.Id; r.ItemName = it.Name;
                if (it.Id !== prev) { r.ItemUomId = 0; r.Uom = 0; r.UomCode = ''; r.RateUom = 0; r.RateUomId = 0; r.RateUomCode = ''; r.NetWeight = 0; }
                if (!r.ItemAttributeVarientId) {
                    attributesOf(it.Id).then(function (a) {
                        if (a.length) {
                            r.ItemAttributeVarientId = a[0].Id; r.AttributeVarient = a[0].Name; r.VarientEquivalent = n(a[0].Equivalent);
                            r.VarientQty = n(r.ItemQty) * r.VarientEquivalent; r.Amount = n(r.Rate) * r.VarientQty;
                        }
                        render(); calculation();
                    }).catch(W3B.fail);
                } else { render(); calculation(); }
            });
        } else if (f === 'WarehouseName') {
            pick('Warehouse', activeWh, function (w) { r.WarehouseId = w.Id; r.WarehouseName = w.Name; render(); });
        } else if (f === 'AttributeVarient') {
            attributesOf(r.ItemId).then(function (rows) {
                pick('Variant', rows, function (a) {
                    r.ItemAttributeVarientId = a.Id; r.AttributeVarient = a.Name; r.VarientEquivalent = n(a.Equivalent);
                    r.VarientQty = n(r.ItemQty) * r.VarientEquivalent; r.Amount = n(r.Rate) * r.VarientQty;
                    render(); calculation();
                });
            }).catch(W3B.fail);
        }
    }

    // ----------------------------------------------------------------------------- overhead
    function overheadChanged(r, f) {                                 // grdOverHead_CellUpdated:2184
        try {
            if (f === 'Qty' || f === 'Rate') {
                if (r.Qty === '' || r.Rate === '') throw new Error('Qty And Rate Should be greater than zero...');
                r.Amount = n(r.Qty) * n(r.Rate);
            }
            if (f === 'PercentOfTotalInput') {
                var total = tot(overhead, 'PercentOfTotalInput');
                if (total > 100) {
                    var over = total - 100;
                    r.PercentOfTotalInput = n(r.PercentOfTotalInput) - over;
                    throw new Error('TotalPercentage Can not be Greater than 100...');
                }
                var t = tot(input, 'Amount') / 100 * n(r.PercentOfTotalInput);
                r.Amount = t > 0 ? Math.round(t * Math.pow(10, amountDec)) / Math.pow(10, amountDec) : 0;     // MidpointRounding.AwayFromZero
                r.Qty = 0; r.Rate = 0;
            }
            if (f === 'Amount') { r.Qty = 0; r.Rate = 0; }
        } catch (e) { alertMsg(e.message); }
        render(); calculation();
    }
    function pickOverhead(i) {
        var r = overhead[i]; if (!r) return;
        pick('Account', accounts, function (a) { r.OverHeadAccountId = a.Id; r.OverHeadAccountTitle = a.Name; r.OverHeadTypeId = a.Id; render(); });
    }

    // ----------------------------------------------------------------------------- wages
    function wagesRate(r, rows, scheduleId) {
        var f = rows.filter(function (x) { return x.Id === scheduleId; })[0];
        var qty = f ? n(f.Qty) : 0, rate = f ? n(f.Rate) : 0;
        r.Rate = qty > 0 ? rate / qty : 0;
        r.Amount = r.Rate * toIntBank(r.Qty);                          // the desktop truncates the row Qty with Conversion.ToInt (:5060)
    }
    function wagesChanged(r) {                                       // grdContractorWages_CellUpdated:2404
        try {
            if (r.Qty === '' || r.Rate === '') throw new Error('Qty And Rate Should be greater than zero...');
            r.Amount = n(r.Qty) * n(r.Rate);
        } catch (e) { alertMsg(e.message); }
        render(); calculation();
    }
    function pickWages(i, f) {
        var r = wages[i]; if (!r) return;
        if (f === 'OutPutItemName') {                                  // :5191
            if (!output.length) { alertMsg('Out put Grid Record not founds...'); return; }
            var seen = {}, rows = [];
            output.forEach(function (o) { if (!seen[o.ItemId]) { seen[o.ItemId] = 1; rows.push({ Id: o.ItemId, Name: o.ItemName }); } });
            if (rows.length) pick('OutPut Item', rows, function (x) { r.OutPutItemId = x.Id; r.OutPutItemName = x.Name; render(); });
        } else if (f === 'ContractorName') {                           // :5052
            if (!r.OutPutItemId) { alertMsg('Please Select OutPut Item for Wages Entry...'); return; }
            pick('Contractor', contractors, function (c) {
                r.ContractorId = c.Id; r.ContractorName = c.Name;
                W3B.getJson(api + '/service-activities' + W3B.qs({ itemId: r.OutPutItemId, docDate: $('txtdocdate').value, activityId: 0, contractorId: c.Id })).then(function (rows) {
                    if (rows.length) {
                        var found = rows.filter(function (x) { return x.Name === r.ServiceActivity; })[0];
                        r.ContractorWagesRateScheduleId = found ? found.Id : 0;
                        var q = found ? n(found.Qty) : 0, rt = found ? n(found.Rate) : 0;
                        r.Rate = q > 0 ? rt / q : 0;
                        r.Amount = r.Rate * toIntBank(r.Qty);
                    }
                    render(); calculation();
                }).catch(W3B.fail);
            });
        } else if (f === 'ServiceActivity') {                          // :5100
            if (!r.OutPutItemId) { alertMsg('Please Select OutPut Item for Wages Entry...'); return; }
            W3B.getJson(api + '/service-activities' + W3B.qs({ itemId: r.OutPutItemId, docDate: $('txtdocdate').value, activityId: 0, contractorId: r.ContractorId })).then(function (rows) {
                pick('Service Activity', rows, function (a) {
                    r.ContractorWagesRateScheduleId = a.Id; r.ServiceActivity = a.Name;
                    if (a.Id > 0) { r.Rate = n(a.Qty) > 0 ? n(a.Rate) / n(a.Qty) : 0; r.Amount = r.Rate * toIntBank(r.Qty); }
                    else { r.Rate = 0; r.Amount = 0; }
                    render(); calculation();
                });
            }).catch(W3B.fail);
        }
    }
    function updateContractorInWages() {                             // BtnUpdateConctratorInWages_Click:5538
        if (!wages.length) { alertMsg('Wages Grid Record not Found'); return; }
        var contrId = W3B.int($('CmbContractorForUpdateInWagesGrid').value), contrName = contrId ? W3B.selText('CmbContractorForUpdateInWagesGrid') : '';
        var actName = W3B.selText('CmbActivityforContractorUpdateInWagesGrid');
        var work = Promise.resolve();
        try {
            wages.forEach(function (r) {
                if (contrId === 0 && !actName) {
                    if (!r.ContractorId) throw new Error('ContractorName Field is Empty in Wages Grid Row No: 01');
                    contrId = r.ContractorId; contrName = r.ContractorName;
                }
                if (actName && r.ServiceActivity !== actName) return;
                work = work.then(function () {
                    r.ContractorId = contrId === 0 ? r.ContractorId : contrId;
                    r.ContractorName = contrName || r.ContractorName;
                    if (r.ContractorWagesRateScheduleId > 0) {
                        return W3B.getJson(api + '/service-activities' + W3B.qs({ itemId: r.OutPutItemId, docDate: $('txtdocdate').value, activityId: r.ContractorWagesRateScheduleId, contractorId: contrId }))
                            .then(function (rows) {
                                if (rows.length) { wagesRate(r, rows, r.ContractorWagesRateScheduleId); r.Amount = r.Rate * n(r.Qty); }
                                else { r.Rate = 0; r.Amount = 0; }
                            });
                    }
                    r.Rate = 0; r.Amount = 0;
                });
            });
        } catch (e) { alertMsg(e.message); return; }
        work.then(function () {
            if (contrId === 0) { alertMsg('ContractorName Field is Empty'); }
            render(); calculation(); fillWagesFilterCombo();
        }).catch(W3B.fail);
    }
    function fillWagesFilterCombo() {                                // FillWagesGridFilterCombo:2540
        var seen = {}, rows = [];
        wages.forEach(function (r) { if (r.ServiceActivity && !seen[r.ServiceActivity]) { seen[r.ServiceActivity] = 1; rows.push({ Id: r.ContractorWagesRateScheduleId, Name: r.ServiceActivity }); } });
        W3B.fill('CmbActivityforContractorUpdateInWagesGrid', rows, true, '');
    }
    function updateWarehouse(list, label) {                          // BtnUpdateWarehouseInput / Output
        if (!list.length) { alertMsg(label + ' Grid Record not Found'); return; }
        var first = output[0];
        if (!first || !first.WarehouseId) { alertMsg('Warehouse Field is Empty in ' + (label === 'Input' ? 'Input' : 'OutPut') + ' Grid Row No: 01'); return; }
        list.forEach(function (r) { r.WarehouseId = first.WarehouseId; r.WarehouseName = first.WarehouseName; });
        render();
    }
    function clearInput() {                                          // btnClearInput_Click:5717
        if (!W3B.ask('are you sure to reset Input Detail?')) return;
        input = [blankInput()]; render(); calculation();
    }

    // ----------------------------------------------------------------------------- btnGenerateRatesOutput_Click:5232
    function generateRates(userClick) {
        var steps = Promise.resolve();
        function msg(e) { if (userClick) W3B.fail(e); else W3B.fail(e); }
        return steps.then(updateInputCurrentStockAndAvgRate).catch(msg)
            .then(updateWagesRates).catch(msg)
            .then(function () { try { updateRatesOutput(); } catch (e) { msg(e); } })
            .then(function () { render(); calculation(); });
    }
    function updateInputCurrentStockAndAvgRate() {
        if (!input.length) throw new Error('Input Grid Record not found');
        var chain = Promise.resolve();
        input.forEach(function (r) {
            chain = chain.then(function () {
                if (!r.ItemId) { r.BalanceStock = 0; r.Rate = 0; r.Amount = 0; return; }
                return W3B.getJson(api + '/stock' + W3B.qs({ itemId: r.ItemId, warehouseId: r.WarehouseId, varientId: r.ItemAttributeVarientId, itemQty: n(r.ItemQty),
                    docDate: $('txtdocdate').value, recId: recId })).then(function (s) {
                    if (s.found) {
                        r.BalanceStock = n(s.qtyInHand);
                        r.Rate = s.fifo ? n(s.fifoRate) : n(s.avgRate);
                        r.Amount = (n(r.RateUom) > 0 && n(r.Rate) > 0) ? n(r.ItemQty) * n(r.Uom) / n(r.RateUom) * n(r.Rate) : 0;
                    } else { r.BalanceStock = 0; r.Rate = 0; r.Amount = 0; }
                });
            });
        });
        return chain;
    }
    function updateWagesRates() {
        var chain = Promise.resolve();
        wages.forEach(function (r) {
            chain = chain.then(function () {
                if (r.ContractorWagesRateScheduleId > 0) {
                    return W3B.getJson(api + '/service-activities' + W3B.qs({ itemId: r.OutPutItemId, docDate: $('txtdocdate').value,
                        activityId: r.ContractorWagesRateScheduleId, contractorId: r.ContractorId })).then(function (rows) {
                        if (rows.length) wagesRate(r, rows, r.ContractorWagesRateScheduleId); else { r.Rate = 0; r.Amount = 0; }
                    });
                }
                r.Rate = 0; r.Amount = 0;
            });
        });
        return chain;
    }
    function updateRatesOutput() {
        if (output.length === 1 && output[0].ItemId === 0) throw new Error('Output Grid Record not Filled Completely');
        var net = tot(input, 'Amount') + tot(overhead, 'Amount') + tot(wages, 'Amount');
        var totalVq = tot(output, 'VarientQty');
        output.forEach(function (o) {
            var vq = n(o.VarientQty);
            o.Amount = totalVq > 0 ? net / totalVq * vq : 0;
            o.Rate = vq > 0 ? o.Amount / vq : 0;
        });
    }

    // ----------------------------------------------------------------------------- toolbar
    function applyRights() {
        $('btnSave').disabled = !rights.save; $('btnUpdate').disabled = !rights.update; $('btnDelete').disabled = !rights.delete;
        $('btnPrint').disabled = !rights.print; $('btnVoucherReport').disabled = !rights.print;
        var m = [];
        if (!rights.save) m.push('Save'); if (!rights.update) m.push('Update'); if (!rights.delete) m.push('Delete'); if (!rights.print) m.push('Print');
        var nt = $('rightsNote');
        nt.textContent = m.length ? 'Your account does not have the ' + m.join(', ') + ' right on this screen, so those buttons are off.' : '';
        nt.classList.toggle('is-hidden', !m.length);
    }
    function mode() {
        var edit = recId > 0;
        W3B.show('btnSave', !edit); W3B.show('btnUpdate', edit); W3B.show('btnDelete', edit);
    }
    function fromReset() {                                           // FromReset:2563 (conversion no, type, plant, remarks and the date are NOT cleared by the desktop)
        recId = 0; updateIdx = -1; checkWarehouse = true;
        input = [blankInput()]; output = [blankOutput()]; overhead = [blankOverhead()]; wages = [blankWages()]; rmInput = []; rmOutput = [];
        W3B.clearCombo('cmbItemBOM'); W3B.fill('cmbUOM', [], true, ''); $('txtQty').value = '';
        W3B.show('BtnAddDetail', true); W3B.show('BtnUpdateDetail', false); W3B.show('BtnCancelDetail', false);
        ['txtTotalInputs', 'txtOverHead', 'txtWagesAmount', 'txtTotalOutPutAutoRatedAmount', 'txtOutPutAutoRatePerQtyAmount'].forEach(function (id) { $(id).value = ''; });
        mode(); render(); fillWagesFilterCombo();
        return W3B.getJson(api + '/doc-no').then(function (d) { $('txtdocno').value = d.docNo; }).catch(W3B.fail);
    }
    function newClick() { fromReset(); }
    function refresh() {                                             // btnRefresh_Click:2616
        W3B.getJson(api + '/refresh').then(function (d) { applyLookups(d); return loadItemsForType(); }).catch(W3B.fail);
    }

    function formValidation() {                                      // FormValidation:1060
        if (!String($('txtdocno').value).trim() || $('txtdocno').value === '0') { alertMsg('Doc No Field Required'); return false; }
        if (!$('txtProductionNo').value.trim()) { alertMsg('Production No Field Required'); W3B.focus('txtProductionNo'); return false; }
        if (!W3B.int($('CmbProductionType').value)) { alertMsg('Production Type Feild Required'); W3B.focus('CmbProductionType'); return false; }
        if (!W3B.int($('CmbPlantName').value)) { alertMsg('Plant Name Feild Required'); W3B.focus('CmbPlantName'); return false; }
        return true;
    }
    var saving = false;
    function save() {                                                // Insert():2776
        if (saving) return;
        if (!formValidation()) return;
        if (!input.length || !output.length) { alertMsg('Grid Record Not Found...\nGrid (Input and OutPut) Should have atleast one record'); return; }
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        saving = true;
        generateRates(false).then(function () {
            var net = calculation(), outTotal = tot(output, 'Amount');
            if (r2(net) !== r2(outTotal) && Math.abs(r2(net) - r2(outTotal)) > 0.0051) {
                throw new Error('Net Vale ' + fmt2(net) + ' In Header and Total OutPut Amount ' + fmt2(outTotal) + ' are not equal');
            }
            return allocationPrompts();
        }).then(function (go) {
            if (!go) { saving = false; return null; }
            return W3B.postJson(api + '/save', body()).then(function (d) {
                saving = false;
                alertMsg(d.message);
                var savedId = d.id, print = rights.print;
                var slip = $('ChkPreviewSlip').checked, vch = $('chkPrintVoucher').checked;
                return fromReset().then(function () {
                    if (slip && print) printSlipFor(savedId);
                    if (vch && print) printVoucherFor(savedId);
                });
            });
        }).catch(function (e) { saving = false; W3B.fail(e); });
    }
    function allocationPrompts() {                                   // Insert:2860 - the Yes / No / Cancel prompt per input row
        var plant = W3B.int($('CmbPlantName').value), chain = Promise.resolve(true);
        input.forEach(function (r, idx) {
            chain = chain.then(function (go) {
                if (!go || !r.ItemId || !r.WarehouseId || !checkWarehouse) return go;
                return W3B.getJson(api + '/warehouse-allocated?warehouseId=' + r.WarehouseId + '&plantId=' + plant).then(function (a) {
                    if (a.allocated) return true;
                    return yesNoCancel('Warehouse = ' + r.WarehouseName + ' is Not Allocated to Selected Plant ' + W3B.selText('CmbPlantName') + ' In Input Grid row no : ' + (idx + 1) + '\n' +
                        "To Allow this Warehouse Select 'Yes'\nTo Not Allow this Select 'No'\nTo Cancel this Validation for Every Entry onwards In Input Grid Select 'Cancel'?").then(function (v) {
                        if (v === 'no') { checkWarehouse = true; return false; }
                        if (v === 'cancel') checkWarehouse = false;
                        return true;
                    });
                });
            });
        });
        return chain;
    }
    function body() {
        return { id: recId, docDate: $('txtdocdate').value, docNo: $('txtdocno').value, conversionNo: $('txtProductionNo').value,
            plantId: W3B.int($('CmbPlantName').value), conversionTypeId: W3B.int($('CmbProductionType').value), remarksHeader: $('txtRemarksHeader').value,
            input: input, output: output, overhead: overhead, wages: wages, removedInput: rmInput, removedOutput: rmOutput };
    }
    function del() {                                                 // btnDelete_Click:3267
        if (recId <= 0) { alertMsg('No record found to Delete'); return; }
        if (!W3B.ask('Are you sure to Delete?')) return;
        W3B.postForm(api + '/delete?id=' + recId).then(function (d) { alertMsg(d.message); return fromReset(); }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- print
    function printSlipFor(id) {                                      // CommonServices.ConversionSlipConcrete(id)
        if (!id) return;
        window.open('/reports/print/1867-conversion-slip' + W3B.qs({ branchesId: look.branchId, documentTypeId: 1867, id: id }), '_blank');
    }
    function printSlip() { printSlipFor(recId); }
    function printVoucherFor(id) {                                   // VoucherReprot:3330
        if (!id) return;
        var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        W3B.getJson(api + '/voucher-head?id=' + id).then(function (d) {
            var vh = W3B.int(d.voucherHeadId);
            if (!vh) { if (win) window.CrystalPrint.release(win); alertMsg('VoucherId Not Found'); return; }
            return window.CrystalPrint.open('acc-118', { id: vh, documentTypeId: 0 }, null, win);
        }).catch(function (e) { if (win) window.CrystalPrint.release(win); W3B.fail(e); });
    }
    function printVoucher() { printVoucherFor(recId); }

    // ----------------------------------------------------------------------------- ReadById:3155
    function load(id) {
        return W3B.getJson(api + '/read/' + id).then(function (d) {
            return fromReset().then(function () {
                recId = d.id;
                $('txtdocdate').value = d.docDate; $('txtdocno').value = d.docNo;
                W3B.setCombo('CmbProductionType', d.conversionTypeId);
                return loadItemsForType().then(function () {
                    W3B.setCombo('CmbPlantName', d.plantId);
                    return plantLeave();
                }).then(function () {
                    $('txtProductionNo').value = d.conversionNo; $('txtRemarksHeader').value = d.remarksHeader;
                    output = d.output.length ? d.output : [blankOutput()];
                    input = d.input.length ? d.input : [blankInput()];
                    overhead = d.overhead.length ? d.overhead : [blankOverhead()];
                    wages = d.wages.length ? d.wages : [blankWages()];
                    mode(); render(); calculation(); fillWagesFilterCombo(); tab('form');
                });
            });
        }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- history
    function tab(t) {
        $('panelForm').classList.toggle('is-active', t === 'form'); $('panelHist').classList.toggle('is-active', t === 'hist');
        $('tabForm').classList.toggle('is-active', t === 'form'); $('tabHist').classList.toggle('is-active', t === 'hist');
    }
    function dateType() {
        var v = W3B.int($('cmbDateType').value), now = new Date(), f = new Date(now), t = new Date(now);
        if (v === 2) { f.setDate(now.getDate() - 7); }
        else if (v === 3) { f = new Date(now.getFullYear(), now.getMonth(), 1); }
        else if (v === 4) { f = new Date(now.getFullYear(), 0, 1); }
        else if (v === 5) { f = look.financialYearStart ? new Date(look.financialYearStart) : new Date(now.getFullYear(), 0, 1); }
        $('dtpFrom').value = W3B.iso(f); $('dtpTo').value = W3B.iso(t);
    }
    function history() {                                             // btnshow_Click / FormHistory
        var q = { fromDocNo: W3B.int($('txtFromDocNo').value), toDocNo: W3B.int($('txtToDocNo').value), conversionId: W3B.int($('cmbHistConversionNo').value) };
        if ($('chkUseDates').checked) { q.fromDate = $('dtpFrom').value; q.toDate = $('dtpTo').value; }
        W3B.getJson(api + '/history' + W3B.qs(q)).then(function (rows) {
            hist = rows;
            $('DatagridHistory').innerHTML = W3B.tableHtml([
                { h: 'Print', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-hact="print" data-hi="' + i + '">Print</button>'; } },
                { h: 'Voucher', html: true, cls: 'w3-btn', w: 50, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-hact="voucher" data-hi="' + i + '">Voucher</button>'; } },
                { h: 'Edit', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-hact="edit" data-hi="' + i + '">Edit</button>'; } },
                { h: 'RecordNo', k: 'RecordNo', cls: 'num' }, { h: 'DocNo', k: 'DocNo' }, { h: 'DocDate', f: function (r) { return W3B.fmtDMY(r.DocDate); } },
                { h: 'ConversionNo', k: 'ConversionNo' }, { h: 'PlantName', k: 'PlantName' }, { h: 'Remarks', k: 'RemarksHeader' },
                { h: 'EntryDate', f: function (r) { return W3B.fmtStamp(r.EntryDate); } }, { h: 'EntryUser', k: 'EntryUserName' },
                { h: 'ModifyDate', f: function (r) { return W3B.fmtStamp(r.ModifyDate); } }, { h: 'ModifyUser', k: 'ModifyUserName' },
                { h: 'IsApproved', bool: true, k: 'IsApproved', cls: 'ctr' }, { h: 'ApprovedDate', f: function (r) { return W3B.fmtStamp(r.ApprovedDate); } },
                { h: 'Attachments', k: 'NoOfAttachments', cls: 'num' }
            ], rows, { empty: 'No records' });
        }).catch(W3B.fail);
    }
    function historyDetail(id) {                                     // DatagridHistoryDetailbind:3742
        W3B.getJson(api + '/read/' + id).then(function (d) {
            $('grdHistoryInput').innerHTML = W3B.tableHtml([
                { h: 'WarehouseName', k: 'WarehouseName' }, { h: 'ItemName', k: 'ItemName' }, { h: 'Uom', k: 'UomCode' }, { h: 'AttributeVariant', k: 'AttributeVarient' },
                { h: 'VariantUnit', k: 'VarientEquivalent', cls: 'num' }, { h: 'ItemQty', k: 'ItemQty', cls: 'num' }, { h: 'VariantQty', k: 'VarientQty', cls: 'num' },
                { h: 'Rate', k: 'Rate', cls: 'num' }, { h: 'Amount', k: 'Amount', cls: 'num' }], d.input, { empty: 'No rows' });
            $('grdHistoryOutput').innerHTML = W3B.tableHtml([
                { h: 'WarehouseName', k: 'WarehouseName' }, { h: 'ItemName', k: 'ItemName' }, { h: 'AttributeVariant', k: 'AttributeVarient' },
                { h: 'VariantUnit', k: 'VarientEquivalent', cls: 'num' }, { h: 'Uom', k: 'UomCode' }, { h: 'ItemQty', k: 'ItemQty', cls: 'num' },
                { h: 'VariantQty', k: 'VarientQty', cls: 'num' }, { h: 'Rate', k: 'Rate', cls: 'num' }, { h: 'Amount', k: 'Amount', cls: 'num' }], d.output, { empty: 'No rows' });
            $('grdHistoryOverHead').innerHTML = W3B.tableHtml([
                { h: 'OverHeadType', k: 'OverHeadTypeId' }, { h: 'OverHeadAccount', k: 'OverHeadAccountTitle' }, { h: 'Percentage', k: 'PercentOfTotalInput', cls: 'num' },
                { h: 'Qty', k: 'Qty', cls: 'num' }, { h: 'Rate', k: 'Rate', cls: 'num' }, { h: 'Amount', k: 'Amount', cls: 'num' }, { h: 'OhRemarks', k: 'OhRemarks' }],
                d.overhead, { empty: 'No rows' });
            $('grdWagesHistory').innerHTML = W3B.tableHtml([
                { h: 'ContractorName', k: 'ContractorName' }, { h: 'ItemName', k: 'OutPutItemName' }, { h: 'ServiceActivity', k: 'ServiceActivity' },
                { h: 'Qty', k: 'Qty', cls: 'num' }, { h: 'Rate', k: 'Rate', cls: 'num' }, { h: 'Amount', k: 'Amount', cls: 'num' }, { h: 'Remarks', k: 'Remarks' }],
                d.wages, { empty: 'No rows' });
        }).catch(W3B.fail);
    }
    function editFromHistory(i) {                                    // DatagridHistory_KeyDown:5121
        if (!rights.update) { alertMsg("you don't have update rights..."); return; }
        var r = hist[i];
        if (W3B.int(r.ReleaseNo) > 0) { alertMsg("You can't update record because this document is referred in release"); return; }
        load(r.Id);
    }

    // ----------------------------------------------------------------------------- events
    document.addEventListener('click', function (e) {
        var t = e.target;
        var del_ = t.closest ? t.closest('button[data-act]') : null;
        if (del_) {
            var g = del_.dataset.g, i = +del_.dataset.i;
            if (del_.dataset.act === 'del') {
                if (g === 'input') deleteInput(i);
                else if (g === 'output') deleteOutput(i);
                else if (g === 'overhead') { overhead.splice(i, 1); if (!overhead.length) overhead.push(blankOverhead()); render(); calculation(); }
                else { wages.splice(i, 1); if (!wages.length) wages.push(blankWages()); render(); calculation(); fillWagesFilterCombo(); }
            } else {
                if (g === 'input') { input.push(blankInput()); render(); }
                else if (g === 'output') {
                    if (!W3B.int($('cmbItemBOM').value) && false) { alertMsg('Please Select Item First'); } else { addOutputRow(); render(); calculation(); }
                } else if (g === 'overhead') { overhead.push(blankOverhead()); render(); }
                else {
                    var cur = wages[i] || {}, nw = blankWages();
                    if (cur.OutPutItemId) { nw.OutPutItemId = cur.OutPutItemId; nw.OutPutItemName = cur.OutPutItemName; nw.ContractorId = cur.ContractorId; nw.ContractorName = cur.ContractorName; }
                    else {
                        var f = wages.filter(function (x) { return x.OutPutItemId; })[0];
                        if (f) { nw.OutPutItemId = f.OutPutItemId; nw.OutPutItemName = f.OutPutItemName; nw.ContractorId = f.ContractorId; nw.ContractorName = f.ContractorName; }
                    }
                    wages.push(nw); render(); fillWagesFilterCombo();
                }
            }
            return;
        }
        var p = t.closest ? t.closest('button.pk') : null;
        if (p) { doPick(p); return; }
        var h = t.closest ? t.closest('button[data-hact]') : null;
        if (h) {
            var hi = +h.dataset.hi;
            if (h.dataset.hact === 'edit') editFromHistory(hi);
            else if (h.dataset.hact === 'print') printSlipFor(hist[hi].Id);
            else printVoucherFor(hist[hi].Id);
            return;
        }
        var tr = t.closest ? t.closest('#DatagridHistory tbody tr[data-i]') : null;
        if (tr) historyDetail(hist[+tr.dataset.i].Id);
    });
    document.addEventListener('dblclick', function (e) {
        var tr = e.target.closest ? e.target.closest('#grdOutPutByProduct tbody tr[data-i]') : null;
        if (tr && !e.target.closest('input,button')) { editOutputRow(+tr.dataset.i); return; }
        var th = e.target.closest ? e.target.closest('#DatagridHistory tbody tr[data-i]') : null;
        if (th && !e.target.closest('button')) editFromHistory(+th.dataset.i);
    });
    document.addEventListener('keydown', function (e) {
        if (e.key === 'F1') { var p = e.target.closest ? e.target.closest('button.pk') : null; if (p) { e.preventDefault(); doPick(p); } }
    });
    function doPick(b) {
        var g = b.dataset.pk, i = +b.dataset.i, f = b.dataset.f;
        if (g === 'input') pickInput(i, f);
        else if (g === 'output') pickOutput(i, f);
        else if (g === 'overhead') pickOverhead(i);
        else pickWages(i, f);
    }
    document.addEventListener('input', function (e) {
        var t = e.target; if (!t.dataset || !t.dataset.g || !t.dataset.f) return;
        var row = arr(t.dataset.g)[+t.dataset.i]; if (!row) return;
        row[t.dataset.f] = t.classList.contains('txt') ? t.value : (t.value === '' ? '' : n(t.value));
    });
    document.addEventListener('change', function (e) {
        var t = e.target; if (!t.dataset || !t.dataset.g || !t.dataset.f) return;
        var g = t.dataset.g, f = t.dataset.f, row = arr(g)[+t.dataset.i]; if (!row) return;
        if (g === 'input' && f === 'ItemQty') { row.ItemQty = n(row.ItemQty); inputQtyChanged(row).catch(W3B.fail); }
        else if (g === 'output' && f === 'ItemQty') { row.ItemQty = n(row.ItemQty); outputQtyChanged(row); }
        else if (g === 'overhead') { if (f !== 'OhRemarks') row[f] = row[f] === '' ? '' : n(row[f]); overheadChanged(row, f); }
        else if (g === 'wages') { row.Qty = n(row.Qty); wagesChanged(row); }
    });
    $('txtdocdate').addEventListener('change', function () { /* the stock reads follow the current date on the next pick */ });

    window.Cv = { newClick: newClick, refresh: refresh, save: save, del: del, printSlip: printSlip, printVoucher: printVoucher, tab: tab, history: history,
        dateType: dateType, productionTypeLeave: productionTypeLeave, plantLeave: plantLeave, itemLeave: itemLeave, attributeLeave: attributeLeave,
        itemSearchMode: itemSearchMode, addDetail: addDetail, updateDetail: updateDetail, cancelDetail: cancelDetail, updateWarehouseInput: function () { updateWarehouse(input, 'Input'); },
        updateWarehouseOutput: function () { updateWarehouse(output, 'OutPut'); }, clearInput: clearInput, updateContractorInWages: updateContractorInWages,
        generateRates: generateRates, pickClose: pickClose };

    W3B.keys({ n: newClick, s: save, u: function () { if (recId > 0) save(); }, r: refresh });

    // ----------------------------------------------------------------------------- boot (frmBillOfMaterial_Manufacturing_Load)
    W3B.getJson(api + '/lookups').then(function (d) {
        look = d; rights = d.rights || {}; fifo = !!d.fifo; amountDec = d.amountDecimals === undefined ? 2 : d.amountDecimals;
        applyRights(); applyLookups(d);
        W3B.fill('cmbDateType', d.dateTypes || [], false);
        W3B.fill('cmbHistConversionNo', d.conversionNos, true);
        if (d.productionTypes && d.productionTypes.length) $('CmbProductionType').value = String(d.productionTypes[0].Id);   // Rows[1].Activate()
        $('txtdocdate').value = W3B.today();
        return W3B.getJson(api + '/items?typeIds=45,46,47').then(function (rows) { itemsAll = rows; return loadItemsForType(); }).then(function () {
            input = [blankInput()]; output = [blankOutput()]; overhead = [blankOverhead()]; wages = [blankWages()];
            $('txtdocno').value = d.docNo; mode(); render(); dateType(); fillWagesFilterCombo();
        });
    }).catch(W3B.fail);
}());
