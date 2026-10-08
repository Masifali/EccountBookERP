/* ============================================================================================
 * Screen 486 frmSaleInvoiceReturnRegister - Architecture.WinApp.Inventory_Reports.frmSaleInvoiceReturnRegister (.cs)
 * Page: templates/sale/reports/sarpt1_sale_invoice_return_register.html (route /sale/reports/sale-invoice-return-register).
 *   frmPurchaseInvoiceRegister_Load :120  ComboFill, BranchesFill, Datetypefill, From = Now - 15 days, cmbDateType.Rows[2].Activate() (This Week: the value
 *                                         change rule sets From = today - 7 and shows), SaleInvoiceRegisterBind, focus the date type
 *   Datetypefill :157     CommonServices.DateType, BindDDL(ZeroIndex true)
 *   ComboFill :171        Usp_AllComboAgainstPurchaseInvoice @DocumentTypeIds='98' [@BranchesIds ",id,id"]: Supplier -> cmbSupplier ("Customer"),
 *                         ItemName -> cmbItem, StockAccount -> CmbStockAccount, all ZeroIndex true with Rows[0] activated
 *   BranchesFill :261     Branches.GetAll (Sp_Branches_GetAllMethod 'GetAll'), BindDDL(Id, BranchName, ZeroIndex false), checked list, text = the user's branch
 *   cmbDateType_ValueChanged :685  1 From = today | 2 today - 7 | 3 first of (UTC) month, To = today | 4 1 Jan, To = today | 5 Start_Period; each then Show
 *   btnshow_Click :380 / SaleInvoiceRegisterBind :392  Sp_InvPurchaseInvoice_SaleReturnRegister (DocumentTypeId 98); no branch -> "Select Branch First";
 *                         33 column table (CommAgent <- CommissionAgent, ManualNo <- ManualBillNo, PackingType <- PackType, NetWeight <- NetBillWeight ...)
 *   DataGridHistorySetting :481  Id hidden, dd-MMM-yy dates, widths, "#,##0.###" weights, DecimalRateFormate rates, stringFormatsingle amounts, sums
 *   btnPrint_Click :596   no rows -> "Record Not Found For Display"; 0228-InvPurchaseInvoice_SaleReturnRegister.rpt (@CompanyName / @CompanyAddress)
 *   btnRefresh :631 (ComboFill), btnnew :643 / Reset :655, btnshow_Leave :670, CmbBranchName_Leave :727, KeyDown :294, MakeShortCutKeys :341
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/sarpt1/sale-invoice-return-register';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null, userBranchId: null };
    var lastArgs = null, hasRows = false, seq = 0;
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl + P', 'For print'],
        ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];

    // ------------------------------------------------------------------ lists
    function datetypefill() { S.fill(el('cmbDateType'), S.dateTypeRows(false), true); }
    function branchesFill(rows) { S.fillBranches(el('CmbBranchName'), rows, lookup.userBranchId); }
    function bindCombos(data) {
        function rows(k) { return (data && data[k]) || []; }
        S.fill(el('cmbSupplier'), rows('Supplier'), true);
        S.fill(el('cmbItem'), rows('ItemName'), true);
        S.fill(el('CmbStockAccount'), rows('StockAccount'), true);
    }
    function comboFill() {
        var ids = S.branchIds(el('CmbBranchName'));
        return A.getJson(API + '/combos?' + S.qs({ branchesIds: ids })).then(bindCombos);
    }
    el('cmbDateType').addEventListener('change', function () {              // cmbDateType_ValueChanged
        var id = A.toInt(this.value);
        if (id < 1 || id > 5) return;
        S.dateRule(id, el('txtFromDate'), el('txtTodate'), lookup.yearStart);
        show();
    });

    // ------------------------------------------------------------------ grid
    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 1 });
    function cols() {
        var t3 = '#,##0.###', single = S.fmtSingle(lookup.amountDecimals), rate = S.fmtRate(lookup.rateDecimals);
        return [
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'BranchSrNo', caption: 'BranchSrNo', width: 70 }, { key: 'BranchName', caption: 'BranchName', width: 150 },
            { key: 'DocNo', caption: 'DocNo', width: 40 }, { key: 'DocDate', caption: 'DocDate', width: 70, date: 'dd-MMM-yy' },
            { key: 'CustomerName', caption: 'CustomerName', width: 150 }, { key: 'CommAgent', caption: 'CommAgent', width: 70 }, { key: 'CommType', caption: 'CommType', width: 60 },
            { key: 'CommRate', caption: 'CommRate', width: 60, fmt: t3, num: true }, { key: 'CommAmount', caption: 'CommAmount', width: 70, fmt: t3, sum: true, num: true },
            { key: 'ManualNo', caption: 'ManualNo', width: 70 }, { key: 'BillAmount', caption: 'BillAmount', width: 80, fmt: single, sum: true, num: true },
            { key: 'ItemName', caption: 'ItemName', width: 150 }, { key: 'CropYear', caption: 'CropYear', width: 70 }, { key: 'JobLot', caption: 'JobLot', width: 70 },
            { key: 'PackingType', caption: 'PackingType', width: 80 }, { key: 'PackUom', caption: 'PackUom', width: 70 },
            { key: 'ItemQty', caption: 'ItemQty', width: 70, fmt: t3, sum: true, num: true }, { key: 'GrossWeight', caption: 'GrossWeight', width: 80, fmt: t3, sum: true, num: true },
            { key: 'WtCut', caption: 'WtCut', width: 70, fmt: t3, sum: true, num: true }, { key: 'WtCutTotal', caption: 'WtCutTotal', width: 100, fmt: t3, sum: true, num: true },
            { key: 'AdLsWt', caption: 'AdLsWt', width: 80, fmt: t3, sum: true, num: true }, { key: 'NetWeight', caption: 'NetWeight', width: 100, fmt: t3, sum: true, num: true },
            { key: 'StockWeight', caption: 'StockWeight', width: 100, fmt: t3, sum: true, num: true },
            { key: 'ItemRate', caption: 'ItemRate', width: 100, fmt: rate, num: true }, { key: 'RateUom', caption: 'RateUom', width: 80 },
            { key: 'RateCut', caption: 'RateCut', width: 80, fmt: rate, num: true },
            { key: 'RateCutAmount', caption: 'RateCutAmount', width: 100, fmt: single, sum: true, num: true },
            { key: 'ItemAmount', caption: 'ItemAmount', width: 120, fmt: single, sum: true, num: true },
            { key: 'Warehouse', caption: 'Warehouse', width: 120 }, { key: 'GpDate', caption: 'GpDate', width: 70, date: 'dd-MMM-yy' },
            { key: 'GpNo', caption: 'GpNo', width: 40 }, { key: 'VehicleNo', caption: 'VehicleNo', width: 100 }
        ];
    }
    function mapRow(r) {
        var c = A.ci;
        return { Id: c(r, 'Id'), BranchSrNo: c(r, 'BranchSrNo'), BranchName: c(r, 'BranchName'), DocNo: c(r, 'DocNo'), DocDate: c(r, 'DocDate'),
            CustomerName: c(r, 'SupplierName'), CommAgent: c(r, 'CommissionAgent'), CommType: c(r, 'CommissionType'), CommRate: c(r, 'CommRate'),
            CommAmount: c(r, 'CommAmount'), ManualNo: c(r, 'ManualBillNo'), BillAmount: c(r, 'BillAmount'), ItemName: c(r, 'ItemName'), CropYear: c(r, 'CropYear'),
            JobLot: c(r, 'JobLot'), PackingType: c(r, 'PackType'), PackUom: c(r, 'UOM'), ItemQty: c(r, 'ItemQty'), GrossWeight: c(r, 'GrossWeight'),
            WtCut: c(r, 'WeightCut'), WtCutTotal: c(r, 'WeightCutTotal'), AdLsWt: c(r, 'AdLsWeight'), NetWeight: c(r, 'NetBillWeight'),
            StockWeight: c(r, 'NetStockWeight'), ItemRate: c(r, 'ItemRate'), RateUom: c(r, 'RateUOM'), RateCut: c(r, 'RateCut'), RateCutAmount: c(r, 'RateCutAmount'),
            ItemAmount: c(r, 'Amount'), Warehouse: c(r, 'WareHouseName'), GpDate: c(r, 'GpDate'), GpNo: c(r, 'GpNo'), VehicleNo: c(r, 'VehicleNo') };
    }

    // ------------------------------------------------------------------ Show / print
    function show() {
        var b = el('show'); if (b.disabled) return;
        var ids = S.branchIds(el('CmbBranchName'));
        if (!ids) { alert('Select Branch First'); S.focus('CmbBranchName'); return; }
        var a = { supplierCustomerId: S.selInt('cmbSupplier'), itemId: S.selInt('cmbItem'), stockAccountId: S.selInt('CmbStockAccount'),
                  fromDate: el('txtFromDate').value, toDate: el('txtTodate').value, branchesIds: ids };
        var token = ++seq;
        A.busy(b, true);
        A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.clear(); return; }
            grid.setData(cols(), rows.map(mapRow));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function print() {
        var b = el('print'); if (b.disabled) return;
        if (!hasRows || !lastArgs) { alert('Record Not Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/sale/reports/sarpt1/print/sale-invoice-return-register?' + S.qs(lastArgs)).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function blank(id) { var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true })); }
    function reset() { blank('cmbItem'); blank('cmbSupplier'); S.focus('cmbDateType'); datetypefill(); }
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        comboFill().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    S.onLeave('CmbBranchName', function () {                                // CmbBranchName_Leave
        if (S.branchIds(el('CmbBranchName'))) comboFill().catch(function (e) { alert(e.message); });
        else { ['cmbSupplier', 'cmbItem'].forEach(function (id) { S.fill(el(id), [], true); blank(id); }); }
    });
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && el('grid').contains(e.relatedTarget)) S.focus('cmbDateType'); });
    S.keys({ p: print, s: show, e: closeForm, n: reset, r: refresh, F5: function () { S.focus('cmbDateType'); }, ArrowUp: function () { S.focus('cmbDateType'); },
             ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('txtFromDate').value = A.today(); el('txtTodate').value = A.today();
    grid.render();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        return A.getJson(API + '/combos?' + S.qs({ branchesIds: '' })).then(function (c) {      // ComboFill runs before BranchesFill: no branch filter yet
            bindCombos(c);
            branchesFill(lookup.branches);
            datetypefill();
            el('txtFromDate').value = A.addDays(A.today(), -15);                // Now.AddDays(-15)
            S.activate('cmbDateType', 2, true);                                 // Rows[2] = This Week: From = today - 7 and Show
            S.focus('cmbDateType');
        });
    }).catch(function (e) { alert(e.message); });
}(window, document));
