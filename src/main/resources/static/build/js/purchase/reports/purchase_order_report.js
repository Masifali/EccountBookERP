/* 479 Purchase Order Report - Architecture.WinApp.InventoryReports.PurchaseOrderHistory. ":NNN" = line in
   PurchaseOrderHistory.cs. API /purchase/api/reports/purchase-order. Built on pr_common.js (window.PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/purchase/api/reports/purchase-order', SCREEN = 479, K = PR.K;
    var st = { init: null, detailFilter: null, summaryFilter: null, globaldt: [], dtSummary: [], summaryType: '', printText: 'Print' };
    var from = new PR.Picker('datFromDate', 'chkFromDate'), to = new PR.Picker('datToDate', 'chkToDate');
    var fromS = new PR.Picker('datFromDateSumm', 'chkFromDateSumm'), toS = new PR.Picker('datToDateSumm', 'chkToDateSumm');
    var branches = new PR.CheckList('cmbBranchName'), branchesS = new PR.CheckList('cmbBranchNameSumm');
    var grid = new PR.Grid('grd', 'lblCount'), gridS = new PR.Grid('GridSummary', 'lblCountSumm');
    var ACTIVITY = { 1: 'OutStanding & Open', 2: 'OutStanding & InProcess', 3: 'InProcess & Excess Received', 4: 'Expiring Next 7 Days', 5: 'Canceled' };
    function radio(name) { var r = document.querySelector('input[name="' + name + '"]:checked'); return r ? r.value : ''; }
    function rights() { return (st.init && st.init.rights) || {}; }

    // ------------------------------------------------------------------ combos
    function bindParentCategories(rows) {   // BindParentCategory :643 - ZeroIndex false, Rows[0] active
        PR.fill('CmbParentCatgory', rows, { text: 'InvParentCateDescription' });
        PR.fill('cmbParentcategorySumm', rows, { text: 'InvParentCateDescription' });
        if (rows && rows.length) { $('CmbParentCatgory').value = String(rows[0].Id); $('cmbParentcategorySumm').value = String(rows[0].Id); }
    }
    /** AllDropDownBind :694 - FilterData by Activity; BindComboBox is BindDDL(ZeroIndex false). */
    function AllDropDownBind(rows) {
        var f = function (a) { return PR.byActivity(rows, a); };
        var b = function (id, a) { PR.fill(id, f(a), {}); };
        if (!rows || !rows.length) {
            ['CmbItem', 'cmbItemNameSumm', 'CmbSupplier', 'cmbCustomerSumm', 'CmbCategorySumm', 'cmbItemtypeSumm', 'cmbCropSumm', 'cmbJobLotSumm',
                'cmbDistrictSumm', 'cmbcitySumm', 'CmbBookingPerson', 'cmbBookingPersonSumm'].forEach(function (id) { PR.fill(id, [], {}); });
            return;
        }
        b('CmbSupplier', 'Supplier'); b('cmbCustomerSumm', 'Supplier'); b('CmbItem', 'Item'); b('CmbCategorySumm', 'ItemCategories');
        b('cmbItemtypeSumm', 'ItemTypes'); b('cmbCropSumm', 'CropYear'); b('cmbJobLotSumm', 'JobLot'); b('cmbDistrictSumm', 'District');
        b('cmbcitySumm', 'City'); b('cmbItemNameSumm', 'Item'); b('CmbBookingPerson', 'BookingPerson'); b('cmbBookingPersonSumm', 'BookingPerson');
    }
    /** GetDropDownData :591 - DocumentTypeIds "41" with the DETAIL tab's branches (also for the summary tab). */
    function GetDropDownData() { return PR.request(API + '/lookups?branchIds=' + encodeURIComponent(branches.ids().join(','))).then(AllDropDownBind); }
    branches.onLeave = function () {               // cmbBranchName_Leave :2585
        if (branches.ids().length) { PR.run(GetDropDownData); return; }
        PR.fill('CmbSupplier', [], {}); PR.fill('CmbItem', [], {});
    };
    branchesS.onLeave = function () {              // cmbBranchNameSumm_Leave :2605
        if (branchesS.ids().length) { PR.run(GetDropDownData); return; }
        ['CmbCategorySumm', 'cmbItemtypeSumm', 'cmbCropSumm', 'cmbDistrictSumm', 'cmbcitySumm', 'cmbJobLotSumm', 'cmbItemNameSumm', 'cmbCustomerSumm']
            .forEach(function (id) { PR.fill(id, [], {}); });
    };

    // ------------------------------------------------------------------ status panels
    /** FilterData :989 / FilterDataSummary :1491 - USP_GetOrdersStatus rows 0-4, stringFormatboth. */
    function statusPanels(summary) {
        var sel = summary ? '[data-activity-summ]' : '[data-activity]';
        var ids = summary ? ['txtOustandingSummary', 'txtOustandingInProcessSummary', 'txtInProcessSummary', 'txtExpiringDaysSummary', 'txtCanceledSummary']
            : ['txtOutstandingValue', 'txtOutstandingInprocess', 'txtInProcessExcess', 'txtExpiringDays', 'txtCanceled'];
        document.querySelectorAll(sel).forEach(function (b) { b.classList.add('is-hidden'); });
        return PR.request(API + '/status').then(function (v) {
            if (!v || !v.length) { ids.forEach(function (id) { $(id).textContent = ''; }); return; }
            document.querySelectorAll(sel).forEach(function (b) { b.classList.remove('is-hidden'); });
            ids.forEach(function (id, i) { $(id).textContent = PR.fmt(v[i], 'amt'); });
        });
    }

    // ------------------------------------------------------------------ detail tab
    function detailFilter(activity) {
        return {
            branchIds: branches.ids(), fromDate: from.value(), toDate: to.value(), fromNo: PR.val('txtSrFrom'), toNo: PR.val('txtSrTo'),
            supplierId: PR.val('CmbSupplier'), bookingPersonId: PR.val('CmbBookingPerson'), parentCategoryId: PR.val('CmbParentCatgory'),
            itemId: PR.val('CmbItem'), status: radio('statusDetail'), approval: radio('approvalDetail'), activityId: activity || 0
        };
    }
    /** Which action columns / toolbar buttons GridSettings (:1280-1331) shows for these filters. */
    function actionState(f) {
        var r = rights(), s = { open: false, complete: false, cancel: false, update: false };
        if (f.approval === 'Approved' && f.status !== 'Open' && f.status !== 'All' && r.CanChangeOrderStatusToOpen) s.open = true;
        else if (f.approval === 'Approved' && f.status === 'Open') {
            s.complete = !!r.CanChangeOrderStatusToComplete; s.cancel = !!r.CanChangeOrderStatusToCancel; s.update = !!r.CanChangeOrderExpiryDate;
        }
        s.any = s.open || s.complete || s.cancel || s.update;
        return s;
    }
    /** GridSettings :1049. */
    function detailColumns(s) {
        var cols = [{ key: 'Slip', caption: 'Slip203', width: 60, button: 'Slip203' }];
        if (st.init.approvalPolicy) cols.push({ key: 'ApprovalDetail', caption: 'Approval Detail', width: 100, button: 'Approval Detail' });
        cols = cols.concat([
            { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true }, { key: 'BranchName', width: K.String },
            { key: 'ItemCategory', hidden: true }, { key: 'ItemType', hidden: true },
            { key: 'DocDate', width: K.Date, type: 'date' }, { key: 'DocNo', width: K.DocNo, open: function (r) { return PR.int(r.Id) > 0 ? [PR.int(r.DocumentTypeId) || 41, PR.int(r.Id)] : null; } },
            { key: 'OrderSupCustId', hidden: true }, { key: 'SupplierName', width: K.String, link: true },
            { key: 'BookingPerson', width: K.String }, { key: 'CommissionAgent', width: K.String }, { key: 'DeliveryTerm', width: K.Term },
            { key: 'DeliveryDays', width: K.DueDays, red: function (r) { return expired(r); } },
            { key: 'DueDate', width: K.Date + 10, type: 'date' }, { key: 'PurchaseGLAC', hidden: true }, { key: 'ItemId', hidden: true },
            { key: 'ItemName', width: K.String }, { key: 'Crop', width: K.CropYear }, { key: 'PackUom', caption: 'Pack Size', width: K.Uom },
            { key: 'Qty', caption: 'Order Qty', width: K.Qty, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'RcvdQty', caption: 'Received Qty', width: K.Qty, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'RejQty', hidden: true },
            { key: 'BalQty', caption: 'Balance Qty', width: K.Qty, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'Weight', caption: 'Order Weight', width: K.Weight, type: 'num', fmt: 'n4', agg: 'sum' },
            { key: 'RcvdWeight', caption: 'Received Weight', width: K.Weight, type: 'num', fmt: 'n4', agg: 'sum' },
            { key: 'BalWeight', caption: 'Balance Weight', width: K.Weight, type: 'num', fmt: 'n4', agg: 'sum' },
            { key: 'Rate', width: K.Rate, type: 'num', fmt: 'n4' },
            { key: 'OrderAmount', width: K.Amount, type: 'num', fmt: 'n4', agg: 'sum' },
            { key: 'ReceivedAmount', width: K.Amount, type: 'num', fmt: 'n4', agg: 'sum' },
            { key: 'BalAmount', width: K.Amount, type: 'num', fmt: 'n4', agg: 'sum' },
            { key: 'PPBagRate', width: K.Rate }, { key: 'JuteBagRate', width: K.Rate }, { key: 'PPBagWeight', width: K.Weight }, { key: 'JuteBagWeight', width: K.Weight },
            { key: 'ApprovedBy', width: K.ApprovedBy }, { key: 'ApprovedDate', width: K.Date, type: 'date' },
            s.update ? { key: 'ExpiryDate', width: 110, edit: 'date' } : { key: 'ExpiryDate', width: K.Date, type: 'date' },
            s.any ? { key: 'ActionRemarks', width: K.String, edit: 'text' } : { key: 'ActionRemarks', width: K.String }]);
        if (s.open) cols.push({ key: 'Open', width: 40, button: 'Open' });
        if (s.complete) cols.push({ key: 'Complete', width: 80, button: 'Complete' });
        if (s.cancel) cols.push({ key: 'Cancel', width: 60, button: 'Cancel' });
        if (s.update) cols.push({ key: 'Update', caption: 'UpdateExpiryDate', width: 120, button: 'UpdateExpiryDate' });
        return cols;
    }
    /** grd_FormattingRow :2945 - DeliveryDays red/bold when ExpiryDate is before now. */
    function expired(r) { var d = String(r.ExpiryDate || '').slice(0, 10); return d && new Date(d + 'T00:00:00') < new Date(); }

    /** GetDetailData :822 + BindDetailData :918. */
    function GetDetailData(activity) {
        var f = detailFilter(activity);
        if (!f.branchIds.length) return Promise.reject(new Error('Select Branch First'));
        $('label36').hidden = true;
        return PR.request(API + '/detail', f).then(function (res) {
            st.detailFilter = f; st.globaldt = res.raw || [];
            $('label36').textContent = ACTIVITY[activity] || ''; $('label36').hidden = !(activity > 0);
            var s = actionState(f);
            ['btnOpenOrders', 'btnCompleteOrders', 'btnCancelOrders'].forEach(function (id) { $(id).classList.add('is-hidden'); });
            if (!(res.rows || []).length) { grid.clear(); return; }
            if (s.open) $('btnOpenOrders').classList.remove('is-hidden');
            if (s.complete) $('btnCompleteOrders').classList.remove('is-hidden');
            if (s.cancel) $('btnCancelOrders').classList.remove('is-hidden');
            grid.show(detailColumns(s), res.rows, { groupBy: radio('groupBy'), selector: s.any, onLink: detailLink, onButton: detailButton });
        });
    }
    /** btnshow_Click :803 - the status panels, then the history. */
    function btnshow() { return statusPanels(false).then(function () { return GetDetailData(0); }); }

    function detailLink(col, r) {                          // grd_LinkClicked :1340
        // DocNo: the desktop opens ActivityDetails (not ported); the web opens the purchase order itself (column "open").
        if (col === 'SupplierName') PR.openLedger(SCREEN, PR.int(r.OrderSupCustId), from.checked() ? from.date() : dateAdd(-30), to.date());
    }
    function dateAdd(days) { var d = new Date(); d.setDate(d.getDate() + days); return PR.ymd(d); }
    function detailButton(col, r) {                        // grd_ColumnButtonClick_1 :1404
        if (col === 'Slip') PR.run(function () { return PR.printTemplate('203-InvRptPurchaseOrderRiceSlip.rpt', { id: PR.int(r.Id) }); });
        if (col === 'ApprovalDetail') PR.approval(API + '/approval', { filter: st.detailFilter, id: PR.int(r.Id), documentTypeId: PR.int(r.DocumentTypeId) });
        var action = { Complete: 'Complete', Open: 'Open', Cancel: 'Cancel', Update: 'UpdateExpiryDate' }[col];
        if (action) rowAction(action, r);
    }
    /** CompleteStatus :1993 / OpenStatus :2044 / CancelStatus :2095 / UpdateExpiryStatus :2142. */
    function rowAction(action, r) {
        var remarks = String(r.ActionRemarks || '');
        if (!remarks && !st.init.remarksNotRequired) { PR.box('Action Remarks Required'); return; }
        var q = { Complete: 'Are you sure to Complete Status?', Open: 'Are you sure to Open Status?', Cancel: 'Are you sure to Cancel Status?',
            UpdateExpiryDate: 'Are you sure to ExpiryDate Update?' }[action];
        if (!window.confirm(q)) return;
        PR.run(function () {
            return PR.request(API + '/action', { action: action, bulk: false, filter: st.detailFilter,
                selections: [{ id: PR.int(r.Id), remarks: remarks, expiryDate: action === 'UpdateExpiryDate' ? String(r.ExpiryDate || '').slice(0, 10) : '' }] })
                .then(function (res) { PR.box(res.message); return btnshow(); });
        });
    }
    /** btnOpenOrders_Click :2780 / btnCompleteOrders_Click :2835 / btnCancelOrders_Click :2890. */
    function bulkAction(action) {
        var checked = grid.checked();
        if (!checked.length) { PR.box('Please Check Rows To Proceed!'); return; }
        var q = { Open: 'Are you sure to Open Status?', Complete: 'Are you sure to Complete Status?', Cancel: 'Are you sure to Cancel Status?' }[action];
        if (!window.confirm(q)) return;
        PR.remarks().then(function (remarks) {
            if (!remarks) { PR.box('Action Remarks Required'); return; }
            PR.run(function () {
                return PR.request(API + '/action', { action: action, bulk: true, remarks: remarks, filter: st.detailFilter,
                    selections: checked.map(function (r) { return { id: PR.int(r.Id) }; }) })
                    .then(function (res) { PR.box(res.message); return btnshow(); });
            });
        });
    }
    /** reset :471. */
    function reset() {
        var d = new Date(); d.setDate(d.getDate() - 7); from.set(d); to.set(new Date());
        $('CmbSupplier').value = ''; $('CmbItem').value = ''; $('txtSrFrom').value = ''; $('txtSrTo').value = '';
        from.focus(); grid.clear();
    }
    function print(rpt, withPrintedBy) {                   // toolStripButton1_Click :2480 / toolStripButton2_Click :2505
        if (!st.globaldt.length) { PR.box('Not Record Found For Display'); return; }
        PR.run(function () { return PR.printGrid(rpt, rpt.replace(/\.rpt$/, ''), st.globaldt); });
    }

    // ------------------------------------------------------------------ summary tab
    var SUMMARY_PRINT = { 'Order Register': ['OrderRegister-391', '391-OrderRegister.rpt'], 'Order Summary By Item': ['OrderSummaryByItem-392', '392-OrderSummaryByItem.rpt'],
        'Order Summary By Supplier': ['OrderSummaryBySupplier-393', '393-OrderSummaryBySupplier.rpt'],
        'Order Summary By Supplier & Item': ['OrderSummaryBySupplierAndItem-394', '394-OrderSummaryByCustomerandItem.rpt'] };
    function summaryFilter(activity) {
        return {
            branchIds: branchesS.ids(), fromDate: fromS.value(), toDate: toS.value(), fromNo: PR.val('txtFromDocNoSumm'), toNo: PR.val('txtToDocNoSumm'),
            parentCategoryId: PR.val('cmbParentcategorySumm'), itemCategoryId: PR.val('CmbCategorySumm'), itemTypeId: PR.val('cmbItemtypeSumm'),
            supplierId: PR.val('cmbCustomerSumm'), bookingPersonId: PR.val('cmbBookingPersonSumm'), itemId: PR.val('cmbItemNameSumm'),
            cropYearId: PR.val('cmbCropSumm'), cropYear: PR.text('cmbCropSumm'), jobLotId: PR.val('cmbJobLotSumm'), cityId: PR.val('cmbcitySumm'),
            districtId: PR.val('cmbDistrictSumm'), reportType: PR.text('cmbActivitySumm') || $('cmbActivitySumm').value,
            status: radio('statusSumm'), approval: radio('approvalSumm'), packUom: $('chkIncludePackUom').checked,
            cityWise: $('chkIncludeCity').checked, skipZero: $('chkSkipZero').checked, activityId: activity || 0
        };
    }
    /** DataGridHistorySetting :1730 - visible columns per report type. */
    function summaryColumns(type) {
        var show = {
            'Order Register': ['DocType', 'DocDate', 'DocNo', 'BookingPerson', 'BranchName', 'ItemCode', 'ItemName', 'PackUom', 'CropYear', 'JobLot'],
            'Order Summary By Item': ['BranchName', 'ItemCode', 'ItemName', 'PackUom'],
            'Order Summary By Supplier': ['BranchName', 'Supplier', 'PackUom'],
            'Order Summary By Supplier & Item': ['BranchName', 'Supplier', 'ItemCode', 'ItemName', 'PackUom']
        }[type].concat(['OrderQty', 'RcvdQty', 'BalQty', 'OrderWeight', 'RcvdWeight', 'BalWeight', 'OrderAmount', 'RcvdAmount', 'BalAmount', 'AvgRate', 'PercentOfTotal', 'CityName']);
        var cols = [
            { key: 'Id' }, { key: 'DocumentTypeId' }, { key: 'DocType', width: 90 }, { key: 'DocDate', width: K.Date, type: 'date' },
            { key: 'DocNo', width: K.DocNo, open: function (r) { return PR.int(r.Id) > 0 ? [PR.int(r.DocumentTypeId) || 41, PR.int(r.Id)] : null; } }, { key: 'BranchName', width: K.String }, { key: 'ItemCode', width: K.Term }, { key: 'ItemName', width: K.String },
            { key: 'PackUom', width: K.Uom }, { key: 'OrderQty', width: K.Qty, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'RcvdQty', caption: 'Received Qty', width: K.Qty, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'BalQty', caption: 'Balance Qty', width: K.Qty, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'OrderWeight', width: K.Weight, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'RcvdWeight', caption: 'Received Weight', width: K.Weight, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'BalWeight', caption: 'Balance Weight', width: K.Weight, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'OrderAmount', width: K.Amount, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'RcvdAmount', caption: 'Received Amount', width: K.Amount, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'BalAmount', caption: 'Balance Amount', width: K.Amount, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'AvgRate', caption: 'Average Rate', width: K.Rate, type: 'num', fmt: 'n3' },
            { key: 'PercentOfTotal', caption: '% Of Total', width: K.Percent, type: 'num', fmt: 'n3', agg: 'sum' },
            { key: 'CityName', width: K.Term }, { key: 'Supplier', width: K.String }, { key: 'BookingPerson', width: K.String },
            { key: 'CropYear', width: K.CropYear }, { key: 'JobLot', width: K.JobLot }];
        cols.forEach(function (c) { c.hidden = show.indexOf(c.key) < 0; });
        if (type === 'Order Register' && st.init.approvalPolicy) cols.unshift({ key: 'ApprovalDetail', caption: 'Approval Detail', width: 100, button: 'Approval Detail' });
        return cols;
    }
    /** GridSummaryFill :1549. */
    function GridSummaryFill(activity) {
        var f = summaryFilter(activity);
        if (!f.branchIds.length) return Promise.reject(new Error('Select Branch First'));
        $('label15').hidden = true;
        return PR.request(API + '/summary', f).then(function (res) {
            st.dtSummary = res.raw || []; st.summaryFilter = f; st.summaryType = f.reportType;
            $('label15').textContent = ACTIVITY[activity] || ''; $('label15').hidden = !(activity > 0);
            if (!(res.rows || []).length) { gridS.clear(); return; }
            st.printText = SUMMARY_PRINT[f.reportType][0]; $('btnSummaryPrint').textContent = st.printText;
            gridS.show(summaryColumns(f.reportType), res.rows, { onButton: function (col, r) {
                if (col === 'ApprovalDetail') PR.approval(API + '/approval', { filter: st.summaryFilter, summary: true, id: PR.int(r.Id), documentTypeId: PR.int(r.DocumentTypeId) });
            } });
        });
    }
    /** BtnShowSumm_Click :1470. */
    function BtnShowSumm() {
        if (!$('cmbActivitySumm').value) { PR.box('Please Select Activity First...'); $('cmbActivitySumm').focus(); return Promise.resolve(); }
        return GridSummaryFill(0).then(function () { return statusPanels(true); });
    }
    /** btnSummaryPrint_Click :1956 - dtSummary into 391/392/393/394. */
    function summaryPrint() {
        if (!st.dtSummary.length) { PR.box('No Record Found For Display'); return; }
        var p = Object.keys(SUMMARY_PRINT).map(function (k) { return SUMMARY_PRINT[k]; }).find(function (x) { return x[0] === st.printText; });
        if (!p) return;
        PR.run(function () { return PR.printGrid(p[1], p[1].replace(/\.rpt$/, ''), st.dtSummary); });
    }
    /** btnNewSummary_Click :527. */
    function newSummary() {
        fromS.set(new Date()); toS.set(new Date());
        ['txtFromDocNoSumm', 'txtToDocNoSumm'].forEach(function (id) { $(id).value = ''; });
        ['cmbParentcategorySumm', 'CmbCategorySumm', 'cmbItemtypeSumm', 'cmbCropSumm', 'cmbDistrictSumm', 'cmbcitySumm', 'cmbJobLotSumm', 'cmbItemNameSumm', 'cmbCustomerSumm']
            .forEach(function (id) { $(id).value = ''; });
        gridS.clear();
    }
    function refreshCombos() { return GetDropDownData(); }   // btnRefreshSummar_Click :552

    function shortcutsDetail() {
        PR.shortcuts([['Ctrl+S', 'For Show History'], ['Ctrl+E', 'For Close'], ['Esc', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Alt+1', 'For Print 206'], ['Alt+2', 'For Print 205'], ['Alt+3', 'For Open Multi Records'], ['Alt+4', 'For Complete Multi Record'],
            ['Alt+5', 'For Cancel Multi Records'], ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus on From Date'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function shortcutsSummary() {
        PR.shortcuts([['Ctrl+S', 'For Show History'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
            ['Ctrl+T', 'For Tab Transfer'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+ArrowUp', 'For Focus on From Date'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    /** InitializeComponentMethod :390. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            st.init = d; PR.cfg.amountDecimals = d.amountDecimals; PR.cfg.rateDecimals = d.rateDecimals;
            from.set(d.now); to.set(d.now); fromS.set(d.now); toS.set(d.now);
            bindParentCategories(d.parentCategories || []);
            PR.fill('cmbActivitySumm', d.reportTypes.map(function (t, i) { return { Id: i + 1, Name: t }; }), { text: 'Name' });
            $('cmbActivitySumm').value = '1';               // ReportTypeFill: Rows[0] active
            var b = (d.branches || []).map(function (x) { return { id: x.BranchId, name: x.BranchName }; });
            branches.set(b, d.branchId ? [d.branchId] : []); branchesS.set(b, d.branchId ? [d.branchId] : []);
            AllDropDownBind(d.lookups);
            var q = new URLSearchParams(location.search);    // FromDate / ToDate / SupplierId set by a caller
            if (q.get('fromDate')) {
                from.set(q.get('fromDate')); from.setChecked(true); if (q.get('toDate')) to.set(q.get('toDate'));
                if (q.get('supplierId')) $('CmbSupplier').value = q.get('supplierId');
                return GetDetailData(0);
            }
            to.set(new Date(), true);                     // datToDate.Value = DateTime.Today (:430)
            from.focus();
        });
    }

    $('btnshow').addEventListener('click', function () { PR.run(btnshow); });
    $('btnNew').addEventListener('click', reset);
    $('btnPurchsaeOrderDetail').addEventListener('click', function () { print('206-RptPurchaseOrderRegisterRice.rpt'); });
    $('toolStripButton2').addEventListener('click', function () { print('205-InvRptPurchaseOrderDetailRegisterRice.rpt'); });
    $('btnShortCutKeys').addEventListener('click', shortcutsDetail);
    $('btnOpenOrders').addEventListener('click', function () { bulkAction('Open'); });
    $('btnCompleteOrders').addEventListener('click', function () { bulkAction('Complete'); });
    $('btnCancelOrders').addEventListener('click', function () { bulkAction('Cancel'); });
    document.querySelectorAll('[data-activity]').forEach(function (b) { b.addEventListener('click', function () { PR.run(function () { return GetDetailData(PR.int(b.dataset.activity)); }); }); });
    document.querySelectorAll('[data-activity-summ]').forEach(function (b) { b.addEventListener('click', function () { PR.run(function () { return GridSummaryFill(PR.int(b.dataset.activitySumm)); }); }); });
    document.querySelectorAll('input[name=groupBy]').forEach(function (r) {            // RdGroupByCategory_CheckedChanged :2757
        r.addEventListener('change', function () { if (grid.rows.length) { grid.opts.groupBy = radio('groupBy'); grid.draw(false); } });
    });
    $('BtnShowSumm').addEventListener('click', function () { PR.run(BtnShowSumm); });
    $('btnNewSummary').addEventListener('click', newSummary);
    $('btnRefreshSummar').addEventListener('click', function () { PR.run(refreshCombos); });
    $('btnSummaryPrint').addEventListener('click', summaryPrint);
    $('btnShortCutKeySumamryTab').addEventListener('click', shortcutsSummary);
    PR.digitsOnly(['txtFromDocNoSumm', 'txtToDocNoSumm']);
    ['txtSrFrom', 'txtSrTo'].forEach(function (id) {     // OnlytextdecimelFunction
        $(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[0-9.]/.test(e.key)) e.preventDefault(); });
    });
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Purchase Order Register'; });
    PR.gridTools(gridS, 'btnGridPrintSumm', 'btnGridExportSumm', function () { return st.summaryType || 'Summary'; });
    PR.fullscreen('btnFullscreen', 'detailSection'); PR.fullscreen('btnFullscreenSumm', 'summarySection');
    PR.tabs(function () { (PR.currentTab() === 'summary' ? fromS : from).focus(); });
    /** PurchaseOrderHistory_KeyDown :2190. */
    document.addEventListener('keydown', function (e) {
        var k = e.key.toLowerCase(), tab = PR.currentTab();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = '/purchase/reports'; } return; }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); (tab === 'summary' ? shortcutsSummary : shortcutsDetail)(); return; }
        if (tab === 'detail' && e.altKey) {
            var alt = { '1': function () { print('206-RptPurchaseOrderRegisterRice.rpt'); }, '2': function () { print('205-InvRptPurchaseOrderDetailRegisterRice.rpt'); },
                '3': function () { if (!$('btnOpenOrders').classList.contains('is-hidden')) bulkAction('Open'); },
                '4': function () { if (!$('btnCompleteOrders').classList.contains('is-hidden')) bulkAction('Complete'); },
                '5': function () { if (!$('btnCancelOrders').classList.contains('is-hidden')) bulkAction('Cancel'); } }[e.code.replace(/^(Digit|Numpad)/, '')];
            if (alt) { e.preventDefault(); alt(); }
            return;
        }
        if (!e.ctrlKey) return;
        var a;
        if (k === 't') a = function () { PR.selectTab(tab === 'detail' ? 'summary' : 'detail'); (tab === 'detail' ? fromS : from).focus(); };
        else if (k === 'r') a = function () { PR.run(refreshCombos); };
        else if (k === 'n') a = reset;                  // both tabs call btnNew_Click (:2224, :2251)
        else if (tab === 'detail') a = { s: function () { PR.run(btnshow); }, arrowdown: function () { $('gridWrap').focus(); }, arrowup: function () { from.focus(); } }[k];
        else a = { s: function () { PR.run(BtnShowSumm); }, arrowdown: function () { $('gridWrapSumm').focus(); }, arrowup: function () { fromS.focus(); }, p: summaryPrint }[k];
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
