/* ============================================================================================
 * Screen 556 SaleReportWithActivities_Engr - Architecture.WinApp.Inventory_Reports.SaleReportWithActivities_Engr (.cs, 3342 lines)
 * Page: templates/sale/reports/saengr_sale_report_activities.html (route /sale/reports/engr/sale-report-with-activities).
 *   frmEvaulationDetailSalesReports_Load :196  AllComboBind, ActivityFill, Datetypefill, ItemUOMFill, cmbActivity.Rows[0].Activate(), PrintButtonManage, focus the date type
 *   Datetypefill :368          CommonServices.DateType, BindDDL(ZeroIndex true), Rows[2].Activate() ("This Week")
 *   AllComboBind :383          InventoryStockEvalautionDetail.GetDataFromInventoryStocksEvaluationsForSales (DocType "Sale") = USP_GetDataFromInventoryStocksEvaluationsForSales, split by
 *                              Activity: GetParentCategory, GetItemType (also feeds Item Class - the desktop builds dtClass from the item type rows), GetItemCategory, GetSupplierCustomer,
 *                              GetItems, GetCity, GetJobLot, GetWarehouse (text RefName), ZeroIndex false
 *   ItemUOMFill :464 / cmbItem_ValueChanged :522   the fixed pack list 1KG..100KG (Id = the kg); it is rebound (selection cleared) on every item change
 *   ActivityFill :491          the 16 activities (Sales Register ... Sales Summary By Parent Category & Customer)
 *   btnShow_Click :534 / GridFill :547   Sales_EvaulationDetailReports_Engr = USP_Sales_EvaulationDetailReports_Engr (the activity text is @ActivityName), then PrintButtonManage;
 *                              per activity the rows are copied positionally into that activity's table (Load :196 lists each table's columns), btnPrint366B visible only for
 *                              "Sales Summary By Customer & Invoice"; no rows -> ClearStructure
 *   DataGridHistorySetting :802  PrctByAmount / Amount "stringFormatsingle", PrctByWeight / BillWeight "#,##0.####", ItemQty "#,##0.###" (all summed), AvgRate DecimalRateFormate;
 *                              invoice activity: grouped by CustomerName (group totals, grouped column hidden), RefId / RefDocumentTypeId / CustomerId hidden, money columns summed;
 *                              Sales Register: Id and DocumentTypeId hidden, "Approval Detail" button (120) at position 0, FrozenColumns 1
 *   DataGridHistory_ColumnButtonClick :890 / _KeyDown :1568  Approval Detail -> frmApprovalCommentory(DocumentTypeId, Id)
 *   btnPrint_Click :929 / PrintButtonManage :1164  the Print button text is the template name of the activity of the last Show (360 .. 374, 366A); btnPrint366B_Click :1647
 *   btnNew_Click :1246 (Reset), btnRefresh_Click :1284, cmbDateType_ValueChanged :1595, KeyDown :1321 (Alt+1..9 print only when the button text matches), MakeShortCutKeys :1521
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, E = w.SaEngr, el = A.el;
    var API = '/api/sale/saengr/sales-activities';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null };
    var lastArgs = null, hasRows = false, seq = 0, printText = '360-SalesRegisterSummary';
    var INVOICE = 'Sales Summary By Customer & Invoice';
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
        ['Alt + 1', 'For print 360-SalesRegisterSummary'], ['Alt + 2', 'For print 361-SalesSummaryByItem'], ['Alt + 3', 'For print 362-SalesSummaryByItem&City'],
        ['Alt + 4', 'For print 363-SalesSummaryByItem&PackSize'], ['Alt + 5', 'For print 364-SalesSummaryByItem&Warehouse'], ['Alt + 6', 'For print 365-SalesSummaryByItemPackSize&City'],
        ['Alt + 7', 'For print 366-SalesSummaryByCustomer'], ['Alt + 8', 'For print 367-SalesSummaryByCustomer&Item'], ['Alt + 7', 'For print 368-SalesSummaryByCustomer&City'],
        ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
        ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
    /* the per activity tables of Load: { activity, grid (DataGridHistory.Name), cols (table column names), src (dtGrid column of each), shortDate0, p366B } */
    var ACTIVITIES = [
        {"activity":"Sales Register","grid":"SalesRegister","cols":["DocDate","DocNo","Id","DocumentTypeId","DocType","PartyName","ItemCode","ItemName","PackUom","JobLot","VehicleNo","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight","CityName"],"src":["DocDate","DocCodeNo","RefDocIdNo","RefDocumentTypeId","DocumentTypeCode","Customer","ItemCode","ItemName","UOMCode","JobLotCode","VehicleNo","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight","CityName"],"shortDate0":true,"p366B":false},
        {"activity":"Sales Summary By Item","grid":"SalesSummaryByItem","cols":["ItemCode","ItemName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["ItemCode","ItemName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Item & Pack Size","grid":"SalesSummaryByItemAndPackSize","cols":["ItemCode","ItemName","PackUom","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["ItemCode","ItemName","UOMCode","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Item & Warehouse","grid":"SalesSummaryByItemAndWarehouse","cols":["WarehouseName","ItemCode","ItemName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["WarehouseName","ItemCode","ItemName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Item & City","grid":"SalesSummaryByItemAndCity","cols":["ItemCode","ItemName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight","CityName"],"src":["ItemCode","ItemName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight","CityName"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Item,Pack Size & City","grid":"SalesSummaryByItemPackSizeAndCity","cols":["ItemCode","ItemName","PackUom","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight","CityName"],"src":["ItemCode","ItemName","UOMCode","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight","CityName"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Customer","grid":"SalesSummaryByCustomer","cols":["PartyName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["Customer","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Customer & Item","grid":"SalesSummaryByCustomerAndItem","cols":["PartyName","ItemCode","ItemName","PackUom","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["Customer","ItemCode","ItemName","UOMCode","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Customer & City","grid":"SalesSummaryByCustomerAndCity","cols":["PartyName","PackUom","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight","CityName"],"src":["Customer","UOMCode","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight","CityName"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Customer,Item & City","grid":"SalesSummaryByCustomerItemAndCity","cols":["PartyName","ItemCode","ItemName","PackUom","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight","CityName"],"src":["Customer","ItemCode","ItemName","UOMCode","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight","CityName"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Customer & Pack Size","grid":"SalesSummaryByCustomerAndPackSize","cols":["PartyName","PackUom","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["Customer","UOMCode","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Customer & Invoice","grid":"SalesSummaryByCustomerAndInvoice","cols":["RefId","RefDocumentTypeId","CustomerId","CustomerName","InvoiceNo","InvoiceDate","InvoiceAmount","ItemAmount","S.Tax%","S.TaxAmount","ItemAmount+S.Tax","Others","TotalAmount"],"src":["RefDocIdNo","RefDocumentTypeId","SupplierCustomerId","Customer","DocCodeNo","DocDate","BillAmount","AmountOut","TaxPercent","TaxAmount","ItemAmountWithTax","Expenses","TotalAmount"],"shortDate0":false,"p366B":true},
        {"activity":"Sales Summary By City","grid":"SalesSummaryByCity","cols":["CityName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["CityName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Parent Category","grid":"SalesSummaryByParentCategory","cols":["ParentCategory","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["ParentCategory","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Parent Category & Item","grid":"SalesSummaryByParentCategoryAndItem","cols":["ParentCategory","ItemName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["ParentCategory","ItemName","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"],"shortDate0":false,"p366B":false},
        {"activity":"Sales Summary By Parent Category & Customer","grid":"SalesSummaryByParentCategoryAndCustomer","cols":["ParentCategory","CustomerName","ItemQty","BillWeight","Amount","AvgRate","PrctByAmount","PrctByWeight"],"src":["ParentCategory","Customer","QtyOut","BillWeightOut","AmountOut","AvgRate","PrcntOfTotal","PrcntOfTotalWeight"],"shortDate0":false,"p366B":false}
    ];
    /* PrintButtonManage */
    var PRINT_TEXT = {
        'Sales Register': '360-SalesRegisterSummary', 'Sales Summary By Item': '361-SalesSummaryByItem', 'Sales Summary By Item & City': '362-SalesSummaryByItem&City',
        'Sales Summary By Item & Pack Size': '363-SalesSummaryByItem&PackSize', 'Sales Summary By Item & Warehouse': '364-SalesSummaryByItem&Warehouse',
        'Sales Summary By Item,Pack Size & City': '365-SalesSummaryByItemPackSize&City', 'Sales Summary By Customer': '366-SalesSummaryByCustomer',
        'Sales Summary By Customer & Item': '367-SalesSummaryByCustomer&Item', 'Sales Summary By Customer & City': '368-SalesSummaryByCustomer&City',
        'Sales Summary By Customer,Item & City': '369-SalesSummaryByCustomerItem&City', 'Sales Summary By Customer & Pack Size': '370-SalesSummaryByCustomer&PackSize',
        'Sales Summary By City': '371-SalesSummaryByCity', 'Sales Summary By Parent Category': '372-SalesSummaryByParentCategory',
        'Sales Summary By Parent Category & Item': '373-SalesSummaryByParentCategory&Item', 'Sales Summary By Parent Category & Customer': '374-SalesSummaryByParentCategory&Customer',
        'Sales Summary By Customer & Invoice': '366ASalesSummaryByCustomer&Invoice'
    };
    var ALT_PRINT = { '1': '360-SalesRegisterSummary', '2': '361-SalesSummaryByItem', '3': '362-SalesSummaryByItem&City', '4': '363-SalesSummaryByItem&PackSize',
        '5': '364-SalesSummaryByItem&Warehouse', '6': '365-SalesSummaryByItemPackSize&City', '7': '366-SalesSummaryByCustomer', '8': '367-SalesSummaryByCustomer&Item',
        '9': '368-SalesSummaryByCustomer&City' };
    var UOMS = [[1, '1KG'], [5, '5KG'], [10, '10KG'], [20, '20KG'], [25, '25KG'], [40, '40KG'], [50, '50KG'], [60, '60KG'], [65, '65KG'], [80, '80KG'], [100, '100KG']];

    // ------------------------------------------------------------------ lists
    function datetypefill() { S.fill(el('cmbDateType'), S.dateTypeRows(false), true); S.activate('cmbDateType', 2, true); }
    function activityFill() {
        S.fill(el('cmbActivity'), ACTIVITIES.map(function (a, i) { return { Id: i + 1, name: a.activity }; }), false);
    }
    function itemUomFill() { S.fill(el('cmbUOM'), UOMS.map(function (u) { return { Id: u[0], name: u[1] }; }), false); }
    function bindCombos(data) {
        var c = (data && data.combos) || {};
        if (!Object.keys(c).length) return;                                      // AllComboBind binds only when the procedure gives rows
        function rows(k) { return c[k] || []; }
        S.fill(el('CmbParentCategory'), rows('GetParentCategory'), false);
        S.fill(el('cmbItemtype'), rows('GetItemType'), false);
        S.fill(el('cmbItemclass'), rows('GetItemType'), false);                  // dtClass is built from rowsType in the desktop
        S.fill(el('cmbItemCatgory'), rows('GetItemCategory'), false);
        S.fill(el('cmbSupplierCustomer'), rows('GetSupplierCustomer'), false);
        S.fill(el('cmbItem'), rows('GetItems'), false);
        S.fill(el('cmbCity'), rows('GetCity'), false);
        S.fill(el('cmbJobLot'), rows('GetJobLot'), false);
        S.fill(el('cmbWareHouse'), rows('GetWarehouse'), false);
    }
    function allComboBind() { return A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; bindCombos(data); }); }
    el('cmbDateType').addEventListener('change', function () {               // cmbDateType_ValueChanged
        S.dateRule(A.toInt(this.value), el('txtDateFrom'), el('txtToDate'), lookup.yearStart);
    });
    el('cmbItem').addEventListener('change', itemUomFill);                   // cmbItem_ValueChanged -> ItemUOMFill

    function activityText() { return S.selText('cmbActivity'); }
    function printButtonManage() {                                           // PrintButtonManage
        var t = activityText();
        if (t === '') { el('print').hidden = true; return; }
        if (PRINT_TEXT[t]) printText = PRINT_TEXT[t];
        el('printText').textContent = printText;
    }

    // ------------------------------------------------------------------ grid
    var grid = new S.Grid({
        tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, frozen: 0, autosize: true,
        onLink: function (row, col) { if (col && col.key === 'ApprovalDetail') approval(row); },
        onButton: function (row, col) { if (col && col.key === 'ApprovalDetail') approval(row); }
    });
    grid.groupKey = '';
    (function groupSupport() {                                               // RootTable.Groups.Add("CustomerName") with group totals
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
            var g = this, cols = this.visibleCols(), tb = A.el(this.cfg.tableId).tBodies[0], prev = null, n = this.flat.length, i, j;
            var firstSum = 0;
            for (i = 0; i < cols.length; i++) if (cols[i].sum) { firstSum = i; break; }
            for (i = 0; i < n; i++) {
                var f = this.flat[i], key = A.ci(f.row, this.groupKey);
                if (i === 0 || String(key) !== String(prev)) {
                    var tr = d.createElement('tr'); tr.className = 'r grp';
                    var td = d.createElement('td'); td.colSpan = Math.max(1, firstSum); td.style.fontWeight = 'bold';
                    td.textContent = this.groupKey + ': ' + (key == null ? '' : key);
                    tr.appendChild(td);
                    for (j = Math.max(1, firstSum); j < cols.length; j++) {
                        var c = cols[j], cell = d.createElement('td'); cell.style.fontWeight = 'bold';
                        if (c.sum) {
                            var s = 0;
                            for (var k = 0; k < n; k++) if (String(A.ci(this.flat[k].row, this.groupKey)) === String(key)) s += A.toDouble(A.ci(this.flat[k].row, c.key));
                            cell.className = 'num'; cell.textContent = S.fmtNum(s, c.totalFmt || c.fmt || '#,##0.##');
                        }
                        tr.appendChild(cell);
                    }
                    tr.style.background = '#dfe8f3';
                    f.tr.parentNode.insertBefore(tr, f.tr);
                }
                prev = key;
            }
        };
    }());
    function approval(row) {                                                 // frmApprovalCommentory(DocumentTypeId, Id)
        if (!lastArgs) return;
        var q = {}; Object.keys(lastArgs).forEach(function (k) { q[k] = lastArgs[k]; });
        q.id = A.toInt(row.Id); q.documentTypeId = A.toInt(row.DocumentTypeId); q.idColumn = 'RefDocIdNo'; q.typeColumn = 'RefDocumentTypeId';
        E.approvalHistory(API, q).catch(function (e) { alert(e.message); });
    }
    function columnsFor(a) {
        var single = S.fmtSingle(lookup.amountDecimals), rate = S.fmtRate(lookup.rateDecimals);
        var inv = a.activity === INVOICE;
        var list = a.cols.map(function (name, i) {
            var c = { key: name, caption: name };
            if (a.shortDate0 && name === 'DocDate') c.date = 'dd/MM/yyyy';
            if (name === 'InvoiceDate') c.date = 'dd/MM/yyyy';
            if (!inv) {
                if (name === 'PrctByAmount' || name === 'Amount') { c.fmt = single; c.totalFmt = single; c.sum = true; c.num = true; }
                else if (name === 'PrctByWeight' || name === 'BillWeight') { c.fmt = '#,##0.####'; c.totalFmt = '#,##0.####'; c.sum = true; c.num = true; }
                else if (name === 'ItemQty') { c.fmt = '#,##0.###'; c.totalFmt = '#,##0.###'; c.sum = true; c.num = true; }
                else if (name === 'AvgRate') { c.fmt = rate; c.num = true; }
            } else {
                if (name === 'InvoiceAmount' || name === 'ItemAmount' || name === 'S.TaxAmount' || name === 'ItemAmount+S.Tax' || name === 'Others' || name === 'TotalAmount') { c.fmt = single; c.totalFmt = single; c.sum = true; c.num = true; }
                else if (name === 'S.Tax%') { c.fmt = '#,##0.###'; c.num = true; }
                if (name === 'RefId' || name === 'RefDocumentTypeId' || name === 'CustomerId' || name === 'CustomerName') c.hidden = true;   // hidden / HideColumnsWhenGrouped
            }
            if (a.activity === 'Sales Register' && (name === 'Id' || name === 'DocumentTypeId')) c.hidden = true;
            return c;
        });
        if (a.activity === 'Sales Register') list.unshift({ key: 'ApprovalDetail', caption: 'Approval Detail', width: 120, button: 'Approval Detail' });
        return list;
    }
    function mapRows(a, rows) {
        return rows.map(function (r) {
            var m = {};
            a.cols.forEach(function (name, i) { m[name] = A.ci(r, a.src[i]); });
            return m;
        });
    }

    // ------------------------------------------------------------------ Show / print
    function args() {
        return { fromDate: el('txtDateFrom').value, toDate: el('txtToDate').value, fromDocNo: A.toInt(el('txtFromDocNo').value), toDocNo: A.toInt(el('txtToDocNo').value),
                 parentCategoryId: S.selInt('CmbParentCategory'), categoryId: S.selInt('cmbItemCatgory'), itemTypeId: S.selInt('cmbItemtype'), itemClassId: S.selInt('cmbItemclass'),
                 itemId: S.selInt('cmbItem'), packUom: S.selInt('cmbUOM'), jobLotId: S.selInt('cmbJobLot'), supplierCustomerId: S.selInt('cmbSupplierCustomer'),
                 warehouseId: S.selInt('cmbWareHouse'), cityId: S.selInt('cmbCity'), activity: activityText() };
    }
    function gridFill() {                                                    // GridFill
        var b = el('show'), a = args(), token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.groupKey = ''; grid.clear(); return; }      // ClearStructure
            var act = ACTIVITIES.filter(function (x) { return x.activity === a.activity; })[0];
            if (!act) { grid.groupKey = ''; grid.clear(); return; }
            el('print').hidden = false;
            el('print366B').hidden = !act.p366B;
            grid.groupKey = act.activity === INVOICE ? 'CustomerName' : '';
            grid.cfg.frozen = act.activity === 'Sales Register' ? 1 : 0;
            grid.setData(columnsFor(act), mapRows(act, rows));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function show() {                                                        // btnShow_Click
        if (el('show').disabled) return;
        gridFill().then(printButtonManage);
    }
    function printUrl(extra) {
        var q = {}; Object.keys(lastArgs).forEach(function (k) { q[k] = lastArgs[k]; });
        Object.keys(extra || {}).forEach(function (k) { q[k] = extra[k]; });
        return '/sale/reports/saengr/print/sale-report-with-activities?' + S.qs(q);
    }
    function doPrint(btn, extra) {
        if (!hasRows || !lastArgs) { alert('Record Not Found For Display'); return; }
        A.busy(btn, true);
        A.openPdf(printUrl(extra)).catch(function (e) { alert(e.message); }).then(function () { A.busy(btn, false); });
    }
    function print() { var b = el('print'); if (!b.disabled) doPrint(b, {}); }          // btnPrint_Click (the template of the activity of the last Show)
    function print366B() { var b = el('print366B'); if (!b.disabled) doPrint(b, { kind: '366B' }); }
    function reset() {                                                       // btnNew_Click
        ['txtFromDocNo', 'txtToDocNo'].forEach(function (id) { el(id).value = ''; });
        ['cmbItemclass', 'CmbParentCategory', 'cmbItemCatgory', 'cmbItemtype', 'cmbWareHouse', 'cmbJobLot', 'cmbItem', 'cmbSupplierCustomer', 'cmbUOM', 'cmbCity'].forEach(function (id) {
            var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true }));
        });
        S.activate('cmbActivity', 0, false);
        S.focus('cmbDateType');
        datetypefill();
    }
    function refresh() {                                                     // btnRefresh_Click
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        allComboBind().then(itemUomFill).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    ['txtFromDocNo', 'txtToDocNo'].forEach(E.digits);
    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print);
    el('print366B').addEventListener('click', print366B);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && el('grid').contains(e.relatedTarget)) S.focus('cmbDateType'); });   // btnShow_Leave
    S.keys({ s: show, e: closeForm, r: refresh, n: reset, F5: function () { S.focus('cmbDateType'); }, ArrowUp: function () { S.focus('cmbDateType'); },
             ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);
    d.addEventListener('keydown', function (e) {                             // Alt+1 .. Alt+9: the print of that template only when the Print button shows it
        if (!e.altKey || e.ctrlKey) return;
        var k = e.code && /^(Digit|Numpad)[1-9]$/.test(e.code) ? e.code.slice(-1) : '';
        if (!k) return;
        e.preventDefault();
        if (printText === ALT_PRINT[k]) print();
    });

    // ------------------------------------------------------------------ start (Load)
    el('txtDateFrom').value = A.today(); el('txtToDate').value = A.today();
    grid.render();
    activityFill(); datetypefill(); itemUomFill();
    S.activate('cmbActivity', 0, false);                                     // cmbActivity.Rows[0].Activate()
    printButtonManage();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        bindCombos(data);                                                    // AllComboBind
        S.activate('cmbDateType', 2, true);                                  // the year start is known now: re-apply the date type rule
        S.focus('cmbDateType');
    }).catch(function (e) { alert(e.message); });
}(window, document));
