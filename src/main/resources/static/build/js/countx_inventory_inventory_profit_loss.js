/* frmInventoryProfitLoss (Inventory_Stocks_Report/frmInventoryProfitLoss.cs) - ScreenDefinition 289 "Inventory Stock Evaluation Profit & Loss (Not Use)".
   Line refs ":NNN" are that file. API: /inventory/reports/inventory-profit-loss/api. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num, API = '/inventory/reports/inventory-profit-loss/api';

    /* ReportTypeFill :267 - Id values repeat, so options carry the row index */
    var REPORT_TYPES = [
        ['ItemStockSummary', 'Item Stock Summary'],
        ['ItemandWarehouseStockSummary', 'Warehouse and Item Stock Summary'],
        ['ItemandWarehouseStockSummary', 'Item and WareHouse Stock Summary'],
        ['ItemandCropYearStockSummary', 'Crop Year and Item Stock Summary'],
        ['ItemandCropYearandWarehouseStockSummary', 'Crop Year, Warehouse and Item Stock Summary'],
        ['JobLotandItemStockSummary', 'JobLot and Item'],
        ['WarehouseandJoblotandItemStockSummary', 'Warehouse, Joblot and Item Stock Summary']
    ];
    /* StockComboFill :137 - combo id, ActivityType, placeholder (BindDDLNew) */
    var COMBOS = [['cmbMainType', 'ParentCategories', 'Parent Categories'], ['cmbItemType', 'ItemTypes', 'Item Type'], ['cmbCategory', 'ItemCategories', 'Item Categories'],
        ['CmbClassGroup', 'ItemClassGroup', 'ItemClassGroup'], ['cmbItem', 'Items', 'Item Name'], ['cmbJobLot', 'JobLot', 'JobLot'], ['cmbCropYear', 'CropYear', 'CropYear'], ['cmbWareHouse', 'Warehouse', 'WarehouseName']];
    /* GridBind :325-347 - column settings; other columns come from the procedure as they are (RetrieveStructure) */
    var FMT = { SaleQty: '0', SaleWeight: '0', SaleAmount: '0', CgsAmount: '0', TradeProfit: 'pl' };
    var SUM = { SaleQty: 1, SaleWeight: 1, SaleAmount: 1, CgsAmount: 1, TradeProfit: 1 };
    var WIDTH = { ItemName: 300, AccountTitle: 300 };

    var st = { init: null, rows: [], cols: [], groups: [], filters: {}, sel: -1 };

    function fill(id, rows, blank) {
        var el = $id(id), keep = el.value; el.innerHTML = ''; el.appendChild(new Option(blank || '', ''));
        rows.forEach(function (r) { el.appendChild(new Option(String(r.name == null ? '' : r.name), String(r.Id))); });
        el.value = keep; if (el.value !== keep) el.value = '';
    }
    function selText(id) { var o = $id(id).selectedOptions[0]; return o && o.value !== '' ? o.textContent : ''; }
    function reportType() { var v = $id('cmbReportType').value; return v === '' ? null : { value: REPORT_TYPES[+v][0], text: REPORT_TYPES[+v][1] }; }

    /* StockComboFill :137 - one combo per ActivityType; a type with no rows leaves its combo empty */
    function StockComboFill(rows) {
        COMBOS.forEach(function (c) {
            var list = (rows || []).filter(function (r) { return String(SC.ci(r, 'ActivityType')) === c[1]; }).map(function (r) { return { Id: SC.ci(r, 'Id'), name: SC.ci(r, 'name') }; });
            fill(c[0], list);
        });
    }
    /* ClassGroupNameFill :233 - rebinds CmbClassGroup with ItemCategory.GetItemClassGroup when it has rows */
    function ClassGroupNameFill(rows) { if (rows && rows.length) fill('CmbClassGroup', rows.map(function (r) { return { Id: SC.ci(r, 'Id'), name: SC.ci(r, 'ClassGroupName') }; })); }
    /* ItemStockAcFill :250 */
    function ItemStockAcFill(rows) { if (rows && rows.length) fill('cmbStockAccounts', rows.map(function (r) { return { Id: SC.ci(r, 'Id'), name: SC.ci(r, 'AccountTitle') }; })); }
    function ReportTypeFill() { var el = $id('cmbReportType'); el.innerHTML = '<option value=""></option>'; REPORT_TYPES.forEach(function (r, i) { el.appendChild(new Option(r[1], String(i))); }); }

    function params() {
        var rt = reportType(), itemWise = $id('ChkItemWiseStock').checked;
        return { fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value, activity: rt ? rt.value : '',
            itemTypeId: num($id('cmbItemType').value), itemCategoryId: num($id('cmbCategory').value), itemId: num($id('cmbItem').value), warehouseId: num($id('cmbWareHouse').value),
            jobLotId: num($id('cmbJobLot').value), purchaseGLAC: num($id('cmbStockAccounts').value), inventoryParentCategories: num($id('cmbMainType').value),
            itemClassGroupId: num($id('CmbClassGroup').value), stockUOM: itemWise ? 0 : 1, packingTypeId: itemWise ? 0 : 1, cropYear: selText('cmbCropYear') };
    }

    /* GridBind :289 */
    function GridBind() {
        var rt = reportType();
        return SC.postJson(API + '/show', params()).then(function (rows) {
            st.rows = rows || []; st.filters = {}; st.sel = -1; st.groups = [];
            if (!st.rows.length) { st.cols = []; render(); return; }   // grdfrm.ClearStructure()
            var hidden = { ItemId: 1 }, t = rt ? rt.text : '', v = rt ? rt.value : '';
            if (t === 'Warehouse and Item Stock Summary') { st.groups = ['WareHouseCode']; }
            else if (t === 'Item and WareHouse Stock Summary') { st.groups = ['ItemName']; }
            else if (t === 'Crop Year and Item Stock Summary') { st.groups = ['CropBatch']; }
            else if (t === 'Crop Year, Warehouse and Item Stock Summary') { st.groups = ['CropBatch', 'WareHouseCode']; }
            else if (v === 'WarehouseandJoblotandItemStockSummary') { st.groups = ['WareHouseCode', 'JobLot']; }
            else if (v === 'JobLotandItemStockSummary') { st.groups = ['JobLot']; }
            st.groups.forEach(function (g) { hidden[g] = 1; });
            st.cols = Object.keys(st.rows[0]).filter(function (c) { return !hidden[c]; });
            render();
        });
    }

    function roundAway(x) { var s = x < 0 ? -1 : 1; return s * Math.round(Math.abs(x) + 1e-9); }
    function fmtNum(v, kind) {
        v = Number(v) || 0;
        var r = roundAway(v), a = Math.abs(r).toLocaleString('en-US', { maximumFractionDigits: 0 });
        if (kind === 'pl') return r === 0 ? '0' : r < 0 ? '(' + a + ')' : a;   // "#,#;(#,#);0"
        return (r < 0 ? '-' : '') + a;                                            // "#,##0"
    }
    function cellText(r, c) { return FMT[c] ? fmtNum(r[c], FMT[c]) : String(r[c] == null ? '' : r[c]); }
    function filteredRows() {
        var keys = Object.keys(st.filters).filter(function (k) { return st.filters[k]; }), out = [];
        st.rows.forEach(function (r, i) { if (keys.every(function (k) { return cellText(r, k).toLowerCase().indexOf(st.filters[k]) >= 0; })) out.push(i); });
        return out;
    }
    function render() {
        var t = $id('grdfrm');
        if (!st.rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; $id('lblNavigator').textContent = ''; return; }
        var cols = st.cols;
        t.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { var w = WIDTH[c] || (FMT[c] ? 95 : 110); return '<th data-col="' + c + '" style="min-width:' + w + 'px;width:' + w + 'px">' + esc(c) + '</th>'; }).join('') + '</tr>'
            + '<tr class="flt">' + cols.map(function (c) { return '<td><input type="text" data-flt="' + c + '" value="' + esc(st.filters[c] || '') + '"></td>'; }).join('') + '</tr>';
        renderBody();
    }
    /* groups with GroupTotals Always (:354) and the TotalRow */
    function renderBody() {
        var t = $id('grdfrm'), cols = st.cols, idx = filteredRows(), groups = st.groups, html = '', total = {};
        var gt = groups.map(function () { return {}; }), prev = groups.map(function () { return null; });
        function sums(obj) { return cols.map(function (c) { return SUM[c] ? '<td class="n">' + esc(fmtNum(obj[c] || 0, FMT[c])) + '</td>' : '<td></td>'; }).join(''); }
        function closeFrom(level) { for (var L = groups.length - 1; L >= level; L--) { if (prev[L] !== null) html += '<tr class="gtot">' + sums(gt[L]) + '</tr>'; gt[L] = {}; prev[L] = null; } }
        idx.forEach(function (i) {
            var r = st.rows[i];
            for (var L = 0; L < groups.length; L++) {
                var key = String(r[groups[L]] == null ? '' : r[groups[L]]);
                if (prev[L] !== key) {
                    closeFrom(L);
                    for (var M = L; M < groups.length; M++) {
                        var k2 = String(r[groups[M]] == null ? '' : r[groups[M]]);
                        html += '<tr class="grp"><td colspan="' + cols.length + '" style="padding-left:' + (4 + M * 16) + 'px">' + esc(groups[M] + ': ' + k2) + '</td></tr>';
                        prev[M] = k2;
                    }
                    break;
                }
            }
            html += '<tr data-i="' + i + '"' + (i === st.sel ? ' class="sel"' : '') + '>' + cols.map(function (c) {
                if (c === 'ItemName') return '<td><a data-link="ItemName" data-i="' + i + '">' + esc(cellText(r, c)) + '</a></td>';   // ColumnType Link :348
                return FMT[c] ? '<td class="n">' + esc(cellText(r, c)) + '</td>' : '<td>' + esc(cellText(r, c)) + '</td>';
            }).join('') + '</tr>';
            cols.forEach(function (c) { if (SUM[c]) { var v = Number(r[c]) || 0; total[c] = (total[c] || 0) + v; gt.forEach(function (o) { o[c] = (o[c] || 0) + v; }); } });
        });
        closeFrom(0);
        t.querySelector('tbody').innerHTML = html;
        t.querySelector('tfoot').innerHTML = '<tr>' + sums(total) + '</tr>';
        $id('lblNavigator').textContent = 'Records: ' + idx.length + (idx.length !== st.rows.length ? ' of ' + st.rows.length : '');
    }

    /* grdfrm_LinkClicked :461 - InventoryTransactionReport (CmbItem = ItemId, CmbWareHouse / CmbJobLot when chosen) */
    function linkClicked(a) {
        var r = st.rows[+a.getAttribute('data-i')]; if (!r) return;
        var q = { itemId: num(SC.ci(r, 'ItemId')) };
        if ($id('cmbWareHouse').value) q.warehouseId = num($id('cmbWareHouse').value);
        if ($id('cmbJobLot').value) q.jobLotId = num($id('cmbJobLot').value);
        window.open('/inventory/stock-transactions' + SC.qs(q), '_blank');
    }

    /* btnRefresh_Click :415 */
    function btnRefresh_Click() {
        if (st.init) $id('txtDateFrom').value = st.init.fyStart;
        ['cmbCategory', 'cmbCropYear', 'cmbItem', 'cmbItemType', 'cmbJobLot', 'cmbMainType', 'cmbReportType', 'CmbClassGroup', 'cmbWareHouse'].forEach(function (id) { $id(id).value = ''; });
        $id('txtDateFrom').focus();
        st.rows = []; st.cols = []; render();   // grdfrm.ClearStructure()
    }

    function showClick() { return SC.withBusy($id('btnshow'), GridBind).catch(function (e) { alert(e.message); }); }

    /* frmStockReport_Load :115 */
    function load() {
        SC.getJson(API + '/init').then(function (d) {
            st.init = d;
            $id('txtDateTo').value = SC.today();          // DateTimePicker default = now
            $id('txtDateFrom').value = d.fyStart;         // ActiveYr.Start_Period
            $id('txtDateFrom').focus();
            StockComboFill(d.combos || []); ItemStockAcFill(d.stockAccounts); ClassGroupNameFill(d.classGroups); ReportTypeFill();
            $id('cmbReportType').value = '0';             // Rows[0].Activate() = "Item Stock Summary"
            if (selText('cmbReportType') === 'Item Stock Summary') return GridBind();
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', showClick);
        $id('btnRefresh').addEventListener('click', btnRefresh_Click);
        var grid = $id('grdfrm');
        grid.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (!k) return; st.filters[k] = e.target.value.trim().toLowerCase(); renderBody(); });
        grid.addEventListener('click', function (e) {
            var a = e.target.closest('a[data-link]'); if (a) { e.preventDefault(); linkClicked(a); return; }
            var tr = e.target.closest('tbody tr[data-i]'); if (tr) { st.sel = +tr.getAttribute('data-i'); grid.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); }
        });
        load();
    });
})();
