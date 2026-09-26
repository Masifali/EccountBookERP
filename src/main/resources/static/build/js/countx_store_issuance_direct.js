/* ============================================================================================
 * Screen 321 "Store Issuance Direct" — Architecture.WinApp.StoreManagement.StoreIssuanceDirect,
 * DocumentTypeId 452. Rows are typed in the entry bar and added with Add, as on the desktop.
 *
 *   ItemCategoryOrTypeBind:576   Category / Type list built from the items, ItemTypeOfTypeId 14/17
 *   cmbItem_Leave:1002           stock + avg rate, last three rates, UOM, warehouse, rack
 *   BindWarehouseDropdown:1019   one warehouse → chosen; several → the configured default by parent category
 *   BindRacks:1068               one rack → chosen; several → the BaseRackId one
 *   CmbRackName_ValueChanged     picking a rack moves the warehouse to the rack's
 *   FormValidationDetail:1151    Item, Warehouse, Rack, Condition, UOM, Qty, Rate, Amount, Department,
 *                                Fixed Asset, Issuance Type; Category1 57 needs a schedule
 *   CmbCategory1_Leave:920       debit accounts: 57 → AccountType 21, 58 → AccountType 11
 *   DeleteDetailRow:1376         refused while a row is being edited ("Reset Detail First")
 *   txtDocdate_Leave:2207        rates of every grid row re-read at the new date
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, num = C.num, intOf = C.intOf;
    var api = '/api/store/issuance/direct';

    var look = { rights: {}, items: [], uoms: [], racks: [], itemConditions: [] };
    var recId = 0, voucherHeadId = 0;
    var table = [];                 // the grid
    var removed = [];               // lstRemoveRecord
    var updateDetailIndex = -1;
    var loaded = false;             // first Load done — ChkPrintPreview / history dates are set once

    /* ------------------------------------------------------------------ pickers */

    function categoryBind() {
        var byCat = $id('RadCategory').checked, seen = {}, list = [];
        look.items.forEach(function (it) {
            var id = byCat ? it.ItemCategoryId : it.ItemTypeId, d = byCat ? it.ItemCategory : it.ItemType;
            if (!d || seen[d]) return;
            seen[d] = 1; list.push({ Id: id, Description: d });
        });
        C.fillSelect('CmbCategory', list, 'Id', 'Description');
    }
    function filteredItems() {
        var id = intOf($id('CmbCategory').value), byCat = $id('RadCategory').checked;
        return look.items.filter(function (it) {
            return id === 0 || (byCat && it.ItemCategoryId === id) || (!byCat && it.ItemTypeId === id);
        });
    }
    function itemBind() {
        C.fillSelect('cmbItem', filteredItems(), 'Id', $id('rdSearchByName').checked ? 'ItemName' : 'ItemCode');
        itemLeave();
    }
    function selectedItem() {
        var id = intOf($id('cmbItem').value);
        return look.items.find(function (x) { return x.Id === id; }) || null;
    }

    function uomBind(itemId) {                                                  // UomFromGlobalBind:712
        var el = $id('cmbUOM'), keepText = el.selectedOptions[0] ? el.selectedOptions[0].textContent : '';
        var list = look.uoms.filter(function (u) { return u.ItemId === itemId; });
        C.fillSelect(el, list, 'Id', 'UOMCode');
        var byText = Array.prototype.find.call(el.options, function (o) { return keepText && o.textContent === keepText; });
        if (byText) el.value = byText.value; else el.value = '0';
        if (intOf(el.value) !== 0) return;
        if (list.length === 1) el.value = String(list[0].Id);
        else if (list.length > 1) {
            var eq = list.find(function (u) { return num(u.Equivalent) === 1; });
            if (eq) el.value = String(eq.Id);
        }
    }

    function warehouseBind(itemId) {                                            // BindWarehouseDropdown:1019
        var el = $id('CmbWarehouse'), keep = intOf(el.value);
        var whs = C.distinct(look.racks.filter(function (x) { return x.ItemId === itemId; }), 'WarehouseId')
            .map(function (w) { return { Id: w.WarehouseId, Warehouse: w.WareHouseName }; });
        C.fillSelect(el, whs, 'Id', 'Warehouse');
        if (keep && whs.some(function (w) { return w.Id === keep; })) el.value = String(keep); else el.value = '0';
        if (intOf(el.value) !== 0) return;
        if (whs.length === 1) el.value = String(whs[0].Id);
        else if (whs.length > 1) {
            var it = selectedItem();
            var cfg = C.configWarehouse(it ? it.ParentCategoryId : 0, look);
            if (cfg !== 0 && whs.some(function (w) { return w.Id === cfg; })) el.value = String(cfg);
        }
    }

    function rackBind(itemId, warehouseId) {                                    // BindRacks:1068
        var el = $id('CmbRackName'), keep = intOf(el.value);
        var racks = C.distinct(look.racks.filter(function (x) {
            return x.ItemId === itemId && (warehouseId === 0 || x.WarehouseId === warehouseId);
        }), 'Id');
        C.fillSelect(el, racks, 'Id', 'RackName');
        if (keep && racks.some(function (r) { return r.Id === keep; })) el.value = String(keep); else el.value = '0';
        if (intOf(el.value) !== 0) return;
        if (racks.length === 1) el.value = String(racks[0].Id);
        else if (racks.length > 1) {
            var base = racks.find(function (r) { return intOf(r.BaseRackId) > 0; });
            if (base) el.value = String(base.Id);
        }
    }

    function debitAccountsBind() {                                              // DebitAcDtFillFromGlobal:831
        var c1 = intOf($id('CmbCategory1').value);
        var list = c1 === 57 ? look.debitAccounts57 : (c1 === 58 ? look.debitAccounts58 : []);
        C.fillSelect('cmbDrAc', list, 'Id', 'AccountTitle');
    }

    /* ------------------------------------------------------------------ entry bar events */

    function catTypeChanged() { categoryBind(); itemBind(); }
    function categoryLeave() {                                                  // CmbCategory_Leave:948
        C.fillSelect('CmbWarehouse', [], 'Id', 'x'); C.fillSelect('CmbRackName', [], 'Id', 'x');
        itemBind();
    }
    function itemLeave() {                                                      // :1002
        var itemId = intOf($id('cmbItem').value);
        balance();
        lastRates();
        uomBind(itemId);
        warehouseBind(itemId);
        rackBind(itemId, intOf($id('CmbWarehouse').value));
        balance();
    }
    function warehouseLeave() { rackBind(intOf($id('cmbItem').value), intOf($id('CmbWarehouse').value)); balance(); }
    function rackChanged() {                                                    // :1118
        var rackId = intOf($id('CmbRackName').value), wh = intOf($id('CmbWarehouse').value);
        var rack = look.racks.find(function (r) { return r.Id === rackId; });
        if (!rack) return;
        if (wh === 0 || rack.WarehouseId !== wh) {
            if ($id('CmbWarehouse').options.length > 1) $id('CmbWarehouse').value = String(rack.WarehouseId);
        }
        balance();
    }
    function conditionLeave() { balance(); lastRates(); }
    function category1Leave() {                                                 // :920
        debitAccountsBind();
        var is57 = intOf($id('CmbCategory1').value) === 57;
        $id('CmbInvoiceNo').disabled = !is57;
        if (!is57) $id('CmbInvoiceNo').value = '0';
    }

    var balanceSeq = 0;
    function balance() {                                                        // BalanceStockQtyandAvgRate:2140
        $id('txtItemRate').value = '0'; $id('txtBalanceQty').value = '0'; $id('txtItemAmount').value = '0';
        var itemId = intOf($id('cmbItem').value);
        var seq = ++balanceSeq;
        if (!itemId) return Promise.resolve(false);
        return C.getJson(api + '/stock' + C.qs({
            recId: recId, itemId: itemId, docDate: $id('txtDocdate').value,
            itemConditionId: intOf($id('CmbItemCondition').value),
            warehouseId: intOf($id('CmbWarehouse').value), rackId: intOf($id('CmbRackName').value)
        })).then(function (s) {
            if (seq !== balanceSeq) return false;
            $id('txtBalanceQty').value = s.qtyInHand;
            $id('txtItemRate').value = s.avgRate;
            amount();
            return true;
        }).catch(function (e) { alert(e.message); return false; });
    }
    function amount() {                                                         // AmountCalculation:2174
        var q = num($id('txtItemQty').value), r = num($id('txtItemRate').value);
        $id('txtItemAmount').value = (q !== 0 && r !== 0) ? String(q * r) : '0';
    }
    function lastRates() {                                                      // GetItemLastRates:2104
        var box = $id('RatesListView'), itemId = intOf($id('cmbItem').value);
        box.innerHTML = '';
        if (itemId <= 0) return;
        C.getJson(api + '/last-rates' + C.qs({ itemId: itemId, itemConditionId: intOf($id('CmbItemCondition').value) }))
            .then(function (rows) {
                if (!rows || !rows.length) return;
                var cols = Object.keys(rows[0]);
                box.innerHTML = '<table><thead><tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') +
                    '</tr></thead><tbody>' + rows.map(function (r) {
                        return '<tr>' + cols.map(function (c) {
                            var v = r[c];
                            return '<td>' + esc(/^\d{4}-\d{2}-\d{2}/.test(String(v || '')) ? C.gridDate(v) : v) + '</td>';
                        }).join('') + '</tr>';
                    }).join('') + '</tbody></table>';
            }).catch(function () { /* the desktop shows its own box; the list simply stays empty */ });
    }
    function barcode() {                                                        // txtBarcodeReader_KeyDown:2261
        var b = $id('txtBarcodeReader').value.trim();
        C.getJson(api + '/barcode' + C.qs({ barcode: b })).then(function (r) {
            $id('cmbItem').value = String(r.itemId || 0);
            itemLeave();
            $id('txtBarcodeReader').value = '';
            $id('cmbUOM').focus();
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ detail rows */

    function text(id) { var el = $id(id); return el.selectedOptions && el.selectedOptions[0] ? el.selectedOptions[0].textContent.trim() : ''; }

    function validateDetail() {                                                 // FormValidationDetail:1151
        var combos = [['cmbItem', 'Item'], ['CmbWarehouse', 'Warehouse'], ['CmbRackName', 'Rack Name'],
            ['CmbItemCondition', 'Item Condition'], ['cmbUOM', 'UOM']];
        for (var i = 0; i < combos.length; i++) {
            if (intOf($id(combos[i][0]).value) === 0) { alert(combos[i][1] + ' field is required'); $id(combos[i][0]).focus(); return false; }
        }
        var nums = [['txtItemQty', 'Item Quantity'], ['txtItemRate', 'Item Rate'], ['txtItemAmount', 'Item Amount']];
        for (var j = 0; j < nums.length; j++) {
            var v = parseFloat($id(nums[j][0]).value);
            if (isNaN(v) || v === 0) { alert(nums[j][1] + ' must be a non-zero number'); $id(nums[j][0]).focus(); return false; }
        }
        var more = [['CmbDepartment', 'Department'], ['cmbAssetsRef', 'Fixed Asset'], ['CmbCategory1', 'Issuance Type']];
        for (var k = 0; k < more.length; k++) {
            if (intOf($id(more[k][0]).value) === 0) { alert(more[k][1] + ' field is required'); $id(more[k][0]).focus(); return false; }
        }
        if (intOf($id('CmbCategory1').value) === 57 && intOf($id('CmbInvoiceNo').value) === 0) {
            alert('Contract Schedule No Required.'); $id('CmbInvoiceNo').focus(); return false;
        }
        return true;
    }
    function rowFromBar(r) {
        var it = selectedItem() || {};
        r.ItemId = intOf($id('cmbItem').value); r.ItemCode = it.ItemCode || ''; r.ItemName = it.ItemName || '';
        r.WarehouseId = intOf($id('CmbWarehouse').value); r.WarehouseName = text('CmbWarehouse');
        r.RackId = intOf($id('CmbRackName').value); r.RackName = text('CmbRackName');
        r.ItemConditionId = intOf($id('CmbItemCondition').value); r.ItemCondition = text('CmbItemCondition');
        r.UnitId = intOf($id('cmbUOM').value); r.Unit = text('cmbUOM');
        r.IssueQty = num($id('txtItemQty').value); r.ItemRate = num($id('txtItemRate').value); r.ItemAmount = num($id('txtItemAmount').value);
        r.DepartmentId = intOf($id('CmbDepartment').value); r.Department = text('CmbDepartment');
        r.AssetId = intOf($id('cmbAssetsRef').value); r.Asset = text('cmbAssetsRef');
        r.IssuanceTypeId = intOf($id('CmbCategory1').value); r.IssuanceType = text('CmbCategory1');
        r.ContractScheduleId = intOf($id('CmbInvoiceNo').value); r.ContractScheduleNo = text('CmbInvoiceNo');
        r.DrAcId = intOf($id('cmbDrAc').value); r.DebitAc = text('cmbDrAc');
        r.Remarks = $id('txtRemarksDetail').value.trim();
        return r;
    }
    function add() { if (!validateDetail()) return; table.push(rowFromBar({ Id: 0 })); render(); resetDetail(); }
    function updateDetail() {
        if (!validateDetail()) return;
        rowFromBar(table[updateDetailIndex]);
        render(); resetDetail();
    }
    function edit(i) {                                                          // grdDetail_DoubleClick:1257
        var r = table[i];
        updateDetailIndex = i;
        $id('CmbCategory').value = '0';
        itemBind();
        $id('cmbItem').value = String(r.ItemId);
        itemLeave();
        $id('CmbWarehouse').value = String(r.WarehouseId);
        rackBind(r.ItemId, r.WarehouseId);
        $id('CmbRackName').value = String(r.RackId);
        $id('CmbItemCondition').value = String(r.ItemConditionId);
        $id('cmbUOM').value = String(r.UnitId);
        $id('txtItemQty').value = r.IssueQty;
        /* :1275 the desktop's lookups run synchronously BEFORE the row's rate and amount are put
           back, so the row's own values win. Here the lookups are async: run the last one with the
           row's warehouse/rack/condition, then put the row's rate and amount back over it. */
        balance().then(function (current) {
            if (!current || updateDetailIndex !== i) return;                    // superseded by a later lookup
            $id('txtItemRate').value = r.ItemRate; $id('txtItemAmount').value = r.ItemAmount;
        });
        $id('txtItemRate').value = r.ItemRate; $id('txtItemAmount').value = r.ItemAmount;
        $id('CmbDepartment').value = String(r.DepartmentId); $id('cmbAssetsRef').value = String(r.AssetId);
        $id('CmbCategory1').value = String(r.IssuanceTypeId); category1Leave();
        $id('CmbInvoiceNo').value = String(r.ContractScheduleId); $id('cmbDrAc').value = String(r.DrAcId);
        $id('txtRemarksDetail').value = r.Remarks || '';
        $id('Add').classList.add('is-hidden'); $id('btnUpdateDetail').classList.remove('is-hidden'); $id('btnCancelDetail').classList.remove('is-hidden');
    }
    function del(i) {                                                           // DeleteDetailRow:1376
        if (updateDetailIndex !== -1) { alert('Reset Detail First'); return; }
        var r = table[i];
        if (intOf(r.Id) > 0) { if (!confirm('Are you sure to Delete?')) return; removed.push(r); }
        table.splice(i, 1); render();
    }
    function resetDetail() {                                                    // ResetDetail:1721
        updateDetailIndex = -1;
        ['cmbItem', 'CmbItemCondition', 'cmbUOM', 'cmbDrAc'].forEach(function (id) { $id(id).value = '0'; });
        C.fillSelect('CmbWarehouse', [], 'Id', 'x'); C.fillSelect('CmbRackName', [], 'Id', 'x');
        ['txtBalanceQty', 'txtItemQty', 'txtItemRate', 'txtItemAmount', 'txtRemarksDetail'].forEach(function (id) { $id(id).value = ''; });
        $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelDetail').classList.add('is-hidden'); $id('Add').classList.remove('is-hidden');
        $id('cmbItem').focus();
    }

    var GRID = [['ItemCode', 'Item Code'], ['ItemName', 'Item Name'], ['WarehouseName', 'WareHouse Name'], ['RackName', 'Rack Name'],
        ['ItemCondition', 'Item Condition'], ['Unit', 'Pack Uom'], ['IssueQty', 'Issue Qty', 1], ['ItemRate', 'Item Rate', 1],
        ['ItemAmount', 'Item Amount', 1], ['Department', 'Department'], ['Asset', 'Asset'], ['IssuanceType', 'Issuance Type'],
        ['ContractScheduleNo', 'Schedule / Invoice No'], ['DebitAc', 'Debit Ac'], ['Remarks', 'Remarks']];
    function render() {
        var t = $id('grdDetail');
        t.tHead.innerHTML = '<tr><th></th><th></th>' + GRID.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr ondblclick="IssDirect.edit(' + i + ')"><td><button type="button" class="cx-x" onclick="IssDirect.del(' + i + ')">X</button></td>' +
                '<td><button type="button" class="cx-link" onclick="IssDirect.edit(' + i + ')">+</button></td>' +
                GRID.map(function (c) { return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(r[c[0]]) + '</td>'; }).join('') + '</tr>';
        }).join('');
    }

    /* ------------------------------------------------------------------ load / reset */

    function init() {
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            var r = l.rights || {};
            $id('btnSave').disabled = !r.save; $id('btnPrint').disabled = !r.print; $id('btnUpdate').disabled = !r.update;
            if (!loaded) $id('ChkPrintPreview').checked = !!r.print;             // Load:446 only
            if (!r.view) { $id('rightsNote').textContent = 'You do not have the View right for Store Issuance Direct.'; $id('rightsNote').classList.remove('is-hidden'); }
            if (l.itemSearchByCode) $id('rdSearchByCode').checked = true; else $id('rdSearchByName').checked = true;
            if (!recId) { $id('txtDocNo').value = l.docNo || ''; $id('txtBranchSrNo').value = l.branchSrNo || ''; }
            categoryBind(); itemBind();
            C.fillSelect('CmbItemCondition', l.itemConditions, 'Id', 'Description');
            C.fillSelect('CmbDepartment', l.departments, 'Id', 'Name');
            C.fillSelect('cmbAssetsRef', l.assets, 'Id', 'Name');
            C.fillSelect('CmbCategory1', l.issuanceTypes, 'Id', 'Name');
            C.fillSelect('CmbInvoiceNo', l.schedules, 'Id', 'ScheduleCode');
            debitAccountsBind();
            if (!loaded) {                                                      // Load:469 only
                var days = intOf(l.defaultDaysToLessFromHistoryFromDate) > 0 ? intOf(l.defaultDaysToLessFromHistoryFromDate) : 3;
                var f = new Date(); f.setDate(f.getDate() - days);
                $id('FromDateHistory').value = f.getFullYear() + '-' + String(f.getMonth() + 1).padStart(2, '0') + '-' + String(f.getDate()).padStart(2, '0');
                $id('ToDateHistory').value = C.today();
            }
            loaded = true;
        }).catch(function (e) { alert(e.message); });
    }
    function reset() {                                                          // formReset:1686
        recId = 0; voucherHeadId = 0; removed = []; table = [];
        $id('txtManualNo').value = ''; $id('txtRemarks').value = '';            // txtDocdate kept (formReset:1686)
        $id('btnSave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden'); $id('btnDelete').classList.add('is-hidden');
        $id('lblRecId').textContent = ''; $id('lblVoucher').textContent = '';
        render();
        return init().then(resetDetail);
    }
    function refresh() { init(); }

    /* btnDepartment_Click:2331 / btnAssetDefine_Click:2343 — new Define_Department / frmLookUpDefineAsset(UserAccount).Show().
       The desktop picks new rows up only on btnRefresh_Click:1762 (DepartmentBind / FixedAssetsBind).
       Web deviation: when the dialog closes, only that combo is re-filled from this page's own
       /lookups (same source: Sp_Department_GetAllMethod / Sp_FixedAssetsRegister_GetAllMethod ReadAll),
       keeping the current selection (fillSelect retains the value). */
    function refillCombo(id, key) {
        return C.getJson(api + '/lookups').then(function (l) {
            if (look) look[key] = l[key];
            C.fillSelect(id, l[key], 'Id', 'Name');
        }).catch(function (e) { alert(e.message); });
    }
    function defineDepartment() {
        window.StoreDefine.openDepartment(function () { refillCombo('CmbDepartment', 'departments'); }, { host: 'StoreIssuanceDirect' });
    }
    function defineAsset() {
        window.StoreDefine.openAsset(function () { refillCombo('cmbAssetsRef', 'assets'); }, { host: 'StoreIssuanceDirect' });
    }

    /* ------------------------------------------------------------------ save */

    function save() { return C.withBusy('btnSave', function () { recId = 0; return insert(); }); }
    function update() { return C.withBusy('btnUpdate', function () { return insert(); }); }
    function insert() {                                                         // Insert():1418
        if (!table.length) { alert('Grid Record Not Found'); return Promise.resolve(); }
        if (!$id('txtDocNo').value.trim()) { alert('Doc No field is required'); $id('txtDocNo').focus(); return Promise.resolve(); }
        if (!confirm(recId === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return Promise.resolve();
        var wasNew = recId === 0, shouldPrint = $id('ChkPrintPreview').checked;
        return C.postJson(api + '/save', {
            Id: recId, DocNo: intOf($id('txtDocNo').value), BranchSrNo: intOf($id('txtBranchSrNo').value),
            DocDate: $id('txtDocdate').value, ManualNo: $id('txtManualNo').value, Remarks: $id('txtRemarks').value,
            rows: table, removed: removed
        }).then(function (res) {
            alert(res.message);
            var code = res.id;
            reset();
            if ($id('ChkStoreReturnLink').checked && wasNew) {
                window.open('/store/store-return?issuanceId=' + code + (shouldPrint ? '&printIssuance=' + code + '&printScreen=direct' : ''), '_blank');
            } else if (shouldPrint) {
                printId(code);
            }
        }).catch(function (e) { alert(e.message); });
    }
    function remove() {
        if (!(recId > 0)) { alert('Record Not Found'); return Promise.resolve(); }
        if (!confirm('Are you sure to Delete?')) return Promise.resolve();
        return C.withBusy('btnDelete', function () {
            return C.postJson(api + '/' + recId + '/delete', {}).then(function (res) { alert(res.message); reset(); })
                .catch(function (e) { alert(e.message); });
        });
    }
    function printId(id) { if (!id) { alert('No Record Found For Display'); return; } C.printSlip(api + '/' + id + '/slip', 'Store Issuance Slip'); }

    function readById(id) {                                                     // ReadById:1590
        C.getJson(api + '/' + id).then(function (h) {
            resetDetail();                                                      // ReadById:1596 formReset → ResetDetail
            recId = h.Id; removed = [];
            tab('tabForm');
            $id('btnSave').classList.add('is-hidden'); $id('btnUpdate').classList.remove('is-hidden');
            $id('btnDelete').classList.toggle('is-hidden', !(look.rights || {}).delete);
            $id('txtDocdate').value = C.isoDay(h.DocDate); $id('txtDocNo').value = h.DocNo; $id('txtBranchSrNo').value = h.BranchSrNo;
            $id('txtRemarks').value = h.Remarks || ''; $id('txtManualNo').value = h.ManualNo || '';
            voucherHeadId = h.voucherHeadId || 0;
            $id('lblVoucher').textContent = voucherHeadId ? ('Voucher #' + voucherHeadId) : '';
            $id('lblRecId').textContent = 'Record #' + recId;
            table = h.rows || [];
            render();
        }).catch(function (e) { alert(e.message); });
    }

    function docDateLeave() {                                                   // txtDocdate_Leave:2207
        lastRates(); balance();
        Promise.all(table.map(function (r) {
            return C.getJson(api + '/stock' + C.qs({ recId: recId, itemId: r.ItemId, docDate: $id('txtDocdate').value, itemConditionId: r.ItemConditionId }))
                .then(function (s) { r.ItemRate = s.avgRate; r.ItemAmount = num(r.IssueQty) * s.avgRate; });
        })).then(render).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ history */

    function newHistory() {
        $id('FromDateHistory').value = C.today(); $id('ToDateHistory').value = C.today();
        $id('FromDocNoHistory').value = ''; $id('ToDocNoHistory').value = '';
        $id('DataGridHistory').tBodies[0].innerHTML = ''; $id('grddetailofmain').tBodies[0].innerHTML = '';
    }
    function showHistory(btn) {                                                 // HistoryFill:1866
        return C.withBusy(btn || document.querySelector('#tabHistory .win-btn-action') || $id('btnFooterHistory'), function () {
            return C.getJson(api + '/history' + C.qs({
                fromDate: $id('chkFromDate').checked ? $id('FromDateHistory').value : '',
                toDate: $id('chkToDate').checked ? $id('ToDateHistory').value : '',
                fromDocNo: intOf($id('FromDocNoHistory').value), toDocNo: intOf($id('ToDocNoHistory').value)
            })).then(function (list) {
                var t = $id('DataGridHistory');
                t.tHead.innerHTML = '<tr><th>Edit</th><th>Slip</th><th>Doc Date</th><th>Doc No</th><th>Type</th><th>Manual No</th>' +
                    '<th>Entry Date</th><th>Entry User</th><th>Modify Date</th><th>Modify User</th><th>Remarks</th><th>No Of Attachments</th><th>Branch Sr No</th></tr>';
                t.tBodies[0].innerHTML = (list || []).map(function (h) {
                    return '<tr data-id="' + h.Id + '" onclick="IssDirect.historyDetail(this)" ondblclick="IssDirect.open(' + h.Id + ')">' +
                        '<td><button type="button" class="cx-link" onclick="event.stopPropagation();IssDirect.open(' + h.Id + ')">Edit</button></td>' +
                        '<td><button type="button" class="cx-link" onclick="event.stopPropagation();IssDirect.printId(' + h.Id + ')">Slip</button></td>' +
                        '<td>' + esc(C.gridDate(h.DocDate)) + '</td><td><a class="cx-link" href="javascript:void(0)" onclick="event.stopPropagation();IssDirect.open(' + h.Id + ')">' + esc(h.DocNo) + '</a></td><td>' + esc(h.Type) + '</td><td>' + esc(h.ManualNo) +
                        '</td><td>' + esc(C.gridDateTime(h.EntryDate)) + '</td><td>' + esc(h.EntryUser) + '</td><td>' + esc(C.gridDateTime(h.ModifyDate)) +
                        '</td><td>' + esc(h.ModifyUser) + '</td><td>' + esc(h.Remarks) + '</td><td class="num">' + esc(h.NoOfAttachments) +
                        '</td><td class="num">' + esc(h.BranchSrNo) + '</td></tr>';
                }).join('');
                $id('grddetailofmain').tBodies[0].innerHTML = '';
            }).catch(function (e) { alert(e.message); });
        });
    }

    function historyDetail(tr) {
        document.querySelectorAll('#DataGridHistory tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        C.getJson(api + '/' + tr.getAttribute('data-id')).then(function (h) {
            var t = $id('grddetailofmain');
            t.tHead.innerHTML = '<tr>' + GRID.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = (h.rows || []).map(function (r) {
                return '<tr>' + GRID.map(function (c) { return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(r[c[0]]) + '</td>'; }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { alert(e.message); });
    }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    window.IssDirect = {
        reset: reset, save: save, update: update, remove: remove, refresh: refresh, tab: tab,
        defineDepartment: defineDepartment, defineAsset: defineAsset,
        print: function () { printId(recId); }, printId: printId, open: readById,
        catTypeChanged: catTypeChanged, categoryLeave: categoryLeave, itemBind: itemBind, itemLeave: itemLeave,
        warehouseLeave: warehouseLeave, rackChanged: rackChanged, conditionLeave: conditionLeave,
        category1Leave: category1Leave, amount: amount, barcode: barcode,
        add: add, updateDetail: updateDetail, resetDetail: resetDetail, edit: edit, del: del,
        docDateLeave: docDateLeave, showHistory: showHistory, newHistory: newHistory, historyDetail: historyDetail
    };
    document.addEventListener('DOMContentLoaded', function () {
        $id('txtDocdate').value = C.today();                                    // designer: DateTime.Now, once
        reset();
    });
})();
