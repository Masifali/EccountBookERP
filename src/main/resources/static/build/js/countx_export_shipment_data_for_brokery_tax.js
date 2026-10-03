/* countx_export_shipment_data_for_brokery_tax.js - ShipmentdataForBrokeryTax.cs, screen 242.
 * Data: /api/export/shipment-data-for-brokery-tax. */
(function (global) {
    'use strict';
    var H = global.ExRptC, API = '/api/export/shipment-data-for-brokery-tax';
    var YEAR_START = '', ROWS = [], CUR = -1;
    var COLS = [
        { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true }, { key: 'InvoiceId', hidden: true },
        { key: 'DocDate', cap: 'Doc Date', kind: 'short' }, { key: 'PTYInvoiceNo', cap: 'PTY Invoice No' }, { key: 'FCL', cap: 'FCL', kind: 'q3', sum: true },
        { key: 'CustomerName', cap: 'Consignee' }, { key: 'InvoiceAmount', cap: 'Invoice Amount', kind: 'amt', sum: true }, { key: 'InvoiceExRate', cap: 'Invoice Ex Rate', kind: 'rate' },
        { key: 'LcyAmount', cap: 'Lcy Amount', kind: 'amt', sum: true }, { key: 'BankName', cap: 'Bank Name' }, { key: 'GdNo', cap: 'Gd No' }, { key: 'GdDate', cap: 'Gd Date', kind: 'short' },
        { key: 'GdValue', cap: 'Gd Value', kind: 'amt', sum: true }, { key: 'ExchangeRate', cap: 'Exchange Rate', kind: 'rate' }, { key: 'LcyGdValue', cap: 'Lcy Gd Value', kind: 'amt', sum: true },
        { key: 'DiffAmount', cap: 'Diff Amount', kind: 'both', sum: true }, { key: 'LcyDiffAmount', cap: 'Lcy Diff Amount', kind: 'both', sum: true }
    ];

    function render() { H.drawGrid('grdShipment', ROWS, COLS, { cur: CUR }); H.show('grdShipmentEmpty', !ROWS.length); }
    function dateType() { H.applyDateType(H.val('CmbDateType'), 'txtdatefrom', 'txtdateto', YEAR_START); }
    /** GridBind: rows + groupBox2 with Sum(LcyDiffAmount); without rows the box hides and the labels go back to "0". */
    function show(btn) {
        return H.busy(btn, function () {
            return H.postJson(API + '/show', { fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto') }).then(function (d) {
                ROWS = (d && d.rows) || []; CUR = -1; render();
                if (ROWS.length) {
                    H.show('groupBox2', true);
                    H.setText('lblTotalDiffPkr', H.fmtBoth(d.totalLcyDiff));
                    percentChanged(); brokeryChanged();
                } else {
                    H.show('groupBox2', false);
                    H.setText('lblTotalDiffPkr', '0'); H.setText('lblPercntPkr', '0'); H.setText('lblBrokeryPkr', '0');
                }
            });
        });
    }
    /* txtPercent_TextChanged: outside 0..99 -> "0" + warning; lblPercntPkr = Total * % / 100. */
    function percentChanged() {
        var p = H.netD(H.val('txtPercent'));
        if (p < 0 || p > 99) { H.setText('txtPercent', '0'); H.box("Percent Can't greater than 99"); p = 0; }
        H.setText('lblPercntPkr', H.fmtBoth(H.netD(H.$id('lblTotalDiffPkr').textContent) * (p / 100)));
        brokeryChanged();
    }
    /* txtBrokeryPrcnt_TextChanged: the desktop resets txtPercent (not txtBrokeryPrcnt) on an out-of-range value - kept. */
    function brokeryChanged() {
        var p = H.netD(H.val('txtBrokeryPrcnt'));
        if (p < 0 || p > 99) { H.setText('txtPercent', '0'); H.box("Brokery Percent Can't greater than 99"); }
        H.setText('lblBrokeryPkr', H.fmtBoth(H.netD(H.$id('lblPercntPkr').textContent) * (p / 100)));
    }
    /** Commercial_Invoice_Shipments_Load (the desktop's handler name): ParameterFill, Rows[2] "This Month". */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            YEAR_START = d.yearStart || '';
            H.bindDateTypes('CmbDateType', d.dateTypes);
            H.setText('txtdatefrom', H.today()); H.setText('txtdateto', H.today());
            H.setVal('CmbDateType', 3); dateType();
            H.$id('footerInfo').textContent = 'ShipmentdataForBrokeryTax  -  Print 570';
            H.focus('CmbDateType');
        }).catch(function (e) { H.box(e.message || 'Error occurred during database call.'); });
    }
    /** btnnew_Click: CmbDateType = 3, To = now, grid cleared. */
    function reset() {
        H.setVal('CmbDateType', 3); dateType();
        H.setText('txtdateto', H.today());
        ROWS = []; CUR = -1; render();
    }
    function print(btn) {
        if (!ROWS.length) { H.box('No Record Found For Display'); return; }
        return H.print('570-ShipmentdataForBrokeryTax.rpt', { fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto') }, btn);
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+F5', 'For Focus on Date Type'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+P', 'For Print'], ['Ctrl+S', 'For Showing Data'],
            ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's of Grid (On Button Or Link)"]]);
    }
    function toggleHistory() { var b = H.$id('grdShipmentBox'); if (b) b.scrollIntoView({ behavior: 'smooth' }); H.focusGrid('grdShipment'); }

    document.addEventListener('DOMContentLoaded', function () {
        H.wireFullscreen();
        H.$id('CmbDateType').addEventListener('change', dateType);
        H.$id('txtPercent').addEventListener('input', percentChanged);
        H.$id('txtBrokeryPrcnt').addEventListener('input', brokeryChanged);
        H.wireGrid('grdShipment', { select: function (i) { CUR = i; } });
        H.wireKeys({ shortcuts: shortcuts, handle: function (e, k) {
            if (!e.ctrlKey) return;
            if (k === 'n') { e.preventDefault(); reset(); }
            else if (k === 'r') { e.preventDefault(); /* btnRefresh_Click is empty on the desktop */ }
            else if (k === 'p') { e.preventDefault(); print(H.$id('btnRegisterReport')); }
            else if (k === 's') { e.preventDefault(); show(H.$id('btnShow')); }
            else if (e.key === 'F5') { e.preventDefault(); H.focus('CmbDateType'); }
            else if (e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdShipment'); }
        } });
        load();
    });

    global.ExportBrokeryTax = { show: show, reset: reset, print: print, shortcuts: shortcuts, toggleHistory: toggleHistory };
}(window));
