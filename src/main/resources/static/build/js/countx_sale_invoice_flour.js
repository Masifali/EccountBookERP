/* ============================================================================================
 * Sale Invoice Direct (Flour) - screen 137, frmSaleInvoiceDirect, DocumentTypeId 186
 * Sale Invoice Direct Auto Rated - screen 138, frmSaleInvoiceDirectAutoRated, DocumentTypeId 184
 * One script for both pages: the page's #frm[data-doc] picks the variant (184 = auto rated).
 * Desktop: Architecture.WinApp.Sale.frmSaleInvoiceDirect (137) / frmSaleInvoiceDirectAutoRated (138)
 *   137 Load :429  PackUOM :784  AvailableStockGetByItem :974  ConfigurationDefault :1055  FormValidationDetila :1135  Reset :1236
 *       ResetDetail :1301  OptionResetFields :1331  combpttrm_Leave :1386  grdInvExp_* :1451-1570  btnAdd :1730  grd_DoubleClick :1756
 *       btnUpdateDetail :1802  grdSettings :1849  grd_CellUpdated :1964  Insert :2083  ReadById :2350  btnDelete :2443  BillAmount :2482
 *       ExpProportion :2517  BillProportion :2538  Total :2574  DetailAmountCaluculation :2604  GetAll :2893  GetDetailGrdByHeadId :3122
 *       KeyDown :3321  DueDateGenerate :3756  btnLoadSaleOrder :3910  LoadDataDetailfromPurchaseInvoivce :3939
 *   138 RateUOMFromSchedule :926  GetRateFromItemPriceScheduleByItemId :1213  UpdateRateFromItemPriceScheduleByInGrid :1270
 *       UpdateDiscountTypeAndRateInGrid :1395  btnAdd :2151  btnUpdateDetail :2229  grd_CellUpdated :2450  DetailDiscAmtCalculation :3265
 *       BtnPropotionateDiscApply_Click :4735
 * The server re-validates and recomputes everything Insert posts; nothing here is trusted for the amounts.
 * ============================================================================================ */
