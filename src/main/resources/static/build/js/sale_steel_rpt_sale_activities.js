/* ============================================================================================
 * Screen 558 SaleInvoiceRegisterWithActivities - Architecture.WinApp.Steel.Reports.SalesReports.SaleInvoiceRegisterWithActivities (.cs)
 * Page: templates/sale/steel/rpt_sale_invoice_register_activities_st.html (route /sale/reports/steel/sale-invoice-register-with-activities).
 *   frmEvaulationDetailSalesReports_Load :55  AllComboBind, ActivityFill, ItemUOMFill, txtDateFrom = Start_Period, cmbActivity.Rows[0] ("Sales Register"), GridFill, PrintButtonManage
 *   AllComboBind :219    InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales (USP_GetDataFromInventoryStocksEvaluationsForSales Org, Company, UserId, AppId,
 *                        DocType 'Sale'): Activity GetParentCategory / GetItemType / GetItemClass (the desktop fills the class combo from the GetItemType rows) / GetItemCategory /
 *                        GetSupplierCustomer / GetItems / GetCity / GetJobLot / GetWarehouse / GetDistrict, Id / RefName (BindDDLNew, ZeroIndex false)
 *   ItemUOMFill :298     1KG 5KG 10KG 20KG 25KG 40KG 50KG 60KG 65KG 80KG 100KG (Id = the number).  cmbItem_ValueChanged :352 re-binds it.
 *   ActivityFill :324    the 14 activity names.  GridFill :375  [ST].[USP-SaleInvoiceRegisterWithActivities] (@ActivityName = the combo TEXT; @PackUomId is never sent: the BLL reads
 *                        RateUOM, the form sets PackSizeFrom); the dtSales... table of the activity is shown; no row / no activity -> ClearStructure
 *   DataGridHistorySetting :587  PrctByAmount / PrctByWeight "#,##0.###" (summed), ItemQty / BillWeight / Amount "0,0" (total "#,##0.##", summed), AvgRate "0,0";
 *                        Sales Register hides DocumentTypeId and Id; CityName hidden for Item, Item & Pack Size, Item & Warehouse, Customer, Customer & Item; GridAutoAdjustment
 *   PrintButtonManage :703  the Print button text becomes the activity's .rpt name; btnPrint_Click :652 "Record Not Found For Display" else that .rpt over dtGrid
 *   btnNew :800 (Refresh) AllComboBind, ActivityFill, ItemUOMFill.  txtFromDocNo / txtToDocNo: digits only.  KeyDown: Ctrl+S show, Ctrl+P print, Ctrl+R refresh, Ctrl+E / Esc close
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/steel-reports/sale-invoice-register-with-activities';
    var PRINT = '/sale/reports/steel/print/';
    var ACTIVITIES = ['Sales Register', 'Sales Summary By Item', 'Sales Summary By Item & City', 'Sales Summary By Item & Pack Size', 'Sales Summary By Item & Warehouse',
        'Sales Summary By Item,Pack Size & City', 'Sales Summary By Customer', 'Sales Summary By Customer & Item', 'Sales Summary By Customer & City',
        'Sales Summary By Customer,Item & City', 'Sales Summary By Customer & Pack Size', 'Sales Summary By Parent Category', 'Sales Summary By Parent Category & Item',
        'Sales Summary By Parent Category & Customer'];
    var RPT = { 'Sales Register': '1522-SalesRegisterSummary', 'Sales Summary By Item': '1526-SalesRegisterSummaryByItemWithoutPacking', 'Sales Summary By Item & City': '1528-SalesSummaryByItem&City',
        'Sales Summary By Item & Pack Size': '1524-SalesRegisterSummaryByItem', 'Sales Summary By Item & Warehouse': '1527-SalesRegisterSummaryByWarehouse',
        'Sales Summary By Item,Pack Size & City': '1530-SalesSummaryByItemPackSize&City', 'Sales Summary By Customer': '1523-SalesRegisterSummaryByCustomer',
        'Sales Summary By Customer & Item': '1525-SalesRegisterSummaryByCustomer&Item', 'Sales Summary By Customer & City': '1529-SalesSummaryByCustomer&City',
        'Sales Summary By Customer,Item & City': '1531-SalesSummaryByCustomerItem&City', 'Sales Summary By Customer & Pack Size': '1532-SalesSummaryByCustomer&PackSize',
        'Sales Summary By Parent Category': '1533-SalesSummaryByParentCategory', 'Sales Summary By Parent Category & Item': '1534-SalesSummaryByParentCategory&Item',
        'Sales Summary By Parent Category & Customer': '1535-SalesSummaryByParentCategory&Customer' };
    var HIDE_CITY = ['Sales Summary By Item & Pack Size', 'Sales Summary By Item', 'Sales Summary By Item & Warehouse', 'Sales Summary By Customer', 'Sales Summary By Customer & Item'];
    var lastArgs = null, hasRows = false, seq = 0, rptName = '', printVisible = true;

    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, autosize: true });

    // ------------------------------------------------------------------ grid columns of the shown table (DataGridHistorySetting)
    function colsOf(names, activity) {
        var q3 = '#,##0.###';
        return names.map(function (n) {
            var c = { key: n, caption: n };
            if (n === 'PrctByAmount' || n === 'PrctByWeight') { c.fmt = q3; c.totalFmt = q3; c.sum = true; c.num = true; }
            else if (n === 'ItemQty' || n === 'BillWeight' || n === 'Amount') { c.fmt = '0,0'; c.totalFmt = '#,##0.##'; c.sum = true; c.num = true; }
            else if (n === 'AvgRate') { c.fmt = '0,0'; c.num = true; }
            else if (n === 'DocDate') c.date = 'dd-MMM-yy';
            else if (n === 'ItemCode') c.align = 'r';
            if (activity === 'Sales Register' && (n === 'DocumentTypeId' || n === 'Id')) c.hidden = true;
            if (n === 'CityName' && HIDE_CITY.indexOf(activity) >= 0) c.hidden = true;
            return c;
        });
    }

    // ------------------------------------------------------------------ lists
    function bind(data) {
        S.fill(el('CmbParentCategory'), data.parentCategories, false);
        S.fill(el('cmbItemtype'), data.itemTypes, false);
        S.fill(el('cmbItemclass'), data.itemClasses, false);
        S.fill(el('cmbItemCatgory'), data.itemCategories, false);
        S.fill(el('cmbSupplierCustomer'), data.parties, false);
        S.fill(el('cmbItem'), data.items, false);
        S.fill(el('cmbCity'), data.cities, false);
        S.fill(el('cmbJobLot'), data.jobLots, false);
        S.fill(el('cmbWareHouse'), data.warehouses, false);
        S.fill(el('cmbdistrict'), data.districts, false);
    }
    function allComboBind(url) { return A.getJson(url).then(function (data) { bind(data); return data; }); }
    function itemUOMFill() {
        S.fill(el('cmbUOM'), [1, 5, 10, 20, 25, 40, 50, 60, 65, 80, 100].map(function (n) { return { Id: n, name: n + 'KG' }; }), false);
    }
    function activityFill() { S.fill(el('cmbActivity'), ACTIVITIES.map(function (n, i) { return { Id: i + 1, name: n }; }), false); }
    el('cmbItem').addEventListener('change', itemUOMFill);                       // cmbItem_ValueChanged -> ItemUOMFill(item)

    // ------------------------------------------------------------------ PrintButtonManage
    function printButtonManage() {
        var act = S.selText('cmbActivity'), b = el('print');
        if (act !== '') {                                                         // cmbActivity.ActiveRow != null
            if (RPT[act]) { rptName = RPT[act]; b.querySelector('span').textContent = rptName; }
        } else { printVisible = false; b.hidden = true; }                         // btnPrint.Visible = false
    }

    // ------------------------------------------------------------------ Show
    function gridFill() {
        var b = el('btnShow'); if (b.disabled) return Promise.resolve();
        var a = { fromDate: el('txtDateFrom').value, toDate: el('txtToDate').value, fromDocNo: A.toIntText(el('txtFromDocNo').value), toDocNo: A.toIntText(el('txtToDocNo').value),
                  parentCategoryId: S.selInt('CmbParentCategory'), itemCategoryId: S.selInt('cmbItemCatgory'), supplierCustomerId: S.selInt('cmbSupplierCustomer'),
                  itemId: S.selInt('cmbItem'), jobLotId: S.selInt('cmbJobLot'), cityId: S.selInt('cmbCity'), warehouseId: S.selInt('cmbWareHouse'),
                  districtId: S.selInt('cmbdistrict'), itemClassId: S.selInt('cmbItemclass'), itemTypeId: S.selInt('cmbItemtype'), activity: S.selText('cmbActivity') };
        var token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (res) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(res && res.rows && res.rows.length);
            if (!hasRows) { grid.clear(); return; }
            grid.setData(colsOf(res.columns, res.activity), res.rows);
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function show() { return gridFill().then(printButtonManage); }               // btnShow_Click: GridFill, PrintButtonManage

    // ------------------------------------------------------------------ toolbar
    function btnNew() {                                                           // btnNew_Click
        allComboBind(API + '/combos').then(function () { activityFill(); itemUOMFill(); }).catch(function (e) { alert(e.message); });
    }
    function print() {                                                            // btnPrint_Click
        if (!hasRows || !lastArgs) { alert('Record Not Found For Display'); return; }
        if (!rptName) return;
        A.openPdf(PRINT + 'sale-invoice-register-with-activities?' + S.qs(Object.assign({ rpt: rptName }, lastArgs))).catch(function (e) { alert(e.message); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    el('btnShow').addEventListener('click', show);
    el('reset').addEventListener('click', btnNew);
    el('print').addEventListener('click', print);
    S.digitsOnly('txtFromDocNo'); S.digitsOnly('txtToDocNo');
    S.keys({ s: show, p: print, r: btnNew, e: closeForm }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('txtDateFrom').value = A.today(); el('txtToDate').value = A.today();
    grid.render();
    allComboBind(API + '/lookups').then(function (data) {
        activityFill(); itemUOMFill();
        if (data.yearStart) el('txtDateFrom').value = S.isoDay(data.yearStart);    // txtDateFrom = ActiveYr.Start_Period
        S.activate('cmbActivity', 0, false);                                       // cmbActivity.Rows[0]
        return show();
    }).then(function () { S.focus('txtDateFrom'); }).catch(function (e) { alert(e.message); });
}(window, document));
