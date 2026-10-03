/* ============================================================================================
 * countx_export_receivable_by_due_date.js - ExportReceivableByDueDateRegisterA.cs, screen 235
 * "Export Receivable Register by Due Date". Data: /api/export/receivable-by-due-date
 * (ExportReportsAController -> ExportReportsAService -> USP_ExportReceivableByDueDateRegisterA, 6 tables).
 * Print 243-ExportReceivablesDueByDate.rpt through CrystalPrint key "exp-243" (registered by the coordinator).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, $id = H.$id, box = H.box;
    var API = '/api/export/receivable-by-due-date';
    var S = { yearStart: '', rows: [], eta: [], sortNo: 0, cur: -1 };

    /* dtRegister columns, in order; InvoiceId / ContractIds / Receipt / Advance hidden; captions as GridSetting. */
    var COLS = [
        { key: 'Customer', caption: 'Customer' }, { key: 'BankName', caption: 'Bank Name' }, { key: 'ContractIds', hidden: true },
        { key: 'ContractNo', caption: 'Contract No', link: true }, { key: 'InvoiceId', hidden: true }, { key: 'InvoiceNo', caption: 'Invoice No' },
        { key: 'PaymentTerm', caption: 'Payment Term' }, { key: 'Fcy', caption: 'Fcy' },
        { key: 'InvoiceValue', caption: 'Invoice Value', num: true, sum: true, fmt: 'n2' },
        { key: 'AdvanceReceipt', caption: 'Receipt', num: true, sum: true, fmt: 'n2' },
        { key: 'NetReceivable', caption: 'Net Receivable', num: true, sum: true, fmt: 'n2' },
        { key: 'DueDate', caption: 'Due Date', fmt: 'dmy', cls: function (v) { return H.isoDate(v) ? '' : 'red'; } },
        { key: 'InvoiceDate', caption: 'Invoice Date', fmt: 'dmy' }, { key: 'BolDate', caption: 'Bol Date', fmt: 'dmy' },
        { key: 'DueDays', caption: 'Due Days', fmt: 'o3', cls: function (v) { return H.netD(v) === 0 ? 'red' : ''; } },
        { key: 'OverDueDays', caption: 'Over Due Days', fmt: 'i', cls: function (v) { return H.netD(v) > 0 ? 'red' : ''; } },
        { key: 'ETADestination', caption: 'ETA Destination', fmt: 'dmy' }, { key: 'ReachedAtDestination', caption: 'Reached At Destination', fmt: 'dmy' },
        { key: 'FCL', caption: 'FCL' }, { key: 'MTons', caption: 'M Tons', num: true, sum: true, fmt: 'n2' }, { key: 'SaleMan', caption: 'Sale Man' },
        { key: 'ShippedOnBoard', caption: 'Shipped On Board', fmt: 'dmy' }, { key: 'InTransitDays', caption: 'In Transit Days', fmt: 'o3' },
        { key: 'IncoTerm', caption: 'Inco Term' }, { key: 'Advance', hidden: true }, { key: 'Receipt', hidden: true },
        { key: 'DestinationPort', caption: 'Destination Port' }, { key: 'ExportReceivablesAging', caption: 'Export Receivables Aging' },
        { key: 'InvoiceStatus', caption: 'Invoice Status' }
    ];
    function shareCols(name, cap, fmt3) {
        var c = [];
        if (name) c.push({ key: name, caption: cap });
        c.push({ key: 'Fcy', caption: 'Fcy' });
        c.push({ key: 'NetReceivable', caption: 'Net Receivable', num: true, sum: true, fmt: fmt3 ? 'o3' : 'n2' });
        c.push({ key: 'TotalReceivable', hidden: !fmt3, caption: 'Total Receivable', num: true, sum: true, fmt: fmt3 ? 'o3' : 'n3' });
        c.push({ key: 'PrcntOfTotal', caption: '% Of Total', num: true, sum: true, fmt: fmt3 ? 'o3' : 'n2' });
        return c;
    }
    var ETA_COLS = [{ key: 'SortNo', hidden: true }, { key: 'ETADestinationPort', caption: 'Export Receivables Aging', width: 325 },
        { key: 'FcyAmount', caption: 'Fcy Amount', num: true, sum: true, fmt: 'n2' }, { key: 'PkrAmount', caption: 'Pkr Amount', num: true, sum: true, fmt: 'n2' }];

    function bindAll(d) {
        ['invoices', 'customers', 'contracts', 'deliveryTerms', 'paymentTerms', 'ports'].forEach(function (k) { if (d[k + 'Error']) box(d[k + 'Error']); });
        H.bind('CmbInvoiceNo', d.invoices, 'Id', 'name');
        H.bind('CmbCustomerName', d.customers, 'Id', 'name');
        H.bind('cmbContractNo', d.contracts, 'Id', 'name');
        H.bind('cmbDeliveryTerm', d.deliveryTerms, 'Id', 'name');
        H.bind('cmbPaymentTerm', d.paymentTerms, 'Id', 'name');
        H.bind('cmbDestinationPort', d.ports, 'Id', 'name');
    }
    /** frmExportShipingLineBookingRpt_Load: focus From, From = ActiveYr.Start_Period, both pickers unchecked, combos, GridBind. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            S.yearStart = d.yearStart || '';
            H.setText('datFromDate', S.yearStart); $id('datFromDateChk').checked = false; $id('datToDateChk').checked = false;
            bindAll(d);
            H.focus('datFromDate');
            return gridBind(0);
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function filters(sortNo) {
        return {
            fromChecked: H.checked('datFromDateChk'), fromDate: H.val('datFromDate'), toChecked: H.checked('datToDateChk'), toDate: H.val('datToDate'),
            customerId: H.netI(H.val('CmbCustomerName')), contractId: H.netI(H.val('cmbContractNo')), invoiceId: H.netI(H.val('CmbInvoiceNo')),
            paymentTermId: H.netI(H.val('cmbPaymentTerm')), deliveryTermId: H.netI(H.val('cmbDeliveryTerm')), destinationPortId: H.netI(H.val('cmbDestinationPort')),
            skipZero: H.checked('chkSkipZero'), sortNo: sortNo || 0
        };
    }
    /** GridBind(SortNo). */
    function gridBind(sortNo) {
        S.sortNo = sortNo || 0;
        return H.postJson(API + '/show', filters(S.sortNo)).then(function (d) {
            d = d || {};
            S.rows = d.register || []; S.eta = d.eta || []; S.cur = -1;
            H.drawGrid('grdfrm', COLS, S.rows, { group: 'ExportReceivablesAging', groupTotals: true });
            H.show('grdfrmEmpty', !S.rows.length);
            H.drawGrid('grdBank', shareCols('BankName', 'Bank Name'), d.bank || [], {});
            H.drawGrid('grdCustomer', shareCols('Customer', 'Customer'), d.customer || [], {});
            H.drawGrid('grdSaleMan', shareCols('SaleMan', 'Sale Man', true), d.saleMan || [], {});
            H.drawGrid('grdFCY', shareCols(null, null), d.fcy || [], {});
            H.drawGrid('grdEtaDestination', ETA_COLS, S.eta, {});
            H.focusGrid('grdfrm');
        }).catch(function (e) { box(e.message); });
    }
    /** button1_Click */
    function show(btn) { return H.busy(btn, function () { return gridBind(0); }); }
    /** btnNew_Click -> Reset(): From = year start, both unchecked, combos blank, grids cleared (grdSaleMan/grdFCY are not cleared on the desktop). */
    function btnNew() {
        H.focus('datFromDate');
        H.setText('datFromDate', S.yearStart); $id('datFromDateChk').checked = false; $id('datToDateChk').checked = false;
        ['CmbInvoiceNo', 'CmbCustomerName', 'cmbContractNo', 'cmbDeliveryTerm', 'cmbPaymentTerm', 'cmbDestinationPort'].forEach(function (id) { H.setVal(id, '0'); });
        S.rows = []; S.eta = [];
        ['grdfrm', 'grdCustomer', 'grdEtaDestination', 'grdBank'].forEach(H.clearGrid);
        H.show('grdfrmEmpty', false);
    }
    /** btnRefresh_Click: the six combos again. */
    function btnRefresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindAll).catch(function (e) { box(e.message); }); }); }
    /** BtnPrint_Click: "Not Record Found For Display" without rows, else 243 with the same filters (@CompanyAddress/@CompanyName). */
    function btnPrint(btn) {
        if (!S.rows.length) { box('Not Record Found For Display'); return; }
        var f = filters(S.sortNo);
        return H.print('exp-243', {
            customerId: f.customerId, contractId: f.contractId, invoiceId: f.invoiceId,
            dueFrom: f.fromChecked ? f.fromDate : '', dueTo: f.toChecked ? f.toDate : '',
            paymentTermId: f.paymentTermId, deliveryTermId: f.deliveryTermId, destinationPortId: f.destinationPortId,
            skipZero: f.skipZero ? 1 : 0, sortNo: f.sortNo
        }, btn);
    }
    /** grdfrm_LinkClicked: ContractNo -> first id of ContractIds -> frmContractDetailByContractId. */
    function link(key, i) {
        var r = S.rows[i]; if (!r || key !== 'ContractNo') return;
        var ids = H.str(r.ContractIds); var id = ids ? H.netI(ids.split(',')[0]) : 0;
        if (id > 0) H.contractDetail(id, 235);
    }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        H.gridEvents('grdfrm', { select: function (i) { S.cur = i; }, link: link, ctrlSpace: function (i) { link('ContractNo', i); } });
        /* grdEtaDestination_Click: re-bind everything with the row's SortNo */
        H.gridEvents('grdEtaDestination', { select: function (i) { var r = S.eta[i]; if (r) gridBind(H.netI(r.SortNo)); } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, null)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); }
            if (e.ctrlKey && k === 'p') { e.preventDefault(); btnPrint($id('BtnPrint')); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('button1')); }
        });
        load();
    });
    global.ExportRcv = { btnNew: btnNew, btnRefresh: btnRefresh, btnPrint: btnPrint, show: show };
}(window));
