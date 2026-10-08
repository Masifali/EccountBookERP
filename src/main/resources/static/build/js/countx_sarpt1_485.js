/* ============================================================================================
 * Screen 485 frmSaleInvoiceHistory - Architecture.WinApp.Inventory_Reports.frmSaleInvoiceHistory (.cs, 1000+ lines)
 * Page: templates/sale/reports/sarpt1_sale_invoice_history.html (route /sale/reports/sale-invoice-history).
 *   frmSaleInvoiceRegister_Load :138  ComboFill, BranchesFill, ReportsLoad, gridHisory (first Show), Datetypefill, focus the date type
 *   ComboFill :155        Usp_AllComboAgainstSaleInvoice (no DocumentTypeIds) [@BranchesIds ",id,id"]: JobLot -> CmbJobLot, Supplier -> CmbSupplier ("Customer Name"),
 *                         WareHouse -> cmbWareHouse, ItemName -> cmbitem; all ZeroIndex true, Rows[0] activated
 *   BranchesFill :235     GetBranchsAllocatedToUserFromSaleInvoice (document type 95), BindDDL(BranchId, BranchName, ZeroIndex false), checked list,
 *                         LimitToList false, text = the user's branch
 *   Datetypefill :264     DateType, BindDDL(ZeroIndex true), Rows[2].Activate() (This Week)
 *   cmbDateType_ValueChanged :935  1 From = today | 2 today - 7 | 3 first of (UTC) month, To = today | 4 1 Jan, To = today | 5 Start_Period; each then Show
 *   gridHisory :279 / btnshow_Click :391  Sp_InvSaleInvoice_Rpt; no branch -> "Select branch first"; 49 column table (CommissionAgent <- CompanyNamecmagnt,
 *                         WareHouse <- WareHouseName, PackingType <- PackTypeDesc, JobLot <- JobLotDescription)
 *   GridSettings :403     Print (50, pos 0) and Voucher (50, pos 1) button columns, FrozenColumns 2, HeaderLines 2, links on InvoiceNo / CustomerName / ItemName,
 *                         widths from InventoryConstants, "#,##0.###" / "#,#" formats and sums
 *   DataGridHistory_ColumnButtonClick :753  Print -> SaleInvoicetSlip_301 (the 301 slip), Voucher -> VoucherHeadIdGet then AcRptPurchaseSalesVoucherSlip_103
 *   DataGridHistory_LinkClicked :786 / _KeyDown :812 (Ctrl+Space)  InvoiceNo -> SaleInvoiceReverseActivity (web: the sale invoice form, DocLink 95),
 *                         CustomerName -> GoToCustomerLedgerFromLinkedEvent (general ledger), ItemName -> GoToItemEvaluationLedgerFromLinkedEvent (no web page)
 *   ReportsLoad :560 / tsDropDown_DropDownItemClicked :597  the "Sales" folder .rpt files; the clicked one prints the last Show's table
 *   btnNew :620 (clears the text boxes and customer / warehouse / item, Datetypefill), btnRefresh :642 (ComboFill), cmbBranchName_Leave :992, btnshow_Leave :977
 *   KeyDown :678, MakeShortCutKeys :726, txt*_KeyPress (digits)
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/sarpt1/sale-invoice-history';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null, userBranchId: null };
    var lastArgs = null, hasRows = false, seq = 0, reports = [];
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl + P', 'For print'],
        ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];

    // ------------------------------------------------------------------ lists
    function datetypefill() { S.fill(el('cmbDateType'), S.dateTypeRows(false), true); S.activate('cmbDateType', 2, true); }
    function branchesFill(rows) { S.fillBranches(el('cmbBranchName'), rows, lookup.userBranchId); }
    function bindCombos(data) {
        function rows(k) { return (data && data[k]) || []; }
        S.fill(el('CmbJobLot'), rows('JobLot'), true);
        S.fill(el('cmbitem'), rows('ItemName'), true);
        S.fill(el('cmbWareHouse'), rows('WareHouse'), true);
        S.fill(el('CmbSupplier'), rows('Supplier'), true);
    }
    function comboFill() {
        return A.getJson(API + '/combos?' + S.qs({ branchesIds: S.branchIds(el('cmbBranchName')) })).then(bindCombos);
    }
    el('cmbDateType').addEventListener('change', function () {              // cmbDateType_ValueChanged
        var id = A.toInt(this.value);
        if (id < 1 || id > 5) return;
        S.dateRule(id, el('GRNfromdate'), el('GRNToDate'), lookup.yearStart);
        show();
    });

    // ------------------------------------------------------------------ grid
    var grid = new S.Grid({
        tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, frozen: 2,
        onLink: function (row, col) { link(row, col ? col.key : ''); },
        onButton: function (row, col) { button(row, col.button); }
    });
    function cols() {
        var t3 = '#,##0.###', t1 = '#,#';
        return [
            { key: '_Print', caption: 'Print', width: 50, button: 'Print' }, { key: '_Voucher', caption: 'Voucher', width: 50, button: 'Voucher' },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'SuppCustId', caption: 'SuppCustId', hidden: true }, { key: 'OrderItemId', caption: 'OrderItemId', hidden: true },
            { key: 'SaleGLAC', caption: 'SaleGLAC', hidden: true },
            { key: 'BranchName', caption: 'BranchName', width: 120 }, { key: 'InvoiceNo', caption: 'InvoiceNo', width: 60, link: true },
            { key: 'InvoiceDate', caption: 'InvoiceDate', width: 73, date: 'dd-MMM-yy' }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'CustomerName', caption: 'CustomerName', width: 170, link: true }, { key: 'DueDays', caption: 'DueDays', width: 60 },
            { key: 'DueDate', caption: 'DueDate', width: 73, date: 'dd-MMM-yy' }, { key: 'ManualBillNo', caption: 'ManualBillNo', width: 70 },
            { key: 'CommissionAgent', caption: 'CommissionAgent', width: 150 }, { key: 'SaleOrderNo', caption: 'SaleOrderNo', width: 80 },
            { key: 'OrderDate', caption: 'OrderDate', width: 73, date: 'dd-MMM-yy' }, { key: 'DoNO', caption: 'DoNO', width: 50 },
            { key: 'WareHouse', caption: 'WareHouse', width: 150 }, { key: 'ItemName', caption: 'ItemName', width: 150, link: true },
            { key: 'PackingType', caption: 'PackingType', width: 80 }, { key: 'CropYear', caption: 'CropYear', width: 73 }, { key: 'JobLot', caption: 'JobLot', width: 80 },
            { key: 'ItemQty', caption: 'ItemQty', width: 70, fmt: t1, sum: true, num: true }, { key: 'UOMCodeItem', caption: 'Pack Uom', width: 60 },
            { key: 'GrossWeight', caption: 'GrossWeight', width: 80, fmt: t1, sum: true, num: true },
            { key: 'EBWPerUnit', caption: 'EBWPerUnit', width: 80, fmt: t3, num: true }, { key: 'EBWTotal', caption: 'EBWTotal', width: 80, fmt: t1, sum: true, num: true },
            { key: 'AdLsWeight', caption: 'AdLsWeight', width: 80, fmt: t1, sum: true, num: true }, { key: 'StockWeight', caption: 'StockWeight', width: 80, fmt: t3, sum: true, num: true },
            { key: 'NetBillWeight', caption: 'NetBillWeight', width: 80, fmt: t1, sum: true, num: true },
            { key: 'OrderItemRate', caption: 'OrderItemRate', width: 70, fmt: t3, num: true },
            { key: 'RateCutAmount', caption: 'RateCutAmount', width: 70, fmt: t3, sum: true, num: true }, { key: 'NetRate', caption: 'NetRate', width: 70, fmt: t3, num: true },
            { key: 'RateUOM', caption: 'RateUOM', width: 60 }, { key: 'ItemAmount', caption: 'ItemAmount', width: 90, fmt: t1, sum: true, num: true },
            { key: 'FreightAmount', caption: 'FreightAmount', width: 90, fmt: t3, sum: true, num: true },
            { key: 'ExpenseAmount', caption: 'ExpenseAmount', width: 90, fmt: t1, sum: true, num: true },
            { key: 'JournalAmount', caption: 'JournalAmount', width: 90, fmt: t1, sum: true, num: true },
            { key: 'CommissionAmount', caption: 'CommissionAmount', width: 90, fmt: t1, sum: true, num: true },
            { key: 'GpSrNo', caption: 'GpSrNo', width: 45 }, { key: 'GpDate', caption: 'GpDate', width: 73, date: 'dd-MMM-yy' }, { key: 'VehicleNo', caption: 'VehicleNo', width: 80 },
            { key: 'ItemNetAmount', caption: 'ItemNetAmount', width: 110, fmt: t1, sum: true, num: true }, { key: 'PartyBillAmount', caption: 'PartyBillAmount', width: 110, fmt: t1, num: true },
            { key: 'GdnDate', caption: 'GdnDate', width: 73, date: 'dd-MMM-yy' }, { key: 'GdnNo', caption: 'GdnNo', width: 60 },
            { key: 'WtCut', caption: 'WtCut', width: 60, fmt: t3, num: true }, { key: 'WtCutTotal', caption: 'WtCutTotal', width: 70, fmt: t3, sum: true, num: true },
            { key: 'RateCut', caption: 'RateCut', width: 70, fmt: t3, num: true }, { key: 'TicketNos', caption: 'TicketNos', width: 100 }
        ];
    }
    function mapRow(r) {
        var c = A.ci;
        return { Id: c(r, 'Id'), SuppCustId: c(r, 'SuppCustId'), OrderItemId: c(r, 'OrderItemId'), SaleGLAC: c(r, 'SaleGLAC'), BranchName: c(r, 'BranchName'),
            InvoiceNo: c(r, 'InvoiceNo'), InvoiceDate: c(r, 'InvoiceDate'), DocumentTypeId: c(r, 'DocumentTypeId'), CustomerName: c(r, 'CustomerName'),
            DueDays: c(r, 'DueDays'), DueDate: c(r, 'DueDate'), ManualBillNo: c(r, 'ManualBillNo'), CommissionAgent: c(r, 'CompanyNamecmagnt'), SaleOrderNo: c(r, 'SaleOrderNo'),
            OrderDate: c(r, 'OrderDate'), DoNO: c(r, 'DoNO'), WareHouse: c(r, 'WareHouseName'), ItemName: c(r, 'ItemName'), PackingType: c(r, 'PackTypeDesc'),
            CropYear: c(r, 'CropYear'), JobLot: c(r, 'JobLotDescription'), ItemQty: c(r, 'ItemQty'), UOMCodeItem: c(r, 'UOMCodeItem'), GrossWeight: c(r, 'GrossWeight'),
            EBWPerUnit: c(r, 'EBWPerUnit'), EBWTotal: c(r, 'EBWTotal'), AdLsWeight: c(r, 'AdLsWeight'), StockWeight: c(r, 'StockWeight'), NetBillWeight: c(r, 'NetBillWeight'),
            OrderItemRate: c(r, 'OrderItemRate'), RateCutAmount: c(r, 'RateCutAmount'), NetRate: c(r, 'NetRate'), RateUOM: c(r, 'RateUOM'), ItemAmount: c(r, 'ItemAmount'),
            FreightAmount: c(r, 'FreightAmount'), ExpenseAmount: c(r, 'ExpenseAmount'), JournalAmount: c(r, 'JournalAmount'), CommissionAmount: c(r, 'CommissionAmount'),
            GpSrNo: c(r, 'GpSrNo'), GpDate: c(r, 'GpDate'), VehicleNo: c(r, 'VehicleNo'), ItemNetAmount: c(r, 'ItemNetAmount'), PartyBillAmount: c(r, 'PartyBillAmount'),
            GdnDate: c(r, 'GdnDate'), GdnNo: c(r, 'GdnNo'), WtCut: c(r, 'WtCut'), WtCutTotal: c(r, 'WtCutTotal'), RateCut: c(r, 'RateCut'), TicketNos: c(r, 'TicketNos') };
    }

    // ------------------------------------------------------------------ buttons / links
    function button(r, which) {
        var id = A.toInt(r.Id), docType = A.toInt(r.DocumentTypeId);
        if (which === 'Print') {                                             // SaleInvoicetSlip_301
            A.openPdf('/reports/print/301-inv-rep-sale-bill-customer?id=' + id).catch(function (e) { alert(e.message); });
        } else if (which === 'Voucher') {                                    // VoucherHeadIdGet, then the 103 slip when it is > 0
            A.getJson(API + '/voucher-head-id?' + S.qs({ id: id, documentTypeId: docType })).then(function (o) {
                var vh = A.toInt(o && o.voucherHeadId);
                if (vh > 0) return A.openPdf('/reports/print/103-purchase-sales-voucher-slip?' + S.qs({ id: vh, documentTypeId: docType }));
            }).catch(function (e) { alert(e.message); });
        }
    }
    function link(r, key) {
        if (key === 'InvoiceNo') {
            if (w.DocLink) w.DocLink.open(95, A.toInt(r.Id)); else alert('The sale invoice page is not available.');
        } else if (key === 'CustomerName') {                                 // GoToCustomerLedgerFromLinkedEvent(SuppCustId, from, to)
            A.getJson(API + '/gl-account?' + S.qs({ supplierCustomerId: A.toInt(r.SuppCustId) })).then(function (o) {
                var acc = A.toInt(o && o.glAccountId);
                if (acc <= 0) { alert('No ledger account is linked to this customer.'); return; }
                w.open('/accounts/reports/general-ledger?' + S.qs({ accountId: acc, fromDate: el('GRNfromdate').value, toDate: el('GRNToDate').value }), '_blank');
            }).catch(function (e) { alert(e.message); });
        } else if (key === 'ItemName') {                                     // GoToItemEvaluationLedgerFromLinkedEvent
            alert('The Item Evaluation Ledger does not have a web page yet.');
        }
    }

    // ------------------------------------------------------------------ Show / print
    function show() {
        var b = el('show'); if (b.disabled) return;
        var ids = S.branchIds(el('cmbBranchName'));
        if (!ids) { alert('Select branch first'); S.focus('cmbBranchName'); return; }
        var a = { fromDate: el('GRNfromdate').value, toDate: el('GRNToDate').value, fromDocNo: A.toIntText(el('txtInvoiceNoFrom').value.trim()),
                  toDocNo: A.toIntText(el('txtInvoiceNoto').value.trim()), supplierCustomerId: S.selInt('CmbSupplier'), itemId: S.selInt('cmbitem'),
                  saleOrderFrom: A.toIntText(el('txtsaleorderfrom').value.trim()), saleOrderTo: A.toIntText(el('txtsaleorderto').value.trim()),
                  rateFrom: A.toDouble(el('txtratefrom').value.trim()), rateTo: A.toDouble(el('txtrateto').value.trim()),
                  warehouseId: S.selInt('cmbWareHouse'), jobLotId: S.selInt('CmbJobLot'), branchesIds: ids };
        var token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.clear(); return; }
            grid.setData(cols(), rows.map(mapRow));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function printWith(name) {
        if (!hasRows || !lastArgs) { alert('Record Not Found For Display'); return; }
        var a = Object.assign({}, lastArgs, { template: name });
        A.openPdf('/sale/reports/sarpt1/print/sale-invoice-history?' + S.qs(a)).catch(function (e) { alert(e.message); });
    }
    function loadReports() {
        return A.getJson('/api/sale/sarpt1/dynamic-reports?folder=Sales').then(function (l) { reports = l || []; });
    }
    var dd = S.dropdown('print', 'printMenu', function () { return reports; }, printWith);

    function clearTexts() {
        ['txtInvoiceNoFrom', 'txtInvoiceNoto', 'txtratefrom', 'txtrateto', 'txtsaleorderfrom', 'txtsaleorderto'].forEach(function (id) { el(id).value = ''; });
        ['CmbSupplier', 'cmbWareHouse', 'cmbitem'].forEach(function (id) { var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true })); });
    }
    function reset() { clearTexts(); datetypefill(); S.focus('cmbDateType'); }
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        comboFill().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    ['txtInvoiceNoFrom', 'txtInvoiceNoto', 'txtsaleorderfrom', 'txtsaleorderto'].forEach(function (id) { S.digitsOnly(id, false); });
    ['txtratefrom', 'txtrateto'].forEach(function (id) { S.digitsOnly(id, true); });
    S.onLeave('cmbBranchName', function () {                                // cmbBranchName_Leave
        if (S.branchIds(el('cmbBranchName'))) comboFill().catch(function (e) { alert(e.message); });
        else ['CmbJobLot', 'CmbSupplier', 'cmbitem', 'cmbWareHouse'].forEach(function (id) { S.fill(el(id), [], true); el(id).value = ''; el(id).dispatchEvent(new Event('change', { bubbles: true })); });
    });
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && el('grid').contains(e.relatedTarget)) S.focus('cmbDateType'); });
    S.keys({ s: show, e: closeForm, n: reset, r: refresh, p: function () { el('print').click(); }, F5: function () { S.focus('cmbDateType'); },
             ArrowUp: function () { S.focus('cmbDateType'); }, ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('GRNfromdate').value = A.today(); el('GRNToDate').value = A.today();
    grid.render();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        return A.getJson(API + '/combos?' + S.qs({ branchesIds: '' }));                 // ComboFill runs before BranchesFill: no branch filter yet
    }).then(function (c) {
        bindCombos(c);
        branchesFill(lookup.branches);
        return loadReports().catch(function () { reports = []; });                      // ReportsLoad
    }).then(function () { return show(); })                                             // gridHisory
      .then(function () { datetypefill(); S.focus('cmbDateType'); })                    // Datetypefill: Rows[2] fires the rule and a second Show
      .catch(function (e) { alert(e.message); });
}(window, document));
