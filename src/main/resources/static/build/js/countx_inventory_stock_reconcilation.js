/* StockReconcilation (Inventory_Reports/StockReconcilation.cs) - ScreenDefinition 303 "Stock Reconcilation (Not Use)".
   Line refs ":NNN" are that file. API: /inventory/reports/stock-reconcilation/api. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num, API = '/inventory/reports/stock-reconcilation/api';

    /* USP_StockReconcilation_VIEWCGS result columns in procedure order; gridSetting :212 formats and sums */
    var COLS = ['DocDate', 'DocNo', 'DocumentTypeDescription', 'ItemName', 'JobLotDescription', 'InQty', 'InRate', 'InAmount', 'OutQty', 'OutRate', 'OutAmount', 'StockFlag', 'StockInHand', 'StockAmount', 'StockRate'];
    var WIDTH = { ItemName: 300, DocumentTypeDescription: 180 };
    /* FormatString: "#,#" = 0 decimals, "#,#.####" = up to 4, "#,#.###" = up to 3, "#,#.##" = up to 2 */
    var DEC = { InQty: 0, InRate: 4, InAmount: 3, OutQty: 0, OutRate: 4, OutAmount: 3, StockInHand: 2, StockAmount: 3, StockRate: 4 };
    var SUM = { InQty: 1, InAmount: 1, OutQty: 1, OutAmount: 1 };

    var st = { rows: [], filters: {}, sel: -1 };

    function fill(id, rows, idKey, nameKey) { var el = $id(id), keep = el.value; el.innerHTML = '<option value=""></option>'; (rows || []).forEach(function (r) { el.appendChild(new Option(String(SC.ci(r, nameKey) == null ? '' : SC.ci(r, nameKey)), String(SC.ci(r, idKey)))); }); el.value = keep; if (el.value !== keep) el.value = ''; }
    function fmt(v, d) { v = Number(v) || 0; var s = v.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: d }); return (v === 0 && d === 0) ? '' : s; }   // "#,#" shows blank for zero
    function cellText(r, c) { var v = SC.ci(r, c); if (c === 'DocDate') return SC.gridDate(v); if (DEC[c] !== undefined) return fmt(v, DEC[c]); return v == null ? '' : String(v); }

    function filteredRows() {
        var keys = Object.keys(st.filters).filter(function (k) { return st.filters[k]; }), out = [];
        st.rows.forEach(function (r, i) { if (keys.every(function (k) { return cellText(r, k).toLowerCase().indexOf(st.filters[k]) >= 0; })) out.push(i); });
        return out;
    }
    function render() {
        var t = $id('grdfrm');
        if (!st.rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; $id('lblNavigator').textContent = ''; return; }   // grdfrm.ClearStructure()
        var h = '<tr>' + COLS.map(function (c) { return '<th data-col="' + c + '"' + (WIDTH[c] ? ' style="min-width:' + WIDTH[c] + 'px;width:' + WIDTH[c] + 'px"' : '') + '>' + esc(c) + '</th>'; }).join('') + '</tr>';
        var hf = '<tr class="flt">' + COLS.map(function (c) { return '<td><input type="text" data-flt="' + c + '" value="' + esc(st.filters[c] || '') + '"></td>'; }).join('') + '</tr>';
        t.querySelector('thead').innerHTML = h + hf;
        renderBody();
    }
    function renderBody() {
        var t = $id('grdfrm'), idx = filteredRows(), html = '', total = {};
        idx.forEach(function (i) {
            var r = st.rows[i];
            html += '<tr data-i="' + i + '"' + (i === st.sel ? ' class="sel"' : '') + '>' + COLS.map(function (c) {
                if (DEC[c] === undefined) return '<td>' + esc(cellText(r, c)) + '</td>';
                var neg = c === 'StockInHand' && (Number(SC.ci(r, c)) || 0) < 0;   // GridEXFormatCondition StockInHand < 0 -> red :248
                return '<td class="n' + (neg ? ' neg' : '') + '">' + esc(cellText(r, c)) + '</td>';
            }).join('') + '</tr>';
            COLS.forEach(function (c) { if (SUM[c]) total[c] = (total[c] || 0) + (Number(SC.ci(r, c)) || 0); });
        });
        t.querySelector('tbody').innerHTML = html;
        t.querySelector('tfoot').innerHTML = '<tr>' + COLS.map(function (c) { return SUM[c] ? '<td class="n">' + esc(fmt(total[c] || 0, DEC[c])) + '</td>' : '<td></td>'; }).join('') + '</tr>';
        $id('lblNavigator').textContent = 'Records: ' + idx.length + (idx.length !== st.rows.length ? ' of ' + st.rows.length : '');
    }

    function params() { return { toDate: $id('ToDate').value, itemId: num($id('CmbItemName').value), jobLotId: num($id('CmbJobLot').value) }; }

    /* GridFill :181 - "Record Not found For Display" comes from the service when no rows */
    function GridFill() {
        return SC.postJson(API + '/show', params()).then(function (rows) { st.rows = rows || []; st.filters = {}; st.sel = -1; render(); })
            .catch(function (e) { st.rows = []; render(); alert(e.message); });
    }
    /* toolStripButton1_Click :287 - only when an item is chosen (ActiveRow != null) */
    function Reconcile() {
        if (!$id('CmbItemName').value) return Promise.resolve();
        return SC.postJson(API + '/reconcile', params()).then(function (res) { alert((res && res.message) || 'Record Update'); }).catch(function (e) { alert(e.message); });
    }
    /* Reset :99 (btnRefresh_Click :282) */
    function Reset() { $id('ToDate').focus(); $id('ToDate').value = SC.today(); $id('CmbItemName').value = ''; st.rows = []; render(); }

    /* frmGatePassReport_Load :114 */
    function load() {
        SC.getJson(API + '/init').then(function (d) {
            $id('ToDate').value = d.today || SC.today();
            if (d.items && d.items.length) fill('CmbItemName', d.items, 'ItemId', 'ItemName');
            if (d.itemCategories && d.itemCategories.length) fill('cmbItemCategory', d.itemCategories, 'ItemCategoryId', 'CategoryDescription');
            if (d.itemTypes && d.itemTypes.length) fill('cmbItemType', d.itemTypes, 'ItemTypeId', 'TypeDescription');
            if (d.jobLots && d.jobLots.length) fill('CmbJobLot', d.jobLots, 'Id', 'JobLotDescription');
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', function () { SC.withBusy($id('btnshow'), GridFill); });
        $id('btnRefresh').addEventListener('click', Reset);
        $id('toolStripButton1').addEventListener('click', function () { SC.withBusy($id('toolStripButton1'), Reconcile); });
        var grid = $id('grdfrm');
        grid.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (!k) return; st.filters[k] = e.target.value.trim().toLowerCase(); renderBody(); });
        grid.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr[data-i]'); if (tr) { st.sel = +tr.getAttribute('data-i'); grid.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); }
        });
        load();
    });
})();
