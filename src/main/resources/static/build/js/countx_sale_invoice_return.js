/* InvfrmSaleInvoiceReturn (Sale Invoice Return, DocumentTypeId 98) - the desktop form's events, line for line.
   References ":NNN" are Architecture.WinApp.Sale/InvfrmSaleInvoiceReturn.cs, "L:NNN" LoadSaleInvoiceForReturn.cs.
   Server: /sale/sale-invoice-return/api (SaleInvoiceReturnController). Uses StoreCommon (busy buttons, searchable
   combos, footer History button) and PurchaseInvoiceAttachments (the same attachment dialog as the purchase invoices). */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num;
    var API = '/sale/sale-invoice-return/api';

    var st = {
        recId: 0, voucherHeadId: 0, approved: false, updateDetailIndex: -1,
        flags: {}, look: {}, N: 0, R: 2,
        grid: [], invExp: [], gl: [], freight: [], emptyBags: [],
        historyRows: [], historyBranches: [], loader: { branches: [], rows: [], userBranchId: 0 },
        supplierGlId: 0   /* txtSupplierGLId is never assigned on the desktop (Q-B3) */
    };

    // ------------------------------------------------------------------ Conversion.* / formats
    function toDouble(v) { if (v === null || v === undefined) return 0; var s = String(v).trim().replace(/,/g, ''); if (s === '') return 0; var x = Number(s); return isFinite(x) ? x : 0; }
    function toInt(v) { if (typeof v === 'number') return Math.round(v) === v ? v : bankers(v); var s = String(v == null ? '' : v).trim(); if (s === '') return 0; return /^-?\d+$/.test(s) ? parseInt(s, 10) : 0; }
    function bankers(x) { var r = Math.round(x); if (Math.abs(x % 1) === 0.5) r = 2 * Math.round(x / 2); return r; }
    function afz(x, d) { var f = Math.pow(10, d); var y = Math.abs(x) * f; return (x < 0 ? -1 : 1) * Math.round(y + 1e-9) / f; }   // MidpointRounding.AwayFromZero
    function grp(v, d) { if (!isFinite(v)) return v > 0 ? '∞' : v < 0 ? '-∞' : 'NaN'; return Number(v).toLocaleString('en-US', { minimumFractionDigits: d, maximumFractionDigits: d }); }
    function fmtAmt(v) { return grp(v, st.N); }                       // stringFormatsingle
    function fmtRate(v) { return grp(v, st.R === 0 ? 2 : st.R); }     // DecimalRateFormate ("#,#0." + zeros; 0 -> "00")
    function fmtN(v, max) { if (!isFinite(v)) return String(v); return Number(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: max }); }   // "#,##0.##" / "#,##0.###"
    function today() { return SC.today(); }
    function addDays(iso, d) { var t = new Date(iso + 'T00:00:00'); t.setDate(t.getDate() + d); return SC.isoDay(t); }
    function daysBetween(a, b) { return Math.round((new Date(b + 'T00:00:00') - new Date(a + 'T00:00:00')) / 86400000); }
    function shortDate(v) { if (!v) return ''; var s = String(v); return s.length >= 10 ? s.substring(0, 10) : s; }
    function gridDate(v) { var s = shortDate(v); if (!s || s === '1900-01-01') return ''; return SC.gridDate(s); }

    function msg(text, error) {
        var m = $id('fxMsg'); m.textContent = text || ''; m.classList.toggle('is-error', !!error); m.style.display = text ? 'block' : 'none';
        if (text) { clearTimeout(msg.t); msg.t = setTimeout(function () { m.style.display = 'none'; }, error ? 9000 : 4000); }
    }
    function alertBox(text) { alert(text); }
    function get(url) { return SC.getJson(url); }
    function post(url, body) { return SC.postJson(url, body); }

    function fill(id, rows, valueKey, textKey, blank) {
        var el = $id(id), keep = el.value; el.innerHTML = '';
        if (blank !== false) el.appendChild(new Option(blank === true ? '' : (blank || ''), ''));
        (rows || []).forEach(function (r) { el.appendChild(new Option(String(SC.ci(r, textKey) == null ? '' : SC.ci(r, textKey)), String(SC.ci(r, valueKey)))); });
        el.value = keep; if (el.value !== keep) el.value = '';
    }
    function selText(id) { var el = $id(id); return el.selectedOptions[0] ? el.selectedOptions[0].textContent : ''; }
    function selRow(id, rows, key) { var v = $id(id).value; if (v === '') return null; return (rows || []).find(function (r) { return String(SC.ci(r, key)) === v; }) || null; }
    function setValue(id, v) { var el = $id(id); el.value = v == null ? '' : String(v); if (el.tagName === 'SELECT' && el.value !== String(v == null ? '' : v)) el.value = ''; }
    function setText(id, text) { var el = $id(id); var o = Array.from(el.options).find(function (x) { return x.textContent === String(text); }); el.value = o ? o.value : ''; }

    function decimalKey(e, allowMinus) {
        var c = e.key; if (c.length !== 1) return; if (/\d/.test(c)) return; if (c === '.' && e.target.value.indexOf('.') < 0) return; if (allowMinus && c === '-' && e.target.selectionStart === 0 && e.target.value.indexOf('-') < 0) return;
        e.preventDefault();
    }

    // ------------------------------------------------------------------ 5.1 detail box calculations
    function Total() {   // :3069
        try {
            var qty = toDouble($id('txtQty').value), gross = toDouble($id('txtGrossWeight').value), ebUnit = toDouble($id('txtEmptybagsUnit').value);
            var ebTotal = ebUnit * qty > 0 ? ebUnit * qty : 0;
            $id('txtEmptyBagsTotal').value = fmtN(ebTotal, 3);
            var wc = toDouble($id('txtwtcut').value) !== 0 ? parseFloat($id('txtwtcut').value.trim().replace(/,/g, '')) : 0;
            var addLess = toDouble($id('txtAddLss').value) !== 0 ? $id('txtAddLss').value.trim() : '0';
            var wct = qty * wc; $id('txtWeightCutTotal').value = String(wct);
            var weight = gross - ebTotal - wct + parseFloat(addLess.replace(/,/g, ''));
            $id('txtNetBillWeight').value = fmtN(weight, 2); $id('txtStockWeight').value = fmtN(weight, 2);
            AmountCaluculation();
        } catch (e) { alertBox(e.message); }
    }
    function txtQty_TextChanged() {   // :3229
        try {
            var q = $id('txtQty').value, u = selRow('comPackUOM', st.uoms, 'Id');
            if (q !== '' && u) $id('txtGrossWeight').value = fmtN(parseFloat(q.trim()) * toDouble(SC.ci(u, 'Equivalent')), 2);
            Total();
        } catch (e) { alertBox(e.message); }
    }
    function comPackUOM_Leave() {   // :3211
        try {
            var u = selRow('comPackUOM', st.uoms, 'Id');
            if ($id('txtQty').value !== '' && u) $id('txtGrossWeight').value = fmtN(toDouble($id('txtQty').value) * toDouble(SC.ci(u, 'Equivalent')), 3);
            Total();
        } catch (e) { alertBox(e.message); }
    }
    function AmountCaluculation() {   // :3173
        var u = selRow('comRateUOM', st.uoms, 'Id'), rateUom = u ? toDouble(SC.ci(u, 'Equivalent')) : 0;
        var rate = toDouble($id('txtRate').value), rateCut = toDouble($id('txtratecut').value), net = toDouble($id('txtNetBillWeight').value);
        var itemAmount = net / rateUom * rate, rateCutTotal = net / rateUom * rateCut;
        $id('txtratecuttotal').value = fmtAmt(afz(rateCutTotal, st.N));
        itemAmount -= rateCutTotal;
        $id('txtAmount').value = fmtAmt(afz(itemAmount, st.N));
    }
    function rateChanged() { AmountCaluculation(); TotalCommissionAmount(); BillAmount(); }

    /** comItem_Leave -> PackUOM() :1309 */
    function PackUOM() {
        var itemId = num($id('comItem').value), prevPack = selText('comPackUOM'), prevRate = selText('comRateUOM');
        if (!itemId) { st.uoms = []; fill('comPackUOM', [], 'Id', 'UOMCode'); fill('comRateUOM', [], 'Id', 'UOMCode'); return Promise.resolve(); }
        return get(API + '/uoms?itemId=' + itemId).then(function (rows) {
            st.uoms = rows || [];
            fill('comPackUOM', st.uoms, 'Id', 'UOMCode'); fill('comRateUOM', st.uoms, 'Id', 'UOMCode');
            setText('comPackUOM', prevPack); setText('comRateUOM', prevRate);
        }).catch(function (e) { msg(e.message, true); });
    }

    // ------------------------------------------------------------------ 5.2 detail box buttons
    function FormValidationDetail() {   // :934
        var checks = [
            ['comItem', function () { return $id('comItem').value === '' || num($id('comItem').value) === 0; }, 'Item Name Field is Required'],
            ['comCropYear', function () { return $id('comCropYear').value === ''; }, 'Crop Year Field is Required'],
            ['comjobLot', function () { return $id('comjobLot').value === '' || num($id('comjobLot').value) === 0; }, 'Job/Lot Field is Required'],
            ['comPackingType', function () { return $id('comPackingType').value === ''; }, 'Packing Type Field is Required'],
            ['txtQty', function () { var v = $id('txtQty').value; return v === '' || v === '0'; }, 'Qty Field is Required'],
            ['comPackUOM', function () { return $id('comPackUOM').value === ''; }, 'Pack Unit Field is Required'],
            ['txtGrossWeight', function () { var v = $id('txtGrossWeight').value; return v === '' || v === '0'; }, 'Gross Weight Field is Required'],
            ['txtNetBillWeight', function () { var v = $id('txtNetBillWeight').value; return v === '' || v === '0'; }, 'Net Bill Weight Field is Required'],
            ['txtStockWeight', function () { var v = $id('txtStockWeight').value; return v === '' || v === '0'; }, 'Stock Weight Field is Required'],
            ['txtRate', function () { return $id('txtRate').value === '' || toDouble($id('txtRate').value) === 0; }, 'Rate Field is Required'],
            ['comRateUOM', function () { return $id('comRateUOM').value === ''; }, 'Rate UOM Field is Required'],
            ['comWarehouse', function () { return $id('comWarehouse').value === '' || num($id('comWarehouse').value) === 0; }, 'Warehouse Field is Required']
        ];
        for (var i = 0; i < checks.length; i++) if (checks[i][1]()) { alertBox(checks[i][2]); $id(checks[i][0]).focus(); return false; }
        return true;
    }
    function modeCheck() {
        for (var i = 0; i < st.grid.length; i++) {
            if (num(st.grid[i].GrnId) > 0) throw new Error('Entry Against Grn Exists in Grid,So Manual Entry is Not Allow');
            if (num(st.grid[i].RefDocId) > 0) throw new Error('Entry Against SaleInvoice Exists in Grid,So Manual Entry is Not Allow');
        }
    }
    function branchCheck() {
        var wh = selRow('comWarehouse', st.look.warehouses, 'Id'), lot = selRow('comjobLot', st.look.jobLots, 'Id');
        var b = { id: wh ? num(SC.ci(wh, 'BranchId')) : 0, name: wh ? String(SC.ci(wh, 'BranchName') || '') : '', jobId: lot ? num(SC.ci(lot, 'BranchId')) : 0, jobName: lot ? String(SC.ci(lot, 'BranchName') || '') : '' };
        if (st.flags.branchFeature && !st.flags.branchImplemented && b.id !== b.jobId) {
            alertBox("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + b.name + "' and JobLot is of Branch '" + b.jobName + "'"); return null;
        }
        return b;
    }
    function blankRow() {
        return { Id: 0, GrnId: 0, GrnDetailId: 0, GrnNo: 0, WarehouseId: 0, Warehouse: '', ItemId: 0, Item: '', CropYear: '', JobLotId: 0, JobLot: '', PackingTypeId: 0, PackingType: '',
            PackUOMId: 0, PackUOM: '', PackUOMEquivalent: 0, ItemQty: 0, GrossWeight: 0, EmptyBags: 0, EmptyBagsTotal: 0, WeightCut: 0, WeightCutTotal: 0, AddLss: 0, NetBillWeight: 0, StockWeight: 0,
            Rate: 0, RateUOMId: 0, RateUOM: '', RateEquivalent: 0, RateCut: 0, RateCutTotal: 0, ItemAmount: 0, LabSampleNo: '', GpDate: '', GpNo: 0, VehicleNo: '', BillAmount: 0, Expense: 0, Freights: 0,
            Journal: 0, Commission: 0, Wages: 0, EbPurAgainstWeightAmount: 0, BranchId: 0, BranchName: '', RefDocumentTypeId: 0, RefDocumentType: '', RefDocId: 0, RefDocNo: 0, RefDocSubId: 0, RefInvoiceQty: 0, RefInvoiceWeight: 0 };
    }
    function btnAdd_Click() {   // :1431
        try {
            modeCheck();
            if (!FormValidationDetail()) return;
            if ($id('txtAmount').value === '' || $id('txtAmount').value === '0') throw new Error('Please Check ItemAmount');
            var b = branchCheck(); if (!b) return;
            var rateU = selRow('comRateUOM', st.uoms, 'Id');
            var r = blankRow();
            r.WarehouseId = num($id('comWarehouse').value); r.Warehouse = selText('comWarehouse'); r.ItemId = num($id('comItem').value); r.Item = selText('comItem');
            r.CropYear = selText('comCropYear'); r.JobLotId = num($id('comjobLot').value); r.JobLot = selText('comjobLot'); r.PackingTypeId = num($id('comPackingType').value); r.PackingType = selText('comPackingType');
            /* Q-D1: btnAdd writes the RATE UOM into PackUOMId/PackUOM/PackUOMEquivalent (btnUpdateDetail uses the pack UOM) */
            r.PackUOMId = num($id('comRateUOM').value); r.PackUOM = selText('comRateUOM'); r.PackUOMEquivalent = rateU ? toDouble(SC.ci(rateU, 'Equivalent')) : 0;
            r.ItemQty = toDouble($id('txtQty').value); r.GrossWeight = toDouble($id('txtGrossWeight').value); r.EmptyBags = toDouble($id('txtEmptybagsUnit').value); r.EmptyBagsTotal = toDouble($id('txtEmptyBagsTotal').value);
            r.WeightCut = toDouble($id('txtwtcut').value); r.WeightCutTotal = toDouble($id('txtWeightCutTotal').value); r.AddLss = toDouble($id('txtAddLss').value); r.NetBillWeight = toDouble($id('txtNetBillWeight').value); r.StockWeight = toDouble($id('txtStockWeight').value);
            r.Rate = toDouble($id('txtRate').value); r.RateUOMId = num($id('comRateUOM').value); r.RateUOM = selText('comRateUOM'); r.RateEquivalent = rateU ? toDouble(SC.ci(rateU, 'Equivalent')) : 0;
            r.RateCut = toDouble($id('txtratecut').value); r.RateCutTotal = toDouble($id('txtratecuttotal').value); r.ItemAmount = toDouble($id('txtAmount').value);
            r.LabSampleNo = $id('txtlabanalysisno').value; r.GpDate = $id('txtgpdate').value || today(); r.GpNo = toInt($id('txtgatepassno').value); r.VehicleNo = $id('txtvehicleno').value;
            r.BranchId = b.id; r.BranchName = b.name;
            st.grid.push(r);
            $id('comsupplier').disabled = true;
            renderGrid(); TotalCommissionAmount(); FreightProportion(); CommissionProportion(); ExpProportion(); BillAmount(); ResetDetail();   // no WagesAmountProportion (Q-D2)
        } catch (e) { alertBox(e.message); }
    }
    function grd_DoubleClick(i) {   // :1494
        var r = st.grid[i]; if (!r || num(r.GrnId) !== 0 || num(r.RefDocId) !== 0) return;
        st.updateDetailIndex = i;
        setValue('comItem', r.ItemId); if ($id('comItem').value === '') { $id('comItem').appendChild(new Option(r.Item, String(r.ItemId))); $id('comItem').value = String(r.ItemId); }
        PackUOM().then(function () {
            setText('comCropYear', r.CropYear); setValue('comjobLot', r.JobLotId); setValue('comPackingType', r.PackingTypeId);
            $id('txtQty').value = String(r.ItemQty); setValue('comPackUOM', r.PackUOMId);
            $id('txtGrossWeight').value = fmtN(r.GrossWeight, 3); $id('txtEmptybagsUnit').value = fmtN(r.EmptyBags, 3); $id('txtEmptyBagsTotal').value = fmtN(r.EmptyBagsTotal, 3);
            $id('txtwtcut').value = fmtN(r.WeightCut, 3); $id('txtWeightCutTotal').value = fmtN(r.WeightCutTotal, 3); $id('txtAddLss').value = fmtN(r.AddLss, 3);
            $id('txtNetBillWeight').value = fmtN(r.NetBillWeight, 3); $id('txtStockWeight').value = fmtN(r.StockWeight, 3);
            $id('txtRate').value = fmtRate(r.Rate); setValue('comRateUOM', r.RateUOMId); $id('txtratecut').value = fmtRate(r.RateCut);
            $id('txtratecuttotal').value = fmtAmt(r.RateCutTotal); $id('txtAmount').value = fmtAmt(r.ItemAmount);
            setValue('comWarehouse', r.WarehouseId); $id('txtgpdate').value = shortDate(r.GpDate); $id('txtgatepassno').value = String(r.GpNo || ''); $id('txtvehicleno').value = r.VehicleNo || ''; $id('txtlabanalysisno').value = r.LabSampleNo || '';
            Total();   // TextChanged handlers recompute from the loaded values
            $id('btnAdd').classList.add('is-hidden'); $id('btnUpdateDetail').classList.remove('is-hidden'); $id('btnCancelUpdateDetial').classList.remove('is-hidden');
        });
    }
    function btnUpdateDetail_Click() {   // :1538
        try {
            modeCheck();
            if (!FormValidationDetail()) return;
            var b = branchCheck(); if (!b) return;
            var r = st.grid[st.updateDetailIndex]; if (!r) return;
            var packU = selRow('comPackUOM', st.uoms, 'Id'), rateU = selRow('comRateUOM', st.uoms, 'Id');
            r.WarehouseId = num($id('comWarehouse').value); r.Warehouse = selText('comWarehouse'); r.ItemId = num($id('comItem').value); r.Item = selText('comItem');
            r.CropYear = selText('comCropYear'); r.JobLotId = num($id('comjobLot').value); r.JobLot = selText('comjobLot'); r.PackingTypeId = num($id('comPackingType').value); r.PackingType = selText('comPackingType');
            r.ItemQty = toDouble($id('txtQty').value); r.PackUOMId = num($id('comPackUOM').value); r.PackUOM = selText('comPackUOM'); r.PackUOMEquivalent = packU ? toDouble(SC.ci(packU, 'Equivalent')) : 0;
            r.GrossWeight = toDouble($id('txtGrossWeight').value); r.EmptyBags = toDouble($id('txtEmptybagsUnit').value); r.EmptyBagsTotal = toDouble($id('txtEmptyBagsTotal').value);
            r.WeightCut = toDouble($id('txtwtcut').value); r.WeightCutTotal = toDouble($id('txtWeightCutTotal').value); r.AddLss = toDouble($id('txtAddLss').value); r.NetBillWeight = toDouble($id('txtNetBillWeight').value); r.StockWeight = toDouble($id('txtStockWeight').value);
            r.Rate = toDouble($id('txtRate').value); r.RateUOMId = num($id('comRateUOM').value); r.RateUOM = selText('comRateUOM'); r.RateEquivalent = rateU ? toDouble(SC.ci(rateU, 'Equivalent')) : 0;
            r.RateCut = toDouble($id('txtratecut').value); r.RateCutTotal = toDouble($id('txtratecuttotal').value); r.ItemAmount = toDouble($id('txtAmount').value);
            r.LabSampleNo = $id('txtlabanalysisno').value; r.GpDate = $id('txtgpdate').value || today(); r.GpNo = toInt($id('txtgatepassno').value); r.VehicleNo = $id('txtvehicleno').value;
            r.BranchId = b.id; r.BranchName = b.name;
            detailButtonsAdd();
            renderGrid(); TotalCommissionAmount(); FreightProportion(); CommissionProportion(); ExpProportion(); WagesAmountProportion(); BillAmount(); ResetDetail();
        } catch (e) { alertBox(e.message); }
    }
    function detailButtonsAdd() { $id('btnAdd').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden'); st.updateDetailIndex = -1; }
    function ResetDetail() {   // :2816 (Item, CropYear, JobLot, PackingType, PackUOM, GpDate are kept)
        ['txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtWeightCutTotal', 'txtAddLss', 'txtNetBillWeight', 'txtStockWeight', 'txtRate', 'txtratecut', 'txtratecuttotal', 'txtAmount', 'txtlabanalysisno', 'txtgatepassno', 'txtvehicleno'].forEach(function (id) { $id(id).value = ''; });
        $id('comRateUOM').value = ''; $id('comWarehouse').value = '';
        detailButtonsAdd(); $id('comItem').focus();
    }

    // ------------------------------------------------------------------ 6. calculations
    function sumGrid(key) { var s = 0; st.grid.forEach(function (r) { s += toDouble(r[key]); }); return s; }
    function TotalCommissionAmount() {   // :3742
        var rate = $id('txtcommrate').value, type = selText('combCommType');
        try { if (rate !== '' && type === 'Flat') $id('txtcommamount').value = String(toDouble(rate)); } catch (e) { }
        try { if (rate !== '' && (type === 'Percent' || type === 'Percentage')) $id('txtcommamount').value = String(sumGrid('ItemAmount') * toDouble(rate) / 100); } catch (e) { }
        try { if (rate !== '' && type === 'Comm Weight') $id('txtcommamount').value = String(sumGrid('NetBillWeight') / toDouble(selText('combcommUOM')) * toDouble(rate)); } catch (e) { }
        CommissionProportion();
    }
    function CommissionProportion() { var t = toDouble($id('txtcommamount').value), w = sumGrid('NetBillWeight'); st.grid.forEach(function (r) { r.Commission = t / w * toDouble(r.NetBillWeight); }); BillProportion(); }
    function ExpProportion() { var t = 0; st.invExp.forEach(function (e) { t += toDouble(e.Amount); }); var w = sumGrid('NetBillWeight'); st.grid.forEach(function (r) { r.Expense = t / w * toDouble(r.NetBillWeight); }); BillProportion(); }
    function FreightProportion() { var w = sumGrid('NetBillWeight'), c = 0; st.freight.forEach(function (f) { c += toDouble(f.Freight); }); st.grid.forEach(function (r) { r.Freights = c > 0 ? c / w * toDouble(r.NetBillWeight) : 0; }); BillProportion(); }
    function WagesAmountProportion() {
        var wg = toDouble($id('txtWagesAmountHeader').value);
        if (wg > 0) { var q = sumGrid('ItemQty'), w = sumGrid('NetBillWeight'); st.grid.forEach(function (r) { r.Wages = st.flags.wagesAmountCalculateOnQty ? wg / q * toDouble(r.ItemQty) : wg / w * toDouble(r.NetBillWeight); }); }
        else st.grid.forEach(function (r) { r.Wages = 0; });
        BillProportion();
    }
    function BillProportion() {
        st.grid.forEach(function (r) {
            r.BillAmount = toDouble(r.ItemAmount) + toDouble(r.Expense) + toDouble(r.Freights) - toDouble(r.Commission) - toDouble(r.EbPurAgainstWeightAmount) + (st.flags.contractWagesChargetoProduct ? toDouble(r.Wages) : 0);
        });
        renderGridValues();
    }
    function BillAmount() {   // :2885
        try {
            var item = sumGrid('ItemAmount'), exp = 0; st.invExp.forEach(function (e) { exp += toDouble(e.Amount); });
            var jDr = 0, jCr = 0;
            st.gl.forEach(function (g) { var a = parseInt(String(g.AccountId == null ? '' : g.AccountId), 10); if (isNaN(a)) throw new Error('Input string was not in a correct format.'); if (a > 0) { jDr += toDouble(g.Debit); jCr += toDouble(g.Credit); } });
            var cust = num($id('comsupplier').value), party = selRow('comsupplier', st.look.parties, 'Id'), tCr = 0, tDr = 0;
            st.freight.forEach(function (f) {
                if (toInt(f.Transporter) > 0 || toInt(f.GlAccountId) > 0) {
                    var same = st.flags.subsidiaryAccountAllownOnVouchers ? cust === toInt(f.Transporter) : cust > 0 && toInt(party ? SC.ci(party, 'PartyCode') : 0) === toInt(f.GlAccountId);   // Q-B1
                    if (same) { tCr += toDouble(f.Freight); tDr += toDouble(f.Debit); }
                }
            });
            var eb = 0; st.emptyBags.forEach(function (b) { if (toDouble(b.Amount) > 0 && toInt(b.TypeId) === 1) eb += toDouble(b.Amount); });
            var bill = item + exp + eb + jDr - jCr + tCr - tDr;
            if (cust === num($id('combcommAgent').value)) bill -= toDouble($id('txtcommamount').value);   // Q-B2
            $id('txtBillAmount').value = fmtAmt(afz(bill, st.N));
            BillProportion();
        } catch (e) { alertBox(e.message); }
    }
    function CalculateEbPurAgainstWeightAmountBasedOnBalQty() {   // :4694
        var anyCredit = st.emptyBags.some(function (b) { return (toInt(b.TypeId) === 2 || toInt(b.TypeId) === 3) && toInt(b.CreditAccountId) > 0; });
        var a = 0, q = 0; st.emptyBags.forEach(function (b) { if (toInt(b.TypeId) === 2 || toInt(b.TypeId) === 3) { a += toDouble(b.Amount); q += toDouble(b.Qty); } });
        if (anyCredit || q <= 0 || a <= 0) { st.grid.forEach(function (r) { r.EbPurAgainstWeightAmount = 0; }); return false; }
        var per = a / q, balQ = q, balA = a;
        for (var i = 0; i < st.grid.length; i++) { var used = Math.min(toDouble(st.grid[i].ItemQty), balQ), e = per * used; st.grid[i].EbPurAgainstWeightAmount = e; balQ -= used; balA -= e; if (balA <= 0) break; }
        return true;
    }

    // ------------------------------------------------------------------ 4. grids
    var GRID_COLS = [
        ['Warehouse', 's'], ['Item', 's'], ['CropYear', 's'], ['JobLot', 's'], ['PackingType', 's'], ['PackUOM', 's'], ['ItemQty', 'n2'], ['GrossWeight', 'n2'], ['EmptyBags', 'n2'], ['EmptyBagsTotal', 'n2'],
        ['WeightCut', 'n2'], ['WeightCutTotal', 'n2'], ['AddLss', 'n2'], ['NetBillWeight', 'n2'], ['StockWeight', 'n2'], ['Rate', 'r'], ['RateUOM', 's'], ['RateEquivalent', 'r'], ['RateCut', 'r'], ['RateCutTotal', 'r'],
        ['ItemAmount', 'a'], ['LabSampleNo', 's'], ['GpDate', 'd'], ['GpNo', 'i'], ['VehicleNo', 's'], ['BillAmount', 'a'], ['Expense', 'n3'], ['Freights', 'n3'], ['Journal', 'n3'], ['Commission', 'n3'], ['Wages', 'n2'],
        ['EbPurAgainstWeightAmount', 'a'], ['BranchName', 's'], ['GrnNo', 'i'], ['RefDocumentType', 's'], ['RefDocNo', 'i']
    ];
    var SUMS = { ItemAmount: 1, BillAmount: 1, EbPurAgainstWeightAmount: 1, Expense: 1, Freights: 1, Journal: 1, Commission: 1, ItemQty: 1, GrossWeight: 1, EmptyBags: 1, EmptyBagsTotal: 1, WeightCut: 1, WeightCutTotal: 1, AddLss: 1, NetBillWeight: 1, StockWeight: 1, Wages: 1, GpNo: 0 };
    function gridMode() { var r = st.grid[0]; if (!r) return 'manual'; if (num(r.GrnId) > 0) return 'grn'; if (num(r.RefDocId) > 0) return 'invoice'; return 'manual'; }
    function fmtCell(kind, v) {
        switch (kind) { case 'n2': return fmtN(toDouble(v), 2); case 'n3': return fmtN(toDouble(v), 3); case 'r': return fmtRate(toDouble(v)); case 'a': return fmtAmt(toDouble(v)); case 'd': return gridDate(v); case 'i': return v == null ? '' : String(v); default: return v == null ? '' : String(v); }
    }
    function visibleCols() {
        var mode = gridMode();
        return GRID_COLS.filter(function (c) {
            var k = c[0];
            if (k === 'BranchName') return st.flags.branchFeature && !st.flags.branchImplemented;
            if (k === 'GrnNo') return mode === 'grn';
            if (k === 'RateEquivalent') return mode !== 'manual';
            if (k === 'RateUOM') return mode === 'manual';
            if (k === 'RefDocumentType' || k === 'RefDocNo') return mode === 'invoice';
            return true;
        });
    }
    var EDIT_GRN = { Rate: 1, RateEquivalent: 1, RateCut: 1 };
    var EDIT_INV = { Rate: 1, RateEquivalent: 1, ItemQty: 1, GrossWeight: 1, EmptyBags: 1, WeightCut: 1, AddLss: 1, RateCut: 1, LabSampleNo: 1, GpNo: 1, VehicleNo: 1, GpDate: 1 };
    function renderGrid() {
        var cols = visibleCols(), mode = gridMode(), editable = mode === 'grn' ? EDIT_GRN : mode === 'invoice' ? EDIT_INV : {};
        var t = $id('grd');
        t.querySelector('thead').innerHTML = '<tr>' + (mode !== 'grn' ? '<th style="width:20px">Delete</th>' : '') + cols.map(function (c) { return '<th>' + esc(c[0] === 'RateEquivalent' ? 'Rate Uom' : c[0] === 'EbPurAgainstWeightAmount' ? 'Empty Bags (Charge to Product)' : c[0]) + '</th>'; }).join('') + '</tr>';
        t.querySelector('tbody').innerHTML = st.grid.map(function (r, i) {
            return '<tr data-i="' + i + '">' + (mode !== 'grn' ? '<td class="c"><button type="button" class="x" data-del="' + i + '">X</button></td>' : '') + cols.map(function (c) {
                var k = c[0], kind = c[1], cls = kind === 's' || kind === 'd' ? '' : kind === 'i' ? 'c' : 'n';
                if (editable[k]) return '<td><input class="' + (cls === 'n' ? 'n' : '') + '" data-edit="' + k + '" data-i="' + i + '" type="' + (kind === 'd' ? 'date' : 'text') + '" value="' + esc(kind === 'd' ? shortDate(r[k]) : kind === 's' ? r[k] : String(toDouble(r[k]))) + '"></td>';
                return '<td class="' + cls + '" data-k="' + k + '">' + esc(fmtCell(kind, r[k])) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        renderGridValues();
        groupBoxState();
    }
    function renderGridValues() {
        var cols = visibleCols(), mode = gridMode();
        var body = $id('grd').querySelector('tbody');
        st.grid.forEach(function (r, i) { var tr = body.children[i]; if (!tr) return; cols.forEach(function (c) { var td = tr.querySelector('td[data-k="' + c[0] + '"]'); if (td) td.textContent = fmtCell(c[1], r[c[0]]); }); });
        $id('grd').querySelector('tfoot').innerHTML = st.grid.length ? '<tr>' + (mode !== 'grn' ? '<td></td>' : '') + cols.map(function (c) {
            if (!SUMS[c[0]]) return '<td></td>'; var s = 0; st.grid.forEach(function (r) { s += toDouble(r[c[0]]); }); return '<td>' + esc(fmtCell(c[1] === 'r' ? 'r' : c[1] === 'a' ? 'a' : c[1] === 'n3' ? 'n3' : 'n2', s)) + '</td>';
        }).join('') + '</tr>' : '';
    }
    function groupBoxState() {
        var loaded = gridMode() !== 'manual';
        $id('groupBox1').classList.toggle('is-hidden', loaded); $id('panel3').style.height = loaded ? '154px' : '274px';
    }
    function grd_CellUpdated(i, col, value) {   // :4820 (loaded modes only)
        var r = st.grid[i]; if (!r) return;
        if (col === 'GpDate' || col === 'LabSampleNo' || col === 'VehicleNo') r[col] = value; else if (col === 'GpNo') r[col] = toInt(value); else r[col] = toDouble(value);
        var eq = toDouble(r.RateEquivalent), net;
        if (col === 'GrossWeight') { net = toDouble(r.GrossWeight) - toDouble(r.EmptyBagsTotal) - toDouble(r.WeightCutTotal) + toDouble(r.AddLss); r.NetBillWeight = net; r.StockWeight = net; r.ItemAmount = afz(net / eq * toDouble(r.Rate) - toDouble(r.RateCutTotal), 4); }
        else if (col === 'EmptyBags' || col === 'WeightCut' || col === 'ItemQty') {
            var qty = toDouble(r.ItemQty), gross = qty * toDouble(r.PackUOMEquivalent), ebt = qty * toDouble(r.EmptyBags), wct = qty * toDouble(r.WeightCut);
            r.GrossWeight = gross; r.EmptyBagsTotal = toDouble(r.WeightCut); /* Q-G1 */ r.WeightCutTotal = wct;
            net = gross - ebt - wct + toDouble(r.AddLss); r.NetBillWeight = net; r.StockWeight = net; r.ItemAmount = afz(net / eq * toDouble(r.Rate) - toDouble(r.RateCutTotal), 4);
        }
        else if (col === 'AddLss') { net = toDouble(r.GrossWeight) - toDouble(r.EmptyBagsTotal) - toDouble(r.WeightCutTotal) + toDouble(r.AddLss); r.NetBillWeight = net; r.StockWeight = net; r.ItemAmount = afz(net / eq * toDouble(r.Rate) - toDouble(r.RateCutTotal), 4); }
        else if (col === 'Rate' || col === 'RateCut' || col === 'RateEquivalent') { net = toDouble(r.NetBillWeight); r.RateCutTotal = net / eq * toDouble(r.RateCut); r.ItemAmount = afz(net / eq * toDouble(r.Rate) - toDouble(r.RateCutTotal), 4); }
        CalculateEbPurAgainstWeightAmountBasedOnBalQty(); BillAmount();
    }
    function DeleteDetailrow(i) {   // :5221
        var r = st.grid[i]; if (!r) return;
        var done = function () { st.grid.splice(i, 1); renderGrid(); if (!st.grid.length) { $id('comsupplier').disabled = false; groupBoxState(); } };   // no recalculation (Q-G3)
        if (num(r.Id) > 0) {
            if (!confirm('Are you sure you want to delete this record?')) return;
            post(API + '/' + st.recId + '/detail/' + num(r.Id) + '/check-delete', {}).then(done).catch(function (e) { alertBox(e.message); });
        } else done();
    }

    // sub-grids (4.2-4.5) -------------------------------------------------
    function accountOptions(rows) { var sub = st.flags.subsidiaryAccountAllownOnVouchers; return '<option value="0"></option>' + (rows || []).map(function (a) { return '<option value="' + esc(sub ? a.SupplierCustomerId : a.Id) + '">' + esc(a.AccountTitle) + '</option>'; }).join(''); }
    function lookupById(rows, id) { var sub = st.flags.subsidiaryAccountAllownOnVouchers; return (rows || []).find(function (a) { return String(sub ? a.SupplierCustomerId : a.Id) === String(id); }); }
    function renderInvExp() {
        if (!st.invExp.length) st.invExp.push({ ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' });
        var t = $id('grdInvExp');
        t.querySelector('thead').innerHTML = '<tr><th style="min-width:250px">Item</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th><th>Delete</th><th>Add</th></tr>';
        t.querySelector('tbody').innerHTML = st.invExp.map(function (e, i) {
            return '<tr><td><select class="win-combo" data-g="invExp" data-i="' + i + '" data-k="ItemId"><option value="0"></option>' + (st.look.otherItems || []).map(function (o) { return '<option value="' + esc(SC.ci(o, 'Id')) + '"' + (String(SC.ci(o, 'Id')) === String(e.ItemId) ? ' selected' : '') + '>' + esc(SC.ci(o, 'OtherItemName')) + '</option>'; }).join('') + '</select></td>'
                + ['Qty', 'Rate', 'Amount'].map(function (k) { return '<td><input class="n" data-g="invExp" data-i="' + i + '" data-k="' + k + '" value="' + esc(e[k]) + '"></td>'; }).join('')
                + '<td><input class="wide" data-g="invExp" data-i="' + i + '" data-k="Remarks" value="' + esc(e.Remarks) + '"></td>'
                + '<td class="c"><button type="button" class="x" data-gdel="invExp" data-i="' + i + '">X</button></td><td class="c"><button type="button" class="x" data-gadd="invExp">+</button></td></tr>';
        }).join('');
        var s = 0; st.invExp.forEach(function (e) { s += toDouble(e.Amount); });
        t.querySelector('tfoot').innerHTML = '<tr><td></td><td></td><td></td><td>' + esc(fmtN(s, 2)) + '</td><td></td><td></td><td></td></tr>';
    }
    function renderGL() {
        if (!st.gl.length) st.gl.push({ AccountId: 0, GlAccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 });
        var t = $id('grdGLedger');
        t.querySelector('thead').innerHTML = '<tr><th style="min-width:250px">Account</th><th>Remarks</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Debit</th><th>Credit</th><th>Delete</th><th>Add</th></tr>';
        t.querySelector('tbody').innerHTML = st.gl.map(function (g, i) {
            return '<tr><td><select class="win-combo" data-g="gl" data-i="' + i + '" data-k="AccountId">' + accountOptions(st.look.journalAccounts) + '</select></td>'
                + '<td><input class="wide" data-g="gl" data-i="' + i + '" data-k="Remarks" value="' + esc(g.Remarks) + '"></td>'
                + ['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'].map(function (k) { return '<td><input class="n" data-g="gl" data-i="' + i + '" data-k="' + k + '" value="' + esc(g[k]) + '"></td>'; }).join('')
                + '<td class="c"><button type="button" class="x" data-gdel="gl" data-i="' + i + '">X</button></td><td class="c"><button type="button" class="x" data-gadd="gl">+</button></td></tr>';
        }).join('');
        st.gl.forEach(function (g, i) { var sel = t.querySelector('select[data-i="' + i + '"]'); if (sel) sel.value = String(g.AccountId || 0); });
        var d = 0, c = 0; st.gl.forEach(function (g) { d += toDouble(g.Debit); c += toDouble(g.Credit); });
        t.querySelector('tfoot').innerHTML = '<tr><td></td><td></td><td></td><td></td><td></td><td>' + esc(fmtN(d, 2)) + '</td><td>' + esc(fmtN(c, 2)) + '</td><td></td><td></td></tr>';
    }
    function renderFreight() {
        if (!st.freight.length) st.freight.push({ GrnId: 0, Transporter: 0, GlAccountId: 0, Freight: 0, Debit: 0, Remarks: '' });
        var t = $id('grdFreight'), debitVisible = !!st.flags.freightDebitToExpenses;
        t.querySelector('thead').innerHTML = '<tr><th style="min-width:250px">Transporter</th><th>Credit</th>' + (debitVisible ? '<th>Debit</th>' : '') + '<th>Remarks</th><th style="width:20px">Delete</th><th style="width:20px">Add</th></tr>';
        t.querySelector('tbody').innerHTML = st.freight.map(function (f, i) {
            return '<tr><td><select class="win-combo" data-g="freight" data-i="' + i + '" data-k="Transporter">' + accountOptions(st.look.freightAccounts) + '</select></td>'
                + '<td><input class="n" data-g="freight" data-i="' + i + '" data-k="Freight" value="' + esc(f.Freight) + '"></td>'
                + (debitVisible ? '<td><input class="n" data-g="freight" data-i="' + i + '" data-k="Debit" value="' + esc(f.Debit) + '"></td>' : '')
                + '<td><input class="wide" data-g="freight" data-i="' + i + '" data-k="Remarks" value="' + esc(f.Remarks) + '"></td>'
                + '<td class="c"><button type="button" class="x" data-gdel="freight" data-i="' + i + '">X</button></td><td class="c"><button type="button" class="x" data-gadd="freight">+</button></td></tr>';
        }).join('');
        st.freight.forEach(function (f, i) { var sel = t.querySelector('select[data-i="' + i + '"]'); if (sel) sel.value = String(f.Transporter || 0); });
        var c = 0, d = 0; st.freight.forEach(function (f) { c += toDouble(f.Freight); d += toDouble(f.Debit); });
        t.querySelector('tfoot').innerHTML = '<tr><td></td><td>' + esc(fmtAmt(c)) + '</td>' + (debitVisible ? '<td>' + esc(fmtAmt(d)) + '</td>' : '') + '<td></td><td></td><td></td></tr>';
    }
    function renderEmptyBags() {
        var t = $id('grdEmptyBags');
        t.querySelector('thead').innerHTML = '<tr><th>Type</th><th>ItemName</th><th>Qty</th><th>Rate</th><th>Amount</th><th>Remarks</th><th style="min-width:220px">Credit account</th></tr>';
        t.querySelector('tbody').innerHTML = st.emptyBags.map(function (b, i) {
            return '<tr><td>' + esc(b.Type) + '</td><td>' + esc(b.ItemName) + '</td><td class="n">' + esc(fmtN(toDouble(b.Qty), 3)) + '</td>'
                + '<td><input class="n" data-g="emptyBags" data-i="' + i + '" data-k="Rate" value="' + esc(b.Rate) + '"></td><td class="n">' + esc(fmtN(toDouble(b.Amount), 4)) + '</td>'
                + '<td><input class="wide" data-g="emptyBags" data-i="' + i + '" data-k="Remarks" value="' + esc(b.Remarks) + '"></td>'
                + '<td><select class="win-combo" data-g="emptyBags" data-i="' + i + '" data-k="CreditAccountId"><option value="0"></option>' + (st.look.emptyBagCreditAccounts || []).map(function (a) { return '<option value="' + esc(a.Id) + '"' + (String(a.Id) === String(b.CreditAccountId) ? ' selected' : '') + '>' + esc(a.AccountTitle) + '</option>'; }).join('') + '</select></td></tr>';
        }).join('');
        var q = 0, a = 0; st.emptyBags.forEach(function (b) { q += toDouble(b.Qty); a += toDouble(b.Amount); });
        t.querySelector('tfoot').innerHTML = st.emptyBags.length ? '<tr><td></td><td></td><td>' + esc(fmtN(q, 3)) + '</td><td></td><td>' + esc(fmtN(a, 4)) + '</td><td></td><td></td></tr>' : '';
    }
    function numericWarning(v) { if (v !== '' && !isFinite(Number(String(v).replace(/,/g, '')))) alertBox('Please Type Only Numeric Value'); }   // UpdatingCell - shown, edit not cancelled
    function subGridEdit(g, i, k, value) {
        var row = st[g][i]; if (!row) return;
        if (g === 'invExp') {   // :1866
            if (['Qty', 'Rate', 'Amount'].indexOf(k) >= 0) numericWarning(value);
            row[k] = k === 'ItemId' ? toInt(value) : k === 'Remarks' ? value : value;
            if ((k === 'Qty' || k === 'Rate') && String(row.Qty) !== '' && String(row.Rate) !== '') row.Amount = afz(toDouble(row.Qty) * toDouble(row.Rate), st.N);
            renderInvExp(); BillAmount(); ExpProportion();
        } else if (g === 'gl') {   // :1968
            if (['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'].indexOf(k) >= 0) numericWarning(value);
            row[k] = k === 'Remarks' ? value : value;
            if ((k === 'Qty' || k === 'Rate') && String(row.Qty) !== '' && String(row.Rate) !== '') { row.Credit = afz(toDouble(row.Qty) * toDouble(row.Rate), st.N); row.Debit = 0; row.Percentage = 0; }
            if (k === 'Percentage' && String(row.Percentage) !== '') { var t = sumGrid('ItemAmount') / 100 * toDouble(row.Percentage); if (t > 0) { row.Credit = afz(t, st.N); row.Debit = 0; } else { row.Debit = afz(Math.abs(t), st.N); row.Credit = 0; } row.Qty = 0; row.Rate = 0; }
            if (k === 'Credit') { row.Credit = afz(toDouble(row.Credit), st.N); if (toDouble(row.Debit) > 0) { row.Credit = 0; alertBox('Debit Side is aleady added'); } }
            if (k === 'Debit') { row.Debit = afz(toDouble(row.Debit), st.N); if (toDouble(row.Credit) > 0) { row.Debit = 0; alertBox('Credit Side is aleady added'); } }
            if (k === 'AccountId') SupCustIdUpdate(row, st.look.journalAccounts, 'AccountId');
            renderGL(); BillAmount();
        } else if (g === 'freight') {   // :2139
            if (k === 'Freight') numericWarning(value);
            row[k] = value;
            if (k === 'Transporter') SupCustIdUpdate(row, st.look.freightAccounts, 'Transporter');
            if (k === 'Freight') row.Freight = afz(toDouble(row.Freight), st.N);
            if (st.flags.freightDebitToExpenses) {
                if (k === 'Freight' && toDouble(row.Debit) > 0) { row.Freight = 0; alertBox('Debit Side is aleady added'); }
                if (k === 'Debit') { row.Debit = afz(toDouble(row.Debit), st.N); if (toDouble(row.Freight) > 0) { row.Debit = 0; alertBox('Credit Side is aleady added'); } }
            }
            renderFreight(); BillAmount(); FreightProportion();
        } else if (g === 'emptyBags') {   // :4595
            if (k === 'Rate') numericWarning(value);
            row[k] = k === 'CreditAccountId' ? toInt(value) : value;
            if (k === 'Rate' && String(row.Qty) !== '' && String(row.Rate) !== '') row.Amount = toDouble(row.Qty) * toDouble(row.Rate);
            renderEmptyBags(); CalculateEbPurAgainstWeightAmountBasedOnBalQty(); BillAmount();
        }
    }
    function SupCustIdUpdate(row, accounts, col) {   // :5021 SupCustIdUpdateforGLGrid / Freight
        var picked = lookupById(accounts, row[col]);
        row.GlAccountId = picked ? toDouble(picked.Id) : 0;
        if (!st.flags.subsidiaryAccountAllownOnVouchers) {
            if (toInt(st.supplierGlId) === toInt(row.GlAccountId)) { alertBox('Selected Account Can not be Same As Supplier Account'); row.GlAccountId = 0; row[col] = 0; }   // Q-B3
        } else if (num($id('comsupplier').value) === toInt(row[col])) { alertBox('Selected Account Can not be Same As Supplier Account'); row[col] = 0; row.GlAccountId = 0; }
    }
    function subGridDelete(g, i) {
        if (g === 'gl' || g === 'freight') { if (!confirm('Are you sure to Delete?')) return; }
        st[g].splice(i, 1);
        if (g === 'invExp') { renderInvExp(); BillAmount(); } else if (g === 'gl') { renderGL(); BillAmount(); } else { renderFreight(); BillAmount(); }
    }
    function subGridAdd(g) {
        if (g === 'invExp') { st.invExp.push({ ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }); renderInvExp(); }
        else if (g === 'gl') { st.gl.push({ AccountId: 0, GlAccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }); renderGL(); }
        else { st.freight.push({ GrnId: 0, Transporter: 0, GlAccountId: 0, Freight: 0, Debit: 0, Remarks: '' }); renderFreight(); }
        BillAmount();
    }

    // ------------------------------------------------------------------ 5.7 header events
    function comsupplier_Leave() {   // ItemsBindByParty :1071
        var id = num($id('comsupplier').value);
        if (!id) { fill('comItem', [], 'Id', 'ItemName'); return Promise.resolve(); }
        return get(API + '/items-by-party?partyId=' + id).then(function (rows) { fill('comItem', rows || [], 'Id', 'ItemName'); }).catch(function (e) { msg(e.message, true); });
    }
    function combpttrm_TextChanged() {   // :5247
        var cash = selText('combpttrm') === 'Cash';
        $id('txtduedays').disabled = cash; $id('duedate').disabled = cash;
        if (cash) { $id('txtduedays').value = ''; $id('duedate').value = $id('DocDate').value; }
    }
    function txtduedays_TextChanged() {   // :3971
        var d = $id('txtduedays').value, doc = $id('DocDate').value || today();
        $id('duedate').value = d !== '' ? addDays(doc, Math.trunc(toDouble(d))) : doc;
    }
    function duedate_ValueChanged() {   // :5270
        var doc = $id('DocDate').value || today(); if (!$id('duedate').value) $id('duedate').value = doc;
        if ($id('duedate').value < doc) $id('duedate').value = doc;
        $id('txtduedays').value = String(daysBetween(doc, $id('duedate').value));
    }

    // ------------------------------------------------------------------ 7. save
    function Insert() {
        try {
            if ($id('txtdocno').value === '' || $id('txtdocno').value === '0') { alertBox('DocNo Field is Required'); $id('txtdocno').focus(); return; }
            if ($id('comsupplier').value === '') { alertBox('Supplier Field is Required'); $id('comsupplier').focus(); return; }
            if (!confirm(st.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var payload = {
                Id: st.recId, DocNo: $id('txtdocno').value, BranchSrNo: $id('txtBranchSrNo').value, DocDate: $id('DocDate').value, SupplierCustomerId: num($id('comsupplier').value),
                ManualBillNo: $id('txtbillno').value.trim(), SupplierReferenceNo: $id('txtSupplierReference').value.trim(), PaymentTermsId: num($id('combpttrm').value),
                DueDays: toInt($id('txtduedays').value), DueDate: $id('duedate').value, CommAmount: toDouble($id('txtcommamount').value), CommissionCreditAccountId: num($id('CmbCommissionDebitAccount').value),
                CommissionAgentId: num($id('combcommAgent').value), CommissionRemarks: $id('txtCommissionRemarks').value.trim(), CommissionType: selText('combCommType').trim(), CommRate: toDouble($id('txtcommrate').value),
                UomScheduleIdCmRate: selText('combcommUOM'), WagesAmount: toDouble($id('txtWagesAmountHeader').value), RemarksHeader: $id('txtremarks').value.trim(), CustomAccounts: $id('chkCustomAccounts').checked,
                details: st.grid, freight: st.freight, journal: st.gl, expenses: st.invExp, emptyBags: st.emptyBags, attachments: attachmentEditor.payload()
            };
            var btn = st.recId > 0 ? $id('btnUpdate') : $id('btnSave');
            SC.withBusy(btn, function () {
                return post(API + '/save', payload).then(function (res) {
                    var savedId = num(res.id), voucherId = num(res.voucherHeadId);
                    alertBox(res.message);
                    if (res.openWagesBill) msg('Contractor wages bill (frmwagesBillHeader, RefDocTypeId 98, RefDocId ' + savedId + ', GrossWeight ' + res.grossWeightTotal + ') is a separate screen - open Contractor Wages to enter it.', false);
                    var slip = $id('ChkBok').checked, voucher = $id('chkVouchrePrint').checked;
                    Reset();
                    if (slip) printRpt('98A_SaleInvoiceReturnPartySlip.rpt', { id: savedId });
                    if (voucher) printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: voucherId, documentTypeId: 98 });
                });
            }).catch(function (e) { alertBox(e.message); });
        } catch (e) { alertBox(e.message); }
    }
    function btnSave_Click() { st.recId = 0; Insert(); }
    function btnUpdate_Click() { if (st.approved) { alertBox('Record Not Update because Record has approved'); return; } Insert(); }
    function btnDelete_Click() {   // :4030
        try {
            if (st.approved) throw new Error('Record Not Delete because Record has approved');
            if (st.recId === 0) throw new Error('RecordId not found for delete');
            if (!confirm('Are you sure to Delete?')) return;
            SC.withBusy($id('btnDelete'), function () {
                return SC.getJson ? fetch(API + '/' + st.recId, { method: 'DELETE', credentials: 'same-origin', headers: { Accept: 'application/json' } }).then(function (r) { return r.json().then(function (b) { if (!r.ok) throw new Error(b.message || 'Delete failed'); return b; }); }) : null;
            }).then(function (b) { if (b) { alertBox('Delete Record Successfully'); Reset(); } }).catch(function (e) { alertBox(e.message); });
        } catch (e) { alertBox(e.message); }
    }

    // ------------------------------------------------------------------ 9.2 Reset
    function Reset() {
        attachmentEditor.reset();
        $id('groupBox1').classList.remove('is-hidden'); $id('panel3').style.height = '274px';
        st.recId = 0; st.voucherHeadId = 0; st.approved = false; $id('lblRecId').textContent = '';
        $id('comsupplier').value = ''; $id('comsupplier').disabled = false;
        ['txtSupplierReference', 'txtbillno', 'txtCommissionRemarks', 'txtcommrate', 'txtcommamount', 'txtremarks', 'txtduedays', 'txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtWeightCutTotal', 'txtAddLss', 'txtNetBillWeight', 'txtStockWeight', 'txtRate', 'txtratecut', 'txtratecuttotal', 'txtAmount', 'txtlabanalysisno', 'txtgatepassno', 'txtvehicleno', 'txtBillAmount', 'txtWagesAmountHeader'].forEach(function (id) { $id(id).value = ''; });
        ['combcommAgent', 'combCommType', 'combcommUOM', 'comCropYear', 'comjobLot', 'comPackingType', 'comPackUOM', 'comRateUOM', 'comWarehouse'].forEach(function (id) { $id(id).value = ''; });
        fill('comItem', [], 'Id', 'ItemName'); st.uoms = [];
        st.freight = []; st.invExp = []; st.gl = []; st.grid = []; st.emptyBags = [];
        renderGL(); renderInvExp(); renderFreight(); renderEmptyBags(); renderGrid();
        DocumentNo();
        $id('btnSave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden'); $id('btnDelete').classList.add('is-hidden');
        $id('chkCustomAccounts').checked = false;
        $id('DocDate').focus();
        /* not reset: DocDate, combpttrm, combdeliverytrm, CmbCommissionDebitAccount, duedate, txtgpdate, chkVouchrePrint, ChkBok (Q-R4) */
    }
    function DocumentNo() { return get(API + '/numbers').then(function (n) { $id('txtdocno').value = String(n.docNo); $id('txtBranchSrNo').value = String(n.branchSrNo); }).catch(function (e) { msg(e.message, true); }); }

    // ------------------------------------------------------------------ 8. ReadById
    function ReadById(id) {
        return get(API + '/' + id).then(function (h) {
            var c = function (k) { return SC.ci(h, k); };
            st.recId = id; tab('tabForm');
            $id('txtdocno').value = String(c('DocNo') == null ? '' : c('DocNo')); $id('txtBranchSrNo').value = String(c('BranchSrNo') == null ? '' : c('BranchSrNo')); $id('DocDate').value = shortDate(c('DocDate'));
            setValue('comsupplier', c('SupplierCustomerId'));
            return comsupplier_Leave().then(function () {
                $id('comsupplier').disabled = true;
                $id('txtSupplierReference').value = c('SupplierReferenceNo') || ''; $id('txtbillno').value = c('ManualBillNo') || '';
                if (num(c('CommissionAgentId')) > 0) setValue('combcommAgent', c('CommissionAgentId')); else $id('combcommAgent').value = '';
                setValue('CmbCommissionDebitAccount', c('CommissionCreditAccountId'));
                setText('combCommType', c('CommissionType')); $id('txtcommrate').value = fmtRate(toDouble(c('CommRate'))); setText('combcommUOM', c('UomScheduleIdCmRate'));
                $id('txtcommamount').value = fmtN(toDouble(c('CommAmount')), 3); $id('txtCommissionRemarks').value = c('CommissionRemarks') || ''; $id('txtremarks').value = c('RemarksHeader') || '';
                $id('txtBillAmount').value = fmtAmt(toDouble(c('BillAmount'))); setValue('combpttrm', c('PaymentTermsId')); combpttrm_TextChanged();
                $id('txtduedays').value = c('DueDays') == null ? '' : String(c('DueDays')); $id('duedate').value = shortDate(c('DueDate')) || $id('DocDate').value;
                setText('combdeliverytrm', c('DeliveryTerm')); $id('chkCustomAccounts').checked = !!c('CustomAccounts');
                $id('txtWagesAmountHeader').value = fmtN(toDouble(c('WagesAmount')), 4);
                st.approved = !!c('IsApproved'); st.voucherHeadId = num(c('voucherHeadId')); $id('lblRecId').textContent = 'Rec Id: ' + id + (st.approved ? '  (Approved)' : '');
                // grid rows (52 values, :2660-2697); EBTotalWt / WeightCutTotal are Math.Round-ed to integers (Q-R2)
                st.grid = (c('details') || []).map(function (d) {
                    var v = function (k) { return SC.ci(d, k); }; var r = blankRow();
                    r.Id = num(v('Id')); r.GrnId = num(v('InvGrnId')); r.GrnDetailId = num(v('InvGrnDetailId')); r.GrnNo = num(v('GrnNo')); r.WarehouseId = num(v('WarehouseId')); r.Warehouse = v('WareHouseName') || '';
                    r.ItemId = num(v('ItemId')); r.Item = v('ItemName') || ''; r.CropYear = v('CropYear') || ''; r.JobLotId = num(v('JobLotId')); r.JobLot = v('JobLotDescription') || ''; r.PackingTypeId = num(v('PackingTypeId')); r.PackingType = v('PackTypeDesc') || '';
                    r.PackUOMId = num(v('ItemUOMId')); r.PackUOM = v('UOMCodeItem') || ''; r.PackUOMEquivalent = toDouble(v('ItemUOMEquivalent')); r.ItemQty = toDouble(v('ItemQty')); r.GrossWeight = toDouble(v('GrossWeight'));
                    r.EmptyBags = toDouble(v('EBWeight')); r.EmptyBagsTotal = bankers(toDouble(v('EBTotalWt'))); r.WeightCut = toDouble(v('WeightCut')); r.WeightCutTotal = bankers(toDouble(v('WeightCutTotal')));
                    r.AddLss = toDouble(v('AdLsWeight')); r.NetBillWeight = toDouble(v('NetBillWeight')); r.StockWeight = toDouble(v('NetStockWeight')); r.Rate = toDouble(v('ItemRate')); r.RateUOMId = num(v('UomScheduleIdRate')); r.RateUOM = v('RateUom') || '';
                    r.RateEquivalent = toDouble(v('EquivalentPoRate')); r.RateCut = toDouble(v('RateCut')); r.RateCutTotal = toDouble(v('RateCutAmount')); r.ItemAmount = toDouble(v('ItemAmount')); r.LabSampleNo = v('LabAnalisysNo') || '';
                    r.GpDate = shortDate(v('GpDate')); r.GpNo = num(v('GpNo')); r.VehicleNo = v('VehicleNo') || ''; r.BillAmount = toDouble(v('BillAmount')); r.Expense = toDouble(v('ExpenseAmount')); r.Freights = toDouble(v('FreightAmount'));
                    r.Journal = toDouble(v('JournalAmount')); r.Commission = toDouble(v('CommissionAmount')); r.Wages = toDouble(v('WagesAmount')); r.EbPurAgainstWeightAmount = toDouble(v('EbPurAgainstWeightAmount'));
                    r.BranchId = num(v('BranchId')); r.BranchName = v('BranchName') || ''; r.RefDocumentTypeId = num(v('RefDocumentTypeId')); r.RefDocumentType = v('RefDocumentType') || ''; r.RefDocId = num(v('RefDocId')); r.RefDocNo = toInt(v('RefDocNo'));
                    r.RefDocSubId = num(v('RefDocSubId')); r.RefInvoiceQty = toDouble(v('RefInvoiceQty')); r.RefInvoiceWeight = toDouble(v('RefInvoiceWeight'));
                    return r;
                });
                st.invExp = (c('expenses') || []).map(function (e) { var v = function (k) { return SC.ci(e, k); }; return { Id: num(v('Id')), ItemId: num(v('InvRevExpItemId')), Qty: toDouble(v('Qty')), Rate: toDouble(v('Rate')), Amount: toDouble(v('Amount')), Remarks: v('Remarks') || '' }; });
                var sub = st.flags.subsidiaryAccountAllownOnVouchers;
                st.gl = (c('journal') || []).map(function (j) { var v = function (k) { return SC.ci(j, k); }; return { Id: num(v('Id')), AccountId: sub ? num(v('SupplierCustomerId')) : num(v('ChartofAccountId')), GlAccountId: num(v('ChartofAccountId')), Remarks: v('JvRemarks') || '', Percentage: toDouble(v('JvPrcnt')), Qty: toDouble(v('JvQty')), Rate: toDouble(v('JvRate')), Debit: toDouble(v('JvDebit')), Credit: toDouble(v('JvCredit')) }; });
                st.freight = (c('freight') || []).map(function (f) { var v = function (k) { return SC.ci(f, k); }; return { Id: num(v('Id')), GrnId: num(v('InvGrnId')), Transporter: sub ? num(v('SupplierCustomerId')) : num(v('TansporterId')), GlAccountId: num(v('TansporterId')), Freight: toDouble(v('FreightAmount')), Debit: toDouble(v('Debit')), Remarks: v('Remarks') || '' }; });
                st.emptyBags = (c('emptyBags') || []).map(function (b) { var v = function (k) { return SC.ci(b, k); }; return { Id: num(v('Id')), OrderId: num(v('PurchaseOrderId')), TypeId: num(v('TypeId')), Type: v('EmptyBagsType') || '', InvGrnId: num(v('InvGrnId')), ItemId: num(v('ItemId')), ItemName: v('ItemName') || '', Qty: toDouble(v('PurchaseQty')), Rate: toDouble(v('Rate')), Amount: toDouble(v('Amount')), Remarks: v('Remarks') || '', CreditAccountId: num(v('CreditAccountId')) }; });
                $id('txtWagesAmountHeader').value = String(toDouble(c('wagesAmountOverride')));   // Q-W1
                renderInvExp(); renderGL(); renderFreight(); renderEmptyBags(); renderGrid();
                CommissionProportion(); ExpProportion(); FreightProportion(); WagesAmountProportion(); BillAmount();
                $id('btnSave').classList.add('is-hidden'); $id('btnUpdate').classList.remove('is-hidden'); $id('btnDelete').classList.remove('is-hidden');
                applyRights();
            });
        }).catch(function (e) { alertBox(e.message); });
    }

    // ------------------------------------------------------------------ 9.3 history
    function historyBranchIds() { var ids = ''; $id('cmbBranchNameList').querySelectorAll('input:checked').forEach(function (i) { ids += ',' + i.value; }); return ids; }   // leading comma, as the desktop
    function renderHistoryBranches() {
        var list = $id('cmbBranchNameList'); list.innerHTML = st.historyBranches.map(function (b) { return '<label><input type="checkbox" value="' + esc(b.Id) + '"' + (b.Selected ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>'; }).join('');
        syncBranchText('cmbBranchNameList', 'cmbBranchNameText');
    }
    function syncBranchText(listId, textId) { var names = []; $id(listId).querySelectorAll('input:checked').forEach(function (i) { names.push(i.parentNode.textContent.trim()); }); $id(textId).textContent = names.join(','); }
    function HistoryComboFill(validate) {
        var ids = historyBranchIds();
        if (!ids) { if (validate) { alertBox('Select branch first'); return Promise.resolve(); } ids = String(st.flags.userBranchId); }
        return get(API + '/history-customers?branchesIds=' + encodeURIComponent(ids)).then(function (rows) { fill('cmbSupplierNameHistory', rows || [], 'Id', 'Supplier'); }).catch(function (e) { msg(e.message, true); });
    }
    var HIST_COLS = [['DocNo', 'i'], ['BranchSrNo', 'i'], ['BranchName', 's'], ['DocDate', 'd'], ['CustomerName', 's'], ['CommAgent', 's'], ['CommType', 's'], ['CommRate', 'n2'], ['CommAmount', 'n2'], ['CommRemarks', 's'], ['DueDays', 'i'], ['DueDate', 'd'], ['BillAmount', 'a'], ['EntryUser', 's'], ['EntryDate', 'dt'], ['ModifyUser', 's'], ['ModifyDate', 'dt'], ['ApprovedUser', 's'], ['ApprovedDate', 'dt'], ['ApprovedStatus', 's'], ['NoOfAttachments', 'i'], ['Remarks', 's']];
    function GetAll() {
        if (!historyBranchIds()) { alertBox('Select branch first'); return; }
        var q = { dateMode: document.querySelector('input[name=histDate]:checked').value, fromDocNo: toInt($id('txtFromDocNoHistory').value), toDocNo: toInt($id('txtToDocNoHistory').value), customerId: num($id('cmbSupplierNameHistory').value), branchesIds: historyBranchIds() };
        if ($id('chkFromDateHistory').checked && $id('FromDateHistory').value) q.fromDate = $id('FromDateHistory').value;
        if ($id('chkToDateHistory').checked && $id('ToDateHistory').value) q.toDate = $id('ToDateHistory').value;
        SC.withBusy($id('btnshow'), function () {
            return get(API + '/history?' + SC.qs(q)).then(function (rows) { st.historyRows = rows || []; renderHistory(); $id('grdDetail').querySelector('tbody').innerHTML = ''; });
        }).catch(function (e) { alertBox(e.message); });
    }
    function renderHistory() {
        var t = $id('grdHistory'), showBranch = st.flags.branchFeature && !st.flags.branchImplemented;
        var cols = HIST_COLS.filter(function (c) { return showBranch || (c[0] !== 'BranchSrNo' && c[0] !== 'BranchName'); });
        t.querySelector('thead').innerHTML = '<tr><th>Voucher</th><th>Slip</th><th>PartySlip</th><th>Edit</th>' + cols.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '<th>Add Attachment</th></tr>';
        t.querySelector('tbody').innerHTML = st.historyRows.map(function (r, i) {
            var v = function (k) { return SC.ci(r, k); }; var id = num(v('Id'));
            var map = { CustomerName: v('SupplierName'), CommAgent: v('CommissionAgent'), CommType: v('CommissionType'), CommRemarks: v('CommissionRemarks'), ApprovedDate: v('PostDate'), ApprovedStatus: v('IsApproved') ? 'APPROVED' : 'Not Approved', Remarks: v('RemarksHeader') };
            return '<tr data-i="' + i + '" data-id="' + id + '"><td class="c"><button type="button" class="x" style="width:60px" data-h="voucher">Voucher</button></td><td class="c"><button type="button" class="x" style="width:35px" data-h="slip">Slip</button></td><td class="c"><button type="button" class="x" style="width:65px" data-h="party">PartySlip</button></td><td class="c"><button type="button" class="x" style="width:35px" data-h="edit">Edit</button></td>'
                + cols.map(function (c) { var k = c[0], val = k in map ? map[k] : v(k); var txt = c[1] === 'dt' ? SC.gridDateTime(val, true) : fmtCell(c[1], val); if (k === 'DocNo') return '<td class="c"><a href="#" class="cx-link" data-h="edit">' + esc(txt) + '</a></td>'; if (k === 'NoOfAttachments') return '<td class="c"><a href="#" class="cx-link" data-h="attachments">' + esc(txt) + '</a></td>'; return '<td class="' + (c[1] === 's' || c[1] === 'd' || c[1] === 'dt' ? '' : c[1] === 'i' ? 'c' : 'n') + '">' + esc(txt) + '</td>'; }).join('')
                + '<td class="c"><button type="button" class="x" style="width:120px" data-h="addAttachment">Add Attachment</button></td></tr>';
        }).join('');
        var ba = 0, ca = 0; st.historyRows.forEach(function (r) { ba += toDouble(SC.ci(r, 'BillAmount')); ca += toDouble(SC.ci(r, 'CommAmount')); });
        t.querySelector('tfoot').innerHTML = st.historyRows.length ? '<tr><td colspan="4"></td>' + cols.map(function (c) { return '<td>' + (c[0] === 'BillAmount' ? esc(fmtAmt(ba)) : c[0] === 'CommAmount' ? esc(fmtN(ca, 2)) : '') + '</td>'; }).join('') + '<td></td></tr>' : '';
    }
    var DET_COLS = [['ItemName', 's', 'ItemName'], ['JobLot', 's', 'JobLotDescription'], ['CropYear', 's', 'CropYear'], ['PackingType', 's', 'PackTypeDesc'], ['PackUom', 's', 'UOMCodeItem'], ['Qty', 'n2', 'ItemQty'], ['GrossWeight', 'n2', 'GrossWeight'], ['WeightCut', 'n2', 'WeightCut'], ['CutTotal', 'n2', 'WeightCutTotal'], ['AD/Ls', 'n2', 'AdLsWeight'], ['NetBillWeight', 'n3', 'NetBillWeight'], ['StockWeight', 'n3', 'NetStockWeight'], ['Rate', 'r', 'ItemRate'], ['RateUom', 's', 'RateUom'], ['RateCut', 'r', 'RateCut'], ['RateCutAmount', 'a', 'RateCutAmount'], ['ItemAmount', 'a', 'ItemAmount'], ['WareHouseName', 's', 'WareHouseName'], ['GpNo', 'i', 'GpNo'], ['VehicleNo', 's', 'VehicleNo'], ['FreightAmount', 'n3', 'FreightAmount'], ['CommAmount', 'n3', 'CommissionAmount'], ['ExpenseAmount', 'n3', 'ExpenseAmount'], ['BranchName', 's', 'BranchName']];
    function GetDetailGrdByHeadId(id) {
        $id('grdHistory').querySelectorAll('tr').forEach(function (tr) { tr.classList.toggle('sel', String(tr.dataset.id) === String(id)); });
        get(API + '/' + id + '/details').then(function (rows) {
            var t = $id('grdDetail'), cols = DET_COLS.filter(function (c) { return c[0] !== 'BranchName' || (st.flags.branchFeature && !st.flags.branchImplemented); });
            t.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '</tr>';
            t.querySelector('tbody').innerHTML = (rows || []).map(function (r) { return '<tr>' + cols.map(function (c) { var kind = c[1]; return '<td class="' + (kind === 's' ? '' : kind === 'i' ? 'c' : 'n') + '">' + esc(fmtCell(kind, SC.ci(r, c[2]))) + '</td>'; }).join('') + '</tr>'; }).join('');
            var sums = {}; (rows || []).forEach(function (r) { cols.forEach(function (c) { if (c[1] !== 's' && c[1] !== 'i' && c[1] !== 'r') sums[c[0]] = (sums[c[0]] || 0) + toDouble(SC.ci(r, c[2])); }); });
            t.querySelector('tfoot').innerHTML = (rows || []).length ? '<tr>' + cols.map(function (c) { return '<td>' + (c[0] in sums ? esc(fmtCell(c[1], sums[c[0]])) : '') + '</td>'; }).join('') + '</tr>' : '';
        }).catch(function (e) { alertBox(e.message); });
    }
    function btnNewHistory_Click() { $id('FromDateHistory').value = today(); $id('ToDateHistory').value = today(); $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = ''; $id('cmbSupplierNameHistory').value = ''; st.historyRows = []; renderHistory(); $id('grdDetail').querySelector('thead').innerHTML = ''; $id('grdDetail').querySelector('tbody').innerHTML = ''; }

    // ------------------------------------------------------------------ 9.5 prints
    function printRpt(rpt, args) {
        var w = window.open('', '_blank'); try { if (w) w.document.write('<p style="font:13px Segoe UI">Preparing report...</p>'); } catch (e) { }
        fetch('/reports/print/by-template/' + encodeURIComponent(rpt) + '/pdf', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify(args || {}) })
            .then(function (r) {
                if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u, '_blank'); });
                return r.text().then(function (t) { if (w) w.close(); alertBox(t || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (w) w.close(); alertBox(e.message); });
    }
    function printSlip(id) { if (!(id > 0)) { alertBox('No Record Found For Display'); return; } printRpt('98_SaleInvoiceReturnItemSlip.rpt', { id: id }); }
    function printPartySlip(id) { if (!(id > 0)) { alertBox('No Record Found For Display'); return; } printRpt('98A_SaleInvoiceReturnPartySlip.rpt', { id: id }); }
    function printVoucher(voucherId) { if (!(voucherId > 0)) { alertBox('No Record Found For Display'); return; } printRpt('103-AcRptPurchaseSalesVoucherSlip.rpt', { id: voucherId, documentTypeId: 98 }); }

    // ------------------------------------------------------------------ 10. Load Sale Invoice dialog
    var LDR_COLS = [['DocumentType', 's'], ['DocNo', 'i'], ['DocDate', 'd'], ['CustomerName', 's'], ['CommissionAgent', 's'], ['CommAmount', 'n2'], ['PaymentTerm', 's', 'TermsDescription'], ['EntryDate', 'dt'], ['EntryUser', 's', 'EntryUserName'], ['ModifyDate', 'dt'], ['ModifyUser', 's', 'ModifyUserName'], ['ApprovalStatus', 's', 'IsApproved'], ['ApprovedDate', 'dt'], ['ApprovedUser', 's', 'ApprovedUserName'], ['BillAmount', 'n2'], ['WareHouseName', 's'], ['ItemCode', 's'], ['ItemName', 's'], ['CropYear', 's'], ['JobLot', 's', 'JobLotCode'], ['PackUom', 's', 'PackUomCode'], ['ItemQty', 'n2'], ['NetBillWeight', 'n2'], ['ItemRate', 'n2'], ['RateUom', 's', 'RateUomCode'], ['ItemAmount', 'n2'], ['UsedQty', 'n2'], ['UsedWeight', 'n2'], ['UsedAmount', 'n2'], ['BalQty', 'n2'], ['BalWeight', 'n2'], ['BalAmount', 'n2'], ['GpNo', 'i'], ['VehicleNo', 's'], ['NoOfAttachments', 'i']];
    function loaderBranchIds() { var ids = []; $id('ldrBranchList').querySelectorAll('input:checked').forEach(function (i) { ids.push(i.value); }); return ids.join(','); }
    function openLoader() {
        try {
            var r0 = st.grid[0];
            if (r0 && !(num(r0.RefDocumentTypeId) > 0 || num(r0.RefDocId) > 0)) throw new Error("You Can't Load Because Direct Entry against SaleInvoice Already Exist");
            SC.openModal('mdlLoadSaleInvoice');
            get(API + '/loader/init').then(function (d) {
                st.loader.branches = d.branches || []; st.loader.userBranchId = num(d.userBranchId);
                $id('ldrFromDate').value = d.fromDate; $id('ldrToDate').value = d.toDate;
                $id('ldrBranchList').innerHTML = st.loader.branches.map(function (b) { return '<label><input type="checkbox" value="' + esc(b.BranchId) + '"' + (num(b.BranchId) === st.loader.userBranchId || st.loader.branches.length === 1 ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>'; }).join('');
                if (!$id('ldrBranchList').querySelector('input:checked') && st.loader.branches.length) $id('ldrBranchList').querySelector('input').checked = true;
                syncBranchText('ldrBranchList', 'ldrBranchText');
                // initial PendingDataDbCall(useBranchDt) - ALL branches of dtBranch
                return get(API + '/loader/pending?' + SC.qs({ fromDate: d.fromDate, toDate: d.toDate, branchesIds: st.loader.branches.map(function (b) { return b.BranchId; }).join(',') }));
            }).then(function (rows) { st.loader.rows = rows || []; renderLoader(); $id('ldrFromDate').focus(); }).catch(function (e) { alertBox('Error occurred during database call. ' + e.message); });
        } catch (e) { alertBox(e.message); }
    }
    function loaderSearch() {
        if (!loaderBranchIds()) { alertBox('Select branch first'); return; }
        SC.withBusy($id('ldrSearch'), function () {
            return get(API + '/loader/pending?' + SC.qs({ fromDate: $id('ldrFromDate').value, toDate: $id('ldrToDate').value, branchesIds: loaderBranchIds() })).then(function (rows) { st.loader.rows = rows || []; renderLoader(); });
        }).catch(function (e) { alertBox(e.message); });
    }
    function renderLoader() {
        var t = $id('ldrGrid');
        t.querySelector('thead').innerHTML = '<tr><th>Select</th>' + LDR_COLS.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '</tr>';
        t.querySelector('tbody').innerHTML = st.loader.rows.map(function (r, i) {
            return '<tr><td class="c"><input type="checkbox" data-ldr="' + i + '"></td>' + LDR_COLS.map(function (c) { var v = SC.ci(r, c[2] || c[0]); var txt = c[1] === 'dt' ? SC.gridDateTime(v, true) : c[0] === 'ApprovalStatus' ? (v ? 'True' : 'False') : fmtCell(c[1], v); if (c[0] === 'DocNo') return '<td class="c"><a href="/sale/sale-invoice?id=' + num(SC.ci(r, 'Id')) + '" class="cx-link" target="_blank">' + esc(txt) + '</a></td>'; return '<td class="' + (c[1] === 's' || c[1] === 'd' || c[1] === 'dt' ? '' : c[1] === 'i' ? 'c' : 'n') + '">' + esc(txt) + '</td>'; }).join('') + '</tr>';
        }).join('');
    }
    function loaderLoad() {
        var ids = []; $id('ldrGrid').querySelectorAll('input[data-ldr]:checked').forEach(function (i) { var id = num(SC.ci(st.loader.rows[num(i.dataset.ldr)], 'Id')); if (ids.indexOf(id) < 0) ids.push(id); });
        if (!ids.length) { alertBox('Chek the row first'); return; }
        SC.withBusy($id('ldrLoad'), function () {
            return post(API + '/loader/load', { invoiceIds: ids, fromDate: $id('ldrFromDate').value, toDate: $id('ldrToDate').value, branchesIds: loaderBranchIds() }).then(function (d) {
                SC.closeModal('mdlLoadSaleInvoice');
                AddInGridDetailFromSaleInvoice(d);
            });
        }).catch(function (e) { alertBox(e.message); });
    }
    function AddInGridDetailFromSaleInvoice(d) {   // :5525
        try {
            var rows = d.rows || []; if (!rows.length) return;
            var r0 = rows[0], v0 = function (k) { return SC.ci(r0, k); };
            setValue('combcommAgent', v0('CommissionAgentId')); setValue('CmbCommissionDebitAccount', v0('CommissionDebitAcGLId'));
            $id('txtcommrate').value = toDouble(v0('CommRate')) === 0 ? '' : fmtN(toDouble(v0('CommRate')), 2);   // "#,##.##"
            setText('combCommType', v0('CommissionType')); setText('combcommUOM', v0('CommUom'));
            $id('txtcommamount').value = toDouble(v0('CommAmount')) === 0 ? '' : fmtN(toDouble(v0('CommAmount')), 2);
            // LoadDataDetailGridFromSaleInvoice :5588
            if ($id('comsupplier').value !== '' && num(v0('SupplierCustomerId')) !== num($id('comsupplier').value)) throw new Error("You Can't Load Grn Of Different Party At Same Time");
            $id('groupBox1').classList.add('is-hidden'); $id('panel3').style.height = '154px';
            setValue('comsupplier', v0('SupplierCustomerId')); $id('comsupplier').disabled = true; comsupplier_Leave();
            setText('combdeliverytrm', v0('DeliveryTerm'));
            rows.forEach(function (x) {
                var v = function (k) { return SC.ci(x, k); };
                if (st.grid.some(function (g) { return num(g.RefDocSubId) === num(v('DetailId')); })) return;
                var r = blankRow();
                r.WarehouseId = num(v('WarehouseId')); r.Warehouse = v('WareHouseName') || ''; r.ItemId = num(v('ItemId')); r.Item = v('ItemName') || ''; r.CropYear = v('CropYear') || ''; r.JobLotId = num(v('JobLotId')); r.JobLot = v('JobLotCode') || '';
                r.PackingTypeId = num(v('PackingTypeId')); r.PackingType = v('PackTypeDesc') || ''; r.PackUOMId = num(v('ItemUOMId')); r.PackUOM = v('PackUomCode') || ''; r.PackUOMEquivalent = toDouble(v('PackUomEquivalent'));
                r.ItemQty = toDouble(v('BalQty')); r.GrossWeight = toDouble(v('BalWeight')); r.NetBillWeight = toDouble(v('BalWeight')); r.StockWeight = toDouble(v('StockWeight'));
                r.Rate = toDouble(v('ItemRate')); r.RateUOMId = num(v('RateUomId')); r.RateUOM = v('RateUomCode') || ''; r.RateEquivalent = toDouble(v('RateUomEquivalent')); r.RateCut = toDouble(v('RateCut')); r.RateCutTotal = toDouble(v('RateCutAmount'));
                r.ItemAmount = toDouble(v('BalAmount')); r.LabSampleNo = v('LabAnalisysNo') || ''; r.GpDate = shortDate(v('GpDate')); r.GpNo = num(v('GpNo')); r.VehicleNo = v('VehicleNo') || '';
                r.BranchId = num(v('BranchIdDetail')); r.BranchName = v('BranchNameDetail') || ''; r.RefDocumentTypeId = num(v('DocumentTypeId')); r.RefDocumentType = v('DocumentType') || ''; r.RefDocId = num(v('Id')); r.RefDocNo = num(v('DocNo')); r.RefDocSubId = num(v('DetailId'));
                r.RefInvoiceQty = toDouble(v('BalQty')); r.RefInvoiceWeight = toDouble(v('BalWeight'));
                st.grid.push(r);
            });
            renderGrid();
            $id('txtWagesAmountHeader').value = String(toDouble(d.wagesAmount));
            var sub = st.flags.subsidiaryAccountAllownOnVouchers;
            st.freight = (d.freight || []).map(function (f) { var v = function (k) { return SC.ci(f, k); }; return { GrnId: 0, Transporter: sub ? num(v('TransporterSupCustId')) : num(v('TansporterId')), GlAccountId: num(v('TansporterId')), Freight: toDouble(v('FreightAmount')), Debit: toDouble(v('Debit')), Remarks: v('Remarks') || '' }; });
            renderFreight();
            TotalCommissionAmount(); FreightProportion(); ExpProportion(); CommissionProportion(); WagesAmountProportion(); BillProportion(); BillAmount();
        } catch (e) { alertBox(e.message); }
    }

    // ------------------------------------------------------------------ attachments (btnAttachment / Ctrl+F10)
    var attachmentEditor = PurchaseInvoiceAttachments.create({ type: 98, getId: function () { return st.recId; }, canEdit: function () { return !!(st.flags.rights || {})[st.recId ? 'update' : 'save']; }, message: function (t) { msg(t, true); } });

    // ------------------------------------------------------------------ load / refresh / rights
    function applyRights() {
        var r = st.flags.rights || {};
        $id('btnSave').disabled = !r.save; $id('btnUpdate').disabled = !r.update; $id('btnDelete').disabled = !r.delete;
        $id('LabelCommissionDrCaption').classList.toggle('is-hidden', !st.flags.commissionDebitToExpenses); $id('CmbCommissionDebitAccount').classList.toggle('is-hidden', !st.flags.commissionDebitToExpenses);
    }
    function bindLookups(l) {
        st.look = l;
        fill('CmbCommissionDebitAccount', l.commissionDebitAccounts, 'Id', 'AccountTitle', 'Comm Dr Account ');
        fill('combpttrm', l.paymentTerms, 'Id', 'TermsDescription', false); if (l.paymentTerms && l.paymentTerms.length) $id('combpttrm').selectedIndex = 0;   // Rows[0]
        fill('combdeliverytrm', l.deliveryTerms, 'Id', 'Value', false); $id('combdeliverytrm').selectedIndex = 0;
        fill('combCommType', l.commissionTypes, 'Id', 'Value', false); $id('combCommType').selectedIndex = 0;
        fill('combcommUOM', l.commissionUoms, 'Id', 'type', false); $id('combcommUOM').selectedIndex = 0;
        fill('comWarehouse', l.warehouses, 'Id', 'WareHouseName', '...Select Any Value...'); if (l.warehouses && l.warehouses.length) $id('comWarehouse').selectedIndex = 1;   // Rows[1]
        fill('comsupplier', l.parties, 'Id', 'CompanyName', false); $id('comsupplier').value = ''; fill('combcommAgent', l.parties, 'Id', 'CompanyName', false); $id('combcommAgent').value = '';
        fill('comPackingType', l.packingTypes, 'Id', 'PackTypeDesc', false); $id('comPackingType').value = '';
        fill('comjobLot', l.jobLots, 'Id', 'JobLotDescription', '...Select Any Value...'); $id('comjobLot').selectedIndex = 0;
        fill('comCropYear', l.cropYears, 'Id', 'CropYear', false); $id('comCropYear').value = '';
        st.historyBranches = l.historyBranches || []; renderHistoryBranches();
        combpttrm_TextChanged();
    }
    function load() {
        return get(API + '/init').then(function (d) {
            st.flags = d; st.N = num(d.amountDecimals); st.R = num(d.rateDecimals);
            bindLookups(d.lookups || {});
            $id('txtdocno').value = String(d.lookups.numbers.docNo); $id('txtBranchSrNo').value = String(d.lookups.numbers.branchSrNo);
            $id('DocDate').value = today(); $id('duedate').value = today(); $id('txtgpdate').value = today(); $id('FromDateHistory').value = today(); $id('ToDateHistory').value = today();
            $id('ChkBok').checked = true;
            renderGL(); renderInvExp(); renderFreight(); renderEmptyBags(); renderGrid();
            applyRights();
            return HistoryComboFill(false);
        }).then(function () {
            var q = new URLSearchParams(location.search), id = num(q.get('id'));
            if (id > 0) return ReadById(id);
            $id('DocDate').focus();
        }).catch(function (e) { alertBox(e.message); });
    }
    function btnRefresh_Click() {   // :2849 (Q-R1: GridcomboBind's exception is not reproduced; the rebinds it never reached still run here)
        SC.withBusy($id('btnRefresh'), function () {
            return get(API + '/init').then(function (d) {
                st.flags = d; bindLookups(d.lookups || {});
                return get(API + '/all-items').then(function (items) { fill('comItem', items || [], 'Id', 'ItemName'); applyRights(); });
            });
        }).catch(function (e) { alertBox(e.message); });
    }
    function tab(name) {
        document.querySelectorAll('.fx-tabs .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.dataset.tab === name); });
        $id('tabForm').classList.toggle('is-active', name === 'tabForm'); $id('tabHistory').classList.toggle('is-active', name === 'tabHistory');
        if (name === 'tabHistory') $id('FromDateHistory').focus();
    }
    function showShortcuts() {
        var rows = [['Ctrl+S', 'For Save in Form Tab And For Show History in History Tabs'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Alt+1', 'For Slip Print'], ['Alt+2', 'For Voucher Print'], ['Ctrl+F5', 'For Focus on DocDate'], ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowRight', 'For Toggle Between Expense Grids'], ['Ctrl+ArrowUp', 'For Focus On Warehouse in Detail Box'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        $id('shortcutRows').innerHTML = rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('');
        SC.openModal('mdlShortcuts');
    }
    function subTab(name) { document.querySelectorAll('.fx-subtabs .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.dataset.sub === name); }); document.querySelectorAll('.fx-subpane').forEach(function (p) { p.classList.toggle('is-active', p.id === name); }); }

    // ------------------------------------------------------------------ wiring (3.7 + 5.8)
    document.addEventListener('DOMContentLoaded', function () {
        $id('btnNew').addEventListener('click', Reset); $id('btnRefresh').addEventListener('click', btnRefresh_Click);
        $id('btnSave').addEventListener('click', btnSave_Click); $id('btnUpdate').addEventListener('click', btnUpdate_Click); $id('btnDelete').addEventListener('click', btnDelete_Click);
        $id('btnAttachment').addEventListener('click', function () { attachmentEditor.open(this); });
        $id('btnSlip').addEventListener('click', function () { printSlip(st.recId); }); $id('btnPartySlip').addEventListener('click', function () { printPartySlip(st.recId); }); $id('btnPrint').addEventListener('click', function () { printVoucher(st.voucherHeadId); });
        $id('BtnLoadSaleInvoice').addEventListener('click', openLoader);
        $id('btnLoadGrn').addEventListener('click', function () { var r0 = st.grid[0]; if (r0 && !(num(r0.GrnId) > 0)) { alertBox("You Can't Load Because Direct Entry Already Exist"); return; } alertBox('Load GRN (frmLoadGRN, DocumentTypeId 143) is not ported on the web yet.'); });
        $id('btnShortCutKey').addEventListener('click', showShortcuts);
        document.querySelectorAll('[data-close]').forEach(function (b) { b.addEventListener('click', function () { SC.closeModal(b.dataset.close); }); });
        document.querySelectorAll('.fx-tabs .win-tab').forEach(function (t) { t.addEventListener('click', function () { tab(t.dataset.tab); }); });
        document.querySelectorAll('.fx-subtabs .win-tab').forEach(function (t) { t.addEventListener('click', function () { subTab(t.dataset.sub); }); });
        // detail box
        ['txtQty'].forEach(function (id) { $id(id).addEventListener('input', txtQty_TextChanged); });
        ['txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtAddLss'].forEach(function (id) { $id(id).addEventListener('input', Total); });
        $id('txtNetBillWeight').addEventListener('input', AmountCaluculation);
        ['txtRate', 'txtratecut'].forEach(function (id) { $id(id).addEventListener('input', rateChanged); });
        $id('comRateUOM').addEventListener('change', rateChanged); $id('comPackUOM').addEventListener('change', comPackUOM_Leave); $id('comItem').addEventListener('change', PackUOM);
        ['txtcommrate', 'txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtWeightCutTotal', 'txtNetBillWeight', 'txtRate', 'txtratecut'].forEach(function (id) { $id(id).addEventListener('keydown', function (e) { decimalKey(e, false); }); });
        $id('txtAddLss').addEventListener('keydown', function (e) { decimalKey(e, true); });
        ['txtFromDocNoHistory', 'txtToDocNoHistory'].forEach(function (id) { $id(id).addEventListener('keydown', function (e) { if (e.key.length === 1 && !/\d/.test(e.key)) e.preventDefault(); }); });
        $id('btnAdd').addEventListener('click', btnAdd_Click); $id('btnUpdateDetail').addEventListener('click', btnUpdateDetail_Click); $id('btnCancelUpdateDetial').addEventListener('click', function () { detailButtonsAdd(); ResetDetail(); });
        // header
        $id('comsupplier').addEventListener('change', function () { BillAmount(); comsupplier_Leave(); });
        $id('combcommAgent').addEventListener('change', BillAmount);
        $id('combCommType').addEventListener('change', TotalCommissionAmount);
        $id('txtcommrate').addEventListener('input', function () { TotalCommissionAmount(); BillAmount(); });
        $id('combpttrm').addEventListener('change', combpttrm_TextChanged);
        $id('txtduedays').addEventListener('input', txtduedays_TextChanged); $id('duedate').addEventListener('change', duedate_ValueChanged);
        // main grid
        $id('grd').addEventListener('click', function (e) { var d = e.target.closest('[data-del]'); if (d) DeleteDetailrow(num(d.dataset.del)); });
        $id('grd').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('input')) grd_DoubleClick(num(tr.dataset.i)); });
        $id('grd').addEventListener('change', function (e) { var i = e.target.closest('[data-edit]'); if (i) grd_CellUpdated(num(i.dataset.i), i.dataset.edit, i.value); });
        // sub grids
        ['grdInvExp', 'grdGLedger', 'grdFreight', 'grdEmptyBags'].forEach(function (id) {
            $id(id).addEventListener('change', function (e) { var c = e.target.closest('[data-g]'); if (c) subGridEdit(c.dataset.g, num(c.dataset.i), c.dataset.k, c.value); });
            $id(id).addEventListener('click', function (e) { var d = e.target.closest('[data-gdel]'); if (d) { subGridDelete(d.dataset.gdel, num(d.dataset.i)); return; } var a = e.target.closest('[data-gadd]'); if (a) subGridAdd(a.dataset.gadd); });
        });
        // history
        $id('btnshow').addEventListener('click', GetAll); $id('btnNewHistory').addEventListener('click', btnNewHistory_Click);
        $id('btnRefreshHistory').addEventListener('click', function () { renderHistoryBranches(); HistoryComboFill(true); });
        $id('cmbSupplierNameHistory').addEventListener('blur', function () { if (historyBranchIds()) HistoryComboFill(true); else { $id('cmbSupplierNameHistory').innerHTML = ''; } });
        $id('cmbBranchName').querySelector('.fx-multi-btn').addEventListener('click', function () { var l = $id('cmbBranchNameList'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
        $id('cmbBranchNameList').addEventListener('change', function () { syncBranchText('cmbBranchNameList', 'cmbBranchNameText'); });
        $id('ldrBranch').querySelector('.fx-multi-btn').addEventListener('click', function () { var l = $id('ldrBranchList'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
        $id('ldrBranchList').addEventListener('change', function () { syncBranchText('ldrBranchList', 'ldrBranchText'); });
        document.addEventListener('mousedown', function (e) { if (!e.target.closest('.fx-multi')) document.querySelectorAll('.fx-multi-list').forEach(function (l) { l.style.display = 'none'; }); });
        $id('grdHistory').addEventListener('click', function (e) {
            var b = e.target.closest('[data-h]'); var tr = e.target.closest('tr[data-id]'); if (!tr) return; var id = num(tr.dataset.id), row = st.historyRows[num(tr.dataset.i)];
            if (!b) { GetDetailGrdByHeadId(id); return; }
            e.preventDefault();
            if (b.dataset.h === 'edit') { Reset(); ReadById(id); }
            else if (b.dataset.h === 'voucher') printVoucher(num(SC.ci(row, 'VoucherHeadId')));
            else if (b.dataset.h === 'slip') printSlip(id);
            else if (b.dataset.h === 'party') printPartySlip(id);
            else if (b.dataset.h === 'attachments' || b.dataset.h === 'addAttachment') { Reset(); ReadById(id).then(function () { attachmentEditor.open(null); }); }
        });
        $id('grdHistory').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-id]'); if (tr) { Reset(); ReadById(num(tr.dataset.id)); } });
        // loader
        $id('ldrSearch').addEventListener('click', loaderSearch); $id('ldrLoad').addEventListener('click', loaderLoad);
        $id('ldrNew').addEventListener('click', function () { get(API + '/loader/init').then(function (d) { $id('ldrFromDate').value = d.fromDate; $id('ldrToDate').value = d.toDate; st.loader.rows = []; renderLoader(); $id('ldrFromDate').focus(); }); });
        $id('ldrRefresh').addEventListener('click', function () { get(API + '/loader/init').then(function (d) { st.loader.branches = d.branches || []; $id('ldrBranchList').innerHTML = st.loader.branches.map(function (b) { return '<label><input type="checkbox" value="' + esc(b.BranchId) + '"' + (num(b.BranchId) === st.loader.userBranchId ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>'; }).join(''); syncBranchText('ldrBranchList', 'ldrBranchText'); }); });
        $id('ldrShortcuts').addEventListener('click', function () { alertBox('Ctrl+E For Close\nCtrl+L To Press Load Button\nCtrl+S For Search\nCtrl+alt To Show ShortCut Keys Form\nCtrl+Space When Focus On Any Grid To Call Function\'s On Button Or Link'); });
        // form KeyDown :3807
        document.addEventListener('keydown', function (e) {
            var onForm = $id('tabForm').classList.contains('is-active'), k = e.key.toLowerCase();
            if (!$id('mdlLoadSaleInvoice').classList.contains('is-open')) {
                if (e.ctrlKey && k === 'n') { e.preventDefault(); onForm ? Reset() : btnNewHistory_Click(); }
                else if (e.ctrlKey && k === 'r') { e.preventDefault(); onForm ? btnRefresh_Click() : (renderHistoryBranches(), HistoryComboFill(true)); }
                else if (e.ctrlKey && k === 's') { e.preventDefault(); if (onForm) { if (!$id('btnSave').classList.contains('is-hidden') && !$id('btnSave').disabled) btnSave_Click(); } else GetAll(); }
                else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (!$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) btnUpdate_Click(); }
                else if (e.ctrlKey && k === 't') { e.preventDefault(); tab(onForm ? 'tabHistory' : 'tabForm'); }
                else if (e.altKey && (k === '1' || e.code === 'Numpad1')) { e.preventDefault(); if ((st.flags.rights || {}).print) printSlip(st.recId); }
                else if (e.altKey && (k === '2' || e.code === 'Numpad2')) { e.preventDefault(); if ((st.flags.rights || {}).print) printVoucher(st.voucherHeadId); }
                else if (e.ctrlKey && k === 'f5') { e.preventDefault(); $id('DocDate').focus(); }
                else if (e.ctrlKey && k === 'f10') { e.preventDefault(); attachmentEditor.open(null); }
                else if (e.ctrlKey && k === 'arrowup') { e.preventDefault(); onForm ? (!$id('groupBox1').classList.contains('is-hidden') && $id('comWarehouse').focus()) : $id('FromDateHistory').focus(); }
                else if (e.ctrlKey && k === 'arrowright' && onForm) { e.preventDefault(); var order = ['tabChargedToProduct', 'tabGLedger', 'tabOtherExpense', 'tabEmptyBag'], cur = document.querySelector('.fx-subpane.is-active').id; subTab(order[(order.indexOf(cur) + 1) % 4]); }
                else if (e.ctrlKey && e.altKey) { showShortcuts(); }
            } else {
                if (e.ctrlKey && k === 's') { e.preventDefault(); loaderSearch(); } else if (e.ctrlKey && k === 'l') { e.preventDefault(); loaderLoad(); } else if ((e.ctrlKey && k === 'e') || k === 'escape') { SC.closeModal('mdlLoadSaleInvoice'); }
            }
        });
        load();
    });
})();
