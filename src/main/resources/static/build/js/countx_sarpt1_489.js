/* ============================================================================================
 * Screen 489 DeliveryOrderHistory - Architecture.WinApp.Inventory_Reports.DeliveryOrderHistory (.cs, 940 lines)
 * Page: templates/sale/reports/sarpt1_delivery_order_history.html (route /sale/reports/delivery-order-history).
 *   frmStockWithSupplierHistory_Load :143  Datetypefill, (no request from another form) focus the date type, StatusFill, ComboFill - no Show on load
 *   Datetypefill :418     CommonServices.DateType, BindDDL(ZeroIndex true), Rows[1].Activate() ("This Day", id 1: From = today)
 *   StatusFill :398       1 Not Approved, 2 Approved, 3 All (BindDDLNew, ZeroIndex true), Rows[3] = All
 *   ComboFill :275        InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder (USP_GetDataForDropDownFromDeliveryOrder, org + company): Item -> cmbItem,
 *                         Customer -> cmbSupplier, DeliveryOrderType -> cmbDeliveryType, InvoiceNo -> CmbInvoice (bound only when the delivery type list has rows, as the desktop),
 *                         all ZeroIndex true; the previous selections are kept when they are still in the new lists
 *   cmbdatetype_ValueChanged :445  1 From = today | 2 today - 7 | 3 first of (UTC) month, To = today | 4 1 Jan, To = today | 5 Start_Period (no Show)
 *   btnshow_Click :482 / GridFill :500  Sp_InvDeliveryOrderForApproval_Rpt: customer, invoice, item, the delivery type TEXT only when its id > 0, dates, status
 *                         (Approved true / Not Approved false / All), Reffered -> 1, Not Reffered -> 2, 1st Wt Only -> 1, With 2nd Wt -> 2
 *   GridSetting :600      Id, DocumentTypeId, SaleOrderId, SaleOrderNo, IsApproved, GpId hidden; GpNo link; EntryDate / ModifyDate "dd-MMM-yy hh:mm tt"; widths from
 *                         InventoryConstants; HeaderLines 2; sums + "#,##0.##" / "#,##0.###"; Print button column (40, position 0), FrozenColumns 1
 *   grdHistory_ColumnButtonClick :692  Print -> InvDeliveryOrderSlip(Id, 84) (the approve branch belongs to the hidden IsApproved column and cannot be reached)
 *   grdHistory_LinkClicked :735  GpNo -> OutwardGatePassWithWb(GpId).  btnPrint_Click :752  no rows -> "Not Record Found For Display"; 263-InvDeliveryOrderForApproval.rpt
 *   Reset :191 (item / customer / date type text cleared, Status Rows[0], focus), btnrefresh :207 (ComboFill), btnshow_Leave :249, KeyDown :794, MakeShortCutKeys :857
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/sarpt1/delivery-order-history';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null };
    var lastArgs = null, hasRows = false, seq = 0;
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
        ['Ctrl+F5', 'For Focus on DateType'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid History'],
        ['Ctrl+ArrowUp', 'For Focus On DateType in Delivery Order History']];

    // ------------------------------------------------------------------ lists
    function datetypefill() { S.fill(el('cmbdatetype'), S.dateTypeRows(false), true); S.activate('cmbdatetype', 1, true); }
    function statusFill() {
        S.fill(el('cmbStatus'), [{ Id: 1, name: 'Not Approved' }, { Id: 2, name: 'Approved' }, { Id: 3, name: 'All' }], true);
        S.activate('cmbStatus', 3, true);
    }
    function bindCombos(data) {
        function rows(k) { return (data && data[k]) || []; }
        S.fill(el('cmbItem'), rows('Item'), true, true);                       // previous selections are kept when still listed
        S.fill(el('cmbSupplier'), rows('Customer'), true, true);
        var types = rows('DeliveryOrderType');
        if (types.length) {
            S.fill(el('cmbDeliveryType'), types, true, true);
            S.fill(el('CmbInvoice'), rows('InvoiceNo'), true, true);          // bound only when the delivery type list has rows
        }
    }
    function comboFill() { return A.getJson(API + '/combos').then(bindCombos); }
    el('cmbdatetype').addEventListener('change', function () {              // cmbdatetype_ValueChanged (no Show)
        S.dateRule(A.toInt(this.value), el('DateFrom'), el('DateTo'), lookup.yearStart);
    });

    // ------------------------------------------------------------------ grid
    var grid = new S.Grid({
        tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, frozen: 1,
        onLink: function (row, col) { if (!col || col.key === 'GpNo') { if (A.toInt(row.GpId) > 0) w.open('/sale/outward-gate-pass?id=' + A.toInt(row.GpId), '_blank'); } },
        onButton: function (row) {                                           // InvDeliveryOrderSlip(Id, 84)
            A.openPdf('/reports/print/262-delivery-order-slip?' + S.qs({ id: A.toInt(row.Id), documentTypeId: 84 })).catch(function (e) { alert(e.message); });
        }
    });
    function cols() {
        return [
            { key: '_Print', caption: 'Print', width: 40, button: 'Print' },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'DocumentTypeId', caption: 'DocumentTypeId', hidden: true },
            { key: 'DocNo', caption: 'DocNo', width: 60 }, { key: 'DocDate', caption: 'DocDate', width: 73, date: 'dd-MMM-yy' },
            { key: 'D.Type', caption: 'D.Type', width: 60 }, { key: 'GpId', caption: 'GpId', hidden: true },
            { key: 'GpNo', caption: 'GpNo', width: 45, link: true }, { key: 'InvoiceNo', caption: 'InvoiceNo', width: 100 },
            { key: 'SaleOrderId', caption: 'SaleOrderId', hidden: true }, { key: 'SaleOrderNo', caption: 'SaleOrderNo', hidden: true },
            { key: 'CustomerName', caption: 'CustomerName', width: 170 }, { key: 'ItemName', caption: 'ItemName', width: 150 }, { key: 'UOM', caption: 'UOM', width: 60 },
            { key: 'ItemQty', caption: 'ItemQty', width: 70, fmt: '#,##0.##', sum: true, num: true },
            { key: 'NetWeight', caption: 'NetWeight', width: 80, fmt: '#,##0.###', sum: true, num: true },
            { key: 'EbUnit', caption: 'EbUnit', width: 70, fmt: '#,##0.###', num: true },
            { key: 'EbTotal', caption: 'EbTotal', width: 70, fmt: '#,##0.###', sum: true, num: true },
            { key: 'GrossWeight', caption: 'GrossWeight', width: 80, fmt: '#,##0.###', sum: true, num: true },
            { key: 'EntryUser', caption: 'EntryUser', width: 80 }, { key: 'EntryDate', caption: 'EntryDate', width: 135, date: 'dd-MMM-yy hh:mm tt' },
            { key: 'ModifyUser', caption: 'ModifyUser', width: 80 }, { key: 'ModifyDate', caption: 'ModifyDate', width: 135, date: 'dd-MMM-yy hh:mm tt' },
            { key: 'IsApproved', caption: 'IsApproved', hidden: true }, { key: 'Remarks', caption: 'Remarks', width: 100 },
            { key: 'Container', caption: 'Container', width: 70 }, { key: 'NoOfAttachments', caption: 'NoOfAttachments', width: 100 }
        ];
    }
    function mapRow(r) {
        var c = A.ci;
        return { Id: c(r, 'Id'), DocumentTypeId: c(r, 'DocumentTypeId'), DocNo: c(r, 'DocNo'), DocDate: c(r, 'DocDate'), 'D.Type': c(r, 'DeliveryOrderType'), GpId: c(r, 'GpId'),
            GpNo: c(r, 'GpNo'), InvoiceNo: c(r, 'InvoiceNo'), SaleOrderId: c(r, 'SaleOrderId'), SaleOrderNo: c(r, 'SaleOrderNo'), CustomerName: c(r, 'CustomerName'),
            ItemName: c(r, 'ItemName'), UOM: c(r, 'PackUOM'), ItemQty: c(r, 'LoadingQty'), NetWeight: c(r, 'LoadingWeight'), EbUnit: c(r, 'EbUnit'),
            EbTotal: c(r, 'TotalPackingWeight'), GrossWeight: c(r, 'GrossWeight'), EntryUser: c(r, 'EntryUserName'), EntryDate: c(r, 'EntryDate'),
            ModifyUser: c(r, 'ModifyUserName'), ModifyDate: c(r, 'ModifyDate'), IsApproved: c(r, 'IsApproved'), Remarks: c(r, 'LoadingRemarks'),
            Container: c(r, 'Container'), NoOfAttachments: c(r, 'NoOfAttachments') };
    }

    // ------------------------------------------------------------------ Show / print
    function radio(name) { var x = d.querySelector('input[name="' + name + '"]:checked'); return x ? x.value : ''; }
    function show() {
        var b = el('show'); if (b.disabled) return;
        var a = { supplierCustomerId: S.selInt('cmbSupplier'), invoiceId: S.selInt('CmbInvoice'), itemId: S.selInt('cmbItem'),
                  deliveryType: S.selInt('cmbDeliveryType') > 0 ? S.selText('cmbDeliveryType') : '', fromDate: el('DateFrom').value, toDate: el('DateTo').value,
                  statusId: S.selInt('cmbStatus'), referred: radio('rdRef'), weight: radio('rdWt') };
        var token = ++seq;
        A.busy(b, true);
        A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            lastArgs = a; hasRows = !!(rows && rows.length);
            if (!hasRows) { grid.clear(); return; }
            grid.setData(cols(), rows.map(mapRow));
        }).catch(function (e) { alert(e.message); }).then(function () { if (token === seq) A.busy(b, false); });
    }
    function print() {
        var b = el('print'); if (b.disabled) return;
        if (!hasRows || !lastArgs) { alert('Not Record Found For Display'); return; }
        A.busy(b, true);
        A.openPdf('/sale/reports/sarpt1/print/delivery-order-history?' + S.qs(lastArgs)).catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() {
        ['cmbItem', 'cmbSupplier', 'cmbdatetype'].forEach(function (id) { var s = el(id); s.value = ''; s.dispatchEvent(new Event('change', { bubbles: true })); });
        S.activate('cmbStatus', 0, true);
        S.focus('cmbdatetype');
    }
    function refresh() {
        var b = el('refresh'); if (b.disabled) return;
        A.busy(b, true);
        comboFill().catch(function (e) { alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function closeForm() { w.location.href = '/sale/reports'; }

    el('show').addEventListener('click', show);
    el('reset').addEventListener('click', reset);
    el('refresh').addEventListener('click', refresh);
    el('print').addEventListener('click', print);
    el('shortcuts').addEventListener('click', function () { A.shortcuts(SHORTCUTS); });
    S.closeShortcuts();
    el('show').addEventListener('blur', function (e) { if (e.relatedTarget && !el('show').contains(e.relatedTarget)) { /* btnshow_Leave: the date type takes the focus */ if (el('grid').contains(e.relatedTarget)) S.focus('cmbdatetype'); } });
    S.keys({ s: show, e: closeForm, r: refresh, n: reset, p: print, F5: function () { S.focus('cmbdatetype'); }, ArrowUp: function () { S.focus('cmbdatetype'); },
             ArrowDown: function () { grid.focus(); }, alt: function () { A.shortcuts(SHORTCUTS); } }, closeForm);

    // ------------------------------------------------------------------ start (Load)
    el('DateFrom').value = A.today(); el('DateTo').value = A.today();
    grid.render();
    A.getJson(API + '/lookups').then(function (data) {
        lookup = data || lookup;
        datetypefill();                                                          // Datetypefill
        S.focus('cmbdatetype');
        statusFill();                                                            // StatusFill
        bindCombos(data);                                                        // ComboFill (the lookups call carries the same lists)
    }).catch(function (e) { alert(e.message); });
}(window, document));
