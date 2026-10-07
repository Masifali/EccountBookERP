/* ============================================================================================
 * Screen 85 "Fcy Bank Charges Register" - Architecture.WinApp.Account_Reports.FcyBankCharges_Register
 * Page: templates/accounts/reports/acrpt3_fcy_bank_charges_register.html (route /accounts/reports/fcy-bank-charges-register).
 * Every desktop handler, in desktop order (FcyBankCharges_Register.cs):
 *   FcyReceiptsSummaryRegister_Load -> ReceiverAccountFill, FDBCNoFill, ChargesAccountFill (USP_GetDataForDropDownFromFcyBankReceipts by Activity
 *                                      ReceiverAccount / FDBCNo / ChargesAccount, rows Id + ReferenceName; bound only when they have rows, else the combo
 *                                      is emptied), then GridBind() - the register runs on open with no filter.
 *   GridBind                        -> usp_FcyBankCharges_Register: @BankId, @ChargesAccountId when non-zero, @FDBCNo = the combo's TEXT when non-empty;
 *                                      rows -> 11 typed columns -> GridSetting(), no rows -> grd.ClearStructure().
 *   GridSetting                     -> HeaderLines 2, centred headers, Id hidden, widths (55 / 75 / 150 / 135 / 160 / 110 / 85 / 110 / 70 / 130), Charges summed,
 *                                      Charges / FTT / TotalRealized in stringFormatbothForFcy, ExchangeRate DecimalFCYRateFormate, TotalLocalAmount stringFormatboth.
 *   btnShow_Click                   -> GridBind().
 *   btnPrint_Click                  -> dtHistory (the procedure's own rows) -> 283-FcyBankChargesRegister.rpt ("Not Record Found For Display").
 *   btnRefresh_Click                -> the three lookups are re-read (selection cleared by the re-bind).
 *   btnnew_Click / Reset            -> the three combos' text cleared, GridBind().
 *   FcyReceiptsSummaryRegister_KeyDown -> Enter = Tab, Ctrl+N New, Ctrl+R Refresh, Ctrl+P Print, Ctrl+E / Esc close, Ctrl+F5 Bank Account, Ctrl+S Show,
 *                                      Ctrl+Down grid, Ctrl+Alt ShortCutKeyPopUp.
 *   btnShortCutKey_Click / MakeShortCutKeys -> ShortCutKeyPopUp (8 rows).
 *   btnShow_Leave                   -> focus moving onto the grid bar sends focus back to Bank Account (the gear is a separate control on the web).
 *   ctrlGrdBar1_Load                -> saved grid layout (countx_grid_bar.js).
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt3, el = A.el;
    var API = '/api/accounts/acrpt3/bankcharges';
    var RPT = '283-FcyBankChargesRegister.rpt';
    var SHORTCUTS = [['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+P', 'For Print'], ['Ctrl+S', 'For Showing Data'],
        ['Ctrl+F5', 'For Focus on Date Type'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+alt', 'To Show ShortCut Keys Form']];

    var look = { fcyDecimals: 0, fcyRateDecimals: 0, amountDecimals: 0 };
    var dtHistory = [];
    var grid, seq = 0;

    function zeros(n) { return new Array(n + 1).join('0'); }
    function fcyFmt() { var z = zeros(look.fcyDecimals); return '#,##0.' + z + ';(0,0.' + z + '); 0'; }
    function lcyFmt() { var z = zeros(look.amountDecimals); return '#,##0.' + z + ';(0,0.' + z + '); 0'; }
    function num(v) { var n = parseFloat(v); return isNaN(n) ? 0 : n; }

    function columns() {
        var f = fcyFmt();
        return [
            { key: 'Id', caption: 'Id', hidden: true, width: 60, type: 'int' },
            { key: 'DocNo', caption: 'DocNo', width: 55, type: 'int' },
            { key: 'DocDate', caption: 'DocDate', width: 75, type: 'date' },
            { key: 'BankAccount', caption: 'BankAccount', width: 150 },
            { key: 'FDBCNo', caption: 'FDBCNo', width: 135 },
            { key: 'ChargesAccount', caption: 'ChargesAccount', width: 160 },
            { key: 'Charges', caption: 'Charges', width: 110, type: 'num', fmt: f, sum: true, totalFmt: f },
            { key: 'FTT', caption: 'FTT', width: 85, type: 'num', fmt: f },
            { key: 'TotalRealized', caption: 'TotalRealized', width: 110, type: 'num', fmt: f },
            { key: 'ExchangeRate', caption: 'ExchangeRate', width: 70, type: 'num', fmt: '#,#0.' + zeros(look.fcyRateDecimals) },
            { key: 'TotalLocalAmount', caption: 'TotalLocalAmount', width: 130, type: 'num', fmt: lcyFmt() }
        ];
    }

    // ------------------------------------------------------------------ lookups (ReceiverAccountFill / FDBCNoFill / ChargesAccountFill)
    function bind(sel, rows) {
        if (rows && rows.length) A.fillSelect(sel, rows, 'Id', 'Name', { keep: false });      // DDL.BindDDL(..., ZeroIndex: false)
        else { A.fillSelect(sel, [], 'Id', 'Name', { keep: false }); }                         // Text = "", DataSource = null
    }
    function fills() {
        return A.getJson(API + '/lookups').then(function (data) {
            look = Object.assign(look, data || {});
            bind(el('bank'), look.bankAccounts); bind(el('fdbc'), look.fdbcNos); bind(el('charges'), look.chargesAccounts);
            if (w.DesktopCombo && w.DesktopCombo.refresh) w.DesktopCombo.refresh();
            ['bank', 'fdbc', 'charges'].forEach(function (id) { el(id).dispatchEvent(new Event('change', { bubbles: true })); });
        });
    }
    function textOf(sel) { var o = sel.selectedIndex >= 0 ? sel.options[sel.selectedIndex] : null; return o && sel.value !== '' ? o.textContent.trim() : ''; }

    // ------------------------------------------------------------------ GridBind
    function gridBind() {
        var q = new URLSearchParams({ bankId: A.intArg(el('bank').value), chargesAccountId: A.intArg(el('charges').value), fdbcNo: textOf(el('fdbc')) });
        var token = ++seq;
        return A.getJson(API + '/data?' + q.toString()).then(function (rows) {
            if (token !== seq) return;
            dtHistory = rows || [];
            if (!dtHistory.length) { grid.clear(); return; }                 // grd.ClearStructure()
            grid.setData(columns(), dtHistory.map(function (r) {
                return {
                    Id: A.intArg(A.ci(r, 'Id')), DocNo: A.intArg(A.ci(r, 'DocNo')), DocDate: A.ci(r, 'DocDate'), BankAccount: A.ci(r, 'BankAccount'),
                    FDBCNo: A.ci(r, 'FDBCNo'), ChargesAccount: A.ci(r, 'ChargesAccount'), Charges: num(A.ci(r, 'Charges')), FTT: num(A.ci(r, 'FTT')),
                    TotalRealized: num(A.ci(r, 'TotalRealized')), ExchangeRate: num(A.ci(r, 'ExchangeRate')), TotalLocalAmount: num(A.ci(r, 'TotalLocalAmount'))
                };
            }));
        });
    }
    function show() {
        var b = el('show'); if (b.disabled) return Promise.resolve();
        A.busy(b, true);
        return gridBind().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }

    // ------------------------------------------------------------------ New / Refresh / Print
    function reset() {
        var b = el('new'); if (b.disabled) return;
        ['bank', 'fdbc', 'charges'].forEach(function (id) { A.clearSelect(el(id)); });     // Text = string.Empty
        A.busy(b, true);
        gridBind().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        fills().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function print283() {
        if (!dtHistory.length) { alert('Not Record Found For Display'); return; }
        A.postGrid(RPT, dtHistory, null, el('print'));
    }
    function closeForm() { w.location.href = '/accounts/dashboard'; }

    // ------------------------------------------------------------------ events
    el('show').addEventListener('click', show);
    el('new').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print283);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    el('closeShortcuts').addEventListener('click', function () { el('shortcutDialog').close(); });

    /* tab order: CmbBankAccount 3, CmbChargesAccount 4, btnShow 5, CmbFDBCNo 585 */
    function focusables() { return [A.focusOf('bank'), A.focusOf('charges'), el('show'), A.focusOf('fdbc')]; }
    d.addEventListener('keydown', function (e) {                         // FcyReceiptsSummaryRegister_KeyDown
        var dlg = el('shortcutDialog'); if (dlg.open) return;
        if (e.ctrlKey && e.altKey && (e.key === 'Control' || e.key === 'Alt')) { e.preventDefault(); A.shortcuts(SHORTCUTS); return; }
        if (e.key === 'Enter' && !e.ctrlKey && !e.altKey && !e.shiftKey && !A.comboOpen() && e.target.tagName !== 'BUTTON' && !el('grid').contains(e.target)) {
            var list = focusables(), i = list.indexOf(d.activeElement);
            if (i >= 0 && i < list.length - 1) { e.preventDefault(); list[i + 1].focus(); return; }
        }
        if (e.key === 'Escape' && !A.comboOpen()) { e.preventDefault(); closeForm(); return; }
        if (!e.ctrlKey || e.altKey) return;
        var k = e.key.toLowerCase();
        if (k === 'n') { e.preventDefault(); reset(); }
        else if (k === 'r') { e.preventDefault(); refresh(); }
        else if (k === 'p') { e.preventDefault(); print283(); }
        else if (k === 'e') { e.preventDefault(); closeForm(); }
        else if (e.key === 'F5') { e.preventDefault(); A.focusOf('bank').focus(); }
        else if (k === 's') { e.preventDefault(); show(); }
        else if (e.key === 'ArrowDown') { e.preventDefault(); grid.focus(); }
    });

    // ------------------------------------------------------------------ start (FcyReceiptsSummaryRegister_Load)
    grid = A.createGrid({ table: el('grd'), scroller: el('grid'), nav: el('nav'), navText: el('navText'), headerLines: 2,
        alternate: false, groupTotals: true, totalRow: true });
    grid.clear();
    fills().catch(function (e) { alert(e.message); }).then(function () {
        return gridBind().catch(function (e) { alert(e.message); });
    });
}(window, document));
