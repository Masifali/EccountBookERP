/* ============================================================================================
 * Screen 346 "Purchase Invoice Return Store" — Architecture.WinApp.StoreManagement.PurchaseInvoiceReturn_Store
 * (ScreenName PurchaseInvoiceReturn_Store), DocumentTypeId 145.
 *
 * Two ways in, never mixed (btnAdd_Click:1365, BtnLoadSaleInvoice_Click:5013):
 *   - direct rows through the Detail box ("+", double-click a row to edit it), or
 *   - Load Purchase Invoice (frmPendingPurchaseInvoiceForReturn) — one invoice; Return Qty, Tax, Tax %,
 *     GP Date/No, Reason, Vehicle and Remarks are then edited in the grid (grdSettings:1539).
 *
 *   BillAmount:3720                 header Bill Amount / Invoice Qty / Fcy Amount
 *   GridItemNetAmountCalculation:3834  each row's "Item Net Amount"
 *   ExpProportion:3813 / FreightProportion:3780  Other Items / Charge To Product spread by quantity
 *   txtExchangeRate_TextChanged:1309  every row's FcyAmount, then BillAmount
 * Calculations run here as they run in the form; the server re-validates everything and owns the
 * document number, the voucher and the stock effects.
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, ci = C.ci, num = C.num, intOf = C.intOf;
    var api = '/api/store/purchase-invoice-return-store';

    var look = { rights: {}, formats: {}, partiesAndItems: [], racks: [], uoms: [], taxTypes: [], reasons: [], accounts: [], freightAccounts: [], otherItems: [] };
    var loaded = false;
    var recId = 0, voucherHeadId = 0, approved = false;
    var gdnExist = false, refDocumentExist = false;                           // form flags
    var grid = [], exp = [], gl = [], fr = [];                                 // dtGrid, dtInvExp, dtGrdGL, dtFreight
    var updateDetailIndex = -1;
    var dtSupplier = [], dtitem = [];
    var whValue = 0;                                                           // CmbWarehouseName.Value before any list is bound
    var dueDateValue = '';                                                     // duedate.Value
    var baseCurrency = 0, baseRate = 0;
    var historyRows = [];
    var loaderRows = [], loaderMaster = [], loaderChecked = {}, loaderInfo = null;
    var reasonRecId = 0;

    // ------------------------------------------------------------------ number formats (clsGlobalVariables)

    function F() { return look.formats || {}; }
    function roundAway(v, d) {                                                 // Math.Round(v, d, AwayFromZero)
        v = Number(v);
        if (!isFinite(v)) return v;
        d = Math.max(0, Math.min(15, intOf(d)));
        var s = v < 0 ? -1 : 1, r = Number(Math.round(Number(Math.abs(v) + 'e' + d)) + 'e-' + d);
        return s * r;
    }
    function roundEven(v, d) {                                                  // Math.Round(decimal, d)
        var f = Math.pow(10, d), x = Number(v) * f, r = Math.round(x);
        if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2);
        return r / f;
    }
    function fmtFixed(v, dec) {
        v = Number(v);
        if (isNaN(v)) return 'NaN';
        if (!isFinite(v)) return v > 0 ? 'Infinity' : '-Infinity';
        return v.toLocaleString('en-US', { minimumFractionDigits: dec, maximumFractionDigits: dec });
    }
    function fmtOpt(v, dec) {                                                   // "#,##0.###" / "#,##0.##"
        v = Number(v);
        if (!isFinite(v)) return String(v);
        return v.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dec });
    }
    function fmtAmount(v) { return fmtFixed(v, intOf(F().amountDecimals)); }   // stringFormatsingle
    function fmtRate(v) { return fmtFixed(v, intOf(F().rateDecimals)); }       // DecimalRateFormate
    function fmtFcy(v) { return fmtFixed(v, intOf(F().fcyDecimals)); }         // stringFormatsingleForFcy
    function amtRound() { return intOf(F().amountRound); }
    function clr(v) { v = Number(v); if (!isFinite(v)) return String(v); return String(Number(v.toPrecision(15))); }

    function sel(id) { var e = $id(id); return e && e.selectedOptions && e.selectedOptions[0] && e.value !== '0' ? e.selectedOptions[0].textContent : ''; }
    function val(id) { return intOf($id(id).value); }
    function setSelect(id, v) {
        var e = $id(id), s = String(v === null || v === undefined ? 0 : v);
        e.value = Array.prototype.some.call(e.options, function (o) { return o.value === s; }) ? s : (e.options.length && e.options[0].value === '0' ? '0' : '');
    }
    function addDays(iso, n) {
        var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(iso || ''); if (!m) return '';
        var d = new Date(+m[1], +m[2] - 1, +m[3]); d.setDate(d.getDate() + n);
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }

    // ------------------------------------------------------------------ header

    /* suppliercustomer:693 — distinct (SupplierCustomerId, CompanyName, GlAccountId); row.Field<int>("GlAccountId")
       throws on a NULL GL and the combo then keeps its old list. */
    function supplierBind() {
        var keep = val('CmbSupplierName'), seen = {}, list = [];
        try {
            look.partiesAndItems.forEach(function (r) {
                if (r.GlAccountId === null || r.GlAccountId === undefined) throw new Error('Specified cast is not valid.');
                var k = r.SupplierCustomerId + '|' + r.CompanyName + '|' + r.GlAccountId;
                if (!seen[k]) { seen[k] = 1; list.push({ SupplierCustomerId: r.SupplierCustomerId, CompanyName: r.CompanyName, GlAccountId: r.GlAccountId }); }
            });
        } catch (e) { alert(e.message); return; }
        dtSupplier = list;
        C.fillSelect('CmbSupplierName', dtSupplier, 'SupplierCustomerId', 'CompanyName');
        setSelect('CmbSupplierName', keep);
    }
    var supplierValue = 0;
    /* comsupplier_ValueChanged:829 — fires only when the value really changes. */
    function supplierChanged() {
        var v = val('CmbSupplierName');
        if (v === supplierValue) return;
        supplierValue = v;
        itemdtFillByParty();
        itemBind();
        billAmount();
        dtSupplier.forEach(function (s) { if (intOf(s.SupplierCustomerId) === v) $id('txtSupplierGLId').value = String(s.GlAccountId); });
    }
    function setSupplier(v) { setSelect('CmbSupplierName', v); supplierChanged(); }

    function paymentTermLeave() {                                               // combpttrm_Leave:854
        $id('txtduedays').disabled = false;
        if (val('CmbPaymentTerm') === 1) { setDueDays(''); $id('txtduedays').disabled = true; }
    }
    function setDueDays(t) { if ($id('txtduedays').value !== t) { $id('txtduedays').value = t; dueDateGenerate(); } }
    function dueDaysChanged() {                                                 // txtduedays: digits only (KeyPress:4177)
        var e = $id('txtduedays'), c = e.value.replace(/[^0-9]/g, '');
        if (c !== e.value) e.value = c;
        dueDateGenerate();
    }
    function dueDateGenerate() {                                                // DueDateGenerate:4038
        var t = $id('txtduedays').value.trim();
        dueDateValue = t !== '' ? addDays($id('DocDate').value, num(t)) : $id('DocDate').value;
        $id('duedate').value = C.isoDay(dueDateValue);
    }
    function docDateChanged() { dueDateGenerate(); }

    function currencyLeave() {                                                  // cmbCurrency_Leave:1266
        var cur = val('cmbCurrency');
        if (cur === 0 || num($id('txtExchangeRate').value) !== 0) return;
        if (cur !== baseCurrency) {
            C.getJson(api + '/last-exchange-rate' + C.qs({ currencyId: cur })).then(function (r) {
                setRate(r.found ? fmtRate(r.rate) : '0');
            }).catch(function (e) { alert(e.message); });
        } else setRate(fmtRate(baseRate));
    }
    function setRate(t) { if ($id('txtExchangeRate').value !== t) { $id('txtExchangeRate').value = t; exRateChanged(); } }
    function exchangeRateChanged() {                                            // KeyPress: decimals only
        var e = $id('txtExchangeRate'), c = e.value.replace(/[^0-9.,-]/g, '');
        if (c !== e.value) e.value = c;
        exRateChanged();
    }
    function exchangeRateLeave() { setRate(fmtRate(num($id('txtExchangeRate').value))); }

    /* GetConfigurationsFromGlobalandBind:619 */
    function configBind() {
        if (intOf(look.defaultWarehouseId) > 0) { whValue = intOf(look.defaultWarehouseId); setSelect('CmbWarehouseName', whValue); }
        if (intOf(look.defaultCityId) > 0 && val('CmbCityName') === 0) setSelect('CmbCityName', look.defaultCityId);
        if (intOf(look.defaultJobLotId) > 0 && val('CmbJobLot') === 0) setSelect('CmbJobLot', look.defaultJobLotId);
        if (intOf(look.baseCurrency) > 0) { baseCurrency = intOf(look.baseCurrency); if (val('cmbCurrency') === 0) setSelect('cmbCurrency', baseCurrency); }
        if (num(look.baseCurrencyRate) > 0) { baseRate = num(look.baseCurrencyRate); if (num($id('txtExchangeRate').value) === 0) setRate(fmtRate(baseRate)); }
    }

    // ------------------------------------------------------------------ entry bar

    function itemdtFillByParty() {                                              // :884
        var sup = val('CmbSupplierName'), seen = {};
        dtitem = [];
        look.partiesAndItems.forEach(function (r) {
            if (intOf(r.SupplierCustomerId) !== sup) return;
            var k = r.ItemId + '|' + r.ItemName + '|' + r.ItemCodeNew;
            if (!seen[k]) { seen[k] = 1; dtitem.push({ Id: intOf(r.ItemId), ItemName: r.ItemName, ItemCode: r.ItemCodeNew }); }
        });
    }
    function itemBind() {                                                       // ItemNameBind:912
        var keep = val('CmbItemName');
        C.fillSelect('CmbItemName', dtitem, 'Id', $id('rdbtnItemName').checked ? 'ItemName' : 'ItemCode');
        setSelect('CmbItemName', keep);
        if (dtitem.length > 0) itemLeave();
    }
    function selectedItem() { var id = val('CmbItemName'); return dtitem.find(function (x) { return x.Id === id; }) || null; }
    function itemLeave() {                                                      // CmbItemName_Leave:1103
        var itemId = val('CmbItemName'), wh = val('CmbWarehouseName') || whValue;
        uomBind(itemId);
        warehouseBind(itemId);
        rackBind(itemId, wh);                                                   // the warehouse read BEFORE the rebind
        stock();
    }
    function uomBind(itemId) {                                                  // ItemUomFromGlobalBind:987 — retained by UOMCode text
        var text = sel('CmbPackUOM');
        var list = look.uoms.filter(function (u) { return u.ItemId === itemId; });
        C.fillSelect('CmbPackUOM', list, 'Id', 'UOMCode');
        var m = list.find(function (u) { return u.UOMCode === text; });
        $id('CmbPackUOM').value = m ? String(m.Id) : '0';
    }
    function warehouseBind(itemId) {                                            // WareHouseBindFromGlobalRacksByItemId:931
        var keep = val('CmbWarehouseName') || whValue;
        var whs = C.distinct(look.racks.filter(function (x) { return x.ItemId === itemId; }), 'WarehouseId')
            .map(function (w) { return { Id: w.WarehouseId, WareHouseName: w.WareHouseName }; });
        C.fillSelect('CmbWarehouseName', whs, 'Id', 'WareHouseName');
        setSelect('CmbWarehouseName', keep);
        whValue = val('CmbWarehouseName');
    }
    function rackBind(itemId, whId) {                                           // RackBindFromGlobalRacksByItemId:959
        var keep = val('CmbRackName');
        var racks = C.distinct(look.racks.filter(function (x) { return x.ItemId === itemId && (whId === 0 || x.WarehouseId === whId); }), 'Id');
        C.fillSelect('CmbRackName', racks, 'Id', 'RackName');
        setSelect('CmbRackName', keep);
    }
    function warehouseLeave() {                                                 // CmbWarehouseName_Leave:1120
        whValue = val('CmbWarehouseName');
        rackBind(val('CmbItemName'), whValue);
        stock();
    }
    function rackLeave() {                                                      // CmbRackName_Leave:1128
        var r = look.racks.find(function (x) { return x.Id === val('CmbRackName'); });
        if (r && r.WarehouseId > 0) { setSelect('CmbWarehouseName', r.WarehouseId); whValue = val('CmbWarehouseName'); }
        stock();
    }
    function packUomLeave() { amountCalc(); taxCalc(); stock(); }            // comPackUOM_Leave:3973
    function stock() {                                                          // AvailableStockGetByItem:1158
        C.getJson(api + '/stock' + C.qs({ recId: recId, docDate: $id('DocDate').value, itemId: val('CmbItemName'),
            warehouseId: val('CmbWarehouseName'), jobLotId: val('CmbJobLot'), uomId: val('CmbPackUOM') }))
            .then(function (r) { $id('lblBalance').textContent = r.balance; })
            .catch(function (e) { alert(e.message); });
    }
    function amountCalc() {                                                     // AmountCaluculation:3873
        var q = num($id('txtQty').value), r = num($id('txtRate').value);
        if (q > 0 && r > 0) {
            var a = roundAway(q * r, amtRound());
            $id('txtAmount').value = fmtAmount(a); $id('txtTotalAmount').value = fmtAmount(a);
        } else { $id('txtAmount').value = '0'; $id('txtTotalAmount').value = '0'; }
    }
    function taxCalc() {                                                        // CalculateTaxAmountandTotalAmount:3906
        var t = taxTypeId();
        if (t > 0) {
            var pct;
            if (!(num($id('txtTaxPercent').value) > 0)) {
                var row = look.taxTypes.find(function (x) { return x._k === $id('CmbTaxType').value; });
                pct = row ? num(row.TaxPercent) : 0;
                $id('txtTaxPercent').value = fmtOpt(pct, 3);
            } else pct = num($id('txtTaxPercent').value);
            if (pct > 100) { alert("Tax % Can't be greater than 100..."); pct = 100; $id('txtTaxPercent').value = String(pct); }
            var amount = num($id('txtAmount').value), tax = amount * pct / 100;
            $id('txtTaxAmount').value = fmtAmount(tax);
            $id('txtTotalAmount').value = fmtAmount(amount + tax);
        } else $id('txtTotalAmount').value = fmtAmount(num($id('txtAmount').value));
    }
    function qtyRateChanged() { amountCalc(); taxCalc(); }                   // txtQty / txtRate _TextChanged
    function taxPercentChanged() {                                              // txtTaxPercent_TextChanged:3988
        if (taxTypeId() > 0) { taxCalc(); return; }
        $id('txtTaxPercent').value = ''; $id('txtTaxAmount').value = '';
        $id('txtTotalAmount').value = fmtAmount(num($id('txtAmount').value));
    }
    function taxTypeLeave() { taxPercentChanged(); }                            // CmbTaxType_Leave:4007 (same body)

    function formValidationDetail() {                                           // FormValidationDetila:2406
        if (val('CmbWarehouseName') === 0) { alert('Warehouse Field is Required'); $id('CmbWarehouseName').focus(); return false; }
        if (val('CmbItemName') === 0) { alert('Item Name Field is Required'); $id('CmbItemName').focus(); return false; }
        var q = $id('txtQty').value.trim();
        if (q === '' || q === '0') { alert('Qty Field is Required'); $id('txtQty').focus(); return false; }
        if (val('CmbPackUOM') === 0) { alert('Pack Unit Field is Required'); $id('CmbPackUOM').focus(); return false; }
        var r = $id('txtRate').value.trim();
        if (r === '' || r === '0') { alert('Rate Field is Required'); $id('txtRate').focus(); return false; }
        var a = $id('txtAmount').value;
        if (a === '' || a === '0') { alert('Amount Field is Required'); $id('txtAmount').focus(); return false; }
        if (val('CmbCityName') === 0) { alert('city Field is Required'); $id('CmbCityName').focus(); return false; }
        return true;
    }
    function fillDetailRow(dr) {                                                // FillDetailRow:1410
        var it = selectedItem(), uom = look.uoms.find(function (u) { return u.Id === val('CmbPackUOM'); });
        var amount = num($id('txtAmount').value);
        dr.Id = 0; dr.InvGdnId = 0; dr.InvGdnDetailId = 0; dr.GdnNo = 0;
        dr.ItemId = val('CmbItemName'); dr.ItemCode = it ? it.ItemCode : ''; dr.Item = it ? it.ItemName : '';
        dr.WarehouseId = val('CmbWarehouseName'); dr.Warehouse = sel('CmbWarehouseName');
        dr.RackId = val('CmbRackName'); dr.RackName = sel('CmbRackName');
        dr.ItemConditionId = val('CmbItemCondition'); dr.ItemCondition = sel('CmbItemCondition');
        dr.JobLotId = val('CmbJobLot'); dr.JobLot = sel('CmbJobLot');
        dr.PackUOMId = val('CmbPackUOM'); dr.PackUOM = sel('CmbPackUOM'); dr.PackEquivalent = uom ? num(uom.Equivalent) : 0;
        dr.ItemQty = num($id('txtQty').value); dr.Rate = num($id('txtRate').value); dr.ItemAmount = amount; dr.FcyAmount = amount;
        dr.TaxTypeId = taxTypeId(); dr.TaxPercent = num($id('txtTaxPercent').value); dr.TaxAmount = num($id('txtTaxAmount').value);
        dr.BillAmount = 0;
        dr.GpDate = $id('txtgpdate').value || C.today();
        dr.GpNo = intOf($id('txtgatepassno').value); dr.VehicleNo = $id('txtvehicleno').value;
        dr.CityId = val('CmbCityName'); dr.CityName = sel('CmbCityName'); dr.Remarks = $id('txtRemarksDetail').value;
        return dr;
    }
    function blankRow() {
        return { Id: 0, InvGdnId: 0, InvGdnDetailId: 0, GdnNo: 0, RefDocumentTypeId: 0, RefDocId: 0, RefDocSubId: 0, ItemId: 0, ItemCode: '', Item: '',
            WarehouseId: 0, Warehouse: '', RackId: 0, RackName: '', ItemConditionId: 0, ItemCondition: '', JobLotId: 0, JobLot: '', PackUOMId: 0, PackUOM: '',
            PackEquivalent: 0, ItemQty: 0, Rate: 0, ItemAmount: 0, FcyAmount: 0, TaxTypeId: 0, TaxPercent: 0, TaxAmount: 0, BillAmount: 0, Expense: 0,
            Journal: 0, Freight: 0, GpDate: null, GpNo: 0, VehicleNo: '', CityId: 0, CityName: '', Remarks: '', ReasonId: 0, BalQty: 0 };
    }
    function add() {                                                            // btnAdd_Click:1361
        if (grid.length > 0 && (gdnExist || refDocumentExist)) {
            alert('You cannot add a manual record because a ' + (gdnExist ? 'GDN' : 'Purchase Invoice') + ' record already exists in the grid.');
            return;
        }
        if (!formValidationDetail()) return;
        grid.push(fillDetailRow(blankRow()));
        bindGrid(); expProportion(); freightProportion(); exRateChanged();
        resetDetail();
    }
    function updateDetail() {                                                   // btnUpdateDetail_Click:1387
        if (!formValidationDetail() || updateDetailIndex < 0 || updateDetailIndex >= grid.length) return;
        fillDetailRow(grid[updateDetailIndex]);
        bindGrid(); expProportion(); freightProportion(); exRateChanged();
        showAddButtons(true);
    }
    function cancelDetail() { showAddButtons(true); }                           // btnCancelUpdateDetial_Click:1493
    function showAddButtons(addVisible) {
        $id('btnAdd').classList.toggle('is-hidden', !addVisible);
        $id('btnUpdateDetail').classList.toggle('is-hidden', addVisible);
        $id('btnCancelUpdateDetial').classList.toggle('is-hidden', addVisible);
    }
    function resetDetail() {                                                    // ResetDetail:2553
        $id('CmbPackUOM').value = '0';
        $id('txtQty').value = ''; $id('txtRate').value = ''; $id('txtAmount').value = '';
        if ($id('ChkResetOnSave').checked) optionResetFields();
        $id('CmbSupplierName').disabled = grid.length > 0;
        $id('CmbItemName').focus();
    }
    function optionResetFields() {                                              // OptionResetFields:2574
        $id('CmbItemName').value = '0'; $id('CmbJobLot').value = '0'; $id('CmbWarehouseName').value = '0'; whValue = 0;
        $id('txtgatepassno').value = ''; $id('txtvehicleno').value = ''; $id('CmbCityName').value = '0';
    }
    function enableFields(on) {                                                 // DisableFields:2453 / EnablesFields:2469
        ['CmbWarehouseName', 'CmbItemName', 'CmbPackUOM', 'CmbJobLot', 'txtQty'].forEach(function (id) { $id(id).disabled = !on; });
    }
    function rowDoubleClick(i) {                                                // grd_DoubleClick:1452
        var r = grid[i];
        if (!r || gdnExist || refDocumentExist) return;
        updateDetailIndex = i;
        if (intOf(r.InvGdnId) > 0) enableFields(false);
        setSelect('CmbItemName', r.ItemId); itemLeave();
        setSelect('CmbWarehouseName', r.WarehouseId); whValue = val('CmbWarehouseName');
        rackBind(r.ItemId, r.WarehouseId);
        setSelect('CmbRackName', r.RackId); setSelect('CmbItemCondition', r.ItemConditionId); setSelect('CmbJobLot', r.JobLotId);
        setSelect('CmbPackUOM', r.PackUOMId);
        $id('txtQty').value = fmtOpt(r.ItemQty, 2); qtyRateChanged();
        $id('txtRate').value = fmtRate(r.Rate); qtyRateChanged();
        $id('txtAmount').value = fmtAmount(r.ItemAmount);
        taxTypeValue(r.TaxTypeId);
        $id('txtTaxPercent').value = clr(r.TaxPercent); taxPercentChanged();
        $id('txtTaxAmount').value = clr(r.TaxAmount);
        $id('txtgpdate').value = C.isoDay(r.GpDate) || $id('txtgpdate').value;
        $id('txtgatepassno').value = String(intOf(r.GpNo)); $id('txtvehicleno').value = r.VehicleNo || '';
        setSelect('CmbCityName', r.CityId);
        showAddButtons(false);
    }
    /* CmbTaxType.Value — the TaxNameId of the selected schedule row (rows can repeat a TaxNameId, so
       the option value is the row's position and the percent is read from THAT row, SelectedRow.Cells[2]). */
    function taxTypeId() { var t = look.taxTypes.find(function (x) { return x._k === $id('CmbTaxType').value; }); return t ? intOf(t.TaxNameId) : 0; }
    function taxTypeValue(id) {
        var o = look.taxTypes.find(function (x) { return intOf(x.TaxNameId) === intOf(id); });
        $id('CmbTaxType').value = o ? o._k : '';
    }

    // ------------------------------------------------------------------ calculations

    function sum(list, key) { return list.reduce(function (s, r) { return s + num(r[key]); }, 0); }
    function gridItemNet() {                                                    // GridItemNetAmountCalculation:3834
        if (grid.length <= 0) return;
        var cfg = look.creditAmountInItemSaleGL;
        try {
            grid.forEach(function (r) {
                var itemAmount = roundAway(num(r.ItemAmount), amtRound()), expense = roundAway(num(r.Expense), amtRound());
                if (cfg !== null && cfg !== undefined) {
                    var t = String(cfg).trim().toLowerCase();
                    if (t !== 'true' && t !== 'false') throw new Error('String was not recognized as a valid Boolean.');
                    r.BillAmount = t === 'false' ? itemAmount + num(r.TaxAmount) : roundAway(itemAmount + num(r.TaxAmount), amtRound());
                } else r.BillAmount = roundAway(itemAmount + num(r.TaxAmount) + expense, amtRound());
            });
        } catch (e) { alert(e.message); }
    }
    function expProportion() {                                                  // ExpProportion:3813
        var total = sum(exp, 'Amount'), qty = sum(grid, 'ItemQty');
        grid.forEach(function (r) { r.Expense = roundAway(total / qty * num(r.ItemQty), amtRound()); });
        gridItemNet();
        renderGrid();
    }
    function freightProportion() {                                              // FreightProportion:3780
        var total = sum(fr, 'Freight');
        if (total > 0) {
            var qty = sum(grid, 'ItemQty');
            grid.forEach(function (r) { r.Freight = roundAway(total / qty * num(r.ItemQty), amtRound()); });
            gridItemNet();
        } else grid.forEach(function (r) { r.Freight = 0; });
        renderGrid();
    }
    function exRateChanged() {                                                  // txtExchangeRate_TextChanged:1309
        var rate = num($id('txtExchangeRate').value);
        if (grid.length > 0 && rate > 0) grid.forEach(function (r) { r.FcyAmount = num(r.ItemAmount) / rate; });
        else grid.forEach(function (r) { r.FcyAmount = 0; });
        billAmount();
    }
    function billAmount() {                                                     // BillAmount:3720
        if (grid.length > 0) {
            var totalQty = sum(grid, 'ItemQty'), item = sum(grid, 'ItemAmount'), tax = sum(grid, 'TaxAmount'), e = sum(exp, 'Amount');
            var jd = 0, jc = 0, freight = 0, supGl = intOf($id('txtSupplierGLId').value), sup = val('CmbSupplierName');
            gl.forEach(function (r) { if (intOf(r.AccountId) > 0) { jd += num(r.Debit); jc += num(r.Credit); } });
            fr.forEach(function (r) {
                if (!look.subsidiary) { if (intOf(r.GlAccountId) > 0 && supGl > 0 && supGl === intOf(r.GlAccountId)) freight += num(r.Freight); }
                else if (intOf(r.Transporter) > 0 && sup > 0 && sup === intOf(r.Transporter)) freight += num(r.Freight);
            });
            var bill = item + tax + e - freight;
            bill = bill + jc - jd;
            $id('txtBillAmount').value = fmtAmount(roundAway(bill, amtRound()));
            $id('txtInvoiceQty').value = fmtOpt(roundEven(totalQty, 2), 3);
            $id('txtFcyAmount').value = fmtFcy(bill / num($id('txtExchangeRate').value));
        } else { $id('txtInvoiceQty').value = '0'; $id('txtFcyAmount').value = '0'; }
        gridItemNet();
        renderGrid();
    }

    // ------------------------------------------------------------------ detail grid

    function taxName(id) { var t = look.taxTypes.find(function (x) { return intOf(x.TaxNameId) === intOf(id); }); return t ? t.TaxName : (intOf(id) ? String(id) : ''); }
    function reasonName(id) { var t = (look.reasons || []).find(function (x) { return intOf(x.ReasonId) === intOf(id); }); return t ? t.Reason : ''; }
    /* grdSettings:1500 — the column list, captions and editability depend on the form's flags. */
    function detailColumns(flags) {
        var cols = [['ItemCode', 'ItemCode'], ['Item', 'Item'], ['Warehouse', 'Warehouse'], ['RackName', 'RackName'], ['ItemCondition', 'ItemCondition'],
            ['PackUOM', 'UOM'], ['ItemQty', flags.ref ? 'Return Qty' : 'Item Qty']];
        if (flags.ref) cols.push(['BalQty', 'BalQty']);
        cols.push(['Rate', 'Rate'], ['ItemAmount', 'ItemAmount']);
        if (look.multiCurrency) cols.push(['FcyAmount', 'FcyAmount']);
        cols.push(['TaxTypeId', 'TaxTypeId'], ['TaxPercent', 'Tax%'], ['TaxAmount', 'TaxAmount'], ['BillAmount', 'Item Net Amount'], ['Expense', 'Expense'],
            ['Journal', 'Journal'], ['Freight', 'Freight'], ['GpDate', 'GpDate'], ['GpNo', 'GpNo'], ['VehicleNo', 'VehicleNo'], ['CityName', 'CityName'], ['Remarks', 'Remarks']);
        if (flags.ref) cols.push(['ReasonId', 'ReasonId']);
        return cols;
    }
    function editable(col) {
        if (gdnExist) return col === 'Rate' || col === 'TaxTypeId' || (col === 'TaxPercent' && look.taxPercentEditable);
        if (refDocumentExist) return ['ItemQty', 'TaxTypeId', 'GpDate', 'GpNo', 'ReasonId', 'VehicleNo', 'Remarks'].indexOf(col) >= 0 || (col === 'TaxPercent' && look.taxPercentEditable);
        return col === 'GpDate' || col === 'GpNo' || col === 'VehicleNo';
    }
    function cellText(r, c) {
        var v = r[c];
        if (c === 'TaxTypeId') return taxName(v);
        if (c === 'ReasonId') return reasonName(v);
        if (c === 'GpDate') return v ? C.gridDate(v) : '';
        if (typeof v === 'number') return c === 'ItemQty' || c === 'BalQty' ? fmtOpt(v, 3) : fmtOpt(v, 3);
        return v === null || v === undefined ? '' : v;
    }
    function renderGrid() {
        var t = $id('grd'), cols = detailColumns({ ref: refDocumentExist });
        t.tHead.innerHTML = '<tr>' + (gdnExist ? '' : '<th>X</th>') + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = grid.map(function (r, i) {
            return '<tr ondblclick="PiReturnStore.rowDoubleClick(' + i + ')">' + (gdnExist ? '' : '<td><button type="button" class="cx-x" onclick="PiReturnStore.delRow(' + i + ')">X</button></td>') +
                cols.map(function (c) {
                    var k = c[0];
                    if (!editable(k)) return '<td class="' + (typeof r[k] === 'number' ? 'num' : '') + '">' + esc(cellText(r, k)) + '</td>';
                    if (k === 'TaxTypeId') {
                        return '<td><select class="win-combo" onchange="PiReturnStore.cell(' + i + ',\'TaxTypeId\',this.value)"><option value="0"></option>' +
                            look.taxTypes.map(function (x) { return '<option value="' + esc(x.TaxNameId) + '"' + (intOf(x.TaxNameId) === intOf(r.TaxTypeId) ? ' selected' : '') + '>' + esc(x.TaxName) + '</option>'; }).join('') + '</select></td>';
                    }
                    if (k === 'ReasonId') {
                        return '<td><select class="win-combo" onchange="PiReturnStore.cell(' + i + ',\'ReasonId\',this.value)"><option value="0"></option>' +
                            (look.reasons || []).map(function (x) { return '<option value="' + esc(x.ReasonId) + '"' + (intOf(x.ReasonId) === intOf(r.ReasonId) ? ' selected' : '') + '>' + esc(x.Reason) + '</option>'; }).join('') + '</select></td>';
                    }
                    if (k === 'GpDate') return '<td><input type="date" value="' + esc(C.isoDay(r.GpDate)) + '" onchange="PiReturnStore.cell(' + i + ',\'GpDate\',this.value)"></td>';
                    var numeric = ['ItemQty', 'Rate', 'TaxPercent', 'GpNo'].indexOf(k) >= 0;
                    return '<td><input type="text" class="' + (numeric ? 'num' : '') + '" value="' + esc(r[k] === null || r[k] === undefined ? '' : r[k]) +
                        '" onchange="PiReturnStore.cell(' + i + ',\'' + k + '\',this.value)"></td>';
                }).join('') + '</tr>';
        }).join('');
    }
    function bindGrid() {                                                       // BindGrid:1354 — DetailGridComboBind re-reads the reasons
        renderGrid();
        return C.getJson(api + '/reasons').then(function (r) { look.reasons = r || []; renderGrid(); }).catch(function (e) { alert(e.message); });
    }
    function cell(i, col, value) {                                              // grd_CellUpdated:1677
        var r = grid[i]; if (!r) return;
        if (col === 'GpDate') r.GpDate = value || null;
        else if (col === 'VehicleNo' || col === 'Remarks') r[col] = value;
        else if (col === 'GpNo') r.GpNo = intOf(value);
        else r[col] = num(value);
        var numCol = (gdnExist && (col === 'ItemQty' || col === 'Rate')) || (refDocumentExist && col === 'ItemQty');
        if (numCol) {
            if (refDocumentExist && col === 'ItemQty' && num(r.ItemQty) > num(r.BalQty)) {       // ValidateQtyAgainstBalance:1730
                alert("Item Quantity can't be greater than Balance Quantity...");
                r.ItemQty = num(r.BalQty);
            }
            var q = num(r.ItemQty), rt = num(r.Rate);
            r.ItemAmount = q > 0 && rt > 0 ? q * rt : 0;                         // CalculateItemAmount:1741
        }
        if (gdnExist || refDocumentExist) {
            if (col === 'TaxTypeId') r.TaxPercent = taxPercentFor(r);
            else if (col === 'TaxPercent' && num(r.TaxPercent) === 0) r.TaxPercent = taxPercentFor(r);
            recalcTax(r);
            expProportion(); freightProportion(); exRateChanged();
        }
        renderGrid();
    }
    function taxPercentFor(r) {                                                 // GetTaxPercentForSelectedType:1752
        var id = intOf(r.TaxTypeId);
        if (id === 0) return 0;
        var m = look.taxTypes.find(function (x) { return intOf(x.TaxNameId) === id; });
        return m ? num(m.TaxPercent) : num(r.TaxPercent);
    }
    function recalcTax(r) {                                                     // RecalculateTax:1767
        var t = intOf(r.TaxTypeId), p = num(r.TaxPercent), a = num(r.ItemAmount);
        if (p > 100) { alert("Tax % Can't be greater than 100..."); p = 100; r.TaxPercent = p; }
        if (a > 0 && p > 0 && t > 0) r.TaxAmount = roundAway(a * p / 100, amtRound());
        else { r.TaxPercent = 0; r.TaxAmount = 0; }
    }
    function delRow(i) {                                                        // grd_ColumnButtonClick:1656
        if (gdnExist || !grid[i]) return;
        if (!confirm('Are you sure to Delete?')) return;
        grid.splice(i, 1);
        freightProportion(); expProportion(); billAmount();                     // grd_RecordsDeleted
        $id('CmbSupplierName').disabled = !(grid.length <= 0);
        freightProportion(); expProportion(); exRateChanged();
    }

    // ------------------------------------------------------------------ Other Items / Party Add-Less / Charge To Product

    function blankExp() { return { Id: 0, ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }
    function blankGl() { return { AccountId: 0, GlAccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }; }
    function blankFr() { return { Id: 0, GdnId: 0, Transporter: 0, GlAccountId: 0, Freight: 0, Debit: 0, Remarks: '' }; }
    function opts(list, v, t, cur) {
        return '<option value="0"></option>' + (list || []).map(function (x) {
            return '<option value="' + esc(x[v]) + '"' + (intOf(x[v]) === intOf(cur) ? ' selected' : '') + '>' + esc(x[t]) + '</option>';
        }).join('');
    }
    function inp(grid, i, k, v, isNum) {
        return '<td><input type="text" class="' + (isNum ? 'num' : '') + '" value="' + esc(v === null || v === undefined ? '' : v) +
            '" onchange="PiReturnStore.' + grid + '(' + i + ',\'' + k + '\',this)"></td>';
    }
    function tfoot(t, cells) { t.tFoot.innerHTML = '<tr>' + cells.map(function (c) { return '<td class="num">' + esc(c) + '</td>'; }).join('') + '</tr>'; }

    function renderExp() {                                                      // grdInvExpSettings:1796 — X, +, Item, Qty, Rate, Amount, Remarks
        var t = $id('grdInvExp');
        t.tHead.innerHTML = '<tr><th>X</th><th>+</th><th>Item</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th></tr>';
        t.tBodies[0].innerHTML = exp.map(function (r, i) {
            return '<tr><td><button type="button" class="cx-x" onclick="PiReturnStore.expButton(' + i + ',\'Delete\')">X</button></td>' +
                '<td><button type="button" class="cx-link" onclick="PiReturnStore.expButton(' + i + ',\'Add\')">+</button></td>' +
                '<td><select class="win-combo" onchange="PiReturnStore.expCell(' + i + ',\'ItemId\',this)">' + opts(look.otherItems, 'Id', 'OtherItemName', r.ItemId) + '</select></td>' +
                inp('expCell', i, 'Qty', fmtOpt(r.Qty, 2), true) + inp('expCell', i, 'Rate', fmtRate(r.Rate), true) + inp('expCell', i, 'Amount', fmtAmount(r.Amount), true) +
                inp('expCell', i, 'Remarks', r.Remarks, false) + '</tr>';
        }).join('');
        tfoot(t, ['', '', '', fmtOpt(sum(exp, 'Qty'), 2), '', fmtAmount(sum(exp, 'Amount')), '']);
    }
    function numericOk(el) {                                                    // *_UpdatingCell: "Please Type Only Numeric Value"
        var v = String(el.value).replace(/,/g, '').trim();
        if (v !== '' && isNaN(Number(v))) { alert('Please Type Only Numeric Value'); return false; }
        return true;
    }
    function expCell(i, k, el) {                                                // grdInvExp_CellUpdated:1880
        var r = exp[i]; if (!r) return;
        if (k === 'Qty' || k === 'Rate' || k === 'Amount') { if (!numericOk(el)) { renderExp(); return; } r[k] = num(el.value); }
        else if (k === 'ItemId') r.ItemId = intOf(el.value);
        else r[k] = el.value;
        if ((k === 'Qty' || k === 'Rate') && String(r.Qty) !== '' && String(r.Rate) !== '') r.Amount = roundAway(num(r.Qty) * num(r.Rate), amtRound());
        if (k === 'Amount') { r.Qty = 0; r.Rate = 0; r.Amount = roundAway(num(r.Amount), amtRound()); }
        expProportion(); billAmount(); renderExp();
    }
    function expButton(i, key) {                                                // grdInvExp_ColumnButtonClick:1843
        if (key === 'Delete') {
            if (!confirm('Are you sure to Delete?')) return;
            exp.splice(i, 1);
            if (exp.length === 0) exp.push(blankExp());
        }
        if (key === 'Add') exp.push(blankExp());
        expProportion(); billAmount(); renderExp();
    }

    function renderGl() {                                                       // gridGLSettings:2009 — +, X, Account, Remarks, Percentage, Qty, Rate, Debit, Credit
        var t = $id('grdGLedger'), vm = look.subsidiary ? 'SupplierCustomerId' : 'Id';
        t.tHead.innerHTML = '<tr><th>+</th><th>X</th><th>Account</th><th>Remarks</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Debit</th><th>Credit</th></tr>';
        t.tBodies[0].innerHTML = gl.map(function (r, i) {
            return '<tr><td><button type="button" class="cx-link" onclick="PiReturnStore.glButton(' + i + ',\'Add\')">+</button></td>' +
                '<td><button type="button" class="cx-x" onclick="PiReturnStore.glButton(' + i + ',\'Delete\')">X</button></td>' +
                '<td><select class="win-combo" onchange="PiReturnStore.glCell(' + i + ',\'AccountId\',this)">' + opts(look.accounts, vm, 'AccountTitle', r.AccountId) + '</select></td>' +
                inp('glCell', i, 'Remarks', r.Remarks, false) + inp('glCell', i, 'Percentage', clr(r.Percentage), true) + inp('glCell', i, 'Qty', clr(r.Qty), true) +
                inp('glCell', i, 'Rate', clr(r.Rate), true) + inp('glCell', i, 'Debit', clr(r.Debit), true) + inp('glCell', i, 'Credit', clr(r.Credit), true) + '</tr>';
        }).join('');
        tfoot(t, ['', '', '', '', '', '', '', clr(sum(gl, 'Debit')), clr(sum(gl, 'Credit'))]);
    }
    function glCell(i, k, el) {                                                 // grdGLedger_CellUpdated:2087
        var r = gl[i]; if (!r) return;
        if (['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'].indexOf(k) >= 0) { if (!numericOk(el)) { renderGl(); return; } r[k] = num(el.value); }
        else if (k === 'AccountId') r.AccountId = intOf(el.value);
        else r[k] = el.value;
        if (k === 'Qty' || k === 'Rate') { r.Credit = roundAway(num(r.Qty) * num(r.Rate), amtRound()); r.Debit = 0; r.Percentage = 0; }
        if (k === 'Percentage') {
            var tp = sum(grid, 'ItemAmount') / 100 * num(r.Percentage);
            if (tp > 0) { r.Credit = roundAway(tp, amtRound()); r.Debit = 0; }
            else { r.Debit = roundAway(Math.abs(tp), amtRound()); r.Credit = 0; }
            r.Qty = 0; r.Rate = 0;
        }
        if (k === 'Credit') { r.Credit = roundAway(num(r.Credit), amtRound()); if (num(r.Debit) > 0) { r.Credit = 0; alert('Debit Side is aleady added'); } }
        if (k === 'Debit') { r.Debit = roundAway(num(r.Debit), amtRound()); if (num(r.Credit) > 0) { r.Debit = 0; alert('Credit Side is aleady added'); } }
        if (k === 'AccountId') glAccountSelected(r, el);
        billAmount(); renderGl();
    }
    function glAccountSelected(r, el) {                                         // SupCustIdUpdateforGLGrid:1953
        var title = el.selectedOptions && el.selectedOptions[0] ? el.selectedOptions[0].textContent : '';
        if (title !== '' && title !== '0') {
            var m = look.accounts.find(function (a) { return a.AccountTitle === title; });
            if (m) r.GlAccountId = intOf(m.Id);
        }
        var supGl = intOf($id('txtSupplierGLId').value), sup = val('CmbSupplierName');
        if (!look.subsidiary) {
            if (intOf(r.GlAccountId) > 0 && supGl > 0 && supGl === intOf(r.GlAccountId)) {
                alert('Selected Account Can not be Same As Supplier Account'); r.GlAccountId = 0; r.AccountId = 0;
            }
        } else if (intOf(r.AccountId) > 0 && sup > 0 && sup === intOf(r.AccountId)) {
            alert('Selected Account Can not be Same As Supplier Account'); r.AccountId = 0; r.GlAccountId = 0;
        }
    }
    function glButton(i, key) {                                                 // grdGLedger_ColumnButtonClick:2056 — no confirmation
        if (key === 'Delete') { gl.splice(i, 1); if (gl.length === 0) gl.push(blankGl()); }
        if (key === 'Add') gl.push(blankGl());
        billAmount(); renderGl();
    }

    function renderFr() {                                                       // grdFreightSettings:2204 — +, X, Transporter, Credit, [Debit], Remarks
        var t = $id('grdFreight'), vm = look.subsidiary ? 'SupplierCustomerId' : 'Id', deb = !!look.freightDebitToExpenses;
        t.tHead.innerHTML = '<tr><th>+</th><th>X</th><th>Transporter</th><th>Credit</th>' + (deb ? '<th>Debit</th>' : '') + '<th>Remarks</th></tr>';
        t.tBodies[0].innerHTML = fr.map(function (r, i) {
            return '<tr><td><button type="button" class="cx-link" onclick="PiReturnStore.frButton(' + i + ',\'Add\')">+</button></td>' +
                '<td><button type="button" class="cx-x" onclick="PiReturnStore.frButton(' + i + ',\'Delete\')">X</button></td>' +
                '<td><select class="win-combo" onchange="PiReturnStore.frCell(' + i + ',\'Transporter\',this)">' + opts(look.freightAccounts, vm, 'AccountTitle', r.Transporter) + '</select></td>' +
                inp('frCell', i, 'Freight', fmtAmount(r.Freight), true) + (deb ? inp('frCell', i, 'Debit', fmtAmount(r.Debit), true) : '') +
                inp('frCell', i, 'Remarks', r.Remarks, false) + '</tr>';
        }).join('');
        tfoot(t, ['', '', '', fmtAmount(sum(fr, 'Freight'))].concat(deb ? [fmtAmount(sum(fr, 'Debit'))] : []).concat(['']));
    }
    function frCell(i, k, el) {                                                 // grdFreight_CellUpdated:2307
        var r = fr[i]; if (!r) return;
        if (k === 'Freight' || k === 'Debit') { if (!numericOk(el)) { renderFr(); return; } r[k] = num(el.value); }
        else if (k === 'Transporter') r.Transporter = intOf(el.value);
        else r[k] = el.value;
        if (k === 'Transporter') {                                              // SupCustIdUpdateforFreightGrid:2169
            var title = el.selectedOptions && el.selectedOptions[0] ? el.selectedOptions[0].textContent : '';
            if (title !== '' && title !== '0') { var m = look.freightAccounts.find(function (a) { return a.AccountTitle === title; }); if (m) r.GlAccountId = intOf(m.Id); }
        }
        if (k === 'Freight') r.Freight = roundAway(num(r.Freight), amtRound());
        if (look.freightDebitToExpenses) {
            if (k === 'Freight') { r.Freight = roundAway(num(r.Freight), amtRound()); if (num(r.Debit) > 0) { r.Freight = 0; alert('Debit Side is aleady added'); } }
            if (k === 'Debit') { r.Debit = roundAway(num(r.Debit), amtRound()); if (num(r.Freight) > 0) { r.Debit = 0; alert('Credit Side is aleady added'); } }
        }
        freightProportion(); billAmount(); renderFr();
    }
    function frButton(i, key) {                                                 // grdFreight_ColumnButtonClick:2270
        if (key === 'Delete') { fr.splice(i, 1); if (fr.length === 0) fr.push(blankFr()); }
        if (key === 'Add') {
            /* dtFreight.Rows.Add(0, Transporter, 0, Remarks) — positional: Id, GdnId, Transporter, GlAccountId. */
            var cur = fr[i], rem = cur ? cur.Remarks : null;
            if (rem !== null && rem !== undefined && !/^\s*-?\d+\s*$/.test(String(rem))) {
                alert('Input string was not in a correct format.Couldn\'t store <' + rem + '> in GlAccountId Column.  Expected type is Int32.');
                return;
            }
            fr.push({ Id: 0, GdnId: cur ? intOf(cur.Transporter) : 0, Transporter: 0, GlAccountId: rem === null || rem === undefined ? 0 : intOf(rem), Freight: 0, Debit: 0, Remarks: null });
        }
        billAmount(); renderFr();
    }
    function gridKeys(e, which) {                                               // Ctrl+D (grd*_KeyDown) — add a blank row
        if (!(e.ctrlKey && (e.key === 'd' || e.key === 'D'))) return;
        e.preventDefault();
        if (which === 'exp') { exp.push(blankExp()); exRateChanged(); renderExp(); }
        if (which === 'gl') { gl.push(blankGl()); billAmount(); renderGl(); }
        if (which === 'fr') { fr.push(blankFr()); freightProportion(); billAmount(); renderFr(); }
    }
    function renderAll() { renderGrid(); renderExp(); renderGl(); renderFr(); }

    // ------------------------------------------------------------------ save / update / delete

    function preValidate() {                                                    // Insert():2736-2846, before the confirmation
        if (grid.length === 0) return 'Detail Record Not Found';
        if (val('CmbSupplierName') === 0) return 'Customer field is required';
        if (!/^\s*[-+]?\d+\s*$/.test($id('txtdocno').value) || intOf($id('txtdocno').value) === 0) return 'Doc No must be a non-zero number';
        if (val('CmbPaymentTerm') === 0) return 'Payment Term field is required';
        if (!$id('combdeliverytrm').value || val('combdeliverytrm') === 0) return 'Delivery Term field is required';
        if (val('CmbPaymentTerm') === 2 && intOf($id('txtduedays').value) === 0) return 'Due Days Field is Required';
        var rate = $id('txtExchangeRate').value.trim(), fcy = $id('txtFcyAmount').value.trim();
        if (look.multiCurrency) {
            if (val('cmbCurrency') === 0) return 'Fcy Code Field is Required';
            if (rate === '' || rate === '0') return 'Exchange Rate Field is Required';
            if (fcy === '' || fcy === '0') return 'Fcy Amount Rate Field is Required';
        } else {
            if (val('cmbCurrency') === 0) return 'Please Configure Your Base Currency In configurations';
            if (rate === '' || rate === '0') return 'Please Configure Your Base Currency Rate In configurations';
        }
        if (grid.some(function (r) { return intOf(r.TaxTypeId) > 0; }) && val('CmbTaxAccount') === 0) return 'Tax Account field is required';
        var supGl = intOf($id('txtSupplierGLId').value), sup = val('CmbSupplierName');
        for (var k = 0; k < gl.length; k++) {
            var r = gl[k];
            if (!(num(r.Credit) > 0) && Math.round(num(r.Debit)) <= 0) continue;
            if (intOf(r.AccountId) === 0 && intOf(r.GlAccountId) === 0) return 'Please Select an Account Against JL First';
            if (!look.subsidiary) { if (intOf(r.GlAccountId) > 0 && supGl > 0 && supGl === intOf(r.GlAccountId)) return 'you cannot Select Account Same As Supplier Account in GL Grid Row no : ' + (k + 1); }
            else if (intOf(r.AccountId) > 0 && sup > 0 && sup === intOf(r.AccountId)) return 'you cannot Select Account Same As Supplier Account in GL Grid Row no : ' + (k + 1);
        }
        for (var f = 0; f < fr.length; f++) {
            if ((num(fr[f].Freight) > 0 || Math.round(num(fr[f].Debit)) > 0) && intOf(fr[f].GlAccountId) === 0 && intOf(fr[f].Transporter) === 0) return 'Please Select an Account Against Freight First';
        }
        for (var x = 0; x < exp.length; x++) if (num(exp[x].Amount) > 0 && intOf(exp[x].ItemId) === 0) return 'Please Select an Item Against Expense First';
        return null;
    }
    function payload() {
        return {
            Id: recId, DocDate: $id('DocDate').value, DueDate: dueDateValue, DueDaysText: $id('txtduedays').value,
            SupplierCustomerId: val('CmbSupplierName'), SupplierReferenceNo: $id('txtSupplierReference').value, ManualBillNo: $id('txtbillno').value,
            PaymentTermId: val('CmbPaymentTerm'), DeliveryTermId: val('combdeliverytrm'), DeliveryTerm: sel('combdeliverytrm'),
            RemarksHeader: $id('txtremarks').value, TaxAccountId: val('CmbTaxAccount'), BillAmountText: $id('txtBillAmount').value,
            CurrencyId: val('cmbCurrency'), ExchangeRateText: $id('txtExchangeRate').value, InvoiceQtyText: $id('txtInvoiceQty').value,
            FcyAmountText: $id('txtFcyAmount').value, SupplierGLIdText: $id('txtSupplierGLId').value,
            rows: grid.map(function (r) { var o = Object.assign({}, r); o.TaxTypeText = taxName(r.TaxTypeId); o.GpDate = r.GpDate ? C.isoDay(r.GpDate) : null; return o; }),
            expenses: exp, journals: gl, freights: fr
        };
    }
    function insert() {                                                         // Insert():2722
        var m = preValidate();
        if (m) { alert(m); return; }
        if (!confirm(recId === 0 ? 'Are you sure to Save' : 'Are you sure to Update')) return;
        var printVoucher = $id('ChkBok').checked, printSlip = $id('ChkPrintslip').checked;
        C.postJson(api + '/save', payload()).then(function (res) {
            alert(res.message);
            reset();
            if (printVoucher) printVoucherId(res.voucherHeadId);
            if (printSlip) printSlipId(res.id, '145A-PurchaseInvoiceReturn_StorePartySlip');
        }).catch(function (e) { alert(e.message); });
    }
    function save() { recId = 0; insert(); }                                    // saveToolStripButton_Click:3025
    function update() {                                                         // btnUpdate_Click:2646
        if (approved) { alert('Record Not Update beacause Record has approved'); return; }
        insert();
    }
    function del() {                                                            // btnDelete_Click:3131
        if (approved) { alert('Record Not Delete beacause Record has approved'); return; }
        if (!(recId > 0)) { alert('RecordId Not Found.....'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + recId + '/delete', {}).then(function () { alert('Delete Record Successfully'); reset(); })
            .catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------ print

    function printSlipId(id, title) {
        if (!(id > 0)) { alert('No Record Found For Display'); return; }
        C.printSlip(api + '/' + id + '/slip', title);
    }
    function printVoucherId(id) {
        if (!id) { alert('No Record Found For Display'); return; }
        C.printSlip(api + '/voucher-slip/' + id, '103-AcRptPurchaseSalesVoucherSlip');
    }

    // ------------------------------------------------------------------ open (ReadById:3038)

    function readById(id, withReset) {
        var p = withReset ? reset() : Promise.resolve();
        return p.then(function () { return C.getJson(api + '/' + id); }).then(function (h) {
            recId = h.Id;
            tab('tabForm');
            $id('txtdocno').value = h.DocNo;
            $id('DocDate').value = C.isoDay(h.DocDate); dueDateGenerate();
            setSupplier(h.SupplierCustomerId);
            $id('CmbSupplierName').disabled = true;
            $id('txtSupplierReference').value = h.SupplierReferenceNo || '';
            $id('txtbillno').value = h.ManualBillNo || '';
            setSelect('CmbPaymentTerm', h.PaymentTermId);
            setDueDays(String(h.SupplierInvoiceNo));
            dueDateValue = String(h.SupplierInvoiceDate || '').replace(' ', 'T');
            $id('duedate').value = C.isoDay(dueDateValue);
            var dt = Array.prototype.find.call($id('combdeliverytrm').options, function (o) { return o.textContent === h.DeliveryTerm; });
            $id('combdeliverytrm').value = dt ? dt.value : '';
            $id('txtremarks').value = h.OtherRemarks || '';
            $id('txtInvoiceQty').value = fmtOpt(h.InvoiceQty, 3);
            $id('txtFcyAmount').value = fmtFcy(h.FcyAmount);
            setSelect('CmbTaxAccount', h.TaxAccountId);
            $id('txtBillAmount').value = fmtAmount(h.BillAmount);
            approved = !!h.IsApproved;
            setSelect('cmbCurrency', h.CurrencyId);
            if (h.CurrencyId > 0) setRate(h.ExchangeRateText); else configBind();
            voucherHeadId = intOf(h.voucherHeadId);
            grid = (h.rows || []).map(function (r) { var o = Object.assign(blankRow(), r); return o; });
            exp = (h.expenses || []).slice(); if (!exp.length) exp.push(blankExp());
            gl = (h.journals || []).slice(); if (!gl.length) gl.push(blankGl());
            fr = (h.freights || []).slice(); if (!fr.length) fr.push(blankFr());
            refDocumentExist = grid.some(function (r) { return intOf(r.RefDocumentTypeId) > 0 && intOf(r.RefDocId) > 0 && intOf(r.RefDocSubId) > 0; });
            bindGrid();
            expProportion(); freightProportion(); billAmount(); exRateChanged();
            renderAll();
            $id('btnSave').classList.add('is-hidden'); $id('btnUpdate').classList.remove('is-hidden'); $id('btnDelete').classList.remove('is-hidden');
            $id('lblRecId').textContent = 'Record #' + recId;
        }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------ loader (frmPendingPurchaseInvoiceForReturn)

    function openLoader() {                                                     // BtnLoadSaleInvoice_Click:5013
        if (grid.length > 0) {
            if (intOf(grid[0].RefDocumentTypeId) === 0) { alert("You Can't Load Because direct Entry Already Exist"); return; }
            alert("You Can't Load other Invoice."); return;
        }
        /* a new dialog per click: fresh filters */
        loaderRows = []; loaderMaster = []; loaderChecked = {};
        $id('grdPi').tHead.innerHTML = ''; $id('grdPi').tBodies[0].innerHTML = '';
        $id('grdPiDetail').tHead.innerHTML = ''; $id('grdPiDetail').tBodies[0].innerHTML = '';
        C.getJson(api + '/loader').then(function (l) {
            loaderInfo = l;
            $id('piFromDate').value = l.fromDate || '';
            $id('piTodate').value = C.today();
            var name = l.userBranchName || '';
            var branches = l.branches || [];
            var hit = branches.some(function (b) { return b.BranchName === name; });
            $id('CmbBranch').innerHTML = branches.map(function (b, i) {
                var on = hit ? b.BranchName === name : (name === '' && i === 0);
                return '<label style="display:block"><input type="checkbox" value="' + esc(b.BranchId) + '"' + (on ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>';
            }).join('');
            C.openModal('dlgPi');
            searchLoader();
        }).catch(function (e) { alert(e.message); });
    }
    function searchLoader() {                                                   // PendingGdnLoad:182
        var ids = Array.prototype.filter.call($id('CmbBranch').querySelectorAll('input[type=checkbox]'), function (c) { return c.checked; })
            .map(function (c) { return c.value; });
        if (!ids.length) { alert('Select branch first'); return; }
        C.getJson(api + '/pending-invoices' + C.qs({ fromDate: $id('piFromDate').value, toDate: $id('piTodate').value, branchIds: ids.join(',') }))
            .then(function (rows) {
                loaderRows = rows || []; loaderChecked = {};
                var seen = {}; loaderMaster = [];
                loaderRows.forEach(function (r) { var id = intOf(ci(r, 'Id')); if (!seen[id]) { seen[id] = 1; loaderMaster.push(r); } });
                renderLoader();
                $id('grdPiDetail').tBodies[0].innerHTML = '';
            }).catch(function (e) { alert(e.message); });
    }
    function renderLoader() {                                                   // grdSettingsForGrnPending:262
        var showBranch = !(loaderInfo && loaderInfo.branchImplemented);
        var cols = (showBranch ? [['BranchName', 'BranchName']] : []).concat([['DocumentType', 'DocumentType'], ['DocNo', 'DocNo'], ['DocDate', 'DocDate'], ['SupplierName', 'SupplierName'],
            ['TermsDescription', 'PaymentTerm'], ['DueDays', 'DueDays'], ['DueDate', 'DueDate'], ['BillAmount', 'BillAmount'], ['EntryDate', 'EntryDate'],
            ['EntryUserName', 'EntryUser'], ['ModifyDate', 'ModifyDate'], ['ModifyUserName', 'ModifyUser'], ['IsApproved', 'ApprovalStatus'], ['ApprovedDate', 'ApprovedDate'],
            ['ApprovedUserName', 'ApprovedUser'], ['NoOfAttachments', 'NoOfAttachments']]);
        var t = $id('grdPi');
        if (!loaderMaster.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
        t.tHead.innerHTML = '<tr><th><input type="checkbox" onclick="PiReturnStore.checkAllLoader(this.checked)"></th>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = loaderMaster.map(function (r, i) {
            return '<tr onclick="PiReturnStore.loaderDetail(' + i + ',this)"><td><input type="checkbox"' + (loaderChecked[i] ? ' checked' : '') +
                ' onclick="event.stopPropagation();PiReturnStore.checkLoader(' + i + ',this.checked)"></td>' + cols.map(function (c) {
                    var v = ci(r, c[0]);
                    if (c[0] === 'DocDate' || c[0] === 'DueDate') v = C.gridDate(v);
                    if (c[0] === 'EntryDate' || c[0] === 'ModifyDate' || c[0] === 'ApprovedDate') v = C.gridDateTime(v, true);
                    if (c[0] === 'BillAmount') v = fmtAmount(v);
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
    }
    function checkLoader(i, on) { loaderChecked[i] = on; }
    function checkAllLoader(on) { loaderMaster.forEach(function (r, i) { loaderChecked[i] = on; }); renderLoader(); }
    function loaderDetail(i, tr) {                                              // grd_SelectionChanged:488 → DetailGridBind
        document.querySelectorAll('#grdPi tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        var id = intOf(ci(loaderMaster[i], 'Id'));
        var cols = [['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'], ['WareHouseName', 'WareHouseName'], ['RackName', 'RackName'], ['ItemCondition', 'ItemCondition'],
            ['JobLotCode', 'JobLot'], ['PackUomCode', 'Uom'], ['ItemQty', 'Qty'], ['ItemRate', 'Rate'], ['ItemAmount', 'ItemAmount'], ['UsedQty', 'UsedQty'], ['BalQty', 'BalQty'],
            ['UsedAmount', 'UsedAmount'], ['BalAmount', 'BalAmount'], ['GpNo', 'GpNo'], ['VehicleNo', 'VehicleNo']];
        var t = $id('grdPiDetail');
        t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + c[1] + '</th>'; }).join('') + '</tr>';
        t.tBodies[0].innerHTML = loaderRows.filter(function (r) { return intOf(ci(r, 'Id')) === id; }).map(function (r) {
            return '<tr>' + cols.map(function (c) { var v = ci(r, c[0]); return '<td' + (typeof v === 'number' ? ' class="num"' : '') + '>' + esc(typeof v === 'number' ? fmtOpt(v, 3) : v) + '</td>'; }).join('') + '</tr>';
        }).join('');
    }
    function loadChecked() {                                                    // BtnLoad:360
        var picked = loaderMaster.filter(function (r, i) { return loaderChecked[i]; });
        if (!picked.length) { alert('Chek the row first'); return; }
        var id = 0;
        for (var k = 0; k < picked.length; k++) {
            var d = intOf(ci(picked[k], 'Id'));
            if (d !== 0) { if (id === 0) id = d; if (id !== d) { alert('Sorry!. you can only select one invoice at a time'); return; } }
        }
        var rows = loaderRows.filter(function (r) { return intOf(ci(r, 'Id')) === id; });
        C.closeModal('dlgPi');
        loadInGrid(rows);
    }
    /* FormHelper.ResolveWarehouseRack:743 — only when the row lacks a warehouse or a rack. */
    function resolveWarehouseRack(o, itemId, parentCategoryId) {
        if (o.WarehouseId > 0 && o.RackId > 0) return;
        var cfg = intOf(parentCategoryId) === 2 ? intOf(look.defaultWarehouseForStoreFlow) : intOf(look.packingMaterialDefaultWarehouse);
        var itemRacks = look.racks.filter(function (x) { return x.ItemId === itemId; });
        if (!itemRacks.length) return;
        var res = C.resolveWarehouseAndRack(itemRacks, cfg);
        if (o.WarehouseId === 0 && res.WarehouseId > 0) { o.WarehouseId = res.WarehouseId; o.Warehouse = res.WarehouseName; }
        if (o.RackId === 0 && res.RackId > 0) { o.RackId = res.RackId; o.RackName = res.RackName; }
    }
    function loadInGrid(dt) {                                                   // LoadInGridDetail(DataTable):5051
        if (!dt || !dt.length) return;
        var first = dt[0];
        setSupplier(intOf(ci(first, 'SupplierCustomerId')));
        setSelect('CmbPaymentTerm', intOf(ci(first, 'PaymentTermId')));
        setDueDays(String(intOf(ci(first, 'DueDays'))));
        dueDateValue = ci(first, 'DueDate') ? String(ci(first, 'DueDate')).replace(' ', 'T') : '1900-01-01';
        $id('duedate').value = C.isoDay(dueDateValue);
        $id('CmbSupplierName').disabled = true;
        $id('txtremarks').value = ci(first, 'RemarksHeader') || '';
        refDocumentExist = true;
        var existing = {};
        grid.forEach(function (r) { existing[intOf(r.RefDocSubId)] = 1; });
        dt.forEach(function (dr) {
            var itemId = intOf(ci(dr, 'ItemId')), detailId = intOf(ci(dr, 'DetailId'));
            if (existing[detailId]) return;
            existing[detailId] = 1;
            var o = blankRow(), bal = num(ci(dr, 'BalQty')), rate = num(ci(dr, 'ItemRate')), pct = num(ci(dr, 'TaxPercent'));
            o.RefDocId = intOf(ci(dr, 'Id')); o.RefDocSubId = detailId; o.RefDocumentTypeId = intOf(ci(dr, 'DocumentTypeId'));
            o.ItemId = itemId; o.Item = itemId > 0 ? ci(dr, 'ItemName') : ''; o.ItemCode = itemId > 0 ? ci(dr, 'ItemCode') : '';
            o.WarehouseId = intOf(ci(dr, 'WarehouseId')); o.Warehouse = ci(dr, 'WareHouseName') || '';
            o.RackId = intOf(ci(dr, 'RackId')); o.RackName = ci(dr, 'RackName') || '';
            resolveWarehouseRack(o, itemId, ci(dr, 'ParentCategoryId'));
            o.ItemConditionId = intOf(ci(dr, 'ItemConditionId')); o.ItemCondition = ci(dr, 'ItemCondition') || '';
            o.JobLotId = intOf(ci(dr, 'JobLotId')); o.JobLot = ci(dr, 'JobLotCode') || '';
            o.PackUOMId = intOf(ci(dr, 'ItemUOMId')); o.PackUOM = ci(dr, 'PackUomCode') || ''; o.PackEquivalent = num(ci(dr, 'PackUomEquivalent'));
            o.ItemQty = bal; o.BalQty = bal; o.Rate = rate; o.ItemAmount = bal * rate;
            o.TaxTypeId = intOf(ci(dr, 'TaxNameId')); o.TaxPercent = pct; o.TaxAmount = bal * rate * pct / 100;
            o.CityId = intOf(ci(dr, 'CityId')); o.CityName = ci(dr, 'CityName') || '';
            o.GpNo = intOf(ci(dr, 'GpNo')); o.VehicleNo = ci(dr, 'VehicleNo') || '';
            o.GpDate = null; o.Remarks = null;
            grid.push(o);
        });
        bindGrid(); expProportion(); freightProportion(); exRateChanged();
    }

    // ------------------------------------------------------------------ history

    function historyDateMode() {
        if ($id('drdocdate').checked) return 'doc';
        if ($id('rdentrydate').checked) return 'entry';
        if ($id('rdmodifydate').checked) return 'modify';
        return 'approved';
    }
    function showHistory() {                                                    // FillHistoryGrid:3337
        C.getJson(api + '/history' + C.qs({
            dateMode: historyDateMode(),
            fromDate: $id('chkFromDateHistory').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDateHistory').checked ? $id('ToDateHistory').value : '',
            fromDocNo: intOf($id('FromDocNoHistory').value), toDocNo: intOf($id('ToDocNoHistory').value),
            supplierCustomerId: val('CmbCustomerHistory')
        })).then(function (list) {
            historyRows = list || [];
            var t = $id('grdHistory');
            $id('grdDetail').tHead.innerHTML = ''; $id('grdDetail').tBodies[0].innerHTML = '';
            if (!historyRows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; return; }
            t.tHead.innerHTML = '<tr><th>Edit</th><th>ItemSlip</th><th>PartySlip</th><th>Voucher</th><th>NoOfAttachments</th><th>DocNo</th><th>DocDate</th><th>ManualBillNo</th>' +
                '<th>CustomerName</th><th>PaymentTerm</th><th>DueDate</th><th>BillAmount</th><th>EntryUser</th><th>EntryDate</th><th>ModifyUser</th><th>ModifyDate</th><th>Remarks</th></tr>';
            t.tBodies[0].innerHTML = historyRows.map(function (h, i) {
                return '<tr onclick="PiReturnStore.historyDetail(' + i + ',this)" ondblclick="PiReturnStore.open(' + h.Id + ',false)">' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PiReturnStore.open(' + h.Id + ',true)">Edit</button></td>' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PiReturnStore.slipFor(' + h.Id + ',\'145-PurchaseInvoiceReturn_StoreItemSlip\')">ItemSlip</button></td>' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PiReturnStore.slipFor(' + h.Id + ',\'145A-PurchaseInvoiceReturn_StorePartySlip\')">PartySlip</button></td>' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();PiReturnStore.voucherFor(' + intOf(h.VoucherHeadId) + ')">Voucher</button></td>' +
                    '<td class="num">' + esc(h.NoOfAttachments) + '</td>' +
                    '<td class="cx-docno" onclick="event.stopPropagation();PiReturnStore.open(' + h.Id + ',false)">' + esc(h.DocNo) + '</td>' +
                    '<td>' + esc(C.gridDate(h.DocDate)) + '</td><td>' + esc(h.ManualBillNo) + '</td><td>' + esc(h.CustomerName) + '</td><td>' + esc(h.PaymentTerm) + '</td>' +
                    '<td>' + esc(C.gridDate(h.DueDate)) + '</td><td class="num">' + esc(fmtAmount(h.BillAmount)) + '</td><td>' + esc(h.EntryUser) + '</td>' +
                    '<td>' + esc(C.gridDateTime(h.EntryDate)) + '</td><td>' + esc(h.ModifyUser) + '</td><td>' + esc(C.gridDateTime(h.ModifyDate)) + '</td><td>' + esc(h.Remarks) + '</td></tr>';
            }).join('');
            t.tFoot.innerHTML = '<tr><td colspan="11"></td><td class="num">' + esc(fmtAmount(sum(historyRows, 'BillAmount'))) + '</td><td colspan="5"></td></tr>';
        }).catch(function (e) { alert(e.message); });
    }
    function historyDetail(i, tr) {                                             // grdHistory_SelectionChanged:3574 → GetDetailGrdByHeadId
        document.querySelectorAll('#grdHistory tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        C.getJson(api + '/' + historyRows[i].Id).then(function (h) {
            var t = $id('grdDetail'), rows = h.rows || [];
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; return; }
            /* grddetailhistorySettings:3614 — grdSettings with the CURRENT form's flags; TaxTypeId hidden, TaxType after it. */
            var cols = detailColumns({ ref: refDocumentExist }).map(function (c) { return c[0] === 'TaxTypeId' ? ['TaxType', 'TaxType'] : c; });
            t.tHead.innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
            t.tBodies[0].innerHTML = rows.map(function (r) {
                return '<tr>' + cols.map(function (c) { return '<td' + (typeof r[c[0]] === 'number' ? ' class="num"' : '') + '>' + esc(cellText(r, c[0])) + '</td>'; }).join('') + '</tr>';
            }).join('');
        }).catch(function (e) { alert(e.message); });
    }
    function setHistoryDates(days) {
        var f = new Date(); f.setDate(f.getDate() - days);
        $id('FromDateHistory').value = f.getFullYear() + '-' + String(f.getMonth() + 1).padStart(2, '0') + '-' + String(f.getDate()).padStart(2, '0');
        $id('ToDateHistory').value = C.today();
        $id('chkFromDateHistory').checked = true; $id('chkToDateHistory').checked = true;
    }
    function newHistory() {                                                     // ResetHistory:3255 — always 3 days
        setHistoryDates(3);
        $id('FromDocNoHistory').value = ''; $id('ToDocNoHistory').value = ''; $id('CmbCustomerHistory').value = '0';
        ['grdHistory', 'grdDetail'].forEach(function (id) { var t = $id(id); t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; if (t.tFoot) t.tFoot.innerHTML = ''; });
    }
    function refreshHistory() {                                                 // btnRefreshHistory_Click:3313 → HistoryCombosFill
        C.getJson(api + '/history-customers').then(function (r) { C.fillSelect('CmbCustomerHistory', r, 'Id', 'Customer'); })
            .catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------ Define Reasons (frmDefineReasons)

    function openReasons() {                                                    // btnDefineReason_Click:5127 (RefDocumentTypeId = 145)
        C.getJson(api + '/define-reasons').then(function (d) {
            C.fillSelect('CmbRefDocumentType', d.documentTypes, 'Id', 'Description');
            $id('CmbRefDocumentType').value = '145';
            renderReasons(d.history);
            reasonRecId = 0; $id('txtReasonName').value = '';
            $id('rsSave').classList.remove('is-hidden'); $id('rsUpdate').classList.add('is-hidden');
            C.openModal('dlgReasons');
        }).catch(function (e) { alert(e.message); });
    }
    function renderReasons(list) {
        var t = $id('grdDefineReasons');
        t.tHead.innerHTML = '<tr><th>RefDocumentType</th><th>ReasonName</th><th>EntryDate</th><th>EntryUser</th><th>ModifyDate</th><th>ModifyUser</th></tr>';
        t.tBodies[0].innerHTML = (list || []).map(function (r) {
            return '<tr ondblclick="PiReturnStore.reasonEdit(' + intOf(r.ReasonId) + ')"><td>' + esc(r.RefDocumentType) + '</td><td>' + esc(r.ReasonName) + '</td><td>' +
                esc(C.gridDateTime(r.EntryDate, true)) + '</td><td>' + esc(r.EntryUser) + '</td><td>' + esc(C.gridDateTime(r.ModifyDate, true)) + '</td><td>' + esc(r.ModifyUser) + '</td></tr>';
        }).join('');
    }
    function reasonEdit(id) {                                                   // grdDefineCity_DoubleClick:272
        $id('rsSave').classList.add('is-hidden'); $id('rsUpdate').classList.remove('is-hidden');
        reasonRecId = id;
        C.getJson(api + '/define-reasons/' + id).then(function (r) {
            $id('CmbRefDocumentType').value = String(r.RefDocumentTypeId); $id('txtReasonName').value = r.Reason || '';
        }).catch(function (e) { alert(e.message); });
    }
    function reasonNew() {                                                      // Reset:106 — RefDocumentTypeId stays 145
        C.getJson(api + '/define-reasons/history').then(renderReasons).catch(function (e) { alert(e.message); });
        $id('txtReasonName').value = ''; reasonRecId = 0;
        $id('rsSave').classList.remove('is-hidden'); $id('rsUpdate').classList.add('is-hidden');
    }
    function reasonSave(isUpdate) {                                             // btnsave_Click:208 / btnUpdate_Click:174
        if (!$id('CmbRefDocumentType').value || intOf($id('CmbRefDocumentType').value) === 0) { alert('Reference Document Type Field Required'); return; }
        if ($id('txtReasonName').value.trim() === '') { alert('Reason Name Field Required'); return; }
        C.postJson(api + '/define-reasons/save', { ReasonId: isUpdate ? reasonRecId : 0, RefDocumentTypeId: intOf($id('CmbRefDocumentType').value), Reason: $id('txtReasonName').value })
            .then(function (res) { alert(res.message); reasonNew(); })
            .catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------ tabs / load / reset / refresh

    function tab(id) {
        document.querySelectorAll('.win-tab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }
    function subTab(id) {
        document.querySelectorAll('.pir-subtab').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-sub') === id); });
        document.querySelectorAll('.pir-subpanel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }

    function bindCombos(l) {
        look.currencies = l.currencies; look.partiesAndItems = l.partiesAndItems || []; look.taxTypes = (l.taxTypes || []).map(function (t, i) { t._k = String(i + 1); return t; });
        look.racks = l.racks || []; look.uoms = l.uoms || []; look.otherItems = l.otherItems || []; look.accounts = l.accounts || [];
        look.freightAccounts = l.freightAccounts || []; look.reasons = l.reasons || [];
        var keepCur = val('cmbCurrency');                                       // CurrencyFill:1198
        C.fillSelect('cmbCurrency', l.currencies, 'Id', 'CurrencyCode'); setSelect('cmbCurrency', keepCur);
        supplierBind();
        var keepPt = val('CmbPaymentTerm');
        C.fillSelect('CmbPaymentTerm', l.paymentTerms, 'Id', 'Description'); setSelect('CmbPaymentTerm', keepPt);
        C.fillSelect('combdeliverytrm', [{ Id: 1, DeliveryTerm: 'Load' }, { Id: 2, DeliveryTerm: 'Ponch' }], 'Id', 'DeliveryTerm', false);
        $id('combdeliverytrm').value = '1';                                     // DeliveryTerm:744 Rows[0].Activate()
        var keepTa = val('CmbTaxAccount');
        C.fillSelect('CmbTaxAccount', l.taxAccounts, 'Id', 'AccountTitle'); setSelect('CmbTaxAccount', keepTa);
        var keepTt = $id('CmbTaxType').value;                                   // GetallTaxType:1051
        $id('CmbTaxType').innerHTML = '<option value=""></option>' + look.taxTypes.map(function (t) { return '<option value="' + t._k + '">' + esc(t.TaxName) + '</option>'; }).join('');
        var tt = look.taxTypes.find(function (t) { return t._k === keepTt; });
        $id('CmbTaxType').value = tt ? tt._k : '';
        itemdtFillByParty(); itemBind();
        var keepC = val('CmbItemCondition');
        C.fillSelect('CmbItemCondition', l.itemConditions, 'Id', 'Description'); setSelect('CmbItemCondition', keepC);
        var keepJ = val('CmbJobLot');
        C.fillSelect('CmbJobLot', l.jobLots, 'Id', 'Description'); setSelect('CmbJobLot', keepJ);
        var keepCi = val('CmbCityName');
        C.fillSelect('CmbCityName', l.cities, 'Id', 'Description'); setSelect('CmbCityName', keepCi);
    }
    function init() {                                                           // InvfrmPurchasedirectInvoice_Load:456
        return C.getJson(api + '/lookups').then(function (l) {
            ['rights', 'subsidiary', 'multiCurrency', 'formats', 'taxPercentEditable', 'freightDebitToExpenses', 'defaultWarehouseId', 'defaultCityId', 'defaultJobLotId',
                'baseCurrency', 'baseCurrencyRate', 'creditAmountInItemSaleGL', 'defaultWarehouseForStoreFlow', 'packingMaterialDefaultWarehouse'].forEach(function (k) { look[k] = l[k]; });
            var r = l.rights || {};
            $id('btnSave').disabled = !r.save;
            ['btnPrint', 'btnItemSlip', 'btnPartySlip'].forEach(function (id) { $id(id).disabled = !r.print; });
            $id('btnUpdate').disabled = !r.update; $id('btnDelete').disabled = !r.delete;
            if (!r.view) { $id('rightsNote').textContent = 'You do not have the View right for Purchase Invoice Return Store.'; $id('rightsNote').classList.remove('is-hidden'); }
            document.querySelectorAll('.pir-mc').forEach(function (e) { e.classList.toggle('is-hidden', !l.multiCurrency); });   // MultiCurrencyFeature:1236
            $id('txtdocno').value = l.docNo || '';
            bindCombos(l);
            C.fillSelect('CmbCustomerHistory', l.historyCustomers, 'Id', 'Customer');   // HistoryCombosFill — Load and history Refresh only
            exp = [blankExp()]; gl = [blankGl()]; fr = [blankFr()];             // AddRowInvExpGrid / AddRowInGLGrid / AddRowInFreightGrid
            configBind();
            var days = intOf(l.defaultDaysToLessFromHistoryFromDate);
            setHistoryDates(days > 0 ? days : 3);                               // Form_Load:588
            if (val('CmbPaymentTerm') === 1) { $id('txtduedays').value = ''; $id('txtduedays').disabled = true; }
            loaded = true;
            renderAll();
        }).catch(function (e) { alert(e.message); });
    }
    function reset() {                                                          // Reset:2485
        enableFields(true);
        gdnExist = false; refDocumentExist = false;
        recId = 0; voucherHeadId = 0; approved = false;
        setSupplier(0); $id('CmbSupplierName').disabled = false;
        $id('txtSupplierReference').value = ''; $id('txtbillno').value = ''; $id('txtInvoiceQty').value = '';
        setRate('');                                                            // TextChanged fires here with the old rows
        $id('txtremarks').value = ''; setDueDays('0');
        $id('CmbTaxAccount').value = '0'; $id('CmbItemName').value = '0'; $id('CmbPackUOM').value = '0';
        $id('txtQty').value = ''; $id('txtRate').value = ''; $id('txtAmount').value = '';
        exp = [blankExp()]; gl = [blankGl()]; grid = []; fr = [blankFr()];
        $id('lblRecId').textContent = '';
        return C.getJson(api + '/doc-no').then(function (l) {                   // DocumentNo():663
            $id('txtdocno').value = l.docNo || '';
            if ($id('ChkResetOnSave').checked) optionResetFields();
            $id('btnSave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden');
            showAddButtons(true);
            $id('txtBillAmount').value = '';
            $id('txtduedays').disabled = false;
            if (val('CmbPaymentTerm') === 1) { $id('txtduedays').value = ''; $id('txtduedays').disabled = true; }
            configBind();
            renderAll();
        }).catch(function (e) { alert(e.message); renderAll(); });
    }
    function refresh() {                                                        // btnRefresh_Click:2585
        return C.getJson(api + '/refresh').then(function (l) {
            bindCombos(l);
            configBind();
            renderAll();
        }).catch(function (e) { alert(e.message); });
    }

    window.PiReturnStore = {
        reset: reset, refresh: refresh, save: save, update: update, del: del, tab: tab, subTab: subTab, open: readById,
        printItemSlip: function () { printSlipId(recId, '145-PurchaseInvoiceReturn_StoreItemSlip'); },
        printPartySlip: function () { printSlipId(recId, '145A-PurchaseInvoiceReturn_StorePartySlip'); },
        printVoucher: function () { printVoucherId(voucherHeadId); },
        slipFor: printSlipId, voucherFor: printVoucherId,
        supplierChanged: supplierChanged, paymentTermLeave: paymentTermLeave, dueDaysChanged: dueDaysChanged, docDateChanged: docDateChanged,
        currencyLeave: currencyLeave, exchangeRateChanged: exchangeRateChanged, exchangeRateLeave: exchangeRateLeave,
        itemBind: itemBind, itemLeave: itemLeave, warehouseLeave: warehouseLeave, rackLeave: rackLeave, packUomLeave: packUomLeave, stock: stock,
        qtyRateChanged: qtyRateChanged, taxPercentChanged: taxPercentChanged, taxTypeLeave: taxTypeLeave,
        add: add, updateDetail: updateDetail, cancelDetail: cancelDetail, rowDoubleClick: rowDoubleClick, delRow: delRow, cell: cell,
        expCell: expCell, expButton: expButton, glCell: glCell, glButton: glButton, frCell: frCell, frButton: frButton,
        openLoader: openLoader, searchLoader: searchLoader, checkLoader: checkLoader, checkAllLoader: checkAllLoader, loaderDetail: loaderDetail, loadChecked: loadChecked,
        showHistory: showHistory, historyDetail: historyDetail, newHistory: newHistory, refreshHistory: refreshHistory,
        openReasons: openReasons, reasonEdit: reasonEdit, reasonNew: reasonNew, reasonSave: reasonSave
    };

    document.addEventListener('DOMContentLoaded', function () {
        $id('DocDate').value = C.today();                                       // designer: DateTime.Now, once
        $id('txtgpdate').value = C.today();
        dueDateGenerate();
        $id('grdInvExp').addEventListener('keydown', function (e) { gridKeys(e, 'exp'); });
        $id('grdGLedger').addEventListener('keydown', function (e) { gridKeys(e, 'gl'); });
        $id('grdFreight').addEventListener('keydown', function (e) { gridKeys(e, 'fr'); });
        init();
    });
})();
