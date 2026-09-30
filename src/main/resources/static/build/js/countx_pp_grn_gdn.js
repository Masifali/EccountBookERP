/* ============================================================================================
 * countx_pp_grn_gdn.js - Party Processing Goods Receiving / Dispatch Notes (PartyProGrnAndGdn.cs)
 *   ?mode=grn -> 687, DocumentTypeId 49 (gate pass 54)      ?mode=gdn -> 688, DocumentTypeId 89 (gate pass 55)
 * Follows the form: Load, the detail entry box (Total / TotalWeight / TotalSupplierWeight), Add / edit / update / delete rows,
 * the editable detail grid (grd_CellUpdated on a GDN), Empty Bag grid, Gate Pass Info grid (Load -> GatePassRecordFill, Advance DO
 * rows on a GDN), the "Load Data" transactions popup (LoadavailableTransactionsForIssuancePartyProcessing), Insert(), reset(),
 * ReadById, history + detail of the selected row, 333 / 339 prints, keys, Template I / II.
 * Exports window.PpGg.
 * ============================================================================================ */
(function () {
    'use strict';
    var MODE = document.body.getAttribute('data-mode') || 'grn';
    var DOC = MODE === 'gdn' ? 89 : 49;
    var API = '/api/party-processing/grn-gdn';
    var P = {}; window.PpGg = P;
    var S = {
        rights: {}, recId: 0, gpId: 0, gpQty: 0, gpType: '', removed: [], uoms: [], updateIndex: -1,
        allowMultiple: false, defaultDays: 0, lists: {}, loader: null
    };

    function q(extra) { var o = { mode: MODE }; Object.keys(extra || {}).forEach(function (k) { o[k] = extra[k]; }); return o; }
    function get(path, extra) { return HRM.get(API + path, q(extra)); }
    function post(path, body, extra) { return HRM.post(API + path + '?' + new URLSearchParams(q(extra)).toString(), body); }
    function n(v) { return HRM.num(v); }
    function saveMode() { return HRM.visible('btnSave') && !HRM.$('btnSave').disabled; }       // btnSave.Visible && btnSave.Enabled
    function dt12(v) {
        var d = HRM.day(v); if (!d) return '';
        var mon = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
        var p = d.split('-'), t = HRM.time(v) || '00:00', h = +t.substring(0, 2);
        return p[2] + '-' + mon[+p[1] - 1] + '-' + p[0] + ' ' + String(h % 12 === 0 ? 12 : h % 12).padStart(2, '0') + ':' + t.substring(3, 5) + ' ' + (h < 12 ? 'AM' : 'PM');
    }
    function anyGt0(rows, k) { return rows.some(function (r) { return HRM.int(r[k]) > 0; }); }    // GridEX_Helper.IsAnyCellValueGreaterThanZero

    // ------------------------------------------------------------------ grid combo columns (GridDetailComboBind)
    function comboCol(key, caption, listKey, textKey, nameKey, lockable) {
        return {
            key: key, caption: caption, type: 'select',
            options: function (r) {
                var list = (S.lists[listKey] || []).map(function (x) { return [x.Id, x[textKey]]; });
                var v = HRM.int(r[key]);
                if (!list.some(function (o) { return HRM.int(o[0]) === v; })) list.unshift([v, v ? HRM.str(r[nameKey]) || String(v) : '']);
                return list;
            },
            readOnly: lockable ? function () { return locked(); } : null
        };
    }
    /** grdSettings(): WareHouseId / CropYearId / JobId / PackingTypeId become read only on a GDN with loader rows, or with Advance DO rows
     *  when AllowMultipleOrPartialInvoicingForADO is on. */
    function locked() {
        var rows = grd.rows();
        return DOC === 89 && (anyGt0(rows, 'RefDocumentTypeId') || (anyGt0(rows, 'DeliveryOrderId') && S.allowMultiple));
    }
    function numEdit(key, caption) {
        return { key: key, caption: caption, type: 'edit-num', sum: true, decimals: 3 };
    }

    // ------------------------------------------------------------------ grids
    var grd = new HRM.Grid('grd', {
        columns: [
            { key: 'Delete', caption: 'X', width: 20, render: function () { return PPB.btnCell('delete', 'X'); } },
            { key: 'Add', caption: '+', width: 20, render: function () { return PPB.btnCell('add', '+'); } },
            { key: 'Id', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocIdNo', hidden: true },
            { key: 'RefDocSubIdNo', hidden: true }, { key: 'DoDocumentTypeId', hidden: true }, { key: 'DeliveryOrderId', hidden: true },
            { key: 'DeliveryOrderDetailId', hidden: true },
            { key: 'DeliveryOrderNo', caption: 'DeliveryOrderNo', type: 'int', hidden: true },
            { key: 'InvoiceDocumentTypeId', hidden: true }, { key: 'InvoiceId', hidden: true }, { key: 'InvoiceDetailId', hidden: true },
            { key: 'InvoiceDocType', caption: 'InvoiceDocType', hidden: true },
            { key: 'InvoiceNo', caption: 'InvoiceNo', type: 'int', hidden: true },
            { key: 'InvoiceManualBillNo', caption: 'InvoiceManualBillNo', hidden: true },
            comboCol('WareHouseId', 'WareHouse Name', 'warehouses', 'WareHouseName', 'WareHouseName', true),
            { key: 'ItemId', hidden: true },
            { key: 'Item', caption: 'Item' },
            comboCol('CropYearId', 'Crop Year', 'cropYears', 'CropYear', 'CropYear', true),
            comboCol('JobId', 'Job Lot', 'jobLots', 'JobLotDescription', 'JobLot', true),
            comboCol('PackingTypeId', 'Packing Type', 'packingTypes', 'PackTypeDesc', 'PackingType', true),
            { key: 'PackUomId', hidden: true },
            { key: 'PackUom', caption: 'PackUom' },
            { key: 'Equivalent', hidden: true },
            numEdit('Qty', 'Qty'), numEdit('GrossWight', 'GrossWight'), numEdit('EbUnit', 'EbUnit'), numEdit('EbTotal', 'EbTotal'),
            numEdit('AddLesswt', 'AddLesswt'),
            PPB.numCol('NetBillWeight', 'NetBillWeight'), PPB.numCol('StockWeight', 'StockWeight'),
            { key: 'BalWeight', hidden: true },
            comboCol('CityId', 'City Name', 'cities', 'CityName', 'CityName', false),
            { key: 'Comments', caption: 'Comments', type: 'edit' },
            { key: 'ContainerNo', caption: 'ContainerNo', type: 'edit', hidden: DOC === 49 },
            { key: 'SealNo', caption: 'SealNo', type: 'edit', hidden: DOC === 49 },
            Object.assign(PPB.numCol('InvoiceQty', 'InvoiceQty'), { hidden: true }), Object.assign(PPB.numCol('InvoiceWeight', 'InvoiceWeight'), { hidden: true }),
            Object.assign(PPB.numCol('InvoiceEbUnit', 'InvoiceEbUnit'), { hidden: true }), Object.assign(PPB.numCol('InvoiceEbTotal', 'InvoiceEbTotal'), { hidden: true })
        ],
        totals: true, emptyText: '',
        onDouble: function (r, i) { grdDoubleClick(i); },
        onChange: function (r, k) { cellUpdated(r, k); }
    });
    PPB.onButton(grd, 'grd', function (r, act, i) {                                              // grd_ColumnButtonClick
        if (act === 'delete') deleteDetailRecord(i);
        // "Add" -> AddDetailRow(r) has an empty body on the desktop.
    });

    /** Re-renders the header after a column's Visible changed (GridEX RootTable.Columns[x].Visible). */
    function redrawHead(grid) {
        var head = '<tr>' + grid.columns.map(function (c) {
            var style = (c.hidden ? 'display:none;' : '') + (c.width ? 'min-width:' + c.width + 'px;width:' + c.width + 'px;' : '');
            return '<th data-key="' + HRM.esc(c.key) + '" style="' + style + '">' + HRM.esc(c.caption === undefined ? c.key : c.caption) + '</th>';
        }).join('') + '</tr>';
        grid.table.tHead.innerHTML = head;
        grid.draw();
    }
    /** grdSettings(): visibility of the DO columns and the X / + buttons, panel5 and the stock party on a GDN with loader / DO rows. */
    function grdSettings() {
        var rows = grd.rows();
        var hasRef = anyGt0(rows, 'RefDocumentTypeId'), hasDo = anyGt0(rows, 'DeliveryOrderId');
        var btns = DOC === 49 || !S.allowMultiple;
        grd.columnOf('Delete').hidden = !btns;
        grd.columnOf('Add').hidden = !btns;
        ['DeliveryOrderNo', 'InvoiceDocType', 'InvoiceNo', 'InvoiceManualBillNo', 'InvoiceQty', 'InvoiceWeight', 'InvoiceEbUnit', 'InvoiceEbTotal']
            .forEach(function (k) { grd.columnOf(k).hidden = !(DOC === 89 && hasDo); });
        if (DOC === 89 && (hasRef || hasDo)) {
            HRM.show('panel5', false);
            HRM.enable('CmbStockParty', false);
        }
        redrawHead(grd);
    }

    var grdEmptyBags = new HRM.Grid('grdEmptyBags', {                                          // grdEmptyBagsSettings()
        columns: [
            { key: 'Delete', caption: 'X', width: 20, render: function () { return PPB.btnCell('delete', 'X'); } },
            { key: 'Add', caption: '+', width: 20, render: function () { return PPB.btnCell('add', '+'); } },
            { key: 'ItemId', caption: 'Item Name', type: 'select', width: 250,
                options: function () { return [[0, '']].concat((S.lists.emptyBagItems || []).map(function (x) { return [x.Id, x.ItemName]; })); } },
            { key: 'Qty', caption: 'Qty', type: 'edit-num', sum: true, decimals: 3 },
            { key: 'Remarks', caption: 'Remarks', type: 'edit' }
        ],
        totals: true, emptyText: ''
    });
    function blankBag() { return { ItemId: 0, Qty: 0, Remarks: '' }; }                          // AddRowInvEmptyBagsGrid
    PPB.onButton(grdEmptyBags, 'grdEmptyBags', function (r, act, i) {                          // grdEmptyBags_ColumnButtonClick
        if (act === 'delete') { grdEmptyBags.remove(i); if (!grdEmptyBags.rows().length) grdEmptyBags.add(blankBag()); }
        else if (act === 'add') grdEmptyBags.add(blankBag());
    });

    var grdGp = new HRM.Grid('grdGp', {                                                         // Gatepass() / GRNGridSetting()
        columns: [
            { key: 'Load', caption: 'Load', width: 40, render: function () { return PPB.btnCell('load', 'Load'); } },
            { key: 'Id', hidden: true }, { key: 'GatepassType', caption: 'GatepassType' },
            { key: 'GpNo', caption: 'GpNo', type: 'int' }, { key: 'GpDate', caption: 'GpDate', type: 'date' },
            { key: 'Status', caption: 'Status' }, { key: 'StockPartyId', hidden: true }, { key: 'StockParty', caption: 'StockParty' },
            { key: 'RefPartyId', hidden: true }, { key: 'ReferenceParty', caption: 'ReferenceParty' },
            { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'BiltyNo', caption: 'BiltyNo' }, { key: 'ItemName', caption: 'ItemName' },
            PPB.numCol('ItemQty', 'ItemQty'), PPB.numCol('SupplierWeight', 'SupplierWeight'), PPB.numCol('FactoryWeight', 'FactoryWeight')
        ],
        totals: true, emptyText: ''
    });
    PPB.onButton(grdGp, 'grdGp', function (r) { loadGatePass(r); });                           // grdGp_ColumnButtonClick

    var GrdHistory = new HRM.Grid('GrdHistory', {                                               // HistoryGridSettings()
        columns: [
            { key: 'Edit', caption: 'Edit', width: 40, render: function () { return PPB.btnCell('edit', 'Edit'); } },
            { key: 'Print', caption: 'Print', width: 40, render: function () { return PPB.btnCell('print', 'Print'); } },
            { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
            { key: 'DocDate', caption: 'DocDate', type: 'date' }, { key: 'DocNo', caption: 'DocNo', type: 'int' },
            { key: 'GatePassId', hidden: true }, { key: 'GpNo', caption: 'GpNo', type: 'int' },
            { key: 'StockPartyName', caption: 'StockPartyName' }, { key: 'ReferencePartyName', caption: 'ReferencePartyName' },
            { key: 'TradeToCompany', caption: 'TradeToCompany' }, { key: 'TransporterName', caption: 'TransporterName' },
            PPB.numCol('Freight', 'Freight', '#,##0.##'),
            { key: 'VehicleType', caption: 'VehicleType' }, { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'BiltyNo', caption: 'BiltyNo' },
            PPB.numCol('PartyWeight', 'PartyWeight'), PPB.numCol('FactoryWeight', 'FactoryWeight'),
            { key: 'EntryDate', caption: 'EntryDate', render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'EntryUser', caption: 'EntryUser' },
            { key: 'ModifyDate', caption: 'ModifyDate', render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' },
            { key: 'RemarksHeader', caption: 'RemarksHeader' }
        ],
        filterRow: true, totals: true, emptyText: '',
        onDouble: function (r) { readById(HRM.int(r.Id)); },                                     // GrdHistory_DoubleClick
        onSelect: function (r) { detailGridBind(HRM.int(r.Id)); }                                // GrdHistory_SelectionChanged
    });
    PPB.onButton(GrdHistory, 'GrdHistory', function (r, act) {                                  // GrdHistory_ColumnButtonClick
        if (act === 'edit') { reset().then(function () { readById(HRM.int(r.Id)); }); }
        else if (act === 'print') printSlip(HRM.int(r.Id), HRM.int(r.DocumentTypeId), null);
    });
    var GrdHistoryDetail = new HRM.Grid('GrdHistoryDetail', {                                   // grdHistoryDetailSettings()
        columns: [
            { key: 'DeliveryOrderNo', caption: 'DeliveryOrderNo', type: 'int', hidden: true },
            { key: 'InvoiceDocType', caption: 'InvoiceDocType', hidden: true }, { key: 'InvoiceNo', caption: 'InvoiceNo', type: 'int', hidden: true },
            { key: 'InvoiceManualBillNo', caption: 'InvoiceManualBillNo', hidden: true },
            { key: 'WareHouseName', caption: 'WareHouseName' }, { key: 'Item', caption: 'Item' }, { key: 'CropYear', caption: 'CropYear' },
            { key: 'JobLot', caption: 'JobLot' }, { key: 'PackingType', caption: 'PackingType' }, { key: 'PackUom', caption: 'PackUom' },
            PPB.numCol('Qty', 'Qty'), PPB.numCol('GrossWight', 'GrossWight'), PPB.numCol('EbUnit', 'EbUnit'), PPB.numCol('EbTotal', 'EbTotal'),
            PPB.numCol('AddLesswt', 'AddLesswt'), PPB.numCol('NetBillWeight', 'NetBillWeight'), PPB.numCol('StockWeight', 'StockWeight'),
            { key: 'CityName', caption: 'CityName' }, { key: 'Comments', caption: 'Comments' },
            { key: 'ContainerNo', caption: 'ContainerNo', hidden: DOC === 49 }, { key: 'SealNo', caption: 'SealNo', hidden: DOC === 49 },
            Object.assign(PPB.numCol('InvoiceQty', 'InvoiceQty'), { hidden: true }), Object.assign(PPB.numCol('InvoiceWeight', 'InvoiceWeight'), { hidden: true }),
            PPB.numCol('InvoiceEbUnit', 'InvoiceEbUnit'), PPB.numCol('InvoiceEbTotal', 'InvoiceEbTotal')
        ],
        totals: true, emptyText: ''
    });

    // ------------------------------------------------------------------ lists
    function fillLists(o, keep) {
        S.lists = o;
        var k = { keep: keep };
        HRM.fill('CmbStockParty', o.stockParties || [], 'Id', 'CompanyName', { zero: '', keep: keep });
        HRM.fill('CmbRefParty', o.refParties || [], 'Id', 'ReferencePartyName', { zero: '', keep: keep });
        HRM.fill('CmbTransport', o.refParties || [], 'Id', 'ReferencePartyName', { zero: '', keep: keep });
        HRM.fill('CmbTradeToCompany', o.tradeToCompany || [], 'Id', 'LookupName', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('combvehtyp', o.vehicleTypes || [], 'Id', 'VehicleDescription', { zero: false, keep: keep });
        fillFirst('CmbCropYear', o.cropYears || [], 'CropYear', keep);                             // ZeroIndex true + Rows[1].Activate()
        HRM.fill('CmbJobLot', o.jobLots || [], 'Id', 'JobLotDescription', { zero: '...Select Any Value...', keep: keep });
        fillFirst('CmbPackingtype', o.packingTypes || [], 'PackTypeDesc', keep);
        HRM.fill('CmbWareHouse', o.warehouses || [], 'Id', 'WareHouseName', { zero: '...Select Any Value...', keep: keep });
        HRM.fill('CmbCity', o.cities || [], 'Id', 'CityName', { zero: '', keep: keep });
        HRM.fill('CmbItem', o.items || [], 'Id', 'ItemName', { zero: '...Select Any Value...', keep: keep });
        grd.draw(); grdEmptyBags.draw();
        return k;
    }
    function fillFirst(id, rows, text, keep) {
        var had = keep ? HRM.comboVal(id) : 0;
        HRM.fill(id, rows, 'Id', text, { zero: '...Select Any Value...' });
        if (had && HRM.hasOption(id, had)) HRM.setCombo(id, had);
        else if (rows.length) HRM.setCombo(id, rows[0].Id);
    }
    /** PackUOMFillWithoutOrder(): the item's UOM schedule; keeps the typed pack UOM text when it is still in the list. */
    function packUomFill() {
        var prev = HRM.comboText('CmbPackUom');
        var itemId = HRM.comboVal('CmbItem');
        if (!itemId) { S.uoms = []; HRM.fill('CmbPackUom', [], 'Id', 'UOMCode', { zero: false }); HRM.$('CmbPackUom').selectedIndex = -1; HRM.refreshCombos(); return Promise.resolve(); }
        return get('/uoms', { itemId: itemId }).then(function (rows) {
            S.uoms = rows || [];
            HRM.fill('CmbPackUom', S.uoms, 'Id', 'UOMCode', { zero: false });
            HRM.$('CmbPackUom').selectedIndex = -1;
            if (prev) HRM.setComboText('CmbPackUom', prev);
            HRM.refreshCombos();
        }).catch(HRM.fail);
    }
    function packUomSelected() { var s = HRM.$('CmbPackUom'); return s.selectedIndex >= 0 && s.value !== '' && HRM.int(s.value) > 0; }
    function packEquivalent() { var r = PPB.rowOf(S.uoms, 'Id', HRM.val('CmbPackUom')); return r ? n(r.Equivalent) : 0; }

    // ------------------------------------------------------------------ entry box calculations
    /** Total(): E.b unit / total by the focused box (Tag "EbUnit" / "EbTotal"), Stock Weight = Gross - E.b Total + Ad/Ls (rounded 2, 0 when not > 0). */
    function total(active) {
        var qty = n(HRM.val('txtQty')), gross = n(HRM.val('txtGrossWeight'));
        var ebUnit = n(HRM.val('txtebu')), ebTotal = n(HRM.val('txtEmptyBagsTotal'));
        if (active === 'EbUnit') { ebTotal = ebUnit * qty; HRM.setVal('txtEmptyBagsTotal', PPB.fmt(ebTotal, '#,##0.###')); }
        else if (active === 'EbTotal') { ebUnit = ebTotal / qty; HRM.setVal('txtebu', isFinite(ebUnit) ? PPB.fmt(ebUnit, '#,##0.#####') : ''); }
        else { ebUnit = qty > 0 ? ebTotal / qty : 0; HRM.setVal('txtebu', PPB.fmt(ebUnit, '#,##0.#####')); }
        var addLess = HRM.val('txtadlswt') !== '' ? n(HRM.val('txtadlswt')) : 0;
        var w = gross - ebTotal + addLess;
        HRM.setVal('txtstockwt', w > 0 ? PPB.net(PPB.round(w, 2)) : '0');
    }
    /** TotalWeight(): Gross Weight = pack UOM equivalent * Qty. */
    function totalWeight() {
        var eq = packUomSelected() && HRM.comboText('CmbPackUom') !== '' ? packEquivalent() : 0;
        HRM.setVal('txtGrossWeight', PPB.net(eq * n(HRM.val('txtQty'))));
    }
    /** TotalSupplierWeight(): Weight Diff = Supp Weight - Fact Weight (both typed). */
    function totalSupplierWeight() {
        if (HRM.val('txtsuppwt') !== '' && HRM.val('txtfctwt') !== '') HRM.setVal('txtwtdiff', PPB.net(n(HRM.val('txtsuppwt')) - n(HRM.val('txtfctwt'))));
    }
    function grossBalance() {                                                                   // Add_Click / btnUpdateDetail_Click tail
        var gridWeight = 0; grd.rows().forEach(function (r) { gridWeight += n(r.GrossWight); });
        return n(HRM.val('txtfctwt')) - gridWeight;
    }

    // ------------------------------------------------------------------ detail rows
    /** FormValidationDetail() */
    function detailValid() {
        if (HRM.comboVal('CmbWareHouse') <= 0) { HRM.box('Warehouse Name Field is Required'); HRM.focus('CmbWareHouse'); return false; }
        if (HRM.comboVal('CmbItem') <= 0) { HRM.box('Item Name Field is Required'); HRM.focus('CmbItem'); return false; }
        if (HRM.comboVal('CmbCropYear') <= 0) { HRM.box('Crop Year Field is Required'); HRM.focus('CmbCropYear'); return false; }
        if (HRM.comboVal('CmbJobLot') <= 0) { HRM.box('Job/Lot Field is Required'); HRM.focus('CmbJobLot'); return false; }
        if (HRM.comboVal('CmbPackingtype') <= 0) { HRM.box('Packing Type Field is Required'); HRM.focus('CmbPackingtype'); return false; }
        if (!packUomSelected()) { HRM.box('Pack Unit Field is Required'); HRM.focus('CmbPackUom'); return false; }
        if (HRM.val('txtQty') === '' || n(HRM.val('txtQty')) === 0) { HRM.box('Qty Field is Required'); HRM.focus('txtQty'); return false; }
        if (HRM.val('txtGrossWeight') === '' || n(HRM.val('txtGrossWeight')) === 0) { HRM.box('Gross Weight Field is Required'); HRM.focus('txtGrossWeight'); return false; }
        if (HRM.val('txtstockwt') === '' || n(HRM.val('txtstockwt')) === 0) { HRM.box('Stock Weight Field is Required'); HRM.focus('txtstockwt'); return false; }
        if (HRM.comboVal('CmbCity') <= 0) { HRM.box('City Field is Required'); HRM.focus('CmbCity'); return false; }
        return true;
    }
    function names(r) {
        r.WareHouseName = HRM.comboText('CmbWareHouse'); r.CropYear = HRM.comboText('CmbCropYear'); r.JobLot = HRM.comboText('CmbJobLot');
        r.PackingType = HRM.comboText('CmbPackingtype'); r.CityName = HRM.comboText('CmbCity');
        return r;
    }
    function manualGuard(what) {                                                               // Add_Click / grd_DoubleClick (GDN)
        var rows = grd.rows();
        if (DOC === 89 && rows.length) {
            if (anyGt0(rows, 'DeliveryOrderId')) { HRM.box('You can not ' + what + ' beacause record against Do exist in Grid'); return false; }
            if (anyGt0(rows, 'RefDocumentTypeId') || anyGt0(rows, 'RefDocIdNo')) { HRM.box('You can not ' + what + ' beacause record against Loader exist in Grid'); return false; }
        }
        return true;
    }
    P.add = function () {                                                                       // Add_Click
        if (!manualGuard('add manual Record')) return;
        if (!detailValid()) return;
        var sw = n(HRM.val('txtstockwt'));
        grd.add(names({
            Id: 0, RefDocumentTypeId: 0, RefDocIdNo: 0, RefDocSubIdNo: 0, DoDocumentTypeId: 0, DeliveryOrderId: 0, DeliveryOrderDetailId: 0,
            DeliveryOrderNo: 0, InvoiceDocumentTypeId: 0, InvoiceId: 0, InvoiceDetailId: 0, InvoiceDocType: '0', InvoiceNo: 0, InvoiceManualBillNo: '0',
            WareHouseId: HRM.comboVal('CmbWareHouse'), ItemId: HRM.comboVal('CmbItem'), Item: HRM.comboText('CmbItem'),
            CropYearId: HRM.comboVal('CmbCropYear'), JobId: HRM.comboVal('CmbJobLot'), PackingTypeId: HRM.comboVal('CmbPackingtype'),
            PackUomId: HRM.int(HRM.val('CmbPackUom')), PackUom: HRM.comboText('CmbPackUom').trim(), Equivalent: packEquivalent(),
            Qty: n(HRM.val('txtQty')), GrossWight: n(HRM.val('txtGrossWeight')), EbUnit: n(HRM.val('txtebu')), EbTotal: n(HRM.val('txtEmptyBagsTotal')),
            AddLesswt: n(HRM.val('txtadlswt')), NetBillWeight: sw, StockWeight: sw, BalWeight: 0, CityId: HRM.comboVal('CmbCity'),
            Comments: HRM.val('txtCommentsDetail').trim(), ContainerNo: HRM.val('txtContainerNo').trim(), SealNo: HRM.val('txtSealNo').trim(),
            InvoiceQty: 0, InvoiceWeight: 0, InvoiceEbUnit: 0, InvoiceEbTotal: 0
        }));
        grdSettings();
        resetDetail();
        var fw = n(HRM.val('txtfctwt'));
        if (fw > 0) { HRM.setVal('txtGrossWeight', PPB.net(grossBalance())); total(); }
    };
    function grdDoubleClick(i) {                                                               // grd_DoubleClick
        if (!manualGuard('Update like manual Record')) return;
        var r = grd.rows()[i]; if (!r) return;
        S.updateIndex = i;
        HRM.setCombo('CmbWareHouse', r.WareHouseId);
        HRM.setCombo('CmbItem', r.ItemId);
        HRM.setCombo('CmbCropYear', r.CropYearId);
        HRM.setCombo('CmbJobLot', r.JobId);
        HRM.setCombo('CmbPackingtype', r.PackingTypeId);
        packUomFill().then(function () { HRM.setCombo('CmbPackUom', r.PackUomId); });
        HRM.setVal('txtQty', PPB.net(r.Qty));
        HRM.setVal('txtGrossWeight', PPB.net(r.GrossWight));
        HRM.setVal('txtebu', PPB.net(r.EbUnit));
        HRM.setVal('txtEmptyBagsTotal', PPB.net(r.EbTotal));
        HRM.setVal('txtadlswt', PPB.net(r.AddLesswt));
        HRM.setVal('txtstockwt', PPB.net(r.StockWeight));
        HRM.setCombo('CmbCity', r.CityId);
        HRM.setVal('txtCommentsDetail', HRM.str(r.Comments));
        HRM.setVal('txtContainerNo', HRM.str(r.ContainerNo));
        HRM.setVal('txtSealNo', HRM.str(r.SealNo));
        HRM.show('Add', false); HRM.show('btnUpdateDetail', true); HRM.show('btnCancelUpdateDetial', true);
        HRM.focus('CmbWareHouse');
    }
    P.btnUpdateDetail = function () {                                                           // btnUpdateDetail_Click
        if (!detailValid()) return;
        var r = grd.rows()[S.updateIndex]; if (!r) return;
        var sw = n(HRM.val('txtstockwt'));
        r.WareHouseId = HRM.comboVal('CmbWareHouse'); r.ItemId = HRM.comboVal('CmbItem'); r.Item = HRM.comboText('CmbItem');
        r.CropYearId = HRM.comboVal('CmbCropYear'); r.JobId = HRM.comboVal('CmbJobLot'); r.PackingTypeId = HRM.comboVal('CmbPackingtype');
        r.PackUomId = HRM.int(HRM.val('CmbPackUom')); r.PackUom = HRM.comboText('CmbPackUom'); r.Equivalent = packEquivalent();
        r.Qty = n(HRM.val('txtQty')); r.GrossWight = n(HRM.val('txtGrossWeight')); r.EbUnit = n(HRM.val('txtebu'));
        r.EbTotal = n(HRM.val('txtEmptyBagsTotal')); r.AddLesswt = n(HRM.val('txtadlswt'));
        r.StockWeight = sw; r.BalWeight = sw;
        r.NetBillWeight = sw;   // the desktop leaves NetBillWeight at the old value here (stale net bill weight saved) - fixed
        r.CityId = HRM.comboVal('CmbCity');
        r.Comments = HRM.val('txtCommentsDetail').trim(); r.ContainerNo = HRM.val('txtContainerNo').trim(); r.SealNo = HRM.val('txtSealNo').trim();
        names(r);
        grd.draw();
        HRM.show('Add', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        resetDetail();
        HRM.setVal('txtGrossWeight', PPB.net(grossBalance())); total();
    };
    P.btnCancelUpdateDetial = function () {
        HRM.show('Add', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
    };
    /** ResetDetial() */
    function resetDetail() {
        ['txtQty', 'txtGrossWeight', 'txtebu', 'txtEmptyBagsTotal', 'txtadlswt', 'txtstockwt', 'txtCommentsDetail', 'txtContainerNo', 'txtSealNo']
            .forEach(function (id) { HRM.setVal(id, ''); });
        HRM.$('CmbPackUom').selectedIndex = -1; HRM.refreshCombos();
        HRM.focus('CmbWareHouse');
        HRM.enable('CmbStockParty', grd.rows().length <= 0);
    }
    /** DeleteDetailRecord(r): a saved row goes to lstRemoveRecord (ActionTypeId 3) while updating. */
    function deleteDetailRecord(i) {
        var r = grd.rows()[i]; if (!r) return;
        if (!HRM.ask('Are you sure to Delete?')) return;
        if (!saveMode()) S.removed.push(JSON.parse(JSON.stringify(r)));
        grd.remove(i);
        if (S.gpType !== 'AdvanceDO' && grd.rows().length === 0) { HRM.enable('CmbStockParty', true); HRM.show('panel5', true); }
        grdSettings();
    }
    /** grd_UpdatingCell + grd_CellUpdated: the GDN recalculates the row. */
    function cellUpdated(row, col) {
        if ((col === 'EbUnit' || col === 'EbTotal') && n(row[col]) < 0) HRM.box('Please Type Only Positive Numeric Value');
        if (DOC !== 89) { grd._totals(); return; }
        var qty = n(row.Qty), uom = n(row.Equivalent), gross = n(row.GrossWight), ebUnit = n(row.EbUnit), ebTotal = n(row.EbTotal);
        var addLess = n(row.AddLesswt), invW = n(row.InvoiceWeight), invQ = n(row.InvoiceQty);
        if (col === 'Qty') {
            var w = uom * qty; row.GrossWight = w; row.NetBillWeight = w - ebTotal + addLess;
            row.StockWeight = (invQ !== 0 ? invW / invQ : 0) * qty;
        } else if (col === 'GrossWight' || col === 'AddLesswt') {
            row.NetBillWeight = gross - ebTotal + addLess;
        } else if (col === 'EbUnit') {
            var t = ebUnit * qty; row.EbTotal = t; row.NetBillWeight = gross - t + addLess;
        } else if (col === 'EbTotal') {
            row.EbUnit = qty !== 0 ? ebTotal / qty : 0; row.NetBillWeight = gross - ebTotal + addLess;
        }
        grd.draw();
    }

    // ------------------------------------------------------------------ gate pass
    /** loadonGdnvisibleitem(value): Vehicle Type / Vehicle No / Bilty No / Gate Pass read only. */
    function gdnReadOnly(v) {
        HRM.enable('combvehtyp', !v); HRM.readOnly('txtvehno', v); HRM.readOnly('txtbltyno', v); HRM.readOnly('combgatepass', v);
    }
    function loadGatePass(r) {                                                                  // grdGp_ColumnButtonClick "Load"
        if (!saveMode()) { HRM.box('Please Reset the form First...'); return; }
        if (HRM.str(r.Status) !== 'Accepted') { HRM.box('Status Not Accepted Please check status'); return; }
        if (!HRM.$('CmbStockParty').disabled && grd.rows().length === 0 && HRM.int(HRM.val('combgatepass')) === 0) {
            HRM.setCombo('CmbStockParty', r.StockPartyId);
            HRM.setCombo('CmbRefParty', r.RefPartyId);
        }
        S.gpId = HRM.int(r.Id);
        HRM.setVal('combgatepass', HRM.str(r.GpNo));
        return gatePassRecordFill();
    }
    /** GatePassRecordFill() */
    function gatePassRecordFill() {
        return HRM.loading(get('/gate-pass', { gpNo: HRM.int(HRM.val('combgatepass')), gpId: S.gpId, stockPartyId: HRM.comboVal('CmbStockParty') })).then(function (o) {
            if (!o.found) return;
            var g = o.gp;
            S.gpType = HRM.str(g.GatepassType);
            HRM.setComboText('combvehtyp', HRM.str(g.VehicleType));
            HRM.setVal('txtvehno', HRM.str(g.VehicleNo)); HRM.setVal('txtbltyno', HRM.str(g.BiltyNo));
            HRM.setVal('txtremarks', HRM.str(g.OtherRemarks)); HRM.setVal('txtcarramount', PPB.net(g.Freight));
            HRM.setVal('txtQty', PPB.net(g.ItemQty));
            S.gpQty = n(g.ItemQty);
            HRM.setVal('txtsuppwt', PPB.net(g.SupplierWeight)); HRM.setVal('txtfctwt', PPB.net(g.FactoryWeight));
            HRM.setVal('txtGrossWeight', PPB.net(g.FactoryWeight));
            HRM.setVal('txtwtdiff', PPB.net(g.DifferenceWeight));
            HRM.setCombo('CmbCity', g.CityId);
            HRM.setComboText('CmbItem', HRM.str(g.VarietyName));
            total();
            HRM.focus('CmbStockParty');
            gdnReadOnly(true);
            if (DOC === 89 && S.gpType === 'AdvanceDO') fillFromAdvanceDo(o.ado || []);
        }).catch(HRM.fail);
    }
    /** GridDetailFillFromAdvanceDeliveryOrderData(partyId, gpId) */
    function fillFromAdvanceDo(rows) {
        grd.clear();
        if (!rows.length) { return reset().then(function () { HRM.box('Record not found...'); }); }
        var f = rows[0];
        HRM.fill('CmbStockParty', [{ Id: f.SupplierCustomerId, CompanyName: f.CustomerName }], 'Id', 'CompanyName', { zero: '' });
        HRM.setCombo('CmbStockParty', f.SupplierCustomerId);
        grd.set(rows.map(function (r) {
            var refQty = n(r.RefDocQty), refW = n(r.RefDocWeight), qty = n(r.Qty), net = n(r.NetWeight);
            return {
                Id: 0, RefDocumentTypeId: 0, RefDocIdNo: 0, RefDocSubIdNo: 0, DoDocumentTypeId: r.DocumentTypeId, DeliveryOrderId: r.InvDeliveryOrderId,
                DeliveryOrderDetailId: r.InvDeliveryOrderDetailId, DeliveryOrderNo: r.DocNo, InvoiceDocumentTypeId: r.RefDocumentTypeId,
                InvoiceId: r.RefDocIdNo, InvoiceDetailId: r.RefDocSubIdNo, InvoiceDocType: r.RefDocumentType, InvoiceNo: r.RefDocNo,
                InvoiceManualBillNo: r.RefManualBillNo, WareHouseId: r.WarehouseId, ItemId: r.ItemId, Item: r.ItemName, CropYearId: r.CropYearId,
                JobId: r.JobLotId, PackingTypeId: r.PackingTypeId, PackUomId: r.PackUomId, PackUom: r.PackUom, Equivalent: r.PackEquivalent,
                Qty: r.Qty, GrossWight: r.GrossWight, EbUnit: r.EbUnit, EbTotal: r.EbTotal, AddLesswt: 0, NetBillWeight: net,
                StockWeight: refQty > 0 ? refW / refQty * qty : 0, BalWeight: net, CityId: 0, Comments: '', ContainerNo: '', SealNo: '',
                InvoiceQty: r.RefDocQty, InvoiceWeight: r.RefDocWeight, InvoiceEbUnit: r.RefDocEbUnit, InvoiceEbTotal: r.RefDocEbTotal,
                WareHouseName: r.WareHouseName || r.WarehouseName, CropYear: r.CropYear, JobLot: r.JobLot, PackingType: r.PackingType
            };
        }));
        grdSettings();
    }

    // ------------------------------------------------------------------ Load Data popup (LoadavailableTransactionsForIssuancePartyProcessing)
    P.btnLoadAvailableData = function () {                                                      // btnLoadAvailableData_Click
        var rows = grd.rows();
        if (DOC === 89 && anyGt0(rows, 'DeliveryOrderId')) { openLoader(HRM.comboVal('CmbStockParty'), false); return; }
        if (rows.length) {
            var r = rows[0];
            if (HRM.int(r.RefDocumentTypeId) === 0 && HRM.int(r.RefDocIdNo) === 0 && HRM.int(r.RefDocSubIdNo) === 0) {
                HRM.box('You can not add Record From Loader beacause manual record exist in Grid'); return;
            }
        }
        if (HRM.comboVal('CmbStockParty') === 0) { HRM.focus('CmbStockParty'); HRM.box('Stock Party Account select First'); return; }
        openLoader(HRM.comboVal('CmbStockParty'), true);
    };
    function openLoader(stockPartyId, canLoad) {
        PPB.loader({ get: get, post: post, stockPartyId: stockPartyId, canLoad: canLoad, onLoad: loadFromLoader });
    }
    /** LoadDataDetailfromPurchaseInvoivce() */
    function loadFromLoader(rows) {
        if (!rows.length) return;
        if (grd.rows().length > 0 && HRM.comboVal('CmbStockParty') !== HRM.int(rows[0].StockPartyId)) { HRM.box('Data against another Stock Party Already Exist'); return; }
        rows.forEach(function (x) {
            var dup = grd.rows().some(function (r) {
                return HRM.int(x.RefDocumentTypeId) === HRM.int(r.RefDocumentTypeId) && HRM.int(x.RefDocIdNo) === HRM.int(r.RefDocIdNo) &&
                    HRM.int(x.RefDocSubIdNo) === HRM.int(r.RefDocSubIdNo);
            });
            if (dup) return;
            grd.rows().push({
                Id: 0, RefDocumentTypeId: x.RefDocumentTypeId, RefDocIdNo: x.RefDocIdNo, RefDocSubIdNo: x.RefDocSubIdNo, DoDocumentTypeId: 0,
                DeliveryOrderId: 0, DeliveryOrderDetailId: 0, DeliveryOrderNo: 0, InvoiceDocumentTypeId: 0, InvoiceId: 0, InvoiceDetailId: 0,
                InvoiceDocType: '', InvoiceNo: 0, InvoiceManualBillNo: '', WareHouseId: x.WarehouseId, ItemId: x.ItemId, Item: x.ItemName,
                CropYearId: x.CropYearId, JobId: x.JobLotId, PackingTypeId: x.InvPackingTypeId, PackUomId: x.ItemUomId, PackUom: x.PackUom,
                Equivalent: x.Equivalent, Qty: x.QtyBalance, GrossWight: x.WeightBalance, EbUnit: 0, EbTotal: 0, AddLesswt: 0,
                NetBillWeight: x.WeightBalance, StockWeight: x.WeightBalance, BalWeight: 0, CityId: 0, Comments: '', ContainerNo: '', SealNo: '',
                InvoiceQty: 0, InvoiceWeight: 0, InvoiceEbUnit: 0, InvoiceEbTotal: 0,
                WareHouseName: x.WareHouse, CropYear: x.CropYear, JobLot: x.JobLotCode, PackingType: x.PackingType
            });
        });
        grdSettings();
    }

    // ------------------------------------------------------------------ load / reset / read
    function buttons(update) { HRM.show('btnSave', !update); HRM.show('btnUpdate', update); HRM.show('btnDelete', update); }
    function load() {
        HRM.show('btnGdn339', DOC === 89); HRM.show('printToolStripButton', DOC === 49);
        HRM.show('fldContainer', DOC === 89); HRM.show('fldSeal', DOC === 89);
        HRM.show('fldSuppWt', DOC === 49);
        HRM.show('fldReffered', DOC === 49);
        HRM.show('btnLoadAvailableData', DOC === 89);
        return HRM.loading(get('/setup')).then(function (o) {
            S.rights = o.rights || {};
            S.allowMultiple = !!o.allowMultipleAdo;
            S.defaultDays = HRM.int(o.defaultDays);
            HRM.applyRights(S.rights, { save: 'btnSave', update: 'btnUpdate', print: ['printToolStripButton', 'btnGdn339'], delete: 'btnDelete' });
            fillLists(o, false);
            HRM.setVal('txtdocno', o.docNo > 0 ? o.docNo : '');
            grdGp.set(o.pending || []);
            historyCombos(o.history || {}, false);
            PPB.resetHistoryFilters(S.defaultDays);
            buttons(false);
            gdnReadOnly(false);
            HRM.readOnly('combgatepass', true);                                                 // combgatepass.ReadOnly (GRN and GDN after Load)
            if (DOC === 89) HRM.readOnly('combgatepass', false);
            grdEmptyBags.set([blankBag()]);
            grdSettings();
            HRM.focus('DocDate');
        }).catch(HRM.fail);
    }
    function historyCombos(o, keep) {
        HRM.fill('cmbStockPartyHistory', o.stockParties || [], 'Id', 'name', { zero: '', keep: keep });
        HRM.fill('cmbReferencePartyHistory', o.refParties || [], 'Id', 'name', { zero: '', keep: keep });
    }
    /** reset() */
    function reset() {
        S.removed = []; S.recId = 0; S.gpId = 0; S.gpQty = 0; S.gpType = '';
        HRM.setCombo('CmbTradeToCompany', 0);
        HRM.show('panel5', true);
        HRM.setCombo('CmbStockParty', 0); HRM.setCombo('CmbTransport', 0);
        ['txtcarramount', 'txtsuppwt', 'txtfctwt', 'txtwtdiff', 'combgatepass', 'txtvehno', 'txtbltyno', 'txtremarks', 'txtQty', 'txtebu',
            'txtEmptyBagsTotal', 'txtadlswt', 'txtstockwt', 'txtContainerNo', 'txtSealNo'].forEach(function (id) { HRM.setVal(id, ''); });
        var vt = HRM.$('combvehtyp'); if (vt.options.length) { vt.selectedIndex = 0; HRM.refreshCombos(); }
        HRM.setCombo('CmbItem', 0); HRM.setCombo('CmbCity', 0);
        HRM.$('CmbPackUom').selectedIndex = -1; HRM.refreshCombos();
        grd.clear();
        grdEmptyBags.set([blankBag()]);
        HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false); HRM.show('Add', true);
        buttons(false);
        if (DOC === 89) gdnReadOnly(false);
        HRM.enable('CmbStockParty', true);
        grdSettings();
        HRM.focus('DocDate');
        return Promise.all([
            get('/code').then(function (o) { HRM.setVal('txtdocno', o.docNo > 0 ? o.docNo : ''); }),
            get('/pending').then(function (rows) { grdGp.set(rows || []); }),
            // the stock party combo may hold only the Advance DO party; the desktop keeps it until Refresh - rebound here
            get('/refresh').then(function (o) { fillLists(o, true); HRM.setCombo('CmbStockParty', 0); })
        ]).catch(HRM.fail);
    }
    function detailRows(o) { return (o.details || []).map(function (d) { return Object.assign({}, d); }); }
    /** ReadById(ID) */
    function readById(id) {
        return HRM.loading(get('/by-id', { id: id })).then(function (o) {
            buttons(true);
            S.recId = HRM.int(o.Id); S.removed = [];
            HRM.setVal('txtdocno', HRM.str(o.DocNo));
            HRM.setVal('DocDate', HRM.day(o.DocDate));
            PPB.showTab('form');
            HRM.setCombo('CmbStockParty', o.StockPartyId);
            HRM.setCombo('CmbRefParty', o.SupplierCustomerId);
            HRM.setCombo('CmbTransport', o.TransporterId);
            HRM.setVal('txtcarramount', PPB.net(o.CarriageAmount));
            HRM.setVal('txtsuppwt', PPB.net(o.PartyWeight));
            HRM.setVal('txtfctwt', PPB.net(o.FactoryWeight));
            totalSupplierWeight();
            HRM.setVal('combgatepass', HRM.str(o.GpNo));
            HRM.setComboText('combvehtyp', HRM.str(o.VehicleType));
            HRM.setVal('txtvehno', HRM.str(o.VehicleNo)); HRM.setVal('txtbltyno', HRM.str(o.BiltyNo));
            HRM.setVal('txtremarks', HRM.str(o.RemarksHeader));
            S.gpId = HRM.int(o.GatePassId);
            if (HRM.int(o.TradeToCompanyId) > 0) HRM.setCombo('CmbTradeToCompany', o.TradeToCompanyId);
            grd.set(detailRows(o));
            grdSettings();
            if (anyGt0(grd.rows(), 'DeliveryOrderId')) {
                S.gpQty = grd.sum('Qty'); S.gpType = 'AdvanceDO';
                HRM.fill('CmbStockParty', [{ Id: o.StockPartyId, CompanyName: o.StockPartyName }], 'Id', 'CompanyName', { zero: '' });
                HRM.setCombo('CmbStockParty', o.StockPartyId);
            }
            var bags = (o.emptyBags || []).map(function (b) { return { ItemId: b.ItemId, Qty: b.Qty, Remarks: HRM.str(b.Remarks) }; });
            grdEmptyBags.set(bags.length ? bags : [blankBag()]);
        }).catch(HRM.fail);
    }
    /** DetailGridBind(CurrId) */
    function detailGridBind(id) {
        return get('/by-id', { id: id }).then(function (o) {
            var rows = detailRows(o);
            var hasDo = DOC === 89 && anyGt0(rows, 'DeliveryOrderId');
            ['DeliveryOrderNo', 'InvoiceDocType', 'InvoiceNo', 'InvoiceManualBillNo', 'InvoiceQty', 'InvoiceWeight']
                .forEach(function (k) { GrdHistoryDetail.columnOf(k).hidden = !hasDo; });
            redrawHead(GrdHistoryDetail);
            GrdHistoryDetail.set(rows);
        }).catch(function () { GrdHistoryDetail.clear(); });
    }

    // ------------------------------------------------------------------ save / delete / print
    function insert(btn) {                                                                      // Insert()
        if (HRM.val('txtdocno').trim() === '' || HRM.val('txtdocno').trim() === '0') { HRM.box('DocNo Field is Required'); HRM.focus('txtdocno'); return; }
        if (HRM.comboVal('CmbStockParty') <= 0) { HRM.box('Stock Party Field is Required'); HRM.focus('CmbStockParty'); return; }
        if (HRM.comboVal('CmbRefParty') <= 0) { HRM.box('Reference Party Field is Required'); HRM.focus('CmbRefParty'); return; }
        if (HRM.int(HRM.val('combgatepass').trim()) === 0) { HRM.box('Gatepass Field is Required'); HRM.focus('combgatepass'); return; }
        if (n(HRM.val('txtcarramount')) > 0 && HRM.comboVal('CmbTransport') === 0) { HRM.box('Transporter Field is Required'); HRM.focus('CmbTransport'); return; }
        if (HRM.comboVal('CmbTransport') > 0 && n(HRM.val('txtcarramount')) === 0) { HRM.box('Freight Amount Field is Required'); HRM.focus('txtcarramount'); return; }
        if (n(HRM.val('txtfctwt')) === 0) { HRM.box('Factory Weight Field is Required'); HRM.focus('txtfctwt'); return; }
        if (!grd.rows().length) { HRM.box('Grid Record Not Found'); return; }
        if (!HRM.ask(S.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        var preview = HRM.checked('ChkBox');
        var body = {
            id: S.recId, docNo: HRM.val('txtdocno'), docDate: HRM.val('DocDate'), stockPartyId: HRM.comboVal('CmbStockParty'),
            gpNo: HRM.val('combgatepass'), refPartyId: HRM.comboVal('CmbRefParty'), partyWeight: HRM.val('txtsuppwt'),
            factoryWeight: HRM.val('txtfctwt'), vehicleType: HRM.comboText('combvehtyp'), vehicleNo: HRM.val('txtvehno'),
            biltyNo: HRM.val('txtbltyno'), transporterId: HRM.comboVal('CmbTransport'), carriageAmount: HRM.val('txtcarramount'),
            remarks: HRM.val('txtremarks'), gatePassId: S.gpId, tradeToCompanyId: HRM.comboVal('CmbTradeToCompany'),
            rows: grd.rows(), removed: S.removed, emptyBags: grdEmptyBags.rows()
        };
        return HRM.busy(btn, function () {
            return post('/save', body).then(function (res) {
                HRM.box(res.message);
                var win = preview && DOC === 49 ? CrystalPrint.reserve() : null;
                return reset().then(function () { if (preview) printSlip(res.id, DOC, null, win); });
            }).catch(HRM.fail);
        });
    }
    /** GrnSlipPartyProcessing333(Id) / GdnSlipPartyProcessing339(Id) */
    function printSlip(id, docType, btn, win) {
        var w = win || (docType === 49 ? CrystalPrint.reserve() : null);
        return HRM.busy(btn, function () {
            return get('/slip', { id: id }).then(function (rows) {
                if (docType === 49) return CrystalPrint.open('ppb-333', { id: id }, null, w);
                PPB.printRows('339 Goods Dispatch Notes Party Processing Slip', rows);
            }).catch(function (e) { CrystalPrint.release(w); HRM.fail(e); });
        }, 'gg-print');
    }
    P.btnSave = function (btn) { S.recId = 0; return insert(btn || 'btnSave'); };
    P.btnUpdate = function (btn) { if (S.recId === 0) { HRM.box('Record Id not found'); return; } return insert(btn || 'btnUpdate'); };
    P.btnNew = function () { return reset(); };
    P.btnRefresh = function (btn) {                                                             // toolStripButton1_Click
        return HRM.busy(btn || 'toolStripButton1', function () { return get('/refresh').then(function (o) { fillLists(o, true); }).catch(HRM.fail); });
    };
    P.btnDelete = function (btn) {                                                              // btnDelete_Click
        if (S.recId <= 0) { HRM.box('No record found to Delete'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        return HRM.busy(btn || 'btnDelete', function () {
            return post('/delete', {}, { id: S.recId }).then(function (res) { HRM.box(res.message); return reset(); }).catch(HRM.fail);
        });
    };
    P.print333 = function (btn) { return printSlip(S.recId, 49, btn || 'printToolStripButton'); };
    P.print339 = function (btn) { return printSlip(S.recId, 89, btn || 'btnGdn339'); };
    P.shortcuts = function () {                                                                 // MakeShortCutKeys()
        HRM.box('Ctrl+E  For Close\nCtrl+N  For New\nCtrl+R  For Refresh\nCtrl+S  For Save in form tab and to show history in history tab\n' +
            'Ctrl+U  For Update\nCtrl+P  For Print\nCtrl+F5  For Focus on Doc Date\nCtrl+F10  For Open Attachments\nCtrl+T  For Tab Transfer\n' +
            'Ctrl+alt  To Show ShortCut Keys Form\nAlt+C  To Show Define City Form\nAlt+R  To Show Define Reference Party Form\n' +
            'Ctrl+ArrowDown  For Focus On Detail Grid\nCtrl+ArrowUp  For Focus On Warehouse when in form tab and for focus on FromDate when in history tab\n' +
            'Ctrl+Right  when Template 2 toggle between Form tab grids\nCtrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link');
    };

    // ------------------------------------------------------------------ history
    P.btnshowHistory = function (btn) {                                                         // HistoryGridFill()
        var ref = document.querySelector('input[name="histRef"]:checked');
        return HRM.busy(btn || 'btnshowHistory', function () {
            return post('/history', PPB.historyFilters({
                stockPartyId: HRM.comboVal('cmbStockPartyHistory'), refPartyId: HRM.comboVal('cmbReferencePartyHistory'),
                actionId: ref ? HRM.int(ref.value) : 0
            })).then(function (rows) { GrdHistory.set(rows || []); GrdHistoryDetail.clear(); }).catch(HRM.fail);
        });
    };
    P.btnNewHistory = function () {                                                             // btnNewHistory_Click
        PPB.resetHistoryFilters(S.defaultDays, ['cmbStockPartyHistory', 'cmbReferencePartyHistory']);
        GrdHistory.clear(); GrdHistoryDetail.clear();
    };
    P.btnRefreshHistory = function (btn) {                                                      // HistoryCombosFill()
        return HRM.busy(btn || 'btnRefreshHistory', function () {
            return get('/history-combos').then(function (o) { historyCombos(o, true); }).catch(HRM.fail);
        });
    };

    // ------------------------------------------------------------------ template I / II (rdTemplate1_CheckedChanged / ChangeTemplate)
    function changeTemplate() {
        var t2 = HRM.$('rdTemplate2').checked;
        var box = HRM.$('ppbGrids');
        box.classList.toggle('ppb-t1', !t2); box.classList.toggle('ppb-t2', t2);
        showSub(t2 ? currentSub() : 'detail');
    }
    function currentSub() { var a = document.querySelector('.ppb-subtabs .hrm-tab.is-active'); return a ? a.getAttribute('data-sub') : 'detail'; }
    function showSub(name) {
        document.querySelectorAll('.ppb-subtabs .hrm-tab').forEach(function (b) { b.classList.toggle('is-active', b.getAttribute('data-sub') === name); });
        document.querySelectorAll('#ppbGrids .ppb-pane').forEach(function (p) { p.classList.toggle('is-active', p.getAttribute('data-pane') === name); });
    }
    document.querySelectorAll('.ppb-subtabs .hrm-tab').forEach(function (b) { b.addEventListener('click', function () { showSub(b.getAttribute('data-sub')); }); });
    HRM.$('rdTemplate1').addEventListener('change', changeTemplate);
    HRM.$('rdTemplate2').addEventListener('change', changeTemplate);

    // ------------------------------------------------------------------ wiring
    PPB.tabs(function (name) { HRM.focus(name === 'history' ? 'FromDateHistory' : 'DocDate'); });
    HRM.footer(function () { PPB.showTab('history'); });
    HRM.$('CmbItem').addEventListener('change', function () { packUomFill(); });                 // CmbItem.ValueChanged / Leave -> combitem_Leave
    PPB.leave('CmbPackUom', function () { if (saveMode()) totalWeight(); total(); });            // combpckuom_Leave
    HRM.$('txtQty').addEventListener('input', function () { if (saveMode()) totalWeight(); total(); });
    HRM.$('txtebu').addEventListener('input', function () { total('EbUnit'); });
    HRM.$('txtEmptyBagsTotal').addEventListener('input', function () { total('EbTotal'); });
    HRM.$('txtGrossWeight').addEventListener('input', function () { total(); });
    HRM.$('txtadlswt').addEventListener('input', function () { total(); });
    HRM.$('txtadlswt').addEventListener('blur', function () { total(); });
    PPB.guard('txtQty', 'dec'); PPB.guard('txtebu', 'dec'); PPB.guard('txtGrossWeight', 'dec'); PPB.guard('txtEmptyBagsTotal', 'dec');
    PPB.guard('txtadlswt', 'signed'); PPB.guard('txtcarramount', 'int'); PPB.guard('combgatepass', 'int');
    PPB.guard('txtFromDocNoHistory', 'int'); PPB.guard('txtToDocNoHistory', 'int');
    HRM.setVal('DocDate', HRM.today());
    PPB.resetHistoryFilters(3);
    function focusGrid(id) { var t = HRM.$(id); var tr = t && t.querySelector('tbody tr'); if (tr) tr.focus(); }
    HRM.keys({
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+t': function () { PPB.toggleTab(); },
        'ctrl+alt+alt': P.shortcuts, 'ctrl+alt+control': P.shortcuts,
        'ctrl+n': function () { if (PPB.currentTab() === 'history') P.btnNewHistory(); else P.btnNew(); },
        'ctrl+r': function () { if (PPB.currentTab() === 'history') P.btnRefreshHistory(); else P.btnRefresh(); },
        'ctrl+s': function () { if (PPB.currentTab() === 'history') P.btnshowHistory(); else if (saveMode()) P.btnSave(); },
        'ctrl+u': function () { if (HRM.visible('btnUpdate') && !HRM.$('btnUpdate').disabled) P.btnUpdate(); },
        'ctrl+p': function () { if (S.rights.print === false) return; if (DOC === 49) P.print333(); else P.print339(); },
        'ctrl+f5': function () { HRM.focus('DocDate'); },
        'ctrl+arrowdown': function () { if (PPB.currentTab() === 'history') focusGrid('GrdHistory'); else focusGrid('grd'); },
        'ctrl+arrowup': function () { if (PPB.currentTab() === 'history') HRM.focus('FromDateHistory'); else HRM.focus('CmbWareHouse'); },
        'ctrl+arrowright': function () {
            if (PPB.currentTab() !== 'form' || !HRM.$('rdTemplate2').checked) return;
            var order = ['detail', 'eb', 'gp']; showSub(order[(order.indexOf(currentSub()) + 1) % 3]);
        }
    });
    changeTemplate();
    load();
})();
