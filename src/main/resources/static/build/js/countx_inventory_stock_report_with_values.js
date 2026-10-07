/* frmStockReportWithValues (Inventory_Stocks_Report/frmStockReportWithValues.cs) - ScreenDefinition 299 "Stock Report With Values".
   Line refs ":NNN" are that file. API: /inventory/stock-report-with-values/api.
   Optional query (the desktop's RequestFromAnotherForm fields): fromDate, toDate, branchesIds, parentCategoryIds. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num, API = '/inventory/stock-report-with-values/api';

    /* ReportTypeFill :407 - Id values repeat, so options carry the row index. */
    var REPORT_TYPES = [
        ['ItemStockSummary', 'Item Stock Summary'],
        ['ItemandWarehouseStockSummary', 'Warehouse and Item Stock Summary'],
        ['ItemandWarehouseStockSummary', 'Item and WareHouse Stock Summary'],
        ['ItemandCropYearStockSummary', 'Crop Year and Item Stock Summary'],
        ['ItemandCropYearandWarehouseStockSummary', 'Crop Year, Warehouse and Item Stock Summary'],
        ['JobLotandItemStockSummary', 'JobLot and Item'],
        ['WarehouseandJoblotandItemStockSummary', 'Warehouse, Joblot and Item Stock Summary'],
        ['WarehouseandJoblotandItemStockSummary', 'Warehouse, Item and Joblot Stock Summary'],
        ['ItemandPackSizeStockSummary', 'Item and PackSize Stock Summary'],
        ['ItemandPackingTypeStockSummary', 'Item and PackingType Stock Summary'],
        ['ItemandPackSizeandPackingTypeStockSummary', 'Item and PackSize and PackingType Stock Summary'],
        ['ItemWarehouseJobCropPackingTypePackSizeStockSummary', 'All Filters Stock Summary']
    ];
    /* UOMFill :379 */
    var PACK_UOMS = [[1, '1KG'], [5, '5KG'], [10, '10KG'], [15, '15KG'], [20, '20KG'], [25, '25KG'], [40, '40KG'], [50, '50KG'], [60, '60KG'], [65, '65KG'], [80, '80KG'], [100, '100KG']];

    /* grdfrm designer layout (resx LayoutString): column sets in position order, column captions, GridSettings widths */
    var SETS = [
        { key: 'Item', caption: 'Item', cols: ['AccountTitle', 'ItemName'] },
        { key: 'Pack', caption: '', cols: ['PackUom'] },
        { key: 'PackingType', caption: '', cols: ['PackingType'] },
        { key: 'WareHouse', caption: '', cols: ['WareHouseName'] },
        { key: 'Crop', caption: '', cols: ['CropYear'] },
        { key: 'Job', caption: '', cols: ['JobLot'] },
        { key: 'Qty', caption: 'Qty', cols: ['OpQty', 'QtyIn', 'QtyOut', 'BalQty'] },
        { key: 'Weight', caption: 'Weight', cols: ['OpWeight', 'WeightIn', 'WeightOut', 'BalWeight'] },
        { key: 'Amount', caption: 'Amount', cols: ['OpAmount', 'AmountIn', 'AmountOut', 'BalAmount', 'AvgRate'] },
        { key: 'Custom', caption: 'Custom', cols: ['M_Rate', 'M_Amount'] }
    ];
    var CAPTION = { AccountTitle: 'Account', ItemName: 'Name', PackUom: 'UOM', PackingType: 'Packing Type', WareHouseName: 'Warehouse Name', CropYear: 'Crop Year', JobLot: 'Job Lot',
        OpQty: 'Opening', QtyIn: 'In', QtyOut: 'Out', BalQty: 'Balance', OpWeight: 'Opening', WeightIn: 'In', WeightOut: 'Out', BalWeight: 'Balance',
        OpAmount: 'Opening', AmountIn: 'In', AmountOut: 'Out', BalAmount: 'Balance', AvgRate: 'Avg Rate', M_Rate: 'Manual Rate / 40 Kg', M_Amount: 'Manual Amount' };
    var WIDTH = { AccountTitle: 150, ItemName: 150, PackUom: 60, PackingType: 115, WareHouseName: 150, CropYear: 73, JobLot: 80,
        OpQty: 70, QtyIn: 70, QtyOut: 70, BalQty: 70, OpWeight: 80, WeightIn: 80, WeightOut: 80, BalWeight: 80,
        OpAmount: 110, AmountIn: 110, AmountOut: 110, BalAmount: 110, AvgRate: 100, M_Rate: 80, M_Amount: 110 };
    /* number formats (GridSettings): '0' = "#,##0", '00' = "#,##00", 'rate' = AvgRateDecimalsChange ("#,##0.00/.000/.0000") */
    var FMT = { OpQty: '0', QtyIn: '0', QtyOut: '0', BalQty: '0', OpWeight: '0', WeightIn: '0', WeightOut: '0', BalWeight: '0',
        OpAmount: '0', AmountIn: '00', AmountOut: '00', BalAmount: '00', AvgRate: 'rate', M_Rate: 'rate', M_Amount: '00' };
    var SUM = { OpQty: 1, QtyIn: 1, QtyOut: 1, BalQty: 1, OpWeight: 1, WeightIn: 1, WeightOut: 1, BalWeight: 1, OpAmount: 1, AmountIn: 1, AmountOut: 1, BalAmount: 1, M_Amount: 1 };
    /* PrintButtonManage :1385 (both ItemandWarehouseStockSummary rows end on 190 - the 189 assignment is overwritten) */
    var PRINT_BY_VALUE = { ItemStockSummary: '188-ItemStockSummary', ItemandWarehouseStockSummary: '190-ItemandWarehouseStockSummary', ItemandCropYearStockSummary: '191-ItemandCropYearStockSummary',
        ItemandCropYearandWarehouseStockSummary: '192-ItemandCropYearandWarehouseStockSummary', JobLotandItemStockSummary: '193-JobLotandItemStockSummary',
        ItemandPackSizeStockSummary: '195-ItemandPackSizeStockSummary', ItemandPackingTypeStockSummary: '196-ItemandPackingTypeStockSummary',
        ItemandPackSizeandPackingTypeStockSummary: '197-ItemandPackSizeandPackingTypeStockSummary' };
    var PRINT_BY_TEXT = { 'Warehouse, Joblot and Item Stock Summary': '194-WarehouseandJoblotandItemStockSummary', 'Warehouse, Item and Joblot Stock Summary': '194A-WarehouseandItemandJoblotStockSummary' };
    var PRINTABLE = ['188-ItemStockSummary', '189-ItemandWarehouseStockSummary', '190-ItemandWarehouseStockSummary', '191-ItemandCropYearStockSummary', '192-ItemandCropYearandWarehouseStockSummary',
        '193-JobLotandItemStockSummary', '194-WarehouseandJoblotandItemStockSummary', '194A-WarehouseandItemandJoblotStockSummary', '195-ItemandPackSizeStockSummary',
        '196-ItemandPackingTypeStockSummary', '197-ItemandPackSizeandPackingTypeStockSummary'];

    var st = { init: null, combos: [], mainTypes: [], branches: [], raw: [], rows: [], lay: null, filters: {}, sel: -1 };

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
    function reportType() { var v = $id('cmbReportType').value; return v === '' ? null : { value: REPORT_TYPES[+v][0], text: REPORT_TYPES[+v][1] }; }
    function show(id, on) { $id(id).classList.toggle('is-hidden', !on); }

    /* BranchesFill :351 - checked list, Text = UserAccount.BranchName */
    function BranchesFill() {
        $id('cmbBranchNameList').innerHTML = multiHtml(st.branches, 'BranchId', 'BranchName', function (b) { return num(SC.ci(b, 'BranchId')) === num(st.init.userBranchId); });
        syncText('cmbBranchNameList', 'cmbBranchNameText');
    }
    /* ParentCategoryComboFill :610 - ActivityType 'ParentCategories', every row ticked */
    function ParentCategoryComboFill() {
        st.mainTypes = st.combos.filter(function (r) { return String(SC.ci(r, 'ActivityType')) === 'ParentCategories'; }).map(function (r) { return { Id: num(SC.ci(r, 'Id')), name: String(SC.ci(r, 'name')) }; });
        $id('cmbMainTypeList').innerHTML = multiHtml(st.mainTypes, 'Id', 'name', function () { return true; });
        syncText('cmbMainTypeList', 'cmbMainTypeText');
    }
    /* OtherComboByParentCategoryFill :479 - rows whose InventoryParentCategoriesId is a ticked parent category */
    function OtherComboByParentCategoryFill() {
        var valid = {}; checked('cmbMainTypeList').forEach(function (c) { valid[String(num(c.id))] = 1; });
        var byType = { ItemTypes: [], ItemCategories: [], Items: [], Warehouse: [], Stock_Account: [], ItemClassGroup: [], JobLot: [], CropYear: [], PackingType: [] }, seen = {}, any = false;
        if (!Object.keys(valid).length) return;
        st.combos.forEach(function (r) {
            if (!valid[String(num(SC.ci(r, 'InventoryParentCategoriesId')))]) return;
            any = true;
            var t = String(SC.ci(r, 'ActivityType')), id = num(SC.ci(r, 'Id'));
            if (!byType[t]) return; var k = t + ':' + id; if (seen[k]) return; seen[k] = 1;
            byType[t].push({ Id: id, name: String(SC.ci(r, 'name') == null ? '' : SC.ci(r, 'name')) });
        });
        if (!any) return;   // dtFiltered.Rows.Count <= 0 -> return
        fill('cmbItemType', byType.ItemTypes); fill('cmbCategory', byType.ItemCategories); fill('cmbItem', byType.Items); fill('cmbWareHouse', byType.Warehouse);
        fill('cmbStockAccounts', byType.Stock_Account); fill('CmbClassGroup', byType.ItemClassGroup); fill('cmbJobLot', byType.JobLot);
        fill('cmbCropYear', byType.CropYear); fill('cmbPacktype', byType.PackingType);
    }
    /* cmbMainType_Leave :438 */
    function cmbMainType_Leave() {
        if (checked('cmbMainTypeList').length) OtherComboByParentCategoryFill();
        else clearCombos(['cmbCategory', 'cmbItemType', 'cmbItem', 'cmbWareHouse', 'cmbStockAccounts', 'cmbCropYear', 'CmbClassGroup', 'cmbPacktype', 'cmbJobLot']);
    }
    function UOMFill() { fill('cmbPackUom', PACK_UOMS.map(function (u) { return { Id: u[0], name: u[1] }; })); }
    function ReportTypeFill() { var el = $id('cmbReportType'); el.innerHTML = '<option value=""></option>'; REPORT_TYPES.forEach(function (r, i) { el.appendChild(new Option(r[1], String(i))); }); }
    /* AccountFill3rdLevel :668 */
    function AccountFill3rdLevel(rows) { if (rows && rows.length) fill('cmbStockGroupAccount', rows.map(function (r) { return { Id: SC.ci(r, 'Id'), name: SC.ci(r, 'AccountTitle') }; })); }

    /* PrintButtonManage :1385 - the text only changes when a rule matches (the "All Filters" type keeps the previous text) */
    function PrintButtonManage() {
        var rt = reportType();
        if (!rt) { show('btnPrint', false); return; }
        show('btnPrint', true);
        var t = PRINT_BY_TEXT[rt.text] || PRINT_BY_VALUE[rt.value];
        if (t) $id('btnPrintText').textContent = t;
    }

    function params() {
        var rt = reportType(), itemWise = $id('ChkItemWiseStock').checked;
        return { fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value, activity: rt ? rt.value : '',
            itemTypeId: num($id('cmbItemType').value), itemCategoryId: num($id('cmbCategory').value), itemId: num($id('cmbItem').value),
            packingTypeId: num($id('cmbPacktype').value), warehouseId: num($id('cmbWareHouse').value), jobLotId: num($id('cmbJobLot').value),
            itemStockAc: num($id('cmbStockAccounts').value), groupAccountId: num($id('cmbStockGroupAccount').value), itemClassGroupId: num($id('CmbClassGroup').value),
            cropYear: selText('cmbCropYear'), saleValue: $id('RdSaleAmount').checked ? 1 : 0, stockUOM: itemWise ? 0 : 1, rateUOM: itemWise ? 0 : 1,
            zeroBalanceType: $id('chkSkipZero').checked ? 1 : 0, allowWipItem: $id('chkIncludeWipItem').checked ? 1 : 0,
            ids: checked('cmbMainTypeList').map(function (c) { return c.id; }).join(','),
            branchesIds: checked('cmbBranchNameList').map(function (c) { return c.id; }).join(',') };
    }

    /* GridBind :800 */
    function GridBind() {
        show('BtnUpdate', false);
        var p = params(), rt = reportType();
        if (!p.branchesIds) { $id('cmbBranchName').querySelector('button').focus(); return Promise.reject(new Error('Select Branch First')); }
        return SC.postJson(API + '/show', p).then(function (rows) {
            st.raw = rows || [];
            if (!st.raw.length) { st.rows = []; st.lay = null; render(); return; }   // grdfrm.DataSource = null
            var perKg = $id('rdAvgRatePerKg').checked;
            if (perKg) st.raw.forEach(function (r) { r.AvgRate = (Number(SC.ci(r, 'AvgRate')) || 0) / 40.0; });
            var v = rt ? rt.value : '';
            show('btnPrintStockSummeryByWeight', v === 'ItemStockSummary'); show('btnPrintStockReport', v === 'ItemStockSummary');
            st.rows = st.raw.map(function (r) {   // dtditinct :883
                var g = function (k) { return SC.ci(r, k); }, d = function (k) { return Number(g(k)) || 0; };
                var wh = '', cy = '', jl = '';
                if (v === 'ItemandWarehouseStockSummary' || v === 'ItemandCropYearandWarehouseStockSummary' || v === 'WarehouseandJoblotandItemStockSummary') wh = g('WareHouseName');
                if (v === 'ItemandCropYearStockSummary' || v === 'ItemandCropYearandWarehouseStockSummary') cy = g('CropYear');
                if (v === 'JobLotandItemStockSummary' || v === 'WarehouseandJoblotandItemStockSummary') jl = g('JobLot');
                if (v === 'ItemWarehouseJobCropPackingTypePackSizeStockSummary') { wh = g('WareHouseName'); cy = g('CropYear'); jl = g('JobLot'); }
                var iss = v === 'ItemStockSummary', mRate = iss ? d('ManualRate') : 0, mAmt = iss ? d('BalWeight') / 40.0 * mRate : 0;
                return { AccountId: g('AccountId'), AccountTitle: g('AccountTitle'), ItemId: g('ItemId'), ItemName: g('ItemName'),
                    PackUomId: iss ? g('PackUomId') : 0, PackUom: v === 'ItemandPackingTypeStockSummary' ? '' : g('PackUom'),
                    PackingTypeId: iss ? g('PackingTypeId') : 0, PackingType: v === 'ItemandPackSizeStockSummary' ? '' : g('PackingType'),
                    WareHouseName: wh == null ? '' : wh, CropYear: cy == null ? '' : cy, JobLot: jl == null ? '' : jl,
                    OpQty: d('OpQty'), QtyIn: d('QtyIn'), QtyOut: d('QtyOut'), BalQty: d('BalQty'), OpWeight: d('OpWeight'), WeightIn: d('WeightIn'), WeightOut: d('WeightOut'), BalWeight: d('BalWeight'),
                    OpAmount: d('OpAmount'), AmountIn: d('AmountIn'), AmountOut: d('AmountOut'), BalAmount: d('BalAmount'), AvgRate: d('AvgRate'),
                    M_Rate: mRate, M_Amount: mAmt, Saved_M_Rate: mRate };
            });
            GridSettings(rt, perKg);
        });
    }

    /* GridSettings :934 - visible column sets / columns, groups (GroupTotals Always) and the editable manual-rate column */
    function GridSettings(rt, perKg) {
        var v = rt ? rt.value : '', t = rt ? rt.text : '';
        var sets = { Item: true, Pack: false, PackingType: false, WareHouse: false, Crop: false, Job: false, Qty: true, Weight: true, Amount: true, Custom: false };
        var hidden = {}, groups = [], editRate = false;
        var manual = !!(st.init && st.init.manualRateConfig) && !$id('ChkItemWiseStock').checked;
        if (v === 'ItemStockSummary') {
            show('BtnUpdate', manual); editRate = manual; sets.Custom = manual; sets.Pack = true; sets.PackingType = true;
        } else if (t === 'Item and PackSize Stock Summary') { sets.Pack = true; }
        else if (t === 'Item and PackingType Stock Summary') { sets.PackingType = true; }
        else if (t === 'Item and PackSize and PackingType Stock Summary') { sets.Pack = true; sets.PackingType = true; }
        else if (t === 'Warehouse and Item Stock Summary') { sets.Pack = true; sets.PackingType = true; groups = ['WareHouseName']; }
        else if (t === 'Item and WareHouse Stock Summary') { sets.Pack = true; sets.PackingType = true; sets.WareHouse = true; sets.Item = false; groups = ['ItemName']; }
        else if (t === 'Crop Year and Item Stock Summary') { sets.Pack = true; sets.PackingType = true; groups = ['CropYear']; }
        else if (t === 'Crop Year, Warehouse and Item Stock Summary') { sets.Pack = true; sets.PackingType = true; groups = ['CropYear', 'WareHouseName']; }
        else if (t === 'Warehouse, Item and Joblot Stock Summary') { sets.Item = false; sets.WareHouse = false; sets.Pack = true; sets.PackingType = true; sets.Job = true; groups = ['WareHouseName', 'ItemName']; hidden.AccountTitle = 1; }
        else if (t === 'Warehouse, Joblot and Item Stock Summary') { sets.Pack = true; sets.PackingType = true; groups = ['WareHouseName', 'JobLot']; }
        else if (t === 'JobLot and Item') { sets.Pack = true; sets.PackingType = true; groups = ['JobLot']; }
        else if (v === 'ItemWarehouseJobCropPackingTypePackSizeStockSummary') { sets.WareHouse = true; sets.PackingType = true; sets.Pack = true; sets.Crop = true; sets.Job = true; }
        groups.forEach(function (g) { hidden[g] = 1; });
        st.lay = { sets: sets, hidden: hidden, groups: groups, editRate: editRate, avgCaption: perKg ? 'AvgRate/1Kg' : 'AvgRate/40Kg' };
        st.filters = {}; st.sel = -1;
        render();
    }

    function decimals() { return $id('rdAvgRateFourDecimals').checked ? 4 : $id('rdAvgRateThreeDecimals').checked ? 3 : 2; }
    function roundAway(x, d) { var f = Math.pow(10, d), s = x < 0 ? -1 : 1; return s * Math.round(Math.abs(x) * f + 1e-9) / f; }
    function fmtNum(v, kind) {
        v = Number(v) || 0;
        if (kind === 'rate') { var dd = decimals(); return roundAway(v, dd).toLocaleString('en-US', { minimumFractionDigits: dd, maximumFractionDigits: dd }); }
        var r = roundAway(v, 0), a = Math.abs(r).toLocaleString('en-US', { maximumFractionDigits: 0 });
        if (kind === '00' && Math.abs(r) < 10) a = '0' + a;
        return (r < 0 ? '-' : '') + a;
    }
    function visibleSets() {
        var out = [];
        if (!st.lay) return out;
        SETS.forEach(function (s) {
            if (!st.lay.sets[s.key]) return;
            var cols = s.cols.filter(function (c) { return !st.lay.hidden[c]; });
            if (cols.length) out.push({ set: s, cols: cols });
        });
        return out;
    }
    function visibleColumns() { var cols = []; visibleSets().forEach(function (g) { cols = cols.concat(g.cols); }); return cols; }
    function cellText(r, c) { return FMT[c] ? fmtNum(r[c], FMT[c]) : String(r[c] == null ? '' : r[c]); }
    function filteredRows() {
        var keys = Object.keys(st.filters).filter(function (k) { return st.filters[k]; }), out = [];
        st.rows.forEach(function (r, i) { if (keys.every(function (k) { return cellText(r, k).toLowerCase().indexOf(st.filters[k]) >= 0; })) out.push(i); });
        return out;
    }

    function render() {
        var t = $id('grdfrm'), vs = visibleSets();
        if (!st.lay || !st.rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; $id('lblNavigator').textContent = ''; return; }
        var cols = visibleColumns();
        var h1 = '<tr>' + vs.map(function (g) { return '<th colspan="' + g.cols.length + '">' + esc(g.set.caption) + '</th>'; }).join('') + '</tr>';
        var h2 = '<tr>' + cols.map(function (c) { return '<th data-col="' + c + '" style="min-width:' + WIDTH[c] + 'px;width:' + WIDTH[c] + 'px">' + esc(c === 'AvgRate' ? st.lay.avgCaption : CAPTION[c]) + '</th>'; }).join('') + '</tr>';
        var hf = '<tr class="flt">' + cols.map(function (c) { return '<td><input type="text" data-flt="' + c + '" value="' + esc(st.filters[c] || '') + '"></td>'; }).join('') + '</tr>';
        t.querySelector('thead').innerHTML = h1 + h2 + hf;
        renderBody();
    }
    function renderBody() {
        var t = $id('grdfrm'), cols = visibleColumns(), idx = filteredRows(), groups = st.lay.groups, html = '', total = {};
        var gt = groups.map(function () { return {}; }), prev = groups.map(function () { return null; });
        function sums(obj) { return cols.map(function (c) { return SUM[c] ? '<td class="n">' + esc(fmtNum(obj[c] || 0, FMT[c])) + '</td>' : '<td></td>'; }).join(''); }
        function closeFrom(level) { for (var L = groups.length - 1; L >= level; L--) { if (prev[L] !== null) html += '<tr class="gtot">' + sums(gt[L]) + '</tr>'; gt[L] = {}; prev[L] = null; } }
        function val(r, c) { return esc(r[c] == null ? '' : r[c]); }
        idx.forEach(function (i) {
            var r = st.rows[i];
            for (var L = 0; L < groups.length; L++) {
                var key = String(r[groups[L]] == null ? '' : r[groups[L]]);
                if (prev[L] !== key) {
                    closeFrom(L);
                    for (var M = L; M < groups.length; M++) {
                        var k2 = String(r[groups[M]] == null ? '' : r[groups[M]]);
                        html += '<tr class="grp"><td colspan="' + cols.length + '" style="padding-left:' + (4 + M * 16) + 'px">' + esc(CAPTION[groups[M]] + ': ' + k2) + '</td></tr>';
                        prev[M] = k2;
                    }
                    break;
                }
            }
            html += '<tr data-i="' + i + '"' + (i === st.sel ? ' class="sel"' : '') + '>' + cols.map(function (c) {
                if (c === 'M_Rate' && st.lay.editRate) return '<td class="ed"><input type="text" data-rate="' + i + '" value="' + esc(fmtNum(r.M_Rate, 'rate')) + '"></td>';
                if (c === 'AccountTitle') return '<td title="ItemEvaluationLedgerNew"><a data-link="AccountTitle" data-i="' + i + '">' + val(r, c) + '</a></td>';
                if (c === 'ItemName') return '<td title="Open Item Ledger with Value"><a data-link="ItemName" data-i="' + i + '">' + val(r, c) + '</a></td>';
                return FMT[c] ? '<td class="n">' + esc(cellText(r, c)) + '</td>' : '<td>' + esc(cellText(r, c)) + '</td>';
            }).join('') + '</tr>';
            cols.forEach(function (c) { if (SUM[c]) { var v = Number(r[c]) || 0; total[c] = (total[c] || 0) + v; gt.forEach(function (o) { o[c] = (o[c] || 0) + v; }); } });
        });
        closeFrom(0);
        t.querySelector('tbody').innerHTML = html;
        t.querySelector('tfoot').innerHTML = '<tr>' + sums(total) + '</tr>';
        $id('lblNavigator').textContent = 'Records: ' + idx.length + (idx.length !== st.rows.length ? ' of ' + st.rows.length : '');
    }

    /* grdfrm_CellUpdated :1277 - M_Amount = BalWeight / 40 * M_Rate when both > 0, else 0 */
    function rateChanged(input) {
        var i = +input.getAttribute('data-rate'), r = st.rows[i]; if (!r) return;
        var rate = Number(String(input.value).replace(/,/g, '')) || 0, w = Number(r.BalWeight) || 0;
        r.M_Rate = rate; r.M_Amount = (w > 0 && rate > 0) ? w / 40 * rate : 0;
        renderBody();
    }

    /* grdfrm_LinkClicked :1207 */
    function linkClicked(a) {
        var r = st.rows[+a.getAttribute('data-i')]; if (!r) return;
        if (a.getAttribute('data-link') === 'AccountTitle') {   // ItemEvaluationLedgerNew: fromdate, todate, CmbItem = ItemId
            window.open('/stocks/item-ledger' + SC.qs({ itemId: num(r.ItemId), fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value }), '_blank');
        } else {
            var q = new URLSearchParams({ itemId: String(num(r.ItemId)), fromDate: $id('txtDateFrom').value, toDate: $id('txtDateTo').value,
                branchIds: checked('cmbBranchNameList').map(function (c) { return c.id; }).join(','),
                saleValue: $id('RdSaleAmount').checked ? '1' : '0' });
            var parents = checked('cmbMainTypeList').map(function (c) { return c.id; }).join(',');
            if (parents) q.set('parentIds', parents);
            [['itemClassGroupId', 'CmbClassGroup'], ['itemCategoryId', 'cmbCategory'], ['itemTypeId', 'cmbItemType'],
                ['warehouseId', 'cmbWareHouse'], ['jobLotId', 'cmbJobLot'], ['itemStockAc', 'cmbStockAccounts']].forEach(function (pair) {
                var value = $id(pair[1]).value;
                if (value) q.set(pair[0], value);
            });
            var cropYear = selText('cmbCropYear'); if (cropYear) q.set('cropYear', cropYear);
            window.open('/inventory/stock-transactions-with-value?' + q.toString(), '_blank');
        }
    }

    /* InsertInCaseOfItemSummary :720 */
    function BtnUpdate_Click() {
        if (!st.rows.length) { alert('Grid Record Not Found'); return Promise.resolve(); }
        if (!confirm('Are you sure to Update?')) return Promise.resolve();
        var list = st.rows.filter(function (r) { return (Number(r.M_Amount) || 0) > 0 && Math.trunc(Number(r.M_Rate) || 0) > 0 && Math.trunc(Number(r.M_Rate) || 0) !== Math.trunc(Number(r.Saved_M_Rate) || 0); })
            .map(function (r) { return { ItemId: r.ItemId, PackingTypeId: r.PackingTypeId, PackUomId: r.PackUomId, BalQty: r.BalQty, BalWeight: r.BalWeight, M_Rate: r.M_Rate, M_Amount: r.M_Amount, Saved_M_Rate: r.Saved_M_Rate }; });
        if (!list.length) { alert('No Record Found For Update...'); return Promise.resolve(); }
        return SC.postJson(API + '/update', { toDate: $id('txtDateTo').value, rows: list }).then(function (res) {
            alert((res && res.message) || 'Data Updated Successfully.... ');
            return GridBind().catch(function (e) { alert(e.message); });
        }).catch(function (e) { alert(e.message); });
    }

    /* btnPrint_Click :1448 / btnPrintStockSummeryByWeight_Click :1613 / btnPrintStockReport_Click :1635 -
       Reporting.ShowReportWithDataTable(dtStockSum, rpt) + @CompanyName / @CompanyAddress (added by /reports/print/grid). */
    function printTable(rpt) {
        if (!st.raw || !st.raw.length) { alert('Record Not Found For Display'); return; }
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify({ rpt: rpt, title: null, rows: st.raw }) })
            .then(function (r) { if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); }); return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); }); })
            .catch(function (e) { if (w) w.close(); alert(e.message); });
    }
    function btnPrint_Click() { var t = $id('btnPrintText').textContent; if (PRINTABLE.indexOf(t) >= 0) printTable(t + '.rpt'); }

    function applyInit(d) { st.init = d; st.combos = d.combos || []; st.branches = d.branches || []; }
    /* btnRefresh_Click :1307 */
    function btnRefresh_Click() {
        return SC.getJson(API + '/init').then(function (d) { applyInit(d); BranchesFill(); ParentCategoryComboFill(); OtherComboByParentCategoryFill(); }).catch(function (e) { alert(e.message); });
    }
    /* BtnReset_Click :1338 */
    function BtnReset_Click() {
        if (st.init) $id('txtDateFrom').value = st.init.fyStart;
        ['cmbCategory', 'cmbCropYear', 'cmbItem', 'cmbItemType', 'cmbJobLot', 'cmbReportType', 'CmbClassGroup', 'cmbWareHouse'].forEach(function (id) { $id(id).value = ''; });
        $id('cmbMainTypeList').querySelectorAll('input').forEach(function (i) { i.checked = false; }); syncText('cmbMainTypeList', 'cmbMainTypeText');
        $id('txtDateFrom').focus(); show('BtnUpdate', false);
        return SC.getJson(API + '/as-on-date').then(function (d) { if (d && d.asOnDate) $id('txtDateFrom').value = d.asOnDate; }).catch(function (e) { alert(e.message); });
    }

    /* frmStockReport_Load :197 */
    function load() {
        var q = new URLSearchParams(location.search);
        SC.getJson(API + '/init').then(function (d) {
            applyInit(d);
            $id('txtDateTo').value = SC.today();
            if (q.get('fromDate') && q.get('toDate')) { $id('txtDateFrom').value = q.get('fromDate'); $id('txtDateTo').value = q.get('toDate'); }
            else $id('txtDateFrom').value = d.fyStart;
            if (d.asOnDate) $id('txtDateFrom').value = d.asOnDate;
            $id('txtDateFrom').focus();
            BranchesFill(); ParentCategoryComboFill(); OtherComboByParentCategoryFill(); UOMFill(); ReportTypeFill(); AccountFill3rdLevel(d.groupAccounts);
            $id('cmbReportType').value = '0';   // Rows[0].Activate() = "Item Stock Summary"
            if (q.has('branchesIds') || q.has('parentCategoryIds')) {   // RequestFromAnotherForm
                var b = (q.get('branchesIds') || '').split(',').filter(Boolean).map(num);
                $id('cmbBranchNameList').querySelectorAll('input[data-id]').forEach(function (i) { i.checked = !b.length || b.indexOf(num(i.getAttribute('data-id'))) >= 0; });
                syncText('cmbBranchNameList', 'cmbBranchNameText');
                var pc = (q.get('parentCategoryIds') || '').split(',').filter(Boolean).map(num);
                if (pc.length) { $id('cmbMainTypeList').querySelectorAll('input[data-id]').forEach(function (i) { i.checked = pc.indexOf(num(i.getAttribute('data-id'))) >= 0; }); syncText('cmbMainTypeList', 'cmbMainTypeText'); }
            }
            PrintButtonManage();
        }).catch(function (e) { alert(e.message); });
    }

    function showClick() { return SC.withBusy($id('btnshow'), function () { return GridBind().then(PrintButtonManage); }).catch(function (e) { alert(e.message); }); }
    function closeForm() { if (window.opener || history.length <= 1) window.close(); else history.back(); }

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnshow').addEventListener('click', showClick);
        $id('btnRefresh').addEventListener('click', function () { SC.withBusy($id('btnRefresh'), btnRefresh_Click); });
        $id('BtnReset').addEventListener('click', BtnReset_Click);
        $id('btnPrint').addEventListener('click', btnPrint_Click);
        $id('btnPrintStockSummeryByWeight').addEventListener('click', function () { printTable('188_01-ItemStockSummary.rpt'); });
        $id('btnPrintStockReport').addEventListener('click', function () { printTable('188_02-ItemStockSummary.rpt'); });
        $id('BtnUpdate').addEventListener('click', function () { SC.withBusy($id('BtnUpdate'), BtnUpdate_Click); });
        ['rdAvgRateTwoDecimals', 'rdAvgRateThreeDecimals', 'rdAvgRateFourDecimals'].forEach(function (id) { $id(id).addEventListener('change', function () { if (st.lay) renderBody(); }); });   // AvgRateDecimalsChange :1692
        ['cmbBranchName', 'cmbMainType'].forEach(function (id) {
            $id(id).querySelector('.fx-multi-btn').addEventListener('click', function () { var l = $id(id + 'List'); l.style.display = l.style.display === 'block' ? 'none' : 'block'; });
            $id(id + 'List').addEventListener('change', function (e) {
                if (e.target.classList.contains('all')) $id(id + 'List').querySelectorAll('input[data-id]').forEach(function (i) { i.checked = e.target.checked; });
                syncText(id + 'List', id + 'Text');
            });
        });
        document.addEventListener('mousedown', function (e) {
            if (e.target.closest('.fx-multi')) return;
            Array.prototype.forEach.call(document.querySelectorAll('.fx-multi-list'), function (l) {
                if (l.style.display !== 'block') return;
                l.style.display = 'none';
                if (l.id === 'cmbMainTypeList') cmbMainType_Leave();   // cmbBranchName_Leave :434 is empty
            });
        });
        var grid = $id('grdfrm');
        grid.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (!k) return; st.filters[k] = e.target.value.trim().toLowerCase(); renderBody(); });
        grid.addEventListener('change', function (e) { if (e.target.hasAttribute('data-rate')) rateChanged(e.target); });
        grid.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && e.target.hasAttribute('data-rate')) {
                e.preventDefault(); e.stopPropagation();
                var i = +e.target.getAttribute('data-rate'); rateChanged(e.target);
                var all = Array.prototype.slice.call(grid.querySelectorAll('input[data-rate]')), pos = all.findIndex(function (x) { return +x.getAttribute('data-rate') === i; });
                if (pos >= 0 && all[pos + 1]) { all[pos + 1].focus(); all[pos + 1].select(); }
            }
        });
        grid.addEventListener('click', function (e) {
            var a = e.target.closest('a[data-link]'); if (a) { e.preventDefault(); linkClicked(a); return; }
            var tr = e.target.closest('tbody tr[data-i]'); if (tr) { st.sel = +tr.getAttribute('data-i'); grid.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); }
        });
        document.addEventListener('keydown', function (e) {   // frmStockReportWithValues_KeyDown :1657
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && k === 'p') { e.preventDefault(); btnPrint_Click(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); closeForm(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); BtnReset_Click(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh_Click(); }
            else if (e.ctrlKey && k === 's') { e.preventDefault(); showClick(); }
        });
        load();
    });
})();
