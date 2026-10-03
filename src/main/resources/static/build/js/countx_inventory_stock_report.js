/* frmStockReport (Inventory_Stocks_Report/frmStockReport.cs) - Stock Report. Line refs ":NNN" are that file.
   Opened standalone or from the Sale Order / GDN / GRN "Stock Report" buttons (?itemId&warehouseId&cropYear&jobLotId&uomId&packingTypeId&dateTo
   = the desktop's RequestedbyOtherForm fields). API: /inventory/stock-report/api. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num, API = '/inventory/stock-report/api';
    var st = { init: null, combos: [], mainTypes: [], branches: [], rows: [], reportValue: '', reportText: '' };
    var REPORT_COLS = {
        ItemStockSummary: ['ParentCategory', 'ItemId', 'ItemName', 'PackUom', 'PackingType', 'TotalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'],
        ItemandWarehouseStockSummary: ['ParentCategory', 'ItemId', 'ItemName', 'WareHouseName', 'PackUom', 'PackingType', 'TotalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'],
        ItemandCropYearStockSummary: ['ParentCategory', 'ItemId', 'ItemName', 'CropYear', 'PackUom', 'PackingType', 'TotalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'],
        ItemandCropYearandWarehouseStockSummary: ['ParentCategory', 'ItemId', 'ItemName', 'WareHouseName', 'CropYear', 'PackUom', 'PackingType', 'TotalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'],
        JobLotandItemStockSummary: ['ParentCategory', 'ItemId', 'ItemName', 'JobLot', 'PackUom', 'PackingType', 'TotalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'],
        WarehouseandJoblotandItemStockSummary: ['ParentCategory', 'ItemId', 'ItemName', 'WareHouseName', 'JobLot', 'PackUom', 'PackingType', 'TotalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'],
        ItemandPackSizeStockSummary: ['ParentCategory', 'ItemId', 'ItemName', 'PackUom', 'TotalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'],
        ItemandPackingTypeStockSummary: ['ParentCategory', 'ItemId', 'ItemName', 'PackingType', 'TotalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'],
        ItemandPackSizeandPackingTypeStockSummary: ['ParentCategory', 'ItemId', 'ItemName', 'PackUom', 'PackingType', 'TotalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'],
        WIPStockPlantWiseSummary: ['ParentCategory', 'ItemId', 'WareHouseName', 'ItemName', 'TotalQty', 'OpWeight', 'WeightIn', 'WeightOut', 'BalWeight']
    };
    var SRC = { ParentCategory: 'CategoryDescription' };   // dtStockSum column -> grid column
    var NUM = { TotalQty: 1, OpWeight: 1, WeightIn: 1, WeightOut: 1, BalWeight: 1 };
    var WIDTH = { ParentCategory: 150, ItemName: 150, TotalQty: 85, OpWeight: 90, WeightIn: 90, WeightOut: 90, BalWeight: 95, WareHouseName: 120, PackUom: 70, PackingType: 110, CropYear: 80, JobLot: 110 };
    var PRINT = { ItemStockSummary: '178-ItemStockSummary', 'Item and WareHouse Stock Summary': '179-ItemandWarehouseStockSummary', 'Warehouse and Item Stock Summary': '180-ItemandWarehouseStockSummary',
        ItemandCropYearStockSummary: '181-ItemandCropYearStockSummary', ItemandCropYearandWarehouseStockSummary: '182-ItemandCropYearandWarehouseStockSummary', JobLotandItemStockSummary: '183-JobLotandItemStockSummary',
        WarehouseandJoblotandItemStockSummary: '184-WarehouseandJoblotandItemStockSummary', ItemandPackSizeStockSummary: '185-ItemandPackSizeStockSummary', ItemandPackingTypeStockSummary: '186-ItemandPackingTypeStockSummary',
        ItemandPackSizeandPackingTypeStockSummary: '187-ItemandPackSizeandPackingTypeStockSummary', WIPStockPlantWiseSummary: '198-WIPStockPlantWiseSummary' };

    function msg(t) { $id('fxMsg').textContent = t || ''; }
    function fmt(v) { v = Number(v) || 0; if (v === 0) return '00'; var r = Math.round(Math.abs(v)).toLocaleString('en-US'); return v < 0 ? '(' + r + ')' : r; }   // "#,#;(#,#);00"
    function fmtTotal(v) { v = Number(v) || 0; if (v === 0) return '0'; var r = Math.round(Math.abs(v)).toLocaleString('en-US'); return v < 0 ? '(' + r + ')' : r; }   // "#,#;(#,#);0"
    function checked(listId) { var out = []; $id(listId).querySelectorAll('input:checked').forEach(function (i) { out.push({ id: i.value, name: i.parentNode.textContent.trim() }); }); return out; }
    function syncText(listId, textId) { $id(textId).textContent = checked(listId).map(function (c) { return c.name; }).join(','); }
    function fill(id, rows, blank) { var el = $id(id), keep = el.value; el.innerHTML = ''; el.appendChild(new Option(blank || '', '')); rows.forEach(function (r) { el.appendChild(new Option(String(r.name), String(r.Id))); }); el.value = keep; if (el.value !== keep) el.value = ''; }
    function setText(id, text) { var el = $id(id), o = Array.from(el.options).find(function (x) { return x.textContent === String(text); }); el.value = o ? o.value : ''; }

    /** BranchesFill :420 */
    function BranchesFill() {
        $id('cmbBranchNameList').innerHTML = st.branches.map(function (b) { return '<label><input type="checkbox" value="' + esc(SC.ci(b, 'BranchId')) + '"' + (num(SC.ci(b, 'BranchId')) === num(st.init.userBranchId) ? ' checked' : '') + '> ' + esc(SC.ci(b, 'BranchName')) + '</label>'; }).join('');
        syncText('cmbBranchNameList', 'cmbBranchNameText');
    }
    /** ParentCategoryFill :447 - rows ActivityType='ParentCategories', all ticked */
    function ParentCategoryFill() {
        st.mainTypes = st.combos.filter(function (r) { return SC.ci(r, 'ActivityType') === 'ParentCategories'; }).map(function (r) { return { Id: num(SC.ci(r, 'Id')), name: String(SC.ci(r, 'name')) }; });
        $id('cmbMainCategoryList').innerHTML = st.mainTypes.map(function (m) { return '<label><input type="checkbox" value="' + esc(m.Id) + '" checked> ' + esc(m.name) + '</label>'; }).join('');
        syncText('cmbMainCategoryList', 'cmbMainCategoryText');
    }
    /** StockComboFill :504 - the other combos from the rows whose ParentCategoryId is among the ticked parent categories */
    function StockComboFill() {
        var valid = {}; checked('cmbMainCategoryList').forEach(function (c) { valid[c.id] = 1; });
        var byType = { ItemTypes: [], ItemCategories: [], Items: [], JobLot: [], CropYear: [], Warehouse: [], PackingType: [], ItemClassGroup: [] }, seen = {};
        if (Object.keys(valid).length) st.combos.forEach(function (r) {
            if (!valid[String(SC.ci(r, 'ParentCategoryId'))]) return;
            var t = String(SC.ci(r, 'ActivityType')), id = num(SC.ci(r, 'Id'));
            if (!byType[t]) return; var k = t + ':' + id; if (seen[k]) return; seen[k] = 1;
            byType[t].push({ Id: id, name: String(SC.ci(r, 'name')) });
        });
        if (!Object.keys(valid).length) return;   // the desktop returns before binding when nothing is filtered
        fill('cmbItemType', byType.ItemTypes); fill('cmbCategory', byType.ItemCategories); fill('cmbItem', byType.Items); fill('cmbJobLot', byType.JobLot);
        fill('cmbCropYear', byType.CropYear); fill('cmbWareHouse', byType.Warehouse); fill('cmbPacktype', byType.PackingType); fill('CmbClassGroup', byType.ItemClassGroup);
    }
    function clearCombos(ids) { ids.forEach(function (id) { $id(id).innerHTML = '<option value=""></option>'; }); }
    /** cmbItem_Leave :693 */
    function cmbItem_Leave() {
        var id = num($id('cmbItem').value);
        if (!id) { clearCombos(['cmbUOM']); return Promise.resolve(); }
        return SC.getJson(API + '/uoms?itemId=' + id).then(function (rows) {
            if (rows && rows.length) fill('cmbUOM', rows.map(function (r) { return { Id: SC.ci(r, 'Id'), name: SC.ci(r, 'PackUom') }; })); else clearCombos(['cmbUOM']);
        }).catch(function (e) { alert(e.message); });
    }
    function PrintButtonManage() {
        var v = $id('cmbReportType').value, t = $id('cmbReportType').selectedOptions[0] ? $id('cmbReportType').selectedOptions[0].textContent : '';
        if (!t) { $id('btnPrint').classList.add('is-hidden'); return; }
        $id('btnPrint').classList.remove('is-hidden');
        var name = v === 'ItemandWarehouseStockSummary' ? PRINT[t] : PRINT[v];
        $id('btnPrintText').textContent = name || 'Print';
    }
    function params() {
        var ids = ''; checked('cmbMainCategoryList').forEach(function (c) { ids += c.id + ','; });
        var branchIds = ''; checked('cmbBranchNameList').forEach(function (c) { branchIds += ',' + c.id; });
        var itemWise = $id('ChkItemWiseStock').checked;
        return { fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value, itemClassGroupId: num($id('CmbClassGroup').value), ids: ids, itemCategoryId: num($id('cmbCategory').value), itemTypeId: num($id('cmbItemType').value),
            itemId: num($id('cmbItem').value), cropYear: $id('cmbCropYear').selectedOptions[0] ? $id('cmbCropYear').selectedOptions[0].textContent : '', warehouseId: num($id('cmbWareHouse').value), packingTypeId: num($id('cmbPacktype').value),
            jobLotId: num($id('cmbJobLot').value), itemUomId: num($id('cmbUOM').value), activity: $id('cmbReportType').value, stockUOM: itemWise ? 0 : 1, rateUOM: itemWise ? 0 : 1,
            zeroBalanceType: $id('chkSikeZero').checked ? 1 : 0, allowWipItem: $id('chkIncludeWipItem').checked ? 1 : 0, branchesIds: branchIds };
    }
    /** GridBind :720 */
    function GridBind() {
        var p = params();
        if (!p.branchesIds) { $id('cmbBranchName').querySelector('button').focus(); throw new Error('Select branch first'); }
        return SC.postJson(API + '/show', p).then(function (rows) { st.rows = rows || []; st.reportValue = p.activity; st.reportText = $id('cmbReportType').selectedOptions[0] ? $id('cmbReportType').selectedOptions[0].textContent : ''; render(); PrintButtonManage(); });
    }
    /** grid per report type + GridSettings :1000 grouping */
    function render() {
        var t = $id('grdfrm'), cols = (REPORT_COLS[st.reportValue] || []).filter(function (c) { return c !== 'ItemId'; });
        var groups = [];
        switch (st.reportText) {
            case 'Warehouse and Item Stock Summary': groups = ['WareHouseName']; break;
            case 'Item and WareHouse Stock Summary': groups = ['ItemName']; break;
            case 'Crop Year and Item Stock Summary': groups = ['CropYear']; break;
            case 'Crop Year, Warehouse and Item Stock Summary': groups = ['CropYear', 'WareHouseName']; break;
            default: if (st.reportValue === 'WarehouseandJoblotandItemStockSummary') groups = ['WareHouseName', 'JobLot']; else if (st.reportValue === 'JobLotandItemStockSummary') groups = ['JobLot'];
        }
        var shown = cols.filter(function (c) { return groups.indexOf(c) < 0; });
        $id('lblRecords').textContent = st.rows.length ? 'Records: ' + st.rows.length : '';
        if (!st.rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; return; }
        t.querySelector('thead').innerHTML = '<tr>' + shown.map(function (c) { return '<th style="min-width:' + (WIDTH[c] || 90) + 'px">' + esc(c) + '</th>'; }).join('') + '</tr>';
        var q = new URLSearchParams(location.search);
        function cell(r, c) {
            var v = SC.ci(r, SRC[c] || c);
            if (NUM[c]) return '<td class="n">' + esc(fmt(v)) + '</td>';
            if (c === 'ItemName') return '<td><a class="cx-link" href="/inventory/stock-transactions?' + SC.qs({ itemId: num(SC.ci(r, 'ItemId')), fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value, warehouseId: num($id('cmbWareHouse').value), jobLotId: num($id('cmbJobLot').value) }) + '" target="_blank" title="InventoryTransactionReport">' + esc(v) + '</a></td>';
            if (c === 'ParentCategory') return '<td title="ItemLedgerWithoutValue (item ledger without value) is not ported on the web">' + esc(v) + '</td>';
            return '<td>' + esc(v == null ? '' : v) + '</td>';
        }
        var html = '', totals = {}, gtot = {}, prevKey = null;
        function sumRow(cls, label, tot) { return '<tr class="' + cls + '">' + shown.map(function (c, i) { return NUM[c] ? '<td class="n">' + esc(fmtTotal(tot[c] || 0)) + '</td>' : '<td>' + (i === 0 ? esc(label) : '') + '</td>'; }).join('') + '</tr>'; }
        st.rows.forEach(function (r, i) {
            var key = groups.map(function (g) { return String(SC.ci(r, g) == null ? '' : SC.ci(r, g)); }).join(' / ');
            if (groups.length && key !== prevKey) { if (prevKey !== null) html += sumRow('gtot', 'Total', gtot); gtot = {}; html += '<tr class="grp"><td colspan="' + shown.length + '">' + esc(key) + '</td></tr>'; prevKey = key; }
            html += '<tr>' + shown.map(function (c) { return cell(r, c); }).join('') + '</tr>';
            shown.forEach(function (c) { if (NUM[c]) { var v = Number(SC.ci(r, c)) || 0; totals[c] = (totals[c] || 0) + v; gtot[c] = (gtot[c] || 0) + v; } });
        });
        if (groups.length && prevKey !== null) html += sumRow('gtot', 'Total', gtot);
        t.querySelector('tbody').innerHTML = html;
        t.querySelector('tfoot').innerHTML = '<tr>' + shown.map(function (c) { return '<td>' + (NUM[c] ? esc(fmtTotal(totals[c] || 0)) : '') + '</td>'; }).join('') + '</tr>';
    }
    /** btnPrint_Click :1264 - the same rows through the report's .rpt */
    function btnPrint_Click() {
        if (!st.rows.length) { alert('Record Not Found For Display'); return; }
        var rpt = $id('btnPrintText').textContent + '.rpt', p = params();
        var w = window.open('', '_blank');
        fetch('/reports/print/by-template/' + encodeURIComponent(rpt) + '/pdf', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify(p) })
            .then(function (r) { if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); }); return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); }); })
            .catch(function (e) { if (w) w.close(); alert(e.message); });
    }
    /** btnRefresh_Click :1129 */
    function btnRefresh_Click() {
        ['cmbCategory', 'cmbCropYear', 'cmbItem', 'cmbItemType', 'cmbJobLot', 'cmbUOM', 'cmbWareHouse', 'cmbReportType'].forEach(function (id) { $id(id).value = ''; });
        $id('cmbMainCategoryList').querySelectorAll('input').forEach(function (i) { i.checked = false; }); syncText('cmbMainCategoryList', 'cmbMainCategoryText');
        $id('txtDateFrom').focus(); st.rows = []; render();
        return SC.getJson(API + '/init').then(function (d) {
            st.init = d; st.combos = d.combos || []; st.branches = d.branches || [];
            $id('txtDateFrom').value = d.asOnDate || d.fyStart;
            BranchesFill(); ParentCategoryFill(); StockComboFill(); PrintButtonManage();
        }).catch(function (e) { alert(e.message); });
    }
    /** frmStockReport_Load :203 */
    function load() {
        var q = new URLSearchParams(location.search), other = q.has('itemId');
        SC.getJson(API + '/init').then(function (d) {
            st.init = d; st.combos = d.combos || []; st.branches = d.branches || [];
            var dateTo = q.get('dateTo') || '';
            if (!dateTo) { $id('txtDateFrom').value = d.fyStart; $id('txtDateFrom').focus(); $id('txtDateTo').value = SC.today(); }
            else { var f = new Date(); f.setMonth(f.getMonth() - 1); $id('txtDateFrom').value = SC.isoDay(f); $id('txtDateTo').value = dateTo; }
            if (d.asOnDate) $id('txtDateFrom').value = d.asOnDate;
            fill('cmbReportType', d.reportTypes);
            BranchesFill(); ParentCategoryFill(); StockComboFill();
            var c = d.config || {}, fifo = !!d.fifoFeature, chain = Promise.resolve();
            if (other) {
                var set = function (lvl) {
                    $id('cmbItem').value = q.get('itemId') || ''; $id('cmbWareHouse').value = q.get('warehouseId') || '';
                    if (lvl >= 2) setText('cmbCropYear', q.get('cropYear') || '');
                    if (lvl >= 3) $id('cmbJobLot').value = q.get('jobLotId') || '';
                    if (lvl >= 4) chain = cmbItem_Leave().then(function () { $id('cmbUOM').value = q.get('uomId') || ''; if (lvl >= 5) $id('cmbPacktype').value = q.get('packingTypeId') || ''; });
                };
                if (!fifo) { if (c.CheckStockItemWareHouse) set(1); else if (c.CheckStockItemWareHouseCropYear) set(2); else if (c.CheckStockItemWareHouseCropYearJobLot) set(3); else if (c.CheckStockItemWareHouseCropYearJobLotPackUom) set(4); else if (c.CheckStockItemWareHouseCropYearJobLotPackingTypeUom) set(5); }
                else set(5);
            }
            $id('cmbReportType').selectedIndex = 1;   // Rows[0] = "Item Stock Summary"
            chain.then(function () { PrintButtonManage(); if ($id('cmbReportType').selectedOptions[0].textContent === 'Item Stock Summary') return GridBind(); }).catch(function (e) { alert(e.message); });
        }).catch(function (e) { alert(e.message); msg(e.message); });
    }
    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', function () { SC.withBusy($id('btnshow'), function () { return GridBind().then(PrintButtonManage); }).catch(function (e) { alert(e.message); }); });
        $id('btnRefresh').addEventListener('click', function () { SC.withBusy($id('btnRefresh'), btnRefresh_Click); });
        $id('btnPrint').addEventListener('click', btnPrint_Click);
        $id('btnclose').addEventListener('click', function () { if (window.opener || history.length <= 1) window.close(); else history.back(); });
        $id('cmbItem').addEventListener('change', cmbItem_Leave);
        $id('cmbReportType').addEventListener('change', PrintButtonManage);
        ['cmbBranchName', 'cmbMainCategory'].forEach(function (id) { $id(id).querySelector('.fx-multi-btn').addEventListener('click', function () { var l = $id(id + 'List'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; }); });
        $id('cmbBranchNameList').addEventListener('change', function () { syncText('cmbBranchNameList', 'cmbBranchNameText'); });
        $id('cmbMainCategoryList').addEventListener('change', function () { syncText('cmbMainCategoryList', 'cmbMainCategoryText'); });
        document.addEventListener('mousedown', function (e) {
            if (e.target.closest('.fx-multi')) return;
            var wasOpen = Array.from(document.querySelectorAll('.fx-multi-list')).filter(function (l) { return l.style.display === 'block'; });
            wasOpen.forEach(function (l) { l.style.display = 'none'; });
            /* cmbBranchName_Leave :1230 / cmbMainCategory_Leave :1472 */
            wasOpen.forEach(function (l) {
                if (l.id === 'cmbBranchNameList') { if (checked('cmbBranchNameList').length) StockComboFill(); else clearCombos(['CmbClassGroup', 'cmbCategory', 'cmbItemType', 'cmbPacktype', 'cmbCropYear', 'cmbItem', 'cmbWareHouse', 'cmbJobLot']); }
                if (l.id === 'cmbMainCategoryList') { if (checked('cmbMainCategoryList').length) StockComboFill(); else clearCombos(['cmbCategory', 'cmbItemType', 'cmbItem', 'cmbWareHouse', 'cmbCropYear', 'CmbClassGroup', 'cmbPacktype', 'cmbJobLot']); }
            });
        });
        document.addEventListener('keydown', function (e) {   // :1429
            var k = e.key.toLowerCase();
            if (e.ctrlKey && k === 'p') { e.preventDefault(); btnPrint_Click(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { $id('btnclose').click(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnRefresh_Click(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); $id('btnshow').click(); }
        });
        load();
    });
})();
