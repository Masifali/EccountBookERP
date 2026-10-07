/* ============================================================================================
 * Screen 76 "Pdc Inventory Report" - Architecture.WinApp.Account_Reports.PdcInventoryReport
 * Page: templates/accounts/reports/acrpt4_pdc_inventory.html (route /accounts/reports/pdc-inventory-report).
 * Every desktop handler, in desktop order (PdcInventoryReport.cs):
 *   VoucherValidation_Load   -> AccountTitleFill() (CommonServices.CoaAllocationGetForComboServiceBind: Sp_COAAllocation_GetAllMethod 'COAForCombobindig',
 *                               Id / AccountTitle, list column 240 wide), CheqStatus() (fixed 1 Pending / 2 Clear / 3 All, nothing selected),
 *                               ChkFromdate.Focus().
 *   btnshow_Click            -> GeneralReprots.PdcReceiptsRegister (Sp_PdcInventory_SlipAndRegister_Rpt): account id (when not 0), each date only when its
 *                               check box is ticked, CheqStatus = the combo's Text trimmed (not sent for "All").
 *                               rows > 0 -> grd.DataSource = dtvoucher; RetrieveStructure(); GridSettings(); no rows -> dtvoucher cleared and re-bound
 *                               (the grid keeps its columns, shows no rows).
 *   GridSettings             -> Id, MGLAccountDrId, MGLAccountCrId, CompLogoImage hidden; AccountTitle / AccountTitleDebit 250 wide; CheqDate filter = calendar
 *                               combo; CheqAmount "0,0", right, Sum; then the saved layout (ctrlGrdBar1_Load).
 *   btnNew_Click / reset     -> account text cleared, the four check boxes unticked, ChkFromdate.Focus(), grd.DataSource = null + RetrieveStructure().
 *                               (the status combo is NOT cleared and dtvoucher is NOT cleared, so 125-Print still prints the last result).
 *   print_Click_1 / ShowReport -> dtvoucher rows > 0: 125-PdcInventoryPending.rpt (CompanyAddress / CompanyName); else "Record Not Found For Display".
 *   VoucherValidation_KeyDown -> Ctrl+P print, Ctrl+E / Esc close, Ctrl+N reset. (No Enter = Tab on this form.)
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt4, el = A.el, API = '/api/accounts/acrpt4/pdc', seq = 0;
    var RPT = '125-PdcInventoryPending.rpt';
    var last = { columns: [], types: [], rows: [] };                      // dtvoucher
    var HIDDEN = ['Id', 'MGLAccountDrId', 'MGLAccountCrId', 'CompLogoImage'];

    var grid = A.createGrid({ table: el('grd'), scroller: el('gridWrap'), nav: el('nav'), navText: el('navText'),
        headerLines: 1, groupTotals: true, totalRow: true });              // GroupByBoxVisible = false
    grid.clear();

    function columns(data) {                                             // RetrieveStructure() + GridSettings()
        var extra = {};
        HIDDEN.forEach(function (k) { extra[k] = { hidden: true }; });
        extra.AccountTitle = { width: 250 };
        extra.AccountTitleDebit = { width: 250 };
        extra.CheqAmount = { fmt: '0,0', align: 'right', sum: true };
        return A.inferColumns(data, extra);
    }

    // ------------------------------------------------------------------ Load
    function accountTitleFill(rows) {
        A.fillSelect(el('acct'), rows, 'Id', 'AccountTitle', { keep: false });
    }
    function cheqStatus() {                                              // 1 Pending, 2 Clear, 3 All; no row selected
        A.fillSelect(el('status'), [{ Id: 1, Status: 'Pending' }, { Id: 2, Status: 'Clear' }, { Id: 3, Status: 'All' }], 'Id', 'Status', { keep: false });
    }

    // ------------------------------------------------------------------ Show
    function statusText() { var s = el('status'); return s.selectedIndex >= 0 && s.value !== '' ? s.options[s.selectedIndex].textContent.trim() : ''; }
    function show() {                                                    // btnshow_Click
        var b = el('show'); if (b.disabled) return Promise.resolve();
        var q = new URLSearchParams({ accountId: A.intArg(el('acct').value), status: statusText() });
        if (el('chkFrom').checked) q.set('dateFrom', el('datFrom').value || A.today());
        if (el('chkTo').checked) q.set('dateTo', el('datTo').value || A.today());
        if (el('chkCheqFrom').checked) q.set('chqFrom', el('datCheqFrom').value || A.today());
        if (el('chkCheqTo').checked) q.set('chqTo', el('datCheqTo').value || A.today());
        A.busy(b, true);
        var token = ++seq;
        return A.getJson(API + '/data?' + q.toString()).then(function (data) {
            if (token !== seq) return;
            last = data;
            if (data.rows && data.rows.length) grid.setData(columns(data), data.rows);
            else { last = { columns: data.columns, types: data.types, rows: [] }; grid.setData(columns(data), []); }
        }).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ reset (btnNew_Click)
    function reset() {
        A.clearSelect(el('acct'));
        ['chkCheqFrom', 'chkCheqTo', 'chkFrom', 'chkTo'].forEach(function (id) { el(id).checked = false; });
        el('chkFrom').focus();
        grid.clear();
    }

    // ------------------------------------------------------------------ print (ShowReport)
    function printReport() {
        if (!last.rows || !last.rows.length) { alert('Record Not Found For Display'); return; }
        var rows = last.rows.map(function (r) { var o = Object.assign({}, r); o.CompLogoImage = null; return o; });
        A.postGrid(RPT, rows, null, el('print125'));
    }
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    el('show').addEventListener('click', show);
    el('refresh').addEventListener('click', reset);
    el('print125').addEventListener('click', printReport);
    d.addEventListener('keydown', function (e) {                         // VoucherValidation_KeyDown
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'p') { e.preventDefault(); printReport(); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (k === 'n') { e.preventDefault(); reset(); }
    });

    // VoucherValidation_Load
    el('datFrom').value = el('datTo').value = el('datCheqFrom').value = el('datCheqTo').value = A.today();
    cheqStatus();
    A.getJson(API + '/init').then(function (init) {
        var t = init && init.accounts;
        if (t && t.rows && t.rows.length) accountTitleFill(t.rows);
    }).catch(function (e) { alert(e.message); }).then(function () { el('chkFrom').focus(); });
}(window, document));
