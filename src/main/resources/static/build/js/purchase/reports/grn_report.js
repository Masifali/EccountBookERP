/* 477 Grn Report - Architecture.WinApp.Inventory_Reports.frmGRNHistory. ":NNN" = line in frmGRNHistory.cs.
   API /purchase/api/reports/grn. Built on pr_common.js (window.PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/purchase/api/reports/grn', SCREEN = 477;
    var st = { init: null, lookups: [], rows: [], raw: [], poLink: false };
    var from = new PR.Picker('GRNfromdate'), to = new PR.Picker('GRNToDate');
    var branches = new PR.CheckList('cmbBranchName');
    var grid = new PR.Grid('DataGridHistory', 'lblCount');
    var K = PR.K;

    /** GridSetting :474 - desktop column order (dtgrid :385), captions, widths, formats, aggregates; Print at position 0. */
    function columns() {
        return [
            { key: 'Print', caption: 'Print', width: 50, button: 'Print' },
            { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true },
            { key: 'BranchName', width: K.String }, { key: 'DocDate', width: K.Date, type: 'date' },
            { key: 'DocNo', width: K.DocNo, open: function (r) { return PR.int(r.Id) > 0 ? [PR.int(r.DocumentTypeId) || 46, PR.int(r.Id)] : null; } }, { key: 'InvoiceNo', width: K.DocNo }, { key: 'SupplierName', width: K.SupplierName },
            { key: 'GpDate', width: K.Date, type: 'date' }, { key: 'GpNo', width: K.DocNo, link: true, open: function (r) { return PR.int(r.InwardGatePassId) > 0 ? [51, PR.int(r.InwardGatePassId)] : null; } },
            { key: 'WareHouse', width: K.WareHouseName }, { key: 'ParentCategory', width: K.Status }, { key: 'Item', width: K.ItemName },
            { key: 'CropYear', width: K.CropYear }, { key: 'JobLot', width: K.JobLot }, { key: 'UOM', width: K.Uom },
            { key: 'ItemQty', width: K.Qty, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'VehicleNo', width: K.VehicleNo }, { key: 'BiltyNo', width: K.BiltyNo },
            { key: 'InwardGatePassId', hidden: true }, { key: 'WagesId', hidden: true },
            { key: 'WagesNo', width: K.DocNo, link: true }, { key: 'OrderId', hidden: true },
            { key: 'PoDate', width: K.Date, type: 'date' },
            { key: 'PoNo', width: K.DocNo, link: st.poLink, linkIf: function (r) { return PR.int(r.OrderId) > 0; }, open: function (r) { return st.poLink && PR.int(r.OrderId) > 0 ? [41, PR.int(r.OrderId)] : null; } },
            { key: 'PackingType', width: K.Uom }, { key: 'GrossWeight', width: K.Weight, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'EB/Unit', width: K.Weight, type: 'num', fmt: 'amt', agg: 'avg' },
            { key: 'EBWTotal', width: K.Weight, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'EbPurAgainstWeight', width: K.Weight, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'AdLsWeight', width: K.AddLess, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'WtCut', width: K.Weight, type: 'num', fmt: 'amt', agg: 'avg' },
            { key: 'WtCutTotal', width: K.Weight, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'ScaleShortWt', width: 90, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'ScaleShortWtApply', caption: 'Apply', width: K.DocNo, type: 'bool' },
            { key: 'SuppShortWt', width: 90, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'SuppShortWtApply', caption: 'Apply', width: K.DocNo, type: 'bool' },
            { key: 'NetBillWeight', width: K.Weight, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'StockEbUnit', width: 90, type: 'num', fmt: 'amt', agg: 'avg' },
            { key: 'StockEbTotal', width: 90, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'StockWeight', width: K.Weight, type: 'num', fmt: 'n2', agg: 'sum' },
            { key: 'CityName', width: K.Status }, { key: 'Transporter', width: K.String },
            { key: 'Freight', width: K.Freight, type: 'num', fmt: 'amt', agg: 'sum' },
            { key: 'EntryDate', width: K.Date, type: 'date' }, { key: 'EntryUser', width: K.EntryUser },
            { key: 'ModifyDate', width: K.Date, type: 'date' }, { key: 'ModifyUser', width: K.EntryUser },
            { key: 'NoOfAttachments', width: K.NoOfAttachments, link: true }];
    }

    function branchParam() { return branches.ids().join(','); }

    /** AllDropDownBind :198 - one InvGrn.GetDataForDropDownFromGrn call split by Activity. */
    function bindLookups(rows) {
        st.lookups = rows || [];
        PR.fill('CmbSupplier', PR.byActivity(rows, 'Supplier'), { zeroIndex: true });
        PR.fill('CmbItem', PR.byActivity(rows, 'Item'), { zeroIndex: true });
        PR.fill('cmbParentCategory', PR.byActivity(rows, 'ParentCategory'), { zeroIndex: true });
        PR.fill('CmbWarehouseName', PR.byActivity(rows, 'Warehouse'), { zeroIndex: true });
        PR.fill('CmbJobLotName', PR.byActivity(rows, 'JobLot'), { zeroIndex: true });
        PR.fill('CmbCityName', PR.byActivity(rows, 'City'), { zeroIndex: true });
    }
    function AllDropDownBind() {
        return PR.request(API + '/lookups?branchIds=' + encodeURIComponent(branchParam())).then(bindLookups);
    }
    /** cmbBranchName_Leave :1058 - rebind, or clear Parent Category / Item / JobLot / Supplier when nothing is ticked. */
    branches.onLeave = function () {
        if (branches.ids().length) { PR.run(AllDropDownBind); return; }
        ['cmbParentCategory', 'CmbItem', 'CmbJobLotName', 'CmbSupplier'].forEach(function (id) { PR.fill(id, [], {}); });
    };

    function filter() {
        return {
            branchIds: branches.ids(), fromDate: from.value(), toDate: to.value(),
            fromNo: PR.val('txtGrnNoFrom'), toNo: PR.val('txtGrnNoTo'), gpFrom: PR.val('GpsNoFrom'), gpTo: PR.val('GpsNoTo'),
            supplierId: PR.val('CmbSupplier'), warehouseId: PR.val('CmbWarehouseName'), parentCategoryId: PR.val('cmbParentCategory'),
            itemId: PR.val('CmbItem'), jobLotId: PR.val('CmbJobLotName'), vehicleNo: $('textBox1').value,
            cityId: PR.val('CmbCityName'), cityName: PR.text('CmbCityName')
        };
    }

    /** gridHisory :341. */
    function gridHisory() {
        if (!branches.ids().length) { branches.el.querySelector('summary').focus(); return Promise.reject(new Error('Select Branch First')); }
        return PR.request(API + '/rows', filter()).then(function (res) {
            st.rows = res.rows || []; st.raw = res.raw || [];
            if (!st.rows.length) { grid.clear(); return; }
            grid.show(columns(), st.rows, { onLink: link, onButton: button });
        });
    }

    function button(col, r) {
        // GenerateReport :1021 -> CommonServices.GrnSlipWithSubReports(Id, DocumentTypeId): the 211 GRN slip.
        if (col === 'Print') PR.run(function () { return PR.printTemplate('211-InvRptGoodsReceiptsNotesRiceSlip.rpt', { id: PR.int(r.Id), documentTypeId: PR.int(r.DocumentTypeId) }); });
    }
    /** DataGridHistory_LinkClicked :671. */
    function link(col, r) {
        if (col === 'NoOfAttachments') PR.attachments(SCREEN, PR.int(r.Id), PR.int(r.DocumentTypeId));
        if (col === 'GpNo' && PR.int(r.InwardGatePassId) > 0)
            PR.run(function () { return PR.printTemplate('251-InvRptInwardGatePassSlip.rpt', { id: PR.int(r.InwardGatePassId) }); });
        if (col === 'WagesNo' && PR.int(r.WagesId) > 0)
            PR.run(function () { return PR.printTemplate('002-ContractorWagesSlip.rpt', { id: PR.int(r.WagesId) }); });
        if (col === 'PoNo' && PR.int(r.OrderId) > 0)
            PR.run(function () { return PR.printTemplate('203-InvRptPurchaseOrderRiceSlip.rpt', { id: PR.int(r.OrderId) }); });
    }

    /** Reset :869. */
    function Reset() {
        from.focus();
        ['CmbSupplier', 'CmbWarehouseName', 'CmbJobLotName', 'CmbCityName'].forEach(function (id) { $(id).value = ''; });
        ['txtGrnNoFrom', 'txtGrnNoTo', 'GpsNoFrom', 'GpsNoTo'].forEach(function (id) { $(id).value = ''; });
    }
    /** btn334Register_Click :996 / btn335Register_Click :1033 - dt (Sp_InvGrn_History rows) into the .rpt. */
    function register(rpt) {
        if (!st.raw.length) { PR.box('Not Record Found For Display'); return; }
        PR.run(function () { return PR.printGrid(rpt, rpt.replace(/\.rpt$/, ''), st.raw); });
    }
    /** toolStripButton1_Click :1082 - usp_GrnRegisterSummaryItemWise with the same filters into 335_01. */
    function register335_01() {
        if (!branches.ids().length) { PR.box('Select Branch First'); return; }
        PR.run(function () {
            return PR.request(API + '/print-args', filter()).then(function (args) {
                return PR.printTemplate('335_01-GrnRegisterSummaryItemWise.rpt', args);
            });
        });
    }
    /** MakeShortCutKeys :795. */
    function shortcuts() {
        PR.shortcuts([['Ctrl+S', 'For Shoe Record'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'],
            ['Alt+1', 'For Print 334-Register'], ['Alt+2', 'For Print 335-Register'], ['Ctrl+F5', 'For Focus on Combo Date'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid History'],
            ['Ctrl+ArrowUp', 'For Focus On Date combo in Filters Box'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    /** frmGRNHistory_Load :162 - AllDropDownBind, ParameterFill, BranchesFill; a caller may pass a parent category. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            st.init = d; st.poLink = !!d.poLink;
            PR.cfg.amountDecimals = d.amountDecimals; PR.cfg.rateDecimals = d.rateDecimals;
            from.set(d.now); to.set(d.now);
            branches.set((d.branches || []).map(function (b) { return { id: b.BranchId, name: b.BranchName }; }), d.branchId ? [d.branchId] : []);
            PR.fill('cmbperemeter', d.dateTypes, { text: 'Parameters' });
            bindLookups(d.lookups);
            var q = new URLSearchParams(location.search), parent = PR.int(q.get('parentCategoryId'));
            if (parent) { $('cmbParentCategory').value = String(parent); return gridHisory(); }   // RequestedByOtherDocument
            $('cmbperemeter').value = '1';                   // Rows[0].Activate() -> "This Day"
            PR.applyDateType(1, from, to, d.yearStart);
            $('cmbperemeter').focus();
        });
    }

    $('cmbperemeter').addEventListener('change', function () { PR.applyDateType($('cmbperemeter').value, from, to, st.init ? st.init.yearStart : ''); });
    $('btnshow').addEventListener('click', function () { PR.run(gridHisory); });
    $('btnNew').addEventListener('click', Reset);
    $('btnRefresh').addEventListener('click', function () { PR.run(AllDropDownBind); });
    $('btn334Register').addEventListener('click', function () { register('334-RptGrnRegister.rpt'); });
    $('btn335Register').addEventListener('click', function () { register('335-RptGrnRegister.rpt'); });
    $('btnRegister335_01').addEventListener('click', register335_01);
    $('btnShortCutKeys').addEventListener('click', shortcuts);
    PR.digitsOnly(['txtGrnNoFrom', 'txtGrnNoTo', 'GpsNoFrom', 'GpsNoTo']);
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Grn History'; });
    PR.fullscreen('btnFullscreen', 'gridSection');
    PR.enterAsTab();
    /** frmGRNHistory_KeyDown :718. */
    document.addEventListener('keydown', function (e) {
        var k = e.key.toLowerCase();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = '/purchase/reports'; } return; }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        if (e.altKey && (e.key === '1' || e.code === 'Numpad1')) { e.preventDefault(); register('334-RptGrnRegister.rpt'); return; }
        if (e.altKey && (e.key === '2' || e.code === 'Numpad2')) { e.preventDefault(); register('335-RptGrnRegister.rpt'); return; }
        if (!e.ctrlKey) return;
        var a = { s: function () { PR.run(gridHisory); }, f5: function () { $('cmbperemeter').focus(); }, n: Reset,
            r: function () { PR.run(AllDropDownBind); }, arrowdown: function () { $('gridWrap').focus(); }, arrowup: function () { $('cmbperemeter').focus(); } }[k];
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
