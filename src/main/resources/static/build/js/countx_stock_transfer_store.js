/* ============================================================================================
 * Stock Transfer Store - screen 505, module 54, DocumentTypeId 807.
 * Desktop: Architecture.WinApp.StoreManagement.frmStockTransferStore.
 *
 *   Load :303   BranchesFill :457   CmbFromBranch_Leave :578   CmbRefDocumentType_Leave :684
 *   FormValidation :909   FormValidationDetail :942   OtherExpensesProportion :1086   OtherChargesAmountUpdate :1123
 *   grdOtherCharges_CellUpdated :1154   btnUpdateDetail :1247   grd_DoubleClick :1302   btnplus :1402
 *   DeleteDetailRow :1461   Insert :1509   ReadById :1743   Reset :1840   ResetDetail :1896   gridhistoryfill :1994
 *   AvailableStockGetByItem :2367   GetAvgRate :2393   cmbItemName_Leave :2582   BindWarehouses :2601   BindRacks :2687
 *   AvgRateUpdateOnDocDateChange :3098
 * The server repeats every figure and validation; nothing here is trusted by it.
 * ============================================================================================ */
(function () {
    'use strict';

    var api = '/api/packing-material/stock-transfer-store';
    var L = null, R = {};
    var table = [], dtExp = [], RecId = 0, IsApproved = false, updateDetailIndex = -1;
    var fromRacks = [], toRacks = [], dtRefDoc = [], historyRows = [], currentTab = 0;

    function $id(id) { return document.getElementById(id); }
    function val(id) { var e = $id(id); return e ? e.value : ''; }
    function setVal(id, v) { var e = $id(id); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblFormStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
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
    function away(x, n) { var f = Math.pow(10, n || 0); var s = x < 0 ? -1 : 1; return s * Math.round(Math.abs(x) * f + 1e-9) / f; }
    function dp() { return int(L && L.amountDecimals); }
    function fmt(n, d) { return num(n).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d === undefined ? 3 : d }); }
    function amt(n) { return num(n).toLocaleString('en-US', { minimumFractionDigits: dp(), maximumFractionDigits: dp() }); }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    var MON = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Aug','Sep','Oct','Nov','Dec'];
    function ddmmm(v) { var d = dateOnly(v); if (!d) return ''; var p = d.split('-'); return p[2] + '-' + MON[+p[1] - 1] + '-' + p[0]; }

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
    function fill(id, rows, v, t) {
        var sel = $id(id); if (!sel) return; var keep = sel.value;
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(col(r, v)) + '">' + esc(col(r, t)) + '</option>'; });
        sel.innerHTML = html;
        if (keep && sel.querySelector('option[value="' + String(keep).replace(/"/g, '') + '"]')) sel.value = keep;
    }
    function has(id, v) { var s = $id(id); return !!(s && v !== '' && s.querySelector('option[value="' + String(v).replace(/"/g, '') + '"]')); }
    function text(id) { var s = $id(id); return s && s.selectedIndex > 0 && s.options[s.selectedIndex] ? s.options[s.selectedIndex].text : ''; }
    function show(id, on) { var e = $id(id); if (e) e.style.display = on ? '' : 'none'; }
    function focus(id) { var e = $id(id); if (e) try { e.focus(); } catch (x) { /* ignore */ } }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function byId(list, id) { id = int(id); return (list || []).filter(function (x) { return int(col(x, 'Id')) === id; })[0] || null; }

    // ------------------------------------------------------------------ load

    function init() {
        bindEvents();
        setVal('DocDate', today());
        loadLookups().then(function () { AddRowInExpenseGrid(); renderAll(); });
    }

    function loadLookups() {
        say('Loading...');
        return getJson(api + '/lookups').then(function (d) {
            L = d; R = d.rights || {};
            Array.prototype.forEach.call(document.querySelectorAll('.fin'), function (e) { e.style.display = d.financialEffect ? '' : 'none'; });
            Array.prototype.forEach.call(document.querySelectorAll('.branch'), function (e) { e.style.display = d.multiBranch ? '' : 'none'; });
            $id('txtAvgRate').readOnly = !d.rateEditable;
            fill('cmbItemName', d.items, 'Id', 'ItemName');
            fill('CmbItemCondition', d.conditions, 'Id', 'Description');
            fill('CmbRefDocumentType', d.refDocumentTypes, 'Id', 'type');
            fill('CmbFromBranch', d.branches, 'Id', 'Description'); fill('CmbToBranch', d.branches, 'Id', 'Description');
            if (!d.multiBranch) { fromRacks = d.racks || []; toRacks = fromRacks; }
            else if (!int(val('CmbFromBranch')) && has('CmbFromBranch', d.userBranchId)) { setVal('CmbFromBranch', d.userBranchId); branchLeave(true); }
            if (RecId === 0) { setVal('txtdocno', d.docNo); $id('txtDocNoShow').textContent = 'ST-' + d.docNo; }
            var f = new Date(); f.setDate(f.getDate() - int(d.historyDays || 3)); setVal('FromDateHistory', iso(f)); setVal('ToDateHistory', today());
            refreshCombos(); applyRights(); say('');
        }).catch(function (e) { say(''); box(e.message); });
    }

    function applyRights() {
        var upd = RecId > 0;
        show('btnsave', !upd); show('btnupdate', upd); show('btnDelete', upd && R['delete']);
        $id('btnsave').disabled = !R.save; $id('btnupdate').disabled = !R.update; $id('btnDelete').disabled = !R['delete'];
        $id('btnprint').disabled = !R.update; $id('ChkBoxPrintPreview').disabled = !R.update;          // :314-318 (Update right)
    }

    /* CmbFromBranch_Leave / CmbToBranch_Leave */
    function branchLeave(isFrom) {
        var f = int(val('CmbFromBranch')), t = int(val('CmbToBranch'));
        if (f > 0 && t > 0 && f === t) { setVal(isFrom ? 'CmbFromBranch' : 'CmbToBranch', ''); box("From Branch And To Branch Can't be same"); }
        var b = int(val(isFrom ? 'CmbFromBranch' : 'CmbToBranch'));
        return getJson(api + '/branch-racks?branchId=' + b).then(function (rows) { if (isFrom) fromRacks = rows || []; else toRacks = rows || []; })
            .catch(function (e) { box(e.message); });
    }
    function ToggleBranchControls() { $id('CmbFromBranch').disabled = table.length > 0; $id('CmbToBranch').disabled = table.length > 0; }

    // ------------------------------------------------------------------ entry bar

    function item() { return byId(L.items, val('cmbItemName')); }

    /* BindWarehouses :2601 */
    function BindWarehouses(itemId) {
        if (L.multiBranch) {
            if (!int(val('CmbFromBranch'))) { box('Please select From Branch first.'); focus('CmbFromBranch'); return false; }
            if (!int(val('CmbToBranch'))) { box('Please select To Branch first.'); focus('CmbToBranch'); return false; }
        }
        bindWarehouseDropdown('CmbWareHouseFrom', fromRacks, itemId);
        bindWarehouseDropdown('cmbWareHouseTo', toRacks, itemId);
        return true;
    }
    function bindWarehouseDropdown(id, source, itemId) {
        var seen = {}, list = [];
        source.forEach(function (r) { if (int(r.ItemId) === itemId && !seen[r.WarehouseId]) { seen[r.WarehouseId] = 1; list.push({ Id: r.WarehouseId, Warehouse: r.WareHouseName }); } });
        fill(id, list, 'Id', 'Warehouse');
        if (int(val(id))) return;
        if (list.length === 1) setVal(id, list[0].Id);
        else if (list.length > 1) {
            var it = item(), pc = it ? int(it.ParentCategoryId) : 0;
            var cfg = pc === 7 ? int(L.packingMaterialDefaultWarehouse) : pc === 8 ? int(L.defaultWarehouseForStoreFlow) : 0;
            if (cfg && list.some(function (w) { return int(w.Id) === cfg; })) setVal(id, cfg);
        }
    }
    /* BindRacks :2687 */
    function BindRacks(itemId, whId, isFrom) {
        var seen = {}, list = [];
        (isFrom ? fromRacks : toRacks).forEach(function (r) { if (int(r.ItemId) === itemId && (!whId || int(r.WarehouseId) === whId) && !seen[r.Id]) { seen[r.Id] = 1; list.push(r); } });
        var id = isFrom ? 'CmbRackFrom' : 'CmbRackTo';
        fill(id, list, 'Id', 'RackName');
        if (int(val(id))) return;
        if (list.length === 1) setVal(id, list[0].Id);
        else if (list.length > 1) { var b = list.filter(function (r) { return int(r.BaseRackId) > 0; })[0]; if (b) setVal(id, b.Id); }
    }
    function bindPackUom() {
        var keep = text('CmbPackUom');
        var list = (L.uoms || []).filter(function (u) { return int(u.ItemId) === int(val('cmbItemName')); });
        fill('CmbPackUom', list, 'Id', 'UOMCode');
        var m = list.filter(function (u) { return String(u.UOMCode) === keep; })[0];
        setVal('CmbPackUom', m && keep ? m.Id : '');
        if (int(val('CmbPackUom'))) return;
        if (list.length === 1) setVal('CmbPackUom', list[0].Id);
        else if (list.length > 1) { var e = list.filter(function (u) { return num(u.Equivalent) === 1; })[0]; if (e) setVal('CmbPackUom', e.Id); }
    }
    /* cmbItemName_Leave :2582 */
    function itemLeave() {
        var itemId = int(val('cmbItemName'));
        if (BindWarehouses(itemId)) {
            BindRacks(itemId, int(val('CmbWareHouseFrom')), true);
            BindRacks(itemId, int(val('cmbWareHouseTo')), false);
            bindPackUom();
        }
        return stock(true);
    }
    /* AvailableStockGetByItem :2367 (+ GetAvgRate :2393 when rate is true) */
    function stock(rate) {
        var itemId = int(val('cmbItemName'));
        if (!itemId) { setVal('txtAvailableStock', '0'); return Promise.resolve(); }
        var q = '?recId=' + RecId + '&itemId=' + itemId + '&conditionId=' + int(val('CmbItemCondition')) + '&warehouseId=' + int(val('CmbWareHouseFrom'))
            + '&rackId=' + int(val('CmbRackFrom')) + '&docDate=' + encodeURIComponent(val('DocDate'));
        return getJson(api + '/stock' + q).then(function (d) {
            setVal('txtAvailableStock', num(d.availableStock) > 0 ? fmt(d.availableStock) : '0');
            if (rate) { setVal('txtAvgRate', num(d.avgRate) > 0 ? String(d.avgRate) : '0'); AvgAmountCalculation(); }
        }).catch(function (e) { box(e.message); });
    }
    /* AvgAmountCalculation :2414 */
    function AvgAmountCalculation() {
        var q = num(val('txtQty')), r = num(val('txtAvgRate'));
        setVal('txtAmount', q > 0 && r > 0 ? amt(away(q * r, dp())) : '0');
    }

    /* CmbRefDocumentType_Leave :684 */
    function refTypeLeave() {
        var t = int(val('CmbRefDocumentType')); dtRefDoc = [];
        if (t <= 0) { fill('CmbRefDocNo', []); fill('CmbRefDocInvoiceNo', []); return Promise.resolve(); }
        return getJson(api + '/ref-docs?refDocumentTypeId=' + t + '&recId=' + RecId).then(function (rows) {
            dtRefDoc = rows || [];
            var inv = [], doc = [], si = {}, sd = {};
            dtRefDoc.forEach(function (r) {
                var i = int(col(r, 'RefDocInvoiceId')), d = int(col(r, 'RefDocId'));
                if (i > 0 && !si[i]) { si[i] = 1; inv.push({ Id: i, No: col(r, 'RefDocInvoiceNo') }); }
                if (d > 0 && !sd[d]) { sd[d] = 1; doc.push({ Id: d, No: col(r, 'RefDocNo') }); }
            });
            fill('CmbRefDocInvoiceNo', inv, 'Id', 'No'); fill('CmbRefDocNo', doc, 'Id', 'No');
        }).catch(function (e) { box(e.message); });
    }

    /* FormValidationDetail :942 */
    function FormValidationDetail() {
        var c = [['cmbItemName', 'Item Name field is required'], ['CmbWareHouseFrom', 'Warehouse From field is required'], ['CmbRackFrom', 'Rack From field is required'],
            ['cmbWareHouseTo', 'Warehouse To field is required'], ['CmbRackTo', 'Rack To field is required']];
        for (var i = 0; i < c.length; i++) if (!int(val(c[i][0]))) { box(c[i][1]); focus(c[i][0]); return false; }
        if (int(val('CmbRackFrom')) === int(val('CmbRackTo'))) { box('Rack From and Rack To cannot be the same. Please select a different rack.'); focus('CmbRackTo'); return false; }
        if (!int(val('CmbItemCondition'))) { box('Item Condition field is required'); focus('CmbItemCondition'); return false; }
        if (!int(val('CmbPackUom'))) { box('PackUOM field is required'); focus('CmbPackUom'); return false; }
        if (L.financialEffect) {
            if (!num(val('txtAvgRate'))) { box('AvgRate field is required'); focus('txtAvgRate'); return false; }
            if (!num(val('txtAmount'))) { box('Amount field is required'); focus('txtAmount'); return false; }
        }
        if (!num(val('txtQty'))) { box('Qty field is required'); focus('txtQty'); return false; }
        return true;
    }
    function editorRow(r) {
        var t = int(val('CmbRefDocumentType'));
        r.ItemId = int(val('cmbItemName')); r.ItemName = text('cmbItemName');
        r.WareHouseFromId = int(val('CmbWareHouseFrom')); r.WareHouseFrom = text('CmbWareHouseFrom');
        r.RackFromId = int(val('CmbRackFrom')); r.RackFrom = text('CmbRackFrom');
        r.WareHouseToId = int(val('cmbWareHouseTo')); r.WareHouseTo = text('cmbWareHouseTo');
        r.RackToId = int(val('CmbRackTo')); r.RackTo = text('CmbRackTo');
        r.ItemConditionId = int(val('CmbItemCondition')); r.ItemCondition = text('CmbItemCondition');
        r.PackUOMId = int(val('CmbPackUom')); r.PackUOM = text('CmbPackUom');
        r.QTY = num(val('txtQty')); r.ItemRate = num(val('txtAvgRate')); r.ItemAmount = num(val('txtAmount'));
        r.RefDocumentTypeId = t; r.RefDocumentType = t ? text('CmbRefDocumentType') : '';
        r.RefDocId = t ? int(val('CmbRefDocNo')) : 0; r.RefDocNo = t ? text('CmbRefDocNo') : '';
        r.RefDocInvoiceId = t ? int(val('CmbRefDocInvoiceNo')) : 0; r.RefDocInvoiceNo = t ? text('CmbRefDocInvoiceNo') : '';
        r.Remarks = val('txtRemarksDetail').trim();
        return r;
    }
    function btnplus_Click() {
        if (!FormValidationDetail()) return;
        table.push(editorRow({ Id: 0, Expense: 0 }));
        OtherExpensesProportion(); ResetDetail(); if (L.multiBranch) ToggleBranchControls(); renderAll();
    }
    function btnUpdateDetail_Click() {
        if (updateDetailIndex < 0 || !FormValidationDetail()) return;
        editorRow(table[updateDetailIndex]);
        ResetDetail(); OtherExpensesProportion(); OtherChargesAmountUpdate(); renderAll();
    }
    /* grd_DoubleClick :1302 */
    function grd_DoubleClick(i) {
        var r = table[i]; if (!r) return;
        updateDetailIndex = i;
        setVal('cmbItemName', r.ItemId); refreshCombos();
        setVal('CmbWareHouseFrom', ''); setVal('cmbWareHouseTo', ''); setVal('CmbRackFrom', ''); setVal('CmbRackTo', '');
        if (BindWarehouses(int(r.ItemId))) {
            setVal('CmbWareHouseFrom', r.WareHouseFromId); setVal('cmbWareHouseTo', r.WareHouseToId);
            BindRacks(int(r.ItemId), int(r.WareHouseFromId), true); BindRacks(int(r.ItemId), int(r.WareHouseToId), false);
            bindPackUom();
        }
        setVal('CmbRackFrom', r.RackFromId); setVal('CmbRackTo', r.RackToId);
        setVal('CmbItemCondition', r.ItemConditionId); setVal('CmbPackUom', r.PackUOMId);
        setVal('txtQty', fmt(r.QTY)); setVal('txtAvgRate', r.ItemRate); setVal('txtAmount', amt(r.ItemAmount));
        setVal('CmbRefDocumentType', r.RefDocumentTypeId || '');
        refTypeLeave().then(function () { setVal('CmbRefDocNo', r.RefDocId || ''); setVal('CmbRefDocInvoiceNo', r.RefDocInvoiceId || ''); });
        setVal('txtRemarksDetail', r.Remarks);
        stock(false);
        show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        renderAll(); focus('CmbWareHouseFrom');
    }
    /* ResetDetail :1896 - condition stays */
    function ResetDetail() {
        updateDetailIndex = -1;
        setVal('cmbItemName', ''); ['CmbWareHouseFrom', 'cmbWareHouseTo', 'CmbRackFrom', 'CmbRackTo'].forEach(function (k) { fill(k, []); });
        setVal('CmbPackUom', ''); ['txtQty', 'txtRemarksDetail', 'txtAvgRate', 'txtAmount'].forEach(function (k) { setVal(k, ''); });
        setVal('CmbRefDocumentType', ''); fill('CmbRefDocNo', []); fill('CmbRefDocInvoiceNo', []);
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        refreshCombos(); renderAll(); focus('cmbItemName');
    }
    /* DeleteDetailRow :1461 */
    function DeleteDetailRow(i) {
        if (updateDetailIndex !== -1) { box('Please Reset the detail first...'); return; }
        table.splice(i, 1); OtherExpensesProportion(); if (L.multiBranch) ToggleBranchControls(); renderAll();
    }

    // ------------------------------------------------------------------ other charges

    function AddRowInExpenseGrid() { dtExp.push({ Account: 0, Percentage: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }); }
    function totals() { var t = { qty: 0, amount: 0, exp: 0 }; table.forEach(function (r) { t.qty += num(r.QTY); t.amount += num(r.ItemAmount); t.exp += num(r.Expense); }); return t; }
    /* OtherExpensesProportion :1086 */
    function OtherExpensesProportion() {
        if (!table.length) return;
        var tq = totals().qty, c = 0; dtExp.forEach(function (e) { c += num(e.Amount); });
        table.forEach(function (r) { r.Expense = c > 0 ? c / tq * num(r.QTY) : 0; });
    }
    /* OtherChargesAmountUpdate :1123 */
    function OtherChargesAmountUpdate() {
        var tot = totals().amount;
        dtExp.forEach(function (e) {
            if (num(e.Amount) > 0) { var v = tot / 100 * num(e.Percentage); if (v > 0) e.Amount = v; e.Qty = 0; e.Rate = 0; }
        });
        OtherExpensesProportion();
    }
    function accountOptions(cur) {
        var html = '<option value="0"></option>';
        (L && L.accounts || []).forEach(function (a) { html += '<option value="' + int(a.Id) + '"' + (int(a.Id) === int(cur) ? ' selected' : '') + '>' + esc(a.AccountTitle) + '</option>'; });
        return html;
    }

    // ------------------------------------------------------------------ render

    var G = [['ItemName', 'Item Name'], ['WareHouseFrom', 'WareHouse From'], ['RackFrom', 'Rack From'], ['WareHouseTo', 'WareHouse To'], ['RackTo', 'Rack To'],
        ['ItemCondition', 'Item Condition'], ['PackUOM', 'PackUOM'], ['QTY', 'QTY', 'q'], ['ItemRate', 'ItemRate', 'q', 1], ['ItemAmount', 'ItemAmount', 'a', 1],
        ['Expense', 'Expense', 'a', 1], ['RefDocumentType', 'RefDocumentType'], ['RefDocNo', 'RefDocNo'], ['RefDocInvoiceNo', 'RefDocInvoiceNo'], ['Remarks', 'Remarks']];
    function cols() { return G.filter(function (c) { return !c[3] || L.financialEffect; }); }
    function renderAll() {
        if (!L) return;
        var C = cols(), h = '<th>X</th><th>Edit</th>'; C.forEach(function (c) { h += '<th>' + c[1] + '</th>'; }); $id('grdHead').innerHTML = h;
        var html = '';
        table.forEach(function (r, i) {
            html += '<tr class="data-row' + (i === updateDetailIndex ? ' sel' : '') + '" data-i="' + i + '"><td><button type="button" class="win-btn-mini" data-act="del">X</button></td>'
                + '<td><button type="button" class="win-btn-mini" data-act="edit">Edit</button></td>';
            C.forEach(function (c) { var v = r[c[0]]; html += '<td' + (c[2] ? ' class="num"' : '') + '>' + (c[2] === 'a' ? amt(v) : c[2] === 'q' ? fmt(v) : esc(v)) + '</td>'; });
            html += '</tr>';
        });
        $id('grd').innerHTML = html;
        var t = totals(), f = '<td></td><td></td>';
        C.forEach(function (c) { f += '<td class="num">' + (c[0] === 'QTY' ? fmt(t.qty) : c[0] === 'ItemAmount' ? amt(t.amount) : c[0] === 'Expense' ? amt(t.exp) : '') + '</td>'; });
        $id('grdFoot').innerHTML = table.length ? f : '';

        $id('expHead').innerHTML = '<th>X</th><th>+</th><th>Account</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th>';
        var eh = '', ea = 0;
        dtExp.forEach(function (e, i) {
            ea += num(e.Amount);
            eh += '<tr data-e="' + i + '"><td><button type="button" class="win-btn-mini" data-act="del">X</button></td><td><button type="button" class="win-btn-mini" data-act="add">+</button></td>'
                + '<td><select class="cell-sel" data-f="Account">' + accountOptions(e.Account) + '</select></td>'
                + ['Percentage', 'Qty', 'Rate', 'Amount'].map(function (k) { return '<td><input class="cell-sm" data-f="' + k + '" value="' + esc(e[k]) + '"/></td>'; }).join('')
                + '<td><input class="cell-text" data-f="Remarks" value="' + esc(e.Remarks) + '"/></td></tr>';
        });
        $id('grdOtherCharges').innerHTML = eh;
        $id('expFoot').innerHTML = '<td></td><td></td><td></td><td></td><td></td><td></td><td class="num">' + amt(ea) + '</td><td></td>';
    }

    // ------------------------------------------------------------------ save

    function Insert(btnId) {
        if (!table.length) { box('Grid Record Not Found'); return; }
        if (!int(val('txtdocno'))) { box('DocNo Field is Required'); return; }
        if (L.multiBranch) {
            if (!int(val('CmbFromBranch'))) { box('From Branch Field is Required'); return; }
            if (!int(val('CmbToBranch'))) { box('To Branch Field is Required'); return; }
            if (int(val('CmbFromBranch')) === int(val('CmbToBranch'))) { box("From Branch And To Branch can't be same"); return; }
        }
        if (!window.confirm(RecId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        for (var i = 0; i < dtExp.length; i++) {
            if (!(num(dtExp[i].Amount) > 0)) continue;
            if (!int(dtExp[i].Account)) { box('Please Select an Account Against OtherCharges Amount First'); return; }
            if (!String(dtExp[i].Remarks || '')) { box('Grid Charge to Product Remarks required Please Check'); return; }
        }
        var req = {
            Id: RecId, DocDate: val('DocDate'), RemarksHeader: val('txtRemarksHead'),
            FromBranchId: int(val('CmbFromBranch')), ToBranchId: int(val('CmbToBranch')),
            rows: table.map(function (r) {
                return { Id: int(r.Id), ItemId: r.ItemId, WareHouseFromId: r.WareHouseFromId, RackFromId: r.RackFromId, WareHouseToId: r.WareHouseToId,
                    RackToId: r.RackToId, ItemConditionId: r.ItemConditionId, PackUOMId: r.PackUOMId, QTY: num(r.QTY), ItemRate: num(r.ItemRate),
                    RefDocumentTypeId: int(r.RefDocumentTypeId), RefDocId: int(r.RefDocId), RefDocInvoiceId: int(r.RefDocInvoiceId), Remarks: r.Remarks || '' };
            }),
            expenses: dtExp.map(function (e) { return { Account: int(e.Account), Percentage: num(e.Percentage), Qty: num(e.Qty), Rate: num(e.Rate), Amount: num(e.Amount), Remarks: e.Remarks || '' }; })
        };
        return busy(btnId, function () {
            say('Saving...');
            return http('POST', api + '/save', req).then(function (d) {
                say(''); box(d.message);
                var id = d.id;
                return Reset().then(function () { if ($id('ChkBoxPrintPreview').checked) GeneratePrint(id); });
            }).catch(function (e) { say(''); box(e.message); });
        });
    }
    function GeneratePrint(id) { if (!int(id)) { box('No Record Found For Display'); return; } window.open('/api/reports/st-415/data?id=' + encodeURIComponent(id), '_blank'); }

    /* Reset :1840 - item condition and from branch stay */
    function Reset() {
        RecId = 0; IsApproved = false;
        setVal('txtRemarksHead', ''); setVal('CmbToBranch', ''); if (L && L.multiBranch) toRacks = [];
        table = []; dtExp = []; AddRowInExpenseGrid();
        ResetDetail();
        if (L && L.multiBranch) ToggleBranchControls();
        renderAll(); applyRights();
        return getJson(api + '/doc-no').then(function (d) { setVal('txtdocno', d.docNo); $id('txtDocNoShow').textContent = 'ST-' + d.docNo; })
            .catch(function (e) { box(e.message); });
    }

    /* ReadById :1743 */
    function ReadById(id) {
        say('Loading...');
        return getJson(api + '/' + id).then(function (d) {
            say('');
            RecId = int(d.Id); IsApproved = !!d.IsApproved;
            showTab(0);
            setVal('DocDate', dateOnly(d.DocDate)); setVal('txtdocno', d.DocNo); $id('txtDocNoShow').textContent = 'ST-' + d.DocNo;
            setVal('txtRemarksHead', d.RemarksHeader);
            var p = [];
            if (int(d.FromBranchId) > 0) { setVal('CmbFromBranch', d.FromBranchId); p.push(branchLeave(true)); }
            if (int(d.ToBranchId) > 0) { setVal('CmbToBranch', d.ToBranchId); p.push(branchLeave(false)); }
            table = (d.rows || []).map(function (r) { var o = {}; for (var k in r) o[k] = r[k]; return o; });
            dtExp = (d.expenses || []).map(function (e) { var o = {}; for (var k in e) o[k] = e[k]; return o; });
            if (!dtExp.length) AddRowInExpenseGrid();
            if (L.multiBranch) ToggleBranchControls();
            return Promise.all(p).then(function () { refreshCombos(); renderAll(); applyRights(); });
        }).catch(function (e) { say(''); Reset(); box(e.message); });
    }

    function btnDelete_Click() {
        if (RecId <= 0) { box('RecordId Not Found.....'); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        return busy('btnDelete', function () {
            return http('POST', api + '/' + RecId + '/delete').then(function (d) { box(d.message); return Reset(); }).catch(function (e) { box(e.message); });
        });
    }

    /* DocDate_Leave :3140 */
    function docDateLeave() {
        if (!(L.financialEffect || L.cgsEntryAllow) || !table.length) return;
        return Promise.all(table.map(function (r) {
            return getJson(api + '/stock?recId=' + RecId + '&itemId=' + int(r.ItemId) + '&conditionId=' + int(r.ItemConditionId) + '&docDate=' + encodeURIComponent(val('DocDate')))
                .then(function (d) { var rate = num(d.avgRate); r.ItemRate = rate > 0 ? rate : 0; r.ItemAmount = rate > 0 ? num(r.QTY) * rate : 0; });
        })).then(renderAll).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ history

    function gridhistoryfill() {
        var dt = (document.querySelector('input[name="rdDate"]:checked') || {}).value || 'doc';
        var q = '?dateType=' + dt + ($id('chkFromDate').checked ? '&fromDate=' + encodeURIComponent(val('FromDateHistory')) : '')
            + ($id('chkToDate').checked ? '&toDate=' + encodeURIComponent(val('ToDateHistory')) : '')
            + '&fromDocNo=' + int(val('txtFromDocNoHistory')) + '&toDocNo=' + int(val('txtToDocNoHistory'));
        return getJson(api + '/history' + q).then(function (rows) {
            historyRows = rows || [];
            var H = [['DocDate', 'DocDate', 'd'], ['DocNo', 'DocNo'], ['TransType', 'TransType']];
            if (L.multiBranch) H.push(['FromBranchName', 'FromBranchName'], ['ToBranchName', 'ToBranchName']);
            H.push(['Remarks', 'Remarks'], ['EntryDate', 'EntryDate', 'd'], ['EntryUser', 'EntryUser'], ['ModifyDate', 'ModifyDate', 'd'], ['ModifyUser', 'ModifyUser'], ['NoOfAttachments', 'NoOfAttachments']);
            var head = '<th>Edit</th><th>Print</th>'; H.forEach(function (c) { head += '<th>' + c[1] + '</th>'; });
            $id('histHead').innerHTML = historyRows.length ? head : '';
            var html = '';
            historyRows.forEach(function (r, i) {
                html += '<tr class="data-row" data-h="' + i + '"><td><button type="button" class="win-btn-mini" data-h-act="edit">Edit</button></td><td><button type="button" class="win-btn-mini" data-h-act="print">Print</button></td>';
                H.forEach(function (c) { html += '<td>' + (c[2] === 'd' ? ddmmm(r[c[0]]) : esc(r[c[0]])) + '</td>'; });
                html += '</tr>';
            });
            $id('grdhistory').innerHTML = html; $id('GridDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
            $id('lblHistCount').textContent = historyRows.length ? historyRows.length + ' record(s)' : '';
        }).catch(function (e) { box(e.message); });
    }
    function detail(id) {
        return getJson(api + '/' + id).then(function (d) {
            var C = [['ItemName', 'ItemName'], ['WareHouseFrom', 'WarehouseFrom'], ['RackFrom', 'RackFrom'], ['WareHouseTo', 'WarehouseTo'], ['RackTo', 'RackTo'],
                ['ItemCondition', 'ItemCondition'], ['PackUOM', 'PackUOM'], ['QTY', 'QTY', 'q']];
            if (L.financialEffect) C.push(['ItemRate', 'ItemRate', 'q'], ['ItemAmount', 'ItemAmount', 'q']);
            C.push(['Expense', 'Expense', 'q'], ['RefDocumentType', 'RefDocumentType'], ['RefDocNo', 'RfDocNo'], ['RefDocInvoiceNo', 'RefDocInvoiceNo'], ['Remarks', 'Remarks']);
            var head = ''; C.forEach(function (c) { head += '<th>' + c[1] + '</th>'; }); $id('histDetailHead').innerHTML = head;
            var html = ''; (d.rows || []).forEach(function (l) { html += '<tr>'; C.forEach(function (c) { html += '<td' + (c[2] ? ' class="num"' : '') + '>' + (c[2] ? fmt(l[c[0]]) : esc(l[c[0]])) + '</td>'; }); html += '</tr>'; });
            $id('GridDetail').innerHTML = html;
        }).catch(function () { $id('GridDetail').innerHTML = ''; });
    }

    // ------------------------------------------------------------------ events

    function showTab(i) {
        currentTab = i;
        $id('tabPage1').style.display = i === 0 ? '' : 'none'; $id('tabPage2').style.display = i === 1 ? '' : 'none';
        $id('tabForm').classList.toggle('active', i === 0); $id('tabHistory').classList.toggle('active', i === 1);
    }
    function on(id, ev, fn) { var e = $id(id); if (e) e.addEventListener(ev, fn); }
    function later(fn) { return function (e) { var t = e.target; setTimeout(function () { fn(t, e); }, 0); }; }

    function bindEvents() {
        on('CmbFromBranch', 'change', function () { branchLeave(true); });
        on('CmbToBranch', 'change', function () { branchLeave(false); });
        on('cmbItemName', 'change', function () { ['CmbWareHouseFrom', 'cmbWareHouseTo', 'CmbRackFrom', 'CmbRackTo'].forEach(function (k) { setVal(k, ''); }); itemLeave(); });
        on('CmbWareHouseFrom', 'change', function () { BindRacks(int(val('cmbItemName')), int(val('CmbWareHouseFrom')), true); stock(false); });
        on('cmbWareHouseTo', 'change', function () { BindRacks(int(val('cmbItemName')), int(val('cmbWareHouseTo')), false); });
        on('CmbRackFrom', 'change', function () {
            var r = byId(fromRacks, val('CmbRackFrom'));
            if (r && int(val('CmbWareHouseFrom')) !== int(r.WarehouseId) && has('CmbWareHouseFrom', r.WarehouseId)) setVal('CmbWareHouseFrom', r.WarehouseId);
            stock(false);
        });
        on('CmbRackTo', 'change', function () {
            var r = byId(toRacks, val('CmbRackTo'));
            if (r && int(val('cmbWareHouseTo')) !== int(r.WarehouseId) && has('cmbWareHouseTo', r.WarehouseId)) setVal('cmbWareHouseTo', r.WarehouseId);
        });
        on('CmbItemCondition', 'change', function () { stock(true); });
        on('txtQty', 'input', AvgAmountCalculation);
        on('txtAvgRate', 'input', AvgAmountCalculation);
        on('CmbRefDocumentType', 'change', refTypeLeave);
        on('CmbRefDocNo', 'change', function () {
            var d = int(val('CmbRefDocNo')); if (d <= 0) return;
            var r = dtRefDoc.filter(function (x) { return int(col(x, 'RefDocId')) === d; })[0], i = r ? int(col(r, 'RefDocInvoiceId')) : 0;
            setVal('CmbRefDocInvoiceNo', has('CmbRefDocInvoiceNo', i) ? i : '');
        });
        on('CmbRefDocInvoiceNo', 'change', function () {
            var i = int(val('CmbRefDocInvoiceNo')); if (i <= 0) return;
            var r = dtRefDoc.filter(function (x) { return int(col(x, 'RefDocInvoiceId')) === i; })[0], d = r ? int(col(r, 'RefDocId')) : 0;
            setVal('CmbRefDocNo', has('CmbRefDocNo', d) ? d : '');
        });
        on('DocDate', 'change', docDateLeave);
        on('grd', 'click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var i = int(b.closest('tr').getAttribute('data-i'));
            if (b.getAttribute('data-act') === 'del') DeleteDetailRow(i); else grd_DoubleClick(i);
        });
        on('grd', 'dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) grd_DoubleClick(int(tr.getAttribute('data-i'))); });
        on('grdOtherCharges', 'change', later(function (t) {
            var f = t.getAttribute('data-f'), tr = t.closest('tr[data-e]'); if (!f || !tr) return;
            var e = dtExp[int(tr.getAttribute('data-e'))]; if (!e) return;
            if (f === 'Remarks') { e.Remarks = t.value; return; }
            if (f === 'Account') e.Account = int(t.value);
            else {
                e[f] = num(t.value);
                if ((f === 'Qty' || f === 'Rate') && t.value !== '') { e.Amount = num(e.Qty) * num(e.Rate); e.Percentage = 0; }
                if (f === 'Percentage') { var v = totals().amount / 100 * num(e.Percentage); if (v > 0) e.Amount = v; e.Qty = 0; e.Rate = 0; }
            }
            OtherExpensesProportion(); renderAll();
        }));
        on('grdOtherCharges', 'click', function (e) {
            var b = e.target.closest('button[data-act]'); if (!b) return;
            var i = int(b.closest('tr').getAttribute('data-e'));
            if (b.getAttribute('data-act') === 'del') { dtExp.splice(i, 1); if (!dtExp.length) AddRowInExpenseGrid(); OtherExpensesProportion(); } else AddRowInExpenseGrid();
            renderAll();
        });
        on('grdhistory', 'click', function (e) {
            var tr = e.target.closest('tr[data-h]'); if (!tr) return;
            var r = historyRows[int(tr.getAttribute('data-h'))]; if (!r) return;
            Array.prototype.forEach.call(document.querySelectorAll('#grdhistory tr'), function (x) { x.classList.toggle('sel', x === tr); });
            var b = e.target.closest('button[data-h-act]');
            if (!b) { detail(int(r.Id)); return; }
            if (b.getAttribute('data-h-act') === 'edit') Reset().then(function () { ReadById(int(r.Id)); }); else GeneratePrint(int(r.Id));
        });
        on('grdhistory', 'dblclick', function (e) {
            var tr = e.target.closest('tr[data-h]'); if (!tr || e.target.closest('button')) return;
            var r = historyRows[int(tr.getAttribute('data-h'))]; if (r) ReadById(int(r.Id));
        });
        document.addEventListener('keydown', function (e) {
            if (!e.ctrlKey) return;
            var k = e.key.toLowerCase();
            if (k === 's') { e.preventDefault(); if (currentTab === 0) { if (RecId === 0 && R.save) Insert('btnsave'); } else gridhistoryfill(); }
            else if (k === 'n') { e.preventDefault(); Reset(); }
            else if (k === 't') { e.preventDefault(); showTab(currentTab === 1 ? 0 : 1); }
        });
    }

    window.Sts = {
        btnnew_Click: function () { return busy('btnnew', Reset); },
        btnRefresh_Click: function () { return busy('btnRefresh', function () { return loadLookups().then(renderAll); }); },
        btnsave_Click: function () { if (RecId !== 0 || $id('btnsave').disabled) return; return Insert('btnsave'); },
        btnupdate_Click: function () { if (RecId === 0 || $id('btnupdate').disabled) return; if (IsApproved) { box("Approved Record Can't Update"); return; } return Insert('btnupdate'); },
        btnDelete_Click: btnDelete_Click,
        btnprint_Click: function () { if (RecId > 0) GeneratePrint(RecId); else box('No Record Selected'); },
        btnplus_Click: btnplus_Click, btnUpdateDetail_Click: btnUpdateDetail_Click, ResetDetail: ResetDetail,
        showTab: showTab, btnshow_Click: function () { return busy('btnshow', gridhistoryfill); },
        btnNewHistory_Click: function () {
            setVal('FromDateHistory', today()); setVal('ToDateHistory', today()); setVal('txtFromDocNoHistory', ''); setVal('txtToDocNoHistory', '');
            historyRows = []; $id('grdhistory').innerHTML = ''; $id('histHead').innerHTML = ''; $id('GridDetail').innerHTML = ''; $id('histDetailHead').innerHTML = '';
            var d = document.querySelector('input[name="rdDate"][value="doc"]'); if (d) d.checked = true;
        }
    };

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
