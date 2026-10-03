/* ============================================================================================
 * countx_export_shipments_document_status.js - frmExportPendingWorksRegister.cs, screen 239
 * "Pending Work Status". Data: /api/export/shipments-document-status (USP_PendingWorkExportRegister,
 * usp_ExportInvoice_StatusUpdate). Print 555 through the seeded key "555-pendingworkexportregister".
 * Link columns open the desktop's slips through their seeded CrystalPrint keys:
 *   Inspection      513-invlabpreproductionslip            (InvLabPreProductionExportLotInspection_Slip(InspectionId))
 *   CROBooking      560-exbooking-info-cro                 (ExImExportShipingLineBooking.PrintSlipandRegister ExImInvoiceId)
 *   BillOfLading    505-eximbilloflading-slip              (ExImBillOfLading_Slip(InvoiceId))
 *   DeliveryOrder   -> /export/delivery-order-report?... (DeliveryOrderHistory dialog: DateTypeId, DOType Export, dates, InvoiceId)
 *   PackingMaterial 475-rptinvgsstoreissuanceheader-slip   (StoreIssuanceHeader_Slip475(0, InvoiceId))
 *   InvoiceStatus   102-anewacrptexportinvoicevoucherslip  (ExportInvoiceVoucher_Slip(ExportVoucherId), Admin only)
 *   Print (Admin)   501-exportsalescontractexportnew       (SaleContractReports501(ContractId), ApprovedFilter All)
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, $id = H.$id, box = H.box;
    var API = '/api/export/shipments-document-status';
    var S = { rows: [], cur: -1, isAdmin: false, yearStart: '', suspend: false };

    function st(v) { var s = H.str(v); return s === 'Pending' || s === 'pending' ? 'red' : s === 'Complete' ? 'dkgreen' : s === 'InProcess' ? 'orange' : ''; }
    function stClose(v) { var s = H.str(v); return s === 'Open' ? 'red' : s === 'In-Transit' ? 'dkgreen' : ''; }
    function cols() {
        var c = [
            { key: 'InvoiceDocumentTypeId', hidden: true }, { key: 'InvoiceId', hidden: true }, { key: 'InvoiceNo', caption: 'Invoice No' },
            { key: 'ContractId', hidden: true }, { key: 'ContractScheduleId', hidden: true }, { key: 'ContractNo', caption: 'Contract No' },
            { key: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'Product', caption: 'Product' },
            { key: 'Fcl', caption: 'Fcl', num: true, sum: true, fmt: 'h3' }, { key: 'MTons', caption: 'M Tons', num: true, sum: true, fmt: 'h3' },
            { key: 'InspectionId', hidden: true }, { key: 'Inspection', caption: 'Inspection', link: true, cls: st },
            { key: 'CROBooking', caption: 'CRO Booking', link: true, cls: st }, { key: 'DeliveryOrder', caption: 'Delivery Order', link: true, cls: st },
            { key: 'Forwarding', caption: 'Forwarding', link: true, cls: st }, { key: 'BillOfLading', caption: 'Bill Of Lading', link: true, cls: st },
            { key: 'DocToParty', caption: 'Document To Party', link: true, cls: st }, { key: 'GoodsDeclaration', caption: 'GD/FI', link: true, cls: st },
            { key: 'PortPayment', hidden: true }, { key: 'PackingMaterial', caption: 'Packing Material', link: true, cls: st },
            { key: 'ShipmentExpenses', caption: 'Shipment Expenses', link: true, cls: st }, { key: 'ShipmentClose', caption: 'Reached At Destination Port', link: true, cls: stClose },
            { key: 'InvoiceStatus', caption: 'Export Voucher', link: true, cls: st }, { key: 'ExportVoucherId', hidden: true },
            { key: 'NoOfAttachments', caption: 'No Of Attachments', num: true, link: true }
        ];
        if (S.isAdmin) {
            c.push({ key: 'CompleteStatus', caption: 'Complete Status', html: function () { return '<button type="button" class="win-btn-cell" data-btn="CompleteStatus">Complete Status</button>'; } });
            c.push({ key: 'OpenStatus', caption: 'Open Status', html: function () { return '<button type="button" class="win-btn-cell" data-btn="OpenStatus">Open Status</button>'; } });
            c.push({ key: 'Print', caption: 'Print', html: function () { return '<button type="button" class="win-btn-cell" data-btn="Print">Print</button>'; } });
        }
        return c;
    }
    function bindCombos(d) {
        if (d.combosError) box(d.combosError);
        H.bind('cmbcontractNo', d.contracts, 'Id', 'name', false);
        H.bind('cmbSupplierName', d.customers, 'Id', 'name', false);
    }
    /** Load: ParameterFill (no blank row, nothing selected -> dates untouched), ComboFill, GridBind. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            S.isAdmin = !!d.isAdmin; S.yearStart = d.yearStart || '';
            S.suspend = true;
            H.bind('cmbperemeter', d.dateTypes, 'Id', 'Parameters', false); $id('cmbperemeter').selectedIndex = -1; H.refreshCombos();
            S.suspend = false;
            H.setText('txtdatefrom', H.today()); H.setText('txtdateto', H.today()); $id('txtdatefromChk').checked = false;
            bindCombos(d);
            $id('cmbcontractNo').selectedIndex = -1; $id('cmbSupplierName').selectedIndex = -1; H.refreshCombos();
            return gridBind();
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function filters() {
        return { contractId: H.netI(H.val('cmbcontractNo')), fromChecked: H.checked('txtdatefromChk'), fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto'),
            supplierCustomerId: H.netI(H.val('cmbSupplierName')), actionId: H.netI(H.radio('pndStatus')) };
    }
    function gridBind() {
        return H.postJson(API + '/show', filters()).then(function (rows) {
            S.rows = rows || []; S.cur = -1;
            H.drawGrid('grdContractRegister', cols(), S.rows, {});
            H.show('grdContractRegisterEmpty', !S.rows.length);
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn, gridBind); }
    /** btnnew_Click: ComboFill + Reset (combos blank, focus From, GridBind). */
    function btnNew(btn) {
        return H.busy(btn, function () {
            return H.getJson(API + '/combos').then(bindCombos).catch(function (e) { box(e.message); }).then(function () {
                $id('cmbcontractNo').selectedIndex = -1; $id('cmbSupplierName').selectedIndex = -1; H.refreshCombos();
                H.focus('txtdatefrom');
                return gridBind();
            });
        });
    }
    function btnRefresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindCombos).catch(function (e) { box(e.message); }); }); }
    /** BtnPrintRegister_Click -> ShowRegister(): "Not Record Found For Display" without rows (@PrintedBy = user). */
    function btnPrint(btn) {
        if (!S.rows.length) { box('Not Record Found For Display'); return; }
        var f = filters();
        return H.print('555-pendingworkexportregister', { exImLcOrderId: f.contractId, fromDate: f.fromChecked ? f.fromDate : '', toDate: f.toDate,
            supplierCustomerId: f.supplierCustomerId, actionId: f.actionId }, btn);
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+N', 'For New'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+F5', 'For Focus on Date Type'], ['Ctrl+S', 'For Showing Data'],
            ['Ctrl+P', 'For Print'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }
    /** grdfrm_ColumnButtonClick (Admin): Print 501 of the contract, CompleteStatus / OpenStatus -> ExportInvoice_StatusUpdate then GridBind. */
    function button(key, i, btn) {
        var r = S.rows[i]; if (!r) return;
        if (key === 'Print') { H.print('501-exportsalescontractexportnew', { id: H.netI(r.ContractId), status: 'All' }, btn); return; }
        var status = key === 'CompleteStatus' ? 'Complete' : 'Open';
        H.busy(btn, function () {
            return H.postJson(API + '/status', { invoiceId: H.netI(r.InvoiceId), status: status }).then(function (d) {
                box((d && d.message) || ('Update ' + status + ' successfully...'));
                return gridBind();
            }).catch(function (e) { box(e.message); });
        });
    }
    /** grdContractRegister_LinkClicked: each status link opens its slip / form when the status is not "Pending". */
    function link(key, i) {
        var r = S.rows[i]; if (!r) return;
        var v = H.str(r[key]);
        var invoiceId = H.netI(r.InvoiceId);
        if (key === 'Inspection' && v !== 'Pending') H.print('513-invlabpreproductionslip', { id: H.netI(r.InspectionId) });
        else if (key === 'CROBooking' && v !== 'Pending') H.print('560-exbooking-info-cro', { exImInvoiceId: invoiceId, id: 0 });
        else if (key === 'BillOfLading' && v !== 'Pending') H.print('505-eximbilloflading-slip', { invoiceId: invoiceId });
        else if (key === 'DeliveryOrder' && v !== 'Pending') {
            global.open('/export/delivery-order-report?dateTypeId=' + H.netI(H.val('cmbperemeter')) + '&doType=Export&fromDate=' + encodeURIComponent(H.val('txtdatefrom'))
                + '&toDate=' + encodeURIComponent(H.val('txtdateto')) + '&invoiceId=' + invoiceId, '_blank');
        }
        else if (key === 'Forwarding' && v !== 'Pending') box('Forwarding history of invoice ' + H.str(r.InvoiceNo) + ' - the ExImForwardingHistory form has no web page yet.');
        else if (key === 'ShipmentExpenses' && v !== 'Pending') box('Service bill history of invoice ' + H.str(r.InvoiceNo) + ' - the frmServiceBillHistory form has no web page yet.');
        else if (key === 'PackingMaterial' && v !== 'Pending') H.print('475-rptinvgsstoreissuanceheader-slip', { id: 0, invoiceId: invoiceId });
        else if (key === 'InvoiceStatus' && v !== 'Pending' && S.isAdmin) H.print('102-anewacrptexportinvoicevoucherslip', { voucherHeadId: H.netI(r.ExportVoucherId) });
        else if (key === 'NoOfAttachments') box('Attachments of invoice ' + invoiceId + ' (document type ' + H.netI(r.InvoiceDocumentTypeId) + ') - the attachment viewer has no web page yet.');
    }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        /* cmbperemeter_ValueChanged */
        $id('cmbperemeter').addEventListener('change', function () { if (!S.suspend) H.dateTypeChanged('cmbperemeter', 'txtdatefrom', 'txtdateto', S.yearStart); });
        /* btnShow_Leave: focus goes back to the date type combo (Tab after Show) */
        $id('btnShow').addEventListener('blur', function () { if (document.activeElement && document.activeElement.id === 'cmbperemeter') H.focus('cmbperemeter'); });
        H.gridEvents('grdContractRegister', { select: function (i) { S.cur = i; }, link: link, button: button,
            ctrlSpace: function (i) { if (S.isAdmin) { var r = S.rows[i]; if (r) H.print('501-exportsalescontractexportnew', { id: H.netI(r.ContractId), status: 'All' }); } } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew($id('btnnew')); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnRefresh')); }
            if (e.ctrlKey && e.key === 'F5') { e.preventDefault(); H.focus('cmbperemeter'); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnShow')); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdContractRegister'); }
            if (e.ctrlKey && k === 'p') { e.preventDefault(); btnPrint($id('BtnPrintRegister')); }
        });
        load();
    });
    global.ExportPnd = { btnNew: btnNew, btnRefresh: btnRefresh, btnPrint: btnPrint, show: show, shortcuts: shortcuts };
}(window));
