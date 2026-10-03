/* 480 Purchase Report (With Activites) - Architecture.WinApp.Inventory_Reports.PurchaseRegisterNew. ":NNN" = line in
   PurchaseRegisterNew.cs. API /purchase/api/reports/purchase-register. Built on pr_common.js (window.PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/purchase/api/reports/purchase-register';
    var st = { init: null, detailFilter: null, activity: '', detailHistory: [], dtGridSummary: [], summaryType: '', printSumm: '' };
    var from = new PR.Picker('txtDateFrom'), to = new PR.Picker('txtToDate');
    var fromS = new PR.Picker('txtFromDateSumm'), toS = new PR.Picker('txtToDateSumm');
    var branches = new PR.CheckList('cmbBranchName'), branchesS = new PR.CheckList('CmbBranchNameSumm');
    var parents = new PR.CheckList('cmbparentcategoryCombo'), parentsS = new PR.CheckList('cmbParentcategorySumm');
    var groups = new PR.CheckList('CmbCustomGroup'), groupsS = new PR.CheckList('CmbCustomGroupSummary');
    var grid = new PR.Grid('DataGridHistory', 'lblCount'), gridS = new PR.Grid('GridSummary', 'lblCountSumm');
    var VALID_PARENTS = [1, 2, 4];                     // validIds (:869, :1058)

    // ------------------------------------------------------------------ combos
    /** AllcomboFillFromPurchaseInvoice :787 - Usp_AllComboAgainstPurchaseInvoice split by Activity; ZeroIndex true, row 0 active. */
    function bindDetail(rows) {
        var by = function (a) { return PR.byActivity(rows, a); };
        parents.set(by('ItemParentCategory').filter(function (r) { return VALID_PARENTS.indexOf(PR.int(r.Id)) >= 0; })
            .map(function (r) { return { id: r.Id, name: r.ReferenceName }; }), []);
        [['cmbItemClass', 'ItemClassGroup'], ['cmbItemCategory', 'ItemCategory'], ['cmbItemtype', 'ItemType'], ['cmbCropYear', 'CropYear'],
            ['cmbPackingtype', 'PackingType'], ['cmbjoblot', 'JobLot'], ['cmbwarehouse', 'WareHouse'], ['cmbItemName', 'ItemName'],
            ['CmbStockAccount', 'StockAccount'], ['cmbsupplier', 'Supplier'], ['cmbcommagent', 'CommissionAgent'], ['CmbDocumentType', 'DocumentType']]
            .forEach(function (p) { PR.fill(p[0], by(p[1]), { zeroIndex: true }); $(p[0]).value = ''; });
    }
    /** AllcomboFillFromEvaluation :974 - USP_Inventory_StockEvalautionDetail_DropDownAndLists (56,57) by ActivityType. */
    function bindSummary(rows) {
        var by = function (a) { return PR.byActivity(rows, a, 'ActivityType'); };
        parentsS.set(by('ParentCategories').filter(function (r) { return VALID_PARENTS.indexOf(PR.int(r.Id)) >= 0; })
            .map(function (r) { return { id: r.Id, name: r.name }; }), []);
        [['CmbItemClassSumm', 'ItemClassGroup'], ['CmbCategorySumm', 'ItemCategories'], ['cmbItemtypeSumm', 'ItemTypes'], ['CmbCropYearSumm', 'CropYear'],
            ['CmbPackingTypeSumm', 'PackingType'], ['cmbJobLotSumm', 'JobLot'], ['CmbWareHouseSumm', 'Warehouse'], ['cmbItemNameSumm', 'Items'],
            ['cmbSupplierSumm', 'Supplier_Customer'], ['cmbcitySumm', 'City'], ['cmbDistrictSumm', 'District'], ['CmbStockAccountSummary', 'Stock_Account']]
            .forEach(function (p) { PR.fill(p[0], by(p[1]), { zeroIndex: true, text: 'name' }); $(p[0]).value = ''; });
    }
    function detailLookups() { return PR.request(API + '/detail-lookups?branchIds=' + encodeURIComponent(branches.ids().join(','))).then(bindDetail); }
    /** The summary lists follow the DETAIL tab's branches (AllcomboFillFromEvaluation reads cmbBranchName, :987). */
    function summaryLookups() { return PR.request(API + '/summary-lookups?branchIds=' + encodeURIComponent(branches.ids().join(','))).then(bindSummary); }
    branches.onLeave = function () {                // cmbBranchName_Leave :2487
        if (branches.ids().length) { PR.run(detailLookups); return; }
        parents.set([], []);
        ['cmbItemClass', 'cmbItemCategory', 'cmbItemtype', 'cmbCropYear', 'cmbPackingtype', 'cmbjoblot', 'cmbwarehouse', 'cmbItemName', 'cmbsupplier', 'cmbcommagent']
            .forEach(function (id) { PR.fill(id, [], {}); });
    };
    branchesS.onLeave = function () {               // CmbBranchNameSumm_Leave :2525 - tests the DETAIL branch combo
        if (branches.ids().length) { PR.run(summaryLookups); return; }
        parentsS.set([], []);
        ['CmbItemClassSumm', 'CmbCategorySumm', 'cmbItemtypeSumm', 'cmbItemNameSumm', 'cmbJobLotSumm', 'CmbWareHouseSumm', 'CmbCropYearSumm',
            'CmbPackingTypeSumm', 'cmbDistrictSumm', 'cmbcitySumm', 'cmbSupplierSumm'].forEach(function (id) { PR.fill(id, [], {}); });
    };

    // ------------------------------------------------------------------ detail tab
    function detailFilter() {
        return {
            branchIds: branches.ids(), fromDate: from.value(), toDate: to.value(), fromNo: PR.val('txtFromDocNo'), toNo: PR.val('txtToDocNo'),
            itemClassId: PR.val('cmbItemClass'), itemCategoryId: PR.val('cmbItemCategory'), itemTypeId: PR.val('cmbItemtype'),
            cropYearId: PR.val('cmbCropYear'), cropYear: PR.text('cmbCropYear'), packingTypeId: PR.val('cmbPackingtype'), jobLotId: PR.val('cmbjoblot'),
            warehouseId: PR.val('cmbwarehouse'), itemId: PR.val('cmbItemName'), supplierId: PR.val('cmbsupplier'), commissionAgentId: PR.val('cmbcommagent'),
            documentTypeId: PR.val('CmbDocumentType'), stockAccountId: PR.val('CmbStockAccount'), reportType: PR.text('cmbActivity'),
            parentCategoryIds: parents.ids(), customGroupIds: groups.ids()
        };
    }
    var INT_KEYS = ['Id', 'DocumentTypeId', 'PurchaseOrderId', 'PoNo', 'BillNo', 'BranchSrNo', 'GpNo', 'GrnNo'];
    var TEXT_KEYS = ['ParentCategory', 'TransactionType', 'PurchaseType', 'SupplierName', 'ManualNo', 'WareHouseName', 'ItemName', 'PackUom', 'CropYear',
        'JobLot', 'PackingType', 'VehicleNo', 'TicketNos', 'CommissionAgent', 'CommissionType', 'BrokerName', 'BrokeryType', 'CityName'];
    var DETAIL_KEYS = ['ParentCategory', 'TransactionType', 'PurchaseType', 'SupplierName', 'Id', 'DocumentTypeId', 'PurchaseOrderId', 'PoNo', 'BillDate', 'BillNo',
        'BranchSrNo', 'ManualNo', 'WareHouseName', 'ItemName', 'PackUom', 'CropYear', 'JobLot', 'PackingType', 'ItemQty', 'QtyByParentCategory', 'BillWeight',
        'StockWeight', 'ItemRate', 'RateCut', 'RateCutAmount', 'BillAmount', 'Commission', 'Freight', 'Expense', 'Wages', 'EmptyBagsAmount', 'EmptyBagsAmount40KG',
        'FreightDeduction', 'FreightDeduction40KG', 'ItemAddLess(JV)', 'TotalExpense', 'Exp/40kg', 'Amount+Exp', 'PartyBillAmount', 'PartyAddLess_JV',
        'RateWithExp', 'StockAvgRateKg', 'AvgRatePerKgOnBillWeight', 'GpNo', 'GpDate', 'GrnDate', 'GrnNo', 'VehicleNo', 'TicketNos', 'CommissionAgent',
        'CommissionType', 'CommRate', 'BrokerName', 'BrokeryType', 'BrokeryRate', 'CityName'];
    var COMPARISON_KEYS = ['ParentCategory', 'ItemName', 'CropYear', 'ItemQty', 'QtyByParentCategory', 'BillWeight', 'StockWeight', 'ItemRate', 'BillAmount',
        'TotalExpense', 'Amount+Exp', 'RateWithExp', 'TotalExp40Kg', 'Freight40Kg', 'Comm40Kg', 'Expense40Kg', 'Wages40Kg', 'EmptyBagsAmount', 'EmptyBagsAmount40KG',
        'FreightDeduction', 'FreightDeduction40KG', 'Freight', 'Commission', 'Expense', 'Wages', 'ItemAddLess(JV)'];
    var CAPTIONS = { BillAmount: 'Item Amount', EmptyBagsAmount40KG: 'Empty Bags Amount 40KG', FreightDeduction40KG: 'Freight Deduction 40KG' };
    var CAPTIONS_COMPARISON = { TotalExp40Kg: 'Total Exp 40Kg', Freight40Kg: 'Freight 40Kg', Comm40Kg: 'Comm 40Kg', Expense40Kg: 'Expense 40Kg', Wages40Kg: 'Wages 40Kg' };
    var CAPTIONS_DETAIL = { PoNo: 'Order No', Expense: 'Other Expense', RateWithExp: 'AvgRate 40Kg', StockAvgRateKg: 'AvgRate PerKG (@StockWeight)',
        AvgRatePerKgOnBillWeight: 'AvgRate PerKg (@BillWeight)' };
    var WIDTHS = { ParentCategory: 100, ItemName: 180, CropYear: 75, ItemQty: 80, QtyByParentCategory: 90, BillWeight: 85, StockWeight: 85, ItemRate: 70,
        BillAmount: 100, Commission: 80, Freight: 80, Expense: 80, Wages: 75, TotalExpense: 75, 'Amount+Exp': 100 };
    var WIDTHS_COMPARISON = { RateWithExp: 85, TotalExp40Kg: 85, Freight40Kg: 85, Comm40Kg: 75, Expense40Kg: 80, Wages40Kg: 80 };
    var WIDTHS_DETAIL = { TransactionType: 80, PurchaseType: 100, SupplierName: 150, BillDate: 80, BillNo: 50, BranchSrNo: 50, ManualNo: 80, GpNo: 50, PoNo: 50,
        GpDate: 75, GrnDate: 80, GrnNo: 50, PackUom: 55, JobLot: 90, PackingType: 95, 'Exp/40kg': 70, PartyBillAmount: 100, PartyAddLess_JV: 100, RateWithExp: 70,
        WareHouseName: 110, TicketNos: 70, AvgRatePerKgOnBillWeight: 90 };

    /** DataGridHistorySetting :1355 (GridEX_Helper.GridWrappingAndColumnSettings(grid, 3, 2) is not in the decompiled
        sources: numeric columns are summed, "Amount" columns in the amount format, as the other ported registers do). */
    function detailColumns(activity) {
        var detail = activity === 'Detail';
        var keys = detail ? DETAIL_KEYS : activity === 'Comparison By Item' ? COMPARISON_KEYS
            : COMPARISON_KEYS.slice(0, 3).concat(['SupplierName']).concat(COMPARISON_KEYS.slice(3));
        var cols = keys.map(function (k) {
            var c = { key: k, caption: (detail ? CAPTIONS_DETAIL[k] : CAPTIONS_COMPARISON[k]) || CAPTIONS[k] || k,
                width: (detail ? WIDTHS_DETAIL[k] : WIDTHS_COMPARISON[k]) || WIDTHS[k] || 90 };
            if (k === 'SupplierName' && !detail) c.width = 150;
            if (/Date$/.test(k)) c.type = 'short';
            else if (INT_KEYS.indexOf(k) >= 0) c.type = 'text';
            else if (TEXT_KEYS.indexOf(k) < 0) { c.type = 'num'; c.fmt = /Amount/.test(k) ? 'amt' : 'n2'; c.agg = 'sum'; }
            return c;
        });
        if (detail) {
            cols.forEach(function (c) {
                if (['Id', 'PurchaseOrderId', 'DocumentTypeId'].indexOf(c.key) >= 0) c.hidden = true;
                // PoNo / BillNo: the desktop opens ActivityDetails / PurchaseInvoiceReverseActivity (not ported); the web opens
                // the purchase order and the invoice (its own screen by DocumentTypeId).
                if (c.key === 'PoNo') c.open = function (r) { return PR.int(r.PurchaseOrderId) > 0 ? [41, PR.int(r.PurchaseOrderId)] : null; };
                if (c.key === 'BillNo') c.open = function (r) { return PR.int(r.Id) > 0 ? [PR.int(r.DocumentTypeId), PR.int(r.Id)] : null; };
            });
            cols.unshift({ key: 'ApprovalDetail', caption: 'ApprovalDetail', width: 100, button: 'ApprovalDetail' });
        }
        return cols;
    }
    /** GridFill :1226. */
    function GridFill() {
        var f = detailFilter();
        if (!f.branchIds.length) return Promise.reject(new Error('Select Branch First'));
        return PR.request(API + '/detail', f).then(function (res) {
            st.detailHistory = res.raw || []; st.detailFilter = f; st.activity = f.reportType;
            if (!(res.rows || []).length) { grid.clear(); return; }
            grid.show(detailColumns(f.reportType), res.rows, { groupBy: 'ParentCategory', selector: f.reportType === 'Detail', checkedStyle: true,
                onButton: function (col, r) {      // DataGridHistory_ColumnButtonClick :1440
                    if (col === 'ApprovalDetail') PR.approval(API + '/approval', { filter: st.detailFilter, id: PR.int(r.Id), documentTypeId: PR.int(r.DocumentTypeId) });
                } });
        });
    }
    /** btnPrint_DropDownItemClicked :2565 - detailHistory into the chosen .rpt of the PurchaseRegister_* report folder. */
    function detailPrint() {
        if (!st.detailHistory.length) { PR.box('Record not found for Display'); return; }
        PR.run(function () { return PR.printGrid('', 'Purchase Invoice Register - ' + st.activity, st.detailHistory); });
    }

    // ------------------------------------------------------------------ summary tab
    var SUMMARY_PRINT = { 'Purchase Register': '56_01_PurchaseRegister', 'Purchase Summary By Item': '56_02_PurchaseSummaryByItem',
        'Purchase Summary By Item & Warehouse': '56_03_PurchaseSummaryByItemAndWarehouse', 'Purchase Summary By Supplier': '56_04_PurchaseSummaryBySupplier',
        'Purchase Summary By Item & Supplier': '56_05_PurchaseSummaryByItemAndSupplier', 'Purchase Summary By Parent Category': '56_06_PurchaseSummaryByParentCategory',
        'Purchase Summary By Parent Category & Supplier': '56_07_PurchaseSummaryByParentCategoryAndSupplier', 'Purchase Summary By HsCode': '56_09_PurchaseSummaryByHsCode' };
    var S_WIDTHS = { Qty: 70, GrossWeight: 80, EbTotal: 65, AddLess: 70, BillWeight: 90, StockWeight: 85, AvgRate: 75, Amount: 100, TotalExpenses: 85,
        TotalAmountWithExp: 100, AvgRateExp: 75, Freight: 75, Commission: 80, Expenses: 70, Wages: 70, EmptyBagsAmount: 85, FreightDeduction: 85,
        PrcntOfTotal: 60, PrcntOfTotalWeight: 80, DocDate: 80, DocumentType: 70, DocCodeNo: 50, PartyName: 120, ParentCategory: 110, ItemCategory: 110,
        ItemType: 100, ItemCode: 60, ItemName: 120, UOMCode: 60, CropBatch: 75, JobLotCode: 90, PackTypeCode: 95, VehicleNo: 80, CityName: 120,
        WarehouseName: 110, HsCode: 100 };
    var S_CAPTIONS = { PrcntOfTotal: 'Pct Of Total', PrcntOfTotalWeight: 'Pct Of Total Weight', UOMCode: 'Pack Uom', DocCodeNo: 'Doc No', DocumentType: 'Doc Type',
        CropBatch: 'Crop Year', JobLotCode: 'Job Lot', PackTypeCode: 'Packing Type' };
    var S_N3 = ['Qty', 'GrossWeight', 'EbTotal', 'AddLess', 'BillWeight', 'StockWeight', 'PrcntOfTotal', 'PrcntOfTotalWeight'];
    var S_AMT = ['Amount', 'TotalExpenses', 'TotalAmountWithExp', 'Freight', 'Commission', 'Expenses', 'Wages', 'EmptyBagsAmount', 'FreightDeduction'];
    /** GridSummarySetting :1968 plus the per-type widths/captions (:1797-1939). */
    function summaryColumns(type, keys) {
        return keys.map(function (k) {
            var c = { key: k, caption: S_CAPTIONS[k] || k, width: S_WIDTHS[k] || 90 };
            if (k === 'RefDocumentTypeId') c.hidden = true;
            if (k === 'DocDate') c.type = 'short';
            if (S_N3.indexOf(k) >= 0) { c.type = 'num'; c.fmt = 'n3'; c.agg = 'sum'; }
            if (S_AMT.indexOf(k) >= 0) { c.type = 'num'; c.fmt = 'amt'; c.agg = 'sum'; }
            if (k === 'AvgRate' || k === 'AvgRateExp') { c.type = 'num'; c.fmt = 'rate'; c.agg = 'avg'; }
            if (type === 'Purchase Summary By Parent Category' && k === 'CityName') c.width = 70;
            if (type !== 'Purchase Register' && k === 'ItemCode') c.width = 80;
            if (type !== 'Purchase Register' && k === 'CityName' && type !== 'Purchase Summary By Parent Category') c.width = 110;
            return c;
        });
    }
    function summaryFilter() {
        return {
            branchIds: branchesS.ids(), fromDate: fromS.value(), toDate: toS.value(), fromNo: PR.val('txtFromDocNoSumm'), toNo: PR.val('txtToDocNoSumm'),
            itemClassId: PR.val('CmbItemClassSumm'), itemCategoryId: PR.val('CmbCategorySumm'), itemTypeId: PR.val('cmbItemtypeSumm'), itemId: PR.val('cmbItemNameSumm'),
            jobLotId: PR.val('cmbJobLotSumm'), warehouseId: PR.val('CmbWareHouseSumm'), cropYearId: PR.val('CmbCropYearSumm'), cropYear: PR.text('CmbCropYearSumm'),
            packingTypeId: PR.val('CmbPackingTypeSumm'), districtId: PR.val('cmbDistrictSumm'), stockAccountId: PR.val('CmbStockAccountSummary'),
            cityId: PR.val('cmbcitySumm'), supplierId: PR.val('cmbSupplierSumm'), cityWise: $('chkCity').checked, packUom: $('chkPackUom').checked,
            reportType: $('cmbReportTypeSummary').value, parentCategoryIds: parentsS.ids(), customGroupIds: groupsS.ids()
        };
    }
    /** BtnShowSumm_Click :1673 -> GridSummaryFill :1702 + PrintButtonManageSummary :2254. */
    function BtnShowSumm() {
        var f = summaryFilter();
        if (!f.reportType) { PR.box('Please Select Activity First...'); $('cmbReportTypeSummary').focus(); return Promise.resolve(); }
        if (!f.branchIds.length) return Promise.reject(new Error('Select Branch First'));
        return PR.request(API + '/summary', f).then(function (res) {
            st.dtGridSummary = res.raw || []; st.summaryType = f.reportType;
            st.printSumm = SUMMARY_PRINT[f.reportType]; $('btnPrintSumm').textContent = st.printSumm;
            $('lblSummaryName').textContent = f.reportType;
            if (!(res.rows || []).length) { gridS.clear(); return; }
            var group = f.reportType !== 'Purchase Summary By Supplier' && f.reportType !== 'Purchase Summary By HsCode';
            gridS.show(summaryColumns(f.reportType, Object.keys(res.rows[0])), res.rows, { groupBy: group ? 'ParentCategory' : null });
        });
    }
    /** btnPrintSumm_Click :2131 - dtGridSummary into 56_0x. */
    function summaryPrint() {
        if (!st.printSumm) return;
        if (!st.dtGridSummary.length) { PR.box('Record Not Found For Display'); return; }
        PR.run(function () { return PR.printGrid(st.printSumm + '.rpt', st.printSumm, st.dtGridSummary); });
    }
    /** BtnNewSummary_Click :1648 - clears, report type row 0, then Show. */
    function newSummary() {
        ['cmbSupplierSumm', 'CmbCategorySumm', 'cmbItemNameSumm', 'cmbcitySumm', 'cmbDistrictSumm', 'CmbItemClassSumm', 'cmbJobLotSumm', 'CmbCropYearSumm']
            .forEach(function (id) { $(id).value = ''; });
        parentsS.clear(); $('txtFromDocNoSumm').value = ''; $('txtToDocNoSumm').value = '';
        $('cmbReportTypeSummary').value = 'Purchase Register';
        return PR.run(BtnShowSumm).then(function () { fromS.focus(); });
    }

    function shortcuts() {
        PR.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Show'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    /** frmEvaulationDetailSalesReports_Load :320. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            st.init = d; PR.cfg.amountDecimals = d.amountDecimals; PR.cfg.rateDecimals = d.rateDecimals;
            from.set(d.now); to.set(d.now); fromS.set(d.now); toS.set(d.now);
            var b = (d.branches || []).map(function (x) { return { id: x.BranchId, name: x.BranchName }; });
            branchesS.set(b, d.branchId ? [d.branchId] : []); branches.set(b, d.branchId ? [d.branchId] : []);
            bindDetail(d.detailLookups);
            PR.fill('cmbActivity', d.activities.map(function (a, i) { return { Id: i + 1, Name: a }; }), { text: 'Name' });
            $('cmbActivity').value = '1';                   // Rows[0] "Detail"
            bindSummary(d.summaryLookups);
            PR.fill('cmbReportTypeSummary', d.reportTypes.map(function (t) { return { Id: t, name: t }; }), { text: 'name' });
            $('cmbReportTypeSummary').value = d.reportTypes[0];
            var cg = (d.customGroups || []).map(function (r) { return { id: r.Id, name: r.AcLookUpsDescription }; });
            groups.set(cg, []); groupsS.set(cg, []);
            PR.fill('CmbDateTypeSumm', d.dateTypes, { text: 'Parameters' });
            $('CmbDateTypeSumm').value = '2';               // ParameterFill: Rows[1] ("This Week")
            PR.applyDateType(2, fromS, toS, d.yearStart);
            var q = new URLSearchParams(location.search);
            if (!q.get('fromDate')) { var f = new Date(); f.setDate(f.getDate() - 15); from.set(f); return; }
            from.set(q.get('fromDate')); if (q.get('toDate')) to.set(q.get('toDate'));
            if (q.get('supplierId')) $('cmbsupplier').value = q.get('supplierId');
            if (q.get('stockAccountId')) $('CmbStockAccount').value = q.get('stockAccountId');
            if (q.get('itemId')) $('cmbItemName').value = q.get('itemId');
            return GridFill();
        });
    }

    $('CmbDateTypeSumm').addEventListener('change', function () { PR.applyDateType($('CmbDateTypeSumm').value, fromS, toS, st.init ? st.init.yearStart : ''); });
    $('btnShow').addEventListener('click', function () { PR.run(GridFill); });
    $('btnNew').addEventListener('click', function () { if (st.init && st.init.yearStart) from.set(st.init.yearStart, true); });   // btnNew_Click :1564
    $('BtnReferesh').addEventListener('click', function () { PR.run(detailLookups); });
    $('btnPrint').addEventListener('click', detailPrint);
    $('btnShortcutKeys').addEventListener('click', shortcuts);
    $('BtnShowSumm').addEventListener('click', function () { PR.run(BtnShowSumm); });
    $('BtnNewSummary').addEventListener('click', newSummary);
    $('btnRefreshSummary').addEventListener('click', function () { PR.run(summaryLookups); });
    $('btnPrintSumm').addEventListener('click', summaryPrint);
    $('BtnShortCutKeysSumm').addEventListener('click', shortcuts);
    PR.digitsOnly(['txtFromDocNo', 'txtToDocNo']);
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Purchase Invoice Register - ' + (st.activity || ''); });
    PR.gridTools(gridS, 'btnGridPrintSumm', 'btnGridExportSumm', function () { return st.summaryType || 'Summary Register'; });
    PR.fullscreen('btnFullscreen', 'detailSection'); PR.fullscreen('btnFullscreenSumm', 'summarySection');
    PR.tabs(function (t) { if (t === 'summary') fromS.focus(); });
    PR.enterAsTab();
    /** PurchaseRegisterNew_KeyDown :2389. */
    document.addEventListener('keydown', function (e) {
        var k = e.key.toLowerCase(), tab = PR.currentTab();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = '/purchase/reports'; } return; }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        var a = null;
        if (tab === 'detail') {
            if (e.altKey && (e.code === 'Digit1' || e.code === 'Numpad1')) { e.preventDefault(); return; }   // btnPrint_Click is empty
            if (!e.ctrlKey) return;
            a = { n: function () { $('btnNew').click(); }, r: function () { PR.run(summaryLookups); },   // Ctrl+R calls btnRefreshSummary_Click
                s: function () { PR.run(GridFill); }, p: function () {}, f5: function () { from.focus(); }, arrowup: function () { to.focus(); },
                arrowdown: function () { $('gridWrap').focus(); } }[k];
        } else if (e.ctrlKey) {
            a = { n: newSummary, r: function () { PR.run(summaryLookups); }, s: function () { PR.run(BtnShowSumm); }, p: summaryPrint,
                f5: function () { fromS.focus(); }, arrowup: function () { fromS.focus(); }, arrowdown: function () { $('gridWrapSumm').focus(); } }[k];
        }
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
