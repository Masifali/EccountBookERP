/* 629 "Lab Sample Analysis Report" (ScreenName InvLabSampleRegister) and 632 "Sample Analysis Register"
   (ScreenName SampleAnalysisRegister) - both open Architecture.WinApp.Lab.InvLabSampleRegister.
   ":NNN" = line in InvLabSampleRegister.cs. API /api/lab/reports/sample-register. Built on pr_common.js (window.PR). */
(function () {
    'use strict';
    var $ = PR.$, API = '/api/lab/reports/sample-register';
    var SCREEN = PR.int(document.body.dataset.screenId);      // 629 or 632 - picks the rights row, nothing else
    var BACK = '/quality';
    var DOCUMENT_PAGE = '/quality/sample-analysis';           // InvLabSampleAnalysis, loads ?id=<InvLabSampleAnalysisHeader.Id>
    var st = { init: null, count: 0, columns: [], rows: [] };
    var from = new PR.Picker('txtdatef'), to = new PR.Picker('txtdatet');
    var grid = new PR.Grid('grdGroupAnalysis', 'lblCount');

    /** dtG (:264-282) as RetrieveStructure shows it, then GridGroupAnalysisSettings :327 - Id and ItemId hidden,
        ItemName / PartyName 220, DocNo a link column (ColumnType 5). Every dtG column is a string column,
        so nothing is formatted or totalled. */
    function columns() {
        var cols = [
            { key: 'Id', hidden: true },
            { key: 'DocDate', width: 80, type: 'short' },          // ToShortDateString (:289)
            { key: 'DocNo', width: 70, link: true },               // opens the document (web)
            { key: 'Slip', width: 40, link: true, web: true },     // web column: the desktop's DocNo click (657 slip)
            { key: 'ItemId', hidden: true },
            { key: 'ItemName', width: 220 },
            { key: 'PartyName', width: 220 },
            { key: 'PartyLotRefNo', width: 100 },
            { key: 'ItemQty', width: 70 },
            { key: 'OrderNo', width: 80 },
            { key: 'Status', width: 80 }];
        st.columns.forEach(function (c) { cols.push({ key: c.key, caption: c.caption, width: 90 }); });   // one per Parms_NN of the first row
        return cols;
    }

    /** ItemNameFill :152, ParentCategoryBind :195, StatusBind :214, cmbsupplierfill :180 (all ZeroIndex: false). */
    function bindCombos(d) {
        PR.fill('cmbItemName', d.items, { value: 'Id', text: 'ItemName' });
        PR.fill('CmbParentCategory', d.parentCategories, { value: 'Id', text: 'InvParentCateDescription' });
        PR.fill('cmbstatus', d.statuses, { value: 'Id', text: 'Status' });
        PR.fill('cmbsupplier', d.suppliers, { value: 'Id', text: 'CompanyName' });
        // CmbParentCategory.Rows[0].Activate() (:203) - the first category becomes the selection on every bind.
        if (d.parentCategories && d.parentCategories.length) $('CmbParentCategory').value = String(d.parentCategories[0].Id);
    }

    /** BindGrid :239 - the ReportsParameters the form fills (:246-260). */
    function filter() {
        return {
            screen: SCREEN,
            parentCategoryId: PR.val('CmbParentCategory'),
            fromDate: from.value(), toDate: to.value(),
            status: PR.text('cmbstatus') === '' ? '' : $('cmbstatus').value,      // Text == "" -> ApprovedFilter "All" (:251)
            supplierId: PR.val('cmbsupplier'),
            itemId: PR.val('cmbItemName')
        };
    }
    function BindGrid() {
        return PR.request(API + '/rows', filter()).then(function (res) {
            st.count = PR.int(res.count); st.columns = res.columns || []; st.rows = res.rows || [];
            st.rows.forEach(function (r) { r.Slip = '657'; });
            if (!st.rows.length) { grid.clear(); return; }                        // DataSource = null (:320)
            grid.show(columns(), st.rows, { onLink: link });
        });
    }

    /** grdGroupAnalysis_LinkClicked :338 - on the desktop DocNo opens the 657 sample analysis slip for the row's Id.
        Web: DocNo opens the Sample Analysis document (?id=), the slip is the "Slip" cell beside it. */
    function link(col, r) {
        if (col === 'DocNo') { LabRep.open(DOCUMENT_PAGE, PR.int(r.Id)); return; }
        if (col !== 'Slip') return;
        if (!PR.int(r.Id)) { PR.box('No Record Found For Display'); return; }
        if (!window.CrystalPrint) { PR.box('The print helper is not available on this page.'); return; }
        window.CrystalPrint.open('657-rptinvlabsampleanalysisslipa', { id: PR.int(r.Id) });
    }

    /** print_Click_1 :115 - the loaded rows into 664-LabSampleAnalysisRegister.rpt. The rows are the ones on
        screen (all of them, whatever the filter row shows), so the print always matches the filters used. */
    function print() {
        if (!st.count) { PR.box('Record Not Found For Display'); return; }        // :121
        var cols = columns().filter(function (c) { return !c.hidden && !c.web; });
        var rows = st.rows.map(function (r) {
            var o = {};
            cols.forEach(function (c) { o[c.caption || c.key] = grid.cell(c, r); });
            return o;
        });
        return LabRep.run('print', function () { return PR.printGrid('664-LabSampleAnalysisRegister.rpt', 'Sample Analysis Register', rows); });
    }

    /** reset :142 (btnNew "&Refresh") - rebind the combos, then Show. */
    function reset() {
        return PR.request(API + '/init?screen=' + SCREEN).then(function (d) { st.init = d; bindCombos(d); return BindGrid(); });
    }

    /** VoucherValidation_Load :103 - combos, BindGrid with both pickers still on "now", THEN Date From =
        ActiveYr.Start_Period (:112). */
    function load() {
        return PR.request(API + '/init?screen=' + SCREEN).then(function (d) {
            st.init = d;
            from.set(d.now); to.set(d.now);
            bindCombos(d);
            return BindGrid().then(function () {
                $('CmbParentCategory').focus();
                if (d.yearStart) from.set(d.yearStart);      // the time of day is dropped by the procedure's CAST(... AS date)
            });
        });
    }

    function show() { return LabRep.run('btnshow', BindGrid); }
    function refresh() { return LabRep.run('btnNew', reset); }
    $('btnshow').addEventListener('click', show);
    $('btnNew').addEventListener('click', refresh);
    $('print').addEventListener('click', print);
    PR.gridTools(grid, 'btnGridPrint', 'btnGridExport', function () { return 'Sample Analysis Register'; });
    PR.fullscreen('btnFullscreen', 'gridSection');
    /** VoucherValidation_KeyDown :396. */
    document.addEventListener('keydown', function (e) {
        var k = e.key.toLowerCase();
        if ((e.ctrlKey && k === 'e') || e.key === 'Escape') { if (!document.querySelector('dialog[open]')) { e.preventDefault(); location.href = BACK; } return; }
        if (e.ctrlKey && k === 'p') { e.preventDefault(); print(); return; }
        if (e.ctrlKey && k === 'n') { e.preventDefault(); refresh(); }
    });
    PR.run(load);
})();
