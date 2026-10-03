/* 633 Purchase Analylsis Report - Architecture.WinApp.Lab.InvLabPurchaseRegister (module 1011 Lab Report).
   ":NNN" = line in InvLabPurchaseRegister.cs. API /api/lab/reports/purchase-register. Built on pr_common.js (PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/api/lab/reports/purchase-register';
    var st = { init: null, dtGroupAnalysis: [] };
    var DOCUMENT_PAGE = '/quality/purchase-analysis';                            // InvLabPurchaseAnalysis, loads ?id=<InvLabAnalysisPurchaseHeader.Id>
    var from = new PR.Picker('txtdatef'), to = new PR.Picker('txtdatet');
    var grid = new PR.Grid('grdGroupAnalysis', 'lblCount');

    /** AllComboBinds :112-165 - nothing is bound when the procedure returns no row (:123). */
    function bindCombos(d) {
        if (!d || !d.bound) return;
        [['cmbItemName', d.items], ['cmbsupplier', d.suppliers], ['CmbParentCategory', d.parentCategories], ['CmbOrder', d.orders]]
            .forEach(function (p) { $(p[0]).value = ''; PR.fill(p[0], p[1], { zeroIndex: true }); });   // ZeroIndex: true, Rows[0] active
    }
    function AllComboBinds() { return PR.request(API + '/lookups').then(bindCombos); }
    /** StatusBind :218-236 - (0, "Rejected"), (1, "Accepted"); ZeroIndex false, nothing selected. */
    function StatusBind() {
        $('cmbstatus').value = '';
        PR.fill('cmbstatus', [{ Id: 0, ReferenceName: 'Rejected' }, { Id: 1, ReferenceName: 'Accepted' }], {});
    }

    /**
     * GridGroupAnalysisSettings :378-420 on dtG (:308-323 + the analysis-parameter columns).
     * The width / Average / "#,##0.###" loop runs over column INDEX 13 .. 13 + n - 1 (:393-400), i.e. WareHouseName,
     * Status and the first n - 2 parameter columns; the last two parameter columns keep the RetrieveStructure defaults.
     */
    function columns(dynamic) {
        var cols = [
            { key: 'Id', hidden: true },
            { key: 'DocDate', width: 75, type: 'short' },
            { key: 'DocNo', width: 42, link: true },                             // ColumnType.Link :385 - opens the document (web)
            { key: 'Slip', width: 40, link: true },                              // web column: the desktop's DocNo click (653 slip)
            { key: 'ItemId', hidden: true },
            { key: 'ItemName', width: 150, hidden: true },                       // the group column :380-381
            { key: 'GatePassInwardId', hidden: true },
            { key: 'GpNo', width: 42, link: true },                              // ColumnType.Link :386
            { key: 'VehicleNo', width: 72 }, { key: 'CropYear', width: 66 }, { key: 'PartyName', width: 150 },
            { key: 'OrderNo', width: 50 },
            { key: 'ItemQty', width: 55, type: 'num', fmt: 'n3', agg: 'sum' },   // "#,##0.###" Sum :387-390
            { key: 'PartyLot', width: 80 }, { key: 'WareHouseName', width: 100 }, { key: 'Status', width: 75 }];
        var n = dynamic.length;
        dynamic.forEach(function (name, k) {
            var formatted = 15 + k <= 13 + n - 1;
            cols.push(formatted ? { key: name, width: 68, type: 'num', fmt: 'n3', agg: 'avg' } : { key: name, width: 100, type: 'num', fmt: 'raw' });
        });
        return cols;
    }

    /** BindGrid :268. */
    function BindGrid() {
        return PR.request(API + '/rows', {
            parentCategoryId: PR.val('CmbParentCategory'), fromDate: from.value(), toDate: to.value(),
            gpNoFrom: $('txtgpnof').value.trim(), gpNoTo: $('txtgpnot').value.trim(),
            status: $('cmbstatus').value, supplierId: PR.val('cmbsupplier'), orderId: PR.val('CmbOrder'), itemId: PR.val('cmbItemName')
        }).then(function (res) {
            st.dtGroupAnalysis = res.raw || [];
            (res.rows || []).forEach(function (r) { r.Slip = '653'; });
            if (!(res.rows || []).length) { grid.clear(); return; }              // grdGroupAnalysis.DataSource = null :371
            grid.show(columns(res.dynamicColumns || []), res.rows, { groupBy: 'ItemName', onLink: grdGroupAnalysis_LinkClicked });
        });
    }
    function btnshow_Click() { return LabRep.run('btnshow', BindGrid); }

    /** grdGroupAnalysis_LinkClicked :422. */
    function grdGroupAnalysis_LinkClicked(col, r) {
        if (col === 'DocNo') { LabRep.open(DOCUMENT_PAGE, PR.int(r.Id)); return; }   // web: the lab document itself
        if (col === 'Slip') {                                                    // 653-RptInvLabPurchaseAnalysisSlip.rpt by Id
            if (!PR.int(r.Id)) { PR.box('No Record Found For Display'); return; }
            PR.run(function () { return PR.printTemplate('653-RptInvLabPurchaseAnalysisSlip.rpt', { history: String(PR.int(r.Id)) }); });
        } else if (col === 'GpNo') {                                             // CommonServices.GatePassInwardSlipAndRegisterReport
            if (!PR.int(r.GatePassInwardId)) { PR.box('No Record Found For Display'); return; }
            PR.run(function () { return PR.printTemplate('251-InvRptInwardGatePassSlip.rpt', { id: PR.int(r.GatePassInwardId) }); });
        }
    }

    /** print_Click_1 :84 - dtGroupAnalysis into 661-LabPurchaseAnalysisRegitser.rpt. */
    function print_Click() {
        if (!st.dtGroupAnalysis.length) { PR.box('Record Not Found For Display'); return; }
        return LabRep.run('print', function () { return PR.printGrid('661-LabPurchaseAnalysisRegitser.rpt', 'Purchase Analysis Register', st.dtGroupAnalysis); });
    }
    /** reset :105 (btnNew "&Refresh") - AllComboBinds, StatusBind, btnshow_Click. */
    function reset() {
        return LabRep.run('btnNew', function () { return AllComboBinds().then(function () { StatusBind(); return BindGrid(); }); });
    }

    /** VoucherValidation_Load :72. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            st.init = d;
            from.set(d.yearStart, true);                                         // ActiveYr.Start_Period :77
            to.set(d.now);
            bindCombos(d.lookups);
            StatusBind();
            $('CmbParentCategory').focus();
        });
    }

    $('btnshow').addEventListener('click', btnshow_Click);
    $('btnNew').addEventListener('click', reset);
    $('print').addEventListener('click', print_Click);
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Purchase Analysis Register'; });
    LabRep.fullscreen('btnFullscreen', 'gridSection');
    PR.enterAsTab();
    /** VoucherValidation_KeyDown :474. */
    document.addEventListener('keydown', function (e) {
        var k = e.key.toLowerCase();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = '/quality'; } return; }
        if (!e.ctrlKey) return;
        var a = { p: print_Click, n: reset, s: btnshow_Click }[k];
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
