/* ============================================================================================
 * countx_pp_wages_bill.js - 614 Wages Bill (Party Processing)
 * Desktop: Architecture.WinApp.PartyProcessing.frmWagesBillPartyProcessing.cs. Exports window.PpWb.
 * Load (menu or opener: ?refDocTypeId=&refDocId=&grossWeight=), pending grid + Load (LoadDataForWages), Regular / Other
 * wages grids (grdwagesDetail_CellUpdated / grdStiching_CellUpdated with the rate and free-of-cost lookups, Add / Delete rows,
 * Apply All), ValidationOnformClose, Insert (server), ReadById, history (Slip / Edit / Voucher, detail), keys.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/wages-bill/';
    var P = {}; window.PpWb = P;
    var REF_TYPE = HRM.int(HRM.param('refDocTypeId')), REF_ID = HRM.int(HRM.param('refDocId')), GROSS_TOTAL = HRM.num(HRM.param('grossWeight'));
    var OPENED = REF_TYPE > 0 && REF_ID > 0;
    var S = { rights: {}, recId: 0, referred: false, cfg: {}, contractors: [], wagesAccounts: [], otherAccounts: [] };
    var tabs = PPC.tabs(function (t) { if (t === 'history' && OPENED) tabs.show('form'); });
    var wtabs = PPC.subTabs('wages');
    var ptabs = PPC.subTabs('pending');
    function R(v, d) { return PPC.round(v, d); }
    function refType() { return HRM.comboVal('cmbReferenceDocType'); }
    function onQty() { return !!S.cfg.amountOnQty; }

    // ------------------------------------------------------------------ grids
    function wagesColumns(kind) {
        var ro = function () { return S.referred; };
        var cols = [
            { key: 'Add', caption: 'Add', width: 40, render: function () { return S.referred ? '' : PPC.btn('Add', 'Add'); } },
            { key: 'Delete', caption: 'X', width: 30, render: function () { return S.referred ? '' : PPC.btn('Delete', 'X'); } },
            { key: 'SupplierId', caption: 'Contractor', width: 170, type: 'select',
                options: function () { return [[0, '']].concat(S.contractors.map(function (c) { return [c.Id, c.CompanyName]; })); } },
            { key: 'ContractorName', hidden: true },
            { key: 'WagesId', caption: 'WagesAccount', width: 170, type: 'select', readOnly: ro,
                options: function () { var l = kind === 'other' ? S.otherAccounts : S.wagesAccounts; return [[0, '']].concat(l.map(function (a) { return [a.Id, a.WagesAccountName]; })); } },
            { key: 'WagesAccount', hidden: true }, { key: 'WagesType', caption: 'WagesType' }, { key: 'Date', caption: 'Date' },
            { key: 'packingTypeId', hidden: true }, { key: 'packingType', caption: 'packingType' },
            { key: 'Weight', caption: 'Weight', type: 'edit-num', readOnly: ro, decimals: 2, sum: true },
            { key: 'PackSize', caption: 'PackSize' },
            { key: 'Quantity', caption: 'Quantity', type: 'edit-num', readOnly: ro, decimals: 2, sum: true },
            { key: 'WeightCut', caption: 'WeightCut', type: 'edit-num', readOnly: ro, decimals: 2 },
            { key: 'BillQty', hidden: true }, { key: 'BillWeight', caption: 'BillWeight', type: 'num', decimals: 2, sum: true },
            { key: 'RateWithoutAddLess', caption: 'RateWithoutAddLess', type: 'num', decimals: 2 },
            { key: 'RateAddLess', caption: 'RateAddLess', type: 'edit-num', decimals: 2, readOnly: function () { return !S.cfg.enableAddLess; } },
            { key: 'Rate', caption: 'Rate', type: 'num', decimals: 2 }, { key: 'Amount', caption: 'Amount', type: 'num', decimals: 2, sum: true },
            { key: 'ItemId', hidden: true }, { key: 'Item', caption: 'Item' }, { key: 'jobLotId', hidden: true }, { key: 'jobLot', caption: 'jobLot' },
            { key: 'Crop', caption: 'Crop' }, { key: 'MoveFromId', hidden: true }, { key: 'MoveFrom', caption: 'MoveFrom' }, { key: 'MoveToId', hidden: true },
            { key: 'MoveTo', caption: 'MoveTo', hidden: true }, { key: 'PurchaseGLAC', hidden: true }, { key: 'WarehouseType', hidden: true },
            { key: 'IsCompany', caption: 'IsCompany', type: 'edit-check', readOnly: ro }, { key: 'JobOrderId', hidden: true },
            { key: 'RefDocQty', hidden: true }, { key: 'RefDocWeight', hidden: true }, { key: 'RefLineId', caption: 'RefLineId', hidden: true }];
        return cols;
    }
    var grdwagesDetail = new HRM.Grid('grdwagesDetail', { emptyText: '', totals: true, checkAll: 'IsCompany',
        canCheck: function () { return !S.referred; },
        columns: wagesColumns('regular'), rowClass: function (r) { return r.WagesType === 'Free Of Cost' ? 'ppc-free' : ''; },
        onChange: function (r, key, i) { regularUpdated(r, key, i); } });
    var grdStiching = new HRM.Grid('grdStiching', { emptyText: '', totals: true, checkAll: 'IsCompany',
        canCheck: function () { return !S.referred; },
        columns: wagesColumns('other'), rowClass: function (r) { return r.WagesType === 'Free Of Cost' ? 'ppc-free' : ''; },
        onChange: function (r, key, i) { otherUpdated(r, key, i); } });
    PPC.onButton(grdwagesDetail, 'grdwagesDetail', function (r, act, i) {   // grdwagesDetail_ColumnButtonClick
        if (S.referred) return;
        if (act === 'Delete') { if (grdwagesDetail.rows().length <= 1) { HRM.box('You Can Not Delete All rows'); return; } grdwagesDetail.remove(i); }
        if (act === 'Add') addRow(grdwagesDetail, i, false);
    });
    PPC.onButton(grdStiching, 'grdStiching', function (r, act, i) {         // grdStiching_ColumnButtonClick
        if (S.referred) return;
        if (act === 'Delete') { if (grdStiching.rows().length <= 1) { HRM.box('You Can Not Delete All rows....'); return; } grdStiching.remove(i); }
        if (act === 'Add') addRow(grdStiching, i, true);
    });
    function applyVisibility() {                                            // DetailGridSettings / GridStichingSettings visibility
        [grdwagesDetail, grdStiching].forEach(function (g) {
            PPC.showCols(g, ['RateAddLess', 'RateWithoutAddLess'], !!S.cfg.enableAddLess);
            PPC.showCols(g, ['MoveTo'], refType() === 220);
        });
        PPC.showCols(grdwagesDetail, ['RefLineId'], refType() === 118 || refType() === 176);
        PPC.showCols(grdStiching, ['RefLineId'], refType() === 112 || refType() === 66);
        grdwagesDetail.draw(); grdStiching.draw();
    }
    function prep(rows) { (rows || []).forEach(function (r) { r._sup = HRM.int(r.SupplierId); }); return rows || []; }

    var gridPending = new HRM.Grid('gridPendingWagesSlip', { emptyText: '',
        columns: [PPC.btnCol('Load', 'Load'), { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
            { key: 'DocumentTypeDescription', caption: 'DocumentTypeDescription' }, { key: 'DocDate', caption: 'DocDate', type: 'date' },
            { key: 'DocNo', caption: 'DocNo', type: 'int' }, { key: 'StockPartyId', hidden: true }, { key: 'StockParty', caption: 'StockParty' },
            { key: 'GpNo', caption: 'GpNo', type: 'int' }, { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'BiltyNo', caption: 'BiltyNo' },
            PPC.numCol('GrossWeight', 'GrossWeight', '#,##0.###'), PPC.numCol('TotalQty', 'TotalQty', '#,##0.###'), { key: 'JobOrderId', hidden: true },
            { key: 'RefDocQty', hidden: true }, { key: 'RefDocWeight', hidden: true }, { key: 'RefLineId', hidden: true }] });
    PPC.onButton(gridPending, 'gridPendingWagesSlip', function (r, act, i, b) { if (act === 'Load') loadData(b); });
    var grdDataRetrieve = new HRM.Grid('grdDataRetrieve', { emptyText: '',
        columns: [{ key: 'HeaderId', hidden: true }, { key: 'ContractorName', caption: 'ContractorName' }, { key: 'WagesAccount', caption: 'WagesAccount' },
            { key: 'PackingType', caption: 'PackingType' }, PPC.numCol('Weight', 'Weight', '#,##0.##'), { key: 'PackSize', caption: 'PackSize' },
            PPC.numCol('Quantity', 'Quantity', '#,##0.##'), { key: 'WeightCut', caption: 'WeightCut' }, PPC.numCol('BillWeight', 'BillWeight', '#,##0.##'),
            { key: 'RateWithoutAddLess', caption: 'RateWithoutAddLess' }, PPC.numCol('RateAddLess', 'RateAddLess', '#,##0.##'), { key: 'Rate', caption: 'Rate' },
            PPC.numCol('Amount', 'Amount', '#,##0.##'), { key: 'HeaderWeight', hidden: true }, { key: 'HeaderQty', hidden: true },
            { key: 'IsCompany', caption: 'IsCompany', type: 'check' }, { key: 'JobOrderId', hidden: true }, { key: 'RefDocQty', hidden: true },
            { key: 'RefDocWeight', hidden: true }, { key: 'RefLineId', hidden: true }] });

    var DataGridHistory = new HRM.Grid('DataGridHistory', { filterRow: true, emptyText: 'No record found.',
        columns: [PPC.btnCol('Slip', 'Slip'), PPC.btnCol('Edit', 'Edit'), PPC.btnCol('Voucher', 'Voucher', 70), { key: 'Id', hidden: true },
            { key: 'DocumentTypeId', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocNoId', hidden: true },
            { key: 'DocumentType', caption: 'DocumentType' }, { key: 'DocDate', caption: 'DocDate', type: 'date' }, { key: 'DocNo', caption: 'DocNo', type: 'int' },
            { key: 'RefDocNo', caption: 'RefDocNo', type: 'int' }, { key: 'StockPartyId', hidden: true }, { key: 'StockParty', caption: 'StockParty' },
            PPC.numCol('QtyTotal', 'QtyTotal', '#,##0.##'), PPC.numCol('WeightTotal', 'WeightTotal', '#,##0.##'), PPC.numCol('WagesAmount', 'WagesAmount', '#,##0.##'),
            { key: 'OtherRemarks', caption: 'OtherRemarks' }, { key: 'JobOrderId', hidden: true }, { key: 'JobOrderNo', caption: 'JobOrderNo' },
            { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' }, { key: 'EntryUser', caption: 'EntryUser' },
            { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' }, { key: 'ModifyUser', caption: 'ModifyUser' }, { key: 'IsReferred', hidden: true }],
        onDouble: function (r) { S.referred = HRM.int(r.IsReferred) === 1; readById(HRM.int(r.Id)); },
        onSelect: function (r) { HRM.get(API + 'history-detail', { id: HRM.int(r.Id) }).then(function (rows) { GrdHistoryDetail.set(rows); }).catch(HRM.fail); } });
    PPC.onButton(DataGridHistory, 'DataGridHistory', function (r, act, i, b) {
        var id = HRM.int(r.Id);
        if (act === 'Voucher') PPC.voucher(API + 'voucher', id, b);
        if (act === 'Slip') slip(id, b, true);
        if (act === 'Edit') { S.referred = HRM.int(r.IsReferred) === 1; readById(id); }
    });
    var GrdHistoryDetail = new HRM.Grid('GrdHistoryDetail', { emptyText: '',
        columns: [{ key: 'HeaderId', hidden: true }, { key: 'ContractorName', caption: 'ContractorName' }, { key: 'WagesAccount', caption: 'WagesAccount' },
            { key: 'packingType', caption: 'packingType' }, PPC.numCol('Weight', 'Weight', '#,##0.##'), { key: 'PackSize', caption: 'PackSize' },
            PPC.numCol('Quantity', 'Quantity', '#,##0.##'), { key: 'WeightCut', caption: 'WeightCut' }, PPC.numCol('BillWeight', 'BillWeight', '#,##0.##'),
            { key: 'RateWithoutAddLess', caption: 'RateWithoutAddLess' }, PPC.numCol('RateAddLess', 'RateAddLess', '#,##0.##'), { key: 'Rate', caption: 'Rate' },
            PPC.numCol('Amount', 'Amount', '#,##0.##'), { key: 'IsCompany', caption: 'IsCompany', type: 'check' }] });

    // ------------------------------------------------------------------ server helpers
    function freeOfCost(r) {                                                // CommonServices.CheckItemsFreeofcostforWages
        return HRM.get(API + 'free-of-cost', { date: HRM.str(r.Date), refDocTypeId: refType(), itemId: HRM.int(r.ItemId), wagesId: HRM.int(r.WagesId) })
            .then(function (o) { return !!o.free; });
    }
    function wagesRate(r) {                                                 // CommonServices.GetWagesRate
        return HRM.get(API + 'rate', { date: HRM.str(r.Date), packSize: HRM.num(r.PackSize), wagesId: HRM.int(r.WagesId), contractorId: HRM.int(r.SupplierId) })
            .then(function (o) { return HRM.num(o.rate); });
    }
    function addLess(r, key, rateWithout) {                                 // RateAddLess against PercentageForRateAddLessPartyProcessing
        var al = HRM.num(r.RateAddLess);
        if (S.cfg.enableAddLess && key === 'RateAddLess') {
            var pct = HRM.num(S.cfg.addLessPercentage);
            if (pct > 0) {
                var byCfg = pct * rateWithout / 100;
                var minus = !(al > 0);
                if (Math.abs(al) > byCfg) { al = minus ? -byCfg : byCfg; r.RateAddLess = al; HRM.box('RateAddLess can not be grater than RateAdLess In config ' + PPC.net(byCfg)); }
            } else { al = 0; r.RateAddLess = 0; HRM.box('Please set RateAddLess Percentage in config first...'); }
        }
        return al;
    }
    function othersSum(grid, r, k) { var s = 0; grid.rows().forEach(function (x) { if (x !== r) s += HRM.num(x[k]); }); return s; }
    function notProd() { return refType() !== 118 && refType() !== 176; }

    // ------------------------------------------------------------------ grdwagesDetail_CellUpdated
    function regularUpdated(r, key) {
        if (key === 'IsCompany') return;
        if (key === 'SupplierId' || key === 'WagesId') r[key] = HRM.int(r[key]);
        var gross = PPC.dbl('txtGrossWeight'), tq = PPC.dbl('txtQty');
        var regTotalWeight = gross - othersSum(grdwagesDetail, r, 'BillWeight');
        var regGrossWeight = gross - othersSum(grdwagesDetail, r, 'Weight');
        var regTotalQty = tq - othersSum(grdwagesDetail, r, 'Quantity');
        var prevRate = HRM.num(r.Rate);
        var chain = Promise.resolve();
        if (key === 'WagesId') chain = chain.then(function () { return freeOfCost(r).then(function (f) { r.WagesType = f ? 'Free Of Cost' : 'Regular'; }); });
        return chain.then(function () {
            if ((key === 'Quantity' || key === 'WeightCut') && HRM.num(r.Quantity) > 0 && HRM.num(r.PackSize) > 0) {
                var qty = HRM.num(r.Quantity);
                if (onQty() && qty > regTotalQty && notProd()) { HRM.box('Stiching Total Qty can not be Greater than Wages total Qty'); r.Quantity = R(regTotalQty, 2); qty = R(regTotalQty, 2); }
                else r.Quantity = R(qty, 2);
                var wc = HRM.num(r.WeightCut), ps = HRM.num(r.PackSize), w = qty * ps;
                if (!onQty() && w > regGrossWeight && notProd()) {
                    r.Weight = R(regGrossWeight, 2); HRM.box('Stiching Weight can not be Greater than Wages Gross Weight');
                    w = R(regGrossWeight, 2); qty = R(w / ps, 2); r.Quantity = qty;
                }
                r.Weight = w;
                var bw = w - Math.abs(qty * wc);
                if (!onQty() && bw > regTotalWeight && notProd()) {
                    r.Weight = R(regGrossWeight, 2); r.BillWeight = R(regTotalWeight, 2);
                    HRM.box('Stiching Total Weight can not be Greater than Wages total Weight'); r.Quantity = R(qty * ps, 2);
                } else r.BillWeight = R(bw, 2);
            }
            if ((key === 'Weight' || key === 'WeightCut') && HRM.num(r.Weight) > 0 && HRM.num(r.PackSize) > 0) {
                var a = HRM.num(r.Weight);
                if (!onQty() && a > regGrossWeight && notProd()) { r.Weight = R(regGrossWeight, 2); HRM.box('Stiching Weight can not be Greater than Wages Gross Weight'); a = R(regGrossWeight, 2); }
                var b = HRM.num(r.PackSize), q2 = a / b;
                r.Quantity = R(q2, 2);
                if (onQty() && q2 > regTotalQty && notProd()) {
                    HRM.box('Stiching Total Qty can not be Greater than Wages total Qty'); r.Quantity = R(regTotalQty, 2); q2 = R(regTotalQty, 2);
                    r.Weight = R(q2 * HRM.num(r.PackSize), 2);
                }
                var bw2 = HRM.num(r.Weight) - Math.abs(HRM.num(r.WeightCut) * q2);
                if (!onQty() && bw2 > regTotalWeight && notProd()) {
                    r.Weight = R(regGrossWeight, 2); r.BillWeight = R(regTotalWeight, 2); HRM.box('Stiching Total Weight can not be Greater than Wages total Weight');
                } else r.BillWeight = R(bw2, 2);
            }
            var rateKeys = ['WagesId', 'SupplierId', 'Quantity', 'Weight', 'WeightCut', 'RateAddLess'];
            if (rateKeys.indexOf(key) < 0) return 0;
            if (r.WagesType === 'Free Of Cost' || r.WarehouseType === 'Dryer') { r.Rate = 0; r.Amount = 0; return 0; }
            return wagesRate(r);
        }).then(function (rate) {
            function apply() {
                if (rate > 0) {
                    r.Rate = rate;
                    if (HRM.num(r.Rate) > 0 && HRM.num(r.BillWeight) > 0) {
                        var rw = HRM.num(r.Rate);
                        r.RateWithoutAddLess = rw;
                        var total = rw + addLess(r, key, rw);
                        r.Rate = total;
                        r.Amount = R(onQty() ? HRM.num(r.Quantity) * total : HRM.num(r.BillWeight) / HRM.num(r.PackSize) * total, 2);
                    } else r.Amount = 0;
                } else { r.Rate = 0; r.RateWithoutAddLess = 0; r.Amount = 0; }
            }
            if (!S.referred) apply();
            else if (rate === prevRate) apply();
            else { r.SupplierId = r._sup; HRM.box('Contractor You are trying Change has diffrent rate than Previous So it Can not be Change'); }
            r._sup = HRM.int(r.SupplierId);
            grdwagesDetail.draw();
        }).catch(function (e) { grdwagesDetail.draw(); HRM.fail(e); });
    }

    // ------------------------------------------------------------------ grdStiching_CellUpdated
    function otherUpdated(r, key) {
        if (key === 'SupplierId' || key === 'WagesId') r[key] = HRM.int(r[key]);
        var gross = PPC.dbl('txtGrossWeight'), tq = PPC.dbl('txtQty');
        var regTotalWeight = gross - othersSum(grdStiching, r, 'BillWeight');
        var regGrossWeight = gross - othersSum(grdStiching, r, 'Weight');
        var regTotalQty = tq - othersSum(grdStiching, r, 'Quantity');
        function rateCalc(k) {
            if (HRM.num(r.Rate) > 0 && HRM.num(r.BillWeight) > 0) {
                var rw = HRM.num(r.Rate);
                r.RateWithoutAddLess = rw;
                var total = rw + addLess(r, k, rw);
                r.Rate = total;
                r.Amount = R(HRM.num(r.BillWeight) / HRM.num(r.PackSize) * total, 2);
            } else r.Amount = 0;
        }
        function weightBlock() {
            if (HRM.num(r.Weight) > 0 && HRM.num(r.PackSize) > 0) {
                var w = HRM.num(r.Weight);
                if (!onQty() && w > regTotalWeight && notProd()) { HRM.box('Stiching Total Weight can not be Greater than Wages total Weight'); r.BillWeight = R(regTotalWeight, 2); }
                else r.BillWeight = R(w, 2);
            }
        }
        function cutBlock(checkQty) {
            if (HRM.num(r.Quantity) > 0 && HRM.num(r.WeightCut) > 0 && HRM.num(r.PackSize) > 0 && HRM.num(r.Weight) > 0) {
                var q = HRM.num(r.Quantity), bw = HRM.num(r.Weight) - Math.abs(HRM.num(r.WeightCut) * q);
                if (!onQty() && bw > regTotalWeight && notProd()) { HRM.box('Stiching Total Weight can not be Greater than Wages total Weight'); r.BillWeight = R(regTotalWeight, 2); r.Weight = R(regGrossWeight, 2); }
                else r.BillWeight = R(bw, 2);
                if (checkQty) {
                    if (onQty() && q > regTotalQty && notProd()) { HRM.box('Stiching Total Qty can not be Greater than Wages total Qty'); r.Quantity = R(regTotalQty, 2); }
                    else r.Quantity = R(q, 2);
                }
            }
        }
        var chain = Promise.resolve();
        if (key === 'WagesId') chain = chain.then(function () { return freeOfCost(r).then(function (f) { r.WagesType = f ? 'Free Of Cost' : 'Regular'; }); });
        return chain.then(function () {
            var rateKeys = ['WagesId', 'SupplierId', 'Quantity', 'Weight', 'WeightCut', 'RateAddLess'];
            if (rateKeys.indexOf(key) < 0) return;
            if (r.WagesType === 'Free Of Cost' || r.WarehouseType === 'Dryer') { r.Rate = 0; r.Amount = 0; return; }
            var prev = HRM.num(r.Rate);
            return wagesRate(r).then(function (rate) {
                function apply() {
                    if (rate > 0) { r.Rate = rate; weightBlock(); cutBlock(true); rateCalc(key); }
                    else { r.Rate = 0; if (S.referred) r.RateWithoutAddLess = 0; }
                }
                if (!S.referred) apply();
                else if (rate === prev) apply();
                else { r.SupplierId = r._sup; HRM.box('Contractor You are trying Change has diffrent rate than Previous So it Can not be Change'); }
            });
        }).then(function () {
            if (key === 'Weight' || key === 'WeightCut') {
                if (HRM.num(r.Weight) > 0 && HRM.num(r.PackSize) > 0) {
                    var q3 = HRM.num(r.Weight) / HRM.num(r.PackSize), w3 = HRM.num(r.Weight);
                    if (!onQty() && w3 > regTotalWeight && notProd()) { HRM.box('Stiching Total Weight can not be Greater than Wages total Weight'); r.BillWeight = R(regTotalWeight, 2); r.Weight = R(regGrossWeight, 2); }
                    else r.BillWeight = R(w3, 2);
                    if (onQty() && q3 > regTotalQty && notProd()) { HRM.box('Stiching Total Qty can not be Greater than Wages total Qty'); r.Quantity = R(regTotalQty, 2); }
                    else r.Quantity = R(q3, 2);
                }
                cutBlock(true);
                rateCalc(key);
            }
            if (key === 'Quantity' || key === 'WeightCut') {
                if (HRM.num(r.Quantity) > 0 && HRM.num(r.PackSize) > 0 && HRM.num(r.Weight) > 0) {
                    var wc = HRM.num(r.WeightCut), ps = HRM.num(r.PackSize), q5 = HRM.num(r.Quantity), w4 = q5 * ps, bw4 = w4 - Math.abs(q5 * Math.abs(wc));
                    if (!onQty() && bw4 > regTotalWeight) { HRM.box('Stiching Total Weight can not be Greater than Wages total Weight'); r.BillWeight = R(regTotalWeight, 2); r.Weight = R(regGrossWeight, 2); }
                    else { r.Weight = w4; r.BillWeight = R(bw4, 2); }
                    if (onQty() && q5 > regTotalQty) { HRM.box('Stiching Total Qty can not be Greater than Wages total Qty'); r.Quantity = R(regTotalQty, 2); }
                    else r.Quantity = R(q5, 2);
                }
                if (HRM.num(r.Quantity) > 0 && HRM.num(r.WeightCut) > 0 && HRM.num(r.PackSize) > 0 && HRM.num(r.Weight) > 0) {
                    var q6 = HRM.num(r.Quantity), bw5 = HRM.num(r.Weight) - Math.abs(HRM.num(r.WeightCut) * q6);
                    if (!onQty() && bw5 > regTotalWeight) { HRM.box('Stiching Total Weight can not be Greater than Wages total Weight'); r.BillWeight = R(regTotalWeight, 2); r.Weight = R(regGrossWeight, 2); }
                    else r.BillWeight = R(bw5, 2);
                }
                if (HRM.num(r.Rate) > 0 && HRM.num(r.BillWeight) > 0) {
                    var rw4 = HRM.num(r.Rate), total4 = rw4 + addLess(r, key, rw4);
                    r.Rate = total4;
                    r.Amount = R(HRM.num(r.BillWeight) / HRM.num(r.PackSize) * total4, 2);
                } else r.Amount = 0;
            }
            r._sup = HRM.int(r.SupplierId);
            grdStiching.draw();
        }).catch(function (e) { grdStiching.draw(); HRM.fail(e); });
    }

    // ------------------------------------------------------------------ AddRowInGLGrid / AddRowInStichingGrid
    function addRow(grid, i, other) {
        var cur = grid.rows()[i]; if (!cur) return;
        var tw = PPC.dbl('txtGrossWeight'), tq = PPC.dbl('txtQty'), w = 0, wc = 0, q = 0;
        grid.rows().forEach(function (x) { w += HRM.num(x.Weight); wc = HRM.num(x.WeightCut); q += HRM.num(x.Quantity); });
        var gw = tw - w, gq = tq - q, prod = !notProd();
        /* desktop: the Other grid copies WagesType from the Regular grid's current row (a null row crashes there; the web falls back to its own row) */
        var typeSrc = other ? (grdwagesDetail.current() || cur) : cur;
        function copy(over) {
            var n = Object.assign({}, cur, { WagesType: typeSrc.WagesType, IsCompany: true }, over || {});
            n._sup = HRM.int(n.SupplierId);
            grid.add(n);
        }
        if (!(gw > 0 || gq > 0 || prod)) { HRM.box('Please Check Grid GrossWeight and TotalGrossWeight'); return; }
        if (onQty()) {
            if (gq > 0) {
                var iw = gq * HRM.num(cur.PackSize);
                copy({ Weight: R(iw, 2), Quantity: R(gq, 2), BillWeight: iw, Amount: R(gq * HRM.num(cur.Rate), 2) });
            } else if (prod) copy();
            else HRM.box('Please Check Grid Qty and TotalQty');
        } else if (gw > 0) {
            var iq = gw / HRM.num(cur.PackSize), bwp = gw - iq * wc;
            copy({ Weight: gw, Quantity: R(iq, 2), BillWeight: bwp, Amount: R(bwp / HRM.num(cur.PackSize) * HRM.num(cur.Rate), 2) });
        } else if (prod) copy();
        else HRM.box('Please Check Grid GrossWeight and TotalGrossWeight');
    }

    // ------------------------------------------------------------------ AddContractorValuesForAllInGrid
    P.applyAll = function (which) {
        var grid = which === 'other' ? grdStiching : grdwagesDetail;
        var rows = grid.rows();
        if (!rows.length || !grid.current()) return;
        var c = HRM.int(rows[0].SupplierId), wa = HRM.int(rows[0].WagesId);
        if (c === 0 && wa === 0) { HRM.box('First row contains No values Of Contractor And Wages Account.'); return; }
        var chain = Promise.resolve();
        rows.forEach(function (r) {
            chain = chain.then(function () {
                r.SupplierId = c; r.WagesId = wa; r._sup = c;
                return wagesRate(r).then(function (rate) {
                    if (rate > 0) {
                        r.Rate = rate;
                        if (HRM.num(r.Weight) > 0 && HRM.num(r.PackSize) > 0) r.BillWeight = R(HRM.num(r.Weight), 2);
                        if (HRM.num(r.Quantity) > 0 && HRM.num(r.WeightCut) > 0 && HRM.num(r.PackSize) > 0 && HRM.num(r.Weight) > 0)
                            r.BillWeight = R(HRM.num(r.Weight) - Math.abs(HRM.num(r.WeightCut) * HRM.num(r.Quantity)), 2);
                        r.Amount = HRM.num(r.Rate) > 0 && HRM.num(r.BillWeight) > 0 ? R(HRM.num(r.BillWeight) / HRM.num(r.PackSize) * HRM.num(r.Rate), 2) : 0;
                    } else r.Rate = 0;
                });
            });
        });
        return HRM.loading(chain.then(function () { grid.draw(); }).catch(HRM.fail));
    };

    // ------------------------------------------------------------------ load
    function histReset() { HRM.setVal('FromDateHistory', HRM.today()); HRM.setVal('ToDateHistory', HRM.today()); }
    function applyConfig(o) {
        S.cfg = { enableAddLess: !!o.enableAddLess, addLessPercentage: HRM.num(o.addLessPercentage), amountOnQty: !!o.amountOnQty,
            otherCompulsory118: !!o.otherCompulsory118, otherCompulsory176: !!o.otherCompulsory176 };
        S.contractors = o.contractors || S.contractors;
        if (o.wagesAccounts) S.wagesAccounts = o.wagesAccounts;
    }
    function setHeader(h) {                                                 // ReadOnlyMasterById / ReadById header part
        HRM.fill('CmbStockParty', [{ Id: h.StockPartyId, Name: h.StockPartyName }], 'Id', 'Name', { zero: false });
        HRM.fill('cmbReferenceDocType', [{ Id: h.RefDocumentTypeId, Name: h.DocumentTypeDescription }], 'Id', 'Name', { zero: false });
        HRM.setVal('txtDocdate', HRM.day(h.DocDate)); HRM.setVal('txtdocnumber', h.DocNo); HRM.setVal('txtDocNo', h.RefDocNo); HRM.setVal('txtGRNId', h.RefDocNoId);
        HRM.enable('cmbReferenceDocType', false);
        HRM.setVal('txtRemarks', h.OtherRemarks); HRM.setVal('txtGpNo', h.ScaleSlipNo); HRM.setVal('txtEntryType', h.RefDocument); HRM.check('chkisapprove', h.IsAproved);
    }
    function load() {                                                       // frmWagesBillPartyProcessing_Load
        HRM.setVal('txtDocdate', HRM.today()); histReset();
        wtabs.showTab('other', REF_TYPE === 118 || REF_TYPE === 176);
        grdwagesDetail.set([]); grdStiching.set([]);
        return HRM.loading(HRM.get(API + 'setup', { refDocTypeId: REF_TYPE, refDocId: REF_ID }).then(function (o) {
            S.rights = o.rights || {};
            HRM.applyRights(S.rights, { save: 'btnsave', print: 'Print', update: 'btnUpdate' });
            applyConfig(o);
            HRM.setVal('txtdocnumber', o.docNo);
            HRM.fill('CmbDocumentTypeHistory', o.docTypes, 'Id', 'DocumentTypeDescription', { zero: '' });
            gridPending.set(o.pending || []);
            S.recId = HRM.int(o.recId);
            if (OPENED && S.recId > 0) {
                HRM.show('btnsave', false); HRM.show('btnUpdate', true);
                setHeader(o.header);
                HRM.setVal('txtGrossWeight', PPC.net(o.header.WeightTotal));
                return loadData(null).then(function () {
                    if (GROSS_TOTAL > 0) HRM.setVal('txtGrossWeight', PPC.net(GROSS_TOTAL));
                    ptabs.showTab('pending', REF_TYPE === 176);
                    ptabs.showTab('previous', true);
                    ptabs.show('previous');
                    grdDataRetrieve.set(o.retrieval || []);
                });
            }
            if (OPENED) return loadData(null);
        }).catch(HRM.fail));
    }

    function loadData(btn) {                                                // LoadDataForWages (the pending grid's current row; the first on open)
        if ((!HRM.visible('btnsave') || HRM.visible('btnUpdate') || S.recId > 0) && REF_ID === 0) { HRM.box('Record Not Loaded Please Reset Form First'); return Promise.resolve(); }
        var row = gridPending.current() || gridPending.rows()[0];
        if (!row) return Promise.resolve();
        wtabs.showTab('other', false); wtabs.show('regular');
        var body = Object.assign({}, row, { refDocTypeId: REF_TYPE });
        return HRM.busy(btn, function () {
            return HRM.post(API + 'load', body).then(function (o) {
                HRM.fill('CmbStockParty', [{ Id: o.stockPartyId, Name: o.stockParty }], 'Id', 'Name', { zero: false });
                HRM.fill('cmbReferenceDocType', [{ Id: o.refDocTypeId, Name: o.refDocType }], 'Id', 'Name', { zero: false });
                HRM.setVal('txtDocNo', o.docNo); HRM.setVal('txtGRNId', o.grnId); HRM.setVal('txtGpNo', o.gpNo);
                HRM.setVal('txtGrossWeight', o.grossWeight); HRM.setVal('txtQty', o.qty); HRM.setVal('txtEntryType', o.entryType);
                if (HRM.int(o.refDocTypeId) === 176) {
                    S.recId = HRM.int(o.recId);
                    if (S.recId > 0 && o.header) {
                        HRM.show('btnUpdate', true); HRM.show('btnsave', false);
                        HRM.setVal('txtDocdate', HRM.day(o.header.DocDate)); HRM.setVal('txtdocnumber', o.header.DocNo); HRM.setVal('txtDocNo', o.header.RefDocNo);
                        HRM.setVal('txtGRNId', o.header.RefDocNoId); HRM.enable('cmbReferenceDocType', false); HRM.setVal('txtRemarks', o.header.OtherRemarks);
                        HRM.setVal('txtGpNo', o.header.ScaleSlipNo); HRM.setVal('txtEntryType', o.header.RefDocument); HRM.check('chkisapprove', o.header.IsAproved);
                    } else { HRM.show('btnUpdate', false); HRM.show('btnsave', true); }
                }
                if (o.detail) {
                    if (o.otherAccounts) S.otherAccounts = o.otherAccounts;
                    S.wagesAccounts = o.wagesAccounts || S.wagesAccounts;
                    wtabs.showTab('other', !!o.otherTab);
                    HRM.setVal('txtDocdate', HRM.day(o.docDate));
                    grdwagesDetail.set(prep(o.detail));
                    grdStiching.set(prep(o.other));
                    applyVisibility();
                }
            }).catch(HRM.fail);
        }, 'ppcWagesLoad');
    }

    // ------------------------------------------------------------------ ValidationOnformClose / reset
    function closeCheck(word) {
        if (!grdwagesDetail.rows().length || !grdDataRetrieve.rows().length) return Promise.resolve(true);
        var head = grdDataRetrieve.rows()[0];
        var differs = onQty() ? PPC.dbl('txtQty') !== HRM.num(head.HeaderQty) : PPC.dbl('txtGrossWeight') !== HRM.num(head.HeaderWeight);
        if (!differs) return Promise.resolve(true);
        var what = onQty() ? 'Qty' : 'Weight';
        if (!HRM.ask('Current ' + what + ' Does not match with Previous ' + what + '\nIf You ' + word + ' the Form Previous Saved Record Will Be Deleted\nAre you Sure to Do So???')) return Promise.resolve(false);
        /* the desktop passes RefDocId in the qty branch and RECID in the weight branch */
        return HRM.post(API + 'delete-by-reference', { refDocTypeId: REF_TYPE, refDocId: onQty() ? REF_ID : S.recId })
            .then(function () { return true; }).catch(function (e) { HRM.fail(e); return false; });
    }
    function formReset() {                                                  // FormRest
        S.recId = 0; S.referred = false;
        HRM.fill('cmbReferenceDocType', [], 'Id', 'Name', { zero: false }); HRM.fill('CmbStockParty', [], 'Id', 'Name', { zero: false });
        ['txtDocNo', 'txtGpNo', 'txtRemarks', 'txtQty', 'txtGrossWeight', 'txtGRNId'].forEach(function (id) { HRM.setVal(id, ''); });
        grdwagesDetail.set([]); grdStiching.set([]); grdDataRetrieve.set([]);
        HRM.show('btnsave', true); HRM.show('btnUpdate', false); HRM.enable('cmbReferenceDocType', true);
        wtabs.showTab('other', false); wtabs.show('regular');
        ptabs.showTab('pending', true); ptabs.showTab('previous', false); ptabs.show('pending');
        return HRM.get(API + 'reset', { refDocTypeId: REF_TYPE, refDocId: REF_ID }).then(function (o) {
            HRM.setVal('txtdocnumber', o.docNo); gridPending.set(o.pending || []);
        }).catch(HRM.fail);
    }
    P.btnnew = function () { closeCheck('Reset').then(function (ok) { if (ok) formReset(); }); };
    function closeForm() { closeCheck('Close').then(function (ok) { if (ok) HRM.close(); }); }
    P.btnRefresh = function (b) {                                           // btnRefresh_Click
        HRM.busy(b, function () {
            return HRM.get(API + 'refresh', { refDocTypeId: REF_TYPE }).then(function (o) { applyConfig(o); applyVisibility(); }).catch(HRM.fail);
        });
    };

    // ------------------------------------------------------------------ save / read
    function withNames(rows, accounts) {
        return rows.map(function (r) {
            var o = Object.assign({}, r); delete o._sup;
            var c = null;
            S.contractors.forEach(function (x) { if (HRM.int(x.Id) === HRM.int(r.SupplierId)) c = x; });
            var a = null; accounts.forEach(function (x) { if (HRM.int(x.Id) === HRM.int(r.WagesId)) a = x; });
            o.SupplierText = c ? c.CompanyName : ''; o.WagesText = a ? a.WagesAccountName : '';
            return o;
        });
    }
    function insert(btn) {                                                  // Insert()
        var dn = HRM.val('txtdocnumber').trim();
        if (dn === '' || dn === '0') { HRM.box('document Number Field Required'); return; }
        if (!refType()) { HRM.box('ReferenceDocType Field Required'); return; }
        if (!HRM.comboVal('CmbStockParty')) { HRM.box('Stock Party Field Required'); return; }
        var rn = HRM.val('txtDocNo').trim(), gid = HRM.val('txtGRNId').trim();
        if (rn === '' || rn === '0') { HRM.box('Doc No Field Required'); return; }
        if (gid === '' || gid === '0') { HRM.box('DocNoId Field Required'); return; }
        if (!onQty()) {
            if (grdwagesDetail.sum('Weight') !== PPC.dbl('txtGrossWeight')) { HRM.box('Wages Grid Total Weight and GrossWeight not equal please check!'); return; }
            if (refType() === 118 && grdStiching.sum('Weight') !== PPC.dbl('txtGrossWeight')) { HRM.box('Other Wages Grid Total Weight and GrossWeight not equal please check!'); return; }
        }
        if (!HRM.ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { id: S.recId, docNo: dn, docDate: HRM.val('txtDocdate'), refDocTypeId: refType(), pageRefDocTypeId: REF_TYPE,
            stockPartyId: HRM.comboVal('CmbStockParty'), refDocNo: rn, refDocNoId: gid, grossWeight: PPC.dbl('txtGrossWeight'), qty: PPC.dbl('txtQty'),
            remarks: HRM.val('txtRemarks'), gpNo: HRM.val('txtGpNo'), entryType: HRM.val('txtEntryType'), approved: HRM.checked('chkisapprove'),
            detail: withNames(grdwagesDetail.rows(), S.wagesAccounts), other: withNames(grdStiching.rows(), S.otherAccounts) };
        HRM.busy(btn, function () {
            return HRM.post(API + 'save', body).then(function (r) {
                HRM.box(r.message);
                if (OPENED && REF_TYPE !== 176) { HRM.close(); return; }
                return formReset();
            }).catch(HRM.fail);
        });
    }
    P.btnsave = function (b) { S.recId = 0; insert(b); };
    P.btnUpdate = function (b) { insert(b); };
    function readById(id) {                                                 // ReadById
        return HRM.loading(HRM.get(API + 'read', { id: id }).then(function (o) {
            S.recId = id;
            HRM.show('btnsave', false); HRM.show('btnUpdate', true);
            ptabs.showTab('previous', false); ptabs.show('pending');
            tabs.show('form');
            setHeader(o.header);
            HRM.setVal('txtQty', PPC.net(o.header.QtyTotal));
            S.wagesAccounts = o.wagesAccounts || S.wagesAccounts;
            if (o.otherAccounts) S.otherAccounts = o.otherAccounts;
            HRM.setVal('txtGrossWeight', o.grossWeight);
            grdwagesDetail.set(prep(o.detail));
            grdStiching.set(prep(o.other));
            wtabs.showTab('other', !!o.otherTab); wtabs.show('regular');
            applyVisibility();
        }).catch(HRM.fail));
    }
    function slip(id, btn, fromHistory) { return PPC.print(API + 'print', 'ppc-219', { id: id, history: !!fromHistory }, btn); }
    P.print = function (b) { slip(S.recId, b, false); };

    // ------------------------------------------------------------------ history
    P.btnResetHistory = function () {
        histReset(); HRM.setVal('txtFromDocNoHistory', ''); HRM.setVal('txtToDocNoHistory', ''); HRM.setCombo('CmbDocumentTypeHistory', 0);
        DataGridHistory.clear(); GrdHistoryDetail.clear();
    };
    P.btnRefreshHistory = function (b) {
        HRM.busy(b, function () { return HRM.get(API + 'doc-types').then(function (o) { HRM.fill('CmbDocumentTypeHistory', o.docTypes, 'Id', 'DocumentTypeDescription', { zero: '' }); }).catch(HRM.fail); });
    };
    P.btnShow = function (b) {                                              // bindHistory
        HRM.busy(b, function () {
            var body = PPC.hist(null, { refDocTypeId: HRM.comboVal('CmbDocumentTypeHistory') });
            return HRM.post(API + 'history', body).then(function (rows) { DataGridHistory.set(rows); GrdHistoryDetail.clear(); }).catch(HRM.fail);
        });
    };

    // ------------------------------------------------------------------ keys
    function onForm() { return tabs.current() === 'form'; }
    function can(id) { return HRM.visible(id) && !PPC.$(id).disabled; }
    function focusedGrid() {
        var a = document.activeElement, w = a && a.closest ? a.closest('[data-grid-tag]') : null;
        return w ? w.getAttribute('data-grid-tag') : '';
    }
    HRM.keys({
        'ctrl+s': function () { if (onForm()) { if (can('btnsave')) P.btnsave(PPC.$('btnsave')); } else P.btnShow(PPC.$('btnShow')); },
        'ctrl+n': function () { if (onForm()) P.btnnew(); else P.btnResetHistory(); },
        'ctrl+t': function () { tabs.show(OPENED || !onForm() ? 'form' : 'history'); },
        'ctrl+e': function () { if (!OPENED) closeForm(); },
        'esc': function () { if (!OPENED) closeForm(); },
        'ctrl+u': function () { if (can('btnUpdate')) P.btnUpdate(PPC.$('btnUpdate')); },
        'ctrl+p': function () { if (can('Print')) P.print(PPC.$('Print')); },
        'ctrl+d': function () {
            if (!onForm() || S.referred) return;
            var tag = focusedGrid();
            if (tag === 'WagesGrid' && grdwagesDetail.currentIndex() >= 0) addRow(grdwagesDetail, grdwagesDetail.currentIndex(), false);
            if (tag === 'StichingGrid' && grdStiching.currentIndex() >= 0) addRow(grdStiching, grdStiching.currentIndex(), true);
        },
        'ctrl+l': function () { if (onForm()) loadData(null); }
    });
    PPC.guard('txtFromDocNoHistory', 'int'); PPC.guard('txtToDocNoHistory', 'int');
    HRM.footer(function () { if (!OPENED) tabs.show('history'); });
    load();
})();
