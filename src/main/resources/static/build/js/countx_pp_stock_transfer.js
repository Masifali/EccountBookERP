/* ============================================================================================
 * countx_pp_stock_transfer.js - 603 Stock Transfer (Party Processing)
 * Desktop: Architecture.WinApp.PartyProcessing.StockTransfer.cs. Exports window.PpSt.
 * Load, ticket -> factory weight, detail entry (btnplus / grd_DoubleClick / btnUpdateDetail / Delete), GrossWeightCalculation,
 * WeightCalculation, AvailableStockGetByItem, loader, "Update Same In All Rows", Insert (server; opens the wages bill after
 * the save as the desktop does when wages are compulsory and active), ReadById, history, keys.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/stock-transfer/';
    var P = {}; window.PpSt = P;
    var S = { rights: {}, recId: 0, updIndex: -1, approved: false, defaultDays: 0 };
    var tabs = PPC.tabs(function (t) { HRM.focus(t === 'history' ? 'FromDateHistory' : 'DocDate'); });
    var n3 = function (k, c) { return PPC.numCol(k, c, '#,##0.###', { sum: true }); };

    function saveMode() { return HRM.visible('btnsave') && !PPC.$('btnsave').disabled; }
    var grd = new HRM.Grid('grd', {
        emptyText: '', totals: true,
        columns: [
            { key: 'Delete', caption: 'X', width: 30, render: function () { return saveMode() ? PPC.btn('Delete', 'X') : ''; } },
            { key: 'Id', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocNoId', hidden: true }, { key: 'RefDocSubIdNo', hidden: true },
            { key: 'StockPartyFromId', hidden: true }, { key: 'StockPartyFrom', caption: 'StockPartyFrom' },
            { key: 'StockPartyToId', hidden: true }, { key: 'StockPartyTo', caption: 'StockPartyTo' },
            { key: 'WareHouseFromId', hidden: true }, { key: 'WareHouseFrom', caption: 'WareHouseFrom' },
            { key: 'WareHouseToId', hidden: true }, { key: 'WareHouseTo', caption: 'WareHouseTo' },
            { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot' }, { key: 'JobLotIdTo', hidden: true }, { key: 'JobLotTo', caption: 'JobLotTo' },
            { key: 'CropYearId', hidden: true }, { key: 'CropYear', caption: 'CropYear' }, { key: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName' },
            { key: 'PackTypeId', hidden: true }, { key: 'PackType', caption: 'PackType' }, { key: 'PackUomId', hidden: true }, { key: 'PackUom', caption: 'PackUom' },
            n3('QTY', 'QTY'), n3('GrossWeight', 'GrossWeight'), n3('EbUnit', 'EbUnit'), n3('EbTotal', 'EbTotal'), n3('Ad/LsWeight', 'Ad/LsWeight'),
            n3('NetWeight', 'NetWeight'), n3('BalQty', 'BalQty'), n3('BalWeight', 'BalWeight'), { key: 'Remarks', caption: 'Remarks' }
        ],
        onDouble: function (r, i) { editRow(i); }
    });
    function balCols() {                                                   // grdSettings: BalQty / BalWeight hidden for manual rows
        var cur = grd.rows()[0];
        var hide = !cur || HRM.int(cur.RefDocumentTypeId) === 0;
        PPC.showCols(grd, ['BalQty', 'BalWeight'], !hide);
    }
    PPC.onButton(grd, 'grd', function (r, act, i) {                          // grd_ColumnButtonClick
        if (act !== 'Delete' || !saveMode()) return;
        if (S.updIndex > -1) { HRM.box('Reset Detail First!'); return; }
        grd.remove(i);
    });

    var grdhistory = new HRM.Grid('grdhistory', {
        filterRow: true, emptyText: 'No record found.',
        columns: [PPC.btnCol('Edit', 'Edit'), PPC.btnCol('Print', 'Print'), { key: 'Id', hidden: true }, { key: 'DocDate', caption: 'DocDate', type: 'date' },
            { key: 'DocNo', caption: 'DocNo' }, { key: 'TicketId', hidden: true }, { key: 'TicketNo', caption: 'TicketNo' },
            PPC.numCol('FactoryWeight', 'FactoryWeight', '#,##0.###'), { key: 'EntryDate', caption: 'EntryDate', type: 'datetime' },
            { key: 'EntryUser', caption: 'EntryUser' }, { key: 'ModifyDate', caption: 'ModifyDate', type: 'datetime' }, { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' }, { key: 'Remarks', caption: 'Remarks' }],
        onDouble: function (r) { readById(HRM.int(r.Id)); },
        onSelect: function (r) {
            HRM.get(API + 'history-detail', { id: HRM.int(r.Id) }).then(function (rows) { GridDetailHistory.set(rows); }).catch(HRM.fail);
        }
    });
    PPC.onButton(grdhistory, 'grdhistory', function (r, act, i, b) {
        if (act === 'Edit') { reset(); readById(HRM.int(r.Id)); } else if (act === 'Print') report(HRM.int(r.Id), b, true);
    });
    var GridDetailHistory = new HRM.Grid('GridDetailHistory', {
        emptyText: '',
        columns: [{ key: 'FromStockParty', caption: 'FromStockParty' }, { key: 'ToStockParty', caption: 'ToStockParty' }, { key: 'FromWarehouse', caption: 'FromWarehouse' },
            { key: 'ToWarehouse', caption: 'ToWarehouse' }, { key: 'FromJobLot', caption: 'FromJobLot' }, { key: 'ToJobLot', caption: 'ToJobLot' },
            { key: 'CropYear', caption: 'CropYear' }, { key: 'ItemName', caption: 'ItemName' }, { key: 'PackingType', caption: 'PackingType' },
            PPC.numCol('Qty', 'Qty', '#,##0.##'), { key: 'PackUom', caption: 'PackUom' }, PPC.numCol('GrossWeight', 'GrossWeight', '#,##0.###'),
            PPC.numCol('EbUnit', 'EbUnit', '#,##0.###'), PPC.numCol('EbTotal', 'EbTotal', '#,##0.###'), PPC.numCol('AddLessWt', 'AddLessWt', '#,##0.###'),
            PPC.numCol('StockWeight', 'StockWeight', '#,##0.###'), { key: 'RemarksDetail', caption: 'RemarksDetail' }]
    });

    // ------------------------------------------------------------------ load / combos
    function fillLookups(o) {
        ['CmbStockPartyFrom', 'CmbStockPartyTo'].forEach(function (id) { HRM.fill(id, o.stockParties, 'Id', 'CompanyName', { zero: '', keep: true }); });
        ['CmbWareHouseFrom', 'cmbWareHouseTo'].forEach(function (id) { HRM.fill(id, o.warehouses, 'Id', 'WareHouseName', { zero: '', keep: true }); });
        ['cmbJobLot', 'CmbJobLotTo'].forEach(function (id) { HRM.fill(id, o.jobLots, 'Id', 'JobLotDescription', { zero: '', keep: true }); });
        HRM.fill('cmbCropYear', o.cropYears, 'Id', 'CropYear', { zero: '', keep: true });
        HRM.fill('cmbItemName', o.items, 'Id', 'ItemName', { zero: '', keep: true });
        HRM.fill('cmbPackingType', o.packingTypes, 'Id', 'PackTypeDesc', { zero: '', keep: true });
    }
    function fillTickets(rows, keepId) {                                    // TicketNofill(220)
        HRM.fill('cmbTicketNo', rows, 'Id', 'TicketNo', { zero: '', attrs: ['NetWbWeight'] });
        if (keepId) HRM.setCombo('cmbTicketNo', keepId);
    }
    function histDates() {
        HRM.setVal('FromDateHistory', HRM.addDays(HRM.today(), -(S.defaultDays > 0 ? S.defaultDays : 3)));
        HRM.setVal('ToDateHistory', HRM.today());
    }
    function load() {
        HRM.setVal('DocDate', HRM.today());
        grd.set([]); balCols();
        return HRM.loading(HRM.get(API + 'setup').then(function (o) {
            S.rights = o.rights || {}; S.wages = !!o.wagesStatus && !!o.wagesActive; S.defaultDays = HRM.int(o.defaultDays);
            HRM.applyRights(S.rights, { save: 'btnsave', update: 'btnupdate' });
            fillLookups(o);
            HRM.setVal('txtdocno', o.docNo);
            fillTickets(o.tickets);
            histDates();
            HRM.focus('DocDate');
        }).catch(HRM.fail));
    }

    function ticketChanged() {                                              // cmbTicketNo_TextChanged
        var r = HRM.comboVal('cmbTicketNo') > 0 ? HRM.comboRow('cmbTicketNo', 'Id') : null;
        if (r) { HRM.enable('txtHeadNetWeight', false); HRM.setVal('txtHeadNetWeight', PPC.fmt(HRM.num(HRM.col(r, 'NetWbWeight')), '#,##0.###')); }
        else { HRM.enable('txtHeadNetWeight', true); HRM.setVal('txtHeadNetWeight', ''); }
    }
    function uoms(itemId) {                                                 // bindRateUomAndItemPackUom
        return HRM.get(API + 'uoms', { itemId: itemId }).then(function (rows) {
            HRM.fill('CmbPackUom', rows, 'Id', 'UOMCode', { zero: '', attrs: ['Equivalent'] });
        }).catch(HRM.fail);
    }
    function availableStock() {                                             // AvailableStockGetByItem
        return HRM.get(API + 'stock', { warehouseId: HRM.comboVal('CmbWareHouseFrom'), itemId: HRM.comboVal('cmbItemName'),
            jobLotId: HRM.comboVal('cmbJobLot'), cropYear: HRM.comboText('cmbCropYear').trim(), docDate: HRM.val('DocDate') })
            .then(function (r) { HRM.setVal('txtAvailableStock', r.available); }).catch(function () { HRM.setVal('txtAvailableStock', '0'); });
    }
    function grossCalc() {                                                  // GrossWeightCalculation
        var row = HRM.comboVal('CmbPackUom') ? HRM.comboRow('CmbPackUom', 'Id') : null;
        if (HRM.val('txtQty').trim() !== '' && row) HRM.setVal('txtGrossWeight', PPC.net(PPC.dbl('txtQty') * HRM.num(HRM.col(row, 'Equivalent'))));
        else HRM.setVal('txtGrossWeight', '0');
    }
    function weightCalc(active) {                                           // WeightCalculation
        var pos = function (id) { var v = PPC.dbl(id); return v > 0 ? v : 0; };
        var gross = pos('txtGrossWeight'), qty = pos('txtQty'), ebU = pos('txtEbUnit'), ebT = pos('txtEbTotal'), al = pos('txtAdLsWeight');
        if (active === 'EbUnit') { ebT = ebU * qty; HRM.setVal('txtEbTotal', PPC.fmt(ebT, '#,##0.###')); }
        else if (active === 'EbTotal') { ebU = ebT / qty; HRM.setVal('txtEbUnit', isFinite(ebU) ? PPC.fmt(ebU, '#,##0.####') : ''); }
        else { ebU = qty > 0 ? ebT / qty : 0; HRM.setVal('txtEbUnit', PPC.fmt(ebU, '#,##0.####')); }
        HRM.setVal('txtDetailNetWeight', PPC.fmt(gross - ebT + al, '#,##0.###'));
    }

    // ------------------------------------------------------------------ detail entry
    function detailCheck() {                                                // FormValidationDetail
        var c = [['CmbStockPartyFrom', 'Stock Party From field is required'], ['CmbStockPartyTo', 'Stock Party To field is required'],
            ['CmbWareHouseFrom', 'WarehouseFrom field is required'], ['cmbWareHouseTo', 'WarehouseTo field is required'],
            ['cmbJobLot', 'JobLot From field is required'], ['CmbJobLotTo', 'JobLotTo field is required']];
        for (var i = 0; i < c.length; i++) if (!HRM.comboVal(c[i][0])) { HRM.box(c[i][1]); HRM.focus(c[i][0]); return false; }
        if (HRM.comboVal('CmbStockPartyFrom') === HRM.comboVal('CmbStockPartyTo') && HRM.comboVal('CmbWareHouseFrom') === HRM.comboVal('cmbWareHouseTo') &&
            HRM.comboVal('cmbJobLot') === HRM.comboVal('CmbJobLotTo')) {
            HRM.box('At least one adjacent pair (Stock Party, Warehouse, Job Lot) should differ to proceed with the entry.'); return false;
        }
        var d = [['cmbCropYear', 'Crop Year field is required'], ['cmbItemName', 'Item Name field is required'], ['cmbPackingType', 'PackingType field is required']];
        for (var j = 0; j < d.length; j++) if (!HRM.comboVal(d[j][0])) { HRM.box(d[j][1]); HRM.focus(d[j][0]); return false; }
        if (PPC.$('CmbPackUom').selectedIndex < 0 || !HRM.comboVal('CmbPackUom')) { HRM.box('Pack Uom field is required'); HRM.focus('CmbPackUom'); return false; }
        if (HRM.val('txtQty').trim() === '' || PPC.dbl('txtQty') === 0) { HRM.box('Qty field is required'); HRM.focus('txtQty'); return false; }
        if (HRM.val('txtGrossWeight').trim() === '' || PPC.dbl('txtGrossWeight') === 0) { HRM.box('GrossWeight field is required'); HRM.focus('txtGrossWeight'); return false; }
        if (HRM.val('txtDetailNetWeight').trim() === '' || PPC.dbl('txtDetailNetWeight') === 0) { HRM.box('NetWeight field is required'); HRM.focus('txtDetailNetWeight'); return false; }
        return true;
    }
    function fromEditable(on) {
        ['CmbStockPartyFrom', 'CmbWareHouseFrom', 'cmbJobLot', 'cmbCropYear', 'cmbItemName', 'cmbPackingType', 'CmbPackUom'].forEach(function (id) { HRM.enable(id, on); });
    }
    function resetDetail() {                                                // ResetDetail
        ['CmbStockPartyFrom', 'CmbStockPartyTo', 'CmbWareHouseFrom', 'cmbWareHouseTo', 'cmbJobLot', 'CmbJobLotTo', 'cmbCropYear', 'cmbItemName', 'cmbPackingType']
            .forEach(function (id) { HRM.setCombo(id, 0); });
        HRM.fill('CmbPackUom', [], 'Id', 'UOMCode', { zero: '' });
        ['txtQty', 'txtGrossWeight', 'txtAdLsWeight', 'txtEbUnit', 'txtEbTotal', 'txtDetailNetWeight', 'txtRemarksDetail'].forEach(function (id) { HRM.setVal(id, ''); });
        HRM.show('btnplus', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        S.updIndex = -1;
        HRM.focus('CmbStockPartyFrom');
    }
    P.btnplus = function () {                                               // btnplus_Click
        if (grd.rows().some(function (r) { return HRM.int(r.RefDocumentTypeId) > 0 || HRM.int(r.RefDocNoId) > 0 || HRM.int(r.RefDocSubIdNo) > 0; })) {
            HRM.box('Record cannot add Because Loader Entry Exist!'); return;
        }
        if (!detailCheck()) return;
        grd.add({ Id: 0, RefDocumentTypeId: 0, RefDocNoId: 0, RefDocSubIdNo: 0,
            StockPartyFromId: HRM.comboVal('CmbStockPartyFrom'), StockPartyFrom: HRM.comboText('CmbStockPartyFrom').trim(),
            StockPartyToId: HRM.comboVal('CmbStockPartyTo'), StockPartyTo: HRM.comboText('CmbStockPartyTo').trim(),
            WareHouseFromId: HRM.comboVal('CmbWareHouseFrom'), WareHouseFrom: HRM.comboText('CmbWareHouseFrom').trim(),
            WareHouseToId: HRM.comboVal('cmbWareHouseTo'), WareHouseTo: HRM.comboText('cmbWareHouseTo').trim(),
            JobLotId: HRM.comboVal('cmbJobLot'), JobLot: HRM.comboText('cmbJobLot').trim(), JobLotIdTo: HRM.comboVal('CmbJobLotTo'), JobLotTo: HRM.comboText('CmbJobLotTo').trim(),
            CropYearId: HRM.comboVal('cmbCropYear'), CropYear: HRM.comboText('cmbCropYear').trim(), ItemId: HRM.comboVal('cmbItemName'), ItemName: HRM.comboText('cmbItemName').trim(),
            PackTypeId: HRM.comboVal('cmbPackingType'), PackType: HRM.comboText('cmbPackingType').trim(), PackUomId: HRM.comboVal('CmbPackUom'), PackUom: HRM.comboText('CmbPackUom').trim(),
            QTY: PPC.dbl('txtQty'), GrossWeight: PPC.dbl('txtGrossWeight'), EbUnit: PPC.dbl('txtEbUnit'), EbTotal: PPC.dbl('txtEbTotal'),
            'Ad/LsWeight': PPC.dbl('txtAdLsWeight'), NetWeight: PPC.dbl('txtDetailNetWeight'), BalQty: 0, BalWeight: 0, Remarks: HRM.val('txtRemarksDetail').trim() });
        balCols();
        resetDetail();
    };
    function editRow(i) {                                                   // grd_DoubleClick
        var r = grd.rows()[i]; if (!r) return;
        S.updIndex = i;
        fromEditable(!(HRM.int(r.RefDocumentTypeId) > 0));
        HRM.setCombo('CmbStockPartyFrom', r.StockPartyFromId);
        if (HRM.int(r.StockPartyToId) > 0) HRM.setCombo('CmbStockPartyTo', r.StockPartyToId);
        HRM.setCombo('CmbWareHouseFrom', r.WareHouseFromId);
        if (HRM.int(r.WareHouseToId) > 0) HRM.setCombo('cmbWareHouseTo', r.WareHouseToId);
        HRM.setCombo('cmbJobLot', r.JobLotId);
        if (HRM.int(r.JobLotIdTo) > 0) HRM.setCombo('CmbJobLotTo', r.JobLotIdTo);
        HRM.setCombo('cmbCropYear', r.CropYearId);
        HRM.setCombo('cmbItemName', r.ItemId);
        uoms(r.ItemId).then(function () {
            HRM.setCombo('cmbPackingType', r.PackTypeId);
            HRM.setCombo('CmbPackUom', r.PackUomId);
            HRM.setVal('txtQty', PPC.fmt(r.QTY, '#,##0.###'));
            HRM.setVal('txtGrossWeight', PPC.fmt(r.GrossWeight, '#,##0.###'));
            HRM.setVal('txtEbUnit', PPC.fmt(r.EbUnit, '#,##0.###'));
            HRM.setVal('txtEbTotal', PPC.fmt(r.EbTotal, '#,##0.###'));
            HRM.setVal('txtAdLsWeight', PPC.fmt(r['Ad/LsWeight'], '#,##0.###'));
            HRM.setVal('txtDetailNetWeight', PPC.fmt(r.NetWeight, '#,##0.###'));
            HRM.setVal('txtRemarksDetail', r.Remarks);
            HRM.show('btnplus', false); HRM.show('btnUpdateDetail', true); HRM.show('btnCancelUpdateDetial', true);
            HRM.focus(HRM.int(r.RefDocumentTypeId) > 0 ? 'CmbStockPartyTo' : 'CmbStockPartyFrom');
        });
    }
    P.btnUpdateDetail = function () {                                       // btnUpdateDetail_Click (CropYearId is not rewritten, as on the desktop)
        if (!detailCheck()) return;
        var r = grd.rows()[S.updIndex]; if (!r) return;
        r.StockPartyFromId = HRM.comboVal('CmbStockPartyFrom'); r.StockPartyFrom = HRM.comboText('CmbStockPartyFrom');
        r.StockPartyToId = HRM.comboVal('CmbStockPartyTo'); r.StockPartyTo = HRM.comboText('CmbStockPartyTo');
        r.WareHouseFromId = HRM.comboVal('CmbWareHouseFrom'); r.WareHouseFrom = HRM.comboText('CmbWareHouseFrom');
        r.WareHouseToId = HRM.comboVal('cmbWareHouseTo'); r.WareHouseTo = HRM.comboText('cmbWareHouseTo');
        r.JobLotId = HRM.comboVal('cmbJobLot'); r.JobLot = HRM.comboText('cmbJobLot');
        r.JobLotIdTo = HRM.comboVal('CmbJobLotTo'); r.JobLotTo = HRM.comboText('CmbJobLotTo');
        r.CropYear = HRM.comboText('cmbCropYear');
        r.ItemId = HRM.comboVal('cmbItemName'); r.ItemName = HRM.comboText('cmbItemName');
        r.PackTypeId = HRM.comboVal('cmbPackingType'); r.PackType = HRM.comboText('cmbPackingType');
        r.PackUomId = HRM.comboVal('CmbPackUom'); r.PackUom = HRM.comboText('CmbPackUom');
        r.QTY = PPC.dbl('txtQty'); r.GrossWeight = PPC.dbl('txtGrossWeight'); r.EbUnit = PPC.dbl('txtEbUnit'); r.EbTotal = PPC.dbl('txtEbTotal');
        r['Ad/LsWeight'] = PPC.dbl('txtAdLsWeight'); r.NetWeight = PPC.dbl('txtDetailNetWeight'); r.Remarks = HRM.val('txtRemarksDetail');
        grd.draw();
        fromEditable(true);
        resetDetail();
    };
    P.btnCancelUpdateDetial = function () {
        S.updIndex = -1; fromEditable(true);
        HRM.show('btnplus', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
    };
    P.btnSameUpdate = function () {                                          // BtnSameUpdateWarehouse_Click
        if (!HRM.checked('chkUpdateStockParty') && !HRM.checked('chkUpdateWareHouse') && !HRM.checked('chkUpdateJobLot')) {
            HRM.box('Please Checked atleast one value (StockParty,warehouse,JobLot) value to Update'); return;
        }
        var rows = grd.rows();
        if (!rows.length) { HRM.box('Grid Record not Found'); return; }
        var f = rows[0];
        if (HRM.int(f.WareHouseToId) === 0) { HRM.box('WareHouseTo Field is Empty in Grid Row No: 01'); return; }
        var sp = [HRM.int(f.StockPartyFromId), HRM.str(f.StockPartyFrom)], wh = [HRM.int(f.WareHouseToId), HRM.str(f.WareHouseTo)], jl = [HRM.int(f.JobLotIdTo), HRM.str(f.JobLotTo)];
        rows.forEach(function (r) {
            if (HRM.checked('chkUpdateWareHouse')) { r.WareHouseToId = wh[0]; r.WareHouseTo = wh[1]; }
            if (HRM.checked('chkUpdateJobLot')) { r.JobLotIdTo = jl[0]; r.JobLotTo = jl[1]; }
            if (HRM.checked('chkUpdateStockParty')) { r.StockPartyToId = sp[0]; r.StockPartyTo = sp[1]; }   // desktop copies the first row's Stock Party FROM
        });
        grd.draw();
    };

    P.btnLoadInvoices = function () {                                       // btnLoadInvoices_Click / LoadDataDetail
        var f = grd.rows()[0];
        if (f && HRM.int(f.RefDocumentTypeId) === 0 && HRM.int(f.RefDocNoId) === 0 && HRM.int(f.RefDocSubIdNo) === 0) {
            HRM.box('You can not add Record From Loader beacause manual record exist in Grid'); return;
        }
        PPC.loader({ page: 'stock-transfer', stockPartyId: 0, onLoad: function (rows) {
            rows.forEach(function (d) {
                var c = function (k) { return HRM.col(d, k); };
                var dup = grd.rows().some(function (g) {
                    return HRM.int(c('RefDocumentTypeId')) === HRM.int(g.RefDocumentTypeId) && HRM.int(c('RefDocIdNo')) === HRM.int(g.RefDocNoId) && HRM.int(c('RefDocSubIdNo')) === HRM.int(g.RefDocSubIdNo);
                });
                if (dup) return;
                grd.data.push({ Id: 0, RefDocumentTypeId: c('RefDocumentTypeId'), RefDocNoId: c('RefDocIdNo'), RefDocSubIdNo: c('RefDocSubIdNo'),
                    StockPartyFromId: c('StockPartyId'), StockPartyFrom: c('StockParty'), StockPartyToId: 0, StockPartyTo: '',
                    WareHouseFromId: c('WarehouseId'), WareHouseFrom: c('WareHouseCode'), WareHouseToId: 0, WareHouseTo: '',
                    JobLotId: c('JobLotId'), JobLot: c('JobLotCode'), JobLotIdTo: 0, JobLotTo: '', CropYearId: c('CropYearId'), CropYear: c('CropYear'),
                    ItemId: c('ItemId'), ItemName: c('ItemName'), PackTypeId: c('InvPackingTypeId'), PackType: c('PackingType'), PackUomId: c('ItemUomId'),
                    PackUom: c('PackUom'), QTY: c('QtyBalance'), GrossWeight: c('WeightBalance'), EbUnit: 0, EbTotal: 0, 'Ad/LsWeight': 0,
                    NetWeight: c('WeightBalance'), BalQty: c('QtyBalance'), BalWeight: c('WeightBalance'), Remarks: '' });
            });
            balCols();
        } });
    };

    // ------------------------------------------------------------------ save / read
    function reset() {                                                      // Reset()
        S.recId = 0; S.approved = false;
        HRM.setVal('txtHeadNetWeight', ''); HRM.setVal('txtRemarksHead', '');
        resetDetail();
        fromEditable(true);
        HRM.show('btnsave', true); HRM.show('btnupdate', false);
        grd.set([]); balCols();
        HRM.focus('DocDate');
        return HRM.get(API + 'code', { recId: 0 }).then(function (c) {
            HRM.setVal('txtdocno', c.docNo); fillTickets(c.tickets); ticketChanged();
        }).catch(HRM.fail);
    }
    P.btnnew = function () { reset(); };
    P.btnRefresh = function (b) { HRM.busy(b, function () { return HRM.get(API + 'refresh').then(fillLookups).catch(HRM.fail); }); };

    function insert(btn) {                                                  // Insert()
        var docNo = HRM.val('txtdocno').trim();
        if (docNo === '' || docNo === '0') { HRM.box('DocNo Field is Required'); return; }
        var ticketSel = HRM.comboVal('cmbTicketNo') > 0;
        if (ticketSel && PPC.dbl('txtHeadNetWeight') === 0) { HRM.box('Factory Weight Field is Required'); HRM.focus('cmbTicketNo'); return; }
        if (!grd.rows().length) { HRM.box('Grid Record Not Found'); return; }
        if (!HRM.ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var body = { id: S.recId, docNo: docNo, docDate: HRM.val('DocDate'), remarks: HRM.val('txtRemarksHead'), ticketId: HRM.comboVal('cmbTicketNo'),
            ticketSelected: ticketSel, headNetWeight: String(PPC.dbl('txtHeadNetWeight')), rows: grd.rows() };
        HRM.busy(btn, function () {
            return HRM.post(API + 'save', body).then(function (r) {
                HRM.box(r.message);
                var id = HRM.int(r.id);
                if (r.openWages) HRM.open('/party-processing/wages-bill?refDocTypeId=220&refDocId=' + id + '&grossWeight=' + encodeURIComponent(r.grossWeightTotal));
                return reset().then(function () { if (HRM.checked('ChkBoxPrintPreview')) report(id, null, false); });
            }).catch(HRM.fail);
        });
    }
    P.btnsave = function (b) { S.recId = 0; insert(b); };
    P.btnupdate = function (b) { if (S.approved) { HRM.box('Approved Record Not Update'); return; } insert(b); };
    function readById(id) {                                                 // ReadById
        return HRM.loading(HRM.get(API + 'read', { id: id }).then(function (h) {
            S.recId = HRM.int(h.Id);
            fillTickets(h.tickets);
            tabs.show('form');
            HRM.setVal('txtdocno', h.DocNo);
            HRM.setVal('DocDate', HRM.day(h.DocDate));
            HRM.setCombo('cmbTicketNo', h.WbTicketId);                       // web: selects the saved ticket (the desktop writes the id into the combo's text)
            ticketChanged();
            HRM.setVal('txtHeadNetWeight', h.WbNetWeight);
            HRM.setVal('txtRemarksHead', h.RemarksHeader);
            S.approved = !!h.IsApproved;
            HRM.show('btnsave', false); HRM.show('btnupdate', true);
            grd.set(h.rows || []); balCols();
        }).catch(HRM.fail));
    }
    function report(id, btn, fromHistory) { return PPC.print(API + 'print', 'ppc-220', { id: id, history: !!fromHistory }, btn); }
    P.btnprint = function (b) { report(S.recId, b, true); };         // the button is not rights-gated on the desktop (Ctrl+P is)

    // ------------------------------------------------------------------ history
    P.btnNewHistory = function () { histDates(); HRM.setVal('txtFromDocNoHistory', ''); HRM.setVal('txtToDocNoHistory', ''); grdhistory.clear(); GridDetailHistory.clear(); };
    P.btnshowHistory = function (b) {
        HRM.busy(b, function () { return HRM.post(API + 'history', PPC.hist()).then(function (rows) { grdhistory.set(rows); GridDetailHistory.clear(); }).catch(HRM.fail); });
    };
    P.shortcuts = function () {
        PPC.shortcuts([['Ctrl+S', 'For Save / Show History'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Ctrl+P', 'For Print'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form']]);
    };

    // ------------------------------------------------------------------ events / keys
    PPC.$('cmbTicketNo').addEventListener('change', ticketChanged);
    PPC.$('cmbItemName').addEventListener('change', function () { uoms(HRM.comboVal('cmbItemName')); availableStock(); });
    ['CmbWareHouseFrom', 'cmbCropYear', 'cmbJobLot'].forEach(function (id) { PPC.$(id).addEventListener('change', availableStock); });
    PPC.$('CmbPackUom').addEventListener('change', function () { grossCalc(); weightCalc(); });
    PPC.$('txtQty').addEventListener('input', function () { grossCalc(); weightCalc(); });
    PPC.$('txtGrossWeight').addEventListener('input', function () { weightCalc(); });
    PPC.$('txtAdLsWeight').addEventListener('input', function () { weightCalc(); });
    PPC.$('txtEbUnit').addEventListener('input', function () { weightCalc('EbUnit'); });
    PPC.$('txtEbTotal').addEventListener('input', function () { weightCalc('EbTotal'); });
    ['txtQty', 'txtHeadNetWeight', 'txtGrossWeight', 'txtEbUnit', 'txtEbTotal', 'txtAdLsWeight'].forEach(function (id) { PPC.guard(id, 'dec'); });
    PPC.guard('txtFromDocNoHistory', 'int'); PPC.guard('txtToDocNoHistory', 'int');
    function onForm() { return tabs.current() === 'form'; }
    HRM.keys({
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+n': function () { if (onForm()) P.btnnew(); else P.btnNewHistory(); },
        'ctrl+r': function () { if (onForm()) P.btnRefresh(PPC.$('btnRefresh')); },
        'ctrl+s': function () { if (onForm()) { if (saveMode()) P.btnsave(PPC.$('btnsave')); } else P.btnshowHistory(PPC.$('btnshowHistory')); },
        'ctrl+u': function () { if (HRM.visible('btnupdate') && !PPC.$('btnupdate').disabled) P.btnupdate(PPC.$('btnupdate')); },
        'ctrl+p': function () { if (onForm() && S.rights.print !== false) P.btnprint(PPC.$('btnprint')); },
        'ctrl+t': function () { tabs.show(onForm() ? 'history' : 'form'); },
        'ctrl+f5': function () { if (onForm()) HRM.focus('DocDate'); }
    });
    HRM.footer(function () { tabs.show('history'); });
    load();
})();