(function () {
    'use strict';

    var frm = document.getElementById('frm');
    var DOC = parseInt(frm.getAttribute('data-doc'), 10) || 186;
    var AUTO = DOC === 184;
    var api = '/sale/api/sale-invoice-flour/' + DOC;
    var L = {}, perms = {}, cfg = { amountDecimals: 0, rateDecimals: 2 };
    var dtGrid = [], dtExp = [], historyRows = [], uoms = [], rateUoms = [], LD = { rows: [], setup: null };
    var Id = 0, VoucherHeadId = 0, Approved = false, IsFromLoader = false, updateIndex = -1, selRow = -1, selHist = -1, tab = 0;

    function $id(x) { return document.getElementById(x); }
    function val(x) { var e = $id(x); return e ? e.value : ''; }
    function setVal(x, v) { var e = $id(x); if (e) e.value = (v === null || v === undefined) ? '' : v; }
    function say(m) { var e = $id('lblStatus'); if (e) e.textContent = m || ''; }
    function box(m) { window.alert(m); }
    function esc(s) { return String(s === null || s === undefined ? '' : s).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;'); }
    function num(v) { var n = parseFloat(String(v === null || v === undefined ? '' : v).replace(/,/g, '')); return isNaN(n) ? 0 : n; }
    function int(v) { var n = parseInt(String(v === null || v === undefined ? '' : v).replace(/,/g, ''), 10); return isNaN(n) ? 0 : n; }
    function col(row, name) {
        if (!row) return '';
        if (Object.prototype.hasOwnProperty.call(row, name)) return row[name];
        for (var k in row) if (Object.prototype.hasOwnProperty.call(row, k) && k.toLowerCase() === name.toLowerCase()) return row[k];
        return '';
    }
    function first(row, names) { for (var i = 0; i < names.length; i++) { var v = col(row, names[i]); if (v !== '' && v !== null && v !== undefined) return v; } return ''; }
    /* .NET Math.Round(x, n, MidpointRounding.AwayFromZero) */
    function away(x, n) {
        x = num(x); var f = Math.pow(10, n || 0), y = Math.abs(x) * f;
        return (x < 0 ? -1 : 1) * Math.floor(y + 0.5 + 1e-9) / f;
    }
    /* custom number format with thousands separators; zero shows as given by zeroText */
    function fmt(n, d, zeroText) {
        n = num(n);
        if (n === 0 && zeroText !== undefined) return zeroText;
        return away(n, d).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d || 0 });
    }
    function amt(n) { return fmt(n, cfg.amountDecimals, '0'); }          /* clsGlobalVariables.stringFormatsingle */
    function rateF(n) { return fmt(n, cfg.rateDecimals, '0'); }          /* clsGlobalVariables.DecimalRateFormate */
    function qtyF(n) { return fmt(n, 2, '0'); }                           /* "#,##0.##" */
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    function today() { return iso(new Date()); }
    function dateOnly(v) { if (!v) return ''; var m = String(v).match(/^(\d{4})-(\d{2})-(\d{2})/); if (m) return m[0]; var d = new Date(v); return isNaN(d.getTime()) ? '' : iso(d); }
    function shortDate(v) { var p = dateOnly(v); return p ? p.slice(8, 10) + '/' + p.slice(5, 7) + '/' + p.slice(0, 4) : ''; }
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
    function fill(x, rows, v, t) {
        var sel = $id(x); if (!sel) return;
        var html = '<option value=""></option>';
        (rows || []).forEach(function (r) { html += '<option value="' + esc(typeof v === 'function' ? v(r) : col(r, v)) + '">' + esc(typeof t === 'function' ? t(r) : col(r, t)) + '</option>'; });
        sel.innerHTML = html;
    }
    function text(x) { var s = $id(x); return s && s.selectedIndex >= 0 && s.options[s.selectedIndex] ? s.options[s.selectedIndex].text : ''; }
    function refreshCombos() { if (window.DesktopCombo && window.DesktopCombo.refresh) window.DesktopCombo.refresh(); }
    function show(x, on) { var e = $id(x); if (e) e.style.display = on ? '' : 'none'; }
    function focus(x) { var e = $id(x); if (e) try { e.focus(); } catch (z) { /* ignore */ } }
    function on(x, ev, fn) { var e = $id(x); if (e) e.addEventListener(ev, fn); }
    function custName(r) { return first(r, ['CompanyName', 'CustomerName', 'Name']); }

    // ------------------------------------------------------------------ load (Load :429)

    function bindLists() {
        fill('comsupplier', L.customers, 'Id', custName);
        fill('comItem', L.items, 'Id', $id('rdbtnItemCode') && $id('rdbtnItemCode').checked ? 'ItemCode' : 'ItemName');
        fill('combpttrm', L.dueTerms, 'Id', 'TermsDescription');
        if ((L.dueTerms || []).length > 1) setVal('combpttrm', col(L.dueTerms[1], 'Id'));                 /* Rows[1].Activate() */
        fill('combdeliverytrm', L.deliveryTerms, 'Id', 'DeliveryTerm');
        if ((L.deliveryTerms || []).length) setVal('combdeliverytrm', col(L.deliveryTerms[0], 'Id'));    /* Rows[0].Activate() */
        fill('comWarehouse', L.warehouses, 'Id', 'WareHouseName');
        fill('comCropYear', L.cropYears, 'Id', 'CropYear');
        fill('comPackingType', L.packingTypes, 'Id', 'PackTypeDesc');
        fill('txtCity', L.cities, 'Id', function (r) { return first(r, ['CityName', 'City', 'Name']); });
        if (AUTO) {
            fill('comjobLot', L.jobLots, 'Id', 'JobLotDescription');
            fill('CmbDiscountType', L.discountTypes, 'Id', 'DiscountType');
        }
        refreshCombos();
    }
    function configurationDefault() {                                                                    /* ConfigurationDefault :1055 */
        if (cfg.defaultCropYearId) setVal('comCropYear', cfg.defaultCropYearId);
        if (cfg.defaultPackingTypeId) setVal('comPackingType', cfg.defaultPackingTypeId);
        if (cfg.defaultWarehouseId) setVal('comWarehouse', cfg.defaultWarehouseId);
        if (!cfg.jobLotConfigured) box('JobLot config not configure properly pls check');
        else if (AUTO && cfg.defaultJobLotId) setVal('comjobLot', cfg.defaultJobLotId);
        refreshCombos();
    }
    function applyRights() {
        $id('btnSave').disabled = !perms.save;
        $id('btnUpdate').disabled = !perms.update;
        $id('btnPrint').disabled = !perms.print;
        if ($id('btnDelete')) $id('btnDelete').disabled = !perms.delete;
    }
    function init() {
        bindEvents();
        setVal('DocDate', today()); setVal('duedate', today()); setVal('txtduedays', '0');
        setVal('txtFromdateHistory', today()); setVal('txtToDateHistory', today());
        $id('ChkPrintslip').checked = false;
        $id('txtRate').disabled = AUTO;                                                                /* txtRate.Enabled = false in the 138 designer */
        http('GET', api + '/lookups').then(function (d) {
            L = d || {}; perms = L.rights || {}; cfg = L.configuration || cfg;
            setVal('txtdocno', L.docNo);
            resetExp(); bindLists(); applyRights(); renderAll(); configurationDefault(); resetDetailPacking();
            show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
            show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            dueTermChanged();
            focus('DocDate'); say('');
        }, function (e) { say(e.message); box(e.message); });
    }

    // ------------------------------------------------------------------ calculations

    function packEq() { var t = text('comPackUOM').trim(); return t === '' ? 0 : num(t); }
    function rateEq() {                                                                                 /* Cells[2] of the selected Rate UOM row */
        var id = int(val('comRateUOM')); if (!id) return 0;
        if (AUTO) { var r = rateUoms.filter(function (x) { return int(col(x, 'RateUomId')) === id; })[0]; return r ? num(col(r, 'Equivalent')) : 0; }
        var u = uoms.filter(function (x) { return int(col(x, 'Id')) === id; })[0]; return u ? num(col(u, 'Equivalent')) : 0;
    }
    function packEqOf(id) { var u = uoms.filter(function (x) { return int(col(x, 'Id')) === int(id); })[0]; return u ? num(col(u, 'Equivalent')) : 0; }

    /* Total() :2574 */
    function Total() {
        var p = packEq(), q = val('txtQty').trim() === '' ? 0 : num(val('txtQty'));
        setVal('txtNetBillWeight', p > 0 && q > 0 ? fmt(p * q, 2, '0') : '0');
    }
    /* DetailAmountCaluculation() :2604 */
    function DetailAmountCaluculation() {
        var ru = rateEq(), r = num(val('txtRate')), nw = num(val('txtNetBillWeight'));
        if (nw > 0 && ru > 0 && r > 0) setVal('txtAmount', amt(away(nw / ru * r, cfg.amountDecimals)));
        else setVal('txtAmount', '0');
    }
    /* DetailDiscAmtCalculation() :3265 (138) */
    function DetailDiscAmtCalculation() {
        if (!AUTO) return;
        var itemAmount = num(val('txtAmount')), discAmount = 0, rate = 0;
        if (int(val('CmbDiscountType')) !== 0) {
            rate = num(val('txtDiscountRate'));
            if (int(val('CmbDiscountType')) === 1) { discAmount = rate; setVal('txtDiscountAmount', amt(discAmount)); }
            else if (int(val('CmbDiscountType')) === 2) { discAmount = itemAmount * rate / 100; setVal('txtDiscountAmount', amt(discAmount)); }
        } else setVal('txtDiscountAmount', '0');
        setVal('txtTotalAmount', itemAmount > 0 && discAmount > 0 ? rateF(itemAmount - discAmount) : rateF(itemAmount));
    }
    function gridTotal(key) { return dtGrid.reduce(function (a, r) { return a + num(r[key]); }, 0); }
    function expTotal(key) { return dtExp.reduce(function (a, r) { return a + num(r[key]); }, 0); }
    /* BillAmount() :2482 */
    function BillAmount() {
        var item = away(gridTotal('ItemAmount'), cfg.amountDecimals), exp = away(expTotal('Amount'), cfg.amountDecimals);
        setVal('txtBillAmount', amt(away(item + exp, cfg.amountDecimals)));
    }
    /* ExpProportion() :2517 */
    function ExpProportion() {
        var totalExp = expTotal('Amount'), netWeight = gridTotal('NetBillWeight');
        dtGrid.forEach(function (r) { r.Expense = netWeight > 0 ? away(totalExp / netWeight * num(r.NetBillWeight), cfg.amountDecimals) : 0; });
        BillProportion();
    }
    /* BillProportion() :2538 - the CreditAmountInItemSaleGL configuration decides whether the expense is added */
    function BillProportion() {
        var c = String(cfg.creditAmountInItemSaleGL === undefined ? '' : cfg.creditAmountInItemSaleGL).trim();
        dtGrid.forEach(function (r) {
            var item = away(r.ItemAmount, cfg.amountDecimals), exp = away(r.Expense, cfg.amountDecimals);
            r.BillAmount = c !== '' ? item : away(item + exp, cfg.amountDecimals);
        });
        renderGrid();
    }
    function recalcTotals() { ExpProportion(); BillAmount(); }

    // ------------------------------------------------------------------ item / uom / stock / rate

    /* PackUOM() :784 */
    function PackUOM() {
        var item = int(val('comItem')), keepPack = text('comPackUOM'), keepRate = text('comRateUOM');
        setVal('comPackUOM', ''); setVal('comRateUOM', '');
        if (!item) { uoms = []; fill('comPackUOM', [], 'Id', 'Equivalent'); fill('comRateUOM', [], 'Id', 'Equivalent'); refreshCombos(); return Promise.resolve(); }
        return http('GET', api + '/uoms?itemId=' + item).then(function (rows) {
            uoms = rows || [];
            if (uoms.length > 0) {
                fill('comPackUOM', uoms, 'Id', 'Equivalent');
                if (!AUTO) fill('comRateUOM', uoms, 'Id', 'Equivalent');
                var hit = uoms.filter(function (x) { return String(col(x, 'Equivalent')) === keepPack; })[0];
                setVal('comPackUOM', hit ? col(hit, 'Id') : '');
                if (!AUTO) { var h2 = uoms.filter(function (x) { return String(col(x, 'Equivalent')) === keepRate; })[0]; setVal('comRateUOM', h2 ? col(h2, 'Id') : ''); }
            } else { fill('comPackUOM', [], 'Id', 'Equivalent'); if (!AUTO) fill('comRateUOM', [], 'Id', 'Equivalent'); }
            refreshCombos();
        }, function (e) { box(e.message); });
    }
    /* RateUOMFromSchedule() (138) :926 */
    function RateUOMFromSchedule() {
        if (!AUTO) return Promise.resolve();
        var item = int(val('comItem')), keep = text('comRateUOM');
        setVal('comRateUOM', '');
        if (!item) { rateUoms = []; fill('comRateUOM', [], 'RateUomId', 'UOMCode'); refreshCombos(); return Promise.resolve(); }
        return http('GET', api + '/rate-uoms?itemId=' + item + '&docDate=' + encodeURIComponent(val('DocDate'))).then(function (rows) {
            if (rows && rows.length > 0) {
                rateUoms = rows;
                fill('comRateUOM', rateUoms, 'RateUomId', 'UOMCode');
                var hit = rateUoms.filter(function (x) { return String(col(x, 'UOMCode')) === keep; })[0];
                setVal('comRateUOM', hit ? col(hit, 'RateUomId') : '');
            } else { rateUoms = []; fill('comRateUOM', [], 'RateUomId', 'UOMCode'); }
            refreshCombos();
        }, function (e) { box(e.message); });
    }
    /* GetRateFromItemPriceScheduleByItemId() (138) :1213 */
    function GetRateFromItemPriceScheduleByItemId() {
        if (!AUTO) return Promise.resolve();
        var item = int(val('comItem'));
        if (!item) { setVal('txtRate', '0'); return Promise.resolve(); }
        return http('GET', api + '/rate-uoms?itemId=' + item + '&docDate=' + encodeURIComponent(val('DocDate'))).then(function (rows) {
            if (!rows || rows.length === 0) { setVal('txtRate', '0'); return; }
            if (int(val('comRateUOM')) === 0) {
                var id = int(col(rows[0], 'RateUomId'));
                if (rateUoms.some(function (x) { return int(col(x, 'RateUomId')) === id; })) setVal('comRateUOM', id); else setVal('comRateUOM', '');
                refreshCombos();
            }
            return http('GET', api + '/rate?itemId=' + item + '&docDate=' + encodeURIComponent(val('DocDate')) + '&rateUomId=' + int(val('comRateUOM'))).then(function (d) {
                var r = num(d && d.rate);
                if (r > 0) { setVal('txtRate', String(r)); $id('txtRate').disabled = true; } else setVal('txtRate', '0');
                afterRate();
            });
        }, function (e) { box(e.message); });
    }
    function afterRate() { DetailAmountCaluculation(); DetailDiscAmtCalculation(); BillAmount(); }
    /* AvailableStockGetByItem() :974 */
    function AvailableStockGetByItem() {
        var item = int(val('comItem'));
        if (!item) { $id('lblBalance').textContent = '0'; return Promise.resolve(); }
        return http('GET', api + '/stock?itemId=' + item + '&docDate=' + encodeURIComponent(val('DocDate')) + '&warehouseId=' + int(val('comWarehouse'))
            + '&jobLotId=' + (AUTO ? int(val('comjobLot')) : 0) + '&cropYear=' + encodeURIComponent(text('comCropYear').trim()) + '&packingTypeId=' + int(val('comPackingType'))
            + '&stockUom=' + int(val('comPackUOM'))).then(function (d) {
            var s = num(d && d.stock);
            $id('lblBalance').textContent = s > 0 ? String(s) : '0';
        }, function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ entry events

    function itemLeave() {
        PackUOM().then(function () { return RateUOMFromSchedule(); }).then(AvailableStockGetByItem).then(GetRateFromItemPriceScheduleByItemId);
    }
    function qtyChanged() { Total(); DetailAmountCaluculation(); DetailDiscAmtCalculation(); BillAmount(); }
    function packUomLeave() { Total(); DetailAmountCaluculation(); DetailDiscAmtCalculation(); BillAmount(); AvailableStockGetByItem(); }
    function rateUomLeave() { GetRateFromItemPriceScheduleByItemId().then(afterRate); if (!AUTO) afterRate(); }
    function rateChanged() { afterRate(); }
    function discTypeLeave() { DetailAmountCaluculation(); DetailDiscAmtCalculation(); BillAmount(); }
    function discRateChanged() {
        if (int(val('CmbDiscountType')) === 2 && num(val('txtDiscountRate')) > 100) { box("The Discount Percent Can't Be Greater Than 100"); setVal('txtDiscountRate', '100'); }
        afterRate();
    }
    function dueDateGenerate() {                                                                         /* DueDateGenerate :3756 */
        var t = val('txtduedays').trim(), d = val('DocDate') ? new Date(val('DocDate') + 'T00:00:00') : new Date();
        if (t !== '') d.setDate(d.getDate() + num(t));
        setVal('duedate', iso(d));
    }
    function dueTermChanged() {                                                                          /* combpttrm_Leave :1386 */
        $id('txtduedays').disabled = false;
        if (int(val('combpttrm')) === 1) { setVal('txtduedays', ''); $id('txtduedays').disabled = true; dueDateGenerate(); }
    }
    function supplierChanged() { BillAmount(); }

    function needCombo(x, m) { if (val(x) === '' || int(val(x)) === 0) { box(m); focus(x); return false; } return true; }
    function needText(x, m) { var t = val(x).trim(); if (t === '' || t === '0') { box(m); focus(x); return false; } return true; }
    /* FormValidationDetila() :1135 */
    function FormValidationDetila() {
        if (!needCombo('comWarehouse', 'Warehouse Field is Required')) return false;
        if (!needCombo('comItem', 'Item Name Field is Required')) return false;
        if (!needCombo('comCropYear', 'Crop Year Field is Required')) return false;
        if (AUTO && !needCombo('comjobLot', 'Job/Lot Field is Required')) return false;
        if (!needCombo('comPackingType', 'Packing Type Field is Required')) return false;
        if (!needCombo('comPackUOM', 'Pack Unit Field is Required')) return false;
        if (!needText('txtQty', 'Qty Field is Required')) return false;
        if (!needText('txtNetBillWeight', 'Net Bill Weight Field is Required')) return false;
        if (!needText('txtRate', 'Rate Field is Required')) return false;
        if (!needCombo('comRateUOM', 'Rate UOM Field is Required')) return false;
        if (!needText('txtAmount', 'Item Amount Field is Required')) return false;
        if (AUTO && !needText('txtTotalAmount', 'Total Amount Field is Required')) return false;
        return true;
    }
    function itemRow() { return (L.items || []).filter(function (x) { return int(col(x, 'Id')) === int(val('comItem')); })[0] || {}; }
    function entryFields() {
        var it = itemRow();
        var o = {
            WarehouseId: int(val('comWarehouse')), Warehouse: text('comWarehouse'), ItemId: int(val('comItem')), Item: col(it, 'ItemName'), ItemCode: col(it, 'ItemCode'),
            CropYear: text('comCropYear'), JobLotId: AUTO ? int(val('comjobLot')) : 0, JobLot: AUTO ? text('comjobLot') : '',
            PackingTypeId: int(val('comPackingType')), PackingType: text('comPackingType'), PackUOMId: int(val('comPackUOM')), PackUOMEquivalent: packEq(),
            ItemQty: num(val('txtQty')), NetBillWeight: num(val('txtNetBillWeight')), Rate: num(val('txtRate')), RateUOMId: int(val('comRateUOM')), RateEquivalent: rateEq(),
            CityId: int(val('txtCity')), CityName: int(val('txtCity')) > 0 ? text('txtCity') : '', GpDate: today(), GpNo: 0, VehicleNo: ''
        };
        if (AUTO) {
            o.ItemAmountWithoutDisc = num(val('txtAmount')); o.DiscountTypeId = int(val('CmbDiscountType')); o.DiscountType = text('CmbDiscountType');
            o.DiscountRate = num(val('txtDiscountRate')); o.DiscountAmount = num(val('txtDiscountAmount')); o.ItemAmount = num(val('txtTotalAmount'));
        } else o.ItemAmount = num(val('txtAmount'));
        return o;
    }
    /* btnAdd_Click :1730 */
    function btnAdd_Click() {
        if (!FormValidationDetila()) return;
        if (dtGrid.length > 0 && IsFromLoader) { box('You can not Add Manual Record Becuase Record from Loader Exist in grd'); return; }
        var r = entryFields();
        r.Id = 0; r.RefDocumentTypeId = 0; r.RefDocIdNo = 0; r.RefDocSubIdNo = 0; r.BalQty = r.ItemQty; r.BalWeight = r.NetBillWeight; r.CgsRate = r.Rate;
        r.BillAmount = 0; r.Expense = 0; r.Journal = 0;
        if (!AUTO) { r.DiscountTypeId = 0; r.DiscountType = ''; r.DiscountRate = 0; r.DiscountAmount = 0; r.ItemAmountWithoutDisc = r.ItemAmount; }
        dtGrid.push(r);
        IsFromLoader = false;
        recalcTotals(); ResetDetail();
    }
    /* ResetDetail() :1301 */
    function ResetDetail() {
        ['comPackUOM', 'txtQty', 'txtNetBillWeight', 'txtRate', 'comRateUOM', 'txtAmount'].forEach(function (x) { setVal(x, ''); });
        if (AUTO) ['CmbDiscountType', 'txtDiscountRate', 'txtDiscountAmount', 'txtTotalAmount'].forEach(function (x) { setVal(x, ''); });
        if ($id('ChkResetOnSave').checked) OptionResetFields();
        resetDetailPacking();
        refreshCombos(); focus('comItem');
    }
    function resetDetailPacking() {
        var has = (L.packingTypes || []).some(function (x) { return int(col(x, 'Id')) === 2; });
        setVal('comPackingType', has ? 2 : ''); refreshCombos();
    }
    function OptionResetFields() {                                                                       /* OptionResetFields :1331 */
        ['comItem', 'comCropYear', 'comPackingType', 'comWarehouse', 'txtCity'].forEach(function (x) { setVal(x, ''); });
        refreshCombos(); focus('comItem');
    }
    /* grd_DoubleClick :1756 */
    function grdDoubleClick(i) {
        var r = dtGrid[i]; if (!r || IsFromLoader) return;
        updateIndex = i;
        setVal('comWarehouse', r.WarehouseId); setVal('comItem', r.ItemId);
        PackUOM().then(function () { return RateUOMFromSchedule(); }).then(function () {
            setVal('comCropYear', (L.cropYears || []).filter(function (c) { return String(col(c, 'CropYear')) === String(r.CropYear); }).map(function (c) { return col(c, 'Id'); })[0] || '');
            if (AUTO) setVal('comjobLot', r.JobLotId);
            setVal('comPackingType', r.PackingTypeId); setVal('txtQty', r.ItemQty); setVal('comPackUOM', r.PackUOMId);
            setVal('txtNetBillWeight', fmt(r.NetBillWeight, 2, '0')); setVal('txtRate', r.Rate); setVal('comRateUOM', r.RateUOMId);
            setVal('txtAmount', amt(AUTO ? r.ItemAmountWithoutDisc : r.ItemAmount)); setVal('txtCity', r.CityId || '');
            if (AUTO) { setVal('CmbDiscountType', r.DiscountTypeId || ''); setVal('txtDiscountRate', r.DiscountRate); setVal('txtDiscountAmount', amt(r.DiscountAmount)); setVal('txtTotalAmount', rateF(r.ItemAmount)); }
            refreshCombos();
            show('btnAdd', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
        });
    }
    /* btnUpdateDetail_Click */
    function btnUpdateDetail_Click() {
        if (!FormValidationDetila()) return;
        var r = dtGrid[updateIndex]; if (!r) return;
        var n = entryFields(); delete n.GpDate;
        for (var k in n) r[k] = n[k];
        if (!AUTO) r.ItemAmountWithoutDisc = r.ItemAmount;
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        recalcTotals();
    }
    function btnCancelUpdateDetial_Click() { show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); }

    // ------------------------------------------------------------------ grids

    function cols() {
        var c = [['Warehouse', 'Warehouse', 't'], ['Item', 'Item', 't'], ['ItemCode', 'ItemCode', 't'], ['CropYear', 'CropYear', 't']];
        if (AUTO) c.push(['JobLot', 'JobLot', 't']);
        c.push(['PackingType', 'PackingType', 't'], ['PackUOMEquivalent', 'Pack Uom', 'q'], ['ItemQty', 'ItemQty', 'q', 1], ['NetBillWeight', 'NetBillWeight', 'q', 1],
            ['Rate', 'Rate', 'r'], ['CgsRate', 'CgsRate', 'r'], ['RateEquivalent', 'Rate Uom', 'q']);
        if (AUTO) c.push(['ItemAmountWithoutDisc', 'Amount W-O Disc', 'a'], ['DiscountType', 'DiscountType', 't'], ['DiscountRate', 'DiscountRate', 'r'], ['DiscountAmount', 'DiscountAmount', 'a']);
        c.push(['ItemAmount', 'ItemAmount', 'a', 1], ['GpDate', 'GpDate', 'd'], ['GpNo', 'GpNo', 'i'], ['VehicleNo', 'VehicleNo', 't'], ['BillAmount', 'Item Net Amount', 'a', 1],
            ['Expense', 'Expense', 'a', 1], ['Journal', 'Journal', 'a', 1], ['CityName', 'CityName', 't']);
        return c;
    }
    var gridCols = cols();
    function fmtCell(kind, v) {
        if (kind === 'a') return amt(v);
        if (kind === 'r') return rateF(v);
        if (kind === 'q') return qtyF(v);
        if (kind === 'd') return shortDate(v);
        if (kind === 'i') return esc(v === 0 || v === '' ? '0' : v);
        return esc(v);
    }
    function isEditable(key) { return IsFromLoader && (key === 'ItemQty' || (AUTO && key === 'DiscountRate')); }
    function renderGrid() {
        $id('grdHead').innerHTML = '<th style="width:20px">X</th>' + gridCols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('');
        $id('grdBody').innerHTML = dtGrid.map(function (r, i) {
            return '<tr data-i="' + i + '" class="' + (i === selRow ? 'sel' : '') + '"><td class="ctr"><button type="button" class="bt" data-del="' + i + '" style="width:20px;height:17px;">X</button></td>'
                + gridCols.map(function (c) {
                    var n = c[2] !== 't' && c[2] !== 'd';
                    if (isEditable(c[0])) return '<td class="n"><input class="ce" data-f="' + c[0] + '" value="' + esc(r[c[0]]) + '"/></td>';
                    return '<td class="' + (n ? 'n' : '') + '">' + fmtCell(c[2], r[c[0]]) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $id('grdFoot').innerHTML = '<td></td>' + gridCols.map(function (c) {
            if (!c[3]) return '<td></td>';
            var t = gridTotal(c[0]);
            return '<td class="n">' + (c[2] === 'a' ? amt(t) : qtyF(t)) + '</td>';
        }).join('');
    }
    function resetExp() { dtExp = [{ ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }]; }       /* AddRowInvExpGrid :1445 */
    function renderExp() {
        $id('expHead').innerHTML = '<th style="width:56px">Delete</th><th style="width:48px">Add New</th><th style="width:250px">Item</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th>';
        var opts = '<option value=""></option>' + (L.otherItems || []).map(function (r) { return '<option value="' + esc(col(r, 'Id')) + '">' + esc(col(r, 'OtherItemName')) + '</option>'; }).join('');
        $id('expBody').innerHTML = dtExp.map(function (r, i) {
            return '<tr data-e="' + i + '"><td class="ctr"><button type="button" class="bt" data-edel="' + i + '" style="height:17px;">Delete</button></td>'
                + '<td class="ctr"><button type="button" class="bt" data-eadd="' + i + '" style="height:17px;">Add</button></td>'
                + '<td><select class="ct" data-f="ItemId" style="width:100%;height:17px;font-size:12px;">' + opts.replace('value="' + r.ItemId + '"', 'value="' + r.ItemId + '" selected') + '</select></td>'
                + '<td><input class="ce" data-f="Qty" value="' + esc(r.Qty ? qtyF(r.Qty) : (r.Qty === '' ? '' : '0')) + '"/></td>'
                + '<td><input class="ce" data-f="Rate" value="' + esc(r.Rate === '' ? '' : (r.Rate ? rateF(r.Rate) : '0')) + '"/></td>'
                + '<td><input class="ce" data-f="Amount" value="' + esc(r.Amount ? amt(r.Amount) : '0') + '"/></td>'
                + '<td><input class="ct" data-f="Remarks" value="' + esc(r.Remarks) + '"/></td></tr>';
        }).join('');
        $id('expFoot').innerHTML = '<td></td><td></td><td></td><td class="n">' + qtyF(expTotal('Qty')) + '</td><td></td><td class="n">' + amt(expTotal('Amount')) + '</td><td></td>';
    }
    function renderAll() { renderGrid(); renderExp(); }

    /* grdInvExp_CellUpdated :1525 */
    function expCell(i, f, v) {
        var r = dtExp[i]; if (!r) return;
        if (f === 'Remarks') { r.Remarks = v; return; }
        if (f === 'ItemId') { r.ItemId = int(v); BillAmount(); return; }
        if ((f === 'Qty' || f === 'Rate' || f === 'Amount') && v.trim() !== '' && isNaN(num(v))) { box('Please Type Only Numeric Value'); renderExp(); return; }
        r[f] = v.trim() === '' ? '' : num(v);
        if (f === 'Qty' || f === 'Rate') {
            if (String(r.Qty) !== '' && String(r.Rate) !== '') r.Amount = away(num(r.Qty) * num(r.Rate), cfg.amountDecimals);
        } else if (f === 'Amount') r.Amount = away(num(r.Amount), cfg.amountDecimals);
        BillAmount(); ExpProportion(); renderExp();
    }
    /* grd_CellUpdated :1964 / 2450 */
    function gridCell(i, f, v) {
        var r = dtGrid[i]; if (!r) return;
        if (!IsFromLoader) return;
        if (f === 'ItemQty') {
            var qty = num(v), netWt;
            if (qty > r.BalQty) { box('Item Cannot be greater than Balance Qty ' + r.BalQty + ' Of this row'); qty = r.BalQty; netWt = r.BalWeight; }
            else { netWt = num(r.PackUOMEquivalent) * qty; if (netWt > r.BalWeight) netWt = r.BalWeight; }
            r.ItemQty = qty; r.NetBillWeight = netWt;
            var amtW = netWt > 0 ? netWt / num(r.RateEquivalent) * num(r.Rate) : 0;
            if (!isFinite(amtW)) amtW = 0;
            if (AUTO) discountRow(r, amtW); else r.ItemAmount = amtW;
        } else if (f === 'DiscountRate' && AUTO) {
            r.DiscountRate = num(v); discountRow(r, num(r.ItemAmountWithoutDisc));
        }
        recalcTotals();
    }
    function discountRow(r, amtW) {                                                                       /* per-row discount block shared by the 138 handlers */
        r.ItemAmountWithoutDisc = amtW;
        var dis = 0;
        if (int(r.DiscountTypeId) !== 0) {
            if (int(r.DiscountTypeId) === 1) dis = num(r.DiscountRate);
            else if (int(r.DiscountTypeId) === 2) dis = amtW * num(r.DiscountRate) / 100;
            r.DiscountAmount = dis;
        } else { r.DiscountRate = 0; r.DiscountAmount = 0; }
        r.ItemAmount = amtW > 0 && dis > 0 ? amtW - dis : (amtW > 0 ? amtW : 0);
    }
    function gridDelete(i) {                                                                              /* grd_ColumnButtonClick :2050 */
        if (!window.confirm('Are you sure to Delete?')) return;
        dtGrid.splice(i, 1); selRow = -1;
        if (dtGrid.length === 0) IsFromLoader = false;
        recalcTotals();
    }

    // ------------------------------------------------------------------ 138 buttons

    /* UpdateRateFromItemPriceScheduleByInGrid() :1270 */
    function BtnRefreshRates_Click() {
        if (dtGrid.length <= 0) return;
        var jobs = dtGrid.map(function (r) {
            return http('GET', api + '/rate?itemId=' + r.ItemId + '&docDate=' + encodeURIComponent(val('DocDate')) + '&rateUomId=' + r.RateUOMId).then(function (d) {
                var rate = num(d && d.rate);
                if (rate > 0) {
                    r.Rate = rate;
                    var amtW = r.NetBillWeight > 0 ? num(r.NetBillWeight) / num(r.RateEquivalent) * rate : 0;
                    if (int(r.DiscountTypeId) === 2 && num(r.DiscountRate) >= 100) { box('DiscountRate Cannot be greater than 99 when Discount Type is Percent'); r.DiscountRate = 99; }
                    discountRow(r, amtW);
                } else r.Rate = 0;
            });
        });
        return Promise.all(jobs).then(recalcTotals, function (e) { box(e.message); });
    }
    /* UpdateDiscountTypeAndRateInGrid() :1395 */
    function btnUpdateDiscountInGrid_Click() {
        if (int(val('CmbDiscountType')) === 0) { box('DiscountType Required'); focus('CmbDiscountType'); return; }
        if (num(val('txtDiscountRate')) === 0) { box('Discount Rate Required'); focus('txtDiscountRate'); return; }
        if (dtGrid.length === 0) { box('Atlest one Record Required in Grid'); return; }
        var id = int(val('CmbDiscountType')), name = text('CmbDiscountType'), rate = num(val('txtDiscountRate'));
        if (id === 2 && rate >= 100) { box('Discount Rate Cannot be greater than 99 when Discount Type is Percent'); return; }
        dtGrid.forEach(function (r) { r.DiscountTypeId = id; r.DiscountType = name; r.DiscountRate = rate; discountRow(r, num(r.ItemAmountWithoutDisc)); });
        recalcTotals();
    }
    /* BtnPropotionateDiscApply_Click :4735 */
    function BtnPropotionateDiscApply_Click() {
        if (int(val('CmbDiscountType')) !== 1) { box('DiscountType Should Be Flat in Case of Porpotionate'); focus('CmbDiscountType'); return; }
        if (dtGrid.length === 0) return;
        var total = num(val('txtDiscountAmtSetForAllRows')), bill = num(val('txtBillAmount'));
        if (total >= bill) { box('Discount Amount Cannot be greater than or Equal to Bill Amount'); return; }
        var wt = gridTotal('NetBillWeight'), name = text('CmbDiscountType');
        dtGrid.forEach(function (r) {
            r.DiscountTypeId = 1; r.DiscountType = name;
            r.DiscountRate = away(total / wt * num(r.NetBillWeight), cfg.amountDecimals);
            discountRow(r, num(r.ItemAmountWithoutDisc));
        });
        recalcTotals();
    }

    // ------------------------------------------------------------------ save (Insert :2083)

    function collect() {
        return {
            Id: Id, DocNo: val('txtdocno'), DocDate: val('DocDate'), SupplierCustomerId: int(val('comsupplier')), ManualBillNo: val('txtbillno').trim(),
            DeliveryTerm: text('combdeliverytrm'), PaymentTermId: int(val('combpttrm')), DueDays: val('txtduedays'), DueDate: val('duedate'),
            RemarksHeader: val('txtremarks').trim(),
            details: dtGrid.map(function (r) {
                return { Id: r.Id, RefDocumentTypeId: r.RefDocumentTypeId, RefDocIdNo: r.RefDocIdNo, RefDocSubIdNo: r.RefDocSubIdNo, WarehouseId: r.WarehouseId, ItemId: r.ItemId,
                    CropYear: r.CropYear, JobLotId: r.JobLotId, PackingTypeId: r.PackingTypeId, PackUOMId: r.PackUOMId, RateUOMId: r.RateUOMId, ItemQty: r.ItemQty, Rate: r.Rate,
                    DiscountTypeId: r.DiscountTypeId, DiscountRate: r.DiscountRate, CityId: r.CityId, GpNo: r.GpNo, VehicleNo: r.VehicleNo, GpDate: dateOnly(r.GpDate) };
            }),
            expenses: dtExp.filter(function (r) { return int(r.ItemId) > 0 || num(r.Amount) > 0 || num(r.Qty) > 0; }).map(function (r) {
                return { ItemId: int(r.ItemId), Remarks: r.Remarks, Qty: String(r.Qty) === '' ? '' : num(r.Qty), Rate: String(r.Rate) === '' ? '' : num(r.Rate), Amount: num(r.Amount) };
            })
        };
    }
    function Insert() {
        if (dtGrid.length === 0) { box('Grid Record Not Found'); return; }
        /* FormValidation :1096 - the branch and project checks run on the server */
        if (int(val('comsupplier')) === 0) { box('CustomerName Field is Required'); focus('comsupplier'); return; }
        var dn = val('txtdocno').trim();
        if (dn === '' || dn === '0') { box('DocNo Field is Required'); focus('txtdocno'); return; }
        if (text('combpttrm') === 'Credit' && val('txtduedays').trim() === '') { box('Due Days Field is Required'); focus('txtduedays'); return; }
        if (!window.confirm(Id > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
        for (var i = 0; i < dtExp.length; i++)
            if (num(dtExp[i].Amount) > 0 && int(dtExp[i].ItemId) === 0) { box('Please Select an Item Against Expense First'); return; }
        var chkBok = $id('ChkBok').checked, chkSlip = $id('ChkPrintslip').checked;
        return http('POST', api + '/save', collect()).then(function (d) {
            (d.warnings || []).forEach(function (w) { box(w); });
            box(d.message);
            Reset();
            if (chkBok) VoucherReport_103(d.voucherHeadId);
            if (chkSlip) SlipUnavailable();
        }, function (e) { box(e.message); });
    }
    function btnSave_Click() { Id = 0; return Insert(); }
    function btnUpdate_Click() {
        if (Approved) { box(AUTO ? 'Record Not Update beacause Record has approved' : 'Record Not Update because Record has approved'); return; }
        return Insert();
    }
    function btnDelete_Click() {
        if (Approved) { box('Record Not Delete because Record has approved'); return; }
        if (!window.confirm('Are you sure to Delete?')) return;
        http('DELETE', api + '/' + Id).then(function (d) { box(d.message); Reset(); }, function (e) { box(e.message); });
    }

    /* Reset() :1236 */
    function Reset() {
        Id = 0; VoucherHeadId = 0; Approved = false; IsFromLoader = false; updateIndex = -1;
        ['txtbillno', 'txtremarks', 'comItem', 'comPackUOM', 'txtQty', 'txtNetBillWeight', 'txtRate', 'comRateUOM', 'txtAmount', 'txtBillAmount', 'comsupplier'].forEach(function (x) { setVal(x, ''); });
        if (AUTO) ['CmbDiscountType', 'txtDiscountRate', 'txtDiscountAmount', 'txtTotalAmount', 'txtDiscountAmtSetForAllRows'].forEach(function (x) { setVal(x, ''); });
        setVal('txtduedays', '0'); dueDateGenerate();
        dtGrid = []; selRow = -1; resetExp();
        if ($id('ChkResetOnSave').checked) OptionResetFields();
        show('btnSave', true); show('btnUpdate', false); show('btnDelete', false);
        show('btnAdd', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        $id('txtRate').disabled = AUTO;
        renderAll(); configurationDefault(); resetDetailPacking(); dueTermChanged(); refreshCombos();
        http('GET', api + '/numbers').then(function (n) { setVal('txtdocno', n.docNo); });
        focus('DocDate');
    }
    function btnNew_Click() { Reset(); }

    // ------------------------------------------------------------------ read (ReadById :2350)

    function ReadById(id) {
        return http('GET', api + '/' + id).then(function (m) {
            Reset();
            Id = int(id);
            setVal('txtdocno', col(m, 'DocNo')); setVal('DocDate', dateOnly(col(m, 'DocDate')));
            showTab(0);
            setVal('comsupplier', col(m, 'SupplierCustomerId')); setVal('txtbillno', col(m, 'ManualBillNo'));
            setVal('txtremarks', col(m, 'RemarksHeader'));
            setVal('txtduedays', col(m, 'SupplierInvoiceNo')); setVal('duedate', dateOnly(col(m, 'SupplierInvoiceDate')));
            Approved = !!col(m, 'IsApproved');
            VoucherHeadId = int(col(m, 'VoucherHeadId'));
            IsFromLoader = false;
            dtGrid = (m.details || []).map(function (r) {
                var row = {
                    Id: int(col(r, 'Id')), RefDocumentTypeId: int(col(r, 'RefRefDocumentTypeId')), RefDocIdNo: int(col(r, 'RefRefDocIdNo')), RefDocSubIdNo: int(col(r, 'RefDocSubId')),
                    WarehouseId: int(col(r, 'WarehouseId')), Warehouse: first(r, ['WareHouseName', 'Warehouse']), ItemId: int(col(r, 'ItemId')), Item: col(r, 'ItemName'), ItemCode: col(r, 'ItemCode'),
                    CropYear: col(r, 'CropYear'), JobLotId: int(col(r, 'JobLotId')), JobLot: col(r, 'JobLotDescription'), PackingTypeId: int(col(r, 'PackingTypeId')), PackingType: col(r, 'PackTypeDesc'),
                    PackUOMId: int(col(r, 'ItemUOMId')), PackUOMEquivalent: num(col(r, 'PackUom')), ItemQty: num(col(r, 'ItemQty')), BalQty: num(col(r, 'ItemQty')),
                    NetBillWeight: num(col(r, 'NetBillWeight')), BalWeight: num(col(r, 'NetBillWeight')), Rate: num(col(r, 'ItemRate')), CgsRate: num(col(r, 'ItemCgsRate')),
                    RateUOMId: int(col(r, 'UomScheduleIdRate')), RateEquivalent: num(col(r, 'RateUOM')),
                    DiscountTypeId: int(col(r, 'DiscountTypeId')), DiscountType: col(r, 'DiscountType'), DiscountRate: num(col(r, 'ItemDiscount')), DiscountAmount: num(col(r, 'ItemDiscountAmount')),
                    ItemAmount: num(col(r, 'ItemAmount')), GpDate: col(r, 'GpDate'), GpNo: int(col(r, 'GpNo')), VehicleNo: col(r, 'VehicleNo'),
                    BillAmount: num(col(r, 'BillAmount')), Expense: num(col(r, 'ExpenseAmount')), Journal: num(col(r, 'JournalAmount')), CityId: int(col(r, 'CityId')), CityName: col(r, 'CityName')
                };
                row.ItemAmountWithoutDisc = row.ItemAmount + row.DiscountAmount;
                if (row.RefDocumentTypeId > 0 || row.RefDocSubIdNo > 0 || row.RefDocIdNo > 0) IsFromLoader = true;
                return row;
            });
            dtExp = (m.expenses || []).map(function (r) {
                return { ItemId: int(col(r, 'InvRevExpItemId')), Qty: num(col(r, 'Qty')), Rate: num(col(r, 'Rate')), Amount: num(col(r, 'Amount')), Remarks: col(r, 'Remarks') };
            });
            if (dtExp.length === 0) resetExp();
            show('btnSave', false); show('btnUpdate', true); show('btnDelete', !AUTO);
            recalcTotals(); renderAll(); refreshCombos();
        }, function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ history (GetAll :2893)

    var HIST_COLS = ['DocNo', 'DocDate', 'CustomerName', 'ManualBillNo', 'DeliveryTerm', 'PaymentTerm', 'DueDays', 'DueDate', 'BillAmount', 'EntryDate', 'EntryUser', 'ModifyDate', 'ModifyUser', 'NoOfAttachments', 'Remarks'];
    var DATE_COLS = { DocDate: 1, DueDate: 1, EntryDate: 1, ModifyDate: 1 };
    function dateMode() {
        if ($id('rdentrydate').checked) return 'entry';
        if ($id('rdmodifydate').checked) return 'modify';
        if ($id('rdapproveddate').checked) return 'approved';
        return 'doc';
    }
    function GetAll() {
        var qs = '?dateMode=' + dateMode() + '&from=' + encodeURIComponent(val('txtFromdateHistory')) + '&to=' + encodeURIComponent(val('txtToDateHistory'))
            + '&fromDocNo=' + int(val('txtFromNoHistory')) + '&toDocNo=' + int(val('txtToDocNoHistory')) + '&customerId=' + int(val('CmbCustomerHistory'));
        return http('GET', api + '/history' + qs).then(function (rows) {
            historyRows = rows || []; selHist = -1; renderHistory(); renderDetail(null);
        }, function (e) { box(e.message); });
    }
    function renderHistory() {
        var head = $id('histHead'), body = $id('histBody');
        if (!historyRows.length) { head.innerHTML = ''; body.innerHTML = ''; $id('histFoot').innerHTML = ''; return; }
        head.innerHTML = '<th>Slip_' + (AUTO ? '291' : '282') + '</th><th>Slip_' + (AUTO ? '291A' : '282A') + '</th><th>Voucher</th><th>Edit</th>' + HIST_COLS.map(function (k) { return '<th>' + esc(k) + '</th>'; }).join('');
        body.innerHTML = historyRows.map(function (r, i) {
            return '<tr data-h="' + i + '" class="' + (i === selHist ? 'sel' : '') + '">'
                + '<td><button type="button" class="bt" data-b="View" data-i="' + i + '">Slip_' + (AUTO ? '291' : '282') + '</button></td>'
                + '<td><button type="button" class="bt" data-b="PrintTwo" data-i="' + i + '">Slip_' + (AUTO ? '291A' : '282A') + '</button></td>'
                + '<td><button type="button" class="bt" data-b="Voucher" data-i="' + i + '">Voucher</button></td>'
                + '<td><button type="button" class="bt" data-b="Edit" data-i="' + i + '">Edit</button></td>'
                + HIST_COLS.map(function (k) {
                    var v = col(r, k);
                    if (DATE_COLS[k]) v = shortDate(v) || v;
                    if (k === 'BillAmount') return '<td class="n">' + amt(v) + '</td>';
                    if (k === 'NoOfAttachments') return '<td><span class="glink" data-att="' + i + '">' + esc(v) + '</span></td>';
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
        }).join('');
        $id('histFoot').innerHTML = '<td></td><td></td><td></td><td></td>' + HIST_COLS.map(function (k) {
            return '<td class="n">' + (k === 'BillAmount' ? amt(historyRows.reduce(function (a, r) { return a + num(col(r, k)); }, 0)) : '') + '</td>';
        }).join('');
    }
    var detCols = [['Warehouse', 't'], ['Item', 't'], ['ItemCode', 't'], ['CropYear', 't'], ['PackingType', 't'], ['PackUOM', 't'], ['ItemQty', 'q', 1], ['NetBillWeight', 'q', 1], ['Rate', 'r'],
        ['RateUOM', 't'], ['ItemAmount', 'a', 1], ['GpDate', 'd'], ['GpNo', 'i'], ['VehicleNo', 't'], ['BillAmount', 'a', 1], ['Expense', 'a', 1], ['Journal', 'a', 1], ['CityName', 't']];
    function renderDetail(rows) {
        $id('detHead').innerHTML = rows ? detCols.map(function (c) { return '<th>' + (c[0] === 'BillAmount' ? 'Item Net Amount' : c[0]) + '</th>'; }).join('') : '';
        $id('detBody').innerHTML = (rows || []).map(function (r) {
            return '<tr>' + detCols.map(function (c) { return '<td class="' + (c[1] === 't' || c[1] === 'd' ? '' : 'n') + '">' + fmtCell(c[1], r[c[0]]) + '</td>'; }).join('') + '</tr>';
        }).join('');
        $id('detFoot').innerHTML = rows ? detCols.map(function (c) {
            return '<td class="n">' + (c[2] ? (c[1] === 'a' ? amt : qtyF)(rows.reduce(function (a, r) { return a + num(r[c[0]]); }, 0)) : '') + '</td>';
        }).join('') : '';
    }
    /* GetDetailGrdByHeadId :3122 */
    function GetDetailGrdByHeadId(i) {
        var h = historyRows[i]; if (!h) return;
        http('GET', api + '/' + int(col(h, 'Id'))).then(function (m) {
            var d = (m.details || []).map(function (r) {
                var disc = num(col(r, 'ItemDiscountAmount'));
                return { Warehouse: first(r, ['WareHouseName', 'Warehouse']), Item: col(r, 'ItemName'), ItemCode: col(r, 'ItemCode'), CropYear: col(r, 'CropYear'), PackingType: col(r, 'PackTypeDesc'),
                    PackUOM: col(r, 'PackUom'), ItemQty: col(r, 'ItemQty'), NetBillWeight: col(r, 'NetBillWeight'), Rate: col(r, 'ItemRate'), RateUOM: col(r, 'RateUOM'),
                    ItemAmount: num(col(r, 'ItemAmount')), GpDate: col(r, 'GpDate'), GpNo: col(r, 'GpNo'), VehicleNo: col(r, 'VehicleNo'), BillAmount: col(r, 'BillAmount'),
                    Expense: col(r, 'ExpenseAmount'), Journal: col(r, 'JournalAmount'), CityName: col(r, 'CityName'), _disc: disc };
            });
            renderDetail(d.length ? d : null);
        }, function () { renderDetail(null); });
    }
    function historyComboBind() {                                                                         /* HistoryComboBind */
        return http('GET', api + '/history-customers').then(function (rows) {
            var keep = val('CmbCustomerHistory');
            fill('CmbCustomerHistory', rows, 'Id', 'CustomerName'); setVal('CmbCustomerHistory', keep); refreshCombos();
        }, function (e) { box(e.message); });
    }
    var DATE_TYPES = [{ Id: 1, Parameters: 'This Day' }, { Id: 2, Parameters: 'This Week' }, { Id: 3, Parameters: 'This Month' }, { Id: 4, Parameters: 'This Year' }, { Id: 5, Parameters: 'Financial Year' }];
    function dateTypeChanged() {                                                                          /* cmbDateTypeHistory_ValueChanged :2790 */
        var t = int(val('cmbDateTypeHistory')), d = new Date();
        if (t === 1) setVal('txtFromdateHistory', iso(d));
        else if (t === 2) { d.setDate(d.getDate() - 7); setVal('txtFromdateHistory', iso(d)); }
        else if (t === 3) { setVal('txtFromdateHistory', iso(new Date(d.getFullYear(), d.getMonth(), 1))); setVal('txtToDateHistory', today()); }
        else if (t === 4) { setVal('txtFromdateHistory', iso(new Date(d.getFullYear(), 0, 1))); setVal('txtToDateHistory', today()); }
        else if (t === 5) { if (cfg.yearStart) setVal('txtFromdateHistory', cfg.yearStart); }
    }
    function Resethistory() {                                                                             /* btnLoadAll_Click :2869 -> Resethistory */
        setVal('cmbDateTypeHistory', 3); dateTypeChanged(); setVal('txtFromNoHistory', ''); setVal('txtToDocNoHistory', ''); setVal('CmbCustomerHistory', ''); refreshCombos();
    }
    function showTab(t) {
        tab = t;
        $id('tabPage1').style.display = t === 0 ? '' : 'none';
        $id('tabPage2').style.display = t === 1 ? '' : 'none';
        $id('tabForm').className = t === 0 ? 'on' : '';
        $id('tabHistory').className = t === 1 ? 'on' : '';
        if (t === 1) {                                                                                    /* tabControl1_SelectedIndexChanged :2839 */
            historyComboBind();
            fill('cmbDateTypeHistory', DATE_TYPES, 'Id', 'Parameters');
            if (int(val('cmbDateTypeHistory')) === 0) setVal('cmbDateTypeHistory', 1);
            refreshCombos();
        }
    }

    // ------------------------------------------------------------------ prints

    function VoucherReport_103(vh) {
        if (!int(vh)) { box('VoucherId Not Found'); return; }
        window.open('/api/print/acc-103/pdf?id=' + encodeURIComponent(vh) + '&documentTypeId=' + DOC, '_blank');
    }
    function SlipUnavailable() { box('The ' + (AUTO ? '291 / 291A' : '282 / 282A') + ' sale bill report is not available on the web yet.'); }
    function btnPrint_Click() { VoucherReport_103(VoucherHeadId); }
    function btnSlip_Click() { SlipUnavailable(); }
    function btn294APrint_Click() { SlipUnavailable(); }
    function btnAttachment_Click() { box('Attachments are not available on this screen yet.'); }
    function btnRefresh_Click() {                                                                         /* btnRefresh_Click :1344 */
        http('GET', api + '/lookups').then(function (d) {
            var keep = { s: val('comsupplier'), i: val('comItem'), w: val('comWarehouse'), c: val('comCropYear'), p: val('comPackingType'), t: val('combpttrm') };
            L = d || {}; perms = L.rights || {}; cfg = L.configuration || cfg; bindLists();
            setVal('comsupplier', keep.s); setVal('comItem', keep.i); setVal('comWarehouse', keep.w); setVal('comCropYear', keep.c); setVal('comPackingType', keep.p); setVal('combpttrm', keep.t);
            refreshCombos(); applyRights(); renderExp();
        }, function (e) { box(e.message); });
    }
    function shortcutKeys() {
        box(['Ctrl+E  For Close', 'Ctrl+N  For New', 'Ctrl+R  For Refresh', 'Ctrl+L  To Open Load From Issuance', 'Ctrl+S  For Save', 'Ctrl+U  For Update',
            'Ctrl+Shift+Delete  For Delete', 'Alt+1  For Print Slip ' + (AUTO ? '291' : '282'), 'Alt+2  For Print Slip ' + (AUTO ? '291A' : '282A'), 'Alt+3  For Print Voucher 103',
            'Ctrl+F5  For Focus on Doc Date', 'Ctrl+F10  For Open Attachments', 'Ctrl+T  For Tab Transfer', 'Ctrl+ArrowDown  For Focus On Detail Grid',
            'Ctrl+ArrowUp  For Focus On WareHouse Name in Detail Box'].join('\n'));
    }

    // ------------------------------------------------------------------ Load From Issuance (LoadavailableTransactionsForIssuance)

    function btnLoadSaleOrder_Click() {                                                                   /* btnLoadSaleOrder_Click :3910 */
        if (dtGrid.length > 0 && !IsFromLoader) { box('You cannot Load Data from Loader when Manual record Exist in grd'); return; }
        LD = { rows: [], setup: null };
        $id('loaderModal').style.display = 'flex';
        setVal('ldTo', today()); setVal('ldFrom', today());
        $id('loaderHead').innerHTML = ''; $id('gridLoader').innerHTML = ''; $id('loaderFoot').innerHTML = '';
        http('GET', api + '/loader/setup').then(function (d) {
            LD.setup = d || {};
            if (LD.setup.loadError) { box(LD.setup.loadError); return; }
            if (LD.setup.fromDate) setVal('ldFrom', dateOnly(LD.setup.fromDate));
            var l = LD.setup.lists || {};
            fill('ldParent', l.ParentCategories, 'Id', 'name'); fill('ldItemCategory', l.ItemCategories, 'Id', 'name'); fill('ldItemType', l.ItemTypes, 'Id', 'name');
            fill('ldJobLot', l.JobLot, 'Id', 'name'); fill('ldCrop', l.CropYear, 'Id', 'name'); fill('ldWarehouse', l.Warehouse, 'Id', 'name');
            fill('ldRefDoc', l.DocumentType, 'Id', 'name'); fill('ldParty', l.Supplier_Customer, 'Id', 'name'); fill('ldItem', l.Items, 'Id', 'name');
            setVal('ldWarehouse', val('comWarehouse'));                                                   /* LoadTransactions.CmbWarehouse.Value = comWarehouse */
            refreshCombos();
            loaderSearch();
        }, function (e) { box(e.message); });
    }
    function selText(x) { return text(x).trim(); }
    function loaderSearch() {
        if (!LD.setup || LD.setup.loadError) return;
        var qs = '?fromDate=' + encodeURIComponent(val('ldFrom')) + '&toDate=' + encodeURIComponent(val('ldTo')) + '&parentCategoryId=' + int(val('ldParent'))
            + '&itemCategoryId=' + int(val('ldItemCategory')) + '&itemTypeId=' + int(val('ldItemType')) + '&jobLotId=' + int(val('ldJobLot'))
            + '&cropYear=' + encodeURIComponent(selText('ldCrop')) + '&warehouseId=' + int(val('ldWarehouse')) + '&refDocumentTypeId=' + int(val('ldRefDoc'))
            + '&supplierCustomerId=' + int(val('ldParty')) + '&itemId=' + int(val('ldItem'));
        return http('GET', api + '/loader/search' + qs).then(function (rows) {
            LD.rows = (rows || []).map(function (r) { r._chk = false; return r; }); drawLoader();
        }, function (e) { box(e.message); });
    }
    var LD_COLS = [['RefDocumentType', 't'], ['DocDate', 'd'], ['DocCodeNo', 't'], ['ManualNo', 't'], ['GrnNo', 't'], ['SupplierCustomerName', 't'], ['VehicleNo', 't'], ['BiltyNo', 't'],
        ['GpNo', 't'], ['WareHouseCode', 't'], ['RefWarehouse', 't'], ['ItemName', 't'], ['ItemCode', 't'], ['CropBatch', 't'], ['JobLotCode', 't'], ['PackingType', 't'], ['PackUom', 't'],
        ['QtyIn', 'q', 1], ['QtyOut', 'q', 1], ['QtyBalance', 'q', 1], ['WeightIn', 'q', 1], ['WeightOut', 'q', 1], ['WeightBalance', 'q', 1], ['AVgRate', 'r', 0, 1], ['RateUom', 't'],
        ['ItemAmount', 'a', 1, 1], ['Remarks', 't']];
    function drawLoader() {
        var showValues = !!(LD.setup && LD.setup.valuesShowRights);
        var cs = LD_COLS.filter(function (c) { return showValues || !c[3]; });
        if (!LD.rows.length) { $id('loaderHead').innerHTML = ''; $id('gridLoader').innerHTML = ''; $id('loaderFoot').innerHTML = ''; selectedTotals(); return; }
        var all = LD.rows.every(function (r) { return r._chk; });
        $id('loaderHead').innerHTML = '<th><input type="checkbox" id="ldAll"' + (all ? ' checked' : '') + '/> Select</th>' + cs.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('');
        $id('gridLoader').innerHTML = LD.rows.map(function (r, i) {
            return '<tr data-l="' + i + '"><td class="ctr"><input type="checkbox" data-l="' + i + '"' + (r._chk ? ' checked' : '') + '/></td>' + cs.map(function (c) {
                var v = col(r, c[0]);
                return '<td class="' + (c[1] === 't' || c[1] === 'd' ? '' : 'n') + '">' + (c[1] === 'd' ? esc(shortDate(v)) : (c[1] === 't' ? esc(v) : fmtCell(c[1], v))) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        $id('loaderFoot').innerHTML = '<td></td>' + cs.map(function (c) {
            return '<td class="n">' + (c[2] ? fmtCell(c[1], LD.rows.reduce(function (a, r) { return a + num(col(r, c[0])); }, 0)) : '') + '</td>';
        }).join('');
        selectedTotals();
    }
    function selectedTotals() {                                                                           /* SelectedWeightCalculation */
        var w = 0, q = 0;
        LD.rows.forEach(function (r) { if (r._chk) { w += num(col(r, 'WeightBalance')); q += num(col(r, 'QtyBalance')); } });
        setVal('ldSelWeight', fmt(w, 0, '0')); setVal('ldSelQty', fmt(q, 0, '0'));
    }
    function loaderReset() { setVal('ldParty', ''); setVal('ldItem', ''); LD.rows = []; drawLoader(); refreshCombos(); }
    function loaderClose() { $id('loaderModal').style.display = 'none'; }
    /* btnLoadOnInvoice_Click_1 then LoadDataDetailfromPurchaseInvoivce :3939 */
    function loaderLoad() {
        var sel = LD.rows.filter(function (r) { return r._chk; });
        if (sel.length === 0) { box('Please Select Row first'); return; }
        loaderClose();
        IsFromLoader = true;
        sel.forEach(function (d) {
            var t = int(col(d, 'RefDocumentTypeId')), idn = int(col(d, 'RefDocIdNo')), sub = int(col(d, 'RefDocSubIdNo'));
            if (dtGrid.some(function (x) { return x.RefDocumentTypeId === t && x.RefDocIdNo === idn && x.RefDocSubIdNo === sub; })) return;
            var amtv = num(col(d, 'ItemAmount')), avg = num(col(d, 'AVgRate'));
            var row = {
                Id: 0, RefDocumentTypeId: t, RefDocIdNo: idn, RefDocSubIdNo: sub, WarehouseId: int(col(d, 'WarehouseId')), Warehouse: col(d, 'WareHouseCode'),
                ItemId: int(col(d, 'ItemId')), Item: col(d, 'ItemName'), ItemCode: col(d, 'ItemCode'), CropYear: col(d, 'CropBatch'), JobLotId: int(col(d, 'JobLotId')), JobLot: col(d, 'JobLotCode'),
                PackingTypeId: int(col(d, 'InvPackingTypeId')), PackingType: col(d, 'PackingType'), PackUOMId: int(col(d, 'ItemUom')), PackUOMEquivalent: num(col(d, 'PackSize')),
                ItemQty: num(col(d, 'QtyBalance')), BalQty: num(col(d, 'QtyBalance')), NetBillWeight: num(col(d, 'WeightBalance')), BalWeight: num(col(d, 'WeightBalance')),
                Rate: AUTO ? 0 : avg, CgsRate: avg, RateUOMId: int(col(d, 'RateUomId')), RateEquivalent: num(col(d, 'Equivalent')), ItemAmountWithoutDisc: amtv,
                DiscountTypeId: 0, DiscountType: '', DiscountRate: 0, DiscountAmount: 0, ItemAmount: amtv, GpDate: today(), GpNo: 0, VehicleNo: '',
                BillAmount: 0, Expense: 0, Journal: 0, CityId: 0, CityName: ''
            };
            dtGrid.push(row);
        });
        recalcTotals();
    }

    // ------------------------------------------------------------------ wiring

    function decOnly(e) { if (e.key && e.key.length === 1 && !/[0-9.,]/.test(e.key) && !e.ctrlKey && !e.metaKey) e.preventDefault(); }
    function intOnly(e) { if (e.key && e.key.length === 1 && !/[0-9]/.test(e.key) && !e.ctrlKey && !e.metaKey) e.preventDefault(); }

    function bindEvents() {
        on('txtQty', 'input', qtyChanged); on('comPackUOM', 'change', packUomLeave); on('comRateUOM', 'change', rateUomLeave); on('txtRate', 'input', rateChanged);
        on('txtduedays', 'input', dueDateGenerate); on('DocDate', 'change', function () { dueDateGenerate(); if (AUTO) GetRateFromItemPriceScheduleByItemId(); });
        on('combpttrm', 'change', dueTermChanged); on('comsupplier', 'change', supplierChanged);
        on('comItem', 'change', itemLeave);
        ['comWarehouse', 'comCropYear', 'comPackingType'].forEach(function (x) { on(x, 'change', AvailableStockGetByItem); });
        on('comjobLot', 'change', AvailableStockGetByItem);
        on('rdbtnItemName', 'change', function () { var k = val('comItem'); fill('comItem', L.items, 'Id', 'ItemName'); setVal('comItem', k); refreshCombos(); });
        on('rdbtnItemCode', 'change', function () { var k = val('comItem'); fill('comItem', L.items, 'Id', 'ItemCode'); setVal('comItem', k); refreshCombos(); });
        on('CmbDiscountType', 'change', discTypeLeave); on('txtDiscountRate', 'input', discRateChanged);
        on('btnUpdateDiscountInGrid', 'click', btnUpdateDiscountInGrid_Click); on('BtnPropotionateDiscApply', 'click', BtnPropotionateDiscApply_Click);
        on('BtnRefreshRates', 'click', BtnRefreshRates_Click);
        ['txtQty', 'txtRate', 'txtDiscountRate', 'txtDiscountAmtSetForAllRows'].forEach(function (x) { on(x, 'keypress', decOnly); });
        ['txtduedays', 'txtFromNoHistory', 'txtToDocNoHistory'].forEach(function (x) { on(x, 'keypress', intOnly); });
        on('btnAdd', 'click', btnAdd_Click); on('btnUpdateDetail', 'click', btnUpdateDetail_Click); on('btnCancelUpdateDetial', 'click', btnCancelUpdateDetial_Click);
        on('cmbDateTypeHistory', 'change', dateTypeChanged); on('btnshow', 'click', GetAll);
        $id('grdBody').addEventListener('click', function (e) {
            var d = e.target.closest('button[data-del]'); if (d) { gridDelete(int(d.getAttribute('data-del'))); return; }
            var tr = e.target.closest('tr[data-i]'); if (tr && e.target.tagName !== 'INPUT') { selRow = int(tr.getAttribute('data-i')); renderGrid(); }
        });
        $id('grdBody').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && e.target.tagName !== 'INPUT') grdDoubleClick(int(tr.getAttribute('data-i'))); });
        $id('grdBody').addEventListener('change', function (e) {
            var t = e.target, tr = t.closest('tr[data-i]'); if (!tr || !t.getAttribute('data-f')) return;
            gridCell(int(tr.getAttribute('data-i')), t.getAttribute('data-f'), t.value);
        });
        $id('expBody').addEventListener('change', function (e) {
            var t = e.target, tr = t.closest('tr[data-e]'); if (!tr || !t.getAttribute('data-f')) return;
            expCell(int(tr.getAttribute('data-e')), t.getAttribute('data-f'), t.value);
        });
        $id('expBody').addEventListener('click', function (e) {
            var d = e.target.closest('button[data-edel]'), a = e.target.closest('button[data-eadd]');
            if (d) { dtExp.splice(int(d.getAttribute('data-edel')), 1); if (dtExp.length === 0) resetExp(); renderExp(); BillAmount(); }
            if (a) { dtExp.push({ ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }); renderExp(); BillAmount(); }
        });
        $id('histBody').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-b]'), tr = e.target.closest('tr[data-h]');
            if (tr && !b) { selHist = int(tr.getAttribute('data-h')); renderHistory(); GetDetailGrdByHeadId(selHist); }
            if (!b) return;
            var i = int(b.getAttribute('data-i')), h = historyRows[i]; if (!h) return;
            var k = b.getAttribute('data-b');
            if (k === 'Edit') ReadById(int(col(h, 'Id')));
            if (k === 'View' || k === 'PrintTwo') SlipUnavailable();
            if (k === 'Voucher') VoucherReport_103(int(col(h, 'VoucherHeadId')));
        });
        $id('histBody').addEventListener('dblclick', function (e) {
            var tr = e.target.closest('tr[data-h]'); if (!tr || e.target.closest('button')) return;
            var h = historyRows[int(tr.getAttribute('data-h'))]; if (h) ReadById(int(col(h, 'Id')));
        });
        $id('gridLoader').addEventListener('change', function (e) {
            var c = e.target.closest('input[data-l]'); if (!c) return;
            LD.rows[int(c.getAttribute('data-l'))]._chk = c.checked; selectedTotals();
        });
        $id('loaderHead').addEventListener('change', function (e) {
            if (e.target.id !== 'ldAll') return;
            LD.rows.forEach(function (r) { r._chk = e.target.checked; }); drawLoader();
        });
        document.addEventListener('keydown', function (e) {
            var tg = e.target, tn = tg && tg.tagName, k = (e.key || '');
            var loaderOpen = $id('loaderModal') && $id('loaderModal').style.display !== 'none';
            if (loaderOpen) {
                if (k === 'Escape' || (e.ctrlKey && (k === 'e' || k === 'E'))) { loaderClose(); return; }
                if (e.ctrlKey && (k === 's' || k === 'S')) { e.preventDefault(); loaderSearch(); return; }
                if (e.ctrlKey && (k === 'l' || k === 'L')) { e.preventDefault(); loaderLoad(); return; }
                return;
            }
            if (k === 'Enter' && (tn === 'INPUT' || tn === 'SELECT') && tg.type !== 'checkbox' && tg.type !== 'radio' && tg.type !== 'button') {            /* SendKeys TAB */
                var all = Array.prototype.filter.call(document.querySelectorAll('#frm input:not([type=hidden]), #frm select, #frm textarea, #frm button'),
                    function (x) { return x.offsetParent !== null && !x.disabled && !x.readOnly && !x.classList.contains('dtcombo-native'); });
                var ix = all.indexOf(tg); if (ix >= 0 && ix < all.length - 1) { e.preventDefault(); all[ix + 1].focus(); }
            }
            if (e.ctrlKey && e.altKey && !e.shiftKey && (k === 'Control' || k === 'Alt')) { shortcutKeys(); return; }
            if (e.ctrlKey && (k === 't' || k === 'T')) { e.preventDefault(); showTab(tab === 1 ? 0 : 1); return; }
            if (tab !== 0) return;
            if (e.ctrlKey && (k === 's' || k === 'S') && !$id('btnSave').disabled && $id('btnSave').style.display !== 'none') { e.preventDefault(); btnSave_Click(); }
            else if (e.ctrlKey && (k === 'l' || k === 'L')) { e.preventDefault(); btnLoadSaleOrder_Click(); }
            else if (e.ctrlKey && (k === 'u' || k === 'U') && !$id('btnUpdate').disabled && $id('btnUpdate').style.display !== 'none') { e.preventDefault(); btnUpdate_Click(); }
            else if (e.ctrlKey && (k === 'n' || k === 'N')) { e.preventDefault(); btnNew_Click(); }
            else if (e.ctrlKey && (k === 'r' || k === 'R')) { e.preventDefault(); btnRefresh_Click(); }
            else if (e.ctrlKey && e.key === 'F5') focus('DocDate');
            else if (e.ctrlKey && e.key === 'F10') btnAttachment_Click();
            else if (e.ctrlKey && e.key === 'ArrowUp') focus('comWarehouse');
            else if (e.ctrlKey && e.key === 'ArrowDown') { var g = $id('grdBody').querySelector('input,button'); if (g) g.focus(); }
            else if (e.ctrlKey && e.shiftKey && k === 'Delete' && $id('btnDelete') && $id('btnDelete').style.display !== 'none') btnDelete_Click();
            else if (e.altKey && k === '1') btnSlip_Click();
            else if (e.altKey && k === '2') btn294APrint_Click();
            else if (e.altKey && k === '3') btnPrint_Click();
        });
    }

    window.SFLOUR = { showTab: showTab, btnNew_Click: btnNew_Click, btnRefresh_Click: btnRefresh_Click, btnSave_Click: btnSave_Click, btnUpdate_Click: btnUpdate_Click,
        btnDelete_Click: btnDelete_Click, btnAttachment_Click: btnAttachment_Click, btnSlip_Click: btnSlip_Click, btn294APrint_Click: btn294APrint_Click,
        btnPrint_Click: btnPrint_Click, btnLoadSaleOrder_Click: btnLoadSaleOrder_Click, btnShortcutKeys_Click: shortcutKeys, BtnNewHistory_Click: Resethistory,
        BtnRefreshHistory_Click: historyComboBind, loaderSearch: loaderSearch, loaderReset: loaderReset, loaderClose: loaderClose, loaderLoad: loaderLoad };
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
