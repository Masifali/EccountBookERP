/* 626 Purchase Analysis By Vehicle - Architecture.WinApp.Lab.LabDataVehicleWiseByParent (module 1011 Lab Report).
   ":NNN" = line in LabDataVehicleWiseByParent.cs. API /api/lab/reports/purchase-by-vehicle. Built on pr_common.js (PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/api/lab/reports/purchase-by-vehicle';
    /* Constants.ExportConstants (Architecture.WinApp.Common/Constants.cs) used by GridSetting / GridSummarySetting. */
    var K = { DocNo: 55, Date: 73, InvoiceNo: 110, String: 130, VehicleNo: 80, Qty: 70, Weight: 80, ExRate: 70, Fcl: 60, Fcy: 60, CurrentStatus: 90 };
    var st = { init: null, rawDetail: [], rawSummary: [] };
    var ZERO_ROW = '...Select Any Value...';                                     // DropDownBind.BindDDL ZeroIndex row (value 0)
    var DOCUMENT_PAGE = '/quality/purchase-analysis';                            // InvLabPurchaseAnalysis, loads ?id=<InvLabAnalysisPurchaseHeader.Id>
    var from = new PR.Picker('datFromDate', 'chkFromDate'), to = new PR.Picker('datToDate', 'chkToDate');
    var detail = new PR.Grid('grdLabDetail', 'lblCount'), summary = new PR.Grid('grdLabSummary');

    /* Janus FormatString "#,#.##" ('h2') and "#,#" ('h0'): no digit placeholder is mandatory, so 0 prints nothing
       and 0.5 prints ".5". 'raw' = a double column without a FormatString. */
    var baseFmt = PR.fmt;
    PR.fmt = function (v, kind) {
        if (kind !== 'h2' && kind !== 'h0') return baseFmt(v, kind);
        if (v === null || v === undefined || v === '') return '';
        var n = Number(v); if (!isFinite(n)) return String(v);
        var s = n.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: kind === 'h2' ? 2 : 0 });
        if (s === '0' || s === '-0') return '';
        return s.replace(/^(-?)0\./, '$1.');
    };
    function num(key, width, fmt, agg, hidden) { return { key: key, width: width, type: 'num', fmt: fmt, agg: agg, hidden: !!hidden }; }

    /** GridSetting :435-534 - dtLabDetail column order (:355-381); grouped by ItemName; Paddy / Rice column sets. */
    function detailColumns(category) {
        var paddy = category === 'Paddy', rice = category === 'Rice';
        return [
            { key: 'Id', hidden: true }, { key: 'GatePassInwardId', hidden: true },
            { key: 'PurchaseType', width: K.InvoiceNo },
            { key: 'LabDate', width: K.Date, type: 'date' },                     // "dd-MMM-yy"
            { key: 'LabNo', width: K.DocNo, link: true },                        // ColumnType.Link :470 - opens the document (web)
            { key: 'Slip', width: 40, link: true },                              // web column: the desktop's LabNo click (653 slip)
            { key: 'OrderId', hidden: true },
            { key: 'OrderNo', width: K.DocNo, link: true },                      // ColumnType.Link :471
            { key: 'SupplierCustomerId', hidden: true },
            { key: 'Supplier', width: K.String }, { key: 'VehicleNo', width: K.VehicleNo },
            { key: 'ItemId', hidden: true }, { key: 'ItemName', width: K.String, hidden: true },   // the group column :440-441
            num('AnalysisQty', K.Qty, 'h2', 'sum'), num('NetWeight', K.Weight, 'h0', 'sum'),
            num('QtyForWtCut', K.Qty, 'h0', 'sum', true), num('WeightCut', K.Weight, 'h2', 'sum'),
            num('ItemRate', K.ExRate, 'h2'), num('RateCut', K.Fcl, 'h2', 'sum'),
            num('NetRate', K.Fcl, 'h2', 'avg'), num('Moisture', K.Fcl, 'h2', 'avg'),
            num('Empty Shell / Trash', K.CurrentStatus, 'raw', null, !paddy), num('Dust/Stone', K.Fcl, 'raw', null, true),
            num('Broken', K.Fcl, 'raw', null, !rice), num('AGL', K.Fcl, 'raw', null, !rice),
            num('Damage', K.Fcl, 'raw', null, !(paddy || rice)), num('Chooba', K.Fcl, 'raw', null, !paddy)];
    }
    /** GridSummarySetting :536-641. */
    function summaryColumns(category) {
        var paddy = category === 'Paddy', rice = category === 'Rice';
        var wm = num('WeightedAvgMoisture', K.Weight, 'h2', 'avg', !paddy);
        if (paddy) wm.caption = 'WtAvgMoisture';                                 // :561
        return [
            { key: 'ItemId', hidden: true }, { key: 'ItemName', width: 220 },
            num('TotalAnalysisQty', K.Qty, 'h2', 'sum'), num('TotalNetWeight', K.Weight, 'h2', 'sum'),
            num('AvgBroken', K.Fcy, 'h2', 'avg', !rice), num('AvgMoisture', K.Fcy, 'h2', 'avg', !paddy),
            num('WeightedAvgBroken', K.Weight, 'h2', 'avg', !rice), wm,
            num('MinMoisture', K.Fcy, 'h2', 'avg', !paddy), num('MaxMoisture', K.Fcy, 'h2', 'avg', !paddy),
            num('MinBroken', K.Fcy, 'h2', 'avg', !rice), num('MaxBroken', K.Fcy, 'h2', 'avg', !rice),
            num('Slab1', K.Fcl, 'h2', 'avg', !paddy), num('Slab2', K.Fcl, 'h2', 'avg', !paddy), num('Slab3', K.Fcl, 'h2', 'avg', !paddy)];
    }

    /** HistoryComboFill :210 - suppliers and items of the parent category (CmbParentCategory_Leave :734). */
    function HistoryComboFill() {
        var current = LabRep.latest('vehicleLookups');                           // a slower, older answer must not overwrite a newer one
        return PR.request(API + '/lookups?parentCategoryId=' + encodeURIComponent(PR.int($('CmbParentCategory').value))).then(function (d) {
            if (!current()) return;
            // BindDDL(..., ZeroIndex: true) + Rows[0].Activate(): the "...Select Any Value..." row (value 0) is the selection.
            LabRep.fill('CmbSupplier', d.suppliers, { zeroRow: ZERO_ROW });
            LabRep.fill('CmbItemName', d.items, { zeroRow: ZERO_ROW });
        });
    }

    /** gridHistory :318. */
    function gridHistory() {
        return PR.request(API + '/rows', {
            parentCategoryId: PR.val('CmbParentCategory'), supplierId: PR.val('CmbSupplier'), itemId: PR.val('CmbItemName'),
            orderNo: $('txtOrderNo').value.trim(), fromDate: from.value(), toDate: to.value(),
            moistureFrom: $('txtMoistureFrom').value.trim(), moistureTo: $('txtMoistureTo').value.trim(),
            slab1From: $('txtSlab1From').value.trim(), slab2From: $('txtSlab2From').value.trim(), slab3From: $('txtSlab3From').value.trim(),
            slab1To: $('txtSlab1To').value.trim(), slab2To: $('txtSlab2To').value.trim(), slab3To: $('txtSlab3To').value.trim()
        }).then(function (res) {
            st.rawDetail = res.rawDetail || []; st.rawSummary = res.rawSummary || [];
            var category = PR.text('CmbParentCategory');
            (res.detail || []).forEach(function (r) { r.Slip = '653'; });
            if ((res.detail || []).length) detail.show(detailColumns(category), res.detail, { groupBy: 'ItemName', onLink: grdLabDetail_LinkClicked });
            else detail.clear();                                                 // grdLabDetail.DataSource = null :391
            if ((res.summary || []).length) summary.show(summaryColumns(category), res.summary, {});
            else summary.clear();                                                // :420
        });
    }
    /** btnshow_Click :299. */
    function btnshow_Click() {
        if (!$('CmbParentCategory').value) { PR.box('Please Select Parent Category First...'); return; }
        return LabRep.run('btnshow', gridHistory);
    }

    /** grdLabDetail_LinkClicked :785. */
    function grdLabDetail_LinkClicked(col, r) {
        if (col === 'LabNo') { LabRep.open(DOCUMENT_PAGE, PR.int(r.Id)); return; }   // web: the lab document itself
        if (col === 'Slip') {                                                    // CommonServices.LabAnalysisReport653(Id) :789
            if (!PR.int(r.Id)) { PR.box('No Record Found For Display'); return; }
            PR.run(function () { return PR.printTemplate('653-RptInvLabPurchaseAnalysisSlip.rpt', { history: String(PR.int(r.Id)) }); });
            return;
        }
        if (col !== 'OrderNo') return;
        var orderId = PR.int(r.OrderId);
        if (orderId <= 0) return;                                                // :800 / :806
        if (st.init.admin || st.init.activityDetailsView) { PR.notPorted('ActivityDetails', 'Order No ' + r.OrderNo); return; }
        PR.box("you don't have Screen Rights");                                  // :819 / :824
    }

    /** Print_Click :746 - DS_Table00 into 665-LabDataVehicleWiseByParentIdRegister.rpt (DS_Table01 is its sub-report). */
    function Print_Click() {
        if (!st.rawSummary.length) { PR.box('No Record Found For Display'); return; }     // :766-769
        return LabRep.run('Print', function () { return PR.printGrid('665-LabDataVehicleWiseByParentIdRegister.rpt', 'Purchase Analysis By Vehicle', st.rawDetail); });
    }

    /** Reset :262 (btnNew_Click). */
    function Reset() {
        $('CmbSupplier').value = '';
        ['txtOrderNo', 'txtMoistureFrom', 'txtMoistureTo', 'txtSlab1From', 'txtSlab2From', 'txtSlab3From', 'txtSlab1To', 'txtSlab2To', 'txtSlab3To']
            .forEach(function (id) { $(id).value = ''; });
        $('CmbParentCategory').value = '';
        from.set(st.init.yearStart, true);
        from.focus();
    }
    /** MakeShortCutKeys :679. */
    function MakeShortCutKeys() {
        PR.shortcuts([['Ctrl+S', 'For show data'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
            ['Ctrl+F5', 'For Focus on From Date'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    /** LabDataVehicleWiseByParent_Load :144. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            st.init = d;
            from.set(d.yearStart, true);                                         // clsGlobalVariables.ActiveYr.Start_Period :155
            to.set(d.now);
            PR.fill('CmbParentCategory', d.parentCategories, {});               // ParentCategoryFill: BindDDL(..., ZeroIndex: false)
            var q = new URLSearchParams(location.search);
            var parentId = PR.int(q.get('parentCategoryId')), itemId = PR.int(q.get('itemId'));
            if (parentId > 0 && itemId > 0) {                                    // :157-163 opened by another form
                $('CmbParentCategory').value = String(parentId);
                return HistoryComboFill().then(function () { $('CmbItemName').value = String(itemId); return gridHistory(); });
            }
            if (d.parentCategories.length) {                                     // CmbParentCategory.Rows[0].Activate() :166
                $('CmbParentCategory').value = String(d.parentCategories[0].Id);
                return HistoryComboFill();
            }
        });
    }

    $('btnshow').addEventListener('click', btnshow_Click);
    $('btnNew').addEventListener('click', Reset);
    $('Print').addEventListener('click', Print_Click);
    $('btnShortCut').addEventListener('click', MakeShortCutKeys);
    $('CmbParentCategory').addEventListener('change', function () { PR.run(HistoryComboFill); });   // CmbParentCategory_Leave :734
    LabRep.fullscreen('btnFullscreen', 'gridSection');
    /* ctrlGrdBar1.MyGrid = grdLabSummary (designer): the grid bar prints / exports the SUMMARY grid. */
    PR.gridTools(summary, 'btnGridPrint', 'btnGridExport', function () { return 'Purchase Analysis By Vehicle'; });
    PR.enterAsTab();
    /** frmlabdatavehiclewise_KeyDown :643 (Ctrl+P and Ctrl+F5 are listed in the ShortCut Keys form but not handled there). */
    document.addEventListener('keydown', function (e) {
        var k = e.key.toLowerCase();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = '/quality'; } return; }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); MakeShortCutKeys(); return; }
        if (!e.ctrlKey) return;
        var a = { n: Reset, s: btnshow_Click, arrowdown: function () { $('summaryWrap').focus(); } }[k];
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
