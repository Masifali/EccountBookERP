/* ============================================================================================
 * Screen 332 "Stock Transfer Manual" — Architecture.WinApp.StoreManagement.frmStockTransferManual,
 * DocumentTypeId 806.
 *
 * Flow as on the desktop:
 *   - Trans.Type is a free combo (TransferTypeId is always 0, PurchsaeOrder_Load:398), default "Inward".
 *   - Gate Pass# / Ticket No# / Factory Weight are TextBoxes; Factory Weight is recomputed as the grid's
 *     GrossWeight total (FactoryWeightCalculation:2158) after "+", row Update, row delete and the Loader.
 *   - Rows are typed in the Detail box ("+", btnplus_Click:879) or come from the "Loader" dialog
 *     (LoadavailableTransactionsForIssuance → LoadDataDetailfromPurchaseInvoivce:2358), never both.
 *   - Weights: txtQty / CmbPackUom → GrossWeightCalculation:2139 + WeightCalculation:2095.
 *   - Avg rate: GetAvgRate:2290 (AvgRateOnlyForCGS * 40); amount = net / 40 * rate formatted "#,#".
 *   - Other Charges (only with StockTransferFinancialEffectIsActive) spread over NetWeight as Expense.
 * Every save rule is repeated on the server; the client repeats the checks the desktop makes before
 * anything reaches the BLL.
 * ============================================================================================ */
