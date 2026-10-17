/* EvaluationStockTransactionsReport (Pcc.Reports/EvaluationStockTransactionsReport.cs) - ScreenDefinition 815 "Evaluation Transactions Report".
   Line refs ":NNN" are that file. API: /inventory/reports/evaluation-transactions-report/api.
   Optional query (RequestedByOtherDocument :36 - the caller presets the public combos): fromDate, toDate, itemId, supplierCustomerId, warehouseIds. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num, API = '/inventory/reports/evaluation-transactions-report/api';

    /* dtWarehouse :501-519 - grid shape (ItemId hidden :544); GridSettings :540 formats "#,##0.###", amounts clsGlobalVariables.stringFormatsingle */
    var COLS = ['DocumentType', 'DocNo', 'DocDate', 'WareHouse', 'Item', 'Uom', 'JobLot', 'InQty', 'OutQty', 'BalQty', 'InWeight', 'OutWeight', 'InBillWeight', 'OutBillWeight', 'BalWeight', 'AmountIn', 'AmountOut'];
    var CAPTION = { DocumentType: 'Document Type', DocNo: 'Doc No', DocDate: 'Doc Date', WareHouse: 'Ware House', Item: 'Item', Uom: 'Uom', JobLot: 'Job Lot', InQty: 'In Qty', OutQty: 'Out Qty', BalQty: 'Bal Qty',
        InWeight: 'In Weight', OutWeight: 'Out Weight', InBillWeight: 'In Bill Weight', OutBillWeight: 'Out Bill Weight', BalWeight: 'Bal Weight', AmountIn: 'Amount In', AmountOut: 'Amount Out' };
    var FMT = { InQty: 'q', OutQty: 'q', BalQty: 'q', InWeight: 'q', OutWeight: 'q', InBillWeight: 'q', OutBillWeight: 'q', BalWeight: 'q', AmountIn: 'a', AmountOut: 'a' };
    var SUM = { InQty: 1, OutQty: 1, BalQty: 1, InWeight: 1, OutWeight: 1, InBillWeight: 1, OutBillWeight: 1, BalWeight: 1, AmountIn: 1, AmountOut: 1 };
    var RPT = '417-InventoryStockTransactionsReport.rpt';   // btnStockReport_Click :420
    /* MakeShortCutKeys :645 */
    var SHORTCUTS = [['Ctrl+S', 'For Show Record'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print'], ['Ctrl+F5', 'For Focus on from Date'],
        ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid History'], ['Ctrl+ArrowUp', 'For Focus On from Date']];

    var st = { init: null, combos: [], lst: [], rows: [], filters: {}, sel: -1 };

    function checked(listId) { var out = []; $id(listId).querySelectorAll('input[data-id]:checked').forEach(function (i) { out.push({ id: i.getAttribute('data-id'), name: i.parentNode.textContent.trim() }); }); return out; }
    function syncText(listId, textId) {
        $id(textId).textContent = checked(listId).map(function (c) { return c.name; }).join(',');
        var all = $id(listId).querySelector('input.all'), boxes = $id(listId).querySelectorAll('input[data-id]');
        if (all) all.checked = boxes.length > 0 && checked(listId).length === boxes.length;
    }
    function multiHtml(rows) {
        return '<label class="all"><input type="checkbox" class="all"> </label>' + rows.map(function (r) {
            return '<label><input type="checkbox" data-id="' + esc(r.Id) + '"> ' + esc(r.name) + '</label>';
        }).join('');
    }
    function fill(id, rows, blank) { var el = $id(id), keep = el.value; el.innerHTML = ''; el.appendChild(new Option(blank || '', '')); rows.forEach(function (r) { el.appendChild(new Option(String(r.name), String(r.Id))); }); el.value = keep; if (el.value !== keep) el.value = ''; }
    function ids(listId) { return checked(listId).map(function (c) { return c.id; }).join(','); }

    /* StockComboFill :182 - split Sp_Inventory_InventoryTransactions_DropDownAndLists by ActivityType; Item Type and WareHouse are checked lists */
    function StockComboFill(combos) {
        st.combos = combos || [];
        var by = { ItemTypes: [], ItemCategories: [], Items: [], JobLot: [], Supplier_Customer: [], ParentCategories: [], ItemClassGroup: [], Warehouse: [] };
        st.combos.forEach(function (r) { var t = String(SC.ci(r, 'ActivityType')); if (by[t]) by[t].push({ Id: SC.ci(r, 'Id'), name: String(SC.ci(r, 'name') == null ? '' : SC.ci(r, 'name')) }); });
        $id('cmbItemtypeList').innerHTML = multiHtml(by.ItemTypes); syncText('cmbItemtypeList', 'cmbItemtypeText');
        $id('CmbWareHouseList').innerHTML = multiHtml(by.Warehouse); syncText('CmbWareHouseList', 'CmbWareHouseText');
        if (by.ItemCategories.length) fill('cmbCategory', by.ItemCategories);
        if (by.Items.length) fill('CmbItem', by.Items);
        if (by.JobLot.length) fill('CmbJobLot', by.JobLot);
        if (by.Supplier_Customer.length) fill('CmbSupplier', by.Supplier_Customer);
        if (by.ParentCategories.length) fill('cmbParentCategory', by.ParentCategories);
        if (by.ItemClassGroup.length) fill('cmbItemClaas', by.ItemClassGroup);
    }
    /* ParameterFill :168 */
    function ParameterFill(dateTypes) { fill('cmbperemeter', (dateTypes || []).map(function (d) { return { Id: SC.ci(d, 'Id'), name: SC.ci(d, 'Parameters') }; })); }
    /* DocumentTypeFillFromInventoryTransactions :342 */
    function DocumentTypeFill(rows) { if (rows && rows.length) fill('CmbDocumentType', rows.map(function (r) { return { Id: SC.ci(r, 'RefDocumentTypeId'), name: SC.ci(r, 'DocumentTypeDescription') }; })); }

    function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
    /* cmbperemeter_ValueChanged :683 */
    function cmbperemeter_ValueChanged() {
        var v = num($id('cmbperemeter').value), today = new Date();
        if (v === 1) $id('fromdate').value = iso(today);
        else if (v === 2) { var d = new Date(today); d.setDate(d.getDate() - 7); $id('fromdate').value = iso(d); }
        else if (v === 3) { $id('fromdate').value = iso(new Date(today.getFullYear(), today.getMonth(), 1)); $id('todate').value = iso(today); }
        else if (v === 4) { $id('fromdate').value = iso(new Date(today.getFullYear(), 0, 1)); $id('todate').value = iso(today); }
        else if (v === 5 && st.init) $id('fromdate').value = st.init.fyStart;
    }

    function params() {
        return { fromDate: $id('fromdate').value, toDate: $id('todate').value, itemId: num($id('CmbItem').value), supplierCustomerId: num($id('CmbSupplier').value),
            jobLotId: num($id('CmbJobLot').value), documentTypeId: num($id('CmbDocumentType').value), parentCategoryId: num($id('cmbParentCategory').value),
            itemCategoryId: num($id('cmbCategory').value), itemClassGroupId: num($id('cmbItemClaas').value), warehouseIds: ids('CmbWareHouseList'), itemTypeIds: ids('cmbItemtypeList') };
    }

    /* btnshow_Click :451 */
    function btnshow_Click() {
        st.lst = [];
        return SC.postJson(API + '/show', params()).then(function (rows) {
            st.lst = rows || [];
            if (!st.lst.length) { st.rows = []; render(); return; }   // grd.ClearStructure() :531
            st.rows = st.lst.map(function (r) {   // dtWarehouse :523
                var g = function (k) { var v = SC.ci(r, k); return v == null ? '' : v; }, d = function (k) { return Number(SC.ci(r, k)) || 0; };
                return { DocumentType: g('DocumentTypeDescription'), DocNo: g('DocCodeNo'), DocDate: SC.gridDate(g('DocDate')), WareHouse: g('WareHouseName'), ItemId: g('ItemId'), Item: g('ItemName'),
                    Uom: g('ItemUom'), JobLot: g('JobLotDescription'), InQty: d('CurrInQty'), OutQty: d('CurrOutQty'), BalQty: d('BalQty'), InWeight: d('CurrInWeight'), OutWeight: d('CurrOutWeight'),
                    InBillWeight: d('CurrInBillWeight'), OutBillWeight: d('CurrOutBillWeight'), BalWeight: d('BalWeight'), AmountIn: d('AmountIn'), AmountOut: d('AmountOut') };
            });
            st.filters = {}; st.sel = -1;
            render();   // GridSettings :540
        });
    }

    function fmtNum(v, kind) {
        v = Number(v) || 0;
        if (kind === 'a') return v.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 });   // stringFormatsingle
        return v.toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 3 });                      // "#,##0.###"
    }
    function cellText(r, c) { return FMT[c] ? fmtNum(r[c], FMT[c]) : String(r[c] == null ? '' : r[c]); }
    function filteredRows() {
        var keys = Object.keys(st.filters).filter(function (k) { return st.filters[k]; }), out = [];
        st.rows.forEach(function (r, i) { if (keys.every(function (k) { return cellText(r, k).toLowerCase().indexOf(st.filters[k]) >= 0; })) out.push(i); });
        return out;
    }
    function render() {
        var t = $id('grd');
        if (!st.rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; $id('lblNavigator').textContent = ''; return; }
        t.querySelector('thead').innerHTML = '<tr>' + COLS.map(function (c) { return '<th data-col="' + c + '">' + esc(CAPTION[c]) + '</th>'; }).join('') + '</tr>'
            + '<tr class="flt">' + COLS.map(function (c) { return '<td><input type="text" data-flt="' + c + '" value="' + esc(st.filters[c] || '') + '"></td>'; }).join('') + '</tr>';
        renderBody();
    }
    function renderBody() {
        var t = $id('grd'), idx = filteredRows(), html = '', total = {};
        idx.forEach(function (i) {
            var r = st.rows[i];
            html += '<tr data-i="' + i + '"' + (i === st.sel ? ' class="sel"' : '') + '>' + COLS.map(function (c) { return (FMT[c] ? '<td class="n">' : '<td>') + esc(cellText(r, c)) + '</td>'; }).join('') + '</tr>';
            COLS.forEach(function (c) { if (SUM[c]) total[c] = (total[c] || 0) + (Number(r[c]) || 0); });
        });
        t.querySelector('tbody').innerHTML = html;
        t.querySelector('tfoot').innerHTML = '<tr>' + COLS.map(function (c) { return SUM[c] ? '<td class="n">' + esc(fmtNum(total[c] || 0, FMT[c])) + '</td>' : '<td></td>'; }).join('') + '</tr>';
        $id('lblNavigator').textContent = 'Records: ' + idx.length + (idx.length !== st.rows.length ? ' of ' + st.rows.length : '');
    }

    /* btnStockReport_Click :408 - Reporting.ShowReportWithDataTable(lst, "417-InventoryStockTransactionsReport.rpt") + @CompanyName / @CompanyAddress */
    function btnStockReport_Click() {
        if (!st.lst || !st.lst.length) { alert('Record Not Found For Display'); return; }
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify({ rpt: RPT, title: null, rows: st.lst }) })
            .then(function (r) { if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); }); return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); }); })
            .catch(function (e) { if (w) w.close(); alert(e.message); });
    }

    /* reset :367 */
    function reset() {
        $id('CmbWareHouseList').querySelectorAll('input').forEach(function (i) { i.checked = false; }); syncText('CmbWareHouseList', 'CmbWareHouseText');
        $id('CmbSupplier').value = ''; $id('CmbItem').value = ''; $id('CmbJobLot').value = '';
        $id('cmbperemeter').focus();
        if (st.init) ParameterFill(st.init.dateTypes);
    }
    /* btnRefresh_Click :735 */
    function btnRefresh_Click() {
        return SC.getJson(API + '/lookups').then(function (d) { StockComboFill(d.combos); DocumentTypeFill(d.documentTypes); }).catch(function (e) { alert(e.message); });
    }
    function showShortcuts() {
        $id('shortcutRows').innerHTML = SHORTCUTS.map(function (s) { return '<tr><td>' + esc(s[0]) + '</td><td>' + esc(s[1]) + '</td></tr>'; }).join('');
        $id('shortcutModal').classList.add('is-open');
    }

    /* PurchaseOrderHistory_Load :141 */
    function load() {
        var q = new URLSearchParams(location.search), requested = q.has('itemId') || q.has('supplierCustomerId') || q.has('fromDate');
        SC.getJson(API + '/init').then(function (d) {
            st.init = d;
            StockComboFill(d.combos); ParameterFill(d.dateTypes); DocumentTypeFill(d.documentTypes);
            $id('fromdate').value = SC.today(); $id('todate').value = SC.today();
            $id('cmbperemeter').focus();
            if (!requested) {   // :152
                $id('cmbperemeter').value = '2'; cmbperemeter_ValueChanged();   // Rows[1].Activate() = "This Week"
                if (d.asOnDate) $id('fromdate').value = d.asOnDate;             // GetInventoryStockAsOnDate :155
            } else {
                if (q.get('fromDate')) $id('fromdate').value = q.get('fromDate');
                if (q.get('toDate')) $id('todate').value = q.get('toDate');
                if (q.get('itemId')) $id('CmbItem').value = q.get('itemId');
                if (q.get('supplierCustomerId')) $id('CmbSupplier').value = q.get('supplierCustomerId');
                var wh = (q.get('warehouseIds') || '').split(',').filter(Boolean).map(num);
                if (wh.length) { $id('CmbWareHouseList').querySelectorAll('input[data-id]').forEach(function (i) { i.checked = wh.indexOf(num(i.getAttribute('data-id'))) >= 0; }); syncText('CmbWareHouseList', 'CmbWareHouseText'); }
            }
        }).catch(function (e) { alert(e.message); });
    }

    function showClick() { return SC.withBusy($id('btnshow'), btnshow_Click).catch(function (e) { alert(e.message); }); }
    function closeForm() { if (window.opener || history.length <= 1) window.close(); else history.back(); }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', showClick);
        $id('btnNew').addEventListener('click', reset);
        $id('btnRefresh').addEventListener('click', function () { SC.withBusy($id('btnRefresh'), btnRefresh_Click); });
        $id('btnStockReport').addEventListener('click', btnStockReport_Click);
        $id('btnShortCutKeys').addEventListener('click', showShortcuts);
        $id('shortcutClose').addEventListener('click', function () { $id('shortcutModal').classList.remove('is-open'); });
        $id('cmbperemeter').addEventListener('change', cmbperemeter_ValueChanged);
        ['cmbItemtype', 'CmbWareHouse'].forEach(function (id) {
            $id(id).querySelector('.fx-multi-btn').addEventListener('click', function () { var l = $id(id + 'List'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
            $id(id + 'List').addEventListener('change', function (e) {
                if (e.target.classList.contains('all')) $id(id + 'List').querySelectorAll('input[data-id]').forEach(function (i) { i.checked = e.target.checked; });
                syncText(id + 'List', id + 'Text');
            });
        });
        document.addEventListener('mousedown', function (e) {
            if (e.target.closest('.fx-multi')) return;
            Array.prototype.forEach.call(document.querySelectorAll('.fx-multi-list'), function (l) { l.style.display = 'none'; });
        });
        var grid = $id('grd');
        grid.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (!k) return; st.filters[k] = e.target.value.trim().toLowerCase(); renderBody(); });
        grid.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr[data-i]'); if (tr) { st.sel = +tr.getAttribute('data-i'); grid.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); }
        });
        document.addEventListener('keydown', function (e) {   // PurchaseOrderHistory_KeyDown :594
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey) { e.preventDefault(); showShortcuts(); return; }
            if (e.ctrlKey && k === 'p') { e.preventDefault(); btnStockReport_Click(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); if ($id('shortcutModal').classList.contains('is-open')) $id('shortcutModal').classList.remove('is-open'); else closeForm(); }
            else if (e.ctrlKey && (k === 'n' || k === 'r')) { e.preventDefault(); btnRefresh_Click(); }   // Ctrl+N and Ctrl+R both run btnRefresh_Click (:610, :622)
            else if (e.ctrlKey && k === 'f5') { e.preventDefault(); $id('cmbperemeter').focus(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); showClick(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); var f = grid.querySelector('input'); if (f) f.focus(); else grid.focus(); }
            else if (e.ctrlKey && k === 'arrowup') { e.preventDefault(); $id('cmbperemeter').focus(); }
        });
        load();
    });
})();
