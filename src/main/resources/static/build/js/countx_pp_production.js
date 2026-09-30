/* ============================================================================================
 * countx_pp_production.js - 677 Production (Party Processing)
 * Desktop: Architecture.WinApp.PartyProcessing.frmProductionPartyProcessing.cs. Exports window.PpPr.
 * Input (117): job order leave (WIP item / stock party), issuance loader, grdInput_CellUpdated, delete rows, InsertInput,
 *   ReadByIdInput, Delete, history. Output (118): detail entry (add / edit / update / delete), CalculateGrossWeight,
 *   CalculateNetWeightOutPut, job order totals, OutPutInsert, ReadByIdOutPut, Delete, history. Packing Material (119): PMInsert,
 *   ReadByIdPM, Delete, history, 601_02 print. Transaction History with the 608 / 609 prints. Wages bill opens after an
 *   input / output save when compulsory and active (as the desktop's ShowDialog).
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/production/';
    var P = {}; window.PpPr = P;
    var S = { rights: {}, inId: 0, outId: 0, pmId: 0, inRemoved: [], outRemoved: [], outUpd: -1, items: [], pmItems: [], defaultDays: 0 };
    var main = PPC.tabs(function (t) { focusTab(t); });
    var inTabs = PPC.subTabs('in'), outTabs = PPC.subTabs('out');
    function focusTab(t) { HRM.focus(t === 'input' ? 'DocDateInput' : t === 'output' ? 'DocDateOutPut' : t === 'pm' ? 'txtdocdatePM' : 'trFrom'); }
    function can(id) { return HRM.visible(id) && !PPC.$(id).disabled; }
    var n3 = function (k, c) { return PPC.numCol(k, c, '#,##0.###', { sum: true }); };

    // ------------------------------------------------------------------ grids
    var inSaveMode = function () { return can('btnSaveInput'); };
    var grdInput = new HRM.Grid('grdInput', { emptyText: '', totals: true, filterRow: true,
        columns: [PPC.btnCol('Delete', 'X', 30), { key: 'Id', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocIdNo', hidden: true },
            { key: 'RefDocSubIdNo', hidden: true }, { key: 'DocDate', hidden: true }, { key: 'DocNo', hidden: true }, { key: 'StockPartyId', hidden: true },
            { key: 'StockParty', caption: 'StockParty' }, { key: 'WareHouseId', hidden: true }, { key: 'WareHouse', caption: 'WareHouse' },
            { key: 'ItemId', hidden: true }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'Item', caption: 'Item' }, { key: 'CropYearId', hidden: true },
            { key: 'CropYear', caption: 'CropYear' }, { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot' }, { key: 'PackingTypeId', hidden: true },
            { key: 'PackingType', caption: 'PackingType' }, { key: 'ItemUOMId', hidden: true }, { key: 'ItemUOM', caption: 'ItemUOM' },
            { key: 'ItemUOMEquivalent', hidden: true }, { key: 'Quantity', caption: 'Quantity', type: 'edit-num', sum: true, decimals: 3 },
            { key: 'Weight', caption: 'Weight', type: 'edit-num', sum: true, decimals: 3 }, { key: 'BalanceQty', hidden: true }, { key: 'BalanceWeight', hidden: true },
            { key: 'ReferencePartyId', hidden: true }, { key: 'ReferenceParty', caption: 'ReferenceParty' }, { key: 'Remarks', caption: 'Remarks', type: 'edit' },
            { key: 'ActionId', hidden: true }],
        onChange: function (r, key) {                                       // grdInput_CellUpdated
            if (inSaveMode()) {
                if (key === 'Quantity') {
                    var q = HRM.num(r.Quantity), bq = HRM.num(r.BalanceQty), bw = HRM.num(r.BalanceWeight);
                    r.Weight = bw / bq * q;
                    if (bq < q) { r.Quantity = bq; r.Weight = bw; HRM.box('Qty cannot greater than BalanceQty ' + PPC.net(bq) + ' Please check!'); }
                }
                if (key === 'Weight' && HRM.num(r.BalanceWeight) < HRM.num(r.Weight)) {
                    r.Weight = HRM.num(r.BalanceWeight); HRM.box('Weight cannot greater than BalanceWeight ' + PPC.net(r.BalanceWeight) + ' Please check!');
                }
            } else if (key === 'Quantity') r.Weight = HRM.num(r.ItemUOMEquivalent) * HRM.num(r.Quantity);
            grdInput.draw();
        } });
    PPC.onButton(grdInput, 'grdInput', function (r, act, i) {               // grdInput_ColumnButtonClick
        if (act !== 'Delete') return;
        if (HRM.int(r.Id) > 0) S.inRemoved.push(Object.assign({}, r));
        grdInput.remove(i);
    });
    var grdOutPut = new HRM.Grid('grdOutPut', { emptyText: '', totals: true, filterRow: true,
        columns: [PPC.btnCol('Delete', 'X', 30), PPC.btnCol('Edit', 'Edit'), { key: 'Id', hidden: true }, { key: 'EntryType', caption: 'EntryType' },
            { key: 'WareHouseId', hidden: true }, { key: 'WareHouse', caption: 'WareHouse' }, { key: 'ItemId', hidden: true }, { key: 'ItemCode', caption: 'ItemCode' },
            { key: 'Item', caption: 'Item' }, { key: 'CropYearId', hidden: true }, { key: 'CropYear', caption: 'CropYear' }, { key: 'JobLotId', hidden: true },
            { key: 'JobLot', caption: 'JobLot' }, { key: 'PackingTypeId', hidden: true }, { key: 'PackingType', caption: 'PackingType' }, { key: 'ItemUOMId', hidden: true },
            { key: 'ItemUOM', caption: 'ItemUOM' }, { key: 'ItemUOMEquivalent', hidden: true }, n3('Quantity', 'Quantity'), n3('GrossWeight', 'GrossWeight'),
            PPC.numCol('EbUnit', 'EbUnit', '#,##0.####'), n3('EbTotal', 'EbTotal'), n3('Weight', 'Weight'), { key: 'Remarks', caption: 'Remarks' }],
        onDouble: function (r, i) { editOutput(i); } });
    PPC.onButton(grdOutPut, 'grdOutPut', function (r, act, i) {             // grdByProduct_ColumnButtonClick
        if (act === 'Delete') {
            if (S.outUpd !== -1) { HRM.focus('btnCancelOutPut'); HRM.box('Please Reset the Detail First...'); return; }
            if (HRM.int(r.Id) > 0) S.outRemoved.push(Object.assign({}, r));
            grdOutPut.remove(i);
        }
        if (act === 'Edit') editOutput(i);
    });

    function histGrid(id, withWip, onEdit, onPrint, onSelect) {
        var cols = [PPC.btnCol('Print', 'Print'), PPC.btnCol('Edit', 'Edit'), { key: 'Id', hidden: true }, { key: 'DocDate', caption: 'DocDate', type: 'date' },
            { key: 'DocNo', caption: 'DocNo' }, { key: 'DocumentTypeId', hidden: true }];
        if (withWip) cols = cols.concat([{ key: 'WIPItemId', hidden: true }, { key: 'WIPItemCode', caption: 'WIPItemCode' }, { key: 'WIPItemName', caption: 'WIPItemName' }]);
        cols = cols.concat([{ key: 'InvJobOrderId', hidden: true }, { key: 'InvJobOrderNo', caption: 'InvJobOrderNo' }, { key: 'EntryType', caption: 'EntryType' },
            { key: 'Remarks', caption: 'Remarks' }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' }, { key: 'EntryUserName', caption: 'EntryUserName' },
            { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' }, { key: 'ModifyUserName', caption: 'ModifyUserName' }]);
        var g = new HRM.Grid(id, { filterRow: true, emptyText: 'No record found.', columns: cols,
            onDouble: function (r) { onEdit(r); }, onSelect: onSelect });
        PPC.onButton(g, id, function (r, act, i, b) { if (act === 'Edit') onEdit(r); if (act === 'Print') onPrint(r, b); });
        return g;
    }
    function detailCols(input) {
        var c = [{ key: 'Id', hidden: true }];
        if (input) c = c.concat([{ key: 'StockParty', caption: 'StockParty' }, { key: 'ReferenceParty', caption: 'ReferenceParty' }]);
        else c.push({ key: 'EntryType', caption: 'EntryType' });
        return c.concat([{ key: 'WareHouse', caption: 'WareHouse' }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'Item', caption: 'ItemName' },
            { key: 'CropYear', caption: 'CropYear' }, { key: 'JobLot', caption: 'JobLot' }, { key: 'PackingType', caption: 'PackingType' }, { key: 'ItemUOM', caption: 'ItemUOM' },
            PPC.numCol('Quantity', 'Quantity', '#,##0.###'), PPC.numCol('Weight', 'Weight', '#,##0.###'), { key: 'Remarks', caption: 'Remarks' }]);
    }
    var inDetail = new HRM.Grid('grdInputHistoryDetail', { emptyText: '', columns: detailCols(true) });
    var outDetail = new HRM.Grid('grdOutputHistoryDetail', { emptyText: '', columns: detailCols(false) });
    var trDetail = new HRM.Grid('grdTransactionhistoryDetail', { emptyText: '', columns: detailCols(true).concat([{ key: 'EntryType', caption: 'EntryType' }]) });
    function detailOf(grid, filter) {
        return function (r) {
            HRM.get(API + 'history-detail', { id: HRM.int(r.Id) }).then(function (rows) { grid.set((rows || []).filter(filter || function () { return true; })); }).catch(HRM.fail);
        };
    }
    var inHist = histGrid('grdInputHistoryMain', true,
        function (r) { if (r.EntryType === 'Input') readInput(HRM.int(r.Id)); }, function (r, b) { print601(HRM.int(r.Id), b, true); },
        detailOf(inDetail, function (d) { return d.EntryType === 'Issue'; }));
    var outHist = histGrid('grdOutputHistoryMain', false,
        function (r) { if (r.EntryType === 'Output') readOutput(HRM.int(r.Id)); }, function (r, b) { print601(HRM.int(r.Id), b, true); },
        detailOf(outDetail, function (d) { return d.EntryType !== 'Issue'; }));
    var trHist = histGrid('grdTransactionHistoryMain', false,
        function (r) { if (r.EntryType === 'Input') readInput(HRM.int(r.Id)); else readOutput(HRM.int(r.Id)); },
        function (r, b) { print601(HRM.int(r.Id), b, true); },
        function (r) {
            PPC.showCols(trDetail, ['StockParty', 'ReferenceParty'], r.EntryType === 'Input');
            detailOf(trDetail)(r);
        });
    var grdPM = new HRM.Grid('grdPM', { filterRow: true, emptyText: 'No record found.',
        columns: [PPC.btnCol('Print', 'Print'), PPC.btnCol('Edit', 'Edit'), { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
            { key: 'DocDate', caption: 'DocDate', type: 'date' }, { key: 'DocNo', caption: 'DocNo' }, { key: 'InvJobOrderId', hidden: true },
            { key: 'JobOrderNo', caption: 'JobOrderNo' }, { key: 'WarehouseId', hidden: true }, { key: 'WareHouseName', caption: 'WareHouseName' },
            { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName' }, { key: 'ItemCode', caption: 'ItemCode' }, PPC.numCol('Qty', 'Qty', '#,##0.###'),
            { key: 'Remarks', caption: 'Remarks' }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' }, { key: 'EntryUserName', caption: 'EntryUserName' },
            { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' }, { key: 'ModifyUserName', caption: 'ModifyUserName' }],
        onDouble: function (r) { readPm(HRM.int(r.Id)); } });
    PPC.onButton(grdPM, 'grdPM', function (r, act, i, b) { if (act === 'Edit') readPm(HRM.int(r.Id)); if (act === 'Print') P.printPm(b, HRM.int(r.Id)); });

    // ------------------------------------------------------------------ load / combos
    function fillItems(id, radio, rows) {
        var keep = HRM.comboVal(id);
        HRM.fill(id, rows, 'Id', HRM.checked(radio) ? 'ItemName' : 'ItemCode', { zero: '', attrs: ['ItemCode', 'ItemName'] });
        HRM.setCombo(id, keep);
    }
    function fillLookups(o) {
        ['CmbJobOrderNoInput', 'CmbJobOrderNoOutput', 'cmbJobOrderPM'].forEach(function (id) { HRM.fill(id, o.jobOrders, 'Id', 'PlanCode', { zero: '', keep: true }); });
        HRM.fill('CmbWarehouseOutput', o.warehouses, 'Id', 'WareHouseName', { zero: '', keep: true });
        S.items = o.items || []; fillItems('CmbItemOutput', 'rdbtnItemNameOutPut', S.items);
        HRM.fill('CmbCropYearOutPut', o.cropYears, 'Id', 'CropYear', { zero: '', keep: true });
        HRM.fill('CmbLotOutPut', o.jobLots, 'Id', 'JobLotDescription', { zero: '', keep: true });
        HRM.fill('CmbPackTypeOutPut', o.packingTypes, 'Id', 'PackTypeDesc', { zero: '', keep: true });
        HRM.fill('cmbWarehouse', o.pmWarehouses, 'Id', 'WareHouseName', { zero: '', keep: true });
        S.pmItems = o.pmItems || []; fillItems('cmbItemPM', 'rdbtnItemNamePM', S.pmItems);
    }
    function fillHistoryCombos(o) {
        HRM.fill('CmbJobOrderInputHistory', o.inputHistoryJobOrders, 'Id', 'ReferenceName', { zero: '' });
        HRM.fill('CmbJobOrderHistoryOutPut', o.outputHistoryJobOrders, 'Id', 'ReferenceName', { zero: '' });
        HRM.fill('CmbJobOrderPMHistory', o.pmHistoryJobOrders, 'Id', 'ReferenceName', { zero: '' });
        HRM.fill('CmbJobOrderTransactionHistory', o.summaryJobOrders, 'Id', 'ReferenceName', { zero: '' });
    }
    function histDates(p, days) { HRM.setVal(p + 'From', HRM.addDays(HRM.today(), -(days > 0 ? days : 3))); HRM.setVal(p + 'To', HRM.today()); }
    function load() {                                                       // frmFoodProduction_Load
        ['DocDateInput', 'DocDateOutPut', 'txtdocdatePM'].forEach(function (id) { HRM.setVal(id, HRM.today()); });
        HRM.fillFixed('CmbEntryType', [[1, 'ByProduct'], [2, 'FinishGoods']], { zero: '' });
        grdInput.set([]); grdOutPut.set([]);
        return HRM.loading(HRM.get(API + 'setup').then(function (o) {
            S.rights = o.rights || {}; S.defaultDays = HRM.int(o.defaultDays);
            HRM.applyRights(S.rights, { save: ['btnSaveInput', 'btnSaveOutPut'], update: ['btnUpdateInput', 'btnUpdateOutPut'],
                delete: ['BtnDeleteInput', 'BtnDeleteOutput'], print: ['BtnPrintInput', 'btnPrintOutPut'] });
            HRM.fill('CmbLanguage', o.languages, 'Id', 'LanguageDescription', { zero: '' });
            HRM.setVal('DocNoInput', o.docNoInput); HRM.setVal('txtDocNoOutPut', o.docNoOutput); HRM.setVal('txtDocNoPM', o.docNoPm);
            fillLookups(o); fillHistoryCombos(o);
            histDates('in', S.defaultDays); histDates('out', S.defaultDays); histDates('pm', S.defaultDays); histDates('tr', 3);
            HRM.focus('DocDateInput');
        }).catch(HRM.fail));
    }
    P.refresh = function (b) { HRM.busy(b, function () { return HRM.get(API + 'refresh').then(fillLookups).catch(HRM.fail); }); };
    P.refreshHistoryCombos = function (b) { HRM.busy(b, function () { return HRM.get(API + 'history-combos').then(fillHistoryCombos).catch(HRM.fail); }); };
    function codes() {
        return HRM.get(API + 'codes').then(function (c) { return c; });
    }

    // ------------------------------------------------------------------ INPUT
    function jobOrderLeave() {                                          // CmbJobOrderNo_Leave
        return HRM.get(API + 'job-order', { id: HRM.comboVal('CmbJobOrderNoInput') }).then(function (o) {
            HRM.fill('CmbWIPItemInput', o.wipItems, 'WipItemId', 'ItemName', { zero: false });
            HRM.fill('CmbStockPartyWIPInput', o.stockParties, 'StockPartyId', 'StockParty', { zero: false });
        }).catch(HRM.fail);
    }
    P.loadInput = function () {                                             // btnLoadInvoices_Click / LoadDataDetailfromPurchaseInvoivce
        if (!HRM.comboVal('CmbJobOrderNoInput')) { HRM.focus('CmbJobOrderNoInput'); HRM.box('JobOrder select First'); return; }
        if (!HRM.comboVal('CmbStockPartyWIPInput')) { HRM.focus('CmbStockPartyWIPInput'); HRM.box('Stock Party Account select First'); return; }
        PPC.loader({ page: 'production', stockPartyId: HRM.comboVal('CmbStockPartyWIPInput'), onLoad: function (rows) {
            if (!rows.length) return;
            var sp = HRM.int(HRM.col(rows[0], 'StockPartyId'));
            if (grdInput.rows().some(function (g) { return HRM.int(g.StockPartyId) !== sp; })) { HRM.box('Data against another Stock Party Already Exist'); return; }
            rows.forEach(function (d) {
                var c = function (k) { return HRM.col(d, k); };
                var dup = grdInput.rows().some(function (g) {
                    return HRM.int(c('RefDocumentTypeId')) === HRM.int(g.RefDocumentTypeId) && HRM.int(c('RefDocIdNo')) === HRM.int(g.RefDocIdNo) && HRM.int(c('RefDocSubIdNo')) === HRM.int(g.RefDocSubIdNo);
                });
                if (dup) return;
                grdInput.data.push({ Id: 0, RefDocumentTypeId: c('RefDocumentTypeId'), RefDocIdNo: c('RefDocIdNo'), RefDocSubIdNo: c('RefDocSubIdNo'),
                    DocDate: HRM.fmtDate(c('DocDate')), DocNo: c('DocCodeNo'), StockPartyId: c('StockPartyId'), StockParty: c('StockParty'),
                    WareHouseId: c('WareHouseId'), WareHouse: c('WareHouseCode'), ItemId: c('ItemId'), ItemCode: c('ItemCode'), Item: c('ItemName'),
                    CropYearId: c('CropYearId'), CropYear: c('CropYear'), JobLotId: c('JobLotId'), JobLot: c('JobLotCode'), PackingTypeId: c('InvPackingTypeId'),
                    PackingType: c('PackingType'), ItemUOMId: c('ItemUomId'), ItemUOM: c('PackUom'), ItemUOMEquivalent: c('Equivalent'),
                    Quantity: c('QtyBalance'), Weight: c('WeightBalance'), BalanceQty: c('QtyBalance'), BalanceWeight: c('WeightBalance'),
                    ReferencePartyId: c('SupplierCustomerId'), ReferenceParty: c('ReferencePartyName'), Remarks: '', ActionId: 1 });
            });
            grdInput.draw();
        } });
    };
    function resetInput() {                                                 // ResetInputMain
        S.inId = 0; S.inRemoved = [];
        HRM.setCombo('CmbJobOrderNoInput', 0);
        HRM.fill('CmbWIPItemInput', [], 'WipItemId', 'ItemName', { zero: false }); HRM.fill('CmbStockPartyWIPInput', [], 'StockPartyId', 'StockParty', { zero: false });
        grdInput.set([]);
        HRM.show('btnSaveInput', true); HRM.show('btnUpdateInput', false); HRM.show('BtnDeleteInput', false);
        HRM.enable('CmbJobOrderNoInput', true);
        return Promise.all([codes(), HRM.get(API + 'refresh')]).then(function (a) {
            HRM.setVal('DocNoInput', a[0].docNoInput);
            HRM.fill('CmbJobOrderNoInput', a[1].jobOrders, 'Id', 'PlanCode', { zero: '' });
        }).catch(HRM.fail);
    }
    P.newInput = function () { resetInput(); };
    P.saveInput = function (btn, update) {                                  // btnsave_Click / btnUpdate_Click -> InsertInput
        if (update && !S.inId) { HRM.box('Rec Id Not Found....'); return; }
        if (!update) S.inId = 0;
        if (!grdInput.rows().length) { HRM.box('Grd Record Not Found.\nEnter Detail First ...'); return; }
        var dn = HRM.val('DocNoInput').trim();
        if (dn === '' || dn === '0') { HRM.box('document Number Field Required'); return; }
        if (!HRM.comboVal('CmbJobOrderNoInput')) { HRM.box('Job Order Number Field Required'); HRM.focus('CmbJobOrderNoInput'); return; }
        if (!HRM.ask(S.inId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { id: S.inId, docNo: dn, docDate: HRM.val('DocDateInput'), jobOrderId: HRM.comboVal('CmbJobOrderNoInput'), jobOrderText: HRM.comboText('CmbJobOrderNoInput'),
            wipItemId: HRM.comboVal('CmbWIPItemInput'), wipStockPartyId: HRM.comboVal('CmbStockPartyWIPInput'), remarks: HRM.val('txtRemarks'),
            rows: grdInput.rows(), removed: S.inRemoved };
        HRM.busy(btn, function () {
            return HRM.post(API + 'save-input', body).then(function (r) {
                HRM.box(r.message);
                var id = HRM.int(r.id);
                if (r.openWages) HRM.open('/party-processing/wages-bill?refDocTypeId=117&refDocId=' + id + '&grossWeight=' + encodeURIComponent(r.grossWeightTotal));
                return resetInput().then(function () { if (HRM.checked('ChkPrintPreviewInput')) print601(id, null, false); });
            }).catch(HRM.fail);
        });
    };
    function readInput(id) {                                                // ReadByIdInput
        return HRM.loading(HRM.get(API + 'read', { id: id, expect: 'Input' }).then(function (h) {
            S.inId = HRM.int(h.Id); S.inRemoved = [];
            HRM.show('btnSaveInput', false); HRM.show('btnUpdateInput', true); HRM.show('BtnDeleteInput', true);
            HRM.enable('CmbJobOrderNoInput', false);
            main.show('input'); inTabs.show('form');
            HRM.setVal('DocNoInput', h.DocNo); HRM.setVal('DocDateInput', HRM.day(h.DocDate)); HRM.setVal('txtRemarks', h.MainRemarks);
            HRM.fill('CmbJobOrderNoInput', [{ Id: h.InvJobOrderId, PlanCode: h.InvJobOrderNo }], 'Id', 'PlanCode', { zero: false });
            grdInput.set((h.details || []).filter(function (d) { return d.EntryType === 'Issue'; }).map(function (d) { d.DocDate = HRM.fmtDate(d.DocDate); return d; }));
            return jobOrderLeave();
        }).catch(HRM.fail));
    }
    P.deleteInput = function (b) {                                          // BtnDeleteInput_Click
        if (!(S.inId > 0)) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        HRM.busy(b, function () { return HRM.post(API + 'delete?id=' + S.inId + '&expect=Input', {}).then(function (r) { HRM.box(r.message); return resetInput(); }).catch(HRM.fail); });
    };
    function print601(id, btn, fromHistory) { return PPC.print(API + 'print', 'ppc-601_01', { id: id, history: !!fromHistory }, btn); }
    P.printInput = function (b) { print601(S.inId, b, false); };

    // ------------------------------------------------------------------ OUTPUT
    function outTotals() {                                                  // CmbJobOrderNoOutput_Leave
        return HRM.get(API + 'totals', { jobOrderId: HRM.comboVal('CmbJobOrderNoOutput') }).then(function (t) {
            HRM.setVal('txtInPutTotalQty', t.inQty); HRM.setVal('txtInPutTotalWeight', t.inWeight);
            HRM.setVal('txtOutPutTotalQty', t.outQty); HRM.setVal('txtOutPutTotalWeight', t.outWeight);
        }).catch(HRM.fail);
    }
    function outUoms(keep) {                                                // CmbItemOutput_Leave -> bindUomOutPut
        return HRM.get(API + 'uoms', { itemId: HRM.comboVal('CmbItemOutput') }).then(function (rows) {
            HRM.fill('CmbUomOutPut', rows, 'Id', 'UOMCode', { zero: '', attrs: ['Equivalent'] });
            if (keep) HRM.setCombo('CmbUomOutPut', keep);
        }).catch(HRM.fail);
    }
    function grossCalc() {                                                  // CalculateGrossWeight
        var row = HRM.comboVal('CmbUomOutPut') ? HRM.comboRow('CmbUomOutPut', 'Id') : null;
        var uom = row ? HRM.num(HRM.col(row, 'Equivalent')) : 0;
        if (can('btnSaveOutPut')) { HRM.setVal('txtGrossWeight', PPC.fmt(PPC.dbl('txtQtyOutPut') * uom, '#,##0.###')); netCalc(); }
    }
    function netCalc(active) {                                              // CalculateNetWeightOutPut
        var q = PPC.dbl('txtQtyOutPut'), g = PPC.dbl('txtGrossWeight'), ebU = PPC.dbl('txtEbUnit'), ebT = PPC.dbl('txtEbTotal');
        if (active === 'EbUnit') { ebT = ebU * q; HRM.setVal('txtEbTotal', PPC.fmt(ebT, '#,##0.###')); }
        else if (active === 'EbTotal') { ebU = ebT / q; HRM.setVal('txtEbUnit', isFinite(ebU) ? PPC.fmt(ebU, '#,##0.####') : ''); }
        else { ebU = q > 0 ? ebT / q : 0; HRM.setVal('txtEbUnit', PPC.fmt(ebU, '#,##0.####')); }
        HRM.setVal('txtNetWeightOutPut', PPC.fmt(g - ebT, '#,##.###'));
    }
    function outDetailCheck() {                                             // FormValidationOfDetailPortionOutPut
        var c = [['CmbEntryType', 'EntryType Field Required'], ['CmbWarehouseOutput', 'Ware house Field Required'], ['CmbItemOutput', 'Item Field Field Required'],
            ['CmbCropYearOutPut', 'CropYear Field Required'], ['CmbLotOutPut', 'Job Lot Field Required'], ['CmbPackTypeOutPut', 'Packing Type Field Required'],
            ['CmbUomOutPut', 'UOM Field Required']];
        for (var i = 0; i < c.length; i++) if (!HRM.comboVal(c[i][0])) { HRM.box(c[i][1]); HRM.focus(c[i][0]); return false; }
        if (HRM.val('txtQtyOutPut').trim() === '' || PPC.dbl('txtQtyOutPut') === 0) { HRM.box('Bag Quantity Field Required'); HRM.focus('txtQtyOutPut'); return false; }
        if (HRM.val('txtNetWeightOutPut').trim() === '' || PPC.dbl('txtNetWeightOutPut') === 0) { HRM.box('Unit Weight Field Required'); HRM.focus('txtNetWeightOutPut'); return false; }
        return true;
    }
    function outRow(r) {
        var it = HRM.comboRow('CmbItemOutput', 'Id') || {}, u = HRM.comboRow('CmbUomOutPut', 'Id') || {};
        r = r || { Id: 0 };
        r.EntryType = HRM.comboText('CmbEntryType').trim(); r.WareHouseId = HRM.comboVal('CmbWarehouseOutput'); r.WareHouse = HRM.comboText('CmbWarehouseOutput').trim();
        r.ItemId = HRM.comboVal('CmbItemOutput'); r.ItemCode = HRM.str(HRM.col(it, 'ItemCode')); r.Item = HRM.str(HRM.col(it, 'ItemName'));
        r.CropYearId = HRM.comboVal('CmbCropYearOutPut'); r.CropYear = HRM.comboText('CmbCropYearOutPut').trim();
        r.JobLotId = HRM.comboVal('CmbLotOutPut'); r.JobLot = HRM.comboText('CmbLotOutPut').trim();
        r.PackingTypeId = HRM.comboVal('CmbPackTypeOutPut'); r.PackingType = HRM.comboText('CmbPackTypeOutPut').trim();
        r.ItemUOMId = HRM.comboVal('CmbUomOutPut'); r.ItemUOM = HRM.str(HRM.col(u, 'UOMCode')); r.ItemUOMEquivalent = HRM.num(HRM.col(u, 'Equivalent'));
        r.Quantity = PPC.dbl('txtQtyOutPut'); r.GrossWeight = PPC.dbl('txtGrossWeight'); r.EbUnit = PPC.dbl('txtEbUnit'); r.EbTotal = PPC.dbl('txtEbTotal');
        r.Weight = PPC.dbl('txtNetWeightOutPut'); r.Remarks = HRM.val('txtRemarksOutPut').trim();
        return r;
    }
    P.resetDetailOutput = function () {                                    // resetDetailOutPut
        S.outUpd = -1;
        ['CmbEntryType', 'CmbWarehouseOutput', 'CmbItemOutput', 'CmbCropYearOutPut', 'CmbLotOutPut', 'CmbPackTypeOutPut', 'CmbUomOutPut'].forEach(function (id) { HRM.setCombo(id, 0); });
        ['txtQtyOutPut', 'txtGrossWeight', 'txtEbUnit', 'txtEbTotal', 'txtNetWeightOutPut', 'txtRemarksOutPut'].forEach(function (id) { HRM.setVal(id, ''); });
        HRM.show('btnAddOutPut', true); HRM.show('btnUpdateDetailOutPut', false); HRM.show('btnCancelOutPut', false);
        HRM.focus('CmbEntryType');
    };
    P.addOutput = function () { if (!outDetailCheck()) return; grdOutPut.add(outRow()); P.resetDetailOutput(); HRM.focus('CmbEntryType'); };
    function editOutput(i) {                                                // grdByProduct_DoubleClick
        var r = grdOutPut.rows()[i]; if (!r) return;
        S.outUpd = i;
        HRM.setComboText('CmbEntryType', r.EntryType);
        HRM.setCombo('CmbWarehouseOutput', r.WareHouseId);
        HRM.setCombo('CmbItemOutput', r.ItemId);
        outUoms(r.ItemUOMId).then(function () {
            HRM.setComboText('CmbCropYearOutPut', r.CropYear);
            HRM.setCombo('CmbLotOutPut', r.JobLotId); HRM.setCombo('CmbPackTypeOutPut', r.PackingTypeId);
            HRM.setVal('txtQtyOutPut', PPC.net(r.Quantity));
            HRM.setVal('txtGrossWeight', PPC.fmt(r.GrossWeight, '#,##0.###')); HRM.setVal('txtEbUnit', PPC.fmt(r.EbUnit, '#,##0.####'));
            HRM.setVal('txtEbTotal', PPC.fmt(r.EbTotal, '#,##0.####')); HRM.setVal('txtNetWeightOutPut', PPC.fmt(r.Weight, '#,##0.###'));
            HRM.setVal('txtRemarksOutPut', r.Remarks);
            HRM.show('btnAddOutPut', false); HRM.show('btnUpdateDetailOutPut', true); HRM.show('btnCancelOutPut', true);
            HRM.focus('CmbEntryType');
        });
    }
    P.updateDetailOutput = function () {                                    // btnUpdateDetailOutPut_Click
        if (!outDetailCheck()) return;
        var r = grdOutPut.rows()[S.outUpd]; if (r) grdOutPut.update(S.outUpd, outRow(r));
        P.resetDetailOutput();
    };
    function resetOutput() {                                                // resetOutPut
        S.outId = 0; S.outRemoved = []; S.outUpd = -1;
        HRM.show('btnSaveOutPut', true); HRM.show('btnUpdateOutPut', false); HRM.show('BtnDeleteOutput', false);
        HRM.enable('CmbJobOrderNoOutput', true);
        grdOutPut.set([]);
        return Promise.all([codes(), HRM.get(API + 'refresh')]).then(function (a) {
            HRM.setVal('txtDocNoOutPut', a[0].docNoOutput);
            HRM.fill('CmbJobOrderNoOutput', a[1].jobOrders, 'Id', 'PlanCode', { zero: '' });
        }).catch(HRM.fail);
    }
    P.newOutput = function () { resetOutput(); P.resetDetailOutput(); };
    P.saveOutput = function (btn, update) {                                 // OutPutInsert
        if (update && !S.outId) { HRM.box('Rec Id not found'); return; }
        if (!update) S.outId = 0;
        if (!grdOutPut.rows().length) { HRM.box('Grd Record Not Found.\nEnter Detail First ...'); return; }
        var dn = HRM.val('txtDocNoOutPut').trim();
        if (dn === '' || dn === '0') { HRM.box('DocNo Field Required'); return; }
        if (!HRM.comboVal('CmbJobOrderNoOutput')) { HRM.box('Job Order Number Field Required'); HRM.focus('CmbJobOrderNoOutput'); return; }
        if (!HRM.ask(S.outId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { id: S.outId, docNo: dn, docDate: HRM.val('DocDateOutPut'), jobOrderId: HRM.comboVal('CmbJobOrderNoOutput'),
            jobOrderText: HRM.comboText('CmbJobOrderNoOutput'), remarks: HRM.val('txtRemarksHeaderOutPut'), rows: grdOutPut.rows(), removed: S.outRemoved };
        HRM.busy(btn, function () {
            return HRM.post(API + 'save-output', body).then(function (r) {
                HRM.box(r.message);
                var id = HRM.int(r.id);
                if (r.openWages) HRM.open('/party-processing/wages-bill?refDocTypeId=118&refDocId=' + id + '&grossWeight=' + encodeURIComponent(r.grossWeightTotal));
                return resetOutput().then(function () { P.resetDetailOutput(); if (HRM.checked('ChkPrintPreviewOutPut')) print601(id, null, false); });
            }).catch(HRM.fail);
        });
    };
    function readOutput(id) {                                               // ReadByIdOutPut
        return HRM.loading(HRM.get(API + 'read', { id: id, expect: 'Output' }).then(function (h) {
            S.outId = HRM.int(h.Id); S.outRemoved = [];
            HRM.show('btnSaveOutPut', false); HRM.show('btnUpdateOutPut', true); HRM.show('BtnDeleteOutput', true);
            main.show('output'); outTabs.show('form');
            HRM.setVal('txtDocNoOutPut', h.DocNo); HRM.setVal('DocDateOutPut', HRM.day(h.DocDate));
            HRM.fill('CmbJobOrderNoOutput', [{ Id: h.InvJobOrderId, PlanCode: h.InvJobOrderNo }], 'Id', 'PlanCode', { zero: false });
            HRM.setVal('txtRemarksHeaderOutPut', h.MainRemarks);
            grdOutPut.set((h.details || []).filter(function (d) { return d.EntryType !== 'Issue'; }));
            return outTotals();
        }).catch(HRM.fail));
    }
    P.deleteOutput = function (b) {                                         // BtnDeleteOutput_Click
        if (!(S.outId > 0)) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        HRM.busy(b, function () { return HRM.post(API + 'delete?id=' + S.outId + '&expect=Output', {}).then(function (r) { HRM.box(r.message); return resetOutput(); }).catch(HRM.fail); });
    };
    P.printOutput = function (b) { print601(S.outId, b, false); };

    // ------------------------------------------------------------------ PACKING MATERIAL
    function resetPm() {                                                    // FormResetOfPM
        S.pmId = 0;
        HRM.show('btnSavePM', true); HRM.show('btnUpdatePM', false); HRM.show('BtnDeletePM', false);
        HRM.setCombo('cmbItemPM', 0); HRM.setCombo('cmbWarehouse', 0); HRM.setVal('txtPMQTY', ''); HRM.setVal('txtremarksPM', '');
        HRM.focus('cmbJobOrderPM');
        return codes().then(function (c) { HRM.setVal('txtDocNoPM', c.docNoPm); }).catch(HRM.fail);
    }
    P.newPm = function () { resetPm(); };
    P.savePm = function (btn, update) {                                     // PMInsert
        if (update && !S.pmId) { HRM.box('Rec Id not found'); return; }
        if (!update) S.pmId = 0;
        if (!HRM.comboVal('cmbJobOrderPM')) { HRM.box('Product No. Field Required'); HRM.focus('cmbJobOrderPM'); return; }
        if (!HRM.comboVal('cmbItemPM')) { HRM.box(' Item Field Required'); HRM.focus('cmbItemPM'); return; }
        if (!HRM.comboVal('cmbWarehouse')) { HRM.box('Warehouse  Field Required'); HRM.focus('cmbWarehouse'); return; }
        if (HRM.val('txtPMQTY').trim() === '' || PPC.dbl('txtPMQTY') === 0) { HRM.box(' Qty Field Required'); HRM.focus('txtPMQTY'); return; }
        if (!HRM.ask(S.pmId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { id: S.pmId, docNo: HRM.val('txtDocNoPM'), docDate: HRM.val('txtdocdatePM'), jobOrderId: HRM.comboVal('cmbJobOrderPM'),
            warehouseId: HRM.comboVal('cmbWarehouse'), itemId: HRM.comboVal('cmbItemPM'), qty: HRM.val('txtPMQTY'), remarks: HRM.val('txtremarksPM') };
        HRM.busy(btn, function () { return HRM.post(API + 'save-pm', body).then(function (r) { HRM.box(r.message); return resetPm(); }).catch(HRM.fail); });
    };
    function readPm(id) {                                                   // ReadByIdPM
        return HRM.loading(HRM.get(API + 'read-pm', { id: id }).then(function (h) {
            S.pmId = HRM.int(h.Id);
            HRM.show('btnSavePM', false); HRM.show('btnUpdatePM', true); HRM.show('BtnDeletePM', true);
            main.show('pm');
            HRM.setVal('txtdocdatePM', HRM.day(h.DocDate)); HRM.setVal('txtDocNoPM', h.DocNo);
            HRM.setCombo('cmbJobOrderPM', h.InvJobOrderId); HRM.setCombo('cmbWarehouse', h.WarehouseId); HRM.setCombo('cmbItemPM', h.ItemId);
            HRM.setVal('txtPMQTY', h.Qty); HRM.setVal('txtremarksPM', h.Remarks);
            HRM.focus('cmbWarehouse');
        }).catch(HRM.fail));
    }
    P.deletePm = function (b) {                                             // BtnDeletePM_Click
        if (!(S.pmId > 0)) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        HRM.busy(b, function () { return HRM.post(API + 'delete-pm?id=' + S.pmId, {}).then(function (r) { HRM.box(r.message); return resetPm(); }).catch(HRM.fail); });
    };
    P.printPm = function (b, id) {                                          // GenerateReportPM
        var rid = id || S.pmId;
        HRM.busy(b, function () {
            return HRM.get(API + 'pm-print', { id: rid }).then(function (g) {
                if (window.CrystalPrint) return window.CrystalPrint.open('ppc-601_02', { id: rid });
            }).catch(HRM.fail);
        });
    };

    // ------------------------------------------------------------------ histories / reports
    function histBody(p) {
        return PPC.hist({ from: p + 'From', fromChk: p + 'FromChk', to: p + 'To', toChk: p + 'ToChk', docFrom: p + 'DocFrom', docTo: p + 'DocTo', radio: p + 'Mode' });
    }
    var HIST = { in: ['CmbJobOrderInputHistory', 117, inHist, inDetail], out: ['CmbJobOrderHistoryOutPut', 118, outHist, outDetail],
        pm: ['CmbJobOrderPMHistory', 119, grdPM, null], tr: ['CmbJobOrderTransactionHistory', 0, trHist, trDetail] };
    P.showHistory = function (p, b) {
        var h = HIST[p];
        HRM.busy(b, function () {
            var body = histBody(p); body.jobOrderId = HRM.comboVal(h[0]);
            var url = p === 'pm' ? API + 'pm-history' : API + 'history?docType=' + h[1];
            return HRM.post(url, body).then(function (rows) { h[2].set(rows); if (h[3]) h[3].clear(); }).catch(HRM.fail);
        });
    };
    P.newHistory = function (p) {
        var h = HIST[p];
        histDates(p, p === 'tr' ? 3 : S.defaultDays);
        HRM.setVal(p + 'DocFrom', ''); HRM.setVal(p + 'DocTo', ''); HRM.setCombo(h[0], 0);
        var r = document.querySelector('input[name="' + p + 'Mode"][value="doc"]'); if (r) r.checked = true;
        h[2].clear(); if (h[3]) h[3].clear();
    };
    P.report = function (which, b) {                                        // btnSummeryReport_Click / btn609ProductionIssuance_Click
        var jo = HRM.comboVal('CmbJobOrderTransactionHistory'), lang = HRM.comboVal('CmbLanguage');
        HRM.busy(b, function () {
            return HRM.get(API + 'report-check', { which: which, jobOrderId: jo, languageId: lang }).then(function () {
                if (window.CrystalPrint) return window.CrystalPrint.open(which === '608' ? 'ppc-608' : 'ppc-609', { jobOrderId: jo, languageId: lang || '' });
            }).catch(HRM.fail);
        });
    };
    P.shortcuts = function () {
        PPC.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'],
            ['Ctrl+P', 'For Print'], ['Ctrl+L', 'For Load Input'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form']]);
    };

    // ------------------------------------------------------------------ events / keys
    PPC.$('CmbJobOrderNoInput').addEventListener('change', function () { jobOrderLeave(); });
    PPC.$('CmbJobOrderNoOutput').addEventListener('change', outTotals);
    PPC.$('CmbItemOutput').addEventListener('change', function () { outUoms(0); });
    PPC.$('CmbUomOutPut').addEventListener('change', grossCalc);
    PPC.$('txtQtyOutPut').addEventListener('input', grossCalc);
    PPC.$('txtGrossWeight').addEventListener('input', function () { netCalc(); });
    PPC.$('txtEbUnit').addEventListener('input', function () { netCalc('EbUnit'); });
    PPC.$('txtEbTotal').addEventListener('input', function () { netCalc('EbTotal'); });
    document.querySelectorAll('input[name="rdOut"]').forEach(function (r) { r.addEventListener('change', function () { fillItems('CmbItemOutput', 'rdbtnItemNameOutPut', S.items); }); });
    document.querySelectorAll('input[name="rdPm"]').forEach(function (r) { r.addEventListener('change', function () { fillItems('cmbItemPM', 'rdbtnItemNamePM', S.pmItems); }); });
    ['txtQtyOutPut', 'txtGrossWeight', 'txtEbUnit', 'txtEbTotal', 'txtNetWeightOutPut', 'txtPMQTY'].forEach(function (id) { PPC.guard(id, 'dec'); });
    ['in', 'out', 'pm', 'tr'].forEach(function (p) { PPC.guard(p + 'DocFrom', 'int'); PPC.guard(p + 'DocTo', 'int'); });
    function cur() { return main.current(); }
    HRM.keys({
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+t': function () {
            var t = cur();
            if (t === 'input') inTabs.show(inTabs.current() === 'form' ? 'history' : 'form');
            else if (t === 'output') outTabs.show(outTabs.current() === 'form' ? 'history' : 'form');
        },
        'ctrl+s': function () {
            var t = cur();
            if (t === 'input' && inTabs.current() === 'form' && can('btnSaveInput')) P.saveInput(PPC.$('btnSaveInput'), false);
            if (t === 'output' && outTabs.current() === 'form' && can('btnSaveOutPut')) P.saveOutput(PPC.$('btnSaveOutPut'), false);
            if (t === 'pm' && can('btnSavePM')) P.savePm(PPC.$('btnSavePM'), false);
        },
        'ctrl+u': function () {
            var t = cur();
            if (t === 'input' && can('btnUpdateInput')) P.saveInput(PPC.$('btnUpdateInput'), true);
            if (t === 'output' && can('btnUpdateOutPut')) P.saveOutput(PPC.$('btnUpdateOutPut'), true);
            if (t === 'pm' && can('btnUpdatePM')) P.savePm(PPC.$('btnUpdatePM'), true);
        },
        'ctrl+n': function () { var t = cur(); if (t === 'input') P.newInput(); if (t === 'output') P.newOutput(); if (t === 'pm') P.newPm(); },
        'ctrl+l': function () { if (cur() === 'input') P.loadInput(); }
    });
    HRM.footer(function () {
        var t = cur();
        if (t === 'input') inTabs.show('history'); else if (t === 'output') outTabs.show('history'); else main.show('trans');
    });
    load();
})();
