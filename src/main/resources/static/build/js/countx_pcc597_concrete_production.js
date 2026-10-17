/* ============================================================================================
 * Screen 597 "Concrete Production" - Architecture.WinApp.Pcc.frmConcreteProduction (base.Name "frmConcreteProduction", DocumentTypeId 1850).
 * Desktop line numbers (.cs, recovered_source): load:~480-760, FormValidation, FormValidationOutPutGrid, CmbWorkOrderNo_Leave:1431, FillOutPutGridFromWorkOrder:1479,
 * BtnLoadInputFromBom_Click:1579 / FillInputDetailForMultiBom:1602, AddRowInOutPutByProductGrid:2350, Calculation (~3500), CheckBomHeaderIds:3468,
 * Insert():3500-3700, ReadById:3843, HistoryGridFill:4328, grdInputMaterial_KeyDown (F1):4935, grdOutPutByProduct_KeyDown (F1):5160, grdOverHead_KeyDown:5340,
 * grdContractorWages_KeyDown:5412, btnGenerateRatesOutput_Click:5723, UpdateInputQtyIncaseOfSlabProduction:5753, UpdateWagesQty...:5798, UpdateOHQty...:5838,
 * UpdateInputCurrentStockAndAvgRate:5880, UpdateWagesRates:6000, UpdateRatesOutput:6010, btnUpdateInputWagesOH_Click:6067, BtnUpdateInOhGrid_Click:6107,
 * BtnUpdateWarehouseInput/OutPut:6160/6213, BtnUpdateConctratorInWages_Click:6264.
 * API /production/pcc/concrete-production/api. Not ported: Attachment dialog (DMS), grid layout / SaveAs files, shortcut-keys dialog, Define Plant popup.
 * ============================================================================================ */
