/* StockReportByFifo (Pcc.Reports/StockReportByFifo.cs) - ScreenDefinition 842 "Stock By FiFo".
   Line refs ":NNN" are that file. API: /inventory/reports/stock-by-fifo-pcc/api. */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv19a, $id = SC.$id, num = SC.num, esc = SC.esc, API = '/inventory/reports/stock-by-fifo-pcc/api';
    var F0 = { dec: 0 }, F4 = { dec: 4 };    // "#,##0" (weights; totals "#,##0.##" shown with 2) / "#,#0.####"

    var st = { dtStockSum: [] };

    /* GridSettings :293 - Weight columns "#,##0" (total "#,##0.##"), Qty/Amount "#,#0.####", AvgRate not aggregated. */
    var grid = new X.Grid('grdfrm', [
        { key: 'DocDate', caption: 'Doc Date', width: 80 },
        { key: 'WareHouseName', caption: 'Ware House Name', width: 130 },
        { key: 'ItemName', caption: 'Item Name', width: 170 },
        { key: 'Varient', caption: 'Varient', width: 110 },
        { key: 'QtyIn', caption: 'Qty In', width: 80, num: F4, sum: true },
        { key: 'QtyOut', caption: 'Qty Out', width: 80, num: F4, sum: true },
        { key: 'BalQty', caption: 'Bal Qty', width: 80, num: F4, sum: true },
        { key: 'WeightIn', caption: 'Weight In', width: 90, num: F0, sum: true },
        { key: 'WeightOut', caption: 'Weight Out', width: 90, num: F0, sum: true },
        { key: 'BalWeight', caption: 'Bal Weight', width: 90, num: F0, sum: true },
        { key: 'AmountIn', caption: 'Amount In', width: 100, num: F4, sum: true },
        { key: 'AmountOut', caption: 'Amount Out', width: 100, num: F4, sum: true },
        { key: 'BalAmount', caption: 'Bal Amount', width: 100, num: F4, sum: true },
        { key: 'AvgRate', caption: 'Avg Rate', width: 90, num: F4 }
    ], { navigator: '#lblNavigator' });

    function fill(id, rows, idKey, nameKey) { var el = $id(id), keep = el.value; el.innerHTML = '<option value=""></option>'; rows.forEach(function (r) { el.appendChild(new Option(String(SC.ci(r, nameKey) == null ? '' : SC.ci(r, nameKey)), String(SC.ci(r, idKey)))); }); el.value = keep; if (el.value !== keep) el.value = ''; }

    /* StockComboFill :121 - 'Items' -> cmbItem ("Item Name"), 'JobLot' -> cmbJobLot ("JobLot"), 'Warehouse' -> cmbWareHouse ("WarehouseName"); empty lists leave the combo as is */
    function StockComboFill(rows) {
        var items = [], lots = [], whs = [];
        (rows || []).forEach(function (r) {
            var t = String(SC.ci(r, 'ActivityType'));
            if (t === 'Items') items.push(r); if (t === 'JobLot') lots.push(r); if (t === 'Warehouse') whs.push(r);
        });
        if (items.length) fill('cmbItem', items, 'Id', 'name');
        if (lots.length) fill('cmbJobLot', lots, 'Id', 'name');
        if (whs.length) fill('cmbWareHouse', whs, 'Id', 'name');
    }

    /* cmbItem_Leave :181 / bindvarientunit :200 - no item -> cmbPackUom text "0"; no varients -> empty + DataSource null */
    function cmbItem_Leave() {
        var itemId = num($id('cmbItem').value), el = $id('cmbPackUom');
        if (itemId <= 0) { el.innerHTML = '<option value="">0</option>'; el.value = ''; return Promise.resolve(); }
        return SC.getJson(API + '/varients?itemId=' + itemId).then(function (rows) {
            if (rows && rows.length) fill('cmbPackUom', rows, 'ItemAttributeVarientId', 'ItemAttribute');
            else { el.innerHTML = '<option value=""></option>'; el.value = ''; }
        }).catch(function (e) { alert(e.message); });
    }

    /* GridBind :242 - dtItemStockSummary :259-276 (Varient = PackUom); empty -> grdfrm.ClearStructure() */
    function GridBind() {
        return SC.postJson(API + '/show', { docDate: $id('txtDateFrom').value, itemId: num($id('cmbItem').value), varientId: num($id('cmbPackUom').value),
            warehouseId: num($id('cmbWareHouse').value), jobLotId: num($id('cmbJobLot').value) }).then(function (rows) {
            st.dtStockSum = rows || [];
            grid.setRows(st.dtStockSum.map(function (r) {
                var g = function (k) { return SC.ci(r, k); }, d = function (k) { return num(g(k)); };
                return { DocDate: SC.gridDate(g('DocDate')), WareHouseName: g('WareHouseName'), ItemName: g('ItemName'), Varient: g('PackUom'),
                    QtyIn: d('QtyIn'), QtyOut: d('QtyOut'), BalQty: d('BalQty'), WeightIn: d('WeightIn'), WeightOut: d('WeightOut'), BalWeight: d('BalWeight'),
                    AmountIn: d('AmountIn'), AmountOut: d('AmountOut'), BalAmount: d('BalAmount'), AvgRate: d('AvgRate') };
            }));
        }).catch(function (e) { alert(e.message); });
    }

    /* btnNew_Click :371 - combos cleared (the date keeps its value), focus date, grid cleared */
    function btnNew_Click() { ['cmbItem', 'cmbJobLot', 'cmbWareHouse'].forEach(function (id) { $id(id).value = ''; }); $id('txtDateFrom').focus(); st.dtStockSum = []; grid.clear(); }
    /* btnRefresh_Click :359 */
    function btnRefresh_Click() { return SC.getJson(API + '/combos').then(StockComboFill).catch(function (e) { alert(e.message); }); }
    /* btnPrint_Click :429 - dtStockSum -> 1887-GetReportByFiFo.rpt ("Record Not Found For Display") */
    function btnPrint_Click() { X.printRows('1887-GetReportByFiFo.rpt', st.dtStockSum, 'Record Not Found For Display'); }
    /* MakeShortCutKeys :498 - ShortCutKeyPopUp */
    function MakeShortCutKeys() {
        var rows = [['Ctrl+S', 'For show data'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'],
            ['Ctrl+F5', 'For Focus on Doc Date'], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowUp', 'For Focus On Doc Date'], ['Ctrl+ArrowDown', 'For Focus On Grid']];
        X.modal('ShortCut Keys', '<div class="fx-grid-host"><table class="win-grid"><thead><tr><th>KeyCombination</th><th>Description</th></tr></thead><tbody>'
            + rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('') + '</tbody></table></div>');
    }

    /* frmStockReport_Load :105 - focus date, StockComboFill (no GridBind on load) */
    function load() {
        SC.getJson(API + '/init').then(function (d) { $id('txtDateFrom').value = d.docDate || SC.today(); StockComboFill(d.combos); $id('txtDateFrom').focus(); }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', function () { SC.withBusy($id('btnshow'), GridBind); });   // btnshow_Click :230
        $id('btnNew').addEventListener('click', btnNew_Click);
        $id('btnRefresh').addEventListener('click', function () { SC.withBusy($id('btnRefresh'), btnRefresh_Click); });
        $id('btnPrint').addEventListener('click', btnPrint_Click);
        $id('btnshortcut').addEventListener('click', MakeShortCutKeys);
        $id('cmbItem').addEventListener('change', cmbItem_Leave);
        $id('cmbItem').addEventListener('blur', cmbItem_Leave);
        document.addEventListener('keydown', function (e) {   // frmStockReportWithValues_KeyDown :451
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey) { e.preventDefault(); MakeShortCutKeys(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); GridBind(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew_Click(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); btnPrint_Click(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh_Click(); }
            else if (e.ctrlKey && k === 'f5') { e.preventDefault(); $id('txtDateFrom').focus(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); var tr = $id('grdfrm').querySelector('tbody tr[data-i]'); if (tr) tr.click(); $id('grdfrm').scrollIntoView(); }
            else if (e.ctrlKey && k === 'arrowup') { e.preventDefault(); $id('txtDateFrom').focus(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.closeForm(); }
        });
        load();
    });
})();
