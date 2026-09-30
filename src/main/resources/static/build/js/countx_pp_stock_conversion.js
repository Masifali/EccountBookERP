/* ============================================================================================
 * countx_pp_stock_conversion.js - 602 Stock Conversion (Party Processing)
 * Desktop: Architecture.WinApp.PartyProcessing.invfrmStockConversionPartyProcessing.cs. Exports window.PpSc.
 * Load, stock party change (reference parties, job orders), detail entry for the output grid (the entry-type list has only
 * the two recovery types, so inputs come from the issuance loader), grid edits, PM grid, GenerateSummaryForUser, Insert
 * (server; opens the wages bill after the save when compulsory and active), ReadById, Delete, history, keys.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/stock-conversion/';
    var P = {}; window.PpSc = P;
    var S = { rights: {}, recId: 0, updIndex: -1, updGrid: null, removed: [], pmItems: [], pmWarehouses: [], defaultDays: 0 };
    var tabs = PPC.tabs(function (t) { HRM.focus(t === 'history' ? 'FromDateHistory' : 'txtDocdate'); });
    var gtabs = PPC.subTabs('grids');

    function saveMode() { return HRM.visible('btnsave') && !PPC.$('btnsave').disabled; }
    function refRow(r) { return HRM.int(r.RefDocNoId) > 0 || HRM.int(r.RefDocSubIdNo) > 0 || HRM.int(r.RefDocumentTypeId) > 0; }
    var baseCols = [
        { key: 'JobOrderId', hidden: true }, { key: 'JobOrder', caption: 'JobOrder' }, { key: 'ReferencePartyId', hidden: true },
        { key: 'ReferenceParty', caption: 'ReferenceParty' }, { key: 'EntryType', caption: 'EntryType' }, { key: 'WareHouseId', hidden: true },
        { key: 'WareHouse', caption: 'WareHouse' }, { key: 'ItemId', hidden: true }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'Item', caption: 'Item' },
        { key: 'CropYearId', hidden: true }, { key: 'CropYear', caption: 'CropYear' }, { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot' },
        { key: 'PackingTypeId', hidden: true }, { key: 'PackingType', caption: 'PackingType' }, { key: 'ItemUOMId', hidden: true }, { key: 'PackUom', caption: 'PackUom' },
        { key: 'PackUomEquivalent', hidden: true },
        { key: 'Quantity', caption: 'Quantity', type: 'edit-num', sum: true, decimals: 2 }, { key: 'Weight', caption: 'Weight', type: 'edit-num', sum: true, decimals: 3 },
        { key: 'Remarks', caption: 'Remarks' }];
    var grdInput = new HRM.Grid('grdInput', {
        emptyText: '', totals: true,
        columns: [PPC.btnCol('Delete', 'X', 30), { key: 'Id', hidden: true }, { key: 'RefDocNoId', hidden: true }, { key: 'RefDocSubIdNo', hidden: true },
            { key: 'RefDocumentTypeId', hidden: true }].concat(baseCols).concat([{ key: 'BalQty', hidden: true }, { key: 'BalWeight', hidden: true }]),
        onChange: inputCellUpdated,
        onDouble: function (r, i) { if (!refRow(r)) editRow(grdInput, i); }
    });
    var grdByProduct = new HRM.Grid('grdByProduct', {
        emptyText: '', totals: true,
        columns: [PPC.btnCol('Delete', 'X', 30), { key: 'Id', hidden: true }].concat(baseCols.map(function (c) { return Object.assign({}, c); })),
        onChange: function (r, key) { if (key === 'Quantity') r.Weight = HRM.num(r.Quantity) * HRM.num(r.PackUomEquivalent); grdByProduct.draw(); summary(); },
        onDouble: function (r, i) { editRow(grdByProduct, i); }
    });
    var grdPM = new HRM.Grid('grdPM', {
        emptyText: '',
        columns: [PPC.btnCol('Delete', 'X', 30), PPC.btnCol('Add', '+', 30), { key: 'Id', hidden: true },
            { key: 'ItemId', caption: 'ItemId', type: 'select', options: function () { return [[0, '']].concat(S.pmItems.map(function (x) { return [x.Id, x.ItemName]; })); } },
            { key: 'ItemQTY', caption: 'ItemQTY', type: 'edit-num', decimals: 2 },
            { key: 'WarehouseId', caption: 'WarehouseId', type: 'select', options: function () { return [[0, '']].concat(S.pmWarehouses.map(function (x) { return [x.Id, x.WareHouseName]; })); } }],
        onChange: function (r, key) { if (key === 'ItemQTY') summary(); }
    });
    function removeDetail(grid, i) {                                         // grdInput / grdByProduct _ColumnButtonClick (Delete)
        var r = grid.rows()[i]; if (!r) return;
        if (S.updIndex !== -1) { HRM.box('Reset the Detail First...'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        if (!saveMode() && HRM.int(r.Id) > 0) S.removed.push(Object.assign({}, r, { EntryType: r.EntryType }));
        grid.remove(i);
        if (!grdInput.rows().length && !grdByProduct.rows().length) HRM.enable('CmbStockPartyHeader', true);
        summary();
    }
    PPC.onButton(grdInput, 'grdInput', function (r, act, i) { if (act === 'Delete') removeDetail(grdInput, i); });
    PPC.onButton(grdByProduct, 'grdByProduct', function (r, act, i) { if (act === 'Delete') removeDetail(grdByProduct, i); });
    PPC.onButton(grdPM, 'grdPM', function (r, act, i) {                     // grdPackingMaterial_ColumnButtonClick
        if (act === 'Delete') { grdPM.remove(i); if (!grdPM.rows().length) pmRow(); }
        if (act === 'Add') pmRow();
        summary();
    });
    function pmRow() { grdPM.add({ Id: 0, ItemId: 0, ItemQTY: 0, WarehouseId: 0 }); }

    function inputCellUpdated(r, key) {                                     // grdInput_CellUpdated
        var bq = HRM.num(r.BalQty), bw = HRM.num(r.BalWeight);
        if (saveMode() && refRow(r)) {
            if (key === 'Quantity') {
                var q = HRM.num(r.Quantity);
                r.Weight = bw / bq * q;
                if (bq < q) { r.Quantity = 0; r.Weight = 0; HRM.box('Qty cannot greater than BalanceQty ' + PPC.net(bq) + ' Please check!'); }
            }
            if (key === 'Weight' && bw < HRM.num(r.Weight)) { r.Weight = 0; HRM.box('Weight cannot greater than BalanceWeight ' + PPC.fmt(bw, '#,##0.###') + ' Please check!'); }
        } else if (key === 'Quantity') r.Weight = HRM.num(r.Quantity) * HRM.num(r.PackUomEquivalent);
        grdInput.draw();
        summary();
    }

    var hMain = new HRM.Grid('GrdHistoryMain', {
        filterRow: true, emptyText: 'No record found.',
        columns: [PPC.btnCol('Edit', 'Edit'), PPC.btnCol('Print', 'Print'), { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
            { key: 'DocNo', caption: 'DocNo' }, { key: 'DocDate', caption: 'DocDate', type: 'date' }, { key: 'ProductionNo', caption: 'ProductionNo' },
            { key: 'ManualNo', caption: 'ManualNo' }, { key: 'StockPartyId', hidden: true }, { key: 'StockPartyName', caption: 'StockPartyName' },
            { key: 'Remarks', caption: 'Remarks' }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' }, { key: 'EntryUser', caption: 'EntryUser' },
            { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' }, { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' }],
        onDouble: function (r) { readById(HRM.int(r.Id)); },
        onSelect: function (r) {                                             // HistoryDetailsBind
            HRM.get(API + 'history-detail', { id: HRM.int(r.Id) }).then(function (o) { hDetail.set(o.details || []); hPm.set(o.pm || []); }).catch(HRM.fail);
        }
    });
    PPC.onButton(hMain, 'GrdHistoryMain', function (r, act, i, b) { if (act === 'Edit') readById(HRM.int(r.Id)); else if (act === 'Print') report(HRM.int(r.Id), b, true); });
    var hDetail = new HRM.Grid('grdHistoryDetail', { emptyText: '',
        columns: [{ key: 'Id', hidden: true }, { key: 'JobOrder', caption: 'JobOrder' }, { key: 'ReferencePartyId', hidden: true }, { key: 'ReferenceParty', caption: 'ReferenceParty' },
            { key: 'EntryType', caption: 'EntryType' }, { key: 'WareHouseId', hidden: true }, { key: 'WareHouse', caption: 'WareHouse' }, { key: 'ItemId', hidden: true },
            { key: 'ItemCode', caption: 'ItemCode' }, { key: 'Item', caption: 'Item' }, { key: 'CropYearId', hidden: true }, { key: 'CropYear', caption: 'CropYear' },
            { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot' }, { key: 'PackingTypeId', hidden: true }, { key: 'PackingType', caption: 'PackingType' },
            { key: 'ItemUOMId', hidden: true }, { key: 'PackUom', caption: 'PackUom' }, { key: 'PackUomEquivalent', hidden: true },
            PPC.numCol('Quantity', 'Quantity', '#,##0.##'), PPC.numCol('Weight', 'Weight', '#,##0.##'), { key: 'Remarks', caption: 'Remarks' }] });
    var hPm = new HRM.Grid('grdHistoryPM', { emptyText: '',
        columns: [{ key: 'Item', caption: 'Item' }, PPC.numCol('ItemQTY', 'ItemQTY', '#,##'), { key: 'WareHouse', caption: 'WareHouse' }] });

    // ------------------------------------------------------------------ load / combos
    function fillLookups(o) {
        HRM.fill('CmbStockPartyHeader', o.stockParties, 'Id', 'CompanyName', { zero: '', keep: true });
        HRM.fill('cmbGodown', o.warehouses, 'Id', 'WareHouseName', { zero: '', keep: true });
        HRM.fill('cmbItem', o.items, 'Id', 'ItemName', { zero: '', keep: true, attrs: ['ItemCode'] });
        HRM.fill('CmbCropyr', o.cropYears, 'Id', 'CropYear', { zero: '', keep: true });
        HRM.fill('cmbLot', o.jobLots, 'Id', 'JobLotDescription', { zero: '', keep: true });
        HRM.fill('cmbBagType', o.packingTypes, 'Id', 'PackTypeDesc', { zero: '', keep: true });
    }
    function histDates(days) {
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -(days > 0 ? days : 3)));
        HRM.setVal('ToDateHistory', HRM.today());
    }
    function load() {
        HRM.setVal('txtDocdate', HRM.today());
        HRM.fillFixed('cmbEntryType', [[2, 'Recovery By Product'], [3, 'Recovery Head Rice']], { zero: '' });
        return HRM.loading(HRM.get(API + 'setup').then(function (o) {
            S.rights = o.rights || {}; S.pmItems = o.pmItems || []; S.pmWarehouses = o.pmWarehouses || []; S.defaultDays = HRM.int(o.defaultDays);
            HRM.applyRights(S.rights, { save: 'btnsave', update: 'btnUpdate', delete: 'btnDelete', print: 'Print' });
            HRM.setVal('txtdocnumber', o.docNo);
            fillLookups(o);
            grdInput.set([]); grdByProduct.set([]); grdPM.set([]); pmRow();
            histDates(S.defaultDays);
            HRM.focus('txtProductionNo');
        }).catch(HRM.fail));
    }
    function partyChanged() {                                               // CmbStockPartyHeader_TextChanged + _Leave
        var id = HRM.comboVal('CmbStockPartyHeader');
        if (!id) { HRM.fill('cmbReferenceParty', [], 'SupplierCustomerId', 'ReferencePartyName', { zero: '' }); HRM.fill('CmbJobOrder', [], 'Id', 'PlanCode', { zero: '' }); return Promise.resolve(); }
        return HRM.get(API + 'party', { stockPartyId: id }).then(function (o) {
            var keep = HRM.comboVal('cmbReferenceParty');
            HRM.fill('cmbReferenceParty', o.referenceParties, 'SupplierCustomerId', 'ReferencePartyName', { zero: '' });
            if (keep && HRM.hasOption('cmbReferenceParty', keep)) HRM.setCombo('cmbReferenceParty', keep);
            else if ((o.referenceParties || []).length) HRM.setCombo('cmbReferenceParty', HRM.col(o.referenceParties[0], 'SupplierCustomerId'));
            var jo = HRM.comboVal('CmbJobOrder');
            HRM.fill('CmbJobOrder', o.jobOrders, 'Id', 'PlanCode', { zero: '' });
            if (jo) HRM.setCombo('CmbJobOrder', jo);
            return balance();
        }).catch(HRM.fail);
    }
    function balance() {                                                    // GetBalQtyAndWeightForPartyProcessing
        var sp = HRM.comboVal('CmbStockPartyHeader'), wh = HRM.comboVal('cmbGodown'), it = HRM.comboVal('cmbItem');
        if (!sp || !wh || !it) return Promise.resolve();
        return HRM.get(API + 'balance', { itemId: it, stockPartyId: sp, warehouseId: wh }).then(function (r) {
            HRM.setVal('txtStock', r.qty); HRM.setVal('txtBalanceWeight', r.weight);
        }).catch(HRM.fail);
    }
    function uoms(keepText) {                                               // bindUom
        return HRM.get(API + 'uoms', { itemId: HRM.comboVal('cmbItem') }).then(function (rows) {
            HRM.fill('cmbUOM', rows, 'Id', 'UOMCode', { zero: '', attrs: ['Equivalent'] });
            if (keepText) HRM.setComboText('cmbUOM', keepText);
        }).catch(HRM.fail);
    }
    function netWeight() {                                                  // cmbUOM_Leave / txtQty_TextChanged
        var row = HRM.comboVal('cmbUOM') ? HRM.comboRow('cmbUOM', 'Id') : null;
        var eq = row ? HRM.num(HRM.col(row, 'Equivalent')) : 0, q = PPC.dbl('txtQty');
        HRM.setVal('txtUnitWeight', row && eq > 0 && q > 0 ? PPC.fmt(eq * q, '#,##0.###') : '0');
    }
    function summary() {                                                    // GenerateSummaryForUser
        ['txtInputQuantity', 'txtInputWeight', 'txtOutputByProductQty', 'txtOutputByProductWeight', 'txtFinishGoodsQty', 'txtFinishGoodsWeight', 'txtPackingMaterialQty']
            .forEach(function (id) { HRM.setVal(id, ''); });
        HRM.setVal('txtInputQuantity', PPC.net(grdInput.sum('Quantity')));
        HRM.setVal('txtInputWeight', PPC.net(grdInput.sum('Weight')));
        var rows = grdByProduct.rows();
        rows.forEach(function (r) {
            var t = HRM.str(r.EntryType);
            if (rows.length === 1) {
                if (t === 'Recovery By Product') { HRM.setVal('txtOutputByProductQty', PPC.net(r.Quantity)); HRM.setVal('txtOutputByProductWeight', PPC.net(r.Weight)); }
                else if (t === 'Recovery Head Rice') { HRM.setVal('txtFinishGoodsQty', PPC.net(r.Quantity)); HRM.setVal('txtFinishGoodsWeight', PPC.net(r.Weight)); }
            } else {
                if (t === 'Recovery By Product') {
                    HRM.setVal('txtOutputByProductQty', PPC.net(PPC.dbl('txtOutputByProductQty') + HRM.num(r.Quantity)));
                    HRM.setVal('txtOutputByProductWeight', PPC.net(PPC.dbl('txtOutputByProductWeight') + HRM.num(r.Weight)));
                } else if (t === 'Recovery Head Rice') {
                    HRM.setVal('txtFinishGoodsQty', PPC.net(PPC.dbl('txtFinishGoodsQty') + HRM.num(r.Quantity)));
                    HRM.setVal('txtFinishGoodsWeight', PPC.net(PPC.dbl('txtFinishGoodsWeight') + HRM.num(r.Weight)));
                }
            }
        });
        HRM.setVal('txtPackingMaterialQty', PPC.net(grdPM.sum('ItemQTY')));
    }

    // ------------------------------------------------------------------ detail entry
    function detailCheck() {                                                // FormValidationOfDetailPortion
        var c = [['CmbStockPartyHeader', 'Stock Party Field is Required'], ['cmbReferenceParty', 'Reference Party Field is Required'],
            ['cmbEntryType', 'Entry Type Field Required'], ['cmbGodown', 'Ware house Field Required'], ['cmbItem', 'Item Field Field Required'],
            ['CmbCropyr', 'Crop Year Field Required'], ['cmbLot', 'JobLot Field Required'], ['cmbBagType', 'Packing Type Field Required'], ['cmbUOM', 'UOM Field Required']];
        for (var i = 0; i < c.length; i++) if (!HRM.comboVal(c[i][0])) { HRM.box(c[i][1]); HRM.focus(c[i][0]); return false; }
        var q = HRM.val('txtQty').trim(), w = HRM.val('txtUnitWeight').trim();
        if (q === '' || q === '0') { HRM.box('Quantity Field Required'); HRM.focus('txtQty'); return false; }
        if (w === '' || w === '0') { HRM.box('Weight Field Required'); HRM.focus('txtUnitWeight'); return false; }
        return true;
    }
    function entryRow(r) {
        var it = HRM.comboRow('cmbItem', 'Id') || {}, u = HRM.comboRow('cmbUOM', 'Id') || {};
        r = r || { Id: 0 };
        r.JobOrderId = HRM.comboVal('CmbJobOrder'); r.JobOrder = HRM.comboText('CmbJobOrder').trim();
        r.ReferencePartyId = HRM.comboVal('cmbReferenceParty'); r.ReferenceParty = HRM.comboText('cmbReferenceParty').trim();
        r.EntryType = HRM.comboText('cmbEntryType');
        r.WareHouseId = HRM.comboVal('cmbGodown'); r.WareHouse = HRM.comboText('cmbGodown');
        r.ItemId = HRM.comboVal('cmbItem'); r.ItemCode = HRM.str(HRM.col(it, 'ItemCode')); r.Item = HRM.str(HRM.col(it, 'ItemName'));
        r.CropYearId = HRM.comboVal('CmbCropyr'); r.CropYear = HRM.comboText('CmbCropyr');
        r.JobLotId = HRM.comboVal('cmbLot'); r.JobLot = HRM.comboText('cmbLot');
        r.PackingTypeId = HRM.comboVal('cmbBagType'); r.PackingType = HRM.comboText('cmbBagType');
        r.ItemUOMId = HRM.comboVal('cmbUOM'); r.PackUom = HRM.str(HRM.col(u, 'UOMCode')); r.PackUomEquivalent = HRM.num(HRM.col(u, 'Equivalent'));
        r.Quantity = PPC.dbl('txtQty'); r.Weight = PPC.dbl('txtUnitWeight'); r.Remarks = HRM.val('txtdeailRemarks');
        return r;
    }
    function resetDetail() {                                                // resetDetail
        S.updIndex = -1; S.updGrid = null;
        ['CmbJobOrder', 'cmbGodown', 'cmbEntryType', 'CmbCropyr', 'cmbItem', 'cmbUOM', 'cmbLot', 'cmbBagType'].forEach(function (id) { HRM.setCombo(id, 0); });
        HRM.setVal('txtUnitWeight', ''); HRM.setVal('txtQty', ''); HRM.setVal('txtStock', '0'); HRM.setVal('txtBalanceWeight', '0'); HRM.setVal('txtdeailRemarks', '');
        HRM.focus('CmbJobOrder');
    }
    P.add = function () {                                                   // AddInGrid_Click
        if (!detailCheck()) return;
        if (HRM.comboText('cmbEntryType') === 'Issue') {
            if (grdInput.rows().some(refRow)) { HRM.box('Record cannot add Because Loader Entry Exist!'); return; }
            var r = entryRow(); r.RefDocNoId = 0; r.RefDocSubIdNo = 0; r.RefDocumentTypeId = 0; r.BalQty = 0; r.BalWeight = 0;
            grdInput.add(r);
        } else grdByProduct.add(entryRow());
        resetDetail();
        summary();
        if (grdInput.rows().length || grdByProduct.rows().length) HRM.enable('CmbStockPartyHeader', false);
    };
    function editRow(grid, i) {                                             // grdInput_DoubleClick / grdByProduct_DoubleClick
        var r = grid.rows()[i]; if (!r) return;
        S.updIndex = i; S.updGrid = grid;
        if (HRM.int(r.JobOrderId) > 0) HRM.setCombo('CmbJobOrder', r.JobOrderId);
        HRM.setCombo('cmbReferenceParty', r.ReferencePartyId);
        HRM.setComboText('cmbEntryType', r.EntryType);
        HRM.setCombo('cmbGodown', r.WareHouseId);
        HRM.setCombo('cmbItem', r.ItemId);
        uoms(null).then(function () {
            HRM.setCombo('cmbLot', r.JobLotId); HRM.setCombo('CmbCropyr', r.CropYearId); HRM.setCombo('cmbBagType', r.PackingTypeId);
            HRM.setCombo('cmbUOM', r.ItemUOMId);
            HRM.setVal('txtQty', PPC.net(r.Quantity)); HRM.setVal('txtUnitWeight', PPC.net(r.Weight));
            HRM.setVal('txtRemarks', r.Remarks);                              // the desktop writes the row's remarks into the HEADER remarks box
            HRM.show('Add', false); HRM.show('btnUpdateDetail', true); HRM.show('btnCancelDetail', true);
            if (grid === grdInput) balance();
            HRM.focus('CmbJobOrder');
        });
    }
    P.btnUpdateDetail = function () {                                       // btnUpdateDetail_Click
        if (!detailCheck()) return;
        var grid = HRM.comboText('cmbEntryType') === 'Issue' ? grdInput : grdByProduct;
        var r = grid.rows()[S.updIndex];
        if (r) grid.update(S.updIndex, entryRow(r));
        HRM.show('btnUpdateDetail', false); HRM.show('btnCancelDetail', false); HRM.show('Add', true);
        resetDetail(); summary(); HRM.focus('CmbJobOrder');
    };
    P.btnCancelDetail = function () { HRM.show('Add', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelDetail', false); resetDetail(); };

    P.btnLoadInput = function () {                                          // btnLoadInput_Click / LoadInInputGridDetail
        if (!HRM.comboVal('CmbStockPartyHeader')) { HRM.focus('CmbStockPartyHeader'); HRM.box('Stock Party Account select First'); return; }
        var f = grdInput.rows()[0];
        if (f && !refRow(f)) { HRM.box("You Can't Load Because Direct Entry Already Exist"); return; }
        PPC.loader({ page: 'stock-conversion', stockPartyId: HRM.comboVal('CmbStockPartyHeader'), onLoad: function (rows) {
            if (!rows.length) return;
            if (grdInput.rows().length && HRM.comboVal('CmbStockPartyHeader') !== HRM.int(HRM.col(rows[0], 'StockPartyId'))) {
                HRM.box("Already Loaded Row's Have Different Stock Party. So you Can't Load Rows Of Different Stock Party!"); return;
            }
            rows.forEach(function (d) {
                var c = function (k) { return HRM.col(d, k); };
                var dup = grdInput.rows().some(function (g) {
                    return HRM.int(g.RefDocNoId) === HRM.int(c('RefDocIdNo')) && HRM.int(g.RefDocSubIdNo) === HRM.int(c('RefDocSubIdNo')) && HRM.int(g.RefDocumentTypeId) === HRM.int(c('RefDocumentTypeId'));
                });
                if (dup) return;
                grdInput.data.push({ Id: 0, RefDocNoId: c('RefDocIdNo'), RefDocSubIdNo: c('RefDocSubIdNo'), RefDocumentTypeId: c('RefDocumentTypeId'),
                    JobOrderId: c('JobOrderId'), JobOrder: c('JobOrderNo'), ReferencePartyId: c('SupplierCustomerId'), ReferenceParty: c('ReferencePartyName'),
                    EntryType: 'Issue', WareHouseId: c('WarehouseId'), WareHouse: c('WareHouseCode'), ItemId: c('ItemId'), ItemCode: c('ItemCode'), Item: c('ItemName'),
                    CropYearId: c('CropYearId'), CropYear: c('CropYear'), JobLotId: c('JobLotId'), JobLot: c('JobLotCode'), PackingTypeId: c('InvPackingTypeId'),
                    PackingType: c('PackingType'), ItemUOMId: c('ItemUomId'), PackUom: c('PackUom'), PackUomEquivalent: c('Equivalent'),
                    Quantity: c('QtyBalance'), Weight: c('WeightBalance'), Remarks: '', BalQty: c('QtyBalance'), BalWeight: c('WeightBalance') });
            });
            grdInput.draw();
            HRM.enable('CmbStockPartyHeader', false);
            gtabs.show('input');
            summary();
        } });
    };

    // ------------------------------------------------------------------ save / read / delete
    function refreshForm() {                                                // RefreshForm + summeryreset
        S.recId = 0; S.updIndex = -1; S.removed = [];
        HRM.show('btnsave', true); HRM.show('btnUpdate', false); HRM.show('btnDelete', false);
        HRM.enable('CmbStockPartyHeader', true); HRM.setCombo('CmbStockPartyHeader', 0);
        HRM.fill('CmbJobOrder', [], 'Id', 'PlanCode', { zero: '' });
        HRM.setVal('txtProductionNo', ''); HRM.setVal('txtRemarks', ''); HRM.setVal('txtQty', ''); HRM.setVal('txtUnitWeight', '');
        grdInput.set([]); grdByProduct.set([]); grdPM.set([]); pmRow();
        summary();
        HRM.focus('txtDocdate');
        return HRM.get(API + 'code').then(function (c) { HRM.setVal('txtdocnumber', c.docNo); }).catch(HRM.fail);
    }
    P.btnnew = function () { resetDetail(); refreshForm(); };
    P.btnRefresh = function (b) { HRM.busy(b, function () { return HRM.get(API + 'refresh').then(fillLookups).catch(HRM.fail); }); };

    function insert(btn) {                                                  // Insert()
        var dn = HRM.val('txtdocnumber').trim(), pn = HRM.val('txtProductionNo').trim();
        if (dn === '' || dn === '0') { HRM.box('document Number Field Required'); return; }
        if (pn === '' || pn === '0') { HRM.box('Production NO. Field Required'); HRM.focus('txtProductionNo'); return; }
        if (!HRM.comboVal('CmbStockPartyHeader')) { HRM.box('Stock Party Field Required'); HRM.focus('CmbStockPartyHeader'); return; }
        if (!grdInput.rows().some(function (r) { return r.EntryType === 'Issue'; })) { HRM.box('Input is Required in Detail Grid'); return; }
        if (!grdByProduct.rows().length) { HRM.box('OutPut Grid Not Found'); return; }
        if (!grdByProduct.rows().some(function (r) { return r.EntryType === 'Recovery Head Rice'; })) { HRM.box('Recovery Head Rice is Required in Detail Grid'); return; }
        if (!HRM.ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { id: S.recId, docNo: dn, docDate: HRM.val('txtDocdate'), productionNo: HRM.val('txtProductionNo'), stockPartyId: HRM.comboVal('CmbStockPartyHeader'),
            remarks: HRM.val('txtRemarks'), inputs: grdInput.rows(), outputs: grdByProduct.rows(), removed: S.removed, pm: grdPM.rows() };
        HRM.busy(btn, function () {
            return HRM.post(API + 'save', body).then(function (r) {
                HRM.box(r.message);
                var id = HRM.int(r.id);
                if (r.openWages) HRM.open('/party-processing/wages-bill?refDocTypeId=176&refDocId=' + id);
                return refreshForm().then(function () { if (HRM.checked('chkPrint')) report(id, null, false); });
            }).catch(HRM.fail);
        });
    }
    P.btnsave = function (b) { S.recId = 0; insert(b); };
    P.btnUpdate = function (b) { if (!S.recId) { HRM.box('RecId not found'); return; } insert(b); };
    P.btnDelete = function (b) {                                            // btnDelete_Click
        if (!S.recId) { HRM.box('Record Id Not Found'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        HRM.busy(b, function () { return HRM.post(API + 'delete?id=' + S.recId, {}).then(function (r) { HRM.box(r.message); return refreshForm(); }).catch(HRM.fail); });
    };
    function readById(id) {                                                 // ReadById
        return HRM.loading(HRM.get(API + 'read', { id: id }).then(function (h) {
            S.recId = HRM.int(h.Id); S.removed = [];
            tabs.show('form');
            HRM.show('btnUpdate', true); HRM.show('btnDelete', true); HRM.show('btnsave', false);
            HRM.setVal('txtdocnumber', h.DocNo); HRM.setVal('txtDocdate', HRM.day(h.DocDate)); HRM.setVal('txtProductionNo', h.ProductionNo);
            HRM.enable('CmbStockPartyHeader', false);
            HRM.setCombo('CmbStockPartyHeader', h.StockPartyId);
            HRM.setVal('txtRemarks', h.Remarks);
            grdInput.set((h.details || []).filter(function (d) { return d.EntryType === 'Issue'; }));
            grdByProduct.set((h.details || []).filter(function (d) { return d.EntryType !== 'Issue'; }));
            grdPM.set(h.pm && h.pm.length ? h.pm : []); if (!grdPM.rows().length) pmRow();
            summary();
            HRM.focus('txtDocdate');
            return partyChanged();
        }).catch(function (e) { hDetail.clear(); HRM.fail(e); }));
    }
    function report(id, btn, fromHistory) { return PPC.print(API + 'print', 'ppc-600', { id: id, history: !!fromHistory }, btn); }
    P.print = function (b) { report(S.recId, b, false); };

    // ------------------------------------------------------------------ history
    P.btnNewHistory = function () {                                        // btnNewHistory_Click
        HRM.setVal('txtFromDocNoHistory', ''); HRM.setVal('txtToDocNoHistory', ''); histDates(3);
        var r = document.querySelector('input[name="histDateMode"][value="doc"]'); if (r) r.checked = true;
        hMain.clear(); hPm.clear(); hDetail.clear();
    };
    P.btnshowHistory = function (b) {
        HRM.busy(b, function () { return HRM.post(API + 'history', PPC.hist()).then(function (rows) { hMain.set(rows); hDetail.clear(); hPm.clear(); }).catch(HRM.fail); });
    };
    P.shortcuts = function () {
        PPC.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+L', 'For Load Input'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form']]);
    };

    // ------------------------------------------------------------------ events / keys
    PPC.$('CmbStockPartyHeader').addEventListener('change', partyChanged);
    PPC.$('cmbItem').addEventListener('change', function () { uoms(HRM.comboText('cmbUOM')); if (HRM.comboText('cmbEntryType') === 'Issue') balance(); });
    PPC.$('cmbGodown').addEventListener('change', function () { if (HRM.comboText('cmbEntryType') === 'Issue') balance(); });
    PPC.$('cmbEntryType').addEventListener('change', function () {           // cmbEntryType_Leave
        if (!HRM.comboText('cmbEntryType')) return;
        if (!HRM.comboVal('CmbStockPartyHeader')) { HRM.box('Please Select Stock Party First!'); HRM.focus('CmbStockPartyHeader'); return; }
        if (HRM.comboText('cmbEntryType') === 'Issue') gtabs.show('input');
        else { gtabs.show('output'); HRM.setVal('txtStock', '0'); HRM.setVal('txtBalanceWeight', '0'); }
        HRM.focus('cmbGodown');
    });
    PPC.$('cmbUOM').addEventListener('change', netWeight);
    PPC.$('txtQty').addEventListener('input', netWeight);
    PPC.guard('txtQty', 'dec'); PPC.guard('txtUnitWeight', 'dec'); PPC.guard('txtFromDocNoHistory', 'int'); PPC.guard('txtToDocNoHistory', 'int');
    function onForm() { return tabs.current() === 'form'; }
    function can(id) { return HRM.visible(id) && !PPC.$(id).disabled; }
    HRM.keys({
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+t': function () { tabs.show(onForm() ? 'history' : 'form'); },
        'ctrl+s': function () { if (onForm()) { if (can('btnsave')) P.btnsave(PPC.$('btnsave')); } else P.btnshowHistory(PPC.$('btnshowHistory')); },
        'ctrl+u': function () { if (onForm() && can('btnUpdate')) P.btnUpdate(PPC.$('btnUpdate')); },
        'ctrl+n': function () { if (onForm()) P.btnnew(); else P.btnNewHistory(); },
        'ctrl+r': function () { if (onForm()) P.btnRefresh(PPC.$('btnRefresh')); },
        'ctrl+p': function () { if (onForm() && can('Print')) P.print(PPC.$('Print')); },
        'ctrl+l': function () { if (onForm()) P.btnLoadInput(); }
    });
    HRM.footer(function () { tabs.show('history'); });
    load();
})();
