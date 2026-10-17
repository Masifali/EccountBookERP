/* ============================================================================================
 * Screen 927 "Issuance Gdn" — Architecture.WinApp.ReturnableInventoryManagement.GdnReturnable
 * (base.Name "GdnReturnable"), DocumentTypeId 1202.
 *
 *   InitializeComponentMethod :330   rights, combos, InvGdnGenerateCode, pending gate passes   (init)
 *   btnRefresh_Click :951            combos again                                                 (refresh)
 *   grdGp "Load" :806 → RowbindFromPendingGpGrid :823 → LoadInGridDetail :1799
 *   BtnDeliveryOrder_Click :1834     frmLoadDeliveryOrderOnGdnReturnable (modal here) → LoadInGridDetail
 *   grdDetail_ColumnButtonClick :675 "Add" copies the row, "Delete" drops it
 *   Insert :983 / btnsave :1125 / btnUpdate :1138 / ReadById :1154 / btnDelete :1228 / Print :1264 / formReset :907
 *   HistoryFill :1340 / DataGridHistory Edit, Print :1501 / SelectionChanged :1543
 *   frmGSIssuance_KeyDown :1866 / MakeShortCutKeys :2002
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc;
    var api = '/inventory/returnable/gdn-returnable/api';
    var RPT = '1203_GdnReturnable_Slip.rpt';

    var look = { rights: {} };
    var recId = 0, gpId = 0;
    var rows = [];               // table
    var gps = [];                // dtGp
    var hist = [], histSel = -1;
    var loader = null;

    function num(v) { var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim(); if (!s) return 0; var n = Number(s); return isNaN(n) ? 0 : n; }
    function fmt(v, p) { return (Number(v) || 0).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: p }); }
    function fail(e) { alert(e && e.message ? e.message : String(e)); }
    function val(id) { return parseInt($id(id).value || '0', 10) || 0; }
    function text(id) { var el = $id(id); return el.selectedIndex >= 0 && el.options.length ? el.options[el.selectedIndex].text : ''; }
    function addDays(d, n) { var x = new Date(d.getFullYear(), d.getMonth(), d.getDate() + n); return x.getFullYear() + '-' + String(x.getMonth() + 1).padStart(2, '0') + '-' + String(x.getDate()).padStart(2, '0'); }
    function show(id, on) { $id(id).classList.toggle('is-hidden', !on); }

    /** InfragisticsHelper.BindAndRetainSelection. */
    function bind(id, list, idKey, nameKey, defaultRow, extra) {
        var el = $id(id), prev = el.value;
        var h = defaultRow ? '<option value="0">' + esc(defaultRow) + '</option>' : '<option value="0"></option>';
        (list || []).forEach(function (r) {
            var t = esc(r[nameKey]);
            if (extra) extra.forEach(function (k) { if (r[k] !== undefined && r[k] !== null && String(r[k]) !== '') t += ' | ' + esc(r[k]); });
            h += '<option value="' + esc(r[idKey]) + '">' + t + '</option>';
        });
        el.innerHTML = h;
        el.value = (list || []).some(function (r) { return String(r[idKey]) === prev; }) ? prev : '0';
    }

    // ------------------------------------------------------------------------------ load

    function init() {
        $id('txtDocdate').value = C.today();
        $id('datReturnableDate').value = C.today();
        renderGrid();
        C.getJson(api + '/lookups').then(function (d) {
            look = d;
            var r = d.rights || {};
            $id('btnsave').disabled = !r.save;                        // :362-366
            $id('btnUpdate').disabled = !r.update;
            $id('Print').disabled = !r.print;
            $id('btnDelete').disabled = !r.delete;
            if (d.docNo > 0) $id('txtDocNo').value = d.docNo;
            applyLookups(d);
            renderGp(d.pendingGatePasses || []);
            historyCombosBind(d.historyCombos);
            var days = d.defaultDaysToLessFromHistoryFromDate > 0 ? d.defaultDaysToLessFromHistoryFromDate : 3;    // :390
            $id('FromDateHistory').value = addDays(new Date(), -days);
            $id('ToDateHistory').value = C.today();
            $id('txtDocdate').focus();
        }).catch(fail);
        document.addEventListener('keydown', keyDown);
    }

    function applyLookups(d) {
        bind('CmbType', d.deliveryTypes, 'Id', 'Name');                                                     // :459
        bind('CmbCustomer', d.customers, 'Id', 'CompanyName', null, ['PartyCode', 'CityName', 'MobileNo']); // :497
        bind('CmbReferenceParty', d.referenceParties, 'Id', 'Name');                                        // :534
        bind('CmbRequested', d.requestedBy, 'Id', 'Name');                                                  // :558
        bind('CmbTransporter', d.transporters, 'Id', 'AccountTitle', null, ['AccountCode']);               // :605
        bind('cmbvehicletype', d.vehicleTypes, 'Id', 'Name');                                               // :546
        if (d.warehouses) look.warehouses = d.warehouses;
        if (d.transporters) look.transporters = d.transporters;
        if (d.customers) look.customers = d.customers;
        if (d.deliveryTypes) look.deliveryTypes = d.deliveryTypes;
    }

    /** btnRefresh_Click :951. */
    function refresh() { C.getJson(api + '/refresh').then(applyLookups).catch(fail); }

    // ------------------------------------------------------------------------------ detail grid

    /* DetailGridSettings :654 — hidden Id, DeliveryOrderId, DeliveryOrderDetailId, ItemId, PackUomId, IsAssetItem; WareHouseId combo. */
    var COLS = [
        { key: 'DeliveryOrderNo', cap: 'DeliveryOrderNo', cls: 'ctr' }, { key: 'WareHouseId', cap: 'WareHouseName', combo: true },
        { key: 'ItemCategory', cap: 'ItemCategory' }, { key: 'ItemCode', cap: 'ItemCode' }, { key: 'ItemName', cap: 'ItemName' },
        { key: 'PackUom', cap: 'PackUom' }, { key: 'ItemDescription', cap: 'ItemDescription' }, { key: 'ItemQty', cap: 'ItemQty', qty: true }
    ];
    function renderGrid() {
        var t = $id('grdDetail');
        t.tHead.innerHTML = '<tr>' + COLS.map(function (c) { return '<th>' + esc(c.cap) + '</th>'; }).join('') + '<th style="width:20px">+</th><th style="width:20px">X</th></tr>';
        t.tBodies[0].innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '">' + COLS.map(function (c) {
                if (c.combo) return '<td><select class="cell" data-w="' + i + '">' + (look.warehouses || []).map(function (w) { return '<option value="' + w.Id + '"' + (w.Id === r.WareHouseId ? ' selected' : '') + '>' + esc(w.WareHouseName) + '</option>'; }).join('') + '</select></td>';
                if (c.qty) return '<td class="num"><input type="text" class="qty" data-q="' + i + '" value="' + esc(r.ItemQty) + '"></td>';
                return '<td' + (c.cls ? ' class="' + c.cls + '"' : '') + '>' + esc(r[c.key]) + '</td>';
            }).join('') + '<td class="ctr"><button type="button" class="cx-link" data-add="' + i + '">+</button></td><td class="ctr"><button type="button" class="cx-x" data-x="' + i + '">X</button></td></tr>';
        }).join('');
        total();
        t.tBodies[0].querySelectorAll('[data-w]').forEach(function (s) { s.onchange = function () { rows[parseInt(s.getAttribute('data-w'), 10)].WareHouseId = parseInt(s.value, 10) || 0; }; });
        t.tBodies[0].querySelectorAll('[data-q]').forEach(function (inp) { inp.onchange = function () { rows[parseInt(inp.getAttribute('data-q'), 10)].ItemQty = num(inp.value); total(); }; });
        t.tBodies[0].querySelectorAll('[data-add]').forEach(function (b) { b.onclick = function () { addCopy(parseInt(b.getAttribute('data-add'), 10)); }; });
        t.tBodies[0].querySelectorAll('[data-x]').forEach(function (b) { b.onclick = function () { removeRow(parseInt(b.getAttribute('data-x'), 10)); }; });
    }
    function total() {
        var s = rows.reduce(function (a, r) { return a + num(r.ItemQty); }, 0);
        $id('grdDetail').tFoot.innerHTML = rows.length ? '<tr>' + COLS.map(function (c) { return '<td class="num">' + (c.qty ? fmt(s, 3) : '') + '</td>'; }).join('') + '<td></td><td></td></tr>' : '';
    }
    /** "Add" :685 — a copy of the row (Id 0 while updating). */
    function addCopy(i) {
        var r = rows[i]; if (!r) return;
        var c = Object.assign({}, r);
        if (!$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) c.Id = 0;
        rows.push(c); renderGrid();
    }
    /** "Delete" :695 — drop the row; an empty grid re-enables Type and Customer. */
    function removeRow(i) {
        rows.splice(i, 1); renderGrid();
        if (!rows.length) { $id('CmbType').disabled = false; $id('CmbCustomer').disabled = false; }
    }

    // ------------------------------------------------------------------------------ pending gate passes

    var GPCOLS = ['OrderType', 'DeliveryType', 'CustomerName', 'OrderNo', 'GpDate', 'GpSrNo', 'VehicleNo', 'BiltyNo', 'Qty', 'Freight', 'Status', 'RequestedBy', 'ApprovedBy'];
    function renderGp(list) {
        gps = list || [];
        var t = $id('grdGp');
        t.tHead.innerHTML = '<tr><th>Load</th>' + GPCOLS.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = gps.map(function (g, i) {
            return '<tr><td class="ctr"><button type="button" class="cx-link" data-l="' + i + '">Load</button></td>' + GPCOLS.map(function (c) {
                if (c === 'Qty' || c === 'Freight') return '<td class="num">' + esc(fmt(g[c], 3)) + '</td>';
                if (c === 'GpDate') return '<td>' + esc(C.gridDate(g.GpDate)) + '</td>';
                return '<td>' + esc(g[c]) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = gps.length ? '<tr><td></td>' + GPCOLS.map(function (c) { return '<td class="num">' + ((c === 'Qty' || c === 'Freight') ? fmt(gps.reduce(function (a, g) { return a + num(g[c]); }, 0), 3) : '') + '</td>'; }).join('') + '</tr>' : '';
        t.tBodies[0].querySelectorAll('[data-l]').forEach(function (b) { b.onclick = function () { rowbindFromPendingGp(gps[parseInt(b.getAttribute('data-l'), 10)]); }; });
    }

    /** RowbindFromPendingGpGrid :823. */
    function rowbindFromPendingGp(item) {
        try {
            if ($id('btnsave').classList.contains('is-hidden') || $id('btnsave').disabled) throw new Error('Please reset the form first...');
            if (!item) throw new Error('Invalid row selected.');
            if (rows.length && !confirm('Are you sure to load? Existing detail entries will be lost.')) return;
            if (item.Status !== 'Accepted') throw new Error('Status not accepted. Please check status.');
            $id('txtGpNo').disabled = true; $id('txtVehicleNo').disabled = true; $id('txtBiltyNo').disabled = true;
            var typeId = item.DeliveryTypeId | 0, editable = typeId !== 1;
            $id('CmbType').disabled = !editable; $id('CmbCustomer').disabled = !editable;
            $id('CmbType').value = typeId > 0 ? String(typeId) : '0';
            gpId = item.OutwardGatePassId | 0;
            $id('txtGpNo').value = item.GpSrNo;
            $id('txtFreightAmount').value = fmt(item.Freight, 4);
            $id('txtVehicleNo').value = item.VehicleNo || '';
            $id('txtBiltyNo').value = item.BiltyNo || '';
            $id('CmbCustomer').value = String(item.SupplierCustomerId);
            rows = [];
            C.getJson(api + '/pending-delivery-order-rows' + C.qs({ customerId: val('CmbCustomer'), deliveryTypeId: typeId, deliveryOrderId: item.DeliverOrderId, fromGatePass: true }))
                .then(loadInGridDetail).catch(fail);
        } catch (e) { fail(e); }
    }

    /** LoadInGridDetail :1799. */
    function loadInGridDetail(dt) {
        try {
            if (dt && dt.length) {
                if (rows.length && (rows[0].DeliveryOrderId | 0) !== (dt[0].Id | 0)) throw new Error("Already Loaded Row's Have Different Order. So you Can't Load Rows Of Different Order!");
                $id('CmbCustomer').disabled = true; $id('CmbType').disabled = true;
                $id('CmbCustomer').value = String(dt[0].SupplierCustomerId);
                $id('CmbType').value = String(dt[0].DeliveryTypeId);
                var existing = {};
                rows.forEach(function (r) { existing[r.DeliveryOrderDetailId | 0] = 1; });
                dt.forEach(function (d) {
                    if (existing[d.DetailId]) return;
                    existing[d.DetailId] = 1;
                    rows.push({ Id: 0, DeliveryOrderId: d.Id, DeliveryOrderDetailId: d.DetailId, DeliveryOrderNo: d.DocNo, WareHouseId: d.WarehouseId,
                        ItemCategoryId: d.ItemCategoryId, ItemCategory: d.ItemCategory, ItemId: d.ItemId, ItemCode: d.ItemCode, ItemName: d.ItemName,
                        PackUomId: d.PackUomId, PackUom: d.PackUomCode, ItemDescription: d.ItemDescription, ItemQty: d.Qty, IsAssetItem: !!d.IsAssetItem });
                });
            }
            renderGrid();
        } catch (e) { fail(e); }
    }

    // ------------------------------------------------------------------------------ Load Delivery Order dialog

    /** BtnDeliveryOrder_Click :1834. */
    function loadDeliveryOrder() {
        if (rows.length && !((rows[0].DeliveryOrderId | 0) > 0)) { alert('You cannot load a Delivery Order because a Direct Entry already exists.'); return; }
        loader = { rows: [] };
        bind('ldCustomer', look.customers, 'Id', 'CompanyName', 'Customer');
        bind('ldType', look.deliveryTypes, 'Id', 'Name', 'Delivery Type');
        $id('ldCustomer').value = String(val('CmbCustomer'));
        $id('ldType').value = String(val('CmbType'));
        C.openModal('dlgLoadDo');
        loadDoSearch();
    }
    var LCOLS = ['DocDate', 'DocNo', 'DeliveryOrderType', 'WareHouseName', 'ItemCategory', 'ItemCode', 'ItemName', 'PackUomCode', 'ItemDescription', 'Qty'];
    function loadDoSearch() {
        C.getJson(api + '/pending-delivery-order-rows' + C.qs({ customerId: val('ldCustomer') || '', deliveryTypeId: val('ldType') || '' })).then(function (list) {
            loader.rows = list || [];
            var t = $id('grdLoadDo');
            t.tHead.innerHTML = '<tr><th><input type="checkbox" id="ldAll"></th>' + LCOLS.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = loader.rows.map(function (r, i) {
                return '<tr><td class="ctr"><input type="checkbox" data-c="' + i + '"></td>' + LCOLS.map(function (c) {
                    if (c === 'Qty') return '<td class="num">' + fmt(r.Qty, 3) + '</td>';
                    if (c === 'DocDate') return '<td>' + esc(C.gridDate(r.DocDate)) + '</td>';
                    return '<td>' + esc(r[c]) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            $id('ldAll').onchange = function () { t.tBodies[0].querySelectorAll('[data-c]').forEach(function (c) { c.checked = $id('ldAll').checked; }); };
        }).catch(fail);
    }
    function loadDoClose(load) {
        var picked = [];
        if (load && loader) $id('grdLoadDo').tBodies[0].querySelectorAll('[data-c]').forEach(function (c) { if (c.checked) picked.push(loader.rows[parseInt(c.getAttribute('data-c'), 10)]); });
        C.closeModal('dlgLoadDo');
        loader = null;
        if (load && !picked.length) { alert('Check the row first'); return; }
        loadInGridDetail(picked);
    }

    // ------------------------------------------------------------------------------ save

    function body() {
        var tr = (look.transporters || []).filter(function (x) { return x.Id === val('CmbTransporter'); })[0];
        return {
            Id: recId, DocDate: $id('txtDocdate').value, DeliveryTypeId: val('CmbType'), SupplierCustomerId: val('CmbCustomer'),
            ReferencePartyId: val('CmbReferenceParty'), VehicleTypeId: val('cmbvehicletype'), VehicleType: val('cmbvehicletype') ? text('cmbvehicletype') : '',
            VehicleNo: $id('txtVehicleNo').value, BiltyNo: $id('txtBiltyNo').value, RemarksHeader: $id('txtRemarks').value,
            ReturnableDate: $id('datReturnableDate').value, ReferenceDocNo: $id('txtManualNo').value, OutwardGatePassId: gpId,
            GpNo: num($id('txtGpNo').value), RequestedByLookUpId: val('CmbRequested'), TransporterId: val('CmbTransporter'),
            TransporterSupCustId: tr ? (tr.SupplierCustomerId | 0) : 0, FreightAmount: num($id('txtFreightAmount').value),
            rows: rows
        };
    }

    /** FormValidation :613. */
    function formValidation() {
        var dn = $id('txtDocNo').value.trim();
        if (dn === '' || num(dn) === 0) { alert('DocNo Field Required'); $id('txtDocNo').focus(); return false; }
        if (val('CmbType') <= 0) { alert('Delivery Type Field Required'); $id('CmbType').focus(); return false; }
        if (val('CmbCustomer') <= 0) { alert('Customer Field Required'); $id('CmbCustomer').focus(); return false; }
        if (val('CmbRequested') <= 0) { alert('RequestedBy Field Required'); $id('CmbRequested').focus(); return false; }
        var fr = num($id('txtFreightAmount').value);
        if (val('CmbTransporter') > 0 && !(fr > 0)) { alert('Freight Amount Field is Required'); $id('txtFreightAmount').focus(); return false; }
        if (fr > 0 && val('CmbTransporter') <= 0) { alert('Transporter Field is Required'); $id('CmbTransporter').focus(); return false; }
        return true;
    }

    /** Insert :983. */
    function insert() {
        if (!rows.length) { alert('Grid record not found'); return; }
        if (!formValidation()) return;
        if (!confirm(recId === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return;
        C.postJson(api + '/save', body()).then(function (res) {
            alert(res.message);
            var id = res.id;
            formReset();
            if ($id('ChkBox').checked) printSlip(id);                                            // :1114
        }).catch(fail);
    }
    function save() { recId = 0; insert(); }
    function update() { insert(); }

    /** btnDelete_Click :1228. */
    function del() {
        if (!(recId > 0)) { alert('Record Not Found'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + recId + '/delete', {}).then(function (res) { alert(res.message); formReset(); }).catch(fail);
    }

    /** Print_Click :1264 → GdnReturnable_Slip_1203 → 1203_GdnReturnable_Slip.rpt. */
    function printSlip(id) { window.printRpt(RPT, { id: id, documentTypeId: 1202 }); }
    function print() { if (recId > 0) printSlip(recId); else alert('No Record Found'); }

    // ------------------------------------------------------------------------------ read / reset

    /** ReadById :1154. */
    function readById(id) {
        formReset(false);
        C.getJson(api + '/' + id).then(function (h) {
            recId = h.Id;
            tab('tabPage1');
            show('btnsave', false); show('btnUpdate', true); show('btnDelete', true);
            $id('txtDocNo').value = h.DocNo;
            $id('txtDocdate').value = C.isoDay(h.DocDate);
            var first = (h.rows || [])[0];
            if (first) $id('CmbType').value = String(first.DeliveryTypeId);
            $id('CmbCustomer').value = String(h.SupplierCustomerId);
            $id('CmbReferenceParty').value = String(h.ReferencePartyId);
            $id('cmbvehicletype').value = String(h.VehicleTypeId);
            $id('txtVehicleNo').value = h.VehicleNo || '';
            $id('txtBiltyNo').value = h.BiltyNo || '';
            $id('txtRemarks').value = h.RemarksHeader || '';
            if (h.ReturnableDate) $id('datReturnableDate').value = C.isoDay(h.ReturnableDate);
            $id('txtManualNo').value = h.ReferenceDocNo || '';
            $id('txtGpNo').value = h.GpNo;
            gpId = h.OutwardGatePassId | 0;
            if (gpId > 0) { $id('txtGpNo').disabled = true; $id('txtVehicleNo').disabled = true; $id('txtBiltyNo').disabled = true; }
            $id('CmbRequested').value = String(h.RequestedByLookUpId);
            if (h.TransporterId > 0) $id('CmbTransporter').value = String(h.TransporterId);          // :1198 (the combo value is the GL account in both modes)
            $id('txtFreightAmount').value = fmt(h.CarriageAmount, 3);
            rows = (h.rows || []).map(function (d) { return Object.assign({}, d); });
            renderGrid();
        }).catch(function (e) { if (e && e.message === 'Record Not Found') return; fail(e); });
    }

    /** formReset :907. */
    function formReset(reloadGp) {
        gpId = 0; recId = 0;
        show('btnsave', true); show('btnUpdate', false); show('btnDelete', false);
        ['CmbType', 'CmbCustomer', 'txtGpNo', 'txtVehicleNo', 'txtBiltyNo'].forEach(function (id) { $id(id).disabled = false; });
        $id('CmbType').selectedIndex = 0;
        ['CmbCustomer', 'CmbReferenceParty', 'cmbvehicletype', 'CmbRequested', 'CmbTransporter'].forEach(function (id) { $id(id).value = '0'; });
        ['txtVehicleNo', 'txtBiltyNo', 'txtRemarks', 'txtManualNo', 'txtGpNo', 'txtFreightAmount'].forEach(function (id) { $id(id).value = ''; });
        rows = []; renderGrid();
        if (reloadGp !== false) C.getJson(api + '/pending-gate-passes').then(renderGp).catch(fail);              // :941
        C.getJson(api + '/doc-no').then(function (d) { if (d && d.docNo > 0) $id('txtDocNo').value = d.docNo; }).catch(fail);
        $id('txtDocdate').focus();
    }

    // ------------------------------------------------------------------------------ history

    function historyCombosBind(d) {
        d = d || {};
        bind('CmbCustomerHistory', d.Supplier, 'Id', 'Name', 'Customer');
        bind('CmbRequestedByHistory', d.RequestedBy, 'Id', 'Name', 'Requested By');
    }
    function refreshHistory() { C.getJson(api + '/history-combos').then(historyCombosBind).catch(fail); }
    /** btnResetHistory_Click :1308. */
    function resetHistory() {
        $id('FromDateHistory').value = addDays(new Date(), -3); $id('ToDateHistory').value = C.today();
        $id('FromDocNoHistory').value = ''; $id('ToDocNoHistory').value = '';
        $id('CmbCustomerHistory').value = '0'; $id('CmbRequestedByHistory').value = '0';
        hist = []; histSel = -1; renderHistory(); clearTable('grddetailofmain');
        $id('drdocdate').checked = true;
    }
    function clearTable(id) { var t = $id(id); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; if (t.tFoot) t.tFoot.innerHTML = ''; }
    function dateType() { var r = document.querySelector('input[name="rdHistDate"]:checked'); return r ? r.value : 'doc'; }

    /** HistoryFill :1340. */
    function showHistory() {
        C.postJson(api + '/history', {
            dateType: dateType(), fromDate: $id('FromDateHistory').value, toDate: $id('ToDateHistory').value,
            fromDocNo: num($id('FromDocNoHistory').value), toDocNo: num($id('ToDocNoHistory').value),
            supplierCustomerId: val('CmbCustomerHistory'), requestedById: val('CmbRequestedByHistory'), isRefered: num($id('RadReferred').value)
        }).then(function (list) { hist = list || []; histSel = -1; renderHistory(); }).catch(fail);
    }
    var HCOLS = ['DocNo', 'DocDate', 'Type', 'Customer', 'OrderNo', 'InvoiceNo', 'RequestedBy', 'ManualNo', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'ApprovedUser', 'ApprovedDate', 'Remarks', 'GdnStatus', 'NoOfAttachments'];
    function renderHistory() {
        var t = $id('DataGridHistory');
        t.tHead.innerHTML = '<tr><th>Edit</th><th>Print</th>' + HCOLS.map(function (c) { return '<th>' + (c === 'Customer' ? 'CustomerName' : c) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = hist.map(function (h, i) {
            return '<tr data-i="' + i + '"' + (i === histSel ? ' class="is-selected"' : '') + '>' +
                '<td class="ctr"><button type="button" class="cx-link" data-b="Edit" data-i="' + i + '">Edit</button></td>' +
                '<td class="ctr"><button type="button" class="cx-link" data-b="Print" data-i="' + i + '">Print</button></td>' +
                HCOLS.map(function (c) {
                    var v = h[c];
                    if (c === 'DocDate') v = C.gridDate(v);
                    if (c === 'EntryDate' || c === 'ModifyDate' || c === 'ApprovedDate') v = C.gridDateTime(v, false);
                    if (c === 'NoOfAttachments') return '<td class="lnk" title="Attachments are not ported to the web page">' + esc(v) + '</td>';
                    return '<td' + (c === 'DocNo' ? ' class="ctr"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) {
            var i = parseInt(tr.getAttribute('data-i'), 10);
            tr.onclick = function () { selectHistory(i); };
            tr.ondblclick = function () { readById(hist[i].Id); };                                   // :1488
        });
        t.tBodies[0].querySelectorAll('[data-b]').forEach(function (b) {
            b.onclick = function (e) {
                e.stopPropagation();
                var h = hist[parseInt(b.getAttribute('data-i'), 10)];
                if (b.getAttribute('data-b') === 'Print') printSlip(h.Id); else readById(h.Id);        // :1509-1517
            };
        });
    }
    /** DataGridHistory_SelectionChanged :1543. */
    function selectHistory(i) {
        if (histSel === i) return;
        histSel = i;
        $id('DataGridHistory').tBodies[0].querySelectorAll('tr').forEach(function (tr, k) { tr.classList.toggle('is-selected', k === i); });
        C.getJson(api + '/' + hist[i].Id).then(function (po) {
            var transfer = hist[i].Type === 'Transfer';
            var cols = [['WareHouse', transfer ? 'WareHouse From' : 'WareHouse'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'], ['ItemDescription', 'ItemDescription'],
                ['CommentsDetail', 'Specification/Remarks'], ['JobLot', 'JobLot'], ['PackUom', 'UOM'], ['ItemQty', 'ItemQty'], ['AssetName', 'AssetName']];
            if (transfer) cols.push(['WarehouseTo', 'WarehouseTo']);
            var t = $id('grddetailofmain'), list = po.rows || [];
            t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = list.map(function (r) { return '<tr>' + cols.map(function (c) { return c[0] === 'ItemQty' ? '<td class="num">' + fmt(r.ItemQty, 2) + '</td>' : '<td>' + esc(r[c[0]]) + '</td>'; }).join('') + '</tr>'; }).join('');
            var s = list.reduce(function (a, r) { return a + num(r.ItemQty); }, 0);
            t.tFoot.innerHTML = '<tr>' + cols.map(function (c) { return '<td class="num">' + (c[0] === 'ItemQty' ? fmt(s, 2) : '') + '</td>'; }).join('') + '</tr>';
        }).catch(function (e) { if (e && e.message === 'Record Not Found') return; fail(e); });
    }

    // ------------------------------------------------------------------------------ tabs / keys

    function tab(name) {
        document.querySelectorAll('.r42-tabs:not(.inner) .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === name); });
        $id('tabPage1').classList.toggle('is-active', name === 'tabPage1');
        $id('tabPage2').classList.toggle('is-active', name === 'tabPage2');
        if (name === 'tabPage2') $id('FromDateHistory').focus();                                        // :1285
    }
    function onForm() { return $id('tabPage1').classList.contains('is-active'); }

    /** MakeShortCutKeys :2002. */
    function shortcutKeys() {
        alert(['Ctrl+S  When In Form For Save Else For Show Data in History', 'Ctrl+U  For Update', 'Ctrl+E  For Close', 'Ctrl+R  For Refresh',
            'Ctrl+N  For New', 'Ctrl+P  For Print', 'Ctrl+F5  For Focus on Do cDate', 'Ctrl+F10  For Open Attachments', 'Ctrl+T  For Tab Transfer',
            'Ctrl+alt  To Show ShortCut Keys Form', 'Ctrl+ArrowDown  For Focus On Detail Grid', 'Ctrl+ArrowUp  For Focus On Warehouse in Detail Box',
            'Ctrl+Enter  When Focus On Any Grid For Update Record', 'Ctrl+Space  When Focus On Any Grid To Call Function\'s On Button Or Link'].join('\n'));
    }
    /** frmGSIssuance_KeyDown :1866. */
    function keyDown(e) {
        if (e.key === 'Escape' && loader) { e.preventDefault(); loadDoClose(false); return; }
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase();
        if (e.altKey) { e.preventDefault(); shortcutKeys(); return; }
        if (k === 's') { e.preventDefault(); if (onForm()) { if (!$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled) save(); } else showHistory(); }
        else if (k === 'e') { e.preventDefault(); window.location.href = '/app/Inventory?module=42'; }
        else if (k === 'n') { e.preventDefault(); if (onForm()) formReset(); else resetHistory(); }
        else if (k === 't') { e.preventDefault(); if (onForm()) tab('tabPage2'); else { tab('tabPage1'); $id('txtDocdate').focus(); } }
        else if (k === 'u') { if (!$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) { e.preventDefault(); update(); } }
        else if (k === 'p') { e.preventDefault(); print(); }
        else if (k === 'r') { e.preventDefault(); refresh(); }
        else if (k === 'f5') { e.preventDefault(); $id('txtDocdate').focus(); }
        else if (k === 'arrowdown') { e.preventDefault(); var tr = (onForm() ? $id('grdDetail') : $id('DataGridHistory')).tBodies[0].querySelector('tr'); if (tr) tr.focus(); }
        else if (k === 'arrowup') { e.preventDefault(); if (!onForm()) $id('FromDateHistory').focus(); }
    }

    document.addEventListener('DOMContentLoaded', init);

    window.GdnRet = {
        newClick: function () { formReset(); }, refresh: refresh, save: save, update: update, del: del, print: print, tab: tab,
        loadDeliveryOrder: loadDeliveryOrder, loadDoSearch: loadDoSearch, loadDoClose: loadDoClose, shortcutKeys: shortcutKeys,
        showHistory: showHistory, resetHistory: resetHistory, refreshHistory: refreshHistory
    };
})();
