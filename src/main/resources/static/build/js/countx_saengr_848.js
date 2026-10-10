/* ============================================================================================
 * Screen 848 frmSaleOrderHistory_Engr - Architecture.WinApp.Mfg.Reports.frmSaleOrderHistory_Engr (.cs, 4644 lines), module 135, document type 1656
 * Page: templates/sale/reports/saengr_mfg_sale_order_history.html (route /sale/reports/engr/mfg-sale-order-history). Same family as 554 (Inventory_Reports); what differs:
 *   frmSaleOrderHistory_Load :243   StatusFill, OrderStatusFill, BranchesFill, AllDropDownBind, ActivityFill, both From dates = Start_Period, PrintButtonManage, "Brief" sub tab
 *   BranchesFill :442               SaleOrder.GetBranchesAllocatedToUserFromSaleOrder(.., 1656) = USP_GetBranchsAllocatedToUserFromSaleOrder, two checked list combos
 *                                   (Detail and Summary), both start on the user's own branch
 *   AllDropDownBind :292            USP_GetDataForDropDownFromSaleOrder (DocumentTypeIds "1656", BranchesIds of the Detail tab's checked branches) split by Activity: Customer, Item,
 *                                   ParentCategory, ItemCategory, ItemType, City - ZeroIndex true (a "...Select Any Value..." row 0, no JobLot list)
 *   StatusFill :543                 UnApprove | Approve | All (the Inventory form says "UnApporve")
 *   gridHisory :944 (btnshow)       "Select Branch First" without a branch; SaleOrder.SaleOrderDetailRegister_Mfg = [Mfg].[USP_SaleOrderDetailRegister_Eng] (DocumentTypeId 1656);
 *                                   37 column detail table (adds VariantDescription and CastingType, no per-order rounding in GrdMain)
 *   GridSettings :1112 / MainGridSetting :1265 / GridDetailSetting :1500   CommonServices.GridWrappingAndColumnSettings(grid, 2, 3) (GridColumnSettings: Amount columns summed with
 *                                   stringFormatsingle, Rate columns DecimalRateFormate, other numbers "#,##0.###" summed, integers centred) and the Constants.InventoryConstants widths
 *   Links :1245 / :1389             DocNo -> CommonServices.SaleOrderSlipEngr_1656 (1656_SaleOrderSlip_Engr.rpt); CustomerName -> the general ledger of the customer
 *   print_Click :682                1656_SaleOrderDetailRegister.rpt (the button reads 1656_PO_Register)
 *   GridSummaryFill :1675           SaleOrder.SaleOrderEng_SummaryRegister = USP_SaleOrderEng_SummaryRegister; EVERY report type goes into the one Order Register table and
 *                                   DataGridHistorySetting :1734 shows the columns of that report type (visibleColumnsMap)
 *   PrintButtonManage :1918 / btnPrintSummary_Click :1895   1656_nn_*.rpt named by the Print button text
 *   Order actions (ApproveRecord / CompleteStatus / CancelStatus / UpdateExpiryDate) are the Inventory form's, unchanged.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, E = w.SaEngr, el = A.el;
    var API = '/api/sale/saengr/mfg-order-history';
    var PRINT_API = '/sale/reports/saengr/print/mfg-sale-order-history';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null, userBranchId: 0, rights: {} };
    var detailRaw = [], detailArgs = null, summaryArgs = null, summaryCount = 0, seqD = 0, seqS = 0;
    var printText = 'Print';

    // ------------------------------------------------------------------ column helpers (GridColumnSettings with DecimalCount 3 and the Constants.InventoryConstants widths)
    function H(key) { return { key: key, caption: key, hidden: true }; }
    function I(key, width, o) { var c = { key: key, caption: key, width: width, align: 'c' }; if (o) for (var k in o) c[k] = o[k]; return c; }       // int: centred
    function T(key, width, o) { var c = { key: key, caption: key, width: width }; if (o) for (var k in o) c[k] = o[k]; return c; }
    function DD(key, width) { return { key: key, caption: key, width: width, date: 'dd/MM/yyyy' }; }                                               // date only
    function DT(key, width) { return { key: key, caption: key, width: width }; }                                                                      // pre formatted by fixDates
    function Q(key, width) { return { key: key, caption: key, width: width, num: true, fmt: '#,##0.###', sum: true, totalFmt: '#,##0.###' }; }     // other doubles
    function M(key, width) { var f = S.fmtSingle(lookup.amountDecimals); return { key: key, caption: key, width: width, num: true, fmt: f, sum: true, totalFmt: f }; }   // *Amount*
    function R(key, width) { return { key: key, caption: key, width: width, num: true, fmt: S.fmtRate(lookup.rateDecimals) }; }                  // *Rate*
    function B(key, caption, text, width) { return { key: key, caption: caption, width: width, button: text }; }

    // ------------------------------------------------------------------ lists
    function branchIdsNow() {                                                                  // ",id,id" of the checked Detail branches (names mapped back to ids on the desktop)
        var s = ''; A.checkedValues(el('cmbBranchName')).forEach(function (v) { s += ',' + v; }); return s;
    }
    function bindCombos(data) {
        var c = (data && data.combos) || {};
        if (!Object.keys(c).length) return;                                                    // AllDropDownBind binds only when the procedure gives rows
        function rows(k) { return c[k] || []; }
        S.fill(el('CmbSupplier'), rows('Customer'), true); S.fill(el('cmbCustomerSumm'), rows('Customer'), true);
        S.fill(el('CmbItem'), rows('Item'), true); S.fill(el('cmbItemNameSumm'), rows('Item'), true);
        S.fill(el('cmbcitySumm'), rows('City'), true);
        S.fill(el('cmbParentcategorySumm'), rows('ParentCategory'), true); S.fill(el('CmbParentCategory'), rows('ParentCategory'), true);
        S.fill(el('CmbItemCategory'), rows('ItemCategory'), true); S.fill(el('CmbCategorySumm'), rows('ItemCategory'), true);
        S.fill(el('CmbItemType'), rows('ItemType'), true); S.fill(el('cmbItemtypeSumm'), rows('ItemType'), true);
    }
    function loadLookups(first) {                                                              // BranchesFill (Load only) + AllDropDownBind
        var url = API + '/lookups' + (first ? '' : '?branchIds=' + encodeURIComponent(branchIdsNow()));
        return A.getJson(url).then(function (data) {
            lookup = data || lookup; lookup.rights = lookup.rights || {};
            if (first && data && data.branches && data.branches.length) {                      // Branches bound only when the user has some
                S.fillBranches(el('cmbBranchName'), data.branches, data.userBranchId);
                S.fillBranches(el('cmbBranchNameSumm'), data.branches, data.userBranchId);
            }
            bindCombos(data);
        }).catch(function (e) { alert(e.message); });
    }
    function allDropDownBind() { return loadLookups(false); }
    function statusFill() {
        S.fill(el('CmbIsApproved'), [{ Id: 1, name: 'UnApprove' }, { Id: 2, name: 'Approve' }, { Id: 3, name: 'All' }], false);
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
    function slip(id) {                                                                        // CommonServices.SaleOrderSlipEngr_1656(Id)
        if (typeof w.printRpt !== 'function') { alert('Printing is not available on this page.'); return; }
        w.printRpt('1656_SaleOrderSlip_Engr.rpt', { id: id, branchesId: A.toInt(lookup.userBranchId), documentTypeIds: '1656' });
    }
    function ledger(supplierCustomerId) {
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
        onSelect: function (row) { fillBrief(row); },
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

    function makeExpiryEditable(grid) {                                                        // AllowEdit + EditType 2 on ExpiryDate
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

    /* gridHisory: the foreach copy of grdlst into the 37 column table */
    var MAP_A = [['Id', 'Id'], ['DocumentTypeId', 'DocumentTypeId'], ['DocNo', 'DocNo'], ['DocDate', 'DocDate'], ['OrderSupCustId', 'OrderSupCustId'], ['CustomerName', 'CustomerName'],
        ['ReferenceNo', 'SupplierRefNo'], ['PaymentTerm', 'TermsDescription'], ['DeliveryTerm', 'DeliveryTerm'], ['ItemId', 'OrderItemId'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'],
        ['ItemDescription', 'ItemDiscription'], ['PackUom', 'UOMCodeItm'], ['VariantDescription', 'VarientDescription'], ['CastingType', 'CastingType'], ['ItemQty', 'OrderItemQty'],
        ['DispatchQty', 'DispatchQty'], ['BalQty', 'BalQty'], ['ItemRate', 'OrderItemRate'], ['ItemAmount', 'Amount'], ['DispatchAmount', 'DispatchAmount'], ['BalAmount', 'BalAmount'],
        ['CityName', 'CityName'], ['DetailRemarks', 'OrderRemarks'], ['DueDays', 'OrderDueDays'], ['DueDate', 'OrderDueDate'], ['OrderStatus', 'OrderStatus'], ['ApprovedStatus', 'ApprovedStatus'],
        ['ExpiryDate', 'OrderExpiryDate'], ['EntryDate', 'EntryDate'], ['EntryUser', 'UserNameEusr'], ['ModifyDate', 'ModifyDate'], ['ModifyUser', 'UserNameMusr'], ['ApprovedDate', 'PostDate'],
        ['ApprovedUser', 'UserNameAusr'], ['Remarks', 'RemarksHeader']];
    function mapRows(raw, map) {
        return raw.map(function (r) { var o = {}; map.forEach(function (m) { var v = A.ci(r, m[1]); o[m[0]] = v === undefined ? null : v; }); return o; });
    }
    function colsA(approval, status) {
        var c = [H('Id'), H('DocumentTypeId'), I('DocNo', 60, { link: true }), DD('DocDate', 73), H('OrderSupCustId'), T('CustomerName', 170, { link: true }), T('ReferenceNo', 60), T('PaymentTerm', 70),
            T('DeliveryTerm', 70), H('ItemId'), T('ItemCode', 130), T('ItemName', 150), T('ItemDescription', 130), T('Uom', 60, { key: 'PackUom', caption: 'Uom' }), T('VariantDescription', 65),
            T('CastingType', 90), Q('ItemQty', 70), Q('DispatchQty', 70), Q('BalQty', 70), R('ItemRate', 70), M('ItemAmount', 90), M('DispatchAmount', 90), M('BalAmount', 90), T('CityName', 130),
            T('DetailRemarks', 100), I('DueDays', 60), DT('DueDate', 73), T('OrderStatus', 75), T('ApprovedStatus', 75), DT('ExpiryDate', 73), DT('EntryDate', 135), T('EntryUser', 80),
            DT('ModifyDate', 135), T('ModifyUser', 80), DT('ApprovedDate', 135), T('ApprovedUser', 80), T('Remarks', 100)];
        if (approval === 'Approve') { c.push(B('Complete', 'Complete', 'Complete', 80)); c.push(B('Cancel', 'Cancel', 'Cancel', 60)); }
        if (approval === 'UnApprove') { var hb = B('IsApproved', 'IsApproved', 'IsApproved', 80); hb.hidden = true; c.push(hb); }
        if (status === 'Open' && approval === 'Approve') c.push(B('Update', 'UpdateExpiryDate', 'UpdateExpiryDate', 120));
        return c;
    }
    function colsB(approval, status) {
        var r = lookup.rights || {};
        var c = [H('Id'), H('DocumentTypeId'), I('DocNo', 60, { link: true }), DT('DocDate', 73), H('SupplierCustomerId'), T('CustomerName', 170, { link: true }), T('OrderStatus', 75), T('ApprovedStatus', 75),
            T('PaymentTerm', 70), I('DueDays', 60), DT('DueDate', 73), T('DeliveryTerm', 70), Q('ItemQty', 70), Q('DispatchQty', 70), Q('BalQty', 70), M('OrderAmount', 90), M('DispatchAmount', 90),
            M('BalAmount', 90), DT('ExpiryDate', 73), DT('EntryDate', 135), T('EntryUser', 80), DT('ModifyDate', 135), T('ModifyUser', 80), DT('ApprovedDate', 135), T('ApprovedUser', 80), T('Remarks', 100)];
        var edit = false;
        if (approval === 'Approve' && status === 'Open') {
            if (r.CanChangeOrderStatusToComplete) c.push(B('Complete', 'Complete', 'Complete', 80));
            if (r.CanChangeOrderStatusToCancel) c.push(B('Cancel', 'Cancel', 'Cancel', 60));
            if (r.CanChangeOrderExpiryDate) { edit = true; c.push(B('Update', 'UpdateExpiryDate', 'UpdateExpiryDate', 120)); }
        }
        if (approval === 'UnApprove') { var hb = B('IsApproved', 'IsApproved', 'IsApproved', 60); hb.hidden = true; c.push(hb); }
        return { cols: c, edit: edit };
    }
    var DATETIME_A = ['DueDate', 'ExpiryDate', 'EntryDate', 'ModifyDate', 'ApprovedDate'];
    var DATETIME_B = ['DocDate', 'DueDate', 'ExpiryDate', 'EntryDate', 'ModifyDate', 'ApprovedDate'];

    /* dtMain: one row per Id (first occurrence); the sums are plain doubles here */
    function mainRows(raw) {
        var seen = {}, out = [];
        function sum(id, col) { var s = 0; raw.forEach(function (r) { if (String(A.ci(r, 'Id')) === id) s += A.toDouble(A.ci(r, col)); }); return s; }
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
    var MAP_C = [['ItemId', 'OrderItemId'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'], ['Uom', 'UOMCodeItm'], ['VariantDescription', 'VarientDescription'], ['CastingType', 'CastingType'],
        ['ItemQty', 'OrderItemQty'], ['DispatchQty', 'DispatchQty'], ['BalQty', 'BalQty'], ['ItemRate', 'OrderItemRate'], ['ItemAmount', 'Amount'], ['DispatchAmount', 'DispatchAmount'],
        ['BalAmount', 'BalAmount'], ['TaxType', 'TaxName'], ['Tax%', 'TaxPercent'], ['TaxAmount', 'TaxAmount'], ['CityName', 'CityName']];
    function fillBrief(row) {                                                                  // GrdMain_SelectionChanged -> GrdDetail
        if (!row) return;
        var id = String(row.Id);
        var raw = detailRaw.filter(function (r) { return String(A.ci(r, 'Id')) === id; });
        var cols = [H('ItemId'), T('ItemCode', 130), T('ItemName', 150), T('Uom', 60), T('VariantDescription', 100), T('CastingType', 90), Q('ItemQty', 70), Q('DispatchQty', 70), Q('BalQty', 70),
            R('ItemRate', 70), M('ItemAmount', 90), M('DispatchAmount', 90), M('BalAmount', 90), T('TaxType', 110), Q('Tax%', 60), M('TaxAmount', 90), T('CityName', 130)];
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
        if (A.checkedValues(el('cmbBranchName')).length === 0) { S.focus('cmbBranchName'); alert('Select Branch First'); return Promise.resolve(); }
        var a = detailArgsNow(), token = ++seqD;
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seqD) return;
            detailArgs = a; detailRaw = rows || [];
            if (!detailRaw.length) { gA.clear(); gB.clear(); gC.clear(); return; }
            var approval = a.approval, status = a.status;
            gA.editExpiry = approval === 'Approve' && status === 'Open';
            gA.setData(colsA(approval, status), E.fixDates(mapRows(detailRaw, MAP_A), DATETIME_A));
            var b2 = colsB(approval, status);
            gB.editExpiry = b2.edit;
            gB.setData(b2.cols, E.fixDates(mainRows(detailRaw), DATETIME_B));
            gB.select(0);
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seqD) A.busy(b, false); });
    }
    function clearCombo(id) { var s = el(id); s.selectedIndex = -1; s.dispatchEvent(new Event('change', { bubbles: true })); }     // Text = string.Empty
    function resetDetail() {                                                                   // resetDetailRegister
        el('chkincludeDo').checked = false;
        ['CmbSupplier', 'CmbItem', 'CmbParentCategory', 'CmbItemCategory', 'CmbItemType'].forEach(clearCombo);
        el('txtSrFrom').value = ''; el('txtsrTo').value = '';
        S.activate('CmbIsApproved', 0, false); S.activate('CmbOrderStatus', 0, false);
        S.focus('fromdate');
        return showDetail();
    }

    // ------------------------------------------------------------------ order actions (unchanged from the Inventory form)
    function post(action, id, expiry) { return S.post(API + '/action', { action: action, id: id, expiryDate: expiry || null }); }
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
    function printDetail() {                                                                   // print_Click
        var b = el('print'); if (b.disabled) return;
        if (!detailRaw.length || !detailArgs) { alert('Record Not Found'); return; }
        A.busy(b, true);
        A.openPdf(PRINT_API + '?' + S.qs(Object.assign({ kind: 'detail' }, detailArgs))).catch(function () { alert('Record Not Found'); }).then(function () { A.busy(b, false); });
    }
    var TEMPLATES = ['1656_01_OrderRegister', '1656_02_OrderSummaryByItem&PackSize', '1656_03_OrderSummaryByItem,PackSize&City', '1656_04_OrderSummaryByCustomer&PackSize',
        '1656_05_OrderSummaryByItem', '1656_06_OrderSummaryByItem&City', '1656_07_OrderSummaryByCustomer', '1656_08_OrderSummaryByCustomer&City', '1656_09_OrderSummaryByCustomer&Item',
        '1656_10_OrderSummaryByCustomer,Item&City', '1656_11_OrderSummaryByCustomer&Order'];
    /* PrintButtonManage ("Sales Summary By Customer & City" never equals a report type, so "Order Summary By Customer & City" keeps the previous text) */
    var PRINT_TEXT = { 'Order Register': TEMPLATES[0], 'Order Summary By Item & Pack Size': TEMPLATES[1], 'Order Summary By Item,Pack Size & City': TEMPLATES[2],
        'Order Summary By Customer & Pack Size': TEMPLATES[3], 'Order Summary By Item': TEMPLATES[4], 'Order Summary By Item & City': TEMPLATES[5],
        'Order Summary By Customer': TEMPLATES[6], 'Order Summary By Customer & Item': TEMPLATES[8], 'Order Summary By Customer,Item & City': TEMPLATES[9],
        'Order Summary By Customer & OrderNo': TEMPLATES[10] };
    function printButtonManage() {
        var b = el('sPrint'), text = S.selText('cmbActivitySumm');
        if (text !== '') { b.hidden = false; if (PRINT_TEXT[text]) printText = PRINT_TEXT[text]; } else b.hidden = true;
        b.querySelector('span').textContent = printText;
    }
    function printSummary() {                                                                  // btnPrintSummary_Click
        var b = el('sPrint'); if (b.disabled) return;
        if (!summaryCount || !summaryArgs) { alert('Record Not Found For Display'); return; }
        if (TEMPLATES.indexOf(printText) < 0) { alert('Invalid report name.'); return; }
        A.busy(b, true);
        A.openPdf(PRINT_API + '?' + S.qs(Object.assign({ kind: 'summary', template: printText }, summaryArgs))).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ Summary tab (GridSummaryFill / DataGridHistorySetting)
    var VISIBLE = {
        'Order Register': ['DocDate', 'DocNo', 'PartyName', 'ItemCode', 'ItemName', 'PackUom', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal', 'CityName'],
        'Order Summary By Item': ['ItemCode', 'ItemName', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal'],
        'Order Summary By Item & Pack Size': ['ItemCode', 'ItemName', 'PackUom', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal'],
        'Order Summary By Item & City': ['ItemCode', 'ItemName', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal', 'CityName'],
        'Order Summary By Item,Pack Size & City': ['ItemCode', 'ItemName', 'PackUom', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal', 'CityName'],
        'Order Summary By Customer': ['PartyName', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal'],
        'Order Summary By Customer & OrderNo': ['PartyName', 'DocDate', 'DocNo', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal'],
        'Order Summary By Customer & Item': ['PartyName', 'ItemCode', 'ItemName', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal'],
        'Order Summary By Customer & City': ['PartyName', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal', 'CityName'],
        'Order Summary By Customer,Item & City': ['PartyName', 'ItemCode', 'ItemName', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal', 'CityName'],
        'Order Summary By Customer & Pack Size': ['PartyName', 'PackUom', 'OrderQty', 'DispatchedQty', 'BalQty', 'OrderAmount', 'DispatchedAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal']
    };
    var SUMM_MAP = [['Id', 'Id'], ['DocumentTypeId', 'DocumentTypeId'], ['DocDate', 'DocDate'], ['DocNo', 'DocNo'], ['PartyName', 'Customer'], ['ItemCode', 'ItemCode'], ['ItemName', 'ItemName'],
        ['PackUom', 'UOMCode'], ['OrderQty', 'OrderQty'], ['DispatchedQty', 'DispatchQty'], ['BalQty', 'BalQty'], ['OrderAmount', 'OrderAmount'], ['DispatchedAmount', 'DispatchAmount'],
        ['BalAmount', 'BalAmount'], ['AvgRate', 'AvgRate'], ['PercentOfTotal', 'PrcntOfTotal'], ['CityName', 'CityName']];
    function summaryCols(activity) {
        var shown = (VISIBLE[activity] || []);
        var c = [H('Id'), H('DocumentTypeId'), DT('DocDate', 73), I('DocNo', 60), T('PartyName', 170), T('ItemCode', 80), T('ItemName', 150), T('PackUom', 60, { caption: 'Uom' }), Q('OrderQty', 70), Q('DispatchedQty', 70),
            Q('BalQty', 70), M('OrderAmount', 90), M('DispatchedAmount', 90), M('BalAmount', 90), R('AvgRate', 70), Q('PercentOfTotal', 55), T('CityName', 130)];
        c.forEach(function (col) { if (col.hidden) return; if (shown.indexOf(col.key) < 0) col.hidden = true; });
        return c;
    }
    function summaryArgsNow() {
        return { fromDate: el('txtFromDateSumm').value, toDate: el('txtToDateSumm').value, fromDocNo: A.toInt(el('txtFromDocNoSumm').value), toDocNo: A.toInt(el('txtToDocNoSumm').value),
                 parentCategoryId: S.selInt('cmbParentcategorySumm'), categoryId: S.selInt('CmbCategorySumm'), itemTypeId: S.selInt('cmbItemtypeSumm'),
                 supplierCustomerId: S.selInt('cmbCustomerSumm'), itemId: S.selInt('cmbItemNameSumm'), cityId: S.selInt('cmbcitySumm'),
                 activity: S.selText('cmbActivitySumm'), actionId: el('ChkDoApprovedIncludSummary').checked ? 1 : 0, skipZero: el('chkSkipZero').checked ? 1 : 0 };
    }
    function showSummary() {                                                                   // BtnShowSumm_Click
        var b = el('BtnShowSumm'); if (b.disabled) return Promise.resolve();
        if (S.selText('cmbActivitySumm') === '') { alert('Please Select Activity First...'); S.focus('cmbActivitySumm'); return Promise.resolve(); }
        var a = summaryArgsNow(), token = ++seqS;
        A.busy(b, true);
        return A.getJson(API + '/summary?' + S.qs(a)).then(function (rows) {
            if (token !== seqS) return;
            summaryArgs = a; summaryCount = rows ? rows.length : 0;
            if (!summaryCount) gD.clear();
            else gD.setData(summaryCols(a.activity), E.fixDates(mapRows(rows, SUMM_MAP), ['DocDate']));
            printButtonManage();
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seqS) A.busy(b, false); });
    }
    function resetSummary() {                                                                  // btnNewSumm_Click
        el('txtFromDateSumm').value = A.today(); el('txtToDateSumm').value = A.today();
        el('txtFromDocNoSumm').value = ''; el('txtToDocNoSumm').value = '';
        ['cmbParentcategorySumm', 'CmbCategorySumm', 'cmbItemtypeSumm', 'cmbCustomerSumm', 'cmbItemNameSumm', 'cmbcitySumm'].forEach(clearCombo);
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

    // ------------------------------------------------------------------ start (frmSaleOrderHistory_Load: StatusFill, OrderStatusFill, BranchesFill, AllDropDownBind, ActivityFill)
    el('fromdate').value = A.today(); el('ToDate').value = A.today();
    el('txtFromDateSumm').value = A.today(); el('txtToDateSumm').value = A.today();
    [gA, gB, gC, gD].forEach(function (g) { g.render(); });
    statusFill(); orderStatusFill();
    loadLookups(true).then(function () {
        activityFill();
        var start = S.isoDay(lookup.yearStart);                                           // clsGlobalVariables.ActiveYr.Start_Period
        if (start) { el('fromdate').value = start; el('txtFromDateSumm').value = start; }
        printButtonManage();
        setTab1(1);
    });
}(window, document));
