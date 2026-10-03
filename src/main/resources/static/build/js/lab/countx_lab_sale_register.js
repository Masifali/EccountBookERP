/* 628 "Lab Sale Analysis Report" (ScreenName InvLabSaleRegister) - Architecture.WinApp.Lab.InvLabSaleRegister
   (module 1011 Lab Report). ":NNN" = line in InvLabSaleRegister.cs. API /api/lab/reports/lab-sale-register.
   Built on pr_common.js (window.PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/api/lab/reports/lab-sale-register';
    var BACK = '/quality';
    var st = { count: 0, columns: [], rows: [], dtGroupAnalysis: [] };
    var from = new PR.Picker('txtdatef'), to = new PR.Picker('txtdatet');
    var grid = new PR.Grid('grdGroupAnalysis', 'lblCount');

    /** Disable + spinner while the request is in flight; re-enabled on success AND on failure; extra clicks ignored. */
    function withBusy(btn, fn) {
        if (btn && btn.disabled) return Promise.resolve();
        if (btn) { btn.disabled = true; btn.classList.add('btn-busy'); }
        var p;
        try { p = Promise.resolve(fn()); } catch (e) { p = Promise.reject(e); }
        return p.then(function (v) { return v; }, function () { /* PR.run has already shown the message */ })
            .then(function (v) { if (btn) { btn.disabled = false; btn.classList.remove('btn-busy'); } return v; });
    }

    /** dtG (:289-306) as RetrieveStructure shows it, then GridGroupAnalysisSettings :345 - Id, OrderId,
        GatePassOutwardId and ItemId hidden, ItemName / PartyName 220, DocNo and GpSrNo link columns (ColumnType 5).
        Every dtG column is a string column, so nothing is formatted or totalled. */
    function columns() {
        var cols = [
            { key: 'Id', hidden: true },
            { key: 'DocDate', width: 80, type: 'short' },          // ToShortDateString (:314)
            { key: 'DocNo', width: 70, link: true },
            { key: 'ItemId', hidden: true },
            { key: 'ItemName', width: 220 },
            { key: 'GatePassOutwardId', hidden: true },
            { key: 'GpSrNo', width: 70, link: true },
            { key: 'PartyName', width: 220 },
            { key: 'OrderId', hidden: true },
            { key: 'OrderNo', width: 80 },
            { key: 'Status', width: 80 }];
        st.columns.forEach(function (c) { cols.push({ key: c.key, caption: c.caption, width: 90 }); });   // one per Parms_NN of the first row
        return cols;
    }

    /** ItemNameFill :166, ParentCategoryBind :198, StatusBind :231, PurchaseOrderBind :215, cmbsupplierfill :182
        (all ZeroIndex: false; each binds only when its procedure returned rows). */
    function bindCombos(d) {
        function bind(id, rows, text) { if (rows && rows.length) { $(id).value = ''; PR.fill(id, rows, { value: 'Id', text: text }); } }
        bind('cmbItemName', d.items, 'ItemName');
        bind('CmbParentCategory', d.parentCategories, 'InvParentCateDescription');
        // CmbParentCategory.Rows[0].Activate() (:206) - the first category becomes the selection on every bind.
        if (d.parentCategories && d.parentCategories.length) $('CmbParentCategory').value = String(d.parentCategories[0].Id);
        bind('cmbstatus', d.statuses, 'Status');
        bind('CmbOrder', d.orders, 'DocNo');
        bind('cmbsupplier', d.customers, 'CompanyName');
    }

    /** BindGrid :256 - the ReportsParameters the form fills (:260-281). */
    function filter() {
        return {
            parentCategoryId: PR.val('CmbParentCategory'),
            fromDate: from.value(), toDate: to.value(),
            gpNoFrom: $('txtgpnof').value.trim(), gpNoTo: $('txtgpnot').value.trim(),
            status: PR.text('cmbstatus') === '' ? '' : $('cmbstatus').value,      // Text == "" -> ApprovedFilter "All" (:270)
            customerId: PR.val('cmbsupplier'),
            orderId: PR.val('CmbOrder'),
            itemId: PR.val('cmbItemName')
        };
    }
    function BindGrid() {
        return PR.request(API + '/rows', filter()).then(function (res) {
            st.count = PR.int(res.count); st.columns = res.columns || []; st.rows = res.rows || [];
            st.dtGroupAnalysis = res.raw || [];
            if (!st.rows.length) { grid.clear(); return; }                        // DataSource = null (:336)
            grid.show(columns(), st.rows, { onLink: grdGroupAnalysis_LinkClicked });
        });
    }

    /** grdGroupAnalysis_LinkClicked :358. On the desktop the DocNo link does nothing (the comparison's result is
        discarded, :363); here it opens the Sale Analysis page for the row. GpSrNo prints the outward gate pass
        (CommonServices.GatePassOutwardSlipandRegister -> 295-InvRptOutwardGatePassSlipWithItems.rpt by Id). */
    function grdGroupAnalysis_LinkClicked(col, r) {
        if (col === 'DocNo') { window.open('/quality/sale-analysis?id=' + PR.int(r.Id), '_blank'); return; }
        if (col === 'GpSrNo') {
            var id = PR.int(r.GatePassOutwardId);
            if (!(id > 0)) { PR.box('PrintId not found...'); return; }           // CommonServices.cs:9259-9281
            PR.run(function () { return PR.printTemplate('295-InvRptOutwardGatePassSlipWithItems.rpt', { id: id }); });
        }
    }

    /** print_Click_1 :124 - dtGroupAnalysis (the procedure's rows) into 662-LabSaleAnalysisRegitser.rpt. */
    function print_Click(btn) {
        if (!st.count) { PR.box('Record Not Found For Display'); return; }        // :131
        return withBusy(btn, function () {
            return PR.run(function () { return PR.printGrid('662-LabSaleAnalysisRegitser.rpt', 'Sale Analysis Register', st.dtGroupAnalysis); });
        });
    }

    /** reset :151 (btnNew "&Refresh") - rebind the combos, then Show. */
    function reset() {
        return PR.request(API + '/init').then(function (d) { bindCombos(d); return BindGrid(); });
    }
    function btnNew_Click() { return withBusy($('btnNew'), function () { return PR.run(reset); }); }
    function btnshow_Click() { return withBusy($('btnshow'), function () { return PR.run(BindGrid); }); }

    /** VoucherValidation_Load :112 - combos, BindGrid with both pickers still on "now", focus, THEN Date From =
        ActiveYr.Start_Period (:121). */
    function load() {
        return PR.request(API + '/init').then(function (d) {
            from.set(d.now); to.set(d.now);
            bindCombos(d);
            return BindGrid().then(function () {
                $('CmbParentCategory').focus();
                if (d.yearStart) from.set(d.yearStart);
            });
        });
    }

    $('btnshow').addEventListener('click', btnshow_Click);                        // btnshow_Click :251
    $('btnNew').addEventListener('click', btnNew_Click);                          // btnNew_Click :146
    $('print').addEventListener('click', function () { print_Click(this); });
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Sale Analysis Register'; });
    PR.fullscreen('btnFullscreen', 'gridSection');
    /** VoucherValidation_KeyDown :393 - Ctrl+P print, Ctrl+E / Esc close, Ctrl+N refresh. */
    document.addEventListener('keydown', function (e) {
        var k = (e.key || '').toLowerCase();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = BACK; } return; }
        if (e.ctrlKey && k === 'p') { e.preventDefault(); print_Click($('print')); return; }
        if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew_Click(); }
    });
    PR.run(load);
})();
