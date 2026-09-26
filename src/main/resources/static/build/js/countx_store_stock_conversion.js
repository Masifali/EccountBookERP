/* ============================================================================================
 * Store Stock Conversion - screen 502, module 54, DocTypeId 808.
 * Desktop: Architecture.WinApp.Production.StoreStockConversion (+ LoadavailableTransactionsForConversionStore).
 *
 *   Load :309   History :397   RefreshForm :490   resetDetail :521   BindInputItem :620   cmbItem_Leave :727
 *   BindWarehouseDropdown :744   RackBind :788   bindItemPackUom :860   GetAvgRate :913   AvailableStock :962
 *   Insert :1073   grdInput_DoubleClick :1275   ReadById :1325   FormValidationOfDetailPortion :1404
 *   AddInGrid :1466   AmountCalculation :1617   btnUpdateDetail :1699   cmbEntryType_Leave :1862
 *   grdByProduct_DoubleClick :1946   handleAverageRateCalculation :2052   GenerateSummaryForUser :2089
 *   LoadDataDetailfromPurchaseInvoivce :2189   grdInput_CellUpdated :2240   grdByProduct_CellUpdated :2340
 *   AvgRateUpdateOnDocDateChange :2454   BtnForwardRows :2542   btnUpdateItemCondition :2593   grdOutput_KeyDown(F1) :2646
 * The server repeats the Insert() checks.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/store-stock-conversion';
    var L = null, R = {};
    var table = [], tableOutput = [], RecId = 0, updateDetailIndex = -1, updateDetailIndexOutput = -1;
    var DetailEditMood = false, historyRows = [], currentTab = 0, ld = { rows: [], data: [] };

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
    function ask(m) { return window.confirm(m); }
    function esc(s) {
        return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    function dp() { return int(L && L.amountDecimals) || 2; }
    function fixed(n, d) { return num(n).toLocaleString('en-US', { minimumFractionDigits: d, maximumFractionDigits: d }); }
    function qtyFmt(n) { return num(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 }); }
    function amtFmt(n) { return fixed(n, dp()); }
    function rateFmt(n) { return fixed(n, 3); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function ddmmm(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0]; }
    function dt(v) { if (!v) return ''; var d = new Date(String(v).replace(' ', 'T')); if (isNaN(d.getTime())) return String(v);
        var h = d.getHours(), ap = h >= 12 ? 'PM' : 'AM'; h = h % 12 || 12;
        return ddmmm(iso(d)) + ' ' + String(h).padStart(2, '0') + ':' + String(d.getMinutes()).padStart(2, '0') + ' ' + ap; }

    function busy(btn, fn) {
        var b = (typeof btn === 'string') ? $id(btn) : btn;
        if (b) { if (b.disabled || b.classList.contains('is-busy')) return; b.disabled = true; b.classList.add('is-busy'); }
        var done = function () { if (b) { b.disabled = false; b.classList.remove('is-busy'); } applyRights(); };
        var p; try { p = fn(); } catch (e) { done(); throw e; }
        if (p && typeof p.then === 'function') p.then(done, done); else done();
        return p;
    }
    function http(method, url, body) {
        var opt = { method: method, headers: { 'Accept': 'application/json' }, credentials: 'same-origin' };
        if (body !== undefined) { opt.headers['Content-Type'] = 'application/json'; opt.body = JSON.stringify(body); }
        return fetch(url, opt).then(function (r) {
            return r.text().then(function (t) {
                var b = null; try { b = t ? JSON.parse(t) : null; } catch (e) { /* not json */ }
                if (!r.ok) throw new Error((b && b.message) || ('Request failed (' + r.status + ')'));
                return b;
            });
        });
    }
    function getJson(url) { return http('GET', url); }
    function fill(id, rows, v, t, blank) {
        var sel = $id(id); if (!sel) return; var keep = sel.value;
        var html = blank === false ? '' : '<option value=""></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, v)) + '">' + esc(col(r, t)) + '</option>'; });
        sel.innerHTML = html;
        if (keep && sel.querySelector('option[value="' + String(keep).replace(/"/g, '') + '"]')) sel.value = keep;
    }
    function has(id, v) { var s = $id(id); return !!(s && v !== '' && v !== null && s.querySelector('option[value="' + String(v).replace(/"/g, '') + '"]')); }
    function text(id) { var s = $id(id); return s && s.selectedIndex >= 0 && s.options[s.selectedIndex] && s.value !== '' ? s.options[s.selectedIndex].text : ''; }
    function show(id, on) { var e = $id(id); if (e) e.style.display = on ? '' : 'none'; }
    function focus(id) { var e = $id(id); if (e) try { e.focus(); } catch (x) { /* ignore */ } }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function byId(list, id) { id = int(id); return (list || []).filter(function (x) { return int(col(x, 'Id')) === id; })[0] || null; }
    function saveMode() { return $id('btnsave').style.display !== 'none' && !$id('btnsave').disabled; }

    // ------------------------------------------------------------------ load

    function init() {
        bindEvents();
        setVal('txtDocdate', today()); setVal('FromDateHistory', today()); setVal('ToDateHistory', today());
        loadLookups().then(function () { renderAll(); });
    }

    function loadLookups() {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d; R = d.rights || {};
            fill('cmbParentCategory', d.parentCategories, 'Id', 'Name', false);                 // Rows[0].Activate()
            fill('cmbEntryType', [{ Id: 1, EntryType: 'Input' }, { Id: 2, EntryType: 'Output' }], 'Id', 'EntryType');
            fill('cmbItemCondition', d.conditions, 'Id', 'Description');
            fill('cmbLot', d.jobLots, 'Id', 'Name');
            if (int(d.defaultJobLot) > 0 && has('cmbLot', d.defaultJobLot)) setVal('cmbLot', d.defaultJobLot);
            if (RecId === 0) { setVal('txtdocnumber', d.docNo); $id('txtDocNoShow').textContent = 'SC-' + d.docNo; }
            refreshCombos(); applyRights(); say('');
        }).catch(function (e) { say(''); box(e.message); });
    }

    function applyRights() {
        var upd = RecId > 0;
        show('btnsave', !upd); show('btnUpdate', upd);
        $id('btnsave').disabled = !R.save; $id('btnUpdate').disabled = !R.update; $id('Print').disabled = !R.print;
    }

    // ------------------------------------------------------------------ entry bar

    function entryType() { return int(val('cmbEntryType')); }

    /* cmbEntryType_Leave :1862 */
    function entryTypeLeave() {
        if (text('cmbParentCategory') === '') { box('Please Select Parent Category First!'); focus('cmbParentCategory'); return; }
        if (text('cmbEntryType') === '') { box('Please Select Entry Type First!'); focus('cmbEntryType'); return; }
        $id('txtRate').disabled = !(entryType() !== 1 || (!L.fifo && !L.cgsEntryAllow));
        if (entryType() === 1) BindInputItem(); else if (entryType() === 2) ItemNameBind(); else fill('cmbItem', [], 'Id', 'ItemName');
        refreshCombos();
    }
    /* cmbParentCategory_Leave :2621 */
    function parentLeave() {
        if (entryType() === 1) BindInputItem(); else if (entryType() === 2) ItemNameBind(); else fill('cmbItem', [], 'Id', 'ItemName');
        refreshCombos();
    }
    /* BindInputItem :620 */
    function BindInputItem() {
        var pc = int(val('cmbParentCategory'));
        fill('cmbItem', (L.inputItems || []).filter(function (r) { return pc <= 0 || int(r.ParentCategoryId) === pc; }), 'Id', 'ItemName');
    }
    function ItemNameBind() { fill('cmbItem', L.outputItems, 'Id', 'ItemName'); }

    /* bindItemPackUom :860 */
    function bindItemPackUom() {
        var keep = text('CmbPackUom');
        var list = (L.uoms || []).filter(function (u) { return int(u.ItemId) === int(val('cmbItem')); });
        fill('CmbPackUom', list, 'Id', 'UOMCode');
        var m = list.filter(function (u) { return String(u.UOMCode) === keep; })[0];
        setVal('CmbPackUom', m && keep ? m.Id : '');
        if (int(val('CmbPackUom'))) return;
        if (list.length === 1) setVal('CmbPackUom', list[0].Id);
        else if (list.length > 1) { var e = list.filter(function (u) { return num(u.Equivalent) === 1; })[0]; if (e) setVal('CmbPackUom', e.Id); }
    }
    /* BindWarehouseDropdown :744 */
    function bindWarehouseDropdown(itemId) {
        var seen = {}, list = [];
        (L.racks || []).forEach(function (r) { if (int(r.ItemId) === itemId && !seen[r.WarehouseId]) { seen[r.WarehouseId] = 1; list.push({ Id: r.WarehouseId, Warehouse: r.WareHouseName }); } });
        fill('cmbGodown', list, 'Id', 'Warehouse');
        if (int(val('cmbGodown'))) return;
        if (list.length === 1) setVal('cmbGodown', list[0].Id);
        else if (list.length > 1) {
            var it = byId(L.outputItems, itemId), pc = it ? int(it.ParentCategoryId) : 0;
            var cfg = pc === 7 ? int(L.packingMaterialDefaultWarehouse) : pc === 8 ? int(L.defaultWarehouseForStoreFlow) : 0;
            if (cfg && list.some(function (w) { return int(w.Id) === cfg; })) setVal('cmbGodown', cfg);
        }
    }
    /* RackBindFromGlobalRacksByItemId :788 */
    function rackBind(itemId, whId) {
        var seen = {}, list = [];
        (L.racks || []).forEach(function (r) { if (int(r.ItemId) === itemId && (!whId || int(r.WarehouseId) === whId) && !seen[r.Id]) { seen[r.Id] = 1; list.push(r); } });
        fill('CmbRackName', list, 'Id', 'RackName');
        if (int(val('CmbRackName'))) return null;
        var pick = null;
        if (list.length === 1) pick = list[0];
        else if (list.length > 1) pick = list.filter(function (r) { return int(r.BaseRackId) > 0; })[0] || null;
        if (pick) { setVal('CmbRackName', pick.Id); return rackLeave(); }
        return null;
    }
    /* CmbRackName_Leave :838 */
    function rackLeave() {
        var r = byId(L.racks, val('CmbRackName'));
        if (!r) return null;
        if (!int(val('cmbGodown')) || int(val('cmbGodown')) !== int(r.WarehouseId)) {
            if (!has('cmbGodown', r.WarehouseId)) fill('cmbGodown', [{ Id: r.WarehouseId, Warehouse: r.WareHouseName }], 'Id', 'Warehouse');
            setVal('cmbGodown', r.WarehouseId);
        }
        return AvailableStock();
    }
    /* cmbItem_Leave :727 */
    function itemLeave() {
        var itemId = int(val('cmbItem'));
        bindItemPackUom();
        var p = [AvailableStock(), GetAvgRate()];
        bindWarehouseDropdown(itemId);
        p.push(rackBind(itemId, int(val('cmbGodown'))));
        refreshCombos();
        return Promise.all(p);
    }
    function stockCall() {
        var q = '?recId=' + RecId + '&itemId=' + int(val('cmbItem')) + '&conditionId=' + int(val('cmbItemCondition'))
            + '&warehouseId=' + int(val('cmbGodown')) + '&rackId=' + int(val('CmbRackName')) + '&docDate=' + encodeURIComponent(val('txtDocdate'));
        return getJson(api + '/stock' + q);
    }
    /* AvailableStockGetByItem :962 */
    function AvailableStock() {
        return stockCall().then(function (d) { setVal('txtStock', num(d.stock) > 0 ? qtyFmt(d.stock) : '0'); }).catch(function (e) { box(e.message); });
    }
    /* GetAvgRate :913 (the condition is read as 0 - see the service note D1) */
    function GetAvgRate() {
        return stockCall().then(function (d) {
            var rate = num(d.avgRate), fc = L.fifo && L.cgsEntryAllow;
            if (rate > 0) {
                setVal('txtAverageRate', rateFmt(rate));
                if (fc) setVal('txtRate', rateFmt(rate)); else if (!DetailEditMood) setVal('txtRate', rateFmt(rate));
            } else {
                setVal('txtAverageRate', '0');
                if (fc) setVal('txtRate', '0'); else if (saveMode()) setVal('txtRate', '0');
            }
            AmountCalculation();
        }).catch(function (e) { box(e.message); });
    }
    /* AmountCalculation :1617 */
    function AmountCalculation() {
        var q = num(val('txtQty')), r = num(val('txtRate'));
        setVal('txtAmount', r > 0 && q > 0 ? amtFmt(q * r) : '0');
    }

    /* FormValidationOfDetailPortion :1404 */
    function FormValidationOfDetailPortion() {
        if (!entryType()) { box('Entry Type Field Required'); focus('cmbEntryType'); return false; }
        if (!int(val('cmbItem'))) { box('Item Field Field Required'); focus('cmbItem'); return false; }
        if (!int(val('cmbGodown'))) { box('Warehouse Field Required'); focus('cmbGodown'); return false; }
        if (!int(val('CmbRackName'))) { box('RackName Field Required'); focus('CmbRackName'); return false; }
        if (!int(val('cmbItemCondition'))) { box('Item Condition Field Field Required'); focus('cmbItemCondition'); return false; }
        if (!int(val('CmbPackUom'))) { box('Uom Field Field Required'); focus('CmbPackUom'); return false; }
        if (entryType() === 1) {
            if (num(val('txtRate')) === 0) { box('Rate Field Required'); focus('txtRate'); return false; }
            if (num(val('txtAmount')) === 0) { box('Amount Field Required'); focus('txtAmount'); return false; }
        }
        if (!int(val('cmbLot'))) { box('Job Lot Field Required'); focus('cmbLot'); return false; }
        return true;
    }
    function balanceExceeded(itemId, whId, rackId, condId, qty, skip, withRack) {
        var match = table.filter(function (r) { return int(r.ItemId) === itemId && int(r.WareHouseId) === whId && (!withRack || int(r.RackId) === rackId) && int(r.ItemConditionId) === condId; });
        if (!match.length) return false;
        var bal = num(match[0].BalQty), sum = 0;
        table.forEach(function (r, i) { if (i !== skip && int(r.ItemId) === itemId && int(r.WareHouseId) === whId && (!withRack || int(r.RackId) === rackId) && int(r.ItemConditionId) === condId) sum += num(r.Quantity); });
        if (sum + qty > bal) return { bal: bal, sum: sum };
        return false;
    }
    function maxLine() { var m = 0; table.forEach(function (r) { if (r.LineId !== null && r.LineId !== undefined && r.LineId !== '') m = Math.max(m, int(r.LineId)); }); return m; }
    function barRow() {
        return { EntryType: String(entryType()), ItemId: int(val('cmbItem')), ItemName: text('cmbItem'), WareHouseId: int(val('cmbGodown')), WareHouse: text('cmbGodown'),
            RackId: int(val('CmbRackName')), RackName: text('CmbRackName'), ItemConditionId: int(val('cmbItemCondition')), ItemCondition: text('cmbItemCondition'),
            ItemUOMId: int(val('CmbPackUom')), UOM: text('CmbPackUom'), JobLotId: int(val('cmbLot')), JobLot: text('cmbLot'),
            Quantity: num(val('txtQty')), Rate: num(val('txtRate')), Amount: num(val('txtAmount')), Remarks: val('txtdeailRemarks') };
    }
    /* AddInGrid_Click :1466 */
    function AddInGrid_Click() {
        if (!FormValidationOfDetailPortion()) return;
        if (text('cmbEntryType') === 'Input') {
            if (num(val('txtStock')) === 0 && !ask('This item has Zero Stock. Quantity validations may not work correctly.\nDo you still want to continue?')) return;
            var qty = num(val('txtQty'));
            if (saveMode()) {
                var x = balanceExceeded(int(val('cmbItem')), int(val('cmbGodown')), int(val('CmbRackName')), int(val('cmbItemCondition')), qty, -1, true);
                if (x) { box('Entered Quantity (' + qty + ') exceeds Available Balance. \nBalance Qty : ' + x.bal + '\nAlready Used : ' + x.sum + '\nRemaining    : ' + (x.bal - x.sum)); return; }
            }
            var r = barRow(); r.Id = 0; r.RefDocumentTypeId = 0; r.RefDocNoId = 0; r.RefDocSubId = 0; r.BalQty = num(val('txtStock')); r.LineId = maxLine() + 1;
            table.push(r);
            resetDetail(); DetailEditMood = false;
        } else {
            var o = barRow(); o.Id = 0; o.LineId = null;                                     // 18 values for 19 columns: LineId stays DBNull
            tableOutput.push(o);
            resetDetail();
        }
        GenerateSummaryForUser(); renderAll(); focus('cmbEntryType');
    }
    /* btnUpdateDetail_Click :1699 */
    function btnUpdateDetail_Click() {
        if (!FormValidationOfDetailPortion()) return;
        if (text('cmbEntryType') === 'Input') {
            if (num(val('txtStock')) === 0 && !ask('This item has Zero Stock. Quantity validations may not work correctly.\nDo you still want to continue?')) return;
            var qty = num(val('txtQty'));
            if (saveMode()) {
                var x = balanceExceeded(int(val('cmbItem')), int(val('cmbGodown')), 0, int(val('cmbItemCondition')), qty, updateDetailIndex, false);
                if (x) { box('Entered Quantity (' + qty + ') exceeds Available Balance. \nBalance Qty : ' + x.bal + '\nAlready Used : ' + x.sum + '\nRemaining    : ' + (x.bal - x.sum)); return; }
            }
            var t = table[updateDetailIndex];
            if (!t) { box('There is no row at position ' + updateDetailIndex + '.'); return; }
            /* :1731-1736 - the fifth write targets column "RckId", which the table does not have. */
            t.EntryType = String(entryType()); t.ItemId = int(val('cmbItem')); t.ItemName = text('cmbItem');
            t.WareHouseId = int(val('cmbGodown')); t.WareHouse = text('cmbGodown');
            renderAll();
            box("Column 'RckId' does not belong to table .");
            return;
        }
        var o = tableOutput[updateDetailIndexOutput];
        if (!o) { box('There is no row at position ' + updateDetailIndexOutput + '.'); return; }
        var b = barRow();
        for (var k in b) o[k] = b[k];
        show('btnUpdateDetail', false); show('btnCancelDetail', false); show('Add', true);
        resetDetail(); GenerateSummaryForUser(); renderAll(); focus('cmbEntryType');
    }
    function btnCancelDetail_Click() { show('Add', true); show('btnUpdateDetail', false); show('btnCancelDetail', false); resetDetail(); renderAll(); }
    /* resetDetail :521 */
    function resetDetail() {
        updateDetailIndex = -1; updateDetailIndexOutput = -1;
        setVal('cmbItem', ''); fill('cmbGodown', [], 'Id', 'Warehouse'); fill('CmbRackName', [], 'Id', 'RackName');
        setVal('txtAverageRate', ''); setVal('txtQty', ''); setVal('txtStock', '0'); setVal('txtRate', ''); setVal('txtAmount', ''); setVal('txtdeailRemarks', '');
        refreshCombos();
    }
    /* grdInput_DoubleClick :1275 / grdByProduct_DoubleClick :1946 */
    function editRow(isInput, i) {
        var r = isInput ? table[i] : tableOutput[i]; if (!r) return;
        if (isInput) { DetailEditMood = true; updateDetailIndex = i; } else updateDetailIndexOutput = i;
        setVal('cmbEntryType', int(r.EntryType));
        if (entryType() === 1) BindInputItem(); else if (entryType() === 2) ItemNameBind();
        setVal('cmbItem', r.ItemId);
        setVal('cmbGodown', ''); setVal('CmbRackName', '');
        itemLeave();
        setVal('cmbGodown', r.WareHouseId); setVal('CmbRackName', r.RackId);
        setVal('cmbItemCondition', r.ItemConditionId); setVal('cmbLot', r.JobLotId);
        setVal('txtQty', isInput ? qtyFmt(r.Quantity) : qtyFmt(r.Quantity)); setVal('txtRate', rateFmt(r.Rate)); setVal('txtAmount', amtFmt(r.Amount));
        setVal('txtRemarks', r.Remarks);                                                     // D3: the HEADER remarks box
        show('Add', false); show('btnUpdateDetail', true); show('btnCancelDetail', true);
        refreshCombos(); renderAll(); focus('cmbEntryType');
    }
    /* grdStockConversionProductionDetail_ColumnButtonClick :2282 / grdByProduct_ColumnButtonClick :2311 */
    function deleteRow(isInput, i) {
        if (!saveMode()) return;
        if ((isInput ? updateDetailIndex : updateDetailIndexOutput) !== -1) { box("You can't delete the record because detail in edit mode"); return; }
        (isInput ? table : tableOutput).splice(i, 1);
        GenerateSummaryForUser(); renderAll();
    }

    /* handleAverageRateCalculation :2052 */
    function handleAverageRateCalculation() {
        var inAmt = 0, outQty = 0;
        table.forEach(function (r) { inAmt += num(r.Amount); });
        tableOutput.forEach(function (r) { outQty += num(r.Quantity); });
        var rate = inAmt / outQty;
        tableOutput.forEach(function (r) { r.Rate = rate; r.Amount = num(r.Quantity) * rate; });
        GenerateSummaryForUser();
    }
    /* GenerateSummaryForUser :2089 */
    function GenerateSummaryForUser() {
        ['txtInputAmount', 'txtInputQuantity', 'txtInputRate', 'txtOutputByProductAmount', 'txtOutputByProductQty', 'txtOutputByProductRate'].forEach(function (k) { setVal(k, ''); });
        if (table.length > 0) {
            var q = 0, r = 0, a = 0;
            table.forEach(function (x) { q += num(x.Quantity); r += num(x.Rate); a += num(x.Amount); });
            setVal('txtInputQuantity', qtyFmt(q)); setVal('txtInputRate', rateFmt(r)); setVal('txtInputAmount', amtFmt(a));
        }
        var oq = 0, or = 0, oa = 0;
        tableOutput.forEach(function (x) { oq += num(x.Quantity); or += (isFinite(num(x.Rate)) ? num(x.Rate) : 0); oa += (isFinite(num(x.Amount)) ? num(x.Amount) : 0); });
        setVal('txtOutputByProductQty', qtyFmt(oq)); setVal('txtOutputByProductRate', rateFmt(or)); setVal('txtOutputByProductAmount', amtFmt(oa));
    }

    /* grdInput_CellUpdated :2240 */
    function inputQtyChanged(i, v) {
        var r = table[i]; if (!r) return;
        r.Quantity = num(v);
        if (saveMode()) {
            var x = balanceExceeded(int(r.ItemId), int(r.WareHouseId), 0, int(r.ItemConditionId), r.Quantity, i, false);
            if (x) { box('Entered Quantity (' + r.Quantity + ') exceeds Available Balance.\nBalance Qty  : ' + x.bal + '\nAlready Used : ' + x.sum + '\nRemaining    : ' + (x.bal - x.sum)); r.Quantity = x.bal - x.sum; }
        }
        r.Amount = r.Quantity * num(r.Rate);
        GenerateSummaryForUser(); renderAll();
    }
    /* grdByProduct_CellUpdated :2340 */
    function outputQtyChanged(i, v) {
        var r = tableOutput[i]; if (!r) return;
        r.Quantity = num(v); r.Amount = r.Quantity * num(r.Rate);
        handleAverageRateCalculation(); GenerateSummaryForUser(); renderAll();
    }
    /* grdOutput_KeyDown F1 pickers :2646 - offered as cell selects. */
    function outputPick(i, field, v) {
        var r = tableOutput[i]; if (!r) return;
        var itemId = int(r.ItemId);
        if (field === 'cond') { var c = byId(L.conditions, v); r.ItemConditionId = int(v); r.ItemCondition = c ? c.Description : ''; }
        else if (field === 'wh') {
            var w = (L.racks || []).filter(function (x) { return int(x.ItemId) === itemId && int(x.WarehouseId) === int(v); });
            r.WareHouseId = int(v); r.WareHouse = w.length ? w[0].WareHouseName : '';
            var rackId = int(r.RackId);
            if (rackId > 0) {
                if (!w.some(function (x) { return int(x.Id) === rackId; })) {
                    if (w.length === 1) { r.RackId = w[0].Id; r.RackName = w[0].RackName; } else { r.RackId = 0; r.RackName = ''; }
                }
            } else if (w.length === 1) { r.RackId = w[0].Id; r.RackName = w[0].RackName; }
        } else if (field === 'rack') {
            var rk = byId(L.racks, v);
            r.RackId = int(v); r.RackName = rk ? rk.RackName : '';
            if (int(r.WareHouseId) === 0 && rk) { r.WareHouseId = rk.WarehouseId; r.WareHouse = rk.WareHouseName; }
        }
        renderAll();
    }

    /* BtnForwardRowsFromInputToOutPut_Click :2542 */
    function forwardRows() {
        if (table.length === 0) { box('Input Grid Have no Rows..'); return; }
        var checked = table.filter(function (r) { return r._checked; });
        if (!checked.length) { box('Please Check any Row First...'); return; }
        checked.forEach(function (r) {
            var line = int(r.LineId);
            if (tableOutput.some(function (o) { return int(o.LineId) === line; })) return;
            tableOutput.push({ Id: 0, EntryType: '2', ItemId: int(r.ItemId), ItemName: r.ItemName, WareHouseId: int(r.WareHouseId), WareHouse: r.WareHouse,
                RackId: int(r.RackId), RackName: r.RackName, ItemConditionId: 0, ItemCondition: '', ItemUOMId: int(r.ItemUOMId), UOM: r.UOM,
                JobLotId: int(r.JobLotId), JobLot: r.JobLot, Quantity: num(r.Quantity), Rate: num(r.Rate), Amount: num(r.Amount), Remarks: '', LineId: line });
        });
        if (tableOutput.length) GenerateSummaryForUser();
        renderAll();
    }
    /* btnUpdateItemCondition_Click :2593 */
    function updateItemCondition() {
        var id = int(val('cmbItemCondition')), name = text('cmbItemCondition');
        tableOutput.forEach(function (r) { if (int(r.ItemConditionId) === 0) { r.ItemConditionId = id; r.ItemCondition = name; } });
        renderAll();
    }

    // ------------------------------------------------------------------ grids

    var IN_COLS = [['ItemName', 'ItemName'], ['WareHouse', 'WareHouse'], ['RackName', 'RackName'], ['ItemCondition', 'ItemCondition'], ['UOM', 'UOM'],
        ['JobLot', 'JobLot'], ['Quantity', 'Quantity', 'q'], ['Rate', 'Rate', 'r'], ['Amount', 'Amount', 'a'], ['Remarks', 'Remarks']];
    function cell(c, r) { var v = r[c[0]]; return c[2] === 'q' ? qtyFmt(v) : c[2] === 'r' ? rateFmt(v) : c[2] === 'a' ? amtFmt(v) : v; }
    function renderAll() {
        $id('inHead').innerHTML = '<th>&#10003;</th><th>X</th>' + IN_COLS.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('');
        $id('grdInput').innerHTML = table.map(function (r, i) {
            return '<tr class="data-row' + (i === updateDetailIndex ? ' editing' : '') + '" data-i="' + i + '"><td><input type="checkbox" data-act="chk"' + (r._checked ? ' checked' : '') + '/></td>'
                + '<td><button type="button" class="win-btn-mini" data-act="del">X</button></td>'
                + IN_COLS.map(function (c) {
                    if (c[0] === 'Quantity') return '<td><input class="cell" data-act="qty" value="' + esc(qtyFmt(r.Quantity)) + '"/></td>';
                    return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(cell(c, r)) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $id('outHead').innerHTML = '<th>X</th>' + IN_COLS.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('');
        $id('grdOutput').innerHTML = tableOutput.map(function (r, i) {
            var itemId = int(r.ItemId);
            var whs = [], seen = {};
            (L && L.racks || []).forEach(function (x) { if (int(x.ItemId) === itemId && !seen[x.WarehouseId]) { seen[x.WarehouseId] = 1; whs.push({ Id: x.WarehouseId, Name: x.WareHouseName }); } });
            var racks = (L && L.racks || []).filter(function (x) { return int(x.ItemId) === itemId && (!int(r.WareHouseId) || int(x.WarehouseId) === int(r.WareHouseId)); });
            function sel(act, list, cur, name, label) {
                return '<select class="cell-sel" data-act="' + act + '"><option value="">' + esc(label || '') + '</option>' + list.map(function (x) {
                    return '<option value="' + esc(x.Id) + '"' + (int(x.Id) === int(cur) ? ' selected' : '') + '>' + esc(x[name]) + '</option>'; }).join('') + '</select>';
            }
            return '<tr class="data-row' + (i === updateDetailIndexOutput ? ' editing' : '') + '" data-i="' + i + '"><td><button type="button" class="win-btn-mini" data-act="del">X</button></td>'
                + IN_COLS.map(function (c) {
                    if (c[0] === 'Quantity') return '<td><input class="cell" data-act="qty" value="' + esc(qtyFmt(r.Quantity)) + '"/></td>';
                    if (c[0] === 'WareHouse') return '<td>' + sel('wh', whs, r.WareHouseId, 'Name', r.WareHouse) + '</td>';
                    if (c[0] === 'RackName') return '<td>' + sel('rack', racks, r.RackId, 'RackName', r.RackName) + '</td>';
                    if (c[0] === 'ItemCondition') return '<td>' + sel('cond', L ? L.conditions : [], r.ItemConditionId, 'Description', r.ItemCondition) + '</td>';
                    return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(cell(c, r)) + '</td>';
                }).join('') + '</tr>';
        }).join('');
    }

    // ------------------------------------------------------------------ save / read

    /* Insert :1073 */
    function Insert(btn) {
        if (!int(val('txtdocnumber'))) { box('document Number Field Required'); focus('txtdocnumber'); return; }
        if (val('txtProductionNo').trim() === '') { box('Production NO. Field Required'); focus('txtProductionNo'); return; }
        if (!ask(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        if (table.length <= 0) { box('At Least 1 Entry Of Input is Required in Input Grid'); return; }
        if (tableOutput.length <= 0) { box('At Least 1 Entry Of Output is Required in Output Grid'); return; }
        if (num(val('txtInputAmount')) !== num(val('txtOutputByProductAmount'))) { box('Total Input Amount Should Be Equal To Total Output Amount'); return; }
        var strip = function (r) { var o = {}; for (var k in r) if (k.charAt(0) !== '_') o[k] = r[k]; return o; };
        var body = { Id: RecId, DocDate: val('txtDocdate'), ProductionNo: val('txtProductionNo'), Remarks: val('txtRemarks'),
            parentCategoryId: int(val('cmbParentCategory')), input: table.map(strip), output: tableOutput.map(strip) };
        return busy(btn, function () {
            return http('POST', api + '/save', body).then(function (d) {
                box(d.message);
                if ($id('chkPrint').checked) GeneratePrint(d.id);
                return RefreshForm();
            }).catch(function (e) { box(e.message); });
        });
    }
    /* RefreshForm :490 + summeryreset :538 */
    function RefreshForm() {
        RecId = 0; updateDetailIndex = -1; updateDetailIndexOutput = -1;
        setVal('txtProductionNo', ''); setVal('txtRemarks', ''); setVal('txtQty', '');
        table = []; tableOutput = [];
        if (int(L.defaultJobLot) > 0 && has('cmbLot', L.defaultJobLot)) setVal('cmbLot', L.defaultJobLot);
        GenerateSummaryForUser(); ['txtInputAmount', 'txtInputQuantity', 'txtInputRate', 'txtOutputByProductAmount', 'txtOutputByProductQty', 'txtOutputByProductRate'].forEach(function (k) { setVal(k, ''); });
        applyRights(); renderAll(); refreshCombos();
        return getJson(api + '/doc-no').then(function (d) { setVal('txtdocnumber', d.docNo); $id('txtDocNoShow').textContent = 'SC-' + d.docNo; })
            .catch(function (e) { box(e.message); });
    }
    /* ReadById :1325 */
    function ReadById(id) {
        return getJson(api + '/' + id).then(function (d) {
            RecId = int(d.Id); showTab(0);
            setVal('txtdocnumber', d.DocSrNo); $id('txtDocNoShow').textContent = 'SC-' + d.DocSrNo;
            setVal('txtProductionNo', d.ProductionNo); setVal('txtDocdate', dateOnly(d.DocDate)); setVal('txtRemarks', d.Remarks);
            if (has('cmbParentCategory', d.parentCategoryId)) setVal('cmbParentCategory', d.parentCategoryId);
            table = d.input || []; tableOutput = d.output || [];
            applyRights();
            handleAverageRateCalculation();                                                      // btnGenerate_Click(null, null)
            GenerateSummaryForUser(); renderAll(); refreshCombos();
        }).catch(function (e) { box(e.message); });
    }
    function GeneratePrint(id) { window.open('/api/reports/ssc-665/data?id=' + encodeURIComponent(id), '_blank'); }

    /* AvgRateUpdateOnDocDateChange :2454 (txtDocdate_Leave when FIFO or CGSEntryAllow) */
    function docDateLeave() {
        if (!(L.fifo || L.cgsEntryAllow) || table.length <= 0) return Promise.resolve();
        var rows = table.map(function (r) { return { ItemId: int(r.ItemId), ItemConditionId: int(r.ItemConditionId) }; });
        return http('POST', api + '/rates', { recId: RecId, docDate: val('txtDocdate'), rows: rows }).then(function (rates) {
            table.forEach(function (r, i) { var rate = num(rates[i]); if (rate > 0) { r.Rate = rate; r.Amount = num(r.Quantity) * rate; } else { r.Rate = 0; r.Amount = 0; } });
            handleAverageRateCalculation(); renderAll();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ loader

    function loaderOpen() {
        $id('loaderModal').classList.add('open');
        setVal('ldTodate', today());
        return getJson(api + '/loader/lookups').then(function (d) {
            ld.rows = d.rows || [];
            setVal('ldFromDate', d.fromDate ? dateOnly(d.fromDate) : today());
            var parents = ld.rows.filter(function (r) { return r.ActivityType === 'ParentCategories' && (int(r.Id) === 7 || int(r.Id) === 8); });
            var keep = int(val('ldParent'));
            fill('ldParent', parents, 'Id', 'name');
            setVal('ldParent', keep > 0 && has('ldParent', keep) ? keep : (has('ldParent', 7) ? 7 : ''));
            loaderOtherCombos();
            return loaderSearch();
        }).catch(function (e) { box(e.message); });
    }
    /* OtherComboByParentCategoryFill :151 */
    function loaderOtherCombos() {
        var pid = val('ldParent');
        var rows = pid ? ld.rows.filter(function (r) { return String(r.InventoryParentCategoriesId) === String(pid); }) : [];
        function pick(t) { var seen = {}, out = []; rows.forEach(function (r) { if (r.ActivityType === t && !seen[r.Id]) { seen[r.Id] = 1; out.push(r); } }); return out; }
        fill('ldItemType', pick('ItemTypes'), 'Id', 'name'); fill('ldItemCategory', pick('ItemCategories'), 'Id', 'name');
        fill('ldItem', pick('Items'), 'Id', 'name'); fill('ldWarehouse', pick('Warehouse'), 'Id', 'name');
        var keep = int(val('ldCondition'));
        fill('ldCondition', pick('ItemCondition'), 'Id', 'name');
        var c = keep > 0 ? keep : 7; if (has('ldCondition', c)) setVal('ldCondition', c);
    }
    /* PendingInventoryTransactions :265 */
    function loaderSearch() {
        var q = '?fromDate=' + encodeURIComponent(val('ldFromDate')) + '&toDate=' + encodeURIComponent(val('ldTodate'))
            + '&parentCategoryId=' + int(val('ldParent')) + '&itemCategoryId=' + int(val('ldItemCategory')) + '&itemTypeId=' + int(val('ldItemType'))
            + '&warehouseId=' + int(val('ldWarehouse')) + '&itemId=' + int(val('ldItem')) + '&itemConditionId=' + int(val('ldCondition'));
        return getJson(api + '/loader/search' + q).then(function (rows) {
            ld.data = rows || [];
            var cols = [['CategoryDescription', 'ItemCategory'], ['TypeDescription', 'ItemType'], ['ItemName', 'ItemName'], ['WareHouseName', 'WareHouse'],
                ['rackName', 'RackName'], ['ItemCondition', 'ItemCondition'], ['UOMCode', 'PackUom'], ['QtyIn', 'QtyIn', 1], ['QtyOut', 'QtyOut', 1],
                ['AvailableStock', 'AvailableStock', 1], ['AvgRate', 'AvgRate', 1], ['InAmount', 'InAmount', 1], ['OutAmount', 'OutAmount', 1],
                ['BalanceAmount', 'BalanceAmount', 1], ['SecondaryUom', 'SecondaryUom'], ['AvgSecondaryUomQty', 'AvgSecondaryUomQty', 1], ['AvgSecondaryUomItemRate', 'AvgSecondaryUomItemRate', 1]];
            $id('ldHead').innerHTML = ld.data.length ? '<th>&#10003;</th><th>rowNo</th>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') : '';
            $id('ldGrid').innerHTML = ld.data.map(function (r, i) {
                return '<tr class="data-row" data-i="' + i + '"><td><input type="checkbox" data-act="ldchk"/></td><td>' + esc(col(r, 'rowNo')) + '</td>'
                    + cols.map(function (c) { var v = col(r, c[0]); return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(c[2] ? qtyFmt(v) : v) + '</td>'; }).join('') + '</tr>';
            }).join('');
            SelectedWeightCalculation();
        }).catch(function (e) { box(e.message); });
    }
    function checkedLoaderRows() {
        var out = [];
        Array.prototype.forEach.call(document.querySelectorAll('#ldGrid input[data-act="ldchk"]'), function (c) { if (c.checked) out.push(ld.data[int(c.closest('tr').getAttribute('data-i'))]); });
        return out;
    }
    /* SelectedWeightCalculation :533 */
    function SelectedWeightCalculation() {
        var q = 0, w = 0;
        checkedLoaderRows().forEach(function (r) { q += num(col(r, 'AvailableStock')); w += num(col(r, 'AvgSecondaryUomQty')); });
        var f = function (n) { var s = Math.round(n).toLocaleString('en-US'); return s.length < 2 ? ('0' + s) : s; };   // "0,0"
        setVal('txtSelectedQty', f(q)); setVal('txtSelectedStock', f(w));
    }
    /* btnLoadOnInvoice_Click_1 :442 → LoadDataDetailfromPurchaseInvoivce :2189 */
    function loaderLoad() {
        var rows = checkedLoaderRows();
        if (!rows.length) { box('Please Select Row first'); return; }
        $id('loaderModal').classList.remove('open');
        var keys = {};
        table.forEach(function (r) { keys[[int(r.ItemId), int(r.WareHouseId), int(r.RackId), int(r.ItemConditionId)].join('|')] = 1; });
        rows.forEach(function (dr) {
            var key = [int(col(dr, 'ItemId')), int(col(dr, 'WarehouseId')), int(col(dr, 'RackId')), int(col(dr, 'ItemConditionId'))].join('|');
            if (keys[key]) return; keys[key] = 1;
            var empty = table.length === 0;
            var lot = empty ? int(val('cmbLot')) : int(L.defaultJobLot);
            var lotName = empty ? text('cmbLot') : ((byId(L.jobLots, lot) || {}).Name || '');
            table.push({ Id: 0, RefDocumentTypeId: 0, RefDocNoId: 0, RefDocSubId: 0, EntryType: '1', ItemId: int(col(dr, 'ItemId')), ItemName: col(dr, 'ItemName'),
                WareHouseId: int(col(dr, 'WarehouseId')), WareHouse: col(dr, 'WareHouseName'), RackId: int(col(dr, 'RackId')), RackName: col(dr, 'rackName'),
                ItemConditionId: int(col(dr, 'ItemConditionId')), ItemCondition: col(dr, 'ItemCondition'), ItemUOMId: int(col(dr, 'PackUomId')), UOM: col(dr, 'UOMCode'),
                JobLotId: lot, JobLot: lotName, Quantity: num(col(dr, 'AvailableStock')), Rate: num(col(dr, 'AvgRate')), Amount: num(col(dr, 'BalanceAmount')),
                Remarks: '', BalQty: num(col(dr, 'AvailableStock')), LineId: maxLine() + 1 });
        });
        GenerateSummaryForUser(); renderAll();
    }

    // ------------------------------------------------------------------ history

    var HIST_COLS = [['DocNo', 'DocNo'], ['DocDate', 'DocDate', 'd'], ['ProductionNo', 'ProductionNo'], ['Remarks', 'Remarks'], ['EntryDate', 'EntryDate', 't'],
        ['EntryUser', 'EntryUser'], ['ModifyDate', 'ModifyDate', 't'], ['ModifyUser', 'ModifyUser'], ['ApprovedDate', 'ApprovedDate', 't'], ['ApprovedUser', 'ApprovedUser']];
    function historyFill() {
        var dtp = (document.querySelector('input[name="rdDate"]:checked') || {}).value || 'doc';
        var q = '?dateType=' + dtp + '&fromDate=' + ($id('chkFromDate').checked ? encodeURIComponent(val('FromDateHistory')) : '')
            + '&toDate=' + ($id('chkToDate').checked ? encodeURIComponent(val('ToDateHistory')) : '')
            + '&fromDocNo=' + int(val('txtFromDocNoHistory')) + '&toDocNo=' + int(val('txtToDocNoHistory'));
        return getJson(api + '/history' + q).then(function (rows) {
            historyRows = rows || [];
            $id('histDetailHead').innerHTML = ''; $id('grdDetail').innerHTML = '';
            if (!historyRows.length) { $id('histHead').innerHTML = ''; $id('DatagridHistory').innerHTML = ''; $id('lblHistCount').textContent = ''; return; }
            $id('histHead').innerHTML = '<th>Print</th><th>Edit</th>' + HIST_COLS.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('');
            $id('DatagridHistory').innerHTML = historyRows.map(function (r, i) {
                return '<tr class="data-row" data-i="' + i + '"><td><button type="button" class="win-btn-mini" data-act="print">Print</button></td>'
                    + '<td><button type="button" class="win-btn-mini" data-act="edit">Edit</button></td>'
                    + HIST_COLS.map(function (c) { var v = r[c[0]]; return '<td>' + esc(c[2] === 'd' ? ddmmm(v) : c[2] === 't' ? dt(v) : v) + '</td>'; }).join('') + '</tr>';
            }).join('');
            $id('lblHistCount').textContent = historyRows.length + ' record(s)';
        }).catch(function (e) { box(e.message); });
    }
    /* DatagridHistory_SelectionChanged :2363 → BindDetailsByHeaderId :2380 */
    function historySelect(i) {
        var r = historyRows[i]; if (!r) return;
        Array.prototype.forEach.call($id('DatagridHistory').rows, function (tr) { tr.classList.toggle('sel', int(tr.getAttribute('data-i')) === i); });
        getJson(api + '/' + int(r.Id)).then(function (d) {
            var rows = (d.input || []).concat(d.output || []);
            var cols = [['EntryType', 'EntryType'], ['WareHouse', 'WareHouse'], ['ItemName', 'ItemName'], ['UOM', 'PackUom'], ['ItemCondition', 'ItemCondition'],
                ['JobLot', 'JobLot'], ['Quantity', 'Qty', 1], ['Rate', 'Rate', 1], ['Amount', 'Amount', 1], ['Remarks', 'Remarks']];
            $id('histDetailHead').innerHTML = cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('');
            $id('grdDetail').innerHTML = rows.map(function (x) {
                return '<tr>' + cols.map(function (c) {
                    var v = c[0] === 'EntryType' ? (int(x.EntryType) === 1 ? 'InPut' : 'OutPut') : x[c[0]];
                    return '<td' + (c[2] ? ' class="num"' : '') + '>' + esc(c[2] ? qtyFmt(v) : v) + '</td>'; }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ wiring

    function showTab(i) {
        currentTab = i;
        $id('tabPage1').style.display = i === 0 ? '' : 'none'; $id('tabPage2').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0); $id('tabHistory').classList.toggle('active', i === 1);
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }

    function bindEvents() {
        on('cmbEntryType', 'change', entryTypeLeave);
        on('cmbParentCategory', 'change', parentLeave);
        on('cmbItem', 'change', function () { setVal('cmbGodown', ''); setVal('CmbRackName', ''); itemLeave(); });
        on('cmbGodown', 'change', function () { setVal('CmbRackName', ''); rackBind(int(val('cmbItem')), int(val('cmbGodown'))); AvailableStock(); refreshCombos(); });
        on('CmbRackName', 'change', rackLeave);
        on('cmbItemCondition', 'change', function () { AvailableStock(); GetAvgRate(); });
        on('cmbLot', 'change', function () { AvailableStock(); GetAvgRate(); });
        on('txtQty', 'input', AmountCalculation);
        on('txtRate', 'input', AmountCalculation);
        on('txtDocdate', 'change', docDateLeave);
        function gridClick(isInput) {
            return function (e) {
                var tr = e.target.closest('tr'); if (!tr) return;
                var i = int(tr.getAttribute('data-i')), act = e.target.getAttribute('data-act');
                if (act === 'del') deleteRow(isInput, i);
                else if (act === 'chk' && isInput) table[i]._checked = e.target.checked;
            };
        }
        on('grdInput', 'click', gridClick(true));
        on('grdOutput', 'click', gridClick(false));
        on('grdInput', 'change', function (e) { if (e.target.getAttribute('data-act') === 'qty') inputQtyChanged(int(e.target.closest('tr').getAttribute('data-i')), e.target.value); });
        on('grdOutput', 'change', function (e) {
            var act = e.target.getAttribute('data-act'), i = int(e.target.closest('tr').getAttribute('data-i'));
            if (act === 'qty') outputQtyChanged(i, e.target.value); else if (act === 'wh' || act === 'rack' || act === 'cond') outputPick(i, act, e.target.value);
        });
        on('grdInput', 'dblclick', function (e) { if (e.target.closest('input,select,button')) return; var tr = e.target.closest('tr'); if (tr) editRow(true, int(tr.getAttribute('data-i'))); });
        on('grdOutput', 'dblclick', function (e) { if (e.target.closest('input,select,button')) return; var tr = e.target.closest('tr'); if (tr) editRow(false, int(tr.getAttribute('data-i'))); });
        on('DatagridHistory', 'click', function (e) {
            var tr = e.target.closest('tr'); if (!tr) return;
            var i = int(tr.getAttribute('data-i')), b = e.target.closest('button');
            if (!b) { historySelect(i); return; }
            if (b.getAttribute('data-act') === 'edit') ReadById(int(historyRows[i].Id)); else GeneratePrint(int(historyRows[i].Id));
        });
        on('DatagridHistory', 'dblclick', function (e) { var tr = e.target.closest('tr'); if (tr) ReadById(int(historyRows[int(tr.getAttribute('data-i'))].Id)); });
        on('ldParent', 'change', loaderOtherCombos);
        on('ldGrid', 'change', SelectedWeightCalculation);
        document.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var k = e.key.toLowerCase();
            if (k === 't') { e.preventDefault(); showTab(currentTab === 1 ? 0 : 1); }
            else if (k === 's' && currentTab === 0 && RecId === 0) { e.preventDefault(); window.Ssc.btnsave_Click(); }
            else if (k === 'u' && RecId > 0) { e.preventDefault(); window.Ssc.btnUpdate_Click(); }
            else if (k === 'n') { e.preventDefault(); window.Ssc.btnnew_Click(); }
        });
    }

    window.Ssc = {
        btnnew_Click: function () { return busy('btnnew', function () { var p = RefreshForm(); resetDetail(); return p; }); },
        btnRefresh_Click: function () {
            return busy('btnRefresh', function () {
                return getJson(api + '/refresh').then(function (d) {
                    for (var k in d) L[k] = d[k];
                    fill('cmbItemCondition', L.conditions, 'Id', 'Description'); fill('cmbLot', L.jobLots, 'Id', 'Name');
                    if (entryType() === 1) BindInputItem(); else if (entryType() === 2) ItemNameBind();
                    refreshCombos();
                }).catch(function (e) { box(e.message); });
            });
        },
        btnsave_Click: function () { if (RecId !== 0 || $id('btnsave').disabled) return; return Insert('btnsave'); },
        btnUpdate_Click: function () { if ($id('btnUpdate').disabled) return; return Insert('btnUpdate'); },
        Print_Click: function () { if (RecId > 0) GeneratePrint(RecId); else box('No Record Selected'); },
        btnGenerate_Click: function () { handleAverageRateCalculation(); renderAll(); },
        btnIssuanceLoad_Click: loaderOpen,
        AddInGrid_Click: AddInGrid_Click, btnUpdateDetail_Click: btnUpdateDetail_Click, btnCancelDetail_Click: btnCancelDetail_Click,
        BtnForwardRowsFromInputToOutPut_Click: forwardRows, btnUpdateItemCondition_Click: updateItemCondition,
        loaderSearch: function () { return busy('btngrnlod', loaderSearch); }, loaderLoad: loaderLoad,
        loaderReset: function () { setVal('ldCondition', ''); setVal('ldItem', ''); $id('ldGrid').innerHTML = ''; $id('ldHead').innerHTML = ''; ld.data = []; },
        loaderClose: function () { $id('loaderModal').classList.remove('open'); },
        showTab: showTab, btnshowHistory_Click: function () { return busy('btnshowHistory', historyFill); },
        btnHNew_Click: function () {
            setVal('txtToDocNoHistory', ''); setVal('txtFromDocNoHistory', ''); setVal('FromDateHistory', today()); setVal('ToDateHistory', today());
            historyRows = []; ['histHead', 'DatagridHistory', 'histDetailHead', 'grdDetail'].forEach(function (k) { $id(k).innerHTML = ''; });
        }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
