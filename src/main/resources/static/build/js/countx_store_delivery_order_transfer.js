/* ============================================================================================
 * Screen 331 "Delivery Order (Stock Transfer)" — Architecture.WinApp.Sale.DeliveryOrder (DeliveryOrder.cs)
 * opened with ScreenName / Tag "DeliveryOrderTransfer": DocumentTypeId 84, DO Type locked to 2 "StockTransfer".
 *
 *   InitializeComponentMethod:520   rights, configuration, every combo, DO type 2, history dates
 *   CmbDeliveryOrderType_Leave:3431 the StockTransfer branch (items from the global list, weight read-only,
 *                                   Branch From / Branch To labels, loader grid hidden)
 *   btnplus_Click:1207              FormValidationDetail:1136 then one grid row (type-2 row layout :1232)
 *   grd_DoubleClick:1483 (StockTransfer branch :1531) / btnUpdateDetail_Click:1410 / btnCancelUpdateDetial_Click:1399
 *   DeleteDetailRow:1568 / AddDetailRow:1618 / grd_CellUpdated:1352
 *   CalculateWeight:2790 / CalculateGrossWeight:2806   the entry-bar arithmetic
 *   Insert():1768                   Save / SaveAs / Update — validations, confirmations, stack warning
 *   ReadById:2020, Reset():2200, ResetDetail:2283, gridhistoryfill:2388, grdhistory_*:2537-2719
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, ci = C.ci, num = C.num, intOf = C.intOf;
    var api = '/api/store/delivery-order-transfer';

    var look = { rights: {}, config: {}, branches: [], vehicleTypes: [], suppliers: [], warehouses: [], items: [], uoms: [],
                 cropYears: [], jobLots: [], packingTypes: [], refParties: [], otherItems: [], historyCustomers: [] };
    var cfg = {};
    var recId = 0;                  // RecId
    var saveAsFromId = 0;           // the history row SaveAs was pressed on (server re-checks it)
    var isApproved = false;
    var scheduleExist = false;      // never set in this mode (no schedule rows); kept for the grid's column rule
    var table = [];                 // grd's DataTable
    var dtExpense = [];             // GridExpense's DataTable
    var removed = [];               // lstRemoveRecord (ids of saved rows)
    var updateDetailIndex = -1;
    var refIds = { RefDocumentTypeId: '', RefDocIdNo: '', RefDocSubIdNo: '' };   // txtRefDocTypeId / txtDocNoId / txtSubNoId (Visible = false)
    var dthistoryfill = [];
    var histSel = -1;
    var docNoReq = 0, stockReq = 0;

    var DO_TYPES = [{ Id: 1, Name: 'Local' }, { Id: 2, Name: 'StockTransfer' }];                 // DeliveryOrderTypeBind:776
    var SALE_TYPES = [{ Id: 1, Name: 'Weigh Bridge Based Billing' }, { Id: 2, Name: 'Pack Size Based Billing' }];   // SaleTypeBind:831

    /* ------------------------------------------------------------------ formatting */

    /** .NET double.ToString() — 15 significant digits. */
    function clr(v) { var n = num(v); return String(Number(n.toPrecision(15))); }
    /** .NET ToString("#,##0.###")-style: thousands separators, up to d decimals, 0 prints "0". */
    function fmt(v, d) { return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d }); }
    function show(id, on) { $id(id).classList.toggle('is-hidden', !on); }
    function visible(id) { return !$id(id).classList.contains('is-hidden'); }
    function val(id) { return intOf($id(id).value); }
    function selText(id) { var e = $id(id); var o = e.selectedOptions && e.selectedOptions[0]; return o && e.value !== '' ? o.textContent : ''; }
    function addDays(iso, days) {
        var d = new Date(iso + 'T00:00:00'); d.setDate(d.getDate() + days);
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }
    /** Janus RetrieveStructure caption: the column name split at its word boundaries ("ItemUOM" → "Item UOM"). */
    function cap(k) { return String(k).replace(/([a-z0-9])([A-Z])/g, '$1 $2').replace(/([A-Z]+)([A-Z][a-z])/g, '$1 $2'); }
    /** Conversion.ToDateTime(DBNull) = 01-Jan-1900. */
    function dt1900(v) { return v ? v : '1900-01-01T00:00:00'; }
    function dmy(v) { var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || '')); return m ? m[3] + '-' + m[2] + '-' + m[1] : ''; }

    /* ------------------------------------------------------------------ combos
       InfragisticsHelper.BindAndRetainSelection:15 — an empty source empties the combo (Text "", DataSource null);
       insertDefaultRow puts "-- Select --" (value 0) first and selects it; the previous value is kept when it is in
       the source (by value, or by previousValueColumnName text), otherwise Rows[activateRowIndex] is activated when
       ActivateRow, otherwise the Text is emptied. A hidden blank option stands for an empty Text. */
    function bindCombo(id, rows, o) {
        var el = $id(id);
        rows = rows || [];
        if (!rows.length) { el.innerHTML = '<option value=""></option>'; el.value = ''; return; }
        var list = o.defaultRow ? [{ Id: 0, Name: '-- Select --' }].concat(rows) : rows;
        el.innerHTML = '<option value="" hidden></option>' + list.map(function (r) {
            return '<option value="' + esc(r.Id) + '">' + esc(r.Name) + '</option>';
        }).join('');
        el.value = o.defaultRow ? '0' : '';
        var idx = o.activateIndex || 0, act = o.activate !== false;
        var prev = o.prev, hit = null;
        if (prev !== null && prev !== undefined) {
            hit = rows.find(function (r) { return String(o.byText ? r.Name : r.Id) === String(prev); }) || null;
        }
        if (hit) el.value = String(hit.Id);
        else if (act && list.length > idx) el.value = String(list[idx].Id);
        else el.value = '';
    }
    /** combo.Value = v (an id not in the list leaves the combo empty). */
    function setValue(id, v) {
        var el = $id(id), s = String(v === null || v === undefined ? '' : v);
        var hit = Array.prototype.some.call(el.options, function (op) { return op.value === s && s !== ''; });
        el.value = hit ? s : '';
    }
    /** combo.Text = t (LimitToList: a text not in the list empties it). */
    function setText(id, t) {
        var el = $id(id), s = String(t || '');
        var op = Array.prototype.find.call(el.options, function (x) { return x.value !== '' && x.textContent === s; });
        el.value = op ? op.value : '';
    }

    function branchNameBind() {                                                  // :794
        bindCombo('cmbBranchFrom', look.branches, { prev: val('cmbBranchFrom'), activate: false });
        bindCombo('cmbBranchTo', look.branches, { prev: val('cmbBranchTo'), activate: false });
        branchLabels(look.branches.length > 1 && val('CmbDeliveryOrderType') === 2);
    }
    function branchLabels(two) {
        $id('label13').textContent = two ? 'Branch From' : 'Branch Name';
        show('label18', two);
        show('cmbBranchTo', two);
    }
    function vehicleTypesBind() { bindCombo('CmbVehicleType', look.vehicleTypes, { prev: val('CmbVehicleType') }); }   // :819
    function saleTypeBind() { bindCombo('CmbSaleType', SALE_TYPES, { prev: null, activate: false }); }                 // DDL.BindDDL:840
    function bindSupplierName() {                                                // :880
        bindCombo('CmbSupplierCustomer', look.suppliers.map(function (s) { return { Id: s.Id, Name: s.CompanyName }; }),
            { prev: val('CmbSupplierCustomer'), activate: false });
    }
    function warehouseBind() { bindCombo('CmbWareHouse', look.warehouses, { prev: val('CmbWareHouse'), defaultRow: true }); }  // :911
    function cropBind() { bindCombo('CmbCropYear', look.cropYears, { prev: val('CmbCropYear'), defaultRow: true }); }        // :991
    function jobLotBind() { bindCombo('CmbJobLot', look.jobLots, { prev: val('CmbJobLot'), defaultRow: true }); }            // :1006
    function packingTypeBind() { bindCombo('CmbPackingType', look.packingTypes, { prev: val('CmbPackingType'), defaultRow: true }); }  // :1022
    function historyComboBind() {                                                // :693
        bindCombo('CmbCustomerHistory', look.historyCustomers, { prev: val('CmbCustomerHistory'), activate: false });
    }

    /** ItemNameBind:947 — DO type 2: value member Id, display ItemName / ItemCode, "-- Select --" first, Rows[1] activated
     *  when the old item is not kept; the UOM list is rebound for the item selected BEFORE the rebind. */
    function itemNameBind() {
        var id = val('CmbItemName') > 0 ? val('CmbItemName') : 0;
        var key = $id('rdSearchByName').checked ? 'ItemName' : 'ItemCode';
        bindCombo('CmbItemName', look.items.map(function (i) { return { Id: i.Id, Name: i[key] }; }),
            { prev: id, defaultRow: true, activateIndex: 1 });
        packUomFromGlobalBind(id);
    }
    function uomsOf(itemId) {                                                    // dtUomFromGloablUomScheduleByItemId:2159
        return look.uoms.filter(function (u) { return u.ItemId === itemId; });
    }
    function packUomFromGlobalBind(itemId) {                                     // :1037 — kept by its UOMCode text
        var prevText = selText('CmbPackUom');
        bindCombo('CmbPackUom', uomsOf(itemId).map(function (u) { return { Id: u.Id, Name: u.UOMCode }; }),
            { prev: prevText, byText: true, defaultRow: true });
    }
    function selectedUom() {
        var id = val('CmbPackUom');
        if (!id) return null;
        return look.uoms.find(function (u) { return u.Id === id; }) || null;
    }
    function selectedItem() { var id = val('CmbItemName'); return look.items.find(function (i) { return i.Id === id; }) || null; }
    function configDefaults() {                                                  // GetConfigurationsFromGlobalAndBindValuesInColumns:740
        if (cfg.JobLot) setValue('CmbJobLot', cfg.JobLot);
        if (cfg.CropYear) setValue('CmbCropYear', cfg.CropYear);
        if (cfg.PackingType) setValue('CmbPackingType', cfg.PackingType);
        if (cfg.Warehouse) setValue('CmbWareHouse', cfg.Warehouse);
    }
    function configurations() {                                                  // GetConfigurationsFromGlobal:705
        show('lblChkIsStockReservedPerParty', !!cfg.IsStockReservedPerParty);
        show('BtnLoadCustomerSchedule', !!look.isCustomerPortal);
    }

    /** CmbDeliveryOrderType_Leave:3431 — the type-2 branch. */
    function deliveryOrderTypeLeave() {
        if (val('CmbDeliveryOrderType') === 1) {
            branchLabels(false);
        } else {
            $id('txtweight').readOnly = true;                                    // :3455
            branchLabels(true);                                                  // :3456-3458 (always, whatever the branch count)
        }
        itemNameBind();
    }

    /* ------------------------------------------------------------------ entry bar arithmetic */

    var TXT_HANDLERS = {};
    /** txt.Text = v — TextChanged runs only when the text really changes. */
    function setTxt(id, v) {
        var el = $id(id), s = String(v);
        if (el.value === s) return;
        el.value = s;
        if (TXT_HANDLERS[id]) TXT_HANDLERS[id]();
    }
    function activeTag() { var a = document.activeElement; return a && a.getAttribute ? a.getAttribute('data-tag') : null; }

    function calculateWeight() {                                                 // :2790
        var qty = num($id('txtqty').value);
        var u = selectedUom();
        var pack = u ? num(u.Equivalent) : 0;
        var w = qty * pack;
        setTxt('txtweight', fmt(w, 3));
        setTxt('txtGrossWeight', fmt(w, 3));
    }
    function calculateGrossWeight() {                                            // :2806
        var weight = num($id('txtweight').value);
        if (weight <= 0) { setTxt('txtGrossWeight', '0'); return; }
        var qty = num($id('txtqty').value);
        var packingUnit = num($id('txtPackUnit').value);
        var packingWeight = num($id('txtPackWeight').value);
        if (activeTag() === 'PackWeight') {
            packingUnit = qty > 0 ? packingWeight / qty : 0;
            setTxt('txtPackUnit', fmt(packingUnit, 5));
        } else {
            packingWeight = packingUnit * qty;
            setTxt('txtPackWeight', fmt(packingWeight, 3));
        }
        setTxt('txtGrossWeight', fmt(weight + packingWeight, 3));
    }
    TXT_HANDLERS.txtqty = calculateWeight;                                       // txtqty_TextChanged:2852
    TXT_HANDLERS.txtweight = calculateGrossWeight;                               // txtweight_TextChanged:2869
    TXT_HANDLERS.txtPackUnit = calculateGrossWeight;                             // txtPackUnit_TextChanged:2881
    TXT_HANDLERS.txtPackWeight = calculateGrossWeight;                           // txtPackSize_TextChanged:2864

    function qtyChanged() { calculateWeight(); }
    function weightChanged() { calculateGrossWeight(); }
    function packUnitChanged() { calculateGrossWeight(); }
    function packWeightChanged() { calculateGrossWeight(); }
    function vehicleNoChanged() {                                                // :3650 — upper case
        var e = $id('txtVehicleNo'), p = e.selectionStart;
        e.value = e.value.toUpperCase();
        try { e.setSelectionRange(p, p); } catch (x) { /* ignore */ }
    }
    function decimalOnly(ev) {                                                   // CommonServices.OnlytextdecimelFunction:2475
        var k = ev.key;
        if (!k || k.length !== 1) return true;
        if (/[0-9]/.test(k)) return true;
        if (k === '.' && ev.target.value.indexOf('.') < 0) return true;
        ev.preventDefault(); return false;
    }
    function numberOnly(ev) {                                                    // OnlytextNumberFunction:2494
        var k = ev.key;
        if (!k || k.length !== 1 || /[0-9]/.test(k)) return true;
        ev.preventDefault(); return false;
    }

    /** AvailableStockGetByItem:3608 — lblBalance; the newest request wins. */
    function availableStockGetByItem() {
        var t = ++stockReq;
        C.getJson(api + '/available-stock' + C.qs({
            itemId: val('CmbItemName'), warehouseId: val('CmbWareHouse'), jobLotId: val('CmbJobLot'),
            cropYear: selText('CmbCropYear').trim(), docDate: $id('DocDate').value,
            packingTypeId: val('CmbPackingType'), uomId: val('CmbPackUom')
        })).then(function (r) {
            if (t !== stockReq) return;
            var v = num(ci(r, 'AvailableStock'));
            $id('lblBalance').textContent = v > 0 ? clr(v) : '0';
        }).catch(function (e) { if (t === stockReq) alert(e.message); });
    }
    function stockInputsChanged() { availableStockGetByItem(); }                  // warehouse / crop / job lot / packing type Text/Leave
    function itemLeave() {                                                       // CmbItemName_Leave:1068 (type 2)
        packUomFromGlobalBind(val('CmbItemName'));
        availableStockGetByItem();
    }
    function packUomTextChanged() { availableStockGetByItem(); }                  // CmbPackUom_TextChanged:3601
    function packUomLeave() { calculateWeight(); }                               // combitempck_Leave:2838
    function supplierLeave() { $id('CmbOrderNo').innerHTML = '<option value=""></option>'; }   // SaleOrderBind:3333 (type 2: Text "", DataSource null)
    function searchModeChanged() { itemNameBind(); }                              // rdSearchByName_CheckedChanged:1095
    function saleTypeLeave() { $id('txtweight').disabled = val('CmbSaleType') === 2; }          // CmbSaleType_Leave:3656

    /* ------------------------------------------------------------------ the detail grid */

    function newRow() {
        return { Id: 0, RefDocumentTypeId: 0, RefDocIdNo: 0, RefDocSubIdNo: 0, SupplierCustomerId: 0, SupplierCustomer: '', RefPartyId: 0,
                 DeliveryScheduleCustomerId: 0, DeliveryScheduleCustomerNo: 0, DeliveryScheduleCustomerDetailId: 0, OrderId: 0, OrderNo: 0,
                 SaleOrderDetailId: 0, ItemId: 0, ItemCode: '', Item: '', ItemUOM: '', ItemUOMId: 0, PUomEquivalent: 0, CropYearId: '',
                 CropYear: '', PackingTypeId: 0, QTY: 0, Weight: 0, LoadQty: 0, LoadWeight: 0, PackingUnit: 0, PackingWeight: 0,
                 GrossWeight: 0, WareHouseId: 0, JobLotId: 0, Remarks: '', Rate: 0, RateUom: 0, DocumentTypeId: 0, AvailableStock: 0,
                 OrderPaymentTermId: null, OrderPaymentTerm: null };
    }

    /** grdCommonSetting:1253 + grdComboBind:1296 — visible columns, captions (ComboBind sets the combo captions). */
    function grdCols(detail) {
        var cols = [
            { k: 'SupplierCustomer', c: 'CustomerName' },
            { k: 'RefPartyId', c: 'Reference Party', list: 'refParties', edit: !detail },
            { k: 'DeliveryScheduleCustomerNo', c: 'Delivery Schedule No', hide: !scheduleExist, t: 'int' },
            { k: 'OrderNo', c: 'Order No', t: 'int', hide: detail && !table2AnyOrder },
            { k: 'ItemCode', c: 'Item Code' },
            { k: 'Item', c: 'Item' },
            { k: 'ItemUOM', c: 'Item UOM' },
            { k: 'CropYear', c: 'Crop Year' }
        ];
        if (detail) cols.push({ k: 'PackingType', c: 'Packing Type' });
        else cols.push({ k: 'PackingTypeId', c: 'Packing Type', list: 'packingTypes', edit: true });
        cols = cols.concat([
            { k: 'LoadQty', c: 'Load Qty', t: 'num', sum: true, edit: !detail },
            { k: 'LoadWeight', c: 'Load Weight', t: 'num', sum: true },
            { k: 'PackingUnit', c: 'Packing Unit', t: 'num', sum: true },
            { k: 'PackingWeight', c: 'Packing Weight', t: 'num', sum: true },
            { k: 'GrossWeight', c: 'Gross Weight', t: 'num', sum: true }
        ]);
        if (detail) cols.push({ k: 'WareHouseName', c: 'Ware House Name' }, { k: 'JobLot', c: 'Job Lot' });
        else cols.push({ k: 'WareHouseId', c: 'WareHouse Name', list: 'warehouses', edit: true }, { k: 'JobLotId', c: 'Job Lot', list: 'jobLots', edit: true });
        cols = cols.concat([
            { k: 'Remarks', c: 'Remarks', edit: !detail },
            { k: 'Rate', c: 'Rate', t: 'rate', hide: !cfg.ShowInfoGrid },
            { k: 'AvailableStock', c: 'Available Stock', t: 'num', sum: true },
            { k: 'OrderPaymentTerm', c: 'Order Payment Term' }
        ]);
        return cols.filter(function (x) { return !x.hide; });
    }
    var table2AnyOrder = false;

    function listName(listKey, v) {
        var r = (look[listKey] || []).find(function (x) { return String(x.Id) === String(v); });
        return r ? r.Name : '';
    }
    function cellText(col, v) {
        if (col.t === 'num') return v === null || v === undefined || v === '' ? '' : fmt(v, 3);
        if (col.t === 'rate') return v === null || v === undefined || v === '' ? '' : fmt(v, 2);
        if (col.list) return (look[col.list] || []).length ? listName(col.list, v) : (v === null || v === undefined ? '' : String(v));
        return v === null || v === undefined ? '' : String(v);
    }

    function renderGrid() {                                                      // grdSettings:1311 — "+" / "X" frozen first
        var g = $id('grd'), cols = grdCols(false);
        g.tHead.innerHTML = '<tr><th style="width:20px">+</th><th style="width:20px">X</th>' +
            cols.map(function (c) { return '<th>' + esc(c.c) + '</th>'; }).join('') + '</tr>';
        g.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr data-i="' + i + '"><td class="c"><button type="button" class="gbtn" data-add="' + i + '">+</button></td>' +
                '<td class="c"><button type="button" class="gbtn" data-del="' + i + '">X</button></td>' +
                cols.map(function (c) {
                    var v = r[c.k];
                    if (c.edit && c.list && (look[c.list] || []).length) {
                        var ops = '<option value=""></option>' + look[c.list].map(function (x) {
                            return '<option value="' + esc(x.Id) + '"' + (String(x.Id) === String(v) ? ' selected' : '') + '>' + esc(x.Name) + '</option>';
                        }).join('');
                        return '<td><select class="win-combo" data-i="' + i + '" data-k="' + c.k + '">' + ops + '</select></td>';
                    }
                    if (c.edit && !c.list) {
                        return '<td><input type="text" class="' + (c.t === 'num' ? 'n' : '') + '" data-i="' + i + '" data-k="' + c.k +
                            '" value="' + esc(c.t === 'num' ? clr(v) : (v || '')) + '"></td>';
                    }
                    return '<td class="' + (c.t === 'num' || c.t === 'rate' ? 'n' : (c.t === 'int' ? 'c' : '')) + '">' + esc(cellText(c, v)) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        g.tFoot.innerHTML = '<tr><td></td><td></td>' + cols.map(function (c) {       // TotalRow: sums of the double columns
            if (!c.sum) return '<td></td>';
            var s = table.reduce(function (a, r) { return a + num(r[c.k]); }, 0);
            return '<td class="n">' + esc(fmt(s, 3)) + '</td>';
        }).join('') + '</tr>';
        g.tBodies[0].querySelectorAll('button[data-add]').forEach(function (b) {
            b.onclick = function (e) { e.stopPropagation(); addDetailRow(parseInt(b.getAttribute('data-add'), 10)); };
        });
        g.tBodies[0].querySelectorAll('button[data-del]').forEach(function (b) {
            b.onclick = function (e) { e.stopPropagation(); deleteDetailRow(parseInt(b.getAttribute('data-del'), 10)); };
        });
        g.tBodies[0].querySelectorAll('[data-k]').forEach(function (el) {
            el.onchange = function () {                                          // one CellUpdated per edit (the re-render blurs the cell)
                el.onchange = null;
                cellUpdated(parseInt(el.getAttribute('data-i'), 10), el.getAttribute('data-k'), el.value);
            };
        });
        g.tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) {
            tr.ondblclick = function (e) {
                if (e.target.closest('input,select,button')) return;
                grdDoubleClick(parseInt(tr.getAttribute('data-i'), 10));
            };
        });
        $id('grdNav').textContent = table.length ? 'Records: ' + table.length : ' ';
    }

    /** grd_CellUpdated:1352. */
    function cellUpdated(i, k, v) {
        var r = table[i];
        if (!r) return;
        if (k === 'LoadQty') r.LoadQty = num(v);
        else if (k === 'Remarks') r.Remarks = v;
        else r[k] = intOf(v);
        if (k === 'LoadQty' || k === 'PackingWeight') {
            var w = num(r.LoadQty) * num(r.PUomEquivalent);
            r.LoadWeight = w;
            r.GrossWeight = w;
            if (r.PackingWeight !== null && r.PackingWeight !== undefined && r.PackingWeight !== '') r.GrossWeight = num(r.PackingWeight) + num(r.LoadWeight);
        }
        renderGrid();
        if (k === 'WareHouseId' || k === 'CropYearId' || k === 'JobLotId' || k === 'PackingTypeId') updateAvailableStockInRow(r).then(renderGrid);
    }

    /** UpdateAvailableStockInGridDetailRow:3477 — @CropYear = the CropYearId cell's text (the id). */
    function updateAvailableStockInRow(r) {
        return C.getJson(api + '/current-stock' + C.qs({
            itemId: intOf(r.ItemId), warehouseId: intOf(r.WareHouseId), jobLotId: intOf(r.JobLotId),
            cropYear: String(r.CropYearId === null || r.CropYearId === undefined ? '' : r.CropYearId),
            docDate: $id('DocDate').value, packingTypeId: intOf(r.PackingTypeId), uomId: intOf(r.ItemUOMId)
        })).then(function (res) { r.AvailableStock = num(ci(res, 'AvailableStock')); })
          .catch(function (e) { alert(e.message); });
    }
    function docDateChanged() {                                                  // DocDate_ValueChanged:3516
        if (!table.length) return;
        Promise.all(table.map(updateAvailableStockInRow)).then(renderGrid);
    }

    function formValidationDetail() {                                            // :1136 — same messages, same order
        function need(ok, msg, focus) { if (!ok) { alert(msg); $id(focus).focus(); } return ok; }
        return need(val('CmbSupplierCustomer') !== 0, 'Customer field is required', 'CmbSupplierCustomer')
            && need(val('CmbItemName') !== 0, 'ItemName field is required', 'CmbItemName')
            && need(val('CmbPackUom') !== 0, 'PackUOM field is required', 'CmbPackUom')
            && need(val('CmbPackingType') !== 0, 'Packingtype field is required', 'CmbPackingType')
            && need(val('CmbCropYear') !== 0, 'CropYear field is required', 'CmbCropYear')
            && need(val('CmbJobLot') !== 0, 'JobLot field is required', 'CmbJobLot')
            && need(num($id('txtqty').value) !== 0, 'ItemQty field is required', 'txtqty')
            && need(num($id('txtweight').value) !== 0, 'Weight field is required', 'txtweight')
            && need(num($id('txtGrossWeight').value) !== 0, 'GrossWeight field is required', 'txtGrossWeight')
            && need(val('CmbWareHouse') !== 0, 'WareHouseName field is required', 'CmbWareHouse');
    }

    function add() {                                                             // btnplus_Click:1207 (type-2 row :1232)
        if (table.length > 0 && scheduleExist) { alert('You can not add manual Record because record against Customer Schedule exist in Grid'); return; }
        if (!formValidationDetail()) return;
        var it = selectedItem() || {}, u = selectedUom() || {};
        var r = newRow();
        r.SupplierCustomerId = val('CmbSupplierCustomer');
        r.SupplierCustomer = selText('CmbSupplierCustomer').trim();
        r.OrderId = val('CmbOrderNo');
        r.OrderNo = intOf(selText('CmbOrderNo').trim());
        r.ItemId = val('CmbItemName');
        r.ItemCode = it.ItemCode || '';
        r.Item = it.ItemName || '';
        r.ItemUOM = selText('CmbPackUom').trim();
        r.ItemUOMId = val('CmbPackUom');
        r.PUomEquivalent = num(u.Equivalent);
        r.CropYearId = $id('CmbCropYear').value;
        r.CropYear = selText('CmbCropYear').trim();
        r.PackingTypeId = val('CmbPackingType');
        r.QTY = num($id('txtqty').value);
        r.Weight = num($id('txtweight').value);
        r.LoadQty = num($id('txtqty').value);
        r.LoadWeight = num($id('txtweight').value);
        r.PackingUnit = num($id('txtPackUnit').value);
        r.PackingWeight = num($id('txtPackWeight').value);
        r.GrossWeight = num($id('txtGrossWeight').value);
        r.WareHouseId = val('CmbWareHouse');
        r.JobLotId = val('CmbJobLot');
        r.Remarks = $id('txtremarksdetail').value.trim();
        table.push(r);
        renderGrid();
        resetDetail();
        $id('CmbDeliveryOrderType').disabled = true;
        $id('CmbSaleType').disabled = false;
    }

    /** grd_DoubleClick:1483 — the StockTransfer branch (:1531). */
    function grdDoubleClick(i) {
        var r = table[i];
        if (!r || scheduleExist) return;
        updateDetailIndex = i;
        refIds.RefDocumentTypeId = String(r.RefDocumentTypeId);
        refIds.RefDocIdNo = String(r.RefDocIdNo);
        refIds.RefDocSubIdNo = String(r.RefDocSubIdNo);
        setValue('CmbSupplierCustomer', r.SupplierCustomerId);
        setValue('CmbItemName', r.ItemId);
        packUomFromGlobalBind(intOf($id('CmbItemName').value));
        setValue('CmbPackUom', r.ItemUOMId);
        setValue('CmbPackingType', r.PackingTypeId);
        setValue('CmbCropYear', r.CropYearId);
        $id('txtBalQty').value = clr(r.QTY);
        $id('txtBalWeight').value = clr(r.Weight);
        setTxt('txtqty', clr(r.LoadQty));
        setTxt('txtweight', clr(r.LoadWeight));
        setTxt('txtPackUnit', clr(r.PackingUnit));
        setTxt('txtPackWeight', clr(r.PackingWeight));
        setTxt('txtGrossWeight', clr(r.GrossWeight));
        setValue('CmbWareHouse', r.WareHouseId);
        setValue('CmbJobLot', r.JobLotId);
        $id('txtremarksdetail').value = r.Remarks === null || r.Remarks === undefined ? '' : String(r.Remarks);
        show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        availableStockGetByItem();
        $id('CmbSupplierCustomer').focus();
    }

    function updateDetail() {                                                    // btnUpdateDetail_Click:1410
        if (table.length > 0 && scheduleExist) { alert('You can not update manual Record because record against Customer Schedule exist in Grid'); return; }
        if (!formValidationDetail()) return;
        var r = table[updateDetailIndex];
        if (!r) { alert('There is no row at position ' + updateDetailIndex + '.'); return; }
        var it = selectedItem() || {}, u = selectedUom() || {};
        r.QTY = num($id('txtqty').value);
        r.Weight = num($id('txtweight').value);
        r.RefDocumentTypeId = intOf(refIds.RefDocumentTypeId);
        r.RefDocIdNo = intOf(refIds.RefDocIdNo);
        r.RefDocSubIdNo = intOf(refIds.RefDocSubIdNo);
        r.SupplierCustomerId = val('CmbSupplierCustomer');
        r.SupplierCustomer = selText('CmbSupplierCustomer');
        r.ItemId = intOf(it.Id);
        r.ItemCode = it.ItemCode || '';
        r.Item = it.ItemName || '';
        r.ItemUOMId = val('CmbPackUom');
        r.ItemUOM = selText('CmbPackUom');
        r.PUomEquivalent = num(u.Equivalent);
        r.PackingTypeId = val('CmbPackingType');
        r.CropYearId = $id('CmbCropYear').value;
        r.CropYear = selText('CmbCropYear');
        r.LoadQty = num($id('txtqty').value);
        r.PackingUnit = num($id('txtPackUnit').value);
        r.PackingWeight = num($id('txtPackWeight').value);
        r.GrossWeight = num($id('txtGrossWeight').value);
        r.LoadWeight = num($id('txtweight').value);
        r.WareHouseId = val('CmbWareHouse');
        r.JobLotId = val('CmbJobLot');
        r.Rate = 0;                                                              // the type-2 item list has no Rate / RateUom / DocumentTypeId
        r.RateUom = '';
        r.DocumentTypeId = 0;
        r.Remarks = $id('txtremarksdetail').value;
        renderGrid();
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        resetDetail();
        bindSupplierName();
    }

    function cancelUpdateDetail() {                                              // :1399 — the fields are not cleared
        refIds = { RefDocumentTypeId: '', RefDocIdNo: '', RefDocSubIdNo: '' };
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        updateDetailIndex = -1;
    }

    function deleteDetailRow(i) {                                                // DeleteDetailRow:1568
        var r = table[i];
        if (!r) return;
        if (updateDetailIndex !== -1) { alert('Reset Detail First'); return; }
        if (intOf(r.Id) > 0) {
            if (!confirm('Are you sure to Delete?')) return;
            removed.push(intOf(r.Id));
        }
        table.splice(i, 1);
        renderGrid();
        if (!table.length) $id('CmbSaleType').disabled = false;
    }

    function addDetailRow(i) {                                                   // AddDetailRow:1618
        var r = table[i];
        if (!r) return;
        var copy = JSON.parse(JSON.stringify(r));
        if (visible('btnupdate') && !$id('btnupdate').disabled) copy.Id = 0;
        table.push(copy);
        renderGrid();
    }

    function resetDetail() {                                                     // ResetDetail:2283
        refIds = { RefDocumentTypeId: '', RefDocIdNo: '', RefDocSubIdNo: '' };
        $id('CmbOrderNo').value = '';
        $id('CmbItemName').value = '';
        $id('CmbPackUom').value = '';
        setTxt('txtqty', '');
        setTxt('txtweight', '');
        setTxt('txtPackUnit', '0');
        setTxt('txtPackWeight', '0');
        setTxt('txtGrossWeight', '0');
        $id('txtBalQty').value = '0';
        $id('txtBalWeight').value = '0';
        $id('CmbPackingType').value = '';
        $id('txtremarksdetail').value = '';
        $id('CmbWareHouse').value = '';
        $id('CmbJobLot').value = '';
        updateDetailIndex = -1;
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        availableStockGetByItem();
        $id('CmbSupplierCustomer').focus();
    }

    /* ------------------------------------------------------------------ the expense grid */

    function addRowInvExpGrid() { dtExpense.push({ Id: 0, SaleOrderId: 0, SaleOrder: 0, SaleOrderCustomerExpId: 0, ItemId: 0, Qty: 0, Remarks: '' }); }   // :1643
    function renderExpense() {                                                   // grdInvExpSettings:1656
        var g = $id('GridExpense');
        var items = look.otherItems || [];
        g.tHead.innerHTML = '<tr><th style="width:20px">+</th><th style="width:20px">X</th><th>Sale Order</th><th style="width:250px">Item</th><th>Qty</th><th>Remarks</th></tr>';
        g.tBodies[0].innerHTML = dtExpense.map(function (r, i) {
            var ops = '<option value=""></option>' + items.map(function (x) {
                return '<option value="' + esc(x.Id) + '"' + (String(x.Id) === String(r.ItemId) ? ' selected' : '') + '>' + esc(x.Name) + '</option>';
            }).join('');
            return '<tr><td class="c"><button type="button" class="gbtn" data-eadd="' + i + '">+</button></td>' +
                '<td class="c"><button type="button" class="gbtn" data-edel="' + i + '">X</button></td>' +
                '<td class="c">' + esc(r.SaleOrder === null || r.SaleOrder === undefined ? '' : r.SaleOrder) + '</td>' +
                '<td><select class="win-combo" data-ei="' + i + '" data-ek="ItemId">' + ops + '</select></td>' +
                '<td><input type="text" class="n" data-ei="' + i + '" data-ek="Qty" value="' + esc(clr(r.Qty)) + '"></td>' +
                '<td><input type="text" data-ei="' + i + '" data-ek="Remarks" value="' + esc(r.Remarks || '') + '"></td></tr>';
        }).join('');
        var q = dtExpense.reduce(function (a, r) { return a + num(r.Qty); }, 0);
        g.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td></td><td class="n">' + esc(fmt(q, 3)) + '</td><td></td></tr>';
        g.tBodies[0].querySelectorAll('button[data-eadd]').forEach(function (b) { b.onclick = function () { addRowInvExpGrid(); renderExpense(); }; });
        g.tBodies[0].querySelectorAll('button[data-edel]').forEach(function (b) {
            b.onclick = function () {                                            // DeleteExpenseDetailRow:1728
                dtExpense.splice(parseInt(b.getAttribute('data-edel'), 10), 1);
                if (!dtExpense.length) addRowInvExpGrid();
                renderExpense();
            };
        });
        g.tBodies[0].querySelectorAll('[data-ek]').forEach(function (el) {
            el.onchange = function () {
                var r = dtExpense[parseInt(el.getAttribute('data-ei'), 10)], k = el.getAttribute('data-ek');
                if (k === 'Qty') el.onchange = null;                             // the re-render below blurs the cell
                if (k === 'ItemId') r.ItemId = intOf(el.value);
                else if (k === 'Qty') r.Qty = num(el.value);
                else r.Remarks = el.value;
                if (k === 'Qty') renderExpense();
            };
        });
        $id('expNav').textContent = 'Records: ' + dtExpense.length;
    }

    /* ------------------------------------------------------------------ form */

    function setButtons(mode) {                                                  // 'save' | 'update' | 'saveas'
        show('btnsave', mode === 'save');
        show('btnupdate', mode === 'update');
        show('btnDelete', mode === 'update');
        show('btnSaveAs', mode === 'saveas');
    }

    function updateDocumentNo() {                                                // UpdateDocumentNoUI(DeliveryOrderGenerateCode(84))
        var t = ++docNoReq;
        C.getJson(api + '/doc-no').then(function (r) { if (t === docNoReq) $id('txtdocno').value = String(ci(r, 'docNo')); })
            .catch(function (e) { alert(e.message); });
    }

    function reset() {                                                           // Reset():2200 — Doc Date is kept
        removed = [];
        $id('CmbSaleType').disabled = false;
        $id('ChkIsStockReservedPerParty').checked = false;
        scheduleExist = false;
        recId = 0;
        saveAsFromId = 0;
        $id('CmbDeliveryOrderType').disabled = true;
        isApproved = false;
        if (look.branches.length) $id('cmbBranchTo').value = String(look.branches[0].Id);   // Rows[0].Activate()
        $id('CmbItemName').value = '';
        $id('CmbPackUom').value = '';
        setTxt('txtqty', '');
        setTxt('txtweight', '');
        $id('CmbPackingType').value = '';
        $id('txtremarks').value = '';
        refIds = { RefDocumentTypeId: '', RefDocIdNo: '', RefDocSubIdNo: '' };
        $id('txtVehicleNo').value = '';
        $id('CmbVehicleType').value = '';
        setTxt('txtPackUnit', '0');
        setTxt('txtPackWeight', '0');
        setTxt('txtGrossWeight', '0');
        $id('txtBalQty').value = '0';
        $id('txtBalWeight').value = '0';
        setButtons('save');
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        table = [];
        renderGrid();
        bindSupplierName();
        dtExpense = [];
        addRowInvExpGrid();
        renderExpense();
        updateDocumentNo();
        if (val('CmbDeliveryOrderType') === 2) branchLabels(look.branches.length > 1);
        setValue('cmbBranchFrom', look.userBranchId);
        availableStockGetByItem();
        $id('DocDate').focus();
    }
    function newClick() { reset(); }                                             // btnnew_Click:2158

    /** GridEXRow values of a ReadByIdDetailId row (FilldtDetailFromListCommonForReadById:2129). */
    function rowFromDetail(d) {
        var r = newRow();
        r.Id = intOf(ci(d, 'Id'));
        r.RefDocumentTypeId = intOf(ci(d, 'RefDocumentTypeId'));
        r.RefDocIdNo = intOf(ci(d, 'RefDocIdNo'));
        r.RefDocSubIdNo = intOf(ci(d, 'RefDocSubIdNo'));
        r.SupplierCustomerId = intOf(ci(d, 'SupplierCustomerId'));
        r.SupplierCustomer = ci(d, 'SupplierCustomer') || '';
        r.RefPartyId = intOf(ci(d, 'RefPartyId'));
        r.DeliveryScheduleCustomerId = intOf(ci(d, 'DeliveryScheduleId'));
        r.DeliveryScheduleCustomerNo = intOf(ci(d, 'DeliveryScheduleCustomerNo'));
        r.DeliveryScheduleCustomerDetailId = intOf(ci(d, 'DeliveryScheduleDetailId'));
        r.OrderId = intOf(ci(d, 'SaleOrderId'));
        var on = ci(d, 'OrderNo');
        r.OrderNo = on === null || on === '' || on === undefined ? '' : intOf(on);
        r.SaleOrderDetailId = intOf(ci(d, 'SaleOrderDetailId'));
        r.ItemId = intOf(ci(d, 'ItemId'));
        r.ItemCode = ci(d, 'ItemCode') || '';
        r.Item = ci(d, 'ItemName') || '';
        r.ItemUOM = ci(d, 'PackUOM') || '';
        r.ItemUOMId = intOf(ci(d, 'PackUomId'));
        r.PUomEquivalent = num(ci(d, 'PUomEquivalent'));
        r.CropYearId = String(intOf(ci(d, 'CropYearId')));
        r.CropYear = ci(d, 'CropYear') || '';
        r.PackingTypeId = intOf(ci(d, 'InvPackingTypeId'));
        r.QTY = num(ci(d, 'DoQty'));
        r.Weight = num(ci(d, 'DoWeight'));
        r.LoadQty = num(ci(d, 'LoadingQty'));
        r.LoadWeight = num(ci(d, 'LoadingWeight'));
        r.PackingUnit = num(ci(d, 'PackingWeight'));
        r.PackingWeight = num(ci(d, 'OuterEbTotal'));
        r.GrossWeight = num(ci(d, 'GrossWeight'));
        r.WareHouseId = intOf(ci(d, 'WarehouseId'));
        r.JobLotId = intOf(ci(d, 'JobLotId'));
        r.Remarks = ci(d, 'LoadingRemarks') || '';
        r.Rate = num(ci(d, 'Rate'));
        r.RateUom = num(ci(d, 'RateUom'));
        r.DocumentTypeId = intOf(ci(d, 'DocumentTypeId'));
        r.AvailableStock = 0;
        r.OrderPaymentTermId = ci(d, 'OrderPaymentTermId');
        r.OrderPaymentTerm = ci(d, 'OrderPaymentTerm');
        r.WareHouseName = ci(d, 'WareHouseName') || '';
        r.JobLot = ci(d, 'JobLotDescription') || '';
        r.PackingType = ci(d, 'PackTypeDesc') || '';
        if (!scheduleExist && r.DeliveryScheduleCustomerDetailId > 0) scheduleExist = true;
        return r;
    }

    /** ReadById:2020 — resolves true when the document was loaded. */
    function readById(id) {
        reset();
        recId = id;
        return C.getJson(api + '/' + id).then(function (res) {
            var h = res && res.header, det = (res && res.details) || [];
            if (!h || !det.length) return false;
            ++docNoReq;                                                          // the Reset() number request is stale now
            tab('tabForm');
            setButtons('update');
            $id('txtdocno').value = String(intOf(ci(h, 'DocNo')));
            $id('DocDate').value = C.isoDay(ci(h, 'DocDate'));
            setValue('cmbBranchFrom', intOf(ci(h, 'BranchesId')));
            setValue('cmbBranchTo', intOf(ci(h, 'ToBranchId')));
            setText('CmbDeliveryOrderType', ci(h, 'DeliveryOrderType') || '');
            deliveryOrderTypeLeave();
            $id('CmbDeliveryOrderType').disabled = true;
            if (intOf(ci(h, 'SaleTypeId')) > 0) {
                setValue('CmbSaleType', intOf(ci(h, 'SaleTypeId')));
                $id('CmbSaleType').disabled = false;
            }
            setText('CmbVehicleType', ci(h, 'VehicleType') || '');
            $id('txtVehicleNo').value = ci(h, 'VehicleNo') || '';
            $id('txtremarks').value = ci(h, 'LoadingInstructions') || '';
            var ap = ci(h, 'IsApproved');
            isApproved = ap === true || ap === 1 || String(ap).toLowerCase() === 'true';
            var sr = ci(h, 'IsStockReserved');
            $id('ChkIsStockReservedPerParty').checked = sr === true || sr === 1 || String(sr).toLowerCase() === 'true';
            table = det.map(rowFromDetail);
            renderGrid();
            dtExpense = ((res && res.expenses) || []).map(function (e) {
                return { Id: intOf(ci(e, 'Id')), SaleOrderId: intOf(ci(e, 'SaleOrderId')), SaleOrder: intOf(ci(e, 'SaleOrder')),
                         SaleOrderCustomerExpId: intOf(ci(e, 'SaleOrderCustomerExpId')), ItemId: intOf(ci(e, 'ItemId')),
                         Qty: num(ci(e, 'Qty')), Remarks: ci(e, 'Remarks') || '' };
            });
            if (!dtExpense.length) addRowInvExpGrid();
            renderExpense();
            deliveryOrderTypeLeave();
            return true;
        }).catch(function (e) { alert(e.message); return false; });
    }

    /* ------------------------------------------------------------------ save / update / delete / print */

    function formValidation() {                                                  // FormValidation():1107
        if (!val('cmbBranchFrom')) { alert('branch field is required'); $id('cmbBranchFrom').focus(); return false; }
        var dn = $id('txtdocno').value.trim();
        if (dn === '' || dn === '0') { alert('DocNo Field is Required'); return false; }
        if (!val('CmbDeliveryOrderType')) { alert('DeliveryOrderType Field is Required'); return false; }
        if (!val('CmbSaleType')) { alert('Sale Type Field is Required'); $id('CmbSaleType').focus(); return false; }
        return true;
    }
    function validatePaymentTypes() {                                            // ValidatePaymentTypes:1746
        var msgs = [];
        table.forEach(function (r) {
            var t = intOf(r.OrderPaymentTermId);
            if (t === 1 || t === 3) {
                var m = 'Order No ' + intOf(r.OrderNo) + ": Payment Term is '" + (r.OrderPaymentTerm || '') + "'.";
                if (msgs.indexOf(m) < 0) msgs.push(m);
            }
        });
        return !msgs.length || confirm(msgs.join('\n') + '\n\nAre you sure you want to continue?');
    }
    function payload(confirmed) {
        return {
            Id: recId, SaveAsFromId: recId === 0 ? saveAsFromId : 0, DocDate: $id('DocDate').value,
            DoTypeId: val('CmbDeliveryOrderType'), SaleTypeId: val('CmbSaleType'),
            VehicleType: selText('CmbVehicleType'), VehicleNo: $id('txtVehicleNo').value.trim(),
            BranchFromId: val('cmbBranchFrom'), BranchToId: val('cmbBranchTo'), Remarks: $id('txtremarks').value,
            IsStockReserved: visible('lblChkIsStockReservedPerParty') && $id('ChkIsStockReservedPerParty').checked,
            ConfirmWarning: !!confirmed,
            rows: table.map(function (r) {
                return { Id: intOf(r.Id), SupplierCustomerId: intOf(r.SupplierCustomerId), RefPartyId: intOf(r.RefPartyId),
                         ItemId: intOf(r.ItemId), ItemUOMId: intOf(r.ItemUOMId), CropYearId: intOf(r.CropYearId),
                         PackingTypeId: intOf(r.PackingTypeId), LoadQty: num(r.LoadQty), LoadWeight: num(r.LoadWeight),
                         PackingUnit: num(r.PackingUnit), PackingWeight: num(r.PackingWeight), GrossWeight: num(r.GrossWeight),
                         WareHouseId: intOf(r.WareHouseId), JobLotId: intOf(r.JobLotId),
                         Remarks: r.Remarks === null || r.Remarks === undefined ? '' : String(r.Remarks) };
            }),
            removedIds: recId > 0 ? removed.slice() : [],
            expenses: dtExpense.map(function (e) {
                return { Id: intOf(e.Id), SaleOrderId: intOf(e.SaleOrderId), SaleOrderCustomerExpId: intOf(e.SaleOrderCustomerExpId),
                         ItemId: intOf(e.ItemId), Qty: num(e.Qty), Remarks: e.Remarks === null || e.Remarks === undefined ? '' : String(e.Remarks) };
            })
        };
    }
    function insert() {                                                          // Insert():1768
        if (!table.length) { alert('Grid Record Not Found'); return; }
        if (!formValidation()) return;
        if (!val('cmbBranchTo')) { alert("'Branch To' Field is Required..."); $id('cmbBranchTo').focus(); return; }
        if (val('cmbBranchFrom') === val('cmbBranchTo')) { alert(" 'Branch From' Cannot Be Equal To 'Branch To' ..."); return; }
        if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;     // FormHelper.ConfirmAction
        var totalPackingWeight = table.reduce(function (a, r) { return a + num(r.PackingWeight); }, 0);
        if (cfg.WarningMessageOnDOForPackingWeight && totalPackingWeight === 0 &&
            !confirm('There is no Packing Weight added on any row. Are you Sure To Proceed?')) return;
        if (cfg.WarningMsgForPaymentTermOtherThanCreditOnDO && !validatePaymentTypes()) return;
        post(false);
    }
    function post(confirmed) {
        C.postJson(api + '/save', payload(confirmed)).then(function (res) {
            if (res && res.confirm) {                                            // DeliveryOrderStackWarningMessage (D4)
                if (confirm(res.message)) post(true);
                return;
            }
            alert(res.message);
            var id = intOf(res.id), docNo = intOf(res.docNo);
            reset();
            if (visible('btnsave') && !$id('btnsave').disabled) openLinkedFormOnInsert(docNo);   // :1935
            if ($id('ChkPrint').checked) C.printSlip(api + '/' + id + '/slip', '262 - Delivery Order Slip');
        }).catch(function (e) { alert(e.message); });
    }
    function save() { recId = 0; insert(); }                                     // btnsave_Click:1950
    function saveAs() { recId = 0; insert(); }                                   // btnSaveAs_Click:1963
    function update() {                                                          // btnupdate_Click:1976
        if (recId === 0) { alert('Record Not Update because RecId Not Found'); return; }
        insert();
    }
    function del() {                                                             // btnDelete_Click:1992
        if (recId === 0) { alert('Record Id Not Found'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        if (isApproved) { alert('Record has been approved'); return; }
        if (!visible('btnDelete') || $id('btnDelete').disabled) return;
        C.postJson(api + '/' + recId + '/delete', {}).then(function (res) {
            alert(res.message);
            reset();
        }).catch(function (e) { alert(e.message); });
    }
    function print() { C.printSlip(api + '/' + recId + '/slip', '262 - Delivery Order Slip'); }   // btnprint_Click:2898

    function openLinkedFormOnInsert(docNo) {                                     // OpenLinkedFormOnInsert:3276
        if ($id('RadGpOutwardToOpenOnInsert').checked) openOutwardGatePass(docNo);
    }
    /** labelGpOutwardForm_Click:3323 / GpOutwardForm:3291 — the Sale module's Outward Gate Pass page. */
    function openOutwardGatePass(docNo) {
        window.open('/sale/outward-gate-pass' + (docNo > 0 ? C.qs({ gpBasedOn: 84, orderNo: docNo }) : ''), '_blank');
    }
    function loadCustomerSchedule() {                                            // BtnLoadCustomerSchedule_Click:4076
        if (selText('CmbDeliveryOrderType').trim().toLowerCase() !== 'local') {
            alert("Please select 'Local' as Delivery Order Type.");
        }
    }
    function refresh() {                                                         // btnRefresh_Click:2168
        C.getJson(api + '/lookups').then(function (l) {
            applyLookups(l);
            configurations();
            branchNameBind();
            vehicleTypesBind();
            bindSupplierName();
            warehouseBind();
            itemNameBind();
            packUomFromGlobalBind(val('CmbItemName'));
            cropBind();
            jobLotBind();
            packingTypeBind();
            configDefaults();
            renderGrid();
            renderExpense();
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ tabs */

    function tab(name) {                                                         // tabControl1_SelectedIndexChanged:2333
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === name); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === name); });
        if (name === 'tabHistory') $id('FromDateHistory').focus(); else $id('DocDate').focus();
    }
    function tab2(name) {                                                        // DetailGridPanel
        document.querySelectorAll('.do-tab2').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab2') === name); });
        document.querySelectorAll('.do-tab2-page').forEach(function (p) { p.classList.toggle('is-active', p.id === name); });
    }

    /* ------------------------------------------------------------------ history */

    function dateMode() {
        if ($id('rdentrydate').checked) return 'entry';
        if ($id('rdmodifydate').checked) return 'modify';
        if ($id('rdapproveddate').checked) return 'approved';
        return 'doc';
    }
    var HIST_COLS = [                                                            // dthistoryfill:2463 (Id hidden)
        { k: 'DoType' }, { k: 'DocDate', t: 'date' }, { k: 'DocNo', t: 'link' }, { k: 'VehicleType' }, { k: 'VehicleNo' },
        { k: 'ApprovalStatus' }, { k: 'GpDate', t: 'dmy' }, { k: 'GpNo', t: 'int' }, { k: 'GpStatus' }, { k: 'GdnNo', t: 'int' },
        { k: 'FactoryWeight', t: 'num' }, { k: 'EntryUser' }, { k: 'EntryDate', t: 'dt' }, { k: 'ModifyUser' }, { k: 'ModifyDate', t: 'dt' },
        { k: 'ApprovedUser' }, { k: 'ApprovedDate', t: 'dt' }, { k: 'OrderStatus' }, { k: 'Remarks' }, { k: 'NoOfAttachments', t: 'int' }
    ];
    function showHistory() {                                                     // gridhistoryfill:2388
        C.getJson(api + '/history' + C.qs({
            dateMode: dateMode(),
            fromDate: $id('chkFromDateHistory').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDateHistory').checked ? $id('ToDateHistory').value : '',
            docNoFrom: intOf($id('FromDocNoHistory').value),
            docNoTo: intOf($id('ToDocNoHistory').value),
            supplierCustomerId: val('CmbCustomerHistory')
        })).then(function (rows) {
            rows = rows || [];
            if (!rows.length) { dthistoryfill = null; renderHistory(); return; }   // grdhistory.ClearStructure()
            var seen = {};
            dthistoryfill = [];
            rows.forEach(function (r) {
                var id = intOf(ci(r, 'Id'));
                if (seen[id]) return;
                seen[id] = true;
                dthistoryfill.push({
                    Id: id, DoType: ci(r, 'DeliveryOrderType'), DocDate: dt1900(ci(r, 'DocDate')), DocNo: intOf(ci(r, 'DocNo')),
                    VehicleType: ci(r, 'VehicleType'), VehicleNo: ci(r, 'VehicleNo'), ApprovalStatus: ci(r, 'ApprovalStatus'),
                    GpDate: dt1900(ci(r, 'GpDate')), GpNo: intOf(ci(r, 'GpNo')), GpStatus: ci(r, 'GpStatus'), GdnNo: intOf(ci(r, 'GdnNo')),
                    FactoryWeight: num(ci(r, 'FactoryWeight')), EntryUser: ci(r, 'EntryUser'), EntryDate: dt1900(ci(r, 'EntryDate')),
                    ModifyUser: ci(r, 'ModifyUser'), ModifyDate: dt1900(ci(r, 'ModifyDate')), ApprovedUser: ci(r, 'ApprovedUser'),
                    ApprovedDate: dt1900(ci(r, 'ApprovedDate')), OrderStatus: ci(r, 'OrderStatus'), Remarks: ci(r, 'HeaderRemarks'),
                    NoOfAttachments: ci(r, 'NoOfAttachments')
                });
            });
            renderHistory();
        }).catch(function (e) { alert(e.message); });
    }
    function histCell(c, v) {
        if (c.t === 'date') return C.gridDate(v);
        if (c.t === 'dmy') return dmy(v);
        if (c.t === 'dt') return C.gridDateTime(v);
        if (c.t === 'num') return fmt(v, 2);
        return v === null || v === undefined ? '' : String(v);
    }
    function renderHistory() {                                                   // HistoryGridSettings:2512
        var g = $id('grdhistory');
        histSel = -1;
        clearHistoryDetail();
        if (!dthistoryfill) { g.tHead.innerHTML = ''; g.tBodies[0].innerHTML = ''; g.tFoot.innerHTML = ''; $id('histNav').innerHTML = '&nbsp;'; return; }
        g.tHead.innerHTML = '<tr><th style="width:40px">Edit</th><th style="width:40px">Print</th><th style="width:70px">Print 264</th><th style="width:65px">SaveAs</th>' +
            HIST_COLS.map(function (c) { return '<th>' + esc(cap(c.k)) + '</th>'; }).join('') + '<th style="width:110px">Add Attachment</th></tr>';
        g.tBodies[0].innerHTML = dthistoryfill.map(function (r, i) {
            return '<tr data-h="' + i + '">' +
                '<td class="c"><button type="button" class="gbtn" data-act="edit">Edit</button></td>' +
                '<td class="c"><button type="button" class="gbtn" data-act="print">Print</button></td>' +
                '<td class="c"><button type="button" class="gbtn" data-act="print264">Print 264</button></td>' +
                '<td class="c"><button type="button" class="gbtn" data-act="saveas">SaveAs</button></td>' +
                HIST_COLS.map(function (c) {
                    var v = r[c.k];
                    if (c.t === 'link') return '<td class="c"><span class="glink" data-act="open">' + esc(v) + '</span></td>';
                    return '<td class="' + (c.t === 'num' ? 'n' : (c.t === 'int' ? 'c' : '')) + '">' + esc(histCell(c, v)) + '</td>';
                }).join('') +
                '<td class="c"><button type="button" class="gbtn" disabled title="Attachments are not ported to the web">Add Attachment</button></td></tr>';
        }).join('');
        var fw = dthistoryfill.reduce(function (a, r) { return a + num(r.FactoryWeight); }, 0);
        g.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td></td>' + HIST_COLS.map(function (c) {
            return c.t === 'num' ? '<td class="n">' + esc(fmt(fw, 2)) + '</td>' : '<td></td>';
        }).join('') + '<td></td></tr>';
        g.tBodies[0].querySelectorAll('tr[data-h]').forEach(function (tr) {
            var i = parseInt(tr.getAttribute('data-h'), 10);
            tr.onclick = function (e) {
                var a = e.target.closest('[data-act]');
                selectHistory(i);
                if (!a) return;
                historyAction(i, a.getAttribute('data-act'));
            };
            tr.ondblclick = function (e) { if (!e.target.closest('button')) historyAction(i, 'open'); };
        });
        $id('histNav').textContent = 'Records: ' + dthistoryfill.length;
    }
    function historyAction(i, act) {                                             // grdhistory_ColumnButtonClick:2578 / _DoubleClick:2554
        var r = dthistoryfill[i];
        if (!r) return;
        if (act === 'saveas') { historySaveAs(r.Id); return; }
        if (act === 'print') { C.printSlip(api + '/' + r.Id + '/slip', '262 - Delivery Order Slip'); return; }
        if (act === 'print264') { C.printSlip(api + '/' + r.Id + '/slip264', '264 - Delivery Challan By Delivery Order'); return; }
        if (act === 'edit' || act === 'open') {
            var status = r.OrderStatus === null || r.OrderStatus === undefined ? '' : String(r.OrderStatus);
            if (status !== 'Open') { alert("Sorry You can't Update this record because Order_Status is:" + status); return; }
            readById(r.Id);
        }
    }
    function historySaveAs(id) {                                                 // grdhistory_Saveas:2626
        readById(id).then(function (loaded) {
            setButtons('saveas');
            table.forEach(function (r) { r.Id = 0; });
            renderGrid();
            if (loaded) saveAsFromId = id;
        });
    }
    function selectHistory(i) {                                                  // grdhistory_SelectionChanged:2652
        if (histSel === i) return;
        histSel = i;
        $id('grdhistory').tBodies[0].querySelectorAll('tr[data-h]').forEach(function (tr) {
            tr.classList.toggle('sel', parseInt(tr.getAttribute('data-h'), 10) === i);
        });
        var r = dthistoryfill && dthistoryfill[i];
        if (!r) { clearHistoryDetail(); return; }
        C.getJson(api + '/' + r.Id).then(function (res) {                        // DetailGridBind:2674
            if (histSel !== i) return;
            if (!res || !res.header) { clearHistoryDetail(); return; }
            renderHistoryDetail(((res && res.details) || []).map(rowFromDetail));
        }).catch(function () { if (histSel === i) clearHistoryDetail(); });
    }
    function clearHistoryDetail() {
        var g = $id('GrdHistoryDetail');
        g.tHead.innerHTML = ''; g.tBodies[0].innerHTML = ''; g.tFoot.innerHTML = '';
    }
    function renderHistoryDetail(rows) {                                         // grdHistoryDetailSettings:2699
        var g = $id('GrdHistoryDetail');
        table2AnyOrder = rows.some(function (r) { return intOf(r.OrderId) > 0; });
        var cols = grdCols(true);
        g.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c.c) + '</th>'; }).join('') + '</tr>';
        g.tBodies[0].innerHTML = rows.map(function (r) {
            return '<tr>' + cols.map(function (c) {
                return '<td class="' + (c.t === 'num' || c.t === 'rate' ? 'n' : (c.t === 'int' ? 'c' : '')) + '">' + esc(cellText(c, r[c.k])) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        g.tFoot.innerHTML = '<tr>' + cols.map(function (c) {
            if (!c.sum) return '<td></td>';
            return '<td class="n">' + esc(fmt(rows.reduce(function (a, r) { return a + num(r[c.k]); }, 0), 3)) + '</td>';
        }).join('') + '</tr>';
    }
    function resetHistory() {                                                    // btnResetHistory_Click:2345 — 3 days, not the configuration
        var t = C.today();
        $id('FromDateHistory').value = addDays(t, -3);
        $id('ToDateHistory').value = t;
        $id('FromDocNoHistory').value = '';
        $id('ToDocNoHistory').value = '';
        $id('CmbCustomerHistory').value = '';
        dthistoryfill = null;
        renderHistory();
        $id('drdocdate').checked = true;
    }
    function refreshHistory() {                                                  // btnRefreshHistory_Click:2364
        C.getJson(api + '/history-customers').then(function (rows) {
            look.historyCustomers = rows || [];
            historyComboBind();
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ load */

    function applyLookups(l) {
        ['rights', 'branches', 'vehicleTypes', 'suppliers', 'warehouses', 'items', 'uoms', 'cropYears', 'jobLots', 'packingTypes',
         'refParties', 'otherItems', 'historyCustomers'].forEach(function (k) { look[k] = l[k] || (k === 'rights' ? {} : []); });
        look.isCustomerPortal = !!l.isCustomerPortal;
        look.userBranchId = intOf(l.userBranchId);
        cfg = l.config || {};
    }

    function init() {                                                            // InitializeComponentMethod:520
        $id('DocDate').value = C.today();
        dtExpense = []; addRowInvExpGrid();
        C.getJson(api + '/lookups').then(function (l) {
            applyLookups(l);
            var rt = look.rights || {};
            $id('btnsave').disabled = !rt.save;
            $id('btnSaveAs').disabled = !rt.save;
            $id('btnupdate').disabled = !rt.update;
            $id('btnDelete').disabled = !rt['delete'];
            $id('ChkPrint').checked = !!rt.print;
            $id('btnprint').classList.toggle('checked', !!rt.print);
            configurations();
            if (cfg.ItemSearchByCode) $id('rdSearchByCode').checked = true; else $id('rdSearchByName').checked = true;
            if (recId === 0) $id('txtdocno').value = String(intOf(l.docNo));
            bindCombo('CmbDeliveryOrderType', DO_TYPES, { prev: null, activate: true });   // BindDDLNew + Rows[0].Activate()
            branchNameBind();
            vehicleTypesBind();
            saleTypeBind();
            bindSupplierName();
            warehouseBind();
            cropBind();
            jobLotBind();
            packingTypeBind();
            configDefaults();
            renderGrid();
            renderExpense();
            branchLabels(look.branches.length > 1 && val('CmbDeliveryOrderType') === 2);
            setValue('CmbDeliveryOrderType', 2);                                 // ScreenName "DeliveryOrderTransfer" (:629)
            $id('CmbDeliveryOrderType').disabled = true;
            deliveryOrderTypeLeave();
            setButtons('save');
            show('btnplus', true);
            $id('cmbBranchFrom').disabled = !(look.branches.length > 1 && val('CmbDeliveryOrderType') === 1);
            show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            setValue('cmbBranchFrom', look.userBranchId);
            setValue('CmbSaleType', SALE_TYPES[0].Id);                           // CmbSaleType.Rows[0].Activate()
            saleTypeLeave();
            historyComboBind();
            var days = intOf(cfg.DefaultDaysToLessFromHistoryFromDate);
            $id('FromDateHistory').value = addDays(C.today(), days > 0 ? -days : -3);
            $id('ToDateHistory').value = C.today();
            availableStockGetByItem();                                           // the combos' TextChanged during the binds
            $id('DocDate').focus();
        }).catch(function (e) { alert(e.message); });
    }

    /* the Leave events the desktop hangs on these combos */
    function wireLeaves() {
        $id('CmbItemName').addEventListener('blur', itemLeave);
        $id('CmbPackUom').addEventListener('blur', packUomLeave);
        $id('CmbSaleType').addEventListener('blur', saleTypeLeave);
        $id('CmbSupplierCustomer').addEventListener('blur', supplierLeave);
    }

    window.DoTransfer = {
        newClick: newClick, refresh: refresh, save: save, saveAs: saveAs, update: update, del: del, print: print,
        add: add, updateDetail: updateDetail, cancelUpdateDetail: cancelUpdateDetail,
        qtyChanged: qtyChanged, weightChanged: weightChanged, packUnitChanged: packUnitChanged, packWeightChanged: packWeightChanged,
        vehicleNoChanged: vehicleNoChanged, decimalOnly: decimalOnly, numberOnly: numberOnly,
        itemLeave: itemLeave, packUomLeave: function () { packUomTextChanged(); packUomLeave(); },
        stockInputsChanged: stockInputsChanged, supplierLeave: supplierLeave, searchModeChanged: searchModeChanged,
        saleTypeLeave: saleTypeLeave, docDateChanged: docDateChanged,
        openOutwardGatePass: openOutwardGatePass, loadCustomerSchedule: loadCustomerSchedule,
        tab: tab, tab2: tab2, showHistory: showHistory, resetHistory: resetHistory, refreshHistory: refreshHistory
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', function () { wireLeaves(); init(); });
    else { wireLeaves(); init(); }
})();
