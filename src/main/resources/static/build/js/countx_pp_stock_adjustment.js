/* ============================================================================================
 * countx_pp_stock_adjustment.js - 604 Stock Adjustment (Party Processing)
 * Desktop: Architecture.WinApp.PartyProcessing.StockAdjustment.cs. Exports window.PpSa.
 * Load, the detail entry (btnplus / grd_DoubleClick / btnUpdateDetail / DeleteDetailRecord, CalculateWeight,
 * AvailableStock, bindRateUomAndItemPackUom), grd_CellUpdated for loader rows, the issuance loader (Load Data), Insert
 * (server), ReadById, Delete, history (main + detail grids), keys.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/stock-adjustment/';
    var P = {}; window.PpSa = P;
    var S = { rights: {}, recId: 0, removed: [], updIndex: -1, items: [], defaultDays: 3 };
    var tabs = PPC.tabs(function (t) { HRM.focus(t === 'history' ? 'FromDateHistory' : 'DocDate'); });

    function loaderRows() {                                                  // first row carries a reference (loader rows)
        var r = grd.rows()[0];
        return !!r && (HRM.int(r.RefDocumentTypeId) > 0 || HRM.int(r.RefDocIdNo) > 0 || HRM.int(r.RefDocSubIdNo) > 0);
    }
    function refEditable() { var r = grd.rows()[0]; return !!r && HRM.int(r.RefDocumentTypeId) > 0; }
    var ro = function () { return !refEditable(); };
    var grd = new HRM.Grid('grd', {
        emptyText: '',
        columns: [
            PPC.btnCol('Delete', 'X', 30), PPC.btnCol('Edit', 'Edit'),
            { key: 'Id', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocIdNo', hidden: true }, { key: 'RefDocSubIdNo', hidden: true },
            { key: 'WareHouseId', hidden: true }, { key: 'WareHouse', caption: 'WareHouse' }, { key: 'ItemId', hidden: true },
            { key: 'ItemCode', caption: 'ItemCode' }, { key: 'Item', caption: 'Item' }, { key: 'CropYearId', hidden: true }, { key: 'CropYear', caption: 'CropYear' },
            { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot' }, { key: 'PackingTypeId', hidden: true }, { key: 'PackingType', caption: 'PackingType' },
            { key: 'ItemUOMId', hidden: true }, { key: 'ItemUOM', caption: 'ItemUOM' }, { key: 'Equivalent', hidden: true },
            { key: 'BalQty', hidden: true }, { key: 'BalWeight', hidden: true },
            { key: 'ItemQty', caption: 'ItemQty', type: 'edit-num', readOnly: ro, sum: true, decimals: 2 },
            { key: 'Weight', caption: 'Weight', type: 'edit-num', readOnly: ro, sum: true, decimals: 3 },
            { key: 'Comments', caption: 'Comments', type: 'edit', readOnly: ro }
        ],
        totals: true,
        onDouble: function (r, i) { editRow(i); },
        onChange: cellUpdated
    });
    PPC.onButton(grd, 'grd', function (r, act, i) {                          // grd_ColumnButtonClick
        if (act === 'Delete') deleteDetail(i); else if (act === 'Edit') editRow(i);
        HRM.enable('CmbEntryType', !(grd.rows().length && loaderRowsOf(r)));
    });
    function loaderRowsOf(r) { return !!r && (HRM.int(r.RefDocumentTypeId) > 0 || HRM.int(r.RefDocIdNo) > 0 || HRM.int(r.RefDocSubIdNo) > 0); }

    var grdhistory = new HRM.Grid('grdhistory', {
        filterRow: true, emptyText: 'No record found.',
        columns: [PPC.btnCol('Print', 'Print'), PPC.btnCol('Edit', 'Edit'),
            { key: 'Id', hidden: true }, { key: 'DocDate', caption: 'DocDate', type: 'date' }, { key: 'DocNo', caption: 'DocNo' },
            { key: 'EntryType', caption: 'EntryType' }, { key: 'StockParty', caption: 'StockParty' }, { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
            { key: 'EntryUser', caption: 'EntryUser' }, { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' }, { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'NoOfAttachments', caption: 'Attached', type: 'int' }, { key: 'RemarksHeader', caption: 'RemarksHeader' }],
        onDouble: function (r) { readById(HRM.int(r.Id)); },
        onSelect: function (r) { detailBind(HRM.int(r.Id)); }
    });
    PPC.onButton(grdhistory, 'grdhistory', function (r, act, i, b) {
        if (act === 'Edit') readById(HRM.int(r.Id)); else if (act === 'Print') report(HRM.int(r.Id), b, true);
    });
    var grdHistoryDetail = new HRM.Grid('grdHistoryDetail', {
        emptyText: '',
        columns: [{ key: 'Id', hidden: true }, { key: 'WareHouse', caption: 'WareHouse' }, { key: 'ItemCode', caption: 'ItemCode' },
            { key: 'ItemName', caption: 'ItemName' }, { key: 'CropYear', caption: 'CropYear' }, { key: 'JobLot', caption: 'JobLot' },
            { key: 'PackingType', caption: 'PackingType' }, { key: 'PackUom', caption: 'PackUom' },
            PPC.numCol('ItemQty', 'ItemQty', '#,#.##'), PPC.numCol('Weight', 'Weight', '#,#.##'), { key: 'Comments', caption: 'Comments' }]
    });

    // ------------------------------------------------------------------ load / combos
    function fillLookups(o) {
        HRM.fill('CmbStockParty', o.stockParties, 'Id', 'CompanyName', { zero: '', keep: true });
        HRM.fill('CmbWareHouse', o.warehouses, 'Id', 'WareHouseName', { zero: '', keep: true });
        S.items = o.items || [];
        HRM.fill('CmbItemName', S.items, 'Id', 'ItemName', { zero: '', keep: true, attrs: ['ItemCode'] });
        HRM.fill('CmbCropYear', o.cropYears, 'Id', 'CropYear', { zero: '', keep: true });
        HRM.fill('CmbJobLot', o.jobLots, 'Id', 'JobLotDescription', { zero: '', keep: true });
        HRM.fill('CmbPackingType', o.packingTypes, 'Id', 'PackTypeDesc', { zero: '', keep: true });
    }
    function fillHistoryCombos(o) {
        HRM.fill('CmbAdjustmentTypeHistory', o.historyEntryTypes, 'Id', 'Name', { zero: '' });
        HRM.fill('CmbStockPartyHistory', o.historyParties, 'Id', 'Name', { zero: '' });
    }
    function load() {
        HRM.setVal('DocDate', HRM.today());
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -3)); HRM.setVal('ToDateHistory', HRM.today());
        grd.set([]);
        return HRM.loading(HRM.get(API + 'setup').then(function (o) {
            S.rights = o.rights || {};
            HRM.applyRights(S.rights, { save: 'btnsave', update: 'btnupdate', print: 'btnprint', delete: 'btnDelete' });
            HRM.check('ChkPrint', S.rights.print !== false);
            HRM.setVal('txtdocno', o.docNo);
            HRM.fill('CmbEntryType', o.entryTypes, 'Id', 'Type', { zero: '' });
            fillLookups(o);
            fillHistoryCombos(o);
            HRM.focus('DocDate');
        }).catch(HRM.fail));
    }

    function uoms(itemId, keepText) {                                       // bindRateUomAndItemPackUom
        return HRM.get(API + 'uoms', { itemId: itemId }).then(function (rows) {
            HRM.fill('CmbPackUom', rows, 'Id', 'UOMCode', { zero: '', attrs: ['Equivalent'] });
            if (keepText) HRM.setComboText('CmbPackUom', keepText);
            calcWeight();
        }).catch(HRM.fail);
    }
    function availableStock() {                                             // AvailableStock
        var item = HRM.comboVal('CmbItemName');
        return HRM.get(API + 'stock', { itemId: item, docDate: HRM.val('DocDate'), jobLotId: HRM.comboVal('CmbJobLot'),
            warehouseId: HRM.comboVal('CmbWareHouse'), cropYear: HRM.comboText('CmbCropYear') }).then(function (r) {
            var bal = HRM.num(r.balance);
            HRM.show('lblStockBalance', bal > 0);
            HRM.text('lblStockBalance', bal > 0 ? PPC.fmt(bal, '#,#.##') : '');
        }).catch(function () { HRM.show('lblStockBalance', false); });
    }
    function calcWeight() {                                                 // CalculateWeight
        var qty = PPC.dbl('txtqty');
        var row = HRM.comboVal('CmbPackUom') > 0 ? HRM.comboRow('CmbPackUom', 'Id') : null;
        var uom = row ? HRM.num(HRM.col(row, 'Equivalent')) : 0;
        HRM.setVal('txtweight', qty > 0 && uom > 0 ? PPC.fmt(qty * uom, '#,##0.####') : '0');
    }

    // ------------------------------------------------------------------ detail entry
    function detailCheck() {                                                // FormValidationDetail
        var c = [['CmbWareHouse', 'WareHouseName field is required'], ['CmbItemName', 'ItemName field is required'],
            ['CmbCropYear', 'CropYear field is required'], ['CmbJobLot', 'JobLot field is required'],
            ['CmbPackingType', 'Packingtype field is required'], ['CmbPackUom', 'PackUOM field is required']];
        for (var i = 0; i < c.length; i++) if (!HRM.comboVal(c[i][0])) { HRM.box(c[i][1]); HRM.focus(c[i][0]); return false; }
        if (PPC.dbl('txtqty') === 0) { HRM.box('ItemQty field is required'); HRM.focus('txtqty'); return false; }
        if (PPC.dbl('txtweight') === 0) { HRM.box('Weight field is required'); HRM.focus('txtweight'); return false; }
        return true;
    }
    function itemRow() { return HRM.comboRow('CmbItemName', 'Id') || {}; }
    function entryRow(base) {
        var it = itemRow(), u = HRM.comboRow('CmbPackUom', 'Id') || {};
        var r = base || { Id: 0, RefDocumentTypeId: 0, RefDocIdNo: 0, RefDocSubIdNo: 0 };
        r.WareHouseId = HRM.comboVal('CmbWareHouse'); r.WareHouse = HRM.comboText('CmbWareHouse');
        r.ItemId = HRM.comboVal('CmbItemName'); r.ItemCode = HRM.str(HRM.col(it, 'ItemCode')); r.Item = HRM.str(HRM.col(it, 'ItemName'));
        r.CropYearId = HRM.comboVal('CmbCropYear'); r.CropYear = HRM.comboText('CmbCropYear').trim();
        r.JobLotId = HRM.comboVal('CmbJobLot'); r.JobLot = HRM.comboText('CmbJobLot');
        r.PackingTypeId = HRM.comboVal('CmbPackingType'); r.PackingType = HRM.comboText('CmbPackingType');
        r.ItemUOMId = HRM.comboVal('CmbPackUom'); r.ItemUOM = HRM.comboText('CmbPackUom').trim(); r.Equivalent = HRM.num(HRM.col(u, 'Equivalent'));
        r.ItemQty = PPC.dbl('txtqty'); r.Weight = PPC.dbl('txtweight'); r.BalQty = r.ItemQty; r.BalWeight = r.Weight;
        r.Comments = HRM.val('txtremarksdetail').trim();
        return r;
    }
    function afterGridChange() {
        HRM.enable('CmbStockParty', grd.rows().length <= 0);
        HRM.enable('CmbEntryType', !refEditable());
        grd.draw();
    }
    function resetDetail() {                                                // ResetDetail
        ['CmbWareHouse', 'CmbItemName', 'CmbJobLot', 'CmbPackingType', 'CmbPackUom'].forEach(function (id) { HRM.setCombo(id, 0); });
        HRM.setVal('txtqty', ''); HRM.setVal('txtweight', ''); HRM.setVal('txtremarksdetail', '');
        HRM.show('btnplus', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        HRM.enable('CmbStockParty', grd.rows().length <= 0);
    }
    P.btnplus = function () {                                               // btnplus_Click
        if (grd.rows().length && loaderRows()) { HRM.box('You can not add manual Record beacause record against Loader exist in Grid'); return; }
        if (!detailCheck()) return;
        grd.add(entryRow());
        resetDetail();
        afterGridChange();
    };
    function editRow(i) {                                                   // grd_DoubleClick
        if (grd.rows().length && loaderRows()) { HRM.box('You can Update like manual Record beacause record against Loader exist in Grid'); return; }
        var r = grd.rows()[i]; if (!r) return;
        S.updIndex = i;
        HRM.setCombo('CmbWareHouse', r.WareHouseId);
        HRM.setCombo('CmbItemName', r.ItemId);
        uoms(r.ItemId, null).then(function () {
            HRM.setCombo('CmbPackUom', r.ItemUOMId);
            HRM.setCombo('CmbPackingType', r.PackingTypeId);
            HRM.setCombo('CmbCropYear', r.CropYearId);
            HRM.setVal('txtqty', PPC.net(r.ItemQty));
            HRM.setVal('txtweight', PPC.net(r.Weight));
            HRM.setCombo('CmbJobLot', r.JobLotId);
            HRM.setVal('txtremarksdetail', r.Comments);
            HRM.show('btnplus', false); HRM.show('btnUpdateDetail', true); HRM.show('btnCancelUpdateDetial', true);
            HRM.focus('CmbWareHouse');
        });
    }
    P.btnUpdateDetail = function () {                                       // btnUpdateDetail_Click
        if (!detailCheck()) return;
        var r = grd.rows()[S.updIndex]; if (!r) return;
        grd.update(S.updIndex, entryRow(r));
        S.updIndex = -1;
        resetDetail();
        afterGridChange();
    };
    P.btnCancelUpdateDetial = function () { S.updIndex = -1; resetDetail(); };
    function deleteDetail(i) {                                              // DeleteDetailRecord
        var r = grd.rows()[i]; if (!r) return;
        if (i === S.updIndex) { HRM.box("You can't delete this record. Because this record is in update Mode"); return; }
        if (HRM.int(r.Id) > 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            S.removed.push(Object.assign({}, r));
        }
        grd.remove(i);
        if (S.updIndex > i) S.updIndex--;
        afterGridChange();
    }
    function cellUpdated(r, key) {                                          // grd_CellUpdated
        var saveMode = HRM.visible('btnsave') && !PPC.$('btnsave').disabled;
        if (saveMode) {
            if (key === 'ItemQty') {
                var qty = HRM.num(r.ItemQty), bq = HRM.num(r.BalQty), bw = HRM.num(r.BalWeight);
                r.Weight = bw / bq * qty;
                if (bq < qty) { r.ItemQty = bq; r.Weight = bw; HRM.box('Qty cannot greater than Balance Qty ' + PPC.net(bq) + ' Please check!'); }
            }
            if (key === 'Weight' && HRM.num(r.BalWeight) < HRM.num(r.Weight)) {
                r.Weight = HRM.num(r.BalWeight);
                HRM.box('Weight cannot greater than Balance Weight ' + PPC.net(r.BalWeight) + ' Please check!');
            }
        } else if (key === 'ItemQty') r.Weight = HRM.num(r.Equivalent) * HRM.num(r.ItemQty);
        grd.draw();
    }

    // ------------------------------------------------------------------ loader
    P.btnLoadAvailableData = function () {                                  // btnLoadAvailableData_Click
        if (grd.rows().length) {
            var f = grd.rows()[0];
            if (HRM.int(f.RefDocumentTypeId) === 0 && HRM.int(f.RefDocIdNo) === 0 && HRM.int(f.RefDocSubIdNo) === 0) { HRM.box('You can not add Record From Loader beacause manual record exist in Grid'); return; }
        }
        var et = HRM.comboVal('CmbEntryType');
        if (et === 0 || et === 1) { HRM.focus('CmbEntryType'); HRM.box("EntryType Should Be Selected to 'Loss'"); return; }
        if (!HRM.comboVal('CmbStockParty')) { HRM.focus('CmbStockParty'); HRM.box('Stock Party Account select First'); return; }
        PPC.loader({ page: 'stock-adjustment', stockPartyId: HRM.comboVal('CmbStockParty'), onLoad: function (rows) {
            if (!rows.length) return;                                       // LoadDataDetailfromPurchaseInvoivce
            if (grd.rows().length && HRM.comboVal('CmbStockParty') !== HRM.int(HRM.col(rows[0], 'StockPartyId'))) { HRM.box('Data against another Stock Party Already Exist'); return; }
            rows.forEach(function (d) {
                var c = function (k) { return HRM.col(d, k); };
                var dup = grd.rows().some(function (g) {
                    return HRM.int(c('RefDocumentTypeId')) === HRM.int(g.RefDocumentTypeId) && HRM.int(c('RefDocIdNo')) === HRM.int(g.RefDocIdNo) && HRM.int(c('RefDocSubIdNo')) === HRM.int(g.RefDocSubIdNo);
                });
                if (dup) return;
                grd.data.push({ Id: 0, RefDocumentTypeId: c('RefDocumentTypeId'), RefDocIdNo: c('RefDocIdNo'), RefDocSubIdNo: c('RefDocSubIdNo'),
                    WareHouseId: c('WareHouseId'), WareHouse: c('WareHouseCode'), ItemId: c('ItemId'), ItemCode: c('ItemCode'), Item: c('ItemName'),
                    CropYearId: c('CropYearId'), CropYear: c('CropYear'), JobLotId: c('JobLotId'), JobLot: c('JobLotCode'), PackingTypeId: c('InvPackingTypeId'),
                    PackingType: c('PackingType'), ItemUOMId: c('ItemUomId'), ItemUOM: c('PackUom'), Equivalent: c('Equivalent'),
                    BalQty: c('QtyBalance'), BalWeight: c('WeightBalance'), ItemQty: c('QtyBalance'), Weight: c('WeightBalance'), Comments: '' });
            });
            afterGridChange();
        } });
    };

    // ------------------------------------------------------------------ save / read / delete
    function reset() {                                                      // Reset()
        S.updIndex = -1; S.removed = []; S.recId = 0;
        HRM.enable('CmbEntryType', true);
        ['CmbEntryType', 'CmbStockParty', 'CmbItemName', 'CmbPackingType', 'CmbPackUom'].forEach(function (id) { HRM.setCombo(id, 0); });
        HRM.setVal('txtqty', ''); HRM.setVal('txtweight', ''); HRM.setVal('txtremarks', '');
        HRM.show('btnsave', true); HRM.show('btnupdate', false); HRM.show('btnplus', true); HRM.show('btnUpdateDetail', false);
        HRM.show('btnCancelUpdateDetial', false); HRM.show('btnDelete', false);
        grd.set([]);
        HRM.enable('CmbStockParty', true);
        HRM.focus('DocDate');
        return HRM.get(API + 'code').then(function (c) { HRM.setVal('txtdocno', c.docNo); }).catch(HRM.fail);
    }
    P.btnnew = function () { reset(); };
    P.btnRefresh = function (b) { HRM.busy(b, function () { return HRM.get(API + 'refresh').then(fillLookups).catch(HRM.fail); }); };

    function insert(btn) {                                                  // Insert()
        if (!grd.rows().length) { HRM.box('Grid Record Not Found'); return; }
        var docNo = HRM.val('txtdocno').trim();
        if (docNo === '' || docNo === '0') { HRM.box('DocNo Field is Required'); return; }
        if (!HRM.comboVal('CmbEntryType')) { HRM.box('EntryType Field is Required'); HRM.focus('CmbEntryType'); return; }
        if (!HRM.comboVal('CmbStockParty')) { HRM.box('Stock Party Field is Required'); HRM.focus('CmbStockParty'); return; }
        if (!HRM.ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { id: S.recId, docNo: docNo, docDate: HRM.val('DocDate'), entryTypeId: HRM.comboVal('CmbEntryType'),
            stockPartyId: HRM.comboVal('CmbStockParty'), remarks: HRM.val('txtremarks'), rows: grd.rows(), removed: S.removed };
        HRM.busy(btn, function () {
            return HRM.post(API + 'save', body).then(function (r) {
                HRM.box(r.message);
                var id = HRM.int(r.id);
                return reset().then(function () { if (HRM.checked('ChkPrint')) report(id, null, false); });
            }).catch(HRM.fail);
        });
    }
    P.btnsave = function (b) { S.recId = 0; insert(b); };
    P.btnupdate = function (b) { if (!S.recId) { HRM.box('Record Not Update because RecId Not Found'); return; } insert(b); };
    P.btnDelete = function (b) {                                            // btnDelete_Click
        if (!(S.recId > 0)) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        HRM.busy(b, function () { return HRM.post(API + 'delete?id=' + S.recId, {}).then(function (r) { HRM.box(r.message); return reset(); }).catch(HRM.fail); });
    };
    function readById(id) {                                                 // ReadById
        return HRM.loading(HRM.get(API + 'read', { id: id }).then(function (h) {
            S.recId = HRM.int(h.Id); S.removed = []; S.updIndex = -1;
            tabs.show('form');
            HRM.setVal('DocDate', HRM.day(h.DocDate));
            HRM.setVal('txtdocno', h.DocNo);
            HRM.setCombo('CmbEntryType', h.AdjustmentTypeId);
            HRM.setCombo('CmbStockParty', h.StockPartyId);
            HRM.setVal('txtremarks', h.RemarksHeader);
            grd.set(h.rows || []);
            afterGridChange();
            HRM.show('btnsave', false); HRM.show('btnupdate', true); HRM.show('btnDelete', true);
        }).catch(HRM.fail));
    }
    function report(id, btn, fromHistory) { return PPC.print(API + 'print', 'ppc-158', { id: id, history: !!fromHistory }, btn); }
    P.btnprint = function (b) { report(S.recId, b, false); };

    // ------------------------------------------------------------------ history
    function detailBind(id) {                                               // GridDetailBind
        HRM.get(API + 'read', { id: id }).then(function (h) {
            grdHistoryDetail.set((h.rows || []).map(function (d) {
                return { Id: d.Id, WareHouse: d.WareHouse, ItemCode: d.ItemCode, ItemName: d.Item, CropYear: d.CropYear, JobLot: d.JobLot,
                    PackingType: d.PackingType, PackUom: d.ItemUOM, ItemQty: d.ItemQty, Weight: d.Weight, Comments: d.Comments };
            }));
        }).catch(function () { grdHistoryDetail.clear(); });
    }
    P.btnNewHistory = function () {                                        // ResetHistory
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -3)); HRM.setVal('ToDateHistory', HRM.today());
        HRM.setVal('txtFromDocNoHistory', ''); HRM.setVal('txtToDocNoHistory', '');
        HRM.setCombo('CmbAdjustmentTypeHistory', 0);
        grdhistory.clear(); grdHistoryDetail.clear();
        HRM.focus('FromDateHistory');
    };
    P.btnRefreshHistory = function (b) { HRM.busy(b, function () { return HRM.get(API + 'history-combos').then(fillHistoryCombos).catch(HRM.fail); }); };
    P.btnShowHistory = function (b) {                                       // gridhistoryfill
        HRM.busy(b, function () {
            return HRM.post(API + 'history', PPC.hist(null, { entryTypeId: HRM.comboVal('CmbAdjustmentTypeHistory'), stockPartyId: HRM.comboVal('CmbStockPartyHistory') }))
                .then(function (rows) { grdhistory.set(rows); grdHistoryDetail.clear(); }).catch(HRM.fail);
        });
    };

    P.shortcuts = function () {
        PPC.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+L', 'For Load Data'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+T', 'For Tab Transfer'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form']]);
    };

    // ------------------------------------------------------------------ events / keys
    PPC.$('CmbItemName').addEventListener('change', function () { uoms(HRM.comboVal('CmbItemName'), HRM.comboText('CmbPackUom').trim()); availableStock(); });
    ['CmbWareHouse', 'CmbCropYear', 'CmbJobLot'].forEach(function (id) { PPC.$(id).addEventListener('change', availableStock); });
    PPC.$('CmbPackUom').addEventListener('change', calcWeight);
    PPC.$('txtqty').addEventListener('input', calcWeight);
    PPC.guard('txtqty', 'dec'); PPC.guard('txtFromDocNoHistory', 'int'); PPC.guard('txtToDocNoHistory', 'int');
    function onForm() { return tabs.current() === 'form'; }
    function can(id) { return HRM.visible(id) && !PPC.$(id).disabled; }
    HRM.keys({
        'ctrl+t': function () { tabs.show(onForm() ? 'history' : 'form'); },
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+l': function () { if (onForm()) P.btnLoadAvailableData(); },
        'ctrl+s': function () { if (onForm()) { if (can('btnsave')) P.btnsave(PPC.$('btnsave')); } else P.btnShowHistory(PPC.$('btnShowHistory')); },
        'ctrl+u': function () { if (onForm() && can('btnupdate')) P.btnupdate(PPC.$('btnupdate')); },
        'ctrl+p': function () { if (onForm() && can('btnprint')) P.btnprint(PPC.$('btnprint')); },
        'ctrl+n': function () { if (onForm()) P.btnnew(); },
        'ctrl+r': function () { if (onForm()) P.btnRefresh(PPC.$('btnRefresh')); },
        'ctrl+f5': function () { if (onForm()) HRM.focus('DocDate'); },
        'ctrl+arrowup': function () { if (onForm()) HRM.focus('CmbWareHouse'); }
    });
    HRM.footer(function () { tabs.show('history'); });
    load();
})();
