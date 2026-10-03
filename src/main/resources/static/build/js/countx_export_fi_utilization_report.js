/* ============================================================================================
 * countx_export_fi_utilization_report.js - frmFinancialInstrumentAdvanceBalanceSummary.cs, screen 935 "5017 FI Utilization Report".
 * Data: /api/export/fi-utilization-report (usp_FinancialInstrumentAdvanceBalanceSummary, Sp_SupplierCustomer_GetAllMethod
 * 'GetCustomerIdByLcOrderAndFcReceipt', USP_GetDataForDropDownFromFcyBankReceipts 'ReceiverAccount', usp_getFIUtilizeInfoByFIId for the
 * frmFinancialInstrumentUtilizedDetailByFI popup). Print: exp-300-02.
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptB, API = '/api/export/fi-utilization-report';
    var $id = H.$id, box = H.box;
    var S = { rows: [], cur: -1, yearStart: '', suspend: true };

    /* gridHisory's dtHisGrid + GridSetting: hidden Id, SupplierCustomerId, BankAccountId, PaymenttermId, DocumentTypeId, AdjustedAgainstGdAmount;
       "FI NO" captioned "FI Number" as a link; amounts "#,##0.##"; GridWrappingAndColumnSettings sums the numeric columns. */
    var COLS = [
        { key: 'Id', hidden: true }, { key: 'DocNo', caption: 'Doc No' }, { key: 'DocDate', caption: 'Doc Date', fmt: 'd' }, { key: 'DocType', caption: 'Doc Type' },
        { key: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'Consignee', caption: 'Consignee' }, { key: 'ContractNo', caption: 'Contract No' },
        { key: 'FI NO', caption: 'FI Number', link: true }, { key: 'FIDate', caption: 'FI Date', fmt: 'd' }, { key: 'FIExpiryDate', caption: 'FI ExpiryDate', fmt: 'd' },
        { key: 'BankAccountId', hidden: true }, { key: 'BankName', caption: 'Bank Name' }, { key: 'TotalAmount', caption: 'Total Amount', num: true, sum: true, fmt: 'o2' },
        { key: 'AdjustedAmount', caption: 'Adjusted Amount', num: true, sum: true, fmt: 'o2' }, { key: 'AdjustedAgainstGdAmount', hidden: true },
        { key: 'BalanceAmount', caption: 'Balance Amount', num: true, sum: true, fmt: 'o2' }, { key: 'FcyCode', caption: 'Fcy Code' }, { key: 'ExchangeRate', caption: 'Exchange Rate', num: true, fmt: 'n4' },
        { key: 'LcyAmount', caption: 'Lcy Amount', num: true, sum: true, fmt: 'o2' }, { key: 'InvoiceNo', caption: 'Invoice No' }, { key: 'PaymenttermId', hidden: true },
        { key: 'DocumentTypeId', hidden: true }, { key: 'DestinationPort', caption: 'Destination Port' }
    ];
    /* frmFinancialInstrumentUtilizedDetailByFI.BindGdbreakUp: hidden FIId, EximInvoiceId, GDId, RefDocumentTypeId, SupplierCustomerId, ConsigneeId, BankId; TotalRow on. */
    var FI_COLS = [
        { key: 'FIId', hidden: true }, { key: 'FINo', caption: 'FI No' }, { key: 'FIDate', caption: 'FI Date', fmt: 'd' }, { key: 'EximInvoiceId', hidden: true }, { key: 'InvoiceNo', caption: 'Invoice No' },
        { key: 'InvoiceDate', caption: 'Invoice Date', fmt: 'd' }, { key: 'GDId', hidden: true }, { key: 'RefDocumentTypeId', hidden: true }, { key: 'GdNo', caption: 'Gd No' },
        { key: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'ConsigneeId', hidden: true }, { key: 'Consignee', caption: 'Consignee' },
        { key: 'BankId', hidden: true }, { key: 'BankAccount', caption: 'Bank Account' }, { key: 'UtilizeAmount', caption: 'Utilize Amount', num: true, sum: true, fmt: 'n2' },
        { key: 'FcyCode', caption: 'Fcy Code' }, { key: 'ExchangeRate', caption: 'Exchange Rate', num: true, fmt: 'n4' }, { key: 'LcyAmount', caption: 'Lcy Amount', num: true, sum: true, fmt: 'n2' },
        { key: 'InvoiceMTon', caption: 'Invoice M Ton', num: true, sum: true, fmt: 'o3' }, { key: 'InvoiceAmount', caption: 'Invoice Amount', num: true, sum: true, fmt: 'n2' }
    ];

    function bindCombos(d) {
        ['customers', 'banks'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
        if (d.customers) H.bind('CmbSupplier', d.customers);
        if (d.banks) H.bind('CmbBankName', d.banks);
    }
    /** Load: suppliercustomer, BankNameFill, ParameterFill, Rows[4].Activate() (Financial Year -> From = FY start); grid only on Show. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {}; S.yearStart = H.isoDate(d.yearStart);
            H.bind('cmbperemeter', d.dateTypes || [], 'Id', 'Parameters');
            H.setText('Fromdate', H.today()); H.setText('ToDate', H.today());
            bindCombos(d);
            S.suspend = false;
            H.setVal('cmbperemeter', '5'); dateType();
            render();
            H.focus('cmbperemeter');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function dateType() { if (!S.suspend) H.dateTypeChanged('cmbperemeter', 'Fromdate', 'ToDate', S.yearStart, false); }
    function filters() {
        return { fromDate: H.val('Fromdate'), toDate: H.val('ToDate'), bankId: H.netI(H.val('CmbBankName')), supplierCustomerId: H.netI(H.val('CmbSupplier')),
            skipZero: H.checked('chkSkipZero'), actionId: H.netI(H.radio('exportType')) || 2 };
    }
    function render() { H.drawGrid('DataGridHistory', COLS, S.rows, { cur: S.cur }); H.show('grdEmpty', S.rows.length === 0); }
    function show(btn) {
        return H.busy(btn, function () { return H.postJson(API + '/show', filters()).then(function (rows) { S.rows = rows || []; S.cur = -1; render(); }).catch(function (e) { box(e.message); }); });
    }
    /** Reset(): focus From Date, Customer emptied. */
    function reset() { H.focus('Fromdate'); H.setVal('CmbSupplier', '0'); }
    /** btnRefresh_Click: suppliercustomer, BankNameFill. */
    function refresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/setup').then(bindCombos).catch(function (e) { box(e.message); }); }); }
    /** btn334Register_Click: 300_02-FIAdvanceBalanceSummary.rpt on dt. */
    function print300(btn) {
        if (!S.rows.length) { box('Not Record Found For Display'); return; }
        var f = filters();
        return H.print('exp-300-02', { supplierCustomerId: f.supplierCustomerId, bankId: f.bankId, fromDate: f.fromDate, toDate: f.toDate, skipZero: f.skipZero ? 1 : 0, actionId: f.actionId }, btn);
    }
    /** DataGridHistory_LinkClicked "FI NO": the utilize-detail popup when AdjustedAmount > 0. */
    function link(key, i) {
        var r = S.rows[i]; if (!r || key !== 'FI NO') return;
        if (!(H.netD(r.AdjustedAmount) > 0)) { box('No Breakup Found Against this Financial Instrument Number...'); return; }
        H.modal('rptbFiDetail', 'Financial Instrument Utilize Detail',
            '<div class="ex-grid-scroll ex-h400"><table class="win-grid" id="rptbFiGrid"><thead></thead><tbody></tbody><tfoot></tfoot></table></div><div class="ex-grid-empty is-hidden" id="rptbFiEmpty">No record found.</div>', true);
        H.getJson(API + '/utilize-detail?fiId=' + H.netI(r.Id) + '&documentTypeId=' + H.netI(r.DocumentTypeId)).then(function (rows) {
            rows = rows || [];
            H.drawGrid('rptbFiGrid', FI_COLS, rows, {});
            H.show('rptbFiEmpty', rows.length === 0);
        }).catch(function (e) { H.closeModal('rptbFiDetail'); box(e.message); });
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+S', 'For Shoe Record'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Alt+1', 'For Print 334-Register'], ['Alt+2', 'For Print 335-Register'],
            ['Ctrl+F5', 'For Focus on Combo Date'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid History'],
            ['Ctrl+ArrowUp', 'For Focus On Date combo in Filters Box'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        $id('cmbperemeter').addEventListener('change', dateType);
        H.gridEvents('DataGridHistory', { select: function (i) { S.cur = i; }, link: link, ctrlSpace: function (i) { link('FI NO', i); } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnshow')); }
            else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); H.focus('cmbperemeter'); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); reset(); }
            else if (e.altKey && (e.key === '1' || e.code === 'Numpad1')) { e.preventDefault(); print300($id('btn300_02Register')); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); refresh($id('btnRefresh')); }
            else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('DataGridHistory'); }
        });
        load();
    });

    global.ExportFiu = { show: show, reset: reset, refresh: refresh, print300: print300, shortcuts: shortcuts };
}(window));
