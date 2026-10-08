/* ============================================================================================
 * Screen 487 frmSaleInvoiceDirectRegister - Architecture.WinApp.Inventory_Reports.frmSaleInvoiceDirectRegister (.cs)
 * Page: templates/sale/reports/sarpt1_sale_invoice_direct_register.html (route /sale/reports/sale-invoice-direct-register).
 *   frmSaleInvoiceRegister_Load :137  ReportsLoad, BranchesFill, Datetypefill, AllComboBindAgainstSale, DocumentTypeFill, ActivityFill, cmbDateType.Rows[2].Activate()
 *                                     (This Week: the rule shows), GRNfromdate = Now - 15 days, CmbActivity.Rows[1].Activate() ("Sale"), gridHisory (final Show), focus
 *   ReportsLoad :181      the "SaleDirectRegister" folder .rpt files -> the Print drop-down
 *   AllComboBindAgainstSale :200  Usp_AllComboAgainstSaleInvoice @DocumentTypeIds='99,139,184' [@BranchesIds]: Supplier -> CmbSupplier, WareHouse -> CmbWarehouse,
 *                         ItemName -> cmbitem, JobLot -> CmbJobLot, StockAccount -> CmbStockAccount; all ZeroIndex true, Rows[0] activated
 *   BranchesFill :352     GetBranchsAllocatedToUserFromSaleInvoice (document type 99), checked list, text = the user's branch
 *   ActivityFill :380     1 Sale, 2 Sale Return (ZeroIndex true).  DocumentTypeFill :399  99 SID, 184 SID-Auto (ZeroIndex true).  Datetypefill :418 (ZeroIndex true)
 *   gridHisory :432 / btnshow_Click :523  Sp_InvSaleInvoiceDirectRegister; @Activity = the Activity combo TEXT (the "...Select Any Value..." placeholder when row 0 is
 *                         active, exactly as the desktop sends it); 32 column table (TranType <- TransactionType, PaymentTerm <- TermsDescription, JobLot <- JobLotDescription,
 *                         PackingType <- PackTypeDesc, PackUom <- ItemUom, Discount% <- ItemDiscount, DiscountAmount <- ItemDiscountAmount, AmountWithDiscount <- ItemAmountWithDiscount)
 *   GridSettings :535     RateCut / RateCutAmount / FreightAmount / commissionAmount / ExpenseAmount hidden (the desktop addresses them through ColumnSets, which throws
 *                         there; the intended hide, widths and formats are applied), dd-MMM-yy, "#,##0.##" qty / weight, stringFormatsingle amounts, "#,##0.###" discounts
 *   print_Click :619      no rows -> "Record Not Found For Dispaly" (sic); 302-InvSaleInvoice.rpt.  tsDropDown_DropDownItemClicked :858 (the folder's report over dtSale)
 *   btnNew :728 / reset :755 (job lot / item / customer / warehouse text cleared, then Show), btnRefresh :740 (warehouse, job lot, item, supplier re-bound from the all-records lists),
 *   cmbDateType_ValueChanged :801, btnshow_Leave :843, CmbBranchName_Leave :881, KeyDown :642, MakeShortCutKeys :689
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/sarpt1/sale-invoice-direct-register';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null, userBranchId: null };
    var lastArgs = null, hasRows = false, seq = 0, reports = [];
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl + P', 'For print'],
        ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];

    // ------------------------------------------------------------------ lists
    function datetypefill() { S.fill(el('cmbDateType'), S.dateTypeRows(false), true); }
    function branchesFill(rows) { S.fillBranches(el('CmbBranchName'), rows, lookup.userBranchId); }
    function activityFill() { S.fill(el('CmbActivity'), [{ Id: 1, name: 'Sale' }, { Id: 2, name: 'Sale Return' }], true); }
    function documentTypeFill() { S.fill(el('CmbDocumentType'), [{ Id: 99, name: 'SID' }, { Id: 184, name: 'SID-Auto' }], true); }
    function bindCombos(data) {
        function rows(k) { return (data && data[k]) || []; }
        S.fill(el('CmbSupplier'), rows('Supplier'), true);
        S.fill(el('CmbWarehouse'), rows('WareHouse'), true);
        S.fill(el('cmbitem'), rows('ItemName'), true);
        S.fill(el('CmbJobLot'), rows('JobLot'), true);
        S.fill(el('CmbStockAccount'), rows('StockAccount'), true);
    }
    function comboFill() {
        return A.getJson(API + '/combos?' + S.qs({ branchesIds: S.branchIds(el('CmbBranchName')) })).then(bindCombos);
    }
    el('cmbDateType').addEventListener('change', function () {              // cmbDateType_ValueChanged
        var id = A.toInt(this.value);
        if (id < 1 || id > 5) return;
        S.dateRule(id, el('GRNfromdate'), el('GRNToDate'), lookup.yearStart);
        show();
    });

    // ------------------------------------------------------------------ grid
    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 1 });
    function cols() {
        var q2 = '#,##0.##', q3 = '#,##0.###', single = S.fmtSingle(lookup.amountDecimals), rate = S.fmtRate(lookup.rateDecimals);
        return [
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'BranchSrNo', caption: 'BranchSrNo', width: 70 }, { key: 'BranchName', caption: 'BranchName', width: 150 },
            { key: 'TranType', caption: 'TranType', width: 60 }, { key: 'DocumentType', caption: 'DocumentType', width: 70 }, { key: 'DocNo', caption: 'DocNo', width: 40 },
            { key: 'DocDate', caption: 'DocDate', width: 70, date: 'dd-MMM-yy' }, { key: 'ManualBillNo', caption: 'ManualBillNo', width: 70 },
            { key: 'CustomerName', caption: 'CustomerName', width: 150 }, { key: 'PaymentTerm', caption: 'PaymentTerm', width: 70 },
            { key: 'WareHouseName', caption: 'WareHouseName', width: 120 }, { key: 'ItemName', caption: 'ItemName', width: 150 }, { key: 'JobLot', caption: 'JobLot', width: 100 },
            { key: 'CropYear', caption: 'CropYear', width: 60 }, { key: 'PackingType', caption: 'PackingType', width: 80 },
            { key: 'ItemQty', caption: 'ItemQty', width: 70, fmt: q2, sum: true, num: true }, { key: 'PackUom', caption: 'PackUom', width: 70 },
            { key: 'NetBillWeight', caption: 'NetBillWeight', width: 80, fmt: q2, sum: true, num: true }, { key: 'RateUom', caption: 'RateUom', width: 50 },
            { key: 'ItemRate', caption: 'ItemRate', width: 50, num: true, fmt: '#,##0.##########' },
            { key: 'RateCut', caption: 'RateCut', width: 100, hidden: true, fmt: rate, num: true },
            { key: 'RateCutAmount', caption: 'RateCutAmount', width: 100, hidden: true, fmt: single, sum: true, num: true },
            { key: 'ItemAmount', caption: 'ItemAmount', width: 80, fmt: single, sum: true, num: true }, { key: 'DiscountType', caption: 'DiscountType', width: 80 },
            { key: 'Discount%', caption: 'Discount%', width: 60, fmt: q3, num: true },
            { key: 'DiscountAmount', caption: 'DiscountAmount', width: 80, fmt: q3, sum: true, num: true },
            { key: 'AmountWithDiscount', caption: 'AmountWithDiscount', width: 90, fmt: q3, sum: true, num: true },
            { key: 'CityName', caption: 'CityName', width: 90 }, { key: 'VehicleNo', caption: 'VehicleNo', width: 80 },
            { key: 'FreightAmount', caption: 'FreightAmount', width: 120, hidden: true, num: true },
            { key: 'commissionAmount', caption: 'commissionAmount', width: 120, hidden: true, num: true },
            { key: 'ExpenseAmount', caption: 'ExpenseAmount', width: 120, hidden: true, fmt: single, sum: true, num: true },
            { key: 'RemarksHeader', caption: 'RemarksHeader', width: 200 }
        ];
    }
    function mapRow(r) {
        var c = A.ci;
        return { Id: c(r, 'Id'), BranchSrNo: c(r, 'BranchSrNo'), BranchName: c(r, 'BranchName'), TranType: c(r, 'TransactionType'), DocumentType: c(r, 'DocumentType'),
            DocNo: c(r, 'DocNo'), DocDate: c(r, 'DocDate'), ManualBillNo: c(r, 'ManualBillNo'), CustomerName: c(r, 'CustomerName'), PaymentTerm: c(r, 'TermsDescription'),
            WareHouseName: c(r, 'WareHouseName'), ItemName: c(r, 'ItemName'), JobLot: c(r, 'JobLotDescription'), CropYear: c(r, 'CropYear'), PackingType: c(r, 'PackTypeDesc'),
            ItemQty: c(r, 'ItemQty'), PackUom: c(r, 'ItemUom'), NetBillWeight: c(r, 'NetBillWeight'), RateUom: c(r, 'RateUom'), ItemRate: c(r, 'ItemRate'),
            RateCut: c(r, 'RateCut'), RateCutAmount: c(r, 'RateCutAmount'), ItemAmount: c(r, 'ItemAmount'), DiscountType: c(r, 'DiscountType'),
            'Discount%': c(r, 'ItemDiscount'), DiscountAmount: c(r, 'ItemDiscountAmount'), AmountWithDiscount: c(r, 'ItemAmountWithDiscount'), CityName: c(r, 'CityName'),
            VehicleNo: c(r, 'VehicleNo'), FreightAmount: c(r, 'FreightAmount'), commissionAmount: c(r, 'CommissionAmount'), ExpenseAmount: c(r, 'ExpenseAmount'),
            RemarksHeader: c(r, 'RemarksHeader') };
    }

    // ------------------------------------------------------------------ Show / print
    function show() {
        var b = el('show'); if (b.disabled) return;
        var a = { fromDate: el('GRNfromdate').value, toDate: el('GRNToDate').value, supplierCustomerId: S.selInt('CmbSupplier'), itemId: S.selInt('cmbitem'),
                  jobLotId: S.selInt('CmbJobLot'), warehouseId: S.selInt('CmbWarehouse'), documentTypeId: S.selInt('CmbDocumentType'),
                  stockAccountId: S.selInt('CmbStockAccount'), activity: S.selText('CmbActivity'), branchesIds: S.branchIds(el('CmbBranchName')) };
        var token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.clear(); return; }
            grid.setData(cols(), rows.map(mapRow));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function print302() {
        var b = el('print302'); if (b.disabled) return;
        if (!hasRows || !lastArgs) { alert('Record Not Found For Dispaly'); return; }
        A.busy(b, true);
        A.openPdf('/sale/reports/sarpt1/print/sale-invoice-direct-register?' + S.qs(Object.assign({ kind: '302' }, lastArgs))).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function printWith(name) {
        if (!hasRows || !lastArgs) { alert('Record Not Found For Display'); return; }
        A.openPdf('/sale/reports/sarpt1/print/sale-invoice-direct-register?' + S.qs(Object.assign({ template: name }, lastArgs))).catch(function (e) { alert(e.message); });
    }
    function loadReports() { return A.getJson('/api/sale/sarpt1/dynamic-reports?folder=SaleDirectRegister&report=sale-invoice-direct-register').then(function (l) { reports = l || []; }); }
    S.dropdown('print', 'printMenu', function () { return reports; }, printWith);

    function blank(id) { var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true })); }
    function reset() { ['CmbJobLot', 'cmbitem', 'CmbSupplier', 'CmbWarehouse'].forEach(blank); show(); }       // reset()
    function refresh() {                                                                                     // btnRefresh_Click
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        A.getJson(API + '/refresh').then(function (o) {
            S.fill(el('CmbWarehouse'), o.warehouses, false);
            S.fill(el('CmbJobLot'), o.jobLots, false);
            S.fill(el('cmbitem'), o.items, false);
            S.fill(el('CmbSupplier'), o.suppliers, false);
        }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print302').addEventListener('click', print302);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    S.onLeave('CmbBranchName', function () {                                // CmbBranchName_Leave
        if (S.branchIds(el('CmbBranchName'))) comboFill().catch(function (e) { alert(e.message); });
        else ['CmbSupplier', 'cmbitem', 'CmbWarehouse', 'CmbJobLot'].forEach(function (id) { S.fill(el(id), [], true); blank(id); });
    });
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && el('grid').contains(e.relatedTarget)) S.focus('cmbDateType'); });
    S.keys({ p: print302, s: show, e: closeForm, n: reset, r: refresh, F5: function () { S.focus('cmbDateType'); }, ArrowUp: function () { S.focus('cmbDateType'); },
             ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('GRNfromdate').value = A.today(); el('GRNToDate').value = A.today();
    grid.render();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        return loadReports().catch(function () { reports = []; });                      // ReportsLoad
    }).then(function () {
        branchesFill(lookup.branches);                                                  // BranchesFill
        datetypefill();                                                                 // Datetypefill
        return comboFill();                                                             // AllComboBindAgainstSale (with the user's branch)
    }).then(function () {
        documentTypeFill(); activityFill();
        S.activate('cmbDateType', 2, true);                                             // Rows[2] = This Week: From = today - 7 and Show
        el('GRNfromdate').value = A.addDays(A.today(), -15);                            // GRNfromdate = Now.AddDays(-15)
        S.activate('CmbActivity', 1, true);                                             // Rows[1] = Sale
        S.focus('cmbDateType');
        return show();                                                                  // gridHisory
    }).catch(function (e) { alert(e.message); });
}(window, document));
