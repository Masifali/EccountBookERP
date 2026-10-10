/* ============================================================================================
 * Screen 765 BookingReport - Architecture.WinApp.PreBookingAndDelivery.PreBookingOrderRegister (.cs)
 * Page: templates/sale/bk/booking_report.html (route /sale/reports/booking-report).
 *   frmStockWithSupplierHistory_Load :112  Datetypefill, CostCenterFill, AppId 5 -> first cost center + combo disabled, ComboFill(cost center), CmbDateType.Rows[1] ("This Week")
 *   Datetypefill :315 / cmbdatetype_ValueChanged :329   CommonServices.DateType(); 1 From = today, 2 From = today - 7, 3 From = 1st of the month (UTC) To = today,
 *                                          4 From = 1 Jan To = today, 5 From = Start_Period
 *   CostCenterFill :215                    Projects.GetSubCostCenters(Org, Company, UserId, AppId, 0)
 *   ComboFill :250                         GetDataForDropDownFromPreBookingOrder (DocumentTypeIds 129): Customer -> cmbCustomer, Item -> cmbItem; AppId 4 or 6: customer Rows[0]
 *   btnrefresh_Click :153                  ComboFill() with cost center 0 (not the selected one)
 *   btnnew_Click :165 / Reset :136         combos cleared, CmbDateType.Rows[1], focus, grid cleared (the last Show's table stays for Print)
 *   btnshow_Click :366 / GridFill :378     PreBookingOrderSlipAndRegister(...); rows -> 25 column grid, none -> ClearStructure
 *   GridSetting :454                       Id, DetailId, ItemId hidden; quantity / weight columns #,##0.### summed; ItemRate rate format; amounts single format summed;
 *                                          Print button column 50 at position 0, FrozenColumns 1, best fit widths, CustomerName and ItemName 200
 *   grdHistory_ColumnButtonClick :520 / grdHistory_KeyDown :685   Print -> CommonServices.PreBookingSlip(Id) (Ctrl+Space on the Print column too)
 *   btnPrint_Click :555                    no rows -> "Not Record Found For Display" else 274_1-PreBookinRegister.rpt over the last Show's table
 *   btnshow_Leave :189                     focus returns to CmbDateType
 *   DeliveryOrderHistory_KeyDown :596      Enter = Tab; Ctrl+P print; Ctrl+S show; Ctrl+E / Esc close; Ctrl+N new; Ctrl+R refresh; Ctrl+F5 / Up date type; Ctrl+Down grid; Ctrl+Alt shortcuts
 * ============================================================================================ */
