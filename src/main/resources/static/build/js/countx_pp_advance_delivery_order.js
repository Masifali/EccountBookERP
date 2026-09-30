/* ============================================================================================
 * countx_pp_advance_delivery_order.js - Party Processing Advance Delivery Order (AdvanceDeliveryOrderPP.cs), ScreenId 875, DocumentTypeId 910.
 * Follows the form: Load (configs, rights, caches, defaults), the detail box calculations and stock balance, the detail grid
 * (grd_CellUpdated, X / Ctrl+D when partial invoicing is allowed), Load Sale Invoice (LoadSaleInvoiceForADO), Insert() / SaveAs,
 * Reset(), ReadById, history (Edit, Print, PrintII, Print 910A, SaveAs) + detail of the selected row, keys. Exports window.PpAdo.
 * ============================================================================================ */
(function () {
    'use strict';
    var API = '/api/party-processing/advance-delivery-order';
    var P = {}; window.PpAdo = P;
    var S = { rights: {}, recId: 0, isApproved: false, removed: [], uoms: [], lists: {}, allowMultiple: false, tolerance: 0, warnPacking: false, defaults: {} };

    function get(path, params) { return HRM.get(API + path, params || {}); }
    function post(path, body, params) { return HRM.post(API + path + (params ? '?' + new URLSearchParams(params).toString() : ''), body); }
    var n = HRM.num;
    function dt12(v) {
        var d = HRM.day(v); if (!d) return '';
        var p = d.split('-'), t = HRM.time(v) || '00:00', h = +t.substring(0, 2);
        return p[2] + '-' + p[1] + '-' + p[0] + ' ' + String(h % 12 === 0 ? 12 : h % 12).padStart(2, '0') + ':' + t.substring(3, 5) + ' ' + (h < 12 ? 'AM' : 'PM');
    }
    function dmy(v) { var d = HRM.day(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + p[1] + '-' + p[0]; }
    function nameCol(key, caption, listKey, text, nameKey) {                                   // GridEX_Helper.ComboBind, EditType NoTextBox
        return { key: key, caption: caption, render: function (v, r) {
            var x = PPB.rowOf(S.lists[listKey], 'Id', v);
            return HRM.esc(x ? x[text] : HRM.str(r[nameKey]));
        } };
    }
    function editable(k) { return k === 'Remarks' || k === 'OtherRemarks' || (S.allowMultiple ? (k === 'QTY' || k === 'Weight' || k === 'PackingUnit') : k === 'StockWeight'); }

    // ------------------------------------------------------------------ grids
    var grd;
    function buildGrid() {                                                                      // grdSettings()
        var numE = function (k, cap, hidden) { return { key: k, caption: cap, type: editable(k) ? 'edit-num' : 'num', decimals: 3, sum: true, hidden: !!hidden }; };
        var txtE = function (k, cap) { return { key: k, caption: cap, type: editable(k) ? 'edit' : 'text' }; };
        HRM.$('grd').replaceWith(HRM.$('grd').cloneNode(false));
        grd = new HRM.Grid('grd', {
            columns: [
                { key: 'Delete', caption: 'X', width: 20, hidden: !S.allowMultiple, render: function () { return PPB.btnCell('delete', 'X'); } },
                { key: 'Id', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'RefDocIdNo', hidden: true }, { key: 'RefDocSubIdNo', hidden: true },
                { key: 'RefDocType', caption: 'RefDocType', width: 70 }, { key: 'InvoiceNo', caption: 'InvoiceNo', type: 'int' },
                { key: 'InvoiceManualBillNo', caption: 'InvoiceManualBillNo' },
                nameCol('WareHouseId', 'WareHouse Name', 'warehouses', 'WareHouseName', 'WareHouseName'),
                { key: 'ItemId', hidden: true }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'Item', caption: 'Item', width: 250 },
                nameCol('CropYearId', 'Crop Year', 'cropYears', 'CropYear', 'CropYear'),
                nameCol('JobLotId', 'Job Lot', 'jobLots', 'JobLotDescription', 'JobLot'),
                nameCol('PackingTypeId', 'Packing Type', 'packingTypes', 'PackTypeDesc', 'PackingType'),
                { key: 'ItemUOMId', hidden: true }, { key: 'ItemUOM', caption: 'ItemUOM' }, { key: 'PUomEquivalent', hidden: true },
                numE('QTY', 'QTY'), numE('Weight', 'Weight'), numE('PackingUnit', 'PackingUnit', true), numE('PackingWeight', 'PackingWeight', true),
                numE('GrossWeight', 'GrossWeight', true), txtE('Remarks', 'Remarks'), numE('AvailableStock', 'AvailableStock'),
                txtE('OtherRemarks', 'OtherRemarks'), numE('StockWeight', 'StockWeight'), numE('InvoiceQty', 'InvoiceQty'), numE('InvoiceWeight', 'InvoiceWeight')
            ],
            totals: true, emptyText: '',
            onChange: function (r, k) { cellUpdated(r, k); }
        });
        PPB.onButton(grd, 'grd', function (r, act, i) { if (act === 'delete') deleteDetailRow(i); });   // grd_ColumnButtonClick
    }
    /** grd_CellUpdated */
    function cellUpdated(row, col) {
        var qty = n(row.QTY), weight = n(row.Weight), puom = n(row.PUomEquivalent), unit = n(row.PackingUnit), pw = n(row.PackingWeight);
        var iq = n(row.InvoiceQty), iw = n(row.InvoiceWeight);
        function sw() { if (S.allowMultiple) row.StockWeight = iw > 0 && iq > 0 ? iw / iq * qty : 0; }
        if (col === 'QTY' || col === 'PackingUnit') {
            var w = qty * puom; row.Weight = w; var p = unit * qty; row.PackingWeight = p; row.GrossWeight = w + p; sw();
        } else if (col === 'Weight') {
            var p2 = unit * qty; row.PackingWeight = p2; row.GrossWeight = n(row.Weight) + p2; sw();
        } else if (col === 'PackingWeight') {
            row.PackingUnit = qty > 0 ? pw / qty : 0; row.GrossWeight = pw + weight; sw();
        }
        if (col === 'StockWeight') {
            var s = n(row.StockWeight), g = n(row.GrossWeight), up = g + S.tolerance, lo = g - S.tolerance;
            if (s > up) { HRM.box('Stock Weight should be less than or equal to Gross Weight + ' + PPB.net(S.tolerance)); row.StockWeight = up; }
            else if (s < lo) { HRM.box('Stock Weight should be greater than or equal to Gross Weight - ' + PPB.net(S.tolerance)); row.StockWeight = lo; }
        }
        grd.draw();
    }
    /** DeleteDetailRow(r): a saved row goes to lstRemoveRecord (ActionTypeId 3) after the confirm; a new row is removed at once. */
    function deleteDetailRow(i) {
        var r = grd.rows()[i]; if (!r) return;
        if (HRM.int(r.Id) > 0) {
            if (!HRM.ask('Are you sure to Delete?')) return;
            S.removed.push(Object.assign({}, r));
        }
        grd.remove(i);
    }
    /** AddDetailRow(r) (Ctrl+D): a copy of the row; its Id is 0 while updating. */
    function addDetailRow(i) {
        var r = grd.rows()[i]; if (!r) return;
        var c = Object.assign({}, r);
        if (HRM.visible('btnupdate') && !HRM.$('btnupdate').disabled) c.Id = 0;
        grd.add(c);
    }

    var grdhistory = new HRM.Grid('grdhistory', {                                              // HistoryGridSettings()
        columns: [
            { key: 'Edit', caption: 'Edit', width: 40, render: function () { return PPB.btnCell('edit', 'Edit'); } },
            { key: 'Print', caption: 'Print', width: 40, render: function () { return PPB.btnCell('print', 'Print'); } },
            { key: 'PrintII', caption: 'PrintII', width: 55, render: function () { return PPB.btnCell('print2', 'PrintII'); } },
            { key: 'Print910A', caption: 'Print 910A', width: 70, render: function () { return PPB.btnCell('print910a', 'Print 910A'); } },
            { key: 'SaveAs', caption: 'SaveAs', width: 65, render: function () { return PPB.btnCell('saveas', 'SaveAs'); } },
            { key: 'Id', hidden: true }, { key: 'DoType', caption: 'DoType' }, { key: 'DocDate', caption: 'DocDate', type: 'date' },
            { key: 'DocNo', caption: 'DocNo', type: 'int' }, { key: 'ExpiryDate', caption: 'ExpiryDate', render: function (v) { return HRM.esc(dmy(v)); } },
            { key: 'VehicleType', caption: 'VehicleType' }, { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'SupplierCustomerId', hidden: true },
            { key: 'CustomerName', caption: 'CustomerName' }, { key: 'RefPartyId', hidden: true }, { key: 'ReferencePartyName', caption: 'ReferencePartyName' },
            { key: 'ApprovalStatus', caption: 'ApprovalStatus' }, { key: 'EntryUser', caption: 'EntryUser' },
            { key: 'EntryDate', caption: 'EntryDate', render: function (v) { return HRM.esc(dt12(v)); } }, { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'ModifyDate', caption: 'ModifyDate', render: function (v) { return HRM.esc(dt12(v)); } }, { key: 'ApprovedUser', caption: 'ApprovedUser' },
            { key: 'ApprovedDate', caption: 'ApprovedDate', render: function (v) { return HRM.esc(dt12(v)); } },
            { key: 'OrderStatus', caption: 'OrderStatus' }, { key: 'Remarks', caption: 'Remarks' }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', type: 'int' }
        ],
        filterRow: true, emptyText: '',
        onDouble: function (r) { if (openStatus(r)) readById(HRM.int(r.Id)); },                // grdhistory_DoubleClick
        onSelect: function (r) { detailGridBind(HRM.int(r.Id)); }
    });
    function openStatus(r) {
        var st = HRM.str(r.OrderStatus);
        if (st !== 'Open') { HRM.box("Sorry You can't Update this record because Order_Status is:" + st); return false; }
        return true;
    }
    PPB.onButton(grdhistory, 'grdhistory', function (r, act) {                                 // grdhistory_ColumnButtonClick
        var id = HRM.int(r.Id);
        if (act === 'saveas') saveAsFrom(id);
        else if (act === 'edit') { if (openStatus(r)) readById(id); }
        else if (act === 'print') printSlip('910', id, null);
        else if (act === 'print2') printSlip('910_01', id, null);
        else if (act === 'print910a') printSlip('910A', id, null);
    });
    var GrdHistoryDetail = new HRM.Grid('GrdHistoryDetail', {                                  // grdHistoryDetailSettings()
        columns: [
            { key: 'RefDocType', caption: 'RefDocType', width: 70 }, { key: 'InvoiceNo', caption: 'InvoiceNo', type: 'int' },
            { key: 'InvoiceManualBillNo', caption: 'InvoiceManualBillNo' }, { key: 'WareHouseName', caption: 'WareHouseName' },
            { key: 'ItemCode', caption: 'ItemCode' }, { key: 'Item', caption: 'Item' }, { key: 'CropYear', caption: 'CropYear' },
            { key: 'JobLot', caption: 'JobLot' }, { key: 'PackingType', caption: 'PackingType' }, { key: 'ItemUOM', caption: 'ItemUOM' },
            PPB.numCol('QTY', 'QTY'), PPB.numCol('Weight', 'Weight'), { key: 'Remarks', caption: 'Remarks' }, PPB.numCol('AvailableStock', 'AvailableStock'),
            { key: 'OtherRemarks', caption: 'OtherRemarks' }, PPB.numCol('StockWeight', 'StockWeight')
        ],
        totals: true, emptyText: ''
    });

    // ------------------------------------------------------------------ lists
    function fillLists(o) {
        Object.keys(o).forEach(function (k) { S.lists[k] = o[k]; });
        HRM.fill('CmbSupplierCustomer', o.customers || [], 'Id', 'CompanyName', { zero: '', keep: true });
        HRM.fill('ultraCombo1', o.refParties || [], 'Id', 'ReferencePartyName', { zero: '', keep: true });
        HRM.fill('CmbVehicleType', o.vehicleTypes || [], 'Id', 'VehicleDescription', { zero: '', keep: true });
        HRM.fill('CmbWareHouse', o.warehouses || [], 'Id', 'WareHouseName', { zero: '...Select Any Value...', keep: true });
        itemBind();
        HRM.fill('CmbCropYear', o.cropYears || [], 'Id', 'CropYear', { zero: '...Select Any Value...', keep: true });
        HRM.fill('CmbJobLot', o.jobLots || [], 'Id', 'JobLotDescription', { zero: '...Select Any Value...', keep: true });
        HRM.fill('CmbPackingType', o.packingTypes || [], 'Id', 'PackTypeDesc', { zero: '...Select Any Value...', keep: true });
        if (o.defaults) S.defaults = o.defaults;
        if (grd) grd.draw();
    }
    /** GetConfigurationsFromGlobalAndBindValuesInColumns() */
    function applyDefaults() {
        var d = S.defaults || {};
        if (HRM.int(d.jobLotId)) HRM.setCombo('CmbJobLot', d.jobLotId);
        if (HRM.int(d.cropYearId)) HRM.setCombo('CmbCropYear', d.cropYearId);
        if (HRM.int(d.packingTypeId)) HRM.setCombo('CmbPackingType', d.packingTypeId);
        if (HRM.int(d.warehouseId)) HRM.setCombo('CmbWareHouse', d.warehouseId);
    }
    /** ItemNameBind(): name or code; then the pack UOMs of the item. */
    function itemBind() {
        var byName = HRM.$('rdSearchByName').checked;
        HRM.fill('CmbItemName', S.lists.items || [], 'Id', byName ? 'ItemName' : 'ItemCode', { zero: '...Select Any Value...', keep: true });
        return packUomBind();
    }
    function packUomBind() {                                                                    // PackUomFromGlobalBind(ItemId)
        var prev = HRM.comboText('CmbPackUom');
        return get('/uoms', { itemId: HRM.comboVal('CmbItemName') }).then(function (rows) {
            S.uoms = rows || [];
            HRM.fill('CmbPackUom', S.uoms, 'Id', 'UOMCode', { zero: '...Select Any Value...' });
            if (prev && S.uoms.some(function (u) { return u.UOMCode === prev; })) HRM.setComboText('CmbPackUom', prev);
            else if (S.uoms.length) HRM.setCombo('CmbPackUom', S.uoms[0].Id);
        }).catch(HRM.fail);
    }
    function uomEq() { var r = PPB.rowOf(S.uoms, 'Id', HRM.val('CmbPackUom')); return r ? n(r.Equivalent) : 0; }

    // ------------------------------------------------------------------ detail box
    function calculateWeight() {                                                                // CalculateWeight()
        var w = n(HRM.val('txtqty')) * (HRM.comboVal('CmbPackUom') ? uomEq() : 0);
        HRM.setVal('txtweight', PPB.fmt(w, '#,##0.###')); HRM.setVal('txtGrossWeight', PPB.fmt(w, '#,##0.###'));
        calculateGrossWeight();
    }
    function calculateGrossWeight(active) {                                                     // CalculateGrossWeight()
        var weight = n(HRM.val('txtweight'));
        if (weight <= 0) { HRM.setVal('txtGrossWeight', '0'); return; }
        var qty = n(HRM.val('txtqty')), unit = n(HRM.val('txtPackUnit')), pw = n(HRM.val('txtPackWeight'));
        if (active === 'PackWeight') { unit = qty > 0 ? pw / qty : 0; HRM.setVal('txtPackUnit', PPB.fmt(unit, '#,##0.#####')); }
        else { pw = unit * qty; HRM.setVal('txtPackWeight', PPB.fmt(pw, '#,##0.###')); }
        HRM.setVal('txtGrossWeight', PPB.fmt(weight + pw, '#,##0.###'));
    }
    function stockLabel() {                                                                     // UpdateAvailableStockLabel()
        return post('/stock', {
            docDate: HRM.val('DocDate'), warehouseId: HRM.comboVal('CmbWareHouse'), itemId: HRM.comboVal('CmbItemName'),
            cropYear: HRM.comboVal('CmbCropYear') ? HRM.comboText('CmbCropYear').trim() : '', jobLotId: HRM.comboVal('CmbJobLot'),
            packingTypeId: HRM.comboVal('CmbPackingType'), uomId: HRM.comboVal('CmbPackUom')
        }).then(function (o) { var v = n(o.stock); HRM.text('lblBalance', v > 0 ? PPB.net(v) : '0'); }).catch(HRM.fail);
    }
    P.btnplus = function () { };                                                                // btnplus_Click has an empty body on the desktop
    P.resetDetail = function () {                                                               // ResetDetail()
        HRM.setCombo('CmbItemName', 0); S.uoms = []; HRM.fill('CmbPackUom', [], 'Id', 'UOMCode', { zero: '' });
        HRM.setVal('txtqty', ''); HRM.setVal('txtweight', ''); HRM.setVal('txtPackUnit', '0'); HRM.setVal('txtPackWeight', '0'); HRM.setVal('txtGrossWeight', '0');
        HRM.setCombo('CmbPackingType', 0);
        HRM.show('btnplus', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        HRM.focus('CmbWareHouse');
    };

    // ------------------------------------------------------------------ row stock
    function rowStocks() {                                                                      // DocDate_ValueChanged
        var rows = grd.rows(); if (!rows.length) return Promise.resolve();
        return post('/row-stock', { docDate: HRM.val('DocDate'), rows: rows.map(function (r) {
            var cy = PPB.rowOf(S.lists.cropYears, 'Id', r.CropYearId);
            return { ItemId: r.ItemId, WareHouseId: r.WareHouseId, JobLotId: r.JobLotId, CropYear: cy ? cy.CropYear : HRM.str(r.CropYear),
                PackingTypeId: r.PackingTypeId, ItemUOMId: r.ItemUOMId };
        }) }).then(function (vals) { (vals || []).forEach(function (v, i) { if (rows[i]) rows[i].AvailableStock = v; }); grd.draw(); }).catch(HRM.fail);
    }

    // ------------------------------------------------------------------ Load Sale Invoice (LoadSaleInvoiceForADO)
    P.loadSaleInvoice = function () {                                                           // BtnLoadSaleInvoice_Click
        var rows = grd.rows();
        if (rows.length && !rows.some(function (r) { return HRM.int(r.RefDocIdNo) > 0; })) {
            HRM.box("You cannot load 'SaleInvoice' because record without 'SaleInvoice' exists in the grid."); return;
        }
        var m = HRM.modal({ title: 'Load Sale Invoice For Advance DO', width: '1100px',
            html: '<div class="win-tool-strip"><button type="button" id="siNew" class="win-btn-tool"><i class="fa fa-plus"></i> <span>New</span></button>' +
                '<button type="button" id="siRefresh" class="win-btn-tool"><i class="fa fa-refresh"></i> <span>Refresh</span></button></div>' +
                '<fieldset class="hrm-group"><legend>Filters</legend><div class="hrm-entry" style="grid-template-columns:repeat(auto-fill,minmax(180px,1fr))">' +
                '<div class="hrm-field hrm-stack"><label for="siFrom">From Date</label><input type="date" id="siFrom" class="win-textbox"/></div>' +
                '<div class="hrm-field hrm-stack"><label for="siTo">To Date</label><input type="date" id="siTo" class="win-textbox"/></div>' +
                '<div class="hrm-field hrm-stack"><label for="siCustomer">Customer</label><select id="siCustomer" class="win-combo dtcombo" data-dtcombo="single"></select></div>' +
                '<div class="hrm-field hrm-stack"><label>&nbsp;</label><button type="button" id="siShow" class="win-btn-action">Show</button></div>' +
                '<div class="hrm-field hrm-stack"><label>&nbsp;</label><button type="button" id="siLoad" class="win-btn-action">Load</button></div>' +
                '</div></fieldset><div class="hrm-grid-box"><div class="hrm-grid-wrap" style="max-height:360px"><table id="siGrid"></table></div></div>'
        });
        var all = [], multi = false, g = null;
        function build() {
            var vis = !multi;
            g = new HRM.Grid('siGrid', {
                columns: [
                    { key: 'Select', caption: 'Select', type: 'edit-check', width: 30 },
                    { key: 'Id', hidden: true }, { key: 'DetailId', hidden: true }, { key: 'DocumentTypeId', hidden: true },
                    { key: 'DocType', caption: 'DocType' }, { key: 'DocDate', caption: 'DocDate', type: 'date' }, { key: 'DocNo', caption: 'DocNo', type: 'int' },
                    { key: 'ManualBillNo', caption: 'ManualBillNo' }, { key: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'CustomerName' },
                    PPB.numCol('BillAmount', 'BillAmount', '#,##0.##'), { key: 'WareHouseName', caption: 'WareHouseName' }, { key: 'ItemId', hidden: true },
                    { key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName' }, { key: 'CropYear', caption: 'CropYear' },
                    { key: 'JobLot', caption: 'JobLot' }, { key: 'PackingType', caption: 'PackingType' }, { key: 'PackUom', caption: 'PackUom' },
                    { key: 'InvoiceQty', caption: 'InvoiceQty', align: 'right' }, { key: 'GdnQty', caption: 'GdnQty', align: 'right', hidden: vis },
                    { key: 'OutstandingDoQty', caption: 'OutstandingDoQty', align: 'right', hidden: vis },
                    { key: 'AvailableQtyForDo', caption: 'AvailableQtyForDo', align: 'right', hidden: vis },
                    { key: 'InvoiceWeight', caption: 'InvoiceWeight', align: 'right' }, { key: 'GdnWeight', caption: 'GdnWeight', align: 'right', hidden: vis },
                    { key: 'OutstandingDoWeight', caption: 'OutstandingDoWeight', align: 'right', hidden: vis },
                    { key: 'AvailableWeightForDo', caption: 'AvailableWeightForDo', align: 'right', hidden: vis },
                    PPB.numCol('NetStockWeight', 'NetStockWeight'), PPB.numCol('ItemRate', 'ItemRate', '#,##0.####', false), { key: 'RateUom', caption: 'RateUom' },
                    PPB.numCol('ItemAmount', 'ItemAmount', '#,##0.##'), { key: 'RemarksHeader', caption: 'RemarksHeader' }
                ],
                checkAll: 'Select', filterRow: true, totals: true, emptyText: ''
            });
        }
        function show(btn) {                                                                    // PendingDataDbCall + PendingDataGridBind
            return HRM.busy(btn, function () {
                return post('/invoices', { fromDate: HRM.val('siFrom'), toDate: HRM.val('siTo'), customerId: HRM.comboVal('siCustomer') }).then(function (rows) {
                    all = rows || [];
                    g.set(all.map(function (r) {
                        return { Select: false, Id: r.Id, DetailId: r.DetailId, DocumentTypeId: r.DocumentTypeId, DocType: r.DocumentType, DocDate: r.DocDate,
                            DocNo: r.DocNo, ManualBillNo: r.ManualBillNo, SupplierCustomerId: r.SupplierCustomerId, CustomerName: r.CustomerName,
                            BillAmount: r.BillAmount, WareHouseName: r.WareHouseName, ItemId: r.ItemId, ItemCode: r.ItemCode, ItemName: r.ItemName,
                            CropYear: r.CropYear, JobLot: r.JobLotDescription, PackingType: r.PackTypeDesc, PackUom: r.PackUom, InvoiceQty: r.InvoiceQty,
                            GdnQty: r.GdnQty, OutstandingDoQty: r.OutStandingDoQty, AvailableQtyForDo: r.AvailableForDoDoQty, InvoiceWeight: r.InvoiceWeight,
                            GdnWeight: r.GdnWeight, OutstandingDoWeight: r.OutStandingDoWeight, AvailableWeightForDo: r.AvailableForDoDoWeight,
                            NetStockWeight: r.NetStockWeight, ItemRate: r.ItemRate, RateUom: r.RateUom, ItemAmount: r.ItemAmount, RemarksHeader: r.RemarksHeader };
                    }));
                }).catch(HRM.fail);
            }, 'si-show');
        }
        HRM.setVal('siTo', HRM.today());
        HRM.loading(get('/invoice-setup')).then(function (o) {
            multi = !!o.allowMultipleAdo; build();
            HRM.fill('siCustomer', o.customers || [], 'Id', 'ReferenceName', { zero: '' });
            HRM.setVal('siFrom', HRM.day(o.fromDate) || HRM.today());
            return show(null);
        }).catch(HRM.fail);
        HRM.$('siShow').addEventListener('click', function () { show('siShow'); });
        HRM.$('siNew').addEventListener('click', function () { HRM.setVal('siTo', HRM.today()); if (g) g.clear(); });
        HRM.$('siRefresh').addEventListener('click', function () {
            HRM.busy('siRefresh', function () { return get('/invoice-setup').then(function (o) { HRM.fill('siCustomer', o.customers || [], 'Id', 'ReferenceName', { zero: '', keep: true }); }).catch(HRM.fail); });
        });
        m.el.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var k = (e.key || '').toLowerCase();
            if (k === 's') { e.preventDefault(); HRM.$('siShow').click(); } else if (k === 'l') { e.preventDefault(); HRM.$('siLoad').click(); }
        });
        HRM.$('siLoad').addEventListener('click', function () {                                // BtnLoad
            var checked = g ? g.checked('Select') : [];
            if (!checked.length) { HRM.box('Check the row first'); return; }
            var first = 0, ids = [];
            for (var i = 0; i < checked.length; i++) {
                var r = checked[i], cmp = multi ? HRM.int(r.SupplierCustomerId) : HRM.int(r.Id);
                if (cmp !== 0) {
                    if (first === 0) first = cmp;
                    else if (first !== cmp) { HRM.box(multi ? 'Sorry! You can only select rows of the same customer.' : 'Sorry! You can only select rows of the same invoice.'); return; }
                    ids.push(multi ? HRM.int(r.DetailId) : HRM.int(r.Id));
                }
            }
            var picked = ids.length ? all.filter(function (r) { return ids.indexOf(HRM.int(multi ? r.DetailId : r.Id)) >= 0; }) : [];
            m.close();
            loadInGrid(picked);
        });
    };
    /** LoadInGridDetailFromGrn(dt) */
    function loadInGrid(dt) {
        if (!dt.length) return;
        var rows = grd.rows();
        if (rows.length) {
            var sameInvoice = rows.some(function (r) { return HRM.int(r.RefDocIdNo) === HRM.int(dt[0].Id); });
            if (!sameInvoice && !S.allowMultiple) {
                if (!HRM.ask("Already Loaded Row's Have Different Invoice. So you Can't Load Rows Of Different Invoice!.If you proceed, the existing data will be reset.Are you sure you want to continue?")) return;
                grd.clear();
            } else if (HRM.comboVal('CmbSupplierCustomer') !== HRM.int(dt[0].SupplierCustomerId)) {
                if (!HRM.ask("Already Loaded Row's Have Different Customer. So you Can't Load Rows Of Different Customer!. If you proceed, the existing data will be reset. Are you sure you want to continue?")) return;
                grd.clear();
            }
        }
        HRM.setCombo('CmbSupplierCustomer', dt[0].SupplierCustomerId);
        var existing = grd.rows().map(function (r) { return HRM.int(r.RefDocSubIdNo); });
        dt.forEach(function (d) {
            if (existing.indexOf(HRM.int(d.DetailId)) >= 0) return;
            grd.rows().push({
                Id: 0, RefDocumentTypeId: d.DocumentTypeId, RefDocIdNo: d.Id, RefDocSubIdNo: d.DetailId, RefDocType: d.DocumentType, InvoiceNo: d.DocNo,
                InvoiceManualBillNo: d.ManualBillNo, WareHouseId: d.WarehouseId, ItemId: d.ItemId, ItemCode: d.ItemCode, Item: d.ItemName,
                CropYearId: d.CropYearId, JobLotId: d.JobLotId, PackingTypeId: d.PackingTypeId, ItemUOMId: d.ItemUOMId, ItemUOM: d.PackUom,
                PUomEquivalent: d.PackEquivalent, QTY: d.AvailableForDoDoQty, Weight: d.AvailableForDoDoWeight, PackingUnit: 0, PackingWeight: 0,
                GrossWeight: d.AvailableForDoDoWeight, Remarks: '', AvailableStock: 0, OtherRemarks: '', StockWeight: d.AvailableForDoDoWeight,
                InvoiceQty: d.AvailableForDoDoQty, InvoiceWeight: d.AvailableForDoDoWeight,
                WareHouseName: d.WareHouseName, CropYear: d.CropYear, JobLot: d.JobLotDescription, PackingType: d.PackTypeDesc
            });
        });
        grd.draw();
        HRM.enable('CmbSupplierCustomer', grd.rows().length === 0);
    }

    // ------------------------------------------------------------------ load / reset / read
    function buttons(mode) {                                                                    // 'new' | 'edit' | 'saveas'
        HRM.show('btnsave', mode === 'new'); HRM.show('btnupdate', mode === 'edit'); HRM.show('btnDelete', mode === 'edit'); HRM.show('btnSaveAs', mode === 'saveas');
    }
    function load() {
        return HRM.loading(get('/setup')).then(function (o) {
            S.rights = o.rights || {};
            S.allowMultiple = !!o.allowMultipleAdo; S.tolerance = n(o.tolerance); S.warnPacking = !!o.warningPackingWeight;
            HRM.applyRights(S.rights, { save: ['btnsave', 'btnSaveAs'], update: 'btnupdate', delete: 'btnDelete', print: ['btnprint', 'BtnPrintIII', 'btnPrintII'] });
            HRM.check('ChkPrint', S.rights.print !== false);
            if (o.itemSearchByCode) HRM.$('rdSearchByCode').checked = true; else HRM.$('rdSearchByName').checked = true;
            buildGrid();
            fillLists(o);
            HRM.setVal('txtdocno', o.docNo > 0 ? o.docNo : '');
            applyDefaults();
            HRM.fill('CmbCustomerHistory', o.historyCustomers || [], 'Id', 'ReferenceName', { zero: '' });
            PPB.resetHistoryFilters(HRM.int(o.defaultDays));
            buttons('new');
            HRM.focus('DocDate');
        }).catch(HRM.fail);
    }
    /** Reset() */
    function reset() {
        S.removed = []; S.recId = 0; S.isApproved = false;
        buttons('new');
        HRM.setVal('txtdocno', '');
        HRM.setCombo('CmbSupplierCustomer', 0); HRM.enable('CmbSupplierCustomer', true);
        HRM.setCombo('ultraCombo1', 0); HRM.setVal('txtVehicleNo', ''); HRM.setCombo('CmbVehicleType', 0); HRM.setVal('txtremarks', '');
        HRM.setCombo('CmbWareHouse', 0); HRM.setCombo('CmbItemName', 0); HRM.setCombo('CmbCropYear', 0); HRM.setCombo('CmbJobLot', 0);
        HRM.setCombo('CmbPackingType', 0); S.uoms = []; HRM.fill('CmbPackUom', [], 'Id', 'UOMCode', { zero: '' });
        HRM.setVal('txtqty', ''); HRM.setVal('txtweight', ''); HRM.setVal('txtPackUnit', '0'); HRM.setVal('txtPackWeight', '0'); HRM.setVal('txtGrossWeight', '0');
        HRM.setVal('txtremarksdetail', '');
        HRM.show('btnplus', true); HRM.show('btnUpdateDetail', false); HRM.show('btnCancelUpdateDetial', false);
        grd.clear();
        HRM.focus('DocDate');
        return get('/code').then(function (o) { HRM.setVal('txtdocno', o.docNo > 0 ? o.docNo : ''); applyDefaults(); }).catch(HRM.fail);
    }
    /** ReadById(ID): Reset() first; nothing more when the order has no detail rows. */
    function readById(id) {
        return reset().then(function () {
            S.recId = id;
            return HRM.loading(get('/by-id', { id: id })).then(function (o) {
                if (!(o.details || []).length) return;
                PPB.showTab('form');
                buttons('edit');
                HRM.setVal('txtdocno', HRM.str(o.DocNo)); HRM.setVal('DocDate', HRM.day(o.DocDate)); HRM.setVal('txtExpiryDate', HRM.day(o.ExpiryDate));
                HRM.setComboText('CmbVehicleType', HRM.str(o.VehicleType)); HRM.setVal('txtVehicleNo', HRM.str(o.VehicleNo));
                HRM.setCombo('CmbSupplierCustomer', o.details[0].SupplierCustomerId);
                HRM.setCombo('ultraCombo1', o.details[0].RefPartyId);
                HRM.setVal('txtremarks', HRM.str(o.LoadingInstructions));
                S.isApproved = HRM.bool(o.IsApproved);
                grd.set(o.details.map(function (d) { return Object.assign({}, d); }));
            });
        }).catch(HRM.fail);
    }
    /** grdhistory_Saveas(CurrId): the order read, only SaveAs visible, every row Id 0. */
    function saveAsFrom(id) {
        return readById(id).then(function () {
            buttons('saveas');
            grd.rows().forEach(function (r) { r.Id = 0; }); grd.draw();
        });
    }
    function detailGridBind(id) {                                                               // DetailGridBind(CurrId)
        return get('/by-id', { id: id }).then(function (o) { GrdHistoryDetail.set(o.details || []); }).catch(function () { GrdHistoryDetail.clear(); });
    }

    // ------------------------------------------------------------------ save / delete / print
    function insert(btn) {                                                                      // Insert()
        if (!grd.rows().length) { HRM.box('Grid Record Not Found'); return; }
        if (HRM.int(HRM.val('txtdocno')) === 0) { HRM.box('Doc No must be a non-zero number'); HRM.focus('txtdocno'); return; }
        if (HRM.comboVal('CmbSupplierCustomer') === 0) { HRM.box('Customer field is required'); HRM.focus('CmbSupplierCustomer'); return; }
        if ((HRM.val('DocDate') || '') > (HRM.val('txtExpiryDate') || '')) { HRM.box('Expiry Date Should be Greater than or Equal to Doc Date'); return; }
        if (!HRM.ask(S.recId > 0 ? 'Are you sure to Update' : 'Are you sure to Save')) return;
        if (S.warnPacking && grd.sum('PackingWeight') === 0 && !HRM.ask('There is no Packing Weight added in any row. Are you Sure To Proceed?')) return;
        var p910 = HRM.checked('ChkPrint'), p91001 = HRM.checked('chkPrintIII'), p910a = HRM.checked('chkPrintII');
        var body = {
            id: S.recId, docNo: HRM.val('txtdocno'), docDate: HRM.val('DocDate'), expiryDate: HRM.val('txtExpiryDate'),
            customerId: HRM.comboVal('CmbSupplierCustomer'), refPartyId: HRM.comboVal('ultraCombo1'), vehicleType: HRM.comboText('CmbVehicleType'),
            vehicleNo: HRM.val('txtVehicleNo'), remarks: HRM.val('txtremarks'), rows: grd.rows(), removed: S.removed
        };
        return HRM.busy(btn, function () {
            return post('/save', body).then(function (res) {
                HRM.box(res.message);
                return reset().then(function () {
                    if (p910) printSlip('910', res.id, null);
                    if (p91001) printSlip('910_01', res.id, null);
                    if (p910a) printSlip('910A', res.id, null);
                });
            }).catch(HRM.fail);
        });
    }
    /** AdvanceDeliveryOrderSlip910 / 910_01 / AdvanceDeliveryOrderChallanSlip910A: the saved order through the grid-to-PDF printer. */
    function printSlip(kind, id, btn) {
        return HRM.busy(btn, function () {
            return get('/slip', { id: id }).then(function (o) {
                PPB.printRows(kind + ' Advance Delivery Order No ' + HRM.str(o.DocNo), (o.details || []).map(function (d) {
                    return { DocNo: o.DocNo, DocDate: o.DocDate, ExpiryDate: o.ExpiryDate, VehicleNo: o.VehicleNo, Invoice: HRM.str(d.RefDocType) + ' ' + HRM.str(d.InvoiceNo),
                        ManualBillNo: d.InvoiceManualBillNo, Warehouse: d.WareHouseName, ItemCode: d.ItemCode, Item: d.Item, CropYear: d.CropYear,
                        JobLot: d.JobLot, PackingType: d.PackingType, UOM: d.ItemUOM, Qty: d.QTY, Weight: d.Weight, PackingWeight: d.PackingWeight,
                        GrossWeight: d.GrossWeight, StockWeight: d.StockWeight, Remarks: d.Remarks };
                }));
            }).catch(HRM.fail);
        }, 'ado-print-' + kind);
    }
    P.btnsave = function (btn) { S.recId = 0; return insert(btn || 'btnsave'); };
    P.btnSaveAs = function (btn) { S.recId = 0; return insert(btn || 'btnSaveAs'); };
    P.btnupdate = function (btn) { if (S.recId === 0) { HRM.box('Record Not Update because RecId Not Found'); return; } return insert(btn || 'btnupdate'); };
    P.btnnew = function () { return reset(); };
    P.btnRefresh = function (btn) {
        return HRM.busy(btn || 'btnRefresh', function () { return get('/refresh').then(function (o) { fillLists(o); applyDefaults(); }).catch(HRM.fail); });
    };
    P.btnDelete = function (btn) {                                                              // btnDelete_Click
        if (S.recId === 0) { HRM.box('Record Id Not Found'); return; }
        if (!HRM.ask('Are you sure to Delete?')) return;
        if (S.isApproved) { HRM.box('Record has been approved'); return; }
        return HRM.busy(btn || 'btnDelete', function () {
            return post('/delete', {}, { id: S.recId }).then(function (res) { HRM.box(res.message); return reset(); }).catch(HRM.fail);
        });
    };
    P.print = function (kind, btn) { return printSlip(kind, S.recId, btn); };
    P.shortcuts = function () {                                                                 // MakeShortCutKeys()
        HRM.box('Ctrl+S  For Save in Form Tab and For Show History in History Tab\nCtrl+U  For Update\nCtrl+E  For Close\nCtrl+R  For Refresh\n' +
            'Ctrl+N  For New\nCtrl+P  For Print 910\nAlt+1  For Print 910\nAlt+2  For Print 910A\nCtrl+F1  For Reference Parties Look up\n' +
            'Ctrl+F10  For Open Attachments\nCtrl+F12  For SaveAs\nCtrl+T  For Tab Transfer\nCtrl+alt  To Show ShortCut Keys Form\n' +
            'Ctrl+ArrowDown  For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab\n' +
            'Ctrl+ArrowUp  For Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab\n' +
            'Ctrl+Enter  When Focus On Any Grid For Update Record\nCtrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link');
    };

    // ------------------------------------------------------------------ history
    P.btnShowHistory = function (btn) {                                                         // gridhistoryfill()
        return HRM.busy(btn || 'btnShowHistory', function () {
            return post('/history', PPB.historyFilters({ customerId: HRM.comboVal('CmbCustomerHistory') }))
                .then(function (rows) { grdhistory.set(rows || []); GrdHistoryDetail.clear(); }).catch(HRM.fail);
        });
    };
    P.btnResetHistory = function () {                                                           // btnResetHistory_Click: today - 3 days
        PPB.resetHistoryFilters(3, ['CmbCustomerHistory']);
        grdhistory.clear(); GrdHistoryDetail.clear();
    };
    P.btnRefreshHistory = function (btn) {
        return HRM.busy(btn || 'btnRefreshHistory', function () {
            return get('/history-customers').then(function (rows) { HRM.fill('CmbCustomerHistory', rows || [], 'Id', 'ReferenceName', { zero: '', keep: true }); }).catch(HRM.fail);
        });
    };

    // ------------------------------------------------------------------ wiring
    PPB.tabs(function (name) { HRM.focus(name === 'history' ? 'FromDateHistory' : 'DocDate'); });
    HRM.footer(function () { PPB.showTab('history'); });
    ['rdSearchByName', 'rdSearchByCode'].forEach(function (id) { HRM.$(id).addEventListener('change', function () { itemBind(); }); });
    PPB.leave('CmbItemName', function () { packUomBind().then(stockLabel); });                  // CmbItemName_Leave
    ['CmbWareHouse', 'CmbCropYear', 'CmbJobLot', 'CmbPackingType'].forEach(function (id) { HRM.$(id).addEventListener('change', stockLabel); });
    HRM.$('CmbPackUom').addEventListener('change', stockLabel);
    PPB.leave('CmbPackUom', calculateWeight);                                                   // combitempck_Leave
    HRM.$('txtqty').addEventListener('input', calculateWeight);
    HRM.$('txtweight').addEventListener('input', function () { calculateGrossWeight(); });
    HRM.$('txtPackUnit').addEventListener('input', function () { calculateGrossWeight(); });
    HRM.$('txtPackWeight').addEventListener('input', function () { calculateGrossWeight('PackWeight'); });
    HRM.$('DocDate').addEventListener('change', function () { rowStocks(); });
    PPB.upper('txtVehicleNo');
    ['txtqty', 'txtweight', 'txtPackUnit', 'txtPackWeight'].forEach(function (id) { PPB.guard(id, 'dec'); });
    PPB.guard('txtFromDocNoHistory', 'int'); PPB.guard('txtToDocNoHistory', 'int');
    HRM.setVal('DocDate', HRM.today()); HRM.setVal('txtExpiryDate', HRM.today());
    PPB.resetHistoryFilters(3);
    function focusGrid(id) { var t = HRM.$(id); var tr = t && t.querySelector('tbody tr'); if (tr) tr.focus(); }
    function formTab() { return PPB.currentTab() === 'form'; }
    function canPrint() { return S.rights.print !== false; }
    document.addEventListener('keydown', function (e) {                                        // grd_KeyDown: Ctrl+D / Ctrl+Delete with partial invoicing
        if (!e.ctrlKey || !S.allowMultiple || !grd) return;
        var inGrid = e.target && e.target.closest && e.target.closest('#grd'); if (!inGrid) return;
        var i = grd.currentIndex(); if (i < 0) return;
        if ((e.key || '').toLowerCase() === 'd') { e.preventDefault(); addDetailRow(i); }
        else if (e.key === 'Delete') { e.preventDefault(); deleteDetailRow(i); }
    });
    HRM.keys({
        'ctrl+e': HRM.close, 'esc': HRM.close,
        'ctrl+alt+alt': P.shortcuts, 'ctrl+alt+control': P.shortcuts,
        'ctrl+t': function () { PPB.toggleTab(); },
        'ctrl+n': function () { if (formTab()) P.btnnew(); else P.btnResetHistory(); },
        'ctrl+s': function () { if (!formTab()) P.btnShowHistory(); else if (HRM.visible('btnsave') && !HRM.$('btnsave').disabled) P.btnsave(); },
        'ctrl+r': function () { if (formTab()) P.btnRefresh(); else P.btnRefreshHistory(); },
        'ctrl+shift+delete': function () { if (formTab() && HRM.visible('btnDelete') && !HRM.$('btnDelete').disabled) P.btnDelete(); },
        'ctrl+u': function () { if (formTab() && HRM.visible('btnupdate') && !HRM.$('btnupdate').disabled) P.btnupdate(); },
        'ctrl+f5': function () { HRM.focus(formTab() ? 'DocDate' : 'FromDateHistory'); },
        'ctrl+f12': function () { if (formTab() && HRM.visible('btnSaveAs') && !HRM.$('btnSaveAs').disabled) P.btnSaveAs(); },
        'ctrl+arrowdown': function () { focusGrid(formTab() ? 'grd' : 'grdhistory'); },
        'ctrl+arrowup': function () { HRM.focus(formTab() ? 'DocDate' : 'FromDateHistory'); },
        'ctrl+p': function () { if (formTab() && canPrint()) P.print('910'); },
        'alt+1': function () { if (formTab() && canPrint()) P.print('910'); },
        'alt+2': function () { if (formTab() && canPrint()) P.print('910A'); }
    });
    load();
})();
