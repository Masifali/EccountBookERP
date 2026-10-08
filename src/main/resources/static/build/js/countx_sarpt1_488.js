/* ============================================================================================
 * Screen 488 frmPurchaseAndSaleDetailByJobLot - Architecture.WinApp.Inventory_Reports.frmPurchaseAndSaleDetailByJobLot (.cs, 760 lines)
 * Page: templates/sale/reports/sarpt1_purchase_and_sale_by_joblot.html (route /sale/reports/purchase-and-sale-detail-by-joblot).
 *   frmPurchaseAndSaleDetailByJobLot_Load :115  BranchesFill, ParameterFill, ComboFill, cmbperemeter.Rows[1].Activate() (This Week), focus
 *   ParameterFill :137   CommonServices.DateType, BindDDL(ZeroIndex false, "Date Type")
 *   cmbperemeter_ValueChanged :157  1 From = today | 2 today-7 | 3 first of (UTC) month, To = today | 4 1 Jan, To = today | 5 From = Start_Period
 *   BranchesFill :194    GetBranchsAllocatedToUser (USP_GetBranchsAllocatedToUser), BindDDL(BranchId, BranchName, ZeroIndex false), checked list,
 *                        text = the user's branch
 *   ComboFill :223       USP_Inventory_StockEvalautionDetail_DropDownAndLists @DocumentTypeIds='56,57,95,99' @BranchesIds=",id,id": rows with
 *                        ActivityType "JobLot" -> cmbjoblot (ZeroIndex true, row 0 activated)
 *   cmbBranchName_Leave :272  branch text + active row -> ComboFill, else job lot cleared
 *   btnshow_Click :290   no branch -> "Select Branch First"; usp_PurchaseAndSaleDetailByJobLot; FillGridHistory :329 + FillGridSummary :383
 *   grdSetting :424 / SummaryGridSetting :498   widths, "#,##0.##" qty / weight, stringFormatsingle amounts, DecimalRateFormate rate, sums
 *   Reset :551 (job lot text cleared, focus date type), btnNew :589, toolStripButton1 :564 (BranchesFill + ComboFill), btnRegister_Click :646
 *   (no rows -> "Record Not Found For Display"; 56_08_PurchaseAndSaleDetailByJobLot.rpt, @CompanyAddress / @CompanyName), KeyDown :668,
 *   MakeShortCutKeys :720, btnshow_Leave :613
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/sarpt1/purchase-and-sale-detail-by-joblot';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null, userBranchId: null };
    var lastArgs = null, hasRows = false, seq = 0;
    var SHORTCUTS = [['Ctrl+S', 'For show data'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus on Date Type'], ['Ctrl+alt', 'To Show ShortCut Date Type'], ['Ctrl+ArrowDown', 'For Focus On Grid']];

    // ------------------------------------------------------------------ lists
    function parameterFill() { S.fill(el('cmbperemeter'), S.dateTypeRows(false), false); }
    function branchesFill(rows) { S.fillBranches(el('cmbBranchName'), rows, lookup.userBranchId); }
    function comboFill() {
        var ids = S.branchIds(el('cmbBranchName'));
        if (!ids) { S.fill(el('cmbjoblot'), [], true); el('cmbjoblot').value = ''; return Promise.resolve(); }
        return A.getJson(API + '/combos?' + S.qs({ branchesIds: ids })).then(function (data) {
            var jobs = (data && (data.JobLot || data.joblot)) || [];
            S.fill(el('cmbjoblot'), jobs.map(function (r) { return { Id: A.ci(r, 'Id'), name: A.ci(r, 'name') }; }), true);     // ZeroIndex true, Rows[0]
        });
    }
    el('cmbperemeter').addEventListener('change', function () {            // cmbperemeter_ValueChanged
        S.dateRule(A.toInt(this.value), el('datGdnFromDate'), el('dateGdnToDate'), lookup.yearStart);
    });

    // ------------------------------------------------------------------ grids
    var fSingle = function () { return S.fmtSingle(lookup.amountDecimals); }, fRate = function () { return S.fmtRate(lookup.rateDecimals); };
    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 1 });
    var summary = new S.Grid({ tableId: 'sumResults', gridId: 'sumGrid', headerLines: 1, noFilter: true, noTotal: true });
    function histCols() {
        var q = '#,##0.##';
        return [
            { key: 'Id', caption: 'Id', hidden: true, width: 20 }, { key: 'DocDate', caption: 'DocDate', width: 70, date: 'dd-MMM-yy' },
            { key: 'DocNo', caption: 'DocNo', width: 60, align: 'c' }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'DocType', caption: 'DocType', width: 70 }, { key: 'TransType', caption: 'TransType', width: 80 },
            { key: 'SupplierCustomerId', caption: 'SupplierCustomerId', hidden: true }, { key: 'PartyName', caption: 'PartyName', width: 120 },
            { key: 'WarehouseId', caption: 'WarehouseId', hidden: true }, { key: 'WareHouseName', caption: 'WareHouseName', width: 120 },
            { key: 'ItemId', caption: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName', width: 150 },
            { key: 'ItemUOMId', caption: 'ItemUOMId', hidden: true }, { key: 'PackUom', caption: 'PackUom', width: 60 },
            { key: 'CropYear', caption: 'CropYear', width: 70 }, { key: 'JobLotId', caption: 'JobLotId', hidden: true },
            { key: 'JobLot', caption: 'JobLot', width: 140 }, { key: 'PackingTypeId', caption: 'PackingTypeId', hidden: true },
            { key: 'PackingType', caption: 'PackingType', width: 90 },
            { key: 'InQty', caption: 'InQty', width: 60, fmt: q, sum: true, num: true }, { key: 'InWeight', caption: 'InWeight', width: 70, fmt: q, sum: true, num: true },
            { key: 'ItemRate', caption: 'ItemRate', width: 70, fmt: fRate(), num: true }, { key: 'RateUom', caption: 'RateUom', width: 60 },
            { key: 'InAmount', caption: 'InAmount', width: 90, fmt: fSingle(), sum: true, num: true },
            { key: 'OutQty', caption: 'OutQty', width: 90, fmt: q, sum: true, num: true }, { key: 'OutWeight', caption: 'OutWeight', width: 100, fmt: q, sum: true, num: true },
            { key: 'OutAmount', caption: 'OutAmount', width: 100, fmt: fSingle(), sum: true, num: true }
        ];
    }
    function sumCols() {
        var q = '#,##0.##';
        return [
            { key: 'JobLotId', caption: 'JobLotId', hidden: true }, { key: 'JobLot', caption: 'JobLot', width: 140 },
            { key: 'InQty', caption: 'InQty', width: 70, fmt: q, sum: true, num: true }, { key: 'InWeight', caption: 'InWeight', width: 80, fmt: q, sum: true, num: true },
            { key: 'InAmount', caption: 'InAmount', width: 110, fmt: fSingle(), sum: true, num: true },
            { key: 'OutQty', caption: 'OutQty', width: 80, fmt: q, sum: true, num: true }, { key: 'OutWeight', caption: 'OutWeight', width: 100, fmt: q, sum: true, num: true },
            { key: 'OutAmount', caption: 'OutAmount', width: 110, fmt: fSingle(), sum: true, num: true },
            { key: 'GrossProfitLoss', caption: 'GrossProfitLoss', width: 130, fmt: fSingle(), sum: true, num: true }
        ];
    }
    function mapHist(r) {
        var c = A.ci;
        return { Id: c(r, 'Id'), DocDate: c(r, 'DocDate'), DocNo: c(r, 'DocNo'), DocumentTypeId: c(r, 'DocumentTypeId'), DocType: c(r, 'DocType'), TransType: c(r, 'TransType'),
            SupplierCustomerId: c(r, 'SupplierCustomerId'), PartyName: c(r, 'PartyName'), WarehouseId: c(r, 'WarehouseId'), WareHouseName: c(r, 'WareHouseName'),
            ItemId: c(r, 'ItemId'), ItemName: c(r, 'ItemName'), ItemUOMId: c(r, 'ItemUOMId'), PackUom: c(r, 'PackUom'), CropYear: c(r, 'CropYear'),
            JobLotId: c(r, 'JobLotId'), JobLot: c(r, 'JobLotDescription'), PackingTypeId: c(r, 'PackingTypeId'), PackingType: c(r, 'PackTypeDesc'),
            InQty: c(r, 'InQty'), InWeight: c(r, 'InWeight'), ItemRate: c(r, 'ItemRate'), RateUom: c(r, 'RateUom'), InAmount: c(r, 'InAmount'),
            OutQty: c(r, 'OutQty'), OutWeight: c(r, 'OutWeight'), OutAmount: c(r, 'OutAmount') };
    }
    function mapSummary(rows) {                                             // one row per JobLotId (HashSet)
        var seen = {}, out = [], c = A.ci;
        rows.forEach(function (r) {
            var k = A.toInt(c(r, 'JobLotId')); if (seen[k]) return; seen[k] = true;
            out.push({ JobLotId: c(r, 'JobLotId'), JobLot: c(r, 'JobLotDescription'), InQty: c(r, 'TotalInQtyByJob'), InWeight: c(r, 'TotalInWeightByJob'),
                InAmount: c(r, 'TotalInAmountByJob'), OutQty: c(r, 'TotalOutQtyByJob'), OutWeight: c(r, 'TotalOutWeightByJob'), OutAmount: c(r, 'TotalOutAmountByJob'),
                GrossProfitLoss: c(r, 'GrossProfitLossByJob') });
        });
        return out;
    }

    // ------------------------------------------------------------------ Show / print
    function show() {
        var b = el('show'); if (b.disabled) return;
        var ids = S.branchIds(el('cmbBranchName'));
        if (!ids) { alert('Select Branch First'); S.focus('cmbBranchName'); return; }
        var a = { fromDate: el('datGdnFromDate').value, toDate: el('dateGdnToDate').value, jobLotId: S.selInt('cmbjoblot'), branchesIds: ids };
        var token = ++seq;
        A.busy(b, true);
        A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.clear(); summary.clear(); return; }
            grid.setData(histCols(), rows.map(mapHist));
            summary.setData(sumCols(), mapSummary(rows));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function print() {
        var b = el('print'); if (b.disabled) return;
        if (!hasRows || !lastArgs) { alert('Record Not Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/sale/reports/sarpt1/print/purchase-and-sale-detail-by-joblot?' + S.qs(lastArgs)).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() { el('cmbjoblot').value = ''; el('cmbjoblot').dispatchEvent(new Event('change', { bubbles: true })); S.focus('cmbperemeter'); }
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; branchesFill(lookup.branches); return comboFill(); })
            .catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    S.onLeave('cmbBranchName', function () { if (S.branchIds(el('cmbBranchName'))) comboFill().catch(function (e) { alert(e.message); }); else { el('cmbjoblot').value = ''; el('cmbjoblot').dispatchEvent(new Event('change', { bubbles: true })); } });
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && el('grid').contains(e.relatedTarget)) S.focus('cmbperemeter'); });   // btnshow_Leave
    S.keys({ s: show, e: closeForm, r: refresh, n: reset, p: print, F5: function () { S.focus('cmbperemeter'); }, ArrowUp: function () { S.focus('cmbperemeter'); },
             ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('datGdnFromDate').value = A.today();
    el('dateGdnToDate').value = A.today();
    grid.render(); summary.render();
    parameterFill();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        branchesFill(lookup.branches);
        return comboFill();
    }).then(function () {
        S.activate('cmbperemeter', 1, false);                               // Rows[1].Activate() = This Week
        S.focus('cmbperemeter');
    }).catch(function (e) { alert(e.message); });
}(window, document));