(function (w, d) {
    'use strict';
    var A = w.AcRpt1, S = w.SaRpt1, el = A.el;
    var API = '/api/sale/bk/booking-report';
    var lookup = { amountDecimals: 0, rateDecimals: 0, yearStart: null, appId: 0, costCenters: [] };
    var last = { args: null, rows: 0 };
    var q3 = '#,##0.###';
    var grid = new S.Grid({ tableId: 'results', gridId: 'grid', navId: 'nav', navTextId: 'navText', headerLines: 2, autosize: true, frozen: 1, noFilter: true,
        onButton: function (row, col) { if (col.button === 'Print') printSlip(row); } });

    function cols() {
        var D = 'dd-MMM-yy', single = S.fmtSingle(lookup.amountDecimals), rate = S.fmtRate(lookup.rateDecimals);
        function qty(k) { return { key: k, caption: k, fmt: q3, sum: true, num: true }; }
        function amt(k) { return { key: k, caption: k, fmt: single, sum: true, num: true }; }
        return [{ key: '_Print', caption: 'Print', width: 50, button: 'Print' },
            { key: 'Id', caption: 'Id', hidden: true }, { key: 'DetailId', caption: 'DetailId', hidden: true }, { key: 'DocNo', caption: 'DocNo', align: 'r' },
            { key: 'DocDate', caption: 'DocDate', date: D }, { key: 'CustomerName', caption: 'CustomerName', width: 200 }, { key: 'ItemId', caption: 'ItemId', hidden: true },
            { key: 'ItemName', caption: 'ItemName', width: 200 }, { key: 'PackUom', caption: 'PackUom' }, qty('ItemQty'), qty('DispatchQty'), qty('BalQty'), qty('Weight'),
            qty('DispatchWeight'), qty('BalWeight'), { key: 'ItemRate', caption: 'ItemRate', fmt: rate, num: true }, { key: 'RateUom', caption: 'RateUom' },
            amt('ItemAmount'), amt('DispatchAmount'), amt('BalAmount'), { key: 'EntryUser', caption: 'EntryUser' },
            { key: 'EntryDate', caption: 'EntryDate', date: 'dd-MMM-yyyy hh:mm tt' }, { key: 'ModifyUser', caption: 'ModifyUser' },
            { key: 'ModifyDate', caption: 'ModifyDate', date: 'dd-MMM-yyyy hh:mm tt' }, { key: 'DetailRemarks', caption: 'DetailRemarks' }, { key: 'RemarksHeader', caption: 'RemarksHeader' }];
    }

    // ------------------------------------------------------------------ Load
    function costCenterFill() { S.fill(el('CmbCostCenter'), lookup.costCenters, false); }
    function comboFill(costCenterId) {
        return A.getJson(API + '/combos?' + S.qs({ costCenterId: costCenterId || 0 })).then(function (data) {
            if (data.customers && data.customers.length) {
                S.fill(el('cmbCustomer'), data.customers, false);
                if (lookup.appId === 4 || lookup.appId === 6) S.activate('cmbCustomer', 0, false);
            }
            if (data.items && data.items.length) S.fill(el('cmbItem'), data.items, false);
        }).catch(function (e) { w.alert(e.message); });
    }
    function dateRule() {
        var t = S.selInt('CmbDateType');
        if (t) S.dateRule(t, el('DateFrom'), el('DateTo'), lookup.yearStart, {});
    }
    function load() {
        el('DateTo').value = A.today();
        return A.getJson(API + '/lookups').then(function (data) {
            lookup = data || lookup;
            S.fill(el('CmbDateType'), S.dateTypeRows(false), false);
            costCenterFill();
            if (lookup.appId === 5 && lookup.costCenters.length > 0) {
                S.activate('CmbCostCenter', 0, false);
                var c = S.wrapOf('CmbCostCenter'); if (c) c.classList.add('dis');
                el('CmbCostCenter').disabled = true;
            }
            return comboFill(S.selInt('CmbCostCenter'));
        }).then(function () {
            S.focus('CmbDateType');
            S.activate('CmbDateType', 1, false);
        }).catch(function (e) { w.alert(e.message); });
    }

    // ------------------------------------------------------------------ Show / Reset / Print
    function args() {
        return { fromDate: el('DateFrom').value, toDate: el('DateTo').value, supplierCustomerId: S.selInt('cmbCustomer'), itemId: S.selInt('cmbItem'),
                 referred: el('RadReffered').checked ? '1' : el('RadNotReffered').checked ? '2' : '', costCenterId: S.selInt('CmbCostCenter') };
    }
    var seq = 0;
    function gridFill() {
        var a = args(), token = ++seq, b = el('btnshow');
        last = { args: a, rows: 0 };                                   // dtGrid.Rows.Clear() happens first on the desktop
        A.busy(b, true);
        return A.getJson(API + '/rows?' + S.qs(a)).then(function (rows) {
            if (token !== seq) return;
            last.rows = rows ? rows.length : 0;
            if (last.rows > 0) grid.setData(cols(), rows); else grid.clear();
        }).catch(function (e) { w.alert(e.message); }).then(function () { A.busy(b, false); });
    }
    function reset() {
        S.setIndex('CmbDateType', 0); S.setIndex('cmbCustomer', 0); S.setIndex('cmbItem', 0);
        S.activate('CmbDateType', 1, false);
        S.focus('CmbDateType');
        grid.clear();
    }
    function btnPrint() {
        if (!last.args || !last.rows) { w.alert('Not Record Found For Display'); return; }
        A.openPdf('/sale/bk/print/prebooking-register?' + S.qs(last.args));
    }
    function printSlip(row) {
        var id = A.toInt(A.ci(row, 'Id'));
        if (id === 0) { w.alert('No Record Found For Display'); return; }
        A.openPdf('/sale/bk/print/prebooking-slip?id=' + id);
    }
    function shortcutKeys() {
        A.shortcuts([['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on DateType'],
            ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid History'], ['Ctrl+ArrowUp', 'For Focus On DateType in Delivery Order History']]);
    }
    function closeForm() { if (w.history.length > 1) w.history.back(); else w.location.href = '/dashboard'; }

    // grdHistory_KeyDown: Ctrl+Space on the Print column
    el('grid').addEventListener('keydown', function (e) {
        if (e.ctrlKey && (e.code === 'Space' || e.key === ' ')) { var r = grid.current(); if (r) { e.preventDefault(); printSlip(r); } }
    });
    el('CmbDateType').addEventListener('change', dateRule);
    var order = ['CmbDateType', 'DateFrom', 'DateTo', 'CmbCostCenter', 'cmbCustomer', 'cmbItem', 'btnshow'];
    d.addEventListener('keydown', function (e) {
        if (e.key !== 'Enter' || e.ctrlKey || e.altKey || A.comboOpen()) return;
        var t = e.target; if (t.tagName === 'BUTTON' || t.closest('#grid')) return;
        var cur = -1;
        order.forEach(function (id, i) { var f = A.focusable(id); if (f === t || (f && f.contains && f.contains(t))) cur = i; });
        if (cur < 0) return;
        e.preventDefault(); S.focus(order[cur + 1] || 'btnshow');
    });
    el('btnshow').addEventListener('blur', function () { setTimeout(function () { var a = d.activeElement; if (!a || a === d.body || !a.closest || !a.closest('.frm')) S.focus('CmbDateType'); }, 0); });
    S.keys({ p: btnPrint, s: gridFill, n: reset, r: function () { return comboFill(0); }, e: closeForm, F5: function () { S.focus('CmbDateType'); },
             ArrowDown: function () { grid.focus(); }, ArrowUp: function () { S.focus('CmbDateType'); }, alt: shortcutKeys }, closeForm);
    el('btnshow').addEventListener('click', gridFill);
    el('btnnew').addEventListener('click', reset);
    el('btnrefresh').addEventListener('click', function () { comboFill(0); });
    el('btnPrint').addEventListener('click', btnPrint);
    el('btnShortCutKeys').addEventListener('click', shortcutKeys);
    S.closeShortcuts();
    load();
}(window, document));
