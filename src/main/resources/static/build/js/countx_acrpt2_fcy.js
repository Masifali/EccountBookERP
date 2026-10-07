/* ============================================================================================
 * Screen 72 "Export Payables & Receivables (Accounts)" - Architecture.WinApp.Account_Reports.FCYPayablesAndReceivablesRpt
 * Page: templates/accounts/reports/acrpt2_fcy_payables_receivables.html (route /accounts/reports/fcy-payables-receivables).
 * Every desktop handler, in desktop order (FCYPayablesAndReceivablesRpt.cs):
 *   VoucherValidation_Load      -> fromdate = ActiveYr.Start_Period (RequestedByOtherDocument is false), fromdate.Focus(), CurrencySelection()
 *                                  (VoucherReports.GetMultiCurrencyAndLastRate -> CmbCurrencySelection, "Currency Name"), both flow layouts hidden,
 *                                  then btnshow_Click(null, null) - the report runs on open.
 *   btnshow_Click / GridFill    -> uspAccounts_getReceivablesAndPayables_Export (FromDate only when the From box is checked, ToDate, currency);
 *                                  rows are split by the sign of FcyClosing: < 0 -> Payables, > 0 -> Receivables (0 is in neither);
 *                                  no rows -> both grids ClearStructure().
 *   GridSettingPayable / Rcv    -> Id + CurrencyId hidden, HeaderLines 3, GridWrappingAndColumnSettings(ColumnSetting: false), Fcy* / Lcy* in
 *                                  stringFormatbothForFcy, only FcyBaseCurrencyBalance is summed, BaseExchangeRate "#,##0.###" (85),
 *                                  AccountTitle = link (205), grouped by FcyCode (column hidden when grouped), GroupTotals Always, TotalRow.
 *   btnNew_Click / reset        -> CurrencyFlowLayout hidden, fromdate.Focus(), grdPayable.ClearStructure() (the Receivables grid is NOT cleared).
 *   print_Click_1 / btn153PayablePrint_Click -> FCYPayablesAndReceivables_Rpt (ReportTypeId 1 / 2, FromDate = fromdate.Value, ToDate) ->
 *                                  no rows "No Record Found For Display"; else 153-FCYPayablesAndReceivablesRpt.rpt over that table.
 *   grd_LinkClicked / grdRcvAble_LinkClicked -> GoToFcyGeneralLedgerFromLinkedEvent(Id, fromdate.Value, todate.Value).
 *   txtexchangerate_KeyPress    -> OnlytextdecimelFunction (the box is disabled; nothing can be typed).
 *   VoucherValidation_KeyDown   -> Enter = Tab, Ctrl+P prints 153-Receivables, Ctrl+E / Esc close, Ctrl+N New.
 *   CardsCreate / TotalOfAllCurrencyAmounts exist on the desktop but are never called, so the card panels stay hidden (not ported).
 *   ctrlGrdBar1_Load / ctrlGrdBar2_Load -> saved grid layout (countx_grid_bar.js).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt2, el = A.el;
    var API = '/api/accounts/acrpt2/fcy';
    var RPT = '153-FCYPayablesAndReceivablesRpt.rpt';
    var FCY_GL = '/accounts/reports/fcy-general-ledger';

    var look = { fcyDecimals: 0, yearStart: null };
    var currencies = [];
    var seq = 0;

    function fcyFormat() {      // clsGlobalVariables.stringFormatbothForFcy
        var z = new Array(look.fcyDecimals + 1).join('0');
        return '#,##0.' + z + ';(0,0.' + z + '); 0';
    }

    var gridRcv, gridPay;
    function columns() {
        var f = fcyFormat();
        function num(key, extra) { return Object.assign({ key: key, caption: key, width: 100, type: 'num', fmt: f }, extra || {}); }
        return [
            { key: 'Id', caption: 'Id', hidden: true, width: 60 },
            { key: 'CurrencyId', caption: 'CurrencyId', hidden: true, width: 60 },
            { key: 'AccountTitle', caption: 'AccountTitle', width: 205, link: true },
            { key: 'FcyCode', caption: 'FcyCode', width: 100 },
            num('FcyOpening'), num('FcyDebit'), num('FcyCredit'), num('FcyClosing'), num('LcyClosing'),
            num('FcyBaseCurrencyBalance', { sum: true, totalFmt: f }),
            { key: 'BaseExchangeRate', caption: 'BaseExchangeRate', width: 85, type: 'num', fmt: '#,##0.###' }
        ];
    }

    function initGrids() {
        gridRcv = A.createGrid({ table: el('grdRcvAble'), scroller: el('gridRcv'), nav: el('navRcv'), navText: el('navTextRcv'),
            headerLines: 3, groupBy: 'FcyCode', groupTotals: true, totalRow: true, onLink: ledger });
        gridPay = A.createGrid({ table: el('grdPayable'), scroller: el('gridPay'), nav: el('navPay'), navText: el('navTextPay'),
            headerLines: 3, groupBy: 'FcyCode', groupTotals: true, totalRow: true, onLink: ledger });
        gridRcv.clear(); gridPay.clear();
    }

    // ------------------------------------------------------------------ CurrencySelection
    function currencySelection() {
        return A.getJson(API + '/lookups').then(function (data) {
            look = data || look;
            currencies = look.currencies || [];
            // DDL.BindDDL(dtCurrency, Cmb, "DMultiCurrencyId", "CurrencyName", "Currency Name", ZeroIndex: true)
            if (currencies.length) A.fillSelect(el('cur'), currencies, 'DMultiCurrencyId', 'CurrencyName', { keep: false });
        });
    }

    // ------------------------------------------------------------------ GridFill
    function gridFill() {
        var from = el('fromChk').checked ? (el('fromDate').value || '') : '';
        var args = { toDate: el('toDate').value || A.today(), currencyId: A.intArg(el('cur').value) };
        var q = new URLSearchParams({ toDate: args.toDate, currencyId: args.currencyId });
        if (from) q.set('fromDate', from);
        var token = ++seq;
        return A.getJson(API + '/data?' + q.toString()).then(function (rows) {
            if (token !== seq) return;
            rows = rows || [];
            if (!rows.length) { gridPay.clear(); gridRcv.clear(); return; }     // grdPayable.ClearStructure(); grdRcvAble.ClearStructure()
            var pay = [], rcv = [];
            rows.forEach(function (r) {
                var closing = Number(A.ci(r, 'FcyClosing'));
                var o = {
                    Id: A.intArg(A.ci(r, 'AccountId')), CurrencyId: A.intArg(A.ci(r, 'CurrencyId')),
                    AccountTitle: A.ci(r, 'AccountTitle'), FcyCode: A.ci(r, 'CurrencyCode'),
                    FcyOpening: A.ci(r, 'OpeningFcy'), FcyDebit: A.ci(r, 'FcyDebit'), FcyCredit: A.ci(r, 'FcyCredit'),
                    FcyClosing: A.ci(r, 'FcyClosing'), LcyClosing: A.ci(r, 'ClosingBalance'),
                    FcyBaseCurrencyBalance: A.ci(r, 'FcyBaseCurrencyBalance'), BaseExchangeRate: A.ci(r, 'FcyBaseExchangeRate')
                };
                if (closing < 0) pay.push(o); else if (closing > 0) rcv.push(o);
            });
            gridPay.setData(columns(), pay);          // grdPayable.DataSource = dtPayable; RetrieveStructure(); GridSettingPayable()
            gridRcv.setData(columns(), rcv);
        });
    }

    function show() {                                  // btnshow_Click
        var b = el('show'); if (b.disabled) return Promise.resolve();
        A.busy(b, true);
        return gridFill().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ New
    function reset() {                                 // btnNew_Click -> reset()
        el('fromDate').focus();
        gridPay.clear();
    }

    // ------------------------------------------------------------------ prints
    function printType(typeId, btn) {
        if (btn.disabled) return;
        var from = el('fromDate').value || A.today(), to = el('toDate').value || A.today();     // fromdate.Value (even when unchecked)
        A.busy(btn, true);
        A.getJson(API + '/print-rows?' + new URLSearchParams({ fromDate: from, toDate: to, reportTypeId: typeId }).toString())
            .then(function (rows) {
                if (!rows || !rows.length) { alert('No Record Found For Display'); return; }
                return A.postGrid(RPT, rows, null, null);
            })
            .catch(function (e) { alert(e.message); })
            .then(function () { A.busy(btn, false); });
    }
    function print153Receivables() { printType(1, el('printRcv')); }
    function print153Payables() { printType(2, el('printPay')); }

    // ------------------------------------------------------------------ link
    /* CommonServices.GoToFcyGeneralLedgerFromLinkedEvent(Id, fromdate.Value, todate.Value) -> FcyGeneralLedgerRpt (screen 60). */
    function ledger(row) {
        var q = new URLSearchParams({ accountId: A.ci(row, 'Id'), fromDate: el('fromDate').value || A.today(), toDate: el('toDate').value || A.today() });
        fetch(FCY_GL, { method: 'HEAD', credentials: 'same-origin' }).then(function (r) {
            if (r.status === 404) { alert('Fcy General Ledger (screen 60) is not on the web yet.'); return; }
            w.open(FCY_GL + '?' + q.toString(), '_blank');
        }).catch(function () { alert('Fcy General Ledger (screen 60) is not on the web yet.'); });
    }

    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ tabs
    function tab(i) {
        el('pageRcv').hidden = i !== 0; el('pagePay').hidden = i !== 1;
        el('tabRcv').classList.toggle('on', i === 0); el('tabPay').classList.toggle('on', i === 1);
    }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('new').addEventListener('click', reset);
    el('printRcv').addEventListener('click', print153Receivables);
    el('printPay').addEventListener('click', print153Payables);
    el('tabRcv').addEventListener('click', function () { tab(0); });
    el('tabPay').addEventListener('click', function () { tab(1); });
    el('fromChk').addEventListener('change', function () { el('fromWrap').classList.toggle('off', !this.checked); });
    /* txtexchangerate_KeyPress = OnlytextdecimelFunction: digits and one dot (the box is disabled on the desktop). */
    el('rate').addEventListener('keypress', function (e) { if (e.key.length === 1 && !/[\d.]/.test(e.key) && !e.ctrlKey) e.preventDefault(); });

    function focusables() { return ['fromDate', 'toDate', 'cur', 'show'].map(A.focusOf); }
    /* VoucherValidation_KeyDown (KeyPreview = true) */
    d.addEventListener('keydown', function (e) {
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !A.comboOpen() && e.target.tagName !== 'BUTTON') {      // SendKeys "{TAB}"
            var list = focusables(), i = list.indexOf(d.activeElement);
            if (i >= 0 && i < list.length - 1) { e.preventDefault(); list[i + 1].focus(); return; }
        }
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); print153Receivables(); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // ------------------------------------------------------------------ start (VoucherValidation_Load)
    initGrids();
    el('toDate').value = A.today();
    el('fromDate').value = A.today();
    currencySelection().catch(function (e) { alert(e.message); }).then(function () {
        if (look.yearStart) el('fromDate').value = look.yearStart;     // fromdate.Value = ActiveYr.Start_Period
        el('fromDate').focus();
        return show();                                                  // btnshow_Click(null, null)
    });
}(window, document));
