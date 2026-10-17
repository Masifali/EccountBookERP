/* RptGenrateStocksStore (Inventory_Stocks_Report/RptGenrateStocksStore.cs) - ScreenDefinition 300 "Stock Stocks Report" / "Stock Report Store".
   Line refs ":NNN" are that file. API: /inventory/reports/stock-report-store/api. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num, API = '/inventory/reports/stock-report-store/api';

    /* Datetypefill :161 - InfragisticsHelper.BindComboDateType(..., ActivateRow: true, 5): the five literal date types, 5 active */
    var DATE_TYPES = [[1, 'This Day'], [2, 'This Week'], [3, 'This Month'], [4, 'This Year'], [5, 'Financial Year']];
    /* GridBind :338 dtcol column order; Gridsetting :396 hides ItemId, WareHouseId, RackId, ItemConditionId and groups on ParentCategory */
    var COLS = ['WareHouseCode', 'RackName', 'TypeDescription', 'CategoryDescription', 'ItemCode', 'ItemCondition', 'ItemName', 'Opening', 'QtyIn', 'QtyOut', 'Closing'];
    var CAPTION = { ParentCategory: 'Parent Category', WareHouseCode: 'WareHouse Code', RackName: 'Rack Name', TypeDescription: 'Type Description', CategoryDescription: 'Category Description',
        ItemCode: 'Item Code', ItemCondition: 'Item Condition', ItemName: 'Item Name', Opening: 'Opening', QtyIn: 'Qty In', QtyOut: 'Qty Out', Closing: 'Closing' };
    /* Constants.InventoryConstants widths are not in the recovered source (WareHouseName, DiscountType, DocNoConstant, StringConstant, ItemName, QtyConstant) */
    var WIDTH = { WareHouseCode: 120, RackName: 120, TypeDescription: 110, CategoryDescription: 110, ItemCode: 80, ItemCondition: 100, ItemName: 200, Opening: 90, QtyIn: 90, QtyOut: 90, Closing: 90 };
    var NUMC = { Opening: 1, QtyIn: 1, QtyOut: 1, Closing: 1 };   // AggregateFunction Sum, FormatString "#,##0.###"

    var st = { init: null, combos: [], raw: [], rows: [], filters: {}, sel: -1 };

    function fill(id, rows, blank) { var el = $id(id), keep = el.value; el.innerHTML = ''; el.appendChild(new Option(blank || '', '')); rows.forEach(function (r) { el.appendChild(new Option(String(r.name), String(r.Id))); }); el.value = keep; if (el.value !== keep) el.value = ''; }
    function fmt3(v) { v = Number(v) || 0; return v.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 }); }
    function fmt0(v) { v = Number(v) || 0; return Math.round(v).toLocaleString('en-US', { maximumFractionDigits: 0 }); }

    /* ParentCategoryFill :191 - literal 7 Packing Material / 8 Store, caption "Parent Category", no zero row */
    function ParentCategoryFill() { fill('CmbParentCategory', (st.init.parentCategories || []).map(function (r) { return { Id: SC.ci(r, 'Id'), name: SC.ci(r, 'ParentCategory') }; })); }
    /* Datetypefill :161 */
    function Datetypefill() { var el = $id('cmbDateType'); el.innerHTML = ''; DATE_TYPES.forEach(function (d) { el.appendChild(new Option(d[1], String(d[0]))); }); el.value = '5'; cmbDateType_ValueChanged(); }

    /* OtherComboByParentCategoryFill :209 - rows of the chosen parent category (or of 7,8 when none), one entry per ActivityType+Id */
    function OtherComboByParentCategoryFill() {
        var pc = num($id('CmbParentCategory').value);
        var rows = st.combos.filter(function (r) { var p = num(SC.ci(r, 'ParentCategoryId')); return pc > 0 ? p === pc : ('7,8'.indexOf(String(SC.ci(r, 'ParentCategoryId'))) >= 0); });
        if (!rows.length) return;   // dtFiltered.Rows.Count <= 0 -> return
        var byType = { ItemCategories: [], ItemTypes: [], Items: [], ItemCondition: [], RackName: [], Warehouse: [] }, seen = {};
        rows.forEach(function (r) {
            var t = String(SC.ci(r, 'ActivityType')), id = num(SC.ci(r, 'Id'));
            if (!byType[t]) return; var k = t + ':' + id; if (seen[k]) return; seen[k] = 1;
            byType[t].push({ Id: id, name: SC.ci(r, 'name') == null ? '' : String(SC.ci(r, 'name')) });
        });
        fill('cmbItemCategory', byType.ItemCategories); fill('cmbItemType', byType.ItemTypes); fill('cmbItemName', byType.Items);
        fill('cmbWarehouse', byType.Warehouse); fill('cmbItemCondition', byType.ItemCondition); fill('CmbRackName', byType.RackName);
    }

    /* cmbDateType_ValueChanged :647 */
    function cmbDateType_ValueChanged() {
        var v = num($id('cmbDateType').value), t = new Date(), iso = function (d) { return d.getFullYear() + '-' + ('0' + (d.getMonth() + 1)).slice(-2) + '-' + ('0' + d.getDate()).slice(-2); };
        if (v === 1) $id('txtDateFrom').value = iso(t);
        else if (v === 2) { var w = new Date(t); w.setDate(w.getDate() - 7); $id('txtDateFrom').value = iso(w); }
        else if (v === 3) { $id('txtDateFrom').value = iso(new Date(t.getFullYear(), t.getMonth(), 1)); $id('txtDateTo').value = iso(t); }
        else if (v === 4) { $id('txtDateFrom').value = t.getFullYear() + '-01-01'; $id('txtDateTo').value = iso(t); }
        else if (v === 5 && st.init) $id('txtDateFrom').value = st.init.fyStart;
    }

    function params() {
        return { fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value, itemId: num($id('cmbItemName').value), itemTypeId: num($id('cmbItemType').value),
            itemCategoryId: num($id('cmbItemCategory').value), parentCategoryId: num($id('CmbParentCategory').value), warehouseId: num($id('cmbWarehouse').value),
            itemConditionId: num($id('cmbItemCondition').value), storeRackId: num($id('CmbRackName').value), zeroBalanceType: $id('chkSkipZero').checked ? 1 : 0 };
    }

    /* GridBind :312 */
    function GridBind() {
        return SC.postJson(API + '/show', params()).then(function (rows) {
            st.raw = rows || [];
            st.rows = st.raw.map(function (r) {   // dtcol :338-359
                var g = function (k) { var v = SC.ci(r, k); return v == null ? '' : v; }, d = function (k) { return Number(SC.ci(r, k)) || 0; };
                return { ParentCategory: g('ParentCategory'), WareHouseId: num(g('WareHouseId')), WareHouseCode: g('WareHouseCode'), RackId: num(g('RackId')), RackName: g('RackName'),
                    TypeDescription: g('TypeDescription'), CategoryDescription: g('CategoryDescription'), ItemId: num(g('ItemId')), ItemCode: g('ItemCode'),
                    ItemConditionId: num(g('ItemConditionId')), ItemCondition: g('ItemCondition'), ItemName: g('ItemName'),
                    Opening: d('QtyOpening'), QtyIn: d('QtyIn'), QtyOut: d('QtyOut'), Closing: d('BalQty') };
            });
            st.filters = {}; st.sel = -1;
            render();
            var qin = 0, qout = 0;   // :363-368 - Balance total is In - Out
            st.rows.forEach(function (r) { qin += r.QtyIn; qout += r.QtyOut; });
            $id('txtQtyInTotal').value = fmt0(qin); $id('txtQtyOutTotal').value = fmt0(qout); $id('txtQtyBalTotal').value = fmt0(qin - qout);
        });
    }

    function cellText(r, c) { return NUMC[c] ? fmt3(r[c]) : String(r[c] == null ? '' : r[c]); }
    function filteredRows() {
        var keys = Object.keys(st.filters).filter(function (k) { return st.filters[k]; }), out = [];
        st.rows.forEach(function (r, i) { if (keys.every(function (k) { return cellText(r, k).toLowerCase().indexOf(st.filters[k]) >= 0; })) out.push(i); });
        return out;
    }
    function render() {
        var t = $id('grdfrm');
        if (!st.rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; $id('lblNavigator').textContent = ''; return; }   // grdfrm.ClearStructure()
        var h = '<tr>' + COLS.map(function (c) { return '<th data-col="' + c + '" style="min-width:' + WIDTH[c] + 'px;width:' + WIDTH[c] + 'px">' + esc(CAPTION[c]) + '</th>'; }).join('') + '</tr>';
        var hf = '<tr class="flt">' + COLS.map(function (c) { return '<td><input type="text" data-flt="' + c + '" value="' + esc(st.filters[c] || '') + '"></td>'; }).join('') + '</tr>';
        t.querySelector('thead').innerHTML = h + hf;
        renderBody();
    }
    /* Gridsetting :396 - grouped on ParentCategory (hidden when grouped), GroupTotals Always, sums on the four qty columns */
    function renderBody() {
        var t = $id('grdfrm'), idx = filteredRows(), html = '', total = {}, gt = {}, prev = null;
        function sums(o) { return COLS.map(function (c) { return NUMC[c] ? '<td class="n">' + esc(fmt3(o[c] || 0)) + '</td>' : '<td></td>'; }).join(''); }
        function closeGroup() { if (prev !== null) html += '<tr class="gtot">' + sums(gt) + '</tr>'; gt = {}; }
        idx.forEach(function (i) {
            var r = st.rows[i], key = String(r.ParentCategory == null ? '' : r.ParentCategory);
            if (prev !== key) { closeGroup(); html += '<tr class="grp"><td colspan="' + COLS.length + '">' + esc(CAPTION.ParentCategory + ': ' + key) + '</td></tr>'; prev = key; }
            html += '<tr data-i="' + i + '"' + (i === st.sel ? ' class="sel"' : '') + '>' + COLS.map(function (c) {
                if (c === 'ItemName') return '<td title="Open Store Item Ledger"><a data-link="ItemName" data-i="' + i + '">' + esc(r[c]) + '</a></td>';
                return NUMC[c] ? '<td class="n">' + esc(cellText(r, c)) + '</td>' : '<td>' + esc(cellText(r, c)) + '</td>';
            }).join('') + '</tr>';
            COLS.forEach(function (c) { if (NUMC[c]) { total[c] = (total[c] || 0) + r[c]; gt[c] = (gt[c] || 0) + r[c]; } });
        });
        closeGroup();
        t.querySelector('tbody').innerHTML = html;
        t.querySelector('tfoot').innerHTML = '<tr>' + sums(total) + '</tr>';
        $id('lblNavigator').textContent = 'Records: ' + idx.length + (idx.length !== st.rows.length ? ' of ' + st.rows.length : '');
    }

    /* grdfrm_LinkClicked :699 - InventoryTransactionReportStore (RequestedByOtherDocument) = /stocks/store-item-ledger */
    function openLedger(r, withRack) {
        var wh = r.WareHouseId; if (wh === 0 && num($id('cmbWarehouse').value) > 0) wh = num($id('cmbWarehouse').value);
        var q = { itemId: r.ItemId, warehouseId: wh || '', conditionId: r.ItemConditionId || '', fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value };
        if (withRack) q.rackId = r.RackId || '';
        window.open('/stocks/store-item-ledger' + SC.qs(q), '_blank');
    }

    /* PrintRegister :612 - Reporting.ShowReportWithDataTable(dtdetail, "401-StockEvalautionDetail_GenerateStocks_Register -Store.rpt") */
    function PrintRegister() {
        if (!st.raw || !st.raw.length) { alert('Not Record Found For Display'); return; }
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' },
            body: JSON.stringify({ rpt: '401-StockEvalautionDetail_GenerateStocks_Register -Store.rpt', title: null, rows: st.raw }) })
            .then(function (r) { if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); }); return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); }); })
            .catch(function (e) { if (w) w.close(); alert(e.message); });
    }

    /* Reset :463 */
    function Reset() {
        ['cmbItemName', 'cmbItemType', 'cmbItemCategory', 'cmbWarehouse'].forEach(function (id) { $id(id).value = ''; });
        $id('cmbDateType').focus(); Datetypefill();
    }
    /* btnRefresh_Click :492 */
    function btnRefresh_Click() {
        return SC.getJson(API + '/combos').then(function (rows) { st.combos = rows || []; OtherComboByParentCategoryFill(); }).catch(function (e) { alert(e.message); });
    }
    function showClick() { return SC.withBusy($id('btnshow'), GridBind).catch(function (e) { alert(e.message); }); }
    function closeForm() { if (window.opener || history.length <= 1) window.close(); else history.back(); }

    /* InventoryStockEvaluation_GenrateStocks_Load :141 */
    function load() {
        SC.getJson(API + '/init').then(function (d) {
            st.init = d; st.combos = d.combos || [];
            $id('txtDateTo').value = d.today || SC.today(); $id('txtDateFrom').value = d.today || SC.today();
            ParentCategoryFill(); OtherComboByParentCategoryFill(); Datetypefill();
            $id('cmbDateType').focus();
            return GridBind();
        }).catch(function (e) { alert(e.message); });
    }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', showClick);
        $id('btnRefresh').addEventListener('click', function () { SC.withBusy($id('btnRefresh'), btnRefresh_Click); });
        $id('btnNew').addEventListener('click', Reset);
        $id('btnPrint').addEventListener('click', PrintRegister);
        $id('btnshortcutkeys').addEventListener('click', function () { $id('shortcutModal').classList.add('is-open'); });
        $id('shortcutClose').addEventListener('click', function () { $id('shortcutModal').classList.remove('is-open'); });
        $id('cmbDateType').addEventListener('change', cmbDateType_ValueChanged);
        $id('CmbParentCategory').addEventListener('change', OtherComboByParentCategoryFill);   // CmbParentCategory_Leave :763
        var grid = $id('grdfrm');
        grid.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (!k) return; st.filters[k] = e.target.value.trim().toLowerCase(); renderBody(); });
        grid.addEventListener('click', function (e) {
            var a = e.target.closest('a[data-link]'); if (a) { e.preventDefault(); var r = st.rows[+a.getAttribute('data-i')]; if (r) openLedger(r, true); return; }
            var tr = e.target.closest('tbody tr[data-i]'); if (tr) { st.sel = +tr.getAttribute('data-i'); grid.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); }
        });
        document.addEventListener('keydown', function (e) {   // RptGenrateStocks_KeyDown :505, grdfrm_KeyDown :736
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey) { $id('shortcutModal').classList.add('is-open'); return; }
            if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); if ($id('shortcutModal').classList.contains('is-open')) { $id('shortcutModal').classList.remove('is-open'); return; } closeForm(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); PrintRegister(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); Reset(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh_Click(); }
            else if (e.ctrlKey && (k === 'f5' || k === 'arrowup')) { e.preventDefault(); $id('cmbDateType').focus(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); showClick(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); var first = grid.querySelector('tbody tr[data-i]'); if (first) { first.click(); first.scrollIntoView(); } }
            else if (e.ctrlKey && k === ' ' && st.sel >= 0 && st.rows[st.sel]) { e.preventDefault(); openLedger(st.rows[st.sel], false); }
        });
        load();
    });
})();
