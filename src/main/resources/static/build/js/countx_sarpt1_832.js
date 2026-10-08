/* ============================================================================================
 * Screen 832 GdnRegister - Architecture.WinApp.Pcc.Reports.GdnRegister (.cs)
 * Page: templates/sale/reports/sarpt1_gdn_register.html (route /sale/reports/gdn-register).
 *   LoadInvoices_Load :152      FromDate = Start_Period, ComboFill, ParameterFill (Rows[1] = This Week, which sets From = today - 7), StatusFill (Rows[0] = UnApproved), FillDetailDataGrid
 *   StatusFill :172             1 UnApproved, 2 Approve, 3 All (ZeroIndex false)
 *   cmbperemeter_ValueChanged :212   1 From = today | 2 today - 7 | 3 first of UTC month, To = today | 4 To = 1 Jan, From = today (as the desktop, swapped) | 5 Start_Period
 *   ComboFill :249              [pcc].[USP_DropDownFillFromSaleOrder] split by Activity (ParentCategory, ItemCategory, ItemType, Item, Customer; ReferenceName), ZeroIndex false
 *   btngrnlod_Click :397        tab "Detail Data" -> FillDetailDataGrid :416, tab "Main Detail Data" -> FillMainGrid :594; both InvGdn.GdnRegister = [pcc].[USP_GdnRegister]
 *                               (the main grid sends no item / category / type / parent category filter), Radio FOC / Not FOC -> the FOC filter
 *   grdDetailDataSettings :531  Id, DetailId, DocumentTypeId, SupplierCustomerId, DeliveryOrderId, DoOrderDetailId, OrderId, OrderDetailId hidden; captions; red rows when IsFOC;
 *                               ItemQty "#,##0.##" (total "#,##0.###"), NetWeight "##,#.##", Carriage stringFormatsingle (all summed); GridAutoAdjustmentNew; Print (60) and Attached
 *                               added as columns 0 and 1; FrozenColumns 2
 *   grdSettings :697 / grdMain_LinkClicked :736 / grdMain_ColumnButtonClick :753   main grid (Id distinct); Attached link; Print -> GenerateGdnSlip :1075 (1855-InvGdn_Slip)
 *   grdMain_SelectionChanged :771 / DetailGridBind :783 / GridDetailSetting :838    the lines of the selected GDN from the loaded rows (red when FOC, sums, autosize)
 *   btnReset_Click :360 (clears customer, categories, item, type, status, doc nos), btnRefresh_Click :385 (ComboFill), printRegister_Click :1031 (1855-GDNRegister.rpt),
 *   BtnRegister_Click :1053 (1855A-GDNRegister.rpt), MakeShortCutKeys :944, KeyDown :980
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/sarpt1/gdn-register';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null };
    var allRows = [], lastArgs = null, tab = 0, seq = 0;
    var SHORTCUTS = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Show'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"],
        ['Ctrl+P', 'For Print 1855A-Register'], ['Ctrl+F5', 'For Focus on From Date'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On From Date']];

    function radio(name) { var x = d.querySelector('input[name="' + name + '"]:checked'); return x ? x.value : ''; }
    function isTrue(v) { return v === true || v === 1 || v === '1' || String(v).toLowerCase() === 'true'; }
    function foc(r) { return isTrue(A.ci(r, 'IsFOC')); }

    // ------------------------------------------------------------------ grids
    function attached(row) { alert('The attachment viewer is not available on the web yet (GetNoofAttachmentsByRefDocumentTypeID ' + A.toInt(row.Id) + ', 1853).'); }
    function printSlip(row) {                                                // GenerateGdnSlip(Id)
        A.openPdf('/reports/print/1855-inv-gdn-slip?' + S.qs({ id: A.toInt(row.Id) })).catch(function (e) { alert(e.message); });
    }
    var red = function (r) { return r._foc ? { color: 'red' } : null; };
    var grdD = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, frozen: 2, autosize: true, rowStyle: red,
        onLink: function (row, col) { if (col && col.key === 'NoOfAttachments') attached(row); }, onButton: function () { /* Print on the detail grid has no handler on the desktop */ } });
    var grdMain = new S.Grid({ tableId: 'resultsMain', gridId: 'gridMain', navId: 'navMain', navTextId: 'navTextMain', headerLines: 2, autosize: true,
        onLink: function (row, col) { if (col && col.key === 'NoOfAttachments') attached(row); }, onButton: function (row) { printSlip(row); }, onSelect: bindDetail });
    var grdLines = new S.Grid({ tableId: 'resultsSub', gridId: 'gridSub', navId: 'navSub', navTextId: 'navTextSub', headerLines: 2, autosize: true, rowStyle: red });

    var DT = 'dd-MMM-yy', DTT = 'dd-MMM-yy hh:mm tt';
    function approvedText(v) { return v === null || v === undefined || v === '' ? '' : (isTrue(v) ? 'Approved' : (String(v).toLowerCase() === 'false' || v === 0 || v === '0' ? 'Not Approved' : String(v))); }
    function detailColumns() {
        var f = S.fmtSingle(lookup.amountDecimals);
        return [
            { key: '_Print', caption: 'Print', width: 60, button: 'Print' },
            { key: 'NoOfAttachments', caption: 'Attached', link: true, align: 'c' },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'DetailId', caption: 'DetailId', hidden: true }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'DocNo', caption: 'DocNo' }, { key: 'DocDate', caption: 'DocDate', date: DT },
            { key: 'DeliveryOrderId', caption: 'DeliveryOrderId', hidden: true }, { key: 'DoOrderDetailId', caption: 'DoOrderDetailId', hidden: true },
            { key: 'DeliveryOrderNo', caption: 'DoNo' }, { key: 'DeliveryOrderDate', caption: 'DoDate', date: DT },
            { key: 'OrderId', caption: 'OrderId', hidden: true }, { key: 'OrderDetailId', caption: 'OrderDetailId', hidden: true },
            { key: 'SaleOrderNo', caption: 'SoNo' }, { key: 'SaleOrderDate', caption: 'SoDate', date: DT },
            { key: 'SupplierCustomerId', caption: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'CustomerName' },
            { key: 'ReferenceNo', caption: 'ReferenceNo' }, { key: 'ReferencPartyName', caption: 'ReferencPartyName' }, { key: 'ReferencPartyAddress', caption: 'ReferencPartyAddress' },
            { key: 'ReferencPartyCellNo', caption: 'ReferencPartyCellNo' }, { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'GpNo', caption: 'GpNo' }, { key: 'GpDate', caption: 'GpDate', date: DT },
            { key: 'DriverName', caption: 'DriverName' }, { key: 'DriverCellNo', caption: 'DriverCellNo' }, { key: 'DriverCNICNo', caption: 'DriverCNICNo' },
            { key: 'WareHouseName', caption: 'WareHouseName' }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName' },
            { key: 'ItemArrtibuteVarient', caption: 'Varient' }, { key: 'JobLotDescription', caption: 'JobLot' }, { key: 'CityName', caption: 'CityName' },
            { key: 'NetWeight', caption: 'NetWeight', fmt: '##,#.##', totalFmt: '##,#.##', sum: true, num: true },
            { key: 'ItemQty', caption: 'ItemQty', fmt: '#,##0.##', totalFmt: '#,##0.###', sum: true, num: true },
            { key: 'IsFOC', caption: 'IsFOC', align: 'c' },
            { key: 'TransporterName', caption: 'CarriageAccount' },
            { key: 'FrieghtAmountHeader', caption: 'Carriage', fmt: f, totalFmt: f, sum: true, num: true },
            { key: 'DeliveryTerm', caption: 'DeliveryTerm' }, { key: 'VisitedByName', caption: 'VisitedByName' }, { key: 'RefSalesMan', caption: 'RefSalesMan' },
            { key: 'IsApproved', caption: 'IsApproved' },
            { key: 'EntryDate', caption: 'EntryDate', date: DTT }, { key: 'EntryUserName', caption: 'EntryUserName' },
            { key: 'ModifyDate', caption: 'ModifyDate', date: DTT }, { key: 'ModifyUserName', caption: 'ModifyUserName' },
            { key: 'ApprovedDate', caption: 'ApprovedDate', date: DTT }, { key: 'ApprovedUserName', caption: 'ApprovedUserName' },
            { key: 'BuildingStorey', caption: 'BuildingStorey' }, { key: 'BuildingHeight', caption: 'BuildingHeight' }, { key: 'BuildingArea', caption: 'BuildingArea' }
        ];
    }
    function mainColumns() {
        var f = S.fmtSingle(lookup.amountDecimals);
        return [
            { key: '_Print', caption: 'Print', width: 60, button: 'Print' },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'DocNo', caption: 'DocNo' }, { key: 'DocDate', caption: 'DocDate', date: DT },
            { key: 'DeliveryOrderId', caption: 'DeliveryOrderId', hidden: true }, { key: 'DeliveryOrderNo', caption: 'DoNo' }, { key: 'DeliveryOrderDate', caption: 'DoDate', date: DT },
            { key: 'OrderId', caption: 'OrderId', hidden: true }, { key: 'SaleOrderNo', caption: 'SoNo' }, { key: 'SaleOrderDate', caption: 'SoDate', date: DT },
            { key: 'SupplierCustomerId', caption: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'CustomerName' },
            { key: 'ReferenceNo', caption: 'ReferenceNo' }, { key: 'ReferencPartyName', caption: 'ReferencPartyName' }, { key: 'ReferencPartyAddress', caption: 'ReferencPartyAddress' },
            { key: 'ReferencPartyCellNo', caption: 'ReferencPartyCellNo' }, { key: 'VehicleNo', caption: 'VehicleNo' }, { key: 'GpNo', caption: 'GpNo' }, { key: 'GpDate', caption: 'GpDate', date: DT },
            { key: 'DriverName', caption: 'DriverName' }, { key: 'DriverCellNo', caption: 'DriverCellNo' }, { key: 'DriverCNICNo', caption: 'DriverCNICNo' },
            { key: 'VisitedByName', caption: 'VisitedByName' }, { key: 'RefSalesMan', caption: 'RefSalesMan' }, { key: 'OrderExpiryDate', caption: 'OrderExpiryDate', date: DT },
            { key: 'DeliveryTerm', caption: 'DeliveryTerm' }, { key: 'TransporterName', caption: 'CarriageAccount' },
            { key: 'FrieghtAmountHeader', caption: 'Carriage', fmt: f, totalFmt: f, sum: true, num: true },
            { key: 'RemarksHeader', caption: 'RemarksHeader' }, { key: 'IsApproved', caption: 'IsApproved' },
            { key: 'EntryDate', caption: 'EntryDate', date: DTT }, { key: 'EntryUserName', caption: 'EntryUserName' },
            { key: 'ModifyDate', caption: 'ModifyDate', date: DTT }, { key: 'ModifyUserName', caption: 'ModifyUserName' },
            { key: 'ApprovedDate', caption: 'ApprovedDate', date: DTT }, { key: 'ApprovedUserName', caption: 'ApprovedUserName' },
            { key: 'BuildingStorey', caption: 'BuildingStorey' }, { key: 'BuildingHeight', caption: 'BuildingHeight' }, { key: 'BuildingArea', caption: 'BuildingArea' },
            { key: 'NoOfAttachments', caption: 'NoOfAttachments', link: true }
        ];
    }
    function linesColumns() {
        return [
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'DetailId', caption: 'DetailId', hidden: true }, { key: 'DeliveryOrderId', caption: 'DeliveryOrderId', hidden: true },
            { key: 'DoOrderDetailId', caption: 'DoOrderDetailId', hidden: true }, { key: 'OrderId', caption: 'OrderId', hidden: true }, { key: 'OrderDetailId', caption: 'OrderDetailId', hidden: true },
            { key: 'ItemId', caption: 'ItemId', hidden: true }, { key: 'SaleGLAC', caption: 'SaleGLAC', hidden: true },
            { key: 'WareHouseName', caption: 'WareHouseName' }, { key: 'ItemCode', caption: 'ItemCode' }, { key: 'ItemName', caption: 'ItemName' },
            { key: 'ItemArrtibuteVarient', caption: 'Varient' }, { key: 'VarientEquivalent', caption: 'VarientUnit', fmt: '##,#.##', totalFmt: '##,#.##', sum: true, num: true },
            { key: 'JobLotDescription', caption: 'JobLot' },
            { key: 'ItemQty', caption: 'ItemQty', fmt: '##,#.##', totalFmt: '##,#.##', sum: true, num: true },
            { key: 'ItemWeight', caption: 'ItemWeight', fmt: '##,#.##', totalFmt: '##,#.##', sum: true, num: true },
            { key: 'NetWeight', caption: 'NetWeight', fmt: '##,#.##', totalFmt: '##,#.##', sum: true, num: true },
            { key: 'CityName', caption: 'CityName' }, { key: 'IsFOC', caption: 'IsFOC', align: 'c' }, { key: 'RemarksDetail', caption: 'RemarksDetail' }
        ];
    }
    function prep(r) {                                                         // a plain copy with the display only fields
        var o = {}; Object.keys(r).forEach(function (k) { o[k] = r[k]; });
        o._foc = foc(r);
        o.IsFOC = o._foc ? '✔' : '';
        o.IsApproved = approvedText(A.ci(r, 'IsApproved'));
        return o;
    }
    function bindDetail(row) {                                                 // DetailGridBind: the loaded rows of the selected GDN
        if (!row) { grdLines.clear(); return; }
        var id = A.toInt(row.Id);
        var rows = allRows.filter(function (r) { return A.toInt(A.ci(r, 'Id')) === id; });
        if (!rows.length) { grdLines.clear(); return; }
        grdLines.setData(linesColumns(), rows.map(prep));
    }

    // ------------------------------------------------------------------ Show
    function args(mode) {
        var a = { fromDate: el('FromDate').value, toDate: el('Todate').value, fromDocNo: A.toIntText(el('txtOrderNoFrom').value), toDocNo: A.toIntText(el('txtOrderNoTo').value),
                  supplierCustomerId: S.selInt('CmbSupplier'), statusId: S.selInt('cmbApproveStatus'), foc: radio('rdFoc'), mode: mode };
        if (mode === 'detail') {
            a.itemId = S.selInt('cmbItem'); a.itemCategoryId = S.selInt('cmbCategory'); a.parentCategoryId = S.selInt('cmbParentCategory'); a.itemTypeId = S.selInt('cmbItemtype');
        }
        return a;
    }
    function show() {
        var b = el('show'); if (b.disabled) return Promise.resolve();
        var mode = tab === 0 ? 'detail' : 'main', a = args(mode), token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            allRows = rows || []; lastArgs = allRows.length ? a : null;
            if (mode === 'detail') {
                if (!allRows.length) grdD.clear(); else grdD.setData(detailColumns(), allRows.map(prep));
            } else if (!allRows.length) { grdMain.clear(); grdLines.clear(); }
            else {
                var seen = {}, heads = [];
                allRows.forEach(function (r) { var id = A.toInt(A.ci(r, 'Id')); if (!seen[id]) { seen[id] = 1; heads.push(prep(r)); } });
                grdMain.setData(mainColumns(), heads);
                grdMain.select(0);
            }
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function print(kind) {
        var b = el(kind === '1855A' ? 'print1855A' : 'print1855'); if (b.disabled) return;
        if (!allRows.length || !lastArgs) { alert('Record Not Found For Display'); return; }
        A.busy(b, true);
        var q = {}; Object.keys(lastArgs).forEach(function (k) { q[k] = lastArgs[k]; }); q.kind = kind;
        A.openPdf('/sale/reports/sarpt1/print/gdn-register?' + S.qs(q)).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ lists / date type
    function bindCombos(data) {
        var c = (data && data.combos) || {};
        S.fill(el('cmbParentCategory'), c.ParentCategory || [], false, true);
        S.fill(el('cmbCategory'), c.ItemCategory || [], false, true);
        S.fill(el('cmbItemtype'), c.ItemType || [], false, true);
        S.fill(el('cmbItem'), c.Item || [], false, true);
        S.fill(el('CmbSupplier'), c.Customer || [], false, true);
    }
    function statusFill() { S.fill(el('cmbApproveStatus'), [{ Id: 1, name: 'UnApproved' }, { Id: 2, name: 'Approve' }, { Id: 3, name: 'All' }], false); S.activate('cmbApproveStatus', 0, false); }
    function parameterFill() { S.fill(el('cmbperemeter'), S.dateTypeRows(false), false); S.activate('cmbperemeter', 1, false); }
    el('cmbperemeter').addEventListener('change', function () { S.dateRule(A.toInt(this.value), el('FromDate'), el('Todate'), lookup.yearStart, { swap4: true }); });

    function reset() {                                                         // btnReset_Click
        S.focus('FromDate');
        ['CmbSupplier', 'cmbParentCategory', 'cmbCategory', 'cmbItem', 'cmbItemtype', 'cmbApproveStatus'].forEach(function (id) { var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true })); });
        el('txtOrderNoFrom').value = ''; el('txtOrderNoTo').value = '';
    }
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; bindCombos(data); }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }
    function selectTab(n) {
        tab = n;
        el('pageDetail').hidden = n !== 0; el('pageMain').hidden = n !== 1;
        el('tabD').classList.toggle('on', n === 0); el('tabM').classList.toggle('on', n === 1);
    }

    S.digitsOnly('txtOrderNoFrom'); S.digitsOnly('txtOrderNoTo');
    el('tabD').addEventListener('click', function () { selectTab(0); });
    el('tabM').addEventListener('click', function () { selectTab(1); });
    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print1855').addEventListener('click', function () { print('1855'); });
    el('print1855A').addEventListener('click', function () { print('1855A'); });
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    S.keys({ s: show, l: show, e: closeForm, n: reset, r: refresh, p: function () { print('1855A'); }, F5: function () { S.focus('FromDate'); }, ArrowUp: function () { S.focus('FromDate'); },
             ArrowDown: function () { (tab === 0 ? grdD : grdMain).focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('FromDate').value = A.today(); el('Todate').value = A.today();
    grdD.render(); grdMain.render(); grdLines.render(); selectTab(0);
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        S.focus('FromDate');
        if (lookup.yearStart) el('FromDate').value = S.isoDay(lookup.yearStart);   // FromDate = Start_Period
        bindCombos(data);                                                          // ComboFill
        parameterFill();                                                           // ParameterFill (This Week: From = today - 7)
        statusFill();                                                              // StatusFill
        return show();                                                             // FillDetailDataGrid
    }).catch(function (e) { alert(e.message); });
}(window, document));
