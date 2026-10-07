/* ============================================================================================
 * Screen 57 "General Journal Summery" - Architecture.WinApp.Account_Reports.GeneralJournalSummeryRegister
 * (Account_Reports/GeneralJournalSummeryRegister.cs, 537 lines). Page: templates/accounts/reports/inact2_general_journal_summary.html
 * (route /accounts/reports/general-journal-summary-register). Group K (Inact2). Every desktop handler in desktop order (line numbers are the .cs):
 *   GeneralJournalSummeryRegister_Load :69  GridBind(); RegisterTypeFill(); cmbRegisterType.Focus()
 *   RegisterTypeFill        :76   table Id / RegisterType = (1, "Summery"), (2, "Detail"); BindDDL caption "Register Type", ZeroIndex false; column 1 width 182
 *   GridBind                :94   VoucherReports.GeneralJournalReport = Sp_Accounts_GeneralJournal_SummeryRegister_Rpt with OrganizationId + CompanyId ONLY (the BLL adds
 *                                 @DateFrom / @DateTo under "if (CheckDateTimeNull(date))", so a real From / To is never sent); rows -> 13 string columns RegisterType,
 *                                 Document Type (V_Type), Voucher No (V_No), VoucherDate dd-MMM-yyyy, RefAccountTitle, VoucherAmount, Remarks, ChequeNo, AgainstAccountTitle,
 *                                 EntryUser, EntryDate dd-MMM-yyyy, Is_Approved, PostDate dd-MMM-yyyy; no rows -> the grid is not touched. A column the procedure does not
 *                                 return makes dtGrd.Rows[i][...] throw "Column 'X' does not belong to table ." -> MessageBox (the server raises exactly that text)
 *   Reset / btnRefresh_Click :137 / :205  cmbRegisterType.Text = ""; txtDateFrom = ActiveYr.Start_Period; txtDateTo = now; GridBind(); cmbRegisterType.Focus()
 *   KeyDown                 :153  Enter -> Tab; Ctrl+P btnPrint_Click; Ctrl+N btnRefresh_Click; Ctrl+E / Esc close
 *   gridSetting             :180  widths RegisterType 100, Voucher No 90, VoucherDate 100, RefAccountTitle 200, VoucherAmount 110, Remarks 120, ChequeNo 80,
 *                                 AgainstAccountTitle 200, EntryUser 110, EntryDate 100, Is_Approved 100, PostDate 100; "Document Type" hidden;
 *                                 VoucherAmount right + "0,0" + Sum
 *   btnshow_Click           :200  GridBind()
 *   btnPrint_Click          :210  empty handler (nothing happens)
 *   cmbRegisterType_TextChanged :214  rows loaded -> text not empty: rebuild the grid from the rows whose RegisterType == the text (only when some row matches);
 *                                 text empty: GridBind()
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, I = w.Inact2, el = A.el;
    var API = '/api/accounts/inact2/general-journal';
    var lookup = { yearStart: null };
    var dtGrd = [];          // dtGrd = the last GridBind result
    var seq = 0;

    var COLS = [
        { key: 'RegisterType', caption: 'RegisterType', width: 100 },
        { key: 'Document Type', caption: 'Document Type', hidden: true },
        { key: 'Voucher No', caption: 'Voucher No', width: 90 },
        { key: 'VoucherDate', caption: 'VoucherDate', width: 100 },
        { key: 'RefAccountTitle', caption: 'RefAccountTitle', width: 200 },
        { key: 'VoucherAmount', caption: 'VoucherAmount', width: 110, type: 'amount', align: 'r', sum: true, fmt: function (v) { return v === null || v === undefined ? '' : String(v); }, totalFmt: I.fmt00 },
        { key: 'Remarks', caption: 'Remarks', width: 120 },
        { key: 'ChequeNo', caption: 'ChequeNo', width: 80 },
        { key: 'AgainstAccountTitle', caption: 'AgainstAccountTitle', width: 200 },
        { key: 'EntryUser', caption: 'EntryUser', width: 110 },
        { key: 'EntryDate', caption: 'EntryDate', width: 100 },
        { key: 'Is_Approved', caption: 'Is_Approved', width: 100 },
        { key: 'PostDate', caption: 'PostDate', width: 100 }
    ];
    function s(v) { return v === null || v === undefined ? '' : String(v); }
    function dmy(v) { return A.fmtDate(v, true); }                                    // Conversion.ToDateTime(...).ToString("dd-MMM-yyyy")
    function row(r) {
        var c = A.ci;
        return {
            'RegisterType': c(r, 'RegisterType'), 'Document Type': c(r, 'V_Type'), 'Voucher No': c(r, 'V_No'), 'VoucherDate': dmy(c(r, 'VoucherDate')),
            'RefAccountTitle': c(r, 'RefAccountTitle'), 'VoucherAmount': c(r, 'VoucherAmount'), 'Remarks': c(r, 'Remarks'), 'ChequeNo': c(r, 'ChequeNo'),
            'AgainstAccountTitle': c(r, 'AgainstAccountTitle'), 'EntryUser': c(r, 'EntryUser'), 'EntryDate': dmy(c(r, 'EntryDate')),
            'Is_Approved': c(r, 'Is_Approved'), 'PostDate': dmy(c(r, 'PostDate'))
        };
    }
    var grid = new A.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', groupTotals: false, decimals: function () { return 0; } });
    I.totals(grid);
    function bind(rows) { grid.setData(COLS.map(function (c) { return Object.assign({}, c); }), rows.map(row)); }

    // ------------------------------------------------------------------ GridBind
    function gridBind() {
        var b = el('show'), token = ++seq;
        A.busy(b, true);
        return A.getJson(API).then(function (rows) {
            if (token !== seq) return;
            dtGrd = rows || [];
            if (dtGrd.length) bind(dtGrd);                                           // no rows: nothing happens
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }

    // ------------------------------------------------------------------ cmbRegisterType_TextChanged
    function textChanged() {
        if (!dtGrd.length) return;
        var sel = el('cmbRegisterType'), text = sel.value ? sel.options[sel.selectedIndex].textContent.trim() : '';
        if (text !== '') {
            var match = dtGrd.filter(function (r) { return s(A.ci(r, 'RegisterType')) === text; });
            if (match.length) bind(match);
        } else gridBind();
    }
    function registerTypeFill() {
        A.fill(el('cmbRegisterType'), [{ Id: 1, RegisterType: 'Summery' }, { Id: 2, RegisterType: 'Detail' }],
            function (r) { return r.Id; }, function (r) { return r.RegisterType; });
    }

    // ------------------------------------------------------------------ Reset
    function reset() {
        var sel = el('cmbRegisterType');
        if (sel.value !== '') { sel.value = ''; sel.dispatchEvent(new Event('change', { bubbles: true })); }      // cmbRegisterType.Text = ""
        el('txtDateFrom').value = lookup.yearStart || A.today();
        el('txtDateTo').value = A.today();
        var p = gridBind();
        var f = A.focusable('cmbRegisterType'); if (f) f.focus();
        return p;
    }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', gridBind);                                  // btnshow_Click
    el('btnRefresh').addEventListener('click', reset);                               // btnRefresh_Click -> Reset
    el('btnPrint').addEventListener('click', function () { /* btnPrint_Click has an empty body */ });
    el('cmbRegisterType').addEventListener('change', textChanged);
    I.enterTab(['cmbRegisterType', 'txtDateFrom', 'txtDateTo', 'show']);
    d.addEventListener('keydown', function (e) {                // GeneralJournalSummeryRegister_KeyDown (KeyPreview)
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); I.closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); /* btnPrint_Click: nothing */ }
        else if (k === 'n') { e.preventDefault(); reset(); }
        else if (k === 'e') { e.preventDefault(); I.closeForm(); }
    });

    // ------------------------------------------------------------------ start (GeneralJournalSummeryRegister_Load)
    el('txtDateFrom').value = A.today();
    el('txtDateTo').value = A.today();                          // DateTimePicker default = Now
    grid.render();
    gridBind().then(function () {
        registerTypeFill();
        var f = A.focusable('cmbRegisterType'); if (f) f.focus();
    });
    A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; }).catch(function () { });
}(window, document));
