/*
 * Screen 538  DeliveryOrder_Engr  (Architecture.WinApp.SaleTrading.DeliveryOrder_Engr, document type 1606)
 * Page script. Desktop methods are named in the comments (DeliveryOrder_Engr.cs). Server: /sale/engr/delivery-order/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/delivery-order/api', DOC = 1606;
    var S = {
        id: 0, approved: false, rights: {}, remove: [], updateIdx: -1,
        files: [], removedAtt: [], existing: [],
        out: [],            // dtOutStandingOrdersAndParties
        sup: [],            // dtSupplier
        items: [],          // dtitem
        ordIds: '', ordMainIds: '',
        wh: [], job: [], asset: [], histRows: []
    };
    var cb = {}, G = {}, tabs;

    function fail(e) { return SE.dbError(e); }
    function msg(t) { return SE.alert(t); }
    function typeId() { return +cb.otype.value() || 0; }
    function field(id) { return ($(id).value || '').trim(); }
    function enable(c, on) { c.el.disabled = !on; var w = c.el.__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.otype = XCombo('CmbDeliveryType', { columns: [{ key: 'type', caption: 'Delivery Type' }], textKey: 'type', onSelect: function () { typeChanged(); typeLeave(); } });
        cb.cust = XCombo('CmbSupplierCustomer', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420, onSelect: customerLeave });
        cb.order = XCombo('CmbOrderNo', { columns: [{ key: 'SaleOrderNo', caption: 'Order No' }], valueKey: 'SaleOrderId', textKey: 'SaleOrderNo', onSelect: function () { itemsByOrder(); } });
        cb.veh = XCombo('cmbvehicletype', { columns: [{ key: 'VehicleDescription', caption: 'Vehicle Type' }], textKey: 'VehicleDescription' });
        cb.req = XCombo('CmbRequestedBy', { columns: [{ key: 'Type', caption: 'Requested By' }], textKey: 'Type' });
        cb.app = XCombo('CmbApprovedBy', { columns: [{ key: 'Type', caption: 'Approved By' }], textKey: 'Type' });
        cb.wh = XCombo('CmbWareHouse', { columns: [{ key: 'WareHouseName', caption: 'WareHouseName' }], textKey: 'WareHouseName' });
        cb.whTo = XCombo('CmbWareHouseTo', { columns: [{ key: 'WareHouseName', caption: 'WareHouseTo Name' }], textKey: 'WareHouseName' });
        cb.item = XCombo('CmbItemName', { columns: [{ key: '_t', caption: 'Item Name' }, { key: '_o', caption: 'Item Code' }], textKey: '_t', popupWidth: 460, onSelect: itemLeave });
        cb.job = XCombo('CmbJobLot', { columns: [{ key: 'JobLotDescription', caption: 'Job Lot' }], textKey: 'JobLotDescription' });
        cb.uom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'Pack Uom' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode' });
        cb.asset = XCombo('CmbAssetName', { columns: [{ key: 'AssetName', caption: 'Asset Name' }], textKey: 'AssetName', popupWidth: 360 });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Name', caption: 'Customer Name' }], textKey: 'Name', popupWidth: 380 });
        cb.hType = XCombo('CmbDeliveryTypeHistory', { columns: [{ key: 'Name', caption: 'Delivery Type' }], textKey: 'Name' });
        cb.hReq = XCombo('CmbRequestedByHistory', { columns: [{ key: 'Name', caption: 'Requested By' }], textKey: 'Name' });
        cb.hApp = XCombo('CmbApprovedByHistory', { columns: [{ key: 'Name', caption: 'Approved By' }], textKey: 'Name' });
    }

    /* SupplierNameFill (skipped when the delivery type is Sale Order) */
    function fillSuppliers(rows, force) {
        S.sup = rows || S.sup;
        if (!force && typeId() === 1) return;
        var keep = +cb.cust.value() || 0;
        cb.cust.setData(S.sup);
        if (keep > 0 && !cb.cust.setValue(keep)) cb.cust.clear();
    }
    /* ItemDetailFillWithoutOrderId / rdbtnItemName_CheckedChanged / ItemBindbyOrderId all end in this bind */
    function bindItems(rows) {
        var byName = $('rdbtnItemName').checked;
        cb.item.setData(rows.map(function (r) { return { Id: r.Id, _t: byName ? r.ItemName : r.ItemCode, _o: byName ? r.ItemCode : r.ItemName, ItemName: r.ItemName, ItemCode: r.ItemCode, SaleOrderDetailId: r.SaleOrderDetailId, BalQty: r.BalQty }; }));
    }
    function fillItemsWithoutOrder() {
        if (typeId() === 1) return;
        var keep = +cb.item.value() || 0;
        if (S.items.length) { bindItems(S.items); if (keep > 0 && !cb.item.setValue(keep)) cb.item.clear(); }
        else { cb.item.setData([]); }
    }
    function fillWarehouses() {
        var a = +cb.wh.value() || 0, b = +cb.whTo.value() || 0;
        cb.wh.setData(S.wh); cb.whTo.setData(S.wh);
        if (a > 0 && !cb.wh.setValue(a)) cb.wh.clear();
        if (b > 0 && !cb.whTo.setValue(a)) cb.whTo.clear();        // WareHouseFill sets CmbWareHouseTo.Value = WHId (desktop quirk kept)
    }
    function fillJobLots() { cb.job.setData(S.job); }
    function fillAssets() {
        var k = +cb.asset.value() || 0;
        cb.asset.setData(S.asset);
        if (k > 0 && !cb.asset.setValue(k)) cb.asset.clear();
    }
    function fillLookups(rows) {
        var r = +cb.req.value() || 0, a = +cb.app.value() || 0;
        cb.req.setData(rows); cb.app.setData(rows);
        if (r > 0 && !cb.req.setValue(r)) cb.req.clear();
        if (a > 0 && !cb.app.setValue(a)) cb.app.clear();
    }

    /* ------------------------------------------------------------------ delivery type / customer / order (CmbDeliveryType_TextChanged / _Leave, SaleOrderBind ...) */
    function warehouseToShowHide() {
        var t = typeId(), grid = G.grd.spec.cols;
        function col(k) { return grid.filter(function (c) { return c.k === k; })[0]; }
        if (cb.otype.row() && t > 0 && t === 5) {
            $('labelWarFrom').textContent = 'Warehouse From'; $('labelWarTo').style.display = ''; cb.whTo.el.style.display = '';
            col('WareHouseId').t = 'WareHouse From'; col('WareHouseToId').hide = false; $('CmbAssetName').style.width = '262px';
        } else {
            $('labelWarFrom').textContent = 'Warehouse'; $('labelWarTo').style.display = 'none'; cb.whTo.el.style.display = 'none';
            col('WareHouseId').t = 'WareHouse'; col('WareHouseToId').hide = true; $('CmbAssetName').style.width = '468px';
        }
        G.grd.refresh();
    }
    function typeChanged() {
        customerLeave();                                          // CmbSupplierCustomer_Leave -> SaleOrderBind
        var rows = G.grd.rows();
        if (rows.length > 0 && cb.otype.row() && typeId() > 0) {
            if (typeId() === 1) {
                var all = rows.every(function (r) { return (+r.OrderId || 0) !== 0; });
                if (!all) { cb.otype.setValue(2); cb.otype.focus(); msg("You cannot select delivery type 'Sale Order' because data without OrderId exists in the grid."); return; }
            }
            rows.forEach(function (r) { r.DeliveryTypeId = typeId(); });
        }
        warehouseToShowHide();
    }
    function typeLeave() {
        if (typeId() === 1) { outstandingParties(); return; }
        cb.order.setData([]);
        var keep = +cb.cust.value() || 0;
        cb.cust.setData(S.sup);
        if (keep > 0 && !cb.cust.setValue(keep)) cb.cust.clear();
        fillItemsWithoutOrder();
    }
    /* GetOutStandingParties: the first outstanding row of every customer */
    function firstPerKey(rows, key) {
        var seen = {}, out = [];
        rows.forEach(function (r) { var k = String(r[key]); if (!seen[k]) { seen[k] = 1; out.push(r); } });
        return out;
    }
    function outstandingParties() {
        var keep = +cb.cust.value() || 0;
        if (S.out.length > 0) cb.cust.setData(firstPerKey(S.out, 'Id'));
        else { cb.cust.setData([]); cb.order.setData([]); }
        if (keep > 0 && !cb.cust.setValue(keep)) cb.cust.clear();
    }
    /* SaleOrderBind: only the first outstanding row of the selected customer reaches the order combo (the desktop groups by customer Id) */
    function customerLeave() {
        if (typeId() === 1) {
            var c = +cb.cust.value() || 0;
            var rows = firstPerKey(S.out.filter(function (r) { return +r.Id === c; }), 'Id');
            cb.order.setData(rows);
        } else cb.order.setData([]);
    }
    /* ItemBindbyOrderId (+ CmbOrderNo_TextChanged / _Leave) */
    function itemsByOrder() {
        if (typeId() !== 1) return;
        var keep = +cb.item.value() || 0, o = +cb.order.value() || 0;
        var rows = S.out.filter(function (r) { return +r.SaleOrderId === o; });
        if (rows.length > 0) {
            S.items = rows.map(function (r) { return { Id: r.ItemId, ItemName: r.ItemName, ItemCode: r.ItemCode, SaleOrderDetailId: r.SaleOrderDetailId, BalQty: r.BalQty }; });
            bindItems(S.items);
            if (keep > 0 && !cb.item.setValue(keep)) cb.item.clear();
        } else cb.item.setData([]);
    }
    /* CmbItemName_Leave -> bindRateUomAndItemPackUom */
    function itemLeave() {
        var id = +cb.item.value() || 0;
        if (id > 0) return bindUoms(id);
        cb.uom.setData([]);
    }
    function bindUoms(itemId) {
        var prev = cb.uom.text().trim();
        return SE.api(API + '/uoms' + SE.q({ itemId: itemId })).then(function (rows) {
            if (rows.length) cb.uom.setData(rows); else cb.uom.setData([]);
            if (prev !== '') {
                var hit = rows.filter(function (r) { return r.UOMCode === prev; })[0];
                if (hit) cb.uom.setValue(hit.Id); else cb.uom.clear();
            }
        }).catch(fail);
    }

    /* GetOutStandingOrderAndParties */
    function loadOutstanding() {
        return SE.api(API + '/outstanding' + SE.q({ recId: S.id })).then(function (rows) { S.out = rows; if (typeId() > 0) typeLeave(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ grids */
    function makeGrids() {
        G.grd = SE.grid('grd', { footer: true, onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) { if (k === 'Add') addCopy(i); else if (k === 'Delete') deleteRow(i); },
            onEdit: function (r, k, v) {
                if (k === 'ItemQty') { var n = Number(String(v).replace(/,/g, '')); if (isFinite(n)) r[k] = n; }
                else if (/Id$/.test(k)) r[k] = v === '' ? 0 : +v;
                else r[k] = v;
            },
            cols: [
                { k: 'Id', hide: true }, { k: 'DeliveryTypeId', hide: true }, { k: 'OrderId', hide: true }, { k: 'SaleOrderDetailId', hide: true }, { k: 'SupCustId', hide: true },
                { k: 'WareHouseId', t: 'WareHouseName', w: 150, edit: true, list: function () { return S.wh; }, lk: 'Id', lt: 'WareHouseName' },
                { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 },
                { k: 'ItemDescription', t: 'ItemDescription', w: 200, edit: true }, { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'Uom', w: 70 },
                { k: 'Remarks', t: 'Specification / Remarks', w: 200, edit: true },
                { k: 'JobLotId', t: 'Job Lot', w: 150, edit: true, list: function () { return S.job; }, lk: 'Id', lt: 'JobLotDescription' },
                { k: 'ItemQty', t: 'Qty', w: 90, f: 'n3', sum: true, edit: true },
                { k: 'MachineAssetId', t: 'Machine/Asset', w: 150, edit: true, list: function () { return S.asset; }, lk: 'Id', lt: 'AssetName' },
                { k: 'WareHouseToId', t: 'WareHouseTo Name', w: 150, edit: true, hide: true, list: function () { return S.wh; }, lk: 'Id', lt: 'WareHouseName' },
                { k: 'Add', t: '+', w: 28, btn: '+' }, { k: 'Delete', t: 'X', w: 28, btn: 'X' }] });
        G.hist = SE.grid('grdhistory', { frozen: 4, onDbl: function (r) { histEdit(r, false, false); },
            onBtn: function (k, r) {
                if (k === 'Edit') histEdit(r, true, false); else if (k === 'SaveAs') histSaveAs(r); else if (k === 'Print') print(r.Id);
            },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(r.Id); },
            onSel: histSelected,
            cols: [
                { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 50, btn: 'Print' }, { k: 'Print264', t: 'DC-Print', w: 50, btn: 'DC-Print', hide: true },
                { k: 'SaveAs', t: 'SaveAs', w: 60, btn: 'SaveAs' }, { k: 'Id', hide: true },
                { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' }, { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'DeliveryType', t: 'DeliveryType', w: 100 }, { k: 'Customer', t: 'Customer', w: 220 },
                { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'VehicleType', t: 'VehicleType', w: 100 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
                { k: 'RequestedBy', t: 'RequestedBy', w: 110 }, { k: 'ApprovedBy', t: 'ApprovedBy', w: 110 }, { k: 'ApprovalStatus', t: 'ApprovalStatus', w: 100 },
                { k: 'EntryUser', t: 'EntryUser', w: 100 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'ModifyUser', t: 'ModifyUser', w: 100 },
                { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ApprovedUser', t: 'ApprovedUser', w: 100 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' },
                { k: 'Remarks', t: 'Remarks', w: 200 }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }] });
        G.hd = SE.grid('GrdHistoryDetail', {
            cols: [{ k: 'WareHouseName', t: 'WareHouse', w: 150 }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemDescription', t: 'ItemDescription', w: 200 },
                { k: 'UOM', t: 'UOM', w: 70 }, { k: 'Specification/Remarks', t: 'Specification/Remarks', w: 200 }, { k: 'JobLot', t: 'JobLot', w: 120 },
                { k: 'Qty', t: 'Qty', w: 90, f: 'n3', sum: true }, { k: 'Machine/Asset', t: 'Machine/Asset', w: 150 }, { k: 'WareHouseTo', t: 'WareHouseTo', w: 150, hide: true }] });
    }

    /* grdhistory_SelectionChanged */
    function histSelected(item) {
        if (!item) { G.hd.setRows([]); return; }
        var rows = S.histRows.filter(function (r) { return +r.Id === +item.Id; });
        var transfer = String(item.DeliveryType) === 'Transfer';
        var cols = G.hd.spec.cols;
        cols[0].t = transfer ? 'WareHouse From' : 'WareHouse';
        cols[cols.length - 1].hide = !transfer;
        G.hd.setRows(rows.map(function (r) {
            return { WareHouseName: r.WareHouseName, ItemCode: r.ItemCode, ItemName: r.ItemName, ItemDescription: r.ItemDiscription, UOM: r.PackUOM, 'Specification/Remarks': r.LoadingRemarks,
                JobLot: r.JobLotDescription, Qty: r.LoadingQty, 'Machine/Asset': r.AssetName, WareHouseTo: r.WareHouseToName };
        }));
    }

    /* ------------------------------------------------------------------ detail entry (btnplus_Click / FormValidationDetail / ResetDetail) */
    function detailValid() {
        function stop(m, f) { msg(m); if (f) f(); return false; }
        var t = typeId();
        if (t <= 0) return stop('Please Select Delivery Type field First', function () { cb.otype.focus(); });
        if (!(+cb.cust.value() > 0)) return stop('Please Select Customer Field ', function () { cb.cust.focus(); });
        if (t === 1 && !cb.order.row()) return stop('OrderNo field is required', function () { cb.order.focus(); });
        if (!cb.wh.row() || !(+cb.wh.value())) return stop('WareHouseName field is required', function () { cb.wh.focus(); });
        if (!cb.item.row() || !(+cb.item.value())) return stop('ItemName field is required', function () { cb.item.focus(); });
        if (!cb.job.row() || !(+cb.job.value())) return stop('JobLot field is required', function () { cb.job.focus(); });
        if (!cb.uom.row() || !(+cb.uom.value())) return stop('PackUOM field is required', function () { cb.uom.focus(); });
        if (SE.toNum($('txtqty').value) === 0) return stop('ItemQty field is required', function () { $('txtqty').focus(); });
        if (t === 3 && !(+cb.asset.value() > 0)) return stop('Product/Machine/Asset field is required', function () { cb.asset.focus(); });
        if (t === 5 && !(+cb.whTo.value() > 0)) return stop('WareHouse To field is required', function () { cb.whTo.focus(); });
        return true;
    }
    function resetDetail() {
        cb.item.clear(); cb.uom.clear(); $('txtremarksdetail').value = ''; $('txtqty').value = '';
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        cb.item.focus();
    }
    function plus() {
        if (!detailValid()) return;
        var t = typeId(), orderOn = t === 1 && (+cb.order.value() > 0), ir = cb.item.row();
        G.grd.addRow({ Id: 0, DeliveryTypeId: t, OrderId: orderOn ? +cb.order.value() : 0, SaleOrderDetailId: orderOn ? (+ir.SaleOrderDetailId || 0) : 0, SupCustId: +cb.cust.value(),
            WareHouseId: +cb.wh.value(), ItemId: +cb.item.value(), ItemCode: ir.ItemCode, ItemName: ir.ItemName, ItemDescription: $('txtItemDescriptions').value,
            PackUomId: +cb.uom.value(), PackUom: cb.uom.text().trim(), Remarks: $('txtremarksdetail').value.trim(), JobLotId: +cb.job.value(), ItemQty: SE.toNum($('txtqty').value),
            MachineAssetId: +cb.asset.value() || 0, WareHouseToId: +cb.whTo.value() || 0 });
        resetDetail();
        enable(cb.otype, false); enable(cb.cust, false);
        if (t === 1) enable(cb.order, false);
    }
    /* grd_DoubleClick */
    function editRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        S.updateIdx = i;
        cb.otype.setValue(r.DeliveryTypeId);
        cb.wh.setValue(r.WareHouseId);
        cb.item.setValue(+r.ItemId);
        bindUoms(+r.ItemId).then(function () {
            $('txtItemDescriptions').value = r.ItemDescription == null ? '' : r.ItemDescription;
            cb.uom.setValue(r.PackUomId);
        });
        $('txtremarksdetail').value = r.Remarks == null ? '' : r.Remarks;
        cb.job.setValue(r.JobLotId);
        $('txtqty').value = r.ItemQty == null ? '' : r.ItemQty;
        cb.asset.setValue(+r.MachineAssetId || 0);
        cb.whTo.setValue(r.WareHouseToId);
        show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        cb.wh.focus();
    }
    function updateDetail() {
        if (!detailValid()) return;
        var r = G.grd.rows()[S.updateIdx]; if (!r) return;
        var t = typeId(), ir = cb.item.row();
        r.DeliveryTypeId = t; r.WareHouseId = +cb.wh.value(); r.OrderId = +cb.order.value() || 0;
        if (t === 1) r.SaleOrderDetailId = +ir.SaleOrderDetailId || 0;
        r.ItemId = +cb.item.value(); r.ItemName = ir.ItemName; r.ItemCode = ir.ItemCode; r.ItemDescription = $('txtItemDescriptions').value;
        r.PackUomId = +cb.uom.value(); r.PackUom = cb.uom.text(); r.Remarks = $('txtremarksdetail').value; r.JobLotId = +cb.job.value();
        r.ItemQty = SE.toNum($('txtqty').value); r.MachineAssetId = +cb.asset.value() || 0; r.WareHouseToId = +cb.whTo.value() || 0;
        G.grd.refresh();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        resetDetail();
    }
    function cancelUpdateDetail() {
        resetDetail();
        $('txtRefDocTypeId').value = ''; $('txtDocNoId').value = ''; $('txtSubNoId').value = '';
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
    }
    /* grd_ColumnButtonClick "Add" */
    function addCopy(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        var c = JSON.parse(JSON.stringify(r));
        if (isShown('btnupdate') && !$('btnupdate').disabled) c.Id = 0;
        G.grd.addRow(c);
    }
    /* grd_ColumnButtonClick "Delete" */
    function deleteRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        function finish() {
            G.grd.removeAt(i);
            if (G.grd.count() === 0) { enable(cb.otype, true); enable(cb.cust, true); enable(cb.order, true); }
            getSaleOrderIds();
        }
        if ((+r.Id || 0) > 0) {
            return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
                if (!yes) return;
                S.remove.push({ id: +r.Id, deliveryTypeId: +r.DeliveryTypeId, orderId: +r.OrderId, saleOrderDetailId: +r.SaleOrderDetailId, supCustId: +r.SupCustId, wareHouseId: +r.WareHouseId,
                    itemId: +r.ItemId, packUomId: +r.PackUomId, remarks: r.Remarks == null ? '' : String(r.Remarks), jobLotId: +r.JobLotId, itemQty: SE.toNum(r.ItemQty), machineAssetId: +r.MachineAssetId || 0 });
                finish();
            });
        }
        finish();
    }
    function getSaleOrderIds() {
        S.ordIds = ''; S.ordMainIds = '';
        G.grd.rows().forEach(function (r) { S.ordIds += ',' + (+r.SaleOrderDetailId || 0); S.ordMainIds += ',' + (+r.OrderId || 0); });
    }

    /* ------------------------------------------------------------------ Load */
    function applyInitial(d) {
        S.rights = d.rights || {};
        S.wh = d.warehouses || []; S.job = d.jobLots || []; S.asset = d.assets || [];
        S.out = d.outstanding || [];
        cb.otype.setData(d.deliveryTypes);
        S.sup = d.customers; S.items = d.items;
        $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : $('txtdocno').value;
        cb.veh.setData(d.vehicleTypes);
        fillWarehouses(); fillJobLots(); fillAssets();
        fillSuppliers(S.sup, false);
        fillItemsWithoutOrder();
        fillLookups(d.lookups || []);
        var h = d.history || {};
        cb.hCust.setData(h.customers || []); cb.hType.setData(h.deliveryTypes || []); cb.hReq.setData(h.requestedBy || []); cb.hApp.setData(h.approvedBy || []);
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') $('FromDateHistory').focus(); });
        SE.digitsOnly($('txtFromDocNoHistory')); SE.digitsOnly($('txtToDocNoHistory')); SE.upper($('txtVehicleNo'));
        $('txtqty').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        makeGrids();
        SE.api(API + '/initial').then(function (d) {
            applyInitial(d);
            $('btnsave').disabled = !S.rights.save; $('btnupdate').disabled = !S.rights.update; $('btnprint').disabled = !S.rights.print;
            show('btnDelete', !!S.rights.delete);
            show('btnupdate', false); show('btnSaveAs', false); show('btnsave', true); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            $('ChkPrint').checked = true;
            show('btnLoadSaleOrder', true); $('btnLoadSaleOrder').disabled = false;
            $('DocDate').value = SE.today();
            $('FromDateHistory').value = SE.today(); $('ToDateHistory').value = SE.today();
            warehouseToShowHide();
            $('DocDate').focus();
            var rec = SE.param('record'); if (rec) edit(+rec, true);
        }).catch(fail);
        wire();
    }

    /* ------------------------------------------------------------------ Reset (btnnew_Click) */
    function reset() {
        S.files = []; S.removedAtt = []; S.existing = []; S.remove = []; S.ordIds = ''; S.ordMainIds = ''; S.id = 0;
        enable(cb.otype, true); enable(cb.order, true); enable(cb.cust, true);
        G.grd.setRows([]);
        cb.otype.clear();
        if (cb.otype.rows().length) { cb.otype.setValue(cb.otype.rows()[0].Id); }
        typeLeave();
        cb.cust.clear(); cb.order.clear(); cb.order.setData([]);
        $('txtVehicleNo').value = ''; cb.veh.clear();
        cb.req.clear(); cb.app.clear();
        $('txtremarks').value = ''; $('txtAccountRemarks').value = '';
        S.approved = false;
        cb.item.clear(); cb.uom.clear(); $('txtqty').value = '';
        $('txtRefDocTypeId').value = ''; $('txtDocNoId').value = ''; $('txtSubNoId').value = '';
        show('btnsave', true); show('btnupdate', false); show('btnSaveAs', false);
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        warehouseToShowHide();
        $('DocDate').focus();
        return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); }).catch(fail);
    }

    /* btnRefresh_Click */
    function refresh() {
        return SE.api(API + '/refresh' + SE.q({ recId: S.id })).then(function (d) {
            S.sup = d.customers; S.items = d.items; S.wh = d.warehouses; S.job = d.jobLots; S.asset = d.assets; S.out = d.outstanding;
            fillSuppliers(S.sup, false); fillItemsWithoutOrder();
            if (typeId() > 0) typeLeave();
            fillLookups(d.lookups || []); fillWarehouses(); fillJobLots(); fillAssets();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Insert / btnupdate_Click / btnSaveAs_Click */
    function formValid() {
        function stop(m, f) { return SE.alert(m).then(function () { if (f) f(); return false; }); }
        var no = field('txtdocno');
        if (no === '' || no === '0') return stop('DocNo Field is Required', function () { $('txtdocno').focus(); });
        if (!(+cb.cust.value() > 0)) return stop('Customer Field Required', function () { cb.cust.focus(); });
        if (!cb.veh.row()) return stop('Vehicle Type Field is Required', function () { cb.veh.focus(); });
        if ($('txtVehicleNo').value === '') return stop('Vehicle No Field is Required', function () { $('txtVehicleNo').focus(); });
        if (!(+cb.req.value() > 0)) return stop('Requested By Field is Required', function () { cb.req.focus(); });
        if (!(+cb.app.value() > 0)) return stop('Approved By Field is Required', function () { cb.app.focus(); });
        return Promise.resolve(true);
    }
    function insert() {
        if (G.grd.count() === 0) return SE.alert('Grid Record Not Found', 'Database Error');
        return formValid().then(function (ok) {
            if (!ok) return;
            return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var body = {
                    id: S.id, docDate: $('DocDate').value, docNo: field('txtdocno'), deliveryTypeId: typeId(), customerId: +cb.cust.value() || 0,
                    vehicleType: cb.veh.text(), vehicleNo: field('txtVehicleNo'), requestedById: +cb.req.value() || 0, approvedById: +cb.app.value() || 0,
                    remarks: $('txtremarks').value, accountRemarks: $('txtAccountRemarks').value,
                    lines: G.grd.rows().map(lineOf), removed: S.remove,
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt }
                };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return SE.alert(r.message).then(function () {
                        var preview = $('ChkPrint').checked;
                        return reset().then(loadOutstanding).then(function () { if (preview) print(r.id); });
                    });
                }).catch(function (e) { return SE.alert(e.message, 'Database Error'); });
            });
        });
    }
    function lineOf(r) {
        return { id: +r.Id || 0, deliveryTypeId: +r.DeliveryTypeId || 0, orderId: +r.OrderId || 0, saleOrderDetailId: +r.SaleOrderDetailId || 0, supCustId: +r.SupCustId || 0,
            wareHouseId: +r.WareHouseId || 0, itemId: +r.ItemId || 0, itemDescription: r.ItemDescription == null ? '' : String(r.ItemDescription), packUomId: +r.PackUomId || 0,
            remarks: r.Remarks == null ? '' : String(r.Remarks), jobLotId: +r.JobLotId || 0, itemQty: SE.toNum(r.ItemQty), machineAssetId: +r.MachineAssetId || 0, wareHouseToId: +r.WareHouseToId || 0 };
    }
    function btnSave() { S.id = 0; return insert(); }
    function btnSaveAs() {
        S.id = 0;
        G.grd.rows().forEach(function (r) { r.Id = 0; });
        return insert();
    }
    function btnUpdate() {
        if (S.id === 0) return SE.alert('Record Not Update because RecId Not Found', 'Database Error');
        return insert();
    }

    /* ------------------------------------------------------------------ ReadById */
    function edit(id, resetFirst) {
        var p = resetFirst ? reset() : Promise.resolve();
        return p.then(function () {
            S.id = id;
            return SE.api(API + '/' + id);
        }).then(function (d) {
            S.out = d.outstanding || S.out;
            var h = d.head, lines = d.lines || [];
            tabs.select('tabPage1');
            $('txtdocno').value = h.DocNo; $('DocDate').value = SE.dateInput(h.DocDate);
            if (lines.length > 0) {
                enable(cb.otype, false); enable(cb.cust, false); enable(cb.order, false);
                cb.otype.setValue(lines[0].DeliveryTypeId); typeChanged(); typeLeave();
                cb.cust.setValue(lines[0].SupplierCustomerId); customerLeave();
                if ((+lines[0].SaleOrderId || 0) > 0) { cb.order.setValue(lines[0].SaleOrderId); itemsByOrder(); }
            }
            selByTextVeh(h.VehicleType);
            $('txtVehicleNo').value = h.VehicleNo || '';
            cb.req.setValue(h.RequestedByLookUpId); cb.app.setValue(h.ApprovedByLookUpId);
            $('txtremarks').value = h.LoadingInstructions || ''; $('txtAccountRemarks').value = h.AccountRemarks || '';
            S.approved = !!h.IsApproved;
            G.grd.setRows(lines.map(function (l) {
                return { Id: l.Id, DeliveryTypeId: l.DeliveryTypeId, OrderId: l.SaleOrderId, SaleOrderDetailId: l.SaleOrderDetailId, SupCustId: l.SupplierCustomerId, WareHouseId: l.WarehouseId,
                    ItemId: l.ItemId, ItemCode: l.ItemCode, ItemName: l.ItemName, ItemDescription: l.ItemDiscription, PackUomId: l.PackUomId, PackUom: l.PackUOM, Remarks: l.LoadingRemarks,
                    JobLotId: l.JobLotId, ItemQty: l.DoQty, MachineAssetId: l.AssetId, WareHouseToId: l.WareHouseToId };
            }));
            warehouseToShowHide();
            S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
            show('btnsave', false); show('btnupdate', true); show('btnSaveAs', true);
            $('DocDate').focus();
        }).catch(fail);
    }
    function selByTextVeh(text) {
        var rows = cb.veh.rows();
        for (var i = 0; i < rows.length; i++) if (String(rows[i].VehicleDescription) === String(text)) { cb.veh.setValue(rows[i].Id); return; }
        cb.veh.clear();
    }
    /* history Edit button / Ctrl+Enter: Reset(); ReadById(Id); then SaveAs/Save/Delete hidden, Update shown. Double click: ReadById(Id) only. */
    function histEdit(r, resetFirst) {
        return edit(+r.Id, resetFirst).then(function () { show('btnSaveAs', false); show('btnsave', false); show('btnupdate', true); show('btnDelete', false); });
    }
    function histSaveAs(r) {
        if (String(r.DeliveryType) === 'Sale Order') return SE.alert("Record With type 'Sale Order' Can not be Save As");
        return reset().then(function () { return edit(+r.Id, false); }).then(function () {
            show('btnSaveAs', true); show('btnsave', false); show('btnupdate', false); show('btnDelete', false);
            enable(cb.otype, true); enable(cb.cust, true); enable(cb.order, true);
            G.grd.rows().forEach(function (x) { x.Id = 0; }); G.grd.refresh();
        });
    }

    /* ------------------------------------------------------------------ history (gridhistoryfill) */
    function showHistory() {
        var dt = $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : ($('rdapproveddate').checked ? 'approved' : 'document'));
        if ($('drdocdate').checked) dt = 'document';
        var q = { dateType: dt, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
            fromNo: SE.toInt($('txtFromDocNoHistory').value), toNo: SE.toInt($('txtToDocNoHistory').value), deliveryTypeId: +cb.hType.value() || 0, customerId: +cb.hCust.value() || 0,
            requestedById: +cb.hReq.value() || 0, approvedById: +cb.hApp.value() || 0 };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            S.histRows = rows;
            var seen = {}, main = [];
            rows.forEach(function (r) {
                if (seen[r.Id]) return; seen[r.Id] = 1;
                main.push({ Id: r.Id, DocDate: r.DocDate, DocNo: r.DocNo, DeliveryType: r.DeliveryType, Customer: r.CustomerName, OrderNo: r.OrderNo, VehicleType: r.VehicleType, VehicleNo: r.VehicleNo,
                    RequestedBy: r.RequestedBy, ApprovedBy: r.ApprovedBy, ApprovalStatus: r.ApprovalStatus, EntryUser: r.EntryUser, EntryDate: r.EntryDate, ModifyUser: r.ModifyUser,
                    ModifyDate: r.ModifyDate, ApprovedUser: r.ApprovedUser, ApprovedDate: r.ApprovedDate, Remarks: r.HeaderRemarks, NoOfAttachments: r.NoOfAttachments });
            });
            G.hist.setRows(main);
            if (!main.length) G.hd.setRows([]);
        }).catch(fail);
    }
    function resetHistory() {
        var d = new Date(); d.setDate(d.getDate() - 3);
        $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today();
        $('txtFromDocNoHistory').value = ''; $('txtToDocNoHistory').value = '';
        cb.hCust.clear(); cb.hReq.clear(); cb.hApp.clear(); cb.hType.clear();
        G.hist.setRows([]); G.hd.setRows([]); S.histRows = [];
        $('drdocdate').checked = true;
    }
    function refreshHistoryCombos() {
        return SE.api(API + '/history-combos').then(function (h) {
            cb.hCust.setData(h.customers || []); cb.hType.setData(h.deliveryTypes || []); cb.hReq.setData(h.requestedBy || []); cb.hApp.setData(h.approvedBy || []);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ print / delete / attachments */
    function print(id) {
        if (!(id > 0)) return SE.alert('No Record Found For Display');
        SE.printRpt('1606-DeliveryOrderSlip_Engr.rpt', { id: id, documentTypeId: DOC });
    }
    function del() {
        if (S.id === 0) return SE.alert('Record Id Not Found', 'Message');
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            if (S.approved) return SE.alert('Record has been approved', 'Message');
            if (!(isShown('btnDelete') && !$('btnDelete').disabled)) return;
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return SE.alert(r.message); }).catch(function (e) { return SE.alert(e.message, 'Message'); });
        });
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
        if (!readOnly) h += '<div style="padding:6px"><input type="file" id="atFile" multiple> <span class="se-note">Up to 5 MB each. Files are stored when the delivery order is saved.</span></div>';
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

    /* ------------------------------------------------------------------ SaleOrderLoadForDo (btnLoadSaleOrder = main + detail, btnLoaderView2 = main only) */
    function openLoader(mainDetail) {
        var h = '<div class="lbar"><span class="dl" style="left:3px;top:6px">Sale Order Load</span></div>' +
            '<div class="lf">' +
            '<span class="dl" style="left:4px;top:9px">From Date</span><input class="f" type="date" id="ldFrom" style="left:75px;top:5px;width:155px;height:23px">' +
            '<span class="dl" style="left:4px;top:39px">To Date</span><input class="f" type="date" id="ldTo" style="left:75px;top:35px;width:155px;height:23px">' +
            '<span class="dl" style="left:241px;top:9px">CustomerName</span><select class="f" id="ldCust" style="left:336px;top:3px;width:286px;height:26px"></select>' +
            '<span class="dl" style="left:241px;top:39px">Item Name</span><select class="f" id="ldItem" style="left:336px;top:33px;width:286px;height:26px"></select>' +
            '<button type="button" class="dbtn" id="ldSearch" style="left:625px;top:33px;width:71px;height:26px">Search</button>' +
            '<button type="button" class="dbtn" id="ldLoad" style="left:696px;top:33px;width:71px;height:26px">Load</button></div>' +
            '<div class="dtc" id="ldTabs"><div class="dsubtabs"><span class="tab on" data-tab="ldP1">' + (mainDetail ? 'Main Detail' : 'Main') + '</span></div>' +
            '<div class="tpane" id="ldP1">' +
            (mainDetail ? '<div class="lbar" style="height:30px"><span class="dl" style="left:5px;top:5px">Order Main Information</span></div><div class="lgrid" id="ldGrd" style="height:240px"></div>' +
                '<div class="lbar" style="height:29px"><span class="dl" style="left:3px;top:5px">Detail Information</span></div><div class="lgrid" id="ldDet" style="height:230px"></div>'
                : '<div class="lbar" style="height:30px"><span class="dl" style="left:5px;top:5px">Order Information</span></div><div class="lgrid" id="ldSecond" style="height:420px"></div>') +
            '</div></div>';
        var lst = [], pop = SE.pop('SaleOrderLoad', h, [{ t: 'Close' }], { wide: true });
        pop.open();
        var $$ = function (id) { return pop.body.querySelector('#' + id); };
        var cc = XCombo($$('ldCust'), { columns: [{ key: 'Name', caption: 'Customer Name' }], textKey: 'Name', popupWidth: 380 });
        var ci = XCombo($$('ldItem'), { columns: [{ key: 'Name', caption: 'Item Name' }], textKey: 'Name', popupWidth: 380 });
        var mainCols = [{ k: 'Sel', t: '', w: 30, sel: true }, { k: 'Id', hide: true }, { k: 'OrderDetailId', hide: true }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' },
            { k: 'CustomerId', hide: true }, { k: 'CustomerName', t: 'CustomerName', w: 260 }, { k: 'SupplierRefNo', t: 'SupplierRefNo', w: 120 }, { k: 'PaymentTerm', t: 'PaymentTerm', w: 140 },
            { k: 'DueDays', t: 'DueDays', w: 70 }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 140 }];
        var detCols = [{ k: 'Sel', t: '', w: 30, sel: true }, { k: 'Id', hide: true }, { k: 'OrderDetailId', hide: true }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'date' }, { k: 'DocNo', t: 'DocNo', w: 60 },
            { k: 'SupplierCustomerId', hide: true }, { k: 'CustomerName', t: 'CustomerName', w: 240 }, { k: 'SupplierRefNo', t: 'SupplierRefNo', w: 110 }, { k: 'PaymentTerm', t: 'PaymentTerm', w: 120 },
            { k: 'DueDays', t: 'DueDays', w: 60 }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 110 }, { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 },
            { k: 'JobLotId', hide: true }, { k: 'JobLot', t: 'JobLot', w: 100 }, { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 70 }, { k: 'PackEquivalent', hide: true },
            { k: 'ItemQty', t: 'ItemQty', w: 80, f: 'n3', sum: true }, { k: 'DispatchQty', t: 'DispatchQty', w: 90, f: 'n3', sum: true }, { k: 'BalQty', t: 'BalQty', w: 80, f: 'n3', sum: true },
            { k: 'Weight', t: 'Weight', w: 80, f: 'n3', sum: true }, { k: 'DispatchWeight', t: 'DispatchWeight', w: 100, f: 'n3', sum: true }, { k: 'BalWeight', t: 'BalWeight', w: 90, f: 'n3', sum: true },
            { k: 'ItemRate', t: 'ItemRate', w: 80 }, { k: 'RateUomId', hide: true }, { k: 'RateUom', t: 'RateUom', w: 70 }, { k: 'RateEquivalent', hide: true }];
        function detRow(r) {
            return { Id: r.Id, OrderDetailId: r.PoDId, DocDate: r.DocDate, DocNo: r.DocNo, SupplierCustomerId: r.OrderSupCustId, CustomerName: r.SupplierName, SupplierRefNo: r.SupplierRefNo,
                PaymentTerm: r.TermsDescription, DueDays: r.OrderDueDays, DeliveryTerm: r.DeliveryTerm, ItemId: r.OrderItemId, ItemCode: r.ItemCode, ItemName: r.ItemName, JobLotId: r.JobLotId,
                JobLot: r.JobLotCode, PackUomId: r.PackUomId, PackUom: r.PackUom, PackEquivalent: r.PackEquivalent, ItemQty: r.OrderItemQty, DispatchQty: r.DispatchQty, BalQty: r.BalQty,
                Weight: r.NetWeight, DispatchWeight: r.DispatchWeight, BalWeight: r.BalWeight, ItemRate: r.OrderItemRate, RateUomId: r.RateUomId, RateUom: r.RateUom, RateEquivalent: r.EquivalentRate };
        }
        var gMain, gDet, gSecond;
        if (mainDetail) {
            gMain = SE.grid($$('ldGrd'), { footer: false, cols: mainCols, onCheck: function () { bindDetail(); } });
            gDet = SE.grid($$('ldDet'), { cols: detCols });
        } else {
            gSecond = SE.grid($$('ldSecond'), { cols: detCols });
        }
        function query() {
            return SE.api(API + '/loader/orders' + SE.q({ fromDate: $$('ldFrom').value, toDate: $$('ldTo').value, customerId: +cc.value() || 0, itemId: +ci.value() || 0 }));
        }
        function pendingLoad() {                                                           // PendingSaleOrderLoad
            return query().then(function (rows) {
                lst = rows;
                var seen = {}, main = [];
                rows.forEach(function (r) { if (seen[r.Id]) return; seen[r.Id] = 1; main.push({ Id: r.Id, OrderDetailId: r.PoDId, DocNo: r.DocNo, DocDate: r.DocDate, CustomerId: r.OrderSupCustId,
                    CustomerName: r.SupplierName, SupplierRefNo: r.SupplierRefNo, PaymentTerm: r.TermsDescription, DueDays: r.OrderDueDays, DeliveryTerm: r.DeliveryTerm }); });
                gMain.setRows(main);
            });
        }
        function bindDetail() {                                                            // GetCheckedRowsId + GridDetailBind + CheckedAllDetailRows
            var ids = gMain.checked().map(function (r) { return +r.Id; });
            if (!ids.length) { gDet.setRows([]); return; }
            var rows = lst.filter(function (r) { return ids.indexOf(+r.Id) >= 0; }).map(detRow);
            gDet.setRows(rows);
            if (S.ordIds !== '') gDet.check(function (r) { return S.ordIds.indexOf(String(r.OrderDetailId)) >= 0; });
            else gDet.check(function () { return true; });
        }
        function secondFill() {                                                            // GridSecondViewFill
            return query().then(function (rows) { lst = rows; gSecond.setRows(rows.map(detRow)); });
        }
        function run() {
            if (mainDetail) return pendingLoad().then(function () { gMain.check(function (r) { return S.ordMainIds !== '' && S.ordMainIds.indexOf(String(r.Id)) >= 0; }); bindDetail(); });
            return secondFill().then(function () { /* CheckedAllMainRowsSecondView */ gSecond.check(function (r) { return S.ordMainIds !== '' && S.ordMainIds.indexOf(String(r.Id)) >= 0; }); });
        }
        $$('ldSearch').onclick = function () { (mainDetail ? pendingLoad() : secondFill()).catch(fail); };       // btngrnlod_Click: PendingSaleOrderLoad(); GridSecondViewFill();
        $$('ldLoad').onclick = function () {                                               // btnLoadOnInvoice_Click_1
            var grid = mainDetail ? gDet : gSecond, picked = grid.checked();
            if (!picked.length) { SE.alert('No Row is Selected'); return; }
            var oid = 0, out = [];
            for (var i = 0; i < picked.length; i++) {
                var id = +picked[i].Id;
                if (oid === 0) oid = id;
                if (oid !== id) { SE.alert('Sorry!. The Selected ' + (mainDetail ? "Row's" : 'Rows') + ' are not of same Order'); return; }
                var src = lst.filter(function (r) { return +r.PoDId === +picked[i].OrderDetailId; })[0];
                if (src) out.push(src);
            }
            pop.close();
            loadInGrid(out);
        };
        SE.api(API + '/loader/combos').then(function (d) {
            cc.setData(d.customers || []); ci.setData(d.items || []);
            $$('ldFrom').value = SE.dateInput(d.fromDate) || SE.today(); $$('ldTo').value = SE.today();
            return run();
        }).catch(fail);
    }
    /* LoadInGridDetail */
    function loadInGrid(sale) {
        if (!sale.length) return;
        if (typeId() === 0) { cb.otype.setValue(1); typeChanged(); typeLeave(); }
        if (typeId() === 1) {
            enable(cb.otype, false); enable(cb.cust, false);
            var rows = G.grd.rows();
            if (rows.length > 0 && (+rows[0].OrderId || 0) !== (+sale[0].Id || 0)) { SE.alert("Already Loaded Row's are of different Order. So you can't Load"); return; }
            enable(cb.order, false);
            cb.cust.setValue(+sale[0].OrderSupCustId); customerLeave();
            cb.order.setValue(+sale[0].Id); itemsByOrder();
            for (var i = 0; i < sale.length; i++) {
                var dup = G.grd.rows().some(function (r) { return (+r.SaleOrderDetailId || 0) === (+sale[i].PoDId || 0); });
                if (!dup) {
                    /* table.Rows.Add(...) in the desktop passes ItemName before ItemCode, so the two columns hold each other's value */
                    G.grd.rows().push({ Id: 0, DeliveryTypeId: 1, OrderId: sale[i].Id, SaleOrderDetailId: sale[i].PoDId, SupCustId: sale[0].OrderSupCustId, WareHouseId: 0, ItemId: sale[i].OrderItemId,
                        ItemCode: sale[i].ItemName, ItemName: sale[i].ItemCode, ItemDescription: sale[i].ItemDiscription, PackUomId: sale[i].PackUomId, PackUom: sale[i].PackUom, Remarks: '',
                        JobLotId: sale[i].JobLotId, ItemQty: sale[i].BalQty, MachineAssetId: 0, WareHouseToId: null });
                }
            }
            G.grd.refresh(); warehouseToShowHide(); getSaleOrderIds();
        } else if (G.grd.count() > 0) {
            SE.alert("You Can't Load data against order because Other delivery type data exist in detail grd");
        }
    }

    /* ------------------------------------------------------------------ shortcut keys (PurchsaeOrder_KeyDown) */
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+F12', 'For Save As'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On WareHouse'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'),
                function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.ctrlKey && (e.key === 't' || e.key === 'T')) { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); $('DocDate').focus(); } else { tabs.select('tabPage2'); $('FromDateHistory').focus(); } return; }
        var k = e.key.toLowerCase();
        if (tabs.index() === 0) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnsave') && !$('btnsave').disabled) btnSave(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnupdate') && !$('btnupdate').disabled) btnUpdate(); }
            else if (e.ctrlKey && k === 'p' && S.rights.print) { e.preventDefault(); $('DocDate').focus(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('DocDate').focus(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachmentDialog(); }
            else if (e.ctrlKey && e.key === 'F12') { e.preventDefault(); if (isShown('btnSaveAs') && !$('btnSaveAs').disabled) btnSaveAs(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { cb.wh.focus(); }
        } else if (tabs.index() === 1) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); resetHistory(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refreshHistoryCombos(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); showHistory(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grdhistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { $('FromDateHistory').focus(); }
            else if (e.ctrlKey && e.key === 'Enter' && G.hist.cur()) { e.preventDefault(); histEdit(G.hist.cur(), true); }
        }
    }

    function wire() {
        $('btnnew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnsave').onclick = btnSave; $('btnupdate').onclick = btnUpdate; $('btnSaveAs').onclick = btnSaveAs;
        $('btnattachment').onclick = openAttachmentDialog; $('btnDelete').onclick = del; $('btnprint').onclick = function () { print(S.id); };
        $('btnLoadSaleOrder').onclick = function () { openLoader(true); };
        $('btnLoaderView2').onclick = function () { openLoader(false); };
        $('btnplus').onclick = plus; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelUpdateDetial').onclick = cancelUpdateDetail;
        $('RequestedApprovedLookup').onclick = function () { window.open('/inventory/lookup-definitions', '_blank'); };
        $('btnNewHistory').onclick = resetHistory; $('btnRefreshHistory').onclick = refreshHistoryCombos; $('btnShowHistory').onclick = showHistory; $('btnLoadAll').onclick = showHistory;
        $('rdbtnItemName').addEventListener('change', rdbChanged); $('rdbtnItemCode').addEventListener('change', rdbChanged);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }
    /* rdbtnItemName_CheckedChanged */
    function rdbChanged() {
        if (!S.items.length && !cb.item.rows().length) return;
        var id = +cb.item.value() || 0;
        bindItems(S.items);
        if (id > 0) { cb.item.setValue(id); cb.item.focus(); }
    }

    document.addEventListener('DOMContentLoaded', load);
})();
