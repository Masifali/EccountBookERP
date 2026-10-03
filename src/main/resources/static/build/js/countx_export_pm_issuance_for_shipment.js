/* ============================================================================================
 * countx_export_pm_issuance_for_shipment.js - PackingMaterialIssuanceForShipment.cs
 * (Architecture.WinApp.Export) "Packing Material / Consumable Store Consumption For Shipment",
 * InvGsStoreIssuanceHeader DocumentTypeId 213. Every event of the desktop form has its counterpart:
 * Load, txtDocdate_Leave, CmbInvoiceNo_Leave, cmbItem_Leave, CmbWarehouse_Leave, CmbRackName_Leave,
 * CmbItemCondition_Leave, txtItemQty / txtItemRate TextChanged, + / Update / Cancel, grid DoubleClick /
 * X / KeyDown, New, Refresh, Save, Update, Attachment, 118-Voucher, 475-Print, ShortCut Keys, Check Stock,
 * history New / Show / SelectionChanged / DoubleClick / Edit / Print, form KeyDown.
 * Data: /api/export/pm-issuance-for-shipment.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var G = global.ExportG, S = global.ExportSF;
    var API = '/api/export/pm-issuance-for-shipment';
    var DOC_TYPE = 213;
    /* RptGenrateStocksStore (ScreenDefinition 300) has no web page yet; set the route here when it exists. */
    var STOCK_REPORT_ROUTE = null;

    var RIGHTS = { Save: false, Update: false, Print: false, CanViewAllRecord: false };
    var STOCK_VIEW = false;
    var RECID = 0, VOUCHER_HEAD_ID = 0, UPDATE_INDEX = -1;
    var L = { invoices: [], items: [], conditions: [], accounts: [], ports: [], racks: [], defaultWarehouse: 0 };
    var HIST = [];

    var grdDetail = G.grid('grdDetail', [
        { key: 'ItemName' }, { key: 'WareHouseName' }, { key: 'RackName' }, { key: 'ItemCondition' },
        { key: 'ItemQty', num: true }, { key: 'ItemRate', num: true }, { key: 'ItemAmount', num: true }, { key: 'Remarks' }, { key: 'DebitAc' }
    ], { withX: true, totals: ['ItemQty', 'ItemAmount'], onDelete: deleteRow, onDblClick: editRow });

    var grdHistory = G.grid('DataGridHistory', [
        { key: 'DocNo', link: true }, { key: 'DocDate', fmt: G.ddMMMyyyy }, { key: 'InvoiceNo' }, { key: 'EntryUser' }, { key: 'EntryDate', datetime: true },
        { key: 'ModifyUser' }, { key: 'ModifyDate', datetime: true }, { key: 'Remarks' }, { key: 'NoOfAttachments', num: true, dec: 0 }
    ], { buttons: [{ key: 'Edit', text: 'Edit' }, { key: 'Print', text: 'Print' }], emptyId: 'DataGridHistoryEmpty',
        onButton: historyButton, onSelect: historySelect, onDblClick: historyDoubleClick, onLink: function (i) { historyDoubleClick(i); } });

    var grdDetailOfMain = G.grid('grddetailofmain', [
        { key: 'ItemName' }, { key: 'Warehouse' }, { key: 'RackName' }, { key: 'ItemCondition' }, { key: 'ItemQty', num: true },
        { key: 'ItemRate', num: true }, { key: 'ItemAmount', num: true }, { key: 'Remarks' }, { key: 'DebitAc' }
    ], { totals: ['ItemQty', 'ItemAmount'] });

    var footerLabel = null;
    var tabs = G.tabs('main', ['tabForm', 'tabHistory'], function (p) {
        if (footerLabel) footerLabel();
        if (p === 'tabHistory') G.focus('FromDateHistory');                       /* tabControlGsIssuance_SelectedIndexChanged */
    });
    footerLabel = S.footerToggle('btnFooterHistory', tabs, 'tabForm', 'tabHistory');

    // ------------------------------------------------------------------------------ binds

    function bindInvoices(keep) { S.bindX('CmbInvoiceNo', L.invoices, 'Id', 'InvoiceNo', ['DocumentTypeId', 'NoOfContainers', 'MTon'], keep); }
    /* ItemNameBind: all columns, ParentCategoryId hidden. */
    function bindItems() { S.bindX('cmbItem', L.items, 'Id', 'ItemName', ['ItemCode', 'ItemCategory', 'LeadTimeDay', 'WeightCapacity']); }
    function bindConditions() { S.bindX('CmbItemCondition', L.conditions, 'Id', 'Description', []); }
    function bindAccounts() { S.bindX('cmbDrAc', L.accounts, 'Id', 'AccountTitle', ['AccountCode', 'ParentAccountTitle', 'AccountClass']); }
    /* PortFrom: both port combos from SeaPorts, values kept when still present. */
    function bindPorts() { S.bindX('CmbPortFrom', L.ports, 'Id', 'PortName', []); S.bindX('CmbPortTo', L.ports, 'Id', 'PortName', []); }
    function applyLookups(d) {
        ['invoices', 'items', 'itemConditions', 'debitAccounts', 'ports', 'racks', 'docNo'].forEach(function (k) { if (d[k + 'Error']) G.box(d[k + 'Error']); });
        if (d.invoices) { L.invoices = d.invoices; bindInvoices(); }
        if (d.items) { L.items = d.items; bindItems(); }
        if (d.itemConditions) { L.conditions = d.itemConditions; bindConditions(); }
        if (d.debitAccounts) { L.accounts = d.debitAccounts; bindAccounts(); }
        if (d.ports) { L.ports = d.ports; bindPorts(); }
        if (d.racks) L.racks = d.racks;
        if (d.packingMaterialDefaultWarehouse !== undefined) L.defaultWarehouse = G.netI(d.packingMaterialDefaultWarehouse);
    }

    // ------------------------------------------------------------------------------ load

    /** frmGSIssuance_Load. */
    function load() {
        G.setText('txtDocdate', G.today());
        G.setText('FromDateHistory', G.today()); G.setText('ToDateHistory', G.today());
        return G.getJson(API + '/setup').then(function (d) {
            d = d || {};
            RIGHTS = d.rights || RIGHTS;
            STOCK_VIEW = !!d.stockReportView;
            G.$id('btnsave').disabled = !RIGHTS.Save;
            G.$id('btnUpdate').disabled = !RIGHTS.Update;
            G.$id('btnVoucher').disabled = !RIGHTS.Print;
            G.$id('Print').disabled = !RIGHTS.Print;
            if (G.netI(d.docNo) > 0) G.setText('txtDocNo', d.docNo);
            applyLookups(d);
            grdDetail.draw([]);
            return itemLeave();
        }).then(function () { G.focus('txtDocdate'); }).catch(function (e) { G.box(e.message); });
    }

    // ------------------------------------------------------------------------------ stock / rate

    function stockQuery(withLocation) {
        var q = '?recId=' + RECID + '&itemId=' + G.valI('cmbItem') + '&docDate=' + encodeURIComponent(G.val('txtDocdate')) + '&itemConditionId=' + G.valI('CmbItemCondition');
        if (withLocation) q += '&warehouseId=' + G.valI('CmbWarehouse') + '&rackId=' + G.valI('CmbRackName');
        return G.getJson(API + '/stock' + q);
    }
    /** BalanceStockQtyandAvgRate: QtyInHand ("#,##.###"; 0 prints empty). */
    function balanceStock() {
        return stockQuery(true).then(function (d) {
            var stock = G.netD(d && d.qtyInHand);
            G.setText('txtBalanceQty', stock > 0 ? S.fmtHash(stock, 3) : '');
        }).catch(function (e) { G.box(e.message); });
    }
    /** getAvgRate: "0", then the rounded average rate when > 0. */
    function avgRate() {
        G.setText('txtItemRate', '0');
        return stockQuery(false).then(function (d) {
            var r = G.netD(d && d.avgRate);
            if (r > 0) G.setText('txtItemRate', G.str(r));
            amountCalc();
        }).catch(function (e) { G.box(e.message); });
    }
    /** AmountCalculation. */
    function amountCalc() {
        var q = G.netD(G.val('txtItemQty')), r = G.netD(G.val('txtItemRate'));
        G.setText('txtItemAmount', q !== 0 && r !== 0 ? G.fmt(q * r, 3) : '0');
    }

    // ------------------------------------------------------------------------------ leave events

    /** txtDocdate_Leave. */
    function docDateLeave() {
        return balanceStock().then(avgRate).then(function () { if (grdDetail.rows.length > 0) return avgRateOnDocDateChange(); });
    }
    /** AvgRateUpdateOnDocDateChange: every row's rate (Math.Round 3) and amount = qty * rate. */
    function avgRateOnDocDateChange() {
        if (grdDetail.rows.length <= 0) return Promise.resolve();
        return G.postJson(API + '/rates-on-date', { recId: RECID, docDate: G.val('txtDocdate'), rows: grdDetail.rows }).then(function (rates) {
            (rates || []).forEach(function (rate, i) {
                var r = grdDetail.rows[i]; if (!r) return;
                r.ItemRate = G.netD(rate);
                r.ItemAmount = G.netD(r.ItemQty) * r.ItemRate;
            });
            grdDetail.draw(grdDetail.rows);
        }).catch(function (e) { G.box(e.message); });
    }
    /** CmbInvoiceNo_Leave -> InvoiceInfo. */
    function invoiceLeave() {
        var id = G.valI('CmbInvoiceNo');
        return G.getJson(API + '/invoice-info?invoiceId=' + id).then(function (rows) {
            rows = rows || [];
            if (rows.length > 0) {
                var r = rows[0];
                G.setVal('CmbPortFrom', G.netI(r.LoadingPortId));
                G.setVal('CmbPortTo', G.netI(r.DestinationPortId));
                G.setText('txtnoofcontainer', G.str(r.NoOfContainers));
                G.setText('txtnetweight', G.fmt(G.netD(r.NetWeight) / 1000.0, 3));
                S.bindX('CmbContractNo', rows, 'LcOrderNoId', 'LcOrderNo', [], G.netI(r.LcOrderNoId));
                S.bindX('CmbScheduleNo', rows, 'ContractScheduleId', 'ContractScheduleNo', [], G.netI(r.ContractScheduleId));
                S.bindX('CmbCustomer', rows, 'SupplierCustomerId', 'CustomerName', [], G.netI(r.SupplierCustomerId));
                if (G.netI(r.CreditAccountId) > 0) G.setVal('cmbDrAc', G.netI(r.CreditAccountId));
            } else {
                G.setVal('CmbPortFrom', '0'); G.setVal('CmbPortTo', '0');
                G.setText('txtnoofcontainer', '');
                G.setVal('CmbContractNo', '0'); G.setVal('CmbScheduleNo', '0'); G.setVal('CmbCustomer', '0');
                G.setVal('cmbDrAc', '0');
                G.setText('txtnetweight', '');
            }
        }).catch(function (e) { G.box(e.message); });
    }
    /** cmbItem_Leave: stock, rate, BindWarehouseDropdown, RackBindFromGlobalRacksByItemId. */
    function itemLeave() {
        var itemId = G.valI('cmbItem');
        return balanceStock().then(avgRate).then(function () {
            bindWarehouses(itemId);
            return rackBind(itemId, G.valI('CmbWarehouse'));
        });
    }
    /** BindWarehouseDropdown: the item's racks grouped by warehouse; one -> chosen, several -> PackingMaterialDefaultWarehouse. */
    function bindWarehouses(itemId) {
        var seen = {}, rows = [];
        L.racks.forEach(function (x) { if (G.netI(x.ItemId) === itemId && !seen[x.WarehouseId]) { seen[x.WarehouseId] = 1; rows.push({ Id: x.WarehouseId, Warehouse: x.WareHouseName }); } });
        S.bindX('CmbWarehouse', rows, 'Id', 'Warehouse', []);
        if (G.valI('CmbWarehouse') !== 0) return;
        if (rows.length === 1) G.setVal('CmbWarehouse', rows[0].Id);
        else if (rows.length > 1 && L.defaultWarehouse !== 0 && rows.some(function (r) { return G.netI(r.Id) === L.defaultWarehouse; })) G.setVal('CmbWarehouse', L.defaultWarehouse);
    }
    /** RackBindFromGlobalRacksByItemId: one rack -> chosen (+ Leave); several -> the BaseRackId one (+ Leave). */
    function rackBind(itemId, warehouseId) {
        var seen = {}, rows = [];
        L.racks.forEach(function (x) {
            if (G.netI(x.ItemId) === itemId && (warehouseId === 0 || G.netI(x.WarehouseId) === warehouseId) && !seen[x.Id]) {
                seen[x.Id] = 1; rows.push({ Id: x.Id, RackName: x.RackName, WarehouseId: x.WarehouseId, WarehouseName: x.WareHouseName, BaseRackId: x.BaseRackId });
            }
        });
        S.bindX('CmbRackName', rows, 'Id', 'RackName', ['WarehouseName']);
        if (G.valI('CmbRackName') !== 0) return Promise.resolve();
        if (rows.length === 1) { G.setVal('CmbRackName', rows[0].Id); return rackLeave(); }
        if (rows.length > 1) {
            var base = rows.filter(function (r) { return G.netI(r.BaseRackId) > 0; })[0];
            if (base) { G.setVal('CmbRackName', base.Id); return rackLeave(); }
        }
        return Promise.resolve();
    }
    /** CmbWarehouse_Leave. */
    function warehouseLeave() { return rackBind(G.valI('cmbItem'), G.valI('CmbWarehouse')).then(balanceStock); }
    /** CmbRackName_Leave: the rack's warehouse when none or another is chosen, then the stock. */
    function rackLeave() {
        var rackId = G.valI('CmbRackName');
        var rack = L.racks.filter(function (r) { return G.netI(r.Id) === rackId; })[0];
        if (!rack) return Promise.resolve();
        var wh = G.valI('CmbWarehouse');
        if (wh === 0 || G.netI(rack.WarehouseId) !== wh) G.setVal('CmbWarehouse', rack.WarehouseId);
        return balanceStock();
    }
    /** CmbItemCondition_Leave. */
    function conditionLeave() { return balanceStock().then(avgRate); }

    // ------------------------------------------------------------------------------ detail entry

    function selText(id) { var s = G.$id(id); return s && s.selectedIndex > 0 ? s.options[s.selectedIndex].textContent : ''; }
    /** FormValidationDetail. */
    function detailValidation() {
        var checks = [['cmbItem', 'Item Field Required'], ['CmbWarehouse', 'Warehouse Field Required'], ['CmbRackName', 'Rack Name Field Required'],
            ['CmbItemCondition', 'Item Condition Field Required']];
        for (var i = 0; i < checks.length; i++) if (G.valI(checks[i][0]) === 0) { G.box(checks[i][1]); G.focus(checks[i][0]); return false; }
        if (G.netD(G.val('txtItemQty').trim()) === 0) { G.box('ItemQty Field Required'); G.focus('txtItemQty'); return false; }
        if (G.netD(G.val('txtItemRate').trim()) === 0) { G.box('ItemRate Field Required'); G.focus('txtItemRate'); return false; }
        if (G.netD(G.val('txtItemAmount').trim()) === 0) { G.box('ItemAmount Field Required'); G.focus('txtItemAmount'); return false; }
        if (G.valI('cmbDrAc') === 0) { G.box('Debit Account Field Required'); G.focus('cmbDrAc'); return false; }
        return true;
    }
    function rowFromEntry() {
        return { ItemId: G.valI('cmbItem'), ItemName: selText('cmbItem').trim(), WareHouseId: G.valI('CmbWarehouse'), WareHouseName: selText('CmbWarehouse'),
            RackId: G.valI('CmbRackName'), RackName: selText('CmbRackName'), ItemConditionId: G.valI('CmbItemCondition'), ItemCondition: selText('CmbItemCondition').trim(),
            ItemQty: G.netD(G.val('txtItemQty').trim()), ItemRate: G.netD(G.val('txtItemRate').trim()), ItemAmount: G.netD(G.val('txtItemAmount').trim()),
            Remarks: G.val('txtRemarksDetail').trim(), DrAcId: G.valI('cmbDrAc'), DebitAc: selText('cmbDrAc').trim() };
    }
    /** Add_Click. */
    function addRow() {
        if (!detailValidation()) return;
        grdDetail.rows.push(rowFromEntry());
        grdDetail.draw(grdDetail.rows);
        resetDetail();
    }
    /** btnUpdateDetail_Click (texts not trimmed except where the desktop trims). */
    function updateRow() {
        if (!detailValidation()) return;
        var r = grdDetail.rows[UPDATE_INDEX]; if (!r) return;
        var n = rowFromEntry();
        n.ItemName = selText('cmbItem'); n.ItemCondition = selText('CmbItemCondition'); n.DebitAc = selText('cmbDrAc'); n.Remarks = G.val('txtRemarksDetail');
        grdDetail.rows[UPDATE_INDEX] = n;
        grdDetail.draw(grdDetail.rows);
        resetDetail();
    }
    /** grdDetail_DoubleClick. */
    function editRow(i) {
        var item = grdDetail.rows[i]; if (!item) return;
        UPDATE_INDEX = i;
        G.setVal('cmbItem', item.ItemId);
        itemLeave().then(function () {
            G.setVal('CmbWarehouse', item.WareHouseId);
            G.setVal('CmbRackName', item.RackId);
            G.setVal('CmbItemCondition', item.ItemConditionId);
            G.setText('txtItemQty', G.fmt(item.ItemQty)); G.setText('txtItemRate', G.fmt(item.ItemRate)); G.setText('txtItemAmount', G.fmt(item.ItemAmount));
            G.setText('txtRemarksDetail', item.Remarks);
            G.setVal('cmbDrAc', item.DrAcId);
            G.show('Add', false); G.show('btnUpdateDetail', true); G.show('btnCancelDetail', true);
            G.focus('cmbItem');
        });
    }
    /** grdDetail_ColumnButtonClick "Delete" (no confirm). */
    function deleteRow(i) { grdDetail.rows.splice(i, 1); grdDetail.draw(grdDetail.rows); }
    /** ResetDetail. */
    function resetDetail() {
        UPDATE_INDEX = -1;
        G.setVal('cmbItem', '0');
        S.bindX('CmbWarehouse', [], 'Id', 'Warehouse', []);
        S.bindX('CmbRackName', [], 'Id', 'RackName', []);
        ['txtBalanceQty', 'txtItemQty', 'txtItemRate', 'txtItemAmount', 'txtRemarksDetail'].forEach(function (id) { G.setText(id, ''); });
        G.show('Add', true); G.show('btnUpdateDetail', false); G.show('btnCancelDetail', false);
        G.focus('cmbItem');
    }

    // ------------------------------------------------------------------------------ form

    /** formReset (btnnew_Click). */
    function formReset() {
        RECID = 0; VOUCHER_HEAD_ID = 0;
        G.setVal('CmbInvoiceNo', '0');
        G.setText('txtRemarks', '');
        G.setVal('cmbDrAc', '0');
        ['CmbContractNo', 'CmbScheduleNo', 'CmbCustomer', 'CmbPortFrom', 'CmbPortTo'].forEach(function (id) { G.setVal(id, '0'); });
        G.setText('txtnoofcontainer', ''); G.setText('txtnetweight', '');
        grdDetail.draw([]);
        G.show('btnsave', true); G.show('btnUpdate', false);
        return Promise.all([
            G.getJson(API + '/invoices?recId=' + RECID).then(function (rows) { L.invoices = rows || []; bindInvoices(); }),
            G.getJson(API + '/doc-no').then(function (d) { if (d && G.netI(d.docNo) > 0) G.setText('txtDocNo', d.docNo); })
        ]).catch(function (e) { G.box(e.message); }).then(function () { resetDetail(); G.focus('txtDocdate'); });
    }

    /** Insert(): FormValidation, the confirm, then the server (grid / row validations repeated there). */
    function insert(btn) {
        return G.busy(btn, function () {
            var docNo = G.val('txtDocNo').trim();
            if (docNo === '' || G.netI(docNo) === 0) { G.box('DocNo Field Required'); G.focus('txtDocNo'); return Promise.resolve(); }
            if (G.valI('CmbInvoiceNo') === 0) { G.box('Invoice No Field Required'); G.focus('CmbInvoiceNo'); return Promise.resolve(); }
            if (!G.ask(RECID > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
            return G.postJson(API + '/save', { recId: RECID, docNo: docNo, docDate: G.val('txtDocdate'), invoiceId: G.valI('CmbInvoiceNo'),
                remarks: G.val('txtRemarks'), rows: grdDetail.rows }).then(function (d) {
                G.box(d && d.message);
                return formReset();
            }).catch(function (e) { G.box(e.message); });
        });
    }
    function save(btn) { RECID = 0; return insert(btn); }                    /* btnsave_Click */
    function update(btn) { return insert(btn); }                             /* btnUpdate_Click */

    /** ReadById. */
    function readById(id, btn) {
        return G.busy(btn, function () {
            return G.getJson(API + '/by-id?id=' + id).then(function (h) {
                tabs.select('tabForm'); if (footerLabel) footerLabel();
                G.show('btnsave', false); G.show('btnUpdate', true);
                G.setText('txtDocdate', h.DocDate);
                G.setText('txtDocNo', h.DocNo);
                G.setText('txtRemarks', h.Remarks);
                VOUCHER_HEAD_ID = G.netI(h.voucherHeadId);
                grdDetail.draw(h.rows || []);
                return G.getJson(API + '/invoices?recId=' + RECID).then(function (rows) { L.invoices = rows || []; bindInvoices(h.ExImInvoiceId); })
                    .then(function () { resetDetail(); });
            }).catch(function (e) { G.box(e.message); });
        });
    }

    /** Print_Click / GeneratePrint -> StoreIssuanceHeader_Slip475. */
    function generatePrint(id, btn) {
        if (!id) { G.box('No Record Found For Display'); return; }
        if (!global.CrystalPrint) { G.box('Print is not available.'); return; }
        var w = global.CrystalPrint.reserve();
        G.getJson(API + '/print-check?id=' + id).then(function () { return global.CrystalPrint.open('exp-475', { id: id }, btn, w); })
            .catch(function (e) { global.CrystalPrint.release(w); G.box(e.message); });
    }
    function printClick(btn) { if (RECID > 0) generatePrint(RECID, btn); else G.box('No record found'); }
    /** btnVoucher_Click -> VoucherReport_118(VoucherHeadId, 213). */
    function voucherClick(btn) {
        if (VOUCHER_HEAD_ID === 0) { G.box('VoucherId Not Found'); return; }
        S.print('acc-118', { id: VOUCHER_HEAD_ID, documentTypeId: DOC_TYPE }, btn);
    }
    /** btnRefresh_Click: global services, InvoiceNoFill, items, conditions, debit accounts, ports. */
    function refresh(btn) {
        return G.busy(btn, function () {
            return G.getJson(API + '/refresh?recId=' + RECID).then(function (d) { applyLookups(d || {}); }).catch(function (e) { G.box(e.message); });
        });
    }
    /** BtnStockReport_Click. */
    function stockReport() {
        if (!STOCK_VIEW) { G.box("You Don't Have rights View Of This Form.."); return; }
        if (!STOCK_REPORT_ROUTE) { G.box('RptGenrateStocksStore (Stock Stocks Report, screen 300) is not available in the web version yet.'); return; }
        global.open(STOCK_REPORT_ROUTE + '?warehouseId=' + G.valI('CmbWarehouse') + '&itemId=' + G.valI('cmbItem'), '_blank');
    }
    function shortcuts() {
        G.shortcuts([['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Alt+1', 'For Vouvher Print'], ['Alt+2', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachment'],
            ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'],
            ['Ctrl+ArrowUp', 'For Focus On Warehouse in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    // ------------------------------------------------------------------------------ history

    /** HistoryFill. */
    function historyFill(btn) {
        return G.busy(btn, function () {
            var q = '?fromDate=' + (G.checked('FromDateHistoryChk') ? encodeURIComponent(G.val('FromDateHistory')) : '')
                + '&toDate=' + (G.checked('ToDateHistoryChk') ? encodeURIComponent(G.val('ToDateHistory')) : '')
                + '&fromDocNo=' + G.valI('FromDocNoHistory') + '&toDocNo=' + G.valI('ToDocNoHistory');
            return G.getJson(API + '/history' + q).then(function (rows) {
                HIST = rows || [];
                grdHistory.current = -1;
                grdHistory.draw(HIST);
            }).catch(function (e) { G.box(e.message); });
        });
    }
    /** btnNewHistory_Click. */
    function historyNew() {
        G.setText('FromDateHistory', G.today()); G.setText('ToDateHistory', G.today());
        G.setText('FromDocNoHistory', ''); G.setText('ToDocNoHistory', '');
        HIST = []; grdHistory.draw([]); grdDetailOfMain.draw([]);
        G.focus('FromDateHistory');
    }
    /** DataGridHistory_SelectionChanged: RECID = the row, its detail below (GetByID). */
    function historySelect(i) {
        var r = HIST[i]; if (!r) return;
        RECID = G.netI(r.Id);
        G.getJson(API + '/by-id?id=' + RECID).then(function (h) {
            grdDetailOfMain.draw((h.rows || []).map(function (d) {
                return { ItemName: d.ItemName, Warehouse: d.WareHouseName, RackName: d.RackName, ItemCondition: d.ItemCondition, ItemQty: d.ItemQty,
                    ItemRate: d.ItemRate, ItemAmount: d.ItemAmount, Remarks: d.Remarks, DebitAc: d.DebitAc };
            }));
        }).catch(function (e) { G.box(e.message); });
    }
    /** DataGridHistory_DoubleClick. */
    function historyDoubleClick(i) { var r = HIST[i]; if (!r) return; RECID = G.netI(r.Id); readById(RECID); }
    /** DataGridHistory_ColumnButtonClick: Edit (Update right) / Print (Print right). */
    function historyButton(i, key, btn) {
        var r = HIST[i]; if (!r) return;
        if (key === 'Edit') {
            if (!RIGHTS.Update) { G.box("You don't have right to update"); return; }
            RECID = G.netI(r.Id); readById(RECID, btn);
        } else if (key === 'Print') {
            if (!RIGHTS.Print) { G.box("You don't have right to print"); return; }
            generatePrint(G.netI(r.Id), btn);
        }
    }

    // ------------------------------------------------------------------------------ wiring

    function on(id, ev, fn) { var e = G.$id(id); if (e) e.addEventListener(ev, fn); }
    function onLeave(id, fn) {
        var e = G.$id(id); if (!e) return;
        if (e.tagName === 'SELECT') e.addEventListener('change', fn); else e.addEventListener('blur', fn);
    }

    document.addEventListener('DOMContentLoaded', function () {
        G.initFullscreen();
        on('btnnew', 'click', formReset);
        on('btnRefresh', 'click', function () { refresh(this); });
        on('btnsave', 'click', function () { save(this); });
        on('btnUpdate', 'click', function () { update(this); });
        on('btnAttachment', 'click', function () { G.box('Attachments are not available in the web version.'); });
        on('btnVoucher', 'click', function () { voucherClick(this); });
        on('Print', 'click', function () { printClick(this); });
        on('btnShortcutkeys', 'click', shortcuts);
        on('Add', 'click', addRow);
        on('btnUpdateDetail', 'click', updateRow);
        on('btnCancelDetail', 'click', resetDetail);
        on('BtnStockReport', 'click', stockReport);
        on('btnShowHistory', 'click', function () { historyFill(this); });
        on('btnNewHistory', 'click', historyNew);
        onLeave('txtDocdate', docDateLeave);
        onLeave('CmbInvoiceNo', invoiceLeave);
        onLeave('cmbItem', itemLeave);
        onLeave('CmbWarehouse', warehouseLeave);
        onLeave('CmbRackName', rackLeave);
        onLeave('CmbItemCondition', conditionLeave);
        on('txtItemQty', 'input', amountCalc);
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            var onForm = tabs.current() === 'tabForm';
            if (S.enterAsTab(e)) return;
            if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
            if (e.ctrlKey && k === 's' && onForm && !G.$id('btnsave').classList.contains('is-hidden') && !G.$id('btnsave').disabled) { e.preventDefault(); save(G.$id('btnsave')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); formReset(); }
            if (e.ctrlKey && k === 't') { e.preventDefault(); tabs.select(onForm ? 'tabHistory' : 'tabForm'); if (onForm) G.focus('DataGridHistory'); else G.focus('txtDocdate'); }
            if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('.ex-fullscreen')) { e.preventDefault(); G.cancelWindow(); } }
            if (e.ctrlKey && k === 'u' && !G.$id('btnUpdate').classList.contains('is-hidden') && !G.$id('btnUpdate').disabled) { e.preventDefault(); update(G.$id('btnUpdate')); }
            if (e.altKey && (e.key === '1')) { e.preventDefault(); voucherClick(G.$id('btnVoucher')); }
            if (e.altKey && (e.key === '2')) { e.preventDefault(); printClick(G.$id('Print')); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(G.$id('btnRefresh')); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); G.focus('txtDocdate'); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); G.focus(onForm ? 'grdDetail' : 'grddetailofmain'); }
            if (e.ctrlKey && e.key === 'ArrowUp') { e.preventDefault(); G.focus(onForm ? 'CmbWarehouse' : 'DataGridHistory'); }
        });
        load();
    });

    global.ExportPmIssuance = { formReset: formReset, save: save, update: update, readById: readById };
})(window);
