/* ============================================================================================
 * countx_export_service_bill_register.js - frmServiceBillHistory.cs (Architecture.WinApp.Service), screen 912 "Service Bill Register".
 * Data: /api/export/service-bill-register (Sp_ExImClearingAgentBill_Rpt + USP_GetDataForDropDownFromClearingAgentBill). Print: exp-541.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptB, API = '/api/export/service-bill-register';
    var $id = H.$id, box = H.box;
    var S = { rows: [], cur: -1, yearStart: '', suspend: true };

    /* gridHisory's dt + GridSettings captions (Services, LcyAmount, Other Charges Amount, Total LcyAmount); Id hidden; GridWrappingAndColumnSettings sums the numeric columns. */
    var COLS = [
        { key: 'Id', hidden: true }, { key: 'DocNo', caption: 'Doc No', num: true, fmt: 'i' }, { key: 'DocDate', caption: 'Doc Date', fmt: 'd' }, { key: 'InvoiceNo', caption: 'Invoice No' },
        { key: 'PartyName', caption: 'Party Name' }, { key: 'RefBillNo', caption: 'Ref Bill No' }, { key: 'ContractNo', caption: 'Contract No' },
        { key: 'NoOfContainer', caption: 'No Of Container', num: true, sum: true, fmt: 'o2' }, { key: 'MTon', caption: 'M Ton', num: true, sum: true, fmt: 'o3' },
        { key: 'BookingRate', caption: 'Booking Rate', num: true, fmt: 'n2' }, { key: 'ItemName', caption: 'Services' }, { key: 'UomCode', caption: 'Uom Code' },
        { key: 'Qty', caption: 'Qty', num: true, sum: true, fmt: 'o3' }, { key: 'ChargesRate', caption: 'Charges Rate', num: true, fmt: 'n2' }, { key: 'CurrencyCode', caption: 'Currency Code' },
        { key: 'FcyAmount', caption: 'Fcy Amount', num: true, sum: true, fmt: 'n2' }, { key: 'ExhangeRate', caption: 'Exhange Rate', num: true, fmt: 'n4' }, { key: 'Amount', caption: 'LcyAmount', num: true, sum: true, fmt: 'n2' },
        { key: 'OtherChargesAmount', caption: 'Other Charges Amount', num: true, sum: true, fmt: 'n2' }, { key: 'TotalAmount', caption: 'Total LcyAmount', num: true, sum: true, fmt: 'n2' },
        { key: 'Description', caption: 'Description' }, { key: 'LoadingPort', caption: 'Loading Port' }, { key: 'DestinationPort', caption: 'Destination Port' },
        { key: 'DebitAccount', caption: 'Debit Account' }, { key: 'DueDays', caption: 'Due Days', num: true, fmt: 'i' }, { key: 'DueDate', caption: 'Due Date', fmt: 'd' }
    ];

    function bindCombos(d) {
        var c = (d && d.combos) || {};
        if (d && d.combosError) box(d.combosError);
        H.bind('CmbSupplier', c.Customer || []); H.bind('cmbInvoiceNo', c.Invoice || []); H.bind('cmbLoadingPort', c.LoadingPort || []);
        H.bind('CmbDestinationPort', c.DestinationPort || []); H.bind('CmbServiceItem', c.Items || []);
    }
    /** frmGDNHistory_Load: ParameterFill, AllComboBind, Rows[1].Activate() (This Week), gridHisory. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {}; S.yearStart = H.isoDate(d.yearStart);
            H.bind('cmbperemeter', d.dateTypes || [], 'Id', 'Parameters');
            H.setText('datFromDate', H.today()); H.setText('dateToDate', H.today());
            bindCombos(d);
            S.suspend = false;
            H.setVal('cmbperemeter', '2'); dateType();
            return gridBind();
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function dateType() { if (!S.suspend) H.dateTypeChanged('cmbperemeter', 'datFromDate', 'dateToDate', S.yearStart, false); }
    function filters() {
        return { fromDate: H.val('datFromDate'), toDate: H.val('dateToDate'), invoiceId: H.netI(H.val('cmbInvoiceNo')), supplierCustomerId: H.netI(H.val('CmbSupplier')),
            loadingPortId: H.netI(H.val('cmbLoadingPort')), destinationPortId: H.netI(H.val('CmbDestinationPort')), itemId: H.netI(H.val('CmbServiceItem')) };
    }
    function render() { H.drawGrid('DataGridHistory', COLS, S.rows, { cur: S.cur }); H.show('grdEmpty', S.rows.length === 0); }
    function gridBind() { return H.postJson(API + '/show', filters()).then(function (rows) { S.rows = rows || []; S.cur = -1; render(); }); }
    function show(btn) { return H.busy(btn, function () { return gridBind().catch(function (e) { box(e.message); }); }); }
    /** Reset(): Rows[1] (This Week), five combos emptied, focus Date Type, grid cleared. */
    function reset() {
        H.setVal('cmbperemeter', '2'); dateType();
        ['cmbLoadingPort', 'cmbInvoiceNo', 'CmbSupplier', 'CmbDestinationPort', 'CmbServiceItem'].forEach(function (id) { H.setVal(id, '0'); });
        H.focus('cmbperemeter'); S.rows = []; S.cur = -1; render();
    }
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/setup').then(bindCombos).catch(function (e) { box(e.message); }); }); }
    /** btnRegister_Click: "Record Not Found For Display" (thrown, shown) - 541-ServiceBillRegister.rpt on dtRec. */
    function print541(btn) {
        if (!S.rows.length) { box('Record Not Found For Display'); return; }
        var f = filters();
        return H.print('exp-541', { exImInvoiceId: f.invoiceId, supplierCustomerId: f.supplierCustomerId, itemId: f.itemId, fromDate: f.fromDate, toDate: f.toDate, loadingPortId: f.loadingPortId, destinationPortId: f.destinationPortId }, btn);
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+S', 'For Show Data'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on Doc Date'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Detail Grid'], ['Ctrl+Enter', 'When Focus On Any Grid For Update Record'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        $id('cmbperemeter').addEventListener('change', dateType);
        H.gridEvents('DataGridHistory', { select: function (i) { S.cur = i; } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); print541($id('btnRegister')); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnshow')); }
            else if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); H.focus('cmbperemeter'); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('DataGridHistory'); }
        });
        load();
    });

    global.ExportSvc = { show: show, reset: reset, refresh: refresh, print541: print541, shortcuts: shortcuts };
}(window));
