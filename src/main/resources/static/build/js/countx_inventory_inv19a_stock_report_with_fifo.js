/* StockReportyWithFIFO (RetailStockReports/StockReportyWithFIFO.cs) - ScreenDefinition 296 "Stock Report With FIFO".
   Line refs ":NNN" are that file. API: /inventory/reports/stock-report-with-fifo/api. */
(function () {
    'use strict';
    var SC = window.StoreCommon, X = window.Inv19a, $id = SC.$id, num = SC.num, API = '/inventory/reports/stock-report-with-fifo/api';
    var F3 = { dec: 3 };    // "#,##0.###"

    var st = { init: null, dtStock: [] };

    /* GridSettings :201 - Id hidden; Qty/Weight/Amount summed, AvgRate averaged (AggregateFunction 3); all "#,##0.###". */
    var grid = new X.Grid('grdfrm', [
        { key: 'Id', hidden: true },
        { key: 'DocDate', caption: 'Doc Date', width: 80 },
        { key: 'ItemName', caption: 'Item Name', width: 170 },
        { key: 'Warehouse', caption: 'Warehouse', width: 120 },
        { key: 'PackUom', caption: 'Pack Uom', width: 70 },
        { key: 'RateUom', caption: 'Rate Uom', width: 70 },
        { key: 'QtyIn', caption: 'Qty In', width: 80, num: F3, sum: true },
        { key: 'QtyOut', caption: 'Qty Out', width: 80, num: F3, sum: true },
        { key: 'BalQty', caption: 'Bal Qty', width: 80, num: F3, sum: true },
        { key: 'WeightIn', caption: 'Weight In', width: 90, num: F3, sum: true },
        { key: 'WeightOut', caption: 'Weight Out', width: 90, num: F3, sum: true },
        { key: 'BalWeight', caption: 'Bal Weight', width: 90, num: F3, sum: true },
        { key: 'AmountIn', caption: 'Amount In', width: 100, num: F3, sum: true },
        { key: 'AmountOut', caption: 'Amount Out', width: 100, num: F3, sum: true },
        { key: 'BalAmount', caption: 'Bal Amount', width: 100, num: F3, sum: true },
        { key: 'AvgRate', caption: 'Avg Rate', width: 90, num: F3, sum: 'avg' }
    ], { navigator: '#lblNavigator' });

    function fill(id, rows) { var el = $id(id), keep = el.value; el.innerHTML = '<option value=""></option>'; rows.forEach(function (r) { el.appendChild(new Option(String(r.name), String(r.Id))); }); el.value = keep; if (el.value !== keep) el.value = ''; }

    /* StockComboFill :101 - ActivityType 'Items' -> cmbItem ("Item Name"), 'Warehouse' -> cmbWareHouse ("Warehouse"); a combo with no rows is left as it is */
    function StockComboFill(rows) {
        var items = [], whs = [];
        (rows || []).forEach(function (r) {
            var t = String(SC.ci(r, 'ActivityType')), o = { Id: SC.ci(r, 'Id'), name: SC.ci(r, 'name') == null ? '' : SC.ci(r, 'name') };
            if (t === 'Items') items.push(o); else if (t === 'Warehouse') whs.push(o);
        });
        if (items.length) fill('cmbItem', items);
        if (whs.length) fill('cmbWareHouse', whs);
    }

    /* GridBind :149 - dt reshaped :165-185 (Warehouse = WareHouseName, DocDate short date); empty -> grdfrm.ClearStructure() */
    function GridBind() {
        return SC.postJson(API + '/show', { docDate: $id('txtDocDate').value, itemId: num($id('cmbItem').value), warehouseId: num($id('cmbWareHouse').value) }).then(function (rows) {
            st.dtStock = rows || [];
            grid.setRows(st.dtStock.map(function (r) {
                var g = function (k) { return SC.ci(r, k); }, d = function (k) { return num(g(k)); };
                return { Id: d('Id'), DocDate: SC.gridDate(g('DocDate')), ItemName: g('ItemName'), Warehouse: g('WareHouseName'), PackUom: g('PackUom'), RateUom: g('RateUom'),
                    QtyIn: d('QtyIn'), QtyOut: d('QtyOut'), BalQty: d('BalQty'), WeightIn: d('WeightIn'), WeightOut: d('WeightOut'), BalWeight: d('BalWeight'),
                    AmountIn: d('AmountIn'), AmountOut: d('AmountOut'), BalAmount: d('BalAmount'), AvgRate: d('AvgRate') };
            }));
        }).catch(function (e) { alert(e.message); });
    }

    /* btnRefresh_Click :289 (btnReset) - date = Start_Period, both combos cleared, grid cleared */
    function btnReset_Click() {
        if (st.init) $id('txtDocDate').value = st.init.fyStart;
        $id('cmbItem').value = ''; $id('cmbWareHouse').value = '';
        $id('btnPrint').classList.remove('is-hidden');
        $id('txtDocDate').focus();
        st.dtStock = []; grid.clear();
    }
    /* btnRefreshCombo_Click :323 */
    function btnRefreshCombo_Click() { return SC.getJson(API + '/combos').then(StockComboFill).catch(function (e) { alert(e.message); }); }
    /* btnPrint_Click :366 - dtStock -> 390_01_GetStockByFifo.rpt ("No Record Found For Display") */
    function btnPrint_Click() { X.printRows('390_01_GetStockByFifo.rpt', st.dtStock, 'No Record Found For Display'); }

    /* frmStockReport_Load :83 - txtDocDate = ActiveYr.Start_Period; StockComboFill; GridBind; focus date */
    function load() {
        SC.getJson(API + '/init').then(function (d) {
            st.init = d;
            $id('txtDocDate').value = d.fyStart;
            StockComboFill(d.combos);
            return GridBind().then(function () { $id('txtDocDate').focus(); });
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', function () { SC.withBusy($id('btnshow'), GridBind); });   // btnshow_Click :255
        $id('btnReset').addEventListener('click', btnReset_Click);
        $id('btnRefreshCombo').addEventListener('click', function () { SC.withBusy($id('btnRefreshCombo'), btnRefreshCombo_Click); });
        $id('btnPrint').addEventListener('click', btnPrint_Click);
        document.addEventListener('keydown', function (e) {   // frmStockReport_KeyDown :335
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'p') { e.preventDefault(); btnPrint_Click(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); X.closeForm(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnReset_Click(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); GridBind(); }
        });
        load();
    });
})();
