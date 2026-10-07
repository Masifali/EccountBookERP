/* ============================================================================================
 * Screen 48 "Payables Report" - Architecture.WinApp.Account_Reports.Payables (Account_Reports/Payables.cs, 1596 lines)
 * Page: templates/accounts/reports/acrpt1_payables.html (route /accounts/reports/payables-balance-classification).
 * Group R1, every desktop handler in desktop order (line numbers are Payables.cs):
 *   VoucherValidation_Load :164  BranchFeature = GetERPFeatureById(17), BranchFeatureConsolidated = (18); no feature -> Branches label + combo hidden,
 *                                otherwise BranchesFill (:187, caption "Branch Name", consolidated = checked list, Text = the user's branch);
 *                                datFromDate = ActiveYr.Start_Period; CustomeGroupsDefine (:300), AccountFill3rdLevel (:238, checked, caption
 *                                "Account Title"), CityNameFill (:218), CustomerGroupBind (:224, checked, caption "Customer Group")
 *   btnshow_Click          :378  GroupIds / account ids from the checked texts, GetBranchesIdsByFeature, ReportsParameters -> VoucherReports.PayablesReport
 *                                = Sp_Accounts_Payables_Rpt (FinancialYearId, OrganizationId, CompanyId, UserId, FromDate, ToDate always; ShowOnlyTrade when
 *                                SkipZero or Trade Parties is ticked (ActionId = 1 - vh.SkipZero is never set), CityId, BranchesIds, ControlAccountIds,
 *                                CustomGroupId, CustomerGroupIds, BalanceFrom/To = Conversion.ToDouble, ShowAssetLiability 2 Assets / 3 Liability only when
 *                                non-zero); rows -> the 26-column table; no rows -> grd.DataSource = null (no message)
 *   GridSettings           :486  HeaderLines 2 centre, columns/widths/captions/aggregates/formats below, BranchName visible = feature && branch text,
 *                                Four Columns hides the six Opening/Closing/Increase debit-credit columns, Six hides the three net ones,
 *                                SortKeys AccountTitle ascending, AccountCode = link
 *   RdFourColumns_CheckedChanged :634  toggles those visibilities when a result exists
 *   grd_LinkClicked        :612  AccountCode -> GoToGeneralLedgerFromLinkedEvent(AccountId, datFromDate, datToDate, 0, BranchesId)
 *   print_Click_1          :330  no rows -> "No Record Found For Display"; else 121-Accounts_Payables_Rpt.rpt over the last Show (@CompanyName)
 *   btnNew_Click / reset   :313  datFromDate.Focus(); cmbAccount.Text = ""; cmbAccount.DataSource = null (the control account list is emptied - as the desktop)
 *   VoucherValidation_KeyDown :141  Ctrl+P print, Ctrl+E / Esc close, Ctrl+N reset (no Enter-to-Tab on this form)
 *   ctrlGrdBar1_Load (layout restore) and Payables_FormClosing (HistoryStack) have no web equivalent.
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, el = A.el;
    var API = '/api/accounts/acrpt1/payables';
    var lookup = { amountDecimals: 0, branchFeature: false, branchConsolidated: false, yearStart: null };
    var dtrpt = [];            // the last Show's procedure rows (what Print hands to the template)
    var lastArgs = null;
    var seq = 0;

    function wrapOf(id) { var s = el(id); return s ? (s.closest('.dtcombo-wrap') || s) : null; }
    function showCombo(id, on) { var x = wrapOf(id); if (x) x.style.display = on ? '' : 'none'; }
    function branchSel() { return lookup.branchConsolidated ? el('cmbBranchesChecked') : el('cmbBranches'); }
    function branchText() {
        if (!lookup.branchFeature) return '';
        var s = branchSel();
        return lookup.branchConsolidated ? A.checkedTexts(s).join(',') : (s.value ? s.options[s.selectedIndex].textContent.trim() : '');
    }

    // ------------------------------------------------------------------ load (VoucherValidation_Load)
    function load() {
        showCombo('cmbBranches', false); showCombo('cmbBranchesChecked', false);
        return A.getJson(API + '/lookups').then(function (data) {
            lookup = data || lookup;
            A.fill(el('cmbCityName'), lookup.cities, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'CityName'); });
            A.fill(el('cmbCustomGroup'), lookup.customGroups, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'AcLookUpsDescription'); });
            A.fill(el('cmbAccount'), lookup.accounts, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'AccountTitle'); });
            A.fill(el('cmbCustomerGroup'), lookup.inventoryGroups, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'Description'); });
            var feature = !!lookup.branchFeature;
            el('lblBranches').hidden = !feature;
            if (feature && (lookup.branches || []).length) {
                var s = branchSel(), uid = String(lookup.userBranchId == null ? '' : lookup.userBranchId);
                A.fill(s, lookup.branches, function (r) { return A.ci(r, 'Id'); }, function (r) { return A.ci(r, 'BranchName'); });
                for (var i = 0; i < s.options.length; i++) if (s.options[i].value === uid && uid !== '') {   // CmbBranches.Text = UserAccount.BranchName
                    if (lookup.branchConsolidated) s.options[i].selected = true; else s.selectedIndex = i;
                }
                s.dispatchEvent(new Event('change', { bubbles: true }));
                showCombo(lookup.branchConsolidated ? 'cmbBranchesChecked' : 'cmbBranches', true);
            }
            el('datFromDate').value = lookup.yearStart || A.today();      // datFromDate.Value = ActiveYr.Start_Period
        }).catch(function (e) { alert(e.message); });
    }

    // ------------------------------------------------------------------ the grid (dt2 columns, GridSettings)
    function colDefs() {
        var four = el('rdFour').checked;
        function amt(key, caption, width, extra) { return Object.assign({ key: key, caption: caption, width: width, type: 'amount', sum: true }, extra || {}); }
        return [
            { key: 'BranchesId', caption: 'BranchesId', hidden: true },
            { key: 'BranchName', caption: 'BranchName', width: 120, hidden: !(lookup.branchFeature && branchText() !== '') },
            { key: 'ParentAccountTitle', caption: 'ParentAccountTitle', width: 145 },
            { key: 'AccountClass', caption: 'AccountClass', width: 80 },
            { key: 'AccountId', caption: 'AccountId', hidden: true },
            { key: 'AccountCode', caption: 'AccountCode', width: 80, link: true },
            { key: 'AccountTitle', caption: 'AccountTitle', width: 180 },
            { key: 'AccountType', caption: 'AccountType', width: 80 },
            amt('Opening', 'Opening Balance', 105, { hidden: !four }),
            amt('OpeningDebit', 'OpeningDebit', 80, { hidden: four }),
            amt('OpeningCredit', 'OpeningCredit', 100, { hidden: four }),
            amt('CurrentDebit', 'Debit', 100),
            amt('CurrentCredit', 'Credit', 100),
            amt('Closing', 'Closing Balance', 115, { hidden: !four }),
            amt('ClosingDebit', 'ClosingDebit', 80, { hidden: four }),
            amt('ClosingCredit', 'ClosingCredit', 110, { hidden: four }),
            amt('Increase/Decrease', 'Increase/Decrease', 100, { hidden: !four }),
            amt('Increase/Decrease_Debit', 'Increase/Decrease_Debit', 100, { hidden: four }),
            amt('Increase/Decrease_Credit', 'Increase/Decrease_Credit', 100, { hidden: four }),
            { key: 'LastBillDate', caption: 'LastBillDate', width: 85 },
            amt('LastBillAmount', 'LastBillAmount', 100),
            { key: 'BillDays', caption: 'BillDays', width: 65, align: 'c' },
            { key: 'LastPaidDate', caption: 'LastPaidDate', width: 85 },
            amt('LastPaidAmount', 'LastPaidAmount', 100),
            { key: 'PaidDays', caption: 'PaidDays', width: 65, align: 'c' },
            { key: 'CityName', caption: 'CityName', width: 120 }
        ];
    }
    /* dt2.Rows.Add(row["BranchesId"], ..., Conversion.ToString(row["ClassName"]), ..., row["ObDebit"], ..., row["LastBillsAmount"], row["BillDays"], row["LastPaidDate"], row["LastPidAmount"], ...) */
    function mapRow(r) {
        var c = A.ci;
        return {
            'BranchesId': c(r, 'BranchesId'), 'BranchName': c(r, 'BranchName'), 'ParentAccountTitle': c(r, 'ParentAccountTitle'),
            'AccountClass': c(r, 'ClassName') == null ? '' : String(c(r, 'ClassName')), 'AccountId': c(r, 'AccountId'), 'AccountCode': c(r, 'AccountCode'),
            'AccountTitle': c(r, 'AccountTitle'), 'AccountType': c(r, 'AccountType'), 'Opening': c(r, 'Opening'),
            'OpeningDebit': c(r, 'ObDebit'), 'OpeningCredit': c(r, 'ObCredit'), 'CurrentDebit': c(r, 'CurrDebit'), 'CurrentCredit': c(r, 'CurrCredit'),
            'Closing': c(r, 'Closing'), 'ClosingDebit': c(r, 'ClDebit'), 'ClosingCredit': c(r, 'ClCredit'),
            'Increase/Decrease': c(r, 'Increase/Decrease'), 'Increase/Decrease_Debit': c(r, 'Increase/Decrease_Debit'), 'Increase/Decrease_Credit': c(r, 'Increase/Decrease_Credit'),
            'LastBillDate': c(r, 'LastBillDate') == null ? '' : String(c(r, 'LastBillDate')), 'LastBillAmount': c(r, 'LastBillsAmount'), 'BillDays': c(r, 'BillDays'),
            'LastPaidDate': c(r, 'LastPaidDate') == null ? '' : String(c(r, 'LastPaidDate')), 'LastPaidAmount': c(r, 'LastPidAmount'), 'PaidDays': c(r, 'PaidDays'),
            'CityName': c(r, 'CityName')
        };
    }
    var grid = new A.Grid({
        tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', groupTotals: true,
        decimals: function () { return lookup.amountDecimals; }, sort: { key: 'AccountTitle', dir: 1 },
        onLink: function (row) { ledger(row); }
    });

    /* RdFourColumns_CheckedChanged: only when a result exists */
    function columnsToggle() {
        if (!dtrpt.length || !grid.columns) return;
        var four = el('rdFour').checked, vis = {
            'Opening': four, 'Closing': four, 'Increase/Decrease': four,
            'OpeningDebit': !four, 'OpeningCredit': !four, 'ClosingDebit': !four, 'ClosingCredit': !four,
            'Increase/Decrease_Debit': !four, 'Increase/Decrease_Credit': !four
        };
        grid.columns.forEach(function (c) { if (c.key in vis) c.hidden = !vis[c.key]; });
        grid.render();
    }

    // ------------------------------------------------------------------ Show (btnshow_Click)
    function parseDouble(s) { return A.toDouble(String(s || '').trim()); }       // Conversion.ToDouble((object)text)
    function branchIds() {                                                       // InfragisticsHelper.GetBranchesIdsByFeature
        if (!lookup.branchFeature) return '';
        var s = branchSel();
        if (lookup.branchConsolidated) {
            var ids = ''; A.checkedValues(s).forEach(function (v) { ids += ',' + v; }); return ids;
        }
        if (!s.value || A.toInt(s.value) === 0) { var f = A.focusable('cmbBranches'); if (f) f.focus(); throw new Error('Please Select Branch first!'); }
        return s.value;
    }
    function show() {
        var b = el('show'); if (b.disabled) return;
        var args;
        try {
            var groupIds = A.checkedValues(el('cmbCustomerGroup')).join(',');
            var account = A.checkedValues(el('cmbAccount')).join(',');
            var bids = branchIds();
            var asset = el('rdAssets').checked ? 2 : (el('rdLiability').checked ? 3 : 0);
            args = {
                fromDate: el('datFromDate').value, toDate: el('datToDate').value,
                actionId: (el('skipZero').checked || el('chkTradeParties').checked) ? 1 : 0,
                customGroupId: A.toInt(el('cmbCustomGroup').value), controlAccountIds: account, customerGroupIds: groupIds,
                branchesIds: bids, cityId: A.toInt(el('cmbCityName').value),
                balanceFrom: parseDouble(el('txtBalanceFrom').value), balanceTo: parseDouble(el('txtBalanceTo').value),
                showAssetLiability: asset
            };
        } catch (e) { alert(e.message); return; }
        if (!args.fromDate || !args.toDate) { alert('Please select the From && To Date'); return; }
        A.busy(b, true);
        var token = ++seq;
        var q = new URLSearchParams(args);
        A.getJson(API + '?' + q.toString()).then(function (rows) {
            if (token !== seq) return;
            dtrpt = rows || [];
            if (!dtrpt.length) { lastArgs = null; grid.clear(); return; }                 // grd.DataSource = null
            lastArgs = args;
            grid.setData(colDefs(), dtrpt.map(mapRow));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }

    // ------------------------------------------------------------------ link / print / reset
    function ledger(r) {
        var q = new URLSearchParams();
        q.set('accountId', A.ci(r, 'AccountId'));
        if (el('datFromDate').value) q.set('fromDate', el('datFromDate').value);
        if (el('datToDate').value) q.set('toDate', el('datToDate').value);
        var br = A.toInt(A.ci(r, 'BranchesId')); if (br > 0) q.set('branchId', br);
        w.open('/accounts/reports/general-ledger?' + q.toString(), '_blank');
    }
    function print() {
        var b = el('print'); if (b.disabled) return;
        if (!dtrpt.length || !lastArgs) { alert('No Record Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/accounts/reports/acrpt1/print/payables?' + new URLSearchParams(lastArgs).toString())
            .catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() {                                          // btnNew_Click -> reset()
        el('datFromDate').focus();
        A.fill(el('cmbAccount'), [], function () { return ''; }, function () { return ''; });      // cmbAccount.Text = ""; DataSource = null
    }
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('print').addEventListener('click', print);
    el('reset').addEventListener('click', reset);
    el('rdFour').addEventListener('change', columnsToggle);
    el('rdSix').addEventListener('change', columnsToggle);
    /* txtBalanceFrom / txtBalanceTo have no KeyPress handler on this form (free text, Conversion.ToDouble) */
    d.addEventListener('keydown', function (e) {                // VoucherValidation_KeyDown (KeyPreview)
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); print(); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // ------------------------------------------------------------------ start
    el('datFromDate').value = A.today();
    el('datToDate').value = A.today();                          // DateTimePicker default = Now
    grid.render();
    load();
}(window, document));
