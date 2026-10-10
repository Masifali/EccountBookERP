/* ============================================================================================
 * Screen 561 Sales_EvaulationDetailReports - Architecture.WinApp.pcc.Reports.Sales_EvaulationDetailReports (.cs, 3294 lines)
 * Page: templates/sale/reports/pcc_sales_evaluation_detail.html (route /sale/reports/pcc/sales-evaluation-detail).
 *   frmEvaulationDetailSalesReports_Load :202   the 17 per activity tables, AllComboBind, ActivityFill, Datetypefill, cmbActivity.Rows[0].Activate(), PrintButtonManage, focus Date Type
 *   Datetypefill :367           CommonServices.DateType, BindDDL(ZeroIndex true), Rows[2].Activate() ("This Week", From = today - 7)
 *   AllComboBind :382           InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales (DocType "Sale") = USP_GetDataFromInventoryStocksEvaluationsForSales, split by
 *                               Activity: GetParentCategory, GetItemType, GetItemClass, GetItemCategory, GetSupplierCustomer, GetItems, GetCity, GetJobLot, GetWarehouse, GetVehicleNos,
 *                               GetDistrict (RefName), each list bound only when it has rows, ZeroIndex false
 *   ActivityFill :544           the 17 activities (Sales Register ... Sales Summary By Parent Category & Customer)
 *   cmbDateType_ValueChanged :576   1 From = today | 2 today - 7 | 3 first of UTC month, To = today | 4 1 Jan, To = today | 5 Start_Period
 *   btnShow_Click :613 / GridFill :627   Sales or Returns must be chosen (ActionId 1 / 2); InvSaleInvoice.EvaulationDetailSalesReports = [pcc].[USP_Sales_EvaulationDetailReports]
 *                               (@ActivityName = the activity text); per activity the rows are copied positionally into that activity's table (Load :202 lists each table's columns);
 *                               no rows -> ClearStructure; then PrintButtonManage
 *   DataGridHistorySetting :893  Vehicles: grouped by VehicleNo (group totals, grouped column hidden), Id and DocumentTypeId hidden; every other activity: PrctByAmount / PrctByWeight
 *                               "#,##0.###" (summed), ItemQty / BillWeight / Amount "0,0" with total "#,##0.##" (summed), AvgRate "0,0"; GridAutoAdjustmentNew
 *   btnPrint_Click :963 / PrintButtonManage :1212   the Print button text is the template name of the activity of the last Show (1880 .. 1899, 1881 .. 1885); an activity without a
 *                               mapping ("Customer & Varient") keeps the previous text; the print runs 'Record Not Found For Display' when the last Show gave no rows
 *   btnNew_Click :1298 (clears doc nos, class, parent, category, type, warehouse, joblot, party, item, district; Activity row 0; Date Type re-bound), btnRefresh_Click :1336 (AllComboBind),
 *   btnShow_Leave :1348, KeyDown :1387, MakeShortCutKeys :1438
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/salepcc/sales-evaluation-detail';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null };
    var lastArgs = null, hasRows = false, seq = 0, printText = '', printShown = true;
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For print 312-SalesRegisterSummary'],
        ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    /* the per activity tables of Load: { activity, grid (DataGridHistory.Name), cols (table column names), src (dtGrid column of each), shortDate0, vehicles } */
    var ACTIVITIES = [
        {"activity":"Sales Register","grid":"SalesRegister","cols":["DocDate","DocCodeNo","Customer","ItemName","Varient","JobLotCode","VehicleNo","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight","CityName"],"src":["DocDate","DocCodeNo","Customer","ItemName","UOMCode","JobLotCode","VehicleNo","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight","CityName"],"shortDate0":true},
        {"activity":"Sales Summary By Vehicles","grid":"SalesSummaryByVehicles","cols":["Id","DocumentTypeId","DocumentType","InvoiceDate","InvoiceNo","Customer","ReferenceNo","ReferenceParty","RefPartyAddress","GdnNo","Distance","VehicleNo"],"src":["RefDocIdNo","RefDocumentTypeId","DocumentTypeCode","DocDate","DocCodeNo","Customer","ReferenceNo","ReferencePartyName","RefPartyAddress","GdnNo","Distance","VehicleNo"],"vehicles":true},
        {"activity":"Sales Summary By Item","grid":"SalesSummaryByItem","cols":["ItemName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["ItemName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]},
        {"activity":"Sales Summary By Item & City","grid":"","cols":["ItemName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight","CityName"],"src":["ItemName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight","CityName"]},
        {"activity":"Sales Summary By Item & Varient","grid":"SalesSummaryByItemandVarient","cols":["ItemName","Varient","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["ItemName","UOMCode","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]},
        {"activity":"Sales Summary By Item & Warehouse","grid":"SalesSummaryByItemNWarehouse","cols":["WarehouseName","ItemName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["WarehouseName","ItemName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]},
        {"activity":"Sales Summary By Item,Varient & City","grid":"SalesSummaryByItemNVarientCity","cols":["ItemName","Varient","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight","CityName"],"src":["ItemName","UOMCode","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight","CityName"]},
        {"activity":"Sales Summary By Customer","grid":"SalesSummaryByCustomer","cols":["PartyName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["Customer","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]},
        {"activity":"Sales Summary By Customer & Item","grid":"SalesSummaryByCustomerandItem","cols":["PartyName","ItemName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["Customer","ItemName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]},
        {"activity":"Sales Summary By Customer & City","grid":"SalesSummaryByCustomerandCity","cols":["PartyName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight","CityName"],"src":["Customer","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight","CityName"]},
        {"activity":"Sales Summary By Customer & Varient","grid":"SalesSummaryByCustomerandVareient","cols":["PartyName","Varient","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["Customer","UOMCode","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]},
        {"activity":"Sales Summary By Customer,Item & Varient","grid":"SalesSummaryByCustomerItemandVareient","cols":["PartyName","ItemName","Varient","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["Customer","ItemName","UOMCode","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]},
        {"activity":"Sales Summary By Customer,Item & City","grid":"SalesSummaryByCustomerItemandCity","cols":["PartyName","ItemName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight","CityName"],"src":["Customer","ItemName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight","CityName"]},
        {"activity":"Sales Summary By City","grid":"SalesSummaryByCity","cols":["CityName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["CityName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]},
        {"activity":"Sales Summary By Parent Category","grid":"SalesSummaryByParentCategory","cols":["ParentCategory","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["ParentCategory","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]},
        {"activity":"Sales Summary By Parent Category & Item","grid":"SalesSummaryByItemAndParentCategory","cols":["ItemName","ParentCategory","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["ItemName","ParentCategory","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]},
        {"activity":"Sales Summary By Parent Category & Customer","grid":"SalesSummaryByCustomerAndParentCategory","cols":["CustomerName","ParentCategory","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["Customer","ParentCategory","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"]}
    ];
    /* PrintButtonManage */
    var PRINT_TEXT = {
        'Sales Register': '1880-SalesRegisterSummary', 'Sales Summary By Item': '1889-SalesRegisterSummaryByItem', 'Sales Summary By Item & City': '1890-SalesSummaryByItem&City',
        'Sales Summary By Item & Varient': '1891-SalesRegisterSummaryByItem&Varient', 'Sales Summary By Item & Warehouse': '1892-SalesRegisterSummaryByItem&Warehouse',
        'Sales Summary By Item,Varient & City': '1893-SalesSummaryByItemPackSize&City', 'Sales Summary By Customer': '1894-SalesRegisterSummaryByCustomer',
        'Sales Summary By Customer & Item': '1895-SalesSummaryByCustomer&Item', 'Sales Summary By Customer & City': '1896-SalesRegisterSummaryByCustomer&City',
        'Sales Summary By Customer,Item & City': '1898-SalesSummaryByCustomerItem&City', 'Sales Summary By Customer & Pack Size': '1897-SalesSummaryByCustomer&Varient',
        'Sales Summary By City': '1899-SalesSummaryByCity', 'Sales Summary By Parent Category': '1881-SalesSummaryByParentCategory',
        'Sales Summary By Parent Category & Item': '1882-SalesSummaryByParentCategory&Item', 'Sales Summary By Parent Category & Customer': '1883-SalesSummaryByParentCategory&Customter',
        'Sales Summary By Vehicles': '1884-SalesSummaryByVehicles', 'Sales Summary By Customer,Item & Varient': '1885-SalesSummaryByCustomerItemandVareient'
    };

    // ------------------------------------------------------------------ lists
    function datetypefill() { S.fill(el('cmbDateType'), S.dateTypeRows(false), true); S.activate('cmbDateType', 2, true); }
    function activityFill() { S.fill(el('cmbActivity'), ACTIVITIES.map(function (a, i) { return { Id: i + 1, name: a.activity }; }), false); }
    var COMBOS = [['CmbParentCategory', 'GetParentCategory'], ['cmbItemtype', 'GetItemType'], ['cmbItemclass', 'GetItemClass'], ['cmbItemCatgory', 'GetItemCategory'],
        ['cmbSupplierCustomer', 'GetSupplierCustomer'], ['cmbItem', 'GetItems'], ['cmbCity', 'GetCity'], ['cmbJobLot', 'GetJobLot'], ['cmbWareHouse', 'GetWarehouse'],
        ['CmbVehicleNo', 'GetVehicleNos'], ['cmbdistrict', 'GetDistrict']];
    function bindCombos(data) {                                              // AllComboBind: a list is bound only when it has rows
        var c = (data && data.combos) || {};
        if (!Object.keys(c).length) return;
        COMBOS.forEach(function (m) { var rows = c[m[1]] || []; if (rows.length) S.fill(el(m[0]), rows, false); });
    }
    function allComboBind() { return A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; bindCombos(data); }); }
    el('cmbDateType').addEventListener('change', function () {               // cmbDateType_ValueChanged
        S.dateRule(A.toInt(this.value), el('txtDateFrom'), el('txtToDate'), lookup.yearStart);
    });
    function activityText() { return S.selText('cmbActivity'); }
    function printButtonManage() {                                           // PrintButtonManage
        var t = activityText();
        if (t === '') { printShown = false; el('print').hidden = true; return; }
        if (PRINT_TEXT[t]) printText = PRINT_TEXT[t];
        if (printText) el('printText').textContent = printText;
    }

    // ------------------------------------------------------------------ grid
    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 1, frozen: 0, autosize: true });
    grid.groupKey = '';
    (function groupSupport() {                                               // RootTable.Groups.Add("VehicleNo") with group totals
        var vr = grid.visibleRows, rr = grid.render;
        grid.visibleRows = function () {
            var rows = vr.call(this);
            if (!this.groupKey) return rows;
            var gk = this.groupKey;
            return rows.map(function (r, i) { return { r: r, i: i }; }).sort(function (a, b) {
                var x = String(A.ci(a.r, gk) == null ? '' : A.ci(a.r, gk)), y = String(A.ci(b.r, gk) == null ? '' : A.ci(b.r, gk));
                return x.localeCompare(y) || a.i - b.i;
            }).map(function (o) { return o.r; });
        };
        grid.render = function () {
            rr.call(this);
            if (!this.groupKey || !this.columns) return;
            var cols = this.visibleCols(), tb = A.el(this.cfg.tableId).tBodies[0], prev = null, n = this.flat.length, i, j;
            for (i = 0; i < n; i++) {
                var f = this.flat[i], key = A.ci(f.row, this.groupKey);
                if (i === 0 || String(key) !== String(prev)) {
                    var tr = d.createElement('tr'); tr.className = 'r grp';
                    var td = d.createElement('td'); td.colSpan = Math.max(1, cols.length); td.style.fontWeight = 'bold';
                    td.textContent = this.groupKey + ': ' + (key == null ? '' : key);
                    tr.appendChild(td);
                    tr.style.background = '#dfe8f3';
                    f.tr.parentNode.insertBefore(tr, f.tr);
                }
                prev = key;
            }
            void tb; void j;
        };
    }());
    function columnsFor(a) {
        var cols = a.cols.map(function (name) {
            var c = { key: name, caption: name };
            if (a.vehicles) {                                                // only the id columns are set: Id and DocumentTypeId hidden, VehicleNo is the group
                if (name === 'Id' || name === 'DocumentTypeId' || name === 'VehicleNo') c.hidden = true;
                if (name === 'InvoiceDate') c.date = 'dd/MM/yyyy';
                return c;
            }
            if (a.shortDate0 && name === 'DocDate') c.date = 'dd/MM/yyyy';
            if (name === 'PrctByAmount' || name === 'PrctByWeight') { c.fmt = '#,##0.###'; c.totalFmt = '#,##0.###'; c.sum = true; c.num = true; }
            else if (name === 'ItemQty' || name === 'BillWeight' || name === 'Amount') { c.fmt = '0,0'; c.totalFmt = '#,##0.##'; c.sum = true; c.num = true; }
            else if (name === 'AvgRate') { c.fmt = '0,0'; c.num = true; }
            return c;
        });
        return cols;
    }
    function mapRows(a, rows) {
        return rows.map(function (r) {
            var m = {};
            a.cols.forEach(function (name, i) { m[name] = A.ci(r, a.src[i]); });
            return m;
        });
    }

    // ------------------------------------------------------------------ Show / print
    function radio() { var x = d.querySelector('input[name="rdAction"]:checked'); return x ? A.toInt(x.value) : 0; }
    function args() {
        var vehId = S.selInt('CmbVehicleNo');
        return { fromDate: el('txtDateFrom').value, toDate: el('txtToDate').value, fromDocNo: A.toIntText(el('txtFromDocNo').value), toDocNo: A.toIntText(el('txtToDocNo').value),
                 parentCategoryId: S.selInt('CmbParentCategory'), itemCategoryId: S.selInt('cmbItemCatgory'), supplierCustomerId: S.selInt('cmbSupplierCustomer'),
                 itemId: S.selInt('cmbItem'), jobLotId: S.selInt('cmbJobLot'), cityId: S.selInt('cmbCity'), vehicleId: vehId, vehicleNo: vehId > 0 ? S.selText('CmbVehicleNo') : '',
                 warehouseId: S.selInt('cmbWareHouse'), districtId: S.selInt('cmbdistrict'), itemClassId: S.selInt('cmbItemclass'), itemTypeId: S.selInt('cmbItemtype'),
                 actionId: radio(), activity: activityText() };
    }
    function gridFill() {                                                    // GridFill
        var b = el('show');
        if (!radio()) { alert('Please Select on Value Either Sales or Return'); return Promise.resolve(); }
        var a = args(), token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.groupKey = ''; grid.clear(); return; }      // ClearStructure
            var act = ACTIVITIES.filter(function (x) { return x.activity === a.activity; })[0];
            if (!act) { grid.groupKey = ''; grid.clear(); return; }
            grid.groupKey = act.vehicles ? 'VehicleNo' : '';
            grid.setData(columnsFor(act), mapRows(act, rows));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function show() {                                                        // btnShow_Click
        if (el('show').disabled) return;
        el('print').hidden = false; printShown = true;
        gridFill().then(printButtonManage);
    }
    function print() {                                                       // btnPrint_Click
        var b = el('print'); if (b.disabled || b.hidden) return;
        if (!hasRows || !lastArgs) { alert('Record Not Found For Display'); return; }
        var q = {}; Object.keys(lastArgs).forEach(function (k) { q[k] = lastArgs[k]; });
        q.template = printText;
        A.busy(b, true);
        A.openPdf('/sale/reports/pcc/print/sales-evaluation-detail?' + S.qs(q)).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() {                                                       // btnNew_Click
        ['txtFromDocNo', 'txtToDocNo'].forEach(function (id) { el(id).value = ''; });
        ['cmbItemclass', 'CmbParentCategory', 'cmbItemCatgory', 'cmbItemtype', 'cmbWareHouse', 'cmbJobLot', 'cmbSupplierCustomer', 'cmbItem', 'cmbdistrict'].forEach(function (id) {
            var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true }));
        });
        S.activate('cmbActivity', 0, false);
        S.focus('cmbDateType');
        datetypefill();
    }
    function refresh() {                                                     // btnRefresh_Click
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        allComboBind().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    ['txtFromDocNo', 'txtToDocNo'].forEach(function (id) { S.digitsOnly(id); });
    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && el('grid').contains(e.relatedTarget)) S.focus('cmbDateType'); });   // btnShow_Leave
    S.keys({ s: show, e: closeForm, r: refresh, n: reset, p: print, F5: function () { S.focus('cmbDateType'); }, ArrowUp: function () { S.focus('cmbDateType'); },
             ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('txtDateFrom').value = A.today(); el('txtToDate').value = A.today();
    grid.render();
    activityFill(); datetypefill();
    S.activate('cmbActivity', 0, false);                                     // cmbActivity.Rows[0].Activate()
    printButtonManage();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        bindCombos(data);                                                    // AllComboBind
        S.activate('cmbDateType', 2, true);                                  // the year start is known now: re-apply the date type rule
        S.focus('cmbDateType');
    }).catch(function (e) { alert(e.message); });
}(window, document));
