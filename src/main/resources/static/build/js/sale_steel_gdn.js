/*
 * Screen 540  InvFrmGDN_St  (Architecture.WinApp.Steel.Sale.InvFrmGDN_St, Goods Dispatch Notes, document type 1508)
 * Page script. Desktop methods are named in the comments (InvFrmGDN_St.cs). Server: /sale/steel/gdn/api
 */
(function () {
    'use strict';
    var $ = SE.$, API = '/sale/steel/gdn/api', fail = SS.fail;
    var S = { id: 0, rights: {}, orderType: '', orderStatus: '', deliveryOrderType: '', gpType: '', gpId: 0, soOrderId: 0, updateIdx: -1, remove: [], files: [], removed: [], existing: [],
              dso: [], customers: [], items: [], jobLots: [], packs: [], whs: [], cities: [], defaults: {}, wages: false, histSeq: 0, doLayout: false, historyDays: 3 };
    var cb = {}, G = {}, tabs;

    function field(id) { return ($(id).value || '').trim(); }
    function tn(v) { return SE.toNum(v); }
    function ti(v) { var n = Number(String(v == null ? '' : v).replace(/,/g, '')); return isFinite(n) ? Math.trunc(n) : 0; }
    function msg(t, cap) { return SE.alert(t, cap); }
    function show(id, on) { var b = $(id); if (b) b.style.display = on ? '' : 'none'; }
    function visible(id) { return SS.visible(id); }
    function onBlur(c, fn) { var x = c.el.__dtcombo; if (x && x.input) x.input.addEventListener('blur', fn); }
    function f2(v) { return SE.num(v, 2, 0); }                                          // "#,##0.##"
    function dbl(s) { var t = String(s == null ? '' : s).trim(); var n = Number(t.replace(/,/g, '')); if (t === '' || !isFinite(n)) throw new Error('Input string was not in a correct format.'); return n; }     // double.Parse
    function toDouble(s) { var n = Number(String(s == null ? '' : s).trim().replace(/,/g, '')); return isFinite(n) ? n : 0; }                                                                                  // Conversion.ToDouble
    function r3(v) { return Math.round(v * 1000) / 1000; }                                                                                                                                                      // ToString("#,##0.###")
    function rowsOf(c, key) { return c.rows(); }
    function first(c, key) { var r = c.rows()[0]; if (r) c.setValue(r[key || 'Id']); }                                                                                                                         // Rows[0].Activate()
    /* TextBox.Text = value: raises TextChanged only when the text really changes */
    function setT(id, v) { var e = $(id), s = v == null ? '' : String(v); if (e.value !== s) { e.value = s; e.dispatchEvent(new Event('input')); } }
    function guard(fn, cap) { try { return fn(); } catch (e) { return msg(e.message, cap || 'Error Message'); } }
    function setCombo(c, id, key) { var k = key || 'Id', hit = c.rows().some(function (r) { return String(r[k]) === String(id); }); if (hit) c.setValue(id); else c.clear(); }

    function saveMode() { return visible('btnSave'); }                                    // btnSave.Visible && btnSave.Enabled

    /* ------------------------------------------------------------------ combos */
    function makeCombos() {
        cb.order = XCombo('CmbOrderNo', { columns: [{ key: 'OrderNo', caption: 'Order No' }], textKey: 'OrderNo', onSelect: function () { orderLeave(); } });
        cb.item = XCombo('CmbItemName', { columns: [{ key: 'ItemName', caption: 'Item Name' }], valueKey: 'OrderItemId', textKey: 'ItemName', popupWidth: 520, onSelect: function () { itemLeave(); } });
        cb.job = XCombo('CmbJobLot', { columns: [{ key: 'JobLotDescription', caption: 'JobLot Description' }], textKey: 'JobLotDescription', onSelect: availableStock });
        cb.pack = XCombo('CmbPackingType', { columns: [{ key: 'PackTypeDesc', caption: 'Description' }], textKey: 'PackTypeDesc' });
        cb.uom = XCombo('CmbPackUom', { columns: [{ key: 'UOMCode', caption: 'PackUom' }], textKey: 'UOMCode', onSelect: function () { guard(total); } });
        cb.wh = XCombo('CmbWarehouse', { columns: [{ key: 'WareHouseName', caption: 'Godown Name' }], textKey: 'WareHouseName', onSelect: availableStock });
        cb.city = XCombo('CmbCity', { columns: [{ key: 'CityName', caption: 'City Name' }], textKey: 'CityName' });
        cb.cust = XCombo('CmbSupplier', { columns: [{ key: 'CompanyName', caption: 'Supplier Name' }], textKey: 'CompanyName', popupWidth: 420 });
        cb.trans = XCombo('CmbTransporter', { columns: [{ key: 'AccountTitle', caption: 'Transporter Name' }], textKey: 'AccountTitle', popupWidth: 380 });
        cb.veh = XCombo('CmbVehicleType', { columns: [{ key: 'VehicleDescription', caption: 'Vehicle Description' }], textKey: 'VehicleDescription' });
        cb.ref = XCombo('CmbReferanceParty', { columns: [{ key: 'ReferencePartyName', caption: 'ReferencePartyName' }], textKey: 'ReferencePartyName', popupWidth: 380 });
        cb.hcust = XCombo('CmbCustomerHistory', { columns: [{ key: 'Customer', caption: 'Party' }], textKey: 'Customer', popupWidth: 380 });
        onBlur(cb.order, orderLeave); onBlur(cb.item, itemLeave); onBlur(cb.uom, function () { guard(total); });
        onBlur(cb.wh, availableStock); onBlur(cb.job, availableStock);                  // combwr_Leave / combjoblot_Leave
        /* CmbLabNo is a DropDown-style ComboBox: its text can be typed (no items) */
        var sel = $('CmbLabNo'), inp = document.createElement('input');
        inp.className = 'f'; inp.id = 'CmbLabNo'; inp.setAttribute('style', sel.getAttribute('style') || ''); inp.value = '';
        sel.parentNode.replaceChild(inp, sel);
    }

    /* ------------------------------------------------------------------ layout: panel5 Dock = None / Top */
    function deliveryLayout(on) {
        S.doLayout = on;
        show('panel5', !on);
        $('panel10').style.top = (on ? 203 : 333) + 'px';
        $('panel6').style.top = (on ? 391 : 521) + 'px';
        $('panel6').style.height = (on ? 340 : 210) + 'px';
        $('panel12').style.height = (on ? 314 : 184) + 'px';
        $('grdGp').style.height = (on ? 314 : 184) + 'px';
        if (G.gp) G.gp.refresh();
    }

    /* ------------------------------------------------------------------ calculations (Total :1600, TotalWeight :1657, TotalSupplierWeight :1684) */
    function total() {
        var qty = 0, gross = 0, cut = 0, cutTotal = 0, addLess = 0;
        if ($('txtQty').value !== '') qty = dbl($('txtQty').value);
        if ($('txtGrossWeight').value !== '') gross = dbl($('txtGrossWeight').value);
        if ($('txtNetWeight').value !== '') dbl($('txtNetWeight').value);
        if ($('txtStockWeight').value !== '') dbl($('txtStockWeight').value);
        if ($('txtWtCut').value !== '' && $('txtQty').value !== '') { cut = dbl($('txtWtCut').value); $('txtWtCuTtotal').value = String(qty * cut); }
        else { cut = 0; $('txtWtCuTtotal').value = '0'; }
        if ($('txtWtCuTtotal').value !== '') cutTotal = dbl($('txtWtCuTtotal').value);
        if ($('txtAddLessWt').value !== '') addLess = toDouble($('txtAddLessWt').value);
        var w = gross - cutTotal + addLess;
        $('txtNetWeight').value = String(w); $('txtStockWeight').value = String(w);
    }
    function totalWeight() {
        var pu = 0, qty = 0;
        if (cb.uom.text() !== '') {
            var um = cb.uom.row();
            if (!um) throw new Error('Object reference not set to an instance of an object.');
            pu = dbl(um.Equivalent);
        }
        if ($('txtQty').value !== '') qty = dbl($('txtQty').value);
        if ($('txtGrossWeight').value !== '') dbl($('txtGrossWeight').value);
        setT('txtGrossWeight', String(pu * qty));
    }
    function totalSupplierWeight() {
        if ($('txtsuppwt').value !== '' && $('txtFactoryWeight').value !== '') $('txtwtdiff').value = String(dbl($('txtsuppwt').value) - dbl($('txtFactoryWeight').value));
    }

    /* AvailableStock :1522 */
    function availableStock() {
        return SE.api(API + '/stock' + SE.q({ itemId: +cb.item.value() || 0, docDate: $('DocDate').value, jobLotId: +cb.job.value() || 0, warehouseId: +cb.wh.value() || 0 })).then(function (d) {
            $('lblStock').textContent = d.stock > 0 ? String(d.stock) : '0';
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ order / item combos */
    function itemRows(rows) { return rows.map(function (r) { return { OrderItemId: r.OrderItemId, ItemName: r.ItemName, OrderItemRate: r.OrderItemRate, RateUOM: r.RateUOM, SoDetailId: r.Id }; }); }
    function itemFillWithoutOrder() {                                                    // ItemNameFillWithoutOrder :1119
        if (S.items.length > 0) cb.item.setData(S.items.map(function (r) { return { OrderItemId: r.Id, ItemName: r.ItemName, SoDetailId: 0 }; }));
    }
    function itemFill() {                                                                // ItemNameFill :1089
        var rows = itemRows(S.dso);
        if (rows.length > 0) cb.item.setData(rows); else cb.item.setData([]);
    }
    /* comborderno_Leave :1558 */
    function orderLeave() {
        var oid = +cb.order.value() || 0;
        if (cb.order.row() && oid !== 0) {
            return SE.api(API + '/order-items' + SE.q({ orderId: oid })).then(function (rows) {
                S.dso = rows;
                if (rows.length > 0) {
                    $('txtDeliverTerm').value = rows[0].DeliveryTerm == null ? '' : String(rows[0].DeliveryTerm);
                    cb.cust.setData(rows.map(function (r) { return { Id: r.OrderSupCustId, CompanyName: r.CompanyName }; }));
                    first(cb.cust);
                    itemFill();
                }
            }).catch(fail);
        }
        S.dso = [];
        itemFillWithoutOrder();
        return Promise.resolve();
    }
    /* PackUOMFillWithoutOrder :1186 */
    function packUomFill() {
        return SE.api(API + '/uoms' + SE.q({ itemId: +cb.item.value() || 0 })).then(function (rows) {
            if (rows.length > 0) cb.uom.setData(rows);
            if (saveMode()) cb.uom.clear();
        }).catch(fail);
    }
    /* the remaining gross weight prefill of combitem_Leave */
    function grossPrefill(ponchAware) {
        if (G.grd.rows().length === 0) {
            if (ponchAware && $('txtDeliverTerm').value === 'Ponch') setT('txtGrossWeight', String(dbl($('txtFactoryWeight').value)));
            else setT('txtGrossWeight', $('txtFactoryWeight').value.trim());
        } else if (S.updateIdx < 0) {
            var sum = G.grd.rows().reduce(function (a, r) { return a + tn(r.GrossWight); }, 0);
            var g = toDouble($('txtFactoryWeight').value) - sum;
            setT('txtGrossWeight', g === 0 ? '0' : String(g));
        }
    }
    /* combitem_Leave :1442 */
    function itemLeave() {
        return availableStock().then(function () {
            if (cb.order.row()) {
                return packUomFill().then(function () {
                    grossPrefill(true);
                    if (!saveMode() || S.orderType !== 'SaleOrder') return;
                    var dso = S.dso, pick;
                    if (dso.length === 1) pick = dso[0];
                    else {
                        var iid = +cb.item.value() || 0, hit = dso.filter(function (r) { return ti(r.OrderItemId) === iid; });
                        pick = hit[0];
                        if (!pick) throw new Error('There is no row at position 0.');
                    }
                    setCombo(cb.uom, pick.OrderItemUOMId);
                    SS.byText(cb.job, 'JobLotDescription', pick.JobLotDescription == null ? '' : pick.JobLotDescription);
                    setCombo(cb.city, pick.CityId);
                });
            }
            $('txtGrossWeight').disabled = false;
            grossPrefill(false);
            return packUomFill().then(function () { cb.job.setData(S.jobLots); });
        }).catch(function (e) { return msg(e.message); });
    }

    /* ------------------------------------------------------------------ grids */
    function numCol(k, w) { return { k: k, t: k, w: w, cls: 'num', sum: true, f: 'n2', render: function (v) { return v == null || v === '' ? '' : f2(v); } }; }
    function gdSpec(doMode) {
        function c(k, t, w, o) { var x = { k: k, t: t, w: w }; if (o) for (var p in o) x[p] = o[p]; return x; }
        var cols = [
            c('Id', 'Id', 40, { hide: true }), c('SaleOrderDetailId', 'SaleOrderDetailId', 60, { hide: true }), c('SaleOrderId', 'SaleOrderId', 60, { hide: true }),
            c('OrderNo', 'OrderNo', 70), c('RefPartyId', 'RefPartyId', 60, { hide: true }), c('ItemId', 'ItemId', 60, { hide: true }), c('ItemName', 'ItemName', 260),
            c('JobLotId', 'Job Lot', 120, doMode ? { edit: true, list: function () { return S.jobLots; }, lk: 'Id', lt: 'JobLotDescription' } : { hide: true }),
            c('JobLot', 'JobLot', 110, doMode ? { hide: true } : null),
            c('PackingTypeId', 'Packing Type', 110, doMode ? { edit: true, list: function () { return S.packs; }, lk: 'Id', lt: 'PackTypeDesc' } : { hide: true }),
            c('PackingType', 'PackingType', 110, doMode ? { hide: true } : null),
            c('PackUomId', 'PackUomId', 60, { hide: true }), c('PackUom', 'PackUom', 80),
            numCol('ItemQty', 80), numCol('GrossWight', 100), numCol('WtCut', 70), numCol('WtCutTotal', 80), numCol('AddLesswt', 80), numCol('NetWeight', 100), numCol('StockWeight', 100),
            c('ItemRate', 'ItemRate', 60, { hide: true }),
            c('WareHouse', 'WareHouseName', 140, { edit: true, list: function () { return S.whs; }, lk: 'Id', lt: 'WareHouseName' }),
            c('LabNo', 'LabNo', 90, doMode ? { hide: true } : null),
            c('City', 'City Name', 110, { edit: true, list: function () { return S.cities; }, lk: 'Id', lt: 'CityName' }),
            c('Remarks', 'Remarks', 200, { edit: true }),
            c('Delete', 'X', 28, { btn: 'X' }), c('Add', '+', 28, { btn: '+' })];
        if (doMode) ['SaleOrderDetailId', 'SaleOrderId', 'ItemId', 'PackUomId', 'WtCut', 'WtCutTotal'].forEach(function (k) { cols.forEach(function (x) { if (x.k === k) x.hide = true; }); });
        var editable = doMode ? ['ItemQty', 'GrossWight', 'AddLesswt', 'Remarks', 'WareHouse', 'City', 'JobLotId', 'PackingTypeId'] : ['WareHouse', 'City', 'Remarks'];
        cols.forEach(function (x) { x.edit = editable.indexOf(x.k) >= 0; });
        return { footer: true, onDbl: function (r, i) { guard(function () { editRow(i); }); }, onBtn: function (k, r, i) { if (k === 'Delete') deleteRow(i); else addCopy(i); }, onEdit: gridEdit, cols: cols };
    }
    function makeGrid(doMode) {
        var old = $('grd'), fresh = old.cloneNode(false);                               // a new element: the old grid's listeners go with the old one
        old.parentNode.replaceChild(fresh, old);
        G.grd = SE.grid(fresh, gdSpec(doMode));
        G.mode = doMode;
        footFix(G.grd);
    }
    /* total-row format "#,##0.##" of the shared grid */
    function footFix(g) {
        var fixing = false;
        function fix() {
            if (fixing) return; fixing = true;
            try {
                var vc = g.spec.cols.filter(function (c) { return !c.hide; }), tds = g.el.querySelectorAll('tfoot td');
                vc.forEach(function (c, i) { if (c.sum && tds[i] && c.f === 'n2') tds[i].textContent = f2(g.rows().reduce(function (a, r) { return a + tn(r[c.k]); }, 0)); });
            } finally { fixing = false; }
        }
        new MutationObserver(fix).observe(g.el, { childList: true });
        g.fix = fix; fix();
    }
    /* grdSettings :1791 - the columns follow OrderStatus */
    function grdSettings() {
        var doMode = S.orderStatus === 'DeliveryOrder', rows = G.grd.rows();
        if (G.mode !== doMode) { makeGrid(doMode); }
        G.grd.setRows(rows);
    }
    function makeOtherGrids() {
        G.gp = SE.grid('grdGp', { frozen: 1, footer: true, onBtn: function (k, r) { if (k === 'Load') guard(function () { loadGatePass(r); }); },
            cols: [{ k: 'Load', t: 'Load', w: 40, btn: 'Load' },
                { k: 'OutwardGatePassId', hide: true }, { k: 'GatepassType', hide: true }, { k: 'SupplierContractCode', hide: true },
                { k: 'OrderType', t: 'OrderType', w: 100 }, { k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'GpSrNo', t: 'GpSrNo', w: 70 }, { k: 'GpDate', t: 'GpDate', w: 85, f: 'sdate' },
                { k: 'VehicleType', t: 'VehicleType', w: 110 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 }, { k: 'BiltyNo', t: 'BiltyNo', w: 90 }, { k: 'CustomerName', t: 'CustomerName', w: 220 },
                { k: 'VarietyName', t: 'VarietyName', w: 150 }, numCol('NoOfPackages', 100), numCol('SupplierWeight', 110), numCol('FactoryWeight', 110), numCol('DifferenceWeight', 110),
                { k: 'Status', t: 'Status', w: 90 }] });
        footFix(G.gp);
        G.hist = SE.grid('DataGridHistory', { frozen: 3, footer: true, onDbl: function (r) { guard(function () { histOpen(r); }); }, onSel: histSelected,
            onBtn: function (k, r) { guard(function () { histButton(k, r); }); }, onLink: function (k, r) { histLink(k, r); },
            cols: [{ k: 'Edit', t: 'Edit', w: 50, btn: 'Edit' }, { k: 'Print', t: 'Print', w: 50, btn: 'Print' }, { k: 'Print1', t: 'Print1', w: 80, btn: '1515A-Print' },
                { k: 'Id', hide: true }, { k: 'DocNo', t: 'DocNo', w: 60 }, { k: 'DocDate', t: 'DocDate', w: 85, f: 'sdate' }, { k: 'CustomerName', t: 'CustomerName', w: 200 },
                { k: 'DeliveryTerm', t: 'DeliveryTerm', w: 100 }, { k: 'Transporter', t: 'Transporter', w: 150 }, { k: 'Freight', t: 'Freight', w: 90, cls: 'num', sum: true, f: 'n2', render: function (v) { return v == null || v === '' ? '' : SE.num(v, 0, 0); } },
                { k: 'GpId', hide: true }, { k: 'GpNo', t: 'GpNo', w: 70, link: true }, { k: 'VehicleType', t: 'VehicleType', w: 110 }, { k: 'VehicleNo', t: 'VehicleNo', w: 90 },
                { k: 'BiltyNo', t: 'BiltyNo', w: 90 }, { k: 'RefrenceParty', t: 'RefrenceParty', w: 150 }, { k: 'TotalQty', t: 'TotalQty', w: 80, cls: 'num', render: function (v) { return v == null ? '' : String(v); } },
                { k: 'FactoryWeight', t: 'FactoryWeight', w: 100, cls: 'num', render: function (v) { return v == null ? '' : String(v); } },
                { k: 'PartyWeight', t: 'PartyWeight', w: 100, cls: 'num', render: function (v) { return v == null ? '' : String(v); } },
                { k: 'EntryUser', t: 'EntryUser', w: 110 }, { k: 'EntryDate', t: 'EntryDate', w: 85, f: 'sdate' }, { k: 'ModifyUser', t: 'ModifyUser', w: 110 }, { k: 'ModifyDate', t: 'ModifyDate', w: 85, f: 'sdate' },
                { k: 'IsApproved', t: 'IsApproved', w: 100 }, { k: 'ApprovedUser', t: 'ApprovedUser', w: 110 }, { k: 'ApprovedDate', t: 'ApprovedDate', w: 85, f: 'sdate' },
                { k: 'NoOfAttachments', t: 'NoOfAttachments', w: 110, link: true }, { k: 'Remarks', t: 'Remarks', w: 200 }] });
        footFix(G.hist);
        G.hd = SE.grid('DataGridHistoryDetail', { footer: true,
            cols: [{ k: 'OrderNo', t: 'OrderNo', w: 70 }, { k: 'ItemName', t: 'ItemName', w: 260 }, { k: 'JobLot', t: 'JobLot', w: 110 }, { k: 'PackingType', t: 'PackingType', w: 110 }, { k: 'PackUom', t: 'PackUom', w: 80 },
                numCol('ItemQty', 80), numCol('GrossWeight', 100), numCol('WeightCut', 90), numCol('WeightCutTotal', 100), numCol('AdLsWeight', 90), numCol('NetBillWeight', 100), numCol('StockWeight', 100),
                { k: 'WareHouse', t: 'WareHouse', w: 130 }, { k: 'LabNo', t: 'LabNo', w: 90 }, { k: 'CityName', t: 'CityName', w: 110 }] });
        footFix(G.hd);
    }

    /* grd_CellUpdated :3072 */
    function gridEdit(r, k, v, i) {
        if (k === 'Remarks') { r[k] = v; return; }
        if (k === 'WareHouse' || k === 'City' || k === 'JobLotId' || k === 'PackingTypeId') { r[k] = v === '' ? 0 : +v; G.grd.refresh(); return; }
        var n = v === '' ? 0 : toDouble(v);
        r[k] = n;
        if (k === 'GrossWight' || k === 'AddLesswt') {
            if (String(r.GrossWight == null ? '' : r.GrossWight) !== '' && String(r.NetWeight == null ? '' : r.NetWeight) !== '') {
                var net = r3(tn(r.GrossWight) + tn(r.AddLesswt));
                r.NetWeight = net; r.StockWeight = net;
            }
        }
        G.grd.refresh();
    }
    function addCopy(i) {                                                               // grd_ColumnButtonClick "Add"
        var src = G.grd.rows()[i]; if (!src) return;
        var c = {}; Object.keys(src).forEach(function (k) { c[k] = src[k]; });
        G.grd.rows().push(c);
        if (visible('btnUpdate')) c.Id = 0;
        G.grd.refresh();
    }
    function lineOf(r) {
        return { id: ti(r.Id), saleOrderDetailId: ti(r.SaleOrderDetailId), saleOrderId: ti(r.SaleOrderId), saleOrderNo: ti(r.OrderNo), refPartyId: ti(r.RefPartyId), itemId: ti(r.ItemId),
            jobLotId: ti(r.JobLotId), packingTypeId: ti(r.PackingTypeId), packUomId: ti(r.PackUomId), warehouseId: ti(r.WareHouse), cityId: ti(r.City),
            itemQty: tn(r.ItemQty), grossWeight: tn(r.GrossWight), wtCut: tn(r.WtCut), wtCutTotal: tn(r.WtCutTotal), addLess: tn(r.AddLesswt), netWeight: tn(r.NetWeight), stockWeight: tn(r.StockWeight),
            labNo: r.LabNo == null ? '' : String(r.LabNo), remarks: r.Remarks == null ? '' : String(r.Remarks), vehicleNo: field('txtVehicleNo') };
    }
    function deleteRow(i) {                                                              // grd_ColumnButtonClick "Delete"
        var r = G.grd.rows()[i]; if (!r) return;
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            if (!saveMode() && ti(r.Id) > 0) S.remove.push(lineOf(r));
            G.grd.removeAt(i);
            if (G.grd.rows().length === 0) SS.dis(cb.order, false);
        });
    }

    /* ------------------------------------------------------------------ detail entry */
    /* FormDetailValidation :820 */
    function validateDetail() {
        function stop(m, c) { return msg(m).then(function () { if (c) { if (c.focus) c.focus(); else $(c).focus(); } return false; }); }
        if (S.orderType !== 'GateSale' && (cb.order.text().trim() === '' || (+cb.order.value() || 0) === 0 || cb.order.text().trim() === '0')) return stop('Please Select an Order No First', cb.order);
        if (!cb.job.row()) return stop('Job/Lot Field is Required', cb.job);
        if (!cb.pack.row()) return stop('Packing Type Field is Required', cb.pack);
        if (toDouble($('txtQty').value) === 0) return stop('Qty Field is Required', 'txtQty');
        if (!cb.uom.row()) return stop('Pack Unit Field is Required', cb.uom);
        if (field('txtGrossWeight') === '' || toDouble($('txtGrossWeight').value) === 0) return stop('Gross Weight Field is Required', 'txtGrossWeight');
        if (toDouble($('txtGrossWeight').value) < toDouble($('txtNetWeight').value)) return stop('GrossWeight Not less than NetWeight');
        if (field('txtNetWeight') === '' || toDouble($('txtNetWeight').value) < 0) return stop('NetBillWeight Field is Empty or Less than zero Please Check', 'txtNetWeight');
        if (field('txtStockWeight') === '' || toDouble($('txtStockWeight').value) < 0) return stop('StockWeight Field is Empty or Less than zero Please Check', 'txtStockWeight');
        if (!cb.wh.row()) return stop('Warehouse Field is Required', cb.wh);
        return Promise.resolve(true);
    }
    /* Add_Click :403 */
    function addDetail() {
        return validateDetail().then(function (ok) {
            if (!ok) return;
            var gs = S.orderType === 'GateSale', it = cb.item.row();
            SS.dis(cb.order, true);
            G.grd.rows().push({ Id: 0, SaleOrderDetailId: gs ? 0 : (it ? it.SoDetailId : 0), SaleOrderId: gs ? 0 : cb.order.value(), OrderNo: gs ? '0' : cb.order.text(), RefPartyId: 0,
                ItemId: cb.item.value(), ItemName: cb.item.text(), JobLotId: cb.job.value(), JobLot: cb.job.text(), PackingTypeId: cb.pack.value(), PackingType: cb.pack.text(),
                PackUomId: ti(cb.uom.value()), PackUom: cb.uom.text().trim(), ItemQty: toDouble($('txtQty').value.trim()), GrossWight: toDouble($('txtGrossWeight').value.trim()),
                WtCut: toDouble($('txtWtCut').value.trim()), WtCutTotal: toDouble($('txtWtCuTtotal').value.trim()), AddLesswt: toDouble($('txtAddLessWt').value.trim()),
                NetWeight: toDouble($('txtNetWeight').value.trim()), StockWeight: toDouble($('txtStockWeight').value.trim()), ItemRate: 0, WareHouse: cb.wh.value(),
                LabNo: $('CmbLabNo').value, City: cb.city.value(), Remarks: $('txtRemarksDetail').value });
            grdSettings();
            resetDetail();
        }).catch(function (e) { return msg(e.message, 'Error Message'); });
    }
    /* ResetDetail :1766 */
    function resetDetail() {
        guard(function () {
            cb.item.clear(); cb.job.clear(); cb.pack.clear();
            setT('txtQty', ''); setT('txtGrossWeight', ''); setT('txtAddLessWt', ''); setT('txtNetWeight', ''); setT('txtStockWeight', ''); cb.uom.clear();
            first(cb.item, 'OrderItemId'); first(cb.job); first(cb.wh);
            $('CmbLabNo').value = '';
            cb.item.focus();
        });
    }
    /* grd_DoubleClick :2680 */
    function editRow(i) {
        var r = G.grd.rows()[i]; if (!r) return Promise.resolve();
        S.updateIdx = i;
        if (String(r.OrderNo == null ? '' : r.OrderNo) === '') SS.dis(cb.order, true);
        cb.order.setData([{ Id: String(r.SaleOrderId), OrderNo: String(r.OrderNo == null ? '' : r.OrderNo) }]);
        cb.order.setValue(ti(r.SaleOrderId));
        return orderLeave().then(function () {
            cb.item.setValue(String(r.ItemId));
            return itemLeave();
        }).then(function () {
            setCombo(cb.job, r.JobLotId); setCombo(cb.pack, r.PackingTypeId);
            setT('txtQty', String(r.ItemQty));
            setCombo(cb.uom, r.PackUomId);
            setT('txtGrossWeight', String(r.GrossWight)); setT('txtWtCut', String(r.WtCut)); setT('txtWtCuTtotal', String(r.WtCutTotal)); setT('txtAddLessWt', String(r.AddLesswt));
            setT('txtNetWeight', String(r.NetWeight)); setT('txtStockWeight', String(r.StockWeight));
            setCombo(cb.wh, r.WareHouse); $('CmbLabNo').value = r.LabNo == null ? '' : String(r.LabNo); setCombo(cb.city, r.City);
            show('Add', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        }).catch(function (e) { return msg(e.message, 'Error Message'); });
    }
    /* btnUpdateDetail_Click :2729 */
    function updateDetail() {
        return validateDetail().then(function (ok) {
            if (!ok) return;
            var r = G.grd.rows()[S.updateIdx]; if (!r) return;
            var gs = S.orderType === 'GateSale', it = cb.item.row();
            r.SaleOrderDetailId = gs ? 0 : (it ? it.SoDetailId : 0); r.SaleOrderId = cb.order.value(); r.OrderNo = cb.order.text(); r.ItemId = cb.item.value(); r.ItemName = cb.item.text();
            r.JobLotId = cb.job.value(); r.JobLot = cb.job.text(); r.PackingTypeId = cb.pack.value(); r.PackingType = cb.pack.text();
            r.ItemQty = toDouble($('txtQty').value); r.PackUom = cb.uom.text(); r.PackUomId = cb.uom.value(); r.GrossWight = toDouble($('txtGrossWeight').value);
            r.WtCut = toDouble($('txtWtCut').value); r.WtCutTotal = toDouble($('txtWtCuTtotal').value); r.AddLesswt = toDouble($('txtAddLessWt').value);
            r.NetWeight = toDouble($('txtNetWeight').value); r.StockWeight = toDouble($('txtStockWeight').value); r.WareHouse = cb.wh.value(); r.LabNo = $('CmbLabNo').value; r.City = cb.city.value();
            G.grd.refresh();
            show('Add', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            resetDetail();
        }).catch(function (e) { return msg(e.message, 'Error Message'); });
    }
    function cancelDetail() { show('Add', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); resetDetail(); }   // btnCancelUpdateDetial_Click

    /* ------------------------------------------------------------------ gate pass loading */
    /* grdGp_ColumnButtonClick :3292 */
    function loadGatePass(r) {
        if (!saveMode()) return;
        S.gpId = 0;
        S.orderType = r.OrderType == null ? '' : String(r.OrderType);
        if (String(r.Status) !== 'Accepted') throw new Error('Status Not Accepted Please check status');
        S.gpId = ti(r.OutwardGatePassId);
        setT('txtGatePassNo', r.GpSrNo == null ? '' : String(r.GpSrNo));
        SS.byText(cb.veh, 'VehicleDescription', r.VehicleType == null ? '' : String(r.VehicleType));
        return gatePassLeave();
    }
    /* combgatepass_Leave :2418 */
    function gatePassLeave() {
        if (!saveMode()) return Promise.resolve();
        return SE.api(API + '/gatepass/delivery-order' + SE.q({ gpId: S.gpId })).then(function (d) {
            if (d.found) {
                var h = d.head;
                S.gpId = ti(h.Id); S.soOrderId = ti(h.SaleOrderId); S.orderStatus = h.OrderStatus == null ? '' : String(h.OrderStatus);
                $('txtVehicleNo').value = h.VehicleNo == null ? '' : String(h.VehicleNo); $('txtBiltyNo').value = h.BiltyNo == null ? '' : String(h.BiltyNo);
                setT('txtFactoryWeight', h.FactoryWeight == null ? '' : String(h.FactoryWeight)); $('txtCarrAmount').value = h.NetPaid == null ? '' : String(h.NetPaid);
                $('txtDeliverTerm').value = S.orderStatus; S.deliveryOrderType = h.DeliveryOrderType == null ? '' : String(h.DeliveryOrderType);
                if (S.orderStatus === 'DeliveryOrder') {
                    deliveryLayout(true);
                    var cust = d.customers || [];
                    if (cust.length > 0) {
                        $('txtPartyCount').value = String(cust.length);
                        cb.cust.setData(cust.map(function (c) { return { Id: c.SupplierCustomerId, CompanyName: c.CompanyName }; }));
                        first(cb.cust);
                    }
                    var gdn = d.gdnGrossWeight || 0;
                    $('txtGdnGrossWeight').value = gdn > 0 ? String(gdn) : '0';
                    $('txtBalanceWeight').value = String(toDouble($('txtFactoryWeight').value) - gdn);
                    return deliverOrderRecordFill();
                }
                $('txtBalanceWeight').value = '0'; $('txtPartyCount').value = '0'; deliveryLayout(false);
                return;
            }
            if (S.orderType === 'SaleOrder') return gatePassRecordFill();
            if (S.orderType === 'GateSale') {
                SS.dis(cb.order, true); $('txtDeliverTerm').value = 'Load';
                return gatePassRecordFillWithoutOrder().then(function () { itemFillWithoutOrder(); cb.item.focus(); });
            }
        }).catch(function (e) { return msg(e.message, 'Error Message'); });
    }
    /* DeliverOrderRecordFill :1379 */
    function deliverOrderRecordFill() {
        return SE.api(API + '/gatepass/delivery-lines' + SE.q({ soOrderId: S.soOrderId, customerId: +cb.cust.value() || 0 })).then(function (g) {
            var rows = g.rows || [];
            if (rows.length === 0) return;
            G.grd.setRows(rows.map(function (r) {
                return { Id: 0, SaleOrderDetailId: r.SaleOrderDetailId, SaleOrderId: r.OrderId, OrderNo: r.OrderNo, RefPartyId: r.RefPartyId, ItemId: r.ItemId, ItemName: r.Item, JobLotId: r.JobId, JobLot: r.Job,
                    PackingTypeId: r.PackingTypeId, PackingType: r.PackingType, PackUomId: r.UOMId, PackUom: r.UOM, ItemQty: tn(r.Qty), GrossWight: tn(r.GrossWight), WtCut: tn(r.WtCut),
                    WtCutTotal: tn(r.WtCutTotal), AddLesswt: tn(r.AddLesswt), NetWeight: tn(r.NetWeight), StockWeight: tn(r.StockWeight), ItemRate: tn(r.ItemRate), WareHouse: r.WarehouseId,
                    LabNo: r.LabNo, City: r.CityId, Remarks: r.LoadingRemarks };
            }));
            grdSettings();
            guard(netAndStockWeight);
        });
    }
    /* NetandStockWeight :3114 */
    function netAndStockWeight() {
        G.grd.rows().forEach(function (r) { var w = toDouble(r.GrossWight) + toDouble(r.AddLesswt); r.NetWeight = w; r.StockWeight = w; });
        G.grd.refresh();
    }
    /* GatePassRecordFill :1305 */
    function gatePassRecordFill() {
        return SE.api(API + '/gatepass/rows' + SE.q({ gpId: S.gpId, netWeight: true })).then(function (d) {
            var rows = d.rows || [];
            if (rows.length === 0) return;
            var h = rows[0];
            S.gpId = ti(h.Id);
            if ((d.wbRows || 0) > 0) {
                $('txtVehicleNo').value = h.VehicleNo == null ? '' : String(h.VehicleNo); $('txtBiltyNo').value = h.BiltyNo == null ? '' : String(h.BiltyNo);
                $('txtRemarks').value = h.OtherRemarks == null ? '' : String(h.OtherRemarks); S.gpType = h.GatepassType == null ? '' : String(h.GatepassType);
                $('txtCarrAmount').value = h.NetPaid == null ? '' : String(h.NetPaid);
                guard(function () { setT('txtQty', h.NoOfPackages == null ? '' : String(h.NoOfPackages)); });
                setT('txtFactoryWeight', String(d.netWeight));
                setCombo(cb.wh, h.WarehouseId);
                if (ti(h.SaleOrderId) <= 0) throw new Error('Sale Order Id not Found');
                cb.order.setData(rows.map(function (r) { return { Id: String(r.SaleOrderId), OrderNo: r.SupplierContractCode == null ? '' : String(r.SupplierContractCode) }; }));
            }
        }).catch(function (e) { return msg(e.message); });
    }
    /* GatePassRecordFillWinthoutSaleorder :1262 */
    function gatePassRecordFillWithoutOrder() {
        return SE.api(API + '/gatepass/rows' + SE.q({ gpId: S.gpId })).then(function (d) {
            var rows = d.rows || [];
            if (rows.length === 0) return;
            var h = rows[0];
            S.gpId = ti(h.Id);
            $('txtVehicleNo').value = h.VehicleNo == null ? '' : String(h.VehicleNo); $('txtBiltyNo').value = h.BiltyNo == null ? '' : String(h.BiltyNo);
            $('txtRemarks').value = h.OtherRemarks == null ? '' : String(h.OtherRemarks); S.gpType = h.GatepassType == null ? '' : String(h.GatepassType);
            $('txtCarrAmount').value = h.NetPaid == null ? '' : String(h.NetPaid);
            guard(function () { setT('txtQty', h.NoOfPackages == null ? '' : String(h.NoOfPackages)); });
            guard(function () { setT('txtsuppwt', h.SupplierWeight == null ? '' : String(h.SupplierWeight)); setT('txtFactoryWeight', h.FactoryWeight == null ? '' : String(h.FactoryWeight)); });
            $('txtwtdiff').value = h.DifferenceWeight == null ? '' : String(h.DifferenceWeight);
            setCombo(cb.wh, h.WarehouseId);
            cb.cust.setData(S.customers); setCombo(cb.cust, h.SupplierCustomerId);
        }).catch(function (e) { return msg(e.message); });
    }

    /* ------------------------------------------------------------------ reset (:1701) */
    function pendingFill() {                                                              // PendingGatepass :993
        return SE.api(API + '/pending').then(function (g) { G.gp.setRows(g.rows || []); }).catch(fail);
    }
    function defaultConfig() {                                                            // defaultConfiquration :884
        var d = S.defaults;
        if (d.cityId !== undefined) setCombo(cb.city, d.cityId);
        if (d.jobLotId !== undefined) setCombo(cb.job, d.jobLotId);
        if (d.packingTypeId !== undefined) setCombo(cb.pack, d.packingTypeId);
        if (d.warehouseId !== undefined) setCombo(cb.wh, d.warehouseId);
    }
    function reset() {
        S.files = []; S.removed = []; S.existing = []; S.remove = [];
        S.gpId = 0; S.id = 0; S.orderStatus = ''; S.orderType = ''; S.deliveryOrderType = '';
        $('txtGdnGrossWeight').value = '0'; $('txtPartyCount').value = '0'; $('txtBalanceWeight').value = '0';
        cb.cust.clear(); cb.ref.clear(); cb.trans.clear();
        ['txtCarrAmount', 'txtsuppwt', 'txtFactoryWeight', 'txtwtdiff', 'txtGatePassNo', 'txtVehicleNo', 'txtBiltyNo', 'txtRemarks', 'txtDeliverTerm', 'txtdocno'].forEach(function (k) { $(k).value = ''; });
        cb.veh.clear(); cb.order.clear(); cb.item.clear();
        guard(function () { setT('txtQty', ''); });
        ['txtAddLessWt', 'txtNetWeight', 'txtStockWeight'].forEach(function (k) { $(k).value = ''; });
        $('CmbLabNo').value = ''; cb.uom.clear();
        show('btnSave', true); show('btnUpdate', false);
        deliveryLayout(false);
        grdSettings(); G.grd.setRows([]);
        SS.dis(cb.cust, false); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); show('Add', true); SS.dis(cb.order, false);
        $('DocDate').focus();
        cb.cust.setData([]); cb.item.setData([]);
        defaultConfig();
        return Promise.all([SE.api(API + '/next-no').then(function (d) { if (d.nextNo > 0) $('txtdocno').value = String(d.nextNo); else throw new Error('Max Number Not Found'); }).catch(fail), pendingFill()]);
    }
    /* toolStripButton1_Click :2778 */
    function refresh() {
        return SE.api(API + '/refresh').then(function (d) {
            cb.trans.setData(d.transporters); S.jobLots = d.jobLots; cb.job.setData(S.jobLots); S.packs = d.packingTypes; cb.pack.setData(S.packs); first(cb.pack);
            S.whs = d.warehouses; cb.wh.setData(S.whs); first(cb.wh); S.cities = d.cities; cb.city.setData(S.cities); first(cb.city);
            G.grd.refresh();
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ Save (Insert :423) */
    function formValidation() {
        function stop(m, c) { return msg(m, 'Message').then(function () { if (c) { if (c.focus) c.focus(); else $(c).focus(); } return false; }); }
        var no = field('txtdocno');
        if (no === '' || no === '0') return stop('DocNo Field is Required', 'txtdocno');
        if (!cb.cust.row()) return stop('Supplier Field is Required', cb.cust);
        if ($('txtDeliverTerm').value === '') return stop('Delivery Term Field is Required', 'txtDeliverTerm');
        var fr = field('txtCarrAmount');
        if (cb.trans.row() && (fr === '' || fr === '0')) return stop('Freight Field is Required', 'txtFactoryWeight');
        if (toDouble(fr) > 0 && !cb.trans.row()) return stop('Transport Field is Required', cb.trans);
        var gp = field('txtGatePassNo');
        if (gp === '' || gp === '0') return stop('Gatepass Field is Required', 'txtGatePassNo');
        var fw = field('txtFactoryWeight');
        if (fw === '' || fw === '0') return stop('FactoryWeight Field is Required', 'txtFactoryWeight');
        return Promise.resolve(true);
    }
    function insert(id) {
        var rows = G.grd.rows();
        if (rows.length === 0) return msg('Grid Record Not Found', 'Database Error');
        return formValidation().then(function (ok) {
            if (!ok) return;
            return SE.ask(id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?', 'Confirm').then(function (yes) {
                if (!yes) return;
                var body = { id: id, docDate: $('DocDate').value, docNo: field('txtdocno'), customerId: +cb.cust.value() || 0, deliveryTerm: field('txtDeliverTerm'), transporterId: +cb.trans.value() || 0,
                    carriageAmount: field('txtCarrAmount'), gatePassNo: $('txtGatePassNo').value, vehicleTypeId: +cb.veh.value() || 0, vehicleNo: field('txtVehicleNo'), biltyNo: field('txtBiltyNo'),
                    refPartyId: +cb.ref.value() || 0, factoryWeight: field('txtFactoryWeight'), partyWeight: field('txtsuppwt'), remarks: field('txtRemarks'), gpId: S.gpId, orderType: S.orderType,
                    partyCount: field('txtPartyCount'), gdnGrossWeight: field('txtGdnGrossWeight'),
                    lines: rows.map(lineOf), removed: id > 0 ? S.remove : [],
                    attachments: { files: S.files.map(function (f) { return { name: f.name, base64: f.base64 }; }), removeAttachmentIds: S.removed } };
                return SE.api(API, { method: 'POST', body: body }).then(function (r) {
                    return SE.alert(r.message).then(function () {
                        var preview = $('ChkBox').checked, wages = r.wagesStatus;
                        return reset().then(function () {
                            if (wages) return msg('Contractor wages bill is compulsory for this Goods Dispatch Note (Gross Weight ' + r.grossWeight + '). The wages bill form is not available on the web; enter it on the Labour Wages screen.', 'Message');
                        }).then(function () { if (preview) print(r.id); });
                    });
                }).catch(function (e) { return msg(e.message, 'Database Error'); });
            });
        });
    }
    function print(id) {                                                                  // CommonServices.InvGdnSlipSteel
        if (!(id > 0)) return msg('No Record Found For Display', 'Message');
        SE.printRpt('1515-InvRptGdnSlipAndRegister.rpt', { id: id });
    }
    function printA(id) { SE.printRpt('1515A-InvRptGdnSlipDliveryChallan.rpt', { id: id }); }   // PrintReportA :3160
    function doSave() { S.id = 0; return insert(0); }
    function doUpdate() {
        if (S.id === 0) return msg('Record not update because Id not found', 'Database Error');
        return insert(S.id);
    }
    function doDelete() {                                                                 // btnDelete_Click :2961
        return SE.ask('Are you sure to Delete?', 'Confirm').then(function (yes) {
            if (!yes) return;
            return SE.api(API + '/' + S.id, { method: 'DELETE' }).then(function (r) { return SE.alert(r.message).then(function () { return reset(); }); })
                .catch(function (e) { return msg(e.message, 'Error Message'); });
        });
    }

    /* ------------------------------------------------------------------ ReadById :688 */
    function readById(id) {
        S.id = id; S.remove = [];
        return SE.api(API + '/' + id).then(function (d) {
            var h = d.head;
            $('txtdocno').value = h.DocNo == null ? '' : String(h.DocNo); $('DocDate').value = SE.dateInput(h.DocDate);
            tabs.select('tabPage1');
            cb.cust.setData(S.customers); setCombo(cb.cust, h.SupplierCustomerId); SS.dis(cb.cust, true);
            setCombo(cb.trans, h.TransporterId);
            $('txtCarrAmount').value = h.CarriageAmount == null ? '' : String(h.CarriageAmount);
            $('txtGatePassNo').value = h.GpNo == null ? '' : String(h.GpNo);
            setCombo(cb.veh, h.VehicleTypeId);
            $('txtVehicleNo').value = h.VehicleNo == null ? '' : String(h.VehicleNo); $('txtBiltyNo').value = h.BiltyNo == null ? '' : String(h.BiltyNo);
            setCombo(cb.ref, h.ReferencePartyId);
            guard(function () { setT('txtFactoryWeight', h.FactoryWeight == null ? '' : String(h.FactoryWeight)); setT('txtsuppwt', h.PartyWeight == null ? '' : String(h.PartyWeight)); });
            $('txtRemarks').value = h.RemarksHeader == null ? '' : String(h.RemarksHeader);
            $('txtDeliverTerm').value = h.DeliveryTerm == null ? '' : String(h.DeliveryTerm);
            S.orderType = h.OrderType == null ? '' : String(h.OrderType);
            S.gpId = ti(h.OutwardGatePassId);
            var lines = d.lines || [], gross = 0;
            var rows = lines.map(function (l) {
                gross += toDouble(l.GrossWeight);
                return { Id: l.Id, SaleOrderDetailId: l.SaleOrderDetailId, SaleOrderId: l.SaleOrderId, OrderNo: l.SaleOrderNo, RefPartyId: l.ReferencePartyId, ItemId: l.ItemId, ItemName: l.ItemName,
                    JobLotId: l.JobLotId, JobLot: l.JobLotDescription, PackingTypeId: l.PackingTypeId, PackingType: l.PackingType, PackUomId: l.ItemUomId, PackUom: l.UOMCode, ItemQty: tn(l.ItemQty),
                    GrossWight: tn(l.GrossWeight), WtCut: tn(l.WtCut), WtCutTotal: tn(l.WtCutTotal), AddLesswt: tn(l.AdLsWeight), NetWeight: tn(l.NetBillWeight), StockWeight: tn(l.StockWeight),
                    ItemRate: 0, WareHouse: l.WarehouseId, LabNo: l.LabReportRef, City: l.CityId, Remarks: l.RemarksDetail };
            });
            if (S.orderType === 'DeliveryOrder') {
                $('txtPartyCount').value = '1'; deliveryLayout(true);
                var gdn = d.gdnGrossWeight || 0;
                if (gdn > 0) {
                    var disp = gdn - gross;
                    $('txtGdnGrossWeight').value = String(disp); $('txtBalanceWeight').value = String(toDouble($('txtFactoryWeight').value) - disp);
                }
            } else deliveryLayout(false);
            G.grd.rows().length = 0; rows.forEach(function (r) { G.grd.rows().push(r); });
            grdSettings();
            S.existing = d.attachments || []; S.files = []; S.removed = [];
            show('btnSave', false); show('btnUpdate', true);
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history (HistoryGridFill :2115) */
    function showHistory() {
        var type = $('rdentrydate').checked ? 'entry' : $('rdmodifydate').checked ? 'modify' : $('rdapproveddate').checked ? 'approved' : 'document';
        var q = { dateType: type, fromDate: $('FromDateHistory_chk').checked ? $('FromDateHistory').value : '', toDate: $('ToDateHistory_chk').checked ? $('ToDateHistory').value : '',
                  fromNo: ti($('FromDocNoHistory').value), toNo: ti($('ToDocNoHistory').value), customerId: +cb.hcust.value() || 0 };
        return SE.api(API + '/history' + SE.q(q)).then(function (rows) {
            if (rows.length > 0) G.hist.setRows(rows); else { G.hist.setRows([]); G.hd.setRows([]); }
        }).catch(fail);
    }
    function resetHistory() {                                                             // btnResetHistory_Click :2051
        $('FromDateHistory').value = SE.addDays(-3); $('ToDateHistory').value = SE.today(); $('FromDocNoHistory').value = ''; $('ToDocNoHistory').value = '';
        cb.hcust.clear(); G.hist.setRows([]); G.hd.setRows([]);
    }
    function histOpen(r) { S.id = ti(r.Id); return readById(S.id); }                      // DataGridHistory_DoubleClick
    function histButton(k, r) {                                                           // DataGridHistory_ColumnButtonClick
        if (k === 'Edit') return reset().then(function () { return readById(ti(r.Id)); });
        if (k === 'Print') return print(ti(r.Id));
        if (k === 'Print1' || k === '1515A-Print') return printA(ti(r.Id));
    }
    function histLink(k, r) {                                                             // DataGridHistory_LinkClicked
        if (k === 'NoOfAttachments') return SS.showAttachments(API, r.Id);
        if (k === 'GpNo') window.open('/sale/steel/outward-gate-pass?record=' + encodeURIComponent(ti(r.GpId)), '_blank');
    }
    function histSelected(r) {                                                            // DataGridHistory_SelectionChanged
        var seq = ++S.histSeq;
        if (!r) { G.hd.setRows([]); return; }
        SE.api(API + '/' + r.Id + '/history-detail', { quiet: true }).then(function (rows) { if (seq === S.histSeq) G.hd.setRows(rows); })
            .catch(function (e) { if (seq === S.histSeq) SE.alert(e.message, 'Message'); });
    }

    /* ------------------------------------------------------------------ keys (InvFrmGRN_KeyDown :2270) */
    function onKey(e) {
        SS.enterTab(e);
        var k = (e.key || '').toLowerCase(), tab = tabs.index();
        if (e.ctrlKey && k === 's' && tab === 0) { e.preventDefault(); if (saveMode()) doSave(); }
        if (e.ctrlKey && k === 'n' && tab === 0) { e.preventDefault(); reset(); }
        if (e.ctrlKey && k === 't') { e.preventDefault(); if (tab === 1) tabs.select('tabPage1'); else tabs.select('tabPage2'); }
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (e.ctrlKey) e.preventDefault(); if (window.history.length > 1) window.history.back(); }
        if (e.ctrlKey && k === 'u') { e.preventDefault(); if (visible('btnUpdate')) doUpdate(); }
        if (e.ctrlKey && k === 'p') { e.preventDefault(); if (visible('printToolStripButton')) print(S.id); }
        if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); cb.order.focus(); }
        if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); $('grd').focus(); }
        if (e.ctrlKey && k === 'l' && tab === 1) { e.preventDefault(); showHistory(); }
        if (e.ctrlKey && e.key === 'Enter' && tab === 1) { var c = G.hist.cur(); if (c) { e.preventDefault(); S.id = ti(c.Id); readById(S.id); } }
        if (e.ctrlKey && k === 'h' && tab === 1) { e.preventDefault(); gdnHistory(); }
        if (e.ctrlKey && e.key === 'ArrowRight') { e.preventDefault(); $('grdGp').focus(); }
        if (e.ctrlKey && e.key === 'Enter' && tab === 0 && saveMode()) {
            var g = G.gp.cur();
            if (g) {
                e.preventDefault();
                if (String(g.Status) === 'Accepted') { setT('txtGatePassNo', String(g.GpSrNo)); gatePassLeave().then(function () { cb.cust.focus(); }); }
                else msg('Status Not Accepted Please check status', 'Message');
            }
        }
    }
    function gdnHistory() { msg('The Gdn History screen (frmGDNHistory) is not available in the web application.', 'Message'); }   // btnGdnFormHistory_Click :3206

    /* ------------------------------------------------------------------ load (InvFrmGRN_Load :1918) */
    function decimalOnly(el) {                                                            // CommonServices.OnlytextdecimelFunction
        el.addEventListener('keypress', function (e) {
            if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault();
            else if (e.key === '.' && el.value.indexOf('.') >= 0) e.preventDefault();
        });
    }
    function load() {
        makeCombos();
        tabs = SE.tabs('tabControl1', function (id) { if (id === 'tabPage2') $('FromDateHistory').focus(); else $('ToDateHistory').focus(); });
        SE.digitsOnly($('FromDocNoHistory')); SE.digitsOnly($('ToDocNoHistory')); SE.digitsOnly($('txtGatePassNo')); SE.digitsOnly($('txtCarrAmount'));
        ['txtQty', 'txtGrossWeight', 'txtWtCut'].forEach(function (k) { decimalOnly($(k)); });
        $('txtGatePassNo').disabled = true;
        makeGrid(false); makeOtherGrids();
        wire();
        SE.api(API + '/initial').then(function (d) {
            S.rights = d.rights || {}; S.customers = d.customers; S.items = d.items; S.jobLots = d.jobLots; S.packs = d.packingTypes; S.whs = d.warehouses; S.cities = d.cities;
            S.defaults = d.defaults || {}; S.wages = !!d.wagesStatus; S.historyDays = d.historyDays;
            if (d.nextNo > 0) $('txtdocno').value = String(d.nextNo); else msg('Max Number Not Found', 'Error Message');
            cb.ref.setData(d.refParties); cb.trans.setData(d.transporters); cb.veh.setData(d.vehicleTypes); cb.job.setData(S.jobLots);
            cb.pack.setData(S.packs); first(cb.pack); cb.wh.setData(S.whs); first(cb.wh); cb.city.setData(S.cities); first(cb.city);
            G.gp.setRows((d.pending && d.pending.rows) || []);
            defaultConfig();
            cb.hcust.setData(d.historyCustomers || []);
            $('DocDate').value = SE.today(); $('ChkBox').checked = true;
            $('btnSave').disabled = !S.rights.save; $('printToolStripButton').disabled = !S.rights.print; $('btnUpdate').disabled = !S.rights.update; show('btnDelete', !!S.rights['delete']);
            show('btnSave', true); show('btnUpdate', false); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            $('FromDateHistory').value = SE.addDays(-d.historyDays); $('ToDateHistory').value = SE.today();
            $('txtGdnGrossWeight').value = '0'; $('txtBalanceWeight').value = '0'; $('txtPartyCount').value = '0';
            $('DocDate').focus();
        }).catch(fail);
        var rec = SE.param('record'); if (rec) setTimeout(function () { readById(+rec); }, 800);
    }

    function wire() {
        $('btnNew').onclick = reset; $('btnSave').onclick = doSave; $('btnUpdate').onclick = doUpdate; $('btnDelete').onclick = doDelete; $('toolStripButton1').onclick = refresh;
        $('btnAttachment').onclick = function () { SS.attachmentDialog(S, API, function () { return S.id; }); };
        $('printToolStripButton').onclick = function () { print(S.id); };
        $('btnPrint260A').onclick = function () { if (S.id > 0) printA(S.id); else msg('No Record Selected', 'Error Message'); };
        $('Add').onclick = addDetail; $('btnUpdateDetail').onclick = updateDetail; $('btnCancelUpdateDetial').onclick = cancelDetail;
        $('btnShow').onclick = showHistory; $('btnResetHistory').onclick = resetHistory;
        $('btnRefreshHistory').onclick = function () { SE.api(API + '/history-customers').then(function (rows) { cb.hcust.setData(rows); }).catch(fail); };
        $('btnGdnFormHistory').onclick = gdnHistory;
        /* TextChanged handlers */
        $('txtQty').addEventListener('input', function () { guard(function () { totalWeight(); total(); }); });
        $('txtGrossWeight').addEventListener('input', function () { guard(total); });
        $('txtWtCut').addEventListener('input', function () { guard(total); });
        $('txtAddLessWt').addEventListener('input', function () { guard(total); });
        $('txtAddLessWt').addEventListener('blur', function () { guard(total); });
        $('txtsuppwt').addEventListener('input', function () { guard(totalSupplierWeight); });
        $('txtFactoryWeight').addEventListener('input', function () { guard(totalSupplierWeight); });
        document.addEventListener('keydown', function (e) { try { onKey(e); } catch (x) { SE.alert(x.message); } });
    }

    document.addEventListener('DOMContentLoaded', load);
})();
