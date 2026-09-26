/* ============================================================================================
 * Screen 324 "Grn Store" — Architecture.WinApp.StoreManagement.GrnStore (ScreenName "GrnStore"),
 * DocumentTypeId 48. Store Purchase, ModuleId 67.
 *
 * Rows only ever come from one of three loaders, never mixed (the Load buttons' guards):
 *   Load PO              frmPendingPurchaseOrderStoreLoader  → LoadInGridDetailFromPO:1937         (base 1)
 *   Load Demand          frmPendingPurchaseDemand            → LoadInGridDetailForDemand:2084      (base 2)
 *   Load Delivery Challan frmPendingDeliveryChallanLoader    → LoadInGridDetailFromDeliveryChallan:2225 (base 3)
 *
 *   grdSettings:725           visible columns by BaseDocumentTypeId; ThisQty / Remarks / Item Condition editable
 *   RowBalanceStockQtyUpdate  AvailableStockQty = QtyInHand at DocDate (after a load, a cell edit, a pick, a date change)
 *   EnabledDisabledFeilds:1892 Supplier only for base 2; Gate Pass / Vehicle No / Bilty No only without a gate pass
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, ci = C.ci, num = C.num, intOf = C.intOf;
    var api = '/api/store/grn-store';
    var MONTHS = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    var look = { rights: {}, suppliers: [], transporters: [], vehicleTypes: [], itemConditions: [], racks: [], historyFilters: {} };
    var Id = 0, GPID = 0, BaseDocumentTypeId = 0;
    var dtdetail = [];
    var loaded = false;
    var historyRows = [], registerRaw = [];

    /* ------------------------------------------------------------------ formatting */

    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function daysAgo(n) { var d = new Date(); d.setDate(d.getDate() - n); return iso(d); }
    /** "dd-MMM-yy" */
    function dmy(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(v || ''));
        return m ? m[3] + '-' + MONTHS[parseInt(m[2], 10) - 1] + '-' + m[1].substring(2) : String(v || '');
    }
    /** "dd-MMM-yy hh:mm tt" */
    function dmyt(v) {
        var m = /^(\d{4})-(\d{2})-(\d{2})[T ](\d{2}):(\d{2})/.exec(String(v || ''));
        if (!m) return dmy(v);
        var h = parseInt(m[4], 10), ap = h >= 12 ? 'PM' : 'AM';
        h = h % 12; if (h === 0) h = 12;
        return dmy(v) + ' ' + String(h).padStart(2, '0') + ':' + m[5] + ' ' + ap;
    }
    function n2(v) { return num(v).toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 }); }
    function cap(k) { return String(k).replace(/([a-z0-9])([A-Z])/g, '$1 $2'); }

    /* ------------------------------------------------------------------ form grid */

    /* dtdetail's visible columns in grid order — ItemConditionId moved behind UomEquivalent (GridComboBind:717). */
    var GRID_COLS = [
        { k: 'OrderNo' }, { k: 'DemandNo' }, { k: 'PurchasePreBillNo' }, { k: 'DeliveryChallanNo' },
        { k: 'ItemCode' }, { k: 'ItemName' }, { k: 'WarehouseName', pick: 'wh' }, { k: 'RackName', pick: 'rack' },
        { k: 'UOM' }, { k: 'ItemConditionId', c: 'Item Condition', combo: true },
        { k: 'TotalOrderQty', n: 1 }, { k: 'UsedOrderQty', n: 1 }, { k: 'BalOrderQty', n: 1 },
        { k: 'TotalDemandQty', n: 1, c: 'Approved Demand Qty' }, { k: 'UsedDemandQty', n: 1 }, { k: 'BalDemandQty', n: 1 },
        { k: 'PurchasePreBillQty', n: 1 }, { k: 'TotalDeliveryChallanQty', n: 1 }, { k: 'UsedDeliveryChallanQty', n: 1 },
        { k: 'BalDeliveryChallanQty', n: 1 }, { k: 'ThisQty', n: 1, c: 'Received Qty', edit: true },
        { k: 'AvailableStockQty', n: 1 }, { k: 'Remarks', edit: true }
    ];
    /** grdSettings:729-752 */
    function hiddenFor(base) {
        switch (base) {
            case 0: return ['OrderNo', 'DemandNo', 'PurchasePreBillNo', 'DeliveryChallanNo', 'TotalOrderQty', 'UsedOrderQty', 'BalOrderQty', 'TotalDemandQty', 'UsedDemandQty', 'BalDemandQty',
                'PurchasePreBillQty', 'TotalDeliveryChallanQty', 'UsedDeliveryChallanQty', 'BalDeliveryChallanQty'];
            case 1: return ['DemandNo', 'DeliveryChallanNo', 'TotalDemandQty', 'UsedDemandQty', 'BalDemandQty', 'TotalDeliveryChallanQty', 'UsedDeliveryChallanQty', 'BalDeliveryChallanQty', 'PurchasePreBillNo', 'PurchasePreBillQty'];
            case 2: return ['OrderNo', 'DeliveryChallanNo', 'TotalOrderQty', 'UsedOrderQty', 'BalOrderQty', 'TotalDeliveryChallanQty', 'UsedDeliveryChallanQty', 'BalDeliveryChallanQty', 'PurchasePreBillNo', 'PurchasePreBillQty'];
            case 3: return ['OrderNo', 'TotalOrderQty', 'UsedOrderQty', 'BalOrderQty', 'UsedDemandQty', 'BalDemandQty'];
            default: return [];
        }
    }
    function visibleCols(base) { var h = hiddenFor(base); return GRID_COLS.filter(function (c) { return h.indexOf(c.k) < 0; }); }

    function conditionOptions(sel) {
        var h = '<option value="0"></option>';
        (look.itemConditions || []).forEach(function (c) {
            h += '<option value="' + c.Id + '"' + (intOf(sel) === c.Id ? ' selected' : '') + '>' + esc(c.Description) + '</option>';
        });
        return h;
    }

    function render() {
        var cols = visibleCols(BaseDocumentTypeId);
        var t = $id('grd');
        t.tHead.innerHTML = '<tr><th></th>' + cols.map(function (c) { return '<th>' + esc(c.c || cap(c.k)) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = dtdetail.map(function (r, i) {
            return '<tr><td><button type="button" class="cx-x" title="Delete" onclick="GrnStore.delRow(' + i + ')">X</button></td>' + cols.map(function (c) {
                var v = r[c.k];
                if (c.combo) return '<td><select class="win-combo" onchange="GrnStore.cell(' + i + ',\'' + c.k + '\',this.value)">' + conditionOptions(v) + '</select></td>';
                if (c.edit) return '<td><input type="text" class="' + (c.n ? 'num' : '') + '" value="' + esc(c.n ? num(v) : (v || '')) +
                    '" onchange="GrnStore.cell(' + i + ',\'' + c.k + '\',this.value)"></td>';
                if (c.pick) return '<td class="pick" title="Pick (F1)" onclick="GrnStore.pick(' + i + ',\'' + c.pick + '\')">' + esc(v || '') + '</td>';
                return '<td class="' + (c.n ? 'num' : '') + '">' + esc(c.n ? n2(v) : (v === null || v === undefined ? '' : v)) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        $id('lblBase').textContent = BaseDocumentTypeId === 1 ? 'Purchase Order' : BaseDocumentTypeId === 2 ? 'Purchase Demand' : BaseDocumentTypeId === 3 ? 'Delivery Challan' : '';
    }

    /** grd_CellUpdated:939 → RowBalanceStockQtyUpdate for the current row. */
    function cell(i, key, value) {
        var r = dtdetail[i]; if (!r) return;
        if (key === 'ThisQty') r.ThisQty = num(value);
        else if (key === 'ItemConditionId') r.ItemConditionId = intOf(value);
        else r[key] = value;
        stockUpdate([i]);
    }
    /** grd_ColumnButtonClick:788 — the "X" column deletes the row. */
    function delRow(i) { dtdetail.splice(i, 1); render(); }

    /** grd_KeyDown:806, F1 on a Warehouse or Rack cell — GrdPopUp from racksWithWarehouseAndItems. */
    function pick(i, what) {
        var r = dtdetail[i]; if (!r) return;
        var itemId = intOf(r.ItemId), whId = intOf(r.WarehouseId), rackId = intOf(r.RackId);
        var racks = look.racks || [];
        if (what === 'wh') {
            var whs = C.distinct(racks.filter(function (x) { return x.ItemId === itemId; }), 'WarehouseId')
                .map(function (w) { return { Id: w.WarehouseId, WareHouseName: w.WareHouseName }; });
            C.pickFrom('Warehouse', whs, [{ key: 'WareHouseName', caption: 'WareHouseName' }]).then(function (sel) {
                /* ReturnId / ReturnName are 0 / "" when the popup is closed without a pick (:852-855). */
                var selectedId = sel ? intOf(sel.Id) : 0;
                r.WarehouseId = selectedId; r.WarehouseName = sel ? sel.WareHouseName : '';
                var rs = racks.filter(function (x) { return x.ItemId === itemId && x.WarehouseId === selectedId; });
                if (rackId > 0) {
                    if (!rs.some(function (x) { return x.Id === rackId; })) {
                        if (rs.length === 1) { r.RackId = rs[0].Id; r.RackName = rs[0].RackName; }
                        else { r.RackId = 0; r.RackName = ''; }
                    }
                } else if (rs.length === 1) { r.RackId = rs[0].Id; r.RackName = rs[0].RackName; }
                render(); stockUpdate([i]);
            });
        } else {
            var list = C.distinct(racks.filter(function (x) { return x.ItemId === itemId && (whId === 0 || x.WarehouseId === whId); }), 'Id');
            if (!list.length) return;                                            // :894
            C.pickFrom('Rack', list, [{ key: 'RackName', caption: 'RackName' }, { key: 'WareHouseName', caption: 'WarehouseName' }]).then(function (sel) {
                var selectedId = sel ? intOf(sel.Id) : 0;
                r.RackId = selectedId; r.RackName = sel ? sel.RackName : '';
                if (whId === 0) {
                    var s = list.find(function (x) { return x.Id === selectedId; });
                    if (s) { r.WarehouseId = s.WarehouseId; r.WarehouseName = s.WareHouseName; }
                }
                render(); stockUpdate([i]);
            });
        }
    }

    /** RowBalanceStockQtyUpdate:956 for the given rows (all rows when idx is omitted). */
    function stockUpdate(idx) {
        var list = idx || dtdetail.map(function (_, i) { return i; });
        if (!list.length) return Promise.resolve();
        var snapshot = list.map(function (i) { return dtdetail[i]; });
        return C.postJson(api + '/stock', {
            docDate: $id('DocDate').value, recId: Id,
            rows: snapshot.map(function (r) { return { ItemId: r.ItemId, ItemConditionId: r.ItemConditionId, WarehouseId: r.WarehouseId, RackId: r.RackId }; })
        }).then(function (q) {
            snapshot.forEach(function (r, k) { if (r) r.AvailableStockQty = num(q[k]); });
            render();
        }).catch(function (e) { alert(e.message); });
    }
    function docDateChanged() { stockUpdate(); }                                 // DocDate_ValueChanged:987

    /* ------------------------------------------------------------------ header state */

    function transporterBind() {                                                // TransporterAcFill:645 (default row)
        var el = $id('CmbTransporter'), keep = el.value;
        var h = '<option value=""></option>';
        (look.transporters || []).forEach(function (t, i) { h += '<option value="' + i + '">' + esc(t.AccountTitle) + '</option>'; });
        el.innerHTML = h; el.value = keep;
    }
    function transporterId() { var t = look.transporters[intOf($id('CmbTransporter').value)]; return $id('CmbTransporter').value === '' || !t ? 0 : t.Id; }
    function transporterSupCust() { var t = look.transporters[intOf($id('CmbTransporter').value)]; return $id('CmbTransporter').value === '' || !t ? 0 : t.SupplierCustomerId; }
    function setTransporter(id) {
        var i = (look.transporters || []).findIndex(function (t) { return t.Id === intOf(id); });
        $id('CmbTransporter').value = i >= 0 ? String(i) : '';
    }
    /** GetConfigurationsFromGlobalAndBindValuesInColumns:504 — FreightInwardAc when feature 4 is off. */
    function defaultTransporter() {
        if (look.subsidiaryAccountAllownOnVouchers) return;
        var tid = intOf(look.freightInwardAc);
        if (tid !== 0 && look.transporters.some(function (t) { return t.Id === tid; })) setTransporter(tid);
    }
    function setSupplier(id) {
        var el = $id('CmbBillToParty');
        el.value = String(intOf(id));
        if (el.value !== String(intOf(id))) el.value = '0';
    }
    /** EnabledDisabledFeilds:1892 */
    function enabledDisabled(base, gp) {
        $id('CmbBillToParty').disabled = base !== 2;
        $id('CmbVehicleType').disabled = false;
        $id('CmbGpNo').disabled = gp !== 0;
        $id('txtVehicleNo').disabled = gp !== 0;
        $id('txtBiltyNo').disabled = gp !== 0;
    }
    function setMode(update) {
        $id('btnSave').classList.toggle('is-hidden', update);
        $id('btnUpdate').classList.toggle('is-hidden', !update);
        $id('btnDelete').classList.toggle('is-hidden', !update);
    }

    /** reset():1324 — DocDate is not touched. */
    function reset() {
        Id = 0; GPID = 0; BaseDocumentTypeId = 0;
        setMode(false);
        $id('CmbBillToParty').value = '0';
        ['txtReferenceNo', 'txtRemarksHeader', 'CmbGpNo', 'CmbVehicleType', 'txtVehicleNo', 'txtBiltyNo', 'txtFreightAmount'].forEach(function (x) { $id(x).value = ''; });
        $id('CmbTransporter').value = '';
        dtdetail = []; render();
        return C.getJson(api + '/doc-no').then(function (r) {
            $id('txtdocno').value = r.docNo;
            defaultTransporter();
            enabledDisabled(0, 0);
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ load / refresh */

    function bindLookups(l) {
        look = l;
        C.fillSelect('CmbBillToParty', l.suppliers, 'Id', 'CompanyName');          // BindSupplierName:582
        transporterBind();
        $id('lstVehicleType').innerHTML = (l.vehicleTypes || []).map(function (v) { return '<option value="' + esc(v.VehicleDescription) + '">'; }).join('');
        historyCombos(l.historyFilters || {});
    }
    /** HistoryComboBind:461 */
    function historyCombos(hf) {
        C.fillSelect('cmbSupplierNameHistory', hf.Supplier, 'Id', 'Name');
        C.fillSelect('CmbPartyNameRegister', hf.Supplier, 'Id', 'Name');
        C.fillSelect('CmbTransporterRegister', hf.Transporter, 'Id', 'Name');
        C.fillSelect('CmbItemNameRegister', hf.Item, 'Id', 'Name');
        C.fillSelect('CmbWarehouseRegister', hf.Warehouse, 'Id', 'Name');
    }

    /** InitializeComponentMethod:384 — once, when the page opens. */
    function init() {
        return C.getJson(api + '/lookups').then(function (l) {
            bindLookups(l);
            var r = l.rights || {};
            $id('btnSave').disabled = !r.save; $id('BtnPrint').disabled = !r.print;
            $id('btnUpdate').disabled = !r.update; $id('btnDelete').disabled = !r.delete;
            $id('btnDelete').classList.add('is-hidden');
            if (!r.view) { $id('rightsNote').textContent = 'You do not have the View right for Grn Store.'; $id('rightsNote').classList.remove('is-hidden'); }
            if (Id === 0) $id('txtdocno').value = l.docNo;
            defaultTransporter();
            var days = intOf(l.defaultDaysToLessFromHistoryFromDate) > 0 ? intOf(l.defaultDaysToLessFromHistoryFromDate) : 3;
            $id('FromDateHistory').value = daysAgo(days); $id('ToDateHistory').value = C.today();
            var p = new URLSearchParams(window.location.search);
            if (p.get('fromDate') && p.get('toDate')) {                          // :431 opened with FromDate / ToDate
                $id('txtDatefromRegister').value = l.financialYearStart || p.get('fromDate');
                $id('txtDateToRegister').value = C.isoDay(p.get('toDate'));
                tab('tabRegister'); showRegister();
            } else {
                $id('txtDatefromRegister').value = daysAgo(days); $id('txtDateToRegister').value = C.today();
            }
            loaded = true;
            render();
        }).catch(function (e) { alert(e.message); });
    }

    /** toolStripButton1_Click:1365 — the globals again; the form's values are kept. */
    function refresh() {
        var sup = $id('CmbBillToParty').value, tr = transporterId();
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;                                                           // history / register combos are not rebound here
            C.fillSelect('CmbBillToParty', l.suppliers, 'Id', 'CompanyName');
            $id('CmbBillToParty').value = sup;
            transporterBind(); setTransporter(tr);
            $id('lstVehicleType').innerHTML = (l.vehicleTypes || []).map(function (v) { return '<option value="' + esc(v.VehicleDescription) + '">'; }).join('');
            defaultTransporter();
            render();
        }).catch(function (e) { alert(e.message); });
    }
    /** BtnRefresh_Click:2367 (History and Register Refresh). */
    function refreshFilters() {
        return C.getJson(api + '/history-filters').then(historyCombos).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ save / update / delete */

    function body() {
        return {
            Id: Id, DocDate: $id('DocDate').value, SupplierCustomerId: intOf($id('CmbBillToParty').value),
            ReferenceDocNo: $id('txtReferenceNo').value, InwardGatePassId: GPID, GpNo: $id('CmbGpNo').value,
            VehicleType: $id('CmbVehicleType').value, VehicleNo: $id('txtVehicleNo').value, BiltyNo: $id('txtBiltyNo').value,
            CarriageAmount: $id('txtFreightAmount').value, TransporterId: transporterId(), TransporterSupCustId: transporterSupCust(),
            RemarksHeader: $id('txtRemarksHeader').value, BaseDocumentTypeId: BaseDocumentTypeId,
            rows: dtdetail.map(function (r) {
                return {
                    Id: intOf(r.Id), /* :1142 then :1282 — the row's own Id either way */ OrderId: intOf(r.OrderId), OrderDetailId: intOf(r.OrderDetailId), OrderNo: intOf(r.OrderNo),
                    DemandId: intOf(r.DemandId), DemandDetailId: intOf(r.DemandDetailId), DemandNo: intOf(r.DemandNo),
                    PurchasePreBillId: intOf(r.PurchasePreBillId), PurchasePreBillDetailId: intOf(r.PurchasePreBillDetailId), PurchasePreBillNo: intOf(r.PurchasePreBillNo),
                    DeliveryChallanId: intOf(r.DeliveryChallanId), DeliveryChallanDetailId: intOf(r.DeliveryChallanDetailId), DeliveryChallanNo: intOf(r.DeliveryChallanNo),
                    ItemId: intOf(r.ItemId), ItemCode: r.ItemCode || '', ItemName: r.ItemName || '',
                    WarehouseId: intOf(r.WarehouseId), WarehouseName: r.WarehouseName || '', RackId: intOf(r.RackId), RackName: r.RackName || '',
                    ItemConditionId: intOf(r.ItemConditionId), UOMId: intOf(r.UOMId), UOM: r.UOM || '', UomEquivalent: num(r.UomEquivalent),
                    TotalOrderQty: num(r.TotalOrderQty), UsedOrderQty: num(r.UsedOrderQty), BalOrderQty: num(r.BalOrderQty),
                    TotalDemandQty: num(r.TotalDemandQty), UsedDemandQty: num(r.UsedDemandQty), BalDemandQty: num(r.BalDemandQty),
                    PurchasePreBillQty: num(r.PurchasePreBillQty), TotalDeliveryChallanQty: num(r.TotalDeliveryChallanQty),
                    UsedDeliveryChallanQty: num(r.UsedDeliveryChallanQty), BalDeliveryChallanQty: num(r.BalDeliveryChallanQty),
                    ThisQty: num(r.ThisQty), AvailableStockQty: num(r.AvailableStockQty), Remarks: r.Remarks || ''
                };
            })
        };
    }

    /** Insert():1064 — the checks before the confirm run here too, in the same order and words. */
    function insert() {
        if (!dtdetail.length) { alert('Detail Record Not Found'); return; }
        if (intOf($id('CmbBillToParty').value) === 0) { alert('Supplier field is required'); $id('CmbBillToParty').focus(); return; }
        if (!$id('CmbGpNo').value.trim()) { alert('Gate Pass No field is required'); $id('CmbGpNo').focus(); return; }
        if (BaseDocumentTypeId === 3 && GPID === 0) { alert('No Record found against gp no'); return; }
        if (!confirm(Id === 0 ? 'Are you sure to Save?' : 'Are you sure to Update?')) return;
        var wasUpdate = Id > 0;
        C.postJson(api + '/save', body()).then(function (r) {
            alert(r.message);
            var printPreview = $id('ChkPrint').checked;
            return reset().then(function () { if (printPreview) printId(r.id); });
        }).catch(function (e) { alert(e.message); if (wasUpdate) { /* the form keeps its state on failure */ } });
    }
    function save() { Id = 0; insert(); }                                     // btnSave_Click:1003
    function update() { insert(); }                                          // btnUpdate_Click:1016

    function del() {                                                            // btnDelete_Click:1028
        if (Id <= 0) { alert('Record Not Found'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + Id + '/delete', {}).then(function (r) { alert(r.message); reset(); })
            .catch(function (e) { alert(e.message); });
    }

    function printId(id) { C.printSlip(api + '/' + intOf(id) + '/slip', '212 - Goods Receipt Notes Store Slip'); }

    /* ------------------------------------------------------------------ read by id */

    function readById(id) {                                                     // ReadById:1180
        return C.getJson(api + '/' + id).then(function (h) {
            return reset().then(function () {
                Id = h.Id;
                tab('tabForm');
                setMode(true);
                $id('txtdocno').value = h.DocNo;
                $id('DocDate').value = C.isoDay(h.DocDate);
                setSupplier(h.SupplierCustomerId);
                $id('txtReferenceNo').value = h.ReferenceDocNo || '';
                GPID = intOf(h.InwardGatePassId);
                $id('CmbGpNo').value = String(h.GpNo);
                $id('CmbVehicleType').value = h.VehicleType || '';
                $id('txtVehicleNo').value = h.VehicleNo || '';
                $id('txtBiltyNo').value = h.BiltyNo || '';
                setTransporter(h.TransporterId);
                $id('txtFreightAmount').value = String(num(h.CarriageAmount));
                $id('txtRemarksHeader').value = h.RemarksHeader || '';
                BaseDocumentTypeId = intOf(h.BaseDocumentTypeId);
                dtdetail = h.rows || [];
                render();
                enabledDisabled(BaseDocumentTypeId, GPID);
                return stockUpdate();
            });
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ history */

    function showHistory() {                                                    // HistoryGridFill:1456
        var type = (document.querySelector('input[name="histDate"]:checked') || {}).value || 'doc';
        C.getJson(api + '/history' + C.qs({
            dateType: type,
            fromDate: $id('chkFromDateHistory').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDateHistory').checked ? $id('ToDateHistory').value : '',
            fromDocNo: intOf($id('txtFromDocNoHistory').value), toDocNo: intOf($id('txtToDocNoHistory').value),
            supplierId: intOf($id('cmbSupplierNameHistory').value)
        })).then(function (rows) {
            historyRows = rows || [];
            var cols = ['BaseDocumentType', 'DocDate', 'DocNo', 'SupplierName', 'GpNo', 'VehicleNo', 'BiltyNo', 'Transporter', 'FreightAmount',
                'RemarksHeader', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'NoOfAttachments'];
            var t = $id('GrdHistory');
            t.tHead.innerHTML = '<tr><th>Edit</th><th>Print</th>' + cols.map(function (c) { return '<th>' + esc(cap(c)) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = historyRows.map(function (r, i) {
                return '<tr data-i="' + i + '" onclick="GrnStore.historyDetail(' + i + ')" ondblclick="GrnStore.open(' + r.Id + ')">' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();GrnStore.open(' + r.Id + ')">Edit</button></td>' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();GrnStore.printId(' + r.Id + ')">Print</button></td>' +
                    cols.map(function (c) {
                        var v = r[c];
                        if (c === 'DocDate') v = dmy(v);
                        else if (c === 'EntryDate' || c === 'ModifyDate') v = dmyt(v);
                        else if (c === 'FreightAmount') return '<td class="num">' + esc(n2(v)) + '</td>';
                        if (c === 'DocNo') return '<td class="lnk" onclick="event.stopPropagation();GrnStore.open(' + r.Id + ')">' + esc(v) + '</td>';
                        return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                    }).join('') + '</tr>';
            }).join('');
            if (!historyRows.length) { $id('GrdHistoryDetail').tHead.innerHTML = ''; $id('GrdHistoryDetail').tBodies[0].innerHTML = ''; }
        }).catch(function (e) { alert(e.message); });
    }

    /** GrdHistory_SelectionChanged:1645 → grddetailhistorySettings:1676. */
    function historyDetail(i) {
        var h = historyRows[i]; if (!h) return;
        document.querySelectorAll('#GrdHistory tbody tr').forEach(function (tr) { tr.classList.toggle('is-selected', tr.getAttribute('data-i') === String(i)); });
        var base = intOf(h.BaseDocumentTypeId);
        C.getJson(api + '/' + h.Id).then(function (doc) {
            var hide = hiddenFor(base).concat(['AvailableStockQty']);
            if (base === 1) hide.push('UsedOrderQty');
            if (base === 2) hide.push('UsedDemandQty');
            if (base === 3) hide.push('UsedDeliveryChallanQty');
            var cols = GRID_COLS.filter(function (c) { return hide.indexOf(c.k) < 0; }).map(function (c) {
                return c.combo ? { k: 'ItemCondition', c: 'Item Condition' } : c;
            });
            var t = $id('GrdHistoryDetail');
            t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c.c || cap(c.k)) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = (doc.rows || []).map(function (r) {
                return '<tr>' + cols.map(function (c) {
                    var v = r[c.k];
                    return '<td class="' + (c.n ? 'num' : '') + '">' + esc(c.n ? n2(v) : (v === null || v === undefined ? '' : v)) + '</td>';
                }).join('') + '</tr>';
            }).join('');
        }).catch(function () {
            $id('GrdHistoryDetail').tHead.innerHTML = ''; $id('GrdHistoryDetail').tBodies[0].innerHTML = '';
        });
    }

    function newHistory() {                                                     // btnNewHistory_Click:1410
        $id('FromDateHistory').value = C.today(); $id('ToDateHistory').value = C.today();
        $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = '';
        $id('cmbSupplierNameHistory').value = '0';
        historyRows = [];
        ['GrdHistory', 'GrdHistoryDetail'].forEach(function (g) { $id(g).tHead.innerHTML = ''; $id(g).tBodies[0].innerHTML = ''; });
    }

    /* ------------------------------------------------------------------ register */

    var REG_COLS = ['BaseDocumentType', 'Print', 'DocDate', 'DocNo', 'OrderNo', 'OrderDate', 'PartyName', 'ReferenceNo', 'VehicleNo', 'BiltyNo',
        'TransporterName', 'FreightAmount', 'ItemName', 'ItemUOM', 'ItemQty', 'WareHouseName', 'NoOfAttachments'];

    function showRegister() {                                                   // RegisterGridFill:2391
        var p = new URLSearchParams(window.location.search);
        C.getJson(api + '/register' + C.qs({
            fromDate: $id('txtDatefromRegister').value, toDate: $id('txtDateToRegister').value,
            fromDocNo: intOf($id('txtDocNoFromRegister').value), toDocNo: intOf($id('txtDocNoToRegister').value),
            supplierId: intOf($id('CmbPartyNameRegister').value), warehouseId: intOf($id('CmbWarehouseRegister').value),
            itemId: intOf($id('CmbItemNameRegister').value), onlyPending: p.get('onlyPending') === '1' ? 'true' : ''
        })).then(function (res) {
            registerRaw = res.raw || [];
            var rows = res.rows || [];
            var t = $id('GrdiRegister');
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
            t.tHead.innerHTML = '<tr>' + REG_COLS.map(function (c) { return '<th>' + esc(c === 'Print' ? 'Print' : cap(c)) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (r) {
                return '<tr>' + REG_COLS.map(function (c) {
                    var v = r[c];
                    if (c === 'Print') return '<td><button type="button" class="cx-link" onclick="GrnStore.printId(' + r.Id + ')">Print</button></td>';
                    if (c === 'DocDate' || c === 'OrderDate') return '<td>' + esc(dmy(v)) + '</td>';
                    if (c === 'OrderNo') return '<td class="lnk" onclick="GrnStore.poSlip(' + intOf(r.OrderId) + ')">' + esc(v === null || v === undefined ? '' : v) + '</td>';
                    if (c === 'FreightAmount' || c === 'ItemQty') return '<td class="num">' + esc(n2(v)) + '</td>';
                    return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { alert(e.message); });
    }

    function resetRegister() {                                                  // BtnResetRegister_Click:2346
        $id('txtDatefromRegister').value = C.today(); $id('txtDateToRegister').value = C.today();
        $id('txtDocNoFromRegister').value = ''; $id('txtDocNoToRegister').value = '';
        ['CmbPartyNameRegister', 'CmbItemNameRegister', 'CmbWarehouseRegister', 'CmbTransporterRegister'].forEach(function (x) { $id(x).value = '0'; });
        $id('GrdiRegister').tHead.innerHTML = ''; $id('GrdiRegister').tBodies[0].innerHTML = '';
    }

    /** BtnRegisterPrint_Click:2577 — the register procedure's rows (336 layout not ported). */
    function printRegister() {
        if (!registerRaw.length) { alert('Not Record Found For Display'); return; }
        printRows(registerRaw, '336 - Grn Store Register');
    }
    function poSlip(orderId) {                                                  // GrdiRegister_LinkClicked:2538
        C.printSlip(api + '/po-slip/' + orderId, '201 - Purchase Order General Slip');
    }
    function printRows(rows, title) {
        var cols = Object.keys(rows[0]);
        var w = window.open('', '_blank');
        if (!w) { alert('Allow pop-ups to print.'); return; }
        w.document.write('<html><head><title>' + esc(title) + '</title><style>body{font-family:Verdana;font-size:10px}' +
            'table{border-collapse:collapse}td,th{border:1px solid #444;padding:2px 4px;white-space:nowrap}</style></head><body><h3>' + esc(title) +
            '</h3><table><thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr></thead><tbody>' +
            rows.map(function (r) { return '<tr>' + cols.map(function (c) { return '<td>' + esc(r[c]) + '</td>'; }).join('') + '</tr>'; }).join('') +
            '</tbody></table><script>window.print()<\/script></body></html>');
        w.document.close();
    }

    /* ------------------------------------------------------------------ loaders: shared */

    function loaderGrid(tableId, cols, rows, onCheck, checkedAll) {
        var t = $id(tableId);
        t.tHead.innerHTML = '<tr><th>Select</th>' + cols.map(function (c) { return '<th>' + esc(cap(c.k || c)) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = rows.map(function (r, i) {
            return '<tr><td><input type="checkbox" data-i="' + i + '"' + (checkedAll ? ' checked' : '') + (onCheck ? ' onchange="' + onCheck + '"' : '') + '></td>' +
                cols.map(function (c) {
                    var k = c.k || c, v = ci(r, k);
                    if (c.chk) return '<td style="text-align:center"><input type="checkbox" disabled' + (v === true || v === 1 || String(v).toLowerCase() === 'true' ? ' checked' : '') + '></td>';
                    if (c.link) return '<td class="lnk" onclick="GrnStore.loaderSlip(\'' + c.link + '\',' + intOf(ci(r, c.idKey)) + ')">' + esc(v === null || v === undefined ? '' : v) + '</td>';
                    if (c.d === 1) v = C.gridDate(v); else if (c.d === 2) v = C.gridDateTime(v, true);
                    return '<td class="' + (c.n ? 'num' : '') + '">' + esc(v === null || v === undefined ? '' : v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
    }
    function checkedIdx(tableId) {
        return Array.prototype.map.call($id(tableId).querySelectorAll('tbody input[type=checkbox]:checked'), function (x) { return intOf(x.getAttribute('data-i')); });
    }
    function clearGrid(id) { $id(id).tHead.innerHTML = ''; $id(id).tBodies[0].innerHTML = ''; }
    function closeLoader(id) { C.closeModal(id); }
    /** The loaders' grd_LinkClicked on DocNo — 201 (PO), 454 (Demand), 148 (Delivery Challan). */
    function loaderSlip(kind, id) {
        var t = kind === 'po' ? '201 - Purchase Order General Slip' : kind === 'demand' ? '454 - Purchase Demand Slip' : '148 - Delivery Challan Slip';
        C.printSlip(api + '/loader/' + kind + '/' + id + '/slip', t);
    }

    function resolveWh(row, itemId, configWh) {                                 // FormHelper.ResolveWarehouseAndRack:697
        var itemData = (look.racks || []).filter(function (x) { return x.ItemId === itemId; });
        if (!itemData.length) return;
        var r = C.resolveWarehouseAndRack(itemData, configWh);
        if (intOf(r.WarehouseId) > 0) { row.WarehouseId = r.WarehouseId; row.WarehouseName = r.WarehouseName; }
        if (intOf(r.RackId) > 0) { row.RackId = r.RackId; row.RackName = r.RackName; }
    }
    function blankRow() {
        return { Id: 0, OrderId: 0, OrderDetailId: 0, OrderNo: 0, DemandId: 0, DemandDetailId: 0, DemandNo: 0, PurchasePreBillId: 0, PurchasePreBillDetailId: 0,
            PurchasePreBillNo: 0, DeliveryChallanId: 0, DeliveryChallanDetailId: 0, DeliveryChallanNo: 0, ItemId: 0, ItemCode: '', ItemName: '', WarehouseId: 0,
            WarehouseName: '', RackId: 0, RackName: '', ItemConditionId: 0, UOMId: 0, UOM: '', UomEquivalent: 0, TotalOrderQty: 0, UsedOrderQty: 0, BalOrderQty: 0,
            TotalDemandQty: 0, UsedDemandQty: 0, BalDemandQty: 0, PurchasePreBillQty: 0, TotalDeliveryChallanQty: 0, UsedDeliveryChallanQty: 0,
            BalDeliveryChallanQty: 0, ThisQty: 0, AvailableStockQty: 0, Remarks: '' };
    }
    function afterLoad() {                                                      // BindGrids + EnabledDisabledFeilds + GridStockUpdate
        render();
        enabledDisabled(BaseDocumentTypeId, GPID);
        stockUpdate();
    }
    function noRowsLoaded() { if (!dtdetail.length) BaseDocumentTypeId = 0; }
    function setGp(first) {
        GPID = intOf(ci(first, 'GatePassId'));
        $id('CmbGpNo').value = ci(first, 'GpSrNo') === null ? '' : String(ci(first, 'GpSrNo'));
        $id('CmbVehicleType').value = ci(first, 'VehicleType') || '';
        $id('txtVehicleNo').value = ci(first, 'VehicleNo') || '';
        $id('txtBiltyNo').value = ci(first, 'BiltyNo') || '';
    }
    function guard(allowed, target) {                                           // the three Load buttons' first check
        if (BaseDocumentTypeId > 0 && BaseDocumentTypeId !== allowed && dtdetail.length > 0) {
            var name = BaseDocumentTypeId === 1 ? 'Purchase Order' : BaseDocumentTypeId === 2 ? 'Purchase Demand' : BaseDocumentTypeId === 3 ? 'Delivery Challan' : '';
            alert('Data for ' + name + ' already exists in the grid. Please Reset the Form before loading data for ' + target + '.');
            return false;
        }
        return true;
    }

    /* ------------------------------------------------------------------ Load PO */

    var po = { rows: [], opened: false };
    var PO_COLS = [{ k: 'DocDate', d: 1 }, { k: 'DocNo', link: 'po', idKey: 'Id' }, 'SupplierName', 'DeliveryTerm', 'RemarksHeader', 'VehicleNo', 'BiltyNo', 'GpSrNo', { k: 'GpDate', d: 1 }, 'GpStatus',
        { k: 'EntryDate', d: 1 }, 'EntryUserName', { k: 'ModifyDate', d: 1 }, 'ModifyUserName'];
    var PO_DETAIL = ['DocNo', 'ItemCode', 'ItemName', 'UomCode', { k: 'OrderItemQty', n: 1 }, { k: 'UsedQty', n: 1 }, { k: 'BalQty', n: 1 }];

    function openPo() {                                                         // btnLoadSupplySchedule_Click:1908 — a new dialog per click
        if (!guard(1, 'Purchase Order')) return;
        po.rows = []; clearGrid('poGrd'); clearGrid('poGrdDetail');
        $id('poFromDate').value = daysAgo(7); $id('poToDate').value = C.today();
        $id('poDocNoFrom').value = ''; $id('poDocNoTo').value = '';
        C.openModal('dlgPo');
        C.getJson(api + '/loader/po/lookups').then(function (l) {
            C.fillSelect('poBillToParty', l.suppliers, 'Id', 'Name'); $id('poBillToParty').value = '0';
            C.fillSelect('poItemName', l.items, 'Id', 'Name'); $id('poItemName').value = '0';
            return poSearch(daysAgo(7));                                        // PendingDataDbCall(Intilization: true)
        }).catch(function (e) { alert(e.message); });
    }
    function poSearch(from) {
        return C.getJson(api + '/loader/po' + C.qs({
            fromDate: from || $id('poFromDate').value, toDate: $id('poToDate').value,
            fromDocNo: intOf($id('poDocNoFrom').value), toDocNo: intOf($id('poDocNoTo').value),
            supplierId: intOf($id('poBillToParty').value)
        })).then(function (rows) {
            po.rows = rows || [];
            /* GrdDataBind:252 — one header row per pending line (no de-duplication). */
            if (po.rows.length) loaderGrid('poGrd', PO_COLS, po.rows, 'GrnStore.poChecked()'); else clearGrid('poGrd');
            clearGrid('poGrdDetail');
        }).catch(function (e) { alert(e.message); });
    }
    function poShow() { poSearch(); }
    function poChecked() {                                                      // grd_RowCheckStateChanged:351
        var ids = {};
        checkedIdx('poGrd').forEach(function (i) { ids[intOf(ci(po.rows[i], 'Id'))] = 1; });
        po.detail = po.rows.filter(function (r) { return ids[intOf(ci(r, 'Id'))]; });
        if (!po.detail.length) { clearGrid('poGrdDetail'); return; }
        var view = po.detail.map(function (r) { var o = Object.assign({}, r); o.BalQty = ci(r, 'ItemQty'); return o; });
        loaderGrid('poGrdDetail', PO_DETAIL, view, null, true);                 // CheckAllRows
    }
    function poNew() {                                                          // btnReset_Click:486
        $id('poFromDate').value = look.financialYearStart || $id('poFromDate').value;
        $id('poDocNoFrom').value = ''; $id('poDocNoTo').value = ''; $id('poItemName').value = '0';
        poSearch();
    }
    function poRefresh() {                                                      // btnRefresh_Click:504 — branch-wise from here on
        C.getJson(api + '/loader/po/lookups?refresh=true').then(function (l) {
            var s = $id('poBillToParty').value, it = $id('poItemName').value;
            C.fillSelect('poBillToParty', l.suppliers, 'Id', 'Name'); $id('poBillToParty').value = s;
            C.fillSelect('poItemName', l.items, 'Id', 'Name'); $id('poItemName').value = it;
        }).catch(function (e) { alert(e.message); });
    }
    function poLoad() {                                                         // btnLoadOnInvoice_Click_1:436
        var idx = checkedIdx('poGrdDetail');
        if (!idx.length) { alert('Check the row first in Detail Grid'); return; }
        var sel = idx.map(function (i) { return po.detail[i]; });
        var baseId = intOf(ci(sel[0], 'Id')), detailIds = [];
        for (var k = 0; k < sel.length; k++) {
            if (intOf(ci(sel[k], 'Id')) !== baseId) { alert('Sorry! You can Only Check rows which have the same DocNo'); return; }
            detailIds.push(intOf(ci(sel[k], 'OrderDetailId')));
        }
        if (!detailIds.length) { alert('No valid rows selected.'); return; }
        var loader = po.rows.filter(function (r) { return detailIds.indexOf(intOf(ci(r, 'OrderDetailId'))) >= 0; });
        C.closeModal('dlgPo');
        loadFromPO(loader);
    }
    function loadFromPO(dtLoader) {                                             // LoadInGridDetailFromPO:1937
        if (!dtLoader || !dtLoader.length) { noRowsLoaded(); return; }
        BaseDocumentTypeId = 1;
        var first = dtLoader[0];
        if (dtdetail.length > 0) {
            var existingOrderId = intOf(dtdetail[0].OrderId);
            if (existingOrderId !== 0 && existingOrderId !== intOf(ci(first, 'Id'))) {
                alert('Data for another Order already exists in the grid. Please Reset the Form before loading data for a different Order.'); return;
            }
            var existingPartyId = intOf($id('CmbBillToParty').value);
            if (existingPartyId !== 0 && existingPartyId !== intOf(ci(first, 'SupplierCustomerId'))) {
                alert('Data for another party already exists in the grid. Please Reset the Form before loading data for a different party.'); return;
            }
        }
        setSupplier(ci(first, 'SupplierCustomerId'));
        if (intOf(ci(first, 'GatePassId')) > 0) setGp(first);
        var existing = {};
        dtdetail.forEach(function (r) { existing[intOf(r.OrderDetailId)] = 1; });
        dtLoader.forEach(function (row) {
            var odId = intOf(ci(row, 'OrderDetailId'));
            if (existing[odId]) return;
            existing[odId] = 1;
            var dr = blankRow();
            dr.OrderId = intOf(ci(row, 'Id')); dr.OrderDetailId = odId; dr.OrderNo = intOf(ci(row, 'DocNo'));
            dr.ItemId = intOf(ci(row, 'ItemId')); dr.ItemCode = ci(row, 'ItemCode'); dr.ItemName = ci(row, 'ItemName');
            dr.UOMId = intOf(ci(row, 'ItemUOMId')); dr.UOM = ci(row, 'UomCode'); dr.UomEquivalent = num(ci(row, 'Equivalent'));
            if (Object.keys(row).some(function (k) { return k.toLowerCase() === 'itemconditionid'; })) dr.ItemConditionId = intOf(ci(row, 'ItemConditionId'));
            dr.TotalOrderQty = num(ci(row, 'OrderItemQty')); dr.UsedOrderQty = num(ci(row, 'UsedQty')); dr.BalOrderQty = num(ci(row, 'ItemQty'));
            dr.ThisQty = dr.BalOrderQty;
            resolveWh(dr, dr.ItemId, intOf(look.wareHouseConfigId));
            dtdetail.push(dr);
        });
        afterLoad();
    }

    /* ------------------------------------------------------------------ Load Demand */

    var pd = { rows: [], heads: [], detail: [] };
    var PD_COLS = [{ k: 'DocNo', link: 'demand', idKey: 'Id' }, { k: 'DocDate', d: 1 }, 'ParentCategory', 'Status', 'RemarksHeader', 'VehicleNo', 'BiltyNo', 'GpSrNo', { k: 'GpDate', d: 1 }, 'GpStatus',
        { k: 'EntryDate', d: 1 }, 'EntryUserName', { k: 'ModifyDate', d: 1 }, 'ModifyUserName', { k: 'IsApproved', chk: 1 }, { k: 'ApprovedDate', d: 1 }, 'ApprovalUserName'];
    var PD_DETAIL = ['DepartmentName', 'RequestBy', 'ItemName', 'ItemCode', 'Uom', 'ItemCondition', 'JobLot', { k: 'RequiredQty', n: 1 }, { k: 'ApprovedQty', n: 1 },
        { k: 'QtyUsedInPreBill', n: 1 }, { k: 'QtyUsedInGrnDirectly', n: 1 }, { k: 'BalanceQty', n: 1 }, 'RemarksDetail'];

    function openDemand() {                                                     // BtnLoadPurchaseDemand_Click:2056
        if (!guard(2, 'Purchase Demand')) return;
        pd.rows = []; pd.heads = []; clearGrid('pdGrd'); clearGrid('pdGrdDetail');
        $id('pdFromDate').value = daysAgo(7); $id('pdToDate').value = C.today();
        $id('pdDocNoFrom').value = ''; $id('pdDocNoTo').value = '';
        C.openModal('dlgDemand');
        C.getJson(api + '/loader/demand/lookups').then(function (l) {
            if ((l.items || []).length) { C.fillSelect('pdItemName', l.items, 'Id', 'Name'); } else { $id('pdItemName').innerHTML = '<option value="0"></option>'; }
            $id('pdItemName').value = '0';
            return pdSearch(daysAgo(7));
        }).catch(function (e) { alert(e.message); });
    }
    function pdSearch(from) {
        return C.getJson(api + '/loader/demand' + C.qs({
            fromDate: from || $id('pdFromDate').value, toDate: $id('pdToDate').value,
            fromDocNo: intOf($id('pdDocNoFrom').value), toDocNo: intOf($id('pdDocNoTo').value), itemId: intOf($id('pdItemName').value)
        })).then(function (rows) {
            pd.rows = rows || [];
            var seen = {}; pd.heads = pd.rows.filter(function (r) { var id = intOf(ci(r, 'Id')); if (seen[id]) return false; seen[id] = 1; return true; });
            if (pd.heads.length) loaderGrid('pdGrd', PD_COLS, pd.heads, 'GrnStore.pdChecked()'); else clearGrid('pdGrd');
            clearGrid('pdGrdDetail');
        }).catch(function (e) { alert(e.message); });
    }
    function pdShow() { pdSearch(); }
    function pdChecked() {
        var ids = {};
        checkedIdx('pdGrd').forEach(function (i) { ids[intOf(ci(pd.heads[i], 'Id'))] = 1; });
        pd.detail = pd.rows.filter(function (r) { return ids[intOf(ci(r, 'Id'))]; });
        if (!pd.detail.length) { clearGrid('pdGrdDetail'); return; }
        loaderGrid('pdGrdDetail', PD_DETAIL, pd.detail, null, true);
    }
    function pdNew() {
        $id('pdFromDate').value = look.financialYearStart || $id('pdFromDate').value;
        $id('pdDocNoFrom').value = ''; $id('pdDocNoTo').value = ''; $id('pdItemName').value = '0';
        pdSearch();
    }
    function pdRefresh() {
        C.getJson(api + '/loader/demand/lookups').then(function (l) {
            if (!(l.items || []).length) return;
            var it = $id('pdItemName').value; C.fillSelect('pdItemName', l.items, 'Id', 'Name'); $id('pdItemName').value = it;
        }).catch(function (e) { alert(e.message); });
    }
    function pdLoad() {                                                         // frmPendingPurchaseDemand.btnLoadOnInvoice_Click_1:496
        var idx = checkedIdx('pdGrdDetail');
        if (!idx.length) { alert('Check the row first in Detail Grid'); return; }
        var detailIds = [], gatePassRow = null, gatePassDocId = null;
        for (var k = 0; k < idx.length; k++) {
            var row = pd.detail[idx[k]];
            var gpId = intOf(ci(row, 'GatePassId')), gpStatus = String(ci(row, 'GpStatus') || ''), docId = intOf(ci(row, 'Id'));
            if (gpId > 0) {
                if (gatePassRow === null) { gatePassRow = row; gatePassDocId = docId; }
                else if (gatePassDocId !== docId) { alert('You can select only rows from the same document when a Gate Pass is included.'); return; }
                if (gpStatus !== 'Accepted') { alert('Only Accepted Gate Pass can be selected.'); return; }
            } else if (gatePassDocId !== null && gatePassDocId !== docId) {
                alert('You can select only rows from the same document when a Gate Pass is included.'); return;
            }
            detailIds.push(intOf(ci(row, 'DetailId')));
        }
        if (!detailIds.length) { alert('No valid rows selected.'); return; }
        var loader = pd.rows.filter(function (r) { return detailIds.indexOf(intOf(ci(r, 'DetailId'))) >= 0; });
        C.closeModal('dlgDemand');
        loadFromDemand(loader);
    }
    function loadFromDemand(dtLoader) {                                         // LoadInGridDetailForDemand:2084
        if (!dtLoader || !dtLoader.length) { noRowsLoaded(); return; }
        var first = dtLoader[0];
        var currentGatePassId = intOf(ci(first, 'GatePassId'));
        BaseDocumentTypeId = 2;
        if (dtdetail.length > 0) {
            var existingOrderId = intOf(dtdetail[0].DemandId);
            if (GPID === 0 && currentGatePassId > 0) {
                alert('Data for another demand (without a Gate Pass) already exists in the grid. You cannot load a new row with a Gate Pass. Please reset the form or select a demand without a Gate Pass.'); return;
            }
            if (GPID > 0 && existingOrderId !== 0 && existingOrderId !== intOf(ci(first, 'Id'))) {
                alert('Data for another demand (with a Gate Pass) already exists in the grid. Please reset the form before loading data for a different demand.'); return;
            }
        }
        if (currentGatePassId > 0) setGp(first);
        var existing = {};
        dtdetail.forEach(function (r) { existing[intOf(r.DemandDetailId)] = 1; });
        dtLoader.forEach(function (row) {
            var dId = intOf(ci(row, 'DetailId'));
            if (existing[dId]) return;
            existing[dId] = 1;
            var dr = blankRow();
            dr.DemandId = intOf(ci(row, 'Id')); dr.DemandDetailId = dId; dr.DemandNo = intOf(ci(row, 'DocNo'));
            dr.ItemId = intOf(ci(row, 'ItemId')); dr.ItemCode = ci(row, 'ItemCode'); dr.ItemName = ci(row, 'ItemName');
            dr.UOMId = intOf(ci(row, 'ItemSchuomId')); dr.UOM = ci(row, 'Uom'); dr.UomEquivalent = num(ci(row, 'UomEquivalent'));
            if (Object.keys(row).some(function (k) { return k.toLowerCase() === 'itemconditionid'; })) dr.ItemConditionId = intOf(ci(row, 'ItemConditionId'));
            dr.TotalDemandQty = num(ci(row, 'ApprovedQty'));
            dr.UsedDemandQty = num(ci(row, 'QtyUsedInPreBill')) + num(ci(row, 'QtyUsedInGrnDirectly'));
            dr.BalDemandQty = num(ci(row, 'BalanceQty'));
            dr.ThisQty = dr.BalDemandQty;
            dr.Remarks = ci(row, 'RemarksDetail') === null ? '' : String(ci(row, 'RemarksDetail'));
            resolveWh(dr, dr.ItemId, intOf(look.wareHouseConfigId));
            dtdetail.push(dr);
        });
        afterLoad();
    }

    /* ------------------------------------------------------------------ Load Delivery Challan */

    var dc = { rows: [], heads: [], detail: [] };
    var DC_COLS = [{ k: 'DocNo', link: 'dc', idKey: 'DeliveryChallanHeaderId' }, { k: 'DocDate', d: 1 }, 'ReferenceNo', 'DeliveryTerm', 'CityName', 'VehicleNo', 'BiltyNo', 'GpSrNo', { k: 'GpDate', d: 1 }, 'GpStatus',
        'RemarksHeader', { k: 'EntryDate', d: 1 }, 'EntryUserName', { k: 'ModifyDate', d: 1 }, 'ModifyUserName'];
    var DC_DETAIL = [{ k: 'PurchaseDemandNo' }, { k: 'PurchaseDemandDate', d: 1 }, { k: 'PurchasePreBillNo' }, { k: 'PurchasePreBillDate', d: 1 }, 'BillToPartyName',
        'WareHouseName', 'ItemCode', 'ItemName', 'Uom', 'ItemCondition', { k: 'ChallanQty', n: 1 }, 'VendorSupplierName', 'ReferencePartyName', 'PartyBillNo',
        { k: 'PartyBillDate', d: 1 }, { k: 'PurchaseDemandQty', n: 1 }, { k: 'PurchaseOrderQty', n: 1 }, { k: 'PurchasePreBillQty', n: 1 },
        { k: 'ChallanQtyUsedInGrn', n: 1 }, { k: 'BalChallanQty', n: 1 }, 'RemarksDetail'];

    function openDc() {                                                         // BtnLoadDeliveryChallan_Click:2197
        if (!guard(3, 'Delivery Challan')) return;
        dc.rows = []; dc.heads = []; clearGrid('dcGrd'); clearGrid('dcGrdDetail');
        $id('dcFromDate').value = daysAgo(7); $id('dcToDate').value = C.today();
        $id('dcDocNoFrom').value = ''; $id('dcDocNoTo').value = '';
        C.openModal('dlgDc');
        C.getJson(api + '/loader/dc/lookups').then(function (l) {
            C.fillSelect('dcBillToParty', l.billToParties, 'Id', 'Name'); $id('dcBillToParty').value = '0';
            C.fillSelect('dcItemName', l.items, 'Id', 'Name'); $id('dcItemName').value = '0';
            return dcSearch(daysAgo(7));
        }).catch(function (e) { alert(e.message); });
    }
    function dcSearch(from) {
        return C.getJson(api + '/loader/dc' + C.qs({
            fromDate: from || $id('dcFromDate').value, toDate: $id('dcToDate').value,
            fromDocNo: intOf($id('dcDocNoFrom').value), toDocNo: intOf($id('dcDocNoTo').value),
            itemId: intOf($id('dcItemName').value), billToPartyId: intOf($id('dcBillToParty').value)
        })).then(function (rows) {
            dc.rows = rows || [];
            var seen = {};
            dc.heads = dc.rows.filter(function (r) { var id = intOf(ci(r, 'DeliveryChallanHeaderId')); if (seen[id]) return false; seen[id] = 1; return true; });
            if (dc.heads.length) loaderGrid('dcGrd', DC_COLS, dc.heads, 'GrnStore.dcChecked()'); else clearGrid('dcGrd');
            clearGrid('dcGrdDetail');
        }).catch(function (e) { alert(e.message); });
    }
    function dcShow() { dcSearch(); }
    function dcChecked() {
        var ids = {};
        checkedIdx('dcGrd').forEach(function (i) { ids[intOf(ci(dc.heads[i], 'DeliveryChallanHeaderId'))] = 1; });
        dc.detail = dc.rows.filter(function (r) { return ids[intOf(ci(r, 'DeliveryChallanHeaderId'))]; });
        if (!dc.detail.length) { clearGrid('dcGrdDetail'); return; }
        var view = dc.detail.map(function (r) {
            var o = Object.assign({}, r);
            o.PurchaseDemandNo = ci(r, 'PurchaseDemandDocNo'); o.PurchaseDemandDate = ci(r, 'PurchaseDemandDocDate');
            o.PurchasePreBillNo = ci(r, 'PurchasePreBillDocNo'); o.PurchasePreBillDate = ci(r, 'PurchasePreBillDocDate');
            return o;
        });
        loaderGrid('dcGrdDetail', DC_DETAIL, view, null, true);
    }
    function dcNew() {
        $id('dcFromDate').value = look.financialYearStart || $id('dcFromDate').value;
        $id('dcDocNoFrom').value = ''; $id('dcDocNoTo').value = ''; $id('dcItemName').value = '0';
        dcSearch();
    }
    function dcRefresh() {
        C.getJson(api + '/loader/dc/lookups').then(function (l) {
            var s = $id('dcBillToParty').value, it = $id('dcItemName').value;
            C.fillSelect('dcBillToParty', l.billToParties, 'Id', 'Name'); $id('dcBillToParty').value = s;
            C.fillSelect('dcItemName', l.items, 'Id', 'Name'); $id('dcItemName').value = it;
        }).catch(function (e) { alert(e.message); });
    }
    function dcLoad() {                                                         // frmPendingDeliveryChallanLoader.btnLoadOnInvoice_Click_1:548
        var idx = checkedIdx('dcGrdDetail');
        if (!idx.length) { alert('Check the row first in Detail Grid'); return; }
        var sel = idx.map(function (i) { return dc.detail[i]; });
        var baseId = intOf(ci(sel[0], 'DeliveryChallanHeaderId')), detailIds = [];
        for (var k = 0; k < sel.length; k++) {
            if (String(ci(sel[k], 'GpStatus') || '') !== 'Accepted') { alert('Only Accepted Gate Pass can be selected.'); return; }
            if (intOf(ci(sel[k], 'DeliveryChallanHeaderId')) !== baseId) { alert('Sorry! You can Only Check rows which have the same DocNo'); return; }
            detailIds.push(intOf(ci(sel[k], 'DetailId')));
        }
        if (!detailIds.length) { alert('No valid rows selected.'); return; }
        var loader = dc.rows.filter(function (r) { return detailIds.indexOf(intOf(ci(r, 'DetailId'))) >= 0; });
        C.closeModal('dlgDc');
        loadFromDc(loader);
    }
    function loadFromDc(dtLoader) {                                             // LoadInGridDetailFromDeliveryChallan:2225
        if (!dtLoader || !dtLoader.length) { noRowsLoaded(); return; }
        BaseDocumentTypeId = 3;
        var first = dtLoader[0];
        if (dtdetail.length > 0) {
            var existingOrderId = intOf(dtdetail[0].DeliveryChallanId);
            if (existingOrderId !== 0 && existingOrderId !== intOf(ci(first, 'DeliveryChallanHeaderId'))) {
                alert('Data for another Delivery-Challan already exists in the grid. Please Reset the Form before loading data for a different Delivery-Challan.'); return;
            }
            var existingPartyId = intOf($id('CmbBillToParty').value);
            if (existingPartyId !== 0 && existingPartyId !== intOf(ci(first, 'BillToPartyId'))) {
                alert('Data for another party already exists in the grid. Please Reset the Form before loading data for a different party.'); return;
            }
        }
        setSupplier(ci(first, 'BillToPartyId'));
        setGp(first);                                                           // :2258-2262, unconditionally
        $id('txtRemarksHeader').value = ci(first, 'RemarksHeader') === null ? '' : String(ci(first, 'RemarksHeader'));
        var existing = {};
        dtdetail.forEach(function (r) { existing[intOf(r.DeliveryChallanDetailId)] = 1; });
        dtLoader.forEach(function (row) {
            var detailId = intOf(ci(row, 'DetailId'));
            if (existing[detailId]) return;
            existing[detailId] = 1;
            var dr = blankRow();
            dr.OrderId = intOf(ci(row, 'PurchaseOrderDetailId'));              // :2282 — swapped on the desktop
            dr.OrderDetailId = intOf(ci(row, 'PurchaseOrderHeaderId'));        // :2283
            dr.OrderNo = intOf(ci(row, 'PurchaseOrderDocNo'));
            dr.DemandId = intOf(ci(row, 'PurchaseDemandHeaderId')); dr.DemandDetailId = intOf(ci(row, 'PurchaseDemandDetailId'));
            dr.DemandNo = intOf(ci(row, 'PurchaseDemandDocNo'));
            dr.DeliveryChallanId = intOf(ci(row, 'DeliveryChallanHeaderId')); dr.DeliveryChallanDetailId = detailId; dr.DeliveryChallanNo = intOf(ci(row, 'DocNo'));
            dr.ItemId = intOf(ci(row, 'ItemId')); dr.ItemCode = ci(row, 'ItemCode'); dr.ItemName = ci(row, 'ItemName');
            dr.UOMId = intOf(ci(row, 'UomId')); dr.UOM = ci(row, 'Uom'); dr.UomEquivalent = num(ci(row, 'UomEquivalent'));
            if (Object.keys(row).some(function (k) { return k.toLowerCase() === 'itemconditionid'; })) dr.ItemConditionId = intOf(ci(row, 'ItemConditionId'));
            dr.TotalOrderQty = num(ci(row, 'PurchaseOrderQty'));
            dr.TotalDemandQty = num(ci(row, 'PurchaseDemandQty'));
            dr.PurchasePreBillId = intOf(ci(row, 'PurchasePreBillHeaderId')); dr.PurchasePreBillDetailId = intOf(ci(row, 'PurchasePreBillDetailId'));
            dr.PurchasePreBillNo = intOf(ci(row, 'PurchasePreBillDocNo')); dr.PurchasePreBillQty = num(ci(row, 'PurchasePreBillQty'));
            dr.TotalDeliveryChallanQty = num(ci(row, 'ChallanQty')); dr.UsedDeliveryChallanQty = num(ci(row, 'ChallanQtyUsedInGrn'));
            dr.BalDeliveryChallanQty = num(ci(row, 'BalChallanQty'));
            dr.ThisQty = dr.BalDeliveryChallanQty;
            var wh = intOf(ci(row, 'WareHouseId')) > 0 ? intOf(ci(row, 'WareHouseId')) : intOf(look.wareHouseConfigId);
            resolveWh(dr, dr.ItemId, wh);
            dtdetail.push(dr);
        });
        afterLoad();
    }

    /* ------------------------------------------------------------------ tabs / wiring */

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    window.GrnStore = {
        reset: reset, refresh: refresh, refreshFilters: refreshFilters, save: save, update: update, del: del,
        print: function () { printId(Id); }, printId: printId, open: readById, tab: tab,
        cell: cell, delRow: delRow, pick: pick, docDateChanged: docDateChanged,
        showHistory: showHistory, historyDetail: historyDetail, newHistory: newHistory,
        showRegister: showRegister, resetRegister: resetRegister, printRegister: printRegister, poSlip: poSlip,
        closeLoader: closeLoader, loaderSlip: loaderSlip,
        openPo: openPo, poShow: poShow, poChecked: poChecked, poNew: poNew, poRefresh: poRefresh, poLoad: poLoad,
        openDemand: openDemand, pdShow: pdShow, pdChecked: pdChecked, pdNew: pdNew, pdRefresh: pdRefresh, pdLoad: pdLoad,
        openDc: openDc, dcShow: dcShow, dcChecked: dcChecked, dcNew: dcNew, dcRefresh: dcRefresh, dcLoad: dcLoad
    };

    document.addEventListener('DOMContentLoaded', function () {
        $id('DocDate').value = C.today();                                       // designer: DateTime.Now, once
        init();
    });
})();
