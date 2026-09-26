/* ============================================================================================
 * Screen 334 "Purchase Invoice Direct Store" — Architecture.WinApp.StoreManagement.
 * frmPurchaseInvoiceDirectStore, DocumentTypeId 61 (global PiDirectStore).
 *
 *   Load:333               lists, rights (print boxes checked = print right), blank Freight / GL rows,
 *                          all active warehouses with the first one selected, history branch + supplier
 *   comItem_Leave:1030     PackUOM, BalanceStock, warehouses / racks of the item
 *   btnAdd_Click:1995      one grid row; FreightProportion + BillAmount; ResetDetail
 *   grdFreight / grdGLedger CellUpdated:1769 / :1897 — GL id found by title; Qty*Rate / % of items
 *   BillAmount:1621        Math.Round(items + tax + JL debit − JL credit + supplier-own freight)
 *   Insert:1201            validation order and messages; the server recalculates and saves
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, ci = C.ci, num = C.num, intOf = C.intOf;
    var api = '/api/store/purchase-invoice-direct-store';

    var look = { rights: {}, suppliers: [], items: [], accounts: [], warehouses: [], racks: [], uoms: [], jobLots: [], itemConditions: [], historyBranches: [] };
    var recId = 0, approved = false, voucherHeadId = 0, updateDetailIndex = -1, loaded = false;
    var table = [], freight = [], gl = [];
    var whList = [];                 // comWarehouse's current DataSource (Id, WareHouseName, BranchId, BranchName)
    var supplierGlText = '';         // txtSupplierGLId (hidden)

    /* ------------------------------------------------------------------ number helpers */

    /** Math.Round(v, p) — .NET rounds half to even. */
    function roundEven(v, p) {
        var f = Math.pow(10, p || 0), x = num(v) * f, r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r / f;
    }
    /** Convert.ToInt32(string) — only an integer text converts; anything else is 0 (Conversion.ToInt). */
    function cInt(v) { var s = String(v === null || v === undefined ? '' : v).trim(); return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0; }
    function fmt3(v) { return num(v).toLocaleString('en-US', { maximumFractionDigits: 3 }); }
    function selText(id) { var e = $id(id); return e && e.selectedOptions && e.selectedOptions[0] && e.value !== '0' ? e.selectedOptions[0].textContent : ''; }
    function branchFeatureCols() { return !!look.branchFeature && !look.branchImplemented; }

    /* ------------------------------------------------------------------ binds */

    function itemNameBind() {                                                  // ItemNameBind:773
        C.fillSelect('comItem', look.items, 'Id', $id('rdbtnItemName').checked ? 'ItemName' : 'ItemCode');
    }
    function selectedItem() { var id = intOf($id('comItem').value); return look.items.find(function (x) { return x.Id === id; }) || null; }

    function bindSuppliers() { C.fillSelect('comsupplier', look.suppliers, 'Id', 'CompanyName'); }
    function bindJobLots() {                                                    // Branchlot:647
        var el = $id('comjobLot'), keep = intOf(el.value);
        C.fillSelect(el, look.jobLots, 'Id', 'JobLotDescription');
        el.value = look.jobLots.some(function (j) { return j.Id === keep; }) ? String(keep) : '0';
    }
    function bindConditions() { C.fillSelect('CmbItemCondition', look.itemConditions, 'Id', 'Description'); }
    function bindWarehouses(list) {
        whList = list || [];
        C.fillSelect('comWarehouse', whList, 'Id', 'WareHouseName');
        var el = $id('comWarehouse');
        if (!whList.some(function (w) { return String(w.Id) === el.value; })) el.value = '0';
    }

    /** PackUOM:910 — both UOM combos from the item's schedule, retained by TEXT; default single / Equivalent 1. */
    function packUom() {
        var itemId = intOf($id('comItem').value);
        var list = look.uoms.filter(function (u) { return u.ItemId === itemId; });
        ['cmbPackUom', 'comRateUOM'].forEach(function (id) {
            var el = $id(id), keepText = selText(id);
            C.fillSelect(el, list, 'Id', 'UOMCode');
            var hit = list.find(function (u) { return u.UOMCode === keepText; });
            el.value = hit ? String(hit.Id) : '0';
            if (intOf(el.value) === 0) {
                if (list.length === 1) el.value = String(list[0].Id);
                else if (list.length > 1) { var eq = list.find(function (u) { return num(u.Equivalent) === 1; }); if (eq) el.value = String(eq.Id); }
            }
        });
    }

    /** BalanceStock:1146. */
    function balanceStock() {
        $id('lblStockQty').textContent = '0';
        var itemId = intOf($id('comItem').value);
        if (!itemId) return Promise.resolve();
        return C.getJson(api + '/stock' + C.qs({
            itemId: itemId, docDate: $id('DocDate').value, itemConditionId: intOf($id('CmbItemCondition').value),
            warehouseId: intOf($id('comWarehouse').value), rackId: intOf($id('CmbRackName').value)
        })).then(function (r) { $id('lblStockQty').textContent = String(num(r.qtyInHand)); })
            .catch(function (e) { alert(e.message); });
    }

    /** WareHouseBindFromGlobalRacksByItemId:1040. */
    function warehouseBindByItem(itemId) {
        var seen = {}, list = [];
        look.racks.forEach(function (x) {
            if (x.ItemId !== itemId || seen[x.WarehouseId]) return;
            seen[x.WarehouseId] = 1;
            list.push({ Id: x.WarehouseId, WareHouseName: x.WareHouseName, BranchId: x.BranchId, BranchName: x.BranchName });
        });
        bindWarehouses(list);
        if (intOf($id('CmbRackName').value) !== 0) return;
        var el = $id('comWarehouse');
        if (list.length === 1) el.value = String(list[0].Id);
        else if (list.length > 1) {
            var cfg = intOf(look.defaultWarehouseForStoreFlow);
            if (cfg !== 0 && list.some(function (w) { return w.Id === cfg; })) el.value = String(cfg);
        }
    }
    /** RackBindFromGlobalRacksByItemId:1088. */
    function rackBind(itemId, whId) {
        var el = $id('CmbRackName'), keep = intOf(el.value);
        var racks = C.distinct(look.racks.filter(function (x) { return x.ItemId === itemId && (whId === 0 || x.WarehouseId === whId); }), 'Id');
        C.fillSelect(el, racks, 'Id', 'RackName');
        el.value = racks.some(function (r) { return r.Id === keep; }) ? String(keep) : '0';
        if (intOf(el.value) === 0 && racks.length === 1) { el.value = String(racks[0].Id); rackLeave(); }
    }
    /** CmbRackName_Leave:1124. */
    function rackLeave() {
        var rackId = intOf($id('CmbRackName').value), wh = intOf($id('comWarehouse').value);
        var rack = look.racks.find(function (r) { return r.Id === rackId; });
        if (!rack) return;
        if ((wh === 0 || rack.WarehouseId !== wh) && whList.some(function (w) { return w.Id === rack.WarehouseId; })) $id('comWarehouse').value = String(rack.WarehouseId);
        balanceStock();
    }
    /** comItem_ValueChanged:2398 + comItem_Leave:1030. */
    function itemChanged() {
        packUom();
        balanceStock();
        var itemId = intOf($id('comItem').value), whId = intOf($id('comWarehouse').value);   // warehouse read BEFORE the rebind
        warehouseBindByItem(itemId);
        rackBind(itemId, whId);
    }
    /** comWarehouse_Leave:3571. */
    function warehouseLeave() {
        rackBind(intOf($id('comItem').value), intOf($id('comWarehouse').value));
        balanceStock();
    }

    /** AmountCaluculation:2278 — Qty / RateUOM.Equivalent * Rate, Math.Round(2), "#,##0.###". */
    function amountCalc() {
        var rateUom = look.uoms.find(function (u) { return u.Id === intOf($id('comRateUOM').value); });
        var eq = rateUom && intOf($id('comRateUOM').value) > 0 ? num(rateUom.Equivalent) : 0;
        var rate = $id('txtRate').value.trim() !== '' ? num($id('txtRate').value) : 0;
        var qty = $id('txtQty').value.trim() !== '' ? num($id('txtQty').value) : 0;
        $id('txtAmount').value = (qty > 0 && eq > 0 && rate > 0) ? fmt3(roundEven(qty / eq * rate, 2)) : '0';
    }
    /** txtRate_TextChanged:2320 / comRateUOM_Leave:2314. */
    function rateChanged() { amountCalc(); billAmount(); }

    /* ------------------------------------------------------------------ totals */

    function billProportion() {                                                 // :2360
        table.forEach(function (r) { r.BillAmount = num(r.ItemAmount) + num(r.Freights) + num(r.TaxAmount); });
    }
    function billAmount() {                                                     // :1621
        var items = 0, tax = 0, jd = 0, jc = 0, tc = 0;
        table.forEach(function (r) { items += num(r.ItemAmount); tax += num(r.TaxAmount); });
        gl.forEach(function (r) { if (cInt(r.AccountId) > 0) { jd += num(r.Debit); jc += num(r.Credit); } });
        freight.forEach(function (r) {
            var t = String(r.Transporter === null || r.Transporter === undefined ? '' : r.Transporter).trim();
            if (cInt(t) > 0 && supplierGlText === t) tc += num(r.Freight);
        });
        var b = items + tax; b = b + jd - jc; b += tc;
        $id('txtBillAmount').value = String(roundEven(b, 0));
        billProportion();
        renderGrid();
    }
    function freightProportion() {                                              // :2326
        var net = 0, credit = 0;
        table.forEach(function (r) { net += num(r.ItemQty); });
        freight.forEach(function (f) { credit += num(f.Freight); });
        table.forEach(function (r) { r.Freights = credit > 0 ? roundEven(credit, 0) / net * num(r.ItemQty) : 0; });
        billProportion();
        renderGrid();
    }

    /* ------------------------------------------------------------------ detail grid (grd) */

    var GRID_COLS = ['ItemCode', 'Item', 'Warehouse', 'RackName', 'ItemCondition', 'JobLot', 'UOM', 'ItemQty', 'Rate', 'RateUOM',
        'ItemAmount', 'BillAmount', 'Freights', 'RemarksDetail', 'GpNo', 'VehicleNo', 'BranchName'];
    function gridCols() { return GRID_COLS.filter(function (c) { return c !== 'BranchName' || branchFeatureCols(); }); }
    function cellText(r, c) {
        if (c === 'Freights' || c === 'TaxAmount') return fmt3(r[c]);
        return r[c] === null || r[c] === undefined ? '' : r[c];
    }
    function renderGrid() {
        var t = $id('grd'), cols = gridCols();
        t.tHead.innerHTML = '<tr><th>X</th>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr ondblclick="PiDirectStore.edit(' + i + ')"><td><button type="button" class="cx-link" onclick="PiDirectStore.deleteRow(' + i + ')">X</button></td>' +
                cols.map(function (c) { return '<td' + (typeof r[c] === 'number' ? ' class="num"' : '') + '>' + esc(cellText(r, c)) + '</td>'; }).join('') + '</tr>';
        }).join('');
    }

    /** FormValidationDetail:509 — same messages, same order. */
    function validateDetail() {
        var checks = [['comItem', 'Item Name Field is Required'], ['comWarehouse', 'Warehouse Field is Required'],
            ['CmbRackName', 'Rack Name Field is Required'], ['comjobLot', 'Job/Lot Field is Required'],
            ['cmbPackUom', 'UOM Field is Required'], ['CmbItemCondition', 'Item Condition Field is Required']];
        for (var k = 0; k < checks.length; k++) {
            if (intOf($id(checks[k][0]).value) === 0) { alert(checks[k][1]); $id(checks[k][0]).focus(); return false; }
        }
        if ($id('txtQty').value.trim() === '' || num($id('txtQty').value) === 0) { alert('Qty Field is Required'); $id('txtQty').focus(); return false; }
        if ($id('txtRate').value.trim() === '' || num($id('txtRate').value) === 0) { alert('Rate Field is Required'); $id('txtRate').focus(); return false; }
        if (intOf($id('comRateUOM').value) === 0) { alert('Rate UOM Field is Required'); $id('comRateUOM').focus(); return false; }
        if ($id('txtAmount').value.trim() === '' || num($id('txtAmount').value) === 0) { alert('ItemAmount Field is Required'); $id('txtAmount').focus(); return false; }
        return true;
    }
    /** btnAdd_Click:2003-2021 — the warehouse row's branch against the job lot's. */
    function branchPair() {
        var w = whList.find(function (x) { return x.Id === intOf($id('comWarehouse').value); });
        var j = look.jobLots.find(function (x) { return x.Id === intOf($id('comjobLot').value); });
        var p = { BranchId: w ? intOf(w.BranchId) : 0, BranchName: w ? (w.BranchName || '') : '', JobBranchId: j ? intOf(j.BranchId) : 0, JobBranchName: j ? (j.BranchName || '') : '' };
        if (look.branchFeature && !look.branchImplemented && p.BranchId !== p.JobBranchId) {
            alert('Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch \'' + p.BranchName + '\' and JobLot is of Branch \'' + p.JobBranchName + '\'');
            return null;
        }
        return p;
    }
    function entryRow(p) {
        var it = selectedItem() || {};
        var amount = num($id('txtAmount').value);
        return {
            ItemId: intOf($id('comItem').value), ItemCode: it.ItemCode || '', Item: it.ItemName || '',
            WarehouseId: intOf($id('comWarehouse').value), Warehouse: selText('comWarehouse'),
            RackId: intOf($id('CmbRackName').value), RackName: selText('CmbRackName'),
            ItemConditionId: intOf($id('CmbItemCondition').value), ItemCondition: selText('CmbItemCondition').trim(),
            JobLotId: intOf($id('comjobLot').value), JobLot: selText('comjobLot'),
            UOMId: intOf($id('cmbPackUom').value), UOM: selText('cmbPackUom').trim(),
            ItemQty: num($id('txtQty').value), Rate: num($id('txtRate').value),
            RateUOMId: intOf($id('comRateUOM').value), RateUOM: selText('comRateUOM').trim(),
            ItemAmount: amount, TaxNameId: 0, TaxName: '', TaxPercent: 0, TaxAmount: 0,        // DocumentTypeId 61: no tax lookup
            BranchId: p.BranchId, BranchName: p.BranchName
        };
    }
    function add() {                                                            // btnAdd_Click:1995
        if (!validateDetail()) return;
        var p = branchPair(); if (!p) return;
        var r = entryRow(p);
        r.Id = 0;
        r.BillAmount = r.ItemAmount + r.TaxAmount;
        r.Freights = 0;
        r.RemarksDetail = $id('txtRemarksdetail').value.trim();
        r.GpNo = String(cInt($id('txtGpNo').value));
        r.VehicleNo = $id('txtVehicleNo').value.trim();
        table.push(r);
        freightProportion();
        billAmount();
        resetDetail();
    }
    function edit(i) {                                                          // grd_DoubleClick:2109
        var r = table[i]; if (!r) return;
        updateDetailIndex = i;
        $id('comItem').value = String(r.ItemId);
        itemChanged();
        $id('comWarehouse').value = String(r.WarehouseId);
        $id('CmbRackName').value = String(r.RackId);
        $id('CmbItemCondition').value = String(r.ItemConditionId);
        $id('comjobLot').value = String(r.JobLotId);
        $id('cmbPackUom').value = String(r.UOMId);
        $id('txtQty').value = r.ItemQty;
        $id('txtRate').value = r.Rate;
        $id('comRateUOM').value = String(r.RateUOMId);
        $id('txtAmount').value = r.ItemAmount;
        $id('txtRemarksdetail').value = r.RemarksDetail || '';
        $id('txtGpNo').value = r.GpNo || '';
        $id('txtVehicleNo').value = r.VehicleNo || '';
        $id('btnAdd').classList.add('is-hidden'); $id('btnUpdateDetail').classList.remove('is-hidden'); $id('btnCancelUpdateDetial').classList.remove('is-hidden');
        $id('comWarehouse').focus();
    }
    function updateDetail() {                                                   // btnUpdateDetail_Click:2144
        if (!validateDetail()) return;
        var p = branchPair(); if (!p) return;
        var r = table[updateDetailIndex]; if (!r) return;
        var n = entryRow(p);
        Object.keys(n).forEach(function (k) { r[k] = n[k]; });
        r.RemarksDetail = $id('txtRemarksdetail').value;
        r.GpNo = $id('txtGpNo').value;
        r.VehicleNo = $id('txtVehicleNo').value;
        $id('btnAdd').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
        freightProportion();
        billAmount();
        resetDetail();
    }
    function cancelDetail() {                                                   // :2227
        $id('btnAdd').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
        resetDetail();
    }
    /** DeleteDetailrow:2252 — a saved row is checked by usp_StockInReferenceValidationReferredOrNot first. No recalculation. */
    function deleteRow(i) {
        var r = table[i]; if (!r) return;
        var detailId = intOf(r.Id);
        if (detailId > 0) {
            if (!confirm('Are you sure you want to delete this record?')) return;
            C.postJson(api + '/' + recId + '/detail/' + detailId + '/check-delete', {})
                .then(function () { table.splice(table.indexOf(r), 1); renderGrid(); })
                .catch(function (e) { alert(e.message); });
            return;
        }
        table.splice(i, 1);
        renderGrid();
    }
    function resetDetail() {                                                    // ResetDetail:1554
        $id('comItem').value = '0';
        bindWarehouses([]);
        C.fillSelect('CmbRackName', [], 'Id', 'RackName');
        $id('txtQty').value = ''; $id('txtRate').value = '';
        $id('comRateUOM').value = '0';
        $id('txtAmount').value = '';
        $id('comItem').focus();
    }

    /* ------------------------------------------------------------------ freight grid (grdFreight) */

    function accountOptions(value) {
        var v = String(value === null || value === undefined ? '' : value);
        return '<option value="0"></option>' + look.accounts.map(function (a) {
            return '<option value="' + esc(a.SupplierCustomerId) + '"' + (String(a.SupplierCustomerId) === v ? ' selected' : '') + '>' + esc(a.AccountTitle) + '</option>';
        }).join('');
    }
    function accountTitle(value) {
        var a = look.accounts.find(function (x) { return String(x.SupplierCustomerId) === String(value); });
        return a ? a.AccountTitle : '';
    }
    /** SupCustIdUpdateforFrieghtGrid:1789 / SupCustIdUpdateforGLGrid:1962 — GL id = first account with the same TITLE. */
    function glByTitle(rows, key) {
        rows.forEach(function (r) {
            var title = accountTitle(r[key]);
            if (title === '' || title === '0') return;
            var m = look.accounts.find(function (a) { return a.AccountTitle === title; });
            if (m) r.GlAccountId = intOf(m.Id);
        });
    }
    function inp(grid, i, k, v, cls) {
        return '<td><input type="text"' + (cls ? ' class="' + cls + '"' : '') + ' data-g="' + grid + '" data-i="' + i + '" data-k="' + k +
            '" value="' + esc(v === null || v === undefined ? '' : v) + '" onchange="PiDirectStore.cell(this)"></td>';
    }
    function renderFreight() {
        var t = $id('grdFreight');
        t.tHead.innerHTML = '<tr><th>X</th><th>+</th><th>Transporter</th><th>Credit</th><th>Remarks</th></tr>';
        t.tBodies[0].innerHTML = freight.map(function (r, i) {
            return '<tr><td><button type="button" class="cx-link" onclick="PiDirectStore.freightDel(' + i + ')">X</button></td>' +
                '<td><button type="button" class="cx-link" onclick="PiDirectStore.freightAdd(' + i + ')">+</button></td>' +
                '<td><select class="win-combo" data-g="f" data-i="' + i + '" data-k="Transporter" onchange="PiDirectStore.cell(this)">' + accountOptions(r.Transporter) + '</select></td>' +
                inp('f', i, 'Freight', r.Freight, 'num') + inp('f', i, 'Remarks', r.Remarks) + '</tr>';
        }).join('');
        var tot = freight.reduce(function (s, r) { return s + num(r.Freight); }, 0);
        t.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td class="num">' + esc(fmt3(tot)) + '</td><td></td></tr>';
    }
    function addFreightRow() { freight.push({ Transporter: '0', Freight: 0, Remarks: '0', GlAccountId: 0 }); }   // AddRowInFreightGrid:1688
    function freightDel(i) {                                                    // grdFreight_ColumnButtonClick "Delete"
        freight.splice(i, 1);
        if (!freight.length) addFreightRow();
        renderFreight();
        billAmount();
    }
    function freightAdd(i) {                                                    // "Add" — copies Transporter and Remarks only
        var r = freight[i]; if (!r) return;
        freight.push({ Transporter: r.Transporter, Freight: 0, Remarks: r.Remarks, GlAccountId: 0 });
        renderFreight();
        billAmount();
    }

    /* ------------------------------------------------------------------ GL grid (grdGLedger) */

    function renderGl() {
        var t = $id('grdGLedger');
        t.tHead.innerHTML = '<tr><th>X</th><th>+</th><th>Account</th><th>Remarks</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Debit</th><th>Credit</th></tr>';
        t.tBodies[0].innerHTML = gl.map(function (r, i) {
            return '<tr><td><button type="button" class="cx-link" onclick="PiDirectStore.glDel(' + i + ')">X</button></td>' +
                '<td><button type="button" class="cx-link" onclick="PiDirectStore.glAdd()">+</button></td>' +
                '<td><select class="win-combo" data-g="g" data-i="' + i + '" data-k="AccountId" onchange="PiDirectStore.cell(this)">' + accountOptions(r.AccountId) + '</select></td>' +
                inp('g', i, 'Remarks', r.Remarks) + inp('g', i, 'Percentage', r.Percentage, 'num') + inp('g', i, 'Qty', r.Qty, 'num') +
                inp('g', i, 'Rate', r.Rate, 'num') + inp('g', i, 'Debit', r.Debit, 'num') + inp('g', i, 'Credit', r.Credit, 'num') + '</tr>';
        }).join('');
        var d = 0, c = 0;
        gl.forEach(function (r) { d += num(r.Debit); c += num(r.Credit); });
        t.tFoot.innerHTML = '<tr><td colspan="7"></td><td class="num">' + esc(fmt3(d)) + '</td><td class="num">' + esc(fmt3(c)) + '</td></tr>';
    }
    function addGlRow() { gl.push({ AccountId: '0', Remarks: '', Percentage: '0', Qty: '0', Rate: '0', Debit: 0, Credit: 0, GlAccountId: 0 }); } // :1822
    function glDel(i) { gl.splice(i, 1); if (!gl.length) addGlRow(); renderGl(); billAmount(); }
    function glAdd() { addGlRow(); renderGl(); billAmount(); }

    /** grdFreight_CellUpdated:1769 / grdGLedger_CellUpdated:1897. */
    function cell(el) {
        var g = el.getAttribute('data-g'), i = intOf(el.getAttribute('data-i')), k = el.getAttribute('data-k');
        if (g === 'f') {
            var fr = freight[i]; if (!fr) return;
            fr[k] = k === 'Freight' ? num(el.value) : el.value;
            if (k === 'Transporter') glByTitle(freight, 'Transporter');
            renderFreight();
            billAmount();
            freightProportion();
            return;
        }
        var r = gl[i]; if (!r) return;
        r[k] = (k === 'Debit' || k === 'Credit') ? num(el.value) : el.value;
        if ((k === 'Qty' || k === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') {
            r.Credit = num(r.Qty) * num(r.Rate); r.Debit = 0; r.Percentage = '0';
        }
        if (k === 'Percentage' && String(r.Percentage) !== '') {
            var items = table.reduce(function (s, x) { return s + num(x.ItemAmount); }, 0);
            var tp = items / 100 * num(r.Percentage);
            if (tp > 0) { r.Credit = roundEven(tp, 0); r.Debit = 0; } else { r.Debit = Math.abs(roundEven(tp, 0)); r.Credit = 0; }
            r.Qty = '0'; r.Rate = '0';
        }
        if (k === 'Credit' && num(r.Debit) > 0) { r.Credit = 0; alert('Debit Side is aleady added'); }
        if (k === 'Debit' && num(r.Credit) > 0) { r.Debit = 0; alert('Credit Side is aleady added'); }
        if (k === 'AccountId') {
            if (cInt(supplierGlText) === cInt(r.AccountId)) { alert('Supplier Account Not select'); r.AccountId = '0'; renderGl(); return; }
            glByTitle(gl, 'AccountId');
        }
        renderGl();
        billAmount();
    }

    /* ------------------------------------------------------------------ header events */

    function supplierChanged() {                                                // comsupplier_ValueChanged:2378
        billAmount();
        var id = intOf($id('comsupplier').value);
        if (look.suppliers.length > 0 && id > 0) {
            var s = look.suppliers.find(function (x) { return x.Id === id; });
            if (s) supplierGlText = String(s.GlAccountId);
        }
    }
    function itemByChanged() { itemNameBind(); }                                // rdbtnItemName_CheckedChanged:1189

    /* ------------------------------------------------------------------ save / delete / print */

    function payload() {
        return {
            Id: recId, DocDate: $id('DocDate').value, SupplierCustomerId: intOf($id('comsupplier').value),
            ManualBillNo: $id('txtbillno').value, RemarksHeader: $id('txtremarks').value,
            rows: table.map(function (r) {
                return { Id: intOf(r.Id), ItemId: r.ItemId, ItemCode: r.ItemCode, Item: r.Item, WarehouseId: r.WarehouseId, Warehouse: r.Warehouse,
                    RackId: r.RackId, RackName: r.RackName, ItemConditionId: r.ItemConditionId, ItemCondition: r.ItemCondition,
                    JobLotId: r.JobLotId, JobLot: r.JobLot, UOMId: r.UOMId, UOM: r.UOM, ItemQty: num(r.ItemQty), Rate: num(r.Rate),
                    RateUOMId: r.RateUOMId, RateUOM: r.RateUOM, ItemAmount: num(r.ItemAmount), TaxNameId: intOf(r.TaxNameId), TaxName: r.TaxName || '',
                    TaxPercent: num(r.TaxPercent), TaxAmount: num(r.TaxAmount), BillAmount: num(r.BillAmount), Freights: num(r.Freights),
                    RemarksDetail: r.RemarksDetail || '', GpNo: String(r.GpNo === null || r.GpNo === undefined ? '' : r.GpNo), VehicleNo: r.VehicleNo || '',
                    BranchId: intOf(r.BranchId), BranchName: r.BranchName || '' };
            }),
            freight: freight.map(function (r) { return { Transporter: String(r.Transporter), Freight: num(r.Freight), Remarks: r.Remarks === null || r.Remarks === undefined ? '' : String(r.Remarks), GlAccountId: intOf(r.GlAccountId) }; }),
            journal: gl.map(function (r) {
                return { AccountId: String(r.AccountId), Remarks: String(r.Remarks === null || r.Remarks === undefined ? '' : r.Remarks), Percentage: String(r.Percentage),
                    Qty: String(r.Qty), Rate: String(r.Rate), Debit: num(r.Debit), Credit: num(r.Credit), GlAccountId: intOf(r.GlAccountId) };
            })
        };
    }
    function insert() {                                                         // Insert:1201
        if (intOf($id('comsupplier').value) === 0) { alert('Supplier Field is Required'); $id('comsupplier').focus(); return; }
        var d = $id('txtdocno').value.trim();
        if (d === '' || d === '0') { alert('DocNo Field is Required'); return; }
        if (!confirm(recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        for (var a = 0; a < freight.length; a++) {
            if (num(freight[a].Freight) > 0 && cInt(freight[a].Transporter) === 0) { alert('Please Select an Account Against Freight First'); return; }
        }
        for (var b = 0; b < gl.length; b++) {
            if ((num(gl[b].Credit) > 0 || roundEven(gl[b].Debit, 0) > 0) && cInt(gl[b].AccountId) === 0) { alert('Please Select an Account Against JL First'); return; }
        }
        billAmount();
        freightProportion();
        if (!table.length) { alert('Grid Record Not Found'); return; }
        var printVoucher = $id('ChkBok').checked, printSlipAfter = $id('ChkPrintSlip').checked;
        C.postJson(api + '/save', payload()).then(function (res) {
            alert(res.message);
            var id = res.id;
            reset().then(function () {
                if (printVoucher) C.printSlip(api + '/' + id + '/voucher', '118-Voucher');
                if (printSlipAfter) C.printSlip(api + '/' + id + '/slip', '233-Purchase Invoice Store Bill Direct');
            });
        }).catch(function (e) { alert(e.message); });
    }
    function save() { recId = 0; insert(); }                                    // saveToolStripButton_Click:1396
    function update() {                                                         // btnUpdate_Click:1409
        if (approved) { alert('Record Not Update because Record has approved'); return; }
        insert();
    }
    function del() {                                                            // btnDelete_Click:3531
        if (approved) { alert('Record Not Update because Record has approved'); return; }
        if (!(recId > 0)) { alert('Record Id Not Found.....'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + recId + '/delete', {}).then(function (res) { alert(res.message); reset(); })
            .catch(function (e) { alert(e.message); });
    }
    function printVoucher() {                                                   // btnPrint_Click:2986 → VoucherReport_118
        if (!voucherHeadId) { alert('VoucherId Not Found'); return; }
        C.printSlip(api + '/' + recId + '/voucher', '118-Voucher');
    }
    function printSlip() {                                                      // btnSlip_Click:2998
        if (!recId) { alert('Record Id Not Found'); return; }
        C.printSlip(api + '/' + recId + '/slip', '233-Purchase Invoice Store Bill Direct');
    }

    /* ------------------------------------------------------------------ open */

    function readById(id) {                                                     // ReadById:1425
        return C.getJson(api + '/' + id).then(function (h) {
            recId = h.Id;
            $id('txtdocno').value = h.DocNo; $id('txtBranchSrNo').value = h.BranchSrNo; $id('txtTaxInvoiceNo').value = h.SalesTaxNo;
            $id('DocDate').value = C.isoDay(h.DocDate);
            tab('tabForm');
            $id('comsupplier').value = String(h.SupplierCustomerId);
            if (!$id('comsupplier').value) $id('comsupplier').value = '0';
            supplierChanged();                                                  // ValueChanged fires on the OLD grid
            $id('CmbTaxAccount').value = String(h.ReferencePartyId || 0);
            $id('txtbillno').value = h.ManualBillNo || '';
            $id('txtremarks').value = h.RemarksHeader || '';
            $id('txtBillAmount').value = String(h.BillAmount);
            approved = !!h.IsApproved;
            voucherHeadId = intOf(h.voucherHeadId);
            table = h.rows || [];
            gl = (h.journal || []).slice(); if (!gl.length) addGlRow();
            freight = (h.freight || []).slice(); if (!freight.length) addFreightRow();
            renderGl(); renderFreight();
            billProportion();
            renderGrid();
            $id('btnSave').classList.add('is-hidden'); $id('btnUpdate').classList.remove('is-hidden'); $id('btnDelete').classList.remove('is-hidden');
            $id('lblRecId').textContent = 'Record #' + recId + (voucherHeadId ? '  Voucher #' + voucherHeadId : '');
        }).catch(function (e) { alert(e.message); });
    }
    function openFromHistory(id) {                                              // grdHistory_DoubleClick:2768 / "Edit"
        if (!look.rights.update) { alert("You don't Have Update Rights..."); return; }
        readById(id);
    }

    /* ------------------------------------------------------------------ history */

    function renderBranches(list) {                                             // HistoryBranchComboFill:2408
        look.historyBranches = list || [];
        var ub = intOf(look.userBranchId);
        $id('cmbBranchName').innerHTML = look.historyBranches.map(function (b) {
            return '<label><input type="checkbox" value="' + esc(b.Id) + '"' + (b.Id === ub ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>';
        }).join('');
    }
    function branchIds() {
        var ids = '';
        document.querySelectorAll('#cmbBranchName input[type=checkbox]').forEach(function (c) { if (c.checked) ids += ',' + c.value; });
        return ids;
    }
    function historyCombo(validate) {                                           // HistoryComboFill:2452
        return C.getJson(api + '/history-suppliers' + C.qs({ branchIds: branchIds(), validate: validate ? 'true' : 'false' }))
            .then(function (rows) { C.fillSelect('cmbSupplierNameHistory', rows, 'Id', 'Supplier'); })
            .catch(function (e) { alert(e.message); });
    }
    var H_COLS = [['DocDate', 'DocDate'], ['DocNo', 'DocNo'], ['BranchSrNo', 'BranchSrNo'], ['BranchName', 'BranchName'], ['ManualBillNo', 'ManualBillNo'],
        ['SupplierName', 'SupplierName'], ['BillAmount', 'BillAmount'], ['ApprovedStatus', 'ApprovedStatus'], ['EntryUser', 'EntryUser'],
        ['EntryDate', 'EntryDate'], ['ModifyUser', 'ModifyUser'], ['ModifyDate', 'ModifyDate'], ['NoOfAttachments', 'NoOfAttachments'], ['Remarks', 'Remarks']];
    function histCell(h, k) {
        if (k === 'DocDate') return C.gridDate(h.DocDate);
        if (k === 'EntryDate' || k === 'ModifyDate') return C.gridDateTime(h[k]);
        return h[k];
    }
    function showHistory() {                                                    // btnshow_Click → GetAll:2510
        var ids = branchIds();
        if (!ids) { alert('Select branch first'); return; }
        var dt = (document.querySelector('input[name=histDate]:checked') || {}).value || 'doc';
        C.getJson(api + '/history' + C.qs({
            dateType: dt,
            fromDate: $id('chkFromDate').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDate').checked ? $id('ToDateHistory').value : '',
            fromDocNo: intOf($id('txtFromDocNoHistory').value), toDocNo: intOf($id('txtToDocNoHistory').value),
            supplierCustomerId: intOf($id('cmbSupplierNameHistory').value), branchIds: ids
        })).then(function (list) {
            var rt = look.rights || {}, t = $id('grdHistory');
            var cols = H_COLS.filter(function (c) { return (c[0] !== 'BranchSrNo' && c[0] !== 'BranchName') || branchFeatureCols(); });
            t.tHead.innerHTML = '<tr>' + (rt.print ? '<th>Slip</th><th>Voucher</th>' : '') + (rt.update ? '<th>Edit</th>' : '') +
                cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '<th>Add Attachment</th></tr>';
            t.tBodies[0].innerHTML = (list || []).map(function (h) {
                return '<tr data-id="' + h.Id + '" onclick="PiDirectStore.historyDetail(this)" ondblclick="PiDirectStore.open(' + h.Id + ')">' +
                    (rt.print ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PiDirectStore.historySlip(' + h.Id + ')">Slip</button></td>' +
                        '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PiDirectStore.historyVoucher(' + h.Id + ',' + intOf(h.VoucherHeadId) + ')">Voucher</button></td>' : '') +
                    (rt.update ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PiDirectStore.open(' + h.Id + ')">Edit</button></td>' : '') +
                    cols.map(function (c) {
                        if (c[0] === 'DocNo') return '<td class="num"><button type="button" class="cx-link" onclick="event.stopPropagation();PiDirectStore.open(' + h.Id + ')">' + esc(h.DocNo) + '</button></td>';
                        var v = histCell(h, c[0]);
                        return '<td' + (typeof h[c[0]] === 'number' ? ' class="num"' : '') + '>' + esc(v) + '</td>';
                    }).join('') +
                    '<td><button type="button" class="cx-link" disabled title="Attachments are not ported">Add Attachment</button></td></tr>';
            }).join('');
            if (!list || !list.length) t.tHead.innerHTML = '';                  // ClearStructure
            $id('grdDetail').tHead.innerHTML = ''; $id('grdDetail').tBodies[0].innerHTML = ''; $id('grdDetail').tFoot.innerHTML = '';
        }).catch(function (e) { alert(e.message); });
    }
    var D_COLS = ['ItemCode', 'Item', 'Warehouse', 'RackName', 'ItemCondition', 'JobLot', 'UOM', 'ItemQty', 'Rate', 'RateUOM', 'ItemAmount',
        'BillAmount', 'Freights', 'RemarksDetail', 'GpNo', 'VehicleNo', 'BranchName'];
    var D_SUM = { ItemQty: 1, ItemAmount: 1, BillAmount: 1, Freights: 1 };
    function historyDetail(tr) {                                                // grdHistory_SelectionChanged → GetDetailGrdByHeadId:2798
        document.querySelectorAll('#grdHistory tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        C.getJson(api + '/' + tr.getAttribute('data-id')).then(function (h) {
            var t = $id('grdDetail'), rows = h.rows || [];
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
            var cols = D_COLS.filter(function (c) { return c !== 'BranchName' || branchFeatureCols(); });
            t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (r) {
                return '<tr>' + cols.map(function (c) { return '<td' + (typeof r[c] === 'number' ? ' class="num"' : '') + '>' + esc(D_SUM[c] || c === 'Rate' ? fmt3(r[c]) : r[c]) + '</td>'; }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = '<tr>' + cols.map(function (c) {
                return '<td class="num">' + (D_SUM[c] ? esc(fmt3(rows.reduce(function (s, r) { return s + num(r[c]); }, 0))) : '') + '</td>';
            }).join('') + '</tr>';
        }).catch(function (e) { alert(e.message); });
    }
    function newHistory() {                                                     // btnNewHistory_Click:2963
        $id('FromDateHistory').value = C.today(); $id('ToDateHistory').value = C.today();
        $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = '';
        $id('cmbSupplierNameHistory').value = '0';
        ['grdHistory', 'grdDetail'].forEach(function (id) { var t = $id(id); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; if (t.tFoot) t.tFoot.innerHTML = ''; });
    }
    function refreshHistory() {                                                 // btnRefreshHistory_Click:2950
        C.getJson(api + '/history-branches').then(function (list) { renderBranches(list); return historyCombo(true); })
            .catch(function (e) { alert(e.message); });
    }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    /* ------------------------------------------------------------------ load / reset / refresh */

    function applyLookups(l) {
        look = l;
        itemNameBind();
        bindSuppliers();
        bindJobLots();
        bindConditions();
    }
    function init() {                                                           // InvfrmPurchasedirectInvoice_Load:333
        return C.getJson(api + '/lookups').then(function (l) {
            applyLookups(l);
            var r = l.rights || {};
            $id('btnSave').disabled = !r.save; $id('btnUpdate').disabled = !r.update; $id('btnDelete').disabled = !r.delete;
            $id('btnPrint').disabled = !r.print;
            $id('ChkPrintSlip').disabled = !r.print; $id('ChkPrintSlip').checked = !!r.print;
            $id('ChkBok').disabled = !r.print; $id('ChkBok').checked = !!r.print;
            if (!r.view) { $id('rightsNote').textContent = 'You do not have the View right for Purchase Invoice Direct Store.'; $id('rightsNote').classList.remove('is-hidden'); }
            addGlRow(); renderGl();
            addFreightRow(); renderFreight();
            bindWarehouses(l.warehouses || []);                                 // bindWareHouse:689 — Rows[1] = the first warehouse
            if (whList.length) $id('comWarehouse').value = String(whList[0].Id);
            $id('txtdocno').value = l.docNo || ''; $id('txtBranchSrNo').value = l.branchSrNo || '';
            renderGrid();
            renderBranches(l.historyBranches);
            loaded = true;
            return historyCombo(false);
        }).catch(function (e) { alert(e.message); });
    }
    function reset() {                                                          // Reset:1493 — DocDate, condition, UOM, job lot list kept
        recId = 0; voucherHeadId = 0; approved = false;
        $id('comsupplier').value = '0';
        $id('CmbTaxAccount').value = '0';
        $id('txtbillno').value = ''; $id('txtremarks').value = '';
        $id('comItem').value = '0';
        bindWarehouses([]);
        C.fillSelect('CmbRackName', [], 'Id', 'RackName');
        $id('comjobLot').value = '0';
        $id('txtQty').value = ''; $id('txtRate').value = '';
        $id('comRateUOM').value = '0';
        $id('txtAmount').value = '';
        $id('txtGpNo').value = ''; $id('txtVehicleNo').value = '';
        $id('txtBillAmount').value = '';
        freight = []; gl = []; table = [];
        addGlRow(); renderGl();
        addFreightRow(); renderFreight();
        renderGrid();
        $id('btnSave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden'); $id('btnDelete').classList.add('is-hidden');
        $id('btnAdd').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
        $id('lblRecId').textContent = '';
        return C.getJson(api + '/numbers').then(function (n) {                  // DocumentNo():973, BranchSrNoFill():574
            if (intOf(n.docNo) > 0) $id('txtdocno').value = n.docNo;
            if (intOf(n.branchSrNo) > 0) $id('txtBranchSrNo').value = n.branchSrNo;
        }).catch(function (e) { alert(e.message); });
    }
    function refresh() {                                                        // btnRefresh_Click:1580 — rebinds, keeps selections
        return C.getJson(api + '/lookups').then(function (l) {
            var keepItem = $id('comItem').value, keepSup = $id('comsupplier').value, keepCond = $id('CmbItemCondition').value;
            l.rights = look.rights;
            applyLookups(l);
            $id('comItem').value = keepItem || '0'; $id('comsupplier').value = keepSup || '0'; $id('CmbItemCondition').value = keepCond || '0';
            renderFreight(); renderGl();                                        // GridcomboBind:1660
        }).catch(function (e) { alert(e.message); });
    }

    window.PiDirectStore = {
        reset: reset, refresh: refresh, save: save, update: update, del: del, printVoucher: printVoucher, printSlip: printSlip,
        tab: tab, open: openFromHistory, supplierChanged: supplierChanged, itemByChanged: itemByChanged, itemChanged: itemChanged,
        warehouseLeave: warehouseLeave, rackLeave: rackLeave, balanceStock: balanceStock, amountCalc: amountCalc, rateChanged: rateChanged,
        add: add, edit: edit, updateDetail: updateDetail, cancelDetail: cancelDetail, deleteRow: deleteRow,
        cell: cell, freightDel: freightDel, freightAdd: freightAdd, glDel: glDel, glAdd: glAdd,
        showHistory: showHistory, historyDetail: historyDetail, newHistory: newHistory, refreshHistory: refreshHistory,
        historySlip: function (id) { C.printSlip(api + '/' + id + '/slip', '233-Purchase Invoice Store Bill Direct'); },
        historyVoucher: function (id, vh) {                                     // AcRptPurchaseSalesVoucherSlip_103
            if (!vh) { alert('No Record Found For Display'); return; }
            C.printSlip(api + '/' + id + '/voucher-slip', '103-Purchase Voucher Slip');
        },
        digits: function (el) { el.value = el.value.replace(/[^0-9]/g, ''); }   // OnlytextNumberFunction
    };

    document.addEventListener('DOMContentLoaded', function () {
        $id('DocDate').value = C.today();                                       // DateTimePicker default: now, once
        $id('FromDateHistory').value = C.today(); $id('ToDateHistory').value = C.today();
        init();
    });
})();
