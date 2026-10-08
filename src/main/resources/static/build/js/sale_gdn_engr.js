/*
 * Screen 539  GoodsDispatchNotes_Engr  (Architecture.WinApp.SaleTrading.GoodsDispatchNotes_Engr, frmGSIssuance, document type 1612)
 * Page script. Desktop methods are named in the comments. Server: /sale/engr/gdn/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/gdn/api', DOC = 1612;
    var S = {
        id: 0, gpId: 0, saleOrderId: 0, orderStatus: '', rights: {}, subsidiary: false, updateIdx: -1,
        files: [], removedAtt: [], existing: [],
        out: [], sup: [], items: [], wh: [], job: [], asset: [], other: [], histRows: [], trans: []
    };
    var cb = {}, G = {}, tabs, tabs1;

    function fail(e) { return SE.dbError(e); }
    function msg(t) { return SE.alert(t); }
    function typeId() { return +cb.type.value() || 0; }
    function field(id) { return ($(id).value || '').trim(); }
    function enable(c, on) { c.el.disabled = !on; var w = c.el.__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function ci(r, k) { if (!r) return undefined; if (k in r) return r[k]; var l = k.toLowerCase(); for (var x in r) if (x.toLowerCase() === l) return r[x]; return undefined; }
    function has(rows, key, v) { return rows.some(function (r) { return +r[key] === +v; }); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.type = XCombo('CmbType', { columns: [{ key: 'type', caption: 'Typ' }], textKey: 'type', onSelect: function () { warehouseToShowHide(); typeLeave(); } });
        cb.req = XCombo('CmbRequested', { columns: [{ key: 'LookupName', caption: 'Requested By' }], textKey: 'LookupName' });
        cb.term = XCombo('CmbDeliveryTerm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], textKey: 'DeliveryTerm' });
        cb.cust = XCombo('CmbCustomer', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420, onSelect: customerLeave });
        cb.trans = XCombo('CmbTransporter', { columns: [{ key: 'CompanyName', caption: 'Transporter Name' }], textKey: 'CompanyName', popupWidth: 380 });
        cb.order = XCombo('CmbOrderNo', { columns: [{ key: 'SaleOrderNo', caption: 'Order No' }], valueKey: 'SaleOrderId', textKey: 'SaleOrderNo', onSelect: function () { itemsByOrder(); } });
        cb.item = XCombo('cmbItem', { columns: [{ key: '_t', caption: 'Item Name' }, { key: '_o', caption: 'Item Code' }], textKey: '_t', popupWidth: 500, onSelect: itemLeave });
        cb.wh = XCombo('CmbWarehouse', { columns: [{ key: 'WareHouseName', caption: 'WareHouseName' }], textKey: 'WareHouseName' });
        cb.whTo = XCombo('CmbWareHouseTo', { columns: [{ key: 'WareHouseName', caption: 'WareHouseTo Name' }], textKey: 'WareHouseName' });
        cb.job = XCombo('CmbJobLot', { columns: [{ key: 'JobLotDescription', caption: 'JobLot Description' }], textKey: 'JobLotDescription' });
        cb.uom = XCombo('cmbUOM', { columns: [{ key: 'UOMCode', caption: 'Pack UOM' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode' });
        cb.asset = XCombo('cmbAssetsRef', { columns: [{ key: 'AssetName', caption: 'Fixed Asset Name' }], textKey: 'AssetName', popupWidth: 360 });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Name', caption: 'Customer' }], textKey: 'Name', popupWidth: 380 });
        cb.hReq = XCombo('CmbRequestedByHistory', { columns: [{ key: 'Name', caption: 'Requested By' }], textKey: 'Name' });
    }

    /* CustomerFilll (skipped when the type is Sale Order) */
    function fillCustomers(rows) {
        if (typeId() === 1) return;
        S.sup = rows || S.sup;
        var keep = +cb.cust.value() || 0;
        cb.cust.setData(S.sup);
        if (keep > 0 && !cb.cust.setValue(keep)) cb.cust.clear();
    }
    /* TransporterFill */
    function fillTransporters(rows) {
        S.trans = rows || S.trans;
        var keep = +cb.trans.value() || 0, key = S.subsidiary ? 'CompanyName' : 'AccountTitle';
        cb.trans.setData(S.trans.map(function (r) { return { Id: r.Id, CompanyName: r[key], SupplierCustomerId: r.SupplierCustomerId }; }));
        if (keep > 0 && !cb.trans.setValue(keep)) cb.trans.clear();
    }
    function fillLookups(rows) {
        var k = +cb.req.value() || 0;
        cb.req.setData(rows);
        if (k > 0 && !cb.req.setValue(k)) cb.req.clear();
    }
    function fillAssets(rows) {
        S.asset = rows || S.asset;
        var k = +cb.asset.value() || 0;
        cb.asset.setData(S.asset);
        if (k > 0 && !cb.asset.setValue(k)) cb.asset.clear();
    }
    /* WarehouseFill (desktop quirk kept: the To combo is restored with the From id) */
    function fillWarehouses(rows) {
        S.wh = rows || S.wh;
        var a = +cb.wh.value() || 0, b = +cb.whTo.value() || 0;
        cb.wh.setData(S.wh); cb.whTo.setData(S.wh);
        if (a > 0 && !cb.wh.setValue(a)) cb.wh.clear();
        if (b > 0 && !cb.whTo.setValue(a)) cb.whTo.clear();
    }
    /* binds dtitem to the item combo; byName follows the Name / Code radio */
    function bindItems(byName) {
        cb.item.setData(S.items.map(function (r) { return { Id: r.Id, _t: byName ? r.ItemName : r.ItemCode, _o: byName ? r.ItemCode : r.ItemName, ItemName: r.ItemName, ItemCode: r.ItemCode, SaleOrderDetailId: r.SaleOrderDetailId, BalQty: r.BalQty }; }));
    }
    /* ItemDetailFillWithoutOrderId (load only; skipped for Sale Order) */
    function fillItemsWithoutOrder(rows) {
        if (typeId() === 1) return;
        var keep = +cb.item.value() || 0;
        cb.item.clear();
        S.items = S.items.concat(rows || []);                 // the desktop appends to dtitem
        if (S.items.length) { bindItems($('rdbtnItemName').checked); if (keep > 0 && !cb.item.setValue(keep)) cb.item.clear(); }
        else cb.item.setData([]);
    }

    /* ------------------------------------------------------------------ type / customer / order */
    function warehouseToShowHide() {                          // WareHouseToShowHide
        var t = typeId(), cols = G.grd.spec.cols;
        function col(k) { return cols.filter(function (c) { return c.k === k; })[0]; }
        if (cb.type.row() && t > 0 && t === 5) {
            $('labelWarFrom').textContent = 'Warehouse From'; $('labelWarTo').style.display = ''; cb.whTo.el.style.display = '';
            col('WareHouseId').t = 'WareHouse From'; col('WareHouseToId').hide = false; $('cmbAssetsRef').style.width = '262px';
        } else {
            $('labelWarFrom').textContent = 'Warehouse'; $('labelWarTo').style.display = 'none'; cb.whTo.el.style.display = 'none';
            col('WareHouseId').t = 'WareHouse'; col('WareHouseToId').hide = true; $('cmbAssetsRef').style.width = '458px';
        }
        G.grd.refresh();
    }
    function firstPerKey(rows, key) {
        var seen = {}, out = [];
        rows.forEach(function (r) { var k = String(r[key]); if (!seen[k]) { seen[k] = 1; out.push(r); } });
        return out;
    }
    /* GetOutStandingParties (type 1) */
    function outstandingParties() {
        var keep = +cb.cust.value() || 0;
        if (S.out.length > 0) cb.cust.setData(firstPerKey(S.out, 'Id'));
        else { cb.cust.clear(); cb.cust.setData([]); cb.order.clear(); cb.order.setData([]); }
        if (keep > 0 && !cb.cust.setValue(keep)) cb.cust.clear();
    }
    /* CmbType_Leave */
    function typeLeave() {
        if (typeId() === 1) { outstandingParties(); enable(cb.order, true); }
        else {
            cb.order.clear(); cb.order.setData([]);
            var c = +cb.cust.value() || 0;
            cb.cust.setData(S.sup);
            if (c > 0 && !cb.cust.setValue(c)) cb.cust.clear();
            var it = +cb.item.value() || 0;
            cb.item.clear();
            bindItems(true);                                    // DDL.BindDDL(dtitem, cmbItem, "Id", "ItemName", ...)
            if (it > 0 && !cb.item.setValue(it)) cb.item.clear();
        }
        warehouseToShowHide();
    }
    /* OrderNoBind (CmbCustomer_Leave) */
    function customerLeave() {
        if (typeId() === 1) {
            var c = +cb.cust.value() || 0;
            var rows = firstPerKey(S.out.filter(function (r) { return +r.Id === c; }), 'Id');
            if (rows.length) cb.order.setData(rows); else { cb.order.clear(); cb.order.setData([]); }
        } else { cb.order.clear(); cb.order.setData([]); }
    }
    /* ItemBindbyOrderId (CmbOrderNo_Leave) */
    function itemsByOrder() {
        if (typeId() !== 1) return;
        var keep = +cb.item.value() || 0, o = +cb.order.value() || 0;
        var rows = S.out.filter(function (r) { return +r.SaleOrderId === o; });
        if (rows.length > 0) {
            S.items = rows.map(function (r) { return { Id: r.ItemId, ItemName: r.ItemName, ItemCode: r.ItemCode, SaleOrderDetailId: r.SaleOrderDetailId, BalQty: r.BalQty }; });
            bindItems($('rdbtnItemName').checked);
            if (keep > 0 && !cb.item.setValue(keep)) cb.item.clear();
        } else { cb.item.clear(); cb.item.setData([]); }
    }
    /* ItemUomFill (cmbItem_Leave) */
    function itemLeave() { return bindUoms(+cb.item.value() || 0); }
    function bindUoms(itemId) {
        var prev = cb.uom.text().trim();
        if (isShown('btnsave') && !$('btnsave').disabled) cb.uom.clear();
        return SE.api(API + '/uoms' + SE.q({ itemId: itemId })).then(function (rows) {
            if (rows.length) cb.uom.setData(rows);
            if (prev !== '') {
                var hit = rows.filter(function (r) { return r.UOMCode === prev; })[0];
                if (hit) cb.uom.setValue(hit.Id); else cb.uom.clear();
            }
        }).catch(fail);
    }
    /* GetOutStandingOrderAndParties(recId) */
    function loadOutstanding(recId) {
        return SE.api(API + '/outstanding' + SE.q({ recId: recId || 0 })).then(function (rows) { S.out = rows; }).catch(fail);
    }
    function rdbChanged() {                                   // rdbtnItemName_CheckedChanged
        if (!S.items.length) return;
        var id = +cb.item.value() || 0;
        bindItems($('rdbtnItemName').checked);
        if (id > 0) { cb.item.setValue(id); cb.item.focus(); }
    }

    /* ------------------------------------------------------------------ grids */
    function makeGrids() {
        G.grd = SE.grid('grdDetail', { footer: false, frozen: 2, onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) { if (k === 'Add') addCopy(i); else if (k === 'Delete') deleteRow(i); },
            onEdit: function (r, k, v) {
                if (k === 'ItemQty') { var n = Number(String(v).replace(/,/g, '')); if (isFinite(n)) r[k] = n; }
                else if (/Id$/.test(k)) r[k] = v === '' ? 0 : +v;
                else r[k] = v;
            },
            cols: [
                { k: 'Add', t: '+', w: 28, btn: '+' }, { k: 'Delete', t: 'X', w: 28, btn: 'X' },
                { k: 'Id', hide: true }, { k: 'DeliveryOrderId', hide: true }, { k: 'DeliveryOrderDetailId', hide: true }, { k: 'DeliveryOrderNo', t: 'DeliveryOrderNo', w: 100 },
                { k: 'OrderId', hide: true }, { k: 'OrderDetailId', hide: true },
                { k: 'WareHouseId', t: 'WareHouseName', w: 150, edit: true, list: function () { return S.wh; }, lk: 'Id', lt: 'WareHouseName' },
                { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemDescription', t: 'ItemDescription', w: 200 },
                { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 70 }, { k: 'Remarks', t: 'Specification/Remarks', w: 200, edit: true },
                { k: 'JobLotId', t: 'Job Lot', w: 150, edit: true, list: function () { return S.job; }, lk: 'Id', lt: 'JobLotDescription' },
                { k: 'ItemQty', t: 'ItemQty', w: 90, f: 'n3', edit: true },
                { k: 'AssetId', t: 'Machine/Asset', w: 150, edit: true, list: function () { return S.asset; }, lk: 'Id', lt: 'AssetName' },
                { k: 'DeliveryTypeId', hide: true },
                { k: 'WareHouseToId', t: 'WareHouse To', w: 150, edit: true, hide: true, list: function () { return S.wh; }, lk: 'Id', lt: 'WareHouseName' }] });
        G.gp = SE.grid('grdGp', { footer: true, frozen: 2, onBtn: function (k, r) { if (k === 'Load') loadGatePass(r); },
            cols: [
                { k: 'Load', t: 'Load', w: 40, btn: 'Load' },
                { k: 'OrderType', t: 'OrderType', w: 90 }, { k: 'GatepassType', hide: true }, { k: 'OutwardGatePassId', hide: true }, { k: 'DeliveryTypeId', hide: true },
                { k: 'DeliveryType', t: 'DeliveryType', w: 100 }, { k: 'DeliverOrderId', hide: true }, { k: 'SaleOrderId', hide: true }, { k: 'SupplierCustomerId', hide: true },
                { k: 'CustomerName', t: 'CustomerName', w: 220 }, { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'GpDate', t: 'GpDate', w: 85, f: 'sdate' }, { k: 'GpSrNo', t: 'GpSrNo', w: 60 },
                { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'BiltyNo', t: 'BiltyNo', w: 80 }, { k: 'VarietyName', t: 'VarietyName', w: 140 },
                { k: 'Qty', t: 'Qty', w: 80, f: 'n3', sum: true }, { k: 'Freight', t: 'Freight', w: 80, f: 'n3', sum: true }, { k: 'Status', t: 'Status', w: 80 },
                { k: 'RequestedBy', t: 'RequestedBy', w: 110 }, { k: 'ApprovedBy', t: 'ApprovedBy', w: 110 }] });
        G.exp = SE.grid('GridExpense', { footer: true, frozen: 2,
            onBtn: function (k, r, i) { if (k === 'Add') addExpenseRow(); else if (k === 'Delete') deleteExpenseRow(i); },
            onEdit: function (r, k, v) {
                if (k === 'Qty') { var n = Number(String(v).replace(/,/g, '')); r[k] = isFinite(n) ? n : 0; }
                else if (k === 'ItemId') r[k] = v === '' ? '' : +v;
                else r[k] = v;
            },
            cols: [{ k: 'Add', t: '+', w: 28, btn: '+' }, { k: 'Delete', t: 'X', w: 28, btn: 'X' },
                { k: 'ItemId', t: 'Item', w: 250, edit: true, list: function () { return S.other; }, lk: 'Id', lt: 'OtherItemName' },
                { k: 'Qty', t: 'Qty', w: 100, f: 'n3', sum: true, edit: true }, { k: 'Remarks', t: 'Remarks', w: 300, edit: true }] });
        G.hist = SE.grid('DataGridHistory', { frozen: 2, onDbl: function (r) { edit(+r.Id); },
            onBtn: function (k, r) { if (k === 'Edit') edit(+r.Id); else if (k === 'Print') print(+r.Id); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(r.Id); },
            onSel: histSelected,
            cols: [
                { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 50, btn: 'Print' }, { k: 'Id', hide: true },
                { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' }, { k: 'Type', t: 'Type', w: 100 }, { k: 'Customer', t: 'CustomerName', w: 220 },
                { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'InvoiceNo', t: 'InvoiceNo', w: 70 }, { k: 'RequestedBy', t: 'RequestedBy', w: 110 }, { k: 'ManualNo', t: 'ManualNo', w: 90 },
                { k: 'EntryUser', t: 'EntryUser', w: 100 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'ModifyUser', t: 'ModifyUser', w: 100 },
                { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ApprovedUser', t: 'ApprovedUser', w: 100 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' },
                { k: 'Remarks', t: 'Remarks', w: 200 }, { k: 'GdnStatus', t: 'GdnStatus', w: 90 }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }] });
        G.hd = SE.grid('grddetailofmain', {
            cols: [{ k: 'Warehouse', t: 'WareHouse', w: 150 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemDescription', t: 'ItemDescription', w: 200 },
                { k: 'Specification/Remarks', t: 'Specification/Remarks', w: 200 }, { k: 'JobLot', t: 'JobLot', w: 120 }, { k: 'UOM', t: 'UOM', w: 70 },
                { k: 'ItemQty', t: 'ItemQty', w: 90, f: 'n2', sum: true }, { k: 'AssetName', t: 'AssetName', w: 150 }, { k: 'WarehouseTo', t: 'WarehouseTo', w: 150, hide: true }] });
    }
    /* DetailGridSettings: the "+" column and the editable quantity depend on OrderStatus */
    function detailGridSettings() {
        var cols = G.grd.spec.cols, dOrder = S.orderStatus === 'DeliverOrder';
        cols[0].hide = dOrder;
        cols.filter(function (c) { return c.k === 'ItemQty'; })[0].edit = !dOrder;
        warehouseToShowHide();
    }
    function addExpenseRow() { G.exp.addRow({ ItemId: 0, Qty: 0, Remarks: '' }); }
    function deleteExpenseRow(i) {
        if (!G.exp.rows()[i]) return;
        G.exp.removeAt(i);
        if (G.exp.count() === 0) addExpenseRow();
    }
    function resetExpenses() { G.exp.setRows([]); addExpenseRow(); }

    /* DataGridHistory_SelectionChanged */
    function histSelected(item) {
        if (!item) return;
        return SE.api(API + '/' + item.Id + '/details', { quiet: true }).then(function (rows) {
            if (!rows || !rows.length) return;
            var transfer = String(item.Type) === 'Transfer', cols = G.hd.spec.cols;
            cols[0].t = transfer ? 'WareHouse From' : 'WareHouse';
            cols[cols.length - 1].hide = !transfer;
            G.hd.setRows(rows.map(function (r) {
                return { Warehouse: r.WareHouseCode, ItemCode: r.ItemCode, ItemName: r.Item, ItemDescription: r.ItemDescription, 'Specification/Remarks': r.CommentsDetail,
                    JobLot: r.JobLot, UOM: r.UOMCode, ItemQty: r.ItemQty, AssetName: r.AssetName, WarehouseTo: r.WareHouseToCode };
            }));
        }).catch(function () { /* the desktop shows the exception text only */ });
    }

    /* ------------------------------------------------------------------ detail entry */
    function detailValid() {                                  // FormValidationDetail
        function stop(m, f) { msg(m); if (f) f(); return false; }
        var t = typeId();
        if (t === 0) return stop(' Please Select Type Field First', function () { cb.type.focus(); });
        if (!(+cb.cust.value() > 0)) return stop(' Please Select Customer Field First', function () { cb.cust.focus(); });
        if (t === 1 && !(+cb.order.value() > 0)) return stop('Order No Field is required', function () { cb.order.focus(); });
        if (!cb.wh.row() || !(+cb.wh.value())) return stop('Warehouse Field Required', function () { cb.wh.focus(); });
        if (!cb.item.row() || !(+cb.item.value())) return stop('Item Field Required', function () { cb.item.focus(); });
        if (!cb.uom.row() || !(+cb.uom.value())) return stop('UOM Field Required', function () { cb.uom.focus(); });
        if (t === 3 && (!cb.asset.row() || !(+cb.asset.value()))) return stop('Machine / Product Field Required', function () { cb.asset.focus(); });
        if (t === 5 && (!cb.whTo.row() || !(+cb.whTo.value()))) return stop('WareHouseTo Field Required', function () { cb.whTo.focus(); });
        if (SE.toNum($('txtItemQty').value.trim()) === 0) return stop('ItemQty Field Required', function () { $('txtItemQty').focus(); });
        return true;
    }
    function resetDetail() {                                  // ResetDetail
        cb.item.clear(); cb.uom.clear(); $('txtItemQty').value = ''; cb.asset.clear(); $('txtRemarksDetail').value = '';
        S.updateIdx = -1;
        show('btnUpdateDetail', false); show('btnCancelDetail', false); show('Add', true);
        cb.wh.focus();
    }
    function plus() {                                         // Add_Click
        if (!detailValid()) return;
        var t = typeId(), rows = G.grd.rows(), ir = cb.item.row(), orderOn = t === 1 && (+cb.order.value() > 0);
        if (rows.length > 0 && (+rows[0].OrderId || 0) > 0) enable(cb.order, false);
        G.grd.addRow({ Id: 0, DeliveryOrderId: 0, DeliveryOrderDetailId: 0, DeliveryOrderNo: 0, OrderId: orderOn ? +cb.order.value() : 0, OrderDetailId: orderOn ? (+ir.SaleOrderDetailId || 0) : 0,
            WareHouseId: +cb.wh.value(), ItemId: +cb.item.value(), ItemCode: ir.ItemCode, ItemName: ir.ItemName, ItemDescription: $('txtItemDescriptions').value,
            PackUomId: +cb.uom.value(), PackUom: cb.uom.text().trim(), Remarks: $('txtRemarksDetail').value.trim(), JobLotId: +cb.job.value(),
            ItemQty: SE.toNum($('txtItemQty').value.trim()), AssetId: +cb.asset.value() || 0, DeliveryTypeId: t, WareHouseToId: +cb.whTo.value() || 0 });
        resetDetail();
        if (t === 1) { enable(cb.type, false); enable(cb.cust, false); enable(cb.order, false); }
    }
    function editRow(i) {                                     // grdDetail_DoubleClick
        if (S.orderStatus === 'DeliverOrder') return;
        var r = G.grd.rows()[i]; if (!r || (+r.DeliveryOrderId || 0) !== 0) return;
        S.updateIdx = i;
        cb.wh.setValue(r.WareHouseId);
        cb.item.setValue(+r.ItemId);
        bindUoms(+r.ItemId).then(function () {
            cb.uom.setValue(r.PackUomId);
            if (r.PackUom) { /* cmbUOM.Text = PackUom */ }
        });
        $('txtItemDescriptions').value = r.ItemDescription == null ? '' : r.ItemDescription;
        cb.job.setValue(r.JobLotId);
        $('txtItemQty').value = r.ItemQty == null ? '' : SE.num(r.ItemQty, 3);
        cb.asset.setValue(+r.AssetId || 0);
        cb.whTo.setValue(+r.WareHouseToId || 0);
        $('txtRemarksDetail').value = r.Remarks == null ? '' : r.Remarks;
        show('Add', false); show('btnUpdateDetail', true); show('btnCancelDetail', true);
    }
    function updateDetail() {                                 // btnUpdateDetail_Click
        if (!detailValid()) return;
        var r = G.grd.rows()[S.updateIdx]; if (!r) return;
        var ir = cb.item.row();
        if (typeId() === 1 && +cb.order.value() > 0) { r.OrderId = +cb.order.value(); r.OrderDetailId = +ir.SaleOrderDetailId || 0; }
        r.WareHouseId = +cb.wh.value(); r.ItemId = +cb.item.value(); r.ItemName = ir.ItemName; r.ItemCode = ir.ItemCode;
        r.JobLotId = +cb.job.value(); r.PackUomId = +cb.uom.value(); r.PackUom = cb.uom.text(); r.ItemQty = SE.toNum($('txtItemQty').value.trim());
        r.AssetId = +cb.asset.value() || 0; r.WareHouseToId = +cb.whTo.value() || 0; r.Remarks = $('txtRemarksDetail').value;
        G.grd.refresh();
        resetDetail();
    }
    function cancelDetail() { show('Add', true); show('btnUpdateDetail', false); show('btnCancelDetail', false); resetDetail(); }
    function addCopy(i) {                                     // grdDetail_ColumnButtonClick "Add"
        var r = G.grd.rows()[i]; if (!r) return;
        var c = JSON.parse(JSON.stringify(r));
        if (isShown('btnUpdate') && !$('btnUpdate').disabled) c.Id = 0;
        G.grd.addRow(c);
    }
    function deleteRow(i) {                                   // "Delete"
        if (!G.grd.rows()[i]) return;
        G.grd.removeAt(i);
        if (G.grd.count() === 0) { enable(cb.type, true); enable(cb.order, true); enable(cb.cust, true); }
    }

    /* ------------------------------------------------------------------ pending gate passes (grdGp_ColumnButtonClick) */
    function loadGatePass(item) {
        if (!(isShown('btnsave') && !$('btnsave').disabled)) return;
        var p = G.grd.count() > 0 ? SE.ask('Are you sure to Load? Because detail entries will lost', 'Confirm') : Promise.resolve(true);
        return p.then(function (yes) {
            if (!yes || !item) return;
            S.orderStatus = String(item.OrderType == null ? '' : item.OrderType);
            var pre = S.orderStatus === 'DeliverOrder' ? loadOutstanding(+item.DeliverOrderId || 0) : Promise.resolve();
            return pre.then(function () {
                if (String(item.Status) !== 'Accepted') throw new Error('Status Not Accepted Please check status');
                $('txtGpNo').disabled = true; $('txtVehicleNo').disabled = true; $('txtBiltyNo').disabled = true;
                var dId = +item.DeliveryTypeId || 0;
                if (dId > 0) {
                    if (dId === 1) { enable(cb.type, false); enable(cb.cust, false); enable(cb.order, false); }
                    else { enable(cb.order, false); enable(cb.type, true); enable(cb.cust, true); }
                    cb.type.setValue(dId);
                } else cb.type.clear();
                warehouseToShowHide(); typeLeave();
                S.gpId = +item.OutwardGatePassId || 0;
                S.saleOrderId = +item.SaleOrderId || 0;
                $('txtGpNo').value = item.GpSrNo == null ? '' : String(item.GpSrNo);
                $('txtFreightAmount').value = SE.num(item.Freight, 4);
                $('txtVehicleNo').value = item.VehicleNo == null ? '' : item.VehicleNo;
                $('txtBiltyNo').value = item.BiltyNo == null ? '' : item.BiltyNo;
                cb.cust.setValue(+item.SupplierCustomerId || 0); customerLeave();
                if (S.saleOrderId > 0) { cb.order.setValue(S.saleOrderId); itemsByOrder(); }
                if (S.orderStatus === 'DeliverOrder') {
                    G.grd.setRows([]);
                    show('panel13', false);
                    return SE.api(API + '/do-data' + SE.q({ customerId: +cb.cust.value() || 0, deliveryOrderId: +item.DeliverOrderId || 0, deliveryTypeId: +item.DeliveryTypeId || 0 })).then(function (dt) {
                        if (dt.length) {
                            dt.forEach(function (r) {
                                G.grd.rows().push({ Id: 0, DeliveryOrderId: 0, DeliveryOrderDetailId: 0, DeliveryOrderNo: 0, OrderId: ci(r, 'OrderId'), OrderDetailId: ci(r, 'SaleOrderDetailId'),
                                    WareHouseId: ci(r, 'WareHouse'), ItemId: ci(r, 'ItemId'), ItemCode: ci(r, 'ItemCode'), ItemName: ci(r, 'Item'), ItemDescription: ci(r, 'ItemDescription'),
                                    PackUomId: ci(r, 'UOMId'), PackUom: ci(r, 'UOM'), Remarks: '', JobLotId: ci(r, 'JobId'), ItemQty: ci(r, 'Qty'), AssetId: 0, DeliveryTypeId: +item.DeliveryTypeId || 0, WareHouseToId: null });
                            });
                            G.grd.refresh(); detailGridSettings();
                        } else G.grd.setRows([]);
                    });
                } else show('panel13', true);
            });
        }).catch(function (e) { return SE.alert(e.message); });
    }

    /* ------------------------------------------------------------------ Load (frmGSIssuance_Load) */
    function applyInitial(d) {
        S.rights = d.rights || {}; S.subsidiary = !!d.subsidiary;
        S.wh = d.warehouses || []; S.job = d.jobLots || []; S.asset = d.assets || []; S.other = d.otherItems || [];
        S.out = d.outstanding || [];
        fillWarehouses(S.wh);
        cb.job.setData(S.job);
        fillAssets(S.asset);
        cb.type.setData(d.deliveryTypes);
        cb.term.setData([{ Id: 1, DeliveryTerm: 'Load' }, { Id: 2, DeliveryTerm: 'Ponch' }]);
        cb.term.setValue(1);
        S.sup = d.customers || [];
        fillCustomers(S.sup);
        fillTransporters(d.transporters || []);
        fillLookups(d.lookups || []);
        S.items = [];
        fillItemsWithoutOrder(d.items || []);
        G.gp.setRows(d.gatePasses || []);
        $('txtDocNo').value = d.nextNo > 0 ? String(d.nextNo) : $('txtDocNo').value;
        var h = d.history || {};
        cb.hCust.setData(h.customers || []); cb.hReq.setData(h.requestedBy || []);
        if (d.defaultJobLot > 0) cb.job.setValue(d.defaultJobLot);
        if (d.defaultWarehouse > 0) cb.wh.setValue(d.defaultWarehouse);
        resetExpenses();
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControlGsIssuance', function (id) { if (id === 'tabPage2') $('FromDateHistory').focus(); });
        tabs1 = SE.tabs('tabControl1');
        SE.digitsOnly($('FromDocNoHistory')); SE.digitsOnly($('ToDocNoHistory')); SE.digitsOnly($('txtGpNo'));
        $('txtItemQty').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        $('txtFreightAmount').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        makeGrids();
        SE.api(API + '/initial').then(function (d) {
            applyInitial(d);
            $('btnsave').disabled = !S.rights.save; $('Print').disabled = !S.rights.print; $('btnUpdate').disabled = !S.rights.update; $('btnDelete').disabled = !S.rights.delete;
            show('btnUpdate', false); show('btnsave', true);
            $('txtDocdate').value = SE.today(); $('datReturnableDate').value = SE.today();
            $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today();
            detailGridSettings();
            $('txtDocNo').focus();
            var rec = SE.param('record'); if (rec) edit(+rec);
        }).catch(fail);
        wire();
    }

    /* ------------------------------------------------------------------ formReset */
    function reset() {
        S.files = []; S.removedAtt = []; S.existing = [];
        show('panel13', true);
        S.saleOrderId = 0; S.gpId = 0; S.orderStatus = ''; S.id = 0;
        show('btnsave', true); show('btnUpdate', false); show('btnDelete', false);
        enable(cb.type, true); enable(cb.order, true); enable(cb.cust, true);
        $('txtGpNo').disabled = false; $('txtVehicleNo').disabled = false; $('txtBiltyNo').disabled = false;
        var rows = cb.type.rows();
        if (rows.length) cb.type.setValue(rows[0].Id); else cb.type.clear();
        typeLeave();
        cb.cust.clear(); cb.order.clear(); cb.order.setData([]);
        $('txtGpNo').value = ''; $('txtVehicleNo').value = ''; $('txtBiltyNo').value = '';
        cb.req.clear(); $('txtManualNo').value = ''; $('txtRemarks').value = '';
        resetDetail();
        G.grd.setRows([]); detailGridSettings();
        resetExpenses();
        return SE.api(API + '/reset-data').then(function (d) {
            if (d.nextNo > 0) $('txtDocNo').value = String(d.nextNo);
            G.gp.setRows(d.gatePasses || []);
            $('txtDocdate').focus();
        }).catch(fail);
    }
    /* btnRefresh_Click */
    function refresh() {
        return SE.api(API + '/refresh').then(function (d) {
            fillWarehouses(d.warehouses); fillAssets(d.assets); fillLookups(d.lookups || []); fillTransporters(d.transporters || []);
            fillCustomers(d.customers || []);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Insert */
    function formValid() {                                    // FormValidation
        function stop(m, f) { return SE.alert(m).then(function () { if (f) f(); return false; }); }
        var no = field('txtDocNo');
        if (no === '' || SE.toInt(no) === 0) return stop('DocNo Field Required', function () { $('txtDocNo').focus(); });
        if (!(+cb.term.value() > 0)) return stop('Delivery Term Field Required', function () { cb.term.focus(); });
        if (typeId() <= 0) return stop('Type Field Required', function () { cb.type.focus(); });
        if (!(+cb.cust.value() > 0)) return stop('Customer Field Required', function () { cb.cust.focus(); });
        if (!(+cb.req.value() > 0)) return stop('RequestedBy Field Required', function () { cb.req.focus(); });
        var f = SE.toNum($('txtFreightAmount').value);
        if (+cb.trans.value() > 0 && !(f > 0)) return stop('Freight Amount Field is Required', function () { $('txtFreightAmount').focus(); });
        if (f > 0 && !(+cb.trans.value() > 0)) return stop('Transporter Field is Required', function () { cb.trans.focus(); });
        return Promise.resolve(true);
    }
    function insert() {
        return formValid().then(function (ok) {
            if (!ok) return;
            return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                if (G.grd.count() === 0) return SE.alert('Grid record not found', 'Message');
                var body = {
                    id: S.id, docDate: $('txtDocdate').value, docNo: field('txtDocNo'), deliveryTermId: +cb.term.value() || 0, deliveryTypeId: typeId(), customerId: +cb.cust.value() || 0,
                    requestedById: +cb.req.value() || 0, manualNo: field('txtManualNo'), transporterId: +cb.trans.value() || 0, freight: field('txtFreightAmount'),
                    returnableDate: $('datReturnableDate').value, remarks: $('txtRemarks').value, gatePassId: S.gpId, gpNo: field('txtGpNo'), vehicleNo: $('txtVehicleNo').value, biltyNo: field('txtBiltyNo'),
                    lines: G.grd.rows().map(lineOf),
                    expenses: G.exp.rows().map(function (r) { return { itemId: +r.ItemId || 0, qty: SE.toNum(r.Qty), remarks: r.Remarks == null ? '' : String(r.Remarks) }; }),
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt }
                };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return SE.alert(r.message).then(function () {
                        var preview = $('ChkBox').checked;
                        return reset().then(function () { return loadOutstanding(0); }).then(function () { if (preview) print(r.id); });
                    });
                }).catch(function (e) { return SE.alert(e.message, 'Message'); });
            });
        });
    }
    function lineOf(r) {
        return { deliveryOrderId: +r.DeliveryOrderId || 0, deliveryOrderDetailId: +r.DeliveryOrderDetailId || 0, deliveryOrderNo: +r.DeliveryOrderNo || 0, orderId: +r.OrderId || 0, orderDetailId: +r.OrderDetailId || 0,
            wareHouseId: +r.WareHouseId || 0, itemId: +r.ItemId || 0, itemDescription: r.ItemDescription == null ? '' : String(r.ItemDescription), packUomId: +r.PackUomId || 0,
            remarks: r.Remarks == null ? '' : String(r.Remarks), jobLotId: +r.JobLotId || 0, itemQty: SE.toNum(r.ItemQty), assetId: +r.AssetId || 0, wareHouseToId: +r.WareHouseToId || 0 };
    }
    function btnSave() { S.id = 0; return insert(); }
    function btnUpdate() { return insert(); }

    /* ------------------------------------------------------------------ ReadById */
    function edit(id) {
        S.id = id;
        return loadOutstanding(id).then(function () { return SE.api(API + '/' + id); }).then(function (d) {
            var h = d.head, lines = d.lines || [];
            tabs.select('tabPage1');
            show('btnsave', false); show('btnUpdate', true); show('btnDelete', true);
            $('txtDocNo').value = h.DocNo; $('txtDocdate').value = SE.dateInput(h.DocDate);
            if (lines.length > 0) {
                cb.type.setValue(+lines[0].DeliveryTypeId); warehouseToShowHide(); typeLeave();
                if (+lines[0].DeliveryTypeId === 1) { enable(cb.type, false); enable(cb.cust, false); enable(cb.order, false); }
                else { enable(cb.type, true); enable(cb.cust, true); enable(cb.order, true); }
                cb.cust.setValue(+h.SupplierCustomerId); customerLeave();
                if ((+lines[0].SaleOrderId || 0) > 0) { cb.order.setValue(+lines[0].SaleOrderId); itemsByOrder(); }
            }
            var t = String(h.DeliveryTerm == null ? '' : h.DeliveryTerm), tr = cb.term.rows().filter(function (x) { return x.DeliveryTerm === t; })[0];
            if (tr) cb.term.setValue(tr.Id); else cb.term.clear();
            cb.req.setValue(+h.RequestedByLookUpId);
            $('txtManualNo').value = h.ReferenceDocNo || '';
            $('txtGpNo').value = String(+h.GpNo || 0);
            $('txtVehicleNo').value = h.VehicleNo || '';
            $('txtBiltyNo').value = h.BiltyNo || '';
            S.gpId = +h.OutwardGatePassId || 0;
            if (S.gpId > 0) { $('txtGpNo').disabled = true; $('txtVehicleNo').disabled = true; $('txtBiltyNo').disabled = true; }
            S.orderStatus = String(h.OtherSupCust == null ? '' : h.OtherSupCust);
            if (S.orderStatus === 'DeliverOrder') show('panel13', false);
            var rd = SE.dateInput(h.ReturnableDate); if (rd) $('datReturnableDate').value = rd;
            if (+h.TransporterId > 0) cb.trans.setValue(S.subsidiary ? +h.TransporterSupCustId : +h.TransporterId);
            $('txtFreightAmount').value = SE.num(h.CarriageAmount, 3);
            $('txtRemarks').value = h.RemarksHeader || '';
            G.grd.setRows(lines.map(function (l) {
                return { Id: l.Id, DeliveryOrderId: l.InvDeliveryOrderId, DeliveryOrderDetailId: l.InvDeliveryOrderDetailId, DeliveryOrderNo: l.DeliveryScheduleNo, OrderId: l.SaleOrderId, OrderDetailId: l.SaleOrderDetailId,
                    WareHouseId: l.WarehouseId, ItemId: l.ItemId, ItemCode: l.ItemCode, ItemName: l.Item, ItemDescription: l.ItemDescription, PackUomId: l.ItemUomId, PackUom: l.UOMCode,
                    Remarks: l.CommentsDetail, JobLotId: l.JobLotId, ItemQty: l.ItemQty, AssetId: l.AssetId, DeliveryTypeId: l.DeliveryTypeId, WareHouseToId: l.WareHouseToId };
            }));
            detailGridSettings();
            var ex = d.expenses || [];
            G.exp.setRows(ex.map(function (e) { return { ItemId: e.ItemId, Qty: e.Qty, Remarks: e.Remarks }; }));
            if (G.exp.count() === 0) addExpenseRow();
            S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history (HistoryFill) */
    function showHistory() {
        var dt = $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : ($('rdapproveddate').checked ? 'approved' : 'document'));
        if ($('drdocdate').checked) dt = 'document';
        var q = { dateType: dt, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
            fromNo: SE.toInt($('FromDocNoHistory').value), toNo: SE.toInt($('ToDocNoHistory').value), customerId: +cb.hCust.value() || 0, requestedById: +cb.hReq.value() || 0,
            referred: $('RadReffered').checked ? 'referred' : ($('RadNotReffered').checked ? 'notReferred' : 'all') };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            S.histRows = rows;
            if (!rows.length) { G.hist.setRows([]); return; }
            G.hist.setRows(rows.map(function (r) {
                return { Id: ci(r, 'Id'), DocNo: SE.toInt(ci(r, 'DocNo')), DocDate: ci(r, 'DocDate'), Type: ci(r, 'DeliveryType'), Customer: ci(r, 'CustomerName'), OrderNo: SE.toInt(ci(r, 'SaleOrderNo')),
                    InvoiceNo: SE.toInt(ci(r, 'InvoiceNo')), RequestedBy: ci(r, 'RequestedBy'), ManualNo: ci(r, 'ReferenceDocNo'), EntryUser: ci(r, 'UserName'), EntryDate: ci(r, 'EntryDate'),
                    ModifyUser: ci(r, 'ModifyUserName'), ModifyDate: ci(r, 'ModifyDate'), ApprovedUser: ci(r, 'ApprovedUserName'), ApprovedDate: ci(r, 'ApprovedDate'),
                    Remarks: ci(r, 'RemarksHeader'), GdnStatus: ci(r, 'GdnStatus'), NoOfAttachments: SE.toInt(ci(r, 'NoOfAttachments')) };
            }));
        }).catch(function (e) { return SE.alert(e.message); });
    }
    function resetHistory() {                                 // btnResetHistory_Click
        $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today();
        $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hCust.clear(); cb.hReq.clear();
        G.hist.setRows([]); G.hd.setRows([]); S.histRows = [];
        $('drdocdate').checked = true;
    }
    function refreshHistoryCombos() {
        return SE.api(API + '/history-combos').then(function (h) { cb.hCust.setData(h.customers || []); cb.hReq.setData(h.requestedBy || []); }).catch(function (e) { return SE.alert(e.message); });
    }

    /* ------------------------------------------------------------------ print / delete / attachments */
    function print(id) {
        if (!(id > 0)) return SE.alert('No Record Found');
        SE.printRpt('1612-InvGdn_Slip.rpt', { id: id });
    }
    function del() {
        if (S.id > 0) {
            return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
                if (!yes) return;
                return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return SE.alert(r.message).then(reset); })
                    .catch(function (e) { return SE.alert(e.message, 'Database Error'); });
            });
        }
        return SE.alert('Record Not Found', 'Database Error');
    }
    function showAttachments(id) {
        return SE.api(API + '/' + id + '/attachments').then(function (rows) { renderAttachments(rows, id, true); }).catch(fail);
    }
    function openAttachmentDialog() {
        var rows = (S.existing || []).filter(function (r) { return S.removedAtt.indexOf(r.Id) < 0; });
        renderAttachments(rows, S.id, false);
    }
    function renderAttachments(rows, id, readOnly) {
        var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>Attachment</th><th style="width:90px">Action</th></tr></thead><tbody>';
        rows.forEach(function (r) {
            h += '<tr><td>' + SE.esc(r.Attachment) + '</td><td>' + (id > 0 ? '<a class="lnk" href="' + API + '/' + id + '/attachments/' + r.Id + '" target="_blank">Open</a>' : '') +
                 (readOnly ? '' : ' <a class="lnk" data-rm="' + r.Id + '" style="cursor:pointer">Remove</a>') + '</td></tr>';
        });
        S.files.forEach(function (f, i) { if (!readOnly) h += '<tr><td>' + SE.esc(f.name) + ' (new)</td><td><a class="lnk" data-nf="' + i + '" style="cursor:pointer">Remove</a></td></tr>'; });
        h += '</tbody></table></div>';
        if (!readOnly) h += '<div style="padding:6px"><input type="file" id="atFile" multiple> <span class="se-note">Up to 5 MB each. Files are stored when the note is saved.</span></div>';
        var pop = SE.pop('Attachments', h, [{ t: 'Close' }]);
        pop.open();
        pop.body.addEventListener('click', function (e) {
            var rm = e.target.getAttribute && e.target.getAttribute('data-rm');
            if (rm) { S.removedAtt.push(+rm); pop.close(); openAttachmentDialog(); }
            if (e.target.hasAttribute && e.target.hasAttribute('data-nf')) { S.files.splice(+e.target.getAttribute('data-nf'), 1); pop.close(); openAttachmentDialog(); }
        });
        var fi = pop.body.querySelector('#atFile');
        if (fi) fi.addEventListener('change', function () {
            var list = Array.prototype.slice.call(fi.files), left = list.length;
            if (!left) return;
            list.forEach(function (f) {
                if (f.size > 5 * 1024 * 1024) { SE.alert(f.name + ' exceeds 5 MB'); left--; return; }
                var fr = new FileReader();
                fr.onload = function () { S.files.push({ name: f.name, base64: String(fr.result).split(',')[1] || '' }); if (--left <= 0) { pop.close(); openAttachmentDialog(); } };
                fr.readAsDataURL(f);
            });
        });
    }

    /* ------------------------------------------------------------------ Delivery Order loader (frmLoadDeliveryOrderOnGdn_Engr) */
    function loadDeliveryOrder() {                            // BtnDeliveryOrder_Click
        var t = typeId();
        if (t !== 4 && t !== 8) { cb.type.focus(); return SE.alert("Please select Delivery Type as ['Returnable'] or ['Returned'] to load a Delivery Order."); }
        var rows = G.grd.rows();
        if (rows.length === 0 || (+rows[0].DeliveryOrderId || 0) > 0) return openLoader(t);
        return SE.alert('You cannot load a Delivery Order because a Direct Entry already exists.');
    }
    function openLoader(typeIdValue) {
        var h = '<div class="lbar" style="height:50px"><div class="dts"><button type="button" class="tsi" id="ldNew"><i class="fa fa-plus-circle g"></i>New</button>' +
            '<button type="button" class="tsi" id="ldRefresh"><i class="fa fa-refresh g"></i>Refresh</button><button type="button" class="tsi" id="ldLoad"><i class="fa fa-check b"></i>Load</button>' +
            '<button type="button" class="tsi" id="ldKeys">ShortCut Keys</button></div><span class="dl white" style="left:5px;top:27px">Pending Delivery Order [Returnable / Returned]</span></div>' +
            '<div class="lgrid" id="ldGrd" style="height:327px"></div>' +
            '<div class="lf" style="height:62px">' +
            '<span class="dl" style="left:7px;top:10px">From Date</span><input class="f" type="date" id="ldFrom" style="left:71px;top:6px;width:125px;height:22px">' +
            '<span class="dl" style="left:200px;top:10px">Doc No From</span><input class="f" id="ldNoFrom" style="left:276px;top:6px;width:89px;height:23px">' +
            '<span class="dl" style="left:372px;top:10px">Customer Name</span><select class="f" id="ldCust" style="left:466px;top:4px;width:220px;height:26px"></select>' +
            '<span class="dl" style="left:7px;top:38px">To Date</span><input class="f" type="date" id="ldTo" style="left:71px;top:34px;width:125px;height:22px">' +
            '<span class="dl" style="left:200px;top:38px">Doc No To</span><input class="f" id="ldNoTo" style="left:276px;top:34px;width:89px;height:23px">' +
            '<span class="dl" style="left:372px;top:38px">Item Name</span><select class="f" id="ldItem" style="left:466px;top:32px;width:220px;height:26px"></select>' +
            '<button type="button" class="dbtn" id="ldSearch" style="left:692px;top:32px;width:65px;height:26px">Search</button></div>';
        var lst = [], pop = SE.pop('Pending Delivery Order', h, [{ t: 'Close' }], { wide: true });
        pop.open();
        var $$ = function (id) { return pop.body.querySelector('#' + id); };
        SE.digitsOnly($$('ldNoFrom')); SE.digitsOnly($$('ldNoTo'));
        var cc = XCombo($$('ldCust'), { columns: [{ key: 'Name', caption: 'Supplier Name' }], textKey: 'Name', popupWidth: 380 });
        var ci2 = XCombo($$('ldItem'), { columns: [{ key: 'Name', caption: 'Item Name' }], textKey: 'Name', popupWidth: 380 });
        var cols = [{ k: 'Sel', t: 'Select', w: 40, sel: true }, { k: 'Id', hide: true }, { k: 'DocumentTypeId', hide: true }, { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' },
            { k: 'DeliveryTypeId', hide: true }, { k: 'DeliveryOrderType', t: 'DeliveryOrderType', w: 110 }, { k: 'SupplierCustomerId', hide: true }, { k: 'SaleOrderId', hide: true },
            { k: 'SaleOrderDetailId', hide: true }, { k: 'SaleOrderNo', t: 'SaleOrderNo', w: 70 }, { k: 'DetailId', hide: true }, { k: 'WareHouseName', t: 'WareHouseName', w: 140 },
            { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemDescription', t: 'ItemDescription', w: 200 },
            { k: 'CropYear', t: 'CropYear', w: 80 }, { k: 'JobLot', t: 'JobLot', w: 100 }, { k: 'PackingType', t: 'PackingType', w: 100 }, { k: 'PackUomCode', t: 'PackUomCode', w: 80 },
            { k: 'Qty', t: 'Qty', w: 80, f: 'n3' }, { k: 'GrossWight', t: 'GrossWight', w: 80, f: 'n3' }, { k: 'EbUnit', t: 'EbUnit', w: 80, f: 'n3' }, { k: 'EbTotal', t: 'EbTotal', w: 80, f: 'n3' },
            { k: 'NetWeight', t: 'NetWeight', w: 80, f: 'n3' }, { k: 'SaleOrderCityName', t: 'SaleOrderCityName', w: 120 }];
        var g = SE.grid($$('ldGrd'), { footer: false, frozen: 1, cols: cols });
        function bind(rows) {                                 // GrdDataBind
            lst = rows;
            g.setRows(rows.map(function (r) { var o = {}; cols.forEach(function (c) { if (c.k !== 'Sel') o[c.k] = ci(r, c.k); }); return o; }));
        }
        function pending() {                                  // PendingDataDbCall (only customer, delivery type and item are sent)
            return SE.api(API + '/loader/rows' + SE.q({ customerId: +cc.value() || 0, deliveryTypeId: typeIdValue, itemId: +ci2.value() || 0 })).then(bind);
        }
        function combos() {                                   // ComboDbCall + CombosFill
            return SE.api(API + '/loader/combos').then(function (d) {
                var kc = +cc.value() || 0, ki = +ci2.value() || 0;
                cc.setData(d.customers || []); ci2.setData(d.items || []);
                if (kc > 0 && !cc.setValue(kc)) cc.clear();
                if (ki > 0 && !ci2.setValue(ki)) ci2.clear();
                return d;
            });
        }
        $$('ldFrom').value = SE.addDays(-7); $$('ldTo').value = SE.today();
        $$('ldSearch').onclick = function () { pending().catch(function (e) { SE.alert(e.message); }); };
        $$('ldNew').onclick = function () {                   // btnReset_Click
            SE.api(API + '/loader/combos').then(function (d) { $$('ldFrom').value = SE.dateInput(d.fromDate) || SE.today(); }).catch(function () { });
            $$('ldNoFrom').value = ''; $$('ldNoTo').value = ''; cc.clear();
            pending().catch(function (e) { SE.alert(e.message); });
        };
        $$('ldRefresh').onclick = function () { combos().catch(function (e) { SE.alert(e.message); }); };
        $$('ldKeys').onclick = function () {
            SE.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Search'],
                ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
        };
        $$('ldLoad').onclick = function () {                  // btnLoadOnInvoice_Click_1
            var picked = g.checked();
            if (!picked.length) { SE.alert('Check the row first'); return; }
            var base = +picked[0].Id, ids = [];
            for (var i = 0; i < picked.length; i++) {
                if (+picked[i].Id !== base) { SE.alert('Sorry! You can Only Check rows which have the same DocNo'); return; }
                ids.push(+picked[i].DetailId);
            }
            var rows = lst.filter(function (r) { return ids.indexOf(+ci(r, 'DetailId')) >= 0; });
            if (rows.length) { pop.close(); loadInGridDetail(rows); }
        };
        combos().then(pending).catch(function (e) { SE.alert('Error occurred during database call: ' + e.message); });
    }
    /* LoadInGridDetail */
    function loadInGridDetail(dt) {
        try {
            S.saleOrderId = 0;
            if (dt.length > 0) {
                show('panel13', false);
                var rows = G.grd.rows();
                if (rows.length > 0 && (+rows[0].DeliveryOrderId || 0) !== (+ci(dt[0], 'Id') || 0))
                    throw new Error("Already Loaded Row's Have Different Order. So you Can't Load Rows Of Different Order!");
                enable(cb.cust, false); enable(cb.order, false); enable(cb.type, false);
                cb.cust.setValue(+ci(dt[0], 'SupplierCustomerId') || 0);
                cb.order.setValue(String(ci(dt[0], 'SaleOrderId')));
                var have = {};
                rows.forEach(function (r) { have[+r.DeliveryOrderDetailId || 0] = 1; });
                dt.forEach(function (dr) {
                    var detailId = +ci(dr, 'DetailId') || 0;
                    if (!have[detailId]) {
                        have[detailId] = 1;
                        G.grd.rows().push({ Id: 0, DeliveryOrderId: ci(dr, 'Id'), DeliveryOrderDetailId: detailId, DeliveryOrderNo: ci(dr, 'DocNo'), OrderId: ci(dr, 'SaleOrderId'), OrderDetailId: ci(dr, 'SaleOrderDetailId'),
                            WareHouseId: ci(dr, 'WarehouseId'), ItemId: ci(dr, 'ItemId'), ItemCode: ci(dr, 'ItemCode'), ItemName: ci(dr, 'ItemName'), ItemDescription: ci(dr, 'ItemDescription'),
                            PackUomId: ci(dr, 'PackUomId'), PackUom: ci(dr, 'PackUomCode'), Remarks: '', JobLotId: ci(dr, 'JobLotId'), ItemQty: ci(dr, 'Qty'), AssetId: ci(dr, 'AssetId'),
                            DeliveryTypeId: ci(dr, 'DeliveryTypeId'), WareHouseToId: ci(dr, 'WarehouseId') });
                    }
                });
                S.saleOrderId = +ci(dt[0], 'Id') || 0;
            }
            S.orderStatus = 'DeliverOrder';
            G.grd.refresh(); detailGridSettings();
        } catch (e) { SE.alert(e.message); }
    }

    /* ------------------------------------------------------------------ expense item pop-up (GridExpense_KeyDown F1 -> GrdPopUp) */
    function expensePopup(row) {
        var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>OtherItemName</th></tr></thead><tbody>' +
            S.other.map(function (r) { return '<tr data-id="' + SE.esc(r.Id) + '" style="cursor:pointer"><td>' + SE.esc(r.OtherItemName) + '</td></tr>'; }).join('') + '</tbody></table></div>';
        var pop = SE.pop('Select', h, [{ t: 'Close' }]);
        pop.open();
        pop.body.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-id]'); if (!tr) return;
            row.ItemId = +tr.getAttribute('data-id'); G.exp.refresh(); pop.close();
        });
    }

    /* ------------------------------------------------------------------ shortcut keys (frmGSIssuance_KeyDown) */
    var SHORT = [['Ctrl+S', 'When In Form For Save Else For Show Data in History'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Do cDate'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Warehouse in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && !e.ctrlKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'),
                function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (!e.ctrlKey) {
            if (e.key === 'F1' && t && t.closest && t.closest('#GridExpense') && G.exp.curKey() === 'ItemId' && G.exp.cur()) { e.preventDefault(); expensePopup(G.exp.cur()); }
            return;
        }
        var k = e.key.toLowerCase();
        if (k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); $('txtDocdate').focus(); } else { tabs.select('tabPage2'); $('FromDateHistory').focus(); } return; }
        if (tabs.index() === 0) {
            if (k === 's') { e.preventDefault(); if (isShown('btnsave') && !$('btnsave').disabled) btnSave(); }
            else if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'u') { e.preventDefault(); if (isShown('btnUpdate') && !$('btnUpdate').disabled) btnUpdate(); }
            else if (k === 'p') { e.preventDefault(); print(S.id); }
            else if (k === 'r') { e.preventDefault(); refresh(); }
            else if (e.key === 'F5') { e.preventDefault(); $('txtDocdate').focus(); }
            else if (e.key === 'F10') { e.preventDefault(); openAttachmentDialog(); }
            else if (e.key === 'ArrowDown') { if (tabs1.index() === 0) $('grdDetail').focus(); else $('GridExpense').focus(); }
            else if (e.key === 'ArrowRight') { if (tabs1.index() === 0) { tabs1.select('tabPage4'); $('GridExpense').focus(); } else { tabs1.select('tabPage3'); cb.wh.focus(); } }
            else if (e.key === 'ArrowUp') { tabs1.select('tabPage3'); cb.wh.focus(); }
            else if (k === 'd' && t && t.closest && t.closest('#GridExpense')) { e.preventDefault(); addExpenseRow(); }
            else if (e.key === 'Delete' && t && t.closest && t.closest('#GridExpense')) { e.preventDefault(); deleteExpenseRow(G.exp.curIndex()); }
        } else if (tabs.index() === 1) {
            if (k === 's') { e.preventDefault(); showHistory(); }
            else if (k === 'n') { e.preventDefault(); resetHistory(); }
            else if (k === 'r') { e.preventDefault(); refreshHistoryCombos(); }
            else if (e.key === 'ArrowDown') { $('DataGridHistory').focus(); }
            else if (e.key === 'ArrowRight') { $('grddetailofmain').focus(); }
            else if (e.key === 'ArrowUp') { $('FromDateHistory').focus(); }
        }
    }

    function wire() {
        $('btnnew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnsave').onclick = btnSave; $('btnUpdate').onclick = btnUpdate;
        $('btnDelete').onclick = del; $('btnAttachment').onclick = openAttachmentDialog; $('Print').onclick = function () { print(S.id); };
        $('BtnDeliveryOrder').onclick = loadDeliveryOrder;
        $('btnShortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnRequestedByLookUp').onclick = function () { window.open('/inventory/lookup-definitions', '_blank'); };
        $('Add').onclick = plus; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelDetail').onclick = cancelDetail;
        $('btnResetHistory').onclick = resetHistory; $('btnRefreshHistory').onclick = refreshHistoryCombos; $('btnShow').onclick = showHistory;
        $('rdbtnItemName').addEventListener('change', rdbChanged); $('rdbtnItemCode').addEventListener('change', rdbChanged);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
