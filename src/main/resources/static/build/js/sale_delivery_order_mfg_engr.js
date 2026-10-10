/*
 * Screen 808  frmDeliveryOrderEngr  (Architecture.WinApp.Mfg.frmDeliveryOrderEngr, Sale Engr module 134, document type 1657)
 * Page script. Desktop methods are named in the comments (frmDeliveryOrderEngr.cs). Server: /sale/engr/mfg/delivery-order/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/mfg/delivery-order/api', DOC = 1657;
    var S = {
        id: 0, approved: false, rights: {}, remove: [], updateIdx: -1,
        files: [], removedAtt: [], existing: [],
        items: [],            // dtitem (Id, ItemName, ItemCode, ItemCategoryId, FinishWeight, OrderDetailId)
        ref: [], wh: [], cust: [], cat: [], cast: [],
        ordIds: '', ordMainIds: '', histRows: [], histDays: 3
    };
    var cb = {}, G = {}, tabs;

    function fail(e) { return SE.dbError(e); }
    function msg(t, cap) { return SE.alert(t, cap); }
    function field(id) { return ($(id).value || '').trim(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function keepSet(c, v) { if (v > 0 && !c.setValue(v)) c.clear(); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.cust = XCombo('CmbSupplierCustomer', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420, onLeave: customerLeave });
        cb.ref = XCombo('CmbReferenceParty', { columns: [{ key: 'ReferencePartyName', caption: 'ReferencePartyName' }], textKey: 'ReferencePartyName', popupWidth: 300 });
        cb.order = XCombo('CmbOrderNo', { columns: [{ key: 'SaleOrderNo', caption: 'SaleOrderNo' }], valueKey: 'SaleOrderId', textKey: 'SaleOrderNo', onLeave: orderLeave });
        cb.wh = XCombo('CmbWareHouse', { columns: [{ key: 'WareHouseName', caption: 'WareHouseName' }], textKey: 'WareHouseName' });
        cb.cat = XCombo('CmbCategory', { columns: [{ key: 'CategoryDescription', caption: 'ItemCategory' }], textKey: 'CategoryDescription', onLeave: function () { itemDetailBind(+cb.cat.value() || 0); } });
        cb.item = XCombo('CmbItemName', { columns: [{ key: '_t', caption: 'Item Name' }, { key: '_o', caption: 'Item Code' }], textKey: '_t', popupWidth: 500, onLeave: itemLeave });
        cb.uom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'PackUom' }, { key: 'Equivalent', caption: 'Equivalent' }], textKey: 'UOMCode', onLeave: calcBalance });
        cb.variant = XCombo('CmbVariantDescription', { columns: [{ key: 'VariantDescription', caption: 'VariantDescription' }], valueKey: 'VariantId', textKey: 'VariantDescription' });
        cb.casting = XCombo('CmbCastingTypeDetail', { columns: [{ key: 'LookupName', caption: 'Casting Type' }], textKey: 'LookupName', popupWidth: 350 });
        cb.veh = XCombo('cmbvehicletype', { columns: [{ key: 'VehicleDescription', caption: 'Vehicle Description' }], textKey: 'VehicleDescription' });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Customer', caption: 'Party' }], textKey: 'Customer', popupWidth: 380 });
    }

    /* CustomerNameFill / WareHouseFill / CmbReferencePartiesFill / ItemCategoryFill / CastingTypeFill / VehicleTypefill: bind, then keep the previous value when still listed */
    function fillCustomers(rows) { var k = +cb.cust.value() || 0; S.cust = rows || []; cb.cust.setData(S.cust); keepSet(cb.cust, k); }
    function fillRef(rows) { var k = +cb.ref.value() || 0; S.ref = rows || []; cb.ref.setData(S.ref); keepSet(cb.ref, k); }
    function fillWarehouses(rows) { var k = +cb.wh.value() || 0; S.wh = rows || []; cb.wh.setData(S.wh); keepSet(cb.wh, k); }
    function fillCategories(rows) { var k = +cb.cat.value() || 0; S.cat = rows || []; cb.cat.setData(S.cat); keepSet(cb.cat, k); }
    function fillCasting(rows) { var k = +cb.casting.value() || 0; S.cast = rows || []; cb.casting.setData(S.cast); keepSet(cb.casting, k); }
    function fillVehicles(rows) { var k = +cb.veh.value() || 0; cb.veh.setData(rows || []); keepSet(cb.veh, k); }

    /* ItemDetailBind(CategoryId): the items of the loaded order, filtered by the category; the display column follows the Name / Code radio */
    function bindItemRows(rows) {
        var byName = $('rdSearchByName').checked;
        cb.item.setData(rows.map(function (r) {
            return { Id: r.Id, _t: byName ? r.ItemName : r.ItemCode, _o: byName ? r.ItemCode : r.ItemName, ItemName: r.ItemName, ItemCode: r.ItemCode,
                ItemCategoryId: r.ItemCategoryId, FinishWeight: r.FinishWeight, OrderDetailId: r.OrderDetailId };
        }));
    }
    function itemDetailBind(categoryId) {
        var keep = +cb.item.value() || 0;
        var rows = categoryId > 0 ? S.items.filter(function (r) { return +r.ItemCategoryId === categoryId; }) : S.items;
        if (rows.length > 0) { bindItemRows(rows); keepSet(cb.item, keep); }
        else { cb.item.setData([]); cb.item.clear(); }
    }
    /* rdSearchByName_CheckedChanged */
    function rdSearchChanged() {
        if (!S.items.length) return;
        var id = +cb.item.value() || 0;
        bindItemRows(S.items);
        cb.item.setValue(id);
        cb.item.focus();
    }

    /* SaleOrderBind (CmbSupplierCustomer_Leave) */
    function customerLeave() {
        var c = +cb.cust.value() || 0;
        return loadOrders(c);
    }
    function loadOrders(customerId) {
        var keep = +cb.order.value() || 0;
        return SE.api(API + '/orders' + SE.q({ customerId: customerId, recId: S.id })).then(function (rows) {
            if (rows.length) { cb.order.setData(rows); keepSet(cb.order, keep); }
            else { cb.order.setData([]); cb.order.clear(); }
        }).catch(fail);
    }
    /* CmbOrderNo_Leave: ItemBindbyOrderId when an order is selected, then CalculateOrderBalanceQtyandWeight */
    function orderLeave() {
        var o = +cb.order.value() || 0;
        if (cb.order.row() && o > 0) {
            return SE.api(API + '/order-items' + SE.q({ orderId: o })).then(function (rows) {
                S.items = rows;
                itemDetailBind(+cb.cat.value() || 0);                    // CmbCategory_Leave
                return calcBalance();
            }).catch(fail);
        }
        return calcBalance();
    }
    /* CmbItemName_Leave: bindRateUomAndItemPackUom, bindvarientunit, FillItemFinishWeight, CalculateOrderBalanceQtyandWeight */
    function itemLeave() {
        var id = +cb.item.value() || 0;
        return bindUoms(id).then(function () { return bindVariants(id); }).then(function () {
            var r = cb.item.row();
            if (r && id > 0) $('txtweight').value = SE.num(SE.toNum(r.FinishWeight), 2);       // FinishWeight.ToString("#,##.##")
            return calcBalance();
        }).catch(fail);
    }
    function bindUoms(itemId) {
        var prev = cb.uom.text().trim();
        cb.uom.clear();
        if (!(itemId > 0)) { cb.uom.setData([]); return Promise.resolve(); }
        return SE.api(API + '/uoms' + SE.q({ itemId: itemId })).then(function (rows) {
            cb.uom.setData(rows);
            if (rows.length) {
                var hit = rows.filter(function (r) { return r.UOMCode === prev; })[0];
                if (hit) cb.uom.setValue(hit.Id); else cb.uom.clear();
            }
        });
    }
    function bindVariants(itemId) {
        var prev = cb.variant.text();
        if (!(itemId > 0)) { cb.variant.setData([]); return Promise.resolve(); }
        return SE.api(API + '/variants' + SE.q({ itemId: itemId })).then(function (rows) {
            cb.variant.setData(rows);
            if (rows.length) {
                if (prev !== '') {
                    var hit = rows.filter(function (r) { return r.VariantDescription === prev; })[0];
                    if (hit) cb.variant.setValue(hit.VariantId); else cb.variant.clear();
                }
            } else cb.variant.clear();
        });
    }
    /* CalculateOrderBalanceQtyandWeight */
    function calcBalance() {
        var o = +cb.order.value() || 0, it = +cb.item.value() || 0, u = +cb.uom.value() || 0, ir = cb.item.row();
        if (o > 0 && ir && cb.uom.row() && it !== 0 && u !== 0) {
            return SE.api(API + '/balance' + SE.q({ orderId: o, orderDetailId: +ir.OrderDetailId || 0, itemId: it })).then(function (r) {
                var b = Number(r.balWeight) || 0;
                $('txtBalQty').value = b > 0 ? String(b) : '';
            }).catch(fail);
        }
        return Promise.resolve();
    }

    /* ------------------------------------------------------------------ grids */
    function refName(id) { for (var i = 0; i < S.ref.length; i++) if (String(S.ref[i].Id) === String(id)) return S.ref[i].ReferencePartyName; return ''; }
    function makeGrids() {
        /* grdSettings: only LoadQty is a text edit; RefPartyId / WareHouseId are drop-down lists; Add / Delete buttons are frozen at the left */
        G.grd = SE.grid('grd', { footer: false, frozen: 2, dec: 3,
            onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) { if (k === 'Add') addCopy(i); else if (k === 'Delete') deleteRow(i); },
            onEdit: function (r, k, v) {
                if (k === 'LoadQty') { var n = Number(String(v).replace(/,/g, '')); if (isFinite(n)) r[k] = n; }
                else if (k === 'RefPartyId' || k === 'WareHouseId') r[k] = v === '' ? 0 : +v;
            },
            cols: [
                { k: 'Add', t: '+', w: 20, btn: '+' }, { k: 'Delete', t: 'X', w: 20, btn: 'X' },
                { k: 'Id', hide: true }, { k: 'SupplierCustomerId', hide: true },
                { k: 'CustomerName', t: 'CustomerName', w: 170 },
                { k: 'RefPartyId', t: 'ReferenceParty', w: 150, edit: true, list: function () { return S.ref; }, lk: 'Id', lt: 'ReferencePartyName' },
                { k: 'OrderId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 80 },
                { k: 'WareHouseId', t: 'WareHouseName', w: 150, edit: true, list: function () { return S.wh; }, lk: 'Id', lt: 'WareHouseName' },
                { k: 'OrderDetailId', hide: true }, { k: 'ItemId', hide: true },
                { k: 'ItemCode', t: 'ItemCode', w: 80 }, { k: 'Item', t: 'Item', w: 150 },
                { k: 'ItemUOMId', hide: true }, { k: 'ItemUOM', t: 'ItemUOM', w: 60 }, { k: 'UomEquivalent', hide: true },
                { k: 'VariantId', hide: true }, { k: 'VariantDescription', t: 'Modal Description', w: 100 },
                { k: 'CastingTypeId', hide: true }, { k: 'CastingType', t: 'CastingType', w: 90 },
                { k: 'Remarks', t: 'Specification / Remarks', w: 100 },
                { k: 'QTY', hide: true },
                { k: 'Weight', t: 'Weight', w: 80, f: 'n3' },
                { k: 'LoadQty', t: 'LoadQty', w: 70, f: 'n3', edit: true }] });
        G.hist = SE.grid('grdhistory', { frozen: 3, onDbl: function (r) { histEdit(r); },
            onBtn: function (k, r) {
                if (k === 'Edit') histEdit(r); else if (k === 'SaveAs') histSaveAs(r); else if (k === 'Print') print(r.Id);
            },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(r.Id); },
            onSel: histSelected,
            cols: [
                { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 50, btn: 'Print' }, { k: 'SaveAs', t: 'SaveAs', w: 70, btn: 'SaveAs' },
                { k: 'Id', hide: true }, { k: 'DoType', t: 'DoType', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' }, { k: 'DocNo', t: 'DocNo', w: 60 },
                { k: 'VehicleType', t: 'VehicleType', w: 100 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'ApprovalStatus', t: 'ApprovalStatus', w: 100 },
                { k: 'GdnNo', t: 'GdnNo', w: 60 }, { k: 'EntryUser', t: 'EntryUser', w: 100 }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' },
                { k: 'ModifyUser', t: 'ModifyUser', w: 100 }, { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ApprovedUser', t: 'ApprovedUser', w: 100 },
                { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' }, { k: 'OrderStatus', t: 'OrderStatus', w: 80 }, { k: 'Remarks', t: 'Remarks', w: 150 },
                { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }] });
        G.hd = SE.grid('GrdHistoryDetail', { dec: 3,
            cols: [{ k: 'CustomerName', t: 'CustomerName', w: 170 }, { k: 'RefPartyId', t: 'ReferenceParty', w: 150, list: function () { return S.ref; }, lk: 'Id', lt: 'ReferencePartyName' },
                { k: 'OrderNo', t: 'OrderNo', w: 80 }, { k: 'WareHouseName', t: 'WareHouseName', w: 150 }, { k: 'ItemCode', t: 'ItemCode', w: 80 }, { k: 'Item', t: 'Item', w: 150 },
                { k: 'ItemUOM', t: 'ItemUOM', w: 60 }, { k: 'VariantDescription', t: 'Modal Description', w: 100 }, { k: 'CastingType', t: 'CastingType', w: 90 },
                { k: 'Remarks', t: 'Specification / Remarks', w: 100 }, { k: 'Weight', t: 'Weight', w: 80, f: 'n3' }, { k: 'LoadQty', t: 'LoadQty', w: 70, f: 'n3' }] });
    }

    /* grdhistory_SelectionChanged: InvDeliveryOrder.GetByID, then the detail grid */
    var histSeq = 0;
    function histSelected(item) {
        if (!item) { G.hd.setRows([]); return; }
        var seq = ++histSeq;
        SE.api(API + '/' + item.Id, { quiet: true }).then(function (d) {
            if (seq !== histSeq) return;
            var lines = d.lines || [];
            if (!lines.length) { G.hd.setRows([]); return; }
            G.hd.setRows(lines.map(function (l) {
                return { CustomerName: l.SupplierCustomer, RefPartyId: l.RefPartyId, OrderNo: l.OrderNo, WareHouseName: l.WareHouseName, ItemCode: l.ItemCode, Item: l.ItemName,
                    ItemUOM: l.PackUOM, VariantDescription: l.VariantDescription, CastingType: l.CastingType, Remarks: l.LoadingRemarks, Weight: l.DoWeight, LoadQty: l.LoadingQty };
            }));
        }).catch(function () { if (seq === histSeq) G.hd.setRows([]); });
    }

    /* ------------------------------------------------------------------ detail entry (btnplus_Click / FormValidationDetail / ResetDetail) */
    function detailValid() {
        function stop(m, f) { msg(m); if (f) f(); return false; }
        if (!cb.cust.row() || !(+cb.cust.value())) return stop('Customer field is required', function () { cb.cust.focus(); });
        if (!cb.wh.row() || !(+cb.wh.value())) return stop('WareHouseName field is required', function () { cb.wh.focus(); });
        if (!cb.order.row() || !(+cb.order.value())) return stop('OrderNo field is required', function () { cb.order.focus(); });
        if (!cb.item.row() || !(+cb.item.value())) return stop('ItemName field is required', function () { cb.item.focus(); });
        if (!cb.uom.row() || !(+cb.uom.value())) return stop('UOM field is required', function () { cb.uom.focus(); });
        if (SE.toNum($('txtqty').value) === 0) return stop('ItemQty field is required', function () { $('txtqty').focus(); });
        return true;
    }
    function resetDetail() {
        cb.cust.focus();
        $('txtRefDocTypeId').value = ''; $('txtDocNoId').value = ''; $('txtSubNoId').value = '';
        cb.order.clear(); cb.item.clear(); cb.uom.clear();
        $('txtqty').value = ''; $('txtweight').value = ''; $('txtBalQty').value = '0'; $('txtremarksdetail').value = '';
        cb.wh.clear();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        S.updateIdx = -1;
    }
    function rowFromEntry() {
        var ir = cb.item.row(), ur = cb.uom.row();
        return { SupplierCustomerId: +cb.cust.value(), CustomerName: cb.cust.text().trim(), OrderId: +cb.order.value(), OrderNo: SE.toInt(cb.order.text().trim()),
            WareHouseId: +cb.wh.value(), OrderDetailId: +ir.OrderDetailId || 0, ItemId: +cb.item.value(), ItemCode: ir.ItemCode, Item: ir.ItemName,
            ItemUOMId: +cb.uom.value(), ItemUOM: cb.uom.text().trim(), UomEquivalent: SE.toNum(ur.Equivalent), VariantId: +cb.variant.value() || 0,
            VariantDescription: cb.variant.text(), CastingTypeId: +cb.casting.value() || 0, CastingType: cb.casting.text(), Remarks: $('txtremarksdetail').value,
            QTY: SE.toNum($('txtqty').value.trim()), Weight: SE.toNum($('txtweight').value.trim()), LoadQty: SE.toNum($('txtqty').value.trim()) };
    }
    function plus() {
        if (!detailValid()) return;
        var r = rowFromEntry(); r.Id = 0; r.RefPartyId = 0;
        G.grd.addRow(r);
        resetDetail();
    }
    /* grd_DoubleClick */
    function editRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        S.updateIdx = i;
        cb.cust.setValue(r.SupplierCustomerId);
        return loadOrders(+r.SupplierCustomerId).then(function () {
            cb.order.setValue(r.OrderId);
            return orderLeave();
        }).then(function () {
            cb.wh.setValue(r.WareHouseId);
            cb.item.setValue(+r.ItemId);
            return itemLeave();
        }).then(function () {
            cb.uom.setValue(r.ItemUOMId);
            cb.variant.setValue(+r.VariantId);
            cb.casting.setValue(+r.CastingTypeId);
            $('txtBalQty').value = r.QTY == null ? '' : r.QTY;
            $('txtqty').value = r.LoadQty == null ? '' : r.LoadQty;
            $('txtweight').value = r.Weight == null ? '' : r.Weight;
            $('txtremarksdetail').value = r.Remarks == null ? '' : r.Remarks;
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
            cb.cust.focus();
        }).catch(fail);
    }
    function updateDetail() {
        if (!detailValid()) return;
        var r = G.grd.rows()[S.updateIdx]; if (!r) return;
        var n = rowFromEntry();
        ['SupplierCustomerId', 'CustomerName', 'OrderId', 'OrderNo', 'WareHouseId', 'OrderDetailId', 'ItemId', 'ItemCode', 'Item', 'ItemUOMId', 'ItemUOM', 'UomEquivalent',
            'VariantId', 'VariantDescription', 'CastingTypeId', 'CastingType', 'Remarks', 'QTY', 'Weight', 'LoadQty'].forEach(function (k) { r[k] = n[k]; });
        G.grd.refresh();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        resetDetail();
        SE.api(API + '/lists', { quiet: true }).then(function (d) { fillCustomers(d.customers); }).catch(fail);   // CustomerNameFill(BindCustomerCombo())
    }
    /* grd_ColumnButtonClick "Add" */
    function addCopy(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        var c = JSON.parse(JSON.stringify(r));
        if (isShown('btnupdate') && !$('btnupdate').disabled) c.Id = 0;
        G.grd.addRow(c);
    }
    /* DeleteDetailrow */
    function deleteRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        if (S.updateIdx !== -1) return msg('Reset Detail First');
        function finish() { G.grd.removeAt(i); getSaleOrderIds(); }
        if ((+r.Id || 0) > 0) {
            return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
                if (!yes) return;
                S.remove.push(lineOf(r));
                finish();
            });
        }
        finish();
    }
    function getSaleOrderIds() {
        S.ordIds = ''; S.ordMainIds = '';
        G.grd.rows().forEach(function (r) { S.ordIds += ',' + (+r.OrderDetailId || 0); S.ordMainIds += ',' + (+r.OrderId || 0); });
    }

    /* ------------------------------------------------------------------ Load */
    function applyInitial(d) {
        S.rights = d.rights || {};
        S.histDays = d.historyDays || 3;
        $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : $('txtdocno').value;
        fillVehicles(d.vehicleTypes); fillCustomers(d.customers); fillRef(d.refParties); fillWarehouses(d.warehouses);
        fillCategories(d.categories); fillCasting(d.castingTypes);
        cb.hCust.setData(d.historyCustomers || []);
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') $('FromDateHistory').focus(); else $('DocDate').focus(); });
        SE.digitsOnly($('FromDocNoHistory')); SE.digitsOnly($('ToDocNoHistory')); SE.upper($('txtVehicleNo'));
        ['txtqty', 'txtweight'].forEach(function (id) {
            $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        });
        makeGrids();
        $('DocDate').value = SE.today();
        $('txtBalQty').value = '0';
        SE.api(API + '/initial').then(function (d) {
            applyInitial(d);
            $('btnsave').disabled = !S.rights.save; $('btnSaveAs').disabled = !S.rights.save;
            $('btnupdate').disabled = !S.rights.update; $('btnDelete').disabled = !S.rights.delete; $('btnprint').disabled = !S.rights.print;
            $('ChkPrint').checked = !!S.rights.print;
            show('btnupdate', false); show('btnDelete', false); show('btnSaveAs', false); show('btnsave', true);
            show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            $('FromDateHistory').value = SE.addDays(-S.histDays); $('ToDateHistory').value = SE.today();
            $('DocDate').focus();
        }).catch(fail);
        wire();
    }

    /* ------------------------------------------------------------------ Reset (btnnew_Click / Reset) */
    function reset() {
        S.files = []; S.removedAtt = []; S.existing = []; S.remove = []; S.ordIds = ''; S.ordMainIds = ''; S.id = 0; S.approved = false;
        $('DocDate').focus();
        cb.item.clear(); cb.uom.clear();
        $('txtqty').value = ''; $('txtweight').value = ''; $('txtremarks').value = '';
        $('txtRefDocTypeId').value = ''; $('txtDocNoId').value = ''; $('txtSubNoId').value = '';
        $('txtVehicleNo').value = ''; cb.veh.clear(); $('txtBalQty').value = '0';
        show('btnsave', true); show('btnupdate', false); show('btnDelete', false); show('btnSaveAs', false);
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        S.updateIdx = -1;
        G.grd.setRows([]);
        return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); })
            .then(function () { return SE.api(API + '/lists', { quiet: true }); }).then(function (d) { fillCustomers(d.customers); }).catch(fail);
    }

    /* btnRefresh_Click */
    function refresh() {
        return SE.api(API + '/lists').then(function (d) {
            fillVehicles(d.vehicleTypes); fillCustomers(d.customers); fillRef(d.refParties); fillWarehouses(d.warehouses); fillCategories(d.categories); fillCasting(d.castingTypes);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Insert / btnupdate_Click / btnSaveAs_Click */
    function lineOf(r) {
        return { id: +r.Id || 0, supplierCustomerId: +r.SupplierCustomerId || 0, refPartyId: +r.RefPartyId || 0, orderId: +r.OrderId || 0, orderDetailId: +r.OrderDetailId || 0,
            warehouseId: +r.WareHouseId || 0, itemId: +r.ItemId || 0, packUomId: +r.ItemUOMId || 0, variantId: +r.VariantId || 0, castingTypeId: +r.CastingTypeId || 0,
            remarks: r.Remarks == null ? '' : String(r.Remarks), loadQty: SE.toNum(r.LoadQty) };
    }
    function insert() {
        if (G.grd.count() === 0) return msg('Grid Record Not Found', 'Message Error');
        var no = field('txtdocno');
        if (no === '' || no === '0') return msg('DocNo Field is Required').then(function () { $('txtdocno').focus(); });
        return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
            if (!yes) return;
            var body = {
                id: S.id, docDate: $('DocDate').value, docNo: no, vehicleType: cb.veh.text().trim(), vehicleNo: field('txtVehicleNo'), remarks: $('txtremarks').value,
                lines: G.grd.rows().map(lineOf), removed: S.id > 0 ? S.remove : [],
                attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt }
            };
            return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                var preview = $('ChkPrint').checked;
                return SE.alert(r.message).then(function () {
                    return reset().then(function () { if (preview) print(r.id); });
                });
            }).catch(function (e) { return SE.alert(e.message, 'Message Error'); });
        });
    }
    function btnSave() { S.id = 0; return insert(); }
    function btnSaveAs() { S.id = 0; return insert(); }
    function btnUpdate() {
        if (S.id === 0) return SE.alert('Record Not Update because RecId Not Found', 'Database Error');
        return insert();
    }

    /* ------------------------------------------------------------------ ReadById */
    function edit(id) {
        return reset().then(function () {
            S.id = id;
            return SE.api(API + '/' + id);
        }).then(function (d) {
            var h = d.head, lines = d.lines || [];
            if (!lines.length) return;
            show('btnsave', false); show('btnSaveAs', false); show('btnupdate', true); show('btnDelete', true);
            tabs.select('tabPage1');
            $('DocDate').focus();
            $('DocDate').value = SE.dateInput(h.DocDate);
            $('txtdocno').value = h.DocNo;
            var vt = String(h.VehicleType == null ? '' : h.VehicleType), vr = cb.veh.rows(), vi = -1;
            for (var i = 0; i < vr.length; i++) if (String(vr[i].VehicleDescription) === vt) { vi = i; break; }
            if (vi >= 0) cb.veh.setValue(vr[vi].Id); else cb.veh.clear();
            $('txtVehicleNo').value = h.VehicleNo || '';
            $('txtremarks').value = h.LoadingInstructions || '';
            S.approved = !!h.IsApproved;
            G.grd.setRows(lines.map(function (l) {
                return { Id: l.Id, SupplierCustomerId: l.SupplierCustomerId, CustomerName: l.SupplierCustomer, RefPartyId: l.RefPartyId, OrderId: l.SaleOrderId, OrderNo: l.OrderNo,
                    WareHouseId: l.WarehouseId, OrderDetailId: l.SaleOrderDetailId, ItemId: l.ItemId, ItemCode: l.ItemCode, Item: l.ItemName, ItemUOMId: l.PackUomId,
                    ItemUOM: l.PackUOM, UomEquivalent: l.PUomEquivalent, VariantId: l.ItemVariantId, VariantDescription: l.VariantDescription, CastingTypeId: l.CastingTypeId,
                    CastingType: l.CastingType, Remarks: l.LoadingRemarks, QTY: l.DoQty, Weight: l.DoWeight, LoadQty: l.LoadingQty };
            }));
            S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
        }).catch(fail);
    }
    function notOpen(r) {
        var st = String(r.OrderStatus == null ? '' : r.OrderStatus);
        if (st !== 'Open') { msg("Sorry You can't Update this record because Order_Status is:" + st, 'Error'); return true; }
        return false;
    }
    /* grdhistory_DoubleClick / Edit button / Ctrl+Enter */
    function histEdit(r) { if (notOpen(r)) return; return edit(+r.Id); }
    /* grdhistory_ColumnButtonClick "SaveAs": Reset; ReadById; the SaveAs button only */
    function histSaveAs(r) {
        return edit(+r.Id).then(function () { show('btnsave', false); show('btnupdate', false); show('btnDelete', false); show('btnSaveAs', true); });
    }

    /* ------------------------------------------------------------------ history (gridhistoryfill) */
    function showHistory() {
        var dt = $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : ($('rdapproveddate').checked ? 'approved' : 'document'));
        if ($('drdocdate').checked) dt = 'document';
        var q = { dateType: dt, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
            fromNo: SE.toInt($('FromDocNoHistory').value), toNo: SE.toInt($('ToDocNoHistory').value), customerId: +cb.hCust.value() || 0 };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            var seen = {}, main = [];
            rows.forEach(function (r) {
                if (seen[r.Id]) return; seen[r.Id] = 1;
                main.push({ Id: r.Id, DoType: r.DeliveryOrderType, DocDate: r.DocDate, DocNo: r.DocNo, VehicleType: r.VehicleType, VehicleNo: r.VehicleNo, ApprovalStatus: r.ApprovalStatus,
                    GdnNo: SE.toInt(r.GdnNo), EntryUser: r.EntryUser, EntryDate: r.EntryDate, ModifyUser: r.ModifyUser, ModifyDate: r.ModifyDate, ApprovedUser: r.ApprovedUser,
                    ApprovedDate: r.ApprovedDate, OrderStatus: r.OrderStatus, Remarks: r.HeaderRemarks, NoOfAttachments: r.NoOfAttachments });
            });
            G.hist.setRows(main);
            if (!main.length) G.hd.setRows([]);
        }).catch(function (e) { return SE.alert(e.message, 'Error Message'); });
    }
    /* ResetHistory */
    function resetHistory() {
        $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today();
        $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hCust.clear();
        G.hist.setRows([]); G.hd.setRows([]);
        $('drdocdate').checked = true;
    }
    function refreshHistoryCombos() {
        return SE.api(API + '/history-customers').then(function (rows) { cb.hCust.setData(rows || []); }).catch(function (e) { return SE.alert(e.message, 'Error Message'); });
    }

    /* ------------------------------------------------------------------ print / delete / attachments */
    function print(id) {
        if (!(id > 0)) return SE.alert('No Record Found For Display');
        SE.printRpt('1657_DeliveryOrderSlip_Engr.rpt', { id: id, documentTypeId: DOC });
    }
    function del() {
        if (S.id === 0) return SE.alert('Record Id Not Found', 'Message');
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            if (S.approved) return SE.alert('Record has been approved', 'Message');
            if (!(isShown('btnDelete') && !$('btnDelete').disabled)) return;
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return SE.alert(r.message).then(reset); })
                .catch(function (e) { return SE.alert(e.message, 'Message'); });
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

    /* ------------------------------------------------------------------ frmLoadSaleOrderForDoEngr (btnLoadSaleOrder = main + detail, btnLoaderView2 = main only) */
    function openLoader(mainDetail) {
        var h = '<div class="lbar"><span class="dl" style="left:3px;top:6px;font:bold 16px Tahoma,sans-serif">Sale Order Load</span></div>' +
            '<div class="lf" style="height:61px">' +
            '<span class="dl" style="left:6px;top:11px">Branch Name</span><div id="ldBranch" class="f" tabindex="0" style="position:absolute;left:85px;top:5px;width:215px;height:26px;border:1px solid #7a7a7a;background:#fff;overflow:hidden;white-space:nowrap;cursor:pointer;font:12px Verdana,sans-serif;padding:4px"></div>' +
            '<span class="dl" style="left:301px;top:11px">Customer</span><select class="f" id="ldCust" style="left:359px;top:5px;width:349px;height:26px"></select>' +
            '<span class="dl" style="left:6px;top:39px">From Date</span><input class="f" type="date" id="ldFrom" style="left:85px;top:35px;width:107px;height:22px">' +
            '<input class="f" type="date" id="ldTo" style="left:193px;top:35px;width:107px;height:22px">' +
            '<span class="dl" style="left:301px;top:39px">Item</span><select class="f" id="ldItem" style="left:359px;top:33px;width:349px;height:26px"></select>' +
            '<button type="button" class="dbtn" id="ldSearch" style="left:711px;top:33px;width:56px;height:26px">Show</button>' +
            '<button type="button" class="dbtn" id="ldLoad" style="left:767px;top:33px;width:56px;height:26px">Load</button></div>' +
            '<div class="dtc" id="ldTabs"><div class="dsubtabs"><span class="tab on" data-tab="ldP1">' + (mainDetail ? 'Main Detail' : 'Main') + '</span></div>' +
            '<div class="tpane" id="ldP1">' +
            (mainDetail ? '<div class="lgrid" id="ldGrd" style="height:200px"></div><div class="lbar" style="height:19px"></div><div class="lgrid" id="ldDet" style="height:200px"></div>'
                : '<div class="lgrid" id="ldSecond" style="height:420px"></div>') +
            '</div></div>';
        var pop = SE.pop('frmLoadSaleOrderForDoEngr', h, [{ t: 'Close', fn: function (p) { p.close(); } }], { wide: true });
        pop.open();
        var $$ = function (id) { return pop.body.querySelector('#' + id); };
        var cc = XCombo($$('ldCust'), { columns: [{ key: 'Customer', caption: 'Customer Name' }], textKey: 'Customer', popupWidth: 380 });
        var ci = XCombo($$('ldItem'), { columns: [{ key: 'ItemName', caption: 'Item Name' }], textKey: 'ItemName', popupWidth: 500 });
        var st = { branches: [], feature: false, implemented: false };
        /* cmbBranchName: a checked list (all ticked at load); the text is the ticked names joined by "," */
        var bl = $$('ldBranch'), bpop = null;
        function branchText() { return st.branches.filter(function (b) { return b._on; }).map(function (b) { return b.BranchName; }).join(','); }
        function branchIds() {                                                    // ",id,id" from the text, as the desktop builds it
            var out = '';
            (branchText() + ',').split(',').forEach(function (n) {
                var hit = st.branches.filter(function (b) { return b.BranchName === n; })[0];
                if (hit) out += ',' + hit.Id;
            });
            return out;
        }
        function paintBranch() { bl.textContent = branchText(); }
        bl.addEventListener('click', function () {
            if (bpop) { bpop.remove(); bpop = null; paintBranch(); if (branchText() !== '') loadCombos(); return; }
            bpop = document.createElement('div');
            bpop.style.cssText = 'position:absolute;left:85px;top:31px;width:215px;max-height:160px;overflow:auto;background:#fff;border:1px solid #7a7a7a;z-index:5;font:12px Verdana,sans-serif';
            st.branches.forEach(function (b, i) {
                var l = document.createElement('label'); l.style.cssText = 'display:block;padding:2px 4px;cursor:pointer';
                l.innerHTML = '<input type="checkbox" data-i="' + i + '"' + (b._on ? ' checked' : '') + '> ' + SE.esc(b.BranchName);
                bpop.appendChild(l);
            });
            bpop.addEventListener('change', function (e) { var i = e.target.getAttribute('data-i'); if (i != null) { st.branches[+i]._on = e.target.checked; paintBranch(); } });
            bl.parentNode.appendChild(bpop);
        });
        var mainCols = [{ k: 'Select', t: '', w: 30, sel: true }, { k: 'OrderId', hide: true }, { k: 'DocumentType', t: 'DocumentType', w: 150 }, { k: 'OrderSupCustId', hide: true },
            { k: 'BranchName', t: 'BranchName', w: 200, hide: true }, { k: 'PartyName', t: 'PartyName', w: 200 }, { k: 'DocDate', t: 'Order Date', w: 100, f: 'sdate' },
            { k: 'DocNo', t: 'Order No', w: 100 }, { k: 'OrderQty', t: 'OrderQty', w: 90, f: 'n2' }, { k: 'DispatchedQty', t: 'DispatchedQty', w: 90, f: 'n2' },
            { k: 'BalQty', t: 'BalQty', w: 90, f: 'n2' }, { k: 'Remarks', t: 'Remarks', w: 100 }];
        var detCols = [{ k: 'Select', t: '', w: 30, sel: true }, { k: 'DocumentType', t: 'DocumentType', w: 100 }, { k: 'OrderId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 100 },
            { k: 'OrderDetailId', hide: true }, { k: 'OrderSupCustId', hide: true }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 300 }, { k: 'ItemUomId', hide: true },
            { k: 'UOM', t: 'UOM', w: 70 }, { k: 'ItemVariantId', hide: true }, { k: 'VariantDescription', t: 'VariantDescription', w: 120 }, { k: 'CastingTypeId', hide: true },
            { k: 'CastingType', t: 'CastingType', w: 100 }, { k: 'OrderQTY', t: 'OrderQTY', w: 90, f: 'n2' }, { k: 'DispatchQty', t: 'DispatchQty', w: 90, f: 'n2' },
            { k: 'BalQty', t: 'BalQty', w: 90, f: 'n2' }];
        var secCols = [{ k: 'Select', t: '', w: 30, sel: true }, { k: 'DocumentType', t: 'DocumentType', w: 100 }, { k: 'OrderId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 100 },
            { k: 'BranchName', t: 'BranchName', w: 200, hide: true }, { k: 'OrderDetailId', hide: true }, { k: 'OrderSupCustId', hide: true }, { k: 'PartyName', t: 'PartyName', w: 200 },
            { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 180 }, { k: 'ItemUomId', hide: true }, { k: 'UOM', t: 'UOM', w: 70 }, { k: 'ItemVariantId', hide: true },
            { k: 'VariantDescription', t: 'VariantDescription', w: 120 }, { k: 'CastingTypeId', hide: true }, { k: 'CastingType', t: 'CastingType', w: 100 },
            { k: 'OrderQTY', t: 'OrderQTY', w: 90, f: 'n2' }, { k: 'DispatchQty', t: 'DispatchQty', w: 90, f: 'n2' }, { k: 'BalQty', t: 'BalQty', w: 90, f: 'n2' }];
        var gMain, gDet, gSecond;
        function detRow(r) {
            return { DocumentType: r.DocumentType, OrderId: r.Id, OrderNo: r.DocNo, OrderDetailId: r.OrderDetailId, OrderSupCustId: r.OrderSupCustId, ItemId: r.ItemId, ItemName: r.ItemName,
                ItemUomId: r.ItemUOMId, UOM: r.PackUom, ItemVariantId: r.ItemVariantId, VariantDescription: r.VarientDescription, CastingTypeId: r.CastingTypeId, CastingType: r.CastingType,
                OrderQTY: r.OrderQTY, DispatchQty: r.DispatchQty, BalQty: r.BalQty, BranchName: r.BranchName, PartyName: r.CustomerName };
        }
        function branchCol(cols) { var c = cols.filter(function (x) { return x.k === 'BranchName'; })[0]; if (c) c.hide = !(st.feature && !st.implemented); }
        if (mainDetail) {
            gMain = SE.grid($$('ldGrd'), { frozen: 1, footer: false, cols: mainCols, onCheck: function () { bindDetail(); } });
            gDet = SE.grid($$('ldDet'), { frozen: 1, footer: false, cols: detCols });
        } else gSecond = SE.grid($$('ldSecond'), { frozen: 1, footer: false, cols: secCols });
        function params() {
            return { fromDate: $$('ldFrom').value, toDate: $$('ldTo').value, customerId: +cc.value() || 0, itemId: +ci.value() || 0, branchIds: branchIds() };
        }
        function branchGuard() {
            if (branchText() === '') { bl.focus(); SE.alert('Select branch first'); return false; }
            return true;
        }
        function checkedIds(g, key) { return g.checked().map(function (r) { return +r[key]; }); }
        function mainLoad() {                                                      // PendingSaleOrderLoad
            if (!branchGuard()) return Promise.resolve();
            return SE.api(API + '/loader/main' + SE.q(params())).then(function (rows) {
                gMain.setRows(rows.map(function (r) {
                    return { OrderId: r.Id, DocumentType: r.DocumentType, OrderSupCustId: r.OrderSupCustId, BranchName: r.BranchName, PartyName: r.PartyName, DocDate: r.DocDate,
                        DocNo: r.DocNo, OrderQty: r.OrderQty, DispatchedQty: r.DispatchQty, BalQty: r.BalQty, Remarks: r.RemarksHeader };
                }));
                if (!rows.length) gDet.setRows([]);
            }).catch(fail);
        }
        function checkMainFromGrid() {                                             // CheckedAllMainRows: orderMainIds.Contains(OrderId)
            if (S.ordMainIds === '') return;
            gMain.check(function (r) { return S.ordMainIds.indexOf(String(r.OrderId)) >= 0; });
        }
        function bindDetail() {                                                    // GetCheckedRowsId + GridDetailBind + CheckedAllDetailRows
            var ids = checkedIds(gMain, 'OrderId').map(function (x) { return ',' + x; }).join('');
            if (ids === '') { gDet.setRows([]); return Promise.resolve(); }
            return SE.api(API + '/loader/detail' + SE.q({ ids: ids, orderDetailIds: S.ordIds })).then(function (rows) {
                gDet.setRows(rows.map(detRow));
                gDet.check(function () { return true; });
            }).catch(fail);
        }
        function secondLoad() {                                                    // GridSecondViewFill
            if (!branchGuard()) return Promise.resolve();
            return SE.api(API + '/loader/second' + SE.q(params())).then(function (rows) {
                gSecond.setRows(rows.map(detRow));
            }).catch(fail);
        }
        function loadCombos() {                                                    // HistoryCombosFill
            return SE.api(API + '/loader/combos' + SE.q({ branchIds: branchIds() })).then(function (d) {
                cc.setData(d.customers || []); ci.setData(d.items || []);
            }).catch(fail);
        }
        $$('ldSearch').onclick = function () { (mainDetail ? mainLoad() : secondLoad()); };            // btngrnlod_Click
        $$('ldLoad').onclick = function () {                                                          // btnLoadOnInvoice_Click_1 (document type 1657: the order detail ids)
            var grid = mainDetail ? gDet : gSecond, picked = grid.checked();
            if (!picked.length) { SE.alert('No Row is Selected'); return; }
            var ids = picked.map(function (r) { return String(r.OrderDetailId); }).join(',');
            pop.close();
            loadInGrid(ids);
        };
        SE.api(API + '/loader/init').then(function (d) {
            st.branches = (d.branches || []).map(function (b) { b._on = true; return b; });
            st.feature = !!d.branchFeature; st.implemented = !!d.branchImplemented;
            paintBranch();
            $$('ldFrom').value = SE.dateInput(d.fromDate) || SE.today(); $$('ldTo').value = SE.today();
            if (mainDetail) branchCol(mainCols); else branchCol(secCols);
            return loadCombos();
        }).then(function () {
            if (mainDetail) {
                return mainLoad().then(function () { checkMainFromGrid(); return bindDetail(); });
            }
            return secondLoad().then(function () {
                if (S.ordMainIds !== '') gSecond.check(function (r) { return S.ordMainIds.indexOf(String(r.OrderId)) >= 0; });      // CheckedAllMainRowsSecondView
            });
        }).catch(fail);
    }
    /* LoadInGridDetail */
    function loadInGrid(ids) {
        if (!ids) return;
        return SE.api(API + '/loader/rows' + SE.q({ orderDetailIds: ids })).then(function (rows) {
            if (!rows.length) return reset();
            rows.forEach(function (r) {
                var dup = G.grd.rows().some(function (x) { return (+x.OrderDetailId || 0) === (+r.OrderDetailId || 0); });
                if (dup) return;
                G.grd.rows().push({ Id: 0, SupplierCustomerId: r.SupplierCustomerId, CustomerName: r.CustomerName, RefPartyId: 0, OrderId: r.OrderId, OrderNo: r.OrderNo,
                    WareHouseId: r.WareHouseId, OrderDetailId: r.OrderDetailId, ItemId: r.ItemId, ItemCode: r.ItemCode, Item: r.Item, ItemUOMId: r.ItemUomId, ItemUOM: r.ItemUOM,
                    UomEquivalent: r.UomEquivalent, VariantId: r.ItemVariantId, VariantDescription: r.VarientDescription, CastingTypeId: r.CastingTypeId, CastingType: r.CastingType,
                    Remarks: r.OrderRemarks, QTY: r.BalanceQty, Weight: r.WeightFinishGoods, LoadQty: r.BalanceQty });
            });
            G.grd.refresh();
            getSaleOrderIds();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ shortcut keys (frmDeliveryOrderEngr_KeyDown :2550) */
    var SHORT = [['Ctrl+S', 'For Save in Form Tab and For Show History in History Tab'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print 262'], ['Alt+1', 'For Print 262'], ['Ctrl+F1', 'For Loader SO Main Detail'], ['Ctrl+F2', 'For Loader SO Main'], ['Ctrl+F5', 'For Reference Parties Look up'],
        ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+F12', 'For SaveAs'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab'],
        ['Ctrl+ArrowUp', 'For Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function defineRefParties() { window.open('/party-processing/reference-parties', '_blank'); }
    function defineVariant() {
        /* ItemAttributeVarient_Engr is not a web screen of this port: the desktop form is opened with the item preselected and the variant list re-read afterwards */
        msg('Define Item Variant (ItemAttributeVarient_Engr) is not available on the web. The variant list has been re-read.');
        return bindVariants(+cb.item.value() || 0).catch(fail);
    }
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && !e.ctrlKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'),
                function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        var k = e.key.toLowerCase();
        if (e.ctrlKey && k === 'e') { e.preventDefault(); history.back(); return; }
        if (e.ctrlKey && e.altKey) { SE.shortcuts(SHORT); return; }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); $('DocDate').focus(); } else { tabs.select('tabPage2'); $('FromDateHistory').focus(); } return; }
        if (tabs.index() === 0) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnsave') && !$('btnsave').disabled) btnSave(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && e.shiftKey && e.key === 'Delete') { e.preventDefault(); if (isShown('btnDelete') && !$('btnDelete').disabled) del(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnupdate') && !$('btnupdate').disabled) btnUpdate(); }
            else if (e.ctrlKey && e.key === 'F1') { e.preventDefault(); openLoader(true); }
            else if (e.ctrlKey && e.key === 'F2') { e.preventDefault(); openLoader(false); }
            else if (e.ctrlKey && e.key === 'F3') { e.preventDefault(); defineRefParties(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('DocDate').focus(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachmentDialog(); }
            else if (e.ctrlKey && e.key === 'F12') { e.preventDefault(); if (isShown('btnSaveAs') && !$('btnSaveAs').disabled) btnSaveAs(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { cb.cust.focus(); }
            else if (e.ctrlKey && k === 'p' && S.rights.print) { e.preventDefault(); print(S.id); }
            else if (e.altKey && e.key === '1' && S.rights.print) { e.preventDefault(); print(S.id); }
            else if (t && t.id === 'grd') gridKeys(e);
        } else {
            if (e.ctrlKey && k === 's') { e.preventDefault(); showHistory(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); resetHistory(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refreshHistoryCombos(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('FromDateHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grdhistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { $('FromDateHistory').focus(); }
            else if (e.ctrlKey && e.key === 'Enter' && G.hist.cur()) { e.preventDefault(); histEdit(G.hist.cur()); }
        }
    }
    /* grd_KeyDown: Ctrl+D copies the row, Ctrl+Delete deletes it */
    function gridKeys(e) {
        var i = G.grd.curIndex(); if (i < 0) return;
        if (e.ctrlKey && (e.key === 'd' || e.key === 'D')) { e.preventDefault(); addCopy(i); }
        else if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); deleteRow(i); }
    }

    function wire() {
        $('btnnew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnsave').onclick = btnSave; $('btnupdate').onclick = btnUpdate; $('btnSaveAs').onclick = btnSaveAs;
        $('btnattachment').onclick = openAttachmentDialog; $('btnDelete').onclick = del; $('btnprint').onclick = function () { print(S.id); };
        $('btnLoadSaleOrder').onclick = function () { openLoader(true); };
        $('btnLoaderView2').onclick = function () { openLoader(false); };
        $('BtnShortCutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnplus').onclick = plus; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelUpdateDetial').onclick = resetDetail;
        $('btnDefineReferenceParties').onclick = defineRefParties; $('BtnDefineVariant').onclick = defineVariant;
        $('btnNewHistory').onclick = resetHistory; $('btnRefreshHistory').onclick = refreshHistoryCombos; $('btnShowHistory').onclick = showHistory;
        $('rdSearchByName').addEventListener('change', rdSearchChanged); $('rdSearchByCode').addEventListener('change', rdSearchChanged);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
