/* frmStockEvaluationCgsReport (Inventory_Stocks_Report/frmStockEvaluationCgsReport.cs) - ScreenDefinition 874 "Stock Evaluation Cgs Report".
   Line refs ":NNN" are that file. API: /inventory/reports/stock-evaluation-cgs-report/api.
   Optional query (RequestFromAnotherForm fields :25-29): fromDate, toDate, supplierCustomerId, itemId. */
(function () {
    'use strict';
    var SC = window.StoreCommon, $id = SC.$id, esc = SC.esc, num = SC.num, API = '/inventory/reports/stock-evaluation-cgs-report/api';

    /* CommonServices.DateType() (Datetypefill :278) - Id / Parameters */
    var DATE_TYPES = [[1, 'This Day'], [2, 'This Week'], [3, 'This Month'], [4, 'This Year'], [5, 'Financial Year']];
    /* ActivityFill :411 */
    var ACTIVITIES = [[1, 'Detail'], [2, 'Summary']];
    /* AllComboBind :346 - Activity value of the ComboData rows per combo, placeholder text (BindComboBox :332) */
    var COMBOS = [
        ['CmbParentCategory', 'GetParentCategory', 'Parent Category'], ['cmbItemtype', 'GetItemType', 'Item Type'], ['cmbItemCatgory', 'GetItemCategory', 'Item Category'],
        ['cmbItemclass', 'GetItemClass', 'Item Class'], ['cmbItem', 'GetItems', 'Item Name'], ['cmbSupplierCustomer', 'GetSupplierCustomer', 'Party Name'],
        ['cmbCity', 'GetCity', 'City Name'], ['cmbWareHouse', 'GetWarehouse', 'Warehouse'], ['CmbDocumentType', 'GetDocumentType', 'Document Type'],
        ['cmbJobLot', 'GetJobLot', 'Job Lot'], ['cmbPacktype', 'GetPackingType', 'Packing Type'], ['cmbCropYear', 'GetCropYear', 'Crop Year']];

    /* EnsureSalesRegisterSchema :620 - dtSalesRegister columns in order; hidden ids per DataGridHistorySetting :684-694 */
    var COLS = ['DocumentTypeId', 'DocumentType', 'Id', 'InvoiceDate', 'InvoiceNo', 'SupplierCustomerId', 'CustomerName', 'InvoiceDetailId', 'WarehouseId', 'WareHouseName',
        'ItemId', 'ItemName', 'ItemCode', 'CropYear', 'JobLotId', 'JobLot', 'PackingTypeId', 'PackingType', 'ItemUOMId', 'ItemUom', 'SaleQty', 'SaleWeight', 'ItemRate', 'ItemRateKg',
        'RateUomId', 'RateUom', 'ItemAmount', 'ItemNetAmount', 'CgsRate', 'CgsRateKg', 'CgsAmount', 'GrossProfitLoss', 'DueDate', 'DueDays', 'RemarksHeader', 'CommissionAgentId',
        'CommissionAgent', 'CommissionType', 'CommRate', 'CommAmount', 'CommissionRemarks', 'CityName', 'VehicleNo', 'GpDate', 'GpNo'];
    var ID_COLS = { Id: 1, DocumentTypeId: 1, SupplierCustomerId: 1, CommissionAgentId: 1, InvoiceDetailId: 1, WarehouseId: 1, ItemId: 1, JobLotId: 1, PackingTypeId: 1, ItemUOMId: 1, RateUomId: 1 };
    var DATE_COLS = { InvoiceDate: 1, DueDate: 1, GpDate: 1 };
    /* GridWrappingAndColumnSettings(DataGridHistory, 2, 2): numbers with 2 decimals; totals on the amount / qty columns */
    var NUM_COLS = { SaleQty: 1, SaleWeight: 1, ItemRate: 1, ItemRateKg: 1, ItemAmount: 1, ItemNetAmount: 1, CgsRate: 1, CgsRateKg: 1, CgsAmount: 1, GrossProfitLoss: 1, CommRate: 1, CommAmount: 1, DueDays: 0, GpNo: 0 };
    var SUM = { SaleQty: 1, SaleWeight: 1, ItemAmount: 1, ItemNetAmount: 1, CgsAmount: 1, GrossProfitLoss: 1, CommAmount: 1 };
    /* Constants.InventoryConstants widths (:702-735) - the constants class is not in the recovered source, widths are representative */
    var WIDTH = { DocumentType: 120, InvoiceDate: 80, InvoiceNo: 70, CustomerName: 180, DueDate: 80, DueDays: 55, RemarksHeader: 160, CommissionAgent: 140, CommissionType: 80, CommRate: 70, CommAmount: 95,
        CommissionRemarks: 160, WareHouseName: 130, ItemName: 180, ItemCode: 80, CropYear: 70, JobLot: 80, PackingType: 100, ItemUom: 60, SaleQty: 75, SaleWeight: 85, ItemRate: 70, ItemRateKg: 70,
        RateUom: 60, ItemAmount: 95, ItemNetAmount: 110, CityName: 110, VehicleNo: 90, GpDate: 80, GpNo: 60, CgsRate: 70, CgsRateKg: 70, CgsAmount: 95, GrossProfitLoss: 110 };
    var CAPTION = { DocumentType: 'Document Type', InvoiceDate: 'Invoice Date', InvoiceNo: 'Invoice No', CustomerName: 'Customer Name', WareHouseName: 'WareHouse Name', ItemName: 'Item Name', ItemCode: 'Item Code',
        CropYear: 'Crop Year', JobLot: 'Job Lot', PackingType: 'Packing Type', ItemUom: 'Item Uom', SaleQty: 'Sale Qty', SaleWeight: 'Sale Weight', ItemRate: 'Item Rate', ItemRateKg: 'Item Rate Kg', RateUom: 'Rate Uom',
        ItemAmount: 'Item Amount', ItemNetAmount: 'Item Net Amount', CgsRate: 'Cgs Rate', CgsRateKg: 'Cgs Rate Kg', CgsAmount: 'Cgs Amount', GrossProfitLoss: 'Gross Profit Loss', DueDate: 'Due Date', DueDays: 'Due Days',
        RemarksHeader: 'Remarks Header', CommissionAgent: 'Commission Agent', CommissionType: 'Commission Type', CommRate: 'Comm Rate', CommAmount: 'Comm Amount', CommissionRemarks: 'Commission Remarks',
        CityName: 'City Name', VehicleNo: 'Vehicle No', GpDate: 'Gp Date', GpNo: 'Gp No' };

    var st = { init: null, combos: [], dtGrid: [], rows: [], visible: [], filters: {}, sel: -1, loaded: false };

    function fill(id, rows, blank) {
        var el = $id(id), keep = el.value; el.innerHTML = ''; el.appendChild(new Option(blank || '', ''));
        rows.forEach(function (r) { el.appendChild(new Option(String(r.name == null ? '' : r.name), String(r.Id))); });
        el.value = keep; if (el.value !== keep) el.value = '';
    }
    function selText(id) { var o = $id(id).selectedOptions[0]; return o && o.value !== '' ? o.textContent : ''; }
    function show(id, on) { $id(id).classList.toggle('is-hidden', !on); }
    function activity() { return selText('cmbActivity'); }

    /* Datetypefill :278 - BindDDL(ZeroIndex false): no empty row is selected by the desktop; the web keeps a blank option */
    function Datetypefill() { fill('cmbDateType', DATE_TYPES.map(function (d) { return { Id: d[0], name: d[1] }; })); }
    function ActivityFill() { fill('cmbActivity', ACTIVITIES.map(function (d) { return { Id: d[0], name: d[1] }; })); }
    /* FilterData :320 + BindComboBox :332 - rows of one Activity, Id / RefName; an empty set leaves the combo untouched (BindComboBox: Rows.Count > 0) */
    function AllComboBind(rows) {
        st.combos = rows || [];
        if (!st.combos.length) { COMBOS.forEach(function (c) { $id(c[0]).innerHTML = '<option value=""></option>'; }); return; }   // :377-402
        COMBOS.forEach(function (c) {
            var list = st.combos.filter(function (r) { return String(SC.ci(r, 'Activity')) === c[1]; }).map(function (r) { return { Id: SC.ci(r, 'Id'), name: SC.ci(r, 'RefName') }; });
            if (list.length) fill(c[0], list);
        });
    }

    /* cmbDateType_ValueChanged :428 */
    function cmbDateType_ValueChanged() {
        var v = num($id('cmbDateType').value), today = SC.today(), n = new Date();
        function iso(d) { return d.getFullYear() + '-' + String(d.getMonth() + 1).padStart(2, '0') + '-' + String(d.getDate()).padStart(2, '0'); }
        if (v === 1) $id('txtDateFrom').value = today;
        else if (v === 2) { var d = new Date(); d.setDate(d.getDate() - 7); $id('txtDateFrom').value = iso(d); }
        else if (v === 3) { $id('txtDateFrom').value = n.getUTCFullYear() + '-' + String(n.getUTCMonth() + 1).padStart(2, '0') + '-01'; $id('txtToDate').value = today; }   // DateTime.UtcNow
        else if (v === 4) { $id('txtDateFrom').value = n.getFullYear() + '-01-01'; $id('txtToDate').value = today; }
        else if (v === 5 && st.init) $id('txtDateFrom').value = st.init.fyStart;
    }

    /* cmbActivity_TextChanged :823 */
    function cmbActivity_TextChanged() {
        var t = activity();
        if (t === '') return;
        if (t === 'Summary') { $id('panel3').style.height = '235px'; show('tsDrowdown', true); show('btnPrint', false); }
        else { show('tsDrowdown', false); show('btnPrint', true); $id('panel3').style.height = '188px'; }
    }

    /* ReportsLoad :259 - tsDrowdown items */
    function ReportsLoad(files) {
        var l = $id('tsDrowdownList');
        l.innerHTML = (files && files.length) ? files.map(function (f) { return '<button type="button" data-rpt="' + esc(f) + '">' + esc(f) + '</button>'; }).join('') : '<div class="empty">No report in EvaluationCgsReport</div>';
    }

    function params() {
        return { fromDate: $id('txtDateFrom').value, toDate: $id('txtToDate').value, fromDocNo: num($id('txtFromDocNo').value), toDocNo: num($id('txtToDocNo').value),
            itemClassId: num($id('cmbItemclass').value), parentCategoryId: num($id('CmbParentCategory').value), itemCategoryId: num($id('cmbItemCatgory').value),
            itemTypeId: num($id('cmbItemtype').value), itemId: num($id('cmbItem').value), warehouseId: num($id('cmbWareHouse').value), cropYear: selText('cmbCropYear'),
            jobLotId: num($id('cmbJobLot').value), packingTypeId: num($id('cmbPacktype').value), documentTypeId: num($id('CmbDocumentType').value),
            supplierCustomerId: num($id('cmbSupplierCustomer').value), cityId: num($id('cmbCity').value), reportType: activity(),
            documentTypeWise: $id('chkAddDocumentType').checked, documentWise: $id('chkAddDoc').checked, customerWise: $id('chkAddCustomer').checked, warehouseWise: $id('chkAddWarehouse').checked,
            itemWise: $id('ChkAddItem').checked, cropYearWise: $id('chkAddCrop').checked, jobLotWise: $id('chkaddJob').checked, packingTypeWise: $id('chkAddPackingType').checked,
            uomWise: $id('ChkAddUom').checked, cityWise: $id('ChkAddCity').checked };
    }

    /* GridFill :483 */
    function GridFill() {
        var p = params();
        if (p.reportType === 'Summary' && !(p.cityWise || p.warehouseWise || p.documentWise || p.uomWise || p.documentTypeWise || p.customerWise || p.itemWise || p.cropYearWise || p.jobLotWise || p.packingTypeWise)) {
            return Promise.reject(new Error('Please Add Atleast One Check!'));   // :559
        }
        return SC.postJson(API + '/show', p).then(function (rows) {
            st.dtGrid = rows || [];
            st.rows = []; st.filters = {}; st.sel = -1;
            if (!st.dtGrid.length) { st.visible = []; render(); return; }   // DataGridHistory.ClearStructure()
            var summary = p.reportType === 'Summary';
            st.rows = st.dtGrid.map(function (r) { return AddSalesRegisterRow(r, summary); });
            DataGridHistorySetting(summary);
        });
    }
    /* IsAdjustmentRequired :589 / AdjustValue :598 - Detail rows of DocumentTypeId 1662 are negated on the grid (not on the print DataTable) */
    function AddSalesRegisterRow(r, summary) {
        var adj = !summary && num(SC.ci(r, 'DocumentTypeId')) === 1662, out = {};
        COLS.forEach(function (c) {
            var v = SC.ci(r, c);
            if (c === 'SaleQty' || c === 'SaleWeight' || c === 'ItemAmount' || c === 'ItemNetAmount' || c === 'CgsAmount' || c === 'GrossProfitLoss') { v = Number(v) || 0; if (adj) v = 0 - v; }
            out[c] = v == null ? '' : v;
        });
        return out;
    }
    /* DataGridHistorySetting :680 - visible columns */
    function DataGridHistorySetting(summary) {
        var hide = {};
        Object.keys(ID_COLS).forEach(function (c) { hide[c] = 1; });
        if (summary) {
            hide.GpNo = 1; hide.GpDate = 1; hide.VehicleNo = 1;   // :738-740
            if (!$id('chkAddDocumentType').checked) hide.DocumentType = 1;
            if (!$id('chkAddDoc').checked) { hide.InvoiceNo = 1; hide.InvoiceDate = 1; }
            ['DueDate', 'DueDays', 'RemarksHeader', 'CommissionAgent', 'CommissionType', 'CommRate', 'CommAmount', 'CommissionRemarks', 'ItemRate', 'ItemRateKg', 'RateUom', 'CgsRate', 'CgsRateKg'].forEach(function (c) { hide[c] = 1; });
            if (!$id('ChkAddItem').checked) { hide.ItemName = 1; hide.ItemCode = 1; }
            if (!$id('ChkAddUom').checked) hide.ItemUom = 1;
            if (!$id('chkAddWarehouse').checked) hide.WareHouseName = 1;
            if (!$id('chkAddCustomer').checked) hide.CustomerName = 1;
            if (!$id('chkAddCrop').checked) hide.CropYear = 1;
            if (!$id('chkaddJob').checked) hide.JobLot = 1;
            if (!$id('chkAddPackingType').checked) hide.PackingType = 1;
            if (!$id('ChkAddCity').checked) hide.CityName = 1;
        }
        st.visible = COLS.filter(function (c) { return !hide[c]; });
        render();
    }

    function fmtNum(v, d) { v = Number(v) || 0; return v.toLocaleString('en-US', { minimumFractionDigits: d, maximumFractionDigits: d }); }
    function fmtDate(v) { if (!v) return ''; var s = String(v); return s.length >= 10 ? SC.gridDate(s) : s; }
    function cellText(r, c) {
        if (NUM_COLS[c] !== undefined) return fmtNum(r[c], NUM_COLS[c]);
        if (DATE_COLS[c]) return fmtDate(r[c]);
        return String(r[c] == null ? '' : r[c]);
    }
    function filteredRows() {
        var keys = Object.keys(st.filters).filter(function (k) { return st.filters[k]; }), out = [];
        st.rows.forEach(function (r, i) { if (keys.every(function (k) { return cellText(r, k).toLowerCase().indexOf(st.filters[k]) >= 0; })) out.push(i); });
        return out;
    }
    function render() {
        var t = $id('DataGridHistory');
        if (!st.rows.length) { t.querySelector('thead').innerHTML = ''; t.querySelector('tbody').innerHTML = ''; t.querySelector('tfoot').innerHTML = ''; $id('lblNavigator').textContent = ''; return; }
        var cols = st.visible;
        t.querySelector('thead').innerHTML = '<tr>' + cols.map(function (c) { return '<th data-col="' + c + '" style="min-width:' + (WIDTH[c] || 90) + 'px;width:' + (WIDTH[c] || 90) + 'px">' + esc(CAPTION[c] || c) + '</th>'; }).join('') + '</tr>'
            + '<tr class="flt">' + cols.map(function (c) { return '<td><input type="text" data-flt="' + c + '" value="' + esc(st.filters[c] || '') + '"></td>'; }).join('') + '</tr>';
        renderBody();
    }
    function renderBody() {
        var t = $id('DataGridHistory'), cols = st.visible, idx = filteredRows(), html = '', total = {};
        idx.forEach(function (i) {
            var r = st.rows[i];
            html += '<tr data-i="' + i + '"' + (i === st.sel ? ' class="sel"' : '') + '>' + cols.map(function (c) {
                var cls = NUM_COLS[c] !== undefined ? 'n' : '';
                if (c === 'GrossProfitLoss') { var g = Number(r[c]) || 0; cls += g > 0 ? ' pos' : g < 0 ? ' neg' : ''; }   // AddConditionalFormatting :696
                return '<td' + (cls ? ' class="' + cls.trim() + '"' : '') + '>' + esc(cellText(r, c)) + '</td>';
            }).join('') + '</tr>';
            cols.forEach(function (c) { if (SUM[c]) total[c] = (total[c] || 0) + (Number(r[c]) || 0); });
        });
        t.querySelector('tbody').innerHTML = html;
        t.querySelector('tfoot').innerHTML = '<tr>' + cols.map(function (c) { return SUM[c] ? '<td class="n">' + esc(fmtNum(total[c] || 0, 2)) + '</td>' : '<td></td>'; }).join('') + '</tr>';
        $id('lblNavigator').textContent = 'Records: ' + idx.length + (idx.length !== st.rows.length ? ' of ' + st.rows.length : '');
    }

    /* btnPrint_Click :800 / tsDrowdown_DropDownItemClicked :849 - Reporting.ShowReportWithDataTable(dtGrid, rpt) + @CompanyName / @CompanyAddress (added by /reports/print/grid) */
    function printTable(rpt, emptyMsg) {
        if (!st.dtGrid || !st.dtGrid.length) { alert(emptyMsg); return; }
        var w = window.open('', '_blank');
        fetch('/reports/print/grid', { method: 'POST', credentials: 'same-origin', headers: { 'Content-Type': 'application/json', Accept: 'application/pdf, text/plain' }, body: JSON.stringify({ rpt: rpt, title: null, rows: st.dtGrid }) })
            .then(function (r) { if (r.ok && (r.headers.get('Content-Type') || '').indexOf('application/pdf') >= 0) return r.blob().then(function (b) { var u = URL.createObjectURL(b); if (w) w.location.href = u; else window.open(u); }); return r.text().then(function (x) { if (w) w.close(); alert(x || ('Print failed (' + r.status + ')')); }); })
            .catch(function (e) { if (w) w.close(); alert(e.message); });
    }
    function btnPrint_Click() { printTable('910A_StockEvaluationCgsReport.rpt', 'Record Not Found For Display'); }

    /* btnNew_Click :872 */
    function btnNew_Click() {
        $id('txtFromDocNo').value = ''; $id('txtToDocNo').value = '';
        ['cmbItemclass', 'CmbParentCategory', 'cmbItemCatgory', 'cmbItemtype', 'cmbWareHouse', 'cmbSupplierCustomer', 'cmbItem'].forEach(function (id) { $id(id).value = ''; });
        $id('cmbActivity').value = '1'; cmbActivity_TextChanged();   // Rows[0].Activate()
        $id('cmbDateType').focus();
        Datetypefill();
    }
    /* btnRefresh_Click :907 */
    function btnRefresh_Click() { return SC.getJson(API + '/combos').then(AllComboBind).catch(function (e) { alert(e.message); }); }

    /* MakeShortCutKeys :1029 */
    function MakeShortCutKeys() {
        var rows = [['Ctrl+S', 'For Show'], ['Ctrl+E', 'For Close'], ['Ctrl+R', 'For Refresh'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For print'], ['Alt + 1', 'For print'],
            ['Ctrl+F5', 'For Focus On DateType '], ['Ctrl+alt', 'To Show ShortCut Keys Form'], ['Ctrl+ArrowDown', 'For Focus On Grid'], ['Ctrl+ArrowUp', 'For Focus On DateType in Filter'],
            ['Ctrl+Space', "When Focus On Any Grid To Call Function's On Button Or Link"]];
        $id('shortCutRows').innerHTML = rows.map(function (r) { return '<tr><td>' + esc(r[0]) + '</td><td>' + esc(r[1]) + '</td></tr>'; }).join('');
        $id('shortCutModal').classList.add('is-open');
    }

    function showClick() { return SC.withBusy($id('btnShow'), GridFill).catch(function (e) { alert(e.message); }); }
    function closeForm() { if (window.opener || history.length <= 1) window.close(); else history.back(); }

    /* InitializeComponentMethod :203 */
    function load() {
        var q = new URLSearchParams(location.search);
        SC.getJson(API + '/init').then(function (d) {
            st.init = d;
            $id('txtDateFrom').value = SC.today(); $id('txtToDate').value = SC.today();   // DateTimePicker default = now
            AllComboBind(d.combos || []);
            ActivityFill(); Datetypefill();
            if (activity() === '') { $id('cmbActivity').value = '1'; }   // Rows[0].Activate()
            cmbActivity_TextChanged();
            if (q.get('supplierCustomerId')) $id('cmbSupplierCustomer').value = q.get('supplierCustomerId');
            if (q.get('itemId')) $id('cmbItem').value = q.get('itemId');
            ReportsLoad(d.dynamicReports || []);
            if (num($id('cmbSupplierCustomer').value) > 0 || num($id('cmbItem').value) > 0) {   // :224 RequestFromAnotherForm
                if (q.get('fromDate')) $id('txtDateFrom').value = q.get('fromDate');
                if (q.get('toDate')) $id('txtToDate').value = q.get('toDate');
                GridFill().catch(function (e) { alert(e.message); });
            } else { $id('cmbDateType').value = '3'; cmbDateType_ValueChanged(); }   // Rows[2].Activate() = This Month
            $id('cmbDateType').focus();
            st.loaded = true;
        }).catch(function (e) { alert(e.message || 'Error occurred during database call.'); });
    }

    function onlyNumber(e) { if (e.data && /\D/.test(e.data)) e.target.value = e.target.value.replace(/\D/g, ''); }   // CommonServices.OnlytextNumberFunction :944

    document.addEventListener('DOMContentLoaded', function () {
        $id('btnShow').addEventListener('click', showClick);
        $id('btnNew').addEventListener('click', btnNew_Click);
        $id('btnRefresh').addEventListener('click', function () { SC.withBusy($id('btnRefresh'), btnRefresh_Click); });
        $id('btnPrint').addEventListener('click', btnPrint_Click);
        $id('btnShortcut').addEventListener('click', MakeShortCutKeys);
        $id('shortCutClose').addEventListener('click', function () { $id('shortCutModal').classList.remove('is-open'); });
        $id('shortCutModal').addEventListener('mousedown', function (e) { if (e.target === e.currentTarget) e.currentTarget.classList.remove('is-open'); });
        $id('tsDrowdownBtn').addEventListener('click', function () { $id('tsDrowdownList').classList.toggle('is-open'); });
        $id('tsDrowdownList').addEventListener('click', function (e) {
            var b = e.target.closest('button[data-rpt]'); if (!b) return;
            $id('tsDrowdownList').classList.remove('is-open');
            printTable(b.getAttribute('data-rpt') + '.rpt', 'Record not found for Display');   // :864
        });
        document.addEventListener('mousedown', function (e) { if (!e.target.closest('#tsDrowdown')) $id('tsDrowdownList').classList.remove('is-open'); });
        $id('cmbDateType').addEventListener('change', cmbDateType_ValueChanged);
        $id('cmbActivity').addEventListener('change', cmbActivity_TextChanged);
        $id('txtFromDocNo').addEventListener('input', onlyNumber); $id('txtToDocNo').addEventListener('input', onlyNumber);
        var grid = $id('DataGridHistory');
        grid.addEventListener('input', function (e) { var k = e.target.getAttribute('data-flt'); if (!k) return; st.filters[k] = e.target.value.trim().toLowerCase(); renderBody(); });
        grid.addEventListener('click', function (e) {
            var tr = e.target.closest('tbody tr[data-i]'); if (tr) { st.sel = +tr.getAttribute('data-i'); grid.querySelectorAll('tbody tr.sel').forEach(function (x) { x.classList.remove('sel'); }); tr.classList.add('sel'); }
        });
        /* frmEvaulationDetailSalesReports_KeyDown :964 (Alt+1 only fires when btnPrint.Text is "312-SalesRegisterSummary", which this form never sets) */
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.ctrlKey && e.altKey) { e.preventDefault(); MakeShortCutKeys(); return; }
            if (e.ctrlKey && k === 's') { e.preventDefault(); showClick(); }
            else if (e.ctrlKey && k === 'p') { e.preventDefault(); btnPrint_Click(); }
            else if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew_Click(); }
            else if (e.ctrlKey && k === 'r') { e.preventDefault(); btnRefresh_Click(); }
            else if (e.ctrlKey && (k === 'f5' || k === 'arrowup')) { e.preventDefault(); $id('cmbDateType').focus(); }
            else if (e.ctrlKey && k === 'arrowdown') { e.preventDefault(); grid.focus(); }
            else if ((e.ctrlKey && k === 'e') || k === 'escape') { e.preventDefault(); if ($id('shortCutModal').classList.contains('is-open')) $id('shortCutModal').classList.remove('is-open'); else closeForm(); }
        });
        load();
    });
})();
