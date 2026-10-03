/* ============================================================================================
 * countx_export_delivery_order_report.js - DeliveryOrderHistory.cs (Inventory_Reports), screen 250
 * "Delivery Order Register". Data: /api/export/delivery-order-report (Sp_InvDeliveryOrderForApproval_Rpt).
 * Prints: 263-InvDeliveryOrderForApproval.rpt through key "exp-263-do" (coordinator), row Print =
 * InvDeliveryOrderSlip(Id, 84) through key "exp-262-do84" (coordinator), GpNo link = OutwardGatePassWithWb
 * through the seeded key "258-outwardgatepasswithwbandlabslip" (arg id).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptA, $id = H.$id, box = H.box;
    var API = '/api/export/delivery-order-report';
    var S = { rows: [], cur: -1, yearStart: '', suspend: false, documentTypeId: 0 };
    var COLS = [
        { key: 'Print', caption: 'Print', html: function () { return '<button type="button" class="win-btn-cell" data-btn="Print">Print</button>'; } },
        { key: 'Id', hidden: true }, { key: 'DocumentTypeId', hidden: true }, { key: 'DocNo', caption: 'Doc No', fmt: 'i' }, { key: 'DocDate', caption: 'Doc Date', fmt: 'd' },
        { key: 'DType', caption: 'D.Type' }, { key: 'GpId', hidden: true }, { key: 'GpNo', caption: 'Gp No', link: true, fmt: 'i' }, { key: 'InvoiceNo', caption: 'Invoice No' },
        { key: 'SaleOrderId', hidden: true }, { key: 'SaleOrderNo', hidden: true }, { key: 'CustomerName', caption: 'Customer Name' }, { key: 'ItemName', caption: 'Item Name' },
        { key: 'UOM', caption: 'UOM' }, { key: 'ItemQty', caption: 'Item Qty', num: true, sum: true, fmt: 'o2' }, { key: 'NetWeight', caption: 'Net Weight', num: true, sum: true, fmt: 'o3' },
        { key: 'EbUnit', caption: 'Eb Unit', num: true, fmt: 'o3' }, { key: 'EbTotal', caption: 'Eb Total', num: true, sum: true, fmt: 'o3' },
        { key: 'GrossWeight', caption: 'Gross Weight', num: true, sum: true, fmt: 'o3' }, { key: 'EntryUser', caption: 'Entry User' }, { key: 'EntryDate', caption: 'Entry Date', fmt: 'dt' },
        { key: 'ModifyUser', caption: 'Modify User' }, { key: 'ModifyDate', caption: 'Modify Date', fmt: 'dt' }, { key: 'IsApproved', hidden: true },
        { key: 'Remarks', caption: 'Remarks' }, { key: 'Container', caption: 'Container' }, { key: 'NoOfAttachments', caption: 'No Of Attachments', num: true }
    ];
    function q(name) { var m = new RegExp('[?&]' + name + '=([^&]*)').exec(location.search); return m ? decodeURIComponent(m[1].replace(/\+/g, ' ')) : ''; }
    function bindCombos(d) {
        if (d.combosError) box(d.combosError);
        /* the desktop keeps the current selection when it still exists (BindDDL + the Any() checks) - bind() does the same */
        H.bind('cmbSupplier', d.customers, 'Id', 'name');
        H.bind('cmbItem', d.items, 'Id', 'name');
        H.bind('cmbDeliveryType', d.deliveryTypes, 'Id', 'name');
        if ((d.deliveryTypes || []).length) H.bind('CmbInvoice', d.invoices, 'Id', 'name');   /* quirk: the invoice combo is bound only when delivery types exist */
    }
    function selectByText(id, text) { var s = $id(id); if (!s || !text) return; for (var i = 0; i < s.options.length; i++) if (s.options[i].text === text) { s.value = s.options[i].value; break; } H.refreshCombos(); }
    /** frmStockWithSupplierHistory_Load. */
    function load() {
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            S.yearStart = d.yearStart || '';
            S.suspend = true;
            H.bind('cmbdatetype', d.dateTypes, 'Id', 'Parameters');
            H.setText('DateFrom', H.today()); H.setText('DateTo', H.today());
            var dt = H.netI(q('dateTypeId'));
            if (dt > 0) { H.setVal('cmbdatetype', dt); S.suspend = false; H.dateTypeChanged('cmbdatetype', 'DateFrom', 'DateTo', S.yearStart); S.suspend = true; }
            else { $id('cmbdatetype').selectedIndex = 1; H.refreshCombos(); S.suspend = false; H.dateTypeChanged('cmbdatetype', 'DateFrom', 'DateTo', S.yearStart); S.suspend = true; }  /* Rows[1].Activate() -> "This Day" */
            if (q('fromDate')) { H.setText('DateFrom', H.isoDate(q('fromDate'))); H.setText('DateTo', H.isoDate(q('toDate')) || H.today()); }
            S.suspend = false;
            H.setVal('cmbStatus', '3');
            bindCombos(d);
            if (q('doType')) selectByText('cmbDeliveryType', q('doType'));
            var inv = H.netI(q('invoiceId'));
            if (inv > 0) { H.setVal('CmbInvoice', inv); return gridBind(); }
            H.focus('cmbdatetype');
        }).catch(function (e) { box(e.message || 'Error occurred during database call.'); });
    }
    function filters() {
        return { supplierCustomerId: H.netI(H.val('cmbSupplier')), invoiceId: H.netI(H.val('CmbInvoice')), itemId: H.netI(H.val('cmbItem')),
            deliveryTypeId: H.netI(H.val('cmbDeliveryType')), deliveryTypeText: H.selText('cmbDeliveryType'), fromDate: H.val('DateFrom'), toDate: H.val('DateTo'),
            documentTypeId: S.documentTypeId, statusText: H.selText('cmbStatus'), referred: H.radio('dorRef'), weight: H.radio('dorWeight') };
    }
    function gridBind() {
        return H.postJson(API + '/show', filters()).then(function (rows) {
            S.rows = rows || []; S.cur = -1;
            H.drawGrid('grdHistory', COLS, S.rows, {});
            H.show('grdHistoryEmpty', !S.rows.length);
        }).catch(function (e) { box(e.message); });
    }
    function show(btn) { return H.busy(btn, gridBind); }
    /** btnnew_Click -> Reset(): Item, Customer and Date Type blank, Status row 0 (blank), focus Date Type. */
    function btnNew() {
        H.setVal('cmbItem', '0'); H.setVal('cmbSupplier', '0');
        S.suspend = true; H.setVal('cmbdatetype', '0'); S.suspend = false;
        H.setVal('cmbStatus', '0');
        H.focus('cmbdatetype');
    }
    function btnRefresh(btn) { return H.busy(btn, function () { return H.getJson(API + '/combos').then(bindCombos).catch(function (e) { box(e.message); }); }); }
    /** btnPrint_Click: "Not Record Found For Display" without rows, else 263 with the same filters. */
    function btnPrint(btn) {
        if (!S.rows.length) { box('Not Record Found For Display'); return; }
        var f = filters();
        var isApproved = f.statusText === 'Approved' ? 1 : f.statusText === 'All' ? '' : 0;
        return H.print('exp-263-do', { documentTypeId: f.documentTypeId, supplierCustomerId: f.supplierCustomerId, itemId: f.itemId, exImInvoiceId: f.invoiceId,
            wbWeightStatus: f.weight === 'first' ? 1 : f.weight === 'second' ? 2 : 0, doReferedStatus: f.referred === 'referred' ? 1 : f.referred === 'not' ? 2 : 0,
            fromDate: f.fromDate, toDate: f.toDate, isApproved: isApproved, deliveryOrderType: f.deliveryTypeId > 0 ? f.deliveryTypeText : '' }, btn);
    }
    function shortcuts() {
        H.shortcuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on DateType'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid History'], ['Ctrl+ArrowUp', 'For Focus On DateType in Delivery Order History']]);
    }
    /** grdHistory_ColumnButtonClick Print: InvDeliveryOrderSlip(Id, 84). (IsApproved is a hidden non-button column - its branch is unreachable.) */
    function rowPrint(i, btn) { var r = S.rows[i]; if (!r) return; return H.print('exp-262-do84', { id: H.netI(r.Id) }, btn); }
    /** grdHistory_LinkClicked GpNo: OutwardGatePassWithWb(GpId). */
    function link(key, i) { var r = S.rows[i]; if (!r || key !== 'GpNo') return; H.print('258-outwardgatepasswithwbandlabslip', { id: H.netI(r.GpId) }); }
    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        $id('cmbdatetype').addEventListener('change', function () { if (!S.suspend) H.dateTypeChanged('cmbdatetype', 'DateFrom', 'DateTo', S.yearStart); });
        /* btnshow_Leave: focus returns to the Date Type combo */
        $id('btnshow').addEventListener('blur', function () { setTimeout(function () { if (document.activeElement === document.body) H.focus('cmbdatetype'); }, 0); });
        H.gridEvents('grdHistory', { select: function (i) { S.cur = i; }, link: link, button: function (k, i, b) { if (k === 'Print') rowPrint(i, b); }, ctrlSpace: function (i) { rowPrint(i, null); } });
        document.addEventListener('keydown', function (e) {
            if (H.baseKeys(e, shortcuts)) return;
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'p') { e.preventDefault(); btnPrint($id('btnPrint')); }
            if (e.ctrlKey && k === 's') { e.preventDefault(); show($id('btnshow')); }
            if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
            if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh($id('btnrefresh')); }
            if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); H.focus('cmbdatetype'); }
            if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdHistory'); }
        });
        load();
    });
    global.ExportDor = { btnNew: btnNew, btnRefresh: btnRefresh, btnPrint: btnPrint, show: show, shortcuts: shortcuts };
}(window));
