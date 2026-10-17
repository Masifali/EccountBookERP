/* ============================================================================================
 * Screen 904 "Purchase Invoice Direct Store With Variant" - Architecture.WinApp.StoreManagement.
 * frmPurchaseInvoiceDirectStoreWithVariant, DocumentTypeId 61 (global PiVariant).
 *
 *   Load:336               accounts, warehouses, suppliers, company, project, job lots, branches, Doc No
 *   comItem_Leave:866      PackUOM, BindVariant, BalanceStock
 *   btnAdd_Click:1782      one grid row; FreightProportion + BillAmount; ResetDetail (item, qty, rate,
 *                          variant, rate UOM, amount only - warehouse, job lot and UOM stay)
 *   AmountCaluculation:2022 Qty / RateUOM.Equivalent * Rate, Math.Round(2), no thousands separator
 *   FormValidation:464     Branch, Project, Supplier, DocNo
 *   Insert:975             the server recalculates and saves
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, num = C.num, intOf = C.intOf;
    var api = '/api/purchase/purchase-invoice-direct-store-with-variant';

    var look = { rights: {}, companies: [], branches: [], projects: [], suppliers: [], items: [], accounts: [], warehouses: [], jobLots: [] };
    var recId = 0, approved = false, voucherHeadId = 0, updateDetailIndex = -1;
    var table = [], freight = [], gl = [];
    var uomList = [];                // PackUOM(): the item's schedule (Id, UOMCode, Equivalent)
    var supplierGlText = '';         // txtSupplierGLId (hidden)

    /* ------------------------------------------------------------------ number helpers */

    /** Math.Round(v, p) - .NET rounds half to even. */
    function roundEven(v, p) {
        var f = Math.pow(10, p || 0), x = num(v) * f, r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r / f;
    }
    /** Convert.ToInt32(string) - only an integer text converts; anything else is 0 (Conversion.ToInt). */
    function cInt(v) { var s = String(v === null || v === undefined ? '' : v).trim(); return /^[+-]?\d+$/.test(s) ? parseInt(s, 10) : 0; }
    function fmt3(v) { return num(v).toLocaleString('en-US', { maximumFractionDigits: 3 }); }
    function selText(id) { var e = $id(id); return e && e.selectedOptions && e.selectedOptions[0] && e.value !== '0' ? e.selectedOptions[0].textContent : ''; }
    function err(e) { alert(e && e.message ? e.message : String(e)); }

    /* ------------------------------------------------------------------ binds */

    function itemNameBind() {                                                  // item():604 / rdbtnItemName_CheckedChanged
        var keep = intOf($id('comItem').value);
        C.fillSelect('comItem', look.items, 'Id', $id('rdbtnItemName').checked ? 'ItemName' : 'ItemCode');
        $id('comItem').value = look.items.some(function (x) { return x.Id === keep; }) ? String(keep) : '0';
    }
    function selectedItem() { var id = intOf($id('comItem').value); return look.items.find(function (x) { return x.Id === id; }) || null; }
    function bindSuppliers() { C.fillSelect('comsupplier', look.suppliers, 'Id', 'CompanyName'); }
    function bindFirstReal(id, list, textKey) {                                // BindDDLNew + Rows[1].Activate()
        C.fillSelect(id, list, 'Id', textKey);
        if (list && list.length) $id(id).value = String(list[0].Id);
    }
    function bindWarehouses() { C.fillSelect('comWarehouse', look.warehouses, 'Id', 'WareHouseName'); $id('comWarehouse').value = '0'; }  // Rows[0] = blank row
    function bindJobLots() { C.fillSelect('comjobLot', look.jobLots, 'Id', 'JobLotDescription'); }

    /** PackUOM:790 - both UOM combos rebound from the item's schedule (ZeroIndex, nothing retained). */
    function packUom() {
        var itemId = intOf($id('comItem').value);
        return C.getJson(api + '/uoms' + C.qs({ itemId: itemId })).then(function (list) {
            if (!list || !list.length) return;                                  // rows.Count > 0 only
            uomList = list;
            C.fillSelect('comRateUOM', list, 'Id', 'UOMCode');
            C.fillSelect('cmbPackUom', list, 'Id', 'UOMCode');
            $id('comRateUOM').value = '0'; $id('cmbPackUom').value = '0';
        }).catch(err);
    }
    /** BindVariant:873 - keeps the previous text when it still exists, otherwise blank; no rows clears the text. */
    function bindVariant(itemId) {
        var el = $id('CmbAttributeVariant'), keepText = selText('CmbAttributeVariant');
        return C.getJson(api + '/variants' + C.qs({ itemId: itemId })).then(function (list) {
            if (!list || !list.length) { C.fillSelect(el, [], 'Id', 'ItemAttribute'); el.value = '0'; return; }
            C.fillSelect(el, list, 'Id', 'ItemAttribute');
            var hit = keepText !== '' ? list.find(function (v) { return v.ItemAttribute === keepText; }) : null;
            el.value = hit ? String(hit.Id) : '0';
        }).catch(err);
    }
    /** BalanceStock:915 - CurrStockByItem, "#,##0.##". */
    function balanceStock() {
        var lbl = $id('lblStockQty');
        return C.getJson(api + '/stock' + C.qs({ itemId: intOf($id('comItem').value), docDate: $id('DocDate').value,
            warehouseId: intOf($id('comWarehouse').value) })).then(function (r) {
            lbl.textContent = num(r.qtyInHand).toLocaleString('en-US', { maximumFractionDigits: 2 });
            lbl.style.visibility = 'visible';
        }).catch(err);
    }
    /** comItem_ValueChanged:2142 + comItem_Leave:866. */
    function itemChanged() {
        var itemId = intOf($id('comItem').value);
        return packUom().then(function () { return bindVariant(itemId); }).then(balanceStock);
    }

    /** AmountCaluculation:2022 - Qty / RateUOM.Equivalent * Rate; Math.Round(2).ToString(). */
    function amountCalc() {
        var rateUom = uomList.find(function (u) { return u.Id === intOf($id('comRateUOM').value); });
        var eq = rateUom && selText('comRateUOM') !== '' ? num(rateUom.Equivalent) : 0;
        var rate = $id('txtRate').value.trim() !== '' ? num($id('txtRate').value) : 0;
        var qty = $id('txtQty').value.trim() !== '' ? num($id('txtQty').value) : 0;
        $id('txtAmount').value = (qty > 0 && eq > 0 && rate > 0) ? String(roundEven(qty / eq * rate, 2)) : '0';
    }
    /** txtRate_TextChanged:2064 / comRateUOM_Leave:2058. */
    function rateChanged() { amountCalc(); billAmount(); }

    /* ------------------------------------------------------------------ totals */

    function billProportion() {                                                 // BillProportion
        table.forEach(function (r) { r.BillAmount = num(r.ItemAmount) + num(r.Freights) + num(r.TaxAmount); });
    }
    function billAmount() {                                                     // BillAmount:1391
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
    function freightProportion() {                                              // FreightProportion:2085
        var net = 0, credit = 0;
        table.forEach(function (r) { net += num(r.ItemQty); });
        freight.forEach(function (f) { credit += num(f.Freight); });
        table.forEach(function (r) { r.Freights = credit > 0 ? roundEven(credit, 0) / net * num(r.ItemQty) : 0; });
        billProportion();
        renderGrid();
    }

    /* ------------------------------------------------------------------ detail grid (grd) */

    var GRID_COLS = ['Warehouse', 'Item', 'ItemCode', 'JobLot', 'UOM', 'AttributeVariant', 'ItemQty', 'Rate', 'RateUOM',
        'ItemAmount', 'BillAmount', 'Freights', 'RemarksDetail', 'GpNo', 'VehicleNo'];   // dtGrid order, hidden columns left out
    function cellText(r, c) {
        if (c === 'Freights' || c === 'TaxAmount') return fmt3(r[c]);
        return r[c] === null || r[c] === undefined ? '' : r[c];
    }
    function renderGrid() {
        var t = $id('grd');
        t.tHead.innerHTML = '<tr><th>X</th>' + GRID_COLS.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr ondblclick="PiVariant.edit(' + i + ')"><td><button type="button" class="cx-link" onclick="PiVariant.deleteRow(' + i + ')">X</button></td>' +
                GRID_COLS.map(function (c) { return '<td' + (typeof r[c] === 'number' ? ' class="num"' : '') + '>' + esc(cellText(r, c)) + '</td>'; }).join('') + '</tr>';
        }).join('');
        var s = { ItemQty: 0, ItemAmount: 0, BillAmount: 0, Freights: 0 };
        table.forEach(function (r) { Object.keys(s).forEach(function (k) { s[k] += num(r[k]); }); });
        t.tFoot.innerHTML = '<tr><td></td>' + GRID_COLS.map(function (c) {
            return '<td class="num">' + (c in s ? esc(fmt3(s[c])) : '') + '</td>';
        }).join('') + '</tr>';
    }

    /** FormValidationDetail:504 - same messages, same order. */
    function validateDetail() {
        var checks = [['comWarehouse', 'Warehouse Field is Required'], ['comItem', 'Item Name Field is Required'],
            ['comjobLot', 'Job/Lot Field is Required'], ['cmbPackUom', 'UOM Field is Required']];
        for (var k = 0; k < checks.length; k++) {
            if (intOf($id(checks[k][0]).value) === 0) { alert(checks[k][1]); $id(checks[k][0]).focus(); return false; }
        }
        if ($id('txtQty').value.trim() === '' || num($id('txtQty').value) === 0) { alert('Qty Field is Required'); $id('txtQty').focus(); return false; }
        if ($id('txtRate').value.trim() === '' || num($id('txtRate').value) === 0) { alert('Rate Field is Required'); $id('txtRate').focus(); return false; }
        if (intOf($id('comRateUOM').value) === 0) { alert('Rate UOM Field is Required'); $id('comRateUOM').focus(); return false; }
        if ($id('txtAmount').value.trim() === '' || num($id('txtAmount').value) === 0) { alert('ItemAmount Field is Required'); $id('txtAmount').focus(); return false; }
        return true;
    }
    function entryRow() {
        var it = selectedItem() || {};
        return {
            WarehouseId: intOf($id('comWarehouse').value), Warehouse: selText('comWarehouse'),
            ItemId: intOf($id('comItem').value), Item: it.ItemName || '', ItemCode: it.ItemCode || '',
            JobLotId: intOf($id('comjobLot').value), JobLot: selText('comjobLot'),
            UOMId: intOf($id('cmbPackUom').value), UOM: selText('cmbPackUom'),
            AttributeVariantId: intOf($id('CmbAttributeVariant').value), AttributeVariant: selText('CmbAttributeVariant'),
            ItemQty: num($id('txtQty').value), Rate: num($id('txtRate').value),
            RateUOMId: intOf($id('comRateUOM').value), RateUOM: selText('comRateUOM'),
            ItemAmount: num($id('txtAmount').value.trim()), TaxNameId: 0, TaxName: '', TaxPercent: 0, TaxAmount: 0   // DocumentTypeId 61: no tax lookup
        };
    }
    function add() {                                                            // btnAdd_Click:1782
        if (!validateDetail()) return;
        var r = entryRow();
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
    function edit(i) {                                                          // grd_DoubleClick:1874
        var r = table[i]; if (!r) return;
        updateDetailIndex = i;
        $id('comWarehouse').value = String(r.WarehouseId);
        $id('comItem').value = String(r.ItemId);
        packUom().then(function () { return bindVariant(r.ItemId); }).then(function () {
            $id('comjobLot').value = String(r.JobLotId);
            $id('cmbPackUom').value = String(r.UOMId);
            $id('CmbAttributeVariant').value = String(r.AttributeVariantId || 0);
            $id('txtQty').value = r.ItemQty;
            $id('txtRate').value = r.Rate;
            $id('comRateUOM').value = String(r.RateUOMId);
            $id('txtAmount').value = r.ItemAmount;
            $id('txtRemarksdetail').value = r.RemarksDetail || '';
            $id('txtGpNo').value = r.GpNo || '';
            $id('txtVehicleNo').value = r.VehicleNo || '';
            $id('btnAdd').classList.add('is-hidden'); $id('btnUpdateDetail').classList.remove('is-hidden'); $id('btnCancelUpdateDetial').classList.remove('is-hidden');
            $id('comWarehouse').focus();
        });
    }
    function updateDetail() {                                                   // btnUpdateDetail_Click:1910
        if (!validateDetail()) return;
        var r = table[updateDetailIndex]; if (!r) return;
        var n = entryRow();
        Object.keys(n).forEach(function (k) { r[k] = n[k]; });
        r.RemarksDetail = $id('txtRemarksdetail').value;
        r.GpNo = String(cInt($id('txtGpNo').value));
        r.VehicleNo = $id('txtVehicleNo').value;
        $id('btnAdd').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
        freightProportion();
        billAmount();
        resetDetail();
    }
    function cancelDetail() {                                                   // btnCancelUpdateDetial_Click
        $id('btnAdd').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
        resetDetail();
    }
    /** DeleteDetailrow:1995 - a saved row is checked by usp_StockInReferenceValidationReferredOrNot first. No recalculation. */
    function deleteRow(i) {
        var r = table[i]; if (!r) return;
        var detailId = intOf(r.Id);
        if (detailId > 0) {
            if (!confirm('Are you sure you want to delete this record?')) return;
            C.postJson(api + '/' + recId + '/detail/' + detailId + '/check-delete', {})
                .then(function () { table.splice(table.indexOf(r), 1); renderGrid(); })
                .catch(err);
            return;
        }
        table.splice(i, 1);
        renderGrid();
    }
    function resetDetail() {                                                    // ResetDetail:1340
        $id('comItem').value = '0';
        $id('txtQty').value = ''; $id('txtRate').value = '';
        $id('CmbAttributeVariant').value = '0';
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
    /** SupCustIdUpdateforFrieghtGrid / SupCustIdUpdateforGLGrid - GL id = first account with the same TITLE. */
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
            '" value="' + esc(v === null || v === undefined ? '' : v) + '" onchange="PiVariant.cell(this)"></td>';
    }
    function renderFreight() {
        var t = $id('grdFreight');
        t.tHead.innerHTML = '<tr><th>X</th><th>+</th><th>Transporter</th><th>Credit</th><th>Remarks</th></tr>';
        t.tBodies[0].innerHTML = freight.map(function (r, i) {
            return '<tr><td><button type="button" class="cx-link" onclick="PiVariant.freightDel(' + i + ')">X</button></td>' +
                '<td><button type="button" class="cx-link" onclick="PiVariant.freightAdd(' + i + ')">+</button></td>' +
                '<td><select class="win-combo" data-g="f" data-i="' + i + '" data-k="Transporter" onchange="PiVariant.cell(this)">' + accountOptions(r.Transporter) + '</select></td>' +
                inp('f', i, 'Freight', r.Freight, 'num') + inp('f', i, 'Remarks', r.Remarks) + '</tr>';
        }).join('');
        var tot = freight.reduce(function (s, r) { return s + num(r.Freight); }, 0);
        t.tFoot.innerHTML = '<tr><td></td><td></td><td></td><td class="num">' + esc(fmt3(tot)) + '</td><td></td></tr>';
    }
    function addFreightRow() { freight.push({ Transporter: '0', Freight: 0, Remarks: '0', GlAccountId: 0 }); }   // AddRowInFreightGrid
    function freightDel(i) {                                                    // grdFreight_ColumnButtonClick "Delete"
        freight.splice(i, 1);
        if (!freight.length) addFreightRow();
        renderFreight();
        billAmount();
    }
    function freightAdd(i) {                                                    // "Add" - copies Transporter and Remarks only
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
            return '<tr><td><button type="button" class="cx-link" onclick="PiVariant.glDel(' + i + ')">X</button></td>' +
                '<td><button type="button" class="cx-link" onclick="PiVariant.glAdd()">+</button></td>' +
                '<td><select class="win-combo" data-g="g" data-i="' + i + '" data-k="AccountId" onchange="PiVariant.cell(this)">' + accountOptions(r.AccountId) + '</select></td>' +
                inp('g', i, 'Remarks', r.Remarks) + inp('g', i, 'Percentage', r.Percentage, 'num') + inp('g', i, 'Qty', r.Qty, 'num') +
                inp('g', i, 'Rate', r.Rate, 'num') + inp('g', i, 'Debit', r.Debit, 'num') + inp('g', i, 'Credit', r.Credit, 'num') + '</tr>';
        }).join('');
        var d = 0, c = 0;
        gl.forEach(function (r) { d += num(r.Debit); c += num(r.Credit); });
        t.tFoot.innerHTML = '<tr><td colspan="7"></td><td class="num">' + esc(fmt3(d)) + '</td><td class="num">' + esc(fmt3(c)) + '</td></tr>';
    }
    function addGlRow() { gl.push({ AccountId: '0', Remarks: '', Percentage: '0', Qty: '0', Rate: '0', Debit: 0, Credit: 0, GlAccountId: 0 }); } // AddRowInGLGrid
    function glDel(i) { gl.splice(i, 1); if (!gl.length) addGlRow(); renderGl(); billAmount(); }
    function glAdd() { addGlRow(); renderGl(); billAmount(); }

    /** grdFreight_CellUpdated / grdGLedger_CellUpdated. */
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

    function supplierChanged() {                                                // comsupplier_ValueChanged:2118
        billAmount();
        var id = intOf($id('comsupplier').value);
        if (look.suppliers.length > 0 && id > 0) {
            var s = look.suppliers.find(function (x) { return x.Id === id; });
            if (s) supplierGlText = String(s.GlAccountId);
        }
    }
    function itemByChanged() { itemNameBind(); }                                // rdbtnItemName_CheckedChanged

    /* ------------------------------------------------------------------ save / delete / print */

    function payload() {
        return {
            Id: recId, DocDate: $id('DocDate').value, CompanyId: intOf($id('comCompany').value),
            BranchesId: intOf($id('combranches').value), ProjectsId: intOf($id('comproject').value),
            SupplierCustomerId: intOf($id('comsupplier').value),
            ManualBillNo: $id('txtbillno').value, RemarksHeader: $id('txtremarks').value,
            rows: table.map(function (r) {
                return { Id: intOf(r.Id), ItemId: r.ItemId, ItemCode: r.ItemCode, Item: r.Item, WarehouseId: r.WarehouseId, Warehouse: r.Warehouse,
                    JobLotId: r.JobLotId, JobLot: r.JobLot, UOMId: r.UOMId, UOM: r.UOM,
                    AttributeVariantId: intOf(r.AttributeVariantId), AttributeVariant: r.AttributeVariant || '',
                    ItemQty: num(r.ItemQty), Rate: num(r.Rate),
                    RateUOMId: r.RateUOMId, RateUOM: r.RateUOM, ItemAmount: num(r.ItemAmount), TaxNameId: intOf(r.TaxNameId), TaxName: r.TaxName || '',
                    TaxPercent: num(r.TaxPercent), TaxAmount: num(r.TaxAmount), BillAmount: num(r.BillAmount), Freights: num(r.Freights),
                    RemarksDetail: r.RemarksDetail || '', GpNo: String(r.GpNo === null || r.GpNo === undefined ? '' : r.GpNo), VehicleNo: r.VehicleNo || '' };
            }),
            freight: freight.map(function (r) { return { Transporter: String(r.Transporter), Freight: num(r.Freight), Remarks: r.Remarks === null || r.Remarks === undefined ? '' : String(r.Remarks), GlAccountId: intOf(r.GlAccountId) }; }),
            journal: gl.map(function (r) {
                return { AccountId: String(r.AccountId), Remarks: String(r.Remarks === null || r.Remarks === undefined ? '' : r.Remarks), Percentage: String(r.Percentage),
                    Qty: String(r.Qty), Rate: String(r.Rate), Debit: num(r.Debit), Credit: num(r.Credit), GlAccountId: intOf(r.GlAccountId) };
            })
        };
    }
    function formValidation() {                                                 // FormValidation:464
        if (intOf($id('combranches').value) === 0) { alert('Branch Field is Required'); $id('combranches').focus(); return false; }
        if (intOf($id('comproject').value) === 0) { alert('Project Field is Required'); $id('comproject').focus(); return false; }
        if (intOf($id('comsupplier').value) === 0) { alert('Supplier Field is Required'); $id('comsupplier').focus(); return false; }
        var d = $id('txtdocno').value.trim();
        if (d === '' || d === '0') { alert('DocNo Field is Required'); return false; }
        return true;
    }
    function insert() {                                                         // Insert:975
        if (!formValidation()) return;
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
        }).catch(err);
    }
    function save() { recId = 0; insert(); }                                    // saveToolStripButton_Click
    function update() {                                                         // btnUpdate_Click
        if (approved) { alert('Record Not Update because Record has approved'); return; }
        insert();
    }
    function del() {                                                            // btnDelete_Click
        if (approved) { alert('Record Not Update because Record has approved'); return; }
        if (!(recId > 0)) { alert('Record Id Not Found.....'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + recId + '/delete', {}).then(function (res) { alert(res.message); reset(); }).catch(err);
    }
    function printVoucher() {                                                   // btnPrint_Click -> VoucherReport_118
        if (!voucherHeadId) { alert('VoucherId Not Found'); return; }
        C.printSlip(api + '/' + recId + '/voucher', '118-Voucher');
    }
    function printSlip() {                                                      // btnSlip_Click
        if (!recId) { alert('Record Id Not Found'); return; }
        C.printSlip(api + '/' + recId + '/slip', '233-Purchase Invoice Store Bill Direct');
    }

    /* ------------------------------------------------------------------ open */

    function readById(id) {                                                     // ReadById:1215
        return C.getJson(api + '/' + id).then(function (h) {
            recId = h.Id;
            $id('comCompany').value = String(h.CompanyId || 0);
            $id('combranches').value = String(h.BranchesId || 0);
            $id('comproject').value = String(h.ProjectsId || 0);
            $id('txtdocno').value = h.DocNo; $id('txtTaxInvoiceNo').value = h.SalesTaxNo;
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
        }).catch(err);
    }
    function openFromHistory(id) {                                              // grdHistory_DoubleClick / "Edit"
        if (!look.rights.update) { alert("You don't Have Update Rights..."); return; }
        readById(id);
    }

    /* ------------------------------------------------------------------ history */

    var H_COLS = ['DocNo', 'DocDate', 'DueDays', 'DueDate', 'ManualBillNo', 'SupplierName', 'BillAmount', 'ApprovedStatus', 'EntryUser',
        'EntryDate', 'ModifyUser', 'ModifyDate', 'NoOfAttachments', 'Remarks'];
    function histCell(h, k) {
        if (k === 'DocDate' || k === 'DueDate') return C.gridDate(h[k]);
        if (k === 'EntryDate' || k === 'ModifyDate') return C.gridDateTime(h[k]);
        return h[k];
    }
    function showHistory() {                                                    // btnshow_Click -> GetAll
        var dt = (document.querySelector('input[name=histDate]:checked') || {}).value || 'doc';
        C.getJson(api + '/history' + C.qs({
            dateType: dt,
            fromDate: $id('chkFromDate').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDate').checked ? $id('ToDateHistory').value : '',
            fromDocNo: intOf($id('txtFromDocNoHistory').value), toDocNo: intOf($id('txtToDocNoHistory').value),
            supplierCustomerId: intOf($id('cmbSupplierNameHistory').value)
        })).then(function (list) {
            var rt = look.rights || {}, t = $id('grdHistory');
            t.tHead.innerHTML = '<tr>' + (rt.print ? '<th>Slip</th><th>Voucher</th>' : '') + (rt.update ? '<th>Edit</th>' : '') +
                H_COLS.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = (list || []).map(function (h) {
                return '<tr data-id="' + h.Id + '" onclick="PiVariant.historyDetail(this)" ondblclick="PiVariant.open(' + h.Id + ')">' +
                    (rt.print ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PiVariant.historySlip(' + h.Id + ')">Slip</button></td>' +
                        '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PiVariant.historyVoucher(' + h.Id + ',' + intOf(h.VoucherHeadId) + ')">Voucher</button></td>' : '') +
                    (rt.update ? '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PiVariant.open(' + h.Id + ')">Edit</button></td>' : '') +
                    H_COLS.map(function (c) {
                        return '<td' + (typeof h[c] === 'number' ? ' class="num"' : '') + '>' + esc(histCell(h, c)) + '</td>';
                    }).join('') + '</tr>';
            }).join('');
            if (!list || !list.length) t.tHead.innerHTML = '';                  // ClearStructure
            $id('grdDetail').tHead.innerHTML = ''; $id('grdDetail').tBodies[0].innerHTML = ''; $id('grdDetail').tFoot.innerHTML = '';
        }).catch(err);
    }
    var D_COLS = ['Warehouse', 'Item', 'ItemCode', 'JobLot', 'UOM', 'AttributeVariant', 'ItemQty', 'Rate', 'RateUOM', 'ItemAmount',
        'BillAmount', 'Freights', 'RemarksDetail', 'GpNo', 'VehicleNo'];
    var D_SUM = { ItemQty: 1, ItemAmount: 1, BillAmount: 1, Freights: 1 };
    function historyDetail(tr) {                                                // grdHistory_SelectionChanged -> GetDetailGrdByHeadId
        document.querySelectorAll('#grdHistory tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        C.getJson(api + '/' + tr.getAttribute('data-id')).then(function (h) {
            var t = $id('grdDetail'), rows = h.rows || [];
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
            t.tHead.innerHTML = '<tr>' + D_COLS.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (r) {
                return '<tr>' + D_COLS.map(function (c) { return '<td' + (typeof r[c] === 'number' ? ' class="num"' : '') + '>' + esc(D_SUM[c] || c === 'Rate' ? fmt3(r[c]) : r[c]) + '</td>'; }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = '<tr>' + D_COLS.map(function (c) {
                return '<td class="num">' + (D_SUM[c] ? esc(fmt3(rows.reduce(function (s, r) { return s + num(r[c]); }, 0))) : '') + '</td>';
            }).join('') + '</tr>';
        }).catch(err);
    }
    function newHistory() {                                                     // btnNewHistory_Click:3013
        $id('FromDateHistory').value = C.today(); $id('ToDateHistory').value = C.today();
        $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = '';
        $id('cmbSupplierNameHistory').value = '0';
        ['grdHistory', 'grdDetail'].forEach(function (id) { var t = $id(id); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; if (t.tFoot) t.tFoot.innerHTML = ''; });
    }
    function refreshHistory() {                                                 // btnRefreshHistory_Click:3032 -> suppliercustomer()
        return C.getJson(api + '/lookups').then(function (l) { look.suppliers = l.suppliers || []; bindSuppliers(); }).catch(err);
    }

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    /* ------------------------------------------------------------------ load / reset / refresh */

    function init() {                                                           // InvfrmPurchasedirectInvoice_Load:336
        return C.getJson(api + '/lookups').then(function (l) {
            look = l;
            var r = l.rights || {};
            $id('btnSave').disabled = !r.save; $id('btnUpdate').disabled = !r.update;
            $id('btnPrint').disabled = !r.print;
            $id('ChkPrintSlip').disabled = !r.print; $id('ChkPrintSlip').checked = !!r.print;
            $id('ChkBok').disabled = !r.print; $id('ChkBok').checked = !!r.print;
            if (!r.view) { $id('rightsNote').textContent = 'You do not have the View right for this screen.'; $id('rightsNote').classList.remove('is-hidden'); }
            addGlRow(); renderGl();
            addFreightRow(); renderFreight();
            bindWarehouses();                                                   // bindWareHouse:557
            bindSuppliers();                                                    // suppliercustomer:574
            itemNameBind();                                                     // item():604
            bindFirstReal('comCompany', l.companies, 'CompName');               // Company():668
            bindFirstReal('comproject', l.projects, 'ProjectName');             // Project():685
            bindJobLots();                                                      // Branchlot():719
            bindFirstReal('combranches', l.branches, 'BranchName');             // braches():702
            C.fillSelect('cmbPackUom', [], 'Id', 'UOMCode'); C.fillSelect('comRateUOM', [], 'Id', 'UOMCode');
            C.fillSelect('CmbAttributeVariant', [], 'Id', 'ItemAttribute');
            C.fillSelect('cmbSupplierNameHistory', [], 'Id', 'CompanyName');    // never bound on the desktop
            C.fillSelect('CmbTaxAccount', [], 'Id', 'AccountTitle');
            $id('txtdocno').value = l.docNo || '';
            renderGrid();
        }).catch(err);
    }
    function reset() {                                                          // Reset:1403
        recId = 0; voucherHeadId = 0; approved = false;
        $id('comsupplier').value = '0';
        $id('CmbTaxAccount').value = '0';
        $id('txtbillno').value = ''; $id('txtremarks').value = '';
        $id('comItem').value = '0';
        $id('comjobLot').value = '0';
        $id('txtQty').value = ''; $id('txtRate').value = '';
        $id('comRateUOM').value = '0';
        $id('txtAmount').value = '';
        $id('comWarehouse').value = '0';
        $id('txtGpNo').value = ''; $id('txtVehicleNo').value = '';
        $id('txtBillAmount').value = '';
        freight = []; gl = []; table = [];
        addGlRow(); renderGl();
        addFreightRow(); renderFreight();
        renderGrid();
        $id('btnSave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden'); $id('btnDelete').classList.add('is-hidden');
        $id('btnAdd').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden');
        $id('lblRecId').textContent = '';
        return C.getJson(api + '/numbers').then(function (n) {                  // DocumentNo()
            if (intOf(n.docNo) > 0) $id('txtdocno').value = n.docNo;
        }).catch(err);
    }
    /** btnRefresh_Click:1363 - AccountsFill, GridcomboBind (reads a missing "AccountTypeId" column and always shows its error), then the binds. */
    function refresh() {
        return C.getJson(api + '/lookups').then(function (l) {
            if (l.accounts && l.accounts.length) look.accounts = l.accounts;    // AccountsFill clears and refills only when rows came back
            if (look.accounts.length) alert("Column 'AccountTypeId' does not belong to table .");   // GridcomboBind:1433 (desktop defect, reproduced)
            look.warehouses = l.warehouses || []; bindWarehouses();             // bindWareHouse
            look.suppliers = l.suppliers || []; bindSuppliers();                // suppliercustomer
            look.items = l.items || []; itemNameBind();                         // item()
            look.projects = l.projects || []; bindFirstReal('comproject', look.projects, 'ProjectName');   // Project()
            look.jobLots = l.jobLots || []; bindJobLots();                      // Branchlot()
            look.branches = l.branches || []; bindFirstReal('combranches', look.branches, 'BranchName');   // braches()
        }).catch(err);
    }

    window.PiVariant = {
        reset: reset, refresh: refresh, save: save, update: update, del: del, printVoucher: printVoucher, printSlip: printSlip,
        tab: tab, open: openFromHistory, supplierChanged: supplierChanged, itemByChanged: itemByChanged, itemChanged: itemChanged,
        balanceStock: balanceStock, amountCalc: amountCalc, rateChanged: rateChanged,
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
