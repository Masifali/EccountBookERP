/* frmStockReportWithValues_Engr (Mfg.Reports/frmStockReportWithValues_Engr.cs) - ScreenDefinition 805 "Stock Report With Values (Engr)".
   Line refs ":NNN" are that file. API: /inventory/reports/stock-report-with-values-engr/api.
   Optional query (the desktop's public FromDate / ToDate fields :37-39): fromDate, toDate. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num, API = '/inventory/reports/stock-report-with-values-engr/api';

    /* grdfrm column sets in GridSettings :773 order; widths from Constants.InventoryConstants / GridSettings :785-817.
       dtditinct :193-217 is the row shape. The "Weight" set is Visible=false (:783). */
    var SETS = [
        { key: 'Item', caption: 'Item', cols: ['AccountTitle', 'ItemName'] },
        { key: 'Pack', caption: '', cols: ['PackUom'] },
        { key: 'ModalDescription', caption: '', cols: ['ModalDescription'] },
        { key: 'ProductionStage', caption: '', cols: ['ProductionStage'] },
        { key: 'CastingType', caption: '', cols: ['CastingType'] },
        { key: 'WareHouse', caption: '', cols: ['WareHouseName'] },
        { key: 'Qty', caption: 'Qty', cols: ['OpQty', 'QtyIn', 'QtyOut', 'BalQty'] },
        { key: 'Weight', caption: 'Weight', cols: ['OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'] },
        { key: 'Amount', caption: 'Amount', cols: ['OpAmount', 'AmountIn', 'AmountOut', 'BalAmount', 'AvgRate'] },
        { key: 'WorkOrderNo', caption: '', cols: ['WorkOrderNo'] },
        { key: 'BatchNo', caption: '', cols: ['BatchNo'] }
    ];
    var VISIBLE_SETS = { Item: 1, Pack: 1, ModalDescription: 1, ProductionStage: 1, CastingType: 1, WareHouse: 1, Qty: 1, Weight: 0, Amount: 1, WorkOrderNo: 1, BatchNo: 1 };
    var CAPTION = { AccountTitle: 'Account', ItemName: 'Name', PackUom: 'UOM', ModalDescription: 'Modal Description', ProductionStage: 'Production Stage', CastingType: 'Casting Type', WareHouseName: 'Warehouse Name',
        OpQty: 'Opening', QtyIn: 'In', QtyOut: 'Out', BalQty: 'Balance', OpWeight: 'Opening', WeightIn: 'In', WeightOut: 'Out', BalWeight: 'Balance',
        OpAmount: 'Opening', AmountIn: 'In', AmountOut: 'Out', BalAmount: 'Balance', AvgRate: 'Avg Rate', WorkOrderNo: 'Work Order No', BatchNo: 'Batch No' };
    var WIDTH = { AccountTitle: 150, ItemName: 150, PackUom: 60, ModalDescription: 70, ProductionStage: 100, CastingType: 100, WareHouseName: 150,
        OpQty: 70, QtyIn: 70, QtyOut: 70, BalQty: 70, OpWeight: 80, WeightIn: 80, WeightOut: 80, BalWeight: 80,
        OpAmount: 110, AmountIn: 110, AmountOut: 110, BalAmount: 110, AvgRate: 60, WorkOrderNo: 75, BatchNo: 100 };
    /* FormatString :818-868 - "#,##0" everywhere, AvgRate "#,##0.####" */
    var FMT = { OpQty: '0', QtyIn: '0', QtyOut: '0', BalQty: '0', OpWeight: '0', WeightIn: '0', WeightOut: '0', BalWeight: '0',
        OpAmount: '0', AmountIn: '0', AmountOut: '0', BalAmount: '0', AvgRate: 'rate' };
    var SUM = { OpQty: 1, QtyIn: 1, QtyOut: 1, BalQty: 1, OpWeight: 1, WeightIn: 1, WeightOut: 1, BalWeight: 1, OpAmount: 1, AmountIn: 1, AmountOut: 1, BalAmount: 1 };
    var RPT = '1671_ItemStockSummary_Engr';   // PrintButtonManage :1068 / btnPrint_Click :1089

    var st = { init: null, combos: [], mainTypes: [], branches: [], raw: [], rows: [], filters: {}, sel: -1, loaded: false };

    function checked(listId) { var out = []; $id(listId).querySelectorAll('input[data-id]:checked').forEach(function (i) { out.push({ id: i.getAttribute('data-id'), name: i.parentNode.textContent.trim() }); }); return out; }
    function syncText(listId, textId) {
        $id(textId).textContent = checked(listId).map(function (c) { return c.name; }).join(',');
        var all = $id(listId).querySelector('input.all'), boxes = $id(listId).querySelectorAll('input[data-id]');
        if (all) all.checked = boxes.length > 0 && checked(listId).length === boxes.length;
    }
    function multiHtml(rows, idKey, nameKey, isChecked) {
        return '<label class="all"><input type="checkbox" class="all"> </label>' + rows.map(function (r) {
            var id = SC.ci(r, idKey);
            return '<label><input type="checkbox" data-id="' + esc(id) + '"' + (isChecked(r) ? ' checked' : '') + '> ' + esc(SC.ci(r, nameKey)) + '</label>';
        }).join('');
    }
    function fill(id, rows, blank) { var el = $id(id), keep = el.value; el.innerHTML = ''; el.appendChild(new Option(blank || '', '')); rows.forEach(function (r) { el.appendChild(new Option(String(r.name), String(r.Id))); }); el.value = keep; if (el.value !== keep) el.value = ''; }
    function clearCombos(ids) { ids.forEach(function (id) { $id(id).innerHTML = '<option value=""></option>'; }); }
    function selText(id) { var o = $id(id).selectedOptions[0]; return o && o.value !== '' ? o.textContent : ''; }
    function show(id, on) { $id(id).classList.toggle('is-hidden', !on); }
    function branchIds() { return checked('cmbBranchNameList').map(function (c) { return c.id; }).join(','); }
    var OTHER = ['cmbCategory', 'cmbItemType', 'cmbItem', 'cmbWareHouse', 'cmbStockAccounts', 'CmbWorkOrder', 'CmbModalDescription', 'CmbBatchNo', 'CmbProductionStage', 'CmbCastingType'];

    /* BranchesFill :263 - checked list, Text = UserAccount.BranchName */
    function BranchesFill() {
        $id('cmbBranchNameList').innerHTML = multiHtml(st.branches, 'BranchId', 'BranchName', function (b) { return num(SC.ci(b, 'BranchId')) === num(st.init.userBranchId); });
        syncText('cmbBranchNameList', 'cmbBranchNameText');
    }
    /* ParentCategoryComboFill :291 - rows with ActivityType 'ParentCategories' and Id in 1,2,3,4 (:324), every row ticked (:346) */
    function ParentCategoryComboFill(combos) {
        st.combos = combos || [];
        st.mainTypes = st.combos.filter(function (r) { var id = num(SC.ci(r, 'Id')); return String(SC.ci(r, 'ActivityType')) === 'ParentCategories' && (id === 1 || id === 2 || id === 3 || id === 4); })
            .map(function (r) { return { Id: num(SC.ci(r, 'Id')), name: String(SC.ci(r, 'name')) }; });
        $id('cmbMainTypeList').innerHTML = multiHtml(st.mainTypes, 'Id', 'name', function () { return true; });
        syncText('cmbMainTypeList', 'cmbMainTypeText');
    }
    /* OtherComboByParentCategoryFill :363 - rows whose InventoryParentCategoriesId is a ticked parent category */
    function OtherComboByParentCategoryFill() {
        var valid = {}; checked('cmbMainTypeList').forEach(function (c) { valid[String(num(c.id))] = 1; });
        if (!Object.keys(valid).length) return;
        var byType = { ItemTypes: [], ItemCategories: [], Items: [], Warehouse: [], ModalDescription: [], WorkOrder: [], ProductionStage: [], CastingType: [], BatchNo: [], Stock_Account: [] }, seen = {}, any = false;
        st.combos.forEach(function (r) {
            if (!valid[String(num(SC.ci(r, 'InventoryParentCategoriesId')))]) return;
            any = true;
            var t = String(SC.ci(r, 'ActivityType')), id = num(SC.ci(r, 'Id'));
            if (!byType[t]) return; var k = t + ':' + id; if (seen[k]) return; seen[k] = 1;
            byType[t].push({ Id: id, name: String(SC.ci(r, 'name') == null ? '' : SC.ci(r, 'name')) });
        });
        if (!any) return;   // dtFiltered.Rows.Count <= 0 -> return :393
        fill('cmbItemType', byType.ItemTypes); fill('cmbCategory', byType.ItemCategories); fill('cmbItem', byType.Items); fill('cmbWareHouse', byType.Warehouse);
        fill('CmbModalDescription', byType.ModalDescription); fill('CmbWorkOrder', byType.WorkOrder); fill('CmbProductionStage', byType.ProductionStage);
        fill('CmbCastingType', byType.CastingType); fill('CmbBatchNo', byType.BatchNo); fill('cmbStockAccounts', byType.Stock_Account);
    }
    /* cmbBranchName_Leave :518 - refill the parent combo for the checked branches, or clear it */
    function cmbBranchName_Leave() {
        var b = branchIds();
        if (!b) { ParentCategoryComboFill([]); return Promise.resolve(); }
        return SC.getJson(API + '/combos' + SC.qs({ branchesIds: b })).then(function (rows) { ParentCategoryComboFill(rows); OtherComboByParentCategoryFill(); }).catch(function (e) { alert(e.message); });
    }
    /* cmbMainType_Leave :536 */
    function cmbMainType_Leave() {
        if (checked('cmbMainTypeList').length) OtherComboByParentCategoryFill();
        else clearCombos(OTHER);
    }

    /* PrintButtonManage :1059 */
    function PrintButtonManage() {
        var v = $id('cmbReportType').value;
        show('btnPrint', !!v);
        if (v === 'ItemStockSummary') $id('btnPrintText').textContent = RPT;
    }

    function params() {
        return { fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value, activity: $id('cmbReportType').value,
            itemCategoryId: num($id('cmbCategory').value), itemTypeId: num($id('cmbItemType').value), warehouseId: num($id('cmbWareHouse').value), itemId: num($id('cmbItem').value),
            itemStockAc: num($id('cmbStockAccounts').value), workOrderId: num($id('CmbWorkOrder').value), productionStageId: num($id('CmbProductionStage').value),
            castingTypeId: num($id('CmbCastingType').value), batchNo: selText('CmbBatchNo'), varientId: num($id('CmbModalDescription').value),
            zeroBalanceType: $id('chkSkipZero').checked ? 1 : 0, saleValue: $id('RdSaleAmount').checked ? 1 : 0,
            modalDescriptionWise: $id('ChkModalDescriptionWise').checked ? 1 : 0, productionStageWise: $id('ChkProductionStageWise').checked ? 1 : 0,
            warehouseWise: $id('ChkWarehouseWise').checked ? 1 : 0, workOrderWise: $id('ChkWorkOrderWise').checked ? 1 : 0,
            batchWise: $id('ChkBatchNoWise').checked ? 1 : 0, castingTypeWise: $id('ChkCastingTypeWise').checked ? 1 : 0,
            ids: checked('cmbMainTypeList').map(function (c) { return c.id; }).join(','), branchesIds: branchIds() };
    }

    /* GridBind :662 */
    function GridBind() {
        show('BtnUpdate', false);
        var p = params();
        if (!p.branchesIds) { $id('cmbBranchName').querySelector('button').focus(); return Promise.reject(new Error('Select Branch First')); }   // :764
        if (!p.ids) { $id('cmbMainType').querySelector('button').focus(); return Promise.reject(new Error('Select Parent Category First')); }    // :761
        return SC.postJson(API + '/show', p).then(function (rows) {
            st.raw = rows || [];
            if (!st.raw.length) { st.rows = []; st.loaded = false; render(); return; }   // grdfrm.DataSource = null :757
            if (p.activity !== 'ItemStockSummary') return;                                 // :745 - only this report type fills dtditinct
            st.rows = st.raw.map(function (r) {   // dtditinct :749
                var g = function (k) { var v = SC.ci(r, k); return v == null ? '' : v; }, d = function (k) { return Number(SC.ci(r, k)) || 0; };
                return { WareHouseName: g('WareHouseName'), AccountId: g('AccountId'), AccountTitle: g('AccountTitle'), ItemId: g('ItemId'), ItemName: g('ItemName'),
                    PackUomId: g('PackUomId'), PackUom: g('PackUom'), ModalDescription: g('ModalDescription'), ProductionStage: g('ProductionStage'), CastingType: g('CastingType'),
                    OpQty: d('OpQty'), QtyIn: d('QtyIn'), QtyOut: d('QtyOut'), BalQty: d('BalQty'), OpWeight: d('OpWeight'), WeightIn: d('WeightIn'), WeightOut: d('WeightOut'), BalWeight: d('BalWeight'),
                    OpAmount: d('OpAmount'), AmountIn: d('AmountIn'), AmountOut: d('AmountOut'), BalAmount: d('BalAmount'), AvgRate: d('AvgRate'),
                    WorkOrderNo: g('WorkOrderNo'), BatchNo: g('BatchNo') };
            });
            st.loaded = true; st.filters = {}; st.sel = -1;
            render();   // GridSettings :773
        });
    }

    function roundAway(x, d) { var f = Math.pow(10, d), s = x < 0 ? -1 : 1; return s * Math.round(Math.abs(x) * f + 1e-9) / f; }
    function fmtNum(v, kind) {
        v = Number(v) || 0;
        if (kind === 'rate') return roundAway(v, 4).toLocaleString('en-US', { minimumFractionDigits: 0, maximumFractionDigits: 4 });   // "#,##0.####"
        return roundAway(v, 0).toLocaleString('en-US', { maximumFractionDigits: 0 });                                                   // "#,##0"
    }
    function visibleSets() { var out = []; SETS.forEach(function (s) { if (VISIBLE_SETS[s.key]) out.push({ set: s, cols: s.cols }); }); return out; }
    function visibleColumns() { var cols = []; visibleSets().forEach(function (g) { cols = cols.concat(g.cols); }); return cols; }
    function cellText(r, c) { return FMT[c] ? fmtNum(r[c], FMT[c]) : String(r[c] == null ? '' : r[c]); }
    function filteredRows() {
        var keys = Object.keys(st.filters).filter(function (k) { return st.filters[k]; }), out = [];
        st.rows.forEach(function (r, i) { if (keys.every(function (k) { return cellText(r, k).toLowerCase().indexOf(st.filters[k]) >= 0; })) out.push(i); });
        return out;
    }

    function render() {
        var t = $id('grdfrm'), vs = visibleSets();
        if (!st.loaded || !st.rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; $id('lblNavigator').textContent = ''; return; }
        var cols = visibleColumns();
        var h1 = '<tr>' + vs.map(function (g) { return '<th colspan="' + g.cols.length + '">' + esc(g.set.caption) + '</th>'; }).join('') + '</tr>';
        var h2 = '<tr>' + cols.map(function (c) { return '<th data-col="' + c + '" style="min-width:' + WIDTH[c] + 'px;width:' + WIDTH[c] + 'px">' + esc(CAPTION[c]) + '</th>'; }).join('') + '</tr>';
        var hf = '<tr class="flt">' + cols.map(function (c) { return '<td><input type="text" data-flt="' + c + '" value="' + esc(st.filters[c] || '') + '"></td>'; }).join('') + '</tr>';
        t.querySelector('thead').innerHTML = h1 + h2 + hf;
        renderBody();
    }
    function renderBody() {
        var t = $id('grdfrm'), cols = visibleColumns(), idx = filteredRows(), html = '', total = {};
        idx.forEach(function (i) {
            var r = st.rows[i];
            html += '<tr data-i="' + i + '"' + (i === st.sel ? ' class="sel"' : '') + '>' + cols.map(function (c) {
                if (c === 'AccountTitle') return '<td title="frmItemEvaluationLedger_Engr"><a data-link="AccountTitle" data-i="' + i + '">' + esc(r[c]) + '</a></td>';   // ColumnType Link :781
                if (c === 'ItemName') return '<td title="frmTransactionReportWithValues_Engr"><a data-link="ItemName" data-i="' + i + '">' + esc(r[c]) + '</a></td>';   // ColumnType Link :780
                return FMT[c] ? '<td class="n">' + esc(cellText(r, c)) + '</td>' : '<td>' + esc(cellText(r, c)) + '</td>';
            }).join('') + '</tr>';
            cols.forEach(function (c) { if (SUM[c]) total[c] = (total[c] || 0) + (Number(r[c]) || 0); });
        });
        t.querySelector('tbody').innerHTML = html;
        t.querySelector('tfoot').innerHTML = '<tr>' + cols.map(function (c) { return SUM[c] ? '<td class="n">' + esc(fmtNum(total[c] || 0, FMT[c])) + '</td>' : '<td></td>'; }).join('') + '</tr>';
        $id('lblNavigator').textContent = 'Records: ' + idx.length + (idx.length !== st.rows.length ? ' of ' + st.rows.length : '');
    }

    /* grdfrm_LinkClicked :876 - ItemName -> frmTransactionReportWithValues_Engr, AccountTitle -> frmItemEvaluationLedger_Engr.
       Neither _Engr child form has a web page yet; the plain variants are opened with the same parameters. */
    function linkClicked(a) {
        var r = st.rows[+a.getAttribute('data-i')]; if (!r) return;
        if (a.getAttribute('data-link') === 'AccountTitle') {   // fromdate, todate, CmbItem = ItemId :926-939
            window.open('/stocks/item-ledger' + SC.qs({ itemId: num(r.ItemId), fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value }), '_blank');
        } else {   // :883-922
            var parents = checked('cmbMainTypeList').map(function (c) { return c.id; });
            var q = new URLSearchParams({ itemId: String(num(r.ItemId)), fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value,
                branchIds: branchIds(), saleValue: $id('RdSaleAmount').checked ? '1' : '0' });
            if (parents.length) q.set('parentIds', parents[0]);   // obj.cmbMainType.Value = lst[0] :902
            [['itemCategoryId', 'cmbCategory'], ['itemTypeId', 'cmbItemType'], ['warehouseId', 'cmbWareHouse'], ['itemStockAc', 'cmbStockAccounts']].forEach(function (pair) {
                var value = $id(pair[1]).value; if (num(value) > 0) q.set(pair[0], value);
            });
            window.open('/inventory/stock-transactions-with-value?' + q.toString(), '_blank');
        }
    }

    /* btnPrint_Click :1082 / tsDrowdown_DropDownItemClicked :1035 - Reporting.ShowReportWithDataTable(dtStockSum, rpt) + @CompanyName / @CompanyAddress */
    function printTable(rpt, emptyMessage) {
        if (!st.raw || !st.raw.length) { alert(emptyMessage); return; }
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify({ rpt: rpt, title: null, rows: st.raw }) })
            .then(function (r) { if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); }); return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); }); })
            .catch(function (e) { if (w) w.close(); alert(e.message); });
    }
    function btnPrint_Click() { if ($id('btnPrintText').textContent === RPT) printTable(RPT + '.rpt', 'Record Not Found For Display'); }

    /* btnRefresh_Click :978 - this form's "reset": FY start, clear combos, empty grid */
    function btnRefresh_Click() {
        if (st.init) $id('txtDateFrom').value = st.init.fyStart;
        ['cmbCategory', 'cmbItem', 'cmbItemType', 'cmbWareHouse'].forEach(function (id) { $id(id).value = ''; });
        $id('cmbMainTypeList').querySelectorAll('input').forEach(function (i) { i.checked = false; }); syncText('cmbMainTypeList', 'cmbMainTypeText');
        $id('txtDateFrom').focus();
        st.raw = []; st.rows = []; st.loaded = false; render();
        show('BtnUpdate', false);
    }

    /* frmStockReport_Load :174 */
    function load() {
        var q = new URLSearchParams(location.search);
        SC.getJson(API + '/init').then(function (d) {
            st.init = d; st.branches = d.branches || [];
            $id('ChkModalDescriptionWise').checked = true;   // :187
            $id('txtDateTo').value = SC.today();
            if (q.get('fromDate') && q.get('toDate')) { $id('txtDateFrom').value = q.get('fromDate'); $id('txtDateTo').value = q.get('toDate'); }   // :220
            else $id('txtDateFrom').value = d.fyStart;                                                                                           // :227
            $id('txtDateFrom').focus();
            BranchesFill(); ParentCategoryComboFill(d.combos); OtherComboByParentCategoryFill();
            $id('cmbReportType').value = 'ItemStockSummary';   // Rows[0].Activate() :234
            PrintButtonManage();
        }).catch(function (e) { alert(e.message); });
    }

    function showClick() { return SC.withBusy($id('btnshow'), function () { return GridBind().then(PrintButtonManage); }).catch(function (e) { alert(e.message); }); }
    function closeForm() { if (window.opener || history.length <= 1) window.close(); else history.back(); }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', showClick);
        $id('btnRefresh').addEventListener('click', btnRefresh_Click);
        $id('btnPrint').addEventListener('click', btnPrint_Click);
        $id('tsDrowdownBtn').addEventListener('click', function () { var l = $id('tsDrowdownList'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
        $id('tsDrowdownList').addEventListener('click', function (e) { var b = e.target.closest('button[data-rpt]'); if (b) printTable(b.getAttribute('data-rpt') + '.rpt', 'Record not found for Display'); });
        ['cmbBranchName', 'cmbMainType'].forEach(function (id) {
            $id(id).querySelector('.fx-multi-btn').addEventListener('click', function () { var l = $id(id + 'List'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
            $id(id + 'List').addEventListener('change', function (e) {
                if (e.target.classList.contains('all')) $id(id + 'List').querySelectorAll('input[data-id]').forEach(function (i) { i.checked = e.target.checked; });
                syncText(id + 'List', id + 'Text');
            });
        });
        document.addEventListener('mousedown', function (e) {
            if (!e.target.closest('.fx-dd')) $id('tsDrowdownList').style.display = 'none';
            if (e.target.closest('.fx-multi')) return;
            Array.prototype.forEach.call(document.querySelectorAll('.fx-multi-list'), function (l) {
                if (l.style.display !== 'block') return;
                l.style.display = 'none';
                if (l.id === 'cmbMainTypeList') cmbMainType_Leave();       // :536
                if (l.id === 'cmbBranchNameList') cmbBranchName_Leave();   // :518
            });
        });
        var grid = $id('grdfrm');
        grid.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (!k) return; st.filters[k] = e.target.value.trim().toLowerCase(); renderBody(); });
        grid.addEventListener('click', function (e) {
            var a = e.target.closest('a[data-link]'); if (a) { e.preventDefault(); linkClicked(a); return; }
            var tr = e.target.closest('tbody tr[data-i]'); if (tr) { st.sel = +tr.getAttribute('data-i'); grid.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); }
        });
        document.addEventListener('keydown', function (e) {   // frmStockReportWithValuesForStore_KeyDown :1107
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'p') { e.preventDefault(); btnPrint_Click(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); closeForm(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnRefresh_Click(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); showClick(); }
        });
        load();
    });
})();
