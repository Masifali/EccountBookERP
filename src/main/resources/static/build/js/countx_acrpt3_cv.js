/* ============================================================================================
 * Screen 67 "Customer Voucher Report" - Architecture.WinApp.Account_Reports.CostomerWiseVoucherSlip
 * Page: templates/accounts/reports/acrpt3_customer_voucher_report.html (route /accounts/reports/customer-voucher-report).
 * Every desktop handler, in desktop order (CostomerWiseVoucherSlip.cs):
 *   VoucherValidation_Load -> CmbPartyName.Focus(), datToDate = today, CashBankFill(), PartyAccountFill(), radioPayment + chkCash checked.
 *   CashBankFill           -> VoucherHead.GetCashAndBanksAccountByDate(datToDate) (Sp_Vouchers_GetMethods 'GetCashAndBanksAccountByDate'); only with rows:
 *                             text cleared, BindDDLNew(Id, AccountTitle, "Account Title") as a CHECKED list (Selected column, "," separator).
 *   PartyAccountFill       -> VoucherHead.GetPartiesFromVouchersByDate(datToDate) (Sp_Vouchers_GetMethods 'GetPartiesFromVouchersByDate'); BindDDL(Id, AccountTitle,
 *                             "Account Title", ZeroIndex false) with rows, else Text = "" and DataSource = null.
 *   datToDate_Leave        -> CashBankFill(), PartyAccountFill()   (web: when the date value changed since the last fill).
 *   btnshow_Click          -> FormValidation: no party -> "Party Account Field is Required" (focus party); neither Cash nor Bank -> "Selection Of Cash Or Bank Field is Required"
 *                             (focus Cash); VoucherReports.VoucherSlipPartyWise (Usp_VouchersPaymentReceiptSlip_Report: FromDate = ToDate = the date, AccountId, SlipFlag
 *                             Payment / Receipt, CashVouchers 'Cash', BankVouchers 'Bank', AccountIds ",id,id" built from the ticked titles against the loaded Bank & Cash list,
 *                             sent only when the combo has text); rows -> grd + GridSettings(), no rows -> grd.DataSource = null.
 *   GridSettings           -> vhAccountCode, coagAccountCode, coagAccountTitle, PostDate, ApprovedUserName, VoucherDate, AccountCode, AccountTitle hidden; vhAccountTitle 150 "Bank Cash Ac",
 *                             Comments 200, DocumentTypeCode "Voucher Type", CheqReference "Cheque No"; Debit / Credit "0,0" with "#,##0.##" totals, right aligned, summed;
 *                             VoucherCode is a link column (the form has no LinkClicked handler, so a click does nothing).
 *   print_Click_1 / ShowReport -> dtvoucher with rows -> 100-PartyWisePaymentReceiptsVoucherSlip.rpt, else "Record Not Found For Display".
 *   btnNew_Click / reset   -> CmbPartyName.Focus(), PartyAccountFill(), radioPayment + chkCash checked (Bank, the Bank & Cash list, the date and the grid are left alone).
 *   VoucherValidation_KeyDown -> Ctrl+P print, Ctrl+E / Esc close, Ctrl+N Refresh (no Enter = Tab on this form).
 *   ctrlGrdBar1_Load       -> saved grid layout (countx_grid_bar.js).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt3, el = A.el;
    var API = '/api/accounts/acrpt3/cv';
    var RPT = '100-PartyWisePaymentReceiptsVoucherSlip.rpt';

    var dtCashBank = [];             // the Bank & Cash list as last bound (Id, AccountTitle)
    var dtvoucher = [];              // the procedure's rows of the last Show
    var grid, seq = 0, filledOn = null;

    var HIDDEN = { vhaccountcode: 1, coagaccountcode: 1, coagaccounttitle: 1, postdate: 1, approvedusername: 1, voucherdate: 1, accountcode: 1, accounttitle: 1 };
    var CAPTION = { vhaccounttitle: 'Bank Cash Ac', documenttypecode: 'Voucher Type', cheqreference: 'Cheque No' };
    var WIDTH = { accounttitle: 150, vhaccounttitle: 150, comments: 200 };
    var DATES = { voucherdate: 1, chequedate: 1, entrydate: 1, postdate: 1 };

    function dateText() { return el('datToDate').value || A.today(); }

    // ------------------------------------------------------------------ CashBankFill / PartyAccountFill
    function cashBankFill() {
        return A.getJson(API + '/cashbank?date=' + encodeURIComponent(dateText())).then(function (rows) {
            if (!rows || !rows.length) return;                                    // if (dtCashBank.Rows.Count > 0 && dtCashBank != null) ... only
            dtCashBank = rows;
            A.fillSelect(el('bank'), rows, 'Id', 'AccountTitle', { keep: false });
            for (var i = 0; i < el('bank').options.length; i++) el('bank').options[i].selected = false;      // Text = ""
            if (w.DesktopCombo && w.DesktopCombo.refresh) w.DesktopCombo.refresh();
            el('bank').dispatchEvent(new Event('change', { bubbles: true }));
        });
    }
    function partyAccountFill() {
        return A.getJson(API + '/parties?date=' + encodeURIComponent(dateText())).then(function (rows) {
            A.fillSelect(el('party'), rows || [], 'Id', 'AccountTitle', { keep: false });      // no rows: Text = "", DataSource = null
            if (w.DesktopCombo && w.DesktopCombo.refresh) w.DesktopCombo.refresh();
            el('party').dispatchEvent(new Event('change', { bubbles: true }));
        });
    }
    function fills() {
        filledOn = dateText();
        return Promise.all([cashBankFill(), partyAccountFill()]);
    }

    // ------------------------------------------------------------------ btnshow_Click
    function accountIds() {
        var texts = A.checkedTexts(el('bank')), text = texts.join(',');
        if (text === '') return '';
        var ids = '';
        (text + ',').split(',').forEach(function (author) {                      // dtCashBank.Select("AccountTitle='" + author + "'") -> first row's first column
            var low = author.toLowerCase();
            for (var i = 0; i < dtCashBank.length; i++) {
                if (String(dtCashBank[i].AccountTitle).toLowerCase() === low) { ids += ',' + dtCashBank[i].Id; break; }
            }
        });
        return ids;
    }
    function columnsFor(row) {
        var keys = Object.keys(row);
        return keys.map(function (k) {
            var low = k.toLowerCase();
            var o = { key: k, caption: CAPTION[low] || k, width: WIDTH[low] || (DATES[low] ? 90 : 100), hidden: !!HIDDEN[low] };
            if (DATES[low]) o.type = 'date';
            if (low === 'vouchercode') { o.type = 'int'; o.link = true; }
            if (low === 'debitamount' || low === 'creditamount') { o.type = 'num'; o.fmt = '0,0'; o.totalFmt = '#,##0.##'; o.sum = true; }
            return o;
        });
    }
    function show() {
        var b = el('show'); if (b.disabled) return Promise.resolve();
        if (!A.intArg(el('party').value)) { alert('Party Account Field is Required'); A.focusOf('party').focus(); return Promise.resolve(); }
        if (!el('chkCash').checked && !el('chkBank').checked) { alert('Selection Of Cash Or Bank Field is Required'); el('chkCash').focus(); return Promise.resolve(); }
        var q = new URLSearchParams({ date: dateText(), partyId: A.intArg(el('party').value),
            slipFlag: el('radioPayment').checked ? 'Payment' : (el('radioReceipt').checked ? 'Receipt' : ''),
            cash: el('chkCash').checked, bank: el('chkBank').checked, accountIds: accountIds() });
        var token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/data?' + q.toString()).then(function (rows) {
            if (token !== seq) return;
            dtvoucher = rows || [];
            if (dtvoucher.length) grid.setData(columnsFor(dtvoucher[0]), dtvoucher);      // grd.DataSource = dtvoucher; RetrieveStructure(); GridSettings()
            else grid.clear();                                                           // grd.DataSource = null
        }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ Refresh (reset) / print
    function reset() {
        A.focusOf('party').focus();
        partyAccountFill().catch(function (e) { alert(e.message); });
        el('radioPayment').checked = true;
        el('chkCash').checked = true;
    }
    function print100() {                                                                // ShowReport()
        if (!dtvoucher.length) { alert('Record Not Found For Display'); return; }
        A.postGrid(RPT, dtvoucher, null, el('print'));
    }
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('new').addEventListener('click', reset);
    el('print').addEventListener('click', print100);
    el('datToDate').addEventListener('blur', function () {                               // datToDate_Leave
        if (dateText() === filledOn) return;
        fills().catch(function (e) { alert(e.message); });
    });
    d.addEventListener('keydown', function (e) {                                         // VoucherValidation_KeyDown (KeyPreview = true)
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); print100(); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // ------------------------------------------------------------------ start (VoucherValidation_Load)
    grid = A.createGrid({ table: el('grd'), scroller: el('grid'), nav: el('nav'), navText: el('navText'), headerLines: 1,
        alternate: false, totalRow: true, groupTotals: false, onLink: function () { /* CostomerWiseVoucherSlip has no grd_LinkClicked handler */ } });
    grid.clear();
    el('datToDate').value = A.today();
    el('radioPayment').checked = true;
    el('chkCash').checked = true;
    A.focusOf('party').focus();
    fills().catch(function (e) { alert(e.message); });
}(window, document));
