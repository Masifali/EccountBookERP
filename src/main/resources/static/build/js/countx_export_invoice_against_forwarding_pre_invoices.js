/* ============================================================================================
 * countx_export_invoice_against_forwarding_pre_invoices.js - CommercialInvoiceAgainstForwardingPreInvoices.cs, screen 259
 * "Export Sales Report (Commercial Invoice Against Forwarding/Pre Invoices)" (Invoice Register).
 * Data: /api/export/invoice-against-forwarding-pre-invoices (USP_GetDataForDropDownFromExportInvoice '211',
 * USP_ExportInvoiceAgainstForwarding_Register). Print 558 through CrystalPrint key "558-exportinvoiceagainstforwarding-register".
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, N = global.ExportRptN, $id = H.$id, box = H.box;
    var API = '/api/export/invoice-against-forwarding-pre-invoices';
    var decFcyRate = 0;

    function u3(v) { return N.fmtUpTo(v, 3); }
    function cols() { /* GridBind dtship + GridSettingForShipmentDetail */
        return [
            { key: 'CustomerName', caption: 'CustomerName' }, { key: 'ContractNo', caption: 'ContractNo' }, { key: 'ContractDate', caption: 'ContractDate', fmt: 'd' },
            { key: 'InvoiceNo', caption: 'InvoiceNo' }, { key: 'InvoiceDate', caption: 'InvoiceDate', fmt: 'd' }, { key: 'FcyCode', caption: 'FcyCode' },
            { key: 'ExchangeRate', caption: 'ExchangeRate', num: true, fmt: function (v) { return N.fmtRate(v, decFcyRate); } },
            { key: 'CommissionAgent', caption: 'CommissionAgent' }, { key: 'Comm%', caption: 'Comm%', num: true, fmt: u3 },
            { key: 'CommissionAmount', caption: 'CommissionAmount', num: true, sum: true, fmt: u3 }, { key: 'HsCode', caption: 'HsCode' },
            { key: 'ItemName', caption: 'ItemName' }, { key: 'CropYear', caption: 'CropYear' }, { key: 'PackingType', caption: 'PackingType' },
            { key: 'NoofContainers', caption: 'No Of Containers', num: true, sum: true, fmt: u3 },
            { key: 'NoofBagsPerContainer', caption: 'NoofBagsPerContainer', num: true, fmt: u3 }, { key: 'PackUom', caption: 'PackUom' },
            { key: 'MTon', caption: 'MTon', num: true, sum: true, fmt: u3 }, { key: 'NoofBags', caption: 'NoofBags', num: true, sum: true, fmt: u3 },
            { key: 'NetWeight', caption: 'NetWeight', num: true, sum: true, fmt: u3 }, { key: 'PackingWeight', caption: 'PackingWeight', num: true, sum: true, fmt: u3 },
            { key: 'GrossWeight', caption: 'GrossWeight', num: true, sum: true, fmt: u3 }, { key: 'RatePrice', caption: 'RatePrice', num: true, fmt: u3 },
            { key: 'RateUom', caption: 'RateUom' }, { key: 'FcAmount', caption: 'FcAmount', num: true, sum: true, fmt: u3 },
            { key: 'BankRate', caption: 'BankRate', num: true, fmt: u3 }, { key: 'BankAmount', caption: 'BankAmount', num: true, sum: true, fmt: u3 },
            { key: 'OtherHsCode', caption: 'OtherHsCode' }, { key: 'PackingExpiryDate', caption: 'PackingExpiryDate', width: 120 },
            { key: 'ProductionNo', caption: 'ProductionNo', width: 70 }, { key: 'ItemCommodity', caption: 'ItemCommodity' }, { key: 'HealthPermit', caption: 'HealthPermit' }
        ];
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Alt+1', 'For Print 543'], ['Ctrl+F5', 'For Focus on Date Type'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+S', 'For Showing Data'], ['Ctrl+ArrowDown', 'For Focus On Grid'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    var P = N.contractRegisterPage({ api: API, grid: 'grdShipmentDetail', empty: 'grdShipmentDetailEmpty', cols: cols, shortcuts: shortcuts,
        onSetup: function (d) { decFcyRate = H.netI(d.decimalsFcyRate); } });

    /** btnRegisterPrint543_Click (caption "Print-558"): "Not Record Found For Display" while the grid table is empty. */
    function print558(btn) {
        var l = P.st.last;
        if (!P.st.rows.length || !l) { box('Not Record Found For Display'); return; }
        return H.print('558-exportinvoiceagainstforwarding-register', N.args({ fromDate: l.fromDate, toDate: l.toDate, supplierCustomerId: l.supplierCustomerId,
            exImLcOrderId: l.exImLcOrderId, itemTypeId: l.itemTypeId, itemId: l.itemId }), btn || $id('btnRegisterPrint543'));
    }
    document.addEventListener('DOMContentLoaded', function () {
        N.embedFooter('fwdFooter');
        H.gridEvents('grdShipmentDetail', {});
        P.wire(function (e) { if (e.altKey && !e.ctrlKey && (e.key === '1' || e.code === 'Digit1' || e.code === 'Numpad1')) { e.preventDefault(); print558(); } });
        P.load();
    });
    global.ExportFwd = { btnNew: P.reset, refresh: function () { /* btnRefresh_Click: empty on the desktop */ }, show: P.show, print558: print558, shortcuts: shortcuts };
}(window));
