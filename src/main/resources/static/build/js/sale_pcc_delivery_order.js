/*
 * Screen 549  DeliveryOrderConcrete  (Architecture.WinApp.pcc.Sale.DeliveryOrderConcrete, document type 1853)
 * Page script. Desktop methods are named in the comments (DeliveryOrderConcrete.cs). Server: /sale/pcc/delivery-order/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/pcc/delivery-order/api', PRINT = '/sale/pcc/print/delivery-order-slip';
    var S = { id: 0, approved: false, rights: {}, remove: [], updateIdx: -1, files: [], removedAtt: [], existing: [], ordIds: '', ordMainIds: '',
        cust: [], wh: [], job: [], city: [], refs: [], items: [], hist: [], fyStart: '', dateTypes: [] };
    var cb = {}, G = {}, tabs;

    function fail(e) { return SE.dbError(e); }
    function msg(t) { return SE.alert(t); }
    function iv(c) { return +c.value() || 0; }
    function isShown(id) { var b = $(id); return !!b && b.style.display !== 'none' && !b.hidden; }
    function show(id, on) { var b = $(id); if (!b) return; b.style.display = on ? '' : 'none'; if (b.hasAttribute('hidden')) b.hidden = !on; }
    function dec(v) { return SE.toNum(v); }
    function d2(v) { return v === '' || v == null ? '' : SE.num(SE.toNum(v), 2, 0); }       // "##,#.##"
    function setBy(c, id, key) { if (c.setValue(id)) return true; return false; }
    function textOf(id) { return ($(id).value || '').trim(); }

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.cust = XCombo('CmbSupplierCustomer', { columns: [{ key: 'CompanyName', caption: 'Customer Name' }], textKey: 'CompanyName', popupWidth: 420,
            onSelect: function () { saleOrderBind(); } });
        cb.order = XCombo('CmbOrderNo', { columns: [{ key: 'OrderNo', caption: 'Sale Order No' }], valueKey: 'OrderId', textKey: 'OrderNo',
            onSelect: function () { itemBindByOrder(); stockLabel(); } });
        cb.item = XCombo('CmbItemName', { columns: [{ key: '_t', caption: 'Item Name' }], valueKey: 'ItemId', textKey: '_t', popupWidth: 420, onSelect: itemLeave });
        cb.varient = XCombo('CmbAttributeVarient', { columns: [{ key: 'ItemAttribute', caption: 'Varient' }], valueKey: 'ItemAttributeVarientId', textKey: 'ItemAttribute',
            onSelect: varientChanged });
        cb.wh = XCombo('CmbWareHouse', { columns: [{ key: 'WareHouseName', caption: 'WareHouse Name' }], textKey: 'WareHouseName', onSelect: stockLabel });
        cb.job = XCombo('CmbJobLot', { columns: [{ key: 'JobLotDescription', caption: 'Job Lot' }], textKey: 'JobLotDescription', onSelect: stockLabel });
        cb.city = XCombo('combcityarea', { columns: [{ key: 'CityName', caption: 'City Name' }], textKey: 'CityName' });
        cb.storey = XCombo('Cmbstoreys', { columns: [{ key: 'LookupName', caption: 'Building Storey' }], textKey: 'LookupName' });
        cb.height = XCombo('Cmbheight', { columns: [{ key: 'LookupName', caption: 'Building Height' }], textKey: 'LookupName' });
        cb.vtype = XCombo('CmbVehicleType', { columns: [{ key: 'VehicleDescription', caption: 'Vehicle Type' }], textKey: 'VehicleDescription' });
        cb.hCust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Name', caption: 'Customer Name' }], textKey: 'Name', popupWidth: 380 });
        cb.hItem = XCombo('CmbItemHistory', { columns: [{ key: 'Name', caption: 'ItemName' }], textKey: 'Name', popupWidth: 380 });
        cb.hVeh = XCombo('CmbVehicleNoHistory', { columns: [{ key: 'Name', caption: 'VehicleNo' }], textKey: 'Name' });
        cb.dateType = XCombo('cmbDateTypeHistory', { columns: [{ key: 'Parameters', caption: 'Parameters' }], textKey: 'Parameters', onSelect: dateTypeChanged });
    }
    function keep(c, rows, fn) {                                 // desktop pattern: rebind, restore the previous Value if it is still in the list
        var k = iv(c);
        c.setData(rows || []);
        if (k > 0 && !c.setValue(k)) c.clear();
        if (fn) fn();
    }
    function fillLists(d) {
        S.refs = d.refParties || S.refs; S.wh = d.warehouses || S.wh; S.job = d.jobLots || S.job; S.city = d.cities || S.city; S.cust = d.customers || S.cust;
        SPC.fillDatalist('CmbReferenceParty', S.refs, 'ReferencePartyName');                       // ReferencePartyFill
        keep(cb.storey, d.storeys); keep(cb.height, d.heights);                                  // BuildingStoreysFill / BuildingHeightFill
        keep(cb.vtype, d.vehicleTypes);                                                          // vehicleTypefill
        SPC.fillDatalist('CmbVehicleNo', d.vehicles || [], 'VehicleNo');                         // BindVehicles
        keep(cb.cust, S.cust); keep(cb.wh, S.wh); keep(cb.job, S.job); keep(cb.city, S.city);    // SupplierNameFill / WareHouseFill / JobLotFill / BinCity
        G.grd.refresh();
    }

    /* SaleOrderBind (CmbSupplierCustomer_TextChanged / _Leave) */
    function saleOrderBind() {
        var c = iv(cb.cust);
        if (c <= 0) return Promise.resolve();
        return SE.api(API + '/order-nos' + SE.q({ customerId: c })).then(function (rows) {
            if (rows.length) cb.order.setData(rows); else { cb.order.clear(); cb.order.setData([]); }
        }).catch(fail);
    }
    /* ItemBindbyOrderId (CmbOrderNo_TextChanged / _Leave) */
    function bindItems(rows) {
        var byName = $('rdbtnItemName').checked;
        cb.item.setData(rows.map(function (r) { return { ItemId: r.ItemId, ItemName: r.ItemName, ItemCode: r.ItemCode, ItemWeight: r.ItemWeight, OrderDetailId: r.OrderDetailId, DocumentTypeId: r.DocumentTypeId,
            _t: byName ? r.ItemName : r.ItemCode }; }));
    }
    function itemBindByOrder() {
        var o = iv(cb.order), k = iv(cb.item);
        return SE.api(API + '/order-items' + SE.q({ orderId: o })).then(function (rows) {
            S.items = rows.map(function (r) { return { ItemId: r.OrderItemId, ItemName: r.ItemName, ItemCode: r.ItemCode, ItemWeight: r.ItemWeight, OrderDetailId: r.OrderDetailId, DocumentTypeId: r.DocumentTypeId }; });
            if (S.items.length) { bindItems(S.items); if (k > 0 && !cb.item.setValue(k)) cb.item.clear(); }
            else { cb.item.clear(); cb.item.setData([]); }
        }).catch(fail);
    }
    /* CmbItemName_Leave */
    function itemLeave() {
        var id = iv(cb.item), r = cb.item.row();
        if (id > 0 && r) {
            $('txtItemWeight').value = r.ItemWeight == null ? '' : String(r.ItemWeight);
            bindVarient(id).then(function () { stockLabel(); });
        } else $('txtItemWeight').value = '0';
        calcWeight();
    }
    /* bindvarientunit */
    function bindVarient(itemId) {
        var k = iv(cb.varient);
        return SE.api(API + '/varients' + SE.q({ itemId: itemId })).then(function (rows) {
            if (rows.length) {
                cb.varient.setData(rows);
                if (k > 0 && !cb.varient.setValue(k)) cb.varient.clear();
            } else { cb.varient.clear(); $('txtVarientEquivalent').value = ''; cb.varient.setData([]); }
        }).catch(fail);
    }
    /* CmbPackUom_TextChanged (wired to CmbAttributeVarient) + combitempck_Leave */
    function varientChanged() {
        var r = cb.varient.row();
        $('txtVarientEquivalent').value = r && iv(cb.varient) > 0 ? String(r.VarientEquivalent == null ? 0 : r.VarientEquivalent) : '0';
        calcWeight(); stockLabel();
    }
    /* CalculateWeight */
    function calcWeight() {
        if (iv(cb.varient) > 0 && $('txtqty').value !== '') $('txtNetWeight').value = String(dec($('txtVarientEquivalent').value) * dec($('txtqty').value) * dec($('txtItemWeight').value));
        else $('txtNetWeight').value = '0';
    }
    /* AvailableStockGetForLabel */
    function stockLabel() {
        var item = iv(cb.item);
        if (item <= 0) { $('lblBalance').textContent = '0'; return Promise.resolve(); }
        return SE.api(API + '/stock' + SE.q({ itemId: item, docDate: $('DocDate').value, warehouseId: iv(cb.wh), jobLotId: iv(cb.job), varientId: iv(cb.varient) })).then(function (r) {
            $('lblBalance').textContent = String(r.qty == null ? 0 : r.qty);
        }).catch(fail);
    }
    /* AvailableStockUpdateInGrid */
    function stockGrid() {
        var rows = G.grd.rows();
        if (!rows.length) return Promise.resolve();
        return SE.api(API + '/stock-lines', { method: 'POST', body: { docDate: $('DocDate').value, lines: rows.map(function (r) {
            return { itemId: +r.ItemId || 0, wareHouseId: +r.WareHouseId || 0, jobLotId: +r.JobLotId || 0, varientId: +r.AttributeVarientId || 0 }; }) } }).then(function (q) {
            rows.forEach(function (r, i) { r.AvailableQty = q[i] == null ? 0 : q[i]; });
            G.grd.refresh();
        }).catch(fail);
    }
    function stockRow(r) {                                       // grd_CellUpdated WareHouseId / JobLotId
        var item = +r.ItemId || 0, wh = +r.WareHouseId || 0;
        if (!(item > 0 || wh > 0)) { r.AvailableQty = 0; G.grd.refresh(); return; }
        SE.api(API + '/stock' + SE.q({ itemId: item, docDate: $('DocDate').value, warehouseId: wh, jobLotId: +r.JobLotId || 0, varientId: +r.AttributeVarientId || 0 })).then(function (q) {
            r.AvailableQty = q.qty == null ? 0 : q.qty; G.grd.refresh();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ grids */
    function nameIn(list, id, key) { for (var i = 0; i < list.length; i++) if (+list[i].Id === +id) return list[i][key]; return ''; }
    function makeGrids() {
        G.grd = SE.grid('grd', { footer: true, onDbl: function (r, i) { editRow(i); },
            onBtn: function (k, r, i) { if (k === 'Add') addCopy(i); else if (k === 'Delete') deleteRow(i); else if (k === 'Edit') editRow(i); },
            onEdit: function (r, k, v) {                                                   // grd_CellUpdated
                if (k === 'QTY') {
                    r.QTY = dec(v);
                    r.NetWeight = (r.QTY !== '' && r.VarientUnit !== '' && r.ItemWeight !== '') ? r.QTY * dec(r.VarientUnit) * dec(r.ItemWeight) : 0;
                } else if (k === 'WareHouseId') { r.WareHouseId = +v || 0; r.WareHouse = nameIn(S.wh, r.WareHouseId, 'WareHouseName'); stockRow(r); }
                else if (k === 'JobLotId') { r.JobLotId = +v || 0; r.JobLot = nameIn(S.job, r.JobLotId, 'JobLotDescription'); stockRow(r); }
                else if (k === 'CityId') { r.CityId = +v || 0; r.CityName = nameIn(S.city, r.CityId, 'CityName'); }
                else if (k === 'SupplierCustomerId') { r.SupplierCustomerId = +v || 0; r.SupplierCustomer = nameIn(S.cust, r.SupplierCustomerId, 'CompanyName'); }
                else r[k] = v;
            },
            cols: [
                { k: 'Delete', t: 'X', w: 28, btn: 'X' }, { k: 'Add', t: '+', w: 28, btn: '+' }, { k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' },
                { k: 'Id', hide: true },
                { k: 'SupplierCustomerId', t: 'Customer Name', w: 150, list: function () { return S.cust; }, lk: 'Id', lt: 'CompanyName' },
                { k: 'SupplierCustomer', hide: true }, { k: 'OrderId', hide: true }, { k: 'OrderDetailId', hide: true },
                { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'ReferencePartyId', hide: true },
                { k: 'ReferencePartyName', t: 'ReferencePartyName', w: 130, edit: true }, { k: 'ReferencePartyCellNo', t: 'ReferencePartyCellNo', w: 110, edit: true },
                { k: 'ReferencePartyAddress', t: 'ReferencePartyAddress', w: 160, edit: true },
                { k: 'BuildingStoreyId', hide: true }, { k: 'BuildingStorey', t: 'BuildingStorey', w: 90 }, { k: 'BuildingHeightId', hide: true }, { k: 'BuildingHeight', t: 'BuildingHeight', w: 90 },
                { k: 'BuildingArea', t: 'BuildingArea', w: 80 },
                { k: 'WareHouseId', t: 'WareHouseName', w: 150, edit: true, list: function () { return S.wh; }, lk: 'Id', lt: 'WareHouseName' }, { k: 'WareHouse', hide: true },
                { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'AttributeVarientId', hide: true },
                { k: 'AttributeVarient', t: 'AttributeVarient', w: 100 }, { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: d2, sum: true, cls: 'num' },
                { k: 'JobLotId', t: 'JobLotDescription', w: 150, edit: true, list: function () { return S.job; }, lk: 'Id', lt: 'JobLotDescription' }, { k: 'JobLot', hide: true },
                { k: 'QTY', t: 'QTY', w: 80, edit: true, render: d2, sum: true, cls: 'num' }, { k: 'AvailableQty', t: 'AvailableQty', w: 90, render: d2, sum: true, cls: 'num' },
                { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d2, sum: true, cls: 'num' }, { k: 'NetWeight', t: 'NetWeight', w: 90, render: d2, sum: true, cls: 'num' },
                { k: 'CityId', t: 'City Name', w: 150, edit: true, list: function () { return S.city; }, lk: 'Id', lt: 'CityName' }, { k: 'CityName', hide: true },
                { k: 'RemarksDetail', t: 'RemarksDetail', w: 160, edit: true },
                { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk', render: function (v) { return '<input type="checkbox" data-foc' + (v ? ' checked' : '') + '>'; } }] });
        $('grd').addEventListener('click', function (e) {                                  // grd_Click: IsFOC is a check-box column
            if (e.target && e.target.hasAttribute && e.target.hasAttribute('data-foc')) {
                var tr = e.target.closest('tr[data-i]'), r = tr && G.grd.rows()[+tr.getAttribute('data-i')];
                if (r) { r.IsFOC = e.target.checked; G.grd.refresh(); }
            }
        });
        G.focMain = SPC.focRows($('grd'), G.grd);

        G.hist = SE.grid('grdhistory', { frozen: 4, onDbl: function (r) { histOpen(r, false); },
            onBtn: function (k, r) { if (k === 'Edit') histOpen(r, true); else if (k === 'Print') print(+r.Id, false); else if (k === 'PrintII') print(+r.Id, true); },
            onLink: function (k, r) { if (k === 'NoOfAttachments') showAttachments(+r.Id); },
            onSel: histSelected,
            cols: [{ k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 50, btn: 'Print' }, { k: 'PrintII', t: 'PrintII', w: 55, btn: 'PrintII' },
                { k: 'NoOfAttachments', t: 'Attached', w: 70, link: true }, { k: 'Id', hide: true }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocumentTypeId', hide: true },
                { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' }, { k: 'DeliveryOrderTypeId', hide: true }, { k: 'RefrenenceNo', t: 'RefrenenceNo', w: 110 }, { k: 'Distance', t: 'Distance', w: 70 },
                { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'VehicleType', t: 'VehicleType', w: 110 }, { k: 'DoTotalQty', t: 'DoTotalQty', w: 90, render: d2, sum: true, cls: 'num' },
                { k: 'RemarksHeader', t: 'RemarksHeader', w: 180 }, { k: 'IsApproved', t: 'IsApproved', w: 80, f: 'chk' }, { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' },
                { k: 'EntryUserName', t: 'EntryUserName', w: 110 }, { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' }, { k: 'ModifyUserName', t: 'ModifyUserName', w: 110 },
                { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' }, { k: 'ApprovedUserName', t: 'ApprovedUserName', w: 110 }] });
        G.hd = SE.grid('GrdHistoryDetail', { cols: [{ k: 'Id', hide: true }, { k: 'SupplierCustomerId', hide: true }, { k: 'SupplierCustomer', t: 'SupplierCustomer', w: 170 },
            { k: 'OrderId', hide: true }, { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'ReferencePartyName', t: 'ReferencePartyName', w: 130 }, { k: 'ReferencePartyCellNo', t: 'ReferencePartyCellNo', w: 110 },
            { k: 'ReferencePartyAddress', t: 'ReferencePartyAddress', w: 160 }, { k: 'WareHouse', t: 'WareHouse', w: 130 }, { k: 'ItemId', hide: true }, { k: 'ItemCode', t: 'ItemCode', w: 90 },
            { k: 'ItemName', t: 'ItemName', w: 180 }, { k: 'AttributeVarient', t: 'AttributeVarient', w: 100 }, { k: 'VarientUnit', t: 'VarientUnit', w: 80, render: d2, sum: true, cls: 'num' },
            { k: 'JobLot', t: 'JobLot', w: 110 }, { k: 'QTY', t: 'QTY', w: 80, render: d2, sum: true, cls: 'num' }, { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d2, sum: true, cls: 'num' },
            { k: 'NetWeight', t: 'NetWeight', w: 90, render: d2, sum: true, cls: 'num' }, { k: 'CityName', t: 'CityName', w: 110 }, { k: 'RemarksDetail', t: 'RemarksDetail', w: 160 },
            { k: 'IsFOC', t: 'IsFOC', w: 50, f: 'chk' }] });
        G.focHd = SPC.focRows($('GrdHistoryDetail'), G.hd);
    }

    /* grdhistory_SelectionChanged: InvDeliveryOrder.ReadById, detail columns of the History grid */
    function histSelected(item) {
        if (!item) { G.hd.setRows([]); return; }
        SE.api(API + '/' + item.Id).then(function (d) {
            G.hd.setRows((d.lines || []).map(function (l) {
                return { Id: l.Id, SupplierCustomerId: l.SupplierCustomerId, SupplierCustomer: l.CustomerName, OrderId: l.SaleOrderId, OrderNo: l.OrderNo, ReferencePartyName: l.ReferencePartyName,
                    ReferencePartyCellNo: l.ReferencePartyCellNo, ReferencePartyAddress: l.ReferencePartyAddress, WareHouse: l.WareHouseName, ItemId: l.ItemId, ItemCode: l.ItemCode, ItemName: l.ItemName,
                    AttributeVarient: l.ItemAttributeVarient, VarientUnit: l.VarientEquivalent, JobLot: l.JobLotDescription, QTY: l.DoQty, ItemWeight: l.ItemWeight, NetWeight: l.DoNetWeight,
                    CityName: l.CityName, RemarksDetail: l.RemarksDetail, IsFOC: !!l.IsFOC };
            }));
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ FormValidationDetail / btnplus / ResetDetail */
    function detailValid() {
        function stop(m, f) { msg(m); if (f) f(); return false; }
        if (!cb.cust.row() || iv(cb.cust) === 0) return stop('Customer field is required', function () { cb.cust.focus(); });
        if (!cb.order.row() || iv(cb.order) === 0) return stop('OrderNo field is required', function () { cb.order.focus(); });
        if (textOf('CmbReferenceParty') === '') return stop('Ref Party Name Field is Required', function () { $('CmbReferenceParty').focus(); });
        if (textOf('txtRefPartyCellNo') === '') return stop('Ref Party CellNo Field is Required', function () { $('txtRefPartyCellNo').focus(); });
        if (textOf('txtRefPartyAddress') === '') return stop('Ref Party Address Field is Required', function () { $('txtRefPartyAddress').focus(); });
        if (!cb.wh.row() || iv(cb.wh) === 0) return stop('WareHouseName field is required', function () { cb.wh.focus(); });
        if (!cb.item.row() || iv(cb.item) === 0) return stop('ItemName field is required', function () { cb.item.focus(); });
        if (!cb.varient.row() || iv(cb.varient) === 0) return stop('AttributeVarient field is required', function () { cb.varient.focus(); });
        if (!cb.job.row() || iv(cb.job) === 0) return stop('JobLot field is required', function () { cb.job.focus(); });
        if (dec($('txtqty').value) === 0) return stop('ItemQty field is required', function () { $('txtqty').focus(); });
        if (dec($('txtItemWeight').value) === 0) return stop('Item Weight field is required', function () { $('txtItemWeight').focus(); });
        if (dec($('txtNetWeight').value) === 0) return stop('NetWeight field is required', function () { $('txtNetWeight').focus(); });
        if (!cb.city.row() || iv(cb.city) === 0) return stop('City field is required', function () { cb.city.focus(); });
        return true;
    }
    function refPartyId() { var t = textOf('CmbReferenceParty'); for (var i = 0; i < S.refs.length; i++) if (String(S.refs[i].ReferencePartyName) === t) return +S.refs[i].Id || 0; return 0; }
    function entryRow() {
        var ir = cb.item.row(), or = cb.order.row();
        return { SupplierCustomerId: iv(cb.cust), SupplierCustomer: cb.cust.text().trim(), OrderId: iv(cb.order), OrderDetailId: +ir.OrderDetailId || 0, OrderNo: SE.toInt(cb.order.text().trim()),
            ReferencePartyId: refPartyId(), ReferencePartyName: textOf('CmbReferenceParty'), ReferencePartyCellNo: textOf('txtRefPartyCellNo'), ReferencePartyAddress: textOf('txtRefPartyAddress'),
            BuildingStoreyId: iv(cb.storey), BuildingStorey: cb.storey.text(), BuildingHeightId: iv(cb.height), BuildingHeight: cb.height.text(), BuildingArea: $('txtArea').value,
            WareHouseId: iv(cb.wh), WareHouse: cb.wh.text(), ItemId: iv(cb.item), ItemCode: ir.ItemCode, ItemName: ir.ItemName, AttributeVarientId: iv(cb.varient),
            AttributeVarient: cb.varient.text().trim(), VarientUnit: dec($('txtVarientEquivalent').value), JobLotId: iv(cb.job), JobLot: cb.job.text(), QTY: dec($('txtqty').value),
            AvailableQty: dec($('txtqty').value), ItemWeight: dec($('txtItemWeight').value), NetWeight: dec($('txtNetWeight').value), CityId: iv(cb.city), CityName: cb.city.text(),
            RemarksDetail: $('txtremarksdetail').value.trim(), IsFOC: $('chkisFreeOfCost').checked };
    }
    function resetDetail() {
        $('chkisFreeOfCost').checked = false;
        cb.order.clear(); cb.wh.clear(); cb.item.clear(); cb.varient.clear(); $('txtVarientEquivalent').value = ''; cb.job.clear();
        $('txtqty').value = ''; $('txtItemWeight').value = ''; $('txtNetWeight').value = '0'; $('txtBalQty').value = '0'; $('txtBalWeight').value = '0'; $('txtremarksdetail').value = '';
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        cb.order.setData([]); cb.item.setData([]);
        keep(cb.cust, S.cust);
        cb.cust.focus();
    }
    function plus() {
        if (!detailValid()) return;
        G.grd.addRow(Object.assign({ Id: 0 }, entryRow()));
        resetDetail();
        getSaleOrderIds();
    }
    /* grd_DoubleClick */
    function editRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        S.updateIdx = i;
        cb.cust.setData([{ Id: r.SupplierCustomerId, CompanyName: nameIn(S.cust, r.SupplierCustomerId, 'CompanyName') || r.SupplierCustomer }]); cb.cust.setValue(r.SupplierCustomerId);
        cb.order.setData([{ OrderId: r.OrderId, OrderNo: String(r.OrderNo) }]); cb.order.setValue(r.OrderId);
        cb.storey.setValue(+r.BuildingStoreyId || 0); cb.height.setValue(+r.BuildingHeightId || 0); $('txtArea').value = r.BuildingArea || '';
        $('CmbReferenceParty').value = r.ReferencePartyName || ''; $('txtRefPartyCellNo').value = r.ReferencePartyCellNo || ''; $('txtRefPartyAddress').value = r.ReferencePartyAddress || '';
        cb.wh.setValue(+r.WareHouseId || 0);
        S.items = [{ ItemId: r.ItemId, ItemName: r.ItemName, ItemCode: r.ItemCode, ItemWeight: r.ItemWeight, OrderDetailId: r.OrderDetailId, DocumentTypeId: 1853 }];
        cb.item.setData(S.items.map(function (x) { return Object.assign({ _t: x.ItemName }, x); })); cb.item.setValue(+r.ItemId);
        bindVarient(+r.ItemId).then(function () { cb.varient.setValue(+r.AttributeVarientId || 0); $('txtVarientEquivalent').value = String(r.VarientUnit == null ? '' : r.VarientUnit); });
        cb.job.setValue(+r.JobLotId || 0);
        $('txtqty').value = String(r.QTY == null ? '' : r.QTY); $('txtItemWeight').value = String(r.ItemWeight == null ? '' : r.ItemWeight); $('txtNetWeight').value = String(r.NetWeight == null ? '' : r.NetWeight);
        cb.city.setValue(+r.CityId || 0); $('txtremarksdetail').value = r.RemarksDetail || ''; $('chkisFreeOfCost').checked = !!r.IsFOC;
        show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        $('CmbReferenceParty').focus();
    }
    function updateDetail() {
        if (!detailValid()) return;
        var r = G.grd.rows()[S.updateIdx]; if (!r) return;
        var n = entryRow();
        ['SupplierCustomerId', 'SupplierCustomer', 'OrderId', 'OrderNo', 'ReferencePartyName', 'ReferencePartyCellNo', 'ReferencePartyAddress', 'BuildingStoreyId', 'BuildingStorey', 'BuildingHeightId',
            'BuildingHeight', 'BuildingArea', 'WareHouseId', 'WareHouse', 'OrderDetailId', 'ItemId', 'ItemCode', 'ItemName', 'AttributeVarientId', 'AttributeVarient', 'VarientUnit', 'JobLotId', 'JobLot',
            'QTY', 'ItemWeight', 'NetWeight', 'CityId', 'CityName', 'RemarksDetail', 'IsFOC'].forEach(function (k) { r[k] = n[k]; });
        G.grd.refresh();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        resetDetail();
        keep(cb.cust, S.cust);
        getSaleOrderIds();
    }
    /* btnCancelUpdateDetial_Click */
    function cancelUpdateDetail() {
        $('txtRefDocTypeId').value = ''; $('txtDocNoId').value = ''; $('txtSubNoId').value = '';
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        cb.storey.clear(); cb.height.clear(); $('txtArea').value = '';
        keep(cb.cust, S.cust);
    }
    /* grd_ColumnButtonClick "Add" */
    function addCopy(i) {
        var r = G.grd.rows()[i]; if (!r) return;
        var c = JSON.parse(JSON.stringify(r)); c.Id = 0; delete c._chk;
        G.grd.addRow(c);
    }
    /* grd_ColumnButtonClick "Delete" */
    function deleteRow(i) {
        var r = G.grd.rows()[i]; if (!r) return;
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
    function lineOf(r) {
        return { id: +r.Id || 0, supplierCustomerId: +r.SupplierCustomerId || 0, orderId: +r.OrderId || 0, orderDetailId: +r.OrderDetailId || 0, referencePartyId: +r.ReferencePartyId || 0,
            referencePartyName: r.ReferencePartyName == null ? '' : String(r.ReferencePartyName), referencePartyCellNo: r.ReferencePartyCellNo == null ? '' : String(r.ReferencePartyCellNo),
            referencePartyAddress: r.ReferencePartyAddress == null ? '' : String(r.ReferencePartyAddress), buildingStoreyId: +r.BuildingStoreyId || 0, buildingHeightId: +r.BuildingHeightId || 0,
            buildingArea: r.BuildingArea == null ? '' : String(r.BuildingArea), wareHouseId: +r.WareHouseId || 0, itemId: +r.ItemId || 0, attributeVarientId: +r.AttributeVarientId || 0,
            varientUnit: dec(r.VarientUnit), jobLotId: +r.JobLotId || 0, qty: dec(r.QTY), itemWeight: dec(r.ItemWeight), netWeight: dec(r.NetWeight), cityId: +r.CityId || 0,
            remarksDetail: r.RemarksDetail == null ? '' : String(r.RemarksDetail), isFoc: !!r.IsFOC };
    }

    /* ------------------------------------------------------------------ Load / Reset / Refresh */
    function applyInitial(d) {
        S.rights = d.rights || {}; S.fyStart = d.fyStart || ''; S.dateTypes = d.dateTypes || [];
        $('txtdocno').value = d.nextNo > 0 ? String(d.nextNo) : $('txtdocno').value;
        cb.dateType.setData(S.dateTypes);
        histCombos(d.history || {});
    }
    function histCombos(h) {
        keep(cb.hCust, h.customers); keep(cb.hItem, h.items); keep(cb.hVeh, h.vehicles);
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') tabHistory(); else $('DocDate').focus(); });
        SE.upper($('CmbVehicleNo'));
        $('txtqty').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });   // txtqty_KeyPress
        $('txtqty').addEventListener('input', calcWeight);
        $('DocDate').addEventListener('change', function () { stockGrid(); stockLabel(); });
        $('rdbtnItemName').addEventListener('change', rdbChanged); $('rdbtnItemCode').addEventListener('change', rdbChanged);
        makeGrids();
        Promise.all([SE.api(API + '/initial'), SE.api(API + '/lists')]).then(function (a) {
            applyInitial(a[0]); fillLists(a[1]);
            var r = S.rights;
            $('btnsave').disabled = !r.save; $('btnupdate').disabled = !r.update; $('btnDelete').disabled = !r.delete;
            $('btnPrintSlip').disabled = !r.print; $('btnPrintCustomerWise').disabled = !r.print; $('ChkPrint').disabled = !r.print; $('chkPrintCustomerWise').disabled = !r.print;
            $('chkPrintCustomerWise').checked = !!r.print;
            show('btnupdate', false); show('btnDelete', false); show('btnsave', true); show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            $('DocDate').value = SE.today();
            $('txtFromdateHistory').value = SE.today(); $('txtToDateHistory').value = SE.today();
            $('DocDate').focus();
            var rec = SE.param('record'); if (rec) getUpdate(+rec, true);
        }).catch(fail);
        wire();
    }
    /* btnnew_Click -> Reset */
    function reset() {
        S.files = []; S.removedAtt = []; S.existing = []; S.remove = []; S.ordIds = ''; S.ordMainIds = ''; S.id = 0; S.approved = false;
        $('chkisFreeOfCost').checked = false; $('txtReferenceNoHeader').value = ''; $('CmbVehicleNo').value = ''; cb.vtype.clear();
        show('btnsave', true); show('btnupdate', false); show('btnDelete', false);
        cb.item.clear(); cb.varient.clear(); $('txtVarientEquivalent').value = ''; $('txtqty').value = ''; $('txtDistance').value = ''; $('txtItemWeight').value = ''; $('txtNetWeight').value = '0';
        $('txtBalQty').value = '0'; $('txtBalWeight').value = '0'; cb.city.clear(); $('txtRemarksHeader').value = '';
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        cb.storey.clear(); cb.height.clear(); $('txtArea').value = '';
        keep(cb.cust, S.cust);
        G.grd.setRows([]);
        $('DocDate').focus();
        return SE.api(API + '/next-no').then(function (r) { if (r.nextNo > 0) $('txtdocno').value = String(r.nextNo); }).catch(fail);
    }
    /* btnRefresh_Click */
    function refresh() { return SE.api(API + '/lists').then(fillLists).catch(fail); }
    function rdbChanged() {                                                              // rdbtnItemName_CheckedChanged
        if (!S.items.length) return;
        var id = iv(cb.item);
        bindItems(S.items);
        if (id > 0) { cb.item.setValue(id); cb.item.focus(); }
    }

    /* ------------------------------------------------------------------ Insert / btnsave / btnupdate */
    function formValid() {                                                               // FormValidation (checked on the server as well)
        function stop(m, f) { return SE.alert(m).then(function () { if (f) f(); return false; }); }
        var no = textOf('txtdocno');
        if (no === '' || no === '0') return stop('DocNo Field is Required', function () { $('txtdocno').focus(); });
        if (!cb.vtype.row()) return stop('VehicleType Field is Required', function () { cb.vtype.focus(); });
        var v = textOf('CmbVehicleNo');
        if (v === '' || v === '0') return stop('VehicleNo Field is Required', function () { $('CmbVehicleNo').focus(); });
        return Promise.resolve(true);
    }
    function insert(ignoreWarning) {
        if (G.grd.count() === 0) return SE.alert('Grid Record Not Found', 'Database Error');
        return formValid().then(function (ok) {
            if (!ok) return;
            var go = (S.id > 0 && !ignoreWarning) ? SE.ask('Are you sure to Update?', 'Confirm') : Promise.resolve(true);
            return go.then(function (yes) {
                if (!yes) return;
                var body = { id: S.id, docDate: $('DocDate').value, docNo: textOf('txtdocno'), vehicleTypeId: iv(cb.vtype), vehicleNo: textOf('CmbVehicleNo'),
                    referenceNo: $('txtReferenceNoHeader').value, distance: $('txtDistance').value, lines: G.grd.rows().map(lineOf), removed: S.remove, ignoreWarning: !!ignoreWarning,
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removedAtt } };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    if (r.warning) {                                                    // DeliveryOrderStackWarningMessage -> Yes / No
                        return SE.ask(r.warning, 'Confirm').then(function (y) { if (y) return insert(true); });
                    }
                    return SE.alert(r.message).then(function () {
                        var p1 = $('ChkPrint').checked, p2 = $('chkPrintCustomerWise').checked;
                        return reset().then(function () { if (p1) print(r.id, false); if (p2) print(r.id, true); });
                    });
                }).catch(function (e) { return SE.alert(e.message, 'Database Error'); });
            });
        });
    }
    function btnSave() { S.id = 0; return insert(false); }
    function btnUpdate() {
        if (S.id === 0) return SE.alert('Record Not Update because RecId Not Found', 'Database Error');
        return insert(false);
    }

    /* ------------------------------------------------------------------ getUpdate (ReadById) */
    function getUpdate(id, resetFirst) {
        var p = resetFirst ? reset() : Promise.resolve();
        return p.then(function () { S.id = id; return SE.api(API + '/' + id); }).then(function (d) {
            var h = d.head, lines = d.lines || [];
            tabs.select('tabPage1');
            $('DocDate').value = SE.dateInput(h.DocDate); $('txtdocno').value = h.DocNo;
            var rows = cb.vtype.rows(), hit = null;
            for (var i = 0; i < rows.length; i++) if (String(rows[i].VehicleDescription) === String(h.VehicleType)) hit = rows[i];
            if (hit) cb.vtype.setValue(hit.Id); else if (h.VehicleTypeId) cb.vtype.setValue(h.VehicleTypeId); else cb.vtype.clear();
            $('CmbVehicleNo').value = h.VehicleNo || ''; $('txtReferenceNoHeader').value = h.RefrenenceNo || ''; $('txtDistance').value = h.Distance || '';
            $('txtRemarksHeader').value = h.RemarksHeader || '';
            S.approved = !!h.IsApproved;
            G.grd.setRows(lines.map(function (l, ix) {
                return { Id: l.Id, SupplierCustomerId: l.SupplierCustomerId, SupplierCustomer: l.CustomerName, OrderId: l.SaleOrderId, OrderDetailId: l.SaleOrderDetailId, OrderNo: l.OrderNo,
                    ReferencePartyId: l.ReferencePartyId, ReferencePartyName: l.ReferencePartyName, ReferencePartyCellNo: l.ReferencePartyCellNo, ReferencePartyAddress: l.ReferencePartyAddress,
                    BuildingStoreyId: l.BuildingStoreyId, BuildingStorey: l.BuildingStorey, BuildingHeightId: l.BuildingHeightId, BuildingHeight: l.BuildingHeight, BuildingArea: l.BuildingArea,
                    WareHouseId: l.WarehouseId, WareHouse: l.WareHouseName, ItemId: l.ItemId, ItemCode: l.ItemCode, ItemName: l.ItemName, AttributeVarientId: l.ItemAttributeVarientId,
                    AttributeVarient: l.ItemAttributeVarient, VarientUnit: l.VarientEquivalent, JobLotId: l.JobLotId, JobLot: l.JobLotDescription, QTY: l.DoQty,
                    AvailableQty: (d.stock || [])[ix] == null ? 0 : d.stock[ix], ItemWeight: l.ItemWeight, NetWeight: l.DoNetWeight, CityId: l.CityId, CityName: l.CityName,
                    RemarksDetail: l.RemarksDetail, IsFOC: !!l.IsFOC };
            }));
            getSaleOrderIds();
            S.existing = d.attachments || []; S.files = []; S.removedAtt = [];
            show('btnsave', false); show('btnupdate', true); show('btnDelete', !!S.rights.delete);
        }).catch(fail);
    }
    function histOpen(r, resetFirst) { return getUpdate(+r.Id, resetFirst); }

    /* btnDelete_Click */
    function del() {
        if (S.id === 0) return SE.alert('Record Id Not Found', 'Message');
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            if (S.approved) return SE.alert('Record has been approved', 'Message');
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return SE.alert(r.message || 'Delete Record Seccessfully').then(reset); })
                .catch(function (e) { return SE.alert(e.message, 'Message'); });
        });
    }

    /* ------------------------------------------------------------------ history */
    function tabHistory() {
        if (!cb.dateType.rows().length) cb.dateType.setData(S.dateTypes);
        if (iv(cb.dateType) === 0 && cb.dateType.rows().length) cb.dateType.setValue(cb.dateType.rows()[0].Id);
        cb.dateType.focus();
        SE.api(API + '/history-combos').then(histCombos).catch(fail);                       // HistoryComboBind
    }
    function dateTypeChanged() { SPC.setRangeFromDateType(iv(cb.dateType), $('txtFromdateHistory'), $('txtToDateHistory'), S.fyStart); }
    function showHistory() {                                                              // FillHistory
        var q = { fromDate: $('txtFromdateHistory').value, toDate: $('txtToDateHistory').value, fromNo: SE.toInt($('txtFromNoHistory').value), toNo: SE.toInt($('txtToDocNoHistory').value),
            vehicleNo: iv(cb.hVeh) > 0 ? cb.hVeh.text() : '', customerId: iv(cb.hCust) };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            S.hist = rows;
            var seen = {}, main = [];
            rows.forEach(function (r) { if (seen[r.Id]) return; seen[r.Id] = 1; main.push(r); });
            G.hist.setRows(main);
            if (!main.length) G.hd.setRows([]);
        }).catch(fail);
    }
    function resetHistory() {                                                             // Resethistory (btnLoadAll_Click)
        cb.dateType.setValue(3); dateTypeChanged();
        $('txtFromNoHistory').value = ''; $('txtToDocNoHistory').value = '';
        cb.hCust.clear(); cb.hItem.clear(); cb.hVeh.clear();
        cb.dateType.focus();
    }

    /* ------------------------------------------------------------------ print / attachments */
    function print(id, customerWise) {                                                    // CommonServices.DeliveryOrderConcreteSlip / ...CustomerWise
        if (!(id > 0)) return SE.alert('No Record Found For Display', 'Message');
        return SPC.openPdf(PRINT + SE.q({ id: id, kind: customerWise ? 'customer' : '' }));
    }
    function showAttachments(id) {
        return SE.api(API + '/' + id + '/attachments').then(function (rows) { SPC.attachments(S, API, id, true, rows); }).catch(fail);
    }
    function openAttachments() { SPC.attachments(S, API, S.id, false); }

    /* ------------------------------------------------------------------ LoadSaleOrderForDeliveryOrderConcrete */
    var L = { cb: {}, grid: {}, rows: [], main: [], hdr: null, headerIds: '' };
    function $l(id) { return $('ld_' + id); }
    function money(v) { return SE.num(SE.toNum(v), 2, 0); }
    function openLoader() {
        $('ld_host').style.display = 'flex';
        if (!L.ready) initLoader();
        SE.api(API + '/loader/combos').then(function (d) {                                // StockComboFill
            keep(L.cb.sup, d.customers); keep(L.cb.item, d.items); keep(L.cb.cat, d.categories); keep(L.cb.type, d.itemTypes); keep(L.cb.par, d.parentCategories);
            if (!$l('FromDate').value) $l('FromDate').value = SE.dateInput(d.fromDate) || SE.today();
            if (!$l('Todate').value) $l('Todate').value = SE.today();
            return loaderLoad();
        }).catch(fail);
    }
    function closeLoader() { $('ld_host').style.display = 'none'; }
    function initLoader() {
        L.ready = true;
        function nm(id, cap) { return XCombo('ld_' + id, { columns: [{ key: 'name', caption: cap }], textKey: 'name', popupWidth: 360 }); }
        L.cb.sup = nm('CmbSupplier', 'PartyName'); L.cb.item = nm('cmbItem', 'Item'); L.cb.cat = nm('cmbCategory', 'Item Categories'); L.cb.type = nm('cmbItemtype', 'Item Type'); L.cb.par = nm('cmbParentCategory', 'Parent Category');
        L.grid.main = SE.grid('ld_grdMain', { frozen: 2, onCheck: function () { loaderPick(); }, onSel: function () { },
            cols: [{ k: 'Select', t: '', w: 30, sel: true }, { k: 'NoOfAttachments', t: 'Attached', w: 60 }, { k: 'Id', hide: true }, { k: 'DocNo', t: 'DocNo', w: 70 }, { k: 'DocDate', t: 'DocDate', w: 90, f: 'sdate' },
                { k: 'CustomerName', t: 'CustomerName', w: 200 }, { k: 'RefrenenceNo', t: 'RefrenenceNo', w: 100 }, { k: 'Distance', t: 'Distance', w: 70 }, { k: 'VisitedByName', t: 'VisitedByName', w: 110 },
                { k: 'ReferencePartyId', hide: true }, { k: 'ReferenceParty', t: 'ReferenceParty', w: 120 }, { k: 'RefPartyCellNo', t: 'RefPartyCellNo', w: 100 },
                { k: 'BuildingStoreyId', hide: true }, { k: 'BuildingStorey', t: 'BuildingStorey', w: 90 }, { k: 'BuildingHeightId', hide: true }, { k: 'BuildingHeight', t: 'BuildingHeight', w: 90 },
                { k: 'BuildingArea', t: 'BuildingArea', w: 80 }, { k: 'CommissionAgent', t: 'CommissionAgent', w: 110 }, { k: 'RefSalesMan', t: 'RefSalesMan', w: 100 },
                { k: 'CommAmount', t: 'CommAmount', w: 90, render: money, sum: true, cls: 'num' }, { k: 'PaymentTerm', t: 'PaymentTerm', w: 110 }, { k: 'OrderDueDays', t: 'OrderDueDays', w: 80 },
                { k: 'OrderDueDate', t: 'OrderDueDate', w: 90, f: 'sdate' }, { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 110 }, { k: 'DeliveryStartDate', t: 'DeliveryStartDate', w: 100, f: 'sdate' },
                { k: 'TransporterName', t: 'TransporterName', w: 110 }, { k: 'WagesAccount', t: 'WagesAccount', w: 110 }, { k: 'Status', t: 'Status', w: 80 }, { k: 'IsApproved', t: 'IsApproved', w: 80 },
                { k: 'EntryDate', t: 'EntryDate', w: 130, f: 'dt12' }, { k: 'EntryUserName', t: 'EntryUserName', w: 100 }, { k: 'ModifyDate', t: 'ModifyDate', w: 130, f: 'dt12' },
                { k: 'ModifyUserName', t: 'ModifyUserName', w: 100 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 130, f: 'dt12' }, { k: 'ApprovedUserName', t: 'ApprovedUserName', w: 100 },
                { k: 'RemarksHeader', t: 'RemarksHeader', w: 160 }] });
        L.grid.det = SE.grid('ld_grdDetail', { frozen: 1, cols: [{ k: 'Select', t: '', w: 40, sel: true }, { k: 'Id', hide: true }, { k: 'DetailId', hide: true },
            { k: 'ItemCode', t: 'ItemCode', w: 90 }, { k: 'ItemName', t: 'ItemName', w: 200 }, { k: 'ItemArrtibuteVarient', t: 'Varient', w: 100 }, { k: 'VarientEquivalent', t: 'VarientUnit', w: 80, render: d2, sum: true, cls: 'num' },
            { k: 'JobLotDescription', t: 'JobLot', w: 110 }, { k: 'ItemQty', t: 'ItemQty', w: 80, render: d2, sum: true, cls: 'num' }, { k: 'DispatchQty', t: 'DispatchQty', w: 85, render: d2, sum: true, cls: 'num' },
            { k: 'BalanceQty', t: 'BalanceQty', w: 85, render: d2, sum: true, cls: 'num' }, { k: 'ItemWeight', t: 'ItemWeight', w: 80, render: d2, sum: true, cls: 'num' },
            { k: 'NetWeight', t: 'NetWeight', w: 85, render: d2, sum: true, cls: 'num' }, { k: 'DispatchWeight', t: 'DispatchWeight', w: 95, render: function (v) { return SE.num(SE.toNum(v), 3, 0); }, sum: true, cls: 'num' },
            { k: 'BalanceWeight', t: 'BalanceWeight', w: 90, render: function (v) { return SE.num(SE.toNum(v), 3, 0); }, sum: true, cls: 'num' },
            { k: 'CityName', t: 'CityName', w: 100 }, { k: 'RemarksDetail', t: 'RemarksDetail', w: 160 }] });
        $('ld_close').onclick = closeLoader;
        $l('btngrnlod').onclick = function () { loaderLoad().catch(fail); };
        $l('btnReset').onclick = function () { $l('FromDate').focus(); L.cb.sup.clear(); $l('txtOrderNoFrom').value = ''; $l('txtOrderNoTo').value = ''; loaderLoad().catch(fail); };
        $l('btnRefresh').onclick = openLoader;
        $l('btnNew').onclick = function () { $l('btnReset').click(); };
        $l('btnLoadOnInvoice').onclick = loaderApply;
        $l('btnShortcutKeys').onclick = function () { SE.shortcuts([['Ctrl+S', 'For Show'], ['Ctrl+L', 'For Load'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+E', 'For Close'], ['Ctrl+F5', 'For Focus'],
            ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Main Grid']]); };
        SE.digitsOnly($l('txtOrderNoFrom')); SE.digitsOnly($l('txtOrderNoTo'));
    }
    /* PendingPurchaseOrderRegularForLoad */
    function loaderLoad() {
        var q = { fromDate: $l('FromDate').value, toDate: $l('Todate').value, fromNo: SE.toInt($l('txtOrderNoFrom').value), toNo: SE.toInt($l('txtOrderNoTo').value), customerId: iv(L.cb.sup),
            categoryId: iv(L.cb.cat), itemId: iv(L.cb.item), itemTypeId: iv(L.cb.type), parentCategoryId: iv(L.cb.par) };
        return SE.api(API + '/loader/orders' + SE.q(q)).then(function (rows) {
            L.rows = rows;
            var seen = {}, main = [];
            rows.forEach(function (r) {
                if (seen[r.Id]) return; seen[r.Id] = 1;
                main.push({ Id: r.Id, DocNo: r.DocNo, DocDate: r.DocDate, CustomerName: r.CustomerName, RefrenenceNo: r.RefrenenceNo, Distance: r.Distance, VisitedByName: r.VisitedByName,
                    ReferencePartyId: r.ReferencePartyId, ReferenceParty: r.ReferencPartyName, RefPartyCellNo: r.ReferencPartyCellNo, BuildingStoreyId: r.BuildingStoreyId, BuildingStorey: r.BuildingStorey,
                    BuildingHeightId: r.BuildingHeightId, BuildingHeight: r.BuildingHeight, BuildingArea: r.BuildingArea, CommissionAgent: r.CommissionAgent, RefSalesMan: r.RefSalesMan,
                    CommAmount: r.CommissionAmount, PaymentTerm: r.TermsDescription, OrderDueDays: r.OrderDueDays, OrderDueDate: r.OrderDueDate, DeliveryTerm: r.DeliveryTerm,
                    DeliveryStartDate: r.DeliveryStartDate, TransporterName: r.TransporterName, WagesAccount: r.OtherWagesAccount, Status: r.OrderStatus, IsApproved: r.IsApproved,
                    EntryDate: r.EntryDate, EntryUserName: r.EntryUserName, ModifyDate: r.ModifyDate, ModifyUserName: r.ModifyUserName, ApprovedDate: r.ApprovedDate,
                    ApprovedUserName: r.ApprovedUserName, RemarksHeader: r.RemarksHeader, NoOfAttachments: r.NoOfAttachments });
            });
            L.grid.main.setRows(main);
            loaderPick();
        });
    }
    /* grdMain Select -> GetCheckedRowsId + CalculateTotal + DetailGridBind + CheckedAllDetailRows */
    function loaderPick() {
        var checked = L.grid.main.checked(), ids = checked.map(function (r) { return +r.Id; });
        var T = { Order: ['txtOrderQtyHeader', 'txtQrderWeightHeader', 'txtOrderAmountHeader'], Dispatch: ['txtDispatchQtyHeader', 'txtDispatchWeightHeader', 'txtDispatchAmountHeader'],
            Balance: ['txtBalanceQtyHeader', 'txtBalanceWeightHeadrer', 'txtBalanceAmountHeader'] };
        var cur = L.grid.main.cur(), src = cur && L.rows.filter(function (r) { return +r.Id === +cur.Id; })[0];
        var v = function (n) { return src ? SE.num(SE.toNum(src[n]), 2, 0) : '0'; };
        $l(T.Order[0]).value = v('OrderQtyHeader'); $l(T.Order[1]).value = v('OrderWeightHeader'); $l(T.Order[2]).value = v('OrderAmountHeader');
        $l(T.Dispatch[0]).value = v('DispatchQtyHeader'); $l(T.Dispatch[1]).value = v('DispatchWeightHeader'); $l(T.Dispatch[2]).value = v('DispatchAmountHeader');
        $l(T.Balance[0]).value = v('BalanceQtyHeader'); $l(T.Balance[1]).value = v('BalanceWeightHeader'); $l(T.Balance[2]).value = v('BalanceAmountHeader');
        if (!ids.length) { L.grid.det.setRows([]); return; }
        L.grid.det.setRows(L.rows.filter(function (r) { return ids.indexOf(+r.Id) >= 0; }).map(function (r) { return Object.assign({}, r); }));
        L.grid.det.check(function () { return true; });
    }
    /* btnLoadOnInvoice_Click_1 + LoadDataDetailfromOrder */
    function loaderApply() {
        var picked = L.grid.det.checked();
        if (!picked.length) { SE.alert('Check the Row first'); return; }
        closeLoader();
        var first = picked[0];
        $('txtReferenceNoHeader').value = first.RefrenenceNo == null ? '' : String(first.RefrenenceNo);
        $('txtDistance').value = first.Distance == null ? '' : String(first.Distance);
        var flag = false;
        picked.forEach(function (p) {
            G.grd.rows().forEach(function (g) { if (!flag && (+g.OrderDetailId || 0) === (+p.DetailId || 0)) flag = true; });
            /* desktop quirk kept: once a duplicate is found `flag` is never reset, so every later loader row is skipped as well */
            if (!flag) {
                G.grd.rows().push({ Id: 0, SupplierCustomerId: p.SupplierCustomerId, SupplierCustomer: String(p.SupplierCustomerId), OrderId: p.Id, OrderDetailId: p.DetailId, OrderNo: SE.toInt(p.DocNo),
                    ReferencePartyId: first.ReferencePartyId, ReferencePartyName: first.ReferencPartyName, ReferencePartyCellNo: first.ReferencPartyCellNo, ReferencePartyAddress: first.ReferencPartyAddress,
                    BuildingStoreyId: SE.toInt(first.BuildingStoreyId), BuildingStorey: first.BuildingStorey || '', BuildingHeightId: SE.toInt(first.BuildingHeightId), BuildingHeight: first.BuildingHeight || '',
                    BuildingArea: first.BuildingArea || '', WareHouseId: 0, WareHouse: '', ItemId: p.OrderItemId, ItemCode: p.ItemCode, ItemName: p.ItemName, AttributeVarientId: p.ItemAttributeVarientId,
                    AttributeVarient: p.ItemArrtibuteVarient, VarientUnit: p.VarientEquivalent, JobLotId: p.JobLotId, JobLot: p.JobLotDescription, QTY: p.BalanceQty, AvailableQty: p.BalanceQty,
                    ItemWeight: p.ItemWeight, NetWeight: p.BalanceWeight, CityId: p.CityId, CityName: p.CityName, RemarksDetail: p.RemarksDetail, IsFOC: false });
            }
        });
        G.grd.refresh(); getSaleOrderIds();
        stockGrid();                                                                      // AvailableStockUpdateInGrid
    }

    /* ------------------------------------------------------------------ shortcut keys (PurchsaeOrder_KeyDown / MakeShortCutKeys) */
    var SHORT = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+Shift+Delete', 'For Delete'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Ctrl+P', 'For Print Slip 1853'], ['Alt+1', 'For Print Slip 1853'], ['Ctrl+F5', 'For Focus on Customer Name'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+D', 'For Adding an row in Focused Grid'], ['Ctrl+Delete', 'For Deleting an row of Focused Grid'],
        ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On Customer Name in Detail Box'], ['Ctrl+ArrowRight', 'For Change Focus from one Grid To another Grid'],
        ["Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function onKey(e) {
        if ($('ld_host').style.display !== 'none') {                                     // LoadPurchaseOrder_KeyDown
            var k0 = e.key.toLowerCase();
            if (e.key === 'Escape' || (e.ctrlKey && k0 === 'e')) { e.preventDefault(); closeLoader(); }
            else if (e.ctrlKey && k0 === 's') { e.preventDefault(); $l('btngrnlod').click(); }
            else if (e.ctrlKey && k0 === 'l') { e.preventDefault(); $l('btnLoadOnInvoice').click(); }
            else if (e.ctrlKey && k0 === 'n') { e.preventDefault(); $l('btnReset').click(); }
            else if (e.ctrlKey && k0 === 'r') { e.preventDefault(); openLoader(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $l('FromDate').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('ld_grdDetail').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { $('ld_grdMain').focus(); }
            return;
        }
        var t = e.target, k = e.key.toLowerCase();
        if (e.key === 'Enter' && t && (t.tagName === 'INPUT' || t.tagName === 'SELECT') && t.type !== 'checkbox' && t.type !== 'radio' && t.type !== 'button') {
            var f = Array.prototype.filter.call(document.querySelectorAll('.dform input:not([type=hidden]):not([disabled]), .dform select:not([disabled]), .dform .dtcombo-input'), function (x) { return x.offsetParent !== null; });
            var i = f.indexOf(t); if (i >= 0 && f[i + 1]) { e.preventDefault(); f[i + 1].focus(); }
        }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tabs.index() === 1) { tabs.select('tabPage1'); $('DocDate').focus(); } else { tabs.select('tabPage2'); tabHistory(); $('grdhistory').focus(); } return; }
        if (tabs.index() === 0) {
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh(); }
            else if (e.ctrlKey && e.shiftKey && e.key === 'Delete') { e.preventDefault(); if (isShown('btnDelete') && !$('btnDelete').disabled) del(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); if (isShown('btnsave') && !$('btnsave').disabled) btnSave(); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (isShown('btnupdate') && !$('btnupdate').disabled) btnUpdate(); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('DocDate').focus(); }
            else if (e.ctrlKey && e.key === 'F10') { e.preventDefault(); openAttachments(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); print(S.id, false); }
            else if (e.altKey && (e.key === '1')) { e.preventDefault(); print(S.id, false); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { cb.cust.focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') { if (document.activeElement === $('grd')) cb.cust.focus(); else $('grd').focus(); }
            else if (e.ctrlKey && e.key === 'Delete') { var gi = G.grd.curIndex(); if (gi >= 0) deleteRow(gi); }
            else if (e.ctrlKey && k === 'd') { e.preventDefault(); var ci = G.grd.curIndex(); if (ci >= 0) addCopy(ci); }
        } else {
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); $('grdhistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { $('GrdHistoryDetail').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowUp') { $('grdhistory').focus(); }
            else if (e.ctrlKey && e.key === 'ArrowRight') { if (document.activeElement === $('GrdHistoryDetail')) $('grdhistory').focus(); else $('GrdHistoryDetail').focus(); }
            else if (e.ctrlKey && e.key === 'Enter' && G.hist.cur() && S.rights.update) { e.preventDefault(); histOpen(G.hist.cur(), true); }
            else if (e.ctrlKey && k === 'p' && G.hist.cur() && S.rights.print) { e.preventDefault(); print(+G.hist.cur().Id, false); }
        }
    }
    function wire() {
        $('btnnew').onclick = reset; $('btnRefresh').onclick = refresh; $('btnsave').onclick = btnSave; $('btnupdate').onclick = btnUpdate; $('btnDelete').onclick = del;
        $('btnattachment').onclick = openAttachments; $('btnPrintSlip').onclick = function () { print(S.id, false); }; $('btnPrintCustomerWise').onclick = function () { print(S.id, true); };
        $('btnLoadSaleOrder').onclick = openLoader;
        $('btnDefineReferenceParties').onclick = function () { window.open('/party-processing/reference-parties', '_blank'); };
        $('BtnDefineCity').onclick = function () { window.open('/master-data/city', '_blank'); };
        $('btnshortcutkeys').onclick = function () { SE.shortcuts(SHORT); };
        $('btnplus').onclick = plus; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelUpdateDetial').onclick = cancelUpdateDetail;
        $('btnNewHistory').onclick = resetHistory; $('BtnRefreshHistory').onclick = function () { SE.api(API + '/history-combos').then(histCombos).catch(fail); }; $('btnShowHistory').onclick = showHistory;
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
        var hook = false;
        document.addEventListener('keydown', function (e) { if (e.ctrlKey && e.altKey && !hook) { hook = true; SE.shortcuts(SHORT); setTimeout(function () { hook = false; }, 400); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
