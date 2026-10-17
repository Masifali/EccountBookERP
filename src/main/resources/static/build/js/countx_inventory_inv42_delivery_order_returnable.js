/* ============================================================================================
 * Screen 923 "Delivery Order" — Architecture.WinApp.ReturnableInventoryManagement.DeliveryOrder_ReturnAble
 * (base.Name "DeliveryOrder_ReturnAble"), DocumentTypeId 1200, DeliveryOrderType "Local".
 *
 *   InitializeComponentMethod :356   rights, combos, GenerateCode                      (init)
 *   btnRefresh_Click :1415           every combo again                                  (refresh)
 *   CmbItemCategory_Leave :2412      items of the category (asset register when parent 5)
 *   CmbItemName_Leave :813           UOMs of the item (asset UOM when parent 5)
 *   btnplus_Click :946 / grd_DoubleClick :1145 / btnUpdateDetail_Click :1115 / btnCancelUpdateDetial_Click :1104
 *   grd_ColumnButtonClick :1028      "+" copies the row, "X" → lstRemoveRecord (saved rows) / drop
 *   Insert :1173                     Save / Update / SaveAs (:1320) → print on ChkPrint
 *   ReadById :1359 / Reset :1441
 *   gridhistoryfill :1839 / grdhistory buttons Edit, Print, SaveAs :2056 / SelectionChanged :2115
 *   PurchsaeOrder_KeyDown :1564      shortcuts
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc;
    var api = '/inventory/returnable/delivery-order-returnable/api';
    var RPT = '1200-DeliveryOrderSlipReturnable.rpt';

    var look = { rights: {} };
    var recId = 0, isApproved = false;
    var rows = [];                 // table
    var removed = [];              // lstRemoveRecord (cleared by Reset :1447)
    var updateDetailIndex = -1;
    var dtitem = [];               // dtitem of the selected category
    var hist = [], histSel = -1;

    function num(v) { var s = String(v === null || v === undefined ? '' : v).replace(/,/g, '').trim(); if (!s) return 0; var n = Number(s); return isNaN(n) ? 0 : n; }
    function fmt(v, p) { return (Number(v) || 0).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: p }); }
    function fail(e) { alert(e && e.message ? e.message : String(e)); }
    function val(id) { return parseInt($id(id).value || '0', 10) || 0; }
    function text(id) { var el = $id(id); return el.selectedIndex >= 0 && el.options.length ? el.options[el.selectedIndex].text : ''; }
    function addDays(d, n) { var x = new Date(d.getFullYear(), d.getMonth(), d.getDate() + n); return x.getFullYear() + '-' + String(x.getMonth() + 1).padStart(2, '0') + '-' + String(x.getDate()).padStart(2, '0'); }

    /** InfragisticsHelper.BindAndRetainSelection: optional default row, keep the value when it still exists. */
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
        $id('DocDate').value = C.today();
        $id('datReturnableDate').value = C.today();
        renderGrid();
        C.getJson(api + '/lookups').then(function (d) {
            look = d;
            var r = d.rights || {};
            $id('btnsave').disabled = !r.save;                 // :384
            $id('btnSaveAs').disabled = !r.save;
            $id('btnupdate').disabled = !r.update;
            $id('btnprint').disabled = !r.print;
            if (d.itemSearchByCode) $id('rdbtnItemCode').checked = true; else $id('rdbtnItemName').checked = true;   // :392
            if (d.docNo > 0) $id('txtdocno').value = d.docNo;
            applyLookups(d);
            historyComboBind(d.historyCombos);
            var days = d.defaultDaysToLessFromHistoryFromDate > 0 ? d.defaultDaysToLessFromHistoryFromDate : 3;   // :424
            $id('FromDateHistory').value = addDays(new Date(), -days);
            $id('ToDateHistory').value = C.today();
            $id('DocDate').focus();
        }).catch(fail);
        document.addEventListener('keydown', keyDown);
    }

    function applyLookups(d) {
        bind('CmbDeliveryType', d.deliveryTypes, 'Id', 'Name');                                 // :535 no default row
        bind('cmbvehicletype', d.vehicleTypes, 'Id', 'Name', 'Vehicle Type');                  // :629 BindDDLNew
        bind('CmbSupplierCustomer', d.customers, 'Id', 'CompanyName', null, ['PartyCode', 'CityName', 'MobileNo']);   // :577
        bind('CmbReferenceParty', d.referenceParties, 'Id', 'Name');                           // :614
        bind('CmbWareHouse', d.warehouses, 'Id', 'WareHouseName', '');                         // :662 insertDefaultRow
        bind('CmbItemCategory', d.itemCategories, 'Id', 'CategoryDescription', '');            // :742
        bind('CmbRequestedBy', d.lookups13, 'Id', 'Name', 'Requested By');                     // :687 ZeroIndex
        bind('CmbApprovedBy', d.lookups13, 'Id', 'Name', 'Approved By');
        look.items = d.items; look.assetItems = d.assetItems; look.itemCategories = d.itemCategories; look.warehouses = d.warehouses;
        itemCategoryLeave();
    }

    /** btnRefresh_Click :1415. */
    function refresh() {
        C.getJson(api + '/refresh').then(function (d) { applyLookups(d); }).catch(fail);
    }

    function categoryRow() {
        var id = val('CmbItemCategory');
        return (look.itemCategories || []).filter(function (c) { return c.Id === id; })[0] || null;
    }

    /** CmbItemCategory_Leave :2412 — parent 5 → asset register rows of the category; else getGlobalAllItems of the category. */
    function itemCategoryLeave() {
        var cat = categoryRow(), id = cat ? cat.Id : 0;
        dtitem = [];
        if (cat && id > 0 && cat.ParentCategoryId === 5) {
            (look.assetItems || []).forEach(function (a) {
                if (a.ItemCategoryId === id) dtitem.push({ Id: a.assetRegisterId, ItemName: a.assetName, ItemCode: a.assetCode });
            });
        } else {
            (look.items || []).forEach(function (i) { if (i.ItemCategoryId === id) dtitem.push({ Id: i.Id, ItemName: i.ItemName, ItemCode: i.ItemCode }); });
        }
        itemNameBind();
    }

    /** ItemNameBind :784 — display Item Name or Item Code (radio). */
    function itemNameBind() {
        var byName = $id('rdbtnItemName').checked;
        bind('CmbItemName', dtitem, 'Id', byName ? 'ItemName' : 'ItemCode', '', [byName ? 'ItemCode' : 'ItemName']);
    }

    /** CmbItemName_Leave :813. */
    function itemNameLeave() {
        var cat = categoryRow(), itemId = val('CmbItemName');
        if (!cat) return;
        if (cat.ParentCategoryId !== 5) {
            if (itemId > 0) {
                C.getJson(api + '/uoms' + C.qs({ itemId: itemId })).then(function (list) {
                    bind('CmbPackUom', list, 'Id', 'Name', '');
                    if (list && list.length) $id('CmbPackUom').value = String(list[0].Id);     // ActivateRow: true
                }).catch(fail);
            } else { $id('CmbPackUom').innerHTML = ''; }
            return;
        }
        var a = (look.assetItems || []).filter(function (x) { return x.assetRegisterId === itemId; })[0];
        if (a) { bind('CmbPackUom', [{ Id: a.UomId, Name: a.UOMCode }], 'Id', 'Name', ''); $id('CmbPackUom').value = String(a.UomId); }
    }

    // ------------------------------------------------------------------------------ detail box

    /** FormValidationDetail :911. */
    function formValidationDetail() {
        if (val('CmbWareHouse') === 0) { alert('WareHouseName field is required'); $id('CmbWareHouse').focus(); return false; }
        if (val('CmbItemCategory') === 0) { alert('Item Category field is required'); $id('CmbItemCategory').focus(); return false; }
        if (val('CmbItemName') === 0) { alert('ItemName field is required'); $id('CmbItemName').focus(); return false; }
        if (val('CmbPackUom') === 0) { alert('PackUOM field is required'); $id('CmbPackUom').focus(); return false; }
        if (num($id('txtqty').value) === 0) { alert('ItemQty field is required'); $id('txtqty').focus(); return false; }
        return true;
    }

    function detailFromBox(id) {
        var cat = categoryRow();
        var item = dtitem.filter(function (i) { return i.Id === val('CmbItemName'); })[0] || {};
        return {
            Id: id || 0, WareHouseId: val('CmbWareHouse'), ItemCategoryId: val('CmbItemCategory'), ItemCategory: text('CmbItemCategory'),
            ItemId: val('CmbItemName'), ItemCode: item.ItemCode || '', ItemName: item.ItemName || '',
            PackUomId: val('CmbPackUom'), PackUom: text('CmbPackUom').trim(), ItemDescription: $id('txtItemDescriptions').value,
            ItemQty: num($id('txtqty').value), IsAssetItem: !!(cat && cat.ParentCategoryId === 5)
        };
    }

    /** btnplus_Click :946. */
    function plus() {
        if (!formValidationDetail()) return;
        rows.push(detailFromBox(0));
        renderGrid();
        resetDetail();
    }

    /** btnUpdateDetail_Click :1115. */
    function updateDetail() {
        if (!formValidationDetail() || updateDetailIndex < 0 || !rows[updateDetailIndex]) return;
        var keep = rows[updateDetailIndex].Id;
        rows[updateDetailIndex] = detailFromBox(keep);
        renderGrid();
        $id('btnplus').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
        resetDetail();
    }

    /** btnCancelUpdateDetial_Click :1104. */
    function cancelUpdateDetail() {
        resetDetail();
        $id('btnplus').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
    }

    /** ResetDetail :1489. */
    function resetDetail() {
        $id('CmbItemName').value = '0';
        $id('CmbPackUom').innerHTML = '';
        $id('txtqty').value = '';
        $id('btnplus').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
        updateDetailIndex = -1;
        $id('CmbItemName').focus();
    }

    /** grd_DoubleClick :1145 — the row back into the Detail box. */
    function editRow(i) {
        var r = rows[i]; if (!r) return;
        updateDetailIndex = i;
        $id('CmbWareHouse').value = String(r.WareHouseId);
        $id('CmbItemCategory').value = String(r.ItemCategoryId);
        itemCategoryLeave();
        $id('CmbItemName').value = String(r.ItemId);
        itemNameLeave();
        $id('txtItemDescriptions').value = r.ItemDescription || '';
        setTimeout(function () { if (!$id('CmbPackUom').options.length || $id('CmbPackUom').value !== String(r.PackUomId)) { bind('CmbPackUom', [{ Id: r.PackUomId, Name: r.PackUom }], 'Id', 'Name', ''); $id('CmbPackUom').value = String(r.PackUomId); } }, 300);
        $id('txtqty').value = r.ItemQty;
        $id('btnplus').classList.add('is-hidden'); $id('btnUpdateDetail').classList.remove('is-hidden'); $id('btnCancelUpdateDetial').classList.remove('is-hidden');
        $id('CmbWareHouse').focus();
    }

    // ------------------------------------------------------------------------------ grid

    /* grdSettings :981 — hidden Id, ItemCategoryId, ItemId, PackUomId, IsAssetItem; WareHouseId is a combo cell; "+" and "X" buttons. */
    var COLS = [
        { key: 'WareHouseId', cap: 'WareHouse Name', combo: true }, { key: 'ItemCategory', cap: 'ItemCategory' },
        { key: 'ItemCode', cap: 'ItemCode' }, { key: 'ItemName', cap: 'ItemName' }, { key: 'PackUom', cap: 'Uom' },
        { key: 'ItemDescription', cap: 'ItemDescription' }, { key: 'ItemQty', cap: 'Qty', qty: true }
    ];
    function renderGrid() {
        var t = $id('grd');
        t.tHead.innerHTML = '<tr>' + COLS.map(function (c) { return '<th>' + esc(c.cap) + '</th>'; }).join('') + '<th style="width:20px">+</th><th style="width:20px">X</th></tr>';
        t.tBodies[0].innerHTML = rows.map(function (r, i) {
            return '<tr data-i="' + i + '">' + COLS.map(function (c) {
                if (c.combo) return '<td><select class="cell" data-w="' + i + '">' + (look.warehouses || []).map(function (w) { return '<option value="' + w.Id + '"' + (w.Id === r.WareHouseId ? ' selected' : '') + '>' + esc(w.WareHouseName) + '</option>'; }).join('') + '</select></td>';
                if (c.qty) return '<td class="num">' + esc(fmt(r.ItemQty, 3)) + '</td>';
                return '<td>' + esc(r[c.key]) + '</td>';
            }).join('') + '<td class="ctr"><button type="button" class="cx-link" data-add="' + i + '">+</button></td><td class="ctr"><button type="button" class="cx-x" data-x="' + i + '">X</button></td></tr>';
        }).join('');
        var s = rows.reduce(function (a, r) { return a + num(r.ItemQty); }, 0);
        t.tFoot.innerHTML = rows.length ? '<tr>' + COLS.map(function (c) { return '<td class="num">' + (c.qty ? fmt(s, 3) : '') + '</td>'; }).join('') + '<td></td><td></td></tr>' : '';
        t.tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) { tr.ondblclick = function () { editRow(parseInt(tr.getAttribute('data-i'), 10)); }; });
        t.tBodies[0].querySelectorAll('[data-w]').forEach(function (s2) { s2.onchange = function () { rows[parseInt(s2.getAttribute('data-w'), 10)].WareHouseId = parseInt(s2.value, 10) || 0; }; });
        t.tBodies[0].querySelectorAll('[data-add]').forEach(function (b) { b.onclick = function () { addCopy(parseInt(b.getAttribute('data-add'), 10)); }; });
        t.tBodies[0].querySelectorAll('[data-x]').forEach(function (b) { b.onclick = function () { removeRow(parseInt(b.getAttribute('data-x'), 10)); }; });
    }

    /** grd_ColumnButtonClick "Add" :1036 — copy of the row; Id 0 while updating. */
    function addCopy(i) {
        var r = rows[i]; if (!r) return;
        var c = Object.assign({}, r);
        if (!$id('btnupdate').classList.contains('is-hidden') && !$id('btnupdate').disabled) c.Id = 0;
        rows.push(c);
        renderGrid();
    }

    /** grd_ColumnButtonClick "Delete" :1050 — a saved row asks and goes to lstRemoveRecord. */
    function removeRow(i) {
        var r = rows[i]; if (!r) return;
        if ((r.Id | 0) > 0) {
            if (!confirm('Are you sure to Delete?')) return;
            removed.push(Object.assign({}, r));
        }
        rows.splice(i, 1);
        renderGrid();
    }

    // ------------------------------------------------------------------------------ save

    function body(saveAs) {
        return {
            Id: recId, saveAs: !!saveAs, DocDate: $id('DocDate').value, DeliveryTypeId: val('CmbDeliveryType'),
            SupplierCustomerId: val('CmbSupplierCustomer'), RefPartyId: val('CmbReferenceParty'),
            VehicleType: text('cmbvehicletype') === 'Vehicle Type' ? '' : text('cmbvehicletype'), VehicleNo: $id('txtVehicleNo').value,
            ReturnableDate: $id('datReturnableDate').value, RequestedByLookUpId: val('CmbRequestedBy'), ApprovedByLookUpId: val('CmbApprovedBy'),
            LoadingInstructions: $id('txtremarks').value,
            rows: rows.map(function (r) { return Object.assign({}, r, { Id: saveAs ? 0 : (r.Id | 0) }); }),
            removed: removed
        };
    }

    /** Insert :1173 — "Grid Record Not Found", FormValidation, confirm, save, message, Reset, print on ChkPrint. */
    function insert(saveAs) {
        if (!rows.length) { alert('Grid Record Not Found'); return; }
        if (!formValidation()) return;
        if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        C.postJson(api + '/save', body(saveAs)).then(function (res) {
            alert(res.message);
            var id = res.id;
            reset();
            if ($id('ChkPrint').checked) printSlip(id);                                       // :1296
        }).catch(fail);
    }

    /** FormValidation :852 — the same order and texts as the desktop. */
    function formValidation() {
        var dn = $id('txtdocno').value.trim();
        if (dn === '' || dn === '0') { alert('DocNo Field is Required'); return false; }
        if (val('CmbDeliveryType') <= 0) { alert('Please Select Delivery Type field First'); $id('CmbDeliveryType').focus(); return false; }
        if (val('CmbSupplierCustomer') <= 0) { alert('Please Select Customer Field '); $id('CmbSupplierCustomer').focus(); return false; }
        if (val('cmbvehicletype') === 0) { alert('Vehicle Type Field is Required'); $id('cmbvehicletype').focus(); return false; }
        if ($id('txtVehicleNo').value === '') { alert('Vehicle No Field is Required'); $id('txtVehicleNo').focus(); return false; }
        if (val('CmbDeliveryType') === 4 && $id('DocDate').value > $id('datReturnableDate').value) { alert('Returnable date can not less than doc date'); $id('datReturnableDate').focus(); return false; }
        if (val('CmbRequestedBy') <= 0) { alert('Requested By Field is Required'); $id('CmbRequestedBy').focus(); return false; }
        if (val('CmbApprovedBy') <= 0) { alert('Approved By Field is Required'); $id('CmbApprovedBy').focus(); return false; }
        return true;
    }

    function save() { recId = 0; insert(false); }                                             // :1307
    function saveAs() { recId = 0; rows.forEach(function (r) { r.Id = 0; }); insert(true); }   // :1320
    function update() {                                                                        // :1343
        if (recId === 0) { alert('Record Not Update because RecId Not Found'); return; }
        insert(false);
    }

    /** CmbDeliveryType_TextChanged :2353 — the rows follow the header type; type 1 is refused without OrderId. */
    function deliveryTypeChanged() {
        if (!rows.length || val('CmbDeliveryType') <= 0) return;
        if (val('CmbDeliveryType') === 1) {
            $id('CmbDeliveryType').value = '2';
            alert("You cannot select delivery type 'Sale Order' because data without OrderId exists in the grid.");
        }
    }

    // ------------------------------------------------------------------------------ print

    /** CommonServices.InvDeliveryOrderSlip_1200(id, 1200) → 1200-DeliveryOrderSlipReturnable.rpt. */
    function printSlip(id) {
        if (!id) { alert('Record Id Not Found'); return; }
        window.printRpt(RPT, { id: id, documentTypeId: 1200 });
    }
    function print() { printSlip(recId); }                                                     // :2253

    // ------------------------------------------------------------------------------ read / reset

    /** ReadById :1359. */
    function readById(id, mode) {
        C.getJson(api + '/' + id).then(function (po) {
            recId = po.Id;
            tab('tabPage1');
            $id('txtdocno').value = po.DocNo;
            $id('DocDate').value = C.isoDay(po.DocDate);
            var first = (po.rows || [])[0];
            if (first) {
                $id('CmbDeliveryType').value = String(first.DeliveryTypeId);
                $id('CmbSupplierCustomer').value = String(first.SupplierCustomerId);
                $id('CmbReferenceParty').value = String(first.RefPartyId);
            }
            var vt = Array.prototype.filter.call($id('cmbvehicletype').options, function (o) { return o.text === po.VehicleType; })[0];
            $id('cmbvehicletype').value = vt ? vt.value : '0';
            $id('txtVehicleNo').value = po.VehicleNo || '';
            $id('CmbRequestedBy').value = String(po.RequestedByLookUpId);
            $id('CmbApprovedBy').value = String(po.ApprovedByLookUpId);
            $id('txtremarks').value = po.LoadingInstructions || '';
            if (po.ReturnableDate) $id('datReturnableDate').value = C.isoDay(po.ReturnableDate);
            isApproved = !!po.IsApproved;
            rows = (po.rows || []).map(function (d) { return Object.assign({}, d); });
            if (mode === 'saveas') rows.forEach(function (r) { r.Id = 0; });                 // :2085-2094
            renderGrid();
            $id('btnsave').classList.add('is-hidden');
            $id('btnupdate').classList.toggle('is-hidden', mode === 'saveas');
            $id('btnSaveAs').classList.toggle('is-hidden', mode !== 'saveas');
            $id('DocDate').focus();
        }).catch(fail);
    }

    /** Reset :1441. */
    function reset() {
        removed = []; recId = 0; isApproved = false; rows = [];
        $id('CmbDeliveryType').selectedIndex = 0;                     // Rows[0].Activate()
        $id('CmbSupplierCustomer').value = '0';
        $id('txtVehicleNo').value = '';
        $id('cmbvehicletype').value = '0';
        $id('CmbRequestedBy').value = '0';
        $id('CmbApprovedBy').value = '0';
        $id('txtremarks').value = '';
        $id('CmbItemName').value = '0';
        $id('CmbPackUom').innerHTML = '';
        $id('txtqty').value = '';
        $id('CmbReferenceParty').value = '0';
        $id('btnsave').classList.remove('is-hidden'); $id('btnupdate').classList.add('is-hidden'); $id('btnSaveAs').classList.add('is-hidden');
        $id('btnplus').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
        renderGrid();
        C.getJson(api + '/doc-no').then(function (d) { if (d && d.docNo > 0) $id('txtdocno').value = d.docNo; }).catch(fail);
        $id('DocDate').focus();
    }

    // ------------------------------------------------------------------------------ history

    /** HistoryComboBind :472 — Party Name / Delivery Type / Requested By / Approved By with a default row. */
    function historyComboBind(d) {
        d = d || {};
        bind('CmbCustomerHistory', d.Customer, 'Id', 'Name', 'Party Name');
        bind('CmbDeliveryTypeHistory', d.DeliveryTypeDetail, 'Id', 'Name', 'Delivery Type');
        bind('CmbRequestedByHistory', d.RequestedBy, 'Id', 'Name', 'Requested By');
        bind('CmbApprovedByHistory', d.ApprovedBy, 'Id', 'Name', 'Approved By');
    }
    /** btnRefreshHistory_Click :2200. */
    function refreshHistory() { C.getJson(api + '/history-combos').then(historyComboBind).catch(fail); }
    /** btnNewHistory_Click :2178. */
    function newHistory() {
        $id('FromDateHistory').value = addDays(new Date(), -3);
        $id('ToDateHistory').value = C.today();
        $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = '';
        ['CmbCustomerHistory', 'CmbRequestedByHistory', 'CmbApprovedByHistory', 'CmbDeliveryTypeHistory'].forEach(function (id) { $id(id).value = '0'; });
        hist = []; histSel = -1; renderHistory(); $id('GrdHistoryDetail').tHead.innerHTML = ''; $id('GrdHistoryDetail').tBodies[0].innerHTML = ''; $id('GrdHistoryDetail').tFoot.innerHTML = '';
        $id('drdocdate').checked = true;
    }
    function dateType() { var r = document.querySelector('input[name="rdHistDate"]:checked'); return r ? r.value : 'doc'; }

    /** gridhistoryfill :1839. */
    function showHistory() {
        C.postJson(api + '/history', {
            dateType: dateType(), fromDate: $id('FromDateHistory').value, toDate: $id('ToDateHistory').value,
            fromDocNo: num($id('txtFromDocNoHistory').value), toDocNo: num($id('txtToDocNoHistory').value),
            deliveryTypeId: val('CmbDeliveryTypeHistory'), supplierCustomerId: val('CmbCustomerHistory'),
            requestedById: val('CmbRequestedByHistory'), approvedById: val('CmbApprovedByHistory')
        }).then(function (list) {
            hist = list || []; histSel = -1;
            renderHistory();
        }).catch(fail);
    }

    var HCOLS = ['DocDate', 'DocNo', 'DeliveryType', 'Customer', 'ReferenceParty', 'VehicleType', 'VehicleNo', 'RequestedBy', 'ApprovedBy',
        'ApprovalStatus', 'EntryUser', 'EntryDate', 'ModifyUser', 'ModifyDate', 'ApprovedUser', 'ApprovedDate', 'Remarks', 'NoOfAttachments'];
    function heads() {                       // one row per Id (:1933)
        var seen = {}, out = [];
        hist.forEach(function (r) { if (!seen[r.Id]) { seen[r.Id] = 1; out.push(r); } });
        return out;
    }
    function renderHistory() {
        var t = $id('grdhistory'), list = heads();
        t.tHead.innerHTML = '<tr><th>Edit</th><th>Print</th><th>SaveAs</th>' + HCOLS.map(function (c) { return '<th>' + c + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = list.map(function (h, i) {
            return '<tr data-i="' + i + '" data-id="' + h.Id + '"' + (i === histSel ? ' class="is-selected"' : '') + '>' +
                '<td class="ctr"><button type="button" class="cx-link" data-b="Edit" data-i="' + i + '">Edit</button></td>' +
                '<td class="ctr"><button type="button" class="cx-link" data-b="Print" data-i="' + i + '">Print</button></td>' +
                '<td class="ctr"><button type="button" class="cx-link" data-b="SaveAs" data-i="' + i + '">SaveAs</button></td>' +
                HCOLS.map(function (c) {
                    var v = h[c];
                    if (c === 'DocDate') v = C.gridDate(v);
                    if (c === 'EntryDate' || c === 'ModifyDate' || c === 'ApprovedDate') v = C.gridDateTime(v, true);
                    if (c === 'NoOfAttachments') return '<td class="lnk" title="Attachments are not ported to the web page">' + esc(v) + '</td>';
                    return '<td' + (c === 'DocNo' ? ' class="ctr"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        t.tBodies[0].querySelectorAll('tr[data-i]').forEach(function (tr) {
            var i = parseInt(tr.getAttribute('data-i'), 10);
            tr.onclick = function () { selectHistory(i, list); };
            tr.ondblclick = function () { readById(list[i].Id, 'edit'); };                       // :2046
        });
        t.tBodies[0].querySelectorAll('[data-b]').forEach(function (b) {
            b.onclick = function (e) {
                e.stopPropagation();
                var h = list[parseInt(b.getAttribute('data-i'), 10)], k = b.getAttribute('data-b');
                if (k === 'Print') printSlip(h.Id);                                                // :2098
                else if (k === 'Edit') { reset(); readById(h.Id, 'edit'); }                       // :2060
                else {                                                                             // :2070
                    if (h.DeliveryType === 'Sale Order') { alert("Record With type 'Sale Order' Can not be Save As"); return; }
                    reset(); readById(h.Id, 'saveas');
                }
            };
        });
    }

    /** grdhistory_SelectionChanged :2115 — the rows of dthistoryfill with the same Id. */
    function selectHistory(i, list) {
        histSel = i;
        $id('grdhistory').tBodies[0].querySelectorAll('tr').forEach(function (tr, k) { tr.classList.toggle('is-selected', k === i); });
        var id = list[i].Id, det = hist.filter(function (r) { return r.Id === id; });
        var cols = [['WareHouseName', 'WareHouse'], ['ItemCategory', 'ItemCategory'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'], ['PackUOM', 'UOM'], ['ItemDiscription', 'ItemDescription'], ['LoadingQty', 'Qty']];
        var t = $id('GrdHistoryDetail');
        t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = det.map(function (r) { return '<tr>' + cols.map(function (c) { return c[0] === 'LoadingQty' ? '<td class="num">' + fmt(r.LoadingQty, 3) + '</td>' : '<td>' + esc(r[c[0]]) + '</td>'; }).join('') + '</tr>'; }).join('');
        var s = det.reduce(function (a, r) { return a + num(r.LoadingQty); }, 0);
        t.tFoot.innerHTML = '<tr>' + cols.map(function (c) { return '<td class="num">' + (c[0] === 'LoadingQty' ? fmt(s, 3) : '') + '</td>'; }).join('') + '</tr>';
    }

    // ------------------------------------------------------------------------------ tabs / keys

    function tab(name) {
        document.querySelectorAll('.r42-tabs .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === name); });
        $id('tabPage1').classList.toggle('is-active', name === 'tabPage1');
        $id('tabPage2').classList.toggle('is-active', name === 'tabPage2');
        if (name === 'tabPage2') $id('FromDateHistory').focus();                                   // :2107
    }
    function onForm() { return $id('tabPage1').classList.contains('is-active'); }

    /** PurchsaeOrder_KeyDown :1564. */
    function keyDown(e) {
        if (e.key === 'Enter' && e.target && /^(INPUT|SELECT)$/.test(e.target.tagName) && e.target.type !== 'checkbox') { /* SendKeys TAB */ return; }
        if (!e.ctrlKey) return;
        var k = e.key.toLowerCase();
        if (k === 'n') { e.preventDefault(); if (onForm()) reset(); else newHistory(); }
        else if (k === 'r') { e.preventDefault(); if (onForm()) refresh(); else refreshHistory(); }
        else if (k === 's') { e.preventDefault(); if (onForm()) { if (!$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled) save(); } else showHistory(); }
        else if (k === 'u') { if (!$id('btnupdate').classList.contains('is-hidden') && !$id('btnupdate').disabled) { e.preventDefault(); update(); } }
        else if (k === 'p') { if (look.rights && look.rights.print) { e.preventDefault(); $id('DocDate').focus(); } }
        else if (k === 'e') { e.preventDefault(); window.location.href = '/app/Inventory?module=42'; }
        else if (k === 'f5') { e.preventDefault(); $id('DocDate').focus(); }
        else if (k === 'f12') { if (!$id('btnSaveAs').classList.contains('is-hidden') && !$id('btnSaveAs').disabled) { e.preventDefault(); saveAs(); } }
        else if (k === 't') { e.preventDefault(); if (onForm()) tab('tabPage2'); else { tab('tabPage1'); $id('DocDate').focus(); } }
        else if (k === 'arrowdown') { e.preventDefault(); var g = onForm() ? $id('grd') : $id('grdhistory'); var tr = g.tBodies[0].querySelector('tr'); if (tr) tr.focus(); }
        else if (k === 'arrowup') { e.preventDefault(); if (onForm()) $id('CmbWareHouse').focus(); else $id('FromDateHistory').focus(); }
    }

    document.addEventListener('DOMContentLoaded', init);

    window.DoRet = {
        newClick: reset, refresh: refresh, save: save, saveAs: saveAs, update: update, print: print, tab: tab,
        itemCategoryLeave: itemCategoryLeave, itemNameBind: itemNameBind, itemNameLeave: itemNameLeave, deliveryTypeChanged: deliveryTypeChanged,
        plus: plus, updateDetail: updateDetail, cancelUpdateDetail: cancelUpdateDetail,
        showHistory: showHistory, newHistory: newHistory, refreshHistory: refreshHistory
    };
})();
