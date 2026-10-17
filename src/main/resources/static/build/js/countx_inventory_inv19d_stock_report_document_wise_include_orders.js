/* StockReportWithDocumentWiseIncludeOrders (Pcc.Reports/StockReportWithDocumentWiseIncludeOrders.cs) - ScreenDefinition 843
   "Stock Report With Document Wise Include Orders". Line refs ":NNN" are that file. API: /inventory/reports/stock-report-document-wise-include-orders/api. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num, API = '/inventory/reports/stock-report-document-wise-include-orders/api';

    /* gf :264-284 - grid shape; Gridsetting :317 hides BalQty, QtyIn, QtyOut; HeaderLines 3; widths Constants.InventoryConstants (ItemName 150, Variant 100, QtyConstant 70) */
    var COLS = ['ItemName', 'Variant', 'Op_Qty', 'QtyIn', 'QtyOut', 'BalQty', 'SaleOrderQty', 'GdnQtyOverAll', 'GdnQtyAgainstOrder', 'GrnQty', 'ProInQty', 'ProOutQty', 'ConversionInQty', 'ConversionOutQty',
        'PurQty', 'PurReturnQty', 'SaleQty', 'SaleReturnQty', 'BalQtyCalculate', 'BalSOQtyByGdn'];
    var HIDDEN = { BalQty: 1, QtyIn: 1, QtyOut: 1 };
    var CAPTION = { ItemName: 'Item Name', Variant: 'Variant', Op_Qty: 'Op Qty', QtyIn: 'Qty In', QtyOut: 'Qty Out', BalQty: 'Bal Qty', SaleOrderQty: 'Sale Order Qty', GdnQtyOverAll: 'Gdn Qty Over All',
        GdnQtyAgainstOrder: 'Gdn Qty Against Order', GrnQty: 'Grn Qty', ProInQty: 'Pro In Qty', ProOutQty: 'Pro Out Qty', ConversionInQty: 'Conversion In Qty', ConversionOutQty: 'Conversion Out Qty',
        PurQty: 'Pur Qty', PurReturnQty: 'Pur Return Qty', SaleQty: 'Sale Qty', SaleReturnQty: 'Sale Return Qty', BalQtyCalculate: 'Bal Qty Calculate', BalSOQtyByGdn: 'Bal SO Qty By Gdn' };
    var WIDTH = { ItemName: 150, Variant: 100, ConversionInQty: 80, ConversionOutQty: 80 };
    /* ConfigureNumericalColumn :365 - every double column (none contains "Amount"/"Rate"): Sum, right aligned, "#,##0.##".
       PurReturnQty is typed string in gf (:280) so the desktop neither sums nor formats it. */
    var NUMERIC = { Op_Qty: 1, QtyIn: 1, QtyOut: 1, BalQty: 1, SaleOrderQty: 1, GdnQtyOverAll: 1, GdnQtyAgainstOrder: 1, GrnQty: 1, ProInQty: 1, ProOutQty: 1, ConversionInQty: 1, ConversionOutQty: 1,
        PurQty: 1, SaleQty: 1, SaleReturnQty: 1, BalQtyCalculate: 1, BalSOQtyByGdn: 1 };
    var RPT = '1886_01_StockReportWithDocumentWiseIncludeOrders.rpt';   // PrintRegister :571
    /* MakeShortCutKeys :500 */
    var SHORTCUTS = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl + P', 'For print'], ['Ctrl+F5', 'For Focus On DateType '],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On DateType in Filter']];

    var st = { init: null, dt: [], rows: [], filters: {}, sel: -1 };

    function fill(id, rows, blank) { var el = $id(id), keep = el.value; el.innerHTML = ''; el.appendChild(new Option(blank || '', '')); rows.forEach(function (r) { el.appendChild(new Option(String(r.name), String(r.Id))); }); el.value = keep; if (el.value !== keep) el.value = ''; }

    /* StockComboFill :134 - ParentCategories / ItemTypes / ItemCategories / Items from [pcc].[USP_StockDocumentWise_DropDownLists] */
    function StockComboFill(combos) {
        var by = { ParentCategories: [], ItemTypes: [], ItemCategories: [], Items: [] };
        (combos || []).forEach(function (r) { var t = String(SC.ci(r, 'ActivityType')); if (by[t]) by[t].push({ Id: SC.ci(r, 'Id'), name: String(SC.ci(r, 'name') == null ? '' : SC.ci(r, 'name')) }); });
        if (by.ParentCategories.length) fill('cmbMainType', by.ParentCategories);
        if (by.ItemTypes.length) fill('cmbItemType', by.ItemTypes);
        if (by.ItemCategories.length) fill('cmbCategory', by.ItemCategories);
        if (by.Items.length) fill('cmbItemName', by.Items);
    }
    /* Datetypefill :119 - ZeroIndex:true then Rows[2].Activate() = Id 2 "This Week" */
    function Datetypefill() {
        fill('cmbDateType', (st.init.dateTypes || []).map(function (d) { return { Id: SC.ci(d, 'Id'), name: SC.ci(d, 'Parameters') }; }), '...Select Any Value...');
        $id('cmbDateType').value = '2'; cmbDateType_ValueChanged();
    }
    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    /* cmbDateType_ValueChanged :209 */
    function cmbDateType_ValueChanged() {
        var v = num($id('cmbDateType').value), today = new Date();
        if (v === 1) $id('txtDateFrom').value = iso(today);
        else if (v === 2) { var d = new Date(today); d.setDate(d.getDate() - 7); $id('txtDateFrom').value = iso(d); }
        else if (v === 3) { $id('txtDateFrom').value = iso(new Date(today.getFullYear(), today.getMonth(), 1)); $id('txtDateTo').value = iso(today); }
        else if (v === 4) { $id('txtDateFrom').value = iso(new Date(today.getFullYear(), 0, 1)); $id('txtDateTo').value = iso(today); }
        else if (v === 5 && st.init) $id('txtDateFrom').value = st.init.fyStart;
    }

    /* GridBind :246 */
    function GridBind() {
        var p = { fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value, itemId: num($id('cmbItemName').value), itemTypeId: num($id('cmbItemType').value),
            itemCategoryId: num($id('cmbCategory').value), inventoryParentCategories: num($id('cmbMainType').value) };
        return SC.postJson(API + '/show', p).then(function (rows) {
            st.dt = rows || [];
            if (!st.dt.length) { st.rows = []; render(); return; }   // grdfrm.DataSource = null :296
            st.rows = st.dt.map(function (r) {   // gf.Rows.Add :288
                var g = function (k) { var v = SC.ci(r, k); return v == null ? '' : String(v); }, d = function (k) { return Number(SC.ci(r, k)) || 0; };
                return { ItemName: g('ItemName'), Variant: g('Varient'), Op_Qty: d('Op_Qty'), QtyIn: d('QtyIn'), QtyOut: d('QtyOut'), BalQty: d('BalQty'), SaleOrderQty: d('SaleOrderQty'),
                    GdnQtyOverAll: d('GdnQtyOverAll'), GdnQtyAgainstOrder: d('GdnQtyAgainstOrders'), GrnQty: d('GrnQty'), ProInQty: d('ProInQty'), ProOutQty: d('ProOutQty'),
                    ConversionInQty: d('ConversionInQty'), ConversionOutQty: d('ConversionOutQty'), PurQty: d('PurQty'), PurReturnQty: String(d('PurReturnQty')), SaleQty: d('SaleQty'),
                    SaleReturnQty: d('SaleReturnQty'), BalQtyCalculate: d('BalQtyCalculate'), BalSOQtyByGdn: d('BalSOQtyByGdn') };
            });
            st.filters = {}; st.sel = -1;
            render();   // Gridsetting :317
        });
    }

    function fmtNum(v) { return (Number(v) || 0).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 2 }); }   // "#,##0.##"
    function visibleColumns() { return COLS.filter(function (c) { return !HIDDEN[c]; }); }
    function cellText(r, c) { return NUMERIC[c] ? fmtNum(r[c]) : String(r[c] == null ? '' : r[c]); }
    function filteredRows() {   // DefaultFilterRowComparison Contains :323
        var keys = Object.keys(st.filters).filter(function (k) { return st.filters[k]; }), out = [];
        st.rows.forEach(function (r, i) { if (keys.every(function (k) { return cellText(r, k).toLowerCase().indexOf(st.filters[k]) >= 0; })) out.push(i); });
        return out;
    }
    function render() {
        var t = $id('grdfrm'), cols = visibleColumns();
        if (!st.rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; $id('lblNavigator').textContent = ''; return; }
        t.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { var w = WIDTH[c] || 70; return '<th data-col="' + c + '" style="min-width:' + w + 'px;width:' + w + 'px">' + esc(CAPTION[c]) + '</th>'; }).join('') + '</tr>'
            + '<tr class="flt">' + cols.map(function (c) { return '<td><input type="text" data-flt="' + c + '" value="' + esc(st.filters[c] || '') + '"></td>'; }).join('') + '</tr>';
        renderBody();
    }
    function renderBody() {
        var t = $id('grdfrm'), cols = visibleColumns(), idx = filteredRows(), html = '', total = {};
        idx.forEach(function (i) {
            var r = st.rows[i];
            html += '<tr data-i="' + i + '"' + (i === st.sel ? ' class="sel"' : '') + '>' + cols.map(function (c) { return (NUMERIC[c] ? '<td class="n">' : '<td>') + esc(cellText(r, c)) + '</td>'; }).join('') + '</tr>';
            cols.forEach(function (c) { if (NUMERIC[c]) total[c] = (total[c] || 0) + (Number(r[c]) || 0); });
        });
        t.querySelector('tbody').innerHTML = html;
        t.querySelector('tfoot').innerHTML = '<tr>' + cols.map(function (c) { return NUMERIC[c] ? '<td class="n">' + esc(fmtNum(total[c] || 0)) + '</td>' : '<td></td>'; }).join('') + '</tr>';
        $id('lblNavigator').textContent = 'Records: ' + idx.length + (idx.length !== st.rows.length ? ' of ' + st.rows.length : '');
    }

    /* PrintRegister :556 - Reporting.ShowReportWithDataTable(dt, "1886_01_StockReportWithDocumentWiseIncludeOrders.rpt") + @CompanyName / @CompanyAddress */
    function PrintRegister() {
        if (!st.dt || !st.dt.length) { alert('Not Record Found For Display'); return; }
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify({ rpt: RPT, title: null, rows: st.dt }) })
            .then(function (r) { if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); }); return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); }); })
            .catch(function (e) { if (w) w.close(); alert(e.message); });
    }

    /* Reset :388 */
    function Reset() { $id('cmbItemName').value = ''; $id('cmbDateType').focus(); if (st.init) Datetypefill(); }
    /* btnRefresh_Click :402 */
    function btnRefresh_Click() { return SC.getJson(API + '/combos').then(StockComboFill).catch(function (e) { alert(e.message); }); }
    function showShortcuts() {
        $id('shortcutRows').innerHTML = SHORTCUTS.map(function (s) { return '<tr><td>' + esc(s[0]) + '</td><td>' + esc(s[1]) + '</td></tr>'; }).join('');
        $id('shortcutModal').classList.add('is-open');
    }

    /* InventoryStockEvaluation_GenrateStocks_Load :101 */
    function load() {
        SC.getJson(API + '/init').then(function (d) {
            st.init = d;
            $id('txtDateFrom').value = SC.today(); $id('txtDateTo').value = SC.today();
            StockComboFill(d.combos); Datetypefill();
            $id('cmbDateType').focus();
        }).catch(function (e) { alert(e.message); });
    }

    function showClick() { return SC.withBusy($id('btnshow'), GridBind).catch(function (e) { alert(e.message); }); }
    function closeForm() { if (window.opener || history.length <= 1) window.close(); else history.back(); }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', showClick);
        $id('btnNew').addEventListener('click', Reset);
        $id('btnRefresh').addEventListener('click', function () { SC.withBusy($id('btnRefresh'), btnRefresh_Click); });
        $id('btnPrint').addEventListener('click', PrintRegister);
        $id('btnshortcutkeys').addEventListener('click', showShortcuts);
        $id('shortcutClose').addEventListener('click', function () { $id('shortcutModal').classList.remove('is-open'); });
        $id('cmbDateType').addEventListener('change', cmbDateType_ValueChanged);
        var grid = $id('grdfrm');
        grid.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (!k) return; st.filters[k] = e.target.value.trim().toLowerCase(); renderBody(); });
        grid.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr[data-i]'); if (tr) { st.sel = +tr.getAttribute('data-i'); grid.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); }
        });
        document.addEventListener('keydown', function (e) {   // RptGenrateStocks_KeyDown :453
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey) { e.preventDefault(); showShortcuts(); return; }
            if (e.ctrlKey && k === 'p') { e.preventDefault(); PrintRegister(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); showClick(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); if ($id('shortcutModal').classList.contains('is-open')) $id('shortcutModal').classList.remove('is-open'); else closeForm(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); Reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh_Click(); }
            else if (e.ctrlKey && (k === 'f5' || k === 'arrowup')) { e.preventDefault(); $id('cmbDateType').focus(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); var f = grid.querySelector('input'); if (f) f.focus(); else grid.focus(); }
        });
        load();
    });
})();
