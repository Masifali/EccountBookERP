/* ============================================================================================
 * Screen 557 SaleInvoiceRegisterSteel - Architecture.WinApp.Steel.Reports.SalesReports.SaleInvoiceRegisterSteel (.cs)
 * Page: templates/sale/steel/rpt_sale_invoice_register_st.html (route /sale/reports/steel/sale-invoice-register).
 *   frmSaleInvoiceRegister_Load :147   GetDataForSaleInvoiceRegisterDropdownBind, FromDate = Start_Period, ReportsLoad, gridHisory, DirectRegisterHistory
 *   ReportsLoad :164                   CommonServices.DynamicReportsLoad("Sales_Steel") -> "Print For Flow" items, ("SalesDirect_Steel ") -> "Print For Direct " items
 *   GetDataForSaleInvoiceRegisterDropdownBind :191   InvSaleInvoice.GetDataForDropDownFromSaleInvoiceForSteel ([ST].[USP_GetDataForDropDownFromSaleInvoice] Org, Company):
 *                                      Activity Customer -> CmbSupplier, Warehouse -> cmbWareHouse, JobLot -> CmbJobLot, Item -> cmbitem (ZeroIndex false)
 *   gridHisory :254 / GridSettings :353   InvSaleInvoice.SaleInvoiceRegister = [ST].[USP_SaleInvoiceRegister]; 52 column table; Id, VoucherHeadId, InvoiceDetailId, DocumentTypeId, OrderId,
 *                                      GpId, GrnId, SupplierCustomerId, SaleGLAC, ItemId hidden; CustomerName is a link; amounts stringFormatsingle (summed), weights "#,##0.###" (summed),
 *                                      rates DecimalRateFormate; Print (50) and Voucher (60) button columns, FrozenColumns 2
 *   DirectRegisterHistory :461 / GridDirectSettings :552   InvSaleInvoice.SaleInvoiceDirectRegister = [ST].[USP_SaleInvoiceRegisterDirect]; 43 column table, same settings
 *   btnshow_Click :675                 gridHisory, DirectRegisterHistory.   btnNew :711 (doc numbers, rates, customer / warehouse / item text cleared, From = Start_Period, To = now)
 *   btnRefresh :917                    GetDataForSaleInvoiceRegisterDropdownBind
 *   tsDropDown :688 / tsDropDownForDirect :894   "Record Not Found For Display" else <item text>.rpt over dtInvoice / dtInvoiceDirect
 *   DataGridHistory_ColumnButtonClick :733   Print -> SaleInvoiceSteelSlip_1514 (1514-InvRepSaleBillCustomer.rpt + sub report), Voucher -> AcRptPurchaseSalesVoucherSlip_103
 *   DataGridHistoryDirect_ColumnButtonClick :845   Print -> SaleInvoiceDirectSteelSlip_1516 (1516-SaleInvoiceCustomerBill.rpt), Voucher as above
 *   LinkClicked :752 / :869            CustomerName -> GoToCustomerLedgerFromLinkedEvent (customer ledger for From / To)
 *   KeyPress: invoice and sale order numbers digits only, rates digits and a point
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/steel-reports/sale-invoice-register';
    var PRINT = '/sale/reports/steel/print/';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null };
    var st = { flow: { rows: 0, args: null, seq: 0, reports: [] }, direct: { rows: 0, args: null, seq: 0, reports: [] } };

    function mkGrid(n) { return new S.Grid({ tableId: 'results' + n, gridId: 'grid' + n, navId: 'nav' + n, navTextId: 'navText' + n, headerLines: 2, autosize: true, frozen: 2,
        onLink: function (row, col) { onLink(row, col); }, onButton: function (row, col) { onButton(n === '1' ? 'flow' : 'direct', row, col); } }); }
    var grids = { flow: mkGrid('1'), direct: mkGrid('2') };

    // ------------------------------------------------------------------ columns (the dtGrid tables and GridSettings)
    function cols(direct) {
        var D = 'dd-MMM-yy', q3 = '#,##0.###', single = S.fmtSingle(lookup.amountDecimals), rate = S.fmtRate(lookup.rateDecimals);
        function amt(k) { return { key: k, caption: k, fmt: single, sum: true, num: true }; }
        function wt(k) { return { key: k, caption: k, fmt: q3, sum: true, num: true }; }
        function rt(k) { return { key: k, caption: k, fmt: rate, num: true }; }
        var c = [{ key: '_Print', caption: 'Print', width: 50, button: 'Print' }, { key: '_Voucher', caption: 'Voucher', width: 60, button: 'Voucher' },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'InvoiceDetailId', caption: 'InvoiceDetailId', hidden: true }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'VoucherHeadId', caption: 'VoucherHeadId', hidden: true }, { key: 'DocDate', caption: 'DocDate', date: D }, { key: 'DocNo', caption: 'DocNo', align: 'r' },
            { key: 'InvoiceType', caption: 'InvoiceType' }];
        if (!direct) c.push({ key: 'OrderId', caption: 'OrderId', hidden: true }, { key: 'OrderDate', caption: 'OrderDate', date: D }, { key: 'OrderNo', caption: 'OrderNo', align: 'r' },
            { key: 'GpId', caption: 'GpId', hidden: true });
        c.push({ key: 'GpDate', caption: 'GpDate', date: D }, { key: 'GpSrNo', caption: 'GpSrNo', align: 'r' });
        if (!direct) c.push({ key: 'GrnId', caption: 'GrnId', hidden: true }, { key: 'GrnDate', caption: 'GrnDate', date: D }, { key: 'GrnNo', caption: 'GrnNo', align: 'r' });
        c.push({ key: 'VehicleNo', caption: 'VehicleNo' });
        if (!direct) c.push({ key: 'BiltyNo', caption: 'BiltyNo' });
        c.push({ key: 'SupplierCustomerId', caption: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'CustomerName', link: true }, { key: 'CommAgentName', caption: 'CommAgentName' },
            { key: 'CommType', caption: 'CommType' }, rt('CommRate'), amt('CommAmount'), amt('PartyBillAmount'), { key: 'DueDate', caption: 'DueDate', date: D },
            { key: 'ManualBillNo', caption: 'ManualBillNo' }, { key: 'SaleGLAC', caption: 'SaleGLAC', hidden: true }, { key: 'ItemId', caption: 'ItemId', hidden: true },
            { key: 'ItemName', caption: 'ItemName' }, { key: 'WareHouseCode', caption: 'WareHouseCode' }, { key: 'JobLotCode', caption: 'JobLotCode' }, { key: 'PackTypeCode', caption: 'PackTypeCode' },
            { key: 'UOMCode', caption: 'UOMCode' }, wt('ItemQty'), wt('GrossWeight'), wt('WtCutPerUnit'), wt('WtCutTotal'), wt('AdLsWeight'), wt('NetBillWeight'), wt('NetStockWeight'),
            rt('ItemRate'), rt('NetRate'), { key: 'RateUom', caption: 'RateUom' }, rt('RateCut'), amt('RateCutAmount'), amt('ItemAmount'), amt('FreightAmount'), amt('ExpenseAmount'),
            amt('CommissionAmount'), amt('ItemNetAmount'), { key: 'EntryDate', caption: 'EntryDate', date: direct ? D : 'dd-MMM-yy hh:mm tt' }, { key: 'EntryUser', caption: 'EntryUser' });
        return c;
    }

    // ------------------------------------------------------------------ lists
    function fillLists(data) {
        S.fill(el('CmbSupplier'), data.customers, false);
        S.fill(el('cmbWareHouse'), data.warehouses, false);
        S.fill(el('CmbJobLot'), data.jobLots, false);
        S.fill(el('cmbitem'), data.items, false);
    }
    function getDataForDropdownBind() { return A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; fillLists(lookup); return lookup; }); }
    function loadReports() {
        return Promise.all([['flow', 'Sales_Steel'], ['direct', 'SalesDirect_Steel']].map(function (x) {
            return A.getJson('/api/sale/steel-reports/dynamic-reports?' + S.qs({ folder: x[1] })).then(function (l) { st[x[0]].reports = l || []; }).catch(function () { st[x[0]].reports = []; });
        }));
    }

    // ------------------------------------------------------------------ Show
    function args(mode) {
        return { mode: mode, fromDate: el('FromDate').value, toDate: el('ToDate').value, fromDocNo: A.toIntText(el('txtInvoiceNoFrom').value.trim()), toDocNo: A.toIntText(el('txtInvoiceNoto').value.trim()),
                 supplierCustomerId: S.selInt('CmbSupplier'), itemId: S.selInt('cmbitem'), warehouseId: S.selInt('cmbWareHouse'), jobLotId: S.selInt('CmbJobLot'),
                 rateFrom: A.toDouble(el('txtratefrom').value.trim()), rateTo: A.toDouble(el('txtrateto').value.trim()) };
    }
    function fill(mode) {
        var s = st[mode], a = args(mode), token = ++s.seq;
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== s.seq) return;
            s.args = a; s.rows = rows ? rows.length : 0;
            if (!s.rows) { grids[mode].clear(); return; }
            grids[mode].setData(cols(mode === 'direct'), rows);
        });
    }
    function gridHisory() { return fill('flow'); }
    function directRegisterHistory() { return fill('direct'); }
    function showAll() {
        var b = el('btnshow'); if (b.disabled) return Promise.resolve();
        A.busy(b, true);
        return gridHisory().then(directRegisterHistory).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ buttons / links
    function onButton(mode, row, col) {
        if (!row || !col) return;
        var id = A.toInt(A.ci(row, 'Id'));
        if (col.button === 'Print') {
            A.openPdf(PRINT + 'sale-invoice-slip?' + S.qs({ id: id, mode: mode })).catch(function (e) { alert(e.message); });
        } else if (col.button === 'Voucher') {                         // AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId)
            var vh = A.toInt(A.ci(row, 'VoucherHeadId')), dtId = A.toInt(A.ci(row, 'DocumentTypeId'));
            if (vh === 0) { alert('No Record Found For Display'); return; }
            if (vh < 0) { alert('Record Not Found For Display because VoucherHeadId not found'); return; }
            A.openPdf('/reports/print/103-purchase-sales-voucher-slip?' + S.qs({ id: vh, documentTypeId: dtId })).catch(function (e) { alert(e.message); });
        }
    }
    function onLink(row, col) {
        if (!row || !col || col.key !== 'CustomerName') return;       // GoToCustomerLedgerFromLinkedEvent(SupplierCustomerId, FromDate, ToDate)
        w.open('/accounts/reports/customer-ledger?' + S.qs({ supplierCustomerId: A.toInt(A.ci(row, 'SupplierCustomerId')), fromDate: el('FromDate').value, toDate: el('ToDate').value }), '_blank');
    }
    function printWith(mode, name) {                                    // tsDropDown_DropDownItemClicked / tsDropDownForDirect_DropDownItemClicked
        var s = st[mode];
        if (!s.rows || !s.args) { alert('Record Not Found For Display'); return; }
        A.openPdf(PRINT + 'sale-invoice-register?' + S.qs(Object.assign({ template: name }, s.args))).catch(function (e) { alert(e.message); });
    }
    S.dropdown('printFlow', 'printFlowMenu', function () { return st.flow.reports; }, function (n) { printWith('flow', n); });
    S.dropdown('printDirect', 'printDirectMenu', function () { return st.direct.reports; }, function (n) { printWith('direct', n); });

    function blank(id) { var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true })); }
    function reset() {                                                  // btnNew_Click
        ['txtInvoiceNoFrom', 'txtInvoiceNoto', 'txtratefrom', 'txtrateto', 'txtsaleorderfrom', 'txtsaleorderto'].forEach(function (id) { el(id).value = ''; });
        blank('CmbSupplier'); blank('cmbWareHouse'); blank('cmbitem');
        el('FromDate').value = lookup.yearStart ? S.isoDay(lookup.yearStart) : A.today();
        el('ToDate').value = A.today();
    }
    function refresh() {                                                // btnRefresh_Click
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        getDataForDropdownBind().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ tabs
    function selectTab(n) {
        [1, 2].forEach(function (i) { el('page' + i).hidden = i !== n; el('tab' + i).classList.toggle('on', i === n); });
    }
    el('tab1').addEventListener('click', function () { selectTab(1); });
    el('tab2').addEventListener('click', function () { selectTab(2); });
    selectTab(1);

    el('btnshow').addEventListener('click', showAll);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    ['txtInvoiceNoFrom', 'txtInvoiceNoto', 'txtsaleorderfrom', 'txtsaleorderto'].forEach(function (id) { S.digitsOnly(id); });
    ['txtratefrom', 'txtrateto'].forEach(function (id) { S.digitsOnly(id, true); });

    // ------------------------------------------------------------------ start (Load)
    el('FromDate').value = A.today(); el('ToDate').value = A.today();
    grids.flow.render(); grids.direct.render();
    getDataForDropdownBind().then(function () {
        if (lookup.yearStart) el('FromDate').value = S.isoDay(lookup.yearStart);        // FromDate = ActiveYr.Start_Period
        S.focus('FromDate');
        return loadReports();                                                           // ReportsLoad
    }).then(showAll).catch(function (e) { alert(e.message); });
}(window, document));
