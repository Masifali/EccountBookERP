/* ============================================================================================
 * Screen 59 "Receivables Aging Report" - Architecture.WinApp.Account_Reports.ReceivableAging (Account_Reports/ReceivableAging.cs, 651 lines)
 * Page: templates/accounts/reports/inact2_receivable_aging.html (route /accounts/reports/receivable-aging-company). Group K (Inact2).
 * Every desktop handler in desktop order (line numbers are ReceivableAging.cs):
 *   VoucherValidation_KeyDown :84  Ctrl+P print, Ctrl+E / Esc close, Ctrl+N reset (no Enter-to-Tab on this form)
 *   CompanyFill             :100  Company.GetAll(OrgCompanyTypeId = organization) -> Id, CompName; BindDDL caption "Comapny Name", ZeroIndex false; Rows[0].Activate()
 *   VoucherValidation_Load  :133  CompanyFill; txtIntervaldays.Focus(); btnshow_Click (the report is shown on open)
 *   reset / btnNew_Click    :140 / :168  txtIntervaldays.Clear(); btnshow_Click; txtIntervaldays.Focus()
 *   print_Click_1           :147  dtrpt rows > 0 -> 111-AcRptReceivablesAging.rpt over the LAST Show's procedure rows (@CompanyName only), else "Not Record Found For Display"
 *   btnshow_Click           :191  ReportsParameters: Organization, Company, UserId, FinancialYear, EndDate, IntervalDays = ToInt(text) -> VoucherReports.ReceivableAging
 *                                 = Sp_Accounts_ReceivablesAging_Rpt (FinancialYearId, OrganizationId, CompanyId, UserId always; IntervalDays when non-zero; EndDate).
 *                                 grd is bound to the procedure rows themselves (RetrieveStructure); no rows -> grd.DataSource = null
 *   GridSettings            :238  Ac1..Ac3 codes/titles, AcId, Ac4LevelCode, RCounter, CompLogoImage, DecimlePointsRate/Amount hidden; Opening, Closing, IntervalAbove,
 *                                 Interval3rd/2nd/1St = Sum, right aligned, FormatString "0,0", TotalFormatString "#,##0.##"; GridAutoAdjustment; layout restore
 *   txtIntervaldays_KeyPress :292  OnlytextNumberFunction (digits + control keys)
 *   btnclose_Click :287 (no control) / ReceivableAging_FormClosing :304 (HistoryStack) / ctrlGrdBar1_Load :173 (layout restore, countx_grid_bar): no further web equivalent
 *   The company combo only shows the list: the report always runs for UserAccount.CompanyId (the desktop never reads cmbcomp).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, I = w.Inact2, el = A.el;
    var API = '/api/accounts/inact2/receivable-aging';
    var lookup = { amountDecimals: 0, companies: [] };
    var dtrpt = [];          // the last Show's procedure rows
    var lastArgs = null;
    var seq = 0;

    // ------------------------------------------------------------------ CompanyFill
    function companyFill() {
        var sel = el('cmbcomp'), list = lookup.companies || [];
        if (!list.length) return;
        A.fill(sel, list, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'CompName'); });
        sel.value = String(A.ci(list[0], 'Id'));                  // Rows[0].Activate()
        sel.dispatchEvent(new Event('change', { bubbles: true }));
    }

    // ------------------------------------------------------------------ grid (GridSettings): only the visible columns, in procedure order
    function num(key, width) {
        return { key: key, caption: key, width: width, type: 'int', align: 'r', sum: true, fmt: function (v) { return I.fmt00(v); }, totalFmt: I.fmtTotal2 };
    }
    var COLS = [
        { key: 'AccountTitle', caption: 'AccountTitle', width: 320 },
        num('Opening', 130), num('Interval1St', 130), num('Interval2nd', 130), num('Interval3rd', 130), num('IntervalAbove', 130), num('Closing', 130)
    ];
    var grid = new A.Grid({
        tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', groupTotals: false,
        decimals: function () { return lookup.amountDecimals; }
    });
    I.totals(grid);

    // ------------------------------------------------------------------ Show (btnshow_Click)
    function show() {
        var b = el('show'); if (b.disabled) return;
        var args = { endDate: el('EndDate').value, intervalDays: A.toInt(el('txtIntervaldays').value) };
        if (!args.endDate) { alert('Please select the End Date'); return; }
        A.busy(b, true);
        var token = ++seq;
        return A.getJson(API + '?' + new URLSearchParams(args).toString()).then(function (rows) {
            if (token !== seq) return;
            dtrpt = rows || [];
            lastArgs = args;
            if (!dtrpt.length) { grid.clear(); return; }                           // grd.DataSource = null
            grid.setData(COLS.map(function (c) { return Object.assign({}, c); }), dtrpt);
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }

    // ------------------------------------------------------------------ print / reset
    function print() {
        var b = el('print'); if (b.disabled) return;
        if (!dtrpt.length || !lastArgs) { alert('Not Record Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/accounts/reports/inact2/print/receivable-aging-111?' + new URLSearchParams(lastArgs).toString())
            .catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() {                                          // btnNew_Click -> reset()
        el('txtIntervaldays').value = '';
        var p = show();
        el('txtIntervaldays').focus();
        return p;
    }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('print').addEventListener('click', print);
    el('reset').addEventListener('click', reset);
    I.digitsOnly(el('txtIntervaldays'));
    d.addEventListener('keydown', function (e) {                // VoucherValidation_KeyDown (KeyPreview)
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); I.closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); print(); }
        else if (k === 'e') { e.preventDefault(); I.closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // ------------------------------------------------------------------ start (VoucherValidation_Load)
    el('EndDate').value = A.today();                            // DateTimePicker default = Now
    grid.render();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        companyFill();
        el('txtIntervaldays').focus();
        return show();
    }).catch(function (e) { alert(e.message); });
}(window, document));
