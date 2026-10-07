/* ============================================================================================
 * Screen 86 "Profit & Loss 02" - Architecture.WinApp.Account_Reports.ProfitLoss (screen name frmProfitAndLoss)
 * Page: templates/accounts/reports/acrpt3_profit_loss.html (route /accounts/reports/profit-and-loss-02).
 * Every desktop handler, in desktop order (ProfitLoss.cs):
 *   VoucherValidation_Load -> AutoScroll (AutoScrollMinSize 980x700), WaitForm, ERP features 17 / 18; without 17 lblBranch / cmbbranch are hidden and btnshow moves to
 *                             (243,24), else BranchFill (VoucherHead.GetBranchesFromVouchersByAccountId -> "Branch Name"; checked list with 17 + 18; Text = UserAccount.BranchName);
 *                             fromdate.Focus(), fromdate = ActiveYr.Start_Period, GridBind().
 *   btnshow_Click / GridBind -> BranchIds = InfragisticsHelper.GetBranchesIdsByFeature (17 + 18: ids of the ticked names; 17 only: the selected branch, else
 *                             "Please Select Branch first!"), VoucherReports.ProftLoss (SpAccounts_ProfitLoassFormatA_Report: UserId, FromDate, ToDate, BranchesIds);
 *                             no rows -> nothing changes; otherwise rows are summed by AccountNoteId (2 Sales Net, 4 Cost Of Sale, 5 Other Expenses, 6 Tax, 7 Selling,
 *                             8 Administration, 9 Financial, 32 Operating, 33 Discount Allowed, 1 Mark Up On Loans, 3 Other Income), each caption takes AccountsNotes and
 *                             every amount is shown in stringFormatboth; Gross Profit, T. Non Operating Income, Total Operating Expense, Total Financial Expense,
 *                             Total Profit, Total Operating Profit, Profit Before / After Taxation and the Total Income / Total Expense / Net Profit footer follow the desktop's sums.
 *   btnNew_Click / reset   -> fromdate.Focus() only.
 *   print_Click_1          -> Accounts_ProfitLoss_ForCrystal (SpAccounts_ProfitLoss_ForCrystal, ClassIds "4,5") -> 166-ProfitLossNewReport.rpt
 *                             ("Not Record Found For Display" when empty).
 *   VoucherValidation_KeyDown -> Enter = Tab, Ctrl+P print, Ctrl+E / Esc close, Ctrl+N Refresh.
 *   label12_Click / ctrlGrdBar1_Load / btnclose_Click -> empty on the desktop.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt3, el = A.el;
    var API = '/api/accounts/acrpt3/pl';
    var RPT = '166-ProfitLossNewReport.rpt';

    var look = { branchFeature: false, branchFeatureConsolidated: false, branches: [], userBranchName: '', amountDecimals: 0, yearStart: null };
    var multi = false, seq = 0;

    function bothFmt() { var z = new Array(look.amountDecimals + 1).join('0'); return '#,##0.' + z + ';(0,0.' + z + '); 0'; }     // stringFormatboth
    function fmt(n) { return A.netFmt(n, bothFmt()); }
    function num(v) { var n = parseFloat(v); return isNaN(n) ? 0 : n; }
    function setText(id, t) { var x = el(id); if (x) x.textContent = t; }

    // ------------------------------------------------------------------ branch (BranchFill / GetBranchesIdsByFeature inputs)
    function branchFill() {
        multi = !!look.branchFeatureConsolidated;
        var rows = look.branches || [];
        el('wrapBranchMulti').hidden = !multi; el('wrapBranchOne').hidden = multi;
        var sel = multi ? el('branchMulti') : el('branchOne');
        if (rows.length) {                                                       // if (dt.Rows.Count > 0) BindDDL(dt, cmbbranch, "Id", "BranchName", "Branch Name", false)
            A.fillSelect(sel, rows, 'Id', 'BranchName', { keep: false, empty: multi ? undefined : true });
            for (var i = 0; i < sel.options.length; i++) {                       // cmbbranch.Text = UserAccount.BranchName
                if (sel.options[i].value !== '' && sel.options[i].text === String(look.userBranchName || '')) {
                    if (multi) sel.options[i].selected = true; else sel.value = sel.options[i].value;
                }
            }
        }
        if (w.DesktopCombo && w.DesktopCombo.refresh) w.DesktopCombo.refresh();
        sel.dispatchEvent(new Event('change', { bubbles: true }));
    }
    function branchQuery() {
        if (!look.branchFeature) return { branchText: '' };
        if (multi) return { branchText: A.checkedTexts(el('branchMulti')).join(',') };
        return { branchText: '', branchId: A.intArg(el('branchOne').value) };
    }
    function query() {
        var q = new URLSearchParams({ fromDate: el('fromDate').value || A.today(), toDate: el('toDate').value || A.today() });
        var b = branchQuery(); q.set('branchText', b.branchText);
        if (b.branchId !== undefined) q.set('branchId', b.branchId);
        return q.toString();
    }

    // ------------------------------------------------------------------ GridBind
    var NOTES = { 2: ['lblSalesnet', 'sales'], 4: ['lblcostofsale', 'cost'], 5: ['lblotherexpense', 'other'], 6: ['lblTaxexpense', 'tax'], 7: ['lblSellingExp', 'selling'],
        8: ['lblAdmiExp', 'admin'], 9: ['lblFinanExp', 'fin'], 32: ['lblOperExp', 'oper'], 33: ['lblDiscountAllowedAmount', 'disc'], 1: ['lblMarkupLoan', 'markup'], 3: ['lblotherincome', 'otherInc'] };

    function gridBind() {
        var token = ++seq;
        return A.getJson(API + '/data?' + query()).then(function (dt) {
            if (token !== seq) return;
            if (!dt || !dt.length) return;                                       // if (dt.Rows.Count <= 0) return;
            var s = { sales: 0, cost: 0, other: 0, tax: 0, selling: 0, admin: 0, fin: 0, oper: 0, disc: 0, markup: 0, otherInc: 0 };
            dt.forEach(function (r) {
                var id = A.intArg(A.ci(r, 'AccountNoteId')), n = NOTES[id];
                if (!n) return;
                s[n[1]] += num(A.ci(r, 'Amount'));
                var cap = A.ci(r, 'AccountsNotes'); cap = cap == null ? '' : String(cap);
                setText(n[0], cap);               // lbl<...>.Text = AccountsNotes (id 33 sets lblDiscountAllowedAmount, which is overwritten with the amount below)
            });
            setText('lblsalesnetamt', fmt(s.sales)); setText('lblcostofsaleamt', fmt(s.cost)); setText('lblotherexpenseamt', fmt(s.other));
            setText('lbltaxexpenseamt', fmt(s.tax)); setText('lblsellingexpamt', fmt(s.selling)); setText('lbladministratorexpamt', fmt(s.admin));
            setText('lblfinancialexpamt', fmt(s.fin)); setText('lbloperatingexpenseamt', fmt(s.oper)); setText('lblDiscountAllowedAmount', fmt(s.disc));
            setText('lblmarkuploadAmount', fmt(s.markup)); setText('lblotherincomeamt', fmt(s.otherInc));
            var opExp = s.other + s.selling + s.admin + s.oper, nonOp = s.disc + s.otherInc, finExp = s.fin + s.markup;
            setText('lblGrossprofit', fmt(s.sales + s.cost));
            setText('lblToNonOperatingIncome', fmt(nonOp));
            setText('lblToOperatingExpense', fmt(opExp));
            setText('lblTotalFinancialExpense', fmt(finExp));
            setText('lblTotalProfit', fmt(s.sales + s.cost + nonOp));
            setText('lblToOperatingProfit', fmt(s.sales + s.cost + nonOp + opExp));
            setText('lblProfitBeforeTax', fmt(s.sales + s.cost + nonOp + opExp + finExp));
            setText('lblProfitAfterTax', fmt(s.sales + s.cost + nonOp + opExp + finExp + s.tax));
            var totalIncome = s.sales + s.disc + s.otherInc;
            var totalExpenses = s.cost + s.other + s.selling + s.admin + s.oper + s.fin + s.markup + s.tax;
            setText('lblTotalIncomeFoterAmt', fmt(totalIncome));
            setText('lblTotalExpFoterAmt', fmt(totalExpenses));
            setText('lblNetProfitFoterAmt', fmt(totalIncome + totalExpenses));
        });
    }
    function show() {
        var b = el('btnshow'); if (b.disabled) return Promise.resolve();
        A.busy(b, true); d.body.style.cursor = 'progress';                        // WaitForm
        return gridBind().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); d.body.style.cursor = ''; });
    }

    // ------------------------------------------------------------------ Refresh (reset) / print
    function reset() { el('fromDate').focus(); }
    function print166() {
        var b = el('print'); if (b.disabled) return;
        A.busy(b, true);
        A.getJson(API + '/print-rows?' + query()).then(function (rows) {
            if (!rows || !rows.length) { alert('Not Record Found For Display'); return; }
            return A.postGrid(RPT, rows, null, null);
        }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ events
    el('btnshow').addEventListener('click', show);
    el('new').addEventListener('click', reset);
    el('print').addEventListener('click', print166);

    function focusables() { var l = [el('fromDate'), el('toDate'), el('btnshow')]; if (look.branchFeature) l.push(A.focusOf(multi ? 'branchMulti' : 'branchOne')); return l; }   // tab order 0, 1, 6, 152
    d.addEventListener('keydown', function (e) {                                  // VoucherValidation_KeyDown (KeyPreview = true)
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !A.comboOpen() && e.target.tagName !== 'BUTTON') {      // SendKeys "{TAB}"
            var list = focusables(), i = list.indexOf(d.activeElement);
            if (i >= 0 && i < list.length - 1) { e.preventDefault(); list[i + 1].focus(); return; }
        }
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); print166(); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // ------------------------------------------------------------------ start (VoucherValidation_Load)
    el('toDate').value = A.today();
    el('fromDate').value = A.today();
    A.busy(el('btnshow'), true); d.body.style.cursor = 'progress';
    A.getJson(API + '/lookups').then(function (data) {
        look = Object.assign(look, data || {});
        if (!look.branchFeature) {                                                 // lblBranch / cmbbranch hidden, btnshow at (243,24)
            el('lblBranch').hidden = true; el('btnshow').classList.add('nobranch');
            el('wrapBranchMulti').hidden = true; el('wrapBranchOne').hidden = true;
            el('btnshow').style.left = '243px';
        } else {
            el('lblBranch').hidden = false;
            branchFill();
        }
        el('fromDate').focus();
        if (look.yearStart) el('fromDate').value = look.yearStart;               // fromdate.Value = ActiveYr.Start_Period
        A.busy(el('btnshow'), false);
        return gridBind();                                                        // GridBind()
    }).catch(function (e) { alert(e.message); }).then(function () { A.busy(el('btnshow'), false); d.body.style.cursor = ''; });
}(window, document));