(function () {
    'use strict';
    var C = window.StoreCommon, $id = C.$id, esc = C.esc, ci = C.ci, num = C.num, intOf = C.intOf;
    var api = '/api/store/stock-transfer-manual';

    var look = { rights: {}, items: [], accounts: [], warehouses: [], cropYears: [], jobLots: [], packingTypes: [] };
    var RecId = 0;
    var IsApproved = false;
    var DetailRowsRemoveIds = '';
    var table = [];                // "table" — grd
    var dtExp = [];                // dtExp — grdOtherCharges
    var updateDetailIndex = 0;
    var currentRow = -1;           // grd.CurrentRow
    var balVisible = true;         // grdSettings:1552 — BalQty / BalWeight columns
    var loaderRows = [], loaderChecked = {};
    var historyRows = [];

    /* ------------------------------------------------------------------ small helpers */

    /** .NET Framework double.ToString(): 15 significant digits. */
    function clr(v) { var n = num(v); if (n === 0) return '0'; return String(Number(n.toPrecision(15))); }
    /** Custom format "#,#" / "#,#.##": zero renders empty. */
    function hash(v, dec) {
        var n = num(v), f = Math.pow(10, dec || 0), r = Math.sign(n) * Math.round(Math.abs(n) * f) / f;
        if (r === 0) return '';
        return r.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dec || 0 });
    }
    /** "#,##0.##" style. */
    function fmt0(v, dec) { return num(v).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: dec }); }
    function selText(id) { var e = $id(id); return e && e.selectedOptions && e.selectedOptions[0] && intOf(e.value) !== 0 ? e.selectedOptions[0].textContent.trim() : ''; }
    function transferType() { var e = $id('CmbTransferType'); return e.selectedOptions[0] && e.value !== '0' ? e.selectedOptions[0].textContent : ''; }
    function setOptions(id, rows, valueKey, textKey, blank) { C.fillSelect(id, rows, valueKey, textKey, blank); }
    function clearCombo(id) { $id(id).innerHTML = '<option value="0"></option>'; }
    function keepOr(id, value, rows, key) {
        var el = $id(id);
        if (value > 0) el.value = rows.some(function (r) { return intOf(ci(r, key)) === value; }) ? String(value) : '0';
    }
    function selectByText(id, text) {
        var el = $id(id), t = String(text === null || text === undefined ? '' : text);
        var o = Array.prototype.find.call(el.options, function (x) { return x.value !== '0' && x.textContent.trim() === t; });
        el.value = o ? o.value : '0';
    }
    function show(id, on) { $id(id).classList.toggle('is-hidden', !on); }
    function fail(e) { alert(e && e.message ? e.message : e); }
    function digitsOnly(id, allowDot, allowMinus) {                 // OnlytextNumberFunction / OnlytextdecimelFunction / txtAdLsWeight_KeyPress:2562
        $id(id).addEventListener('keypress', function (e) {
            if (e.key.length !== 1) return;
            if (/[0-9]/.test(e.key)) return;
            if (allowDot && e.key === '.' && this.value.indexOf('.') < 0) return;
            if (allowMinus && e.key === '-') return;
            e.preventDefault();
        });
    }
    function isoOffset(days) {
        var d = new Date(); d.setDate(d.getDate() - days);
        return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0');
    }

    /* ------------------------------------------------------------------ load (PurchsaeOrder_Load:372) */

    function init() {
        $id('DocDate').value = C.today();
        ['txtHeadNetWeight', 'cmbTicketNo', 'cmbGatePass', 'txtFromDocNoHistory', 'txtToDocNoHistory'].forEach(function (id) { digitsOnly(id, false, false); });
        ['txtQty', 'txtGrossWeight', 'txtEbUnit', 'txtAvgRate'].forEach(function (id) { digitsOnly(id, true, false); });
        digitsOnly('txtAdLsWeight', true, true);
        C.getJson(api + '/lookups').then(function (l) {
            look = l;
            $id('btnsave').disabled = !l.rights.save;                         // :377
            $id('btnupdate').disabled = !l.rights.update;
            $id('btnDelete').disabled = !l.rights['delete'];
            document.querySelectorAll('.fin').forEach(function (e) { e.classList.toggle('is-hidden', !l.financialEffect); });   // :381-395
            show('tabHeadOther', !!l.financialEffect);
            setOptions('cmbTransactiontypeHistory', l.historyTransferTypes, 'TransferType', 'TransferType');
            dtExp = [blankExp()];                                              // AddRowInExpenseGrid:643
            fillCombos(l);
            $id('txtdocno').value = l.docNo > 0 ? String(l.docNo) : '';       // DocumentNoFill:1282
            setOptions('CmbTransferType', [{ Id: 1, T: 'Inward' }, { Id: 2, T: 'Outward' }, { Id: 3, T: 'MoveOrder' }], 'Id', 'T', false);
            $id('CmbTransferType').value = '1';                                // TransferTypeFill:1381 Rows[0].Activate()
            clearCombo('CmbPackUom');
            renderExp();
            renderGrid();
            $id('ChkBoxPrintPreview').checked = true;                          // :505
            var days = intOf(l.defaultDaysToLessFromHistoryFromDate) > 0 ? intOf(l.defaultDaysToLessFromHistoryFromDate) : 3;
            $id('FromDateHistory').value = isoOffset(days);                    // :508
            $id('ToDateHistory').value = C.today();
            $id('DocDate').focus();
        }).catch(fail);
    }

    /** WareHouseFill:1213 / GetJobLot:1389 keep a still-listed selection; the rest rebind. */
    function fillCombos(l) {
        function keep(id, rows, textKey) { var v = intOf($id(id).value); setOptions(id, rows, 'Id', textKey); keepOr(id, v, rows, 'Id'); }
        keep('CmbWareHouseFrom', l.warehouses, 'WareHouseName');
        keep('cmbWareHouseTo', l.warehouses, 'WareHouseName');
        keep('cmbJobLot', l.jobLots, 'JobLotDescription');
        keep('CmbJobLotTo', l.jobLots, 'JobLotDescription');
        keep('cmbPackingType', l.packingTypes, 'PackTypeDesc');
        keep('cmbCropYear', l.cropYears, 'CropYear');
        keep('cmbItemName', l.items, 'ItemName');
        look.items = l.items; look.cropYears = l.cropYears; look.warehouses = l.warehouses;
        look.jobLots = l.jobLots; look.packingTypes = l.packingTypes;
    }

    /* ------------------------------------------------------------------ entry bar */

    /** cmbItemName_TextChanged:2312 (GetAvgRate) + cmbItemName_Leave:2202 (UOMs, available stock). */
    function itemChanged() {
        return Promise.all([getAvgRate(), bindUoms(intOf($id('cmbItemName').value)).then(availableStock)]);
    }

    function bindUoms(itemId) {                                                  // bindRateUomAndItemPackUom:1310
        return C.getJson(api + '/uoms' + C.qs({ itemId: itemId })).then(function (dt) {
            if (dt.length > 0) {
                var prev = intOf($id('CmbPackUom').value);
                setOptions('CmbPackUom', dt.map(function (u) { return { Id: u.Id, Equivalent: clr(u.Equivalent) }; }), 'Id', 'Equivalent');
                keepOr('CmbPackUom', prev, dt, 'Id');
            }
        }).catch(fail);
    }

    /** CmbWareHouseFrom / cmbJobLot / CmbJobLotTo / cmbCropYear Leave: AvailableStockGetByItem:2052 + GetAvgRate:2290. */
    function stockAndRate() { return Promise.all([availableStock(), getAvgRate()]); }

    function availableStock() {
        return C.getJson(api + '/available-stock' + C.qs({
            recId: RecId, itemId: intOf($id('cmbItemName').value), warehouseId: intOf($id('CmbWareHouseFrom').value),
            jobLotId: intOf($id('cmbJobLot').value), cropYear: selText('cmbCropYear'), docDate: $id('DocDate').value
        })).then(function (r) { $id('txtAvailableStock').value = r.availableStock; }).catch(fail);
    }

    function getAvgRate() {
        return C.getJson(api + '/avg-rate' + C.qs({
            recId: RecId, itemId: intOf($id('cmbItemName').value), docDate: $id('DocDate').value,
            jobLotId: intOf($id('cmbJobLot').value), cropYearId: intOf($id('cmbCropYear').value),
            warehouseId: intOf($id('CmbWareHouseFrom').value)
        })).then(function (r) {
            if (num(r.rate) > 0) { $id('txtAvgRate').value = clr(r.rate); avgAmount(); }
            else { $id('txtAvgRate').value = '0'; $id('txtAmount').value = '0'; }
        }).catch(fail);
    }

    function avgAmount() {                                                       // AvgAmountCalculation:2219
        var net = num($id('txtDetailNetWeight').value), rate = num($id('txtAvgRate').value);
        $id('txtAmount').value = (net > 0 && rate > 0) ? hash(net / 40.0 * rate) : '0';
    }

    function grossCalc() {                                                       // GrossWeightCalculation:2139
        if ($id('txtQty').value.trim() !== '' && intOf($id('CmbPackUom').value) !== 0) {
            $id('txtGrossWeight').value = clr(num($id('txtQty').value) * num(selText('CmbPackUom')));
        }
        avgAmount();
    }

    function weightCalc() {                                                      // WeightCalculation:2095
        var qtyT = $id('txtQty').value, grT = $id('txtGrossWeight').value;
        var qty = (qtyT.trim() !== '' && qtyT !== '0') ? num(qtyT) : 0;
        var gross = (grT.trim() !== '' && grT !== '0') ? num(grT) : 0;
        var addless = $id('txtAdLsWeight').value.trim() !== '' ? num($id('txtAdLsWeight').value) : 0;
        var ebUnit = 0, ebTotal = 0;
        if ($id('txtEbUnit').value.trim() !== '') ebUnit = num($id('txtEbUnit').value);
        else $id('txtEbTotal').value = '0';
        if (ebUnit > 0) { ebTotal = qty * ebUnit; $id('txtEbTotal').value = clr(ebTotal); }       // D7
        if (qtyT.trim() !== '' && intOf($id('CmbPackUom').value) !== 0 && grT.trim() !== '') {
            $id('txtDetailNetWeight').value = clr(gross - ebTotal + addless);
        }
        avgAmount();
    }

    function qtyChanged() { grossCalc(); weightCalc(); }                         // txtQty_TextChanged:2185 / CmbPackUom_TextChanged:2191

    /** FormValidationDetail:534 — same messages, same order. */
    function validateDetail() {
        function need(id, msg) { if (intOf($id(id).value) === 0) { alert(msg); $id(id).focus(); return false; } return true; }
        function needNum(id, msg) { if ($id(id).value.trim() === '' || num($id(id).value) === 0) { alert(msg); $id(id).focus(); return false; } return true; }
        if (!need('CmbWareHouseFrom', 'WarehouseFrom field is required')) return false;
        if (!need('cmbWareHouseTo', 'WarehouseTo field is required')) return false;
        if (!need('cmbItemName', 'Item Name field is required')) return false;
        if (!need('cmbPackingType', 'PackingType field is required')) return false;
        if (!need('cmbCropYear', 'ItemName field is required')) return false;           // the desktop's own text
        if (!need('CmbPackUom', 'PackUOM field is required')) return false;
        if (!need('cmbJobLot', 'JobLot From field is required')) return false;
        if (!need('CmbJobLotTo', 'JobLotTo field is required')) return false;
        if (!needNum('txtQty', 'Qty field is required')) return false;
        if (!needNum('txtGrossWeight', 'GrossWeight field is required')) return false;
        if (look.financialEffect) {
            if (!needNum('txtAvgRate', 'AvgRate field is required')) return false;
            if (!needNum('txtAmount', 'Amount field is required')) return false;
        }
        if (!needNum('txtDetailNetWeight', 'NetWeight field is required')) return false;
        return true;
    }

    /** One "table" row, in the DataTable's column order (Load:443). */
    function row(id, rdt, rno, rsub, itemId, itemName, packTypeId, packType, crop, jl, jlName, jlTo, jlToName, qty, puomId, puom,
                 gross, ebUnit, ebTotal, adls, net, balQty, balWt, whf, whfName, wht, whtName, remarks, rate, rateUomId, rateUom, amount, expense) {
        function s(v) { return v === null || v === undefined ? '' : String(v); }
        return {
            Id: intOf(id), RefDocumentTypeId: intOf(rdt), RefDocNoId: intOf(rno), RefDocSubIdNo: intOf(rsub),
            ItemId: intOf(itemId), ItemName: s(itemName), PackTypeId: intOf(packTypeId), PackType: s(packType), CropYear: s(crop),
            JobLotId: intOf(jl), JobLot: s(jlName), JobLotIdTo: intOf(jlTo), JobLotTo: s(jlToName),
            QTY: num(qty), PackUOMId: intOf(puomId), PackUOM: s(puom),
            GrossWeight: num(gross), EbUnit: num(ebUnit), EbTotal: num(ebTotal), AdLsWeight: num(adls), NetWeight: num(net),
            BalQty: num(balQty), BalWeight: num(balWt), WareHouseFromId: intOf(whf), WareHouseFrom: s(whfName),
            WareHouseToId: intOf(wht), WareHouseTo: s(whtName), Remarks: s(remarks),
            ItemRate: num(rate), RateUOMId: intOf(rateUomId), RateUOM: num(rateUom), ItemAmount: num(amount), Expense: num(expense)
        };
    }

    function add() {                                                             // btnplus_Click:879
        try {
            if (!validateDetail()) return;
            if (table.some(function (r) { return r.RefDocumentTypeId > 0 || r.RefDocNoId > 0 || r.RefDocSubIdNo > 0; })) {
                throw new Error('Record cannot be add in grid because some rows already exist in grid of Loader');
            }
            table.push(row(0, 0, 0, 0, $id('cmbItemName').value, selText('cmbItemName'), $id('cmbPackingType').value, selText('cmbPackingType'),
                selText('cmbCropYear'), $id('cmbJobLot').value, selText('cmbJobLot'), $id('CmbJobLotTo').value, selText('CmbJobLotTo'),
                num($id('txtQty').value), $id('CmbPackUom').value, selText('CmbPackUom'), num($id('txtGrossWeight').value),
                num($id('txtEbUnit').value), num($id('txtEbTotal').value), num($id('txtAdLsWeight').value), num($id('txtDetailNetWeight').value),
                0, 0, $id('CmbWareHouseFrom').value, selText('CmbWareHouseFrom'), $id('cmbWareHouseTo').value, selText('cmbWareHouseTo'),
                $id('txtRemarksDetail').value.trim(), num($id('txtAvgRate').value), 0, 40, num($id('txtAmount').value), 0));
            gridSettings();
            otherExpensesProportion();
            factoryWeight();
            resetDetail();
            renderGrid();
        } catch (e) { fail(e); }
    }

    /** grd_DoubleClick:818 — the row's values win over anything the combos' own events would load. */
    function editRow(i) {
        var item = table[i];
        if (!item) return;
        updateDetailIndex = i; currentRow = i;
        var loader = item.RefDocumentTypeId > 0;
        if (loader) ['CmbWareHouseFrom', 'cmbItemName', 'CmbPackUom', 'cmbCropYear', 'cmbJobLot'].forEach(function (id) { $id(id).disabled = true; });
        else ['CmbWareHouseFrom', 'cmbItemName', 'CmbPackUom', 'cmbCropYear', 'cmbJobLot'].forEach(function (id) { $id(id).disabled = false; });
        $id('CmbWareHouseFrom').value = String(item.WareHouseFromId);
        $id('cmbWareHouseTo').value = String(item.WareHouseToId);
        $id('cmbItemName').value = String(item.ItemId);
        $id('cmbPackingType').value = String(item.PackTypeId);
        selectByText('cmbCropYear', item.CropYear);
        $id('cmbJobLot').value = String(item.JobLotId);
        $id('CmbJobLotTo').value = String(item.JobLotIdTo);
        $id('txtQty').value = clr(item.QTY);
        bindUoms(item.ItemId).then(function () {
            $id('CmbPackUom').value = String(item.PackUOMId);
            $id('txtGrossWeight').value = clr(item.GrossWeight);
            $id('txtEbUnit').value = clr(item.EbUnit);
            $id('txtEbTotal').value = clr(item.EbTotal);
            $id('txtAdLsWeight').value = clr(item.AdLsWeight);
            $id('txtDetailNetWeight').value = clr(item.NetWeight);
            $id('txtAvgRate').value = clr(item.ItemRate);
            $id('txtAmount').value = clr(item.ItemAmount);
            $id('txtRemarksDetail').value = item.Remarks;
            show('btnplus', false); show('btnUpdateDetail', true); show('btnCancelUpdateDetial', true);
            renderGrid();
        });
    }

    function updateDetail() {                                                    // btnUpdateDetail_Click:765
        try {
            if (!validateDetail()) return;
            var r = table[updateDetailIndex];
            if (!r) return;
            r.ItemId = intOf($id('cmbItemName').value); r.ItemName = selText('cmbItemName');
            r.PackTypeId = intOf($id('cmbPackingType').value); r.PackType = selText('cmbPackingType');
            r.CropYear = selText('cmbCropYear');
            r.JobLotId = intOf($id('cmbJobLot').value); r.JobLot = selText('cmbJobLot');
            r.JobLotIdTo = intOf($id('CmbJobLotTo').value); r.JobLotTo = selText('CmbJobLotTo');
            r.QTY = num($id('txtQty').value);
            r.PackUOMId = intOf($id('CmbPackUom').value); r.PackUOM = selText('CmbPackUom');
            r.GrossWeight = num($id('txtGrossWeight').value);
            r.EbUnit = num($id('txtEbUnit').value); r.EbTotal = num($id('txtEbTotal').value);
            r.AdLsWeight = num($id('txtAdLsWeight').value); r.NetWeight = num($id('txtDetailNetWeight').value);
            r.WareHouseFromId = intOf($id('CmbWareHouseFrom').value); r.WareHouseFrom = selText('CmbWareHouseFrom');
            r.WareHouseToId = intOf($id('cmbWareHouseTo').value); r.WareHouseTo = selText('cmbWareHouseTo');
            r.Remarks = $id('txtRemarksDetail').value;
            if (r.RefDocumentTypeId === 0) {
                r.ItemRate = num($id('txtAvgRate').value);
                r.ItemAmount = num($id('txtAmount').value);
            }
            show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
            itemAmountCalc();
            factoryWeight();
            resetDetail();
            if (transferType() === 'Outward') clearCombo('cmbItemName');         // :805 (D9)
            otherChargesAmountUpdate();
            renderGrid();
        } catch (e) { fail(e); }
    }

    /** btnCancelUpdateDetial_Click:758 — the buttons only. */
    function cancelDetail() { show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); }

    function itemAmountCalc() {                                                  // ItemAmountCalculation:2240
        table.forEach(function (r) {
            var a = r.NetWeight / r.RateUOM * r.ItemRate;
            r.ItemAmount = (r.NetWeight > 0 && r.RateUOM > 0 && r.ItemRate > 0) ? a : 0;
        });
    }

    function factoryWeight() {                                                   // FactoryWeightCalculation:2158 — "#,##0.###"
        var fw = table.reduce(function (a, r) { return a + num(r.GrossWeight); }, 0);
        $id('txtHeadNetWeight').value = (table.length > 0 && fw > 0) ? fmt0(fw, 3) : '0';
    }

    function resetDetail() {                                                     // ResetDetail:1516 — EbUnit / EbTotal / Available Stock are kept
        ['cmbCropYear', 'CmbPackUom', 'cmbItemName', 'cmbJobLot', 'CmbWareHouseFrom', 'cmbWareHouseTo', 'cmbPackingType', 'CmbJobLotTo']
            .forEach(function (id) { $id(id).value = '0'; });
        ['txtDetailNetWeight', 'txtGrossWeight', 'txtAdLsWeight', 'txtQty', 'txtRemarksDetail'].forEach(function (id) { $id(id).value = ''; });
        weightCalc();
        $id('txtAvgRate').value = ''; $id('txtAmount').value = '';
        show('btnplus', true); show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false);
        $id('cmbItemName').focus();
    }

    /* ------------------------------------------------------------------ detail grid (grd) */

    /** grdSettings:1546 — BalQty / BalWeight hidden when the current row is a manual one; re-run on +, Loader, ReadById. */
    function gridSettings() {
        var cr = table[currentRow >= 0 && currentRow < table.length ? currentRow : 0];
        balVisible = !(cr && cr.RefDocumentTypeId === 0);
    }

    function gridCols() {
        var cols = [['ItemName', 'ItemName'], ['PackType', 'PackType'], ['CropYear', 'CropYear'], ['JobLot', 'JobLot'], ['JobLotTo', 'JobLotTo'],
            ['QTY', 'QTY', 0, 1], ['PackUOM', 'PackUOM'], ['GrossWeight', 'GrossWeight', 2, 1], ['EbUnit', 'EbUnit', 2, 1], ['EbTotal', 'EbTotal', 2, 1],
            ['AdLsWeight', 'Ad/LsWeight', 2, 1], ['NetWeight', 'NetWeight', 2, 1]];
        if (balVisible) cols.push(['BalQty', 'BalQty', 0, 1], ['BalWeight', 'BalWeight', 2, 1]);
        cols.push(['WareHouseFrom', 'WareHouseFrom'], ['WareHouseTo', 'WareHouseTo'], ['Remarks', 'Remarks']);
        if (look.financialEffect) cols.push(['ItemRate', 'ItemRate', 3, 0], ['RateUOM', 'RateUOM', -1, 0], ['ItemAmount', 'ItemAmount', 3, 1], ['Expense', 'Expense', -1, 1]);
        return cols;
    }

    function cell(v, c) {
        if (c.length < 3) return '<td>' + esc(v) + '</td>';
        return '<td class="num">' + esc(c[2] < 0 ? clr(v) : hash(v, c[2])) + '</td>';
    }

    function renderGrid() {
        var t = $id('grd'), cols = gridCols();
        t.tHead.innerHTML = '<tr><th>X</th>' + cols.map(function (c) { return '<th>' + esc(c[1]) + '</th>'; }).join('') + '</tr>';
        var tot = {};
        t.tBodies[0].innerHTML = table.map(function (r, i) {
            return '<tr data-i="' + i + '" ondblclick="StockTrM.editRow(' + i + ')" onclick="StockTrM.current(' + i + ')"' + (i === currentRow ? ' class="is-selected"' : '') + '>' +
                '<td><button type="button" class="cx-x" onclick="event.stopPropagation();StockTrM.delRow(' + i + ')">X</button></td>' +
                cols.map(function (c) { if (c[3]) tot[c[0]] = (tot[c[0]] || 0) + num(r[c[0]]); return cell(r[c[0]], c); }).join('') + '</tr>';
        }).join('');
        t.tFoot.innerHTML = table.length ? '<tr style="font-weight:bold"><td></td>' + cols.map(function (c) {
            return tot[c[0]] !== undefined ? '<td class="num">' + esc(hash(tot[c[0]], c[2] < 0 ? 3 : c[2])) + '</td>' : '<td></td>';
        }).join('') + '</tr>' : '';
    }

    function delRow(i) {                                                         // grd_ColumnButtonClick:2727
        var r = table[i];
        if (!r) return;
        if (!confirm('Are you sure to Delete?')) return;
        if (r.Id !== 0) DetailRowsRemoveIds = DetailRowsRemoveIds + ',' + String(r.Id);
        table.splice(i, 1);
        if (currentRow >= table.length) currentRow = table.length - 1;
        otherExpensesProportion();
        factoryWeight();
        renderGrid();
    }

    function sameInAllRows() {                                                   // BtnSameUpdateWarehouse_Click:2755
        try {
            var wh = $id('chkUpdateWareHouse').checked, jl = $id('chkUpdateJobLot').checked;
            if (!wh && !jl) throw new Error('Please Checked atleast one value (warehouse,JobLot) value to Update');
            if (!table.length) throw new Error('Grid Record not Found');
            var f = table[0];
            if (f.WareHouseToId === 0) throw new Error('WareHouseTo Field is Empty in Grid Row No: 01');
            table.forEach(function (r) {
                if (wh) { r.WareHouseToId = f.WareHouseToId; r.WareHouseTo = f.WareHouseTo; }
                if (jl) { r.JobLotIdTo = f.JobLotIdTo; r.JobLotTo = f.JobLotTo; }
            });
            renderGrid();
        } catch (e) { fail(e); }
    }

    /* ------------------------------------------------------------------ other charges (grdOtherCharges) */

    function blankExp() { return { Account: 0, Percentage: 0, Qty: 0, Rate: 0, Amount: 0, Remarks: '' }; }

    function renderExp() {                                                       // grdExpenseSettings:649
        var t = $id('grdOtherCharges');
        t.tHead.innerHTML = '<tr><th>AccountTitle</th><th>%</th><th>Qty</th><th>Rate</th><th>Credit</th><th>Remarks</th><th>X</th><th>+</th></tr>';
        var opts = '<option value="0"></option>' + (look.accounts || []).map(function (a) { return '<option value="' + a.Id + '">' + esc(a.AccountTitle) + '</option>'; }).join('');
        t.tBodies[0].innerHTML = dtExp.map(function (r, i) {
            return '<tr><td><select class="win-combo" data-i="' + i + '" data-k="Account" onchange="StockTrM.expCell(this)">' + opts + '</select></td>' +
                ['Percentage', 'Qty', 'Rate', 'Amount'].map(function (k) {
                    return '<td><input type="text" class="num" data-i="' + i + '" data-k="' + k + '" value="' + esc(k === 'Amount' ? hash(r[k], 3) : clr(r[k])) + '" onchange="StockTrM.expCell(this)"></td>';
                }).join('') +
                '<td><input type="text" data-i="' + i + '" data-k="Remarks" value="' + esc(r.Remarks) + '" onchange="StockTrM.expCell(this)"></td>' +
                '<td><button type="button" class="cx-x" onclick="StockTrM.expDel(' + i + ')">X</button></td>' +
                '<td><button type="button" class="cx-link" onclick="StockTrM.expAdd()">+</button></td></tr>';
        }).join('');
        t.tBodies[0].querySelectorAll('select[data-k="Account"]').forEach(function (s) { s.value = String(dtExp[intOf(s.getAttribute('data-i'))].Account); });
        var total = dtExp.reduce(function (a, r) { return a + num(r.Amount); }, 0);
        t.tFoot.innerHTML = '<tr style="font-weight:bold"><td></td><td></td><td></td><td></td><td class="num">' + esc(hash(total, 3)) + '</td><td colspan="3"></td></tr>';
    }

    function itemAmountTotal() { return table.reduce(function (a, r) { return a + num(r.ItemAmount); }, 0); }

    function expCell(el) {                                                       // grdOtherCharges_CellUpdated:2411
        var i = intOf(el.getAttribute('data-i')), k = el.getAttribute('data-k'), r = dtExp[i];
        if (!r) return;
        r[k] = (k === 'Remarks') ? el.value : (k === 'Account' ? intOf(el.value) : num(el.value));
        if (k === 'Qty' || k === 'Rate') { r.Amount = r.Qty * r.Rate; r.Percentage = 0; }
        if (k === 'Percentage') {
            var tp = itemAmountTotal() / 100.0 * r.Percentage;
            if (tp > 0) r.Amount = tp;
            r.Qty = 0; r.Rate = 0;
        }
        otherExpensesProportion();
        renderExp(); renderGrid();
    }

    function expDel(i) {                                                         // grdOtherCharges_ColumnButtonClick:2447
        dtExp.splice(i, 1);
        if (!dtExp.length) dtExp.push(blankExp());
        otherExpensesProportion(); renderExp(); renderGrid();
    }
    function expAdd() { dtExp.push(blankExp()); otherExpensesProportion(); renderExp(); renderGrid(); }

    function otherExpensesProportion() {                                         // OtherExpensesProportion:690
        if (!table.length) return;
        var net = table.reduce(function (a, r) { return a + num(r.NetWeight); }, 0);
        var credit = dtExp.reduce(function (a, r) { return a + num(r.Amount); }, 0);
        table.forEach(function (r) { r.Expense = credit > 0 ? credit / net * r.NetWeight : 0; });
    }

    function otherChargesAmountUpdate() {                                        // OtherChargesAmountUpdate:727
        dtExp.forEach(function (r) {
            if (num(r.Amount) > 0) {                                             // the Percentage cell always has text
                var tp = itemAmountTotal() / 100.0 * num(r.Percentage);
                if (tp > 0) r.Amount = tp;
                r.Qty = 0; r.Rate = 0;
            }
        });
        otherExpensesProportion();
        renderExp();
    }

    /* ------------------------------------------------------------------ Generate Rate (AvgRateRecalculateOnDocDateChange:2892) */

    function generateRate() {
        if (!table.length || look.fifo) return;
        var p = Promise.resolve(), stop = false;
        table.forEach(function (r) {
            p = p.then(function () {
                if (stop) return;
                if (r.RefDocumentTypeId !== 0 || r.RefDocNoId !== 0 || r.RefDocSubIdNo !== 0) { stop = true; return; }   // break
                return C.getJson(api + '/avg-rate' + C.qs({ recId: RecId, itemId: r.ItemId, docDate: $id('DocDate').value, jobLotId: r.JobLotId,
                    cropYearId: 0, cropYear: r.CropYear, warehouseId: r.WareHouseFromId })).then(function (x) {
                    var rate = num(x.rate);
                    if (rate > 0) { r.ItemRate = rate; r.ItemAmount = r.NetWeight / 40.0 * rate; }
                });
            });
        });
        p.then(renderGrid).catch(fail);
    }

    /* ------------------------------------------------------------------ loader dialog */

    function openLoader() {                                                      // btnLoadInvoices_Click:2342 — a new dialog each time
        loaderRows = []; loaderChecked = {};
        var g = $id('grdLoader'); g.tHead.innerHTML = ''; g.tBodies[0].innerHTML = ''; g.tFoot.innerHTML = '';
        $id('txtSelectedQty').value = ''; $id('txtSelectedStock').value = '';
        C.getJson(api + '/loader/lookups').then(function (l) {
            $id('ldFrom').value = l.fromDate || C.today();                       // FromDate = ActiveYr.Start_Period
            $id('ldTo').value = C.today();
            setOptions('ldParent', l.ParentCategories, 'Id', 'name'); setOptions('ldCategory', l.ItemCategories, 'Id', 'name');
            setOptions('ldType', l.ItemTypes, 'Id', 'name'); setOptions('ldJobLot', l.JobLot, 'Id', 'name');
            setOptions('ldCrop', l.CropYear, 'Id', 'name'); setOptions('ldWarehouse', l.Warehouse, 'Id', 'name');
            setOptions('ldDocType', l.DocumentType, 'Id', 'name'); setOptions('ldParty', l.Supplier_Customer, 'Id', 'name');
            setOptions('ldItem', l.Items, 'Id', 'name');
            ['ldParent', 'ldCategory', 'ldType', 'ldJobLot', 'ldCrop', 'ldWarehouse', 'ldDocType', 'ldParty', 'ldItem'].forEach(function (id) { $id(id).value = '0'; });
            C.openModal('dlgLoader');
            loaderSearch();                                                      // LoadInvoices_Load:188
        }).catch(fail);
    }

    /** grdSettings:415 — visible columns in dtcol order; AVgRate / ItemAmount hidden (FoodProductionWithValues.ValuesShowRights false). */
    var LD_COLS = ['RefDocumentType', 'DocDate', 'DocCodeNo', 'ManualNo', 'GrnNo', 'SupplierCustomerName', 'VehicleNo', 'BiltyNo', 'GpNo',
        'WareHouseCode', 'RefWarehouse', 'ItemName', 'ItemCode', 'CropBatch', 'JobLotCode', 'PackingType', 'PackUom', 'QtyIn', 'QtyOut',
        'QtyBalance', 'WeightIn', 'WeightOut', 'WeightBalance', 'RateUom', 'Remarks'];
    var LD_NUM = { QtyIn: 1, QtyOut: 1, QtyBalance: 1, WeightIn: 1, WeightOut: 1, WeightBalance: 1 };

    function loaderSearch() {                                                    // PendingInventoryTransactionsForIssuanceLoad:319
        C.getJson(api + '/loader/search' + C.qs({
            fromDate: $id('ldFrom').value, toDate: $id('ldTo').value, parentCategoryId: intOf($id('ldParent').value),
            itemCategoryId: intOf($id('ldCategory').value), itemTypeId: intOf($id('ldType').value), jobLotId: intOf($id('ldJobLot').value),
            cropYear: selText('ldCrop'), warehouseId: intOf($id('ldWarehouse').value), refDocumentTypeId: intOf($id('ldDocType').value),
            supplierId: intOf($id('ldParty').value), itemId: intOf($id('ldItem').value)
        })).then(function (rows) {
            loaderRows = rows; loaderChecked = {};
            var t = $id('grdLoader');
            if (!rows.length) { t.tHead.innerHTML = ''; t.tBodies[0].innerHTML = ''; t.tFoot.innerHTML = ''; selectedCalc(); return; }
            t.tHead.innerHTML = '<tr><th><input type="checkbox" onclick="StockTrM.loaderAll(this.checked)"></th>' + LD_COLS.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
            var tot = {};
            t.tBodies[0].innerHTML = rows.map(function (r, i) {
                return '<tr><td><input type="checkbox" data-i="' + i + '" onclick="StockTrM.loaderPick(' + i + ',this.checked)"></td>' + LD_COLS.map(function (c) {
                    var v = ci(r, c);
                    if (LD_NUM[c]) { tot[c] = (tot[c] || 0) + num(v); return '<td class="num">' + esc(fmt0(v, 2)) + '</td>'; }
                    if (c === 'DocDate') return '<td>' + esc(C.gridDate(v)) + '</td>';
                    return '<td>' + esc(v === null ? '' : v) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = '<tr style="font-weight:bold"><td></td>' + LD_COLS.map(function (c) {
                return LD_NUM[c] ? '<td class="num">' + esc(fmt0(tot[c], 2)) + '</td>' : '<td></td>';
            }).join('') + '</tr>';
            selectedCalc();
        }).catch(fail);
    }

    function loaderPick(i, on) { if (on) loaderChecked[i] = true; else delete loaderChecked[i]; selectedCalc(); }
    function loaderAll(on) {
        loaderChecked = {};
        $id('grdLoader').querySelectorAll('tbody input[type=checkbox]').forEach(function (c) { c.checked = on; if (on) loaderChecked[c.getAttribute('data-i')] = true; });
        selectedCalc();
    }
    function fmt00(v) {                                                          // Math.Round(x).ToString("0,0")
        var n = Math.round(v), a = Math.abs(n), s = a.toLocaleString('en-US');
        if (a < 10) s = '0' + a;
        return (n < 0 ? '-' : '') + s;
    }
    function selectedCalc() {                                                    // SelectedWeightCalculation:636
        var w = 0, q = 0;
        Object.keys(loaderChecked).forEach(function (k) { var r = loaderRows[k]; w += num(ci(r, 'WeightBalance')); q += num(ci(r, 'QtyBalance')); });
        $id('txtSelectedStock').value = fmt00(w); $id('txtSelectedQty').value = fmt00(q);
    }
    function loaderReset() {                                                     // btnReset_Click:552
        $id('ldParty').value = '0'; $id('ldItem').value = '0';
        loaderRows = []; loaderChecked = {};
        var g = $id('grdLoader'); g.tHead.innerHTML = ''; g.tBodies[0].innerHTML = ''; g.tFoot.innerHTML = '';
    }

    /** The dialog closed without Load: dtIssuance is empty, only FactoryWeightCalculation runs (btnLoadInvoices_Click:2350). */
    function closeLoader() {
        C.closeModal('dlgLoader');
        factoryWeight();
    }

    function loaderLoad() {                                                      // btnLoadOnInvoice_Click_1:566
        var picked = Object.keys(loaderChecked).map(function (k) { return loaderRows[k]; });
        if (!picked.length) { alert('Please Select Row first'); return; }
        var dtIssuance = picked.map(function (r) {
            return {
                RefDocumentTypeId: intOf(ci(r, 'RefDocumentTypeId')), RefDocIdNo: intOf(ci(r, 'RefDocIdNo')), RefDocSubIdNo: intOf(ci(r, 'RefDocSubIdNo')),
                WareHouseId: intOf(ci(r, 'WarehouseId')), WareHouse: ci(r, 'WareHouseCode'), ItemId: intOf(ci(r, 'ItemId')), ItemName: ci(r, 'ItemName'),
                ItemUOMId: intOf(ci(r, 'ItemUom')), ItemUOM: clr(ci(r, 'PackSize')), CropYear: ci(r, 'CropBatch'), JobLotId: intOf(ci(r, 'JobLotId')),
                JobLotCode: ci(r, 'JobLotCode'), PackingTypeId: intOf(ci(r, 'InvPackingTypeId')), PackTypeCode: ci(r, 'PackingType'),
                ItemQty: num(ci(r, 'QtyBalance')), Weight: num(ci(r, 'WeightBalance')), ItemRate: num(ci(r, 'AVgRate')),
                RateUOMId: intOf(ci(r, 'RateUomId')), Equivalent: num(ci(r, 'Equivalent')), ItemAmount: num(ci(r, 'ItemAmount'))
            };
        });
        C.closeModal('dlgLoader');
        /* LoadDataDetailfromPurchaseInvoivce:2358 (D8) */
        dtIssuance.forEach(function (d) {
            var flag = false;
            if (table.length > 0) {
                table.slice().forEach(function (item) {
                    if (item.RefDocumentTypeId === 0 || item.RefDocNoId === 0) table.splice(table.indexOf(item), 1);
                    if (table.length !== 0 && d.RefDocumentTypeId === item.RefDocumentTypeId && d.RefDocIdNo === item.RefDocNoId && d.RefDocSubIdNo === item.RefDocSubIdNo) {
                        item.QTY = num(item.QTY) + d.ItemQty;
                        item.BalWeight = num(item.BalWeight) + d.Weight;
                        flag = true;
                    }
                });
            }
            if (!flag) {
                table.push(row(0, d.RefDocumentTypeId, d.RefDocIdNo, d.RefDocSubIdNo, d.ItemId, d.ItemName, d.PackingTypeId, d.PackTypeCode,
                    d.CropYear, d.JobLotId, d.JobLotCode, d.JobLotId, d.JobLotCode, d.ItemQty, d.ItemUOMId, d.ItemUOM, d.Weight, 0, 0, 0, d.Weight,
                    d.ItemQty, d.Weight, d.WareHouseId, d.WareHouse, 0, '', '', d.ItemRate, d.RateUOMId, d.Equivalent, d.ItemAmount, 0));
            }
        });
        if (currentRow >= table.length) currentRow = table.length - 1;
        gridSettings();
        otherChargesAmountUpdate();
        factoryWeight();                                                         // btnLoadInvoices_Click:2350
        renderGrid();
    }

    /* ------------------------------------------------------------------ save / update / delete / print */

    /** FormValidation:517 (client copy; the server repeats it). */
    function formValidation() {
        if ($id('txtdocno').value.trim() === '' || $id('txtdocno').value.trim() === '0') { alert('DocNo Field is Required'); return false; }
        if (transferType() === '') { alert('Transfer Type Field is Required'); $id('CmbTransferType').focus(); return false; }
        return true;
    }

    function insert() {                                                          // Insert:912
        if (!formValidation()) return;
        if (!table.length) { alert('Grid Record Not Found'); return; }
        if (RecId > 0) { if (!confirm('Are you sure to Update?')) return; }
        else if (!confirm('Are you sure to Save?')) return;
        var body = {
            Id: RecId, DocDate: $id('DocDate').value, TransferType: transferType(),
            GatePass: $id('cmbGatePass').value, TicketNo: $id('cmbTicketNo').value, WbNetWeight: $id('txtHeadNetWeight').value.trim(),
            RemarksHeader: $id('txtRemarksHead').value, DetailRowsRemoveIds: DetailRowsRemoveIds, rows: table, expenses: dtExp
        };
        C.postJson(api + '/save', body).then(function (r) {
            alert(r.message);
            var printId = r.id;
            return reset().then(function () {
                if ($id('ChkBoxPrintPreview').checked) C.printSlip(api + '/' + printId + '/slip', 'Stock Transfer Slip (407)');
            });
        }).catch(function (e) {
            if (e && e.message === 'Grid Charge to Product Remarks required Please Check') subTab('tabPage4');   // :961
            fail(e);
        });
    }

    function save() { RecId = 0; insert(); }                                    // btnsave_Click:1111
    function update() { if (IsApproved) { alert('Approved Record Not Update'); return; } insert(); }   // btnupdate_Click:1124

    function del() {                                                             // btnDelete_Click:2852
        if (RecId <= 0) { alert('RecordId Not Found.....'); return; }
        if (!confirm('Are you sure to Delete?')) return;
        C.postJson(api + '/' + RecId + '/delete', {}).then(function (r) { alert(r.message); return reset(); }).catch(fail);
    }

    function print() {                                                           // btnprint_Click:2040 → StockTransferSlip407(RecId)
        if (RecId === 0) { alert('No Record Found For Display'); return; }
        C.printSlip(api + '/' + RecId + '/slip', 'Stock Transfer Slip (407)');
    }

    /* ------------------------------------------------------------------ new / reset / refresh / read */

    function reset() {                                                           // Reset:1472 — Doc Date, Trans.Type and the rest of the entry bar are kept
        DetailRowsRemoveIds = ''; RecId = 0; IsApproved = false;
        ['cmbCropYear', 'CmbPackUom', 'cmbPackingType'].forEach(function (id) { $id(id).value = '0'; });
        ['cmbGatePass', 'cmbTicketNo', 'txtHeadNetWeight', 'txtRemarksHead', 'txtWorkingReportNo', 'txtAvgRate', 'txtAmount']
            .forEach(function (id) { $id(id).value = ''; });
        show('btnsave', true); show('btnupdate', false); show('btnplus', true);
        show('btnUpdateDetail', false); show('btnCancelUpdateDetial', false); show('btnDelete', false);
        dtExp = [blankExp()]; renderExp();
        ['CmbWareHouseFrom', 'cmbItemName', 'CmbPackUom', 'cmbCropYear', 'cmbJobLot'].forEach(function (id) { $id(id).disabled = false; });
        table = []; currentRow = -1;
        renderGrid();
        $id('lblRecId').innerHTML = '&nbsp;';
        return C.getJson(api + '/doc-no').then(function (r) { if (r.docNo > 0) $id('txtdocno').value = String(r.docNo); }).catch(fail);
    }

    function btnNew() { reset().then(resetDetail); }                             // btnnew_Click:1461

    function refresh() {                                                         // btnRefresh_Click:1441
        C.getJson(api + '/refresh').then(function (l) {
            fillCombos(l);
            return bindUoms(intOf($id('cmbItemName').value));
        }).catch(fail);
    }

    function readById(id, viaEdit) {                                             // ReadById:1143 (Edit button: Reset first, :1959)
        var p = viaEdit ? reset() : Promise.resolve();
        return p.then(function () { return C.getJson(api + '/' + id); }).then(function (s) {
            RecId = id;
            DetailRowsRemoveIds = '';
            tab('tabForm');
            $id('DocDate').value = C.isoDay(s.DocDate);
            $id('txtdocno').value = String(s.DocNo);
            $id('txtRemarksHead').value = s.RemarksHeader;
            $id('txtHeadNetWeight').value = s.WbNetWeight;
            $id('cmbGatePass').value = s.GatePassId;
            $id('cmbTicketNo').value = s.WbTicketId;
            selectByText('CmbTransferType', s.TransferType);
            $id('txtWorkingReportNo').value = s.WorkingReportNo;
            IsApproved = !!s.IsApproved;
            table = s.rows; currentRow = table.length ? 0 : -1;
            gridSettings();
            dtExp = s.expenses.length ? s.expenses : [blankExp()];
            renderGrid(); renderExp();
            show('btnsave', false); show('btnupdate', true); show('btnDelete', !!look.rights['delete']);
            $id('lblRecId').textContent = 'Record Id: ' + id;
        }).catch(fail);
    }

    /* ------------------------------------------------------------------ history */

    function wbWeight(v) {                                                       // "#,##0,.##" — the ",." scales by 1000 (D11)
        return fmt0(num(v) / 1000, 2);
    }

    function showHistory() {                                                     // gridhistoryfill:1780
        var dt = document.querySelector('input[name="hd"]:checked').value;
        var tt = $id('cmbTransactiontypeHistory');
        C.getJson(api + '/history' + C.qs({
            dateType: dt,
            fromDate: $id('chkFromDateHistory').checked ? $id('FromDateHistory').value : '',
            toDate: $id('chkToDateHistory').checked ? $id('ToDateHistory').value : '',
            fromDocNo: intOf($id('txtFromDocNoHistory').value), toDocNo: intOf($id('txtToDocNoHistory').value),
            transferType: tt.value === '0' ? '' : tt.selectedOptions[0].textContent
        })).then(function (rows) {
            historyRows = rows;
            var t = $id('grdhistory'), d = $id('GridDetailHistory');
            if (!rows.length) {
                t.tHead.innerHTML = t.tBodies[0].innerHTML = t.tFoot.innerHTML = '';
                d.tHead.innerHTML = d.tBodies[0].innerHTML = d.tFoot.innerHTML = '';
                return;
            }
            var cols = ['DocDate', 'DocNo', 'TransType', 'GPNo', 'TicketNo', 'WBWeight', 'Remarks'];
            if (look.multiBranch) cols.push('FromBranchName', 'ToBranchName');                 // HistoryGridSettings:1889
            cols.push('EntryDate', 'EntryUser', 'ModifyDate', 'ModifyUser', 'NoOfAttachments');
            t.tHead.innerHTML = '<tr><th>Edit</th><th>Print</th>' + cols.map(function (c) { return '<th>' + esc(c) + '</th>'; }).join('') + '</tr>';
            var tot = 0;
            t.tBodies[0].innerHTML = rows.map(function (r, i) {
                return '<tr onclick="StockTrM.historySelect(' + i + ',this)" ondblclick="StockTrM.readById(' + r.Id + ')">' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();StockTrM.readById(' + r.Id + ',true)">Edit</button></td>' +
                    '<td><button type="button" class="cx-link" onclick="event.stopPropagation();StockTrM.printId(' + r.Id + ')">Print</button></td>' +
                    cols.map(function (c) {
                        var v = r[c];
                        if (c === 'DocNo') return '<td><a href="#" class="cx-link" onclick="event.preventDefault();event.stopPropagation();StockTrM.readById(' + r.Id + ')">' + esc(v) + '</a></td>';
                        if (c === 'WBWeight') { tot += num(v); return '<td class="num">' + esc(wbWeight(v)) + '</td>'; }
                        if (c === 'DocDate') return '<td>' + esc(v ? C.gridDate(v) : '') + '</td>';
                        if (c === 'EntryDate' || c === 'ModifyDate') return '<td>' + esc(v ? C.gridDateTime(v, true) : '') + '</td>';
                        return '<td>' + esc(v === null || v === undefined ? '' : v) + '</td>';
                    }).join('') + '</tr>';
            }).join('');
            t.tFoot.innerHTML = '<tr style="font-weight:bold"><td></td><td></td>' + cols.map(function (c) {
                return c === 'WBWeight' ? '<td class="num">' + esc(fmt0(tot, 2)) + '</td>' : '<td></td>';
            }).join('') + '</tr>';
        }).catch(fail);
    }

    var HD_COLS = [['FromWarehouse'], ['ToWarehouse'], ['JobLotFrom'], ['JobLotTo'], ['CropYear'], ['ItemName'], ['PackingType'], ['Qty', 2],
        ['PackUom'], ['GrossWeight', 2], ['EbUnit', 2], ['EbTotal', 2], ['AddLessWt', 2], ['StockWeight', 2], ['RemarksDetail']];

    function historySelect(i, tr) {                                              // grdhistory_SelectionChanged:2609
        $id('grdhistory').querySelectorAll('tbody tr').forEach(function (x) { x.classList.remove('is-selected'); });
        tr.classList.add('is-selected');
        var r = historyRows[i];
        C.getJson(api + '/' + r.Id + '/history-detail').then(function (rows) {
            if (!rows.length) return;                                            // the desktop leaves the previous detail
            var d = $id('GridDetailHistory'), tot = {};
            d.tHead.innerHTML = '<tr>' + HD_COLS.map(function (c) { return '<th>' + esc(c[0]) + '</th>'; }).join('') + '</tr>';
            d.tBodies[0].innerHTML = rows.map(function (x) {
                return '<tr>' + HD_COLS.map(function (c) {
                    var v = x[c[0]];
                    if (c.length > 1) { tot[c[0]] = (tot[c[0]] || 0) + num(v); return '<td class="num">' + esc(fmt0(v, c[1])) + '</td>'; }
                    return '<td>' + esc(v) + '</td>';
                }).join('') + '</tr>';
            }).join('');
            d.tFoot.innerHTML = '<tr style="font-weight:bold">' + HD_COLS.map(function (c) {
                return tot[c[0]] !== undefined ? '<td class="num">' + esc(fmt0(tot[c[0]], c[1])) + '</td>' : '<td></td>';
            }).join('') + '</tr>';
        }).catch(fail);
    }

    function historyReset() {                                                    // btnNewHistory_Click:2821
        $id('FromDateHistory').value = C.today();
        $id('ToDateHistory').value = C.today();
        $id('txtFromDocNoHistory').value = ''; $id('txtToDocNoHistory').value = '';
        $id('cmbTransactiontypeHistory').value = '0';
        ['grdhistory', 'GridDetailHistory'].forEach(function (id) { var t = $id(id); t.tHead.innerHTML = t.tBodies[0].innerHTML = t.tFoot.innerHTML = ''; });
        historyRows = [];
        $id('drdocdate').checked = true;
    }

    function historyRefresh() {                                                  // btnRefreshHistory_Click:2840 → HistoryComboFill
        C.getJson(api + '/history-transfer-types').then(function (rows) {
            setOptions('cmbTransactiontypeHistory', rows, 'TransferType', 'TransferType');
        }).catch(fail);
    }

    function printId(id) { C.printSlip(api + '/' + id + '/slip', 'Stock Transfer Slip (407)'); }   // grdhistory_ColumnButtonClick:1963

    /* ------------------------------------------------------------------ tabs */

    function tab(id) {
        document.querySelectorAll('.win-tab[data-tab]').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-tab') === id); });
        document.querySelectorAll('.win-tab-panel').forEach(function (p) { p.classList.toggle('is-active', p.id === id); });
    }
    function subTab(id) {
        document.querySelectorAll('.win-tab[data-sub]').forEach(function (t) { t.classList.toggle('is-active', t.getAttribute('data-sub') === id); });
        ['tabPage3', 'tabPage4'].forEach(function (p) { show(p, p === id); });
    }
    function current(i) {
        currentRow = i;
        $id('grd').querySelectorAll('tbody tr').forEach(function (x) { x.classList.toggle('is-selected', intOf(x.getAttribute('data-i')) === i); });
    }

    window.StockTrM = {
        btnNew: btnNew, refresh: refresh, save: save, update: update, print: print, del: del, openLoader: openLoader,
        generateRate: generateRate, tab: tab, subTab: subTab, current: current,
        itemChanged: itemChanged, stockAndRate: stockAndRate, qtyChanged: qtyChanged, weightCalc: weightCalc, avgAmount: avgAmount,
        add: add, editRow: editRow, updateDetail: updateDetail, cancelDetail: cancelDetail, delRow: delRow,
        sameInAllRows: sameInAllRows, expCell: expCell, expDel: expDel, expAdd: expAdd,
        loaderSearch: loaderSearch, loaderLoad: loaderLoad, loaderReset: loaderReset, loaderPick: loaderPick, loaderAll: loaderAll,
        closeLoader: closeLoader,
        showHistory: showHistory, historySelect: historySelect, historyReset: historyReset, historyRefresh: historyRefresh,
        readById: readById, printId: printId
    };
    document.addEventListener('DOMContentLoaded', init);
})();
