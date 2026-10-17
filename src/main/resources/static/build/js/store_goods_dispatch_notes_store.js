/*
 * Screen 348  GoodsDispatchNotes_Store  (Architecture.WinApp.StoreManagement.GoodsDispatchNotes_Store, frmGSIssuance, document type 144)
 * Page script. Desktop methods are named in the comments. Server: /store/goods-dispatch-notes-store/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/store/goods-dispatch-notes-store/api';
    var S = {
        id: 0, gpId: 0, orderStatus: '', rights: {}, subsidiary: false, updateIdx: -1,
        files: [], removedAtt: [], existing: [],
        items: [], racks: [], uoms: [], job: [], cond: [], city: [], other: [], trans: [], sup: [], veh: [],
        cfg: { historyDays: 0, freightOutwardAc: 0, defaultWarehouse: 0, wagesStatus: false, wagesActive: false }, wantWh: 0
    };
    var cb = {}, G = {}, tabs, tabs1;

    function fail(e) { return SE.dbError(e); }
    function msg(t) { return SE.alert(t); }
    function field(id) { return ($(id).value || '').trim(); }
    function enable(c, on) { c.el.disabled = !on; var w = c.el.__dtcombo; if (w && w.syncFromSelect) w.syncFromSelect(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function ci(r, k) { if (!r) return undefined; if (k in r) return r[k]; var l = k.toLowerCase(); for (var x in r) if (x.toLowerCase() === l) return r[x]; return undefined; }
    function distinct(rows, key) {
        var seen = {}, out = [];
        rows.forEach(function (r) { var k = String(r[key]); if (!seen[k]) { seen[k] = 1; out.push(r); } });
        return out;
    }
    function keepSet(c, v) { if (v > 0 && !c.setValue(v)) c.clear(); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.cust = XCombo('CmbCustomer', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420, onSelect: function () { customerLeave(); } });
        cb.trans = XCombo('CmbTransporter', { columns: [{ key: 'CompanyName', caption: 'Transporter Name' }], textKey: 'CompanyName', popupWidth: 380 });
        cb.veh = XCombo('combvehtyp', { columns: [{ key: 'VehicleDescription', caption: 'Vehicle Description' }], textKey: 'VehicleDescription' });
        cb.item = XCombo('cmbItem', { columns: [{ key: '_t', caption: 'Item Name' }, { key: '_o', caption: 'Item Code' }], textKey: '_t', popupWidth: 500, onSelect: function () { itemLeave(); } });
        cb.wh = XCombo('CmbWarehouse', { columns: [{ key: 'WareHouseName', caption: 'WareHouseName' }], textKey: 'WareHouseName', onSelect: function () { warehouseLeave(); } });
        cb.rack = XCombo('CmbRackName', { columns: [{ key: 'RackName', caption: 'RackName' }], textKey: 'RackName', onSelect: function () { rackLeave(); } });
        cb.cond = XCombo('CmbItemCondition', { columns: [{ key: 'Description', caption: 'Item Condition' }], textKey: 'Description' });
        cb.job = XCombo('cmbJobLot', { columns: [{ key: 'Description', caption: 'Job Lot' }], textKey: 'Description' });
        cb.uom = XCombo('cmbUOM', { columns: [{ key: 'UOMCode', caption: 'UOM' }], textKey: 'UOMCode' });
        cb.cityc = XCombo('CmbCity', { columns: [{ key: 'Description', caption: 'City Name' }], textKey: 'Description', popupWidth: 300 });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Name', caption: 'Customer Name' }], textKey: 'Name', popupWidth: 380 });
    }

    /* SupplierNameBind (retains the selection, then CmbCustomer_Leave) */
    function fillCustomers(rows) {
        S.sup = rows || S.sup;
        var keep = +cb.cust.value() || 0;
        cb.cust.setData(S.sup);
        keepSet(cb.cust, keep);
        return S.sup.length ? customerLeave() : Promise.resolve();
    }
    /* TransporterFill */
    function fillTransporters(rows) {
        S.trans = rows || S.trans;
        var keep = +cb.trans.value() || 0, key = S.subsidiary ? 'CompanyName' : 'AccountTitle';
        cb.trans.setData(S.trans.map(function (r) { return { Id: r.Id, CompanyName: r[key], SupplierCustomerId: r.SupplierCustomerId }; }));
        keepSet(cb.trans, keep);
    }
    /* VehicleTypesBind */
    function fillVehicles(rows) {
        S.veh = rows || S.veh;
        var keep = +cb.veh.value() || 0;
        cb.veh.setData(S.veh);
        keepSet(cb.veh, keep);
    }
    function fillSimple(c, rows) { var k = +c.value() || 0; c.setData(rows || []); keepSet(c, k); }

    /* GetConfigurationsFromGlobal */
    function applyConfig(d) {
        S.cfg.historyDays = +d.historyDays || 0; S.cfg.freightOutwardAc = +d.freightOutwardAc || 0; S.cfg.defaultWarehouse = +d.defaultWarehouse || 0;
        S.cfg.wagesStatus = !!d.wagesStatus; S.cfg.wagesActive = !!d.wagesActive;
        if (S.cfg.freightOutwardAc !== 0 && S.trans.length) cb.trans.setValue(S.cfg.freightOutwardAc);
        if (S.cfg.defaultWarehouse > 0) { S.wantWh = S.cfg.defaultWarehouse; cb.wh.setValue(S.cfg.defaultWarehouse); }
    }

    /* ------------------------------------------------------------------ customer / item / warehouse / rack leave events */
    /* CmbCustomer_Leave: ItemdtFromPurchaseInvoiceStoreAgainstPartyId + ItemNameBind */
    function customerLeave() {
        var c = +cb.cust.value() || 0;
        return SE.api(API + '/items' + SE.q({ customerId: c })).then(function (rows) { S.items = rows || []; itemNameBind(); }).catch(fail);
    }
    function bindItems(byName) {
        cb.item.setData(S.items.map(function (r) { return { Id: r.Id, _t: byName ? r.ItemName : r.ItemCode, _o: byName ? r.ItemCode : r.ItemName, ItemName: r.ItemName, ItemCode: r.ItemCode, ItemCodeNew: r.ItemCodeNew }; }));
    }
    function itemNameBind() {
        var keep = +cb.item.value() || 0;
        bindItems($('rdbtnItemName').checked);
        keepSet(cb.item, keep);
        if (S.items.length > 0) itemLeave();
    }
    /* cmbItem_Leave */
    function itemLeave() {
        var itemId = +cb.item.value() || 0, whId = +cb.wh.value() || 0;
        bindUoms(itemId); bindWarehouses(itemId); bindRacks(itemId, whId);
    }
    /* PackUomFromGlobalBind (dtUomFromGloablUomScheduleByItemId, retains by the UOM text) */
    function bindUoms(itemId) {
        var prev = cb.uom.text().trim();
        var rows = S.uoms.filter(function (u) { return +u.ItemId === itemId; });
        cb.uom.setData(rows);
        if (prev !== '') {
            var hit = rows.filter(function (r) { return r.UOMCode === prev; })[0];
            if (hit) cb.uom.setValue(hit.Id); else cb.uom.clear();
        }
    }
    /* WareHouseBindFromGlobalRacksByItemId */
    function bindWarehouses(itemId) {
        var keep = +cb.wh.value() || S.wantWh;
        var rows = distinct(S.racks.filter(function (r) { return +r.ItemId === itemId; }), 'WarehouseId').map(function (r) { return { Id: r.WarehouseId, WareHouseName: r.WareHouseName }; });
        cb.wh.setData(rows);
        keepSet(cb.wh, keep);
    }
    /* RackBindFromGlobalRacksByItemId */
    function bindRacks(itemId, whId) {
        var keep = +cb.rack.value() || 0;
        var rows = distinct(S.racks.filter(function (r) { return +r.ItemId === itemId && (whId === 0 || +r.WarehouseId === whId); }), 'Id').map(function (r) { return { Id: r.Id, RackName: r.RackName }; });
        cb.rack.setData(rows);
        keepSet(cb.rack, keep);
    }
    function warehouseLeave() { bindRacks(+cb.item.value() || 0, +cb.wh.value() || 0); }          // cmbWarehouse_Leave
    function rackLeave() {                                                                         // CmbRackName_Leave
        var rid = +cb.rack.value() || 0, hit = S.racks.filter(function (r) { return +r.Id === rid; })[0];
        if (hit && +hit.WarehouseId > 0) cb.wh.setValue(+hit.WarehouseId);
    }
    function rdbChanged() { itemNameBind(); }                                                      // rdbtnItemName_CheckedChanged

    /* FormHelper.ResolveWarehouseAndRack */
    function resolveWR(itemData, cfgWh) {
        var base = itemData.filter(function (x) { return +x.BaseRackId > 0; })[0];
        if (base) return { wid: base.WarehouseId, wname: base.WareHouseName, rid: base.Id, rname: base.RackName };
        var whs = distinct(itemData, 'WarehouseId');
        function racksOf(w) { return distinct(itemData.filter(function (x) { return +x.WarehouseId === +w.WarehouseId; }), 'Id'); }
        if (whs.length === 1) {
            var w = whs[0], rk = racksOf(w);
            return rk.length === 1 ? { wid: w.WarehouseId, wname: w.WareHouseName, rid: rk[0].Id, rname: rk[0].RackName } : { wid: w.WarehouseId, wname: w.WareHouseName, rid: 0, rname: null };
        }
        if (cfgWh > 0) {
            var w2 = whs.filter(function (x) { return +x.WarehouseId === cfgWh; })[0];
            if (w2) {
                var rk2 = racksOf(w2);
                return rk2.length === 1 ? { wid: w2.WarehouseId, wname: w2.WareHouseName, rid: rk2[0].Id, rname: rk2[0].RackName } : { wid: w2.WarehouseId, wname: w2.WareHouseName, rid: 0, rname: null };
            }
        }
        return { wid: 0, wname: null, rid: 0, rname: null };
    }

    /* ------------------------------------------------------------------ grids */
    function makeGrids() {
        G.grd = SE.grid('grdDetail', { footer: false, frozen: 2, onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) { if (k === 'Add') addCopy(i); else if (k === 'Delete') deleteRow(i); },
            onEdit: function (r, k, v) {
                if (k === 'ItemQty') { var n = Number(String(v).replace(/,/g, '')); if (isFinite(n)) r[k] = n; }
                else r[k] = v;
            },
            cols: [
                { k: 'Delete', t: 'X', w: 28, btn: 'X' }, { k: 'Add', t: '+', w: 28, btn: '+' },
                { k: 'Id', hide: true }, { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 },
                { k: 'WareHouseId', hide: true }, { k: 'WarehouseName', t: 'WarehouseName', w: 130 }, { k: 'RackId', hide: true }, { k: 'RackName', t: 'RackName', w: 110 },
                { k: 'ItemConditionId', t: 'Item Condition', w: 110, list: function () { return S.cond; }, lk: 'Id', lt: 'Description' },
                { k: 'JobLotId', t: 'Job Lot', w: 120, list: function () { return S.job; }, lk: 'Id', lt: 'Description' },
                { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'UOM', w: 70 },
                { k: 'ItemQty', t: 'ItemQty', w: 90, f: 'n2', edit: true },
                { k: 'CityId', t: 'City Name', w: 130, list: function () { return S.city; }, lk: 'Id', lt: 'Description' },
                { k: 'Remarks', t: 'Remarks', w: 180 }] });
        G.gp = SE.grid('grdGp', { footer: true, frozen: 2, onBtn: function (k, r) { if (k === 'Load') loadGatePass(r); },
            cols: [
                { k: 'Load', t: 'Load', w: 40, btn: 'Load' },
                { k: 'OrderType', t: 'OrderType', w: 100 }, { k: 'GatepassType', hide: true }, { k: 'OutwardGatePassId', hide: true }, { k: 'SupplierCustomerId', hide: true },
                { k: 'CustomerName', t: 'CustomerName', w: 240 }, { k: 'GpDate', t: 'GpDate', w: 85, f: 'sdate' }, { k: 'GpSrNo', t: 'GpSrNo', w: 70 },
                { k: 'VehicleNo', t: 'VehicleNo', w: 100 }, { k: 'BiltyNo', t: 'BiltyNo', w: 90 }, { k: 'VarietyName', t: 'VarietyName', w: 160 },
                { k: 'Qty', t: 'Qty', w: 80, f: 'n3', sum: true }, { k: 'Freight', t: 'Freight', w: 80, f: 'n3', sum: true }, { k: 'Status', t: 'Status', w: 80 }] });
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
                { k: 'Edit', t: 'Edit', w: 40, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 45, btn: 'Print' }, { k: 'Id', hide: true },
                { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'Customer', t: 'Customer', w: 240 }, { k: 'ManualNo', t: 'ManualNo', w: 100 },
                { k: 'EntryDate', t: 'EntryDate', w: 140, f: 'dt12' }, { k: 'EntryUser', t: 'EntryUser', w: 110 }, { k: 'Remarks', t: 'Remarks', w: 240 },
                { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }] });
        G.hd = SE.grid('grddetailofmain', {
            cols: [{ k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'WarehouseName', t: 'WarehouseName', w: 130 }, { k: 'RackName', t: 'RackName', w: 110 },
                { k: 'ItemCondition', t: 'ItemCondition', w: 110 }, { k: 'JobLot', t: 'JobLot', w: 120 }, { k: 'PackUom', t: 'PackUom', w: 70 },
                { k: 'ItemQty', t: 'ItemQty', w: 90, f: 'n3' }, { k: 'CityName', t: 'CityName', w: 130 }, { k: 'Remarks', t: 'Remarks', w: 200 }] });
    }
    /* DetailGridSettings: ItemQty is editable unless OrderStatus is DeliverOrder */
    function detailGridSettings() {
        G.grd.spec.cols.filter(function (c) { return c.k === 'ItemQty'; })[0].edit = S.orderStatus !== 'DeliverOrder';
        G.grd.refresh();
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
            G.hd.setRows(rows.map(function (r) {
                return { ItemCode: r.ItemCode, ItemName: r.Item, WarehouseName: r.WareHouseCode, RackName: r.rackName, ItemCondition: r.ItemCondition, JobLot: r.JobLot,
                    PackUom: r.UOMCode, ItemQty: r.ItemQty, CityName: r.AreaCity, Remarks: r.CommentsDetail };
            }));
        }).catch(function () { /* the desktop shows the exception text only */ });
    }

    /* ------------------------------------------------------------------ detail entry */
    function detailValid() {                                  // FormValidationDetail
        function stop(m, f) { msg(m); if (f) f(); return false; }
        if (!cb.wh.row() || !(+cb.wh.value())) return stop('Warehouse Field Required', function () { cb.wh.focus(); });
        if (!cb.item.row() || !(+cb.item.value())) return stop('Item Field Required', function () { cb.item.focus(); });
        if (!cb.uom.row() || !(+cb.uom.value())) return stop('UOM Field Required', function () { cb.uom.focus(); });
        if (SE.toNum($('txtItemQty').value.trim()) === 0) return stop('ItemQty Field Required', function () { $('txtItemQty').focus(); });
        return true;
    }
    function resetDetail() {                                  // ResetDetail (the City box is not cleared)
        S.updateIdx = -1; S.wantWh = 0;
        cb.item.clear(); cb.wh.clear(); cb.rack.clear(); cb.cond.clear(); cb.job.clear(); cb.uom.clear();
        $('txtItemQty').value = ''; $('txtRemarksDetail').value = '';
        cb.wh.focus();
        show('btnUpdateDetail', false); show('btnCancelDetail', false); show('Add', true);
    }
    function fillDetailRow(row) {                             // FillDetailRow
        var ir = cb.item.row() || {};
        row.ItemId = +cb.item.value() || 0; row.ItemCode = ir.ItemCodeNew == null ? '' : String(ir.ItemCodeNew); row.ItemName = ir.ItemName == null ? '' : String(ir.ItemName);
        row.WareHouseId = +cb.wh.value() || 0; row.WarehouseName = cb.wh.text();
        row.RackId = +cb.rack.value() || 0; row.RackName = cb.rack.text();
        row.ItemConditionId = +cb.cond.value() || 0; row.JobLotId = +cb.job.value() || 0;
        row.PackUomId = +cb.uom.value() || 0; row.PackUom = cb.uom.text().trim();
        row.ItemQty = SE.toNum($('txtItemQty').value.trim()); row.CityId = +cb.cityc.value() || 0; row.Remarks = $('txtRemarksDetail').value.trim();
    }
    function plus() {                                         // Add_Click
        if (!detailValid()) return;
        var row = { Id: 0 };
        fillDetailRow(row);
        G.grd.addRow(row);
        enable(cb.cust, false);
        resetDetail();
    }
    function editRow(i) {                                     // grdDetail_DoubleClick
        if (S.orderStatus === 'DeliverOrder') return;
        var r = G.grd.rows()[i]; if (!r) return;
        S.updateIdx = i;
        cb.item.setValue(+r.ItemId);
        itemLeave();
        cb.wh.setValue(+r.WareHouseId); cb.rack.setValue(+r.RackId); cb.cond.setValue(+r.ItemConditionId); cb.job.setValue(+r.JobLotId);
        cb.uom.setValue(+r.PackUomId);
        $('txtItemQty').value = r.ItemQty == null ? '' : SE.num(r.ItemQty, 3);
        cb.cityc.setValue(+r.CityId || 0);
        $('txtRemarksDetail').value = r.Remarks == null ? '' : r.Remarks;
        show('Add', false); show('btnUpdateDetail', true); show('btnCancelDetail', true);
    }
    function updateDetail() {                                 // btnUpdateDetail_Click
        if (!detailValid()) return;
        var r = G.grd.rows()[S.updateIdx]; if (!r) return;
        fillDetailRow(r);
        G.grd.refresh();
        resetDetail();
    }
    function cancelDetail() { resetDetail(); }                // btnCancelDetail_Click
    function addCopy(i) {                                     // grdDetail_ColumnButtonClick "Add" (AddDetailRow)
        var r = G.grd.rows()[i]; if (!r) return;
        var c = JSON.parse(JSON.stringify(r));
        if (isShown('btnUpdate') && !$('btnUpdate').disabled) c.Id = 0;
        G.grd.addRow(c);
    }
    function deleteRow(i) {                                   // "Delete" (DeleteDetailRow)
        if (!G.grd.rows()[i]) return;
        G.grd.removeAt(i);
        if (G.grd.count() === 0) enable(cb.cust, true);
    }

    /* grdDetail_KeyDown F1: pop-up selection written back into the grid row (GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow; closing without a pick writes 0) */
    function pickInto(rows, vKey, dKey, apply) {
        var picked = false;
        var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>' + SE.esc(dKey) + '</th></tr></thead><tbody>' +
            rows.map(function (r) { return '<tr data-id="' + SE.esc(r[vKey]) + '" style="cursor:pointer"><td>' + SE.esc(r[dKey]) + '</td></tr>'; }).join('') + '</tbody></table></div>';
        var pop = SE.pop('Select', h, [{ t: 'Close' }]);
        var origClose = pop.close;
        pop.close = function () { origClose(); if (!picked) apply(0, ''); G.grd.refresh(); };
        pop.el.querySelector('[data-x]').onclick = pop.close;
        pop.el.querySelectorAll('.pf button').forEach(function (b) { b.onclick = pop.close; });
        pop.open();
        pop.body.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-id]'); if (!tr) return;
            var id = +tr.getAttribute('data-id'), hit = rows.filter(function (r) { return +r[vKey] === id; })[0];
            picked = true; apply(id, hit ? hit[dKey] : ''); origClose(); G.grd.refresh();
        });
    }
    function gridF1(row) {                                    // HandleF1Actions
        var key = G.grd.curKey(), itemId = +row.ItemId || 0, whId = +row.WareHouseId || 0;
        var itemData = S.racks.filter(function (x) { return +x.ItemId === itemId; });
        if (key === 'WareHouseId' || key === 'WarehouseName') {                           // HandleWarehouseSelection
            if (!itemData.length) return;
            var list = distinct(itemData, 'WarehouseId').map(function (x) { return { Id: x.WarehouseId, WareHouseName: x.WareHouseName }; });
            pickInto(list, 'Id', 'WareHouseName', function (id, name) {
                row.WareHouseId = id; row.WarehouseName = name;
                var res = resolveWR(itemData, id);
                if (res.rid > 0) { row.RackId = res.rid; row.RackName = res.rname; } else { row.RackId = 0; row.RackName = ''; }
            });
        } else if (key === 'RackId' || key === 'RackName') {                              // HandleRackSelection
            var racks = distinct(itemData.filter(function (x) { return whId === 0 || +x.WarehouseId === whId; }), 'Id');
            if (!racks.length) return;
            pickInto(racks, 'Id', 'RackName', function (id, name) {
                row.RackId = id; row.RackName = name;
                if (whId === 0) {
                    var sel = racks.filter(function (x) { return +x.Id === id; })[0];
                    if (sel) { row.WareHouseId = sel.WarehouseId; row.WarehouseName = sel.WareHouseName; }
                }
            });
        } else if (key === 'JobLotId') pickInto(S.job, 'Id', 'Description', function (id) { row.JobLotId = id; });
        else if (key === 'ItemConditionId') pickInto(S.cond, 'Id', 'Description', function (id) { row.ItemConditionId = id; });
        else if (key === 'CityId') pickInto(S.city, 'Id', 'Description', function (id) { row.CityId = id; });
    }

    /* ------------------------------------------------------------------ pending gate passes (grdGp_ColumnButtonClick) */
    function loadGatePass(item) {
        if (!(isShown('btnsave') && !$('btnsave').disabled)) return;
        var p = G.grd.count() > 0 ? SE.ask('Are you sure to Load? Because detail entries will lost', 'Confirm') : Promise.resolve(true);
        return p.then(function (yes) {
            if (!yes || !item) return;
            S.orderStatus = String(item.OrderType == null ? '' : item.OrderType);
            if (String(item.Status) !== 'Accepted') throw new Error('Status Not Accepted Please check status');
            $('txtGpNo').disabled = true; $('txtVehicleNo').disabled = true; $('txtBiltyNo').disabled = true;
            S.gpId = +item.OutwardGatePassId || 0;
            $('txtGpNo').value = item.GpSrNo == null ? '' : String(item.GpSrNo);
            $('txtFreightAmount').value = SE.num(item.Freight, 4);
            $('txtVehicleNo').value = item.VehicleNo == null ? '' : item.VehicleNo;
            $('txtBiltyNo').value = item.BiltyNo == null ? '' : item.BiltyNo;
            cb.cust.setValue(+item.SupplierCustomerId || 0);
            return customerLeave().then(function () { enable(cb.cust, false); });
        }).catch(function (e) { return SE.alert(e.message); });
    }

    /* ------------------------------------------------------------------ Load (InitializeComponentMethod continuation) */
    function applyLists(d) {
        S.subsidiary = !!d.subsidiary;
        S.job = d.jobLots || []; S.cond = d.conditions || []; S.city = d.cities || []; S.racks = d.racks || []; S.uoms = d.uoms || []; S.other = d.otherItems || [];
        fillTransporters(d.transporters || []);
        fillVehicles(d.vehicleTypes || []);
        fillSimple(cb.job, S.job); fillSimple(cb.cond, S.cond); fillSimple(cb.cityc, S.city);
        G.gp.setRows(d.gatePasses || []);
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControlGsIssuance', function (id) { if (id === 'tabPage2') $('FromDateHistory').focus(); });
        tabs1 = SE.tabs('tabControl1');
        /* frmGSIssuance_Load: tabControl1.TabPages.Remove(tabPage4) - the Expenses page is not shown on the desktop form */
        var t4 = document.querySelector('#tabControl1 .dsubtabs [data-tab="tabPage4"]'); if (t4) t4.style.display = 'none';
        SE.digitsOnly($('FromDocNoHistory')); SE.digitsOnly($('ToDocNoHistory')); SE.digitsOnly($('txtGpNo')); SE.digitsOnly($('txtItemQty'));
        $('txtFreightAmount').addEventListener('keypress', function (e) {                       // OnlytextdecimelFunction
            if (e.key.length === 1 && !e.ctrlKey && !/[\d.]/.test(e.key)) e.preventDefault();
            if (e.key === '.' && $('txtFreightAmount').value.indexOf('.') > -1) e.preventDefault();
        });
        makeGrids();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {};
            applyLists(d);
            $('btnsave').disabled = !S.rights.save; $('btnUpdate').disabled = !S.rights.update; $('Print').disabled = !S.rights.print;
            show('btnUpdate', false); show('btnsave', true);
            $('txtDocNo').value = d.nextNo > 0 ? String(d.nextNo) : $('txtDocNo').value;
            resetExpenses();
            return fillCustomers(d.customers || []).then(function () {
                applyConfig(d);
                cb.hCust.setData((d.history || []));
                $('txtDocdate').value = SE.today();
                $('FromDateHistory').value = SE.addDays(S.cfg.historyDays > 0 ? -S.cfg.historyDays : -3); $('ToDateHistory').value = SE.today();
                detailGridSettings();
                $('txtDocdate').focus();
                var rec = SE.param('record'); if (rec) edit(+rec);
            });
        }).catch(fail);
        wire();
    }

    /* ------------------------------------------------------------------ formReset */
    function reset() {
        S.files = []; S.removedAtt = []; S.existing = [];
        show('panel13', true);
        S.gpId = 0; S.orderStatus = ''; S.id = 0;
        show('btnsave', true); show('btnUpdate', false);
        enable(cb.cust, true);
        $('txtGpNo').disabled = false; $('txtVehicleNo').disabled = false; $('txtBiltyNo').disabled = false;
        cb.cust.clear(); cb.trans.clear();
        $('txtFreightAmount').value = ''; $('txtGpNo').value = ''; $('txtVehicleNo').value = ''; $('txtBiltyNo').value = ''; $('txtManualNo').value = ''; $('txtRemarks').value = '';
        G.grd.setRows([]);
        resetExpenses();
        resetDetail();
        detailGridSettings();
        return SE.api(API + '/reset-data').then(function (d) {
            if (d.nextNo > 0) $('txtDocNo').value = String(d.nextNo);
            G.gp.setRows(d.gatePasses || []);
            $('txtDocdate').focus();
        }).catch(fail);
    }
    /* btnRefresh_Click */
    function refresh() {
        return SE.api(API + '/refresh').then(function (d) {
            applyLists(d);
            return fillCustomers(d.customers || []).then(function () { applyConfig(d); detailGridSettings(); });
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Insert */
    function formValid() {                                    // FormValidation
        function stop(m, f) { return SE.alert(m).then(function () { if (f) f(); return false; }); }
        var no = field('txtDocNo');
        if (no === '' || SE.toInt(no) === 0) return stop('DocNo Field Required', function () { $('txtDocNo').focus(); });
        var f = SE.toNum($('txtFreightAmount').value);
        if (+cb.trans.value() > 0 && !(f > 0)) return stop('Freight Amount Field is Required', function () { $('txtFreightAmount').focus(); });
        if (f > 0 && !(+cb.trans.value() > 0)) return stop('Transporter Field is Required', function () { cb.trans.focus(); });
        return Promise.resolve(true);
    }
    function lineOf(r) {
        return { id: +r.Id || 0, itemId: +r.ItemId || 0, warehouseId: +r.WareHouseId || 0, rackId: +r.RackId || 0, itemConditionId: +r.ItemConditionId || 0, jobLotId: +r.JobLotId || 0,
            packUomId: +r.PackUomId || 0, itemQty: SE.toNum(r.ItemQty), cityId: +r.CityId || 0, remarks: r.Remarks == null ? '' : String(r.Remarks) };
    }
    function insert() {
        if (G.grd.count() === 0) return SE.alert('Grid record not found', 'Message');
        return formValid().then(function (ok) {
            if (!ok) return;
            return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var body = {
                    id: S.id, docDate: $('txtDocdate').value, docNo: field('txtDocNo'), customerId: +cb.cust.value() || 0, manualNo: field('txtManualNo'),
                    transporterId: +cb.trans.value() || 0, freight: field('txtFreightAmount'), remarks: $('txtRemarks').value, gatePassId: S.gpId, gpNo: field('txtGpNo'),
                    vehicleTypeId: +cb.veh.value() || 0, vehicleType: cb.veh.text(), vehicleNo: $('txtVehicleNo').value, biltyNo: $('txtBiltyNo').value,
                    lines: G.grd.rows().map(lineOf),
                    expenses: G.exp.rows().map(function (r) { return { itemId: +r.ItemId || 0, qty: SE.toNum(r.Qty), remarks: r.Remarks == null ? '' : String(r.Remarks) }; }),
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt }
                };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return SE.alert(r.message).then(function () {
                        /* frmwagesBillHeader (RefDocTypeId 144) opens here on the desktop when both wage switches are on */
                        var w = r.wages ? SE.alert('Wages bill entry is required for this document (RefDocTypeId ' + r.wagesRefDocTypeId + ', RefDocId ' + r.wagesRefDocId + ', GrossWeightTotal ' + SE.num(r.wagesGrossWeightTotal, 3) + '). Open the Wages Bill screen to enter it.', 'Message') : Promise.resolve();
                        return w.then(function () {
                            var preview = $('ChkBox').checked;
                            return reset().then(function () { if (preview) print(r.id); });
                        });
                    });
                }).catch(function (e) { return SE.alert(e.message, 'Message'); });
            });
        });
    }
    function btnSave() { S.id = 0; return insert(); }
    function btnUpdate() { return insert(); }

    /* ------------------------------------------------------------------ ReadById */
    function edit(id) {
        S.id = id;
        return SE.api(API + '/' + id).then(function (d) {
            var h = d.head, lines = d.lines || [];
            tabs.select('tabPage1');
            show('btnsave', false); show('btnUpdate', true);
            enable(cb.cust, false);
            $('txtDocNo').value = h.DocNo; $('txtDocdate').value = SE.dateInput(h.DocDate);
            cb.cust.setValue(+h.SupplierCustomerId);
            return customerLeave().then(function () {
                if (+h.TransporterId > 0) cb.trans.setValue(S.subsidiary ? +h.TransporterSupCustId : +h.TransporterId);
                $('txtFreightAmount').value = SE.num(h.CarriageAmount, 3);
                $('txtManualNo').value = h.ReferenceDocNo || '';
                $('txtGpNo').value = String(+h.GpNo || 0);
                var vt = String(h.VehicleType == null ? '' : h.VehicleType), vr = S.veh.filter(function (x) { return x.VehicleDescription === vt; })[0];
                if (vr) cb.veh.setValue(vr.Id); else cb.veh.clear();
                $('txtVehicleNo').value = h.VehicleNo || '';
                $('txtBiltyNo').value = h.BiltyNo || '';
                S.gpId = +h.OutwardGatePassId || 0;
                if (S.gpId > 0) { $('txtGpNo').disabled = true; $('txtVehicleNo').disabled = true; $('txtBiltyNo').disabled = true; }
                S.orderStatus = String(h.OtherSupCust == null ? '' : h.OtherSupCust);
                if (S.orderStatus === 'DeliverOrder') show('panel13', false);
                $('txtRemarks').value = h.RemarksHeader || '';
                G.grd.setRows(lines.map(function (l) {
                    return { Id: l.Id, ItemId: l.ItemId, ItemCode: l.ItemCode, ItemName: l.Item, WareHouseId: l.WarehouseId, WarehouseName: l.WareHouseCode, RackId: l.RackId, RackName: l.rackName,
                        ItemConditionId: l.ItemConditionId, JobLotId: l.JobLotId, PackUomId: l.ItemUomId, PackUom: l.UOMCode, ItemQty: l.ItemQty, CityId: l.CityId, Remarks: l.CommentsDetail };
                }));
                detailGridSettings();
                var ex = d.expenses || [];
                G.exp.setRows(ex.map(function (e) { return { ItemId: e.ItemId, Qty: e.Qty, Remarks: e.Remarks }; }));
                if (G.exp.count() === 0) addExpenseRow();
                S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
            });
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history (HistoryFill) */
    function showHistory() {
        var q = { fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
            fromDocNo: SE.toInt($('FromDocNoHistory').value), toDocNo: SE.toInt($('ToDocNoHistory').value), customerId: +cb.hCust.value() || 0 };
        return SE.api(API + '/history', { method: 'POST', body: q }).then(function (rows) {
            if (!rows || !rows.length) { G.hist.setRows([]); return; }
            G.hist.setRows(rows);
        }).catch(function (e) { return SE.alert(e.message); });
    }
    function resetHistory() {                                 // btnResetHistory_Click
        $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today();
        $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hCust.clear();
        G.hist.setRows([]); G.hd.setRows([]);
    }
    function refreshHistoryCombos() {                         // btnRefreshHistory_Click
        return SE.api(API + '/history-combos').then(function (rows) { var k = +cb.hCust.value() || 0; cb.hCust.setData(rows || []); keepSet(cb.hCust, k); }).catch(function (e) { return SE.alert(e.message); });
    }

    /* ------------------------------------------------------------------ print / attachments */
    function print(id) {                                      // Print_Click -> CommonServices.GdnStoreSlip144
        if (!(id > 0)) return SE.alert('No Record Found');
        SE.printRpt('144-InvGdn_Slip.rpt', { id: id });
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

    /* ------------------------------------------------------------------ shortcut keys (frmGSIssuance_KeyDown / MakeShortCutKeys) */
    var SHORT = [['Ctrl+S', 'When In Form For Save Else For Show Data in History'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Warehouse in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
        ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function inGrid(t, id) { return !!(t && t.closest && t.closest('#' + id)); }
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && !e.ctrlKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'),
                function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.key === 'F1' && inGrid(t, 'grdDetail')) { var cr = G.grd.cur(); if (cr) { e.preventDefault(); gridF1(cr); } return; }
        if (e.altKey && !e.ctrlKey && (e.code === 'Numpad1' || e.code === 'Digit1') && tabs.index() === 0) { e.preventDefault(); print(S.id); return; }
        if (!e.ctrlKey) return;
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
            else if (e.key === 'ArrowDown') { $('grdDetail').focus(); }
            else if (e.key === 'ArrowRight') { if (inGrid(t, 'grdDetail') || inGrid(t, 'GridExpense')) $('grdDetail').focus(); }
            else if (e.key === 'ArrowUp') { cb.wh.focus(); }
            else if (e.key === 'Enter' && inGrid(t, 'grdDetail')) { e.preventDefault(); editRow(G.grd.curIndex()); }
        } else if (tabs.index() === 1) {
            if (e.key === 'F5') { e.preventDefault(); $('DataGridHistory').focus(); }
            else if (e.key === 'ArrowDown') { $('grddetailofmain').focus(); }
            else if (e.key === 'ArrowRight') { if (document.activeElement === $('grddetailofmain') || inGrid(t, 'grddetailofmain')) $('DataGridHistory').focus(); else $('grddetailofmain').focus(); }
            else if (e.key === 'ArrowUp') { $('DataGridHistory').focus(); }
        }
    }

    function wire() {
        $('btnnew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnsave').onclick = btnSave; $('btnUpdate').onclick = btnUpdate;
        $('btnAttachment').onclick = openAttachmentDialog; $('Print').onclick = function () { print(S.id); };
        $('btnShortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('Add').onclick = plus; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelDetail').onclick = cancelDetail;
        $('btnResetHistory').onclick = resetHistory; $('btnRefreshHistory').onclick = refreshHistoryCombos; $('btnShow').onclick = showHistory;
        $('rdbtnItemName').addEventListener('change', rdbChanged); $('rdbtnItemCode').addEventListener('change', rdbChanged);
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
