/* ============================================================================================
 * Screen 865 DeliveryScheduleCustomer - Architecture.WinApp.Sale.frmDeliveryScheduleCustomer (.cs)
 * Page: templates/sale/bk/delivery_schedule_customer.html (route /sale/delivery-schedule-customer).
 *   LoadInvoices_Load :99          FromDate = ActiveYr.Start_Period (ToDate keeps the DateTimePicker default = today), CombosFill, PendingOrderLoad
 *   CombosFill :117                GetDataForDropDownFromDeliveryScheduleCustomer: Activity Customer -> CmbSupplier ("Party"), Item -> CmbItem (ZeroIndex false)
 *   btngrnlod_Click :158 / PendingOrderLoad :170
 *                                  GetPendingDeliveryScheduleCustomerDetailForDeliveryOrder(FromDate, ToDate, CmbSupplier, CmbItem); when no rows come back the method
 *                                  RETURNS and the grid keeps whatever it showed (same here). txtDocNoFrom / txtDocNoTo are never read by the form.
 *   grdSettings :251               hidden Id, deliveryScheduleDetailId, SupplierCustomerId, SaleOrderId, SaleOrderDetailId, ItemId, ItemRate; widths DateConstant 73,
 *                                  DocNo 60, CustomerName 170, RemarksHeader 100, ItemName 150, PackUom 60, ItemQty 70, JobLot 80, PackingType 115, NetWeight 80;
 *                                  Select column (position 0, 30 wide, header selector), FrozenColumns 1
 *   btnReset_Click :291 (New)      FromDate = Start_Period, doc numbers cleared, customer text cleared, PendingOrderLoad (CmbItem is NOT cleared)
 *   btnRefresh_Click :308          CombosFill only
 *   btnLoadOnInvoice_Click_1 :320  checked rows -> dtLoader, Hide(); none -> "Check the row first"
 *   LoadPurchaseOrder_KeyDown :383 Enter = Tab, Ctrl+E / Esc = dtLoader null + Hide, Ctrl+Alt = shortcut list, Ctrl+S search, Ctrl+L load, Ctrl+N new,
 *                                  Ctrl+R refresh, Ctrl+F5 / Ctrl+Up FromDate, Ctrl+Down grid
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/bk/delivery-schedule-customer';
    var lookup = { yearStart: null };
    var q3 = '#,##0.###';
    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 3, frozen: 1 });

    function cols() {
        return [{ key: '_sel', caption: 'Select', width: 30, check: true },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'deliveryScheduleDetailId', caption: 'deliveryScheduleDetailId', hidden: true },
            { key: 'DocDate', caption: 'DocDate', date: 'dd-MMM-yy', width: 73 }, { key: 'DocNo', caption: 'DocNo', width: 60, align: 'r' },
            { key: 'SaleOrderId', caption: 'SaleOrderId', hidden: true }, { key: 'SaleOrderDetailId', caption: 'SaleOrderDetailId', hidden: true },
            { key: 'SupplierCustomerId', caption: 'SupplierCustomerId', hidden: true }, { key: 'CustomerName', caption: 'CustomerName', width: 170 },
            { key: 'ItemId', caption: 'ItemId', hidden: true }, { key: 'ItemName', caption: 'ItemName', width: 150 }, { key: 'PackUom', caption: 'PackUom', width: 60 },
            { key: 'JobLot', caption: 'JobLot', width: 80 }, { key: 'PackingType', caption: 'PackingType', width: 115 },
            { key: 'ItemQty', caption: 'ItemQty', width: 70, fmt: q3, sum: true, num: true }, { key: 'NetWeight', caption: 'NetWeight', width: 80, fmt: q3, sum: true, num: true },
            { key: 'ItemRate', caption: 'ItemRate', hidden: true }, { key: 'RemarksHeader', caption: 'RemarksHeader', width: 100 }];
    }

    function fillLists() {
        S.fill(el('CmbSupplier'), lookup.customers, false);
        S.fill(el('CmbItem'), lookup.items, false);
    }
    function combosFill() {
        return A.getJson(API + '/lookups').then(function (data) { lookup = data || lookup; fillLists(); }).catch(function (e) { w.alert(e.message); });
    }
    var seq = 0;
    function pendingOrderLoad() {
        var token = ++seq;
        var a = { fromDate: el('FromDate').value, toDate: el('ToDate').value, supplierCustomerId: S.selInt('CmbSupplier'), itemId: S.selInt('CmbItem') };
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            if (!rows || !rows.length) return;                      // dtHistory.Rows.Count <= 0: return, the grid is left as it was
            grid.setData(cols(), rows);
        }).catch(function (e) { w.alert(e.message); });
    }
    function closeForm() {                                          // dtLoader = null; Hide()
        try { w.sessionStorage.removeItem('saleBkDeliveryScheduleLoader'); } catch (e) { }
        if (w.history.length > 1) w.history.back(); else w.location.href = '/dashboard';
    }
    function load() {                                              // btnLoadOnInvoice_Click_1
        var rows = grid.hasData() ? grid.checkedRows() : [];
        if (!rows.length) { w.alert('Check the row first'); return; }
        var out = rows.map(function (r) { var o = {}; Object.keys(r).forEach(function (k) { if (k.charAt(0) !== '_') o[k] = r[k]; }); return o; });
        try { w.sessionStorage.setItem('saleBkDeliveryScheduleLoader', JSON.stringify(out)); } catch (e) { }
        closeForm();                                                // Hide()
    }
    function reset() {                                              // btnReset_Click
        el('FromDate').focus();
        if (lookup.yearStart) el('FromDate').value = lookup.yearStart;
        el('txtDocNoFrom').value = ''; el('txtDocNoTo').value = '';
        S.setIndex('CmbSupplier', 0);
        return pendingOrderLoad();
    }
    function shortcutKeys() {
        A.shortcuts([['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+R', 'For Refresh'], ['Ctrl+L', 'To Press Load Button'], ['Ctrl+S', 'For Search'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]]);
    }

    // Enter = SendKeys {TAB} in the tab order of the designer
    var order = ['FromDate', 'ToDate', 'txtDocNoFrom', 'txtDocNoTo', 'CmbSupplier', 'CmbItem', 'btngrnlod', 'btnLoadOnInvoice'];
    d.addEventListener('keydown', function (e) {
        if (e.key !== 'Enter' || e.ctrlKey || e.altKey || A.comboOpen()) return;
        var t = e.target; if (t.tagName === 'BUTTON' || t.closest('#grid')) return;
        var cur = -1;
        order.forEach(function (id, i) { var f = A.focusable(id); if (f === t || (f && f.contains && f.contains(t))) cur = i; });
        if (cur < 0) return;
        e.preventDefault(); S.focus(order[(cur + 1) % order.length]);
    });
    S.keys({ s: pendingOrderLoad, l: load, n: reset, r: combosFill, F5: function () { el('FromDate').focus(); }, ArrowDown: function () { grid.focus(); },
             ArrowUp: function () { el('FromDate').focus(); }, e: closeForm, alt: shortcutKeys }, closeForm);
    el('btngrnlod').addEventListener('click', pendingOrderLoad);
    el('btnLoadOnInvoice').addEventListener('click', load);
    el('btnNew').addEventListener('click', reset);
    el('btnRefresh').addEventListener('click', combosFill);
    el('btnShortcutKeys').addEventListener('click', shortcutKeys);
    S.closeShortcuts();

    el('ToDate').value = A.today();
    combosFill().then(function () {
        if (lookup.yearStart) el('FromDate').value = lookup.yearStart;
        el('FromDate').focus();
        return pendingOrderLoad();
    });
}(window, document));
