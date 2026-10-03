/* InvfrmInvPurchaseInvoiceReturn (Purchase Invoice Return, screen 125, DocumentTypeId 59) - the desktop form's events.
   ":NNN" = Architecture.WinApp.Purchase/InvfrmInvPurchaseInvoiceReturn.cs, "L:NNN" = frmLoadPurchaseInvoiceForReturn.cs.
   Server: /api/purchase/purchase-invoice-return (PurchaseInvoiceReturnRestController). Uses StoreCommon (requests, busy buttons,
   searchable win-combo selects) and PurchaseInvoiceAttachments (the purchase-invoice attachment dialog). */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num;
    var API = '/api/purchase/purchase-invoice-return';

    var st = {
        recId: 0, voucherHeadId: 0, updateDetailIndex: -1, flags: {}, look: {}, N: 0, R: 2, F: 0,
        grid: [], invExp: [], gl: [], freight: [], uoms: [], supplierGlId: '',
        freightCaption: 'Less To Product', historyRows: [], historyBranches: [], loader: { branches: [], rows: [] }
    };

    // ------------------------------------------------------------------ Conversion.* / formats
    function toDouble(v) { if (v === null || v === undefined) return 0; var s = String(v).trim().replace(/,/g, ''); if (s === '') return 0; var x = Number(s); return isFinite(x) ? x : 0; }
    function toInt(v) { if (typeof v === 'number') return bankers(v, 0); var s = String(v == null ? '' : v).trim(); if (s === '') return 0; return /^-?\d+$/.test(s) ? parseInt(s, 10) : 0; }
    function bankers(x, d) { var f = Math.pow(10, d || 0), y = x * f, r = Math.round(y); if (Math.abs(y % 1) === 0.5) r = 2 * Math.round(y / 2); return r / f; }   // Math.Round (ToEven)
    function afz(x, d) { var f = Math.pow(10, d); var y = Math.abs(x) * f; return (x < 0 ? -1 : 1) * Math.round(y + 1e-9) / f; }   // MidpointRounding.AwayFromZero
    function grp(v, min, max) { if (!isFinite(v)) return String(v); return Number(v).toLocaleString('en-US', { minimumFractionDigits: min, maximumFractionDigits: max }); }
    function fmtAmt(v) { return grp(afz(toDouble(v), st.N), st.N, st.N); }        // clsGlobalVariables.stringFormatsingle
    function fmtRate(v) { return grp(toDouble(v), st.R, st.R); }                     // DecimalRateFormate
    function fmt2(v) { return grp(toDouble(v), 0, 2); }                               // "#,##0.##"
    function fmt3(v) { return grp(toDouble(v), 0, 3); }                               // "#,##0.###"
    function fmtHash(v) { var x = toDouble(v); return x === 0 ? '' : grp(x, 0, 2); }  // "#,#.##" / "#,##.##" (0 -> "")
    function fmtFcy(v) { return grp(toDouble(v), st.F, st.F); }                      // stringFormatsingleForFcy
    function today() { return SC.today(); }
    function addDays(iso, d) { var t = new Date(iso + 'T00:00:00'); t.setDate(t.getDate() + d); return SC.isoDay(t.getFullYear() + '-' + String(t.getMonth() + 1).padStart(2, '0') + '-' + String(t.getDate()).padStart(2, '0')); }
    function shortDate(v) { if (!v) return ''; var s = String(v); return s.length >= 10 ? s.substring(0, 10) : s; }
    function gridDate(v) { var s = shortDate(v); if (!s || s === '1900-01-01') return ''; return SC.gridDate(s); }

    function msg(text, error) {
        var m = $id('fxMsg'); m.textContent = text || ''; m.classList.toggle('is-error', !!error); m.style.display = text ? 'block' : 'none';
        if (text) { clearTimeout(msg.t); msg.t = setTimeout(function () { m.style.display = 'none'; }, error ? 9000 : 5000); }
    }
    function box(text) { alert(text); }
    function get(url) { return SC.getJson(url); }
    function post(url, body) { return SC.postJson(url, body); }
    function rights() { return st.flags.rights || {}; }

    function fill(id, rows, valueKey, textKey, blank) {
        var el = $id(id), keep = el.value; el.innerHTML = '';
        if (blank !== false) el.appendChild(new Option(typeof blank === 'string' ? blank : '', ''));
        (rows || []).forEach(function (r) { el.appendChild(new Option(String(SC.ci(r, textKey) == null ? '' : SC.ci(r, textKey)), String(SC.ci(r, valueKey)))); });
        el.value = keep; if (el.value !== keep) el.value = '';
    }
    function selText(id) { var el = $id(id); return el.selectedOptions[0] ? el.selectedOptions[0].textContent : ''; }
    function selRow(id, rows, key) { var v = $id(id).value; if (v === '') return null; return (rows || []).find(function (r) { return String(SC.ci(r, key || 'Id')) === v; }) || null; }
    function setValue(id, v) { var el = $id(id); el.value = v == null ? '' : String(v); if (el.tagName === 'SELECT' && el.value !== String(v == null ? '' : v)) el.value = ''; }
    function setText(id, text) { var el = $id(id); var o = Array.from(el.options).find(function (x) { return x.textContent === String(text == null ? '' : text); }); el.value = o ? o.value : ''; }
    function decimalKey(e) { var c = e.key; if (c.length !== 1) return; if (/\d/.test(c)) return; if (c === '.' && e.target.value.indexOf('.') < 0) return; if (c === '-' && e.target.selectionStart === 0 && e.target.value.indexOf('-') < 0 && e.target.id === 'txtAddLss') return; e.preventDefault(); }
    function sumGrid(k) { var s = 0; st.grid.forEach(function (r) { s += toDouble(r[k]); }); return s; }
    /* Clickable codes -> DocLink.open (CommonServices.EditMethodFromLinked) with the row's document type. */
    function docLink(type, id, caption) { return num(type) > 0 && num(id) > 0 ? '<a href="#" class="cx-link" data-doclink="' + num(type) + '" data-docid="' + num(id) + '">' + esc(caption) + '</a>' : esc(caption); }

    // ------------------------------------------------------------------ detail box (:3962-4330)
    function packEq() { var u = selRow('comPackUOM', st.uoms); return u ? toDouble(SC.ci(u, 'Equivalent')) : 0; }
    function rateEq() { var u = selRow('comRateUOM', st.uoms); return u ? toDouble(SC.ci(u, 'Equivalent')) : 0; }
    function Total() {   // :4066
        try {
            var qty = $id('txtQty').value !== '' ? toDouble($id('txtQty').value) : 0, gross = $id('txtGrossWeight').value !== '' ? toDouble($id('txtGrossWeight').value) : 0;
            var ebUnit = $id('txtEmptybagsUnit').value !== '' ? toDouble($id('txtEmptybagsUnit').value) : 0;
            var ebTotal = ebUnit > 0 ? ebUnit * qty : 0; $id('txtEmptyBagsTotal').value = fmt2(ebTotal);
            var wct = 0;
            if ($id('txtwtcut').value !== '') { wct = qty * toDouble($id('txtwtcut').value); $id('txtWeightCutTotal').value = fmt2(wct); } else $id('txtWeightCutTotal').value = '0';
            var addLess = $id('txtAddLss').value !== '' ? $id('txtAddLss').value.trim() : '0', w;
            if (addLess === '-') w = gross - ebTotal - wct;
            else { var a = Number(addLess.replace(/,/g, '')); if (!isFinite(a) || addLess === '') throw new Error('Input string was not in a correct format.'); w = gross - ebTotal - wct + a; }   // double.Parse
            $id('txtNetBillWeight').value = fmt2(bankers(w, 2)); $id('txtStockWeight').value = fmt2(bankers(w, 2));
        } catch (e) { box(e.message); }
    }
    function AmountCaluculation() {   // :4171
        var eq = rateEq(), rate = toDouble($id('txtRate').value), cut = toDouble($id('txtratecut').value), net = toDouble($id('txtNetBillWeight').value);
        if (net > 0 && eq > 0 && rate > 0) {
            var amount = net / eq * rate, rct = net / eq * cut;
            $id('txtratecuttotal').value = String(bankers(rct, 0));
            amount -= rct; $id('txtAmount').value = fmtAmt(amount);
        } else { $id('txtAmount').value = '0'; $id('txtBillAmount').value = '0'; }
    }
    function grossFromQty(always) {   // txtQty_TextChanged :3984 / comPackUOM_Leave :3962
        var q = $id('txtQty').value;
        if (q !== '' && $id('comPackUOM').value !== '' && num($id('comPackUOM').value) > 0) $id('txtGrossWeight').value = fmt2(toDouble(q) * packEq());
        else if (always) $id('txtGrossWeight').value = '0';
    }
    function txtQty_TextChanged() { grossFromQty(true); Total(); AmountCaluculation(); TotalCommissionAmount(); BillAmount(); }
    function comPackUOM_Leave() { grossFromQty(false); Total(); AmountCaluculation(); TotalCommissionAmount(); BillAmount(); AvailableStockGetByItem(); }
    function weightInputChanged() { Total(); AmountCaluculation(); TotalCommissionAmount(); BillAmount(); }
    function rateChanged() { AmountCaluculation(); TotalCommissionAmount(); BillAmount(); }
    function UomFromGlobalBind(itemId) {   // :1137 - keeps the selections by UOM text
        var prevPack = selText('comPackUOM'), prevRate = selText('comRateUOM');
        if (!(itemId > 0)) { st.uoms = []; fill('comPackUOM', [], 'Id', 'UOMCode'); fill('comRateUOM', [], 'Id', 'UOMCode'); return Promise.resolve(); }
        return get(API + '/uoms' + SC.qs({ itemId: itemId })).then(function (rows) {
            st.uoms = rows || [];
            fill('comPackUOM', st.uoms, 'Id', 'UOMCode'); fill('comRateUOM', st.uoms, 'Id', 'UOMCode');
            setText('comPackUOM', prevPack); setText('comRateUOM', prevRate);
        }).catch(function (e) { box(e.message); });
    }
    function AvailableStockGetByItem() {   // :4494
        var q = { warehouseId: num($id('comWarehouse').value), itemId: num($id('comItem').value), jobLotId: num($id('comjobLot').value), cropYear: selText('comCropYear').trim(),
            docDate: $id('DocDate').value || today(), packingTypeId: num($id('comPackingType').value), packUomId: num($id('comPackUOM').value) };
        return get(API + '/stock' + SC.qs(q)).then(function (r) { var b = toDouble(r.balance); $id('lblBalance').textContent = b > 0 ? fmt2(b) : '0'; }).catch(function (e) { box(e.message); });
    }
    function bindItems() {   // item() :871 / rdSearchByName_CheckedChanged :1334
        var byName = $id('rdSearchByName').checked;
        fill('comItem', st.look.items, 'Id', byName ? 'ItemName' : 'ItemCode');
    }

    function FormValidationDetila() {   // :758
        var checks = [
            ['comWarehouse', function () { return num($id('comWarehouse').value) === 0; }, 'Warehouse Field is Required'],
            ['comItem', function () { return num($id('comItem').value) === 0; }, 'Item Field is Required'],
            ['comCropYear', function () { return num($id('comCropYear').value) === 0; }, 'Crop Year Field is Required'],
            ['comjobLot', function () { return num($id('comjobLot').value) === 0; }, 'Job/Lot Field is Required'],
            ['comPackingType', function () { return num($id('comPackingType').value) === 0; }, 'Packing Type Field is Required'],
            ['comPackUOM', function () { return num($id('comPackUOM').value) === 0; }, 'UOM Field is Required'],
            ['txtQty', function () { return toDouble($id('txtQty').value.trim()) === 0; }, 'Item Qty Field is Required'],
            ['txtGrossWeight', function () { return toDouble($id('txtGrossWeight').value.trim()) === 0; }, 'Gross Weight Field is Required'],
            ['txtNetBillWeight', function () { return toDouble($id('txtNetBillWeight').value.trim()) === 0; }, 'Net Weight Field is Required'],
            ['txtStockWeight', function () { return toDouble($id('txtStockWeight').value.trim()) === 0; }, 'Stock Weight Field is Required'],
            ['txtRate', function () { return toDouble($id('txtRate').value.trim()) === 0; }, 'Item Rate Field is Required'],
            ['comRateUOM', function () { return num($id('comRateUOM').value) === 0; }, 'Rate UOM Field is Required']
        ];
        for (var i = 0; i < checks.length; i++) if (checks[i][1]()) { box(checks[i][2]); $id(checks[i][0]).focus(); return false; }
        return true;
    }
    function branchCheck() {
        var wh = selRow('comWarehouse', st.look.warehouses), lot = selRow('comjobLot', st.look.jobLots);
        var b = { id: wh ? num(SC.ci(wh, 'BranchId')) : 0, name: wh ? String(SC.ci(wh, 'BranchName') || '') : '', jobId: lot ? num(SC.ci(lot, 'BranchId')) : 0, jobName: lot ? String(SC.ci(lot, 'BranchName') || '') : '' };
        if (st.flags.branchFeature && !st.flags.branchImplemented && b.id !== b.jobId) {
            box("Warehouse and JobLot are Not From Same Branch.\nWarehouse is Of Branch '" + b.name + "' and JobLot is of Branch '" + b.jobName + "'"); return null;
        }
        return b;
    }
    function blankRow() {
        return { Id: 0, GdnId: 0, GdnDetailId: 0, GdnNo: 0, WarehouseId: 0, Warehouse: '', ItemId: 0, ItemCode: '', Item: '', CropYear: '', JobLotId: 0, JobLot: '', PackingTypeId: 0, PackingType: '',
            PackUOMId: 0, UOM: '', UomEquivalent: 0, ItemQty: 0, GrossWeight: 0, EBUnit: 0, EBTotal: 0, WeightCut: 0, WeightCutTotal: 0, AdLs: 0, NetWeight: 0, StockWeight: 0,
            ItemRate: 0, RateUOMId: 0, RateUOM: '', RateEquivalent: 0, RateCut: 0, RateCutTotal: 0, Amount: 0, GpDate: '', GpNo: 0, VehicleNo: '', RemarksDetail: '', FcyAmount: 0,
            BillAmount: 0, Commission: 0, Freights: 0, Expense: 0, BranchId: 0, BranchName: '', RefDocumentTypeId: 0, RefDocumentType: '', RefDocId: 0, RefDocNo: 0, RefDocSubId: 0, RefInvoiceQty: 0, RefInvoiceWeight: 0 };
    }
    function boxToRow(r, b) {
        var item = selRow('comItem', st.look.items);
        r.WarehouseId = num($id('comWarehouse').value); r.Warehouse = selText('comWarehouse'); r.ItemId = num($id('comItem').value);
        r.ItemCode = item ? SC.ci(item, 'ItemCode') : ''; r.Item = item ? SC.ci(item, 'ItemName') : '';
        r.CropYear = selText('comCropYear'); r.JobLotId = num($id('comjobLot').value); r.JobLot = selText('comjobLot'); r.PackingTypeId = num($id('comPackingType').value); r.PackingType = selText('comPackingType');
        r.PackUOMId = num($id('comPackUOM').value); r.UOM = selText('comPackUOM'); r.UomEquivalent = packEq();
        r.ItemQty = toDouble($id('txtQty').value.trim()); r.GrossWeight = toDouble($id('txtGrossWeight').value); r.EBUnit = toDouble($id('txtEmptybagsUnit').value.trim()); r.EBTotal = toDouble($id('txtEmptyBagsTotal').value.trim());
        r.WeightCut = toDouble($id('txtwtcut').value); r.WeightCutTotal = toDouble($id('txtWeightCutTotal').value); r.AdLs = toDouble($id('txtAddLss').value);
        r.NetWeight = toDouble($id('txtNetBillWeight').value); r.StockWeight = toDouble($id('txtStockWeight').value);
        r.ItemRate = toDouble($id('txtRate').value); r.RateUOMId = num($id('comRateUOM').value); r.RateUOM = selText('comRateUOM'); r.RateEquivalent = rateEq();
        r.RateCut = toDouble($id('txtratecut').value); r.RateCutTotal = toDouble($id('txtratecuttotal').value); r.Amount = toDouble($id('txtAmount').value);
        r.GpDate = $id('txtgpdate').value || today(); r.GpNo = toInt($id('txtgatepassno').value); r.VehicleNo = $id('txtvehicleno').value; r.RemarksDetail = $id('txtRemarksDetail').value;
        r.BranchId = b.id; r.BranchName = b.name;
    }
    function btnAdd_Click() {   // :1554
        try {
            if (st.grid.length) {
                if (st.grid.some(function (r) { return num(r.GdnId) > 0; })) throw new Error('Grid has records from Gdn. Manual Entry is not allowed!');
                if (st.grid.some(function (r) { return num(r.RefDocId) > 0; })) throw new Error('Grid has records from Purchase Invoice. Manual Entry is not allowed!');
            }
            if (!FormValidationDetila()) return;
            if ($id('txtAmount').value === '' || $id('txtAmount').value === '0') throw new Error('Please Check Item Amount');
            var b = branchCheck(); if (!b) return;
            var r = blankRow(); boxToRow(r, b);
            r.FcyAmount = toDouble($id('txtFcyAmount').value);   // Q-D1: the header Fcy total, as the desktop adds it
            st.grid.push(r);
            txtExchangeRate_TextChanged(); TotalCommissionAmount(); FrightCalculations(); CalculateTotalInformation(); FreightProportion(); CommissionProportion(); ExpProportion(); BillAmount();
            renderGrid(); ResetDetail();
        } catch (e) { box(e.message); }
    }
    function btnUpdateDetail_Click() {   // :1615
        try {
            if (!FormValidationDetila()) return;
            var b = branchCheck(); if (!b) return;
            var r = st.grid[st.updateDetailIndex]; if (!r) return;
            boxToRow(r, b);
            detailButtonsAdd();
            TotalCommissionAmount(); FrightCalculations(); FreightProportion(); CommissionProportion(); ExpProportion(); BillAmount(); txtExchangeRate_TextChanged(); CalculateTotalInformation();
            renderGrid(); ResetDetail();
        } catch (e) { box(e.message); }
    }
    function detailButtonsAdd() { $id('btnAdd').classList.remove('is-hidden'); $id('btnUpdateDetail').classList.add('is-hidden'); $id('btnCancelUpdateDetial').classList.add('is-hidden'); }
    function grd_DoubleClick(i) {   // :1824
        var r = st.grid[i]; if (!r) return;
        st.updateDetailIndex = i;
        setValue('comWarehouse', r.WarehouseId);
        setValue('comItem', r.ItemId); if ($id('comItem').value === '' && r.ItemId) { $id('comItem').appendChild(new Option(r.Item, String(r.ItemId))); $id('comItem').value = String(r.ItemId); }
        return UomFromGlobalBind(num(r.ItemId)).then(function () {
            setText('comCropYear', r.CropYear); setValue('comjobLot', r.JobLotId); setValue('comPackingType', r.PackingTypeId); setValue('comPackUOM', r.PackUOMId);
            $id('txtQty').value = fmtHash(r.ItemQty); $id('txtGrossWeight').value = fmtHash(r.GrossWeight); $id('txtEmptybagsUnit').value = fmtHash(r.EBUnit); $id('txtEmptyBagsTotal').value = fmtHash(r.EBTotal);
            $id('txtwtcut').value = fmtHash(r.WeightCut); $id('txtWeightCutTotal').value = fmtHash(r.WeightCutTotal); $id('txtAddLss').value = fmtHash(r.AdLs);
            $id('txtNetBillWeight').value = fmtHash(r.NetWeight); $id('txtStockWeight').value = fmtHash(r.StockWeight); $id('txtRate').value = fmtHash(r.ItemRate);
            setValue('comRateUOM', r.RateUOMId); $id('txtratecut').value = fmtHash(r.RateCut); $id('txtratecuttotal').value = fmtHash(r.RateCutTotal); $id('txtAmount').value = fmtHash(r.Amount);
            $id('txtgpdate').value = shortDate(r.GpDate); $id('txtgatepassno').value = String(r.GpNo == null ? '' : r.GpNo); $id('txtvehicleno').value = r.VehicleNo || ''; $id('txtRemarksDetail').value = r.RemarksDetail || '';
            $id('btnAdd').classList.add('is-hidden'); $id('btnUpdateDetail').classList.remove('is-hidden'); $id('btnCancelUpdateDetial').classList.remove('is-hidden');
            $id('comWarehouse').focus();
        });
    }
    function ResetDetail() {   // :2942 (Item, CropYear, JobLot, PackingType, Warehouse, GP fields kept)
        $id('comPackUOM').value = '';
        ['txtQty', 'txtRemarksDetail', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtWeightCutTotal', 'txtAddLss', 'txtNetBillWeight', 'txtStockWeight', 'txtRate', 'txtratecut', 'txtratecuttotal', 'txtAmount'].forEach(function (id) { $id(id).value = ''; });
        $id('comRateUOM').value = '';
        $id('comItem').focus();
    }
    function DeleteDetailrow(i) {   // :1910 + grd_RecordsDeleted :1870
        var r = st.grid[i]; if (!r) return;
        if (num(r.Id) > 0 && !confirm('Are you sure you want to delete this record?')) return;
        st.grid.splice(i, 1);
        TotalCommissionAmount(); FreightProportion(); BillAmount();
        renderGrid();
    }

    // ------------------------------------------------------------------ calculations (:3777-4290)
    function TotalCommissionAmount() {   // :4214
        var rate = $id('txtcommrate').value, type = selText('combCommType');
        if (rate !== '' && type === 'Flat') $id('txtcommamount').value = fmtRate(toDouble(rate.trim()));
        if (rate !== '' && (type === 'Percent' || type === 'Percentage')) $id('txtcommamount').value = fmtAmt(sumGrid('Amount') * toDouble(rate.trim()) / 100);
        if (rate !== '' && type === 'Comm Weight') return;   // Q-C2: the desktop reads a grid column "NetBillWeight" that does not exist; the exception skips the amount and CommissionProportion
        CommissionProportion();
    }
    function CommissionProportion() {   // :3917
        var total = toDouble($id('txtcommamount').value), pct = toDouble($id('txtcommrate').value), w = sumGrid('NetWeight');
        st.grid.forEach(function (r) { r.Commission = total > 0 ? (selText('combCommType') === 'Percent' ? toDouble(r.Amount) * pct / 100 : total / w * toDouble(r.NetWeight)) : 0; });
        BillProportion();
    }
    function ExpProportion() {   // :3830
        var t = 0; st.invExp.forEach(function (e) { t += toDouble(e.Amount); }); var w = sumGrid('NetWeight');
        st.grid.forEach(function (r) { r.Expense = t > 0 ? t / w * toDouble(r.NetWeight) : 0; }); BillProportion();
    }
    function FreightProportion() {   // :3865
        var w = sumGrid('NetWeight'), c = 0; st.freight.forEach(function (f) { c += toDouble(f.Freight); });
        st.grid.forEach(function (r) { r.Freights = c > 0 ? c / w * toDouble(r.NetWeight) : 0; }); BillProportion();
    }
    function BillProportion() {   // :3899
        st.grid.forEach(function (r) { r.BillAmount = afz(toDouble(r.Amount) + toDouble(r.Expense) + toDouble(r.Freights) + toDouble(r.Commission), st.N); });
        renderGridValues();
    }
    function FrightCalculations() {   // :4253
        var amount = sumGrid('Amount'), qty = sumGrid('ItemQty');
        if (amount > 0) st.freight.forEach(function (f) { if (toDouble(f.Freight) > 0) f.Freight = toDouble(f.Percentage) > 0 ? amount * toDouble(f.Percentage) / 100 : qty * toDouble(f.Rate); });
        renderFreight();
    }
    function BillAmount() {   // :3777
        try {
            var item = bankers(sumGrid('Amount'), 0), exp = 0; st.invExp.forEach(function (e) { exp += toDouble(e.Amount); });
            var dr = 0, cr = 0;
            st.gl.forEach(function (g) { var a = String(g.AccountId == null ? '' : g.AccountId).trim(); if (!/^-?\d+$/.test(a)) throw new Error('Input string was not in a correct format.'); if (parseInt(a, 10) > 0) { dr += toDouble(g.Debit); cr += toDouble(g.Credit); } });
            var tCr = 0; st.freight.forEach(function (f) { if (toInt(f.Transporter) > 0 && String(st.supplierGlId) === String(f.Transporter)) tCr += toDouble(f.Freight); });
            var bill = item + exp, diff = cr - dr;
            bill = diff < 0 ? bill - Math.abs(diff) : bill + diff;
            bill += tCr;
            if (num($id('comsupplier').value) === num($id('combcommAgent').value)) bill += toDouble($id('txtcommamount').value.trim());
            $id('txtBillAmount').value = fmtAmt(bill);
            BillProportion();
        } catch (e) { box(e.message); }
    }
    function txtExchangeRate_TextChanged() {   // :1399
        var rate = toDouble($id('txtExchangeRate').value);
        st.grid.forEach(function (r) { r.FcyAmount = st.grid.length > 0 && rate > 0 ? bankers(toDouble(r.BillAmount) / rate, st.F) : 0; });
        CalculateTotalInformation();
    }
    function CalculateTotalInformation() {   // :1432
        if (st.grid.length) {
            $id('txtOrderQty').value = fmt2(bankers(sumGrid('ItemQty'), 2)); $id('txtOrderWeight').value = fmt2(bankers(sumGrid('NetWeight'), 2)); $id('txtFcyAmount').value = fmtFcy(sumGrid('FcyAmount'));
        } else { $id('txtOrderQty').value = '0'; $id('txtOrderWeight').value = '0'; $id('txtFcyAmount').value = '0'; }
    }

    // ------------------------------------------------------------------ grids (grdSettings :1709 and the sub-grids)
    var GRID_COLS = [['GdnNo', 'i'], ['Warehouse', 's'], ['ItemCode', 's'], ['Item', 's'], ['CropYear', 's'], ['JobLot', 's'], ['PackingType', 's'], ['UOM', 's'], ['ItemQty', 'g'],
        ['GrossWeight', 'n2'], ['EBUnit', 'g', 'EB/Unit'], ['EBTotal', 'g'], ['WeightCut', 'g'], ['WeightCutTotal', 'g'], ['AdLs', 'g', 'Ad/Ls'], ['NetWeight', 'n2'], ['StockWeight', 'n2'], ['ItemRate', 'r'],
        ['RateUOM', 's'], ['RateCut', 'r'], ['RateCutTotal', 'a'], ['Amount', 'a'], ['GpDate', 'd'], ['GpNo', 'i'], ['VehicleNo', 's'], ['RemarksDetail', 's'], ['BillAmount', 'a'],
        ['Commission', 'n3'], ['Freights', 'n3'], ['Expense', 'n3'], ['BranchName', 's'], ['RefDocumentType', 's'], ['RefDocNo', 'i']];
    var SUMS = { ItemQty: 1, GrossWeight: 1, EBTotal: 1, NetWeight: 1, StockWeight: 1, WeightCut: 1, WeightCutTotal: 1, Amount: 1, RateCutTotal: 1, BillAmount: 1, Commission: 1, Freights: 1, Expense: 1 };
    function fmtCell(kind, v) {
        switch (kind) { case 'n2': return fmt2(v); case 'n3': return fmt3(v); case 'r': return fmtRate(v); case 'a': return fmtAmt(v); case 'd': return gridDate(v); case 'g': return String(toDouble(v)); default: return v == null ? '' : String(v); }
    }
    function visibleCols() {
        var r0 = st.grid[0] || {};   // grdSettings reads the CurrentRow; the first row stands for it here
        return GRID_COLS.filter(function (c) {
            if (c[0] === 'GdnNo') return num(r0.GdnId) > 0;
            if (c[0] === 'RefDocNo' || c[0] === 'RefDocumentType') return num(r0.RefDocId) > 0;
            if (c[0] === 'BranchName') return st.flags.branchFeature && !st.flags.branchImplemented;
            return true;
        });
    }
    function renderGrid() {
        var cols = visibleCols(), r0 = st.grid[0] || {}, del = num(r0.GdnId) === 0, edit = !!rights().Update, t = $id('grd');
        t.querySelector('thead').innerHTML = '<tr>' + (del ? '<th>Delete</th>' : '') + (edit ? '<th>Edit</th>' : '') + cols.map(function (c) { return '<th>' + esc(c[2] || c[0]) + '</th>'; }).join('') + '</tr>';
        t.querySelector('tbody').innerHTML = st.grid.map(function (r, i) {
            return '<tr data-i="' + i + '">' + (del ? '<td class="c"><button type="button" class="x" data-del="' + i + '">Delete</button></td>' : '') + (edit ? '<td class="c"><button type="button" class="x" data-edit="' + i + '">Edit</button></td>' : '')
                + cols.map(function (c) { var cls = c[1] === 's' || c[1] === 'd' ? '' : c[1] === 'i' ? 'c' : 'n';
                    if (c[0] === 'RefDocNo') return '<td class="' + cls + '" data-k="' + c[0] + '">' + docLink(r.RefDocumentTypeId, r.RefDocId, fmtCell(c[1], r[c[0]])) + '</td>';   // the source invoice opens through DocLink
                    return '<td class="' + cls + '" data-k="' + c[0] + '">' + esc(fmtCell(c[1], r[c[0]])) + '</td>'; }).join('') + '</tr>';
        }).join('');
        renderGridValues();
    }
    function renderGridValues() {
        var cols = visibleCols(), r0 = st.grid[0] || {}, lead = (num(r0.GdnId) === 0 ? 1 : 0) + (rights().Update ? 1 : 0), body = $id('grd').querySelector('tbody');
        st.grid.forEach(function (r, i) { var tr = body.children[i]; if (!tr) return; cols.forEach(function (c) { if (c[0] === 'RefDocNo') return; var td = tr.querySelector('td[data-k="' + c[0] + '"]'); if (td) td.textContent = fmtCell(c[1], r[c[0]]); }); });
        $id('grd').querySelector('tfoot').innerHTML = st.grid.length ? '<tr>' + (lead ? '<td colspan="' + lead + '"></td>' : '') + cols.map(function (c) {
            if (!SUMS[c[0]]) return '<td></td>'; return '<td>' + esc(fmtCell(c[1], sumGrid(c[0]))) + '</td>';
        }).join('') + '</tr>' : '';
    }
    function accountOptions(sel) { return '<option value="0"></option>' + (st.look.accounts || []).map(function (a) { return '<option value="' + esc(a.Id) + '"' + (String(a.Id) === String(sel) ? ' selected' : '') + '>' + esc(a.AccountTitle) + '</option>'; }).join(''); }
    function subCell(g, i, k, v, cls) { return '<td><input class="' + (cls || 'n') + '" data-g="' + g + '" data-i="' + i + '" data-k="' + k + '" value="' + esc(v == null ? '' : v) + '"></td>'; }
    function subButtons(g, i) { return '<td class="c"><button type="button" class="x" data-gdel="' + g + '" data-i="' + i + '">X</button></td><td class="c"><button type="button" class="x" data-gadd="' + g + '" data-i="' + i + '">+</button></td>'; }
    function blankExp() { return { Id: 0, GdnId: 0, GdnExpId: 0, GdnNo: 0, ItemId: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }
    function blankGl() { return { AccountId: 0, Remarks: '', Percentage: 0, Qty: 0, Rate: 0, Debit: 0, Credit: 0 }; }
    function blankFreight() { return { Transporter: 0, Percentage: 0, Qty: 0, Rate: 0, Freight: 0, Remarks: '' }; }
    function renderInvExp() {   // grdInvExpSettings :1954
        if (!st.invExp.length) st.invExp.push(blankExp());
        var t = $id('grdInvExp'), showGdn = num((st.invExp[0] || {}).GdnId) > 0;
        t.querySelector('thead').innerHTML = '<tr><th>X</th><th>+</th>' + (showGdn ? '<th>GdnNo</th>' : '') + '<th style="min-width:250px">Other Item</th><th>Qty</th><th>Rate</th><th>Amount</th><th style="min-width:400px">Remarks</th></tr>';
        t.querySelector('tbody').innerHTML = st.invExp.map(function (e, i) {
            return '<tr>' + subButtons('invExp', i) + (showGdn ? '<td class="c">' + esc(e.GdnNo) + '</td>' : '')
                + '<td><select class="win-combo" data-g="invExp" data-i="' + i + '" data-k="ItemId"><option value="0"></option>' + (st.look.otherItems || []).map(function (o) { return '<option value="' + esc(SC.ci(o, 'Id')) + '"' + (String(SC.ci(o, 'Id')) === String(e.ItemId) ? ' selected' : '') + '>' + esc(SC.ci(o, 'OtherItemName')) + '</option>'; }).join('') + '</select></td>'
                + subCell('invExp', i, 'Qty', e.Qty) + subCell('invExp', i, 'Rate', e.Rate) + subCell('invExp', i, 'Amount', e.Amount) + subCell('invExp', i, 'Remarks', e.Remarks, 'wide') + '</tr>';
        }).join('');
        var s = 0; st.invExp.forEach(function (e) { s += toDouble(e.Amount); });
        t.querySelector('tfoot').innerHTML = '<tr><td colspan="' + (showGdn ? 6 : 5) + '"></td><td>' + esc(fmtAmt(s)) + '</td><td></td></tr>';
    }
    function renderGL() {   // gridGLSettings :2221
        if (!st.gl.length) st.gl.push(blankGl());
        var t = $id('grdGLedger');
        t.querySelector('thead').innerHTML = '<tr><th>X</th><th>+</th><th style="min-width:250px">Account</th><th style="min-width:300px">Remarks</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Debit</th><th>Credit</th></tr>';
        t.querySelector('tbody').innerHTML = st.gl.map(function (g, i) {
            return '<tr>' + subButtons('gl', i) + '<td><select class="win-combo" data-g="gl" data-i="' + i + '" data-k="AccountId">' + accountOptions(g.AccountId) + '</select></td>'
                + subCell('gl', i, 'Remarks', g.Remarks, 'wide') + ['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'].map(function (k) { return subCell('gl', i, k, g[k]); }).join('') + '</tr>';
        }).join('');
        var d = 0, c = 0; st.gl.forEach(function (g) { d += toDouble(g.Debit); c += toDouble(g.Credit); });
        t.querySelector('tfoot').innerHTML = '<tr><td colspan="7"></td><td>' + esc(fmtAmt(d)) + '</td><td>' + esc(fmtAmt(c)) + '</td></tr>';
    }
    function renderFreight() {   // grdFreightSettings :2078
        if (!st.freight.length) st.freight.push(blankFreight());
        var t = $id('grdFreight');
        t.querySelector('thead').innerHTML = '<tr><th>X</th><th>+</th><th style="min-width:' + (st.freightCaption === 'Transporter' ? 250 : 300) + 'px">' + esc(st.freightCaption) + '</th><th>Percentage</th><th>Qty</th><th>Rate</th><th>Debit Amount</th><th style="min-width:500px">Remarks</th></tr>';
        t.querySelector('tbody').innerHTML = st.freight.map(function (f, i) {
            return '<tr>' + subButtons('freight', i) + '<td><select class="win-combo" data-g="freight" data-i="' + i + '" data-k="Transporter">' + accountOptions(f.Transporter) + '</select></td>'
                + ['Percentage', 'Qty', 'Rate', 'Freight'].map(function (k) { return subCell('freight', i, k, f[k]); }).join('') + subCell('freight', i, 'Remarks', f.Remarks, 'wide') + '</tr>';
        }).join('');
        var c = 0; st.freight.forEach(function (f) { c += toDouble(f.Freight); });
        t.querySelector('tfoot').innerHTML = '<tr><td colspan="6"></td><td>' + esc(fmtAmt(c)) + '</td><td></td></tr>';
    }
    function numericWarning(v) { if (String(v).trim() !== '' && !isFinite(Number(String(v).replace(/,/g, '')))) box('Please Type Only Numeric Value'); }   // UpdatingCell
    function subGridEdit(g, i, k, value) {
        var row = st[g][i]; if (!row) return;
        if (g === 'invExp') {   // grdInvExp_CellUpdated :2041
            if (['Qty', 'Rate', 'Amount'].indexOf(k) >= 0) numericWarning(value);
            row[k] = k === 'ItemId' ? toInt(value) : value;
            if ((k === 'Qty' || k === 'Rate') && String(row.Qty) !== '' && String(row.Rate) !== '') row.Amount = afz(toDouble(row.Qty) * toDouble(row.Rate), st.N);
            renderInvExp(); BillAmount(); ExpProportion();
        } else if (g === 'gl') {   // grdGLedger_CellUpdated :2297
            if (['Percentage', 'Qty', 'Rate', 'Debit', 'Credit'].indexOf(k) >= 0) numericWarning(value);
            row[k] = k === 'AccountId' ? toInt(value) : value;
            if ((k === 'Qty' || k === 'Rate') && String(row.Qty) !== '' && String(row.Rate) !== '') { row.Credit = afz(toDouble(row.Qty) * toDouble(row.Rate), st.N); row.Debit = 0; row.Percentage = 0; }
            if (k === 'Percentage' && String(row.Percentage) !== '') { var t = sumGrid('Amount') / 100 * toDouble(row.Percentage); if (t > 0) { row.Credit = fmtAmt(t); row.Debit = 0; } else { row.Debit = fmtAmt(Math.abs(t)); row.Credit = 0; } row.Qty = 0; row.Rate = 0; }
            if (k === 'Credit' && toDouble(row.Debit) > 0) { row.Credit = 0; box('Debit Side is aleady added'); }
            if (k === 'Debit' && toDouble(row.Credit) > 0) { row.Debit = 0; box('Credit Side is aleady added'); }
            renderGL(); BillAmount();
        } else if (g === 'freight') {   // grdFreight_CellUpdated :2159
            if (k === 'Freight') numericWarning(value);
            row[k] = k === 'Transporter' ? toInt(value) : value;
            if ((k === 'Qty' || k === 'Rate') && String(row.Qty) !== '' && String(row.Rate) !== '') { row.Freight = afz(toDouble(row.Qty) * toDouble(row.Rate), st.N); row.Percentage = 0; }
            if (k === 'Percentage' && String(row.Percentage) !== '') { var p = sumGrid('Amount') / 100 * toDouble(row.Percentage); row.Freight = p > 0 ? fmtAmt(p) : 0; row.Qty = 0; row.Rate = 0; }
            renderFreight(); BillAmount(); FreightProportion();
        }
    }
    function subGridDelete(g, i) {   // ColumnButtonClick "Delete"
        st[g].splice(i, 1);
        if (g === 'invExp') { renderInvExp(); BillAmount(); }                     // no ExpProportion (Q-G1)
        else if (g === 'gl') { renderGL(); BillAmount(); }
        else { renderFreight(); BillAmount(); FreightProportion(); }
    }
    function subGridAdd(g, i) {   // ColumnButtonClick "Add"
        if (g === 'invExp') { st.invExp.push(blankExp()); renderInvExp(); BillAmount(); }
        else if (g === 'gl') { st.gl.push(blankGl()); renderGL(); BillAmount(); }
        else { var cur = st.freight[i] || blankFreight(); st.freight.push({ Transporter: cur.Transporter, Percentage: 0, Qty: 0, Rate: 0, Freight: 0, Remarks: cur.Remarks }); renderFreight(); BillAmount(); FreightProportion(); }   // Q-G2: copies account + remarks
    }

    // ------------------------------------------------------------------ header events
    function comsupplier_ValueChanged() {   // :4331
        BillAmount();
        var s = selRow('comsupplier', st.look.suppliers); if (s) st.supplierGlId = String(SC.ci(s, 'GlAccountId'));
    }
    function txtduedays_TextChanged() {   // :4475 - from today, not from the document date
        var d = $id('txtduedays').value.trim();
        $id('duedate').value = d !== '' ? addDays(today(), Math.trunc(toDouble(d))) : today();
    }
    function multiCurrencyFeature() {   // :1363
        var mc = !!st.flags.hasMultiCurrencyFeature;
        ['cmbCurrency', 'txtExchangeRate', 'txtFcyAmount', 'label43', 'label57', 'label58'].forEach(function (id) { $id(id).classList.toggle('is-hidden', !mc); });
        $id('txtremarks').style.width = mc ? '437px' : '652px';
    }
    function DefaultConfigurations() {   // :1483
        var d = st.flags.defaults || {};
        if (d.jobLotId != null) setValue('comjobLot', d.jobLotId);
        if (d.baseCurrencyId != null) setValue('cmbCurrency', d.baseCurrencyId);
        if (d.baseCurrencyRate != null) { $id('txtExchangeRate').value = fmtRate(d.baseCurrencyRate); txtExchangeRate_TextChanged(); }
        var byCode = d.itemSearchByCode === true;   // Q-C1
        if (($id('rdSearchByCode').checked) !== byCode) { $id('rdSearchByCode').checked = byCode; $id('rdSearchByName').checked = !byCode; bindItems(); }
    }

    // ------------------------------------------------------------------ save (Insert :2399)
    function Insert() {
        try {
            if (!st.grid.length) throw new Error('Grid Record Not Found');
            // FormValidation :676
            var docNo = $id('txtdocno').value.trim();
            if (docNo === '' || docNo === '0') { box('DocNo Field is Required'); $id('txtdocno').focus(); return; }
            if (num($id('comsupplier').value) === 0) { box('Supplier Name Field is Required'); $id('comsupplier').focus(); return; }
            if (st.flags.hasMultiCurrencyFeature) {
                if (num($id('cmbCurrency').value) === 0) { box('Fcy Code Field is Required'); $id('cmbCurrency').focus(); return; }
                if (toDouble($id('txtExchangeRate').value) === 0) { box('Exchange Rate Field is Required'); $id('txtExchangeRate').focus(); return; }
                if (toDouble($id('txtFcyAmount').value) === 0) { box('Fcy Amount Field is Required'); $id('txtFcyAmount').focus(); return; }
            } else {
                if (num($id('cmbCurrency').value) === 0) { box('Please Configure Your Base Currency In configurations'); return; }
                var er = $id('txtExchangeRate').value.trim(); if (er === '' || er === '0') { box('Please Configure Your Base Currency Rate In configurations'); return; }
            }
            if (!confirm(st.recId > 0 ? 'Are you sure to Update?' : 'Are you sure to Save?')) return;
            var payload = {
                Id: st.recId, DocNo: docNo, DocDate: $id('DocDate').value, SupplierCustomerId: num($id('comsupplier').value),
                ManualBillNo: $id('txtbillno').value.trim(), SupplierReferenceNo: $id('txtSupplierReference').value, RemarksHeader: $id('txtremarks').value.trim(),
                DueDaysText: $id('txtduedays').value, DueDate: $id('duedate').value, DeliveryTerm: selText('combdeliverytrm'),
                CommAmount: toDouble($id('txtcommamount').value), CommissionAgentId: num($id('combcommAgent').value), CommissionRemarks: $id('txtCommissionRemarks').value.trim(),
                CommissionType: selText('combCommType').trim(), CommRate: toDouble($id('txtcommrate').value), UomScheduleIdCmRate: selText('combcommUOM'),
                CurrencyId: num($id('cmbCurrency').value), ExchangeRate: $id('txtExchangeRate').value.trim().replace(/,/g, ''), FcyAmount: toDouble($id('txtFcyAmount').value),
                CustomAccounts: $id('chkCustomAccounts').checked,
                details: st.grid, freight: st.freight, journal: st.gl,
                expenses: st.invExp.map(function (e) { var o = Object.assign({}, e); var it = (st.look.otherItems || []).find(function (x) { return String(SC.ci(x, 'Id')) === String(e.ItemId); }); o.ItemName = it ? SC.ci(it, 'OtherItemName') : ''; return o; }),
                attachments: attachmentEditor.payload()
            };
            var btn = st.recId > 0 ? $id('btnUpdate') : $id('btnSave');
            SC.withBusy(btn, function () {
                return post(API + '/save', payload).then(function (res) {
                    box(res.message);
                    if (res.openWagesBill) msg('Contractor wages bill (frmwagesBillHeader, RefDocTypeId 59, RefDocId ' + res.id + ', GrossWeight ' + res.grossWeightTotal + ') is a separate screen - open Contractor Wages to enter it.', false);
                    var voucher = $id('ChkBok').checked, slip = $id('ChkPrintslip').checked;
                    Reset();
                    if (voucher) printRpt('118-AcRptVoucherSlip.rpt', { id: num(res.voucherHeadId), documentTypeId: 59 });
                    if (slip) printSlip(num(res.id), num(res.supplierCustomerId), res.docDate);
                });
            }).catch(function (e) { box(e.message); });
        } catch (e) { box(e.message); }
    }

    // ------------------------------------------------------------------ Reset :2876
    function Reset() {
        attachmentEditor.reset();
        st.recId = 0; st.voucherHeadId = 0; $id('lblRecId').textContent = '';
        $id('comsupplier').value = ''; st.supplierGlId = '';
        ['txtSupplierReference', 'txtbillno', 'txtCommissionRemarks', 'txtcommrate', 'txtcommamount', 'txtremarks', 'txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut',
            'txtWeightCutTotal', 'txtAddLss', 'txtNetBillWeight', 'txtStockWeight', 'txtRate', 'txtratecut', 'txtratecuttotal', 'txtAmount', 'txtBillAmount', 'txtFcyAmount', 'txtExchangeRate', 'txtOrderQty', 'txtOrderWeight'].forEach(function (id) { $id(id).value = ''; });
        ['combcommAgent', 'combCommType', 'combcommUOM', 'comItem', 'comPackUOM', 'comRateUOM'].forEach(function (id) { $id(id).value = ''; });
        $id('txtduedays').value = '0'; txtduedays_TextChanged();
        st.freight = []; st.invExp = []; st.gl = []; st.grid = [];
        renderGL(); renderInvExp(); renderFreight(); renderGrid();
        EnableDisableDetailFields(0);
        $id('btnSave').classList.remove('is-hidden'); $id('btnUpdate').classList.add('is-hidden'); detailButtonsAdd();
        $id('chkCustomAccounts').checked = false;
        DocumentNo(); DefaultConfigurations();
        $id('DocDate').focus();
    }
    function DocumentNo() { return get(API + '/next-code').then(function (n) { if (num(n.docNo) > 0) $id('txtdocno').value = String(n.docNo); if (num(n.branchSrNo) > 0) $id('txtBranchSrNo').value = String(n.branchSrNo); }).catch(function (e) { box(e.message); }); }
    function EnableDisableDetailFields(kase, value) {   // :5197
        var all = ['comsupplier', 'comWarehouse', 'comItem', 'comCropYear', 'comjobLot', 'comPackingType', 'comPackUOM', 'txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut',
            'txtWeightCutTotal', 'txtAddLss', 'txtNetBillWeight', 'txtStockWeight', 'txtratecut', 'txtgpdate', 'txtgatepassno', 'txtvehicleno', 'comRateUOM', 'txtRate'];
        all.forEach(function (id) { $id(id).disabled = false; });
        var v = value === undefined ? true : value;
        if (kase === 1) ['comsupplier', 'comWarehouse', 'comItem', 'comCropYear', 'comjobLot', 'comPackingType', 'comPackUOM', 'txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtEmptyBagsTotal', 'txtwtcut', 'txtWeightCutTotal', 'txtAddLss', 'txtNetBillWeight', 'txtStockWeight'].forEach(function (id) { $id(id).disabled = !v; });
        if (kase === 2) ['comsupplier', 'comItem', 'comRateUOM', 'txtRate'].forEach(function (id) { $id(id).disabled = !v; });
    }

    // ------------------------------------------------------------------ ReadById :2746
    function ReadById(id) {
        return get(API + '/' + id).then(function (h) {
            var c = function (k) { return SC.ci(h, k); };
            st.recId = id; tab('tabForm');
            $id('txtBranchSrNo').value = String(c('BranchSrNo') == null ? '' : c('BranchSrNo')); $id('txtdocno').value = String(c('DocNo') == null ? '' : c('DocNo')); $id('DocDate').value = shortDate(c('DocDate'));
            setValue('comsupplier', c('SupplierCustomerId'));
            $id('txtSupplierReference').value = c('SupplierReferenceNo') || ''; $id('txtbillno').value = c('ManualBillNo') || '';
            setValue('combcommAgent', c('CommissionAgentId')); setText('combCommType', c('CommissionType')); $id('txtcommrate').value = String(c('CommRate') == null ? '' : c('CommRate'));
            setText('combcommUOM', c('UomScheduleIdCmRate')); $id('txtcommamount').value = String(c('CommAmount') == null ? '' : c('CommAmount')); $id('txtCommissionRemarks').value = c('CommissionRemarks') || '';
            $id('txtremarks').value = c('RemarksHeader') || ''; $id('txtBillAmount').value = String(c('BillAmount') == null ? '' : c('BillAmount'));
            $id('txtduedays').value = String(c('SupplierInvoiceNo') == null ? '' : c('SupplierInvoiceNo')); $id('duedate').value = shortDate(c('SupplierInvoiceDate')) || today();   // Q-R1
            setValue('cmbCurrency', c('CurrencyId')); $id('txtExchangeRate').value = String(c('ExchangeRate') == null ? '' : c('ExchangeRate')); $id('txtFcyAmount').value = String(c('FcyAmount') == null ? '' : c('FcyAmount'));
            $id('txtOrderQty').value = String(c('InvoiceQty') == null ? '' : c('InvoiceQty')); $id('txtOrderWeight').value = String(c('InvoiceWeight') == null ? '' : c('InvoiceWeight'));
            $id('chkCustomAccounts').checked = !!c('CustomAccounts');
            if (num(c('CurrencyId')) === 0 || toDouble(c('ExchangeRate')) === 0) DefaultConfigurations();
            st.voucherHeadId = num(c('voucherHeadId')); $id('lblRecId').textContent = 'Rec Id: ' + id + (c('IsApproved') ? '  (Approved)' : '');
            EnableDisableDetailFields(0);
            st.grid = (c('details') || []).map(function (d) {
                var v = function (k) { return SC.ci(d, k); }, r = blankRow();
                r.Id = num(v('Id')); r.GdnId = num(v('GdnId')); r.GdnDetailId = num(v('GdnDetailId')); r.GdnNo = num(v('GdnNo')); r.WarehouseId = num(v('WarehouseId')); r.Warehouse = v('WareHouseName') || '';
                r.ItemId = num(v('ItemId')); r.ItemCode = v('ItemCode') || ''; r.Item = v('ItemName') || ''; r.CropYear = v('CropYear') || ''; r.JobLotId = num(v('JobLotId')); r.JobLot = v('JobLotDescription') || '';
                r.PackingTypeId = num(v('PackingTypeId')); r.PackingType = v('PackTypeDesc') || ''; r.PackUOMId = num(v('ItemUOMId')); r.UOM = v('UOMCodeItem') || ''; r.UomEquivalent = toDouble(v('ItemUOMEquivalent'));
                r.ItemQty = toDouble(v('ItemQty')); r.GrossWeight = toDouble(v('GrossWeight')); r.EBUnit = toDouble(v('EBWeight')); r.EBTotal = bankers(toDouble(v('EBTotalWt')), 0);
                r.WeightCut = toDouble(v('WeightCut')); r.WeightCutTotal = bankers(toDouble(v('WeightCutTotal')), 0); r.AdLs = toDouble(v('AdLsWeight')); r.NetWeight = toDouble(v('NetBillWeight')); r.StockWeight = toDouble(v('NetStockWeight'));
                r.ItemRate = toDouble(v('ItemRate')); r.RateUOMId = num(v('UomScheduleIdRate')); r.RateUOM = v('RateUom') || ''; r.RateEquivalent = toDouble(v('EquivalentPoRate')); r.RateCut = toDouble(v('RateCut')); r.RateCutTotal = toDouble(v('RateCutAmount'));
                r.Amount = afz(toDouble(v('ItemAmount')), st.N); r.GpDate = shortDate(v('GpDate')); r.GpNo = num(v('GpNo')); r.VehicleNo = v('VehicleNo') || ''; r.RemarksDetail = v('RemarksDetail') || ''; r.FcyAmount = toDouble(v('FcyAmount'));
                r.BillAmount = afz(toDouble(v('BillAmount')), st.N); r.Commission = toDouble(v('CommissionAmount')); r.Freights = toDouble(v('FreightAmount')); r.Expense = toDouble(v('ExpenseAmount'));
                r.BranchId = num(v('BranchId')); r.BranchName = v('BranchName') || ''; r.RefDocumentTypeId = num(v('RefDocumentTypeId')); r.RefDocumentType = v('RefDocumentType') || ''; r.RefDocId = num(v('RefDocId'));
                r.RefDocNo = toInt(v('RefDocNo')); r.RefDocSubId = num(v('RefDocSubId')); r.RefInvoiceQty = toDouble(v('RefInvoiceQty')); r.RefInvoiceWeight = toDouble(v('RefInvoiceWeight'));
                if (!$id('comsupplier').disabled && r.GdnId > 0) EnableDisableDetailFields(1, false);
                else if (!$id('comsupplier').disabled && r.RefDocumentTypeId > 0 && r.RefDocId > 0) EnableDisableDetailFields(2, false);
                return r;
            });
            st.invExp = (c('expenses') || []).map(function (e) { var v = function (k) { return SC.ci(e, k); }; return { Id: num(v('Id')), GdnId: num(v('GdnId')), GdnExpId: num(v('GdnExpId')), GdnNo: num(v('GdnNo')), ItemId: num(v('InvRevExpItemId')), Qty: toDouble(v('Qty')), Rate: toDouble(v('Rate')), Amount: toDouble(v('Amount')), Remarks: v('Remarks') || '' }; });
            st.gl = (c('journal') || []).map(function (j) { var v = function (k) { return SC.ci(j, k); }; return { AccountId: num(v('ChartofAccountId')), Remarks: v('JvRemarks') || '', Percentage: toDouble(v('JvPrcnt')), Qty: toDouble(v('JvQty')), Rate: toDouble(v('JvRate')), Debit: toDouble(v('JvDebit')), Credit: toDouble(v('JvCredit')) }; });
            st.freight = (c('freight') || []).map(function (f) { var v = function (k) { return SC.ci(f, k); }; return { Transporter: num(v('TansporterId')), Percentage: toDouble(v('Percentage')), Qty: toDouble(v('FrQty')), Rate: toDouble(v('FrRate')), Freight: toDouble(v('FreightAmount')), Remarks: v('Remarks') || '' }; });
            comsupplier_ValueChanged();
            renderInvExp(); renderGL(); renderFreight(); renderGrid();
            ExpProportion(); CommissionProportion(); BillProportion(); BillAmount();
            $id('btnSave').classList.add('is-hidden'); $id('btnUpdate').classList.remove('is-hidden');
            txtExchangeRate_TextChanged();
        }).catch(function (e) { box(e.message); });
    }

    // ------------------------------------------------------------------ history (:3077-3700)
    function historyBranchIds() { var ids = ''; $id('cmbBranchNameList').querySelectorAll('input:checked').forEach(function (i) { ids += ',' + i.value; }); return ids; }
    function syncBranchText(listId, textId) { var names = []; $id(listId).querySelectorAll('input:checked').forEach(function (i) { names.push(i.parentNode.textContent.trim()); }); $id(textId).textContent = names.join(','); }
    function HistoryComboBranchFill() {   // :3563 - the text is the user's branch, so that branch is ticked
        var list = $id('cmbBranchNameList'), user = st.flags.userBranchName || '';
        list.innerHTML = st.historyBranches.map(function (b) { return '<label><input type="checkbox" value="' + esc(b.Id) + '"' + (b.BranchName === user ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>'; }).join('');
        if (!list.querySelector('input:checked') && st.historyBranches.length) list.querySelector('input').checked = true;
        syncBranchText('cmbBranchNameList', 'cmbBranchNameText');
    }
    function HistoryComboFill(validate) {   // :3620
        var ids = historyBranchIds();
        if (!ids) { if (validate) { box('Select branch first'); return Promise.resolve(); } ids = String(st.flags.userBranchId); }
        return get(API + '/history-suppliers' + SC.qs({ branchesIds: ids })).then(function (rows) { if ((rows || []).length) fill('cmbSupplierNameHistory', rows, 'Id', 'Supplier'); }).catch(function (e) { box(e.message); });
    }
    var HIST_COLS = [['DocNo', 'i'], ['BranchSrNo', 'i'], ['BranchName', 's'], ['DocDate', 'd'], ['SupplierName', 's'], ['ManualBillNo', 's'], ['DueDays', 'i'], ['DueDate', 'd'], ['ReferenceParty', 's', 'ReferencePartyName'],
        ['CommAgent', 's', 'CommissionAgent'], ['CommType', 's', 'CommissionType'], ['CommRate', 'r'], ['CommAmount', 'a'], ['CommRemarks', 's', 'CommissionRemarks'], ['BillAmount', 'a'], ['ApprovedStatus', 's'],
        ['EntryDate', 'dt'], ['EntryUser', 's'], ['ModifyDate', 'dt'], ['ModifyUser', 's'], ['NoOfAttachments', 'i'], ['Remarks', 's', 'RemarksHeader']];
    function GetAll() {
        var mode = document.querySelector('input[name=histDate]:checked').value;
        var q = { dateMode: mode, fromDocNo: toInt($id('txtFromDocNoHistory').value), toDocNo: toInt($id('txtToDocNoHistory').value), supplierId: num($id('cmbSupplierNameHistory').value) };
        if ($id('chkFromDateHistory').checked && $id('FromDateHistory').value) q.fromDate = $id('FromDateHistory').value;
        if ($id('chkToDateHistory').checked && $id('ToDateHistory').value) q.toDate = $id('ToDateHistory').value;
        SC.withBusy($id('btnshow'), function () {
            return get(API + '/history' + SC.qs(q)).then(function (rows) { st.historyRows = rows || []; renderHistory(); clearDetail(); });
        }).catch(function (e) { box(e.message); });
    }
    function clearDetail() { $id('grdDetail').querySelector('thead').innerHTML = ''; $id('grdDetail').querySelector('tbody').innerHTML = ''; $id('grdDetail').querySelector('tfoot').innerHTML = ''; }
    function renderHistory() {   // HistoryGridSettings :3190
        var t = $id('grdHistory'), showBranch = st.flags.branchFeature && !st.flags.branchImplemented, r = rights();
        if (!st.historyRows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; return; }
        var cols = HIST_COLS.filter(function (c) { return showBranch || (c[0] !== 'BranchSrNo' && c[0] !== 'BranchName'); });
        t.querySelector('thead').innerHTML = '<tr><th>Slip</th>' + (r.Update ? '<th>Edit</th>' : '') + (r.Print ? '<th>Voucher</th>' : '') + cols.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '<th>Add Attachment</th></tr>';
        t.querySelector('tbody').innerHTML = st.historyRows.map(function (row, i) {
            var v = function (k) { return SC.ci(row, k); }, id = num(v('Id'));
            return '<tr data-i="' + i + '" data-id="' + id + '"><td class="c"><button type="button" class="x" data-h="slip">Slip</button></td>'
                + (r.Update ? '<td class="c"><button type="button" class="x" data-h="edit">Edit</button></td>' : '') + (r.Print ? '<td class="c"><button type="button" class="x" data-h="voucher">Voucher</button></td>' : '')
                + cols.map(function (c) {
                    var val = v(c[2] || c[0]);
                    var txt = c[1] === 'dt' ? (val ? SC.gridDate(shortDate(val)) + ' 12:00 AM' : '') : fmtCell(c[1], val);   // ToShortDateString, then "dd-MMM-yyyy hh:mm tt" (Q-H1)
                    if (c[0] === 'NoOfAttachments') return '<td class="c"><a href="#" class="cx-link" data-h="attachments">' + esc(txt) + '</a></td>';
                    if (c[0] === 'DocNo') return '<td class="c"><a href="#" class="cx-link" data-h="edit">' + esc(txt) + '</a></td>';
                    return '<td class="' + (c[1] === 's' || c[1] === 'd' || c[1] === 'dt' ? '' : c[1] === 'i' ? 'c' : 'n') + '">' + esc(txt) + '</td>';
                }).join('') + '<td class="c"><button type="button" class="x" data-h="addAttachment">Add Attachment</button></td></tr>';
        }).join('');
        var ba = 0, ca = 0; st.historyRows.forEach(function (x) { ba += toDouble(SC.ci(x, 'BillAmount')); ca += toDouble(SC.ci(x, 'CommAmount')); });
        t.querySelector('tfoot').innerHTML = '<tr><td colspan="' + (1 + (r.Update ? 1 : 0) + (r.Print ? 1 : 0)) + '"></td>' + cols.map(function (c) { return '<td>' + (c[0] === 'BillAmount' ? esc(fmtAmt(ba)) : c[0] === 'CommAmount' ? esc(fmtAmt(ca)) : '') + '</td>'; }).join('') + '<td></td></tr>';
    }
    var DET_COLS = [['GdnNo', 'i', 'GdnNo'], ['Warehouse', 's', 'WareHouseName'], ['ItemCode', 's', 'ItemCode'], ['Item', 's', 'ItemName'], ['CropYear', 's', 'CropYear'], ['JobLot', 's', 'JobLotDescription'],
        ['PackingType', 's', 'PackTypeDesc'], ['UOM', 's', 'UOMCodeItem'], ['ItemQty', 'n2', 'ItemQty'], ['GrossWeight', 'n2', 'GrossWeight'], ['EB/Unit', 'g', 'EBWeight'], ['EBTotal', 'n2r', 'EBTotalWt'],
        ['WeightCut', 'g', 'WeightCut'], ['WeightCutTotal', 'n2r', 'WeightCutTotal'], ['Ad/Ls', 'g', 'AdLsWeight'], ['NetWeight', 'n2', 'NetBillWeight'], ['StockWeight', 'n2', 'NetStockWeight'], ['ItemRate', 'r', 'ItemRate'],
        ['RateUOM', 'g', 'EquivalentPoRate'], ['RateCut', 'r', 'RateCut'], ['RateCutAmount', 'a', 'RateCutAmount'], ['Amount', 'aa', 'ItemAmount'], ['GpDate', 'd', 'GpDate'], ['GpNo', 'i', 'GpNo'], ['VehicleNo', 's', 'VehicleNo'],
        ['BillAmount', 'aa', 'BillAmount'], ['Commission', 'a', 'CommissionAmount'], ['Freight', 'a', 'FreightAmount'], ['Expense', 'a', 'ExpenseAmount'], ['RemarksDetail', 's', 'RemarksDetail'], ['BranchName', 's', 'BranchName'],
        ['RefDocumentType', 's', 'RefDocumentType'], ['RefDocNo', 'i', 'RefDocNo']];
    var DET_SUMS = { Amount: 1, ItemQty: 1, NetWeight: 1, StockWeight: 1, RateCut: 1, RateCutAmount: 1, WeightCutTotal: 1, Commission: 1, EBTotal: 1, Expense: 1, Freight: 1, GrossWeight: 1 };
    function GetDetailGrdByHeadId(id) {   // :3333 (RateUOM shows EquivalentPoRate, as the desktop fills it)
        $id('grdHistory').querySelectorAll('tbody tr').forEach(function (tr) { tr.classList.toggle('sel', String(tr.dataset.id) === String(id)); });
        get(API + '/' + id).then(function (h) {
            var rows = SC.ci(h, 'details') || [], t = $id('grdDetail'); if (!rows.length) { clearDetail(); return; }
            var r0 = rows[0], showGdn = num(SC.ci(r0, 'GdnNo')) > 0, showRef = toInt(SC.ci(r0, 'RefDocNo')) > 0, showBranch = st.flags.branchFeature && !st.flags.branchImplemented;
            var cols = DET_COLS.filter(function (c) { if (c[0] === 'GdnNo') return showGdn; if (c[0] === 'RefDocNo' || c[0] === 'RefDocumentType') return showRef; if (c[0] === 'BranchName') return showBranch; return true; });
            var f = function (c, v) { return c[1] === 'n2r' ? fmt2(bankers(toDouble(v), 0)) : c[1] === 'aa' ? fmtAmt(v) : fmtCell(c[1], v); };
            t.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '</tr>';
            t.querySelector('tbody').innerHTML = rows.map(function (r) { return '<tr>' + cols.map(function (c) { return '<td class="' + (c[1] === 's' || c[1] === 'd' ? '' : c[1] === 'i' ? 'c' : 'n') + '">' + esc(f(c, SC.ci(r, c[2]))) + '</td>'; }).join('') + '</tr>'; }).join('');
            t.querySelector('tfoot').innerHTML = '<tr>' + cols.map(function (c) { if (!DET_SUMS[c[0]]) return '<td></td>'; var s = 0; rows.forEach(function (r) { s += c[1] === 'n2r' ? bankers(toDouble(SC.ci(r, c[2])), 0) : toDouble(SC.ci(r, c[2])); }); return '<td>' + esc(f(c, s)) + '</td>'; }).join('') + '</tr>';
        }).catch(function () { clearDetail(); });
    }
    function btnNewHistory_Click() {   // :3531
        $id('FromDateHistory').value = today(); $id('ToDateHistory').value = today(); $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = ''; $id('cmbSupplierNameHistory').value = '';
        st.historyRows = []; renderHistory(); clearDetail(); document.querySelector('input[name=histDate][value=doc]').checked = true;
    }
    function historyEdit(id) { Reset(); return ReadById(id).then(function () { CommissionProportion(); BillProportion(); }); }

    // ------------------------------------------------------------------ prints
    function printRpt(rpt, args, button) {
        var w = window.open('', '_blank'); try { if (w) w.document.write('<p style="font:13px Segoe UI">Preparing report...</p>'); } catch (e) { }
        /* the clicked button stays disabled until the PDF arrived or the request failed */
        return PurchaseRequest.run(button || null, function () { return fetch('/reports/print/by-template/' + encodeURIComponent(rpt) + '/pdf', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify(args || {}) })
            .then(function (r) {
                if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u, '_blank'); });
                return r.text().then(function (t) { if (w) w.close(); box(t || ('Print failed (' + r.status + ')')); });
            }).catch(function (e) { if (w) w.close(); box(e.message); }); });
    }
    /** GenerateReportSlip220A -> CommonServices.PurchaseInvoicettSlip_220A(InvoiceId, SupCustId, 0, Date, Date). */
    function printSlip(id, supplierId, date, button) { var d = shortDate(date) || today(); return printRpt('220A-InvRptPurchaseBillSupplierRiceSlip.rpt', { id: id, supplierCustomerId: supplierId, fromDate: d, toDate: d }, button); }
    function printVoucher(voucherId, button) { return printRpt('118-AcRptVoucherSlip.rpt', { id: voucherId, documentTypeId: 59 }, button); }

    // ------------------------------------------------------------------ Load Purchase Invoice (L:*)
    var LDR_COLS = [['BranchName', 's'], ['DocumentType', 's'], ['DocNo', 'i'], ['DocDate', 'd'], ['SupplierName', 's'], ['CommissionAgent', 's'], ['CommAmount', 'g'], ['PaymentTerm', 's', 'TermsDescription'],
        ['EntryDate', 'dt'], ['EntryUser', 's', 'EntryUserName'], ['ModifyDate', 'dt'], ['ModifyUser', 's', 'ModifyUserName'], ['ApprovalStatus', 's', 'IsApproved'], ['ApprovedDate', 'dt'], ['ApprovedUser', 's', 'ApprovedUserName'],
        ['BillAmount', 'g'], ['WareHouseName', 's'], ['ItemId', 'i'], ['ItemCode', 's'], ['ItemName', 's'], ['CropYear', 's'], ['JobLot', 's', 'JobLotCode'], ['PackUom', 's', 'PackUomCode'], ['ItemQty', 'g'],
        ['NetBillWeight', 'g'], ['ItemRate', 'g'], ['RateUom', 's', 'RateUomCode'], ['ItemAmount', 'g'], ['UsedQty', 'g'], ['UsedWeight', 'g'], ['UsedAmount', 'g'], ['BalQty', 'g'], ['BalWeight', 'g'], ['BalAmount', 'g'],
        ['GpNo', 'i'], ['VehicleNo', 's'], ['NoOfAttachments', 'i']];
    function loaderBranchIds() { var ids = ''; $id('ldrBranchList').querySelectorAll('input:checked').forEach(function (i) { ids += ',' + i.value; }); return ids; }
    function openLoader() {   // BtnLoadSaleInvoice_Click :5257
        try {
            var r0 = st.grid[0];
            if (r0) {
                if (num(r0.RefDocId) === 0 && num(r0.GdnId) === 0) throw new Error("You can't load Invoice because a direct entry already exists.");
                if (num(r0.RefDocId) === 0 && num(r0.GdnId) !== 0) throw new Error("You can't load Gdn because an entry against Gdn already exists.");
            }
            SC.openModal('mdlLoadInvoice');
            get(API + '/loader/init').then(function (d) {
                st.loader.branches = d.branches || []; st.loader.branchImplemented = !!d.branchImplemented;
                $id('ldrFromDate').value = d.fromDate; $id('ldrToDate').value = d.toDate;
                $id('ldrBranchList').innerHTML = st.loader.branches.map(function (b) { return '<label><input type="checkbox" value="' + esc(b.BranchId) + '"' + (b.BranchName === d.userBranchName ? ' checked' : '') + '> ' + esc(b.BranchName) + '</label>'; }).join('');
                if (!$id('ldrBranchList').querySelector('input:checked') && st.loader.branches.length) $id('ldrBranchList').querySelector('input').checked = true;
                syncBranchText('ldrBranchList', 'ldrBranchText');
                loaderSearch();
            }).catch(function (e) { box(e.message); });
        } catch (e) { box(e.message); }
    }
    function loaderSearch() {   // PendingGdnLoad
        if (!loaderBranchIds()) { box('Select branch first'); return; }
        SC.withBusy($id('ldrSearch'), function () {
            return get(API + '/loader/pending' + SC.qs({ fromDate: $id('ldrFromDate').value, toDate: $id('ldrToDate').value, branchesIds: loaderBranchIds() })).then(function (rows) { st.loader.rows = rows || []; renderLoader(); });
        }).catch(function (e) { box(e.message); });
    }
    function renderLoader() {
        var t = $id('ldrGrid'), cols = LDR_COLS.filter(function (c) { return c[0] !== 'BranchName' || !st.loader.branchImplemented; });
        t.querySelector('thead').innerHTML = '<tr><th><input type="checkbox" id="ldrAll"></th>' + cols.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '</tr>';
        t.querySelector('tbody').innerHTML = st.loader.rows.map(function (r, i) {
            return '<tr><td class="c"><input type="checkbox" data-ldr="' + i + '"></td>' + cols.map(function (c) {
                var v = SC.ci(r, c[2] || c[0]), txt = c[1] === 'dt' ? SC.gridDateTime(v, true) : c[0] === 'ApprovalStatus' ? (v ? 'True' : 'False') : fmtCell(c[1], v);
                /* L:grd_LinkClicked - NoOfAttachments -> GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId); DocNo opens the invoice (DocLink) */
                if (c[0] === 'NoOfAttachments') return '<td class="c"><a href="#" class="cx-link" data-ldr-att="' + i + '">' + esc(txt) + '</a></td>';
                if (c[0] === 'DocNo') return '<td class="c">' + docLink(SC.ci(r, 'DocumentTypeId'), SC.ci(r, 'Id'), txt) + '</td>';
                return '<td class="' + (c[1] === 's' || c[1] === 'd' || c[1] === 'dt' ? '' : c[1] === 'i' ? 'c' : 'n') + '">' + esc(txt) + '</td>';
            }).join('') + '</tr>';
        }).join('');
        var all = $id('ldrAll'); if (all) all.addEventListener('change', function () { t.querySelectorAll('input[data-ldr]').forEach(function (c) { c.checked = all.checked; }); });
    }
    function loaderLoad() {   // BtnLoad
        var checked = []; $id('ldrGrid').querySelectorAll('input[data-ldr]:checked').forEach(function (i) { var r = st.loader.rows[num(i.dataset.ldr)]; checked.push({ Id: num(SC.ci(r, 'Id')), DetailId: num(SC.ci(r, 'DetailId')) }); });
        if (!checked.length) { box('Chek the row first'); return; }
        SC.withBusy($id('ldrLoad'), function () {
            return post(API + '/loader/load', { checked: checked, fromDate: $id('ldrFromDate').value, toDate: $id('ldrToDate').value, branchesIds: loaderBranchIds() }).then(function (rows) {
                SC.closeModal('mdlLoadInvoice'); AddInGridDetailFromInvoice(rows || []);
            });
        }).catch(function (e) { box(e.message); });
    }
    function AddInGridDetailFromInvoice(rows) {   // :5306
        try {
            if (!rows.length) return;
            var v0 = function (k) { return SC.ci(rows[0], k); };
            setValue('combcommAgent', v0('CommissionAgentId'));
            $id('txtcommrate').value = fmtHash(v0('CommRate')); TotalCommissionAmount(); BillAmount();   // txtcommrate_TextChanged
            setText('combCommType', v0('CommissionType')); setText('combcommUOM', v0('CommUom'));
            $id('txtcommamount').value = fmtHash(v0('CommAmount'));
            // LoadDataDetailGridFromSaleInvoice :5349
            if ($id('comsupplier').value !== '') {
                if (num(v0('SupplierCustomerId')) !== num($id('comsupplier').value)) throw new Error("You Can't Load Grn Of Different Party At Same Time");
                if (st.grid.length && !st.grid.some(function (g) { return String(g.RefDocumentTypeId) === String(num(v0('DocumentTypeId'))); })) throw new Error("You Can't Load Grn Of Different DocumentType At Same Time");
            }
            setValue('comsupplier', v0('SupplierCustomerId')); comsupplier_ValueChanged();
            setText('combdeliverytrm', v0('DeliveryTerm'));
            EnableDisableDetailFields(2, false);
            rows.forEach(function (x) {
                var v = function (k) { return SC.ci(x, k); };
                if (st.grid.some(function (g) { return num(g.RefDocSubId) === num(v('DetailId')); })) return;
                var r = blankRow();
                r.WarehouseId = num(v('WarehouseId')); r.Warehouse = v('WareHouseName') || ''; r.ItemId = num(v('ItemId')); r.ItemCode = v('ItemCode') || ''; r.Item = v('ItemName') || ''; r.CropYear = v('CropYear') || '';
                r.JobLotId = num(v('JobLotId')); r.JobLot = v('JobLotCode') || ''; r.PackingTypeId = num(v('PackingTypeId')); r.PackingType = v('PackTypeDesc') || ''; r.PackUOMId = num(v('ItemUOMId')); r.UOM = v('PackUomCode') || '';
                r.UomEquivalent = toDouble(v('PackUomEquivalent')); r.ItemQty = toDouble(v('BalQty')); r.GrossWeight = toDouble(v('BalWeight')); r.EBUnit = toDouble(v('EBWeight')); r.EBTotal = toDouble(v('EBTotalWt'));
                r.WeightCut = toDouble(v('WtCut')); r.WeightCutTotal = toDouble(v('WtCutTotal')); r.AdLs = toDouble(v('AdLsWeight')); r.NetWeight = toDouble(v('BalWeight')); r.StockWeight = toDouble(v('StockWeight'));
                r.ItemRate = toDouble(v('ItemRate')); r.RateUOMId = num(v('RateUomId')); r.RateUOM = v('RateUomCode') || ''; r.RateEquivalent = toDouble(v('RateUomEquivalent')); r.RateCut = toDouble(v('RateCut')); r.RateCutTotal = toDouble(v('RateCutAmount'));
                r.Amount = toDouble(v('BalAmount')); r.GpDate = shortDate(v('GpDate')); r.GpNo = num(v('GpNo')); r.VehicleNo = v('VehicleNo') || '';
                r.BranchId = num(v('BranchIdDetail')); r.BranchName = v('BranchNameDetail') || ''; r.RefDocumentTypeId = num(v('DocumentTypeId')); r.RefDocumentType = v('DocumentType') || ''; r.RefDocId = num(v('Id'));
                r.RefDocNo = num(v('DocNo')); r.RefDocSubId = num(v('DetailId')); r.RefInvoiceQty = toDouble(v('BalQty')); r.RefInvoiceWeight = toDouble(v('BalWeight'));
                st.grid.push(r);
            });
            renderGrid();
            TotalCommissionAmount(); FreightProportion(); ExpProportion(); CommissionProportion(); BillProportion(); BillAmount();
        } catch (e) { box(e.message); }
    }

    // ------------------------------------------------------------------ attachments / rights / load
    var attachmentEditor = PurchaseInvoiceAttachments.create({ type: 59, getId: function () { return st.recId; }, canEdit: function () { return !!rights()[st.recId ? 'Update' : 'Save']; }, message: function (t) { msg(t, true); } });
    function applyRights() {   // Load :508-514
        var r = rights();
        $id('btnSave').disabled = !r.Save; $id('btnUpdate').disabled = !r.Update; $id('btnPrint').disabled = !r.Print; $id('ChkBok').disabled = !r.Print;
    }
    function bindLookups(l, first) {
        st.look = l;
        fill('comWarehouse', l.warehouses, 'Id', 'WareHouseName');                       // Rows[1].Activate, then Text cleared when nothing was selected (:803-814)
        fill('comsupplier', l.suppliers, 'Id', 'CompanyName'); fill('combcommAgent', l.suppliers, 'Id', 'CompanyName');
        fill('combpttrm', l.paymentTerms, 'Id', 'TermsDescription'); if (first && (l.paymentTerms || []).length) $id('combpttrm').selectedIndex = 1;
        fill('combdeliverytrm', l.deliveryTerms, 'Id', 'Value'); if (first) $id('combdeliverytrm').selectedIndex = 1;
        fill('combCommType', l.commissionTypes, 'Id', 'Value'); if (first) $id('combCommType').selectedIndex = 1;
        fill('combcommUOM', l.commissionUoms, 'Id', 'type'); if (first && (l.commissionUoms || []).length) $id('combcommUOM').selectedIndex = 1;
        bindItems();
        fill('comPackingType', l.packingTypes, 'Id', 'PackTypeDesc');
        fill('comjobLot', l.jobLots, 'Id', 'JobLotDescription');
        fill('comCropYear', l.cropYears, 'Id', 'CropYear');
        fill('cmbCurrency', l.currencies, 'Id', 'CurrencyCode');
        st.historyBranches = l.historyBranches || [];
    }
    function load() {
        return get(API + '/init').then(function (d) {
            st.flags = d; st.N = num(d.amountDecimals); st.R = num(d.rateDecimals); st.F = num(d.fcyDecimals);
            bindLookups(d.lookups || {}, true);
            HistoryComboBranchFill();
            $id('txtdocno').value = num(d.numbers.docNo) > 0 ? String(d.numbers.docNo) : ''; $id('txtBranchSrNo').value = num(d.numbers.branchSrNo) > 0 ? String(d.numbers.branchSrNo) : '';
            $id('DocDate').value = today(); $id('duedate').value = today(); $id('txtgpdate').value = today(); $id('FromDateHistory').value = today(); $id('ToDateHistory').value = today();
            $id('ChkBok').checked = !!rights().Print;
            renderGL(); renderInvExp(); renderFreight(); renderGrid();
            multiCurrencyFeature(); DefaultConfigurations(); applyRights();
            return HistoryComboFill(false);
        }).then(function () {
            var id = num(new URLSearchParams(location.search).get('id'));
            if (id > 0) return ReadById(id);
            $id('DocDate').focus();
        }).catch(function (e) { box(e.message); });
    }
    function btnRefresh_Click() {   // :2995
        SC.withBusy($id('btnRefresh'), function () {
            return get(API + '/init').then(function (d) {
                st.flags = d; bindLookups(d.lookups || {}, false);
                st.freightCaption = 'Transporter'; renderFreight(); renderGL(); renderInvExp();   // GridcomboBind re-captions the freight account column (Q-G3)
                DefaultConfigurations(); applyRights();
            });
        }).catch(function (e) { box(e.message); });
    }
    function tab(name) {
        document.querySelectorAll('.fx-tabs .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.dataset.tab === name); });
        $id('tabForm').classList.toggle('is-active', name === 'tabForm'); $id('tabHistory').classList.toggle('is-active', name === 'tabHistory');
        if (name === 'tabHistory') $id('FromDateHistory').focus();
    }
    function subTab(name) { document.querySelectorAll('.fx-subtabs .win-tab').forEach(function (t) { t.classList.toggle('is-active', t.dataset.sub === name); }); document.querySelectorAll('.fx-subpane').forEach(function (p) { p.classList.toggle('is-active', p.id === name); }); }
    function showShortcuts() {   // MakeShortCutKeys :5040
        var rows = [['Ctrl+S', 'For Save'], ['Ctrl+U', 'For Update'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Supplier Name'],
            ['Ctrl+F10', 'For Open Attachments'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+ArrowUp', 'For Focus On WareHouse in Detail Box'],
            ['Ctrl+ArrowRight', 'For Focus on Fright Grid'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        $id('shortcutRows').innerHTML = rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('');
        SC.openModal('mdlShortcuts');
    }

    // ------------------------------------------------------------------ wiring
    document.addEventListener('DOMContentLoaded', function () {
        $id('btnNew').addEventListener('click', Reset); $id('btnRefresh').addEventListener('click', btnRefresh_Click);
        $id('btnSave').addEventListener('click', function () { st.recId = 0; Insert(); }); $id('btnUpdate').addEventListener('click', Insert);
        $id('btnAttachment').addEventListener('click', function () { attachmentEditor.open(this); });
        $id('btnPrint').addEventListener('click', function () { if (rights().Update) printVoucher(st.voucherHeadId, $id('btnPrint')); });   // Q-P1: gated on Update, as :4802
        $id('btnSlip').addEventListener('click', function () { if (rights().Print) printSlip(st.recId, num($id('comsupplier').value), $id('DocDate').value, $id('btnSlip')); else box("You don't have Print right!!!"); });
        document.addEventListener('click', function (e) { var a = e.target.closest('a[data-doclink]'); if (!a) return; e.preventDefault(); if (window.DocLink) window.DocLink.open(num(a.dataset.doclink), num(a.dataset.docid), { message: box }); });
        /* grd_KeyDown :4996 - Ctrl+Enter (and Ctrl+Space on the Edit column) edits the row; Ctrl+Space on Delete deletes it. */
        $id('grd').addEventListener('keydown', function (e) {
            var tr = e.target.closest('tr[data-i]'); if (!tr || !e.ctrlKey) return; var i = num(tr.dataset.i);
            if (e.key === 'Enter') { e.preventDefault(); grd_DoubleClick(i); }
            else if (e.key === ' ' && e.target.dataset && e.target.dataset.del !== undefined) { e.preventDefault(); DeleteDetailrow(i); }
            else if (e.key === ' ' && e.target.dataset && e.target.dataset.edit !== undefined) { e.preventDefault(); grd_DoubleClick(i); }
        });
        /* grdHistory_KeyDown :4914 - Ctrl+Enter on the History page reads the selected row (no rights check there). */
        $id('grdHistory').addEventListener('keydown', function (e) {
            var tr = e.target.closest('tr[data-id]'); if (!tr || !e.ctrlKey || e.key !== 'Enter') return; e.preventDefault();
            ReadById(num(tr.dataset.id)).then(function () { ExpProportion(); CommissionProportion(); BillProportion(); });
        });
        $id('BtnLoader').addEventListener('click', function () {   // BtnLoader_Click :5072
            try {
                var r0 = st.grid[0];
                if (r0) { if (num(r0.GdnId) === 0 && num(r0.RefDocId) === 0) throw new Error("You can't load Gdn because a direct entry already exists."); if (num(r0.GdnId) === 0 && num(r0.RefDocId) !== 0) throw new Error("You can't load Gdn because a entry against Invoice already exists."); }
                box('Load Gdn (frmLoadGdnForPurchaseReturn) is not ported on the web: its desktop source is not in the migration sources.');
            } catch (e) { box(e.message); }
        });
        $id('BtnLoadSaleInvoice').addEventListener('click', openLoader);
        $id('btnShortCutKeys').addEventListener('click', showShortcuts);
        document.querySelectorAll('[data-close]').forEach(function (b) { b.addEventListener('click', function () { SC.closeModal(b.dataset.close); }); });
        document.querySelectorAll('.fx-tabs .win-tab').forEach(function (t) { t.addEventListener('click', function () { tab(t.dataset.tab); }); });
        document.querySelectorAll('.fx-subtabs .win-tab').forEach(function (t) { t.addEventListener('click', function () { subTab(t.dataset.sub); }); });
        // detail box
        $id('txtQty').addEventListener('input', txtQty_TextChanged);
        $id('txtGrossWeight').addEventListener('input', Total); $id('txtEmptyBagsTotal').addEventListener('input', Total);
        ['txtEmptybagsUnit', 'txtwtcut', 'txtAddLss'].forEach(function (id) { $id(id).addEventListener('input', weightInputChanged); });
        ['txtRate', 'txtratecut'].forEach(function (id) { $id(id).addEventListener('input', rateChanged); });
        $id('comRateUOM').addEventListener('change', rateChanged); $id('comPackUOM').addEventListener('change', comPackUOM_Leave);
        $id('comItem').addEventListener('change', function () { UomFromGlobalBind(num($id('comItem').value)).then(AvailableStockGetByItem); });
        $id('comWarehouse').addEventListener('change', function () { bindItems(); AvailableStockGetByItem(); });
        ['comCropYear', 'comjobLot', 'comPackingType'].forEach(function (id) { $id(id).addEventListener('change', AvailableStockGetByItem); });
        ['rdSearchByName', 'rdSearchByCode'].forEach(function (id) { $id(id).addEventListener('change', function () { var keep = $id('comItem').value; bindItems(); setValue('comItem', keep); $id('comItem').focus(); }); });
        ['txtQty', 'txtGrossWeight', 'txtEmptybagsUnit', 'txtwtcut', 'txtStockWeight', 'txtRate', 'txtratecut', 'txtcommrate', 'txtAddLss', 'txtExchangeRate', 'txtFcyAmount', 'txtOrderWeight', 'txtOrderQty'].forEach(function (id) { $id(id).addEventListener('keydown', decimalKey); });
        ['txtduedays', 'txtgatepassno', 'txtFromDocNoHistory', 'txtToDocNoHistory'].forEach(function (id) { $id(id).addEventListener('keydown', function (e) { if (e.key.length === 1 && !/\d/.test(e.key)) e.preventDefault(); }); });
        $id('btnAdd').addEventListener('click', btnAdd_Click); $id('btnUpdateDetail').addEventListener('click', btnUpdateDetail_Click);
        $id('btnCancelUpdateDetial').addEventListener('click', detailButtonsAdd);   // :1695 - only the buttons change
        // header
        $id('comsupplier').addEventListener('change', comsupplier_ValueChanged);
        ['combCommType', 'combcommUOM', 'combcommAgent'].forEach(function (id) { $id(id).addEventListener('change', function () { TotalCommissionAmount(); BillAmount(); }); });
        $id('txtcommrate').addEventListener('input', function () { TotalCommissionAmount(); BillAmount(); });
        $id('txtduedays').addEventListener('input', txtduedays_TextChanged);
        $id('txtExchangeRate').addEventListener('input', txtExchangeRate_TextChanged);
        $id('txtExchangeRate').addEventListener('blur', function () { $id('txtExchangeRate').value = fmtRate(toDouble($id('txtExchangeRate').value)); });
        // main grid
        $id('grd').addEventListener('click', function (e) { var d = e.target.closest('[data-del]'); if (d) { DeleteDetailrow(num(d.dataset.del)); return; } var ed = e.target.closest('[data-edit]'); if (ed) grd_DoubleClick(num(ed.dataset.edit)); });
        $id('grd').addEventListener('dblclick', function (e) { var tr = e.target.closest('tr[data-i]'); if (tr && !e.target.closest('button')) grd_DoubleClick(num(tr.dataset.i)); });
        // sub grids
        ['grdInvExp', 'grdGLedger', 'grdFreight'].forEach(function (id) {
            $id(id).addEventListener('change', function (e) { var c = e.target.closest('[data-g]'); if (c) subGridEdit(c.dataset.g, num(c.dataset.i), c.dataset.k, c.value); });
            $id(id).addEventListener('click', function (e) { var d = e.target.closest('[data-gdel]'); if (d) { subGridDelete(d.dataset.gdel, num(d.dataset.i)); return; } var a = e.target.closest('[data-gadd]'); if (a) subGridAdd(a.dataset.gadd, num(a.dataset.i)); });
        });
        // history
        $id('btnshow').addEventListener('click', GetAll); $id('btnNewHistory').addEventListener('click', btnNewHistory_Click);
        $id('btnRefreshHistory').addEventListener('click', function () { get(API + '/history-branches').then(function (b) { st.historyBranches = b || []; HistoryComboBranchFill(); return HistoryComboFill(true); }).catch(function (e) { box(e.message); }); });
        $id('cmbBranchName').querySelector('.fx-multi-btn').addEventListener('click', function () { var l = $id('cmbBranchNameList'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
        $id('cmbBranchNameList').addEventListener('change', function () { syncBranchText('cmbBranchNameList', 'cmbBranchNameText'); if (historyBranchIds()) HistoryComboFill(true); else fill('cmbSupplierNameHistory', [], 'Id', 'Supplier'); });
        $id('ldrBranch').querySelector('.fx-multi-btn').addEventListener('click', function () { var l = $id('ldrBranchList'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
        $id('ldrBranchList').addEventListener('change', function () { syncBranchText('ldrBranchList', 'ldrBranchText'); });
        document.addEventListener('mousedown', function (e) { if (!e.target.closest('.fx-multi')) document.querySelectorAll('.fx-multi-list').forEach(function (l) { l.style.display = 'none'; }); });
        $id('grdHistory').addEventListener('click', function (e) {
            var b = e.target.closest('[data-h]'), tr = e.target.closest('tr[data-id]'); if (!tr) return; var id = num(tr.dataset.id), row = st.historyRows[num(tr.dataset.i)];
            if (!b) { GetDetailGrdByHeadId(id); return; }
            e.preventDefault();
            var btn = b.tagName === 'BUTTON' ? b : null;
            if (b.dataset.h === 'edit') { if (btn) SC.withBusy(btn, function () { return historyEdit(id); }).catch(function (x) { box(x.message); }); else historyEdit(id); }
            else if (b.dataset.h === 'slip') printSlip(id, num(SC.ci(row, 'SupplierCustomerId')), SC.ci(row, 'DocDate'), btn);
            else if (b.dataset.h === 'voucher') printVoucher(num(SC.ci(row, 'VoucherHeadId')), btn);
            /* grdHistory_LinkClicked :3274 - GetNoofAttachmentsByRefDocumentTypeID(Id, 59): the attachment list, read-only */
            else if (b.dataset.h === 'attachments') PurchaseInvoiceAttachments.view({ type: 59, id: id, message: box });
            /* AttachmentAddingFromHistory: the web dialog stages the change with the invoice, so the record is opened first */
            else if (b.dataset.h === 'addAttachment') SC.withBusy(btn, function () { Reset(); return ReadById(id).then(function () { attachmentEditor.open(null); }); }).catch(function (x) { box(x.message); });
        });
        $id('grdHistory').addEventListener('dblclick', function (e) {   // :3503
            var tr = e.target.closest('tr[data-id]'); if (!tr) return;
            if (rights().Update) ReadById(num(tr.dataset.id)).then(function () { ExpProportion(); CommissionProportion(); BillProportion(); }); else box("You don't have right to update");
        });
        // loader
        $id('ldrSearch').addEventListener('click', loaderSearch); $id('ldrLoad').addEventListener('click', loaderLoad);
        $id('ldrGrid').addEventListener('click', function (e) { var a = e.target.closest('[data-ldr-att]'); if (!a) return; e.preventDefault(); var r = st.loader.rows[num(a.dataset.ldrAtt)]; if (r) PurchaseInvoiceAttachments.view({ type: num(SC.ci(r, 'DocumentTypeId')), id: num(SC.ci(r, 'Id')), message: box }); });
        $id('ldrShortcuts').addEventListener('click', function () { box('Ctrl+E For Close\nCtrl+L To Press Load Button\nCtrl+S For Search\nCtrl+alt To Show ShortCut Keys Form\nCtrl+Space When Focus On Any Grid To Call Function\'s On Button Or Link'); });
        // form KeyDown :4813
        document.addEventListener('keydown', function (e) {
            var onForm = $id('tabForm').classList.contains('is-active'), k = e.key.toLowerCase();
            if ($id('mdlLoadInvoice').classList.contains('is-open')) {
                if (e.ctrlKey && k === 's') { e.preventDefault(); loaderSearch(); } else if (e.ctrlKey && k === 'l') { e.preventDefault(); loaderLoad(); } else if ((e.ctrlKey && k === 'e') || k === 'escape') { SC.closeModal('mdlLoadInvoice'); }
                return;
            }
            if (e.ctrlKey && k === 's' && onForm) { e.preventDefault(); if (!$id('btnSave').classList.contains('is-hidden') && !$id('btnSave').disabled) { st.recId = 0; Insert(); } }
            else if (e.ctrlKey && k === 'n' && onForm) { e.preventDefault(); Reset(); }
            else if (e.ctrlKey && k === 't') { e.preventDefault(); tab(onForm ? 'tabHistory' : 'tabForm'); }
            else if (e.ctrlKey && k === 'u') { e.preventDefault(); if (!$id('btnUpdate').classList.contains('is-hidden') && !$id('btnUpdate').disabled) Insert(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); if (!$id('btnPrint').disabled && rights().Print && rights().Update) printVoucher(st.voucherHeadId); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh_Click(); }
            else if (e.ctrlKey && k === 'f10') { e.preventDefault(); attachmentEditor.open(null); }
            else if (e.ctrlKey && k === 'f5') { e.preventDefault(); $id('DocDate').focus(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); (onForm ? $id('grd') : $id('grdDetail')).scrollIntoView({ block: 'nearest' }); }
            else if (e.ctrlKey && k === 'arrowup') { e.preventDefault(); if (onForm) $id('comWarehouse').focus(); else $id('grdHistory').scrollIntoView({ block: 'nearest' }); }
            else if (e.ctrlKey && k === 'arrowright' && onForm) { e.preventDefault(); var order = ['tabFreight', 'tabOtherExpense', 'tabSupplierAddLess'], cur = document.querySelector('.fx-subpane.is-active').id; subTab(order[(order.indexOf(cur) + 1) % 3]); }
            else if (e.ctrlKey && e.altKey) { showShortcuts(); }
        });
        load();
    });
})();
