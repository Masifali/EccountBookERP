/* ============================================================================================
 * countx_export_commercial_invoice_history.js - ExImCommercialInvoiceHistory.cs (no ScreenDefinition row).
 * Tab 0 "Invoices History": Sp_ExImInvoice_ReceivedAndOutstandingHistory_Rpt, print exp-o-522 (522-ExImInvoiceRegister.rpt).
 * Tab 1 "Export History Report": SpExImInvoice_ExportHistoryDetail_Report / SpExImInvoice_ExportsSummery_Reports, prints
 * exp-o-537 (537-ExportDetailHistoryReport.rpt) and exp-o-538-customer / -01-item / -02-port.
 * ?dateType=5|3 plays ExportDashboardHeaderInfo ("Total Export" / "TotalExportByCurrentMonth": cmbDateType.Value before Show).
 * ============================================================================================ */
(function (global) {
    'use strict';
    var H = global.ExportRptO, $id = H.$id, box = H.box;
    var API = '/api/export/commercial-invoice-history';
    var DATE_TYPES = [{ Id: 1, Parameters: 'This Day' }, { Id: 2, Parameters: 'This Week' }, { Id: 3, Parameters: 'This Month' },
        { Id: 4, Parameters: 'This Year' }, { Id: 5, Parameters: 'Financial Year' }];        /* CommonServices.DateType() - fixed */
    var SUMMARY = ['Summary By Customer', 'Summary By Item', 'Summary By Port'];
    var S = {
        yearStart: '', reportTypes: [], tabs: null,
        inv: [], invRaw: 0, invLast: null,                 /* dtHistory */
        detail: [], detailLast: null,                      /* dtdetail */
        sums: [], sumLast: null,                           /* dtSummaries */
        exportRows: [], combos: {}
    };

    /* ---------------------------------------------------------------- grids */
    /* GridSetting(): HeaderLines 2, centred headers, Id hidden, widths, three Σ columns "0,0" (total "#,##0.##") */
    var INV_COLS = [
        { key: 'Id', hidden: true }, { key: 'InvoiceNo', caption: 'InvoiceNo', width: 100, link: true }, { key: 'InvoiceDate', caption: 'InvoiceDate', width: 80, fmt: 'd' },
        { key: 'CustomerName', caption: 'CustomerName', width: 120 }, { key: 'ContractNos', caption: 'ContractNos', width: 100 },
        { key: 'ContractDates', caption: 'ContractDates', width: 90 }, { key: 'PaymentTerm', caption: 'PaymentTerm', width: 90 }, { key: 'FcyCode', caption: 'FcyCode', width: 60 },
        { key: 'InvoiceValue', caption: 'InvoiceValue', width: 100, num: true, sum: true, fmt: 'z2', tfmt: 'o2' },
        { key: 'FcyReceived', caption: 'FcyReceived', width: 100, num: true, sum: true, fmt: 'z2', tfmt: 'o2' },
        { key: 'FcyBalance', caption: 'FcyBalance', width: 100, num: true, sum: true, fmt: 'z2', tfmt: 'o2' }
    ];
    /* GridSettingForDetialHistory(): CustomGroupName hidden + grouped; the by-type column hidden + grouped with GroupTotals Always */
    var BY_TYPE = { 'Export Detail By Item': 'ItemName', 'Export Detail By Customer': 'CustomerName', 'Export Detail By Contract': 'ContractNos',
        'Export Detail By Item ItemType': 'ItemType', 'Export Detail By Item Category': 'ItemCategory' };
    function detailCols(text) {
        var hide = BY_TYPE[text] || '';
        var c = [
            { key: 'CustomGroupName', hidden: true }, { key: 'CustomerName', caption: 'CustomerName', width: 150 },
            { key: 'DocCode', caption: 'DocCode', width: 70 }, { key: 'DocDate', caption: 'DocDate', width: 80, fmt: 'd' },
            { key: 'InvoiceNo', caption: 'InvoiceNo' }, { key: 'ContractNos', caption: 'Contract Nos' }, { key: 'ContractDates', caption: 'Contract Dates' },
            { key: 'ItemCategory', caption: 'ItemCategory' }, { key: 'ItemType', caption: 'ItemType' }, { key: 'ItemName', caption: 'ItemName', width: 150 },
            { key: 'CropYear', caption: 'CropYear', width: 70 }, { key: 'PackingType', caption: 'PackingType' }, { key: 'FcyCode', caption: 'Currency', width: 70 },
            { key: 'RatePrice', caption: 'RatePrice', width: 80, num: true, avg: true, fmt: 'h2' },
            { key: 'NoOfPack', caption: 'Qty', width: 70, num: true, sum: true, fmt: 'h2' },
            { key: 'PackSize', caption: 'PackSize', width: 70, num: true },
            { key: 'NetWeight', caption: 'NetWeight', num: true, sum: true, fmt: 'h0' },
            { key: 'FcAmount', caption: 'FcAmount', num: true, sum: true, fmt: 'h0' },
            { key: 'M_Tons', caption: 'M_Tons', width: 70, num: true, sum: true, fmt: 'h0' },
            { key: 'FarmingType', caption: 'FarmingType' }, { key: 'TradeType', caption: 'TradeType' }
        ];
        c.forEach(function (x) { if (x.key === hide) x.hidden = true; });
        return c;
    }
    /* Summary By Customer / Item: GridSettingForSummaryHistory ("#,#"); Summary By Port: GridSettingsForSummaries ("#,#.##") */
    function summaryCols(text) {
        var port = text === 'Summary By Port', f = port ? 'h2' : 'h0';
        var cap = text === 'Summary By Customer' ? 'Customer' : text === 'Summary By Item' ? (port ? 'Item' : 'Item Name') : port ? 'Port' : 'Description';
        return [
            { key: 'Description', caption: cap, width: port ? 250 : undefined }, { key: 'FcyCode', caption: 'FcyCode', width: 70 },
            { key: 'M_Tons', caption: 'M_Tons', num: true, sum: true, fmt: f },
            { key: 'PKR_Amount', caption: 'PKR_Amount', num: true, sum: true, fmt: f },
            { key: 'FcyAmount', caption: 'FcyAmount', num: true, sum: true, fmt: f }
        ];
    }

    /* ---------------------------------------------------------------- combos */
    function bindIf(id, rows) { if ((rows || []).length) H.bind(id, rows, 'Id', 'name', true); }   /* "if (dt.Rows.Count > 0) BindDDL" */
    /** ComboFill(): Invoice / Customer / TradeType / FarmingType of tab 0. */
    function comboFill(g) { bindIf('cmbInvoiceNo', g.Invoice); bindIf('cmbSupplierName', g.Customer); bindIf('Cmbtradtype', g.TradeType); bindIf('CmbFarmingType', g.FarmingType); }
    /** ComboFillExportReport(): tab 1; DestinationPort binds only when the ItemCategory list has rows (the desktop tests dtItemCat). */
    function comboFillExport(g) {
        bindIf('CmbItemType', g.ItemType); bindIf('CmbItemName', g.Items); bindIf('CmbContractNo', g.ContractNo); bindIf('CmbCustomerName', g.Customer);
        bindIf('CmbItemCategory', g.ItemCategory);
        if ((g.ItemCategory || []).length) H.bind('CmbSeaPort', g.DestinationPort || [], 'Id', 'name', true);
        bindIf('CmbTradeTypeExport', g.TradeType); bindIf('CmbFarmingTypeExport', g.FarmingType);
    }
    /** ReportTypeBind(): option value = row index + 1 (ReportType repeats), Rows[0].Activate(). */
    function reportTypeBind(rows) {
        if (!(rows || []).length) return;
        S.reportTypes = rows;
        var s = $id('CmbReportType'), h = '<option value="0"></option>';
        rows.forEach(function (r, i) { h += '<option value="' + (i + 1) + '">' + H.esc(r.ReportName) + '</option>'; });
        s.innerHTML = h; s.value = '1'; H.refreshCombos();
    }
    function reportText() { return H.netI(H.val('CmbReportType')) ? H.selText('CmbReportType').trim() : ''; }
    function reportTypeValue() { var i = H.netI(H.val('CmbReportType')); return i ? String(S.reportTypes[i - 1].ReportType) : ''; }

    /* ---------------------------------------------------------------- tab 0 */
    function invFilters() {
        return { fromDate: H.val('txtdatefrom'), toDate: H.val('txtdateto'),
            invoiceNo: H.netI(H.val('cmbInvoiceNo')) ? H.selText('cmbInvoiceNo').trim() : '',   /* cmbInvoiceNo.Text.Trim() */
            customerId: H.netI(H.val('cmbSupplierName')), actionId: H.netI(H.radio('rbInvoices')),
            tradeTypeId: H.netI(H.val('Cmbtradtype')), farmingTypeId: H.netI(H.val('CmbFarmingType')) };
    }
    /** GridBindForInvoicesHistory(): rows -> grid; none -> ClearStructure. */
    function bindInvoices() {
        var f = invFilters();
        return H.postJson(API + '/invoices', f).then(function (rows) {
            S.inv = rows || []; S.invLast = f;
            if (S.inv.length) { H.drawGrid('grdfrm', INV_COLS, S.inv, {}); H.show('grdfrmEmpty', false); }
            else clearInvoicesGrid();
        }).catch(function (e) { box(e.message); });
    }
    function clearInvoicesGrid() { H.clearGrid('grdfrm'); $id('grdfrm').querySelector('thead').innerHTML = ''; H.show('grdfrmEmpty', true); }
    function dateTypeChanged() { H.dateTypeChanged('cmbDateType', 'txtdatefrom', 'txtdateto', S.yearStart); }
    /** ResetForInvoicesHistory(): Invoice No and Customer texts blank. */
    function resetInvoices() { H.setVal('cmbInvoiceNo', '0'); H.setVal('cmbSupplierName', '0'); }

    /* ---------------------------------------------------------------- tab 1 */
    function expFilters() {
        return { fromDate: H.val('datFromDate'), toDate: H.val('datToDate'), itemId: H.netI(H.val('CmbItemName')),
            customerId: H.netI(H.val('CmbCustomerName')), contractId: H.netI(H.val('CmbContractNo')), itemTypeId: H.netI(H.val('CmbItemType')),
            itemCategoryId: H.netI(H.val('CmbItemCategory')), portId: H.netI(H.val('CmbSeaPort')), activity: reportText(),
            tradeTypeId: H.netI(H.val('CmbTradeTypeExport')), farmingTypeId: H.netI(H.val('CmbFarmingTypeExport')), reportType: reportTypeValue() };
    }
    function sortBy(rows, keys) {
        return rows.map(function (r, i) { return { r: r, i: i }; }).sort(function (a, b) {
            for (var k = 0; k < keys.length; k++) {
                var x = String(a.r[keys[k]] == null ? '' : a.r[keys[k]]), y = String(b.r[keys[k]] == null ? '' : b.r[keys[k]]);
                if (x < y) return -1; if (x > y) return 1;
            }
            return a.i - b.i;
        }).map(function (o) { return o.r; });
    }
    /**
     * GridBindForExportHistoryReport(). Quirks kept: a detail text the form does not know (e.g. "" after New) leaves the grid
     * as it was; an EMPTY detail result clears grdfrm (the Invoices History grid), not this grid.
     */
    function bindExport() {
        var f = expFilters(), text = f.activity;
        return H.postJson(API + '/export-history', f).then(function (d) {
            d = d || {}; var rows = d.rows || [];
            if (d.mode === 'summary') {
                S.sums = rows; S.sumLast = f;
                if (rows.length) { S.exportRows = rows; H.drawGrid('GridForExportHistoryReport', summaryCols(text), rows, {}); H.show('gridExportEmpty', false); }
                else { S.exportRows = []; H.clearGrid('GridForExportHistoryReport'); $id('GridForExportHistoryReport').querySelector('thead').innerHTML = ''; H.show('gridExportEmpty', true); }
                return;
            }
            S.detail = rows; S.detailLast = f;
            if (!rows.length) { clearInvoicesGrid(); return; }
            if (text !== 'Export Detail' && !BY_TYPE[text]) return;
            var second = BY_TYPE[text];
            var groups = second ? ['CustomGroupName', second] : ['CustomGroupName'];
            S.exportRows = sortBy(rows, groups);
            H.drawGrid('GridForExportHistoryReport', detailCols(text), S.exportRows,
                { groups: groups, groupTotals: !!second, groupCaption: function (v, key) { return (key === 'CustomGroupName' ? 'CustomGroupName' : key) + ': ' + v; } });
            H.show('gridExportEmpty', false);
        }).catch(function (e) { box(e.message); });
    }
    /** CmbReportType_Leave: summary texts show the summary button, any other text the 537 button. */
    function reportTypeLeave() {
        var t = reportText(), sum = SUMMARY.indexOf(t) >= 0;
        H.show('btnSummaryReport538', sum); H.show('btnReport537', !sum);
    }
    /** PrintButtonManage(): with a report type the summary button is shown (whatever the type); its text follows the summary type. */
    function printButtonManage() {
        if (!H.netI(H.val('CmbReportType'))) { H.show('btnSummaryReport538', false); return; }
        H.show('btnSummaryReport538', true);
        var t = H.selText('CmbReportType');
        if (t === 'Summary By Customer') $id('btnSummaryReport538Text').textContent = '538-ExportSummaryByCustomer';
        else if (t === 'Summary By Item') $id('btnSummaryReport538Text').textContent = '538_01-ExportSummaryByItem';
        else if (t === 'Summary By Port') $id('btnSummaryReport538Text').textContent = '538_02-ExportSummaryByPort';
    }
    function peremeterChanged() {
        var v = H.netI(H.val('cmbperemeter'));
        if (v < 1 || v > 5) return;
        H.dateTypeChanged('cmbperemeter', 'datFromDate', 'datToDate', S.yearStart);
        return bindExport();
    }
    /** btnNewForExportHistoryReport_Click: seven texts blank (Report Type too), GridBindForExportHistoryReport. */
    function newExport() {
        H.focus('cmbperemeter');
        ['CmbItemCategory', 'CmbItemType', 'CmbItemName', 'CmbContractNo', 'CmbSeaPort', 'CmbCustomerName', 'CmbReportType'].forEach(function (id) { H.setVal(id, '0'); });
        return bindExport();
    }

    /* ---------------------------------------------------------------- load */
    function load() {
        var today = H.today();
        ['txtdatefrom', 'txtdateto', 'datFromDate', 'datToDate'].forEach(function (id) { H.setText(id, today); });
        H.bind('cmbDateType', DATE_TYPES, 'Id', 'Parameters', true);
        H.bind('cmbperemeter', DATE_TYPES, 'Id', 'Parameters', true);
        return H.getJson(API + '/setup').then(function (d) {
            d = d || {};
            S.yearStart = d.yearStart || '';
            (d.errors || []).forEach(function (m) { box(m); });
            S.combos = d.combos || {};
            comboFill(S.combos);
            comboFillExport(S.combos);
            reportTypeBind(d.reportTypes);
            H.show('btnReport537', true); H.show('btnSummaryReport538', false);
            /* opener: cmbDateType.Value = 5 / 3 before Show; otherwise Rows[1] ("This Week") */
            var q = /[?&]dateType=(\d)/.exec(location.search), dt = q ? H.netI(q[1]) : 0;
            H.setVal('cmbDateType', dt >= 1 && dt <= 5 ? String(dt) : '2'); dateTypeChanged();
            H.focus('cmbDateType');
            H.setVal('cmbperemeter', '2');
            return peremeterChanged();
        }).catch(function (e) { box(e.message); });
    }

    /* ---------------------------------------------------------------- buttons */
    function showInvoices(btn) { return H.busy(btn, bindInvoices); }
    /** btnnew_Click: ResetForInvoicesHistory, DateTypeFill (re-bound, nothing selected), focus Date Type. */
    function btnNew() {
        resetInvoices();
        H.bind('cmbDateType', DATE_TYPES, 'Id', 'Parameters', true); H.setVal('cmbDateType', '0');
        H.focus('cmbDateType');
    }
    function refreshInvoices(btn) {
        return H.busy(btn, function () { return H.getJson(API + '/combos').then(function (d) { d = d || {}; (d.errors || []).forEach(function (m) { box(m); }); comboFill(d.combos || {}); }); });
    }
    /** ShowRegister527(): "Not Record Found For Display" without dtHistory rows, else 522. */
    function print522(btn) {
        if (!S.inv.length || !S.invLast) { box('Not Record Found For Display'); return; }
        var f = S.invLast;
        return H.print('exp-o-522', { fromDate: f.fromDate, toDate: f.toDate, invoiceNo: f.invoiceNo, supplierCustomerId: f.customerId,
            actionId: f.actionId, tradeTypeId: f.tradeTypeId, farmingTypeId: f.farmingTypeId }, btn);
    }
    /** btnShowForExportHistoryReport_Click: "Please Select Activity First..." without a report type. */
    function showExport(btn) {
        if (!H.netI(H.val('CmbReportType'))) { box('Please Select Activity First...'); H.focus('CmbReportType'); return; }
        return H.busy(btn, function () { return bindExport().then(printButtonManage); });
    }
    function btnNewExport(btn) { return H.busy(btn, newExport); }
    /** btnRefreshExportHistory_Click: ParameterFill, ReportTypeBind, ComboFillExportReport. */
    function refreshExport(btn) {
        return H.busy(btn, function () {
            return H.getJson(API + '/combos?reportTypes=true').then(function (d) {
                d = d || {}; (d.errors || []).forEach(function (m) { box(m); });
                H.bind('cmbperemeter', DATE_TYPES, 'Id', 'Parameters', true);
                reportTypeBind(d.reportTypes); comboFillExport(d.combos || {});
            });
        });
    }
    /** btnReport537_Click: "Not Record Found For Display" without dtdetail rows, else 537 with the filters of that bind. */
    function print537(btn) {
        if (!S.detail.length || !S.detailLast) { box('Not Record Found For Display'); return; }
        var f = S.detailLast;
        return H.print('exp-o-537', { activity: f.activity, itemTypeId: f.itemTypeId, itemCategoryId: f.itemCategoryId, itemId: f.itemId,
            fromDate: f.fromDate, toDate: f.toDate, lcContractId: f.contractId, supplierCustomerId: f.customerId, destinationPortId: f.portId,
            tradeTypeId: f.tradeTypeId, farmingTypeId: f.farmingTypeId }, btn);
    }
    /** btnSummaryReport538_Click: by the button text; "Record Not Found For Display" without dtSummaries rows. */
    function print538(btn) {
        var t = $id('btnSummaryReport538Text').textContent, key = t === '538-ExportSummaryByCustomer' ? 'exp-o-538-customer'
            : t === '538_01-ExportSummaryByItem' ? 'exp-o-538-01-item' : t === '538_02-ExportSummaryByPort' ? 'exp-o-538-02-port' : '';
        if (!key) return;                                   /* "SummaryReport": no branch matches, nothing happens */
        if (!S.sums.length || !S.sumLast) { box('Record Not Found For Display'); return; }
        var f = S.sumLast;
        return H.print(key, { supplierCustomerId: f.customerId, destinationPortId: f.portId, itemId: f.itemId, itemTypeId: f.itemTypeId,
            itemCategoryId: f.itemCategoryId, lcOrderId: f.contractId, fromDate: f.fromDate, toDate: f.toDate, reportType: f.reportType,
            tradeTypeId: f.tradeTypeId, farmingTypeId: f.farmingTypeId }, btn);
    }
    function shortcuts() {
        H.showShortcuts([['Ctrl+S', 'For show data'], ['Ctrl+E', 'For Close'], ['Ctrl+N', 'For New'], ['Ctrl+P', 'For Print 522'], ['Alt+1', 'For Print 537'],
            ['Alt+2', 'For Print Summary Register'], ['Ctrl+F5', 'For Focus on DateType'], ['Ctrl+alt', 'To Show ShortCut Keys Form'],
            ['Ctrl+ArrowUp', 'For Focus On Date Type'], ['Ctrl+ArrowDown', 'For Focus On Grid']]);
    }
    function history(btn) { return S.tabs && S.tabs.index() === 1 ? showExport(btn) : showInvoices(btn); }

    document.addEventListener('DOMContentLoaded', function () {
        H.fullscreenButtons();
        /* tabControl1_SelectedIndexChanged: 0 -> ResetForInvoicesHistory, 1 -> btnNewForExportHistoryReport_Click */
        S.tabs = H.tabs('tabControl1', function (i) { if (i === 0) resetInvoices(); else newExport(); });
        $id('cmbDateType').addEventListener('change', dateTypeChanged);
        $id('cmbperemeter').addEventListener('change', peremeterChanged);
        $id('CmbReportType').addEventListener('change', reportTypeLeave);
        /* btnShow_Leave / btnShowForExportHistoryReport_Leave */
        $id('btnShow').addEventListener('blur', function (e) { if (e.relatedTarget && e.relatedTarget.id === 'grdfrm') H.focus('cmbDateType'); });
        $id('btnShowForExportHistoryReport').addEventListener('blur', function (e) { if (e.relatedTarget && e.relatedTarget.closest && e.relatedTarget.closest('#tabControl1')) H.focus('cmbperemeter'); });
        H.gridEvents('grdfrm', {
            /* grdfrm_LinkClicked is empty on the desktop; the invoice number opens the commercial invoice here */
            link: function (key, i) { var r = S.inv[i]; if (r && key === 'InvoiceNo' && H.netI(r.Id) > 0) global.open('/export/commercial-invoice?id=' + H.netI(r.Id), '_blank'); }
        });
        H.gridEvents('GridForExportHistoryReport', {});
        document.addEventListener('keydown', function (e) {
            var k = (e.key || '').toLowerCase();
            if (e.key === 'Enter' || e.key === 'Escape' || (e.ctrlKey && k === 'e') || (e.ctrlKey && e.altKey)) { if (H.baseKeys(e, shortcuts)) return; }
            if (S.tabs.index() === 0) {
                if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNew(); }
                else if (e.ctrlKey && k === 'r') { e.preventDefault(); refreshInvoices($id('btnRefreshInvoicesHistory')); }
                else if (e.ctrlKey && k === 's') { e.preventDefault(); showInvoices($id('btnShow')); }
                else if (e.ctrlKey && k === 'p') { e.preventDefault(); print522($id('toolStripButton2')); }
                else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); H.focus('cmbDateType'); }
                else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('grdfrm'); }
            } else {
                if (e.ctrlKey && k === 'n') { e.preventDefault(); btnNewExport($id('btnNewForExportHistoryReport')); }
                else if (e.ctrlKey && k === 'r') { e.preventDefault(); refreshExport($id('btnRefreshExportHistory')); }
                else if (e.ctrlKey && k === 's') { e.preventDefault(); showExport($id('btnShowForExportHistoryReport')); }
                else if (e.altKey && e.key === '1') { e.preventDefault(); print537($id('btnReport537')); }
                else if (e.altKey && e.key === '2') { e.preventDefault(); print538($id('btnSummaryReport538')); }
                else if (e.ctrlKey && (e.key === 'F5' || e.key === 'ArrowUp')) { e.preventDefault(); H.focus('cmbperemeter'); }
                else if (e.ctrlKey && e.key === 'ArrowDown') { e.preventDefault(); H.focusGrid('GridForExportHistoryReport'); }
            }
        });
        load();
    });
    global.ExportCih = { btnNew: btnNew, refreshInvoices: refreshInvoices, print522: print522, showInvoices: showInvoices, shortcuts: shortcuts,
        btnNewExport: btnNewExport, refreshExport: refreshExport, print537: print537, print538: print538, showExport: showExport, history: history };
}(window));
