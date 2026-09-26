/* ============================================================================================
 * Screen 330 "Store Return" — Architecture.WinApp.StoreManagement.StoreReturn, DocumentTypeId 140.
 *
 * Two ways in, never mixed (btnLoadIssuance_Click:2818):
 *   - direct entry through the Detail box (Add / double-click to edit a direct row), or
 *   - Load Issuance (frmLoadIssuanceForReturn), which hides the Detail box and lets the operator
 *     split an issuance line across rows with "+" (AddDetailrow:1294), each split sharing RecordNo.
 *
 *   grdDetail_CellUpdated:1204   qty / secondary qty / per-item weight / secondary rate / rate couple
 *   ValidateQuantity:1265        an issuance row cannot exceed its balance across its RecordNo
 *   LoadInGridDetail:2874        rate = average rate at the doc date; secondary uom = the item's base one;
 *                                opened from 321/322 (AsReplacement) the condition becomes 4
 *   txtDocdate_ValueChanged:2943 every row's AMOUNT is recomputed at the new date (the rate cell is not)
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, ci = C.ci, num = C.num, intOf = C.intOf;
    var api = '/api/store/store-return';

    var look = { rights: {}, items: [], uoms: [], racks: [], itemConditions: [], creditAccounts: [] };
    var recId = 0;
    var table = [];
    var updateDetailIndex = -1;
    var loaderData = [], loaderPicked = {};
    /* StoreReturn.cs:64-70 — STATIC fields: PerItem and SecondaryRate start true, and nothing resets
       them (not formReset, not reopening the form), so they are set once per page. */
    var flags = { byItemRate: false, bySecRate: true, byPerItem: true, bySecQty: false };
    var loaded = false;             // first Load done — defaults below are Load-only on the desktop
    var crColHidden = false;        // btnRefresh_Click:874 hides Credit Account only with rows + effects off

    function fmt3(v) { return Number(num(v).toFixed(3)).toLocaleString('en-US', { maximumFractionDigits: 3 }); }
    function sel(id) { var e = $id(id); return e.selectedOptions && e.selectedOptions[0] ? e.selectedOptions[0].textContent.trim() : ''; }
    function anyIssuance() { return table.some(function (r) { return intOf(r.IssuanceId) > 0; }); }
    function isNew() { return !$id('btnsave').classList.contains('is-hidden') && !$id('btnsave').disabled; }

    /* ------------------------------------------------------------------ entry bar */

    function itemBind() {
        C.fillSelect('cmbItem', look.items, 'Id', $id('rdSearchByName').checked ? 'ItemName' : 'ItemCode');
    }
    function selectedItem() { var id = intOf($id('cmbItem').value); return look.items.find(function (x) { return x.Id === id; }) || null; }

    function uomBind(itemId) {                                                  // CommonBindings.ItemUomFromGlobalBind
        var list = look.uoms.filter(function (u) { return u.ItemId === itemId; });
        C.fillSelect('cmbUOM', list, 'Id', 'UOMCode');
        C.fillSelect('CmbSecondaryUom', list, 'Id', 'UOMCode');
        var bp = list.find(function (u) { return u.BasePackUom; }), bs = list.find(function (u) { return u.BaseSecondaryUom; });
        if (bp) $id('cmbUOM').value = String(bp.Id);
        if (bs) $id('CmbSecondaryUom').value = String(bs.Id);
    }
    function avgRate(itemId, conditionId) {
        return C.getJson(api + '/avg-rate' + C.qs({ recId: recId, itemId: itemId, docDate: $id('txtDocdate').value, itemConditionId: conditionId }))
            .then(function (r) { return num(r.avgRate); });
    }
    function itemLeave() {                                                      // cmbItem_Leave:535
        var itemId = intOf($id('cmbItem').value);
        C.fillSelect('cmbCrAc', look.creditAccounts.filter(function (a) { return a.ItemId === itemId; }), 'Id', 'AccountTitle');
        uomBind(itemId);
        var p1 = C.getJson(api + '/last-issuance-rate' + C.qs({ itemId: itemId })).then(function (r) { $id('txtIssuanceRate').value = r.itemRate > 0 ? r.itemRate : '0'; })
            .catch(function (e) { alert(e.message); });
        var p2 = avgRate(itemId, intOf($id('CmbItemCondition').value)).then(function (v) { $id('txtItemRate').value = fmt3(v); rateChanged('txtItemRate', true); })
            .catch(function (e) { alert(e.message); });
        warehouseBind(itemId);
        rackBind(itemId, intOf($id('CmbWarehouse').value));
        return Promise.all([p1, p2]);
    }
    /* rdSearchByName_CheckedChanged → ItemNameBind:520 — rebinds, keeps the item, then cmbItem_Leave. */
    function searchModeChanged() { itemBind(); itemLeave(); }
    function warehouseBind(itemId) {                                            // :553
        var el = $id('CmbWarehouse'), keep = intOf(el.value);
        var whs = C.distinct(look.racks.filter(function (x) { return x.ItemId === itemId; }), 'WarehouseId')
            .map(function (w) { return { Id: w.WarehouseId, Warehouse: w.WareHouseName }; });
        C.fillSelect(el, whs, 'Id', 'Warehouse');
        el.value = (keep && whs.some(function (w) { return w.Id === keep; })) ? String(keep) : '0';
        if (intOf(el.value) !== 0) return;
        if (whs.length === 1) el.value = String(whs[0].Id);
        else if (whs.length > 1) {
            var it = selectedItem(), cfg = C.configWarehouse(it ? it.ParentCategoryId : 0, look);
            if (cfg !== 0 && whs.some(function (w) { return w.Id === cfg; })) el.value = String(cfg);
        }
    }
    function rackBind(itemId, whId) {                                           // :602
        var el = $id('CmbRackName'), keep = intOf(el.value);
        var racks = C.distinct(look.racks.filter(function (x) { return x.ItemId === itemId && (whId === 0 || x.WarehouseId === whId); }), 'Id');
        C.fillSelect(el, racks, 'Id', 'RackName');
        el.value = (keep && racks.some(function (r) { return r.Id === keep; })) ? String(keep) : '0';
        if (intOf(el.value) !== 0) return;
        if (racks.length === 1) el.value = String(racks[0].Id);
        else if (racks.length > 1) {
            var b = racks.find(function (r) { return intOf(r.BaseRackId) > 0; });
            if (b) { el.value = String(b.Id); rackLeave(); }
        }
    }
    function rackLeave() {                                                      // :653
        var rack = look.racks.find(function (r) { return r.Id === intOf($id('CmbRackName').value); });
        var wh = intOf($id('CmbWarehouse').value);
        if (rack && (wh === 0 || rack.WarehouseId !== wh) && $id('CmbWarehouse').options.length > 1) $id('CmbWarehouse').value = String(rack.WarehouseId);
    }
    function conditionLeave() {                                                 // :2966
        avgRate(intOf($id('cmbItem').value), intOf($id('CmbItemCondition').value)).then(function (v) { $id('txtItemRate').value = fmt3(v); rateChanged('txtItemRate', true); });
    }

    function amount() {                                                         // CalculateAmount:2284
        var a = num($id('txtItemQty').value) * num($id('txtItemRate').value);
        $id('txtItemAmount').value = a > 0 ? String(a) : '0';
    }
    function rates(active) {                                                    // ItemRateAndSecondaryRateCalculations:2399
        var itemRate = num($id('txtItemRate').value), secRate = num($id('txtSecondaryUomRate').value);
        var secQty = num($id('txtSecondaryUomQty').value), qty = num($id('txtItemQty').value);
        if (qty <= 0 || secQty <= 0) { $id('txtItemRate').value = '0'; $id('txtSecondaryUomRate').value = '0'; return; }
        if (active === 'txtItemRate') { $id('txtSecondaryUomRate').value = fmt3(Math.round(itemRate * (qty / secQty) * 1000) / 1000); flags.byItemRate = true; flags.bySecRate = false; }
        else if (active === 'txtSecondaryUomRate') { $id('txtItemRate').value = fmt3(Math.round(secRate * secQty / qty * 1000) / 1000); flags.byItemRate = false; flags.bySecRate = true; }
        else if (flags.bySecRate) $id('txtItemRate').value = fmt3(Math.round(secRate * secQty / qty * 1000) / 1000);
        else if (flags.byItemRate) $id('txtSecondaryUomRate').value = fmt3(Math.round(itemRate * (qty / secQty) * 1000) / 1000);
    }
    function secondary(active) {                                                // SecondaryUomQtyAndPerItemSecondaryQtyCalculations:2349
        var perItem = num($id('txtPerItemWeight').value), total = num($id('txtSecondaryUomQty').value), qty = num($id('txtItemQty').value);
        if (qty <= 0) { $id('txtPerItemWeight').value = '0'; $id('txtSecondaryUomQty').value = '0'; return; }
        if (active === 'txtPerItemWeight') { $id('txtSecondaryUomQty').value = fmt3(Math.round(perItem * qty * 1000) / 1000); flags.byPerItem = true; flags.bySecQty = false; }
        else if (active === 'txtSecondaryUomQty') { $id('txtPerItemWeight').value = fmt3(Math.round(total / qty * 1000) / 1000); flags.byPerItem = false; flags.bySecQty = true; }
        else if (flags.bySecQty) $id('txtPerItemWeight').value = fmt3(Math.round(total / qty * 1000) / 1000);
        else if (flags.byPerItem) $id('txtSecondaryUomQty').value = fmt3(Math.round(perItem * qty * 1000) / 1000);
    }
    /* A value SET by code fires TextChanged on the desktop too; `programmatic` stands for that path,
       where the active control is not the box itself. */
    function qtyChanged() { rates('txtItemQty'); amount(); }
    function secQtyChanged(which) { secondary(which); amount(); }
    function rateChanged(which, programmatic) { rates(programmatic ? '' : which); amount(); }

    function validateDetail() {                                                 // FormValidationDetail:1836
        var checks = [['cmbItem', 'Item Field Required'], ['CmbWarehouse', 'WareHouseName Field Required'],
            ['CmbRackName', 'Rack Name Field Required'], ['CmbItemCondition', 'Item Condition Field Required']];
        for (var i = 0; i < checks.length; i++) if (intOf($id(checks[i][0]).value) === 0) { alert(checks[i][1]); $id(checks[i][0]).focus(); return false; }
        if (intOf($id('cmbUOM').value) === 0) { alert('Item PackUom Field Required'); return false; }
        if (num($id('txtItemQty').value) === 0) { alert('ItemQty Field Required'); $id('txtItemQty').focus(); return false; }
        if (num($id('txtItemRate').value) === 0) { alert('ItemRate Field Required'); $id('txtItemRate').focus(); return false; }
        if (num($id('txtItemAmount').value) === 0) { alert('ItemAmount Field Required'); return false; }
        if (intOf($id('CmbDepartment').value) === 0) { alert('Department Field Required'); return false; }
        if (intOf($id('cmbAssetsRef').value) === 0) { alert('Fixed Asset Field Required'); return false; }
        if (intOf($id('cmbCrAc').value) === 0 && look.storeFinancialEffects) { alert('Credit Account Field Required'); return false; }
        return true;
    }
    function fillRow(r) {                                                       // FillDetailRow:931
        var it = selectedItem() || {}, qty = num($id('txtItemQty').value);
        r.ItemId = intOf($id('cmbItem').value); r.ItemCode = it.ItemCode || ''; r.ItemName = it.ItemName || '';
        r.WareHouseId = intOf($id('CmbWarehouse').value); r.WareHouseName = sel('CmbWarehouse');
        r.RackId = intOf($id('CmbRackName').value); r.RackName = sel('CmbRackName');
        r.ItemConditionId = intOf($id('CmbItemCondition').value);
        r.PackUomId = intOf($id('cmbUOM').value); r.PackUom = sel('cmbUOM');
        r.ItemQty = qty; r.BalanceQty = qty;
        r.SecondaryUomId = intOf($id('CmbSecondaryUom').value); r.SecondaryUom = sel('CmbSecondaryUom');
        r.SecondaryUomQty = num($id('txtSecondaryUomQty').value); r.PerItemWeight = num($id('txtPerItemWeight').value);
        r.SecondaryUomItemRate = num($id('txtSecondaryUomRate').value);
        r.ItemRate = num($id('txtItemRate').value); r.IssuanceRate = num($id('txtIssuanceRate').value);
        r.ItemAmount = num($id('txtItemAmount').value);
        r.DepartmentId = intOf($id('CmbDepartment').value); r.AssetId = intOf($id('cmbAssetsRef').value);
        r.CrAccountId = intOf($id('cmbCrAc').value); r.CreditAccount = sel('cmbCrAc');
        r.Remarks = $id('txtRemarksDetail').value.trim(); r.RecordNo = 0;
        return r;
    }
    function add() { if (!validateDetail()) return; crColHidden = false; /* Add_Click:998 BindGrids */ table.push(fillRow({ IssuanceId: 0, IssuanceDetailId: 0, IssuanceNo: 0 })); render(); resetDetail(); }
    function updateDetail() { if (!validateDetail()) return; fillRow(table[updateDetailIndex]); render(); resetDetail(); }
    function edit(i) {                                                          // grdDetail_DoubleClick:1025
        var r = table[i];
        if (intOf(r.IssuanceId) > 0) return;
        updateDetailIndex = i;
        $id('cmbItem').value = String(r.ItemId);
        var looked = itemLeave();
        $id('CmbWarehouse').value = String(r.WareHouseId); rackBind(r.ItemId, r.WareHouseId); $id('CmbRackName').value = String(r.RackId);
        $id('CmbItemCondition').value = String(r.ItemConditionId); $id('cmbUOM').value = String(r.PackUomId);
        $id('CmbSecondaryUom').value = String(r.SecondaryUomId);
        /* :1035-1047 cmbItem_Leave's lookups finish (synchronously) BEFORE the row's values go back,
           and each box set by code fires its TextChanged with the grid as the active control — so the
           flag-driven recalculations run in this order, and the row's amount is written last. */
        looked.then(function () {
            if (updateDetailIndex !== i) return;
            $id('txtItemQty').value = r.ItemQty; rates(''); amount();                 // txtItemQty_TextChanged
            $id('txtSecondaryUomQty').value = r.SecondaryUomQty; secondary(''); amount();
            $id('txtPerItemWeight').value = r.PerItemWeight; secondary('');
            $id('txtSecondaryUomRate').value = r.SecondaryUomItemRate; rates(''); amount();
            $id('txtIssuanceRate').value = fmt3(r.IssuanceRate);
            $id('txtItemRate').value = r.ItemRate; rates(''); amount();
            $id('txtItemAmount').value = r.ItemAmount;
        }).catch(function (e) { alert(e.message); });
        $id('CmbDepartment').value = String(r.DepartmentId); $id('cmbAssetsRef').value = String(r.AssetId);
        $id('cmbCrAc').value = String(r.CrAccountId); $id('txtRemarksDetail').value = r.Remarks || '';
        $id('Add').classList.add('is-hidden'); $id('btnUpdateDetail').classList.remove('is-hidden'); $id('btnCancelDetail').classList.remove('is-hidden');
    }
    function resetDetail() {                                                    // ResetDetail:829
        updateDetailIndex = -1;
        $id('cmbItem').value = '0';
        C.fillSelect('CmbWarehouse', [], 'Id', 'x'); C.fillSelect('CmbRackName', [], 'Id', 'x');
        $id('cmbUOM').value = '0'; $id('CmbSecondaryUom').value = '0'; $id('cmbCrAc').value = '0';
        ['txtItemQty', 'txtSecondaryUomQty', 'txtPerItemWeight', 'txtSecondaryUomRate', 'txtItemRate', 'txtItemAmount', 'txtRemarksDetail']
            .forEach(function (id) { $id(id).value = ''; });
        $id('Add').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelDetail').classList.add('is-hidden');
    }

    /* ------------------------------------------------------------------ grid */

    function comboCell(i, key, list, textKey, v) {
        return '<td><select class="win-combo" onchange="StoreRet.cell(' + i + ',\'' + key + '\',this.value)"><option value="0"></option>' +
            list.map(function (o) { return '<option value="' + o.Id + '"' + (intOf(v) === o.Id ? ' selected' : '') + '>' + esc(o[textKey]) + '</option>'; }).join('') + '</select></td>';
    }
    function render() {
        var iss = anyIssuance();
        var t = $id('grdDetail');
        var cols = [];
        if (iss) cols.push(['IssuanceNo', 'Issuance No']);
        cols.push(['ItemCode', 'Item Code'], ['ItemName', 'Item Name'], ['WareHouseName', 'WareHouse Name', 'pick-wh'], ['RackName', 'Rack Name', 'pick-rack'],
            ['ItemConditionId', 'Item Condition', 'cond'], ['PackUom', 'Pack Uom'], ['BalanceQty', 'Balance Qty', 'n'], ['ItemQty', 'Received Qty', 'e'],
            ['SecondaryUom', 'Secondary Uom', 'pick-sec'], ['SecondaryUomQty', 'Secondary Uom Weight', 'e'], ['PerItemWeight', 'Per Item Weight', 'e'],
            ['SecondaryUomItemRate', 'Secondary Uom Item Rate', 'e'], ['ItemRate', 'Item Rate', 'e'], ['IssuanceRate', 'Issuance Rate', 'e'],
            ['ItemAmount', 'Total Amount', 'n'], ['DepartmentId', 'Department', 'dept'], ['AssetId', 'Asset', 'asset'], ['Remarks', 'Remarks', 't']);
        if (!crColHidden) cols.push(['CreditAccount', 'Credit Account', 'pick-cr']);      // DetailGridSettings never hides it
        if (iss) cols.push(['RecordNo', 'Record No', 'n']);
        t.tHead.innerHTML = '<tr><th></th>' + (iss ? '<th></th>' : '') + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr ondblclick="StoreRet.edit(' + i + ')"><td><button type="button" class="cx-x" onclick="StoreRet.del(' + i + ')">X</button></td>' +
                (iss ? '<td><button type="button" class="cx-link" onclick="StoreRet.split(' + i + ')">+</button></td>' : '') +
                cols.map(function (c) {
                    var k = c[0], v = r[k], m = c[2] || '';
                    if (m === 'cond') return comboCell(i, k, look.itemConditions, 'ItemCondition', v);
                    if (m === 'dept') return comboCell(i, k, look.departments, 'Name', v);
                    if (m === 'asset') return comboCell(i, k, look.assets, 'Name', v);
                    if (m === 'e') return '<td><input class="num" value="' + esc(v) + '" onchange="StoreRet.cell(' + i + ',\'' + k + '\',this.value)"></td>';
                    if (m === 't') return '<td><input value="' + esc(v) + '" onchange="StoreRet.cell(' + i + ',\'' + k + '\',this.value)"></td>';
                    if (m.indexOf('pick-') === 0) return '<td class="pick" tabindex="0" onclick="StoreRet.pick(' + i + ',\'' + m + '\')" ' +
                        'onkeydown="if(event.key===\'F1\'){event.preventDefault();StoreRet.pick(' + i + ',\'' + m + '\')}">' + esc(v || '…') + '</td>';
                    return '<td' + (m === 'n' ? ' class="num"' : '') + '>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $id('DetailGBox').classList.toggle('is-hidden', iss);
    }

    function cell(i, key, value) {                                              // grdDetail_CellUpdated:1204
        var r = table[i];
        if (key === 'Remarks') { r.Remarks = value; return; }
        r[key] = (key === 'ItemConditionId' || key === 'DepartmentId' || key === 'AssetId') ? intOf(value) : num(value);
        var qty = num(r.ItemQty), rate = num(r.ItemRate), perItem = num(r.PerItemWeight), secQty = num(r.SecondaryUomQty), secRate = num(r.SecondaryUomItemRate);
        var p = Promise.resolve();
        if (key === 'ItemQty' && intOf(r.IssuanceId) > 0) {
            validateQuantity(r);
            qty = num(r.ItemQty);
            r.ItemRate = qty > 0 ? secRate * secQty / qty : 0;
        }
        if (key === 'ItemConditionId') p = avgRate(r.ItemId, r.ItemConditionId).then(function (v) { r.ItemRate = v; });
        if (key === 'SecondaryUomQty') r.PerItemWeight = qty > 0 ? secQty / qty : 0;
        if (key === 'PerItemWeight') r.SecondaryUomQty = perItem * qty;
        if (key === 'SecondaryUomItemRate') r.ItemRate = qty > 0 ? secRate * secQty / qty : 0;
        if (key === 'ItemRate') r.SecondaryUomItemRate = secQty > 0 ? rate * (qty / secQty) : 0;
        p.then(function () { r.ItemAmount = num(r.ItemQty) * num(r.ItemRate); render(); }).catch(function (e) { alert(e.message); });
    }
    function validateQuantity(r) {                                              // :1265
        if (!isNew()) return;
        var entered = num(r.ItemQty), others = 0;
        table.forEach(function (x) { if (x !== r && intOf(x.RecordNo) === intOf(r.RecordNo)) others += num(x.ItemQty); });
        var max = num(r.BalanceQty) - others;
        if (entered <= 0) { r.ItemQty = max; alert('Qty must be greater than 0. Reset to max allowed: ' + max); }
        else if (entered > max) { r.ItemQty = max; alert('Qty exceeds allowed balance for RecordNo ' + r.RecordNo + '.\nAlready used: ' + others + ', Max allowed: ' + max + '.'); }
    }
    function split(i) {                                                         // AddDetailrow:1294
        var dr = table[i], bal = num(dr.BalanceQty), qty = num(dr.ItemQty), add = 0;
        if (bal === qty) { alert("Current Row can't be breakable. First Break The Qty of Row"); return; }
        var same = table.filter(function (x) { return intOf(x.RecordNo) === intOf(dr.RecordNo); });
        if (same.length === 1) add = bal - qty;
        else {
            var total = same.reduce(function (s, x) { return s + num(x.ItemQty); }, 0);
            if (total > bal) { alert("Total Qty Of Record#" + dr.RecordNo + " can't be greater than balance Qty Which Is " + bal); return; }
            add = bal - total;
            if (add === 0) { alert("Current Row can't be breakable. First Break The Qty of any Row of RecordNo" + dr.RecordNo); return; }
        }
        var copy = JSON.parse(JSON.stringify(dr));
        copy.ItemQty = add; copy.ItemAmount = add * num(copy.ItemRate);
        table.push(copy); render();
    }
    function del(i) { if (!confirm('Are you sure to Delete?')) return; table.splice(i, 1); render(); }

    function pick(i, what) {                                                    // grdDetail_KeyDown F1:1385
        var r = table[i], itemRacks = look.racks.filter(function (x) { return x.ItemId === r.ItemId; });
        if (what === 'pick-cr') {
            C.pickFrom('Credit Account', look.creditAccounts.filter(function (a) { return a.ItemId === r.ItemId; }), [{ key: 'AccountTitle', caption: 'Account Title' }])
                .then(function (a) { r.CrAccountId = a ? a.Id : 0; r.CreditAccount = a ? a.AccountTitle : ''; render(); });
        } else if (what === 'pick-wh') {
            var whs = C.distinct(itemRacks, 'WarehouseId').map(function (w) { return { Id: w.WarehouseId, WareHouseName: w.WareHouseName }; });
            C.pickFrom('Warehouse', whs, [{ key: 'WareHouseName', caption: 'WareHouse Name' }]).then(function (w) {
                var id = w ? w.Id : 0; r.WareHouseId = id; r.WareHouseName = w ? w.WareHouseName : '';
                var racks = itemRacks.filter(function (x) { return x.WarehouseId === id; });
                if (intOf(r.RackId) > 0) {
                    if (!racks.some(function (x) { return x.Id === r.RackId; })) {
                        if (racks.length === 1) { r.RackId = racks[0].Id; r.RackName = racks[0].RackName; } else { r.RackId = 0; r.RackName = ''; }
                    }
                } else if (racks.length === 1) { r.RackId = racks[0].Id; r.RackName = racks[0].RackName; }
                render();
            });
        } else if (what === 'pick-rack') {
            var wid = intOf(r.WareHouseId);
            var list = C.distinct(itemRacks.filter(function (x) { return wid === 0 || x.WarehouseId === wid; }), 'Id');
            if (!list.length) return;
            C.pickFrom('Rack', list, [{ key: 'RackName', caption: 'Rack Name' }, { key: 'WareHouseName', caption: 'Warehouse Name' }]).then(function (k) {
                r.RackId = k ? k.Id : 0; r.RackName = k ? k.RackName : '';
                if (wid === 0 && k) { r.WareHouseId = k.WarehouseId; r.WareHouseName = k.WareHouseName; }
                render();
            });
        } else if (what === 'pick-sec') {
            C.pickFrom('Secondary Uom', look.uoms.filter(function (u) { return u.ItemId === r.ItemId; }), [{ key: 'UOMCode', caption: 'UOM' }, { key: 'Equivalent', caption: 'Equivalent' }])
                .then(function (u) { r.SecondaryUomId = u ? u.Id : 0; r.SecondaryUom = u ? u.UOMCode : ''; render(); });
        }
    }

    /* ------------------------------------------------------------------ loader */

    function openLoader() {                                                     // btnLoadIssuance_Click:2818
        if (table.length && intOf(table[0].IssuanceId) === 0) { alert("You Can't Load Because direct Entry Already Exist"); return; }
        $id('issFrom').value = C.isoDay(look.financialYearStart) || C.today();
        $id('issTo').value = C.today();                                         // a new frmLoadIssuanceForReturn each click
        C.openModal('dlgIss'); searchLoader();
    }
    function searchLoader() {                                                   // PendingOrderForLoad:125
        loaderPicked = {};
        C.getJson(api + '/pending-issuances' + C.qs({ fromDate: $id('issFrom').value, toDate: $id('issTo').value })).then(function (rows) {
            loaderData = rows || [];
            var groups = [], byId = {};
            loaderData.forEach(function (r) {
                var id = intOf(ci(r, 'Id'));
                if (!byId[id]) { byId[id] = { first: r, iss: 0, rep: 0, bal: 0 }; groups.push(id); }
                byId[id].iss += num(ci(r, 'IssueQty')); byId[id].rep += num(ci(r, 'ReplacementQty')); byId[id].bal += num(ci(r, 'BalanceQty'));
            });
            var t = $id('grdIss');
            t.tHead.innerHTML = '<tr><th></th><th>Doc Date</th><th>Doc No</th><th>Document Type</th><th>Manual No</th><th>Total Issue Qty</th>' +
                '<th>Total Replacement Qty</th><th>Total Balance Qty</th><th>Entry Date</th><th>Entry User</th><th>Modify Date</th><th>Modify User</th><th>Remarks</th></tr>';
            t.tBodies[0].innerHTML = groups.map(function (id) {
                var g = byId[id], f = g.first;
                return '<tr><td><input type="checkbox" onchange="StoreRet.pickDoc(' + id + ',this.checked)"></td><td>' + esc(C.gridDate(ci(f, 'DocDate'))) +
                    '</td><td>' + esc(ci(f, 'DocNo')) + '</td><td>' + esc(ci(f, 'DocumentType')) + '</td><td>' + esc(ci(f, 'ManualNo')) + '</td><td class="num">' +
                    g.iss + '</td><td class="num">' + g.rep + '</td><td class="num">' + g.bal + '</td><td>' + esc(C.gridDateTime(ci(f, 'EntryDate'), true)) +
                    '</td><td>' + esc(ci(f, 'EntryUserName')) + '</td><td>' + esc(C.gridDateTime(ci(f, 'ModifyDate'), true)) + '</td><td>' + esc(ci(f, 'ModifyUserName')) +
                    '</td><td>' + esc(ci(f, 'Remarks')) + '</td></tr>';
            }).join('');
            renderLoaderDetail();
        }).catch(function (e) { alert(e.message); });
    }
    function loaderReset() {                                                    // frmLoadIssuanceForReturn btnReset_Click:310
        ('issFrom').focus();
        ('issFrom').value = C.isoDay(look.financialYearStart) || ('issFrom').value;
    }
    function pickDoc(id, on) {                                                  // BindDetailToDetailGridOfRowId:356 — rows arrive checked
        loaderData.forEach(function (r) { if (intOf(ci(r, 'Id')) === id) loaderPicked[intOf(ci(r, 'DetailId'))] = on ? 'on' : undefined; });
        Object.keys(loaderPicked).forEach(function (k) { if (!loaderPicked[k]) delete loaderPicked[k]; });
        renderLoaderDetail();
    }
    function renderLoaderDetail() {
        var t = $id('grdIssDetail');
        t.tHead.innerHTML = '<tr><th></th><th>Warehouse</th><th>Item Name</th><th>Pack Uom</th><th>Condition</th><th>Asset Name</th><th>Department</th>' +
            '<th>Issue Qty</th><th>Replacement Qty</th><th>Balance Qty</th><th>Item Rate</th><th>Item Amount</th></tr>';
        t.tBodies[0].innerHTML = loaderData.filter(function (r) { return loaderPicked[intOf(ci(r, 'DetailId'))]; }).map(function (r) {
            var d = intOf(ci(r, 'DetailId'));
            return '<tr><td><input type="checkbox" ' + (loaderPicked[d] === 'on' ? 'checked' : '') + ' onchange="StoreRet.toggleDetail(' + d + ',this.checked)"></td><td>' +
                esc(ci(r, 'WareHouseName')) + '</td><td>' + esc(ci(r, 'ItemName')) + '</td><td>' + esc(ci(r, 'PackUom')) + '</td><td>' + esc(ci(r, 'ConditionStatus')) +
                '</td><td>' + esc(ci(r, 'AssetName')) + '</td><td>' + esc(ci(r, 'DepartmentName')) + '</td><td class="num">' + esc(ci(r, 'IssueQty')) +
                '</td><td class="num">' + esc(ci(r, 'ReplacementQty')) + '</td><td class="num">' + esc(ci(r, 'BalanceQty')) + '</td><td class="num">' +
                esc(ci(r, 'ItemRate')) + '</td><td class="num">' + esc(ci(r, 'ItemAmount')) + '</td></tr>';
        }).join('');
    }
    function toggleDetail(d, on) { loaderPicked[d] = on ? 'on' : 'off'; }
    function loadChecked() {                                                    // btnLoadOnInvoice_Click_1:263
        var ids = Object.keys(loaderPicked).filter(function (k) { return loaderPicked[k] === 'on'; }).map(Number);
        if (!ids.length) { alert('Check the Row first'); return; }
        var rows = loaderData.filter(function (r) { return ids.indexOf(intOf(ci(r, 'DetailId'))) >= 0; });
        C.closeModal('dlgIss');
        loadInGrid(rows, false);
    }
    function loadInGrid(rows, asReplacement) {                                  // LoadInGridDetail:2874
        crColHidden = false;                                                    // :2931 grid rebuilt, column visible
        if (!rows || !rows.length) return Promise.resolve();
        var existing = {};
        table.forEach(function (r) { existing[intOf(r.IssuanceDetailId)] = 1; });
        var maxRec = table.reduce(function (m, r) { return Math.max(m, intOf(r.RecordNo)); }, 0), counter = 0;
        var jobs = [];
        rows.forEach(function (dr) {
            var detailId = intOf(ci(dr, 'DetailId'));
            if (existing[detailId]) return;
            existing[detailId] = 1;
            var itemId = intOf(ci(dr, 'ItemId'));
            var sec = look.uoms.find(function (u) { return u.ItemId === itemId && u.BaseSecondaryUom; });
            var bal = num(ci(dr, 'BalanceQty'));
            var row = {
                IssuanceId: intOf(ci(dr, 'Id')), IssuanceDetailId: detailId, IssuanceNo: ci(dr, 'DocNo'),
                WareHouseId: intOf(ci(dr, 'Warehouseid')), WareHouseName: ci(dr, 'WareHouseName'), ItemId: itemId,
                ItemName: itemId > 0 ? ci(dr, 'ItemName') : '', ItemCode: itemId > 0 ? ci(dr, 'ItemCode') : '',
                RackId: intOf(ci(dr, 'RackId')), RackName: ci(dr, 'rackName'),
                ItemConditionId: asReplacement ? (look.itemConditions.length > 0 ? 4 : intOf(ci(dr, 'ItemConditionId'))) : intOf(ci(dr, 'ItemConditionId')),
                PackUomId: intOf(ci(dr, 'ItemUomId')), PackUom: ci(dr, 'PackUom'),
                SecondaryUomId: sec ? sec.Id : 0, SecondaryUom: sec ? sec.UOMCode : '',
                BalanceQty: bal, ItemQty: bal, SecondaryUomQty: 0, PerItemWeight: 0, SecondaryUomItemRate: 0,
                IssuanceRate: num(ci(dr, 'ItemRate')), ItemRate: 0, ItemAmount: 0,
                DepartmentId: intOf(ci(dr, 'DepartmentId')), AssetId: intOf(ci(dr, 'AssetsId')),
                CrAccountId: intOf(ci(dr, 'DrAccountId')), CreditAccount: ci(dr, 'CreditAccount'), Remarks: '',
                RecordNo: maxRec + (++counter)
            };
            table.push(row);
            jobs.push(avgRate(itemId, intOf(ci(dr, 'ItemConditionId'))).then(function (v) { row.ItemRate = v; row.ItemAmount = bal * v; }));
        });
        return Promise.all(jobs).then(render).catch(function (e) { alert(e.message); render(); });
    }

    function docDateChanged() {                                                 // :2943 — the AMOUNT only
        Promise.all(table.map(function (r) {
            return avgRate(r.ItemId, r.ItemConditionId).then(function (v) { r.ItemAmount = num(r.ItemQty) * v; });
        })).then(render).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ save / open */

    function save() { return C.withBusy('btnsave', function () { recId = 0; return insert(); }); }
    function update() { return C.withBusy('btnUpdate', function () { return insert(); }); }
    function insert() {                                                         // Insert():1566
        if (!table.length) alert('Grid record not found');                     // R1 — and carries on, as the desktop does
        var d = $id('txtDocNo').value.trim();
        if (d === '' || intOf(d) === 0) { alert('DocNo Field Required'); $id('txtDocNo').focus(); return Promise.resolve(); }
        if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return Promise.resolve();
        var print = $id('ChkPrintPreview').checked;
        return C.postJson(api + '/save', { Id: recId, DocNo: intOf(d), DocDate: $id('txtDocdate').value, Remarks: $id('txtRemarks').value, rows: table })
            .then(function (res) {
                alert(res.message);
                var code = res.id;
                reset();
                if (print) printId(code);
            }).catch(function (e) { alert(e.message); });
    }
    function printId(id) { if (!id) { alert('No Record Found For Display'); return; } C.printSlip(api + '/' + id + '/slip', 'Store Return Slip'); }

    function readById(id) {                                                     // ReadById:1732
        C.getJson(api + '/' + id).then(function (h) {
            resetDetail();                                                      // ReadById:1736 formReset → ResetDetail
            crColHidden = false;                                                // ReadById:1753 BindGrids
            recId = h.Id;
            tab('tabForm');
            $id('btnsave').classList.add('is-hidden'); $id('btnUpdate').classList.remove('is-hidden');
            $id('txtDocdate').value = C.isoDay(h.DocDate); $id('txtDocNo').value = h.DocNo; $id('txtRemarks').value = h.Remarks || '';
            $id('lblVoucher').textContent = h.voucherHeadId ? ('Voucher #' + h.voucherHeadId) : '';
            $id('lblRecId').textContent = 'Record #' + recId;
            table = h.rows || [];
            render();
        }).catch(function (e) { alert(e.message); });
    }

    /* ------------------------------------------------------------------ history */

    function showHistory(btn) {                                                 // HistoryFill:1958
        return C.withBusy(btn || document.querySelector('#tabHistory .win-btn-action') || $id('btnFooterHistory'), function () {
            return C.getJson(api + '/history' + C.qs({
                fromDate: $id('chkFromDate').checked ? $id('FromDateHistory').value : '',
                toDate: $id('chkToDate').checked ? $id('ToDateHistory').value : '',
                docNoFrom: intOf($id('FromDocNoHistory').value), docNoTo: intOf($id('ToDocNoHistory').value),
                warehouseId: intOf($id('CmbWarehouseHistory').value), itemId: intOf($id('CmbItemNameHistory').value),
                departmentId: intOf($id('CmbDepartmentHistory').value), assetId: intOf($id('CmbAssetNameHistory').value),
                accountId: intOf($id('CmbCreditAccountHistory').value)
            })).then(function (list) {
                var rt = look.rights || {}, t = $id('DataGridHistory');
                t.tHead.innerHTML = '<tr>' + (rt.print ? '<th>Print</th><th>Voucher</th>' : '') + (rt.update ? '<th>Edit</th>' : '') +
                    '<th>Doc Date</th><th>Doc No</th><th>Entry Date</th><th>Entry User Name</th><th>Modify Date</th><th>Modify User Name</th><th>No Of Attachments</th></tr>';
                t.tBodies[0].innerHTML = (list || []).map(function (h) {
                    return '<tr data-id="' + h.Id + '" onclick="StoreRet.historyDetail(this)" ondblclick="StoreRet.open(' + h.Id + ')">' +
                        (rt.print ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();StoreRet.printId(' + h.Id + ')">Print</button></td><td>' +
                            (h.VoucherHeadId ? '#' + h.VoucherHeadId : '') + '</td>' : '') +
                        (rt.update ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();StoreRet.open(' + h.Id + ')">Edit</button></td>' : '') +
                        '<td>' + esc(C.gridDate(h.DocDate)) + '</td><td><a class="cx-link" href="javascript:void(0)" onclick="event.stopPropagation();StoreRet.open(' + h.Id + ')">' + esc(h.DocNo) + '</a></td><td>' + esc(C.gridDateTime(h.EntryDate, true)) + '</td><td>' +
                        esc(h.EntryUserName) + '</td><td>' + esc(C.gridDateTime(h.ModifyDate, true)) + '</td><td>' + esc(h.ModifyUserName) + '</td><td class="num">' +
                        esc(h.NoOfAttachments) + '</td></tr>';
                }).join('');
                $id('gridHistory').tBodies[0].innerHTML = '';
            }).catch(function (e) { alert(e.message); });
        });
    }

    function historyDetail(tr) {
        document.querySelectorAll('#DataGridHistory tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        C.getJson(api + '/' + tr.getAttribute('data-id')).then(function (h) {
            var cols = [['IssuanceNo', 'Issuance No'], ['ItemCode', 'Item Code'], ['ItemName', 'Item Name'], ['WareHouseName', 'WareHouse Name'], ['RackName', 'Rack Name'],
                ['ItemCondition', 'Item Condition'], ['PackUom', 'Pack Uom'], ['ItemQty', 'Received Qty'], ['SecondaryUom', 'Secondary Uom'],
                ['SecondaryUomQty', 'Secondary Uom Weight'], ['PerItemWeight', 'Per Item Weight'], ['SecondaryUomItemRate', 'Secondary Uom Item Rate'],
                ['ItemRate', 'Item Rate'], ['IssuanceRate', 'Issuance Rate'], ['ItemAmount', 'Total Amount'], ['DepartmentName', 'Department'],
                ['AssetName', 'Asset'], ['Remarks', 'Remarks'], ['CreditAccount', 'Credit Account']];
            var t = $id('gridHistory');
            t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = (h.rows || []).map(function (r) { return '<tr>' + cols.map(function (c) { return '<td>' + esc(r[c[0]]) + '</td>'; }).join('') + '</tr>'; }).join('');
        }).catch(function (e) { alert(e.message); });
    }
    function newHistory() {
        ['FromDateHistory', 'ToDateHistory'].forEach(function (id) { $id(id).value = ''; });
        ['CmbWarehouseHistory', 'CmbItemNameHistory', 'CmbDepartmentHistory', 'CmbAssetNameHistory', 'CmbCreditAccountHistory'].forEach(function (id) { $id(id).value = '0'; });
        $id('DataGridHistory').tBodies[0].innerHTML = ''; $id('gridHistory').tBodies[0].innerHTML = '';
    }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    /* ------------------------------------------------------------------ init */

    function init() {
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            var r = l.rights || {};
            $id('btnsave').disabled = !r.save; $id('Print').disabled = !r.print; $id('btnUpdate').disabled = !r.update;
            if (!loaded) $id('ChkPrintPreview').checked = !!r.print;             // Load:382 only
            if (!r.view) { $id('rightsNote').textContent = 'You do not have the View right for Store Return.'; $id('rightsNote').classList.remove('is-hidden'); }
            if (l.itemSearchByCode) $id('rdSearchByCode').checked = true; else $id('rdSearchByName').checked = true;
            $id('fldCrAc').classList.toggle('is-hidden', !l.storeFinancialEffects);
            if (!recId) $id('txtDocNo').value = l.docNo || '';
            itemBind();
            C.fillSelect('CmbItemCondition', l.itemConditions, 'Id', 'ItemCondition');
            /* Load:407 Rows[4].Activate() (blank default row first) — first load only; Refresh and
               New keep the selection (BindAndRetainSelection). */
            if (!loaded && l.itemConditions.length >= 4) $id('CmbItemCondition').value = String(l.itemConditions[3].Id);
            C.fillSelect('CmbDepartment', l.departments, 'Id', 'Name');
            C.fillSelect('cmbAssetsRef', l.assets, 'Id', 'Name');
            var hf = l.historyFilters || {};
            C.fillSelect('CmbWarehouseHistory', hf.Warehouse, 'Id', 'Name'); C.fillSelect('CmbItemNameHistory', hf.ItemName, 'Id', 'Name');
            C.fillSelect('CmbDepartmentHistory', hf.Department, 'Id', 'Name'); C.fillSelect('CmbAssetNameHistory', hf.Asset, 'Id', 'Name');
            C.fillSelect('CmbCreditAccountHistory', hf.CreditAccount, 'Id', 'Name');
            if (!loaded) {                                                      // Load:405 only
                var days = intOf(l.defaultDaysToLessFromHistoryFromDate) > 0 ? intOf(l.defaultDaysToLessFromHistoryFromDate) : 3;
                var f = new Date(); f.setDate(f.getDate() - days);
                $id('FromDateHistory').value = f.getFullYear() + '-' + String(f.getMonth() + 1).padStart(2, '0') + '-' + String(f.getDate()).padStart(2, '0');
                $id('ToDateHistory').value = C.today();
            }
            loaded = true;
            render();
        }).catch(function (e) { alert(e.message); });
    }
    function reset() {                                                          // formReset:801
        recId = 0; table = []; crColHidden = false;                             // BindGrids: column visible again; flags are static: kept
        $id('txtRemarks').value = '';                                           // txtDocdate kept (formReset:801)
        $id('btnsave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden');
        $id('lblRecId').textContent = ''; $id('lblVoucher').textContent = '';
        return init().then(resetDetail);
    }

    function refresh() {                                                        // btnRefresh_Click:864
        return init().then(function () {
            /* :876 Visible = StoreFinancialEffects — hides AND shows again, only when rows exist. */
            if (table.length) { crColHidden = !look.storeFinancialEffects; render(); }
        });
    }

    var params = new URLSearchParams(window.location.search);
    var printIssuanceId = intOf(params.get('printIssuance'));
    /* The issuance that opened this page: 322 (451) or 321 (452) — each has its own slip route. */
    var printIssuanceApi = params.get('printScreen') === 'direct' ? '/api/store/issuance/direct/' : '/api/store/issuance/issuance/';

    window.StoreRet = {
        reset: reset, save: save, update: update, refresh: refresh, searchModeChanged: searchModeChanged, tab: tab, open: readById,
        print: function () { printId(recId); }, printId: printId,
        printIssuance: function () { C.printSlip(printIssuanceApi + printIssuanceId + '/slip', 'Store Issuance Slip'); },
        itemBind: itemBind, itemLeave: itemLeave, rackLeave: rackLeave, conditionLeave: conditionLeave,
        qtyChanged: qtyChanged, secQtyChanged: secQtyChanged, rateChanged: rateChanged,
        add: add, updateDetail: updateDetail, resetDetail: resetDetail, edit: edit, del: del, split: split, cell: cell, pick: pick,
        openLoader: openLoader, searchLoader: searchLoader, loaderReset: loaderReset, pickDoc: pickDoc, toggleDetail: toggleDetail, loadChecked: loadChecked,
        docDateChanged: docDateChanged, showHistory: showHistory, historyDetail: historyDetail, newHistory: newHistory
    };

    document.addEventListener('DOMContentLoaded', function () {
        $id('txtDocdate').value = C.today();                                    // designer: DateTime.Now, once
        reset().then(function () {
            var issuanceId = intOf(params.get('issuanceId'));
            if (issuanceId > 0) {                                               // LoadIssuanceByRecId:2849
                C.getJson(api + '/pending-issuances' + C.qs({ issuanceId: issuanceId })).then(function (rows) { return loadInGrid(rows, true); })
                    .catch(function (e) { alert(e.message); });
            }
            if (printIssuanceId > 0) $id('btnIssuanceSlip').classList.remove('is-hidden');
        });
    });
})();
