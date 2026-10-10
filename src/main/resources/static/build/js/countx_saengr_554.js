/* ============================================================================================
 * Screen 554 frmSaleOrderHistory_Engr - Architecture.WinApp.Inventory_Reports.frmSaleOrderHistory_Engr (.cs, 4691 lines)
 * Page: templates/sale/reports/saengr_sale_order_history.html (route /sale/reports/engr/sale-order-history).
 *   frmSaleOrderHistory_Load :268   AllDropDownBind, StatusFill, OrderStatusFill, ActivityFill, both From dates = Start_Period, PrintButtonManage, Detail Register tab with the
 *                                   "Brief" sub tab selected (tabControl1.SelectedIndex = 1), Approved Status and Status row 0
 *   AllDropDownBind :434            SaleOrder.GetDataForDropDownFromSaleOrder = USP_GetDataForDropDownFromSaleOrder split by Activity: Customer, Item, JobLot, City, ParentCategory,
 *                                   ItemCategory, ItemType (ZeroIndex false); the detail and the summary tab each get their own copy of the lists
 *   StatusFill :578 / OrderStatusFill :597 / ActivityFill :1639   fixed lists: UnApporve | Approve | All;  Open | Cancel | Complete;  the 11 report types
 *   gridHisory :1323 (btnshow)      SaleOrder.SaleOrderDetailRegister_Eng = USP_SaleOrderDetailRegister_Eng. The rows feed three grids: grdsaleorder (35 columns, the Detail tab),
 *                                   GrdMain (one row per order, the quantities and amounts are Conversion.ToInt of the per-order sums) and GrdDetail (the items of the GrdMain row)
 *   GridSettings :1237 / MainGridSetting :1152 / GridDetailSetting :1531   hidden columns, formats, summed columns; button columns Complete / Cancel (Approved Status "Approve"),
 *                                   IsApproved (hidden), UpdateExpiryDate (Status "Open" and "Approve"; the ExpiryDate cell is editable). GrdMain shows them only with the
 *                                   CanChangeOrderStatusToComplete / CanChangeOrderStatusToCancel / CanChangeOrderExpiryDate rights
 *   CompleteStatus :893 / CancelStatus :927 / UpdateExpiryDate :966   confirm, SaleOrder.UpdateStatusandIsApprovedbyOrderId (ReqType Status | Cancel | UpdateExpiryDate), message, gridHisory
 *   *_LinkClicked :1041 :1511       DocNo -> CommonServices.SaleOrderSlipEngr_1605(Id) (1605-SaleOrderSlipAndRegister_Engr.rpt); CustomerName -> the general ledger of the customer
 *   btnNew_Click :549 (resetDetailRegister :521)   clears the lists / So numbers / Include approved Do, Approved Status and Status row 0, then Show (the dates stay)
 *   print_Click :717                299-SalesOrderRegistery.rpt over the last Show (a failure always says "Record Not Found")
 *   btnRefreshDetail_Click :566 / btnRefreshSummary_Click :1666   AllDropDownBind
 *   Summary tab: BtnShowSumm_Click :1703 -> GridSummaryFill :1724 (SaleOrder.SaleOrderSummaryRegister = USP_SaleOrderSummaryRegister, IsOnQty 1; the rows are copied positionally into
 *                                   the table of the chosen report type), DataGridHistorySetting :1911, PrintButtonManage :2152, btnPrintSummary_Click :1987 (1605_nn_*.rpt by the
 *                                   Print button text), btnNewSumm_Click :1678 (Reset), KeyDown :761
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, E = w.SaEngr, el = A.el;
    var API = '/api/sale/saengr/order-history';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null, userBranchId: 0, rights: {} };
    var detailRaw = [], detailArgs = null, summaryArgs = null, summaryCount = 0, seqD = 0, seqS = 0;
    var printText = 'Print';
    var GENERAL = '0.##########';

    // ------------------------------------------------------------------ small column helpers
    function H(key) { return { key: key, caption: key, hidden: true }; }
    function T(key, o) { var c = { key: key, caption: key }; if (o) for (var k in o) c[k] = o[k]; return c; }
    function DT(key) { return { key: key, caption: key }; }                      // already formatted by fixDates
    function D(key) { return { key: key, caption: key, date: 'dd/MM/yyyy' }; }   // ToString("dd-MMM-yy") then parsed: midnight, date only
    function N(key, fmt, o) { var c = { key: key, caption: key, num: true, fmt: fmt, sum: true, totalFmt: fmt }; if (o) for (var k in o) c[k] = o[k]; return c; }
    function G(key, total) { return { key: key, caption: key, num: true, fmt: GENERAL, sum: true, totalFmt: total }; }   // no FormatString, only the total format
    function B(key, caption, text, width) { return { key: key, caption: caption, width: width, button: text }; }

    // ------------------------------------------------------------------ lists
    function bindCombos(data) {
        var c = (data && data.combos) || {};
        if (!Object.keys(c).length) return;                                        // AllDropDownBind binds only when the procedure gives rows
        function rows(k) { return c[k] || []; }
        S.fill(el('CmbSupplier'), rows('Customer'), false); S.fill(el('cmbCustomerSumm'), rows('Customer'), false);
        S.fill(el('CmbItem'), rows('Item'), false); S.fill(el('cmbItemNameSumm'), rows('Item'), false);
        S.fill(el('cmbJobLotSumm'), rows('JobLot'), false);
        S.fill(el('cmbcitySumm'), rows('City'), false);
        S.fill(el('cmbParentcategorySumm'), rows('ParentCategory'), false); S.fill(el('CmbParentCategory'), rows('ParentCategory'), false);
        S.fill(el('CmbItemCategory'), rows('ItemCategory'), false); S.fill(el('CmbCategorySumm'), rows('ItemCategory'), false);
        S.fill(el('CmbItemType'), rows('ItemType'), false); S.fill(el('cmbItemtypeSumm'), rows('ItemType'), false);
    }
    function allDropDownBind() {
        return A.getJson(API + '/lookups').then(function (data) {
            lookup = data || lookup; lookup.rights = lookup.rights || {};
            bindCombos(data);
        }).catch(function (e) { alert(e.message); });
    }
    function statusFill() {
        S.fill(el('CmbIsApproved'), [{ Id: 1, name: 'UnApporve' }, { Id: 2, name: 'Approve' }, { Id: 3, name: 'All' }], false);
        S.activate('CmbIsApproved', 0, false);
    }
    function orderStatusFill() {
        S.fill(el('CmbOrderStatus'), [{ Id: 1, name: 'Open' }, { Id: 2, name: 'Cancel' }, { Id: 3, name: 'Complete' }], false);
        S.activate('CmbOrderStatus', 0, false);
    }
    var ACTIVITY_NAMES = ['Order Register', 'Order Summary By Item', 'Order Summary By Item & Pack Size', 'Order Summary By Item & City', 'Order Summary By Item,Pack Size & City',
        'Order Summary By Customer', 'Order Summary By Customer & OrderNo', 'Order Summary By Customer & Item', 'Order Summary By Customer & Pack Size',
        'Order Summary By Customer & City', 'Order Summary By Customer,Item & City'];
    function activityFill() {
        S.fill(el('cmbActivitySumm'), ACTIVITY_NAMES.map(function (n, i) { return { Id: i + 1, name: n }; }), false);
        S.activate('cmbActivitySumm', 0, false);
    }

    // ------------------------------------------------------------------ the three grids of the Detail Register tab
    function slip(id) {                                                            // CommonServices.SaleOrderSlipEngr_1605(Id)
        if (typeof w.printRpt !== 'function') { alert('Printing is not available on this page.'); return; }
        w.printRpt('1605-SaleOrderSlipAndRegister_Engr.rpt', { id: id, branchesId: A.toInt(lookup.userBranchId), documentTypeIds: '1605' });
    }
    function ledger(supplierCustomerId) {                                          // GoToGeneralLedgerFromLinkedEvent(.., fromdate.Value, ToDate.Value)
        E.ledger(API, supplierCustomerId, el('fromdate').value, el('ToDate').value).catch(function (e) { alert(e.message); });
    }
    function rowExpiry(row) { return row._exp != null ? row._exp : S.isoDay(row.ExpiryDate); }

    var gA = new S.Grid({
        tableId: 'resA', gridId: 'gridA', navId: 'navA', navTextId: 'navTextA', headerLines: 2, autosize: true,
        onLink: function (row, col) {
            if (!col) return;
            if (col.key === 'DocNo') slip(A.toInt(row.Id)); else if (col.key === 'CustomerName') ledger(A.toInt(row.OrderSupCustId));
        },
        onButton: function (row, col) {
            var id = A.toInt(row.Id);
            if (col.key === 'Complete') completeStatus(id);
            else if (col.key === 'Cancel') cancelStatus(id, A.toDouble(row.DispatchQty));
            else if (col.key === 'Update') updateExpiryDate(id, rowExpiry(row));
        }
    });
    var gB = new S.Grid({
        tableId: 'resB', gridId: 'gridB', navId: 'navB', navTextId: 'navTextB', headerLines: 2, autosize: true,
        onSelect: function (row) { fillBrief(row); },                              // GrdMain_SelectionChanged
        onLink: function (row, col) {
            if (!col) return;
            if (col.key === 'DocNo') slip(A.toInt(row.Id)); else if (col.key === 'CustomerName') ledger(A.toInt(row.SupplierCustomerId));
        },
        onButton: function (row, col) {
            var id = A.toInt(row.Id);
            if (col.key === 'Complete') { if (S.selText('CmbIsApproved') === 'Approve') completeStatus(id); }
            else if (col.key === 'Cancel') cancelStatus(id, A.toDouble(row.DispatchQty));
            else if (col.key === 'Update') updateExpiryDate(id, rowExpiry(row));
        }
    });
    var gC = new S.Grid({ tableId: 'resC', gridId: 'gridC', navId: 'navC', navTextId: 'navTextC', headerLines: 2, autosize: true });
    var gD = new S.Grid({ tableId: 'resD', gridId: 'gridD', navId: 'navD', navTextId: 'navTextD', headerLines: 2, autosize: true });

    /* GridEX AllowEdit on the ExpiryDate column (EditType 2 = calendar): the cell becomes a date picker, the value is read by the UpdateExpiryDate button */
    function makeExpiryEditable(grid) {
        var base = grid.render;
        grid.editExpiry = false;
        grid.render = function () {
            base.call(grid);
            if (!grid.editExpiry || !grid.columns) return;
            var cols = grid.visibleCols(), at = -1;
            cols.forEach(function (c, i) { if (c.key === 'ExpiryDate') at = i; });
            if (at < 0) return;
            grid.flat.forEach(function (f) {
                var td = f.tr.cells[at]; if (!td) return;
                var inp = d.createElement('input'); inp.type = 'date'; inp.className = 'tb';
                inp.style.cssText = 'position:static;width:100%;height:18px;box-sizing:border-box;font:11px Verdana, Tahoma, sans-serif;';
                inp.value = rowExpiry(f.row);
                inp.addEventListener('change', function () { f.row._exp = inp.value; });
                inp.addEventListener('mousedown', function (e) { e.stopPropagation(); });
                td.textContent = ''; td.appendChild(inp);
            });
        };
    }
    makeExpiryEditable(gA); makeExpiryEditable(gB);

    /* the detail table (gridHisory): positional copy of grdlst into the 35 column table */
    var MAP_A = [['Id', 'Id'], ['DocumentTypeId', 'DocumentTypeId'], ['DocNo', 'DocNo'], ['DocDate', 'DocDate'], ['OrderSupCustId', 'OrderSupCustId'], ['CustomerName', 'CustomerName'],
        ['ReferenceNo', 'SupplierRefNo'], ['PaymentTerm', 'TermsDescription'], ['DeliveryTerm', 'DeliveryTerm'], ['ItemId', 'OrderItemId'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'],
        ['ItemDescription', 'ItemDiscription'], ['PackUom', 'UOMCodeItm'], ['ItemQty', 'OrderItemQty'], ['DispatchQty', 'DispatchQty'], ['BalQty', 'BalQty'], ['ItemRate', 'OrderItemRate'],
        ['ItemAmount', 'Amount'], ['DispatchAmount', 'DispatchAmount'], ['BalAmount', 'BalAmount'], ['CityName', 'CityName'], ['DetailRemarks', 'OrderRemarks'], ['DueDays', 'OrderDueDays'],
        ['DueDate', 'OrderDueDate'], ['OrderStatus', 'OrderStatus'], ['ApprovedStatus', 'ApprovedStatus'], ['ExpiryDate', 'OrderExpiryDate'], ['EntryDate', 'EntryDate'],
        ['EntryUser', 'UserNameEusr'], ['ModifyDate', 'ModifyDate'], ['ModifyUser', 'UserNameMusr'], ['ApprovedDate', 'PostDate'], ['ApprovedUser', 'UserNameAusr'], ['Remarks', 'RemarksHeader']];
    function mapRows(raw, map) {
        return raw.map(function (r) { var o = {}; map.forEach(function (m) { var v = A.ci(r, m[1]); o[m[0]] = v === undefined ? null : v; }); return o; });
    }
    function colsA(approval, status) {
        var single = S.fmtSingle(lookup.amountDecimals), rate = S.fmtRate(lookup.rateDecimals);
        var c = [H('Id'), H('DocumentTypeId'), T('DocNo', { link: true }), D('DocDate'), H('OrderSupCustId'), T('CustomerName', { link: true }), T('ReferenceNo'), T('PaymentTerm'), T('DeliveryTerm'),
            H('ItemId'), T('ItemCode'), T('ItemName'), T('ItemDescription'), T('PackUom'),
            N('ItemQty', '#,##0.###'), N('DispatchQty', '#,##0.###'), G('BalQty', '#,##0.###'), { key: 'ItemRate', caption: 'ItemRate', num: true, fmt: rate },
            N('ItemAmount', single), N('DispatchAmount', single), N('BalAmount', single), T('CityName'), T('DetailRemarks'), T('DueDays'), D('DueDate'), T('OrderStatus'), T('ApprovedStatus'),
            D('ExpiryDate'), DT('EntryDate'), T('EntryUser'), DT('ModifyDate'), T('ModifyUser'), DT('ApprovedDate'), T('ApprovedUser'), T('Remarks')];
        if (approval === 'Approve') { c.push(B('Complete', 'Complete', 'Complete', 80)); c.push(B('Cancel', 'Cancel', 'Cancel', 60)); }
        if (approval === 'UnApporve') { var hb = B('IsApproved', 'IsApproved', 'IsApproved', 80); hb.hidden = true; c.push(hb); }
        if (status === 'Open' && approval === 'Approve') c.push(B('Update', 'UpdateExpiryDate', 'UpdateExpiryDate', 120));
        return c;
    }
    function colsB(approval, status) {
        var single = S.fmtSingle(lookup.amountDecimals), q4 = '#,##0.####', r = lookup.rights || {};
        var c = [H('Id'), H('DocumentTypeId'), T('DocNo', { link: true }), D('DocDate'), H('SupplierCustomerId'), T('CustomerName', { link: true }), T('OrderStatus'), T('ApprovedStatus'),
            T('PaymentTerm'), T('DueDays'), D('DueDate'), T('DeliveryTerm'), N('ItemQty', q4), N('DispatchQty', q4), N('BalQty', q4), N('OrderAmount', single), N('DispatchAmount', single),
            N('BalAmount', single), D('ExpiryDate'), DT('EntryDate'), T('EntryUser'), DT('ModifyDate'), T('ModifyUser'), DT('ApprovedDate'), T('ApprovedUser'), T('Remarks')];
        var edit = false;
        if (approval === 'Approve' && status === 'Open') {
            if (r.CanChangeOrderStatusToComplete) c.push(B('Complete', 'Complete', 'Complete', 80));
            if (r.CanChangeOrderStatusToCancel) c.push(B('Cancel', 'Cancel', 'Cancel', 60));
            if (r.CanChangeOrderExpiryDate) { edit = true; c.push(B('Update', 'UpdateExpiryDate', 'UpdateExpiryDate', 120)); }
        }
        if (approval === 'UnApporve') { var hb = B('IsApproved', 'IsApproved', 'IsApproved', 60); hb.hidden = true; c.push(hb); }
        return { cols: c, edit: edit };
    }
    var DATETIME_KEYS = ['EntryDate', 'ModifyDate', 'ApprovedDate'];

    /* dtMain: one row per Id (first occurrence), the sums are Conversion.ToInt(Compute("Sum(..)", "Id = ..")) */
    function mainRows(raw) {
        var seen = {}, out = [];
        function sum(id, col) {
            var s = 0; raw.forEach(function (r) { if (String(A.ci(r, 'Id')) === id) s += A.toDouble(A.ci(r, col)); });
            return E.toInt32(s);
        }
        raw.forEach(function (r) {
            var id = String(A.ci(r, 'Id') == null ? '' : A.ci(r, 'Id'));
            if (seen[id]) return; seen[id] = true;
            function v(k) { var x = A.ci(r, k); return x === undefined ? null : x; }
            out.push({ Id: id, DocumentTypeId: v('DocumentTypeId'), DocNo: v('DocNo'), DocDate: v('DocDate'), SupplierCustomerId: v('OrderSupCustId'), CustomerName: v('CustomerName'),
                OrderStatus: v('OrderStatus'), ApprovedStatus: v('ApprovedStatus'), PaymentTerm: v('TermsDescription'), DueDays: v('OrderDueDays'), DueDate: v('OrderDueDate'),
                DeliveryTerm: v('DeliveryTerm'), ItemQty: sum(id, 'OrderItemQty'), DispatchQty: sum(id, 'DispatchQty'), BalQty: sum(id, 'BalQty'), OrderAmount: sum(id, 'Amount'),
                DispatchAmount: sum(id, 'DispatchAmount'), BalAmount: sum(id, 'BalAmount'), ExpiryDate: v('OrderExpiryDate'), EntryDate: v('EntryDate'), EntryUser: v('UserNameEusr'),
                ModifyDate: v('ModifyDate'), ModifyUser: v('UserNameMusr'), ApprovedDate: v('PostDate'), ApprovedUser: v('UserNameAusr'), Remarks: v('RemarksHeader') });
        });
        return out;
    }
    var MAP_C = [['ItemId', 'OrderItemId'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'], ['ItemDescription', 'ItemDiscription'], ['JobLot', 'JobLotCode'], ['PackUom', 'UOMCodeItm'],
        ['ItemQty', 'OrderItemQty'], ['DispatchQty', 'DispatchQty'], ['BalQty', 'BalQty'], ['ItemRate', 'OrderItemRate'], ['ItemAmount', 'Amount'], ['DispatchAmount', 'DispatchAmount'],
        ['BalAmount', 'BalAmount'], ['TaxType', 'TaxName'], ['Tax%', 'TaxPercent'], ['TaxAmount', 'TaxAmount'], ['CityName', 'CityName']];
    function fillBrief(row) {                                                      // GrdMain_SelectionChanged -> GrdDetail
        if (!row) return;
        var id = String(row.Id), single = S.fmtSingle(lookup.amountDecimals), rate = S.fmtRate(lookup.rateDecimals), q2 = '#,##0.##';
        var raw = detailRaw.filter(function (r) { return String(A.ci(r, 'Id')) === id; });
        var cols = [H('ItemId'), T('ItemCode'), T('ItemName'), T('ItemDescription'), T('JobLot'), T('PackUom'), N('ItemQty', q2), N('DispatchQty', q2), N('BalQty', q2),
            { key: 'ItemRate', caption: 'ItemRate', num: true, fmt: rate }, N('ItemAmount', single), N('DispatchAmount', single), N('BalAmount', single), T('TaxType'),
            { key: 'Tax%', caption: 'Tax%', num: true, fmt: '#,##0.####' }, N('TaxAmount', single), T('CityName')];
        gC.setData(cols, mapRows(raw, MAP_C));
    }

    // ------------------------------------------------------------------ Show (btnshow_Click -> gridHisory)
    function detailArgsNow() {
        return { fromDate: el('fromdate').value, toDate: el('ToDate').value, fromDocNo: A.toInt(el('txtSrFrom').value.trim()), toDocNo: A.toInt(el('txtsrTo').value.trim()),
                 supplierCustomerId: S.selInt('CmbSupplier'), itemId: S.selInt('CmbItem'), parentCategoryId: S.selInt('CmbParentCategory'), categoryId: S.selInt('CmbItemCategory'),
                 itemTypeId: S.selInt('CmbItemType'), status: S.selText('CmbOrderStatus'), approval: S.selText('CmbIsApproved'),
                 actionId: el('chkincludeDo').checked ? 1 : 0, skipZero: el('chkSkipZeroDetail').checked ? 1 : 0 };
    }
    function showDetail() {
        var b = el('btnshow'); if (b.disabled) return Promise.resolve();
        var a = detailArgsNow(), token = ++seqD;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seqD) return;
            detailArgs = a; detailRaw = rows || [];
            if (!detailRaw.length) { gA.clear(); gB.clear(); gC.clear(); return; }      // DataSource = null, ClearStructure
            var approval = a.approval, status = a.status;
            gA.editExpiry = approval === 'Approve' && status === 'Open';                // AllowEdit = True, ExpiryDate calendar
            gA.setData(colsA(approval, status), E.fixDates(mapRows(detailRaw, MAP_A), DATETIME_KEYS));
            var b2 = colsB(approval, status);
            gB.editExpiry = b2.edit;
            gB.setData(b2.cols, E.fixDates(mainRows(detailRaw), DATETIME_KEYS));
            gB.select(0);                                                               // the first row is current: GrdMain_SelectionChanged
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seqD) A.busy(b, false); });
    }
    function resetDetail() {                                                            // resetDetailRegister
        el('chkincludeDo').checked = false;
        ['CmbSupplier', 'CmbItem', 'CmbParentCategory', 'CmbItemCategory', 'CmbItemType'].forEach(function (id) {
            var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true }));
        });
        el('txtSrFrom').value = ''; el('txtsrTo').value = '';
        S.activate('CmbIsApproved', 0, false); S.activate('CmbOrderStatus', 0, false);
        S.focus('fromdate');
        return showDetail();
    }

    // ------------------------------------------------------------------ order actions
    function post(action, id, expiry) {
        return S.post(API + '/action', { action: action, id: id, expiryDate: expiry || null });
    }
    function completeStatus(id) {
        if (!w.confirm('Are you sure to Complete Status?')) return;
        post('Complete', id).then(function () { alert('Record Complete Successfully'); return showDetail(); }).catch(function (e) { alert(e.message); });
    }
    function cancelStatus(id, dispatchQty) {
        if (dispatchQty > 0) { alert('Record Not Cancel because Dispatch Qty gratter than zero'); resetDetail(); return; }
        if (!w.confirm('Are you sure to Cancel Status?')) return;
        post('Cancel', id).then(function () { alert('Record Cancel Successfully'); return showDetail(); }).catch(function (e) { alert(e.message); });
    }
    function updateExpiryDate(id, expiry) {
        if (!w.confirm('Are you sure to Update ExpiryDate?')) return;
        post('UpdateExpiryDate', id, expiry).then(function () { alert('Update ExpiryDate Successfully'); return showDetail(); }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------ prints
    function printDetail() {                                                            // print_Click
        var b = el('print'); if (b.disabled) return;
        if (!detailRaw.length || !detailArgs) { alert('Record Not Found'); return; }
        A.busy(b, true);
        A.openPdf('/sale/reports/saengr/print/sale-order-history?' + S.qs(Object.assign({ kind: 'detail' }, detailArgs))).catch(function () { alert('Record Not Found'); }).then(function () { A.busy(b, false); });
    }
    var TEMPLATES = ['1605_01_OrderRegister', '1605_02_OrderSummaryByItem&PackSize', '1605_03_OrderSummaryByItem,PackSize&City', '1605_04_OrderSummaryByCustomer&PackSize',
        '1605_05_OrderSummaryByItem', '1605_06_OrderSummaryByItem&City', '1605_07_OrderSummaryByCustomer', '1605_08_OrderSummaryByCustomer&City', '1605_09_OrderSummaryByCustomer&Item',
        '1605_10_OrderSummaryByCustomer,Item&City', '1605_11_OrderSummaryByCustomer&Order'];
    /* PrintButtonManage: the text of the Print button per report type. "Sales Summary By Customer & City" never equals a report type, so that one keeps the previous text. */
    var PRINT_TEXT = { 'Order Register': TEMPLATES[0], 'Order Summary By Item & Pack Size': TEMPLATES[1], 'Order Summary By Item,Pack Size & City': TEMPLATES[2],
        'Order Summary By Customer & Pack Size': TEMPLATES[3], 'Order Summary By Item': TEMPLATES[4], 'Order Summary By Item & City': TEMPLATES[5],
        'Order Summary By Customer': TEMPLATES[6], 'Order Summary By Customer & Item': TEMPLATES[8], 'Order Summary By Customer,Item & City': TEMPLATES[9],
        'Order Summary By Customer & OrderNo': TEMPLATES[10] };
    function printButtonManage() {
        var b = el('sPrint');
        var text = S.selText('cmbActivitySumm');
        if (text !== '') {
            b.hidden = false;
            if (PRINT_TEXT[text]) printText = PRINT_TEXT[text];
        } else b.hidden = true;
        b.querySelector('span').textContent = printText;
    }
    function printSummary() {                                                           // btnPrintSummary_Click
        var b = el('sPrint'); if (b.disabled) return;
        if (TEMPLATES.indexOf(printText) < 0) return;                                   // no branch for the initial "Print" text
        if (!summaryCount || !summaryArgs) { alert('Record Not Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/sale/reports/saengr/print/sale-order-history?' + S.qs(Object.assign({ kind: 'summary', template: printText }, summaryArgs))).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ Summary tab (GridSummaryFill)
    var TAIL = [['OrderQty', 'OrderQty', 'q'], ['DispatchedQty', 'DispatchQty', 'q'], ['BalQty', 'BalQty', 'q'], ['OrderAmount', 'OrderAmount', 'a'], ['DispatchedAmount', 'DispatchAmount', 'a'],
        ['BalAmount', 'BalAmount', 'a'], ['AvgRate', 'AvgRate', 'r'], ['PercentOfTotal', 'PrcntOfTotal', 'q']];
    var CITY = ['CityName', 'CityName', 's'], ITEM = [['ItemCode', 'ItemCode', 's'], ['ItemName', 'ItemName', 's']], UOM = ['PackUom', 'UOMCode', 's'], PARTY = ['PartyName', 'Customer', 's'];
    var ID = ['Id', 'Id', 'h'];
    function cat() { var o = []; for (var i = 0; i < arguments.length; i++) o = o.concat(arguments[i]); return o; }
    var SUMMARY = {
        'Order Register': cat([ID, ['DocumentTypeId', 'DocumentTypeId', 'h'], ['DocDate', 'DocDate', 'd'], ['DocNo', 'DocNo', 's'], PARTY], ITEM, [UOM, ['JobLot', 'JobLotCode', 's']], TAIL, [CITY]),
        'Order Summary By Item': cat([ID], ITEM, TAIL),
        'Order Summary By Item & Pack Size': cat([ID], ITEM, [UOM], TAIL),
        'Order Summary By Item & City': cat([ID], ITEM, TAIL, [CITY]),
        'Order Summary By Item,Pack Size & City': cat([ID], ITEM, [UOM], TAIL, [CITY]),
        'Order Summary By Customer': cat([ID, PARTY], TAIL),
        'Order Summary By Customer & OrderNo': cat([ID, PARTY, ['DocDate', 'DocDate', 'd'], ['DocNo', 'DocNo', 's']], TAIL),
        'Order Summary By Customer & Item': cat([ID, PARTY], ITEM, TAIL),
        'Order Summary By Customer & City': cat([ID, PARTY], TAIL, [CITY]),
        'Order Summary By Customer,Item & City': cat([ID, PARTY], ITEM, TAIL, [CITY]),
        'Order Summary By Customer & Pack Size': cat([ID, PARTY, UOM], TAIL)
    };
    function summaryCols(spec) {
        var single = S.fmtSingle(lookup.amountDecimals);
        return spec.map(function (m) {
            var k = m[0];
            if (m[2] === 'h') return H(k);
            if (m[2] === 'd') return DT(k);
            if (m[2] === 'q') return N(k, '#,##0.###');
            if (m[2] === 'a') return N(k, '#,##0.####');
            if (m[2] === 'r') return { key: k, caption: k, num: true, fmt: '#,##0.###' };
            return T(k);
        });
    }
    function summaryRows(raw, spec) {
        var rows = raw.map(function (r) {
            var o = {};
            spec.forEach(function (m) { var v = A.ci(r, m[1]); o[m[0]] = v === undefined ? null : v; });
            return o;
        });
        return E.fixDates(rows, spec.filter(function (m) { return m[2] === 'd'; }).map(function (m) { return m[0]; }));
    }
    function summaryArgsNow() {
        return { fromDate: el('txtFromDateSumm').value, toDate: el('txtToDateSumm').value, fromDocNo: A.toInt(el('txtFromDocNoSumm').value), toDocNo: A.toInt(el('txtToDocNoSumm').value),
                 parentCategoryId: S.selInt('cmbParentcategorySumm'), categoryId: S.selInt('CmbCategorySumm'), itemTypeId: S.selInt('cmbItemtypeSumm'),
                 supplierCustomerId: S.selInt('cmbCustomerSumm'), itemId: S.selInt('cmbItemNameSumm'), jobLotId: S.selInt('cmbJobLotSumm'), cityId: S.selInt('cmbcitySumm'),
                 activity: S.selText('cmbActivitySumm'), actionId: el('ChkDoApprovedIncludSummary').checked ? 1 : 0, skipZero: el('chkSkipZero').checked ? 1 : 0 };
    }
    function showSummary() {                                                            // BtnShowSumm_Click
        var b = el('BtnShowSumm'); if (b.disabled) return Promise.resolve();
        if (S.selText('cmbActivitySumm') === '') { alert('Please Select Activity First...'); S.focus('cmbActivitySumm'); return Promise.resolve(); }
        var a = summaryArgsNow(), token = ++seqS;
        A.busy(b, true);
        return A.getJson(API + '/summary?' + S.qs(a)).then(function (rows) {
            if (token !== seqS) return;
            summaryArgs = a; summaryCount = rows ? rows.length : 0;
            var spec = SUMMARY[a.activity];
            if (!summaryCount || !spec) gD.clear(); else gD.setData(summaryCols(spec), summaryRows(rows, spec));
            printButtonManage();
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seqS) A.busy(b, false); });
    }
    function resetSummary() {                                                           // btnNewSumm_Click
        el('txtFromDateSumm').value = A.today(); el('txtToDateSumm').value = A.today();
        el('txtFromDocNoSumm').value = ''; el('txtToDocNoSumm').value = '';
        ['cmbParentcategorySumm', 'CmbCategorySumm', 'cmbItemtypeSumm', 'cmbCustomerSumm', 'cmbItemNameSumm', 'cmbcitySumm', 'cmbJobLotSumm'].forEach(function (id) {
            var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true }));
        });
        S.activate('cmbActivitySumm', 0, false);
        S.focus('txtFromDateSumm');
        gD.clear();
    }

    // ------------------------------------------------------------------ tabs
    var tab2 = 0;
    function setTab2(i) {                                                               // tabControl2
        tab2 = i;
        el('pageDetail').hidden = i !== 0; el('pageSummary').hidden = i !== 1;
        el('tab2d').classList.toggle('on', i === 0); el('tab2s').classList.toggle('on', i === 1);
        if (i === 1) S.focus('txtFromDateSumm');                                        // tabControl2_SelectedIndexChanged
    }
    var tab1 = 1;
    function setTab1(i) {                                                               // tabControl1: 0 Detail, 1 Brief
        tab1 = i;
        el('page1d').hidden = i !== 0; el('page1b').hidden = i !== 1;
        el('tab1d').classList.toggle('on', i === 0); el('tab1b').classList.toggle('on', i === 1);
    }
    el('tab2d').addEventListener('click', function () { setTab2(0); });
    el('tab2s').addEventListener('click', function () { setTab2(1); });
    el('tab1d').addEventListener('click', function () { setTab1(0); });
    el('tab1b').addEventListener('click', function () { setTab1(1); });

    function busyRun(btnId, fn) {
        var b = el(btnId); if (b.disabled) return;
        A.busy(b, true);
        Promise.resolve(fn()).then(function () { A.busy(b, false); }, function () { A.busy(b, false); });
    }
    function refresh(btnId) { return function () { busyRun(btnId, allDropDownBind); }; }
    function closeForm() { w.location.href = '/sale/reports'; }

    ['txtSrFrom', 'txtsrTo', 'txtFromDocNoSumm', 'txtToDocNoSumm'].forEach(E.digits);
    el('btnshow').addEventListener('click', showDetail);
    el('reset').addEventListener('click', resetDetail);
    el('refresh').addEventListener('click', refresh('refresh'));
    el('print').addEventListener('click', printDetail);
    el('BtnShowSumm').addEventListener('click', showSummary);
    el('sReset').addEventListener('click', resetSummary);
    el('sRefresh').addEventListener('click', refresh('sRefresh'));
    el('sPrint').addEventListener('click', printSummary);

    S.keys({
        s: function () { if (tab2 === 0) showDetail(); else showSummary(); },
        e: closeForm,
        n: function () { if (tab2 === 0) resetDetail(); else resetSummary(); },
        r: function () { if (tab2 === 0) refresh('refresh')(); else refresh('sRefresh')(); },
        p: function () { if (tab2 === 0) printDetail(); else printSummary(); },
        F5: function () { S.focus(tab2 === 0 ? 'fromdate' : 'txtFromDateSumm'); },
        ArrowDown: function () {
            if (tab2 === 1) { gD.focus(); return; }
            if (tab1 === 0) gA.focus();
            else if (el('gridB').contains(d.activeElement)) gC.focus();                  // Tag == "MasterGrid": GrdDetail, otherwise GrdMain
            else gB.focus();
        },
        t: function () { if (tab2 === 0) { setTab2(1); } else { setTab2(0); S.focus('fromdate'); } }
    }, closeForm);

    // ------------------------------------------------------------------ start (frmSaleOrderHistory_Load)
    el('fromdate').value = A.today(); el('ToDate').value = A.today();
    el('txtFromDateSumm').value = A.today(); el('txtToDateSumm').value = A.today();
    [gA, gB, gC, gD].forEach(function (g) { g.render(); });
    statusFill(); orderStatusFill(); activityFill();
    allDropDownBind().then(function () {
        var start = S.isoDay(lookup.yearStart);                                           // clsGlobalVariables.ActiveYr.Start_Period
        if (start) { el('fromdate').value = start; el('txtFromDateSumm').value = start; }
        printButtonManage();
        setTab1(1);
    });
}(window, document));
