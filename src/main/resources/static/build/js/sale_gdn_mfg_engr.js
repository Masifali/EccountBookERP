/*
 * Screen 844  frmGoodsDispatchNotesEngr  (Architecture.WinApp.Mfg.frmGoodsDispatchNotesEngr, Sale Engr module 134, document type 1659)
 * Page script. Desktop methods are named in the comments (frmGoodsDispatchNotesEngr.cs). Server: /sale/engr/mfg/gdn/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/engr/mfg/gdn/api', DOC = 1659, DO_DOC = 1657;
    var S = {
        id: 0, approved: false, rights: {}, remove: [], files: [], removedAtt: [], existing: [],
        outwardAc: 0, histDays: 3, fyStart: '',
        sup: [], wh: [], cast: [], stage: [], city: [], tr: [], vt: [], veh: []
    };
    var cb = {}, G = {}, tabs;

    function fail(e) { return SE.alert(e && e.message ? e.message : String(e), 'Message'); }
    function msg(t, cap) { return SE.alert(t, cap); }
    function field(id) { return ($(id).value || '').trim(); }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; }
    /* DataRow["Col"] is case-insensitive in the desktop; so is this lookup */
    function v(r, name) {
        if (!r) return undefined;
        if (r[name] !== undefined) return r[name];
        var l = name.toLowerCase();
        for (var k in r) if (Object.prototype.hasOwnProperty.call(r, k) && k.toLowerCase() === l) return r[k];
        return undefined;
    }
    function vi(r, name) { return SE.toInt(v(r, name)); }
    function vn(r, name) { return SE.toNum(v(r, name)); }
    function vs(r, name) { var x = v(r, name); return x == null ? '' : String(x); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.sup = XCombo('CmbSupplier', { columns: [{ key: 'CustomerName', caption: 'Customer Name' }], valueKey: 'SupplierCustomerId', textKey: 'CustomerName', popupWidth: 320, onSelect: supplierChanged });
        cb.doNo = XCombo('CmbDeliveryOrderNo', { columns: [{ key: 'DeliveryNo', caption: 'DeliveryOrder No' }], valueKey: 'DoId', textKey: 'DeliveryNo' });
        cb.term = XCombo('combdeliverytrm', { columns: [{ key: 'DeliveryTerm', caption: 'Delivery Term' }], valueKey: 'Id', textKey: 'DeliveryTerm', onSelect: termChanged });
        cb.tr = XCombo('CmbTransport', { columns: [{ key: 'AccountTitle', caption: 'Transporter Name' }, { key: 'AccountCode', caption: 'AccountCode' }], valueKey: 'Id', textKey: 'AccountTitle', popupWidth: 420 });
        cb.vt = XCombo('CmbVehicleType', { columns: [{ key: 'VehicleDescription', caption: 'Vehicle Description' }], valueKey: 'Id', textKey: 'VehicleDescription', popupWidth: 260 });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'CustomerName', caption: 'Customer Name' }], valueKey: 'Id', textKey: 'CustomerName', popupWidth: 380 });
        cb.dateType = XCombo('cmbDateTypeHistory', { columns: [{ key: 'Parameters', caption: 'Parameters' }], valueKey: 'Id', textKey: 'Parameters', onSelect: dateTypeChanged });
        cb.term.setData([{ Id: 1, DeliveryTerm: 'Load' }, { Id: 2, DeliveryTerm: 'Ponch' }]);       // DeliveryTerm()
        cb.dateType.setData([{ Id: 1, Parameters: 'This Day' }, { Id: 2, Parameters: 'This Week' }, { Id: 3, Parameters: 'This Month' }, { Id: 4, Parameters: 'This Year' }, { Id: 5, Parameters: 'Financial Year' }]);
    }
    function keepSet(c, id) { if (id > 0 && !c.setValue(id)) c.clear(); }

    /* TransporterAcFill / VehicleTypefill / FillVehiclesNo: bind, then keep the previous value when it is still listed */
    function fillTransporters(rows) { var k = +cb.tr.value() || 0; S.tr = rows || []; cb.tr.setData(S.tr); keepSet(cb.tr, k); }
    function fillVehicleTypes(rows) { var k = +cb.vt.value() || 0; S.vt = rows || []; cb.vt.setData(S.vt); keepSet(cb.vt, k); }
    function fillVehicleNos(rows) {
        S.veh = rows || [];
        $('dlVehicleNos').innerHTML = S.veh.map(function (r) { return '<option value="' + SE.esc(vs(r, 'VehicleNo')) + '"></option>'; }).join('');
    }
    function fillGridLists(d) {
        S.wh = d.warehouses || []; S.cast = d.castingTypes || []; S.stage = d.stages || []; S.city = d.cities || [];
        if (G.grd) G.grd.refresh();
        if (G.hd) G.hd.refresh();
    }

    /* combdeliverytrm_TextChanged */
    function termChanged() {
        if (cb.term.text() === 'Ponch') { keepSet(cb.tr, S.outwardAc); if (!(S.outwardAc > 0)) cb.tr.clear(); return; }
        cb.tr.clear();
        $('txtFreightAmountHeader').value = '0';
    }
    function setTerm(text) {                        // combdeliverytrm.Text = text (TextChanged fires)
        var hit = cb.term.rows().filter(function (r) { return r.DeliveryTerm === text; })[0];
        if (hit) cb.term.setValue(hit.Id); else cb.term.clear();
        termChanged();
    }

    /* ------------------------------------------------------------------ customer / delivery order (BindPendingCustomer, BindPendingOrders, BindCustomerCity) */
    function bindPendingCustomer(doId) {
        var keep = +cb.sup.value() || 0;
        return SE.api(API + '/pending-customers' + SE.q({ doId: doId })).then(function (rows) {
            if (rows.length) {
                S.sup = rows;
                cb.sup.setData(rows);
                cb.sup.setValue(rows[0].SupplierCustomerId);                              // Rows[0].Activate()
                if (keep > 0 && !cb.sup.setValue(keep)) cb.sup.clear();
            } else { S.sup = []; cb.sup.setData([]); cb.sup.clear(); }
        });
    }
    function bindPendingOrders(supplierCustomerId) {
        var r = cb.sup.row();
        if (r && supplierCustomerId > 0) {
            cb.doNo.setData([{ DoId: vi(r, 'InvDeliveryOrderId'), DeliveryNo: vi(r, 'DoNo'), SupplierCustomerId: vi(r, 'SupplierCustomerId') }]);
            cb.doNo.setValue(vi(r, 'InvDeliveryOrderId'));                                // Rows[0].Activate()
        } else { cb.doNo.setData([]); cb.doNo.clear(); }
    }
    function bindCustomerCity() {
        $('txtCustomerCity').value = ''; $('txtCustomerCellNo').value = '';
        var r = cb.sup.row(), id = +cb.sup.value() || 0;
        if (r && id > 0) { $('txtCustomerCellNo').value = vs(r, 'MobileNo'); $('txtCustomerCity').value = vs(r, 'CityName'); }
    }
    /* comsupplier_ValueChanged */
    function supplierChanged() {
        return bindGridData().then(bindCustomerCity).catch(fail);
    }
    /* BindGridData */
    function bindGridData() {
        var r = cb.sup.row();
        if (!S.sup.length || !r || (+cb.sup.value() || 0) <= 0) return Promise.resolve();
        var supId = vi(r, 'SupplierCustomerId'), doId = vi(r, 'InvDeliveryOrderId');
        if (G.grd.count() > 0 && cb.doNo.row()) {
            var other = vi(cb.doNo.row(), 'SupplierCustomerId');
            if (supId !== other) {
                return SE.ask('you are changing Supplier and in that Case Data in Grid will be Reset,are you sure to do that?', 'Confirm').then(function (yes) {
                    if (!yes) cb.sup.setValue(other);
                });
            }
            bindPendingOrders(+cb.sup.value() || 0);
            return loadDeliveryOrder(supId, doId);
        }
        bindPendingOrders(+cb.sup.value() || 0);
        if (!r || supId === 0) { cb.sup.focus(); return Promise.reject(new Error('Please Select Customer first')); }
        if (doId === 0) { cb.sup.focus(); return Promise.reject(new Error('Delivery Order no Required,Please Select Customer for that')); }
        G.grd.setRows([]);                                                              // grd.ClearStructure()
        return loadDeliveryOrder(supId, doId);
    }
    function loadDeliveryOrder(supId, doId) {
        return SE.api(API + '/do-data' + SE.q({ customerId: supId, doId: doId })).then(function (rows) {
            if (!rows.length) return;
            loadDataDetailFromDeliveryOrder(rows);
            return availableStockUpdateInGrid();
        });
    }
    /* LoadDataDetailfromDeliveryOrder */
    function loadDataDetailFromDeliveryOrder(rows) {
        setTerm(vs(rows[0], 'DeliveryTerm'));
        $('txtremarks').value = vs(rows[0], 'RemarksHeader');
        keepSet(cb.vt, vi(rows[0], 'VehicleTypeId')); if (!(vi(rows[0], 'VehicleTypeId') > 0)) cb.vt.clear();
        $('CmbVehicleNo').value = vs(rows[0], 'VehicleNo');
        var out = [], dup = false;
        rows.forEach(function (d) {
            if (out.length) {                                                           // the desktop's flag is never cleared once a duplicate DoDetailId is met
                for (var j = 0; j < out.length; j++) if (+out[j].DoOrderDetailId === vi(d, 'DoDetailId')) { dup = true; break; }
            }
            if (dup) return;
            var qty = vn(d, 'DoQty'), wt = vn(d, 'ItemWeight');
            out.push({ Id: 0, OrderId: vi(d, 'SaleOrderId'), OrderDetailId: vi(d, 'SaleOrderDetailId'), OrderNo: v(d, 'SaleOderNo') !== undefined ? vi(d, 'SaleOderNo') : vi(d, 'SaleOrderNo'),
                DoOrderDetailId: vi(d, 'DoDetailId'), WarehouseId: vi(d, 'WareHouseId'), ItemId: vi(d, 'ItemId'), ItemName: vs(d, 'ItemName'), ItemCode: vs(d, 'ItemCode'),
                PackUomId: vi(d, 'PackUomId'), PackUom: vs(d, 'PackUom'), PackEquivalent: vn(d, 'PackEquivalent'), VariantId: vi(d, 'ItemVariantId'),
                VariantDescription: vs(d, 'VarientDescription'), CastingTypeId: vi(d, 'CastingTypeId'), ProductionStageId: 0, Remarks: vs(d, 'RemarksDetail'),
                ItemQty: qty, AvailableQty: qty, ItemWeight: wt, CastingWeight: qty * wt, CityId: vi(d, 'CityId') });
        });
        G.grd.setRows(out);
    }

    /* ------------------------------------------------------------------ stock (grd_CellUpdated / AvailableStockUpdateInGrid) */
    function stockReq(r) { return { itemId: +r.ItemId || 0, warehouseId: +r.WarehouseId || 0, castingTypeId: +r.CastingTypeId || 0, variantId: +r.VariantId || 0, packUomId: +r.PackUomId || 0 }; }
    function stocksFor(rows) {
        return SE.api(API + '/stocks' + SE.q({ docDate: $('DocDate').value }), { method: 'POST', body: rows.map(stockReq), quiet: true });
    }
    function availableStockUpdateInGrid() {
        var rows = G.grd.rows();
        if (!rows.length) return Promise.resolve();
        return stocksFor(rows).then(function (list) {
            rows.forEach(function (r, i) { r.AvailableQty = list[i] > 0 ? list[i] : 0; });
            G.grd.refresh();
        }).catch(fail);
    }
    function stockOfRow(r) {
        return stocksFor([r]).then(function (list) { r.AvailableQty = list[0] > 0 ? list[0] : 0; G.grd.refresh(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ grids */
    function makeGrids() {
        /* grdSettings: ItemQty is a text edit; CastingTypeId / ProductionStageId are drop-down lists, WarehouseId / CityId combos; Add is frozen at the left */
        G.grd = SE.grid('grd', { footer: false, frozen: 1, dec: 3,
            onBtn: function (k, r, i) { if (k === 'Add') addDetailRow(i); },
            onEdit: function (r, k, val) {
                if (k === 'ItemQty') {
                    var n = Number(String(val).replace(/,/g, '')); if (!isFinite(n)) return;
                    r.ItemQty = n; r.CastingWeight = n * SE.toNum(r.ItemWeight);
                } else if (k === 'WarehouseId' || k === 'CastingTypeId' || k === 'ProductionStageId' || k === 'CityId') {
                    r[k] = val === '' ? 0 : +val;
                    if (k === 'WarehouseId' || k === 'CastingTypeId') stockOfRow(r);
                }
            },
            cols: [
                { k: 'Add', t: '+', w: 22, btn: '+' },
                { k: 'Id', hide: true }, { k: 'OrderId', hide: true }, { k: 'OrderDetailId', hide: true },
                { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'DoOrderDetailId', hide: true },
                { k: 'WarehouseId', t: 'WareHouseName', w: 130, edit: true, list: function () { return S.wh; }, lk: 'Id', lt: 'WareHouseName' },
                { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 150 }, { k: 'ItemCode', t: 'ItemCode', w: 80 },
                { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'Uom', w: 55 }, { k: 'PackEquivalent', hide: true },
                { k: 'VariantId', hide: true }, { k: 'VariantDescription', t: 'Modal Description', w: 110 },
                { k: 'CastingTypeId', t: 'Casting Type', w: 100, edit: true, list: function () { return S.cast; }, lk: 'Id', lt: 'LookupName' },
                { k: 'ProductionStageId', t: 'Production Stage', w: 110, edit: true, list: function () { return S.stage; }, lk: 'Id', lt: 'LookupName' },
                { k: 'Remarks', t: 'Specification/Remarks', w: 130 },
                { k: 'ItemQty', t: 'Qty', w: 70, f: 'n3', edit: true }, { k: 'AvailableQty', t: 'AvailableQty', w: 80, f: 'n3' },
                { k: 'ItemWeight', t: 'Item FG Weight', w: 80, f: 'n3' }, { k: 'CastingWeight', t: 'CastingWeight', w: 80, f: 'n3' },
                { k: 'CityId', t: 'City Name', w: 100, edit: true, list: function () { return S.city; }, lk: 'Id', lt: 'CityName' }] });
        G.pend = SE.grid('grdPendingOrders', { frozen: 1, footer: false, dec: 3,
            onBtn: function (k, r) { if (k === 'Load') loadPending(r); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(+r.DocumentTypeId || DO_DOC, +r.Id); },
            cols: [
                { k: 'Load', t: 'Load', w: 50, btn: 'Load' },
                { k: 'Id', hide: true }, { k: 'DocumentTypeId', hide: true }, { k: 'SaleOrderId', hide: true }, { k: 'OrderTypeId', hide: true },
                { k: 'OrderType', t: 'OrderType', w: 100 }, { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' },
                { k: 'CustomerName', t: 'CustomerName', w: 190 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'ItemQty', t: 'ItemQty', w: 70, f: 'n3' },
                { k: 'ItemWeight', t: 'Item FG Weight', w: 90, f: 'n3' }, { k: 'CastingWeight', t: 'CastingWeight', w: 90, f: 'n3' },
                { k: 'RemarksHeader', t: 'RemarksHeader', w: 160 }, { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }] });
        G.hist = SE.grid('grdHistory', { frozen: 3, dec: 3, onDbl: function (r) { histEdit(r); },
            onBtn: function (k, r) { if (k === 'Edit') histEditButton(r); else if (k === 'Print') print(+r.Id); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(DOC, +r.Id); },
            onSel: histSelected,
            cols: [
                { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 50, btn: 'Print' },
                { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true },
                { k: 'Id', hide: true }, { k: 'DocumentTypeId', hide: true },
                { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' }, { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'SupplierCustomerId', hide: true },
                { k: 'CustomerName', t: 'CustomerName', w: 170 }, { k: 'OrderType', hide: true }, { k: 'OrderId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 70 },
                { k: 'ReferenceNo', t: 'ReferenceNo', w: 90 }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 85 }, { k: 'TransporterName', t: 'TransporterName', w: 150 },
                { k: 'CarriageAmount', t: 'CarriageAmount', w: 100, f: 'n2', sum: true }, { k: 'GpNo', t: 'GpNo', w: 60 }, { k: 'GpDate', t: 'GpDate', w: 85, f: 'sdate' },
                { k: 'VehicleType', t: 'VehicleType', w: 100 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'DriverName', t: 'DriverName', w: 110 },
                { k: 'DriverCNICNo', t: 'DriverCNICNo', w: 120 }, { k: 'DriverCellNo', t: 'DriverCellNo', w: 100 }, { k: 'IsApproved', t: 'IsApproved', w: 80 },
                { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'EntryUserName', t: 'EntryUserName', w: 100 },
                { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ModifyUserName', t: 'ModifyUserName', w: 100 }, { k: 'RemarksHeader', t: 'RemarksHeader', w: 150 }] });
        G.hd = SE.grid('grdDetailHistory', { dec: 3, footer: false,
            cols: [{ k: 'Id', hide: true }, { k: 'OrderId', hide: true }, { k: 'OrderDetailId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'DoOrderDetailId', hide: true },
                { k: 'Warehouse', t: 'Warehouse', w: 130 }, { k: 'ItemId', hide: true }, { k: 'ItemName', t: 'ItemName', w: 150 }, { k: 'ItemCode', t: 'ItemCode', w: 80 },
                { k: 'PackUomId', hide: true }, { k: 'PackUom', t: 'Uom', w: 55 }, { k: 'PackEquivalent', hide: true }, { k: 'VariantId', hide: true },
                { k: 'VariantDescription', t: 'Modal Description', w: 110 }, { k: 'CastingType', t: 'CastingType', w: 100 }, { k: 'ProductionStage', t: 'ProductionStage', w: 110 },
                { k: 'Remarks', t: 'Specification/Remarks', w: 130 }, { k: 'ItemQty', t: 'Qty', w: 70, f: 'n3' }, { k: 'ItemWeight', t: 'Item FG Weight', w: 80, f: 'n3' },
                { k: 'CastingWeight', t: 'CastingWeight', w: 80, f: 'n3' }, { k: 'CityName', t: 'CityName', w: 100 }] });
    }

    /* AddDetailRow: a copy of the row with Id = 0 */
    function addDetailRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        var c = JSON.parse(JSON.stringify(r)); c.Id = 0;
        G.grd.addRow(c);
    }
    /* DeleteDetailRow */
    function deleteDetailRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        if ((+r.Id || 0) > 0) {
            return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
                if (!yes) return;
                S.remove.push(lineOf(r));
                G.grd.removeAt(i);
            });
        }
        G.grd.removeAt(i);
    }

    /* grdPendingOrders_ColumnButtonClick "Load" */
    function loadPending(r) {
        if (!isShown('btnSave') || $('btnSave').disabled || G.grd.count() !== 0) return msg('Please Reset the Form First to Load New Data');
        return bindPendingCustomer(+r.Id).then(supplierChanged).then(function () { cb.sup.focus(); }).catch(fail);
    }

    /* ------------------------------------------------------------------ Load (InitializeComponentMethod) */
    function applyInitial(d) {
        S.rights = d.rights || {};
        S.outwardAc = d.outwardFreightAccountId || 0;
        S.histDays = d.historyDays || 3;
        S.fyStart = d.fyStart || '';
        if (!(S.id > 0) && d.nextNo > 0) $('txtdocno').value = String(d.nextNo);
        fillVehicleTypes(d.vehicleTypes); fillVehicleNos(d.vehicleNos); fillTransporters(d.transporters); fillGridLists(d);
        G.grd.setRows([]);
        setTerm('Load');                                                                  // DeliveryTerm(): Rows[0].Activate() -> TextChanged
        G.pend.setRows(d.pending || []);
        $('txtFromdateHistory').value = SE.addDays(-S.histDays); $('txtToDateHistory').value = SE.today();
        if (S.outwardAc > 0) { if (!cb.tr.setValue(S.outwardAc)) cb.tr.clear(); }
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', tabChanged);
        SE.digitsOnly($('txtgatepassno')); SE.digitsOnly($('txtFreightAmountHeader')); SE.digitsOnly($('txtFromNoHistory')); SE.digitsOnly($('txtToDocNoHistory'));
        makeGrids();
        $('DocDate').value = SE.today(); $('txtgpdate').value = SE.today();
        SE.api(API + '/initial').then(function (d) {
            applyInitial(d);
            $('btnSave').disabled = !S.rights.save; $('btnUpdate').disabled = !S.rights.update; $('btnDelete').disabled = !S.rights.delete; $('btnSlip').disabled = !S.rights.print;
            $('ChkPrintslip').checked = !!S.rights.print;
            show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
            $('DocDate').focus();
        }).catch(function (e) { SE.alert('Error occurred during database call.'); });
        wire();
    }

    /* ------------------------------------------------------------------ Reset (btnNew_Click / Reset) */
    function reset() {
        S.files = []; S.removedAtt = []; S.existing = []; S.remove = []; S.id = 0; S.approved = false;
        cb.tr.clear(); $('txtSupplierReference').value = ''; $('txtremarks').value = ''; $('txtFreightAmountHeader').value = ''; $('txtgatepassno').value = '';
        $('CmbVehicleNo').value = ''; cb.term.clear(); $('txtDriverName').value = ''; $('txtDriverCellNo').value = ''; $('txtCNICNO').value = '';
        $('txtgpdate').value = SE.today();
        cb.sup.clear(); cb.sup.el.disabled = false; syncCombo(cb.sup); S.sup = []; cb.sup.setData([]);
        cb.doNo.clear(); cb.doNo.el.disabled = false; syncCombo(cb.doNo); cb.doNo.setData([]);
        termChanged();                                                                    // combdeliverytrm.Text = "" -> TextChanged
        $('txtCustomerCity').value = ''; $('txtCustomerCellNo').value = '';
        $('DocDate').focus();
        G.grd.setRows([]);
        show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
        return SE.api(API + '/reset-data').then(function (d) {                           // FillGrdPendingOrders + GenerateCode
            G.pend.setRows(d.pending || []);
            if (d.nextNo > 0) $('txtdocno').value = String(d.nextNo);
        }).catch(fail);
    }
    function syncCombo(c) { if (c.el.__dtcombo && c.el.__dtcombo.syncFromSelect) c.el.__dtcombo.syncFromSelect(); }

    /* btnRefresh_Click */
    function refresh() {
        return SE.api(API + '/lists').then(function (d) {
            fillVehicleTypes(d.vehicleTypes); fillVehicleNos(d.vehicleNos); fillTransporters(d.transporters); fillGridLists(d);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Insert / btnUpdate_Click */
    function lineOf(r) {
        return { id: +r.Id || 0, orderId: +r.OrderId || 0, orderDetailId: +r.OrderDetailId || 0, doOrderDetailId: +r.DoOrderDetailId || 0, warehouseId: +r.WarehouseId || 0,
            itemId: +r.ItemId || 0, packUomId: +r.PackUomId || 0, variantId: +r.VariantId || 0, castingTypeId: +r.CastingTypeId || 0, productionStageId: +r.ProductionStageId || 0,
            remarks: r.Remarks == null ? '' : String(r.Remarks), itemQty: SE.toNum(r.ItemQty), cityId: +r.CityId || 0 };
    }
    /* FormValidation */
    function formValidation() {
        function stop(m, f) { msg(m).then(function () { if (f) f(); }); return false; }
        var no = field('txtdocno');
        if (no === '' || no === '0') return stop('DocNo Field is Required', function () { $('txtdocno').focus(); });
        if (!cb.sup.row() || !(+cb.sup.value())) return stop('CustomerName Field is Required', function () { cb.sup.focus(); });
        if (!cb.doNo.row() || !(+cb.doNo.value())) return stop('Delivery Order No Field is Required', function () { cb.doNo.focus(); });
        var f = SE.toNum($('txtFreightAmountHeader').value), t = +cb.tr.value() || 0;
        if (f !== 0 && t === 0) return stop('Carriage Ac is Required when Carriage amount is greater than Zero', function () { cb.tr.focus(); });
        if (f === 0 && t !== 0) return stop('Carriage Amount is Required when Carriage Ac is Selected', function () { $('txtFreightAmountHeader').focus(); });
        if (!cb.term.row() || !(+cb.term.value())) return stop('Delivery Term Field is Required', function () { cb.term.focus(); });
        return true;
    }
    function insert() {
        if (G.grd.count() === 0) return msg('Grid Record Not Found', 'Error Message');
        if (!formValidation()) return;
        var carriage = SE.toNum($('txtFreightAmountHeader').value.trim());
        if (carriage > 0) {
            var tr = +cb.tr.value() || 0;
            if (tr === 0) { cb.tr.focus(); return msg('Transporter Account field Required', 'Error Message'); }
            var sr = cb.sup.row(), gl = (+cb.sup.value() > 0 && sr) ? vi(sr, 'GlAccountId') : 0;
            if (tr === gl) return msg('Transporter Account can not be same as Customer Please check', 'Error Message');
        }
        return SE.ask(S.id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
            if (!yes) return;
            var body = {
                id: S.id, docDate: $('DocDate').value, docNo: field('txtdocno'), customerId: +cb.sup.value() || 0, deliveryOrderId: +cb.doNo.value() || 0,
                referenceNo: $('txtSupplierReference').value.trim(), deliveryTerm: cb.term.text(), transporterId: +cb.tr.value() || 0, carriage: $('txtFreightAmountHeader').value.trim(),
                gpNo: $('txtgatepassno').value.trim(), gpDate: $('txtgpdate').value, vehicleTypeId: +cb.vt.value() || 0, vehicleType: cb.vt.text(), vehicleNo: $('CmbVehicleNo').value,
                driverName: $('txtDriverName').value, driverCellNo: $('txtDriverCellNo').value, driverCnic: $('txtCNICNO').value, remarks: $('txtremarks').value.trim(),
                lines: G.grd.rows().map(lineOf), removed: S.id > 0 ? S.remove : [],
                attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt }
            };
            return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                var preview = $('ChkPrintslip').checked;
                return SE.alert(r.message).then(function () {
                    return reset().then(function () { if (preview) print(r.id); });
                });
            }).catch(function (e) { return SE.alert(e.message, 'Error Message'); });
        });
    }
    function btnSave() { S.id = 0; return insert(); }
    function btnUpdate() {
        if (S.id === 0) return msg('Record Not found');
        if (S.approved) return msg('Record Not Update because Record has approved');
        return insert();
    }

    /* ------------------------------------------------------------------ ReadById */
    function readById(id) {
        return reset().then(function () {
            S.id = id;
            return SE.api(API + '/' + id);
        }).then(function (d) {
            var h = d.head, lines = d.lines || [];
            tabs.select('tabPage1');
            show('btnSave', false); show('btnUpdate', true); show('btnDelete', true);
            $('DocDate').focus();
            $('DocDate').value = SE.dateInput(v(h, 'DocDate'));
            $('txtdocno').value = vs(h, 'DocNo');
            $('txtSupplierReference').value = vs(h, 'ReferenceDocNo');
            var supId = vi(h, 'SupplierCustomerId');
            S.sup = [{ SupplierCustomerId: supId, CustomerName: vs(h, 'CompanyName'), InvDeliveryOrderId: vi(h, 'OrderId'), DoNo: vi(h, 'OrderNo'), GlAccountId: vi(h, 'CustomerGlAccountId'),
                CityName: vs(h, 'CustomerCityName'), MobileNo: vs(h, 'CustomerMobileNo') }];
            cb.sup.setData(S.sup); cb.sup.setValue(supId); cb.sup.el.disabled = true; syncCombo(cb.sup);
            bindPendingOrders(supId); cb.doNo.el.disabled = true; syncCombo(cb.doNo);
            bindCustomerCity();
            $('txtremarks').value = vs(h, 'RemarksHeader');
            setTerm(vs(h, 'DeliveryTerm'));
            if (!cb.tr.setValue(vi(h, 'TransporterId'))) cb.tr.clear();
            $('txtFreightAmountHeader').value = String(vn(h, 'CarriageAmount'));
            $('txtgatepassno').value = vs(h, 'GpNo');
            $('txtgpdate').value = SE.dateInput(v(h, 'GPDate') !== undefined ? v(h, 'GPDate') : v(h, 'GpDate')) || SE.today();
            if (!cb.vt.setValue(vi(h, 'VehicleTypeId'))) cb.vt.clear();
            $('CmbVehicleNo').value = vs(h, 'VehicleNo');
            $('txtDriverName').value = vs(h, 'DriverName'); $('txtDriverCellNo').value = vs(h, 'DriverCellNo'); $('txtCNICNO').value = vs(h, 'DriverCNIC');
            S.approved = !!v(h, 'IsApproved');
            G.grd.setRows(lines.map(function (l) {
                var q = vn(l, 'ItemQty'), w = vn(l, 'ItemWeight');
                return { Id: vi(l, 'Id'), OrderId: vi(l, 'SaleOrderId'), OrderDetailId: vi(l, 'SaleOrderDetailId'), OrderNo: vi(l, 'SaleOrderNo'), DoOrderDetailId: vi(l, 'InvDeliveryOrderDetailId'),
                    WarehouseId: vi(l, 'WarehouseId'), ItemId: vi(l, 'ItemId'), ItemName: vs(l, 'Item'), ItemCode: vs(l, 'ItemCode'), PackUomId: vi(l, 'ItemUomId'), PackUom: vs(l, 'UOMCode'),
                    PackEquivalent: vn(l, 'UOMEquivalent'), VariantId: vi(l, 'ItemVariantId'), VariantDescription: vs(l, 'VarientDescription'), CastingTypeId: vi(l, 'CastingTypeId'),
                    ProductionStageId: vi(l, 'ProductionStageId'), Remarks: vs(l, 'CommentsDetail'), ItemQty: q, AvailableQty: q, ItemWeight: w, CastingWeight: q * w, CityId: vi(l, 'CityId') };
            }));
            S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ delete (btnDelete_Click) */
    function del() {
        if (S.id === 0) return msg('Record Id not found for deletion...');
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return SE.alert(r.message).then(reset); }).catch(fail);
        });
    }

    /* ------------------------------------------------------------------ history */
    function tabChanged(id) {                                                              // tabControl1_SelectedIndexChanged
        if (id !== 'tabPage2') return;
        if (!cb.hCust.rows().length) historyComboBind();
        if (!(+cb.dateType.value())) { cb.dateType.setValue(1); dateTypeChanged(); }       // Rows[0].Activate() -> ValueChanged
        cb.dateType.focus();
    }
    function historyComboBind() {
        var k = +cb.hCust.value() || 0;
        return SE.api(API + '/history-customers').then(function (rows) { cb.hCust.setData(rows || []); keepSet(cb.hCust, k); }).catch(fail);
    }
    /* cmbDateTypeHistory_ValueChanged */
    function dateTypeChanged() {
        var t = +cb.dateType.value() || 0, today = SE.today(), n = new Date();
        if (t === 1) $('txtFromdateHistory').value = today;
        else if (t === 2) $('txtFromdateHistory').value = SE.addDays(-7);
        else if (t === 3) { $('txtFromdateHistory').value = n.getFullYear() + '-' + ('0' + (n.getMonth() + 1)).slice(-2) + '-01'; $('txtToDateHistory').value = today; }
        else if (t === 4) { $('txtFromdateHistory').value = n.getFullYear() + '-01-01'; $('txtToDateHistory').value = today; }
        else if (t === 5) { $('txtFromdateHistory').value = SE.dateInput(S.fyStart) || $('txtFromdateHistory').value; }
    }
    /* FillHistory */
    function showHistory() {
        var dt = $('rdentrydate').checked ? 'entry' : ($('rdmodifydate').checked ? 'modify' : ($('rdapproveddate').checked ? 'approved' : 'document'));
        if ($('drdocdate').checked) dt = 'document';
        var q = { dateType: dt, fromDate: $('txtFromdateHistory_chk').checked ? $('txtFromdateHistory').value : '', toDate: $('txtToDateHistory_chk').checked ? $('txtToDateHistory').value : '',
            fromNo: SE.toInt($('txtFromNoHistory').value), toNo: SE.toInt($('txtToDocNoHistory').value), customerId: +cb.hCust.value() || 0 };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            if (!rows.length) { G.hist.setRows([]); G.hd.setRows([]); return; }
            G.hist.setRows(rows.map(function (r) {
                return { Id: vi(r, 'Id'), DocumentTypeId: vi(r, 'DocumentTypeId'), DocDate: v(r, 'DocDate'), DocNo: vi(r, 'DocNo'), SupplierCustomerId: vi(r, 'SupplierCustomerId'), CustomerName: vs(r, 'CustomerName'),
                    OrderType: vs(r, 'OrderType'), OrderId: vi(r, 'OrderId'), OrderNo: vi(r, 'OrderNo'), ReferenceNo: vs(r, 'ReferenceDocNo'), DeliveryTerm: vs(r, 'DeliveryTerm'),
                    TransporterName: vs(r, 'AccountTitle'), CarriageAmount: vn(r, 'Freight'), GpNo: vi(r, 'GpNo'), GpDate: v(r, 'GpDate'), VehicleType: vs(r, 'VehicleType'), VehicleNo: vs(r, 'VehicleNo'),
                    DriverName: vs(r, 'DriverName'), DriverCNICNo: vs(r, 'DriverCNIC'), DriverCellNo: vs(r, 'DriverCellNo'), IsApproved: v(r, 'IsApproved') ? 'Yes' : 'No', EntryDate: v(r, 'EntryDate'),
                    EntryUserName: vs(r, 'UserName'), ModifyDate: v(r, 'ModifyDate'), ModifyUserName: vs(r, 'ModifyUserName'), RemarksHeader: vs(r, 'RemarksHeader'), NoOfAttachments: vi(r, 'NoOfAttachments') };
            }));
        }).catch(function (e) { return SE.alert(e.message, 'Message'); });
    }
    /* Resethistory */
    function resetHistory() {
        cb.dateType.setValue(3); dateTypeChanged();
        $('txtFromNoHistory').value = ''; $('txtToDocNoHistory').value = ''; cb.hCust.clear(); cb.dateType.focus();
    }
    /* grdHistory_SelectionChanged -> GetDetailGrdByHeadId (the desktop also empties the form's dtGrid here) */
    var histSeq = 0;
    function histSelected(item) {
        if (!item) return;
        var seq = ++histSeq;
        SE.api(API + '/' + item.Id + '/detail', { quiet: true }).then(function (lines) {
            if (seq !== histSeq) return;
            if (!lines || !lines.length) { G.hd.setRows([]); return; }
            G.grd.setRows([]);                                                         // dtGrid.Rows.Clear()
            G.hd.setRows(lines.map(function (l) {
                var q = vn(l, 'ItemQty'), w = vn(l, 'ItemWeight');
                return { Id: vi(l, 'Id'), OrderId: vi(l, 'SaleOrderId'), OrderDetailId: vi(l, 'SaleOrderDetailId'), OrderNo: vi(l, 'SaleOrderNo'), DoOrderDetailId: vi(l, 'InvDeliveryOrderDetailId'),
                    Warehouse: vs(l, 'WareHouseCode'), ItemId: vi(l, 'ItemId'), ItemName: vs(l, 'Item'), ItemCode: vs(l, 'ItemCode'), PackUomId: vi(l, 'ItemUomId'), PackUom: vs(l, 'UOMCode'),
                    PackEquivalent: vn(l, 'UOMEquivalent'), VariantId: vi(l, 'ItemVariantId'), VariantDescription: vs(l, 'VarientDescription'), CastingType: vs(l, 'CastingType'),
                    ProductionStage: vs(l, 'ProductionProcessStage'), Remarks: vs(l, 'CommentsDetail'), ItemQty: q, ItemWeight: w, CastingWeight: q * w, CityName: vs(l, 'AreaCity') };
            }));
        }).catch(function () { if (seq === histSeq) G.hd.setRows([]); });
    }
    /* grdHistory_DoubleClick */
    function histEdit(r) { if (!r) return; return readById(+r.Id); }
    /* grdHistory_ColumnButtonClick "Edit" */
    function histEditButton(r) { return readById(+r.Id); }

    /* ------------------------------------------------------------------ print / attachments */
    function print(id) {                                                                   // GenerateCustomerSlip
        id = +id || 0;
        return SE.api(API + '/' + id + '/slip-check', { quiet: true }).then(function (r) {
            if (!r || !r.rows) return msg('Not Record Found For Display');
            SE.printRpt('1659_GdnSlip_Engr.rpt', { id: id, documentTypeId: DOC });
        }).catch(fail);
    }
    function showAttachments(docType, id) {
        return SE.api(API + '/attachments/' + docType + '/' + id).then(function (rows) { renderAttachments(rows, docType, id, true); }).catch(fail);
    }
    function openAttachmentDialog() {
        var rows = (S.existing || []).filter(function (r) { return S.removedAtt.indexOf(r.Id) < 0; });
        renderAttachments(rows, DOC, S.id, false);
    }
    function renderAttachments(rows, docType, id, readOnly) {
        var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>Attachment</th><th style="width:90px">Action</th></tr></thead><tbody>';
        rows.forEach(function (r) {
            h += '<tr><td>' + SE.esc(r.Attachment) + '</td><td>' + (id > 0 ? '<a class="lnk" href="' + API + '/attachments/' + docType + '/' + id + '/' + r.Id + '" target="_blank">Open</a>' : '') +
                 (readOnly ? '' : ' <a class="lnk" data-rm="' + r.Id + '" style="cursor:pointer">Remove</a>') + '</td></tr>';
        });
        S.files.forEach(function (f, i) { if (!readOnly) h += '<tr><td>' + SE.esc(f.name) + ' (new)</td><td><a class="lnk" data-nf="' + i + '" style="cursor:pointer">Remove</a></td></tr>'; });
        h += '</tbody></table></div>';
        if (!readOnly) h += '<div style="padding:6px"><input type="file" id="atFile" multiple> <span class="se-note">Up to 5 MB each. Files are stored when the goods dispatch note is saved.</span></div>';
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

    /* ------------------------------------------------------------------ grd_KeyDown F1 (GrdPopUp) */
    function pickPopup(title, rows, idKey, nameKey, done) {
        var h = '<div class="dgrid" style="max-height:50vh"><table class="jg"><thead><tr><th>' + SE.esc(title) + '</th></tr></thead><tbody>' +
            rows.map(function (r) { return '<tr data-id="' + SE.esc(r[idKey]) + '" data-nm="' + SE.esc(r[nameKey]) + '" style="cursor:pointer"><td>' + SE.esc(r[nameKey]) + '</td></tr>'; }).join('') + '</tbody></table></div>';
        var pop = SE.pop('Select', h, [{ t: 'Close' }]);
        pop.open();
        pop.body.addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-id]'); if (!tr) return;
            done(+tr.getAttribute('data-id'), tr.getAttribute('data-nm')); pop.close();
        });
    }
    function f1(r) {
        var k = G.grd.curKey();
        if (k === 'WarehouseId') pickPopup('WareHouseName', S.wh, 'Id', 'WareHouseName', function (id) { r.WarehouseId = id; G.grd.refresh(); stockOfRow(r); });
        else if (k === 'VariantId' || k === 'VariantDescription') {
            if ((+r.ItemId || 0) <= 0) return msg('Please Select an Item First');
            SE.api(API + '/variants' + SE.q({ itemId: r.ItemId })).then(function (rows) {
                pickPopup('ItemAttribute', rows, 'VariantId', 'VariantDescription', function (id, nm) { r.VariantId = id; r.VariantDescription = nm; G.grd.refresh(); stockOfRow(r); });
            }).catch(fail);
        }
        else if (k === 'CastingTypeId') pickPopup('LookupName', S.cast, 'Id', 'LookupName', function (id) { r.CastingTypeId = id; G.grd.refresh(); stockOfRow(r); });
        else if (k === 'ProductionStageId') pickPopup('LookupName', S.stage, 'Id', 'LookupName', function (id) { r.ProductionStageId = id; G.grd.refresh(); });
        else if (k === 'CityId') pickPopup('CityName', S.city, 'Id', 'CityName', function (id) { r.CityId = id; G.grd.refresh(); });
    }

    /* ------------------------------------------------------------------ shortcut keys (InvfrmPurchasedirectInvoice_KeyDown_1 :2582, MakeShortCutKeys :2702) */
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print'], ['Alt+1', 'For Print Slip 1659'], ['Ctrl+F5', 'For Focus on DocDate'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function onKey(e) {
        var t = e.target;
        if (e.key === 'Enter' && !e.ctrlKey && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'),
                function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        var k = e.key.toLowerCase();
        show('btnRecordsUpdate', false);
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); $('DocDate').focus(); } else { tabs.select('tabPage2'); } return; }
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (e.key === 'Escape' && document.querySelector('.se-pop.on')) return; e.preventDefault(); history.back(); return; }
        if (e.ctrlKey && e.altKey) { SE.shortcuts(SHORT); return; }
        if (tabs.index() === 0) {
            if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnSave') && !$('btnSave').disabled) btnSave(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnUpdate') && !$('btnUpdate').disabled) btnUpdate(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && e.shiftKey && e.key === 'Delete') { e.preventDefault(); if (isShown('btnDelete') && !$('btnDelete').disabled) del(); }
            else if (e.ctrlKey && e.shiftKey && k === 'f') { show('btnRecordsUpdate', true); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); cb.sup.focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') { e.preventDefault(); if (document.activeElement === $('grd')) $('grdPendingOrders').focus(); else $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { cb.sup.focus(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachmentDialog(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); print(S.id); }
            else if (e.altKey && e.key === '1') { e.preventDefault(); print(S.id); }
            else if (t && t.closest && t.closest('#grd')) gridKeys(e);
        } else {
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('grdHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grdDetailHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') { e.preventDefault(); if (document.activeElement === $('grdDetailHistory')) $('grdHistory').focus(); else $('grdDetailHistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { $('grdHistory').focus(); }
            else if (e.ctrlKey && e.key === 'Enter' && G.hist.cur() && S.rights.update) { e.preventDefault(); histEdit(G.hist.cur()); }
            else if (e.ctrlKey && k === 'p' && S.rights.print && G.hist.cur() && t && t.closest && t.closest('#grdHistory')) { e.preventDefault(); print(+G.hist.cur().Id); }
        }
    }
    /* grd_KeyDown: Ctrl+D copies the row, Ctrl+Delete removes it, F1 opens the value pop-up of the current column */
    function gridKeys(e) {
        var i = G.grd.curIndex(); if (i < 0) return;
        if (e.ctrlKey && (e.key === 'd' || e.key === 'D')) { e.preventDefault(); addDetailRow(i); }
        else if (e.ctrlKey && e.key === 'Delete') { e.preventDefault(); deleteDetailRow(i); }
        else if (e.key === 'F1') { e.preventDefault(); f1(G.grd.rows()[i]); }
    }

    function wire() {
        $('btnNew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnSave').onclick = btnSave; $('btnUpdate').onclick = btnUpdate; $('btnDelete').onclick = del;
        $('btnAttachment').onclick = openAttachmentDialog; $('btnSlip').onclick = function () { print(S.id); };
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('BtnNewHistory').onclick = resetHistory; $('BtnRefreshHistory').onclick = historyComboBind; $('btnshow').onclick = showHistory;
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
