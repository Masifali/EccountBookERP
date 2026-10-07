/* ============================================================================================
 * Screen 60 "Fcy General Ledger" - Architecture.WinApp.Account_Reports.FcyGeneralLedgerRpt
 * Page: templates/accounts/reports/acrpt3_fcy_general_ledger.html (route /accounts/reports/fcy-general-ledger).
 * Every desktop handler, in desktop order (FcyGeneralLedgerRpt.cs):
 *   VoucherValidation_Load   -> AllowEditOnVoucherNoinReports, AccountTitleFill() (CoaAllocationAccountTitleByAccountTypeIds("22") -> "Account Title",
 *                               bound only when it has rows), fromdate.Focus(); with AccountId > 0 (?accountId=&fromDate=&toDate=) the combo, both dates and
 *                               btnshow_Click(null, null), else fromdate = ActiveYr.Start_Period.
 *   btnshow_Click            -> dtData cleared first; no Account Title -> "Account Title Required" (focus the combo).
 *                               From Exports : print 156 visible, 105C / 105C-II hidden, grd.Name "FcyLedgerFromExports", FCY_LedgerAgainstAccountId
 *                                              (SPU_Accounts_FCYCustomerLedger_Rpt), rows -> 17 typed columns -> GridSetting(), no rows -> ClearStructure().
 *                               From Accounts: print 156 hidden, 105C / 105C-II visible, grd.Name "FcyLedgerFromAccounts", Accounts_FcyGeneralLedger_Rpt
 *                                              (Usp_Accounts_FcyGeneralLedger_Rpt, ApprovedFilter "All"), no rows -> return (the grid is left as it was),
 *                                              rows -> 20 columns (BalType / LcyBalType = Dr / Cr / Nill from the sign) -> grdFcyLedgerFromAccountsSetting().
 *   GridSetting / grdFcyLedgerFromAccountsSetting -> AutomaticSort false, HeaderLines 2, centred headers, hidden columns, widths, Fcy* in
 *                               stringFormatbothForFcy, Pkr* / Lcy* in stringFormatboth, ExchangeRate DecimalFCYRateFormate (Accounts) / "#,##0.###" (Exports),
 *                               sums on the columns the desktop aggregates; Accounts: DocType = link, VoucherNo = link only for Admin or AllowEditOnVoucherNoinReports.
 *   print_Click_1            -> dtData -> 156-FcyGeneralLedger.rpt ("Not Record Found For Display" / "Report Not Found").
 *   btnPrint105C / II        -> dtData -> 105C-AcRptFcyGeneralLedger.rpt / 105D-AcRptFcyGeneralLedger.rpt.
 *   grd_LinkClicked          -> VoucherNo / VoucherCode: EditMethodFromLinked(DocumentTypeId, VoucherHeadId, DocumentTypeSrNo);
 *                               DocType / VoucherType: ANewAcRptPaymentReceiptsVoucherSlip_102(VoucherHeadId).
 *   btnNew_Click / reset     -> combo text cleared, fromdate.Focus(), grd.ClearStructure() (dtData is NOT cleared, as on the desktop).
 *   VoucherValidation_KeyDown-> Enter = Tab, Ctrl+P = 156 print, Ctrl+E / Esc close, Ctrl+N Reset.
 *   ctrlGrdBar1_Load         -> saved grid layout (countx_grid_bar.js), one per grid name.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt3, el = A.el;
    var API = '/api/accounts/acrpt3/fcygl';

    var look = { fcyDecimals: 0, fcyRateDecimals: 0, amountDecimals: 0, allowVoucherLink: false, accounts: [], yearStart: null };
    var dtData = [];                 // the procedure's DataTable of the last Show
    var gridA, gridB, shown = 'A';   // A = FcyLedgerFromAccounts, B = FcyLedgerFromExports
    var seq = 0;

    function zeros(n) { return new Array(n + 1).join('0'); }
    function fcyFmt() { var z = zeros(look.fcyDecimals); return '#,##0.' + z + ';(0,0.' + z + '); 0'; }           // stringFormatbothForFcy
    function lcyFmt() { var z = zeros(look.amountDecimals); return '#,##0.' + z + ';(0,0.' + z + '); 0'; }         // stringFormatboth
    function rateFmt() { return '#,#0.' + zeros(look.fcyRateDecimals); }                                           // DecimalFCYRateFormate
    var RAW = '0.##########';        // a double column with no FormatString

    function num(v) { var n = parseFloat(v); return isNaN(n) ? 0 : n; }      // Conversion.ToDouble
    function toI(v) { return A.intArg(v); }                                   // Conversion.ToInt
    function str(v) { return v == null ? '' : String(v); }                    // Conversion.ToString

    // ------------------------------------------------------------------ grids
    function colsExports() {
        var f = fcyFmt(), p = lcyFmt();
        function c(key, width, extra) { return Object.assign({ key: key, caption: key, width: width, type: 'num', fmt: RAW }, extra || {}); }
        return [
            { key: 'AccountTitle', caption: 'AccountTitle', width: 200, hidden: true },
            { key: 'VoucherDate', caption: 'VoucherDate', width: 75, type: 'date' },
            { key: 'VoucherType', caption: 'VoucherType', width: 70 },
            { key: 'VoucherCode', caption: 'VoucherCode', width: 65, type: 'int' },
            { key: 'ContractNo', caption: 'ContractNo', width: 90 },
            { key: 'InvoiceNo', caption: 'InvoiceNo', width: 90 },
            c('M_Tons', 70, { fmt: '#,##0.###', sum: true, totalFmt: '#,##0.###' }),
            { key: 'FcyCode', caption: 'FcyCode', width: 70 },
            c('ExchangeRate', 70, { fmt: '#,##0.###' }),
            c('FcyOpening', 110, { fmt: f, sum: true, totalFmt: f }),
            c('FcyDebit', 110, { fmt: f, sum: true, totalFmt: f }),
            c('FcyCredit', 110, { fmt: f, sum: true, totalFmt: f }),
            c('FcyBalance', 110, { fmt: f }),
            c('PkrOpening', 115, { fmt: p, sum: true, totalFmt: p }),
            c('PkrDebit', 110, { fmt: p, sum: true, totalFmt: p }),
            c('PkrCredit', 110, { fmt: p, sum: true, totalFmt: p }),
            c('PkrBalance', 130, { fmt: p })
        ];
    }
    function colsAccounts() {
        var f = fcyFmt(), p = lcyFmt();
        function c(key, width, extra) { return Object.assign({ key: key, caption: key, width: width, type: 'num', fmt: RAW }, extra || {}); }
        return [
            { key: 'VoucherDate', caption: 'VoucherDate', width: 75, type: 'date' },
            { key: 'DocType', caption: 'DocType', width: 70, link: true },
            { key: 'VoucherNo', caption: 'VoucherNo', width: 65, link: !!look.allowVoucherLink },
            { key: 'AccountCode', caption: 'AccountCode', width: 75 },
            { key: 'OffSetTitle', caption: 'OffSetTitle', width: 180 },
            { key: 'FcyCode', caption: 'FcyCode', width: 55 },
            c('ExchangeRate', 70, { fmt: rateFmt() }),
            c('FcyDebit', 110, { fmt: f, sum: true, totalFmt: f }),
            c('FcyCredit', 110, { fmt: f, sum: true, totalFmt: f }),
            c('FcyBalance', 110, { fmt: f }),
            { key: 'BalType', caption: 'BalType', width: 50 },
            c('LcyDebit', 110, { fmt: p, sum: true, totalFmt: p }),
            c('LcyCredit', 110, { fmt: p, sum: true, totalFmt: p }),
            c('LcyBalance', 110, { fmt: p }),
            { key: 'LcyBalType', caption: 'LcyBalType', width: 60 },
            { key: 'Comments', caption: 'Comments', width: 130 },
            { key: 'MannualNo', caption: 'Manual No', width: 70 },
            c('NoOfAttachments', 85),
            { key: 'VoucherHeadId', caption: 'VoucherHeadId', hidden: true, width: 60, type: 'int' },
            { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true, width: 60, type: 'int' },
            { key: 'DocumentTypeSrNo', caption: 'DocumentTypeSrNo', hidden: true, width: 60, type: 'int' }
        ];
    }

    function initGrids() {
        gridA = A.createGrid({ table: el('grdA'), scroller: el('gridA'), nav: el('navA'), navText: el('navTextA'), headerLines: 2,
            sortable: false, alternate: true, totalRow: true, onLink: onLink });
        gridB = A.createGrid({ table: el('grdB'), scroller: el('gridB'), nav: el('navB'), navText: el('navTextB'), headerLines: 2,
            sortable: false, alternate: true, totalRow: true, onLink: onLink });
        gridA.clear(); gridB.clear();
    }
    function view(which) {            // grd.Name = FcyLedgerFromAccounts / FcyLedgerFromExports (the one visible grid)
        shown = which;
        el('boxA').hidden = which !== 'A'; el('boxB').hidden = which !== 'B';
        el('barMountA').hidden = which !== 'A'; el('barMountB').hidden = which !== 'B';
    }
    function current() { return shown === 'A' ? gridA : gridB; }

    // ------------------------------------------------------------------ AccountTitleFill
    function accountTitleFill() {
        return A.getJson(API + '/lookups').then(function (data) {
            look = Object.assign(look, data || {});
            if ((look.accounts || []).length) {                    // if (dt.Rows.Count > 0) DDL.BindDDL(dt, cmb, "Id", "AccountTitle", "Account Title", ZeroIndex: false)
                A.fillSelect(el('cmb'), look.accounts, 'Id', 'AccountTitle', { keep: false });
                if (w.DesktopCombo && w.DesktopCombo.refresh) w.DesktopCombo.refresh();
                el('cmb').dispatchEvent(new Event('change', { bubbles: true }));
            }
        });
    }

    // ------------------------------------------------------------------ btnshow_Click
    function setButtons(exportsMode) {
        el('print156').hidden = !exportsMode;
        el('print105C').hidden = exportsMode; el('print105CII').hidden = exportsMode;
    }
    function toExportsRows(rows) {
        return rows.map(function (r) {
            return {
                AccountTitle: str(A.ci(r, 'AccountTitle')), VoucherDate: A.ci(r, 'VoucherDate'), VoucherType: str(A.ci(r, 'DocType')),
                VoucherCode: toI(A.ci(r, 'VoucherCode')), ContractNo: str(A.ci(r, 'ContractNo')), InvoiceNo: str(A.ci(r, 'InvoiceNo')),
                M_Tons: num(A.ci(r, 'M_Tons')), FcyCode: str(A.ci(r, 'CurrencyCode')), ExchangeRate: num(A.ci(r, 'ExchangeRate')),
                FcyOpening: num(A.ci(r, 'FcyOpening')), FcyDebit: num(A.ci(r, 'FcyDebit')), FcyCredit: num(A.ci(r, 'FcyCredit')), FcyBalance: num(A.ci(r, 'FcyBalance')),
                PkrOpening: num(A.ci(r, 'PkrOpening')), PkrDebit: num(A.ci(r, 'PkrDebit')), PkrCredit: num(A.ci(r, 'PkrCredit')), PkrBalance: num(A.ci(r, 'PkrBalance'))
            };
        });
    }
    function drCr(v) { var n = num(v); return n > 0 ? 'Dr' : (n < 0 ? 'Cr' : 'Nill'); }
    function toAccountsRows(rows) {
        return rows.map(function (r) {
            return {
                VoucherDate: A.ci(r, 'VoucherDate'), DocType: A.ci(r, 'DocTypeCode'), VoucherNo: str(A.ci(r, 'VoucherCode')), AccountCode: A.ci(r, 'AccountCode'),
                OffSetTitle: A.ci(r, 'OffsetAccountTitle'), FcyCode: A.ci(r, 'FcyCode'), ExchangeRate: num(A.ci(r, 'ExRateCal')),
                FcyDebit: num(A.ci(r, 'FcyDebit')), FcyCredit: num(A.ci(r, 'FcyCredit')), FcyBalance: num(A.ci(r, 'FcyBalance')), BalType: drCr(A.ci(r, 'FcyBalance')),
                LcyDebit: num(A.ci(r, 'DebitAmount')), LcyCredit: num(A.ci(r, 'CreditAmount')), LcyBalance: num(A.ci(r, 'Balance')), LcyBalType: drCr(A.ci(r, 'Balance')),
                Comments: A.ci(r, 'Comments'), MannualNo: A.ci(r, 'MannualNo'), NoOfAttachments: num(A.ci(r, 'NoOfAttachments')),
                VoucherHeadId: toI(A.ci(r, 'VoucherHeadId')), DocumentTypeId: toI(A.ci(r, 'DocumentTypeId')), DocumentTypeSrNo: toI(A.ci(r, 'DocumentTypeSrNo'))
            };
        });
    }

    function show() {
        var b = el('show'); if (b.disabled) return Promise.resolve();
        dtData = [];                                             // dtData.Rows.Clear()
        var accountId = toI(el('cmb').value);
        if (accountId === 0) { alert('Account Title Required'); A.focusOf('cmb').focus(); return Promise.resolve(); }
        var exportsMode = el('rdExports').checked, accountsMode = el('rdAccounts').checked;
        if (!exportsMode && !accountsMode) { gridA.clear(); gridB.clear(); return Promise.resolve(); }     // else grd.ClearStructure()
        setButtons(exportsMode);
        var mode = exportsMode ? 'exports' : 'accounts';
        var q = new URLSearchParams({ mode: mode, accountId: accountId, fromDate: el('fromDate').value || A.today(), toDate: el('toDate').value || A.today() });
        var token = ++seq;
        A.busy(b, true);
        return A.getJson(API + '/data?' + q.toString()).then(function (rows) {
            if (token !== seq) return;
            dtData = rows || [];
            if (exportsMode) {
                view('B');
                if (dtData.length) gridB.setData(colsExports(), toExportsRows(dtData)); else gridB.clear();
            } else {
                if (!dtData.length) return;                      // if (dtData.Rows.Count <= 0) return;
                view('A');
                gridA.setData(colsAccounts(), toAccountsRows(dtData));
            }
        }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ New / reset
    function reset() {
        A.clearSelect(el('cmb'));                                // cmbAccountTitle.Text = ""
        el('fromDate').focus();
        gridA.clear(); gridB.clear();                            // grd.ClearStructure()
    }

    // ------------------------------------------------------------------ prints
    function printRpt(rpt, btn, failText) {
        if (!dtData.length) { alert('Not Record Found For Display'); return; }
        A.postGrid(rpt, dtData, null, btn);
    }
    function print156() { printRpt('156-FcyGeneralLedger.rpt', el('print156')); }
    function print105C() { printRpt('105C-AcRptFcyGeneralLedger.rpt', el('print105C')); }
    function print105CII() { printRpt('105D-AcRptFcyGeneralLedger.rpt', el('print105CII')); }

    // ------------------------------------------------------------------ grd_LinkClicked
    function onLink(row, col) {
        var id = toI(A.ci(row, 'VoucherHeadId')), docType = toI(A.ci(row, 'DocumentTypeId')), srNo = toI(A.ci(row, 'DocumentTypeSrNo'));
        if (col.key === 'VoucherNo' || col.key === 'VoucherCode') {
            if (!w.DocLink) { alert('Document links are not available on this page.'); return; }
            w.DocLink.open(docType, id, { srNo: srNo });                              // CommonServices.EditMethodFromLinked
        } else if (col.key === 'DocType' || col.key === 'VoucherType') {
            if (!id) { alert('VoucherId Not Found'); return; }
            w.printRpt('102-ANewAcRptPaymentReceiptsVoucherSlip.rpt', { id: id });    // CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102
        }
    }

    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('new').addEventListener('click', reset);
    el('print156').addEventListener('click', print156);
    el('print105C').addEventListener('click', print105C);
    el('print105CII').addEventListener('click', print105CII);

    function focusables() { return [A.focusOf('fromDate'), A.focusOf('toDate'), A.focusOf('cmb'), el('show'), el('rdExports'), el('rdAccounts')]; }
    d.addEventListener('keydown', function (e) {                 // VoucherValidation_KeyDown (KeyPreview = true)
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !A.comboOpen() && e.target.tagName !== 'BUTTON'
            && !el('boxA').contains(e.target) && !el('boxB').contains(e.target)) {                   // SendKeys "{TAB}"
            var list = focusables(), i = list.indexOf(d.activeElement);
            if (i >= 0 && i < list.length - 1) { e.preventDefault(); list[i + 1].focus(); return; }
        }
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); print156(); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // ------------------------------------------------------------------ start (VoucherValidation_Load)
    initGrids();
    view('A');
    setButtons(true);                                            // print visible, 105C / 105C-II hidden (designer)
    el('toDate').value = A.today();
    el('fromDate').value = A.today();
    var qs = new URLSearchParams(w.location.search), presetAccount = A.intArg(qs.get('accountId'));
    accountTitleFill().catch(function (e) { alert(e.message); }).then(function () {
        el('fromDate').focus();
        if (presetAccount > 0) {                                 // AccountId > 0: cmbAccountTitle.Value, From / To, btnshow_Click
            el('cmb').value = String(presetAccount);
            if (el('cmb').value !== String(presetAccount)) el('cmb').value = '';
            el('cmb').dispatchEvent(new Event('change', { bubbles: true }));
            if (qs.get('fromDate')) el('fromDate').value = qs.get('fromDate').substring(0, 10);
            if (qs.get('toDate')) el('toDate').value = qs.get('toDate').substring(0, 10);
            return show();
        }
        if (look.yearStart) el('fromDate').value = look.yearStart;      // fromdate.Value = ActiveYr.Start_Period
    });
}(window, document));
