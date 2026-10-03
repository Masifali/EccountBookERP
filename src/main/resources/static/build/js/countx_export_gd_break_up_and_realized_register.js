/* countx_export_gd_break_up_and_realized_register.js - GDBreakUpandRealized_Register.cs, screen 245.
 * Data: /api/export/gd-break-up-and-realized-register. */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/gd-break-up-and-realized-register';
    var YEAR_START = '', ROWS = [], CUR = -1;
    var COLS = [
        { key: 'Id', hidden: true }, { key: 'ExImInvoiceId', hidden: true }, { key: 'ShipDate', cap: 'Ship Date' }, { key: 'Tenor', cap: 'Tenor' }, { key: 'DueDate', cap: 'Due Date' },
        { key: 'PTYInvoiceNo', cap: 'PTY Invoice No' }, { key: 'GDNO', cap: 'GDNO' }, { key: 'GDDate', cap: 'GD Date', kind: 'date' }, { key: 'BankInvoiceNo', cap: 'Bank Invoice No' },
        { key: 'BankInvoiceDate', cap: 'Bank Invoice Date', kind: 'date' }, { key: 'BankName', cap: 'Bank Name' }, { key: 'GDValue', cap: 'GD Value', kind: 'amt', sum: true },
        { key: 'Freight', cap: 'Freight', kind: 'amt', sum: true }, { key: 'FOBValue', cap: 'FOB Value', kind: 'amt', sum: true }, { key: 'FTT%', cap: 'FTT%', kind: 'amt', sum: true },
        { key: 'FTT/M-FORM', cap: 'FTT/M-FORM', kind: 'amt', sum: true }, { key: 'NetToBeRealized', cap: 'Net To Be Realized', kind: 'amt', sum: true },
        { key: 'Realized', cap: 'Realized', kind: 'amt', sum: true }, { key: 'Balance', cap: 'Balance', kind: 'amt', sum: true }, { key: 'ExchangeRate', cap: 'Exchange Rate', kind: 'amt' },
        { key: 'GdLcyAmount', cap: 'Gd Lcy Amount', kind: 'amt', sum: true }, { key: 'RealizedLcyAmount', cap: 'Realized Lcy Amount', kind: 'amt', sum: true },
        { key: 'UnRealizedLcyAmount', cap: 'Un Realized Lcy Amount', kind: 'amt', sum: true }, { key: 'GdStatus', cap: 'Gd Status' }
    ];

    function render() { H.drawGrid('DataGridHistory', ROWS, COLS, { cur: CUR }); H.show('DataGridHistoryEmpty', !ROWS.length); }
    function bindCombos(d) { H.bind('cmbinvoice', d.invoices || [], 'Id', 'Name'); H.bind('cmbbank', d.banks || [], 'Id', 'Name'); }
    function dateType() { H.applyDateType(H.val('cmbperemeter'), 'datFromDateSt', 'datToDateST', YEAR_START); }
    function filters() {
        return { fromDate: H.val('datFromDateSt'), toDate: H.val('datToDateST'), invoiceId: H.netI(H.val('cmbinvoice')), bankId: H.netI(H.val('cmbbank')), skipZero: H.checked('ChkZkipZeroSelected') };
    }
    function show(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/show', filters()).then(function (rows) { ROWS = rows || []; CUR = -1; render(); });
        });
    }
    /** GDBreakUpandRealized_Register_Load: ParameterFill (Rows[2] "This Month"), AllDropDownBind. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            YEAR_START = d.yearStart || '';
            H.bindDateTypes('cmbperemeter', d.dateTypes);
            H.setText('datFromDateSt', H.today()); H.setText('datToDateST', H.today());
            H.setVal('cmbperemeter', 3); dateType();
            bindCombos(d);
            H.$id('footerInfo').textContent = 'GDBreakUpandRealized_Register  -  Print 559';
            H.focus('cmbperemeter');
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }
    /** btnRefreshSelectedTrial_Click ("Reset"): Invoice / Bank cleared, ParameterFill -> Rows[2] again. */
    function reset() {
        H.setVal('cmbinvoice', 0); H.setVal('cmbbank', 0);
        H.focus('cmbperemeter');
        H.setVal('cmbperemeter', 3); dateType();
    }
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindCombos); }); }
    function print(btn) {
        if (!ROWS.length) { H.box('Not Record Found For Display'); return; }
        var f = filters();
        return H.print('559-GDBreakUpandRealized_Register.rpt', { fromDate: f.fromDate, toDate: f.toDate, exImFCBankReceiptsId: f.bankId || undefined,
            exImInvoiceId: f.invoiceId || undefined, zeroBalanceType: f.skipZero ? 1 : undefined }, btn);
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+S', 'For Shoe Record'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Combo Date'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid History'], ['Ctrl+ArrowUp', 'For Focus On Date combo in Filters Box'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    function toggleHistory() { var b = H.$id('DataGridHistoryBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('DataGridHistory'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.$id('cmbperemeter').addEventListener('change', dateType);
        H.wireGrid('DataGridHistory', { select: function (i) { CUR = i; } });
        H.wireKeys({ shortcuts: shortcuts, handle: function (e, k) {
            if (!e.ctrlKey) return;
            if (k === 's') { e.preventDefault(); show(H.$id('btnShow')); }
            else if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'r') { e.preventDefault(); refresh(H.$id('btnRefresh')); }
            else if (k === 'p') { e.preventDefault(); print(H.$id('btnReport')); }
            else if (e.key === 'F5' || e.key === 'ArrowUp') { e.preventDefault(); H.focus('cmbperemeter'); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('DataGridHistory'); }
        } });
        load();
    });

    global.ExportGdRealized = { show: show, reset: reset, refresh: refresh, print: print, shortcuts: shortcuts, toggleHistory: toggleHistory };
}(window));
