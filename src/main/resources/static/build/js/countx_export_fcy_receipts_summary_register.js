/* countx_export_fcy_receipts_summary_register.js - FcyReceiptsSummaryRegister.cs (Account_Reports), screen 241
 * "5017 Fcy Receipts Report". Data: /api/export/fcy-receipts-summary-register (ExportReportsCController). */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/fcy-receipts-summary-register';
    var YEAR_START = '', ROWS = [], CUR = -1;
    var COLS = [
        { key: 'VoucherDate', cap: 'Voucher Date', kind: 'date' }, { key: 'VoucherNo', cap: 'Voucher No' }, { key: 'PaymentTerm', cap: 'Payment Term' },
        { key: 'Invoice/Contract', cap: 'Invoice/Contract' }, { key: 'CurrencyName', cap: 'Fcy' }, { key: 'ExchangeRate', cap: 'Exchange Rate', kind: 'rate' },
        { key: 'SenderAccountTitle', cap: 'Credit Account Title' }, { key: 'FcySenderCr', cap: 'Fcy Amount Cr', kind: 'both', sum: true }, { key: 'LcySenderCr', cap: 'Lcy Amount Cr', kind: 'both', sum: true },
        { key: 'ReceiverAccountTitle', cap: 'Debit Account Title' }, { key: 'FcyReceiverDr', cap: 'Fcy Amount Dr', kind: 'both', sum: true }, { key: 'LcyReceiverDr', cap: 'Lcy Amount Dr', kind: 'both', sum: true },
        { key: 'FcyDecutionsDr', cap: 'Fcy Decutions Dr', kind: 'both', sum: true }, { key: 'LcyDecutionsDr', cap: 'Lcy Decutions Dr', kind: 'both', sum: true },
        { key: 'FcyGdsAdjusted', cap: 'Fcy Gds Adjusted', kind: 'both', sum: true }, { key: 'FcyGdsCommission', cap: 'Fcy Gds Commission', kind: 'both', sum: true },
        { key: 'FcyGdsRealized', cap: 'Fcy Gds Realized', kind: 'both', sum: true }
    ];

    function render() {
        H.drawGrid('grd', ROWS, COLS, { cur: CUR });
        H.show('grdEmpty', !ROWS.length);
    }
    function bindCombos(d) {
        if (d.combosError) H.box(d.combosError);
        H.bind('CmbSender', d.senders || [], 'Id', 'Name');
        H.bind('CmbReceiver', d.receivers || [], 'Id', 'Name');
        H.bind('CmbInvoiceNo', d.invoices || [], 'Id', 'Name');
        H.bind('CmbPaymentTerm', d.paymentTerms || [], 'Id', 'Name');
    }
    function dateType() { H.applyDateType(H.val('cmbperemeter'), 'txtFromDate', 'txtToDate', YEAR_START); }
    /* GridBind: PaymentTerm = CmbPaymentTerm.Text (the caption). */
    function filters() {
        return { fromDate: H.val('txtFromDate'), toDate: H.val('txtToDate'), senderId: H.netI(H.val('CmbSender')), receiverId: H.netI(H.val('CmbReceiver')),
            invoiceId: H.netI(H.val('CmbInvoiceNo')), paymentTerm: H.selText('CmbPaymentTerm') };
    }
    function show(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/show', filters()).then(function (rows) { ROWS = rows || []; CUR = -1; render(); });
        });
    }
    /** FcyReceiptsSummaryRegister_Load: combos, Rows[2] ("This Month") when nothing is chosen, GridBind. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            YEAR_START = d.yearStart || '';
            H.bindDateTypes('cmbperemeter', d.dateTypes);
            H.setText('txtFromDate', H.today()); H.setText('txtToDate', H.today());
            bindCombos(d);
            if (H.netI(H.val('cmbperemeter')) === 0) { H.setVal('cmbperemeter', 3); dateType(); }
            H.$id('footerInfo').textContent = 'FcyReceiptsSummaryRegister  -  Print 288';
            return show(null).then(function () { H.focus('cmbperemeter'); });
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }
    /** Reset(): date type back to Rows[2], Sender / Receiver cleared, GridBind. (Invoice / Payment Term stay, as on the desktop.) */
    function reset() {
        H.focus('cmbperemeter');
        H.setVal('cmbperemeter', 3); dateType();
        H.setVal('CmbSender', 0); H.setVal('CmbReceiver', 0);
        show(null);
    }
    /** btnRefresh_Click: AllComboBind. */
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindCombos); }); }
    /** btnPrint_Click: "Not Record Found For Display" without data, else 288-FcyReceiptsSummaryRegister.rpt with the grid's parameters. */
    function print(btn) {
        if (!ROWS.length) { H.box('Not Record Found For Display'); return; }
        var f = filters();
        return H.print('288-FcyReceiptsSummaryRegister.rpt', { fromDate: f.fromDate, toDate: f.toDate, supplierCustomerId: f.senderId || undefined,
            accountId: f.receiverId || undefined, invoiceId: f.invoiceId || undefined, paymentTerm: f.paymentTerm || undefined }, btn);
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+P', 'For Print'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+F5', 'For Focus on Date Type'],
            ['Ctrl+Enter', 'For Showing Data'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+E', 'For Close']]);
    }
    function toggleHistory() { var b = H.$id('grdBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('grd'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.$id('cmbperemeter').addEventListener('change', dateType);
        H.wireGrid('grd', { select: function (i) { CUR = i; } });
        H.wireKeys({ shortcuts: shortcuts, handle: function (e, k) {
            if (!e.ctrlKey) return;
            if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'r') { e.preventDefault(); refresh(H.$id('btnRefresh')); }
            else if (k === 'p') { e.preventDefault(); print(H.$id('btnPrint')); }
            else if (e.key === 'F5') { e.preventDefault(); H.focus('cmbperemeter'); }
            else if (e.key === 'Enter') { e.preventDefault(); show(H.$id('btnShow')); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grd'); }
        } });
        load();
    });

    global.ExportFcyReceipts = { show: show, reset: reset, refresh: refresh, print: print, shortcuts: shortcuts, toggleHistory: toggleHistory };
}(window));