(function () {
    'use strict';
    var api = '/production/pcc/concrete-production/api';
    var $ = W3B.$;
    var look = {}, rights = {}, fifo = false, wagesDebit = false, amountDec = 2;
    var recId = 0, updateIdx = -1, prevWorkOrderId = 0;
    var input = [], output = [], overhead = [], wages = [], rmInput = [], rmOutput = [];
    var itemsType = [], bomNos = [], plantWh = [], workOrders = [], hist = [], attrCache = {};

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
    function amtRound(v) {                                           // Math.Round(x, DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero)
        var p = Math.pow(10, amountDec);
        return v < 0 ? -Math.round(-v * p) / p : Math.round(v * p) / p;
    }
    function typeId() { return W3B.int($('CmbProductionType').value); }
    function curWorkOrder() { return W3B.int($('CmbWorkOrderNo').value); }
    function defJob() { var j = (look.jobLots || [])[0]; return j ? { JobLotId: j.Id, JobLot: j.Name } : { JobLotId: 0, JobLot: '' }; }   // GetDefaultJob

    // ----------------------------------------------------------------------------- blank rows (AddRowIn*Grid)
    function blankInput() {
        return { Id: 0, BomHeaderId: 0, BomNo: '', BomInputId: 0, WarehouseId: 0, WarehouseName: '', JobLotId: 0, JobLot: '', ItemId: 0, ItemName: '', ItemUomId: 0, Uom: 0, UomCode: '',
            BalanceStock: 0, BomQty: 0, ItemQty: 0, NetWeight: 0, BomRate: 0, Rate: 0, RateUomId: 0, RateUom: 0, RateUomCode: '', Amount: 0,
            BomMinQty: 0, BomMaxQty: 0, MinQty: 0, MaxQty: 0, MainItemId: 0 };
    }
    function blankOutput() {                                         // Rows.Add(0, 0, "", 0, 0, "", 1, "Auto Rate", ...) :2356
        return { Id: 0, BomHeaderId: 0, BomNo: '', WorkOrderId: 0, WorkOrderDetailId: 0, WorkOrderNo: '', EntryTypeId: 1, EntryType: 'Auto Rate', CustomerTypeId: 0, CustomerType: '',
            CustomerId: 0, CustomerName: '', ReferenceNo: '', SaleOrderId: 0, SaleOrderDetailId: 0, SaleOrder: 0, WarehouseId: 0, WarehouseName: '', JobLotId: 0, JobLot: '',
            ItemId: 0, ItemName: '', ItemAttributeVarientId: 0, AttributeVarient: '', VarientEquivalent: 0, ItemUomId: 0, Uom: 0, UomCode: '', ItemQty: 0, VarientQty: 0, NetWeight: 0,
            Rate: 0, RateUomId: 0, RateUom: 0, RateUomCode: '', Amount: 0 };
    }
    function blankOverhead(bomId, bomNo) {
        return { Id: 0, BomHeaderId: bomId || 0, BomNo: bomNo || '', BomOverHeadId: 0, OverHeadTypeId: 0, OverHeadAccountId: 0, OverHeadAccountTitle: '', PercentOfTotalInput: 0, BomQty: 0,
            Qty: 0, Rate: 0, Amount: 0, OhRemarks: '' };
    }
    function blankWages(bomId, bomNo) {
        return { Id: 0, BomHeaderId: bomId || 0, BomNo: bomNo || '', BomWagesId: 0, OutPutItemId: 0, OutPutItemName: '', ContractorId: 0, ContractorName: '', ContractorWagesRateScheduleId: 0,
            ServiceActivity: '', BomQty: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' };
    }
    function isPlaceholder(r) { return !r.BomHeaderId && !r.ItemId; }

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
        W3B.fill('CmbWIPWarehouse', d.wipWarehouses, true);
        W3B.fill('CmbWIPAccount', d.wipAccounts, true);
        W3B.fill('CmbWagesExpenseAccount', d.wagesAccounts, true);
        W3B.fill('CmbContractorForUpdateInWagesGrid', d.contractors, true);
        bomNos = d.bomNos || [];
        partyMode();
    }
    function uomsOf(itemId) { return (look.allUoms || []).filter(function (u) { return String(u.ItemId) === String(itemId); }); }
    function attributesOf(itemId) {
        if (attrCache[itemId]) return Promise.resolve(attrCache[itemId]);
        return W3B.getJson(api + '/attributes?itemId=' + itemId).then(function (rows) { attrCache[itemId] = rows; return rows; });
    }
    function loadItemsForType() {                                  // ItemdtFillFromBomDbCall:803
        var t = typeId();
        if (!t) { itemsType = []; fillItemCombo(); return Promise.resolve(); }
        return W3B.getJson(api + '/bom-items?typeId=' + t).then(function (rows) { itemsType = rows; fillItemCombo(); });
    }
    function productionTypeLeave() { loadItemsForType().then(syncEnable).catch(W3B.fail); }
    function partyMode() {                                         // RadCustomers / RadRefParty
        var ref = $('RadRefParty').checked;
        W3B.fill('CmbCustomerName', ref ? look.refParties : look.customers, true);
    }

    function plantLeave() {                                        // CmbPlantName_Leave:1362
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

    function syncEnable() {                                        // EnableDisableProductionType / EnableDisablePlant / EnableDisableItemName
        var o = output[0];
        $('CmbProductionType').disabled = !!(o && o.ItemId > 0 && n(o.ItemQty) > 0);
        $('CmbPlantName').disabled = !!(input.length && input[0].WarehouseId > 0);
        $('cmbItemBOM').disabled = !!(wages.length && wages[0].ContractorWagesRateScheduleId > 0);
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

    function render() {
        $('grdInputMaterial').innerHTML = W3B.tableHtml([
            { h: 'X', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xb('input', i); } },
            { h: 'BomNo', k: 'BomNo', w: 90 },
            { h: 'Warehouse', html: true, w: 150, f: function (r, i) { return pk('input', 'WarehouseName', i, r.WarehouseName); } },
            { h: 'ItemName', html: true, w: 230, f: function (r, i) { return pk('input', 'ItemName', i, r.ItemName); } },
            { h: 'UomCode', k: 'UomCode', w: 70 },
            { h: 'BalanceStock', cls: 'num', f: function (r) { return fmtQ(r.BalanceStock); } },
            { h: 'BomQty', cls: 'num', f: function (r) { return fmtQ(r.BomQty); } },
            { h: 'ItemQty', html: true, cls: 'num', f: function (r, i) { return ed('input', 'ItemQty', i, r.ItemQty); } },
            { h: 'BomRate', cls: 'num', f: function (r) { return W3B.fmt(r.BomRate, look.rateDecimals || 2); } },
            { h: 'Avg Rate', cls: 'num', f: function (r) { return W3B.fmt(r.Rate, look.rateDecimals || 2); } },
            { h: 'Amount', cls: 'num', f: function (r) { return fmt2(r.Amount); } },
            { h: 'BomMinQty', cls: 'num', f: function (r) { return fmtQ(r.BomMinQty); } },
            { h: 'BomMaxQty', cls: 'num', f: function (r) { return fmtQ(r.BomMaxQty); } },
            { h: 'MinQty', cls: 'num', f: function (r) { return fmtQ(r.MinQty); } },
            { h: 'MaxQty', cls: 'num', f: function (r) { return fmtQ(r.MaxQty); } }
        ], input, { empty: 'No input rows' });
        $('grdOutPutByProduct').innerHTML = W3B.tableHtml([
            { h: 'X', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xb('output', i); } },
            { h: '+', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return ab('output', i); } },
            { h: 'BomNo', html: true, w: 90, f: function (r, i) { return r.WorkOrderId > 0 ? pk('output', 'BomNo', i, r.BomNo) : W3B.esc(r.BomNo); } },
            { h: 'WorkOrderNo', k: 'WorkOrderNo' },
            { h: 'CustomerType', k: 'CustomerType' },
            { h: 'CustomerName', k: 'CustomerName', w: 140 },
            { h: 'ReferenceNo', html: true, f: function (r, i) { return ed('output', 'ReferenceNo', i, r.ReferenceNo, 'txt'); } },
            { h: 'Order', html: true, w: 70, f: function (r, i) { return pk('output', 'SaleOrder', i, r.SaleOrder ? String(r.SaleOrder) : ''); } },
            { h: 'WarehouseName', html: true, w: 140, f: function (r, i) { return pk('output', 'WarehouseName', i, r.WarehouseName); } },
            { h: 'ItemName', html: true, w: 200, f: function (r, i) { return pk('output', 'ItemName', i, r.ItemName); } },
            { h: 'Varient', html: true, w: 100, f: function (r, i) { return pk('output', 'AttributeVarient', i, r.AttributeVarient); } },
            { h: 'VarientUnit', cls: 'num', f: function (r) { return fmtQ(r.VarientEquivalent); } },
            { h: 'Qty', html: true, cls: 'num', f: function (r, i) { return ed('output', 'ItemQty', i, r.ItemQty); } },
            { h: 'T.Qty', cls: 'num', f: function (r) { return fmtQ(r.VarientQty); } },
            { h: 'Rate', cls: 'num', f: function (r) { return W3B.fmt(r.Rate, look.rateDecimals || 2); } },
            { h: 'Amount', cls: 'num', f: function (r) { return fmt2(r.Amount); } }
        ], output, { empty: 'No output rows', rowClass: function (r, i) { return i === updateIdx ? 'is-current' : ''; } });
        $('grdOverHead').innerHTML = W3B.tableHtml([
            { h: 'X', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xb('overhead', i); } },
            { h: '+', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return ab('overhead', i); } },
            { h: 'BomNo', html: true, w: 80, f: function (r, i) { return pk('overhead', 'BomNo', i, r.BomNo); } },
            { h: 'OverHeadAccount', html: true, w: 170, f: function (r, i) { return pk('overhead', 'OverHeadAccountId', i, r.OverHeadAccountTitle); } },
            { h: 'Percentage', html: true, cls: 'num', f: function (r, i) { return ed('overhead', 'PercentOfTotalInput', i, r.PercentOfTotalInput); } },
            { h: 'BomQty', cls: 'num', f: function (r) { return fmtQ(r.BomQty); } },
            { h: 'Qty', html: true, cls: 'num', f: function (r, i) { return ed('overhead', 'Qty', i, r.Qty); } },
            { h: 'Rate', html: true, cls: 'num', f: function (r, i) { return ed('overhead', 'Rate', i, r.Rate); } },
            { h: 'Amount', html: true, cls: 'num', f: function (r, i) { return ed('overhead', 'Amount', i, r.Amount); } },
            { h: 'OhRemarks', html: true, f: function (r, i) { return ed('overhead', 'OhRemarks', i, r.OhRemarks, 'txt'); } }
        ], overhead, { empty: 'No over head rows' });
        $('grdContractorWages').innerHTML = W3B.tableHtml([
            { h: 'X', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return xb('wages', i); } },
            { h: '+', html: true, cls: 'w3-btn', w: 22, f: function (r, i) { return ab('wages', i); } },
            { h: 'BomNo', html: true, w: 80, f: function (r, i) { return pk('wages', 'BomNo', i, r.BomNo); } },
            { h: 'OutPutItemName', html: true, w: 170, f: function (r, i) { return pk('wages', 'OutPutItemName', i, r.OutPutItemName); } },
            { h: 'ContractorName', html: true, w: 150, f: function (r, i) { return pk('wages', 'ContractorName', i, r.ContractorName); } },
            { h: 'ServiceActivity', html: true, w: 130, f: function (r, i) { return pk('wages', 'ServiceActivity', i, r.ServiceActivity); } },
            { h: 'BomQty', cls: 'num', f: function (r) { return fmtQ(r.BomQty); } },
            { h: 'Qty', html: true, cls: 'num', f: function (r, i) { return ed('wages', 'Qty', i, r.Qty); } },
            { h: 'Rate', cls: 'num', f: function (r) { return W3B.fmt(r.Rate, look.rateDecimals || 2); } },
            { h: 'Amount', cls: 'num', f: function (r) { return fmt2(r.Amount); } },
            { h: 'Remarks', html: true, f: function (r, i) { return ed('wages', 'Remarks', i, r.Remarks, 'txt'); } }
        ], wages, { empty: 'No wages rows' });
        syncEnable();
    }
    function arr(g) { return g === 'input' ? input : g === 'output' ? output : g === 'overhead' ? overhead : wages; }

    // ----------------------------------------------------------------------------- Calculation()
    function calculation() {
        var ti = tot(input, 'Amount'), to = tot(overhead, 'Amount'), tw = tot(wages, 'Amount');
        var manual = output.filter(function (o) { return o.EntryTypeId !== 1; }).reduce(function (a, o) { return a + n(o.Amount); }, 0);
        $('txtTotalInputs').value = input.length ? fmt2(r2(ti)) : '';
        $('txtOverHead').value = overhead.length ? fmt2(r2(to)) : '';
        $('txtWagesAmount').value = wages.length ? fmt2(r2(tw)) : '';
        var net = ti + to + tw - manual;
        $('txtTotalOutPutAutoRatedAmount').value = fmt2(net);
        var vq = output.filter(function (o) { return o.EntryTypeId === 1; }).reduce(function (a, o) { return a + n(o.VarientQty); }, 0);
        $('txtQtyTotal').value = fmtQ(vq);
        $('txtOutPutAutoRatePerQtyAmount').value = vq ? fmt2(r2(net / vq)) : fmt2(0);
        return net;
    }

    // ----------------------------------------------------------------------------- stock / rate (GetStockInHandAndAvgRateFromEvaluationConcrete + FIFO)
    function stockOf(r) {
        return W3B.getJson(api + '/stock' + W3B.qs({ itemId: r.ItemId, warehouseId: r.WarehouseId, jobLotId: r.JobLotId, itemQty: n(r.ItemQty), netWeight: n(r.NetWeight),
            docDate: $('txtdocdate').value, recId: recId }));
    }
    function inputAmount(r) {
        r.Amount = (n(r.RateUom) > 0 && n(r.Rate) > 0) ? n(r.ItemQty) * n(r.Uom) / n(r.RateUom) * n(r.Rate) : 0;
    }
    function stockRefresh(r) {                                       // :5000-5040 / UpdateInputCurrentStockAndAvgRate:5880
        return stockOf(r).then(function (s) {
            if (s.found) { r.BalanceStock = n(s.balanceStock); r.Rate = n(s.rate); inputAmount(r); }
            else { r.BalanceStock = 0; r.Rate = 0; r.Amount = 0; }
        });
    }
    function afterInputPick(r) {
        return stockRefresh(r).then(function () { render(); calculation(); }).catch(function (e) { render(); calculation(); W3B.fail(e); });
    }
    function inputQtyChanged(r) {                                    // grdInputMaterial_CellUpdated:2067
        function amount() { r.Amount = (n(r.Rate) !== 0 && r.RateUomId && n(r.RateUom) !== 0) ? n(r.ItemQty) * n(r.Uom) / n(r.RateUom) * n(r.Rate) : 0; }
        if (n(r.ItemQty) !== 0 && r.ItemUomId) {
            r.NetWeight = n(r.ItemQty) * n(r.Uom);
            if (fifo && r.ItemId) {
                return stockOf(r).then(function (s) { r.Rate = n(s.rate); amount(); render(); calculation(); });
            }
        } else if (n(r.ItemQty) === 0) { r.NetWeight = 0; }
        amount(); render(); calculation();
        return Promise.resolve();
    }

    function pickInput(i, f) {
        var r = input[i]; if (!r) return;
        if (f === 'ItemName') {                                       // :4946 - alternative items of the BOM input
            W3B.getJson(api + '/alternative-items' + W3B.qs({ itemId: r.MainItemId, bomHeaderId: r.BomHeaderId })).then(function (rows) {
                if (!rows.length) { alertMsg('No Alternative Item found Against this Item ' + r.ItemName); return; }
                pick('Alternative Item', rows.map(function (x) { return { Id: x.ItemId, Name: x.ItemName, raw: x }; }), function (p) {
                    var x = p.raw, j = defJob();
                    r.ItemId = p.Id; r.ItemName = p.Name;
                    r.ItemUomId = n(x.ItemUomId); r.UomCode = x.ItemUom || ''; r.Uom = n(x.ItemUomEquivalent);
                    r.BomQty = n(x.ItemQty); r.BomMinQty = n(x.MinQty); r.BomMaxQty = n(x.MaxQty);
                    r.ItemQty = n(x.ItemQty); r.MinQty = n(x.MinQty); r.MaxQty = n(x.MaxQty); r.NetWeight = n(x.WeightKg);
                    r.JobLotId = j.JobLotId; r.JobLot = j.JobLot;
                    afterInputPick(r);
                });
            }).catch(W3B.fail);
        } else if (f === 'WarehouseName') {                           // :5043
            var plant = W3B.int($('CmbPlantName').value);
            W3B.getJson(api + '/plant-warehouses?plantId=' + plant).then(function (rows) {
                plantWh = rows;
                pick('Warehouse', rows, function (w) {
                    var j = defJob();
                    r.WarehouseId = w.Id; r.WarehouseName = w.Name; r.JobLotId = j.JobLotId; r.JobLot = j.JobLot;
                    afterInputPick(r);
                });
            }).catch(W3B.fail);
        }
    }

    function deleteInput(i) {                                        // DeleteInputDetailRow
        var r = input[i]; if (!r) return;
        if (r.Id > 0) {
            if (!W3B.ask('Are you sure to Delete?')) return;
            rmInput.push(r);
        }
        input.splice(i, 1);
        render(); calculation();
    }

    // ----------------------------------------------------------------------------- output grid + entry panel
    function validateOutputEntry() {                                 // FormValidationOutPutGrid
        if (!W3B.int($('cmbItemBOM').value)) { alertMsg('ItemBOM Feild Required'); W3B.focus('cmbItemBOM'); return false; }
        if (!W3B.int($('cmbBOMNo').value)) { alertMsg('BOM No Feild Required'); W3B.focus('cmbBOMNo'); return false; }
        if (!n($('txtQty').value)) { alertMsg('Qty Field Required'); W3B.focus('txtQty'); return false; }
        if (!W3B.int($('CmbAttribute').value)) { alertMsg('Item Attribute Field Required'); W3B.focus('CmbAttribute'); return false; }
        if (!n($('txtVarientEquivalent').value)) { alertMsg('Varient Equivalent Field Required'); return false; }
        if (!W3B.int($('CmbWarehouseName').value)) { alertMsg('Warehouse Field Required'); W3B.focus('CmbWarehouseName'); return false; }
        return true;
    }
    function selectedBom() {
        var id = W3B.int($('cmbBOMNo').value);
        return bomNos.filter(function (b) { return b.Id === id; })[0] || null;
    }
    function itemLeave() {                                           // cmbItem_Leave -> BOMNobyItemDbCall + AttributeVarientFill
        var id = W3B.int($('cmbItemBOM').value);
        var rows = bomNos.filter(function (b) { return b.ItemId === id; }).map(function (b) { return { Id: b.Id, Name: b.Name }; });
        W3B.fill('cmbBOMNo', rows, true, '');
        W3B.fill('cmbUOM', [], true, ''); $('txtQty').value = '';
        if (rows.length) { $('cmbBOMNo').value = String(rows[0].Id); bomLeave(); }
        if (!id) { W3B.fill('CmbAttribute', [], true, ''); $('txtVarientEquivalent').value = ''; return Promise.resolve(); }
        return attributesOf(id).then(function (a) { W3B.fill('CmbAttribute', a, true, ''); attributeLeave(); }).catch(W3B.fail);
    }
    function bomLeave() {                                            // the BOM's pack UOM + batch qty
        var b = selectedBom();
        if (!b) { W3B.fill('cmbUOM', [], true, ''); $('txtQty').value = ''; return; }
        W3B.fill('cmbUOM', [{ Id: b.ItemUomId, Name: b.PackUom }], false, String(b.ItemUomId));
        $('txtQty').value = b.BomBatchQty;
    }
    function attributeLeave() {                                      // CmbAttribute_Leave
        var id = W3B.int($('CmbAttribute').value), eq = 0;
        if (id > 0) {
            var a = (attrCache[W3B.int($('cmbItemBOM').value)] || []).filter(function (x) { return x.Id === id; })[0];
            eq = a ? n(a.Equivalent) : 0;
        }
        $('txtVarientEquivalent').value = eq;
    }
    function resetDetailOutput() {                                   // ResetDetailOutPut
        updateIdx = -1;
        W3B.clearCombo('CmbCustomerName'); $('txtReferenceNoDetail').value = '';
        W3B.clearCombo('cmbItemBOM'); W3B.fill('cmbBOMNo', [], true, ''); W3B.fill('cmbUOM', [], true, ''); $('txtQty').value = '';
        W3B.fill('CmbAttribute', [], true, ''); $('txtVarientEquivalent').value = ''; W3B.clearCombo('CmbWarehouseName');
        W3B.show('BtnAddDetail', true); W3B.show('BtnUpdateDetail', false); W3B.show('BtnCancelDetail', false);
        render();
    }
    function panelValues() {
        var b = selectedBom(), partyType = $('RadRefParty').checked ? 2 : 1, cust = W3B.int($('CmbCustomerName').value);
        return {
            BomHeaderId: b ? b.Id : 0, BomNo: b ? b.Name : '',
            CustomerTypeId: partyType, CustomerType: partyType === 2 ? 'Ref Party' : 'Customer', CustomerId: cust, CustomerName: W3B.selText('CmbCustomerName'),
            ReferenceNo: $('txtReferenceNoDetail').value,
            WarehouseId: W3B.int($('CmbWarehouseName').value), WarehouseName: W3B.selText('CmbWarehouseName'),
            ItemId: W3B.int($('cmbItemBOM').value), ItemName: W3B.selText('cmbItemBOM'),
            ItemAttributeVarientId: W3B.int($('CmbAttribute').value), AttributeVarient: W3B.selText('CmbAttribute'), VarientEquivalent: n($('txtVarientEquivalent').value),
            ItemUomId: b ? b.ItemUomId : 0, Uom: b ? n(b.PackUomEquivalent) : 0, UomCode: b ? b.PackUom : ''
        };
    }
    function isDuplicate(v, skip) {
        for (var i = 0; i < output.length; i++) {
            var o = output[i];
            if (i !== skip && o.CustomerId === v.CustomerId && String(o.ReferenceNo) === String(v.ReferenceNo) && o.BomHeaderId === v.BomHeaderId && o.ItemId === v.ItemId && o.WarehouseId === v.WarehouseId) return true;
        }
        return false;
    }
    function addDetail() {                                           // BtnAddDetail_Click
        if (!validateOutputEntry()) return;
        var v = panelValues();
        if (isDuplicate(v, -1)) { alertMsg('Duplicate record not added. Same Customer,ReferenceNo, BOM, Item and Warehouse already exists.'); return; }
        if (output.length === 1 && isPlaceholder(output[0])) output = [];
        var j = defJob(), row = blankOutput();
        Object.keys(v).forEach(function (k) { row[k] = v[k]; });
        row.JobLotId = j.JobLotId; row.JobLot = j.JobLot;
        row.RateUomId = v.ItemUomId; row.RateUom = v.Uom; row.RateUomCode = v.UomCode;
        output.push(row);
        resetDetailOutput(); calculation(); syncEnable();
    }
    function updateDetail() {                                        // BtnUpdateDetail_Click
        if (!validateOutputEntry()) return;
        var v = panelValues();
        if (isDuplicate(v, updateIdx)) { alertMsg('Duplicate record not added. Same Customer,ReferenceNo,BOM, Item and Warehouse already exists.'); return; }
        var r = output[updateIdx]; if (!r) return;
        Object.keys(v).forEach(function (k) { r[k] = v[k]; });
        r.RateUomId = v.ItemUomId; r.RateUom = v.Uom; r.RateUomCode = v.UomCode;
        r.VarientQty = n(r.ItemQty) * n(r.VarientEquivalent); r.Amount = n(r.Rate) * r.VarientQty;       // ItemQty itself is unchanged
        resetDetailOutput(); calculation();
    }
    function cancelDetail() { resetDetailOutput(); }
    function editOutputRow(i) {                                      // grdOutPutByProduct_DoubleClick
        var r = output[i]; if (!r || !r.ItemId) return;
        if (r.WorkOrderId > 0) { alertMsg('Manual update is not allowed when a Work Order is present.'); return; }
        updateIdx = i;
        $(r.CustomerTypeId === 2 ? 'RadRefParty' : 'RadCustomers').checked = true; partyMode();
        W3B.setCombo('CmbCustomerName', r.CustomerId); $('txtReferenceNoDetail').value = r.ReferenceNo;
        W3B.setCombo('cmbItemBOM', r.ItemId);
        itemLeave().then(function () {
            W3B.setCombo('cmbBOMNo', r.BomHeaderId); bomLeave();
            W3B.setCombo('CmbAttribute', r.ItemAttributeVarientId); attributeLeave();
            W3B.setCombo('CmbWarehouseName', r.WarehouseId);
            if (n(r.ItemQty) > 0) $('txtQty').value = r.ItemQty;
            W3B.show('BtnAddDetail', false); W3B.show('BtnUpdateDetail', true); W3B.show('BtnCancelDetail', true);
            render();
        });
    }
    function addOutputRow() {                                        // AddRowInOutPutByProductGrid:2350
        if (!output.length) { output.push(blankOutput()); return; }
        var cur = blankOutput(), src = output.filter(function (x) { return x.BomHeaderId && x.ItemId; })[0];
        if (src) {
            ['BomHeaderId', 'BomNo', 'SaleOrderId', 'SaleOrderDetailId', 'SaleOrder', 'ItemName', 'ItemId', 'CustomerTypeId', 'CustomerType', 'ReferenceNo', 'CustomerId', 'CustomerName',
                'WarehouseId', 'WarehouseName', 'ItemAttributeVarientId', 'AttributeVarient', 'VarientEquivalent', 'ItemUomId', 'Uom', 'UomCode', 'ItemQty', 'VarientQty', 'NetWeight', 'Rate']
                .forEach(function (k) { cur[k] = src[k]; });
            cur.RateUomId = src.ItemUomId; cur.RateUom = src.Uom; cur.RateUomCode = src.UomCode;
        }
        var j = defJob(); cur.JobLotId = j.JobLotId; cur.JobLot = j.JobLot;
        output.push(cur);
    }
    function deleteOutput(i) {                                       // DeleteDetailRow
        var r = output[i]; if (!r) return;
        if (updateIdx !== -1) { alertMsg('Please Reset the Detail First...'); return; }
        if (r.Id > 0) {
            if (!W3B.ask('Are you sure to Delete?')) return;
            rmOutput.push(r);
        }
        output.splice(i, 1);
        if (!output.length) output.push(blankOutput());
        render(); calculation();
    }
    function outputQtyChanged(r) {                                   // grdOutPutByProduct_CellUpdated:2067+
        var p = Promise.resolve();
        if (r.ItemId === 0) {
            var it = W3B.int($('cmbItemBOM').value);
            if (it) { r.ItemId = it; r.ItemName = W3B.selText('cmbItemBOM'); }
            var b = selectedBom();
            if (b) { r.ItemUomId = b.ItemUomId; r.Uom = n(b.PackUomEquivalent); r.UomCode = b.PackUom; r.RateUomId = b.ItemUomId; r.RateUom = n(b.PackUomEquivalent); r.RateUomCode = b.PackUom; }
            if (!r.JobLotId) { var j = defJob(); r.JobLotId = j.JobLotId; r.JobLot = j.JobLot; }
        }
        if (r.ItemId > 0 && !r.ItemAttributeVarientId) {
            p = attributesOf(r.ItemId).then(function (a) {
                if (a.length) { r.ItemAttributeVarientId = a[0].Id; r.AttributeVarient = a[0].Name; r.VarientEquivalent = n(a[0].Equivalent); }
            });
        }
        return p.then(function () {
            r.VarientQty = n(r.ItemQty) * n(r.VarientEquivalent); r.Amount = n(r.Rate) * r.VarientQty;
            render(); calculation(); syncEnable();
        }).catch(W3B.fail);
    }
    function pickOutput(i, f) {
        var r = output[i]; if (!r) return;
        var wo = curWorkOrder();
        if (f === 'ItemName') {                                       // :5176 - only without a work order and for production type 45
            if (wo !== 0 || typeId() !== 45) return;
            pick('Item', itemsType, function (it) {
                var prev = r.ItemId;
                r.ItemId = it.Id; r.ItemName = it.Name;
                if (it.Id !== prev) {          // :5192 - the desktop writes the item id into ItemUomId; the web uses the item's first UOM instead
                    var u = uomsOf(it.Id)[0];
                    r.ItemUomId = u ? u.Id : 0; r.Uom = u ? n(u.Equivalent) : 0; r.UomCode = u ? u.UOMCode : '';
                    r.RateUomId = r.ItemUomId; r.RateUom = r.Uom; r.RateUomCode = r.UomCode; r.NetWeight = 0;
                }
                if (!r.ItemAttributeVarientId) {
                    attributesOf(it.Id).then(function (a) {
                        if (a.length) {
                            r.ItemAttributeVarientId = a[0].Id; r.AttributeVarient = a[0].Name; r.VarientEquivalent = n(a[0].Equivalent);
                            r.VarientQty = n(r.ItemQty) * r.VarientEquivalent; r.Amount = n(r.Rate) * r.VarientQty;
                        }
                        render(); calculation(); syncEnable();
                    }).catch(W3B.fail);
                } else { render(); calculation(); syncEnable(); }
            });
        } else if (f === 'WarehouseName') {                           // :5231
            W3B.getJson(api + '/plant-warehouses?plantId=' + W3B.int($('CmbPlantName').value)).then(function (rows) {
                pick('Warehouse', rows, function (w) { var j = defJob(); r.WarehouseId = w.Id; r.WarehouseName = w.Name; r.JobLotId = j.JobLotId; r.JobLot = j.JobLot; render(); });
            }).catch(W3B.fail);
        } else if (f === 'AttributeVarient') {                        // :5266
            if (wo !== 0) return;
            attributesOf(r.ItemId).then(function (rows) {
                pick('Varient', rows, function (a) {
                    r.ItemAttributeVarientId = a.Id; r.AttributeVarient = a.Name; r.VarientEquivalent = n(a.Equivalent);
                    r.VarientQty = n(r.ItemQty) * r.VarientEquivalent; r.Amount = n(r.Rate) * r.VarientQty;
                    render(); calculation();
                });
            }).catch(W3B.fail);
        } else if (f === 'SaleOrder') {                               // :5285 - dtOrders = SaleOrder.GetSaleOrderNo (all orders)
            if (wo !== 0) return;
            W3B.getJson(api + '/sale-orders').then(function (rows) {
                pick('Sale Order', rows.map(function (x) { return { Id: x.Id, Name: x.DocNo }; }), function (s) { r.SaleOrderId = s.Id; r.SaleOrder = W3B.int(s.Name); render(); });
            }).catch(W3B.fail);
        } else if (f === 'BomNo') {                                   // :5298 - only when a work order is selected; applies to every row of that work order
            if (wo <= 0) return;
            var boms = bomNos.filter(function (b) { return b.ItemId === r.ItemId; }).map(function (b) { return { Id: b.Id, Name: b.Name }; });
            pick('BOM No', boms, function (b) {
                output.forEach(function (o) { if (o.WorkOrderId === r.WorkOrderId) { o.BomHeaderId = b.Id; o.BomNo = b.Name; } });
                render();
            });
        }
    }

    // ----------------------------------------------------------------------------- work order (CmbWorkOrderNo_Leave:1431)
    function loadWorkOrders(detailIds) {                             // WorkOrderDbCall:1079
        return W3B.getJson(api + '/work-orders' + W3B.qs({ recId: recId, detailIds: detailIds || '' })).then(function (rows) {
            workOrders = rows;
            var keep = $('CmbWorkOrderNo').value, seen = {}, list = [];
            rows.forEach(function (r) {
                var id = W3B.int(r.WorkOrderHeaderId);
                if (id && !seen[id]) { seen[id] = 1; list.push({ Id: id, Name: r.WorkOrderNo }); }
            });
            W3B.fill('CmbWorkOrderNo', list, true);
            if (keep) W3B.setCombo('CmbWorkOrderNo', keep);
        });
    }
    function removeWorkOrderRows() {
        output = output.filter(function (r) {
            if (r.WorkOrderDetailId > 0) { if (r.Id > 0) rmOutput.push(r); return false; }
            return true;
        });
    }
    function fillOutputFromWorkOrder(id) {                           // FillOutPutGridFromWorkOrder:1479
        var ids = rmOutput.map(function (r) { return r.WorkOrderDetailId; }).filter(function (v, k, a) { return v > 0 && a.indexOf(v) === k; }).join(',');
        var pre = rmOutput.length ? loadWorkOrders(ids) : Promise.resolve();
        return pre.then(function () {
            var rows = workOrders.filter(function (r) { return W3B.int(r.WorkOrderHeaderId) === id; });
            if (!rows.length) { if (!output.length) output.push(blankOutput()); return; }
            var plant = W3B.int(rows[0].PlantId);
            var p = Promise.resolve();
            if (plant > 0) { W3B.setCombo('CmbPlantName', plant); p = plantLeave(); }
            return p.then(function () {
                var j = defJob(), have = {};
                output.forEach(function (o) { if (o.WorkOrderDetailId > 0) have[o.WorkOrderDetailId] = 1; });
                if (output.length === 1 && isPlaceholder(output[0])) output = [];
                rows.forEach(function (dr) {
                    var did = W3B.int(dr.WorkOrderCustomerDetailId);
                    if (have[did]) return;
                    have[did] = 1;
                    var o = blankOutput();
                    o.WorkOrderId = W3B.int(dr.WorkOrderHeaderId); o.WorkOrderDetailId = did; o.WorkOrderNo = dr.WorkOrderNo;
                    o.CustomerTypeId = W3B.int(dr.CustomerTypeId); o.CustomerType = dr.CustomerType || ''; o.CustomerId = W3B.int(dr.CustomerId); o.CustomerName = dr.CustomerName || '';
                    o.ReferenceNo = dr.ReferenceNo || ''; o.SaleOrderId = W3B.int(dr.SaleOrderId); o.SaleOrderDetailId = W3B.int(dr.SaleOrderDetailId); o.SaleOrder = W3B.int(dr.OrderNo);
                    o.JobLotId = j.JobLotId; o.JobLot = j.JobLot;
                    o.ItemId = W3B.int(dr.ItemId); o.ItemName = dr.ItemName || ''; o.ItemAttributeVarientId = W3B.int(dr.ItemAttributeVarientId); o.AttributeVarient = dr.ItemAttribute || '';
                    o.VarientEquivalent = n(dr.VarientEquivalent); o.ItemUomId = W3B.int(dr.ItemUomId); o.Uom = n(dr.ItemUomEquivalent); o.UomCode = dr.ItemUomCode || '';
                    o.ItemQty = n(dr.BalQty); o.VarientQty = n(dr.BalQty) * n(dr.VarientEquivalent); o.NetWeight = n(dr.BalWeight);
                    output.push(o);
                });
            });
        });
    }
    function workOrderLeave() {
        var id = curWorkOrder();
        var count = output.filter(function (r) { return r.ItemId > 0; }).length;
        var p = Promise.resolve();
        if (id === 0 && prevWorkOrderId === 0) {
            removeWorkOrderRows();
            if (!output.length) output.push(blankOutput());
        } else if (id > 0 && prevWorkOrderId === 0) {
            prevWorkOrderId = id; p = fillOutputFromWorkOrder(id);
        } else if (id !== prevWorkOrderId && count > 0) {
            if (!W3B.ask('Are you Sure To change WorkOrder??? Previous Data Of OutPut grid Will be Reset')) { W3B.setCombo('CmbWorkOrderNo', prevWorkOrderId); return; }
            removeWorkOrderRows();
            prevWorkOrderId = id;
            if (id > 0) p = fillOutputFromWorkOrder(id); else if (!output.length) output.push(blankOutput());
        } else if (count === 0) {
            prevWorkOrderId = id;
            if (id > 0) p = fillOutputFromWorkOrder(id);
        }
        p.then(function () { render(); calculation(); syncEnable(); }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- Load Input From BOM (BtnLoadInputFromBom_Click:1579)
    function loadInputFromBom() {
        var plant = W3B.int($('CmbPlantName').value);
        if (!plant) { W3B.focus('CmbPlantName'); alertMsg('Please Select Plant Name First'); return; }
        if (input.length > 0 && !W3B.ask('Input Grid already have Records.Do you want To Refresh Data?')) return;
        if (!output.length) { alertMsg('OutPut Grid Record Not Found...'); return; }
        if (wages.length === 1 && !wages[0].BomHeaderId && !wages[0].OutPutItemId) wages = [];
        if (overhead.length === 1 && !overhead[0].BomHeaderId && !overhead[0].OverHeadAccountId) overhead = [];
        var chain = Promise.resolve();
        output.forEach(function (o) {
            if (!(o.BomHeaderId > 0)) return;
            chain = chain.then(function () {
                return W3B.getJson(api + '/bom-load' + W3B.qs({ bomId: o.BomHeaderId, plantId: plant, outItemId: o.ItemId, outItemName: o.ItemName })).then(function (d) {
                    (d.input || []).forEach(function (r) {
                        if (!input.some(function (x) { return x.BomInputId === r.BomInputId && x.BomHeaderId === r.BomHeaderId; })) input.push(r);
                    });
                    if (!(d.overhead || []).length) { overhead.push(blankOverhead(o.BomHeaderId, o.BomNo)); }
                    (d.overhead || []).forEach(function (r) {
                        if (!overhead.some(function (x) { return x.BomOverHeadId === r.BomOverHeadId && x.BomHeaderId === r.BomHeaderId; })) overhead.push(r);
                    });
                    if (!(d.wages || []).length) { wages.push(blankWages(o.BomHeaderId, o.BomNo)); }
                    (d.wages || []).forEach(function (r) {
                        if (!wages.some(function (x) { return x.BomWagesId === r.BomWagesId && x.BomHeaderId === r.BomHeaderId; })) wages.push(r);
                    });
                });
            });
        });
        chain.then(updateWagesRates).then(function () {
            if (!overhead.length) overhead = [blankOverhead()];
            if (!wages.length) wages = [blankWages()];
            render(); calculation(); fillWagesFilterCombo();
        }).catch(function (e) { if (!overhead.length) overhead = [blankOverhead()]; if (!wages.length) wages = [blankWages()]; render(); calculation(); W3B.fail(e); });
    }

    // ----------------------------------------------------------------------------- overhead
    function overheadChanged(r, f) {                                 // grdOverHead_CellUpdated:2886
        try {
            if (f === 'Qty' || f === 'Rate') {
                if (r.Qty === '' || r.Rate === '') throw new Error('Qty And Rate Should be greater than zero...');
                r.Amount = n(r.Qty) * n(r.Rate);
            }
            if (f === 'PercentOfTotalInput') {
                var total = overhead.filter(function (x) { return x.BomHeaderId === r.BomHeaderId; }).reduce(function (a, x) { return a + n(x.PercentOfTotalInput); }, 0);
                if (total > 100) {
                    r.PercentOfTotalInput = n(r.PercentOfTotalInput) - (total - 100);
                    throw new Error('TotalPercentage Can not be Greater than 100...For Bom No: ' + r.BomNo);
                }
                var inAmt = input.filter(function (x) { return x.BomHeaderId === r.BomHeaderId; }).reduce(function (a, x) { return a + n(x.Amount); }, 0);
                var t = inAmt / 100 * n(r.PercentOfTotalInput);
                r.Amount = t > 0 ? amtRound(t) : 0;
                r.Qty = 0; r.Rate = 0;
            }
            if (f === 'Amount') { r.Qty = 0; r.Rate = 0; }
        } catch (e) { alertMsg(e.message); }
        render(); calculation();
    }
    function outputBomOptions() {
        var seen = {}, rows = [];
        output.forEach(function (o) { if (o.BomHeaderId > 0 && !seen[o.BomHeaderId]) { seen[o.BomHeaderId] = 1; rows.push({ Id: o.BomHeaderId, Name: o.BomNo }); } });
        return rows;
    }
    function pickOverhead(i, f) {
        var r = overhead[i]; if (!r) return;
        if (f === 'BomNo') {
            pick('BOM No', outputBomOptions(), function (b) { r.BomHeaderId = b.Id; r.BomNo = b.Name; render(); });
        } else {
            pick('Account', look.overheadAccounts || [], function (a) { r.OverHeadAccountId = a.Id; r.OverHeadAccountTitle = a.Name; r.OverHeadTypeId = a.Id; render(); });
        }
    }
    function updateInOhGrid() {                                      // BtnUpdateInOhGrid_Click:6107
        var pct = n($('txtPercentToupdateInOhGrid').value);
        if (pct <= 0) { alertMsg('Percentage must be greater than 0.'); return; }
        var totals = {}, skip = 0;
        input.forEach(function (x) { totals[x.BomHeaderId] = (totals[x.BomHeaderId] || 0) + n(x.Amount); });
        overhead.forEach(function (r) {
            var bom = r.BomHeaderId;
            if (skip > 0 && skip === bom) return;
            var cnt = overhead.filter(function (x) { return x.BomHeaderId === bom; }).length, newTotal = pct * cnt;
            if (newTotal > 100) {
                alertMsg('Total percentage would become ' + newTotal + '% for BOM No: ' + r.BomNo + '.\nThis exceeds the allowed 100% limit.\n\nRows in BOM: ' + cnt + ', Each with ' + pct + '%');
                skip = bom; return;
            }
            r.PercentOfTotalInput = pct;
            var u = (totals[bom] || 0) * pct / 100;
            r.Amount = u > 0 ? amtRound(u) : 0;
            r.Qty = 0; r.Rate = 0;
        });
        render(); calculation();
    }

    // ----------------------------------------------------------------------------- wages
    function wagesRate(r, rows, scheduleId) {
        var f = rows.filter(function (x) { return x.Id === scheduleId; })[0];
        var qty = f ? n(f.Qty) : 0, rate = f ? n(f.Rate) : 0;
        r.Rate = qty > 0 ? rate / qty : 0;
        r.Amount = r.Rate * toIntBank(r.Qty);                          // the desktop truncates the row Qty with Conversion.ToInt (:5430)
    }
    function wagesChanged(r) {                                       // grdContractorWages_CellUpdated:3120
        try {
            if (r.Qty === '' || r.Rate === '') throw new Error('Qty And Rate Should be greater than zero...');
            r.Amount = n(r.Qty) * n(r.Rate);
        } catch (e) { alertMsg(e.message); }
        render(); calculation();
    }
    function outputItemsOf(bomId) {                                  // GetOutPutGridItemsForWages
        var seen = {}, rows = [];
        output.forEach(function (o) { if (o.BomHeaderId === bomId && o.ItemId && !seen[o.ItemId]) { seen[o.ItemId] = 1; rows.push({ Id: o.ItemId, Name: o.ItemName }); } });
        return rows;
    }
    function pickWages(i, f) {
        var r = wages[i]; if (!r) return;
        if (f === 'ContractorName') {                                  // :5465
            if (!r.OutPutItemId) { alertMsg('Please Select OutPut Item for Wages Entry...'); return; }
            pick('Contractor', look.contractors || [], function (c) {
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
        } else if (f === 'ServiceActivity') {                          // :5497
            if (!r.OutPutItemId) { alertMsg('Please Select OutPut Item for Wages Entry...'); return; }
            W3B.getJson(api + '/service-activities' + W3B.qs({ itemId: r.OutPutItemId, docDate: $('txtdocdate').value, activityId: 0, contractorId: r.ContractorId })).then(function (rows) {
                pick('Service Activity', rows, function (a) {
                    r.ContractorWagesRateScheduleId = a.Id; r.ServiceActivity = a.Name;
                    if (a.Id > 0) { r.Rate = n(a.Qty) > 0 ? n(a.Rate) / n(a.Qty) : 0; r.Amount = r.Rate * toIntBank(r.Qty); }
                    else { r.Rate = 0; r.Amount = 0; }
                    render(); calculation();
                });
            }).catch(W3B.fail);
        } else if (f === 'BomNo') {                                    // :5535
            if (!output.length) { alertMsg('Out Put Grid Record not founds...'); return; }
            if (r.BomWagesId !== 0) { alertMsg("You can't Update Bom No of row Against Bill Of Material"); return; }
            var boms = outputBomOptions();
            if (!boms.length) return;
            pick('BOM No', boms, function (b) {
                r.BomHeaderId = b.Id; r.BomNo = b.Name;
                if (b.Id > 0) {
                    var it = outputItemsOf(b.Id)[0];
                    r.OutPutItemId = it ? it.Id : 0; r.OutPutItemName = it ? it.Name : '';
                } else { r.OutPutItemId = 0; r.OutPutItemName = ''; }
                render(); calculation();
            });
        } else if (f === 'OutPutItemName') {                           // :5576
            if (!output.length) { alertMsg('Out Put Grid Record not founds...'); return; }
            if (r.BomWagesId !== 0) { alertMsg("You can't Update Bom No of row Against Bill Of Material"); return; }
            if (!r.BomHeaderId) { alertMsg('Please Select Bom No first...'); return; }
            var items = outputItemsOf(r.BomHeaderId);
            if (items.length) pick('OutPut Item', items, function (x) { r.OutPutItemId = x.Id; r.OutPutItemName = x.Name; render(); });
        }
    }
    function updateContractorInWages() {                             // BtnUpdateConctratorInWages_Click:6264
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
    function fillWagesFilterCombo() {                                // FillWagesGridFilterCombo
        var seen = {}, rows = [];
        wages.forEach(function (r) { if (r.ServiceActivity && !seen[r.ServiceActivity]) { seen[r.ServiceActivity] = 1; rows.push({ Id: r.ContractorWagesRateScheduleId, Name: r.ServiceActivity }); } });
        W3B.fill('CmbActivityforContractorUpdateInWagesGrid', rows, true, '');
    }
    function updateWarehouse(list, label) {                          // BtnUpdateWarehouseInput / Output:6160 / 6213 - the first OUTPUT row's warehouse + the default job lot
        if (!list.length) { alertMsg(label + ' Grid Record not Found'); return; }
        var first = output[0];
        if (first && !first.WarehouseId) { alertMsg('Warehouse Field is Empty in ' + label + ' Grid Row No: 01'); return; }
        if (!first) return;
        var j = defJob();
        list.forEach(function (r) { r.WarehouseId = first.WarehouseId; r.WarehouseName = first.WarehouseName; r.JobLotId = j.JobLotId; r.JobLot = j.JobLot; });
        render();
    }
    function clearInput() {                                          // btnClearInput_Click
        if (!W3B.ask('are you sure to reset Input Detail?')) return;
        input.forEach(function (r) { if (r.Id > 0) rmInput.push(r); });
        input = []; render(); calculation();
    }

    // ----------------------------------------------------------------------------- rate batch (btnGenerateRatesOutput_Click:5723 / btnUpdateInputWagesOH_Click:6067)
    function checkTypeAndOutput() {
        if (!typeId()) { W3B.focus('CmbProductionType'); throw new Error('ProductionType Field Required'); }
        if (!output.length) throw new Error('Output Grid Record not Found');
    }
    function generateRates() {
        try { checkTypeAndOutput(); updateRatesOutput(); } catch (e) { W3B.fail(e); }
        render(); calculation();
    }
    function updateRatesOutput() {                                   // :6010
        if (output.length === 1 && output[0].BomHeaderId > 0 && !(output[0].ItemId > 0)) throw new Error('Output Grid Record not Filled Completely');
        var groups = {};
        input.forEach(function (r) { groups[r.BomHeaderId] = (groups[r.BomHeaderId] || 0) + n(r.Amount); });
        Object.keys(groups).forEach(function (k) {
            var bom = +k;
            var oh = overhead.filter(function (x) { return x.BomHeaderId === bom; }).reduce(function (a, x) { return a + n(x.Amount); }, 0);
            var net = groups[k] + oh;
            if (!wagesDebit) net += wages.filter(function (x) { return x.BomHeaderId === bom; }).reduce(function (a, x) { return a + n(x.Amount); }, 0);
            var rows = output.filter(function (o) { return o.BomHeaderId === bom && o.EntryTypeId === 1; });
            var tq = rows.reduce(function (a, o) { return a + n(o.VarientQty); }, 0);
            rows.forEach(function (o) {
                var vq = n(o.VarientQty);
                o.Amount = tq > 0 ? net / tq * vq : 0;
                o.Rate = vq > 0 ? o.Amount / vq : 0;
            });
        });
    }
    function outputQtyByBom() {
        var m = {};
        output.forEach(function (o) { m[o.BomHeaderId] = (m[o.BomHeaderId] || 0) + n(o.ItemQty); });
        return m;
    }
    function updateInputQty(slab) {                                  // UpdateInputQtyIncaseOfSlabProduction:5753
        if (!input.length) throw new Error('Input Grid Record not Found');
        var m = outputQtyByBom();
        Object.keys(m).forEach(function (k) {
            var tq = m[k];
            input.filter(function (r) { return r.BomHeaderId === +k; }).forEach(function (r) {
                var q = slab ? n(r.BomQty) * tq : n(r.BomQty), f = slab ? tq : 1;
                r.ItemQty = q; r.MinQty = n(r.BomMinQty) * f; r.MaxQty = n(r.BomMaxQty) * f;
                r.NetWeight = q * n(r.Uom);
                r.Amount = (n(r.RateUom) > 0 && n(r.Rate) > 0) ? q * n(r.Uom) / n(r.RateUom) * n(r.Rate) : 0;
            });
        });
    }
    function updateQtyByBom(rows, slab, empty) {                     // UpdateWagesQty... :5798 / UpdateOHQty... :5838
        if (!rows.length) throw new Error(empty);
        var m = outputQtyByBom();
        Object.keys(m).forEach(function (k) {
            var tq = slab ? m[k] : 1;
            rows.filter(function (r) { return r.BomHeaderId === +k; }).forEach(function (r) {
                var q = n(r.BomQty) * tq;
                r.Qty = q; r.Amount = (q > 0 && n(r.Rate) > 0) ? q * n(r.Rate) : 0;
            });
        });
    }
    function updateInputCurrentStockAndAvgRate() {                   // :5880
        if (!input.length) throw new Error('Input Grid Record not Found');
        var chain = Promise.resolve();
        input.forEach(function (r) {
            chain = chain.then(function () {
                if (!r.ItemId) { r.BalanceStock = 0; r.Rate = 0; r.Amount = 0; return; }
                return stockRefresh(r);
            });
        });
        return chain;
    }
    function updateWagesRates() {                                    // :6000
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
    function updateInputWagesOH() {
        try {
            checkTypeAndOutput();
            var t = typeId(), slab = (t === 46 || t === 47);
            if (slab || t === 45) { updateInputQty(slab); updateQtyByBom(wages, slab, 'Wages Grid Record not Found'); updateQtyByBom(overhead, slab, 'Overhead Grid Record not Found'); }
        } catch (e) { render(); calculation(); W3B.fail(e); return; }
        updateInputCurrentStockAndAvgRate().then(updateWagesRates).then(function () { render(); calculation(); }).catch(function (e) { render(); calculation(); W3B.fail(e); });
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
    function fromReset() {                                           // FromReset (production no, type, plant, WIP fields and remarks are NOT cleared by the desktop)
        recId = 0; updateIdx = -1; prevWorkOrderId = 0;
        input = []; output = [blankOutput()]; overhead = [blankOverhead()]; wages = [blankWages()]; rmInput = []; rmOutput = [];
        W3B.clearCombo('CmbWorkOrderNo'); resetDetailOutput();
        ['txtTotalInputs', 'txtOverHead', 'txtWagesAmount', 'txtTotalOutPutAutoRatedAmount', 'txtOutPutAutoRatePerQtyAmount', 'txtQtyTotal'].forEach(function (id) { $(id).value = ''; });
        mode(); render(); fillWagesFilterCombo();
        return loadWorkOrders('').then(function () { return W3B.getJson(api + '/doc-no'); }).then(function (d) { $('txtdocno').value = d.docNo; }).catch(W3B.fail);
    }
    function newClick() { fromReset(); }
    function refresh() {                                             // btnRefresh_Click
        W3B.getJson(api + '/refresh').then(function (d) { look.overheadAccounts = d.overheadAccounts; look.allUoms = d.allUoms; look.jobLots = d.jobLots; look.contractors = d.contractors;
            look.refParties = d.refParties; look.customers = d.customers; applyLookups(d); return loadItemsForType(); }).catch(W3B.fail);
    }

    function formValidation() {                                      // FormValidation
        if (!String($('txtdocno').value).trim() || $('txtdocno').value === '0') { alertMsg('Doc No Field Required'); return false; }
        if (!typeId()) { alertMsg('Production Type Feild Required'); W3B.focus('CmbProductionType'); return false; }
        if (!$('txtProductionNo').value.trim()) { alertMsg('Production No Field Required'); W3B.focus('txtProductionNo'); return false; }
        if (!W3B.int($('CmbPlantName').value)) { alertMsg('Plant Name Feild Required'); W3B.focus('CmbPlantName'); return false; }
        if (!W3B.int($('CmbWIPWarehouse').value)) { alertMsg('WIPWarehouse Field Required'); W3B.focus('CmbWIPWarehouse'); return false; }
        if (!W3B.int($('CmbWIPAccount').value)) { alertMsg('WIPAccount Field Required'); W3B.focus('CmbWIPAccount'); return false; }
        if (wagesDebit && !W3B.int($('CmbWagesExpenseAccount').value)) { alertMsg('Wages Expense Account Field Required'); W3B.focus('CmbWagesExpenseAccount'); return false; }
        return true;
    }
    var saving = false;
    function save() {                                                // Insert()
        if (saving) return;
        if (!formValidation()) return;
        if (!input.length || !output.length) { alertMsg('Grid Record Not Found...\nGrid (Input and OutPut) Should have atleast one record'); return; }
        if (!W3B.ask(recId > 0 ? 'Are you sure to Update' : 'Are you sure to Save')) return;
        try { updateRatesOutput(); } catch (e) { W3B.fail(e); return; }      // btnGenerateRatesOutput_Click(null, null) before the net check
        render(); calculation();
        saving = true;
        W3B.postJson(api + '/save', body()).then(function (d) {
            saving = false;
            alertMsg(d.message);
            var savedId = d.id, print = rights.print;
            var slip = $('ChkPreviewSlip').checked, vch = $('chkPrintVoucher').checked;
            return fromReset().then(function () {
                if (slip && print) printSlipFor(savedId);
                if (vch && print) printVoucherFor(savedId);
            });
        }).catch(function (e) { saving = false; W3B.fail(e); });
    }
    function body() {
        var cur = 0;
        for (var i = 0; i < output.length; i++) { if (output[i].BomHeaderId > 0 && output[i].ItemId > 0) { cur = i; break; } }
        return { id: recId, docDate: $('txtdocdate').value, docNo: $('txtdocno').value, productionTypeId: typeId(), productionNo: $('txtProductionNo').value,
            plantId: W3B.int($('CmbPlantName').value), wipWarehouseId: W3B.int($('CmbWIPWarehouse').value), wipAccountId: W3B.int($('CmbWIPAccount').value),
            wagesExpenseAccountId: W3B.int($('CmbWagesExpenseAccount').value), workOrderId: curWorkOrder(), remarksHeader: $('txtRemarksHeader').value,
            currentOutputIndex: cur, input: input, output: output, overhead: overhead, wages: wages, removedInput: rmInput, removedOutput: rmOutput };
    }
    function del() {                                                 // btnDelete_Click
        if (recId <= 0) { alertMsg('No record found to Delete'); return; }
        if (!W3B.ask('Are you sure to Delete?')) return;
        W3B.postForm(api + '/delete?id=' + recId).then(function (d) { alertMsg(d.message); return fromReset(); }).catch(W3B.fail);
    }

    // ----------------------------------------------------------------------------- print
    function printSlipFor(id) {                                      // the 1850 production slip (ConcretePrintController)
        if (!id) return;
        window.open('/reports/print/1850-production-slip' + W3B.qs({ branchesId: look.branchId, documentTypeId: 1850, id: id }), '_blank');
    }
    function printSlip() { printSlipFor(recId); }
    function printVoucherFor(id) {                                   // VoucherReprot
        if (!id) return;
        var win = window.CrystalPrint ? window.CrystalPrint.reserve() : null;
        W3B.getJson(api + '/voucher-head?id=' + id).then(function (d) {
            var vh = W3B.int(d.voucherHeadId);
            if (!vh) { if (win) window.CrystalPrint.release(win); alertMsg('VoucherId Not Found'); return; }
            return window.CrystalPrint.open('acc-118', { id: vh, documentTypeId: 0 }, null, win);
        }).catch(function (e) { if (win) window.CrystalPrint.release(win); W3B.fail(e); });
    }
    function printVoucher() { printVoucherFor(recId); }

    // ----------------------------------------------------------------------------- ReadById:3843
    function load(id) {
        return W3B.getJson(api + '/read/' + id).then(function (d) {
            return fromReset().then(function () {
                recId = d.id;
                $('txtdocdate').value = d.docDate; $('txtdocno').value = d.docNo;
                W3B.setCombo('CmbProductionType', d.productionTypeId);
                return loadItemsForType().then(function () {
                    W3B.setCombo('CmbPlantName', d.plantId);
                    return plantLeave();
                }).then(function () {
                    W3B.setCombo('CmbWIPWarehouse', d.wipWarehouseId); W3B.setCombo('CmbWIPAccount', d.wipAccountId); W3B.setCombo('CmbWagesExpenseAccount', d.wagesExpenseAccountId);
                    $('txtProductionNo').value = d.productionNo; $('txtRemarksHeader').value = d.remarksHeader;
                    output = d.output.length ? d.output : [blankOutput()];
                    input = d.input; overhead = d.overhead.length ? d.overhead : [blankOverhead()]; wages = d.wages.length ? d.wages : [blankWages()];
                    var ids = d.output.map(function (r) { return r.WorkOrderDetailId; }).filter(function (v, k, a) { return v > 0 && a.indexOf(v) === k; }).join(',');
                    return loadWorkOrders(ids).then(function () {
                        if (d.workOrderId > 0) { W3B.setCombo('CmbWorkOrderNo', d.workOrderId); prevWorkOrderId = d.workOrderId; }
                    });
                }).then(function () { mode(); render(); calculation(); fillWagesFilterCombo(); tab('form'); });
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
    function history() {                                             // HistoryGridFill:4328
        var ref = document.querySelector('input[name="rdRef"]:checked');
        var q = { fromDocNo: W3B.int($('txtFromDocNo').value), toDocNo: W3B.int($('txtToDocNo').value), productionId: W3B.int($('cmbHistProductionNo').value), referred: ref ? W3B.int(ref.value) : 0 };
        if ($('chkUseDates').checked) { q.fromDate = $('dtpFrom').value; q.toDate = $('dtpTo').value; }
        W3B.getJson(api + '/history' + W3B.qs(q)).then(function (rows) {
            hist = rows;
            $('DatagridHistory').innerHTML = W3B.tableHtml([
                { h: 'Print', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-hact="print" data-hi="' + i + '">Print</button>'; } },
                { h: 'Voucher', html: true, cls: 'w3-btn', w: 50, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-hact="voucher" data-hi="' + i + '">Voucher</button>'; } },
                { h: 'Edit', html: true, cls: 'w3-btn', w: 40, f: function (r, i) { return '<button type="button" class="rc-cellbtn" data-hact="edit" data-hi="' + i + '">Edit</button>'; } },
                { h: 'RecordNo', k: 'RecordNo', cls: 'num' }, { h: 'DocNo', k: 'DocNo' }, { h: 'DocDate', f: function (r) { return W3B.fmtDMY(r.DocDate); } },
                { h: 'ProductionNo', k: 'ProductionNo' }, { h: 'ReleaseNo', k: 'ReleaseNo' }, { h: 'ProductionType', k: 'ProductionType' }, { h: 'PlantName', k: 'PlantName' },
                { h: 'WipWareHouseName', k: 'WipWareHouseName' }, { h: 'WIPAccountTitle', k: 'WIPAccountTitle' }, { h: 'Remarks', k: 'RemarksHeader' },
                { h: 'EntryDate', f: function (r) { return W3B.fmtStamp(r.EntryDate); } }, { h: 'EntryUser', k: 'EntryUserName' },
                { h: 'ModifyDate', f: function (r) { return W3B.fmtStamp(r.ModifyDate); } }, { h: 'ModifyUser', k: 'ModifyUserName' },
                { h: 'IsApproved', bool: true, k: 'IsApproved', cls: 'ctr' }, { h: 'ApprovedDate', f: function (r) { return W3B.fmtStamp(r.ApprovedDate); } },
                { h: 'Attachments', k: 'NoOfAttachments', cls: 'num' }
            ], rows, { empty: 'No records' });
        }).catch(W3B.fail);
    }
    function historyDetail(id) {                                     // DatagridHistoryDetailbind
        W3B.getJson(api + '/read/' + id).then(function (d) {
            $('grdHistoryInput').innerHTML = W3B.tableHtml([
                { h: 'BomNo', k: 'BomNo' }, { h: 'WarehouseName', k: 'WarehouseName' }, { h: 'ItemName', k: 'ItemName' }, { h: 'Uom', k: 'UomCode' }, { h: 'BomQty', k: 'BomQty', cls: 'num' },
                { h: 'ItemQty', k: 'ItemQty', cls: 'num' }, { h: 'Rate', k: 'Rate', cls: 'num' }, { h: 'Amount', k: 'Amount', cls: 'num' }], d.input, { empty: 'No rows' });
            $('grdHistoryOutput').innerHTML = W3B.tableHtml([
                { h: 'BomNo', k: 'BomNo' }, { h: 'WorkOrderNo', k: 'WorkOrderNo' }, { h: 'CustomerName', k: 'CustomerName' }, { h: 'ReferenceNo', k: 'ReferenceNo' }, { h: 'Order', k: 'SaleOrder' },
                { h: 'WarehouseName', k: 'WarehouseName' }, { h: 'ItemName', k: 'ItemName' }, { h: 'Varient', k: 'AttributeVarient' }, { h: 'VarientUnit', k: 'VarientEquivalent', cls: 'num' },
                { h: 'Qty', k: 'ItemQty', cls: 'num' }, { h: 'T.Qty', k: 'VarientQty', cls: 'num' }, { h: 'Rate', k: 'Rate', cls: 'num' }, { h: 'Amount', k: 'Amount', cls: 'num' }], d.output, { empty: 'No rows' });
            $('grdHistoryOverHead').innerHTML = W3B.tableHtml([
                { h: 'BomNo', k: 'BomNo' }, { h: 'OverHeadAccount', k: 'OverHeadAccountTitle' }, { h: 'Percentage', k: 'PercentOfTotalInput', cls: 'num' }, { h: 'BomQty', k: 'BomQty', cls: 'num' },
                { h: 'Qty', k: 'Qty', cls: 'num' }, { h: 'Rate', k: 'Rate', cls: 'num' }, { h: 'Amount', k: 'Amount', cls: 'num' }, { h: 'OhRemarks', k: 'OhRemarks' }], d.overhead, { empty: 'No rows' });
            $('grdWagesHistory').innerHTML = W3B.tableHtml([
                { h: 'BomNo', k: 'BomNo' }, { h: 'ContractorName', k: 'ContractorName' }, { h: 'ItemName', k: 'OutPutItemName' }, { h: 'ServiceActivity', k: 'ServiceActivity' },
                { h: 'Qty', k: 'Qty', cls: 'num' }, { h: 'Rate', k: 'Rate', cls: 'num' }, { h: 'Amount', k: 'Amount', cls: 'num' }, { h: 'Remarks', k: 'Remarks' }], d.wages, { empty: 'No rows' });
        }).catch(W3B.fail);
    }
    function editFromHistory(i) {                                    // DatagridHistory_KeyDown
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
                if (g === 'output') { if (updateIdx !== -1) { alertMsg('Please Reset the Detail First...'); return; } addOutputRow(); render(); calculation(); }
                else if (g === 'overhead') { overhead.push(blankOverhead()); render(); }
                else {
                    var cur = wages[i] || {}, nw = blankWages();
                    var src = cur.BomHeaderId || cur.OutPutItemId ? cur : wages.filter(function (x) { return x.BomHeaderId || x.OutPutItemId; })[0];
                    if (src) { nw.BomHeaderId = src.BomHeaderId; nw.BomNo = src.BomNo; nw.OutPutItemId = src.OutPutItemId; nw.OutPutItemName = src.OutPutItemName; nw.ContractorId = src.ContractorId; nw.ContractorName = src.ContractorName; }
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
        else if (g === 'overhead') pickOverhead(i, f);
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
        else if (g === 'overhead') { if (f !== 'OhRemarks') row[f] = row[f] === '' ? '' : n(row[f]); if (f !== 'OhRemarks') overheadChanged(row, f); }
        else if (g === 'wages' && f === 'Qty') { row.Qty = n(row.Qty); wagesChanged(row); }
    });

    window.Cp = { newClick: newClick, refresh: refresh, save: save, del: del, printSlip: printSlip, printVoucher: printVoucher, tab: tab, history: history,
        dateType: dateType, productionTypeLeave: productionTypeLeave, plantLeave: plantLeave, itemLeave: itemLeave, bomLeave: bomLeave, attributeLeave: attributeLeave,
        itemSearchMode: itemSearchMode, partyMode: partyMode, workOrderLeave: workOrderLeave, addDetail: addDetail, updateDetail: updateDetail, cancelDetail: cancelDetail,
        loadInputFromBom: loadInputFromBom, updateInputWagesOH: updateInputWagesOH, updateInOhGrid: updateInOhGrid,
        updateWarehouseInput: function () { updateWarehouse(input, 'Input'); }, updateWarehouseOutput: function () { updateWarehouse(output, 'OutPut'); },
        clearInput: clearInput, updateContractorInWages: updateContractorInWages, generateRates: generateRates, pickClose: pickClose };

    W3B.keys({ n: newClick, s: save, u: function () { if (recId > 0) save(); }, r: refresh });

    // ----------------------------------------------------------------------------- boot (frmConcreteProduction_Load)
    W3B.getJson(api + '/lookups').then(function (d) {
        look = d; rights = d.rights || {}; fifo = !!d.fifo; wagesDebit = !!d.wagesDebitExpense; amountDec = d.amountDecimals === undefined ? 2 : d.amountDecimals;
        $('wagesExpWrap').style.display = wagesDebit ? '' : 'none';
        applyRights(); applyLookups(d);
        W3B.fill('cmbDateType', d.dateTypes || [], false);
        W3B.fill('cmbHistProductionNo', d.productionNos, true);
        if (d.productionTypes && d.productionTypes.length) $('CmbProductionType').value = String(d.productionTypes[0].Id);
        $('txtdocdate').value = W3B.today();
        return loadItemsForType().then(function () { return fromReset(); }).then(function () { dateType(); });
    }).catch(W3B.fail);
}());
