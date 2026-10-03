/* 478 Gate Pass Report - Architecture.WinApp.Inventory_Reports.frmGatePassReport. ":NNN" = line in frmGatePassReport.cs.
   API /purchase/api/reports/gate-pass. Built on pr_common.js (window.PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/purchase/api/reports/gate-pass', SCREEN = 478, K = PR.K;
    var st = { init: null, raw: [] };
    var from = new PR.Picker('gpFromDate'), to = new PR.Picker('gpToDate');
    var branches = new PR.CheckList('cmbBranchName');
    var grid = new PR.Grid('grdfrm', 'lblCount');

    /** GrdSetting :407 - dtTable order (:355), captions, widths, formats and sums. */
    var COLUMNS = [
        { key: 'Id', hidden: true }, { key: 'BranchName', width: 120 },
        { key: 'GpSrNo', caption: 'Gp No', width: K.GpNo + 20, link: true, open: function (r) { return PR.int(r.Id) > 0 ? [51, PR.int(r.Id)] : null; } }, { key: 'GpDate', width: K.Date, type: 'short' },
        { key: 'GatePassType', width: 90 }, { key: 'OrderType', width: 90 }, { key: 'OrderNo', width: 70 },
        { key: 'SupplierName', width: K.SupplierName }, { key: 'ItemName', width: K.ItemName },
        { key: 'ItemQty', width: K.Qty, type: 'num', fmt: 'n3', agg: 'sum' },
        { key: 'InTime', width: K.DateTime, type: 'datetime' }, { key: 'OutTime', width: K.DateTime, type: 'datetime' },
        { key: 'Freight', width: K.Amount, type: 'num', fmt: 'amt', agg: 'sum' },
        { key: 'NetPaid', width: K.Amount, type: 'num', fmt: 'amt', agg: 'sum' },
        { key: 'SupplierFirstWeight', width: 90, type: 'num', fmt: 'n3', agg: 'sum' },
        { key: 'SupplierSecondWeight', width: 90, type: 'num', fmt: 'n3', agg: 'sum' },
        { key: 'SupplierWeight', width: K.Weight, type: 'num', fmt: 'n3', agg: 'sum' },
        { key: 'FactoryWeight', width: K.Weight, type: 'num', fmt: 'n3', agg: 'sum' },
        { key: 'DifferenceWeight', width: K.Weight, type: 'num', fmt: 'n3', agg: 'sum' },
        { key: 'EntryUser', width: K.EntryUser }, { key: 'Status', width: K.Status }, { key: 'CityName', width: K.CityName },
        { key: 'VehicleType', width: 80 }, { key: 'VehicleNo', width: K.VehicleNo }, { key: 'BiltyNo', width: K.BiltyNo },
        { key: 'Remarks', width: K.Remarks }, { key: 'NoOfAttachments', width: K.NoOfAttachments, link: true }];

    /** ALlDropDown :148 - Supplier and GatePassType rows of USP_GetDataForDropDownFromGPI, ZeroIndex with row 0 active. */
    function bindLookups(rows) {
        PR.fill('cmbSupplierName', PR.byActivity(rows, 'Supplier'), { zeroIndex: true });
        PR.fill('cmbgptype', PR.byActivity(rows, 'GatePassType'), { zeroIndex: true });
    }
    function ALlDropDown() {
        return PR.request(API + '/lookups?branchIds=' + encodeURIComponent(branches.ids().join(','))).then(bindLookups);
    }
    /** IsAcceptFill :201 - Open / Accepted / Rejected, ZeroIndex false, nothing active. */
    function IsAcceptFill() { PR.fill('cmbIsAccept', st.init.statuses, { text: 'Status' }); $('cmbIsAccept').value = ''; }
    /** Datetypefill :250 - ZeroIndex true, Rows[2] ("This Week") active -> cmbDateType_ValueChanged. */
    function Datetypefill() {
        PR.fill('cmbDateType', st.init.dateTypes, { text: 'Parameters', zeroIndex: true });
        $('cmbDateType').value = '2';
        PR.applyDateType(2, from, to, st.init.yearStart);
    }
    /** cmbBranchName_Leave :751. */
    branches.onLeave = function () {
        if (branches.ids().length) { PR.run(ALlDropDown); return; }
        PR.fill('cmbSupplierName', [], {}); PR.fill('cmbgptype', [], {});
    };

    /** GridFill :320. */
    function GridFill() {
        if (!branches.ids().length) return Promise.reject(new Error('Select Branch First'));
        return PR.request(API + '/rows', {
            branchIds: branches.ids(), fromDate: from.value(), toDate: to.value(), fromNo: PR.val('txtGpFrom'), toNo: PR.val('txtGPTo'),
            supplierId: PR.val('cmbSupplierName'), status: PR.text('cmbIsAccept'),
            gatePassTypeId: PR.val('cmbgptype'), gatePassType: PR.text('cmbgptype')
        }).then(function (res) {
            st.raw = res.raw || [];
            if (!(res.rows || []).length) { grid.clear(); return; }
            grid.show(COLUMNS, res.rows, { onLink: link });
        });
    }
    /** grdfrm_LinkClicked :488. */
    function link(col, r) {
        if (col === 'GpSrNo') PR.run(function () { return PR.printTemplate('251-InvRptInwardGatePassSlip.rpt', { id: PR.int(r.Id) }); });
        if (col === 'NoOfAttachments') PR.attachments(SCREEN, PR.int(r.Id), 51);
    }
    /** btnPrint_Click :527 - dt (Sp_GatePassInward_History rows) into 253-InvRptGatePassInwardRegisterA.rpt. */
    function print() {
        if (!st.raw.length) { PR.box('Not Record Found For Display'); return; }
        PR.run(function () { return PR.printGrid('253-InvRptGatePassInwardRegisterA.rpt', 'Gate Pass Inward Register', st.raw); });
    }
    /** btnReset_Click :550. */
    function reset() {
        $('cmbIsAccept').value = ''; $('cmbSupplierName').value = ''; $('txtGpFrom').value = ''; $('txtGPTo').value = '';
        $('cmbDateType').focus(); Datetypefill();
    }
    /** toolStripButton1_Click_1 :579. */
    function refresh() { return ALlDropDown().then(IsAcceptFill); }
    function shortcuts() {
        PR.shortcuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl + P', 'For print'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    /** frmGatePassReport_Load :122. */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            st.init = d; PR.cfg.amountDecimals = d.amountDecimals; PR.cfg.rateDecimals = d.rateDecimals;
            from.set(d.now); to.set(d.now);
            bindLookups(d.lookups);
            IsAcceptFill();
            branches.set((d.branches || []).map(function (b) { return { id: b.BranchId, name: b.BranchName }; }), d.branchId ? [d.branchId] : []);
            Datetypefill();
            $('cmbDateType').focus();
        });
    }

    $('cmbDateType').addEventListener('change', function () { PR.applyDateType($('cmbDateType').value, from, to, st.init ? st.init.yearStart : ''); });
    $('btnshow').addEventListener('click', function () { PR.run(GridFill); });
    $('btnReset').addEventListener('click', reset);
    $('btnRefresh').addEventListener('click', function () { PR.run(refresh); });
    $('btnPrint').addEventListener('click', print);
    $('btnshortcut').addEventListener('click', shortcuts);
    PR.digitsOnly(['txtGpFrom', 'txtGPTo']);
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Gate Pass Inward'; });
    PR.fullscreen('btnFullscreen', 'gridSection');
    /** frmGatePassReport_KeyDown :637 (the form has no Enter-as-Tab handler). */
    document.addEventListener('keydown', function (e) {
        var k = e.key.toLowerCase();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = '/purchase/reports'; } return; }
        if (e.ctrlKey && e.altKey) { e.preventDefault(); shortcuts(); return; }
        if (!e.ctrlKey) return;
        var a = { p: print, s: function () { PR.run(GridFill); }, n: reset, r: function () { PR.run(refresh); },
            f5: function () { $('cmbDateType').focus(); }, arrowup: function () { $('cmbDateType').focus(); }, arrowdown: function () { $('gridWrap').focus(); } }[k];
        if (a) { e.preventDefault(); a(); }
    });
    PR.run(load);
})();
