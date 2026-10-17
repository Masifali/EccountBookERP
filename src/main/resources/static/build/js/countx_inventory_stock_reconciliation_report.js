/* StockReportReconcilation (Inventory_Stocks_Report/StockReportReconcilation.cs) - ScreenDefinition 295 "Stock Report Reconcilation".
   Line refs ":NNN" are that file. API: /inventory/reports/stock-reconciliation-report/api. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num, API = '/inventory/reports/stock-reconciliation-report/api';

    /* DateTypefill :126 - CommonServices.DateType() bound with ZeroIndex true; Rows[3].Activate() = Id 3, btnReset Rows[2] = Id 2 */
    var DATE_TYPES = [[1, 'This Day'], [2, 'This Week'], [3, 'This Month'], [4, 'This Year'], [5, 'Financial Year']];
    /* USP_StockReconcilation result columns, in procedure order (the grid shows the DataTable as is, :307) */
    var COLS = ['ItemId', 'ItemCodeNew', 'ItemName', 'ItemUom', 'PackUom', 'ItemCategoryId', 'CategoryDescription', 'TypeDescription', 'Op_Qty', 'QtyIn', 'QtyOut', 'BalQty',
        'Op_Weight', 'WeightIn', 'WeightOut', 'BalWeight', 'OpeningWeight', 'PurWeight', 'ProInWeight', 'SaleReturnWeight', 'TransferInWeight', 'SC_InWeight', 'Adj_InWeight',
        'ProOutWeight', 'PurReturnWeight', 'SaleWeight', 'TransferOutWeight', 'SC_OutWeight', 'Adj_OutWeight'];
    /* GridBind :318-385 - Sum + "#,##0.###" on these columns */
    var NUMC = {};
    ['Op_Weight', 'WeightIn', 'WeightOut', 'BalWeight', 'OpeningWeight', 'PurWeight', 'PurReturnWeight', 'ProInWeight', 'ProOutWeight', 'SaleWeight', 'SaleReturnWeight',
        'SC_InWeight', 'SC_OutWeight', 'TransferInWeight', 'TransferOutWeight', 'Adj_InWeight', 'Adj_OutWeight'].forEach(function (c) { NUMC[c] = 1; });
    var PLAIN_NUM = { Op_Qty: 1, QtyIn: 1, QtyOut: 1, BalQty: 1, ItemId: 1, ItemUom: 1, ItemCategoryId: 1 };
    var WIDTH = { ItemName: 200 };

    var st = { init: null, combos: [], rows: [], filters: {}, sel: -1, group: '' };

    function fill(id, rows, blank) { var el = $id(id), keep = el.value; el.innerHTML = ''; el.appendChild(new Option(blank || '', '')); rows.forEach(function (r) { el.appendChild(new Option(String(r.name), String(r.Id))); }); el.value = keep; if (el.value !== keep) el.value = ''; }
    function fmt3(v) { v = Number(v) || 0; return v.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 }); }

    function DateTypefill() { var el = $id('cmbDateType'); el.innerHTML = '<option value="">...Select Any Value...</option>'; DATE_TYPES.forEach(function (d) { el.appendChild(new Option(d[1], String(d[0]))); }); el.value = '3'; cmbDateType_ValueChanged(); }

    /* StockComboFill :141 - ParentCategories with Id 1..4, ItemTypes, ItemCategories, Items, Warehouse; a combo is only re-bound when it has rows */
    function StockComboFill() {
        var dt = st.combos; if (!dt.length) return;
        var main = [], types = [], cats = [], items = [], wr = [];
        dt.forEach(function (r) {
            var t = String(SC.ci(r, 'ActivityType')), row = { Id: SC.ci(r, 'Id'), name: SC.ci(r, 'name') == null ? '' : SC.ci(r, 'name') };
            if (t === 'ParentCategories') { var id = num(row.Id); if (id >= 1 && id <= 4) main.push(row); }
            else if (t === 'ItemTypes') types.push(row);
            else if (t === 'ItemCategories') cats.push(row);
            else if (t === 'Items') items.push(row);
            else if (t === 'Warehouse') wr.push(row);
        });
        if (items.length) fill('CmbItem', items);
        if (wr.length) fill('cmbWareHouse', wr);
        if (main.length) { fill('CmbParentCategory', main); $id('CmbParentCategory').value = String(main[0].Id); }   // Rows[0].Activate() :214
        if (types.length) fill('CmbItemType', types);
        if (cats.length) fill('cmbItemCategory', cats);
    }

    /* cmbDateType_ValueChanged :233 */
    function cmbDateType_ValueChanged() {
        var v = num($id('cmbDateType').value), t = new Date(), iso = function (d) { return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2); };
        if (v === 1) $id('txtDateFrom').value = iso(t);
        else if (v === 2) { var w = new Date(t); w.setDate(w.getDate() - 7); $id('txtDateFrom').value = iso(w); }
        else if (v === 3) { $id('txtDateFrom').value = iso(new Date(t.getFullYear(), t.getMonth(), 1)); $id('txtDateTo').value = iso(t); }
        else if (v === 4) { $id('txtDateFrom').value = t.getFullYear() + '-01-01'; $id('txtDateTo').value = iso(t); }
        else if (v === 5 && st.init) $id('txtDateFrom').value = st.init.fyStart;
    }

    /* FormValidation :399 */
    function FormValidation() {
        if (num($id('CmbParentCategory').value) === 0) { alert('Please Select Parent Category First'); $id('CmbParentCategory').focus(); return false; }
        return true;
    }

    /* GridBind :285 */
    function GridBind() {
        var p = { fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value, itemId: num($id('CmbItem').value), warehouseId: num($id('cmbWareHouse').value),
            itemCategoryId: num($id('cmbItemCategory').value), itemTypeId: num($id('CmbItemType').value), parentCategoryId: num($id('CmbParentCategory').value),
            includePackUom: $id('chkIncludePackUom').checked ? 1 : 0 };
        return SC.postJson(API + '/show', p).then(function (rows) {
            st.rows = rows || [];
            st.group = $id('rdGroupbyItemCategory').checked ? 'CategoryDescription' : ($id('rdGroupbyItemType').checked ? 'TypeDescription' : '');
            st.filters = {}; st.sel = -1;
            render();
        });
    }

    function cellText(r, c) { var v = SC.ci(r, c); if (NUMC[c]) return fmt3(v); return v == null ? '' : String(v); }
    function filteredRows() {
        var keys = Object.keys(st.filters).filter(function (k) { return st.filters[k]; }), out = [];
        st.rows.forEach(function (r, i) { if (keys.every(function (k) { return cellText(r, k).toLowerCase().indexOf(st.filters[k]) >= 0; })) out.push(i); });
        return out;
    }
    function cols() { return COLS.filter(function (c) { return c !== st.group; }); }
    function render() {
        var t = $id('grdFrm');
        if (!st.rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; $id('lblNavigator').textContent = ''; return; }   // grdFrm.DataSource = null
        var cs = cols();
        var h = '<tr>' + cs.map(function (c) { return '<th data-col="' + c + '"' + (WIDTH[c] ? ' style="min-width:' + WIDTH[c] + 'px;width:' + WIDTH[c] + 'px"' : '') + '>' + esc(c) + '</th>'; }).join('') + '</tr>';
        var hf = '<tr class="flt">' + cs.map(function (c) { return '<td><input type="text" data-flt="' + c + '" value="' + esc(st.filters[c] || '') + '"></td>'; }).join('') + '</tr>';
        t.querySelector('thead').innerHTML = h + hf;
        renderBody();
    }
    function renderBody() {
        var t = $id('grdFrm'), cs = cols(), idx = filteredRows(), html = '', total = {}, prev = null;
        idx.forEach(function (i) {
            var r = st.rows[i];
            if (st.group) { var key = String(SC.ci(r, st.group) == null ? '' : SC.ci(r, st.group)); if (prev !== key) { html += '<tr class="grp"><td colspan="' + cs.length + '">' + esc(st.group + ': ' + key) + '</td></tr>'; prev = key; } }
            html += '<tr data-i="' + i + '"' + (i === st.sel ? ' class="sel"' : '') + '>' + cs.map(function (c) {
                return (NUMC[c] || PLAIN_NUM[c]) ? '<td class="n">' + esc(cellText(r, c)) + '</td>' : '<td>' + esc(cellText(r, c)) + '</td>';
            }).join('') + '</tr>';
            cs.forEach(function (c) { if (NUMC[c]) total[c] = (total[c] || 0) + (Number(SC.ci(r, c)) || 0); });
        });
        t.querySelector('tbody').innerHTML = html;
        t.querySelector('tfoot').innerHTML = '<tr>' + cs.map(function (c) { return NUMC[c] ? '<td class="n">' + esc(fmt3(total[c] || 0)) + '</td>' : '<td></td>'; }).join('') + '</tr>';
        $id('lblNavigator').textContent = 'Records: ' + idx.length + (idx.length !== st.rows.length ? ' of ' + st.rows.length : '');
    }

    /* btnPrint_Click :546 - Reporting.ShowReportWithDataTable(dtHistory, "481-StockReconcilationRegister.rpt") */
    function btnPrint_Click() {
        if (!st.rows || !st.rows.length) { alert('Not Record Found For Display'); return; }
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' },
            body: JSON.stringify({ rpt: '481-StockReconcilationRegister.rpt', title: null, rows: st.rows }) })
            .then(function (r) { if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); }); return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); }); })
            .catch(function (e) { if (w) w.close(); alert(e.message); });
    }

    /* btnReset_Click :428 - Rows[2].Activate() = Id 2 (This Week), clear Item / WareHouse, empty grid */
    function btnReset_Click() {
        $id('cmbDateType').value = '2'; cmbDateType_ValueChanged();
        $id('CmbItem').value = ''; $id('cmbWareHouse').value = '';
        st.rows = []; render();
    }
    /* btnRefresh_Click :443 */
    function btnRefresh_Click() {
        return SC.getJson(API + '/combos').then(function (rows) { st.combos = rows || []; StockComboFill(); }).catch(function (e) { alert(e.message); });
    }
    function showClick() { if (!FormValidation()) return Promise.resolve(); return SC.withBusy($id('btnshow'), GridBind).catch(function (e) { alert(e.message); }); }
    function closeForm() { if (window.opener || history.length <= 1) window.close(); else history.back(); }

    /* StockReportReconcilation_Load :109 */
    function load() {
        SC.getJson(API + '/init').then(function (d) {
            st.init = d; st.combos = d.combos || [];
            $id('txtDateTo').value = d.today || SC.today(); $id('txtDateFrom').value = d.today || SC.today();
            DateTypefill(); StockComboFill();
            $id('cmbDateType').focus();
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', showClick);
        $id('btnRefresh').addEventListener('click', function () { SC.withBusy($id('btnRefresh'), btnRefresh_Click); });
        $id('btnReset').addEventListener('click', btnReset_Click);
        $id('btnPrint').addEventListener('click', btnPrint_Click);
        $id('btnshortcutkeys').addEventListener('click', function () { $id('shortcutModal').classList.add('is-open'); });
        $id('shortcutClose').addEventListener('click', function () { $id('shortcutModal').classList.remove('is-open'); });
        $id('cmbDateType').addEventListener('change', cmbDateType_ValueChanged);
        var grid = $id('grdFrm');
        grid.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (!k) return; st.filters[k] = e.target.value.trim().toLowerCase(); renderBody(); });
        grid.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr[data-i]'); if (tr) { st.sel = +tr.getAttribute('data-i'); grid.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); }
        });
        document.addEventListener('keydown', function (e) {   // StockReportReconcilation_KeyDown :455
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey) { $id('shortcutModal').classList.add('is-open'); return; }
            if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); if ($id('shortcutModal').classList.contains('is-open')) { $id('shortcutModal').classList.remove('is-open'); return; } closeForm(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnReset_Click(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh_Click(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); showClick(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); var first = grid.querySelector('tbody tr[data-i]'); if (first) { first.click(); first.scrollIntoView(); } }
            else if (e.ctrlKey && k === 'arrowup') { e.preventDefault(); $id('cmbDateType').focus(); }
        });
        load();
    });
})();
