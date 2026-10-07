/* ============================================================================================
 * Screen 54 "General Ledger Statment Report" - Architecture.WinApp.Account_Reports.GeneralLedgerStatment
 * (Account_Reports/GeneralLedgerStatment.cs, 1000 lines). Page: templates/accounts/reports/inact2_general_ledger_statement.html
 * (route /accounts/reports/general-ledger-statement-desktop). Group K (Inact2). Every desktop handler in desktop order (line numbers are the .cs):
 *   VoucherValidation_KeyDown :89  Enter -> Tab; Ctrl+P print; Ctrl+E / Esc close; Ctrl+N reset
 *   AccountTitleFill        :109  CoaAllocationGetForComboServiceBind (Sp_COAAllocation_GetAllMethod COAForCombobindig) -> Id / AccountTitle, caption "Account Title", ZeroIndex false
 *   CompanyFill             :118  CompanyServiceBind -> Id / CompName, caption "Comapny Name", Rows[0].Activate()
 *   BranchFill              :135  BrancheServiceBind (Sp_Branches_GetAllMethod GetAll) -> Id / BranchName, caption "Branch Name", Rows[0].Activate()
 *   ProjectFill             :152  the SAME branch table, display member "ProjectName", caption "Project Name", Rows[0].Activate()
 *   VoucherValidation_Load  :169  CompanyFill, AccountTitleFill, BranchFill, ProjectFill; fromdate.Focus(); fromdate = ActiveYr.Start_Period; todate = today (no Show)
 *   reset / btnNew_Click    :180 / :225  cmbAccountTitle.Text = ""; fromdate.Focus()
 *   print_Click_1           :186  VoucherReports.GeneralLedgerStatement(FromDate, ToDate, AccountId only) -> no rows "Not Record Found For Display";
 *                                 109-AcRptGeneralLedgerStatement.rpt (@CompanyAddress, @CompanyName); any exception "Report Not Found"
 *   btnshow_Click           :230  AccountId = ToInt(cmbAccountTitle.Value) == 0 -> "Account Title Field Required............"; else GeneralLedgerStatement
 *                                 (AccountId, FromDate, ToDate, BranchesId, ProjectsId) = Sp_GeneralLedgerStatement_Rpt (@ReportType 'Account Statment By Date'; branch / project only when
 *                                 non-zero); rows > 0 -> grd.DataSource + RetrieveStructure (every returned column) + GridSettings; no rows -> the grid keeps what it had
 *   GridSettings            :269  VoucherCode = ColumnType Link (5); ctrlGrdBar1_Load (:282, layout restore -> countx_grid_bar)
 *   grd_LinkClicked         :305  reads Cells["Id"] and Cells["DocumentTypeCode"] and does nothing with them (an exception -> "Hello there is an exception " + message)
 *   toolStripButton17_Click :220 / btnclose_Click :300: Close (no control on the form)
 *   Grid: FilterMode Automatic, GroupByBoxVisible false, RecordNavigator, TotalRow bottom (no aggregate columns), Verdana 8.25.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, I = w.Inact2, el = A.el;
    var API = '/api/accounts/inact2/ledger-statement';
    var lookup = { amountDecimals: 0, yearStart: null, companies: [], coa: [], branches: [] };
    var seq = 0;

    function selFirst(id) {                                                              // Rows[0].Activate()
        var sel = el(id), o = null;
        for (var i = 0; i < sel.options.length; i++) if (sel.options[i].value !== '') { o = sel.options[i]; break; }
        if (o) { sel.value = o.value; sel.dispatchEvent(new Event('change', { bubbles: true })); }
    }
    function companyFill() { A.fill(el('cmbcomp'), lookup.companies, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'CompName'); }); selFirst('cmbcomp'); }
    function accountTitleFill() { A.fill(el('cmbAccountTitle'), lookup.coa, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'AccountTitle'); }); }
    function branchFill() { A.fill(el('cmbbranch'), lookup.branches, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'BranchName'); }); selFirst('cmbbranch'); }
    function projectFill() {
        /* display member "ProjectName" of the branch table; a row without that column can only show its value */
        A.fill(el('cmbproject'), lookup.branches, function (r) { return A.ci(r, 'Id'); },
            function (r) { var p = A.ci(r, 'ProjectName'); return p == null ? A.ci(r, 'Id') : p; });
        selFirst('cmbproject');
    }

    function s(v) { return v === null || v === undefined ? '' : String(v); }
    var grid = new A.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', groupTotals: false,
        decimals: function () { return 0; }, onLink: function (row) { link(row); } });
    I.totals(grid, true);

    /* RetrieveStructure: one column per column the procedure returns */
    function columnsOf(rows) {
        return Object.keys(rows[0]).map(function (k) {
            var sample = null;
            for (var i = 0; i < rows.length && sample === null; i++) if (rows[i][k] !== null && rows[i][k] !== undefined) sample = rows[i][k];
            var c = { key: k, caption: k, width: 110 };
            if (typeof sample === 'number') { c.type = 'int'; c.align = 'r'; c.fmt = function (v) { return s(v); }; }
            else if (typeof sample === 'string' && /^\d{4}-\d{2}-\d{2}(T|$)/.test(sample)) { c.type = 'date'; c.fmt = function (v) { return A.fmtDate(v, true); }; }
            if (k === 'VoucherCode') c.link = true;                                                     // ColumnType Link
            return c;
        });
    }

    // ------------------------------------------------------------------ Show (btnshow_Click)
    function show() {
        var b = el('show'); if (b.disabled) return;
        var acc = A.toInt(el('cmbAccountTitle').value);
        if (acc === 0) { alert('Account Title Field Required............'); return; }
        var args = { accountId: acc, fromDate: el('fromdate').value, toDate: el('todate').value,
                     branchId: A.toInt(el('cmbbranch').value), projectId: A.toInt(el('cmbproject').value) };
        if (!args.fromDate || !args.toDate) { alert('Please select the From Date and To Date'); return; }
        A.busy(b, true);
        var token = ++seq;
        A.getJson(API + '?' + new URLSearchParams(args).toString()).then(function (rows) {
            if (token !== seq) return;
            if (rows && rows.length) grid.setData(columnsOf(rows), rows);                              // no rows: the grid is not touched
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }

    // ------------------------------------------------------------------ grd_LinkClicked
    function link(row) {
        try {
            if (A.ci(row, 'Id') == null) throw new Error('Object reference not set to an instance of an object.');
            if (A.ci(row, 'DocumentTypeCode') == null) throw new Error('Object reference not set to an instance of an object.');
        } catch (e) { alert('Hello there is an exception ' + e.message); }
    }

    // ------------------------------------------------------------------ print / reset
    function print() {
        var b = el('print'); if (b.disabled) return;
        var q = new URLSearchParams({ accountId: A.toInt(el('cmbAccountTitle').value), fromDate: el('fromdate').value, toDate: el('todate').value });
        A.busy(b, true);
        A.openPdf('/accounts/reports/inact2/print/ledger-statement-109?' + q.toString())
            .catch(function (e) { alert(/No Record Found For Display/i.test(e.message) ? 'Not Record Found For Display' : 'Report Not Found'); })
            .then(function () { A.busy(b, false); });
    }
    function reset() {                                          // btnNew_Click -> reset()
        var sel = el('cmbAccountTitle');
        sel.value = ''; sel.dispatchEvent(new Event('change', { bubbles: true }));
        el('fromdate').focus();
    }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('print').addEventListener('click', print);
    el('reset').addEventListener('click', reset);
    I.enterTab(['cmbcomp', 'cmbbranch', 'cmbproject', 'fromdate', 'todate', 'cmbAccountTitle', 'show']);
    d.addEventListener('keydown', function (e) {                // VoucherValidation_KeyDown (KeyPreview)
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); I.closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); print(); }
        else if (k === 'e') { e.preventDefault(); I.closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // ------------------------------------------------------------------ start (VoucherValidation_Load)
    el('fromdate').value = A.today();
    el('todate').value = A.today();
    grid.render();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        companyFill(); accountTitleFill(); branchFill(); projectFill();
        el('fromdate').focus();
        el('fromdate').value = lookup.yearStart || A.today();                        // ActiveYr.Start_Period
        el('todate').value = A.today();
    }).catch(function (e) { alert(e.message); });
}(window, document));
