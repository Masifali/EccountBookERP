/* ============================================================================================
 * Screen 87 "Post Dated Cheque Reports" - Architecture.WinApp.Account_Reports.frmPostDatedChequeReports
 * Page: templates/accounts/reports/acrpt4_post_dated_cheque.html (route /accounts/reports/post-dated-cheque-reports).
 * Every desktop handler, in desktop order (frmPostDatedChequeReports.cs):
 *   frmPostDatedChequeReports_Load -> ChequeFromDate = now, GlAccountFill() (CoaAllocationAccountTitleByAccountTypeIds("15") -> Bank GL Account),
 *                               ChequeAccountFill() (("3,6,8") -> Cheque Account), ChequeTypeFill() (empty method; the Cheque Type combo is hidden).
 *   btnshow_Click            -> VoucherReports.PostDatedChecqueReport (USP_PostDatedChecque_Report): FromDate, ToDate only when its box is ticked,
 *                               cheque numbers (Conversion.ToInt of the text, 0 = not sent), bank id, account id, IsApproved (Unclear = true, Clear = false).
 *                               dtHistory = Tables[0]. rows > 0 -> the 12 typed grid columns, GridWrappingAndColumnSettings(grid, 2, decimals) and the
 *                               widths below, AccountId / AccountIdBank hidden, DebitAmount Sum / right / stringFormatboth; no rows -> ClearStructure().
 *   txtChequeNoFrom / To _KeyPress -> OnlytextNumberFunction (digits only).
 *   btnPrint_Click (177-Print) -> dtHistory.Rows.Count == 0 -> "Record Not Found For Display"; else 177-PostDatedChequeRegister.rpt over dtHistory
 *                               (CompanyName, CompanyAddress, PrintedBy). Pressed before any Show -> the desktop's NullReferenceException text.
 *   btnRefresh (&Refresh)    -> no Click handler on the desktop: nothing happens.
 *   The form has no KeyPreview / KeyDown: no keyboard shortcuts, no Enter = Tab.
 *   ctrlGrdBar1_Load         -> saved grid layout (countx_grid_bar.js).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt4, el = A.el, API = '/api/accounts/acrpt4/pchq', seq = 0;
    var RPT = '177-PostDatedChequeRegister.rpt';
    var dtHistory = null, decimals = 0;

    var grid = A.createGrid({ table: el('grd'), scroller: el('gridWrap'), nav: el('nav'), navText: el('navText'),
        headerLines: 2, groupTotals: true, totalRow: true });              // GroupByBoxVisible = false
    grid.clear();

    function fmtBoth() {                                                     // clsGlobalVariables.stringFormatboth
        var z = ['', '0', '00', '000', '0000'][decimals] || '';
        return '#,##0.' + z + ';(0,0.' + z + '); 0';
    }
    /* Conversion.ToInt: Convert.ToInt32 of the value, 0 when it cannot be converted. */
    function toInt(v) { var s = String(v == null ? '' : v).trim(); return /^[-+]?\d+$/.test(s) ? parseInt(s, 10) : 0; }

    function columns() {
        return [
            { key: 'AccountId', caption: 'AccountId', hidden: true, width: 60, type: 'int', align: 'center' },
            { key: 'AccountIdBank', caption: 'AccountIdBank', hidden: true, width: 60, type: 'int', align: 'center' },
            { key: 'VoucherDate', caption: 'VoucherDate', width: 73, type: 'date' },
            { key: 'VoucherNo', caption: 'VoucherNo', width: 60, type: 'int', align: 'center' },
            { key: 'VoucherType', caption: 'VoucherType', width: 70 },
            { key: 'ChequeDate', caption: 'ChequeDate', width: 73, type: 'date' },
            { key: 'ChequeNo', caption: 'ChequeNo', width: 80, type: 'int', align: 'center' },
            { key: 'ChequeType', caption: 'ChequeType', width: 80 },
            { key: 'AccountTitleBank', caption: 'AccountTitleBank', width: 170 },
            { key: 'AccountTitle', caption: 'AccountTitle', width: 170 },
            { key: 'ChequeStatus', caption: 'ChequeStatus', width: 75 },
            { key: 'DebitAmount', caption: 'DebitAmount', width: 90, type: 'num', align: 'right', sum: true, fmt: fmtBoth(), totalFmt: fmtBoth() }
        ];
    }

    function mapRows(rows) {
        return rows.map(function (r) {
            return {
                AccountId: toInt(A.ci(r, 'AccountId')), AccountIdBank: toInt(A.ci(r, 'AccountIdBank')),
                VoucherDate: A.ci(r, 'VoucherDate'), VoucherNo: toInt(A.ci(r, 'VoucherNo')), VoucherType: A.ci(r, 'VoucherType'),
                ChequeDate: A.ci(r, 'CheqDate'), ChequeNo: toInt(A.ci(r, 'CheqNo')), ChequeType: A.ci(r, 'CheqType'),
                AccountTitleBank: A.ci(r, 'AccountTitleBank'), AccountTitle: A.ci(r, 'AccountTitle'),
                ChequeStatus: A.ci(r, 'ChequeStatus'), DebitAmount: A.ci(r, 'DebitAmount')
            };
        });
    }

    // ------------------------------------------------------------------ Show
    function show() {                                                        // btnshow_Click
        var b = el('show'); if (b.disabled) return Promise.resolve();
        var q = new URLSearchParams({
            fromDate: el('chqFrom').value || A.today(),
            bankId: A.intArg(el('bank').value), accountId: A.intArg(el('acct').value),
            chqNoFrom: toInt(el('noFrom').value), chqNoTo: toInt(el('noTo').value),
            approved: el('rbUnclear').checked ? 'true' : 'false'              // Unclear -> true, Clear -> false
        });
        if (el('toChk').checked) q.set('toDate', el('chqTo').value || A.today());
        A.busy(b, true);
        var token = ++seq;
        return A.getJson(API + '/data?' + q.toString()).then(function (data) {
            if (token !== seq) return;
            dtHistory = data.rows || [];
            if (dtHistory.length) grid.setData(columns(), mapRows(dtHistory));
            else grid.clear();                                                // DataGridHistory.ClearStructure()
        }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ Print (177-Print)
    function print177() {
        if (dtHistory === null) { alert('Object reference not set to an instance of an object.'); return; }
        if (!dtHistory.length) { alert('Record Not Found For Display'); return; }
        var rows = dtHistory.map(function (r) { var o = Object.assign({}, r); o.CompLogoImage = null; return o; });
        A.postGrid(RPT, rows, null, el('print177'));
    }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('print177').addEventListener('click', print177);
    /* btnRefresh has no Click handler on the desktop: the button is inert. */
    el('toChk').addEventListener('change', function () { el('toWrap').classList.toggle('off', !this.checked); el('chqTo').disabled = !this.checked; });   // unchecked DateTimePicker = greyed, not editable
    ['noFrom', 'noTo'].forEach(function (id) {                               // OnlytextNumberFunction: digits only
        el(id).addEventListener('keypress', function (e) { if (e.key.length === 1 && !/\d/.test(e.key) && !e.ctrlKey) e.preventDefault(); });
        el(id).addEventListener('input', function () { var v = this.value.replace(/\D/g, ''); if (v !== this.value) this.value = v; });
    });

    // ------------------------------------------------------------------ Load
    el('chqFrom').value = A.today(); el('chqTo').value = A.today(); el('chqTo').disabled = true;          // ChequeFromDate.Value = DateTime.Now
    A.getJson(API + '/init').then(function (init) {
        decimals = init.amountDecimals || 0;
        if (init.bankAccounts && init.bankAccounts.length) A.fillSelect(el('bank'), init.bankAccounts, 'Id', 'AccountTitle', { keep: false });
        if (init.chequeAccounts && init.chequeAccounts.length) A.fillSelect(el('acct'), init.chequeAccounts, 'Id', 'AccountTitle', { keep: false });
    }).catch(function (e) { alert(e.message); });
}(window, document));
