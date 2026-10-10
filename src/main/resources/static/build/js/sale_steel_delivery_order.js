/*
 * Screen 541  DeliveryOrder_St  (Architecture.WinApp.Steel.Sale.DeliveryOrder_St, document type 1506)
 * Page script. Desktop methods are named in the comments (DeliveryOrder_St.cs). Server: /sale/steel/delivery-order/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/steel/delivery-order/api', fail = SS.fail, DOC = 1506;
    var S = { id: 0, approved: false, rights: {}, dec: 0, remove: [], updateIdx: -1, ordIds: '', ordMainIds: '', files: [], removed: [], existing: [],
              refParties: [], wh: [], job: [], pack: [], customers: [], items: [], types: [], histSeq: 0 };
    var cb = {}, G = {}, tabs;
    var KEYS = ['Id', 'RefDocumentTypeId', 'RefDocIdNo', 'RefDocSubIdNo', 'RefPartyId', 'SupplierCustomerId', 'SupplierCustomer', 'SaleOrderDetailId', 'OrderId', 'OrderNo',
        'WareHouseId', 'ItemId', 'ItemName', 'JobLotId', 'PackingTypeId', 'PackUomId', 'PackUom', 'PUomEquivalent', 'ItemQty', 'Weight', 'LoadQty', 'LoadWeight', 'GrossWeight',
        'Remarks', 'AvailableStock'];                                                  // the DataTable `table` of PurchsaeOrder_Load, in column order

    function field(id) { return ($(id).value || '').trim(); }
    function typeVal() { return +cb.otype.value() || 0; }
    function tn(v) { return SE.toNum(v); }
    /* Conversion.ToInt on a number or a numeric text */
    function ti(v) { var n = Number(String(v == null ? '' : v).replace(/,/g, '')); return isFinite(n) ? Math.trunc(n) : 0; }
    function fmtSingle(v) { return SE.num(v, S.dec, S.dec); }                         // ToString(clsGlobalVariables.stringFormatsingle)
    function msg(t, cap) { return SE.alert(t, cap); }
    function show(id, on) { var b = $(id); if (b) b.style.display = on ? '' : 'none'; }
    function visible(id) { return SS.visible(id); }
    function onBlur(c, fn) { var x = c.el.__dtcombo; if (x && x.input) x.input.addEventListener('blur', fn); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.otype = XCombo('CmbDeliveryOrderType', { columns: [{ key: 'OrderType', caption: 'OrderType' }], textKey: 'OrderType', onLeave: typeLeave });
        cb.veh = XCombo('cmbvehicletype', { columns: [{ key: 'VehicleDescription', caption: 'Vehicle Description' }], textKey: 'VehicleDescription' });
        cb.cust = XCombo('CmbSupplierCustomer', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420, onSelect: saleOrderBind });
        cb.order = XCombo('CmbOrderNo', { columns: [{ key: 'SaleOrderNo', caption: 'SaleOrderNo' }], valueKey: 'SaleOrderId', textKey: 'SaleOrderNo', onSelect: orderChanged });
        cb.wh = XCombo('CmbWareHouse', { columns: [{ key: 'WareHouseName', caption: 'WareHouseName' }], textKey: 'WareHouseName', onSelect: availableStock });
        cb.item = XCombo('CmbItemName', { columns: [{ key: 'ItemName', caption: 'Item Name' }], textKey: 'ItemName', popupWidth: 520, onSelect: itemLeave });
        cb.job = XCombo('CmbJobLot', { columns: [{ key: 'JobLotDescription', caption: 'JobLotDescription' }], textKey: 'JobLotDescription', onSelect: availableStock });
        cb.pack = XCombo('CmbPackingType', { columns: [{ key: 'PackTypeDesc', caption: 'PackTypeDesc' }], textKey: 'PackTypeDesc', onSelect: availableStock });
        cb.uom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'PackUOM' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onSelect: uomChanged });
        onBlur(cb.cust, function () { saleOrderBind(); });                              // CmbSupplierCustomer_Leave
        onBlur(cb.order, function () { orderChanged(); });                              // CmbOrderNo_Leave
        onBlur(cb.item, function () { itemLeave(); });                                  // CmbItemName_Leave
        onBlur(cb.uom, calculateWeight);                                                // combitempck_Leave
    }

    /* SaleOrderBind :2250 */
    function saleOrderBind() {
        if (typeVal() === 1) {
            var c = +cb.cust.value() || 0;
            return SE.api(API + '/orders' + SE.q({ customerId: c })).then(function (rows) {
                if (rows.length > 0) cb.order.setData(rows); else cb.order.setData([]);
            }).catch(fail);
        }
        cb.order.setData([]);
        return Promise.resolve();
    }
    /* CmbOrderNo_TextChanged / _Leave -> ItemBindbyOrderId + CalculateOrderBalanceQtyandWeight */
    function orderChanged() { return itemBindByOrderId().then(calcBalance); }
    /* ItemBindbyOrderId :2295 */
    function itemBindByOrderId() {
        if (typeVal() === 1) {
            return SE.api(API + '/order-items' + SE.q({ orderId: +cb.order.value() || 0 })).then(function (rows) {
                if (rows.length > 0) cb.item.setData(rows); else cb.item.setData([]);
            }).catch(fail);
        }
        if (typeVal() === 2) { cb.item.setData(S.items); }
        return Promise.resolve();
    }
    /* CmbDeliveryOrderType_Leave :2432 (only reachable for a type other than Local) */
    function typeLeave() { if (typeVal() !== 1) { cb.item.setData(S.items); cb.wh.el.disabled = false; $('txtweight').readOnly = true; } }

    /* bindRateUomAndItemPackUom :1262 */
    function bindUoms(itemId) {
        var prev = cb.uom.text();
        cb.uom.clear();
        return SE.api(API + '/uoms' + SE.q({ itemId: itemId })).then(function (rows) {
            if (rows.length > 0) {
                cb.uom.setData(rows);
                var hit = rows.filter(function (r) { return r.UOMCode === prev; })[0];
                if (hit) cb.uom.setValue(hit.Id); else cb.uom.clear();
            } else { cb.uom.setData([]); }
        }).catch(fail);
    }
    /* CmbItemName_Leave :1347 */
    function itemLeave() {
        return bindUoms(+cb.item.value() || 0).then(function () { return availableStock(); }).then(calcBalance);
    }
    /* CmbPackUom_TextChanged :2453 */
    function uomChanged() { calculateWeight(); availableStock(); calcBalance(); }

    /* AvailableStockGetByItem :2470 */
    function availableStock() {
        var wh = +cb.wh.value() || 0, it = +cb.item.value() || 0, jb = +cb.job.value() || 0, pk = +cb.pack.value() || 0, um = +cb.uom.value() || 0;
        if (!wh && !it && !jb && !pk && !um) { $('lblBalance').textContent = '0'; return Promise.resolve(); }
        return SE.api(API + '/stock' + SE.q({ warehouseId: wh, itemId: it, jobLotId: jb, docDate: $('DocDate').value, packingTypeId: pk, uomId: um })).then(function (d) {
            $('lblBalance').textContent = d.stock > 0 ? String(d.stock) : '0';
        }).catch(fail);
    }
    /* CalculateOrderBalanceQtyandWeight :1965 */
    function calcBalance() {
        var it = cb.item.row(), um = cb.uom.row();
        if (typeVal() === 1 && (+cb.order.value() || 0) > 0 && it && um) {
            return SE.api(API + '/balance' + SE.q({ orderId: +cb.order.value(), saleOrderDetailId: ti(it.SaleOrderDetailId), itemId: +cb.item.value() || 0 })).then(function (d) {
                if (d.balWeight > 0) { $('txtBalWeight').value = String(d.balWeight); $('txtBalQty').value = String(d.balWeight / tn(um.Equivalent)); }
                else { $('txtBalWeight').value = ''; $('txtBalQty').value = ''; }
            }).catch(fail);
        }
        return Promise.resolve();
    }
    /* CalculateWeight :2004 */
    function calculateWeight() {
        var um = cb.uom.row();
        if (cb.uom.text() !== '' && $('txtqty').value !== '' && um) {
            var w = ti(um.Equivalent) * ti($('txtqty').value);
            $('txtweight').value = fmtSingle(w); $('txtGrossWeight').value = fmtSingle(w);
        } else { $('txtweight').value = '0'; $('txtGrossWeight').value = '0'; }
    }

    /* ------------------------------------------------------------------ grids */
    function makeGrids() {
        G.grd = SE.grid('grd', { footer: true, onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) { if (k === 'Add') addCopy(i); else if (k === 'Delete') deleteRow(i); },
            onEdit: gridEdit,
            cols: [
                { k: 'Id', hide: true }, { k: 'RefDocumentTypeId', hide: true }, { k: 'RefDocIdNo', hide: true }, { k: 'RefDocSubIdNo', hide: true },
                { k: 'RefPartyId', t: 'ReferenceParty', w: 140, edit: true, list: function () { return S.refParties; }, lk: 'Id', lt: 'ReferencePartyName' },
                { k: 'SupplierCustomerId', hide: true }, { k: 'SupplierCustomer', t: 'CustomerName', w: 200 }, { k: 'SaleOrderDetailId', hide: true }, { k: 'OrderId', hide: true },
                { k: 'OrderNo', t: 'OrderNo', w: 70 },
                { k: 'WareHouseId', t: 'WareHouseName', w: 140, edit: true, list: function () { return S.wh; }, lk: 'Id', lt: 'WareHouseName' },
                { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 240 },
                { k: 'JobLotId', t: 'JobLotDescription', w: 130, edit: true, list: function () { return S.job; }, lk: 'Id', lt: 'JobLotDescription' },
                { k: 'PackingTypeId', t: 'PackingType', w: 110, edit: true, list: function () { return S.pack; }, lk: 'Id', lt: 'PackTypeDesc' },
                { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'PackUom', w: 80 }, { k: 'PUomEquivalent', hide: true },
                { k: 'ItemQty', t: 'ItemQty', w: 90, f: 'amt', sum: true }, { k: 'Weight', hide: true, sum: true },
                { k: 'LoadQty', t: 'LoadQty', w: 90, f: 'amt', sum: true, edit: true }, { k: 'LoadWeight', t: 'LoadWeight', w: 100, f: 'amt', sum: true },
                { k: 'GrossWeight', t: 'GrossWeight', w: 100, f: 'n3' }, { k: 'Remarks', t: 'Remarks', w: 200, edit: true },
                { k: 'AvailableStock', t: 'AvailableStock', w: 110, f: 'n3', sum: true },
                { k: 'Add', t: '+', w: 28, btn: '+' }, { k: 'Delete', t: 'X', w: 28, btn: 'X' }] });
        G.hist = SE.grid('grdhistory', { frozen: 3, onDbl: function (r) { histDbl(r); }, onSel: histSelected,
            onBtn: function (k, r) { histButton(k, r); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') SS.showAttachments(API, r.Id); },
            cols: [
                { k: 'Print', t: 'Print', w: 50, btn: 'Print' }, { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'SaveAs', t: 'SaveAs', w: 50, btn: 'SaveAs' }, { k: 'Id', hide: true },
                { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' }, { k: 'OrderType', t: 'OrderType', w: 90 }, { k: 'ManualNo', t: 'ManualNo', w: 80 },
                { k: 'VehicleType', t: 'VehicleType', w: 110 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
                { k: 'DoQty', t: 'DoQty', w: 80, f: 'amt', sum: true }, { k: 'GrossWeight', t: 'GrossWeight', w: 100, f: 'amt', sum: true }, { k: 'NetWeight', t: 'NetWeight', w: 100, f: 'amt', sum: true },
                { k: 'EntryUser', t: 'EntryUser', w: 110 }, { k: 'EntryDate', t: 'EntryDate', w: 85, f: 'sdate' }, { k: 'ModifyUser', t: 'ModifyUser', w: 110 }, { k: 'ModifyDate', t: 'ModifyDate', w: 85, f: 'sdate' },
                { k: 'IsApproved', t: 'IsApproved', w: 100 }, { k: 'ApprovedUser', t: 'ApprovedUser', w: 110 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 85, f: 'sdate' },
                { k: 'Remarks', t: 'Remarks', w: 200 }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }] });
        G.hd = SE.grid('GridDetail', { footer: false,
            cols: [{ k: 'CustomerName', t: 'CustomerName', w: 200 }, { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'Warehouse', t: 'Warehouse', w: 120 }, { k: 'ItemName', t: 'ItemName', w: 240 },
                { k: 'JobLot', t: 'JobLot', w: 110 }, { k: 'PackingType', t: 'PackingType', w: 110 }, { k: 'PackUom', t: 'PackUom', w: 80 }, { k: 'ItemQty', t: 'ItemQty', w: 90 },
                { k: 'Weight', t: 'Weight', w: 90 }, { k: 'GrossWeight', t: 'GrossWeight', w: 100 }, { k: 'Remarks', t: 'Remarks', w: 200 }] });
    }
    function setGridDec() { G.grd.spec.dec = S.dec; G.hist.spec.dec = S.dec; G.grd.refresh(); G.hist.refresh(); }

    /* grd_CellUpdated :702 */
    function gridEdit(r, k, v, i) {
        if (k === 'Remarks') { r[k] = v; return; }
        if (k === 'LoadQty') {
            r.LoadQty = tn(v);
            if (String(r.PUomEquivalent == null ? '' : r.PUomEquivalent) !== '' && String(r.LoadQty) !== '') {
                var w = r.LoadQty * tn(r.PUomEquivalent);
                r.LoadWeight = w; r.GrossWeight = w;
            }
            G.grd.refresh(); return;
        }
        r[k] = v === '' ? 0 : +v;
        if (k === 'WareHouseId' || k === 'JobLotId') {
            SE.api(API + '/stock' + SE.q({ warehouseId: ti(r.WareHouseId), itemId: ti(r.ItemId), jobLotId: ti(r.JobLotId), docDate: $('DocDate').value, packingTypeId: ti(r.PackingTypeId), uomId: ti(r.PackUomId) }))
                .then(function (d) { r.AvailableStock = d.stock; G.grd.refresh(); }).catch(fail);
        }
        G.grd.refresh();
    }
    function addCopy(i) {                                                              // grd_ColumnButtonClick "Add"
        var src = G.grd.rows()[i]; if (!src) return;
        var c = {}; KEYS.forEach(function (k) { c[k] = src[k]; });
        if (visible('btnupdate')) c.Id = 0;
        G.grd.rows().push(c); G.grd.refresh();
    }
    function deleteRow(i) {                                                            // grd_ColumnButtonClick "Delete"
        var r = G.grd.rows()[i]; if (!r) return;
        function done() { G.grd.removeAt(i); getSaleOrderIds(); }
        if (ti(r.Id) > 0) {
            return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) { if (!yes) return; S.remove.push(toLine(r)); done(); });
        }
        done();
    }
    function getSaleOrderIds() {                                                       // GetSaleOrderIds :2163
        S.ordIds = ''; S.ordMainIds = '';
        G.grd.rows().forEach(function (r) { S.ordIds += ',' + ti(r.SaleOrderDetailId); S.ordMainIds += ',' + ti(r.OrderId); });
    }
    function toLine(r) {
        return { id: ti(r.Id), refDocumentTypeId: ti(r.RefDocumentTypeId), refDocIdNo: ti(r.RefDocIdNo), refDocSubIdNo: ti(r.RefDocSubIdNo), refPartyId: ti(r.RefPartyId),
            supplierCustomerId: ti(r.SupplierCustomerId), saleOrderDetailId: ti(r.SaleOrderDetailId), orderId: ti(r.OrderId), wareHouseId: ti(r.WareHouseId), itemId: ti(r.ItemId),
            jobLotId: ti(r.JobLotId), packingTypeId: ti(r.PackingTypeId), packUomId: ti(r.PackUomId), itemQty: tn(r.ItemQty), loadQty: tn(r.LoadQty), loadWeight: tn(r.LoadWeight),
            grossWeight: tn(r.GrossWeight), remarks: r.Remarks == null ? '' : String(r.Remarks) };
    }

    /* ------------------------------------------------------------------ detail entry */
    /* FormValidationDetail :437 */
    function validateDetail() {
        function stop(m, c) { return msg(m).then(function () { if (c) c.focus ? c.focus() : $(c).focus(); return false; }); }
        if (!cb.cust.row()) return stop('Customer field is required', cb.cust);
        if (typeVal() === 1 && !cb.order.row()) return stop('OrderNo field is required', cb.order);
        if (!cb.wh.row()) return stop('WareHouseName field is required', cb.wh);
        if (!cb.item.row()) return stop('ItemName field is required', cb.item);
        if (!cb.uom.row()) return stop('PackUOM field is required', cb.uom);
        if (!cb.pack.row()) return stop('Packingtype field is required', cb.pack);
        if (!cb.job.row()) return stop('JobLot field is required', cb.job);
        if (tn($('txtqty').value) === 0) return stop('ItemQty field is required', 'txtqty');
        if (tn($('txtweight').value) === 0) return stop('Weight field is required', 'txtweight');
        if (tn($('txtGrossWeight').value) === 0) return stop('GrossWeight field is required', 'txtGrossWeight');
        return Promise.resolve(true);
    }
    /* btnplus_Click :502 */
    function plus() {
        return validateDetail().then(function (ok) {
            if (!ok) return;
            if (typeVal() === 1) {
                var it = cb.item.row(), um = cb.uom.row();
                G.grd.rows().push({ Id: 0, RefDocumentTypeId: 0, RefDocIdNo: 0, RefDocSubIdNo: 0, RefPartyId: 0, SupplierCustomerId: cb.cust.value(), SupplierCustomer: cb.cust.text().trim(),
                    SaleOrderDetailId: it.SaleOrderDetailId, OrderId: cb.order.value(), OrderNo: ti(cb.order.text().trim()), WareHouseId: cb.wh.value(), ItemId: cb.item.value(),
                    ItemName: cb.item.text().trim(), JobLotId: cb.job.value(), PackingTypeId: cb.pack.value(), PackUomId: cb.uom.value(), PackUom: cb.uom.text().trim(),
                    PUomEquivalent: um.Equivalent, ItemQty: tn($('txtqty').value.trim()), Weight: tn($('txtweight').value.trim()), LoadQty: tn($('txtqty').value.trim()),
                    LoadWeight: tn($('txtweight').value.trim()), GrossWeight: tn($('txtGrossWeight').value.trim()), Remarks: $('txtremarksdetail').value.trim(), AvailableStock: 0 });
            } else {
                /* type 2: the desktop's table.Rows.Add(...) lists the values in another column order and the DataTable refuses it */
                return msg("Couldn't store <" + cb.cust.text().trim() + "> in SupplierCustomerId Column.  Expected type is Int32.");
            }
            G.grd.refresh();
            resetDetail();
        });
    }
    /* ResetDetail :1433 */
    function resetDetail() {
        cb.cust.focus();
        $('txtRefDocTypeId').value = ''; $('txtDocNoId').value = ''; $('txtSubNoId').value = '';
        cb.order.clear(); cb.item.clear(); cb.uom.clear(); $('txtqty').value = ''; $('txtweight').value = ''; $('txtGrossWeight').value = '0';
        $('txtBalQty').value = '0'; $('txtBalWeight').value = '0'; cb.pack.clear(); $('txtremarksdetail').value = ''; cb.wh.clear(); cb.job.clear();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        if (typeVal() === 1) { cb.order.setData([]); cb.item.setData([]); }
        $('lblBalance').textContent = '0';
    }
    /* grd_DoubleClick :828 */
    function editRow(i) {
        var r = G.grd.rows()[i]; if (!r) return Promise.resolve();
        S.updateIdx = i;
        cb.cust.setData([{ Id: r.SupplierCustomerId, CompanyName: r.SupplierCustomer }]); cb.cust.setValue(r.SupplierCustomerId);
        cb.order.setData([{ SaleOrderId: r.OrderId, SaleOrderNo: r.OrderNo }]); cb.order.setValue(r.OrderId);
        cb.item.setData([{ Id: r.ItemId, ItemName: r.ItemName, SaleOrderDetailId: r.SaleOrderDetailId }]); cb.item.setValue(r.ItemId);
        return SE.api(API + '/uoms' + SE.q({ itemId: ti(r.ItemId) })).then(function (rows) {
            cb.uom.setData(rows); cb.uom.setValue(r.PackUomId);
            $('txtRefDocTypeId').value = String(r.RefDocumentTypeId); $('txtDocNoId').value = String(r.RefDocIdNo); $('txtSubNoId').value = String(r.RefDocSubIdNo);
            cb.pack.setValue(r.PackingTypeId);
            $('txtBalQty').value = String(r.ItemQty); $('txtBalWeight').value = String(r.Weight);
            $('txtqty').value = String(r.LoadQty); $('txtweight').value = String(r.LoadWeight); $('txtGrossWeight').value = String(r.GrossWeight);
            cb.wh.setValue(r.WareHouseId); cb.job.setValue(r.JobLotId); $('txtremarksdetail').value = r.Remarks == null ? '' : String(r.Remarks);
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
            return availableStock().then(calcBalance);
        }).then(function () { cb.cust.focus(); }).catch(fail);
    }
    /* btnUpdateDetail_Click :783 */
    function updateDetail() {
        return validateDetail().then(function (ok) {
            if (!ok) return;
            var r = G.grd.rows()[S.updateIdx]; if (!r) return;
            var it = cb.item.row(), um = cb.uom.row();
            r.ItemQty = tn($('txtqty').value); r.Weight = tn($('txtweight').value);
            if (typeVal() === 1) r.SaleOrderDetailId = it.SaleOrderDetailId;
            r.RefDocumentTypeId = ti($('txtRefDocTypeId').value); r.RefDocIdNo = ti($('txtDocNoId').value); r.RefDocSubIdNo = ti($('txtSubNoId').value);
            r.SupplierCustomerId = cb.cust.value(); r.SupplierCustomer = cb.cust.text(); r.WareHouseId = cb.wh.value();
            r.OrderId = cb.order.value(); r.OrderNo = cb.order.value();                // the desktop stores the order id in OrderNo as well
            r.ItemId = cb.item.value(); r.ItemName = cb.item.text(); r.JobLotId = cb.job.value(); r.PackUomId = cb.uom.value(); r.PackUom = cb.uom.text();
            r.PUomEquivalent = um.Equivalent; r.PackingTypeId = cb.pack.value();
            r.LoadQty = tn($('txtqty').value); r.GrossWeight = tn($('txtGrossWeight').value); r.LoadWeight = tn($('txtweight').value); r.Remarks = $('txtremarksdetail').value;
            G.grd.refresh();
            show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            resetDetail();
            cb.cust.setData(S.customers);                                              // SupplierNameFill
        });
    }
    function cancelDetail() { resetDetail(); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); }

    /* DocDate_ValueChanged :2343 */
    function docDateChanged() {
        var rows = G.grd.rows(); if (!rows.length) return;
        SE.api(API + '/current-stock', { method: 'POST', body: { docDate: $('DocDate').value, rows: rows.map(function (r) { return { itemId: ti(r.ItemId), warehouseId: ti(r.WareHouseId), jobLotId: ti(r.JobLotId) }; }) } })
            .then(function (d) { rows.forEach(function (r, i) { r.AvailableStock = d.stocks[i]; }); G.grd.refresh(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ sale order loader (btnLoadSaleOrder_Click :2182 / btnLoaderView2_Click :2210) */
    function openLoader(mainDetail) {
        if (cb.otype.text() !== 'Local') return msg('Please Select Other DeliveryOrderType');
        var o = { API: API, mainDetail: mainDetail, documentTypeId: DOC };
        if (S.ordIds !== '') { o.orderIds = S.ordIds; o.orderMainIds = S.ordMainIds; }
        return SS.loadSaleOrder(o).then(function (ids) {
            return loadInGridDetail(ids).then(function () { cb.otype.el.disabled = true; var x = cb.otype.el.__dtcombo; if (x && x.syncFromSelect) x.syncFromSelect(); });
        }).catch(fail);
    }
    /* LoadInGridDetail :2072 */
    function loadInGridDetail(ids) {
        if (!ids || ids.length === 0 || ti(ids[0]) === 0) return Promise.resolve();
        var gdn = ''; ids.forEach(function (x) { gdn += ',' + ti(x); });
        return SE.api(API + '/loader/rows' + SE.q({ detailIds: gdn })).then(function (g) {
            var rows = g.rows || [], cols = g.cols || [];
            if (rows.length === 0) return reset();
            var chain = Promise.resolve();
            rows.forEach(function (dr, j) {
                chain = chain.then(function () {
                    var dup = G.grd.rows().some(function (x) { return ti(x.SaleOrderDetailId) === ti(dr.SaleOrderDetailId); });
                    if (dup) return;
                    var v = cols.map(function (c) { return dr[c]; });                   // dr.ItemArray -> the table's columns by position
                    return SE.api(API + '/stock' + SE.q({ warehouseId: ti(dr.WareHouseId), itemId: ti(dr.ItemId), jobLotId: ti(dr.JobLotId), docDate: $('DocDate').value, packingTypeId: ti(dr.PackingTypeId), uomId: ti(dr.ItemUOMId) }))
                        .then(function (d) {
                            if (d.stock > 0) $('lblBalance').textContent = String(d.stock);
                            var t = {}; KEYS.forEach(function (k, n) { t[k] = n < v.length ? v[n] : null; });
                            G.grd.rows().push(t);
                            var tr = G.grd.rows()[j]; if (tr) tr.AvailableStock = d.stock;           // table.Rows[j]["AvailableStock"] (row index j of the grid)
                        });
                });
            });
            return chain.then(function () { G.grd.refresh(); getSaleOrderIds(); });
        });
    }

    /* ------------------------------------------------------------------ Reset :1390 */
    function reset() {
        S.files = []; S.removed = []; S.existing = []; S.remove = []; S.ordIds = ''; S.ordMainIds = ''; S.id = 0;
        cb.otype.el.disabled = false; var x = cb.otype.el.__dtcombo; if (x && x.syncFromSelect) x.syncFromSelect();
        S.approved = false;
        $('DocDate').focus();
        cb.item.clear(); cb.uom.clear(); $('txtqty').value = ''; $('txtweight').value = ''; cb.pack.clear(); $('txtremarks').value = '';
        $('txtRefDocTypeId').value = ''; $('txtDocNoId').value = ''; $('txtSubNoId').value = ''; $('txtVehicleNo').value = ''; cb.veh.clear();
        $('txtGrossWeight').value = '0'; $('txtBalQty').value = '0'; $('txtBalWeight').value = '0';
        show('btnsave', true); show('btnupdate', false); show('btnSaveAs', false); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        G.grd.setRows([]);
        return SE.api(API + '/next-no').then(function (d) { if (d.nextNo > 0) $('txtdocno').value = String(d.nextNo); }).catch(fail);   // DocumentNoFill
    }
    /* btnRefresh_Click :1371 */
    function refresh() {
        return SE.api(API + '/refresh').then(function (d) {
            S.refParties = d.refParties; S.pack = d.packingTypes; S.wh = d.warehouses; S.job = d.jobLots;
            var keep = [cb.pack, cb.wh, cb.job];
            cb.pack.setData(S.pack); cb.wh.setData(S.wh); cb.job.setData(S.job);
            G.grd.refresh();
            return bindUoms(+cb.item.value() || 0);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Save (Insert :912) */
    function formValidation() {
        function stop(m, c) { return msg(m).then(function () { if (c.focus) c.focus(); else $(c).focus(); return false; }); }
        var no = field('txtdocno');
        if (no === '' || no === '0') return stop('DocNo Field is Required', 'txtdocno');
        if (!cb.otype.row()) return stop('DeliveryOrderType Field is Required', cb.otype);
        if (!cb.veh.row()) return stop('Vehicle type Field is Required', cb.veh);
        var vn = field('txtVehicleNo');
        if (vn === '' || vn === '0') return stop('VehicleNo Field is Required', 'txtVehicleNo');
        return Promise.resolve(true);
    }
    function insert(id) {
        var rows = G.grd.rows();
        if (rows.length === 0) return SE.alert('Grid Record Not Found', 'Database Error');
        return formValidation().then(function (ok) {
            if (!ok) return;
            return SE.ask(id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                for (var i = 0; i < rows.length; i++) if (ti(rows[i].OrderId) === 0) return SE.alert('SaleOrder No Not Found', 'Database Error');
                var body = {
                    id: id, docDate: $('DocDate').value, docNo: field('txtdocno'), deliveryOrderType: typeVal(), manualNo: field('txtmanualno'), remarks: field('txtremarks'),
                    vehicleTypeId: +cb.veh.value() || 0, vehicleNo: field('txtVehicleNo'), lines: rows.map(toLine), removed: id > 0 ? S.remove : [],
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removed }
                };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return SE.alert(r.message).then(function () {
                        var preview = $('ChkPrint').checked;
                        return reset().then(function () { if (preview) print(r.id); });
                    });
                }).catch(function (e) { return SE.alert(e.message, 'Database Error'); });
            });
        });
    }
    function print(id) {
        if (!(id > 0)) return SE.alert('No Record Found For Display');
        SE.printRpt('1513-DeliveryOrderSlip.rpt', { id: id, documentTypeId: DOC });       // CommonServices.DeliveryOrderSlipForSteel(id, 1506)
    }

    /* ------------------------------------------------------------------ ReadById :1564 */
    function readById(id) {
        S.id = id; S.remove = [];
        return SE.api(API + '/' + id).then(function (d) {
            var h = d.head;
            tabs.select('tabPage1');
            $('DocDate').value = SE.dateInput(h.DocDate); $('txtdocno').value = h.DocNo == null ? '' : h.DocNo; $('txtremarks').value = h.LoadingInstructions || '';
            cb.otype.setValue(h.DeliveryOrderType); typeLeave();
            cb.veh.setValue(h.VehicleTypeId);                                          // the desktop sets the combo text to the id (no match); the web selects the vehicle type
            cb.otype.el.disabled = false;
            $('txtVehicleNo').value = h.VehicleNo || '';
            S.approved = !!h.IsApproved;
            var rows = (d.lines || []).map(function (l) {
                return { Id: l.Id, RefDocumentTypeId: l.RefDocumentTypeId, RefDocIdNo: l.RefDocIdNo, RefDocSubIdNo: l.RefDocSubIdNo, RefPartyId: l.RefPartyId, SupplierCustomerId: l.SupplierCustomerId,
                    SupplierCustomer: l.CustomerName, SaleOrderDetailId: l.SaleOrderDetailId, OrderId: l.SaleOrderId, OrderNo: l.OrderNo, WareHouseId: l.WarehouseId, ItemId: l.ItemId,
                    ItemName: l.ItemName, JobLotId: l.JobLotId, PackingTypeId: l.PackingTypeId, PackUomId: l.PackUomId, PackUom: l.PackUom, PUomEquivalent: l.PackUomEquivalent,
                    ItemQty: l.DoQty, Weight: l.DoWeight, LoadQty: l.LoadingQty, LoadWeight: l.LoadingWeight, GrossWeight: l.GrossWeight, Remarks: l.LoadingRemarks, AvailableStock: null };
            });
            G.grd.setRows(rows);
            S.existing = d.attachments || []; S.files = []; S.removed = [];
            show('btnsave', false); show('btnupdate', true); show('btnSaveAs', false);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history (gridhistoryfill :1603) */
    function showHistory() {
        var type = $('rdentrydate').checked ? 'entry' : $('rdmodifydate').checked ? 'modify' : $('rdapproveddate').checked ? 'approved' : 'document';
        var q = { dateType: type, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
                  fromNo: ti($('FromDocNoHistory').value), toNo: ti($('ToDocNoHistory').value) };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            if (rows.length > 0) G.hist.setRows(rows); else { G.hist.setRows([]); G.hd.setRows([]); }
        }).catch(fail);
    }
    function resetHistory() {                                                          // btnResetHistory_Click :1547
        $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today(); $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        G.hist.setRows([]); G.hd.setRows([]);
    }
    function histDbl(r) { S.id = r.Id; return readById(r.Id).then(typeLeave); }          // grdhistory_DoubleClick
    function histButton(k, r) {                                                         // grdhistory_ColumnButtonClick
        if (k === 'Edit') return reset().then(function () { return readById(r.Id); });
        if (k === 'Print') return print(r.Id);
        if (k === 'SaveAs') return reset().then(function () { return readById(r.Id); }).then(function () { show('btnsave', false); show('btnupdate', false); show('btnSaveAs', true); });
    }
    function histSelected(r) {                                                          // grdhistory_SelectionChanged
        var seq = ++S.histSeq;
        if (!r) { G.hd.setRows([]); return; }
        SE.api(API + '/' + r.Id + '/lines', { quiet: true }).then(function (rows) {
            if (seq !== S.histSeq) return;
            G.hd.setRows(rows.map(function (l) { return { CustomerName: l.CustomerName, OrderNo: l.OrderNo, Warehouse: l.WareHouseCode, ItemName: l.ItemName, JobLot: l.JobLot,
                PackingType: l.PackingType, PackUom: l.PackUom, ItemQty: l.DoQty, Weight: l.DoWeight, GrossWeight: l.GrossWeight, Remarks: l.LoadingRemarks }; }));
        }).catch(function (e) { if (seq === S.histSeq) SE.alert(e.message, 'Error Message'); });
    }

    /* ------------------------------------------------------------------ shortcut keys (PurchsaeOrder_KeyDown :1468) */
    function onKey(e) {
        SS.enterTab(e);
        var k = (e.key || '').toLowerCase();
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tabs.index() === 1) tabs.select('tabPage1'); else tabs.select('tabPage2'); return; }
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (e.ctrlKey) e.preventDefault(); if (window.history.length > 1) window.history.back(); return; }
        if (e.ctrlKey && k === 's' && tabs.index() === 0) { e.preventDefault(); if (visible('btnsave')) doSave(); }
        else if (e.ctrlKey && k === 'n' && tabs.index() === 0) { e.preventDefault(); reset(); }
        else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (visible('btnupdate')) doUpdate(); }
    }
    function doSave() { return insert(0); }                                              // btnsave_Click / btnSaveAs_Click: Id = 0; Insert()
    function doUpdate() {                                                                // btnupdate_Click
        if (S.id === 0) return SE.alert('Record Not Update because RecId Not Found', 'Database Error');
        return insert(S.id);
    }

    /* ------------------------------------------------------------------ load (PurchsaeOrder_Load :318) */
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') $('FromDateHistory').focus(); else $('ToDateHistory').focus(); });
        SE.digitsOnly($('FromDocNoHistory')); SE.digitsOnly($('ToDocNoHistory')); SE.upper($('txtVehicleNo'));
        $('txtqty').addEventListener('keypress', function (e) {                         // CommonServices.OnlytextdecimelFunction
            if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault();
            else if (e.key === '.' && $('txtqty').value.indexOf('.') >= 0) e.preventDefault();
        });
        makeGrids();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {}; S.dec = d.decimals || 0; S.refParties = d.refParties; S.pack = d.packingTypes; S.wh = d.warehouses; S.job = d.jobLots; S.customers = d.customers; S.items = d.items;
            setGridDec();
            if (d.nextNo > 0) $('txtdocno').value = String(d.nextNo);
            cb.pack.setData(S.pack); cb.wh.setData(S.wh); cb.job.setData(S.job); cb.veh.setData(d.vehicleTypes);
            cb.cust.setData(S.customers); cb.item.setData(S.items); cb.otype.setData(d.types);
            $('DocDate').value = SE.today(); $('ChkPrint').checked = true;
            $('btnsave').disabled = !S.rights.save; $('btnupdate').disabled = !S.rights.update; $('btnprint').disabled = !S.rights.print; $('btnDelete').disabled = !S.rights['delete'];
            show('btnupdate', false); show('btnSaveAs', false); show('btnsave', true); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            $('FromDateHistory').value = SE.addDays(-d.historyDays); $('ToDateHistory').value = SE.today();
            $('txtGrossWeight').value = $('txtGrossWeight').value || '0'; $('txtBalQty').value = $('txtBalQty').value || '0'; $('txtBalWeight').value = $('txtBalWeight').value || '0';
            $('DocDate').focus();
        }).catch(fail);
        wire();
        var rec = SE.param('record'); if (rec) setTimeout(function () { readById(+rec); }, 600);
    }

    function wire() {
        $('btnnew').onclick = reset; $('btnRefresh').onclick = refresh;
        $('btnsave').onclick = doSave; $('btnSaveAs').onclick = doSave; $('btnupdate').onclick = doUpdate;
        $('btnattachment').onclick = function () { SS.attachmentDialog(S, API, function () { return S.id; }); };
        $('btnprint').onclick = function () { print(S.id); };
        $('btnLoadSaleOrder').onclick = function () { openLoader(true); }; $('btnLoaderView2').onclick = function () { openLoader(false); };
        $('btnDefineReferenceParties').onclick = function () { window.open('/party-processing/reference-parties', '_blank'); };
        $('btnDelete').onclick = function () { };                                       // btnDelete_Click :2536 is empty
        $('btnplus').onclick = plus; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelUpdateDetial').onclick = cancelDetail;
        $('btnShow').onclick = showHistory; $('btnResetHistory').onclick = resetHistory;
        $('DocDate').addEventListener('change', docDateChanged);
        $('txtqty').addEventListener('input', calculateWeight);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
