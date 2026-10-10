/* ============================================================================================
 * Screen 586 SaleInvoice_Register - Architecture.WinApp.Salt.Reports.SaleInvoice_Register (.cs), tabs "Detail Register" / "Summary Register"
 * Page: templates/sale/bk/sale_register_salt.html (route /sale/salt/salt-sale-invoice-register).
 *   frmSaleInvoiceRegister_Load :274    ReportsLoad ("Sales_Salt" folder), Datetypefill (both date types, row 2 = This Month, fires their ValueChanged), AllComboBind, gridHisory,
 *                                       AllComboBindSummary, ActivityFill, ItemUOMFill, activity row 0, PrintButtonManage
 *   cmbDateType_ValueChanged :1318      1 From = today | 2 today - 7 | 3 first of the (UTC) month, To = today | 4 1 Jan, To = today | 5 Start_Period; each then Show
 *   cmbDateTypeSummary_ValueChanged :1364   type 1 sets From and To to Now; types 2 - 5 test the DETAIL tab's cmbDateType.Value (desktop quirk, kept)
 *   btnshow_Click :819 / gridHisory :732    USP_InvSaleInvoice_RegisterSalt; 41 column grid; none -> ClearStructure
 *   GridSettings :831                   links on InvoiceNo / CustomerName / ItemName, formats and sums, Print (40) + Voucher (60) first, FrozenColumns 2
 *   DataGridHistory_ColumnButtonClick :1151   Print -> SaleInvoicetSlip_301, Voucher -> AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId)
 *   DataGridHistory_LinkClicked :1174   InvoiceNo -> SaleInvoiceReverseActivity, CustomerName -> GoToCustomerLedgerFromLinkedEvent, ItemName -> GoToItemEvaluationLedgerFromLinkedEvent
 *   tsDropDown_DropDownItemClicked :962 the clicked Sales_Salt file prints the last Show's table ("Record Not Found For Display" when empty)
 *   btnNew_Click :985 / btnRefresh_Click :1007 / btnNewSummary_Click :1693 / btnRefreshSummary_Click :1719
 *   btnShowSumamry_Click :1402 / SummaryGridFill :1416 / SummaryGridSetting :1631 / btnPrint_Click :1731 / PrintButtonManage :432
 *   frmSaleInvoiceHistory_KeyDown :1043 per tab: Ctrl+N / R / S / F5 / Down / Up / Alt (+ Ctrl+P on the summary tab); Ctrl+E and Esc close
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/bk/sale-register';
    var info = { amountDecimals: 0, rateDecimals: 0, yearStart: null, uom: [] };
    var T3 = '#,##0.###', T4 = '#,##0.####', DATE = 'dd/MM/yyyy';
    var last = { args: null, rows: 0 }, sumLast = { args: null, rows: 0 }, reports = [], tab = 0;
    var times = { sumFrom: '00:00:00', sumTo: '00:00:00' };

    function two(n) { return ('0' + n).slice(-2); }
    function nowTime() { var u = new Date(); return two(u.getHours()) + ':' + two(u.getMinutes()) + ':' + two(u.getSeconds()); }

    // ------------------------------------------------------------------ the detail grid
    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, autosize: true, frozen: 2,
        onLink: function (row, col) { link(row, col ? col.key : ''); },
        onButton: function (row, col) { button(row, col.button); } });
    function hid(k) { return { key: k, caption: k, hidden: true }; }
    function t(k) { return { key: k, caption: k }; }
    function num(k, fmt, sum) { var c = { key: k, caption: k, fmt: fmt, num: true }; if (sum) c.sum = true; return c; }
    function cols() {
        var single = S.fmtSingle(info.amountDecimals), rate = S.fmtRate(info.rateDecimals);
        return [{ key: '_Print', caption: 'Print', width: 40, button: 'Print' }, { key: '_Voucher', caption: 'Voucher', width: 60, button: 'Voucher' },
            hid('Id'), hid('DocumentTypeId'), hid('VoucherHeadId'), { key: 'InvoiceNo', caption: 'InvoiceNo', link: true }, { key: 'InvoiceDate', caption: 'InvoiceDate', date: DATE },
            hid('SuppCustId'), { key: 'CustomerName', caption: 'CustomerName', link: true }, t('PaymentTerm'), t('DueDays'), { key: 'DueDate', caption: 'DueDate', date: DATE },
            t('ManualBillNo'), t('CommissionAgent'), t('OrderNo'), { key: 'OrderDate', caption: 'OrderDate', date: DATE }, t('WareHouse'), hid('ItemId'), hid('SaleGLAC'),
            { key: 'ItemName', caption: 'ItemName', link: true }, t('JobLot'), t('PackingType'), t('PackUom'),
            num('ItemQty', T3, true), num('GrossWeight', T4, true), num('AdLsWeight', T4, true), num('StockWeight', T4, true), num('NetBillWeight', T4, true),
            num('ItemRate', rate, false), t('RateUOM'), num('RateCut', rate, false), num('RateCutAmount', single, true), num('NetRate', rate, false), num('ItemAmount', single, true),
            num('FreightAmount', T4, true), num('ExpenseAmount', T4, true), num('JournalAmount', T4, true), num('CommissionAmount', T4, true),
            t('GpNo'), { key: 'GpDate', caption: 'GpDate', date: DATE }, t('VehicleNo'), num('ItemNetAmount', single, true), num('PartyBillAmount', T4, false)];
    }
    function histArgs() {
        return { fromDate: el('GRNfromdate').value, toDate: el('GRNToDate').value, fromTime: '00:00:00', toTime: '00:00:00',
                 fromDocNo: A.toIntText(el('txtInvoiceNoFrom').value.trim()), toDocNo: A.toIntText(el('txtInvoiceNoto').value.trim()),
                 supplierCustomerId: S.selInt('CmbSupplier'), itemId: S.selInt('cmbitem'),
                 saleOrderFrom: A.toIntText(el('txtsaleorderfrom').value.trim()), saleOrderTo: A.toIntText(el('txtsaleorderto').value.trim()),
                 rateFrom: A.toDouble(el('txtratefrom').value.trim()), rateTo: A.toDouble(el('txtrateto').value.trim()),
                 warehouseId: S.selInt('cmbWareHouse'), jobLotId: S.selInt('CmbJobLot') };
    }
    var seq = 0;
    function gridHistory() {
        var a = histArgs(), token = ++seq, b = el('btnshow');
        last = { args: a, rows: 0 };
        A.busy(b, true);
        return A.getJson(API + '/history?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            last.rows = rows ? rows.length : 0;
            if (last.rows > 0) grid.setData(cols(), rows); else grid.clear();
        }).catch(function (e) { w.alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }

    function button(r, which) {
        var id = A.toInt(A.ci(r, 'Id'));
        if (which === 'Print') {                                          // SaleInvoicetSlip_301(Id, SupCustId, 0, Date, Date)
            A.openPdf('/reports/print/301-inv-rep-sale-bill-customer?id=' + id).catch(function (e) { w.alert(e.message); });
        } else if (which === 'Voucher') {                                 // AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, DocumentTypeId)
            var vh = A.toInt(A.ci(r, 'VoucherHeadId'));
            if (vh === 0) { w.alert('No Record Found For Display'); return; }
            A.openPdf('/reports/print/103-purchase-sales-voucher-slip?' + S.qs({ id: vh, documentTypeId: A.toInt(A.ci(r, 'DocumentTypeId')) })).catch(function (e) { w.alert(e.message); });
        }
    }
    function link(r, key) {
        if (key === 'InvoiceNo') {                                        // new SaleInvoiceReverseActivity(UserAccount, id).Show()
            if (w.DocLink) w.DocLink.open(95, A.toInt(A.ci(r, 'Id'))); else w.alert('The sale invoice page is not available.');
        } else if (key === 'CustomerName') {                              // GoToCustomerLedgerFromLinkedEvent(SuppCustId, from, to)
            A.getJson('/api/sale/sarpt1/sale-invoice-history/gl-account?' + S.qs({ supplierCustomerId: A.toInt(A.ci(r, 'SuppCustId')) })).then(function (o) {
                var acc = A.toInt(o && o.glAccountId);
                if (acc <= 0) { w.alert('No ledger account is linked to this customer.'); return; }
                w.open('/accounts/reports/general-ledger?' + S.qs({ accountId: acc, fromDate: el('GRNfromdate').value, toDate: el('GRNToDate').value }), '_blank');
            }).catch(function (e) { w.alert(e.message); });
        } else if (key === 'ItemName') {                                  // GoToItemEvaluationLedgerFromLinkedEvent
            w.alert('The Item Evaluation Ledger does not have a web page yet.');
        }
    }

    // ------------------------------------------------------------------ the summary grid
    var sgrid = new S.Grid({ tableId: 'sumResults', gridId: 'sumGrid', navId: 'sumNav', navTextId: 'sumNavText', headerLines: 2, autosize: true });
    var CITY_HIDDEN = ['Sales Summary By Item & Pack Size', 'Sales Summary By Item', 'Sales Summary By Item & Warehouse', 'Sales Summary By Customer', 'Sales Summary By Customer & Item'];
    function sumCols(names, activity) {                                   // SummaryGridSetting
        return names.map(function (k) {
            var c = { key: k, caption: k };
            if (k === 'PrctByAmount' || k === 'PrctByWeight' || k === 'BillWeight' || k === 'Amount') { c.fmt = T4; c.sum = true; c.num = true; }
            else if (k === 'ItemQty') { c.fmt = T3; c.sum = true; c.num = true; }
            else if (k === 'AvgRate') { c.fmt = T4; c.num = true; }
            else if (k === 'DocDate') c.date = DATE;
            if (activity === 'Sales Register' && (k === 'DocumentTypeId' || k === 'Id')) c.hidden = true;
            if (k === 'CityName' && CITY_HIDDEN.indexOf(activity) >= 0) c.hidden = true;
            return c;
        });
    }
    var TEMPLATES = { 'Sales Register': '1809_01_SalesRegisterSummary', 'Sales Summary By Item': '1809_05-SalesRegisterSummaryByItemWithoutPacking',
        'Sales Summary By Item & City': '1809_15-SalesSummaryByItem&City', 'Sales Summary By Item & Pack Size': '1809_03-SalesRegisterSummaryByItem',
        'Sales Summary By Item & Warehouse': '1809_06-SalesRegisterSummaryByWarehouse', 'Sales Summary By Item,Pack Size & City': '1809_14-SalesSummaryByItemPackSize&City',
        'Sales Summary By Customer': '1809_02-SalesRegisterSummaryByCustomer', 'Sales Summary By Customer & Item': '1809_04-SalesRegisterSummaryByCustomer&Item',
        'Sales Summary By Customer & City': '1809_07-SalesSummaryByCustomer&City', 'Sales Summary By Customer,Item & City': '1809_09-SalesSummaryByCustomerItem&City',
        'Sales Summary By Customer & Pack Size': '1809_10-SalesSummaryByCustomer&PackSize', 'Sales Summary By Parent Category': '1809_11-SalesSummaryByParentCategory',
        'Sales Summary By Parent Category & Item': '1809_12-SalesSummaryByParentCategory&Item', 'Sales Summary By Parent Category & Customer': '1809_13-SalesSummaryByParentCategory&Customer' };
    var ACTIVITIES = ['Sales Register', 'Sales Summary By Item', 'Sales Summary By Item & City', 'Sales Summary By Item & Pack Size', 'Sales Summary By Item & Warehouse',
        'Sales Summary By Item,Pack Size & City', 'Sales Summary By Customer', 'Sales Summary By Customer & Item', 'Sales Summary By Customer & City',
        'Sales Summary By Customer,Item & City', 'Sales Summary By Customer & Pack Size', 'Sales Summary By Parent Category', 'Sales Summary By Parent Category & Item',
        'Sales Summary By Parent Category & Customer'];
    function activityText() { return S.selText('cmbActivitySummary'); }
    function printButtonManage() {                                        // btnPrint.Text = the template of the activity, hidden without an active row
        var span = el('btnPrint').querySelector('span'), a = activityText();
        if (a) { if (TEMPLATES[a]) span.textContent = TEMPLATES[a]; } else el('btnPrint').hidden = true;
    }
    function sumArgs() {
        return { fromDate: el('txtDateFromSummary').value, fromTime: times.sumFrom, toDate: el('txtToDateSummary').value, toTime: times.sumTo,
                 fromDocNo: A.toIntText(el('txtFromDocNoSummary').value.trim()), toDocNo: A.toIntText(el('txtToDocNoSummary').value.trim()),
                 itemClassGroupId: S.selInt('cmbItemclassSummary'), parentCategoryId: S.selInt('CmbParentCategorySummary'), itemCategoryId: S.selInt('cmbItemCatgorySummary'),
                 itemTypeId: S.selInt('cmbItemTypeSummary'), warehouseId: S.selInt('CmbWarehouseSummary'), jobLotId: S.selInt('CmbJobLotSummary'),
                 supplierCustomerId: S.selInt('CmbPartySummary'), itemId: S.selInt('CmbItemSummary'), packUom: S.selInt('cmbUOM'),
                 districtId: S.selInt('CmbDistrictSummary'), cityId: S.selInt('cmbCitySummary'), activity: activityText() };
    }
    var sseq = 0;
    function btnShowSummary() {
        el('btnPrint').hidden = false;
        var a = sumArgs(), token = ++sseq, b = el('btnShowSumamry');
        sumLast = { args: a, rows: 0 };
        A.busy(b, true);
        return A.getJson(API + '/summary?' + S.qs(a)).then(function (r) {
            if (token !== sseq) return;
            sumLast.rows = r && r.hasData ? 1 : 0;
            if (!r || !r.hasData) { sgrid.clear(); return; }                   // dtSummary empty -> ClearStructure
            if (r.columns) sgrid.setData(sumCols(r.columns, r.activity), r.rows);    // an unknown activity leaves the grid untouched
        }).catch(function (e) { w.alert(e.message); }).then(function () { if (token === sseq) A.busy(b, false); printButtonManage(); });
    }
    function btnPrint() {
        if (!sumLast.args || !sumLast.rows) { w.alert('Record Not Found For Display'); return; }
        var name = el('btnPrint').querySelector('span').textContent.trim();
        A.openPdf('/sale/bk/print/sale-summary-salt?' + S.qs(Object.assign({}, sumLast.args, { template: name }))).catch(function (e) { w.alert(e.message); });
    }
    function printWith(name) {                                            // tsDropDown_DropDownItemClicked
        if (!last.args || !last.rows) { w.alert('Record Not Found For Display'); return; }
        A.openPdf('/sale/bk/print/sale-register-salt?' + S.qs(Object.assign({}, last.args, { template: name }))).catch(function (e) { w.alert(e.message); });
    }
    S.dropdown('print', 'printMenu', function () { return reports; }, printWith);

    // ------------------------------------------------------------------ date types
    function datetypefill() {                                             // both BindDDL(ZeroIndex false), Rows[2].Activate()
        S.fill(el('cmbDateType'), S.dateTypeRows(false), false);
        S.fill(el('cmbDateTypeSummary'), S.dateTypeRows(false), false);
        S.activate('cmbDateType', 2, false);
        S.activate('cmbDateTypeSummary', 2, false);
    }
    el('cmbDateType').addEventListener('change', function () {
        var id = S.selInt('cmbDateType');
        if (id < 1 || id > 5) return;
        S.dateRule(id, el('GRNfromdate'), el('GRNToDate'), info.yearStart, {});
        gridHistory();
    });
    el('cmbDateTypeSummary').addEventListener('change', function () {
        var own = S.selInt('cmbDateTypeSummary'), h = S.selInt('cmbDateType'), t0 = A.today(), u = new Date();
        var from = el('txtDateFromSummary'), to = el('txtToDateSummary');
        if (own === 1) { from.value = t0; to.value = t0; times.sumFrom = nowTime(); times.sumTo = nowTime(); }
        else if (h === 2) { from.value = A.addDays(t0, -7); times.sumFrom = '00:00:00'; }
        else if (h === 3) { from.value = u.getUTCFullYear() + '-' + two(u.getUTCMonth() + 1) + '-01'; to.value = t0; times.sumFrom = '00:00:00'; times.sumTo = nowTime(); }
        else if (h === 4) { from.value = new Date().getFullYear() + '-01-01'; to.value = t0; times.sumFrom = '00:00:00'; times.sumTo = nowTime(); }
        else if (h === 5) { if (info.yearStart) from.value = S.isoDay(info.yearStart); times.sumFrom = '00:00:00'; }
    });

    // ------------------------------------------------------------------ combos
    function bindHistoryCombos(data) {
        if (!data) return;
        S.fill(el('CmbSupplier'), data.supplier, false);
        S.fill(el('cmbWareHouse'), data.warehouse, false);
        S.fill(el('cmbitem'), data.item, false);
        S.fill(el('CmbJobLot'), data.jobLot, false);
    }
    function bindSummaryCombos(data) {
        if (!data) return;
        S.fill(el('CmbParentCategorySummary'), data.parentCategory, false);
        S.fill(el('cmbItemTypeSummary'), data.itemType, false);
        S.fill(el('cmbItemclassSummary'), data.itemClass, false);
        S.fill(el('cmbItemCatgorySummary'), data.itemCategory, false);
        S.fill(el('CmbPartySummary'), data.party, false);
        S.fill(el('CmbItemSummary'), data.item, false);
        S.fill(el('cmbCitySummary'), data.city, false);
        S.fill(el('CmbJobLotSummary'), data.jobLot, false);
        S.fill(el('cmbWareHouse'), data.warehouse, false);                 // the desktop binds the GetWarehouse rows to the DETAIL tab's cmbWareHouse
        S.fill(el('CmbDistrictSummary'), data.district, false);
    }
    function allComboBind() { return A.getJson(API + '/combos').then(bindHistoryCombos).catch(function (e) { w.alert(e.message); }); }
    function allComboBindSummary() { return A.getJson(API + '/summary-combos').then(bindSummaryCombos).catch(function (e) { w.alert(e.message); }); }

    // ------------------------------------------------------------------ Reset / Refresh
    function clearCombo(id) { S.setIndex(id, 0); }
    function btnNew() {
        ['txtInvoiceNoFrom', 'txtInvoiceNoto', 'txtratefrom', 'txtrateto', 'txtsaleorderfrom', 'txtsaleorderto'].forEach(function (id) { el(id).value = ''; });
        clearCombo('CmbSupplier'); clearCombo('cmbWareHouse'); clearCombo('cmbitem');
        datetypefill();
        S.focus('cmbDateType');
    }
    function btnNewSummary() {
        ['txtFromDocNoSummary', 'txtToDocNoSummary'].forEach(function (id) { el(id).value = ''; });
        ['cmbItemclassSummary', 'CmbParentCategorySummary', 'cmbItemCatgorySummary', 'cmbItemTypeSummary', 'CmbWarehouseSummary', 'CmbJobLotSummary', 'CmbPartySummary', 'CmbItemSummary',
         'cmbUOM', 'CmbDistrictSummary'].forEach(clearCombo);
        S.activate('cmbActivitySummary', 0, false);
        S.activate('cmbDateType', 2, false);                               // cmbDateType.Rows[2].Activate() (the DETAIL tab's date type)
        S.focus('cmbDateType');
    }

    // ------------------------------------------------------------------ shortcuts, tabs, keys
    var KEYS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl + P', 'For print'], ['Ctrl+F5', 'For Focus On DateType '],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    var KEYS_SUM = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+S', 'For Show'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus On DateType '],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'],
        ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    function shortcuts() { A.shortcuts(tab === 1 ? KEYS_SUM : KEYS); }
    function closeForm() { if (w.history.length > 1) w.history.back(); else w.location.href = '/dashboard'; }
    function selectTab(i) {
        tab = i;
        Array.prototype.forEach.call(d.querySelectorAll('#tabStrip button'), function (b) { b.classList.toggle('on', Number(b.getAttribute('data-tab')) === i); });
        Array.prototype.forEach.call(d.querySelectorAll('.tabpage'), function (p) { p.hidden = Number(p.getAttribute('data-page')) !== i; });
    }
    el('tabStrip').addEventListener('click', function (e) { var b = e.target.closest('button[data-tab]'); if (b) selectTab(Number(b.getAttribute('data-tab'))); });
    d.addEventListener('keydown', function (e) {                           // frmSaleInvoiceHistory_KeyDown: the keys depend on the selected tab
        var dlg = el('shortcutDialog'); if (dlg && dlg.open) return;
        if ((e.ctrlKey && e.key.toLowerCase() === 'e') || (e.key === 'Escape' && !A.comboOpen())) { e.preventDefault(); closeForm(); return; }
        if (e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt')) { e.preventDefault(); shortcuts(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.length === 1 ? e.key.toLowerCase() : e.key, run = null;
        if (tab === 0) {
            if (k === 'n') run = btnNew; else if (k === 'r') run = allComboBind; else if (k === 's') run = gridHistory; else if (k === 'F5' || k === 'ArrowUp') run = function () { S.focus('cmbDateType'); };
            else if (k === 'ArrowDown') run = function () { grid.focus(); };
        } else {
            if (k === 'n') run = btnNewSummary; else if (k === 'r') run = allComboBindSummary; else if (k === 's') run = btnShowSummary; else if (k === 'p') run = btnPrint;
            else if (k === 'F5' || k === 'ArrowUp') run = function () { S.focus('cmbDateTypeSummary'); }; else if (k === 'ArrowDown') run = function () { sgrid.focus(); };
        }
        if (run) { e.preventDefault(); run(); }
    });
    ['txtInvoiceNoFrom', 'txtInvoiceNoto', 'txtsaleorderfrom', 'txtsaleorderto'].forEach(function (id) { S.digitsOnly(id, false); });   // txt*_KeyPress
    ['txtratefrom', 'txtrateto'].forEach(function (id) { S.digitsOnly(id, true); });                                                   // OnlytextdecimelFunction

    el('btnshow').addEventListener('click', gridHistory);
    el('btnNew').addEventListener('click', btnNew);
    el('btnRefresh').addEventListener('click', allComboBind);
    el('btnKeys').addEventListener('click', shortcuts);
    el('btnShowSumamry').addEventListener('click', btnShowSummary);
    el('sumNew').addEventListener('click', btnNewSummary);
    el('sumRefresh').addEventListener('click', allComboBindSummary);
    el('btnPrint').addEventListener('click', btnPrint);
    el('sumKeys').addEventListener('click', shortcuts);
    S.closeShortcuts();

    // ------------------------------------------------------------------ Load
    function load() {
        times.sumFrom = nowTime(); times.sumTo = nowTime();
        el('GRNToDate').value = A.today(); el('GRNfromdate').value = A.today();
        el('txtDateFromSummary').value = A.today(); el('txtToDateSummary').value = A.today();
        return A.getJson(API + '/lookups').then(function (data) {
            info = data || info;
            return A.getJson(API + '/dynamic-reports').then(function (l) { reports = l || []; }).catch(function () { reports = []; });   // ReportsLoad
        }).then(function () {
            datetypefill();                                                // fires the history Show and both date rules
            return allComboBind();
        }).then(allComboBindSummary).then(function () {
            S.fill(el('cmbActivitySummary'), ACTIVITIES.map(function (n, i) { return { Id: i + 1, name: n }; }), false);       // ActivityFill
            S.fill(el('cmbUOM'), info.uom.map(function (n) { return { Id: n, name: n + 'KG' }; }), false);                    // ItemUOMFill
            S.activate('cmbActivitySummary', 0, false);
            printButtonManage();
            S.focus('cmbDateType');
        }).catch(function (e) { w.alert(e.message); });
    }
    load();
}(window, document));
