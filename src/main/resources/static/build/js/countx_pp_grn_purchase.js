/* ============================================================================================
 * countx_pp_grn_purchase.js - Party Processing GRN For Purchase (GRNForPurchaseFromPartyProcessing.cs), ScreenId 613, DocumentTypeId 217.
 * Follows the form: Load (configs, rights, combos, ticket list), GrossWeightCalculations / NetWeightCalculations /
 * CalculationPurchaseAgainstWeight, detail Add / Edit / Update / X, the Empty Bags grid (grdEmptyBags_CellUpdated), the "Load Data"
 * popup, Insert(), reset(), ReadById, history + detail of the selected row, the 211 print, keys. Exports window.PpGp.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/grn-purchase';
    var P = {}; window.PpGp = P;
    var S = { rights: {}, recId: 0, updateIndex: -1, uoms: [], lists: {}, ebStatus: 0, ebAdd: true, defaultDays: 0 };

    function get(path, params) { return HRM.get(API + path, params || {}); }
    function post(path, body, params) { return HRM.post(API + path + (params ? '?' + new URLSearchParams(params).toString() : ''), body); }
    var n = HRM.num;
    function saveMode() { return HRM.visible('btnSave') && !HRM.$('btnSave').disabled; }
    function dt12(v) {
        var d = HRM.day(v); if (!d) return '';
        var p = d.split('-'), t = HRM.time(v) || '00:00', h = +t.substring(0, 2);
        return p[2] + '-' + p[1] + '-' + p[0] + ' ' + String(h % 12 === 0 ? 12 : h % 12).padStart(2, '0') + ':' + t.substring(3, 5) + ' ' + (h < 12 ? 'AM' : 'PM');
    }
    function r2(v) { return PPB.net(PPB.round(v, 2)); }                                      // Math.Round(x, 2).ToString()
    function isLoaderRow(r) { return !!r && (HRM.int(r.RefDocumentTypeId) > 0 || HRM.int(r.RefDocIdNo) > 0 || HRM.int(r.RefDocSubIdNo) > 0); }

    // ------------------------------------------------------------------ grids
    var grd = new HRM.Grid('grd', {                                                              // grdSettings()
        columns: [
            { key: 'Delete', caption: 'X', width: 20, render: function () { return PPB.btnCell('delete', 'X'); } },
            { key: 'Edit', caption: 'Edit', width: 50, render: function () { return PPB.btnCell('edit', 'Edit'); } },
            { key: 'Id', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocIdNo', hidden: true },
            { key: 'RefDocSubIdNo', hidden: true }, { key: 'WarehouseId', hidden: true }, { key: 'Warehouse', caption: 'Warehouse' },
            { key: 'WarehouseToId', hidden: true }, { key: 'WarehouseTo', caption: 'WarehouseTo' }, { key: 'ItemId', hidden: true },
            { key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName' }, { key: 'CropYear', caption: 'CropYear' },
            { key: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot' }, { key: 'PackingTypeId', hidden: true },
            { key: 'PackingType', caption: 'PackingType' }, { key: 'UOMId', hidden: true }, { key: 'UOM', caption: 'UOM', align: 'right' },
            { key: 'UOMEquivalent', hidden: true },
            PPB.numCol('Qty', 'Qty', '#,##.##'), PPB.numCol('GrossWight', 'GrossWight', '#,##.##'), PPB.numCol('EbUnit', 'EbUnit', '#,##.##'),
            PPB.numCol('EbTotal', 'EbTotal', '#,##.##'), PPB.numCol('AddLesswt', 'AddLesswt', '#,##.##'), PPB.numCol('NetWeight', 'NetWeight', '#,##.##'),
            PPB.numCol('StockWeight', 'StockWeight', '#,##.##'), { key: 'BalWeight', hidden: true },
            { key: 'CityId', hidden: true }, { key: 'City', caption: 'City' }
        ],
        totals: true, emptyText: '',
        onDouble: function (r, i) { grdDoubleClick(i); }
    });
    PPB.onButton(grd, 'grd', function (r, act, i) {                                            // grd_ColumnButtonClick
        if (act === 'delete') {
            if (S.updateIndex > -1) { HRM.box('Reset the Detail First...'); return; }
            grd.remove(i);
        } else if (act === 'edit') grdDoubleClick(i);
        HRM.enable('combsupplier', grd.rows().length <= 0);
        disableEnableForLoader();
    });

    var grdEmptyBags = new HRM.Grid('grdEmptyBags', {                                          // grdEmptyBagsSettings()
        columns: [
            { key: 'Add', caption: '+', width: 20, render: function () { return PPB.btnCell('add', '+'); } },
            { key: 'OrderId', hidden: true },
            { key: 'Type', caption: 'EmptyBagsType', type: 'select', width: 80, options: function () { return opts(S.lists.emptyBagTypes, 'Type'); } },
            { key: 'ItemId', caption: 'Item Name', type: 'select', width: 250, options: function () { return opts(S.lists.emptyBagItems, 'ItemName'); } },
            { key: 'Condition', caption: 'Bags_Condition', type: 'select', width: 120, options: function () { return opts(S.lists.bagsConditions, 'Type'); } },
            { key: 'RecQty', caption: 'RecQty', type: 'edit-num', sum: true, decimals: 3 },
            { key: 'PurQty', caption: 'PurQty', type: 'edit-num', sum: true, decimals: 3 },
            { key: 'Remarks', caption: 'Remarks', type: 'edit' }
        ],
        totals: true, emptyText: '',
        onChange: function (r, k) { emptyBagsCellUpdated(r, k); }
    });
    function opts(rows, text) { return [[0, '']].concat((rows || []).map(function (x) { return [x.Id, x[text]]; })); }
    function blankBag() { return { OrderId: 0, Type: 0, ItemId: 0, Condition: 0, RecQty: 0, PurQty: 0, Remarks: '' }; }   // AddRowInvEmptyBagsGrid
    function emptyBagsSettings() {                                                              // the "+" column exists when there is one row
        S.ebAdd = grdEmptyBags.rows().length === 1;
        grdEmptyBags.columnOf('Add').hidden = !S.ebAdd;
        var head = '<tr>' + grdEmptyBags.columns.map(function (c) {
            var style = (c.hidden ? 'display:none;' : '') + (c.width ? 'min-width:' + c.width + 'px;width:' + c.width + 'px;' : '');
            return '<th data-key="' + HRM.esc(c.key) + '" style="' + style + '">' + HRM.esc(c.caption === undefined ? c.key : c.caption) + '</th>';
        }).join('') + '</tr>';
        grdEmptyBags.table.tHead.innerHTML = head;
        grdEmptyBags.draw();
    }
    PPB.onButton(grdEmptyBags, 'grdEmptyBags', function (r, act) {                             // grdEmptyBags_ColumnButtonClick
        if (act === 'add' && grdEmptyBags.rows().length === 1) grdEmptyBags.add(blankBag());
    });
    /** grdEmptyBags_CellUpdated */
    function emptyBagsCellUpdated(r, col) {
        if (HRM.int(r.PurQty) < 0) { r.PurQty = 0; HRM.box('PurQty cannot be less than Zero'); }
        if (HRM.int(r.RecQty) < 0) { r.RecQty = 0; HRM.box('RecQty cannot be less than Zero'); }
        if (col === 'PurQty' || col === 'Type') {
            S.ebStatus = HRM.int(r.Type);
            if (S.ebStatus === 2) calculationPurchaseAgainstWeight();
        }
        if ((HRM.int(r.Type) === 4 || HRM.int(r.Type) === 5) && col === 'PurQty') { r.PurQty = 0; HRM.box('Retained or Returned Stock you cannot be purchase'); }
        if (HRM.int(r.Type) === 2 && col === 'RecQty') { r.RecQty = 0; HRM.box('You cannot be add value RecQty because EmptyBagsType is Purchase Against Weight'); }
        grdEmptyBags.draw();
    }

    var GrdHistory = new HRM.Grid('GrdHistory', {                                               // HistoryGridSettings()
        columns: [
            { key: 'Print', caption: 'Print', width: 40, render: function () { return PPB.btnCell('print', 'Print'); } },
            { key: 'Edit', caption: 'Edit', width: 40, render: function () { return PPB.btnCell('edit', 'Edit'); } },
            { key: 'RecordNo', hidden: true }, { key: 'Id', hidden: true },
            { key: 'InvoiceNo', caption: 'InvoiceNo', type: 'int', width: 80 }, { key: 'DocNo', caption: 'DocNo', type: 'int', width: 60 },
            { key: 'DocDate', caption: 'DocDate', type: 'date', width: 80 }, { key: 'DocumentTypeId', hidden: true },
            { key: 'DeliveryTerm', caption: 'DeliveryTerm', width: 100 }, { key: 'SupplierName', caption: 'SupplierName', width: 150 },
            { key: 'GpNo', caption: 'GpNo', type: 'int', width: 60 }, { key: 'VehicleNo', caption: 'VehicleNo', width: 70 },
            { key: 'BiltyNo', caption: 'BiltyNo', width: 70 }, { key: 'WbTicketId', hidden: true }, { key: 'TicketNo', caption: 'TicketNo', type: 'int' },
            PPB.numCol('FactoryWeight', 'FactoryWeight', '#,##0.####'), { key: 'WagesId', hidden: true },
            { key: 'WagesNo', caption: 'WagesNo', type: 'int', width: 70 }, { key: 'Transporter', caption: 'Transporter' },
            PPB.numCol('FrieghtAmount', 'Freight Amount', '#,##0.##'), { key: 'RemarksHeader', caption: 'RemarksHeader' },
            { key: 'EntryDate', caption: 'EntryDate', width: 150, render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'EntryUser', caption: 'EntryUser' },
            { key: 'ModifyDate', caption: 'ModifyDate', width: 150, render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'ApprovedDate', caption: 'ApprovedDate', width: 150, render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'ApprovedUser', caption: 'ApprovedUser' }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' },
            { key: 'AttachmentsCount', hidden: true }, { key: 'DetailIdsCount', hidden: true }, { key: 'AddWages', caption: 'AddWages', type: 'check' }
        ],
        filterRow: true, totals: true, emptyText: '',
        onDouble: function (r) {                                                                // GrdHistory_DoubleClick
            if (HRM.int(r.InvoiceNo) > 0) { HRM.box("This Document is referred in invoice. So you can't update this record."); return; }
            readById(HRM.int(r.Id));
        },
        onSelect: function (r) { historyDetail(HRM.int(r.Id)); }                                 // GrdHistory_SelectionChanged
    });
    PPB.onButton(GrdHistory, 'GrdHistory', function (r, act) {                                 // GrdHistory_ColumnButtonClick
        if (act === 'edit') {
            if (HRM.int(r.InvoiceNo) > 0) { HRM.box("This Document is referred in invoice. So you can't update this record."); return; }
            reset().then(function () { readById(HRM.int(r.Id)); });
        } else if (act === 'print') printSlip(HRM.int(r.Id), null);
    });
    var GrdHistoryDetail = new HRM.Grid('GrdHistoryDetail', {                                   // gridhistorydetail(OrderExist)
        columns: [
            { key: 'OrderId', hidden: true }, { key: 'OrderNo', caption: 'OrderNo', type: 'int', hidden: true },
            { key: 'WareHouse', caption: 'WareHouse' }, { key: 'Item', caption: 'Item' }, { key: 'CropYear', caption: 'CropYear' },
            { key: 'JobLot', caption: 'Job/Lot' }, { key: 'PackingType', caption: 'PackingType' }, { key: 'UOM', caption: 'UOM' },
            PPB.numCol('Qty', 'Qty', '#,##0.###'), PPB.numCol('GrossWight', 'GrossWight', '#,##0.####'), PPB.numCol('EbUnit', 'EbUnit', '#,##0.###'),
            PPB.numCol('EbTotal', 'EbTotal', '#,##0.###'), PPB.numCol('AddLesswt', 'AddLesswt', '#,##0.###'),
            PPB.numCol('NetWeight', 'NetWeight', '#,##0.####'), PPB.numCol('StockWeight', 'StockWeight', '#,##0.####'), { key: 'City', caption: 'City' }
        ],
        totals: true, emptyText: ''
    });

    // ------------------------------------------------------------------ lists
    function fillLists(o, keep) {
        ['emptyBagTypes', 'bagsConditions', 'emptyBagItems'].forEach(function (k) { if (o[k]) S.lists[k] = o[k]; });
        Object.keys(o).forEach(function (k) { S.lists[k] = o[k]; });
        HRM.fill('combsupplier', o.suppliers || [], 'Id', 'SupplierName', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbTransport', o.transporters || [], 'Id', 'AccountTitle', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('combvehtyp', o.vehicleTypes || [], 'Id', 'VehicleDescription', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbWareHouse', o.warehouses || [], 'Id', 'WareHouseName', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbWarehouseTo', o.warehouses || [], 'Id', 'WareHouseName', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbJobLot', o.jobLots || [], 'Id', 'JobLotDescription', { zero: '...Select Any Value...', keep: keep });
        itemFill(keep);
        HRM.fill('combpcktyp', o.packingTypes || [], 'Id', 'PackTypeDesc', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbCity', o.cities || [], 'Id', 'CityName', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('combcropyear', o.cropYears || [], 'Id', 'CropYear', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbTicketNo', o.tickets || [], 'Id', 'TicketNo', { zero: '', keep: keep });
        grdEmptyBags.draw();
    }
    /** ItemNameFill() / rdSearchByName_CheckedChanged: the item combo shows the name or the code. */
    function itemFill(keep) {
        var byName = HRM.$('rdSearchByName').checked;
        HRM.fill('combitem', S.lists.items || [], 'Id', byName ? 'ItemName' : 'ItemCode', { zero: '...Select Any Value...', keep: keep });
    }
    function itemRow() { return PPB.rowOf(S.lists.items, 'Id', HRM.comboVal('combitem')) || {}; }
    /** PackUOMFillWithoutOrder() */
    function packUomFill() {
        var prev = HRM.comboText('combpckuom');
        var itemId = HRM.comboVal('combitem');
        if (!itemId) { S.uoms = []; HRM.fill('combpckuom', [], 'Id', 'UOMCode', { zero: false }); HRM.$('combpckuom').selectedIndex = -1; HRM.refreshCombos(); return Promise.resolve(); }
        return get('/uoms', { itemId: itemId }).then(function (rows) {
            S.uoms = rows || [];
            HRM.fill('combpckuom', S.uoms, 'Id', 'UOMCode', { zero: '...Select Any Value...' });
            if (prev && S.uoms.some(function (u) { return u.UOMCode === prev; })) HRM.setComboText('combpckuom', prev);
            else HRM.setCombo('combpckuom', 0);
        }).catch(HRM.fail);
    }
    function uomEq() { var r = PPB.rowOf(S.uoms, 'Id', HRM.val('combpckuom')); return r ? n(r.Equivalent) : 0; }

    // ------------------------------------------------------------------ calculations
    /** GrossWeightCalculations() */
    function grossCalc() {
        var gross = n(HRM.val('txtgwt'));
        if (HRM.comboVal('combpckuom') > 0) {
            var eq = uomEq(), qty = n(HRM.val('txtqty'));
            if (saveMode()) gross = qty * eq;
        }
        HRM.setVal('txtgwt', PPB.fmt(gross, '#,##.###'));
        HRM.setVal('txtnetwt', r2(gross)); HRM.setVal('txtstockwt', r2(gross));
    }
    /** NetWeightCalculations(): E.b by the focused box (Tag EbUnit / EbTotal), the purchase-against-weight bags added to the bill weight. */
    function netCalc(active) {
        var qty = HRM.val('txtqty') !== '' ? n(HRM.val('txtqty')) : 0;
        var gross = HRM.val('txtgwt') !== '' ? n(HRM.val('txtgwt')) : 0;
        var ebUnit = HRM.val('txtebu') !== '' ? n(HRM.val('txtebu')) : 0;
        var ebTotal = n(HRM.val('txtEmptyBagsTotal'));
        var purQty = 0, grdQty = 0, tpaw = 0;
        if (ebUnit > 0 || ebTotal > 0) {
            if (active === 'EbUnit') { ebTotal = ebUnit * qty; HRM.setVal('txtEmptyBagsTotal', r2(ebTotal)); }
            else if (active === 'EbTotal') { ebUnit = ebTotal / qty; HRM.setVal('txtebu', isFinite(ebUnit) ? PPB.net(PPB.round(ebUnit, 3)) : ''); }
            else { ebTotal = ebUnit > 0 && qty > 0 ? ebUnit * qty : 0; HRM.setVal('txtEmptyBagsTotal', r2(ebTotal)); }
        }
        if (ebUnit > 0) {
            S.ebStatus = 0;
            grdQty = grd.sum('Qty');
            grdEmptyBags.rows().forEach(function (e) { if (HRM.int(e.Type) === 2) purQty += n(e.PurQty); });
            if (purQty > 0) {
                var upd = HRM.visible('btnUpdateDetail') && grd.rows()[S.updateIndex] ? n(grd.rows()[S.updateIndex].Qty) : 0;
                var num = Math.abs(purQty) - (grdQty - upd);
                S.ebStatus = 2;
                tpaw = num * ebUnit;
            }
        }
        var addLess = HRM.val('txtadlswt') !== '' ? n(HRM.val('txtadlswt')) : 0;
        var weight = 0;
        if (S.ebStatus === 2 && tpaw > 0) weight = gross - ebTotal + addLess + tpaw;
        else if (S.ebStatus === 2) weight = gross + addLess;
        else weight = gross - ebTotal + addLess;
        HRM.setVal('txtnetwt', r2(weight));
        HRM.setVal('txtstockwt', r2(gross - ebTotal));
    }
    /** CalculationPurchaseAgainstWeight(): every detail row's E.b total, net weight and stock weight. */
    function calculationPurchaseAgainstWeight() {
        var factory = HRM.val('txtfctwt') !== '' ? n(HRM.val('txtfctwt')) : 0;
        var allocate = 0, purQty = 0, ebLess = 0;
        grdEmptyBags.rows().forEach(function (e) { if (HRM.int(e.Type) === 2) purQty += n(e.PurQty); });
        grd.rows().forEach(function (r) {
            var tpaw = 0, qty = n(r.Qty), gross = n(r.GrossWight), ebUnit = n(r.EbUnit), addLess = n(r.AddLesswt), ebTotal;
            if (ebUnit > 0) {
                var grdQty = qty;
                if (purQty > 0) {
                    purQty -= allocate;
                    if (purQty > grdQty) ebLess = grdQty; else if (grdQty > purQty) ebLess = purQty;
                    if (purQty === grdQty) ebLess = grdQty;
                    if (purQty > 0) tpaw = ebLess * ebUnit;
                }
            }
            if (ebUnit > 0 && qty > 0) { r.EbTotal = ebUnit * qty; ebTotal = n(r.EbTotal); } else { r.EbTotal = 0; ebTotal = 0; }
            var weight = gross - ebTotal + addLess + tpaw;
            r.NetWeight = PPB.round(weight, 2);
            if (factory > 0 && gross > 0) r.StockWeight = PPB.round(factory * gross / factory - ebTotal, 2);
            else r.StockWeight = PPB.round(weight, 2);
            allocate = ebLess;
        });
        grd.draw();
    }

    // ------------------------------------------------------------------ detail
    /** FormValidationDetail() */
    function detailValid() {
        if (HRM.comboVal('CmbDeliveryTerm') === 0) { HRM.box('DeliveryTerm Field is Required'); HRM.focus('CmbDeliveryTerm'); return false; }
        if (HRM.val('txtfctwt').trim() === '' || n(HRM.val('txtfctwt')) === 0) { HRM.box('Factory Weight Field is Required'); HRM.focus('txtfctwt'); return false; }
        if (HRM.comboVal('CmbWareHouse') === 0) { HRM.box('Warehouse Field is Required'); HRM.focus('CmbWareHouse'); return false; }
        if (HRM.comboVal('CmbWarehouseTo') === 0) { HRM.box('WarehouseTo Field is Required'); HRM.focus('CmbWarehouseTo'); return false; }
        if (HRM.comboVal('combitem') === 0) { HRM.box('Item Name Field is Required'); HRM.focus('combitem'); return false; }
        if (HRM.comboVal('combcropyear') === 0) { HRM.box('Crop Year Field is Required'); HRM.focus('combcropyear'); return false; }
        if (HRM.comboVal('CmbJobLot') === 0) { HRM.box('JobLot Field is Required'); HRM.focus('CmbJobLot'); return false; }
        if (HRM.comboVal('combpcktyp') === 0) { HRM.box('Packing Type Field is Required'); HRM.focus('combpcktyp'); return false; }
        if (HRM.comboVal('combpckuom') === 0) { HRM.box('Pack Unit Field is Required'); HRM.focus('combpckuom'); return false; }
        if (HRM.val('txtqty').trim() === '' || n(HRM.val('txtqty')) === 0) { HRM.box('Qty Field is Required'); HRM.focus('txtqty'); return false; }
        if (HRM.val('txtgwt').trim() === '' || n(HRM.val('txtgwt')) === 0) { HRM.box('Gross Weight Field is Required'); HRM.focus('txtgwt'); return false; }
        if (HRM.val('txtnetwt').trim() === '' || n(HRM.val('txtnetwt')) === 0) { HRM.box('Net Bill Weight Field is Required'); HRM.focus('txtnetwt'); return false; }
        if (HRM.val('txtstockwt').trim() === '' || n(HRM.val('txtstockwt')) === 0) { HRM.box('Stock Weight Field is Required'); HRM.focus('txtstockwt'); return false; }
        if (HRM.comboVal('CmbCity') === 0) { HRM.box('City Field is Required'); HRM.focus('CmbCity'); return false; }
        return true;
    }
    function detailFields(r) {
        var it = itemRow();
        r.WarehouseId = HRM.comboVal('CmbWareHouse'); r.Warehouse = HRM.comboText('CmbWareHouse').trim();
        r.WarehouseToId = HRM.comboVal('CmbWarehouseTo'); r.WarehouseTo = HRM.comboText('CmbWarehouseTo').trim();
        r.ItemId = HRM.comboVal('combitem'); r.ItemCode = HRM.str(it.ItemCode); r.ItemName = HRM.str(it.ItemName);
        r.CropYear = HRM.comboText('combcropyear').trim(); r.JobLotId = HRM.comboVal('CmbJobLot'); r.JobLot = HRM.comboText('CmbJobLot').trim();
        r.PackingTypeId = HRM.comboVal('combpcktyp'); r.PackingType = HRM.comboText('combpcktyp');
        r.UOMId = HRM.comboVal('combpckuom'); r.UOM = HRM.comboText('combpckuom').trim(); r.UOMEquivalent = uomEq();
        r.Qty = n(HRM.val('txtqty')); r.GrossWight = n(HRM.val('txtgwt')); r.EbUnit = n(HRM.val('txtebu')); r.EbTotal = n(HRM.val('txtEmptyBagsTotal'));
        r.AddLesswt = n(HRM.val('txtadlswt')); r.NetWeight = n(HRM.val('txtnetwt')); r.StockWeight = n(HRM.val('txtstockwt'));
        r.CityId = HRM.comboVal('CmbCity'); r.City = HRM.comboText('CmbCity');
        return r;
    }
    P.add = function () {                                                                       // Add_Click
        if (isLoaderRow(grd.rows()[0])) { HRM.box('You can not add manual Record beacause record against Loader exist in Grid'); return; }
        if (!detailValid()) return;
        if (n(HRM.val('txtnetwt')) > n(HRM.val('txtgwt'))) { HRM.box('NetBillWeight cannot greater than Gross Weight Please check'); return; }
        var r = detailFields({ Id: 0, RefDocumentTypeId: 0, RefDocIdNo: 0, RefDocSubIdNo: 0 });
        r.BalWeight = n(HRM.val('txtstockwt'));
        grd.add(r);
        resetDetail();
        calculationPurchaseAgainstWeight();
    };
    function grdDoubleClick(i) {                                                               // grd_DoubleClick
        var r = grd.rows()[i]; if (!r) return;
        S.updateIndex = i;
        HRM.setCombo('CmbWareHouse', r.WarehouseId);
        HRM.setCombo('CmbWarehouseTo', r.WarehouseToId);
        HRM.setCombo('combitem', r.ItemId);
        packUomFill().then(function () { HRM.setCombo('combpckuom', r.UOMId); });
        HRM.setComboText('combcropyear', HRM.str(r.CropYear));
        HRM.setCombo('CmbJobLot', r.JobLotId);
        HRM.setCombo('combpcktyp', r.PackingTypeId);
        HRM.setVal('txtqty', PPB.net(r.Qty)); HRM.setVal('txtgwt', PPB.net(r.GrossWight));
        HRM.setVal('txtebu', PPB.net(r.EbUnit)); HRM.setVal('txtEmptyBagsTotal', PPB.net(r.EbTotal));
        HRM.setVal('txtadlswt', PPB.net(r.AddLesswt)); HRM.setVal('txtnetwt', PPB.net(r.NetWeight)); HRM.setVal('txtstockwt', PPB.net(r.StockWeight));
        HRM.setCombo('CmbCity', r.CityId);
        HRM.show('Add', false); HRM.show('btnUpdateDetail', true); HRM.show('btnCancelUpdateDetial', true);
        HRM.focus('CmbWareHouse');
        HRM.enable('combitem', true);
        disableEnableForLoader();
    }
    /** DisableEnableFieldsInCaseOfLoader(): a loader row keeps its warehouse / item / crop / job lot / packing / UOM. */
    function disableEnableForLoader() {
        var lock = grd.rows().length > 0 && isLoaderRow(grd.current());
        ['CmbWareHouse', 'combitem', 'combcropyear', 'CmbJobLot', 'combpcktyp', 'combpckuom'].forEach(function (id) { HRM.enable(id, !lock); });
    }
    P.btnUpdateDetail = function () {                                                           // btnUpdateDetail_Click
        if (!detailValid()) return;
        if (n(HRM.val('txtnetwt')) > n(HRM.val('txtgwt'))) { HRM.box('NetBillWeight cannot greater than Gross Weight Please check'); return; }
        var r = grd.rows()[S.updateIndex]; if (!r) return;
        detailFields(r);
        var first = grd.rows()[0];
        if (first && (HRM.int(first.RefDocumentTypeId) === 0 || HRM.int(first.RefDocIdNo) === 0 || HRM.int(first.RefDocSubIdNo) === 0)) r.BalWeight = n(HRM.val('txtstockwt'));
        HRM.show('Add', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        resetDetail();
        grd.draw();
        calculationPurchaseAgainstWeight();
    };
    P.btnCancelUpdateDetial = function () {
        HRM.show('Add', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        S.updateIndex = -1;
    };
    /** ResetDetial(): clearing Qty / Gross / Ad-Ls re-runs the weight calculations (E.b Total follows E.b / Unit). */
    function resetDetail() {
        S.updateIndex = -1;
        HRM.setCombo('CmbWareHouse', 0); HRM.setCombo('CmbWarehouseTo', 0); HRM.setCombo('CmbJobLot', 0);
        HRM.setVal('txtqty', ''); grossCalc(); netCalc();
        HRM.setVal('txtgwt', ''); HRM.setVal('txtadlswt', ''); netCalc();
        HRM.setVal('txtnetwt', ''); HRM.setVal('txtstockwt', '');
        HRM.setCombo('combpckuom', 0);
        HRM.focus('CmbWareHouse');
    }

    // ------------------------------------------------------------------ Load Data popup
    P.btnLoadAvailableData = function () {                                                      // btnLoadAvailableData_Click
        var first = grd.rows()[0];
        if (first && !isLoaderRow(first)) { HRM.box('You can not add Record From Loader because manual record exist in Grid'); return; }
        if (HRM.comboVal('combsupplier') === 0) { HRM.focus('combsupplier'); HRM.box('select Party Account First'); return; }
        PPB.loader({ get: get, post: post, stockPartyId: HRM.comboVal('combsupplier'), canLoad: true, onLoad: loadFromLoader });
    };
    /** LoadDataDetailfromPurchaseInvoivce() */
    function loadFromLoader(rows) {
        if (rows.length) {
            if (grd.rows().length > 0 && HRM.comboVal('combsupplier') !== HRM.int(rows[0].StockPartyId)) { HRM.box('Data against another Stock Party Already Exist'); return; }
            rows.forEach(function (x) {
                var dup = grd.rows().some(function (r) {
                    return HRM.int(x.RefDocumentTypeId) === HRM.int(r.RefDocumentTypeId) && HRM.int(x.RefDocIdNo) === HRM.int(r.RefDocIdNo) &&
                        HRM.int(x.RefDocSubIdNo) === HRM.int(r.RefDocSubIdNo);
                });
                if (dup) return;
                grd.rows().push({
                    Id: 0, RefDocumentTypeId: x.RefDocumentTypeId, RefDocIdNo: x.RefDocIdNo, RefDocSubIdNo: x.RefDocSubIdNo, WarehouseId: x.WarehouseId,
                    Warehouse: x.WareHouse, WarehouseToId: 0, WarehouseTo: '', ItemId: x.ItemId, ItemCode: x.ItemCode, ItemName: x.ItemName,
                    CropYear: x.CropYear, JobLotId: x.JobLotId, JobLot: x.JobLotCode, PackingTypeId: x.InvPackingTypeId, PackingType: x.PackingType,
                    UOMId: x.ItemUomId, UOM: x.PackUom, UOMEquivalent: x.Equivalent, Qty: x.QtyBalance, GrossWight: x.WeightBalance, EbUnit: 0,
                    EbTotal: 0, AddLesswt: 0, NetWeight: x.WeightBalance, StockWeight: x.WeightBalance, BalWeight: x.WeightBalance, CityId: 0, City: ''
                });
            });
            grd.draw();
        }
        HRM.enable('combsupplier', grd.rows().length <= 0);
    }

    // ------------------------------------------------------------------ load / reset / read
    function buttons(update) { HRM.show('btnSave', !update); HRM.show('btnUpdate', update); HRM.show('btnDelete', update); }
    function load() {
        return HRM.loading(get('/setup')).then(function (o) {
            S.rights = o.rights || {};
            S.defaultDays = HRM.int(o.defaultDays);
            HRM.applyRights(S.rights, { save: 'btnSave', print: 'BtnPrint', update: 'btnUpdate', delete: 'btnDelete' });
            HRM.setVal('txtebu', '0'); HRM.setVal('txtadlswt', '0');
            HRM.enable('txtebu', !!o.emptyBagsWeightCutEditable); HRM.enable('txtEmptyBagsTotal', !!o.emptyBagsWeightCutEditable);
            HRM.enable('txtadlswt', !!o.addLessWeightEditable);
            fillLists(o, false);
            HRM.fillFixed('CmbDeliveryTerm', [[0, ''], [1, 'Load'], [2, 'Ponch']]); HRM.$('CmbDeliveryTerm').value = '1';
            HRM.setVal('txtdocno', o.docNo > 0 ? o.docNo : '');
            grdEmptyBags.set([blankBag()]); emptyBagsSettings();
            HRM.fill('cmbSupplierNameHistory', o.historySuppliers || [], 'Id', 'name', { zero: '' });
            PPB.resetHistoryFilters(S.defaultDays);
            buttons(false);
            HRM.focus('DocDate');
        }).catch(HRM.fail);
    }
    /** reset() */
    function reset() {
        S.recId = 0; S.ebStatus = 0; S.updateIndex = -1;
        HRM.check('chkAddWages', false);
        HRM.setCombo('combsupplier', 0); HRM.$('CmbDeliveryTerm').value = '0'; HRM.setCombo('CmbTransport', 0);
        ['txtcarramount', 'txtGpNo', 'txtvehno', 'txtbltyno', 'txtfctwt', 'txtremarks', 'txtebu', 'txtEmptyBagsTotal', 'txtqty', 'txtadlswt',
            'txtnetwt', 'txtstockwt'].forEach(function (id) { HRM.setVal(id, ''); });
        var vt = S.lists.vehicleTypes || []; HRM.setCombo('combvehtyp', vt.length ? vt[0].Id : 0);
        ['combsupplier', 'CmbTransport', 'txtcarramount', 'txtgwt', 'txtfctwt', 'CmbTicketNo', 'combitem', 'CmbDeliveryTerm'].forEach(function (id) { HRM.enable(id, true); });
        HRM.setCombo('combitem', 0); HRM.setCombo('combpckuom', 0); HRM.setCombo('CmbCity', 0);
        grd.clear();
        buttons(false);
        HRM.show('Add', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        grdEmptyBags.set([blankBag()]); emptyBagsSettings();
        disableEnableForLoader();
        HRM.focus('DocDate');
        return Promise.all([
            get('/code').then(function (o) { HRM.setVal('txtdocno', o.docNo > 0 ? o.docNo : ''); }),
            get('/refresh', { recId: 0 }).then(function (o) { HRM.fill('CmbTicketNo', o.tickets || [], 'Id', 'TicketNo', { zero: '' }); S.lists.tickets = o.tickets || []; })
        ]).catch(HRM.fail);
    }
    /** ReadById(ID) */
    function readById(id) {
        buttons(true);
        S.recId = id;
        return HRM.loading(get('/by-id', { id: id })).then(function (o) {
            if (!(o.details || []).length) return;
            PPB.showTab('form');
            HRM.setVal('txtdocno', HRM.str(o.DocNo)); HRM.setVal('DocDate', HRM.day(o.DocDate));
            HRM.setCombo('combsupplier', o.SupplierCustomerId); HRM.enable('combsupplier', true);
            HRM.setComboText('CmbDeliveryTerm', HRM.str(o.DeliveryTerm));
            HRM.setCombo('CmbTransport', o.TransporterId);
            HRM.setVal('txtcarramount', PPB.net(o.CarriageAmount)); HRM.setVal('txtremarks', HRM.str(o.RemarksHeader));
            HRM.setVal('txtGpNo', HRM.str(o.GpNo)); HRM.setComboText('combvehtyp', HRM.str(o.VehicleType));
            HRM.setVal('txtvehno', HRM.str(o.VehicleNo)); HRM.setVal('txtbltyno', HRM.str(o.BiltyNo));
            HRM.setVal('txtfctwt', PPB.net(o.FactoryWeight)); HRM.check('chkAddWages', HRM.bool(o.AddWages));
            grd.set(o.details.map(function (d) { return Object.assign({}, d); }));
            var bags = (o.emptyBags || []).map(function (b) { return Object.assign({}, b); });
            grdEmptyBags.set(bags.length ? bags : [blankBag()]); emptyBagsSettings();
            return get('/refresh', { recId: id }).then(function (x) {                            // TicketNofill() with RecId
                S.lists.tickets = x.tickets || [];
                HRM.fill('CmbTicketNo', S.lists.tickets, 'Id', 'TicketNo', { zero: '' });
                HRM.setCombo('CmbTicketNo', HRM.int(o.details[0].WbTicketId));
                HRM.enable('txtfctwt', HRM.comboVal('CmbTicketNo') <= 0);
            });
        }).catch(HRM.fail);
    }
    function historyDetail(id) {
        return get('/by-id', { id: id }).then(function (o) {
            var rows = (o.details || []).map(function (d) {
                return { OrderId: d.OrderId, OrderNo: d.OrderNo, WareHouse: d.WarehouseTo, Item: d.ItemName, CropYear: d.CropYear, JobLot: d.JobLot,
                    PackingType: d.PackingType, UOM: d.UOMEquivalent, Qty: d.Qty, GrossWight: d.GrossWight, EbUnit: d.EbUnit, EbTotal: d.EbTotal,
                    AddLesswt: d.AddLesswt, NetWeight: d.NetWeight, StockWeight: d.StockWeight, City: d.City };
            });
            var orderExist = rows.some(function (r) { return HRM.int(r.OrderId) > 0; });
            GrdHistoryDetail.columnOf('OrderNo').hidden = !orderExist;
            var head = '<tr>' + GrdHistoryDetail.columns.map(function (c) {
                return '<th data-key="' + HRM.esc(c.key) + '" style="' + (c.hidden ? 'display:none;' : '') + '">' + HRM.esc(c.caption === undefined ? c.key : c.caption) + '</th>';
            }).join('') + '</tr>';
            GrdHistoryDetail.table.tHead.innerHTML = head;
            GrdHistoryDetail.set(rows);
        }).catch(function () { GrdHistoryDetail.clear(); });
    }

    // ------------------------------------------------------------------ save / delete / print
    function insert(btn) {                                                                      // Insert()
        if (HRM.val('txtdocno').trim() === '' || HRM.val('txtdocno').trim() === '0') { HRM.box('DocNo Field is Required'); return; }
        if (HRM.comboVal('combsupplier') === 0) { HRM.box('Supplier Field is Required'); HRM.focus('combsupplier'); return; }
        if (HRM.comboVal('CmbDeliveryTerm') === 0) { HRM.box('DeliveryTerm Field is Required'); HRM.focus('CmbDeliveryTerm'); return; }
        if (n(HRM.val('txtfctwt')) === 0) { HRM.box('Factory Weight Field is Required'); HRM.focus('txtfctwt'); return; }
        if (!grd.rows().length) { HRM.box('Grid Record Not Found'); return; }
        if (!HRM.ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var preview = HRM.checked('ChkBox');
        var body = {
            id: S.recId, docNo: HRM.val('txtdocno'), docDate: HRM.val('DocDate'), supplierId: HRM.comboVal('combsupplier'),
            deliveryTerm: HRM.comboText('CmbDeliveryTerm'), deliveryTermId: HRM.comboVal('CmbDeliveryTerm'), remarks: HRM.val('txtremarks'),
            gpNo: HRM.val('txtGpNo'), vehicleType: HRM.comboText('combvehtyp'), vehicleNo: HRM.val('txtvehno'), biltyNo: HRM.val('txtbltyno'),
            factoryWeight: HRM.val('txtfctwt'), addWages: HRM.checked('chkAddWages'), transporterId: HRM.comboVal('CmbTransport'),
            carriageAmount: HRM.val('txtcarramount'), ticketId: HRM.comboVal('CmbTicketNo'), rows: grd.rows(), emptyBags: grdEmptyBags.rows()
        };
        return HRM.busy(btn, function () {
            return post('/save', body).then(function (res) {
                HRM.box(res.message);
                var w = preview ? CrystalPrint.reserve() : null;
                return reset().then(function () { if (preview) printSlip(res.id, null, w); });
            }).catch(HRM.fail);
        });
    }
    /** GenerateReport(PrintId) -> CommonServices.GrnSlipWithSubReports(Id, 217): the 211 slip. */
    function printSlip(id, btn, win) {
        var w = win || CrystalPrint.reserve();
        return HRM.busy(btn, function () {
            return get('/slip', { id: id }).then(function () { return CrystalPrint.open('grn-211', { id: id }, null, w); })
                .catch(function (e) { CrystalPrint.release(w); HRM.fail(e); });
        }, 'gp-print');
    }
    P.btnSave = function (btn) { S.recId = 0; return insert(btn || 'btnSave'); };
    P.btnUpdate = function (btn) { if (S.recId === 0) { HRM.box('Record not update because Id not found'); return; } return insert(btn || 'btnUpdate'); };
    P.btnNew = function () { return reset(); };
    P.btnRefresh = function (btn) {                                                             // BtnRefresh_Click
        return HRM.busy(btn || 'BtnRefresh', function () { return get('/refresh', { recId: S.recId }).then(function (o) { fillLists(o, true); }).catch(HRM.fail); });
    };
    P.btnDelete = function (btn) {                                                              // btnDelete_Click
        if (S.recId <= 0) { HRM.box('Record Not Found'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return post('/delete', {}, { id: S.recId }).then(function (res) { HRM.box(res.message); return reset(); }).catch(HRM.fail);
        });
    };
    P.btnPrint = function (btn) { return printSlip(S.recId, btn || 'BtnPrint'); };
    P.shortcuts = function () {                                                                 // MakeShortCutKeys()
        HRM.box('Ctrl+S  For Save\nCtrl+U  For Update\nCtrl+Shift+Delete  For Delete\nCtrl+E  For Close\nCtrl+R  For Refresh\nCtrl+N  For New\n' +
            'Ctrl+P  For Print\nCtrl+H  For History Print\nCtrl+F5  For Focus on Doc Date\nCtrl+F10  For Open Attachments\nCtrl+T  For Tab Transfer\n' +
            'Ctrl+alt  To Show ShortCut Keys Form\nCtrl+ArrowDown  For Focus On Detail Grid\nCtrl+ArrowUp  For Focus On warehouse Combo in Detail Box\n' +
            'Ctrl+ArrowRight  For Focus From One Grid To Another\nCtrl+Enter  For Update Record When Focus On Any Grid\n' +
            'Ctrl+Space  To Call Function\'s On Button Or Link When Focus On Any Grid');
    };

    // ------------------------------------------------------------------ history
    P.btnshow = function (btn) {                                                                // HistoryGridFill()
        var ref = document.querySelector('input[name="histRef"]:checked');
        return HRM.busy(btn || 'btnshow', function () {
            return post('/history', PPB.historyFilters({ supplierId: HRM.comboVal('cmbSupplierNameHistory'), actionId: ref ? HRM.int(ref.value) : 0 }))
                .then(function (rows) { GrdHistory.set(rows || []); GrdHistoryDetail.clear(); }).catch(HRM.fail);
        });
    };
    P.btnNewHistory = function () {                                                             // btnNewHistory_Click: both dates today
        HRM.setVal('FromDateHistory', HRM.today()); HRM.setVal('ToDateHistory', HRM.today());
        HRM.setVal('txtFromDocNoHistory', ''); HRM.setVal('txtToDocNoHistory', '');
        HRM.setCombo('cmbSupplierNameHistory', 0);
        GrdHistory.clear(); GrdHistoryDetail.clear();
    };
    P.btnRefreshHistory = function (btn) {                                                      // HistoryComboFill()
        return HRM.busy(btn || 'btnRefreshHistory', function () {
            return get('/history-suppliers').then(function (rows) { HRM.fill('cmbSupplierNameHistory', rows || [], 'Id', 'name', { zero: '', keep: true }); }).catch(HRM.fail);
        });
    };

    // ------------------------------------------------------------------ wiring
    PPB.tabs(function (name) { HRM.focus(name === 'history' ? 'FromDateHistory' : 'DocDate'); });
    HRM.footer(function () { PPB.showTab('history'); });
    HRM.$('combitem').addEventListener('change', function () { packUomFill(); });              // combitem.ValueChanged / Leave
    HRM.$('rdSearchByName').addEventListener('change', function () { var v = HRM.comboVal('combitem'); itemFill(false); HRM.setCombo('combitem', v); HRM.focus('combitem'); });
    HRM.$('rdSearchByCode').addEventListener('change', function () { var v = HRM.comboVal('combitem'); itemFill(false); HRM.setCombo('combitem', v); HRM.focus('combitem'); });
    PPB.leave('combpckuom', function () { grossCalc(); netCalc(); });                           // combpckuom_Leave
    PPB.leave('CmbJobLot', function () { grossCalc(); netCalc(); });                            // CmbJobLot.Leave -> combpckuom_Leave
    PPB.leave('CmbTicketNo', function () {                                                      // CmbTicketNo_Leave
        var t = PPB.rowOf(S.lists.tickets, 'Id', HRM.val('CmbTicketNo'));
        if (t && HRM.comboVal('CmbTicketNo') > 0) { HRM.setVal('txtfctwt', PPB.fmt(t.NetWbWeight, '#,##.###')); HRM.enable('txtfctwt', false); }
        else { HRM.setVal('txtfctwt', ''); HRM.enable('txtfctwt', true); }
    });
    HRM.$('txtqty').addEventListener('input', function () { grossCalc(); netCalc(); });
    HRM.$('txtgwt').addEventListener('input', function () { netCalc(); });
    HRM.$('txtebu').addEventListener('input', function () { netCalc('EbUnit'); });
    HRM.$('txtEmptyBagsTotal').addEventListener('input', function () { netCalc('EbTotal'); });
    HRM.$('txtadlswt').addEventListener('input', function () { netCalc(); });
    HRM.$('txtadlswt').addEventListener('blur', function () { netCalc(); });
    ['txtgwt', 'txtebu', 'txtqty', 'txtEmptyBagsTotal', 'txtfctwt'].forEach(function (id) { PPB.guard(id, 'dec'); });
    PPB.guard('txtadlswt', 'signed'); PPB.guard('txtcarramount', 'int'); PPB.guard('txtGpNo', 'int');
    PPB.guard('txtFromDocNoHistory', 'int'); PPB.guard('txtToDocNoHistory', 'int');
    HRM.setVal('DocDate', HRM.today());
    PPB.resetHistoryFilters(3);
    function focusGrid(id) { var t = HRM.$(id); var tr = t && t.querySelector('tbody tr'); if (tr) tr.focus(); }
    function formTab() { return PPB.currentTab() === 'form'; }
    HRM.keys({
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+t': function () { PPB.toggleTab(); },
        'ctrl+alt+alt': P.shortcuts, 'ctrl+alt+control': P.shortcuts,
        'ctrl+n': function () { if (formTab()) P.btnNew(); else P.btnNewHistory(); },
        'ctrl+r': function () { if (!formTab()) P.btnRefreshHistory(); },
        'ctrl+s': function () { if (!formTab()) P.btnshow(); else if (saveMode()) P.btnSave(); },
        'ctrl+u': function () { if (formTab() && HRM.visible('btnUpdate') && !HRM.$('btnUpdate').disabled) P.btnUpdate(); },
        'ctrl+shift+delete': function () { if (formTab() && HRM.visible('btnDelete') && !HRM.$('btnDelete').disabled) P.btnDelete(); },
        'ctrl+p': function () {
            if (formTab()) { if (HRM.visible('BtnPrint') && !HRM.$('BtnPrint').disabled) P.btnPrint(); }
            else { var r = GrdHistory.current(); if (r) printSlip(HRM.int(r.Id), null); }
        },
        'ctrl+f5': function () { if (formTab()) HRM.focus('DocDate'); },
        'ctrl+arrowdown': function () { focusGrid(formTab() ? 'grd' : 'GrdHistory'); },
        'ctrl+arrowup': function () { HRM.focus(formTab() ? 'CmbWareHouse' : 'FromDateHistory'); },
        'ctrl+arrowright': function () {
            if (!formTab()) return;
            var inGrd = document.activeElement && document.activeElement.closest && document.activeElement.closest('#grd');
            focusGrid(inGrd ? 'grdEmptyBags' : 'grd');
        }
    });
    load();
})();
